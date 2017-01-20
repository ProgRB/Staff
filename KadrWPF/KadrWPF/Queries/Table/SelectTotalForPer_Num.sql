select V2.PAY_TYPE_ID as PAY_TYPE_ID, T0.PER_NUM as PER_NUM, v2.hcas as HCAS
from {0}.TRANSFER T0
join 
(
    /* Собираем 102 */
    select WD.PER_NUM, v1.pay_type_id, 
        case when round(sum(v1.VTIME)/3600, 1) <= 
                    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :year1 and 
                            extract(month from CA.CALENDAR_DAY) = :month1 and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :year1 and 
                            extract(month from CALENDAR_DAY) = :month1 and TYPE_DAY_ID = 3) * 7)
               then 
                    round(sum(v1.VTIME)/3600, 1)
               else
                    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :year1 and 
                            extract(month from CA.CALENDAR_DAY) = :month1 and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :year1 and 
                            extract(month from CALENDAR_DAY) = :month1 and TYPE_DAY_ID = 3) * 7)
        end as hcas   
    from (      
        select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 102 as PAY_TYPE_ID, 
        nvl(sum((select WP.VALID_TIME from {0}.work_pay_type wp2 
                    where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                        and WP2.PAY_TYPE_ID in (102, 302) 
                    group by WP2.WORKED_DAY_ID )), 0) as VTIME      
         from {0}.WORK_PAY_TYPE WP      
         where (WP.PAY_TYPE_ID in (102, 302) or not exists(
                select null from {0}.work_pay_type WP1 
                where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (102, 302)))
                 and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            ((TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) )
                            /* выбираем уволенных за нужный период */
                            or (not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end))
                            )                        
                        union
                        /* Новое условие!!!!!!!!!!!!!!!!!!!!!!11111*/
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            ((TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)))
                            /* выбираем уволенных за нужный период */
                            or (not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end))
                            )                        
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                            and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :month1 and extract(year from TR.DATE_TRANSFER) = :year1)
                       )   
         
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
    ) v1
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:date_begin) and trunc(:date_end)
        and ((trunc(:date_hire) is null and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) < trunc(:date_transfer) and trunc(:date_hire) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(WD.WORK_DATE) < trunc(:date_transfer))
        )
    /* New */
  /*      and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = v1.TRANSFER_ID and TR1.SUBDIV_ID = :per_num)*/
    group by WD.PER_NUM, v1.pay_type_id
    union
    select WD.PER_NUM, v1.pay_type_id, 
        round(sum(v1.VTIME)/3600, 1) - ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :year1 and 
                            extract(month from CA.CALENDAR_DAY) = :month1 and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :year1 and 
                            extract(month from CALENDAR_DAY) = :month1 and TYPE_DAY_ID = 3) * 7) 
        as hcas   
    from (      
        select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 125 as PAY_TYPE_ID, 
        nvl(sum((select WP.VALID_TIME from {0}.work_pay_type wp2 
                    where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                        and WP2.PAY_TYPE_ID in (102, 302) 
                    group by WP2.WORKED_DAY_ID )), 0) as VTIME      
         from {0}.WORK_PAY_TYPE WP      
         where (WP.PAY_TYPE_ID in (102, 302) or not exists(
                select null from {0}.work_pay_type WP1 
                where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (102, 302)))
                 and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                        union
                        /* Новое условие!!!!!!!!!!!!!!!!!!!!!!11111*/
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )                        
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                            and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :month1 and extract(year from TR.DATE_TRANSFER) = :year1))   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
    ) v1
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:date_begin) and trunc(:date_end)  
    and ((trunc(:date_hire) is null and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) < trunc(:date_transfer) and trunc(:date_hire) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(WD.WORK_DATE) < trunc(:date_transfer))
        )
    group by WD.PER_NUM, v1.pay_type_id
    having round(sum(v1.VTIME)/3600, 1) > ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :year1 and 
                            extract(month from CA.CALENDAR_DAY) = :month1 and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :year1 and 
                            extract(month from CALENDAR_DAY) = :month1 and TYPE_DAY_ID = 3) * 7) 
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where (
        T0.SIGN_CUR_WORK = 1 and (T0.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where T0.PER_NUM = TR1.PER_NUM and 
        TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and T0.SIGN_COMB = 1)) 
        /* выбираем уволенных за нужный период */
        or not exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
        and T0.TYPE_TRANSFER_ID = 3 and T0.DATE_TRANSFER = (
            select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where T0.PER_NUM = TR3.PER_NUM)
            and trunc(T0.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
        ) 
union
select V2.PAY_TYPE_ID as PAY_TYPE_ID, T0.PER_NUM as PER_NUM, v2.hcas as HCAS
from {0}.TRANSFER T0
join 
(
    /* Собираем 114 */
    select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas   
    from (    
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (114) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                        union
                        /* Новое условие!!!!!!!!!!!!!!!!!!!!!!11111*/
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )                        
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                            and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :month1 and extract(year from TR.DATE_TRANSFER) = :year1))      
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:date_begin) and trunc(:date_end)  
    and ((trunc(:date_hire) is null and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) < trunc(:date_transfer) and trunc(:date_hire) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(WD.WORK_DATE) < trunc(:date_transfer))
        )
    group by WD.PER_NUM, v1.pay_type_id
    union
    /* Собираем 106 вид оплат*/
    select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas   
    from (    
         /* Выбираем первые 2 часа 106, которые оплачиваются в 1,5-м размере */  
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 106 as PAY_TYPE_ID, 
            sum(WP.VALID_TIME) as VTIME
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (106) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                        union
                        /* Новое условие!!!!!!!!!!!!!!!!!!!!!!11111*/
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )                        
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.PER_NUM = :per_num and 
                            (TR.SIGN_CUR_WORK = 1 and (TR.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM and 
                            TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and TR.SIGN_COMB = 1)) 
                            /* выбираем уволенных за нужный период */
                            or not exists(select null from {0}.TRANSFER TR2 where TR.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
                            and TR.TYPE_TRANSFER_ID = 3 and TR.DATE_TRANSFER = (
                                select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where TR.PER_NUM = TR3.PER_NUM)
                                and trunc(TR.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
                            )    
                            and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :month1 and extract(year from TR.DATE_TRANSFER) = :year1))   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:date_begin) and trunc(:date_end)  
    and ((trunc(:date_hire) is null and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(:date_transfer) is null)
            or (trunc(WD.WORK_DATE) < trunc(:date_transfer) and trunc(:date_hire) is null)
            or (trunc(WD.WORK_DATE) >= trunc(:date_hire) and trunc(WD.WORK_DATE) < trunc(:date_transfer))
        )
    group by WD.PER_NUM, v1.pay_type_id
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where (
        T0.SIGN_CUR_WORK = 1 and (T0.SIGN_COMB = 0 or (not exists(select null from {0}.TRANSFER TR1 where T0.PER_NUM = TR1.PER_NUM and 
        TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0) and T0.SIGN_COMB = 1)) 
        /* выбираем уволенных за нужный период */
        or not exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1) 
        and T0.TYPE_TRANSFER_ID = 3 and T0.DATE_TRANSFER = (
            select max(TR3.DATE_TRANSFER) from {0}.TRANSFER TR3 where T0.PER_NUM = TR3.PER_NUM)
            and trunc(T0.DATE_TRANSFER) between trunc(:date_begin) and trunc(:date_end)
        ) 


