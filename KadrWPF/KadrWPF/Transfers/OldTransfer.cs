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

namespace Kadr
{
    public partial class OldTransfer : Form
    {
        TRANSFER_seq transfer;
        ACCOUNT_DATA_seq account_data;
        DEGREE_seq degree;
        //CHAR_WORK_seq char_work;
        TYPE_TERM_TRANSFER_seq type_Term_Transfer;
        SUBDIV_seq subdiv;
        POSITION_seq position;
        FORM_OPERATION_seq form_operation;
        DateTime dateHire;
        bool flagAdd;
        /// <summary>
        /// Работа со старыми переводами
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_transfer">Таблица перевода</param>
        /// <param name="_flagAdd">Признак добавления старого перевода</param>
        public OldTransfer(TRANSFER_seq _transfer, bool _flagAdd)
        {
            InitializeComponent();
            transfer = _transfer;
            flagAdd = _flagAdd;
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE != null)
            {
                dateHire = (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
            }
            else
            {
                TRANSFER_seq transferHire = new TRANSFER_seq(Connect.CurConnect);
                transferHire.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = "+
                    "(select max({3}) from {4}.transfer where {0} = {1})", 
                    TRANSFER_seq.ColumnsName.PER_NUM,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM,
                    TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.DATE_HIRE, Staff.DataSourceScheme.SchemeName));
                dateHire = (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).DATE_HIRE;
            }
            account_data = new ACCOUNT_DATA_seq(Connect.CurConnect);
            account_data.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID, 
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID));
            if (account_data.Count == 0)
            {
                account_data.AddNew();
                ((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[account_data]).Current)).TRANSFER_ID =
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
            }
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} is null or {0} != 6 order by {1}", SUBDIV_seq.ColumnsName.TYPE_SUBDIV_ID,
                SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            position = new POSITION_seq(Connect.CurConnect);
            position.Fill(string.Format("order by {0}", POSITION_seq.ColumnsName.POS_NAME));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME));

            //char_work = new CHAR_WORK_seq(Connect.CurConnect);
            //char_work.Fill(string.Format("order by {0}", CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME));
            type_Term_Transfer = new TYPE_TERM_TRANSFER_seq(Connect.CurConnect);
            type_Term_Transfer.Fill("ORDER BY TYPE_TERM_TRANSFER_ID");

            form_operation = new FORM_OPERATION_seq(Connect.CurConnect);
            form_operation.Fill(string.Format("order by {0}", FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID, new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID, new LinkArgument(position, POSITION_seq.ColumnsName.POS_NAME));

            //cbChar_Work_ID.AddBindingSource(transfer, CHAR_WORK_seq.ColumnsName.CHAR_WORK_ID, new LinkArgument(char_work, CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME));
            cbChar_Work_ID.DataSource = type_Term_Transfer;
            cbChar_Work_ID.DisplayMember = "TYPE_TERM_TRANSFER_NAME";
            cbChar_Work_ID.ValueMember = "TYPE_TERM_TRANSFER_ID";
            cbChar_Work_ID.DataBindings.Add("SelectedValue", transfer, "CHAR_WORK_ID", true, DataSourceUpdateMode.OnPropertyChanged, "");

            cbDegree_Name.AddBindingSource(transfer, DEGREE_seq.ColumnsName.DEGREE_ID, new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            tbPer_Num.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.PER_NUM);            
            tbTr_Num_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_NUM_ORDER);            
            tbContr_Emp.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.CONTR_EMP);            
            cbSign_Comb.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.SIGN_COMB);
            tbSalary.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.SALARY);
            tbClassific.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.CLASSIFIC);
            cbForm_Operation.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.FORM_OPERATION_ID,
                new LinkArgument(form_operation, FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            deDate_Transfer.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);
            deTr_Date_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_DATE_ORDER);
            deDate_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_CONTR);
            deDate_End_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_END_CONTR);

            if (!flagAdd)
            {
                tbCode_Subdiv.Text = subdiv.Where(s => s.SUBDIV_ID.ToString() == cbSubdiv_Name.SelectedValue.ToString()).FirstOrDefault().CODE_SUBDIV.ToString();
                tbCode_Pos.Text = position.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
                tbCode_Degree.Text = degree.Where(s => s.DEGREE_ID.ToString() == cbDegree_Name.SelectedValue.ToString()).FirstOrDefault().CODE_DEGREE.ToString();
                tbCode_Form_Operation.Text =
                    form_operation.Where(s => s.FORM_OPERATION_ID.ToString() == cbForm_Operation.SelectedValue.ToString()).FirstOrDefault().CODE_FORM_OPERATION.ToString();
            }

            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);
            cbForm_Operation.SelectedIndexChanged += new EventHandler(cbForm_Operation_SelectedIndexChanged);
            tbSalary.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbPer_Num.Enabled = false;
        }

        /// <summary>
        /// Событие изменения индекса подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = Library.CodeBySelectedValue(Connect.CurConnect, POSITION_seq.ColumnsName.CODE_POS.ToString(), Staff.DataSourceScheme.SchemeName,
                    "position", POSITION_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbDegree_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDegree_Name.SelectedValue != null)
            {
                tbCode_Degree.Text = Library.CodeBySelectedValue(Connect.CurConnect, DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(),
                    Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), cbDegree_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue == null || cbSubdiv_Name.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Вы не ввели подразделение!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbCode_Subdiv.Focus();
                return;
            }
            if (cbPos_Name.SelectedValue == null || cbPos_Name.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Вы не ввели должность!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbCode_Pos.Focus();
                return;
            }
            if (deDate_Transfer.Text == null || deDate_Transfer.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату перевода!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.deDate_Transfer.Focus();
                return;
            }
            if (tbTr_Num_Order.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели номер приказа о переводе!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbTr_Num_Order.Focus();
                return;
            }
            if (deTr_Date_Order.Text == null || deTr_Date_Order.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату приказа о переводе!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.deTr_Date_Order.Focus();
                return;
            }
            
            /// Проверка добавления данных или редактирования.
            if (flagAdd)
            {
                /// Создаем новую таблицу.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем таблицу переводом, который по дате является предыдущим для добавляемого перевода.
                transferPrev.Fill(string.Format("where WORKER_ID = {1} and DATE_TRANSFER = " +
                    "(select max(T1.DATE_TRANSFER) from {0}.TRANSFER T1 where T1.WORKER_ID = {1} and T1.DATE_TRANSFER < to_date('{2}', 'DD.MM.YYYY'))",
                    Connect.Schema,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.ToShortDateString()));
                /// Создаем новую таблицу.
                TRANSFER_seq transferNext = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем таблицу переводом, который по дате является предыдущим для добавляемого перевода.
                transferNext.Fill(string.Format("where WORKER_ID = {1} and DATE_TRANSFER = " +
                    "(select min(T1.DATE_TRANSFER) from {0}.TRANSFER T1 where T1.WORKER_ID = {1} and T1.DATE_TRANSFER > to_date('{2}', 'DD.MM.YYYY'))",
                    Connect.Schema,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.ToShortDateString()));    
                /// Заполняем необходимые поля.
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).HIRE_SIGN = true;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SOURCE_ID = 4;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER =
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER;
                /// Проверяем наличие предшествующего перевода.
                if (transferPrev.Count != 0)
                {
                    /// Если он есть, то выставляем FROM_POSITION и тип перевода
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION =
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).TRANSFER_ID;
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID = 2;
                }
                else
                {
                    /// Всегда должен быть предшествующий перевод, так как приемную запись редактировать нельзя
                    /// 

                    ///// Если добавляется самый ранний перевод, то выходит ошибка!!!!!!!!!!!!!!!
                    ///// !!!!!!!!!!!!! Обработать !!!!!!!!!!!!!!!!!
                    //((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION = null;
                    //((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID = 1;
                    //if (transferNext.Count != 0)
                    //{
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNext]).Current)).TYPE_TRANSFER_ID = 2;
                    //}
                }
                /// Проверяем наличие следующего перевода.
                if (transferNext.Count != 0)
                {
                    /// Если он есть, то выставляем FROM_POSITION
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNext]).Current)).FROM_POSITION =
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
                    /// Убираем признак текущей работы у добавляемого перевода
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = false;
                }
                else
                {
                    /// Если добавляется самый последний перевод, то у предыдущего убираем признак текущей работы.
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SIGN_CUR_WORK = false;
                    /// Ставим признак текущей работы.
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = true;
                }
                /// Сохраняем все изменения в базе данных.
                transfer.Save();
                transferPrev.Save();
                transferNext.Save();
            }
            else
            {
                /// Создаем новую таблицу.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем таблицу переводом, который по дате является предыдущим для добавляемого перевода.
                //transferPrev.Fill(string.Format("where WORKER_ID = {1} and DATE_TRANSFER = " +
                //    "(select max(T1.DATE_TRANSFER) from {0}.TRANSFER T1 where T1.WORKER_ID = {1} and T1.DATE_TRANSFER < to_date('{2}', 'DD.MM.YYYY'))",
                //    Connect.Schema,
                //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID,
                //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.ToShortDateString()));
                transferPrev.Fill(string.Format("where WORKER_ID = {0} and TRANSFER_ID = {1}",
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION));
                /// Создаем новую таблицу.
                TRANSFER_seq transferNext = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем таблицу переводом, который по дате является предыдущим для добавляемого перевода.
                transferNext.Fill(string.Format("where WORKER_ID = {1} and DATE_TRANSFER = " +
                    "(select min(T1.DATE_TRANSFER) from {0}.TRANSFER T1 where T1.WORKER_ID = {1} and T1.DATE_TRANSFER > to_date('{2}', 'DD.MM.YYYY'))",
                    Connect.Schema,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.ToShortDateString()));
                TRANSFER_seq transferFrom = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем таблицу переводом, который является дочерним для редактируемого
                transferFrom.Fill(string.Format("where FROM_POSITION = {0}",
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID));
                /// Создаем переменную для хранения позиции родительской записи для редактируемой.
                decimal? transferFrom_ID = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION;
                /// Если данные есть, то ставим дочерней строке нужную позицию
                if (transferFrom.Count != 0)
                {
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferFrom]).Current)).FROM_POSITION = transferFrom_ID;
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferFrom]).Current)).SIGN_CUR_WORK = false;
                }
                transferFrom.Save();
                /// Заполняем необходимые поля.
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER =
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER;
                /// Проверяем наличие предшествующего перевода. Выставляем FROM_POSITION.
                if (transferPrev.Count != 0)
                {
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION =
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).TRANSFER_ID;
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SIGN_CUR_WORK = false;
                }
                else
                {
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION = null;
                }
                /// Проверяем наличие следующего перевода.
                if (transferNext.Count != 0)
                {
                    /// Если он есть, то выставляем FROM_POSITION
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNext]).Current)).FROM_POSITION =
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
                    /// Убираем признак текущей работы у редактируемого перевода.
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = false;
                    ///// Сохраняем данные, так как без этого запрос на дочернюю строку (см.далее) работает неверно.
                    transferNext.Save();
                    transfer.Save();
                    Connect.Commit();
                    /// Создаем таблицу.
                    TRANSFER_seq transferCur = new TRANSFER_seq(Connect.CurConnect);
                    /// Заполняем таблицу дочерней строкой для transferNext.
                    transferCur.Fill(string.Format("where {0} = {1} and {2} = {3} and {4} = " +
                        "(select max({4}) from {5}.transfer where {0} = {1} and {2} = {3} and {6} = 2)", 
                        TRANSFER_seq.ColumnsName.PER_NUM, 
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM,
                        TRANSFER_seq.ColumnsName.SIGN_COMB, 
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_COMB ? 1 : 0,
                        TRANSFER_seq.ColumnsName.DATE_TRANSFER, Staff.DataSourceScheme.SchemeName,
                        TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID));
                    /// Если строка не является родительской (с нее не делали перевод), то делаем ее текущей
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).SIGN_CUR_WORK = true;
                    transferCur.Save();
                }
                else
                {
                    //((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION =
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).TRANSFER_ID;
                    /// Если добавляемый перевод является текущим, то убираем признак у предыдущего 
                    /// и ставим его добавляемому.
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SIGN_CUR_WORK = false;
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = true;
                    //((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION =
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).TRANSFER_ID;
                    //transferNext.Fill(string.Format("where 
                }
                /// Сохраняем все изменения в базе данных.
                
                transfer.Save();
                if (transferFrom.Count != 0 && transferPrev.Count != 0 &&
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).TRANSFER_ID !=
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferFrom]).Current)).TRANSFER_ID)
                {
                    transferPrev.Save();
                }
                transferNext.Save();

