select 
	region_name, 
    code_degree,
    per_num,
    emp_last_name||' '||substr(emp_first_name,1,1)||'. '||substr(emp_middle_name,1,1)||'.' fio,
    DECODE(sign_comb, 1,'X','') sign_comb,
    listagg(case when actual_begin<trunc(:p_date,'year') then to_char(actual_begin,'DD-MM-YYYY')||' ('||count_days||' дн.)' else '' end, ';') WITHIN group (order by actual_begin) as prev_vac,
    listagg(case when actual_begin>=trunc(:p_date,'year') then to_char(actual_begin,'DD-MM-YYYY')||' ('||count_days||' дн.)' else '' end, ';') WITHIN group (order by actual_begin) as current_vac,
    trunc(max(case when actual_begin<sysdate then period_end else null end)) period_end
from
 	(select per_num, sign_comb, worker_id, degree_id from {0}.transfer where sign_cur_work=1
    	and subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
        ) t
    left join 
    (select worker_id, max(region_name) keep (dense_rank last order by date_start_work) region_name
     from {0}.emp_region join {0}.region_subdiv rs using (region_subdiv_id)
     	join {0}.transfer using (transfer_id)
     where rs.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
     group by worker_id
    ) using (worker_id)
    left join 
    (
    	select worker_id, actual_begin, sum(count_days) count_days, max(period_end) as period_end
        from {0}.vacation_schedule join {0}.vac_consist using (vac_sched_id)
        	join {0}.transfer using (transfer_id)
        where actual_begin between add_months(trunc(:p_date,'year'),-12) and add_months(trunc(:p_date,'year'),12)-1/86400
        and plan_sign=0
        group by worker_id, actual_begin
    ) using (worker_id)
    join {0}.emp using (per_num)
    join {0}.degree using (degree_id)
group by region_name,  code_degree,  per_num, emp_last_name, emp_first_name, emp_middle_name, sign_comb
order by region_name, per_num