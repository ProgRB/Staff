select pos_name "���������",emp_last_name "�������",emp_first_name "���",emp_middle_name "��������", emp.per_num "��������� �����", staffs.staffs_id 
from {0}.staffs left join {0}.transfer on (staffs.staffs_id=transfer.staffs_id) left join {0}.emp on (transfer.per_num=emp.per_num)  left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.position on (staffs.pos_id=position.pos_id)
where  staffs.pos_id in (select pos_id from {0}.position where upper(pos_name)=upper('{1}'))
	and staffs.subdiv_id={2}