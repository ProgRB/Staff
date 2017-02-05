select /****/ TR.PER_NUM, 
	(select CODE_SUBDIV from {0}.subdiv sub where sub.SUBDIV_ID = tr.SUBDIV_ID) as CODE_SUBDIV,
	decode(tr.SIGN_COMB, 1, 'X', null) as SIGN_COMB, 
	(select POS_NAME from {0}.position pos where tr.POS_ID = pos.POS_ID) as POS_NAME, 
	TR.POS_NOTE,
	trunc(TR.DATE_TRANSFER) as DATE_TRANSFER, 
	(select TYPE_TRANSFER_NAME from {0}.type_transfer tran where tr.TYPE_TRANSFER_ID = tran.TYPE_TRANSFER_ID) as TYPE_TRANSFER_NAME, 
	TR.CONTR_EMP, trunc(TR.DATE_CONTR) as DATE_CONTR, trunc(TR.DATE_END_CONTR) as DATE_END_CONTR, TR.TR_NUM_ORDER, 
	trunc(TR.TR_DATE_ORDER) as TR_DATE_ORDER, 
	(select CODE_DEGREE from {0}.degree deg where tr.DEGREE_ID = deg.DEGREE_ID) as CODE_DEGREE,
	(select CLASSIFIC from {0}.account_data acc where tr.TRANSFER_ID = acc.TRANSFER_ID) as CLASSIFIC,
	(select (select TG.CODE_TARIFF_GRID from {0}.TARIFF_GRID TG where TG.TARIFF_GRID_ID = acc.TARIFF_GRID_ID) 
	from {0}.account_data acc where tr.TRANSFER_ID = acc.TRANSFER_ID) as TG,
	(select CODE_FORM_OPERATION from {0}.FORM_OPERATION FO where tr.FORM_OPERATION_ID = FO.FORM_OPERATION_ID) as CODE_FORM_OPERATION,
	(select NAME_FORM_PAY from {0}.FORM_PAY FP where tr.FORM_PAY = FP.FORM_PAY) as FORM_PAY,  
	/*(select CHAR_WORK_NAME from {0}.char_work ch where tr.CHAR_WORK_ID = ch.CHAR_WORK_ID) as CHAR_WORK_NAME, 
	(select CHAR_TRANSFER_NAME from {0}.CHAR_TRANSFER CT where tr.CHAR_TRANSFER_ID = CT.CHAR_TRANSFER_ID) as CHAR_TRANSFER_NAME, */
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER ch where tr.CHAR_WORK_ID = ch.TYPE_TERM_TRANSFER_ID) as CHAR_WORK_NAME, 
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER CT where tr.CHAR_TRANSFER_ID = CT.TYPE_TERM_TRANSFER_ID) as CHAR_TRANSFER_NAME, 
	(select SOURCE_NAME from {0}.source_complect so where tr.SOURCE_ID = so.SOURCE_ID) as SOURCE_NAME, 
	(select REASON_NAME from {0}.reason_dismiss re where tr.REASON_ID = re.REASON_ID) as REASON_NAME, 
	(select BASE_DOC_NAME from {0}.base_doc bas where tr.BASE_DOC_ID = bas.BASE_DOC_ID) as BASE_DOC_NAME, 
	decode(tr.CHAN_SIGN, 1, 'X', null) as CHAN_SIGN, 
	transfer_id, WORKER_ID
from {0}.transfer tr 
where TR.PER_NUM = :p_per_num and TR.SIGN_COMB = :p_sign_comb and 
    tr.date_hire = (select max(tr2.date_hire) from {0}.transfer tr2 where tr2.per_num = :p_per_num) 
order by TR.DATE_TRANSFER