SELECT count(*) 
FROM (
    select TW.TIME_BEGIN, TW.TIME_END, MAX(TIME_BEGIN) OVER() as MAX_TIME_BEGIN, MAX(TIME_END) OVER() as MAX_TIME_END
    from TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date-10, 'dd.mm.yyyy'),
            to_char(trunc(last_day(sysdate)), 'dd.mm.yyyy'), :p_per_num, :p_transfer_id)) TW 
    where TW.W_DATE = :p_date
) V   
/*если дата начала документа позже окончания графика или документ попадает в график работы и есть 
оправдательный документ на целый день*/                  
WHERE :p_date_begin  >= V.MAX_TIME_END  
    OR (((:p_date_begin >= V.TIME_BEGIN and :p_date_end <= V.TIME_END)
        OR (:p_date_begin >= V.TIME_BEGIN and :p_date_begin >= V.MAX_TIME_BEGIN and :p_date_end >= V.MAX_TIME_END))    
        AND exists(SELECT NULL FROM {0}.REG_DOC R 
            JOIN {0}.DOC_LIST D ON R.DOC_LIST_ID = D.DOC_LIST_ID
            WHERE :p_date_begin BETWEEN R.DOC_BEGIN AND R.DOC_END AND R.TRANSFER_ID IN 
                (select TR.TRANSFER_ID from {0}.TRANSFER TR start with TR.TRANSFER_ID = :p_transfer_id  
                connect by NOCYCLE prior TR.TRANSFER_ID = TR.FROM_POSITION OR TR.TRANSFER_ID = PRIOR TR.FROM_POSITION)
                AND D.SIGN_ALL_DAY = 1))