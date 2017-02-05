select rownum as num,
last_name,
first_name,
middle_name,
subd,
per_num,
categ,
dat_birth,
dat_dsms
from (
select
em.emp_last_name as last_name, 
em.emp_first_name as first_name, 
em.emp_middle_name as middle_name,
sub.code_subdiv as subd,
em.per_num as per_num,
dg.degree_name as categ,
em.emp_birth_date as dat_birth,
tr.date_transfer as dat_dsms
from {2}.emp em,{2}.per_data pd, {2}.transfer tr, {2}.subdiv sub, {2}.degree dg where tr.type_transfer_id = 3 and ((pd.retirer_sign = '1') or ( em.emp_sex = 'Ì' and (to_char (em.emp_birth_date,'YYYY')<=(to_char (to_date('{1}','dd.mm.yyyy'), 'YYYY')-60)))  or (em.emp_sex = 'Æ' and (to_char(em.emp_birth_date,'YYYY')<=((to_char(to_date('{1}','dd.mm.yyyy'),'YYYY'))-55)))) and 
em.per_num = tr.per_num and pd.per_num = em.per_num and tr.subdiv_id = sub.subdiv_id and tr.degree_id = dg.degree_id and 
trunc(tr.df_book_dismiss) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') order by date_transfer)