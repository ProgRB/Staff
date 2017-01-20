WITH TP_PER AS (
        SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
            (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
                DECODE(sign_cur_work,1,DATE '3000-01-01', 
                    DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                        TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                               WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
               DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
               WORKER_ID, TR_DATE_ORDER
        FROM {0}.transfer t    
        WHERE T.DATE_HIRE <= :p_endPeriod and T.HIRE_SIGN = 1
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id),
    SUBD as 
    (select SUBDIV_ID, CODE_SUBDIV FROM {0}.SUBDIV
    start with subdiv_id in (
        select subdiv_id from {0}.access_subdiv 
        where upper(user_name) = ora_login_user and upper(app_name) = 'ACCOUNT')
    connect by prior subdiv_id = parent_id)
select (SELECT CODE_SUBDIV FROM SUBD S WHERE S.SUBDIV_ID = TP.SUBDIV_ID) CODE_SUBDIV,
    TP.PER_NUM, 
    (SELECT E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' 
    FROM {0}.EMP E WHERE E.PER_NUM = TP.PER_NUM) FIO,
    TP.SIGN_COMB, TRUNC(TP.DATE_TRANSFER) DATE_TRANSFER, TRUNC(TP.TR_DATE_ORDER) TR_DATE_ORDER
from (select * from TP_PER TP where TP.DATE_TRANSFER <= :p_endPeriod and TP.END_TRANSFER >= :p_beginPeriod) TP
where (TR_DATE_ORDER BETWEEN :p_beginPeriod and :p_endPeriod 
        /*or DATE_TRANSFER BETWEEN :p_beginPeriod and :p_endPeriod*/) and 
    TRUNC(DATE_TRANSFER,'MONTH') != TRUNC(TR_DATE_ORDER,'MONTH') and 
    TP.SUBDIV_ID in (select subdiv_id from SUBD)
ORDER BY CODE_SUBDIV, PER_NUM