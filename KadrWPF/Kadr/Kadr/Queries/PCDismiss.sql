select
b_d.base_doc_name,
to_char(tr.date_transfer,'DD') as date_transferD,
to_char(tr.date_transfer,'MM') as date_transferM,
to_char(tr.date_transfer,'YYYY') as date_transferY,
tr.tr_num_order,
to_char(tr.tr_date_order,'DD') as tr_date_orderD,
to_char(tr.tr_date_order,'MM') as tr_date_orderM,
to_char(tr.tr_date_order,'YYYY') as tr_date_orderY
from {1}.transfer tr, {1}.base_doc b_d where tr.type_transfer_id = 3 and tr.per_num =to_number('{0}') and B_D.BASE_DOC_ID(+) = TR.BASE_DOC_ID
and sign_comb != 1 and (select max(date_transfer) from {1}.transfer tr2 where per_num =to_number('{0}') )=tr.date_transfer