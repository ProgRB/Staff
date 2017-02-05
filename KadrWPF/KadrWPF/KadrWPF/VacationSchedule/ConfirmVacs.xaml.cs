using EntityGenerator;
using LibraryKadr;
using LibrarySalary.Helpers;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
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

namespace VacationSchedule
{
    /// <summary>
    /// Interaction logic for ConfirmVacs.xaml
    /// </summary>
    public partial class ConfirmVacs : UserControl, IVacFilter
    {
        private ConfirmVSViewModel _model;

        public ConfirmVacs()
        {
            _model = new ConfirmVSViewModel();
            InitializeComponent();
            DataContext = Model;
            Model.SubdivID = subdivSelector1.SubdivView.OfType<DataRowView>().Select(r => r.Row.Field<decimal>("SUBDIV_ID")).FirstOrDefault();
            Unloaded += ConfirmVacs_Unloaded;
        }

        private void ConfirmVacs_Unloaded(object sender, RoutedEventArgs e)
        {
            Model.CancelCheck();
        }

        static ConfirmVacs()
        {
            CancelCheck = new RoutedUICommand("Отменить проверку отпусков", "bla bla", typeof(ConfirmVacs));
        }

        public ConfirmVSViewModel Model
        {
            get
            {
                return _model;
            }
        }

        public static RoutedUICommand CancelCheck { get; private set; }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Model.SetChecked((sender as CheckBox).IsChecked);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Model.LoadEmpList();
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.CurrentWorker != null;
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditVac f = new EditVac(Model.CurrentWorker["transfer_id"].ToString(), true);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Model.LoadEmpList();
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.CurrentWorker != null && Model.CurrentWorker.Row.Field2<decimal?>("VAC_SCHED_ID")!=null;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditVac f = new EditVac(Model.CurrentWorker.Row.Field2<decimal?>("VAC_SCHED_ID"), Model.CurrentWorker["transfer_id"].ToString(), true);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Model.LoadEmpList();
        }

