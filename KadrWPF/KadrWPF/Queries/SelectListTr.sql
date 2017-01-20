select * from (
    select s.CODE_SUBDIV, s.subdiv_id, em.PER_NUM, em.EMP_LAST_NAME, em.EMP_FIRST_NAME, em.EMP_MIDDLE_NAME, 
        ps.CODE_POS, ps.POS_NAME, tr.TRANSFER_ID/*,
        (CASE WHEN EXISTS(
            select 1 from {0}.WORKED_DAY WD 
            where WD.PER_NUM = em.PER_NUM and trunc(WD.WORK_DATE) < trunc(sysdate) and 
                not (
                (Round(WD.FROM_PERCO / 3600, 2) = Round(WD.FROM_GRAPH / 3600, 2) and
                Round(WD.FROM_GRAPH / 3600, 2) = 
                nvl((select Round(sum(WP.VALID_TIME) / 3600, 2) from {0}.WORK_PAY_TYPE WP 
                where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID group by WP.WORKED_DAY_ID), 0)) 
                or 
                (Round(WD.FROM_GRAPH / 3600, 2) > Round(WD.FROM_PERCO / 3600, 2) 
                    and Round(WD.FROM_GRAPH / 3600, 2) = Round((WD.FROM_PERCO + 
                nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
                left join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
                left join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
                where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.DOC_TYPE = 1 
                group by WP.WORKED_DAY_ID), 0)) / 3600, 2))
                or 
                (Round(WD.FROM_GRAPH / 3600, 2) < Round(WD.FROM_PERCO / 3600, 2) 
                    and Round(WD.FROM_GRAPH / 3600, 2) = Round((WD.FROM_PERCO - 
                nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
                left join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
                left join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
                where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.DOC_TYPE = 2 
                group by WP.WORKED_DAY_ID), 0)) / 3600, 2)) 
                )    
                )
        THEN 1 ELSE 0 END) as IsPink  */
    from {0}.emp em 
    left join {0}.transfer tr on (em.PER_NUM = tr.PER_NUM and tr.SIGN_CUR_WORK = 1)
    left join {0}.subdiv s on (tr.subdiv_id = s.subdiv_id) 
    left join {0}.position ps on (tr.pos_id = ps.pos_id)
) 
emp_link {1} order by per_num