select code_pos "Код профессии",pos_name "Профессия",count(s.staffs_id) "Количество единиц", 
     
     classific "Разряд", 
     ptable.kf_schema "Тарифный коэфф. по схеме", 
     ptable.kf_stavka "Тарифная ставка",
     case 
        when S.CLASSIFIC is null then to_char( round(ptable.kf_stavka*(select tariff from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)))) 
        else  (trim(to_char(ptable.kf_stavka*(select tariff from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)),'9999999990D99'))) end "рубл.",
     s.comb_addition "Надбавка за совм", 
     s.comb_addition*(select tariff from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff  where bdate<SYSDATE)) "руб.",
     S.ADD_EXP_AREA "Надбавка за расш. обслуж.",
     s.add_exp_area* (select tariff from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) "pубл.",
     (nvl(ptable.kf_stavka,0)*(1+nvl(s.add_exp_area,0))+nvl(s.comb_addition,0))*(select tariff from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) "Мес. фонд. ЗП",
     S.HARMFUL_ADDITION "Надбавка за вредн.",
     case 
            when code_tariff_grid||tg.tariff_grid_name is not null then  '('||code_tariff_grid||') '||tg.tariff_grid_name
            else null
     end     "Тарифная сетка",
     to_char(ptable.date_end,'DD-MM-YYYY')
          
         from 
         {0}.staffs s 
         join {0}.position  on (s.pos_id=position.pos_id)   
         join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id) 
          left join {0}.transfer t on (t.staffs_id=s.staffs_id)  
           left join {0}.degree on (s.degree_id=degree.degree_id)
	    left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
           left join  (
                select s2.staffs_id,
		     case 
			  when cs.date_mod is null then s2.date_end_staff
			  else cs.date_mod end date_end,
                     case 
                          when s2.personal_tar is not null then schema_tar_tb.tar
                          else null  
                          end kf_schema,
                     case 
                          when s2.personal_tar is not null then  s2.personal_tar
                          else schema_tar_tb.tar*(1+nvl(s2.harmful_addition,0)/100)  
                          end kf_stavka                      
                from {0}.staffs s2 join (select staffs_id,case 
                                                                    when classific is null then s3.tar_by_schema
                                                                         else 
                                                                         (
                                                                                   select tar_sal   from {0}.descr_tariff_grid   
                                                                                    where tariff_grid_id=s3.tarif_grid_id and tar_classif=s3.classific and   
                                                                                     tar_date=(select max(tar_date) from {0}.descr_tariff_grid     where tariff_grid_id=s3.tarif_grid_id and tar_classif=s3.classific and tar_date<SYSDATE )                                         
                                                                         )  end tar from {0}.staffs s3
                                           ) schema_tar_tb on (schema_tar_tb.staffs_id=s2.staffs_id)
			left join (select distinct staffs_id,date_mod from {0}.confirm_staffs where  nvl(sign_modifi,-1)=-1 and sign_confirm=-1)cs on (cs.staffs_id=s2.staffs_id)
                                                                                  
           ) ptable on (ptable.staffs_id=s.staffs_id)
	where s.staffs_id in ({1})
	   
     group by ptable.date_end, code_pos,pos_name,classific,ptable.kf_schema,ptable.kf_stavka, s.comb_addition, S.ADD_EXP_AREA,s.harmful_addition,code_tariff_grid,tg.tariff_grid_name
              
     order by  mod(to_number(substr(code_pos,1,1))+4,3) asc,nvl(ptable.kf_stavka,0) desc 
     