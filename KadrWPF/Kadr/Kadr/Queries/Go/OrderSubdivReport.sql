select 
	emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
	e.per_num,
    code_subdiv,
	pos_name "Должность",
    name_vac,
    to_char(period_begin,'DD.MM.YYYY'),
    to_char(period_end,'DD.MM.YYYY'),
    COUNT_DAYS,
    to_char({0}.VAC_SCHED_PACK.BEGIN_VAC_CONSIST(vac_consist_id),'DD.MM.YYYY') actual_begin,
    to_char({0}.VAC_SCHED_PACK.END_VAC_CONSIST(vac_consist_id),'DD.MM.YYYY') actual_end,
	'заявление'	as VAC_REASON,
	null
from 
	 {0}.vacation_schedule vs
     join {0}.vac_consist vc using (vac_sched_id)
     join {0}.type_vac using (type_vac_id)
     , {0}.transfer t
     join {0}.emp e on (e.per_num=t.per_num)
     join {0}.position ps on (ps.pos_id =t.pos_id)
     join {0}.subdiv sb on (t.subdiv_id=sb.subdiv_id)
where
		vc.plan_sign=0 and 
		t.transfer_id = (select transfer_id from {0}.transfer where sign_cur_work=1  start with transfer_id=vs.transfer_id connect by NOCYCLE prior from_position=transfer_id or from_position=prior transfer_id)
		and t.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:subd_id connect by prior subdiv_id=parent_id) 
		and vac_sched_id in ({1})
order by fio,number_calc