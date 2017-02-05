with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT COUNT(CASE WHEN COMB=0 THEN PER_NUM END) COUNT_EMP,
    COUNT(DISTINCT CASE WHEN EMP_SEX='ֶ' THEN PER_NUM END) COUNT_WOMAN,
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
FROM 
(   SELECT 
        E.PER_NUM, decode(T1.SIGN_COMB,1,'סמגל.','') AS SIGN_COMB, T1.SIGN_COMB COMB, T1.FORM_OPERATION_ID, T1.TRANSFER_ID, T1.WORKER_ID, E.EMP_SEX, 
		/*POS_NAME,*/
        (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T1.POS_ID) POS_NAME,
		EMP_BIRTH_DATE
    FROM 
        {0}.Emp E
        JOIN
            (SELECT T.* FROM {0}.Transfer T
                 WHERE SIGN_CUR_WORK = 1 and HIRE_SIGN = 1) T1
            ON E.PER_NUM = T1.PER_NUM
	/*LEFT OUTER JOIN {0}.Position P ON T1.POS_ID = P.POS_ID*/
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