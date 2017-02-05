select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = L.SUBDIV_ID) as CODE_SUBDIV, 
    (select D.CODE_DEGREE from {0}.DEGREE D where LD.DEGREE_ID = D.DEGREE_ID) as CODE_DEGREE, 
    L.PAY_TYPE_ID, sum(LD.LIMIT_HOURS_PLAN) as LIMIT_HOURS_PLAN,
    nvl((
        select round(sum(WP.VALID_TIME/3600),2) as hours 
        from {0}.WORKED_DAY W
        join {0}.CALENDAR C on W.WORK_DATE = C.CALENDAR_DAY
        join {0}.TRANSFER TR on W.TRANSFER_ID = TR.TRANSFER_ID
        join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
        where W.WORK_DATE between :p_date_BEGIN and :p_date_END 
            and WP.PAY_TYPE_ID = L.PAY_TYPE_ID
            and TR.SUBDIV_ID = L.SUBDIV_ID and TR.SIGN_COMB = 0 and TR.DEGREE_ID = LD.DEGREE_ID
    ),0) as LIMIT_HOURS_FACT    
from {0}.LIMIT_ON_SUBDIV L  
join {0}.LIMIT_ON_DEGREE LD on L.LIMIT_ON_SUBDIV_ID = LD.LIMIT_ON_SUBDIV_ID
where L.SUBDIV_ID = :p_subdiv_id and :p_begin_limit <= L.LIMIT_END and :p_end_limit >= L.LIMIT_BEGIN
group by L.SUBDIV_ID, LD.DEGREE_ID, L.PAY_TYPE_ID
order by L.PAY_TYPE_ID, 2