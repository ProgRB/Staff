declare
    l_tariff_value Number;
    p_date Date := trunc(:p_date);
begin
    select tariff_value into l_tariff_value from apstaff.v_base_tariff where p_date between date_begin and date_end; 
    open :c for
        select
            staff_id,
            code_subdiv,
			staff_count,
            code_pos,
            pos_name,
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
            secret_add_percent
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
    where
        p_date between date_staff_begin and nvl(date_staff_end, date'3000-01-1')
        and subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
    order by code_subdiv, code_degree, pos_name;
end;