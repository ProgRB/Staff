WITH TP_PER AS (
    SELECT * FROM (    
        SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER,
               /*DECODE(sign_cur_work,1,DATE '3000-01-01', 
                    DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                        TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                               WHERE from_position = t.transfer_id)) - 1 / 86000))*/
                DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                    LEAD(TRUNC(DATE_TRANSFER) - 1 / 86000,1,DATE '3000-01-01') OVER(PARTITION BY WORKER_ID ORDER BY DATE_TRANSFER))
                end_transfer,
                DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
                WORKER_ID, TR_DATE_ORDER
        FROM {0}.transfer t    
        WHERE T.DATE_HIRE <= :p_end_date and T.HIRE_SIGN = 1
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) TP
    where TP.DATE_TRANSFER <= :p_end_date and TP.END_TRANSFER >= :p_begin_date)
select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    E.PER_NUM, 
    E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
    E.EMP_BIRTH_DATE, YS.DATE_BEGIN_ADD PERIOD_BEGIN, YS.DATE_END_ADD PERIOD_END,
	SUM_PAYMENT SUM_SAL,
    (SELECT P.POS_NAME FROM {0}.POSITION P where P.POS_ID = T.POS_ID) POS_NAME
from {0}.EMP E
join (select * from TP_PER ) T on E.PER_NUM = T.PER_NUM
join {0}.YOUNG_SPECIALIST YS ON (E.PER_NUM=YS.PER_NUM)
where NVL(YS.DATE_BEGIN_ADD,DATE '1000-01-01') <= :p_end_date and 
    NVL(YS.DATE_END_ADD, DATE '3000-01-01') >= :p_begin_date
    /*17.11.2015 добавил это условие, чтобы не выводились трочки переводов, 
				где работник уже не молодой специалист*/
	and DATE_TRANSFER <= NVL(YS.DATE_END_ADD, DATE '3000-01-01') and
    END_TRANSFER >= NVL(YS.DATE_BEGIN_ADD,DATE '1000-01-01')
	and {1} /* для основной базы ничего не вставляем, а для архивной - not*/ 
	exists(select null from {0}.TRANSFER T1 
			WHERE T1.PER_NUM = E.PER_NUM and T1.SIGN_CUR_WORK = 1)