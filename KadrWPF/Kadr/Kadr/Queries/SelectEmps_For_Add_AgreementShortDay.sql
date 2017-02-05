declare
begin
    open :c for
        with SUBD as 
            ( select SUBDIV_ID FROM apstaff.SUBDIV
                where subdiv_id = nvl(:p_subdiv_id, subdiv_id)
                start with subdiv_id in ( select subdiv_id from apstaff.access_subdiv where upper(user_name) = ora_login_user and upper(app_name) = 'KADR')
                connect by prior subdiv_id = parent_id)
        select
            per_num, emp_last_name, emp_first_name, emp_middle_name, 
            code_subdiv, subdiv_name,
            pos_name, pos_note,
            tariff, salary, sum_salary,
            harmful_addition_add,
            comb_addition, sum_comb_addition,
            secret_addition, sum_secret_addition,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).REGION as HAB_REGION,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).district as HAB_DISTRICT,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).city as HAB_CITY,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).locality as HAB_LOCALITY,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).street as HAB_STREET,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).house as HAB_HOUSE,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).BULK as HAB_BULK,
            TREAT(addr_type as APSTAFF.EMP_FULL_ADDRESS_TYPE).FLAT as HAB_FLAT,
            date_transfer,
            form_pay, WORKER_ID
        from
        (
            select 
                PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME,
                (select S.CODE_SUBDIV from apstaff.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
                (select S.SUBDIV_NAME from apstaff.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) SUBDIV_NAME,
                (select P.POS_NAME from apstaff.POSITION P WHERE P.POS_ID = T.POS_ID) POS_NAME,
                POS_NOTE,
                TARIFF, AD.SALARY, 
                NVL2(TARIFF_GRID_ID, tar_month, ROUND(AD.SALARY*B.TARIFF,0)) SUM_SALARY,
                AD.HARMFUL_ADDITION_ADD, 
                AD.COMB_ADDITION, ROUND(AD.COMB_ADDITION*B.TARIFF,0) SUM_COMB_ADDITION, 
                AD.SECRET_ADDITION, ROUND(AD.SECRET_ADDITION*B.TARIFF,0) SUM_SECRET_ADDITION,
                APSTAFF.EMP_FULL_ADDRESS_TYPE(per_num) as addr_type,
                B.BDATE DATE_TRANSFER,
                form_pay, WORKER_ID
            from apstaff.TRANSFER T
            join apstaff.ACCOUNT_DATA AD using (TRANSFER_ID)
            join apstaff.EMP E using (PER_NUM)
            join apstaff.PASSPORT P using (PER_NUM)
            join apstaff.HABIT H using(PER_NUM)
            join (SELECT BT.TARIFF, BT.BDATE, LEAD(BT.BDATE-1/86400,1,DATE '3000-01-01') OVER(ORDER BY BT.BDATE) EDATE 
                        FROM apstaff.BASE_TARIFF BT) B ON (sysdate  between B.BDATE AND B.EDATE)
            left join (select tariff_grid_id, tar_classif as classific, tar_month  from apstaff.TARIFF_GRID_SALARY TG where sysdate between tar_date and tariff_end_date and tar_classif>0) using (tariff_grid_id, classific)
            where T.SIGN_CUR_WORK = 1 and 
                T.SUBDIV_ID in (SELECT SUBDIV_ID FROM SUBD)
        )
        ORDER BY EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME;
end;
