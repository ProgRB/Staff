select EG.EMP_GROUP_MASTER_ID, EG.NAME_GROUP_MASTER as "Наименование группы мастера" , 
    EG.BEGIN_GROUP as "Дата начала работы", EG.END_GROUP as "Дата окончания работы", 
	TRUNC(T.DATE_TRANSFER) as "Дата перевода"
from {0}.EMP_GROUP_MASTER EG
join {0}.TRANSFER T on T.TRANSFER_ID = EG.TRANSFER_ID
where EG.TRANSFER_ID in 
    (select TR4.TRANSFER_ID from {0}.TRANSFER TR4 connect by prior TR4.FROM_POSITION = TR4.TRANSFER_ID start with TR4.TRANSFER_ID = :p_transfer_id 
    union
    select TR3.TRANSFER_ID from {0}.TRANSFER TR3 connect by prior TR3.TRANSFER_ID = TR3.FROM_POSITION start with TR3.TRANSFER_ID = :p_transfer_id )
    and T.SUBDIV_ID = (select T1.SUBDIV_ID from {0}.TRANSFER T1 where T1.TRANSFER_ID = :p_transfer_id)
order by BEGIN_GROUP