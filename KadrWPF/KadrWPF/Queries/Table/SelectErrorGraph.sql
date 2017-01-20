select PN.PNUM, 
    (select E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1)||'.' 
    from {0}.EMP E where TR.PER_NUM = E.PER_NUM) as FIO, 
    case when TR.SIGN_COMB = 0 then ' ' else 'X' end as COMB 
from {0}.PN_TMP PN
join {0}.TRANSFER TR on PN.TRANSFER_ID = TR.TRANSFER_ID
where PN.USER_NAME = :p_user_name and TR.GR_WORK_ID not in 
    (select AG.GR_WORK_ID from {0}.ACCESS_GR_WORK AG where AG.SUBDIV_ID = :p_subdiv_id)
order by PN.PNUM