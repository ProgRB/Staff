select code_subdiv,
    subdiv_name,
    confirm_vacs,
    nvl(all_count_vacs, 0) all_count_vacs,
    NVL(percent_confirm,0) percent_confirm
from
(select * from {0}.subdiv join {0}.type_subdiv using (type_subdiv_id) 
	where sub_actual_sign=1 and parent_id=0 and code_subdiv is not null
		and sign_subdiv_plant=1)
left join
(
    select
        code_subdiv,
        count(case when confirm_sign=1 then worker_id else null end) CONFIRM_VACS,
        count(*) ALL_COUNT_VACS,
        round(count(case when confirm_sign=1 then worker_id else null end)/nullif(count(*),0),6) PERCENT_CONFIRM
    from
    (select 
        (select max(code_subdiv) keep (dense_rank last order by date_transfer) from {0}.transfer join {0}.subdiv using (subdiv_id) where worker_id=t.worker_id) code_subdiv,
        plan_begin, actual_begin,
        confirm_sign, worker_id
    from {0}.vacation_schedule vs
        join {0}.transfer t on (t.transfer_id=vs.transfer_id) 
    where 
        nvl(plan_begin, actual_begin) between trunc(:p_date,'year') and add_months(trunc(:p_date, 'year'), 12)-1/86400
        and exists(select 1 from {0}.vac_consist where vac_sched_id=vs.vac_sched_id and type_vac_id not in (2,3))
    )
    group by code_subdiv
    order by code_subdiv
) using (code_subdiv)
