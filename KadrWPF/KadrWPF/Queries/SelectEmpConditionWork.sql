with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
        connect by prior subdiv_id = parent_id)
SELECT CODE_SUBDIV, PER_NUM, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME, EMP_SEX, EMP_BIRTH_DATE, COMB, SiGN_COMB, CODE_DEGREE,
        CODE_POS, POS_NAME , TRANSFER_ID, FROM_POSITION,DATE_HIRE, WORKER_ID,
        TYPE_TRANSFER_ID, SUBCLASS_NUMBER
FROM (
    SELECT 
		(SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T5.SUBDIV_ID) CODE_SUBDIV, --S.CODE_SUBDIV, 
		E.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME,
        E.EMP_MIDDLE_NAME,E.EMP_SEX, E.EMP_BIRTH_DATE, 
        CASE T5.SIGN_COMB WHEN 1 THEN 'X' ELSE '' END AS COMB, T5.SiGN_COMB,
        (select CODE_DEGREE from {0}.DEGREE where DEGREE_ID = T5.DEGREE_ID) as CODE_DEGREE,
        /*P.CODE_POS, P.POS_NAME, */
        (SELECT P.CODE_POS FROM {0}.POSITION P WHERE P.POS_ID = T5.POS_ID) CODE_POS, 
        (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = T5.POS_ID) POS_NAME,
		T5.TRANSFER_ID, T5.WORKER_ID, T5.FROM_POSITION,T5.DATE_HIRE , 
        T5.TYPE_TRANSFER_ID, 
        (select CW.SUBCLASS_NUMBER from {0}.TRANSFER_COND_OF_WORK TC 
        join {0}.CONDITIONS_OF_WORK CW using(CONDITIONS_OF_WORK_ID)
        where TC.TRANSFER_ID = T5.TRANSFER_ID) SUBCLASS_NUMBER 
    FROM {0}.Emp E 
    JOIN (SELECT TRANSFER_ID, PER_NUM, SUBDIV_ID, POS_ID, TYPE_TRANSFER_ID, DATE_HIRE, FROM_POSITION, SIGN_COMB,
                DEGREE_ID, WORKER_ID
            FROM {0}.Transfer 
            WHERE SUBDIV_ID in (select subdiv_id from SUBD)
                and HIRE_SIGN = 1 
                and (SIGN_CUR_WORK = 1 or (TYPE_TRANSFER_ID = 3 and DATE_TRANSFER >= ADD_MONTHS(SYSDATE, -12)))) T5 
        ON E.PER_NUM = T5.PER_NUM
    /*JOIN {0}.Subdiv S ON T5.SUBDIV_ID = S.SUBDIV_ID 
    JOIN {0}.Position P ON T5.POS_ID = P.POS_ID*/
    join {0}.ACCOUNT_DATA AD on AD.TRANSFER_ID = DECODE(T5.TYPE_TRANSFER_ID,3,T5.FROM_POSITION,T5.TRANSFER_ID)
	--WHERE E.PER_NUM = '12714'
)
{1}
order by CODE_SUBDIV,PER_NUM,SiGN_COMB