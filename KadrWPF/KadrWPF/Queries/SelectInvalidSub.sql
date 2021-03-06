select rownum, v1.* 
from (
    select SU.CODE_SUBDIV, EP.PER_NUM, EM.EMP_LAST_NAME||' '||EM.EMP_FIRST_NAME||' '||EM.EMP_MIDDLE_NAME,
		(SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = TR.POS_ID) POS_NAME,
        TP.NAME_PRIV, EP.PRIV_NUM_DOC, EP.DATE_START_PRIV, EP.DATE_END_PRIV, 
		DECODE(EP.MEDICAL_SOCIAL_EXPERTISE,1,'X') MEDICAL_SOCIAL_EXPERTISE,
		DECODE(EP.INDIVIDUAL_REHABILITATION,1,'X') INDIVIDUAL_REHABILITATION
    from {0}.EMP_PRIV EP
    join {0}.EMP EM on EP.PER_NUM = EM.PER_NUM
    join {0}.TYPE_PRIV TP on EP.TYPE_PRIV_ID = TP.TYPE_PRIV_ID 
    join {0}.TRANSFER TR on EP.PER_NUM = TR.PER_NUM
    join {0}.SUBDIV SU on TR.SUBDIV_ID = SU.SUBDIV_ID
    where TP.SIGN_INVALID = 1 and SU.CODE_SUBDIV = :p_CODE_SUBDIV and TR.SIGN_CUR_WORK = 1
        and (TR.SIGN_COMB = 0 or 
        (TR.SIGN_COMB = 1 and not exists(
            select null from {0}.TRANSFER TR1 where TR.PER_NUM = TR1.PER_NUM
                and TR1.SIGN_CUR_WORK = 1 and TR1.SIGN_COMB = 0))) 
        and :p_dateRep between EP.DATE_START_PRIV and nvl(EP.DATE_END_PRIV,Date '3000-01-01')
    order by SU.CODE_SUBDIV, EP.PER_NUM
) v1