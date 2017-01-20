select count(*) from {0}.SUBDIV_FOR_TABLE S
where S.SUBDIV_ID = :p_subdiv_id and 
    (trunc(S.DATE_SALARY) >= trunc(:p_date_end)
    and not exists(select null from user_role_privs where GRANTED_ROLE = 'TABLE_EDIT_OLD'))