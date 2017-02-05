select t.per_num,
		emp_last_name,emp_first_name,emp_middle_name,
		sign_comb,
		pos_name,
		degree_name,
		code_degree,
		first_value(salary) over (PARTITION BY ad.transfer_id order by change_date desc) as salary,
		t.transfer_id,
		t.subdiv_id
from 
	{0}.transfer t 
	join {0}.position p on (p.pos_id=t.pos_id)
	join {0}.emp e on (e.per_num=t.per_num)
	join {0}.degree d on (t.degree_id=d.degree_id)
	left join {0}.account_data ad on (ad.transfer_id=t.transfer_id)
	
where t.transfer_id=:p_transfer_id