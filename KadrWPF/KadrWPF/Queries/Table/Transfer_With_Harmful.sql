select T1.DATE_TRANSFER, CASE WHEN T1.END_TRANSFER != date '3000-01-01' -1/86400 THEN TRUNC(T1.END_TRANSFER) END END_TRANSFER, AD.HARMFUL_ADDITION_ADD/*, 
    NVL((select CH.PERCENTAGE_OF_EMPLOYMENT from {0}.CHANGE_HARMFUL CH 
        WHERE CH.TRANSFER_ID = T1.TRANSFER_ID and VALIDITY = :p_date_end),100) PERCENTAGE_OF_EMPLOYMENT*/
from 
(
    WITH transfer_tree as
       (
        SELECT transfer_id,trunc(date_transfer) date_transfer,
            LEAD(trunc(date_transfer),1, DECODE(type_transfer_id,3,TRUNC(date_transfer)+1,date'3000-01-01')) OVER (ORDER BY date_transfer)-1/86400 end_transfer ,
            type_transfer_id,degree_id,sign_comb,pos_id,from_position,FORM_OPERATION_ID,subdiv_id,per_num, FORM_PAY
        FROM {0}.TRANSFER START WITH transfer_id=:p_transfer_id CONNECT BY NOCYCLE PRIOR transfer_id = from_position or transfer_id = PRIOR from_position
      )
    select * from transfer_tree T
    /*where T.DATE_TRANSFER <= TRUNC(:p_date_end,'MONTH') and T.END_TRANSFER >= :p_date_end*/
	where T.DATE_TRANSFER <= :p_date_end and T.END_TRANSFER >= TRUNC(:p_date_end,'MONTH')
) T1
join {0}.ACCOUNT_DATA AD on (AD.TRANSFER_ID = DECODE(T1.TYPE_TRANSFER_ID, 3, T1.FROM_POSITION, T1.TRANSFER_ID))
/*where AD.HARMFUL_ADDITION_ADD > 0*/