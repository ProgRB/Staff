with function GetID(p_repl_emp_id number) return number is
    k number;
begin
    begin
        select min(repl_emp_id) into k 
        from apstaff.repl_emp 
        where connect_by_isleaf=1
        start with repl_emp_id=p_repl_emp_id
		connect by nocycle prior replacing_transfer_id=replacing_transfer_id and prior repl_start-1=repl_end;
    exception 
        when no_data_found then null;
    end;
    return nvl(k, p_repl_emp_id);
end;
select
    GetID(repl_emp_id) as ID,
    e.per_num as per_num,
    p.pos_name as pos_name,
    case when re.type_repl_id=6 then null else Initcap(e.EMP_LAST_NAME)||' '||Initcap(e.EMP_FIRST_NAME)||' '||initcap(e.emp_middle_name) end FIO,
    s.code_subdiv as code_subdiv,
    report_repl_name text1,
    e1.per_num as per_num1,
    p1.pos_name as pos_name1,
    Initcap(e1.EMP_LAST_NAME)||' '||Initcap(e1.EMP_FIRST_NAME)||' '||initcap(e1.emp_middle_name) FIO1,
    s1.code_subdiv as code_subdiv1,
    repl_start as date_begin,
    repl_end as date_end,     
	repl_percent as percent,   
    date_order ORDER_DATE,
    repl_order CODE_ORDER,
    sign_combine as COMBINE_SIGN
FROM
    apstaff.repl_emp re
    left join apstaff.type_repl tr on (tr.type_repl_id=re.type_repl_id)
    join apstaff.transfer t on (re.replacing_transfer_id=t.transfer_id)
    join apstaff.position p on (p.pos_id=t.pos_id)
    join apstaff.emp e on (e.per_num=t.per_num)
    join apstaff.subdiv s on (s.subdiv_id=t.subdiv_id)
    join apstaff.transfer t1 on (t1.transfer_id=re.transfer_id)
    join apstaff.emp e1 on (e1.per_num=t1.per_num)
    join apstaff.position p1 on (p1.pos_id=t1.pos_id)
    join apstaff.subdiv s1 on (s1.subdiv_id=t1.subdiv_id)   
where 
    repl_emp_id member of :p_repl_id