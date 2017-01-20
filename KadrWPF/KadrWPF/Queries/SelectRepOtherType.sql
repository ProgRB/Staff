select rownum, {1} from 
    (select distinct per_num as pnm, {1} from 
        (		
        WITH ATT as (
            select distinct PER_NUM,
                LISTAGG(TO_CHAR(DATE_ATTEST,'DD.MM.YYYY'),';'||chr(10)) WITHIN GROUP(ORDER BY DATE_ATTEST) OVER(PARTITION BY PER_NUM) DATE_ATTEST,
                LISTAGG(SOLUTION,';'||chr(10)) WITHIN GROUP(ORDER BY DATE_ATTEST) OVER(PARTITION BY PER_NUM) SOLUTION 
            from {0}.ATTEST),
            RISE_Q as (
            select distinct PER_NUM, 
                LISTAGG(TYPE_RISE_QUAL_NAME,';'||chr(10)) WITHIN GROUP(ORDER BY RQ_DATE_START) OVER(PARTITION BY PER_NUM) TYPE_RISE_QUAL_NAME,
                LISTAGG(BASE_DOC_NAME,';'||chr(10)) WITHIN GROUP(ORDER BY RQ_DATE_START) OVER(PARTITION BY PER_NUM) BASE_DOC_NAME,
                LISTAGG(TO_CHAR(RQ_DATE_DOC,'DD.MM.YYYY'),';'||chr(10)) WITHIN GROUP(ORDER BY RQ_DATE_START) OVER(PARTITION BY PER_NUM) RQ_DATE_DOC,
                LISTAGG(RQ_THEME,';'||chr(10)) WITHIN GROUP(ORDER BY RQ_DATE_START) OVER(PARTITION BY PER_NUM) RQ_THEME
            from (
                select RQ.PER_NUM, RQ_DATE_START,
                    (SELECT TRQ.TYPE_RISE_QUAL_NAME FROM {0}.TYPE_RISE_QUAL TRQ 
                    WHERE TRQ.TYPE_RISE_QUAL_ID = RQ.TYPE_RISE_QUAL_ID) TYPE_RISE_QUAL_NAME, 
                    (SELECT BASE_DOC_NAME FROM {0}.BASE_DOC BD 
                    WHERE BD.BASE_DOC_ID = RQ.BASE_DOC_ID) BASE_DOC_NAME, RQ_DATE_DOC, RQ_THEME    
                from {0}.RISE_QUAL RQ))
		select em.per_num, em.emp_last_name||' '||substr(em.emp_first_name,1,1)||'.'||substr(em.emp_middle_name,1,1)||'.' as FIO, 
            emp_first_name, emp_last_name, emp_middle_name, SUB.CODE_SUBDIV,SUB.SUBDIV_NAME, 
            {0}.Rewar(em.per_num) as rew,
            pos.code_pos,pos.pos_name,tr.date_hire,tr.tr_num_order,tr.tr_date_order,            
            tr.date_end_contr,deg.code_degree,ad.classific,TTG.TYPE_TARIFF_GRID_NAME,
            ad.salary,ad.percent13,em.emp_birth_date,to_char(em.emp_birth_date, 'YYYY') as year_birth,em.emp_sex,ms.name_state,
            pd.INN,pd.ser_med_polus,pd.num_med_polus,pd.insurance_num,tpd.name_doc,pas.seria_passport,pas.num_passport,
            pas.when_given,pas.who_given,
            {0}.TypeEduName(em.per_num) as TE_NAME,
            {0}.Institut_name(em.per_num) as instit_name,
            {0}.Name_special(em.per_num) as name_spec,
            {0}.Qualif_name(em.per_num) as qual_name,
            {0}.Date_graduating(em.per_num) as year_graduating,
            {0}.Seria_diplom(em.per_num) as seria_diploma,
            {0}.Num_diplom(em.per_num) as num_diploma,
            cmm.comm_name,pas.country_birth,pas.region_birth,pas.city_birth,pas.distr_birth,pas.locality_birth,cit.name_city, 
            DIST.NAME_DISTRICT,LOC.LOCALITY_NAME,STR1.NAME_STREET,REG.REG_HOUSE,REG.REG_BULK,REG.REG_FLAT,REG.REG_POST_CODE,
            cit2.name_city as hab_city,DIST2.NAME_DISTRICT as hab_district,LOC2.LOCALITY_NAME hab_locality,
            STR2.NAME_STREET as hab_street,hab.hab_HOUSE,hab.hab_BULK,hab.hab_FLAT,hab.hab_post_code,reg.reg_phone,
            decode(TR.TYPE_TRANSFER_ID, 3, TR.DATE_TRANSFER, null) as date_trnsfr,
            (select RD.REASON_NAME from {0}.reason_dismiss rd where reason_id = tr.reason_id) as reason_dis, 
            tr.date_transfer as date_transfer, 
            /*({0}.AllStag(em.per_num)) as all_stag, */
			{0}.STANDING.STANDING_STRING(em.per_num, sysdate, null, null, 1) as All_Stag,
            {0}.STANDING.CALC_DAYS_STANDING(em.per_num, sysdate, null, null, 1) as All_Stag_Days,
			/*({0}.AllStag(em.per_num, 1)) as all_stag_plant, */
			{0}.STANDING.STANDING_STRING(em.per_num, sysdate, 1, null, 1) as all_stag_plant,
            {0}.STANDING.CALC_DAYS_STANDING(em.per_num, sysdate, 1, null, 1) as all_stag_plant_Days,
			/*({0}.contStag(em.per_num)) as workTime, */
			{0}.STANDING.STANDING_STRING(em.per_num, sysdate, null, null, 0) as workTime,
            {0}.STANDING.CALC_DAYS_STANDING(em.per_num, sysdate, null, null, 0) as workTime_Days,
            {0}.FamilyComp(em.per_num) as FamilyCom, 
			(select FO.CODE_FORM_OPERATION from {0}.FORM_OPERATION FO where FO.FORM_OPERATION_ID = TR.FORM_OPERATION_ID) CODE_FORM_OPERATION,
			NVL2(NPP.PER_NUM,'X','') SIGN_NPP, NPP.DATE_SETTING_PENS, 
			CASE WHEN AD.COMB_ADDITION > 0 THEN AD.COMB_ADDITION END COMB_ADDITION, 			
			CASE WHEN AD.HARMFUL_ADDITION > 0 THEN AD.HARMFUL_ADDITION END HARMFUL_ADDITION, 
			CASE WHEN AD.HARMFUL_ADDITION_ADD > 0 THEN AD.HARMFUL_ADDITION_ADD END HARMFUL_ADDITION_ADD,
			CASE WHEN TR.HARMFUL_VAC > 0 THEN TR.HARMFUL_VAC END HARMFUL_VAC,
            /*NVL2(NVL(TO_CHAR({0}.GET_ADD_RATE(TR.TRANSFER_ID, SYSDATE),'FM00'),
					nvl2(AD.PRIVILEGED_POSITION_ID,
						nvl((select decode(substr(PP1.KPS,1,1),1,'09',2,'06') from {0}.PRIVILEGED_POSITION PP1 
							where PP1.PRIVILEGED_POSITION_ID = AD.PRIVILEGED_POSITION_ID), null), null)),'X','') PR_SPLG*/
			nvl2(AD.PRIVILEGED_POSITION_ID,'X','') PR_SPLG,
			PP.SPECIAL_CONDITIONS, PP.KPS, PP.NUMBER_LIST,
            {0}.EDU_FROM_FACT(em.per_num) as FROM_FACT,
            (select DATE_ATTEST from ATT where PER_NUM = em.PER_NUM) DATE_ATTEST,
            (select SOLUTION from ATT where PER_NUM = em.PER_NUM) SOLUTION,
            (select TYPE_RISE_QUAL_NAME from RISE_Q where PER_NUM = em.PER_NUM) TYPE_RISE_QUAL_NAME,
            (select BASE_DOC_NAME from RISE_Q where PER_NUM = em.PER_NUM) BASE_DOC_NAME,
            (select RQ_DATE_DOC from RISE_Q where PER_NUM = em.PER_NUM) RQ_DATE_DOC,
            (select RQ_THEME from RISE_Q where PER_NUM = em.PER_NUM) RQ_THEME,
			InitCap(em.emp_last_name)||' '||substr(em.emp_first_name,1,1)||'.'||substr(em.emp_middle_name,1,1)||'.' as FIO_INIT_CAP, 
			{0}.STANDING.STANDING_FOR_PERIOD_STRING(em.PER_NUM,
				CASE WHEN NVL(TR.CHAR_TRANSFER_ID,0) not in (2,3,4)
					THEN CASE WHEN TR.TYPE_TRANSFER_ID = 1 
							THEN TR.DATE_HIRE
							ELSE (select TRUNC(MIN(T1.DATE_TRANSFER)) 
								FROM {0}.TRANSFER T1
								JOIN {0}.ACCOUNT_DATA AD1  on T1.TRANSFER_ID = AD1.TRANSFER_ID
								JOIN {0}.POSITION P1 on P1.POS_ID = T1.POS_ID 
								where T1.WORKER_ID = TR.WORKER_ID and P1.CODE_POS = pos.CODE_POS and NVL(AD1.CLASSIFIC,0) = NVL(AD.CLASSIFIC,0)
									and NVL(T1.CHAR_TRANSFER_ID,0) not in (2,3,4))
						END 
					ELSE CASE WHEN TR.CHAR_TRANSFER_ID = 2 and MONTHS_BETWEEN(DATE_END_CONTR, DATE_TRANSFER) > 11
                                THEN (select TRUNC(MIN(T1.DATE_TRANSFER)) 
                                        FROM {0}.TRANSFER T1
                                        JOIN {0}.ACCOUNT_DATA AD1  on T1.TRANSFER_ID = AD1.TRANSFER_ID
                                        JOIN {0}.POSITION P1 on P1.POS_ID = T1.POS_ID 
                                        where T1.WORKER_ID = TR.WORKER_ID and P1.CODE_POS = pos.CODE_POS and NVL(AD1.CLASSIFIC,0) = NVL(AD.CLASSIFIC,0)
                                            and NVL2(T1.CHAR_TRANSFER_ID,T1.CHAR_TRANSFER_ID,T1.CHAR_WORK_ID) in (2,1))
                            END       
				END, SYSDATE, '') AS STANDING_CODE_POS,
				APSTAFF.STAFF_PKG.GET_GROUP_MASTER(tr.transfer_id) GROUP_MASTER
        from {0}.emp em 
        join {0}.transfer tr on (em.per_num = tr.per_num) 
        join {0}.subdiv sub on (tr.subdiv_id = sub.subdiv_id) 
        join {0}.position pos on (pos.pos_id=tr.pos_id) 
        join {0}.degree deg on (deg.degree_id = tr.degree_id)         
        join {0}.passport pas on (pas.per_num=em.per_num)         
        join {0}.per_data pd on (pd.per_num = em.per_num) 
		left join {0}.ACCOUNT_DATA AD 
            on (AD.TRANSFER_ID = decode(TR.TYPE_TRANSFER_ID,3,TR.FROM_POSITION,TR.TRANSFER_ID)) 
		left join {0}.mar_state ms on (MS.MAR_STATE_ID=PAS.MAR_STATE_ID) 
        left join {0}.type_per_doc tpd on (pas.type_per_doc_id=TPD.TYPE_PER_DOC_ID ) 
        left join {0}.edu ed on (em.per_num = ed.per_num) 
        left join {0}.type_edu te on (ed.type_edu_id=te.type_edu_id) 
        left join {0}.instit ins on (ed.instit_id=ins.instit_id ) 
        left join {0}.speciality spec on (ed.spec_id=spec.spec_id)
        left join {0}.qual on (qual.qual_id=ed.qual_id) 
        left join {0}.mil_card mc on (em.per_num=mc.per_num) 
        left join {0}.comm cmm on (cmm.comm_id=mc.comm_id)  
        left join {0}.registr reg on (em.per_num=reg.per_num) 
        left join {0}.habit hab on (em.per_num=hab.per_num) 
        left join {0}.street str1 on (reg.reg_code_street=str1.code_street)
        left join {0}.abbrev abbr on (str1.abbrev_id=abbr.abbrev_id) 
        left join {0}.district dist on (dist.code_district = substr(reg.reg_code_street, 1, 5))
        left join {0}.region rgn on (rgn.code_region = substr(reg.reg_code_street, 1, 2)) 
        left join {0}.locality loc on (loc.code_locality = substr(reg.reg_code_street, 1, 11)) 
        left join {0}.city cit on (cit.code_city = substr(reg.reg_code_street, 1, 8))   
        left join {0}.street str2 on (hab.hab_code_street=str2.code_street)
        left join {0}.abbrev abbr2 on (str2.abbrev_id=abbr2.abbrev_id) 
        left join {0}.district dist2 on (dist2.code_district = substr(hab.hab_code_street, 1, 5))
        left join {0}.region rgn2 on (rgn2.code_region = substr(hab.hab_code_street, 1, 2)) 
        left join {0}.locality loc2 on (loc2.code_locality = substr(hab.hab_code_street, 1, 11)) 
        left join {0}.city cit2 on (cit2.code_city = substr(hab.hab_code_street, 1, 8)) 
        left join {0}.tariff_grid tg on (tg.tariff_grid_id=ad.tariff_grid_id) 
        left join {0}.type_tariff_grid ttg on (TTG.TYPE_TARIFF_GRID_ID=TG.TYPE_TARIFF_GRID_ID)
		left join {0}.NONSTATE_PENS_PROV NPP on (EM.PER_NUM = NPP.PER_NUM)
		left join {0}.PRIVILEGED_POSITION PP on (PP.PRIVILEGED_POSITION_ID = AD.PRIVILEGED_POSITION_ID)
        where {2} 
            (ed.year_graduating is null or ed.year_graduating=(select max(year_graduating) from {0}.edu where per_num = em.per_num))
            and TR.TRANSFER_ID in (SELECT TRANSFER_ID FROM {0}.PN_tmp WHERE user_name = :p_user_name))
    order by {1})
