select emp_link.PER_NUM as per_num, emp_link.code_subdiv as subdivision, emp_link.EMP_LAST_NAME as EMP_LAST_NAME, emp_link.EMP_FIRST_NAME as EMP_FIRST_NAME, emp_link.EMP_MIDDLE_NAME as EMP_MIDDLE_NAME from 
    (select em.PER_NUM, (select CODE_SUBDIV from {0}.subdiv sub where tr.SUBDIV_ID = sub.SUBDIV_ID) as code_subdiv, em.EMP_LAST_NAME, em.EMP_FIRST_NAME, em.EMP_MIDDLE_NAME, tr.TRANSFER_ID from {0}.emp em right join {0}.transfer tr on (em.PER_NUM = tr.PER_NUM) 
    where tr.HIRE_SIGN = 1 and tr.TYPE_TRANSFER_ID = 3 and tr.SIGN_CUR_WORK = 0 and 
 extract(year from tr.DATE_TRANSFER) <  extract(year from SYSDATE) - 1
union 
select em.PER_NUM, '' as CODE_SUBDIV, 
    em.EMP_LAST_NAME, em.EMP_FIRST_NAME, em.EMP_MIDDLE_NAME, 0
    from {0}.emp em where not exists(select * from {0}.transfer tr where em.per_num = tr.per_num))
emp_link {1}