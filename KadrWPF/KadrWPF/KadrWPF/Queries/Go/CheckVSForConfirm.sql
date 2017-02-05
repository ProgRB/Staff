select 
    worker_id,
    transfer_id,
    max(coalesce(error1,error2,error3)) KEEP (DENSE_RANK FIRST ORDER BY error1||';'||error2||';'||error3) err_text
from 
    (select * from apstaff.transfer where sign_cur_work=1 and worker_id in (select column_value from TABLE(:p_worker_ids))) t,
    TABLE(apstaff.VAC_SCHED_PACK.GET_RECALCED_PERIODS(t.transfer_id)) 
where
    error1 is not null or error2 is not null or error3 is not null
group by worker_id, transfer_id