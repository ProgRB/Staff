using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;
using Salary;
using Staff;
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
using Tabel;

namespace KadrWPF.Table
{
    /// <summary>
    /// Логика взаимодействия для Worked_Day_Viewer.xaml
    /// </summary>
    public partial class Worked_Day_Viewer : Window
    {
        ABSENCE_seq absence;
        decimal _worked_day_id, _transfer_id, _worker_id;
        OracleDataTable dtWork_Pay_Type, dtPass_Event, _dtReg_doc;
        string _per_num;
        bool _sign_waybill;
        DateTime _work_date;   
        DataTable _dtEditable_Pay_Type, _dtPassWithDoc, _dtDoc_List;
        Table_Viewer _table_Viewer;
        OracleCommand ocUpdateOrder, ocDeleteAbs, ocReg_Doc_Registr, ocDeleteReg_Doc, ocDeleteWPT, ocSignDeleteWPT, ocAbs_Calc_On_Doc;

        public Worked_Day_Viewer(Table_Viewer table_Viewer, string per_num,
            decimal worked_day_id, decimal transfer_id, DateTime date, 
            decimal worker_id, bool sign_waybill, OracleDataTable dtReg_doc)
        {
            _table_Viewer = table_Viewer;
            _worked_day_id = worked_day_id;
            _transfer_id = transfer_id;
            _per_num = per_num;
            _work_date = date;
            _worker_id = worker_id;
            _sign_waybill = sign_waybill;
            _dtReg_doc = dtReg_doc;
            InitializeComponent();
            this.Title = "Отработанные часы по видам оплат на " + date.ToShortDateString();
            dtWork_Pay_Type = new OracleDataTable("", Connect.CurConnect);
            dtWork_Pay_Type.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Work_Pay_Type_For_Day.sql"),
                Connect.Schema);
            dtWork_Pay_Type.SelectCommand.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value =
                worked_day_id;
            GetWork_Pay_Type();

            dtPass_Event = new OracleDataTable("", Connect.CurConnect);
            dtPass_Event.SelectCommand.CommandText =
                string.Format(Queries.GetQuery("Table/SelectEmp_Pass_Event.sql"), "perco");
            dtPass_Event.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
            dtPass_Event.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = date;
            dtPass_Event.Clear();
            dtPass_Event.Fill();
            dgPass_Event.DataContext = dtPass_Event; 

            ///// Если в табеле не текущий месяц или не первое число следующего месяца,
            ///// запрещаем работу с видами оплат и кнопками
            //if (!((DateTime.Now.Day == 1 && table_Viewer.EndDate.AddDays(1) == DateTime.Now.Date)
            //    || (DateTime.Now.Month == date.Month && DateTime.Now.Year == date.Year)))
            //{
            //    tsWorkTime.IsEnabled = false;
            //    pnPassWithDoc.IsEnabled = false;
            //}
            OracleDataTable dtTimeGraph = new OracleDataTable("", Connect.CurConnect);
            dtTimeGraph.SelectCommand.CommandText = string.Format(
                "SELECT NVL(ROUND(WD.FROM_GRAPH/3600,2),0) FROM {0}.WORKED_DAY WD " +
                "WHERE WD.WORKED_DAY_ID = :P_WORKED_DAY_ID", Connect.Schema);
            dtTimeGraph.SelectCommand.Parameters.Add("P_WORKED_DAY_ID", worked_day_id);
            dtTimeGraph.Fill();
            if (dtTimeGraph.Rows.Count > 0)
            {
                lbTimeGraph.Text = "Необходимое время присутствия по графику = " +
                    dtTimeGraph.Rows[0][0].ToString() +
                    " ч.";
            }

            ocUpdateOrder = new OracleCommand("", Connect.CurConnect);
            ocUpdateOrder.BindByName = true;
            ocUpdateOrder.CommandText = string.Format(
                "UPDATE {0}.WORK_PAY_TYPE set ORDER_ID = :p_order_id " +
                "where WORK_PAY_TYPE_ID = :p_work_pay_type_id", Connect.Schema);
            ocUpdateOrder.Parameters.Add("p_order_id", OracleDbType.Decimal);
            ocUpdateOrder.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal);

