select count(*) 
from {0}.WORKED_DAY W 
join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID 
where (W.PER_NUM = :p_per_num and W.WORK_DATE = :p_date and WP.PAY_TYPE_ID in (226, 237, 526, 243, 536, 215, 222, 622) and W.TRANSFER_ID in 
        ( 
            select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id 
            connect by nocycle prior TR1.FROM_POSITION = TR1.TRANSFER_ID or TR1.FROM_POSITION = prior TR1.TRANSFER_ID              
        ))
   /* and not exists(select null from user_role_privs where GRANTED_ROLE = 'TABLE_ABSENCE_HOL')*/