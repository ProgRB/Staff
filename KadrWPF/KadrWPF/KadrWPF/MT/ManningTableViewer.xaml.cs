using EntityGenerator;
using LibraryKadr;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibrarySalary.ViewModel;
using LibrarySalary.Helpers;

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for ManningTableViewer.xaml
    /// </summary>
    public partial class ManningTableViewer : UserControl
    {
        private ManningTableViewModel _model;
        private ViewTabBase _ownerTab;

        public ManningTableViewer()
        {
            _model = new ManningTableViewModel();
            _model.OnException += _model_OnException;
            InitializeComponent();
            _model.SubdivID = subdivSelector.SubdivView.Count > 0 ? (decimal?)subdivSelector.SubdivView[0]["SUBDIV_ID"] : null;
            this.Loaded += ManningTableViewer_Loaded;
            DataContext = Model;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SubdivID" && OwnerTab != null)
                OwnerTab.HeaderText = string.Format("Штатное расписание ({0})", Model.CodeSubdiv);
        }

        public LibrarySalary.ViewModel.ViewTabBase OwnerTab
        {
            get
            {
                return _ownerTab;
            }
            set
            {
                _ownerTab = value;
            }
        }

        private void ManningTableViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Model.RefreshStaffTable();
        }

        private void _model_OnException(object sender, Exception e, string text)
        {
            MessageBox.Show(Window.GetWindow(this), e.GetFormattedException(), text);
        }

        public ManningTableViewModel Model
        {
            get
            {
                return _model;
            }
        }

        private void Expander_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter)
                Model.RefreshStaffTable();
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.CurrentStaff != null;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StaffEditor f = new StaffEditor(Model.CurrentStaffID);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                Model.RefreshStaffTable();
            }
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StaffEditor f = new StaffEditor(Model.CurrentStaffID);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                Model.RefreshStaffTable();
                Model.SetCurrentStaffID(f.Model.StaffID);
            }
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), "Вы действительно хотите удалить выбранные штатные единицы?", "Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                Exception ex = Model.DeleteCurrentStaff();
                if (ex != null)
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка удаления данных");
                else
                    Model.RefreshStaffTable();
            }
        }

        private void EditEmpStaff_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.CurrentStaffID != null;
        }

        private void EditEmpStaff_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EmpStaffEditor f = new EmpStaffEditor(Model.CurrentStaffID.Value);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                Model.RefreshEmpStaff();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Model.RefreshEmpStaff();
        }
    }

    public class ManningTableViewModel : NotificationObject
    {
        private DateTime? _selectedDate = DateTime.Today;
        private decimal? _subdivID;
        DataSet ds;
        OracleDataAdapter odaStaff, odaEmpStaff;
        BackgroundWorker bw;

        public ManningTableViewModel()
        {
            ds = new DataSet();
            odaStaff = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectStaffView.sql"), Connect.CurConnect);
            odaStaff.SelectCommand.BindByName = true;
            odaStaff.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, null, ParameterDirection.Input);
            odaStaff.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaStaff.SelectCommand.Parameters.Add("p_form_operation_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaStaff.SelectCommand.Parameters.Add("p_code_degree", OracleDbType.Varchar2, null, ParameterDirection.Input);
            odaStaff.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
            odaStaff.TableMappings.Add("Table", "STAFF");

            odaEmpStaff = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectEmpStaff.sql"), Connect.CurConnect);
            odaEmpStaff.SelectCommand.BindByName = true;
            odaEmpStaff.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, null, ParameterDirection.Input);
            odaEmpStaff.SelectCommand.Parameters.Add("p_staff_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaEmpStaff.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
            odaEmpStaff.TableMappings.Add("Table", "EMP_STAFF");

            bw = new BackgroundWorker();
            bw.DoWork += LoadStaffTable;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
        }        

        private bool IsWaitUpdate = false;

        public void RefreshStaffTable()
        {
            if (bw.IsBusy)
            {
                IsWaitUpdate = true;
                return;
            }
            IsLoading = true;
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Занято ли загрузкой данных
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }

        }

        private void LoadStaffTable(object sender, DoWorkEventArgs e)
        {
            Exception ex = odaStaff.TryFillWithClear(ds, this);
            if (ex != null)
                throw ex;
        }

        /// <summary>
        /// Завершение загрузки данных - смотрим требуется ли загрузка, запускаем снова если требуется
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsWaitUpdate)
            {
                IsWaitUpdate = false;
                RefreshStaffTable();
            }
            else
            {
                if (e.Cancelled)
                {
                    IsLoading = false;
                    return;
                }
                else if (e.Error != null)
                {
                    IsLoading = false;
                    ThrowException(e.Error, "Ошибка получения данных");
                }
                else
                {
                    decimal? k = CurrentStaffID;
                    RaisePropertyChanged(() => StaffSource);
                    SetCurrentStaffID(k);
                    IsLoading = false;
                }
            }
        }

        public void SetCurrentStaffID(decimal? staffid)
        {
            CurrentStaff = StaffSource.Where(r => (decimal?)r["STAFF_ID"] == staffid).FirstOrDefault();
        }

        private void ThrowException(Exception error, string text)
        {
            if (OnException != null)
                OnException(this, error, text);
        }

        public event ExceptionEventHandler OnException;

        public delegate void ExceptionEventHandler(object sender, Exception e, string headerMessage);

        #region Поля для фильтрации
        /// <summary>
        /// Отчетная дата для фильтра расписания
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_date")]
        public DateTime? SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
                RaisePropertyChanged(() => CurrentBaseTariff);            }
            
        }

        /// <summary>
        /// Подразделение для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_subdiv_id")]
        public decimal? SubdivID
        {
            get
            {
                return _subdivID;
            }
            set
            {
                _subdivID = value;
                RaisePropertyChanged(() => SubdivID);
            }
        }

        /// <summary>
        /// Код выбранного подразделения
        /// </summary>
        public string CodeSubdiv
        {
            get
            {
                if (SubdivID == null) return string.Empty;
                else return WpfControlLibrary.AppDataSet.Tables["SUBDIV"].Rows.Cast<DataRow>().Where(r => r.Field2<decimal?>("SUBDIV_ID") == this.SubdivID).Select(r => r.Field2<string>("CODE_SUBDIV")).FirstOrDefault();
            }
        }

        string _currentCodeDegree;
        /// <summary>
        /// Категория для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_code_degree")]
        public string CurrentCodeDegree
        {
            get
            {
                return _currentCodeDegree;
            }
            set
            {
                _currentCodeDegree = value;
                RaisePropertyChanged(() => CurrentCodeDegree);
            }
        }

        decimal? _currentFormOpID;
        /// <summary>
        /// Категория для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_form_operation_id")]
        public decimal? CurrentCodeFormOperationID
        {
            get
            {
                return _currentFormOpID;
            }
            set
            {
                _currentFormOpID = value;
                RaisePropertyChanged(() => CurrentCodeFormOperationID);
            }
        }

        List<BaseTariff> _baseTarList;
        /// <summary>
        /// Текущий тариф базы коэффициента
        /// </summary>
        public decimal? CurrentBaseTariff
        {
            get
            {
                if (_baseTarList == null)
                    _baseTarList = WpfControlLibrary.ManningDataSet.Tables["BASE_TARIFF"].ConvertToEntityList<BaseTariff>();
                return _baseTarList?.FirstOrDefault(r => SelectedDate >= r.DateBegin && SelectedDate <= r.DateEnd)?.TariffValue;
            }
        }
        #endregion

        DataView _dvStaff, _dvEmpSource;
        private bool _isLoading = false;
        private DataRowView _currentStaff;

        #region Истонники данных
        /// <summary>
        /// Источник данных шттаное расписание
        /// </summary>
        public IList<DataRowView> StaffSource
        {
            get
            {
                if (_dvStaff == null && ds.Tables.Contains("STAFF"))
                    _dvStaff = new DataView(ds.Tables["STAFF"], "", "", DataViewRowState.CurrentRows);
                if (_dvStaff == null)
                    return new List<DataRowView>();
                else
                    return _dvStaff.Cast<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// Текущая выбранная штатная единица
        /// </summary>
        public DataRowView CurrentStaff
        {
            get
            {
                return _currentStaff;
            }
            set
            {
                _currentStaff = value;
                RaisePropertyChanged(() => CurrentStaff);
                RaisePropertyChanged(() => CurrentStaffID);
                RefreshEmpStaff();
            }
        }

        /// <summary>
        /// Поле для получения текущего айдишника штатной единиц
        /// </summary>
        [OracleParameterMapping(ParameterName="p_staff_id")]
        public decimal? CurrentStaffID
        {
            get
            {
                if (CurrentStaff != null && CurrentStaff.Row.RowState== DataRowState.Unchanged)
                    return CurrentStaff.Row.Field2<decimal?>("STAFF_ID");
                else
                    return null;
            }
        }



        public void RefreshEmpStaff()
        {
            Exception ex =  odaEmpStaff.TryFillWithClear(ds, this);
            if (ex != null)
                ThrowException(ex, "Ошибка получения данных");
            else
                RaisePropertyChanged(() => EmpStaffSource);
        }

        /// <summary>
        /// Удаление текущей штатной единицы
        /// </summary>
        /// <returns></returns>
        public Exception DeleteCurrentStaff()
        {
            OracleCommand cmd = new OracleCommand("begin APSTAFF.STAFF_DELETE(:p_staff_id);end;", Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_staff_id", OracleDbType.Decimal, CurrentStaffID, ParameterDirection.Input);
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }

        

        /// <summary>
        /// Источник данных - список сотрудников на штатных единицах
        /// </summary>
        public IList<DataRowView> EmpStaffSource
        {
            get
            {
                if (_dvEmpSource == null && ds.Tables.Contains("EMP_STAFF"))
                    _dvEmpSource = new DataView(ds.Tables["EMP_STAFF"], "", "", DataViewRowState.CurrentRows);
                if (_dvEmpSource == null)
                    return new List<DataRowView>();
                else
                    return _dvEmpSource.Cast<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// Список категорий для фильтра
        /// </summary>
        public IEnumerable<object> DegreeFilterSource
        {
            get
            {
                return WpfControlLibrary.AppDataSet.Tables["DEGREE"].Rows.OfType<DataRow>().Select(
                    r => new { CodeDegree = r.Field2<string>("CODE_DEGREE"), DegreeName = r.Field2<string>("DEGREE_NAME"), Level = new Thickness(3, 3, 3, 3) })
                    .Union(new string[][] {
                                    new string[] { "041", "Руководители" },
                                    new string[] { "042", "Специалисты" },
                                    new string[] { "043", "Прочие специалисты" },
                                    new string[] { "", "Все категории"}
                                }
                                .Select(r => new { CodeDegree = r[0], DegreeName = r[1], Level = (string.IsNullOrEmpty(r[0]) ? new Thickness(1, 3, 3, 3) : new Thickness(25, 3, 3, 3)) }))
                            .OrderBy(r => r.CodeDegree);
            }
        }

        /// <summary>
        /// Список видо производства
        /// </summary>
        public List<FormOperation> FormOperationSource
        {
            get
            {
                return WpfControlLibrary.AppDataSet.Tables["FORM_OPERATION"].ConvertToEntityList<FormOperation>();
            }
        }
        #endregion
    }
}
