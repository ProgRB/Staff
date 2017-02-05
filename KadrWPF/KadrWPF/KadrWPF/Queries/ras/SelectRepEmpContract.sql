select (S.CODE_SUBDIV) as CODE_SUBDIV, E.PER_NUM, 
    E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' as FIO,
    decode(T.SIGN_COMB,1,'X',null) COMB, trunc(T.DATE_END_CONTR) DATE_END_CONTR 
from {0}.EMP E
join {0}.TRANSFER T on E.PER_NUM = T.PER_NUM
join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID 
where T.SUBDIV_ID = :p_subdiv_id and T.SIGN_CUR_WORK = 1 and T.DATE_END_CONTR >= sysdate 
order by 2

