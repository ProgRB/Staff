select
to_char(att.date_attest,'DD.MM.YYYY') as date_attest,
att.solution,
att.num_protocol,
att.date_protocol,
b_d.base_doc_name
from {1}.attest att, {1}.base_doc b_d where att.base_doc_id=b_d.base_doc_id and att.per_num=to_number('{0}')