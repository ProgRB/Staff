select GR_WORK_ID as GR_WORK_ID, 
    ( select case when count(*)=1 then max(code_subdiv) end from APSTAFF.ACCESS_GR_WORK aa  join APSTAFF.SUBDIV using (SUBDIV_ID)  
	  where aa.GR_WORK_ID = GR.GR_WORK_ID) as CODE_SUBDIV, 
    GR_WORK_NAME , 
	COUNT_DAY , 
    decode(SIGN_HOLIDAY_WORK,1,'X') SIGN_HOLIDAY_WORK, 
    decode(SIGN_COMPACT_DAY_WORK,1,'X') as SIGN_COMPACT_DAY_WORK,
    decode(SIGN_FLOATING,1,'X') as SIGN_FLOATING, 
	decode(SIGN_SHORTEN,1,'X') as SIGN_SHORTEN, 
	decode(SIGN_SHIFTMAN,1,'X') as SIGN_SHIFTMAN, 
	HOURS_FOR_NORM, 
    HOURS_FOR_GRAPH,
	HOURS_NORM_CALENDAR,
	(select ROUND(sum(((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+case when TYPE_INTERVAL_ID=2 then 1 else 0 end)-
            TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI'))*24), 8) 
		from apstaff.GR_WORK_DAY GWD
			join apstaff.TIME_ZONE TZ on (GWD.TIME_ZONE_ID = TZ.TIME_ZONE_ID)
			join apstaff.TIME_INTERVAL TI on (TZ.TIME_ZONE_ID = TI.TIME_ZONE_ID)
		where GWD.GR_WORK_ID = GR.GR_WORK_ID and GWD.NUMBER_DAY = 1) CALC_HOURS_GRAPH, 		
    decode(SIGN_SUMMARIZE,1,'X') as SIGN_SUMMARIZE,
	HOURS_DINNER,
    decode((select max(GD.SIGN_EVENING_TIME) from apstaff.GR_WORK_DAY GD where GD.GR_WORK_ID = GR.GR_WORK_ID),1,'X')
        as SIGN_EVENING_TIME,
	DATE_END_GRAPH /*,
	ACCESS_TEMPLATE_ID,
	(SELECT ACCESS_TEMPLATE_NAME FROM apstaff.ACCESS_TEMPLATE AT 
	WHERE AT.ACCESS_TEMPLATE_ID = GR.ACCESS_TEMPLATE_ID) ACCESS_TEMPLATE_NAME*/
from APSTAFF.GR_WORK GR
where 
	(:p_code_subdiv is null or
	  GR.GR_WORK_ID in (select aa.GR_WORK_ID from APSTAFF.ACCESS_GR_WORK aa join APSTAFF.SUBDIV using (subdiv_id) where code_subdiv like :p_code_subdiv||'%')
	)
	and NVL(GR.DATE_END_GRAPH,DATE '3000-01-01') >= NVL(:p_DATE_END,DATE '1000-01-01')
order by 3