select TYPE_SUBDIV_NUM_GROUP, CODE_SUBDIV, AVERAGE_NUMBER 
from 
(
    select V.CODE_SUBDIV, SUBDIV_ID, 
        ROUND(sum(CASE when (V.CODE_SUBDIV = '024' and CODE_DEGREE != '04' and PAY_TYPE_ID in ('540_','124All')) or 
            (V.CODE_SUBDIV = '024' and CODE_DEGREE = '04' and PAY_TYPE_ID in ('540_','222D','124All')) or 
            (V.CODE_SUBDIV != '024' and PAY_TYPE_ID in ('540_','222D', '124All')) THEN round(TIME,0) ELSE null END) / :p_days,2) AVERAGE_NUMBER
    from (
        select 
            (select D.CODE_DEGREE from {0}.DEGREE D where D.DEGREE_ID = ST.DEGREE_ID) CODE_DEGREE,
            (select T.PER_NUM from {0}.TRANSFER T where T.TRANSFER_ID = ST.TRANSFER_ID) PER_NUM,
            (select (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T.SUBDIV_ID) 
            from {0}.TRANSFER T where T.TRANSFER_ID = ST.TRANSFER_ID) CODE_SUBDIV,
            (select T.SUBDIV_ID from {0}.TRANSFER T where T.TRANSFER_ID = ST.TRANSFER_ID) SUBDIV_ID,
            PAY_TYPE_ID, TIME
        from {1}.SALARY_FROM_TABLE ST
        join {0}.DEGREE_AVG_NUMBER DA 
            on ST.DEGREE_ID = DA.DEGREE_ID and ST.CODE_F_O in (DA.CODE_F_O1, DA.CODE_F_O2) 
                and ST.CODE_POS in (DA.CODE_POS1,DA.CODE_POS2,DA.CODE_POS3)
                and DA.SIGN_REP_ECON = 1 and DA.DEG_LEVEL = 1 AND DA.DEG_NUM_GROUP in (1,2)
        where ST.APP_NAME_ID = 1 and ST.START_PERIOD<=:p_end_date and ST.END_PERIOD>=:p_begin_date
            and ST.SIGN_APPENDIX = 1 and ST.SIGN_COMB = 0 and PAY_TYPE_ID in ('540_','222D', '124All')
    ) V
    GROUP BY V.CODE_SUBDIV, SUBDIV_ID
) V2
join (select S.SUBDIV_ID, T.TYPE_SUBDIV_NUM_GROUP 
        from {0}.SUBDIV S join {0}.TYPE_SUBDIV T using(TYPE_SUBDIV_ID) 
        where T.TYPE_SUBDIV_NUM_GROUP in (1,2)) S on (V2.SUBDIV_ID = S.SUBDIV_ID)
ORDER BY S.TYPE_SUBDIV_NUM_GROUP, CODE_SUBDIV