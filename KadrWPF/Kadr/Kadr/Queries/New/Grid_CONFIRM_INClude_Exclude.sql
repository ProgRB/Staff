 select
	s1.date_begin_staff "Ввести с",
	s1.date_end_st "по",
     
     code_pos "Код профессии",pos_name "Профессия",count(s.staffs_id) "Количество единиц",      
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
     end     "Тарифная сетка"
    
          
         from (select s4.staffs_id, date_begin_staff,
		case
		when cs.sign_modifi=-1 then cs.date_mod   
		else s4.date_end_staff end date_end_st 
					from {0}.staffs  s4 
					join {0}.confirm_staffs cs on (cs.staffs_id=s4.staffs_id)
              where 
			{2}
		and sign_confirm is null
                 and s4.subdiv_id in 
                            (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id)      
                ) s1 
         join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         join {0}.position  on (s.pos_id=position.pos_id)   
         join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id) 
          left join {0}.transfer t on (t.staffs_id=s.staffs_id)  
           left join {0}.degree on (s.degree_id=degree.degree_id)
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
           left join  (
                select s2.staffs_id,
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
                                                                                  
           ) ptable on (ptable.staffs_id=s.staffs_id)
       
     group by s1.date_begin_staff,s1.date_end_st,code_pos,pos_name,classific,ptable.kf_schema,ptable.kf_stavka, s.comb_addition, S.ADD_EXP_AREA,s.harmful_addition,code_tariff_grid,tg.tariff_grid_name
              
     order by  mod(to_number(substr(code_pos,1,1))+4,3) asc,nvl(ptable.kf_stavka,0) desc  