select T.SUBDIV_ID, T.POS_ID, 
    (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T.POS_ID) POS_NAME,
    trunc(T.DATE_TRANSFER) DATE_TRANSFER, 
    (select TT.TYPE_TRANSFER_NAME from {0}.TYPE_TRANSFER TT where TT.TYPE_TRANSFER_ID = T.TYPE_TRANSFER_ID) TYPE_TR,
    DECODE(T.SIGN_COMB,1,'X',null) SIGN_COMB,
    PRIVILEGED_POSITION_ID, PP.SPECIAL_CONDITIONS, PP.KPS,  A.ACCOUNT_DATA_ID
from {0}.TRANSFER T
left join {0}.ACCOUNT_DATA A on T.TRANSFER_ID = A.TRANSFER_ID
left join {0}.PRIVILEGED_POSITION PP using (PRIVILEGED_POSITION_ID)
where (T.PER_NUM,T.DATE_HIRE) = 
    (select T1.PER_NUM, T1.DATE_HIRE from {0}.TRANSFER T1 where T1.TRANSFER_ID = :p_transfer_id)
    and A.CHANGE_DATE = 
        (select max(A1.CHANGE_DATE) from {0}.ACCOUNT_DATA A1 where A1.TRANSFER_ID = T.TRANSFER_ID)
order by T.SIGN_COMB, T.DATE_TRANSFER