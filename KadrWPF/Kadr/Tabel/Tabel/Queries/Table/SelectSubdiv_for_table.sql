select ROWNUM as "¹ ï/ï", V.* from ( 
select 
     ST.SUBDIV_ID, (select CODE_SUBDIV from {0}.SUBDIV where SUBDIV_ID = ST.SUBDIV_ID) as CODE_SUBDIV, 
     (select SUBDIV_NAME from {0}.SUBDIV where SUBDIV_ID = ST.SUBDIV_ID) as SUBDIV_NAME, 
     ST.DATE_ADVANCE, ST.DATE_SALARY, nvl(ST.SIGN_PROCESSING,0) as SIGN_PROCESSING
from {0}.SUBDIV_FOR_TABLE ST
order by 2) V
/*select ROWNUM as "¹ ï/ï", V.* from ( 
    select 
         ST.SUBDIV_ID, (select CODE_SUBDIV from APSTAFF.SUBDIV where SUBDIV_ID = ST.SUBDIV_ID) as CODE_SUBDIV, 
         (select SUBDIV_NAME from APSTAFF.SUBDIV where SUBDIV_ID = ST.SUBDIV_ID) as SUBDIV_NAME, 
         DATE '2014-01-15' DATE_ADVANCE, DATE '2014-01-31' DATE_SALARY, 0 as SIGN_PROCESSING
    from APSTAFF.SUBDIV ST
    WHERE PARENT_ID = 0 and ST.TYPE_SUBDIV_ID not in (5,6)  
    order by 2) V*/