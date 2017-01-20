select rownum as num,
last_name, 
birth_date, 
subd,
posit,
classific,
/*lpad(to_char(extract(day from dat_dsms)),2,'0')||'.'||lpad(to_char(extract(month from dat_dsms)),2,'0') as dat_dsms,*/
rsn_dsms,
stag
from
(select distinct em.per_num,
em.emp_last_name||' '||substr(em.emp_first_name,1,1)||'.'||substr(em.emp_middle_name,1,1)||'.' as last_name, 
extract(year from EM.EMP_BIRTH_DATE) as birth_date, 
sub.code_subdiv as subd,
pos.pos_name as posit,
(select classific from {2}.account_data ac where ac.transfer_id = tr.from_position and ac.change_date = 
    (select max(change_date) from {2}.account_data where transfer_id = tr.from_position)) as classific,
tr.date_transfer as dat_dsms,
rd.reason_name as rsn_dsms,
({2}.AllStag(em.per_num)) as stag
from {2}.emp em,{2}.subdiv sub, {2}.position pos, {2}.transfer tr,{2}.reason_dismiss rd, {2}.habit h where tr.reason_id = rd.reason_id and h.per_num = em.per_num and
tr.per_num = em.per_num and sub.subdiv_id = tr.subdiv_id and tr.pos_id = pos.pos_id and TR.TYPE_TRANSFER_ID = 3 and
 trunc(tr.df_book_dismiss) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') order by date_transfer)