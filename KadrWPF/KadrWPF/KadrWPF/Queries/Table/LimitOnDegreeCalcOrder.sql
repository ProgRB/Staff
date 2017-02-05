select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = V.SUBDIV_ID) as CODE_SUBDIV,    
        (select D.CODE_DEGREE from {0}.DEGREE D where V.DEGREE_ID = D.DEGREE_ID) as CODE_DEGREE, 
        V.PAY_TYPE_ID, V.LIMIT_HOURS_PLAN,
        nvl((
            select round(sum(WP.VALID_TIME/3600),2) as hours 
            from {0}.WORKED_DAY W
            join {0}.TRANSFER TR on W.TRANSFER_ID = TR.TRANSFER_ID
            join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
            where W.WORK_DATE between V.BEGIN_PERIOD and V.END_PERIOD
                and WP.PAY_TYPE_ID = V.PAY_TYPE_ID
                and TR.SUBDIV_ID = V.SUBDIV_ID and TR.SIGN_COMB = 0 and TR.DEGREE_ID = V.DEGREE_ID
            ) + ( 
            select sum(EO.COUNT_HOURS) from {0}.DATE_FOR_ORDER D 
            join {0}.EMP_FOR_ORDER EO on D.DATE_FOR_ORDER_ID = EO.DATE_FOR_ORDER_ID
            where D.ORDER_ON_HOLIDAY_ID =
            (select O.ORDER_ON_HOLIDAY_ID from {0}.ORDER_ON_HOLIDAY O 
            join {0}.DATE_FOR_ORDER DF on O.ORDER_ON_HOLIDAY_ID = DF.ORDER_ON_HOLIDAY_ID            
            where O.SUBDIV_ID = V.SUBDIV_ID and O.PAY_TYPE_ID = V.PAY_TYPE_ID and DF.DATE_WORK_ORDER = :p_date_work)
                and D.DATE_WORK_ORDER != :p_date_work),0) 
        as LIMIT_HOURS_FACT    
from
(
    select L.SUBDIV_ID, LD.DEGREE_ID, 
        L.PAY_TYPE_ID, sum(LD.LIMIT_HOURS_PLAN) as LIMIT_HOURS_PLAN,
        (select min(LIMIT_BEGIN) from {0}.LIMIT_ON_SUBDIV L1 
                where L1.SUBDIV_ID = :p_subdiv_id and L1.PAY_TYPE_ID = :p_pay_type_id and trunc(:p_date_work) between trunc(L1.LIMIT_BEGIN) and trunc(L1.LIMIT_END)) as begin_period,
        (select max(L1.LIMIT_END) from {0}.LIMIT_ON_SUBDIV L1 
                where L1.SUBDIV_ID = :p_subdiv_id and L1.PAY_TYPE_ID = :p_pay_type_id and trunc(:p_date_work) between trunc(L1.LIMIT_BEGIN) and trunc(L1.LIMIT_END)) as end_period
    from {0}.LIMIT_ON_SUBDIV L  
    join {0}.LIMIT_ON_DEGREE LD on L.LIMIT_ON_SUBDIV_ID = LD.LIMIT_ON_SUBDIV_ID
    where L.SUBDIV_ID = :p_subdiv_id and L.PAY_TYPE_ID = :p_pay_type_id and trunc(:p_date_work) between trunc(L.LIMIT_BEGIN) and trunc(L.LIMIT_END)
    group by L.SUBDIV_ID, LD.DEGREE_ID, L.PAY_TYPE_ID
) V