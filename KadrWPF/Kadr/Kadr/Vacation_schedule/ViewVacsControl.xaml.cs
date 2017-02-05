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
using Staff;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Windows.Interop;

namespace Kadr.Vacation_schedule
{
    /// <summary>
    /// Interaction logic for ViewVacsControl.xaml
    /// </summary>
    public partial class ViewVacsControl : UserControl, IDataLinkKadr
    {
        private ViewVacsViewModel _model;

        public System.Windows.Forms.Form Owner32Window
        {
            get;
            set;
        }

        /// <summary>
        /// Конструктор создает модель и добавляем менюшки ссылки
        /// </summary>
        public ViewVacsControl()
        {
            _model = new ViewVacsViewModel();
            FilterVS_FilterChanged(this, EventArgs.Empty);
            FilterVS.FilterChanged += new EventHandler(FilterVS_FilterChanged);
            InitializeComponent();
            DataContext = Model;
            ctMenuGrid.Items.Add(ListLinkKadr.GetWPFMenuItem(this));
        }

        void FilterVS_FilterChanged(object sender, EventArgs e)
        {
            Model.CurrentYear = FilterVS.YearVS;
            Model.PerNum = FilterVS.per_num;
            Model.SubdivID = FilterVS.subdiv_id;
           // Model.CurrentCodeDegree = FilterVS.Degree_id
            Model.UpdateEmpSource();
        }

        public ViewVacsViewModel Model
        {
            get
            {
                return _model;
            }
        }

