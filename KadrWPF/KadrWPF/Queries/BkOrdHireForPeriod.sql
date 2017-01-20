select rownum as num,
 last_name||chr(10)|| first_name||' '|| middle_name,
 num_sub,
 per_num,
 posit,
 num_ord_pr,
 d_ord_pr,
 num_contr,
 dat_contr 
  from  (select distinct
 em.emp_last_name as last_name,
 em.emp_first_name as first_name,
 em.emp_middle_name as middle_name, 
 sub.code_subdiv as num_sub,
 tr.per_num as per_num, 
 pos.pos_name as posit, 
 tr.tr_num_order as num_ord_pr,
 tr.tr_date_order as d_ord_pr,
 tr.contr_emp as num_contr,
 tr.date_contr as dat_contr
from {2}.transfer tr ,{2}.emp em, {2}.subdiv sub, {2}.position pos  
where tr.per_num = em.per_num and sub.subdiv_id = tr.subdiv_id and tr.pos_ID = pos.pos_id and TR.TYPE_TRANSFER_ID = 1 
and trunc(tr_date_order) between to_date('{0}', 'dd.mm.yyyy') and to_date('{1}', 'dd.mm.yyyy') order by  to_number(regexp_substr(tr_num_order,'\d*')))