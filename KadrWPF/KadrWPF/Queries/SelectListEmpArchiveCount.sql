with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT COUNT(CASE WHEN COMB=0 THEN PER_NUM END) COUNT_EMP,
    COUNT(DISTINCT CASE WHEN EMP_SEX='Æ' THEN PER_NUM END) COUNT_WOMAN,
    COUNT(CASE WHEN COMB=1 THEN PER_NUM END) COUNT_COMB,
    COUNT(CASE WHEN COMB=1 and 
                exists(select null from {0}.TRANSFER T2 where T2.PER_NUM = CUR_EMP.PER_NUM and T2.SIGN_COMB = 0 
                            and T2.SIGN_CUR_WORK = 1 and T2.TRANSFER_ID != CUR_EMP.TRANSFER_ID) 
            THEN PER_NUM
        END) COUNT_IN_COMB,
    COUNT(CASE WHEN COMB=1 and 
                not exists(select null from {0}.TRANSFER T2 where T2.PER_NUM = CUR_EMP.PER_NUM and T2.SIGN_COMB = 0 
                                and T2.SIGN_CUR_WORK = 1 and T2.TRANSFER_ID != CUR_EMP.TRANSFER_ID) 
            THEN PER_NUM 
        END) COUNT_OUT_COMB
FROM (
    SELECT 
        E.PER_NUM, T1.SIGN_COMB COMB, DECODE(T1.SIGN_COMB, 0, '', 'X') as SIGN_COMB, T1.TRANSFER_ID, E.EMP_SEX, POS_NAME,
		EMP_BIRTH_DATE, R.REASON_ID, T1.WORKER_ID
    FROM 
        (SELECT * FROM {0}.Emp E0 
		WHERE not Exists(SELECT * FROM {0}.Transfer T0 WHERE E0.PER_NUM = T0.PER_NUM and (SIGN_CUR_WORK = 1 or HIRE_SIGN = 0 
				/*or TYPE_TRANSFER_ID = 7*/))
			and exists(select null from {0}.TRANSFER T0 where E0.PER_NUM = T0.PER_NUM)) E
    LEFT JOIN (
        SELECT T2.* 
        FROM 
            {0}.Emp E2, {0}.Transfer T2
        WHERE 
            E2.PER_NUM = T2.PER_NUM and TYPE_TRANSFER_ID = 3 and HIRE_SIGN = 1 and 
            SIGN_COMB = CASE WHEN
                Exists(
                    SELECT * FROM {0}.Transfer T22
                    WHERE
                        E2.PER_NUM = T22.PER_NUM and SIGN_COMB = 0 and TYPE_TRANSFER_ID = 3
                        and DATE_TRANSFER = (
                            SELECT 
                                Max(DATE_TRANSFER) 
                            FROM 
                                {0}.Transfer T23 
                            WHERE
                                E2.PER_NUM = T23.PER_NUM and T23.TYPE_TRANSFER_ID = 3
                        ) 
                ) THEN 0 ELSE 1 
            END
            and DATE_TRANSFER = (
                SELECT 
                    Max(DATE_TRANSFER) 
                FROM 
                    {0}.Transfer T21 
                WHERE
                    E2.PER_NUM = T21.PER_NUM and T21.TYPE_TRANSFER_ID = 3
            )
    ) T1 ON E.PER_NUM = T1.PER_NUM
    LEFT OUTER JOIN {0}.Subdiv S ON T1.SUBDIV_ID = S.SUBDIV_ID
    LEFT OUTER JOIN {0}.Position P ON T1.POS_ID = P.POS_ID
    LEFT OUTER JOIN {0}.REASON_DISMISS R ON T1.REASON_ID = R.REASON_ID
    WHERE T1.SUBDIV_ID in (select subdiv_id from SUBD)
		or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE') and FORM_OPERATION_ID = 9)
        or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and exists(select null from {0}.TRANSFER_QM TQM where TQM.TRANSFER_ID = T1.TRANSFER_ID)
        )
) 
CUR_EMP{1}

