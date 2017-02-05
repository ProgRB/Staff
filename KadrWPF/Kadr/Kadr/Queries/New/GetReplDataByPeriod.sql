select re.repl_emp_id,
	e.emp_last_name||' '||substr(e.emp_first_name,1,1)||'.'||substr(e.emp_middle_name,1,1)||'.' "Замещающий", 
	e1.emp_last_name||' '||substr(e1.emp_first_name,1,1)||'.'||substr(e1.emp_middle_name,1,1)||'.' "Замещаемый", 
	to_char(repl_start,'DD.MM.YYYY') repl_start,
	to_char(repl_end,'DD.MM.YYYY') repl_end,
	repl_sal "Оклад совмещения",
	repl_percent "Проценты",
	repl_order,
	to_char(date_order,'DD.MM.YYYY') date_order, 
	DECODE(SIGN_LOCK_REPL,1,'Закрыто', 'Не закрыто') "Статус",
	sign_lock_repl,
	nvl(sign_combine,0) sign_combine
from {0}.repl_emp re 
	/*join   ( select transfer_id, per_num, (select max(subdiv_id) keep (dense_rank last order by date_transfer) 
		from {0}.transfer where 
			trunc(date_transfer)<=:p_dateEnd 
			and subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=NVL(:p_subdiv_id,0) connect by prior subdiv_id=parent_id) 
			start with transfer_id=t.transfer_id connect by nocycle prior transfer_id=from_position or prior from_position=transfer_id) as subdiv_id,
     	degree_id
        from {0}.transfer t ) t on (t.transfer_id=re.transfer_id) */
	join  {0}.transfer t on (t.transfer_id=re.transfer_id) 
	join {0}.emp e on (e.per_num=t.per_num) 
	left join {0}.degree d on (d.degree_id=t.degree_id)  
	left join {0}.transfer t1 on (re.replacing_transfer_id=t1.transfer_id) 
	left join {0}.emp e1 on (e1.per_num=t1.per_num) 
where nvl(repl_end,date'3000-01-01')>=:p_dateBegin and repl_start<=:p_dateEnd
 and /*t.subdiv_id is not null*/ (T.SUBDIV_ID = :p_subdiv_id or T1.SUBDIV_ID=:p_subdiv_id)
order by e.emp_last_name,e.emp_first_name,repl_start