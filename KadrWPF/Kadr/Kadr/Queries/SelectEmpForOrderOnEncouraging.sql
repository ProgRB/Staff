WITH TP_PER AS (
    SELECT * FROM (    
        SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER,
                DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                    LEAD(TRUNC(DATE_TRANSFER) - 1 / 86000,1,DATE '3000-01-01') OVER(PARTITION BY WORKER_ID ORDER BY DATE_TRANSFER))
                end_transfer,
                DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
                WORKER_ID, TR_DATE_ORDER
        FROM {0}.transfer t    
        WHERE T.DATE_HIRE <= :p_date and T.HIRE_SIGN = 1
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) TP
    where TP.DATE_TRANSFER <= :p_date and TP.END_TRANSFER >= :p_date)
select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
	(select S.SUBDIV_NAME from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) SUBDIV_NAME,
    E.PER_NUM, 
    E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
    DECODE(T.SIGN_COMB,1,'X') COMB, T.SIGN_COMB,
    (SELECT P.POS_NAME FROM {0}.POSITION P where P.POS_ID = T.POS_ID) POS_NAME,
    E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME
from {0}.EMP E
join (select * from TP_PER ) T on E.PER_NUM = T.PER_NUM
order by CODE_SUBDIV, PER_NUM