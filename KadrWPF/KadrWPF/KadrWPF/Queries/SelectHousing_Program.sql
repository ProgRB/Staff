﻿select EMP_LAST_NAME||' '||EMP_FIRST_NAME||' '||EMP_MIDDLE_NAME FIO,
    E.PER_NUM, CODE_SUBDIV,
    EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME, TO_CHAR(EMP_BIRTH_DATE,'DD.MM.YYYY') EMP_BIRTH_DATE,
    TH.CONTR_EMP, TO_CHAR(TH.DATE_CONTR,'DD.MM.YYYY') DATE_CONTR, 
    TH.CONTR_EMP CONTR_EMP2, 
    '«'||TO_CHAR(TH.DATE_CONTR,'DD')||'» '||
        rtrim(replace(translate(to_char(TH.DATE_CONTR,'month','nls_date_language = RUSSIAN'),'ьй','яя'),'т ','та'))||' '||
        TO_CHAR(TH.DATE_CONTR,'YYYY') DATE_CONTR2,
    NVL((SELECT APSTAFF.GET_FULL_ADDRESS(R.REG_CODE_STREET)||', д.'||R.REG_HOUSE||
            NVL2(R.REG_BULK,', кор.'||R.REG_BULK,'')||NVL2(R.REG_FLAT,', кв.'||R.REG_FLAT,'')
        FROM APSTAFF.REGISTR R WHERE R.PER_NUM = T.PER_NUM),
        (SELECT APSTAFF.GET_FULL_ADDRESS(H.HAB_CODE_STREET)||', д.'||H.HAB_HOUSE||
            NVL2(H.HAB_BULK,', кор.'||H.HAB_BULK,'')||NVL2(H.HAB_FLAT,', кв.'||H.HAB_FLAT,'')
        FROM APSTAFF.HABIT H WHERE H.PER_NUM = T.PER_NUM)) ADDRESS,
    P.SERIA_PASSPORT, P.NUM_PASSPORT, TO_CHAR(P.WHEN_GIVEN,'DD.MM.YYYY') WHEN_GIVEN, WHO_GIVEN, 
    (SELECT R.REG_PHONE FROM APSTAFF.REGISTR R WHERE R.PER_NUM = T.PER_NUM) REG_PHONE, PD.INN
FROM APSTAFF.EMP E 
JOIN APSTAFF.PASSPORT P on E.PER_NUM = P.PER_NUM
JOIN APSTAFF.PER_DATA PD on E.PER_NUM = PD.PER_NUM
JOIN APSTAFF.TRANSFER T on E.PER_NUM = T.PER_NUM
JOIN APSTAFF.SUBDIV S on T.SUBDIV_ID = S.SUBDIV_ID
JOIN APSTAFF.TRANSFER TH on T.WORKER_ID = TH.WORKER_ID and TH.TYPE_TRANSFER_ID = 1
WHERE T.TRANSFER_ID = :p_TRANSFER_ID