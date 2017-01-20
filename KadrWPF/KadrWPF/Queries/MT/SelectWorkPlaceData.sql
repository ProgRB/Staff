declare
begin
	open :c1 for select * from apstaff.WORK_PLACE where work_place_id=:p_work_place_id;
	open :c2 for select * from apstaff.WORK_PLACE_PROTECTION where work_place_id=:p_work_place_id;
	open :c3 for select * from apstaff.WORK_PLACE_CONDITION where work_place_id=:p_work_place_id;
	open :c4 for select * from apstaff.TYPE_CONDITION order by ORDER_FOR_EDIT;
	open :c5 for select * from apstaff.INDIVID_PROTECTION order by CODE_PROTECTION;
end;
