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
    public partial class Project_Statement_Viewer : UserControl
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

        private object _statement_ID;
        public object Statement_ID
        {
            get { return _statement_ID; }
            set { _statement_ID = value; }
        }

        private static OracleDataAdapter _daProject_Statement = new OracleDataAdapter();
        private static OracleCommand _ocDependencyProject_Statement;
        private static string stFilter = "";
        private static OracleDependency _odProject_Statement;

        public static OracleDependency OdProject_Statement
        {
            get { return Project_Statement_Viewer._odProject_Statement; }
        }

        private List<GroupNoteRoleApproval> _groupNoteRoleApproval = new List<GroupNoteRoleApproval>();
        public Project_Statement_Viewer()
        {
            InitializeComponent();

            dgTransfer_Project.DataContext = _ds.Tables["PROJECT_STATEMENT"].DefaultView;
            dcPROJECT_PLAN_PRIOR.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;
            // Фильтр
            //cbPROJECT_PLAN_APPROVAL.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;
            cbPROJECT_PLAN_APPROVAL.ItemsSource = from p in ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].Select()
                                                  where Convert.ToInt16(p["PROJECT_PLAN_APPROVAL_ID"]) > -1 && Convert.ToInt16(p["PROJECT_PLAN_APPROVAL_ID"]) > 50
                                                  group p by p["NOTE_ROLE_APPROVAL"].ToString() into g
                                                  select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["PROJECT_PLAN_APPROVAL_ID"])).ToArray());
            cbSubdivNameFilter.ItemsSource = _ds.Tables["SUBDIV_FILTER"].DefaultView;

            GetProject_Statement();
            RefreshDependency();
        }

        static Project_Statement_Viewer()
        {
            _ds = new DataSet();
            _ds.Tables.Add("PROJECT_STATEMENT");
            _ds.Tables.Add("PROJECT_STATEMENT_ROW");
            _ds.Tables.Add("ORDER_EMP");
            _ds.Tables.Add("ORDER_EMP_COND");
            _ds.Tables.Add("APPROVAL");
            _ds.Tables.Add("SIGNES");
            _ds.Tables.Add("SUBDIV_FILTER");
            // Select
            _daProject_Statement.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectList_Project_Statement.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Statement.SelectCommand.BindByName = true;
            _daProject_Statement.SelectCommand.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
            _daProject_Statement.SelectCommand.Parameters.Add("p_SIGN_NOTIFICATION", OracleDbType.Int16);
            _daProject_Statement.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".LONG_VARCHAR_COLLECTION_TYPE"; 
            // Insert
            _daProject_Statement.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_STATEMENT_UPDATE(:PROJECT_STATEMENT_ID,:TRANSFER_ID,:DOCUMENT,:DATE_CREATE,:PROJECT_PLAN_APPROVAL_ID,
                        :TO_SUBDIV_ID, :BASE_DOC_ID, :TYPE_TRANSFER_ID, :PROJECT_TRANSFER_ID); 
                END;",
                Connect.Schema), Connect.CurConnect);
            _daProject_Statement.InsertCommand.BindByName = true;
            _daProject_Statement.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Statement.InsertCommand.Parameters.Add("PROJECT_STATEMENT_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_ID").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Statement.InsertCommand.Parameters["PROJECT_STATEMENT_ID"].DbType = DbType.Decimal;
            _daProject_Statement.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daProject_Statement.InsertCommand.Parameters.Add("DOCUMENT", OracleDbType.Blob, 0, "DOCUMENT");
            _daProject_Statement.InsertCommand.Parameters.Add("DATE_CREATE", OracleDbType.Date, 0, "DATE_CREATE");
            _daProject_Statement.InsertCommand.Parameters.Add("PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal, 0, "PROJECT_PLAN_APPROVAL_ID").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Statement.InsertCommand.Parameters["PROJECT_PLAN_APPROVAL_ID"].DbType = DbType.Decimal;
            _daProject_Statement.InsertCommand.Parameters.Add("TO_SUBDIV_ID", OracleDbType.Decimal, 0, "TO_SUBDIV_ID");
            _daProject_Statement.InsertCommand.Parameters.Add("BASE_DOC_ID", OracleDbType.Decimal, 0, "BASE_DOC_ID");
            _daProject_Statement.InsertCommand.Parameters.Add("TYPE_TRANSFER_ID", OracleDbType.Decimal, 0, "TYPE_TRANSFER_ID");
            _daProject_Statement.InsertCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");

            // Update
            _daProject_Statement.UpdateCommand = _daProject_Statement.InsertCommand;
            // Delete
            _daProject_Statement.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_STATEMENT_DELETE(:PROJECT_STATEMENT_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Statement.DeleteCommand.BindByName = true;
            _daProject_Statement.DeleteCommand.Parameters.Add("PROJECT_STATEMENT_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_ID");
            
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
            _ocDependencyProject_Statement = new OracleCommand(string.Format(
                @"select PROJECT_STATEMENT_ID, PROJECT_PLAN_APPROVAL_ID from {0}.PROJECT_STATEMENT",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyProject_Statement.CommandType = CommandType.Text;
            _odProject_Statement = new OracleDependency(_ocDependencyProject_Statement);
            _odProject_Statement.QueryBasedNotification = true;
            _odProject_Statement.OnChange += new OnChangeEventHandler(d_OnChange);
            _ocDependencyProject_Statement.Notification.IsNotifiedOnce = false;
            _ocDependencyProject_Statement.Notification.Timeout = 3600;
            _ocDependencyProject_Statement.AddRowid = true;
            _ocDependencyProject_Statement.ExecuteNonQuery();        
        }

        static void d_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Info)
                {
                    case OracleNotificationInfo.Update:
                        _daProject_Statement.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 1;
                        _daProject_Statement.SelectCommand.Parameters["p_TABLE_ID"].Value =
                            eventArgs.Details.Rows.OfType<DataRow>().Select(i => i["ROWID"].ToString()).ToArray();
                        _ds.Tables["PROJECT_STATEMENT_ROW"].Clear();
                        _daProject_Statement.Fill(_ds.Tables["PROJECT_STATEMENT_ROW"]);
                        _ds.Tables["PROJECT_STATEMENT"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_STATEMENT"].Columns["PROJECT_STATEMENT_ID"] };
                        for (int i = 0; i < _ds.Tables["PROJECT_STATEMENT_ROW"].Rows.Count; i++)
                        {
                            _ds.Tables["PROJECT_STATEMENT"].LoadDataRow(_ds.Tables["PROJECT_STATEMENT_ROW"].Rows[i].ItemArray, LoadOption.OverwriteChanges);
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
                MessageBox.Show(ex.Message);
            }
        }

        private void GetProject_Statement()
        {
            _ds.Tables["PROJECT_STATEMENT"].Clear();
            _ds.Tables["PROJECT_STATEMENT"].DefaultView.RowFilter = "";
            _daProject_Statement.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
            _daProject_Statement.SelectCommand.Parameters["p_TABLE_ID"].Value = null;            
            _daProject_Statement.Fill(_ds.Tables["PROJECT_STATEMENT"]);
        }

        private void Add_Project_Statement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void Add_Project_Statement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Find_Emp findEmp = new Find_Emp(DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                // Проверяем есть ли по данному сотруднику незавершенные проекты заявлений
                OracleCommand _ocCheckAddSt = new OracleCommand(string.Format(
                    @"select count(*) from {0}.PROJECT_STATEMENT PS
                    JOIN {0}.TRANSFER T ON PS.TRANSFER_ID = T.TRANSFER_ID
                    WHERE WORKER_ID = (SELECT T1.WORKER_ID FROM {0}.TRANSFER T1 WHERE T1.TRANSFER_ID = :p_TRANSFER_ID)
                        and PROJECT_TRANSFER_ID is null and PROJECT_PLAN_APPROVAL_ID >= 0 ",
                    Connect.Schema), Connect.CurConnect);
                _ocCheckAddSt.BindByName = true;
                _ocCheckAddSt.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = findEmp.Transfer_ID;
                int _count = Convert.ToInt16(_ocCheckAddSt.ExecuteScalar());
                if (_count > 0)
                {
                    MessageBox.Show("По данному сотруднику существуют непроведенные проекты заявлений о переводе!",
                        "АСУ \"Кадры\" - Ошибка добавления заявления", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataRowView _dr = _ds.Tables["PROJECT_STATEMENT"].DefaultView.AddNew();
                _dr["PROJECT_STATEMENT_ID"] = -1;
                _dr["TRANSFER_ID"] = findEmp.Transfer_ID;
                _dr["SIGN_FULL_APPROVAL"] = 0;
                _dr["SIGN_EXIST_APPROVAL"] = 0;
                _ds.Tables["PROJECT_STATEMENT"].Rows.Add(_dr.Row);
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _daProject_Statement.UpdateCommand.Transaction = transact;
                    _daProject_Statement.Update(_ds.Tables["PROJECT_STATEMENT"]);
                    transact.Commit();
                }
                catch (Exception ex)
                {
                    _ds.Tables["PROJECT_STATEMENT"].RejectChanges();
                    MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка добавления заявления", MessageBoxButton.OK, MessageBoxImage.Error);
                    transact.Rollback();
                    return;
                }
                dgTransfer_Project.SelectedItem = _dr;
                ReloadEmp();

                Project_Statement_Editor trans_pr = new Project_Statement_Editor(_dr);
                trans_pr.Owner = Window.GetWindow(this);
                if (trans_pr.ShowDialog() != true)
                {
                    _ds.Tables["PROJECT_STATEMENT"].RejectChanges();
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
                stFilter = string.Format("{0} {2} {1}", stFilter, "TO_SUBDIV_ID = " + cbSubdivNameFilter.SelectedValue,
                    stFilter != "" ? "and" : "").Trim();
            }
            if (cbPROJECT_PLAN_APPROVAL.SelectedValue != null)
            {                                
                stFilter = string.Format("{0} {2} {1}", stFilter, "PROJECT_PLAN_APPROVAL_ID in (" + 
                    String.Join(",", ((decimal[])cbPROJECT_PLAN_APPROVAL.SelectedValue).ToArray()) +")",
                    stFilter != "" ? "and" : "").Trim();
            }
            //if (cbTYPE_TRANSFER_ID.SelectedValue != null)
            //{
            //    stFilter = string.Format("{0} {2} {1}", stFilter, "TYPE_TRANSFER_NAME = '" + cbTYPE_TRANSFER_ID.SelectedValue + "'",
            //        stFilter != "" ? "and" : "").Trim();
            //}

            _ds.Tables["PROJECT_STATEMENT"].DefaultView.RowFilter = stFilter;
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
            //cbTYPE_TRANSFER_ID.SelectedValue = null;
            _ds.Tables["PROJECT_STATEMENT"].DefaultView.RowFilter = "";
        }

        public static bool SaveProject()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daProject_Statement.InsertCommand.Transaction = transact;
                _daProject_Statement.UpdateCommand.Transaction = transact;
                _daProject_Statement.DeleteCommand.Transaction = transact;
                _daProject_Statement.Update(_ds.Tables["PROJECT_STATEMENT"]);
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

        private void Edit_Project_Statement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgTransfer_Project != null && dgTransfer_Project.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void dgBase_Exchange_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btEdit_Project_Statement.Command.Execute(null);
        }

        public void ReloadEmp()
        {
            if (dgTransfer_Project.SelectedCells.Count > 0)
            {
                _daProject_Statement.SelectCommand.Parameters["p_PROJECT_STATEMENT_ID"].Value = Statement_ID;
                _daProject_Statement.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 0;
                _daProject_Statement.SelectCommand.Parameters["p_TABLE_ID"].Value = null;
                _ds.Tables["PROJECT_STATEMENT_ROW"].Clear();
                _daProject_Statement.Fill(_ds.Tables["PROJECT_STATEMENT_ROW"]);
                if (_ds.Tables["PROJECT_STATEMENT_ROW"].Rows.Count > 0)
                {
                    _ds.Tables["PROJECT_STATEMENT"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_STATEMENT"].Columns["PROJECT_STATEMENT_ID"] };
                    _ds.Tables["PROJECT_STATEMENT"].LoadDataRow(_ds.Tables["PROJECT_STATEMENT_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                }
                else
                {
                    _ds.Tables["PROJECT_STATEMENT"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_STATEMENT"].Columns["PROJECT_STATEMENT_ID"] };
                    _ds.Tables["PROJECT_STATEMENT"].Rows.Remove(_ds.Tables["PROJECT_STATEMENT"].Rows.Find(Statement_ID));
                }
                _daProject_Statement.SelectCommand.Parameters["p_PROJECT_STATEMENT_ID"].Value = null;
            }
            dgTransfer_Project.Items.Refresh();
        }

        private void Edit_Project_Statement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentEmp = (DataRowView)dgTransfer_Project.SelectedCells[0].Item;
            Statement_ID = _currentEmp["PROJECT_STATEMENT_ID"];
            Project_Statement_Editor trans_pr = new Project_Statement_Editor(_currentEmp);
            trans_pr.Owner = Window.GetWindow(this);
            if (trans_pr.ShowDialog() != true)
            {
                _ds.Tables["PROJECT_STATEMENT"].RejectChanges();
            }
            ReloadEmp();
        }

        private void Delete_Project_Statement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //if ((((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "1" &&
            //        !GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE")) ||
            //        (((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["TYPE_TRANSFER_ID"].ToString() == "2" &&
            //        !GrantedRoles.GetGrantedRole("STAFF_PERSONNEL")))
            //{
            //    MessageBox.Show("Вы не обладаете необходимыми правами, чтобы удалить данный тип проекта!",
            //        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //if (Convert.ToBoolean(((DataRowView)dgTransfer_Project.SelectedCells[0].Item)["SIGN_EXIST_APPROVAL"]))
            //{
            //    MessageBox.Show("Нельзя удалить проект, по которому были согласования!"+
            //        "\n\nПри необходимости нужно зайти в проект и в согласовании выбрать решение - Аннулировано.",
            //        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}
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
                _odProject_Statement.OnChange -= d_OnChange;
                _odProject_Statement.RemoveRegistration(Connect.CurConnect);
            }
            catch { }
        }

        private void btRefreshState_Click(object sender, RoutedEventArgs e)
        {
            GetProject_Statement();
            btFilter_Clear_Click(null, null);
        }
    }
}
