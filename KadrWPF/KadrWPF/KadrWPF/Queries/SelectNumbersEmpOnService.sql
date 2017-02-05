select CODE_SUBDIV,
    DECODE(KT01,0,null,KT01) KT01,DECODE(KT02,0,null,KT02) KT02,
    DECODE(KT04,0,null,KT04) KT04,DECODE(KT08,0,null,KT08) KT08,
    DECODE(KT09,0,null,KT09) KT09,DECODE(KTPr,0,null,KTPr) KTPr,
    DECODE(KTRuk,0,null,KTRuk) KTRuk,
    KT01 + KT02 + KT04 + KT08 + KT09 + KTPr + KTRuk KTITOG 
from (
    select DECODE(GROUPING(CODE_SUBDIV),0,LPAD(CODE_SUBDIV,7,' '),'  Итого') CODE_SUBDIV, 
        SUM(KT01) KT01,SUM(KT02) KT02, SUM(KT04) KT04,SUM(KT08) KT08,
        SUM(KT09) KT09,SUM(KTPr) KTPr, SUM(KTRuk) KTRuk
    from 
    (
        select CODE_SUBDIV, 
            DECODE(DEGREE_ID,1,1,0) as kt01,
            DECODE(DEGREE_ID,2,1,0) as kt02,
            CASE WHEN DEGREE_ID = 4 and SIGN_POS != '2' THEN 1 ELSE 0 END as kt04,
            DECODE(DEGREE_ID,8,1,0) as kt08,
            DECODE(DEGREE_ID,9,1,0) as kt09,
            CASE WHEN DEGREE_ID not in (1,2,4,8,9) THEN 1 ELSE 0 END as KTPr,
            CASE WHEN DEGREE_ID = 4 and SIGN_POS = '2' THEN 1 ELSE 0 END as KTRuk
        from
        (    
            with SUBD as 
                (select * from 
                    (select SUBDIV_ID, CODE_SUBDIV, SUBDIV_NAME, SUB_ACTUAL_SIGN,
                            NVL(SUB_DATE_START,DATE '1000-01-01') SUB_DATE_START,
                            NVL(SUB_DATE_END,DATE '3000-01-01') SUB_DATE_END 
                        from {0}.SUBDIV S 
                        join {0}.TYPE_SUBDIV TS using(TYPE_SUBDIV_ID)
                        where S.SERVICE_ID in (select COLUMN_VALUE from TABLE(:p_table_service)) 
							and TS.SIGN_SUBDIV_PLANT = 1 and S.PARENT_ID = 0
                    )
                where SUB_DATE_START<=:p_date_end AND SUB_DATE_END>=:p_date_begin)
            select distinct FIRST_VALUE(CODE_SUBDIV) OVER(PARTITION BY PER_NUM,CODE_SUBDIV ORDER BY DATE_TRANSFER DESC) CODE_SUBDIV, 
                FIRST_VALUE(PER_NUM) OVER(PARTITION BY PER_NUM,CODE_SUBDIV ORDER BY DATE_TRANSFER DESC) PER_NUM,
                FIRST_VALUE(DEGREE_ID) OVER(PARTITION BY PER_NUM,CODE_SUBDIV ORDER BY DATE_TRANSFER DESC) DEGREE_ID,
                FIRST_VALUE(SIGN_POS) OVER(PARTITION BY PER_NUM,CODE_SUBDIV ORDER BY DATE_TRANSFER DESC) SIGN_POS
            from 
            (
                select SU.CODE_SUBDIV, TR.SUBDIV_ID, TR.DEGREE_ID, TRANSFER_ID, PER_NUM, DATE_TRANSFER, END_TRANSFER, SUBSTR(P.CODE_POS,1,1) SIGN_POS
                from (select TRANSFER_ID, PER_NUM, SUBDIV_ID, POS_ID, DEGREE_ID, TRUNC(DATE_TRANSFER) DATE_TRANSFER, 
                        NVL((select DECODE(T1.TYPE_TRANSFER_ID,3,TRUNC(DATE_TRANSFER)+86399/86400,TRUNC(DATE_TRANSFER)-1/86400) 
                        from {0}.TRANSFER T1 where T1.FROM_POSITION = T.TRANSFER_ID),DATE '3000-01-01') END_TRANSFER
                    from {0}.TRANSFER T where T.HIRE_SIGN = 1 and T.TYPE_TRANSFER_ID != 3) TR
                join (select * from SUBD) SU on TR.SUBDIV_ID = SU.SUBDIV_ID
                join {0}.POSITION P on TR.POS_ID = P.POS_ID
                where TR.DATE_TRANSFER<=:p_date_end AND TR.END_TRANSFER>=:p_date_begin
            )
        )
    )
    group by rollup(CODE_SUBDIV)
)