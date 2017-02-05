select rownum,is_repl, "Код профессии","Профессия",
     "ФИО","Табельный",
     "Профессия реальн.",
     "Разряд", 
     "Тарифный коэфф. по схеме", 
     "Установленный кф.",     
     "Надбавка за совм", 
     "Надбавка за расш. обслуж.",
     "Надбавка за вредн.",
     "Тарифная сетка",    
      in_otpusk,
    need_transfer,
    staffs_id,transfer_id
from
(
	select 
		DECODE(t1.is_repl,1,'Кратк.',2,'Длит.' ,'') is_repl,
		p.code_pos "Код профессии",
		p.pos_name "Профессия",
		e.emp_last_name||' '||emp_first_name||' '||emp_middle_name "ФИО",nvl(e.per_num,'') "Табельный",
		p1.pos_name "Профессия реальн.",
		sf.classific "Разряд", 
		sf.tar_by_schema "Тарифный коэфф. по схеме", 
		ad.salary "Установленный кф.",  
		ad.comb_addition "Надбавка за совм", 
		sf.ADD_EXP_AREA "Надбавка за расш. обслуж.",
		sf.HARMFUL_ADDITION "Надбавка за вредн.",
		code_tariff_grid "Тарифная сетка",    
		DECODE(t_abs.per_num,null,null,1) in_otpusk,
		case when sf.date_end_staff<SYSDATE then 1 else 0 end need_transfer,
		sf.staffs_id,
		t.transfer_id
	from 
		{0}.staffs sf
		join {0}.position p  on (sf.pos_id=p.pos_id)   
		join {0}.subdiv sb on (sf.subdiv_id=sb.subdiv_id) 
		left join (select transfer_id,staffs_id, 0 as is_repl from {0}.transfer where sign_cur_work=1
					union all
					select re1.transfer_id,t2.staffs_id,1 as is_repl from {0}.repl_emp re1 join {0}.transfer t2 on (t2.transfer_id=re1.replacing_transfer_id) 
						where t2.sign_cur_work=1 and SYSDATE between  nvl(re1.repl_start,DATE '1000-01-01') and nvl(re1.repl_end,DATE '3000-01-01') and repl_actual_sign=1) t1 on (t1.staffs_id=sf.staffs_id)
		left join {0}.transfer t on (t.transfer_id=t1.transfer_id)
		left join (select * from (select ad1.*, row_number() over (partition by transfer_id order by CHANGE_DATE desc) as rn from  {0}.ACCOUNT_DATA ad1) where rn =1) ad on (t.transfer_id=ad.transfer_id)
		left join {0}.emp e on (e.per_num=t.per_num)
		left join {0}.position p1 on (t.pos_id=p1.pos_id)
		left join {0}.degree d on (sf.degree_id=d.degree_id)
		left join {0}.tariff_grid tg on(tg.tariff_grid_id=sf.tarif_grid_id)
		left join (select distinct rd.per_num from {0}.reg_doc rd join {0}.doc_list dl on (rd.doc_list_id=dl.doc_list_id) where SYSDATE between  doc_begin and doc_end and PAY_TYPE_ID in (226,227,236,237,238,501,502,532 ) ) t_abs on (t_abs.per_num=t.per_num)
	where sf.staff_sign=1 and sf.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
	 {1}          
	order by   mod(to_number(substr(p.code_pos,1,1))+4,3) asc,p.code_pos desc,sf.staffs_id,t1.is_repl , "ФИО"
)
 {2}