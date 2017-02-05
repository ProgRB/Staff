select to_char(EP.event_time,'dd.mm.yyyy hh24:mi:ss') as event_time, 
    case when EP.where_into=2 and EP.where_from=1 then 'Вход на предприятие'
            when EP.where_into=3 and EP.where_from=1 then 'Вход с летного поля в цех 16'  
            when EP.where_into=1 and EP.where_from=2 then 'Выход из предприятия'  
            when EP.where_into=3 and EP.where_from=2 then 'Вход в цех 16'  
            when EP.where_into=4 and EP.where_from=2 then 'Вход в цех 95'  
            when EP.where_into=1 and EP.where_from=3 then 'Выход на летное поле из цеха 16'  
            when EP.where_into=2 and EP.where_from=3 then 'Выход из цеха 16' 
            when EP.where_into=2 and EP.where_from=4 then 'Выход из цеха 95' 
            when EP.where_into=5 and EP.where_from=3 then 'Выход на летное поле из цеха 16' 
            when EP.where_into=3 and EP.where_from=5 then 'Вход с летного поля в цех 16' 
			when EP.where_into=6 and EP.where_from=1 then 'Вход на 2-й этаж корпуса 87' 
			when EP.where_into=6 and EP.where_from=6 then 'Вход на 2-й этаж корпуса 87'              
			when EP.where_into=7 and EP.where_from=1 then 'Вход в серверную корпуса 31' 
			when EP.where_into=8 and EP.where_from=1 then 'Вход в корпус 33'                                
			when EP.where_into=1 and EP.where_from=8 then 'Выход из корпуса 33' 
    end as event, 
    (select PD.DEVICE_NAME from {0}.PERCO_DEVICE PD where EP.EVENT_DEVICE = PD.EVENT_DEVICE) DISPLAY_NAME
from {0}.EMP_PASS_EVENT EP 
where EP.per_num = :p_per_num and trunc(EP.event_time) = trunc(:p_date) 
order by event_time