select rownum as num, emp_last_name, emp_first_name, emp_middle_name, code_subdiv,
    per_num, date_start_priv, priv_num_doc, date_recalc, emp_birth_date, name_priv
from 
    (select em.emp_last_name, em.emp_first_name, em.emp_middle_name, em.emp_birth_date,
        (SELECT sub.code_subdiv FROM {0}.subdiv sub WHERE sub.subdiv_id = tr.subdiv_id) code_subdiv , 
        em.per_num, null date_start_priv, null priv_num_doc, null date_recalc, null name_priv,
        pd.retirer_sign
    from {0}.per_data pd
    join {0}.emp em on pd.per_num = em.per_num
    join {0}.transfer tr on em.per_num = tr.per_num
    where tr.sign_cur_work=1 and TR.SIGN_COMB = 0 and
        ((em.emp_sex = 'Ì' and em.emp_birth_date<=ADD_MONTHS(TRUNC(sysdate),-720)) or  
            (em.emp_sex = 'Æ' and em.emp_birth_date<=ADD_MONTHS(TRUNC(sysdate),-660)))
        and (pd.retirer_sign = 0 or
            not exists(select null from {0}.EMP_PRIV EP join {0}.TYPE_PRIV TP using(TYPE_PRIV_ID)
                        where EP.PER_NUM = PD.PER_NUM and TP.SIGN_RETIRER = 1 and 
                            TRUNC(SYSDATE) BETWEEN NVL(EP.DATE_START_PRIV,DATE '1000-01-01') and 
                                NVL(EP.DATE_END_PRIV,DATE '3000-01-01')))
    order by code_subdiv, emp_last_name)