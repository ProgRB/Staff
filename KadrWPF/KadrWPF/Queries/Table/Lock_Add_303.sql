select COUNT(*) from {0}.REG_DOC R
join {0}.DOC_LIST D using(DOC_LIST_ID)
where R.PER_NUM = :p_PER_NUM and R.TRANSFER_ID in 
    (select TRANSFER_ID from {0}.TRANSFER T where T.WORKER_ID = :p_WORKER_ID) and
    ((TRUNC(:p_DATE_DOC) between TRUNC(R.DOC_BEGIN) and R.DOC_END and D.DOC_NOTE = 'О') 
    or (TRUNC(:p_DATE_DOC) between TRUNC(R.DOC_BEGIN)+1 and R.DOC_END and D.DOC_NOTE = 'Б')) 