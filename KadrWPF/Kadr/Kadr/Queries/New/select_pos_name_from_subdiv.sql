   select position.pos_name, count(position.pos_name)
    from (select transfer.pos_id from {0}.transfer 
		where transfer.sign_cur_work>0 and 
				subdiv_id=(select subdiv_id 
					   from {0}.subdiv 
					   where  upper(SUBDIV.SUBDIV_NAME)=upper('{1}') and sub_actual_sign>0
					  )
	 ) tr_s join
        {0}.position on (tr_s.pos_id=position.pos_id)
        group by position.pos_name
        order by position.pos_name