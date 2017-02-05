using Kadr;
using Kadr.Shtat;
using LibraryKadr;
using LibrarySalary.ViewModel;
using Microsoft.Reporting.WinForms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
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
using TabDayLibrary;
using Tabel;
using WpfControlLibrary;
using WExcel = Microsoft.Office.Interop.Excel;

namespace KadrWPF.Table
{
    /// <summary>
    /// Логика взаимодействия для Table_Viewer.xaml
    /// </summary>
    public partial class Table_Viewer : UserControl, INotifyPropertyChanged, IDataLinkKadr
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

        public Visibility CanVisibleSalary
        {
            get
            {
                if (GrantedRoles.GetGrantedRole("SALARY_VIEW"))
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public OracleDataTable dtEmp;
        decimal transfer_id;
        public TabDayGrid TDG;
        /// <summary>
        /// Флаг отображения дней заполнения табеля
        /// </summary>
        public bool flagVisible;
        /// <summary>
        /// Флаг определяет нужно ли перезаполнять форму табеля
        /// </summary>
        public bool flagReload;
        /// <summary>
        /// Идентификатор подразделения, с которым сейчас работаем
        /// </summary>
        private decimal? _subdiv_id;
        public decimal? Subdiv_ID
        {
            get { return this._subdiv_id; }
            set
            {
                if (value != this._subdiv_id)
                {
                    this._subdiv_id = value;
                    OnPropertyChanged("Subdiv_ID");
                }
            }
        }

        DataRowView _currentEmpRow;
        /// <summary>
        /// Текущий выбранный сотрудник в таблице
        /// </summary>
        public DataRowView CurrentEmp
        {
            get
            {
                return _currentEmpRow;
            }
            set
            {
                _currentEmpRow = value;
                OnPropertyChanged("CurrentEmp");
                OnPropertyChanged("CurrentWorkerID");
            }
        }
        /// <summary>
        /// WorkerID выбранного работника
        /// </summary>
        public decimal? CurrentWorkerID
        {
            get
            {
                return CurrentEmp?.Row.Field2<Decimal?>("WORKER_ID");
            }
        }
        /// <summary>
        /// Текущий перевод сотрудника
        /// </summary>
        public decimal? CurrentTransferID
        {
            get
            {
                return CurrentEmp?.Row.Field2<Decimal?>("TRANSFER_ID");
            }
        }


        /// <summary>
        /// Код подразделения, с которым сейчас работаем
        /// </summary>
        public string code_subdiv;
        public string curWorker_ID = "";
        /// <summary>
        /// Дата начала периода отображения табеля
        /// </summary>
        private DateTime _beginDate = DateTime.Now;
        public DateTime BeginDate
        {
            get { return _beginDate; }
            set { _beginDate = value; }
        }
        /// <summary>
        /// Дата окончания периода отображения табеля
        /// </summary>
        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        /// <summary>
        /// Дата, на которой производится редактирование
        /// </summary>
        TabDayItem RestoreItem;

        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        public TimeExecute timeExecute;

        OracleDataTable dtWork_pay_type, dtHoursCalc, dtHoursPeriod, dtWorked_day, dtAbsence, dtWorkOut,
            dtDays_calendar, dtReg_doc, dtEmpErrors, dtHoursSaved; //dtTotalHours, 

        OracleCommand ocClosedTable, ocDateClose, ocDeleteReg_Doc, ocCalc_Absence, _ocDependencyCloseTable,
            _ocTable_Closing_ID, _ocGet_Status_Closing, _ocDependencyTable_Closing, _ocSign_Table_Closing;
        OracleDependency _odCloseTable, _odTable_Closing;
        DataSet _dsTable_Approval;
        OracleDataAdapter _daPN_TMP, _daTable_Approval, _daType_Approval, _daPlan_Approval = new OracleDataAdapter()
            //, _daAppendix = new OracleDataAdapter()
            ;

        DataRowView _curRowEmp, _row_Table_Approval, _row_Advance_Approval;
        private decimal? _table_Closing_ID;
        
        public decimal? Table_Closing_ID
        {
            get { return _table_Closing_ID; }
            set { _table_Closing_ID = value; }
        }

        private decimal? _advance_Closing_ID;
        public decimal? Advance_Closing_ID
        {
            get { return _advance_Closing_ID; }
            set { _advance_Closing_ID = value; }
        }

        /// <summary>
        /// Доступное время отгулов в часах
        /// </summary>
        private static decimal hoursAbsence = 0;

        /// <summary>
        /// Переменная для хранения идентификатора заказа
        /// </summary>
        int order_id;

        /// <summary>
        /// Признак закрытия табеля для редактирования
        /// </summary>
        public bool flagClosedTable;
        /// <summary>
        /// Дата закрытия табеля на зп
        /// </summary>
        public static DateTime dCloseTable = DateTime.Now.AddYears(-1);
        System.Windows.Forms.Panel pnTable;
        
        /// <summary>
        /// Форма выбора периода для отчета
        /// </summary>
        SelectPeriod selPeriod;
        // Переменные для определения какие листы табеля нужно формировать
        bool fl_form_title = false;
        bool fl_form_table = false;
        bool fl_form_appendix = false;
        /// <summary>
        /// Команда для различных запросов при работе с табелем
        /// </summary>
        OracleCommand command;
        /// <summary>
        /// Переменная определяет, нужно ли разбивать табель по заказам или нет
        /// </summary>
        bool fl_break_order = true;

        public Table_Viewer()
        {
            this.PropertyChanged += Table_Viewer_PropertyChanged;
            InitializeComponent();
            pnTable = new System.Windows.Forms.Panel();
            windowsFormsHostTable.Child = pnTable;
            dpPeriod.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            BeginDate = SelectedDate;
            EndDate = BeginDate.AddMonths(1).AddSeconds(-1);
            Load_Table();

            RefreshDependencyCloseTable();
            RefreshDependency();
            
            dgEmp.ContextMenu.Items.Add(ListLinkKadr.GetWPFMenuItem(this));
            dcSalary.Visibility = CanVisibleSalary;
            dcSum_Salary_Calc.Visibility = CanVisibleSalary;
            dcSum_Salary_Saved.Visibility = CanVisibleSalary;            
        }

        void RefreshDependencyCloseTable()
        {
            _ocDependencyCloseTable = new OracleCommand(string.Format(
                @"select SUBDIV_ID, DATE_SALARY from {0}.SUBDIV_FOR_TABLE",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyCloseTable.CommandType = CommandType.Text;
            _odCloseTable = new OracleDependency(_ocDependencyCloseTable);
            _odCloseTable.QueryBasedNotification = true;
            _odCloseTable.OnChange += new OnChangeEventHandler(d_OnChange);
            _ocDependencyCloseTable.Notification.IsNotifiedOnce = false;
            _ocDependencyCloseTable.Notification.Timeout = 3600;
            _ocDependencyCloseTable.AddRowid = true;
            _ocDependencyCloseTable.ExecuteNonQuery();
        }

        void d_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Info)
                {
                    case OracleNotificationInfo.Update:
                        RefreshFlagClosedTable();
                        break;
                    case OracleNotificationInfo.End:
                        RefreshDependencyCloseTable();
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

        /// <summary>
        /// Процедура обновления признака закрытия табеля
        /// </summary>
        void RefreshFlagClosedTable()
        {
            ocClosedTable.Parameters["p_subdiv_id"].Value = Subdiv_ID;
            ocClosedTable.Parameters["p_date_end"].Value = EndDate;
            int rowCount = Convert.ToInt32(ocClosedTable.ExecuteScalar());
            /// Если дата закрытия табеля меньше или равна дате конца табеля, то закрывает табель
            if (rowCount > 0)
            {
                flagClosedTable = true;
            }
            else
            {
                flagClosedTable = false;
            }
            ocDateClose.Parameters["p_subdiv_id"].Value = Subdiv_ID;
            dCloseTable = Convert.ToDateTime(ocDateClose.ExecuteScalar());
        }

        void RefreshDependency()
        {
            // Команда проверки обновления статуса проекта
            _ocDependencyTable_Closing = new OracleCommand(string.Format(
                @"select TABLE_CLOSING_ID, TABLE_PLAN_APPROVAL_ID from {0}.TABLE_CLOSING
                WHERE SUBDIV_ID = :p_SUBDIV_ID and TABLE_DATE = :p_TABLE_DATE and TYPE_TABLE_ID = 2",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyTable_Closing.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = Subdiv_ID;
            _ocDependencyTable_Closing.Parameters.Add("p_TABLE_DATE", OracleDbType.Date).Value = BeginDate;
            _ocDependencyTable_Closing.CommandType = CommandType.Text;
            _odTable_Closing = new OracleDependency(_ocDependencyTable_Closing);
            _odTable_Closing.QueryBasedNotification = true;
            _odTable_Closing.OnChange += new OnChangeEventHandler(Table_Closing_OnChange);
            _ocDependencyTable_Closing.Notification.IsNotifiedOnce = false;
            _ocDependencyTable_Closing.Notification.Timeout = 600;
            _ocDependencyTable_Closing.AddRowid = true;
            _ocDependencyTable_Closing.ExecuteNonQuery();
        }

        void Table_Closing_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Info)
                {
                    case OracleNotificationInfo.Update:
                        // 26.07.2016 убрал условие cbType_Approval_Advance.IsHandleCreated и поставил try - catch
                        //if (cbType_Approval_Advance != null && cbType_Approval_Advance.IsLoaded
                        //    //&& cbType_Approval_Advance.IsHandleCreated //&& cbType_Approval_Advance.Handle != null
                        //    )
                        {
                            try
                            {
                                //cbType_Approval_Table.BeginInvoke(new Action(GetTable_Approval));

                                GetTable_Approval();
                            }
                            catch { }
                        }
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
               // MessageBox.Show(ex.Message, "Table_Closing_OnChange");
            }
        }

        void Load_Table()
        {
            dtEmp = new OracleDataTable("", Connect.CurConnect);
            dtEmp.SelectCommand.Parameters.Add("beginDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("endDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);

            dgEmp.DataContext = dtEmp.DefaultView;

            ocClosedTable = new OracleCommand("", Connect.CurConnect);
            ocClosedTable.BindByName = true;
            ocClosedTable.CommandText = string.Format(Queries.GetQuery("Table/SelectClosedTable.sql"), Connect.Schema);
            ocClosedTable.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ocClosedTable.Parameters.Add("p_date_end", OracleDbType.Date);

            ocDateClose = new OracleCommand("", Connect.CurConnect);
            ocDateClose.BindByName = true;
            ocDateClose.CommandText = string.Format(
                Queries.GetQuery("Table/DateCloseTable.sql"),
                Connect.Schema);
            ocDateClose.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);

            // Данные по согласованию закрытия аванса и табеля
            _dsTable_Approval = new DataSet();
            _dsTable_Approval.Tables.Add("ADVANCE_APPROVAL");
            _dsTable_Approval.Tables.Add("TABLE_APPROVAL");
            _dsTable_Approval.Tables.Add("TABLE_PLAN_APPROVAL");
            _dsTable_Approval.Tables.Add("TYPE_APPROVAL_ADVANCE");
            _dsTable_Approval.Tables.Add("TYPE_APPROVAL_TABLE");
            _dsTable_Approval.Tables.Add("PLAN_APPROVAL_ADVANCE");
            _dsTable_Approval.Tables.Add("PLAN_APPROVAL_TABLE");
            _dsTable_Approval.Tables.Add("TABLE_APPENDIX");

            _daTable_Approval = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/SelectTable_Approval.sql"),
                Connect.Schema), Connect.CurConnect);
            _daTable_Approval.SelectCommand.BindByName = true;
            _daTable_Approval.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal).Value = 0;
            _daTable_Approval.Fill(_dsTable_Approval.Tables["ADVANCE_APPROVAL"]);
            _daTable_Approval.Fill(_dsTable_Approval.Tables["TABLE_APPROVAL"]);
            dgAdvance_Approval.AutoGenerateColumns = false;
            dgTable_Approval.AutoGenerateColumns = false;
            dgAdvance_Approval.DataContext = _dsTable_Approval.Tables["ADVANCE_APPROVAL"];
            dgTable_Approval.DataContext = _dsTable_Approval.Tables["TABLE_APPROVAL"];
            // Insert
            _daTable_Approval.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TABLE_APPROVAL_UPDATE(:TABLE_APPROVAL_ID, :TABLE_CLOSING_ID, :TABLE_PLAN_APPROVAL_ID, :DATE_APPROVAL, 
                        :NOTE_APPROVAL, :USER_NAME, :USER_FIO, :TYPE_APPROVAL_TABLE_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daTable_Approval.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daTable_Approval.InsertCommand.BindByName = true;
            _daTable_Approval.InsertCommand.Parameters.Add("TABLE_APPROVAL_ID", OracleDbType.Decimal, 0, "TABLE_APPROVAL_ID").Direction =
                ParameterDirection.InputOutput;
            _daTable_Approval.InsertCommand.Parameters["TABLE_APPROVAL_ID"].DbType = DbType.Decimal;
            _daTable_Approval.InsertCommand.Parameters.Add("TABLE_CLOSING_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_ID");
            _daTable_Approval.InsertCommand.Parameters.Add("TABLE_PLAN_APPROVAL_ID", OracleDbType.Decimal, 0, "TABLE_PLAN_APPROVAL_ID");
            _daTable_Approval.InsertCommand.Parameters.Add("DATE_APPROVAL", OracleDbType.Date, 0, "DATE_APPROVAL").Direction =
                ParameterDirection.InputOutput;
            _daTable_Approval.InsertCommand.Parameters["DATE_APPROVAL"].DbType = DbType.DateTime;
            _daTable_Approval.InsertCommand.Parameters.Add("NOTE_APPROVAL", OracleDbType.Varchar2, 500, "NOTE_APPROVAL");
            _daTable_Approval.InsertCommand.Parameters.Add("USER_NAME", OracleDbType.Varchar2, 30, "USER_NAME").Direction =
                ParameterDirection.InputOutput;
            _daTable_Approval.InsertCommand.Parameters["USER_NAME"].DbType = DbType.String;
            _daTable_Approval.InsertCommand.Parameters.Add("USER_FIO", OracleDbType.Varchar2, 100, "USER_FIO").Direction =
                ParameterDirection.InputOutput;
            _daTable_Approval.InsertCommand.Parameters["USER_FIO"].DbType = DbType.String;
            _daTable_Approval.InsertCommand.Parameters.Add("TYPE_APPROVAL_TABLE_ID", OracleDbType.Decimal, 0, "TYPE_APPROVAL_TABLE_ID");

            // Команда выбора строки закрытия табеля
            _ocTable_Closing_ID = new OracleCommand(string.Format(
                @"SELECT (SELECT MAX(TABLE_CLOSING_ID) KEEP(DENSE_RANK LAST ORDER BY TIME_CLOSING) FROM {0}.TABLE_CLOSING TC
                        WHERE TC.SUBDIV_ID = :p_SUBDIV_ID and TC.TABLE_DATE = :p_TABLE_DATE and TC.TYPE_TABLE_ID = :p_TYPE_TABLE_ID)
                FROM DUAL", Connect.Schema), Connect.CurConnect);
            _ocTable_Closing_ID.BindByName = true;
            _ocTable_Closing_ID.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            _ocTable_Closing_ID.Parameters.Add("p_TABLE_DATE", OracleDbType.Date);
            _ocTable_Closing_ID.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal);

            // Select
            _daType_Approval = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/SelectType_Approval_By_Table.sql"),
                Connect.Schema), Connect.CurConnect);
            _daType_Approval.SelectCommand.BindByName = true;
            _daType_Approval.SelectCommand.Parameters.Add("p_TABLE_PLAN_APPROVAL_ID", OracleDbType.Decimal);

