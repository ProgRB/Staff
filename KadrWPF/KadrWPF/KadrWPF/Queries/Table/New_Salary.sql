select T.NOTE as "Вид оплат", round(sum(time),2) as "Часы", round(sum(sum),2) as "Сумма", 
    /*T.ORDER_NAME*/(select O.ORDER_NAME from {0}.ORDERS O where O.ORDER_ID = T.ORDER_ID) as "Заказ", 
	lpad(T.GROUP_MASTER,3,'0') as "ГМ", sum(T.COUNT_YN) as "Я/Н",
    /*T.CODE_DEGREE*/(select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) as "КТ", 
	CASE WHEN T.REPL_SIGN = 1 then 'Зам-е' end as "Зам-я",
    DECODE(T.SIGN_ACCOUNT,1,'X') "Сброс на ЗП",
    DECODE(T.SIGN_APPENDIX,1,'X') "Сброс в Числ-ть"
from {0}.TEMP_SALARY T 
where T.TEMP_SALARY_ID = :p_temp_salary_id and T.SIGN_COMB = :p_sign_comb 
    and nvl(T.SIGN_VISIBLE,0) in (1, :p_SIGN_VIS) and nvl(T.SIGN_APPENDIX,0) in (1, :p_sign_appendix)
group by T.PAY_TYPE_ID,T.ORDER_ID,T.NSTR,T.GROUP_MASTER,T.DEGREE_ID,T.NOTE,REPL_SIGN,T.SIGN_ACCOUNT,T.SIGN_APPENDIX
order by nvl(REPL_SIGN,0),T.SIGN_ACCOUNT DESC,T.PAY_TYPE_ID,T.ORDER_ID, T.NSTR,T.GROUP_MASTER,T.DEGREE_ID