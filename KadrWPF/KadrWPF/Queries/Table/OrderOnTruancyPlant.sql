select DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,'По предприятию') CODE_SUBDIV,
    DECODE(sum(C_PN),0,null,sum(C_PN)) C_PN, DECODE(sum(C_WD),0,null,sum(C_WD)) C_WD,
    DECODE(sum(C_INC),0,null,sum(C_INC)) C_INC,
    DECODE(sum(C_WDYear1),0,null,sum(C_WDYear1)) C_WDYear1,DECODE(sum(C_INCYear1),0,null,sum(C_INCYear1)) C_INCYear1,
    DECODE(sum(C_WDYear2),0,null,sum(C_WDYear2)) C_WDYear2,DECODE(sum(C_INCYear2),0,null,sum(C_INCYear2)) C_INCYear2,
    DECODE(sum(C_WDYear1+C_WDYear2),0,null,sum(C_WDYear1+C_WDYear2)) C_WDYear,
    DECODE(sum(C_INCYear1+C_INCYear2),0,null,sum(C_INCYear1+C_INCYear2)) C_INCYear
from 
( 
    select CODE_SUBDIV,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then TRANS_ID end) C_PN,
        sum(case when V2.WORK_DATE between :p_beginDate and :p_endDate then 1 end) C_WD,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then REG_DOC_ID end) C_INC,                
        sum(case when extract(month from V2.WORK_DATE) between 1 and 6 then 1 else 0 end) C_WDYear1,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 6 then REG_DOC_ID end) C_INCYear1, 
        sum(case when extract(month from V2.WORK_DATE) between 7 and 12 then 1 else 0 end) C_WDYear2,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 12 then REG_DOC_ID end) C_INCYear2
    from (
        select V2.*, (select TRANSFER_ID from {0}.TRANSFER where SIGN_CUR_WORK = 1 or TYPE_TRANSFER_ID = 3 
            START WITH TRANSFER_ID = V2.TRANSFER_ID 
            CONNECT BY NOCYCLE PRIOR TRANSFER_ID = FROM_POSITION OR TRANSFER_ID = PRIOR FROM_POSITION) TRANS_ID 
        from (
            select S.CODE_SUBDIV, V.* from 
            (   select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, RD.REG_DOC_ID
                from {0}.WORK_PAY_TYPE WP
                join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
                join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
                join {0}.DOC_LIST DL on (RD.DOC_LIST_ID = DL.DOC_LIST_ID)    
                where WD.WORK_DATE between TRUNC(:p_beginDate,'YEAR') and :p_endDate and WP.PAY_TYPE_ID = 533                             
                group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, RD.REG_DOC_ID) V
            join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
            where T.SIGN_COMB = 0
                ) V2
    ) V2    
    group by CODE_SUBDIV
)
group by rollup(CODE_SUBDIV)