select V.ABS_TIME_BEGIN, V.ABS_TIME_END, V.TYPE_ABSENCE,
    to_char(trunc(V.TIME),'FM00')||':'||to_char((V.TIME - trunc(V.TIME))*60,'FM00') TIME, round(V.TIME,2) TIME_DEC, ABSENCE_ID
from (
    select A.ABSENCE_ID, A.ABS_TIME_BEGIN, A.ABS_TIME_END, 
        DECODE(A.TYPE_ABSENCE, 1, 'Ќаработка', 'ќтгул') as TYPE_ABSENCE,
            /* ѕровер€ем условие: если документ больше 8 часов, то провер€ем на один ли он день. ≈сли на несколько дней,
                то умножаем количество целых дней на 8 и прибавл€ем остаток. ≈сли на один день, сразу считаем за 8 часов.
                ≈сли документ меньше 8 часов, то считаем его как есть. */
            case when (A.ABS_TIME_END - A.ABS_TIME_BEGIN) * 24 >= 8 then
                case when A.ABS_TIME_END - A.ABS_TIME_BEGIN > 1 
                    then TRUNC(A.ABS_TIME_END - A.ABS_TIME_BEGIN)*8 +
                        case when ((A.ABS_TIME_END - A.ABS_TIME_BEGIN) - TRUNC(A.ABS_TIME_END - A.ABS_TIME_BEGIN))*24 >= 8
                            then 8 
                            else ((A.ABS_TIME_END - A.ABS_TIME_BEGIN) - TRUNC(A.ABS_TIME_END - A.ABS_TIME_BEGIN))*24
                        end 
                    else 8 
                end
                else (A.ABS_TIME_END - A.ABS_TIME_BEGIN) * 24 
            end TIME  
    from {0}.ABSENCE A 
    where per_num = :p_per_num and 
        A.ABS_TIME_BEGIN >= 
		(
			select NVL((
                select DISTINCT FIRST_VALUE(DATE_HIRE) OVER(ORDER BY DATE_TRANSFER DESC) DATE_BEGIN_ABSENCE 
                from 
                    (select PER_NUM, DATE_HIRE, TRUNC(DATE_TRANSFER) DATE_TRANSFER, TYPE_TRANSFER_ID, TRANSFER_ID,
                        (DATE_HIRE - LAG(DATE_TRANSFER,1,null) OVER(ORDER BY DATE_HIRE, DATE_TRANSFER)) DAY_BETWEEN
                    from {0}.TRANSFER where PER_NUM = :p_per_num
                    ORDER BY DATE_TRANSFER)
                WHERE DAY_BETWEEN > 30),DATE '1001-01-01') DATE_BEGIN_ABSENCE 
            FROM DUAL)
		and extract(YEAR from A.ABS_TIME_BEGIN) between :p_year_begin and :p_year_end
    order by A.ABS_TIME_BEGIN DESC
) V