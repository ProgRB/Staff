select hab.hab_post_code,
abr.ABBREV_NAME||' '|| rgn.name_region||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = dist.abbrev_id) ||' '||dist.name_district||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = cit.abbrev_id) ||' '||cit.name_city||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = loc.abbrev_id) ||' '||loc.locality_name||' '||
(select abbr.abbrev_name from {1}.abbrev abbr where abbr.abbrev_id(+) = st.abbrev_id) ||' '||regexp_substr(trim(st.name_street),'.*\s')||' д.'||
hab.hab_house||(case when hab.hab_bulk is null then '' else ' кор.' end)||hab.hab_bulk/*' кор.'||reg.reg_bulk*/||(case when hab.hab_flat is null then '' else ' кв.' end)||hab.hab_flat as rgstr
from {1}.street st, {1}.region rgn, {1}.abbrev abr, {1}.district dist, {1}.locality loc, {1}.city cit, {1}.habit hab, {1}.passport pas, {1}.mar_state m_s where 
PAS.MAR_STATE_ID = m_s.MAR_STATE_ID and hab.per_num = pas.per_num and rgn.code_region(+) = substr(hab.hab_code_street, 1, 2) and
dist.code_district(+) = substr(hab.hab_code_street, 1, 5) and cit.code_city(+) = substr(hab.hab_code_street, 1, 8) and loc.code_locality(+) = substr(hab.hab_code_street, 1, 11)
 and st.code_street(+) = hab.hab_code_street and ABR.ABBREV_ID = RGN.ABBREV_ID and pas.per_num = to_number('{0}')