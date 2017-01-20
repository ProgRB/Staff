with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT COUNT(*) FROM 
(   SELECT 
        S.CODE_SUBDIV, E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
        E.EMP_MIDDLE_NAME, E.EMP_BIRTH_DATE, CASE T5.SIGN_COMB WHEN 1 THEN 'סמגל.' ELSE '' END AS SIGN_COMB, 
        CASE WHEN EXISTS(SELECT * FROM {0}.Transfer T6 WHERE T6.PER_NUM = T1.PER_NUM and T6.SIGN_CUR_WORK = 1 and 
        T6.SIGN_COMB = 1) THEN 'X' ELSE '' END as COMB, P.CODE_POS, P.POS_NAME, T5.TRANSFER_ID, T5.DATE_HIRE, P.POS_ID
    FROM 
        {0}.Emp E
        RIGHT JOIN
            (SELECT T.*, (SELECT TRANSFER_ID FROM {0}.Transfer T2
                          WHERE T2.PER_NUM = T.PER_NUM and T2.SUBDIV_ID = T.SUBDIV_ID 
                                and T2.SIGN_CUR_WORK = 1 and T2.SIGN_COMB = {2}
                         ) as TRANSFER_ID FROM 
                (SELECT PER_NUM, SUBDIV_ID FROM {0}.Transfer  
                 WHERE SIGN_CUR_WORK = 1 and HIRE_SIGN = 1 GROUP BY PER_NUM, SUBDIV_ID) T) T1
            ON E.PER_NUM = T1.PER_NUM
        LEFT OUTER JOIN {0}.Subdiv S ON T1.SUBDIV_ID = S.SUBDIV_ID
        LEFT JOIN {0}.Transfer T5 ON T1.TRANSFER_ID = T5.TRANSFER_ID
        LEFT OUTER JOIN {0}.Position P ON T5.POS_ID = P.POS_ID
    WHERE {3}
        T5.SUBDIV_ID in (select subdiv_id from SUBD)
)
CUR_EMP{1}

/*
0 schema
1 sort.ToString()
*/