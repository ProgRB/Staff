select 
(select count(tr.per_num) from {1}.transfer tr, {1}.position pos, {1}.subdiv sub, {1}.REWARD R 
where degree_id=04 and sub.code_subdiv=lpad('{0}',3,'0') and tr.subdiv_id=sub.subdiv_id and r.per_num=TR.PER_NUM and 
pos.pos_id=tr.pos_id and substr(pos.code_pos,1,1)=3 and R.REWARD_NAME_ID in (select RN.REWARD_NAME_ID from {1}.REWARD_NAME RN
        where upper(RN.REWARD_NAME) like '%¬≈“≈–¿Õ%“–”ƒ¿%') and 
(tr.date_transfer = (select max(date_transfer) from {1}.transfer tr2 where tr2.per_num = tr.per_num)) and tr.type_transfer_id!=3) as rtr,
(select count(tr.per_num) from {1}.transfer tr, {1}.position pos, {1}.subdiv sub, {1}.REWARD R, {1}.emp 
where degree_id=04 and sub.code_subdiv=lpad('{0}',3,'0') and tr.subdiv_id=sub.subdiv_id and r.per_num=TR.PER_NUM and 
pos.pos_id=tr.pos_id and substr(pos.code_pos,1,1)=3 and emp.per_num=tr.per_num and upper(emp.emp_sex)='∆' and 
R.REWARD_NAME_ID in (select RN.REWARD_NAME_ID from {1}.REWARD_NAME RN
        where upper(RN.REWARD_NAME) like '%¬≈“≈–¿Õ%“–”ƒ¿%') and 
(tr.date_transfer = (select max(date_transfer) from {1}.transfer tr2 where tr2.per_num = tr.per_num)) and tr.type_transfer_id!=3) as rtr_w
from dual