Select 
	t.per_num,
	e.emp_last_name||' '||substr(e.emp_first_name,1,1)||'.'||substr(e.emp_middle_name,1,1)||'.' fio,
	p1.pos_name,
	case when re.type_repl_id in (6) then null else e1.per_num end per_num, 
	case when re.type_repl_id in (6) then 'Вакантная единица'
	else
		e1.emp_last_name||' '||substr(e1.emp_first_name,1,1)||'.'||substr(e1.emp_middle_name,1,1)||'.' 
	end fio1,
	p2.pos_name as pos_name1,
    repl_order code_order,
	REPL_NAME text,
    to_char(repl_start,'DD.MM.YYYY')||'-'||NVL(to_char(repl_end,'DD.MM.YYYY'),'(по табелю)') text_period,
	repl_percent percent,
	repl_sal
from {0}.repl_emp re 
	join {0}.transfer t on (t.transfer_id=re.transfer_id) 
	join {0}.emp e on (e.per_num=t.per_num)  
	left join {0}.position p1 on (t.pos_id=p1.pos_id)  
	join {0}.transfer t1 on (re.replacing_transfer_id=t1.transfer_id) 
	left join {0}.position p2 on (p2.pos_id=t1.pos_id)  
	left join {0}.emp e1 on (e1.per_num=t1.per_num)  
	left join {0}.type_repl tre on (re.type_repl_id=tre.type_repl_id)  
where repl_emp_id in ({1}) 
	--and sign_combine=1