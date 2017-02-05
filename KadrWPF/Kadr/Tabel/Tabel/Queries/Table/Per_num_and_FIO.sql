WITH TRANS as 
    (
    select T.SUBDIV_ID, T.TRANSFER_ID, T.PER_NUM, TRUNC(T.DATE_TRANSFER) DATE_TRANSFER, 
        nvl((select TRUNC(T1.DATE_TRANSFER) - DECODE(T1.TYPE_TRANSFER_ID, 3, 0, 1/86400) 
            from APSTAFF.TRANSFER T1 where T1.FROM_POSITION = T.TRANSFER_ID),DATE '3000-01-01') END_TRANSFER,
        CASE WHEN exists(select TRUNC(T1.DATE_TRANSFER) - DECODE(T1.TYPE_TRANSFER_ID, 3, 0, 1/86400) 
                        from APSTAFF.TRANSFER T1 where T1.FROM_POSITION = T.TRANSFER_ID)                
            THEN 
                (select TRUNC(T1.DATE_TRANSFER) - DECODE(T1.TYPE_TRANSFER_ID, 3, 0, 1/86400) 
                from APSTAFF.TRANSFER T1 where T1.FROM_POSITION = T.TRANSFER_ID)
            ELSE CASE WHEN SIGN_CUR_WORK = 1 THEN DATE '3000-01-01' ELSE DATE '1000-01-01' END
        END END_TRANS, DEGREE_ID,
        T.WORKER_ID, SIGN_COMB
    from APSTAFF.TRANSFER T
    join APSTAFF.SUBDIV S on (T.SUBDIV_ID = S.SUBDIV_ID)
    where T.TYPE_TRANSFER_ID in (1,2) and HIRE_SIGN = 1 and T.SIGN_COMB = 0
    ),
    EGWORK as (
    select T.PER_NUM, TRANSFER_ID, TRUNC(EGW.GR_WORK_DATE_BEGIN) GR_WORK_DATE_BEGIN, 
        LEAD(EGW.GR_WORK_DATE_BEGIN-1/86400,1, DATE '3000-01-01')
        OVER (PARTITION BY T.WORKER_ID ORDER BY GR_WORK_DATE_BEGIN) GR_WORK_DATE_END,
        CASE WHEN GW.GR_WORK_NAME LIKE '#НС%' THEN 1 ELSE 0 END SIGN_SHORT_GR
    from APSTAFF.EMP_GR_WORK EGW
    join APSTAFF.TRANSFER T using(TRANSFER_ID)
    join APSTAFF.GR_WORK GW on (GW.GR_WORK_ID=EGW.GR_WORK_ID)
    where T.SIGN_COMB = 0 --and T.SUBDIV_ID = :p_SUBDIV_ID 
    ),
    DOCS as (
    select T.PER_NUM, TRANSFER_ID, T.WORKER_ID 
    from APSTAFF.REG_DOC R
    join APSTAFF.DOC_LIST D using(DOC_LIST_ID)
    join APSTAFF.TRANSFER T using(TRANSFER_ID)
    where :p_WORK_DATE between R.DOC_BEGIN and R.DOC_END
        and D.PAY_TYPE_ID in (222, 226, 237, 526, 622, 501, 532, 215)
        and T.SUBDIV_ID = :p_SUBDIV_ID and T.SIGN_COMB = 0
    )
select E1.PER_NUM, EMPL, FIO, COMB, E1.TRANSFER_ID, CODE_DEGREE, EO1.SIGN_HOLIDAY, 
    NVL(EO1.COUNT_HOURS,DECODE(:p_pay_type_id,124,6,2)) COUNT_HOURS, EO1.SIGN_ACTUAL_TIME, 
    DATE_FOR_ORDER_ID, EMP_FOR_ORDER_ID
from 
(   select E.PER_NUM, E.emp_last_name||' '||E.emp_first_name||' '||E.emp_middle_name as empl,         
        initcap(E.emp_last_name)||' '||substr(E.emp_first_name,1,1)||'.'||substr(E.emp_middle_name,1,1)||'.' as FIO, 
        DECODE(T.SIGN_COMB,1,'X') COMB, T.TRANSFER_ID, T.WORKER_ID,
        (select D.CODE_DEGREE from APSTAFF.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) as CODE_DEGREE
    from APSTAFF.EMP E
    join (select * from TRANS where DATE_TRANSFER <= :p_WORK_DATE and END_TRANSFER >= :p_WORK_DATE) T on E.PER_NUM = T.PER_NUM  
    where T.SUBDIV_ID = :p_subdiv_id) E1 
left join (select SIGN_HOLIDAY, COUNT_HOURS, SIGN_ACTUAL_TIME,
            TRANSFER_ID, DATE_FOR_ORDER_ID, EMP_FOR_ORDER_ID,
            (select PER_NUM from APSTAFF.TRANSFER T3 where T3.TRANSFER_ID = EO.TRANSFER_ID) PER_NUM
        from APSTAFF.EMP_FOR_ORDER EO
        where DATE_FOR_ORDER_ID = :p_DFO_ID) EO1 
    on E1.PER_NUM = EO1.PER_NUM
where 
    /*15.08.2016 Выкидываем из списка тех, кто на сокращенном графике работы*/
    not exists(select null from EGWORK EGW1 
        where EGW1.PER_NUM = E1.PER_NUM and EGW1.SIGN_SHORT_GR = 1 
            and :p_WORK_DATE between GR_WORK_DATE_BEGIN and GR_WORK_DATE_END)
    /*15.08.2016 Выкидываем из списка тех, у кого есть определенный оправдательный документ на этот день */
    and not exists(select null from DOCS D1 where D1.WORKER_ID = E1.WORKER_ID)
order by E1.EMPL
/*select PER_NUM, EMPL, FIO, COMB, E1.TRANSFER_ID, CODE_DEGREE, EO1.SIGN_HOLIDAY, 
    NVL(EO1.COUNT_HOURS,DECODE(:p_pay_type_id,124,6,2)) COUNT_HOURS, EO1.SIGN_ACTUAL_TIME, 
	DATE_FOR_ORDER_ID, EMP_FOR_ORDER_ID
from 
(   select E.PER_NUM, E.emp_last_name||' '||E.emp_first_name||' '||E.emp_middle_name as empl,         
        initcap(E.emp_last_name)||' '||substr(E.emp_first_name,1,1)||'.'||substr(E.emp_middle_name,1,1)||'.' as FIO, 
        DECODE(T.SIGN_COMB,1,'X') COMB, T.TRANSFER_ID,
		T.PER_NUM TRANS_ID,
        (select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = T.DEGREE_ID) as CODE_DEGREE
    from {0}.EMP E
    join {0}.TRANSFER T on E.PER_NUM = T.PER_NUM  
    where T.SUBDIV_ID = :p_subdiv_id and T.SIGN_CUR_WORK = 1 and T.SIGN_COMB = 0) E1 
left join (select SIGN_HOLIDAY, COUNT_HOURS, SIGN_ACTUAL_TIME,
            TRANSFER_ID, DATE_FOR_ORDER_ID, EMP_FOR_ORDER_ID,
			(select PER_NUM from {0}.TRANSFER T where T.TRANSFER_ID = EO.TRANSFER_ID) TRANS_ID
        from {0}.EMP_FOR_ORDER EO
        where DATE_FOR_ORDER_ID = :p_DFO_ID) EO1 
    on E1.TRANS_ID = EO1.TRANS_ID
order by E1.EMPL*/