declare
    p_date Date := trunc(:p_date,'month');
begin
    open :c for
        select
            emp_staff_id,
			emp_last_name,
			emp_first_name,
			emp_middle_name,
			per_num,
			code_degree,
			date_start_work,
			date_end_work,
            work_cf,
            pos_name,
            code_pos,
            code_tariff_grid,
            classific,
            tarif_cf
        from
        (
            select 
               emp_staff_id
            from
                apstaff.emp_staff
            where 
                staff_id=:p_staff_id
                and p_date between date_start_work and nvl(date_end_work, date'3000-01-01')
        )
        join (select emp_staff_id, date_start_work, date_end_work, work_cf, worker_id from apstaff.emp_staff join (select transfer_id, worker_id from apstaff.transfer) using (transfer_id)) using (emp_staff_id)
        join (select worker_id,
				max(transfer_id) keep (dense_rank last order by date_transfer) transfer_id
              from
                apstaff.transfer
              where date_transfer<:p_date+86399/86400
                and type_transfer_id!=3
              group by worker_id) using (worker_id)
        join apstaff.transfer using (transfer_id)
		join apstaff.emp using (per_num)
        join apstaff.position using (pos_id)
        join apstaff.degree using (degree_id)
        join apstaff.subdiv using (subdiv_id)
        left join (select transfer_id, salary as tarif_cf, classific, code_tariff_grid from apstaff.account_data left join apstaff.tariff_grid using (tariff_grid_id)) using (transfer_id);              
end;