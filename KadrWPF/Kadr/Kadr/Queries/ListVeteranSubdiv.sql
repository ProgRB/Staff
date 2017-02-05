select
rownum as num,
last_name, 
first_name, 
middle_name,
subd,
per_num,
dat_birth
from
(select
last_name, 
first_name, 
middle_name,
subd,
per_num,
dat_birth
 from 
(select distinct
em.emp_last_name as last_name, 
em.emp_first_name as first_name,
em.emp_middle_name as middle_name,
sub.code_subdiv as subd,
em.per_num as per_num,
em.emp_birth_date as dat_birth
from {1}.emp em, {1}.transfer tr, {1}.subdiv sub, {1}.reward r 
where em.per_num = tr.per_num and sub.subdiv_id = tr.subdiv_id and
r.per_num = em.per_num and 
sign_cur_work=1 and R.REWARD_NAME_ID in (select RN.REWARD_NAME_ID from {1}.REWARD_NAME RN
        where upper(RN.REWARD_NAME) like '%¬≈“≈–¿Õ%“–”ƒ¿%')
order by code_subdiv, emp_last_name)) where subd=lpad('{0}',3,'0')