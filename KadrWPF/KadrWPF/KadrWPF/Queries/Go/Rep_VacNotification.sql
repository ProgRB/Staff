select s.*,
    date_end - date_begin +1 -(select count(*) from apstaff.calendar where calendar_day between date_begin and date_end and type_day_id in (4)) as count_days
from
(
select
    vac_sched_id  as ID,
    code_subdiv,
    emp_last_name,
    emp_first_name,
    emp_middle_name,
    per_num,
    min(apstaff.VAC_SCHED_PACK.begin_vac_consist(vac_consist_id)) as date_begin,
    max(apstaff.VAC_SCHED_PACK.end_vac_consist(vac_consist_id)) as date_end
from
    apstaff.vacation_schedule
    join apstaff.vac_consist using (vac_sched_id)
    join (select transfer_id, per_num,  worker_id from apstaff.transfer) using (transfer_id)
    join (select worker_id, max(code_subdiv) keep (dense_rank last order by date_transfer) code_subdiv from apstaff.transfer join apstaff.subdiv using (subdiv_id) group by worker_id) using (worker_id)
    join apstaff.emp using (per_num)
where vac_sched_id in (select column_value from table(:p_vac_ids))
	and plan_sign=0
group by vac_sched_id, code_subdiv,
    emp_last_name,
    emp_first_name,
    emp_middle_name,
    per_num
) s
    