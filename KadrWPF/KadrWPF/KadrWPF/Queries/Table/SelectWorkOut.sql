select (select (select SUB.CODE_SUBDIV from {0}.SUBDIV SUB where SUB.SUBDIV_ID = T1.SUBDIV_ID) 
     from {0}.TRANSFER T1 where T1.TRANSFER_ID = PT.TRANSFER_ID) CODE_SUBDIV,   
    to_char(DATE '0001-01-01' + WP.VALID_TIME/3600/24,'HH24:MI') VTIME, /*R.PER_NUM,*/
    (select E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1)||'.' 
    from {0}.EMP E where R.PER_NUM = E.PER_NUM) as FIO, 
    (select (select P.POS_NAME from {0}.POSITION P where P.POS_ID = TR.POS_ID) 
    from {0}.TRANSFER TR where TR.TRANSFER_ID = R.TRANSFER_ID) as POS_NAME,
    (round(WP.VALID_TIME/3600,2)) VTIMEHours,
    to_char(R.DOC_BEGIN,'dd.mm.yyyy hh24:mi') as DOC_BEGIN, to_char(R.DOC_END,'dd.mm.yyyy hh24:mi') as DOC_END,
    R.DOC_LOCATION as DOC_LOCATION
from {0}.REG_DOC R
join {0}.PN_TMP PT on R.PER_NUM = PT.PNUM
join {0}.TRANSFER TR on (PT.TRANSFER_ID = TR.TRANSFER_ID)
join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
join {0}.WORK_PAY_TYPE WP on R.REG_DOC_ID = WP.REG_DOC_ID
where PT.USER_NAME = :p_user_name and :p_date_begin<=R.DOC_END and 
    LEAST(:p_date_end,
    nvl((select TRUNC(TR1.DATE_TRANSFER) - 1/86399 from {0}.TRANSFER TR1 
    where TR1.FROM_POSITION = PT.TRANSFER_ID and TR1.SUBDIV_ID != TR.SUBDIV_ID),:p_date_end))>=R.DOC_BEGIN
    and D.PAY_TYPE_ID = 535 and R.TRANSFER_ID in 
    (
        select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = PT.TRANSFER_ID 
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION
    )
order by CODE_SUBDIV, R.PER_NUM, R.DOC_BEGIN