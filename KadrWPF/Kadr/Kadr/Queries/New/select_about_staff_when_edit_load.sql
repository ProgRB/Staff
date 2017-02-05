select subdiv_name,degree_name,code_pos,harmful_addition, gr_work_name, vacant_sign,date_begin_staff,date_end_staff,
	date_end_vacant,staffs.subdiv_id,pos_name,type_person,comment_to_pos,order_name,staff_sign,
	'('||code_tariff_grid||')'||tariff_grid_name t_g,classific,
	tar_by_schema,code_subdiv,add_exp_area,staffs.order_id
    from {0}.staffs 
		left join {0}.transfer t on (staffs.staffs_id=t.staffs_id)
		left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) 
		left join {0}.position on (staffs.pos_id=position.pos_id)
		left join {0}.degree on (staffs.degree_id=degree.degree_id) 
		left join {0}.gr_work on (t.gr_work_id=gr_work.gr_work_id) 
		left join {0}.orders on (staffs.order_id=orders.order_id)
		left join {0}.tariff_grid on (staffs.tarif_grid_id=tariff_grid.tariff_grid_id)
where staffs.staffs_id={1}