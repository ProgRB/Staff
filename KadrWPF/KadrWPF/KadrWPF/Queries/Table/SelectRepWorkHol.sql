SELECT GROUPING(V.PER_NUM) GR1, V.NGM, V.CD, V.PER_NUM, 
    DECODE(GROUPING(V.NGM)+GROUPING(V.PER_NUM), 
    0, E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.',
    1, 'ÈÒÎÃÎ ïî ãðóïïå ìàñòåðà',
    2, 'ÂÑÅÃÎ') FIO,
    V.INTO_PLANT, V.FROM_PLANT, SUM(V.PTIME) as PTIME
FROM (    
    WITH tp_per as
    (   select V.TRANSFER_ID, V.DEGREE_ID from 
        (SELECT        
          subdiv_id,
          transfer_id, 
          degree_id
        FROM {0}.transfer t    
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        WHERE V.SUBDIV_ID = :p_subdiv_id)
    select GM.NGM, D.CODE_DEGREE CD, P2.PER_NUM, 
        P2.INTO_PLANT, P2.FROM_PLANT, round((P2.FROM_PLANT - P2.INTO_PLANT)*24,2) as PTIME
    from (with CAL as ((select C.CALENDAR_DAY CD from {0}.CALENDAR C 
                        where C.CALENDAR_DAY between :p_date_begin and :p_date_end and C.TYPE_DAY_ID in (1,4)))
            select distinct P1.PER_NUM, P1.TRANSFER_ID, 
                case when EP.WHERE_INTO = 2 and EP.WHERE_FROM = 1
                    then EP.EVENT_TIME 
                    else nvl((select max(EP1.EVENT_TIME) from {1}.EMP_PASS_EVENT EP1 
                            join CAL C1 on C1.CD = trunc(EP1.EVENT_TIME) 
                            where EP1.per_num = EP.PER_NUM and EP1.EVENT_TIME < EP.EVENT_TIME  and 
                                (EP1.EVENT_TIME - EP.EVENT_TIME) < 2 and EP1.where_into = 2 and EP1.WHERE_FROM = 1), 
                            trunc(EP.EVENT_TIME))
                end as INTO_PLANT,
                case when EP.WHERE_INTO = 1 and EP.WHERE_FROM = 2
                    then EP.EVENT_TIME 
                    else nvl((select min(EP1.EVENT_TIME) from {1}.EMP_PASS_EVENT EP1 
                            join CAL C1 on C1.CD = trunc(EP1.EVENT_TIME) 
                            where EP1.per_num = EP.PER_NUM and EP1.event_time > EP.EVENT_TIME and 
                                (EP1.EVENT_TIME - EP.EVENT_TIME) < 2  and EP1.where_into = 1 and EP1.WHERE_FROM = 2),
                            trunc(EP.EVENT_TIME)+1)
                end as FROM_PLANT    
            from (SELECT TR.TRANSFER_ID, TR.PER_NUM, TR.SIGN_COMB 
                    FROM {0}.PN_TMP PN JOIN {0}.TRANSFER TR on PN.TRANSFER_ID = TR.TRANSFER_ID
                    WHERE PN.USER_NAME = :p_user_name) P1
            join {1}.EMP_PASS_EVENT EP on EP.PER_NUM = P1.PER_NUM
            JOIN CAL C1 on C1.CD = TRUNC(EP.EVENT_TIME)) P2
    left join tp_per tp on (tp.transfer_id=P2.TRANSFER_ID)
    join {0}.DEGREE D on tp.DEGREE_ID = D.DEGREE_ID
    left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                    LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
            from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
        GM on (gm.transfer_id=tp.transfer_id and :p_date_end between begin_group and end_group)   
    where P2.FROM_PLANT - P2.INTO_PLANT > (60/3600)
) V
JOIN {0}.EMP E ON E.PER_NUM = V.PER_NUM
GROUP BY ROLLUP(V.NGM, (V.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME, V.CD, V.FROM_PLANT,
    V.INTO_PLANT, V.PTIME))
ORDER BY V.NGM, V.CD, V.PER_NUM, V.INTO_PLANT