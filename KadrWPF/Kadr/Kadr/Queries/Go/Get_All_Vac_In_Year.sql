with vs as
    ( select * from 
        {0}.VS_CURRENT_ALL vs 
        where type_vac_id!=3 and vs.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={3} connect by prior subdiv_id=parent_id)
        and plan_begin between to_Date('{1}','DD-MM-YYYY') and TO_DATE('{2}','DD-MM-YYYY')
    )
select 
        sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=1 then 1 else 0 end) "Январь" ,
         sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=2 then 1 else 0 end) "Февраль",
          sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=3 then 1 else 0 end) "Март",
           sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=4 then 1 else 0 end) "Апрель",
            sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=5 then 1 else 0 end) "Май",
             sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=6 then 1 else 0 end) "Июнь",
              sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=7 then 1 else 0 end) "Июль",
               sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=8 then 1 else 0 end) "Август",
                 sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=9 then 1 else 0 end) "Сентябрь",
                 sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=10 then 1 else 0 end) "Октябрь",
                  sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=11 then 1 else 0 end) "Ноябрь",
                   sum(case when EXTRACT(MONTH FROM vs1.plan_begin)=12 then 1 else 0 end) "Декабрь"
    from vs vs1 join {0}.transfer t on (t.transfer_id=vs1.transfer_now)  left join {0}.degree d on (d.degree_id=t.degree_id)
	{4}
	