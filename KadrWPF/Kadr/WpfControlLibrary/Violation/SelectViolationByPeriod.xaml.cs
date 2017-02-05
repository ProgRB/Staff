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
using System.Globalization;
using LibraryKadr;
using Kadr.Vacation_schedule;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for SelectViolationByPeriod.xaml
    /// </summary>
    public partial class SelectViolationByPeriod : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string selectedDate)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selectedDate));
            }
        }

        private DateTime _selectedDateBegin;
        public DateTime SelectedDateBegin
        {
            get { return this._selectedDateBegin; }
            set
            {
                if (value != this._selectedDateBegin)
                {
                    this._selectedDateBegin = value;
                    OnPropertyChanged("SelectedDateBegin");
                }
            }
        }
        private DateTime _selectedDateEnd;
        public DateTime SelectedDateEnd
        {
            get { return this._selectedDateEnd; }
            set
            {
                if (value != this._selectedDateEnd)
                {
                    this._selectedDateEnd = value;
                    OnPropertyChanged("SelectedDateEnd");
                }
            }
        }

        public static readonly DependencyProperty AllItemsAreCheckedProperty =
            DependencyProperty.Register("AllItemsAreChecked", typeof(bool), typeof(SelectViolationByPeriod),
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
            foreach (DataRowView row in _ds.Tables["VIOLATION"].DefaultView)
            {
                row["FL"] = flagSelect;
            }
        }

        private static int flagSelect = 0;

        private static DataSet _ds;
        private static OracleDataAdapter _daViolation;
        private int _sign_report;
        public SelectViolationByPeriod(int sign_report)
        {
            InitializeComponent();
            _sign_report = sign_report;
            _daViolation.SelectCommand.Parameters["p_sign_report"].Value = _sign_report;
            dgList_Violations.ItemsSource = _ds.Tables["VIOLATION"].DefaultView;
            dpBegin.SelectedDate = DateTime.Today.AddDays(-1);
            dpEnd.SelectedDate = DateTime.Today;
            this.PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);
            LoadViolation();
            flagSelect = 0;
        }

        static SelectViolationByPeriod()
        {
            _ds = new DataSet();
            _ds.Tables.Add("VIOLATION");
            _ds.Tables.Add("REPORT");
            _ds.Tables.Add("SIGNES");
            _ds.Tables.Add("HEADING");

            _daViolation = new OracleDataAdapter(string.Format(Queries.GetQuery("PO/SelectViolationByPeriod.sql"),
                Connect.Schema), Connect.CurConnect);
            _daViolation.SelectCommand.BindByName = true;
            _daViolation.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date);
            _daViolation.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date);
            _daViolation.SelectCommand.Parameters.Add("p_sign_report", OracleDbType.Int16);
        }

        void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDateBegin" || e.PropertyName == "SelectedDateEnd")
            {
                LoadViolation();
            }
        }

        void LoadViolation()
        {
            _ds.Tables["VIOLATION"].Rows.Clear();
            _daViolation.SelectCommand.Parameters["p_begin_date"].Value = SelectedDateBegin;
            _daViolation.SelectCommand.Parameters["p_end_date"].Value = SelectedDateEnd.AddDays(1).AddSeconds(-1);            
            _daViolation.Fill(_ds.Tables["VIOLATION"]);
        }

        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            _ds.Tables["REPORT"].Rows.Clear();
            _ds.Tables["SIGNES"].Rows.Clear();
            _ds.Tables["HEADING"].Rows.Clear();
            OracleDataAdapter daReport = new OracleDataAdapter();
            daReport.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/RepViolationByPeriod.sql"), Connect.Schema), Connect.CurConnect);
            daReport.SelectCommand.BindByName = true;
            daReport.SelectCommand.Parameters.Add("p_TABLE", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
            DataView _dv = _ds.Tables["VIOLATION"].DefaultView.ToTable().DefaultView;
            _dv.RowFilter = "FL=1";
            daReport.SelectCommand.Parameters["p_TABLE"].Value =
                _dv.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("VIOLATION_ID")).ToArray();
            daReport.Fill(_ds.Tables["REPORT"]);
            if (_ds.Tables["REPORT"].Rows.Count > 0)
            {
                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "HeadingViolationByPeriod",
                    "Введите должность и ФИО лица", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                {
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                        Connect.CurConnect).Fill(_ds.Tables["HEADING"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        _ds.Tables["HEADING"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                    }

                    if (Signes.Show(0, "ViolationByPeriod",
                        "Введите должность и ФИО лица", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                    {
                        new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                            Connect.CurConnect).Fill(_ds.Tables["SIGNES"]);
                        for (int i = 0; i < s_pos.Count(); i++)
                        {
                            _ds.Tables["SIGNES"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                        }

                        ReportViewerWindow report =
                            new ReportViewerWindow(
                                "Сводка по нарушителям", "Reports/RepViolationByPeriod.rdlc", _ds,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                    new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", SelectedDateBegin.ToShortDateString()),
                                    new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", SelectedDateEnd.ToShortDateString()),
                                    new Microsoft.Reporting.WinForms.ReportParameter("P_SIGN_REPORT", _sign_report.ToString())}
                            );
                        report.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("За указанный период данных нет!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
