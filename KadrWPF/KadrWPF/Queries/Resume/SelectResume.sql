select R.RESUME_ID, R.RESUME_PER_NUM, R.FILING_DATE_RESUME,
    E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME, E.EMP_SEX, E.EMP_BIRTH_DATE, 
    R.SOURCE_EMPLOYABILITY_ID, 
    (SELECT SE.SOURCE_EMPLOYABILITY_NAME FROM {0}.SOURCE_EMPLOYABILITY SE 
    WHERE SE.SOURCE_EMPLOYABILITY_ID = R.SOURCE_EMPLOYABILITY_ID) SOURCE_EMPLOYABILITY_NAME
from {0}.resume R
join {0}.EMP E on (R.RESUME_PER_NUM = E.PER_NUM)
where ((:p_begin_period is null and R.DATE_HIRE is null) or 
    (:p_begin_period is not null and R.DATE_HIRE between :p_begin_period and :p_end_period))
	and NVL(:p_EMP_LAST_NAME,E.EMP_LAST_NAME)=E.EMP_LAST_NAME
    and NVL(:p_EMP_FIRST_NAME,E.EMP_FIRST_NAME)=E.EMP_FIRST_NAME
    and NVL(:p_EMP_MIDDLE_NAME,E.EMP_MIDDLE_NAME)=E.EMP_MIDDLE_NAME
    and NVL(:p_EMP_SEX,E.EMP_SEX)=E.EMP_SEX
    and E.EMP_BIRTH_DATE between NVL(:p_BEGIN_BIRTH_DATE,DATE '1001-01-01') and NVL(:p_END_BIRTH_DATE,DATE '3001-01-01')
    and NVL(:p_SOURCE_EMPLOYABILITY_ID,NVL(R.SOURCE_EMPLOYABILITY_ID,-1)) = NVL(R.SOURCE_EMPLOYABILITY_ID,-1)
    and R.FILING_DATE_RESUME between NVL(:p_BEGIN_FILING,DATE '1001-01-01') and NVL(:p_END_FILING,DATE '3001-01-01')
    and (:p_PW_NAME_POS is null or 
        exists(select null from {0}.PREV_WORK PW 
                where PW.PER_NUM = E.PER_NUM and PW.PW_NAME_POS like :p_PW_NAME_POS))
    and ((:p_INSTIT_ID is null and :p_SPEC_ID is null and :p_QUAL_ID is null and :p_TYPE_EDU_ID is null) or 
        exists(select null from {0}.EDU ED 
                WHERE ED.PER_NUM = E.PER_NUM and NVL(:p_INSTIT_ID,NVL(ED.INSTIT_ID,0)) = NVL(ED.INSTIT_ID,0) and
                    NVL(:p_SPEC_ID,NVL(ED.SPEC_ID,0)) = NVL(ED.SPEC_ID,0) and 
					NVL(:p_QUAL_ID,NVL(ED.QUAL_ID,0)) = NVL(ED.QUAL_ID,0) and 
                    NVL(:p_TYPE_EDU_ID,NVL(ED.TYPE_EDU_ID,0)) = NVL(ED.TYPE_EDU_ID,0)))