select to_char(rew.date_reward,'DD.MM.YYYY')||'�. - '||(select reward_name from {1}.reward_name where reward_name_id = rew.reward_name_id) as rewar 
from {1}.reward rew where rew.per_num = to_number('{0}') 
order by rew.date_reward