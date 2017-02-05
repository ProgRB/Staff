insert into {0}.PN_TMP(PNUM, USER_NAME, TRANSFER_ID)
select V.PER_NUM,:p_user_name,V.TRANSFER_ID 
from 
    (SELECT        
      subdiv_id,
      transfer_id, PER_NUM, POS_ID, DEGREE_ID, DATE_TRANSFER, TYPE_TRANSFER_ID
    FROM {0}.transfer t    
    WHERE sign_cur_work = 1 OR type_transfer_id = 3) V
WHERE ((V.TYPE_TRANSFER_ID = 3 and V.DATE_TRANSFER between :p_date_begin and :p_date_end) 
    or V.TYPE_TRANSFER_ID != 3) and
    ((:p_subdiv_id != 0 and V.SUBDIV_ID = :p_subdiv_id) or 
    (:p_subdiv_id = 0 and V.SUBDIV_ID in (select SUBDIV_ID from {0}.SUBDIV_FOR_TABLE)))