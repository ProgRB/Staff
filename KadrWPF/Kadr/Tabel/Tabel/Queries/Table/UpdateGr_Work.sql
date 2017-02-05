/*merge into {0}.emp_gr_work EG
    using (select :PER_NUM as PER_NUM, :TRANSFER_ID as TRANSFER_ID, :GR_WORK_DATE_BEGIN as GR_WORK_DATE_BEGIN from dual) T1 
    on (EG.PER_NUM = T1.PER_NUM and EG.TRANSFER_ID = T1.TRANSFER_ID and trunc(EG.GR_WORK_DATE_BEGIN) = trunc(T1.GR_WORK_DATE_BEGIN))
    when matched then
        update set GR_WORK_ID = :gr_work_id, GR_WORK_DAY_NUM = :gr_work_day_num
    when not matched then
        insert(EMP_GR_WORK_ID, PER_NUM, TRANSFER_ID, GR_WORK_ID, GR_WORK_DATE_BEGIN, GR_WORK_DAY_NUM)
        values({0}.EMP_GR_WORK_ID_seq.nextval, :PER_NUM, :TRANSFER_ID, :GR_WORK_ID, trunc(:GR_WORK_DATE_BEGIN), :GR_WORK_DAY_NUM)*/
BEGIN
	{0}.EMP_GR_WORK_UPDATE(:PER_NUM, :TRANSFER_ID, :GR_WORK_ID, :GR_WORK_DATE_BEGIN, :GR_WORK_DAY_NUM);
END;