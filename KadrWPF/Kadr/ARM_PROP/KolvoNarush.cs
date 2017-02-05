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
using Kadr;

namespace ARM_PROP
{
    public partial class KolvoNarush : Form
    {
        OracleDataTable dtEmp;
        VIOLATION_LOG_seq violation_log;
        string per_num, other_violatin_id, perco_sync_id, cause_arrest, laste_name, first_name, middle_name, unit;
        decimal summ;
        decimal kolvo;
        decimal violation_log_id, types_exact_id;
        LIST_VIOLATOR_seq list_violator;

        public KolvoNarush(OracleDataTable _dtEmp, string _per_num, 
            string _other_violatin_id, string _perco_sync_id, decimal violation_log_id, 
            string cause_arrest, decimal types_exact_id, string _laste_name,
            string _first_name, string _middle_name, string _unit, decimal _kolvo, decimal _summ)
        {
            InitializeComponent();
            per_num = _per_num;
            other_violatin_id = _other_violatin_id;
            perco_sync_id = _perco_sync_id;
            laste_name = _laste_name;
            first_name = _first_name;
            middle_name = _middle_name;
            unit = _unit;
            kolvo = _kolvo;
            summ = _summ;

            list_violator = new LIST_VIOLATOR_seq(Connect.CurConnect);
            list_violator.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();

            dtEmp = _dtEmp;
            dgvChelovec.DataSource = dtEmp;

            dgvChelovec.Columns["violation_log_id"].Visible = false;
            dgvChelovec.Columns["TYPE_EXACT_ID"].Visible = false;
            dgvChelovec.Columns["TYPE_VIOLATION_ID"].Visible = false;
            dgvChelovec.Columns["ARREST_DATE"].HeaderText = "Дата задержания";
            dgvChelovec.Columns["ARREST_DATE"].Width = 80;
            dgvChelovec.Columns["ARREST_DATE"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["ARREST_DATE"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["EXACT_DATE"].HeaderText = "Дата взыскания";
            dgvChelovec.Columns["EXACT_DATE"].Width = 80;
            dgvChelovec.Columns["EXACT_DATE"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["EXACT_DATE"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["INFORM_SUBD_DATE"].HeaderText = "Дата подачи в подразделение";
            dgvChelovec.Columns["INFORM_SUBD_DATE"].Width = 115;
            dgvChelovec.Columns["INFORM_SUBD_DATE"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["INFORM_SUBD_DATE"].DefaultCellStyle.SelectionForeColor = Color.Black;
            //dgvChelovec.Columns["INFORM_SUBD_DATE"].HeaderText = "Дата подачи в подразделение";
            //dgvChelovec.Columns["INFORM_SUBD_DATE"].Width = 100;
            //dgvChelovec.Columns["INFORM_SUBD_DATE"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            //dgvChelovec.Columns["INFORM_SUBD_DATE"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["CAUSE_ARREST"].HeaderText = "Причина задержания";
            dgvChelovec.Columns["CAUSE_ARREST"].Width = 300;
            dgvChelovec.Columns["CAUSE_ARREST"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["CAUSE_ARREST"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["NOTE"].HeaderText = "Примечание";
            dgvChelovec.Columns["NOTE"].Width = 200;
            dgvChelovec.Columns["NOTE"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["NOTE"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["TAKE_MEASURES"].HeaderText = "Принятые меры";
            dgvChelovec.Columns["TAKE_MEASURES"].Width = 300;
            dgvChelovec.Columns["TAKE_MEASURES"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["TAKE_MEASURES"].DefaultCellStyle.SelectionForeColor = Color.Black;
            //dgvChelovec.Columns["violation_log_id1"].Visible = false;
            //dgvChelovec.Columns["TYPE_EXACT_ID"].Visible = false;
            //dgvChelovec.Columns["TYPE_violation_ID"].Visible = false;
            //dgvChelovec.Columns["per_num"].Visible = false;
            dgvChelovec.Columns["per_num"].Visible = false;
            dgvChelovec.Columns["PERCO_SYNC_ID"].Visible = false;
            dgvChelovec.Columns["OTHER_VIOLATOR_ID"].Visible = false;
            dgvChelovec.Columns["UNIT"].HeaderText = "Единица измерения";
            dgvChelovec.Columns["UNIT"].Width = 100;
            dgvChelovec.Columns["UNIT"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["UNIT"].DefaultCellStyle.SelectionForeColor = Color.Black;
            //dgvChelovec.Columns["SIGN_STEAL"].Visible = false;
            //dgvChelovec.Columns["SIGN_GROUP"].Visible = false;
            //dgvChelovec.Columns["SIGN_CRIMINAL"].Visible = false;
            dgvChelovec.Columns["TYPE_STOLEN_TMC"].HeaderText = "Тип похищенного ТМЦ";
            dgvChelovec.Columns["TYPE_STOLEN_TMC"].Width = 100;
            dgvChelovec.Columns["TYPE_STOLEN_TMC"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["TYPE_STOLEN_TMC"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["QUANTITY_GOODS"].HeaderText = "Количество похищенного";
            dgvChelovec.Columns["QUANTITY_GOODS"].Width = 100;
            dgvChelovec.Columns["QUANTITY_GOODS"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["QUANTITY_GOODS"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvChelovec.Columns["TOTAL_STOLEN"].HeaderText = "Сумма похищенного";
            dgvChelovec.Columns["TOTAL_STOLEN"].Width = 100;
            dgvChelovec.Columns["TOTAL_STOLEN"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvChelovec.Columns["TOTAL_STOLEN"].DefaultCellStyle.SelectionForeColor = Color.Black;
            //dgvChelovec.Columns["PER_NUM"].HeaderText = "Табельный номер";
            //dgvChelovec.Columns["PER_NUM"].Width = 70;
            //dgvChelovec.Columns["PER_NUM"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            //dgvChelovec.Columns["PER_NUM"].DefaultCellStyle.SelectionForeColor = Color.Black;

            pictureBox1.Image = EmployeePhoto.GetPhoto(per_num);

            textBox1.Text = laste_name;
            textBox2.Text = first_name;
            textBox3.Text = middle_name;

            pnButton.EnableByRules();
        }

        /// <summary>
        /// Событие нажатия кнопки выхода из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие нажатия кнопки изменения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btInsert_Click(object sender, EventArgs e)
        {
            decimal violation_log_id = Convert.ToDecimal(dgvChelovec.CurrentRow.Cells["violation_log_id"].Value);
            decimal types_exact_id = (decimal)dgvChelovec.CurrentRow.Cells["type_exact_id"].Value;
            decimal types_violation_id = (decimal)dgvChelovec.CurrentRow.Cells["TYPE_VIOLATION_ID"].Value;

            InsertNarush ins = new InsertNarush(per_num, other_violatin_id, perco_sync_id, laste_name, first_name,
            middle_name, violation_log_id, cause_arrest, types_exact_id, unit, kolvo, summ, types_violation_id);
            ins.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvChelovec.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить нарушение", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //string vlid = ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID.ToString();
                    violation_log.Clear();
                    string list_violation_id = dgvChelovec.CurrentRow.Cells["violation_log_id"].Value.ToString();
                    violation_log.Fill("where violation_log_id = " + violation_log_id);
                    dgvChelovec.Rows.Remove(dgvChelovec.CurrentRow);
                    violation_log.Remove((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current);
                    violation_log.Save();
                    Connect.Commit();
                }
            }
        }

        private void KolvoNarush_Load(object sender, EventArgs e)
        {
            btDelet.Visible = false;
            if (dgvChelovec.CurrentRow != null)
            {
                violation_log.Fill(string.Format("where {0} = '{1}'", VIOLATION_LOG_seq.ColumnsName.VIOLATION_LOG_ID, dgvChelovec.CurrentRow.Cells["violation_log_id"].Value.ToString()));

                if (((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID == 1 ||
                    ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID == 3)
                {
                    dgvChelovec.Columns["UNIT"].Visible = false;
                    dgvChelovec.Columns["TYPE_STOLEN_TMC"].Visible = false;
                    dgvChelovec.Columns["QUANTITY_GOODS"].Visible = false;
                    dgvChelovec.Columns["TOTAL_STOLEN"].Visible = false;
                }
            }
        }
    }
}
