with HOURS_ON_ORDER as (
		select extract(month from DATE_WORK_ORDER) NUM_MONTH, extract(YEAR from DATE_WORK_ORDER) NUM_YEAR
		from {0}.DATE_FOR_ORDER 
		where ORDER_ON_HOLIDAY_ID = :p_order_on_holiday_id)
select * from (
    select CL.NUM_MONTH, NY.NUM_YEAR, CODE_DEGREE, NVL(PLAN_WITH_REM/*PLAN*/,0) PLAN, FACT, NVL(FACT303,0) FACT303, HOURS_ON_ORDER
    from (select distinct NUM_YEAR FROM HOURS_ON_ORDER) NY,
		TABLE({0}.CALC_LIMIT(:p_subdiv_id, null, null, :p_pay_type_id, :p_order_on_holiday_id, NY.NUM_YEAR)) CL
    )
where NUM_MONTH in (select distinct NUM_MONTH from HOURS_ON_ORDER)
order by NUM_MONTH, CODE_DEGREE