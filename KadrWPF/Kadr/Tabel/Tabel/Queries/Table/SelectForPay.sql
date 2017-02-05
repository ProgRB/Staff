select PTN, SC, NP, ZN, P_RAB, VOP, PR, ZAK, TN, HCAS, SUM, GM, YN, KT  
from (
select '9' as PTN, :code_subdiv as SC, '002' as NP, '1' as ZN, case when T0.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, 
 V2.PAY_TYPE_ID as VOP, '00' as PR,       
 (case when T0.STAFFS_ID is not null 
    then (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where T0.STAFFS_ID = ST.STAFFS_ID)
    else
    (select (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where RE.STAFFS_ID = ST.STAFFS_ID)
     from {0}.REPL_EMP RE where T0.TRANSFER_ID = RE.TRANSFER_ID and RE.REPL_ACTUAL_SIGN = 1) 
    end) as ZAK,      
 T0.PER_NUM as TN, lpad(to_char(v2.hcas*10),6,'0') as HCAS, 
 case when extract(month from T0.DATE_TRANSFER) = :MonthCalc and extract(year from T0.DATE_TRANSFER) = :YearCalc and
    extract(day from T0.DATE_TRANSFER) > 1 and T0.TYPE_TRANSFER_ID = 2
 then 
  lpad(
  to_char(
    round(
    (
    nvl(((select AD1.SALARY from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.FROM_POSITION and AD1.CHANGE_DATE = 
    (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
    *
    (select tariff from {0}.BASE_TARIFF where BDATE = (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
    extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
    + 
    (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
    extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
    *    
    (select round(sum(WP.VALID_TIME)/3600, 1)   
    from {0}.WORKED_DAY WD
    left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
    where trunc(work_date) between trunc(:beginDate) and trunc(T0.DATE_TRANSFER) - 1  
        and WP.PAY_TYPE_ID in (101, 102, 302) and WP.TRANSFER_ID in (T0.FROM_POSITION, T0.TRANSFER_ID)
        and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id)
    group by WD.PER_NUM)),0) 
    + 
    nvl(((select AD1.SALARY from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.TRANSFER_ID and AD1.CHANGE_DATE = 
    (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
    *
    (select tariff from {0}.BASE_TARIFF where BDATE = 
    (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
    extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
    + 
    (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
    extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
    *    
    (select round(sum(WP.VALID_TIME)/3600, 1)   
    from {0}.WORKED_DAY WD
    left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
    where trunc(work_date) between trunc(T0.DATE_TRANSFER) and trunc(:endDate)  
        and WP.PAY_TYPE_ID in (101, 102, 302) and WP.TRANSFER_ID in (T0.TRANSFER_ID, T0.FROM_POSITION)
        and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.TRANSFER_ID and TR1.SUBDIV_ID = :subdiv_id) 
    group by WD.PER_NUM) ),0)
    ),2) *100
    ),11,'0')
 else
  lpad('0',11, '0')
  end as SUM,
 lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT, N_ST
from {0}.TRANSFER T0
join 
(
    /* Собираем 102 */
    select WD.PER_NUM, v1.pay_type_id, 
        case when round(sum(v1.VTIME)/3600, 1) <= 
                    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
                            extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
                            extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
               then 
                    round(sum(v1.VTIME)/3600, 1)
               else
                    ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
                            extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
                            extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
        end as hcas, 1 as N_ST   
    from (      
        select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 102 as PAY_TYPE_ID, 
        nvl(sum((select WP.VALID_TIME from {0}.work_pay_type wp2 
                    where WP2.TRANSFER_ID = WP.TRANSFER_ID and WP2.WORKED_DAY_ID = WP.WORKED_DAY_ID 
                        and WP2.PAY_TYPE_ID in (101, 102, 302) 
                    group by WP2.WORKED_DAY_ID )), 0) as VTIME      
         from {0}.WORK_PAY_TYPE WP      
         where (WP.PAY_TYPE_ID in (101, 102, 302) or not exists(
                select null from {0}.work_pay_type WP1 
                where WP1.TRANSFER_ID = WP.TRANSFER_ID and  WP1.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP1.PAY_TYPE_ID in (101, 102, 302)))
                 and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)   
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             )     
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
    ) v1
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, v1.pay_type_id
    union
    select WD.PER_NUM, v1.pay_type_id, 
        round(sum(v1.VTIME)/3600, 1) - ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
                            extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
                            extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) 
        as hcas, 1 as N_ST   
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
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
    ) v1
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, v1.pay_type_id
    having round(sum(v1.VTIME)/3600, 1) > ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
                            extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
                            + 
                            (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
                            extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) 
    union
    select WD.PER_NUM, v1.pay_type_id, 
        round(sum(v1.VTIME)/3600, 1) as hcas, 1 as N_ST   
    from (      
        select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 121 as PAY_TYPE_ID, 
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
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             )     
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
    ) v1
    left outer join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    left outer join {0}.CALENDAR CR on (trunc(WD.WORK_DATE) = trunc(CR.CALENDAR_DAY))
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
        and CR.TYPE_DAY_ID = 4 and V1.VTIME > 0
    group by WD.PER_NUM, v1.pay_type_id
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where ((T0.sign_cur_work = 1 or (
    not exists(select null from {0}.TRANSFER T1 where T1.PER_NUM = V2.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0)
    and T0.TYPE_TRANSFER_ID = 3 and extract(year from T0.DATE_TRANSFER) = :YearCalc and extract(month from T0.DATE_TRANSFER) = :MonthCalc ))
    and T0.SUBDIV_ID = :subdiv_id
    or 
    (exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1 and TR2.SUBDIV_ID != :subdiv_id and
                trunc(T0.DATE_TRANSFER) between trunc(:beginDate) and trunc(:endDate))
            and exists(select null from {0}.TRANSFER TR3 where T0.FROM_POSITION = TR3.TRANSFER_ID and
                     TR3.SUBDIV_ID = :subdiv_id )    
    ))     
    and T0.SIGN_COMB = 0 
union
select '9' as PTN, :code_subdiv as SC, '002' as NP, '1' as ZN, case when T0.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, 
 V2.PAY_TYPE_ID as VOP, '00' as PR,       
 (case when T0.STAFFS_ID is not null 
    then (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where T0.STAFFS_ID = ST.STAFFS_ID)
    else
    (select (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where RE.STAFFS_ID = ST.STAFFS_ID)
     from {0}.REPL_EMP RE where T0.TRANSFER_ID = RE.TRANSFER_ID and RE.REPL_ACTUAL_SIGN = 1) 
    end) as ZAK,      
 T0.PER_NUM as TN, lpad(to_char(v2.hcas*10),6,'0') as HCAS, lpad('0',11, '0') as SUM,
 lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT, N_ST
from {0}.TRANSFER T0
join 
(
    /* Собираем другие виды оплат, кроме 106 */
    select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas, 1 as N_ST   
    from (    
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, WP.PAY_TYPE_ID, WP.VALID_TIME as VTIME      
         from {0}.WORK_PAY_TYPE WP           
         where WP.PAY_TYPE_ID in (111, 112, 114, 120, 124, 211, 531, 533) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            ) 
             and not exists(select null from {0}.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID)                                 
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, v1.pay_type_id
    union
    /* Собираем 106 вид оплат*/
    select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas, N_ST   
    from (    
         /* Выбираем первые 2 часа 106, которые оплачиваются в 1,5-м размере */  
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 106 as PAY_TYPE_ID, 
             case when sum(WP.VALID_TIME) >= 7200 then 7200 else sum(WP.VALID_TIME) end as VTIME, 1 as n_st      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (106) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         union
         /* Выбираем оставшиеся часы 106, которые оплачиваются в 2-м размере */  
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 106 as PAY_TYPE_ID, sum(WP.VALID_TIME) - 7200 as VTIME, 2 as n_st      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (106) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)   
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             )  
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         having sum(WP.VALID_TIME) > 7200    
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, v1.pay_type_id, n_st
    union
    select WD.PER_NUM, pay_type_id, count(*) as hcas, 1 as N_ST
    from (      
        /* Выбираем Кол-во отработанных дней */
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 540 as pay_type_id      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (102, 302, 533) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)  
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             )   
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         union 
         /* Выбираем Кол-во дней суббот */
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 524 as pay_type_id      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (124) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         union
         /* Выбираем Отвлечения в календарных днях, которые даются на целый день */
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 541 as pay_type_id      
         from {0}.WORK_PAY_TYPE WP      
         where WP.PAY_TYPE_ID in (222, 215, 501, 237, 111, 226, 536, 532, 243) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )   
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
         union
         /* Выбираем в Отвлечения в календарных днях - административный на целый день */
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 541 as pay_type_id      
         from {0}.worked_day WD
         left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)      
         where WP.PAY_TYPE_ID in (531, 210, 529) and WD.FROM_GRAPH = WP.VALID_TIME and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)    
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )          
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID         
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, pay_type_id
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where ((T0.sign_cur_work = 1 or (
    not exists(select null from {0}.TRANSFER T1 where T1.PER_NUM = V2.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0)
    and T0.TYPE_TRANSFER_ID = 3 and extract(year from T0.DATE_TRANSFER) = :YearCalc and extract(month from T0.DATE_TRANSFER) = :MonthCalc ))
    and T0.SUBDIV_ID = :subdiv_id
    or 
    (exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1 and TR2.SUBDIV_ID != :subdiv_id and
                trunc(T0.DATE_TRANSFER) between trunc(:beginDate) and trunc(:endDate))
            and exists(select null from {0}.TRANSFER TR3 where T0.FROM_POSITION = TR3.TRANSFER_ID and
                     TR3.SUBDIV_ID = :subdiv_id )    
    ))  
    and T0.SIGN_COMB = 0 
