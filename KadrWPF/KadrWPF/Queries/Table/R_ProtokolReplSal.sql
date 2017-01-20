select 
	e.per_num,
	emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' FIO,
	NOTE,
	"TIME",
	round(SUM,2),
	ORDER_NAME
from {0}.TEMP_SALARY ts
	join {0}.emp e on (e.per_num=ts.per_num)
where REPL_SIGN=1 AND SIGN_VISIBLE=1 and ts.TEMP_SALARY_ID in ({1}) 
ORDER BY 2,PAY_TYPE_ID
	