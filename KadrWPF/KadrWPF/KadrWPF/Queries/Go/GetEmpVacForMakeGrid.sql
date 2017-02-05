select 
	  vs.vac_sched_id as vac_id,
	  NVL(close_sign,0) CLOSE_SIGN,
	  plan_begin,
	  {0}.END_OF_VAC(sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID=1 THEN count_days ELSE 0 END),
					 sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID=2 THEN count_days ELSE 0 END),
					 sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID=3 THEN count_days ELSE 0 END),plan_begin)  Plan_end,
					 
	  NULLIF(sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID = 1 THEN count_days ELSE 0 END),0) PLAN_CALEND_DAYS,
	  NULLIF(sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID=2 THEN count_days ELSE 0 END),0) PLAN_WORK_DAYS,
	  NULLIF(sum(CASE when plan_sign=1 and TYPE_VAC_CALC_ID=3 THEN count_days ELSE 0 END),0) PLAN_OTHER_DAYS,
	  actual_begin ,
	  {0}.END_OF_VAC(sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID=1 THEN count_days ELSE 0 END),
					 sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID=2 THEN count_days ELSE 0 END),
					 sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID=3 THEN count_days ELSE 0 END),actual_begin) ACTUAL_END,
	  NULLIF(sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID = 1 THEN count_days ELSE 0 END),0) CALEND_DAYS,
	  NULLIF(sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID=2 THEN count_days ELSE 0 END),0) WORK_DAYS,
	  NULLIF(sum(CASE when plan_sign=0 and TYPE_VAC_CALC_ID=3 THEN count_days ELSE 0 END),0) OTHER_DAYS,
	  DECODE(confirm_sign,1,'Утв.;','Не утв.;')||CASE WHEN CLOSE_SIGN>0 THEN ' Закр.' ELSE 'Не закр.' END VAC_STATE
from
	{0}.vacation_schedule vs
    left join {0}.vac_consist vc on (VC.VAC_SCHED_ID=VS.VAC_SCHED_ID)
    left join {0}.type_vac tv on (vc.type_vac_id=tv.type_vac_id)
where
	transfer_id in (select transfer_id from {0}.transfer where worker_id=(select worker_id from {0}.transfer where transfer_id=:p_transfer_id))
GROUP BY vs.vac_sched_id,close_sign,plan_begin, actual_begin, confirm_sign
order by NVL(actual_begin,plan_begin) DESC