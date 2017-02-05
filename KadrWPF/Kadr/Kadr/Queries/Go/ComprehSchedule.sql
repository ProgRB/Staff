select  per_num, fio, pos_name, sign_comb, 
		name_group_master,
		DECODE(grouping(per_num)+grouping(fl),1, sum(a_y),2,sum(a_z), sum(a_z)) as a,
        DECODE(grouping(per_num)+grouping(fl),1, sum(b_y),2,sum(b_z), sum(b_z)) as b,
        DECODE(grouping(per_num)+grouping(fl),1, sum(c_y),2,sum(c_z), sum(c_z)) as c,
        DECODE(grouping(per_num)+grouping(fl),1, sum(d_y),2,sum(d_z), sum(d_z)) as d,
        DECODE(grouping(per_num)+grouping(fl),1, sum(e_y),2,sum(e_z), sum(e_z)) as e,
        DECODE(grouping(per_num)+grouping(fl),1, sum(f_y),2,sum(f_z), sum(f_z)) as f,
        DECODE(grouping(per_num)+grouping(fl),1, sum(g_y),2,sum(g_z), sum(g_z)) as g,
        DECODE(grouping(per_num)+grouping(fl),1, sum(h_y),2,sum(h_z), sum(h_z)) as h,
        DECODE(grouping(per_num)+grouping(fl),1, sum(i_y),2,sum(i_z), sum(i_z)) as i,
        DECODE(grouping(per_num)+grouping(fl),1, sum(j_y),2,sum(j_z), sum(j_z)) as j,
        DECODE(grouping(per_num)+grouping(fl),1, sum(k_y),2,sum(k_z), sum(k_z)) as k,
        DECODE(grouping(per_num)+grouping(fl),1, sum(l_y),2,sum(l_z), sum(l_z)) as l,
        NULLIF(DECODE(grouping(per_num)+grouping(fl),1, sum(NVL(A_Y,0)+NVL(B_Y,0)+NVL(C_Y,0)+NVL(D_Y,0)+NVL(E_Y,0)+NVL(F_Y,0)+NVL(G_Y,0)+NVL(H_Y,0)+NVL(I_Y,0)+NVL(J_Y,0)+NVL(K_Y,0)+NVL(L_Y,0)),2,
        					sum(NVL(A_Z,0)+NVL(B_Z,0)+NVL(C_Z,0)+NVL(D_Z,0)+NVL(E_Z,0)+NVL(F_Z,0)+NVL(G_Z,0)+NVL(H_Z,0)+NVL(I_Z,0)+NVL(J_Z,0)+NVL(K_Z,0)+NVL(L_Z,0)), 
                            sum(NVL(A_Z,0)+NVL(B_Z,0)+NVL(C_Z,0)+NVL(D_Z,0)+NVL(E_Z,0)+NVL(F_Z,0)+NVL(G_Z,0)+NVL(H_Z,0)+NVL(I_Z,0)+NVL(J_Z,0)+NVL(K_Z,0)+NVL(L_Z,0))),0) as aa
from
(
    select 1 as fl,
     per_num, fio, pos_name, decode(sign_comb,1, 'X','') as sign_comb,
     name_group_master,
   A_Z, B_Z, C_Z, D_Z, E_Z, F_Z, G_Z, H_Z, I_Z, J_Z, K_Z, L_Z,
   ROUND(A_Y,1) as A_Y, ROUND(B_Y,1) as B_Y, ROUND(C_Y,1) as C_Y, ROUND(D_Y,1) as D_Y, ROUND(E_Y,1) as E_Y, ROUND(F_Y,1) as F_Y, ROUND(G_Y,1) as G_Y, ROUND(H_Y,1) as H_Y, ROUND(I_Y,1) as I_Y, ROUND(J_Y,1) as J_Y, ROUND(K_Y,1) as K_Y, ROUND(L_Y,1) as L_Y
   from
    (
        select 
                per_num,
                EMP_LAST_NAME||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' as fio,
                pos_name,
                SIGN_COMB,
                name_group_master,            
                /*last_transfer_id,*/
                EXTRACT(month from actual_begin) as mnth,
                cnt_d, 
                1/nullif(count(vac_sched_id) over (partition by t.worker_id),0) as cnt_emp
        from
            {0}.transfer t 
            join {0}.emp using (per_num)
            join {0}.position p on (t.pos_id=p.pos_id)
            left join (select worker_id, max(NAME_GROUP_MASTER) keep (dense_rank last order by begin_group) NAME_GROUP_MASTER from  {0}.EMP_GROUP_MASTER join {0}.transfer using (transfer_id) 
							where  sysdate between BEGIN_GROUP and nvl(end_GROUP, date'3000-01-01') group by worker_id) egm on (t.worker_id=egm.worker_id)
            left join ( select worker_id,
                        actual_begin,
                        vac_sched_id,
                        sum(count_days) as cnt_d
                        from 
                                (select 
                                    worker_id, 
                                    nvl(actual_begin,plan_begin) actual_begin,
                                    vac_sched_id,
                                    type_vac_id,
                                    count_days,
                                    type_vac_calc_id
                                from {0}.vacation_schedule vs 
									join {0}.vac_consist using (vac_sched_id) 
									join {0}.transfer using (transfer_id)
									join {0}.type_vac using (type_vac_id)
									JOIN (select type_vac_id, max(vac_group_type_id) keep (dense_rank first order by vac_group_type_id) vac_group_type_id, 
                                            max(NEED_PERIOD) keep (dense_rank first order by vac_group_type_id) NEED_PERIOD,
                                            max(GROUP_VAC_NAME) keep (dense_rank first order by vac_group_type_id) GROUP_VAC_NAME
                                            from {0}.type_vac_group_relation join {0}.vac_group_type using (vac_group_type_id)
                                           group by type_vac_id) using (type_vac_id)
                                where NVL(actual_begin,plan_begin) between :p_date_begin and :p_date_end 
                                and  SING_PAYMENT =1 and need_period=1 and (actual_begin is null and plan_sign=1 or actual_begin is not null and plan_sign=0) )
                        group by actual_begin, worker_id, vac_sched_id) vs on (vs.worker_id=t.worker_id)
        where t.sign_cur_work=1 and subdiv_id in (select subdiv_id from {0}.subdiv where sub_actual_sign=1 start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
			and (:p_degree_ids is null or t.degree_id member of :p_degree_ids)
			and (:p_form_oper_ids is null or nvl(t.form_operation_id, 1) member of :p_form_oper_ids)
    )
    PIVOT
    (
        SUM(cnt_d) z,
        sum(cnt_emp) as y
        for mnth in (1 as a ,2 as b,3 as c,4 as d,5 as e,6 as f,7 as g,8 as h,9 as i,10 as j,11 as k,12 as l)
    )
)
group by rollup (fl,(fio,per_num, pos_name, sign_comb, name_group_master))
order by FIO, sign_comb
   