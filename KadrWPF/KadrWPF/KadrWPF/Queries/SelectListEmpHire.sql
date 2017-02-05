with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
select * from 
	 (select (select CODE_SUBDIV from {0}.subdiv where SUBDIV_ID = tr.SUBDIV_ID) as CODE_SUBDIV, 
	 em.PER_NUM as PER_NUM, (case tr.TYPE_TRANSFER_ID when 3 then '*' else '' end) as DISMISS,
	 em.EMP_LAST_NAME as EMP_LAST_NAME, em.EMP_FIRST_NAME as EMP_FIRST_NAME, em.EMP_MIDDLE_NAME as EMP_MIDDLE_NAME, 
	 (case tr.SIGN_COMB when 1 then 'X' else '' end) as SIGN_COMB,   
	 em.EMP_BIRTH_DATE as EMP_BIRTH_DATE, 
	 (select CODE_POS from {0}.position pos where tr.POS_ID = pos.POS_ID) as CODE_POS, 
	 (select POS_NAME from {0}.position pos where tr.POS_ID = pos.POS_ID) as POS_NAME,
	 tr.TRANSFER_ID, tr.DATE_HIRE, TR.POS_ID 
	from {0}.emp em right join {0}.transfer tr on (em.PER_NUM = tr.PER_NUM) 
	 where (tr.SIGN_CUR_WORK = 1 or (tr.SIGN_CUR_WORK != 1 
	and ((tr.DATE_TRANSFER = (select max(DATE_TRANSFER) from {0}.transfer tr2 where tr.PER_NUM = tr2.PER_NUM  and tr2.SIGN_COMB = 0 ) and   tr.SIGN_COMB = 0 and 
		extract(year from tr.DATE_TRANSFER) >=  extract(year from SYSDATE) - 1 )
	or (tr.DATE_TRANSFER = (select max(DATE_TRANSFER) from {0}.transfer tr2 where tr.PER_NUM = tr2.PER_NUM  and tr2.SIGN_COMB = 1 ) and  tr.SIGN_COMB = 1 and 
		extract(year from tr.DATE_TRANSFER) >=  extract(year from SYSDATE) - 1)))) and tr.HIRE_SIGN = 0
	and
	tr.SUBDIV_ID in (select subdiv_id from SUBD)
	/*tr.subdiv_id in
	 ( select subdiv_id from {0}.transfer tr3 where 
	per_num = regexp_substr(ora_login_user,'\d.*')                                      
	or trim(regexp_substr(ora_login_user,'\d')) is null
	or exists(select null from user_role_privs where granted_role =  'STAFF_FULL_FILL')
	or exists(select null from role_role_privs where granted_role =  'STAFF_FULL_FILL'))*/
) 
cur_emp{1}

/*
0 schema
1 sort.ToString()
*/