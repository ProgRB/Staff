select DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,'Итого') CODE_SUBDIV,
    DECODE(sum(C_WD),0,null,sum(C_WD)) C_WD,
    DECODE(sum(C_WDKV1),0,null,sum(C_WDKV1)) C_WDKV1, DECODE(sum(C_WDKV2),0,null,sum(C_WDKV2)) C_WDKV2,
    DECODE(sum(C_WDYear1),0,null,sum(C_WDYear1)) C_WDYear1,
    DECODE(sum(C_WDKV3),0,null,sum(C_WDKV3)) C_WDKV3, DECODE(sum(C_WDKV4),0,null,sum(C_WDKV4)) C_WDKV4,
    DECODE(sum(C_WDYear2),0,null,sum(C_WDYear2)) C_WDYear2,
    DECODE(sum(C_WDYear),0,null,sum(C_WDYear)) C_WDYear 
from 
(
    select CODE_SUBDIV,
        sum(case when V2.WORK_DATE between :p_beginDate and :p_endDate then 1 end) C_WD,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 3 then 1 else 0 end) C_WDKV1,
        sum(case when extract(month from V2.WORK_DATE) between 4 and 6 then 1 else 0 end) C_WDKV2,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 6 then 1 else 0 end) C_WDYear1,
        sum(case when extract(month from V2.WORK_DATE) between 7 and 9 then 1 else 0 end) C_WDKV3,
        sum(case when extract(month from V2.WORK_DATE) between 10 and 12 then 1 else 0 end) C_WDKV4,
        sum(case when extract(month from V2.WORK_DATE) between 7 and 12 then 1 else 0 end) C_WDYear2,
        sum(case when extract(month from V2.WORK_DATE) between 1 and 12 then 1 else 0 end) C_WDYear
    from (
        select V2.*, 
            (select TRANSFER_ID from {0}.TRANSFER T0 where CONNECT_BY_ISLEAF = 1
            start with T0.TRANSFER_ID = V2.TRANSFER_ID 
            connect by nocycle T0.TRANSFER_ID = prior T0.FROM_POSITION) TRANS_ID
        from (
            select S.CODE_SUBDIV, V.* 
            from 
                (select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID
                from {0}.WORKED_DAY WD
                join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
                join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)   
                where WD.WORK_DATE between TRUNC(:p_beginDate,'YEAR') and :p_endDate and WP.PAY_TYPE_ID = :p_pay_type_id                                 
                group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID) V
            join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
            where T.SIGN_COMB = 0 and ((S.SUBDIV_ID = :p_subdiv_id and :p_subdiv_id != 0) or :p_subdiv_id = 0)) V2
    ) V2    
    group by CODE_SUBDIV
)
group by rollup(CODE_SUBDIV)