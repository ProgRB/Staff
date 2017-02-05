select 
	NVL(region_name,'<не указано>') region_name, 
    extract(month from actual_begin) plan_begin,
    listagg(emp_last_name||' '||substr(emp_first_name,1,1)||'. '||substr(emp_middle_name,1,1)||'.'||'('||count_days||' дн.)',';') within group (order by emp_last_name)  fio
from
 	(select per_num, sign_comb, worker_id, degree_id from {0}.transfer where sign_cur_work=1
    	and subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
        ) t
    join 
    (
    	select worker_id, 
        	trunc(NVL(actual_begin, plan_begin), 'month') actual_begin, 
        	sum(count_days) count_days
        from {0}.vacation_schedule join {0}.vac_consist using (vac_sched_id)
        	join {0}.transfer using (transfer_id)
            join {0}.type_vac using (type_vac_id)
        where NVL(actual_begin, plan_begin) between trunc(:p_date,'year') and add_months(trunc(:p_date,'year'),12)-1/86400
        and (actual_begin is not null and plan_sign=0 or actual_begin is null and plan_sign=1) and sing_payment=1
        group by worker_id, trunc(nvl(actual_begin, plan_begin),'month')
    ) using (worker_id)
    left join 
    (select worker_id, max(region_name) keep (dense_rank last order by date_start_work) region_name
     from {0}.emp_region join {0}.region_subdiv rs using (region_subdiv_id)
     	join {0}.transfer using (transfer_id)
     where rs.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
     group by worker_id
    ) using (worker_id)    
    join {0}.emp using (per_num)
    join {0}.degree using (degree_id)
group by region_name,  actual_begin
order by region_name, actual_begin