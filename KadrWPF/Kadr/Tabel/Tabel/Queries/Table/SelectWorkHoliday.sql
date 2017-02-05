select rownum, PAY_TYPE_ID as "Вид оплат", hours as "Часы" 
from
(
    select WP.PAY_TYPE_ID, round(sum(WP.VALID_TIME/3600),2) as hours 
    from {0}.WORKED_DAY W
    join {0}.TRANSFER TR on W.TRANSFER_ID = TR.TRANSFER_ID
    join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
    where W.WORK_DATE between :p_date_begin and :p_date_end and WP.PAY_TYPE_ID in (121, 124)
        and TR.SUBDIV_ID = :p_subdiv_id
    group by rollup (WP.PAY_TYPE_ID)
) V