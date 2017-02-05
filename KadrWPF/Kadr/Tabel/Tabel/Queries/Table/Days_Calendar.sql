SELECT Extract(Day from Calendar_Day), type_day_id FROM {0}.Calendar 
WHERE Calendar_Day between :p_date_begin and :p_date_end and Type_Day_ID in (1,3,4) 
ORDER BY Calendar_Day