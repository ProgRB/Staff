SELECT GW.GR_WORK_DAY_ID, GW.GR_WORK_ID, GW.NUMBER_DAY as "День графика", 
    (select TZ.TIME_ZONE_NAME||' = '||TO_CHAR( 
        NVL((select sum(((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+case when TYPE_INTERVAL_ID=2 then 1 else 0 end)-
                TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI'))*24) from {0}.TIME_INTERVAL TI 
        where TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID),0) )||' ч.'
	from {0}.TIME_ZONE TZ where GW.TIME_ZONE_ID = TZ.TIME_ZONE_ID) as "Время работы для обычного дня",
    (select TZ.TIME_ZONE_NAME||' = '||TO_CHAR( 
        NVL((select sum(((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+case when TYPE_INTERVAL_ID=2 then 1 else 0 end)-
                TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI'))*24) from {0}.TIME_INTERVAL TI 
        where TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID),0) )||' ч.'
	from {0}.TIME_ZONE TZ where GW.HOLIDAY_TIME_ZONE_ID = TZ.TIME_ZONE_ID) as "Время работы для выходного дня",
    (select TZ.TIME_ZONE_NAME||' = '||TO_CHAR( 
        NVL((select sum(((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+case when TYPE_INTERVAL_ID=2 then 1 else 0 end)-
                TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI'))*24) from {0}.TIME_INTERVAL TI 
        where TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID),0) )||' ч.'
	from {0}.TIME_ZONE TZ where GW.COMPACT_TIME_ZONE_ID = TZ.TIME_ZONE_ID) as "Время работы для сокращ. дня",
    case when GW.SIGN_EVENING_TIME = 1 then 'X' else '' end as "Расчет вечернего времени",
    (select TO_CHAR((select MIN((TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI')+DECODE(TYPE_INTERVAL_ID,1,0,2,0,3,1))) 
        from {0}.TIME_INTERVAL TI 
        where TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID),'HH24:MI')
    from {0}.TIME_ZONE TZ where GW.TIME_ZONE_ID = TZ.TIME_ZONE_ID) MIN_TIME_BEGIN,
    (select TO_CHAR((select MAX((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+DECODE(TYPE_INTERVAL_ID,1,0,2,1,3,1))) 
        from {0}.TIME_INTERVAL TI 
        where TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID),'HH24:MI')
    from {0}.TIME_ZONE TZ where GW.TIME_ZONE_ID = TZ.TIME_ZONE_ID) MAX_TIME_END,
    CASE WHEN EXISTS(select null from {0}.TIME_INTERVAL TI 
                    join {0}.TIME_ZONE TZ on TI.TIME_ZONE_ID = TZ.TIME_ZONE_ID
                    where GW.TIME_ZONE_ID = TZ.TIME_ZONE_ID and TYPE_INTERVAL_ID in (2,3)) 
        THEN 1 else 0 END FOR_NEXT_DAY, 
    GW.NUMBER_DAY
from  {0}.GR_WORK_DAY GW 
WHERE GW.GR_WORK_ID = :p_gr_work_id
ORDER BY GW.NUMBER_DAY