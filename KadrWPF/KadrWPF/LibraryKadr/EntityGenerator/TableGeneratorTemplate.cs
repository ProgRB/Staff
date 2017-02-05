/***********************************************************/
/**********   Generated at 03.02.2017 11:10:07     ********/
/*********************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using Oracle.DataAccess.Client;
using System.Data.Linq.Mapping;

namespace EntityGenerator
{
    
    [Table(Name="SUBDIV_PART_TYPE"), SchemaName("APSTAFF")]
    public partial class SubdivPartType : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный год номер типа подструктуры
        /// </summary>
        [Column(Name="SUBDIV_PART_TYPE_ID", CanBeNull=false)]
        public Decimal? SubdivPartTypeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivPartTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivPartTypeID, value);
            }
        }
        /// <summary>
        /// Код типа подструктуры
        /// </summary>
        [Column(Name="SUBDIV_PART_TYPE_CODE")]
        public String SubdivPartTypeCode 
        {
            get
            {
                return this.GetDataRowField<String>(() => SubdivPartTypeCode);
            }
            set
            {
                UpdateDataRow<String>(() => SubdivPartTypeCode, value);
            }
        }
        /// <summary>
        /// Наименование типа подструктуры
        /// </summary>
        [Column(Name="SUBDIV_PART_TYPE_NAME")]
        public String SubdivPartTypeName 
        {
            get
            {
                return this.GetDataRowField<String>(() => SubdivPartTypeName);
            }
            set
            {
                UpdateDataRow<String>(() => SubdivPartTypeName, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="SUBDIV_PARTITION"), SchemaName("APSTAFF")]
    public partial class SubdivPartition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер подструктуры
        /// </summary>
        [Column(Name="SUBDIV_PARTITION_ID")]
        public Decimal? SubdivPartitionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivPartitionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivPartitionID, value);
            }
        }
        /// <summary>
        /// Ссылка на уникальный тип структуры подразделения
        /// </summary>
        [Column(Name="SUBDIV_PART_TYPE_ID", CanBeNull=false)]
        public Decimal? SubdivPartTypeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivPartTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivPartTypeID, value);
            }
        }
        /// <summary>
        /// Номер подструктуры
        /// </summary>
        [Column(Name="SUBDIV_NUMBER", CanBeNull=false)]
        public String SubdivNumber 
        {
            get
            {
                return this.GetDataRowField<String>(() => SubdivNumber);
            }
            set
            {
                UpdateDataRow<String>(() => SubdivNumber, value);
            }
        }
        /// <summary>
        /// Ссылка на родительский элемент структуры
        /// </summary>
        [Column(Name="PARENT_SUBDIV_ID")]
        public Decimal? ParentSubdivID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ParentSubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ParentSubdivID, value);
            }
        }
        /// <summary>
        /// Подразделение которому принадлежит струкутра
        /// </summary>
        [Column(Name="SUBDIV_ID", CanBeNull=false)]
        public Decimal? SubdivID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivID, value);
            }
        }
        /// <summary>
        /// Дата начала действия структуры
        /// </summary>
        [Column(Name="DATE_START_SUBDIV_PART", CanBeNull=false)]
        public DateTime? DateStartSubdivPart 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateStartSubdivPart);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateStartSubdivPart, value);
            }
        }
        /// <summary>
        /// Дата окончания действия структуры
        /// </summary>
        [Column(Name="DATE_END_SUBDIV_PART")]
        public DateTime? DateEndSubdivPart 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateEndSubdivPart);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateEndSubdivPart, value);
            }
        }
        /// <summary>
        /// Наименование структурного подразделения
        /// </summary>
        [Column(Name="SUBDIV_PART_NAME", CanBeNull=false)]
        public String SubdivPartName 
        {
            get
            {
                return this.GetDataRowField<String>(() => SubdivPartName);
            }
            set
            {
                UpdateDataRow<String>(() => SubdivPartName, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="INDIVID_PROTECTION"), SchemaName("APSTAFF")]
    public partial class IndividProtection : RowEntityBase
    {
        #region Class Members
        [Column(Name="INDIVID_PROTECTION_ID", CanBeNull=false)]
        public Decimal? IndividProtectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => IndividProtectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => IndividProtectionID, value);
            }
        }
        [Column(Name="CODE_PROTECTION", CanBeNull=false)]
        public String CodeProtection 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeProtection);
            }
            set
            {
                UpdateDataRow<String>(() => CodeProtection, value);
            }
        }
        [Column(Name="NAME_PROTECTION")]
        public String NameProtection 
        {
            get
            {
                return this.GetDataRowField<String>(() => NameProtection);
            }
            set
            {
                UpdateDataRow<String>(() => NameProtection, value);
            }
        }
        [Column(Name="TYPE_INDIVID_PROTECTION_ID", CanBeNull=false)]
        public Decimal? TypeIndividProtectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeIndividProtectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeIndividProtectionID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TYPE_INDIVID_PROTECTION"), SchemaName("APSTAFF")]
    public partial class TypeIndividProtection : RowEntityBase
    {
        #region Class Members
        [Column(Name="TYPE_INDIVID_PROTECTION_ID", CanBeNull=false)]
        public Decimal? TypeIndividProtectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeIndividProtectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeIndividProtectionID, value);
            }
        }
        [Column(Name="TYPE_PROTECTION_NAME", CanBeNull=false)]
        public String TypeProtectionName 
        {
            get
            {
                return this.GetDataRowField<String>(() => TypeProtectionName);
            }
            set
            {
                UpdateDataRow<String>(() => TypeProtectionName, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="WORK_PLACE"), SchemaName("APSTAFF")]
    public partial class WorkPlace : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер рабочего места
        /// </summary>
        [Column(Name="WORK_PLACE_ID")]
        public Decimal? WorkPlaceID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceID, value);
            }
        }
        /// <summary>
        /// Ссылка на структурное деление
        /// </summary>
        [Column(Name="SUBDIV_ID", CanBeNull=false)]
        public Decimal? SubdivID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivID, value);
            }
        }
        /// <summary>
        /// Ссылка на должность
        /// </summary>
        [Column(Name="POS_ID", CanBeNull=false)]
        public Decimal? PosID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PosID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PosID, value);
            }
        }
        /// <summary>
        /// Кол-во работников на рабочем месте
        /// </summary>
        [Column(Name="WORKER_COUNT", CanBeNull=false)]
        public Decimal? WorkerCount 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkerCount);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkerCount, value);
            }
        }
        /// <summary>
        /// Признак повышенной оплаты труда
        /// </summary>
        [Column(Name="HIGH_SALARY_SIGN", CanBeNull=false)]
        public Decimal? HighSalarySign 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HighSalarySign);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HighSalarySign, value);
            }
        }
        /// <summary>
        /// Признак доп. отпуска
        /// </summary>
        [Column(Name="ADDITION_VAC_SIGN", CanBeNull=false)]
        public Decimal? AdditionVacSign 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => AdditionVacSign);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => AdditionVacSign, value);
            }
        }
        /// <summary>
        /// Признак сокр. дня работы
        /// </summary>
        [Column(Name="SHORT_WORK_DAY_SIGN", CanBeNull=false)]
        public Decimal? ShortWorkDaySign 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ShortWorkDaySign);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ShortWorkDaySign, value);
            }
        }
        /// <summary>
        /// Признак молочника
        /// </summary>
        [Column(Name="MILK_SIGN", CanBeNull=false)]
        public Decimal? MilkSign 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => MilkSign);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => MilkSign, value);
            }
        }
        /// <summary>
        /// Период мед. проверки (месяцы)
        /// </summary>
        [Column(Name="MED_CHECKUP_PERIOD", CanBeNull=false)]
        public Decimal? MedCheckupPeriod 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => MedCheckupPeriod);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => MedCheckupPeriod, value);
            }
        }
        /// <summary>
        /// Номер карты условий труда
        /// </summary>
        [Column(Name="WORK_PLACE_NUM")]
        public String WorkPlaceNum 
        {
            get
            {
                return this.GetDataRowField<String>(() => WorkPlaceNum);
            }
            set
            {
                UpdateDataRow<String>(() => WorkPlaceNum, value);
            }
        }
        /// <summary>
        /// Признак льготного пенсионного обеспечения
        /// </summary>
        [Column(Name="SIGN_PREFERENTIAL_PENS")]
        public Decimal? SignPreferentialPens 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SignPreferentialPens);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SignPreferentialPens, value);
            }
        }
        /// <summary>
        /// Ссылка на список льготных профессий
        /// </summary>
        [Column(Name="PRIVILEGED_POSITION_ID")]
        public Decimal? PrivilegedPositionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PrivilegedPositionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PrivilegedPositionID, value);
            }
        }
        /// <summary>
        /// Номер приказа о СОУТ
        /// </summary>
        [Column(Name="WORK_PLACE_ORDER")]
        public String WorkPlaceOrder 
        {
            get
            {
                return this.GetDataRowField<String>(() => WorkPlaceOrder);
            }
            set
            {
                UpdateDataRow<String>(() => WorkPlaceOrder, value);
            }
        }
        /// <summary>
        /// Дата приказа о СОУТ
        /// </summary>
        [Column(Name="WORK_PLACE_DATE")]
        public DateTime? WorkPlaceDate 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => WorkPlaceDate);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => WorkPlaceDate, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="WORK_PLACE_CONDITION"), SchemaName("APSTAFF")]
    public partial class WorkPlaceCondition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер связи условий труда и места
        /// </summary>
        [Column(Name="WORK_PLACE_CONDITION_ID", CanBeNull=false)]
        public Decimal? WorkPlaceConditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceConditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceConditionID, value);
            }
        }
        /// <summary>
        /// Ссылка на условия труда
        /// </summary>
        [Column(Name="WORK_PLACE_ID", CanBeNull=false)]
        public Decimal? WorkPlaceID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceID, value);
            }
        }
        /// <summary>
        /// Ссылка на тип условий
        /// </summary>
        [Column(Name="TYPE_CONDITION_ID", CanBeNull=false)]
        public Decimal? TypeConditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeConditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeConditionID, value);
            }
        }
        /// <summary>
        /// Ссылка на значения условий труда
        /// </summary>
        [Column(Name="CONDITIONS_OF_WORK_ID", CanBeNull=false)]
        public Decimal? ConditionsOfWorkID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ConditionsOfWorkID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ConditionsOfWorkID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="WORK_PLACE_PROTECTION"), SchemaName("APSTAFF")]
    public partial class WorkPlaceProtection : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер связи средств защиты и места
        /// </summary>
        [Column(Name="WORK_PLACE_PROTECTION_ID", CanBeNull=false)]
        public Decimal? WorkPlaceProtectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceProtectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceProtectionID, value);
            }
        }
        /// <summary>
        /// Ссылка на условия труда
        /// </summary>
        [Column(Name="WORK_PLACE_ID", CanBeNull=false)]
        public Decimal? WorkPlaceID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceID, value);
            }
        }
        /// <summary>
        /// Ссылка на тип СИЗ
        /// </summary>
        [Column(Name="INDIVID_PROTECTION_ID", CanBeNull=false)]
        public Decimal? IndividProtectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => IndividProtectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => IndividProtectionID, value);
            }
        }
        /// <summary>
        /// Период использования СИЗ в мес.
        /// </summary>
        [Column(Name="PERIOD_FOR_USE", CanBeNull=false)]
        public Decimal? PeriodForUse 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PeriodForUse);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PeriodForUse, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="PROF_STANDART"), SchemaName("APSTAFF")]
    public partial class ProfStandart : RowEntityBase
    {
        #region Class Members
        [Column(Name="PROF_STANDART_ID", CanBeNull=false)]
        public Decimal? ProfStandartID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ProfStandartID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ProfStandartID, value);
            }
        }
        /// <summary>
        /// Код профессионального стандарта
        /// </summary>
        [Column(Name="CODE_PROF")]
        public String CodeProf 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeProf);
            }
            set
            {
                UpdateDataRow<String>(() => CodeProf, value);
            }
        }
        /// <summary>
        /// Область профессии
        /// </summary>
        [Column(Name="PROF_AREA")]
        public String ProfArea 
        {
            get
            {
                return this.GetDataRowField<String>(() => ProfArea);
            }
            set
            {
                UpdateDataRow<String>(() => ProfArea, value);
            }
        }
        /// <summary>
        /// Тип проф стандарта
        /// </summary>
        [Column(Name="TYPE_PROF_NAME")]
        public String TypeProfName 
        {
            get
            {
                return this.GetDataRowField<String>(() => TypeProfName);
            }
            set
            {
                UpdateDataRow<String>(() => TypeProfName, value);
            }
        }
        /// <summary>
        /// Наименование профессионального стандарта
        /// </summary>
        [Column(Name="PROF_STANDART_NAME")]
        public String ProfStandartName 
        {
            get
            {
                return this.GetDataRowField<String>(() => ProfStandartName);
            }
            set
            {
                UpdateDataRow<String>(() => ProfStandartName, value);
            }
        }
        /// <summary>
        /// Номер приказа
        /// </summary>
        [Column(Name="ORDER_NUM")]
        public String OrderNum 
        {
            get
            {
                return this.GetDataRowField<String>(() => OrderNum);
            }
            set
            {
                UpdateDataRow<String>(() => OrderNum, value);
            }
        }
        /// <summary>
        /// Дата приказа
        /// </summary>
        [Column(Name="ORDER_DATE")]
        public DateTime? OrderDate 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => OrderDate);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => OrderDate, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="PRIVILEGED_POSITION"), SchemaName("APSTAFF")]
    public partial class PrivilegedPosition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор льготной профессии по подразделению;
        /// </summary>
        [Column(Name="PRIVILEGED_POSITION_ID", CanBeNull=false)]
        public Decimal? PrivilegedPositionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PrivilegedPositionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PrivilegedPositionID, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор подразделения;
        /// </summary>
        [Column(Name="SUBDIV_ID")]
        public Decimal? SubdivID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivID, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор профессии;
        /// </summary>
        [Column(Name="POS_ID")]
        public Decimal? PosID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PosID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PosID, value);
            }
        }
        /// <summary>
        /// Особые условия;
        /// </summary>
        [Column(Name="SPECIAL_CONDITIONS")]
        public String SpecialConditions 
        {
            get
            {
                return this.GetDataRowField<String>(() => SpecialConditions);
            }
            set
            {
                UpdateDataRow<String>(() => SpecialConditions, value);
            }
        }
        /// <summary>
        /// КПС;
        /// </summary>
        [Column(Name="KPS")]
        public String Kps 
        {
            get
            {
                return this.GetDataRowField<String>(() => Kps);
            }
            set
            {
                UpdateDataRow<String>(() => Kps, value);
            }
        }
        /// <summary>
        /// Номер списка;
        /// </summary>
        [Column(Name="NUMBER_LIST")]
        public Decimal? NumberList 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => NumberList);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => NumberList, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="CONDITIONS_OF_WORK"), SchemaName("APSTAFF")]
    public partial class ConditionsOfWork : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор подкласса условий труда;
        /// </summary>
        [Column(Name="CONDITIONS_OF_WORK_ID", CanBeNull=false)]
        public Decimal? ConditionsOfWorkID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ConditionsOfWorkID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ConditionsOfWorkID, value);
            }
        }
        /// <summary>
        /// Номер подкласса;
        /// </summary>
        [Column(Name="SUBCLASS_NUMBER")]
        public String SubclassNumber 
        {
            get
            {
                return this.GetDataRowField<String>(() => SubclassNumber);
            }
            set
            {
                UpdateDataRow<String>(() => SubclassNumber, value);
            }
        }
        /// <summary>
        /// Признак расчета льготного стажа;
        /// </summary>
        [Column(Name="SIGN_CALC_PREF_WORK_EXP")]
        public Int16? SignCalcPrefWorkExp 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignCalcPrefWorkExp);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignCalcPrefWorkExp, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TYPE_CONDITION"), SchemaName("APSTAFF")]
    public partial class TypeCondition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор типа условий труда;
        /// </summary>
        [Column(Name="TYPE_CONDITION_ID", CanBeNull=false)]
        public Decimal? TypeConditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeConditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeConditionID, value);
            }
        }
        /// <summary>
        /// Наименование типа условий труда;
        /// </summary>
        [Column(Name="TYPE_CONDITION_NAME")]
        public String TypeConditionName 
        {
            get
            {
                return this.GetDataRowField<String>(() => TypeConditionName);
            }
            set
            {
                UpdateDataRow<String>(() => TypeConditionName, value);
            }
        }
        /// <summary>
        /// Родитель;
        /// </summary>
        [Column(Name="PARENT_ID")]
        public Decimal? ParentID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ParentID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ParentID, value);
            }
        }
        /// <summary>
        /// Признак главного подкласса;
        /// </summary>
        [Column(Name="SIGN_MAIN_TYPE")]
        public Decimal? SignMainType 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SignMainType);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SignMainType, value);
            }
        }
        /// <summary>
        /// Порядок сортировки в договоре;
        /// </summary>
        [Column(Name="ORDER_FOR_CONTRACT")]
        public Decimal? OrderForContract 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => OrderForContract);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => OrderForContract, value);
            }
        }
        /// <summary>
        /// Порядок сортировки при редактировании;
        /// </summary>
        [Column(Name="ORDER_FOR_EDIT")]
        public Decimal? OrderForEdit 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => OrderForEdit);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => OrderForEdit, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="POSITION"), SchemaName("APSTAFF")]
    public partial class Position : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор должности;
        /// </summary>
        [Column(Name="POS_ID", CanBeNull=false)]
        public Decimal? PosID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PosID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PosID, value);
            }
        }
        /// <summary>
        /// Код должности; Текущий шифр профессии;
        /// </summary>
        [Column(Name="CODE_POS")]
        public String CodePos 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodePos);
            }
            set
            {
                UpdateDataRow<String>(() => CodePos, value);
            }
        }
        /// <summary>
        /// Наименование должности; Текущее наименование профессии;
        /// </summary>
        [Column(Name="POS_NAME")]
        public String PosName 
        {
            get
            {
                return this.GetDataRowField<String>(() => PosName);
            }
            set
            {
                UpdateDataRow<String>(() => PosName, value);
            }
        }
        /// <summary>
        /// Признак актуальности; Признак актуальности должности (если равен 1, то должность актуальна на данный момент, то есть действительна, а если 0, то нет);
        /// </summary>
        [Column(Name="POS_ACTUAL_SIGN")]
        public Int16? PosActualSign 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => PosActualSign);
            }
            set
            {
                UpdateDataRow<Int16?>(() => PosActualSign, value);
            }
        }
        /// <summary>
        /// Дата введения должности; Дата введения должности в действие;
        /// </summary>
        [Column(Name="POS_DATE_START")]
        public DateTime? PosDateStart 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => PosDateStart);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => PosDateStart, value);
            }
        }
        /// <summary>
        /// Дата окончания действия данной должности;
        /// </summary>
        [Column(Name="POS_DATE_END")]
        public DateTime? PosDateEnd 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => PosDateEnd);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => PosDateEnd, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор должности;
        /// </summary>
        [Column(Name="FROM_POS_ID")]
        public Decimal? FromPosID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => FromPosID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => FromPosID, value);
            }
        }
        /// <summary>
        /// Признак руководителя или зама;
        /// </summary>
        [Column(Name="POS_CHIEF_OR_DEPUTY_SIGN")]
        public Int16? PosChiefOrDeputySign 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => PosChiefOrDeputySign);
            }
            set
            {
                UpdateDataRow<Int16?>(() => PosChiefOrDeputySign, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="DEGREE"), SchemaName("APSTAFF")]
    public partial class Degree : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор категории (сотрудника или тарифной ставки);
        /// </summary>
        [Column(Name="DEGREE_ID", CanBeNull=false)]
        public Decimal? DegreeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => DegreeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => DegreeID, value);
            }
        }
        /// <summary>
        /// Шифр категории;
        /// </summary>
        [Column(Name="CODE_DEGREE")]
        public String CodeDegree 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeDegree);
            }
            set
            {
                UpdateDataRow<String>(() => CodeDegree, value);
            }
        }
        /// <summary>
        /// Наименование категории; Наименование категории (сотрудника или тарифной ставки);
        /// </summary>
        [Column(Name="DEGREE_NAME")]
        public String DegreeName 
        {
            get
            {
                return this.GetDataRowField<String>(() => DegreeName);
            }
            set
            {
                UpdateDataRow<String>(() => DegreeName, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="FORM_OPERATION"), SchemaName("APSTAFF")]
    public partial class FormOperation : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор вида производства;
        /// </summary>
        [Column(Name="FORM_OPERATION_ID", CanBeNull=false)]
        public Decimal? FormOperationID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => FormOperationID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => FormOperationID, value);
            }
        }
        /// <summary>
        /// Наименование вида производства;
        /// </summary>
        [Column(Name="NAME_FORM_OPERATION")]
        public String NameFormOperation 
        {
            get
            {
                return this.GetDataRowField<String>(() => NameFormOperation);
            }
            set
            {
                UpdateDataRow<String>(() => NameFormOperation, value);
            }
        }
        /// <summary>
        /// Шифр вида производства;
        /// </summary>
        [Column(Name="CODE_FORM_OPERATION")]
        public String CodeFormOperation 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeFormOperation);
            }
            set
            {
                UpdateDataRow<String>(() => CodeFormOperation, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="WORKING_TIME"), SchemaName("APSTAFF")]
    public partial class WorkingTime : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор типа продолжительности рабочего времени;
        /// </summary>
        [Column(Name="WORKING_TIME_ID", CanBeNull=false)]
        public Decimal? WorkingTimeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkingTimeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkingTimeID, value);
            }
        }
        /// <summary>
        /// Наименование типа продолжительности рабочего времени;
        /// </summary>
        [Column(Name="WORKING_TIME_NAME")]
        public String WorkingTimeName 
        {
            get
            {
                return this.GetDataRowField<String>(() => WorkingTimeName);
            }
            set
            {
                UpdateDataRow<String>(() => WorkingTimeName, value);
            }
        }
        /// <summary>
        /// Шаблон продолжительности рабочего времени;
        /// </summary>
        [Column(Name="WORKING_TIME_PATTERN")]
        public String WorkingTimePattern 
        {
            get
            {
                return this.GetDataRowField<String>(() => WorkingTimePattern);
            }
            set
            {
                UpdateDataRow<String>(() => WorkingTimePattern, value);
            }
        }
        /// <summary>
        /// Признак ввода кол-ва часов;
        /// </summary>
        [Column(Name="SIGN_NUMBER_OF_HOURS")]
        public Int16? SignNumberOfHours 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignNumberOfHours);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignNumberOfHours, value);
            }
        }
        /// <summary>
        /// Номер для документов (ТД, Доп.согл.);
        /// </summary>
        [Column(Name="NUMBER_FOR_ORDER")]
        public Decimal? NumberForOrder 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => NumberForOrder);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => NumberForOrder, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TYPE_EDU"), SchemaName("APSTAFF")]
    public partial class TypeEdu : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор вида образования;
        /// </summary>
        [Column(Name="TYPE_EDU_ID", CanBeNull=false)]
        public Decimal? TypeEduID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeEduID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeEduID, value);
            }
        }
        /// <summary>
        /// Вид образования; Вид образования (высшее, среднее и т.п.);
        /// </summary>
        [Column(Name="TE_NAME")]
        public String TeName 
        {
            get
            {
                return this.GetDataRowField<String>(() => TeName);
            }
            set
            {
                UpdateDataRow<String>(() => TeName, value);
            }
        }
        /// <summary>
        /// Приоритер вида образования для отчета; Приоритет используется при формировании отчетов;
        /// </summary>
        [Column(Name="TE_PRIORITY")]
        public Int16? TePriority 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => TePriority);
            }
            set
            {
                UpdateDataRow<Int16?>(() => TePriority, value);
            }
        }
        /// <summary>
        /// Приоритет вида образования;
        /// </summary>
        [Column(Name="TYPE_EDU_PRIOR")]
        public Decimal? TypeEduPrior 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeEduPrior);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeEduPrior, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="BASE_TARIFF"), SchemaName("APSTAFF")]
    public partial class BaseTariff : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Column(Name="BASE_TARIFF_ID", CanBeNull=false)]
        public Decimal? BaseTariffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => BaseTariffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => BaseTariffID, value);
            }
        }
        /// <summary>
        /// Дата начала действия
        /// </summary>
        [Column(Name="DATE_BEGIN")]
        public DateTime? DateBegin 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateBegin);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateBegin, value);
            }
        }
        /// <summary>
        /// Дата окончания действия
        /// </summary>
        [Column(Name="DATE_END")]
        public DateTime? DateEnd 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateEnd);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateEnd, value);
            }
        }
        /// <summary>
        /// Размеры базы коэффициента
        /// </summary>
        [Column(Name="TARIFF_VALUE")]
        public Decimal? TariffValue 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TariffValue);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TariffValue, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="GR_WORK"), SchemaName("APSTAFF")]
    public partial class GrWork : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор графика работы;
        /// </summary>
        [Column(Name="GR_WORK_ID", CanBeNull=false)]
        public Decimal? GrWorkID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => GrWorkID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => GrWorkID, value);
            }
        }
        /// <summary>
        /// Имя графика работы;
        /// </summary>
        [Column(Name="GR_WORK_NAME")]
        public String GrWorkName 
        {
            get
            {
                return this.GetDataRowField<String>(() => GrWorkName);
            }
            set
            {
                UpdateDataRow<String>(() => GrWorkName, value);
            }
        }
        /// <summary>
        /// Количество дней в графике;
        /// </summary>
        [Column(Name="COUNT_DAY")]
        public Decimal? CountDay 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CountDay);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CountDay, value);
            }
        }
        /// <summary>
        /// Признак работы графика в выходной день;
        /// </summary>
        [Column(Name="SIGN_HOLIDAY_WORK")]
        public Int16? SignHolidayWork 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignHolidayWork);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignHolidayWork, value);
            }
        }
        /// <summary>
        /// Признак работы графика в сокращенный день;
        /// </summary>
        [Column(Name="SIGN_COMPACT_DAY_WORK")]
        public Int16? SignCompactDayWork 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignCompactDayWork);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignCompactDayWork, value);
            }
        }
        /// <summary>
        /// Признак плавающего графика;
        /// </summary>
        [Column(Name="SIGN_FLOATING")]
        public Int16? SignFloating 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignFloating);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignFloating, value);
            }
        }
        [Column(Name="COMPACT_TIME_ZONE_ID")]
        public Decimal? CompactTimeZoneID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CompactTimeZoneID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CompactTimeZoneID, value);
            }
        }
        [Column(Name="HOLIDAY_TIME_ZONE_ID")]
        public Decimal? HolidayTimeZoneID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HolidayTimeZoneID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HolidayTimeZoneID, value);
            }
        }
        /// <summary>
        /// Количество часов по норме;
        /// </summary>
        [Column(Name="HOURS_FOR_NORM")]
        public Decimal? HoursForNorm 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HoursForNorm);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HoursForNorm, value);
            }
        }
        /// <summary>
        /// Часы обеденного перерыва;
        /// </summary>
        [Column(Name="HOURS_DINNER")]
        public Decimal? HoursDinner 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HoursDinner);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HoursDinner, value);
            }
        }
        /// <summary>
        /// Признак суммированного учета времени;
        /// </summary>
        [Column(Name="SIGN_SUMMARIZE")]
        public Int16? SignSummarize 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignSummarize);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignSummarize, value);
            }
        }
        /// <summary>
        /// Количество часов в день по графику;
        /// </summary>
        [Column(Name="HOURS_FOR_GRAPH")]
        public Decimal? HoursForGraph 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HoursForGraph);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HoursForGraph, value);
            }
        }
        /// <summary>
        /// Дата окончания действия графика;
        /// </summary>
        [Column(Name="DATE_END_GRAPH")]
        public DateTime? DateEndGraph 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateEndGraph);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateEndGraph, value);
            }
        }
        /// <summary>
        /// Признак сокращенного графика; Признак сокр. графика исп. для начисления 246 в.о.;
        /// </summary>
        [Column(Name="SIGN_SHORTEN")]
        public Int16? SignShorten 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignShorten);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignShorten, value);
            }
        }
        /// <summary>
        /// Количество часов по норме по календарю;
        /// </summary>
        [Column(Name="HOURS_NORM_CALENDAR")]
        public Decimal? HoursNormCalendar 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => HoursNormCalendar);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => HoursNormCalendar, value);
            }
        }
        /// <summary>
        /// Признак сменного графика; Признак бла бла бл
        /// </summary>
        [Column(Name="SIGN_SHIFTMAN")]
        public Int16? SignShiftman 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SignShiftman);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SignShiftman, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TARIFF_GRID_SALARY"), SchemaName("APSTAFF")]
    public partial class TariffGridSalary : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор тарифной сетки
        /// </summary>
        [Column(Name="TARIFF_GRID_ID", CanBeNull=false)]
        public Decimal? TariffGridID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TariffGridID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TariffGridID, value);
            }
        }
        /// <summary>
        /// Код тар. сетки
        /// </summary>
        [Column(Name="CODE_TARIFF_GRID")]
        public String CodeTariffGrid 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeTariffGrid);
            }
            set
            {
                UpdateDataRow<String>(() => CodeTariffGrid, value);
            }
        }
        /// <summary>
        /// Разряд сетки
        /// </summary>
        [Column(Name="TAR_CLASSIF")]
        public String TarClassif 
        {
            get
            {
                return this.GetDataRowField<String>(() => TarClassif);
            }
            set
            {
                UpdateDataRow<String>(() => TarClassif, value);
            }
        }
        /// <summary>
        /// Коэффициент сетки
        /// </summary>
        [Column(Name="TAR_SAL")]
        public Decimal? TarSal 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TarSal);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TarSal, value);
            }
        }
        /// <summary>
        /// Часовая ставка
        /// </summary>
        [Column(Name="TAR_HOUR")]
        public Decimal? TarHour 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TarHour);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TarHour, value);
            }
        }
        /// <summary>
        /// Оклад за месяц
        /// </summary>
        [Column(Name="TAR_MONTH")]
        public Decimal? TarMonth 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TarMonth);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TarMonth, value);
            }
        }
        /// <summary>
        /// Дата начала действия тарифа
        /// </summary>
        [Column(Name="TAR_DATE")]
        public DateTime? TarDate 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => TarDate);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => TarDate, value);
            }
        }
        /// <summary>
        /// Дата окончания действия тарифа
        /// </summary>
        [Column(Name="TARIFF_END_DATE")]
        public DateTime? TariffEndDate 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => TariffEndDate);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => TariffEndDate, value);
            }
        }
        /// <summary>
        /// Коэффициент сетки
        /// </summary>
        [Column(Name="CAR_ID")]
        public Decimal? CarID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CarID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CarID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TYPE_VAC"), SchemaName("APSTAFF")]
    public partial class TypeVac : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор типа отпуска;
        /// </summary>
        [Column(Name="TYPE_VAC_ID", CanBeNull=false)]
        public Decimal? TypeVacID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeVacID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeVacID, value);
            }
        }
        /// <summary>
        /// Вид отпуска;
        /// </summary>
        [Column(Name="NAME_VAC")]
        public String NameVac 
        {
            get
            {
                return this.GetDataRowField<String>(() => NameVac);
            }
            set
            {
                UpdateDataRow<String>(() => NameVac, value);
            }
        }
        /// <summary>
        /// Код отпуска;
        /// </summary>
        [Column(Name="CODE_VAC")]
        public String CodeVac 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeVac);
            }
            set
            {
                UpdateDataRow<String>(() => CodeVac, value);
            }
        }
        /// <summary>
        /// Краткое наименование;
        /// </summary>
        [Column(Name="SHORT_NAME_VAC")]
        public String ShortNameVac 
        {
            get
            {
                return this.GetDataRowField<String>(() => ShortNameVac);
            }
            set
            {
                UpdateDataRow<String>(() => ShortNameVac, value);
            }
        }
        /// <summary>
        /// Количество дней положенных в год;
        /// </summary>
        [Column(Name="COUNT_DAYS_IN_YEAR")]
        public Decimal? CountDaysInYear 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CountDaysInYear);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CountDaysInYear, value);
            }
        }
        /// <summary>
        /// Идентификатор тип расчета отпуска;
        /// </summary>
        [Column(Name="TYPE_VAC_CALC_ID")]
        public Decimal? TypeVacCalcID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeVacCalcID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeVacCalcID, value);
            }
        }
        /// <summary>
        /// Порядковый номер вычисления отпуска
        /// </summary>
        [Column(Name="NUMBER_CALC")]
        public Decimal? NumberCalc 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => NumberCalc);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => NumberCalc, value);
            }
        }
        /// <summary>
        /// Количество дней для расчета периода
        /// </summary>
        [Column(Name="CALC_PERIOD_DAYS")]
        public Decimal? CalcPeriodDays 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CalcPeriodDays);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CalcPeriodDays, value);
            }
        }
        /// <summary>
        /// Признак сброса оправдательных в табель после закрытия;
        /// </summary>
        [Column(Name="NEED_REG_DOC", CanBeNull=false)]
        public Int16? NeedRegDoc 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => NeedRegDoc);
            }
            set
            {
                UpdateDataRow<Int16?>(() => NeedRegDoc, value);
            }
        }
        /// <summary>
        /// Признак, показывать ли отдельной запиской-рассчетом отпуск;
        /// </summary>
        [Column(Name="ACCOUNT_NOTE_SIGN")]
        public Decimal? AccountNoteSign 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => AccountNoteSign);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => AccountNoteSign, value);
            }
        }
        /// <summary>
        /// Основание предоставления отпуска;Основание отпуска;
        /// </summary>
        [Column(Name="VAC_REASON")]
        public String VacReason 
        {
            get
            {
                return this.GetDataRowField<String>(() => VacReason);
            }
            set
            {
                UpdateDataRow<String>(() => VacReason, value);
            }
        }
        /// <summary>
        /// Битовый набор смежности периодов отпусков;
        /// </summary>
        [Column(Name="VAC_GROUP_BITSET")]
        public Decimal? VacGroupBitset 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => VacGroupBitset);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => VacGroupBitset, value);
            }
        }
        /// <summary>
        /// Признак печати отпуска в записке рассчет в таблице доп. отпуск;
        /// </summary>
        [Column(Name="SIGN_BLOCK_NOTE_ACCOUNT")]
        public Decimal? SignBlockNoteAccount 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SignBlockNoteAccount);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SignBlockNoteAccount, value);
            }
        }
        /// <summary>
        /// Признак оплаты - отпуск оплачивается;
        /// </summary>
        [Column(Name="SING_PAYMENT")]
        public Decimal? SingPayment 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SingPayment);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SingPayment, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="VAC_GROUP_TYPE"), SchemaName("APSTAFF")]
    public partial class VacGroupType : RowEntityBase
    {
        #region Class Members
        [Column(Name="VAC_GROUP_TYPE_ID", CanBeNull=false)]
        public Decimal? VacGroupTypeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => VacGroupTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => VacGroupTypeID, value);
            }
        }
        /// <summary>
        /// Наименование группы отпусков сокращенное
        /// </summary>
        [Column(Name="GROUP_VAC_NAME")]
        public String GroupVacName 
        {
            get
            {
                return this.GetDataRowField<String>(() => GroupVacName);
            }
            set
            {
                UpdateDataRow<String>(() => GroupVacName, value);
            }
        }
        /// <summary>
        /// Уникальный номер метода расчета периода отпусков
        /// </summary>
        [Column(Name="TYPE_VAC_CALC_PERIOD_ID")]
        public Decimal? TypeVacCalcPeriodID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeVacCalcPeriodID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeVacCalcPeriodID, value);
            }
        }
        /// <summary>
        /// Требуется ли период для типа отпусков
        /// </summary>
        [Column(Name="NEED_PERIOD")]
        public Int16? NeedPeriod 
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => NeedPeriod);
            }
            set
            {
                UpdateDataRow<Int16?>(() => NeedPeriod, value);
            }
        }
        /// <summary>
        /// Кол-во дней для расчета периода за 1 год
        /// </summary>
        [Column(Name="COUNT_PERIOD_DAY")]
        public Decimal? CountPeriodDay 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => CountPeriodDay);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => CountPeriodDay, value);
            }
        }
        /// <summary>
        /// Полное наименование группы отпуска
        /// </summary>
        [Column(Name="GROUP_VAC_FULL_NAME")]
        public String GroupVacFullName 
        {
            get
            {
                return this.GetDataRowField<String>(() => GroupVacFullName);
            }
            set
            {
                UpdateDataRow<String>(() => GroupVacFullName, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="STAFF"), SchemaName("APSTAFF")]
    public partial class Staff : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный ключ;
        /// </summary>
        [Column(Name="STAFF_ID", CanBeNull=false)]
        public Decimal? StaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffID, value);
            }
        }
        /// <summary>
        /// Идентификатор профессии;
        /// </summary>
        [Column(Name="POS_ID", CanBeNull=false)]
        public Decimal? PosID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PosID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PosID, value);
            }
        }
        /// <summary>
        /// Подразделение;
        /// </summary>
        [Column(Name="SUBDIV_ID", CanBeNull=false)]
        public Decimal? SubdivID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivID, value);
            }
        }
        /// <summary>
        /// Идентификатор категории;
        /// </summary>
        [Column(Name="DEGREE_ID", CanBeNull=false)]
        public Decimal? DegreeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => DegreeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => DegreeID, value);
            }
        }
        /// <summary>
        /// комментарий к профессии, для 3 отдела;
        /// </summary>
        [Column(Name="COMMENTS")]
        public String Comments 
        {
            get
            {
                return this.GetDataRowField<String>(() => Comments);
            }
            set
            {
                UpdateDataRow<String>(() => Comments, value);
            }
        }
        /// <summary>
        /// Номер заказа;
        /// </summary>
        [Column(Name="ORDER_ID")]
        public Decimal? OrderID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => OrderID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => OrderID, value);
            }
        }
        /// <summary>
        /// Тарифная сетка;
        /// </summary>
        [Column(Name="TARIFF_GRID_ID")]
        public Decimal? TariffGridID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TariffGridID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TariffGridID, value);
            }
        }
        /// <summary>
        /// Тариф по схеме;
        /// </summary>
        [Column(Name="TAR_BY_SCHEMA")]
        public Decimal? TarBySchema 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TarBySchema);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TarBySchema, value);
            }
        }
        /// <summary>
        /// Разряд;Разряд штатной единицы;
        /// </summary>
        [Column(Name="CLASSIFIC")]
        public Decimal? Classific 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => Classific);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => Classific, value);
            }
        }
        /// <summary>
        /// Раздел штатного расписания
        /// </summary>
        [Column(Name="STAFF_SECTION_ID")]
        public Decimal? StaffSectionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffSectionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffSectionID, value);
            }
        }
        /// <summary>
        /// Структурное подразделение штатной единицы
        /// </summary>
        [Column(Name="SUBDIV_PARTITION_ID")]
        public Decimal? SubdivPartitionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SubdivPartitionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SubdivPartitionID, value);
            }
        }
        /// <summary>
        /// Вид производства
        /// </summary>
        [Column(Name="FORM_OPERATION_ID", CanBeNull=false)]
        public Decimal? FormOperationID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => FormOperationID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => FormOperationID, value);
            }
        }
        /// <summary>
        /// Коментарий к профессии
        /// </summary>
        [Column(Name="POS_NOTE")]
        public String PosNote 
        {
            get
            {
                return this.GetDataRowField<String>(() => PosNote);
            }
            set
            {
                UpdateDataRow<String>(() => PosNote, value);
            }
        }
        /// <summary>
        /// Количество единиц
        /// </summary>
        [Column(Name="STAFF_COUNT", CanBeNull=false)]
        public Decimal? StaffCount 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffCount);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffCount, value);
            }
        }
        /// <summary>
        /// Ссылка на рабочее место (карта условий труда)
        /// </summary>
        [Column(Name="WORK_PLACE_ID")]
        public Decimal? WorkPlaceID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkPlaceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkPlaceID, value);
            }
        }
        /// <summary>
        /// Ссылка на график работы
        /// </summary>
        [Column(Name="GR_WORK_ID")]
        public Decimal? GrWorkID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => GrWorkID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => GrWorkID, value);
            }
        }
        /// <summary>
        /// Ссылка на условия графика работы
        /// </summary>
        [Column(Name="WORKING_TIME_ID")]
        public Decimal? WorkingTimeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkingTimeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkingTimeID, value);
            }
        }
        /// <summary>
        /// Признак материально ответственного лица
        /// </summary>
        [Column(Name="SIGN_MAT_RESP_CONTR")]
        public Decimal? SignMatRespContr 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SignMatRespContr);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SignMatRespContr, value);
            }
        }
        /// <summary>
        /// Тип образования
        /// </summary>
        [Column(Name="TYPE_EDU_ID")]
        public Decimal? TypeEduID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeEduID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeEduID, value);
            }
        }
        /// <summary>
        /// Тип профстандарта
        /// </summary>
        [Column(Name="PROF_STANDART_ID")]
        public Decimal? ProfStandartID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ProfStandartID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ProfStandartID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="STAFF_ADDITION"), SchemaName("APSTAFF")]
    public partial class StaffAddition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер надбавки штаной единицы
        /// </summary>
        [Column(Name="STAFF_ADDITION_ID", CanBeNull=false)]
        public Decimal? StaffAdditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffAdditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffAdditionID, value);
            }
        }
        /// <summary>
        /// ССылка на штатную единицу
        /// </summary>
        [Column(Name="STAFF_ID", CanBeNull=false)]
        public Decimal? StaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffID, value);
            }
        }
        /// <summary>
        /// Ссылка на тип надбавки
        /// </summary>
        [Column(Name="TYPE_STAFF_ADDITION_ID", CanBeNull=false)]
        public Decimal? TypeStaffAdditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeStaffAdditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeStaffAdditionID, value);
            }
        }
        /// <summary>
        /// Значение надбавки
        /// </summary>
        [Column(Name="ADDITION_VALUE")]
        public Decimal? AdditionValue 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => AdditionValue);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => AdditionValue, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="TYPE_STAFF_ADDITION"), SchemaName("APSTAFF")]
    public partial class TypeStaffAddition : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Униккальный номер надбавки;
        /// </summary>
        [Column(Name="TYPE_STAFF_ADDITION_ID", CanBeNull=false)]
        public Decimal? TypeStaffAdditionID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeStaffAdditionID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeStaffAdditionID, value);
            }
        }
        /// <summary>
        /// Наименование надбавки;
        /// </summary>
        [Column(Name="NAME_STAFF_ADD")]
        public String NameStaffAdd 
        {
            get
            {
                return this.GetDataRowField<String>(() => NameStaffAdd);
            }
            set
            {
                UpdateDataRow<String>(() => NameStaffAdd, value);
            }
        }
        /// <summary>
        /// Наименование надбавки сокращенное
        /// </summary>
        [Column(Name="SHORT_NAME_STAFF_ADD")]
        public String ShortNameStaffAdd 
        {
            get
            {
                return this.GetDataRowField<String>(() => ShortNameStaffAdd);
            }
            set
            {
                UpdateDataRow<String>(() => ShortNameStaffAdd, value);
            }
        }
        /// <summary>
        /// Шифр оплат для надбавки;
        /// </summary>
        [Column(Name="PAY_TYPE_ID")]
        public Decimal? PayTypeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PayTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PayTypeID, value);
            }
        }
        /// <summary>
        /// Максимальное значение
        /// </summary>
        [Column(Name="MAX_VALUE")]
        public Decimal? MaxValue 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => MaxValue);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => MaxValue, value);
            }
        }
        /// <summary>
        /// Минимальное значение
        /// </summary>
        [Column(Name="MIN_VALUE")]
        public Decimal? MinValue 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => MinValue);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => MinValue, value);
            }
        }
        /// <summary>
        /// Тип измерения надбавки (процент, кф-т)
        /// </summary>
        [Column(Name="TYPE_ADD_MEASURE_ID")]
        public Decimal? TypeAddMeasureID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeAddMeasureID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeAddMeasureID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="STAFF_VAC"), SchemaName("APSTAFF")]
    public partial class StaffVac : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер доп. отпуска для штатной единицы
        /// </summary>
        [Column(Name="STAFF_VAC_ID", CanBeNull=false)]
        public Decimal? StaffVacID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffVacID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffVacID, value);
            }
        }
        /// <summary>
        /// Ссылка на штатную единицу
        /// </summary>
        [Column(Name="STAFF_ID", CanBeNull=false)]
        public Decimal? StaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffID, value);
            }
        }
        /// <summary>
        /// Ссылка на группу типа отпуска
        /// </summary>
        [Column(Name="VAC_GROUP_TYPE_ID", CanBeNull=false)]
        public Decimal? VacGroupTypeID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => VacGroupTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => VacGroupTypeID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="EMP_STAFF"), SchemaName("APSTAFF")]
    public partial class EmpStaff : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер расположения сотрудника
        /// </summary>
        [Column(Name="EMP_STAFF_ID", CanBeNull=false)]
        public Decimal? EmpStaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => EmpStaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => EmpStaffID, value);
            }
        }
        /// <summary>
        /// Ссылка на перевод сотрудника
        /// </summary>
        [Column(Name="TRANSFER_ID", CanBeNull=false)]
        public Decimal? TransferID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TransferID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TransferID, value);
            }
        }
        /// <summary>
        /// Ссылка на штатную единицу
        /// </summary>
        [Column(Name="STAFF_ID", CanBeNull=false)]
        public Decimal? StaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffID, value);
            }
        }
        /// <summary>
        /// Ставка на штатную единицу (процент занятости)
        /// </summary>
        [Column(Name="WORK_CF", CanBeNull=false)]
        public Decimal? WorkCf 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkCf);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkCf, value);
            }
        }
        /// <summary>
        /// Дата начала работы на штатной единице
        /// </summary>
        [Column(Name="DATE_START_WORK", CanBeNull=false)]
        public DateTime? DateStartWork 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateStartWork);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateStartWork, value);
            }
        }
        /// <summary>
        /// Дата окончания работы на штатной единице
        /// </summary>
        [Column(Name="DATE_END_WORK")]
        public DateTime? DateEndWork 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateEndWork);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateEndWork, value);
            }
        }
        [Column(Name="TYPE_TERM_TRANSFER_ID")]
        public Decimal? TypeTermTransferID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeTermTransferID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeTermTransferID, value);
            }
        }
        [Column(Name="EMP_STAFF_REL_ID")]
        public Decimal? EmpStaffRelID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => EmpStaffRelID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => EmpStaffRelID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }


    [Table(Name="STAFF_PERIOD"), SchemaName("APSTAFF")]
    public partial class StaffPeriod : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер периода штатной единицы
        /// </summary>
        [Column(Name="STAFF_PERIOD_ID", CanBeNull=false)]
        public Decimal? StaffPeriodID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffPeriodID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffPeriodID, value);
            }
        }
        /// <summary>
        /// Дата начала действия штатной единицы
        /// </summary>
        [Column(Name="DATE_STAFF_BEGIN", CanBeNull=false)]
        public DateTime? DateStaffBegin 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateStaffBegin);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateStaffBegin, value);
            }
        }
        /// <summary>
        /// Дата окончания действия штатной единицы
        /// </summary>
        [Column(Name="DATE_STAFF_END")]
        public DateTime? DateStaffEnd 
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DateStaffEnd);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DateStaffEnd, value);
            }
        }
        /// <summary>
        /// Ссылка на штатную единицу
        /// </summary>
        [Column(Name="STAFF_ID", CanBeNull=false)]
        public Decimal? StaffID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => StaffID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => StaffID, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }

    /// <summary>
    /// Представление данных по переводу сотрудника
    /// </summary>

    [Table(Name="EMP_TRANSFER_DATA"), SchemaName("APSTAFF")]
    public partial class EmpTransferData : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный номер перевода
        /// </summary>
        [Column(Name="TRANSFER_ID", CanBeNull=false)]
        public Decimal? TransferID 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TransferID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TransferID, value);
            }
        }
        /// <summary>
        /// Таб.№
        /// </summary>
        [Column(Name="PER_NUM", CanBeNull=false)]
        public String PerNum 
        {
            get
            {
                return this.GetDataRowField<String>(() => PerNum);
            }
            set
            {
                UpdateDataRow<String>(() => PerNum, value);
            }
        }
        /// <summary>
        /// Подразделение код
        /// </summary>
        [Column(Name="CODE_SUBDIV", CanBeNull=false)]
        public String CodeSubdiv 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeSubdiv);
            }
            set
            {
                UpdateDataRow<String>(() => CodeSubdiv, value);
            }
        }
        /// <summary>
        /// Фамилия
        /// </summary>
        [Column(Name="EMP_LAST_NAME", CanBeNull=false)]
        public String EmpLastName 
        {
            get
            {
                return this.GetDataRowField<String>(() => EmpLastName);
            }
            set
            {
                UpdateDataRow<String>(() => EmpLastName, value);
            }
        }
        /// <summary>
        /// Имя
        /// </summary>
        [Column(Name="EMP_FIRST_NAME", CanBeNull=false)]
        public String EmpFirstName 
        {
            get
            {
                return this.GetDataRowField<String>(() => EmpFirstName);
            }
            set
            {
                UpdateDataRow<String>(() => EmpFirstName, value);
            }
        }
        /// <summary>
        /// Отчество
        /// </summary>
        [Column(Name="EMP_MIDDLE_NAME", CanBeNull=false)]
        public String EmpMiddleName 
        {
            get
            {
                return this.GetDataRowField<String>(() => EmpMiddleName);
            }
            set
            {
                UpdateDataRow<String>(() => EmpMiddleName, value);
            }
        }
        /// <summary>
        /// Категория
        /// </summary>
        [Column(Name="CODE_DEGREE", CanBeNull=false)]
        public String CodeDegree 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeDegree);
            }
            set
            {
                UpdateDataRow<String>(() => CodeDegree, value);
            }
        }
        /// <summary>
        /// Вид производства
        /// </summary>
        [Column(Name="CODE_FORM_OPERATION", CanBeNull=false)]
        public String CodeFormOperation 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeFormOperation);
            }
            set
            {
                UpdateDataRow<String>(() => CodeFormOperation, value);
            }
        }
        /// <summary>
        /// Код тарифной сетки
        /// </summary>
        [Column(Name="CODE_TARIFF_GRID", CanBeNull=false)]
        public String CodeTariffGrid 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodeTariffGrid);
            }
            set
            {
                UpdateDataRow<String>(() => CodeTariffGrid, value);
            }
        }
        /// <summary>
        /// Код должности
        /// </summary>
        [Column(Name="CODE_POS", CanBeNull=false)]
        public String CodePos 
        {
            get
            {
                return this.GetDataRowField<String>(() => CodePos);
            }
            set
            {
                UpdateDataRow<String>(() => CodePos, value);
            }
        }
        /// <summary>
        /// Наименование должности
        /// </summary>
        [Column(Name="POS_NAME", CanBeNull=false)]
        public String PosName 
        {
            get
            {
                return this.GetDataRowField<String>(() => PosName);
            }
            set
            {
                UpdateDataRow<String>(() => PosName, value);
            }
        }
        /// <summary>
        /// Разряд
        /// </summary>
        [Column(Name="CLASSIFIC", CanBeNull=false)]
        public Decimal? Classific 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => Classific);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => Classific, value);
            }
        }
        /// <summary>
        /// Кф-т фактический
        /// </summary>
        [Column(Name="SALARY", CanBeNull=false)]
        public Decimal? Salary 
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => Salary);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => Salary, value);
            }
        }
        /// <summary>
        /// Фото
        /// </summary>
        [Column(Name="PHOTO")]
        public Byte[] Photo 
        {
            get
            {
                return this.GetDataRowField<Byte[]>(() => Photo);
            }
            set
            {
                UpdateDataRow<Byte[]>(() => Photo, value);
            }
        }
                #endregion
        
        		#region Ссылка на классы по другим данным
        
        		#endregion
    }

}
