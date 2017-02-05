select GROUPING(CODE_SUBDIV) GR1, 
    DECODE(GROUPING(CODE_SUBDIV),0,TO_CHAR(CODE_SUBDIV),1,'ИТОГО') CODE_SUBDIV, 
    SUM(S237) S237, SUM(s536) s536, SUM(s529) s529, SUM(s226) s226, SUM(s531D) s531D, 
    SUM(s215) s215, SUM(s533) s533, SUM(s124D) s124D, SUM(s124All) s124All, SUM(s102D) s102D, SUM(s222D) s222D, SUM(sAll) sAll, 
    SUM(ROUND(sAll/DAY_IN_MONTH,2)) AVERAGE_NUMBER, SUM(COUNT_NUMBER) COUNT_NUMBER, SUM(COUNT_WOMAN) COUNT_WOMAN
from (
    select CODE_SUBDIV, EXTRACT(DAY FROM LAST_DAY(ST.START_PERIOD)) DAY_IN_MONTH,
        sum(CASE when PAY_TYPE_ID = '237' THEN round(TIME,0) ELSE null END) as s237,
        sum(CASE when PAY_TYPE_ID = '536' THEN round(TIME,0) ELSE null END) as s536,
        sum(CASE when PAY_TYPE_ID in ('529','542D') THEN round(TIME,0) ELSE null END) as s529,
        sum(CASE when PAY_TYPE_ID = '226' THEN round(TIME,0) ELSE null END) as s226,
        sum(CASE when PAY_TYPE_ID in ('531D', '543D') THEN round(TIME,0) ELSE null END) as s531D,
        sum(CASE when PAY_TYPE_ID = '215' THEN round(TIME,0) ELSE null END) as s215,
        sum(CASE when PAY_TYPE_ID in ('533','539') THEN round(TIME,0) ELSE null END) as s533,
        /* Субботы, выходные и праздничные дни */
        sum(CASE when PAY_TYPE_ID = '124D' THEN round(TIME,0) ELSE null END) as s124D,
        /* Всего неявок */
        sum(CASE when PAY_TYPE_ID = '124All' THEN round(TIME,0) ELSE null END) as s124All,
        /* Дни работы всего */
        sum(CASE when (T.subdiv_id = 11 and CODE_DEGREE != '04' and PAY_TYPE_ID = '540_') or
            (T.subdiv_id = 11 and CODE_DEGREE = '04' and PAY_TYPE_ID in ('540_','222D')) or
            (T.subdiv_id != 11 and PAY_TYPE_ID in ('540_','222D')) THEN round(TIME,0) ELSE null END) as s102D,
        /* Дни работы в том числе командировок */
        sum(CASE when PAY_TYPE_ID = '222D' THEN round(TIME,0) ELSE null END) as s222D,
        /* Всего явок и неявок */
        sum(CASE when (T.subdiv_id = 11 and CODE_DEGREE != '04' and PAY_TYPE_ID in ('540_','124All')) or 
            (T.subdiv_id = 11 and CODE_DEGREE = '04' and PAY_TYPE_ID in ('540_','222D','124All')) or 
            (T.subdiv_id != 11 and PAY_TYPE_ID in ('540_','222D', '124All')) THEN round(TIME,0) ELSE null END) as sAll,
        COUNT(DISTINCT CASE WHEN ST.END_PERIOD = LAST_DAY(ST.END_PERIOD) THEN T.PER_NUM END) COUNT_NUMBER, 
        COUNT(DISTINCT CASE WHEN 
            ST.END_PERIOD = LAST_DAY(ST.END_PERIOD) and (select E.EMP_SEX from {0}.EMP E where T.PER_NUM = E.PER_NUM) = 'Ж' 
            THEN T.PER_NUM END) COUNT_WOMAN
    from {1}.SALARY_FROM_TABLE ST
    join {0}.DEGREE_AVG_NUMBER DA 
        on ST.DEGREE_ID = DA.DEGREE_ID and ST.CODE_F_O in (DA.CODE_F_O1, DA.CODE_F_O2) 
            and ST.CODE_POS in (DA.CODE_POS1,DA.CODE_POS2,DA.CODE_POS3)
			and DA.SIGN_REP_ECON = 1
    join {0}.TRANSFER T on (ST.TRANSFER_ID = T.TRANSFER_ID)
    join {0}.DEGREE D on (ST.DEGREE_ID = D.DEGREE_ID)
	join {0}.SUBDIV S on (S.SUBDIV_ID = T.SUBDIV_ID)
    WHERE ST.APP_NAME_ID = 1 and ST.START_PERIOD<=:p_end_date and ST.END_PERIOD>=:p_begin_date --and T.SUBDIV_ID = 44
        and ST.SIGN_APPENDIX = 1 and ST.SIGN_COMB = 0 
        and DA.DEG_PRIORITY in (SELECT COLUMN_VALUE FROM TABLE(:p_TABLE_DEG_PRIORITY))
    GROUP BY CODE_SUBDIV, EXTRACT(DAY FROM LAST_DAY(ST.START_PERIOD))
)
GROUP BY ROLLUP(CODE_SUBDIV)