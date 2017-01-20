select rownum as num,
last_name,
first_name,
middle_name,
num_ord,
dat_ord,
rownum as page
from 
(select 
	rownum as numb,
	last_name,  
        first_name,  
        middle_name,  
        tr_num, chan_sign,         
        num_ord,  
        dat_ord from (
select 
em.emp_last_name as last_name,
em.emp_first_name as first_name,
em.emp_middle_name as middle_name,
regexp_substr(tr.tr_num_order,'\d*') as tr_num,
(tr.tr_num_order||decode(chan_sign,0,'/ó','/ê')) as num_ord, chan_sign,
tr.tr_date_order as dat_ord
from {2}.transfer tr,{2}.emp em
where TR.TYPE_TRANSFER_ID = 3 and substr(tr.per_num,1,1)!='7' and em.per_num = tr.per_num and 
    trunc(tr_date_order) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') /*and chan_sign != 1*/
 order by to_number(regexp_substr(tr_num_order,'\d*')),tr_date_order)) 
where (tr_num between ({4} * ({3}-1)+1 ) and ({4} * {3}) and chan_sign = 0) or 
(numb between ({4} * ({3}-1)+1 ) and ({4} * {3}) and chan_sign = 1) 