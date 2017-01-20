select rownum,a.* from 
(select 
    DECODE(grouping(t.per_num),1,'бяецн он ондп. ','')||code_subdiv,
    t.per_num,
    DECODE(grouping(t.per_num),1,to_char(count(*)),emp_last_name||' '||emp_first_name||' '||emp_middle_name) fio/*,
    DECODE(SIGN_COMB,1,'X') COMB*/
    FROM
        (WITH TP_PER AS (
            SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
                    DECODE(sign_cur_work,1,DATE '3000-01-01', 
                        DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                            TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                                   WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
                   DATE_END_CONTR,TYPE_TRANSFER_ID,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, DATE_HIRE,
                   WORKER_ID 
            FROM {0}.transfer t    
            WHERE T.DATE_HIRE <= :p_date_end and T.HIRE_SIGN = 1 and T.SIGN_COMB = 0
            START WITH sign_cur_work = 1 OR type_transfer_id = 3
            CONNECT BY NOCYCLE PRIOR from_position = transfer_id)
        select TR.PER_NUM, DATE_HIRE, TRANSFER_ID,FROM_POSITION,TYPE_TRANSFER_ID,
            TR.SUBDIV_ID, TR.WORKER_ID, SIGN_COMB
        from (select * from tp_per WHERE :p_date_end between DATE_TRANSFER and END_TRANSFER) tr 
        where TR.DATE_TRANSFER = 
                    (select max(date_transfer) from tp_per tr5 where TR5.WORKER_ID = TR.WORKER_ID
                        and TR5.DATE_TRANSFER <= :p_date_end) ) t 
    join {0}.emp e on (e.per_num=t.per_num)
    join {0}.subdiv s on (s.subdiv_id=t.subdiv_id)
    where {0}.GET_SIGN_PROFUNION(T.WORKER_id, :p_date_end)={1}
    group by rollup (code_subdiv,(t.per_num,emp_last_name,emp_first_name,emp_middle_name, SIGN_COMB))
) a
/*select rownum,a.* from 
(select 
	DECODE(grouping(t.per_num),1,'бяецн он ондп. ','')||code_subdiv,
	t.per_num,
	DECODE(grouping(t.per_num),1,to_char(count(*)),emp_last_name||' '||emp_first_name||' '||emp_middle_name) fio
	from {0}.transfer t join {0}.emp e on (e.per_num=t.per_num)
	join {0}.per_data pd on (pd.per_num=t.per_num)
	join {0}.subdiv s on (s.subdiv_id=t.subdiv_id)
	where t.sign_cur_work=1 and NVL(sign_comb,0)=0
		and NVL(sign_profunion,0)={1}
	group by rollup (code_subdiv,(t.per_num,emp_last_name,emp_first_name,emp_middle_name))
) a*/