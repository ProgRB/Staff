select sf.staffs_id,
	t.transfer_id,
	DECODE(t1.is_repl,1, 'Кратк.',2,'Длит.',' ') is_repl,		
 p.code_pos "Код",
emp_last_name||' '||emp_first_name ||' '||emp_middle_name "ФИО",
sf.classific "Разряд",
sf.tar_by_schema "Тарифный коэфф. по схеме",
a_d.salary "Установленный кф.",
code_degree "Категория",
sf.harmful_addition "Вредность",
a_d.comb_addition "Совм.",
sf.add_exp_area "За расш. обсл.",
order_name "Заказ", 
to_char(date_begin_staff,'DD.MM.YYYY') "Ввести с",
to_char(date_end_staff,'DD.MM.YYYY') "Исключить с",
	case vacant_sign 
		when 1 then 'Да'		
		else 'НЕТ' end	"Вакант.",
	date_end_vacant "Окончание вакансии",
	DECODE(type_person,0,'АУП',1, 'МОП', 'ПТП') 	"Персонал",
	comment_to_pos "Комментарий",	
	case 
         when date_end_staff<:p_cur_date  and staff_sign=1 then 1
         else 0 end need_transfer

from 
         {0}.staffs sf
         join {0}.position p  on (sf.pos_id=p.pos_id)   
         join {0}.subdiv on (sf.subdiv_id=subdiv.subdiv_id) 
        left join (
						select transfer_id,staffs_id,0 as is_repl from  {0}.transfer  where sign_cur_work=1 
						union all 
						( select re1.transfer_id,t1.staffs_id,1+nvl(sign_longtime,0) as is_repl  from {0}.transfer t1 join {0}.repl_emp re1 on (t1.transfer_id=re1.replacing_transfer_id) where sign_cur_work=1 and repl_actual_sign=1 and SYSDATE BETWEEN  nvl(re1.repl_start,DATE'1000-01-01') and nvl(re1.repl_end,DATE'3000-01-01'))
				  ) t1 on (t1.staffs_id=sf.staffs_id)
		left join {0}.transfer t on (t.transfer_id=t1.transfer_id)
        left join  (select * from ( select salary,comb_addition,transfer_id,row_number() over (partition by transfer_id order by change_date desc) rn from {0}.account_data ad) where rn=1)  
				  a_d on (a_d.transfer_id=t.transfer_id)  
        left join {0}.emp e on (e.per_num=t.per_num)
        left join {0}.position p1 on (t.pos_id=p1.pos_id)
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=sf.tarif_grid_id)
		left join {0}.orders o on (sf.order_id=o.order_id)
		left join {0}.degree on (sf.degree_id=degree.degree_id)
where  sf.pos_id=:p_pos_id 
	and nvl(sf.subdiv_id,0)=:p_subdiv_id and NVL(date_end_staff,DATE'3000-01-01') >:p_cur_date
	and nvl(date_begin_staff,DATE'1000-01-01')<=:p_cur_date  {1}
order by sf.staffs_id