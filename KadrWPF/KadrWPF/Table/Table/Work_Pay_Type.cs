using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;
using Kadr;
using System.Windows.Interop;
using Salary;
using KadrWPF.Table;

namespace Tabel
{
    public partial class Work_Pay_Type : Form
    {        
        ABSENCE_seq absence;     
        decimal worked_day_id;
        decimal transfer_id, degree_id, _worker_id;
        OracleDataTable dtWork_Pay_Type, dtPass_Event, dtReg_doc;
        string per_num;
        DateTime date;
        DataSet dsPassWIthDoc, dsDoc_List;
        Table_Viewer formTable;
        DataTable _dtEditable_Pay_Type;
        OracleCommand ocUpdateOrder, ocDeleteAbs, ocReg_Doc_Registr, ocDeleteReg_Doc, ocDeleteWPT, ocSignDeleteWPT, ocAbs_Calc_On_Doc;
        public Work_Pay_Type(string _per_num, 
            decimal _worked_day_id, decimal _transfer_id, DateTime _date, Table_Viewer _table,
             decimal _degree_id, decimal worker_id)
        {
            InitializeComponent();            
            worked_day_id = _worked_day_id;
            transfer_id = _transfer_id;
            degree_id = _degree_id;
            per_num = _per_num;
            date = _date;
            _worker_id = worker_id;
            formTable = _table;
            this.Text = "Отработанные часы по видам оплат на " + date.ToShortDateString();
            dtWork_Pay_Type = new OracleDataTable("", Connect.CurConnect);
            dtWork_Pay_Type.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Work_Pay_Type_For_Day.sql"), 
                Connect.Schema);
            dtWork_Pay_Type.SelectCommand.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value =
                worked_day_id;            
            RefWorkPayType();
            dtPass_Event = new OracleDataTable("", Connect.CurConnect);
            dtPass_Event.SelectCommand.CommandText = 
                string.Format(Queries.GetQuery("Table/SelectEmp_Pass_Event.sql"), "perco");
            dtPass_Event.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
            dtPass_Event.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = date;            
            RefPassEvent();
            /// Если в табеле не текущий месяц или не первое число следующего месяца,
            /// запрещаем работу с видами оплат и кнопками
            if (!((DateTime.Now.Day == 1 && formTable.EndDate.AddDays(1) == DateTime.Now.Date) 
                || (DateTime.Now.Month == date.Month && DateTime.Now.Year == date.Year)))
            {
                tsWorkTime.Enabled = false;
            }
            tsWorkTime.EnableByRules();
            pnPassWithDoc.EnableByRules();
            pnWorkReg_Doc.EnableByRules();
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
            /*orders = new ORDERS_seq(Connect.CurConnect);
            orders.Fill("order by order_name");*/

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
            if (((DataRowView)((Table_Viewer)formTable).dgEmp.SelectedItem)["FL_WAYBILL"] == DBNull.Value)
            {
                OracleCommand command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                dsPassWIthDoc = new DataSet();
                RefPassWithDoc();

                dsDoc_List = new DataSet();
                OracleDataAdapter _daDoc_List = new OracleDataAdapter(string.Format(
                    @"select -1 as DOC_LIST_ID, ' ' as DOC_NAME, 0 as PAY_TYPE_ID from dual 
                    union 
                    select DOC_LIST_ID, DOC_NAME, PAY_TYPE_ID from {0}.DOC_LIST 
                    where SIGN_ALL_DAY = 0 and DOC_TYPE = 1 and :p_DATE between DOC_BEGIN_VALID and DOC_END_VALID
                    order by DOC_NAME", Connect.Schema),
                    Connect.CurConnect);
                _daDoc_List.SelectCommand.Parameters.Add("p_DATE", OracleDbType.Date).Value = date;
                _daDoc_List.Fill(dsDoc_List);

                DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
                c1.Name = "From_Plant";
                c1.HeaderText = "Начало";
                c1.ReadOnly = true;
                c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
                c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
                c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                c1.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
                dgPassWithDoc.Columns.Add(c1);
                dgPassWithDoc.Columns["From_Plant"].DataPropertyName = "From_Plant";

                DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                c2.Name = "Into_Plant";
                c2.HeaderText = "Окончание";
                c2.ReadOnly = true;
                c2.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
                c2.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
                c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                c2.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
                dgPassWithDoc.Columns.Add(c2);
                dgPassWithDoc.Columns["Into_Plant"].DataPropertyName = "Into_Plant";

                DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
                c3.Name = "DOC_NAME";
                c3.HeaderText = "Наименование документа";
                c3.AutoComplete = true;
                c3.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
                c3.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
                c3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c3.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                dgPassWithDoc.Columns.Add(c3);
                ((DataGridViewComboBoxColumn)dgPassWithDoc.Columns["DOC_NAME"]).DataSource = dsDoc_List.Tables[0];
                ((DataGridViewComboBoxColumn)dgPassWithDoc.Columns["DOC_NAME"]).ValueMember = "DOC_LIST_ID";
                ((DataGridViewComboBoxColumn)dgPassWithDoc.Columns["DOC_NAME"]).DisplayMember = "doc_name";
                dgPassWithDoc.Columns["DOC_NAME"].DataPropertyName = "DOC_LIST_ID";

                DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                c4.Name = "DOC_LOCATION";
                c4.HeaderText = "Местонахождение";
                c4.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
                c4.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
                c4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgPassWithDoc.Columns.Add(c4);
                dgPassWithDoc.Columns["DOC_LOCATION"].DataPropertyName = "DOC_LOCATION";
            }
            else
            {
                pnPassWithDoc.Visible = false;
                gbPassOrPutl.Text = "Путевые листы за текущий день";
                dsPassWIthDoc = new DataSet();
                dgPassWithDoc.ReadOnly = true;
                RefPassWithDoc();
            }
            dtReg_doc = new OracleDataTable("", Connect.CurConnect);
            dgReg_Doc.DataSource = dtReg_doc;
            dtReg_doc.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Reg_doc.sql"),
                Connect.Schema);
            dtReg_doc.SelectCommand.Parameters.Add("p_per_num", per_num);
            dtReg_doc.SelectCommand.Parameters.Add("p_worker_id", _worker_id);
            dtReg_doc.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);
            dtReg_doc.SelectCommand.Parameters["p_date"].Value = date;
            RefReg_Doc();
            if (formTable.flagClosedTable)
            {
                tsbAdd.Enabled = false;
                tsbEdit.Enabled = false;
                tsbDelete.Enabled = false;                
                tsbFromPerco.Enabled = false;
                tsbFromPT.Enabled = false;
                tsbFromSumTP.Enabled = false;                
                tsbEditOrder.Enabled = false;                
                tsbCalcTime.Enabled = false;
                btSaveDoc.Enabled = false;
                tsbAddReg_Doc.Enabled = false;
                tsbDeleteReg_Doc.Enabled = false;
                tsbAdd.Enabled = false;
            }

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

