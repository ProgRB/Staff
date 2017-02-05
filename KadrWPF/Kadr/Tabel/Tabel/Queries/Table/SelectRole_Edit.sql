select count(*) from user_role_privs where granted_role = 'TABLE_ADD_VOUCHER'
    or exists(select 1 from {0}.PERMIT P where P.PER_NUM = :p_per_num and :p_date between P.DATE_START_PERMIT and P.DATE_END_PERMIT
        and P.TYPE_PERMIT_ID in (select TP.TYPE_PERMIT_ID from {0}.TYPE_PERMIT TP where TP.NOT_REGISTR_PASS = 1)) 