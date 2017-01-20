declare
	c sys_refcusor;
begin
	raise_application_error(-20222, 'Îòêëþ÷åíî');
	open c for
		with vacs as (
		select max(period_end) period_end, worker_id, vac_group_type_id
		from {0}.vac_calced_period
			join (select vac_consist_id, plan_sign, vac_sched_id from {0}.vac_consist) using (vac_consist_id)
			join {0}.vac_sched using (vac_sched_id)
			join {0}.transfer using (transfer_id)
			join {0}.vac_group_type using (vac_group_type_id)
		where 
			need_period=1 and actual_begin is not null and plan_sign=0 and actual_begin<:p_date
		group by worker_id, vac_group_type_id)
		select
			row_number() over (order by code_subdiv, code_degree, fio, per_num, pos_name, sign_comb, period_end) rn,
			code_subdiv,
			code_degree,
			decode (GROUPING(fio),1, DECODE(GROUPING(code_degree),1,'ÂÑÅÃÎ ÏÎ ÏÎÄÐ.: '||code_subdiv, 'ÂÑÅÃÎ ïî ÊÀÒÅÃ.'||code_degree), ' '||fio) FIO,
			per_num,
			pos_name,
			sign_comb,
			group_vac_name,
			period_end,
			sum(count_days),
			max(avg_w),
			sum(count_days*avg_w)
		from  
			(select 
				vac_group_type_id,
				group_vac_name,
				worker_id,
				transfer_id,
				nvl(max(trunc(period_end)), min(calc_begin)) period_end,
				round (sum(greatest(least(nvl(calc_end, :p_date), :p_date) - greatest( calc_begin, nvl(period_end, calc_begin)),0))*count_period_day/365) count_days
			from
				(select 
					vac_group_type_id,
					group_vac_name,
					worker_id,
					transfer_id,
					count_period_day,
					case when type_vac_calc_period_id=1 then date_hire else NVL(calc_begin, date_hire) end calc_begin,
					case when type_vac_calc_period_id=1 then date_fire else nvl(calc_end, date'3000-01-01') end calc_end
				from
					{0}.vac_group_type
					join 
					(select max(transfer_id) keep (dense_rank last order by date_transfer) transfer_id, worker_id, 
						(select min(trunc(date_transfer)) from {0}.transfer where worker_id=t.worker_id) date_hire,
						nvl((select trunc(date_transfer)+86399/86400 from {0}.transfer where worker_id=t.worker_id and type_transfer_id=3), date'3000-01-01') date_fire       
					 from
						(
							select transfer_id, worker_id,
								trunc(date_transfer) date_transfer,
								decode(type_transfer_id, 3, trunc(date_transfer)+86399/86400, lead(trunc(date_transfer)-1/86400, 1, date'3000-01-01') over (partition by worker_id order by date_transfer)) end_transfer 
							from {0}.transfer 
								join {0}.subdiv using (subdiv_id)
							where code_subdiv not in ('500', '300')
						) t
						where  trunc(:p_date,'month')-1/86400 between date_transfer and end_transfer
					 group by worker_id
					) t on (1=1)
					left join (select vac_add_period_id, calc_begin, calc_end, vac_group_type_id, worker_id from {0}.vac_add_period join {0}.transfer using (transfer_id)) using (worker_id, vac_group_type_id)
					where need_period=1 and (type_vac_calc_period_id=1 or type_vac_calc_period_id=2 and vac_add_period_id is not null)
				)
				left join {0}.vacs using (vac_group_type_id, worker_id)
			group by vac_group_type_id, group_vac_name, worker_id, transfer_id, count_period_day    
			)
			left join (select worker_id, avg_w from {0}.sr_data where actual_date = trunc(:p_date,'month')) using (worker_id)
			join (select transfer_id, 
						DECODE( code_degree,'04', DECODE(substr(code_pos,1,1),'2','041','3','042','043'), CODE_DEGREE) code_degree,
						pos_name,
						per_num,
						decode(sign_comb,1,'X') sign_comb,
						code_subdiv,
						subdiv_id,
						emp_last_name||' '||Substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio                
				   from 
						 {0}.transfer 
						 join {0}.position using (pos_id)
						 join {0}.subdiv using (subdiv_id)
						 join {0}.degree using (degree_id)
						 join {0}.emp using (per_num)
				  ) using (transfer_id)
		   where (nvl(count_days,0)>0 or nvl(period_end, date'3000-01-01')>add_months(trunc(:p_date,'month'),-3))
				and subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
		group by rollup(code_subdiv,code_degree,
			(fio,    
			per_num,
			pos_name,
			sign_comb,
			group_vac_name,
			period_end));
end;