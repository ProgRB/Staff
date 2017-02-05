select rownum,E.PER_NUM, E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
    V.COMB,V.TYPE_TR,V.DATE_TR,V.DATE_ORD,SAL_OLD,SUB_OLD,SAL,SUB
from (
	select T0.PER_NUM, decode(T0.SIGN_COMB,0,null,1,'X') COMB, 
		DECODE(T0.TYPE_TRANSFER_ID,1,'опхел',2,
			case when T0.SUBDIV_ID != T_OLD.SUBDIV_ID then 'оепебнд' else 'оепебнд (бм)' end) TYPE_TR, 
		T0.DATE_TRANSFER DATE_TR, T0.TR_DATE_ORDER DATE_ORD,
		A_OLD.SALARY SAL_OLD,     
		(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T_OLD.SUBDIV_ID) SUB_OLD,
		A0.SALARY SAL, 
		(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T0.SUBDIV_ID) SUB
	from {0}.TRANSFER T0
	join {0}.ACCOUNT_DATA A0 on T0.TRANSFER_ID = A0.TRANSFER_ID 
	left join {0}.TRANSFER T_OLD on T0.FROM_POSITION = T_OLD.TRANSFER_ID 
	left join {0}.ACCOUNT_DATA A_OLD on T_OLD.TRANSFER_ID = A_OLD.TRANSFER_ID 
	where T0.DATE_TRANSFER between :p_date_begin and :p_date_end 
		and T0.SUBDIV_ID = :p_subdiv_id and T0.TYPE_TRANSFER_ID in (1,2)   
	union
	select T0.PER_NUM, decode(T0.SIGN_COMB,0,null,1,'X') COMB, 
		DECODE(T_NEW.TYPE_TRANSFER_ID,1,'опхел',2,
			case when T0.SUBDIV_ID != T_NEW.SUBDIV_ID then 'оепебнд (хг)' else 'оепебнд (бм)' end,3,'сбнкэмемхе') TYPE_TR, 
		T_NEW.DATE_TRANSFER DATE_TR, T_NEW.TR_DATE_ORDER DATE_ORD,
		A0.SALARY SAL, 
		(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T0.SUBDIV_ID) SUB,
		A_NEW.SALARY SAL_NEW,
		case when T_NEW.TYPE_TRANSFER_ID != 3 
			then (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T_NEW.SUBDIV_ID)
			else null end SUB_NEW
	from {0}.TRANSFER T0
	join {0}.ACCOUNT_DATA A0 on T0.TRANSFER_ID = A0.TRANSFER_ID 
	left join {0}.TRANSFER T_NEW on T0.TRANSFER_ID = T_NEW.FROM_POSITION
	left join {0}.ACCOUNT_DATA A_NEW on T_NEW.TRANSFER_ID = A_NEW.TRANSFER_ID 
	where T_NEW.DATE_TRANSFER between :p_date_begin and :p_date_end 
		and T0.SUBDIV_ID = :p_subdiv_id and 
		(T0.SUBDIV_ID != T_NEW.SUBDIV_ID
		or (T0.SUBDIV_ID = T_NEW.SUBDIV_ID and T_NEW.TYPE_TRANSFER_ID = 3))
) V
join {0}.EMP E on V.PER_NUM = E.PER_NUM
order by PER_NUM