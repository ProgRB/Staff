select 
    transfer_id,
    (select max(coalesce(error1,error2,error3)) KEEP (DENSE_RANK FIRST ORDER BY error1||';'||error2||';'||error3) from 
        TABLE({0}.VAC_SCHED_PACK.GET_RECALCED_PERIODS(t.transfer_id)) where NVL2(error1,error1||';',null)||NVL2(error2, error2||';',null)||error3 is not null) as err_text
from 
    (select * from {0}.transfer where sign_cur_work=1 and subdiv_id=:p_subdiv_id) t
where
    exists (select 1 from  TABLE({0}.VAC_SCHED_PACK.GET_RECALCED_PERIODS(t.transfer_id)) 
			where NVL2(error1,error1||';',null)||NVL2(error2, error2||';',null)||error3 is not null)