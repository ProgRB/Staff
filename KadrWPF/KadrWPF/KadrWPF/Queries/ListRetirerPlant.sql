select rownum as num, emp_last_name, emp_first_name, emp_middle_name, code_subdiv,
    per_num, date_start_priv, priv_num_doc, date_recalc, emp_birth_date, name_priv
from 
    (select em.emp_last_name, em.emp_first_name, em.emp_middle_name, em.emp_birth_date,
        (SELECT sub.code_subdiv FROM {0}.subdiv sub WHERE sub.subdiv_id = tr.subdiv_id) code_subdiv , 
        em.per_num, e_p.date_start_priv, e_p.priv_num_doc, e_p.date_recalc, TP.name_priv
    from (select * from {0}.per_data where retirer_sign = 1) pd
    join {0}.emp em on pd.per_num = em.per_num
    join {0}.transfer tr on em.per_num = tr.per_num
    join {0}.emp_priv e_p on em.per_num = e_p.per_num 
    join {0}.type_priv TP on (E_P.TYPE_PRIV_ID = TP.TYPE_PRIV_ID)
    where tr.sign_cur_work=1 and TR.SIGN_COMB = 0 and TP.SIGN_RETIRER = 1 and
        TRUNC(SYSDATE) BETWEEN NVL(E_P.DATE_START_PRIV,DATE '1000-01-01') and NVL(E_P.DATE_END_PRIV,DATE '3000-01-01')
    /* tr.date_transfer = (select max(date_transfer) from {0}.transfer tr2 where tr2.per_num = tr.per_num and tr.type_transfer_id !=3)*/
    /*or ( em.emp_sex = 'Ì' and (to_char (em.emp_birth_date,'YYYY')<=(to_char (sysdate, 'YYYY')-60)))  or (em.emp_sex = 'Æ' and (to_char(em.emp_birth_date,'YYYY')<=((to_char(sysdate,'YYYY'))-55)) ) ) */
    order by code_subdiv, emp_last_name)
WHERE CODE_SUBDIV = NVL(:p_CODE_SUBDIV, CODE_SUBDIV)