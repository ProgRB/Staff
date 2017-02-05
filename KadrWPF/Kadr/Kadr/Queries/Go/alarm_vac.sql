select t.transfer_id,t.per_num,
		emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,code_subdiv 
from 
{0}.vs_current_all vs 
join {0}.transfer t on (t.transfer_id=vs.transfer_now)
join {0}.emp e on (t.per_num=e.per_num)
join {0}.subdiv s on (s.subdiv_id=t.subdiv_id)
where sign_cur_work=1 and actual_begin is null and plan_begin<=SYSDATE 
and t.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
order by emp_last_name,emp_first_name,emp_middle_name