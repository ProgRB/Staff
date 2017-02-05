select pay_type_id from 
	{0}.repl_emp re
	join {0}.staffs s on (re.staffs_id=s.staffs_id)
	join {0}.transfer t on (re.staffs_id=t.staffs_id) 
	join {0}.reg_doc rd on (t.per_num=rd.per_num) 
	join {0}.doc_list dl on (rd.doc_list_id=dl.doc_list_id)  
where 
	repl_emp_id={1}
	 and sign_cur_work=1 
	 and nvl(repl_start,'01.01.1000') BETWEEN NVL(doc_begin,'01.01.1000') and NVL(doc_end,'01.01.3000')
	 and nvl(repl_end,'01.01.3000')>NVL(doc_begin,'01.01.1000')