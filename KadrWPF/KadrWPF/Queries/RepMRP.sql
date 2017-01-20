select rownum, t_emp.* 
from (
select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    ME.per_num, E.EMP_LAST_NAME||' '||E.EMP_FIRST_NAME||' '||E.EMP_MIDDLE_NAME FIO,
    (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T.POS_ID) POS_NAME  
from (select distinct PER_NUM from {0}.Mat_resp_person) ME
join {0}.EMP E on ME.PER_NUM = E.PER_NUM
left join {0}.TRANSFER T on T.PER_NUM = E.PER_NUM
where T.SIGN_CUR_WORK = 1 and (T.SIGN_COMB = 0 or (T.SIGN_COMB = 1 and 
    not exists(select null from {0}.TRANSFER T1 
                where T1.per_num = T.per_num and T1.sign_cur_work=1 and T1.sign_comb=0)))
order by 1, 3) t_emp