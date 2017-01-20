WITH tp_per as
(   select V.*/*,
        (select TRANSFER_ID from {0}.TRANSFER T0 where CONNECT_BY_ISLEAF = 1
            start with T0.TRANSFER_ID = V.TRANSFER_ID connect by nocycle T0.TRANSFER_ID = prior T0.FROM_POSITION) TRANS_ID*/
	from 
    (   SELECT TRANSFER_ID,subdiv_id,DEGREE_ID,FORM_OPERATION_ID,trunc(date_transfer) DATE_TRANSFER, 
            DECODE(sign_cur_work,1,DATE '3000-01-01', 
                DECODE(TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+85999/86000,
                    TRUNC((SELECT MIN (date_transfer) FROM {0}.transfer
                           WHERE from_position = t.transfer_id)) - 1 / 86000)) end_transfer,
           DATE_END_CONTR,type_transfer_id,POS_ID,PER_NUM,SIGN_COMB,FROM_POSITION, WORKER_ID
        FROM {0}.transfer t    
        WHERE T.DATE_HIRE <= :endDate and T.HIRE_SIGN = 1
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
    WHERE V.SUBDIV_ID = :p_subdiv_id and V.TRANSFER_ID not in (select TRANSFER_ID FROM {0}.EXCLUDE_FROM_TABLE))
select E1.*,rownum as RN   
from (
    select em.PER_NUM, 
        case when AD.TARIFF_GRID_ID is not null
            then AD.CLASSIFIC || ' / ' || 
                round(BT.TARIFF*DECODE(DTG.TAR_CLASSIF,0,AD.SALARY,DTG.TAR_SAL)/AV.AVERAGE_HOURS*
                    (1+TG.TAR_PERCENT/100)*(DECODE(AD.CLASSIFIC,0,1,AD.HARMFUL_ADDITION/100+1)),2)
            else to_char(round(AD.SALARY*BT.TARIFF ,0))
        end 
        || ' / ' || to_char(round(AD.COMB_ADDITION * BT.TARIFF,0))
        as OKL, 
        em.EMP_LAST_NAME, em.EMP_FIRST_NAME, em.EMP_MIDDLE_NAME, 
        em.EMP_LAST_NAME||' '||em.EMP_FIRST_NAME||' '||em.EMP_MIDDLE_NAME as FIO,		
        --ps.CODE_POS, ps.POS_NAME, D.CODE_DEGREE, 	
        (SELECT ps.CODE_POS FROM {0}.position ps WHERE tr.pos_id = ps.pos_id) CODE_POS, 
        (SELECT ps.POS_NAME FROM {0}.position ps WHERE tr.pos_id = ps.pos_id) POS_NAME, 
		tr.TRANSFER_ID, 	
        (SELECT D.CODE_DEGREE FROM {0}.DEGREE D WHERE TR.DEGREE_ID = D.DEGREE_ID) CODE_DEGREE,
        EO.ORDER_NAME, 
        GM.NAME_GROUP_MASTER as GROUP_MASTER,          
        /* Дата приема в подразделение: минимальная дата из приемной или когда был переведен в подр-е*/
        (select trunc(min(tR6.date_transfer)) from {0}.transfer TR6 
        where TR6.TYPE_TRANSFER_ID != 3
            and tR6.subdiv_id = :p_subdiv_id and tR6.DATE_TRANSFER between :beginDate and :endDate
            and (TR6.SUBDIV_ID != (select T7.SUBDIV_ID from {0}.TRANSFER T7 where T7.TRANSFER_ID = TR6.FROM_POSITION) 
                    or TR6.TYPE_TRANSFER_ID = 1)
        start with TR6.TRANSFER_ID = TR.TRANSFER_ID
        connect by nocycle prior TR6.TRANSFER_ID = TR6.FROM_POSITION or TR6.TRANSFER_ID = prior TR6.FROM_POSITION) 
        as date_Hire,
        /*Дата перевода из подразделения: дочерняя строка от текущего перевода,
            дата перевода в которой текущий месяц и подр-е не равно текущему*/
        (select trunc(TR4.DATE_TRANSFER) from {0}.TRANSFER TR4 
        where TR4.FROM_POSITION = TR.TRANSFER_ID 
            and TR4.DATE_TRANSFER between :beginDate and :endDate
            and TR4.SUBDIV_ID != :p_subdiv_id) 
        as date_transfer,
        case when TR.DATE_TRANSFER between :beginDate and :endDate
            and TR.TYPE_TRANSFER_ID = 3 
            then trunc(TR.DATE_TRANSFER) end as date_dismiss,
        TR.SIGN_COMB, case when TR.SIGN_COMB = 0 then '' else 'X' end as Comb, TR.DEGREE_ID,       
        (CASE WHEN EXISTS(
            select 1 from {0}.WORKED_DAY WD 
            where WD.PER_NUM = em.PER_NUM 
                and 
                WD.TRANSFER_ID in 
                    (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = TR.transfer_id
                    CONNECT BY NOCYCLE PRIOR TR1.transfer_id = TR1.from_position or TR1.transfer_id = PRIOR TR1.from_position)
                and 
                trunc(WD.WORK_DATE) between 
                    (select nvl(trunc(min(t6.date_transfer)),:beginDate) from {0}.transfer T6 
                    where t6.per_num = TR.PER_NUM and T6.SIGN_COMB = TR.SIGN_COMB and 
                        t6.subdiv_id = :p_subdiv_id and t6.date_transfer >= :beginDate
                        and (T6.SUBDIV_ID != (select T7.SUBDIV_ID from {0}.TRANSFER T7 where T7.TRANSFER_ID = T6.FROM_POSITION) 
                                or T6.TYPE_TRANSFER_ID = 1))                 
                    and 
                    nvl(LEAST(nvl((select trunc(TR4.DATE_TRANSFER)-1 from {0}.TRANSFER TR4 
                        where TR4.DATE_TRANSFER between :beginDate and :endDate
                            and TR4.FROM_POSITION = TR.TRANSFER_ID 
                            and TR4.SUBDIV_ID != :p_subdiv_id),:endDate),
                        nvl(case when TR.DATE_TRANSFER between :beginDate and :endDate
                            and TR.TYPE_TRANSFER_ID = 3 
                            then trunc(TR.DATE_TRANSFER) end,:endDate)),:endDate)
                and                 
                not (
                abs(WD.FROM_PERCO - (
                    nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
                        join {0}.ORDERS using (ORDER_ID)
                        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 202) 
                        and WP.REG_DOC_ID is null),0) +
                    nvl((select sum(WP.VALID_TIME) 
                        from {0}.WORK_PAY_TYPE WP 
                        join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
                        join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
						join {0}.ORDERS using (ORDER_ID)
                        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 2 
                        group by WP.WORKED_DAY_ID),0))) between 0 and 75 and 
                        abs(WD.FROM_GRAPH - (
                    nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
                        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 202) 
                         and WP.REG_DOC_ID is null),0) +
                    nvl((select sum(WP.VALID_TIME) 
                        from {0}.WORK_PAY_TYPE WP 
                        join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
                        join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
						join {0}.ORDERS using (ORDER_ID)
                        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 1 
                        group by WP.WORKED_DAY_ID),0))) between 0 and 75
                )    
                )
        THEN 1 ELSE 0 END) as IsPink,
        CASE WHEN WB.EMP_WAYBILL_ID is null then null else 'X' END FL_WAYBILL,
        (select FO.CODE_FORM_OPERATION from {0}.FORM_OPERATION FO where FO.FORM_OPERATION_ID = TR.FORM_OPERATION_ID) CODE_FORM_OPERATION,
        CASE WHEN TR.DATE_END_CONTR-trunc(sysdate) between 0 and 31 
			THEN 1 
			ELSE CASE WHEN TR.DATE_END_CONTR > DATE '2014-05-31' and TR.DATE_END_CONTR-trunc(sysdate)<0 THEN 2 ELSE 0 END 
		END as FL_END_DOG, TR.WORKER_ID
    from {0}.emp em 
    join (select * from tp_per) tr 
        on (em.PER_NUM = tr.PER_NUM and TR.DATE_TRANSFER <= :endDate and END_TRANSFER >= :beginDate)  
    /*join {0}.subdiv s on (tr.subdiv_id = s.subdiv_id) 
    join {0}.position ps on (tr.pos_id = ps.pos_id)
    join {0}.DEGREE D on (TR.DEGREE_ID = D.DEGREE_ID)*/
    --LEFT JOIN {0}.FORM_OPERATION FO ON FO.FORM_OPERATION_ID = TR.FORM_OPERATION_ID    
    join (select TRANSFER_ID, NVL(SALARY,0) SALARY,NVL(CLASSIFIC,0) CLASSIFIC, TARIFF_GRID_ID,
            nvl(HARMFUL_ADDITION,0) HARMFUL_ADDITION, nvl(COMB_ADDITION,0) COMB_ADDITION                               
         FROM {0}.ACCOUNT_DATA A0) AD 
         on (AD.TRANSFER_ID=DECODE(tr.type_transfer_id,3,tr.from_position,tr.TRANSFER_ID))
    JOIN (select B.BASE_TARIFF_ID, B.TARIFF, B.BDATE,
                LEAD(B.BDATE-1/86400,1,DATE '3000-01-01') OVER (ORDER BY B.BDATE) EDATE 
            from {0}.BASE_TARIFF B) BT on (:endDate between BT.BDATE and BT.EDATE)
    LEFT JOIN (select DESCR_TARIFF_GRID.*,LEAD(TAR_DATE-1/86400,1,DATE'3000-01-01') OVER (PARTITION BY tariff_grid_id,tar_classif ORDER BY TAR_DATE) as TAR_END  from {0}.DESCR_TARIFF_GRID) DTG 
        on (AD.TARIFF_GRID_ID=DTG.TARIFF_GRID_ID and nvl(AD.CLASSIFIC,0)=DTG.TAR_CLASSIF and :endDate BETWEEN DTG.tar_date and DTG.tar_END)
    LEFT JOIN {0}.TARIFF_GRID TG on (TG.TARIFF_GRID_ID=DTG.TARIFF_GRID_ID)
    LEFT JOIN {0}.AVERAGE_HOURS AV on (AV.BASE_TARIFF_ID=BT.BASE_TARIFF_ID and AV.COUNT_HOURS=TG.TAR_COUNT_HOUR) 
    left join tp_per tp on (tp.transfer_id=tr.transfer_id)
    left join (select distinct FIRST_VALUE(NAME_GROUP_MASTER) OVER (PARTITION BY TRANSFER_ID ORDER BY END_GROUP DESC) NAME_GROUP_MASTER,
                    TRANSFER_ID
                from (select GM.NAME_GROUP_MASTER,GM.transfer_id,begin_group,NVL(end_group,
                        LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
                    from {0}.EMP_GROUP_MASTER GM join  tp_per tp1 on (gm.transfer_id=tp1.transfer_id)
                    where /*trunc(:endDate) between begin_group and NVL(end_group,date'3000-01-01')*/
                        begin_group <= :endDate and NVL(end_group,date'3000-01-01') >= :beginDate)) GM 
        on (gm.transfer_id=decode(TR.TYPE_TRANSFER_ID,3,tp.from_position, tp.transfer_id)) 
    left join (select * from {0}.EMP_WAYBILL WB0 where WB0.TRANSFER_ID in (select TRANSFER_ID from tp_per)) WB
        on (WB.PER_NUM=TR.PER_NUM)
	left join (select DISTINCT WORKER_ID, FIRST_VALUE(ORDER_NAME) OVER(PARTITION BY WORKER_ID ORDER BY DATE_ORDER DESC) ORDER_NAME 
			from (
				SELECT ORDER_NAME, SUBDIV_ID, PER_NUM, DATE_ORDER, 
                    LEAD(DATE_ORDER-1+86399/86400,1,DATE '3000-01-01') OVER(PARTITION BY WORKER_ID ORDER BY DATE_ORDER) END_ORDER,
					WORKER_ID	
				FROM (
                    select O.ORDER_NAME, EO.SUBDIV_ID, EO.PER_NUM, EO.DATE_ORDER, 
						(select WORKER_ID from {0}.TRANSFER T0 where T0.TRANSFER_ID = EO.TRANSFER_ID) WORKER_ID						
                    from {0}.EMP_ORDER EO
                    join {0}.ORDERS O on EO.ORDER_ID = O.ORDER_ID
                    where EO.DATE_ORDER < :endDate)
				)
            WHERE SUBDIV_ID = :p_subdiv_id and DATE_ORDER <= :endDate and END_ORDER >= :beginDate) EO 
        on EO.WORKER_ID = TR.WORKER_ID
    where /* выбираем работающих в данный момент */ 
        TR.DATE_TRANSFER = 
            (select max(date_transfer) from tp_per tr5 where TR5.WORKER_ID = TR.WORKER_ID
                and TR5.DATE_TRANSFER <= :endDate GROUP BY TR5.WORKER_ID) and
		((exists(select null from USER_ROLE_PRIVS WHERE GRANTED_ROLE = 'STAFF_VIEW_ECON_SERVICE') and 
            TR.FORM_OPERATION_ID = 9) or 
            not exists(select null from USER_ROLE_PRIVS WHERE GRANTED_ROLE = 'STAFF_VIEW_ECON_SERVICE')
            or ORA_LOGIN_USER in ('BMW12714','KNV14534')) 
	order by CASE WHEN WB.EMP_WAYBILL_ID is null then 0 else 1 END, CODE_DEGREE, SIGN_COMB, GROUP_MASTER, ORDER_NAME{1}, PER_NUM 			
    /*order by CASE WHEN WB.EMP_WAYBILL_ID is null then 0 else 1 END, GROUP_MASTER, SIGN_COMB, CODE_DEGREE, ORDER_NAME{1}, PER_NUM     */ 
) E1
order by RN