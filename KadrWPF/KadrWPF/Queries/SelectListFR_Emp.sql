select * from 
 (select em.PERCO_SYNC_ID, initcap(em.FR_LAST_NAME) as FR_LAST_NAME, initcap(em.FR_FIRST_NAME) as FR_FIRST_NAME, initcap(em.FR_MIDDLE_NAME) as FR_MIDDLE_NAME,
 (select SUBDIV_NAME from {0}.subdiv where SUBDIV_ID = em.SUBDIV_ID) as SUBDIV_NAME, 
 (select POS_NAME from {0}.position pos where em.POS_ID = pos.POS_ID) as POS_NAME, 
 em.FR_DATE_START as FR_DATE_START, em.FR_DATE_END as FR_DATE_END  
from {0}.fr_emp em {1}) 
emp_link {2}

/*
0 schema
1 where...
2 sort.ToString()
*/