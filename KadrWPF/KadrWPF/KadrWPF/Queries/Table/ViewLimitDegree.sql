select 
    DECODE(GROUPING(CODE_SUBDIV),0,CODE_SUBDIV,' ') "ПОДР.",
    DECODE(GROUPING(V2.NUM_MONTH)+GROUPING(floor((V2.NUM_MONTH-1)/3))+GROUPING(floor((V2.NUM_MONTH-1)/6))+GROUPING(CODE_SUBDIV),
        1, '    '||to_char(floor((V2.NUM_MONTH-1)/3)+1)|| ' кв.',
        2, '  '||to_char(floor((V2.NUM_MONTH-1)/6)+1)||' пол.',        
        3, 'Год', 4, 'Общий итог',
        '      '||to_char(to_date(V2.NUM_MONTH,'MM'),'Month','NLS_DATE_LANGUAGE=RUSSIAN')) as "Месяц",
    GROUPING(V2.NUM_MONTH)+GROUPING(floor((V2.NUM_MONTH-1)/3))+GROUPING(floor((V2.NUM_MONTH-1)/6)) as F_GROUP,
    GROUPING(V2.NUM_MONTH) as FL, 
    /* Если это итоговая строчка за квартал, полугодие или год, то выводим сумму Плана по месяцам. Иначе плавающий план*/   
    round(sum(decode(v2.CODE_DEGREE,'01',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "01-план", 
    round(sum(decode(V2.CODE_DEGREE,'01',V2.FACT,null)),2) as "01-факт", round(sum(decode(V2.CODE_DEGREE,'01',V2.FACT303,null)),2) as "01-303",
    round(sum(decode(v2.CODE_DEGREE,'02',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "02-план", 
    round(sum(decode(V2.CODE_DEGREE,'02',V2.FACT,null)),2) as "02-факт", round(sum(decode(V2.CODE_DEGREE,'02',V2.FACT303,null)),2) as "02-303",
    round(sum(decode(v2.CODE_DEGREE,'04',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "04-план", 
    round(sum(decode(V2.CODE_DEGREE,'04',V2.FACT,null)),2) as "04-факт", round(sum(decode(V2.CODE_DEGREE,'04',V2.FACT303,null)),2) as "04-303",
    round(sum(decode(v2.CODE_DEGREE,'05',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "05-план", 
    round(sum(decode(V2.CODE_DEGREE,'05',V2.FACT,null)),2) as "05-факт", round(sum(decode(V2.CODE_DEGREE,'05',V2.FACT303,null)),2) as "05-303",
    round(sum(decode(v2.CODE_DEGREE,'08',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "08-план", 
    round(sum(decode(V2.CODE_DEGREE,'08',V2.FACT,null)),2) as "08-факт", round(sum(decode(V2.CODE_DEGREE,'08',V2.FACT303,null)),2) as "08-303",
    round(sum(decode(v2.CODE_DEGREE,'09',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "09-план", 
    round(sum(decode(V2.CODE_DEGREE,'09',V2.FACT,null)),2) as "09-факт", round(sum(decode(V2.CODE_DEGREE,'09',V2.FACT303,null)),2) as "09-303",
    round(sum(decode(v2.CODE_DEGREE,'11',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "11-план", 
    round(sum(decode(V2.CODE_DEGREE,'11',V2.FACT,null)),2) as "11-факт", round(sum(decode(V2.CODE_DEGREE,'11',V2.FACT303,null)),2) as "11-303",
    round(sum(decode(v2.CODE_DEGREE,'13',DECODE(V2.PLAN,0,null,V2.PLAN))),2) as "13-план", 
    round(sum(decode(V2.CODE_DEGREE,'13',V2.FACT,null)),2) as "13-факт", round(sum(decode(V2.CODE_DEGREE,'13',V2.FACT303,null)),2) as "13-303"
from (
    select * 
    from TABLE(APSTAFF.CALC_LIMIT(:p_subdiv_id, null, null, :p_pay_type_id, null, :p_year, :p_sign))
    where NUM_MONTH between :p_month1 and :p_month2 and 
            (:p_all_rows = 1 or (:p_all_rows = 0 and nvl(PLAN,0)+nvl(FACT,0)+nvl(FACT303,0) != 0))
) V2
group by rollup(CODE_SUBDIV, floor((V2.NUM_MONTH-1)/6),floor((V2.NUM_MONTH-1)/3),V2.NUM_MONTH)