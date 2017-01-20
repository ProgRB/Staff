select vac_add_period_id,calc_begin,calc_end, transfer_id,vac_group_type_id
from {0}.vac_add_period 
where transfer_id in 
(select transfer_id from {0}.transfer start with transfer_id = :p_transfer_id connect by nocycle prior  transfer_id=from_position or prior from_position =transfer_id) and
vac_group_type_id =:p_vac_group_type_id