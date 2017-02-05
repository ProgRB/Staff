select WD.WORK_DATE from apstaff.WORKED_DAY WD 
where WD.PER_NUM = :p_PER_NUM 
    and 
    trunc(WD.WORK_DATE) between 
         trunc(:beginDate) and trunc(:endDate)   
    and 
    not (
    (Round(WD.FROM_PERCO / 3600, 2) = Round(WD.FROM_GRAPH / 3600, 2) and
    Round(WD.FROM_GRAPH / 3600, 2) = 
    nvl((select Round(sum(WP.VALID_TIME) / 3600, 2) 
            from apstaff.WORK_PAY_TYPE WP
            left join apstaff.PAY_TYPE P on (WP.PAY_TYPE_ID = P.PAY_TYPE_ID)
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and P.SIGN_ADDITION = 0  group by WP.WORKED_DAY_ID), 0)) 
    or 
    (Round(WD.FROM_GRAPH / 3600, 2) > Round(WD.FROM_PERCO / 3600, 2) 
        and (Round(WD.FROM_GRAPH / 3600, 2) - Round((WD.FROM_PERCO + 
    nvl((select sum(WP.VALID_TIME) 
            from apstaff.WORK_PAY_TYPE WP 
            left join apstaff.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            left join apstaff.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            left join apstaff.PAY_TYPE P on (WP.PAY_TYPE_ID = P.PAY_TYPE_ID)
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and P.SIGN_ADDITION = 0 and D.DOC_TYPE = 1 
            group by WP.WORKED_DAY_ID), 0)) / 3600, 2) between 0 and 0.02))
    or 
    Round((WD.FROM_PERCO - 
    nvl((select sum(WP.VALID_TIME) 
            from apstaff.WORK_PAY_TYPE WP 
            left join apstaff.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            left join apstaff.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            left join apstaff.PAY_TYPE P on (WP.PAY_TYPE_ID = P.PAY_TYPE_ID) 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and P.SIGN_ADDITION = 0 and D.DOC_TYPE = 2 
            group by WP.WORKED_DAY_ID), 0)) / 3600, 2) - Round(WD.FROM_GRAPH / 3600, 2) between 0 and 0.02
    or
    abs(Round((WD.FROM_PERCO - 
    nvl((select sum(WP.VALID_TIME) 
            from apstaff.WORK_PAY_TYPE WP 
            left join apstaff.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            left join apstaff.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            left join apstaff.PAY_TYPE P on (WP.PAY_TYPE_ID = P.PAY_TYPE_ID) 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and P.SIGN_ADDITION = 0 and D.DOC_TYPE = 2 
            group by WP.WORKED_DAY_ID),0) + 
    nvl((select sum(WP.VALID_TIME) 
            from apstaff.WORK_PAY_TYPE WP 
            left join apstaff.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            left join apstaff.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            left join apstaff.PAY_TYPE P on (WP.PAY_TYPE_ID = P.PAY_TYPE_ID)
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and P.SIGN_ADDITION = 0 and D.DOC_TYPE = 1 
            group by WP.WORKED_DAY_ID),0)) / 3600, 2) - Round(WD.FROM_GRAPH / 3600, 2)) between 0 and 0.02
    )