select
t_v.name_vac,
to_char(vac.period_work_from,'DD.MM.YYYY') as period_work_from,
to_char(vac.period_work_to,'DD.MM.YYYY') as period_work_to,
vac.count_days,
to_char(VAC.VAC_DATE_START,'dd.mm.yyyy') as VAC_DATE_START,
to_char(VAC.VAC_DATE_END,'DD.MM.YYYY') as VAC_DATE_END,
b_d.base_doc_name
from {1}.vac, {1}.type_vac t_v, {1}.base_doc b_d where vac.base_doc_id=b_d.base_doc_id(+) and t_v.type_vac_id(+)=vac.type_vac_id and vac.per_num=to_number('{0}')