declare 
cursor c is 
	select * from 
    (select {0}.VAC_SCHED_PACK.begin_vac_consist(vac_consist_id) vac_begin,
    		 {0}.VAC_SCHED_PACK.end_vac_consist(vac_consist_id) vac_end,
			 type_vac_id
        from {0}.vacation_schedule join {0}.vac_consist using (vac_sched_id) join {0}.type_vac using (type_vac_id)
        where transfer_id in (select transfer_id from {0}.transfer  where worker_id=(select worker_id from apstaff.transfer where transfer_id=:p_transfer_id))
			and  (actual_begin is not null and plan_sign=0 or actual_begin is null and plan_sign=1) and type_vac_id !=10 and vac_sched_id!=nvl(:p_vac_sched_id,-1)
		
	)
   where 
    vac_begin<=:p_vac_end and vac_end>=:p_vac_begin and 1=2;
begin
	for item in c
    loop
		null;
    	--raise_application_error(-20113,'На период с '||to_char(item.vac_begin,'DD.MM.YYYY')||' по '||to_char(item.vac_end,'DD.MM.YYYY')||' уже существует другой отпуск');
    end loop;
end;
