select 
    DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,' ') "����.",
    DECODE(GROUPING(V2.NUM_MONTH)+GROUPING(floor((V2.NUM_MONTH-1)/3))+GROUPING(floor((V2.NUM_MONTH-1)/6))+GROUPING(CODE_SUBDIV),
        1, '    '||to_char(floor((V2.NUM_MONTH-1)/3)+1)|| ' ��.',
        2, '  '||to_char(floor((V2.NUM_MONTH-1)/6)+1)||' ���.',        
        3, '���', 4, '����� ����',
        '      '||to_char(to_date(V2.NUM_MONTH,'MM'),'Month','NLS_DATE_LANGUAGE=RUSSIAN')) as "�����",
    GROUPING(V2.NUM_MONTH)+GROUPING(floor((V2.NUM_MONTH-1)/3))+GROUPING(floor((V2.NUM_MONTH-1)/6)) as F_GROUP,
    GROUPING(V2.NUM_MONTH) as FL, 
    /* ���� ��� �������� ������� �� �������, ��������� ��� ���, �� ������� ����� ����� �� �������. ����� ��������� ����*/   
    round(sum(decode(v2.CODE_DEGREE,'01',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "01-����", 
    round(sum(decode(V2.CODE_DEGREE,'01',V2.FACT,null)),2) as "01-����", round(sum(decode(V2.CODE_DEGREE,'01',V2.FACT303,null)),2) as "01-303",
    round(sum(decode(v2.CODE_DEGREE,'02',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "02-����", 
    round(sum(decode(V2.CODE_DEGREE,'02',V2.FACT,null)),2) as "02-����", round(sum(decode(V2.CODE_DEGREE,'02',V2.FACT303,null)),2) as "02-303",
    round(sum(decode(v2.CODE_DEGREE,'04',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "04-����", 
    round(sum(decode(V2.CODE_DEGREE,'04',V2.FACT,null)),2) as "04-����", round(sum(decode(V2.CODE_DEGREE,'04',V2.FACT303,null)),2) as "04-303",
    round(sum(decode(v2.CODE_DEGREE,'05',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "05-����", 
    round(sum(decode(V2.CODE_DEGREE,'05',V2.FACT,null)),2) as "05-����", round(sum(decode(V2.CODE_DEGREE,'05',V2.FACT303,null)),2) as "05-303",
    round(sum(decode(v2.CODE_DEGREE,'08',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "08-����", 
    round(sum(decode(V2.CODE_DEGREE,'08',V2.FACT,null)),2) as "08-����", round(sum(decode(V2.CODE_DEGREE,'08',V2.FACT303,null)),2) as "08-303",
    round(sum(decode(v2.CODE_DEGREE,'09',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "09-����", 
    round(sum(decode(V2.CODE_DEGREE,'09',V2.FACT,null)),2) as "09-����", round(sum(decode(V2.CODE_DEGREE,'09',V2.FACT303,null)),2) as "09-303",
    round(sum(decode(v2.CODE_DEGREE,'11',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "11-����", 
    round(sum(decode(V2.CODE_DEGREE,'11',V2.FACT,null)),2) as "11-����", round(sum(decode(V2.CODE_DEGREE,'11',V2.FACT303,null)),2) as "11-303",
    round(sum(decode(v2.CODE_DEGREE,'13',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "13-����", 
    round(sum(decode(V2.CODE_DEGREE,'13',V2.FACT,null)),2) as "13-����", round(sum(decode(V2.CODE_DEGREE,'13',V2.FACT303,null)),2) as "13-303"
from (
    select * 
    from TABLE(APSTAFF.CALC_LIMIT(:p_subdiv_id, null, null, :p_pay_type_id, null, :p_year, :p_sign))
    where NUM_MONTH between :p_month1 and :p_month2 and 
            (:p_all_rows = 1 or (:p_all_rows = 0 and nvl(PLAN,0)+nvl(FACT,0)+nvl(FACT303,0) != 0))
) V2
group by rollup(CODE_SUBDIV, floor((V2.NUM_MONTH-1)/6),floor((V2.NUM_MONTH-1)/3),V2.NUM_MONTH)