select rownum as num_ord,
subd, 
per_num, 
last_name, 
first_name, 
middle_name, 
dat_birth,
dat_dsms
from 
(select
sub.code_subdiv as subd, 
em.per_num as per_num, 
em.emp_last_name as last_name, 
em.emp_first_name as first_name, 
em.emp_middle_name as middle_name, 
EM.EMP_BIRTH_DATE as dat_birth,
(case(type_transfer_id)
when 3 then date_transfer
else null
end) as dat_dsms,
type_transfer_id                           
from {0}.emp em, {0}.transfer tr1, {0}.subdiv sub, {0}.per_data p_d
where EM.PER_NUM = tr1.per_num and TR1.SUBDIV_ID = sub.subdiv_id and p_d.insurance_num is null and EM.PER_NUM = p_d.per_num
and tr1.date_transfer = (select max(date_transfer) from {0}.transfer tr2 where tr2.per_num = tr1.per_num) order by last_name)
