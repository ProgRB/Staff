declare
    l_tariff_value Number;
    p_date Date := trunc(:p_date);
	p_subdiv_id Number := :p_subdiv_id;
	p_form_operation_id Number := :p_form_operation_id;
	p_code_degree Varchar(10) := :p_code_degree;
begin
    select tariff_value into l_tariff_value from apstaff.v_base_tariff where p_date between date_begin and date_end; 
    open :c for
		select 
			s.*,
			NULLIF(NVL(TAR_MONTH,0)
				+NVL(ROUND(TAR_MONTH*harm_percent/100,2),0)
				+NVL(ROUND(comb_tar*l_tariff_value,2),0)
				+NVL(ROUND(TAR_MONTH*secret_percent/100,2),0)
				+NVL(ROUND(TAR_MONTH*secret_add_percent/100,2),0), 0)*staff_count
			SUM_SAL_MONTH
		from
		(
			select
				staff_id,
				code_subdiv,
				staff_count,
				code_pos,
				pos_name,
				code_form_operation,
				tar_by_schema,
				code_degree,
				pos_note,
				classific,
				code_tariff_grid,
				case when tariff_grid_id is not null and classific>0 then tar_month
					 when tariff_grid_id is not null then round(tar_by_schema*l_tariff_value,2)
					 else round(tar_by_schema*l_tariff_value,0)
				end tar_month,
				harm_percent,
				comb_tar,
				secret_percent,
				secret_add_percent,
				case when count(staff_id) over (partition by staff_id)>1 then null else DATE_STAFF_BEGIN end DATE_STAFF_BEGIN,
				DATE_STAFF_END
			from 
				apstaff.staff
				join apstaff.subdiv using (subdiv_id)
				join apstaff.position using (pos_id)
				join apstaff.staff_period using (staff_id)
				left join apstaff.degree using (degree_id)
				left join (select tariff_grid_id, code_tariff_grid, tar_classif classific, tar_sal, tar_month from apstaff.tariff_grid_salary where p_date between tar_date and tariff_end_date and tar_classif>0) using (tariff_grid_id, classific)
				left join (select staff_id, addition_value as harm_percent from apstaff.staff_addition where type_staff_addition_id=1) using (staff_id)
				left join (select staff_id, addition_value as comb_tar from apstaff.staff_addition where type_staff_addition_id=2) using (staff_id)
				left join (select staff_id, addition_value as secret_percent from apstaff.staff_addition where type_staff_addition_id=3) using (staff_id)
				left join (select staff_id, addition_value as secret_add_percent from apstaff.staff_addition where type_staff_addition_id=4) using (staff_id)
				left join apstaff.form_operation using (form_operation_id)
			where
				p_date between date_staff_begin and nvl(date_staff_end, date'3000-01-1')
				and subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=p_subdiv_id connect by prior subdiv_id=parent_id)
				and (p_form_operation_id is null or form_operation_id=p_form_operation_id)
				and (p_code_degree is null or DECODE(code_degree,'04', DECODE(substr(code_pos,1,1),'2','041','3','042','043'), CODE_DEGREE) like p_code_degree||'%')
		) s
    order by code_subdiv, code_degree, pos_name;
end;