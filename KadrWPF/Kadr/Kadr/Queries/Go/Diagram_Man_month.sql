select count(*) from 
        (select 
                    DISTINCT t.per_num
        from ( select transfer_now,NVL(actual_begin,plan_begin) plan_begin
	     from {0}.VS_CURRENT_ALL ) vs 
	     join {0}.transfer t on (t.transfer_id=vs.transfer_now)
	     left join {0}.degree d on (t.degree_id=d.degree_id)
         where t.sign_cur_work=1 and  t.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={3} connect by prior subdiv_id=parent_id) 
	 and  (    plan_begin >='{1}' and  plan_begin <'{2}')
         {4}
         )