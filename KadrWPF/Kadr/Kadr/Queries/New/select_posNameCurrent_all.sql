select p.pos_id,p.pos_name,count(s.pos_id) cnt
from (select staffs_id from {0}.staffs
    MINUS
            select distinct temp_over_id from {0}.staffs  
        where  staff_sign!=0 and nvl(date_end_staff,DATE'3000-01-01')>:p_cur_date
           and date_begin_staff<=:p_cur_date
     ) s1 join {0}.staffs s on (s1.staffs_id=s.staffs_id)      
    join {0}.position p  on (s.pos_id=p.pos_id) 
    join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id)         
  where staff_sign!=0
    and s.subdiv_id=:p_subdiv_id
        and  nvl(date_end_staff,DATE'3000-01-01')>trunc(:p_cur_date)   
        and nvl(date_begin_staff,date'1000-01-01')<=trunc(:p_cur_date)
    {1}    
group by p.pos_id,p.pos_name 
order by p.pos_name 