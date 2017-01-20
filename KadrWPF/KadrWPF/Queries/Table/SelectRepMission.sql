select rownum, decode(GROUPING(V2.FIO),0, V2.FIO, 'Итого'), 
    sum(V2.D622) D622, round(sum(V2.D622)/sum(V2.AllDW)*100,2) D622Pr, 
    sum(V2.D222), round(sum(V2.D222)/sum(V2.AllDW)*100,2) D222Pr,
    sum(V2.All222), round(sum(V2.All222)/sum(V2.AllDW)*100,2) All222Pr, 
    sum(V2.D102), round(sum(V2.D102)/sum(V2.AllDW)*100,2) D102Pr,
    sum(V2.AllWork), round(sum(V2.AllWork)/sum(V2.AllDW)*100,2) AllWorkPr, 
    sum(V2.D541), round(sum(V2.D541)/sum(V2.AllDW)*100,2) D541Pr,
    sum(V2.AllDW), round(sum(V2.AllWork)/sum(V2.AllDW)*100+sum(V2.D541)/sum(V2.AllDW)*100,2) Itogo
from 
    (select  
        E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' FIO, 
        sum(decode(V1.PT, 622, 1, 0)) D622, sum(decode(V1.PT, 222, 1, 0)) D222,
        sum(decode(V1.PT, 622, 1, 222, 1, 0)) All222,
        sum(decode(V1.PT, 102, 1, 0)) D102, 
        sum(decode(V1.PT, 622, 1, 222, 1, 102, 1, 0)) AllWork,
        sum(decode(V1.PT, 541, 1, 0)) D541,
        sum(V1.OneDW) AllDW
    from 
        (select V0.*, 1 OneDW from (
            with Tr_per as (
            (select V.TRANSFER_ID, V.PER_NUM, V.DATE_TRANSFER, V.END_TRANSFER from (
            SELECT transfer_id,per_num,degree_id,subdiv_id,trunc(date_transfer) date_transfer,
                LEAD(trunc(date_transfer),1, DECODE(type_transfer_id,3,TRUNC(date_transfer)+1,date'3000-01-01')) 
                OVER (PARTITION BY PER_NUM, SIGN_COMB ORDER BY date_transfer)-1/86400 end_transfer    
            FROM {0}.TRANSFER T
            START WITH T.SIGN_CUR_WORK = 1 or T.TYPE_TRANSFER_ID = 3
            CONNECT BY NOCYCLE PRIOR T.FROM_POSITION = T.TRANSFER_ID) V
            WHERE V.SUBDIV_ID = :p_subdiv_id and V.DEGREE_ID = :p_degree_id 
                and V.END_TRANSFER >= :p_date_begin and V.DATE_TRANSFER <= :p_date_end))
            select W.PER_NUM, W.WORK_DATE, 
                decode(WP.PAY_TYPE_ID, 
                302, 102, 
                535, 102, 
                102, 102, 
                222, 222, 
                622, 622, 541) PT 
            from {0}.WORKED_DAY W
            join (select C.CALENDAR_DAY from {0}.CALENDAR C 
                    where C.CALENDAR_DAY between :p_date_begin and :p_date_end and C.TYPE_DAY_ID in (2,3)) CL
                on W.WORK_DATE = CL.CALENDAR_DAY
            JOIN (select * from Tr_per) tr on (W.TRANSFER_ID in (select TRANSFER_ID from Tr_per) and 
                W.work_date between TR.Date_transfer and TR.END_TRANSFER)
            join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
            where W.work_date between :p_date_begin and :p_date_end and 
                ((WP.PAY_TYPE_ID in (102, 302, 535, 222, 622) and WP.VALID_TIME > 0)
                or WP.PAY_TYPE_ID in (
                    select D.PAY_TYPE_ID from {0}.DOC_LIST D 
                    where D.SIGN_ALL_DAY = 1 and D.PAY_TYPE_ID not in (102, 510, 529, 222, 622))                     
                or WP.PAY_TYPE_ID in (210, 529, 531, 539, 543) and WP.VALID_TIME = W.FROM_GRAPH - 
                    nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                            where WP.WORKED_DAY_ID = WP3.WORKED_DAY_ID and WP3.PAY_TYPE_ID = 114),0)
                )
            ) V0
        GROUP BY V0.PER_NUM, V0.WORK_DATE, V0.PT) V1
    JOIN {0}.EMP E on V1.PER_NUM = E.PER_NUM
    GROUP BY V1.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME
    ORDER BY E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME) V2
group by rollup ((rownum, V2.FIO))