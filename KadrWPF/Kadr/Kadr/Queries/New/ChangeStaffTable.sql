SELECT null,'������� �',null,null,null,null,null,null,null,null,null,null,null,null,null,null from dual

UNION all

select * from (select code_pos "��� ���������",pos_name "���������",
    to_char(date_begin_staff,'DD.MM.YYYY')||case when date_end_staff is null then null else '-'||to_char(date_end_staff,'DD.MM.YYYY') end ,count(s.staffs_id) "���������� ������",     
     classific "������", 
     case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end  "�������� �����. �� �����", 
     case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end "�������� ������",
     case when s.classific is null then round(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end*tarif.val) 
            else (case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end)*tarif.val end  "����.",
     t.comb_addition "�������� �� ����", 
     t.comb_addition*tarif.val "���.",
     S.ADD_EXP_AREA "�������� �� ����. ������.",
     s.add_exp_area* tarif.val "p���.",
     S.HARMFUL_ADDITION "�������� �� �����.",
     (nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0)*(1+nvl(s.add_exp_area,0))+nvl(t.comb_addition,0))*tarif.val "���. ����. ��",
        code_tariff_grid "�������� �����",
		null
              
         from 
            (select tariff val from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) tarif,         
         (select staffs_id from {0}.staffs  
              where 
                staff_sign!=0 
                and degree_id=(select degree_id from {0}.degree where upper(degree_name)=upper('{2}'))                
                and subdiv_id in   (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id)      
                            and date_begin_staff between to_date('{3}','DD.MM.YYYY') and to_date('{4}','DD.MM.YYYY')
        MINUS
                        select staffs_id from     {0}.staffs st5     
                        where temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=0 )                                           
                        and exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=-1)                         
                ) s1 
                join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         join {0}.position  on (s.pos_id=position.pos_id)   
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
         left  join (select staffs_id,salary,comb_addition  from 
                                                       ( select t1.transfer_id,t1.staffs_id from {0}.transfer t1 where sign_cur_work=1 and t1.staffs_id is not null and not exists( select transfer_id from {0}.repl_emp where staffs_id=t1.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1 )
                                                         union all
                                                         select transfer_id,staffs_id from {0}.repl_emp re where repl_emp_id=( select max(repl_emp_id)  from {0}.repl_emp where staffs_id=re.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1)
                                                        ) t2 join (select salary,comb_addition,transfer_id from  {0}.ACCOUNT_DATA a1 where NVL(Change_date,'01.01.1000')=(select max(NVL(change_date,'01.01.1000')) from {0}.account_data a2 where a2.transfer_id=a1.transfer_id)  ) ad
                                                            on (ad.transfer_id=t2.transfer_id) 
                       ) t on (t.staffs_id=s.staffs_id)
     group by tarif.val,code_pos,pos_name,s.classific, 
              case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end, 
              case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end, 
              t.comb_addition, S.ADD_EXP_AREA,s.harmful_addition,code_tariff_grid,tg.tariff_grid_name,date_begin_staff,date_end_staff                   
    order by date_begin_staff
    )
    
 UNION ALL

    select null,'�����:',null,
    count(s.staffs_id) "���������� ������",     
    round(AVG(classific),2) "������", 
     null schema ,
     sum ( case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end), 
     SUM(case when s.classific is null then round(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end*tarif.val) 
            else (case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end)*tarif.val end ) "����.",    
     sum(t.comb_addition) "�������� �� ����", 
     sum(t.comb_addition*tarif.val) "���.",
     sum(S.ADD_EXP_AREA) "�������� �� ����. ������.",
     sum(s.add_exp_area* tarif.val) "p���.",
     sum(S.HARMFUL_ADDITION) "�������� �� �����.",
     sum((nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0)*(1+nvl(s.add_exp_area,0))+nvl(t.comb_addition,0))*tarif.val) "���. ����. ��",
     null,null
  from 
            (select tariff val from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) tarif,         
         (select staffs_id from {0}.staffs  
              where 
                staff_sign!=0 
                and degree_id=(select degree_id from {0}.degree where upper(degree_name)=upper('{2}'))                
                and subdiv_id in 
                            (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id)      
                            and date_begin_staff between to_date('{3}','DD.MM.YYYY') and to_date('{4}','DD.MM.YYYY')
        MINUS
                        select staffs_id from     {0}.staffs st5     
                        where temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=0 )                                           
                        and exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=-1)                         
                ) s1 
                join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         join {0}.position  on (s.pos_id=position.pos_id)   
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
         left  join (select staffs_id,salary,comb_addition  from 
                                                       ( select t1.transfer_id,t1.staffs_id from {0}.transfer t1 where sign_cur_work=1 and t1.staffs_id is not null and not exists( select transfer_id from {0}.repl_emp where staffs_id=t1.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1 )
                                                         union all
                                                         select transfer_id,staffs_id from {0}.repl_emp re where repl_emp_id=( select max(repl_emp_id)  from {0}.repl_emp where staffs_id=re.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1)
                                                        ) t2 join (select salary,comb_addition,transfer_id from  {0}.ACCOUNT_DATA a1 where NVL(Change_date,'01.01.1000')=(select max(NVL(change_date,'01.01.1000')) from {0}.account_data a2 where a2.transfer_id=a1.transfer_id)  ) ad
                                                            on (ad.transfer_id=t2.transfer_id) 
                       ) t on (t.staffs_id=s.staffs_id)
                       
