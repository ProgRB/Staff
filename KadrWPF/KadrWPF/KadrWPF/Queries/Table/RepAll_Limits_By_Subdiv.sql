﻿SELECT CODE_SUBDIV, DEGREE_ID,  
    (SELECT D.CODE_DEGREE FROM {0}.DEGREE D WHERE V.DEGREE_ID = D.DEGREE_ID) CODE_DEGREE,
    NUM_MONTH, NUM_YEAR, 
    SUM(PLAN_124) PLAN_124, SUM(PLAN_106) PLAN_106,
    SUM(PLAN_304) PLAN_304, SUM(PLAN_306) PLAN_306,
    SUM(FACT_124) FACT_124, SUM(FACT_106) FACT_106,
    SUM(FACT_304) FACT_304, SUM(FACT_306) FACT_306
FROM (
    select PAY_TYPE_ID, DEGREE_ID, CODE_SUBDIV, NUM_MONTH, NUM_YEAR, 
        DECODE(PAY_TYPE_ID,124, PLAN) PLAN_124, DECODE(PAY_TYPE_ID,106, PLAN) PLAN_106,
        DECODE(PAY_TYPE_ID,304, PLAN) PLAN_304, DECODE(PAY_TYPE_ID,306, PLAN) PLAN_306,
        DECODE(PAY_TYPE_ID,124, FACT) FACT_124, DECODE(PAY_TYPE_ID,106, FACT) FACT_106,
        DECODE(PAY_TYPE_ID,304, FACT) FACT_304, DECODE(PAY_TYPE_ID,306, FACT) FACT_306
    from (select PAY_TYPE_ID from {0}.PAY_TYPE
        where PAY_TYPE_ID in (124,106,304,306)) PT,  
        TABLE({0}.CALC_LIMIT(:p_subdiv_id, null, null, PT.PAY_TYPE_ID, null, :p_year, 0))
    /*where NUM_MONTH between :p_month1 and :p_month2 and 
            (:p_all_rows = 1 or (:p_all_rows = 0 and nvl(PLAN,0)+nvl(FACT,0)+nvl(FACT303,0) != 0))*/
) V
GROUP BY CODE_SUBDIV, DEGREE_ID, NUM_MONTH, NUM_YEAR