declare
begin
	delete from {0}.temp_salary where temp_salary_id=:p_temp_salary_id and transfer_id=:p_transfer_id;
	{0}.CALC_SALARY(:p_subdiv_id, :p_beginDate, :p_endDate, :p_transfer_id, :p_sign_calc, :p_temp_salary_id);
end;