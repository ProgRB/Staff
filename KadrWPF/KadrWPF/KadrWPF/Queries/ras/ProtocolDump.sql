SELECT * FROM (
    SELECT S.CODE_SUBDIV as SC, E.PER_NUM as TN, 
        rpad(E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||' '||substr(E.EMP_MIDDLE_NAME,1,1),20,' ') ||
        case when exists(select 1 from {0}.EMP_PRIV ep join {0}.TYPE_PRIV tp on TP.TYPE_PRIV_ID = EP.TYPE_PRIV_ID 
        where EP.PER_NUM = E.PER_NUM and TP.SIGN_INVALID = 1 and :p_date_dump between EP.DATE_START_PRIV and 
            (case when EP.DATE_END_PRIV is not null then EP.DATE_END_PRIV else :p_date_dump + 1 end)) then '2' else ' ' end ||
        case when T0.TYPE_TRANSFER in (2,3) then '*' else ' ' end as FIO,
        CASE T.SIGN_COMB WHEN 1 THEN '2' ELSE ' ' END as P_RAB
    FROM {0}.Emp E 
    join {0}.TRANSFER T on E.PER_NUM = T.PER_NUM
    join (select V.*, 
                case 
                    when T1.TYPE_TRANSFER_ID = 3 then 3 
                    when T1.TYPE_TRANSFER_ID != 3 and 
                        exists(select null from {0}.TRANSFER TT1 
                                where TT1.FROM_POSITION = T1.TRANSFER_ID and TT1.SUBDIV_ID != T1.SUBDIV_ID and TT1.DATE_TRANSFER <= :p_date_dump) then 2
                    else 1 end as TYPE_TRANSFER 
            from {0}.TRANSFER T1
            join
            (
            select T.PER_NUM, T.SUBDIV_ID, T.SIGN_COMB, max(T.DATE_TRANSFER) as DATE_TRANSFER
            from {0}.TRANSFER T 
            where T.DATE_TRANSFER <= :p_date_dump and 
                nvl((select max(TT.DATE_TRANSFER) from {0}.TRANSFER TT where TT.FROM_POSITION = T.TRANSFER_ID), Date '3000-01-01' ) > ADD_MONTHS(:p_date_dump, -12) 
                and (T.TYPE_TRANSFER_ID != 3 or (T.TYPE_TRANSFER_ID = 3 and T.DATE_TRANSFER >=  ADD_MONTHS(:p_date_dump, -12)))
            group by T.PER_NUM, T.SUBDIV_ID, T.SIGN_COMB
            ) V on V.PER_NUM = T1.PER_NUM and T1.SUBDIV_ID = V.SUBDIV_ID and T1.SIGN_COMB = V.SIGN_COMB and T1.DATE_TRANSFER = V.DATE_TRANSFER
            ) T0 
            on (T.PER_NUM = T0.PER_NUM and T.SUBDIV_ID = T0.SUBDIV_ID and T.SIGN_COMB = T0.SIGN_COMB and T.DATE_TRANSFER = T0.DATE_TRANSFER)   
    JOIN {0}.Subdiv S ON T.SUBDIV_ID = S.SUBDIV_ID 
    JOIN {0}.Position P ON T.POS_ID = P.POS_ID
    join {0}.degree dg on T.DEGREE_ID = dg.degree_id
    join {0}.per_data pd on E.PER_NUM = PD.PER_NUM
    LEFT JOIN {0}.ACCOUNT_DATA AD 
                   on (T.TRANSFER_ID = AD.TRANSFER_ID and T.TYPE_TRANSFER_ID != 3) or (T.FROM_POSITION = AD.TRANSFER_ID and T.TYPE_TRANSFER_ID = 3) 
    LEFT JOIN {0}.TARIFF_GRID TG ON (AD.TARIFF_GRID_ID = TG.TARIFF_GRID_ID) 
    where AD.CHANGE_DATE = (select max(AD1.CHANGE_DATE) from {0}.ACCOUNT_DATA AD1 where AD1.TRANSFER_ID = AD.TRANSFER_ID)         
        and AD.DATE_SERVANT is null    
                      ) 
order by SC,TN,P_RAB