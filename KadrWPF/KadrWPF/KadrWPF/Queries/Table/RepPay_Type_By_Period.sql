SELECT CODE_SUBDIV, CODE_DEGREE, N_MONTH, N_YEAR, FIO, PER_NUM, POS_NAME, SUM(TIME) VTIME
FROM ( 
    WITH SUBD as 
    (
        select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id = :p_SUBDIV_ID
        connect by prior subdiv_id = parent_id
    )
    select S.CODE_SUBDIV, D.CODE_DEGREE, 
        EXTRACT(MONTH FROM START_PERIOD) N_MONTH, EXTRACT(YEAR FROM START_PERIOD) N_YEAR,
        E.EMP_LAST_NAME||' '||Substr(E.EMP_FIRST_NAME,1,1)||'.'||Substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
        SFT.PER_NUM, P.POS_NAME, TIME    
    from SALARY.SALARY_FROM_TABLE SFT
    join {0}.TRANSFER T on SFT.TRANSFER_ID = T.TRANSFER_ID
    join {0}.EMP E on T.PER_NUM = E.PER_NUM
    join {0}.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID
    join {0}.DEGREE D on T.DEGREE_ID = D.DEGREE_ID
    join {0}.POSITION P on T.POS_ID = P.POS_ID
    where PAY_TYPE_ID = :p_CODE_PAY_TYPE and START_PERIOD <= :p_endDate and END_PERIOD >= :p_beginDate
        and S.SUBDIV_ID in (SELECT SUBDIV_ID FROM SUBD)
)
GROUP BY CODE_SUBDIV, CODE_DEGREE, N_MONTH, N_YEAR, FIO, PER_NUM, POS_NAME