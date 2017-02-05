select V.PER_NUM, E.EMP_LAST_NAME ||' '|| substr(E.EMP_FIRST_NAME,1,1) ||'.'|| substr(E.EMP_MIDDLE_NAME,1,1)||'.' as FIO,
    COMB, HOURS_T, HOURS_S  
from 
(
    select TT.PER_NUM, TT.TRANSFER_ID,
        case when TT.SIGN_COMB = 0 then ' ' else 'X' end as COMB,
        round(TT.MONTH,2) HOURS_T, 
        round((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
            where TS.TEMP_SALARY_ID = :p_temp_salary_id and TT.PER_NUM = TS.PER_NUM and TT.SIGN_COMB = TS.SIGN_COMB and 
                TS.PAY_TYPE_ID = '100' 
            group by TS.PER_NUM),2) HOURS_S
    from {0}.TEMP_TABLE TT
    where TT.TEMP_TABLE_ID = :p_temp_table_id and TT.SIGN_COMB = 0 and
        TT.PAY_TYPE = 102 and ( 
        round(nvl((select sum(nvl(TS.TIME,0)) from {0}.TEMP_SALARY TS 
            where TS.TEMP_SALARY_ID = :p_temp_salary_id and TT.PER_NUM = TS.PER_NUM and TT.SIGN_COMB = TS.SIGN_COMB 
                and TS.PAY_TYPE_ID = '100' 
            group by TS.PER_NUM),0),2) - round(TT.MONTH,2)) > 0.1
) V
join {0}.EMP E on (E.PER_NUM = V.PER_NUM)
join {0}.TRANSFER using(TRANSFER_ID)
join {0}.DEGREE D using(DEGREE_ID)
where D.CODE_DEGREE != '11'
order by PER_NUM