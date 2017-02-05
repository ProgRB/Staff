select 
      sb.subdiv_name,p.pos_name "Штат. единица",sb_r.subdiv_name "Дополн. направлена",
      e_m.emp_last_name||' '||substr(e_m.emp_first_name,1,1)||'. '||substr(e_m.emp_middle_name,1,1)||'.' "ФИО",
      e_r.emp_last_name||' '||substr(e_r.emp_first_name,1,1)||'. '||substr(e_r.emp_middle_name,1,1)||'.' "Замещение",
	  repl_name "Вид",
	  Case when re.sign_longtime=1 then 'Длительн.' else case when re.repl_emp_id is not null then  'Кратковр.' else '' end end "Тип замещения",
      s.staffs_id,re.repl_emp_id,staffs_temp_id,t_subdiv_id
from 
      {0}.staffs s
      left join {0}.position p on (p.pos_id=s.pos_id)
      left join (select emp.per_num,emp_last_name,emp_first_name,emp_middle_name,transfer.transfer_id,staffs_id 
				 from {0}.transfer join {0}.emp on (transfer.per_num=emp.per_num)  where sign_cur_work=1) e_m
                on (e_m.staffs_id=s.staffs_id)
      left join {0}.subdiv sb on (sb.subdiv_id=s.subdiv_id)
      left join ( select * from {0}.staffs_temp_alloc where  nvl(actual_alloc,1)!=0 ) st_a on (st_a.staffs_id=s.staffs_id)
       left join {0}.subdiv sb_r on (sb_r.subdiv_id=st_a.t_subdiv_id)
       left join {0}.degree d on (s.degree_id=d.degree_id)
       left join (select * from {0}.repl_emp where repl_actual_sign=1 and SYSDATE BETWEEN nvl(repl_start,'01.01.1000') and nvl(repl_end,'01.01.3000') )re on (s.staffs_id=re.staffs_id)
       left join (select emp.per_num,emp_last_name,emp_first_name,emp_middle_name,transfer.transfer_id,staffs_id 
				  from {0}.transfer join {0}.emp on (transfer.per_num=emp.per_num)  where sign_cur_work=1) e_r
                on (e_r.transfer_id=s.re.transfer_id)
		left join {0}.type_repl tre on (re.type_repl_id=tre.type_repl_id)
		,
       (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id) sub_d
			
      where sub_d.subdiv_id=s.subdiv_id   {2}
       order by p.pos_name