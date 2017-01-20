select to_char(EP.event_time,'dd.mm.yyyy hh24:mi:ss') as event_time, 
    case when EP.where_into=2 and EP.where_from=1 then '���� �� �����������'
            when EP.where_into=3 and EP.where_from=1 then '���� � ������� ���� � ��� 16'  
            when EP.where_into=1 and EP.where_from=2 then '����� �� �����������'  
            when EP.where_into=3 and EP.where_from=2 then '���� � ��� 16'  
            when EP.where_into=4 and EP.where_from=2 then '���� � ��� 95'  
            when EP.where_into=1 and EP.where_from=3 then '����� �� ������ ���� �� ���� 16'  
            when EP.where_into=2 and EP.where_from=3 then '����� �� ���� 16' 
            when EP.where_into=2 and EP.where_from=4 then '����� �� ���� 95' 
            when EP.where_into=5 and EP.where_from=3 then '����� �� ������ ���� �� ���� 16' 
            when EP.where_into=3 and EP.where_from=5 then '���� � ������� ���� � ��� 16' 
			when EP.where_into=6 and EP.where_from=1 then '���� �� 2-� ���� ������� 87' 
			when EP.where_into=6 and EP.where_from=6 then '���� �� 2-� ���� ������� 87'              
			when EP.where_into=7 and EP.where_from=1 then '���� � ��������� ������� 31' 
			when EP.where_into=8 and EP.where_from=1 then '���� � ������ 33'                                
			when EP.where_into=1 and EP.where_from=8 then '����� �� ������� 33' 
    end as event, 
    (select PD.DEVICE_NAME from {0}.PERCO_DEVICE PD where EP.EVENT_DEVICE = PD.EVENT_DEVICE) DISPLAY_NAME
from {0}.EMP_PASS_EVENT EP 
where EP.per_num = :p_per_num and trunc(EP.event_time) = trunc(:p_date) 
order by event_time