select to_char(tr.date_transfer,'DD.MM.YYYY') as date_transfer,
sub.subdiv_name,
pos.pos_name||', '||dg.degree_name as pos_name,
a_d.salary,
(case (tr.type_transfer_id)
when 1 then 'Приём'
when 2 then 'Перевод'
when 3 then 'Увольнение'
else ''
end)||' '||b_d.base_doc_name||' '||tr.tr_num_order ||' '||to_char(tr.tr_date_order,'DD.MM.YYYY') as emp_in,
(case (tr1.type_transfer_id)
when 1 then 'Приём'
when 2 then 'Перевод'
when 3 then 'Увольнение'
else ''
end)||' '||b_d1.base_doc_name||' '||tr1.tr_num_order ||' '||to_char(tr1.tr_date_order,'DD.MM.YYYY') as emp_out
from {1}.degree dg, {1}.base_doc b_d, {1}.base_doc b_d1, {1}.transfer tr, {1}.transfer tr1, {1}.account_data a_d, {1}.tariff_grid t_g, {1}.position pos, {1}.subdiv sub where tr.per_num = to_number('{0}') and
 pos.pos_id(+)=tr.pos_id and tr.subdiv_id(+)=sub.subdiv_id and b_d1.base_doc_id(+)=tr1.base_doc_id and tr.transfer_id=tr1.from_position(+) and dg.degree_id = tr.degree_id
and T_G.TARIFF_GRID_ID(+) = A_D.TARIFF_GRID_ID and TR.TRANSFER_ID = a_d.transfer_id(+) and b_d.base_doc_id(+)=tr.base_doc_id order by tr.date_transfer