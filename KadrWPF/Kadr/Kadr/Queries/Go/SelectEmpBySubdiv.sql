select 
	worker_id,
	per_num,
	decode(sign_comb,1,'X','') sign_comb,
	emp_last_name||' '||substr(emp_first_name,1,1)||'. '||substr(emp_middle_name,1,1)||'.' fio,
	max(code_degree) KEEP (dense_rank last order by date_transfer) code_degree,
	max(pos_name) keep(dense_rank last order by date_transfer) pos_name,
    max(transfer_id) keep(dense_rank last order by date_transfer) transfer_id
from
	(select worker_id, transfer_id, trunc(date_transfer) date_transfer, 
    		per_num, sign_comb,
    	decode(type_transfer_id,3, trunc(date_transfer)+86399/86400, lead(trunc(date_transfer)-1/86400, 1, date'3000-01-01') over (partition by worker_id order by date_transfer)) as end_transfer,
        	pos_id, subdiv_id, degree_id from {0}.transfer) t
    join {0}.degree using (degree_id)
    join {0}.POSITION using (pos_id)
    join {0}.emp using (per_num)
where subdiv_id=:p_subdiv_id and :p_date between date_transfer and end_transfer
group by worker_id, per_num, sign_comb, emp_last_name, emp_first_name, emp_middle_name
order by FIO
    