using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Staff;
using Tabel;
using System.Text.RegularExpressions;
using TabDayLibrary;
using LibraryKadr;
using Kadr;
using WpfControlLibrary.Classes;
using System.Windows.Interop;
using Helpers;

namespace Tabel
{
    public partial class Table : Form, IDataLinkKadr
    {
        public OracleDataTable dtEmp;
        public BindingSource bsEmp;
        decimal transfer_id;
        DOC_LIST_seq doc_list;
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
        public int subdiv_id;
        /// <summary>
        /// Код подразделения, с которым сейчас работаем
        /// </summary>
        public string code_subdiv;
        /// <summary>
        /// Наименование подразделения, с которым сейчас работаем
        /// </summary>
        public string subdiv_name;
		public string curPer_num = "";
        /// <summary>
        /// Задает месяц для отображения табеля
        /// </summary>
        public int month;
        /// <summary>
        /// Задает год для отображения табеля
        /// </summary>
        public int year;
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

        object _selectedTab;

        OracleDataTable dtWork_pay_type,dtHoursCalc, dtHoursPeriod,dtWorked_day,dtAbsence, dtWorkOut,
            dtDays_calendar, dtReg_doc, dtEmpErrors, dtHoursSaved; //dtTotalHours, 

        OracleCommand ocCalcSalary, ocClosedTable, ocDateClose, ocDeleteReg_Doc, ocCalc_Absence, _ocDependencyCloseTable,
            _ocTable_Closing_ID, _ocGet_Status_Closing, _ocDependencyTable_Closing, _ocSign_Table_Closing;
        OracleDependency _odCloseTable, _odTable_Closing;
        DataSet _dsTable_Approval;
        OracleDataAdapter _daTable_Approval, _daType_Approval, _daPlan_Approval = new OracleDataAdapter(),
            _daAppendix = new OracleDataAdapter();
        DataRowView _row_Table_Approval, _row_Advance_Approval;
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
        /// <summary>
        /// Конструктор формы отображения списка работников
        /// </summary>
        /// <param name="_connection">Соединение</param>
        /// <param name="_formMenu">Главная форма</param>
        /// <param name="_dtEmp">Список работников</param>
        /// <param name="_emp">Работники</param>
        /// <param name="perNum">Табельный номер</param>
        public Table()
        {
            InitializeComponent();
			cmTabel.EnableByRules(false);
            tsmPerKard.Enabled = true;
			dgEmp.ContextMenuStrip.Items.Add(ListLinkKadr.GetMenuItem(this));
            RefreshDependencyCloseTable();
            RefreshDependency();
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
                        /*_daTransfer_Project.SelectCommand.Parameters["p_SIGN_NOTIFICATION"].Value = 1;
                        _daTransfer_Project.SelectCommand.Parameters["p_TABLE_ID"].Value =
                            eventArgs.Details.Rows.OfType<DataRow>().Select(i => i["ROWID"].ToString()).ToArray();
                        _ds.Tables["PROJECT_TRANSFER_ROW"].Clear();
                        _daTransfer_Project.Fill(_ds.Tables["PROJECT_TRANSFER_ROW"]);
                        _ds.Tables["PROJECT_TRANSFER"].PrimaryKey = new DataColumn[] { _ds.Tables["PROJECT_TRANSFER"].Columns["PROJECT_TRANSFER_ID"] };
                        for (int i = 0; i < _ds.Tables["PROJECT_TRANSFER_ROW"].Rows.Count; i++)
                        {
                            _ds.Tables["PROJECT_TRANSFER"].LoadDataRow(_ds.Tables["PROJECT_TRANSFER_ROW"].Rows[i].ItemArray, LoadOption.OverwriteChanges);
                        }*/                        
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
            ocClosedTable.Parameters["p_subdiv_id"].Value = subdiv_id;
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
            ocDateClose.Parameters["p_subdiv_id"].Value = subdiv_id;
            dCloseTable = Convert.ToDateTime(ocDateClose.ExecuteScalar());
        }

        void RefreshDependency()
        {
            // Команда проверки обновления статуса проекта
            _ocDependencyTable_Closing = new OracleCommand(string.Format(
                @"select TABLE_CLOSING_ID, TABLE_PLAN_APPROVAL_ID from {0}.TABLE_CLOSING
                WHERE SUBDIV_ID = :p_SUBDIV_ID and TABLE_DATE = :p_TABLE_DATE and TYPE_TABLE_ID = 2",
                Connect.Schema), Connect.CurConnect);
            _ocDependencyTable_Closing.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
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
                        if (cbType_Approval_Advance!=null && !cbType_Approval_Advance.IsDisposed
                            //&& cbType_Approval_Advance.IsHandleCreated //&& cbType_Approval_Advance.Handle != null
                            )
                        {
                            try
                            {
                                cbType_Approval_Table.BeginInvoke(new Action(GetTable_Approval));
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
                MessageBox.Show(ex.Message);
            }
        }

        public static decimal HoursAbsence
        {
            get { return hoursAbsence; }
        }
        /// <summary>
        /// Заполнение таблицы списка сотрудников
        /// </summary>
        public void LoadList()
        {            
            flagReload = false;  
            if (dgEmp.CurrentRow != null)
            {
                /// Сохранение позиции               
                curPer_num = dgEmp.CurrentRow.Cells["transfer_id"].Value.ToString();
                dtEmp.Clear();
            }
            /* Дополнительный параметр используется для сортировки списка работников 
             цеха 61, им нужна сортировка по виду производства*/
            dtEmp.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectEmpTable.sql"),
                Staff.DataSourceScheme.SchemeName,
                code_subdiv == "061" ? ", case when CODE_DEGREE = '09' then CODE_FORM_OPERATION else substr(CODE_POS,1,1) end" : ""); 
            dtEmp.SelectCommand.Parameters["beginDate"].Value = BeginDate;
            dtEmp.SelectCommand.Parameters["endDate"].Value = EndDate;
            dtEmp.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            //dtEmp.SelectCommand.Parameters.Add("endTable", OracleDbType.Date).Value =
            //    endDate < DateTime.Now ? endDate : DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);            
            dtEmp.Fill();
            dgEmp.SelectionChanged += new EventHandler(dgEmp_SelectionChanged);
            flagReload = true;
            lbEmpCount.Text = dtEmp.Rows.Count.ToString();
            if (curPer_num != "")
            {
                foreach (DataGridViewRow row in dgEmp.Rows)
                {
                    if (row.Cells["transfer_id"].Value.ToString() == curPer_num)
                    {
                        flagReload = true;
                        if (bsEmp.Position == row.Index)
                            dgEmp_SelectionChanged(null, null);
                        bsEmp.Position = row.Index;
                        break;
                    }
                }
            }
            tbSubdiv.Text = code_subdiv;

            GetTable_Approval();
        }
        
