select V.LIMIT_ON_SUBDIV_ID, 
    case when GROUPING(V.LIMIT_ON_SUBDIV_ID) = 0 
            then V.LIMIT_NUMBER_DOC
            else 'Итого'
         end as "№ приказа", 
    V.LIMIT_DATE_DOC as "Дата приказа", V.LIMIT_BEGIN as "Начало периода", V.LIMIT_END as "Оконч. периода", 
    sum(V.L_PLAN) as "Всего часов", round(sum(V.L_PLAN) / Months_between(V.LIMIT_END+1,V.LIMIT_BEGIN),2)  as "Часов на мес."
from
(select L.LIMIT_ON_SUBDIV_ID, L.LIMIT_NUMBER_DOC, L.LIMIT_DATE_DOC, L.LIMIT_BEGIN, L.LIMIT_END, 
    (select sum(LD.LIMIT_HOURS_PLAN) from {0}.LIMIT_ON_DEGREE LD where LD.LIMIT_ON_SUBDIV_ID = L.LIMIT_ON_SUBDIV_ID) as L_PLAN
from {0}.LIMIT_ON_SUBDIV L
where L.SUBDIV_ID in 
    (select S.SUBDIV_ID from {0}.SUBDIV S 
        start with S.SUBDIV_ID = :p_subdiv_id connect by prior S.FROM_SUBDIV = S.SUBDIV_ID)
    and L.PAY_TYPE_ID = :p_pay_type_id and extract(year from L.LIMIT_END) = :p_year
) V 
group by rollup ((V.LIMIT_ON_SUBDIV_ID, V.LIMIT_NUMBER_DOC, V.LIMIT_DATE_DOC, 
    V.LIMIT_BEGIN, V.LIMIT_END, V.L_PLAN))
order by V.LIMIT_BEGIN