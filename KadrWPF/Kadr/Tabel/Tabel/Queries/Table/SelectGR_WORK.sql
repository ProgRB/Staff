select GR_WORK_ID as GR_WORK_ID, 
    ( select case when (select count(*) from {0}.ACCESS_GR_WORK AG where AG.GR_WORK_ID = GR.GR_WORK_ID) = 1 
            then (select (select CODE_SUBDIV from {0}.SUBDIV where subdiv_id = AG.SUBDIV_ID) 
                        from {0}.ACCESS_GR_WORK AG where AG.GR_WORK_ID = GR.GR_WORK_ID)
            else ' ' 
        end from dual) as "����.", 
    GR_WORK_NAME as "������������ ������� ������", COUNT_DAY as "���� � �������", 
    decode(SIGN_HOLIDAY_WORK,1,'X') as "������ � ���. ����", 
    decode(SIGN_COMPACT_DAY_WORK,1,'X') as "������ � ����. ����",
    decode(SIGN_FLOATING,1,'X') as "����. ������", 
	decode(SIGN_SHORTEN,1,'X') as "����. ������ (246)", 
	decode(SIGN_SHIFTMAN,1,'X') as "��������", 
	HOURS_FOR_NORM as "����� ����� (��� 102)", 
    HOURS_FOR_GRAPH as "����� ����� (��� 111)",
	HOURS_NORM_CALENDAR as "����� ����� (��� 106/124)",
	(select ROUND(sum(((TO_DATE('01.01.2014 '||TI.TIME_END,'DD.MM.YYYY HH24:MI')+case when TYPE_INTERVAL_ID=2 then 1 else 0 end)-
            TO_DATE('01.01.2014 '||TI.TIME_BEGIN,'DD.MM.YYYY HH24:MI'))*24), 8) 
		from {0}.GR_WORK_DAY GWD
			join {0}.TIME_ZONE TZ on (GWD.TIME_ZONE_ID = TZ.TIME_ZONE_ID)
			join {0}.TIME_INTERVAL TI on (TZ.TIME_ZONE_ID = TI.TIME_ZONE_ID)
		where GWD.GR_WORK_ID = GR.GR_WORK_ID and GWD.NUMBER_DAY = 1) "��������� ���� �� �������", 		
    decode(SIGN_SUMMARIZE,1,'X') as "������-�� ����",
	HOURS_DINNER as "����. ���. ��� ������. �����",
    decode((select max(GD.SIGN_EVENING_TIME) from {0}.GR_WORK_DAY GD where GD.GR_WORK_ID = GR.GR_WORK_ID),1,'X')
        as "������ ��������� �������",
	DATE_END_GRAPH as "���� ��������� �������"/*,
	ACCESS_TEMPLATE_ID,
	(SELECT ACCESS_TEMPLATE_NAME FROM {0}.ACCESS_TEMPLATE AT 
	WHERE AT.ACCESS_TEMPLATE_ID = GR.ACCESS_TEMPLATE_ID) ACCESS_TEMPLATE_NAME*/
from {0}.GR_WORK GR
where ((GR.GR_WORK_ID in (
		select AG.GR_WORK_ID from {0}.ACCESS_GR_WORK AG 
		where AG.subdiv_id in (select subdiv_id from {0}.SUBDIV where code_subdiv like :p_sub||'%'))
		)
		or :p_sub = '' or :p_sub is null)
	and NVL(GR.DATE_END_GRAPH,DATE '3000-01-01') >= NVL(:p_DATE_END_GRAPH,DATE '1000-01-01')
order by 3