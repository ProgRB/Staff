select (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    T.PER_NUM, 
	{0}.PADEG.CASE_WORD(e.emp_last_name,'Дательный','last_name',e.emp_sex)||' '||
    {0}.PADEG.CASE_WORD(e.EMP_FIRST_NAME,'Дательный','FIRST_NAME',e.emp_sex)||' '||
    {0}.PADEG.CASE_WORD(e.EMP_MIDDLE_NAME,'Дательный','MIDDLE_NAME',e.emp_sex) FIO,
    T.DATE_HIRE,
    (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T.POS_ID) POS_NAME
from {0}.TRANSFER T 
join {0}.EMP E on T.PER_NUM = E.PER_NUM
WHERE T.TRANSFER_ID = :p_TRANSFER_ID