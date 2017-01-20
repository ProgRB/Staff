/* Команда меняет заказ для человека за указанный период */
/*merge into {0}.ORDER_FOR_PT OP
    using (select WP.WORK_PAY_TYPE_ID 
            from {0}.WORKED_DAY W
            join {0}.TRANSFER T on W.TRANSFER_ID = T.TRANSFER_ID
            join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
            where W.WORK_DATE between :p_DATE_ORDER_begin and :p_DATE_ORDER_end and W.PER_NUM = :p_per_num
                and T.TRANSFER_ID in 
                (
                    select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id  
                    CONNECT BY NOCYCLE PRIOR TR1.transfer_id = TR1.from_position or TR1.transfer_id = PRIOR TR1.from_position
                )) T1 
    on (OP.WORK_PAY_TYPE_ID = T1.WORK_PAY_TYPE_ID)
    when matched then
        update set ORDER_ID = :p_ORDER_ID
    when not matched then
        insert(ORDER_FOR_DAY_ID, ORDER_ID, WORK_PAY_TYPE_ID)
        values({0}.ORDER_FOR_DAY_ID_seq.nextval, :p_ORDER_ID, T1.WORK_PAY_TYPE_ID)*/
UPDATE {0}.WORK_PAY_TYPE WP
SET WP.ORDER_ID = :p_ORDER_ID
WHERE WP.WORK_PAY_TYPE_ID in 
    (select WP.WORK_PAY_TYPE_ID 
    from {0}.WORKED_DAY W
    join {0}.TRANSFER T on W.TRANSFER_ID = T.TRANSFER_ID
    join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
    where W.WORK_DATE between :p_DATE_ORDER_begin and :p_DATE_ORDER_end and W.PER_NUM = :p_per_num
        and T.TRANSFER_ID in 
        (
            select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id  
            CONNECT BY NOCYCLE PRIOR TR1.transfer_id = TR1.from_position or TR1.transfer_id = PRIOR TR1.from_position
        ))
        