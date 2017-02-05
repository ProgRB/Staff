select (select TZ.TIME_ZONE_NAME from {0}.TIME_ZONE TZ where TZ.TIME_ZONE_ID = GD.TIME_ZONE_ID) as TIME_ZONE_NAME, GD.NUMBER_DAY 
from {0}.GR_WORK_DAY GD where GD.GR_WORK_ID = :gr_work_id
order by GD.NUMBER_DAY