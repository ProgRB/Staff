select 
	case when s1.pos_id=1 then (select staffs.pos_id from {0}.staffs  where staffs_id={1}) else -1  end   pos_id,
           case when s1.subdiv_id=1 then (select staffs.subdiv_id from {0}.staffs  where staffs_id={1}) else -1  end   subdiv_id,
            case when s1.degree_id=1 then (select  degree_name from {0}.staffs left join {0}.degree d on (staffs.degree_id=d.degree_id) where staffs_id={1}) else '-1'  end   degree_id,
            /* case when s1.MAX_TARIFF=1 then (select MAX_TARIFF from {0}.staffs where staffs_id={1}) else -1  end   MAX_TARIFF,*/
              case when s1.VACANT_SIGN=1 then (select distinct VACANT_SIGN from {0}.staffs where staffs_id={1}) else -1  end   VACANT_SIGN,
               case when s1.DATE_END_VACANT=1 then (select distinct to_char(DATE_END_VACANT,'DD.MM.YYYY') from {0}.staffs where staffs_id={1}) else '01.01.1000'  end   DATE_END_VACANT,
                case when s1.DATE_BEGIN_STAFF=1 then (select distinct to_char(DATE_BEGIN_STAFF,'DD.MM.YYYY') from {0}.staffs where staffs_id={1}) else '01.01.1000'  end   DATE_BEGIN_STAFF,
                 case when s1.DATE_END_STAFF=1 then (select distinct to_char(DATE_END_STAFF,'DD.MM.YYYY') from {0}.staffs where staffs_id={1}) else '01.01.1000'  end   DATE_END_STAFF,
                  case when s1.HARMFUL_ADDITION=1 then (select distinct HARMFUL_ADDITION from {0}.staffs where staffs_id={1}) else -1  end   HARMFUL_ADDITION,
                   case when s1.TYPE_PERSON=1 then (select distinct TYPE_PERSON from {0}.staffs where staffs_id={1}) else -1  end   TYPE_PERSON,
                    case when s1.COMMENT_TO_POS=1 then (select distinct COMMENT_TO_POS from {0}.staffs where staffs_id={1}) else '-1'  end   COMMENT_TO_POS,
                     case when s1.ORDER_ID=1 then (select distinct ORDER_NAME from {0}.staffs left join {0}.ORDERS o on (o.order_id=staffs.order_id) where staffs_id={1}) else '-1'  end   ORDER_ID,
                     /* case when s1.COMB_ADDITION=1 then (select distinct COMB_ADDITION from {0}.staffs where staffs_id={1}) else -1  end   COMB_ADDITION,*/
                       case when s1.TARIF_GRID_ID=1 then (select distinct tariff_grid_name from {0}.staffs left join {0}.tariff_grid tg on (staffs.tarif_grid_id=tg.tariff_grid_id)  where staffs_id={1}) else '-1'  end   TARIF_GRID_ID,
                        case when s1.TAR_BY_SCHEMA=1 then (select distinct TAR_BY_SCHEMA from {0}.staffs where staffs_id={1}) else -1  end   TAR_BY_SCHEMA,
                         case when s1.CLASSIFIC=1 then (select distinct CLASSIFIC from {0}.staffs where staffs_id={1}) else -1  end   CLASSIFIC,
                         /* case when s1.PERSONAL_TAR=1 then (select distinct PERSONAL_TAR from {0}.staffs where staffs_id={1}) else -1  end   PERSONAL_TAR,*/
                           case when s1.ADD_EXP_AREA=1 then (select distinct ADD_EXP_AREA from {0}.staffs where staffs_id={1}) else -1  end   ADD_EXP_AREA  
            
from
(            
  select
            count(DISTINCT NVL(s.pos_id,-1)) pos_id,
            count(DISTINCT NVL(s.subdiv_id,-1)) subdiv_id,            
            count(DISTINCT  nvl(s.DEGREE_ID,-1)) degree_id,  
            /*count(DISTINCT NVL(s.MAX_TARIFF,-1)) max_tariff, */      
            count(DISTINCT NVL(s.VACANT_SIGN,-1)) vacant_sign,            
            count(DISTINCT  nvl(s.DATE_END_VACANT,sysdate)) date_end_vacant,            
            count(DISTINCT NVL(DATE_BEGIN_STAFF,sysdate)) date_begin_staff,            
            count(DISTINCT NVL(s.DATE_END_STAFF,SYSDATE)) date_end_staff,            
            count(DISTINCT  nvl(s.HARMFUL_ADDITION,-1)) harmful_addition,            
            count(DISTINCT NVL(s.TYPE_PERSON,-1)) type_person,             
            count(DISTINCT NVL(s.COMMENT_TO_POS,'úÿé.')) comment_to_pos,            
            count(DISTINCT  nvl(s.ORDER_ID,-1)) order_id,            
            /*count(DISTINCT NVL(s.COMB_ADDITION,-1)) comb_addition,  */          
            count(DISTINCT NVL(s.TARIF_GRID_ID,-1)) tarif_grid_id,            
            count(DISTINCT  nvl(s.TAR_BY_SCHEMA,-1)) tar_by_schema,            
            count(DISTINCT NVL(s.CLASSIFIC,-1)) classific,            
           /* count(DISTINCT NVL(s.PERSONAL_TAR,-1)) personal_tar, */           
            count(DISTINCT  nvl(s.ADD_EXP_AREA,-1)) add_exp_area                
from (select * from {0}.staffs  where staffs_id in ({2})) s
) s1