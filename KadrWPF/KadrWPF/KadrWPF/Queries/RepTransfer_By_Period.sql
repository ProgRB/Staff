select rownum, V1.*
from (
    select E.PER_NUM, E.EMP_LAST_NAME||' '||substr(E.EMP_FIRST_NAME,1,1)||'.'||substr(E.EMP_MIDDLE_NAME,1,1)||'.' FIO,
        V.COMB,V.TYPE_TR,V.DATE_TR,SUB_OLD,POS_OLD,SUB,POS
    from (
        WITH SUBD as (
            select SUBDIV_ID FROM {0}.SUBDIV
			start with subdiv_id in (
				select subdiv_id from {0}.access_subdiv 
				where upper(user_name) = ora_login_user and upper(app_name) = 'KADR' AND                                 
					SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
			connect by prior subdiv_id = parent_id
        )
        select T0.PER_NUM, decode(T0.SIGN_COMB,0,null,1,'X') COMB, 
            DECODE(T0.TYPE_TRANSFER_ID,1,'опхел',2,
                case when T0.SUBDIV_ID != T_OLD.SUBDIV_ID then 'оепебнд' else 'оепебнд (бм)' end) TYPE_TR, 
            TRUNC(T0.DATE_TRANSFER) DATE_TR, T0.TR_DATE_ORDER DATE_ORD,
            (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T_OLD.SUBDIV_ID) SUB_OLD,
            (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T_OLD.POS_ID) POS_OLD,
            (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T0.SUBDIV_ID) SUB,
            (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T0.POS_ID) POS
        from {0}.TRANSFER T0
        join (select * from SUBD) S0 on T0.SUBDIV_ID = S0.SUBDIV_ID
        left join {0}.TRANSFER T_OLD on T0.FROM_POSITION = T_OLD.TRANSFER_ID 
        where T0.DATE_TRANSFER between :p_date_begin and :p_date_end and T0.TYPE_TRANSFER_ID = 2
            and T0.SUBDIV_ID = T_OLD.SUBDIV_ID
        union
        select T0.PER_NUM, decode(T0.SIGN_COMB,0,null,1,'X') COMB, 
            DECODE(T_NEW.TYPE_TRANSFER_ID,1,'опхел',2,
                case when T0.SUBDIV_ID != T_NEW.SUBDIV_ID then 'оепебнд (хг)' else 'оепебнд (бм)' end,3,'сбнкэмемхе') TYPE_TR, 
            TRUNC(T_NEW.DATE_TRANSFER) DATE_TR, T_NEW.TR_DATE_ORDER DATE_ORD,
            (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T0.SUBDIV_ID) SUB,
            (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T0.POS_ID) POS,
            case when T_NEW.TYPE_TRANSFER_ID != 3 
                then (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = T_NEW.SUBDIV_ID)
                else null end SUB_NEW,
            case when T_NEW.TYPE_TRANSFER_ID != 3 
                then (select P.POS_NAME from {0}.POSITION P where P.POS_ID = T_NEW.POS_ID)
                else null end POS_NEW
        from {0}.TRANSFER T0
        join (select * from SUBD) S0 on T0.SUBDIV_ID = S0.SUBDIV_ID
        left join {0}.TRANSFER T_NEW on T0.TRANSFER_ID = T_NEW.FROM_POSITION
        where T_NEW.DATE_TRANSFER between :p_date_begin and :p_date_end and 
            (T0.SUBDIV_ID != T_NEW.SUBDIV_ID
            or (T0.SUBDIV_ID = T_NEW.SUBDIV_ID and T_NEW.TYPE_TRANSFER_ID = 3))
    ) V
    join {0}.EMP E on V.PER_NUM = E.PER_NUM
    order by PER_NUM
) V1