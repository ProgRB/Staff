with tree_sub as (select S.SUBDIV_ID from {0}.SUBDIV S 
        start with S.SUBDIV_ID = :p_subdiv_id connect by prior S.FROM_SUBDIV = S.SUBDIV_ID)
select sum(LD.LIMIT_HOURS_PLAN) as hours, '1' as signLimit    
from {0}.LIMIT_ON_SUBDIV L  
join {0}.LIMIT_ON_DEGREE LD on L.LIMIT_ON_SUBDIV_ID = LD.LIMIT_ON_SUBDIV_ID
where L.SUBDIV_ID in (select * from tree_sub) and L.PAY_TYPE_ID = :p_pay_type_id and LD.DEGREE_ID = :p_degree_id and 
    L.LIMIT_BEGIN >= (select min(L1.LIMIT_BEGIN) from {0}.LIMIT_ON_SUBDIV L1 
            where L1.SUBDIV_ID in (select * from tree_sub) 
                and L1.PAY_TYPE_ID = :p_pay_type_id and :p_date_doc between L1.LIMIT_BEGIN and L1.LIMIT_END) 
    and L.LIMIT_END <= (select max(L1.LIMIT_END) from {0}.LIMIT_ON_SUBDIV L1 
            where L1.SUBDIV_ID in (select * from tree_sub)
                and L1.PAY_TYPE_ID = :p_pay_type_id and :p_date_doc between L1.LIMIT_BEGIN and L1.LIMIT_END)
union
select round(sum(WP.VALID_TIME/3600),2) as hours, '2' as signLimit
from {0}.WORKED_DAY W
join {0}.TRANSFER TR on W.TRANSFER_ID = TR.TRANSFER_ID
join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
join {0}.CALENDAR C on W.WORK_DATE = C.CALENDAR_DAY
where W.WORK_DATE between 
    (select min(L1.LIMIT_BEGIN) from {0}.LIMIT_ON_SUBDIV L1 
            where L1.SUBDIV_ID in (select * from tree_sub)
                and L1.PAY_TYPE_ID = :p_pay_type_id and :p_date_doc between L1.LIMIT_BEGIN and L1.LIMIT_END) 
    and (select max(L1.LIMIT_END) from {0}.LIMIT_ON_SUBDIV L1 
            where L1.SUBDIV_ID in (select * from tree_sub)
                and L1.PAY_TYPE_ID = :p_pay_type_id and :p_date_doc between L1.LIMIT_BEGIN and L1.LIMIT_END)
    and /*Условие до 01.08.2013 WP.PAY_TYPE_ID = :p_pay_type_id*/
        /* Условие после 01.08.2013 - приказ об обнулении лимитов - смотрим также работу в счет отгула в лимитах*/
        (WP.PAY_TYPE_ID = :p_pay_type_id or (W.WORK_DATE > to_date('31.07.2013','dd.mm.yyyy') and
			W.WORK_DATE not between DATE '2013-09-01' and DATE '2013-09-30' 
            and WP.PAY_TYPE_ID = 303 and 
            ((:p_pay_type_id = 106 and C.TYPE_DAY_ID in (2,3)
                and not exists(select null from {0}.WORK_PAY_TYPE WP1 where WP1.WORKED_DAY_ID = W.WORKED_DAY_ID
                                    and WP1.PAY_TYPE_ID in (226, 237, 526, 243, 536, 215, 222, 622))) 
        or (:p_pay_type_id = 124 and C.TYPE_DAY_ID in (1,4)))))
    and WP.PAY_TYPE_ID = 
        (select D.PAY_TYPE_ID from {0}.REG_DOC R join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID 
        where R.REG_DOC_ID = WP.REG_DOC_ID)
    and TR.SUBDIV_ID in (select * from tree_sub) and TR.SIGN_COMB = 0 and TR.DEGREE_ID = :p_degree_id
    and not exists(select null from {0}.EMP_WAYBILL EW where EW.PER_NUM = TR.PER_NUM )
order by 2