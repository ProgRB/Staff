WITH SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'ACCOUNT')
        connect by prior subdiv_id = parent_id)
SELECT CODE_SUBDIV, PER_NUM, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME, EMP_SEX, EMP_BIRTH_DATE, COMB, SiGN_COMB, CODE_DEGREE,
        CODE_POS, POS_NAME , POS_NOTE, TRANSFER_ID, FROM_POSITION,DATE_HIRE , SIGN_PROFUNION,RETIRER_SIGN, DATE_TRANSFER,
        TYPE_TRANSFER_ID, SIGN_LEAVE, WORKER_ID
FROM (
    SELECT (SELECT S.CODE_SUBDIV FROM {0}.Subdiv S WHERE T5.SUBDIV_ID = S.SUBDIV_ID) CODE_SUBDIV, 
		E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME,
        E.EMP_MIDDLE_NAME,E.EMP_SEX, E.EMP_BIRTH_DATE, CASE T5.SIGN_COMB WHEN 1 THEN 'X' ELSE '' END AS COMB, T5.SiGN_COMB,
        (select CODE_DEGREE from {0}.DEGREE where DEGREE_ID = T5.DEGREE_ID) as CODE_DEGREE,
        (SELECT P.CODE_POS FROM {0}.Position P WHERE T5.POS_ID = P.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM {0}.Position P WHERE T5.POS_ID = P.POS_ID) POS_NAME, 
		POS_NOTE,
        (SELECT PD.RETIRER_SIGN FROM {0}.per_data pd WHERE E.PER_NUM = PD.PER_NUM) RETIRER_SIGN,
		T5.TRANSFER_ID, T5.WORKER_ID, T5.FROM_POSITION,T5.DATE_HIRE , 
		/* 13.02.2015 PD.SIGN_PROFUNION,*/
		{0}.GET_SIGN_PROFUNION(T5.WORKER_ID, sysdate) SIGN_PROFUNION,
		DATE_TRANSFER, T5.TYPE_TRANSFER_ID, 		
        (SELECT NVL(AD.CLASSIFIC,0) FROM {0}.ACCOUNT_DATA AD 
        WHERE AD.TRANSFER_ID = DECODE(T5.TYPE_TRANSFER_ID,3,T5.FROM_POSITION,T5.TRANSFER_ID)) CLASSIFIC,
        case when 
            exists(select null from {0}.reg_doc re 
                    /*join {0}.doc_list doc on (RE.DOC_LIST_ID = DOC.DOC_LIST_ID) */
                    where DOC_LIST_ID = 21 /*DOC.DOC_NOTE = 'сд'*/ and RE.PER_NUM = E.PER_NUM and 
                        :p_date between RE.DOC_BEGIN and RE.DOC_END)
            then 1 else 0 end as SIGN_LEAVE   
    FROM {0}.Emp E 
    JOIN (SELECT TRANSFER_ID, PER_NUM, SUBDIV_ID, POS_ID, TYPE_TRANSFER_ID, DATE_HIRE, FROM_POSITION, SIGN_COMB,
                DEGREE_ID, WORKER_ID, DATE_TRANSFER, POS_NOTE
            FROM {0}.Transfer 
            WHERE SUBDIV_ID in (select subdiv_id from SUBD)
                and HIRE_SIGN = 1 
                and (SIGN_CUR_WORK = 1 or (TYPE_TRANSFER_ID = 3 and DATE_TRANSFER >= ADD_MONTHS(SYSDATE, -12)))) T5 
        ON E.PER_NUM = T5.PER_NUM
)
{1}
order by CODE_SUBDIV,PER_NUM,SiGN_COMB