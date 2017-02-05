select sum(v102) as v102, sum(v106_1) as v106_1, sum(v106_2) as v106_2, sum(v114) as v114, sum(v124) as v124, sum(v125) as v125
from 
    (
    select T.PER_NUM,
        round({0}.SalaryOnPayType(102, T.PER_NUM, T.TRANSFER_ID, :p_date_begin, :p_date_end,0)/3600,1) as v102,
        round({0}.SalaryOnPayType(106, T.PER_NUM, T.TRANSFER_ID, :p_date_begin, :p_date_end,1)/3600,1) as v106_1,
        round({0}.SalaryOnPayType(106, T.PER_NUM, T.TRANSFER_ID, :p_date_begin, :p_date_end,0)/3600,1) as v106_2,
        round({0}.SalaryOnPayType(114, T.PER_NUM, T.TRANSFER_ID, :p_date_begin, :p_date_end,0)/3600,1) as v114,
        round({0}.SalaryOnPayType(124, T.PER_NUM, T.TRANSFER_ID, :p_date_begin, :p_date_end,0)/3600,1) as v124,
        0 as v125
    from {0}.TRANSFER T 
    where T.PER_NUM = :p_per_num 
    connect by prior T.FROM_POSITION = T.TRANSFER_ID 
    start with T.TRANSFER_ID = :p_transfer_id
    ) V1
group by per_num