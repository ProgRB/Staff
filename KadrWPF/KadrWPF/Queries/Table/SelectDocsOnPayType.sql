select DECODE(GROUPING(V.CODE_SUBDIV)+GROUPING(V.CODE_DEGREE)+GROUPING(V.NGM)+GROUPING(V.PER_NUM),
        0, V.CODE_SUBDIV,
        1, '»“Œ√Œ ÔÓ √Ã '||V.NGM,
        2, '»“Œ√Œ ÔÓ  “ '||V.CODE_DEGREE,
        3, '»“Œ√Œ ÔÓ ÔÓ‰. '||V.CODE_SUBDIV,
        4, '¬—≈√Œ') CODE_SUBDIV,
    DECODE(GROUPING(V.PER_NUM),0,V.CODE_DEGREE) CODE_DEGREE,
    DECODE(GROUPING(V.PER_NUM),0,V.NGM) NGM,
    V.CODE_FORM_OPERATION, V.PER_NUM,  
	--V.FIO, 
	DECODE(GROUPING(V.CODE_SUBDIV)+GROUPING(V.CODE_DEGREE)+GROUPING(V.NGM)+GROUPING(V.PER_NUM),
        0, V.FIO,COUNT(DISTINCT V.PER_NUM)||' ('||COUNT(V.DOC_BEGIN)||')' ) FIO, 
	V.POS_NAME, SUM(V.VTIME) VTIME, V.DOC_BEGIN, V.DOC_END, V.DOC_LOCATION
from 
    (  
        select 
            CODE_SUBDIV, 
            NGM, 
            CODE_DEGREE, 
            PER_NUM, 
            EMP_LAST_NAME ||' '|| substr(EMP_FIRST_NAME,1,1) ||'.'|| substr(EMP_MIDDLE_NAME,1,1)||'.' as FIO, 
            POS_NAME,
            (select SUM(round(WP.VALID_TIME/3600,2)) 
                from apstaff.WORKED_DAY W 
                join apstaff.WORK_PAY_TYPE WP on W.WORKED_DAY_ID = WP.WORKED_DAY_ID     
                where WP.REG_DOC_ID = R.REG_DOC_ID and W.WORK_DATE between :p_date_begin and :p_date_end) VTIME,
            to_char(R.DOC_BEGIN,'dd.mm.yyyy hh24:mi') as DOC_BEGIN, to_char(R.DOC_END,'dd.mm.yyyy hh24:mi') as DOC_END,
            R.DOC_LOCATION,
            CODE_FORM_OPERATION 
        from apstaff.REG_DOC R
        join (select transfer_id, worker_id from apstaff.transfer) using (transfer_id)
        join
        (
            select worker_id, 
                   max(subdiv_id) keep (dense_rank last order by date_transfer) subdiv_id, 
                   max(degree_id) keep (dense_rank last order by date_transfer) degree_id, 
                   max(form_operation_id) keep (dense_rank last order by date_transfer) form_operation_id,
                   max(pos_id) keep (dense_rank last order by date_transfer) pos_id
            from apstaff.transfer
            where date_transfer<=:p_date_end
            group by worker_id
        ) using (worker_id)
        join apstaff.degree using (degree_id)
        join apstaff.subdiv using (subdiv_id)
        join apstaff.form_operation using (form_operation_id)
        join apstaff.emp using (per_num)
        join apstaff.position using (pos_id)
        left join (
                    select worker_id, max(NGM) keep (dense_rank last order by begin_group) NGM 
                    from 
                    (
                        select NAME_GROUP_MASTER NGM, worker_id ,begin_group, NVL(end_group,
                            LEAD(trunc(begin_group)-1/86400,1,date'3000-01-01') over (partition by transfer_id order by begin_group)) end_group
                        from apstaff.EMP_GROUP_MASTER GM join (select transfer_id, worker_id from apstaff.transfer) using (transfer_id)
                    )
                    where :p_date_end between begin_group and end_group
                    group by worker_id
                 ) using (worker_id) 
        where R.DOC_LIST_ID = :p_doc_list_id and :p_date_begin<=R.DOC_END and :p_date_end>=R.DOC_BEGIN   
            and subdiv_id in (select subdiv_id from apstaff.subdiv start with subdiv_id=:p_subdiv_id connect by prior subdiv_id=parent_id)
        order by CODE_SUBDIV, CODE_DEGREE, NGM, CODE_FORM_OPERATION, PER_NUM, R.DOC_BEGIN    
    ) V
/*WHERE V.VTIME > 0*/
GROUP BY ROLLUP(V.CODE_SUBDIV, V.CODE_DEGREE, V.NGM, 
    (V.CODE_FORM_OPERATION, V.PER_NUM, V.FIO, V.POS_NAME, V.VTIME, V.DOC_BEGIN, V.DOC_END, V.DOC_LOCATION))
order by V.CODE_SUBDIV, V.CODE_DEGREE, V.NGM, V.CODE_FORM_OPERATION, V.PER_NUM