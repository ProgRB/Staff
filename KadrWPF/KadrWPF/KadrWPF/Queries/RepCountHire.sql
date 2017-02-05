select CS, DECODE(ALLEMP,0,null,ALLEMP) as ALLEMP, DECODE(MAIN_EMP,0,null,MAIN_EMP) as MAIN_EMP,
    DECODE(HELP_EMP,0,null,HELP_EMP) as HELP_EMP, DECODE(PUPIL_EMP11,0,null,PUPIL_EMP11) as PUPIL_EMP11,
    DECODE(PUPIL_EMP12,0,null,PUPIL_EMP12) as PUPIL_EMP12, DECODE(BOSS,0,null,BOSS) as BOSS,
    DECODE(SPEC,0,null,SPEC) as SPEC, DECODE(OTHER,0,null,OTHER) as OTHER, 
    DECODE(NONINDUST,0,null,NONINDUST) as NONINDUST,
    DECODE(TEMP_EMP,0,null,TEMP_EMP) as TEMP_EMP, DECODE(EMP_COMB,0,null,EMP_COMB) as EMP_COMB,
    DECODE(HIGHER_EDU,0,null,HIGHER_EDU) as HIGHER_EDU, DECODE(SEC_PROF_EDU,0,null,SEC_PROF_EDU) as SEC_PROF_EDU,
    DECODE(AVERAGE_EDU,0,null,AVERAGE_EDU) as AVERAGE_EDU, DECODE(PRIMARY_EDU,0,null,PRIMARY_EDU) as PRIMARY_EDU,
    DECODE(BEFORE_18,0,null,BEFORE_18) as BEFORE_18, DECODE(P18_30,0,null,P18_30) as P18_30,
    DECODE(P31_40,0,null,P31_40) as P31_40, DECODE(P41_50,0,null,P41_50) as P41_50,
    DECODE(P51_60,0,null,P51_60) as P51_60, DECODE(AFTER_60,0,null,AFTER_60) as AFTER_60,
    DECODE(TOWN,0,null,TOWN) as TOWN, DECODE(VILL,0,null,VILL) as VILL,
    DECODE(DISTR,0,null,DISTR) as DISTR, DECODE(RUS_ARMY,0,null,RUS_ARMY) as RUS_ARMY
