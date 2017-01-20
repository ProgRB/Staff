declare
	st varchar(1000);
	p_date Date := :p_date;
	p_archiv number := :p_archiv;
begin
    IF (:p_date1 is not null) THEN
    	st:=st||' and exists(select  1 from {0}.vacation_schedule join {0}.transfer using (transfer_id) where worker_id=t.worker_id
				and NVL(actual_begin, decode('||:p_actual_only||', 1, actual_begin, plan_begin)) between date'''||to_char(:p_date1,'YYYY-MM-DD')||''' and date'''||to_char(:p_date2,'YYYY-MM-DD') || ''')';
    END IF;
	IF :p_per_num is not NULL THEN
		st:=st||' and per_num=lpad('''||:p_per_num||''',5,''0'')';
	END IF;
	open :c for 
    	q'[select  code_subdiv,
                pos_name,
				emp_last_name||' '||emp_first_name||' '||emp_middle_name FIO,
                per_num per_num,
                DECODE(t.sign_comb,1,'X',null) sign_comb,
                DECODE( code_degree,'04', DECODE(substr(code_pos,1,1),'2','041','3','042','043'), CODE_DEGREE) code_degree, 
				MIN(PLAN_BEGIN ) next_vac,
				name_group_master,
				t.transfer_id,
				t.WORKER_ID,
				apstaff.Check_Vac_Next_Month(t.transfer_id) vac_months
        from
				(select * from
					(select transfer_id, worker_id, per_num, sign_comb, degree_id, pos_id, subdiv_id, trunc(date_transfer) date_transfer, 
						form_operation_id, type_transfer_id,
						decode(type_transfer_id,3,trunc(date_transfer)+86399/86400, lead(trunc(date_transfer)-1/86400, 1, date'3000-01-01') over (partition by worker_id order by date_transfer)) end_transfer 
				     from {0}.transfer)
					where :p_archiv=0 and sysdate between date_transfer and end_transfer
						or :p_archiv=1 and type_transfer_id=3 and end_transfer between trunc(:p_date,'year') and add_months(trunc(:p_date,'year'),12)) t 
                join {0}.emp em using (per_num) 
                left join (select nvl(actual_begin,plan_begin) plan_begin, transfer_id , worker_id
                             from  {0}.VACATION_SCHEDULE v join {0}.transfer using (transfer_id)
                            where NVL(actual_begin,plan_begin)  between sysdate and add_months(trunc(:p_date,'year'),12)-1) vs on (vs.worker_id=t.worker_id) 
                join {0}.degree d on (d.degree_id=t.degree_id)
                join {0}.position p on (t.pos_id=p.pos_id)
                left join (select transfer_id, max(name_group_master) keep(dense_rank last order by begin_group) name_group_master from {0}.emp_group_master group by transfer_id) egm on (t.transfer_id=egm.transfer_id)
                join {0}.subdiv subd using (subdiv_id)
        where 
			(
				subdiv_id in (select subdiv_id from {0}.subdiv  start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
				or :p_subdiv_id=31 and {0}.CHECK_ECON_EMP(form_operation_id)>0
			)
		 ]'||st||q'[
        group by code_subdiv, pos_name, emp_last_name, emp_first_name, emp_middle_name, per_num, t.sign_comb, 
			DECODE( code_degree,'04', DECODE(substr(code_pos,1,1),'2','041','3','042','043'), CODE_DEGREE), 
			name_group_master, t.transfer_id, t.WORKER_ID
        order by 3 ]' using p_archiv, p_archiv, p_date, p_date,/* для фильтра архива добавил 4 переменных*/ :p_date, :p_subdiv_id, :p_subdiv_id;
end;
