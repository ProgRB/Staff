select m_s.name_state, 
m_s.mar_state_id,
pas.num_passport,
pas.seria_passport,
to_char(pas.when_given,'DD.MM.YYYY') as when_given,
pas.who_given,
reg.reg_post_code,
ABR.ABBREV_NAME||' '|| rgn.name_region||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = dist.abbrev_id) ||' '||dist.name_district||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = cit.abbrev_id) ||' '||cit.name_city||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = loc.abbrev_id) ||' '||loc.locality_name||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = st.abbrev_id) ||' '||regexp_substr(trim(st.name_street),'.*\s')||' д.'||
reg.reg_house||(case when reg.reg_bulk is null then '' else ' кор.' end)||reg.reg_bulk/*' кор.'||reg.reg_bulk*/||(case when reg.reg_flat is null then '' else ' кв.' end)||reg.reg_flat as rgstr,
to_char(reg.date_reg, 'DD.MM.YYYY') as date_reg,
reg.reg_phone
from {1}.street st, {1}.region rgn, {1}.abbrev abr, {1}.district dist, {1}.locality loc, {1}.city cit, {1}.registr reg, {1}.passport pas, {1}.mar_state m_s where 
PAS.MAR_STATE_ID = m_s.MAR_STATE_ID and reg.per_num = pas.per_num and rgn.code_region(+) = substr(reg.reg_code_street, 1, 2) and
dist.code_district(+) = substr(reg.reg_code_street, 1, 5) and cit.code_city(+) = substr(reg.reg_code_street, 1, 8) and loc.code_locality(+) = substr(reg.reg_code_street, 1, 11)
 and st.code_street(+) = reg.reg_code_street and ABR.ABBREV_ID = RGN.ABBREV_ID and pas.per_num = to_number('{0}')