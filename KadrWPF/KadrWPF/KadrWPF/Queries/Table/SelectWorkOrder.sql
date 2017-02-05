WITH tp_per as
    (   select V.TRANSFER_ID, V.PER_NUM, V.SUBDIV_ID, V.POS_ID, V.DEGREE_ID, V.TYPE_TRANSFER_ID, V.FROM_POSITION, WORKER_ID
        from 
        (SELECT        
          subdiv_id, WORKER_ID,
          transfer_id, PER_NUM, POS_ID, DEGREE_ID, TYPE_TRANSFER_ID,FROM_POSITION
        FROM {0}.transfer t    
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        WHERE V.SUBDIV_ID = :p_subdiv_id and DEGREE_ID in (1,2))
select V1.WORK_DATE, V1.CODE_SUBDIV, V1.NGM, V1.CODE_DEGREE, V1.ORDER_NAME,
    null as F1,null as F2,null as F3,null as F4, null as F5,null as F6,null as F7,V1.VTIME, 
    CASE WHEN V1.CODE_DEGREE in ('01','02') 
        THEN /* ≈сли цех 95, то считаем субботы в одинарном размере */ 
         CASE WHEN V1.CODE_SUBDIV not in ('017','051','058') THEN
                ROUND(NVL(TAR_HOUR,TAR_HOUROKL)*V1.VTIME*V1.HARMFULL_ADDITION,2) ELSE 0 END 
            ELSE /*≈сли оклад по командировке не пустой, то умножаем часы на него*/
        ROUND(NVL(TAR_HOUR,TAR_HOUROKL)*V1.VTIME*2*V1.HARMFULL_ADDITION,2) 
    END as VSUM, 
    V1.PER_NUM, V1.CLASSIFIC, V1.FIO
from
(
    select distinct W.WORK_DATE, (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = TR.SUBDIV_ID) CODE_SUBDIV, 
        GM.NGM, (select DE.CODE_DEGREE from {0}.DEGREE DE where DE.DEGREE_ID = TR.DEGREE_ID) CODE_DEGREE,
        round(WP.VALID_TIME/3600,2) VTIME, W.PER_NUM, O.ORDER_NAME,
        AD.CLASSIFIC, AD.HARMFULL_ADDITION,
        E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' as FIO,
        ROUND(ROUND(NVL(RE.REPL_SAL,AD.SALARY)*BT.TARIFF)*3600/{0}.M_Calc_TIME_GRAPH(GR.GR_WORK_ID,:p_date_begin,:p_date_end),2) TAR_HOUROKL, 
        ROUND(BT.TARIFF*NVL(RE.REPL_SAL,DECODE(DTG.TAR_CLASSIF,0,AD.SALARY,DTG.TAR_SAL))/AV.AVERAGE_HOURS*(1+TG.TAR_PERCENT/100),2) as TAR_HOUR     
    from {0}.REG_DOC R 
    join {0}.WORK_PAY_TYPE WP using(REG_DOC_ID)
    join {0}.WORKED_DAY W using(WORKED_DAY_ID)
    join {0}.DOC_LIST D using(DOC_LIST_ID)
    join {0}.ORDERS O using(ORDER_ID)
    join (select * from tp_per) TR on TR.TRANSFER_ID = W.TRANSFER_ID 
    join {0}.EMP E on (E.PER_NUM=W.PER_NUM)
    LEFT JOIN ( select REPL_SAL,REPL_START,TRUNC(REPL_END)+86399/86400 REPL_END, RE11.REPLACING_TRANSFER_ID,
                (select distinct WORKER_ID from {0}.TRANSFER T11 where T11.TRANSFER_ID = RE11.TRANSFER_ID) WORKER_ID
            from {0}.REPL_EMP RE11
            where RE11.SIGN_COMBINE = 0 and :p_date_end>=RE11.REPL_START and :p_date_begin<=RE11.REPL_END) RE
        on (W.WORK_DATE between RE.REPL_START and RE.REPL_END and RE.WORKER_ID = TR.WORKER_ID)
    LEFT JOIN (select transfer_id, SALARY, NVL(CLASSIFIC,0) CLASSIFIC, TARIFF_GRID_ID, NVL(HARMFUL_ADDITION,0)/100+1 HARMFULL_ADDITION
                from {0}.ACCOUNT_DATA) 
        AD on ( AD.transfer_id = NVL2(RE.repl_sal, RE.REPLACING_TRANSFER_ID, DECODE(TR.TYPE_TRANSFER_ID,3, TR.FROM_POSITION,TR.TRANSFER_ID)) )
    left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                    LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
            from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
        GM on (gm.transfer_id=tR.transfer_id and W.WORK_DATE between begin_group and end_group) 
    LEFT JOIN (select DESCR_TARIFF_GRID.*,LEAD(TAR_DATE-1/86400,1,DATE'3000-01-01') OVER (PARTITION BY tariff_grid_id,tar_classif ORDER BY TAR_DATE) as TAR_END  from {0}.DESCR_TARIFF_GRID) DTG on (AD.TARIFF_GRID_ID=DTG.TARIFF_GRID_ID and AD.CLASSIFIC=DTG.TAR_CLASSIF and  W.WORK_DATE BETWEEN DTG.tar_date and DTG.tar_END)
    JOIN (select B.BASE_TARIFF_ID, B.TARIFF, B.BDATE,
                    LEAD(B.BDATE-1/86400,1,DATE '3000-01-01') OVER (ORDER BY B.BDATE) EDATE 
                from {0}.BASE_TARIFF B) BT on (W.work_date between BT.BDATE and BT.EDATE)            
        LEFT JOIN {0}.TARIFF_GRID TG on (TG.TARIFF_GRID_ID=DTG.TARIFF_GRID_ID)            
        LEFT JOIN {0}.AVERAGE_HOURS AV on (AV.BASE_TARIFF_ID=BT.BASE_TARIFF_ID and AV.COUNT_HOURS=TG.TAR_COUNT_HOUR)
    JOIN (select * from (select E.gr_work_id,transfer_id, TRUNC(gr_work_date_begin) GR_WORK_DATE_BEGIN,
                            LEAD(TRUNC(gr_work_date_begin)-1/86400,1,date'3000-01-01') OVER (PARTITION by TRANSFER_ID ORDER BY gr_work_date_begin) GR_WORK_DATE_END, 
							(SELECT WORKER_ID FROM {0}.TRANSFER T WHERE T.TRANSFER_ID = E.TRANSFER_ID) WORKER_ID
                        from  {0}.EMP_GR_WORK e 
                        join {0}.GR_WORK GW on E.GR_WORK_ID = GW.GR_WORK_ID) 
                    where :p_date_begin<=GR_WORK_DATE_END and :p_date_end>=GR_WORK_DATE_BEGIN) GR 
        on GR.WORKER_ID = TR.WORKER_ID
    where D.PAY_TYPE_ID = '124' and W.WORK_DATE between :p_date_begin and :p_date_end
) V1
ORDER BY WORK_DATE, CODE_DEGREE, NGM, PER_NUM