with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT CODE_SUBDIV, PER_NUM, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME, EMP_BIRTH_DATE, SIGN_COMB, COMB, 
    CODE_POS, POS_NAME, POS_NOTE, TRANSFER_ID, DATE_HIRE, POS_ID, EMP_SEX, WORKER_ID, 
    MED_INSPECTION_DATE, STUDY_LABOR_SAFETY, PERCO_SYNC_ID
FROM 
(   SELECT 
        /*S.CODE_SUBDIV, */
        (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T1.SUBDIV_ID) CODE_SUBDIV, 
		E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
        E.EMP_MIDDLE_NAME, E.EMP_BIRTH_DATE, decode(T5.SIGN_COMB,1,'совм.','') AS SIGN_COMB, 
        CASE WHEN EXISTS(SELECT * FROM {0}.Transfer T6 WHERE T6.PER_NUM = T1.PER_NUM and T6.SIGN_CUR_WORK = 1 and 
        T6.SIGN_COMB = 1) THEN 'X' ELSE '' END as COMB, 
		/*P.CODE_POS, P.POS_NAME, */
        (SELECT P.CODE_POS FROM {0}.POSITION P WHERE P.POS_ID = T5.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T5.POS_ID) POS_NAME,
		T5.POS_NOTE, T5.FORM_OPERATION_ID, T5.TRANSFER_ID, T5.DATE_HIRE, T5.POS_ID, E.EMP_SEX, T5.WORKER_ID,
        (SELECT MI.MED_INSPECTION_DATE FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) MED_INSPECTION_DATE,
        (SELECT MI.STUDY_LABOR_SAFETY FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) STUDY_LABOR_SAFETY,
		PERCO_SYNC_ID
    FROM 
        {0}.Emp E
        RIGHT JOIN
            (SELECT T.*, (SELECT TRANSFER_ID FROM {0}.Transfer T2
                          WHERE T2.PER_NUM = T.PER_NUM and T2.SUBDIV_ID = T.SUBDIV_ID 
                                and ( 
                                    /*в случае работы в одном подразделении в качестве осн. и совм.*/
                                    (T2.SIGN_CUR_WORK = 1 and T2.SIGN_COMB = 0 and Exists(
                                        SELECT * FROM {0}.Transfer 
                                        WHERE PER_NUM = T2.PER_NUM and SUBDIV_ID = T2.SUBDIV_ID 
                                            and SIGN_CUR_WORK = 1))
                                    or 
                                    /*в случае работы в разных подразделениях, либо просто совместитель*/
                                    (T2.SIGN_CUR_WORK = 1 and T2.SIGN_COMB = 1 and not Exists(
                                        SELECT * FROM {0}.Transfer 
                                        WHERE PER_NUM = T2.PER_NUM and SUBDIV_ID = T2.SUBDIV_ID 
                                            and SIGN_CUR_WORK = 1 and SIGN_COMB = 0))
                                )
                         ) as TRANSFER_ID FROM 
                (SELECT PER_NUM, SUBDIV_ID FROM {0}.Transfer  
                 WHERE SIGN_CUR_WORK = 1 and HIRE_SIGN = 1 GROUP BY PER_NUM, SUBDIV_ID) T) T1
            ON E.PER_NUM = T1.PER_NUM
        /*LEFT OUTER JOIN {0}.Subdiv S ON T1.SUBDIV_ID = S.SUBDIV_ID*/
        LEFT JOIN {0}.Transfer T5 ON T1.TRANSFER_ID = T5.TRANSFER_ID
        /*LEFT OUTER JOIN {0}.Position P ON T5.POS_ID = P.POS_ID*/
    WHERE T5.SUBDIV_ID in (select subdiv_id from SUBD)
		or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE')
            and FORM_OPERATION_ID = 9
        )
        or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and exists(select null from {0}.TRANSFER_QM TQM where TQM.TRANSFER_ID = T5.TRANSFER_ID))
)
CUR_EMP{1}