select FIRST_TIME_BEGIN, LAST_TIME_END, TIME_BEGIN, TIME_END,
    MAX(case when :p_doc_begin between TIME_BEGIN and TIME_END or :p_doc_end between TIME_BEGIN and TIME_END then 1 else 0 end) OVER() FL_GRAPH
from TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date-5, 'dd.mm.yyyy'), to_char(trunc(:p_date+5), 'dd.mm.yyyy'), :p_per_num, :p_transfer_id)) TW  
where TW.G_DATE = :p_date