select
r_t.name_rel,
(rel_last_name||' '||rel_first_name||' '||rel_middle_name) as relat,
to_char(rel_birth_date, 'YYYY') as year_birth
from {1}.relative rel, {1}.rel_type r_t where REL.REL_ID = R_T.REL_ID and rel.per_num = to_number('{0}')