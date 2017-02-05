select S.SUBDIV_ID, S.CODE_SUBDIV, S.SUBDIV_NAME
from {0}.SUBDIV S 
where exists(select null from {0}.SUBDIV_FOR_TABLE ST where S.SUBDIV_ID = ST.SUBDIV_ID)  
order by S.CODE_SUBDIV