        /// <summary>
        /// Создание объекта отображения кнопок на каждый день месяца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VisibleDay()
        {
            /// Отображаем панель оправдательных документов
            pnReg_Doc.Visible = true;
            tcTable.Visible = true;
            /// Если объект создан
            if (TDG != null)
            {
                /// Удаляем его из памяти
                TDG.Dispose();                
            }
            /// Создаем объект
            TDG = new TabDayGrid(pnTable);
            /// Настраиваем его вид
            TDG.Height = 400;
            TDG.Font = new Font(this.Font.Name, 10);
            TDG.Dock = DockStyle.Fill;
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
                    TDG[Convert.ToInt32(dtDays_calendar.Rows[i][0]) - 1].DayWeekColor = Color.Green;
                }
                else
                {
                    TDG[Convert.ToInt32(dtDays_calendar.Rows[i][0]) - 1].DayWeekColor = Color.DarkRed;
                }
            }
            TDG.GridCaption = "Табель за период с " + BeginDate.ToShortDateString() +
                " по " + EndDate.ToShortDateString();
            flagVisible = true;
            TDG[0].Focus();
            object sender = new object();
            EventArgs e = new EventArgs();
            tslCaption.Enabled = true;
        }

        /// <summary>
        /// Настройка грида списка работников
        /// </summary>
        void RefGrid()
        {      
            dgEmp.Columns["per_num"].HeaderText = "Таб.№";
            dgEmp.Columns["per_num"].Width = 55;
            //dgEmp.Columns["per_num"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["OKL"].HeaderText = "Оклад / надбавки";
            dgEmp.Columns["OKL"].Width = 100;
            //dgEmp.Columns["OKL"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["OKL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgEmp.Columns["EMP_LAST_NAME"].HeaderText = "Фамилия";
            dgEmp.Columns["EMP_LAST_NAME"].Width = 100;
            //dgEmp.Columns["emp_last_name"].SortMode = DataGridViewColumnSortMode.NotSortable;            
            dgEmp.Columns["EMP_FIRST_NAME"].HeaderText = "Имя";
            dgEmp.Columns["EMP_FIRST_NAME"].Width = 100;
            //dgEmp.Columns["emp_first_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["EMP_MIDDLE_NAME"].HeaderText = "Отчество";
            dgEmp.Columns["EMP_MIDDLE_NAME"].Width = 100;
            //dgEmp.Columns["emp_middle_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_pos"].HeaderText = "Шифр профессии";
            dgEmp.Columns["code_pos"].Width = 80;
            //dgEmp.Columns["code_pos"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["pos_name"].HeaderText = "Наименование профессии";
            dgEmp.Columns["pos_name"].Width = 500;
            //dgEmp.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["order_name"].HeaderText = "Заказ";
            dgEmp.Columns["order_name"].Width = 90;
            //dgEmp.Columns["order_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["order_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["CODE_DEGREE"].HeaderText = "Кат.";
            dgEmp.Columns["CODE_DEGREE"].Width = 30;
            //dgEmp.Columns["CODE_DEGREE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["CODE_DEGREE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["GROUP_MASTER"].HeaderText = "Группа мастера";
            dgEmp.Columns["GROUP_MASTER"].Width = 70;
            //dgEmp.Columns["GROUP_MASTER"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["GROUP_MASTER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["COMB"].HeaderText = "С.";
            dgEmp.Columns["COMB"].Width = 20;
            //dgEmp.Columns["COMB"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["COMB"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["COMB"].DisplayIndex = 6;
            dgEmp.Columns["RN"].HeaderText = "№ п/п";
            dgEmp.Columns["RN"].Width = 30;
            dgEmp.Columns["RN"].DisplayIndex = 0;
            dgEmp.Columns["FL_WAYBILL"].HeaderText = "П.Л.";
            dgEmp.Columns["FL_WAYBILL"].Width = 30;            
            dgEmp.Columns["FL_WAYBILL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["CODE_FORM_OPERATION"].HeaderText = "В.П.";
            dgEmp.Columns["CODE_FORM_OPERATION"].Width = 30;
            dgEmp.Columns["CODE_FORM_OPERATION"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["sign_comb"].Visible = false;
            dgEmp.Columns["transfer_id"].Visible = false;
            dgEmp.Columns["worker_id"].Visible = false;
            dgEmp.Columns["degree_id"].Visible = false;
            //dgEmp.Columns["subdiv_id"].Visible = false;
            dgEmp.Columns["ispink"].Visible = false;
            //dgEmp.Columns["CODE_DEGREE"].Visible = false;
            dgEmp.Columns["FIO"].Visible = false;
            dgEmp.Columns["DATE_HIRE"].Visible = false;
            dgEmp.Columns["DATE_TRANSFER"].Visible = false;
            dgEmp.Columns["DATE_DISMISS"].Visible = false;
            dgEmp.Columns["FL_END_DOG"].Visible = false;
        }

        /// <summary>
        /// Настройка грида списка оправдательных документов 
        /// </summary>
        void RefReg_Doc()
        {
            dgReg_Doc.Columns["REG_DOC_ID"].Visible = false;
            dgReg_Doc.Columns["PAY_TYPE_ID"].Visible = false;
            dgReg_Doc.Columns["ABSENCE_ID"].Visible = false;
            dgReg_Doc.Columns["TRANSFER_ID"].Visible = false;
            dgReg_Doc.Columns["Дата начала"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            dgReg_Doc.Columns["Дата окончания"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            foreach (DataGridViewColumn col in dgReg_Doc.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }         
        }

        /// <summary>
        /// Настройка грида списка отгулов
        /// </summary>
        void RefGridAbsence()
        {
            dgAbsence.Columns["ABS_TIME_BEGIN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgAbsence.Columns["ABS_TIME_END"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgAbsence.Columns["TYPE_ABSENCE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgAbsence.Columns["TIME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgAbsence.Columns["TIME_DEC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgAbsence.Columns["ABS_TIME_BEGIN"].Width = 110;
            dgAbsence.Columns["ABS_TIME_END"].Width = 110;
            dgAbsence.Columns["TYPE_ABSENCE"].Width = 80;
            dgAbsence.Columns["TIME"].Width = 110;
            dgAbsence.Columns["TIME_DEC"].Width = 80;
            dgAbsence.Columns["ABS_TIME_BEGIN"].HeaderText = "Начало документа";
            dgAbsence.Columns["ABS_TIME_END"].HeaderText = "Окончание документа";
            dgAbsence.Columns["TYPE_ABSENCE"].HeaderText = "Тип";
            dgAbsence.Columns["TIME"].HeaderText = "Время по документу";
            dgAbsence.Columns["TIME_DEC"].HeaderText = "Время";
            dgAbsence.Columns["ABSENCE_ID"].Visible = false;
        }

        void RefGridSalary()
        {
            dgHoursCalc.Columns[0].Width = 100;
            dgHoursCalc.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[1].Width = 55;
            dgHoursCalc.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[2].Width = 90;
            dgHoursCalc.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[3].Width = 115;
            dgHoursCalc.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[4].Width = 35;
            dgHoursCalc.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[5].Width = 30;
            dgHoursCalc.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[6].Width = 30;
            dgHoursCalc.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[7].Width = 55;
            dgHoursCalc.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[8].Width = 55;
            dgHoursCalc.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns[9].Width = 65;
            dgHoursCalc.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursCalc.Columns["SIGN_VISIBLE"].Visible = false;
            dgHoursCalc.Columns["SIGN_APPENDIX"].Visible = false;

            dgHoursSaved.Columns[0].Width = 100;
            dgHoursSaved.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[1].Width = 55;
            dgHoursSaved.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[2].Width = 90;
            dgHoursSaved.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[3].Width = 115;
            dgHoursSaved.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[4].Width = 35;
            dgHoursSaved.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[5].Width = 30;
            dgHoursSaved.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[6].Width = 30;
            dgHoursSaved.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[7].Width = 55;
            dgHoursSaved.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[8].Width = 55;
            dgHoursSaved.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns[9].Width = 65;
            dgHoursSaved.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgHoursSaved.Columns["SIGN_VISIBLE"].Visible = false;
            dgHoursSaved.Columns["SIGN_APPENDIX"].Visible = false;
        }

        /// <summary>
        /// Добавление оправдательного документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddReg_Doc_Click(object sender, EventArgs e)
        {
            // 22.06.2016 - убираем проверку в коде, т.к. переходим на процедуру, где и будет проверка
            //if (!flagClosedTable)
            {
                /*string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                decimal transfer_id = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                EditReg_Doc editReg_doc = new EditReg_Doc(true, per_num,
                    subdiv_id, transfer_id, this, 0, Convert.ToDecimal(dgEmp.CurrentRow.Cells["degree_id"].Value), 
                    Convert.ToDecimal(dgEmp.CurrentRow.Cells["WORKER_ID"].Value));
                if (RestoreItem == null)
                    RestoreItem = TDG[Convert.ToDateTime(dtWorked_day.Rows[0]["WORK_DATE"]).Day - 1];
                editReg_doc.Text = "Добавление оправдательных документов на: " +
                    RestoreItem.DateOfDay.ToShortDateString();
                DialogResult rez = editReg_doc.ShowDialog();
                if (rez == DialogResult.OK)
                {
                    TabDayItem item = RestoreItem;
                    FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                        Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                            dgEmp.CurrentRow.Cells["date_hire"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                            (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                            dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                            dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                    if (dtEmpErrors.Rows.Count == 0)
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                    }
                    else
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                    }
                    dgEmp_SelectionChanged(sender, e);
                    RestoreItem = item;
                    // Проверяем имеет ли фокус выбранный день. Если нет, то ставим на него фокус.
                    // Если имеет, то перезаполняем оправдательные документы
                    TDG_Enter(RestoreItem);
                    TDG[RestoreItem.DateOfDay.Day - 1].Focus();*/
                string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                decimal transfer_id = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                object waybill = dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value;
                WpfControlLibrary.Table.EditRegDoc editReg_doc = new WpfControlLibrary.Table.EditRegDoc(transfer_id, null, !(waybill==DBNull.Value || waybill.ToString()=="X"));
                WindowInteropHelper helper = new WindowInteropHelper(editReg_doc);
                helper.Owner = this.Handle;
                if (RestoreItem == null)
                    RestoreItem = TDG[Convert.ToDateTime(dtWorked_day.Rows[0]["WORK_DATE"]).Day - 1];
                editReg_doc.Title = "Добавление документа на " + RestoreItem.DateOfDay.ToShortDateString();
                if (editReg_doc.ShowDialog() == true)
                {
                    TabDayItem item = RestoreItem;
                    FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                        Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                            dgEmp.CurrentRow.Cells["date_hire"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                            (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                            dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                            dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                    if (dtEmpErrors.Rows.Count == 0)
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                    }
                    else
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                    }
                    dgEmp_SelectionChanged(sender, e);
                    RestoreItem = item;
                    try
                    {
                        // Проверяем имеет ли фокус выбранный день. Если нет, то ставим на него фокус.
                        // Если имеет, то перезаполняем оправдательные документы
                        TDG_Enter(RestoreItem);
                        TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                    }
                    catch
                    { }
                }
            }
            //else
            //{
            //    MessageBox.Show("Нельзя добавлять документы за прошлые периоды после закрытия табеля!",
            //        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            //}
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
                /*string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                decimal transfer_id = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                decimal reg_doc_id = Convert.ToDecimal(dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value);
                EditReg_Doc editReg_doc = new EditReg_Doc(false, per_num, subdiv_id,
                    transfer_id, this, reg_doc_id, Convert.ToDecimal(dgEmp.CurrentRow.Cells["degree_id"].Value),
                    Convert.ToDecimal(dgEmp.CurrentRow.Cells["WORKER_ID"].Value));
                editReg_doc.Text = "Редактирование оправдательного документа на: " + RestoreItem.DateOfDay.ToShortDateString();
                DialogResult rez = editReg_doc.ShowDialog();
                if (rez == DialogResult.OK)
                {
                    TabDayItem item = RestoreItem;
                    FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                        Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                            dgEmp.CurrentRow.Cells["date_hire"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                            (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                            dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                            dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                    if (dtEmpErrors.Rows.Count == 0)
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                    }
                    else
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                    }
                    dgEmp_SelectionChanged(sender, e);
                    RestoreItem = item;
                    TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                }*/
                string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                decimal transfer_id = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                decimal reg_doc_id = Convert.ToDecimal(dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value);
                object waybill = dgEmp.CurrentRow.Cells["FL_WAYBILL"].Value;
                WpfControlLibrary.Table.EditRegDoc editReg_doc = 
                    new WpfControlLibrary.Table.EditRegDoc(transfer_id, reg_doc_id, !(waybill==DBNull.Value || waybill.ToString()=="X"));
                WindowInteropHelper helper = new WindowInteropHelper(editReg_doc);
                helper.Owner = this.Handle;
                editReg_doc.Title = "Редактирование документа на " + RestoreItem.DateOfDay.ToShortDateString();
                if (editReg_doc.ShowDialog() == true)
                {
                    TabDayItem item = RestoreItem;
                    FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                        Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                            dgEmp.CurrentRow.Cells["date_hire"].Value),
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                            (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                            dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                            dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                    if (dtEmpErrors.Rows.Count == 0)
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                    }
                    else
                    {
                        dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                    }
                    dgEmp_SelectionChanged(sender, e);
                    try
                    {
                        RestoreItem = item;
                        TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                    }
                    catch
                    {}
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
                if (MessageBox.Show("Удалить документ?", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleCommand cmd = new OracleCommand(string.Format("begin {0}.REG_DOC_DELETEN(:p_reg_doc_id);end;", Connect.Schema), Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal, dgReg_Doc.CurrentRow.Cells["reg_doc_id"].Value, ParameterDirection.Input);
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
                        MessageBox.Show(this, ex.GetFormattedException(), "Ошибка удаления документа");
                    }
                    if (fl) // если строка удалилась то 
                    {
                        try // пытаемся обновить день и список документов
                        {
                            TabDayItem item = RestoreItem;
                            FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                                Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                                Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                                    dgEmp.CurrentRow.Cells["date_hire"].Value),
                                Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                                    (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                                    dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                                    dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                            if (dtEmpErrors.Rows.Count == 0)
                            {
                                dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                            }
                            else
                            {
                                dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                            }
                            dgEmp_SelectionChanged(sender, e);
                            RestoreItem = item;
                            TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                            if (dgReg_Doc.CurrentRow != null)
                                dgReg_Doc.Rows.Remove(dgReg_Doc.CurrentRow);
                        }
                        catch
                        { }
                    }
                }
           
                /*if (!(flagClosedTable ||
                    Convert.ToDateTime(dgReg_Doc.CurrentRow.Cells["Дата начала"].Value) < dCloseTable)
                    || dgReg_Doc.CurrentRow.Cells["PAY_TYPE_ID"].Value.ToString() == "303" ||
                    GrantedRoles.GetGrantedRole("STAFF_VS_CLOSING")) 
                {                    
                    if (dgReg_Doc.CurrentRow.Cells["PAY_TYPE_ID"].Value.ToString() == "226" &&
                        !GrantedRoles.GetGrantedRole("STAFF_VS_CLOSING"))
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
                        TabDayItem item = RestoreItem;
                        FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                            Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                            Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                                dgEmp.CurrentRow.Cells["date_hire"].Value),
                            Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                                (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                                dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                                dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                        if (dtEmpErrors.Rows.Count == 0)
                        {
                            dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                        }
                        else
                        {
                            dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                        }
                        dgEmp_SelectionChanged(sender, e);
                        RestoreItem = item;
                        TDG[RestoreItem.DateOfDay.Day - 1].Focus();
                        if (dgReg_Doc.CurrentRow != null)
                            dgReg_Doc.Rows.Remove(dgReg_Doc.CurrentRow);
                    }
                }
                else
                {
                    MessageBox.Show("Нельзя удалять документы за прошлые периоды после закрытия табеля!",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }*/
            }
            
        }

        Work_Pay_Type work_pay_type;
        /// <summary>
        /// Нажатие кнопки просмотра отработанного времени по видам оплат
        /// </summary>
        /// <param name="Item">Выбранный день</param>
        public void TDG_OnItemBtnClick(TabDayItem Item)
        {
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
                work_pay_type = new Work_Pay_Type(dgEmp.CurrentRow.Cells["per_num"].Value.ToString(),
                    worked_day_id, transfer_id, Item.DateOfDay, this, doc_list,
                    Convert.ToDecimal(dgEmp.CurrentRow.Cells["degree_id"].Value),
                    Convert.ToDecimal(dgEmp.CurrentRow.Cells["WORKER_ID"].Value));
                work_pay_type.ShowDialog();                    
                FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                    Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                    Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                        dgEmp.CurrentRow.Cells["date_hire"].Value),
                    Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                        (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                        dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                        dgEmp.CurrentRow.Cells["date_dismiss"].Value));
                if (dtEmpErrors.Rows.Count == 0)
                {
                    dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "0";
                }
                else
                {
                    dtEmp.Rows[Convert.ToInt32(dgEmp.CurrentRow.Cells["RN"].Value) - 1]["IsPink"] = "1";
                }
                TabDayItem storeItem = RestoreItem;
                object sender = new object();
                EventArgs e = new EventArgs();
                dgEmp_SelectionChanged(sender, e);
                TDG[Item.DateOfDay.Day - 1].Focus();    
            }
            else
            {
                MessageBox.Show("Невозможно отобразить данные за выбранный день!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
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

        /// <summary>
        /// Получение фокуса каким-то днем табеля
        /// </summary>
        /// <param name="Item">Выбранный день</param>
        public void TDG_Enter(TabDayItem Item)
        {
            /// Если показываем табель на месяц и выбранная строка есть
            if (flagVisible && dgEmp.CurrentRow != null)
            {
                decimal trans_ID = 0;
                for (int i = 0; i < dtWorked_day.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(dtWorked_day.Rows[i]["work_date"]).ToShortDateString() ==
                        Item.DateOfDay.ToShortDateString())
                    {
                        trans_ID = Convert.ToDecimal(dtWorked_day.Rows[i]["transfer_id"]);
                        break;
                    }
                }
                RestoreItem = Item;
                string st = RestoreItem.Controls[2].Text;
                RestoreItem.Controls[2].BackColor = Color.Aqua;
                tslCaption.Text = "Оправдательные документы на: " + Item.DateOfDay.ToShortDateString();                
                dtReg_doc.Clear();
                dtReg_doc.SelectCommand.Parameters["p_per_num"].Value = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                dtReg_doc.SelectCommand.Parameters["p_transfer_id"].Value = trans_ID;
                dtReg_doc.SelectCommand.Parameters["p_date"].Value = Item.DateOfDay;
                dtReg_doc.Fill();
            }
        }

        /// <summary>
        /// Вывод формы работника и его привилегий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmEmp_Authority_Click(object sender, EventArgs e)
        {
            /// Думаю все интуитивно понятно в представленном коде
            string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
            string last_name = dgEmp.CurrentRow.Cells["EMP_LAST_NAME"].Value.ToString();
            string first_name = dgEmp.CurrentRow.Cells["EMP_FIRST_NAME"].Value.ToString();
            string middle_name = dgEmp.CurrentRow.Cells["EMP_MIDDLE_NAME"].Value.ToString();
            string pos_name = dgEmp.CurrentRow.Cells["POS_NAME"].Value.ToString();
            decimal transfer = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
            EditEmp ep = new EditEmp(per_num, transfer, Convert.ToDecimal(dgEmp.CurrentRow.Cells["worker_id"].Value),
                last_name, first_name, middle_name, pos_name,
                subdiv_id, Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value),
                dgEmp.CurrentRow.Cells["ORDER_NAME"].Value.ToString(),
                dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                dgEmp.CurrentRow.Cells["GROUP_MASTER"].Value.ToString(), EndDate);
            ep.ShowDialog();
        }

        /// <summary>
        /// Изменение позиции в гриде работников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dgEmp_SelectionChanged(object sender, EventArgs e)
        {
            /// Если показываем табель на месяц и выбранная строка есть
            if (flagVisible && dgEmp.CurrentRow != null && flagReload)
            {
                for (int p = 0; p < EndDate.Day; p++)
                {
                    TDG[p].NoteText = "";
                    TDG[p].Text = "";
                    TDG[p].BackColor = Color.FromArgb(240, 240, 240);
                }
                transfer_id = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                dtWorked_day.Clear();
                dtWorked_day.SelectCommand.Parameters["p_per_num"].Value = 
                    dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                dtWorked_day.SelectCommand.Parameters["p_date_begin"].Value =
                    dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                    dgEmp.CurrentRow.Cells["date_hire"].Value;
                dtWorked_day.SelectCommand.Parameters["p_date_end"].Value = 
                    dgEmp.CurrentRow.Cells["date_transfer"].Value == DBNull.Value ? 
                    dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ? EndDate :
                    dgEmp.CurrentRow.Cells["date_dismiss"].Value :
                    Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_transfer"].Value).AddDays(-1);
                //dtWorked_day.SelectCommand.Parameters["p_transfer_id"].Value = transfer_id;
                dtWorked_day.SelectCommand.Parameters["p_WORKER_ID"].Value = dgEmp.CurrentRow.Cells["WORKER_ID"].Value;
                /// Заполняем рабочие дни для выбранного работника
                dtWorked_day.Fill();  
                /// Новый вариант
                foreach (DataRow row in dtWorked_day.Rows)
                {
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].NoteText = row["FROM_PERCO"].ToString();
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Text = row["NOTE"].ToString();
                    if (Convert.ToInt32(row["ISPINK"]) == 1)
                    {
                        TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].BackColor = Color.Pink;
                    }                    
                    TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Width =
                        Math.Max(TDG[Convert.ToDateTime(row["WORK_DATE"]).Day - 1].Text.Length * 10, 56);
                }
                Absence(dgEmp.CurrentRow.Cells["per_num"].Value.ToString(), Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value));
                // Если выбрана вкладка Табель или Отгулы не нужно считать другие вкладки
                if (tcTable.SelectedTab.Name == tpTable.Name || tcTable.SelectedTab.Name == tpAbsence.Name)
                {

                }
                else
                {
                    // Если выбрана вкладка Итоги, считаем итоги
                    if (tcTable.SelectedTab.Name == tpHours.Name)
                    {
                        Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                            Convert.ToInt16(dgEmp.CurrentRow.Cells["sign_comb"].Value.ToString()),
                            BeginDate, EndDate);
                        Fill_Salary_Kept(Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                            dgEmp.CurrentRow.Cells["per_num"].Value.ToString(),
                            BeginDate, EndDate);
                    }
                    else
                    {
                        // Если выбрана вкладка Работа за территорией - заполняем документы
                        if (tcTable.SelectedTab.Name == tpWorkOut.Name)
                        {
                            WorkOut(dgEmp.CurrentRow.Cells["per_num"].Value.ToString());
                        }                        
                    }
                }
                lbHire.Text = lbTrans.Text = lbDism.Text = "";
                if (dgEmp.CurrentRow.Cells["date_hire"].Value != DBNull.Value)
                    lbHire.Text = "Принят: " +
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value).ToShortDateString();
                if (dgEmp.CurrentRow.Cells["date_transfer"].Value != DBNull.Value)
                    lbTrans.Text = "Переведен: " +
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_transfer"].Value).ToShortDateString();
                if (dgEmp.CurrentRow.Cells["date_dismiss"].Value != DBNull.Value)
                    lbDism.Text = "Уволен: " +
                        Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value).ToShortDateString();
            }
        }

        /// <summary>
        /// Метод рассчитывает количество часов отгулов для сотрудника
        /// </summary>
        /// <param name="_per_num">Табельный номер</param>
        void Absence(string _per_num, decimal _trans_id)
        {
            dtAbsence.Clear();
            dtAbsence.SelectCommand.Parameters["p_per_num"].Value = _per_num;
            dtAbsence.Fill();
            ocCalc_Absence.Parameters["p_transfer_id"].Value = _trans_id;
            ocCalc_Absence.ExecuteNonQuery();
            hoursAbsence = ((OracleDecimal)ocCalc_Absence.Parameters["p_hours"].Value).Value;
            decimal hours = Math.Truncate(Math.Abs(hoursAbsence));
            decimal min = hoursAbsence - Math.Truncate(hoursAbsence);                
            tpAbsence.Text = string.Format("Отгулы. Доступно {0}{1}:{2}", hoursAbsence < 0 ? "-" : "", hours, 
                Math.Round(Math.Abs(min) * 60, 0).ToString().PadLeft(2,'0'));
        }

        /// <summary>
        /// Расчет итоговых данных по видам оплат для табельного
        /// </summary>
        /// <param name="per_num"></param>
        void Calc_Hours(OracleDataTable _dtHours, decimal _transfer_id, int _sign_comb, DateTime _beginDate, DateTime _endDate)
        {
            _dtHours.Clear();            
            //ocCalcSalary.Parameters["p_subdiv_id"].Value = subdiv_id;
            //ocCalcSalary.Parameters["p_beginDate"].Value = _beginDate;
            //ocCalcSalary.Parameters["p_endDate"].Value = _endDate;
            //ocCalcSalary.Parameters["p_transfer_id"].Value = _transfer_id;
            //ocCalcSalary.Parameters["p_sign_calc"].Value = 1;
            //ocCalcSalary.Parameters["p_temp_salary_id"].Value = null;
            //ocCalcSalary.ExecuteNonQuery();
            //_dtHours.SelectCommand.Parameters[0].Value = ocCalcSalary.Parameters["p_temp_salary_id"].Value;
            //_dtHours.SelectCommand.Parameters[1].Value = _sign_comb;      
            _dtHours.SelectCommand.Parameters["p_SUBDIV_ID"].Value = subdiv_id;
            _dtHours.SelectCommand.Parameters["p_BEGINDATE"].Value = _beginDate;
            _dtHours.SelectCommand.Parameters["p_ENDDATE"].Value = _endDate;
            _dtHours.SelectCommand.Parameters["p_TRANSFER_ID"].Value = _transfer_id;
            _dtHours.SelectCommand.Parameters["p_sign_calc"].Value = 1;
            try
            {
                _dtHours.Fill();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка расчета итогов:\n"+ex.Message,
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void Fill_Salary_Kept(int transfer_id, string per_num, DateTime beginDate, DateTime endDate)
        {
            dtHoursSaved.Clear();
            tcHours.TabPages.Clear();
            tcHours.TabPages.Add(tpHoursCalc);
            dtHoursSaved.SelectCommand.Parameters["p_per_num"].Value = per_num;
            dtHoursSaved.SelectCommand.Parameters["p_date_begin"].Value = beginDate;
            dtHoursSaved.SelectCommand.Parameters["p_date_end"].Value = endDate;
            dtHoursSaved.SelectCommand.Parameters["p_transfer_id"].Value = transfer_id;
            dtHoursSaved.Fill();
            if (dtHoursSaved.Rows.Count > 0)
                tcHours.TabPages.Add(tpHoursSaved);
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
        /// <summary>
        /// Изменение цвета строки таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgEmp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если по сотруднику нет баланса
            if (dgEmp["IsPink", e.RowIndex].Value.ToString() == "1")
            {
                /// Красим в розовый цвет
                e.CellStyle.BackColor = Color.Pink;
            }
            /// Если по сотруднику заканчивается трудовой договор
            switch (Convert.ToInt16(dgEmp["FL_END_DOG", e.RowIndex].Value))
            {
                case 1:
                    /// Красим в голубой цвет
                    dgEmp["PER_NUM", e.RowIndex].Style.BackColor = Color.LightBlue;
                    dgEmp["EMP_LAST_NAME", e.RowIndex].Style.BackColor = Color.LightBlue;
                    dgEmp["EMP_FIRST_NAME", e.RowIndex].Style.BackColor = Color.LightBlue;
                    dgEmp["EMP_MIDDLE_NAME", e.RowIndex].Style.BackColor = Color.LightBlue;
                    break;
                case 2:
                    /// Красим в серобуромалиновый цвет
                    dgEmp["PER_NUM", e.RowIndex].Style.BackColor = Color.FromArgb(128, 128, 255);
                    dgEmp["EMP_LAST_NAME", e.RowIndex].Style.BackColor = Color.FromArgb(128, 128, 255);
                    dgEmp["EMP_FIRST_NAME", e.RowIndex].Style.BackColor = Color.FromArgb(128, 128, 255);
                    dgEmp["EMP_MIDDLE_NAME", e.RowIndex].Style.BackColor = Color.FromArgb(128, 128, 255);
                    break;
                default:
                    break;
            }
        }        

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _odCloseTable.OnChange -= d_OnChange;
                _odCloseTable.RemoveRegistration(Connect.CurConnect);
            }
            catch { }
            if (((FormMain)(this.ParentForm)).MdiChildren.Where(i =>
                i.Name.ToUpper() == "TABLE").Count() < 2)
            {
                ((FormMain)(this.ParentForm)).rgOperation.Visible = false;
                ((FormMain)(this.ParentForm)).rgOrders.Visible = false;
            }
        }

        /// <summary>
        /// Вывод формы работника и его привилегий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmPerKard_Click(object sender, EventArgs e)
        {
            /// Думаю все интуитивно понятно в представленном коде
            string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
            string last_name = dgEmp.CurrentRow.Cells["EMP_LAST_NAME"].Value.ToString();
            string first_name = dgEmp.CurrentRow.Cells["EMP_FIRST_NAME"].Value.ToString();
            string middle_name = dgEmp.CurrentRow.Cells["EMP_MIDDLE_NAME"].Value.ToString();
            string pos_name = dgEmp.CurrentRow.Cells["POS_NAME"].Value.ToString();
            decimal transfer = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
            EditEmp editEmp = new EditEmp(per_num, transfer, Convert.ToDecimal(dgEmp.CurrentRow.Cells["worker_id"].Value),
                last_name, first_name, middle_name, pos_name,
                subdiv_id, Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value),
                dgEmp.CurrentRow.Cells["ORDER_NAME"].Value.ToString(),
                dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(), 
                dgEmp.CurrentRow.Cells["GROUP_MASTER"].Value.ToString(), EndDate);
            editEmp.ShowDialog(this);
        }

        /// <summary>
        /// Выбор месяца из комбобокса отображаемого табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = cbMonth.SelectedIndex + 1;
            BeginDate = DateTime.Parse("01." + month.ToString() + "." + nudYear.Value.ToString());
            string time1 = DateTime.DaysInMonth((int)nudYear.Value, month).ToString() + "." + month.ToString() + "." + nudYear.Value.ToString() + " 23:59:59";
            EndDate = DateTime.Parse(time1);
            RefreshFlagClosedTable();
            dgEmp.SelectionChanged -= dgEmp_SelectionChanged;
            if (flagVisible)
                VisibleDay();
            LoadList();            
        }

        /// <summary>
        /// Ввод года отображаемого табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudYear_ValueChanged(object sender, EventArgs e)
        {
            int month = cbMonth.SelectedIndex + 1;
            BeginDate = DateTime.Parse("01." + month.ToString() + "." + nudYear.Value.ToString());
            EndDate = DateTime.Parse(DateTime.DaysInMonth((int)nudYear.Value, month).ToString() + "."
                + month.ToString() + "." + nudYear.Value.ToString() + " 23:59:59");
            RefreshFlagClosedTable();
            dgEmp.SelectionChanged -= dgEmp_SelectionChanged;
            if (flagVisible)
                VisibleDay();
            LoadList();  
        }

        /// <summary>
        /// Редактирование личных данных работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgEmp_DoubleClick(object sender, EventArgs e)
        {
            if (dgEmp.CurrentRow != null)
            {
                /// Думаю, что все интуитивно понятно в представленном коде
                string per_num = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                string last_name = dgEmp.CurrentRow.Cells["EMP_LAST_NAME"].Value.ToString();
                string first_name = dgEmp.CurrentRow.Cells["EMP_FIRST_NAME"].Value.ToString();
                string middle_name = dgEmp.CurrentRow.Cells["EMP_MIDDLE_NAME"].Value.ToString();
                string pos_name = dgEmp.CurrentRow.Cells["POS_NAME"].Value.ToString();
                decimal transfer = Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value);
                EditEmp editEmp = new EditEmp(per_num, transfer, Convert.ToDecimal(dgEmp.CurrentRow.Cells["worker_id"].Value),
                    last_name, first_name, middle_name, pos_name,
                    subdiv_id, Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value),
                    dgEmp.CurrentRow.Cells["ORDER_NAME"].Value.ToString(), 
                    dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                    dgEmp.CurrentRow.Cells["GROUP_MASTER"].Value.ToString(), EndDate);
                editEmp.ShowDialog(this);
                Calc_Hours(dtHoursCalc, transfer, Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value), BeginDate, EndDate);
            }
        }

        private void btEditOrder_Click(object sender, EventArgs e)
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

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dCloseTable > BeginDate)
            {
                MessageBox.Show("Нельзя редактировать заказ за прошедший период!", 
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (mbCountHours.Text.Trim().Replace(",","") == "" || mbCountHours.Text == null)
            {
                MessageBox.Show("Вы не ввели количество часов!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbCountHours.Focus();
                return;
            }
            if (tbOrder_Name.Text == "" || tbOrder_Name.Text == null)
            {
                MessageBox.Show("Вы не ввели номер заказа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbOrder_Name.Focus();
                return;
            }
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.ReplaceOrder(:p_per_num, :p_transfer_id, :p_countHours, :p_beginDate, " +
                ":p_endDate, :p_order_id, :p_subdiv_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            com.Parameters["p_per_num"].Value = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
            com.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            com.Parameters["p_transfer_id"].Value = transfer_id;
            com.Parameters.Add("p_countHours", OracleDbType.Decimal);
            com.Parameters["p_countHours"].Value = Convert.ToDecimal(mbCountHours.Text) * 3600;
            com.Parameters.Add("p_beginDate", OracleDbType.Date);
            com.Parameters["p_beginDate"].Value = BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date);
            com.Parameters["p_endDate"].Value = EndDate;
            com.Parameters.Add("p_order_id", OracleDbType.Decimal);
            com.Parameters["p_order_id"].Value = order_id;
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            com.Parameters["p_subdiv_id"].Value = subdiv_id;
            com.ExecuteNonQuery();
            Connect.Commit();
            MessageBox.Show("Данные сохранены!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            mbCountHours.Text = "";
            tbOrder_Name.Text = "";
            Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value),
                BeginDate, EndDate);
        }

        private void tpHours_Enter(object sender, EventArgs e)
        {
            if (_selectedTab != tcTable.SelectedTab)
            {
                _selectedTab = tpHours;
                Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                    Convert.ToInt16(dgEmp.CurrentRow.Cells["sign_comb"].Value.ToString()),
                    BeginDate, EndDate);
                Fill_Salary_Kept(Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                    dgEmp.CurrentRow.Cells["per_num"].Value.ToString(),
                    BeginDate, EndDate);
            }
        }

        private void tpWorkOut_Enter(object sender, EventArgs e)
        {
            WorkOut(dgEmp.CurrentRow.Cells["per_num"].Value.ToString());
        }

        private void Table_Load(object sender, EventArgs e)
        {
            dtEmp = new OracleDataTable("", Connect.CurConnect);
            dtEmp.SelectCommand.Parameters.Add("beginDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("endDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ((FormMain)(this.ParentForm)).rgOperation.Visible = true;
            ((FormMain)(this.ParentForm)).rgOrders.Visible = true;
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;
            dgEmp.DataSource = bsEmp;

            ocClosedTable = new OracleCommand("", Connect.CurConnect);
            ocClosedTable.BindByName = true;
            ocClosedTable.CommandText = string.Format(Queries.GetQuery("Table/SelectClosedTable.sql"),
                DataSourceScheme.SchemeName);
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
            dgAdvance_Approval.DataSource = _dsTable_Approval.Tables["ADVANCE_APPROVAL"];
            dgTable_Approval.DataSource = _dsTable_Approval.Tables["TABLE_APPROVAL"];
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
            dcTABLE_PLAN_APPROVAL_ID.DataSource = _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView;
            dcTABLE_PLAN_APPROVAL_ID.DisplayMember = "NOTE_ROLE_NAME";
            dcTABLE_PLAN_APPROVAL_ID.ValueMember = "TABLE_PLAN_APPROVAL_ID";
            dcTABLE_PLAN_APPROVAL_ID2.DataSource = _dsTable_Approval.Tables["TABLE_PLAN_APPROVAL"].DefaultView;
            dcTABLE_PLAN_APPROVAL_ID2.DisplayMember = "NOTE_ROLE_NAME";
            dcTABLE_PLAN_APPROVAL_ID2.ValueMember = "TABLE_PLAN_APPROVAL_ID";

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

            // Приложения к закрытию табеля
            _daAppendix.SelectCommand = new OracleCommand(string.Format(
                @"SELECT TABLE_CLOSING_APPENDIX_ID, NOTE_DOCUMENT, TABLE_CLOSING_ID 
                FROM {0}.TABLE_CLOSING_APPENDIX WHERE TABLE_CLOSING_ID = :p_TABLE_CLOSING_ID",
                Connect.Schema), Connect.CurConnect);
            _daAppendix.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal);
            // Insert
            _daAppendix.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TABLE_CLOSING_APPENDIX_UPDATE(:TABLE_CLOSING_APPENDIX_ID,:NOTE_DOCUMENT,:DOCUMENT,:TABLE_CLOSING_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.InsertCommand.BindByName = true;
            _daAppendix.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daAppendix.InsertCommand.Parameters.Add("TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_APPENDIX_ID").Direction =
                ParameterDirection.InputOutput;
            _daAppendix.InsertCommand.Parameters["TABLE_CLOSING_APPENDIX_ID"].DbType = DbType.Decimal;
            _daAppendix.InsertCommand.Parameters.Add("NOTE_DOCUMENT", OracleDbType.Varchar2, 0, "NOTE_DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("DOCUMENT", OracleDbType.Blob, 0, "DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("TABLE_CLOSING_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_ID");
            // Update
            _daAppendix.UpdateCommand = _daAppendix.InsertCommand;
            // Delete
            _daAppendix.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TABLE_CLOSING_APPENDIX_DELETE(:TABLE_CLOSING_APPENDIX_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.DeleteCommand.BindByName = true;
            _daAppendix.DeleteCommand.Parameters.Add("TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal, 0, "TABLE_CLOSING_APPENDIX_ID");

            dgAppendix.AutoGenerateColumns = false;
            dgAppendix.DataSource = _dsTable_Approval.Tables["TABLE_APPENDIX"].DefaultView;
            if (!_dsTable_Approval.Tables["TABLE_APPENDIX"].Columns.Contains("DOCUMENT"))
            {
                _dsTable_Approval.Tables["TABLE_APPENDIX"].Columns.Add("DOCUMENT", Type.GetType("System.Byte[]"));
            }
            
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

            cbMonth.SelectedIndex = month - 1;
            cbMonth.SelectedIndexChanged += new EventHandler(cbMonth_SelectedIndexChanged);
            nudYear.Value = year;
            nudYearFilterAbsence.Value = year;
            doc_list = new DOC_LIST_seq(Connect.CurConnect);
            doc_list.Fill(string.Format("order by {0}", DOC_LIST_seq.ColumnsName.DOC_NAME));
            RefGrid();
            dgReg_Doc.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            TDG = new TabDayGrid(this);
            /// Делаем невидимой, чтобы не нажимали кнопки
            pnReg_Doc.Visible = false;

            dtAbsence = new OracleDataTable("", Connect.CurConnect);
            dgAbsence.DataSource = dtAbsence;
            dtAbsence.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAbsence.sql"),
                DataSourceScheme.SchemeName);            
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_per_num", OracleDbType.Varchar2));
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_year_begin", OracleDbType.Decimal)).Value = 
                nudYear.Value;
            dtAbsence.SelectCommand.Parameters.Add(new OracleParameter("p_year_end", OracleDbType.Decimal)).Value =
                nudYear.Value;
            dtAbsence.Fill();
            RefGridAbsence();

            tsReg_Doc.EnableByRules();
            tslCaption.Enabled = true;

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
            dtWorked_day.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;

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

            dtReg_doc = new OracleDataTable("", Connect.CurConnect);
            dgReg_Doc.DataSource = dtReg_doc;
            dtReg_doc.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/Reg_doc.sql"), Connect.Schema);
            dtReg_doc.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = "0";
            dtReg_doc.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = 0;
            dtReg_doc.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            dtReg_doc.Fill();

            /*dtTotalHours = new OracleDataTable("", Connect.CurConnect);
            dtTotalHours.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/New_TotalHours.sql"), Connect.Schema);
            dtTotalHours.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtTotalHours.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtTotalHours.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            dtTotalHours.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);*/

            //ocCalcSalary = new OracleCommand("", Connect.CurConnect);
            //ocCalcSalary.BindByName = true;
            //ocCalcSalary.CommandText = string.Format(
            //    "begin {0}.Calc_Salary(:p_subdiv_id, :p_beginDate, :p_endDate, :p_transfer_id, :p_sign_calc, :p_temp_salary_id); end;",
            //    Connect.Schema);
            //ocCalcSalary.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            //ocCalcSalary.Parameters.Add("p_beginDate", OracleDbType.Date);
            //ocCalcSalary.Parameters.Add("p_endDate", OracleDbType.Date);
            //ocCalcSalary.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            //ocCalcSalary.Parameters.Add("p_sign_calc", OracleDbType.Decimal);
            //ocCalcSalary.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = 
            //    ParameterDirection.InputOutput;

            //dtHoursCalc = new OracleDataTable("", Connect.CurConnect);
            //dtHoursCalc.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Salary_Calc.sql"),
            //    Connect.Schema);
            //dtHoursCalc.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = -1;
            //dtHoursCalc.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal).Value = -1;            
            //dtHoursCalc.Fill();
            // 07.06.2016 - Переходим на новую функцию расчета итогов
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
            dgHoursCalc.DataSource = dtHoursCalc;

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
            dgHoursSaved.DataSource = dtHoursSaved;

            RefGridSalary();

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

            dtWorkOut = new OracleDataTable("", Connect.CurConnect);
            dtWorkOut.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOutPer_Num.sql"),
                Connect.Schema);
            dtWorkOut.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = "0";
            dtWorkOut.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            dtWorkOut.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = BeginDate;
            dtWorkOut.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = EndDate;
            dtWorkOut.SelectCommand.Parameters.Add("p_pay_type", OracleDbType.Decimal).Value = 535;
            dtWorkOut.Fill();
            dgWorkOut.DataSource = dtWorkOut;
            foreach (DataGridViewColumn col in dgWorkOut.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dtEmpErrors = new OracleDataTable("", Connect.CurConnect);
            dtEmpErrors.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/DaysWithError.sql"), Connect.Schema);
            dtEmpErrors.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtEmpErrors.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            dtEmpErrors.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtEmpErrors.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);

            RefReg_Doc();
            VisibleDay();
            // Отключаем обновление дней табеля пока не произойдет событие Load формы
            flagVisible = false;
            // Включаем обновление дней табеля и принудительно обновляем дни
            flagVisible = true;
            //dgEmp_SelectionChanged(sender, e);

            chSign_Vis.CheckedChanged += new EventHandler(chSign_Vis_CheckedChanged);
            chVisibleOnlyAppendix.CheckedChanged += new EventHandler(chVisibleOnlyAppendix_CheckedChanged);
            chFilterToYear.CheckedChanged += new EventHandler(chFilterToYear_CheckedChanged);
            nudYearFilterAbsence.ValueChanged += new EventHandler(nudYearFilterAbsence_ValueChanged);            
            if (GrantedRoles.GetGrantedRole("TABLE_ADD_ABSENCE"))
            {
                btAddAbsence.Visible = true;
                btDeleteAbsence.Visible = true;                
            }
            else
            {
                btAddAbsence.Visible = false;
                btDeleteAbsence.Visible = false;
            }

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

            pnAppendix_Button.EnableByRules();
        }

        /// <summary>
        /// Событие изменения года для фильтра отгулов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nudYearFilterAbsence_ValueChanged(object sender, EventArgs e)
        {
            FilterAbsence(nudYearFilterAbsence.Value, nudYearFilterAbsence.Value);
        }

        /// <summary>
        /// Событие изменения признака фильтра отгулов по году
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chFilterToYear_CheckedChanged(object sender, EventArgs e)
        {
            if (chFilterToYear.Checked)
            {
                FilterAbsence(1000, 3000);        
            }
            else
            {
                FilterAbsence(nudYearFilterAbsence.Value, nudYearFilterAbsence.Value);
            }
            nudYearFilterAbsence.Enabled = !chFilterToYear.Checked;
        }

        /// <summary>
        /// Фильтрация отгулов по годам
        /// </summary>
        /// <param name="_year_begin"></param>
        /// <param name="_year_end"></param>
        void FilterAbsence(decimal _year_begin, decimal _year_end)
        {
            dtAbsence.SelectCommand.Parameters["p_year_begin"].Value = _year_begin;
            dtAbsence.SelectCommand.Parameters["p_year_end"].Value = _year_end;
            dtAbsence.Clear();
            dtAbsence.Fill();
        }

        /// <summary>
        /// Событие изменения признака отображения только данных численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chVisibleOnlyAppendix_CheckedChanged(object sender, EventArgs e)
        {            
            dtHoursCalc.DefaultView.RowFilter = string.Format("SIGN_VISIBLE in (1, {0}) and SIGN_APPENDIX in (1, {1})", 
                (chVisibleOnlyAppendix.Checked ? 0 : Convert.ToInt32(!chSign_Vis.Checked)),
                Convert.ToInt32(chVisibleOnlyAppendix.Checked));
            dtHoursSaved.DefaultView.RowFilter = dtHoursCalc.DefaultView.RowFilter;
            /*Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                Convert.ToInt16(dgEmp.CurrentRow.Cells["sign_comb"].Value.ToString()),
                BeginDate, EndDate);*/
        }

        /// <summary>
        /// Событие изменения признака отображения рассчитанных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chSign_Vis_CheckedChanged(object sender, EventArgs e)
        {
            dtHoursCalc.DefaultView.RowFilter = string.Format("SIGN_VISIBLE in (1, {0}) and SIGN_APPENDIX in (1, {1})",
                (chVisibleOnlyAppendix.Checked ? 0 : Convert.ToInt32(!chSign_Vis.Checked)),
                (chSign_Vis.Checked ? 0 : Convert.ToInt32(chVisibleOnlyAppendix.Checked)));
            dtHoursSaved.DefaultView.RowFilter = dtHoursCalc.DefaultView.RowFilter;
            /*Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                Convert.ToInt16(dgEmp.CurrentRow.Cells["sign_comb"].Value.ToString()),
                BeginDate, EndDate);*/
        }

        private void btRecalcTotal_Click(object sender, EventArgs e)
        {
            if (deBegin_Period.Date == null)
            {
                MessageBox.Show("Вы не указали дату начала периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deBegin_Period.Focus();
                return;
            }
            if (deEnd_Period.Date == null)
            {
                MessageBox.Show("Вы не указали дату окончания периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deEnd_Period.Focus();
                return;
            }
            Calc_Hours(dtHoursPeriod, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                Convert.ToInt16(dgEmp.CurrentRow.Cells["sign_comb"].Value.ToString()),
                (DateTime)deBegin_Period.Date, (DateTime)deEnd_Period.Date.Value.AddDays(1).AddSeconds(-1));
            if (dtHoursPeriod.Rows.Count == 0)
            {
                MessageBox.Show("За указанный период данные отсутствуют!", 
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                if (Application.OpenForms["hoursEmp"].Created)
                {
                    Application.OpenForms["hoursEmp"].Dispose();
                }
            }
            catch { }
            HoursEmp hoursEmp = new HoursEmp(dtHoursPeriod);
            hoursEmp.Text = string.Format("Итоговые часы за период с {0} по {1}",
                deBegin_Period.Date.Value.ToShortDateString(), deEnd_Period.Date.Value.ToShortDateString());
            hoursEmp.Show();
        }

        private void deBegin_Period_Validating(object sender, CancelEventArgs e)
        {
            if ((deBegin_Period.Date != null) && deBegin_Period.Date < BeginDate || deBegin_Period.Date > EndDate)
            {
                MessageBox.Show("Вы указали неверную дату начала периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deBegin_Period.Focus();
                return;
            }
        }

        private void deEnd_Period_Validating(object sender, CancelEventArgs e)
        {
            if ((deEnd_Period.Date != null) && deEnd_Period.Date > EndDate || deEnd_Period.Date < BeginDate)
            {
                MessageBox.Show("Вы указали неверную дату окончания периода!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deEnd_Period.Focus();
                return;
            }
        }

        private void btRestoreOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите восстановить заказ по умолчанию?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                ocUpdate.Parameters["p_per_num"].Value = dgEmp.CurrentRow.Cells["per_num"].Value.ToString();
                ocUpdate.Parameters["p_subdiv_id"].Value = subdiv_id;
                ocUpdate.Parameters["p_transfer_id"].Value = transfer_id;
                ocUpdate.Parameters["p_beginDate"].Value = BeginDate;
                ocUpdate.Parameters["p_endDate"].Value = EndDate;
                ocUpdate.ExecuteNonQuery();
                Connect.Commit();
                Calc_Hours(dtHoursCalc, Convert.ToInt32(dgEmp.CurrentRow.Cells["transfer_id"].Value),
                    Convert.ToInt32(dgEmp.CurrentRow.Cells["sign_comb"].Value),
                    BeginDate, EndDate);
            }
        }

        private void Table_Activated(object sender, EventArgs e)
        {
            FormMain.subdiv_id = subdiv_id;
            FormMain.code_subdiv = code_subdiv;
            FormMain.subdiv_name = subdiv_name;
        }

        /// <summary>
        /// Протокол ошибок в данных по сотруднику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProtError_Click(object sender, EventArgs e)
        {
            FillEmpError(dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString(),
                Convert.ToInt32(dgEmp.CurrentRow.Cells["TRANSFER_ID"].Value),
                Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_hire"].Value == DBNull.Value ? BeginDate :
                    dgEmp.CurrentRow.Cells["date_hire"].Value),
                Convert.ToDateTime(dgEmp.CurrentRow.Cells["date_dismiss"].Value == DBNull.Value ?
                    (dgEmp.CurrentRow.Cells["date_TRANSFER"].Value == DBNull.Value ? EndDate :
                    dgEmp.CurrentRow.Cells["date_TRANSFER"].Value) :
                    dgEmp.CurrentRow.Cells["date_dismiss"].Value));
            if (dtEmpErrors.Rows.Count > 0)
            {
                string strError = "По сотруднику обнаружены следующие ошибочные дни: \n";
                foreach (DataRow row in dtEmpErrors.Rows)
                {
                    strError += "\n" + row[0].ToString();
                }
                MessageBox.Show(strError, "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Ошибок в данных нет!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Добавления наработки на отгул
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddAbsence_Click(object sender, EventArgs e)
        {
            if (dgEmp.CurrentRow != null)
            {
                AddAbsence addAbsence = new AddAbsence(dgEmp.CurrentRow.Cells["per_num"].Value.ToString());
                addAbsence.ShowInTaskbar = false;
                if (addAbsence.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Absence(dgEmp.CurrentRow.Cells["per_num"].Value.ToString(), Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value));
                }
            }
        }

        private void btDeleteAbsence_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить запись из отгулов?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                if (dgAbsence.CurrentRow != null)
                {
                    OracleCommand ocSelDoc = new OracleCommand(
                        string.Format("select 1 from {0}.REG_DOC where absence_id = :p_absence_id", Connect.Schema), Connect.CurConnect);
                    ocSelDoc.BindByName = true;
                    ocSelDoc.Parameters.Add("p_absence_id", OracleDbType.Decimal, 0, "p_absence_id").Value =
                        dgAbsence.CurrentRow.Cells["absence_id"].Value;
                    OracleDataReader odrDoc = ocSelDoc.ExecuteReader();
                    if (odrDoc.Read())
                    {
                        MessageBox.Show("Нельзя удалить данную запись, \nтак как она связана с оправдательным документом!", "АРМ \"Учет рабочего времени\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    OracleCommand ocDelAbsence = new OracleCommand(
                        string.Format("DELETE FROM {0}.ABSENCE WHERE ABSENCE_ID = :p_absence_id", Connect.Schema), Connect.CurConnect);
                    ocDelAbsence.BindByName = true;
                    ocDelAbsence.Parameters.Add("p_absence_id", OracleDbType.Decimal, 0, "p_absence_id").Value =
                        dgAbsence.CurrentRow.Cells["absence_id"].Value;
                    ocDelAbsence.ExecuteNonQuery();
                    Connect.Commit();
                    Absence(dgEmp.CurrentRow.Cells["per_num"].Value.ToString(), Convert.ToDecimal(dgEmp.CurrentRow.Cells["transfer_id"].Value));
                }
            }
        }
		public void SetCurrentTransferId(object transfer_id)
        {
            DataGridViewRow r = dgEmp.Rows.Cast<DataGridViewRow>().FirstOrDefault(t => (t.DataBoundItem as DataRowView)["TRANSFER_ID"].GetHashCode() == transfer_id.GetHashCode());
            if (r != null)
                dgEmp.CurrentCell = r.Cells[0];
        }

        public LinkData GetDataLink(object sender)
        {
            if (dgEmp.CurrentRow != null)
                return new LinkData(null, (dgEmp.CurrentRow.DataBoundItem as DataRowView).Row.Field<Decimal>("TRANSFER_ID"));
            else
                return null;
        }
        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Transfer_id, "TABLE");
        }
        public static void OpenLink(object sender, LinkData e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(@"select subdiv_id, code_subdiv, subdiv_name, transfer_id from {0}.transfer join {0}.subdiv using (subdiv_id) where 
                    transfer_id in (select transfer_id from {0}.transfer where sign_cur_work=1 or type_transfer_id=3 start with transfer_id=:p_transfer_id connect by nocycle PRIOR transfer_id=from_position or prior from_position = transfer_id)", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_transfer_id", OracleDbType.Decimal, e.Transfer_id, ParameterDirection.Input);
                cmd.BindByName = true;
                OracleDataReader r = cmd.ExecuteReader();
                r.Read();
                var OpenTabel = Application.OpenForms.Cast<Form>().Where(t => t.Name == "Table" && (t as Table).subdiv_id==int.Parse(r["SUBDIV_ID"].ToString()));
                Table table;
                if (OpenTabel.Count() == 0)
                {
                    table = new Table();
                    table.subdiv_id = Int32.Parse(r["SUBDIV_ID"].ToString());
                    table.code_subdiv = r["CODE_SUBDIV"].ToString();
                    table.subdiv_name = r["SUBDIV_NAME"].ToString();
                    table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                    table.MdiParent = Application.OpenForms["FormMain"];
                    table.month = DateTime.Now.Month;
                    table.year = DateTime.Now.Year;
                    table.curPer_num = r["TRANSFER_ID"].ToString();
                    table.Show();
                }
                else
                {
                    
                    table = OpenTabel.First<Form>() as Table;
                    table.curPer_num = r["TRANSFER_ID"].ToString();
                    table.month = DateTime.Now.Month;
                    table.year = DateTime.Now.Year;
                    table.SetCurrentTransferId(r["TRANSFER_ID"]);
                    table.Activate();
                }
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                r.Close();
            }
            catch { }
        }
		
		public static Table CurrentActivForm
        {
            get
            {
                Form f = (Application.OpenForms.Cast<Form>().FirstOrDefault(t => t.Name.ToUpper() == "FORMMAIN")).ActiveMdiChild;
                if (f is Table)
                    return (Table)f;
                else return null;
            }
        }

        private void btViewReplEmpTable_Click(object sender, EventArgs e)
        {
            if (this.dgEmp.CurrentRow != null)
            {
                Kadr.Shtat.ReplEmpForm f = new Kadr.Shtat.ReplEmpForm((decimal)(dgEmp.CurrentRow.DataBoundItem as DataRowView)["transfer_id"], "TABLE");
                f.ShowDialog(this);
            }
        }

        private void tcTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedTab = tcTable.SelectedTab;
        }

        public void GetTable_Approval()
        {
            dgAdvance_Approval.DataSource = null;
            dgTable_Approval.DataSource = null;
            dgAppendix.DataSource = null;
            _dsTable_Approval.Tables["ADVANCE_APPROVAL"].Clear();
            _dsTable_Approval.Tables["TABLE_APPROVAL"].Clear();
            _dsTable_Approval.Tables["TABLE_APPENDIX"].Clear();

            _ocTable_Closing_ID.Parameters["p_SUBDIV_ID"].Value = subdiv_id;
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

            _daAppendix.SelectCommand.Parameters["p_TABLE_CLOSING_ID"].Value = Table_Closing_ID;
            _daAppendix.Fill(_dsTable_Approval.Tables["TABLE_APPENDIX"]);

            dgAdvance_Approval.DataSource = _dsTable_Approval.Tables["ADVANCE_APPROVAL"];
            dgTable_Approval.DataSource = _dsTable_Approval.Tables["TABLE_APPROVAL"];
            dgAppendix.DataSource = _dsTable_Approval.Tables["TABLE_APPENDIX"];

            GetStatusApproval();
        }

        void GetStatusApproval()
        {
            cbType_Approval_Advance.Enabled = false;            
            btSave_Approval_Advance.Enabled = false;
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
                    cbType_Approval_Advance.DataSource = _dsTable_Approval.Tables["TYPE_APPROVAL_ADVANCE"].DefaultView;
                    cbType_Approval_Advance.SelectedIndex = 0;
                    cbType_Approval_Advance.Enabled = true;
                    btSave_Approval_Advance.Enabled = true;
                    _dsTable_Approval.Tables["ADVANCE_APPROVAL"].RejectChanges();
                    _row_Advance_Approval = _dsTable_Approval.Tables["ADVANCE_APPROVAL"].DefaultView.AddNew();
                    _row_Advance_Approval["TABLE_CLOSING_ID"] = Advance_Closing_ID;
                    _row_Advance_Approval["TABLE_PLAN_APPROVAL_ID"] =
                        _dsTable_Approval.Tables["PLAN_APPROVAL_ADVANCE"].DefaultView[0]["TABLE_PLAN_APPROVAL_ID_PRIOR"];
                    _dsTable_Approval.Tables["ADVANCE_APPROVAL"].Rows.Add(_row_Advance_Approval.Row);
                }
            }
            //cbType_Approval_Table.DataSource = null;
            cbType_Approval_Table.Enabled = false;
            btSave_Approval_Table.Enabled = false;
            tbNote_Approval.Enabled = false;
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
                    cbType_Approval_Table.DataSource = _dsTable_Approval.Tables["TYPE_APPROVAL_TABLE"].DefaultView;
                    cbType_Approval_Table.SelectedIndex = 0;
                    cbType_Approval_Table.Enabled = true;
                    tbNote_Approval.Enabled = true;
                    tbNote_Approval.Text = "";
                    btSave_Approval_Table.Enabled = true;
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
                lbTable_Closing.Visible = true;
                lbTable_Closing.Text = "СТАТУС - " + _dtSign_Table_Closing.Rows[0]["NOTE_ROLE_APPROVAL"].ToString();
                pnSelectPeriod.Height = 60;
            }
            else
            {
                lbTable_Closing.Visible = false;
                pnSelectPeriod.Height = 40;
            }
        }

        private void btSave_Approval_Advance_Click(object sender, EventArgs e)
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения решения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GetStatusApproval();
            }
        }

        private void btSave_Approval_Table_Click(object sender, EventArgs e)
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения решения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GetStatusApproval();
            }
        }

        private void btAddTable_Appendix_Click(object sender, EventArgs e)
        {
            if (Table_Closing_ID != null)
            {
                DataRowView _currentRow = _dsTable_Approval.Tables["TABLE_APPENDIX"].DefaultView.AddNew();
                _currentRow["TABLE_CLOSING_ID"] = Table_Closing_ID;
                _dsTable_Approval.Tables["TABLE_APPENDIX"].Rows.Add(_currentRow.Row);

                //Appendix_Editor appEditor = new Appendix_Editor(_currentRow);
                Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(null, "", false);
                WindowInteropHelper wih = new WindowInteropHelper(appEditor);
                wih.Owner = this.Handle;
                if (appEditor.ShowDialog() == true)
                {
                    _currentRow["NOTE_DOCUMENT"] = appEditor.Note_Document;
                    _currentRow["DOCUMENT"] = appEditor.Document;
                    SaveAppendix();
                }
                else
                {
                    _dsTable_Approval.Tables["TABLE_APPENDIX"].RejectChanges();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Нельзя добавлять приложения к табелю, пока он не отправлен на согласование!\n" +
                    "(работает после нажатия кнопки - Табель подразделения готов к закрытию)", 
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btEditTable_Appendix_Click(object sender, EventArgs e)
        {
            if (dgAppendix.CurrentRow != null)
            {
                DataRowView rowSelected = dgAppendix.CurrentRow.DataBoundItem as DataRowView;
                rowSelected.Row.RejectChanges();

                //Appendix_Editor appEditor = new Appendix_Editor(rowSelected);
                OracleCommand _ocSelectDocument = new OracleCommand(string.Format(
                    "begin SELECT NVL((select DOCUMENT from {0}.TABLE_CLOSING_APPENDIX where TABLE_CLOSING_APPENDIX_ID = :p_TABLE_CLOSING_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                    Connect.Schema), Connect.CurConnect);
                _ocSelectDocument.BindByName = true;
                _ocSelectDocument.Parameters.Add("p_TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["TABLE_CLOSING_APPENDIX_ID"];
                _ocSelectDocument.Parameters.Add("p_DOCUMENT", OracleDbType.Blob).Direction = ParameterDirection.Output;
                _ocSelectDocument.ExecuteNonQuery();
                byte[] _document = null;
                if (!(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).IsNull)
                {
                    _document = (byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value;
                }
                Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(_document, rowSelected["NOTE_DOCUMENT"].ToString(), false);
                WindowInteropHelper wih = new WindowInteropHelper(appEditor);
                wih.Owner = this.Handle;
                if (appEditor.ShowDialog() == true)
                {
                    rowSelected["NOTE_DOCUMENT"] = appEditor.Note_Document;
                    rowSelected["DOCUMENT"] = appEditor.Document;
                    if (rowSelected.Row.RowState == DataRowState.Unchanged)
                        rowSelected.Row.SetAdded();
                    SaveAppendix();
                }
                else
                {
                    rowSelected.Row.RejectChanges();
                }
            }
        }

        private void btDeleteTable_Appendix_Click(object sender, EventArgs e)
        {
            if (dgAppendix.CurrentRow != null)
            {
                if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"",
                    System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    while (dgAppendix.SelectedCells.Count > 0)
                    {
                        (dgAppendix.CurrentRow.DataBoundItem as DataRowView).Delete();
                    }
                    SaveAppendix();
                }
                dgAppendix.Focus();
            }
        }

        private void btViewTable_Appendix_Click(object sender, EventArgs e)
        {
            if (dgAppendix.CurrentRow != null)
            {
                DataRowView rowSelected = dgAppendix.CurrentRow.DataBoundItem as DataRowView;
                rowSelected.Row.RejectChanges();

                //Appendix_Editor appEditor = new Appendix_Editor(rowSelected);
                OracleCommand _ocSelectDocument = new OracleCommand(string.Format(
                    "begin SELECT NVL((select DOCUMENT from {0}.TABLE_CLOSING_APPENDIX where TABLE_CLOSING_APPENDIX_ID = :p_TABLE_CLOSING_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                    Connect.Schema), Connect.CurConnect);
                _ocSelectDocument.BindByName = true;
                _ocSelectDocument.Parameters.Add("p_TABLE_CLOSING_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["TABLE_CLOSING_APPENDIX_ID"];
                _ocSelectDocument.Parameters.Add("p_DOCUMENT", OracleDbType.Blob).Direction = ParameterDirection.Output;
                _ocSelectDocument.ExecuteNonQuery();
                byte[] _document = null;
                if (!(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).IsNull)
                {
                    _document = (byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value;
                }
                Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(_document, rowSelected["NOTE_DOCUMENT"].ToString(), true);
                WindowInteropHelper wih = new WindowInteropHelper(appEditor);
                wih.Owner = this.Handle;
                appEditor.ShowDialog();
            }
        }
        
        void SaveAppendix()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daAppendix.InsertCommand.Transaction = transact;
                _daAppendix.UpdateCommand.Transaction = transact;
                _daAppendix.DeleteCommand.Transaction = transact;
                _daAppendix.Update(_dsTable_Approval.Tables["TABLE_APPENDIX"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                _dsTable_Approval.Tables["TABLE_APPENDIX"].RejectChanges();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
