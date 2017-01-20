using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using System.Linq.Expressions;
using Staff;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace VacationSchedule
{
    /// <summary>
    /// Interaction logic for VacPeriodForm.xaml
    /// </summary>
    public partial class VacPeriodForm : Window
    {
        PeriodVacModel _model;
        public VacPeriodForm(PeriodVacModel model)
        {
            _model = model;
            InitializeComponent();
            this.DataContext = Model;
            this.Loaded += (p, pw) => { Model.UpdateVacs(); };
        }

        public VacPeriodForm(DateTime DateBeg, DateTime DateEn, decimal? subdivId, bool CheckAll, bool OnlyFact, bool lockDates) 
            : this(new PeriodVacModel() { DateBegin = DateBeg, DateEnd = DateEn, SubdivID = subdivId })
        {
            if (lockDates)
                Model.IsPeriodEnabled = false;
            Model.IsOnlyActual = OnlyFact ? 1m : 0m;
        }
        public VacPeriodForm(DateTime DateBeg, DateTime DateEn, decimal? subdivId, bool CheckAll, bool OnlyFact) 
            : this(DateBeg, DateEn, subdivId, CheckAll, OnlyFact, false)
        {
           
        }

        public VacPeriodForm(DateTime DateBeg, DateTime DateEn, decimal? subdivid) : this(DateBeg, DateEn, subdivid, false, false)
        {
            this.Model.IsNeedSelectedVacs = true;
        }

        public VacPeriodForm()
        {
            InitializeComponent();
        }

        public PeriodVacModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                this.DataContext = value;
            }
        }


        /// <summary>
        /// Выбранные айдишники 
        /// </summary>
        public List<decimal> SelectedVacIDs
        {
            get
            {
                return Model.VacSource.Where(r => r.Row.Field<bool>("FL")).Select(r => r.Row.Field<decimal>("VAC_SCHED_ID")).ToList();
            }
        }


        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (this.ControlClosed != null)
                ControlClosed(this, EventArgs.Empty);
        }

        public event EventHandler ControlClosed;

        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            Model.UpdateVacs();
        }

        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            Model.CheckAll((sender as CheckBox).IsChecked);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Model!=null && (!Model.IsNeedSelectedVacs || Model.VacSource.Any(r => r.Row.Field<bool>("FL")));
        }

        private void Next_CanExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            if (this.ControlClosed != null)
                ControlClosed(this, EventArgs.Empty);
        }
    }

    public class PeriodVacModel : NotificationObject
    {
        DataSet ds;
        OracleDataAdapter odaVacs, odaData;
        public PeriodVacModel()
        {
            ds = new DataSet();
            odaVacs = new OracleDataAdapter(string.Format(Queries.GetQuery("go/GetEmpForFilterPeriod.sql"), Connect.Schema), Connect.CurConnect);
            odaVacs.SelectCommand.BindByName = true;
            odaVacs.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, null, ParameterDirection.Input);
            odaVacs.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, null, ParameterDirection.Input);
            odaVacs.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaVacs.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, null, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
            odaVacs.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, null, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
            odaVacs.SelectCommand.Parameters.Add("p_only_actual", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaVacs.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
            odaVacs.TableMappings.Add("Table", "VACS");

            odaData = new OracleDataAdapter(string.Format(Queries.GetQuery("go/SelectVacFilterPeriodData.sql"), Connect.Schema), Connect.CurConnect);
            odaData.SelectCommand.BindByName = true;
            odaData.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaData.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
            odaData.SelectCommand.Parameters.Add("c3", OracleDbType.RefCursor, ParameterDirection.Output);

            odaData.TableMappings.Add("Table", "SUBDIV");
            odaData.TableMappings.Add("Table1", "DEGREE");
            odaData.TableMappings.Add("Table2", "FORM_OPERATION");
            odaData.Fill(ds);
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            VacPeriodIsBusy = false;
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, "Ошибка получения данных");
            RaisePropertyChanged(() => VacSource);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadData();
        }

        bool _vacPeriodIsBusy = false;
        public bool VacPeriodIsBusy
        {
            get
            {
                return _vacPeriodIsBusy;
            }
            set
            {
                _vacPeriodIsBusy = value;
                RaisePropertyChanged(() => VacPeriodIsBusy);
            }
        }

        DateTime? _dateBegin, _dateEnd;
        decimal? _subdiv_id, _onlyFact;

        /// <summary>
        /// Дата начала периода
        /// </summary>
        public DateTime? DateBegin
        {
            get
            {
                return _dateBegin;
            }
            set
            {
                if (_dateBegin == value) return;
                _dateBegin = value;
                RaisePropertyChanged(() => DateBegin);
            }
        }
        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public DateTime? DateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                if (_dateEnd == value) return;
                _dateEnd = value;
                RaisePropertyChanged(() => DateEnd);
            }
        }

        /// <summary>
        /// Подразделение
        /// </summary>
        public decimal? SubdivID
        {
            get
            {
                return _subdiv_id;
            }
            set
            {
                _subdiv_id = value;
                RaisePropertyChanged(() => SubdivID);
            }
        }

        /// <summary>
        /// Подразделение выбранное
        /// </summary>
        public string CodeSubdiv
        {
            get
            {
                if (_subdiv_id == null)
                    return string.Empty;
                else
                    return SubdivSource.OfType<DataRowView>().Where(r => r.Row.Field<Decimal>("SUBDIV_ID") == _subdiv_id).Select(r => r["CODE_SUBDIV"].ToString()).FirstOrDefault();
            }
        }

        DataView _subdivSource;
        /// <summary>
        /// Источник данных для списка подразделения
        /// </summary>
        public DataView SubdivSource
        {
            get
            {
                if (_subdivSource == null)
                {
                    _subdivSource = new DataView(ds.Tables["SUBDIV"], "", "CODE_SUBDIV", DataViewRowState.CurrentRows);
                }
                return _subdivSource;
            }
        }

        List<Degree> _degreeSource;

        /// <summary>
        /// Источник данных для списка категорий
        /// </summary>
        public List<Degree> DegreeSource
        {
            get
            {
                if (_degreeSource == null)
                {
                    _degreeSource = ds.Tables["DEGREE"].Rows.OfType<DataRow>().OrderBy(r => r["CODE_DEGREE"].ToString()).Select(r => new Degree(r.Field<Decimal>("DEGREE_ID"), r["CODE_DEGREE"].ToString(), r["DEGREE_NAME"].ToString()) { IsChecked = true }).ToList();
                }
                return _degreeSource;
            }
        }

        /// <summary>
        /// Выбранные айдишники категорий
        /// </summary>
        public decimal[] SelectedDegrees
        {
            get
            {
                if (DegreeSource!=null)
                    return DegreeSource.Where(r => r.IsChecked).OrderBy(r => r.CodeDegree).Select(r => r.DegreeID.Value).ToArray();
                else
                    return new decimal[]{};
            }
        }

        public decimal? IsOnlyActual
        {
            get
            { 
                return _onlyFact;
            }
            set
            {
                _onlyFact = value;
                RaisePropertyChanged(()=>IsOnlyActual);
            }
        }

        List<FormOperation> _formOperationSource;

        /// <summary>
        /// Источник данных для списка вида производств
        /// </summary>
        public List<FormOperation> FormOperationSource
        {
            get
            {
                if (_formOperationSource == null)
                {
                    _formOperationSource = ds.Tables["FORM_OPERATION"].Rows.OfType<DataRow>().OrderBy(r => r["CODE_FORM_OPERATION"].ToString()).Select(r => new FormOperation(r.Field<Decimal>("FORM_OPERATION_ID"), r["CODE_FORM_OPERATION"].ToString(), r["NAME_FORM_OPERATION"].ToString()) { IsChecked = true }).ToList();
                }
                return _formOperationSource;
            }
        }

        /// <summary>
        /// Выбранные айдишники категорий
        /// </summary>
        public decimal[] SelectedFormOperations
        {
            get
            {
                if (FormOperationSource!=null)
                    return FormOperationSource.Where(r=>r.IsChecked).OrderBy(r=>r.FormOperationID).Select(r=>r.FormOperationID.Value).ToArray();
                else
                    return new decimal[]{};
            }
        }

        List<DataRowView> _vacSource;
        public List<DataRowView> VacSource
        {
            get
            {
                if (ds != null && ds.Tables.Contains("VACS"))
                   _vacSource = ds.Tables["VACS"].DefaultView.OfType<DataRowView>().ToList();
                return _vacSource;
            }
        }

        bool _needSelected = false;
        public bool IsNeedSelectedVacs
        {
            get
            {
                return _needSelected;
            }
            set
            {
                _needSelected = value;
                RaisePropertyChanged(() => IsNeedSelectedVacs);
            }
        }

        /// <summary>
        /// Загрузка данных по отпускам
        /// </summary>
        public void LoadData()
        {
            bool first_load = true;
            if (ds.Tables.Contains("VACS"))
            {
                ds.Tables["VACS"].Rows.Clear();
                first_load = false;
            }
            else
            {
                ds.Tables.Add("VACS");
                ds.Tables["VACS"].Columns.Add("FL", typeof(bool)).DefaultValue= false;
            }
            odaVacs.SelectCommand.Parameters["p_date_begin"].Value = _dateBegin;
            odaVacs.SelectCommand.Parameters["p_date_end"].Value = _dateEnd;
            odaVacs.SelectCommand.Parameters["p_subdiv_id"].Value = _subdiv_id;
            odaVacs.SelectCommand.Parameters["p_degree_ids"].Value = SelectedDegrees;
            odaVacs.SelectCommand.Parameters["p_form_oper_ids"].Value=SelectedFormOperations;
            odaVacs.SelectCommand.Parameters["p_only_actual"].Value= _onlyFact;
            odaVacs.Fill(ds);
        }

        BackgroundWorker bw;

        /// <summary>
        /// Обновление данных по фильтру отпуска
        /// </summary>
        public void UpdateVacs()
        {
            if (bw.IsBusy) return;
            VacPeriodIsBusy = true;
            bw.RunWorkerAsync();
        }

        bool _isFormOpertaionEnabled = false;
        private bool _isPeriodEnabled = true;
        private  bool _isDegreeEnabled = false;

        /// <summary>
        /// Доступен ли для выбора вид производства
        /// </summary>
        public bool IsFormOpertaionEnabled
        {
            get
            {
                return _isFormOpertaionEnabled;
            }
            set
            {
                _isFormOpertaionEnabled = value;
                RaisePropertyChanged(() => IsFormOpertaionEnabled);
            }
        }

        /// <summary>
        /// Доступен ли период для выбора
        /// </summary>
        public bool IsPeriodEnabled
        {
            get
            {
                return _isPeriodEnabled;
            }
            set
            {
                _isPeriodEnabled = value;
                RaisePropertyChanged(() => IsPeriodEnabled);
            }
        }

        /// <summary>
        /// Доступна ли категория для выбора
        /// </summary>
        public bool IsDegreeEnabled
        {
            get
            {
                return _isDegreeEnabled;
            }
            set
            {
                _isDegreeEnabled = value;
                RaisePropertyChanged(() => IsDegreeEnabled);
            }
        }

        public void CheckAll(bool? fl)
        {
            if (VacSource!=null)
            foreach (DataRowView r in VacSource)
            {
                r["FL"] = fl;
            }
        }

    }

    public class FormOperation : NotificationObject
    {
        public FormOperation(decimal? id, string code, string Name)
        {
            FormOperationID = id;
            CodeOperation = code;
            NameOperation = Name;
        }
        public decimal? FormOperationID
        {
            get;
            set;
        }
        public string CodeOperation
        {
            get;
            set;
        }
        public string NameOperation
        {
            get;
            set;
        }
        bool _is_checked = false;
        public bool IsChecked
        {
            get
            {
                return _is_checked;
            }
            set
            {
                _is_checked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        public override string ToString()
        {
            return CodeOperation;
        }
    }
    public class Degree : NotificationObject
    {
        public Degree(decimal? id, string code, string Name)
        {
            DegreeID = id;
            CodeDegree = code;
            DegreeName = Name;
        }
        public decimal? DegreeID
        {
            get;
            set;
        }
        public string CodeDegree
        {
            get;
            set;
        }
        public string DegreeName
        {
            get;
            set;
        }
        bool _is_checked = false;
        public bool IsChecked
        {
            get
            {
                return _is_checked;
            }
            set
            {
                _is_checked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        public override string ToString()
        {
            return CodeDegree;
        }
    }
}

namespace Staff
{
    public class NotificationObject : DependencyObject, INotifyPropertyChanged
    {
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            RaisePropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}