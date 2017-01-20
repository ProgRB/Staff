select count(*) from {0}.EMP E
left join {0}.TRANSFER T on (E.PER_NUM=T.PER_NUM) 
where E.EMP_LAST_NAME = :p_EMP_LAST_NAME and E.EMP_FIRST_NAME = :p_EMP_FIRST_NAME and 
    E.EMP_MIDDLE_NAME = :p_EMP_MIDDLE_NAME and E.EMP_BIRTH_DATE = :p_EMP_BIRTH_DATE and
    E.PER_NUM != :p_PER_NUM and
    ((TO_NUMBER(E.PER_NUM) < 79000 and T.SIGN_CUR_WORK = 1) or TO_NUMBER(E.PER_NUM) between 79000 and 199999)
