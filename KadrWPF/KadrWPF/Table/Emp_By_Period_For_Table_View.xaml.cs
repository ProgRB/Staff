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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using Oracle.DataAccess.Types;
using Kadr;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Emp_By_Period_For_Table_View.xaml
    /// </summary>
    public partial class Emp_By_Period_For_Table_View : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string selectedDate)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selectedDate));
            }
        }

        public static readonly DependencyProperty AllItemsAreCheckedProperty =
            DependencyProperty.Register("AllItemsAreChecked", typeof(bool), typeof(Emp_By_Period_For_Table_View),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAllItemsAreCheckedChanged)));

        public bool AllItemsAreChecked
        {
            get
            {
                return (bool)GetValue(AllItemsAreCheckedProperty);
            }
            set
            {
                SetValue(AllItemsAreCheckedProperty, value);
            }
        }

        private static void OnAllItemsAreCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            flagSelect = Convert.ToInt32(!Convert.ToBoolean(flagSelect));
            foreach (DataRowView row in _ds.Tables["EMP"].DefaultView)
            {
                row["FL"] = flagSelect;
            }
        }

        private static int flagSelect = 0;

        private DateTime _beginPeriod;
        public DateTime BeginPeriod
        {
            get { return this._beginPeriod; }
            set
            {
                if (value != this._beginPeriod)
                {
                    this._beginPeriod = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }

        private DateTime _endPeriod;
        public DateTime EndPeriod
        {
            get { return this._endPeriod; }
            set
            {
                if (value != this._endPeriod)
                {
                    this._endPeriod = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }

        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }

        private static OracleDataAdapter _daEmp = new OracleDataAdapter(), _daTable_By_Period = new OracleDataAdapter();               

        private decimal _subdiv_id;

        public TimeExecute timeExecute;

        public Emp_By_Period_For_Table_View(decimal subdiv_id, DateTime dateDefault)
        {
            InitializeComponent();
            _subdiv_id = subdiv_id;
            dpBeginPeriod.SelectedDate = dateDefault;
            dpEndPeriod.SelectedDate = dateDefault;
            GetEmp(_subdiv_id, BeginPeriod, EndPeriod);
            this.PropertyChanged += new PropertyChangedEventHandler(Emp_By_Period_For_Table_View_PropertyChanged);
        }

        static Emp_By_Period_For_Table_View()
        {
            _ds.Tables.Add("EMP");
            _ds.Tables.Add("TABLE");
            // Select
            _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectEmp_By_Period_For_Table.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp.SelectCommand.BindByName = true;
            _daEmp.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            _daEmp.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date);
            _daEmp.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date);

            _daTable_By_Period.SelectCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TABLE_PKG.TABLE_BY_PERIOD(:p_SUBDIV_ID, :p_USER_NAME, :p_BEGIN_PERIOD, :p_END_PERIOD, :p_TABLE_ID, :p_cur_table);
                END;", Connect.Schema), Connect.CurConnect);
            _daTable_By_Period.SelectCommand.BindByName = true;
            _daTable_By_Period.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            _daTable_By_Period.SelectCommand.Parameters.Add("p_USER_NAME", OracleDbType.Varchar2);
            _daTable_By_Period.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date);
            _daTable_By_Period.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date);
            _daTable_By_Period.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
            _daTable_By_Period.SelectCommand.Parameters.Add("p_cur_table", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        }

        void Emp_By_Period_For_Table_View_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                GetEmp(_subdiv_id, BeginPeriod, EndPeriod);
            }
        }

        void GetEmp(decimal subdiv_id, DateTime beginPeriod, DateTime endPeriod)
        {
            dgEmp.DataContext = null;
            _ds.Tables["EMP"].Clear();
            AllItemsAreChecked = false;
            _daEmp.SelectCommand.Parameters["p_SUBDIV_ID"].Value = subdiv_id;
            _daEmp.SelectCommand.Parameters["p_BEGIN_PERIOD"].Value = beginPeriod;
            _daEmp.SelectCommand.Parameters["p_END_PERIOD"].Value = endPeriod;
            _daEmp.Fill(_ds.Tables["EMP"]);
            dgEmp.DataContext = _ds.Tables["EMP"].DefaultView;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btExecute_Click(object sender, RoutedEventArgs e)
        {
            _ds.Tables["EMP"].DefaultView.RowFilter = "FL=1";
            _daTable_By_Period.SelectCommand.Parameters["p_SUBDIV_ID"].Value = _subdiv_id;
            _daTable_By_Period.SelectCommand.Parameters["p_USER_NAME"].Value = Connect.UserId.ToUpper();
            _daTable_By_Period.SelectCommand.Parameters["p_BEGIN_PERIOD"].Value = BeginPeriod;
            _daTable_By_Period.SelectCommand.Parameters["p_END_PERIOD"].Value = EndPeriod;
            _daTable_By_Period.SelectCommand.Parameters["p_TABLE_ID"].Value =
                _ds.Tables["EMP"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("TRANSFER_ID")).ToArray();
            _ds.Tables["EMP"].DefaultView.RowFilter = "";
            _ds.Tables["TABLE"].Clear();
            // Создаем форму прогресса
            timeExecute = new TimeExecute(System.Windows.Forms.ProgressBarStyle.Marquee, false);            
            // Настраиваем что он должен выполнять
            timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
            {
                FillTable(timeExecute.backWorker, e1);
            });
            // Запускаем теневой процесс
            timeExecute.backWorker.RunWorkerAsync();
            // Отображаем форму
            timeExecute.ShowDialog();
            if (_ds.Tables["TABLE"].Rows.Count > 0)
            {
                ReportViewerWindow report = new ReportViewerWindow(
                    "Среднесписочная численность", "Reports/RepTable_By_Period.rdlc", _ds, null);
                report.Show();
            }
            else
            {
                MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Формирование табеля
        /// </summary>
        /// <param name="data"></param>
        void FillTable(object sender, DoWorkEventArgs e)
        {
            _daTable_By_Period.Fill(_ds.Tables["TABLE"]);            
        }
    }
}
