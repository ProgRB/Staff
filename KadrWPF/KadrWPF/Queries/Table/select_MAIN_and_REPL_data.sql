declare 
begin
	if (:p_repl_emp_id is not null) then
		open :c for  
             select repl_emp_id,t1.per_num,e1.emp_last_name||' '||e1.emp_first_name||' '||e1.emp_middle_name fio,
                p1.pos_name,
                t1.transfer_id,
                replacing_transfer_id,
                repl_start,repl_end,repl_order, repl_actual_sign, 
				date_order,type_repl_id,SIGN_COMBINE, REPL_SAL,REPL_PERCENT,SIGN_LONGTIME,SIGN_LOCK_REPL,
                e2.emp_last_name||' '||e2.emp_first_name||' '||e2.emp_middle_name fio2,
                e2.per_num per_num2,
                p2.pos_name pos_name2
            from  {0}.repl_emp re 
                join {0}.transfer t1 on (re.transfer_id=t1.transfer_id) 
                join {0}.position p1 on (p1.pos_id=t1.pos_id) 
                join {0}.emp e1 on (e1.per_num=t1.per_num) 
                left join {0}.transfer t2 on (t2.transfer_id=re.replacing_transfer_id) 
                left join {0}.emp e2 on (e2.per_num=t2.per_num) 
                left join {0}.position p2 on (p2.pos_id=t2.pos_id)
            where repl_emp_id=:p_repl_emp_id;
     ELSIF :p_transfer_id is not null then 
       open :c  for 
       	select null as repl_emp_id, per_num, emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
         pos_name,
         t.transfer_id,
         null as replacing_transfer_id,
         to_date(null) as repl_start,
         to_date(null) as repl_end,
         null as repl_order, 
         to_date(null) as date_order,
         to_number(null) as type_repl_id,
         :p_sign_combine as sign_combine,
         to_number(null) as repl_sal,
		 to_number(null) as repl_actual_sign,
         to_number(null) as repl_percent,
         0 as sign_longtime,
         0 as  SIGN_LOCK_REPL,
         null as fio2,
         null as per_num2,
         null as pos_name2
       from {0}.transfer t join {0}.position p using (pos_id) join {0}.emp using (per_num) where transfer_id = :p_transfer_id;
     ELSIF :p_replacing_transfer_id is not null then
        open :c  for 
       	select null as repl_emp_id, per_num as per_num2, emp_last_name||' '||emp_first_name||' '||emp_middle_name fio2,
         pos_name as pos_name2,
         to_number(null) as transfer_id,
         t.transfer_id as replacing_transfer_id,
         to_date(null) as repl_start,
         to_date(null) as repl_end,
         null as repl_order, 
         to_date(null) as date_order,
         to_number(null) as type_repl_id,
         :p_sign_combine as sign_combine,
         to_number(null) as repl_sal,
		 to_number(null) as repl_actual_sign,
         to_number(null) as repl_percent,
         0 as sign_longtime,
         0 as  SIGN_LOCK_REPL,
         null as fio,
         null as per_num,
         null as pos_name
       from {0}.transfer t join {0}.position p using (pos_id) join {0}.emp using (per_num) where transfer_id = :p_replacing_transfer_id;
     END IF;
END;