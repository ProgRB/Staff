declare
begin
	open :c for 
		select --24.03.2016 - чтобы не выделялись все строчки с заполненным default_number,
			-- убираем эту проверку DECODE(default_number,null, 0, 1) as fl, 
			0 as fl,
			sign_doc_id,pos_name_sign,emp_name,default_number
		from {0}.SIGN_DOC
		where upper(TRIM(code_docum))=UPPER(TRIM(:p_code_doc)) and subdiv_id=:p_subdiv_id order by default_number;
end;