select 
	code_subdiv,
    per_num,
    emp_last_name||' '||substr(emp_first_name, 1,1)||'.'||substr(emp_middle_name, 1,1)||'.' FIO,
    min(APSTAFF.VAC_SCHED_PACK.BEGIN_VAC_CONSIST(vac_consist_id)) DATE_BEGIN,
    max(APSTAFF.VAC_SCHED_PACK.END_VAC_CONSIST(vac_consist_id)) DATE_END,
	DATE_CLOSE as ORDER_DATE
from
    apstaff.vacation_schedule
    join apstaff.vac_consist using (vac_sched_id)
    join (select transfer_id, worker_id, per_num from apstaff.transfer) using (transfer_id)
    join apstaff.emp using (per_num)
    join (select max(subdiv_id) keep (dense_rank last order by date_transfer) subdiv_id, worker_id from apstaff.transfer group by worker_id) using (worker_id)
    join (select subdiv_id, code_subdiv from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) using (subdiv_id)
where
    actual_begin between :p_date_begin and :p_date_end
    and plan_sign=0
    and close_sign>0
group by per_num, emp_last_name, emp_first_name, emp_middle_name, code_subdiv, date_close
order by per_num
    