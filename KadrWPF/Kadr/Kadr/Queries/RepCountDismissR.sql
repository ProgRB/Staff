select rownum,
reason_name,
for_period from 
(select distinct
rd.reason_name,
(select count(*) from {2}.transfer tr1, {2}.reason_dismiss rd1 where 
tr1.reason_id = rd1.reason_id and tr1.type_transfer_id = 3 and rd1.reason_ID = rd.reason_id and trunc(date_transfer)  between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy')) as for_period
from {2}.reason_dismiss rd, {2}.transfer tr
where tr.type_transfer_id = 3 /*GROUP BY reason_name */order by reason_name) where for_period!=0