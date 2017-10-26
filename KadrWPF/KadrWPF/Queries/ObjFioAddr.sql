select distinct
	EM.EMP_LAST_NAME||' '||EM.EMP_FIRST_NAME||' '||EM.EMP_MIDDLE_NAME as fio, 
	(select pos_name from {1}.position pos where pos_id = tr.pos_id)||' ñ '||to_char(tr.date_transfer,'DD.MM.YYYY')||'ã.' as qualif ,
	' ñ '||to_char(tr.date_hire,'DD.MM.YYYY') as timeFull,
	to_char(em.emp_birth_date,'DD.MM.YYYY')||'ã.' as emp_birth_date,
	PAS.COUNTRY_BIRTH ||' '||PAS.CITY_BIRTH||' '||PAS.REGION_BIRTH||' '||PAS.DISTR_BIRTH||' '||PAS.LOCALITY_BIRTH as p_birth,
	reg_post_code||' '|| str.name_street ||' '|| reg.reg_house ||' '|| reg.reg_BULK ||' '|| reg.reg_FLAT as address 
from {1}.transfer tr, {1}.emp em, {1}.reward rew, 
	{1}.edu ed, {1}.passport pas, {1}.registr reg, {1}.street str 
where tr.per_num(+) = em.per_num and tr.per_num = rew.per_num(+) and tr.per_num = ed.per_num(+) and
	tr.per_num = pas.per_num(+) and tr.per_num = reg.per_num(+) 
	and (tr.sign_cur_work=1 or tr.date_transfer = (select max(date_transfer) from {1}.transfer tr2 where tr2.per_num = tr.per_num and type_transfer_id !=3)) 
	and em.per_num = to_number('{0}') and REG.REG_CODE_STREET = STR.CODE_STREET(+)