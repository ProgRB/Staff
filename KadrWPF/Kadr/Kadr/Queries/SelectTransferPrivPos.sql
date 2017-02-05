select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    T.PER_NUM, E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1) ||'.' FIO,
    (select P1.POS_NAME||T1.POS_NOTE from {0}.TRANSFER T1 
    join {0}.POSITION P1 on P1.POS_ID = T1.POS_ID
    where T1.TRANSFER_ID = T.FROM_POSITION) POS_NAME_OLD,
    (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T.POS_ID)||T.POS_NOTE POS_NAME,
    PP2.KPS
from {0}.TRANSFER T
join {0}.EMP E on T.PER_NUM = E.PER_NUM
join {0}.PRIVILEGED_POSITION PP on T.SUBDIV_ID = PP.SUBDIV_ID and T.POS_ID = PP.POS_ID 
join {0}.ACCOUNT_DATA A on T.TRANSFER_ID = A.TRANSFER_ID
left join {0}.PRIVILEGED_POSITION PP2 on A.PRIVILEGED_POSITION_ID = PP2.PRIVILEGED_POSITION_ID
where (T.DATE_TRANSFER between :p_beginPeriod and :p_endPeriod or 
    T.TR_DATE_ORDER between :p_beginPeriod and :p_endPeriod) and T.TYPE_TRANSFER_ID in (1,2)
order by 1,2
    
