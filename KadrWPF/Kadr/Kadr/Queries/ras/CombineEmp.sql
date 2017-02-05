select rownum,a.* from 
(select  s.code_subdiv,t.per_num, 
	emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio,
	code_degree, NULLIF(end_transfer,date'3000-01-01') end_transfer
	from 
		{0}.transfer t join {0}.emp e on (e.per_num=t.per_num)
		join {0}.transfer_periods tp on (t.transfer_id=tp.transfer_id)
		join {0}.degree d on (d.degree_id=t.degree_id)
		join {0}.subdiv s on (s.subdiv_id=t.subdiv_id)
	where date'{1}' between tp.date_transfer and tp.end_transfer and t.sign_comb=1
	order by code_subdiv,per_num
) a