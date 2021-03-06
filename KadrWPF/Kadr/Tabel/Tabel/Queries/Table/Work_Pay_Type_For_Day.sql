SELECT W.WORK_PAY_TYPE_ID, W.WORKED_DAY_ID, P.PAY_TYPE_ID, P.PAY_TYPE_NAME, round(W.VALID_TIME / 3600, 2) as VALID_TIME,
    lpad(floor(W.VALID_TIME / 3600),2,'0')||':'|| lpad(round(((round(W.VALID_TIME / 3600, 2) - floor(W.VALID_TIME / 3600)) * 60),0),2,'0') as vFormat ,
    (select O.ORDER_NAME from {0}.ORDERS O where O.ORDER_ID = W.ORDER_ID) ORDER_NAME, W.REG_DOC_ID
FROM  {0}.WORK_PAY_TYPE W 
LEFT JOIN {0}.PAY_TYPE P on (W.PAY_TYPE_ID = P.PAY_TYPE_ID) 
WHERE W.WORKED_DAY_ID = :p_worked_day_id 
ORDER BY P.CODE_PAY_TYPE