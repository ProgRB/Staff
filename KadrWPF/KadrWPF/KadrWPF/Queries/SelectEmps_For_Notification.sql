with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR')
        connect by prior subdiv_id = parent_id),
    TP_PER AS (
        SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
            (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
                DECODE(sign_cur_work,1,DATE '3000-01-01', 
                    DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                        TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                               WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
               DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
               WORKER_ID 
        FROM {0}.transfer t    
        WHERE T.DATE_HIRE <= :p_endPeriod and T.HIRE_SIGN = 1 and T.SIGN_COMB = 0
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id)
select T.PER_NUM, 
    E.EMP_LAST_NAME||' '||E.EMP_FIRST_NAME||' '||E.EMP_MIDDLE_NAME FIO,
    {0}.PADEG.CASE_WORD(E.EMP_LAST_NAME, 'Дательный', 'LAST_NAME', E.EMP_SEX) EMP_LAST_NAME,
    {0}.PADEG.CASE_WORD(E.EMP_FIRST_NAME, 'Дательный', 'FIRST_NAME', E.EMP_SEX) EMP_FIRST_NAME, 
    {0}.PADEG.CASE_WORD(E.EMP_MIDDLE_NAME, 'Дательный', 'MIDDLE_NAME', E.EMP_SEX) EMP_MIDDLE_NAME
from (select * from TP_PER TP
        WHERE TP.DATE_TRANSFER <= :p_endPeriod and TP.END_TRANSFER >= :p_beginPeriod) T
join {0}.EMP E on T.PER_NUM = E.PER_NUM
where T.SUBDIV_ID in (SELECT SUBDIV_ID FROM SUBD) and T.SUBDIV_ID = NVL(:p_SUBDIV_ID,T.SUBDIV_ID)
ORDER BY FIO