select code_subdiv,
subdiv_name,
(select count(per_num) from {1}.transfer tr where sign_cur_work = 1 and subdiv_id = sub.subdiv_id group by subdiv_id) as all_n,
(select count(per_num) from {1}.transfer tr where sign_cur_work = 1 and subdiv_id = sub.subdiv_id and sign_comb=1 group by subdiv_id) as comb,
(select count(tr.per_num) from {1}.transfer tr left join {1}.emp em on (em.per_num=tr.per_num) where sign_cur_work = 1 and subdiv_id = sub.subdiv_id and sign_comb!=1 and upper(EM.EMP_SEX)='Æ' group by subdiv_id) as wom
from {1}.subdiv sub where code_subdiv in ({0}) order by code_subdiv