select
to_char(R_Q.RQ_DATE_START,'DD.MM.YYYY') as RQ_DATE_START,
to_char(R_Q.RQ_DATE_end,'DD.MM.YYYY') as RQ_DATE_end,
T_R_Q.TYPE_RISE_QUAL_NAME,
ins.instit_name,
R_Q.RQ_NAME_DOC,
R_Q.RQ_SERIES||' '||R_Q.RQ_NUM_DOC as ser,
to_char(R_Q.RQ_DATE_doc,'DD.MM.YYYY') as RQ_DATE_doc,
B_D.BASE_DOC_NAME
from {1}.rise_qual r_q, {1}.base_doc b_d, {1}.instit ins, {1}.type_rise_qual t_r_q where r_q.base_doc_id=b_d.base_doc_id(+) and
T_R_Q.TYPE_RISE_QUAL_ID(+)=R_Q.TYPE_RISE_QUAL_ID and r_q.instit_id=ins.instit_id(+) and r_q.per_num = to_number('{0}')