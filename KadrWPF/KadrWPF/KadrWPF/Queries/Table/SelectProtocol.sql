select TT.PER_NUM, 
    (select E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1)||'.' 
    from {0}.EMP E where TT.PER_NUM = E.PER_NUM) as FIO,
    case when TT.SIGN_COMB = 0 then ' ' else 'X' end as COMB,
    1 SIGN_ERROR, null PAY_TYPE_ID, null CODE_DEGREE,
    round(TT.MONTH,2) HOURS_T, 
    round((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
    where TS.TEMP_SALARY_ID = :p_temp_salary_id and TS.SIGN_ACCOUNT = 1 and 
        TS.TRANSFER_ID in 
            (select TR.TRANSFER_ID from {0}.TRANSFER TR start with TR.TRANSFER_ID = TT.TRANSFER_ID
            connect by nocycle prior TR.TRANSFER_ID=TR.FROM_POSITION or TR.TRANSFER_ID= prior TR.FROM_POSITION)
		and TS.PAY_TYPE_ID in ('101','102') and TS.ORDER_NAME is not null
    group by TS.PER_NUM),2) HOURS_S,
    DAY102 DAY_T,
    nvl((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
    where TS.TEMP_SALARY_ID = :p_temp_salary_id and TS.SIGN_ACCOUNT = 1 and 
        TS.TRANSFER_ID in 
            (select TR.TRANSFER_ID from {0}.TRANSFER TR start with TR.TRANSFER_ID = TT.TRANSFER_ID
            connect by nocycle prior TR.TRANSFER_ID=TR.FROM_POSITION or TR.TRANSFER_ID= prior TR.FROM_POSITION)
		and TS.PAY_TYPE_ID = '540' 
    group by TS.PER_NUM),0) DAY_S, null GM, null YN
from {0}.TEMP_TABLE TT
where TT.TEMP_TABLE_ID = :p_temp_table_id and  
    TT.PAY_TYPE = 102 and ((round(TT.MONTH,0) != 
    round(nvl((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
    where TS.TEMP_SALARY_ID = :p_temp_salary_id and TS.SIGN_ACCOUNT = 1 and 
        TS.TRANSFER_ID in 
            (select TR.TRANSFER_ID from {0}.TRANSFER TR start with TR.TRANSFER_ID = TT.TRANSFER_ID
            connect by nocycle prior TR.TRANSFER_ID=TR.FROM_POSITION or TR.TRANSFER_ID= prior TR.FROM_POSITION)
        and TS.PAY_TYPE_ID in ('101','102') and TS.ORDER_NAME is not null
    group by TS.PER_NUM),0),0)
    or round(DAY102,0) != round(nvl((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
    where TS.TEMP_SALARY_ID = :p_temp_salary_id and TS.SIGN_ACCOUNT = 1 and 
        TS.TRANSFER_ID in 
            (select TR.TRANSFER_ID from {0}.TRANSFER TR start with TR.TRANSFER_ID = TT.TRANSFER_ID
            connect by nocycle prior TR.TRANSFER_ID=TR.FROM_POSITION or TR.TRANSFER_ID= prior TR.FROM_POSITION)
		and TS.PAY_TYPE_ID = '540' 
    group by TS.PER_NUM),0),0))
        or (TT.SIGN_COMB = 0 and TT.MONTH > 0 and TT.DAY102 = 0 ) 
        /*or (TT.SIGN_COMB = 0 and TT.MONTH = 0 and TT.DAY102 > 0 )*/)
union
select PER_NUM, (select E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1)||'.' 
    from {0}.EMP E where V.PER_NUM = E.PER_NUM) as FIO, P_RAB, 
    2 SIGN_ERROR, VOP, KT, null, null, null, null, GM, YN  
from (
    select case when SIGN_COMB=1 then 'X' else chr(32) end as P_RAB, 
     PAY_TYPE_ID as VOP, PER_NUM, GROUP_MASTER GM, COUNT_YN as YN, 
     CODE_DEGREE as KT, NSTR, FORM_PAY
     from 
     (
         select PAY_TYPE_ID, ORDER_NAME, TS.PER_NUM, CODE_DEGREE, NSTR, TS.SIGN_COMB, 
            GROUP_MASTER, sum(COUNT_YN) COUNT_YN, FORM_PAY
         from {0}.TEMP_SALARY TS
		 join {0}.TRANSFER using(TRANSFER_ID)
         where TEMP_SALARY_ID = :p_temp_salary_id and SIGN_ACCOUNT = 1 and PAY_TYPE_ID in ('101', '102') and REPL_EMP_ID is null
         group by PAY_TYPE_ID, ORDER_NAME, TS.PER_NUM, CODE_DEGREE, NSTR, GROUP_MASTER, TS.SIGN_COMB, FORM_PAY
     )
) V
where ((VOP = '101' and (GM is null or YN is null)) or (VOP = '101' and FORM_PAY != 1 /*KT not in ('01','02')*/) 
    or (VOP = '102' and FORM_PAY = 1/*KT in ('01','02')*/))
order by SIGN_ERROR, PER_NUM