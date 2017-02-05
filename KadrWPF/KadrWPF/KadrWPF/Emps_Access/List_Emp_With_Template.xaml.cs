using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;
using PercoXML;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlLibrary.Emps_Access
{
    /// <summary>
    /// Interaction logic for List_Emp_With_Template.xaml
    /// </summary>
    public partial class List_Emp_With_Template : UserControl
    {
        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }

        private object _perco_Sync_ID;
        public object Perco_Sync_ID
        {
            get { return _perco_Sync_ID; }
            set { _perco_Sync_ID = value; }
        }
        private object _id_Card;
        public object ID_Card
        {
            get { return _id_Card; }
            set { _id_Card = value; }
        }
        private object _worker_ID;
        public object Worker_ID
        {
            get { return _worker_ID; }
            set { _worker_ID = value; }
        }
        private object _per_Num;
        public object Per_Num
        {
            get { return _per_Num; }
            set { _per_Num = value; }
        }

        public static readonly DependencyProperty AllItemsAreCheckedProperty =
            DependencyProperty.Register("AllItemsAreChecked", typeof(bool), typeof(List_Emp_With_Template),
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

        private static int flagSelect = 0;
        private static void OnAllItemsAreCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            flagSelect = Convert.ToInt32(!Convert.ToBoolean(flagSelect));
            foreach (DataRowView row in _ds.Tables["LIST_EMP"].DefaultView)
            {
                row["FL"] = flagSelect;
            }
        }

        private static OracleDataAdapter _daList_Emp = new OracleDataAdapter(), _daAccess_Emp = new OracleDataAdapter();
        private static string stFilter = "";
        private static OracleCommand _ocAccess_Emp_Update;
        public List_Emp_With_Template()
        {
            InitializeComponent();

            //dgList_Emp_With_Template.DataContext = _ds.Tables["LIST_EMP"].DefaultView;

            // Фильтр
            cbSubdivNameFilter.ItemsSource = _ds.Tables["SUBDIV_FILTER"].DefaultView;

            GetList_Emp_With_Template();
        }

        static List_Emp_With_Template()
        {
            _ds = new DataSet();
            _ds.Tables.Add("LIST_EMP");
            _ds.Tables.Add("LIST_EMP_ROW");
            _ds.Tables.Add("ACCESS_EMP");
            _ds.Tables.Add("APPROVAL");
            _ds.Tables.Add("SIGNES");
            _ds.Tables.Add("SUBDIV_FILTER");
            // Select
            _daList_Emp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("PO/SelectList_Emp_With_Template.sql"),
                Connect.Schema, "PERCO"), Connect.CurConnect);
            _daList_Emp.SelectCommand.BindByName = true;
            _daList_Emp.SelectCommand.Parameters.Add("p_ID_CARD", OracleDbType.Decimal);
            _daList_Emp.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            _daList_Emp.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal);
            
            OracleDataAdapter _daSubdiv_For_Filter = new OracleDataAdapter(string.Format(
                @"SELECT SUBDIV_ID, CODE_SUBDIV, SUBDIV_NAME, SUB_ACTUAL_SIGN, WORK_TYPE_ID, SERVICE_ID, SUB_DATE_START, 
	                SUB_DATE_END, PARENT_ID, FROM_SUBDIV, TYPE_SUBDIV_ID, GR_WORK_ID 
                FROM {0}.SUBDIV S 
                WHERE NVL(S.PARENT_ID,0) = 0 and TYPE_SUBDIV_ID != 6
                ORDER BY SUB_ACTUAL_SIGN desc, CODE_SUBDIV",
                Connect.Schema), Connect.CurConnect);
            _daSubdiv_For_Filter.SelectCommand.BindByName = true;
            _daSubdiv_For_Filter.Fill(_ds.Tables["SUBDIV_FILTER"]);
            _ds.Tables["SUBDIV_FILTER"].PrimaryKey = new DataColumn[] { _ds.Tables["SUBDIV_FILTER"].Columns["SUBDIV_ID"] };
            _ds.Tables["SUBDIV_FILTER"].Columns.Add("DISP_SUBDIV").Expression = "CODE_SUBDIV+' ('+SUBDIV_NAME+')'+IIF(SUB_ACTUAL_SIGN=0,' <не актуально>','')";

            // Select
            _daAccess_Emp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("PO/SelectAccess_Templ_By_Emp.sql"),
                Connect.Schema), Connect.CurConnect);
            _daAccess_Emp.SelectCommand.BindByName = true;
            _daAccess_Emp.SelectCommand.Parameters.Add("p_ID_CARD", OracleDbType.Decimal);
            // Update
            _daAccess_Emp.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.ACCESS_TEMPL_BY_EMP_UPDATE(:ACCESS_TEMPL_BY_EMP_ID,:PER_NUM,:ID_SHABLON_MAIN,:START_DATE_VALID,:END_DATE_VALID,:SIGN_TEMPORARY_SHABLON,:PERCO_SYNC_ID,:ID_CARD); 
                END;",
                Connect.Schema), Connect.CurConnect);
            _daAccess_Emp.UpdateCommand.BindByName = true;
            _daAccess_Emp.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daAccess_Emp.UpdateCommand.Parameters.Add("ACCESS_TEMPL_BY_EMP_ID", OracleDbType.Decimal, 0, "ACCESS_TEMPL_BY_EMP_ID").Direction =
                ParameterDirection.InputOutput;
            _daAccess_Emp.UpdateCommand.Parameters["ACCESS_TEMPL_BY_EMP_ID"].DbType = DbType.Decimal;
            _daAccess_Emp.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daAccess_Emp.UpdateCommand.Parameters.Add("ID_SHABLON_MAIN", OracleDbType.Decimal, 0, "ID_SHABLON_MAIN");
            _daAccess_Emp.UpdateCommand.Parameters.Add("START_DATE_VALID", OracleDbType.Date, 0, "START_DATE_VALID");
            _daAccess_Emp.UpdateCommand.Parameters.Add("END_DATE_VALID", OracleDbType.Date, 0, "END_DATE_VALID");
            _daAccess_Emp.UpdateCommand.Parameters.Add("SIGN_TEMPORARY_SHABLON", OracleDbType.Int16, 0, "SIGN_TEMPORARY_SHABLON");
            _daAccess_Emp.UpdateCommand.Parameters.Add("PERCO_SYNC_ID", OracleDbType.Decimal, 0, "PERCO_SYNC_ID");
            _daAccess_Emp.UpdateCommand.Parameters.Add("ID_CARD", OracleDbType.Decimal, 0, "ID_CARD");
            // Delete
            _daAccess_Emp.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.ACCESS_TEMPL_BY_EMP_DELETE(:ACCESS_TEMPL_BY_EMP_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAccess_Emp.DeleteCommand.BindByName = true;
            _daAccess_Emp.DeleteCommand.Parameters.Add("ACCESS_TEMPL_BY_EMP_ID", OracleDbType.Decimal, 0, "ACCESS_TEMPL_BY_EMP_ID");

//            _ocAccess_Emp_Update = new OracleCommand(string.Format(
//                @"BEGIN
//                    {0}.ACCESS_EMP_UPDATE(:p_ID_SHABLON_MAIN, :p_START_DATE_VALID, :p_END_DATE_VALID, :p_SIGN_TEMPORARY_SHABLON, :p_ID_CARD_TABLE);
//                END;", Connect.Schema), Connect.CurConnect);
//            _ocAccess_Emp_Update.BindByName = true;
//            _ocAccess_Emp_Update.Parameters.Add("p_ID_SHABLON_MAIN", OracleDbType.Decimal);
//            _ocAccess_Emp_Update.Parameters.Add("p_START_DATE_VALID", OracleDbType.Date);
//            _ocAccess_Emp_Update.Parameters.Add("p_END_DATE_VALID", OracleDbType.Date);
//            _ocAccess_Emp_Update.Parameters.Add("p_SIGN_TEMPORARY_SHABLON", OracleDbType.Decimal);
//            _ocAccess_Emp_Update.Parameters.Add("p_ID_CARD_TABLE", OracleDbType.Array).UdtTypeName =
//                Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
        }

        private void GetList_Emp_With_Template()
        {
            dgList_Emp_With_Template.DataContext = null;
            _ds.Tables["LIST_EMP"].Clear();
            _daList_Emp.SelectCommand.Parameters["p_SUBDIV_ID"].Value = cbSubdivNameFilter.SelectedValue;
            //_ds.Tables["LIST_EMP"].DefaultView.RowFilter = "";    
            _daList_Emp.Fill(_ds.Tables["LIST_EMP"]);
            dgList_Emp_With_Template.DataContext = _ds.Tables["LIST_EMP"].DefaultView;
        }

        private void GetAccess_Templ_By_Emp()
        {
            dgAccess_Templ_By_Emp.DataContext = null;
            _ds.Tables["ACCESS_EMP"].Clear();
            _daAccess_Emp.SelectCommand.Parameters["p_ID_CARD"].Value = ID_Card;
            _daAccess_Emp.Fill(_ds.Tables["ACCESS_EMP"]);
            dgAccess_Templ_By_Emp.DataContext = _ds.Tables["ACCESS_EMP"].DefaultView;
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
            //if (cbSubdivNameFilter.SelectedValue != null)
            //{
            //    stFilter = string.Format("{0} {2} {1}", stFilter, "SUBDIV_ID = " + cbSubdivNameFilter.SelectedValue,
            //        stFilter != "" ? "and" : "").Trim();
            //}

            _ds.Tables["LIST_EMP"].DefaultView.RowFilter = stFilter;
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            stFilter = "";
            //cbSubdivNameFilter.SelectedValue = null;
            tbPer_num.Text = "";
            tbEmp_Last_Name.Text = "";
            tbEmp_First_Name.Text = "";
            tbEmp_Middle_Name.Text = "";
            _ds.Tables["LIST_EMP"].DefaultView.RowFilter = "";
        }

        public static bool SaveAccess_Templ_By_Emp()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daAccess_Emp.InsertCommand.Transaction = transact;
                _daAccess_Emp.UpdateCommand.Transaction = transact;
                _daAccess_Emp.DeleteCommand.Transaction = transact;
                _daAccess_Emp.Update(_ds.Tables["ACCESS_EMP"]);
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

        private void EditAccess_Templ_By_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgList_Emp_With_Template != null && dgList_Emp_With_Template.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void dgList_Emp_With_Template_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btEditAccess_Templ_By_Emp.Command.Execute(null);
        }

        public void ReloadEmp()
        {
            if (dgList_Emp_With_Template.SelectedCells.Count > 0)
            {
                _daList_Emp.SelectCommand.Parameters["p_CARD_ID"].Value = ID_Card;
                _daList_Emp.SelectCommand.Parameters["p_WORKER_ID"].Value = Worker_ID;
                _ds.Tables["LIST_EMP_ROW"].Clear();
                _daList_Emp.Fill(_ds.Tables["LIST_EMP_ROW"]);
                if (_ds.Tables["LIST_EMP_ROW"].Rows.Count > 0)
                {
                    _ds.Tables["LIST_EMP"].PrimaryKey = new DataColumn[] { _ds.Tables["LIST_EMP"].Columns["ID_CARD"], _ds.Tables["LIST_EMP"].Columns["WORKER_ID"]};
                    _ds.Tables["LIST_EMP"].LoadDataRow(_ds.Tables["LIST_EMP_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                }
                else
                {
                    _ds.Tables["LIST_EMP"].PrimaryKey = new DataColumn[] { _ds.Tables["LIST_EMP"].Columns["ID_CARD"], _ds.Tables["LIST_EMP"].Columns["WORKER_ID"] };
                    _ds.Tables["LIST_EMP"].Rows.Remove(_ds.Tables["LIST_EMP"].Rows.Find(ID_Card));
                }
                _daList_Emp.SelectCommand.Parameters["p_CARD_ID"].Value = null;
                _daList_Emp.SelectCommand.Parameters["p_WORKER_ID"].Value = null;
            }
            dgList_Emp_With_Template.Items.Refresh();
            GetAccess_Templ_By_Emp();
        }

        private void EditAccess_Templ_By_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Access_Templ_By_Emp_Editor trans_pr = new Access_Templ_By_Emp_Editor((DataRowView)_currentItem);
            trans_pr.Owner = Window.GetWindow(this);            
            trans_pr.ShowDialog();
            ReloadEmp();
        }

        private void btRefreshState_Click(object sender, RoutedEventArgs e)
        {
            GetList_Emp_With_Template();
            btFilter_Clear_Click(null, null);
        }

        object _currentItem;
        private void dgList_Emp_With_Template_CurrentCellChanged(object sender, EventArgs e)
        {
            if (_currentItem != dgList_Emp_With_Template.CurrentItem && dgList_Emp_With_Template.CurrentItem != null)
            {
                _currentItem = dgList_Emp_With_Template.CurrentItem;
                Perco_Sync_ID = ((DataRowView)_currentItem)["PERCO_SYNC_ID"];
                Worker_ID = ((DataRowView)_currentItem)["WORKER_ID"];
                ID_Card = ((DataRowView)_currentItem)["ID_CARD"];
                GetAccess_Templ_By_Emp();
            }
        }

        private void dgList_Emp_With_Template_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
        }

        private void SetAccess_Template_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["LIST_EMP"].DefaultView.Count > 0)
                e.CanExecute = true;
        }

        private void SetAccess_Template_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Convert.ToInt16(_ds.Tables["LIST_EMP"].DefaultView.ToTable().Compute("COUNT(FL)", "FL=1")) == 0)
            { 
                MessageBox.Show("Вы не выбрали ни одной записи для корректировки!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            SetAccess_Template_Editor trans_pr = new SetAccess_Template_Editor(Convert.ToDecimal(cbSubdivNameFilter.SelectedValue));
            trans_pr.Owner = Window.GetWindow(this);
            if (trans_pr.ShowDialog() == true)
            {
                //_ocAccess_Emp_Update.Parameters["p_ID_SHABLON_MAIN"].Value = trans_pr.ID_Shablon_Main;
                //_ocAccess_Emp_Update.Parameters["p_START_DATE_VALID"].Value = trans_pr.Start_Date_Valid;
                //_ocAccess_Emp_Update.Parameters["p_END_DATE_VALID"].Value = trans_pr.End_Date_Valid;
                //_ocAccess_Emp_Update.Parameters["p_SIGN_TEMPORARY_SHABLON"].Value = trans_pr.Sign_Temporary_Shablon;
                //_ocAccess_Emp_Update.Parameters["p_ID_CARD_TABLE"].Value =
                //    _ds.Tables["LIST_EMP"].DefaultView.ToTable().Select("FL=1").Select(i => i.Field<Decimal>("ID_CARD")).ToArray();

                // 17.08.2016 - изначально планировал выполнить обновление одной командой.
                // Однако, так как нужно обновлять данные в Перко, а это можно сделать только по конкретному человеку,
                // то необходимо использовать цикл
                DataRow[] _dr = _ds.Tables["LIST_EMP"].DefaultView.ToTable().Select("FL=1");
                OracleTransaction transact;
                foreach (DataRow row in _dr)
                {
                    _daAccess_Emp.UpdateCommand.Parameters["PER_NUM"].Value = row["PER_NUM"];
                    _daAccess_Emp.UpdateCommand.Parameters["PERCO_SYNC_ID"].Value = row["PERCO_SYNC_ID"];
                    _daAccess_Emp.UpdateCommand.Parameters["ID_CARD"].Value = row["ID_CARD"];
                    _daAccess_Emp.UpdateCommand.Parameters["ID_SHABLON_MAIN"].Value = trans_pr.ID_Shablon_Main;
                    _daAccess_Emp.UpdateCommand.Parameters["START_DATE_VALID"].Value = trans_pr.Start_Date_Valid;
                    _daAccess_Emp.UpdateCommand.Parameters["END_DATE_VALID"].Value = trans_pr.End_Date_Valid;
                    _daAccess_Emp.UpdateCommand.Parameters["SIGN_TEMPORARY_SHABLON"].Value = trans_pr.Sign_Temporary_Shablon;
                    transact = Connect.CurConnect.BeginTransaction();
                    _daAccess_Emp.UpdateCommand.Transaction = transact;
                    try
                    {
                        _daAccess_Emp.UpdateCommand.ExecuteNonQuery();
                        // Если дата начала действия шаблона меньше текущей даты, то работаем дальше.                        
                        if (trans_pr.Start_Date_Valid < DateTime.Today)
                        {
                            // Если дата окончания доступа пустая или она позже текущей даты, то нужно обновить данные в перко
                            if (trans_pr.End_Date_Valid == null || (trans_pr.End_Date_Valid != null && trans_pr.End_Date_Valid >= DateTime.Today))
                            {
                                if (DictionaryPerco.employees.UpdateAccessEmployee(new PercoXML.Employee(row["PERCO_SYNC_ID"].ToString(), row["PER_NUM"].ToString(),
                                    row["ID_CARD"].ToString(), trans_pr.ID_Shablon_Main.ToString())) == true)
                                {
                                    transact.Commit();
                                }
                                else
                                {
                                    transact.Rollback();
                                }
                            }
                            // Иначе просто сохраняем данные в Кадрах, а шаблон доступа в перке не трогаем
                            else
                            {
                                transact.Commit();
                            }
                        }
                        // Иначе просто сохраняем данные в Кадрах, а шаблон доступа в перке не трогаем
                        else
                        {
                            transact.Commit();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка обновления шаблона доступа в АСУ \"Кадры\" по сотруднику:\n"+
                            string.Format("таб.№ {0} - {1} {2} {3}", row["PER_NUM"], row["EMP_LAST_NAME"], row["EMP_FIRST_NAME"], row["EMP_MIDDLE_NAME"]),
                            "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                        transact.Rollback();
                        break;
                    }
                }
                MessageBox.Show("Данные сохранены!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
                if (AllItemsAreChecked == true)
                {
                    AllItemsAreChecked = false;
                }
                GetList_Emp_With_Template();
                GetAccess_Templ_By_Emp();
            }
            //ReloadEmp();
        }

        private void cbSubdivNameFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetList_Emp_With_Template();
        }
    }
}
