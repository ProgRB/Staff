select EG.EMP_GR_WORK_ID as EMP_GR_WORK_ID, EG.GR_WORK_DATE_BEGIN as "Дата установки",
    (select GR.GR_WORK_NAME from {0}.GR_WORK GR where GR.GR_WORK_ID = EG.GR_WORK_ID) as "Наименование графика",
    (select (select TZ.TIME_ZONE_NAME from {0}.TIME_ZONE TZ where TZ.TIME_ZONE_ID = GD.TIME_ZONE_ID) 
    from {0}.GR_WORK_DAY GD where GD.GR_WORK_ID = EG.GR_WORK_ID and GD.NUMBER_DAY = EG.GR_WORK_DAY_NUM) as "Временная зона",
	EXTRACT(YEAR FROM EG.GR_WORK_DATE_BEGIN) YEAR_GR_WORK
from {0}.EMP_GR_WORK EG
where EG.PER_NUM = :p_per_num and EG.TRANSFER_ID in
    (select TRANSFER_ID from {0}.TRANSFER connect by prior FROM_POSITION = TRANSFER_ID start with TRANSFER_ID = :p_transfer_id)
order by GR_WORK_DATE_BEGIN desc