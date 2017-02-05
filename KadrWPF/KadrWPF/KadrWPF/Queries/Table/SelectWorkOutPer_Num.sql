select 
    to_char(R.DOC_BEGIN,'dd.mm.yyyy hh24:mi') as "Дата начала", to_char(R.DOC_END,'dd.mm.yyyy hh24:mi') as "Дата окончания",
    R.DOC_LOCATION as "Местонахождение"
from {0}.REG_DOC R
join {0}.PN_TMP PT on R.PER_NUM = PT.PNUM
join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
where R.PER_NUM = :p_per_num and PT.USER_NAME = :p_user_name and 
    trunc(R.DOC_BEGIN) between :p_date_begin and :p_date_end and D.PAY_TYPE_ID = :p_pay_type
    and R.TRANSFER_ID in 
    (
        select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = PT.TRANSFER_ID 
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION
    )
order by R.PER_NUM, R.DOC_BEGIN