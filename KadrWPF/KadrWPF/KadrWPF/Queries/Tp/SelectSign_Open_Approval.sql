with SUBD as 
    (select SUBDIV_ID FROM {0}.SUBDIV
        start with subdiv_id in (
            select subdiv_id from {0}.access_subdiv 
            where upper(user_name) = ora_login_user and upper(app_name) = 'KADR')
        connect by prior subdiv_id = parent_id)
select /* Если подразделение не попадает в доступные,т.е. человек переходит в другое подразделение,
        нужно дать возможность обновлять статус до начала согласования в другом подразделении*/
    CASE WHEN PT.SUBDIV_ID in (select SUBDIV_ID FROM SUBD) 
        THEN 1 ELSE CASE WHEN PT.SUBDIV_ID not in (select SUBDIV_ID FROM SUBD) and
            PROJECT_PLAN_APPROVAL_ID < 
                (select MIN(PROJECT_PLAN_APPROVAL_ID) from {0}.PROJECT_PLAN_APPROVAL
                where TYPE_TRANSFER_ID = PT.TYPE_TRANSFER_ID and SIGN_APPROVAL_OLD_SUBDIV = 0 and
                    PROJECT_PLAN_APPROVAL_ID >=0) 
        THEN 1 ELSE 0 END END SIGN_OPEN_APPROVAL
from {0}.PROJECT_TRANSFER PT
WHERE PT.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID