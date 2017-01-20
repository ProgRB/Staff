select EMP_REGION_ID, TRANSFER_ID, REGION_SUBDIV_ID, DATE_START_WORK, DATE_END_WORK  
from {0}.EMP_REGION 
where transfer_id in ( select transfer_id from {0}.transfer 
	where worker_id=(select worker_id from {0}.transfer where transfer_id=:p_transfer_id))
and region_subdiv_id in (select region_subdiv_id from {0}.region_subdiv where subdiv_id=:p_subdiv_id)
order by date_start_work