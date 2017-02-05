merge into APSTAFF.SUBDIV_FOR_TABLE ST
    using (select :p_SUBDIV_ID as SUBDIV_ID from dual) T1 
    on (ST.SUBDIV_ID = T1.SUBDIV_ID)
    when matched then
        update set ST.DATE_ADVANCE = :p_date_advance, ST.DATE_SALARY = :p_date_salary, ST.SIGN_PROCESSING = :p_sign_processing
    when not matched then
        insert(SUBDIV_FOR_TABLE_ID, SUBDIV_ID)
        values(apstaff.SUBDIV_FOR_TABLE_ID_seq.nextval, :p_SUBDIV_ID)