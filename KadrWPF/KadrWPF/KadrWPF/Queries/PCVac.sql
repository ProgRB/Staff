select
t_p.name_priv,
e_p.priv_num_doc,
to_char(E_P.DATE_GIVE,'DD.MM.YYYY') as DATE_GIVE,
B_D.BASE_DOC_NAME
from {1}.emp_priv e_p, {1}.type_priv t_p, {1}.base_doc b_d where E_P.TYPE_PRIV_ID=T_P.TYPE_PRIV_ID(+) and 
E_P.BASE_DOC_ID=B_D.BASE_DOC_ID(+) and e_p.per_num=to_number('{0}')