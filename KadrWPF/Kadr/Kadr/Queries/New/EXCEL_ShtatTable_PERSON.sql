select rownum,a.*
from (
select substr(p.code_pos,1,1) fcc ,
     p.code_pos,
     DECODE(grouping(p.pos_name),1,to_char(count(distinct sf.staffs_id)), p. pos_name), 
     DECODE(grouping(sf.tar_by_schema),1,SUM(SF.tar_by_schema),sf.tar_by_schema) , 
      case 
            when ad.salary>SF.tar_by_schema then ad.salary else null end salary ,
     DECODE(grouping( ad.comb_addition),1,SUM( ad.comb_addition),NULLIF(ad.comb_addition,0)) , 
     DECODE(grouping( sf.HARMFUL_ADDITION),1,SUM( sf.HARMFUL_ADDITION), sf.HARMFUL_ADDITION) ,
     e.emp_last_name||' '||emp_first_name||' '||emp_middle_name fio,    
     NVL2(code_tariff_grid, 'тар.сетка'||code_tariff_grid,null) ||
     case 
            when 1=1 /* exists(select * from {0}.repl_emp re1 where re1.staffs_id=SF.staffs_id and sign_longtime=1 and repl_actual_sign=1 and SYSDATE BETWEEN  nvl(re1.repl_start,date'1000-01-01') and nvl(re1.repl_end,'01.01.3000')) */
            then null /*(select chr(10)||' вр. '||emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1) from {0}.repl_emp re1 join {0}.transfer tr1 on (tr1.transfer_id=RE1.TRANSFER_ID) join {0}.emp on (EMP.PER_NUM=tr1.per_num)
             where re1.staffs_id=SF.staffs_id and sign_longtime=1 and repl_actual_sign=1 and SYSDATE BETWEEN  nvl(re1.repl_start,date'1000-01-01') and nvl(re1.repl_end,'01.01.3000'))*/
           else null 
     end ||NVL2(date_add_agree,chr(10)||' доп.согл. '||to_char(date_add_agree,'DD.MM.YYYY'),null)  com
    from 
        (select * from {0}.staffs where :p_cur_date between NVL(date_begin_staff,date'1000-01-01') and NVL(date_end_staff,date'3000-01-01')-1) sf
        join {0}.position p  on (SF.pos_id=p.pos_id)   
        join {0}.subdiv on (SF.subdiv_id=subdiv.subdiv_id) 
        left join (
            select transfer.per_num,transfer.transfer_id,transfer.pos_id,transfer.staffs_id,0 as is_repl from  {0}.transfer  where sign_cur_work=1 
            ) t on (t.staffs_id=SF.staffs_id)
        left join (select * from (select ad.*,row_number() over (partition by transfer_id order by change_date desc) as rn from {0}.account_data ad) where rn=1 )  ad on (ad.transfer_id=t.transfer_id)  
        left join {0}.emp e on (e.per_num=t.per_num)
        left join {0}.position p1 on (t.pos_id=p1.pos_id)
        left join {0}.degree on (SF.degree_id=degree.degree_id)
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=SF.tarif_grid_id)
     where SF.staff_sign=1 and SF.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
                and sf.degree_id=:p_degree_id  
     group by rollup ((sf.staffs_id,p.code_pos,p.pos_name,sf.tar_by_schema,ad.salary,ad.comb_addition, sf.harmful_addition, emp_last_name,emp_first_name,emp_middle_name,code_tariff_grid,sf.staffs_id,date_add_agree))
     order by mod(to_number(fcc)+1,3) asc, nvl(SF.tar_by_schema,0) desc
  ) a