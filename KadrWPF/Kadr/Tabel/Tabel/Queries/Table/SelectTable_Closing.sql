WITH SUBD as 
(
	select SUBDIV_ID FROM {0}.SUBDIV
    start with subdiv_id in (
        select subdiv_id from {0}.access_subdiv 
        where upper(user_name) = ora_login_user and upper(app_name) = 'TABLE' AND                                 
            :p_TABLE_DATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
    connect by prior subdiv_id = parent_id
)
select S.SUBDIV_ID, S.CODE_SUBDIV, S.SUBDIV_NAME, TABLE_CLOSING_ID, TIME_CLOSING, NVL(TC.SIGN_PROCESSING,0) SIGN_PROCESSING,
    TC.TABLE_PLAN_APPROVAL_ID, 
	(SELECT COLOR_ROW_TABLE FROM {0}.TABLE_PLAN_APPROVAL TPA WHERE TPA.TABLE_PLAN_APPROVAL_ID = TC.TABLE_PLAN_APPROVAL_ID) COLOR_ROW_TABLE, 
	CASE WHEN TC.TABLE_PLAN_APPROVAL_ID = (SELECT MAX(TPA.TABLE_PLAN_APPROVAL_ID) FROM {0}.TABLE_PLAN_APPROVAL TPA 
										WHERE TPA.TYPE_TABLE_ID = TC.TYPE_TABLE_ID) 
		THEN 1 ELSE 0 END SIGN_CLOSING
from {0}.SUBDIV_FOR_TABLE SFT
join {0}.SUBDIV S ON SFT.SUBDIV_ID = S.SUBDIV_ID
left join (SELECT * FROM {0}.TABLE_CLOSING TC 
        WHERE TC.TABLE_DATE = TRUNC(:p_TABLE_DATE,'MONTH') and TC.TYPE_TABLE_ID = :p_TYPE_TABLE_ID ) TC 
    on SFT.SUBDIV_ID = TC.SUBDIV_ID
where SFT.SUBDIV_ID in (SELECT SUBDIV_ID FROM SUBD)
	/* по этим параметрам обновляем строки, поменявшие статус согласования */
	and (:p_SIGN_NOTIFICATION = 0 or (:p_SIGN_NOTIFICATION = 1 and 
			TC.ROWID in (select CHARTOROWID(COLUMN_VALUE) from TABLE(:p_TABLE_ID))))
ORDER BY CODE_SUBDIV