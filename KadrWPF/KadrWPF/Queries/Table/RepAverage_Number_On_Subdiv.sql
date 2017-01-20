with SUBD as (select * from {0}.SUBDIV S where S.SUBDIV_ID in (select COLUMN_VALUE from TABLE(:p_TABLE_ID)))
select DA.DEG_PRIORITY_GROUP_HIRE, 1 SIGN_ORDER, DA.DEGREE_ID,
    AVG_1,AVG_2, AVG_3, AVG_4, AVG_5, AVG_6, AVG_7, AVG_8, AVG_9, AVG_10, AVG_11, AVG_12 
from {0}.DEGREE_AVG_NUMBER DA 
left outer join (
    select DA.DEG_PRIORITY_GROUP_HIRE, 1 SIGN_ORDER,
        /*ROUND(SUM("1"),0) AVG_1,ROUND(SUM("2"),0) AVG_2,ROUND(SUM("3"),0) AVG_3,
		ROUND(SUM("4"),0) AVG_4,ROUND(SUM("5"),0) AVG_5,ROUND(SUM("6"),0) AVG_6,
        ROUND(SUM("7"),0) AVG_7,ROUND(SUM("8"),0) AVG_8,ROUND(SUM("9"),0) AVG_9,
		ROUND(SUM("10"),0) AVG_10,ROUND(SUM("11"),0) AVG_11,ROUND(SUM("12"),0) AVG_12*/
		ROUND(SUM("1"),2) AVG_1,ROUND(SUM("2"),2) AVG_2,ROUND(SUM("3"),2) AVG_3,
		ROUND(SUM("4"),2) AVG_4,ROUND(SUM("5"),2) AVG_5,ROUND(SUM("6"),2) AVG_6,
        ROUND(SUM("7"),2) AVG_7,ROUND(SUM("8"),2) AVG_8,ROUND(SUM("9"),2) AVG_9,
		ROUND(SUM("10"),2) AVG_10,ROUND(SUM("11"),2) AVG_11,ROUND(SUM("12"),2) AVG_12
    from {0}.DEGREE_AVG_NUMBER DA 
    join ( 
        select * from (
            select V1.DEGREE_ID, V1.CODE_F_O, V1.CODE_POS, 
                V1.N_MONTH,
                ROUND(sum(CASE when (V1.CODE_SUBDIV = '024' and V1.CODE_DEGREE != '04' and V1.PAY_TYPE_ID in ('540_','124All')) or 
                            (V1.CODE_SUBDIV = '024' and V1.CODE_DEGREE = '04' and V1.PAY_TYPE_ID in ('540_','222D','124All')) or 
                            (V1.CODE_SUBDIV != '024' and V1.PAY_TYPE_ID in ('540_','222D', '124All')) 
                        THEN round(TIME,0) ELSE null END) / COUNT_DAY,2) AVERAGE_NUMBER
            from (
                select ST.DEGREE_ID, 
                    (select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = ST.DEGREE_ID) CODE_DEGREE,
                    T.SUBDIV_ID, 
                    (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
                    ST.CODE_F_O, ST.CODE_POS, 
                    extract(MONTH from ST.START_PERIOD) N_MONTH, PAY_TYPE_ID, round(TIME,0) TIME,
                    EXTRACT(DAY FROM LAST_DAY(ST.START_PERIOD)) COUNT_DAY
                from {1}.SALARY_FROM_TABLE ST
                join {0}.TRANSFER T on T.TRANSFER_ID = ST.TRANSFER_ID
                where ST.APP_NAME_ID = 1 and T.SUBDIV_ID in (select SUBDIV_ID from SUBD)
                    and ST.START_PERIOD<=:p_end_date and ST.END_PERIOD>=:p_begin_date
                    and ST.SIGN_APPENDIX = 1 and ST.SIGN_COMB = 0 and PAY_TYPE_ID in ('540_','222D', '124All')
            ) V1
            group by V1.DEGREE_ID, V1.CODE_F_O, V1.CODE_POS, V1.N_MONTH, V1.COUNT_DAY
        ) V2
        PIVOT (SUM(AVERAGE_NUMBER) FOR N_MONTH in (1,2,3,4,5,6,7,8,9,10,11,12))
    ) V3 on V3.DEGREE_ID = DA.DEGREE_ID and V3.CODE_F_O in (DA.CODE_F_O1, DA.CODE_F_O2) and 
            V3.CODE_POS in (DA.CODE_POS1,DA.CODE_POS2,DA.CODE_POS3)
    group by DA.DEG_PRIORITY_GROUP_HIRE
) V4 on DA.DEG_PRIORITY_GROUP_HIRE = V4.DEG_PRIORITY_GROUP_HIRE
union all
select DA.DEG_PRIORITY_GROUP_HIRE, DA.SIGN_ORDER, DA.DEGREE_ID,
    AVG_1,AVG_2, AVG_3, AVG_4, AVG_5, AVG_6, AVG_7, AVG_8, AVG_9, AVG_10, AVG_11, AVG_12 
from 
    (
    select DA.*,L.SIGN_ORDER from {0}.DEGREE_AVG_NUMBER DA
    join (select level+1 SIGN_ORDER from dual connect by level < 3) L on 1=1
    ) DA 
left outer join (
    select DA.DEG_PRIORITY_GROUP_HIRE, SIGN_ORDER,
        SUM("1") AVG_1,SUM("2") AVG_2,SUM("3") AVG_3,SUM("4") AVG_4,SUM("5") AVG_5,SUM("6") AVG_6,
        SUM("7") AVG_7,SUM("8") AVG_8,SUM("9") AVG_9,SUM("10") AVG_10,SUM("11") AVG_11,SUM("12") AVG_12
    from {0}.DEGREE_AVG_NUMBER DA 
    join ( 
        select * from (
            with tableSalary as
            (
                select DISTINCT V1.DEGREE_ID, V1.CODE_F_O, V1.CODE_POS, 
                    V1.N_MONTH, PER_NUM
                from (
                    select ST.DEGREE_ID, ST.CODE_F_O, ST.CODE_POS, 
                        (select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = ST.DEGREE_ID) CODE_DEGREE,
                        T.PER_NUM, extract(MONTH from ST.START_PERIOD) N_MONTH,
                        (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV
                    from {1}.SALARY_FROM_TABLE ST
                    join {0}.TRANSFER T on T.TRANSFER_ID = ST.TRANSFER_ID
                    where ST.APP_NAME_ID = 1 and T.SUBDIV_ID in (select SUBDIV_ID from SUBD) 
                        and ST.END_PERIOD between :p_begin_date and :p_end_date
                        AND ST.END_PERIOD = LAST_DAY(ST.END_PERIOD)
                        and ST.SIGN_COMB = 0 and ST.SIGN_APPENDIX = 1 and ST.DEGREE_ID is not null
                ) V1 
            )
            select V2.DEGREE_ID, V2.CODE_F_O, V2.CODE_POS, N_MONTH,
                COUNT(DISTINCT V2.PER_NUM) COUNT_NUMBER,
                2 SIGN_ORDER
            from (
                select * from tableSalary
            ) V2
            GROUP BY V2.DEGREE_ID, V2.CODE_F_O, V2.CODE_POS, N_MONTH
            union all
            select V2.DEGREE_ID, V2.CODE_F_O, V2.CODE_POS, N_MONTH,
                COUNT(DISTINCT CASE WHEN (select E.EMP_SEX from {0}.EMP E where V2.PER_NUM = E.PER_NUM) = 'Æ' THEN V2.PER_NUM END) COUNT_WOMAN,
                3 SIGN_ORDER
            from (
                select * from tableSalary
            ) V2
            GROUP BY V2.DEGREE_ID, V2.CODE_F_O, V2.CODE_POS, N_MONTH
        ) V2
        PIVOT (SUM(COUNT_NUMBER) FOR N_MONTH in (1,2,3,4,5,6,7,8,9,10,11,12))
    ) V3 on V3.DEGREE_ID = DA.DEGREE_ID and V3.CODE_F_O in (DA.CODE_F_O1, DA.CODE_F_O2) and 
            V3.CODE_POS in (DA.CODE_POS1,DA.CODE_POS2,DA.CODE_POS3)
    group by DA.DEG_PRIORITY_GROUP_HIRE, SIGN_ORDER
) V4 on DA.DEG_PRIORITY_GROUP_HIRE = V4.DEG_PRIORITY_GROUP_HIRE and DA.SIGN_ORDER = V4.SIGN_ORDER
ORDER BY 1,2