        private void Recheck_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !Model.IsBusy;
        }

        private void Recheck_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.CheckListEmp();
        }

        private void CancelCheck_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Model != null && Model.IsBusy;
        }

        private void CancelCheck_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.CancelCheck();
        }

        private void Menu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }

        private void AllStat_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ConfirmPercentView f = new ConfirmPercentView(Model.CurrentYear);
            f.TopMost = true;
            f.Show();
        }

        private void KardVac_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VacationSchedule.ViewCard f = new VacationSchedule.ViewCard(Model.CurrentWorker.Row.Field<decimal>("TRANSFER_ID").ToString(), string.Empty);
            f.ShowDialog();
        }

        private void Confirm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.SelectedWorkers.Length > 0;
        }

        private void Confirm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), "Вы действительно хотите согласовать выбранные отпуска?", "Графики отпусков", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                Exception ex = Model.SetConfirmed(true);
                if (ex != null)
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка обновления данных");
                else
                    Model.LoadEmpList();
            }
        }

        private void CancelConfirm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), "Вы действительно хотите отменить согласование на выбранные отпуска?", "Графики отпусков", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Exception ex = Model.SetConfirmed(false);
                if (ex != null)
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка обновления данных");
                else
                    Model.LoadEmpList();
            }
        }

        public string GetPerNum()
        {
            return string.Empty;
        }

        public decimal? GetSubdivID()
        {
            return Model.SubdivID;
        }

        public string GetCodeSubdiv()
        {
            return WpfControlLibrary.AppDataSet.Tables["SUBDIV"].AsEnumerable().Where(r => r.Field2<decimal?>("SUBDIV_ID") == Model.SubdivID).Select(r=>r["CODE_SUBDIV"].ToString()).FirstOrDefault();
        }

        public string GetSubdivName()
        {
            return WpfControlLibrary.AppDataSet.Tables["SUBDIV"].AsEnumerable().Where(r => r.Field2<decimal?>("SUBDIV_ID") == Model.SubdivID).Select(r => r["SUBDIV_NAME"].ToString()).FirstOrDefault();
        }

        public int GetCurrentYear()
        {
            return Model.CurrentYear;
        }

        private void GroupBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Model.LoadEmpList();
        }
    }

    public partial class ConfirmVSViewModel : NotificationObject
    {
        BackgroundWorker bw;
        OracleDataAdapter odaConfirm;
        private DataRowView _currentWorker;
        DataSet ds;
        private decimal? _subdivID;

        public ConfirmVSViewModel()
        {
            ds = new DataSet();
            ds.Tables.Add("ConfirmVS");
            ds.Tables["ConfirmVS"].Columns.Add("FL", typeof(bool)).DefaultValue = false;

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.ProgressChanged += Bw_ProgressChanged;

            odaConfirm = new OracleDataAdapter(Queries.GetQueryWithSchema("GO/GetVacForConfirm.sql"), Connect.CurConnect);
            odaConfirm.SelectCommand.BindByName = true;
            odaConfirm.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, SelectedDate, ParameterDirection.Input);
            odaConfirm.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, SubdivID, ParameterDirection.Input);
            odaConfirm.TableMappings.Add("Table", "ConfirmVS");
            AutoCheck = false;
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CheckProgress = e.ProgressPercentage;
        }

        /// <summary>
        /// Процент проверки отпусков
        /// </summary>
        public int CheckProgress
        {
            get
            {
                return _checkProgress;
            }
            set
            {
                _checkProgress = value;
                RaisePropertyChanged(() => CheckProgress);
            }
        }

        [OracleParameterMapping(ParameterName ="p_date")]
        public DateTime? SelectedDate
        {
            get
            {
                return new DateTime(CurrentYear, 1, 1);
            }
        }

        public int CurrentYear
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                RaisePropertyChanged(() => CurrentYear);
            }
        }

        /// <summary>
        /// Подразделение выбранное 
        /// </summary>
        [OracleParameterMapping(ParameterName ="p_subdiv_id")]
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
        /// Воркер айди выбранного работника
        /// </summary>
        public decimal? CurrentWorkerID
        {
            get
            {
                if (CurrentWorker == null)
                    return null;
                else
                    return _currentWorker.Row.Field<decimal>("WORKER_ID");
            }
        }

        /// <summary>
        /// Текущий выбранный сотрудник
        /// </summary>
        public DataRowView CurrentWorker
        {
            get
            {
                return _currentWorker;
            }
            set
            {
                _currentWorker = value;
                RaisePropertyChanged(() => CurrentWorker);
                RaisePropertyChanged(() => CurrentWorkerID);
            }
        }

        DataView _dvEmp;
        private int _year = DateTime.Today.DayOfYear>220? DateTime.Today.Year+1:DateTime.Today.Year;
        private int _checkProgress;

        /// <summary>
        /// Источик данных для списка
        /// </summary>
        public List<DataRowView> ConfirmEmpSource
        {
            get
            {
                if (_dvEmp == null)
                    return new List<DataRowView>();
                else
                    return _dvEmp.OfType<DataRowView>().ToList();
            }
        }

        public bool AutoCheck
        {
            get;
            set;
        }

        /// <summary>
        /// Загрузка списка для данных сотрудников
        /// </summary>
        public void LoadEmpList()
        {
            decimal? workerId = CurrentWorkerID;
            odaConfirm.SelectCommand.SetParameters(this);
            if (ds.Tables.Contains("ConfirmVS"))
                ds.Tables["ConfirmVS"].Rows.Clear();
            try
            {
                if (bw.IsBusy)
                    bw.CancelAsync();
                CheckProgress = 0;
                odaConfirm.Fill(ds);
                if (_dvEmp == null)
                _dvEmp = new DataView(ds.Tables["ConfirmVS"], "", "", DataViewRowState.CurrentRows);
                if (workerId != null)
                    CurrentWorker = ConfirmEmpSource.Where(r => r.Row.Field2<Decimal?>("WORKER_ID") == workerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetFormattedException(), "Ошибка получения списка сотрудников");
            }

            RaisePropertyChanged(() => ConfirmEmpSource);

            if (AutoCheck)
            {
                CheckListEmp();
            }
            
        }

        public void CheckListEmp()
        {
            if (bw.IsBusy)
                bw.CancelAsync();
            while (bw.CancellationPending) ;
            bw.RunWorkerAsync();
            RaisePropertyChanged(() => IsBusy);
        }

        public bool IsBusy
        {
            get
            {
                return bw != null && bw.IsBusy;
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) return;
            if (e.Error != null)
                MessageBox.Show(e.Error.GetFormattedException(), "Ошибка проверки списка");
            RaisePropertyChanged(() => IsBusy);
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable t = new DataTable();
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery(@"go/CheckVSForConfirm.sql"), Connect.Schema), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_worker_ids", OracleDbType.Array, null, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
            List<Tuple<decimal, DataRowView>> workers = new List<Tuple<decimal, DataRowView>>();
            HashSet<decimal> hsEmps = new HashSet<decimal>();
            for (int i = 0; i < _dvEmp.Count; ++i)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                decimal workerid = _dvEmp[i].Row.Field2<Decimal>("worker_id");
                if (!hsEmps.Contains(workerid))
                {
                    hsEmps.Add(workerid);
                }
                workers.Add(new Tuple<decimal, DataRowView>(workerid, _dvEmp[i]));
                if (workers.Count > Math.Max(_dvEmp.Count / 100, 10) || i+1==_dvEmp.Count) // собираем пачками для проверки
                {
                    cmd.Parameters["p_worker_ids"].Value = hsEmps.ToArray(); 
                    t.Rows.Clear();
                    new OracleDataAdapter(cmd).Fill(t);
                    Dictionary<decimal, string> dic = t.Rows.OfType<DataRow>().ToDictionary(r => r.Field<decimal>("WORKER_ID"), r => r.Field<string>("ERR_TEXT"));
                    foreach (var p in workers)
                    {
                        if (dic.ContainsKey(p.Item1))
                            p.Item2["FL_CHECK"] = dic[p.Item1];
                        else
                            p.Item2["FL_CHECK"] = string.Empty;
                    }
                    hsEmps.Clear();
                    workers.Clear();
                }
                bw.ReportProgress(Convert.ToInt32(Math.Round(100*((i + 1m) / _dvEmp.Count))));
            }
        }

        internal void SetChecked(bool? isChecked)
        {
            foreach (var p in ConfirmEmpSource)
                p["FL"] = isChecked;
        }

        /// <summary>
        /// Прерывание проверки отпусков
        /// </summary>
        internal void CancelCheck()
        {
            if (bw.IsBusy) bw.CancelAsync();
            CheckProgress = 0;
        }

        /// <summary>
        /// Адишники выбранных отпусков
        /// </summary>
        public decimal?[] SelectedWorkers
        {
            get
            {
                if (ConfirmEmpSource == null)
                    return null;
                else
                    return ConfirmEmpSource.Where(r=>r.Row.Field2<bool>("FL")).Select(r => r.Row.Field2<decimal?>("VAC_SCHED_ID")).Where(r => r != null).ToArray();
            }
        }

        internal Exception SetConfirmed(bool v)
        {
            OracleCommand cmd = new OracleCommand(string.Format("begin {0}.CONFIRM_VAC(:p_vacs, :p_sign);end;", Connect.Schema), Connect.CurConnect);
            cmd.Parameters.Add("p_vacs", OracleDbType.Varchar2, "", ParameterDirection.Input);
            cmd.Parameters.Add("p_sign", OracleDbType.Decimal, v?1:0, ParameterDirection.Input);
                OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                string[] l = SelectedWorkers.Select(u => u.ToString()).ToArray();
                if (l.Length == 0)
                {
                    tr.Commit(); return null;
                }
                for (int i = 0; i < l.Length / 500; ++i)
                {
                    cmd.Parameters["p_vacs"].Value = string.Join(",", l, 500 * i, 500);
                    cmd.ExecuteNonQuery();
                }
                if (l.Length % 500 > 0)
                {
                    cmd.Parameters["p_vacs"].Value = string.Join(",", l.ToArray(), l.Length - l.Length % 500, l.Length % 500);
                    cmd.ExecuteNonQuery();
                }
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }
    }
}
