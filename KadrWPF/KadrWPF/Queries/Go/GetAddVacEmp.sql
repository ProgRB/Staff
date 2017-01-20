select 
	GROUP_VAC_NAME||' (c '||to_char(CALC_BEGIN,'DD.MM.YYYY')||' по '||NVL(to_char(CALC_END,'DD.MM.YYYY'),'<без ограничения>')||')' as NAME_VAC
from 
	{0}.vac_group_type join
	{0}.vac_add_period using (vac_group_type_id)
where 
	--SYSDATE between NVL(calc_begin,date'1000-01-01') and NVL(calc_end,date'3000-01-01') and	
	transfer_id in (select transfer_id from {0}.transfer start with transfer_id=:transfer_id connect by NOCYCLE prior transfer_id=from_position or transfer_id=prior from_position)
order by group_vac_name, calc_begin, calc_end