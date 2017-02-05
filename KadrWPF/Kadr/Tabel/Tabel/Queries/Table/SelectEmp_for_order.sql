select V2.*/*V2.NST, V2.NGM, V2.FIO, V2.POS_NAME, V2.CODE_DEGREE{4}/*,
    decode(w_date1, 0, 'Отгул', to_char(w_date1)) w_date1,
    decode(w_date2, 0, 'Отгул', to_char(w_date2)) w_date2,
    decode(w_date3, 0, 'Отгул', to_char(w_date3)) w_date3*/
from 
(select rownum NST, V1.NGM, 
    DECODE(grouping(V1.CODE_DEGREE)+grouping(V1.FIO),
        0, V1.FIO,
        1, 'ИТОГО по кат.: ',
        2, 'ВСЕГО') FIO, V1.CODE_DEGREE,
    (select P.POS_NAME from {0}.POSITION P where V1.POS_ID = P.POS_ID) as POS_NAME{2}/*, 
    to_char(sum(regexp_substr(w_date1676,'[[:digit:]]+')))||regexp_substr(w_date1676,'[^[:digit:]]+') w_date1676, 
    to_char(sum(regexp_substr(w_date1678,'[[:digit:]]+')))||regexp_substr(w_date1678,'[^[:digit:]]+') w_date1678*/
from (
select V.NGM, V.FIO, V.CODE_DEGREE, V.POS_ID, /*V.HOURS,*/ V.PER_NUM{1}/*,
    max(decode(V.DW, :p_date1676, to_char(V.HOURS) || decode(V.SIGN_HOLIDAY,0, '', ' (отгул)'))) w_date1676, 
    max(decode(V.DW, :p_date1678, to_char(V.HOURS) || decode(V.SIGN_HOLIDAY,0, '', ' (отгул)'))) w_date1678*/
from
(
WITH tp_per as
(   select V.TRANSFER_ID from 
    (SELECT        
      subdiv_id,
      transfer_id
    FROM {0}.transfer t    
    START WITH sign_cur_work = 1 OR type_transfer_id = 3
    CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
    WHERE V.SUBDIV_ID = :p_subdiv_id)
select
    initcap(E.emp_last_name)||' '||substr(E.emp_first_name,1,1)||'.'|| substr(E.emp_middle_name,1,1)||'.' FIO,
    (select D.CODE_DEGREE from {0}.DEGREE D where T.DEGREE_ID = D.DEGREE_ID) as CODE_DEGREE,
    T.POS_ID,
    EO.COUNT_HOURS HOURS, EO.SIGN_HOLIDAY, EO.SIGN_ACTUAL_TIME,
    E.PER_NUM, D.DATE_WORK_ORDER DW, GM.NGM
from {0}.EMP_FOR_ORDER EO 
join {0}.DATE_FOR_ORDER D on D.DATE_FOR_ORDER_ID = EO.DATE_FOR_ORDER_ID
join {0}.TRANSFER T on EO.TRANSFER_ID = T.TRANSFER_ID 
join {0}.EMP E on T.PER_NUM = E.PER_NUM 
left join tp_per tp on (tp.transfer_id=T.TRANSFER_ID)
left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
        from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
    GM on (gm.transfer_id=tp.transfer_id and D.DATE_WORK_ORDER between begin_group and end_group)  
where D.ORDER_ON_HOLIDAY_ID = :DFO_id
) V
group by V.NGM, V.FIO, V.CODE_DEGREE, V.POS_ID, /*V.HOURS, */V.PER_NUM
order by CODE_DEGREE, V.NGM, FIO
) V1
group by rollup (V1.CODE_DEGREE, (rownum, V1.NGM, V1.FIO, V1.POS_ID{3}/*, w_date1, w_date2, w_date3*/))
) V2
order by V2.NST, V2.CODE_DEGREE