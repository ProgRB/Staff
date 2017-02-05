select
	work_place_id,
	code_pos,
	pos_name,
	subdiv_id,
	code_subdiv,
	worker_count,
	decode(high_salary_sign, 1, 'X', '') high_salary_sign,
	decode(addition_vac_sign, 1, 'X', '') addition_vac_sign,
	decode(short_work_day_sign, 1, 'X', '') short_work_day_sign,
	decode(milk_sign, 1, 'X', '') milk_sign,
	med_checkup_period,
	work_place_num,
	subclass_number
from
	apstaff.work_place
	join apstaff.subdiv using (subdiv_id)
	join apstaff.position using (pos_id)
	left join 
		(select work_place_id, max(decode(sign_main_type,1,subclass_number)) subclass_number
			from apstaff.work_place_condition 
				join apstaff.type_condition using (type_condition_id)
				join apstaff.conditions_of_work using (conditions_of_work_id)
		 group by work_place_id) using (work_place_id)
where
	subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
	and (:p_work_place_num is null or work_place_num=:p_work_place_num)
order by code_subdiv, work_place_num
