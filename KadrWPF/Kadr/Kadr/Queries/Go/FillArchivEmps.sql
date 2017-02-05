select transfer_id,
	code_subdiv,
    emp_last_name||' '||emp_first_name||' '||emp_middle_name as fio,
    per_num,
    decode(sign_comb,1,'X','') sign_comb,
    pos_name,
    code_degree,
    name_group_master as group_master_name
from 
	 {0}.transfer t 
     join {0}.emp e using (per_num)
     join {0}.position p using (pos_id)
     join ( select subdiv_id, code_subdiv from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior  subdiv_id=parent_id) s on (t.subdiv_id=s.subdiv_id)
     join {0}.degree d using (degree_id)
     left join (select max(name_group_master) KEEP (DENSE_RANK FIRST order by begin_group) as name_group_master , 
                    worker_id 
                 from
                    {0}.emp_group_master egm join (select transfer_id, worker_id from {0}.transfer) using (transfer_id)
                    where NVL(begin_group,date'1000-01-01')<add_months(trunc(:p_year,'month'),12)  and  NVL(end_group,date'3000-01-01')>trunc(:p_year,'month')
				group by worker_id) egm on (t.worker_id=egm.worker_id)
where t.type_transfer_id=3 and date_transfer between trunc(:p_year,'year') and add_months(trunc(:p_year,'year'),12)
 and per_num=decode(:p_per_num, null, per_num,:p_per_num)
order by date_transfer desc