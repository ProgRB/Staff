select case when GROUPING(V3.CODE_SUBDIV) = 0 
            then case when GROUPING(V3.PER_NUM) = 0 
                        then V3.CODE_SUBDIV
                        else '�����: '||to_char(count(C_WD))
                    end 
            else '����� '||to_char(count(C_WD))
         end as CODE_SUBDIV,
    case when GROUPING(V3.PER_NUM) = 0 
            then V3.FIO
            else ''
         end as FIO,
    case when GROUPING(V3.PER_NUM) = 0 
            then V3.PER_NUM
            else ''
         end as PER_NUM, 
    DECODE(sum(C_WD),0,null,sum(C_WD)) C_WD, DECODE(sum(C_INC),0,null,sum(C_INC)) C_INC/*,
    DECODE(sum(C_WDYear1),0,null,sum(C_WDYear1)) C_WDYear1,DECODE(sum(C_INCYear1),0,null,sum(C_INCYear1)) C_INCYear1,
    DECODE(sum(C_WDYear2),0,null,sum(C_WDYear2)) C_WDYear2,DECODE(sum(C_INCYear2),0,null,sum(C_INCYear2)) C_INCYear2,
    DECODE(sum(C_WDYear1+C_WDYear2),0,null,sum(C_WDYear1+C_WDYear2)) C_WDYear,
    DECODE(sum(C_INCYear1+C_INCYear2),0,null,sum(C_INCYear1+C_INCYear2)) C_INCYear*/
from 
( 
    select CODE_SUBDIV, E.PER_NUM, 
        E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then TRANS_ID end) C_PN,
        sum(case when V2.WORK_DATE between :p_beginDate and :p_endDate then 1 end) C_WD,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then REG_DOC_ID end) C_INC/*,                
        sum(case when extract(month from V2.WORK_DATE) between 1 and 6 then 1 else 0 end) C_WDYear1,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 6 then REG_DOC_ID end) C_INCYear1, 
        sum(case when extract(month from V2.WORK_DATE) between 7 and 12 then 1 else 0 end) C_WDYear2,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 12 then REG_DOC_ID end) C_INCYear2*/
    from (
        select V2.*, (select TRANSFER_ID from {0}.TRANSFER where SIGN_CUR_WORK = 1 or TYPE_TRANSFER_ID = 3 
            START WITH TRANSFER_ID = V2.TRANSFER_ID 
            CONNECT BY NOCYCLE PRIOR TRANSFER_ID = FROM_POSITION OR TRANSFER_ID = PRIOR FROM_POSITION) TRANS_ID 
        from (
            select S.CODE_SUBDIV, V.* from 
            (   select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, RD.REG_DOC_ID, RD.PER_NUM
                from {0}.WORK_PAY_TYPE WP
                join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
                join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
                where WD.WORK_DATE between /*TRUNC(:p_beginDate,'YEAR')*/:p_beginDate and :p_endDate and 
					WP.PAY_TYPE_ID = :p_PAY_TYPE_ID                             
                group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, RD.REG_DOC_ID, RD.PER_NUM) V
            join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
            where T.SIGN_COMB = 0
        ) V2
    ) V2    
    join {0}.EMP E on V2.PER_NUM = E.PER_NUM
    group by CODE_SUBDIV, E.PER_NUM,E.EMP_LAST_NAME,E.EMP_FIRST_NAME,E.EMP_MIDDLE_NAME
) V3
group by rollup(V3.CODE_SUBDIV, (V3.FIO, V3.PER_NUM))