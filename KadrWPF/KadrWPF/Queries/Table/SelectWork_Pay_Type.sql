select W.WORK_PAY_TYPE_ID, W.WORKED_DAY_ID, W.PAY_TYPE_ID, W.REG_DOC_ID, nvl(W.VALID_TIME,0) as VALID_TIME, 
    /*case when W.PAY_TYPE_ID = 530 then 1 else D.DOC_TYPE end as*/ D.DOC_TYPE, nvl(D.ISCALC,-1) as ISCALC, D.DOC_NOTE 
from {0}.work_pay_type w 
left join {0}.reg_doc r on (W.REG_DOC_ID = R.REG_DOC_ID) 
left join {0}.doc_list d on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
left join {0}.pay_type p on (w.pay_type_id = p.pay_type_id) 
where worked_day_id = :worked_day_id and p.sign_addition = 0
    and (R.REG_DOC_ID is null or (R.REG_DOC_ID is not null and D.PAY_TYPE_ID = W.PAY_TYPE_ID)) 