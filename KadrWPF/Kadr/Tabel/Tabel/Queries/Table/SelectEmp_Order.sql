SELECT (select S.CODE_SUBDIV from {0}.SUBDIV S where S.SUBDIV_ID = EO.SUBDIV_ID) "Подр.", 
    EO.DATE_ORDER "Дата установки", 
    LEAD(EO.DATE_ORDER-1+86399/86400,1,null) OVER(PARTITION BY EO.PER_NUM, EO.SIGN_COMB ORDER BY DATE_ORDER) "Дата окончания",
    (SELECT O.ORDER_NAME FROM {0}.ORDERS O WHERE O.ORDER_ID = EO.ORDER_ID) "Заказ"
FROM {0}.EMP_ORDER EO
WHERE EO.PER_NUM = :p_per_num and EO.TRANSFER_ID in 
    (select TR1.TRANSFER_ID from {0}.TRANSFER TR1 start with TR1.TRANSFER_ID = :p_transfer_id
    CONNECT BY NOCYCLE PRIOR TR1.transfer_id = TR1.from_position or TR1.transfer_id = PRIOR TR1.from_position)