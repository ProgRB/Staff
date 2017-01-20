begin 
	open :c1 for select 
			'True' as fl,
            vac_sched_id,
            vac_consist_id,
            rn,
            type_vac_id,
            name_vac,
            plan_begin,
            count_days,
            period_begin,
            period_end,
            new_begin,
            new_end,
            coalesce(error1, error2, error3) error_message,
			decode(period_begin, new_begin, 0, 1) er_begin,
			decode(period_end, new_end, 0, 1) er_end
         from TABLE(APSTAFF.VAC_SCHED_PACK.GET_RECALCED_PERIODS(:p_transfer_id))
         where vac_group_type_id = :p_vac_group_type_id
          order by rn;
end;