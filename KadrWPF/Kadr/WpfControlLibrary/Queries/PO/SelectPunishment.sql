select PUNISHMENT_ID, VIOLATION_ID, PUNISHMENT_NUM_ORDER, PUNISHMENT_DATE_ORDER, 
	CHIEF_TRANSFER_ID,
    (SELECT (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = T.SUBDIV_ID)||' - '||
    (SELECT E.EMP_LAST_NAME||' '||E.EMP_FIRST_NAME||' '||E.EMP_MIDDLE_NAME FROM {0}.EMP E
    WHERE E.PER_NUM = T.PER_NUM)
    FROM {0}.TRANSFER T WHERE T.TRANSFER_ID = P.CHIEF_TRANSFER_ID) FIO_CHIEF
from {0}.PUNISHMENT P
where P.VIOLATION_ID = :p_VIOLATION_ID