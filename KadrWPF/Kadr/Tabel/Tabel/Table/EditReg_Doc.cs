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

namespace Tabel
{
    public partial class EditReg_Doc : Form
    {
        REG_DOC_seq reg_doc;
        //DOC_LIST_seq doc_list;
        ABSENCE_seq absence;
        Table table;
        string per_num;
        decimal subdiv_id;
        decimal transfer_id;
        decimal reg_doc_id;
        bool flagAdd;
        decimal timeDoc;
        OracleCommand /*cmDoc_List, */cmCalendar, cmRole_GR, cmCountRowsDoc, cmOvertimeLeave, cmTimeLeave, cmTimeNotLeave,
            ocCount102, ocCountDocs, ocAbs_Calc_On_Doc, ocPermitNotRegistrPass, ocSignClosingOrder, _ocLock_Add_303;
        OracleDataTable dtSelTimeGraph, dtSelLimit, dtWaterProc, dtSelPassEvent, dtDoc_List;
        //OracleCommand cmCountRows;
        int pay_type, sign_all_day;
        string doc_note;
        DateTime? _dateEndDoc, _doc_Begin_Valid, _doc_End_Valid;
        /// <summary>
        /// Конструктор формы оправдательного документа
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_reg_doc">Таблица оправдательных документов</param>
        /// <param name="_doc_list">Список видов оправдательных документов</param>
        /// <param name="pos">Позиция в таблице</param>
        /// <param name="flagAdd">Признак добавления данных</param>
        /// <param name="_per_num">Табельный номер</param>
        /// <param name="_table">Родительская форма</param>
        public EditReg_Doc(/*DOC_LIST_seq _doc_list, */           
            bool _flagAdd, string _per_num, decimal _subdiv_id, decimal _transfer_id, 
            Table _table, decimal _reg_doc_id, decimal _degree_id, decimal worker_id)
        {
            InitializeComponent();
            reg_doc = new REG_DOC_seq(Connect.CurConnect);
            //doc_list = _doc_list;
            absence = new ABSENCE_seq(Connect.CurConnect);
            table = _table;
            per_num = _per_num;
            subdiv_id = _subdiv_id;
            transfer_id = _transfer_id;
            flagAdd = _flagAdd;
            reg_doc_id = _reg_doc_id;

            ocAbs_Calc_On_Doc = new OracleCommand(string.Format("select {0}.ABS_CALC_ON_DOC(:p_doc_begin,:p_doc_end) from dual",
                Connect.Schema), Connect.CurConnect);
            ocAbs_Calc_On_Doc.BindByName = true;
            ocAbs_Calc_On_Doc.Parameters.Add("p_doc_begin", OracleDbType.Date);
            ocAbs_Calc_On_Doc.Parameters.Add("p_doc_end", OracleDbType.Date);

            dtDoc_List = new OracleDataTable(string.Format(
                @"SELECT DOC_LIST_ID, DOC_NAME, DOC_NOTE, DOC_TYPE, PAY_TYPE_ID, ISCALC, ADD_HOLIDAY, SIGN_ALL_DAY,
                    DOC_BEGIN_VALID, DOC_END_VALID 
                FROM {0}.DOC_LIST
                ORDER BY DOC_NAME", Connect.Schema), Connect.CurConnect);
            dtDoc_List.Fill();
            /*cmDoc_List = new OracleCommand("", Connect.CurConnect);
            cmDoc_List.BindByName = true;
            cmDoc_List.CommandText = string.Format(
                "select pay_type_id, DOC_NOTE, SIGN_ALL_DAY from {0}.doc_list where doc_list_id = :p_doc_list_id",
                DataSourceScheme.SchemeName);
            cmDoc_List.Parameters.Add("p_doc_list_id", OracleDbType.Decimal);*/

            cbDoc_List_Name.DataSource = dtDoc_List;
            cbDoc_List_Name.DisplayMember = "DOC_NAME";
            cbDoc_List_Name.ValueMember = "DOC_LIST_ID";
            cbDoc_List_Name.DataBindings.Add("SelectedValue", reg_doc, "DOC_LIST_ID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            /*cbDoc_List_Name.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_LIST_ID, 
                new LinkArgument(dtDoc_List, DOC_LIST_seq.ColumnsName.DOC_NAME));*/
            cbDoc_List_Name.SelectedIndexChanged += new EventHandler(cbDoc_List_Name_SelectedIndexChanged);

            /// Если добавление
            if (flagAdd)
            {
                /// Добавляем новую запись
                reg_doc.AddNew();
                /// Устанавливаем позицию
                ((CurrencyManager)BindingContext[reg_doc]).Position = ((CurrencyManager)BindingContext[reg_doc]).Count;
                /// Заносим табельный номер
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).PER_NUM = per_num;
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_DATE = DateTime.Now;
                cbDoc_List_Name.SelectedItem = null;
            }
            else
            {
                reg_doc.Fill("where reg_doc_id = " + reg_doc_id);
                /// Показываем время начала действия документа
                mbDoc_Begin.Text = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" +
                    ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.TimeOfDay.Minutes.ToString().PadLeft(2, '0');
                /// Показываем время окончания действия документа
                mbDoc_End.Text = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" +
                    ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.TimeOfDay.Minutes.ToString().PadLeft(2, '0');
                _dateEndDoc = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                dtDoc_List.DefaultView.RowFilter = "DOC_LIST_ID = " + 
                //cmDoc_List.Parameters["p_doc_list_id"].Value =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
                //OracleDataReader reader = cmDoc_List.ExecuteReader();
                //if (reader.Read())
                if (dtDoc_List.DefaultView[0].Row != null)
                {
                    /*pay_type = Convert.ToInt32(reader["pay_type_id"]);
                    doc_note = reader["DOC_NOTE"].ToString();
                    sign_all_day = Convert.ToInt32(reader["SIGN_ALL_DAY"]);*/
                    pay_type = Convert.ToInt32(dtDoc_List.DefaultView[0]["pay_type_id"]);
                    doc_note = dtDoc_List.DefaultView[0]["DOC_NOTE"].ToString();
                    sign_all_day = Convert.ToInt32(dtDoc_List.DefaultView[0]["SIGN_ALL_DAY"]);
                    _doc_Begin_Valid = DateTime.Parse(dtDoc_List.DefaultView[0]["DOC_BEGIN_VALID"].ToString());
                    _doc_End_Valid = DateTime.Parse(dtDoc_List.DefaultView[0]["DOC_END_VALID"].ToString());
                }
                //reader.Close();
                if (pay_type == 302)
                {
                    ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                    timeDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());
                }
                dtDoc_List.DefaultView.RowFilter = "";
            }
            deDoc_Date.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_DATE);
            deDoc_Begin.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_BEGIN);
            deDoc_End.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_END);
            tbDoc_Number.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_NUMBER);            
            tbDoc_Location.AddBindingSource(reg_doc, REG_DOC_seq.ColumnsName.DOC_LOCATION);
                     
            cmCalendar = new OracleCommand("", Connect.CurConnect);
            cmCalendar.BindByName = true;
            cmCalendar.CommandText = string.Format(
                "select C.TYPE_DAY_ID from {0}.CALENDAR C " +
                "where trunc(C.CALENDAR_DAY) = trunc(:p_date_cal)",
                DataSourceScheme.SchemeName);
            cmCalendar.Parameters.Add("p_date_cal", OracleDbType.Date);

            dtSelPassEvent = new OracleDataTable("", Connect.CurConnect);
            if (((Table)table).dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value == DBNull.Value)
            {
                dtSelPassEvent.SelectCommand.CommandText = string.Format(
                    @"select to_char(max(event_time),'HH24:MI') MAX_EVENT_TIME,  
                        to_char(min(event_time),'HH24:MI') MIN_EVENT_TIME, max(event_time) MAX_EVENT,
                        MAX(CASE WHEN WHERE_FROM = 2 THEN EVENT_TIME END) MAX_EXIT
                    from {0}.EMP_PASS_EVENT EP where EP.per_num = :p_per_num and 
                        trunc(EP.event_time) = trunc(:p_date) ", 
                    "perco");
            }
            else
            {
                dtSelPassEvent.SelectCommand.CommandText = string.Format(
                    @"select to_char(max(END_WORK),'HH24:MI') MAX_EVENT_TIME,  
                        to_char(max(BEGIN_WORK),'HH24:MI') MIN_EVENT_TIME, max(END_WORK) MAX_EVENT,
                        MAX(END_WORK) MAX_EXIT
                    from {0}.V_PUTL_NEW where PER_NUM = :p_per_num and :p_date = trunc(END_WORK)",
                    Connect.Schema);
            }            
            dtSelPassEvent.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtSelPassEvent.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);
                         
            // Таблица, которая будет хранить время по графику
            dtSelTimeGraph = new OracleDataTable("", Connect.CurConnect);
            dtSelTimeGraph.SelectCommand.CommandText = string.Format(
                @"select to_char(FIRST_TIME_BEGIN,'HH24:MI') as MIN_TIME_BEGIN,
                    to_char(LAST_TIME_END,'HH24:MI') MAX_TIME_END,
                    FIRST_TIME_BEGIN, LAST_TIME_END, TIME_BEGIN, TIME_END,
                    MAX(case when :p_doc_begin between TIME_BEGIN and TIME_END or :p_doc_end between TIME_BEGIN and TIME_END 
                        then 1 else 0 end) OVER() FL_GRAPH
                from TABLE({0}.GET_EMP_GR_WORK_NEW(to_char(:p_date-5, 'dd.mm.yyyy'),  
                       to_char(trunc(last_day(sysdate)), 'dd.mm.yyyy'), :p_per_num, :p_transfer_id)) TW  
                where TW.W_DATE = :p_date", DataSourceScheme.SchemeName);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_doc_begin", OracleDbType.Date).Value = 
                DateTime.Today;
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_doc_end", OracleDbType.Date).Value =
                DateTime.Today;
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = 
                transfer_id;            

            cmRole_GR = new OracleCommand("", Connect.CurConnect);
            cmRole_GR.BindByName = true;
            cmRole_GR.CommandText = string.Format(Queries.GetQuery("Table/SelectRole_Edit.sql"),
                    DataSourceScheme.SchemeName);
            cmRole_GR.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            cmRole_GR.Parameters.Add("p_date", OracleDbType.Date);

            /*cmCountRows = new OracleCommand("", Connect.CurConnect);
            cmCountRows.BindByName = true;
            cmCountRows.CommandText = string.Format(Queries.GetQuery("Table/CountRowForPT.sql"), DataSourceScheme.SchemeName);
            cmCountRows.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            cmCountRows.Parameters.Add("p_date", OracleDbType.Date);
            cmCountRows.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            cmCountRows.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);*/

            cmCountRowsDoc = new OracleCommand("", Connect.CurConnect);
            cmCountRowsDoc.BindByName = true;
            cmCountRowsDoc.CommandText = string.Format(Queries.GetQuery("Table/CountRowsDocsForLimit.sql"), DataSourceScheme.SchemeName);
            cmCountRowsDoc.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            cmCountRowsDoc.Parameters.Add("p_date", OracleDbType.Date);
            cmCountRowsDoc.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;

            cmOvertimeLeave = new OracleCommand("", Connect.CurConnect);
            cmOvertimeLeave.BindByName = true;
            cmOvertimeLeave.CommandText = string.Format(Queries.GetQuery("Table/SelectOvertimeLeave.sql"),
                Connect.Schema);
            cmOvertimeLeave.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            cmOvertimeLeave.Parameters.Add("p_date", OracleDbType.Date);
            cmOvertimeLeave.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            cmOvertimeLeave.Parameters.Add("p_date_begin", OracleDbType.Date);
            cmOvertimeLeave.Parameters.Add("p_date_end", OracleDbType.Date);
            cmOvertimeLeave.Parameters["p_per_num"].Value = per_num;

            cmTimeLeave = new OracleCommand("", Connect.CurConnect);
            cmTimeLeave.BindByName = true;
            cmTimeLeave.CommandText = string.Format(Queries.GetQuery("Table/SelectTimeLeave.sql"),
                Connect.Schema);
            cmTimeLeave.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            cmTimeLeave.Parameters.Add("p_date", OracleDbType.Date);
            cmTimeLeave.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            cmTimeLeave.Parameters.Add("p_date_begin", OracleDbType.Date);
            cmTimeLeave.Parameters.Add("p_date_end", OracleDbType.Date);
            cmTimeLeave.Parameters["p_per_num"].Value = per_num;

            cmTimeNotLeave = new OracleCommand("", Connect.CurConnect);
            cmTimeNotLeave.BindByName = true;
            cmTimeNotLeave.CommandText = string.Format(Queries.GetQuery("Table/SelectTimeNotLeave.sql"),
                Connect.Schema);
            cmTimeNotLeave.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            cmTimeNotLeave.Parameters.Add("p_date", OracleDbType.Date);
            cmTimeNotLeave.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            cmTimeNotLeave.Parameters.Add("p_date_begin", OracleDbType.Date);
            cmTimeNotLeave.Parameters.Add("p_date_end", OracleDbType.Date);
            cmTimeNotLeave.Parameters["p_per_num"].Value = per_num;

            if (table.flagClosedTable)
            {
                gbReg_Doc.DisableAll(false, Color.White);
                btSave.Enabled = false;
            }
            /* Проверим условие, чтобы дата начала документа не была в месяце, который уже закрыт*/
            if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN < Table.dCloseTable)
            {
                deDoc_Begin.Enabled = false;
                mbDoc_Begin.Enabled = false;
            }

            // Таблица, которая будет хранить время начала работы по графику
            dtSelLimit = new OracleDataTable("", Connect.CurConnect);
            dtSelLimit.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/ViewLimitForDoc.sql"),
                DataSourceScheme.SchemeName);
            dtSelLimit.SelectCommand.Parameters.Add("p_date_doc", OracleDbType.Date);
            dtSelLimit.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal);
            dtSelLimit.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            dtSelLimit.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            dtSelLimit.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value = null;
            dtSelLimit.SelectCommand.Parameters["p_degree_id"].Value = _degree_id;
            dtSelLimit.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;

            ocCount102 = new OracleCommand(
                string.Format(Queries.GetQuery("Table/Count102.sql"),
                Connect.Schema), Connect.CurConnect);
            ocCount102.BindByName = true;
            ocCount102.Parameters.Add("p_date_work", OracleDbType.Date);
            ocCount102.Parameters.Add("p_transfer_id", OracleDbType.Decimal);

            dtWaterProc = new OracleDataTable("", Connect.CurConnect);
            dtWaterProc.SelectCommand.CommandText = string.Format(
                "select 1 from {0}.EMP_WAYBILL WB where WB.PER_NUM = :p_per_num ",
                Connect.Schema);
            dtWaterProc.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);

            ocCountDocs = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectCountDocs.sql"),
                Connect.Schema), Connect.CurConnect);
            ocCountDocs.BindByName = true;
            ocCountDocs.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
            ocCountDocs.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = transfer_id;
            ocCountDocs.Parameters.Add("p_DOC_END", OracleDbType.Date);
            ocCountDocs.Parameters.Add("p_DOC_BEGIN", OracleDbType.Date);            
            ocCountDocs.Parameters.Add("p_DOC_LIST_ID", OracleDbType.Decimal);
            ocCountDocs.Parameters.Add("p_REG_DOC_ID", OracleDbType.Decimal);
            ocCountDocs.Parameters.Add("p_PAY_TYPE_ID", OracleDbType.Decimal);
            ocCountDocs.Parameters.Add("p_DOC_NOTE", OracleDbType.Varchar2);

            ocPermitNotRegistrPass = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectPermitNotRegistrPass.sql"),
                Connect.Schema), Connect.CurConnect);
            ocPermitNotRegistrPass.BindByName = true;
            ocPermitNotRegistrPass.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
            ocPermitNotRegistrPass.Parameters.Add("p_date", OracleDbType.Date);

            ocSignClosingOrder = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectSignClosingOrder.sql"),
                Connect.Schema), Connect.CurConnect);
            ocSignClosingOrder.BindByName = true;
            ocSignClosingOrder.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
            ocSignClosingOrder.Parameters.Add("p_DATE_WORK", OracleDbType.Date);
            ocSignClosingOrder.Parameters.Add("p_SIGN_HOLIDAY", OracleDbType.Decimal);
            ocSignClosingOrder.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = transfer_id;

            _ocLock_Add_303 = new OracleCommand(string.Format(Queries.GetQuery("Table/Lock_Add_303.sql"),
                Connect.Schema), Connect.CurConnect);
            _ocLock_Add_303.BindByName = true;
            _ocLock_Add_303.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
            _ocLock_Add_303.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = worker_id;
            _ocLock_Add_303.Parameters.Add("p_DATE_DOC", OracleDbType.Date);
        }

        void cbDoc_List_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*cmDoc_List.Parameters["p_doc_list_id"].Value =
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
            OracleDataReader reader = cmDoc_List.ExecuteReader();
            if (reader.Read())*/
            if (cbDoc_List_Name.SelectedItem != null)
            {
                pay_type = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["pay_type_id"]);
                doc_note = ((DataRowView)cbDoc_List_Name.SelectedItem)["DOC_NOTE"].ToString();
                sign_all_day = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["SIGN_ALL_DAY"]);
                _doc_Begin_Valid = DateTime.Parse(((DataRowView)cbDoc_List_Name.SelectedItem)["DOC_BEGIN_VALID"].ToString());
                _doc_End_Valid = DateTime.Parse(((DataRowView)cbDoc_List_Name.SelectedItem)["DOC_END_VALID"].ToString());
            }
            else
            {
                pay_type = -1;
                doc_note = "";
                sign_all_day = -1;
                _doc_Begin_Valid = null;
                _doc_End_Valid = null;
            }
            //reader.Close();
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            deDoc_End.Focus();
            btSave.Focus();            
            /// Проверяем ввод данных
            if (deDoc_Date.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_Date.Focus();
                return;
            }
            if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_LIST_ID == null)
            {
                MessageBox.Show("Вы не указали тип документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbDoc_List_Name.Focus();
                return;
            }
            if (deDoc_Begin.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату начала действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_Begin.Focus();
                return;
            }
            if (deDoc_End.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_End.Focus();
                return;
            }
            if (deDoc_End.Date.Value < deDoc_Begin.Date.Value)
            {
                MessageBox.Show("Вы ввели неверную дату окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_End.Focus();
                return;
            }
            // Строка содержит время начала действия документа
            string str = mbDoc_Begin.Text.Trim(); 
            // Временная переменная для дат
            DateTime? db = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
            try
            {
                /// Добавляем в дату начала время начала
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN =
                    new DateTime(db.Value.Year, db.Value.Month, db.Value.Day, Convert.ToInt32(str.Substring(0, 2)),
                        Convert.ToInt32(str.Substring(3, 2)), 0);
            }
            catch
            {
                MessageBox.Show("Вы ввели неверное время начала действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbDoc_Begin.Focus();
                return;
            }
            // Строка содержит время окончания действия документа
            str = mbDoc_End.Text.Trim();
            db = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
            try
            {
                // Добавляем в дату окончания время окончания
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END =
                    new DateTime(db.Value.Year, db.Value.Month, db.Value.Day, Convert.ToInt32(str.Substring(0, 2)),
                        Convert.ToInt32(str.Substring(3, 2)), 0);
            }
            catch
            {
                MessageBox.Show("Вы ввели неверное время окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbDoc_End.Focus();
                return;
            }
            // Если начало или окончание документа не попадает в период действия типа документа, прерываем работу
            if (!(((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN > _doc_Begin_Valid &&
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN < _doc_End_Valid &&
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END > _doc_Begin_Valid &&
                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END < _doc_End_Valid))
            {
                MessageBox.Show("Документ не попадает в период действия типа документа: " +
                    _doc_Begin_Valid.Value.ToShortDateString() + " - " + _doc_End_Valid.Value.ToShortDateString(), "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbDoc_End.Focus();
                return;
            }         
            if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END < Table.dCloseTable
                && !GrantedRoles.GetGrantedRole("TABLE_EDIT_OLD"))
            {
                if (pay_type != 303 && pay_type != 304 && pay_type != 306)
                {
                    MessageBox.Show("Вы ввели неверное время окончания действия документа!" +
                        "\nЗа введенный период табель уже закрыт.",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deDoc_End.Focus();
                    return;
                }
            }
            DateTime dateControl;
            TimeSpan timeDocTS;
            decimal digit;
            switch (pay_type)
            {
                case 226:
                    MessageBox.Show("Нельзя работать с данным документом!" +
                        "\nОн формируется автоматически после закрытия отпуска " + 
                        "\nиз программы \"График отпусков\"." +
                        "\n\nНеобходимо убедиться, что данный отпуск ЗАКРЫТ в программе \"График отпусков\"",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbDoc_List_Name.Focus();
                    return;
                case 106:
                    #region 106
                    // 27.02.2016 - согласно служебной о.8 закрываем ручное добавление документов по 106 шифру
                    // Проверяем есть ли привилегия у сотрудника
                    //ocPermitNotRegistrPass.Parameters["p_date"].Value =
                    //    ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    //int countPermit1 = Convert.ToInt16(ocPermitNotRegistrPass.ExecuteScalar());
                    // Выбираем максимальный выход, если есть
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    dtSelPassEvent.Fill();
                    // По указанию о.8 ставим проверку на наличие закрытого приказа на работу в сверхурочное время
                    ocSignClosingOrder.Parameters["p_DATE_WORK"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    ocSignClosingOrder.Parameters["p_SIGN_HOLIDAY"].Value = 0;
                    bool _signClosignOrder = ocSignClosingOrder.ExecuteReader().Read();
                    if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END < new DateTime(2016,3,1) ||
                        GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") ||
                        GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                        ((Table)table).dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value != DBNull.Value ||
                            /*GrantedRoles.GetGrantedRole("TABLE_EDIT_ABSENCE") ||*/
                        (/*countPermit1 > 0 && */
                        /* 16.03.2016 - добавил новую роль с возможностью добавления документов при отсутствии проходов, но с закрытым приказом*/
                        (GrantedRoles.GetGrantedRole("TABLE_OVERTIME_DOC") || (dtSelPassEvent.Rows.Count > 0 &&
                            dtSelPassEvent.Rows[0]["MAX_EXIT"] != DBNull.Value &&
                            Convert.ToDateTime(dtSelPassEvent.Rows[0]["MAX_EXIT"]) >=
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)) &&
                            _signClosignOrder
                            /* 06.06.2016 - закрываем добавление документа */ && 1 == 2
                        ) ||
                        /* С разрешения 8 отдела добавляю новую роль с правами заносить документ без проходов, но с закрытым приказом (специально для ц.016)*/
                        (GrantedRoles.GetGrantedRole("TABLE_OVERTIME_WITHOUT_ORDER") && _signClosignOrder))
                    {
                        #region  Проверка лимитов 
                        dtWaterProc.Clear();
                        dtWaterProc.SelectCommand.Parameters["p_per_num"].Value = per_num;
                        dtWaterProc.Fill();
                        if (dtWaterProc.Rows.Count == 0)
                        {
                            dtSelLimit.Clear();
                            dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 106;
                            dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                            dtSelLimit.Fill();
                            double limitPlan =
                                Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                            double limitFakt =
                                Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                            timeDocTS = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                            if (limitFakt + timeDocTS.TotalHours > limitPlan)
                            {
                                MessageBox.Show("Невозможно сохранить документ!" +
                                    "\nПо категории превышен лимит часов на работу в сверхурочное время." +
                                    "\nДоступное время в часах равно " + (limitPlan - limitFakt).ToString("F2"),
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        #endregion
                        // Выполняем приказ от 3 отдела - Сверхурочная не должна быть менее получаса
                        dateControl = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddMinutes(30);
                        if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)
                        {
                            MessageBox.Show("Данный документ должен быть не менее получаса!",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else
                        {
                            dtSelTimeGraph.Clear();
                            dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = deDoc_Begin.Date;
                            dtSelTimeGraph.SelectCommand.Parameters["p_doc_begin"].Value =
                                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddSeconds(1);
                            dtSelTimeGraph.SelectCommand.Parameters["p_doc_end"].Value =
                                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.AddSeconds(-1);
                            dtSelTimeGraph.Fill();
                            if (dtSelTimeGraph.Rows.Count > 0)
                            {
                                /* Можно добавлять служебки после работы по графику или
                                 до работы по графику и в обед, если грантована соответствующая роль */
                                if (!(((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN >=
                                    Convert.ToDateTime(dtSelTimeGraph.Rows[0]["LAST_TIME_END"]) ||
                                    (GrantedRoles.GetGrantedRole("TABLE_ADD_OVERTIME") &&                            
                                    Convert.ToInt32(dtSelTimeGraph.Rows[0]["FL_GRAPH"]) == 0 &&
                                    ((Table)table).dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value != DBNull.Value) ||
                                    (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") &&
                                    Convert.ToInt32(dtSelTimeGraph.Rows[0]["FL_GRAPH"]) == 0)))
                                {
                                    MessageBox.Show("Нельзя вводить данный документ во время работы по графику!",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нельзя вводить документ \"Работа в сверхурочное время\"!" +
                            "\nДанный документ формируется автоматически на основании приказа на работу в сверхурочное время!" +
                            "\n(возможно не закрыт приказ на работу)" +
                            "\n\nПо интересующим вас вопросам, просьба обращаться в Отдел организации труда и заработной платы.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                    #endregion
                case 302:
                    #region 302
                    ocAbs_Calc_On_Doc.Parameters["p_doc_begin"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    ocAbs_Calc_On_Doc.Parameters["p_doc_end"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END;
                    decimal hoursDoc = Convert.ToDecimal(ocAbs_Calc_On_Doc.ExecuteScalar());                
                    if (Table.HoursAbsence - hoursDoc + timeDoc < -10 &&
                        !GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                    {
                        decimal hours = Math.Truncate(Math.Abs(Table.HoursAbsence));
                        decimal min = Table.HoursAbsence - Math.Truncate(Table.HoursAbsence);
                        string hoursAbs = string.Format("{0}{1}:{2}", Table.HoursAbsence < 0 ? "-" : "", hours,
                            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        hours = Math.Truncate(Math.Abs(hoursDoc));
                        min = hoursDoc - Math.Truncate(hoursDoc);
                        string sthoursDoc = string.Format("{0}{1}:{2}", hoursDoc < 0 ? "-" : "", hours,
                            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        hours = Math.Truncate(Math.Abs(Table.HoursAbsence - hoursDoc + timeDoc));
                        min = (Table.HoursAbsence - hoursDoc + timeDoc) - Math.Truncate(Table.HoursAbsence - hoursDoc + timeDoc);
                        string sthours = string.Format("{0}{1}:{2}", (Table.HoursAbsence - hoursDoc + timeDoc) < 0 ? "-" : "", hours,
                            Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2, '0'));
                        MessageBox.Show("Недостаточно доступных часов в отгулах (перерасход не более 10 часов)!\n\n"+
                            "Количество доступных часов отгулов = " +hoursAbs + "\n" +
                            "Количество часов по вносимому документу = " + sthoursDoc + "\n\n" + 
                            "Документ не может быть сохранен, так как доступное время в отгулах будет = " + sthours,
                            "АРМ \"Учет рабочего времени\"", 
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    timeDoc = hoursDoc;
                    absence.Clear();
                    if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID == null)
                    {
                        absence.AddNew();
                        ((CurrencyManager)BindingContext[absence]).Position =
                            ((CurrencyManager)BindingContext[absence]).Count;
                    }
                    else
                    {
                        absence.Fill(string.Format("where {0} = {1}", ABSENCE_seq.ColumnsName.ABSENCE_ID,
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID));
                    }
                    dateControl = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddMinutes(30);
                    if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)
                    {
                        MessageBox.Show("Данный документ менее 30 минут!" +
                            "\nВ отгулах будет убрано 30 минут.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                            dateControl;
                    }
                    else
                    { 
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                    }
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;                
                    ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 2;
                    //absence.Save();
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;
                    break;
                    #endregion
                case 303:
                    #region 303
                    digit = 0;
                    cmCalendar.Parameters["p_date_cal"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    digit = (decimal)cmCalendar.ExecuteScalar();                    
                    dateControl =
                    ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddMinutes(30);
                    if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)
                    {
                        MessageBox.Show("Данный документ должен быть не менее получаса!",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value >
                        DateTime.Today)
                    {
                        MessageBox.Show("Документ не может оканчиваться раньше текущей даты!",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    // Для выходных и праздников выполняем свою проверку. Для рабочих дней - свою
                    if (digit == 1 || digit == 4)
                    {                           
                        // 03.04.2014 Приказ 312 - с 1 апреля 2014 убираем проверку лимитов
                        // 11.04.2015 Приказ 38 - с 1 апреля 2015 добавляем проверку, но уже лимитов по 303
                        if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value < new DateTime(2014, 4, 1))
                        {
                            #region  Проверка лимитов
                            dtWaterProc.Clear();
                            dtWaterProc.SelectCommand.Parameters["p_per_num"].Value = per_num;
                            dtWaterProc.Fill();
                            if (dtWaterProc.Rows.Count == 0)
                            {
                                dtSelLimit.Clear();
                                // 11.04.2015
                                //dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 124;
                                dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 303;
                                dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                                dtSelLimit.Fill();
                                double limitPlan =
                                    Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                                double limitFakt =
                                    Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                                timeDocTS = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                    (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                                if (limitFakt + timeDocTS.TotalHours > limitPlan)
                                {
                                    cmCountRowsDoc.Parameters["p_date"].Value = deDoc_End.Date;
                                    int countRowsDoc = Convert.ToInt32(cmCountRowsDoc.ExecuteScalar());
                                    /// Если существуют записи по отпуску или больничному за этот день, 
                                    /// то добавить 303 шифр можно
                                    if (countRowsDoc == 0)
                                    {
                                        MessageBox.Show("Невозможно сохранить документ!" +
                                            "\nПо категории превышен лимит часов на работу в счет отгула." +
                                            "\nДоступное время в часах равно " + (limitPlan - limitFakt).ToString("F2"),
                                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value >= new DateTime(2015, 4, 1))
                            {
                                MessageBox.Show("Нельзя добавлять данный тип документа в выходные и нерабочие дни!",
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ///* Проверку лимитов делаем с 01.08.2013*/
                        //if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value >
                        //    new DateTime(2013, 8, 1).AddSeconds(-1))
                        ///06.09.2013. С 01.08.2013 по приказу была проверка 303 шифра в учете лимитов.
                        ///По служебной записке о.3 от 03.09.2013 убираем проверку лимитов.
                        /* 12.10.2013 - убираем условие и смотрим лимиты за любой период*/
                        /*if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value >= new DateTime(2013, 8, 1) &&
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value <= new DateTime(2013, 9, 1).AddSeconds(-1))*/
                        // 03.04.2014 Приказ 312 - с 1 апреля убираем проверку лимитов
                        // 11.04.2015 Приказ 38 - с 1 апреля 2015 добавляем проверку, но уже лимитов по 303
                        if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value < new DateTime(2014, 4, 1))
                        {
                            #region  Проверка лимитов
                            dtWaterProc.Clear();
                            dtWaterProc.SelectCommand.Parameters["p_per_num"].Value = per_num;
                            dtWaterProc.Fill();
                            if (dtWaterProc.Rows.Count == 0)
                            {
                                dtSelLimit.Clear();
                                // 11.04.2015
                                //dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 106;
                                dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 303;
                                dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                                dtSelLimit.Fill();
                                double limitPlan =
                                    Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                                double limitFakt =
                                    Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                                timeDocTS = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                    (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                                if (limitFakt + timeDocTS.TotalHours > limitPlan)
                                {
                                    cmCountRowsDoc.Parameters["p_date"].Value = deDoc_End.Date;
                                    int countRowsDoc = Convert.ToInt32(cmCountRowsDoc.ExecuteScalar());
                                    /// Если существуют записи по отпуску или больничному за этот день, 
                                    /// то добавить 303 шифр можно
                                    if (countRowsDoc == 0)
                                    {
                                        MessageBox.Show("Невозможно сохранить документ!" +
                                            "\nПо категории превышен лимит часов на работу в счет отгула." +
                                            "\nДоступное время в часах равно " + (limitPlan - limitFakt).ToString("F2"),
                                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    // Если документ по 303 раньше 01.04.2015, то оставляем старую проверку. 
                    // Если нет - то 303 только в рабочее время при наличие документов типа отпуск, больничный и т.п.
                    if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value < new DateTime(2015, 4, 1))
                    {
                        // Проверяем попадает ли документ в график работы. Если попадает, то не пропускаем это
                        cmOvertimeLeave.Parameters["p_transfer_id"].Value = transfer_id;
                        cmOvertimeLeave.Parameters["p_per_num"].Value = per_num;
                        cmOvertimeLeave.Parameters["p_date"].Value = deDoc_Begin.Date;
                        cmOvertimeLeave.Parameters["p_date_begin"].Value = 
                            DateTime.Parse(deDoc_Begin.Date.Value.ToShortDateString() + " " + mbDoc_Begin.Text);
                        cmOvertimeLeave.Parameters["p_date_end"].Value = 
                            DateTime.Parse(deDoc_End.Date.Value.ToShortDateString() + " " + mbDoc_End.Text);
                        int overtime = Convert.ToInt32(cmOvertimeLeave.ExecuteScalar());
                        if (overtime == 0 && (digit == 2 || digit == 3))
                        {
                            if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_ABSENCE"))
                            {
                                MessageBox.Show("Невозможно сохранить данный документ!" +
                                    "\nЗадайте время в соответствии с графиком работы." +
                                    "\nВремя обеденного перерыва и сверхурочное время не может входить в работу в счет отгула.",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    else
                    {
                        // 21.04.2016 - выполняем служебку о. 5 о запрете ввода отгулов во время отпуска и больничного
                        if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value > new DateTime(2016, 4, 21))
                        {
                            _ocLock_Add_303.Parameters["p_DATE_DOC"].Value = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value;
                            if (Convert.ToInt16(_ocLock_Add_303.ExecuteScalar()) > 0)
                            {
                                MessageBox.Show("Невозможно сохранить данный документ!" +
                                    "\nНачиная с 21.04.2016 запрещено вносить работу в счет отгула во время отпуска и больничного.",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        cmTimeLeave.Parameters["p_transfer_id"].Value = transfer_id;
                        cmTimeLeave.Parameters["p_per_num"].Value = per_num;
                        cmTimeLeave.Parameters["p_date"].Value = deDoc_Begin.Date;
                        cmTimeLeave.Parameters["p_date_begin"].Value =
                            DateTime.Parse(deDoc_Begin.Date.Value.ToShortDateString() + " " + mbDoc_Begin.Text);
                        cmTimeLeave.Parameters["p_date_end"].Value =
                            DateTime.Parse(deDoc_End.Date.Value.ToShortDateString() + " " + mbDoc_End.Text);
                        int overtime = Convert.ToInt32(cmTimeLeave.ExecuteScalar());
                        if (overtime == 0 && (digit == 2 || digit == 3))
                        {
                            MessageBox.Show("Невозможно сохранить данный документ!" +
                                "\nЗадайте время в соответствии с графиком работы." +
                                "\nВремя обеденного перерыва и сверхурочное время не может проведено данным типом документа.",
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    if (flagAdd)
                    {
                        absence.AddNew();
                        ((CurrencyManager)BindingContext[absence]).Position = ((CurrencyManager)BindingContext[absence]).Count;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                        //absence.Save();
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;

                    }
                    else
                    {
                        absence.Clear();
                        if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID == null)
                        {
                            absence.AddNew();
                            ((CurrencyManager)BindingContext[absence]).Position =
                                ((CurrencyManager)BindingContext[absence]).Count;
                        }
                        else
                        {
                            absence.Fill(string.Format("where {0} = {1}", ABSENCE_seq.ColumnsName.ABSENCE_ID,
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID));
                        }
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                        ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;
                        //absence.Save();
                    }  
                    break;
                    #endregion
                case 304:
                    #region 304
                    // Проверяем есть ли привилегия у сотрудника
                    ocPermitNotRegistrPass.Parameters["p_date"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    int countPermit1 = Convert.ToInt16(ocPermitNotRegistrPass.ExecuteScalar());
                    // Выбираем максимальный выход, если есть
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = 
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    dtSelPassEvent.Fill();
                    // По указанию о.3 ставим проверку на наличие закрытого приказа на работу в выходные дни
                    ocSignClosingOrder.Parameters["p_DATE_WORK"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    ocSignClosingOrder.Parameters["p_SIGN_HOLIDAY"].Value = 1;
                    bool _signClosignOrder3 = ocSignClosingOrder.ExecuteReader().Read();
                    if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") ||
                            GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                            GrantedRoles.GetGrantedRole("TABLE_EDIT_ABSENCE") ||
                        (countPermit1 > 0 && dtSelPassEvent.Rows.Count > 0 && 
                            dtSelPassEvent.Rows[0]["MAX_EXIT"] != DBNull.Value &&
                            Convert.ToDateTime(dtSelPassEvent.Rows[0]["MAX_EXIT"]) >=
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END &&
                            _signClosignOrder3
                            /* 06.06.2016 - закрываем добавление документа в счет отгула */ && 1 == 2))
                    {
                        #region  Проверка лимитов
                        dtSelLimit.Clear();
                        dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 304;
                        dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                        dtSelLimit.Fill();
                        double limitPlan304 =
                            Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                        double limitFakt304 =
                            Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                        timeDocTS =
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                        if (limitFakt304 + timeDocTS.TotalHours > limitPlan304)
                        {
                            MessageBox.Show("Невозможно сохранить документ!" +
                                "\nПо категории превышен лимит часов на работу в счет отгула в выходные дни." +
                                "\nДоступное время в часах равно " + (limitPlan304 - limitFakt304).ToString("F2"),
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        #endregion
                        digit = 0;
                        cmCalendar.Parameters["p_date_cal"].Value =
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                        digit = (decimal)cmCalendar.ExecuteScalar();
                        if (digit == 2 || digit == 3)
                        {
                            MessageBox.Show("Нельзя вводить документ \"Работа в счет отгула (выходные)\" в рабочий день!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        if (flagAdd)
                        {
                            absence.AddNew();
                            ((CurrencyManager)BindingContext[absence]).Position = ((CurrencyManager)BindingContext[absence]).Count;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                            //absence.Save();
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;

                        }
                        else
                        {
                            absence.Clear();
                            if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID == null)
                            {
                                absence.AddNew();
                                ((CurrencyManager)BindingContext[absence]).Position =
                                    ((CurrencyManager)BindingContext[absence]).Count;
                            }
                            else
                            {
                                absence.Fill(string.Format("where {0} = {1}", ABSENCE_seq.ColumnsName.ABSENCE_ID,
                                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID));
                            }
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;
                            //absence.Save();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нельзя вводить документ \"Работа в счет отгула (выходные)\"!" +
                            "\nДанный документ формируется автоматически на основании приказа на работу в выходные и нерабочие дни!" +
                            "\n(возможно не закрыт приказ на работу в выходные дни)" +
                            "\n\nПо интересующим вас вопросам, просьба обращаться в Отдел организации труда и заработной платы.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                    #endregion
                case 306:
                    #region 306
                    // 27.02.2016 - согласно служебной о.8 закрываем ручное добавление документов по 306 шифру
                    // Проверяем есть ли привилегия у сотрудника
                    //ocPermitNotRegistrPass.Parameters["p_date"].Value =
                    //    ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    //int countPermit1 = Convert.ToInt16(ocPermitNotRegistrPass.ExecuteScalar());
                    // Выбираем максимальный выход, если есть
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    dtSelPassEvent.Fill();
                    // По указанию о.8 ставим проверку на наличие закрытого приказа на работу в сверхурочное время
                    ocSignClosingOrder.Parameters["p_DATE_WORK"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    ocSignClosingOrder.Parameters["p_SIGN_HOLIDAY"].Value = 1;
                    bool _signClosignOrder2 = ocSignClosingOrder.ExecuteReader().Read();
                    if (((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END < new DateTime(2016, 3, 1) ||
                        GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") ||
                            GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                            ((Table)table).dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value != DBNull.Value ||
                        /*GrantedRoles.GetGrantedRole("TABLE_EDIT_ABSENCE") ||*/
                        (/*countPermit1 > 0 && */
                            /* 16.03.2016 - добавил новую роль с возможностью добавления документов при отсутствии проходов, но с закрытым приказом*/
                        (GrantedRoles.GetGrantedRole("TABLE_OVERTIME_DOC") || (dtSelPassEvent.Rows.Count > 0 &&
                            dtSelPassEvent.Rows[0]["MAX_EXIT"] != DBNull.Value &&
                            dtSelPassEvent.Rows.Count > 0 &&
                            Convert.ToDateTime(dtSelPassEvent.Rows[0]["MAX_EXIT"]) >=
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)) &&
                            _signClosignOrder2 
                            /* 06.06.2016 - закрываем добавление документа в счет отгула */ && 1 == 2
                            ) ||
                        /* С разрешения 8 отдела добавляю новую роль с правами заносить отгулы без приказа (специально для о.27)*/
                        (GrantedRoles.GetGrantedRole("TABLE_OVERTIME_WITHOUT_ORDER") ))
                    {
                        #region  Проверка лимитов 
                        dtWaterProc.Clear();
                        dtWaterProc.SelectCommand.Parameters["p_per_num"].Value = per_num;
                        dtWaterProc.Fill();
                        if (dtWaterProc.Rows.Count == 0)
                        {
                            dtSelLimit.Clear();
                            dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 306;
                            dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                            dtSelLimit.Fill();
                            double limitPlan =
                                Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                            double limitFakt =
                                Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                            timeDocTS = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                            if (limitFakt + timeDocTS.TotalHours > limitPlan)
                            {
                                MessageBox.Show("Невозможно сохранить документ!" +
                                    "\nПо категории превышен лимит часов на работу в счет отгула в сверхурочное время." +
                                    "\nДоступное время в часах равно " + (limitPlan - limitFakt).ToString("F2"),
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        #endregion
                        digit = 0;
                        cmCalendar.Parameters["p_date_cal"].Value =
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                        digit = (decimal)cmCalendar.ExecuteScalar();
                        if (digit == 1 || digit == 4)
                        {
                            MessageBox.Show("Нельзя вводить документ \"Работа в счет отгула (сверхурочные)\" в выходной день!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        // Выполняем приказ от 3 отдела - Сверхурочная не должна быть менее получаса
                        dateControl = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddMinutes(30);
                        if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)
                        {
                            MessageBox.Show("Данный документ должен быть не менее получаса!",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else
                        {
                            /*dtSelTimeGraph.Clear();
                            dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = deDoc_Begin.Date;
                            dtSelTimeGraph.SelectCommand.Parameters["p_doc_begin"].Value =
                                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddSeconds(1);
                            dtSelTimeGraph.SelectCommand.Parameters["p_doc_end"].Value =
                                ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.AddSeconds(-1);
                            dtSelTimeGraph.Fill();
                            if (dtSelTimeGraph.Rows.Count > 0)
                            {
                                // Можно добавлять документ после работы по графику или
                                // до работы по графику и в обед, если грантована соответствующая роль 
                                if (!(((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN >=
                                    Convert.ToDateTime(dtSelTimeGraph.Rows[0]["LAST_TIME_END"]) ||
                                    (GrantedRoles.GetGrantedRole("TABLE_ADD_OVERTIME") &&                            
                                    Convert.ToInt32(dtSelTimeGraph.Rows[0]["FL_GRAPH"]) == 0 &&
                                    ((Table)table).dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value != DBNull.Value) ||
                                    (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") &&
                                    Convert.ToInt32(dtSelTimeGraph.Rows[0]["FL_GRAPH"]) == 0)))
                                {
                                    MessageBox.Show("Нельзя вводить данный документ во время работы по графику!",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }*/
                        }
                        cmTimeNotLeave.Parameters["p_transfer_id"].Value = transfer_id;
                        cmTimeNotLeave.Parameters["p_per_num"].Value = per_num;
                        cmTimeNotLeave.Parameters["p_date"].Value = deDoc_Begin.Date;
                        cmTimeNotLeave.Parameters["p_date_begin"].Value =
                            DateTime.Parse(deDoc_Begin.Date.Value.ToShortDateString() + " " + mbDoc_Begin.Text);
                        cmTimeNotLeave.Parameters["p_date_end"].Value =
                            DateTime.Parse(deDoc_End.Date.Value.ToShortDateString() + " " + mbDoc_End.Text);
                        int timenot306 = Convert.ToInt32(cmTimeNotLeave.ExecuteScalar());
                        if (timenot306 > 0)
                        {
                            MessageBox.Show("Невозможно сохранить данный документ!" +
                                "\nЗадайте время сверхурочной работы.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        // Проверяем попадает ли документ в график работы. Если попадает, то не пропускаем это
                        cmOvertimeLeave.Parameters["p_transfer_id"].Value = transfer_id;
                        cmOvertimeLeave.Parameters["p_per_num"].Value = per_num;
                        cmOvertimeLeave.Parameters["p_date"].Value = deDoc_Begin.Date;
                        cmOvertimeLeave.Parameters["p_date_begin"].Value =
                            DateTime.Parse(deDoc_Begin.Date.Value.ToShortDateString() + " " + mbDoc_Begin.Text);
                        cmOvertimeLeave.Parameters["p_date_end"].Value =
                            DateTime.Parse(deDoc_End.Date.Value.ToShortDateString() + " " + mbDoc_End.Text);
                        int overtime306 = Convert.ToInt32(cmOvertimeLeave.ExecuteScalar());
                        if (overtime306 == 0 && (digit == 2 || digit == 3))
                        {
                            if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_ABSENCE"))
                            {
                                MessageBox.Show("Невозможно сохранить данный документ!" +
                                    "\nЗадайте время в соответствии с графиком работы или время сверхурочной работы." +
                                    "\nВремя обеденного перерыва не может входить в работу в счет отгула.",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        if (flagAdd)
                        {
                            absence.AddNew();
                            ((CurrencyManager)BindingContext[absence]).Position = ((CurrencyManager)BindingContext[absence]).Count;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                            //absence.Save();
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;

                        }
                        else
                        {
                            absence.Clear();
                            if (((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID == null)
                            {
                                absence.AddNew();
                                ((CurrencyManager)BindingContext[absence]).Position =
                                    ((CurrencyManager)BindingContext[absence]).Count;
                            }
                            else
                            {
                                absence.Fill(string.Format("where {0} = {1}", ABSENCE_seq.ColumnsName.ABSENCE_ID,
                                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID));
                            }
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).PER_NUM;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_BEGIN =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABS_TIME_END =
                                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
                            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
                            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID =
                                ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).ABSENCE_ID;
                            //absence.Save();
                        }  
                    }
                    else
                    {
                        MessageBox.Show("Нельзя вводить документ \"Работа в счет отгула в сверхурочное время\"!" +
                            "\nДанный документ формируется автоматически на основании приказа на работу в сверхурочное время!" +
                            "\n(возможно не закрыт приказ на работу)" +
                            "\n\nПо интересующим вас вопросам, просьба обращаться в Отдел организации труда и заработной платы.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                    #endregion
                case 124:
                    #region 124
                    // Проверяем если ли привилегия у сотрудника
                    ocPermitNotRegistrPass.Parameters["p_date"].Value =
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    int countPermit = Convert.ToInt16(ocPermitNotRegistrPass.ExecuteScalar());
                    // Выбираем максимальный выход, если есть
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = 
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                    dtSelPassEvent.Fill();
                    if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV") ||
                            GrantedRoles.GetGrantedRole("TABLE_FORM_FILE") ||
                        (countPermit > 0 && dtSelPassEvent.Rows.Count > 0 && 
                            dtSelPassEvent.Rows[0]["MAX_EXIT"] != DBNull.Value &&
                            Convert.ToDateTime(dtSelPassEvent.Rows[0]["MAX_EXIT"]) >=
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END))
                    {
                        #region  Проверка лимитов
                        dtSelLimit.Clear();
                        dtSelLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 124;
                        dtSelLimit.SelectCommand.Parameters["p_date_doc"].Value = deDoc_End.Date;
                        dtSelLimit.Fill();
                        double limitPlan =
                            Convert.ToDouble(dtSelLimit.Rows[0]["PLAN"] != DBNull.Value ? dtSelLimit.Rows[0]["PLAN"] : 0);
                        double limitFakt =
                            Convert.ToDouble(dtSelLimit.Rows[0]["FACT"] != DBNull.Value ? dtSelLimit.Rows[0]["FACT"] : 0);
                        timeDocTS =
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Subtract(
                                (DateTime)((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN);
                        if (limitFakt + timeDocTS.TotalHours > limitPlan)
                        {
                            MessageBox.Show("Невозможно сохранить документ!" +
                                "\nПо категории превышен лимит часов на работу в выходные и нерабочие дни." +
                                "\nДоступное время в часах равно " + (limitPlan - limitFakt).ToString("F2"),
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        #endregion
                        /*cmCountRows.Parameters["p_per_num"].Value = per_num;
                            cmCountRows.Parameters["p_date"].Value = deDoc_End.Date;
                            cmCountRows.Parameters["p_transfer_id"].Value = transfer_id;
                            cmCountRows.Parameters["p_pay_type_id"].Value = 303;
                            int countRows = Convert.ToInt32(cmCountRows.ExecuteScalar());
                            /// Если существуют записи по 303 шифру за этот день, то добавить 124 шифру нельзя
                            if (countRows > 0)
                            {
                                MessageBox.Show("Нельзя вводить документ \"Работа в выходные и нерабочие дни\" " +
                                    "когда уже есть документ \n\"Работа в счет отгула\"!",
                                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }*/
                        digit = 0;
                        cmCalendar.Parameters["p_date_cal"].Value =
                            ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN;
                        digit = (decimal)cmCalendar.ExecuteScalar();
                        if (digit == 2 || digit == 3)
                        {
                            MessageBox.Show("Нельзя вводить документ \"Работа в выходные и нерабочие дни\" в рабочий день!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нельзя вводить документ \"Работа в выходные и нерабочие дни\"!" +
                            "\nДанный документ формируется автоматически на основании приказа на работу в выходные и нерабочие дни!" +
                            "\n\nПо интересующим вас вопросам, просьба обращаться в Отдел организации труда и заработной платы.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                    #endregion
                case 542:
                    if ((((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.Day !=
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Day
                        || ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.Month !=
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Month
                        || ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.Year !=
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END.Value.Year))
                    {
                        MessageBox.Show("Нельзя вводить документ \"Неявки разрешенные законом\" на несколько дней!",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                case 535:
                    if (tbDoc_Location.Text.Trim() == "")
                    {
                        MessageBox.Show("Для данного оправдательного документа необходимо заполнить поле\nМестонахождение!",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbDoc_Location.Focus();
                        return;
                    }
                    break;
                case 533:
                    dateControl = ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_BEGIN.Value.AddHours(4);
                    if (dateControl > ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).DOC_END)
                    {
                        MessageBox.Show("Данный документ должен быть не менее 4 часов!",
                                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    break;
                default:
                    break;
            }

            // Если данный документ не связан с отгулами, а раньше связь была установлена, то удаляем отгул из базы данных
            if (pay_type != 302 && pay_type != 303 && pay_type != 304 && pay_type != 306 && 
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID != null)
            {
                OracleCommand ocDeleteAbsense = new OracleCommand(string.Format(
                    "delete from {0}.ABSENCE where ABSENCE_ID = :p_ABSENCE_ID", Connect.Schema), Connect.CurConnect);
                ocDeleteAbsense.BindByName = true;
                ocDeleteAbsense.Parameters.Add("p_ABSENCE_ID", OracleDbType.Decimal).Value =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID;
                ocDeleteAbsense.ExecuteNonQuery();
                absence.Clear();
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).ABSENCE_ID = null;
            }

            /// Сохранение данных
            ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).TRANSFER_ID = transfer_id;
            ocCountDocs.Parameters["p_DOC_END"].Value =
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_END;
            ocCountDocs.Parameters["p_DOC_BEGIN"].Value =
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_BEGIN;
            ocCountDocs.Parameters["p_DOC_LIST_ID"].Value =
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
            ocCountDocs.Parameters["p_REG_DOC_ID"].Value =
                ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID;
            ocCountDocs.Parameters["p_PAY_TYPE_ID"].Value = pay_type;
            ocCountDocs.Parameters["p_DOC_NOTE"].Value = doc_note;
            int countDocs = Convert.ToInt16(ocCountDocs.ExecuteScalar());
            if (countDocs > 0)
            {
                MessageBox.Show("За указанный период уже существует оправдательный документ!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                absence.Save();
                reg_doc.Save();                
                Connect.Commit();
                if (pay_type == 226 || pay_type == 237 || pay_type == 501 || pay_type == 502 || (pay_type == 210 && doc_note == "MO"))
                {
                    OracleCommand comm = new OracleCommand(string.Format(
                        "begin {0}.UPDATE_VAC_REG_DOC(:tran_id, :start_scan_date); end;",
                        Connect.Schema), Connect.CurConnect);
                    comm.BindByName = true;
                    comm.Parameters.Add("tran_id", OracleDbType.Decimal).Value = transfer_id;
                    comm.Parameters.Add("start_scan_date", OracleDbType.Date).Value = DateTime.Now.AddMonths(-10);
                    comm.ExecuteNonQuery();
                }
                if (pay_type != 226)
                {
                    OracleCommand comm = new OracleCommand(string.Format(
                        "begin {0}.REG_DOC_REGISTR(:p_reg_doc_id, :p_sign); end;",
                        Connect.Schema), Connect.CurConnect);
                    comm.BindByName = true;
                    comm.Parameters.Add("p_reg_doc_id",
                        ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).REG_DOC_ID);
                    comm.Parameters.Add("p_sign", GrantedRoles.GetGrantedRole("TABLE_ACCOUNTANT") ? 1 : 0);
                    comm.ExecuteNonQuery();
                }
                Connect.Commit();
                if (pay_type == 222 || pay_type == 622)
                {
                    OracleCommand com = new OracleCommand("", Connect.CurConnect);
                    com.BindByName = true;
                    com.CommandText = string.Format(
                        "begin {0}.Mission_Control(:p_reg_doc_id, :p_transfer_id); end;",
                        DataSourceScheme.SchemeName);
                    com.Parameters.Add("p_reg_doc_id",
                        ((REG_DOC_obj)(((CurrencyManager)BindingContext[reg_doc]).Current)).REG_DOC_ID);
                    com.Parameters.Add("p_transfer_id", transfer_id);
                    com.ExecuteNonQuery();
                    Connect.Commit();
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void EditReg_Doc_FormClosing(object sender, FormClosingEventArgs e)
        {
            reg_doc.RollBack();
        }

        private void deDoc_Begin_Leave(object sender, EventArgs e)
        {
            if (deDoc_Begin.Date != null)
            {
                if (deDoc_Begin.Date < table.BeginDate)
                {
                    if (pay_type != 303 && pay_type != 304 && pay_type != 306)
                    {
                        if (Connect.UserId.ToUpper() != "BMW12714")
                        {
                            MessageBox.Show("Значение введенной даты начала документа меньше начала текущего периода!" +
                                "\nУстановите дату начала текущего периода.",
                                "АРМ \"Учет рабочего времени\"",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            deDoc_Begin.Focus();
                            return;
                        }
                    }
                }
                else
                    if (deDoc_Begin.Date > table.EndDate)
                    {
                        MessageBox.Show("Дата начала документа должна быть в выбранном периоде табеля!" +
                                "\nУстановите дату из текущего периода.",
                                "АРМ \"Учет рабочего времени\"",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        deDoc_Begin.Focus();
                        return;
                    }
                DateTime date = Convert.ToDateTime(deDoc_Begin.Date);
                TimeSpan ti = DateTime.Now.Subtract(DateTime.Now.AddDays(-366));
                if (DateTime.Now > date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
                {
                    if (MessageBox.Show("Значение введенной даты начала документа больше на год текущей даты!" +
                        "\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        deDoc_Begin.Focus();
                        return;
                    }
                }
                if (DateTime.Now < date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
                {
                    if (MessageBox.Show("Значение введенной даты начала документа меньше на год текущей даты!" +
                        "\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        deDoc_Begin.Focus();
                        return;
                    }
                }
                /*cmDoc_List.Parameters["p_doc_list_id"].Value =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
                OracleDataReader reader = cmDoc_List.ExecuteReader();
                if (reader.Read())*/
                {
                    pay_type = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["pay_type_id"]);
                    doc_note = ((DataRowView)cbDoc_List_Name.SelectedItem)["DOC_NOTE"].ToString();
                    sign_all_day = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["SIGN_ALL_DAY"]);
                }
                //reader.Close();
                if (pay_type == 106 || pay_type == 306/*303*/)
                {
                    DateTime dateDoc = Convert.ToDateTime(deDoc_Begin.Date);
                    deDoc_Begin.Date = dateDoc;
                    deDoc_End.Date = dateDoc;
                    dtSelTimeGraph.Clear();
                    dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = deDoc_Begin.Date;
                    dtSelTimeGraph.Fill();
                    if (dtSelTimeGraph.Rows.Count > 0 && mbDoc_Begin.Text.Trim().Replace(":", "") == "")
                    {
                        mbDoc_Begin.Text = dtSelTimeGraph.Rows[0]["MAX_TIME_END"].ToString();
                    }
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = dateDoc;
                    dtSelPassEvent.Fill();
                    if (dtSelPassEvent.Rows.Count > 0 && mbDoc_End.Text.Trim().Replace(":", "") == "")
                    {
                        mbDoc_End.Text = dtSelPassEvent.Rows[0]["MAX_EVENT_TIME"].ToString();
                    }
                }
                else
                {
                    if (pay_type == 124 || pay_type == 121 || pay_type == 304)
                    {
                        DateTime dateDoc = Convert.ToDateTime(deDoc_Begin.Date);
                        deDoc_Begin.Date = dateDoc;
                        deDoc_End.Date = dateDoc;

                        dtSelPassEvent.Clear();
                        dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num;
                        dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = dateDoc;
                        dtSelPassEvent.Fill();
                        if (dtSelPassEvent.Rows.Count > 0)
                        {
                            if (mbDoc_Begin.Text.Trim().Replace(":", "") == "")
                                mbDoc_Begin.Text = dtSelPassEvent.Rows[0]["MIN_EVENT_TIME"].ToString();
                            if (mbDoc_End.Text.Trim().Replace(":", "") == "")
                                mbDoc_End.Text = dtSelPassEvent.Rows[0]["MAX_EVENT_TIME"].ToString();
                        }
                    }
                    else
                    {
                        if (sign_all_day == 1)
                        {
                            dtSelTimeGraph.Clear();
                            dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = deDoc_Begin.Date;
                            dtSelTimeGraph.Fill();
                            if (dtSelTimeGraph.Rows.Count > 0 && mbDoc_Begin.Text.Trim().Replace(":", "") == "")
                            {
                                mbDoc_Begin.Text = dtSelTimeGraph.Rows[0]["MIN_TIME_BEGIN"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void deDoc_End_Leave(object sender, EventArgs e)
        {
            if (deDoc_End.Date != null)
            {
                DateTime date = Convert.ToDateTime(deDoc_End.Date);
                TimeSpan ti = DateTime.Now.Subtract(DateTime.Now.AddDays(-366));
                if (DateTime.Now > date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
                {
                    if (MessageBox.Show("Значение введенной даты окончания документа меньше на год " +
                        "текущей даты!\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        deDoc_End.Focus();
                        return;
                    }
                }
                if (DateTime.Now < date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
                {
                    if (MessageBox.Show("Значение введенной даты окончания документа больше на год " +
                        "текущей даты!\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        deDoc_End.Focus();
                        return;
                    }
                }
                /*cmDoc_List.Parameters["p_doc_list_id"].Value =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
                OracleDataReader reader = cmDoc_List.ExecuteReader();
                if (reader.Read())*/
                {
                    pay_type = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["pay_type_id"]);
                    doc_note = ((DataRowView)cbDoc_List_Name.SelectedItem)["DOC_NOTE"].ToString();
                    sign_all_day = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["SIGN_ALL_DAY"]);
                }
                //reader.Close();
                if (deDoc_End.Date != null && sign_all_day == 1)
                {
                    dtSelTimeGraph.Clear();
                    dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = deDoc_End.Date;
                    dtSelTimeGraph.Fill();
                    if (dtSelTimeGraph.Rows.Count > 0 && mbDoc_End.Text.Trim().Replace(":", "") == "")
                    {
                        mbDoc_End.Text = dtSelTimeGraph.Rows[0]["MAX_TIME_END"].ToString();
                    }
                }
            }
        }

        private void mbDoc_End_Leave(object sender, EventArgs e)
        {
            try
            {
                /*cmDoc_List.Parameters["p_doc_list_id"].Value =
                    ((REG_DOC_obj)((CurrencyManager)BindingContext[reg_doc]).Current).DOC_LIST_ID;
                OracleDataReader reader = cmDoc_List.ExecuteReader();
                if (reader.Read())*/
                {
                    pay_type = Convert.ToInt32(((DataRowView)cbDoc_List_Name.SelectedItem)["pay_type_id"]);
                }
                //reader.Close();
                if (pay_type == 106 || pay_type == 303 || pay_type == 304 || pay_type == 306) /// эту проверку полностью перенес в процедуру
                {
                    DateTime dateDoc = Convert.ToDateTime(deDoc_End.Date);
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = per_num; //
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = dateDoc;
                    dtSelPassEvent.Fill();
                    DateTime dateDoc2 = new DateTime(1,1,1);
                    if (dtSelPassEvent.Rows.Count > 0)
                    {
                        dateDoc2 = Convert.ToDateTime(dtSelPassEvent.Rows[0]["MAX_EVENT"]); //
                    }                    
                    DateTime dateDoc3 = Convert.ToDateTime(dateDoc.ToShortDateString() + " " +
                        mbDoc_End.Text + ":00");
                    if (dateDoc3 > dateDoc2)
                    {
                        cmRole_GR.Parameters["p_per_num"].Value = per_num;
                        cmRole_GR.Parameters["p_date"].Value = dateDoc;
                        int p_count = Convert.ToInt32(cmRole_GR.ExecuteScalar());
                        if (p_count == 0)
                        {
                            MessageBox.Show("Нельзя вводить время окончания документа, которое позже последнего выхода!",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            mbDoc_End.Focus();
                            return;
                        }                        
                    }
                }
            }
            catch
            { 
                
            }
        }
    }
}