select V1.NPUT as "� �/�", V1.DAYCOM as "��� ���.", V1.BEGIN_WORK as "���� ������", V1.END_WORK as "���� ���������", 
    V1.BEGIN_PL as "�/� �", V1.END_PL as "�/� ��", 
    V1.RECESS as "�������", V1.CHASREM as "���� �������", V1.DAT_SDACH as "���� �����", NOMAVT "����� ����������"
from (  
select /*+ no_merge(V) */ V.*, C.CALENDAR_DAY 
from 
(
    select * from {0}.V_PUTL_NEW VPL where VPL.PER_NUM = :p_per_num 
) V
join {0}.CALENDAR C on C.CALENDAR_DAY between trunc(V.BEGIN_WORK) and trunc(V.END_WORK)
)  V1
where V1.CALENDAR_DAY = :p_date_work