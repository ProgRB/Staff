merge into {0}.EMP_GROUP_MASTER GM
    using (select :P_PER_NUM as PER_NUM, :P_SIGN_COMB as SIGN_COMB, :P_DATE_GROUP_MASTER as DATE_GROUP_MASTER from dual) T1 
    on (GM.PER_NUM = T1.PER_NUM and GM.SIGN_COMB = T1.SIGN_COMB and trunc(GM.DATE_GROUP_MASTER) = trunc(T1.DATE_GROUP_MASTER))
    when matched then
        update set NAME_GROUP_MASTER = :P_NAME_GROUP_MASTER
    when not matched then
        insert(EMP_GROUP_MASTER_ID, PER_NUM, NAME_GROUP_MASTER, DATE_GROUP_MASTER, SIGN_COMB)
        values({0}.EMP_GROUP_MASTER_ID_seq.nextval, :P_PER_NUM, :P_NAME_GROUP_MASTER, :P_DATE_GROUP_MASTER, :P_SIGN_COMB)