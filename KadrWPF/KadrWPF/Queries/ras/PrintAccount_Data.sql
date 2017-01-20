select E.EMP_LAST_NAME || ' ' || substr(E.EMP_FIRST_NAME,1,1) || '. ' || substr(E.EMP_MIDDLE_NAME,1,1) || '.' as FIO, E.PER_NUM,  
    (select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) as CODE_DEGREE,
     DECODE(A.HARMFUL_ADDITION,0,null,A.HARMFUL_ADDITION) as HARMFUL_ADDITION, 
     DECODE(A.HARMFUL_ADDITION_ADD,0,null,A.HARMFUL_ADDITION_ADD) as HARMFUL_ADDITION_ADD, 
     DECODE(A.SALARY,0,null,A.SALARY) as SALARY, 
     DECODE(A.CLASSIFIC,0,null,A.CLASSIFIC) as CLASSIFIC, 
     (select TG.CODE_TARIFF_GRID from {0}.TARIFF_GRID TG where TG.TARIFF_GRID_ID = A.TARIFF_GRID_ID) as CODE_TARIFF_GRID,
     DECODE(A.COMB_ADDITION,0,null,A.COMB_ADDITION) as COMB_ADDITION, 
     DECODE(A.CHIEF_ADDITION,0,null,A.CHIEF_ADDITION) as CHIEF_ADDITION, 
     DECODE(A.CLASS_ADDITION,0,null,A.CLASS_ADDITION) as CLASS_ADDITION, 
     DECODE(A.PROF_ADDITION,0,null,A.PROF_ADDITION) as PROF_ADDITION, 
     case when exists(select null from {0}.EMP_PRIV ep join {0}.TYPE_PRIV tp on TP.TYPE_PRIV_ID = EP.TYPE_PRIV_ID 
					 where EP.PER_NUM = E.PER_NUM and TP.SIGN_INVALID = 1 and 
						trunc(sysdate) between TRUNC(EP.DATE_START_PRIV,'MONTH') and 
                        nvl2(EP.DATE_END_PRIV, TRUNC(EP.DATE_END_PRIV,'MONTH')-1, SYSDATE)) 
		then '2' 
	 end as SIGN_INVALID,
     (select max(EP.DATE_END_PRIV) from {0}.EMP_PRIV ep join {0}.TYPE_PRIV tp on TP.TYPE_PRIV_ID = EP.TYPE_PRIV_ID 
     where EP.PER_NUM = E.PER_NUM and TP.SIGN_INVALID = 1 and 
		trunc(sysdate) between TRUNC(EP.DATE_START_PRIV,'MONTH') and 
        nvl2(EP.DATE_END_PRIV, TRUNC(EP.DATE_END_PRIV,'MONTH')-1, SYSDATE)) as DATE_END_INVALID,
     case when T.SIGN_COMB = 0 then '' else 'X' end as SIGN_COMB,
     DECODE(A.PERCENT13,0,null,A.PERCENT13) as PERCENT13, 
     A.TAX_CODE, A.SERVICE_ADD,
     DECODE(A.COUNT_DEP14,0,null,A.COUNT_DEP14) as COUNT_DEP14, 
     DECODE(A.COUNT_DEP15,0,null,A.COUNT_DEP15) as COUNT_DEP15, 
     DECODE(A.COUNT_DEP16,0,null,A.COUNT_DEP16) as COUNT_DEP16, 
     DECODE(A.COUNT_DEP17,0,null,A.COUNT_DEP17) as COUNT_DEP17, 
     DECODE(A.COUNT_DEP18,0,null,A.COUNT_DEP18) as COUNT_DEP18, 
     DECODE(A.COUNT_DEP19,0,null,A.COUNT_DEP19) as COUNT_DEP19, 
     DECODE(A.COUNT_DEP20,0,null,A.COUNT_DEP20) as COUNT_DEP20, 
     DECODE(A.COUNT_DEP21,0,null,A.COUNT_DEP21) as COUNT_DEP21     
from {0}.EMP E
join {0}.TRANSFER T on E.PER_NUM = T.PER_NUM
join {0}.ACCOUNT_DATA A on T.TRANSFER_ID = A.TRANSFER_ID
where T.SUBDIV_ID = :p_subdiv_id and T.SIGN_CUR_WORK = 1 
    and ((T.DEGREE_ID = :p_degree_id and :p_degree_id is not null) or :p_degree_id is null) 
    and ((A.CLASSIFIC = :p_classific and :p_classific is not null) or :p_classific is null)   
    and ((E.PER_NUM = :p_per_num and :p_per_num is not null) or :p_per_num is null)
    and ((upper(E.EMP_LAST_NAME) = upper(:p_LAST_NAME) and :p_LAST_NAME is not null) or :p_LAST_NAME is null)
    and ((upper(E.EMP_FIRST_NAME) = upper(:p_FIRST_NAME) and :p_FIRST_NAME is not null) or :p_FIRST_NAME is null)  
    and ((upper(E.EMP_MIDDLE_NAME) = upper(:p_MIDDLE_NAME) and :p_MIDDLE_NAME is not null) or :p_MIDDLE_NAME is null)        
order by E.PER_NUM, T.SIGN_COMB