union
select '9' as PTN, :code_subdiv as SC, '002' as NP, '1' as ZN, case when T0.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, 
 V2.PAY_TYPE_ID as VOP,  '00' as PR,       
 (case when T0.STAFFS_ID is not null 
    then (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where T0.STAFFS_ID = ST.STAFFS_ID)
    else
    (select (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where RE.STAFFS_ID = ST.STAFFS_ID)
     from {0}.REPL_EMP RE where T0.TRANSFER_ID = RE.TRANSFER_ID and RE.REPL_ACTUAL_SIGN = 1) 
    end) as ZAK,      
 T0.PER_NUM as TN, lpad('0',6,'0') as HCAS, 
  case when extract(month from T0.DATE_TRANSFER) = :MonthCalc and extract(year from T0.DATE_TRANSFER) = :YearCalc and
    extract(day from T0.DATE_TRANSFER) > 1 and T0.TYPE_TRANSFER_ID = 2
    then 
        lpad(
      to_char(
        round(
        (
        nvl(((select AD1.COMB_ADDITION from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.FROM_POSITION and AD1.CHANGE_DATE = 
        (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
        *
        (select tariff from {0}.BASE_TARIFF where BDATE = (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
        extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
        + 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
        *    
        (select round(sum(WP.VALID_TIME)/3600, 1)   
        from {0}.WORKED_DAY WD
        left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
        where trunc(work_date) between trunc(:beginDate) and trunc(T0.DATE_TRANSFER) - 1  
            and WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (T0.FROM_POSITION, T0.TRANSFER_ID)
            and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id)
        group by WD.PER_NUM)) ,0)
        + 
        nvl(((select AD1.COMB_ADDITION from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.TRANSFER_ID and AD1.CHANGE_DATE = 
        (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
        *
        (select tariff from {0}.BASE_TARIFF where BDATE = 
        (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
        extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
        + 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
        *    
        (select round(sum(WP.VALID_TIME)/3600, 1)   
        from {0}.WORKED_DAY WD
        left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
        where trunc(work_date) between trunc(T0.DATE_TRANSFER) and trunc(:endDate)  
            and WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (T0.TRANSFER_ID, T0.FROM_POSITION)
            and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.TRANSFER_ID and TR1.SUBDIV_ID = :subdiv_id)
        group by WD.PER_NUM) ) ,0)
        ),2) *100
        ),11,'0')
    else
    case when ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) = hcas
        then   
        lpad(to_char(round(AD.COMB_ADDITION * round((select tariff from {0}.BASE_TARIFF where BDATE = 
       (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) * hcas, 2), 0)*100),11, '0')
        else    
        lpad(to_char(round(AD.COMB_ADDITION * round((select tariff from {0}.BASE_TARIFF where BDATE = 
       (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) * hcas, 2), 2)*100),11, '0')
        end
    end as SUM, 
lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT, N_ST
from {0}.TRANSFER T0
join 
(
/* Собираем 153 у кого есть */
select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas, 1 as N_ST   
from (      
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 153 as PAY_TYPE_ID, sum(WP.VALID_TIME) as VTIME      
     from {0}.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                    union
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)  
                    /*New */
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                        (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                         )   
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
     ) V1  
join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
group by WD.PER_NUM, v1.pay_type_id
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.ACCOUNT_DATA AD on (T0.TRANSFER_ID = AD.TRANSFER_ID)
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where ((T0.sign_cur_work = 1 or (
    not exists(select null from {0}.TRANSFER T1 where T1.PER_NUM = V2.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0)
    and T0.TYPE_TRANSFER_ID = 3 and extract(year from T0.DATE_TRANSFER) = :YearCalc and extract(month from T0.DATE_TRANSFER) = :MonthCalc ))
    and T0.SUBDIV_ID = :subdiv_id
    or 
    (exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1 and TR2.SUBDIV_ID != :subdiv_id and
                trunc(T0.DATE_TRANSFER) between trunc(:beginDate) and trunc(:endDate))
            and exists(select null from {0}.TRANSFER TR3 where T0.FROM_POSITION = TR3.TRANSFER_ID and
                     TR3.SUBDIV_ID = :subdiv_id )    
    ))  
    and T0.SIGN_COMB = 0 
    and AD.COMB_ADDITION > 0
    and AD.CHANGE_DATE = (select max(AD1.CHANGE_DATE) from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID =T0.TRANSFER_ID )
union
select '9' as PTN, :code_subdiv as SC, '002' as NP, '1' as ZN, case when T0.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, 
 V2.PAY_TYPE_ID as VOP, '00' as PR,       
 (case when T0.STAFFS_ID is not null 
    then (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where T0.STAFFS_ID = ST.STAFFS_ID)
    else
    (select (select 
        (select O.ORDER_NAME from {0}.ORDERS O where ST.ORDER_ID = O.ORDER_ID ) 
         from {0}.STAFFS ST where RE.STAFFS_ID = ST.STAFFS_ID)
     from {0}.REPL_EMP RE where T0.TRANSFER_ID = RE.TRANSFER_ID and RE.REPL_ACTUAL_SIGN = 1) 
    end) as ZAK,      
 T0.PER_NUM as TN, lpad('0',6,'0') as HCAS, 
 case when extract(month from T0.DATE_TRANSFER) = :MonthCalc and extract(year from T0.DATE_TRANSFER) = :YearCalc and
    extract(day from T0.DATE_TRANSFER) > 1 and T0.TYPE_TRANSFER_ID = 2
    then 
        lpad(
      to_char(
        round(
        (
        nvl(((select AD1.SECRET_ADDITION from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.FROM_POSITION and AD1.CHANGE_DATE = 
        (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
        *
        (select tariff from {0}.BASE_TARIFF where BDATE = (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
        extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
        + 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
        *    
        (select round(sum(WP.VALID_TIME)/3600, 1)   
        from {0}.WORKED_DAY WD
        left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
        where trunc(work_date) between trunc(:beginDate) and trunc(T0.DATE_TRANSFER) - 1  
            and WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (T0.FROM_POSITION, T0.TRANSFER_ID)
            and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id)            
        group by WD.PER_NUM)) ,0)
        + 
        nvl(((select AD1.SECRET_ADDITION from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = T0.TRANSFER_ID and AD1.CHANGE_DATE = 
        (select max(AD2.CHANGE_DATE) from {0}.ACCOUNT_DATA AD2 where AD2.TRANSFER_ID = AD1.TRANSFER_ID))
        *
        (select tariff from {0}.BASE_TARIFF where BDATE = 
        (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar CA where extract(year from CA.CALENDAR_DAY) = :YearCalc and 
        extract(month from CA.CALENDAR_DAY) = :MonthCalc and CA.TYPE_DAY_ID = 2) * 8 
        + 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7)
        *    
        (select round(sum(WP.VALID_TIME)/3600, 1)   
        from {0}.WORKED_DAY WD
        left join {0}.WORK_PAY_TYPE WP on (WD.WORKED_DAY_ID = WP.WORKED_DAY_ID)
        where trunc(work_date) between trunc(T0.DATE_TRANSFER) and trunc(:endDate)  
            and WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (T0.TRANSFER_ID, T0.FROM_POSITION)
            and exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = T0.TRANSFER_ID and TR1.SUBDIV_ID = :subdiv_id)
        group by WD.PER_NUM) ) ,0)
        ),2) *100
        ),11,'0')
    else
    case when ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) = hcas
        then   
        lpad(to_char(round(AD.SECRET_ADDITION * round((select tariff from {0}.BASE_TARIFF where BDATE = 
       (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) * hcas, 2), 0)*100),11, '0')
        else    
        lpad(to_char(round(AD.SECRET_ADDITION * round((select tariff from {0}.BASE_TARIFF where BDATE = 
       (select max(BT.BDATE) from {0}.BASE_TARIFF BT where BT.BDATE < sysdate)) /
        ((select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 2) * 8+ 
        (select count(*) from {0}.calendar where extract(year from CALENDAR_DAY) = :YearCalc and 
        extract(month from CALENDAR_DAY) = :MonthCalc and TYPE_DAY_ID = 3) * 7) * hcas, 2), 2)*100),11, '0')
        end
    end as SUM, 
lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT, N_ST
from {0}.TRANSFER T0
join 
(
/* Собираем 142 у кого есть */
select WD.PER_NUM, v1.pay_type_id, round(sum(v1.VTIME)/3600, 1) as hcas, 1 as N_ST   
from (      
     select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 142 as PAY_TYPE_ID, sum(WP.VALID_TIME) as VTIME      
     from {0}.WORK_PAY_TYPE WP      
     where WP.PAY_TYPE_ID in (102, 302) and WP.TRANSFER_ID in (
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                    union
                    /*New */
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                        extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                    union
                    select TR.TRANSFER_ID from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                        (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                    /*New */
                    union
                    select TR.FROM_POSITION from {0}.TRANSFER TR 
                    where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                        (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                        and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                         ) 
                        )   
     group by WP.TRANSFER_ID, WP.WORKED_DAY_ID
     ) V1  
join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
group by WD.PER_NUM, v1.pay_type_id
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.ACCOUNT_DATA AD on (T0.TRANSFER_ID = AD.TRANSFER_ID)
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where ((T0.sign_cur_work = 1 or (
    not exists(select null from {0}.TRANSFER T1 where T1.PER_NUM = V2.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0)
    and T0.TYPE_TRANSFER_ID = 3 and extract(year from T0.DATE_TRANSFER) = :YearCalc and extract(month from T0.DATE_TRANSFER) = :MonthCalc ))
    and T0.SUBDIV_ID = :subdiv_id
    or 
    (exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1 and TR2.SUBDIV_ID != :subdiv_id and
                trunc(T0.DATE_TRANSFER) between trunc(:beginDate) and trunc(:endDate))
            and exists(select null from {0}.TRANSFER TR3 where T0.FROM_POSITION = TR3.TRANSFER_ID and
                     TR3.SUBDIV_ID = :subdiv_id )    
    ))  
    and T0.SIGN_COMB = 0 
    and AD.SECRET_ADDITION > 0
    and AD.CHANGE_DATE = (select max(AD1.CHANGE_DATE) from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID =T0.TRANSFER_ID)
union
/* Выбираем дни командировок */
select '9' as PTN, :code_subdiv as SC, '002' as NP, '1' as ZN, case when T0.SIGN_COMB=1 then '2' else chr(32) end as P_RAB, 
 V2.PAY_TYPE_ID as VOP, '00' as PR,       
 V2.ORDER_NAME as ZAK,      
 T0.PER_NUM as TN, lpad(to_char(v2.hcas*10),6,'0') as HCAS, lpad('0',11, '0') as SUM,
 lpad(' ',3, chr(32)) as GM, chr(32) || chr(32) as YN, D.CODE_DEGREE as KT, N_ST
from {0}.TRANSFER T0
join 
(
    select WD.PER_NUM, pay_type_id, round(sum(V1.VTIME)/3600, 1) as hcas, 1 as N_ST, ORDER_NAME
    from (      
         select WP.TRANSFER_ID, WP.WORKED_DAY_ID, 124 as pay_type_id, sum(WP.VALID_TIME) as VTIME,
            OS.ORDER_NAME as ORDER_NAME      
         from {0}.WORK_PAY_TYPE WP      
         left outer join {0}.ORDER_FOR_PT OD on (WP.WORK_PAY_TYPE_ID = OD.WORK_PAY_TYPE_ID)
         left outer join {0}.ORDERS OS on (OD.ORDER_ID = OS.ORDER_ID) 
         where WP.PAY_TYPE_ID in (124) and WP.TRANSFER_ID in (
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        connect by prior TR.FROM_POSITION = TR.TRANSFER_ID
                        start with TR.TRANSFER_ID = (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 
                            where TR.PER_NUM = TR1.PER_NUM and TR1.SIGN_CUR_WORK = 1 and TR1.SUBDIV_ID = :subdiv_id and TR1.SIGN_COMB = 0)
                        /*select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0*/
                        union
                        /*New */
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) and 
                            extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        union
                        select TR.TRANSFER_ID from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc     
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 0 and TR.SUBDIV_ID = :subdiv_id and TR.SIGN_COMB = 0 and TR.TYPE_TRANSFER_ID = 3 and
                            (exists(select * from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc)
                        /*New */
                        union
                        select TR.FROM_POSITION from {0}.TRANSFER TR 
                        where TR.SIGN_CUR_WORK = 1 and TR.SUBDIV_ID != :subdiv_id and TR.SIGN_COMB = 0 and 
                            (exists(select null from {0}.TRANSFER TR1 where TR1.TRANSFER_ID = TR.FROM_POSITION and TR1.SUBDIV_ID = :subdiv_id) 
                            and extract(month from TR.DATE_TRANSFER) = :MonthCalc and extract(year from TR.DATE_TRANSFER) = :YearCalc
                             ) 
                            )   
            and exists(select null from {0}.order_for_pt op where OP.WORK_PAY_TYPE_ID = WP.WORK_PAY_TYPE_ID) 
         group by WP.TRANSFER_ID, WP.WORKED_DAY_ID, OS.ORDER_NAME         
         ) V1  
    join {0}.WORKED_DAY WD on (WD.WORKED_DAY_ID = V1.WORKED_DAY_ID)  
    where trunc(work_date) between trunc(:beginDate) and trunc(:endDate)  
    group by WD.PER_NUM, pay_type_id, ORDER_NAME
) 
v2 on (v2.per_num = T0.PER_NUM) 
left join {0}.STAFFS ST on (T0.STAFFS_ID = ST.STAFFS_ID)  
left join {0}.subdiv s on (T0.subdiv_id = s.subdiv_id)   
left join {0}.position ps on (T0.pos_id = ps.pos_id)  
left join {0}.DEGREE D on (T0.DEGREE_ID = D.DEGREE_ID)  
where ((T0.sign_cur_work = 1 or (
    not exists(select null from {0}.TRANSFER T1 where T1.PER_NUM = V2.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0)
    and T0.TYPE_TRANSFER_ID = 3 and extract(year from T0.DATE_TRANSFER) = :YearCalc and extract(month from T0.DATE_TRANSFER) = :MonthCalc ))
    and T0.SUBDIV_ID = :subdiv_id
    or 
    (exists(select null from {0}.TRANSFER TR2 where T0.PER_NUM = TR2.PER_NUM and TR2.SIGN_CUR_WORK = 1 and TR2.SUBDIV_ID != :subdiv_id and
                trunc(T0.DATE_TRANSFER) between trunc(:beginDate) and trunc(:endDate))
            and exists(select null from {0}.TRANSFER TR3 where T0.FROM_POSITION = TR3.TRANSFER_ID and
                     TR3.SUBDIV_ID = :subdiv_id )    
    ))  
    and T0.SIGN_COMB = 0 
)
order by SC, TN, P_RAB, VOP, N_ST

