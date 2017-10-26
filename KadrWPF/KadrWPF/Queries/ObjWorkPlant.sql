select 
to_char(tr.date_transfer,'MM.YYYY')||' - '||to_char(tr2.date_transfer,'MM.YYYY') ||' - '|| pos.pos_name||' - '||sub.code_subdiv as years
from {1}.transfer tr, {1}.transfer tr2, {1}.position pos, {1}.subdiv sub 
where tr.pos_id = pos.pos_id and tr.subdiv_id = sub.subdiv_id and tr.transfer_id = tr2.from_position(+) 
 and (tr2.date_transfer(+) >= tr.date_transfer) and 
 tr.type_transfer_id in (1,2) and tr.per_num = to_number('{0}') and 
 tr.date_hire = (select max (date_hire) from {1}.transfer tr1 where tr1.sign_comb!=1 /*and tr1.sign_cur_work=1*/ and tr1.per_num=to_number('{0}')) 
 order by tr.date_transfer