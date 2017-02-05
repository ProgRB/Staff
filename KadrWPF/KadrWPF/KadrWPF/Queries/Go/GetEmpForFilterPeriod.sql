declare
    p_date_begin Date := :p_date_begin;
    p_date_end Date := :p_date_end;
    p_subdiv_id Number := :p_subdiv_id;
    p_degree_ids APSTAFF.TYPE_TABLE_NUMBER := :p_degree_ids;
    p_form_oper_ids APSTAFF.TYPE_TABLE_NUMBER :=  :p_form_oper_ids;
    p_only_actual Number := :p_only_actual;
begin
 open :c for 
    select 
        vac_sched_id,
        emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
        per_num,
        pos_name,
        NVL(actual_begin,plan_begin) PLAN_BEGIN,
        SUM(COUNT_DAYS) COUNT_DAYS
    from 
        (select worker_id,
            per_num,
            max(subdiv_id) keep (dense_rank last order by date_transfer) subdiv_id,
            max(pos_id) keep (dense_rank last order by date_transfer) pos_id,
            max(degree_id) keep (dense_rank last order by date_transfer) degree_id,
            max(form_operation_id) keep (dense_rank last order by date_transfer) form_operation_id
         from 
            {0}.transfer
         group by worker_id, per_num) t
         join {0}.emp using (per_num)
         join {0}.position using (pos_id)
         join (select transfer_id, worker_id from {0}.transfer) t1 using (worker_id)
         join {0}.vacation_schedule vs using (transfer_id)
         join {0}.vac_consist using (vac_sched_id)
    where
        nvl(actual_begin,plan_begin) between p_date_begin and  p_date_end 
        and (NVL(p_only_actual,0)!=1 or actual_begin is not null)
        and (p_degree_ids is null or degree_id member of p_degree_ids)
        and (p_form_oper_ids is null or form_operation_id member of p_form_oper_ids)
        and (actual_begin is not null and plan_sign=0 or actual_begin is null and plan_sign=1)   
		and (subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=p_subdiv_id connect by prior subdiv_id=parent_id)
				or p_subdiv_id=31 and form_operation_id=9)     
    group by vac_sched_id, emp_last_name, emp_first_name,emp_middle_name,pos_name, actual_begin,plan_begin, per_num
    order by fio;
end;
