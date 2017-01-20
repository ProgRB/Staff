select NUMBER_DAY as "№ дня", "1" as "Обычный день", "2" as "Выходной день", "3" as "Сокращенный день"  
from
(
    select 1 RN, rownum as T, TI.TIME_BEGIN||' - '||TI.TIME_END||' ('||TL.TYPE_INTERVAL_NAME||')' F1, GR.NUMBER_DAY
    from {0}.GR_WORK_DAY GR
    JOIN {0}.TIME_INTERVAL TI on TI.TIME_ZONE_ID = GR.TIME_ZONE_ID
    join {0}.TYPE_INTERVAL TL USING (TYPE_INTERVAL_ID)
    where GR.GR_WORK_DAY_ID = :P_GR_WORK_DAY_ID
    union
    select 2 RN, rownum as T, TIH.TIME_BEGIN||' - '||TIH.TIME_END||' ('||TL.TYPE_INTERVAL_NAME||')' F1, GR.NUMBER_DAY
    from {0}.GR_WORK_DAY GR
    JOIN {0}.TIME_INTERVAL TIH on TIH.TIME_ZONE_ID = GR.HOLIDAY_TIME_ZONE_ID
    join {0}.TYPE_INTERVAL TL USING (TYPE_INTERVAL_ID)
    where GR.GR_WORK_DAY_ID = :P_GR_WORK_DAY_ID
    union
    select 3 RN, rownum as T, TIC.TIME_BEGIN||' - '||TIC.TIME_END||' ('||TL.TYPE_INTERVAL_NAME||')' F1, GR.NUMBER_DAY
    from {0}.GR_WORK_DAY GR
    JOIN {0}.TIME_INTERVAL TIC on TIC.TIME_ZONE_ID = GR.COMPACT_TIME_ZONE_ID
    join {0}.TYPE_INTERVAL TL USING (TYPE_INTERVAL_ID)
    where GR.GR_WORK_DAY_ID = :P_GR_WORK_DAY_ID
    ORDER BY RN, F1
) V
pivot 
( max(F1) for RN in (1, 2, 3))