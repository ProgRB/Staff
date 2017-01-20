select 1 nst, 1 ins_r, '' from dual
union
select 2 nst, 1 ins_r, '                                  ¬Â‰ÓÏÓÒÚ¸ ‡Ò˜ÂÚ‡ Ú‡·ÂÎˇ ÔÓ ÔÓ‰‡Á‰ÂÎÂÌË˛ '||(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = :p_subdiv_id)||' Á‡ '||:p_month||' ÏÂÒˇˆ '||:p_year||' „.' from dual
union
select 3 nst, 1 ins_r, '' from dual
union
select 4, 1 ins_r, rpad('*',128,'*') from dual
union
select 5, 1 ins_r, ' “¿¡. :    ‘¿Ã»À»ﬂ » Œ     :“¿–»‘:   102   : —¬≈–’”–Œ◊ÕŒ  :   112   :   114   :   121   :   124   :   125   :   111   :   211   ' from dual
union
select 6, 1 ins_r, ' ÕŒÃ≈–:                    : Œ›‘‘:---------:   106 ¬/ŒœÀ. :---------:---------:---------:---------:---------:---------:---------' from dual
union
select 7, 1 ins_r, '      :                    :     :  ◊¿—€   :--------------:  ◊¿—€   :  ◊¿—€   :  ◊¿—€   :  ◊¿—€   :  ◊¿—€   :  ◊¿—€   :  ◊¿—€   ' from dual
union
select 8, 1 ins_r, '      :                    :     :  —”ÃÃ¿  : ◊¿—€: —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  :  —”ÃÃ¿  ' from dual
union
select 9, 1 ins_r, rpad('*',128,'*') from dual
union
select rownum + 9 nst, 2 ins_r, ' '||V1.PER_NUM||' '||V1.FIO||' '||V1.SALARY
    ||' '||V1.T102||' '||V1.T106_1||' '||V1.F106_1||' '||V1.T112||' '||V1.T114||' '||V1.T121||' '||V1.T124||' '||V1.T125||' '||V1.T111||' '||V1.T211
    ||chr(10)||lpad(' ',33,' ')
    ||' '||V1.F102||' '||V1.T106_2||' '||V1.F106_2||' '||V1.F112||' '||V1.F114||' '||V1.F121||' '||V1.F124||' '||V1.F125||' '||V1.F111||' '||V1.F211
FROM
(select V.PER_NUM, 
    decode(grouping(V.PER_NUM),1, rpad('»“Œ√Œ œŒ œŒƒ–. '||(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = :p_subdiv_id),30,' '), V.FIO) as FIO, 
    V.SALARY,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '102', TS.TIME)),'FM999990.0'),' '),9,' ') t102,
    lpad(nvl(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 1 then TS.TIME end),'FM990.0'),' '),5,' ') t106_1,
    lpad(nvl(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 2 then TS.TIME end),'FM990.0'),' '),5,' ') t106_2,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '112', TS.TIME)),'FM999990.0'),' '),9,' ') t112,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '114', TS.TIME)),'FM999990.0'),' '),9,' ') t114,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '121', TS.TIME)),'FM999990.0'),' '),9,' ') t121,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '124', TS.TIME)),'FM999990.0'),' '),9,' ') t124,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '125', TS.TIME)),'FM999990.0'),' '),9,' ') t125,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '111', TS.TIME)),'FM999990.0'),' '),9,' ') t111,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '211', TS.TIME)),'FM999990.0'),' '),9,' ') t211,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '102', TS.SUM)),'FM999999990.00'),' '),9,' ') as f102,
    lpad(nvl(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 1 then TS.SUM end),'FM99990.00'),' '),8,' ') as f106_1,
    lpad(nvl(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 2 then TS.SUM end),'FM99990.00'),' '),8,' ') as f106_2,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '112', TS.SUM)),'FM999999990.00'),' '),9,' ') as f112,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '114', TS.SUM)),'FM999999990.00'),' '),9,' ') as f114,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '121', TS.SUM)),'FM999999990.00'),' '),9,' ') as f121,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '124', TS.SUM)),'FM999999990.00'),' '),9,' ') as f124,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '125', TS.SUM)),'FM999999990.00'),' '),9,' ') as f125,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '111', TS.SUM)),'FM999999990.00'),' '),9,' ') as f111,
    lpad(nvl(to_char(sum(Decode(TS.PAY_TYPE_ID, '211', TS.SUM)),'FM999999990.00'),' '),9,' ') as f211
from
    (select E.PER_NUM, rpad(E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.',20,' ') as FIO,
        lpad(to_char((SELECT DISTINCT FIRST_VALUE(salary) over (order by change_date desc) salary
           from {0}.ACCOUNT_DATA AD where AD.TRANSFER_ID=DECODE(T.TYPE_TRANSFER_ID,3,T.FROM_POSITION,T.TRANSFER_ID)),'FM990.00'),5,' ') as SALARY,
        T.SIGN_COMB
    from {0}.PN_TMP PN
    join {0}.TRANSFER T on PN.TRANSFER_ID = T.TRANSFER_ID
    join {0}.EMP E on T.PER_NUM = E.PER_NUM
    where PN.USER_NAME = :p_user_name
    order by E.PER_NUM) V
join {0}.TEMP_SALARY TS on (V.PER_NUM = TS.PER_NUM and V.SIGN_COMB = TS.SIGN_COMB) 
where TS.TEMP_SALARY_ID = :p_temp_salary_id
group by rollup((V.PER_NUM, V.FIO, V.SALARY))
) V1