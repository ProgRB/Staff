select rownum as num,
last_name, 
first_name, 
middle_name,
adress,
subd,
posit,
combo,
dat_dsms
from (
select distinct
em.emp_last_name as last_name, 
em.emp_first_name as first_name, 
em.emp_middle_name as middle_name,
street.name_street ||' '|| h.hab_house ||' '|| H.HAB_BULK ||' '|| H.HAB_FLAT as adress,
sub.code_subdiv as subd,
pos.pos_name as posit,
( case (tr.sign_comb)
when 1 then 'X'
else ''
end) as combo,
tr.date_transfer as dat_dsms
from {2}.emp em,{2}.subdiv sub, {2}.position pos, {2}.transfer tr,  {2}.street, {2}.habit h where h.hab_code_street = street.code_street and  
h.per_num = em.per_num and tr.per_num = em.per_num and sub.subdiv_id = tr.subdiv_id and tr.pos_id = pos.pos_id 
and TR.TYPE_TRANSFER_ID = 3 and trunc(tr.df_book_dismiss) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') order by date_transfer)