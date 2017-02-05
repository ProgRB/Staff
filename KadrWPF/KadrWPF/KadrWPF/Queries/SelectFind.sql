select emp_link.PER_NUM as per_num, emp_link.code_subdiv as subdivision, emp_link.EMP_LAST_NAME as EMP_LAST_NAME, emp_link.EMP_FIRST_NAME as EMP_FIRST_NAME, emp_link.EMP_MIDDLE_NAME as EMP_MIDDLE_NAME from 
    (select em.PER_NUM, (select CODE_SUBDIV from {0}.subdiv sub where tr.SUBDIV_ID = sub.SUBDIV_ID) as code_subdiv, em.EMP_LAST_NAME, em.EMP_FIRST_NAME, em.EMP_MIDDLE_NAME, tr.TRANSFER_ID from {0}.emp em right join {0}.transfer tr on (em.PER_NUM = tr.PER_NUM) 
    where (tr.SIGN_CUR_WORK = 1 or (tr.SIGN_CUR_WORK != 1 and ((tr.DATE_TRANSFER = (select max(DATE_TRANSFER) from {0}.transfer tr2 where tr.PER_NUM = tr2.PER_NUM  and tr2.SIGN_COMB = 0 ) and   tr.SIGN_COMB = 0 and 
    extract(year from tr.DATE_TRANSFER) >=  extract(year from SYSDATE) - 1 )
or (tr.DATE_TRANSFER = (select max(DATE_TRANSFER) from {0}.transfer tr2 where tr.PER_NUM = tr2.PER_NUM  and tr2.SIGN_COMB = 1 ) and  tr.SIGN_COMB = 1 and 
    extract(year from tr.DATE_TRANSFER) >=  extract(year from SYSDATE) - 1)))) 
        and tr.HIRE_SIGN = 1) 
emp_link {1}