        void RefWorkPayType()
        {
            dtWork_Pay_Type.Clear();
            dtWork_Pay_Type.Fill();
            dgWork_Pay_Type.DataSource = dtWork_Pay_Type;
            dgWork_Pay_Type.Columns["PAY_TYPE_ID"].HeaderText = "Шифр";
            dgWork_Pay_Type.Columns["PAY_TYPE_ID"].Width = 50;
            dgWork_Pay_Type.Columns["pay_type_name"].HeaderText = "Наименование вида оплаты";
            dgWork_Pay_Type.Columns["valid_time"].HeaderText = "Часы";
            dgWork_Pay_Type.Columns["vFormat"].HeaderText = "Время";
            dgWork_Pay_Type.Columns["vFormat"].Width = 50;
            dgWork_Pay_Type.Columns["valid_time"].Width = 50;
            dgWork_Pay_Type.Columns["ORDER_NAME"].HeaderText = "Заказ";
            dgWork_Pay_Type.Columns["ORDER_NAME"].Width = 90;
            dgWork_Pay_Type.Columns["pay_type_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgWork_Pay_Type.Columns["work_pay_type_id"].Visible = false;
            dgWork_Pay_Type.Columns["worked_day_id"].Visible = false;
            dgWork_Pay_Type.Columns["REG_DOC_id"].Visible = false;
            dgWork_Pay_Type.Columns["VTIME"].Visible = false;
            foreach (DataGridViewColumn col in dgWork_Pay_Type.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        void RefPassEvent()
        {
            dtPass_Event.Clear();
            dtPass_Event.Fill();
            dgPass_Event.DataSource = dtPass_Event;
            dgPass_Event.Columns["event_time"].HeaderText = "Время";
            dgPass_Event.Columns["event_time"].Width = 110;
            dgPass_Event.Columns["DISPLAY_NAME"].HeaderText = "Уст-во";
            dgPass_Event.Columns["DISPLAY_NAME"].Width = 60;
            dgPass_Event.Columns["event"].HeaderText = "Тип прохода";
            dgPass_Event.Columns["event"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;            
            foreach (DataGridViewColumn col in dgPass_Event.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
         }

        /// <summary>
        /// Обновление списка оправдательных документов по табелю
        /// </summary>
        void RefReg_Doc()
        {
            dtReg_doc.Clear();
            dtReg_doc.Fill();
            dgReg_Doc.Columns["REG_DOC_ID"].Visible = false;
            dgReg_Doc.Columns["ABSENCE_ID"].Visible = false;
            dgReg_Doc.Columns["TRANSFER_ID"].Visible = false;
            dgReg_Doc.Columns["PAY_TYPE_ID"].Visible = false;
            dgReg_Doc.Columns[4].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            dgReg_Doc.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            foreach (DataGridViewColumn col in dgReg_Doc.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgReg_Doc.Columns[3].Width = 300;
            dgReg_Doc.Columns[3].HeaderText = "Наименование документа";
            dgReg_Doc.Columns[4].Width = 110;
            dgReg_Doc.Columns[4].HeaderText = "Дата начала";
            dgReg_Doc.Columns[5].Width = 110;
            dgReg_Doc.Columns[5].HeaderText = "Дата окончания";
            dgReg_Doc.Columns[6].Width = 95;
            dgReg_Doc.Columns[6].HeaderText = "Дата документа";
            dgReg_Doc.Columns[7].Width = 60;
            dgReg_Doc.Columns[7].HeaderText = "№ док.";
            dgReg_Doc.Columns[8].Width = 150;
            dgReg_Doc.Columns[8].HeaderText = "Местонахождение";
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            EditWork_Pay_Type working_work_pay_type = new EditWork_Pay_Type(true, 0,
                worked_day_id, transfer_id, _dtEditable_Pay_Type, 0, 0);
            working_work_pay_type.Text = "Добавление данных об отработке по видам оплат";
            DialogResult dr = working_work_pay_type.ShowDialog();
            RefWorkPayType();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (dgWork_Pay_Type.CurrentRow != null)
            {
                int pay_type_id = Convert.ToInt32(dgWork_Pay_Type.CurrentRow.Cells["pay_type_id"].Value);
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
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (pay_type_id == 101 || pay_type_id == 102 || pay_type_id == 110 || pay_type_id == 114
                     || pay_type_id == 211 || pay_type_id == 111 || pay_type_id == 510 || (pay_type_id == 112 && GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                    || Connect.UserId.ToUpper() == "BMW12714")
                {
                    int work_pay_type_id = Convert.ToInt32(dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value);
                    EditWork_Pay_Type working_work_pay_type = new EditWork_Pay_Type(false, work_pay_type_id,
                        worked_day_id, transfer_id, _dtEditable_Pay_Type, 0, 0);
                    working_work_pay_type.Text = "Редактирование данных об отработке по видам оплат";
                    DialogResult dr = working_work_pay_type.ShowDialog();
                    RefWorkPayType();
                }
                else
                {
                    MessageBox.Show("В данном окне можно редактировать только 110, 111, 114, 211, 510 виды оплат!\n" +
                        "Для редактирования времени по прочим видам оплат нужно изменить оправдательный документ.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgWork_Pay_Type.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ocSignDeleteWPT.Parameters["p_work_pay_type_id"].Value =
                        dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value;
                    if (ocSignDeleteWPT.ExecuteReader().Read())
                    {
                        ocDeleteWPT.Parameters["p_work_pay_type_id"].Value =
                            dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value;
                        ocDeleteWPT.ExecuteNonQuery();
                        Connect.Commit();
                        RefWorkPayType();
                    }
                    else
                    {
                        MessageBox.Show("Данная запись связана с оправдательным документом!\n" +
                            "Для удаления нужно удалить или отредактировать оправдательный документ.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    //if (dgWork_Pay_Type.CurrentRow.Cells["reg_doc_id"].Value != DBNull.Value)
                    //{
                    //    MessageBox.Show("Данная запись связана с оправдательным документом!\n" +
                    //        "Для удаления нужно удалить или отредактировать оправдательный документ.",
                    //        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}
                    //else
                    //{
                    //    int pay_type_id = Convert.ToInt32(dgWork_Pay_Type.CurrentRow.Cells["pay_type_id"].Value);
                    //    if (pay_type_id == 101 || pay_type_id == 102 || pay_type_id == 112 || pay_type_id == 114 ||
                    //        pay_type_id == 211 || pay_type_id == 111 || pay_type_id == 510 || pay_type_id == 530)
                    //    {
                    //        ocDeleteWPT.Parameters["p_work_pay_type_id"].Value =
                    //            dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value;
                    //        ocDeleteWPT.ExecuteNonQuery();
                    //        Connect.Commit();
                    //        RefWorkPayType();
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("В данном окне можно удалять только 111, 112, 114, 211, 510 виды оплат!\n" +
                    //            "Для удаления времени по прочим видам оплат нужно удалить или отредактировать\n" +
                    //            " оправдательный документ.",
                    //            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    }
                    //}
                }
            }
        }
   
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Принятие времени по проходам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFromPerco_Click(object sender, EventArgs e)
        {
            OracleDataAdapter odaAccess = new OracleDataAdapter();
            odaAccess.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery(
                "Table/SelectAccessButtonTimePass.sql"), Connect.Schema), Connect.CurConnect);
            odaAccess.SelectCommand.BindByName = true;
            odaAccess.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
            odaAccess.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = date;
            DataTable dtAccess = new DataTable();
            odaAccess.Fill(dtAccess);
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") || GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                Convert.ToInt32(dtAccess.Rows[0]["SIGN_ACCESS"]) > 0)
            {
                if (MessageBox.Show("Вы действительно хотите принять время по проходам?" +
                    "\nВ этом случае время по графику будет перезаписано.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                    ocUpdate.BindByName = true;
                    ocUpdate.CommandText = string.Format(
                        "UPDATE {0}.WORKED_DAY W SET W.FROM_GRAPH = W.FROM_PERCO " +
                        "WHERE W.WORKED_DAY_ID = :p_worked_day_id", Connect.Schema);
                    ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = worked_day_id;
                    ocUpdate.ExecuteNonQuery();
                    if (dtWork_Pay_Type.Rows.Count > 0)
                    {
                        int work_pay_type_id =
                            Convert.ToInt32(dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value);
                        string pay_type_id = dgWork_Pay_Type.CurrentRow.Cells["pay_type_id"].Value.ToString();
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
                            ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = worked_day_id;
                            ocUpdate.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal).Value =
                                work_pay_type_id;
                            ocUpdate.ExecuteNonQuery();
                        }
                    }
                    Connect.Commit();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("У вас не хватает полномочий для данной операции!" +
                    "\nОбратитесь в группу табельного учета.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Принятие времени по видам оплат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFromPT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите принять время по текущей записи в видах оплат?" +
                "\nВ этом случае время по проходам будет перезаписано.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleCommand ocUpdate = new OracleCommand("", Connect.CurConnect);
                ocUpdate.BindByName = true;
                ocUpdate.CommandText = string.Format(
                    "update {0}.WORKED_DAY W " +
                    "set W.FROM_PERCO = " +
                        "nvl((select WP.VALID_TIME from {0}.WORK_PAY_TYPE WP " +
                        "where WP.WORK_PAY_TYPE_ID = :p_work_pay_type_id),0) " +
                        "where W.WORKED_DAY_ID = :p_worked_day_id", Connect.Schema);
                ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = worked_day_id;
                ocUpdate.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal).Value =
                    dgWork_Pay_Type.CurrentRow != null ? 
                    dgWork_Pay_Type.CurrentRow.Cells["work_pay_type_id"].Value : null;
                ocUpdate.ExecuteNonQuery();                   
                Connect.Commit();                
                Close();
            }
        }

        /// <summary>
        /// Принять общее время по видам оплат во время проходов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFromSumTP_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите принять общее время по видам оплат?" +
                "\nВ этом случае время по проходам будет перезаписано.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                ocUpdate.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = worked_day_id;
                ocUpdate.ExecuteNonQuery(); 
                Connect.Commit();
                //RefWorkPayType();
                Close();
            }
        }

        /// <summary>
        /// Пересчитать время
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCalcTime_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите пересчитать время за выбранный день?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(
                    "begin {0}.TABLE_UPDATEFORDATE_new(:p_month, :p_year, :p_per_num, :p_transfer_id, :p_date); end;",
                    Connect.Schema);
                com.Parameters.Add("p_month", OracleDbType.Decimal).Value = date.Month;
                com.Parameters.Add("p_year", OracleDbType.Decimal).Value = date.Year;
                com.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
                com.Parameters.Add("p_date", OracleDbType.Date).Value = date;
                com.ExecuteNonQuery();
                Connect.Commit();                
                Close();
            }
        }
               
        /// <summary>
        /// Нажатие кнопки редактирования заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEditOrder_Click(object sender, EventArgs e)
        {
            if (dgWork_Pay_Type.CurrentRow != null)
            {
                if (date > Table_Viewer.dCloseTable)
                {
                    EditOrder editOrder = new EditOrder(false);
                    editOrder.ShowInTaskbar = false;
                    editOrder.ShowDialog();
                    if (editOrder.Order_ID_Property != -1)
                    {
                        ocUpdateOrder.Parameters["p_order_id"].Value = editOrder.Order_ID_Property;
                        ocUpdateOrder.Parameters["p_work_pay_type_id"].Value =
                            dgWork_Pay_Type.CurrentRow.Cells["WORK_PAY_TYPE_ID"].Value;
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
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btSaveDoc_Click(object sender, EventArgs e)
        {
            DataSet dsChanges = dsPassWIthDoc.GetChanges();
            if (dsChanges != null)
            {
                for (int i = 0; i < dsChanges.Tables[0].Rows.Count; ++i)
                {
                    /// Если не пустой тип документа и пустой документ, то добавляем документ выбранного типа
                    if ((dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"] != DBNull.Value ||
                        Convert.ToInt32(dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"]) != -1) &&
                        dsChanges.Tables[0].Rows[i]["REG_DOC_ID"] == DBNull.Value)
                    {
                        int pay_type_id = 0;
                        for (int g = 0; g < dsDoc_List.Tables[0].Rows.Count; g++)
                        {
                            if (dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"].ToString() == 
                                dsDoc_List.Tables[0].Rows[g]["doc_list_id"].ToString())
                            {
                                pay_type_id = Convert.ToInt32(dsDoc_List.Tables[0].Rows[g]["pay_type_id"]);
                                break;
                            }
                        }
                        if (pay_type_id == 535 && dsChanges.Tables[0].Rows[i]["DOC_LOCATION"].ToString() == "")
                        {
                            MessageBox.Show("Для оправдательного документа Работа за территорией предприятия " +
                                "\nнеобходимо заполнить поле Местонахождение!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else
                        {
                            if (pay_type_id != 535)
                            {
                                dsChanges.Tables[0].Rows[i]["DOC_LOCATION"] = "";
                            }
                        }
                        REG_DOC_seq reg_doc = new REG_DOC_seq(Connect.CurConnect);
                        reg_doc.AddNew();
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM = per_num;
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).TRANSFER_ID = transfer_id;
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID =
                            Convert.ToDecimal(dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"]);
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_DATE = DateTime.Now;
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN =
                            dsChanges.Tables[0].Rows[i]["From_Plant"] != DBNull.Value ?
                            Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["From_Plant"]) :
                            Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["TIME_BEGIN"]);
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END =
                            dsChanges.Tables[0].Rows[i]["Into_Plant"] != DBNull.Value ?
                            Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["Into_Plant"]) :
                            Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["TIME_END"]);
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LOCATION =
                                dsChanges.Tables[0].Rows[i]["DOC_LOCATION"].ToString();

                        if (pay_type_id == 302)
                        {
                            decimal timeDoc = 0;
                            ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                            ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                            decimal hoursDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());                
                            if (Table_Viewer.HoursAbsence - hoursDoc + timeDoc < 0 &&//-10 &&
                                !GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                            {
                                decimal hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence));
                                decimal min = Table_Viewer.HoursAbsence - Math.Truncate(Table_Viewer.HoursAbsence);
                                string hoursAbs = string.Format("{0}{1}:{2}", Table_Viewer.HoursAbsence < 0 ? "-" : "", hours,
                                    Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                hours = Math.Truncate(Math.Abs(hoursDoc));
                                min = hoursDoc - Math.Truncate(hoursDoc);
                                string sthoursDoc = string.Format("{0}{1}:{2}", hoursDoc < 0 ? "-" : "", hours,
                                    Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence - hoursDoc + timeDoc));
                                min = (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) - Math.Truncate(Table_Viewer.HoursAbsence - hoursDoc + timeDoc);
                                string sthours = string.Format("{0}{1}:{2}", (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) < 0 ? "-" : "", hours,
                                    Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                MessageBox.Show("Недостаточно доступных часов в отгулах (перерасход не более 0 часов)!\n\n" +
                                    "Количество доступных часов отгулов = " + hoursAbs + "\n" +
                                    "Количество часов по вносимому документу = " + sthoursDoc + "\n\n" +
                                    "Документ не может быть сохранен, так как доступное время в отгулах будет = " + sthours,
                                    "АРМ \"Учет рабочего времени\"",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        reg_doc.Save();
                        Connect.Commit();

                        ocReg_Doc_Registr.Parameters["p_reg_doc_id"].Value =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID;
                        ocReg_Doc_Registr.ExecuteNonQuery();
                        Connect.Commit();
                        RegistrAbsence(pay_type_id, reg_doc);
                    }
                    else
                    {
                        /// Если не пустой тип документа и не пустой документ, то редактируем тип документа в
                        /// существующем документе
                        if (dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"] != DBNull.Value &&
                            Convert.ToInt32(dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"]) != -1 &&
                            dsChanges.Tables[0].Rows[i]["REG_DOC_ID"] != DBNull.Value)
                        {
                            int pay_type_id = 0;
                            for (int g = 0; g < dsDoc_List.Tables[0].Rows.Count; g++)
                            {
                                if (dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"].ToString() ==
                                    dsDoc_List.Tables[0].Rows[g]["doc_list_id"].ToString())
                                {
                                    pay_type_id = Convert.ToInt32(dsDoc_List.Tables[0].Rows[g]["pay_type_id"]);
                                    break;
                                }
                            }
                            if (pay_type_id == 535 && dsChanges.Tables[0].Rows[i]["DOC_LOCATION"].ToString() == "")
                            {
                                MessageBox.Show("Для оправдательного документа Работа за территорией предприятия " +
                                    "\nнеобходимо заполнить поле Местонахождение!",
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            else
                            {
                                if (pay_type_id != 535)
                                {
                                    dsChanges.Tables[0].Rows[i]["DOC_LOCATION"] = "";
                                }
                            }
                            REG_DOC_seq reg_doc = new REG_DOC_seq(Connect.CurConnect);
                            reg_doc.Fill("where REG_DOC_ID = " + dsChanges.Tables[0].Rows[i]["REG_DOC_ID"].ToString());
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID =
                                Convert.ToDecimal(dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"]);
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_DATE = DateTime.Now;
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN =
                                dsChanges.Tables[0].Rows[i]["From_Plant"] != DBNull.Value ?
                                Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["From_Plant"]) :
                                Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["TIME_BEGIN"]);
                            string str = dgPassWithDoc.CurrentRow.Cells["FROM_PLANT"].Value.ToString();
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END =
                                dsChanges.Tables[0].Rows[i]["Into_Plant"] != DBNull.Value ?
                                Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["Into_Plant"]) :
                                Convert.ToDateTime(dsChanges.Tables[0].Rows[i]["TIME_END"]);
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LOCATION =
                                dsChanges.Tables[0].Rows[i]["DOC_LOCATION"].ToString();

                            if (pay_type_id == 302)
                            {
                                decimal timeDoc = 0;
                                ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                                ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                                decimal hoursDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());
                                if (Table_Viewer.HoursAbsence - hoursDoc + timeDoc < -10 &&
                                    !GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                                {
                                    decimal hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence));
                                    decimal min = Table_Viewer.HoursAbsence - Math.Truncate(Table_Viewer.HoursAbsence);
                                    string hoursAbs = string.Format("{0}{1}:{2}", Table_Viewer.HoursAbsence < 0 ? "-" : "", hours,
                                        Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                    hours = Math.Truncate(Math.Abs(hoursDoc));
                                    min = hoursDoc - Math.Truncate(hoursDoc);
                                    string sthoursDoc = string.Format("{0}{1}:{2}", hoursDoc < 0 ? "-" : "", hours,
                                        Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                    hours = Math.Truncate(Math.Abs(Table_Viewer.HoursAbsence - hoursDoc + timeDoc));
                                    min = (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) - Math.Truncate(Table_Viewer.HoursAbsence - hoursDoc + timeDoc);
                                    string sthours = string.Format("{0}{1}:{2}", (Table_Viewer.HoursAbsence - hoursDoc + timeDoc) < 0 ? "-" : "", hours,
                                        Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                                    MessageBox.Show("Недостаточно доступных часов в отгулах (перерасход не более 10 часов)!\n\n" +
                                        "Количество доступных часов отгулов = " + hoursAbs + "\n" +
                                        "Количество часов по вносимому документу = " + sthoursDoc + "\n\n" +
                                        "Документ не может быть сохранен, так как доступное время в отгулах будет = " + sthours,
                                        "АРМ \"Учет рабочего времени\"",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }

                            reg_doc.Save();
                            Connect.Commit();

                            ocReg_Doc_Registr.Parameters["p_reg_doc_id"].Value =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID;
                            ocReg_Doc_Registr.ExecuteNonQuery();
                            Connect.Commit();
                            RegistrAbsence(pay_type_id, reg_doc);
                        }
                        else
                        {
                            /// Если пустой тип документа и не пустой документ, удаляем документ
                            if (Convert.ToInt32(dsChanges.Tables[0].Rows[i]["DOC_LIST_ID"]) == -1 &&
                                dsChanges.Tables[0].Rows[i]["REG_DOC_ID"] != DBNull.Value)
                            {
                                /*int pay_type_id = 0;
                                for (int g = 0; g < dsDoc_List.Tables[0].Rows.Count; g++)
                                {
                                    if (dsChanges.Tables[0].Rows[g]["DOC_LIST_ID"].ToString() ==
                                        dsDoc_List.Tables[0].Rows[g]["doc_list_id"].ToString())
                                    {
                                        pay_type_id = Convert.ToInt32(dsDoc_List.Tables[0].Rows[g]["pay_type_id"]);
                                        break;
                                    }
                                }
                                ocDeleteAbs.Parameters["p_absence_id"].Value =
                                    dsChanges.Tables[0].Rows[i]["ABSENCE_ID"];
                                ocDeleteAbs.ExecuteNonQuery();*/
                                ocDeleteReg_Doc.Parameters["p_reg_doc_id"].Value = dsChanges.Tables[0].Rows[i]["REG_DOC_ID"];
                                ocDeleteReg_Doc.ExecuteNonQuery();
                                Connect.Commit();                                
                            }
                        }
                    }                    
                }
                RefWorkPayType();
                RefPassWithDoc();
                RefReg_Doc();
            }
        }

        /// <summary>
        /// Обновление таблицы просмотра проходов проходной
        /// </summary>
        void RefPassWithDoc()
        {
            if (((DataRowView)((Table_Viewer)formTable).dgEmp.SelectedItem)["FL_WAYBILL"] == DBNull.Value)
            {
                dsPassWIthDoc.Reset();
                OracleDataAdapter adap =
                    new OracleDataAdapter(
                    string.Format(Queries.GetQuery("Table/SelectPassWithDoc.sql"),
                    Connect.Schema, "perco"),
                    Connect.CurConnect);
                adap.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                adap.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = date;
                adap.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
                adap.Fill(dsPassWIthDoc);
                dgPassWithDoc.AutoGenerateColumns = false;
                BindingSource bs = new BindingSource();
                bs.DataSource = dsPassWIthDoc.Tables[0];
                dgPassWithDoc.DataSource = bs;
            }
            else
            {
                dsPassWIthDoc.Reset();
                OracleDataAdapter adap = new OracleDataAdapter("", Connect.CurConnect);
                adap.SelectCommand.BindByName = true;
                if (date < new DateTime(2014, 4, 1))
                {
                    adap.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWayBill_Old.sql"),
                        Connect.Schema);
                }
                else
                {
                    adap.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWayBill.sql"),
                        Connect.Schema);
                }
                adap.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                adap.SelectCommand.Parameters.Add("p_date_work", OracleDbType.Date).Value = date;                
                adap.Fill(dsPassWIthDoc);
                dgPassWithDoc.AutoGenerateColumns = true;
                BindingSource bs = new BindingSource();
                bs.DataSource = dsPassWIthDoc.Tables[0];
                dgPassWithDoc.DataSource = bs;
                foreach (DataGridViewColumn column in dgPassWithDoc.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgPassWithDoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgPassWithDoc.Columns[0].Width = 55;
                dgPassWithDoc.Columns[1].Width = 35;
                dgPassWithDoc.Columns[2].Width = 105;
                dgPassWithDoc.Columns[3].Width = 105;
                dgPassWithDoc.Columns[4].Width = 60;
                dgPassWithDoc.Columns[5].Width = 60;
                dgPassWithDoc.Columns[6].Width = 60;
                dgPassWithDoc.Columns[7].Width = 100;
                dgPassWithDoc.Columns[8].Width = 100;
            }
        }

        void RegistrAbsence(int _pay_type, REG_DOC_seq _reg_doc)
        {       
            /// Проверяем отгулы ли это или наработка на отгулы
            if (_pay_type == 302)
            {
                absence.Clear();
                if (((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID == null)
                {
                    absence.AddNew();
                    ((CurrencyManager)BindingContext[absence]).Position =
                        ((CurrencyManager)BindingContext[absence]).Count;
                }
                else
                {
                    absence.Fill(string.Format("where {0} = {1}", ABSENCE_seq.ColumnsName.ABSENCE_ID,
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID));
                }

                DateTime dateControl =
                    ((REG_DOC_obj)(((CurrencyManager)BindingContext[_reg_doc]).Current)).DOC_BEGIN.Value.AddMinutes(30);
                if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[_reg_doc]).Current)).DOC_END)
                {
                    MessageBox.Show("Отгул менее 30 минут!" +
                        "\nВ отгулах будет убрано 30 минут.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                        dateControl;
                }
                else
                {
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).DOC_END;
                }

                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).PER_NUM;
                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).DOC_BEGIN;
                //((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                //    ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).DOC_END;
                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 2;
                absence.Save();
                ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID =
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;
            }
            else
                /// Если на оправдательный был привязан отгул, удаляем его
                if (((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID != null)
                {
                    absence.Clear();
                    ocDeleteAbs.Parameters["p_absence_id"].Value =
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID;
                    ocDeleteAbs.ExecuteNonQuery();
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[_reg_doc]).Current).ABSENCE_ID = null;
                }
            _reg_doc.Save();
            Connect.Commit();
        }

        private void btCancelDoc_Click(object sender, EventArgs e)
        {
            RefPassWithDoc();
        }

        /// <summary>
        /// Добавление оправдательного документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddReg_Doc_Click(object sender, EventArgs e)
        {
            /*EditReg_Doc editReg_doc = new EditReg_Doc(true, per_num,
                FormMain.subdiv_id, transfer_id, formTable, 0, degree_id, _worker_id);
            editReg_doc.Text = "Добавление оправдательных документов на: " + date.ToShortDateString();
            DialogResult rez = editReg_doc.ShowDialog();
            if (rez == DialogResult.OK)
            {
                RefWorkPayType();                
                RefPassWithDoc();
                RefReg_Doc();
            }*/
            object waybill = ((DataRowView)formTable.dgEmp.SelectedItem)["FL_WAYBILL"];
            WpfControlLibrary.Table.EditRegDoc editReg_doc = 
                new WpfControlLibrary.Table.EditRegDoc(transfer_id, null, !(waybill==DBNull.Value || waybill.ToString()=="X"));
            WindowInteropHelper helper = new WindowInteropHelper(editReg_doc);
            helper.Owner = this.Handle;
            editReg_doc.Title = "Редактирование документа на " + date.ToShortDateString();
            if (editReg_doc.ShowDialog() == true)
            {
                RefWorkPayType();
                RefPassWithDoc();
                RefReg_Doc();
            }
        }

        /// <summary>
        /// Редактирование оправдательного документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEditReg_Doc_Click(object sender, EventArgs e)
        {
            if (dgReg_Doc.CurrentRow != null)
            {
                /*decimal reg_doc_id = Convert.ToDecimal(dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value);
                EditReg_Doc editReg_doc = new EditReg_Doc(false, per_num, FormMain.subdiv_id,
                    transfer_id, formTable, reg_doc_id, degree_id, _worker_id);
                editReg_doc.Text = "Редактирование оправдательного документа на: " + date.ToShortDateString();
                DialogResult rez = editReg_doc.ShowDialog();
                if (rez == DialogResult.OK)
                {
                    RefWorkPayType();
                    RefPassWithDoc();
                    RefReg_Doc();
                }*/
                object waybill = ((DataRowView)formTable.dgEmp.SelectedItem)["FL_WAYBILL"];
                decimal reg_doc_id = Convert.ToDecimal(dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value);
                WpfControlLibrary.Table.EditRegDoc editReg_doc =
                    new WpfControlLibrary.Table.EditRegDoc(transfer_id, reg_doc_id, !(waybill == DBNull.Value || waybill.ToString() == "X"));
                WindowInteropHelper helper = new WindowInteropHelper(editReg_doc);
                helper.Owner = this.Handle;
                editReg_doc.Title = "Редактирование документа на " + date.ToShortDateString();
                if (editReg_doc.ShowDialog() == true)
                {
                    RefWorkPayType();
                    RefPassWithDoc();
                    RefReg_Doc();
                }
            }
        }

        /// <summary>
        /// Удаление оправдательных документов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteReg_Doc_Click(object sender, EventArgs e)
        {
            if (dgReg_Doc.CurrentRow != null)
            {
                /*if (dgReg_Doc.CurrentRow.Cells["PAY_TYPE_ID"].Value.ToString() == "226")
                {
                    MessageBox.Show("Нельзя удалить отпуск!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (MessageBox.Show("Удалить документ?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    ocDeleteReg_Doc.Parameters["p_reg_doc_id"].Value = dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value;
                    ocDeleteReg_Doc.ExecuteNonQuery();
                    Connect.Commit();
                    RefWorkPayType();
                    RefPassWithDoc();
                    RefReg_Doc();
                }*/
                if (MessageBox.Show("Удалить документ?", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleCommand cmd = new OracleCommand(string.Format("begin {0}.REG_DOC_DELETE(:p_reg_doc_id);end;", Connect.Schema), Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal, dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value, ParameterDirection.Input);
                    OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        RefWorkPayType();
                        RefPassWithDoc();
                        RefReg_Doc();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(this, ex.GetFormattedException(), "Ошибка удаления документа");
                    }
                }
            }
        }

        private void dgWork_Pay_Type_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если по шифру оплат нет заказа красим запись
            if (dgWork_Pay_Type["ORDER_NAME", e.RowIndex].Value == DBNull.Value)
            {
                /// Красим в розовый цвет
                e.CellStyle.BackColor = Color.Red;
            }
        }

        private void dgPassWithDoc_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Ошибочный оправдательный документ!!!" + 
                "\nПроверьте время и тип оправдательного документа!" +
                "\nВозможно данный документ не может быть в данное время.", 
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }

    }
}
