select sign_comb,per_num,degree_name,gr_work_name,vacant_sign,date_begin_staff,date_end_staff
from {0}.transfer join {0}.staffs on (transfer.staffs_id=staffs.staffs_id) left join {0}.degree on (transfer.degree_id=degree.degree_id)
		left join {0}.gr_work on (transfer.gr_work_id=gr_work.gr_work_id)
where transfer.sign_cur_work=1 and transfer.staffs_id={1}