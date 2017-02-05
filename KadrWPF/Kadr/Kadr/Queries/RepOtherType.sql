select {1} from
(
    select distinct emp.per_num,
        emp.emp_last_name||'.'||substr(emp.emp_first_name,1,1)||'.'||substr(emp.emp_middle_name,1,1) as FIO,
        emp.emp_last_name||' '||emp.emp_first_name||' '||emp.emp_middle_name as fullFIO,
        SUB.CODE_SUBDIV,
        SUB.SUBDIV_NAME,
        pos.code_pos,
        pos.pos_name,
        tr.date_hire,
        tr.tr_num_order,
        tr.tr_date_order,
        /*Data of transfer to other job*/
        /*Work time on the plant */
        tr.date_end_contr,
        deg.code_degree,
        ad.classific,
        ad.salary,
        ad.percent13,
        emp.emp_birth_date,
        to_char(emp.emp_birth_date, 'YYYY') as year_birth,
        emp.emp_sex,
        ms.name_state,
        pd.INN,
        pd.ser_med_polus,
        pd.num_med_polus,
        tpd.name_doc,
        pas.seria_passport,
        pas.num_passport,
        pas.when_given,
        pas.who_given,
        TE.TE_NAME,
        ins.instit_name,
        spec.name_spec,
        qual.qual_name,
        edu.year_graduating,
        edu.seria_diploma,
        edu.num_diploma,
        comm.comm_name,
        pas.country_birth,
        pas.region_birth,
        pas.city_birth,
        pas.distr_birth,
        pas.locality_birth,
        cit.name_city,
        DIST.NAME_DISTRICT,
        LOC.LOCALITY_NAME,
        STR1.NAME_STREET,
        REG.REG_HOUSE,
        REG.REG_BULK,
        REG.REG_FLAT,
        REG.REG_CODE_STREET,
        DIST2.NAME_DISTRICT,
        LOC2.LOCALITY_NAME,
        STR2.NAME_STREET,
        hab.hab_HOUSE,
        hab.hab_BULK,
        hab.hab_FLAT,
        hab.hab_CODE_STREET,
        reg.reg_phone,
        (case (TR.TYPE_TRANSFER_ID) when 3 then TR.DATE_TRANSFER else null end) as date_trnsfr,
        (select RD.REASON_NAME from {0}.reason_dismiss rd where reason_id = tr.reason_id)
    from {0}.emp 
    left join {0}.transfer tr on (emp.per_num = tr.per_num) 
    left join {0}.subdiv sub on (tr.subdiv_id = sub.subdiv_id) 
    left join {0}.position pos on (pos.pos_id=tr.pos_id) 
    left join {0}.degree deg on (deg.degree_id = tr.degree_id) 
    left join {0}.account_data ad on (tr.transfer_id=ad.transfer_id)
    left join {0}.passport pas on (pas.per_num=emp.per_num) 
    left join {0}.mar_state ms on (MS.MAR_STATE_ID=PAS.MAR_STATE_ID) 
    left join {0}.per_data pd on (pd.per_num = emp.per_num) 
    left join {0}.type_per_doc tpd on (pas.type_per_doc_id=TPD.TYPE_PER_DOC_ID ) 
    left join {0}.edu on (emp.per_num = edu.per_num) 
    left join {0}.type_edu te on (edu.type_edu_id=te.type_edu_id) 
    left join {0}.instit ins on (edu.instit_id=ins.instit_id ) 
    left join {0}.speciality spec on (edu.spec_id=spec.spec_id)
    left join {0}.qual on (qual.qual_id=edu.qual_id) 
    left join {0}.mil_card mc on (emp.per_num=mc.per_num) 
    left join {0}.comm on (comm.comm_id=mc.comm_id)
    left join {0}.registr reg on (emp.per_num=reg.per_num) 
    left join {0}.habit hab on (emp.per_num=hab.per_num) 
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
    where tr.date_transfer = (select max(date_transfer) from {0}.transfer tr2 where per_num = emp.per_num)) 
where per_num in ({2})