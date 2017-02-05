select 
	code_subdiv,
	subdiv_name,
	ACCESS_GR_WORK_id
from APSTAFF.ACCESS_GR_WORK aa join APSTAFF.SUBDIV using (SUBDIV_ID)  
where aa.GR_WORK_ID = :p_GR_WORK_ID
order by 1