//                /// Создаем таблицу.
//                TRANSFER_seq transferCur = new TRANSFER_seq(connection);
//                /// Заполняем таблицу дочерней строкой для transferNext.
//                transferCur.Fill(string.Format("where {0} = {1}",
//                    TRANSFER_seq.ColumnsName.TRANSFER_ID, transferFrom_ID));
//                /// Если строка не является родительской (с нее не делали перевод), то делаем ее текущей
////                transferCur.Save();
//                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).SIGN_CUR_WORK = true;
//                transferCur.Save();
            }
            if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CHANGE_DATE == null)
            {
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CHANGE_DATE =
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER;
            }
            account_data.Save();
            Connect.Commit();
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            transfer.RollBack();
            Connect.Rollback();
            Close();            
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, 5, Connect.CurConnect, e, POSITION_seq.ColumnsName.POS_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);
        }

        /// <summary>
        /// Проверка введенного шифра категории и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Degree_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Degree, cbDegree_Name, 2, Connect.CurConnect, e, DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(), tbCode_Degree.Text);
        }

        private void deDate_Transfer_Validating(object sender, CancelEventArgs e)
        {
            /// Если дата перевода меньше даты приема, то выдаем сообщение об ошибке
            if (dateHire >= deDate_Transfer.Date)
            {
                MessageBox.Show("Невозможно установить введенную дату перевода!\nОна меньше или равна дате приема.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void tbCode_Form_Operation_Leave(object sender, EventArgs e)
        {

        }

        private void cbForm_Operation_Leave(object sender, EventArgs e)
        {
            if (cbForm_Operation.Text == "")
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FORM_OPERATION_ID = null;
        }

        private void tbCode_Form_Operation_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Form_Operation, cbForm_Operation, 1, Connect.CurConnect, e,
                FORM_OPERATION_seq.ColumnsName.FORM_OPERATION_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "form_operation",
                FORM_OPERATION_seq.ColumnsName.CODE_FORM_OPERATION.ToString(), tbCode_Form_Operation.Text);
        }

        void cbForm_Operation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbForm_Operation.SelectedValue != null)
            {
                tbCode_Form_Operation.Text =
                    Library.CodeBySelectedValue(Connect.CurConnect, FORM_OPERATION_seq.ColumnsName.CODE_FORM_OPERATION.ToString(),
                    Staff.DataSourceScheme.SchemeName, "form_operation",
                    FORM_OPERATION_seq.ColumnsName.FORM_OPERATION_ID.ToString(), cbForm_Operation.SelectedValue.ToString());
            }
        }
    }
}
