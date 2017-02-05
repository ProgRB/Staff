select 
	emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
	per_num,
    code_subdiv,
	pos_name,
    name_vac TYPE_VAC_NAME,
    period_begin,
    period_end,
    COUNT_DAYS,
    apstaff.VAC_SCHED_PACK.BEGIN_VAC_CONSIST(vac_consist_id) DATE_BEGIN,
    apstaff.VAC_SCHED_PACK.END_VAC_CONSIST(vac_consist_id) DATE_END
from 
	 apstaff.vacation_schedule vs
     join apstaff.vac_consist vc using (vac_sched_id)
     join apstaff.type_vac using (type_vac_id)
	 join (select transfer_id, worker_id from apstaff.transfer) using (transfer_id)
     join (select per_num, worker_id, 
				max(pos_id) keep (dense_rank last order by date_transfer) pos_id,
				max(subdiv_id) keep (dense_rank last order by date_transfer) subdiv_id
		   from apstaff.transfer
		   group by per_num, worker_id) using (worker_id)
     join apstaff.emp using (per_num)
     join apstaff.position using (pos_id)
     join apstaff.subdiv using (subdiv_id)
where
		plan_sign=0 and 
		subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:subd_id connect by prior subdiv_id=parent_id) 
		and vac_sched_id in ({1})
order by fio,number_calc