select CODE_SUBDIV as "Шифр", SUBDIV_NAME as "Наименование подразделения", SUBDIV_ID from
    (select S.CODE_SUBDIV, S.SUBDIV_NAME, S.SUBDIV_ID
    from {0}.SUBDIV S
    where S.SUBDIV_ID 
        in (select AG.SUBDIV_ID from {0}.ACCESS_GR_WORK AG where AG.GR_WORK_ID = :p_gr_work_id)
    order by S.CODE_SUBDIV ) V