SELECT GROUPING(V.PER_NUM) GR1, V.NGM, V.CD, V.PER_NUM, 
    DECODE(GROUPING(V.CD)+GROUPING(V.NGM)+GROUPING(V.PER_NUM), 
    0, E.EMP_LAST_NAME||' '||SUBSTR(E.EMP_FIRST_NAME,1,1)||'.'||SUBSTR(E.EMP_MIDDLE_NAME,1,1)||'.',
    1, 'ИТОГО по группе мастера',
    2, 'ИТОГО по категории',
    3, 'ВСЕГО') FIO,
    V.DOC_BEGIN, V.DOC_END, SUM(V.PTIME) as PTIME
FROM (
WITH tp_per as
    (   select V.TRANSFER_ID, V.DEGREE_ID from 
        (SELECT        
          subdiv_id,
          transfer_id, 
          degree_id
        FROM {0}.transfer t    
        START WITH sign_cur_work = 1 OR type_transfer_id = 3
        CONNECT BY NOCYCLE PRIOR from_position = transfer_id) V
        WHERE V.SUBDIV_ID = :p_subdiv_id)
select GM.NGM, D.CODE_DEGREE CD, R.PER_NUM, R.DOC_BEGIN, R.DOC_END, 
    round(WP.VALID_TIME/3600,2) PTIME, trunc(R.DOC_BEGIN) DATE_WORK 
from {0}.REG_DOC R 
JOIN {0}.DOC_LIST D on D.DOC_LIST_ID = R.DOC_LIST_ID
left join tp_per tp on (tp.transfer_id=R.TRANSFER_ID)
join {0}.DEGREE D on tp.DEGREE_ID = D.DEGREE_ID
JOIN {0}.WORK_PAY_TYPE WP on WP.REG_DOC_ID = R.REG_DOC_ID
left join (select GM.NAME_GROUP_MASTER NGM,GM.transfer_id,begin_group,NVL(end_group,
                LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by GM.transfer_id order by begin_group)) end_group
        from {0}.EMP_GROUP_MASTER GM join tp_per tp1 on (gm.transfer_id=tp1.transfer_id)) 
    GM on (gm.transfer_id=tp.transfer_id and :p_date_end between begin_group and end_group)   
where R.DOC_BEGIN between :p_date_begin and :p_date_end and D.PAY_TYPE_ID = :p_pay_type_id
) V
JOIN {0}.EMP E ON E.PER_NUM = V.PER_NUM
GROUP BY ROLLUP(V.CD, V.NGM, (V.DATE_WORK, V.PER_NUM, E.EMP_LAST_NAME, E.EMP_FIRST_NAME, E.EMP_MIDDLE_NAME, V.CD, V.DOC_BEGIN, 
    V.DOC_END, V.PTIME))