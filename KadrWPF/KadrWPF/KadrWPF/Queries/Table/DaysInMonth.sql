select count(*) from {0}.CALENDAR C 
where extract(year from C.CALENDAR_DAY) = :p_year and extract(month from C.CALENDAR_DAY) = :p_month and TYPE_DAY_ID in (:p_type_day_id)