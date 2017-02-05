select DISTINCT MIN(CAL_DATE) OVER (PARTITION BY GR_HOL,GR_15) D_BEGIN, 
    MAX(CAL_DATE) OVER (PARTITION BY GR_HOL,GR_15) D_END, GR_15
from 
(
    SELECT Extract(Day from Calendar_Day) CAL_DATE,Extract(Day from Calendar_Day)-15-rownum GR_HOL, 
        case when Extract(Day from Calendar_Day) < 16 then 0 else 1 end as GR_15   
    FROM {0}.Calendar 
    WHERE Calendar_Day between :D1 and :D2 and Type_Day_ID in (1,4) 
    ORDER BY Calendar_Day
) V