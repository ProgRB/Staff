select T.NOTE, round(sum(time),2) TIME, round(sum(sum),2) SUM, 
    /*T.ORDER_NAME*/(select O.ORDER_NAME from {0}.ORDERS O where O.ORDER_ID = T.ORDER_ID) ORDER_NAME, 
	lpad(T.GROUP_MASTER,3,'0') GROUP_MASTER, sum(T.COUNT_YN) COUNT_YN,
    /*T.CODE_DEGREE*/(select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) CODE_DEGREE, 
	CASE WHEN T.REPL_SIGN = 1 then 'Зам-е' end REPL_SIGN,
    DECODE(T.SIGN_ACCOUNT,1,'X') SIGN_ACCOUNT_VIS,
    DECODE(T.SIGN_APPENDIX,1,'X') SIGN_APPENDIX_VIS,
	nvl(T.SIGN_VISIBLE,0) SIGN_VISIBLE, nvl(T.SIGN_APPENDIX,0) SIGN_APPENDIX
from {1}.SALARY_FROM_TABLE T
where T.APP_NAME_ID = 1 and T.START_PERIOD<=:p_date_end and T.END_PERIOD>=:p_date_begin
    and T.PER_NUM = :p_per_num and T.TRANSFER_ID in 
    (select TRANSFER_ID from APSTAFF.TRANSFER T start with T.TRANSFER_ID = :p_transfer_id
    CONNECT BY NOCYCLE PRIOR transfer_id = from_position or transfer_id = PRIOR from_position)
    /*and nvl(T.SIGN_VISIBLE,0) in (1, :p_SIGN_VIS) and nvl(T.SIGN_APPENDIX,0) in (1, :p_sign_appendix)*/
group by T.PAY_TYPE_ID,T.ORDER_ID,T.NSTR,T.GROUP_MASTER,T.DEGREE_ID,T.NOTE,REPL_SIGN,T.SIGN_ACCOUNT,T.SIGN_APPENDIX,T.SIGN_VISIBLE
order by nvl(REPL_SIGN,0),T.SIGN_ACCOUNT DESC,T.PAY_TYPE_ID,T.ORDER_ID, T.NSTR,T.GROUP_MASTER,T.DEGREE_ID