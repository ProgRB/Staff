select V.PER_NUM, 
    decode(grouping(V.PER_NUM),1, '����� �� ����. '||(select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = :p_subdiv_id), V.FIO) as FIO, 
    V.SALARY,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '102', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '102', TS.SUM)),'FM999999990.00'),9,' ') as f102,
    lpad(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 1 then TS.TIME end),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 1 then TS.SUM end),'FM999999990.00'),9,' ') as f106_1,
    lpad(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 2 then TS.TIME end),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(case when TS.PAY_TYPE_ID = '106' and TS.NSTR = 2 then TS.SUM end),'FM999999990.00'),9,' ') as f106_2,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '112', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '112', TS.SUM)),'FM999999990.00'),9,' ') as f112,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '114', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '114', TS.SUM)),'FM999999990.00'),9,' ') as f114,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '121', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '121', TS.SUM)),'FM999999990.00'),9,' ') as f121,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '124', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '124', TS.SUM)),'FM999999990.00'),9,' ') as f124,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '125', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '125', TS.SUM)),'FM999999990.00'),9,' ') as f125,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '111', TS.TIME)),'FM999990.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '111', TS.SUM)),'FM999999990.00'),9,' ') as f111,
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '211', TS.TIME)),'FM9999909.0'),9,' ')||chr(10)||
    lpad(to_char(sum(Decode(TS.PAY_TYPE_ID, '211', TS.SUM)),'FM999999990.00'),9,' ') as f211
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