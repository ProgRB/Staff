select CODE_SUBDIV as "����", SUBDIV_NAME as "������������ �������������", SUBDIV_ID from
    (select S.CODE_SUBDIV, S.SUBDIV_NAME, S.SUBDIV_ID
    from {0}.SUBDIV S
    where S.SUB_ACTUAL_SIGN = 1 and nvl(S.CODE_SUBDIV,'0') != '0' and nvl(S.PARENT_ID,0) = 0
        and S.SUBDIV_ID not in (select AG.SUBDIV_ID from {0}.ACCESS_GR_WORK AG where AG.GR_WORK_ID = :p_gr_work_id)
    order by S.CODE_SUBDIV ) V