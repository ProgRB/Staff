select rownum,
		a.*,null 
		from 
		(select 
				code_subdiv,
				code_degree,
				DECODE(grouping(fio),1,'бяецн:',fio) fio,
				per_num,
				pos_name,   
				sign_comb,
                mnth,
				sum(count_days) cnt_d
		from
			(
				select emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,
						t.per_num,
						pos_name,
						DECODE(code_degree,'04',DECODE(substr(code_pos,1,1),'2','041','3','042','043'),code_degree) code_degree,
						code_subdiv,
						DECODE(SIGN_COMB,1,'X',null) sign_comb,
						count_days,
                        extract(month from plan_begin) as mnth,
						t.transfer_id
				from
					(select * from  {0}.transfer  where sign_cur_work=1) t 
					join 
						(select (select transfer_id from {0}.transfer where sign_cur_work=1 start with transfer_id=vs.transfer_id connect by NOCYCLE prior transfer_id=from_position or prior from_position =transfer_id) as last_transfer_id,
                                count_days, plan_begin
                            from {0}.vacation_schedule vs join {0}.vac_consist vc using (vac_sched_id) join {0}.type_vac using (type_vac_id)
                            where plan_begin between  trunc(:p_date1) and trunc(:p_date2)-1 and plan_sign=1
                            and SING_PAYMENT=1 ) vs on (vs.last_transfer_id=t.transfer_id)        
					join {0}.emp on (emp.per_num=t.per_num)
					join {0}.degree d on (t.degree_id=d.degree_id)   
					join {0}.position p on (t.pos_id=p.pos_id),
					(select subdiv_id,code_subdiv from {0}.subdiv  start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)   subd
				where  t.subdiv_id=subd.subdiv_id
			)
		group by rollup (code_subdiv,code_degree,(mnth,fio,per_num,pos_name,code_subdiv,sign_comb,transfer_id))
		) a
