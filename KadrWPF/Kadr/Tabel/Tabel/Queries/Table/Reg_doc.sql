select R.REG_DOC_ID, R.TRANSFER_ID, R.ABSENCE_ID, 
    (select D.DOC_NAME from {0}.DOC_LIST D where D.DOC_LIST_ID = R.DOC_LIST_ID) as "Наименование документа", 
    R.DOC_BEGIN as "Дата начала", R.DOC_END as "Дата окончания",  R.DOC_DATE as "Дата документа", R.DOC_NUMBER as "№ док.", 
    R.DOC_LOCATION as "Местонахождение", 
    (select D.PAY_TYPE_ID from {0}.DOC_LIST D where D.DOC_LIST_ID = R.DOC_LIST_ID) as PAY_TYPE_ID
from {0}.REG_DOC R where R.PER_NUM = :p_per_num and trunc(:p_date) between trunc(R.DOC_BEGIN) and trunc(R.DOC_END)
    and R.TRANSFER_ID in 
    (
        select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id
        connect by nocycle prior TR1.TRANSFER_ID = TR1.FROM_POSITION or TR1.TRANSFER_ID = prior TR1.FROM_POSITION        
    )