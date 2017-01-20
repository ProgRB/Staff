using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using LibraryKadr;
using PercoXML;
using LibrarySalary.ViewModel;
using KadrWPF;
using System.Windows.Interop;

namespace Kadr
{
    public partial class ListEmp : System.Windows.Forms.UserControl, IDataLinkKadr
    {
        private System.Windows.Window _parentWindow;
        public System.Windows.Window ParentWindow
        {
            get { return _parentWindow; }
            set { _parentWindow = value; }
        }

        public BindingSource bsEmp, bsTransfer, bsTransferComb;
        public OracleDataTable dtEmp, dtTransfer, dtTransferComb ;
        private string per_num;

        public string Per_num
        {
            get { return per_num; }
        }

        private decimal transfer_id;

        public decimal Transfer_id
        {
            get { return transfer_id; }
            set { transfer_id = value; }
        }

        private decimal worker_id;

        public decimal Worker_id
        {
            get { return worker_id; }
            set { worker_id = value; }
        }
        /// Поле указывает метод сортировки переводов (true - возрастаниe даты перевода, 
        /// false - убывание даты перевода)
        bool _flagArchive;
        public bool FlagArchive
        {
            get { return _flagArchive; }
            set { _flagArchive = value; }
        }
        string _nameForm;
        public string NameForm
        {
            get { return _nameForm; }
            set { _nameForm = value; }
        }
        /// <summary>
        /// Конструктор формы списка списка сотрудников
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_dtEmp">Таблица с данными работников</param>
        /// <param name="_parentForm">Родительская форма</param>
        public ListEmp(string findPerNum, string strSort, string nameForm)
        {
            InitializeComponent();
            string sql = "";
            _nameForm = nameForm;
            switch (nameForm)
            {
                case "ListEmpMain":
                    sql = string.Format(Queries.GetQuery("SelectListEmp.sql"), Connect.Schema, strSort);
                    _flagArchive = false;
                    break;
                case "ListEmpArchive":
                    sql = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"), Connect.Schema, strSort);
                    _flagArchive = true;
                    break;
                case "ListEmpTerm":
                    sql = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"), Connect.Schema, "ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM");
                    _flagArchive = false;
                    break;
                default:
                    break;
            }
            dtEmp = new OracleDataTable(sql, Connect.CurConnect);
            dtEmp.Fill();
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;

            RefreshGridEmp();
            // Переводы по основной работе
            dtTransfer = new OracleDataTable(string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                Connect.Schema), Connect.CurConnect);
            dtTransfer.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtTransfer.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal);

