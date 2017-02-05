merge into {0}.EMP_WAYBILL WB
using (SELECT case when exists(
            select null from {0}.EMP_WAYBILL W 
            where W.PER_NUM = :p_per_num)
            then (select W.EMP_WAYBILL_ID 
                from {0}.EMP_WAYBILL W 
                where W.PER_NUM = :p_per_num)
            else -1 end EMP_WAYBILL_ID
            from dual
        ) T1 
    on (WB.EMP_WAYBILL_ID = T1.EMP_WAYBILL_ID)
when matched then
    update set PER_NUM = :p_PER_NUM, TRANSFER_ID = :p_TRANSFER_ID where WB.EMP_WAYBILL_ID = T1.EMP_WAYBILL_ID
when not matched then
    insert(EMP_WAYBILL_ID, PER_NUM, TRANSFER_ID)
    values({0}.EMP_WAYBILL_ID_seq.nextval, :p_PER_NUM, :p_TRANSFER_ID)