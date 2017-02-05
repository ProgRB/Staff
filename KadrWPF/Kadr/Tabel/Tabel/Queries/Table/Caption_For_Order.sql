select pos_name,substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'. '|| emp_last_name fio 
from {0}.transfer t join 
	 {0}.emp e on (e.per_num=t.per_num) join 
	 {0}.position p on (p.pos_id=t.pos_id) join 
	 {0}.account_data ad on (ad.transfer_id=t.transfer_id) 
where sign_cur_work=1 and t.subdiv_id = {1}		
order by salary desc