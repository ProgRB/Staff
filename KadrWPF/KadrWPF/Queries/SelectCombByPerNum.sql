select TR.PER_NUM, 
	(select CODE_SUBDIV from {0}.subdiv sub where sub.SUBDIV_ID = tr.SUBDIV_ID) as CODE_SUBDIV,
	(select POS_NAME from {0}.position pos where tr.POS_ID = pos.POS_ID) as POS_NAME, 
	trunc(TR.DATE_TRANSFER) as DATE_TRANSFER, 
	(select TYPE_TRANSFER_NAME from {0}.type_transfer tran where tr.TYPE_TRANSFER_ID = tran.TYPE_TRANSFER_ID) as TYPE_TRANSFER_NAME, 
	TR.CONTR_EMP, trunc(TR.DATE_CONTR) as DATE_CONTR, trunc(TR.DATE_END_CONTR) as DATE_END_CONTR, TR.TR_NUM_ORDER, 
	trunc(TR.TR_DATE_ORDER) as TR_DATE_ORDER, 
	(select CODE_DEGREE from {0}.degree deg where tr.DEGREE_ID = deg.DEGREE_ID) as CODE_DEGREE,
	(select CLASSIFIC from {0}.account_data acc where tr.TRANSFER_ID = acc.TRANSFER_ID) as CLASSIFIC,
	(case (select CODE_DEGREE from {0}.degree deg where tr.DEGREE_ID = deg.DEGREE_ID) when '01' 
	then 'Сдельная' when '02' then 'Сдельная' else 'Повременная' end) as FORM_PAY,  
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER ch where tr.CHAR_WORK_ID = ch.TYPE_TERM_TRANSFER_ID) as CHAR_WORK_NAME, 
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER CT where tr.CHAR_TRANSFER_ID = CT.TYPE_TERM_TRANSFER_ID) as CHAR_TRANSFER_NAME, 
	(select SOURCE_NAME from {0}.source_complect so where tr.SOURCE_ID = so.SOURCE_ID) as SOURCE_NAME,
	transfer_id 
from {0}.transfer tr 
where TR.PER_NUM = :p_PER_NUM and TR.SIGN_CUR_WORK = 1 and TR.SIGN_COMB = 1
order by TR.DATE_TRANSFER

/*
0 - FormMain.schema
1 - per_num
2 - sign_comb
(case TR.FORM_PAY when 1 then 'Сдельная' else 'Повременная' end) as FORM_PAY
*/