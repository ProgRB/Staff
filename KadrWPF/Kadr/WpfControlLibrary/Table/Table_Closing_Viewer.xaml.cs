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
using Kadr;
using Oracle.DataAccess.Types;

namespace WpfControlLibrary.Table
{
    /// <summary>
    /// Interaction logic for Table_Closing.xaml
    /// </summary>
    public partial class Table_Closing_Viewer : UserControl, INotifyPropertyChanged
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

        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }

        private int _countSubdiv = 0;
        public int CountSubdiv
        {
            get { return _countSubdiv; }
            set 
            {
                if (value != this._countSubdiv)
                {
                    this._countSubdiv = value;
                    OnPropertyChanged("CountSubdiv");
                }
                _countSubdiv = value; 
            }
        }

        private int _countClosingSubdiv = 0;
        public int CountClosingSubdiv
        {
            get { return _countClosingSubdiv; }
            set
            {
                if (value != this._countClosingSubdiv)
                {
                    this._countClosingSubdiv = value;
                    OnPropertyChanged("CountClosingSubdiv");
                }
                _countSubdiv = value;
            }
        }

        private int _countProcessingSubdiv = 0;
        public int CountProcessingSubdiv
        {
            get { return _countProcessingSubdiv; }
            set
            {
                if (value != this._countProcessingSubdiv)
                {
                    this._countProcessingSubdiv = value;
                    OnPropertyChanged("CountProcessingSubdiv");
                }
            }
        }

        FormMain formMain;
        private static OracleDataAdapter _daTable_Closing = new OracleDataAdapter();
        private static OracleCommand _ocDependencyTable_Closing, _ocTableForSalary, _ocSelSalary, _ocTable_List_Emp_PN_TMP;
        private OracleDependency _odTable_Closing;
        BackgroundWorker bw;
        public Table_Closing_Viewer(FormMain _formMain)
        {
            InitializeComponent();
            formMain = _formMain;  
            dgTable_Closing.DataContext = _ds.Tables["TABLE_CLOSING"].DefaultView;
            dcTABLE_PLAN_APPROVAL.ItemsSource = _ds.Tables["TABLE_PLAN_APPROVAL"].DefaultView;
            cbTYPE_TABLE.ItemsSource = _ds.Tables["TYPE_TABLE"].DefaultView;
            if (_ds.Tables["TYPE_TABLE"].DefaultView.Count > 1)
            {
                if (DateTime.Now.Day < 16)
                {
                    dpSelectedDate.SelectedDate = DateTime.Today.AddDays((DateTime.Today.Day - 1) * -1).AddMonths(-1);
                    cbTYPE_TABLE.SelectedValue = 2;
                }
                else
                {
                    dpSelectedDate.SelectedDate = DateTime.Today.AddDays((DateTime.Today.Day - 1) * -1);
                    cbTYPE_TABLE.SelectedValue = 1;
                }
            }

            GetTable_Closing();
            this.PropertyChanged += new PropertyChangedEventHandler(Table_Closing_PropertyChanged);
            cbTYPE_TABLE.SelectionChanged += new SelectionChangedEventHandler(cbTYPE_TABLE_SelectionChanged);
            RefreshDependency();

            bw = ((BackgroundWorker)this.FindResource("bw"));
        }

        static Table_Closing_Viewer()
        {
            _ds.Tables.Add("TABLE_CLOSING");
            _ds.Tables.Add("TABLE_CLOSING_ROW");
            _ds.Tables.Add("TABLE_PLAN_APPROVAL");
            _ds.Tables.Add("TYPE_TABLE");
            // Select
            _daTable_Closing.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("Table/SelectTable_Closing.sql"), Connect.Schema), Connect.CurConnect);
            _daTable_Closing.SelectCommand.BindByName = true;
            _daTable_Closing.SelectCommand.Parameters.Add("p_TABLE_DATE", OracleDbType.Date);
            _daTable_Closing.SelectCommand.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal);
            _daTable_Closing.SelectCommand.Parameters.Add("p_SIGN_NOTIFICATION", OracleDbType.Int16);
            _daTable_Closing.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".LONG_VARCHAR_COLLECTION_TYPE"; 

            new OracleDataAdapter(string.Format(
                @"select TABLE_PLAN_APPROVAL_ID, ROLE_NAME, NOTE_ROLE_NAME, TABLE_PLAN_APPROVAL_ID_PRIOR, TYPE_TABLE_ID, NOTE_ROLE_APPROVAL
                from {0}.TABLE_PLAN_APPROVAL order by TYPE_TABLE_ID, TABLE_PLAN_APPROVAL_ID", Connect.Schema),
                Connect.CurConnect).Fill(_ds.Tables["TABLE_PLAN_APPROVAL"]);
            new OracleDataAdapter(string.Format(
                @"select TYPE_TABLE_ID, TYPE_TABLE_NAME
                from {0}.TYPE_TABLE order by TYPE_TABLE_ID", Connect.Schema),
                Connect.CurConnect).Fill(_ds.Tables["TYPE_TABLE"]);
            
            // Создаем команду для расчета данных аванса
            _ocTableForSalary = new OracleCommand("", Connect.CurConnect);
            _ocTableForSalary.BindByName = true;
            _ocTableForSalary.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            _ocTableForSalary.Parameters.Add("p_beginDate", OracleDbType.Date);
            _ocTableForSalary.Parameters.Add("p_endDate", OracleDbType.Date);
            _ocTableForSalary.Parameters.Add("p_user_name", OracleDbType.Varchar2);
            _ocTableForSalary.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            _ocTableForSalary.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            _ocTableForSalary.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            
            // Создаем команду для запроса данных зарплаты
            _ocSelSalary = new OracleCommand("", Connect.CurConnect);
            _ocSelSalary.BindByName = true;
            _ocSelSalary.CommandText = string.Format(
                "begin {0}.TABLE_PKG.TABLE_EXPORT_TO_SALARY(:p_subdiv_id,:p_code_subdiv, :p_temp_salary_id, :p_begin_date, :p_end_date, :p_cur_table); end;",
                Connect.Schema);
            _ocSelSalary.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2);
            _ocSelSalary.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            _ocSelSalary.Parameters.Add("p_begin_date", OracleDbType.Date);
            _ocSelSalary.Parameters.Add("p_end_date", OracleDbType.Date);
            _ocSelSalary.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            _ocSelSalary.Parameters.Add("p_cur_table", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            
            // Создание таблицы сотрудников подразделения
            _ocTable_List_Emp_PN_TMP = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TABLE_PKG.TABLE_UNLOAD_TO_SALARY(:p_SUBDIV_ID, :p_BEGIN_PERIOD, :p_END_PERIOD);
                END;", Connect.Schema), Connect.CurConnect);
            _ocTable_List_Emp_PN_TMP.BindByName = true;
            _ocTable_List_Emp_PN_TMP.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date);
            _ocTable_List_Emp_PN_TMP.Parameters.Add("p_END_PERIOD", OracleDbType.Date);
            _ocTable_List_Emp_PN_TMP.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
        }

        void Table_Closing_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                GetTable_Closing();
            }
        }
        
        void d_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Info)
                {
                    /* case OracleNotificationInfo.Shutdown: App.appTrayIcon.ShowBalloonTip(10000, "Ошибка работы с сервером", "Сервер был временно отключен. Дождитесь возобновления работы сервера.", System.Windows.Forms.ToolTipIcon.Warning);

                         listNotify.Add(new AppNotify("Ошибка работы с сервером", "Сервер был временно отключен. Дождитесь возобновления работы сервера.")); break;
                     */
                    case OracleNotificationInfo.Update:
                        _daTable_Closing.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 1;
                        _daTable_Closing.SelectCommand.Parameters["p_TABLE_ID"].Value =
                            eventArgs.Details.Rows.OfType<DataRow>().Select(i => i["ROWID"].ToString()).ToArray();
                        _ds.Tables["TABLE_CLOSING_ROW"].Clear();
                        _daTable_Closing.Fill(_ds.Tables["TABLE_CLOSING_ROW"]);
                        _ds.Tables["TABLE_CLOSING"].PrimaryKey = new DataColumn[] { _ds.Tables["TABLE_CLOSING"].Columns["SUBDIV_ID"] };
                        for (int i = 0; i < _ds.Tables["TABLE_CLOSING_ROW"].Rows.Count; i++)
                        {
                            _ds.Tables["TABLE_CLOSING"].LoadDataRow(_ds.Tables["TABLE_CLOSING_ROW"].Rows[i].ItemArray, LoadOption.OverwriteChanges);
                        }
                        CountClosingSubdiv = _ds.Tables["TABLE_CLOSING"].DefaultView.Table.Select("SIGN_CLOSING = 1").Count();
                        break;
                    case OracleNotificationInfo.End:
                        RefreshDependency();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void RefreshDependency()
        {
            // Команда проверки обновления статуса проекта
            _ocDependencyTable_Closing = new OracleCommand(string.Format(
                @"select TABLE_CLOSING_ID, TABLE_PLAN_APPROVAL_ID from {0}.TABLE_CLOSING",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyTable_Closing.CommandType = CommandType.Text;
            _odTable_Closing = new OracleDependency(_ocDependencyTable_Closing);
            _odTable_Closing.QueryBasedNotification = true;
            _odTable_Closing.OnChange += new OnChangeEventHandler(d_OnChange);
            _ocDependencyTable_Closing.Notification.IsNotifiedOnce = false;
            _ocDependencyTable_Closing.Notification.Timeout = 3600;
            _ocDependencyTable_Closing.AddRowid = true;
            _ocDependencyTable_Closing.ExecuteNonQuery();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _odTable_Closing.OnChange -= d_OnChange;
                _odTable_Closing.RemoveRegistration(Connect.CurConnect);
            }
            catch { }
        }

        private void GetTable_Closing()
        {
            dgTable_Closing.DataContext = null;
            _ds.Tables["TABLE_CLOSING"].Clear();
            _daTable_Closing.SelectCommand.Parameters["p_TABLE_DATE"].Value = SelectedDate;
            _daTable_Closing.SelectCommand.Parameters["p_TYPE_TABLE_ID"].Value = cbTYPE_TABLE.SelectedValue;
            _daTable_Closing.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
            _daTable_Closing.SelectCommand.Parameters["p_TABLE_ID"].Value = null;

            _daTable_Closing.Fill(_ds.Tables["TABLE_CLOSING"]);
            dgTable_Closing.DataContext = _ds.Tables["TABLE_CLOSING"].DefaultView;
            CountSubdiv = _ds.Tables["TABLE_CLOSING"].DefaultView.Count;
            CountClosingSubdiv = _ds.Tables["TABLE_CLOSING"].DefaultView.Table.Select("SIGN_CLOSING = 1").Count();
            CountProcessingSubdiv = _ds.Tables["TABLE_CLOSING"].DefaultView.Table.Select("SIGN_PROCESSING = 1").Count();
        }

        void cbTYPE_TABLE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetTable_Closing();
        }

        private void btRefreshState_Click(object sender, RoutedEventArgs e)
        {
            GetTable_Closing();
        }

        private void UnloadTable_To_Salary_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                cbTYPE_TABLE != null && cbTYPE_TABLE.SelectedValue.ToString() == "2")
                e.CanExecute = true;
        }

        private void UnloadTable_To_Salary_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выгрузить данные табеля для расчета зарплаты?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //UnloadTable_To_Salary();
                progressBar.Value = 0;
                this.IsEnabled = false;
                bw.RunWorkerAsync();
            }
        }
        
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            DataRow[] _dt = _ds.Tables["TABLE_CLOSING"].DefaultView.ToTable().Select("SIGN_PROCESSING = 0 and SIGN_CLOSING = 1");
            int k = _dt.Count();
            for (int i = 0; i < k; i++)
            {
                if (bw.CancellationPending)
                    break;
                UnloadTable(_dt[i]["SUBDIV_ID"]);
                bw.ReportProgress(i + 1 / k * 100);
                _ds.Tables["TABLE_CLOSING"].Select("TABLE_CLOSING_ID = "+_dt[i]["TABLE_CLOSING_ID"].ToString()).First()["SIGN_PROCESSING"] = 1;
                CountProcessingSubdiv = _ds.Tables["TABLE_CLOSING"].DefaultView.Table.Select("SIGN_PROCESSING = 1").Count();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Сброс данных отменен");
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Ошибка получения данных");
            }
            this.IsEnabled = true;
            progressBar.Value = 100;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Заполнение таблицы работников
        /// </summary>
        /// <param name="subdiv_id">Идентификатор подразделения</param>
        void UnloadTable(object subdiv_id)
        {
            _ocTable_List_Emp_PN_TMP.Parameters["p_BEGIN_PERIOD"].Value = SelectedDate;
            _ocTable_List_Emp_PN_TMP.Parameters["p_END_PERIOD"].Value = SelectedDate.AddMonths(1).AddSeconds(-1);
            _ocTable_List_Emp_PN_TMP.Parameters["p_SUBDIV_ID"].Value = subdiv_id;
            _ocTable_List_Emp_PN_TMP.ExecuteNonQuery();
        }     
        
        private void EditList_Subdiv_Table_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List_Subdiv_Table_Viewer editSubdivFT = new List_Subdiv_Table_Viewer();
            editSubdivFT.ShowInTaskbar = false;
            if (editSubdivFT.ShowDialog() == true)
            {
                GetTable_Closing();
            }
        }

        private void SetSign_Processing_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateSing_Processing(1);
        }

        private void ClearSing_Processing_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateSing_Processing(0);
        }

        void UpdateSing_Processing(int sing_processing)
        {
            // Создаем список TABLE_CLOSING_ID чтобы по ним обновить признак обработки
            List<decimal?> _drv = new List<decimal?>();
            // Идем по выбранным строчкам
            for (int i = 0; i < dgTable_Closing.SelectedCells.Count; i++)
            {
                // Выбираем только закрытые подразделения
                if (((DataRowView)dgTable_Closing.SelectedCells[i].Item)["TABLE_CLOSING_ID"] != DBNull.Value &&
                    !_drv.Contains((decimal?)((DataRowView)dgTable_Closing.SelectedCells[i].Item)["TABLE_CLOSING_ID"]))
                {
                    _drv.Add((decimal?)((DataRowView)dgTable_Closing.SelectedCells[i].Item)["TABLE_CLOSING_ID"]);
                    ((DataRowView)dgTable_Closing.SelectedCells[i].Item)["SIGN_PROCESSING"] = sing_processing;
                }
            }
            OracleCommand _ocUpdateSign_Processing = new OracleCommand(string.Format(
                @"UPDATE {0}.TABLE_CLOSING TC SET SIGN_PROCESSING = :p_SIGN_PROCESSING 
                WHERE TABLE_CLOSING_ID in (SELECT COLUMN_VALUE FROM TABLE(:p_TABLE))",
                Connect.Schema), Connect.CurConnect);
            _ocUpdateSign_Processing.BindByName = true;
            _ocUpdateSign_Processing.Parameters.Add("p_SIGN_PROCESSING", OracleDbType.Decimal).Value = sing_processing;
            _ocUpdateSign_Processing.Parameters.Add("p_TABLE", OracleDbType.Array).Value = _drv.Select(i => (decimal)i).ToArray();
            //_ds.Tables["LIST_SUBDIV"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("SUBDIV_ID")).ToArray(); 
            _ocUpdateSign_Processing.Parameters["p_TABLE"].UdtTypeName = Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocUpdateSign_Processing.Transaction = transact;
                _ocUpdateSign_Processing.ExecuteNonQuery();
                transact.Commit();
                _ds.Tables["TABLE_CLOSING"].AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления признака обработки!\n" + ex.Message, "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                transact.Rollback();
                _ds.Tables["TABLE_CLOSING"].RejectChanges();
            }
        }

        private void EditTable_Closing_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgTable_Closing != null && dgTable_Closing.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditTable_Closing_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void miViewTable_Click(object sender, RoutedEventArgs e)
        {
            DataRowView _drCur = (DataRowView)dgTable_Closing.SelectedCells[0].Item;
            if (!FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows.Contains(_drCur["SUBDIV_ID"]))
            {
                System.Windows.MessageBox.Show("Нет доступа на просмотр данного подразделения в табеле!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            /// Переменная определяет если ли активные формы табеля
            bool flagActiveTable = false;
            /// Определяет создана ли уже форма работы с табелем
            for (int i = 0; i < formMain.MdiChildren.Length; i++)
            {
                /// Если это форма табеля
                if (formMain.MdiChildren[i].Name.ToUpper() == "TABLE")
                {
                    /// Активируем эту форму
                    formMain.MdiChildren[i].Activate();
                    flagActiveTable = true;
                }
            }
            /// Если на экране показаны формы табеля, то заходим
            if (flagActiveTable)
            {
                System.Windows.Forms.DialogResult drQuestion = System.Windows.Forms.MessageBox.Show("Открыть табель в новом окне?" +
                    "\n(Да - откроется новое окно табеля)" +
                    "\n(Нет - будет использоваться текущее окно табеля)",
                    "АРМ \"Учет рабочего времени\"",
                    System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
                if (drQuestion == System.Windows.Forms.DialogResult.No)
                {
                    ((Tabel.Table)formMain.ActiveMdiChild).Close();
                    /// Скрываем отображение элементов                
                    formMain.rgOperation.Visible = false;
                    formMain.rgOrders.Visible = false;
                }
                else
                    if (drQuestion == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
            }
            /// Проверяем на сколько подразделений дан доступ пользователю
            Tabel.Table table;
            if (FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows.Count == 1)
            {
                FormMain.flagFilter = true;
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                table = new Tabel.Table();
                table.subdiv_id =
                    Convert.ToInt32(FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_ID"]);
                table.code_subdiv =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["CODE_SUBDIV"].ToString();
                table.subdiv_name =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_NAME"].ToString();
                table.month = SelectedDate.Month;
                table.year = SelectedDate.Year;
                table.MdiParent = formMain;
                table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                table.Show();
                formMain.ClickErrorGraph(false);
                formMain.CreateButtonApp(table, formMain.btTable);
            }
            else
            {
                FormMain.flagFilter = true;
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                table = new Tabel.Table();
                table.subdiv_id = Convert.ToInt16(_drCur["SUBDIV_ID"]);
                table.code_subdiv = _drCur["CODE_SUBDIV"].ToString();
                table.subdiv_name = _drCur["SUBDIV_NAME"].ToString();
                table.month = SelectedDate.Month;
                table.year = SelectedDate.Year;
                table.MdiParent = formMain;
                table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                table.Show();
                formMain.ClickErrorGraph(false);
                formMain.CreateButtonApp(table, formMain.btTable);
            }
        }
    }


}
