with SUBD as 
    (select SUBDIV_ID FROM APSTAFF.SUBDIV
        start with subdiv_id in (
            select subdiv_id from APSTAFF.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
select  PS.PROJECT_STATEMENT_ID, PS.PROJECT_TRANSFER_ID, PS.TRANSFER_ID TRANSFER_ID_PREV,
    T.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME,
    E.EMP_SEX, E.EMP_BIRTH_DATE, 
    T.SUBDIV_ID PREV_SUBDIV_ID,
    (SELECT S.CODE_SUBDIV FROM APSTAFF.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) PREV_CODE_SUBDIV,
    PS.TO_SUBDIV_ID, 
    (SELECT S.CODE_SUBDIV FROM APSTAFF.SUBDIV S WHERE S.SUBDIV_ID = PS.TO_SUBDIV_ID) CODE_SUBDIV,
    T.POS_ID PREV_POS_ID, (SELECT P.CODE_POS FROM APSTAFF.POSITION P WHERE P.POS_ID = T.POS_ID) PREV_CODE_POS,
    (SELECT P.POS_NAME FROM APSTAFF.POSITION P WHERE P.POS_ID = T.POS_ID) PREV_POS_NAME,
    T.POS_NOTE PREV_POS_NOTE, T.SIGN_COMB PREV_SIGN_COMB, DECODE(T.SIGN_COMB,1,'X') PREV_COMB, PS.BASE_DOC_ID, PS.PROJECT_PLAN_APPROVAL_ID,
    T.DEGREE_ID PREV_DEGREE_ID, T.FORM_PAY PREV_FORM_PAY, T.PROBA_PERIOD PREV_PROBA_PERIOD, T.DATE_TRANSFER PREV_DATE_TRANSFER, PS.TYPE_TRANSFER_ID,
    T.FORM_OPERATION_ID PREV_FORM_OPERATION_ID, AD.CLASSIFIC PREV_CLASSIFIC, AD.TARIFF_GRID_ID PREV_TARIFF_GRID_ID, AD.SALARY PREV_SALARY,
    NVL(SIGN_PRINT_MATCHING,0) SIGN_PRINT_MATCHING,
    CASE WHEN EXISTS(SELECT NULL FROM APSTAFF.PROJECT_STATEMENT_APPROVAL PA 
                    WHERE PA.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID) 
        THEN 1 ELSE 0 
    END as SIGN_EXIST_APPROVAL_STATEMENT,
    CASE WHEN EXISTS(SELECT NULL FROM APSTAFF.PROJECT_APPROVAL PA 
                    WHERE PA.PROJECT_TRANSFER_ID = PT.PROJECT_TRANSFER_ID) 
        THEN 1 ELSE 0 
    END as SIGN_EXIST_APPROVAL_PROJECT, 
    (SELECT CONTR_EMP FROM APSTAFF.TRANSFER T1 
    WHERE T1.WORKER_ID = T.WORKER_ID and T1.TYPE_TRANSFER_ID = 1) TR_NUM_ORDER_HIRE,
    (SELECT DATE_CONTR FROM APSTAFF.TRANSFER T1 
    WHERE T1.WORKER_ID = T.WORKER_ID and T1.TYPE_TRANSFER_ID = 1) TR_DATE_ORDER_HIRE,
    CASE WHEN EXISTS(SELECT NULL FROM APSTAFF.TRANSFER_QM TQM WHERE TQM.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID)
        THEN 1 ELSE 0 
    END as SIGN_TRANSFER_QM,
    ----- ���� PROJECT_TRANSFER ----
    CASE WHEN PT.TYPE_TRANSFER_ID != 1 or 
            (PT.TYPE_TRANSFER_ID = 1 and EXISTS(SELECT NULL FROM APSTAFF.TRANSFER T WHERE T.PER_NUM = PT.PER_NUM))
        THEN PT.PER_NUM 
    END    VISUAL_PER_NUM, E.PERCO_SYNC_ID,
    PT.SUBDIV_ID,
    PT.POS_ID, (SELECT P.CODE_POS FROM APSTAFF.POSITION P WHERE P.POS_ID = PT.POS_ID) CODE_POS,
    (SELECT P.POS_NAME FROM APSTAFF.POSITION P WHERE P.POS_ID = PT.POS_ID) POS_NAME,
    PT.POS_NOTE, PT.DATE_HIRE, PT.SIGN_COMB, DECODE(PT.SIGN_COMB,1,'X') COMB, 
    (SELECT TTR.TYPE_TRANSFER_NAME FROM APSTAFF.TYPE_TRANSFER TTR 
    WHERE TTR.TYPE_TRANSFER_ID = PT.TYPE_TRANSFER_ID) TYPE_TRANSFER_NAME,
    PT.DATE_TRANSFER, PT.DATE_END_CONTR, PT.TR_NUM_ORDER, PT.TR_DATE_ORDER, PT.FORM_PAY,
    (SELECT FP.NAME_FORM_PAY FROM APSTAFF.FORM_PAY FP WHERE FP.FORM_PAY = PT.FORM_PAY) NAME_FORM_PAY,
    PT.DEGREE_ID, (SELECT D.CODE_DEGREE FROM APSTAFF.DEGREE D WHERE D.DEGREE_ID = PT.DEGREE_ID) CODE_DEGREE,
    PT.FROM_POSITION, PT.CONTR_EMP, PT.DATE_CONTR, PT.DF_BOOK_ORDER, PT.SIGN_CUR_WORK, PT.PROBA_PERIOD,
    PT.SOURCE_ID, PT.FORM_OPERATION_ID, 
    (select SOURCE_EMPLOYABILITY_ID from APSTAFF.PER_DATA PD where PD.PER_NUM = E.PER_NUM) SOURCE_EMPLOYABILITY_ID,
    (SELECT FO.CODE_FORM_OPERATION FROM APSTAFF.FORM_OPERATION FO WHERE FO.FORM_OPERATION_ID = PT.FORM_OPERATION_ID) CODE_FORM_OPERATION,
    (SELECT FO.NAME_FORM_OPERATION FROM APSTAFF.FORM_OPERATION FO WHERE FO.FORM_OPERATION_ID = PT.FORM_OPERATION_ID) NAME_FORM_OPERATION,
    PT.HARMFUL_VAC, PT.ADDITIONAL_VAC, PT.WORKER_ID, PT.HARMFUL_ADDITION_ADD, PT.COMB_ADDITION, PT.COMB_ADDITION_NOTE, 
    PT.SALARY, PT.CLASSIFIC, PT.TARIFF_GRID_ID, 
    (SELECT CODE_TARIFF_GRID FROM APSTAFF.TARIFF_GRID TG WHERE TG.TARIFF_GRID_ID = PT.TARIFF_GRID_ID) CODE_TARIFF_GRID,
    PT.SECRET_ADDITION, PT.SECRET_ADDITION_NOTE, PT.SIGN_MAT_RESP_CONTR,
    PT.OTHER_LIABILITY_BOSS, PT.WORKING_TIME_ID, PT.WORKING_TIME_COMMENT, PT.CHAR_WORK_ID, PT.HIRE_SIGN, PT.CHAN_SIGN,
    PT.CHAR_TRANSFER_ID, PT.TAX_CODE, PT.DATE_SERVANT, PT.PROF_ADDITION, PT.PERCENT13, PT.SIGN_ADD, PT.DATE_ADD, 
    PT.COUNT_DEP14, PT.COUNT_DEP15, PT.COUNT_DEP16, PT.COUNT_DEP17, PT.COUNT_DEP18, PT.COUNT_DEP19, PT.COUNT_DEP20, PT.COUNT_DEP21,
    PT.CLASS_ADDITION, PT.CHIEF_ADDITION, PT.SUM_YOUNG_SPEC, PT.DATE_END_YOUNG_SPEC, PT.DATE_ADD_AGREE, PT.TRIP_ADDITION, 
    PT.SERVICE_ADD, PT.SALARY_MISSION, PT.WATER_PROC, PT.GOVSECRET_ADDITION, PT.PREMIUM_CALC_GROUP_ID,
    PT.ENCODING_ADDITION, PT.TRANSFER_ID, SIGN_RESUME, PT.GR_WORK_ID, 
    PT.REASON_ID, PT.BASE_DOC_NOTE,
    CASE WHEN EXISTS(SELECT NULL FROM APSTAFF.MAT_RESP_PERSON MRP WHERE MRP.PER_NUM = PT.PER_NUM) 
        THEN 1 ELSE 0 
    END as SIGN_MAT_RESP_PERSON,
    CASE WHEN PS.PROJECT_PLAN_APPROVAL_ID = (select MAX(PROJECT_PLAN_APPROVAL_ID) from APSTAFF.PROJECT_PLAN_APPROVAL
                                            where TYPE_TRANSFER_ID = PT.TYPE_TRANSFER_ID)
        THEN 1 ELSE 0 
    END as SIGN_FULL_APPROVAL,
    /* ���� ������������� � ������� �������� � ���������, �� ��������� �������������� */
    /* 04.04.2016 - ������ ������ ��������, ��� ��� ��� ���� ������� ��� ������� ������ ��� ������������ � ������ �������������.
        ������, ������ ��������� ������������ ������ � ������������ ���������� ����� � ����� �������������
        CASE WHEN PT.SUBDIV_ID in (select SUBDIV_ID FROM SUBD) THEN 1 ELSE 0 END */
    1 SIGN_OPEN_EDIT,
    CASE WHEN PT.TRANSFER_ID is not null and 
            NOT EXISTS(SELECT NULL FROM APSTAFF.PROJECT_APPROVAL PA JOIN APSTAFF.TYPE_APPROVAL TA USING(TYPE_APPROVAL_ID)
                        WHERE PA.PROJECT_TRANSFER_ID = PS.PROJECT_TRANSFER_ID and TA.SIGN_REGISTRATION_PROJECT = 1)
        THEN 1 ELSE 0 
    END as SIGN_NO_REGISTRATION
from APSTAFF.PROJECT_STATEMENT PS
JOIN APSTAFF.TRANSFER T ON (T.TRANSFER_ID = PS.TRANSFER_ID) 
join APSTAFF.ACCOUNT_DATA AD on (T.TRANSFER_ID = AD.TRANSFER_ID)
join APSTAFF.EMP E on (T.PER_NUM = E.PER_NUM) 
LEFT JOIN APSTAFF.PROJECT_TRANSFER PT on (PS.PROJECT_TRANSFER_ID = PT.PROJECT_TRANSFER_ID)
/* �������� �����������, ������� ����������� � ������ ������������� � � ��� ��� ������ ������������ 
    � ���������� �������������, � ��� �� ���, ��� ������ �� �������������, ����� ���������� ������������*/
WHERE (((PS.TO_SUBDIV_ID in (select SUBDIV_ID FROM SUBD)
        or (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_ECON_SERVICE') and PT.FORM_OPERATION_ID = 9)
        or (exists(select null from user_role_privs where granted_role =  'STAFF_VIEW_QM')
            and (exists(select null from APSTAFF.TRANSFER_QM TQM 
                        where TQM.PROJECT_TRANSFER_ID = PT.PROJECT_TRANSFER_ID or TQM.TRANSFER_ID = T.TRANSFER_ID or TQM.PROJECT_STATEMENT_ID = PS.PROJECT_STATEMENT_ID))))
            AND PS.PROJECT_PLAN_APPROVAL_ID in 
                (SELECT PPA.PROJECT_PLAN_APPROVAL_ID FROM APSTAFF.PROJECT_PLAN_APPROVAL PPA
                WHERE PPA.SIGN_APPROVAL_OLD_SUBDIV = 0))
        or 
        (T.SUBDIV_ID in (select SUBDIV_ID FROM SUBD)
            and PS.PROJECT_PLAN_APPROVAL_ID in 
                (SELECT PPA.PROJECT_PLAN_APPROVAL_ID FROM APSTAFF.PROJECT_PLAN_APPROVAL PPA
                WHERE PPA.SIGN_APPROVAL_OLD_SUBDIV = 1)))
    and PS.PROJECT_STATEMENT_ID = NVL(:p_PROJECT_STATEMENT_ID,PS.PROJECT_STATEMENT_ID)  
    and (PT.TRANSFER_ID is null 
        or (PT.TRANSFER_ID is not null and 
            exists(select null from user_role_privs where GRANTED_ROLE = 'STAFF_ECON_DEPT')
            and not exists(SELECT NULL FROM APSTAFF.PROJECT_APPROVAL PA 
                        JOIN APSTAFF.TYPE_APPROVAL TA USING(TYPE_APPROVAL_ID)
                        WHERE PA.PROJECT_TRANSFER_ID = PS.PROJECT_TRANSFER_ID and TA.SIGN_REGISTRATION_PROJECT = 1)))
    /* �� ���� ���������� ��������� ������, ���������� ������ ������������ */
    and (:p_SIGN_NOTIFICATION = 0 or (:p_SIGN_NOTIFICATION = 1 and 
            PS.ROWID in (select CHARTOROWID(COLUMN_VALUE) from TABLE(:p_TABLE_ID))))
    and PS.PROJECT_PLAN_APPROVAL_ID > -1
    and NVL(PT.PROJECT_PLAN_APPROVAL_ID,0) > -1