select
/* Собираем 102 */
(select 
    case when round(sum(v1.VTIME)/3600, 1) <= 
                ((select count(*) from apstaff.calendar CA where extract(year from CA.CALENDAR_DAY) = {5} and 
                        extract(month from CA.CALENDAR_DAY) = {4} and CA.TYPE_DAY_ID = 2) * 8 
                        + 
                        (select count(*) from apstaff.calendar where extract(year from CALENDAR_DAY) = {5} and 
                        extract(month from CALENDAR_DAY) = {4} and TYPE_DAY_ID = 3) * 7)
           then 
                round(sum(v1.VTIME)/3600, 1)
           else
                ((select count(*) from apstaff.calendar CA where extract(year from CA.CALENDAR_DAY) = {5} and 
                        extract(month from CA.CALENDAR_DAY) = {4} and CA.TYPE_DAY_ID = 2) * 8 
                        + 
                        (select count(*) from apstaff.calendar where extract(year from CALENDAR_DAY) = {5} and 
                        extract(month from CALENDAR_DAY) = {4} and TYPE_DAY_ID = 3) * 7)
    end as hcas
from (      
    select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 102 as PAY_TYPE_ID, 
    nvl(sum((select WP.VALID_TIME from apstaff.work_pay_type wp2 
                where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                    and WP2.PAY_TYPE_ID in (101, 102, 302) 
                group by WP2.WORKED_DAY_ID )), 0) as VTIME      
     from apstaff.WORK_PAY_TYPE WP      
     where (WP.PAY_TYPE_ID in (101, 102, 302) or not exists(
            select null from apstaff.work_pay_type WP1 
            where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (101, 102, 302)))
             and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})     
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
) v1
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id) 
as h102,
/* Собираем 106 вид оплат*/
(select round(sum(v1.VTIME)/3600, 1) as hcas   
from (    
     /* Выбираем первые 2 часа 106, которые оплачиваются в 1,5-м размере */  
     select WP.WORKED_DAY_ID,
         case when sum(WP.VALID_TIME) >= 7200 then 7200 else sum(WP.VALID_TIME) end as VTIME   
     from apstaff.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (106) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
) as h106_1,
(/* Собираем 106 вид оплат, который больше 2 часов*/
select round(sum(v1.VTIME)/3600, 1) as hcas   
from (    
     /* Выбираем первые 2 часа 106, которые оплачиваются в 1,5-м размере */  
     select WP.WORKED_DAY_ID, sum(WP.VALID_TIME) - 7200 as VTIME      
     from apstaff.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (106) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
     having sum(WP.VALID_TIME) > 7200    
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  ) 
as h106_2,
(/* Собираем 111 */
select round(sum(v1.VTIME)/3600, 1) as hcas  
from (    
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
     from apstaff.WORK_PAY_TYPE WP           
     where WP.PAY_TYPE_ID = 111 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        ) 
         and not exists(select null from apstaff.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id 
) as h111,
(/* Собираем 112/114 */
select round(sum(v1.VTIME)/3600, 1) as hcas  
from (    
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 114 as PAY_TYPE_ID, WP.VALID_TIME as VTIME      
     from apstaff.WORK_PAY_TYPE WP           
     where WP.PAY_TYPE_ID in (112, 114) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        ) 
         and not exists(select null from apstaff.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id 
) as h112_114,
(/* Собираем 121 */
select round(sum(v1.VTIME)/3600, 1) as hcas   
from (      
    select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 121 as PAY_TYPE_ID, 
    nvl(sum((select WP.VALID_TIME from apstaff.work_pay_type wp2 
                where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                    and WP2.PAY_TYPE_ID in (102, 302) 
                group by WP2.WORKED_DAY_ID )), 0) as VTIME      
     from apstaff.WORK_PAY_TYPE WP               
     where (WP.PAY_TYPE_ID in (102, 302) or not exists(
            select null from apstaff.work_pay_type WP1 
            where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (102, 302)))
             and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
) v1
left outer join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
left outer join apstaff.CALENDAR CR on (trunc(WD.WORK_DATE) = trunc(CR.CALENDAR_DAY))
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
    and CR.TYPE_DAY_ID = 4 and V1.VTIME > 0
group by WD.PER_NUM, v1.pay_type_id
) as h121,
(/* Собираем 124 */
select round(sum(v1.VTIME)/3600, 1) as hcas  
from (    
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
     from apstaff.WORK_PAY_TYPE WP           
     where WP.PAY_TYPE_ID in (124) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        ) 
         and not exists(select null from apstaff.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id 
) as h124,
(/* Собираем 125 */
select 
    round(sum(v1.VTIME)/3600, 1) - ((select count(*) from apstaff.calendar CA where extract(year from CA.CALENDAR_DAY) = {5} and 
                        extract(month from CA.CALENDAR_DAY) = {4} and CA.TYPE_DAY_ID = 2) * 8 
                        + 
                        (select count(*) from apstaff.calendar where extract(year from CALENDAR_DAY) = {5} and 
                        extract(month from CALENDAR_DAY) = {4} and TYPE_DAY_ID = 3) * 7) 
    as hcas 
from (      
    select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 125 as PAY_TYPE_ID, 
    nvl(sum((select WP.VALID_TIME from apstaff.work_pay_type wp2 
                where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                    and WP2.PAY_TYPE_ID in (102, 302) 
                group by WP2.WORKED_DAY_ID )), 0) as VTIME      
     from apstaff.WORK_PAY_TYPE WP      
     where (WP.PAY_TYPE_ID in (102, 302) or not exists(
            select null from apstaff.work_pay_type WP1 
            where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (102, 302)))
             and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
) v1
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id
having round(sum(v1.VTIME)/3600, 1) > ((select count(*) from apstaff.calendar CA where extract(year from CA.CALENDAR_DAY) = {5} and 
                        extract(month from CA.CALENDAR_DAY) = {4} and CA.TYPE_DAY_ID = 2) * 8 
                        + 
                        (select count(*) from apstaff.calendar where extract(year from CALENDAR_DAY) = {5} and 
                        extract(month from CALENDAR_DAY) = {4} and TYPE_DAY_ID = 3) * 7) 
) as h125,
(/* Собираем 210 */
select count(*) as hcas
from (      
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 210 as pay_type_id      
     from apstaff.worked_day WD
     left join apstaff.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID = 210 and WD.FROM_GRAPH = WP.VALID_TIME and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h210,
(/* Собираем 211 */
select round(sum(v1.VTIME)/3600, 1) as hcas  
from (    
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
     from apstaff.WORK_PAY_TYPE WP           
     where WP.PAY_TYPE_ID = 211 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        ) 
         and not exists(select null from apstaff.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id 
) as h211,
(/* Собираем 222, 622 */
select count(*) as hcas
from (      
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 222 as pay_type_id      
     from apstaff.worked_day WD
     left join apstaff.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID in (222, 622) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h222_622,
(/* Собираем 226 */
select count(*) as hcas
from (      
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 226 as pay_type_id      
     from apstaff.worked_day WD
     left join apstaff.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID = 226 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h226,
(/* Собираем 237 */
select count(*) as hcas
from (      
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 237 as pay_type_id      
     from apstaff.worked_day WD
     left join apstaff.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID = 237 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h237,
(/* Собираем 540 */
select count(*) as hcas
from (      
    /* Выбираем Кол-во отработанных дней */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 540 as pay_type_id      
     from apstaff.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (102, 302, 533) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID    
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h540,
(/* Собираем 541 */
select count(*) as hcas
from (      
     /* Выбираем Отвлечения в календарных днях, которые даются на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 541 as pay_type_id      
     from apstaff.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (222, 215, 501, 237, 111, 226, 536, 532, 243) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
     union
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 541 as pay_type_id      
     from apstaff.worked_day WD
     left join apstaff.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID in (531, 210, 529) and WD.FROM_GRAPH = WP.VALID_TIME and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join apstaff.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h541,
(/* Собираем 531 */
select round(sum(v1.VTIME)/3600, 1) as hcas  
from (    
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
     from apstaff.WORK_PAY_TYPE WP           
     where WP.PAY_TYPE_ID = 531 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from apstaff.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from apstaff.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        ) 
         and not exists(select null from {0}.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
     ) V1  
join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, v1.pay_type_id 
) as h531,
(/* Собираем 533 */
select count(*) as hcas
from (      
     /* Выбираем в Отвлечения в календарных днях - административный на целый день */
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 533 as pay_type_id      
     from {0}.worked_day WD
     left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
     where WP.PAY_TYPE_ID = 533 and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and 
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})
                    union
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5}     
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.PER_NUM = {1} and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = {4} and extract(year from TR.DATE_TRANSFER) = {5})    
                        )          
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
     ) V1  
join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(to_date('{2}','dd.mm.yyyy')) and trunc(to_date('{3}','dd.mm.yyyy'))  
group by WD.PER_NUM, pay_type_id
) as h533
from dual