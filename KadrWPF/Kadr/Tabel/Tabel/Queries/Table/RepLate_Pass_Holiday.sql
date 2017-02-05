WITH TP_PER AS (    
    SELECT * FROM
        (SELECT V.PER_NUM, TRANSFER_ID, SUBDIV_ID, POS_ID, WORKER_ID, POS_CHIEF_OR_DEPUTY_SIGN, CODE_SUBDIV, DATE_PASS
        FROM (
            SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
                DECODE(sign_cur_work,1,DATE '3000-01-01', 
                    DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                        TRUNC((SELECT MIN (date_transfer) FROM APSTAFF.transfer
                               WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
               DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
               WORKER_ID, (select NVL(P.POS_CHIEF_OR_DEPUTY_SIGN,0) from APSTAFF.POSITION P where P.POS_ID = T.POS_ID) POS_CHIEF_OR_DEPUTY_SIGN, 
               (select S.CODE_SUBDIV from APSTAFF.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV
            FROM APSTAFF.transfer t    
            WHERE T.DATE_HIRE <= :p_DATE_BEGIN and T.HIRE_SIGN = 1 and SIGN_COMB = 0
            START WITH sign_cur_work = 1 OR type_transfer_id = 3
            CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        JOIN (select PER_NUM, MAX(EVENT_TIME) MAX_EVENT_TIME, TRUNC(EVENT_TIME) DATE_PASS
                from PERCO.EMP_PASS_EVENT 
                where PER_NUM IS NOT NULL and EVENT_TIME between :p_DATE_BEGIN and :p_DATE_END
                    /*and (WHERE_INTO = 1 or (WHERE_INTO = 5 and WHERE_FROM = 3))*/ 
                    and not ((where_into=6 and where_from=1) or (where_into=6 and where_from=6))
                    and (select C.TYPE_DAY_ID from APSTAFF.CALENDAR C WHERE C.CALENDAR_DAY = TRUNC(EVENT_TIME)) IN (1,4) 
                group by PER_NUM, TRUNC(EVENT_TIME)) VE
            ON V.PER_NUM = VE.PER_NUM
        WHERE CODE_SUBDIV not in ('058', '106') and --CODE_SUBDIV in ('016','133') and
            DATE_TRANSFER <= :p_DATE_END and END_TRANSFER >= :p_DATE_BEGIN
             and DATE_PASS BETWEEN DATE_TRANSFER AND END_TRANSFER)
    WHERE TRANSFER_ID not in (
            select TRANSFER_ID
            from (select E.gr_work_id,transfer_id, TRUNC(gr_work_date_begin) GR_WORK_DATE_BEGIN,
                    LEAD(TRUNC(gr_work_date_begin)-1/86400,1,date'3000-01-01') OVER (PARTITION by TRANSFER_ID ORDER BY gr_work_date_begin) GR_WORK_DATE_END 
                from APSTAFF.EMP_GR_WORK e 
                WHERE E.GR_WORK_ID in 
                    (select GR.GR_WORK_ID from APSTAFF.GR_WORK GR WHERE GR.SIGN_SUMMARIZE = 1)) V
            where GR_WORK_DATE_BEGIN<=:p_DATE_END and GR_WORK_DATE_END >= :p_DATE_BEGIN)),
    EMP_ORDER AS (
        select T.WORKER_ID, OOH.SUBDIV_ID, DFO.DATE_WORK_ORDER  
        from APSTAFF.ORDER_ON_HOLIDAY OOH
        join APSTAFF.DATE_FOR_ORDER DFO using(ORDER_ON_HOLIDAY_ID)
        join APSTAFF.EMP_FOR_ORDER EFO using(DATE_FOR_ORDER_ID)
        join APSTAFF.TRANSFER T using(TRANSFER_ID)
        where  DFO.DATE_WORK_ORDER between :p_DATE_BEGIN and :p_DATE_END and
            (select C.TYPE_DAY_ID from APSTAFF.CALENDAR C WHERE C.CALENDAR_DAY = DFO.DATE_WORK_ORDER) IN (1,4))
select CODE_SUBDIV, 
    VG.PER_NUM, 
    (select E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' 
    FROM APSTAFF.EMP E WHERE E.PER_NUM = VG.PER_NUM) FIO,
    (select P.POS_NAME from APSTAFF.POSITION P where P.POS_ID = VG.POS_ID) POS_NAME,
    ALL_TIME, GR_WORK_TIME,
    CASE WHEN EXISTS(select null from APSTAFF.PERMIT PT
        join APSTAFF.TYPE_PERMIT using(TYPE_PERMIT_ID)
        where PT.PER_NUM = VG.PER_NUM and DATE_START_PERMIT <= DATE_PASS and DATE_END_PERMIT >= DATE_PASS
           and CODE_TYPE_PERMIT = '10' and PT.TRANSFER_ID in
                (select T1.TRANSFER_ID from APSTAFF.TRANSFER T1 WHERE T1.WORKER_ID = VG.WORKER_ID) 
        ) THEN 'X' END SIGN_PERMIT_CHIEF,
    CASE WHEN /*POS_CHIEF_OR_DEPUTY_SIGN = 0 AND */
            EXISTS(select null from EMP_ORDER where SUBDIV_ID = VG.SUBDIV_ID and WORKER_ID = VG.WORKER_ID)             
        THEN 'X' END SIGN_ORDER,
    (SELECT COUNT(WORKER_ID) FROM EMP_ORDER WHERE SUBDIV_ID = VG.SUBDIV_ID and DATE_WORK_ORDER=DATE_PASS) COUNT_EMP_ORDER,
    (SELECT COUNT(WORKER_ID) FROM EMP_ORDER WHERE SUBDIV_ID = VG.SUBDIV_ID) COUNT_EMP_ORDER_SUB,
    DECODE(POS_CHIEF_OR_DEPUTY_SIGN,1,'X') SIGN_CHIEF,
    /*NVL((SELECT 'X' FROM APSTAFF.TRANSFER_OVERTIME TRO WHERE TRO.TRANSFER_ID = VG.TRANSFER_ID),'')*/ '' SIGN_LIST ,
    DATE_PASS WORK_DATE   
from
    (select distinct CODE_SUBDIV, TP.PER_NUM, W_DATE, TP.TRANSFER_ID, TP.SUBDIV_ID, TP.POS_ID, WORKER_ID, POS_CHIEF_OR_DEPUTY_SIGN,
        nvl(ROUND(ALL_TIME/3600,2),0) ALL_TIME, nvl(ROUND(GR_WORK_TIME/3600,2),0) GR_WORK_TIME, DATE_PASS
    from 
        (select CODE_SUBDIV, PER_NUM, TRANSFER_ID, SUBDIV_ID, POS_ID, WORKER_ID, POS_CHIEF_OR_DEPUTY_SIGN, DATE_PASS FROM TP_PER) TP, 
        TABLE(APSTAFF.GET_NEW_WORK_TIME(to_char(:p_DATE_BEGIN-1, 'dd.mm.yyyy'), to_char(:p_DATE_END+1, 'dd.mm.yyyy'), TP.per_num, TP.transfer_id))
    where W_DATE between :p_DATE_BEGIN and :p_DATE_END and W_DATE = DATE_PASS and GR_WORK_TIME < 5400) VG
ORDER BY CODE_SUBDIV, DATE_PASS, PER_NUM