            new OracleDataAdapter(string.Format(
                @"select TABLE_PLAN_APPROVAL_ID, ROLE_NAME, NOTE_ROLE_NAME, TABLE_PLAN_APPROVAL_ID_PRIOR, TYPE_TABLE_ID, NOTE_ROLE_APPROVAL
                from {0}.TABLE_PLAN_APPROVAL order by TYPE_TABLE_ID, TABLE_PLAN_APPROVAL_ID", Connect.Schema),
                Connect.CurConnect).Fill(_dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"]);
            dcTABLE_PLAN_APPROVAL_ID.ItemsSource = _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView;
            dcTABLE_PLAN_APPROVAL_ID2.ItemsSource = _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView;

            _ocGet_Status_Closing = new OracleCommand(string.Format(
                @"SELECT NVL((select NVL(TABLE_PLAN_APPROVAL_ID,0) from {0}.TABLE_CLOSING 
                                WHERE TABLE_CLOSING_ID = :p_TABLE_CLOSING_ID),0) from dual",
                Connect.Schema), Connect.CurConnect);
            _ocGet_Status_Closing.BindByName = true;
            _ocGet_Status_Closing.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal);

            _daPlan_Approval.SelectCommand = new OracleCommand(string.Format(
                @"select TABLE_PLAN_APPROVAL_ID,TABLE_PLAN_APPROVAL_ID_PRIOR from {0}.TABLE_PLAN_APPROVAL
                where TABLE_PLAN_APPROVAL_ID=:p_TABLE_PLAN_APPROVAL_ID",
                Connect.Schema), Connect.CurConnect);
            _daPlan_Approval.SelectCommand.BindByName = true;
            _daPlan_Approval.SelectCommand.Parameters.Add("p_TABLE_PLAN_APPROVAL_ID", OracleDbType.Decimal).Value = 0;

            _daPlan_Approval.Fill(_dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"]);
            _daPlan_Approval.Fill(_dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"]);

            //// Приложения к закрытию табеля
            //_daAppendix.SelectCommand = new OracleCommand(string.Format(
            //    @"SELECT TABLE_CLOSING_APPENDIX_ID, NOTE_DOCUMENT, TABLE_CLOSING_ID 
            //    FROM {0}.TABLE_CLOSING_APPENDIX WHERE TABLE_CLOSING_ID = :p_TABLE_CLOSING_ID",
            //    Connect.Schema), Connect.CurConnect);
            //_daAppendix.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal);
            //// Insert
            //_daAppendix.InsertCommand = new OracleCommand(string.Format(
            //    @"BEGIN
            //        {0}.TABLE_CLOSING_APPENDIX_UPDATE(:TABLE_CLOSING_APPENDIX_ID,:NOTE_DOCUMENT,:DOCUMENT,:TABLE_CLOSING_ID);
            //    END;", Connect.Schema), Connect.CurConnect);
            //_daAppendix.InsertCommand.BindByName = true;
            //_daAppendix.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            //_daAppendix.InsertCommand.Parameters.Add("TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_APPENDIX_ID").Direction =
            //    ParameterDirection.InputOutput;
            //_daAppendix.InsertCommand.Parameters["TABLE_CLOSING_APPENDIX_ID"].DbType = DbType.Decimal;
            //_daAppendix.InsertCommand.Parameters.Add("NOTE_DOCUMENT", OracleDbType.Varchar2, 0, "NOTE_DOCUMENT");
            //_daAppendix.InsertCommand.Parameters.Add("DOCUMENT", OracleDbType.Blob, 0, "DOCUMENT");
            //_daAppendix.InsertCommand.Parameters.Add("TABLE_CLOSING_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_ID");
            //// Update
            //_daAppendix.UpdateCommand = _daAppendix.InsertCommand;
            //// Delete
            //_daAppendix.DeleteCommand = new OracleCommand(string.Format(
            //    @"BEGIN
            //        {0}.TABLE_CLOSING_APPENDIX_DELETE(:TABLE_CLOSING_APPENDIX_ID);
            //    END;", Connect.Schema), Connect.CurConnect);
            //_daAppendix.DeleteCommand.BindByName = true;
            //_daAppendix.DeleteCommand.Parameters.Add("TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_APPENDIX_ID");

            //dgAppendix.AutoGenerateColumns = false;
            //dgAppendix.DataSource = _dsTable_Approval.Tables["TABLE_APPENDIX"].DefaultView;
            //if (!_dsTable_Approval.Tables["TABLE_APPENDIX"].Columns.Contains("DOCUMENT"))
            //{
            //    _dsTable_Approval.Tables["TABLE_APPENDIX"].Columns.Add("DOCUMENT", Type.GetType("System.Byte[]"));
            //}

            _ocSign_Table_Closing = new OracleCommand(string.Format(
                @"SELECT 
                        CASE WHEN TABLE_PLAN_APPROVAL_ID =
                                (SELECT MAX(TPA.TABLE_PLAN_APPROVAL_ID) FROM {0}.TABLE_PLAN_APPROVAL TPA 
                                WHERE TPA.TYPE_TABLE_ID = TC.TYPE_TABLE_ID)
                            THEN 1 ELSE 0 END FL_TABLE_CLOSING,
                        (SELECT TPA.NOTE_ROLE_APPROVAL FROM {0}.TABLE_PLAN_APPROVAL TPA 
                        WHERE TPA.TABLE_PLAN_APPROVAL_ID = TC.TABLE_PLAN_APPROVAL_ID ) NOTE_ROLE_APPROVAL
                    FROM {0}.TABLE_CLOSING TC
                    WHERE TC.TABLE_CLOSING_ID = :p_TABLE_CLOSING_ID", Connect.Schema), Connect.CurConnect);
            _ocSign_Table_Closing.BindByName = true;
            _ocSign_Table_Closing.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal);

            TDG = new TabDayGrid(pnTable);
            /// Делаем невидимой, чтобы не нажимали кнопки
            pnReg_Doc.Visibility = Visibility.Hidden;

            dtAbsence = new OracleDataTable("", Connect.CurConnect);
            dgAbsence.DataContext = dtAbsence.DefaultView;
            dtAbsence.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAbsence.sql"),
                Connect.Schema);
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_per_num", OracleDbType.Varchar2));
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_year_begin", OracleDbType.Decimal)).Value = SelectedDate.Year;
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_year_end", OracleDbType.Decimal)).Value = SelectedDate.Year;
            dtAbsence.Fill();
            
            dgEmp.Focus();
            /// Создание таблицы рабочего дня
            dtWorked_day = new OracleDataTable("", Connect.CurConnect);
            dtWorked_day.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectWorked_Day.sql"), Connect.Schema);
            dtWorked_day.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtWorked_day.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtWorked_day.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            //dtWorked_day.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            dtWorked_day.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal);
            dtWorked_day.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = Subdiv_ID;

            /// Создание таблицы часов по видам оплат
            dtWork_pay_type = new OracleDataTable("", Connect.CurConnect);
            dtWork_pay_type.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectWork_Pay_Type.sql"), Connect.Schema);
            dtWork_pay_type.SelectCommand.Parameters.Add("worked_day_id", OracleDbType.Decimal);

            /// Создаем таблицу для календаря
            dtDays_calendar = new OracleDataTable("", Connect.CurConnect);
            dtDays_calendar.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/Days_calendar.sql"), Connect.Schema);
            dtDays_calendar.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtDays_calendar.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);

            dtReg_doc = new OracleDataTable(string.Format(
                Queries.GetQuery("Table/Reg_doc.sql"), Connect.Schema), Connect.CurConnect);
            dtReg_doc.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = "0";
            dtReg_doc.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = 0;
            dtReg_doc.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            dtReg_doc.Fill();
            dgReg_Doc.DataContext = dtReg_doc;

            dtHoursCalc = new OracleDataTable("", Connect.CurConnect);
            dtHoursCalc.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Salary_Calc.sql"),
                Connect.Schema);
            dtHoursCalc.SelectCommand.BindByName = true;
            dtHoursCalc.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = -1;
            dtHoursCalc.SelectCommand.Parameters.Add("p_BEGINDATE", OracleDbType.Date).Value = DateTime.Today;
            dtHoursCalc.SelectCommand.Parameters.Add("p_ENDDATE", OracleDbType.Date).Value = DateTime.Today;
            dtHoursCalc.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = -1;
            dtHoursCalc.SelectCommand.Parameters.Add("p_sign_calc", OracleDbType.Decimal).Value = -1;
            dtHoursCalc.Fill();
            // 1 Параметр определяет видны ли все рассчитанные данные. Если 0, то видим всё!
            // 2 Параметр определяет видны ли только данные численности (если стоит 1)
            dtHoursCalc.DefaultView.RowFilter = "SIGN_VISIBLE in (1, 1) and SIGN_APPENDIX in (1, 0)";
            dgHoursCalc.DataContext = dtHoursCalc.DefaultView;

            dtHoursSaved = new OracleDataTable("", Connect.CurConnect);
            dtHoursSaved.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Salary_Kept.sql"),
                Connect.Schema, Connect.SchemaSalary);
            dtHoursSaved.SelectCommand.BindByName = true;
            dtHoursSaved.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = DateTime.Today.Date;
            dtHoursSaved.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = DateTime.Today.Date;
            dtHoursSaved.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = "";
            dtHoursSaved.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = 0;
            dtHoursSaved.Fill();
            // 1 Параметр определяет видны ли все рассчитанные данные. Если 0, то видим всё!
            // 2 Параметр определяет видны ли только данные численности (если стоит 1)                        
            dtHoursSaved.DefaultView.RowFilter = "SIGN_VISIBLE in (1, 1) and SIGN_APPENDIX in (1, 0)";
            dgHoursSaved.DataContext = dtHoursSaved.DefaultView;
            
            dtHoursPeriod = new OracleDataTable("", Connect.CurConnect);
            dtHoursPeriod.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Salary_Calc.sql"),
                Connect.Schema);
            //dtHoursPeriod.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = -1;
            //dtHoursPeriod.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal).Value = -1;
            dtHoursPeriod.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = -1;
            dtHoursPeriod.SelectCommand.Parameters.Add("p_BEGINDATE", OracleDbType.Date).Value = DateTime.Today;
            dtHoursPeriod.SelectCommand.Parameters.Add("p_ENDDATE", OracleDbType.Date).Value = DateTime.Today;
            dtHoursPeriod.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = -1;
            dtHoursPeriod.SelectCommand.Parameters.Add("p_sign_calc", OracleDbType.Decimal).Value = -1;
            dtHoursPeriod.Fill();
            dtHoursPeriod.DefaultView.RowFilter = "SIGN_VISIBLE in (1, 1) and SIGN_APPENDIX in (1, 0)";

            //dtWorkOut = new OracleDataTable("", Connect.CurConnect);
            //dtWorkOut.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOutPer_Num.sql"),
            //    Connect.Schema);
            //dtWorkOut.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = "0";
            //dtWorkOut.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            //dtWorkOut.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = BeginDate;
            //dtWorkOut.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = EndDate;
            //dtWorkOut.SelectCommand.Parameters.Add("p_pay_type", OracleDbType.Decimal).Value = 535;
            //dtWorkOut.Fill();
            //dgWorkOut.DataSource = dtWorkOut;
            //foreach (DataGridViewColumn col in dgWorkOut.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}

            dtEmpErrors = new OracleDataTable("", Connect.CurConnect);
            dtEmpErrors.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/DaysWithError.sql"), Connect.Schema);
            dtEmpErrors.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtEmpErrors.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            dtEmpErrors.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtEmpErrors.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            
            VisibleDay();
            // Отключаем обновление дней табеля пока не произойдет событие Load формы
            flagVisible = false;
            // Включаем обновление дней табеля и принудительно обновляем дни
            flagVisible = true;
            //dgEmp_SelectionChanged(sender, e);

            chSign_Vis.Checked += ChSign_Vis_Checked;
            chSign_Vis.Unchecked += ChSign_Vis_Checked;
            chVisibleOnlyAppendix.Checked += ChVisibleOnlyAppendix_Checked;
            chVisibleOnlyAppendix.Unchecked += ChVisibleOnlyAppendix_Checked;
            chFilterToYear.Checked += ChFilterToYear_Checked;
            chFilterToYear.Unchecked += ChFilterToYear_Checked;
            nudYearFilterAbsence.ValueChanged += nudYearFilterAbsence_ValueChanged;
            nudYearFilterAbsence.Value = DateTime.Today.Year;

            ocCalc_Absence = new OracleCommand("", Connect.CurConnect);
            ocCalc_Absence.BindByName = true;
            ocCalc_Absence.CommandText = string.Format("begin {0}.CALC_ABSENCE(:p_transfer_id,:p_hours); end; ",
                Connect.Schema);
            ocCalc_Absence.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            ocCalc_Absence.Parameters.Add("p_hours", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            ocDeleteReg_Doc = new OracleCommand(string.Format(
                "BEGIN {0}.REG_DOC_delete(:p_reg_doc_id); END;",
                Connect.Schema), Connect.CurConnect);
            ocDeleteReg_Doc.BindByName = true;
            ocDeleteReg_Doc.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal);

            _daPN_TMP = new OracleDataAdapter("SELECT PNUM,USER_NAME,TRANSFER_ID FROM APSTAFF.PN_TMP WHERE 1 = 2",
                Connect.CurConnect);
            _daPN_TMP.InsertCommand = new OracleCommand(string.Format(
                "insert into {0}.PN_TMP(PNUM,USER_NAME,TRANSFER_ID) values (:PNUM, :USER_NAME, :TRANSFER_ID)", Connect.Schema), Connect.CurConnect);
            _daPN_TMP.InsertCommand.BindByName = true;
            _daPN_TMP.InsertCommand.Parameters.Add("PNUM", OracleDbType.Varchar2, 0, "PNUM");
            _daPN_TMP.InsertCommand.Parameters.Add("USER_NAME", OracleDbType.Varchar2, 0, "USER_NAME");
            _daPN_TMP.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");

            //pnAppendix_Button.EnableByRules();

            if (ssTable.SubdivView.Count == 1)
                ssTable.SubdivId = (decimal)ssTable.SubdivView[0]["SUBDIV_ID"];
        }

        public void RefreshTable(DateTime selectedDate, decimal? subdiv_ID)
        {
            Subdiv_ID = subdiv_ID == null ? 0 : (int)subdiv_ID;
            code_subdiv = ssTable.CodeSubdiv;
            SelectedDate = selectedDate;
            BeginDate = SelectedDate;
            EndDate = BeginDate.AddMonths(1).AddSeconds(-1);
            //RefreshFlagClosedTable();
            dgEmp.SelectionChanged -= DgEmp_SelectionChanged;
            if (flagVisible)
                VisibleDay();
            LoadList();
        }
        private void Table_Viewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate" || e.PropertyName == "Subdiv_ID")
            {
                if (Subdiv_ID != null && Subdiv_ID != 0)
                    RefreshTable(SelectedDate, Subdiv_ID);
                if (OwnerTabBase != null)
                    OwnerTabBase.HeaderText = "Табель " +
                        ssTable.SubdivView.Table.Select("SUBDIV_ID=" + Subdiv_ID.ToString())[0]["CODE_SUBDIV"].ToString() +
                        (Convert.ToBoolean(ssTable.SubdivView.Table.Select("SUBDIV_ID=" + Subdiv_ID.ToString())[0]["SUB_ACTUAL_SIGN"]) ? "" : "(-)");
            }
        }
        
        /// <summary>
        /// Владелец контрола
        /// </summary>
        public ViewTabBase OwnerTabBase
        {
            get;
            set;
        }
        private void ssTable_SubdivChanged(object sender, RoutedEventArgs e)
        {
            Subdiv_ID = ssTable.SubdivId;
        }

        /// <summary>
        /// Заполнение таблицы списка сотрудников
        /// </summary>
        public void LoadList()
        {
            flagReload = false;
            if (dgEmp.SelectedItem != null)
            {
                /// Сохранение позиции               
                curWorker_ID = ((DataRowView)dgEmp.SelectedItem)["WORKER_ID"].ToString();
            }
            dgEmp.DataContext = null;
            dtEmp.Clear();
            /* Дополнительный параметр используется для сортировки списка работников 
             цеха 61, им нужна сортировка по виду производства*/
            dtEmp.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectEmpTable.sql"),
                Connect.Schema,
                code_subdiv == "061" ? ", case when CODE_DEGREE = '09' then CODE_FORM_OPERATION else substr(CODE_POS,1,1) end" : "");
            dtEmp.SelectCommand.Parameters["beginDate"].Value = BeginDate;
            dtEmp.SelectCommand.Parameters["endDate"].Value = EndDate;
            dtEmp.SelectCommand.Parameters["p_subdiv_id"].Value = Subdiv_ID;        
            dtEmp.Fill();
            dgEmp.DataContext = dtEmp.DefaultView;
            dgEmp.SelectionChanged += DgEmp_SelectionChanged;
            flagReload = true;
            lbEmpCount.Text = dtEmp.Rows.Count.ToString();
            if (curWorker_ID != "")
            {
                DataTable _dtTemp = dtEmp.DefaultView.ToTable();
                _dtTemp.PrimaryKey = new DataColumn[] { _dtTemp.Columns["WORKER_ID"] };
                if (_dtTemp.Rows.Find(curWorker_ID) != null)
                {
                    DataRowView _row = dtEmp.DefaultView[_dtTemp.Rows.IndexOf(_dtTemp.Rows.Find(curWorker_ID))];
                    if (_row != null)
                    {
                        dgEmp.SelectedItem = _row;
                        dgEmp.ScrollIntoView(_row);
                        dgEmp.Focus();
                    }
                }
                else
                {
                    if (dtEmp.DefaultView.Count > 0)
                    {
                        dgEmp.SelectedItem = dtEmp.DefaultView[0];
                        dgEmp.ScrollIntoView(dtEmp.DefaultView[0]);
                        dgEmp.Focus();
                    }
                }
            }
            else
            {
                if (dtEmp.DefaultView.Count > 0)
                {
                    dgEmp.SelectedItem = dtEmp.DefaultView[0];
                    dgEmp.ScrollIntoView(dtEmp.DefaultView[0]);
                    dgEmp.Focus();
                }
            }
            GetTable_Approval();
            dgEmp.Focus();
        }
        
        /// <summary>
        /// Создание объекта отображения кнопок на каждый день месяца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VisibleDay()
        {
            /// Отображаем панель оправдательных документов
            pnReg_Doc.Visibility = Visibility.Visible;
            tcTable.Visibility = Visibility.Visible;
            /// Если объект создан
            if (TDG != null)
            {
                /// Удаляем его из памяти
                TDG.Dispose();
            }
            // Создаем объект
            TDG = new TabDayGrid(pnTable);
            // Настраиваем его вид
            TDG.Height = 400;
            TDG.Font = new System.Drawing.Font(pnTable.Font.Name, 10);
            TDG.Dock = System.Windows.Forms.DockStyle.Fill;
            TDG.OnItemBtnClick += new TabDayGrid.ItemBtnClick(TDG_OnItemBtnClick);
            TDG.OnItemClick += new TabDayGrid.ItemClick(TDG_Enter);
            TDG.FillDays(BeginDate, EndDate);
            dtDays_calendar.Clear();
            dtDays_calendar.SelectCommand.Parameters["p_date_begin"].Value = BeginDate;
            dtDays_calendar.SelectCommand.Parameters["p_date_end"].Value = EndDate;
            dtDays_calendar.Fill();
            for (int i = 0; i < dtDays_calendar.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtDays_calendar.Rows[i][1]) == 3)
                {
                    TDG[Convert.ToInt32(dtDays_calendar.Rows[i][0]) - 1].DayWeekColor = System.Drawing.Color.Green;
                }
                else
                {
                    TDG[Convert.ToInt32(dtDays_calendar.Rows[i][0]) - 1].DayWeekColor = System.Drawing.Color.DarkRed;
                }
            }
            TDG.GridCaption = "Табель за период с " + BeginDate.ToShortDateString() +
                " по " + EndDate.ToShortDateString();
            flagVisible = true;
        }

        private void DgEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /// Если показываем табель на месяц и выбранная строка есть
            if (flagVisible && dgEmp.SelectedItem != null && flagReload)
            {
                for (int p = 0; p < EndDate.Day; p++)
                {
                    TDG[p].NoteText = "";
                    TDG[p].Text = "";
                    TDG[p].BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                }
                _curRowEmp = ((DataRowView)dgEmp.SelectedItem);
                transfer_id = Convert.ToDecimal(_curRowEmp["transfer_id"]);
                dtWorked_day.Clear();
                dtWorked_day.SelectCommand.Parameters["p_per_num"].Value =
                    _curRowEmp["per_num"].ToString();
                dtWorked_day.SelectCommand.Parameters["p_date_begin"].Value =
                    _curRowEmp["date_hire"] == DBNull.Value ? BeginDate :
                    _curRowEmp["date_hire"];
                dtWorked_day.SelectCommand.Parameters["p_date_end"].Value =
                    _curRowEmp["date_transfer"] == DBNull.Value ?
                    _curRowEmp["date_dismiss"] == DBNull.Value ? EndDate :
                    _curRowEmp["date_dismiss"] :
                    Convert.ToDateTime(_curRowEmp["date_transfer"]).AddDays(-1);
                //dtWorked_day.SelectCommand.Parameters["p_transfer_id"].Value = transfer_id;
                dtWorked_day.SelectCommand.Parameters["p_WORKER_ID"].Value = _curRowEmp["WORKER_ID"];
                dtWorked_day.SelectCommand.Parameters["p_SUBDIV_ID"].Value = Subdiv_ID;
                /// Заполняем рабочие дни для выбранного работника
                dtWorked_day.Fill();
                TabDayItem lastItem = TDG[0];
                /// Новый вариант
                foreach (DataRow row in dtWorked_day.Rows)
                {
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].NoteText = row["FROM_PERCO"].ToString();
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Text = row["NOTE"].ToString();
                    if (Convert.ToInt32(row["ISPINK"]) == 1)
                    {
                        TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].BackColor = System.Drawing.Color.Pink;
                    }
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Width =
                        Math.Max(TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Text.Length * 10, 56);
                    lastItem = TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1];
                }
                TDG_Enter(lastItem);
                Absence(_curRowEmp["per_num"].ToString(), Convert.ToDecimal(_curRowEmp["transfer_id"]));

                // Если выбрана вкладка Итоги, считаем итоги
                if (tcTable.SelectedItem != null && ((TabItem)tcTable.SelectedItem).Name == tpHours.Name)
                {
                    Calc_Hours(dtHoursCalc, Convert.ToInt32(_curRowEmp["transfer_id"]),
                        Convert.ToInt16(_curRowEmp["sign_comb"].ToString()),
                        BeginDate, EndDate);
                    Fill_Salary_Kept(Convert.ToInt32(_curRowEmp["transfer_id"]),
                        _curRowEmp["per_num"].ToString(),
                        BeginDate, EndDate);
                }

                lbHire.Text = lbTrans.Text = lbDism.Text = "";
                if (_curRowEmp["date_hire"] != DBNull.Value)
                    lbHire.Text = "Принят: " +
                        Convert.ToDateTime(_curRowEmp["date_hire"]).ToShortDateString();
                if (_curRowEmp["date_transfer"] != DBNull.Value)
                    lbTrans.Text = "Переведен: " +
                        Convert.ToDateTime(_curRowEmp["date_transfer"]).ToShortDateString();
                if (_curRowEmp["date_dismiss"] != DBNull.Value)
                    lbDism.Text = "Уволен: " +
                        Convert.ToDateTime(_curRowEmp["date_dismiss"]).ToShortDateString();
            }
        }
        
        /// <summary>
        /// Метод рассчитывает количество часов отгулов для сотрудника
        /// </summary>
        /// <param name="_per_num">Табельный номер</param>
        void Absence(string _per_num, decimal _trans_id)
        {
            dgAbsence.DataContext = null;
            dtAbsence.Clear();
            dtAbsence.SelectCommand.Parameters["p_per_num"].Value = _per_num;
            dtAbsence.Fill();
            dgAbsence.DataContext = dtAbsence.DefaultView;
            ocCalc_Absence.Parameters["p_transfer_id"].Value = _trans_id;
            ocCalc_Absence.ExecuteNonQuery();
            hoursAbsence = ((OracleDecimal)ocCalc_Absence.Parameters["p_hours"].Value).Value;
            decimal hours = Math.Truncate(Math.Abs(hoursAbsence));
            decimal min = hoursAbsence - Math.Truncate(hoursAbsence);
            tpAbsence.Header = string.Format("Отгулы. Доступно {0}{1}:{2}", hoursAbsence < 0 ? "-" : "", hours,
                Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
        }
        
        public static decimal HoursAbsence
        {
            get { return hoursAbsence; }
        }

        private void tcTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tpHours == tcTable.SelectedItem)
            {
                Calc_Hours(dtHoursCalc, Convert.ToInt32(_curRowEmp["transfer_id"]),
                    Convert.ToInt16(_curRowEmp["sign_comb"].ToString()),
                    BeginDate, EndDate);
                Fill_Salary_Kept(Convert.ToInt32(_curRowEmp["transfer_id"]),
                    _curRowEmp["per_num"].ToString(),
                    BeginDate, EndDate);
            }
        }

        private void btProtError_Click(object sender, RoutedEventArgs e)
        {
            FillEmpError(_curRowEmp["PER_NUM"].ToString(),
                Convert.ToInt32(_curRowEmp["TRANSFER_ID"]),
                Convert.ToDateTime(_curRowEmp["date_hire"] == DBNull.Value ? BeginDate :
                    _curRowEmp["date_hire"]),
                Convert.ToDateTime(_curRowEmp["date_dismiss"] == DBNull.Value ?
                    (_curRowEmp["date_TRANSFER"] == DBNull.Value ? EndDate :
                    _curRowEmp["date_TRANSFER"]) :
                    _curRowEmp["date_dismiss"]));
            if (dtEmpErrors.Rows.Count > 0)
            {
                string strError = "По сотруднику обнаружены следующие ошибочные дни: \n";
                foreach (DataRow row in dtEmpErrors.Rows)
                {
                    strError += "\n" + row[0].ToString();
                }
                MessageBox.Show(strError, "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Ошибок в данных нет!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Расчет итоговых данных по видам оплат для табельного
        /// </summary>
        /// <param name="per_num"></param>
        void Calc_Hours(OracleDataTable _dtHours, decimal _transfer_id, int _sign_comb, DateTime _beginDate, DateTime _endDate)
        {
            //dgHoursCalc.DataContext = null;
            _dtHours.Clear();     
            _dtHours.SelectCommand.Parameters["p_SUBDIV_ID"].Value = Subdiv_ID;
            _dtHours.SelectCommand.Parameters["p_BEGINDATE"].Value = _beginDate;
            _dtHours.SelectCommand.Parameters["p_ENDDATE"].Value = _endDate;
            _dtHours.SelectCommand.Parameters["p_TRANSFER_ID"].Value = _transfer_id;
            _dtHours.SelectCommand.Parameters["p_sign_calc"].Value = 1;
            try
            {
                //tcHours.SelectedItem = tpHoursCalc;
                _dtHours.Fill();
                //dgHoursCalc.DataContext = _dtHours.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка расчета итогов:\n" + ex.Message,
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        void Fill_Salary_Kept(int transfer_id, string per_num, DateTime beginDate, DateTime endDate)
        {
            dgHoursSaved.DataContext = null;
            dtHoursSaved.Clear();
            //tcHours.Items.Clear();
            //tcHours.Items.Add(tpHoursCalc);
            dtHoursSaved.SelectCommand.Parameters["p_per_num"].Value = per_num;
            dtHoursSaved.SelectCommand.Parameters["p_date_begin"].Value = beginDate;
            dtHoursSaved.SelectCommand.Parameters["p_date_end"].Value = endDate;
            dtHoursSaved.SelectCommand.Parameters["p_transfer_id"].Value = transfer_id;
            dtHoursSaved.Fill();
            dgHoursSaved.DataContext = dtHoursSaved.DefaultView;
            //if (dtHoursSaved.Rows.Count > 0)
            //{
            //    if (tcHours.Items.Contains(tpHoursSaved))
            //        tcHours.Items.Remove(tpHoursSaved);                
            //    tcHours.Items.Add(tpHoursSaved);            
            //}
            //else
            //    tcHours.Items.Remove(tpHoursSaved);

        }

        /// <summary>
        /// Работа за территорией предприятия
        /// </summary>
        /// <param name="_per_num"></param>
        void WorkOut(string _per_num)
        {
            dtWorkOut.Clear();
            dtWorkOut.SelectCommand.Parameters["p_per_num"].Value = _per_num;
            dtWorkOut.SelectCommand.Parameters["p_date_begin"].Value = BeginDate;
            dtWorkOut.SelectCommand.Parameters["p_date_end"].Value = EndDate;
            dtWorkOut.SelectCommand.Parameters["p_pay_type"].Value = 535;
            dtWorkOut.Fill();
        }

        public void GetTable_Approval()
        {
            dgAdvance_Approval.DataContext = null;
            dgTable_Approval.DataContext = null;
            //dgAppendix.DataContext = null;
            _dsTable_Approval.Tables["ADVANCE_APPROVAL"].Clear();
            _dsTable_Approval.Tables["TABLE_APPROVAL"].Clear();
            _dsTable_Approval.Tables["TABLE_APPENDIX"].Clear();

            _ocTable_Closing_ID.Parameters["p_SUBDIV_ID"].Value = Subdiv_ID;
            _ocTable_Closing_ID.Parameters["p_TABLE_DATE"].Value = BeginDate;
            _ocTable_Closing_ID.Parameters["p_TYPE_TABLE_ID"].Value = 1;
            // Получаем ключ строки закрытия аванса
            Advance_Closing_ID = _ocTable_Closing_ID.ExecuteScalar() as Decimal?;
            _daTable_Approval.SelectCommand.Parameters["p_TABLE_CLOSING_ID"].Value = Advance_Closing_ID;
            _daTable_Approval.Fill(_dsTable_Approval.Tables["ADVANCE_APPROVAL"]);
            _ocTable_Closing_ID.Parameters["p_TYPE_TABLE_ID"].Value = 2;
            // Получаем ключ строки закрытия табеля
            Table_Closing_ID = _ocTable_Closing_ID.ExecuteScalar() as Decimal?;
            _daTable_Approval.SelectCommand.Parameters["p_TABLE_CLOSING_ID"].Value = Table_Closing_ID;
            _daTable_Approval.Fill(_dsTable_Approval.Tables["TABLE_APPROVAL"]);

            //_daAppendix.SelectCommand.Parameters["p_TABLE_CLOSING_ID"].Value = Table_Closing_ID;
            //_daAppendix.Fill(_dsTable_Approval.Tables["TABLE_APPENDIX"]);

            dgAdvance_Approval.DataContext = _dsTable_Approval.Tables["ADVANCE_APPROVAL"];
            dgTable_Approval.DataContext = _dsTable_Approval.Tables["TABLE_APPROVAL"];
                //dgAppendix.DataSource = _dsTable_Approval.Tables["TABLE_APPENDIX"];
            GetStatusApproval();
        }

        void GetStatusApproval()
        {
            cbType_Approval_Advance.IsEnabled = false;
            btSave_Approval_Advance.IsEnabled = false;
            if (Advance_Closing_ID != null)
            {
                // Отрабатываем согласование аванса
                _ocGet_Status_Closing.Parameters["p_TABLE_CLOSING_ID"].Value = Advance_Closing_ID;
                _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID"] =
                    _ocGet_Status_Closing.ExecuteScalar();
                _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"] =
                    _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView.Table.Select().Where(r =>
                        r["TABLE_PLAN_APPROVAL_ID"].ToString() == _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].
                            DefaultView[0]["TABLE_PLAN_APPROVAL_ID"].ToString()).FirstOrDefault()["TABLE_PLAN_APPROVAL_ID_PRIOR"];

                _dsTable_Approval.Tables["TYPE_APPROVAL_ADVANCE"].Clear();
                _daType_Approval.SelectCommand.Parameters["p_TABLE_PLAN_APPROVAL_ID"].Value =
                    _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"];
                _daType_Approval.Fill(_dsTable_Approval.Tables["TYPE_APPROVAL_ADVANCE"]);
                // После заполнения типов решений, смотрим доступно ли хоть одно решение. 
                // Если доступно, то добавляем новую запись чтобы пользователю осталось нажать лишь кнопку сохранить
                if (_dsTable_Approval.Tables["TYPE_APPROVAL_ADVANCE"].DefaultView.Count > 0)
                {
                    cbType_Approval_Advance.ItemsSource = _dsTable_Approval.Tables["TYPE_APPROVAL_ADVANCE"].DefaultView;
                    cbType_Approval_Advance.SelectedIndex = 0;
                    cbType_Approval_Advance.IsEnabled = true;
                    btSave_Approval_Advance.IsEnabled = true;
                    _dsTable_Approval.Tables["ADVANCE_APPROVAL"].RejectChanges();
                    _row_Advance_Approval = _dsTable_Approval.Tables["ADVANCE_APPROVAL"].DefaultView.AddNew();
                    _row_Advance_Approval["TABLE_CLOSING_ID"] = Advance_Closing_ID;
                    _row_Advance_Approval["TABLE_PLAN_APPROVAL_ID"] =
                        _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"];
                    _dsTable_Approval.Tables["ADVANCE_APPROVAL"].Rows.Add(_row_Advance_Approval.Row);
                }
            }
            //cbType_Approval_Table.DataSource = null;
            cbType_Approval_Table.IsEnabled = false;
            btSave_Approval_Table.IsEnabled = false;
            tbNote_Approval.IsEnabled = false;
            if (Table_Closing_ID != null)
            {
                // Отрабатываем согласование табеля
                _ocGet_Status_Closing.Parameters["p_TABLE_CLOSING_ID"].Value = Table_Closing_ID;
                _dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID"] =
                    _ocGet_Status_Closing.ExecuteScalar();
                _dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"] =
                    _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView.Table.Select().Where(r =>
                        r["TABLE_PLAN_APPROVAL_ID"].ToString() == _dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"].
                            DefaultView[0]["TABLE_PLAN_APPROVAL_ID"].ToString()).FirstOrDefault()["TABLE_PLAN_APPROVAL_ID_PRIOR"];

                _dsTable_Approval.Tables["TYPE_APPROVAL_TABLE"].Clear();
                _daType_Approval.SelectCommand.Parameters["p_TABLE_PLAN_APPROVAL_ID"].Value =
                    _dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"];
                _daType_Approval.Fill(_dsTable_Approval.Tables["TYPE_APPROVAL_TABLE"]);
                // После заполнения типов решений, смотрим доступно ли хоть одно решение. 
                // Если доступно, то добавляем новую запись чтобы пользователю осталось нажать лишь кнопку сохранить
                if (_dsTable_Approval.Tables["TYPE_APPROVAL_TABLE"].DefaultView.Count > 0)
                {
                    cbType_Approval_Table.ItemsSource = _dsTable_Approval.Tables["TYPE_APPROVAL_TABLE"].DefaultView;
                    cbType_Approval_Table.SelectedIndex = 0;
                    cbType_Approval_Table.IsEnabled = true;
                    tbNote_Approval.IsEnabled = true;
                    tbNote_Approval.Text = "";
                    btSave_Approval_Table.IsEnabled = true;
                    _dsTable_Approval.Tables["TABLE_APPROVAL"].RejectChanges();
                    _row_Table_Approval = _dsTable_Approval.Tables["TABLE_APPROVAL"].DefaultView.AddNew();
                    _row_Table_Approval["TABLE_CLOSING_ID"] = Table_Closing_ID;
                    _row_Table_Approval["TABLE_PLAN_APPROVAL_ID"] =
                        _dsTable_Approval.Tables["PLAN_APPROVAL_TABLE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"];
                    _dsTable_Approval.Tables["TABLE_APPROVAL"].Rows.Add(_row_Table_Approval.Row);
                }
                _ocSign_Table_Closing.Parameters["p_TABLE_CLOSING_ID"].Value = Table_Closing_ID;
                DataTable _dtSign_Table_Closing = new DataTable();
                new OracleDataAdapter(_ocSign_Table_Closing).Fill(_dtSign_Table_Closing);
                if (Convert.ToBoolean(_dtSign_Table_Closing.Rows[0]["FL_TABLE_CLOSING"]))
                {
                    if (!GrantedRoles.GetGrantedRole("TABLE_FORM_FILE"))
                    {
                        flagClosedTable = true;
                    }
                }
                lbTable_Closing.Visibility = Visibility.Visible;
                lbTable_Closing.Text = "СТАТУС - " + _dtSign_Table_Closing.Rows[0]["NOTE_ROLE_APPROVAL"].ToString();
            }
            else
            {
                lbTable_Closing.Visibility = Visibility.Collapsed;
            }
        }
        
        /// <summary>
        /// Событие изменения признака отображения рассчитанных данных
        /// </summary>
        private void ChSign_Vis_Checked(object sender, RoutedEventArgs e)
        {
            dtHoursCalc.DefaultView.RowFilter = string.Format("SIGN_VISIBLE in (1, {0}) and SIGN_APPENDIX in (1, {1})",
                   (chVisibleOnlyAppendix.IsChecked == true ? 0 : Convert.ToInt32(!chSign_Vis.IsChecked)),
                   (chSign_Vis.IsChecked == true ? 0 : Convert.ToInt32(chVisibleOnlyAppendix.IsChecked)));
            dtHoursSaved.DefaultView.RowFilter = dtHoursCalc.DefaultView.RowFilter;
        }

        /// <summary>
        /// Событие изменения признака отображения только данных численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChVisibleOnlyAppendix_Checked(object sender, RoutedEventArgs e)
        {
            dtHoursCalc.DefaultView.RowFilter = string.Format("SIGN_VISIBLE in (1, {0}) and SIGN_APPENDIX in (1, {1})",
                (chVisibleOnlyAppendix.IsChecked == true ? 0 : Convert.ToInt32(!chSign_Vis.IsChecked)),
                Convert.ToInt32(chVisibleOnlyAppendix.IsChecked));
            dtHoursSaved.DefaultView.RowFilter = dtHoursCalc.DefaultView.RowFilter;
        }

        /// <summary>
        /// Событие изменения признака фильтра отгулов по году
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChFilterToYear_Checked(object sender, RoutedEventArgs e)
        {
            if (chFilterToYear.IsChecked == true)
            {
                FilterAbsence(1000, 3000);
            }
            else
            {
                FilterAbsence((decimal)nudYearFilterAbsence.Value, (decimal)nudYearFilterAbsence.Value);
            }
            nudYearFilterAbsence.IsEnabled = (bool)!chFilterToYear.IsChecked;
        }

        /// <summary>
        /// Фильтрация отгулов по годам
        /// </summary>
        /// <param name="_year_begin"></param>
        /// <param name="_year_end"></param>
        void FilterAbsence(decimal _year_begin, decimal _year_end)
        {
            dgAbsence.DataContext = null;
            dtAbsence.Clear();
            dtAbsence.SelectCommand.Parameters["p_year_begin"].Value = _year_begin;
            dtAbsence.SelectCommand.Parameters["p_year_end"].Value = _year_end;
            dtAbsence.Fill();
            dgAbsence.DataContext = dtAbsence.DefaultView;
        }

        /// <summary>
        /// Событие изменения года для фильтра отгулов
        /// </summary>
        private void nudYearFilterAbsence_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FilterAbsence((decimal)nudYearFilterAbsence.Value, (decimal)nudYearFilterAbsence.Value);
        }

        Work_Pay_Type work_pay_type;
        /// <summary>
        /// Нажатие кнопки просмотра отработанного времени по видам оплат
        /// </summary>
        /// <param name="Item">Выбранный день</param>
        public void TDG_OnItemBtnClick(TabDayItem Item)
        {
            TDG_Enter(Item);
            decimal worked_day_id = -1;
            for (int i = 0; i < dtWorked_day.Rows.Count; i++)
            {
                if (Convert.ToDateTime(dtWorked_day.Rows[i]["work_date"]).ToShortDateString() ==
                    Item.DateOfDay.ToShortDateString())
                {
                    worked_day_id = Convert.ToDecimal(dtWorked_day.Rows[i]["worked_day_id"]);
                    break;
                }
            }
            if ((Item.Text != "" || Item.NoteText != "") && worked_day_id >= 0)
            {
                /// Создаем и показываем форму
                if (work_pay_type != null)
                {
                    work_pay_type.Dispose();
                }
                DataRowView _curRow = ((DataRowView)dgEmp.SelectedItem);
                //if (Connect.UserId.ToUpper() != "BMW12714")
                //{
                //    work_pay_type = new Work_Pay_Type(_curRow["per_num"].ToString(),
                //        worked_day_id, transfer_id, Item.DateOfDay, this,
                //        Convert.ToDecimal(_curRow["degree_id"]),
                //        Convert.ToDecimal(_curRow["WORKER_ID"]));
                //    work_pay_type.ShowDialog();
                //}
                //else
                //{
                //    if (MessageBox.Show("Открыть новую форму?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                //    {
                //        Worked_Day_Viewer wd_Viewer = new Worked_Day_Viewer(this, _curRow["per_num"].ToString(), worked_day_id, transfer_id, Item.DateOfDay,
                //            Convert.ToDecimal(_curRow["WORKER_ID"]), (_curRowEmp["FL_WAYBILL"].ToString() == "X" ? true : false),
                //            dtReg_doc);
                //        wd_Viewer.Owner = Window.GetWindow(this);
                //        wd_Viewer.ShowDialog();
                //    }
                //    else
                //    {
                //        work_pay_type = new Work_Pay_Type(_curRow["per_num"].ToString(),
                //            worked_day_id, transfer_id, Item.DateOfDay, this,
                //            Convert.ToDecimal(_curRow["degree_id"]),
                //            Convert.ToDecimal(_curRow["WORKER_ID"]));
                //        work_pay_type.ShowDialog();
                //    }
                //}
                // 10.01.2017 - начинаем работу с новой формой редактирования данных за выбранный день
                Worked_Day_Viewer wd_Viewer = new Worked_Day_Viewer(this, _curRow["per_num"].ToString(), worked_day_id, transfer_id, Item.DateOfDay,
                            Convert.ToDecimal(_curRow["WORKER_ID"]), (_curRowEmp["FL_WAYBILL"].ToString() == "X" ? true : false),
                            dtReg_doc);
                wd_Viewer.Owner = Window.GetWindow(this);
                wd_Viewer.ShowDialog();
                FillEmpError(_curRow["PER_NUM"].ToString(),
                    Convert.ToInt32(_curRow["TRANSFER_ID"]),
                    Convert.ToDateTime(_curRow["date_hire"] == DBNull.Value ? BeginDate :
                        _curRow["date_hire"]),
                    Convert.ToDateTime(_curRow["date_dismiss"] == DBNull.Value ?
                        (_curRow["date_TRANSFER"] == DBNull.Value ? EndDate :
                        _curRow["date_TRANSFER"]) :
                        _curRow["date_dismiss"]));
                if (dtEmpErrors.Rows.Count == 0)
                {
                    dtEmp.Rows[Convert.ToInt32(_curRow["RN"]) - 1]["IsPink"] = "0";
                }
                else
                {
                    dtEmp.Rows[Convert.ToInt32(_curRow["RN"]) - 1]["IsPink"] = "1";
                }
                TabDayItem storeItem = RestoreItem;
                DgEmp_SelectionChanged(null, null);
                TDG_Enter(Item);
            }
            else
            {
                MessageBox.Show("Невозможно отобразить данные за выбранный день!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }
        }

        /// <summary>
        /// Получение фокуса каким-то днем табеля
        /// </summary>
        /// <param name="Item">Выбранный день</param>
        public void TDG_Enter(TabDayItem Item)
        {
            /// Если показываем табель на месяц и выбранная строка есть
            if (flagVisible && dgEmp.SelectedItem != null)
            {
                //decimal trans_ID = 0;
                //for (int i = 0; i < dtWorked_day.Rows.Count; i++)
                //{
                //    if (Convert.ToDateTime(dtWorked_day.Rows[i]["work_date"]).ToShortDateString() ==
                //        Item.DateOfDay.ToShortDateString())
                //    {
                //        trans_ID = Convert.ToDecimal(dtWorked_day.Rows[i]["transfer_id"]);
                //        break;
                //    }
                //}
                RestoreItem = Item;
                //string st = RestoreItem.Controls[2].Text;
                RestoreItem.Controls[2].BackColor = System.Drawing.Color.Aqua;
                tslCaption.Text = "Оправдательные документы на: " + Item.DateOfDay.ToShortDateString();
                GetReg_Doc(((DataRowView)dgEmp.SelectedItem)["per_num"].ToString(), ((DataRowView)dgEmp.SelectedItem)["WORKER_ID"], Item.DateOfDay);
            }
        }

        public void GetReg_Doc(string per_num, object worker_id, DateTime work_date)
        {
            dgReg_Doc.DataContext = null;
            dtReg_doc.Clear();
            dtReg_doc.SelectCommand.Parameters["p_per_num"].Value = per_num;
            dtReg_doc.SelectCommand.Parameters["p_worker_id"].Value = worker_id;
            dtReg_doc.SelectCommand.Parameters["p_date"].Value = work_date;
            dtReg_doc.Fill();
            dgReg_Doc.DataContext = dtReg_doc.DefaultView;
        }

        public void GetReg_Doc()
        {
            dgReg_Doc.DataContext = null;
            dtReg_doc.Clear();
            dtReg_doc.Fill();
            dgReg_Doc.DataContext = dtReg_doc.DefaultView;
        }

        void FillEmpError(string _per_num, int _transfer_id, DateTime _beginDate, DateTime _endDate)
        {
            dtEmpErrors.Clear();
            dtEmpErrors.SelectCommand.Parameters["p_per_num"].Value = _per_num;
            dtEmpErrors.SelectCommand.Parameters["p_transfer_id"].Value = _transfer_id;
            dtEmpErrors.SelectCommand.Parameters["p_date_begin"].Value = _beginDate;
            dtEmpErrors.SelectCommand.Parameters["p_date_end"].Value = _endDate;
            dtEmpErrors.Fill();
        }

        private void AddReg_Doc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddReg_Doc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Table.EditRegDoc editReg_doc = 
                new WpfControlLibrary.Table.EditRegDoc(Convert.ToDecimal(_curRowEmp["transfer_id"]), null, 
                !(_curRowEmp["FL_WAYBILL"] == DBNull.Value || _curRowEmp["FL_WAYBILL"].ToString() == "X"));
            editReg_doc.Owner = Window.GetWindow(this);
            if (RestoreItem == null)
                RestoreItem = TDG[Convert.ToDateTime(dtWorked_day.Rows[0]["WORK_DATE"]).Day - 1];
            editReg_doc.Title = "Добавление документа на " + RestoreItem.DateOfDay.ToShortDateString();
            if (editReg_doc.ShowDialog() == true)
            {
                TabDayItem item = RestoreItem;
                FillEmpError(_curRowEmp["PER_NUM"].ToString(),
                    Convert.ToInt32(_curRowEmp["TRANSFER_ID"]),
                    Convert.ToDateTime(_curRowEmp["date_hire"] == DBNull.Value ? BeginDate :
                        _curRowEmp["date_hire"]),
                    Convert.ToDateTime(_curRowEmp["date_dismiss"] == DBNull.Value ?
                        (_curRowEmp["date_TRANSFER"] == DBNull.Value ? EndDate :
                        _curRowEmp["date_TRANSFER"]) :
                        _curRowEmp["date_dismiss"]));
                if (dtEmpErrors.Rows.Count == 0)
                {
                    dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "0";
                }
                else
                {
                    dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "1";
                }
                DgEmp_SelectionChanged(null, null);
                RestoreItem = item;
                try
                {
                    // Проверяем имеет ли фокус выбранный день. Если нет, то ставим на него фокус.
                    // Если имеет, то перезаполняем оправдательные документы
                    TDG_Enter(RestoreItem);
                    //TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                }
                catch
                { }
            }
        }

        private void EditReg_Doc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && dgReg_Doc != null &&
                dgReg_Doc.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditReg_Doc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Table.EditRegDoc editReg_doc =
                new WpfControlLibrary.Table.EditRegDoc(Convert.ToDecimal(_curRowEmp["transfer_id"]),
                Convert.ToDecimal(((DataRowView)dgReg_Doc.SelectedCells[0].Item)["reg_doc_id"]), 
                !(_curRowEmp["FL_WAYBILL"] == DBNull.Value || _curRowEmp["FL_WAYBILL"].ToString() == "X"));
            editReg_doc.Owner = Window.GetWindow(this);
            editReg_doc.Title = "Редактирование документа на " + RestoreItem.DateOfDay.ToShortDateString();
            if (editReg_doc.ShowDialog() == true)
            {
                TabDayItem item = RestoreItem;
                FillEmpError(_curRowEmp["PER_NUM"].ToString(),
                    Convert.ToInt32(_curRowEmp["TRANSFER_ID"]),
                    Convert.ToDateTime(_curRowEmp["date_hire"] == DBNull.Value ? BeginDate :
                        _curRowEmp["date_hire"]),
                    Convert.ToDateTime(_curRowEmp["date_dismiss"] == DBNull.Value ?
                        (_curRowEmp["date_TRANSFER"] == DBNull.Value ? EndDate :
                        _curRowEmp["date_TRANSFER"]) :
                        _curRowEmp["date_dismiss"]));
                if (dtEmpErrors.Rows.Count == 0)
                {
                    dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "0";
                }
                else
                {
                    dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "1";
                }
                DgEmp_SelectionChanged(null, null);
                try
                {
                    RestoreItem = item;
                    TDG_Enter(RestoreItem);
                    //TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                }
                catch
                { }
            }
        }

        private void DeleteReg_Doc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить документ?", "АРМ \"Учет рабочего времени\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.REG_DOC_DELETE(:p_reg_doc_id);end;", Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal, ((DataRowView)dgReg_Doc.SelectedCells[0].Item)["reg_doc_id"], ParameterDirection.Input);
                OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                bool fl = false;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    fl = true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show(Window.GetWindow(this), ex.GetFormattedException(), "Ошибка удаления документа");
                }
                if (fl) // если строка удалилась то 
                {
                    try // пытаемся обновить день и список документов
                    {
                        TabDayItem item = RestoreItem;
                        FillEmpError(_curRowEmp["PER_NUM"].ToString(),
                            Convert.ToInt32(_curRowEmp["TRANSFER_ID"]),
                            Convert.ToDateTime(_curRowEmp["date_hire"] == DBNull.Value ? BeginDate :
                                _curRowEmp["date_hire"]),
                            Convert.ToDateTime(_curRowEmp["date_dismiss"] == DBNull.Value ?
                                (_curRowEmp["date_TRANSFER"] == DBNull.Value ? EndDate :
                                _curRowEmp["date_TRANSFER"]) :
                                _curRowEmp["date_dismiss"]));
                        if (dtEmpErrors.Rows.Count == 0)
                        {
                            dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "0";
                        }
                        else
                        {
                            dtEmp.Rows[Convert.ToInt32(_curRowEmp["RN"]) - 1]["IsPink"] = "1";
                        }
                        DgEmp_SelectionChanged(null, null);
                        RestoreItem = item;
                        //TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                        TDG_Enter(RestoreItem);
                        //GetReg_Doc(((DataRowView)dgEmp.SelectedItem)["per_num"].ToString(), Convert.ToDecimal(_curRowEmp["WORKER_ID"]), item.DateOfDay);
                    }
                    catch
                    { }
                }
            }
        }
        
        private void SelectOrderChange_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void SelectOrderChange_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditOrder editOrder = new EditOrder(false);
            editOrder.ShowInTaskbar = false;
            editOrder.ShowDialog();
            if (editOrder.Order_ID_Property != -1)
            {
                order_id = editOrder.Order_ID_Property;
                tbOrder_Name.Text = editOrder.Order_Name_Property;
            }
        }

        private void RestoreOrderTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите восстановить заказ по умолчанию?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                ocUpdate.BindByName = true;
                ocUpdate.CommandText = string.Format(Queries.GetQuery("Table/UpdateOrderForPT.sql"),
                    Connect.Schema);
                ocUpdate.Parameters.Add("p_per_num", OracleDbType.Varchar2);
                ocUpdate.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
                ocUpdate.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
                ocUpdate.Parameters.Add("p_beginDate", OracleDbType.Date);
                ocUpdate.Parameters.Add("p_endDate", OracleDbType.Date);
                ocUpdate.Parameters["p_per_num"].Value = _curRowEmp["per_num"].ToString();
                ocUpdate.Parameters["p_subdiv_id"].Value = Subdiv_ID;
                ocUpdate.Parameters["p_transfer_id"].Value = transfer_id;
                ocUpdate.Parameters["p_beginDate"].Value = BeginDate;
                ocUpdate.Parameters["p_endDate"].Value = EndDate;
                ocUpdate.ExecuteNonQuery();
                Connect.Commit();
                Calc_Hours(dtHoursCalc, Convert.ToInt32(_curRowEmp["transfer_id"]),
                    Convert.ToInt32(_curRowEmp["sign_comb"]),
                    BeginDate, EndDate);
            }
        }

        private void ChangeOrderTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (dCloseTable > BeginDate)
            {
                MessageBox.Show("Нельзя редактировать заказ за прошедший период!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (mbCountHours.Text.Trim().Replace(",", "") == "" || mbCountHours.Text == null)
            {
                MessageBox.Show("Вы не ввели количество часов!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                mbCountHours.Focus();
                return;
            }
            else
            {
                decimal dec = 0;
                Decimal.TryParse(mbCountHours.Text, out dec);
                if (!(dec > 0))
                {
                    MessageBox.Show("Указано неверное количество часов!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    mbCountHours.Focus();
                    return;
                }
            }
            if (tbOrder_Name.Text == "" || tbOrder_Name.Text == null)
            {
                MessageBox.Show("Вы не ввели номер заказа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                tbOrder_Name.Focus();
                return;
            }
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.ReplaceOrder(:p_per_num, :p_transfer_id, :p_countHours, :p_beginDate, " +
                ":p_endDate, :p_order_id, :p_subdiv_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _curRowEmp["per_num"].ToString();
            com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
            com.Parameters.Add("p_countHours", OracleDbType.Decimal).Value = Convert.ToDecimal(mbCountHours.Text) * 3600;
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_order_id", OracleDbType.Decimal).Value = order_id;
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.ExecuteNonQuery();
            Connect.Commit();
            MessageBox.Show("Данные сохранены!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            mbCountHours.Text = "";
            tbOrder_Name.Text = "";
            Calc_Hours(dtHoursCalc, Convert.ToInt32(_curRowEmp["transfer_id"]),
                Convert.ToInt32(_curRowEmp["sign_comb"]),
                BeginDate, EndDate);
        }

        private void ViewEmpTable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (dgEmp != null && dgEmp.SelectedItem != null)
                e.CanExecute = true;
        }

        private void ViewEmpTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditEmp editEmp = new EditEmp(_curRowEmp["per_num"].ToString(), 
                Convert.ToDecimal(_curRowEmp["transfer_id"]), 
                Convert.ToDecimal(_curRowEmp["worker_id"]),
                _curRowEmp["EMP_LAST_NAME"].ToString(), 
                _curRowEmp["EMP_FIRST_NAME"].ToString(), 
                _curRowEmp["EMP_MIDDLE_NAME"].ToString(), 
                _curRowEmp["POS_NAME"].ToString(),
                Convert.ToInt16(Subdiv_ID), Convert.ToInt32(_curRowEmp["sign_comb"]),
                _curRowEmp["ORDER_NAME"].ToString(),
                _curRowEmp["CODE_DEGREE"].ToString(),
                _curRowEmp["GROUP_MASTER"].ToString(), EndDate);
            editEmp.ShowDialog();
            Calc_Hours(dtHoursCalc, Convert.ToDecimal(_curRowEmp["transfer_id"]), Convert.ToInt32(_curRowEmp["sign_comb"]), BeginDate, EndDate);
        }

        private void BtFindEmp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && Subdiv_ID != 0)
                e.CanExecute = true;
        }

        private void BtFindEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /// Создаем форму поиска
            Find_EmpTable find_empTable = new Find_EmpTable();
            find_empTable.ShowInTaskbar = false;
            /// Если результат ОК
            if (find_empTable.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dtFind = dtEmp.DefaultView.ToTable();
                dtFind.DefaultView.RowFilter = find_empTable.str_find.ToString();
                /// Если данные найдены
                if (dtFind.DefaultView.Count != 0)
                {
                    DataTable _dtTemp = dtEmp.DefaultView.ToTable();
                    _dtTemp.PrimaryKey = new DataColumn[] { _dtTemp.Columns["WORKER_ID"] };
                    DataRowView _row = dtEmp.DefaultView[_dtTemp.Rows.IndexOf(_dtTemp.Rows.Find(dtFind.DefaultView[0]["WORKER_ID"]))];
                    if (_row != null)
                    {
                        dgEmp.SelectedItem = _row;
                        dgEmp.ScrollIntoView(_row, dgEmp.Columns[0]);
                        dgEmp.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Данные не найдены.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void BtRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /// Запрос на обновление данных по табелю
            if (MessageBox.Show("Вы действительно хотите обновить табель за\n" +
                SelectedDate.Month + " месяц " +
                SelectedDate.Year + " года?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Заносим табельные номера
                InsertPerNum();
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                timeExecute.pbPercentExecute.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RefreshTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
                LoadList();
            }
        }


        /// <summary>
        /// Вставка табельных номеров во временную таблицу по текущему списку работников
        /// </summary>
        void InsertPerNum()
        {
            // Создаем новую команду и заполняем ее строку запроса, 
            // которая будет удалять все записи из временной таблицы PN_TMP для 
            // данного пользователя
            OracleCommand command = new OracleCommand(
                string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                Connect.Schema), Connect.CurConnect);
            command.BindByName = true;
            // Выполняем команду
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            command.ExecuteNonQuery();            
            DataTable _dtPN_TMP = new DataTable();
            _daPN_TMP.Fill(_dtPN_TMP);
            /// Идем по списку работников
            for (int i = 0; i < dtEmp.Rows.Count; i++)
            {
                _dtPN_TMP.Rows.Add(dtEmp.Rows[i]["PER_NUM"], Connect.UserId.ToUpper(), dtEmp.Rows[i]["TRANSFER_ID"]);
            }
            _daPN_TMP.Update(_dtPN_TMP);
        }

        /// <summary>
        /// Метод, который рассчитывает время для табеля
        /// </summary>
        /// <param name="data"></param>
        void RefreshTable(object sender, DoWorkEventArgs e)
        {
            // Создаем новую команду
            OracleCommand command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format(
                "begin {0}.Table_Update_New(:p_month, :p_year, :p_user_name, :p_subdiv_id); end;",
                Connect.Schema);
            command.Parameters.Add("p_month", OracleDbType.Decimal).Value = SelectedDate.Month;
            command.Parameters.Add("p_year", OracleDbType.Decimal).Value = SelectedDate.Year;
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            // Выполняем команду
            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Ошибка обновления табеля\n" +
                    ex.Message, "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtRefTableList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadList();
        }

        private void BtTableForAdvance_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать табель на аванс за\n" +
                SelectedDate.Month + " месяц " +
                SelectedDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    TableForAdvance(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование отчета по табелю на аванс
        /// </summary>
        /// <param name="data"></param>
        void TableForAdvance(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду
            OracleCommand command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            int month = EndDate.Month;
            int year = EndDate.Year;
            /// Создаем запрос
            command.CommandText = string.Format(
                "begin {0}.TABLEForAdvance(:p_month, :p_year, :p_user_name, :p_subdiv_id, :p1); end;",
                Connect.Schema);
            command.Parameters.Add("p_month", OracleDbType.Int16).Value = month;
            command.Parameters.Add("p_year", OracleDbType.Int16).Value = year;
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            command.Parameters.Add("p1", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            command.Parameters["p1"].DbType = DbType.Decimal;
            ((BackgroundWorker)sender).ReportProgress(10);
            /// Выполняем команду
            command.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(40);
            decimal tempTableID = Convert.ToDecimal(command.Parameters["p1"].Value);
            /// Создаем таблицу для часов и заполняем ее
            OracleDataTable dtHours = new OracleDataTable("", Connect.CurConnect);
            /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
            ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("D1", code_subdiv),
                new ExcelParameter("F1", string.Format("{0:d2}", month)),
                new ExcelParameter("H1", string.Format("{0:d4}", year)),
                new ExcelParameter("K1", dtEmp.DefaultView[0]["code_degree"].ToString()),
                new ExcelParameter("Z1", "1")
            };
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                /// Номер текущей строки в отчете куда будет производиться вставка данных
                int rowNow = 5;
                /// Номер листа
                int sheetNumber = 1;
                /// Переменная определяет сколько строк уже заполнено в отчете
                /// Если их количество равно 6, следующий работник будет размещаться на
                /// следующем листе отчета.
                int rowsCount = 1;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Connect.CurrentAppPath + @"\Reports\AdvanceA4.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                /// Заполняем отдельные параметры
                if (excelParameters != null)
                    foreach (ExcelParameter parameter in excelParameters)
                    {
                        m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                    }
                /// Объявляем переменные для категорий и ячеек
                string degree1, degree2, cellExcel1, cellExcel2;
                degree1 = degree2 = cellExcel1 = cellExcel2 = "";
                //01.03.2011 - заполнение таблицы выходных дней (числа за выбранный месяц)
                StringBuilder Sel = new StringBuilder();
                //Sel.AppendLine("SELECT Extract(Day from Calendar_Day) FROM APStaff.Calendar ");
                Sel.AppendLine(string.Format("SELECT Extract(Day from Calendar_Day) FROM {0}.Calendar ", Connect.Schema));
                Sel.AppendLine("WHERE Calendar_Day between :D1 and :D2 ");
                Sel.AppendLine("	and Type_Day_ID in (1,4) ");
                Sel.AppendLine("ORDER BY Calendar_Day");
                OracleDataTable dtDayOff = new OracleDataTable(Sel.ToString(), Connect.CurConnect);
                dtDayOff.SelectCommand.Parameters.Add("D1", OracleDbType.Date, BeginDate, ParameterDirection.Input);
                dtDayOff.SelectCommand.Parameters.Add("D2", OracleDbType.Date, EndDate, ParameterDirection.Input);
                dtDayOff.Fill();
                //заполнение таблицы выходных дней
                //раскраска выходных дней
                m_Book.Styles[1].Interior.Color = 0xDFDFDF;
                m_Book.Styles[1].Font.Size = 8;
                ///// Создаем команду для выбора текущей базы для расчета оклада
                dtHours.SelectCommand.CommandText = string.Format(
                    "select * from {0}.TEMP_TABLE where per_num = :p_per_num and TEMP_TABLE_ID = :P_TEMP_TABLE_ID",
                    Connect.Schema);
                dtHours.SelectCommand.Parameters.Add("P_TEMP_TABLE_ID", OracleDbType.Decimal).Value = tempTableID;
                dtHours.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, "0", ParameterDirection.Input);
                /// Цикл по строкам таблицы
                for (int row = 0; row < dtEmp.DefaultView.Count; row++)
                {
                    ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)row * 50 / dtEmp.DefaultView.Count, 0)) + 40);
                    if (dtEmp.DefaultView[row]["sign_comb"].ToString() != "1")
                    {
                        /// Заносим значения
                        m_Sheet.get_Range(string.Format("A{0}", rowNow), Type.Missing).Value2 =
                            dtEmp.DefaultView[row][0];
                        m_Sheet.get_Range(string.Format("B{0}", rowNow), Type.Missing).Value2 =
                            dtEmp.DefaultView[row][5];
                        m_Sheet.get_Range(string.Format("C{0}", rowNow), Type.Missing).Value2 =
                            dtEmp.DefaultView[row][7];
                        m_Sheet.get_Range(string.Format("I{0}", rowNow), Type.Missing).Value2 =
                            dtEmp.DefaultView[row][1];
                        dtHours.Clear();
                        dtHours.SelectCommand.Parameters["p_per_num"].Value =
                            dtEmp.DefaultView[row][0];
                        dtHours.Fill();

                        //27.02.2011 - добавлена проверка на заполнение dtHours
                        if (dtHours.Rows.Count > 0) //Проверка dtHours на заполнение данными
                        {
                            /// Цикл заполняет ячейки часов и итоги за половину месяца
                            for (int col = 0; col <= 15; col++)
                            {
                                /// Заполняем значения в ячейках для текущей строки
                                /// Увеличиваем номер колонки на 3, потому что в таблице часов
                                /// первые 3 поля не хранят часы
                                m_Sheet.get_Range(string.Format("{0}{1}", Excel.ParseColNum(col + 75), rowNow), Type.Missing).Value2 =
                                    dtHours.Rows[0][col + 3];
                            }
                        }

                        /// Если текущая строка + 1 меньше количества строк
                        if (row + 1 < dtEmp.DefaultView.Count &&
                            !(row + 1 == dtEmp.DefaultView.Count
                            && dtEmp.DefaultView[dtEmp.DefaultView.Count - 1]["sign_comb"].ToString() != "1"))
                        {
                            /// Если количество заполненных строк меньше 6
                            if (rowsCount < 19)
                            {
                                /// Заносим в переменные значения категорий текущей строки и следующей
                                degree1 = dtEmp.DefaultView[row][9].ToString();
                                degree2 = dtEmp.DefaultView[row + 1][9].ToString();
                                /// Если категории равны копируем только строки для вставки данных по работнику
                                if (degree1 == degree2)
                                {
                                    /// Определяем адреса начальной и конечной ячейки для копирования
                                    cellExcel1 = string.Format("A{0}", rowNow + 1);
                                    cellExcel2 = string.Format("Z{0}", rowNow + 1);
                                    /// Копируем строки по адресам ячеек
                                    m_Sheet.get_Range("A5", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                    /// Устанавливаем высоту строк в выбранных ячейках                              
                                    m_Sheet.get_Range(cellExcel1, Type.Missing).RowHeight = 26;
                                    /// Увеличиваем номер строки в отчете на 4, 
                                    /// чтобы следующий работник вставился на нужные строки
                                    rowNow++;
                                    /// Увеличиваем количество вставленных строк
                                    rowsCount++;
                                }
                                /// Если категории не равны копируем шапку отчета
                                else
                                {
                                    /// Определяем адреса начальной и конечной ячейки для копирования
                                    cellExcel1 = string.Format("A{0}", rowNow + 1);
                                    cellExcel2 = string.Format("Z{0}", rowNow + 5);
                                    /// Копируем строки по адресам ячеек
                                    m_Sheet.get_Range("A1", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                    /// Вставляем разрыв страницы, для того чтобы новая категория была
                                    /// на новом листе отчета
                                    m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(cellExcel1, Type.Missing));
                                    /// Заносим категорию
                                    m_Sheet.get_Range(string.Format("K{0}", rowNow + 1), Type.Missing).Value2 =
                                        degree2;
                                    /// Заносим номер страницы
                                    m_Sheet.get_Range(string.Format("Z{0}", rowNow + 1), Type.Missing).Value2 =
                                        ++sheetNumber;
                                    /// Настраиваем высоту нужных строк
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 2), Type.Missing).RowHeight = 5;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 3), Type.Missing).RowHeight = 15;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 4), Type.Missing).RowHeight = 30;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 5), Type.Missing).RowHeight = 26;
                                    /// Увеличиваем номер строки в отчете на 8, 
                                    /// чтобы следующий работник вставился на нужные строки
                                    rowNow += 5;
                                    /// Сбрасываем количество вставленных строк в отчете,
                                    /// так как новую категорию печатаем на новом листе
                                    rowsCount = 1;
                                }
                            }
                            /// Если количество заполненных строк равно 19
                            else
                            {
                                for (int c = 0; c < dtDayOff.Rows.Count; c++)
                                {
                                    int ColDay = Convert.ToInt32(dtDayOff.Rows[c][0].ToString());
                                    if (ColDay < 16)
                                    {
                                        string ColName1 = string.Format("{0}{1}", Excel.ParseColNum(ColDay + 74), rowNow - 19);
                                        string ColName2 = string.Format("{0}{1}", Excel.ParseColNum(ColDay + 74), rowNow);
                                        m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[1];
                                    }
                                }
                                /// Определяем адреса начальной и конечной ячейки для копирования
                                cellExcel1 = string.Format("A{0}", rowNow + 1);
                                cellExcel2 = string.Format("Z{0}", rowNow + 5);
                                /// Копируем строки по адресам ячеек
                                m_Sheet.get_Range("A1", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                /// Вставляем разрыв страницы, для того чтобы следующий работник
                                /// печатался на новом листе отчета
                                m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(cellExcel1, Type.Missing));
                                /// Заносим номер страницы
                                m_Sheet.get_Range(string.Format("Z{0}", rowNow + 1), Type.Missing).Value2 =
                                    ++sheetNumber;
                                /// Настраиваем высоту нужных строк
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 2), Type.Missing).RowHeight = 5;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 3), Type.Missing).RowHeight = 15;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 4), Type.Missing).RowHeight = 30;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 5), Type.Missing).RowHeight = 26;
                                /// Увеличиваем номер строки в отчете на 8, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 5;
                                /// Сбрасываем количество вставленных строк в отчете,
                                /// так как новую категорию печатаем на новом листе
                                rowsCount = 1;
                            }
                        }
                    }
                    else
                    {
                        /* Если вставляли последнюю строку и она была по совместителю, ее надо удалить */
                        if (row == dtEmp.DefaultView.Count - 1)
                        {
                            m_Sheet.get_Range(cellExcel1, cellExcel2).Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
                        }
                    }
                }
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Visible = true;
                //m_Sheet.PrintPreview(true);
                //m_ExcelApp.Quit();                            
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = {1}",
                Connect.Schema, tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }

        private void BtReportTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /// Запрос, нужно ли формировать отчет по табелю
            MessageBoxResult drQuestion = MessageBox.Show("Внимание!\n" + "Сформировать табель за " +
               BeginDate.Month + " месяц " +
               BeginDate.Year + " года в разбивке на заказы?\n\n" +
                "(Да - при смене заказа сотрудник будет выводиться на новом листе отчета)\n" +
                "(Нет - при смене заказа сотрудник будет оставаться на текущем листе отчета)\n" +
                "(Отмена - табель за указанный месяц не будет формироваться)",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (drQuestion != MessageBoxResult.Cancel)
            {
                fl_form_title = false;
                fl_form_table = true;
                fl_form_appendix = false;
                // Вставляем табельные номера
                InsertPerNum();
                if (drQuestion == MessageBoxResult.Yes)
                    fl_break_order = true;
                else
                    fl_break_order = false;
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        private void BtReportPopul_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выдаем запрос нужно ли формировать отчет
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по численности за\n" +
               BeginDate.Month + " месяц " +
               BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                fl_form_title = false;
                fl_form_table = false;
                fl_form_appendix = true;
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        private void BtTableTotal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            MessageBoxResult drQuestion = MessageBox.Show("Внимание!\n" + "Сформировать табель за " +
               BeginDate.Month + " месяц " +
               BeginDate.Year + " года в разбивке на заказы?\n\n" +
                "(Да - при смене заказа сотрудник будет выводиться на новом листе отчета)\n" +
                "(Нет - при смене заказа сотрудник будет оставаться на текущем листе отчета)\n" +
                "(Отмена - табель за указанный месяц не будет формироваться)",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (drQuestion != MessageBoxResult.Cancel)
            {
                fl_form_title = true;
                fl_form_table = true;
                fl_form_appendix = true;
                if (drQuestion == MessageBoxResult.Yes)
                    fl_break_order = true;
                else
                    fl_break_order = false;
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование табеля
        /// </summary>
        /// <param name="data"></param>
        void TotalTable(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Connect.CurrentAppPath + @"\Reports\Title.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                if (fl_form_title)
                {
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                    ExcelParameter[] paramTitle = new ExcelParameter[] {
                        new ExcelParameter("AA15", code_subdiv),
                        new ExcelParameter("AF13", string.Format("{0:d2}",EndDate.Month)),
                        new ExcelParameter("AK13", string.Format("{0:d4}",EndDate.Year))
                    };
                    if (paramTitle != null)
                        foreach (ExcelParameter parameter in paramTitle)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                decimal tempTableID = 0;
                OracleDataTable dtHours;
                ExcelParameter[] excelParameters;
                if (fl_form_table)
                {
                    /// Создаем новую команду
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    /// Указываем тип - хранимая процедура
                    command.CommandType = CommandType.Text;
                    /// Создаем запрос
                    command.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                        ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                        Connect.Schema);
                    /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
                    /// часов для табеля
                    command.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                        DateTime.DaysInMonth(EndDate.Year,
                        EndDate.Month);
                    command.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                        EndDate.Month;
                    command.Parameters.Add("p_year", OracleDbType.Decimal).Value =
                        EndDate.Year;
                    command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
                    command.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
                    command.Parameters["p_temp_table_id"].Direction = ParameterDirection.Output;
                    ((BackgroundWorker)sender).ReportProgress(3);
                    try
                    {
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    if (timeExecute.backWorker.CancellationPending)
                    {
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    ((BackgroundWorker)sender).ReportProgress(8);
                    /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                    tempTableID = (decimal)((OracleDecimal)(command.Parameters["p_temp_table_id"].Value));
                    /// Создаем таблицу для часов и заполняем ее
                    dtHours = new OracleDataTable("", Connect.CurConnect);
                    dtHours.SelectCommand.CommandText = string.Format(Queries.GetQuery(
                        "Table/SelectHoursForTable.sql"), Connect.Schema);
                    //dtHours.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal);
                    //dtHours.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal);
                    /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
                    excelParameters = new ExcelParameter[] {
                        new ExcelParameter("H2", code_subdiv),
                        new ExcelParameter("L2", string.Format("{0:d2}", EndDate.Month)),
                        new ExcelParameter("P2", string.Format("{0:d4}", EndDate.Year)),
                        new ExcelParameter("W2", dtEmp.DefaultView[0]["code_degree"].ToString()),
                        new ExcelParameter("AR2", "1"),
                        new ExcelParameter("AC2", dtEmp.DefaultView[0]["group_master"].ToString() != "" ?
                            dtEmp.DefaultView[0]["group_master"].ToString() +
                            (dtEmp.DefaultView[0]["code_degree"].ToString() == "01" ||
                            dtEmp.DefaultView[0]["code_degree"].ToString() == "02" ?
                            ""
                            : " / " + dtEmp.DefaultView[0]["order_name"].ToString())
                            : dtEmp.DefaultView[0]["order_name"].ToString())
                    };

                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[2];
                    // 29.07.2016 Добавляю время закрытия в печать
                    OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                    adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectTimeClosingTable.sql"),
                        Connect.Schema);
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal).Value = Table_Closing_ID;
                    DataTable dtProtocol = new DataTable();
                    adapter.Fill(dtProtocol);
                    if (dtProtocol.Rows.Count > 0)
                    {
                        m_Sheet.Cells[1, 1] = "Закрыто " + dtProtocol.Rows[0]["DATE_APPROVAL"];
                    }
                    /// Создаем области для копирования строк по работнику и заголовка
                    Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("1:9", Type.Missing);
                    //Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("A1", "AR9");
                    Microsoft.Office.Interop.Excel.Range range_Emp = m_Sheet.get_Range("6:9", Type.Missing);
                    Microsoft.Office.Interop.Excel.Range range_cur;
                    /// Номер текущей строки в отчете куда будет производиться вставка данных
                    int rowNow = 6;
                    /// Номер листа
                    int sheetNumber = 1;
                    /// Переменная определяет сколько строк уже заполнено в отчете
                    /// Если их количество равно 6, следующий работник будет размещаться на
                    /// сделующем листе отчета.
                    int rowsCount = 1;
                    /// Заполняем отдельные параметры
                    if (excelParameters != null)
                        foreach (ExcelParameter parameter in excelParameters)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                    /// Объявляем переменные для категорий и ячеек
                    string degree1, degree2, order1, order2;
                    OracleDataTable dtDayOff = new OracleDataTable("", Connect.CurConnect);
                    dtDayOff.SelectCommand.CommandText = string.Format(
                        Queries.GetQuery("Table/SelectHolidayForTable.sql"), Connect.Schema);
                    dtDayOff.SelectCommand.Parameters.Add("D1", OracleDbType.Date).Value =
                        BeginDate;
                    dtDayOff.SelectCommand.Parameters.Add("D2", OracleDbType.Date).Value =
                        EndDate;
                    dtDayOff.Fill();
                    //заполнение таблицы выходных дней
                    //раскраска выходных дней
                    m_Book.Styles[1].Interior.Color = 0xDFDFDF;
                    m_Book.Styles[1].Font.Size = 6;
                    m_Book.Styles[2].Interior.Color = 0xDFDFDF;
                    m_Book.Styles[2].Font.Size = 8;
                    string[,] strHours = new string[4, 44];
                    for (int row = 0; row < dtEmp.DefaultView.Count; row++)
                    {
                        if (timeExecute.backWorker.CancellationPending)
                        {
                            m_Book.Close(false, Type.Missing, Type.Missing);
                            m_ExcelApp.Quit();
                            return;
                        }
                        ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)row * 50 / dtEmp.DefaultView.Count, 0)) + 10);
                        /// Заносим значения
                        strHours[0, 0] = dtEmp.DefaultView[row][0].ToString();
                        strHours[0, 1] = dtEmp.DefaultView[row][5].ToString();
                        strHours[0, 2] = dtEmp.DefaultView[row][7].ToString();
                        strHours[0, 3] = dtEmp.DefaultView[row][1].ToString();
                        /// Выбираеи из временной таблицы часы для текущего работника
                        dtHours.Clear();
                        dtHours.SelectCommand.Parameters["p_temp_table_id"].Value = tempTableID;
                        dtHours.SelectCommand.Parameters["p_TRANSFER_ID"].Value =
                            dtEmp.DefaultView[row]["TRANSFER_ID"];
                        dtHours.Fill();
                        //27.02.2011 - добавлена проверка на заполнение dtHours
                        if (dtHours.Rows.Count > 0) //Проверка dtHours на заполнение данными
                        {
                            //Перебираем все колонки
                            for (int column = 0; column < 34; column++)
                            {
                                for (int rrow = 0; rrow < 4; rrow++)
                                    strHours[rrow, column + 4] = dtHours.Rows[rrow][column].ToString();
                            }
                            strHours[0, 38] = dtHours.Rows[0][34].ToString();
                            strHours[0, 39] = dtHours.Rows[0][35].ToString();
                            strHours[0, 40] = dtHours.Rows[0][36].ToString();
                            strHours[0, 41] = dtHours.Rows[0][37].ToString();
                            strHours[0, 42] = dtHours.Rows[0][38].ToString();
                            strHours[0, 43] = dtHours.Rows[0][39].ToString();
                        }
                        m_Sheet.get_Range(string.Format("A{0}", rowNow), string.Format("AR{0}", rowNow + 3)
                                ).set_Value(Type.Missing, strHours);
                        /// Если текущая строка + 1 меньше количества строк. 
                        /// Делаем это для того, чтобы не вставить лишний заголовок если следующая строка последняя
                        if (row + 1 < dtEmp.DefaultView.Count)
                        {
                            /// Заносим в переменные значения категорий текущей строки и следующей
                            degree1 = dtEmp.DefaultView[row][9].ToString();
                            degree2 = dtEmp.DefaultView[row + 1][9].ToString();
                            // При формировании заказов определяем нужно ли нам разбивать отчет по заказам
                            // Если нет, то выводим в заказ лишь группу мастера у кого есть.
                            // Если группа мастера пустая, то в order1 пишем заказ
                            if (dtEmp.DefaultView[row]["group_master"] == DBNull.Value)
                            {
                                if (fl_break_order)
                                    order1 = dtEmp.DefaultView[row]["order_name"].ToString();
                                else
                                    order1 = "(нет разбивки)";
                            }
                            else
                            {
                                /* Если подразделение цех 61, 
                                     то в order1 записываем группу мастера + номер заказа.
                                 Иначе в order1 записываем группу мастера + номер заказа для категорий != 01 и 02*/
                                if (code_subdiv == "061")
                                {
                                    order1 = dtEmp.DefaultView[row]["group_master"].ToString() + "/" +
                                        dtEmp.DefaultView[row]["order_name"].ToString();
                                }
                                else
                                {
                                    if (fl_break_order)
                                        order1 = dtEmp.DefaultView[row]["group_master"].ToString() +
                                            (degree1 == "01" || degree1 == "02" ? ""
                                            : " / " + dtEmp.DefaultView[row]["order_name"].ToString());
                                    else
                                        order1 = dtEmp.DefaultView[row]["group_master"].ToString() +
                                            (degree1 == "01" || degree1 == "02" ? ""
                                            : " / " + "(нет разбивки)");
                                }
                            }
                            // Если группа мастера пустая, то в order2 пишем заказ
                            if (dtEmp.DefaultView[row + 1]["group_master"] == DBNull.Value)
                            {
                                if (fl_break_order)
                                    order2 = dtEmp.DefaultView[row + 1]["order_name"].ToString();
                                else
                                    order2 = "(нет разбивки)";
                            }
                            else
                            {
                                /* Если подразделение цех 61, 
                                     то в order2 записываем группу мастера + номер заказа.
                                 Иначе в order2 записываем группу мастера + номер заказа для категорий != 01 и 02*/
                                if (code_subdiv == "061")
                                {
                                    order2 = dtEmp.DefaultView[row + 1]["group_master"].ToString() + "/" +
                                        dtEmp.DefaultView[row + 1]["order_name"].ToString();
                                }
                                else
                                {
                                    if (fl_break_order)
                                        order2 = dtEmp.DefaultView[row + 1]["group_master"].ToString() +
                                            (degree2 == "01" || degree2 == "02" ? ""
                                            : " / " + dtEmp.DefaultView[row + 1]["order_name"].ToString());
                                    else
                                        order2 = dtEmp.DefaultView[row + 1]["group_master"].ToString() +
                                            (degree2 == "01" || degree2 == "02" ? ""
                                            : " / " + "(нет разбивки)");
                                }
                            }

                            /// Если количество заполненных строк меньше 6, категории равны и заказы равны
                            if (rowsCount < 6 && degree1 == degree2 && order1 == order2)
                            {
                                /// Увеличиваем номер строки в отчете на 4, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 4;
                                /// Копируем строки по адресам ячеек                                
                                range_Emp.Rows.Copy(m_Sheet.get_Range(
                                    string.Format("{0}:{1}", rowNow, rowNow + 3), Type.Missing));
                                /// Увеличиваем количество вставленных строк
                                rowsCount++;
                            }
                            else
                            {
                                /// Увеличиваем номер строки в отчете на 3, 
                                /// чтобы встать на последнюю вставленную строку
                                rowNow += 3;
                                string ColName1, ColName2;
                                foreach (DataRow rowCal in dtDayOff.Rows)
                                {
                                    // Устанавливаем цвет заголовка столбцов. Для этого от последней 
                                    // вставленной строки поднимаемся вверх на первую строку по человеку
                                    // и еще -1, чтобы быть на заголовке столбца
                                    ColName1 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_BEGIN"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1) - 1);
                                    ColName2 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_END"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1) - 1);
                                    m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[2];
                                    // Устанавливаем цвет дней табеля
                                    ColName1 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_BEGIN"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1));
                                    ColName2 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_END"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow);
                                    m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[1];
                                }
                                /// Увеличиваем номер строки в отчете на 1, 
                                /// чтобы заголовок покал на нужные строки
                                rowNow += 1;
                                /// Копируем строки по адресам ячеек                              
                                range_cur = m_Sheet.get_Range(
                                    string.Format("{0}:{1}", rowNow, rowNow + 8), Type.Missing);
                                range_Title.Rows.Copy(range_cur);
                                /// Вставляем разрыв страницы, для того чтобы новая категория была
                                /// на новом листе отчета
                                m_Sheet.HPageBreaks.Add(range_cur);

                                /// Увеличиваем номер строки в отчете на 1, 
                                /// чтобы вставлять категорию и номер страницы на нужные строки
                                rowNow += 1;
                                /// Заносим категорию
                                m_Sheet.get_Range(string.Format("W{0}", rowNow), Type.Missing).Value2 =
                                    degree2;
                                /// Заносим заказ
                                m_Sheet.get_Range(string.Format("AC{0}", rowNow), Type.Missing).Value2 =
                                    order2;
                                /// Заносим номер страницы
                                m_Sheet.get_Range(string.Format("AR{0}", rowNow), Type.Missing).Value2 =
                                    ++sheetNumber;
                                /// Увеличиваем номер строки в отчете на 4, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 4;
                                /// Сбрасываем количество вставленных строк в отчете,
                                /// так как новую категорию печатаем на новом листе
                                rowsCount = 1;
                            }
                        }
                    }
                    m_Sheet.Protect("euflfq", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                decimal temp_salary_id = 0;
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(65);
                // Если формировали табель или нужно формировать приложение, то создаем команду
                if (fl_form_table || fl_form_appendix)
                {
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        "begin {0}.Calc_Appendix(:p_user_name, :p_subdiv_id, :p_begin_date, :p_end_date, :p_temp_salary_id); end;",
                        Connect.Schema);
                    command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
                    command.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                        BeginDate;
                    command.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                        EndDate;
                    command.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    /// Выполняем команду
                    try
                    {
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                    temp_salary_id = (decimal)((OracleDecimal)(command.Parameters["p_temp_salary_id"].Value));
                }
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(75);
                // Если формировали табель, то нужно вывести протокол ошибок
                if (fl_form_table)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                    adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectProtocol.sql"),
                        Connect.Schema);
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("p_temp_table_id", OracleDbType.Decimal).Value = tempTableID;
                    adapter.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = temp_salary_id;
                    DataTable dtProtocol = new DataTable();
                    adapter.Fill(dtProtocol);

                    if (dtProtocol.Rows.Count > 0)
                    {
                        ExcelParameter[] excelParameters1 = new ExcelParameter[] {
                            new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                                EndDate.Month + " месяц " +
                                EndDate.Year + "г.")};
                        Excel.PrintWithBorder(true, "ProtocolTable.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters1);
                    }
                    // Убираем данные из временной таблицы табеля
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandText = string.Format(
                        "begin DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id; commit; end;",
                        Connect.Schema);
                    command.Parameters.Add("p_temp_table_id", tempTableID);
                    command.ExecuteNonQuery();
                }
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(80);
                // Если нужно формировать приложение
                if (fl_form_appendix)
                {
                    /// Создаем таблицу и заполняем ее
                    dtHours = new OracleDataTable("", Connect.CurConnect);
                    dtHours.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAppendix.sql"),
                        Connect.Schema);
                    dtHours.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value =
                        temp_salary_id;
                    dtHours.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                        BeginDate;
                    dtHours.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                        EndDate;
                    dtHours.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
                    dtHours.SelectCommand.Parameters.Add("p_days", OracleDbType.Decimal).Value =
                        EndDate.Day;
                    dtHours.SelectCommand.Parameters.Add("p_code_posC", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_degree", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o1", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o2", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos1", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos2", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos3", OracleDbType.Decimal);
                    /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
                    excelParameters = new ExcelParameter[] {
                        new ExcelParameter("Z2", code_subdiv),
                        new ExcelParameter("AB2", string.Format("{0:d2}", EndDate.Month)),
                        new ExcelParameter("AE2", string.Format("{0:d4}", EndDate.Year))
                    };

                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[3];
                    //Заполняем отдельные параметры
                    if (excelParameters != null)
                        foreach (ExcelParameter parameter in excelParameters)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                    dtHours.Clear();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "02";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "09";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "2";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "3";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "4";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "05";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "61";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "07";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "11";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "12";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "13";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    if (dtHours.Rows.Count > 0)
                    {
                        string[,] mas = new string[dtHours.Rows.Count, dtHours.Columns.Count];
                        for (int i = 0; i < dtHours.Rows.Count; i++)
                        {
                            ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)i * 10 / dtEmp.DefaultView.Count, 0)) + 85);
                            for (int j = 0; j < dtHours.Columns.Count; j++)
                            {
                                mas[i, j] = dtHours.Rows[i][j].ToString();
                            }
                        }
                        m_Sheet.get_Range("F13", "AM28").set_Value(Type.Missing, mas);
                        // Выбор согласования закрытия табеля
                        OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                        adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectApprovalForTableClosing.sql"),
                            Connect.Schema);
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal).Value = Table_Closing_ID;
                        DataTable dtProtocol = new DataTable();
                        adapter.Fill(dtProtocol);

                        foreach (DataRow row in dtProtocol.Rows)
                        {
                            m_Sheet.Cells[Convert.ToInt16(row["ROW_INDEX"]), Convert.ToInt16(row["COLUMN_INDEX"])] =
                                row["USER_FIO"].ToString() + " " + row["NOTE"].ToString();
                        }
                        m_Sheet.Protect("euflfq", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[3]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.Visible = true;

            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void BtRepHoursByOrders_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по отработанным часам за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Вставляем табельные номера
                InsertPerNum();
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepHoursByOrders(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование файла для расчета зарплаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RepHoursByOrders(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = null;
            com.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter _daRep = new OracleDataAdapter("", Connect.CurConnect);
            _daRep.SelectCommand.BindByName = true;
            _daRep.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepHoursByOrders.sql"),
                Connect.Schema);
            _daRep.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = temp_salary_id;
            DataTable _dtRep = new DataTable();
            _daRep.Fill(_dtRep);
            Excel.PrintWithBorder(true, "RepHoursByOrders.xlt", "A6",
                        new DataTable[] { _dtRep },
                        new ExcelParameter[] {new ExcelParameter("A2", "за " +
                           BeginDate.Month.ToString().PadLeft(2, '0').ToString() +
                            " месяц " +BeginDate.Year.ToString() + " г.")});
        }

        private void BtTable_By_Period_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Emp_By_Period_For_Table_View emp_View = new Emp_By_Period_For_Table_View((decimal)Subdiv_ID, DateTime.Today.AddMonths(-1));
            emp_View.Owner = Window.GetWindow(this);
            emp_View.ShowDialog();
        }

        private void BtCloseTableAdvance_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите закрыть табель на аванс?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // 13.04.2016 Старый стремный вариант проверки
                //foreach (DataGridViewRow row in dtEmp.DefaultView)
                //{
                //    if (row.Cells["ISPINK"].Value.ToString() == "1")
                //    {
                //        MessageBox.Show("Невозможно закрыть табель!\n" +
                //            "В списке есть работники с несбалансированным временем работы.\n" +
                //            "(горят розовым цветом)",
                //            "АРМ \"Учет рабочего времени\"",
                //            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //        return;
                //    }
                //}
                // Новый вариант
                if (dtEmp.Select("ISPINK = 1").Count() > 0)
                {
                    MessageBox.Show("Невозможно закрыть табель!\n" +
                        "В списке есть работники с несбалансированным временем работы.\n" +
                        "(горят розовым цветом)",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                // 13.04.2016 Записываем нужные данные для нового варианта закрытия табеля
                OracleCommand _ocTable_Close = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TABLE_CLOSE(:p_SUBDIV_ID, :p_TABLE_DATE, :p_TYPE_TABLE_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocTable_Close.BindByName = true;
                _ocTable_Close.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = Subdiv_ID;
                _ocTable_Close.Parameters.Add("p_TABLE_DATE", OracleDbType.Date).Value = BeginDate;
                _ocTable_Close.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal).Value = 1;
                OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocTable_Close.Transaction = _transact;
                    _ocTable_Close.ExecuteNonQuery();
                    _transact.Commit();
                    GetTable_Approval();
                }
                catch (Exception ex)
                {
                    _transact.Rollback();
                    MessageBox.Show("Ошибка закрытия табеля!\n" + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtCloseTableSalary_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите закрыть табель на зарплату?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (dtEmp.Select("ISPINK = 1").Count() > 0)
                {
                    MessageBox.Show("Невозможно закрыть табель!\n" +
                        "В списке есть работники с несбалансированным временем работы.\n" +
                        "(горят розовым цветом)",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }

        private void BtStart_Approval_Table_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (dtEmp.Select("ISPINK = 1").Count() > 0)
            {
                MessageBox.Show("Невозможно закрыть табель!\n" +
                    "В списке есть работники с несбалансированным временем работы.\n" +
                    "(горят розовым цветом)",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // 13.04.2016 Записываем нужные данные для нового варианта закрытия табеля
            OracleCommand _ocTable_Close = new OracleCommand(string.Format(
                @"BEGIN
                        {0}.TABLE_CLOSE(:p_SUBDIV_ID, :p_TABLE_DATE, :p_TYPE_TABLE_ID);
                    END;", Connect.Schema), Connect.CurConnect);
            _ocTable_Close.BindByName = true;
            _ocTable_Close.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = Subdiv_ID;
            _ocTable_Close.Parameters.Add("p_TABLE_DATE", OracleDbType.Date).Value = BeginDate;
            _ocTable_Close.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal).Value = 2;
            OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocTable_Close.Transaction = _transact;
                _ocTable_Close.ExecuteNonQuery();
                _transact.Commit();
                GetTable_Approval();
            }
            catch (Exception ex)
            {
                _transact.Rollback();
                MessageBox.Show("Ошибка закрытия табеля!\n" + ex.Message, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtDocsOnPay_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportDocsOnPay_Type reportDocs = new ReportDocsOnPay_Type(
                BeginDate, EndDate,
                Convert.ToInt16(Subdiv_ID), code_subdiv);
            reportDocs.ShowDialog();
        }

        private void BtLateness_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /// Запрос, нужно ли формировать отчет по опоздавшим
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по опозданиям?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(Lateness));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    Lateness(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        ///  Формирование отчета по опозданиям
        /// </summary>
        /// <param name="data"></param>
        void Lateness(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepLateness.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            adapter.SelectCommand.Parameters["p_date_begin"].Value = BeginDate;
            adapter.SelectCommand.Parameters["p_date_end"].Value = EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "по подразделению " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "Lateness.xlt", "A4", new DataTable[] { dtProtocol }, excelParameters, null, true);
            }
            else
            {
                MessageBox.Show("В подразделении нет документов по опозданиям.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtWorkOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по работе за территорией?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /*/// Вставляем табельные номера
                InsertPerNum();
                /// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(WorkOut));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    WorkOut(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        ///  Формирование отчета по работе за территорией
        /// </summary>
        /// <param name="data"></param>
        void WorkOut(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOut.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", Connect.UserId.ToUpper());
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            adapter.SelectCommand.Parameters["p_date_begin"].Value = BeginDate;
            adapter.SelectCommand.Parameters["p_date_end"].Value = EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "подразделения " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "WorkOut.xlt", "A4", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении нет документов по работе за территорией.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtHoursOnDegree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectDegree sd = new SelectDegree();
            if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                /// Вставляем табельные номера
                OracleCommand command = new OracleCommand(
                    string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                /// Выполняем команду
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                /// Создаем строку запроса, которая будет вставлять во временную таблицу
                /// табельные номера
                command.CommandText = string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)", Connect.Schema);
                command.Parameters.Add("PN", OracleDbType.Varchar2);
                command.Parameters.Add("UN", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal);
                for (int i = 0; i < dtEmp.DefaultView.Count; i++)
                {
                    if (dtEmp.DefaultView[i]["CODE_DEGREE"].ToString() == sd.tbCode_Degree.Text)
                    {
                        command.Parameters[0].Value = dtEmp.DefaultView[i]["per_num"].ToString();
                        command.Parameters[2].Value = dtEmp.DefaultView[i]["transfer_id"].ToString();
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                }
                Connect.Commit();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnDegree));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    HoursOnDegree(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование отчета о количестве часов по категории
        /// </summary>
        /// <param name="data"></param>
        void HoursOnDegree(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHoursOnDegree.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", Subdiv_ID);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            //adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = EndDate;
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "HoursOnDegree.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", System.Drawing.Color.Yellow, System.Drawing.Color.Black, 1m) });
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц нет данных по категории.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtHoursOnSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет?\n" +
                "Это займет продолжительное время.", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                OracleCommand command = new OracleCommand(
                    string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                /// Выполняем команду
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                /// Создаем строку запроса, которая будет вставлять во временную таблицу
                /// табельные номера
                command.CommandText = string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)", Connect.Schema);
                command.Parameters.Add("PN", OracleDbType.Varchar2);
                command.Parameters.Add("UN", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal);
                for (int i = 0; i < dtEmp.DefaultView.Count; i++)
                {
                    command.Parameters[0].Value = dtEmp.DefaultView[i]["per_num"].ToString();
                    command.Parameters[2].Value = dtEmp.DefaultView[i]["transfer_id"].ToString();
                    /// Выполняем команду
                    command.ExecuteNonQuery();
                }
                Connect.Commit();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnDegree));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    HoursOnDegree(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        private void BtRepHoursOnSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по видам оплат в разрезе группы мастера?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnSubdiv));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    HoursOnSubdiv(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование отчета о количестве часов по видам оплат
        /// </summary>
        /// <param name="data"></param>
        void HoursOnSubdiv(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));

            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHoursOnSubdiv.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", Subdiv_ID);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                EndDate;
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(80);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "HoursOnSubdiv.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", System.Drawing.Color.Yellow, System.Drawing.Color.Black, 1m) });
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц нет данных по категории.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtRepWorkHol_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = BeginDate;
            selPeriod.MaxDate = EndDate;
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepWorkHol));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepWorkHol(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование отчета по работе в выходные дни
        /// </summary>
        /// <param name="data"></param>
        void RepWorkHol(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepWorkHol.sql"),
                Connect.Schema, "PERCO");
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A3", "в подразделении " + code_subdiv + " с " +
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepWorkHol.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", System.Drawing.Color.Yellow, System.Drawing.Color.Black, 1m) },
                    true);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtRepWorkPT_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = BeginDate;
            selPeriod.MaxDate = EndDate;
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepWorkPT));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepWorkPT(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование отчета по 124 шифру оплат
        /// </summary>
        /// <param name="data"></param>
        void RepWorkPT(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepPay_Type.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 124;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A3", "в подразделении " + code_subdiv + " с " +
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepWorkHol.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", System.Drawing.Color.Yellow, System.Drawing.Color.Black, 1m) },
                    true);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtRepMission_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = BeginDate.AddYears(-100);
            selPeriod.MaxDate = EndDate.AddYears(100);
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepMission));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepMission(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }


        /// <summary>
        /// Формирование отчета по командировкам
        /// </summary>
        /// <param name="data"></param>
        void RepMission(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepMission.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal).Value = 8;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A2", "в подразделении " + code_subdiv + " с " +
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepMission.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void BtWorkOrder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepWorkOrder(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование рабочего наряда на выходные дни
        /// </summary>
        /// <param name="data"></param>
        void RepWorkOrder(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOrder.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count == 0)
            {
                MessageBox.Show("В подразделении за указанный месяц данные отсутствуют.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    WExcel.Application m_ExcelApp;
                    //Создание книги Excel
                    WExcel._Workbook m_Book;
                    //Создание страницы книги Excel
                    WExcel._Worksheet m_Sheet;
                    //private Excel.Range Range;
                    WExcel.Workbooks m_Books;
                    object oMissing = System.Reflection.Missing.Value;
                    m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    m_ExcelApp.Visible = false;
                    m_Books = m_ExcelApp.Workbooks;
                    string PathOfTemplate = Connect.CurrentAppPath + @"\Reports\WorkOrder.xlt";
                    m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                    m_ExcelApp.ScreenUpdating = false;
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                    ExcelParameter[] excelPars;
                    Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("1:6", Type.Missing);
                    Microsoft.Office.Interop.Excel.Range r_cur = null;
                    int rowNow = 1;
                    int rowIns = 0;
                    string st_key = dtProtocol.Rows[0]["WORK_DATE"].ToString() +
                        dtProtocol.Rows[0]["NGM"].ToString() + dtProtocol.Rows[0]["CODE_DEGREE"].ToString();
                    object[,] strWork = new object[dtProtocol.Rows.Count, 13];
                    for (int row = 0; row < dtProtocol.Rows.Count; row++)
                    {
                        ((BackgroundWorker)sender).ReportProgress((row * 40 / dtProtocol.Rows.Count) + 50);
                        if (rowNow != 1)
                        {
                            range_Title.Rows.Copy(m_Sheet.get_Range(
                                string.Format("{0}:{1}", rowNow, rowNow + 6), Type.Missing));
                        }
                        excelPars = new ExcelParameter[] {
                            new ExcelParameter("A"+(rowNow+1), Convert.ToDateTime(dtProtocol.Rows[row]["WORK_DATE"]).ToShortDateString()),
                            new ExcelParameter("B"+(rowNow+1), dtProtocol.Rows[row]["CODE_SUBDIV"].ToString()),
                            new ExcelParameter("C"+(rowNow+1), dtProtocol.Rows[row]["NGM"].ToString()),
                            new ExcelParameter("D"+(rowNow+1), dtProtocol.Rows[row]["CODE_DEGREE"].ToString())};
                        foreach (ExcelParameter parameter in excelPars)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                        try
                        {
                            do
                            {
                                for (int column = 0; column < 13; column++)
                                {
                                    strWork[rowIns, column] = dtProtocol.Rows[row][column + 4];
                                }
                                rowIns++;
                                st_key = dtProtocol.Rows[row]["WORK_DATE"].ToString() +
                                    dtProtocol.Rows[row]["NGM"].ToString() + dtProtocol.Rows[row]["CODE_DEGREE"].ToString();
                                row++;
                            }
                            while (st_key == dtProtocol.Rows[row]["WORK_DATE"].ToString() +
                                dtProtocol.Rows[row]["NGM"].ToString() + dtProtocol.Rows[row]["CODE_DEGREE"].ToString());
                        }
                        catch { }
                        row--;
                        rowNow += 6;
                        r_cur = m_Sheet.get_Range(string.Format("C{0}", rowNow),
                            string.Format("O{0}", rowNow + rowIns - 1));
                        r_cur.set_Value(Type.Missing, strWork);
                        r_cur = m_Sheet.get_Range(string.Format("A{0}", rowNow),
                            string.Format("P{0}", rowNow + rowIns - 1));
                        r_cur.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                        rowNow += rowIns;
                        m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(string.Format("{0}:{0}", rowNow), Type.Missing));
                        /// Увеличиваем номер строки в отчете на 4, 
                        /// чтобы следующий работник вставился на нужные строки

                        /// Копируем строки по адресам ячеек    
                        rowIns = 0;

                    };

                    m_ExcelApp.ScreenUpdating = true;
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                    m_ExcelApp.DisplayAlerts = false;
                    m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                    m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                    m_ExcelApp.Visible = true;

                }
                finally
                {
                    //Что бы там ни было вызываем сборщик мусора
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }

        private void BtRepAbsenceOnSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по опоздавшим
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по отгулам?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    AbsenceOnSubdiv(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        private void btSave_Approval_Table_Click(object sender, RoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _row_Table_Approval.Row["TYPE_APPROVAL_TABLE_ID"] = cbType_Approval_Table.SelectedValue;
                _row_Table_Approval.Row["TYPE_APPROVAL_TABLE_NAME"] = cbType_Approval_Table.Text;
                _row_Table_Approval.Row["NOTE_APPROVAL"] = tbNote_Approval.Text;
                _daTable_Approval.InsertCommand.Transaction = transact;
                _daTable_Approval.Update(_dsTable_Approval.Tables["TABLE_APPROVAL"]);
                transact.Commit();
                GetStatusApproval();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения решения", MessageBoxButton.OK, MessageBoxImage.Error);
                GetStatusApproval();
            }
        }

        private void btSave_Approval_Advance_Click(object sender, RoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _row_Advance_Approval.Row["TYPE_APPROVAL_TABLE_ID"] = cbType_Approval_Advance.SelectedValue;
                _row_Advance_Approval.Row["TYPE_APPROVAL_TABLE_NAME"] = cbType_Approval_Advance.Text;
                _daTable_Approval.InsertCommand.Transaction = transact;
                _daTable_Approval.Update(_dsTable_Approval.Tables["ADVANCE_APPROVAL"]);
                transact.Commit();
                GetStatusApproval();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения решения", MessageBoxButton.OK, MessageBoxImage.Error);
                GetStatusApproval();
            }
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

        private void btRecalcTotal_Click(object sender, RoutedEventArgs e)
        {
            if (deBegin_Period.SelectedDate == null)
            {
                MessageBox.Show("Вы не указали дату начала периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                deBegin_Period.Focus();
                return;
            }
            if (deEnd_Period.SelectedDate == null)
            {
                MessageBox.Show("Вы не указали дату окончания периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                deEnd_Period.Focus();
                return;
            }
            Calc_Hours(dtHoursPeriod, Convert.ToInt32(_curRowEmp["transfer_id"]),
                Convert.ToInt16(_curRowEmp["sign_comb"].ToString()),
                (DateTime)deBegin_Period.SelectedDate, (DateTime)deEnd_Period.SelectedDate.Value.AddDays(1).AddSeconds(-1));
            if (dtHoursPeriod.Rows.Count == 0)
            {
                MessageBox.Show("За указанный период данные отсутствуют!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //try
            //{
            //    if (Application.OpenForms["hoursEmp"].Created)
            //    {
            //        Application.OpenForms["hoursEmp"].Dispose();
            //    }
            //}
            //catch { }
            //HoursEmp hoursEmp = new HoursEmp(dtHoursPeriod);
            //hoursEmp.Text = string.Format("Итоговые часы за период с {0} по {1}",
            //    deBegin_Period.SelectedDate.Value.ToShortDateString(), deEnd_Period.SelectedDate.Value.ToShortDateString());
            //hoursEmp.ShowDialog();
            Hours_By_Period hours = new Hours_By_Period(dtHoursPeriod);
            hours.Title = string.Format("Итоговые часы за период с {0} по {1}",
                deBegin_Period.SelectedDate.Value.ToShortDateString(), deEnd_Period.SelectedDate.Value.ToShortDateString());
            hours.Owner = Window.GetWindow(this);
            hours.ShowDialog();
        }

        /// <summary>
        /// Может ли выполняться команда по сотруднику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ByEmpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = LibrarySalary.Helpers.ControlRoles.GetState(e.Command) && CurrentEmp != null;
        }

        /// <summary>
        /// Процедура синхронизации WORKED_DAY с переводам сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareTransferWithWorkedDay_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), "Сопоставить дни работы сотрудника с его переводами?", "Сопоставление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand cmd = new OracleCommand("begin APSTAFF.COMPARE_WORKDAY_TRANSFER(:p_transfer_id, :p_date);end;", Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_transfer_id", OracleDbType.Decimal, CurrentTransferID, ParameterDirection.Input);
                cmd.Parameters.Add("p_date", OracleDbType.Date, SelectedDate, ParameterDirection.Input);
                Salary.Helpers.AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Синхронизация данных...", cmd,
                    (p, pw) =>
                        { MessageBox.Show(Window.GetWindow(this), "Операция успешно завершена", "Результат"); }
                      );
            }
        }

        /// <summary>
        ///  Формирование отчета по отгулам
        /// </summary>
        /// <param name="data"></param>
        void AbsenceOnSubdiv(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAbsenceOnSubdiv.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "по подразделению " + code_subdiv + " на " +
                        DateTime.Today.ToShortDateString() + " г.")};
                Excel.PrintWithBorder(true, "RepAbsenceOnSubdiv.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters, null, true);
            }
            else
            {
                MessageBox.Show("В подразделении нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtProtocolTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolTable));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    ProtocolTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре
        /// </summary>
        /// <param name="data"></param>
        void ProtocolTable(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду, которая расчитывает данные для вывода на печать
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            /// Создаем запрос
            com.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                Connect.Schema);
            /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
            /// часов для табеля
            com.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                DateTime.DaysInMonth(EndDate.Year,
                EndDate.Month);
            com.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                EndDate.Month;
            com.Parameters.Add("p_year", OracleDbType.Decimal).Value = EndDate.Year;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
            com.Parameters["p_temp_table_id"].Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            // Выполняем команду
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            /// Переменная содержит идентификатор записей во временной таблице часов для табеля
            decimal tempTableID = (decimal)((OracleDecimal)(com.Parameters["p_temp_table_id"].Value));
            /// Создаем команду для расчета данных, которые сбрасываются в файл
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = null;
            com.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectProtocol.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_table_id", tempTableID);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            DataTable dtProtocol = new DataTable();
            ((BackgroundWorker)sender).ReportProgress(95);
            adapter.Fill(dtProtocol);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolTable.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id",
                Connect.Schema);
            command.Parameters.Add("p_temp_table_id", tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }


        private void BtErrorGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClickErrorGraph(true);
            MessageBox.Show("Работа над протоколом неверных графиков закончена." +
                "\nЕсли на экране не отобразился документ MS Excel, значит все графики верны.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Формирование отчета по ошибочным графикам работы
        /// </summary>
        /// <param name="_flagQuestion"></param>
        public void ClickErrorGraph(bool _flagQuestion)
        {
            // Вставляем табельные номера
            InsertPerNum();
            if (_flagQuestion)
            {
                if (MessageBox.Show("Вы действительно хотите сформировать протокол неверных графиков?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }
            // Создаем форму прогресса
            timeExecute = new TimeExecute();
            // Настраиваем что он должен выполнять
            timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
            {
                ErrorGraph(timeExecute.backWorker, e1);
            });
            // Запускаем теневой процесс
            timeExecute.backWorker.RunWorkerAsync();
            // Отображаем форму
            timeExecute.ShowDialog();
        }

        /// <summary>
        /// Формирование протокола ошибочных графиков
        /// </summary>
        /// <param name="data"></param>
        void ErrorGraph(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectErrorGraph.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", Connect.UserId.ToUpper());
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", Subdiv_ID);
            DataTable dtProtocol = new DataTable();
            ((BackgroundWorker)sender).ReportProgress(10);
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(90);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                        EndDate.Month + " месяц " + EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolErrorGraph.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }
        }

        private void BtProtoсolForAccount_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolForAccount));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    ProtocolForAccount(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре для бухгалтерии
        /// </summary>
        /// <param name="data"></param>
        void ProtocolForAccount(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду, которая расчитывает данные для вывода на печать
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            /// Создаем запрос
            com.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                Connect.Schema);
            /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
            /// часов для табеля
            com.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                DateTime.DaysInMonth(EndDate.Year,
                EndDate.Month);
            com.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                EndDate.Month;
            com.Parameters.Add("p_year", OracleDbType.Decimal).Value = EndDate.Year;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_table_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            /// Выполняем команду
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            /// Переменная содержит идентификатор записей во временной таблице часов для табеля
            decimal tempTableID = (decimal)((OracleDecimal)(com.Parameters["p_temp_table_id"].Value));

            /// Создаем команду для расчета данных, которые сбрасываются в файл
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(55);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectProtocolForAccount.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_table_id", tempTableID);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(95);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " +
                        EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolForAccount.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id",
                Connect.Schema);
            command.Parameters.Add("p_temp_table_id", tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }

        private void BtRepFailedOrders_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolForAccount));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepFailedOrders(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре для бухгалтерии
        /// </summary>
        /// <param name="data"></param>
        void RepFailedOrders(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/Rep_ClosedOrders.sql"),
                Connect.Schema), Connect.CurConnect);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            adapter.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = BeginDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataSet dtProtocol = new DataSet();
            adapter.Fill(dtProtocol, "DataSet1");
            ((BackgroundWorker)sender).ReportProgress(95);
            if (dtProtocol.Tables["DataSet1"].Rows.Count > 0)
            {
                ReportViewerWindow _rep = new ReportViewerWindow("Протокол закрытых или несуществующих заказов",
                    "Reports/RepFailedOrders.rdlc", dtProtocol,
                    new List<ReportParameter>() {
                        new ReportParameter("P_CODE_SUBDIV", code_subdiv),
                        new ReportParameter("P_DATE", BeginDate.ToShortDateString())}, true);
                _rep.ShowDialog();
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtOrderHoliday_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = KadrWPF.App.OpenTabs.GetOpenTabForm(typeof(OrdersOnHoliday));
            if (t == null)
            {
                KadrWPF.App.OpenTabs.AddNewTab("Приказы", new WindowsForms_Viewer(new OrdersOnHoliday(Convert.ToInt16(Subdiv_ID))));
            }
            else
                KadrWPF.App.OpenTabs.SelectedTab = t;
        }

        private void BtViewReplEmpTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (dgEmp.SelectedItem != null)
            {
                ReplEmpForm f = new Kadr.Shtat.ReplEmpForm((decimal)_curRowEmp["transfer_id"], "TABLE");
                f.ShowDialog();
            }
        }

        private void BtRepCalc_Salary_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать ведомость расчета табеля за\n" +
                BeginDate.Month + " месяц " +
                BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepCalc_Salary));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepCalc_Salary(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();

            }
        }

        /// <summary>
        /// Формирование ведомости расчета табеля
        /// </summary>
        /// <param name="data"></param>
        void RepCalc_Salary(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем команду для расчета данных, которые сбрасываются в файл
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = Subdiv_ID;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(5);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepCalc_Salary.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", Subdiv_ID);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtRep = new DataTable();
            adapter.Fill(dtRep);
            if (dtRep.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters1 = new ExcelParameter[] {
                    new ExcelParameter("A1", "Ведомость расчета табеля в подразделении " + code_subdiv +
                        " за " + EndDate.Month + " месяц " +
                        EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "RepCalc_Salary.xlt", "A5", new DataTable[] { dtRep }, excelParameters1);
            }
            ((BackgroundWorker)sender).ReportProgress(80);
            adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepCalc_SalaryPrint.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", Subdiv_ID);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_month",
                BeginDate.Month.ToString().PadLeft(2, '0'));
            adapter.SelectCommand.Parameters.Add("p_year",
                BeginDate.Year.ToString());
            ((BackgroundWorker)sender).ReportProgress(90);
            dtRep = new DataTable();
            adapter.Fill(dtRep);
            if (dtRep.Rows.Count > 0)
            {
                TextWriter writer = new StreamWriter("c:" + string.Format(@"\Z1{0}{1}{2}.txt", code_subdiv,
                    BeginDate.Month.ToString().PadLeft(2, '0').ToString(),
                    BeginDate.Year.ToString().Substring(2, 2)), false, Encoding.GetEncoding(866));
                string st = "";
                int numpage = 1;
                int k = dtRep.Rows.Count / 64;
                int i, j, j1; i = j = 0;
                try
                {
                    for (i = 0; i < k + 1; i++)
                    {
                        writer.WriteLine("ЛИСТ " + numpage++);
                        j = j1 = 0;
                        while (j < 62)
                        {
                            st = dtRep.Rows[i * 64 + j1][2].ToString();
                            j += Convert.ToInt32(dtRep.Rows[i * 64 + j1][1]);
                            writer.WriteLine(st);
                            j1++;
                        }
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                    }
                }
                catch
                {
                    for (j1 = j; j1 < 66; j1++)
                    {
                        writer.WriteLine("");
                    }
                }
                //foreach (DataRow row in dtRep.Rows)
                //{
                //    //string[] str = Array.ConvertAll<object, string>(row.ItemArray, (el => el.ToString()));
                //    st = row.ItemArray[1].ToString(); // string.Join("", str, 1, 1);
                //    writer.WriteLine(st);
                //}
                writer.Close();
            }
        }
        
        private void R_btProtokolReplSalT_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                List<string> l = new List<string>();
                decimal t = 0m;
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery(@"Table/CalcSalaryWithClear.sql"), Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_subdiv_id", Subdiv_ID);
                cmd.Parameters.Add("p_beginDate", BeginDate);
                cmd.Parameters.Add("p_endDate", EndDate);
                cmd.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
                cmd.Parameters.Add("p_sign_calc", 1m);
                cmd.Parameters.Add("p_temp_salary_id", t);
                for (int i = 0; i < dtEmp.DefaultView.Count; ++i)
                {
                    cmd.Parameters["p_transfer_id"].Value = dtEmp.DefaultView[i]["transfer_id"];
                    cmd.ExecuteNonQuery();
                    l.Add(t.ToString());
                }
                DataTable t1 = new DataTable();
                new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table/R_ProtokolReplSal.sql"), Connect.Schema, string.Join(",", l.ToArray())), Connect.CurConnect).Fill(t1);
                if (t1.Rows.Count > 0)
                    Excel.PrintWithBorder("ProtokolReplSal.xlt", "A4", new DataTable[] { t1 }, new ExcelParameter[]{
                        new ExcelParameter("A1",string.Format("Протокол начислений ЗП на замещения по подразделению {0} с {1} по {2}",code_subdiv,BeginDate,EndDate))});
                else
                    MessageBox.Show("Нет начислений по замещениям за выбранный период", "АРМ Учет рабочего времени");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);



            }
        }

        private void R_btCombReplReport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\SelectPayType153Rep.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, Subdiv_ID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, BeginDate, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, EndDate, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add(":c", OracleDbType.RefCursor, ParameterDirection.Output);
                a.Fill(t);
                Excel.PrintWithBorder("SelectPayType153Rep.xlt", "A4", new DataTable[] { t }, 
                    new ExcelParameter[] { new ExcelParameter("C2", code_subdiv), new ExcelParameter("F2", BeginDate.ToString("MMMM yyyy") + " г.") });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }

        public void SetCurrentTransferId(object worker_id)
        {
            DataTable _dtTemp = dtEmp.DefaultView.ToTable();
            _dtTemp.PrimaryKey = new DataColumn[] { _dtTemp.Columns["WORKER_ID"] };
            DataRowView _row = dtEmp.DefaultView[_dtTemp.Rows.IndexOf(_dtTemp.Rows.Find(worker_id))];
            if (_row != null)
            {
                dgEmp.SelectedItem = _row;
                dgEmp.ScrollIntoView(_row, dgEmp.Columns[0]);
                dgEmp.Focus();
            }

        }

        public LinkData GetDataLink(object sender)
        {
            if (dgEmp.SelectedItem != null)
                return new LinkData(null, (dgEmp.SelectedItem as DataRowView).Row.Field<Decimal>("TRANSFER_ID"),
                    (dgEmp.SelectedItem as DataRowView).Row.Field<Decimal>("WORKER_ID"));
            else
                return null;
        }
        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Worker_ID, "TABLE");
        }
        public static void OpenLink(object sender, LinkData e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(@"select distinct FIRST_VALUE(subdiv_id) OVER(ORDER BY DATE_TRANSFER DESC) subdiv_id, 
                        FIRST_VALUE(transfer_id) OVER(ORDER BY DATE_TRANSFER DESC) transfer_id, 
                        FIRST_VALUE(worker_id) OVER(ORDER BY DATE_TRANSFER DESC) worker_id  
                    from {0}.transfer join {0}.subdiv using (subdiv_id) 
                    where worker_id = :p_worker_id", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_worker_id", OracleDbType.Decimal, e.Worker_ID, ParameterDirection.Input);
                cmd.BindByName = true;
                OracleDataReader r = cmd.ExecuteReader();
                r.Read();

                ViewTabBase[] t = App.OpenTabs.GetOpenTabArray(typeof(Table_Viewer));
                if (t != null)
                {
                    bool _signOpenTabs = false;
                    for (int i = 0; i < t.Count(); i++)
                    {
                        if (((Table_Viewer)t[i].ContentData).Subdiv_ID == decimal.Parse(r["SUBDIV_ID"].ToString()))
                        {
                            App.OpenTabs.SelectedTab = t[i];
                            ((Table_Viewer)t[i].ContentData).SetCurrentTransferId(r["WORKER_ID"]);
                            _signOpenTabs = true;
                            break;
                        }
                    }
                    if (_signOpenTabs == false)
                    {
                        Table_Viewer _table = new Table_Viewer();
                        ViewTabBase v = App.OpenTabs.AddNewTab("Табель", _table);
                        _table.OwnerTabBase = v;
                        _table.Subdiv_ID = (decimal)r["SUBDIV_ID"];
                        _table.SetCurrentTransferId(r["WORKER_ID"]);
                    }
                }
                else
                {
                    Table_Viewer _table = new Table_Viewer();
                    ViewTabBase v = App.OpenTabs.AddNewTab("Табель", _table);
                    _table.OwnerTabBase = v;
                    _table.Subdiv_ID = (decimal)r["SUBDIV_ID"];
                    _table.SetCurrentTransferId(r["WORKER_ID"]);
                }

                r.Close();
            }
            catch { }
        }

        private void BtAddAbsence_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && Subdiv_ID != 0
                && dgEmp != null && dgEmp.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void BtAddAbsence_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddAbsence addAbsence = new AddAbsence(_curRowEmp["per_num"].ToString());
            addAbsence.ShowInTaskbar = false;
            if (addAbsence.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Absence(_curRowEmp["per_num"].ToString(), Convert.ToDecimal(_curRowEmp["transfer_id"]));
            }
        }

        private void BtDeleteAbsence_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && Subdiv_ID != 0
                && dgAbsence != null && dgAbsence.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void BtDeleteAbsence_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись из отгулов?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (dgAbsence.SelectedCells[0].Item != null)
                {
                    OracleCommand ocSelDoc = new OracleCommand(
                        string.Format("select 1 from {0}.REG_DOC where absence_id = :p_absence_id", Connect.Schema), Connect.CurConnect);
                    ocSelDoc.BindByName = true;
                    ocSelDoc.Parameters.Add("p_absence_id", OracleDbType.Decimal, 0, "p_absence_id").Value =
                        ((DataRowView)dgAbsence.SelectedCells[0].Item)["absence_id"];
                    OracleDataReader odrDoc = ocSelDoc.ExecuteReader();
                    if (odrDoc.Read())
                    {
                        MessageBox.Show("Нельзя удалить данную запись, \nтак как она связана с оправдательным документом!", "АРМ \"Учет рабочего времени\"",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    OracleCommand ocDelAbsence = new OracleCommand(
                        string.Format("DELETE FROM {0}.ABSENCE WHERE ABSENCE_ID = :p_absence_id", Connect.Schema), Connect.CurConnect);
                    ocDelAbsence.BindByName = true;
                    ocDelAbsence.Parameters.Add("p_absence_id", OracleDbType.Decimal, 0, "p_absence_id").Value =
                        ((DataRowView)dgAbsence.SelectedCells[0].Item)["absence_id"];
                    ocDelAbsence.ExecuteNonQuery();
                    Connect.Commit();
                    Absence(_curRowEmp["per_num"].ToString(), Convert.ToDecimal(_curRowEmp["transfer_id"]));
                }
            }
        }
    }


    public class TableEmp_ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color;
            if (System.Convert.ToBoolean(value) == true)
            {
                color = new SolidColorBrush(Colors.Pink);
            }
            else
            {
                color = new SolidColorBrush(Colors.White);
            }
            return color;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    public class TableEmpCell_ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color;
            switch (System.Convert.ToInt16(value))
            {
                case 1:
                    /// Красим в голубой цвет
                    color = new SolidColorBrush(Colors.LightBlue);
                    break;
                case 2:
                    /// Красим в серобуромалиновый цвет
                    color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 128, 128, 255));
                    break;
                default:
                    color = new SolidColorBrush(Colors.Transparent);
                    break;
            }
            return color;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
