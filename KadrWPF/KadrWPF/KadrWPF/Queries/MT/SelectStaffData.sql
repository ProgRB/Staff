declare
begin
    open :c1 for select * from apstaff.staff where staff_id=:p_staff_id;
	open :c2 for select * from apstaff.staff_period where staff_id=:p_staff_id;
	open :c3 for select * from apstaff.staff_addition where staff_id=:p_staff_id;
	open :c4 for select * from apstaff.staff_vac where staff_id=:p_staff_id;
	open :c5 for select * from apstaff.subdiv_partition;
end;