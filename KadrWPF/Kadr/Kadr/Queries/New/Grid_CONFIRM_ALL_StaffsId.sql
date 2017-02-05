 select s.staffs_id          
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
                             else schema_tar_tb.tar*(1+s2.harmful_addition/100)  
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