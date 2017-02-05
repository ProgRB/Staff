SELECT SUBDIV_ID, CODE_SUBDIV, SUBDIV_NAME, SUB_ACTUAL_SIGN, WORK_TYPE_ID, SERVICE_ID, SUB_DATE_START, 
	SUB_DATE_END, PARENT_ID, FROM_SUBDIV, TYPE_SUBDIV_ID, GR_WORK_ID 
FROM {0}.SUBDIV S 
WHERE S.SUBDIV_ID in
	(select SUBDIV_ID FROM {0}.SUBDIV
	start with subdiv_id in (
		select subdiv_id from {0}.access_subdiv 
		where upper(user_name) = ora_login_user and upper(app_name) = :p_APP_NAME)
	connect by prior subdiv_id = parent_id)
	and NVL(S.PARENT_ID,0) = 0 and TYPE_SUBDIV_ID != 6
ORDER BY SUB_ACTUAL_SIGN desc, CODE_SUBDIV