select V.TRANSFER_ID, WORKER_ID, V.CODE_SUBDIV, V.PER_NUM, V.EMP_LAST_NAME, V.EMP_FIRST_NAME, V.EMP_MIDDLE_NAME,
    decode(V.countP, 0, null, V.COUNTP) COUNTP,
	decode(V.countPAll, 0, null, V.COUNTPALL) COUNTPALL,
	V.COMB, V.POS_NAME 
from
    (select 
        T.TRANSFER_ID, T.WORKER_ID, (S.CODE_SUBDIV) as code_subdiv,
        E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME, 
        (select count(*) from {0}.PERMIT PT
        where PT.PER_NUM = E.PER_NUM 
			and DATE_START_PERMIT <= TRUNC(SYSDATE)
			and DATE_END_PERMIT >= TRUNC(SYSDATE)
			and PT.TRANSFER_ID in
                (select T1.TRANSFER_ID from {0}.TRANSFER T1 WHERE T1.WORKER_ID = T.WORKER_ID) 
        ) as countP, 
		(select count(*) from {0}.PERMIT PT
        where PT.PER_NUM = E.PER_NUM and PT.TRANSFER_ID in
                (select T1.TRANSFER_ID from {0}.TRANSFER T1 WHERE T1.WORKER_ID = T.WORKER_ID) 
        ) as countPAll,
		case when T.SIGN_COMB = 0 then '' else 'X' end COMB, P.POS_NAME
    from {0}.EMP E
    join {0}.TRANSFER T on (E.PER_NUM = T.PER_NUM)
    join {0}.SUBDIV S on (S.SUBDIV_ID = T.SUBDIV_ID)
    join {0}.POSITION P on (T.POS_ID = P.POS_ID)
    where T.SIGN_CUR_WORK = 1 {1}
    order by  E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME) V