select 
(select count(tr.per_num) from {0}.transfer tr, {0}.subdiv sub, {0}.per_data pd where degree_id!=04 and tr.subdiv_id=sub.subdiv_id and pd.per_num=TR.PER_NUM and pd.retirer_sign=1 and (tr.date_transfer = (select max(date_transfer) from {0}.transfer tr2 where tr2.per_num = tr.per_num)) and tr.type_transfer_id!=3) as rtr,
(select count(tr.per_num) from {0}.transfer tr, {0}.subdiv sub, {0}.per_data pd, {0}.emp where degree_id!=04 and tr.subdiv_id=sub.subdiv_id and pd.per_num=TR.PER_NUM and emp.per_num=tr.per_num and upper(emp.emp_sex)='�' and pd.retirer_sign=1 and (tr.date_transfer = (select max(date_transfer) from {0}.transfer tr2 where tr2.per_num = tr.per_num)) and tr.type_transfer_id!=3) as rtr_w
from dual