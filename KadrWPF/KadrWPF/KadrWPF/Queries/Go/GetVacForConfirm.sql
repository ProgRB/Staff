WITH st as (select worker_id, vac_sched_id, confirm_sign, actual_begin, plan_begin, period_end,
					count_days from 
						 {0}.transfer t join 
						 {0}.vacation_schedule vs1 using (transfer_id)
                         join {0}.vac_consist vs using (vac_sched_id)
                         join {0}.type_vac using (type_vac_id)
                 	where SING_PAYMENT=1 and (ACTUAL_BEGIN is null and plan_sign=1 or ACTUAL_BEGIN is not null and plan_sign=0)                    
                 )
select
	'False' fl, 
	vac_sched_id,
	t.per_num,
	emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
	code_degree,
	t.degree_id,
	count_days,
    last_vac,
    next_vac,
    last_period,
	plan_begin,
	period_end to_be_period,
	nvl(Confirm_sign,0) as confirm_sign,
	t.transfer_id,
	t.worker_id,
	t.subdiv_id,
    DECODE(sign_comb,1,'X','') sign_comb,
	'Не проверено' as fl_check
  from
  (select * from {0}.transfer where sign_cur_work=1) t 
  left join  (select worker_id, vac_sched_id, confirm_sign, nvl(actual_begin,plan_begin) as plan_begin, max(period_end) as period_end,
  					 sum(count_days) as count_days from st 
                where NVL(actual_begin,plan_begin) between trunc(:p_date,'year') and add_months(trunc(:p_date,'year'),12)-1 
                  group by worker_id, vac_sched_id, confirm_sign, plan_begin, actual_begin) vs on (vs.worker_id=t.worker_id)	
  join {0}.emp on (emp.per_num=t.per_num)  
  left join {0}.degree on (t.degree_id=degree.degree_id)  
  left join (select max(case when actual_begin<trunc(:p_date,'year') then ACTUAL_BEGIN ELSE NULL END) last_vac,
  						min(case when actual_begin is null then PLAN_BEGIN ELSE NULL END) next_vac,
                        max(case when NVL(actual_begin,plan_begin)<trunc(:p_date,'year') then PERIOD_END else NULL end) last_period,
  					worker_id
                 from st 
             group by worker_id) vs2 on (t.worker_id=vs2.worker_id)
  where t.subdiv_id in (select subdiv_id from {0}.subdiv  start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) 
 order by 4