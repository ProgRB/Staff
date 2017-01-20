with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
select PS.PROJECT_STATEMENT_ID, PROJECT_TRANSFER_ID, PS.TRANSFER_ID,
    T.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME,
    E.EMP_SEX, E.EMP_BIRTH_DATE, 
    T.SUBDIV_ID PREV_SUBDIV_ID,
    (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) PREV_CODE_SUBDIV,
    PS.TO_SUBDIV_ID, 
    (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = PS.TO_SUBDIV_ID) CODE_SUBDIV,
    T.POS_ID, (SELECT P.CODE_POS FROM {0}.POSITION P WHERE P.POS_ID = T.POS_ID) CODE_POS,
    (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T.POS_ID) POS_NAME,
    T.POS_NOTE, T.SIGN_COMB, DECODE(T.SIGN_COMB,1,'X') COMB, PS.BASE_DOC_ID, PROJECT_PLAN_APPROVAL_ID,
    T.DEGREE_ID, T.FORM_PAY, T.PROBA_PERIOD, T.DATE_TRANSFER, PS.TYPE_TRANSFER_ID,
    T.FORM_OPERATION_ID, AD.CLASSIFIC, AD.TARIFF_GRID_ID, AD.SALARY,
    CASE WHEN PROJECT_PLAN_APPROVAL_ID = (select MAX(PROJECT_PLAN_APPROVAL_ID) from {0}.PROJECT_PLAN_APPROVAL
                                            where TYPE_TRANSFER_ID = PS.TYPE_TRANSFER_ID)
        THEN 1 ELSE 0 
    END as SIGN_FULL_APPROVAL,
	NVL(SIGN_PRINT_MATCHING,0) SIGN_PRINT_MATCHING,
    CASE WHEN EXISTS(SELECT NULL FROM {0}.PROJECT_STATEMENT_APPROVAL PA 
                    WHERE PA.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID) 
        THEN 1 ELSE 0 
    END as SIGN_EXIST_APPROVAL,
    (SELECT CONTR_EMP FROM {0}.TRANSFER T1 
    WHERE T1.WORKER_ID = T.WORKER_ID and T1.TYPE_TRANSFER_ID = 1) TR_NUM_ORDER_HIRE,
    (SELECT DATE_CONTR FROM {0}.TRANSFER T1 
    WHERE T1.WORKER_ID = T.WORKER_ID and T1.TYPE_TRANSFER_ID = 1) TR_DATE_ORDER_HIRE,
	CASE WHEN EXISTS(SELECT NULL FROM {0}.TRANSFER_QM TQM WHERE TQM.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID)
        THEN 1 ELSE 0 
    END as SIGN_TRANSFER_QM
from {0}.PROJECT_STATEMENT PS
JOIN {0}.TRANSFER T ON (T.TRANSFER_ID = PS.TRANSFER_ID) 
join {0}.ACCOUNT_DATA AD on (T.TRANSFER_ID = AD.TRANSFER_ID)
join {0}.EMP E on (T.PER_NUM = E.PER_NUM)
/* Выбираем сотрудников, которые переводятся в данное подразделение и у них уже прошло согласование 
    в предыдущем подразделении, а так же тех, кто уходит из подразделения, чтобы проставить согласование*/
WHERE (((PS.TO_SUBDIV_ID in (select SUBDIV_ID FROM SUBD)
        or (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE') and T.FORM_OPERATION_ID = 9)
		or (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and (exists(select null from {0}.TRANSFER_QM TQM where TQM.TRANSFER_ID = T.TRANSFER_ID)
				or exists(select null from {0}.TRANSFER_QM TQM where TQM.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID)))
			)		
		)
        or 
        (T.SUBDIV_ID in (select SUBDIV_ID FROM SUBD)))
    and PS.PROJECT_STATEMENT_ID = NVL(:p_PROJECT_STATEMENT_ID,PS.PROJECT_STATEMENT_ID) 
    and PS.PROJECT_TRANSFER_ID is null         
    /* по этим параметрам обновляем строки, поменявшие статус согласования */
    and (:p_SIGN_NOTIFICATION = 0 or (:p_SIGN_NOTIFICATION = 1 and 
            PS.ROWID in (select CHARTOROWID(COLUMN_VALUE) from TABLE(:p_TABLE_ID))))
    and PROJECT_PLAN_APPROVAL_ID > -1