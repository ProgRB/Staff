with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT * FROM 
(   SELECT 
        (SELECT S.CODE_SUBDIV from {0}.Subdiv S WHERE T1.SUBDIV_ID = S.SUBDIV_ID) CODE_SUBDIV, 
        E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, 
        E.EMP_MIDDLE_NAME, TRUNC(T1.DATE_TRANSFER) DATE_TRANSFER, TRUNC(T1.DATE_END_CONTR) DATE_END_CONTR, 
		(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER ch where T1.CHAR_WORK_ID = ch.TYPE_TERM_TRANSFER_ID) as CHAR_WORK_NAME, 
		(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER CT where T1.CHAR_TRANSFER_ID = CT.TYPE_TERM_TRANSFER_ID) as CHAR_TRANSFER_NAME, 
		decode(T1.SIGN_COMB,1,'סמגל.','') AS SIGN_COMB, 
        CASE WHEN EXISTS(SELECT * FROM {0}.Transfer T6 WHERE T6.PER_NUM = T1.PER_NUM and T6.SIGN_CUR_WORK = 1 and 
        T6.SIGN_COMB = 1) THEN 'X' ELSE '' END as COMB, 
        (SELECT P.CODE_POS from {0}.Position P WHERE T1.POS_ID = P.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME from {0}.Position P WHERE T1.POS_ID = P.POS_ID) POS_NAME, POS_NOTE,
        T1.TRANSFER_ID, T1.DATE_HIRE, T1.POS_ID,
        E.EMP_SEX, T1.WORKER_ID,
        (SELECT MI.MED_INSPECTION_DATE FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) MED_INSPECTION_DATE,
        (SELECT MI.STUDY_LABOR_SAFETY FROM {0}.MED_INSPECTION MI WHERE MI.PER_NUM = E.PER_NUM) STUDY_LABOR_SAFETY,
		PERCO_SYNC_ID
    FROM (select * from {0}.TRANSFER T
        where T.DATE_END_CONTR >= TRUNC(sysdate) and
            not exists(select null from {0}.TRANSFER T2 where T2.FROM_POSITION = T.TRANSFER_ID) and
            not exists(select null from {0}.TRANSFER T3 where T3.WORKER_ID = T.WORKER_ID and T3.TYPE_TRANSFER_ID = 3)) T1
    JOIN {0}.Emp E ON E.PER_NUM = T1.PER_NUM
    WHERE T1.SUBDIV_ID in (select subdiv_id from SUBD)
		or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE') and FORM_OPERATION_ID = 9
        )
		or 
        (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and exists(select null from {0}.TRANSFER_QM TQM where TQM.TRANSFER_ID = T1.TRANSFER_ID))
        /*exists(select null from user_role_privs where granted_role =  'STAFF_FULL_FILL') or 
        exists(select null from role_role_privs where granted_role =  'STAFF_FULL_FILL') or
        T1.SUBDIV_ID in
            (select subdiv_id from {0}.access_subdiv where upper(user_name) = ora_login_user and upper(app_name) = 'KADR')*/
)
CUR_EMP {1}