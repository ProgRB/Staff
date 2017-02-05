select code_pos "Код профессии",
	DECODE(GROUPING(pos_name),1, 'Всего АУП:'||sum(DECODE(type_person,0,1,0))||'; Всего ПТП:'||sum(DECODE(type_person,2,1,0)),pos_name) "Профессия",
	count(distinct staffs_id) "Количество единиц",  
    DECODE(GROUPING(classific),1,ROUND(AVG(classific),2),classific) "Разряд", 
    DECODE(GROUPING(CLASSIFIC),1,ROUND(AVG(case when code_tariff_grid is null and classific is null then  TAR_BY_SCHEMA else null end),2),ROUND(SUM(tar_schema),2),2)  "Тарифный коэфф. по схеме", 
    DECODE(grouping(stavka),1,SUM(stavka),stavka) "Тарифная ставка",
    case when classific is null then round(stavka*val) else stavka*val end  "рубл.",
    NULLIF(comb_addition,0) "Надбавка за совм", 
    NULLIF(comb_addition*val,0) "руб.",
    ADD_EXP_AREA "Надбавка за расш. обслуж.",
    add_exp_area* val "pубл.",
    SUM((nvl(stavka,0)+nvl(comb_addition,0))*(1+nvl(add_exp_area,0))*val) "Мес. фонд. ЗП",
    HARMFUL_ADDITION "Надбавка за вредн.",
    code_tariff_grid "Тарифная сетка",
	to_char(date_add_agree,'DD.MM.YYYY')
 from
  ( select t.transfer_id,sf.harmful_addition,tg.code_tariff_grid,sf.add_exp_area,p.code_pos,p.pos_name,sf.classific,sf.staffs_id,
  	t.date_add_agree, 
    t.comb_addition,
	case when NVL(t.salary,0)<sf.tar_by_schema then sf.tar_by_schema else t.salary end stavka,
	case when nvl(t.salary,0)<=sf.tar_by_schema then null else sf.tar_by_schema end tar_schema,
    tarif.val,
    tar_by_schema,
    SF.TYPE_PERSON
    FROM 
		(select tariff val from apstaff.base_tariff where bdate=(select max(bdate) from apstaff.base_tariff where bdate<SYSDATE)) tarif,
		(select staffs_id from apstaff.staffs  
		  where 
			staff_sign!=0 
			and degree_id = :p_degree_id                
			 and subdiv_id in 
						(select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)  and
				trunc(:p_cur_date)  between nvl(date_begin_staff,date'1000-01-01') and NVL(date_end_staff,date'3000-01-01')-1
			   MINUS
					select distinct temp_over_id from apstaff.staffs  where staff_sign!=0 and 
					trunc(:p_cur_date) between nvl(date_begin_staff,date'1000-01-01') and NVL(date_end_staff,date'3000-01-01')-1 
		) s1 
        join apstaff.staffs sf on (s1.staffs_id=sf.staffs_id)
        join apstaff.position p  on (sf.pos_id=p.pos_id)   
        left join apstaff.tariff_grid tg on(tg.tariff_grid_id=sf.tarif_grid_id)
        left  join (select t2.transfer_id,t2.staffs_id,ad.salary,ad.comb_addition,date_add_agree from 
						   ( select NVL(re1.transfer_id,t1.transfer_id) transfer_id,t1.staffs_id 
								from apstaff.transfer t1 
									left join (select * from apstaff.repl_emp where :p_cur_date BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1) re1 on (t1.transfer_id=re1.replacing_transfer_id)
                              where T1.SIGN_CUR_WORK=1
							) t2 join (select a1.*,ROW_NUMBER() over (PARTITION BY transfer_id order by CHANGE_DATE) rn from  apstaff.ACCOUNT_DATA a1) ad on (ad.transfer_id=t2.transfer_id) 
						 where rn=1
                       ) t on (t.staffs_id=sf.staffs_id)
   ) tba
   group by ROLLUP (( 
     val,code_pos,
     pos_name,
     classific, 
     stavka, 
     tar_schema, 
     comb_addition,
     ADD_EXP_AREA,
     harmful_addition,
     code_tariff_grid,
     date_add_agree))
order by  mod(to_number(substr(code_pos,1,1))+4,3),stavka desc 