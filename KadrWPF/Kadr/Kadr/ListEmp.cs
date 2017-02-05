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

namespace Kadr
{
    public partial class ListEmp : Form, IDataLinkKadr
    {
        FormMain parentForm;
        public BindingSource bsEmp, bsTransfer, bsTransferComb;
        public OracleDataTable dtEmp, dtTransfer, dtTransferComb ;
        private string per_num;

        /// Поле указывает метод сортировки переводов (true - возрастаниe даты перевода, 
        /// false - убывание даты перевода)
        bool sortTransfer = true; 
        /// <summary>
        /// Конструктор формы списка списка сотрудников
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_dtEmp">Таблица с данными работников</param>
        /// <param name="_parentForm">Родительская форма</param>
        public ListEmp(OracleDataTable _dtEmp, FormMain _parentForm, string _thisName)
        {
            InitializeComponent();
            parentForm = _parentForm;
            dtEmp = _dtEmp;
            this.Name = _thisName;
            dtEmp.Fill();
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;
            RefreshGridEmp();
            // Переводы по основной работе
            dtTransfer = new OracleDataTable("", Connect.CurConnect);
            dtTransfer.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtTransfer.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal);

            //bsTransfer = new BindingSource();
            //bsTransfer.DataSource = dtTransfer;
            dgTransfer.DataSource = dtTransfer;
            // Переводы по совмещению
            dtTransferComb = new OracleDataTable("", Connect.CurConnect);
            dtTransferComb.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            dtTransferComb.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal).Value = 0;
            //bsTransferComb = new BindingSource();
            //bsTransferComb.DataSource = dtTransferComb;
            dgTransferComb.DataSource = dtTransferComb;

            bsEmp.PositionChanged += new EventHandler(PositionChange);
            PositionChange(bsEmp, null);
            RefreshGridTransfer(dgTransfer);
            RefreshGridTransfer(dgTransferComb);
            dgEmp.DoubleClick += new EventHandler(parentForm.btEditEmp_Click);            
            dgEmp.KeyPress += new KeyPressEventHandler(dgEmp_KeyPress);
            dgEmp.ContextMenuStrip.Items.Add(ListLinkKadr.GetMenuItem(this));
            dgTransfer.DoubleClick += new EventHandler(parentForm.btViewTransfer_Click);
            dgTransferComb.DoubleClick += new EventHandler(parentForm.btViewTransfer_Click);

