SELECT GROUPING(V.PER_NUM) GR1, V.PER_NUM, 
    DECODE(GROUPING(V.NGM)+GROUPING(V.PER_NUM), 
    0, E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.',
    1, 'ÈÒÎÃÎ ïî ãðóïïå ìàñòåðà',
    2, 'ÂÑÅÃÎ') FIO,
    V.NGM, DECODE(V.SIGN_COMB, 1, 'X', '') COMB, V.CD, 
    SUM(DECODE(V.PT,'101',V.TIME)) P101,
    SUM(DECODE(V.PT,'102',V.TIME)) P102,
    SUM(DECODE(V.PT,'106',V.TIME)) P106, 
    SUM(DECODE(V.PT,'124',V.TIME)) P124,
    SUM(DECODE(V.PT,'540',V.TIME)) P540,
    SUM(DECODE(V.PT,'541',V.TIME)) P541
FROM (
    WITH tp_per as
    (   select V.TRANSFER_ID from 
        (SELECT        
          subdiv_id,
          transfer_id
        FROM {0}.transfer t    
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        WHERE V.SUBDIV_ID = :p_subdiv_id)
    select TS.PAY_TYPE_ID PT, TS.PER_NUM, TS.SIGN_COMB, TS.CODE_DEGREE CD, ROUND(TS.TIME,2) TIME, GM.NGM
    from {0}.TEMP_SALARY TS
    join (SELECT TR.TRANSFER_ID, TR.PER_NUM, TR.SIGN_COMB 
            FROM {0}.PN_TMP PN JOIN {0}.TRANSFER TR on PN.TRANSFER_ID = TR.TRANSFER_ID
            WHERE PN.USER_NAME = :p_user_name) P1
        on P1.PER_NUM = TS.PER_NUM and P1.SIGN_COMB = TS.SIGN_COMB
    left join tp_per tp on (tp.transfer_id=P1.TRANSFER_ID)
    left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                    LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
            from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
        GM on (gm.transfer_id=tp.transfer_id and :p_date_end between begin_group and end_group)  
    WHERE TS.TEMP_SALARY_ID = :P_TEMP_SALARY_ID
    ORDER BY TS.CODE_DEGREE, TS.GROUP_MASTER, TS.PER_NUM
) V
JOIN {0}.EMP E ON E.PER_NUM = V.PER_NUM
GROUP BY ROLLUP(V.NGM, (V.CD, V.PER_NUM, V.SIGN_COMB, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME))