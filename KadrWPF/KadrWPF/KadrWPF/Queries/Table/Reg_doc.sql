select R.REG_DOC_ID, R.TRANSFER_ID, R.ABSENCE_ID, 
    (select D.DOC_NAME from {0}.DOC_LIST D where D.DOC_LIST_ID = R.DOC_LIST_ID) DOC_NAME, 
    R.DOC_BEGIN, R.DOC_END,  R.DOC_DATE, R.DOC_NUMBER, 
    R.DOC_LOCATION, 
    (select D.PAY_TYPE_ID from {0}.DOC_LIST D where D.DOC_LIST_ID = R.DOC_LIST_ID) PAY_TYPE_ID
from {0}.REG_DOC R where R.PER_NUM = :p_per_num and trunc(:p_date) between trunc(R.DOC_BEGIN) and trunc(R.DOC_END)
    and R.TRANSFER_ID in 
    (
        select TR1.TRANSFER_ID from {0}.TRANSFER TR1 WHERE TR1.WORKER_ID = :p_WORKER_ID      
    )