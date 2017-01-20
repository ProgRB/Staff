WITH EMP_PASS as 
        (select * from (
            select PER_NUM, EVENT_TIME, WHERE_INTO, WHERE_FROM, MAX(EVENT_TIME) OVER () as MAX_EVENT_TIME,
                MIN(EVENT_TIME) OVER () as MIN_EVENT_TIME, 
                DECODE(WHERE_INTO, LEAD(WHERE_INTO, 1, 0) OVER (ORDER BY EVENT_TIME),1,0) ERROR_EVENT 
            from {1}.EMP_PASS_EVENT where per_num = :p_per_num and trunc(event_time) = trunc(:p_date) 
                and where_into in (1, 2) and where_from in (1, 2))
        where ERROR_EVENT = 0),
        transfer_tree as 
        (select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = :p_transfer_id
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
select V1.FROM_PLANT, V1.INTO_PLANT,
    R.DOC_LIST_ID, R.REG_DOC_ID, R.DOC_LOCATION,R.ABSENCE_ID
from
(
    select DISTINCT V.PER_NUM,
        DECODE(V.WHERE_INTO, 1, V.EVENT_TIME, 
            nvl((select MAX(EVENT_TIME) from EMP_PASS 
                where WHERE_INTO = 1 and EVENT_TIME between V.TIME_BEGIN and V.EVENT_TIME),V.TIME_BEGIN))as FROM_PLANT, 
        DECODE(V.WHERE_INTO, 2, 
            (select MAX(EVENT_TIME) from EMP_PASS 
                where WHERE_INTO = 2 and EVENT_TIME between V.EVENT_TIME and 
                    nvl((select MIN(EVENT_TIME) from EMP_PASS 
                        where WHERE_INTO = 1 and EVENT_TIME between V.EVENT_TIME and V.TIME_END),V.TIME_END)), 
            nvl((select MIN(EVENT_TIME) from EMP_PASS 
                where WHERE_INTO = 2 and EVENT_TIME between V.EVENT_TIME and V.TIME_END),V.TIME_END)) as INTO_PLANT
    from (    
        select EP.WHERE_INTO, EP.EVENT_TIME, TG.TIME_BEGIN, TG.TIME_END, TG.PER_NUM 
        from (select * from EMP_PASS) EP
        join (
            select TW.PER_NUM, TW.TIME_BEGIN + 
                CASE WHEN HOURS_BEGIN in ('07:30','07:45') then nvl(P0.DISPLACE_GRAPH,0)/1440 else 0 end TIME_BEGIN,
                TIME_END, nvl(P0.FREE_EXIT,0) as FREE_EXIT
            from (select PER_NUM, W_DATE, TIME_BEGIN, to_char(TIME_BEGIN, 'HH24:MI') HOURS_BEGIN, TIME_END 
                    from TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date, 'dd.mm.yyyy'),to_char(:p_date, 'dd.mm.yyyy'), :p_per_num, :p_transfer_id))
                    where W_DATE = :p_date) TW 
            left join (select P.PER_NUM, max(TP.DISPLACE_GRAPH) DISPLACE_GRAPH, max(TP.FREE_EXIT) FREE_EXIT, 
                        max(TP.NOT_REGISTR_PASS) NOT_REGISTR_PASS,max(TP.ROUND_TIME) ROUND_TIME
                    from {0}.PERMIT P
                    join {0}.TYPE_PERMIT TP using (TYPE_PERMIT_ID)
                    where P.TRANSFER_ID in (select * from transfer_tree)
                        and :p_date between P.DATE_START_PERMIT and P.DATE_END_PERMIT
                    group by P.PER_NUM) P0
                on TW.PER_NUM = P0.PER_NUM
            where nvl(P0.NOT_REGISTR_PASS,0) = 0 and nvl(P0.ROUND_TIME,0) = 0
        ) TG on EP.PER_NUM = TG.PER_NUM and EP.EVENT_TIME-1/86400 between TG.TIME_BEGIN and TG.TIME_END
        where (TG.FREE_EXIT = 0 or (TG.FREE_EXIT = 1 and (EP.EVENT_TIME = MAX_EVENT_TIME or EP.EVENT_TIME = MIN_EVENT_TIME))) 
            and not exists(
                select null from {0}.REG_DOC R1 
                where R1.TRANSFER_ID in (select * from transfer_tree) 
                    and EP.EVENT_TIME between R1.DOC_BEGIN and R1.DOC_END
                    and R1.DOC_LIST_ID in (select D1.DOC_LIST_ID from {0}.DOC_LIST D1 where D1.SIGN_ALL_DAY = 1 ))
    ) V
) V1
LEFT JOIN (select PER_NUM,DOC_LIST_ID, REG_DOC_ID, DOC_LOCATION,ABSENCE_ID, TRUNC(DOC_BEGIN,'MI') DOC_BEGIN 
        from {0}.REG_DOC where TRANSFER_ID in (select * from TRANSFER_TREE)
            and TRUNC(DOC_BEGIN) = trunc(:p_date)) R 
    on R.PER_NUM = V1.PER_NUM and R.DOC_BEGIN = trunc(V1.FROM_PLANT,'mi')
ORDER BY V1.FROM_PLANT