select
rownum as num,
last_name, 
first_name, 
middle_name,
subd,
per_num,
dat_retir,
num_ret_ass,
dat_recal,
dat_birth,
type_accs
from
(select
last_name, 
first_name, 
middle_name,
subd,
per_num,
dat_retir,
num_ret_ass,
dat_recal,
dat_birth,
type_accs
 from 
(select distinct
em.emp_last_name as last_name, 
em.emp_first_name as first_name,
em.emp_middle_name as middle_name,
sub.code_subdiv as subd,
em.per_num as per_num,
e_p.date_start_priv as dat_retir,
e_p.priv_num_doc as num_ret_ass,
e_p.date_recalc as dat_recal,
em.emp_birth_date as dat_birth,
t_p.name_priv as type_accs
from {1}.emp em, {1}.transfer tr, {1}.subdiv sub, {1}.emp_priv e_p, {1}.type_priv t_p where em.per_num = tr.per_num and sub.subdiv_id = tr.subdiv_id and
em.per_num = e_p.per_num and t_p.type_priv_id = e_p.type_priv_id and tr.date_transfer = (select max(date_transfer) from {1}.transfer tr2 where tr2.per_num = tr.per_num and tr2.type_transfer_id !=3) order by code_subdiv, emp_last_name)) where subd=to_number('{0}')