        private void Expander_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter)
                Model.UpdateEmpSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.UpdateEmpSource();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ViewOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewCard f = new ViewCard(Model.CurrentTransferID.ToString(), string.Empty);
            f.ShowDialog(Owner32Window);
            Model.UpdateVacSource();
        }

        /// <summary>
        /// Реализация для ссылок в на другие разделы
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public LinkData GetDataLink(object sender)
        {
            if (Model.CurrentEmp != null)
                return new LinkData(Model.CurrentEmp["PER_NUM"].ToString(), Model.CurrentEmp.Row.Field<Decimal>("transfer_id"));
            else
                return null;
        }

        private void PrintPreview_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportViewerWindow.ShowReport("Список сотрудников", "Rep_VacListPrint.rdlc", Model.EmpVacSource.ToTable(), null);
        }
    }

    /// <summary>
    /// Представление для просмотра данных по отпускам
    /// </summary>
    public class ViewVacsViewModel : NotificationObject
    {
        static ViewVacsViewModel()
        {
            OpenViewCard = new RoutedUICommand("Открыть карточку сотрудника", "ViewEmpVacs", typeof(ViewVacsViewModel));
        }
        DataSet ds;
        OracleDataAdapter odaEmps, odaVacs;
        public ViewVacsViewModel()
        {
            ds = new DataSet();
            odaEmps = new OracleDataAdapter(String.Format(Queries.GetQuery("Go/FillMakeVS.sql"),Connect.Schema),Connect.CurConnect);
            odaEmps.SelectCommand.BindByName = true;
            odaEmps.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date);
            odaEmps.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date);
            odaEmps.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(FilterVS.YearVS, 1, 1), ParameterDirection.Input);
            odaEmps.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
            odaEmps.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, FilterVS.per_num, ParameterDirection.Input);
            odaEmps.SelectCommand.Parameters.Add("p_actual_only", OracleDbType.Decimal, 0, ParameterDirection.Input);
            odaEmps.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            odaEmps.TableMappings.Add("Table", "EMPS_VAC");

            odaVacs = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\GetEmpVacForMakeGrid.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            odaVacs.SelectCommand.BindByName = true;
            odaVacs.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaVacs.TableMappings.Add("Table", "VACS");

        }

        decimal? _subdivID = null;
        /// <summary>
        /// Подразделение для фильтра и поиска
        /// </summary>
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

        string _fioFilter, _groupMasterFilter;

        /// <summary>
        /// Фильтр ФИО
        /// </summary>
        public string FIOFilter
        {
            get
            {
                return _fioFilter;
            }
            set
            {
                _fioFilter = value;
                RaisePropertyChanged(() => FIOFilter);
                SetEmpFilter(EmpDataViewFilter);
            }
        }

        /// <summary>
        /// Группа мастера фильтр
        /// </summary>
        public string GroupMasterFilter
        {
            get
            {
                return _groupMasterFilter;
            }
            set
            {
                _groupMasterFilter = value;
                RaisePropertyChanged(() => GroupMasterFilter);
                SetEmpFilter(EmpDataViewFilter);
            }
        }

        /// <summary>
        /// Установка фильтра для представления сотрудников
        /// </summary>
        /// <param name="filter"></param>
        private void SetEmpFilter(string filter)
        {
            if (_viewEmpVacs != null)
                _viewEmpVacs.RowFilter = filter;

        }

        /// <summary>
        /// Фильтр для представления
        /// </summary>
        private string EmpDataViewFilter
        {
            get
            {
                return string.Join(" and ",
                    new string[]
                    {
                        string.IsNullOrWhiteSpace(_groupMasterFilter)? null: string.Format("NAME_GROUP_MASTER like '{0}%'", _groupMasterFilter.ToUpper()),
                        string.IsNullOrWhiteSpace(_fioFilter)? null: string.Format("FIO like '{0}%'", _fioFilter.ToUpper()),
                        string.IsNullOrWhiteSpace(_codeDegree)? null: string.Format("CODE_DEGREE like '{0}%'", _codeDegree.ToUpper())
                    }.Where(r => r != null));
            }
        }

        /// <summary>
        /// Табельнй номер фильтр
        /// </summary>
        public string PerNum
        {
            get
            {
                return _perNum;
            }
            set
            {
                _perNum=value;
                RaisePropertyChanged(()=>PerNum);
            }
        }

        /// <summary>
        /// Текущий выбранный год
        /// </summary>
        public decimal CurrentYear
        {
            get
            {
                return _currentYear;
            }
            set
            {
                _currentYear = value;
                RaisePropertyChanged(()=>CurrentEmp);
            }
        }

        /// <summary>
        /// Выбранное начало отчетного года
        /// </summary>
        public DateTime? SelectedDate
        {
            get
            {
                return new DateTime(Convert.ToInt32(CurrentYear), 1, 1);
            }
        }

        /// <summary>
        /// Фильтр по категории сотрудника
        /// </summary>
        public string CurrentCodeDegree
        {
            get
            {
                return _codeDegree;
            }
            set
            {
                _codeDegree = value;
                RaisePropertyChanged(()=>_codeDegree);
                SetEmpFilter(EmpDataViewFilter);
            }
        }

        DataView _viewEmpVacs;
        /// <summary>
        /// Представление показа сотрудников
        /// </summary>
        public DataView EmpVacSource
        {
            get
            {
                if (_viewEmpVacs == null && ds.Tables.Contains("EMPS_VAC"))
                    _viewEmpVacs = new DataView(ds.Tables["EMPS_VAC"], "", "FIO", DataViewRowState.CurrentRows);
                return _viewEmpVacs;
            }        
        }

        DataRowView _currentEmp;
        private  string _perNum;
        private  decimal _currentYear = FilterVS.YearVS;
        private  string _codeDegree;
        private DataView _vacSource;

        /// <summary>
        /// Текущий выбранный сотрудник в списке
        /// </summary>
        public DataRowView CurrentEmp
        {
            get
            {
                return _currentEmp;
            }
            set
            {
                _currentEmp = value;
                RaisePropertyChanged(() => CurrentEmp);
                UpdateVacSource();
                if (_vacSource == null)
                    RaisePropertyChanged(() => VacSource);
            }
        }

        /// <summary>
        /// Текущий выбранный перевод сотрудника
        /// </summary>
        public decimal? CurrentTransferID
        {
            get
            {
                return CurrentEmp != null ? CurrentEmp.Row.Field2<Decimal?>("TRANSFER_ID") : null;
            }
        }
        /// <summary>
        /// Источник данных для списка отпусков
        /// </summary>
        public DataView VacSource
        {
            get
            {
                if (_vacSource == null && ds.Tables.Contains("VACS"))
                    _vacSource = new DataView(ds.Tables["VACS"], "", "", DataViewRowState.CurrentRows);
                return _vacSource;
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
                                .Select(r => new { CodeDegree = r[0], DegreeName = r[1], Level = (string.IsNullOrEmpty(r[0])? new Thickness(1, 3, 3, 3): new Thickness(25, 3, 3, 3)) }))
                            .OrderBy(r=>r.CodeDegree);
            }
        }

        /// <summary>
        /// Обновление данных по списку сотрудников
        /// </summary>
        public void UpdateEmpSource()
        {
            string cur_pernum = null;
            if (CurrentEmp != null)
                cur_pernum = CurrentEmp["PER_NUM"].ToString();
            if (ds != null && ds.Tables.Contains("EMPS_VAC"))
                ds.Tables["EMPS_VAC"].Rows.Clear();

            odaEmps.SelectCommand.Parameters["p_date"].Value = SelectedDate;
            odaEmps.SelectCommand.Parameters["p_per_num"].Value = PerNum;
            odaEmps.SelectCommand.Parameters["p_subdiv_id"].Value = SubdivID;
            odaEmps.SelectCommand.Parameters["p_date1"].Value = DateBeginFilter;
            odaEmps.SelectCommand.Parameters["p_date2"].Value = DateEndFilter;
            odaEmps.SelectCommand.Parameters["p_actual_only"].Value = IsActualOnly ? 1 : 0;
            try
            {
                odaEmps.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка получения списка сотрудников");
            }
            if (_viewEmpVacs == null)
                RaisePropertyChanged(() => EmpVacSource);
            if (cur_pernum != null)
                CurrentEmp = EmpVacSource.OfType<DataRowView>().Where(r => r.Row.Field2<string>("PER_NUM") == cur_pernum).FirstOrDefault();

        }

        /// <summary>
        /// Обновление отпусков по сотруднику
        /// </summary>
        public void UpdateVacSource()
        {
            if (ds.Tables.Contains("VACS"))
                ds.Tables["VACS"].Rows.Clear();
            odaVacs.SelectCommand.Parameters["p_transfer_id"].Value = CurrentTransferID;
            try
            {
                odaVacs.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка получения данных");
            }
        }

        bool _isAddDatesEnabled = false;
        /// <summary>
        /// Доступен ли фильтр по доп. датам отпуска
        /// </summary>
        public bool IsAddDatesEnabled
        {
            get
            {
                return _isAddDatesEnabled;
            }
            set
            {
                _isAddDatesEnabled = value;
                RaisePropertyChanged(()=>IsAddDatesEnabled);
            }
        }

        bool _isActualOnly;
        /// <summary>
        /// Только фактические даты отпусков
        /// </summary>
        public bool IsActualOnly
        {
            get
            {
                return _isActualOnly;
            }
            set
            {
                _isActualOnly = value;
                RaisePropertyChanged(()=>IsActualOnly);
            }
        }

        DateTime? _dateStartFilter = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), _dateFinishFilter = new DateTime(DateTime.Now.Year, 
            DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        /// <summary>
        /// Дата начала доп. фильтра по датам  начала отпуска
        /// </summary>
        public DateTime? DateStartFilter
        {
            get
            {
                return _dateStartFilter;
            }
            set
            {
                _dateStartFilter = value;
                RaisePropertyChanged(() => DateStartFilter);
            }
        }


        /// <summary>
        /// Дата фильтра - показывать или нет, иначе пустая дата возвращается
        /// </summary>
        public DateTime? DateBeginFilter
        {
            get
            {
                return IsAddDatesEnabled ? DateStartFilter : null;
            }
        }

        /// <summary>
        /// Дата окончания доп фильтра
        /// </summary>
        public DateTime? DateFinishFilter
        {
            get
            {
                return _dateFinishFilter;
            }
            set
            {
                _dateFinishFilter = value;
                RaisePropertyChanged(() => DateFinishFilter);
            }
        }

        /// <summary>
        /// Если доступны даты, то ставим их в параметры, иначе они пустые
        /// </summary>
        public DateTime? DateEndFilter
        {
            get
            {
                return IsAddDatesEnabled ? DateFinishFilter : null;
            }
        }

        public static RoutedUICommand OpenViewCard { get; set; }
    }
}
