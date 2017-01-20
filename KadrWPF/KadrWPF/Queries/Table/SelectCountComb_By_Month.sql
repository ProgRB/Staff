select "1" C_1, "2" C_2, "3" C_3, "4" C_4, "5" C_5, "6" C_6,
    "7" C_7, "8" C_8, "9" C_9, "10" C_10, "11" C_11, "12" C_12
from (
    select N_MONTH, COUNT(DISTINCT V2.PER_NUM) COUNT_NUMBER
    from (
        select T.PER_NUM, extract(MONTH from ST.START_PERIOD) N_MONTH
        from {1}.SALARY_FROM_TABLE ST
        join {0}.TRANSFER T on T.TRANSFER_ID = ST.TRANSFER_ID
        where ST.APP_NAME_ID = 1 and ST.END_PERIOD between :p_beginDate and :p_endDate
            AND ST.END_PERIOD = LAST_DAY(ST.END_PERIOD)
            and ST.SIGN_COMB = 1 and ST.SIGN_APPENDIX = 1 and ST.DEGREE_ID is not null
    ) V2
    GROUP BY N_MONTH
) V2
PIVOT (SUM(COUNT_NUMBER) FOR N_MONTH in (1,2,3,4,5,6,7,8,9,10,11,12))