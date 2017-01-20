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
    public partial class ListEmpTemp : System.Windows.Forms.UserControl
    {
        public BindingSource bsEmp;
        OracleDataTable dtEmp;

        /// <summary>
        /// Конструктор формы списка претендентов на работу
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_dtEmp">Таблица содержит данные претендентов</param>
        /// <param name="_parentform">Родительская форма</param>
        public ListEmpTemp()
        {
            InitializeComponent();
            string textQuery = string.Format(Queries.GetQuery("SelectListEmpHire.sql"),
                Connect.Schema, " order by per_num");
            dtEmp = new OracleDataTable(textQuery, Connect.CurConnect);
            dtEmp.Fill();
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;            
            dgViewEmpTemp.DataSource = bsEmp;
            dgViewEmpTemp.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgViewEmpTemp.Columns["CODE_SUBDIV"].Width = 50;
            dgViewEmpTemp.Columns["CODE_SUBDIV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgViewEmpTemp.Columns["per_num"].HeaderText = "Таб.№";
            dgViewEmpTemp.Columns["per_num"].Width = 55;
            dgViewEmpTemp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgViewEmpTemp.Columns["emp_last_name"].HeaderText = "Фамилия";
            dgViewEmpTemp.Columns["emp_last_name"].Width = 140;
            dgViewEmpTemp.Columns["emp_first_name"].HeaderText = "Имя";
            dgViewEmpTemp.Columns["emp_first_name"].Width = 120;
            dgViewEmpTemp.Columns["emp_middle_name"].HeaderText = "Отчество";
            dgViewEmpTemp.Columns["emp_middle_name"].Width = 160;
            dgViewEmpTemp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
            dgViewEmpTemp.Columns["emp_birth_date"].Width = 80;
            dgViewEmpTemp.Columns["sign_comb"].HeaderText = "Совм.";
            dgViewEmpTemp.Columns["sign_comb"].Width = 50;
            try
            {
                dgViewEmpTemp.Columns["dismiss"].HeaderText = "Увол.";
                dgViewEmpTemp.Columns["dismiss"].Width = 50;
            }
            catch { }
            dgViewEmpTemp.Columns["transfer_id"].Visible = false;
            dgViewEmpTemp.Columns["pos_id"].Visible = false;
            dgViewEmpTemp.Columns["date_hire"].Visible = false;
            dgViewEmpTemp.Columns["CODE_POS"].HeaderText = "Шифр профессии";
            dgViewEmpTemp.Columns["CODE_POS"].Width = 90;
            dgViewEmpTemp.Columns["POS_NAME"].HeaderText = "Наименование профессия";
            dgViewEmpTemp.Columns["POS_NAME"].Width = 500;
            dgViewEmpTemp.Invalidate();
            ColumnWidthSaver.FillWidthOfColumn(dgViewEmpTemp);
            pnButton.EnableByRules();
        }        
        
        /// <summary>
        /// Метод обновляет данные в датагриде
        /// </summary>
        public void EnterDataGridView()
        {
            dtEmp.Clear();
            dtEmp.Fill();
            dgViewEmpTemp.Invalidate();
        }

        private void dgViewEmpTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                bsEmp.Position -= 1;
                //parentForm.btEditEmp_Click(sender, e);
            }
        }

        private void btAddEmp_Click(object sender, EventArgs e)
        {
            AddFindEmp addFindEmp = new AddFindEmp();
            addFindEmp.WindowState = FormWindowState.Normal;
            addFindEmp.ShowInTaskbar = false;
            addFindEmp.ShowDialog();
        }

        private void btEditEmp_Click(object sender, EventArgs e)
        {
            if (dgViewEmpTemp.RowCount != 0)
            {
                string per_num = dgViewEmpTemp.Rows[bsEmp.Position].Cells["per_num"].Value.ToString();
                int transfer_id = Convert.ToInt32(dgViewEmpTemp.Rows[bsEmp.Position].Cells["transfer_id"].Value);
                int sign_comb = dgViewEmpTemp.Rows[bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format("where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                PersonalCard personalcard = new PersonalCard(per_num, transfer_id, record_emp, false, false, false,
                    sign_comb, null, false);
                personalcard.Text = "Личная карточка работника приемной базы данных";
                personalcard.ShowInTaskbar = false;
                personalcard.ShowDialog();
            }
        }

        private void btDeleteEmp_Click(object sender, EventArgs e)
        {
            if (dgViewEmpTemp.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить данные работника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string perNum = dgViewEmpTemp.Rows[bsEmp.Position].Cells["per_num"].Value.ToString();
                    TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                    int signcomb = dgViewEmpTemp.Rows[bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                    transfer.Fill(string.Format(" where {4} = '{3}' and {6} = {8} and ({2} = 1 or " +
                        "(not exists(select null from {0}.{1} tr1 where '{3}' = tr1.{4} and {2} = 1) " + /*У человека нет текущей работы*/
                        "and ({7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 0 ) " + /*Последнюю основную должность*/
                        "or {7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 1 ))))",
                        Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, perNum,
                        TRANSFER_seq.ColumnsName.PER_NUM, TRANSFER_seq.ColumnsName.DATE_TRANSFER,
                        TRANSFER_seq.ColumnsName.SIGN_COMB, "tab1", signcomb));
                    transfer.RemoveAt(0);
                    transfer.Save();
                    transfer.Fill(string.Format("where {0} = '{1}'", TRANSFER_seq.ColumnsName.PER_NUM, perNum));
                    if (transfer.Count == 0)
                    {
                        PER_NUM_BOOK_seq per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                        per_num_book.Fill(string.Format("where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, perNum));
                        ((PER_NUM_BOOK_obj)(per_num_book.Where(i => i.PER_NUM == perNum).FirstOrDefault())).FREE_SIGN = true;
                        per_num_book.Save();
                        EMP_seq emp = new EMP_seq(Connect.CurConnect);
                        emp.Fill(string.Format("where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, perNum));
                        emp.RemoveAt(0);
                        emp.Save();
                    }
                    Connect.Commit();
                    EnterDataGridView();
                }
            }
        }
    }
}
