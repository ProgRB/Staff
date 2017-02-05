select
	t.transfer_id,
	code_pos,
	pos_name,
	code_subdiv,
	per_num,
	decode(sign_comb, 1, 'X') SIGN_COMB,
	INITCAP(emp_last_name) emp_last_name,
	INITCAP(emp_first_name) emp_first_name, 
	INITCAP(emp_middle_name) emp_middle_name,
	classific,
	salary,
	code_tariff_grid,
	date_transfer
from
	apstaff.transfer t
	join (select transfer_id from apstaff.transfer_periods where :p_date is null or :p_date between date_transfer and end_transfer) t1 on (t.transfer_id=t1.transfer_id)
	join apstaff.emp using (per_num)
	join apstaff.position using (pos_id)
	join apstaff.subdiv using (subdiv_id)
	join (select code_tariff_grid, classific, salary, transfer_id 
			from apstaff.account_data left join apstaff.tariff_grid using (tariff_grid_id)
		) ad on (decode(t.type_transfer_id, 3, t.from_position, t.transfer_id) = ad.transfer_id)
where
	(:p_per_num is null or per_num=lpad(:p_per_num,5, '0'))
	and (:p_subdiv_id is null or :p_subdiv_id = subdiv_id)
	and (:p_fio is null or EMP_LAST_NAME||' '||EMP_FIRST_NAME||' '||emp_middle_name like '%'||upper(:p_fio)||'%')