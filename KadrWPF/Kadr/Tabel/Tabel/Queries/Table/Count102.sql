select count(*) from {0}.WORK_PAY_TYPE WP3 
where WP3.WORKED_DAY_ID = (
    select W.WORKED_DAY_ID from {0}.WORKED_DAY W where W.WORK_DATE = :p_date_work and W.TRANSFER_ID in 
    (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
    connect by prior TR1.FROM_POSITION = TR1.TRANSFER_ID start with TR1.TRANSFER_ID = :p_transfer_id
    union 
    select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
    connect by prior TR1.TRANSFER_ID = TR1.FROM_POSITION start with TR1.TRANSFER_ID = :p_transfer_id
    )) 
    and WP3.PAY_TYPE_ID in (101,102)