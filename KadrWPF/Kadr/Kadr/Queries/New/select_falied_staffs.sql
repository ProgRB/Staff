select st.staffs_id,position.pos_name,subdiv_name,degree_name, 
    confirm_staffs.confirm_staffs_id, posit_conf.pos_name pos_name_confirm, confirm_staffs.pos_id,subdiv_name_conf.subdiv_name subdiv_name_con,
    confirm_staffs.subdiv_id,confirm_staffs.per_num,
    emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' fio,
    date_confirm,
    case 
        when exists (select staffs.staffs_id from {0}.staffs join {0}.transfer on (staffs.staffs_id=transfer.staffs_id) 
                            where confirm_staffs.pos_id=staffs.pos_id and transfer.per_num=confirm_staffs.per_num and staffs.subdiv_id=confirm_staffs.subdiv_id and transfer.sign_cur_work=1 and sign_comb=0)  then 0
        else 1 end fl
 from {0}.confirm_staffs join {0}.staffs st on (st.staffs_id=confirm_staffs.staffs_id)
      left join {0}.position on (st.pos_id=position.pos_id)
      join {0}.subdiv subdiv_name_conf on (confirm_staffs.subdiv_id=subdiv_name_conf.subdiv_id)
      left join {0}.subdiv on (st.subdiv_id=subdiv.subdiv_id)
      left join {0}.degree on (st.degree_id=degree.degree_id)
      join {0}.position posit_conf on (confirm_staffs.pos_id=posit_conf.pos_id)
      left join {0}.emp on (confirm_staffs.per_num=emp.per_num)                
       where st.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id) and staff_sign!=2
       and date_confirm is null
       order by st.staffs_id,confirm_staffs_id