select D.CODE_DEGREE, NVL(V.PLAN,0) PLAN, NVL(V.FACT,0) FACT, NVL(V.FACT303,0) FACT303 
from APSTAFF.DEGREE D
left join (
	with HOURS_ON_ORDER as (select extract(month from :p_date_doc) NUM_MONTH from dual
							union all
							select extract(month from DFO.DATE_WORK_ORDER) NUM_MONTH
							from {0}.DATE_FOR_ORDER DFO 
							where ORDER_ON_HOLIDAY_ID = :p_order_on_holiday_id)
	select CODE_DEGREE, PLAN_WITH_REM PLAN, FACT, FACT303, NUM_MONTH
	from (
		select * 
        from TABLE({0}.CALC_LIMIT(:p_subdiv_id, :p_degree_id, :p_date_doc, :p_pay_type_id, :p_order_on_holiday_id, 
			Extract(YEAR from :p_date_doc)))
		)
	where NUM_MONTH = extract(MONTH from :p_date_doc)
		/*NUM_MONTH in (select distinct NUM_MONTH from HOURS_ON_ORDER)/*:p_date_doc BETWEEN LIMIT_BEGIN and LIMIT_END*/ 
		and DEGREE_ID = NVL2(:p_degree_id,:p_degree_id,DEGREE_ID)
) V on D.CODE_DEGREE = V.CODE_DEGREE
where DEGREE_ID = NVL2(:p_degree_id,:p_degree_id,DEGREE_ID)