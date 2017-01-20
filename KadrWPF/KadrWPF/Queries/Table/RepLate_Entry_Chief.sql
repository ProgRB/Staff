select CODE_SUBDIV, E.PER_NUM, 
    E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME,
    TRUNC(BEGIN_PERIOD) WORK_DATE, round((END_PERIOD-BEGIN_PERIOD)*24,2) VTIME,
    COUNT(BEGIN_PERIOD) OVER(PARTITION BY CODE_SUBDIV, E.PER_NUM) COUNT_DOC_NOTE,
    DENSE_RANK() OVER(PARTITION BY CODE_SUBDIV ORDER BY E.PER_NUM) RN
from (    
    select CODE_SUBDIV, V.PER_NUM, V.TRANSFER_ID, 
        TRUNC(DECODE(FL_INPUT,1,TIME_BEGIN,EVENT_TIME),'MI') BEGIN_PERIODDOC,
        DECODE(FL_INPUT,1,TIME_BEGIN,EVENT_TIME) BEGIN_PERIOD,
        TRUNC(DECODE(FL_INPUT,1,EVENT_TIME,TIME_END),'MI') END_PERIOD,
        FL_INPUT, EVENT_TIME 
    from 
    (
        select CODE_SUBDIV, PER_NUM, TRANSFER_ID, TIME_BEGIN, TIME_END,
            MIN(TIME_BEGIN) OVER(PARTITION BY TRANSFER_ID,DATE_WORK) TIME_BEGIN_DATE, 
            MAX(TIME_END) OVER(PARTITION BY TRANSFER_ID,DATE_WORK) TIME_END_DATE
        from
        (   select PN.PER_NUM, CODE_SUBDIV,  PN.TRANSFER_ID, GR.TIME_BEGIN, GR.TIME_END, TRUNC(GR.TIME_BEGIN) DATE_WORK  
            from (WITH TRANS as 
                    (
                    select T.SUBDIV_ID, S.CODE_SUBDIV, T.TRANSFER_ID, T.PER_NUM, TRUNC(T.DATE_TRANSFER) DATE_TRANSFER,  
                        DECODE(T.TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+86399/86400,
                            LEAD(TRUNC(DATE_TRANSFER) - 1/86400,1,DATE '3000-01-01') 
                            OVER(PARTITION BY WORKER_ID ORDER BY DATE_TRANSFER)) END_TRANSFER,
                        T.WORKER_ID, POS_ID
                    from {0}.TRANSFER T
                    join {0}.SUBDIV S on (T.SUBDIV_ID = S.SUBDIV_ID)
                    )
                select * from TRANS T
                join {0}.POSITION P on P.POS_ID = T.POS_ID
                where DATE_TRANSFER <= :p_END_PERIOD and END_TRANSFER >= :p_BEGIN_PERIOD 
                    and POS_CHIEF_OR_DEPUTY_SIGN = 1
                ) PN
            join TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_BEGIN_PERIOD,'DD.MM.YYYY'),
                    to_char(:p_END_PERIOD,'DD.MM.YYYY'), PN.PER_NUM, PN.TRANSFER_ID)) GR
                on PN.PER_NUM = GR.PER_NUM 
        ) V0
    ) V   
    join (select * from (
                select PER_NUM, EVENT_TIME, TRUNC(EVENT_TIME) EVENT_DAY, DECODE(WHERE_INTO,1,0,2,1) FL_INPUT, 
                    DECODE(WHERE_INTO, LEAD(WHERE_INTO, 1, 0) OVER (PARTITION BY PER_NUM ORDER BY EVENT_TIME),1,0) ERROR_EVENT 
                from PERCO.EMP_PASS_EVENT where event_time between :p_BEGIN_PERIOD and :p_END_PERIOD 
                    and where_into in (2,3) and where_from in (1))
            where ERROR_EVENT = 0) EP
        on V.PER_NUM = EP.PER_NUM and EP.EVENT_TIME between V.TIME_BEGIN and V.TIME_END-1/86400
        ) V1
join {0}.EMP E on V1.PER_NUM = E.PER_NUM
ORDER BY CODE_SUBDIV, PER_NUM