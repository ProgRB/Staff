declare
TYPE T_CURSOR is ref cursor;
t T_CURSOR; 
s VARCHAR2(2000);
begin
 --параметры периода задавать как начала мес€цев периода 01.01.2014 по 01.06.2014
    select distinct LISTAGG(a.p,',') WITHIN GROUP ( order by p) over() into s
     from 
      (select 'date'''||to_char(add_months(:p_date1,LEVEL-1),'YYYY-MM-DD')||''' as " на '||to_char(add_months(:p_date1,LEVEL-1),'YYYY-MM-DD')||'"' p from dual connect by add_months(:p_date1,LEVEL-1)<=:p_date2) a;
  open :t for   
 'with vac as
        (select
                worker_id, 
                actual_begin, 
                period_begin, 
                period_end, 
                vac_group_type_id,
                type_vac_id
                from {0}.vacation_schedule vs join {0}.vac_consist using (vac_sched_id) 
                    join {0}.type_vac using (type_vac_id) 
                    join {0}.TYPE_VAC_GROUP_RELATION using (type_vac_id)
                    join {0}.vac_group_type using (vac_group_type_id)
                    join {0}.transfer t1 using (transfer_id)
                where actual_begin is not null and need_period=1 and plan_sign=0
        )
       select * from 
        (
            select 
                code_subdiv,
                code_degree,
                per_num,
                fio,
                sgn_comb ,
                pos_name,
                group_vac_name,   
                dt,
                period_end,
                rem_days,
                sum_vac,
                sum_vs,
                avg_w,
                --sum_ob + NVL(sum_vs,0) as sum_ob
                DECODE(MAX(case when period_end<dt then 1 else 0 end) OVER (partition by worker_id, dt), 1, ROUND(GREATEST(sum(sum_ob) over(partition by worker_id, dt) + NVL(sum_vs,0),0) * ratio_to_report(rem_days) over (partition by worker_id, dt), 2), to_number(null) ) as sum_ob
           FROM 
                 (select         
                  worker_id,         
                s.code_subdiv,
                DECODE( code_degree,''04'', DECODE(substr(code_pos,1,1),''2'',''041'',''3'',''042'',''043''), CODE_DEGREE) code_degree,
                vs.per_num,
                emp_last_name||'' ''||Substr(emp_first_name,1,1)||''.''||substr(emp_middle_name,1,1)||''.'' fio,
                DECODE( vs.sign_comb,1,''X'',null) sgn_comb ,
                pos_name,
                group_vac_name,   
                dt,
                period_end,
                rem_days,
                rem_days*nvl(avg_w,0) as sum_vac,
                nvl(avg_w,0) avg_w,
                sum_vs,
                case when rem_days is null then 0 else  rem_days*avg_w - NVL(LAG(rem_days*avg_w,1, 0) over (partition by worker_id, group_vac_name order by dt),0) end as sum_ob
            from     (
                      select per_num,
                              worker_id, 
                            sign_comb,dt,degree_id,
                            pos_id,subdiv_id,
                            group_vac_name,
                            MAX(period_end) as PERIOD_END,
                            /*NVL(max(case when period_end<dt then period_end else null end), MAX(period_end)) as PERIOD_END,*/
                            SUM(rem_days) rem_days
                        FROM
                        (select per_num,
                            sign_comb,dt,degree_id,
                            pos_id,subdiv_id,
                            worker_id,
                            group_vac_name,
                            period_end,
                            vac_group_type_id,
                            ROUND(GREATEST((LEAST(calc_end,dt)-period_end)*calc_period_days/365,0) ) rem_days
                            from 
                                    (select per_num,
                                            worker_id,
                                            sign_comb,
                                            dt,
                                            max(calc_end) as calc_end,
                                            degree_id,pos_id,subdiv_id,
                                            group_vac_name,
                                            vac_group_type_id,
                                            count_period_day calc_period_days,
                                            COALESCE(max( case when actual_begin<dt THEN period_end else NULL END), MIN(case when actual_begin>= dt THEN period_begin-1 else NULL END), max(calc_begin))   period_end,
                                            NVL(max(case when actual_begin<add_months(trunc(:p_end_date, ''month''),-2) and type_vac_calc_period_id=2 then  period_end else null end), date''1000-01-01'') last_period_for_close                                   
                                     from 
                                                ( select  t.per_num,t.sign_comb, 
                                                        subdiv_id,t.degree_id,t.pos_id,
                                                        t.worker_id, 
                                                        PERIOD_BEGIN,
                                                        period_end,
                                                        actual_begin,
                                                        group_vac_name, 
                                                        t.vac_group_type_id,
                                                        type_vac_calc_period_id,
                                                        count_period_day,
                                                        calc_end,
														calc_begin
                                                    from  (select transfer_id,  t2.worker_id, subdiv_id, pos_id, degree_id, per_num, sign_comb,
                                                                    vgt.vac_group_type_id,
                                                                    count_period_day,
                                                                    group_vac_name,
                                                                    type_vac_calc_period_id,
                                                                    NVL(calc_begin, (select min(trunc(date_transfer)) from {0}.transfer where worker_id=t2.worker_id)) calc_begin,
                                                                    NVL(calc_end, date''3000-01-01'') calc_end
                                                                from 
                                                                (select * from {0}.vac_group_type where need_period=1) vgt
                                                                join (select max(transfer_id) KEEP(DENSE_RANK LAST order by date_transfer) as transfer_id, worker_id
                                                                        from {0}.transfer where date_transfer<trunc(:p_end_date,''month'')
                                                                        start with sign_cur_work=1 
                                                                        connect by prior from_position=transfer_id
                                                                        group by worker_id ) t2 on (1=1)
                                                                join {0}.transfer using (transfer_id)
                                                                left join (select worker_id, vac_group_type_id, min( NVL(trunc(calc_begin), (select min(trunc(date_transfer)) from {0}.transfer where worker_id=tr.worker_id))) calc_begin, 
                                                                                max(nvl(trunc(calc_end)+86399/86400, date''3000-01-01'')) calc_end from {0}.vac_add_period join {0}.TRANSFER tr using (transfer_id) group by worker_id, vac_group_type_id) 
                                                                        vap on (vgt.type_vac_calc_period_id=2 and vgt.vac_group_type_id=vap.vac_group_type_id and t2.worker_id=vap.worker_id)
                                                                where vgt.type_vac_calc_period_id=1 or vgt.type_vac_calc_period_id=2 and calc_end is not null
                                                                order by vac_group_type_id, per_num
                                                           ) t
                                                        join {0}.subdiv s using (subdiv_id)
                                                        left join  vac vs1 on (t.worker_id=vs1.worker_id and t.vac_group_type_id=vs1.vac_group_type_id)
                                                     where subdiv_id in (select subdiv_id from {0}.subdiv where sub_actual_sign=1 start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) 
                                                ) vs,
                                                (select add_months(:p_begin_date,LEVEL-1) as dt from dual connect by add_months(:p_begin_date,LEVEL-1)<=:p_end_date) d
                                     group by worker_id,  per_num, sign_comb, dt, degree_id, pos_id, subdiv_id, group_vac_name, vac_group_type_id, count_period_day
                                 )
                                where calc_end - last_period_for_close>365/calc_period_days
                        )
                        group by worker_id, per_num, sign_comb,dt,degree_id, pos_id,subdiv_id, group_vac_name
                    ) vs left join (select * from {0}.sr_data where actual_date between :p_begin_date and :p_end_date) sr on ( sr.per_num=vs.per_num and sr.sign_comb=vs.sign_comb and sr.actual_date=vs.dt)
                     left join ( select sum(sum_sal) sum_vs,per_num,sign_comb, trunc(pay_date, ''month'') pay_date from salary.salary where trunc(pay_date,''month'') between add_months(:p_begin_date,-2) and add_months(:p_end_date,1) 
                                     and payment_type_id in (160, 161, 162) group by trunc(pay_date, ''month''),per_num,sign_comb) sl on ( sl.per_num=vs.per_num and sl.sign_comb=vs.sign_comb and add_months(sl.pay_date,1)=vs.dt)
                     left join {0}.subdiv s on  (s.subdiv_id=vs.subdiv_id)
                     left join {0}.position using (pos_id)
                     left join {0}.degree using (degree_id)
                     left join {0}.emp e on (vs.per_num=e.per_num)
              ) 
         )
         PIVOT (
                        max(period_end) as "»споль. по",
                        max(rem_days) as "Ќакопл.дней",
                        max(avg_w) as "—редний",
                        max(sum_vac) as "—умма резерв",
                        max(sum_vs) as "—умма факт",
                        max(sum_ob) as "ќЅя«"
                        for dt IN ('||s||')
                     )
         order by code_subdiv,per_num, code_degree,per_num desc' using  :p_date2, :p_date2, :p_subdiv_id, :p_date1, :p_date1,:p_date2,:p_date1,:p_date2,:p_date1,:p_date2;
end;