UNION ALL
                       
SELECT null,'��������� �',null,null,null,null,null,null,null,null,null,null,null,null,null,null from dual

UNION all
select * from (
     select p.code_pos "��� ���������", p.pos_name "���������",
     s1.date_end_staff||case when s1.fl_revers=1 then '-'||s1.date_begin_staff  else null end date_end ,    
     count(s.staffs_id) "���������� ������",      
     s.classific "������", 
     case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end  "�������� �����. �� �����", 
     case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end "�������� ������",
     case when s.classific is null then round(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end*tarif.val) 
            else (case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end)*tarif.val end  "����.",
     t.comb_addition "�������� �� ����", 
     t.comb_addition*tarif.val "���.",
     S.ADD_EXP_AREA "�������� �� ����. ������.",
     s.add_exp_area* tarif.val "p���.",
     S.HARMFUL_ADDITION "�������� �� �����.",
     (nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0)*(1+nvl(s.add_exp_area,0))+nvl(t.comb_addition,0))*tarif.val "���. ����. ��",
        code_tariff_grid "�������� �����",
		null
        from
            (select tariff val from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) tarif,
         (select staffs_id,date_begin_staff,date_end_staff,0 as fl_revers from {0}.staffs  
              where 
                staff_sign=2 
                and degree_id=(select degree_id from {0}.degree where degree_name='{2}')                
                and subdiv_id in 
                            (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id)      
                            and date_begin_staff between to_date('{3}','DD.MM.YYYY') and to_date('{4}','DD.MM.YYYY')
        MINUS
                     select st5.staffs_id,date_begin_staff,date_end_staff,0 from     {0}.staffs st5   /*��������� ��� �������� �� "����������*/  
                     where st5.temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=0 )                                           
                     and exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=-1)
        UNION 
             select st6.staffs_id,date_end_staff,date_begin_staff,1 from     {0}.staffs st6   /*��������� ��� �������� �� "����������*  � ������ �� ���� ������ � ���������*/  
                     where st6.temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st6.staffs_id and sign_modifi=0 )                                           
                     and exists(select  * from {0}.confirm_staffs where staffs_id=st6.staffs_id and sign_modifi=-1)
                ) s1 
         join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         join {0}.position p  on (s.pos_id=p.pos_id)   
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
         left  join (select staffs_id,salary,comb_addition  from 
                                                       ( select t1.transfer_id,t1.staffs_id from {0}.transfer t1 where sign_cur_work=1 and t1.staffs_id is not null and not exists( select transfer_id from {0}.repl_emp where staffs_id=t1.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1 )
                                                         union all
                                                         select transfer_id,staffs_id from {0}.repl_emp re where repl_emp_id=( select max(repl_emp_id)  from {0}.repl_emp where staffs_id=re.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1)
                                                        ) t2 join (select salary,comb_addition,transfer_id from  {0}.ACCOUNT_DATA a1 where NVL(Change_date,'01.01.1000')=(select max(NVL(change_date,'01.01.1000')) from {0}.account_data a2 where a2.transfer_id=a1.transfer_id)  ) ad
                                                            on (ad.transfer_id=t2.transfer_id) 
                       ) t on (t.staffs_id=s.staffs_id)
     group by tarif.val,p.code_pos,pos_name,s.classific, 
              case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end, 
              case when nvl(t.salary,0)<=s.tar_by_schema then null else s.tar_by_schema end, 
              t.comb_addition, S.ADD_EXP_AREA,s.harmful_addition,tg.code_tariff_grid,
              tg.tariff_grid_name,s1.date_begin_staff,s1.date_end_staff,s1.fl_revers                   
    order by  mod(to_number(substr(code_pos,1,1))+4,3),nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0) desc 
    )
  UNION ALL

    select null,'�����:',null,
    count(s.staffs_id) "���������� ������",     
     AVG(classific) "������", 
     null schema ,
     sum(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary  end) "stavka", 
     SUM(case when s.classific is null then round(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end*tarif.val) 
            else (case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end)*tarif.val end ) "����.",    
     sum(t.comb_addition) "�������� �� ����", 
     sum(t.comb_addition*tarif.val) "���.",
     sum(S.ADD_EXP_AREA) "�������� �� ����. ������.",
     sum(s.add_exp_area* tarif.val) "p���.",
     sum(S.HARMFUL_ADDITION) "�������� �� �����.",
     sum((nvl(case when nvl(t.salary,0)<s.tar_by_schema then s.tar_by_schema else t.salary end,0)*(1+nvl(s.add_exp_area,0))+nvl(t.comb_addition,0))*tarif.val) "���. ����. ��",
     null,null
     
        from
            (select tariff val from {0}.base_tariff where bdate=(select max(bdate) from {0}.base_tariff where bdate<SYSDATE)) tarif,
         (select staffs_id,date_begin_staff,date_end_staff,0 as fl_revers from {0}.staffs  
              where 
                staff_sign=2 
                and degree_id=(select degree_id from {0}.degree where degree_name='{2}')                
                and subdiv_id in 
                            (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id)      
                            and date_begin_staff between to_date('{3}','DD.MM.YYYY') and to_date('{4}','DD.MM.YYYY')
        MINUS
                     select st5.staffs_id,date_begin_staff,date_end_staff,0 from     {0}.staffs st5   /*��������� ��� �������� �� "����������*/  
                     where st5.temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=0 )                                           
                     and exists(select  * from {0}.confirm_staffs where staffs_id=st5.staffs_id and sign_modifi=-1)
        UNION 
             select st6.staffs_id,date_end_staff,date_begin_staff,1 from     {0}.staffs st6   /*��������� ��� �������� �� "����������*  � ������ �� ���� ������ � ���������*/  
                     where st6.temp_over_id is not null and not exists(select  * from {0}.confirm_staffs where staffs_id=st6.staffs_id and sign_modifi=0 )                                           
                     and exists(select  * from {0}.confirm_staffs where staffs_id=st6.staffs_id and sign_modifi=-1)
                ) s1 
         join {0}.staffs s on (s1.staffs_id=s.staffs_id)
         join {0}.position p  on (s.pos_id=p.pos_id)   
        left join {0}.tariff_grid tg on(tg.tariff_grid_id=s.tarif_grid_id)
         left  join (select staffs_id,salary,comb_addition  from 
                                                       ( select t1.transfer_id,t1.staffs_id from {0}.transfer t1 where sign_cur_work=1 and t1.staffs_id is not null and not exists( select transfer_id from {0}.repl_emp where staffs_id=t1.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1 )
                                                         union all
                                                         select transfer_id,staffs_id from {0}.repl_emp re where repl_emp_id=( select max(repl_emp_id)  from {0}.repl_emp where staffs_id=re.staffs_id and SYSDATE   BETWEEN repl_start and repl_end and repl_actual_sign=1 and  sign_longtime=1)
                                                        ) t2 join (select salary,comb_addition,transfer_id from  {0}.ACCOUNT_DATA a1 where NVL(Change_date,'01.01.1000')=(select max(NVL(change_date,'01.01.1000')) from {0}.account_data a2 where a2.transfer_id=a1.transfer_id)  ) ad
                                                            on (ad.transfer_id=t2.transfer_id) 
                       ) t on (t.staffs_id=s.staffs_id)