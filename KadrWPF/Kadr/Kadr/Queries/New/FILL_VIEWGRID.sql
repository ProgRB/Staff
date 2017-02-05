select * from (
select code_pos "Код профессии",pos_name "Профессия",count(distinct s.staffs_id) "Количество единиц",  
     s.classific "Разряд", 
     case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end  "Тарифный коэфф. по схеме", 
     case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end "Тарифная ставка",
      case when s.classific is null then round(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end*tarif.val) 
            else (case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end)*tarif.val end  "рубл.",
     t.comb_addition "Надбавка за совм", 
     t.comb_addition*tarif.val "руб.",
     S.ADD_EXP_AREA "Надбавка за расш. обслуж.",
     s.add_exp_area* tarif.val "pубл.",
     (nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0)+(1+nvl(s.add_exp_area,0))+nvl(t.comb_addition,0))*tarif.val "Мес. фонд. ЗП",
     S.HARMFUL_ADDITION "Надбавка за вредн.",
     code_tariff_grid "Тарифная сетка"
 
 from
            (select tariff val from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) tarif,
             (select staffs_id from {0}.staffs  
              where 
                staff_sign!=0 
             {1}                
                 and subdiv_id in 
                            (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)   
                            and  nvl(date_end_staff,DATE'3000-01-1')>:p_cur_date
                            and nvl(date_begin_staff,DATE'1000-01-01')<=:p_cur_date
                   MINUS
                        select distinct temp_over_id from {0}.staffs  where
				staff_sign!=0 and 
                            NVL(date_end_staff,DATE'3000-01-01')>:p_cur_date   
                            and nvl(date_begin_staff,DATE'1000-01-01')<=trunc(:p_cur_date)       
                ) s1 
        join {0}.staffs s on (s1.staffs_id=s.staffs_id)
        left join {0}.position  on (s.pos_id=position.pos_id)   
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
        left  join (select staffs_id,salary,comb_addition  
					 FROM 
                           ( select t1.transfer_id,t1.staffs_id from {0}.transfer t1 where sign_cur_work=1 and t1.staffs_id is not null and 
                           					not exists( select transfer_id from {0}.repl_emp where staffs_id=t1.staffs_id and :p_cur_date   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1 )
                             union all
                             select replacing_transfer_id,staffs_id from {0}.repl_emp re join {0}.transfer t1 on (t1.transfer_id=re.transfer_id) where sign_longtime=1 and repl_actual_sign=1 and :p_cur_date   BETWEEN repl_start and repl_end
                            ) t2 join ( select FIRST_value(salary) over (PARTITION BY transfer_id ORDER BY change_date DESC) as salary, FIRST_VALUE(comb_addition) OVER  (PARTITION BY transfer_id ORDER BY change_date DESC) as comb_addition,
                                            transfer_id, ROW_NUMBER() OVER( PARTITION BY transfer_id ORDER BY change_date DESC) as rn from  {0}.ACCOUNT_DATA) ad
                                            on (ad.transfer_id=t2.transfer_id) 
                        where rn=1
                       ) t on (t.staffs_id=s.staffs_id)
     group by rollup ((tarif.val,code_pos,pos_name,s.classific, 
                    case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end, 
                    case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end, 
                    t.comb_addition, S.ADD_EXP_AREA,s.harmful_addition,code_tariff_grid))        
order by  mod(to_number(substr(code_pos,1,1))+4,3),nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0) desc )
    