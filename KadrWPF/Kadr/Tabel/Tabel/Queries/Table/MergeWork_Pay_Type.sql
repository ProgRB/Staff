MERGE into {0}.WORK_PAY_TYPE W 
using (select :p_work_pay_type_id as WORK_PAY_TYPE_ID from dual) W2
    on (W.WORK_PAY_TYPE_ID = W2.WORK_PAY_TYPE_ID)
when matched then
    update set W.VALID_TIME = :p_valid_time, W.PAY_TYPE_ID = :p_pay_type_id 
when not matched then
    insert(WORK_PAY_TYPE_ID, WORKED_DAY_ID, PAY_TYPE_ID, VALID_TIME)
    values({0}.WORK_PAY_TYPE_ID_SEQ.nextval, :p_worked_day_id, :p_pay_type_id, :p_valid_time)