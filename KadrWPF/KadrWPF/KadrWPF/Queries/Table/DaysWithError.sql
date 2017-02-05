select to_char(WD.WORK_DATE, 'DD.MM.YYYY') from {0}.WORKED_DAY WD 
where WD.WORK_DATE between :p_date_begin and :p_date_end and WD.PER_NUM = :p_per_num and WD.TRANSFER_ID in 
    (
        select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id
        connect by nocycle prior TR1.TRANSFER_ID = TR1.FROM_POSITION or TR1.TRANSFER_ID = prior TR1.FROM_POSITION            
    )
    and 
    not (
    abs(WD.FROM_PERCO - (
        nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 202) and WP.REG_DOC_ID is null),0) +
        nvl((select sum(WP.VALID_TIME) 
            from {0}.WORK_PAY_TYPE WP 
            join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            join {0}.ORDERS using (ORDER_ID)
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 2 
            group by WP.WORKED_DAY_ID),0))) between 0 and 75 and 
            abs(WD.FROM_GRAPH - (
        nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 202)  and WP.REG_DOC_ID is null),0) +
        nvl((select sum(WP.VALID_TIME) 
            from {0}.WORK_PAY_TYPE WP 
            join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            join {0}.ORDERS using (ORDER_ID)
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 1 
            group by WP.WORKED_DAY_ID),0))) between 0 and 75
    /*abs((WD.FROM_PERCO - 
    nvl((select sum(WP.VALID_TIME) 
            from {0}.WORK_PAY_TYPE WP 
            join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 2 
            group by WP.WORKED_DAY_ID),0) + 
    nvl((select sum(WP.VALID_TIME) 
            from {0}.WORK_PAY_TYPE WP 
            join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
            join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
            where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 1 
            group by WP.WORKED_DAY_ID),0)) - WD.FROM_GRAPH) between 0 and 75 */
    )