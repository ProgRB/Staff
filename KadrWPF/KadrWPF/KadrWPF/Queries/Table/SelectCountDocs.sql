select count(*) from {0}.REG_DOC R join {0}.DOC_LIST D on (D.DOC_LIST_ID = R.DOC_LIST_ID)
where R.PER_NUM = :p_PER_NUM and R.DOC_BEGIN < :p_DOC_END and R.DOC_END > :p_DOC_BEGIN and R.TRANSFER_ID in 
        (select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = :p_TRANSFER_ID
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
    and D.DOC_TYPE = (select D1.DOC_TYPE from {0}.DOC_LIST D1 where D1.DOC_LIST_ID = :p_DOC_LIST_ID)
	and R.REG_DOC_ID != :p_REG_DOC_ID and 
    not ((:p_PAY_TYPE_ID in (237, 501, 502) or :p_PAY_TYPE_ID = 210 AND :p_DOC_NOTE = 'MO')
        and D.PAY_TYPE_ID = 226)