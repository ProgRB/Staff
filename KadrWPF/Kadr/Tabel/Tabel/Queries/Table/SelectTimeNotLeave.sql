SELECT count(*) 
FROM (
    select TW.TIME_BEGIN, TW.TIME_END, MAX(TIME_BEGIN) OVER() as MAX_TIME_BEGIN, MAX(TIME_END) OVER() as MAX_TIME_END
    from TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date-10, 'dd.mm.yyyy'),
            to_char(trunc(last_day(sysdate)), 'dd.mm.yyyy'), :p_per_num, :p_transfer_id)) TW 
    where TW.W_DATE = :p_date
) V   
/*если дата начала документа позже окончания графика или документ попадает в график работы и есть 
оправдательный документ на целый день*/                  
WHERE :p_date_begin < V.TIME_END and :p_date_end > V.TIME_BEGIN