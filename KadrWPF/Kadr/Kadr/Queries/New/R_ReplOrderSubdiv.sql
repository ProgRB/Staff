declare
begin
	open :c1 for 
		select '  '||LISTAGG(t,'; ') WITHIN GROUP (order by 1) text1
		FROM 
		(
				select repl_name||' '||LISTAGG( case when type_repl_id=6 then '' else {0}.PADEG.CASE_WORD(emp_last_name,'Родительный','last_name',emp_sex)||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' end,', ') 
                WITHIN GROUP(order by emp_last_name) t
                from 
                (
                    select distinct repl_name||case when re.type_repl_id=6 then ' штатной единицы' end repl_name, re.type_repl_id, emp_last_name, emp_first_name,emp_middle_name, emp_sex
                        from {0}.repl_emp re 
                        left join {0}.type_repl tr on (tr.type_repl_id=NVL(re.type_repl_id,1))
                        join {0}.transfer t on (t.transfer_id=re.replacing_transfer_id)
                        join {0}.emp e on (t.per_num=e.per_num)
                    where repl_emp_id member of :p_repl_id
                ) a
                group by repl_name
		 );
	open :c2 for
		select e1.emp_last_name||' '||substr(e1.emp_first_name,1,1)||'.'||substr(e1.emp_middle_name,1,1)||'.('||e1.per_num||')' fio,
				p1.pos_name pos_name,
			   case when re.type_repl_id =6 then 'Вакантная единица' else 
					e2.emp_last_name||' '||substr(e2.emp_first_name,1,1)||'.'||substr(e2.emp_middle_name,1,1)||'.('||e2.per_num||')' end fio1,
				p2.pos_name pos_name1,
			   to_char(re.repl_start,'DD.MM.YYYY')||'-'||to_char(repl_end,'DD.MM.YYYY') text_period,
			   repl_percent percent,
			   sign_combine,
			   repl_sal
		FROM
			{0}.repl_emp re 
			join {0}.transfer t on (t.transfer_id=re.transfer_id)
			join {0}.emp e1 on (t.per_num=e1.per_num)
			join {0}.position p1 on (t.pos_id=p1.pos_id)
			left join {0}.transfer t1 on (t1.transfer_id=re.replacing_transfer_id)
			left join {0}.emp e2 on (t1.per_num=e2.per_num)
			left join {0}.position p2 on (p2.pos_id=t1.pos_id)
			where repl_emp_id member of :p_repl_id
		order by 1;
end;