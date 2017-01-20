select rownum,
em.emp_last_name, 
em.emp_first_name, 
em.emp_middle_name,
em.per_num,
to_char (em.emp_birth_date,'YYYY') as year_birth,
te.TE_name, 
dg.degree_name,
a_d.classific,
tr.date_transfer,
rd.reason_name
from {3}.emp em, {3}.degree dg, {3}.transfer tr, {3}.account_data a_d, {3}.reason_dismiss rd, {3}.subdiv sub, {3}.type_edu te, {3}.Edu ed where tr.degree_id in (1,2) and trunc(tr.date_transfer) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') and
em.per_num = tr.per_num and tr.reason_id = rd.reason_id and tr.degree_id = dg.degree_id and type_transfer_id = 3 and tr.transfer_id = a_d.transfer_id(+)
and tr.subdiv_id = sub.subdiv_id and code_subdiv = to_number('{2}') and ed.type_edu_id = te.type_edu_id and ed.per_num = em.per_num