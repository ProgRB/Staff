declare
    p_type_vac_ids apstaff.type_table_number := :p_type_vac_ids;
    p_date_begin date := :p_date_begin;
    p_date_end date := :p_date_end;
    p_subdiv_id Number := :p_subdiv_id;
begin
    open :c for
    with vacs as 
    (
        select worker_id, 
            per_num, 
            code_subdiv,
            vac_sched_id,
            plan_sign,
            case when plan_sign=0 then actual_begin else plan_begin end vac_begin,
            sum(count_days) count_days,
            apstaff.end_of_vac(sum(DECODE(TYPE_VAC_CALC_ID,1,COUNT_DAYS,0)), sum(DECODE(TYPE_VAC_CALC_ID,2,COUNT_DAYS,0)), sum(DECODE(TYPE_VAC_CALC_ID,3,COUNT_DAYS,0)), case when plan_sign=0 then actual_begin else plan_begin end) vac_end
        from
            apstaff.vac_sched
            join  apstaff.vac_consist using (vac_sched_id)
            join apstaff.type_vac using (type_vac_id)
            join (select transfer_id, per_num, worker_id from apstaff.transfer) using (transfer_id)
            join (select worker_id, 
                        max(code_subdiv) keep (dense_rank last order by date_transfer) code_subdiv,
                        max(subdiv_id) keep (dense_rank last order by date_transfer) subdiv_id,
						max(sign_cur_work) keep (dense_rank last order by date_transfer) sign_cur_work
                    from apstaff.transfer join apstaff.subdiv using (subdiv_id) 
                   group by worker_id
                ) using (worker_id) 
        where
            subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=p_subdiv_id connect by prior subdiv_id=parent_id)
            and nvl(actual_begin, plan_begin) between p_date_begin and p_date_end
			and sign_cur_work=1
            and (p_type_vac_ids is null or type_vac_id in (select column_value from table(p_type_vac_ids)))
        group by worker_id, per_num, vac_sched_id, plan_sign, code_subdiv,
            case when plan_sign=0 then actual_begin else plan_begin end         
    )
    select
        code_subdiv,
        vac_date cur_date,
        plan_sign,
        count(distinct case when trunc(vac_begin,'month')=vac_date then worker_id end) count_emp,
        sum(case when trunc(vac_begin,'month')=vac_date then count_days end) count_days,
        sum(case when vac_begin>vac_end then count_days else greatest(least(vac_end, vac_end_date+1/86400)+1-greatest(vac_begin, vac_date), 0) end) count_days_month
    from
        vacs
        join (select column_value as vac_date, add_months(column_value,1)-1/86400 as vac_end_date from table(salary.GET_DATE_SEQUENCE(p_date_begin, p_date_end)))
            on (vac_begin<=vac_end_date and vac_end>=vac_date)
    group by code_subdiv, vac_date, plan_sign
    order by code_subdiv, vac_date, plan_sign;
end;
