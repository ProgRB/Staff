declare
begin
	open :r1 for select * from apstaff.EMP_STAFF where staff_id=:p_staff_id;
	open :r2 for 
		select t.transfer_id, 
			code_pos, 
			code_subdiv,
			pos_name, 
			code_degree, 
			per_num, 
			INITCAP(emp_last_name) emp_last_name,
			INITCAP(emp_first_name) emp_first_name, 
			INITCAP(emp_middle_name) emp_middle_name,
			classific, salary, code_tariff_grid, photo
		from
			apstaff.transfer t
			join apstaff.emp using (per_num)
			join apstaff.subdiv using (subdiv_id)
			join apstaff.position using (pos_id)
			join apstaff.degree using (degree_id)
			join (select code_tariff_grid, classific, salary, transfer_id 
					from apstaff.account_data left join apstaff.tariff_grid using (tariff_grid_id)
					) ad on (decode(t.type_transfer_id, 3, t.from_position, t.transfer_id) = ad.transfer_id)
		where 
			t.transfer_id in (select transfer_id from apstaff.emp_staff where staff_id=:p_staff_id);
	open :r3 for select * from apstaff.staff where staff_id=:p_staff_id;		
end;
