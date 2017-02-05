declare
k number;
begin
	k:={0}.temp_salary_id_seq.nextval;
	for item in 
	(select 
    		MAX(transfer_id) KEEP (DENSE_RANK LAST ORDER BY date_transfer) as transfer_id,
			per_num
        from 
        (
            select
            t.transfer_id,  
            (select transfer_id from {0}.transfer where sign_cur_work=1 or type_transfer_id=3 start with transfer_id=t.transfer_id connect by prior transfer_id=from_position) as last_transfer_id,
            date_transfer,
            code_degree, 
            t.per_num,     
            emp_last_name,
            emp_first_name,
            emp_middle_name from 
                (
                        select subdiv_id,transfer_id, degree_id, per_num, trunc(date_transfer) date_transfer, 
                            DECODE(type_transfer_id,3,trunc(date_transfer)+86399/86400, lead(trunc(date_transfer)-1/86400,1,date'3000-01-01') over (PARTITION BY per_num,sign_comb order by date_transfer)) as end_transfer
                        from {0}.transfer
                ) t 
            join {0}.emp e on (e.per_num=t.per_num)
            left join {0}.degree d on (t.degree_id=d.degree_id)
            where subdiv_id=:p_subdiv_id and date_transfer<= :p_date_end and end_transfer>=:p_date_begin
        )
    group by last_transfer_id,per_num, emp_last_name, emp_first_name, emp_middle_name)
    loop
    	{0}.calc_salary(:p_subdiv_id, trunc(:p_date_begin,'month'), add_months(trunc(:p_date_end,'month'),1)-1/86400, item.transfer_id,1, k);
    end loop;
    open :c for select t.per_num, e.emp_last_name||' '||substr(e.emp_first_name,1,1)||'.'||substr(e.emp_middle_name,1,1)||'.' fio1,
         e1.emp_last_name||' '||substr(e1.emp_first_name,1,1)||'.'||substr(e1.emp_middle_name,1,1)||'.' fio1, t1.per_num, 
         repl_percent, to_char(repl_start,'dd.mm.yyyy')||'- '||to_char(repl_end,'dd.mm.yyyy') as period,
         NVL(repl_sal, (select salary from {0}.account_data where transfer_id=re.replacing_transfer_id)) repl_sal, 
		 round(sum(time),2), round(sum(sum),3), code_degree, order_name 
        from
            {0}.repl_emp  re
            join {0}.transfer t on (t.transfer_id=re.transfer_id)
            join {0}.emp e on (e.per_num=t.per_num)
            join {0}.transfer t1 on (t1.transfer_id=re.replacing_transfer_id)
            join {0}.emp e1 on (e1.per_num=t1.per_num)
            join (select * from {0}.temp_salary where temp_salary_id=k and time>0 and pay_type_id='153')  ts on (t.per_num=ts.per_num and re.repl_start<=TS.END_PERIOD and re.repl_end>=TS.START_PERIOD)
            join {0}.transfer tr on (tr.transfer_id=ts.transfer_id)
        where tr.subdiv_id=:p_subdiv_id and re.repl_start<=:p_date_end and re.repl_end>=:p_date_begin 
		group by t.per_num, e.emp_last_name,e.emp_first_name,e.emp_middle_name,e1.emp_last_name,e1.emp_first_name,e1.emp_middle_name, t1.per_num,
			repl_percent, repl_start, repl_end, repl_sal,  replacing_transfer_id,code_degree, order_name
        order by e.emp_last_name, t.per_num;
end;