using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EntityGenerator;
using Oracle.DataAccess.Client;
using System.Data;
using LibraryKadr;
using LibrarySalary.Helpers;
using Salary;
using System.Data.Linq.Mapping;

namespace ManningTable
{
    /// <summary>
    /// Логика взаимодействия для WorkPlaceEditor.xaml
    /// </summary>
    public partial class WorkPlaceEditor : Window
    {
        private WorkPlaceModel _model;

        public WorkPlaceEditor(decimal? workPlaceID)
        {
            _model = new WorkPlaceModel(workPlaceID);
            InitializeComponent();
            DataContext = Model;
        }

        public WorkPlaceModel Model
        {
            get
            {
                return _model;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }
        private void AddWPProtection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.AddNewProtection();
        }

        private void DeleteWPProtection_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && e.Parameter != null;
        }

        private void DeleteWPProtection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.DeleteProtection(e.Parameter as WorkPlaceProtection);
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.HasChanges;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exception ex = Model.Save();
            if (ex != null)
                MessageBox.Show(ex.GetFormattedException(), "Ошибка сохранения данных");
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }

    public partial class WorkPlaceModel : WorkPlace, IDataErrorInfo
    {
        OracleDataAdapter odaWork_Place, odaWork_Place_Condition, odaWork_Place_Protection;
        private DataView _positionSource;
        DataSet ds;

        public WorkPlaceModel(decimal? workPlaceID)
        {
            ds = new DataSet();

            odaWork_Place = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectWorkPlaceData.sql"), Connect.CurConnect);
            odaWork_Place.SelectCommand.BindByName = true;
            odaWork_Place.SelectCommand.Parameters.Add("p_work_place_id", OracleDbType.Decimal, workPlaceID, ParameterDirection.Input);
            odaWork_Place.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaWork_Place.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
            odaWork_Place.SelectCommand.Parameters.Add("c3", OracleDbType.RefCursor, ParameterDirection.Output);
            odaWork_Place.SelectCommand.Parameters.Add("c4", OracleDbType.RefCursor, ParameterDirection.Output);
            odaWork_Place.SelectCommand.Parameters.Add("c5", OracleDbType.RefCursor, ParameterDirection.Output);
            odaWork_Place.TableMappings.Add("Table", "WORK_PLACE");
            odaWork_Place.TableMappings.Add("Table1", "WORK_PLACE_PROTECTION");
            odaWork_Place.TableMappings.Add("Table2", "WORK_PLACE_CONDITION");
            odaWork_Place.TableMappings.Add("Table3", "TYPE_CONDITION");
            odaWork_Place.TableMappings.Add("Table4", "INDIVID_PROTECTION");

            #region Адаптер основновной рабочие места

            odaWork_Place.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_UPDATE(p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_SUBDIV_ID=>:p_SUBDIV_ID,p_POS_ID=>:p_POS_ID,p_WORKER_COUNT=>:p_WORKER_COUNT,p_HIGH_SALARY_SIGN=>:p_HIGH_SALARY_SIGN,p_ADDITION_VAC_SIGN=>:p_ADDITION_VAC_SIGN,p_SHORT_WORK_DAY_SIGN=>:p_SHORT_WORK_DAY_SIGN,p_MILK_SIGN=>:p_MILK_SIGN,p_MED_CHECKUP_PERIOD=>:p_MED_CHECKUP_PERIOD,p_WORK_PLACE_NUM=>:p_WORK_PLACE_NUM,p_SIGN_PREFERENTIAL_PENS=>:p_SIGN_PREFERENTIAL_PENS,p_PRIVILEGED_POSITION_ID=>:p_PRIVILEGED_POSITION_ID, p_WORK_PLACE_ORDER => :p_WORK_PLACE_ORDER, p_WORK_PLACE_DATE=>:p_WORK_PLACE_DATE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place.InsertCommand.BindByName = true;
            odaWork_Place.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place.InsertCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place.InsertCommand.Parameters["p_WORK_PLACE_ID"].DbType = DbType.Decimal;
            odaWork_Place.InsertCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_POS_ID", OracleDbType.Decimal, 0, "POS_ID").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_WORKER_COUNT", OracleDbType.Decimal, 0, "WORKER_COUNT").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_HIGH_SALARY_SIGN", OracleDbType.Decimal, 0, "HIGH_SALARY_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_ADDITION_VAC_SIGN", OracleDbType.Decimal, 0, "ADDITION_VAC_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_SHORT_WORK_DAY_SIGN", OracleDbType.Decimal, 0, "SHORT_WORK_DAY_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_MILK_SIGN", OracleDbType.Decimal, 0, "MILK_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_MED_CHECKUP_PERIOD", OracleDbType.Decimal, 0, "MED_CHECKUP_PERIOD").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_WORK_PLACE_NUM", OracleDbType.Varchar2, 0, "WORK_PLACE_NUM").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_SIGN_PREFERENTIAL_PENS", OracleDbType.Decimal, 0, "SIGN_PREFERENTIAL_PENS").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_PRIVILEGED_POSITION_ID", OracleDbType.Decimal, 0, "PRIVILEGED_POSITION_ID").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_WORK_PLACE_ORDER", OracleDbType.Varchar2, 0, "WORK_PLACE_ORDER").Direction = ParameterDirection.Input;
            odaWork_Place.InsertCommand.Parameters.Add("p_WORK_PLACE_DATE", OracleDbType.Date, 0, "WORK_PLACE_DATE").Direction = ParameterDirection.Input;

            odaWork_Place.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_UPDATE(p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_SUBDIV_ID=>:p_SUBDIV_ID,p_POS_ID=>:p_POS_ID,p_WORKER_COUNT=>:p_WORKER_COUNT,p_HIGH_SALARY_SIGN=>:p_HIGH_SALARY_SIGN,p_ADDITION_VAC_SIGN=>:p_ADDITION_VAC_SIGN,p_SHORT_WORK_DAY_SIGN=>:p_SHORT_WORK_DAY_SIGN,p_MILK_SIGN=>:p_MILK_SIGN,p_MED_CHECKUP_PERIOD=>:p_MED_CHECKUP_PERIOD,p_WORK_PLACE_NUM=>:p_WORK_PLACE_NUM,p_SIGN_PREFERENTIAL_PENS=>:p_SIGN_PREFERENTIAL_PENS,p_PRIVILEGED_POSITION_ID=>:p_PRIVILEGED_POSITION_ID, p_WORK_PLACE_ORDER => :p_WORK_PLACE_ORDER, p_WORK_PLACE_DATE=>:p_WORK_PLACE_DATE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place.UpdateCommand.BindByName = true;
            odaWork_Place.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place.UpdateCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place.UpdateCommand.Parameters["p_WORK_PLACE_ID"].DbType = DbType.Decimal;
            odaWork_Place.UpdateCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_POS_ID", OracleDbType.Decimal, 0, "POS_ID").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_WORKER_COUNT", OracleDbType.Decimal, 0, "WORKER_COUNT").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_HIGH_SALARY_SIGN", OracleDbType.Decimal, 0, "HIGH_SALARY_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_ADDITION_VAC_SIGN", OracleDbType.Decimal, 0, "ADDITION_VAC_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_SHORT_WORK_DAY_SIGN", OracleDbType.Decimal, 0, "SHORT_WORK_DAY_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_MILK_SIGN", OracleDbType.Decimal, 0, "MILK_SIGN").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_MED_CHECKUP_PERIOD", OracleDbType.Decimal, 0, "MED_CHECKUP_PERIOD").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_WORK_PLACE_NUM", OracleDbType.Varchar2, 0, "WORK_PLACE_NUM").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_SIGN_PREFERENTIAL_PENS", OracleDbType.Decimal, 0, "SIGN_PREFERENTIAL_PENS").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_PRIVILEGED_POSITION_ID", OracleDbType.Decimal, 0, "PRIVILEGED_POSITION_ID").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_WORK_PLACE_ORDER", OracleDbType.Varchar2, 0, "WORK_PLACE_ORDER").Direction = ParameterDirection.Input;
            odaWork_Place.UpdateCommand.Parameters.Add("p_WORK_PLACE_DATE", OracleDbType.Date, 0, "WORK_PLACE_DATE").Direction = ParameterDirection.Input;

            odaWork_Place.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_DELETE(:p_WORK_PLACE_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place.DeleteCommand.BindByName = true;
            odaWork_Place.DeleteCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.InputOutput;

            odaWork_Place.AcceptChangesDuringUpdate = false;
            #endregion

            #region Адаптер Условия труда
            odaWork_Place_Condition = new OracleDataAdapter();
            odaWork_Place_Condition.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_CONDITION_UPDATE(p_WORK_PLACE_CONDITION_ID=>:p_WORK_PLACE_CONDITION_ID,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_TYPE_CONDITION_ID=>:p_TYPE_CONDITION_ID,p_CONDITIONS_OF_WORK_ID=>:p_CONDITIONS_OF_WORK_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Condition.InsertCommand.BindByName = true;
            odaWork_Place_Condition.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place_Condition.InsertCommand.Parameters.Add("p_WORK_PLACE_CONDITION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_CONDITION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Condition.InsertCommand.Parameters["p_WORK_PLACE_CONDITION_ID"].DbType = DbType.Decimal;
            odaWork_Place_Condition.InsertCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Condition.InsertCommand.Parameters.Add("p_TYPE_CONDITION_ID", OracleDbType.Decimal, 0, "TYPE_CONDITION_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Condition.InsertCommand.Parameters.Add("p_CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID").Direction = ParameterDirection.Input;

            odaWork_Place_Condition.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_CONDITION_UPDATE(p_WORK_PLACE_CONDITION_ID=>:p_WORK_PLACE_CONDITION_ID,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_TYPE_CONDITION_ID=>:p_TYPE_CONDITION_ID,p_CONDITIONS_OF_WORK_ID=>:p_CONDITIONS_OF_WORK_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Condition.UpdateCommand.BindByName = true;
            odaWork_Place_Condition.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place_Condition.UpdateCommand.Parameters.Add("p_WORK_PLACE_CONDITION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_CONDITION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Condition.UpdateCommand.Parameters["p_WORK_PLACE_CONDITION_ID"].DbType = DbType.Decimal;
            odaWork_Place_Condition.UpdateCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Condition.UpdateCommand.Parameters.Add("p_TYPE_CONDITION_ID", OracleDbType.Decimal, 0, "TYPE_CONDITION_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Condition.UpdateCommand.Parameters.Add("p_CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID").Direction = ParameterDirection.Input;

            odaWork_Place_Condition.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_CONDITION_DELETE(:p_WORK_PLACE_CONDITION_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Condition.DeleteCommand.BindByName = true;
            odaWork_Place_Condition.DeleteCommand.Parameters.Add("p_WORK_PLACE_CONDITION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_CONDITION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Condition.AcceptChangesDuringUpdate = false;
            #endregion

            #region Адаптер защиты рабочего места

            odaWork_Place_Protection = new OracleDataAdapter();
            odaWork_Place_Protection.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_PROTECTION_UPDATE(p_WORK_PLACE_PROTECTION_ID=>:p_WORK_PLACE_PROTECTION_ID,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_INDIVID_PROTECTION_ID=>:p_INDIVID_PROTECTION_ID,p_PERIOD_FOR_USE=>:p_PERIOD_FOR_USE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Protection.InsertCommand.BindByName = true;
            odaWork_Place_Protection.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place_Protection.InsertCommand.Parameters.Add("p_WORK_PLACE_PROTECTION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_PROTECTION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Protection.InsertCommand.Parameters["p_WORK_PLACE_PROTECTION_ID"].DbType = DbType.Decimal;
            odaWork_Place_Protection.InsertCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Protection.InsertCommand.Parameters.Add("p_INDIVID_PROTECTION_ID", OracleDbType.Decimal, 0, "INDIVID_PROTECTION_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Protection.InsertCommand.Parameters.Add("p_PERIOD_FOR_USE", OracleDbType.Decimal, 0, "PERIOD_FOR_USE").Direction = ParameterDirection.Input;

            odaWork_Place_Protection.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_PROTECTION_UPDATE(p_WORK_PLACE_PROTECTION_ID=>:p_WORK_PLACE_PROTECTION_ID,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_INDIVID_PROTECTION_ID=>:p_INDIVID_PROTECTION_ID,p_PERIOD_FOR_USE=>:p_PERIOD_FOR_USE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Protection.UpdateCommand.BindByName = true;
            odaWork_Place_Protection.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWork_Place_Protection.UpdateCommand.Parameters.Add("p_WORK_PLACE_PROTECTION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_PROTECTION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Protection.UpdateCommand.Parameters["p_WORK_PLACE_PROTECTION_ID"].DbType = DbType.Decimal;
            odaWork_Place_Protection.UpdateCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Protection.UpdateCommand.Parameters.Add("p_INDIVID_PROTECTION_ID", OracleDbType.Decimal, 0, "INDIVID_PROTECTION_ID").Direction = ParameterDirection.Input;
            odaWork_Place_Protection.UpdateCommand.Parameters.Add("p_PERIOD_FOR_USE", OracleDbType.Decimal, 0, "PERIOD_FOR_USE").Direction = ParameterDirection.Input;

            odaWork_Place_Protection.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_PROTECTION_DELETE(:p_WORK_PLACE_PROTECTION_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaWork_Place_Protection.DeleteCommand.BindByName = true;
            odaWork_Place_Protection.DeleteCommand.Parameters.Add("p_WORK_PLACE_PROTECTION_ID", OracleDbType.Decimal, 0, "WORK_PLACE_PROTECTION_ID").Direction = ParameterDirection.InputOutput;
            odaWork_Place_Protection.AcceptChangesDuringUpdate = false;
            #endregion

            odaWork_Place.Fill(ds);
            if (workPlaceID == null)
            {
                this.DataRow = ds.Tables["WORK_PLACE"].Rows.Add();
                HighSalarySign = 0;
                AdditionVacSign = 0;
                ShortWorkDaySign = 0;
                MilkSign = 0;
                MedCheckupPeriod = 0;
                SignPreferentialPens = 0;
                isAdded = true;
            }
            else
                this.DataRow = ds.Tables["WORK_PLACE"].Rows[0];
        }

        /// <summary>
        /// Источник данных - список профессий
        /// </summary>
        public DataView PositionSource
        {
            get
            {
                if (_positionSource == null)
                    _positionSource = new DataView(WpfControlLibrary.AppDataSet.Tables["POSITION"], "", "CODE_POS", DataViewRowState.CurrentRows);
                return _positionSource;
            }
        }

        /// <summary>
        /// Типы классов условий труда источник данных
        /// </summary>
        public List<ConditionsOfWork> CondWorkTypeSource
        {
            get
            {
                if (_condWorkTypeSource == null)
                    _condWorkTypeSource = WpfControlLibrary.AppDataSet.Tables["CONDITIONS_OF_WORK"].ConvertToEntityList<ConditionsOfWork>();
                return _condWorkTypeSource;
            }
        }

        /// <summary>
        /// Источник данных периодичность медосмотра
        /// </summary>
        public IEnumerable<object> MedicalCheckSource
        {
            get
            {
                return new Tuple<decimal, string>[]
                    { new Tuple<decimal, string>(0, "Без мед. осмотра"),
                    new Tuple<decimal, string>(6, "2 раза в год"),
                    new Tuple<decimal, string>(12, "1 раз в год"),
                    new Tuple<decimal, string>(24, "1 раз в 2 года")
                    }.Select(r => new { Period = r.Item1, Description = r.Item2 });
            }
        }

        List<PrivilegedPosition> _privPosSource;
        /// <summary>
        /// Источник данных список льготных профессий
        /// </summary>
        public List<PrivilegedPosition> PrivPositionSource
        {
            get
            {
                if (_privPosSource == null)
                    _privPosSource = WpfControlLibrary.AppDataSet.Tables["PRIVILEGED_POSITION"].ConvertToEntityList<PrivilegedPosition>();
                return _privPosSource.Where(r => r.SubdivID == this.SubdivID && r.PosID == this.PosID).ToList();
            }
        }

        /// <summary>
        /// Источник данных  - типы индивидуальной защиты
        /// </summary>
        public List<IndividProtection> ProtectionTypeSource
        {
            get
            {
                if (_protectionSource == null)
                {
                    _protectionSource = ds.Tables["INDIVID_PROTECTION"].ConvertToEntityList<IndividProtection>();
                }
                return _protectionSource;
            }
        }

        /// <summary>
        /// Должность ссылка на нее
        /// </summary>
        [Column(CanBeNull = false, Name = "POS_ID")]
        public new decimal? PosID
        {
            get
            {
                return base.PosID;
            }
            set
            {
                base.PosID = value;
                RaisePropertyChanged(() => PrivPositionSource);
            }
        }

        /// <summary>
        /// ССылка на подразделение
        /// </summary>
        [Column(CanBeNull = false, Name = "SUBDIV_ID")]
        public new decimal? SubdivID
        {
            get
            {
                return base.SubdivID;
            }
            set
            {
                base.SubdivID = value;
                RaisePropertyChanged(() => PrivPositionSource);
            }
        }

        public new decimal? PrivilegedPositionID
        {
            get
            {
                return base.PrivilegedPositionID;
            }
            set
            {
                base.PrivilegedPositionID = value;
                _privPosition = null;
                RaisePropertyChanged(() => PrivPosition);
            }
        }

        PrivilegedPosition _privPosition;
        private List<WorkPlaceConditionViewModel> _wpCondSource;
        private List<WorkPlaceProtectionModel> _wpProtectionSource;
        private List<ConditionsOfWork> _condWorkTypeSource;
        private bool isAdded = false;
        private List<IndividProtection> _protectionSource;

        /// <summary>
        /// Льготная проффесия
        /// </summary>
        public PrivilegedPosition PrivPosition
        {
            get
            {
                if (_privPosition == null)
                    _privPosition = PrivPositionSource.Where(r => r.PrivilegedPositionID == this.PrivilegedPositionID).FirstOrDefault();
                return _privPosition;
            }
        }


        public List<WorkPlaceConditionViewModel> WPConditionSource
        {
            get
            {
                if (_wpCondSource == null)
                {
                    _wpCondSource = ds.Tables["TYPE_CONDITION"].ConvertToEntityList<WorkPlaceConditionViewModel>();
                }
                return _wpCondSource;
            }
        }

        /// <summary>
        /// Nf,b
        /// </summary>
        public List<WorkPlaceCondition> WorkPlaceConditions
        {
            get
            {
                return ds.Tables["WORK_PLACE_CONDITION"].ConvertToEntityList<WorkPlaceCondition>();
            }
        }

        /// <summary>
        /// Источник данных - используемые средства защиты для рабочего места
        /// </summary>
        public List<WorkPlaceProtectionModel> WPProtectionSource
        {
            get
            {
                _wpProtectionSource = ds.Tables["WORK_PLACE_PROTECTION"].ConvertToEntityList<WorkPlaceProtectionModel>();
                return _wpProtectionSource;
            }
        }

        /// <summary>
        /// Ошибка поля модели
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public new string this[string columnName]
        {
            get
            {
                if (IsReadOnly) return string.Empty;
                if (columnName == "SubdivID" && SubdivID == null)
                    return "Не указано подразделение";
                if (columnName == "PosID" && PosID == null)
                    return "Не указана должность сотрудника";
                if (columnName == "WorkerCount" && WorkerCount == null)
                    return "Не указано количество рабочих мест";
                if (columnName == "SubdivID" || columnName == "PosID" || columnName == "WorkerCount")
                    RaisePropertyChanged(() => Error);
                return string.Empty;
            }
        }

        /// <summary>
        /// Ошибка модели
        /// </summary>
        public new string Error
        {
            get
            {
                if (IsReadOnly) return string.Empty;
                if (SubdivID == null)
                {
                    return "Не указано подразделение";
                }
                if (PosID == null)
                {
                    return "Не указана должность сотрудника";
                }
                if (WorkerCount == null)
                {
                    return "Не указано количество рабочих мест";
                }
                foreach (var item in WPProtectionSource)
                    if (item.IndividProtectionID == null || item.PeriodForUse == null)
                        return "Не полностью заполнены поля раздела Индивидуальная защита";
                return string.Empty;
            }
        }

        public bool HasChanges
        {
            get
            {
                return ds != null && ds.HasChanges();
            }
        }

        /// <summary>
        /// Добавлнеие новой записи в таблицу СИЗ
        /// </summary>
        internal void AddNewProtection()
        {
            DataRow r = ds.Tables["WORK_PLACE_PROTECTION"].Rows.Add();
            RaisePropertyChanged(() => WPProtectionSource);
        }

        /// <summary>
        /// Удаление записи из таблицы СИЗ
        /// </summary>
        /// <param name="workPlaceProtection"></param>
        internal void DeleteProtection(WorkPlaceProtection workPlaceProtection)
        {
            workPlaceProtection.DataRow.Delete();
            RaisePropertyChanged(() => WPProtectionSource);
        }

        /// <summary>
        /// Сохранениен данных 
        /// </summary>
        /// <returns></returns>
        public new Exception Save()
        {
            if (!string.IsNullOrEmpty(Error))
            {
                RaisePropertyChanged(() => Error);
                return new Exception(Error);
            }
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                if (isAdded)
                    WorkPlaceID = null;
                odaWork_Place.Update(new DataRow[] { this.DataRow });
                foreach (var r in WPProtectionSource)
                {
                    r.WorkPlaceID = this.WorkPlaceID;
                    if (r.EntityState == DataRowState.Added)
                        r.WorkPlaceProtectionID = null;
                }
                odaWork_Place_Protection.Update(ds.Tables["WORK_PLACE_PROTECTION"]);
                foreach (var r in WorkPlaceConditions)
                {
                    r.WorkPlaceID = this.WorkPlaceID;
                    if (r.EntityState == DataRowState.Added)
                        r.WorkPlaceConditionID = null;
                }
                odaWork_Place_Condition.Update(ds.Tables["WORK_PLACE_CONDITION"]);
                ds.AcceptChanges();
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }


        bool _isReadOnly = false;
        /// <summary>
        /// Доступна ли модель только для чтения
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                RaisePropertyChanged(() => IsReadOnly);
            }
        }
    }

    public partial class WorkPlaceConditionViewModel : TypeCondition
    {

        /// <summary>
        /// Условия труда для конкретного типа вредности
        /// </summary>
        public decimal? ConditionsOfWorkID
        {
            get
            {
                return currentConditionID;
            }
            set
            {
                if (currentConditionID != value)
                {
                    if (value==null)
                        DeleteCond();
                    if (value != null)
                    {
                        if (currentConditionID == null)
                            InsertCond(value.Value);
                        else
                            UpdateCond(value.Value);
                    }
                    RaisePropertyChanged(() => TypeConditionID);
                }
            }
        }

        #region remove, delete, add condition helper
        private decimal? currentConditionID
        {
            get
            {
                return DataSet.Tables["WORK_PLACE_CONDITION"].DefaultView.OfType<DataRowView>()
                    .Where(r => r.Row.Field2<Decimal?>("TYPE_CONDITION_ID") == this.TypeConditionID).Select(r => r.Row.Field2<decimal?>("CONDITIONS_OF_WORK_ID")).FirstOrDefault();
            }
        }

        private void DeleteCond()
        {
            foreach (var row in DataSet.Tables["WORK_PLACE_CONDITION"].DefaultView.OfType<DataRowView>().Where(r => r.Row.Field2<Decimal?>("TYPE_CONDITION_ID") == this.TypeConditionID))
            {
                row.Delete();
            }
        }

        private void InsertCond(decimal condID)
        {
            DataRow r = DataSet.Tables["WORK_PLACE_CONDITION"].Rows.Add();
            r["TYPE_CONDITION_ID"] = this.TypeConditionID;
            r["CONDITIONS_OF_WORK_ID"] = condID;
        }

        private void UpdateCond(decimal condID)
        {
            DataRowView rview = 
            DataSet.Tables["WORK_PLACE_CONDITION"].DefaultView.OfType<DataRowView>()
                    .Where(r => r.Row.Field2<Decimal?>("TYPE_CONDITION_ID") == this.TypeConditionID).FirstOrDefault();
            if (rview != null)
                rview["CONDITIONS_OF_WORK_ID"] = condID;
        }
        #endregion
    }

    public partial class WorkPlaceProtectionModel : WorkPlaceProtection
    {
        private IndividProtection _indivProtection;

        public new decimal? IndividProtectionID
        {
            get
            {
                return base.IndividProtectionID;
            }
            set
            {
                base.IndividProtectionID = value;
                _indivProtection = null;
                RaisePropertyChanged(() => IndividProtection);
            }
        }

        /// <summary>
        /// Индивидуальная защита из справочника
        /// </summary>
        public IndividProtection IndividProtection
        {
            get
            {
                if (_indivProtection == null)
                {
                    _indivProtection = GetParentEntity<IndividProtection>("INDIVID_PROTECTION_ID");
                }
                return _indivProtection;
            }
        }
    }
}
