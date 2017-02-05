using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Model
{
    [Table(Name = "SUBDIV"), SchemaName("APSTAFF")]
    public partial class Subdiv : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор подразделения, из которого образовалось данное подразделение;
        /// </summary>
        [Column(Name = "SUBDIV_ID", CanBeNull = false)]
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
        /// Код подразделения; Текущий код подразделения;
        /// </summary>
        [Column(Name = "CODE_SUBDIV")]
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
        /// Наименование подразделения; Текущее наименование подразделения;
        /// </summary>
        [Column(Name = "SUBDIV_NAME")]
        public String SubdivName
        {
            get
            {
                return this.GetDataRowField<String>(() => SubdivName);
            }
            set
            {
                UpdateDataRow<String>(() => SubdivName, value);
            }
        }
        /// <summary>
        /// Признак актуальности; Признак актуальности подразделения (если равен 1, то подразделение в данный момент актуально, если 0, то подразделение не актуально, то есть было когда-то, но сейчас его нет);
        /// </summary>
        [Column(Name = "SUB_ACTUAL_SIGN")]
        public Int16? SubActualSign
        {
            get
            {
                return this.GetDataRowField<Int16?>(() => SubActualSign);
            }
            set
            {
                UpdateDataRow<Int16?>(() => SubActualSign, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор типа работы подразделения;
        /// </summary>
        [Column(Name = "WORK_TYPE_ID")]
        public Decimal? WorkTypeID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => WorkTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => WorkTypeID, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор службы завода;
        /// </summary>
        [Column(Name = "SERVICE_ID")]
        public Decimal? ServiceID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => ServiceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => ServiceID, value);
            }
        }
        /// <summary>
        /// Дата введения в действие кода и наименования для данного подразделения;
        /// </summary>
        [Column(Name = "SUB_DATE_START")]
        public DateTime? SubDateStart
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => SubDateStart);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => SubDateStart, value);
            }
        }
        /// <summary>
        /// Дата окончания действия имени и кода для данного подразделения;
        /// </summary>
        [Column(Name = "SUB_DATE_END")]
        public DateTime? SubDateEnd
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => SubDateEnd);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => SubDateEnd, value);
            }
        }
        /// <summary>
        /// Код подразделения; Код подразделения, которому подчиняется данное подразделение;
        /// </summary>
        [Column(Name = "PARENT_ID")]
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
        /// Уникальный идентификатор подразделения, из которого образовалось данное подразделение;
        /// </summary>
        [Column(Name = "FROM_SUBDIV")]
        public Decimal? FromSubdiv
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => FromSubdiv);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => FromSubdiv, value);
            }
        }
        /// <summary>
        /// Наименование типа подразделения;
        /// </summary>
        [Column(Name = "TYPE_SUBDIV_ID")]
        public Decimal? TypeSubdivID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TypeSubdivID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TypeSubdivID, value);
            }
        }
        /// <summary>
        /// Уникальный идентификатор графика работы;
        /// </summary>
        [Column(Name = "GR_WORK_ID")]
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
        #endregion
    }


    [Table(Name = "TYPE_VAC"), SchemaName("APSTAFF")]
    public partial class TypeVac : RowEntityBase
    {
        #region Class Members
        /// <summary>
        /// Уникальный идентификатор типа отпуска;
        /// </summary>
        [Column(Name = "TYPE_VAC_ID", CanBeNull = false)]
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
        [Column(Name = "NAME_VAC")]
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
        [Column(Name = "CODE_VAC")]
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
        [Column(Name = "SHORT_NAME_VAC")]
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
        [Column(Name = "COUNT_DAYS_IN_YEAR")]
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
        [Column(Name = "TYPE_VAC_CALC_ID")]
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
        [Column(Name = "NUMBER_CALC")]
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
        [Column(Name = "CALC_PERIOD_DAYS")]
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
        [Column(Name = "NEED_REG_DOC", CanBeNull = false)]
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
        [Column(Name = "ACCOUNT_NOTE_SIGN")]
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
        [Column(Name = "VAC_REASON")]
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
        [Column(Name = "VAC_GROUP_BITSET")]
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
        [Column(Name = "SIGN_BLOCK_NOTE_ACCOUNT")]
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
        [Column(Name = "SING_PAYMENT")]
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
    }
}
