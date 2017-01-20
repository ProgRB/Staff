select distinct sub.code_subdiv, 
	em.per_num,
	em.emp_last_name||' '||SUBSTR(em.emp_first_name,1,1)||' '||SUBSTR(em.emp_middle_name,1,1) FIO,
	EM.EMP_BIRTH_DATE,
	(SELECT POS_NAME FROM {2}.POSITION P WHERE P.POS_ID = TR1.POS_ID) POS_NAME,
	tr1.date_end_contr
from {2}.emp em, {2}.subdiv sub, {2}.transfer tr1 
where 
	SUB.SUBDIV_ID = TR1.SUBDIV_ID and tr1.per_num = em.per_num and (date_transfer = (select max(date_transfer) 
from {2}.transfer tr2 where (tr1.per_num = tr2.per_num and sign_comb=1 and tr1.type_transfer_id ! = 3)) or (sign_comb=0 and tr1.date_transfer = (select max(date_transfer) 
from {2}.transfer tr2 where (tr1.per_num = tr2.per_num and tr1.type_transfer_id ! = 3 and TR1.SIGN_COMB = TR2.SIGN_COMB)))) and 
to_char(date_end_contr,'MM') = to_number('{0}') and to_char(date_end_contr,'YYYY') = to_number('{1}') order by date_end_contr
