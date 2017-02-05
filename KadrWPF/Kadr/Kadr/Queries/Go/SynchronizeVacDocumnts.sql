begin
	{0}.UPDATE_VAC_REG_DOC(:p_transfer_id, date'2012-01-01');
	{0}.Update_RegDocVac(:p_transfer_id, :p_date);
	{0}.Update_RegDocVac(:p_transfer_id, add_months(:p_date, -12));
	COMMIT;
end;