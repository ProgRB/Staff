select 1 from {0}.WORK_PAY_TYPE WP 
where WP.WORK_PAY_TYPE_ID = :p_work_pay_type_id and 
    (WP.PAY_TYPE_ID in (101, 102, 110, 112, 114, 211, 111, 510, 530) or
    (WP.PAY_TYPE_ID = 124 and 
        exists(select null from {0}.REG_DOC R join {0}.DOC_LIST D using(DOC_LIST_ID)
                where R.REG_DOC_ID = WP.REG_DOC_ID and D.PAY_TYPE_ID in (222, 622))))