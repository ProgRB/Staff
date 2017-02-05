select DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,'Итого') CODE_SUBDIV,
    DECODE(sum(C_PN),0,null,sum(C_PN)) C_PN,
    DECODE(sum(C_PNKV1),0,null,sum(C_PNKV1)) C_PNKV1, DECODE(sum(C_PNKV2),0,null,sum(C_PNKV2)) C_PNKV2,
    DECODE(sum(C_PNYear1),0,null,sum(C_PNYear1)) C_PNYear1,
    DECODE(sum(C_PNKV3),0,null,sum(C_PNKV3)) C_PNKV3, DECODE(sum(C_PNKV4),0,null,sum(C_PNKV4)) C_PNKV4,
    DECODE(sum(C_PNYear2),0,null,sum(C_PNYear2)) C_PNYear2,
    DECODE(sum(C_PNYear),0,null,sum(C_PNYear)) C_PNYear
from 
(
    select CODE_SUBDIV,
        count(distinct case when V2.WORK_DATE between :p_beginDate and :p_endDate then TRANS_ID end) C_PN,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 3 then TRANS_ID end) C_PNKV1,
        count(distinct case when extract(month from V2.WORK_DATE) between 4 and 6 then TRANS_ID end) C_PNKV2,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 6 then TRANS_ID end) C_PNYear1,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 9 then TRANS_ID end) C_PNKV3,
        count(distinct case when extract(month from V2.WORK_DATE) between 10 and 12 then TRANS_ID end) C_PNKV4,
        count(distinct case when extract(month from V2.WORK_DATE) between 7 and 12 then TRANS_ID end) C_PNYear2,
        count(distinct case when extract(month from V2.WORK_DATE) between 1 and 12 then TRANS_ID end) C_PNYear
    from (
		select S.CODE_SUBDIV, V.*, T.WORKER_ID TRANS_ID
		from 
			(select WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID
			from {0}.WORKED_DAY WD
			join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID) 
			join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
			join (select C.CALENDAR_DAY from {0}.CALENDAR C
					where C.CALENDAR_DAY BETWEEN TRUNC(:p_beginDate,'YEAR') and :p_endDate 
						and C.CALENDAR_DAY = LAST_DAY(C.CALENDAR_DAY)) C1 on WORK_DATE = C1.CALENDAR_DAY
			where WD.WORK_DATE between TRUNC(:p_beginDate,'YEAR') and :p_endDate and WP.PAY_TYPE_ID = :p_pay_type_id                                 
			group by WD.WORK_DATE, WD.TRANSFER_ID, WD.WORKED_DAY_ID) V
		join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
		join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID   
		where T.SIGN_COMB = 0 and ((S.SUBDIV_ID = :p_subdiv_id and :p_subdiv_id != 0) or :p_subdiv_id = 0)
	) V2
    group by CODE_SUBDIV
)
group by rollup(CODE_SUBDIV)