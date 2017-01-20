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

namespace Kadr
{
    public partial class ListCombTransfer : Form
    {
        EMP_seq emp;
        OracleDataTable dtComb;
        ListEmp listEmp;
        public ListCombTransfer(EMP_seq _emp, ListEmp _listEmp)
        {
            InitializeComponent();
            emp = _emp;
            listEmp = _listEmp;
            string sql = string.Format(Queries.GetQuery("SelectCombByPerNum.sql"), Connect.Schema);
            dtComb = new OracleDataTable(sql, Connect.CurConnect);
            dtComb.SelectCommand.BindByName = true;
            dtComb.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value =
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM;
            dtComb.Fill();
            dgComb.DataSource = dtComb;
            RefreshGridTransfer(dgComb);
        }

        /// <summary>
        /// Метод обновляет грид переводов сотрудника
        /// </summary>
        public static void RefreshGridTransfer(DataGridView _dgTransfer)
        {
            _dgTransfer.Columns["per_num"].Visible = false;
            _dgTransfer.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            _dgTransfer.Columns["CODE_SUBDIV"].Width = 50;
            _dgTransfer.Columns["POS_NAME"].HeaderText = "Наименование должности";
            _dgTransfer.Columns["POS_NAME"].Width = 250;
            _dgTransfer.Columns["DATE_TRANSFER"].HeaderText = "Дата движения";
            _dgTransfer.Columns["DATE_TRANSFER"].Width = 80;
            _dgTransfer.Columns["TYPE_TRANSFER_NAME"].HeaderText = "Тип движения";
            _dgTransfer.Columns["TYPE_TRANSFER_NAME"].Width = 90;
            _dgTransfer.Columns["CONTR_EMP"].HeaderText = "Трудовой договор";
            _dgTransfer.Columns["CONTR_EMP"].Width = 80;
            _dgTransfer.Columns["DATE_CONTR"].HeaderText = "Дата договора";
            _dgTransfer.Columns["DATE_CONTR"].Width = 80;
            _dgTransfer.Columns["DATE_END_CONTR"].HeaderText = "Окончание договора";
            _dgTransfer.Columns["DATE_END_CONTR"].Width = 90;
            _dgTransfer.Columns["TR_NUM_ORDER"].HeaderText = "Номер приказа";
            _dgTransfer.Columns["TR_NUM_ORDER"].Width = 70;
            _dgTransfer.Columns["TR_DATE_ORDER"].HeaderText = "Дата приказа";
            _dgTransfer.Columns["TR_DATE_ORDER"].Width = 80;
            _dgTransfer.Columns["FORM_PAY"].HeaderText = "Форма оплаты";
            _dgTransfer.Columns["FORM_PAY"].Width = 100;
            _dgTransfer.Columns["CODE_DEGREE"].HeaderText = "Кат.";
            _dgTransfer.Columns["CODE_DEGREE"].Width = 50;
            _dgTransfer.Columns["CLASSIFIC"].HeaderText = "Разряд";
            _dgTransfer.Columns["CLASSIFIC"].Width = 65;
            _dgTransfer.Columns["CHAR_WORK_NAME"].HeaderText = "Срок договора";
            _dgTransfer.Columns["CHAR_WORK_NAME"].Width = 80;
            _dgTransfer.Columns["CHAR_TRANSFER_NAME"].HeaderText = "Срок перевода";
            _dgTransfer.Columns["CHAR_TRANSFER_NAME"].Width = 80;
            _dgTransfer.Columns["SOURCE_NAME"].HeaderText = "Источник компл.";
            _dgTransfer.Columns["SOURCE_NAME"].Width = 80;
            _dgTransfer.Columns["TRANSFER_ID"].Visible = false;
        }

