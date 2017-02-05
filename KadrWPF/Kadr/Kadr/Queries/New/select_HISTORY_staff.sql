	Select pos_name,subdiv_name,degree_name,gr_work_name,harmful_addition,max_tariff,vacant_sign,to_char(trunc(date_end_vacant)),to_char(trunc(date_begin_staff)),to_char(trunc(date_end_staff)),staffs.staffs_id 
        from {0}.staffs left join {0}.transfer on (staffs.staffs_id=transfer.staffs_id) left join {0}.position on (staffs.pos_id=position.pos_id) left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) 
	left join {0}.degree on (staffs.degree_id=degree.degree_id) full outer join {0}.gr_work on (transfer.gr_work_id=gr_work.gr_work_id)
        start with staffs.staffs_id={1} connect by prior staffs.make_from_id=staffs.staffs_id 
union 
        Select pos_name,subdiv_name,degree_name,gr_work_name,harmful_addition,max_tariff,vacant_sign,to_char(trunc(date_end_vacant)),to_char(trunc(date_begin_staff)),to_char(trunc(date_end_staff)),staffs.staffs_id 
        from {0}.staffs  left join {0}.transfer on (staffs.staffs_id=transfer.staffs_id) left join {0}.position on (staffs.pos_id=position.pos_id) 
	left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.degree on (staffs.degree_id=degree.degree_id) 
	full outer join {0}.gr_work on (transfer.gr_work_id=gr_work.gr_work_id) 
        start with staffs.staffs_id={1} connect by prior staffs.staffs_id=staffs.make_from_id 
order by staffs_id