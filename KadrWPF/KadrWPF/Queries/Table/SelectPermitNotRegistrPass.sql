WITH transfer_tree as 
        (select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = :p_transfer_id
        connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)
select count(*) COUNT_PERMIT
from (select P.PER_NUM, max(TP.DISPLACE_GRAPH) DISPLACE_GRAPH, max(TP.FREE_EXIT) FREE_EXIT, 
            max(TP.NOT_REGISTR_PASS) NOT_REGISTR_PASS,max(TP.ROUND_TIME) ROUND_TIME
        from {0}.PERMIT P
        join {0}.TYPE_PERMIT TP using (TYPE_PERMIT_ID)
        where P.TRANSFER_ID in (select * from transfer_tree)
            and :p_date between P.DATE_START_PERMIT and P.DATE_END_PERMIT
        group by P.PER_NUM) P0
where nvl(P0.NOT_REGISTR_PASS,0) = 1