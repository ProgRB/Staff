select emp_link.{3} as per_num, emp_link.{5} as LAST_NAME, emp_link.{6} as FIRST_NAME, emp_link.{7} as MIDDLE_NAME, emp_link.CODE_SUBDIV as CODE_SUBDIV from 
    (select em.{3}, em.{5}, em.{6}, em.{7}, s.{8}, {4} from {0}.{1} em right join {0}.{2} tr on (em.{3} = tr.{3})
    left join {0}.{9} s on tr.{10} = s.{10} 
    where tr.{4} in (select {4} from {0}.{2} tr2 where tr2.{3} = tr.{3} 
        and date_transfer = (select max(date_transfer) from {0}.{2} tr4 where tr4.{3} = tr.{3}) 
        and tr.hire_sign = 1))
emp_link where {3}={3} order by emp_link.{3}
/*order by emp_link.{3}*/