select 
    group_master,
    code_degree,
    per_num,
    emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' FIO,
    sum(count_days) count_days,
    nvl(actual_begin, plan_begin) date_begin,
    max(period_end) period_end
from
    (select worker_id, max(degree_id) keep (dense_rank last order by date_transfer) degree_id
     from
        {0}.transfer
        join (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) using (subdiv_id)
     where sign_cur_work=1
     group by worker_id)  t
    join {0}.degree using (degree_id)
    join (select per_num, transfer_id, worker_id from {0}.transfer) using (worker_id)
    join {0}.vac_sched using (transfer_id)
    join {0}.vac_consist using (vac_sched_id)
    join {0}.emp using (per_num)
    left join (select worker_id, max(name_group_master) keep (dense_rank last order by begin_group) group_master 
               from {0}.transfer join {0}.emp_group_master using (transfer_id) where sysdate between begin_group and nvl(end_group, date'3000-01-01') group by worker_id) using (worker_id)
where
    actual_begin is null and plan_sign=1 and plan_begin>=sysdate or actual_begin is not null and plan_sign=0
    and nvl(actual_begin, plan_begin) between :p_date1 and :p_date2
group by group_master, per_num, emp_last_name, emp_first_name, emp_middle_name, actual_begin, plan_begin, code_degree