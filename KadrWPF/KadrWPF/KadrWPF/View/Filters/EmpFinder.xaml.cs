using EntityGenerator;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace KadrWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EmpFinder : Window
    {
        DataSet ds = new DataSet();
        OracleDataAdapter a;
        public EmpFinder()
        {
            InitializeComponent();
            a = new OracleDataAdapter(LibraryKadr.Queries.GetQueryWithSchema(@"Filters/SelectFindEmps.sql"), LibraryKadr.Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_fio", OracleDbType.Varchar2, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, ParameterDirection.Input);
            a.TableMappings.Add("Table", "Emps");
            this.DataContext = this;
        }

        static EmpFinder()
        {
            SelectEmp = new RoutedUICommand("Выбрать сотрудника", "SelectEmp", typeof(EmpFinder));
        }

        public static RoutedUICommand SelectEmp
        {
            get;
            set;
        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ds.Tables.Contains("Emps"))
                    ds.Tables["Emps"].Clear();
                a.SelectCommand.Parameters["p_fio"].Value = EmpFilter.FIO;
                a.SelectCommand.Parameters["p_per_num"].Value = EmpFilter.PerNum;
                a.SelectCommand.Parameters["p_subdiv_id"].Value = EmpFilter.SubdivID;
                a.SelectCommand.Parameters["p_date"].Value = EmpFilter.SelectedDate;
                a.Fill(ds, "Emps");
                if (dgEmps.ItemsSource == null)
                {
                    dgEmps.ItemsSource = new DataView(ds.Tables["Emps"], "", "DATE_TRANSFER desc", DataViewRowState.CurrentRows);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetFormattedException(), "Ошибка получения данных");
            }
        }

        private void Group_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btFind_Click(this, null);
        }

        DataRowView _selectedRow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedRow;
            }
            set
            {
                _selectedRow = value;
            }

        }

        /// <summary>
        /// Полное имя выбранного сотрудника
        /// </summary>
        public string FullFIO
        {
            get
            {
                if (SelectedRow != null)
                    return $"{SelectedRow["EMP_LAST_NAME"]} {SelectedRow["EMP_FIRST_NAME"]} {SelectedRow["EMP_MIDDLE_NAME"]}";
                return string.Empty;
            }
        }

        /// <summary>
        /// Табельный номер сотрудника
        /// </summary>
        public string PerNum
        {
            get
            {
                return SelectedRow?["PER_NUM"].ToString();
            }
        }

        /// <summary>
        /// Перевод сотрудника
        /// </summary>
        public decimal? TransferID
        {
            get
            {
                return SelectedRow?.Row.Field2<Decimal?>("TRANSFER_ID");
            }

        }

        EmpFilter _empFilter;
        /// <summary>
        /// Фильтр для подразделений
        /// </summary>
        public EmpFilter EmpFilter
        {
            get
            {
                if (_empFilter == null)
                    _empFilter = new EmpFilter();
                return _empFilter;
            }
        }


       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = dgEmps.SelectedItem != null;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }

    public class EmpFilter : NotificationObject
    {
        string _fio, _pernum;

        /// <summary>
        /// ФИО сотрудника
        /// </summary>
        public string FIO
        {
            get
            {
                return _fio;
            }
            set
            {
                _fio = value;
                RaisePropertyChanged(() => FIO);
            }
        }

        /// <summary>
        /// Таб. № сотрудника
        /// </summary>
        public string PerNum
        {
            get
            {
                return _pernum;
            }
            set
            {
                _pernum = value;
                RaisePropertyChanged(() => PerNum);
            }
        }
        decimal? _subdivID;
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

        DateTime? _selectedDate = DateTime.Today;
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
            }
        }
    }


}
