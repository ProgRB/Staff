select count(*) from {0}.REWARD R
join {0}.REWARD_NAME RN on R.REWARD_NAME_ID = RN.REWARD_NAME_ID
where R.PER_NUM = :p_per_num and upper(RN.REWARD_NAME) like '%берепюм%рпсдю%'