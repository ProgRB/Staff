select R.DOC_BEGIN as "������ ���������", R.DOC_END as "��������� ���������", D.DOC_NOTE as "��������", D.DOC_NAME as "������������ ���������"
from {0}.REG_DOC R
join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
where D.PAY_TYPE_ID in (121, 542) and R.TRANSFER_ID in (
        select TR1.TRANSFER_ID from {0}.TRANSFER TR1 connect by prior TR1.FROM_POSITION = TR1.TRANSFER_ID start with TR1.TRANSFER_ID = :p_transfer_id 
    )
order by R.DOC_BEGIN 