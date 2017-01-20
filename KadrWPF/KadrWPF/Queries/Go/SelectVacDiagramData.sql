declare
begin
	open :c1 for select * from apstaff.type_vac;
	open :c2 for select subdiv_id, 
					case when subdiv_id=0 then '000' else code_subdiv end code_subdiv,
					case when subdiv_id=0 then 'У-УАЗ' else subdiv_name end subdiv_name,
					sub_actual_sign,
					type_subdiv_id,
					sub_date_start,
					sub_date_end
				from apstaff.subdiv 
				where 
					subdiv_id in (select subdiv_id from apstaff.subdiv_roles where app_name='VS_VIEW')
					and (type_subdiv_id!=6 or subdiv_id=0);
end;