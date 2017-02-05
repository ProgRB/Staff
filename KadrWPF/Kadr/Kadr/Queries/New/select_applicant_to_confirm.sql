select s.fio,s.per_num,
   case 
        when {3} in (select DISTINCT subdiv_id from {0}.staffs start with staffs_id=s.staffs_id connect by prior staffs_id=staff_parent_id ) then 1
        else 0 end fl
  from (select  emp.per_num,staffs.staffs_id staffs_id, pos_name, emp_last_name||' '||emp_first_name||' '||emp_middle_name fio
                    from {0}.transfer join {0}.staffs on (transfer.staffs_id=staffs.staffs_id)
                                                                join {0}.position on (staffs.pos_id=position.pos_id)
                                                                join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id)
                                                                join {0}.emp on (transfer.per_num=EMP.PER_NUM)
                    where transfer.sign_comb=0 and sign_cur_work=1 and staffs.subdiv_id={1} and staffs.pos_id={2} ) s