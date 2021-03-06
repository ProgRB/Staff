with function get_he_value(emp_sex in varchar) return varchar is
begin
    if upper(emp_sex)='�' then return '�'; else return '���'; end if;
end;
select 
    '   '||repl_name||' '|| apstaff.PADEG.CASE_WORD(p1.pos_name,'�����������','other')||' '|| apstaff.PADEG.CASE_WORD(s1.subdiv_name,'�����������','other')||'('||s1.code_subdiv||')'
        ||' '||apstaff.PADEG.CASE_FULL_FIO(e1.per_num,'�����������')||' (���.� '||e1.per_num||')' as text1,
        
    '���������� ������������ '||apstaff.PADEG.CASE_WORD(p1.pos_name,'�����������', 'other')||' '||apstaff.PADEG.CASE_WORD(s1.subdiv_name,'�����������', 'other')||
    DECODE(repl_start,null,null,' c '||to_char(repl_start,'DD.MM.YYYY'))|| DECODE(repl_end,null,null,' �� '||to_char(repl_end,'DD.MM.YYYY'))
    ||' ��������� �� '|| apstaff.PADEG.CASE_WORD(p2.pos_name,'�����������','other')||' '||apstaff.PADEG.CASE_FULL_FIO(e2.per_num, '�����������')||' (���.� '||e2.per_num||'),  � '||get_he_value(e2.emp_sex)||' ��������, '||
        decode(sign_combine, 1, '��� ������������', '���������')|| ' �� �������� ������������.' text2,
    '�������� ���������� - ���������� ���������������� ����������� ������� �.�. ������� '||apstaff.PADEG.CASE_SHORT_FIO(e2.per_num, '���������')||
    ' �� ���������� ������������ �������� �������������� ��������� ����������� � ������� '||decode(sign_combine, 1, repl_percent, 100)||'% �� ������ �� �������� ���������� '||apstaff.PADEG.CASE_WORD(p1.pos_name,'�����������', 'other')
    ||', ��������������� ������������ ������� ���������� ������������.' TEXT3,        
    date_order ORDER_DATE,
    repl_order CODE_ORDER,
    sign_combine as COMBINE_SIGN
FROM
    apstaff.repl_emp re
    left join apstaff.type_repl tr on (tr.type_repl_id=re.type_repl_id)
    join apstaff.transfer t1 on (re.replacing_transfer_id=t1.transfer_id)
    join apstaff.position p1 on (p1.pos_id=t1.pos_id)
    join apstaff.emp e1 on (e1.per_num=t1.per_num)
    left join apstaff.subdiv s1 on (s1.subdiv_id=t1.subdiv_id)
    join apstaff.transfer t2 on (t2.transfer_id=re.transfer_id)
    join apstaff.emp e2 on (e2.per_num=t2.per_num)
    join apstaff.position p2 on (p2.pos_id=t2.pos_id)    
where 
    repl_emp_id member of :p_repl_id