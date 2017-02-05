select t.per_num,
	emp_last_name,emp_first_name,emp_middle_name,
	to_char((select min(date_transfer) from apstaff.transfer t1 start with transfer_id = t.transfer_id connect by t1.transfer_id= prior t1.from_position),'DD.MM.YYYY') date_hire,
	ad.salary,ad.comb_addition,ad.harmful_addition,ad.secret_addition, 
    to_char(ad.date_add_agree,'DD.MM.YYYY') date_add_agree
from apstaff.transfer t 
	 join apstaff.emp e on (e.per_num=t.per_num)
	 left join (select ad1.*, row_number() over (PARTITION BY transfer_id order by CHANGE_DATE desc) rn from apstaff.account_data ad1) ad on (ad.transfer_id=t.transfer_id)
where t.transfer_id=:p_transfer_id and rn=1
order by change_date desc