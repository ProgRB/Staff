select subdiv_id from apstaff.subdiv 
where sub_actual_sign=1 and subdiv_id in 
		(select subdiv_id from apstaff.transfer where sign_cur_work=1 or type_transfer_id=3  
			start with transfer_id=:p_transfer_id 
			connect by nocycle PRIOR transfer_id=from_position or prior from_position = transfer_id)
start with subdiv_id in (	select subdiv_id from APSTAFF.ACCESS_SUBDIV where UPPER(APP_NAME)=upper(:p_app_name) and USER_NAME=user)
connect by prior subdiv_id = parent_id