select 
	DECODE(t1.is_repl,1,'Кратк.',2,'Длит.' ,'') is_repl,
	 sb.subdiv_name "Подразделение",  
	 p.code_pos "Код профессии",  
	 emp_last_name||' '||emp_first_name ||' '||emp_middle_name "ФИО",
	 t.per_num,  
	 sf.classific "Разряд",  
	 sf.tar_by_schema "Тарифный коэфф. по схеме",  
	 ad.salary "Персональный коэфф.",  
	 d.code_degree "Категория",   
	 sf.harmful_addition "Вредность",  
	 ad.comb_addition "Совм.",  
	 sf.add_exp_area "За расш. обсл.",  
	 o.order_name "Заказ",  
	 gw.gr_work_name "График работы",  
	 sf.date_begin_staff "Ввести с",  
	 sf.date_end_staff "Исключить с", 
	 DECODE(sf.vacant_sign,1,'Да','НЕТ') "Вакант.",
	 sf.date_end_vacant "Окончание вакансии",  
	 DECODE(type_person,0,'АУП',1,'МОП','ПТП') "Персонал",  
	 sf.comment_to_pos "Комментарий",  
	 case                   
		when  date_end_staff<SYSDATE and staff_sign=1 then 1                  
		else 0 end need_transfer,
	sf.staffs_id,
	t.transfer_id		
from 
	 (select * from  {0}.staffs sf1 where :pcur_date between  nvl(date_begin_staff,DATE'1000-01-01') and nvl(date_end_staff,DATE'3000-01-01')-1 {1} and not exists(select * from {0}.staffs where temp_over_id=sf1.staffs_id) ) sf
	 left join {0}.position p  on (sf.pos_id=p.pos_id)              
	 left join {0}.subdiv sb on (sf.subdiv_id=sb.subdiv_id)            
	 left join (select transfer_id,staffs_id, 0 as is_repl from {0}.transfer where sign_cur_work=1
				union all
				select re1.transfer_id,t2.staffs_id,1 as is_repl from {0}.repl_emp re1 join {0}.transfer t2 on (t2.transfer_id=re1.replacing_transfer_id) 
					where t2.sign_cur_work=1 and :pcur_date between  nvl(re1.repl_start,DATE '1000-01-01') and nvl(re1.repl_end,DATE '3000-01-01') and repl_actual_sign=1
				) t1 on (t1.staffs_id=sf.staffs_id)             
	 left join {0}.degree d  on (sf.degree_id=d.degree_id)
	 left join {0}.transfer t on (t.transfer_id=t1.transfer_id)  
	 left join {0}.emp e on (t.per_num=e.per_num)     
	 left join {0}.orders o on (o.order_id=sf.order_id)   
	 left join {0}.gr_work gw on (t.gr_work_id=gw.gr_work_id)    
	 left join {0}.tariff_grid tg on(tg.tariff_grid_id=sf.tarif_grid_id)
	 left join (select * from (select ad1.*, row_number() over (partition by transfer_id order by CHANGE_DATE desc) as rn from  {0}.ACCOUNT_DATA ad1) where rn =1) ad on (t.transfer_id=ad.transfer_id)  
where 
	sf.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:psubdiv_id connect by prior subdiv_id=parent_id)
	and NVL(code_pos,'')=NVL(:pcode_pos,'')
	and DECODE(sf.classific,:pclassific,1,0)>0  
	and DECODE(case when nvl(ad.salary,0)<=sf.tar_by_schema then null else sf.tar_by_schema end,:ptar_by_schema,1,0)>0 
	and DECODE(case when nvl(ad.salary,0)<sf.tar_by_schema then sf.tar_by_schema else ad.salary end,:pstavka,1,0)>0  
	and DECODE(ad.comb_addition,:pcomb_addition,1,0)>0  
	and DECODE(sf.add_exp_area,:padd_exp_area,1,0)>0  
	and DECODE(sf.harmful_addition,:pharm_add,1,0)>0    
	and DECODE(code_tariff_grid,:ptar_grid,1,0)>0 
  order by sf.staffs_id     