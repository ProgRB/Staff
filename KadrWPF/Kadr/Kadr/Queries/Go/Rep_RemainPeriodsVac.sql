select *
from
(
    select 
        code_subdiv,
        per_num,
        emp_last_name||' '||substr(emp_first_name, 1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio,
        code_pos as code_order,
        pos_name ,
        code_degree,
        group_vac_name type_vac_name,
        period_end,
        actual_begin date_begin,
        round((select count_days from TABLE({0}.VAC_SCHED_PACK.GET_REM_DAYS_EMP(worker_id, :p_date)) where vac_group_type_id=vs.vac_group_type_id), 2) count_days,
        count_period_day
        --round((:p_date-period_end)*count_period_day/365, 2) count_days
    from
        {0}.transfer
        join (select subdiv_id, code_subdiv from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) using (subdiv_id)
        join {0}.degree using (degree_id)
        join {0}.position using (pos_id)
        join {0}.emp using (per_num)
        join (select worker_id, group_vac_name, count_period_day, vac_group_type_id,
                    max(period_end) period_end, max(actual_begin) actual_begin, 
                    max(vac_sched_id) keep (dense_rank last order by actual_begin) vac_sched_id 
                from {0}.vac_calced_period 
                    join (select vac_consist_id, plan_sign, vac_sched_id from {0}.vac_consist) using (vac_consist_id) 
                    join {0}.vacation_schedule using (vac_sched_id) 
                    join {0}.transfer using (transfer_id)
                    join {0}.vac_group_type using (vac_group_type_id)
                where plan_sign=0 and close_sign>0 and need_period=1
                group by worker_id, group_vac_name, count_period_day, vac_group_type_id
             ) vs using (worker_id)
    where sign_cur_work=1 and period_end<=add_months(:p_date, -:p_months)
)
where count_days>=count_period_day*:p_months/12
order by code_subdiv, per_num, type_vac_name

