SELECT * FROM (
    SELECT T0.CODE_SUBDIV as SC, E.PER_NUM as TN, 
        rpad(E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||' '||substr(E.EMP_MIDDLE_NAME,1,1),20,' ') ||
            case when exists(select null from {0}.EMP_PRIV ep join {0}.TYPE_PRIV tp on TP.TYPE_PRIV_ID = EP.TYPE_PRIV_ID 
                        where EP.PER_NUM = E.PER_NUM and TP.SIGN_INVALID = 1 and 
                            :p_date_dump between TRUNC(EP.DATE_START_PRIV,'MONTH') and 
                            nvl2(EP.DATE_END_PRIV, TRUNC(EP.DATE_END_PRIV,'MONTH')-1, :p_date_dump)) then '2' else ' ' end ||
            case when T0.TYPE_TRANSFER in (2,3) then '*' else ' ' end as FIO,
        case when AD.DATE_ADD is not null then to_char(AD.DATE_ADD,'DD') else to_char(T.DATE_HIRE,'DD') end as POSTD,
        case when AD.DATE_ADD is not null then to_char(AD.DATE_ADD,'MM') else to_char(T.DATE_HIRE,'MM') end as POSTM,
        case when AD.DATE_ADD is not null then to_char(AD.DATE_ADD,'YYYY') else to_char(T.DATE_HIRE,'YYYY') end as POSTG,
        DG.CODE_DEGREE as KT, P.CODE_POS as SHPR, 
        /* 13.02.2015 Меняю условие выбора признака профсоюза 
        case when T0.TYPE_TRANSFER = 1 then PD.SIGN_PROFUNION else 
            case when T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1) 
                then PD.SIGN_PROFUNION else 0 end end as PRF,*/
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1))
            then {0}.GET_SIGN_PROFUNION(T.WORKER_ID, :p_date_dump) else 0 end as PRF,
        nvl(AD.SIGN_ADD,0) as PRN, 
        to_char(nvl(AD.SALARY,0)*1000,'FM000000') as OKL, 
        nvl(AD.CLASSIFIC,0) as RAZ,
        case upper(TG.CODE_TARIFF_GRID) when '101' then '1 7' when '101А' then '1А7' when '101Б' then '1Б7' when '102' then '2 7'
            when '102А' then '2А7' when '102Б' then '2Б7' when '103' then '3 7' when '103А' then '3А7' when '103Б' then '3Б7' 
            else rpad(upper(nvl(TG.CODE_TARIFF_GRID,' ')),3,' ') end as VSET,
         /* Выбираем шифр налога. Обнуляем данные тем кто уволен более месяца назад и тем кто переведен. */
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.TAX_CODE,0),'FM00') else '00' end as NAL, 
        /*trim(to_char(nvl(AD.PERCENT_ALIM,0),'000'))*/'000' as AL, 
        to_char(nvl(AD.HARMFUL_ADDITION,0),'FM00000') as VRED, 
        /*trim(to_char(nvl(AD.PERCENT_BIR,0),'00'))*/'00' as BIR,
        (case upper(E.EMP_SEX) when 'Ж' then '2' else '1' end) as POL,
        to_char(E.EMP_BIRTH_DATE,'DDMMYYYY') as DROG,
        to_char(AD.DATE_SERVANT,'DD') as DVID,
        to_char(AD.DATE_SERVANT,'MM') as DVIM,
        to_char(AD.DATE_SERVANT,'YYYY') as DVIG,
        nvl(AD.SERVICE_ADD,0) as COT,
        to_char(nvl(AD.PERCENT13*10,0),'FM000') as P13,
        /* Выбираем иждивенцев. Обнуляем данные тем кто уволен более месяца назад и тем кто переведен. */
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP14,0),'FM00') else '00' end as KI114,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP15,0),'FM00') else '00' end as KI115,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP16,0),'FM00') else '00' end as KI116,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1))
            then to_char(nvl(AD.COUNT_DEP17,0),'FM00') else '00' end as KI117,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP18,0),'FM00') else '00' end as KI118,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP19,0),'FM00') else '00' end as KI119,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP20,0),'FM00') else '00' end as KI120,
        case when T0.TYPE_TRANSFER = 1 or (T0.TYPE_TRANSFER = 3 and T0.DATE_TRANSFER >= ADD_MONTHS(:p_date_dump, -1)) 
            then to_char(nvl(AD.COUNT_DEP21,0),'FM00') else '00' end as KI121, 
        decode(T.SIGN_COMB, 1, '2', ' ') as P_RAB,
        /* 19,03,2015 Обновляем условие выбора доп.тарифа страхового взноса */
        /*nvl2(AD.PRIVILEGED_POSITION_ID,
            nvl((select decode(substr(PP1.KPS,1,1),1,'09',2,'06') from {0}.PRIVILEGED_POSITION PP1 
            where PP1.PRIVILEGED_POSITION_ID = AD.PRIVILEGED_POSITION_ID), '00'), '00') PR_SPLG,*/
        nvl2(AD.PRIVILEGED_POSITION_ID,
			NVL(TO_CHAR({0}.GET_ADD_RATE(T.TRANSFER_ID, :p_date_dump),'FM00'),
                nvl((select decode(substr(PP1.KPS,1,1),1,'09',2,'06') from {0}.PRIVILEGED_POSITION PP1 
                    where PP1.PRIVILEGED_POSITION_ID = AD.PRIVILEGED_POSITION_ID), '00')), '00') PR_SPLG,
        E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME
    FROM {0}.Emp E 
    join {0}.TRANSFER T on E.PER_NUM = T.PER_NUM
    join (
        select V.*, 
            case 
                when T1.TYPE_TRANSFER_ID = 3 then 3 
                when T1.TYPE_TRANSFER_ID != 3 and 
                    exists(select null from {0}.TRANSFER TT1 
                            where TT1.FROM_POSITION = T1.TRANSFER_ID and TT1.SUBDIV_ID != T1.SUBDIV_ID and TT1.DATE_TRANSFER <= :p_date_dump) then 2
                else 1 end as TYPE_TRANSFER, 
            T1.TRANSFER_ID 
        from {0}.TRANSFER T1
        join {0}.SUBDIV S1 on T1.SUBDIV_ID = S1.SUBDIV_ID
        join
        (
            select T.PER_NUM, S.CODE_SUBDIV, T.SIGN_COMB, max(T.DATE_TRANSFER) as DATE_TRANSFER
            from {0}.TRANSFER T 
            join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID
            where T.DATE_TRANSFER <= :p_date_dump and 
                nvl((select max(TT.DATE_TRANSFER) from {0}.TRANSFER TT where TT.FROM_POSITION = T.TRANSFER_ID), Date '3000-01-01' ) > ADD_MONTHS(:p_date_dump, -12) 
                and (T.TYPE_TRANSFER_ID != 3 or (T.TYPE_TRANSFER_ID = 3 and T.DATE_TRANSFER >=  ADD_MONTHS(:p_date_dump, -12)))
            group by T.PER_NUM, S.CODE_SUBDIV, T.SIGN_COMB
        ) V on (V.PER_NUM = T1.PER_NUM and S1.CODE_SUBDIV = V.CODE_SUBDIV and T1.SIGN_COMB = V.SIGN_COMB 
            and T1.DATE_TRANSFER = V.DATE_TRANSFER)
        ) T0 
        on (T.TRANSFER_ID = T0.TRANSFER_ID)   
    JOIN {0}.Position P ON T.POS_ID = P.POS_ID
    join {0}.degree dg on T.DEGREE_ID = dg.degree_id
    JOIN {0}.ACCOUNT_DATA AD 
        on AD.TRANSFER_ID = decode(T.TYPE_TRANSFER_ID,3,T.FROM_POSITION,T.TRANSFER_ID)         
    LEFT JOIN {0}.TARIFF_GRID TG ON (AD.TARIFF_GRID_ID = TG.TARIFF_GRID_ID)        
                      ) 
order by SC,TN,P_RAB