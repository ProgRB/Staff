with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT CODE_SUBDIV, PER_NUM, DISMISS, EMP_LAST_NAME, EMP_FIRST_NAME, 
    EMP_MIDDLE_NAME, SIGN_COMB, EMP_BIRTH_DATE, CODE_POS, POS_NAME,POS_NOTE,
    TRANSFER_ID, DATE_HIRE, DATE_TRANSFER, REASON_NAME, POS_ID, EMP_SEX , WORKER_ID, 
	MED_INSPECTION_DATE, STUDY_LABOR_SAFETY, PERCO_SYNC_ID
FROM (
	SELECT 
		/*S.CODE_SUBDIV, */
        (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T1.SUBDIV_ID) CODE_SUBDIV,
		E.PER_NUM, '*' as DISMISS, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
		E.EMP_MIDDLE_NAME, DECODE(T1.SIGN_COMB, 0, '', 'X') as SIGN_COMB, E.EMP_BIRTH_DATE, 
		/*P.CODE_POS, P.POS_NAME,*/
        (SELECT P.CODE_POS FROM {0}.POSITION P WHERE P.POS_ID = T1.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T1.POS_ID) POS_NAME,
		T1.POS_NOTE, T1.FORM_OPERATION_ID, T1.TRANSFER_ID, T1.DATE_HIRE, T1.DATE_TRANSFER, 
		/*R.REASON_NAME, */
		(SELECT R.REASON_NAME FROM {0}.REASON_DISMISS R WHERE T1.REASON_ID = R.REASON_ID) REASON_NAME, 
		T1.POS_ID, E.EMP_SEX, T1.REASON_ID, WORKER_ID,
        (SELECT MI.MED_INSPECTION_DATE FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) MED_INSPECTION_DATE,
        (SELECT MI.STUDY_LABOR_SAFETY FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) STUDY_LABOR_SAFETY, 
		PERCO_SYNC_ID
	FROM 
		(SELECT * FROM {0}.Emp E0 
		WHERE not Exists(SELECT * FROM {0}.Transfer T0 WHERE E0.PER_NUM = T0.PER_NUM and 
			(SIGN_CUR_WORK = 1 or HIRE_SIGN = 0 /*or TYPE_TRANSFER_ID = 7*/))
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
		or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE') and FORM_OPERATION_ID = 9
        )
        or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and exists(select null from {0}.TRANSFER_QM TQM where TQM.TRANSFER_ID = T1.TRANSFER_ID)
        )
) 
CUR_EMP{1}

