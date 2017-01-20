select TR.PER_NUM, DOC.DOC_NAME ,to_char(RE.DOC_BEGIN, 'dd.mm.yyyy') as DOC_BEGIN,to_char(RE.DOC_END, 'dd.mm.yyyy') as DOC_END, 
    DOC.DOC_NOTE
from {0}.transfer tr 
join {0}.reg_doc re on (TR.PER_NUM = RE.PER_NUM) 
join {0}.doc_list doc on (RE.DOC_LIST_ID = DOC.DOC_LIST_ID) 
where DOC.DOC_NOTE = 'сд'  and TR.SIGN_CUR_WORK=1 and  TR.PER_NUM = :p_per_num
    and :p_date between RE.DOC_BEGIN and RE.DOC_END