            dgTransfer.DataSource = dtTransfer;
            // Переводы по совмещению
            dtTransferComb = new OracleDataTable(string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                Connect.Schema), Connect.CurConnect);
            dtTransferComb.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            dtTransferComb.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal).Value = 0;
            dgTransferComb.DataSource = dtTransferComb;

            bsEmp.PositionChanged += new EventHandler(PositionChange);
            PositionChange(bsEmp, null);
            RefreshGridTransfer(dgTransfer);
            RefreshGridTransfer(dgTransferComb);         
            dgEmp.KeyPress += new KeyPressEventHandler(dgEmp_KeyPress);
            dgEmp.ContextMenuStrip.Items.Add(ListLinkKadr.GetMenuItem(this));

            dgEmp.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            if (!GrantedRoles.GetGrantedRole("STAFF_VIEW"))
            {
                splitContainer1.Panel2.Hide();
                splitContainer1.Panel2Collapsed = true;
            }

            if (findPerNum != null)
            {
                bsEmp.Position = bsEmp.Find("PER_NUM", findPerNum);           
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            (sender as ContextMenuStrip).Items[0].Enabled = ListLinkKadr.ListLink.Where(r => r.CommandName == "Table").First().CanExecute(GetDataLink(this));
        }

        /// <summary>
        /// Событие нажатия клавиши. Если нажат Enter, показываем личную карточку работника.
        /// Если нет, то сортируем список работников по ФИО и находим букву по нажатой клавише.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgEmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                bsEmp.Position -= 1;
                EditEmp();
            }
            else
            {
                if (e.KeyChar >= Convert.ToChar(1040) && e.KeyChar <= Convert.ToChar(1103))
                {
                    /// Строка поиска
                    string find = string.Format(" where upper(CUR_EMP.{0}) like upper('{1}%') ",
                        EMP_seq.ColumnsName.EMP_LAST_NAME, e.KeyChar);
                    /// Строка сортировки
                    string sort = string.Format("order by {0}, {1}, {2}",
                        EMP_seq.ColumnsName.EMP_LAST_NAME,
                        EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
                    /// Строка запроса
                    //string sql = string.Format(Queries.GetQuery("SelectFind.sql"), Connect.Schema, find + sort);
                    string sql = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                        Connect.Schema, find + sort);
                    OracleDataTable oracleTable = new OracleDataTable(sql, Connect.CurConnect);
                    oracleTable.Fill();
                    if (oracleTable.Rows.Count != 0)
                    {
                        /// Изменяем строку запроса работников и перезаполняем таблицу.
                        int pos = dtEmp.SelectCommand.CommandText.IndexOf("order by");
                        string strSelect = dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
                        bsEmp.PositionChanged -= PositionChange;
                        dtEmp.Clear();
                        dtEmp.SelectCommand.CommandText = strSelect;
                        dtEmp.Fill();
                        bsEmp.PositionChanged += new EventHandler(PositionChange);
                        bsEmp.Position = bsEmp.Find("PER_NUM", oracleTable.Rows[0][1].ToString()); 
                    }
                    dgEmp.Focus();
                }
            }
        }

        int pos = 0;
        /// <summary>
        /// Метод обновляет данные переводов сотрудника при изменении позиции в списке сотрудников
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void PositionChange(object sender, EventArgs e)
        {
            /// Если данные есть, то продолжаем работу. 
            /// Если нет, то скрываем отображение таблиц.
            if (bsEmp.Count != 0)
            {
                dtTransfer.Clear();
                dtTransferComb.Clear();
                pos = bsEmp.Position;
                per_num = dgEmp.Rows[pos].Cells["per_num"].Value.ToString();
                transfer_id = Convert.ToDecimal(dgEmp.Rows[pos].Cells["transfer_id"].Value);
                worker_id = Convert.ToDecimal(dgEmp.Rows[pos].Cells["worker_id"].Value);
                dgTransfer.Visible = true;
                dgTransferComb.Visible = true;

                ///// Формируем строку запроса для переводов из файла
                //string str = string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                //    Connect.Schema);
                //dtTransfer.SelectCommand.CommandText = str;
                dtTransfer.SelectCommand.Parameters["p_per_num"].Value = per_num;
                dtTransfer.SelectCommand.Parameters["p_sign_comb"].Value = 0;
                dtTransfer.Fill(); ;
                dgTransfer.Invalidate();

                //str = string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                //    Connect.Schema);
                //dtTransferComb.SelectCommand.CommandText = str;
                dtTransferComb.SelectCommand.Parameters["p_per_num"].Value = per_num;
                dtTransferComb.SelectCommand.Parameters["p_sign_comb"].Value = 1;
                dtTransferComb.Fill();
                dgTransferComb.Invalidate();
                /// Если по работнику нет основной работы
                if (dtTransfer.Rows.Count == 0)
                {
                    /// Если по работнику есть переводы по совмещению
                    if (dtTransferComb.Rows.Count != 0)
                    {
                        /// Удаляем вкладку совмещения
                        if (tcTransfer.TabPages.IndexOf(tpTransferComb) != -1)
                        {
                            tcTransfer.TabPages.Remove(tpTransferComb);
                        }
                        /// Настраиваем отображение переводов по совмещению в основной вкладке
                        dgTransfer.DataSource = dtTransferComb;
                        tpTransfer.Text = "Совмещение";
                    }
                    /// Если переводов по совмещению нет
                    else
                    {
                        /// Удаляем обе вкладки отображающие совмещение
                        if (tcTransfer.TabPages.IndexOf(tpTransferComb) != -1)
                        {
                            tcTransfer.TabPages.Remove(tpTransferComb);
                            tcTransfer.TabPages.Remove(tpTransfer);
                        }
                    }
                }
                /// Если основная работа есть
                else
                {
                    // Настраиваем отображение переводов по основной работе в основной вкладке
                    dgTransfer.DataSource = dtTransfer;
                    tpTransfer.Text = "Переводы";
                    // Проверяем доступна ли вкладка Переводов по основной работе, если нет - добавляем ее
                    if (tcTransfer.TabPages.IndexOf(tpTransfer) == -1)
                    {
                        tcTransfer.TabPages.Add(tpTransfer);
                    }
                    /// Если есть переводы по совмещению
                    if (dtTransferComb.Rows.Count != 0)
                    {
                        /// Отображаем вкладку совмещения
                        if (tcTransfer.TabPages.IndexOf(tpTransferComb) == -1)
                        {
                            tcTransfer.TabPages.Add(tpTransferComb);
                        }
                    }
                    /// Если переводов по совмещению нет
                    else
                    {
                        /// Скрываем вкладку отображения совмещения
                        if (tcTransfer.TabPages.IndexOf(tpTransferComb) != -1)
                        {
                            tcTransfer.TabPages.Remove(tpTransferComb);
                        }
                    }
                }
            }
            else
            {
                dgTransfer.Visible = false;
                dgTransferComb.Visible = false;
            }
        }

        /// <summary>
        /// Метод обновляет грид списка сотрудников
        /// </summary>
        public void RefreshGridEmp()
        {
            dgEmp.DataSource = bsEmp;
            dgEmp.Invalidate();
            dgEmp.Columns["code_subdiv"].HeaderText = "Подр.";
            dgEmp.Columns["code_subdiv"].Width = 50;
            dgEmp.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_subdiv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["per_num"].HeaderText = "Таб.№";
            dgEmp.Columns["per_num"].Width = 55;
            dgEmp.Columns["per_num"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["emp_last_name"].HeaderText = "Фамилия";
            dgEmp.Columns["emp_last_name"].Width = 140;
            dgEmp.Columns["emp_last_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_first_name"].HeaderText = "Имя";
            dgEmp.Columns["emp_first_name"].Width = 120;
            dgEmp.Columns["emp_first_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_middle_name"].HeaderText = "Отчество";
            dgEmp.Columns["emp_middle_name"].Width = 160;
            dgEmp.Columns["emp_middle_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            if (_nameForm == "ListEmpTerm")
            {
                dgEmp.Columns["DATE_TRANSFER"].HeaderText = "Дата перевода";
                dgEmp.Columns["DATE_TRANSFER"].Width = 80;
                dgEmp.Columns["DATE_TRANSFER"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["DATE_END_CONTR"].HeaderText = "Дата окончания договора";
                dgEmp.Columns["DATE_END_CONTR"].Width = 100;
                dgEmp.Columns["DATE_END_CONTR"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["CHAR_WORK_NAME"].HeaderText = "Срок договора";
                dgEmp.Columns["CHAR_WORK_NAME"].Width = 120;
                dgEmp.Columns["CHAR_WORK_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["CHAR_TRANSFER_NAME"].HeaderText = "Срок перевода";
                dgEmp.Columns["CHAR_TRANSFER_NAME"].Width = 120;
                dgEmp.Columns["CHAR_TRANSFER_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            else
            {
                dgEmp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
                dgEmp.Columns["emp_birth_date"].Width = 80;
                dgEmp.Columns["emp_birth_date"].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgEmp.Columns["sign_comb"].HeaderText = "Совм. в тек.подр.";
            dgEmp.Columns["sign_comb"].Width = 150;
            dgEmp.Columns["sign_comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["sign_comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgEmp.Columns["MED_INSPECTION_DATE"].HeaderText = "Дата прохождения мед. осмотра";
            dgEmp.Columns["MED_INSPECTION_DATE"].Width = 280;
            dgEmp.Columns["MED_INSPECTION_DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["STUDY_LABOR_SAFETY"].HeaderText = "Дата обучения по ОТ";
            dgEmp.Columns["STUDY_LABOR_SAFETY"].Width = 120;
            dgEmp.Columns["STUDY_LABOR_SAFETY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            try
            {
                dgEmp.Columns["comb"].HeaderText = "Совм.";
                dgEmp.Columns["comb"].Width = 50;
                dgEmp.Columns["comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch { }
            try
            {
                dgEmp.Columns["dismiss"].HeaderText = "Ув.";
                dgEmp.Columns["dismiss"].Width = 20;
                dgEmp.Columns["dismiss"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["dismiss"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgEmp.Columns["date_transfer"].Visible = false;
            }
            catch { }
            try
            {
                dgEmp.Columns["REASON_NAME"].HeaderText = "Причина увольнения";
                dgEmp.Columns["REASON_NAME"].Width = 120;
                dgEmp.Columns["REASON_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgEmp.Columns["REASON_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgEmp.Columns["POS_ID"].Visible = false;
            }
            catch { }
            dgEmp.Columns["code_pos"].HeaderText = "Шифр профессии";
            dgEmp.Columns["code_pos"].Width = 90;
            dgEmp.Columns["code_pos"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["pos_name"].HeaderText = "Наименование профессии";
            dgEmp.Columns["pos_name"].Width = 500;
            dgEmp.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["pos_note"].HeaderText = "Примечание к профессии";
            dgEmp.Columns["pos_note"].Width = 100;
            dgEmp.Columns["pos_note"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["transfer_id"].Visible = false;
            dgEmp.Columns["pos_id"].Visible = false;
            dgEmp.Columns["date_hire"].Visible = false;
            dgEmp.Columns["EMP_SEX"].Visible = false;
            dgEmp.Columns["WORKER_ID"].Visible = false;
            if (dgEmp.Columns.Contains("PERCO_SYNC_ID"))
                dgEmp.Columns["PERCO_SYNC_ID"].Visible = false;
            dgEmp.Invalidate();
        }

        /// <summary>
        /// Метод обновляет грид переводов сотрудника
        /// </summary>
        public static void RefreshGridTransfer(DataGridView _dgTransfer)
        {
            _dgTransfer.Columns["per_num"].Visible = false;
            _dgTransfer.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            _dgTransfer.Columns["CODE_SUBDIV"].Width = 50;
            _dgTransfer.Columns["CODE_SUBDIV"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["SIGN_COMB"].HeaderText = "Совм.";
            _dgTransfer.Columns["SIGN_COMB"].Width = 50;
            _dgTransfer.Columns["SIGN_COMB"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["POS_NAME"].HeaderText = "Наименование должности";
            _dgTransfer.Columns["POS_NAME"].Width = 350;
            _dgTransfer.Columns["POS_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["POS_NOTE"].HeaderText = "Примечание к должности";
            _dgTransfer.Columns["POS_NOTE"].Width = 100;
            _dgTransfer.Columns["POS_NOTE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["DATE_TRANSFER"].HeaderText = "Дата движения";
            _dgTransfer.Columns["DATE_TRANSFER"].Width = 80;
            _dgTransfer.Columns["DATE_TRANSFER"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["TYPE_TRANSFER_NAME"].HeaderText = "Тип движения";
            _dgTransfer.Columns["TYPE_TRANSFER_NAME"].Width = 90;
            _dgTransfer.Columns["TYPE_TRANSFER_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CONTR_EMP"].HeaderText = "Трудовой договор";
            _dgTransfer.Columns["CONTR_EMP"].Width = 80;
            _dgTransfer.Columns["CONTR_EMP"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["DATE_CONTR"].HeaderText = "Дата договора";
            _dgTransfer.Columns["DATE_CONTR"].Width = 80;
            _dgTransfer.Columns["DATE_CONTR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["DATE_END_CONTR"].HeaderText = "Окончание договора";
            _dgTransfer.Columns["DATE_END_CONTR"].Width = 90;
            _dgTransfer.Columns["DATE_END_CONTR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["TR_NUM_ORDER"].HeaderText = "Номер приказа";
            _dgTransfer.Columns["TR_NUM_ORDER"].Width = 70;
            _dgTransfer.Columns["TR_NUM_ORDER"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["TR_DATE_ORDER"].HeaderText = "Дата приказа";
            _dgTransfer.Columns["TR_DATE_ORDER"].Width = 80;
            _dgTransfer.Columns["TR_DATE_ORDER"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["FORM_PAY"].HeaderText = "Форма оплаты";
            _dgTransfer.Columns["FORM_PAY"].Width = 100;
            _dgTransfer.Columns["FORM_PAY"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CODE_DEGREE"].HeaderText = "Кат.";
            _dgTransfer.Columns["CODE_DEGREE"].Width = 40;
            _dgTransfer.Columns["CODE_DEGREE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CLASSIFIC"].HeaderText = "Разряд";
            _dgTransfer.Columns["CLASSIFIC"].Width = 65;
            _dgTransfer.Columns["CLASSIFIC"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["TG"].HeaderText = "ТС";
            _dgTransfer.Columns["TG"].Width = 30;
            _dgTransfer.Columns["TG"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CODE_FORM_OPERATION"].HeaderText = "ВП";
            _dgTransfer.Columns["CODE_FORM_OPERATION"].Width = 30;
            _dgTransfer.Columns["CODE_FORM_OPERATION"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CHAR_WORK_NAME"].HeaderText = "Срок договора";
            _dgTransfer.Columns["CHAR_WORK_NAME"].Width = 80;
            _dgTransfer.Columns["CHAR_WORK_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CHAR_TRANSFER_NAME"].HeaderText = "Срок перевода";
            _dgTransfer.Columns["CHAR_TRANSFER_NAME"].Width = 80;
            _dgTransfer.Columns["CHAR_TRANSFER_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["SOURCE_NAME"].HeaderText = "Источник компл.";
            _dgTransfer.Columns["SOURCE_NAME"].Width = 80;
            _dgTransfer.Columns["SOURCE_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["REASON_NAME"].HeaderText = "Причина увольнения";
            _dgTransfer.Columns["REASON_NAME"].Width = 300;
            _dgTransfer.Columns["REASON_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["BASE_DOC_NAME"].HeaderText = "Основание";
            _dgTransfer.Columns["BASE_DOC_NAME"].Width = 200;
            _dgTransfer.Columns["BASE_DOC_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["CHAN_SIGN"].HeaderText = "Признак канц.";
            _dgTransfer.Columns["CHAN_SIGN"].Width = 80;
            _dgTransfer.Columns["CHAN_SIGN"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _dgTransfer.Columns["TRANSFER_ID"].Visible = false;
            _dgTransfer.Columns["WORKER_ID"].Visible = false;
            ColumnWidthSaver.FillWidthOfColumn(_dgTransfer);
        }
        
        /// <summary>
        /// Событие нажатия кнопки на таблице работников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgEmp_Click(object sender, EventArgs e)
        {
            /// Оставляем фокус на гриде, чтобы осуществлять прокрутку
            ((DataGridView)(sender)).Focus();
        }

        private void dgEmp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 40 || e.KeyValue == 38)
                dgEmp.Focus();            
        }

        private void dgEmp_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 38:
                    dgEmp.Focus();
                    break;
                case 40:
                    dgEmp.Focus();
                    break;
                //case 70:
                //    if (e.Control == true)
                //        parentForm.btFind_Click(null, null);
                //    else
                //        if (e.Alt == true)
                //            parentForm.btFilter_Click(null, null);
                //    break;
                default: break;
            }
        }

        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Worker_ID, "KADR");
        }

        public static void OpenLink(object sender, LinkData e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(@"select subdiv_id, code_subdiv, subdiv_name, transfer_id, worker_id 
                    from {0}.transfer join {0}.subdiv using (subdiv_id) 
                    where worker_id = :p_worker_id", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_worker_id", OracleDbType.Decimal, e.Worker_ID, ParameterDirection.Input);
                cmd.BindByName = true;
                OracleDataReader r = cmd.ExecuteReader();
                r.Read();
                ViewTabBase t = App.OpenTabs.GetOpenTabForm(typeof(ListEmpMain));
                if (t == null)
                {
                    App.OpenTabs.AddNewTab("Основная база", new WindowsFormsList_Viewer(new ListEmpMain("", "", "ListEmpMain")));
                    ((ListEmpMain)((LibraryKadr.IWindowsForms_Viewer)App.OpenTabs.SelectedTab.ContentData).ChildForm).SetCurrentTransferId(r["WORKER_ID"]);
                }
                else
                {
                    App.OpenTabs.SelectedTab = t;
                    ((ListEmpMain)((LibraryKadr.IWindowsForms_Viewer)t.ContentData).ChildForm).SetCurrentTransferId(r["WORKER_ID"]);
                }
                r.Close();
            }
            catch { }
        }

        public void SetCurrentTransferId(object worker_id)
        {
            DataGridViewRow r = dgEmp.Rows.Cast<DataGridViewRow>().FirstOrDefault(t => (t.DataBoundItem as DataRowView)["WORKER_ID"].GetHashCode() == worker_id.GetHashCode());
            if (r != null)
                dgEmp.CurrentCell = r.Cells[0];
        }

        public LinkData GetDataLink(object sender)
        {
            if (dgEmp.CurrentRow != null)
                return new LinkData(null, (dgEmp.CurrentRow.DataBoundItem as DataRowView).Row.Field<Decimal>("TRANSFER_ID"), (dgEmp.CurrentRow.DataBoundItem as DataRowView).Row.Field<Decimal>("WORKER_ID"));
            else
                return null;
        }

        /// <summary>
        /// Обработка команды добавления совмещения для сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miListItemAddCombine_Click(object sender, EventArgs e)
        {
            Shtat.ReplEmpForm.AddNewCombine(GetDataLink(this).Transfer_id, new Form());
        }

        public void EditEmp()
        {
            if (LibrarySalary.Helpers.ControlRoles.GetState("BtEditEmp"))
            {
                switch (_nameForm)
                {
                    case "ListEmpMain":
                        if (dgEmp.RowCount != 0)
                        {
                            int sign_comb = dgEmp.Rows[bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                            PersonalCard personalcard = new PersonalCard(Per_num, Transfer_id, record_emp, true, false, false,
                                sign_comb, this, _flagArchive);
                            personalcard.Text = "Личная карточка работника";
                            personalcard.ShowInTaskbar = false;
                            personalcard.ShowDialog();
                        }
                        break;
                    case "ListEmpArchive":
                        if (dgEmp.RowCount != 0)
                        {
                            int sign_comb = dgEmp.Rows[bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                            PersonalCard personalcard = new PersonalCard(Per_num, Transfer_id, record_emp, true, false, false, sign_comb, this, _flagArchive);
                            personalcard.Text = "Личная карточка работника";
                            personalcard.ShowInTaskbar = false;
                            personalcard.ShowDialog();
                        }
                        break;
                    case "ListEmpTerm":
                        if (dgEmp.RowCount != 0)
                        {
                            int sign_comb = dgEmp.Rows[bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                            PersonalCard personalcard = new PersonalCard(Per_num, Transfer_id, record_emp, true, false, false, sign_comb, this, _flagArchive);
                            personalcard.Text = "Личная карточка работника";
                            personalcard.ShowInTaskbar = false;
                            personalcard.ShowDialog();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        // ПЕРЕДЕЛАТЬ --------------------------------------------------------------------------
        /// <summary>
        /// Метод заполняет данные в таблице работников.
        /// </summary>
        public void RefreshGrid()
        {
            bsEmp.PositionChanged -= PositionChange;
            int pos = bsEmp.Position;
            dtEmp.Clear();
            dtEmp.Fill();
            RefreshGridEmp();
            bsEmp.PositionChanged += new EventHandler(PositionChange);
            bsEmp.Position = pos;
        }

        /// <summary>
        /// Событие нажатия кнопки просмотра данных перевода
        /// </summary>
        public void ViewTransfer()
        {
            string per_num = Per_num;
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                this.tcTransfer.SelectedIndex == 0 ?
                Convert.ToInt32(this.dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                Convert.ToInt32(this.dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            account.Fill(string.Format("where TRANSFER_ID = {0}",
                this.tcTransfer.SelectedIndex == 0 ?
                Convert.ToInt32(this.dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                Convert.ToInt32(this.dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
            bool flagDismiss;
            if (this.dgTransfer.Rows[this.dgTransfer.CurrentRow.Index].Cells["type_transfer_name"].Value.ToString().ToUpper() == "УВОЛЬНЕНИЕ")
            { flagDismiss = true; }
            else
            { flagDismiss = false; }
            Transfer formtransfer = new Transfer(record_emp, transfer, transfer, account, account,
                flagDismiss, false, false, this);
            formtransfer.Text = "Переводы";
            formtransfer.ShowDialog();
        }

        /// <summary>
        /// Событие нажатия кнопки добавления текущего перевода.
        /// </summary>
        public void AddTransfer()
        {
            try
            {
                /// Табельный номер.
                string per_num = Per_num;
                /// Данные сотрудника.
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                /// Текущая запись в переводах по основной работе.
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0", TRANSFER_seq.ColumnsName.PER_NUM,
                    per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.SIGN_COMB));
                /// Если записей по основной работе нет, то выводим сообщение об ошибке
                if (transfer.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Данных по основной деятельности работника нет!\nДобавить перевод невозможно!", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Строка перевода, дублирующая текущий перевод. Редактирование необходимых полей.
                TRANSFER_obj r_transfer = (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                r_transfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                r_transfer.FROM_POSITION = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
                r_transfer.CONTR_EMP = "";
                r_transfer.DATE_CONTR = null;
                r_transfer.DATE_TRANSFER = null;
                r_transfer.DATE_END_CONTR = null;
                r_transfer.TR_NUM_ORDER = "";
                r_transfer.TR_DATE_ORDER = null;
                r_transfer.TYPE_TRANSFER_ID = 2;
                r_transfer.FORM_PAY = null;
                r_transfer.DF_BOOK_ORDER = null;
                /// Переводы сотрудника, в которые добавляется новая отредактированная запись.
                TRANSFER_seq transferNew = new TRANSFER_seq(Connect.CurConnect);
                transferNew.AddObject(r_transfer);
                Transfer.flagAdd = true;
                /// Бухгалтерские данные по предыдущему переводу.
                ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
                accountPrev.Fill(string.Format("where TRANSFER_ID = {1} and " +
                    "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})",
                    Connect.Schema, r_transfer.FROM_POSITION));
                /// Бухгалтерские данные. Добавление новой записи для нового перевода.
                ACCOUNT_DATA_obj r_account_date = (ACCOUNT_DATA_obj)((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[accountPrev]).Current)).Clone();
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.AddObject(r_account_date);
                ((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[account]).Current)).TRANSFER_ID =
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNew]).Current)).TRANSFER_ID;
                /// Форма для редактирования данных перевода.
                Transfer formtransfer = new Transfer(record_emp, transferNew, transfer, account,
                    accountPrev, false, true, true, this);
                formtransfer.Text = "Переводы";
                formtransfer.ShowDialog();
                RefreshGrid();
            }
            catch (Exception exp1)
            { System.Windows.Forms.MessageBox.Show(exp1.Message); }
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования данных текущего перевода.
        /// </summary>
        public void EditTransfer()
        {
            try
            {
                /// Табельный номер.
                string per_num = Per_num;
                /// Данные сотрудника.
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                /// Текущая запись в переводах по основной работе
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                    TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Данных по основной деятельности работника нет!\nРедактировать перевод невозможно!", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Бухгалтерские данные по текущему переводу.
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.Fill(string.Format("where TRANSFER_ID = {1} and " +
                    "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})",
                    Connect.Schema,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                /// Предыдущий перевод.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                /// Если предыдущей позиции нет, не заполняем предыдущий перевод.
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION != null)
                    transferPrev.Fill(string.Format(" where {0} = {1}",
                        TRANSFER_seq.ColumnsName.TRANSFER_ID,
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FROM_POSITION));
                /// Бухгалтерские данные по предыдущему переводу.
                ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
                if (transferPrev.Count != 0)
                {
                    accountPrev.Fill(string.Format("where TRANSFER_ID = {1} and " +
                        "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})",
                        Connect.Schema,
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).TRANSFER_ID));
                }
                Transfer.flagAdd = false;
                /// Форма для редактирования данных перевода.
                Transfer formtransfer = new Transfer(record_emp, transfer, transferPrev, account,
                    accountPrev, false, true, true, this);
                formtransfer.Text = "Переводы";
                formtransfer.ShowDialog();
                RefreshGrid();
            }
            catch { }
        }

        /// <summary>
        /// Событие нажатия кнопки удаления текущего перевода.
        /// </summary>
        public void DeleteTransfer()
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить текущей перевод?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                /* Делаю через процедуру с возможностью откатить все данные*/
                DataGridViewRow curRow = dgEmp.Rows[bsEmp.Position];
                OracleCommand _ocTransfer_Cur_Delete = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TRANSFER_CUR_delete(:p_TRANSFER_ID, :p_PREV_SUBDIV_ID, :p_PREV_POS_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocTransfer_Cur_Delete.BindByName = true;
                _ocTransfer_Cur_Delete.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                    curRow.Cells["TRANSFER_ID"].Value;
                _ocTransfer_Cur_Delete.Parameters.Add("p_PREV_SUBDIV_ID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                _ocTransfer_Cur_Delete.Parameters["p_PREV_SUBDIV_ID"].DbType = DbType.Decimal;
                _ocTransfer_Cur_Delete.Parameters.Add("p_PREV_POS_ID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                _ocTransfer_Cur_Delete.Parameters["p_PREV_POS_ID"].DbType = DbType.Decimal;
                OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocTransfer_Cur_Delete.Transaction = _transact;
                    _ocTransfer_Cur_Delete.ExecuteNonQuery();
                    _transact.Commit();

                    DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(curRow.Cells["PERCO_SYNC_ID"].Value.ToString(),
                        curRow.Cells["PER_NUM"].Value.ToString(),
                        curRow.Cells["EMP_LAST_NAME"].Value.ToString(),
                        curRow.Cells["EMP_FIRST_NAME"].Value.ToString(),
                        curRow.Cells["EMP_MIDDLE_NAME"].Value.ToString(),
                        _ocTransfer_Cur_Delete.Parameters["p_PREV_SUBDIV_ID"].ToString(),
                        _ocTransfer_Cur_Delete.Parameters["p_PREV_POS_ID"].ToString()));
                    RefreshGrid();
                }
                catch (Exception ex)
                {
                    _transact.Rollback();
                    System.Windows.Forms.MessageBox.Show(ex.Message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        /// <summary>
        /// Работа с переводами по совмещению
        /// </summary>
        public void TransferComb()
        {
            /// Табельный номер.
            string per_num = Per_num;
            /// Данные сотрудника.
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Текущая запись в переводах по совмещению.
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1", TRANSFER_seq.ColumnsName.PER_NUM,
                per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.SIGN_COMB));
            /// Если записей по совмещению нет, то выводим сообщение об ошибке
            if (transfer.Count == 0)
            {
                MessageBox.Show("Данных по совмещению работника нет!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ListCombTransfer listCombTransfer = new ListCombTransfer(record_emp, this);
            listCombTransfer.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Добавление старых переводов
        /// </summary>
        public void AddOld()
        {
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.AddNew();
            /// Табельный номер по которому добавляется перевод            
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM = Per_num;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID = Worker_id;
            OldTransfer oldTransfer = new OldTransfer(transfer, true);
            oldTransfer.Text = "Добавление старого перевода";
            oldTransfer.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Редактирование старых переводов
        /// </summary>
        public void EditOld()
        {
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format("where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                this.tcTransfer.SelectedIndex == 0 ?
                Convert.ToInt32(this.dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                Convert.ToInt32(this.dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 1)
            {
                MessageBox.Show("Невозможно редактировать приемную запись!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            OldTransfer oldTransfer = new OldTransfer(transfer, false);
            oldTransfer.Text = "Редактирование старого перевода";
            oldTransfer.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Удаление старых переводов
        /// </summary>
        public void DeleteOld()
        {
            if (MessageBox.Show("Вы действительно хотите удалить перевод?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Создаем таблицу для перевода.
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем ее переводом, который сейчас выбран.
                transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    tcTransfer.SelectedIndex == 0 ?
                    Convert.ToInt32(dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                    Convert.ToInt32(dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
                /// Если выбран перевод, то продолжаем работу.
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 2)
                {
                    /// Создаем новую таблицу.
                    TRANSFER_seq transferNext = new TRANSFER_seq(Connect.CurConnect);
                    /// Заполняем таблицу переводом, который является следующим после удаляемого перевода.
                    transferNext.Fill(string.Format("where {0} = {1}",
                        TRANSFER_seq.ColumnsName.FROM_POSITION,
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID));
                    /// Проверяем наличие следующего перевода.
                    if (transferNext.Count != 0)
                    {
                        /// Если он есть, то выставляем FROM_POSITION
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNext]).Current)).FROM_POSITION =
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION;
                        /// Удаляем выбранный перевод.
                        transfer.Remove((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current));
                        /// Сохраняем все изменения в базе данных.
                        transferNext.Save();
                        transfer.Save();
                        Connect.Commit();
                        RefreshGrid();
                    }
                }
                /// Если не перевод, то выводим сообщение.
                else
                {
                    MessageBox.Show("Данный перевод удалить нельзя.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки восстановления работника в должности.
        /// </summary>
        public void RecoveryTransfer()
        {
            if (MessageBox.Show("Вы действительно хотите восстановить работника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string per_num = Per_num;
                /// Последнее увольнение работника.                
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1} and {2} = 3)",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_TRANSFER, Connect.Schema));
                /// Если запись существует, восстанавливаем работника.
                if (transfer.Count != 0)
                {
                    /// Предыдущий перевод. Заполняется по полю FROM_POSITION текущего перевода.
                    TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                    transferPrev.Fill(string.Format(" where {0} = {1}",
                        TRANSFER_seq.ColumnsName.TRANSFER_ID,
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FROM_POSITION));
                    /// Ставим признак текущей работы предыдущему переводу.
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).SIGN_CUR_WORK = true;
                    TRANSFER_obj r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current;
                    /// Удаляем текущей перевод. Сохраняем данные.
                    transfer.Remove((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current);
                    /// Данные по сотруднику.
                    EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                    record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                    /// Проверяем буфер.
                    BUFFER_EMP_seq buffer_emp = new BUFFER_EMP_seq(Connect.CurConnect);
                    buffer_emp.Fill(string.Format("where {0} = {1} and {2} = 3", BUFFER_EMP_seq.ColumnsName.PER_NUM, per_num,
                        BUFFER_EMP_seq.ColumnsName.BUFFER_TYPE));
                    /// Если буфер по этому человеку содержит данные работаем с ними.
                    if (buffer_emp.Count != 0)
                    {
                        /// Удаляем человека из буфера, чтобы он не попал в Перко как уволенный.
                        buffer_emp.Remove((BUFFER_EMP_obj)((CurrencyManager)BindingContext[buffer_emp]).Current);
                        transfer.Save();
                        transferPrev.Save();
                        buffer_emp.Save();
                        Connect.Commit();
                    }
                    /// Если буфер пустой, работаем с Перко.
                    else
                    {
                        /// Восстановление данных в Перко.
                        if (DictionaryPerco.employees.RecoveryEmployee(
                            ((EMP_obj)((CurrencyManager)BindingContext[record_emp]).Current).PERCO_SYNC_ID.ToString(), per_num))
                        {
                            transfer.Save();
                            transferPrev.Save();
                            buffer_emp.Save();
                            Connect.Commit();
                        }
                    }
                    RefreshGrid();
                }
                /// Если человек не был уволен с завода.
                else
                {
                    MessageBox.Show("У работника нет увольнеиня.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование обратного перевода
        /// </summary>
        public void ReverseTransfer()
        {
            if (MessageBox.Show("Вы действительно хотите перевести работника на предыдущее место работы?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime _date_Transfer = DateTime.Today;
                if (InputDataForm.ShowForm(ref _date_Transfer, "dd.MM.yyyy") == System.Windows.Forms.DialogResult.OK)
                {
                    OracleCommand _ocTransfer_Reverse = new OracleCommand(string.Format(
                        @"BEGIN 
                            {0}.TRANSFER_REVERSE(:p_WORKER_ID, :p_DATE_TRANSFER);
                        END;", Connect.Schema), Connect.CurConnect);
                    _ocTransfer_Reverse.BindByName = true;
                    _ocTransfer_Reverse.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = Worker_id;
                    _ocTransfer_Reverse.Parameters.Add("p_DATE_TRANSFER", OracleDbType.Date).Value = _date_Transfer;
                    OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        _ocTransfer_Reverse.Transaction = transact;
                        _ocTransfer_Reverse.ExecuteNonQuery();
                        transact.Commit();
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения перевода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Формирование бланка доп.соглашения по выбранному сотруднику
        /// </summary>
        public void Add_Agreement_For_Emp()
        {
            OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectAdd_Agreement_For_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
            _daReport.SelectCommand.BindByName = true;
            _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = Transfer_id;
            DataSet _ds = new DataSet();
            _daReport.Fill(_ds, "Table1");
            if (_ds.Tables["Table1"].Rows.Count > 0)
            {
                //ReportViewerWindow _rep = new ReportViewerWindow("Дополнительное соглашение", "Reports/Add_Agreement_For_Emp.rdlc",
                //    _ds, null, true);
                //_rep.Show();
                ReportViewerWindow.RenderToExcel(this.Handle, "Add_Agreement_For_Emp.rdlc", _ds.Tables["Table1"], null);
            }
            else
            {
                MessageBox.Show("Нет данных!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки увольнения сотрудника
        /// </summary>
        public void Dismiss()
        {
            /// Табельный номер работника
            string per_num = Per_num;
            /// Создаем таблицу сотрудник и заполняем ее по табельному номеру
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Создаем таблицы перевода и бухгалтерских данных
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            /// Заполняем переводы текущей должностью
            //transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1", EMP_seq.ColumnsName.PER_NUM, per_num,
            //    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0",
                TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                TRANSFER_seq.ColumnsName.SIGN_COMB));
            /// Если сотрудник работает по основному месту
            if (transfer.Count != 0)
            {
                //transfer.Clear();
                //transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1",
                //    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                //    TRANSFER_seq.ColumnsName.SIGN_COMB));
                //if (transfer.Count == 0)
                //{
                //    transfer.Fill(string.Format("where {0} = {1} and {2} = 1 ", TRANSFER_seq.ColumnsName.PER_NUM,
                //        per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                //}
                TRANSFER_obj newTransfer =
                    (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                newTransfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                newTransfer.FROM_POSITION = newTransfer.TRANSFER_ID;
                newTransfer.CONTR_EMP = "";
                newTransfer.DATE_CONTR = null;
                newTransfer.DATE_TRANSFER = null;
                newTransfer.DATE_END_CONTR = null;
                newTransfer.TR_NUM_ORDER = "";
                newTransfer.TR_DATE_ORDER = null;
                newTransfer.TYPE_TRANSFER_ID = 3;
                transfer.Clear();
                transfer.AddObject(newTransfer);
                Transfer.flagAdd = true;
            }
            else
            {
                /// Старое
                //transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                //    "(select max({3}) from {4}.transfer where {0} = {1})",
                //    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                //    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema));
                /// Новое
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {5} = 0 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1})",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema, TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    MessageBox.Show("По работнику нет данных об основном месте работы.\nВозможно он совместитель.",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ///
                Transfer.flagAdd = false;
            }
            Transfer formtransfer = new Transfer(record_emp, transfer, transfer, account, account,
                true, true, true, this);
            formtransfer.Text = "Увольнение";
            formtransfer.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Увольнение в стороннию организацию на территории завода
        /// </summary>
        public void DismissToFR()
        {
            /// Табельный номер работника
            string per_num = Per_num;
            /// Создаем таблицу сотрудник и заполняем ее по табельному номеру
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Создаем таблицы перевода и бухгалтерских данных
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            /// Заполняем переводы текущей должностью
            transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1", EMP_seq.ColumnsName.PER_NUM, per_num,
                TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            /// Проверяем работает ли сотрудник
            if (transfer.Count != 0)
            {
                transfer.Clear();
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1",
                        TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                        TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    transfer.Fill(string.Format("where {0} = {1} and {2} = 1 ", TRANSFER_seq.ColumnsName.PER_NUM,
                        per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                }
                TRANSFER_obj newTransfer =
                    (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                newTransfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                newTransfer.FROM_POSITION = newTransfer.TRANSFER_ID;
                newTransfer.CONTR_EMP = "";
                newTransfer.DATE_CONTR = null;
                newTransfer.DATE_TRANSFER = null;
                newTransfer.DATE_END_CONTR = null;
                newTransfer.TR_NUM_ORDER = "";
                newTransfer.TR_DATE_ORDER = null;
                newTransfer.TYPE_TRANSFER_ID = 3;
                newTransfer.SIGN_CUR_WORK = false;
                newTransfer.DF_BOOK_ORDER = null;
                transfer.Clear();
                transfer.AddObject(newTransfer);
            }
            else
            {
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1} and {2} = 3)",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema));
            }
            TransferToFR formtransfer = new TransferToFR(record_emp, transfer, transfer, account, account,
                true, true, true, this, _flagArchive);
            formtransfer.Text = "Увольнение";
            formtransfer.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Просмотр увольнений по совмещению работника
        /// </summary>
        public void DismissComb()
        {
            /// Табельный номер.
            string per_num = Per_num;
            /// Данные сотрудника.
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Текущая запись в переводах по совмещению.
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            //transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1 and {3} = 3", TRANSFER_seq.ColumnsName.PER_NUM,
            //    per_num, TRANSFER_seq.ColumnsName.SIGN_COMB, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID));
            transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1 and ({3} = 3 or ({3} != 3 and {4} = 1))",
                TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_COMB,
                TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            /// Если записей по совмещению нет, то выводим сообщение об ошибке
            if (transfer.Count == 0)
            {
                MessageBox.Show("Данных по увольнению с совмещения работника нет!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ListCombDismiss listCombDismiss = new ListCombDismiss(record_emp, this);
            listCombDismiss.ShowDialog();
            RefreshGrid();
        }

        /// <summary>
        /// Проект приказа об увольнении
        /// </summary>
        public void Project_Order_Dismiss()
        {
            Order_Dismiss_Project order_Dismiss_Project = new Order_Dismiss_Project(Transfer_id);
            order_Dismiss_Project.ShowDialog();
        }

        private void dgTransfer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LibrarySalary.Helpers.ControlRoles.GetState("BtViewTransfer"))
                ViewTransfer();
        }

        private void dgEmp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditEmp();
        }

        public void RepOtherType()
        {
            try
            {
                string full_str = "";
                //OracleCommand command = new OracleCommand(string.Format("truncate table {0}.PN_TMP", Connect.Schema), connection);
                OracleCommand command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                command.CommandText = string.Format(
                    "delete from {0}.PN_TMP where user_name = :p_user_name", Connect.Schema);
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                Connect.Commit();
                command = new OracleCommand(string.Format(
                    "insert into {0}.PN_TMP(PNUM, USER_NAME,TRANSFER_ID) values (:PN, :UN, :TR)",
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                command.Parameters.Add("PN", OracleDbType.Varchar2, 0, "PN");
                command.Parameters.Add("UN", OracleDbType.Varchar2, 0, "UN").Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal, 0, "TR");
                for (int i = 0; i < dgEmp.RowCount; i++)
                {
                    command.Parameters[0].Value = dgEmp.Rows[i].Cells["per_num"].Value.ToString();
                    command.Parameters[2].Value = dgEmp.Rows[i].Cells["TRANSFER_ID"].Value;
                    command.ExecuteNonQuery();
                }
                Connect.Commit();
                //full_str = string.Format("Exists (SELECT PNUM FROM {0}.PN_tmp WHERE em.per_num = PNUM and user_name = '{1}'", Connect.Schema, Connect.UserId.ToUpper());
                full_str = string.Format("TR.TRANSFER_ID in (SELECT TRANSFER_ID FROM {0}.PN_tmp WHERE user_name = '{1}'", Connect.Schema, Connect.UserId.ToUpper());
                int pos = dtEmp.SelectCommand.CommandText.IndexOf("order by");
                string strOrder = dtEmp.SelectCommand.CommandText.Substring(pos);
                //StaffReports.RepOtherType repOtherType = new StaffReports.RepOtherType(/*full_str, strOrder, */filterOnSubdiv, FlagArchive);
                StaffReports.RepOtherType repOtherType = new StaffReports.RepOtherType(/*full_str, strOrder, */"", FlagArchive);
                repOtherType.ShowDialog();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RepYoung_Specialist()
        {
            SelectPeriod selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet _dsReport = new DataSet();
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                switch (this.NameForm)
                {
                    case "ListEmpArchive":
                        odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepYoung_Specialist.sql"),
                            Connect.Schema, "not");
                        break;
                    case "ListEmpMain":
                        odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepYoung_Specialist.sql"),
                            Connect.Schema, "");
                        break;
                    default: break;
                }
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                odaReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                odaReport.Fill(_dsReport, "REPORT");
                if (_dsReport.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Отчет по молодым специалистам", "Reports/RepYoung_Specialist.rdlc",
                            _dsReport,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGINPERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_ENDPERIOD", selPeriod.EndDate.ToShortDateString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
