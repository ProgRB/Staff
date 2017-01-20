select to_char(sysdate,'DD.MM.YYYY') as dat,
emp.per_num,
p_d.INN,
p_d.insurance_num,
substr(emp.emp_last_name, 1, 1) as alphab,
	(select TYPE_TERM_TRANSFER_NAME from {0}.TYPE_TERM_TRANSFER ch where tr.CHAR_WORK_ID = ch.TYPE_TERM_TRANSFER_ID) as CHAR_WORK_NAME as char_w,
(case (tr.sign_comb)
when 1 then '�� ����.'
when 0 then '��������'
end) as type_w,
emp.emp_sex,
(select contr_emp from {1}.transfer where type_transfer_id =1 and per_num = emp.per_num and date_transfer = (select max(date_transfer) from {1}.transfer tr2 where type_transfer_id = 1 and tr2.per_num = emp.per_num )) as contr_emp,
to_char((select date_contr from {1}.transfer where type_transfer_id =1 and per_num = emp.per_num and date_transfer = (select max(date_transfer) from {1}.transfer tr2 where type_transfer_id = 1 and tr2.per_num = emp.per_num)),'DD.MM.YYYY') as date_contr,
emp.emp_last_name,
emp.emp_first_name,
emp.emp_middle_name,
to_char(emp.emp_birth_date,'DD')||' '||
(case to_char(emp.emp_birth_date,'MM')
when '01' then '������ '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '02' then '������� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '03' then '����� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '04' then '������ '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '05' then '��� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '06' then '���� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '07' then '���� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '08' then '������� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '09' then '�������� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '10' then '������� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '11' then '������ '||to_char(emp.emp_birth_date,'YYYY')||' �.'
when '12' then '������� '||to_char(emp.emp_birth_date,'YYYY')||' �.'
end)
as birth_date,
PAS.COUNTRY_BIRTH ||' '||PAS.CITY_BIRTH||' '||PAS.REGION_BIRTH||' '||PAS.DISTR_BIRTH||' '||PAS.LOCALITY_BIRTH as p_birth,
PAS.CITIZENSHIP,
l.Language,
pos.pos_name,
pos.code_pos
from {1}.position pos, {1}.know_lang k_l, {1}.lang l, {1}.emp, {1}.passport pas,  {1}.transfer tr, {1}.per_data p_d 
where tr.per_num = emp.per_num and pas.per_num = emp.per_num and 
emp.per_num = p_d.per_num and emp.per_num = to_number('{0}') and k_l.per_num(+) = emp.per_num and K_L.LANG_ID = l.lang_id(+) and
 (tr.sign_cur_work = 1 or ((select max(date_transfer) from {1}.transfer) =( select max(date_transfer) from {1}.transfer) and tr.type_transfer_id=3 )) and pos.pos_id = tr.pos_id order by type_w