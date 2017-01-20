select case when GROUPING(V3.CODE_SUBDIV) = 0 
            then case when GROUPING(V3.PER_NUM) = 0 
                        then V3.CODE_SUBDIV
                        else 'хрнцн: '||to_char(count(C_WD))
                    end 
            else 'бяецн '||to_char(count(C_WD))
         end as CODE_SUBDIV,
    case when GROUPING(V3.PER_NUM) = 0 
            then V3.FIO
            else ''
         end as FIO,
    case when GROUPING(V3.PER_NUM) = 0 
            then V3.PER_NUM
            else ''
         end as PER_NUM, 
    DECODE(sum(C_WD),0,null,sum(C_WD)) C_WD, DECODE(sum(C_INC),0,null,sum(C_INC)) C_INC
from 
(
    select CODE_SUBDIV, E.PER_NUM,  
        E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then TRANS_ID end) C_PN,
        sum(case when V2.WORK_DATE between :p_beginDate and :p_endDate then 1 end) C_WD,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then REG_DOC_ID end) C_INC
    from (
        select V2.*, (select max(transfer_id) keep (dense_rank last order by date_transfer)  from {0}.TRANSFER where worker_id=V2.WORKER_ID) TRANS_ID 
        from (
            select S.CODE_SUBDIV, T.WORKER_ID, V.* from 
            (select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID, RD.REG_DOC_ID, RD.PER_NUM,
					sum(WP.VALID_TIME) as VALID_TIME, sum(WD.FROM_GRAPH) as FROM_GRAPH
                from {0}.WORK_PAY_TYPE WP
                join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
                join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
                join {0}.DOC_LIST DL on (RD.DOC_LIST_ID = DL.DOC_LIST_ID)    
                where WD.WORK_DATE between /*TRUNC(:p_beginDate,'YEAR')*/:p_beginDate and :p_endDate and 
					WP.PAY_TYPE_ID = :p_pay_type_id
                    and DL.DOC_NOTE = :p_note and WP.VALID_TIME > 0               
                group by WD.WORK_DATE, WD.TRANSFER_ID, RD.REG_DOC_ID, RD.PER_NUM, WD.WORKED_DAY_ID) V
            join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
            where T.SIGN_COMB = 0 --and ((S.SUBDIV_ID = :p_subdiv_id and :p_subdiv_id != 0) or :p_subdiv_id = 0)
                AND V.VALID_TIME = V.FROM_GRAPH - 
                nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                    where V.WORKED_DAY_ID = WP3.WORKED_DAY_ID and WP3.PAY_TYPE_ID = 114),0)) V2
    ) V2    
    join {0}.EMP E on V2.PER_NUM = E.PER_NUM
    group by CODE_SUBDIV, E.PER_NUM,E.EMP_LAST_NAME,E.EMP_FIRST_NAME,E.EMP_MIDDLE_NAME
) V3
group by rollup(V3.CODE_SUBDIV, (V3.FIO, V3.PER_NUM))