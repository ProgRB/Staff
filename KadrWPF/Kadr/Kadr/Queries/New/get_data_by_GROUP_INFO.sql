select s.staffs_id,t.transfer_id,subdiv_name "Подразделение",code_pos "Код профессии",pos_name "Профессия",
s.classific "Разряд",
s.tar_by_schema "Тарифный коэфф.по схеме",
s.personal_tar "Персональный коэфф.",
degree_name "Категория", gr_work_name "График работы", date_begin_staff "Дата введения",
date_end_staff "Дата исключения",max_tariff "Максимальный тариф",
	case vacant_sign 
		when 1 then 'Вакантно'		
		else 'Не вакантно' end	"Вакантность",date_end_vacant "Дата окончания вакансии",harmful_addition "Надбавка за вредность",
	case type_person 
		when 0 then 'АУП'
		when 1 then 'МОП'
		else 'ПТП' end 	"Тип персонала",order_name "Заказ",comment_to_pos "Комментарий к профессии",
	emp_last_name "Фамилия",emp_first_name "Имя",emp_middle_name "Отчество",
	case 
                when (sign_cur_work=1  and date_end_staff<SYSDATE and staff_sign=1) then 1
                else 0 end need_transfer 
from ({11}       
                ) s1 
         join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         left join {0}.position  on (s.pos_id=position.pos_id)   
         left join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id) 
         left join {0}.transfer t on (t.staffs_id=s.staffs_id)  
         left join {0}.degree on (s.degree_id=degree.degree_id)
	 left join {0}.emp on (t.per_num=emp.per_num)
	 left join {0}.orders on (orders.order_id=s.order_id)
	 left join  {0}.gr_work on (t.gr_work_id=gr_work.gr_work_id) 
	 left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
         join  (
              select s2.staffs_id,
                    case 
                             when nvl(tbsal.salary,0)>schema_tar_tb.tar_schema  then schema_tar_tb.tar_schema
                             else null  
                            end kf_schema,
                   case 
                             when nvl(tbsal.salary,0)>schema_tar_tb.tar_schema  then  tbsal.salary
                             else schema_tar_tb.tar_schema*(1+nvl(s2.harmful_addition,0)/100)  
                            end kf_stavka,
                    tbsal.salary,
                    tbsal.degree_name                                                  
                from {0}.staffs s2 join (select staffs_id,case 
                                                                    when classific is null then s3.tar_by_schema
                                                                         else 
                                                                         (
                                                                                   select tar_sal   from {0}.descr_tariff_grid   
                                                                                    where tariff_grid_id=s3.tarif_grid_id and tar_classif=s3.classific and   
                                                                                     tar_date=(select max(tar_date) from {0}.descr_tariff_grid     where tariff_grid_id=s3.tarif_grid_id and tar_classif=s3.classific and tar_date<SYSDATE )                                         
                                                                         )  end tar_schema from {0}.staffs s3
                                           ) schema_tar_tb on (schema_tar_tb.staffs_id=s2.staffs_id)
                          left join (select salary,staffs_id,degree_name from {0}.TRANSFER t1 
                                                                                    left join  {0}.account_data ad on (ad.transfer_id=t1.transfer_id)
                                                                                    left join {0}.degree d on (t1.degree_id=d.degree_id)
                                         where sign_cur_work=1 and nvl(change_date,'01.01.1000')=(select nvl(max(ad.change_date),'01.01.1000')  from dual where ad.transfer_id=t1.transfer_id )
                                      ) tbsal on (s2.staffs_id=tbsal.staffs_id) 
           ) ptable on (ptable.staffs_id=s.staffs_id)   

where code_pos='{1}' and s.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={2} connect by prior subdiv_id=parent_id)
	and to_char(nvl(s.classific,'-1'))=to_number('{3}')
	and to_char(nvl(ptable.kf_schema,'-1'))=to_number('{4}')
	and to_char(nvl(ptable.kf_stavka,'-1'))=to_number('{5}')
	and to_char(nvl(s.comb_addition,'-1'))=to_number('{6}')
	and to_char(nvl(s.add_exp_area,'-1'))=to_number('{7}')
	and to_char(nvl(s.harmful_addition,'-1'))=to_number('{8}')  
	and (select 
                case
                when code_tariff_grid||tg.tariff_grid_name is null then '-1'
                else '('||code_tariff_grid||') '||tg.tariff_grid_name
                end   
		from dual)='{9}'
	and s1.fl={12}
	{10}


	