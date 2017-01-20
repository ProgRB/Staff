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
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using Kadr;
using System.Windows.Interop;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Base_Exchange.xaml
    /// </summary>
    public partial class Transfer_Project_Viewer : UserControl
    {
        private System.Windows.Forms.Form _parentWinForm;
        public System.Windows.Forms.Form ParentWinForm
        {
            get { return _parentWinForm; }
            set { _parentWinForm = value; }
        }

        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }

        private object _project_ID;
        public object Project_ID
        {
            get { return _project_ID; }
            set { _project_ID = value; }
        }

        private static OracleDataAdapter _daTransfer_Project = new OracleDataAdapter();
        private static OracleCommand _ocCopy_Transfer, _ocDependencyProject_Transfer;
        private static string stFilter = "";
        private static OracleDependency _odProject_Transfer;

        public static OracleDependency OdProject_Transfer
        {
            get { return Transfer_Project_Viewer._odProject_Transfer; }
        }

        private List<GroupNoteRoleApproval> _groupNoteRoleApproval = new List<GroupNoteRoleApproval>();
        public Transfer_Project_Viewer()
        {
            InitializeComponent();

            dgTransfer_Project.DataContext = _ds.Tables["PROJECT_TRANSFER"].DefaultView;
            dcPROJECT_PLAN_PRIOR.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;

            //var opts = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].Select().GroupBy(
            //    o => o["NOTE_ROLE_APPROVAL"].ToString());

            //var results = from p in ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].Select()
            //              group p by p["NOTE_ROLE_APPROVAL"].ToString() into g
            //              select new GroupNoteRoleApproval(g.Key, g.Select(r=> Convert.ToDecimal(r["PROJECT_PLAN_APPROVAL_ID"])).ToArray());

            //var _2groupNoteRoleApproval = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].Select().GroupBy(r => r["NOTE_ROLE_APPROVAL"],
            //    r => r["PROJECT_PLAN_APPROVAL_ID"], (key, g) => GroupNoteRoleApproval( key, g.ToArray<decimal>()) );


            // Фильтр
            //cbPROJECT_PLAN_APPROVAL.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;
            cbPROJECT_PLAN_APPROVAL.ItemsSource = from p in ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].Select()
                                                  where (Convert.ToInt16(p["PROJECT_PLAN_APPROVAL_ID"]) > -1 && Convert.ToInt16(p["PROJECT_PLAN_APPROVAL_ID"]) < 50)
                                                  group p by p["NOTE_ROLE_APPROVAL"].ToString() into g
                                                  select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["PROJECT_PLAN_APPROVAL_ID"])).ToArray());
            cbSubdivNameFilter.ItemsSource = _ds.Tables["SUBDIV_FILTER"].DefaultView;

            GetTransfer_Project();

            //_odProject_Transfer.OnChange += new OnChangeEventHandler(d_OnChange);
            RefreshDependency();

            if (GrantedRoles.GetGrantedRole("STAFF_PERSONNEL"))
                btTest.IsEnabled = true;
            if (Connect.UserId.ToUpper() == "BMW12714")
                btTest.Visibility = System.Windows.Visibility.Visible;
        }

        static Transfer_Project_Viewer()
        {
            _ds = new DataSet();
            _ds.Tables.Add("PROJECT_TRANSFER");
            _ds.Tables.Add("PROJECT_TRANSFER_ROW");
            _ds.Tables.Add("ORDER_EMP");
            _ds.Tables.Add("ORDER_EMP_COND");
            _ds.Tables.Add("APPROVAL");
            _ds.Tables.Add("SIGNES");
            _ds.Tables.Add("SUBDIV_FILTER");
            // Select
            _daTransfer_Project.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectList_Project.sql"),
                Connect.Schema), Connect.CurConnect);
            _daTransfer_Project.SelectCommand.BindByName = true;
            _daTransfer_Project.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            _daTransfer_Project.SelectCommand.Parameters.Add("p_SIGN_NOTIFICATION", OracleDbType.Int16);
            _daTransfer_Project.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".LONG_VARCHAR_COLLECTION_TYPE"; 
            // Insert
            _daTransfer_Project.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.PROJECT_TRANSFER_UPDATE(:PROJECT_TRANSFER_ID,:PER_NUM,:EMP_LAST_NAME,:EMP_FIRST_NAME,:EMP_MIDDLE_NAME,:EMP_SEX,:EMP_BIRTH_DATE,
                    :SUBDIV_ID,:POS_ID,:POS_NOTE,:DATE_HIRE,:SIGN_COMB,:TYPE_TRANSFER_ID,:DATE_TRANSFER,:DATE_END_CONTR,:TR_NUM_ORDER,:TR_DATE_ORDER,
                    :FORM_PAY,:DEGREE_ID,:FROM_POSITION,:CONTR_EMP,:DATE_CONTR,:SIGN_CUR_WORK,:PROBA_PERIOD,:SOURCE_ID,:FORM_OPERATION_ID,
                    :HARMFUL_VAC,:ADDITIONAL_VAC,:WORKER_ID,:HARMFUL_ADDITION_ADD,:COMB_ADDITION,:COMB_ADDITION_NOTE,:SALARY,:CLASSIFIC,:TARIFF_GRID_ID,
                    :SECRET_ADDITION,:SECRET_ADDITION_NOTE,:SIGN_MAT_RESP_CONTR,:OTHER_LIABILITY_BOSS,:WORKING_TIME_ID,:WORKING_TIME_COMMENT,:TRANSFER_ID,
                    :ENCODING_ADDITION,:PREMIUM_CALC_GROUP_ID,:GOVSECRET_ADDITION,:WATER_PROC,:SALARY_MISSION,:SERVICE_ADD,:TRIP_ADDITION,
                    :DATE_ADD_AGREE,:DATE_END_YOUNG_SPEC,:SUM_YOUNG_SPEC,:CHIEF_ADDITION,:CLASS_ADDITION,:COUNT_DEP21,:COUNT_DEP20,:COUNT_DEP19,:COUNT_DEP18,
                    :COUNT_DEP17,:COUNT_DEP16,:COUNT_DEP15,:COUNT_DEP14,:DATE_ADD,:SIGN_ADD,:PERCENT13,:PROF_ADDITION,:DATE_SERVANT,:TAX_CODE,:CHAR_TRANSFER_ID,
                    :CHAN_SIGN,:HIRE_SIGN,:CHAR_WORK_ID,:SIGN_RESUME,:BASE_DOC_ID,:GR_WORK_ID,:REASON_ID,:BASE_DOC_NOTE); 
                END;",
                Connect.Schema), Connect.CurConnect);
            _daTransfer_Project.InsertCommand.BindByName = true;
            _daTransfer_Project.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daTransfer_Project.InsertCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID").Direction = 
                ParameterDirection.InputOutput;
            _daTransfer_Project.InsertCommand.Parameters["PROJECT_TRANSFER_ID"].DbType = DbType.Decimal;
            _daTransfer_Project.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daTransfer_Project.InsertCommand.Parameters.Add("EMP_LAST_NAME", OracleDbType.Varchar2, 0, "EMP_LAST_NAME");
            _daTransfer_Project.InsertCommand.Parameters.Add("EMP_FIRST_NAME", OracleDbType.Varchar2, 0, "EMP_FIRST_NAME");
            _daTransfer_Project.InsertCommand.Parameters.Add("EMP_MIDDLE_NAME", OracleDbType.Varchar2, 0, "EMP_MIDDLE_NAME");
            _daTransfer_Project.InsertCommand.Parameters.Add("EMP_SEX", OracleDbType.Varchar2, 0, "EMP_SEX");
            _daTransfer_Project.InsertCommand.Parameters.Add("EMP_BIRTH_DATE", OracleDbType.Date, 0, "EMP_BIRTH_DATE");
            _daTransfer_Project.InsertCommand.Parameters.Add("SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("POS_ID", OracleDbType.Decimal, 0, "POS_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("POS_NOTE", OracleDbType.Varchar2, 0, "POS_NOTE");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_HIRE", OracleDbType.Date, 0, "DATE_HIRE").Direction =
                ParameterDirection.InputOutput;
            _daTransfer_Project.InsertCommand.Parameters["DATE_HIRE"].DbType = DbType.DateTime;
            _daTransfer_Project.InsertCommand.Parameters.Add("SIGN_COMB", OracleDbType.Decimal, 0, "SIGN_COMB");
            _daTransfer_Project.InsertCommand.Parameters.Add("TYPE_TRANSFER_ID", OracleDbType.Decimal, 0, "TYPE_TRANSFER_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_TRANSFER", OracleDbType.Date, 0, "DATE_TRANSFER").Direction = 
                ParameterDirection.InputOutput;
            _daTransfer_Project.InsertCommand.Parameters["DATE_TRANSFER"].DbType = DbType.DateTime;
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_END_CONTR", OracleDbType.Date, 0, "DATE_END_CONTR");
            _daTransfer_Project.InsertCommand.Parameters.Add("TR_NUM_ORDER", OracleDbType.Varchar2, 0, "TR_NUM_ORDER");
            _daTransfer_Project.InsertCommand.Parameters.Add("TR_DATE_ORDER", OracleDbType.Date, 0, "TR_DATE_ORDER");
            _daTransfer_Project.InsertCommand.Parameters.Add("FORM_PAY", OracleDbType.Decimal, 0, "FORM_PAY");
            _daTransfer_Project.InsertCommand.Parameters.Add("DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("FROM_POSITION", OracleDbType.Decimal, 0, "FROM_POSITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("CONTR_EMP", OracleDbType.Varchar2, 0, "CONTR_EMP");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_CONTR", OracleDbType.Date, 0, "DATE_CONTR");
            _daTransfer_Project.InsertCommand.Parameters.Add("SIGN_CUR_WORK", OracleDbType.Decimal, 0, "SIGN_CUR_WORK");
            _daTransfer_Project.InsertCommand.Parameters.Add("PROBA_PERIOD", OracleDbType.Decimal, 0, "PROBA_PERIOD");
            _daTransfer_Project.InsertCommand.Parameters.Add("SOURCE_ID", OracleDbType.Decimal, 0, "SOURCE_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("FORM_OPERATION_ID", OracleDbType.Decimal, 0, "FORM_OPERATION_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("HARMFUL_VAC", OracleDbType.Decimal, 0, "HARMFUL_VAC");
            _daTransfer_Project.InsertCommand.Parameters.Add("ADDITIONAL_VAC", OracleDbType.Decimal, 0, "ADDITIONAL_VAC");
            _daTransfer_Project.InsertCommand.Parameters.Add("WORKER_ID", OracleDbType.Decimal, 0, "WORKER_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("HARMFUL_ADDITION_ADD", OracleDbType.Decimal, 0, "HARMFUL_ADDITION_ADD");
            _daTransfer_Project.InsertCommand.Parameters.Add("COMB_ADDITION", OracleDbType.Decimal, 0, "COMB_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("COMB_ADDITION_NOTE", OracleDbType.Varchar2, 0, "COMB_ADDITION_NOTE");
            _daTransfer_Project.InsertCommand.Parameters.Add("SALARY", OracleDbType.Decimal, 0, "SALARY");
            _daTransfer_Project.InsertCommand.Parameters.Add("CLASSIFIC", OracleDbType.Decimal, 0, "CLASSIFIC");
            _daTransfer_Project.InsertCommand.Parameters.Add("TARIFF_GRID_ID", OracleDbType.Decimal, 0, "TARIFF_GRID_ID");            
            _daTransfer_Project.InsertCommand.Parameters.Add("SECRET_ADDITION", OracleDbType.Decimal, 0, "SECRET_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("SECRET_ADDITION_NOTE", OracleDbType.Varchar2, 0, "SECRET_ADDITION_NOTE");
            _daTransfer_Project.InsertCommand.Parameters.Add("SIGN_MAT_RESP_CONTR", OracleDbType.Decimal, 0, "SIGN_MAT_RESP_CONTR");
            _daTransfer_Project.InsertCommand.Parameters.Add("OTHER_LIABILITY_BOSS", OracleDbType.Varchar2, 0, "OTHER_LIABILITY_BOSS");
            _daTransfer_Project.InsertCommand.Parameters.Add("WORKING_TIME_ID", OracleDbType.Decimal, 0, "WORKING_TIME_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("WORKING_TIME_COMMENT", OracleDbType.Varchar2, 0, "WORKING_TIME_COMMENT");
            _daTransfer_Project.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("ENCODING_ADDITION", OracleDbType.Decimal, 0, "ENCODING_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("PREMIUM_CALC_GROUP_ID", OracleDbType.Decimal, 0, "PREMIUM_CALC_GROUP_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("GOVSECRET_ADDITION", OracleDbType.Decimal, 0, "GOVSECRET_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("WATER_PROC", OracleDbType.Decimal, 0, "WATER_PROC");
            _daTransfer_Project.InsertCommand.Parameters.Add("SALARY_MISSION", OracleDbType.Decimal, 0, "SALARY_MISSION");
            _daTransfer_Project.InsertCommand.Parameters.Add("SERVICE_ADD", OracleDbType.Decimal, 0, "SERVICE_ADD");
            _daTransfer_Project.InsertCommand.Parameters.Add("TRIP_ADDITION", OracleDbType.Decimal, 0, "TRIP_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_ADD_AGREE", OracleDbType.Date, 0, "DATE_ADD_AGREE");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_END_YOUNG_SPEC", OracleDbType.Date, 0, "DATE_END_YOUNG_SPEC");
            _daTransfer_Project.InsertCommand.Parameters.Add("SUM_YOUNG_SPEC", OracleDbType.Decimal, 0, "SUM_YOUNG_SPEC");
            _daTransfer_Project.InsertCommand.Parameters.Add("CHIEF_ADDITION", OracleDbType.Decimal, 0, "CHIEF_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("CLASS_ADDITION", OracleDbType.Decimal, 0, "CLASS_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP21", OracleDbType.Decimal, 0, "COUNT_DEP21");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP20", OracleDbType.Decimal, 0, "COUNT_DEP20");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP19", OracleDbType.Decimal, 0, "COUNT_DEP19");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP18", OracleDbType.Decimal, 0, "COUNT_DEP18");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP17", OracleDbType.Decimal, 0, "COUNT_DEP17");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP16", OracleDbType.Decimal, 0, "COUNT_DEP16");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP15", OracleDbType.Decimal, 0, "COUNT_DEP15");
            _daTransfer_Project.InsertCommand.Parameters.Add("COUNT_DEP14", OracleDbType.Decimal, 0, "COUNT_DEP14");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_ADD", OracleDbType.Date, 0, "DATE_ADD");
            _daTransfer_Project.InsertCommand.Parameters.Add("SIGN_ADD", OracleDbType.Decimal, 0, "SIGN_ADD");
            _daTransfer_Project.InsertCommand.Parameters.Add("PERCENT13", OracleDbType.Decimal, 0, "PERCENT13");
            _daTransfer_Project.InsertCommand.Parameters.Add("PROF_ADDITION", OracleDbType.Decimal, 0, "PROF_ADDITION");
            _daTransfer_Project.InsertCommand.Parameters.Add("DATE_SERVANT", OracleDbType.Date, 0, "DATE_SERVANT");
            _daTransfer_Project.InsertCommand.Parameters.Add("TAX_CODE", OracleDbType.Decimal, 0, "TAX_CODE");
            _daTransfer_Project.InsertCommand.Parameters.Add("CHAR_TRANSFER_ID", OracleDbType.Decimal, 0, "CHAR_TRANSFER_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("CHAN_SIGN", OracleDbType.Decimal, 0, "CHAN_SIGN");
            _daTransfer_Project.InsertCommand.Parameters.Add("HIRE_SIGN", OracleDbType.Decimal, 0, "HIRE_SIGN");
            _daTransfer_Project.InsertCommand.Parameters.Add("CHAR_WORK_ID", OracleDbType.Decimal, 0, "CHAR_WORK_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("SIGN_RESUME", OracleDbType.Decimal, 0, "SIGN_RESUME");
            _daTransfer_Project.InsertCommand.Parameters.Add("BASE_DOC_ID", OracleDbType.Decimal, 0, "BASE_DOC_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("REASON_ID", OracleDbType.Decimal, 0, "REASON_ID");
            _daTransfer_Project.InsertCommand.Parameters.Add("BASE_DOC_NOTE", OracleDbType.Varchar2, 0, "BASE_DOC_NOTE");
            // Update
            _daTransfer_Project.UpdateCommand = _daTransfer_Project.InsertCommand;
            // Delete
            _daTransfer_Project.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_TRANSFER_DELETE(:PROJECT_TRANSFER_ID,:PER_NUM,:SIGN_RESUME);
                END;", Connect.Schema), Connect.CurConnect);
            _daTransfer_Project.DeleteCommand.BindByName = true;
            _daTransfer_Project.DeleteCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daTransfer_Project.DeleteCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daTransfer_Project.DeleteCommand.Parameters.Add("SIGN_RESUME", OracleDbType.Decimal, 0, "SIGN_RESUME");

            // Copy Transfer
            _ocCopy_Transfer = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_COPY_TRANSFER(:PROJECT_TRANSFER_ID,:TRANSFER_ID,:WORKER_ID, :TYPE_TRANSFER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocCopy_Transfer.BindByName = true;
            _ocCopy_Transfer.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].DbType = DbType.Decimal;
            _ocCopy_Transfer.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal);
            _ocCopy_Transfer.Parameters.Add("WORKER_ID", OracleDbType.Decimal);
            _ocCopy_Transfer.Parameters.Add("TYPE_TRANSFER_ID", OracleDbType.Decimal);

            OracleDataAdapter _daSubdiv_For_Filter = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectSubdiv_For_Filter.sql"),
                Connect.Schema), Connect.CurConnect);
            _daSubdiv_For_Filter.SelectCommand.BindByName = true;
            _daSubdiv_For_Filter.SelectCommand.Parameters.Add("p_APP_NAME", OracleDbType.Varchar2).Value = "KADR";
            _daSubdiv_For_Filter.Fill(_ds.Tables["SUBDIV_FILTER"]);
            _ds.Tables["SUBDIV_FILTER"].PrimaryKey = new DataColumn[] { _ds.Tables["SUBDIV_FILTER"].Columns["SUBDIV_ID"] };
            _ds.Tables["SUBDIV_FILTER"].Columns.Add("DISP_SUBDIV").Expression = "CODE_SUBDIV+' ('+SUBDIV_NAME+')'+IIF(SUB_ACTUAL_SIGN=0,' <не актуально>','')";

        }

        static void RefreshDependency()
        {
            // Команда проверки обновления статуса проекта
            _ocDependencyProject_Transfer = new OracleCommand(string.Format(
                @"select PROJECT_TRANSFER_ID, PROJECT_PLAN_APPROVAL_ID from {0}.PROJECT_TRANSFER",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyProject_Transfer.CommandType = CommandType.Text;
            _odProject_Transfer = new OracleDependency(_ocDependencyProject_Transfer);
            _odProject_Transfer.QueryBasedNotification = true;
            _odProject_Transfer.OnChange += new OnChangeEventHandler(d_OnChange);
            _ocDependencyProject_Transfer.Notification.IsNotifiedOnce = false;
            _ocDependencyProject_Transfer.Notification.Timeout = 3600;
            _ocDependencyProject_Transfer.AddRowid = true;
            _ocDependencyProject_Transfer.ExecuteNonQuery();        
        }

        static void d_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Info)
                {
                    /* case OracleNotificationInfo.Shutdown: App.appTrayIcon.ShowBalloonTip(10000, "Ошибка работы с сервером", "Сервер был временно отключен. Дождитесь возобновления работы сервера.", System.Windows.Forms.ToolTipIcon.Warning);

                         listNotify.Add(new AppNotify("Ошибка работы с сервером", "Сервер был временно отключен. Дождитесь возобновления работы сервера.")); break;
                     */
                    case OracleNotificationInfo.Update:
                        /*string st1 = string.Join(",", eventArgs.Details.Rows.OfType<DataRow>().Select(p => "'" + p["ROWID"].ToString() + "'").ToArray());
                        OracleDataAdapter a = new OracleDataAdapter(string.Format(
                            "select code_subdiv, subdiv_for_close_id, last_date_processing, DATE_CLOSING, subdiv_id from {1}.subdiv_for_close s1 join {0}.subdiv using (subdiv_id) where s1.rowid in ({2})", Connect.Schema, Connect.SchemaSalary, st1), Connect.CurConnect);
                        */
                        _daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 1;
                        _daTransfer_Project.SelectCommand.Parameters["p_TABLE_ID"].Value =
                            eventArgs.Details.Rows.OfType<DataRow>().Select(i => i["ROWID"].ToString()).ToArray();
                        _ds.Tables["PROJECT_TRANSFER_ROW"].Clear();
                        _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER_ROW"]);
                        _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                        for (int i = 0; i < _ds.Tables["PROJECT_TRANSFER_ROW"].Rows.Count; i++)
                        {
                            _ds.Tables["PROJECT_TRANSFER"].LoadDataRow(_ds.Tables["PROJECT_TRANSFER_ROW"].Rows[i].ItemArray, LoadOption.OverwriteChanges);
                        }
                        break;
                    case OracleNotificationInfo.End:
                        /*_ocDependencyProject_Transfer = new OracleCommand(string.Format(
                            @"select PROJECT_TRANSFER_ID, PROJECT_PLAN_APPROVAL_ID from {0}.PROJECT_TRANSFER",
                            Connect.Schema), Connect.CurConnect);
                        _ocDependencyProject_Transfer.CommandType = CommandType.Text;
                        _odProject_Transfer = new OracleDependency(_ocDependencyProject_Transfer);
                        _odProject_Transfer.QueryBasedNotification = true;
                        _odProject_Transfer.OnChange += new OnChangeEventHandler(d_OnChange);
                        _ocDependencyProject_Transfer.Notification.IsNotifiedOnce = false;
                        _ocDependencyProject_Transfer.Notification.Timeout = 3600;
                        _ocDependencyProject_Transfer.AddRowid = true;
                        _ocDependencyProject_Transfer.ExecuteNonQuery();*/
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

        private void GetTransfer_Project()
        {
            _ds.Tables["PROJECT_TRANSFER"].Clear();
            _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowFilter = "";
            _daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
            _daTransfer_Project.SelectCommand.Parameters["p_TABLE_ID"].Value = null;
            
            _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER"]);
            /*if (!_ds.Tables["PROJECT_TRANSFER"].Columns.Contains("PHOTO"))
            {
                _ds.Tables["PROJECT_TRANSFER"].Columns.Add("PHOTO", Type.GetType("System.Byte[]"));
            }*/
        }

        private void Make_Hire_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void Make_Hire_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddEmp addEmp = new AddEmp(_ds.Tables["PROJECT_TRANSFER"], dgTransfer_Project);
            addEmp.Owner = Window.GetWindow(this);
            addEmp.ShowDialog();
            //ReloadEmp();
        }
        
        private void Make_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Find_Emp findEmp = new Find_Emp(DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value = null;
                _ocCopy_Transfer.Parameters["TRANSFER_ID"].Value = findEmp.Transfer_ID;
                _ocCopy_Transfer.Parameters["WORKER_ID"].Value = findEmp.Worker_ID;
                _ocCopy_Transfer.Parameters["TYPE_TRANSFER_ID"].Value = 2;
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocCopy_Transfer.Transaction = transact;
                    _ocCopy_Transfer.ExecuteNonQuery();
                    transact.Commit();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка копирования перевода", MessageBoxButton.OK, MessageBoxImage.Error);
                    transact.Rollback();
                    return;
                }
                _daTransfer_Project.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value;
                _daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
                _ds.Tables["PROJECT_TRANSFER_ROW"].Clear();
                _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER_ROW"]);

                DataRowView _currentEmp = _ds.Tables["PROJECT_TRANSFER"].DefaultView.AddNew();
                _currentEmp["PROJECT_TRANSFER_ID"] = _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value;
                _ds.Tables["PROJECT_TRANSFER"].Rows.Add(_currentEmp.Row);
                _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                _currentEmp.Row.Table.LoadDataRow(_ds.Tables["PROJECT_TRANSFER_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                dgTransfer_Project.SelectedItem = _currentEmp;

                Add_New_Emp_Editor trans_pr = new Add_New_Emp_Editor(_currentEmp);
                trans_pr.Owner = Window.GetWindow(this);
                if (trans_pr.ShowDialog() != true)
                {
                    _ds.Tables["PROJECT_TRANSFER"].RejectChanges();
                }
            }         
        }

        private void btFilter_Apply_Click(object sender, RoutedEventArgs e)
        {
            stFilter = "";
            if (tbPer_num.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "PER_NUM = " + tbPer_num.Text.Trim().PadLeft(5, '0'),
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_LAST_NAME like '%" + tbEmp_Last_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_First_Name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_FIRST_NAME like '%" + tbEmp_First_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_Middle_Name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_MIDDLE_NAME like '%" + tbEmp_Middle_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (cbSubdivNameFilter.SelectedValue != null)
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "SUBDIV_ID = " + cbSubdivNameFilter.SelectedValue,
                    stFilter != "" ? "and" : "").Trim();
            }
            if (cbPROJECT_PLAN_APPROVAL.SelectedValue != null)
            {                                
                stFilter = string.Format("{0} {2} {1}", stFilter, "PROJECT_PLAN_APPROVAL_ID in (" + 
                    String.Join(",", ((decimal[])cbPROJECT_PLAN_APPROVAL.SelectedValue).ToArray()) +")",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (cbTYPE_TRANSFER_ID.SelectedValue != null)
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "TYPE_TRANSFER_NAME = '" + cbTYPE_TRANSFER_ID.SelectedValue + "'",
                    stFilter != "" ? "and" : "").Trim();
            }

            _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowFilter = stFilter;
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            stFilter = "";
            cbSubdivNameFilter.SelectedValue = null;
            tbPer_num.Text = "";
            tbEmp_Last_Name.Text = "";
            tbEmp_First_Name.Text = "";
            tbEmp_Middle_Name.Text = "";
            cbPROJECT_PLAN_APPROVAL.SelectedValue = null;
            cbTYPE_TRANSFER_ID.SelectedValue = null;
            _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowFilter = "";
        }

        public static bool SaveProject()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowStateFilter;
                _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["PROJECT_TRANSFER"].DefaultView.Count; ++i)
                {
                    if (_ds.Tables["PROJECT_TRANSFER"].DefaultView[i]["WORKER_ID"] == DBNull.Value)
                        _ds.Tables["PROJECT_TRANSFER"].DefaultView[i]["WORKER_ID"] =
                            new OracleCommand(string.Format("select {0}.WORKER_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                }
                _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowStateFilter = rs;
                _daTransfer_Project.InsertCommand.Transaction = transact;
                _daTransfer_Project.UpdateCommand.Transaction = transact;
                _daTransfer_Project.DeleteCommand.Transaction = transact;
                _daTransfer_Project.Update(_ds.Tables["PROJECT_TRANSFER"]);
                transact.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Edit_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgTransfer_Project != null && dgTransfer_Project.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void dgBase_Exchange_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btEdit_Project.Command.Execute(null);
        }

        public void ReloadEmp()
        {
            if (dgTransfer_Project.SelectedCells.Count > 0)
            {
                _daTransfer_Project.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = Project_ID;
                _daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
                _daTransfer_Project.SelectCommand.Parameters["p_TABLE_ID"].Value = null;
                _ds.Tables["PROJECT_TRANSFER_ROW"].Clear();
                _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER_ROW"]);
                if (_ds.Tables["PROJECT_TRANSFER_ROW"].Rows.Count > 0)
                {
                    _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                    _ds.Tables["PROJECT_TRANSFER"].LoadDataRow(_ds.Tables["PROJECT_TRANSFER_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                }
                else
                {
                    _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                    _ds.Tables["PROJECT_TRANSFER"].Rows.Remove(_ds.Tables["PROJECT_TRANSFER"].Rows.Find(Project_ID));
                }
                _daTransfer_Project.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = null;
            }
            dgTransfer_Project.Items.Refresh();
        }

        private void Edit_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "1" ||
                ((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "2")
            {
                DataRowView _currentEmp = (DataRowView)dgTransfer_Project.SelectedCells[0].Item;
                Project_ID = _currentEmp["PROJECT_TRANSFER_ID"];
                Add_New_Emp_Editor trans_pr = new Add_New_Emp_Editor(_currentEmp);
                trans_pr.Owner = Window.GetWindow(this);
                if (trans_pr.ShowDialog() != true)
                {
                    _ds.RejectChanges();
                }
                ReloadEmp();
            }
            else if (((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "3")
            {
                DataRowView _currentEmp = (DataRowView)dgTransfer_Project.SelectedCells[0].Item;
                Project_ID = _currentEmp["PROJECT_TRANSFER_ID"];
                Dismiss_Emp_Editor trans_pr = new Dismiss_Emp_Editor(_currentEmp);
                trans_pr.Owner = Window.GetWindow(this);
                if (trans_pr.ShowDialog() != true)
                {
                    _ds.RejectChanges();
                }
                ReloadEmp();
            }
        }

        private void Delete_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "1" &&
                    !GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE")) ||
                    (((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "2" &&
                    !GrantedRoles.GetGrantedRole("STAFF_PERSONNEL")))
            {
                MessageBox.Show("Вы не обладаете необходимыми правами, чтобы удалить данный тип проекта!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Convert.ToBoolean(((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["SIGN_EXIST_APPROVAL"]))
            {
                MessageBox.Show("Нельзя удалить проект, по которому были согласования!"+
                    "\n\nПри необходимости нужно зайти в проект и в согласовании выбрать решение - Аннулировано.",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgTransfer_Project.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgTransfer_Project.SelectedCells[0].Item).Delete();
                }
                SaveProject();
            }
            dgTransfer_Project.Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _odProject_Transfer.OnChange -= d_OnChange;
                _odProject_Transfer.RemoveRegistration(Connect.CurConnect);
            }
            catch { }
        }

        private void btRefreshState_Click(object sender, RoutedEventArgs e)
        {
            GetTransfer_Project();
            btFilter_Clear_Click(null, null);
        }

        private void Make_Dismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Find_Emp findEmp = new Find_Emp(DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value = null;
                _ocCopy_Transfer.Parameters["TRANSFER_ID"].Value = findEmp.Transfer_ID;
                _ocCopy_Transfer.Parameters["WORKER_ID"].Value = findEmp.Worker_ID;
                _ocCopy_Transfer.Parameters["TYPE_TRANSFER_ID"].Value = 3;
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocCopy_Transfer.Transaction = transact;
                    _ocCopy_Transfer.ExecuteNonQuery();
                    transact.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка копирования перевода", MessageBoxButton.OK, MessageBoxImage.Error);
                    transact.Rollback();
                    return;
                }
                _daTransfer_Project.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value;
                _daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
                _ds.Tables["PROJECT_TRANSFER_ROW"].Clear();
                _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER_ROW"]);

                DataRowView _currentEmp = _ds.Tables["PROJECT_TRANSFER"].DefaultView.AddNew();
                _currentEmp["PROJECT_TRANSFER_ID"] = _ocCopy_Transfer.Parameters["PROJECT_TRANSFER_ID"].Value;
                _ds.Tables["PROJECT_TRANSFER"].Rows.Add(_currentEmp.Row);
                _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                _currentEmp.Row.Table.LoadDataRow(_ds.Tables["PROJECT_TRANSFER_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                dgTransfer_Project.SelectedItem = _currentEmp;

                Dismiss_Emp_Editor trans_pr = new Dismiss_Emp_Editor(_currentEmp);
                trans_pr.Owner = Window.GetWindow(this);
                if (trans_pr.ShowDialog() != true)
                {
                    _ds.Tables["PROJECT_TRANSFER"].RejectChanges();
                }
            }  
        }

        private void btTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Количество строчек в PROJECT_TRANSFER = " + _ds.Tables["PROJECT_TRANSFER"].Rows.Count.ToString() + "\n" +
                "Количество строчек в PROJECT_TRANSFER.DefaultView = " + _ds.Tables["PROJECT_TRANSFER"].DefaultView.Count.ToString() + "\n" +
                "DefaultView.RowFilter = " + _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowFilter + "\n" +
                "DefaultView.RowStateFilter = " + _ds.Tables["PROJECT_TRANSFER"].DefaultView.RowStateFilter + "\n");
        }
    }

    public class GroupNoteRoleApproval
    {
        private string _name_Group;
        public string Name_Group
        {
            get { return _name_Group; }
            set { _name_Group = value; }
        }

        private decimal[] _array_ppa_id;
        public decimal[] Array_PPA_ID
        {
            get { return _array_ppa_id; }
            set { _array_ppa_id = value; }
        }

        public GroupNoteRoleApproval(string name_Group, decimal[] array_PPA_ID)
        {
            _name_Group = name_Group;
            _array_ppa_id = array_PPA_ID;
        }

        public GroupNoteRoleApproval(string name_Group)
        {
            _name_Group = name_Group;
        }
    }


}
