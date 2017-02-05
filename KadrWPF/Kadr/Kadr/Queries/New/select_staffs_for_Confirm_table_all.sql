select CONFIRM_STAFFS.PER_NUM,DATE_CONFIRM,confirm_staffs.pos_id,pos_con.pos_name,CONFIRM_STAFFS.SUBDIV_ID,sign_confirm,
            staffs.staffs_id staffs_id,pos_staffs.pos_name staff_pos,pos_staffs.code_pos,SUB_STAFFS.SUBDIV_NAME,degree_name,
            case type_person 
                when 0 then 'АУП'
                when 1 then 'МОП'
                else 'ПТП' end     tp_per,
              case type_staff
                    when 0 then 'Постоянная единицы'
                    else 'Временная единица' end tp_staff,
           date_end_staff,
           harmful_addition,order_name,confirm_staffs_id,emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' "fio"
 from {0}.staffs 
	join (select staffs_id from {0}.confirm_staffs where subdiv_id={1} and pos_id={2} and per_num='{4}') s on (s.staffs_id=staffs.staffs_id)
	join {0}.confirm_staffs on (staffs.staffs_id=confirm_staffs.Staffs_id)
        join {0}.position pos_con on (pos_con.pos_id=confirm_staffs.pos_id)
        join {0}.subdiv sub_con on (SUB_CON.SUBDIV_ID=CONFIRM_STAFFS.SUBDIV_ID)
	left join {0}.emp on (confirm_staffs.per_num=emp.per_num)
        left join  {0}.position pos_staffs  on (pos_staffs.pos_id=staffs.pos_id)
        left join {0}.subdiv sub_staffs on (SUB_staffs.SUBDIV_ID=STAFFS.SUBDIV_ID)
        left join {0}.degree on (staffs.degree_id=degree.degree_id)
        left join {0}.orders on (staffs.order_id=orders.order_id)
 where staff_sign=0 {3}
 order by staffs_id