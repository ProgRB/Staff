select rownum,
emp.emp_last_name||' '||emp.emp_first_name||' '||emp.emp_middle_name as FIO,
to_char(emp.emp_birth_date,'dd.mm.yyyy') as birth_date,
pos.pos_name,
(case (pgs.acad_degree)
when 1 then 'Доктор наук'
when 2 then 'Кандидат наук'
else ''
end) as acad_degree,
ins.instit_name
from {0}.emp join {0}.postg_study pgs on (emp.per_num = pgs.per_num) join {0}.instit ins on (pgs.instit_id = ins.instit_id) join {0}.transfer tr on (tr.per_num = emp.per_num) 
join {0}.position pos on (tr.pos_id = pos.pos_id) and pgs.acad_degree is not null and tr.sign_cur_work=1