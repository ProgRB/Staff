with v_table as 
(select
    per_num, sum as sum_sal, order_name
    from
    (
        select * from
        ( 
            select transfer_id, trunc(date_transfer) date_transfer, subdiv_id,
                decode(type_transfer_id, 3, trunc(date_transfer)+86399/86400, lead(trunc(date_transfer)-1/86400, 1, date'3000-01-01') over (partition by worker_id order by date_transfer)) end_transfer
            from apstaff.transfer
        )
        where date_transfer<add_months(trunc(:p_date, 'month'),1) and end_transfer>=trunc(:p_date,'month')
    ) s, 
    TABLE(apstaff.calc_salary_pipe(:p_subdiv_id, trunc(:p_date,'month'), add_months(trunc(:p_date,'month'),1)-1/86400, s.transfer_id))
    where substr(order_name,1,2) in ('20', '23')
)
select per_num, t.order_name, date_open d1, date_close d2, nvl(reason_name, 'Не существует') d3
from
    v_table t
    left join 
    (select order_book_id, code_order as order_name, date_open, date_close-1 as date_close, 'Закрыт' as reason_name from apstaff.orders_book where add_months(trunc(:p_date,'month'), 12)<date_open or trunc(:p_date,'month')>nvl(date_close-1, date'3000-01-01')
        union all
        select rownum as order_book_id, order_name as order_name, to_date(null) as date_open, date_end, 'Приостановлен' from apstaff.order_period join apstaff.orders using (order_id) where date_end<trunc(:p_date,'month') and last_day(:p_date)<nvl(date_start, date'3000-01-01')
    ) t1 on (t.order_name=t1.order_name)
where order_book_id is not null or not exists(select 1 from apstaff.orders_book where code_order=t.order_name)