select EDU_ID,PER_NUM,MAIN_PROF, SERIA_DIPLOMA,NUM_DIPLOMA, SPECIALIZATION,YEAR_GRADUATING,FROM_FACT,
    SPEC_ID, INSTIT_ID, TYPE_STUDY_ID, TYPE_EDU_ID, QUAL_ID, GR_SPEC_ID
from {0}.EDU ED
WHERE PER_NUM = :p_PER_NUM 