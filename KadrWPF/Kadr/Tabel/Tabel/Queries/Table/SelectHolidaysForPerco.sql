SELECT CALENDAR_DAY, 
    DECODE(TYPE_HOLIDAY,0,'Праздник',1,'Предпраздничный день',2,'Рабочий выходной') DISPLAYNAME,
    TYPE_HOLIDAY,
    DECODE(TYPE_HOLIDAY,1,'1:00:00','0:00:00') PREF_TYPE_HOLIDAY,
    DECODE(TYPE_HOLIDAY,2,(SELECT MAX(C.CALENDAR_DAY) FROM {0}.CALENDAR C 
                                        WHERE C.CALENDAR_DAY < V.CALENDAR_DAY and C.TYPE_DAY_ID = 2)) SAT_SAN_DAY_ISWORK
FROM
(
    select CALENDAR_DAY, TYPE_DAY_ID,
        CASE WHEN TYPE_DAY_ID = 4 or TYPE_DAY_ID = 1 and DAY_TRANS is not null 
            THEN 0
            ELSE CASE WHEN TYPE_DAY_ID = 3 and TO_NUMBER(to_char(CALENDAR_DAY,'DAY', 'NLS_DATE_LANGUAGE=''numeric date language''')) not in (6,7) 
                        THEN 1
                        ELSE CASE WHEN TYPE_DAY_ID in (2,3) and TO_NUMBER(to_char(CALENDAR_DAY,'DAY', 'NLS_DATE_LANGUAGE=''numeric date language''')) in (6,7)
                                    THEN 2
                                END 
                    END
        END TYPE_HOLIDAY
    from {0}.CALENDAR
    where EXTRACT(YEAR FROM CALENDAR_DAY) = :p_YEAR
        and (TYPE_DAY_ID in (3,4) 
            or (TYPE_DAY_ID = 1 and DAY_TRANS is not null)
            or (TYPE_DAY_ID in (2,3) and TO_NUMBER(to_char(CALENDAR_DAY,'DAY', 'NLS_DATE_LANGUAGE=''numeric date language''')) in (6,7)))    
    order by CALENDAR_DAY
) V