            dgEmp.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            if (!GrantedRoles.GetGrantedRole("STAFF_VIEW"))
            {
                splitContainer1.Panel2.Hide();
                splitContainer1.Panel2Collapsed = true;
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
                parentForm.btEditEmp_Click(sender, e);
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
                    //string sql = string.Format(Queries.GetQuery("SelectFind.sql"), Staff.DataSourceScheme.SchemeName, find + sort);
                    string sql = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                        Staff.DataSourceScheme.SchemeName, find + sort);
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
                        string tnom = oracleTable.Rows[0][1].ToString();
                        for (int i = 0; i < dgEmp.Rows.Count; i++)
                        {
                            if (dgEmp["per_num", i].Value.ToString() == tnom)
                            {
                                bsEmp.Position = dgEmp["per_num", i].RowIndex;
                                break;
                            }
                        }
                    }
                    dgEmp.Focus();
                }
            }
        }
        
        /// <summary>
        /// Событие активации формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListEmp_Activated(object sender, EventArgs e)
        {
            dgEmp.Invalidate();
            if (this.Name.ToUpper() == "listemparch".ToUpper())
            {
                parentForm.btDate_DismissSorter.Visible = true;
                FormMain.flagArchive = true;
                //parentForm.ddTransfer.Enabled = false;
            }
            else
            {
                parentForm.btDate_DismissSorter.Visible = false;
                FormMain.flagArchive = false;
                //parentForm.ddTransfer.Enabled = true;
            }
            if (this.Name.ToUpper() != "t_listemp".ToUpper())
            {
                //parentForm.rgFilter.Enabled = true;
                //parentForm.rgTransfer.Enabled = true;
                parentForm.rgFilter.Visible = true;
                parentForm.rgTransfer.Visible = true;
            }
        }

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListEmp_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.rgFilter.Visible = false;
            parentForm.rgTransfer.Visible = false;
            parentForm.btDeleteEmp.Enabled = false;
            parentForm.btEditEmp.Enabled = false;
            parentForm.btRepOtherType.Enabled = false;
            ColumnWidthSaver.SaveWidthOfAllColumns(dgEmp);
            ColumnWidthSaver.SaveWidthOfAllColumns(dgTransfer);
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
                dgTransfer.Visible = true;
                dgTransferComb.Visible = true;

                /// Формируем строку запроса для переводов из файла
                string str = string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                    Staff.DataSourceScheme.SchemeName);
                /// Проверяем порядок сортировки и в зависимости от него, добавляем строку запроса.
                if (!sortTransfer)
                    str += " desc";
                dtTransfer.SelectCommand.CommandText = str;
                dtTransfer.SelectCommand.Parameters["p_per_num"].Value = per_num;
                dtTransfer.SelectCommand.Parameters["p_sign_comb"].Value = 0;
                dtTransfer.Fill(); ;
                dgTransfer.Invalidate();

                str = string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                    Staff.DataSourceScheme.SchemeName);
                if (!sortTransfer)
                    str += " desc";
                dtTransferComb.SelectCommand.CommandText = str;
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
            if (this.Name == "listemp_term")
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
                dgEmp.Columns["REASON_NAME"].Width = 20;
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
            dgEmp.Columns["PERCO_SYNC_ID"].Visible = false;
            dgEmp.Invalidate();
            ColumnWidthSaver.FillWidthOfColumn(dgEmp);
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
        /// Событие нажатия кнопки сортировки переводов в порядке возрастания даты перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSortTransferAsc_Click(object sender, EventArgs e)
        {
            sortTransfer = true;
            PositionChange(sender, e);
        }

        /// <summary>
        /// Событие нажатия кнопки сортировки переводов в порядке убывания даты перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSortTransferDesc_Click(object sender, EventArgs e)
        {
            sortTransfer = false;
            PositionChange(sender, e);
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
                case 70:
                    if (e.Control == true)
                        parentForm.btFind_Click(null, null);
                    else
                        if (e.Alt == true)
                            parentForm.btFilter_Click(null, null);
                    break;
                default: break;
            }
        }

        private void ListEmp_Deactivate(object sender, EventArgs e)
        {
            if (this.Name.ToUpper() != "t_listemp".ToUpper())
            {
                parentForm.rgFilter.Visible = false;
                parentForm.rgTransfer.Visible = false;
            }
        }

        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Transfer_id, "KADR");
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
                var OpenGridRas = Application.OpenForms.Cast<Form>().Where(t => t.Name == "listemp");
                if (OpenGridRas.Count() == 0)
                {
                    FormMain.flagArchive = false;
                    (Application.OpenForms["FormMain"] as FormMain).btDate_DismissSorter.Visible = false;
                    (Application.OpenForms["FormMain"] as FormMain).rgFilter.Visible = true;
                    (Application.OpenForms["FormMain"] as FormMain).rgTransfer.Visible = true;
                    (Application.OpenForms["FormMain"] as FormMain).btEditEmp.Enabled = true;
                    (Application.OpenForms["FormMain"] as FormMain).emp = new EMP_seq(Connect.CurConnect);
                    (Application.OpenForms["FormMain"] as FormMain).textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                        Connect.Schema, Connect.UserId.ToUpper(), " order by per_num");
                    /// Создаем таблицу работников завода.
                    OracleDataTable dtEmp = new OracleDataTable((Application.OpenForms["FormMain"] as FormMain).textQuery, Connect.CurConnect);
                    /// Создаем форму списка работников завода.
                    (Application.OpenForms["FormMain"] as FormMain).listemp = new ListEmp(dtEmp, (Application.OpenForms["FormMain"] as FormMain), "listemp");
                    (Application.OpenForms["FormMain"] as FormMain).listemp.MdiParent = (Application.OpenForms["FormMain"] as FormMain);
                    (Application.OpenForms["FormMain"] as FormMain).listemp.Text = "Список работников завода";
                    (Application.OpenForms["FormMain"] as FormMain).listemp.Show();
                    /* Изменения от 20,07,2013 - если у пользователя роль STAFF_VIEW_LISTEMP - 
                     * то отключаем кнопку отчета произвольной формы*/
                    if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
                    {
                        (Application.OpenForms["FormMain"] as FormMain).btRepOtherType.Enabled = false;
                    }
                    else
                    {
                        (Application.OpenForms["FormMain"] as FormMain).btRepOtherType.Enabled = true;
                    }
                    (Application.OpenForms["FormMain"] as FormMain).listemp.SetCurrentTransferId(r["TRANSFER_ID"]);
                }
                else
                {
                    (Application.OpenForms["FormMain"] as FormMain).listemp = OpenGridRas.First<Form>() as ListEmp;
                    (Application.OpenForms["FormMain"] as FormMain).textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                        Connect.Schema, Connect.UserId.ToUpper(), " order by per_num");
                    (Application.OpenForms["FormMain"] as FormMain).listemp.SetCurrentTransferId(r["TRANSFER_ID"]);
                    (Application.OpenForms["FormMain"] as FormMain).listemp.Activate();
                }
                r.Close();        
            }
            catch { }
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

        /// <summary>
        /// Обработка команды добавления совмещения для сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miListItemAddCombine_Click(object sender, EventArgs e)
        {
            Kadr.Shtat.ReplEmpForm.AddNewCombine(GetDataLink(this).Transfer_id, this);
        }
    }
}
