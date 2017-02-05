/*МСФО Демографическая и фин. статистика по всем сотрудникам компании по состоянию на дату*/
SELECT PER_NUM, EMP_BIRTH_DATE, EMP_SEX, DATE_HIRE,
    /* 26.06.2015 добавляем условие, что если среднемесячная зп = 0, 
    то считаем оклад * на районный коэфф и надбавку за стаж*/
    ROUND(DECODE(AVG_SALARY,0,OKL*1.4+{0}.GET_CALC_EXP_ADD(101, OKL, TRANSFER_ID, :p_date_end),AVG_SALARY),2) AVG_SALARY,
    OKL
FROM (
	with TP_PER AS (
                SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
                        DECODE(sign_cur_work,1,DATE '3000-01-01', 
                            DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                                TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                                       WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
                       DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
                       CONNECT_BY_ROOT(TRANSFER_ID) as LAST_TRANS, WORKER_ID
                FROM {0}.transfer t    
                WHERE T.DATE_HIRE <= :p_date_end and T.HIRE_SIGN = 1 and T.SIGN_COMB = 0
                START WITH sign_cur_work = 1 OR type_transfer_id = 3
                CONNECT BY NOCYCLE PRIOR from_position = transfer_id),    
        EM as (
            SELECT V.PER_NUM, V.EMP_BIRTH_DATE, V.EMP_SEX, V.DATE_HIRE, V.WORKER_ID,
                --(select {0}.AVG_SALARY_TO_DAY(V.transfer_id, :p_date_end)*29.3 from dual) AVG_SALARY,
                round(NVL(A.SALARY,0)*B.TARIFF) OKL, V.TRANSFER_ID
            FROM
                (
                select E.PER_NUM, E.EMP_BIRTH_DATE, E.EMP_SEX, DATE_HIRE, TRANSFER_ID,FROM_POSITION,TYPE_TRANSFER_ID,
                    WORKER_ID
                from {0}.emp e
                join (select * from tp_per) tr 
                    on (e.PER_NUM = tr.PER_NUM and :p_date_end between TR.DATE_TRANSFER and END_TRANSFER)  
                /*where TR.DATE_TRANSFER = 
                            (select max(date_transfer) from tp_per tr5 where TR5.LAST_TRANS = TR.LAST_TRANS
                                and TR5.DATE_TRANSFER <= :p_date_end)*/ ) V
            JOIN {0}.ACCOUNT_DATA A 
                ON DECODE(V.TYPE_TRANSFER_ID, 3, V.FROM_POSITION, V.TRANSFER_ID) = A.TRANSFER_ID
            JOIN (SELECT BT.TARIFF, BT.BDATE, LEAD(BT.BDATE-1/86400,1,DATE '3000-01-01') OVER(ORDER BY BT.BDATE) EDATE 
                    FROM {0}.BASE_TARIFF BT) B
                ON :p_date_end between B.BDATE AND B.EDATE)
    select * from EM 
    left join (select X WORKER_ID, ROUND(Y,2) AVG_SALARY 
                from TABLE({0}.AVG_SALARY_TO_DAY_EMPS((select CAST(COLLECT(WORKER_ID) AS SALARY.Number_collection_type) from EM ), :p_date_end))) AV 
        on EM.WORKER_ID = AV.WORKER_ID
)
ORDER BY PER_NUM