select null ACCESS_TEMPL_BY_SUBDIV_ID, null ID_SHABLON_MAIN, S.SUBDIV_ID, S.CODE_SUBDIV, S.SUBDIV_NAME
from {0}.SUBDIV S 
where S.SUB_ACTUAL_SIGN = 1 and nvl(S.PARENT_ID,0) = 0 and nvl(S.WORK_TYPE_ID,0) != 7 /* (1,2,3,4,5,6) */
    and S.CODE_SUBDIV != 0 and S.CODE_SUBDIV not like '5%'
    and not exists(select null from {0}.ACCESS_TEMPL_BY_SUBDIV ST
                where S.SUBDIV_ID = ST.SUBDIV_ID and ST.ID_SHABLON_MAIN = :p_ID_SHABLON_MAIN)  
order by S.CODE_SUBDIV