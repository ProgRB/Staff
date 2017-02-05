select V1.NPUT as "№ П/Л", V1.DAYCOM as "Дни ком.", V1.BEGIN_WORK as "Дата начала", V1.END_WORK as "Дата окончания", 
    V1.BEGIN_PL as "П/Л с", V1.END_PL as "П/Л по", 
    V1.RECESS as "Перерыв", V1.CHASREM as "Часы ремонта", V1.DAT_SDACH as "Дата сдачи", NOMAVT "Номер автомобиля"
from (  
select /*+ no_merge(V) */ V.*, C.CALENDAR_DAY 
from 
(
    select * from {0}.V_PUTL_NEW VPL where VPL.PER_NUM = :p_per_num 
) V
join {0}.CALENDAR C on C.CALENDAR_DAY between trunc(V.BEGIN_WORK) and trunc(V.END_WORK)
)  V1
where V1.CALENDAR_DAY = :p_date_work