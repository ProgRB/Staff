﻿select (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID) CODE_SUBDIV,
    V.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME,
    WORK_DATE, V.VTIME, DOC_NOTE,
    DENSE_RANK() OVER(PARTITION BY SUBDIV_ID, DOC_NOTE ORDER BY V.PER_NUM) RN,
    COUNT(DOC_NOTE) OVER(PARTITION BY SUBDIV_ID, V.PER_NUM/*, DOC_NOTE*/) COUNT_DOC_NOTE,
    regexp_replace(LISTAGG(DOC_NOTE,';') WITHIN GROUP(ORDER BY V.PER_NUM, DOC_NOTE) OVER(PARTITION BY V.PER_NUM),
        '([^;]+)(;\1)?+','\1') LIST_DOC_NOTE
from 
(
    select W.PER_NUM, W.TRANSFER_ID, WORK_DATE,
        ROUND(FROM_PERCO/3600,2) FROM_PERCO, ROUND(FROM_GRAPH/3600,2) FROM_GRAPH,
        ROUND(WP.VALID_TIME/3600,2) VTIME, D.DOC_NOTE
    from {0}.WORKED_DAY W
    join {0}.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID
    join {0}.REG_DOC R on WP.REG_DOC_ID = R.REG_DOC_ID
    join {0}.DOC_LIST D on R.DOC_LIST_ID = D.DOC_LIST_ID
    where W.WORK_DATE BETWEEN :p_BEGIN_PERIOD AND :p_END_PERIOD and ROUND(FROM_GRAPH/3600,2) > 
        ROUND(FROM_PERCO/3600,2) - 
            ROUND(nvl((select sum(WP3.VALID_TIME) from {0}.WORK_PAY_TYPE WP3 
                        join {0}.REG_DOC R1 on WP3.REG_DOC_ID = R1.REG_DOC_ID
                        join {0}.DOC_LIST D1 on R1.DOC_LIST_ID = D1.DOC_LIST_ID
                        where W.WORKED_DAY_ID = WP3.WORKED_DAY_ID and D1.DOC_TYPE = 2
                        group by WP3.WORKED_DAY_ID),0)/3600,2)
        and D.DOC_LIST_ID in (11, 63, 61)) V
join {0}.EMP E on V.PER_NUM = E.PER_NUM
join {0}.TRANSFER T on V.TRANSFER_ID = T.TRANSFER_ID
ORDER BY CODE_SUBDIV, PER_NUM, WORK_DATE