select '9' as PTN, :p_code_subdiv as SC, '002' as NP, '1' as ZN, case when TR.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, PAY_TYPE as VOP,       
    '00' as PR, lpad('0',13, '0') as ZAK,  TT.PER_NUM as TN, lpad(to_char(MONTH_2*10),7,'0') as HCAS, 
    lpad('0',11, '0') as SUM, lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT  
from  {0}.TEMP_TABLE TT
join {0}.TRANSFER TR on (TT.TRANSFER_ID = TR.TRANSFER_ID)
join {0}.SUBDIV S on (TR.SUBDIV_ID = S.SUBDIV_ID)
join {0}.DEGREE D on (TR.DEGREE_ID = D.DEGREE_ID) 
where (TR.SIGN_CUR_WORK = 1 or (TR.SIGN_CUR_WORK = 0 and TR.TYPE_TRANSFER_ID != 3)) and TR.SIGN_COMB = 0
    and TT.TEMP_TABLE_ID = :p_temp_table_id and TT.MONTH_2 > 0
    /*and D.CODE_DEGREE not in ('01','02')*/
	/*and TR.FORM_PAY = 2*/
	and D.CODE_DEGREE not in ('01','02','11','12')
 order by SC, TN, P_RAB, VOP