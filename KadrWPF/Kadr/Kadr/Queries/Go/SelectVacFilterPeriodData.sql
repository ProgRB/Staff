begin
	open :c1 for select distinct subdiv_id, code_subdiv, subdiv_name from {0}.SUBDIV_ROLES_ALL where APP_NAME='VS_VIEW';
	open :c2 for select * from {0}.degree;
	open :c3 for select * from {0}.form_operation;
end;