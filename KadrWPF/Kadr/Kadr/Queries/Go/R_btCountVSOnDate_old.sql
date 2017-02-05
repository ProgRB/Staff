with vac as
        (select vs.worker_id,
                actual_begin,period_begin, period_end, 
                group_vac_name,
                calc_period_days,
                vs.vac_group_type_id,
                vs.type_vac_id, NVL(calc_end,date'3000-01-01') as calc_end  from
            (select 
                worker_id, 
                actual_begin, period_begin, period_end, group_vac_name,
                calc_period_days,vac_group_type_id,
                type_vac_id
                from {0}.vacation_schedule vs 
                    join {0}.vac_consist using (vac_sched_id)
                    join {0}.transfer using (transfer_id) 
                    join {0}.type_vac using (type_vac_id)
                    JOIN (select type_vac_id, max(vac_group_type_id) keep (dense_rank first order by vac_group_type_id) vac_group_type_id, 
                                            max(NEED_PERIOD) keep (dense_rank first order by vac_group_type_id) NEED_PERIOD,
                                            max(GROUP_VAC_NAME) keep (dense_rank first order by vac_group_type_id) GROUP_VAC_NAME
                            from {0}.type_vac_group_relation join {0}.vac_group_type using (vac_group_type_id)
                            group by type_vac_id) using (type_vac_id) 
                where actual_begin is not null and need_period=1 and plan_sign=0
            ) vs
            left join 
                ( select worker_id, vac_group_type_id,
                        max(calc_end) as calc_end
                    from
                    (select  
                            worker_id,
                            CALC_END,
                            vac_group_type_id 
                     from {0}.VAC_ADD_PERIOD vap join {0}.transfer using (transfer_id))
                    group by worker_id, vac_group_type_id
                ) vap on (vap.worker_id=vs.worker_id and vap.vac_group_type_id=vs.vac_group_type_id)
        )
        select
            row_number() over (order by code_subdiv, code_degree, FIO,per_num, pos_name, sign_comb, group_vac_name, period_end),
            code_subdiv,
            code_degree,
            decode (GROUPING(fio),1, DECODE(GROUPING(code_degree),1,'бяецн он ондп.: '||code_subdiv, 'бяецн ОН йюрец.'||code_degree), ' '||fio) FIO,
            per_num,
            pos_name,
            sign_comb,
            group_vac_name,
            period_end,
            sum(rem_days),
            max(avg_w),
            sum(sum_vac) as sum_vac
         from 
        (
            select
                s.code_subdiv,
                DECODE( code_degree,'04', DECODE(substr(code_pos,1,1),'2','041','3','042','043'), CODE_DEGREE) code_degree,
                vs.per_num,
                group_vac_name,
                emp_last_name||' '||Substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio,
                DECODE( vs.sign_comb,1,'X',null) sign_comb ,
                pos_name,   
                dt,
                period_end,
                rem_days,
                avg_w,
                rem_days*avg_w as sum_vac
            from     (
                      select transfer_id,per_num,
                            sign_comb,dt,degree_id,
                            pos_id,subdiv_id,
                            /*MAX(period_end) as PERIOD_END,*/
                            group_vac_name,
                            NVL(max(case when period_end<dt then period_end else null end), MAX(period_end)) as PERIOD_END,
                            SUM(rem_days) rem_days
                        FROM
                        (select transfer_id,per_num,
                            sign_comb,dt,degree_id,
                            pos_id,subdiv_id,
                            group_vac_name,
                            period_end,
                            vac_group_type_id,
                            ROUND(GREATEST((LEAST(calc_end,dt)-period_end)*calc_period_days/365,0) ) rem_days
                            from 
                                    (select transfer_id,per_num,
                                            sign_comb,
                                            :p_date dt,
                                            max(calc_end) as calc_end,
                                            degree_id,pos_id,subdiv_id,
                                            group_vac_name,
                                            vac_group_type_id,
                                            min(calc_period_days) as calc_period_days,
                                            COALESCE(max( case when actual_begin<= :p_date THEN period_end else NULL END), MIN(case when actual_begin> :p_date THEN period_begin-1 else NULL END),(select min(trunc(date_transfer)) date_hire from {0}.transfer t2 where t2.worker_id=vs.worker_id))   period_end                                    
                                     from 
                                                ( select t.transfer_id as transfer_id, t.per_num, t.worker_id,
                                                        t.sign_comb, subdiv_id,t.degree_id,t.pos_id, 
                                                        PERIOD_BEGIN,
                                                        period_end,actual_begin,
                                                        NVL(group_vac_name, (select group_vac_name from {0}.vac_group_type where vac_group_type_id=1)) as group_vac_name, 
                                                        NVL(vac_group_type_id,1) vac_group_type_id,
                                                        NVL(NULLIF(calc_period_days,365),36) as calc_period_days,
                                                        NVL(calc_end,date'3000-01-01') as calc_end
                                                    from  (select max(transfer_id) KEEP(DENSE_RANK LAST order by date_transfer) as transfer_id, worker_id, sign_comb, per_num,
                                                                        max(pos_id) KEEP(DENSE_RANK LAST order by date_transfer) pos_id,
                                                                        max(degree_id) KEEP(DENSE_RANK LAST order by date_transfer) degree_id,
                                                                        max(subdiv_id) KEEP(DENSE_RANK LAST order by date_transfer) subdiv_id                                                              
                                                                    from {0}.transfer 
                                                                    where date_transfer<trunc(:p_date,'month')
                                                                    start with sign_cur_work=1 
                                                                    connect by prior from_position=transfer_id
                                                                    group by worker_id, sign_comb, per_num
                                                           ) t
                                                        join {0}.subdiv s using (subdiv_id)
                                                        left join  vac vs1 on (t.worker_id=vs1.worker_id)
                                                     where subdiv_id in (select subdiv_id from {0}.subdiv where sub_actual_sign=1 start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) 
                                                ) vs
                                     group by transfer_id, worker_id, per_num,sign_comb,degree_id,pos_id,subdiv_id,group_vac_name, vac_group_type_id)
                        )
                        group by transfer_id,per_num, sign_comb,dt,degree_id, pos_id,subdiv_id, group_vac_name
                    ) vs left join (select * from {0}.sr_data where trunc(actual_date,'month')=trunc(:p_date,'month')) sr on ( sr.per_num=vs.per_num and sr.sign_comb=vs.sign_comb)
                     left join {0}.subdiv s on  (s.subdiv_id=vs.subdiv_id)
                     left join {0}.position using (pos_id)
                     left join {0}.degree using (degree_id)
                     left join {0}.emp e on (vs.per_num=e.per_num) 
         )
         group by rollup(code_subdiv,    code_degree, (FIO,per_num, pos_name, sign_comb, group_vac_name, period_end))