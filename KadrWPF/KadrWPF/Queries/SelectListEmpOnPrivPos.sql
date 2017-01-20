with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT S.CODE_SUBDIV, V2.PER_NUM, E.EMP_LAST_NAME||' '||E.EMP_FIRST_NAME||' '||E.EMP_MIDDLE_NAME FIO,
    SUBSTR(PR.INSURANCE_NUM,1,3) ||'-'||SUBSTR(PR.INSURANCE_NUM,4,3) ||'-'||SUBSTR(PR.INSURANCE_NUM,7,3) ||' '||
    SUBSTR(PR.INSURANCE_NUM,10,2) INSURANCE_NUM, DECODE(SIGN_COMB,1,'X',' ') SIGN_COMB, 
    P.POS_NAME||POS_NOTE, PP.SPECIAL_CONDITIONS, PP.KPS, V2.W_BEGIN, V2.W_END,
    DECODE(V2.PAY_TYPE_ID,531,'Административный отпуск',215,'Ученический отпуск',533,'Прогул',
        532,'Административный по уходу за ребенком до 3-х лет', 
        237,'Временная нетрудоспособность',501,'Отпуск по беременности и родам') DOC_NAME
FROM (
    with tr_per as 
    (   select T.PER_NUM,T.TRANSFER_ID,T.FROM_POSITION,T.WORKER_ID,T.TYPE_TRANSFER_ID,T.SIGN_COMB,T.POS_ID,T.SUBDIV_ID,
            trunc(T.DATE_TRANSFER) DATE_TRANSFER, 
            DECODE(sign_cur_work,1,date'3000-01-01', 
                DECODE(TYPE_TRANSFER_ID,3,TRUNC(date_transfer)+85399/86400, 
                    TRUNC((SELECT date_transfer+DECODE(TYPE_TRANSFER_ID,3,1,0) FROM {0}.transfer WHERE from_position = t.transfer_id))-1/86400))
            as end_transfer, AD.PRIVILEGED_POSITION_ID PRIV_POS, T.POS_NOTE
        from {0}.TRANSFER T 
        join {0}.ACCOUNT_DATA AD on T.TRANSFER_ID = AD.TRANSFER_ID
        where T.TYPE_TRANSFER_ID != 3
        start with T.TRANSFER_ID in (
            select A.TRANSFER_ID
            from {0}.ACCOUNT_DATA A 
            join {0}.PRIVILEGED_POSITION P using (PRIVILEGED_POSITION_ID)
            ) 
        connect by prior T.TRANSFER_ID = T.FROM_POSITION and LEVEL < 3
        union
        select T.PER_NUM,T.TRANSFER_ID,T.FROM_POSITION,T.WORKER_ID,T.TYPE_TRANSFER_ID,T.SIGN_COMB,T.POS_ID,T.SUBDIV_ID,
            trunc(T.DATE_TRANSFER) DATE_TRANSFER, 
            DECODE(sign_cur_work,1,date'3000-01-01', 
                DECODE(TYPE_TRANSFER_ID,3,TRUNC(date_transfer)+85399/86400, 
                    TRUNC((SELECT date_transfer+DECODE(TYPE_TRANSFER_ID,3,1,0) FROM {0}.transfer WHERE from_position = t.transfer_id))-1/86400))
            as end_transfer, AD.PRIVILEGED_POSITION_ID PRIV_POS, T.POS_NOTE
        from {0}.TRANSFER T
        join {0}.ACCOUNT_DATA AD on T.TRANSFER_ID = AD.TRANSFER_ID
        where T.TYPE_TRANSFER_ID != 3 
        start with T.TRANSFER_ID in (
            select A.TRANSFER_ID
            from {0}.ACCOUNT_DATA A 
            join {0}.PRIVILEGED_POSITION P using (PRIVILEGED_POSITION_ID)
            ) 
        connect by T.TRANSFER_ID = prior T.FROM_POSITION and LEVEL < 3
    )
    select DISTINCT PER_NUM, PAY_TYPE_ID, TRANSFER_ID, POS_ID, POS_NOTE, SUBDIV_ID,SIGN_COMB,PRIV_POS,
        MIN(CALENDAR_DAY) OVER(PARTITION BY TRANSFER_ID,PAY_TYPE_ID,NEW_RANKE) W_BEGIN,
        MAX(CALENDAR_DAY) OVER(PARTITION BY TRANSFER_ID,PAY_TYPE_ID,NEW_RANKE) W_END
    from (
        select PER_NUM, CALENDAR_DAY, PAY_TYPE_ID, TRANSFER_ID, POS_ID, POS_NOTE, SUBDIV_ID,SIGN_COMB,PRIV_POS,
                /*Создаем поле, по которому будем объединять документы - если дни идут друг за другом - 
                объединяем их в один документ*/  
               nvl2(WORK_DATE,RANKE,CALENDAR_DAY -
                    ROW_NUMBER() OVER (PARTITION BY TRANSFER_ID,PAY_TYPE_ID ORDER BY TRANSFER_ID,PAY_TYPE_ID,CALENDAR_DAY)) NEW_RANKE
        from (
            select PER_NUM, CALENDAR_DAY, NVL2(TR.PRIV_POS,PAY_TYPE_ID,null) PAY_TYPE_ID, TR.TRANSFER_ID, 
                NVL2(TR.PRIV_POS,WORK_DATE,null) WORK_DATE, NVL2(TR.PRIV_POS,RANKE,null) RANKE, 
                TR.POS_ID, TR.POS_NOTE, TR.SUBDIV_ID, TR.SIGN_COMB, TR.PRIV_POS
            from (select * from TR_PER where DATE_TRANSFER <= :p_endPeriod and end_transfer >= :p_beginPeriod
                    and (PRIV_POS is not null or (PRIV_POS is null and (DATE_TRANSFER between :p_beginPeriod and :p_endPeriod)
                        or (end_transfer between :p_beginPeriod and :p_endPeriod)))) TR
            join (select CALENDAR_DAY from {0}.CALENDAR where CALENDAR_DAY between :p_beginPeriod and :p_endPeriod) C
                on C.CALENDAR_DAY between TR.DATE_TRANSFER and TR.END_TRANSFER 
            left join (
                select W.WORK_DATE, WP.PAY_TYPE_ID, W.TRANSFER_ID, WORK_DATE - 
                    --rownum
                    ROW_NUMBER() OVER (PARTITION BY PER_NUM ORDER BY PER_NUM, WORK_DATE) 
                    RANKE,
                    (select T.WORKER_ID from {0}.TRANSFER T where T.TRANSFER_ID = W.TRANSFER_ID) WORKER_ID  
                from (select * from {0}.WORKED_DAY
                        where WORK_DATE between :p_beginPeriod and :p_endPeriod and 
                            TRANSFER_ID in (select TRANSFER_ID from TR_PER)
                        order by PER_NUM, WORK_DATE, TRANSFER_ID
                    ) W
                join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
                where WP.PAY_TYPE_ID in (215, 531, 533, 532, 237, 501) and 
                    WP.VALID_TIME = W.FROM_GRAPH - nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                                                        where WP.WORKED_DAY_ID = WP3.WORKED_DAY_ID and WP3.PAY_TYPE_ID = 114),0)
                ) DOC
                on C.CALENDAR_DAY = DOC.WORK_DATE and TR.WORKER_ID = DOC.WORKER_ID--TR.TRANSFER_ID = DOC.TRANSFER_ID
            ) V    
        )
    ) V2
join {0}.SUBDIV S on V2.SUBDIV_ID = S.SUBDIV_ID
join {0}.POSITION P on V2.POS_ID = P.POS_ID
join {0}.EMP E on V2.PER_NUM = E.PER_NUM
join {0}.PER_DATA PR on V2.PER_NUM = PR.PER_NUM
left join {0}.PRIVILEGED_POSITION PP on (V2.PRIV_POS = PP.PRIVILEGED_POSITION_ID)
WHERE S.SUBDIV_ID in (select subdiv_id from SUBD)
    /*exists(select null from user_role_privs where granted_role =  'STAFF_FULL_FILL') or 
    exists(select null from role_role_privs where granted_role =  'STAFF_FULL_FILL') or
    S.SUBDIV_ID in
        (select subdiv_id from {0}.access_subdiv where upper(user_name) = ora_login_user and upper(app_name) = 'KADR')*/
ORDER BY S.CODE_SUBDIV, PER_NUM, W_BEGIN