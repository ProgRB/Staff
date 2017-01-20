select emp_last_name,emp_first_name,emp_middle_name,pos_name,
	   subdiv_id, code_subdiv,
	   to_char((select min(date_transfer) from {0}.transfer where connect_by_isleaf=1 start with transfer_id=:transfer_id connect by prior from_position=transfer_id),'DD-MM-YYYY')  date_hire,
	   t.per_num,t.sign_comb 
from {0}.emp e 
	 join {0}.transfer t  on (e.per_num=t.per_num) 
	 join {0}.subdiv using (subdiv_id)
	 left join {0}.position p on (p.pos_id=t.pos_id) 
	 left join {0}.degree d on (d.degree_id=t.degree_id) 
where (t.sign_cur_work=1 or type_transfer_id=3) and 
	  t.transfer_id in (select transfer_id from {0}.transfer start with transfer_id=:transfer_id connect by prior transfer_id=from_position)