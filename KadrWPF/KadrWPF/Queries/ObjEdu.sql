select T_E.TE_NAME, 
INS.INSTIT_NAME ||', '|| to_char(ED.YEAR_GRADUATING, 'YYYY' )||'ã.' as ins_name,
spec.name_spec ||', '|| ql.qual_name as spc_ql
from {1}.qual ql, {1}.edu ed, {1}.instit ins, {1}.speciality spec, {1}.type_edu t_e 
where spec.spec_id(+) = ed.spec_id and ql.qual_id(+) = ed.qual_id and ed.per_num = to_number('{0}') 
and ED.INSTIT_ID=INS.INSTIT_ID(+) and ED.TYPE_EDU_ID = T_E.TYPE_EDU_ID