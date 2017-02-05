select pos_name ,emp_last_name ,emp_first_name ,emp_middle_name ,staffs.staffs_id
from {0}.emp join {0}.transfer on (transfer.per_num=emp.per_num) right join {0}.staffs on (staffs.staffs_id=transfer.staffs_id) left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.position on (staffs.pos_id=position.pos_id) 
where transfer.subdiv_id={1} 
order by {2}