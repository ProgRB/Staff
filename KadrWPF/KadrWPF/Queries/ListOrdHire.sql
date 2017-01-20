select    
    rownum as n,
    last_name,
    first_name,
    middle_name,
    num_ord
from
(select last_name,
    first_name,
    middle_name,
    num_ord,
    rownum as num
     from  (select em.emp_last_name as last_name,
     em.emp_first_name as first_name,
     em.emp_middle_name as middle_name, 
     tr.tr_num_order as num_ord, 
     tr.tr_date_order
    from {2}.emp em, {2}.transfer tr where tr.per_num = em.per_num and tr.TYPE_TRANSFER_ID = 1 and
    trunc(tr_date_order) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') order by to_number(regexp_substr(tr_num_order,'\d*'))))
where num between ({4} * ({3}-1)+1 ) and ({4} * {3})