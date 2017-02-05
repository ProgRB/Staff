select count(*) FROM dual
where :p_date_work in (
    select C.CALENDAR_DAY from {0}.CALENDAR C
    where C.CALENDAR_DAY between sysdate and 
        (select min(C2.CALENDAR_DAY) from {0}.CALENDAR C2 
        where C2.CALENDAR_DAY > ( select min(C1.CALENDAR_DAY) from {0}.CALENDAR C1 where C1.CALENDAR_DAY > sysdate and C1.TYPE_DAY_ID in (1, 4)) 
            and C2.TYPE_DAY_ID in (2, 3)) 
        and C.TYPE_DAY_ID in (1, 4)
    ) 