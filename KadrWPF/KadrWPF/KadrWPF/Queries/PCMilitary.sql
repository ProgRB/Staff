select m_c.res_cat,
m_r.name_mil_rank,
m_ct.mil_cat_name,
m_c.name_mil_spec,
m_cl.design,
cmm.comm_name,
(case (mil_state)
when 1 then 'состоит'
when 0 then 'не состоит'
else ''
end) as mil_st,
special_reg,
(case 
when (m_c.date_REMOVE is not null) then 'снят'
else 'состоит' 
end) as remov
from {1}.mil_card m_c, {1}.mil_rank m_r, {1}.mil_cat m_ct, {1}.comm cmm, {1}.med_classif m_cl where M_C.COMM_ID=cmm.comm_id and M_C.MED_CLASSIF_ID=M_CL.MED_CLASSIF_ID and
M_C.MIL_CAT_ID=M_CT.MIL_CAT_ID and M_C.MIL_RANK_ID=M_R.MIL_RANK_ID and m_c.per_num =to_number('{0}')