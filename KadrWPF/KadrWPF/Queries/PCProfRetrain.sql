select
to_char(p_f.pf_DATE_START,'dd.mm.yyyy') as pf_DATE_START,
to_char(p_f.pf_DATE_end,'dd.mm.yyyy') as pf_DATE_end,
'' as spec,
p_f.pf_NAME_DOC,
p_f.pf_NUM_DOC,
to_char(p_f.pf_DATE_doc,'dd.mm.yyyy') as pf_DATE_doc,
B_D.BASE_DOC_NAME
from {1}.prof_train p_f, {1}.base_doc b_d where p_f.base_doc_id=b_d.base_doc_id(+) and p_f.per_num=to_number('{0}')