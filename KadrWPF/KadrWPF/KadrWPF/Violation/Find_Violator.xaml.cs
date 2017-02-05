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
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Data;
using System.ComponentModel;
using System.Globalization;

namespace Pass_Office
{
    /// <summary>
    /// Interaction logic for Find_Violator.xaml
    /// </summary>
    public partial class Find_Violator : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string selectedDate)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selectedDate));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return this._selectedDate; }
            set
            {
                if (value != this._selectedDate)
                {
                    this._selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }
        private decimal _emp_id;
        public decimal Emp_ID{ get { return _emp_id; }}

        private string _per_num;
        public string Per_Num{ get { return _per_num; }}

        private string _last_name;
        public string Last_Name{ get { return _last_name; }}

        private string _first_name;
        public string First_Name { get { return _first_name; } }

        private string _middle_name;
        public string Middle_Name { get { return _middle_name; } }

        private string _comb;
        public string Comb{get { return _comb; }}

        private string _pos_name;
        public string Pos_Name{ get { return _pos_name; }}

        private string _subdiv_name;
        public string Subdiv_Name { get { return _subdiv_name; } }

        private string _code_subdiv;
        public string Code_Subdiv { get { return _code_subdiv; } }

        private int _signEmp;
        public int SignEmp { get { return _signEmp; } }

        private static DataSet _ds = new DataSet();
        private static OracleDataAdapter _daEmp = new OracleDataAdapter();
        /// <summary>
        /// Форма выбора работников
        /// </summary>
        /// <param name="signEmp">Признак работников при фильтрации: 1 - сотрудники завода, 2 - сторонние сотрудники</param>
        public Find_Violator(int signEmp)
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Find_Violator_PropertyChanged);
            _signEmp = signEmp;
            InitializeComponent();
            if (SignEmp == 1)
            {
                _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("PO/SelectEmp.sql"),
                    Connect.Schema), Connect.CurConnect);
            }
            else
            {
                _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("PO/SelectFR_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
            }
            _daEmp.SelectCommand.BindByName = true;
            _daEmp.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            _daEmp.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            _ds.Tables["EMP"].DefaultView.RowFilter = "";
            dgEmp.ItemsSource = _ds.Tables["EMP"].DefaultView;
            tbLast_Name.Focus();

            dpPeriod.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }

        void Find_Violator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                GetEmp();
            }
        }

        static Find_Violator()
        {         
            _ds.Tables.Add("EMP");
        }

        void GetEmp()
        {
            dgEmp.DataContext = null;
            _ds.Tables["EMP"].Clear();
            _daEmp.SelectCommand.Parameters["p_date_begin"].Value = SelectedDate;
            _daEmp.SelectCommand.Parameters["p_date_end"].Value = SelectedDate.AddMonths(1).AddSeconds(-1);
            _daEmp.Fill(_ds.Tables["EMP"]);
            dgEmp.DataContext = _ds.Tables["EMP"].DefaultView;
        }

        private void Find_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strFilter = "";
            if (tbPer_Num.Text.Trim() != "")
                strFilter = "PER_NUM like '%" + tbPer_Num.Text.Trim() + "%'";
            if (tbLast_Name.Text.Trim() != "")
                strFilter = (strFilter != "" ? strFilter + " and " : "") + "LAST_NAME like '" + tbLast_Name.Text.Trim().ToUpper() + "%'";
            if (tbFirst_Name.Text.Trim() != "")
                strFilter = (strFilter != "" ? strFilter + " and " : "") + "FIRST_NAME like '" + tbFirst_Name.Text.Trim().ToUpper() + "%'";
            if (tbMiddle_Name.Text.Trim() != "")
                strFilter = (strFilter != "" ? strFilter + " and " : "") + "MIDDLE_NAME like '" + tbMiddle_Name.Text.Trim().ToUpper() + "%'";
            _ds.Tables["EMP"].DefaultView.RowFilter = strFilter;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dgEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WpfControlLibrary.Pass_Office_Commands.SelectEmp.Execute(null, null);
        }

        private void SelectEmp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (dgEmp != null && dgEmp.SelectedItem != null)
            {
                e.CanExecute = true;
            }
        }

        private void SelectEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView drView = dgEmp.SelectedItem as DataRowView;
            _emp_id = Convert.ToDecimal(drView["EMP_ID"]);
            _per_num = drView["PER_NUM"].ToString();
            _last_name = drView["LAST_NAME"].ToString();
            _first_name = drView["FIRST_NAME"].ToString();
            _middle_name = drView["MIDDLE_NAME"].ToString();
            _pos_name = drView["POS_NAME"].ToString();
            _subdiv_name = drView["SUBDIV_NAME"].ToString();
            _code_subdiv = drView["CODE_SUBDIV"].ToString();
            this.DialogResult = true;
            this.Close();
        }

        private void btSelectMonth_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "PreviousMonth")
            {
                SelectedDate = SelectedDate.AddMonths(-1);
            }
            else
            {
                SelectedDate = SelectedDate.AddMonths(1);
            }
        }
    }
}
