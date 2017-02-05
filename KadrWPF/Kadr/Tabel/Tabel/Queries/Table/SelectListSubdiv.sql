select S.SUBDIV_ID, S.CODE_SUBDIV, SUB_ACTUAL_SIGN 
from 
(
    select distinct T.SUBDIV_ID 
    from {1}.SALARY_FROM_TABLE ST
    join {0}.TRANSFER T on T.TRANSFER_ID = ST.TRANSFER_ID
    where ST.APP_NAME_ID = 1 and 
		((ST.START_PERIOD<=:p_end_date and ST.END_PERIOD>=:p_begin_date) or ST.END_PERIOD = :p_end_date)
) V
join {0}.SUBDIV S on V.SUBDIV_ID = S.SUBDIV_ID
order by S.CODE_SUBDIV