        /// <summary>
        /// Добавление нового перевода по совмещению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddComb_Click(object sender, EventArgs e)
        {
            TRANSFER_seq transferComb = new TRANSFER_seq(Connect.CurConnect);
            transferComb.Fill(string.Format("where transfer_id = {0}", dgComb.CurrentRow.Cells["transfer_id"].Value));
            /// Строка перевода, дублирующая текущий перевод. Редактирование необходимых полей.
            TRANSFER_obj r_transfer = (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).Clone();
            r_transfer.FROM_POSITION = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).TRANSFER_ID;
            r_transfer.CONTR_EMP = "";
            r_transfer.DATE_CONTR = null;
            r_transfer.DATE_TRANSFER = null;
            r_transfer.DATE_END_CONTR = null;
            r_transfer.TR_NUM_ORDER = "";
            r_transfer.TR_DATE_ORDER = null;
            r_transfer.TYPE_TRANSFER_ID = 2;
            /// Переводы сотрудника, в которые добавляется новая отредактированная запись.
            TRANSFER_seq transferNew = new TRANSFER_seq(Connect.CurConnect);
            transferNew.AddObject(r_transfer);
            Transfer.flagAdd = true;
            /// Бухгалтерские данные по предыдущему переводу.
            ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
            accountPrev.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID, r_transfer.FROM_POSITION));
            /// Бухгалтерские данные. Добавление новой записи для нового перевода.
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            account.AddNew();
            ((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[account]).Current)).TRANSFER_ID = 
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNew]).Current)).TRANSFER_ID;
            /// Форма для редактирования данных перевода.
            Transfer formtransfer = new Transfer(emp, transferNew, transferComb, account, accountPrev, 
                false, true, true, listEmp);
            formtransfer.Text = "Переводы";
            formtransfer.ShowDialog();
            dtComb.Clear();
            dtComb.Fill();
        }

        /// <summary>
        /// Редактирование перевода по совмещению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditComb_Click(object sender, EventArgs e)
        {
            TRANSFER_seq transferComb = new TRANSFER_seq(Connect.CurConnect);
            transferComb.Fill(string.Format("where transfer_id = {0}", dgComb.CurrentRow.Cells["transfer_id"].Value));
            /// Бухгалтерские данные по текущему переводу.
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current).TRANSFER_ID));
            /// Предыдущий перевод.
            TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
            /// Если предыдущей позиции нет, не заполняем предыдущий перевод.
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).FROM_POSITION != null)
                transferPrev.Fill(string.Format(" where {0} = {1}",
                    TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current).FROM_POSITION));
            /// Бухгалтерские данные по предыдущему переводу.
            ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
            if (transferPrev.Count != 0)
            {
                accountPrev.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).TRANSFER_ID));
            }
            Transfer.flagAdd = false;
            /// Форма для редактирования данных перевода.
            Transfer formtransfer = new Transfer(emp, transferComb, transferPrev, account, accountPrev, 
                false, true, true, listEmp);
            formtransfer.Text = "Переводы";
            formtransfer.ShowDialog();
            dtComb.Clear();
            dtComb.Fill();
        }

        /// <summary>
        /// Удаление перевода по совмещению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteComb_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить перевод?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TRANSFER_seq transferComb = new TRANSFER_seq(Connect.CurConnect);
                transferComb.Fill(string.Format("where transfer_id = {0}", dgComb.CurrentRow.Cells["transfer_id"].Value));
                /// Если человек не работает на заводе по основной деятельности.
                if (((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current).TYPE_TRANSFER_ID == 1)
                {
                    MessageBox.Show("Удаляемый перевод является приемной записью!\nУдалить перевод невозможно!", 
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Предыдущий перевод. Заполняется по полю FROM_POSITION текущего перевода.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                transferPrev.Fill(string.Format(" where {0} = {1}",
                    TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current).FROM_POSITION));
                /// Бухгалтерские данные по текущему переводу. Удаляем их.
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current).TRANSFER_ID));
                account.Remove((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current);
                account.Save();
                /// Ставим признак текущей работы предыдущему переводу.
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).SIGN_CUR_WORK = true;
                /// Удаляем текущей перевод. Сохраняем данные.
                transferComb.Remove((TRANSFER_obj)((CurrencyManager)BindingContext[transferComb]).Current);
                transferComb.Save();
                transferPrev.Save();
                Connect.Commit();
                dtComb.Clear();
                dtComb.Fill();
            }
        }
    }
}
