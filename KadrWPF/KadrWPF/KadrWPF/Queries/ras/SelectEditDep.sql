select lpad(to_char(D.CODE_SUBDIV),3,'0') as CODE_SUBDIV, D.PER_NUM, to_char(D.DEP_MONTH,'FM00') as DEP_MONTH, 
    to_char(D.DEP_TAX_CODE,'FM00') as DEP_TAX_CODE, 
    to_char(D.DEP_COUNT114,'FM00'), to_char(D.DEP_COUNT115,'FM00'), to_char(D.DEP_COUNT116,'FM00'), to_char(D.DEP_COUNT117,'FM00'), 
    to_char(D.DEP_COUNT118,'FM00'), to_char(D.DEP_COUNT119,'FM00'), to_char(D.DEP_COUNT120,'FM00'), to_char(D.DEP_COUNT121,'FM00'),
    D.SIGN_COMB 
from {0}.DEPENDENTS D
where D.SIGN_EDIT = 1
order by D.CODE_SUBDIV, D.PER_NUM, D.DEP_MONTH