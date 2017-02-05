with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT CODE_SUBDIV, PER_NUM, DISMISS, EMP_LAST_NAME, EMP_FIRST_NAME, 
    EMP_MIDDLE_NAME, SIGN_COMB, EMP_BIRTH_DATE, CODE_POS, POS_NAME,
    TRANSFER_ID, DATE_HIRE, POS_ID, EMP_SEX , WORKER_ID
FROM (
	SELECT 
        (SELECT S.CODE_SUBDIV FROM APSTAFF.SUBDIV S WHERE S.SUBDIV_ID = T1.SUBDIV_ID) CODE_SUBDIV,
        E.PER_NUM, '*' as DISMISS, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
        E.EMP_MIDDLE_NAME, E.EMP_BIRTH_DATE, DECODE(T1.SIGN_COMB, 0, '', 'X') as SIGN_COMB, 
        CASE WHEN EXISTS(SELECT * FROM APSTAFF.Transfer T6 WHERE T6.PER_NUM = T1.PER_NUM and T6.SIGN_CUR_WORK = 1 and 
            T6.SIGN_COMB = 1) THEN 'X' ELSE '' END as COMB,  
        (SELECT P.CODE_POS FROM APSTAFF.POSITION P WHERE P.POS_ID = T1.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM APSTAFF.POSITION P WHERE P.POS_ID = T1.POS_ID) POS_NAME,
        T1.TRANSFER_ID, T1.DATE_HIRE, T1.POS_ID, E.EMP_SEX, WORKER_ID
	FROM 
		(SELECT * FROM {0}.Emp E0 
		WHERE not Exists(SELECT * FROM {0}.Transfer T0 WHERE E0.PER_NUM = T0.PER_NUM and (SIGN_CUR_WORK = 1 or HIRE_SIGN = 0 or TYPE_TRANSFER_ID = 7))
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
	/*LEFT OUTER JOIN {0}.Subdiv S ON T1.SUBDIV_ID = S.SUBDIV_ID
	LEFT OUTER JOIN {0}.Position P ON T1.POS_ID = P.POS_ID
	LEFT OUTER JOIN {0}.REASON_DISMISS R ON T1.REASON_ID = R.REASON_ID*/
	WHERE T1.SUBDIV_ID in (select subdiv_id from SUBD)
	union 
    SELECT 
        (SELECT S.CODE_SUBDIV FROM APSTAFF.SUBDIV S WHERE S.SUBDIV_ID = T1.SUBDIV_ID) CODE_SUBDIV,
        E.PER_NUM, null DISMISS, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
        E.EMP_MIDDLE_NAME, E.EMP_BIRTH_DATE, DECODE(T1.SIGN_COMB, 0, '', 'X') as SIGN_COMB, 
        CASE WHEN EXISTS(SELECT * FROM APSTAFF.Transfer T6 WHERE T6.PER_NUM = T1.PER_NUM and T6.SIGN_CUR_WORK = 1 and 
            T6.SIGN_COMB = 1) THEN 'X' ELSE '' END as COMB, 
        (SELECT P.CODE_POS FROM APSTAFF.POSITION P WHERE P.POS_ID = T1.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM APSTAFF.POSITION P WHERE P.POS_ID = T1.POS_ID) POS_NAME,
        T1.TRANSFER_ID, T1.DATE_HIRE, T1.POS_ID, E.EMP_SEX, WORKER_ID
    FROM 
        APSTAFF.Emp E
        RIGHT JOIN
            (SELECT T.*, (SELECT TRANSFER_ID FROM APSTAFF.Transfer T2
                          WHERE T2.PER_NUM = T.PER_NUM and T2.SUBDIV_ID = T.SUBDIV_ID 
                                and ( 
                                    /*в случае работы в одном подразделении в качестве осн. и совм.*/
                                    (T2.SIGN_CUR_WORK = 1 and T2.SIGN_COMB = 0 and Exists(
                                        SELECT * FROM APSTAFF.Transfer 
                                        WHERE PER_NUM = T2.PER_NUM and SUBDIV_ID = T2.SUBDIV_ID 
                                            and SIGN_CUR_WORK = 1))
                                    or 
                                    /*в случае работы в разных подразделениях, либо просто совместитель*/
                                    (T2.SIGN_CUR_WORK = 1 and T2.SIGN_COMB = 1 and not Exists(
                                        SELECT * FROM APSTAFF.Transfer 
                                        WHERE PER_NUM = T2.PER_NUM and SUBDIV_ID = T2.SUBDIV_ID 
                                            and SIGN_CUR_WORK = 1 and SIGN_COMB = 0))
                                )
                         ) as TRANSFER_ID FROM 
                (SELECT PER_NUM, SUBDIV_ID, POS_ID, DATE_HIRE, SIGN_COMB FROM APSTAFF.Transfer  
                 WHERE SIGN_CUR_WORK = 1 and HIRE_SIGN = 1 
                 GROUP BY PER_NUM, SUBDIV_ID, POS_ID, DATE_HIRE, SIGN_COMB) T) T1
            ON E.PER_NUM = T1.PER_NUM
        /*LEFT OUTER JOIN APSTAFF.Subdiv S ON T1.SUBDIV_ID = S.SUBDIV_ID*/
        LEFT JOIN APSTAFF.Transfer T5 ON T1.TRANSFER_ID = T5.TRANSFER_ID
        /*LEFT OUTER JOIN APSTAFF.Position P ON T5.POS_ID = P.POS_ID*/
    WHERE T5.SUBDIV_ID in (select subdiv_id from SUBD)
) 
CUR_EMP{1}

