select case when exists(
    select null
    from salary.RETENTION RE
    where RE.TRANSFER_ID in (
        select TRANSFER_ID from apstaff.TRANSFER T 
        start with T.TRANSFER_ID = :p_TRANSFER_ID 
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION and T.TRANSFER_ID = prior T.FROM_POSITION)
        and RE.PAYMENT_TYPE_ID = (select PT.PAYMENT_TYPE_ID FROM salary.PAYMENT_TYPE PT where PT.PAY_TYPE_ID = 283)
        and TRUNC(sysdate,'MONTH') between TRUNC(RE.DATE_START_RET,'MONTH') 
            and NVL(RE.DATE_END_RET,DATE '3000-01-01') )
    THEN 1 ELSE 0 END from dual