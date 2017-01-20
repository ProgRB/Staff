merge into {0}.EMP_ORDER EO
    using (select :p_PER_NUM as PER_NUM, :p_SUBDIV_ID as SUBDIV_ID, :p_DATE_ORDER as DATE_ORDER, :p_SIGN_COMB as SIGN_COMB from dual) T1 
    on (EO.PER_NUM = T1.PER_NUM and EO.SUBDIV_ID = T1.SUBDIV_ID and trunc(EO.DATE_ORDER) = trunc(T1.DATE_ORDER) and EO.SIGN_COMB = T1.SIGN_COMB)
    when matched then
        update set ORDER_ID = :p_ORDER_ID
    when not matched then
        insert(EMP_ORDER_ID, PER_NUM, SUBDIV_ID, ORDER_ID, SIGN_COMB, DATE_ORDER, TRANSFER_ID)
        values({0}.EMP_ORDER_ID_seq.nextval, :p_PER_NUM, :p_SUBDIV_ID, :p_ORDER_ID, :p_SIGN_COMB, :p_DATE_ORDER, :p_TRANSFER_ID)