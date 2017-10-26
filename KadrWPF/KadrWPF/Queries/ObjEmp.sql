declare
	p_transfer_id number := :p_transfer_id;
	p_worker_id number;
	p_per_num varchar(10);
begin
	select worker_id, per_num into p_worker_id, p_per_num from apstaff.transfer where transfer_id=p_transfer_id;
	open :c1 for
	select
		emp_last_name, emp_first_name, emp_middle_name, emp_birth_date,
		PAS.COUNTRY_BIRTH ||' '||PAS.CITY_BIRTH||' '||PAS.REGION_BIRTH||' '||PAS.DISTR_BIRTH||' '||PAS.LOCALITY_BIRTH,
		(select min(date_transfer) from apstaff.transfer where worker_id=p_worker_id) date_hire,
		(select max(pos_name) keep (dense_rank last order by date_transfer) from apstaff.transfer where worker_id=p_worker_id
			and type_transfer_id in (1,2)) pos_name,
		
	from
		apstaff.emp
		left join (select * from apstaff.passport) using (per_num)
	where
		per_num=p_per_num;
select ins.instit_name||' - '|| T_P_S.TYPE_POSTG_STUDY_NAME as edu_level 
from {1}.instit ins, {1}.postg_study p_s, {1}.type_postg_study t_p_s 
where 
	P_S.TYPE_POSTG_STUDY_ID = T_P_S.TYPE_POSTG_STUDY_ID(+) 
	and P_S.PER_NUM = to_number('{0}') 
	and INS.INSTIT_ID = P_S.INSTIT_ID(+)