select t_e.te_name, 
ins.instit_name,
(case
when (ed.type_edu_id in (2,3)) then 'аттестат'
when (ed.type_edu_id in (4,5,6,10,11)) then 'диплом'
when (ed.type_edu_id in (8)) then 'свидетельство'
else ''
end ) as doc,
ed.seria_diploma,
ed.num_diploma,
to_char(ed.year_graduating, 'YYYY') as year_graduating,
q.qual_name,
SPEC.NAME_SPEC,
spec.code_spec
from {1}.speciality spec, {1}.qual q, {1}.instit ins, {1}.type_edu t_e, {1}.edu ed where q.qual_id(+) = ed.qual_id and ins.instit_id(+) = ed.instit_id and T_E.TYPE_EDU_ID(+) = ED.TYPE_EDU_ID
 and ed.per_num = to_number('{0}') and spec.spec_id(+) = ed.spec_id