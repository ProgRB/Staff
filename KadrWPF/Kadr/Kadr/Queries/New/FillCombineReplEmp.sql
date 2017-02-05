select 
	repl_emp_id,
	t.transfer_id,
	emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio,
	t.per_num,
	repl_percent||'%' "Процент совместителя",
	REPL_START,
	repl_end,
	repl_order,
	repl_name "Вид совмещения"
from 
	{0}.repl_emp re 
	join {0}.transfer t on (t.transfer_id=re.transfer_id)
	join {0}.emp e on (e.per_num=t.per_num)
	left join {0}.type_repl tr on (tr.type_repl_id=re.type_repl_id)
where replacing_transfer_id in (select transfer_id from {0}.transfer start with transfer_id=:repl_tr_id connect by NOCYCLE prior transfer_id=from_position or transfer_id=prior from_position) and sign_combine=1
order by REPL_START desc