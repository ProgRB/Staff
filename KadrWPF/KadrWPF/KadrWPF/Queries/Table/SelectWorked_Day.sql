WITH GRW as
    (
    select GR_WORK_ID, TRUNC(GR_WORK_DATE_BEGIN) GR_WORK_DATE_BEGIN, 
        LEAD(TRUNC(GR_WORK_DATE_BEGIN)-1/86400,1,DATE '3000-01-01') OVER(ORDER BY GR_WORK_DATE_BEGIN) GR_WORK_DATE_END,
        (SELECT CASE WHEN GW.GR_WORK_NAME like '#НС%' THEN '/НС' ELSE '' END
        FROM {0}.GR_WORK GW WHERE GW.GR_WORK_ID = EGW.GR_WORK_ID) ADD_NOTE
    from {0}.EMP_GR_WORK EGW
    WHERE EGW.PER_NUM = :p_per_num and EGW.TRANSFER_ID in 
        (
            select TR1.TRANSFER_ID from {0}.TRANSFER TR1 where TR1.WORKER_ID = :p_WORKER_ID
        )
    )
select WORKED_DAY_ID, WORK_DATE, round(FROM_GRAPH/3600,2) as FROM_GRAPH, 
    round(FROM_PERCO/3600,2) as FROM_PERCO, TRANSFER_ID, SIGN_CALC,
    nvl(rtrim(nvl2(V.VT102,to_char(V.VT102)||'/',null)||nvl2(V.VT124,to_char(V.VT124)||'/',null)||
			nvl2(V.VT111,to_char(V.VT111)||'/',null)||nvl2(V.VT541,to_char(V.VT541)||'/',null)||
			nvl2(V.VT202,to_char(V.VT202)||'Р/',null),'/'),'0') ||
        /* Добавляем подпись НС для сокращенных графиков работы */
        CASE WHEN VT102 <> 0
			THEN (SELECT ADD_NOTE FROM GRW WHERE WORK_DATE BETWEEN GR_WORK_DATE_BEGIN and GR_WORK_DATE_END)
            ELSE 
                (SELECT ADD_NOTE FROM GRW 
                WHERE WORK_DATE BETWEEN GR_WORK_DATE_BEGIN and GR_WORK_DATE_END
                    and ((select SUM(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 where WP3.WORKED_DAY_ID = V.WORKED_DAY_ID and
                            WP3.PAY_TYPE_ID in (select PAY_TYPE_ID from {0}.DOC_LIST D where D.SIGN_NOTE_545 = 1)) =
                            V.FROM_GRAPH - nvl((select sum(WP4.VALID_TIME) from {0}.WORK_PAY_TYPE WP4 
                                                where V.WORKED_DAY_ID = WP4.WORKED_DAY_ID and WP4.PAY_TYPE_ID = 114),0))
					 and (select C.TYPE_DAY_ID from APSTAFF.CALENDAR C where C.CALENDAR_DAY = WORK_DATE) in (2,3))
             END NOTE,
    case when not (abs(FROM_PERCO - (TIME_PT+TIME_PLUS)) between 0 and 75 and 
            abs(FROM_GRAPH - (TIME_PT+TIME_MINUS)) between 0 and 75) or
            /*(VT111 is not null AND VT541 != 'О') or*/
            (VT124 is not null and VT541 is not null and VT541 not in ('К','ЗК'))
        then 1
        else 0 
	end IsPink
from
(select distinct
    WORKED_DAY_ID, WORK_DATE, nvl(FROM_GRAPH,0) as FROM_GRAPH, nvl(FROM_PERCO,0) as FROM_PERCO, TRANSFER_ID, SIGN_CALC,   
    (select round(sum(WP.VALID_TIME)/3600,2) from {0}.WORK_PAY_TYPE WP 
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 302, 535/*, 303*/))
    as VT102,
	(select round(sum(WP.VALID_TIME)/3600,2) from {0}.WORK_PAY_TYPE WP 
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID = 202)
    as VT202,
    (select round(sum(WP.VALID_TIME)/3600,2) from {0}.WORK_PAY_TYPE WP 
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (124,128))
    as VT124,
    (select distinct listagg('В',';') within group (order by 1) over () from {0}.WORK_PAY_TYPE WP 
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID = 111)
    as VT111,
    (select distinct listagg(DL.DOC_NOTE, ';') within GROUP (order by DL.DOC_NOTE) OVER ()
    from {0}.WORK_PAY_TYPE WP 
    join {0}.REG_DOC RD on (WP.REG_DOC_ID = RD.REG_DOC_ID)
    join {0}.DOC_LIST DL on (RD.DOC_LIST_ID = DL.DOC_LIST_ID)
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and DL.DOC_TYPE = 1
        and (WP.PAY_TYPE_ID in (select D.PAY_TYPE_ID from {0}.DOC_LIST D 
                                where D.SIGN_ALL_DAY = 1 and D.PAY_TYPE_ID not in (102/*, 510*/, 529))
            or WP.PAY_TYPE_ID in (210, 529, 531, 539, 533, 530, 543, 231, 131, 544, 532, 546) /*and WP.VALID_TIME = WD.FROM_GRAPH - 
                nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                    where WP.WORKED_DAY_ID = WP3.WORKED_DAY_ID and WP3.PAY_TYPE_ID = 114),0)*/)
    group by DL.DOC_NOTE)
    as VT541,
    nvl((select sum(WP.VALID_TIME) from {0}.WORK_PAY_TYPE WP 
        join {0}.ORDERS using (ORDER_ID)
    where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and WP.PAY_TYPE_ID in (101, 102, 202) and WP.REG_DOC_ID is null),0) TIME_PT,
    nvl((select sum(WP.VALID_TIME) 
        from {0}.WORK_PAY_TYPE WP 
        join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
        join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
		join {0}.ORDERS using (ORDER_ID)
        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 2 
        group by WP.WORKED_DAY_ID),0) TIME_PLUS,
    nvl((select sum(WP.VALID_TIME) 
        from {0}.WORK_PAY_TYPE WP 
        join {0}.REG_DOC R on (WP.REG_DOC_ID = R.REG_DOC_ID) 
        join {0}.DOC_LIST D on (R.DOC_LIST_ID = D.DOC_LIST_ID) 
		join {0}.ORDERS using (ORDER_ID)
        where WD.WORKED_DAY_ID = WP.WORKED_DAY_ID and D.PAY_TYPE_ID = WP.PAY_TYPE_ID and D.DOC_TYPE = 1 
        group by WP.WORKED_DAY_ID),0) TIME_MINUS    
from {0}.WORKED_DAY WD
where WD.WORK_DATE between TRUNC(:p_date_begin,'MONTH') and :p_date_end and WD.PER_NUM = :p_per_num and WD.TRANSFER_ID in 
    (
        /*select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id 
        CONNECT BY NOCYCLE PRIOR TR1.transfer_id = TR1.from_position or TR1.transfer_id = PRIOR TR1.from_position */
		select TR1.TRANSFER_ID from {0}.TRANSFER TR1 where TR1.WORKER_ID = :p_WORKER_ID and TR1.SUBDIV_ID = :p_SUBDIV_ID
    )
    and WD.SIGN_CALC = 1
group by WD.WORK_DATE, WD.WORKED_DAY_ID, FROM_GRAPH, FROM_PERCO, TRANSFER_ID, SIGN_CALC
) V 
ORDER BY WORK_DATE DESC