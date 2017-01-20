SELECT V.CD, V.PER_NUM, V.NGM, 
    E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
    CODE_POS, POS_NAME, HOURS_ABS
FROM (    
    WITH tp_per as
    (   select V.TRANSFER_ID, V.DEGREE_ID, V.POS_ID from 
        (SELECT subdiv_id, transfer_id, degree_id, POS_ID
        FROM {0}.transfer t    
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        WHERE V.SUBDIV_ID = :p_subdiv_id)
    select GM.NGM, D.CODE_DEGREE CD, P2.PER_NUM, P.CODE_POS, P.POS_NAME,
        {0}.ABSENCE_CALC(P2.PER_NUM) HOURS_ABS
    from (select distinct P1.PER_NUM, P1.TRANSFER_ID                   
            from (SELECT TR.TRANSFER_ID, TR.PER_NUM, TR.SIGN_COMB 
                    FROM {0}.PN_TMP PN JOIN {0}.TRANSFER TR on PN.TRANSFER_ID = TR.TRANSFER_ID
                    WHERE PN.USER_NAME = :p_user_name) P1
            ) P2
    left join tp_per tp on (tp.transfer_id=P2.TRANSFER_ID)
    join {0}.DEGREE D on tp.DEGREE_ID = D.DEGREE_ID
    join {0}.POSITION P on tp.POS_ID = P.POS_ID
    left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                    LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
            from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
        GM on (gm.transfer_id=tp.transfer_id and :p_date_end between begin_group and end_group)
) V
JOIN {0}.EMP E ON E.PER_NUM = V.PER_NUM
WHERE HOURS_ABS <> 0
ORDER BY V.CD, V.NGM, V.PER_NUM