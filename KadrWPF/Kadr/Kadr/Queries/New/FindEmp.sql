select e.per_num,
		decode(sign_comb, 1, 'X', '') sign_comb,
		emp_last_name||' '|| emp_first_name ||' '|| emp_middle_name fio,
		emp_last_name, emp_first_name, emp_middle_name,
		p.pos_name,
		t.transfer_id
from {0}.emp e 
	join {0}.transfer t on (t.per_num=e.per_num) 
	left join {0}.position p on (t.pos_id=p.pos_id) 
where t.sign_cur_work=1 and t.per_num like '%'||:p_per_num||'%'
and UPPER(emp_last_name) like UPPER('%'||:p_emp_last_name||'%')
and UPPER(emp_first_name) like UPPER('%'||:p_emp_first_name||'%')
and UPPER(emp_middle_name) like UPPER('%'||:p_emp_middle_name||'%')