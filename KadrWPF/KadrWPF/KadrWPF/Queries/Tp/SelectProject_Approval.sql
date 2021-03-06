select PROJECT_APPROVAL_ID, PROJECT_TRANSFER_ID, PA.PROJECT_PLAN_APPROVAL_ID, PPA.NOTE_ROLE_NAME, 
    DATE_APPROVAL, NOTE_APPROVAL, TYPE_APPROVAL_ID, 
	(select TYPE_APPROVAL_NAME from {0}.TYPE_APPROVAL TA where TA.TYPE_APPROVAL_ID = PA.TYPE_APPROVAL_ID) TYPE_APPROVAL_NAME,
    (select EMP_LAST_NAME||' '||EMP_FIRST_NAME||' '||EMP_MIDDLE_NAME 
    FROM {0}.EMP E WHERE E.PER_NUM = {0}.GET_PER_NUM_FROM_USER_NAME(PA.USER_NAME)) USER_FIO
from {0}.PROJECT_APPROVAL PA
LEFT JOIN {0}.PROJECT_PLAN_APPROVAL PPA on (PA.PROJECT_PLAN_APPROVAL_ID=PPA.PROJECT_PLAN_APPROVAL_ID)
WHERE PA.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID
ORDER BY DATE_APPROVAL desc