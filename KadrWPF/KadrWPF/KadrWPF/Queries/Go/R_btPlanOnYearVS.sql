  select
    code_degree,
    per_num,
	code_subdiv,
    fio,
    pos_name,
    DECODE(sign_comb,1,'X',null) sign_comb,
    NAME_GROUP_MASTER group_master,
    GROUP_VAC_NAME type_vac_name,
    last_period_end period_begin,
    plan_begin date_begin,
    count_days count_days,
    period_end as period_end
from
    (  
    
    select
            vac_sched_id,
            t.worker_id,
            t.per_num,
            t.sign_comb,
            pos_name,
			s.code_subdiv,
            emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' FIO,
            DECODE(CODE_DEGREE,'04',DECODE(substr(code_pos,1,1),'2','041','3','042','043'),code_degree) code_degree,
            case when type_vac_id in (10, 24, 25, 26) then count_days else -- если это компенсация, то у него периода отдыха
                vac_end-vac_begin+1 - (select count(*) from {0}.calendar where type_day_id=4 and calendar_day between vac_begin and vac_end) end as count_days,
            vac_end-vac_begin+1,
            vac_begin,
            vac_end,
            t.transfer_id,
            last_period_end,
            period_end,
            plan_begin,
            NAME_GROUP_MASTER,
            GROUP_VAC_NAME,
            vac_group_type_id,
            (select min(date_transfer) from {0}.transfer where worker_id=t.worker_id) as date_hire 
      from 
           {0}.transfer t 
           join {0}.emp e on (e.per_num=t.per_num)
           join {0}.degree d on (d.degree_id=t.degree_id) 
           join {0}.position ps on (ps.pos_id=t.pos_id)
           join {0}.SUBDIV s on (s.subdiv_id=t.subdiv_id)
           left join (select worker_id, max(name_group_master) keep (dense_rank last order by begin_group) name_group_master, 
							max(t.subdiv_id) keep (dense_rank last order by begin_group) subdiv_id
						from {0}.emp_group_master join {0}.transfer t using (transfer_id) 
                        where sysdate between BEGIN_GROUP and nvl(END_GROUP,date'3000-01-01')
                        group by worker_id) egm on (t.worker_id=egm.worker_id and t.subdiv_id=egm.subdiv_id)
           left join (
           
                        SELECT vac_sched_id,
                                  worker_id,
                                  PLAN_BEGIN,
                                  COUNT_DAYS,
                                  (select max(period_end) 
                                                from {0}.vac_calced_period 
                                                    join (select vac_sched_id, vac_consist_id from {0}.vac_consist) using (vac_consist_id) 
                                                    join {0}.vac_sched using (vac_sched_id) 
                                                    join {0}.transfer using (transfer_id)
                                             where vac_group_type_id=t.vac_group_type_id and worker_id=t.worker_id and nvl(actual_begin, plan_begin)<trunc(:p_date_begin,'month')
                                                    and plan_sign=decode(actual_begin, null, 0, 1)) last_period_end,
                                  PERIOD_BEGIN,
                                  PERIOD_END,
                                  TYPE_VAC_ID,
                                  GROUP_VAC_NAME,
                                  vac_group_type_id,
                                  LAG ({0}.END_OF_VAC(cal_days, work_days, usual_days, plan_begin) + 1, 1, plan_begin)
                                   OVER (PARTITION BY vac_sched_id, plan_sign ORDER BY NUMBER_CALC, COUNT_DAYS, vac_consist_id) AS vac_begin,
                                  {0}.END_OF_VAC (cal_days, work_days, usual_days, plan_begin) vac_end
                             FROM 
                                  (SELECT 
                                          worker_id,
                                          plan_begin as plan_begin,
                                          vac_consist_id,
                                          type_vac_id,
                                          vac_group_type_id,
                                          count_days,
                                          plan_sign,
                                          period_begin,
                                          period_end,
                                          group_vac_name,
                                          vac_sched_id,
                                          number_calc,
                                          need_period,
                                          SUM (DECODE (TYPE_VAC_CALC_ID, 1, COUNT_DAYS, 0))
                                          OVER (PARTITION BY vac_sched_id, plan_sign ORDER BY number_calc, count_days, vac_consist_id) AS cal_days,
                                          SUM (DECODE (TYPE_VAC_CALC_ID, 2, COUNT_DAYS, 0)) 
                                          OVER (PARTITION BY vac_sched_id, plan_sign ORDER BY number_calc, count_days, vac_consist_id) work_days,
                                          SUM (DECODE (TYPE_VAC_CALC_ID, 3, COUNT_DAYS, 0))
                                          OVER (PARTITION BY vac_sched_id, plan_sign ORDER BY number_calc, count_days, vac_consist_id) usual_days
                                     FROM {0}.vacation_schedule vs 
                                        join {0}.transfer using (transfer_id)
                                        join {0}.vac_consist vc using (vac_sched_id)
                                        JOIN {0}.type_vac tv USING (type_vac_id)
                                        join {0}.type_vac_group_relation using (type_vac_id)
                                        join {0}.vac_group_type using (vac_group_type_id)
                                     WHERE plan_begin between :p_date_begin and :p_date_end and plan_sign = 1
                                ) t
                                where need_period=1                                
                            ) vs on (vs.worker_id=t.worker_id)
       where sign_cur_work=1 and hire_sign=1
			and (:p_degree_ids is null or t.degree_id member of :p_degree_ids)
			and (:p_form_oper_ids is null or nvl(t.form_operation_id, 1) member of :p_form_oper_ids)
			and (
					:p_subdiv_id=31 and nvl(t.form_operation_id, 0)=9
					or nvl(t.form_operation_id, 0)!=9 and t.subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
				)
   ) t
order by code_degree, plan_begin, per_num