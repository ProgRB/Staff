declare
begin
	open :c1 for select * from apstaff.reg_doc where reg_doc_id=:p_reg_doc_id;
	open :c2 for select * from apstaff.doc_list;
	open :c3 for select * from apstaff.transfer where transfer_id=:p_transfer_id;
end;
