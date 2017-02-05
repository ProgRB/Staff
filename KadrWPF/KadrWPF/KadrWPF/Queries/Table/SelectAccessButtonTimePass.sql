/* Проверка возможности нажания кнопки Принять время по проходам: 11 категория, суммированный учет, совместители*/
with TRANSFER_TREE as 
    (select TR.TRANSFER_ID, TR.PER_NUM, TR.DEGREE_ID, TR.SIGN_COMB, TRUNC(TR.DATE_TRANSFER) DATE_TRANSFER, 
        LEAD(trunc(date_transfer),1, DECODE(type_transfer_id,3,TRUNC(date_transfer)+1,date'3000-01-01')) OVER (ORDER BY date_transfer)-1/86400 end_transfer
    from {0}.TRANSFER TR
    start with TR.TRANSFER_ID = :p_transfer_id 
    connect by nocycle prior TR.TRANSFER_ID = TR.FROM_POSITION or TR.TRANSFER_ID = prior TR.FROM_POSITION)
select SIGN_SUMMARIZE+SIGN_DEGREE_11+SIGN_COMB SIGN_ACCESS
from 
    (select NVL(SIGN_SUMMARIZE,0) SIGN_SUMMARIZE 
    from 
        (select GW.SIGN_SUMMARIZE,TRUNC(gr_work_date_begin) GR_WORK_DATE_BEGIN,                            
            LEAD(TRUNC(gr_work_date_begin)-1/86400,1,date'3000-01-01') OVER (ORDER BY gr_work_date_begin) GR_WORK_DATE_END
        from {0}.EMP_GR_WORK e 
        join {0}.GR_WORK GW using(GR_WORK_ID)
        where e.transfer_id in (select transfer_id from transfer_tree)) V
    where :p_date between V.GR_WORK_DATE_BEGIN and V.GR_WORK_DATE_END)
join (select DECODE(D.CODE_DEGREE,'11',1,0) SIGN_DEGREE_11, SIGN_COMB
    from 
        (select * from TRANSFER_TREE) T
        join {0}.DEGREE D using(DEGREE_ID)
    where :p_date between DATE_TRANSFER and END_TRANSFER
    ) on 1=1  


                             