from
(
    WITH SUBD as (select case (s1.CODE_SUBDIV)
                when '081' then '001'
                when '083' then '003'
                when '085' then '005'
                when '086' then '006'
                when '087' then '007'
                when '089' then '009'
                when '090' then '010'
                when '092' then '012'
                when '093' then '023'
                when '096' then '016'
                when '097' then '017'
                else s1.CODE_SUBDIV end CODE_SUBDIV, SUBDIV_ID, TYPE_SUBDIV_ID, 
                CASE WHEN TYPE_SUBDIV_ID = 4 THEN 4 ELSE 1 END TYPE_NUMBER,
                CASE WHEN TYPE_SUBDIV_ID in (2,3) THEN 2 ELSE 
                    CASE WHEN TYPE_SUBDIV_ID in (1,7) THEN 1 ELSE 3 END END TYPE_SUBDIV
            from {0}.SUBDIV S1 join {0}.TYPE_SUBDIV TS using(TYPE_SUBDIV_ID) 
            where S1.SUB_ACTUAL_SIGN = 1 and TYPE_SUBDIV_ID in (1,2,3,4,5,7)),
        TRANSFERS as (select T0.PER_NUM, E.EMP_BIRTH_DATE, T0.SIGN_COMB, D.CODE_DEGREE, T0.SUBDIV_ID, P.CODE_POS, CHAR_WORK_ID,
                T0.DATE_TRANSFER, PD.RETIRER_SIGN, R.SOURCE_FILL_ID, T0.SOURCE_ID
            from {0}.TRANSFER T0
            join {0}.EMP E on (T0.PER_NUM=E.PER_NUM)
            join {0}.REGISTR R on(T0.PER_NUM=R.PER_NUM)
            join {0}.PER_DATA PD on(T0.PER_NUM=PD.PER_NUM)
            join {0}.DEGREE D using(DEGREE_ID)
            join {0}.POSITION P using(POS_ID)
            where T0.TYPE_TRANSFER_ID = 1 and T0.HIRE_SIGN = 1 and 
				T0.DATE_TRANSFER between :p_date_begin and :p_date_end
				and (T0.SIGN_COMB = 0 or (T0.SIGN_COMB = 1 and not exists(select null from {0}.TRANSFER T1
                    where T1.PER_NUM = T0.PER_NUM and T1.SIGN_CUR_WORK = 1 and T1.SIGN_COMB = 0))))
    select 1 RN, 'Цеха' CS, null ALLEMP, null MAIN_EMP, null HELP_EMP, null PUPIL_EMP11, null PUPIL_EMP12, 
        null BOSS, null SPEC, null OTHER, null NONINDUST, null TEMP_EMP, null EMP_COMB, null HIGHER_EDU, 
        null SEC_PROF_EDU, null AVERAGE_EDU, null PRIMARY_EDU, null BEFORE_18, null P18_30, null P31_40, 
        null P41_50, null P51_60, null AFTER_60, null TOWN, null VILL, null DISTR, null RUS_ARMY 
    from dual
    union
    select 4 RN, 'Отделы' CS, null ALLEMP, null MAIN_EMP, null HELP_EMP, null PUPIL_EMP11, null PUPIL_EMP12, 
        null BOSS, null SPEC, null OTHER, null NONINDUST, null TEMP_EMP, null EMP_COMB, null HIGHER_EDU, 
        null SEC_PROF_EDU, null AVERAGE_EDU, null PRIMARY_EDU, null BEFORE_18, null P18_30, null P31_40, 
        null P41_50, null P51_60, null AFTER_60, null TOWN, null VILL, null DISTR, null RUS_ARMY 
    from dual
    union
    select 7 RN, 'Непром. группа' CS, null ALLEMP, null MAIN_EMP, null HELP_EMP, null PUPIL_EMP11, null PUPIL_EMP12, 
        null BOSS, null SPEC, null OTHER, null NONINDUST, null TEMP_EMP, null EMP_COMB, null HIGHER_EDU, 
        null SEC_PROF_EDU, null AVERAGE_EDU, null PRIMARY_EDU, null BEFORE_18, null P18_30, null P31_40, 
        null P41_50, null P51_60, null AFTER_60, null TOWN, null VILL, null DISTR, null RUS_ARMY 
    from dual
    union
    select * from 
    (select DECODE(GROUPING(S.TYPE_NUMBER)+GROUPING(S.TYPE_SUBDIV)+GROUPING(S.CODE_SUBDIV), 
            0, CASE WHEN TYPE_SUBDIV = 2 THEN 2 ELSE CASE WHEN TYPE_SUBDIV = 1 THEN 5 ELSE 8 END END,
            1, CASE WHEN TYPE_SUBDIV = 2 THEN 3 ELSE CASE WHEN TYPE_SUBDIV = 1 THEN 5 ELSE 8 END END,
            2, CASE WHEN TYPE_NUMBER = 1 THEN 6 ELSE 9 END, 
            3, 10) RN, 
        DECODE(GROUPING(S.TYPE_NUMBER)+GROUPING(S.CODE_SUBDIV)+GROUPING(S.TYPE_SUBDIV),
                0,S.CODE_SUBDIV,
                1,'Итого',
                2,'Всего',
                3,'Итого по заводу') CS, 
        /* 08.07.2015 По просьбе Бурковой Л.В. убираю совместителей из колонки Всего
		SUM(CASE WHEN T.SIGN_COMB is not null THEN 1 END) ALLEMP, */
		SUM(CASE WHEN T.SIGN_COMB=0 THEN 1 END) ALLEMP,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE in ('01','08') THEN 1 else 0 END) MAIN_EMP,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE in ('05','09','07','02') THEN 1 else 0 END) HELP_EMP,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '11' THEN 1 else 0 END) PUPIL_EMP11,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '12' THEN 1 else 0 END) PUPIL_EMP12,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '04' and substr(T.CODE_POS,1,1) = '2' THEN 1 else 0 END) BOSS,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '04' and substr(T.CODE_POS,1,1) = '3' THEN 1 else 0 END) SPEC,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '04' and substr(T.CODE_POS,1,1) = '4' THEN 1 else 0 END) OTHER,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CODE_DEGREE = '13' THEN 1 else 0 END) NONINDUST,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.CHAR_WORK_ID != 1 THEN 1 else 0 END) TEMP_EMP,
        SUM(DECODE(T.SIGN_COMB,1,1,0)) EMP_COMB,
        SUM(DECODE((select Min(TE.TE_PRIORITY) from {0}.EDU ED join {0}.TYPE_EDU TE using(TYPE_EDU_ID)
                    where ED.PER_NUM = T.PER_NUM and T.SIGN_COMB=0),1,1,0)) HIGHER_EDU,
        SUM(DECODE((select Min(TE.TE_PRIORITY) from {0}.EDU ED join {0}.TYPE_EDU TE using(TYPE_EDU_ID)
                    where ED.PER_NUM = T.PER_NUM and T.SIGN_COMB=0),2,1,0)) SEC_PROF_EDU,
        SUM(DECODE((select Min(TE.TE_PRIORITY) from {0}.EDU ED join {0}.TYPE_EDU TE using(TYPE_EDU_ID)
                    where ED.PER_NUM = T.PER_NUM and T.SIGN_COMB=0),3,1,0)) AVERAGE_EDU,
        SUM(DECODE((select Min(TE.TE_PRIORITY) from {0}.EDU ED join {0}.TYPE_EDU TE using(TYPE_EDU_ID)
                    where ED.PER_NUM = T.PER_NUM and T.SIGN_COMB=0),4,1,0)) PRIMARY_EDU,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12))<18 THEN 1 else 0 END) BEFORE_18,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12)) between 18 and 30 THEN 1 else 0 END) P18_30,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12)) between 31 and 40 THEN 1 else 0 END) P31_40,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12)) between 41 and 50 THEN 1 else 0 END) P41_50,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12)) between 51 and 60 THEN 1 else 0 END) P51_60,
        SUM(CASE WHEN T.SIGN_COMB=0 and 
                (trunc(MONTHS_BETWEEN(T.date_transfer, T.emp_birth_date)/12))>60 THEN 1 else 0 END) AFTER_60,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.SOURCE_FILL_ID = 2 THEN 1 else 0 END) TOWN,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.SOURCE_FILL_ID = 1 THEN 1 else 0 END) VILL,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.SOURCE_FILL_ID = 3 THEN 1 else 0 END) DISTR,
        SUM(CASE WHEN T.SIGN_COMB=0 and T.SOURCE_ID = 2 THEN 1 else 0 END) RUS_ARMY 
    from (select * from SUBD) S  
    left join (select * from TRANSFERS) T using(SUBDIV_ID)
    group by rollup(S.TYPE_NUMBER, S.TYPE_SUBDIV, (to_number(CODE_SUBDIV),CODE_SUBDIV,TYPE_SUBDIV_ID)))
)