select 
(select reward_name from {1}.reward_name where reward_name_id = rew.reward_name_id) as rew_name,
rew.rew_doc_name,
rew.series||' '||rew.num_reward as num_reward,
to_char(rew.date_reward,'dd.mm.yyyy') as date_reward
from {1}.reward rew where rew.per_num=to_number('{0}')