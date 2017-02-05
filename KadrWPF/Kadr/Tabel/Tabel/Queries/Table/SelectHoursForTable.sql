select DECODE(PAY_TYPE,102,'урочно',106,'с/ур.',114,'ночные','гидроп.') PAY_TYPE, 
    D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, MONTH_2, D16, D17, D18, D19, D20, D21, D22, 
    D23, D24, D25, D26, D27, D28, D29, D30, D31, MONTH, REMAKE, DAY102, DAYHOL, DAYABS, HOLIDAY, DAYITOG 
from 
(select
    pay_type,
    D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, MONTH_2, D16, D17, D18, D19, D20, D21, D22, 
    D23, D24, D25, D26, D27, D28, D29, D30, D31, MONTH, REMAKE, DAY102, DAYHOL, DAYABS, HOLIDAY, DAYITOG 
from {0}.TEMP_TABLE 
where temp_table_id = :p_temp_table_id and TRANSFER_ID = :p_TRANSFER_ID    
order by pay_type) V