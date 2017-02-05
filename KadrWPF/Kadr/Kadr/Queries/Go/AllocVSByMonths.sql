with
    vs as ( 
    	select last_transfer_id,
        	vac_sched_id,
            actual_begin,
            cnt_d,
        	actual_end-actual_begin+1-(select count(*) from {0}.calendar where type_day_id in (4) and calendar_day between actual_begin and actual_end) as cnt_cal_days
            from
    		(select last_transfer_id,
            actual_begin,
            vac_sched_id,
            sum(count_days) as cnt_d,
          	{0}.end_of_vac(sum(decode(type_vac_calc_id,1,count_days,0)), sum(decode(type_vac_calc_id,2,count_days,0)), sum(decode(type_vac_calc_id,3,count_days,0)), actual_begin) as actual_end
            from 
                    (select 
                        (select transfer_id from {0}.transfer where sign_cur_work=1 start with transfer_id=vs.transfer_id connect by NOCYCLE prior transfer_id=from_position) as last_transfer_id , 
                        nvl(actual_begin,plan_begin) actual_begin,
                        vac_sched_id,
                        type_vac_id,
                        count_days,
                        type_vac_calc_id
                    from {0}.vacation_schedule vs 
						join {0}.vac_consist using (vac_sched_id) 
						join {0}.type_vac using (type_vac_id)
						JOIN (select type_vac_id, max(vac_group_type_id) keep (dense_rank first order by vac_group_type_id) vac_group_type_id, 
                                            max(NEED_PERIOD) keep (dense_rank first order by vac_group_type_id) NEED_PERIOD,
                                            max(GROUP_VAC_NAME) keep (dense_rank first order by vac_group_type_id) GROUP_VAC_NAME
                                            from {0}.type_vac_group_relation join {0}.vac_group_type using (vac_group_type_id)
                                           group by type_vac_id) using (type_vac_id)
                    where NVL(actual_begin,plan_begin) between :p_date_begin and :p_date_end 
                    and  SING_PAYMENT =1 and need_period=1 and (actual_begin is null and plan_sign=1 or actual_begin is not null and plan_sign=0) )
            group by actual_begin, last_transfer_id, vac_sched_id)
  )
select 
		DECODE(GROUPING(EXTRACT(month from actual_begin)),1,'бяецн',to_char(to_date(EXTRACT(month from actual_begin),'MM'),'Month','NLS_DATE_LANGUAGE=RUSSIAN')) as mon,

        NULLIF(sum( DECODE(CODE_DEGREE,'01',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'01',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'01',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
          
        NULLIF(sum( DECODE(CODE_DEGREE,'02',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'02',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'02',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
        NULLIF(sum( DECODE(CODE_DEGREE,'08',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'08',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'08',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
        NULLIF(sum( DECODE(CODE_DEGREE,'09',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'09',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'09',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
        NULLIF(sum( DECODE(CODE_DEGREE,'05',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'05',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'05',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
        NULLIF(sum( DECODE(CODE_DEGREE,'041',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'041',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'041',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		
		NULLIF(sum( DECODE(CODE_DEGREE,'042',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'042',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'042',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		
		NULLIF(sum( DECODE(CODE_DEGREE,'043',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'043',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'043',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
        NULLIF(sum( DECODE(CODE_DEGREE,'11',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'11',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'11',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		
		NULLIF(sum( DECODE(CODE_DEGREE,'06',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'06',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'06',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		
		NULLIF(sum( DECODE(CODE_DEGREE,'07',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'07',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'07',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		
		NULLIF(sum( DECODE(CODE_DEGREE,'12',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'12',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'12',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
         
		NULLIF(sum( DECODE(CODE_DEGREE,'13',cnt_d,0)),0) k_d,
        to_char(NULLIF(sum( DECODE(CODE_DEGREE,'13',1/cnt_v,0)),0),'FM9999999999990.00')  ch,
        to_char(NULLIF(sum(DECODE(CODE_DEGREE,'13',avg_w*cnt_cal_days,0)),0),'FM9999999999990.00') summa,
		 
		 
        NULLIF(sum(cnt_d),0) k_d,
        to_char(NULLIF(sum(1/cnt_v),0),'FM9999999999990.00')  ch,
  		ROUND(200*RATIO_TO_REPORT(sum(1/cnt_v)) over (),2) percent_v,  
		to_char(NULLIF(sum(avg_w*cnt_cal_days),0),'FM9999999999990.00') summa
         
from vs 
join (select transfer_id,DECODE(CODE_DEGREE,'04',DECODE(substr(code_pos,1,1),'2','041','3','042','043'),code_degree) code_degree, 
		per_num,
		subdiv_id, sign_comb
	  from {0}.transfer t 
		join {0}.degree d using (degree_id)
		join {0}.position p using (pos_id)
	  where sign_cur_work=1
			and (:p_degree_ids is null or degree_id member of :p_degree_ids)
			and (:p_form_oper_ids is null or nvl(form_operation_id, 1) member of :p_form_oper_ids)
	  ) t on (t.transfer_id=vs.last_transfer_id)
left join (select (1+:kf_proc/100)*avg_w avg_w,per_num,sign_comb from {0}.sr_data where actual_date=trunc(sysdate,'month')) st on (t.per_num=st.per_num and t.sign_comb=st.sign_comb)
join ( select last_transfer_id, count(distinct vac_sched_id) cnt_v from vs group by last_transfer_id) s on (t.transfer_id=s.last_transfer_id)
where
	code_degree in ('01','02','041','042','043','05','08','09','11','07','13','12') and 
 t.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id) 
group by rollup (extract(month from actual_begin) )
order by extract(month from actual_begin)