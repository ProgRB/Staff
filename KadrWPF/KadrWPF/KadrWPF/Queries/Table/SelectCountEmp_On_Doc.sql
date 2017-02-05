select CASE WHEN C_1 > 0 THEN C_1 END C_1, CASE WHEN C_2 > 0 THEN C_2 END C_2,
    CASE WHEN C_3 > 0 THEN C_3 END C_3, CASE WHEN C_4 > 0 THEN C_4 END C_4,
    CASE WHEN C_5 > 0 THEN C_5 END C_5, CASE WHEN C_6 > 0 THEN C_6 END C_6,
    CASE WHEN C_7 > 0 THEN C_7 END C_7, CASE WHEN C_8 > 0 THEN C_8 END C_8,
    CASE WHEN C_9 > 0 THEN C_9 END C_9, CASE WHEN C_10 > 0 THEN C_10 END C_10,
    CASE WHEN C_11 > 0 THEN C_11 END C_11, CASE WHEN C_12 > 0 THEN C_12 END C_12,
    CASE WHEN C_YEAR > 0 THEN C_YEAR END C_YEAR,
    CASE WHEN C_KV1 > 0 THEN C_KV1 END C_KV1, CASE WHEN C_KV2 > 0 THEN C_KV2 END C_KV2,
    CASE WHEN C_KV3 > 0 THEN C_KV3 END C_KV3, CASE WHEN C_KV4 > 0 THEN C_KV4 END C_KV4
from (
    select 
        count(distinct case when N_MONTH = 1 then WORKER_ID end) C_1,
        count(distinct case when N_MONTH = 2 then WORKER_ID end) C_2,
        count(distinct case when N_MONTH = 3 then WORKER_ID end) C_3,
        count(distinct case when N_MONTH = 4 then WORKER_ID end) C_4,
        count(distinct case when N_MONTH = 5 then WORKER_ID end) C_5,
        count(distinct case when N_MONTH = 6 then WORKER_ID end) C_6,
        count(distinct case when N_MONTH = 7 then WORKER_ID end) C_7,
        count(distinct case when N_MONTH = 8 then WORKER_ID end) C_8,
        count(distinct case when N_MONTH = 9 then WORKER_ID end) C_9,
        count(distinct case when N_MONTH = 10 then WORKER_ID end) C_10,
        count(distinct case when N_MONTH = 11 then WORKER_ID end) C_11,
        count(distinct case when N_MONTH = 12 then WORKER_ID end) C_12,
        count(distinct case when N_MONTH between 1 and 12 then WORKER_ID end) C_Year,
        count(distinct case when N_MONTH between 1 and 3 then WORKER_ID end) C_KV1,
        count(distinct case when N_MONTH between 4 and 6 then WORKER_ID end) C_KV2,
        count(distinct case when N_MONTH between 7 and 9 then WORKER_ID end) C_KV3,
        count(distinct case when N_MONTH between 10 and 12 then WORKER_ID end) C_KV4
    from (
        select V.*, T.WORKER_ID
        from 
            (select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, extract(MONTH from WORK_DATE) N_MONTH
            from {0}.WORKED_DAY WD
            join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.REG_DOC_ID > 0) 
            join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
            join (select C.CALENDAR_DAY from {0}.CALENDAR C
                    where C.CALENDAR_DAY BETWEEN :p_beginDate and :p_endDate 
                        and C.CALENDAR_DAY = LAST_DAY(C.CALENDAR_DAY)) C1 on WORK_DATE = C1.CALENDAR_DAY
            where WD.WORK_DATE between :p_beginDate and :p_endDate and WP.PAY_TYPE_ID = :p_pay_type_id                                 
            group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID) V
        join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID 
        where T.SIGN_COMB = 0
    ) V2
)