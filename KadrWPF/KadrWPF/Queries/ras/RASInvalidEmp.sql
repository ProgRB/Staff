with Tr_per as 
    (select V.TRANSFER_ID, V.PER_NUM, V.DATE_TRANSFER, V.END_TRANSFER, V.SIGN_COMB, 
        V.DEGREE_ID,V.POS_ID, TYPE_TRANSFER_ID, FROM_POSITION, FORM_PAY, SUBDIV_ID
    from (
        SELECT transfer_id,per_num,WORKER_ID, degree_id,POS_ID,subdiv_id,SIGN_COMB,trunc(date_transfer) date_transfer,
            LEAD(trunc(date_transfer),1, DECODE(type_transfer_id,3,TRUNC(date_transfer)+1,date'3000-01-01')) 
            OVER (PARTITION BY WORKER_ID ORDER BY date_transfer)-1/86400 end_transfer, TYPE_TRANSFER_ID, FROM_POSITION, FORM_PAY
        FROM {0}.TRANSFER T
        START WITH T.SIGN_CUR_WORK = 1 or T.TYPE_TRANSFER_ID = 3
        CONNECT BY NOCYCLE PRIOR T.FROM_POSITION = T.TRANSFER_ID) V
    WHERE end_transfer>=trunc(:p_date_print,'month') and date_transfer<last_day(:p_date_print))
select rownum,a.* 
from 
    (select distinct (select s.code_subdiv from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) code_subdiv,
        t.per_num,
        (select emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.'
        from {0}.EMP E where E.PER_NUM = T.PER_NUM) fio,
        (select code_degree from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) code_degree, 
        DECODE(TYPE_TRANSFER_ID,3,TRUNC(END_TRANSFER)) end_transfer, 
        EP.DATE_START_PRIV, EP.DATE_END_PRIV
    from (select DISTINCT MAX(T.PER_NUM) KEEP(DENSE_RANK LAST ORDER BY end_transfer) OVER(PARTITION BY PER_NUM) PER_NUM,
            MAX(T.TYPE_TRANSFER_ID) KEEP(DENSE_RANK LAST ORDER BY end_transfer) OVER(PARTITION BY PER_NUM) TYPE_TRANSFER_ID,
            MAX(T.end_transfer) KEEP(DENSE_RANK LAST ORDER BY end_transfer) OVER(PARTITION BY PER_NUM) END_TRANSFER,
            MAX(T.SUBDIV_ID) KEEP(DENSE_RANK LAST ORDER BY end_transfer) OVER(PARTITION BY PER_NUM) SUBDIV_ID,
            MAX(T.DEGREE_ID) KEEP(DENSE_RANK LAST ORDER BY end_transfer) OVER(PARTITION BY PER_NUM) DEGREE_ID            
        FROM Tr_per T) t 
    join {0}.emp e on (e.per_num=t.per_num)
    join {0}.emp_priv ep on (ep.per_num=t.per_num)
    join {0}.TYPE_PRIV tpr on (TPR.TYPE_PRIV_ID=EP.TYPE_PRIV_ID)
    where TPR.SIGN_INVALID=1 and 
        trunc(:p_date_print) between TRUNC(EP.DATE_START_PRIV,'MONTH') and 
            nvl2(EP.DATE_END_PRIV, TRUNC(EP.DATE_END_PRIV,'MONTH')-1, :p_date_print)
    order by code_subdiv,per_num) a