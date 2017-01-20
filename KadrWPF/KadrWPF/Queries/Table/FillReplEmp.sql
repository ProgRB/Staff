select 
	repl_emp_id,
	SIGN_LOCK_REPL,
	t.per_num,
	e.emp_last_name||' '||substr(e.emp_first_name,1,1)||'.'||substr(e.emp_middle_name,1,1)||'.' fio,
	REPL_START,
	REPL_END,
	repl_sal "Оклад",
	repl_percent "Процент замещения",
	repl_order
from 
	{0}.repl_emp re 
	join {0}.transfer t on (t.transfer_id=DECODE(NVL(:whos_repl,0),0,re.transfer_id,re.replacing_transfer_id))
	join {0}.emp e on (e.per_num=t.per_num)
where DECODE(NVL(:whos_repl,0),0,re.replacing_transfer_id,re.transfer_id)
 in (select transfer_id from {0}.transfer start with transfer_id =:repl_tr_id connect by NOCYCLE prior transfer_id=from_position or transfer_id=prior from_position)
 and sign_combine=NVL(:sign_combine,0)
order by REPL_START desc