            ocDeleteAbs = new OracleCommand("", Connect.CurConnect);
            ocDeleteAbs.BindByName = true;
            ocDeleteAbs.CommandText = string.Format(
                "delete from {0}.ABSENCE where ABSENCE_ID = :p_absence_id", Connect.Schema);
            ocDeleteAbs.Parameters.Add("p_absence_id", OracleDbType.Decimal);

            ocReg_Doc_Registr = new OracleCommand("", Connect.CurConnect);
            ocReg_Doc_Registr.CommandText = string.Format(
                "begin {0}.REG_DOC_REGISTR(:p_reg_doc_id, :p_sign); end;", Connect.Schema);
            ocReg_Doc_Registr.BindByName = true;
            ocReg_Doc_Registr.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal);
            ocReg_Doc_Registr.Parameters.Add("p_sign", OracleDbType.Decimal);

            ocDeleteReg_Doc = new OracleCommand(string.Format(
                "BEGIN {0}.REG_DOC_delete(:p_reg_doc_id); END;",
                Connect.Schema), Connect.CurConnect);
            ocDeleteReg_Doc.BindByName = true;
            ocDeleteReg_Doc.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal);

            ocDeleteWPT = new OracleCommand("", Connect.CurConnect);
            ocDeleteWPT.BindByName = true;
            ocDeleteWPT.CommandText = string.Format(
                "delete from {0}.WORK_PAY_TYPE where WORK_PAY_TYPE_ID = :p_work_pay_type_id",
                Connect.Schema);
            ocDeleteWPT.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal);

            ocSignDeleteWPT = new OracleCommand("", Connect.CurConnect);
            ocSignDeleteWPT.BindByName = true;
            ocSignDeleteWPT.CommandText = string.Format(Queries.GetQuery("Table/SignDeleteWPT.sql"),
                Connect.Schema);
            ocSignDeleteWPT.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal);

            absence = new ABSENCE_seq(Connect.CurConnect);

            /*Если сотрудник не работает по путевым листам, то высчитываем Отсутсвие сотрудника в рабочее время.
             Если сотрудник работает по путевым, показываем путевые за текущую дату.*/
            if (_sign_waybill == false)
            {
                _dtDoc_List = new DataTable();
                OracleDataAdapter _daDoc_List = new OracleDataAdapter(string.Format(
                    @"select -1 as DOC_LIST_ID, ' ' as DOC_NAME, 0 as PAY_TYPE_ID from dual 
                    union 
                    select DOC_LIST_ID, DOC_NAME, PAY_TYPE_ID from {0}.DOC_LIST 
                    where SIGN_ALL_DAY = 0 and DOC_TYPE = 1 and :p_DATE between DOC_BEGIN_VALID and DOC_END_VALID
                    order by DOC_NAME", Connect.Schema),
                    Connect.CurConnect);
                _daDoc_List.SelectCommand.Parameters.Add("p_DATE", OracleDbType.Date).Value = date;
                _daDoc_List.Fill(_dtDoc_List);                
                dcDOC_LIST_ID.ItemsSource = _dtDoc_List.DefaultView;

                GetPassWithDoc();
                gbWay_Bill.Visibility = Visibility.Collapsed;
            }
            else
            {
                GetPassWithDoc();
                gbPassOrPutl.Visibility = Visibility.Collapsed;
                pnPassWithDoc.Visibility = Visibility.Collapsed;
            }

            dgReg_Doc.DataContext = _dtReg_doc.DefaultView;

            ocAbs_Calc_On_Doc = new OracleCommand(string.Format("select {0}.ABS_CALC_ON_DOC(:p_doc_begin,:p_doc_end) from dual",
                Connect.Schema), Connect.CurConnect);
            ocAbs_Calc_On_Doc.BindByName = true;
            ocAbs_Calc_On_Doc.Parameters.Add("p_doc_begin", OracleDbType.Date);
            ocAbs_Calc_On_Doc.Parameters.Add("p_doc_end", OracleDbType.Date);

            OracleDataAdapter _daPay_Type = new OracleDataAdapter(string.Format(
                @"BEGIN
                    {0}.GET_EDITABLE_PAY_TYPE(:c1);
                END;", Connect.SchemaApstaff), Connect.CurConnect);
            _daPay_Type.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            _dtEditable_Pay_Type = new DataTable();
            _daPay_Type.Fill(_dtEditable_Pay_Type);
        }

        void GetWork_Pay_Type()
        {
            dgWork_Pay_Type.DataContext = null;
            dtWork_Pay_Type.Clear();
            dtWork_Pay_Type.Fill();
            dgWork_Pay_Type.DataContext = dtWork_Pay_Type.DefaultView;
        }

        void GetReg_Doc()
        {
            dgReg_Doc.DataContext = null;
            _table_Viewer.GetReg_Doc();
            dgReg_Doc.DataContext = _dtReg_doc.DefaultView;
            GetWork_Pay_Type();
            GetPassWithDoc();
        }
        
        /// <summary>
        /// Обновление таблицы просмотра проходов проходной
        /// </summary>
        void GetPassWithDoc()
        {
            _dtPassWithDoc = new DataTable();
            if (_sign_waybill == false)
            {
                OracleDataAdapter adap =
                    new OracleDataAdapter(
                    string.Format(Queries.GetQuery("Table/SelectPassWithDoc.sql"),
                    Connect.Schema, "perco"),
                    Connect.CurConnect);
                adap.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
                adap.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = _work_date;
                adap.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                adap.Fill(_dtPassWithDoc);
                dgPassWithDoc.DataContext = _dtPassWithDoc.DefaultView;
            }
            else
            {
                OracleDataAdapter adap = new OracleDataAdapter("", Connect.CurConnect);
                adap.SelectCommand.BindByName = true;
                if (_work_date < new DateTime(2014, 4, 1))
                {
                    adap.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWayBill_Old.sql"),
                        Connect.Schema);
                }
                else
                {
                    adap.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWayBill.sql"),
                        Connect.Schema);
                }
                adap.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
                adap.SelectCommand.Parameters.Add("p_date_work", OracleDbType.Date).Value = _work_date;
                adap.Fill(_dtPassWithDoc);
                dgWay_Bill.DataContext = _dtPassWithDoc.DefaultView;
            }
        }

        private void AddReg_Doc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddReg_Doc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Table.EditRegDoc editReg_doc =
                new WpfControlLibrary.Table.EditRegDoc(_transfer_id, null, _sign_waybill);
            editReg_doc.Owner = Window.GetWindow(this);
            editReg_doc.Title = "Добавление документа на " + _work_date.ToShortDateString();
            if (editReg_doc.ShowDialog() == true)
            {
                GetReg_Doc();
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
                new WpfControlLibrary.Table.EditRegDoc(_transfer_id,
                Convert.ToDecimal(((DataRowView)dgReg_Doc.SelectedCells[0].Item)["reg_doc_id"]), 
                _sign_waybill);
            editReg_doc.Owner = Window.GetWindow(this);
            editReg_doc.Title = "Редактирование документа на " + _work_date.ToShortDateString();
            if (editReg_doc.ShowDialog() == true)
            {
                GetReg_Doc();
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgPassWithDoc_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
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
                    GetReg_Doc();
                }
            }
        }
        
        private void AddWork_Pay_Type_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && 
                (!_table_Viewer.flagClosedTable || GrantedRoles.GetGrantedRole("TABLE_EDIT_OLD")))
                e.CanExecute = true;
        }
        private void AddWork_Pay_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditWork_Pay_Type working_work_pay_type = new EditWork_Pay_Type(true, 0,
                _worked_day_id, _transfer_id, _dtEditable_Pay_Type, 0, 0);
            working_work_pay_type.Text = "Добавление данных об отработке по видам оплат";
            working_work_pay_type.ShowDialog();
            GetWork_Pay_Type();
        }

        private void EditWork_Pay_Type_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && 
                (!_table_Viewer.flagClosedTable || GrantedRoles.GetGrantedRole("TABLE_EDIT_OLD"))
                && dgWork_Pay_Type != null && dgWork_Pay_Type.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditWork_Pay_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int pay_type_id = Convert.ToInt32(((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["pay_type_id"]);
            if (pay_type_id == 101 || pay_type_id == 102)
            {
                /*OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(
                    "select count(*) from user_role_privs where granted_role = 'TABLE_EDIT_PT'");
                int p_count = Convert.ToInt32(com.ExecuteScalar());
                if (p_count == 0)*/
                if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                {
                    MessageBox.Show("Недостаточно прав для редактирования данного вида оплат!" +
                        "\nПо вопросам обращаться в группу табельного учета.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            if (_dtEditable_Pay_Type.Select("PAY_TYPE_ID = " + pay_type_id.ToString()).Count() > 0 || 
                (pay_type_id == 112 && GrantedRoles.GetGrantedRole("TABLE_EDIT_PT")))
            {
                int work_pay_type_id = Convert.ToInt32(((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["work_pay_type_id"]);
                EditWork_Pay_Type working_work_pay_type = new EditWork_Pay_Type(false, work_pay_type_id,
                    _worked_day_id, _transfer_id, _dtEditable_Pay_Type, pay_type_id, Convert.ToInt32(((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["VTIME"]));
                working_work_pay_type.Text = "Редактирование данных об отработке по видам оплат";
                working_work_pay_type.ShowDialog();
                GetWork_Pay_Type();
            }
            else
            {
                MessageBox.Show("В данном окне можно редактировать только 110, 111, 114, 211, 510 виды оплат!\n" +
                    "Для редактирования времени по прочим видам оплат нужно изменить оправдательный документ.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DeleteWork_Pay_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ocSignDeleteWPT.Parameters["p_work_pay_type_id"].Value =
                    ((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["work_pay_type_id"];
                if (ocSignDeleteWPT.ExecuteReader().Read())
                {
                    ocDeleteWPT.Parameters["p_work_pay_type_id"].Value =
                        ((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["work_pay_type_id"];
                    ocDeleteWPT.ExecuteNonQuery();
                    Connect.Commit();
                    GetWork_Pay_Type();
                }
                else
                {
                    MessageBox.Show("Данная запись связана с оправдательным документом!\n" +
                        "Для удаления нужно удалить или отредактировать оправдательный документ.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void TimePercoToTimeGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleDataAdapter odaAccess = new OracleDataAdapter();
            odaAccess.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery(
                "Table/SelectAccessButtonTimePass.sql"), Connect.Schema), Connect.CurConnect);
            odaAccess.SelectCommand.BindByName = true;
            odaAccess.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
            odaAccess.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = _work_date;
            DataTable dtAccess = new DataTable();
            odaAccess.Fill(dtAccess);
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") || GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                Convert.ToInt32(dtAccess.Rows[0]["SIGN_ACCESS"]) > 0)
            {
                if (MessageBox.Show("Вы действительно хотите принять время по проходам?" +
                    "\nВ этом случае время по графику будет перезаписано.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    OracleCommand _ocUpdate = new OracleCommand(string.Format(
                        @"BEGIN
                            {0}.WORKED_DAY_PERCO_TO_GRAPH(:p_WORKED_DAY_ID);
                        END;", Connect.SchemaApstaff), Connect.CurConnect);
                    _ocUpdate.BindByName = true;
                    _ocUpdate.Parameters.Add("p_WORKED_DAY_ID", OracleDbType.Decimal).Value = _worked_day_id;
                    OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        _ocUpdate.Transaction = transact;
                        _ocUpdate.ExecuteNonQuery();
                        transact.Commit();
                        Close();
                    }
                    catch(Exception ex)
                    {
                        transact.Rollback();
                        MessageBox.Show("Ошибка обновления времени по графику!\n" + ex.Message, "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    /*OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                    ocUpdate.BindByName = true;
                    ocUpdate.CommandText = string.Format(
                        "UPDATE {0}.WORKED_DAY W SET W.FROM_GRAPH = W.FROM_PERCO " +
                        "WHERE W.WORKED_DAY_ID = :p_worked_day_id", Connect.Schema);
                    ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = _worked_day_id;
                    ocUpdate.ExecuteNonQuery();
                    if (dtWork_Pay_Type.Rows.Count > 0)
                    {
                        int work_pay_type_id =
                            Convert.ToInt32(((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["work_pay_type_id"]);
                        string pay_type_id = ((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["pay_type_id"].ToString();
                        if (pay_type_id == "101" || pay_type_id == "102")
                        {
                            ocUpdate = new OracleCommand("", Connect.CurConnect);
                            ocUpdate.BindByName = true;
                            ocUpdate.CommandText = string.Format(
                                "update {0}.WORK_PAY_TYPE WP " +
                                "set WP.VALID_TIME = " +
                                    "(select W.FROM_PERCO from {0}.WORKED_DAY W " +
                                    "where W.WORKED_DAY_ID = :p_worked_day_id) " +
                                "where WP.WORK_PAY_TYPE_ID = :p_work_pay_type_id", Connect.Schema);
                            ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = _worked_day_id;
                            ocUpdate.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal).Value =
                                work_pay_type_id;
                            ocUpdate.ExecuteNonQuery();
                        }
                    }
                    Connect.Commit();
                    Close();*/
                }
            }
            else
            {
                MessageBox.Show("У вас не хватает полномочий для данной операции!" +
                    "\nОбратитесь в группу табельного учета.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        
        private void TimePayTypeToTimePerco_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите принять время по текущей записи в видах оплат?" +
                "\nВ этом случае время по проходам будет перезаписано.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                ocUpdate.BindByName = true;
                ocUpdate.CommandText = string.Format(
                    "update {0}.WORKED_DAY W " +
                    "set W.FROM_PERCO = " +
                        "nvl((select WP.VALID_TIME from {0}.WORK_PAY_TYPE WP " +
                        "where WP.WORK_PAY_TYPE_ID = :p_work_pay_type_id),0) " +
                        "where W.WORKED_DAY_ID = :p_worked_day_id", Connect.Schema);
                ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = _worked_day_id;
                ocUpdate.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal).Value =
                    ((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["work_pay_type_id"];
                ocUpdate.ExecuteNonQuery();
                Connect.Commit();
                Close();
            }
        }

        private void TimeSumPayTypeToTimePerco_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите принять общее время по видам оплат?" +
                "\nВ этом случае время по проходам будет перезаписано.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                ocUpdate.BindByName = true;
                ocUpdate.CommandText = string.Format(
                    "update {0}.WORKED_DAY W " +
                    "set W.FROM_PERCO = " +
                    "   nvl((select nvl(sum(WP.VALID_TIME),0) as time " +
                    "       from {0}.WORK_PAY_TYPE WP " +
                    "        where WP.WORKED_DAY_ID = W.WORKED_DAY_ID and " +
                    "            WP.PAY_TYPE_ID not in (select PT.PAY_TYPE_ID from {0}.PAY_TYPE PT " +
                    "                                   where PT.SIGN_ADDITION = 1) and " +
                    "            (WP.REG_DOC_ID is null or " +
                    "            (WP.REG_DOC_ID is not null and exists( " +
                    "                select null from {0}.REG_DOC R " +
                    "                join {0}.DOC_LIST D on D.DOC_LIST_ID = R.DOC_LIST_ID " +
                    "                where R.REG_DOC_ID = WP.REG_DOC_ID and D.DOC_TYPE = 2)))),0) " +
                    "where W.WORKED_DAY_ID = :p_worked_day_id", Connect.Schema);
                ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = _worked_day_id;
                ocUpdate.ExecuteNonQuery();
                Connect.Commit();
                Close();
            }
        }

        private void CalcWorked_Day_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите пересчитать время за выбранный день?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(
                    "begin {0}.TABLE_UPDATEFORDATE_new(:p_month, :p_year, :p_per_num, :p_transfer_id, :p_date); end;",
                    Connect.Schema);
                com.Parameters.Add("p_month", OracleDbType.Decimal).Value = _work_date.Month;
                com.Parameters.Add("p_year", OracleDbType.Decimal).Value = _work_date.Year;
                com.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
                com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                com.Parameters.Add("p_date", OracleDbType.Date).Value = _work_date;
                com.ExecuteNonQuery();
                Connect.Commit();
                Close();
            }
        }

        private void EditOrderPayType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_work_date > Table_Viewer.dCloseTable)
            {
                EditOrder editOrder = new EditOrder(false);
                editOrder.ShowInTaskbar = false;
                editOrder.ShowDialog();
                if (editOrder.Order_ID_Property != -1)
                {
                    ocUpdateOrder.Parameters["p_order_id"].Value = editOrder.Order_ID_Property;
                    ocUpdateOrder.Parameters["p_work_pay_type_id"].Value =
                        ((DataRowView)dgWork_Pay_Type.SelectedCells[0].Item)["WORK_PAY_TYPE_ID"];
                    ocUpdateOrder.ExecuteNonQuery();
                    Connect.Commit();
                    dtWork_Pay_Type.Clear();
                    dtWork_Pay_Type.Fill();
                }
            }
            else
            {
                MessageBox.Show("Нельзя редактировать заказ за прошедший период!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveDocWorked_Day_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataTable _dtChanges = _dtPassWithDoc.GetChanges();
            if (_dtChanges != null)
            {
                for (int i = 0; i < _dtChanges.Rows.Count; ++i)
                {
                    /// Если не пустой тип документа и пустой документ, то добавляем документ выбранного типа
                    if ((_dtChanges.Rows[i]["DOC_LIST_ID"] != DBNull.Value ||
                        Convert.ToInt32(_dtChanges.Rows[i]["DOC_LIST_ID"]) != -1) &&
                        _dtChanges.Rows[i]["REG_DOC_ID"] == DBNull.Value)
                    {
                        int pay_type_id = 0;
                        for (int g = 0; g < _dtDoc_List.Rows.Count; g++)
                        {
                            if (_dtChanges.Rows[i]["DOC_LIST_ID"].ToString() ==
                                _dtDoc_List.Rows[g]["doc_list_id"].ToString())
                            {
                                pay_type_id = Convert.ToInt32(_dtDoc_List.Rows[g]["pay_type_id"]);
                                break;
                            }
                        }
                        if (pay_type_id == 535 && _dtChanges.Rows[i]["DOC_LOCATION"].ToString() == "")
                        {
                            MessageBox.Show("Для оправдательного документа Работа за территорией предприятия " +
                                "\nнеобходимо заполнить поле Местонахождение!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        else
                        {
                            if (pay_type_id != 535)
                            {
                                _dtChanges.Rows[i]["DOC_LOCATION"] = "";
                            }
                        }
                        WpfControlLibrary.Table.RegDocModel model = new WpfControlLibrary.Table.RegDocModel(null, _transfer_id, _sign_waybill);                           
                        model.PerNum = _per_num;
                        model.DocListID = Convert.ToDecimal(_dtChanges.Rows[i]["DOC_LIST_ID"]);
                        model.DocDate = DateTime.Now;
                        model.DocBegin = _dtChanges.Rows[i]["From_Plant"] != DBNull.Value ?
                            Convert.ToDateTime(_dtChanges.Rows[i]["From_Plant"]) :
                            Convert.ToDateTime(_dtChanges.Rows[i]["TIME_BEGIN"]);
                        model.DocEnd = _dtChanges.Rows[i]["Into_Plant"] != DBNull.Value ?
                            Convert.ToDateTime(_dtChanges.Rows[i]["Into_Plant"]) :
                            Convert.ToDateTime(_dtChanges.Rows[i]["TIME_END"]);
                        model.DocLocation = _dtChanges.Rows[i]["DOC_LOCATION"].ToString();
                        model.Save();
                        //REG_DOC_seq reg_doc = new REG_DOC_seq(Connect.CurConnect);
                        //reg_doc.AddNew();
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM = per_num;
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).TRANSFER_ID = transfer_id;
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID =
                        //    Convert.ToDecimal(_dtChanges.Rows[i]["DOC_LIST_ID"]);
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_DATE = DateTime.Now;
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN =
                        //    _dtChanges.Tables[0].Rows[i]["From_Plant"] != DBNull.Value ?
                        //    Convert.ToDateTime(_dtChanges.Rows[i]["From_Plant"]) :
                        //    Convert.ToDateTime(_dtChanges.Rows[i]["TIME_BEGIN"]);
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END =
                        //    _dtChanges.Tables[0].Rows[i]["Into_Plant"] != DBNull.Value ?
                        //    Convert.ToDateTime(_dtChanges.Rows[i]["Into_Plant"]) :
                        //    Convert.ToDateTime(_dtChanges.Rows[i]["TIME_END"]);
                        //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LOCATION =
                        //        _dtChanges.Rows[i]["DOC_LOCATION"].ToString();

                        //if (pay_type_id == 302)
                        //{
                        //    decimal timeDoc = 0;
                        //    ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                        //    ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                        //    decimal hoursDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());
                        //    if (Table_Viewer.HoursAbsence - hoursDoc + timeDoc < 0 &&//-10 &&
                        //        !GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                        //    {
                        //        decimal hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence));
                        //        decimal min = Table_Viewer.HoursAbsence - Math.Truncate(Table_Viewer.HoursAbsence);
                        //        string hoursAbs = string.Format("{0}{1}:{2}", Table_Viewer.HoursAbsence < 0 ? "-" : "", hours,
                        //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        //        hours = Math.Truncate(Math.Abs(hoursDoc));
                        //        min = hoursDoc - Math.Truncate(hoursDoc);
                        //        string sthoursDoc = string.Format("{0}{1}:{2}", hoursDoc < 0 ? "-" : "", hours,
                        //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        //        hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence - hoursDoc + timeDoc));
                        //        min = (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) - Math.Truncate(Table_Viewer.HoursAbsence - hoursDoc + timeDoc);
                        //        string sthours = string.Format("{0}{1}:{2}", (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) < 0 ? "-" : "", hours,
                        //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        //        MessageBox.Show("Недостаточно доступных часов в отгулах (перерасход не более 0 часов)!\n\n" +
                        //            "Количество доступных часов отгулов = " + hoursAbs + "\n" +
                        //            "Количество часов по вносимому документу = " + sthoursDoc + "\n\n" +
                        //            "Документ не может быть сохранен, так как доступное время в отгулах будет = " + sthours,
                        //            "АРМ \"Учет рабочего времени\"",
                        //            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        //        return;
                        //    }
                        //}
                        //reg_doc.Save();
                        //Connect.Commit();

                        //ocReg_Doc_Registr.Parameters["p_reg_doc_id"].Value =
                        //    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID;
                        //ocReg_Doc_Registr.ExecuteNonQuery();
                        //Connect.Commit();
                        //RegistrAbsence(pay_type_id, reg_doc);
                    }
                    else
                    {
                        /// Если не пустой тип документа и не пустой документ, то редактируем тип документа в
                        /// существующем документе
                        if (_dtChanges.Rows[i]["DOC_LIST_ID"] != DBNull.Value &&
                            Convert.ToInt32(_dtChanges.Rows[i]["DOC_LIST_ID"]) != -1 &&
                            _dtChanges.Rows[i]["REG_DOC_ID"] != DBNull.Value)
                        {
                            int pay_type_id = 0;
                            for (int g = 0; g < _dtDoc_List.Rows.Count; g++)
                            {
                                if (_dtChanges.Rows[i]["DOC_LIST_ID"].ToString() ==
                                    _dtDoc_List.Rows[g]["doc_list_id"].ToString())
                                {
                                    pay_type_id = Convert.ToInt32(_dtDoc_List.Rows[g]["pay_type_id"]);
                                    break;
                                }
                            }
                            if (pay_type_id == 535 && _dtChanges.Rows[i]["DOC_LOCATION"].ToString() == "")
                            {
                                MessageBox.Show("Для оправдательного документа Работа за территорией предприятия " +
                                    "\nнеобходимо заполнить поле Местонахождение!",
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                return;
                            }
                            else
                            {
                                if (pay_type_id != 535)
                                {
                                    _dtChanges.Rows[i]["DOC_LOCATION"] = "";
                                }
                            }
                            WpfControlLibrary.Table.RegDocModel model = new WpfControlLibrary.Table.RegDocModel(Convert.ToDecimal(_dtChanges.Rows[i]["REG_DOC_ID"]), _transfer_id, _sign_waybill);
                            model.PerNum = _per_num;
                            model.DocListID = Convert.ToDecimal(_dtChanges.Rows[i]["DOC_LIST_ID"]);
                            model.DocDate = DateTime.Now;
                            model.DocBegin = _dtChanges.Rows[i]["From_Plant"] != DBNull.Value ?
                                Convert.ToDateTime(_dtChanges.Rows[i]["From_Plant"]) :
                                Convert.ToDateTime(_dtChanges.Rows[i]["TIME_BEGIN"]);
                            model.DocEnd = _dtChanges.Rows[i]["Into_Plant"] != DBNull.Value ?
                                Convert.ToDateTime(_dtChanges.Rows[i]["Into_Plant"]) :
                                Convert.ToDateTime(_dtChanges.Rows[i]["TIME_END"]);
                            model.DocLocation = _dtChanges.Rows[i]["DOC_LOCATION"].ToString();
                            Exception ex =  model.Save();
                            if (ex != null)
                                MessageBox.Show(ex.GetFormattedException(), "Ошибка сохранения данных");
                            //REG_DOC_seq reg_doc = new REG_DOC_seq(Connect.CurConnect);
                            //reg_doc.Fill("where REG_DOC_ID = " + _dtChanges.Tables[0].Rows[i]["REG_DOC_ID"].ToString());
                            //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID =
                            //    Convert.ToDecimal(_dtChanges.Tables[0].Rows[i]["DOC_LIST_ID"]);
                            //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_DATE = DateTime.Now;
                            //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN =
                            //    _dtChanges.Tables[0].Rows[i]["From_Plant"] != DBNull.Value ?
                            //    Convert.ToDateTime(_dtChanges.Tables[0].Rows[i]["From_Plant"]) :
                            //    Convert.ToDateTime(_dtChanges.Tables[0].Rows[i]["TIME_BEGIN"]);
                            //string str = dgPassWithDoc.CurrentRow.Cells["FROM_PLANT"].Value.ToString();
                            //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END =
                            //    _dtChanges.Tables[0].Rows[i]["Into_Plant"] != DBNull.Value ?
                            //    Convert.ToDateTime(_dtChanges.Tables[0].Rows[i]["Into_Plant"]) :
                            //    Convert.ToDateTime(_dtChanges.Tables[0].Rows[i]["TIME_END"]);
                            //((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LOCATION =
                            //    _dtChanges.Tables[0].Rows[i]["DOC_LOCATION"].ToString();

                            //if (pay_type_id == 302)
                            //{
                            //    decimal timeDoc = 0;
                            //    ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                            //    ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                            //    decimal hoursDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());
                            //    if (Table_Viewer.HoursAbsence - hoursDoc + timeDoc < -10 &&
                            //        !GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                            //    {
                            //        decimal hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence));
                            //        decimal min = Table_Viewer.HoursAbsence - Math.Truncate(Table_Viewer.HoursAbsence);
                            //        string hoursAbs = string.Format("{0}{1}:{2}", Table_Viewer.HoursAbsence < 0 ? "-" : "", hours,
                            //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                            //        hours = Math.Truncate(Math.Abs(hoursDoc));
                            //        min = hoursDoc - Math.Truncate(hoursDoc);
                            //        string sthoursDoc = string.Format("{0}{1}:{2}", hoursDoc < 0 ? "-" : "", hours,
                            //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                            //        hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence - hoursDoc + timeDoc));
                            //        min = (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) - Math.Truncate(Table_Viewer.HoursAbsence - hoursDoc + timeDoc);
                            //        string sthours = string.Format("{0}{1}:{2}", (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) < 0 ? "-" : "", hours,
                            //            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                            //        MessageBox.Show("Недостаточно доступных часов в отгулах (перерасход не более 10 часов)!\n\n" +
                            //            "Количество доступных часов отгулов = " + hoursAbs + "\n" +
                            //            "Количество часов по вносимому документу = " + sthoursDoc + "\n\n" +
                            //            "Документ не может быть сохранен, так как доступное время в отгулах будет = " + sthours,
                            //            "АРМ \"Учет рабочего времени\"",
                            //            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            //        return;
                            //    }
                            //}

                            //reg_doc.Save();
                            //Connect.Commit();

                            //ocReg_Doc_Registr.Parameters["p_reg_doc_id"].Value =
                            //    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID;
                            //ocReg_Doc_Registr.ExecuteNonQuery();
                            //Connect.Commit();
                            //RegistrAbsence(pay_type_id, reg_doc);
                        }
                        else
                        {
                            /// Если пустой тип документа и не пустой документ, удаляем документ
                            if (Convert.ToInt32(_dtChanges.Rows[i]["DOC_LIST_ID"]) == -1 &&
                                _dtChanges.Rows[i]["REG_DOC_ID"] != DBNull.Value)
                            {
                                ocDeleteReg_Doc.Parameters["p_reg_doc_id"].Value = _dtChanges.Rows[i]["REG_DOC_ID"];
                                ocDeleteReg_Doc.ExecuteNonQuery();
                                Connect.Commit();
                            }
                        }
                    }
                }
                GetWork_Pay_Type();
                GetPassWithDoc();
                GetReg_Doc();
            }
        }        
    }
}
