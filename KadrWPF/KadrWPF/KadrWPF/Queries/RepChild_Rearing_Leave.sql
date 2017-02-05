WITH TP_PER AS (
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
select DISTINCT FIRST_VALUE(TP.CODE_SUBDIV) OVER(PARTITION BY WORKER_ID ORDER BY DATE_TRANSFER DESC) CODE_SUBDIV,
    V.PER_NUM, 
    (select E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.'
    from {0}.EMP E where V.PER_NUM = E.PER_NUM) FIO,
    (select TRIM(to_char(INSURANCE_NUM, '000g000g000g00', 'nls_numeric_characters=.-')) INSURANCE_NUM 
    from {0}.PER_DATA P where V.PER_NUM = P.PER_NUM) INSURANCE_NUM,
    DATE_BEGIN_LEAVE_1_5, LEAST(DATE_END_LEAVE_1_5, TRUNC(END_TRANSFER)) DATE_END_LEAVE_1_5, 
	DATE_BEGIN_LEAVE_3, LEAST(DATE_END_LEAVE_3, TRUNC(END_TRANSFER)) DATE_END_LEAVE_3, NOTE
from 
(
    select PER_NUM, 1 RN, DATE_BEGIN_LEAVE DATE_BEGIN_LEAVE_1_5, 
        DECODE(NVL(CRL.BREAKPOINT_LEAVE,0),0,CRL.DATE_END_LEAVE,CRL.DATE_DEPARTURE-1) DATE_END_LEAVE_1_5, null DATE_BEGIN_LEAVE_3, 
        null DATE_END_LEAVE_3, 'дерх' NOTE
    from {0}.CHILD_CARE_LEAVE CRL
    JOIN {0}.RELATIVE using(RELATIVE_ID)
    where CRL.DATE_BEGIN_LEAVE <= :p_endPeriod and 
        DECODE(NVL(CRL.BREAKPOINT_LEAVE,0),0,CRL.DATE_END_LEAVE,CRL.DATE_DEPARTURE-1) >= :p_beginPeriod 
        and CRL.SIGN_LEAVE_1_5_YEARS = 1
    union 
    select PER_NUM, 2 RN, null DATE_BEGIN_LEAVE_1_5, null DATE_END_LEAVE_1_5, 
        CRL.DATE_BEGIN_LEAVE DATE_BEGIN_LEAVE_3, 
        DECODE(NVL(CRL.BREAKPOINT_LEAVE,0),0,CRL.DATE_END_LEAVE,CRL.DATE_DEPARTURE-1) DATE_END_LEAVE_3, 'дкдерх' NOTE
    from {0}.CHILD_CARE_LEAVE CRL
    JOIN {0}.RELATIVE using(RELATIVE_ID)
    where CRL.DATE_BEGIN_LEAVE <= :p_endPeriod and 
        DECODE(NVL(CRL.BREAKPOINT_LEAVE,0),0,CRL.DATE_END_LEAVE,CRL.DATE_DEPARTURE-1) >= :p_beginPeriod
        and CRL.SIGN_LEAVE_1_5_YEARS = 0
) V
left join (select * from TP_PER) TP 
    ON TP.PER_NUM = V.PER_NUM and TP.DATE_TRANSFER <= :p_endPeriod and TP.END_TRANSFER >= :p_beginPeriod
order by CODE_SUBDIV, PER_NUM, NOTE