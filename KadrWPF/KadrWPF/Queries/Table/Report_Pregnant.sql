select CODE_SUBDIV, sum(case when PAY_TYPE_ID = 501 then 1 else null end) as v501, sum(case when PAY_TYPE_ID = 532 then 1 else null end) as v532
from 
(
    select S.CODE_SUBDIV, TR.PER_NUM, WP.PAY_TYPE_ID
    from {0}.WORKED_DAY WD
    join {0}.WORK_PAY_TYPE WP on WD.WORKED_DAY_ID = WP.WORKED_DAY_ID
    join {0}.TRANSFER TR on WD.TRANSFER_ID = TR.TRANSFER_ID
    join {0}.SUBDIV S on TR.SUBDIV_ID = S.SUBDIV_ID
    where WD.WORK_DATE = :p_date_begin and WP.PAY_TYPE_ID in (501, 532)
)
group by CODE_SUBDIV
order by CODE_SUBDIV