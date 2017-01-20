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
    public partial class ListComb : Form
    {
        EMP_seq emp;
        OracleDataTable dtComb;
        public ListComb(EMP_seq _emp)
        {
            InitializeComponent();
            emp = _emp;
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

        private void btAddComb_Click(object sender, EventArgs e)
        {
            TRANSFER_seq transferComb = new TRANSFER_seq(Connect.CurConnect);
            transferComb.AddNew();
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).PER_NUM =
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).TYPE_TRANSFER_ID = 1;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).SUBDIV_ID = 0;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).POS_ID = 0;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).SIGN_COMB = true;
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).WORKER_ID =
                    Convert.ToDecimal(new OracleCommand(string.Format("select {0}.worker_id_seq.nextval from dual", Connect.Schema),
                        Connect.CurConnect).ExecuteScalar());
            Hire_Order order = new Hire_Order(emp, transferComb);
            order.ShowDialog();
            dtComb.Clear();
            dtComb.Fill();
        }

        private void btEditComb_Click(object sender, EventArgs e)
        {
            if (dgComb.CurrentRow != null)
            {
                TRANSFER_seq transferComb = new TRANSFER_seq(Connect.CurConnect);
                transferComb.Fill(string.Format("where transfer_id = {0}", dgComb.CurrentRow.Cells["transfer_id"].Value));
                Hire_Order order = new Hire_Order(emp, transferComb);
                order.ShowDialog();
                dtComb.Clear();
                dtComb.Fill();
            }
        }
    }
}
