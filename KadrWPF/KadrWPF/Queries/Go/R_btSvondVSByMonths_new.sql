declare
TYPE T_CURSOR is ref cursor;
t T_CURSOR; 
s VARCHAR2(2000);
begin
 --параметры периода задавать как начала месяцев периода 01.01.2014 по 01.06.2014
   open :t for 
    with vacs as 
	 (select
			worker_id, 
			dt,
			max(period_end) period_end, 
			vac_group_type_id
		from {0}.vac_sched vs 
			join (select vac_sched_id, vac_consist_id, plan_sign from {0}.vac_consist ) using (vac_sched_id)
			join (select vac_consist_id, period_begin, period_end, vac_group_type_id from {0}.vac_calced_period) using (vac_consist_id)  
			join {0}.vac_group_type using (vac_group_type_id)
			join {0}.transfer t1 using (transfer_id),
			(select add_months(:p_date1,LEVEL-1) as dt from dual connect by add_months(:p_date1,LEVEL-1)<=:p_date2)
		where need_period=1 and actual_begin <add_months(trunc(:p_date2,'month'), 1) and plan_sign=0
			and actual_begin<dt
	   group by worker_id, dt, vac_group_type_id
	),
	 v_periods as
	 (
		select 
			vac_group_type_id,
			group_vac_name,
			worker_id,
			transfer_id,
			count_period_day,
			dt,
			nvl(max(period_end), min(calc_begin)) period_end, 
			round(sum(greatest(least(nvl(calc_end, dt),dt) - greatest( calc_begin, nvl(period_end, calc_begin)),0))*count_period_day/365) 
			count_days 
		from
			(select 
				vac_group_type_id,
				group_vac_name,
				worker_id,
				transfer_id,
				count_period_day,
				case when type_vac_calc_period_id=1 then date_hire else calc_begin end calc_begin,
				case when type_vac_calc_period_id=1 then date_fire else nvl(calc_end, date'3000-01-01') end calc_end
			from
				(select vac_group_type_id, count_period_day, type_vac_calc_period_id, group_vac_name from {0}.vac_group_type where need_period=1)
				join 
				(select max(transfer_id) KEEP(DENSE_RANK LAST order by date_transfer) as transfer_id, worker_id,
						(select min(trunc(date_transfer)) from {0}.transfer where worker_id=t.worker_id) date_hire,
						nvl((select trunc(date_transfer)+86399/86400 from {0}.transfer where worker_id=t.worker_id and type_transfer_id=3), date'3000-01-01') date_fire                                                     
					from {0}.transfer  t
					where date_transfer<trunc(:p_date2,'month')
					start with sign_cur_work=1 
					connect by prior from_position=transfer_id
					group by worker_id) t on (1=1)
				left join (select vac_add_period_id, calc_begin, calc_end, vac_group_type_id, worker_id from {0}.vac_add_period join {0}.transfer using (transfer_id)) using (worker_id, vac_group_type_id)
				where (type_vac_calc_period_id=1 or type_vac_calc_period_id=2 and vac_add_period_id is not null)
			)
			join (select add_months(:p_date1,LEVEL-1) as dt from dual connect by add_months(:p_date1,LEVEL-1)<=:p_date2) d on (1=1)
			left join {0}.vacs using (vac_group_type_id, worker_id, dt)
		group by 
			vac_group_type_id,
			group_vac_name,
			worker_id,
			transfer_id,
			count_period_day,
			dt
	 )
	 select 
		worker_id,
		code_subdiv,
		code_degree,
		per_num,
		fio,
		sign_comb  sign_comb,
		pos_name,
		case 
			when code_subdiv in ('086', '089', '097', '019', '021', '022', '070', '093', '027', '041', '241', '043', '045', '119', '132') then '24'
			when code_subdiv in ('011', '012', '016', '017', '018', '020', '024', '124', '028', '030', '040', '036', '049', '050', '051', '053', '054', '058', '060', '061', '095', '123', '133', '146', '055', '109') then '25'
			when code_subdiv in ('013', '015', '019', '031', '033', '035', '064', '078', '081', '083', '085', '087', '088', '090', '092', '096', '100', '101', '106', '108', '111', '114', '140', '150', '147', '160', '170', '180', '190', '411', '420', '350') then '26'
			when code_subdiv in ('110', '120', '130', '180') then '44'
		else null
		end DOC_NOTE,
		group_vac_name TYPE_VAC_GROUP,   
		dt CUR_DATE,
		period_end,
		COUNT_DAYS,
		sum_vac SUM_RESERV,
		sum_vs SUM_VAC_PAID,
		avg_w AVG_PRICE,
		DECODE(MAX(case when period_end<dt then 1 else 0 end) OVER (partition by worker_id, dt), 1, ROUND(GREATEST(sum(sum_ob) over(partition by worker_id, dt) + NVL(sum_vs,0),0) * ratio_to_report(rem_days) over (partition by worker_id, dt), 2), to_number(null) ) as SUM_PROVISION
	from
	(
		select
			code_subdiv,
			per_num,
			FIO,
			pos_name,
			code_degree,
			sign_comb,
			group_vac_name,
			count_period_day,
			vac_group_type_id,
			vs.worker_id,
			dt,
			period_end,
			count_days,
			count_days rem_days,
			count_days*nvl(avg_w,0) as sum_vac,
			nvl(avg_w,0) avg_w,
			sum_vs,
			case when count_days is null then 0 else  count_days*avg_w - NVL(LAG(count_days*avg_w,1, 0) over (partition by vs.worker_id, group_vac_name order by dt),0) end as sum_ob
		from 
			v_periods vs
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
						 join (select subdiv_id, code_subdiv from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) using (subdiv_id)
						 join {0}.degree using (degree_id)
						 join {0}.emp using (per_num)
				  ) t using (transfer_id) 
			 left join (select worker_id, avg_w, actual_date from {0}.sr_data where actual_date between :p_date1 and :p_date2) sr on ( sr.worker_id=vs.worker_id and sr.actual_date=vs.dt)
			 left join (  select sum(sum_sal) sum_vs, worker_id, trunc(pay_date, 'month') pay_date from salary.view_salary_by_subdiv join salary.payment_type using (payment_type_id) join {0}.transfer using (transfer_id) 
							where trunc(pay_date,'month') between add_months(:p_date1,-2) and add_months(:p_date2,1) 
							 and code_payment in ('226', '227', '228') group by trunc(pay_date, 'month'), worker_id) sl on ( sl.worker_id=vs.worker_id and add_months(sl.pay_date,1)=vs.dt)
	)
	order by code_subdiv, per_num, fio, dt, vac_group_type_id;
end;