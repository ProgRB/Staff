select T.NOTE as "��� �����", round(sum(time),2) as "����", round(sum(sum),2) as "�����", 
    /*T.ORDER_NAME*/(select O.ORDER_NAME from {0}.ORDERS O where O.ORDER_ID = T.ORDER_ID) as "�����", 
	lpad(T.GROUP_MASTER,3,'0') as "��", sum(T.COUNT_YN) as "�/�",
    /*T.CODE_DEGREE*/(select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) as "��", 
	CASE WHEN T.REPL_SIGN = 1 then '���-�' end as "���-�",
    DECODE(T.SIGN_ACCOUNT,1,'X') "����� �� ��",
    DECODE(T.SIGN_APPENDIX,1,'X') "����� � ����-��",
	nvl(T.SIGN_VISIBLE,0) SIGN_VISIBLE, nvl(T.SIGN_APPENDIX,0) SIGN_APPENDIX
/* ��������� �� ����� ������� ������� ������
from {0}.TEMP_SALARY T 
where T.TEMP_SALARY_ID = :p_temp_salary_id and T.SIGN_COMB = :p_sign_comb     */
from TABLE({0}.CALC_SALARY_PIPE(:p_SUBDIV_ID, :p_BEGINDATE, :p_ENDDATE, :p_TRANSFER_ID, :p_sign_calc)) T
group by T.PAY_TYPE_ID,T.ORDER_ID,T.NSTR,T.GROUP_MASTER,T.DEGREE_ID,T.NOTE,REPL_SIGN,T.SIGN_ACCOUNT,T.SIGN_APPENDIX,T.SIGN_VISIBLE
order by nvl(REPL_SIGN,0),T.SIGN_ACCOUNT DESC,T.PAY_TYPE_ID,T.ORDER_ID, T.NSTR,T.GROUP_MASTER,T.DEGREE_ID