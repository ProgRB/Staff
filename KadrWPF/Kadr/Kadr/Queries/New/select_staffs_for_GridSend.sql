select subdiv.subdiv_name "�������������",position.code_pos "��� ���������",position.pos_name "���������",  
s.date_begin_staff "���� ��������",s.date_end_staff "���� ����������",
case s.type_person 
		when 0 then '���'
		when 1 then '���'
		else '���' end 	"��� ���������",
s.staffs_id,s.subdiv_id
from
	{0}.staffs s left join {0}.transfer on (s.staffs_id=transfer.staffs_id)	  
	join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id) 
	left join {0}.position on (s.pos_id=position.pos_id)
	left join {0}.emp on (transfer.per_num=emp.per_num)