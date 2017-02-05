select * from (select 
    case when LV.PER_NUM is not null then 
         SU.CODE_SUBDIV
        else 
        case when LV.PERCO_SYNC_ID is not null 
        then SU.SUBDIV_NAME else 
        '' end
        end as subdiv_name,
    case when LV.PER_NUM is not null then EM.PER_NUM else '' end as per_num,
    case when LV.PER_NUM is not null then EM.EMP_LAST_NAME else 
        case when LV.PERCO_SYNC_ID is not null then FR.FR_LAST_NAME else 
        OV.OT_LAST_NAME end end as last_name,
    case when LV.PER_NUM is not null then EM.EMP_FIRST_NAME else 
        case when LV.PERCO_SYNC_ID is not null then FR.FR_FIRST_NAME else 
        OV.OT_FIRST_NAME end end as FIRST_name,
    case when LV.PER_NUM is not null then EM.EMP_MIDDLE_NAME else 
        case when LV.PERCO_SYNC_ID is not null then FR.FR_MIDDLE_NAME else 
        OV.OT_MIDDLE_NAME end end as MIDDLE_name,
    case when LV.PER_NUM is not null then 
        (select P.POS_NAME from {0}.transfer tr join {0}.position p 
                on (TR.POS_ID = P.POS_ID) 
                where Tr.PER_NUM = LV.PER_NUM and Tr.sign_cur_work = 1 and rownum = 1)
        else 
        case when LV.PERCO_SYNC_ID is not null 
        then PO.POS_NAME else 
        '' end
        end as pos_name,tr2.transfer_id, LV.VIOLATION_LOG_ID, LV.LIST_VIOLATOR_ID, LV.PERCO_SYNC_ID, LV.OTHER_VIOLATOR_ID    
from {0}.list_violator lv 
left join {0}.emp em on (LV.PER_NUM = EM.PER_NUM)
left join {0}.transfer tr2 on (tr2.PER_NUM = EM.PER_NUM and TR2.SIGN_CUR_WORK = 1)
left join {0}.subdiv su on TR2.SUBDIV_ID = SU.SUBDIV_ID
left join {0}.position po on TR2.POS_ID = PO.POS_ID
left join {0}.fr_emp fr on (LV.PERCO_SYNC_ID = FR.PERCO_SYNC_ID)
left join {0}.other_violator ov on (LV.OTHER_VIOLATOR_ID = OV.OTHER_VIOLATOR_ID)) 
emp_list order by last_name

