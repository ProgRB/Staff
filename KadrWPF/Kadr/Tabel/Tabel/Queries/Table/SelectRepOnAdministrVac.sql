select DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,'Итого') CODE_SUBDIV,
    DECODE(sum(C_PN),0,null,sum(C_PN)) C_PN,DECODE(sum(C_WD),0,null,sum(C_WD)) C_WD,
    DECODE(sum(C_PNKV1),0,null,sum(C_PNKV1)) C_PNKV1,DECODE(sum(C_WDKV1),0,null,sum(C_WDKV1)) C_WDKV1,
    DECODE(sum(C_PNKV2),0,null,sum(C_PNKV2)) C_PNKV2,DECODE(sum(C_WDKV2),0,null,sum(C_WDKV2)) C_WDKV2,
    DECODE(sum(C_PNYear1),0,null,sum(C_PNYear1)) C_PNYear1,
    DECODE(sum(C_WDYear1),0,null,sum(C_WDYear1)) C_WDYear1,
    DECODE(sum(C_PNKV3),0,null,sum(C_PNKV3)) C_PNKV3,DECODE(sum(C_WDKV3),0,null,sum(C_WDKV3)) C_WDKV3,
    DECODE(sum(C_PNKV4),0,null,sum(C_PNKV4)) C_PNKV4,DECODE(sum(C_WDKV4),0,null,sum(C_WDKV4)) C_WDKV4,
    DECODE(sum(C_PNYear2),0,null,sum(C_PNYear2)) C_PNYear2,
    DECODE(sum(C_WDYear2),0,null,sum(C_WDYear2)) C_WDYear2,
    DECODE(sum(C_PNYear),0,null,sum(C_PNYear)) C_PNYear,
    DECODE(sum(C_WDYear),0,null,sum(C_WDYear)) C_WDYear 
from 
(
    select CODE_SUBDIV,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then TRANS_ID end) C_PN,
        sum(case when V2.WORK_DATE between :p_beginDate and :p_endDate then 1 end) C_WD,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 3 then TRANS_ID end) C_PNKV1,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 3 then 1 else 0 end) C_WDKV1,
        count(distinct case when extract(month from V2.WORK_DATE) between 4 and 6 then TRANS_ID end) C_PNKV2,
        sum(case when extract(month from V2.WORK_DATE) between 4 and 6 then 1 else 0 end) C_WDKV2,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 6 then TRANS_ID end) C_PNYear1,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 6 then 1 else 0 end) C_WDYear1,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 9 then TRANS_ID end) C_PNKV3,
        sum(case when extract(month from V2.WORK_DATE) between 7 and 9 then 1 else 0 end) C_WDKV3,
        count(distinct case when extract(month from V2.WORK_DATE) between 10 and 12 then TRANS_ID end) C_PNKV4,
        sum(case when extract(month from V2.WORK_DATE) between 10 and 12 then 1 else 0 end) C_WDKV4,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 12 then TRANS_ID end) C_PNYear2,
        sum(case when extract(month from V2.WORK_DATE) between 7 and 12 then 1 else 0 end) C_WDYear2,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 12 then TRANS_ID end) C_PNYear,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 12 then 1 else 0 end) C_WDYear
    from (
        select V2.*, (select max(transfer_id) keep (dense_rank last order by date_transfer) from {0}.TRANSFER where worker_id=v2.worker_id) TRANS_ID 
        from (
            select S.CODE_SUBDIV, T.WORKER_ID, V.* from 
            (   select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, sum(WP.VALID_TIME) as VALID_TIME, 
                    sum(WD.FROM_GRAPH) as FROM_GRAPH
                from {0}.WORK_PAY_TYPE WP
                join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
                join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
                join {0}.DOC_LIST DL on (RD.DOC_LIST_ID = DL.DOC_LIST_ID)    
                where WD.WORK_DATE between TRUNC(:p_beginDate,'YEAR') and :p_endDate and WP.PAY_TYPE_ID = :p_pay_type_id
                    and DL.DOC_NOTE = :p_note and WP.VALID_TIME > 0               
                group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID) V
            join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
            where T.SIGN_COMB = 0 and ((S.SUBDIV_ID = :p_subdiv_id and :p_subdiv_id != 0) or :p_subdiv_id = 0)
                AND V.VALID_TIME = V.FROM_GRAPH - 
                nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                    where V.WORKED_DAY_ID = WP3.WORKED_DAY_ID and WP3.PAY_TYPE_ID = 114),0)) V2
    ) V2    
    group by CODE_SUBDIV
)
group by rollup(CODE_SUBDIV)