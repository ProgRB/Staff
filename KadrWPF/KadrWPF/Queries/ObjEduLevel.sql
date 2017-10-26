select ins.instit_name||' - '|| T_P_S.TYPE_POSTG_STUDY_NAME as edu_level 
from {1}.instit ins, {1}.postg_study p_s, {1}.type_postg_study t_p_s 
where 
	P_S.TYPE_POSTG_STUDY_ID = T_P_S.TYPE_POSTG_STUDY_ID(+) 
	and P_S.PER_NUM = to_number('{0}') 
	and INS.INSTIT_ID = P_S.INSTIT_ID(+)