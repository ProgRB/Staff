SELECT  
	(select CODE_SUBDIV from {0}.subdiv sub where sub.SUBDIV_ID = T1.SUBDIV_ID) as CODE_SUBDIV,
	(case T1.SIGN_COMB when 1 then 'X' else '' end) as SIGN_COMB, 
	(select POS_NAME from {0}.position pos where t1.POS_ID = pos.POS_ID) as POS_NAME, 
	T1.POS_NOTE,
    trunc(DATE_TRANSFER) as DATE_TRANSFER, 	
	(select TYPE_TRANSFER_NAME from {0}.type_transfer tran where t1.TYPE_TRANSFER_ID = tran.TYPE_TRANSFER_ID) as TYPE_TRANSFER_NAME,
	(select CODE_DEGREE from {0}.degree deg where t1.DEGREE_ID = deg.DEGREE_ID) as CODE_DEGREE, T1.CONTR_EMP,  
    T1.DATE_CONTR, T1.DATE_END_CONTR, T1.TR_NUM_ORDER, T1.TR_DATE_ORDER,  
	(select NAME_FORM_PAY from {0}.FORM_PAY FP where T1.FORM_PAY = FP.FORM_PAY) as FORM_PAY, 
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER ch where T1.CHAR_WORK_ID = ch.TYPE_TERM_TRANSFER_ID) as CHAR_WORK_NAME, 
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER CT where T1.CHAR_TRANSFER_ID = CT.TYPE_TERM_TRANSFER_ID) as CHAR_TRANSFER_NAME, 
	(case T1.CHAN_SIGN when 1 then 'X' else '' end) as CHAN_SIGN, 
    T1.TRANSFER_ID , T1.TYPE_TRANSFER_ID ,T1.FROM_POSITION
FROM (select T.SUBDIV_ID, T.POS_ID, T.TYPE_TRANSFER_ID, T.DEGREE_ID, T.CHAR_WORK_ID, T.SOURCE_ID, T.REASON_ID,
            T.BASE_DOC_ID, T.SIGN_COMB , T.DATE_CONTR, T.CONTR_EMP, T.DATE_TRANSFER, T.TRANSFER_ID, T.FORM_PAY,
            T.FROM_POSITION, T.DATE_END_CONTR, T.PER_NUM, T.TR_NUM_ORDER, T.TR_DATE_ORDER, T.CHAN_SIGN,
            T.CHAR_TRANSFER_ID, POS_NOTE
      from {0}.transfer T 
      where (:p_WORKER_ID is not null and T.WORKER_ID = :p_WORKER_ID) 
        or (:p_WORKER_ID is null and T.PER_NUM = :p_PER_NUM)
		/*start with transfer_id = :p_transfer_id connect by prior from_position =  transfer_id*/)  T1 
order by t1.date_transfer desc