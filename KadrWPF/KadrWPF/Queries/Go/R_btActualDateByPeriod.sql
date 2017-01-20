select code_subdiv,
        per_num,
        DECODE(sign_comb,1,'X',null) sign_comb ,
        worker_id,
        DECODE(code_degree,'04',DECODE(substr(code_pos,1,1),'2','041','3','042','043'),code_degree) code_degree ,
        pos_name,
        (select count(*) from apstaff.calendar where calendar_day between actual_begin and actual_end and type_day_id in (1,2,3))  count_days,
        (select count(*) from apstaff.calendar where calendar_day between actual_begin and actual_end and type_day_id in (2,3)) avg_price,
        emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' as fio
from
 ( select worker_id,
    greatest(actual_begin, :p_date_begin) actual_begin,
    least(actual_end, :p_date_end) actual_end
  from        
      ( select worker_id,
             vac_sched_id,
             actual_begin,
             apstaff.end_of_vac(sum(decode(type_vac_calc_id, 1, count_days)), sum(decode(type_vac_calc_id, 2, count_days)), sum(decode(type_vac_calc_id, 3, count_days)), actual_begin) actual_end
        from apstaff.vacation_schedule vs join apstaff.vac_consist vc using (vac_sched_id) join apstaff.type_vac using (type_vac_id)
            join (select worker_id, transfer_id, 
                        max(subdiv_id) keep (dense_rank last order by date_Transfer) over (partition by worker_id) subdiv_id,
                        max(form_operation_id) keep (dense_rank last order by date_Transfer) over (partition by worker_id) form_operation_id 
                  from apstaff.transfer) using (transfer_id)
        where actual_begin < :p_date_end and plan_sign=0  and SING_PAYMENT=1 and close_sign>0
            and (subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) or :p_subdiv_id=31 and apstaff.CHECK_ECON_EMP(form_operation_id)>0)
        group by worker_id, actual_begin, vac_sched_id
     ) vs 
  where actual_end>=:p_date_begin and actual_begin<=actual_end
) vs
join (select worker_id, per_num, sign_comb, 
            max(pos_id) keep (dense_rank last order by date_Transfer) pos_id, 
            max(subdiv_id) keep (dense_rank last order by date_Transfer) subdiv_id,
            max(degree_id) keep (dense_rank last order by date_Transfer) degree_id,
            max(form_operation_id) keep (dense_rank last order by date_Transfer) form_operation_id
        from apstaff.transfer --where sign_cur_work=1
        group by worker_id, per_num, sign_comb
      ) t using (worker_id)
join apstaff.position p using  (pos_id)
join apstaff.degree d using (degree_id)
join apstaff.emp e using (per_num)
join apstaff.subdiv using (subdiv_id)
