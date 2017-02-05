select TPGS.TYPE_POSTG_STUDY_NAME as t_p_s_n,
ins.instit_name,
PGS.PGS_DOC_NAME||' '||
PGS.PGS_DOC_NUM||' '||
PGS.GIVE_DATE as pgs_doc,
to_char(PGS.GIVE_DATE,'YYYY') as year_grad
from 
 {1}.postg_study pgs, {1}.type_postg_study tpgs, {1}.instit ins where 
pgs.per_num = to_number('{0}') and TPGS.TYPE_POSTG_STUDY_ID = PGS.TYPE_POSTG_STUDY_ID