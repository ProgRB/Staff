update {0}.WORK_PAY_TYPE WP set WP.ORDER_ID = 
    (select EO.ORDER_ID from {0}.EMP_ORDER EO where EO.PER_NUM = :p_per_num and EO.SUBDIV_ID = :p_subdiv_id 
        and EO.SIGN_COMB = (select SIGN_COMB from {0}.TRANSFER where TRANSFER_ID = :p_transfer_id)
        and EO.DATE_ORDER = 
            (select max(EO1.DATE_ORDER) from {0}.EMP_ORDER EO1 where EO1.PER_NUM = :p_per_num and EO1.SUBDIV_ID = :p_subdiv_id 
                and EO1.SIGN_COMB = EO.SIGN_COMB
                and EO1.DATE_ORDER <= 
                (select W.WORK_DATE from {0}.WORKED_DAY W join {0}.WORK_PAY_TYPE WP1 on W.WORKED_DAY_ID = WP1.WORKED_DAY_ID
                    where WP1.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID) )
    )
where WP.WORK_PAY_TYPE_ID in (
        select WP.WORK_PAY_TYPE_ID from {0}.WORKED_DAY W join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
        where W.PER_NUM = :p_per_num and W.TRANSFER_ID in 
                (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id
                connect by nocycle prior TR1.FROM_POSITION = TR1.TRANSFER_ID or TR1.FROM_POSITION = prior TR1.TRANSFER_ID )
                 and W.WORK_DATE between :p_beginDate and :p_endDate and WP.PAY_TYPE_ID in (102, 302, 535))