select rownum, a.* from (select code_subdiv,
         code_degree,
         CASE WHEN GROUPING(FIO) = 1 AND GROUPING(code_degree)=0 THEN 'Всего по категории:' ELSE FIO END FIO,
         per_num,
         pos_name,
         sign_comb ,
         sum( count_days) sum_d,
         sum(work_days) work_days
 from
     (select code_subdiv,
                per_num,
                DECODE(sign_comb,1,'X',null) sign_comb ,
                worker_id,
                DECODE(code_degree,'04',DECODE(substr(code_pos,1,1),'2','041','3','042','043'),code_degree) code_degree ,
                pos_name,
                (select count(*) from {0}.calendar where calendar_day between actual_begin and actual_end and type_day_id in (1,2,3))  count_days,
                (select count(*) from {0}.calendar where calendar_day between actual_begin and actual_end and type_day_id in (2,3)) work_days,
                emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' as fio
      from
         ( select worker_id,
            greatest(actual_begin, date'{2}') actual_begin,
            least(actual_end, date'{3}') actual_end
          from        
              ( select worker_id,
                     vac_sched_id,
                     actual_begin,
                     {0}.end_of_vac(sum(decode(type_vac_calc_id, 1, count_days)), sum(decode(type_vac_calc_id, 2, count_days)), sum(decode(type_vac_calc_id, 3, count_days)), actual_begin) actual_end
                from {0}.vacation_schedule vs join {0}.vac_consist vc using (vac_sched_id) join {0}.type_vac using (type_vac_id)
                    join (select worker_id, transfer_id, 
                                max(subdiv_id) keep (dense_rank last order by date_Transfer) over (partition by worker_id) subdiv_id,
                                max(form_operation_id) keep (dense_rank last order by date_Transfer) over (partition by worker_id) form_operation_id 
                          from {0}.transfer) using (transfer_id)
                where actual_begin <= date '{3}'-1 and plan_sign=0  and SING_PAYMENT=1
                    and (subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id) or {1}=31 and {0}.CHECK_ECON_EMP(form_operation_id)>0)
                group by worker_id, actual_begin, vac_sched_id
             ) vs 
          where actual_end>=date'{2}' and actual_begin<=actual_end
        ) vs
         join (select worker_id, per_num, sign_comb, max(pos_id) keep (dense_rank last order by date_Transfer) pos_id, max(subdiv_id) keep (dense_rank last order by date_Transfer) subdiv_id,
                    max(degree_id) keep (dense_rank last order by date_Transfer) degree_id,
                    max(form_operation_id) keep (dense_rank last order by date_Transfer) form_operation_id
                    from {0}.transfer --where sign_cur_work=1
                 group by worker_id, per_num, sign_comb
               ) t using (worker_id)
           join {0}.position p using  (pos_id)
        join {0}.degree d using (degree_id)
         join {0}.emp e using (per_num)
        join {0}.subdiv using (subdiv_id)
     )
 group by rollup(code_subdiv,code_degree,(fio,per_num,pos_name,sign_comb))
 ) a