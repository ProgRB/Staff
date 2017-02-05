select rownum,a.* from 
(select 
    DECODE(grouping(t.per_num),1,'ВСЕГО ПО ПОДР. ','')||code_subdiv,
    t.per_num,
    DECODE(grouping(t.per_num),1,to_char(count(*)),emp_last_name||' '||emp_first_name||' '||emp_middle_name) fio
    FROM
        (
        select T.PER_NUM, DATE_HIRE, T.TRANSFER_ID,FROM_POSITION,TYPE_TRANSFER_ID,
            T.SUBDIV_ID, T.WORKER_ID
        from (select R.TRANSFER_ID from {1}.RETENTION R
            where R.PAYMENT_TYPE_ID = (select PT.PAYMENT_TYPE_ID FROM {1}.PAYMENT_TYPE PT where PT.PAY_TYPE_ID = 283) and
                /*сюда подставляем поле: DATE_START_RET или DATE_END_RET в зависимости от отчета*/
                R.{2} between TRUNC(:p_date_end,'MONTH') and :p_date_end) TR 
        join {0}.TRANSFER t on (T.TRANSFER_ID = TR.TRANSFER_ID)
		WHERE T.SIGN_COMB = 0) t 
    join {0}.emp e on (e.per_num=t.per_num)
    join {0}.subdiv s on (s.subdiv_id=t.subdiv_id)
    group by rollup (code_subdiv,(t.per_num,emp_last_name,emp_first_name,emp_middle_name))
) a