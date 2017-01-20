WITH EMP_PASS as 
    (select * from (
        select PER_NUM, EVENT_TIME, TRUNC(EVENT_TIME) EVENT_DAY, DECODE(WHERE_INTO,1,0,2,1) FL_INPUT, 
            DECODE(WHERE_INTO, LEAD(WHERE_INTO, 1, 0) OVER (PARTITION BY PER_NUM ORDER BY EVENT_TIME),1,0) ERROR_EVENT 
        from PERCO.EMP_PASS_EVENT where event_time between :p_date_begin and :p_date_end 
            and where_into in (1, 2) and where_from in (1, 2))
    where ERROR_EVENT = 0)
select (select (select SUB.CODE_SUBDIV from {0}.SUBDIV SUB where SUB.SUBDIV_ID = T1.SUBDIV_ID) 
     from {0}.TRANSFER T1 where T1.TRANSFER_ID = V1.TRANSFER_ID) CODE_SUBDIV,  
    V1.EVENT_TIME, E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
     (select (select POS.POS_NAME from {0}.POSITION POS where POS.POS_ID = T1.POS_ID) 
     from {0}.TRANSFER T1 where T1.TRANSFER_ID = V1.TRANSFER_ID) POS_NAME, 
     round((END_PERIOD-BEGIN_PERIOD)*24,2) TIME_OUT,
     /*V1.PER_NUM, V1.BEGIN_PERIOD, V1.END_PERIOD,*/
     (select DISTINCT LISTAGG(D.DOC_NAME,'; ') WITHIN GROUP(ORDER BY 1) OVER() 
     from {0}.REG_DOC R join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
     where D.DOC_TYPE = 1 and R.PER_NUM = V1.PER_NUM and R.TRANSFER_ID in 
        (select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = V1.TRANSFER_ID
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
        and R.DOC_BEGIN between V1.BEGIN_PERIODDOC and V1.END_PERIOD) DOC_NAME
from (    
    select V.PER_NUM, V.TRANSFER_ID, 
        TRUNC(DECODE(FL_INPUT,1,TIME_BEGIN,EVENT_TIME),'MI') BEGIN_PERIODDOC,
        DECODE(FL_INPUT,1,TIME_BEGIN,EVENT_TIME) BEGIN_PERIOD,
        TRUNC(DECODE(FL_INPUT,1,EVENT_TIME,TIME_END),'MI') END_PERIOD,
        FL_INPUT, EVENT_TIME 
    from 
    (
        select PER_NUM, TRANSFER_ID, TIME_BEGIN, TIME_END,
            MIN(TIME_BEGIN) OVER(PARTITION BY TRANSFER_ID,DATE_WORK) TIME_BEGIN_DATE, 
            MAX(TIME_END) OVER(PARTITION BY TRANSFER_ID,DATE_WORK) TIME_END_DATE,
            nvl((SELECT max(TP.FREE_EXIT) FROM {0}.PERMIT P
                join {0}.TYPE_PERMIT TP using (TYPE_PERMIT_ID)
                where P.PER_NUM = V0.PER_NUM and V0.DATE_WORK between P.DATE_START_PERMIT and P.DATE_END_PERMIT 
                    and P.TRANSFER_ID in (
                    select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = V0.TRANSFER_ID
                    connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)),0) FREE_EXIT
        from
        (   select PN.PNUM PER_NUM, PN.TRANSFER_ID,
                CASE WHEN TO_CHAR(GR.TIME_BEGIN,'HH24:MI') in ('07:30','07:45') 
                    THEN GR.TIME_BEGIN + nvl((SELECT max(TP.DISPLACE_GRAPH)/1440 FROM {0}.PERMIT P
                            join {0}.TYPE_PERMIT TP using (TYPE_PERMIT_ID)
                            where P.PER_NUM = PN.PNUM and GR.TIME_BEGIN between P.DATE_START_PERMIT and P.DATE_END_PERMIT 
                                and P.TRANSFER_ID in (
                                select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = PN.TRANSFER_ID
                                connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)),0)
                    ELSE GR.TIME_BEGIN 
                END TIME_BEGIN, 
                GR.TIME_END,
                TRUNC(GR.TIME_BEGIN) DATE_WORK  
            from (select PN0.PNUM, PN0.TRANSFER_ID, PN0.USER_NAME,
                    DECODE(T.TYPE_TRANSFER_ID, 3, TRUNC(T.DATE_TRANSFER),
                        LEAST(NVL((select TRUNC(T2.DATE_TRANSFER)-1 from {0}.TRANSFER T2 where T2.FROM_POSITION = T.TRANSFER_ID
                                and T2.SUBDIV_ID != T.SUBDIV_ID), :p_date_end),:p_date_end)) DATE_END
                from {0}.PN_TMP PN0 
                join {0}.TRANSFER T on (T.TRANSFER_ID = PN0.TRANSFER_ID)
                where PN0.USER_NAME = :p_user_name) PN    
            join TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date_begin,'DD.MM.YYYY'),to_char(PN.DATE_END,'DD.MM.YYYY'),
                PN.PNUM, PN.TRANSFER_ID)) GR on PN.PNUM = GR.PER_NUM
            where not exists(
                select null from {0}.PERMIT P
                join {0}.TYPE_PERMIT TP using (TYPE_PERMIT_ID)
                where P.PER_NUM = PN.PNUM and GR.TIME_BEGIN between P.DATE_START_PERMIT and P.DATE_END_PERMIT 
                    and P.TRANSFER_ID in (
                        select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = PN.TRANSFER_ID
                        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
                    and (/*TP.NOT_REGISTR_PASS = 1 or*/TP.CODE_TYPE_PERMIT = 12 or TP.ROUND_TIME = 1)
                    )
        ) V0
    ) V   
    join (select * from EMP_PASS) EP
        on V.PER_NUM = EP.PER_NUM and EP.EVENT_TIME between V.TIME_BEGIN and V.TIME_END-1/86400
    /* Если это вход, то смотрим чтобы не было выхода перед этим. Если это выход, смотрим не было ли входа после.*/
    where 
        (EP.FL_INPUT = 1 and 
            ((FREE_EXIT=0 and not exists(
                select null from EMP_PASS E1 where E1.PER_NUM = V.PER_NUM 
                    and E1.EVENT_TIME between V.TIME_BEGIN and V.TIME_END and E1.EVENT_TIME < EP.EVENT_TIME  
                    and E1.FL_INPUT != EP.FL_INPUT)) 
            or 
            (FREE_EXIT=1 and not exists(
                select null from EMP_PASS E1 where E1.PER_NUM = V.PER_NUM 
                    and E1.EVENT_TIME between V.TIME_BEGIN_DATE and V.TIME_END_DATE and E1.EVENT_TIME < EP.EVENT_TIME  
                    and E1.FL_INPUT != EP.FL_INPUT)))) 
        or 
        (EP.FL_INPUT = 0 and 
            ((FREE_EXIT=0 and not exists(
                select null from EMP_PASS E1 where E1.PER_NUM = V.PER_NUM 
                    and E1.EVENT_TIME between V.TIME_BEGIN and V.TIME_END and E1.EVENT_TIME > EP.EVENT_TIME  
                    and E1.FL_INPUT != EP.FL_INPUT)) 
            or
            (FREE_EXIT=1 and not exists(
                select null from EMP_PASS E1 where E1.PER_NUM = V.PER_NUM 
                    and E1.EVENT_TIME between V.TIME_BEGIN_DATE and V.TIME_END_DATE and E1.EVENT_TIME > EP.EVENT_TIME  
                    and E1.FL_INPUT != EP.FL_INPUT))))) V1
join {0}.EMP E on V1.PER_NUM = E.PER_NUM
where ROUND((END_PERIOD-BEGIN_PERIOD)*24,2) between 0.01 and 0.51
    and not exists((select null from {0}.REG_DOC R join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
                 where D.SIGN_ALL_DAY = 1 and R.PER_NUM = V1.PER_NUM and R.TRANSFER_ID in 
                    (select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = V1.TRANSFER_ID
                    connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
                    and V1.EVENT_TIME between R.DOC_BEGIN and R.DOC_END))
order by CODE_SUBDIV, E.EMP_LAST_NAME,E.EMP_FIRST_NAME,E.EMP_MIDDLE_NAME, V1.BEGIN_PERIOD