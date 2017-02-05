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


namespace ARM_PROP
{
    public partial class StrSotr : Form
    {
        FR_EMP_seq fr_emp;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq type_violation;

        public decimal perco_sync_id;

        /// <summary>
        /// Конструктор формы сторонних сотрудников завода
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public StrSotr(decimal _perco_sync_id)
        {
            InitializeComponent();
            perco_sync_id = _perco_sync_id;

            fr_emp = new FR_EMP_seq(Connect.CurConnect);
            fr_emp.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();
            type_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            type_violation.Fill();

            dgvStrSotrud.AddBindingSource(fr_emp, new LinkArgument(fr_emp, FR_EMP_seq.ColumnsName.SUBDIV_ID));

            dgvStrSotrud.Columns["subdiv_id"].HeaderText = "Подразделение";
            dgvStrSotrud.Columns["pos_id"].Visible = false;
            dgvStrSotrud.Columns["fr_date_start"].Visible = false;
            dgvStrSotrud.Columns["fr_date_end"].Visible = false;            
        }

        /// <summary>
        /// Событие выполняемые при изминении выбора текущей строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStrSotrud_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStrSotrud.CurrentRow != null)
            {
                perco_sync_id = Convert.ToDecimal(dgvStrSotrud.CurrentRow.Cells["PERCO_SYNC_ID"].Value);
                tbPodr.Text = dgvStrSotrud.CurrentRow.Cells["subdiv_id"].Value.ToString();
                tbLast_name.Text = dgvStrSotrud.CurrentRow.Cells["FR_LAST_NAME"].Value.ToString();
                tbFirst_name.Text = dgvStrSotrud.CurrentRow.Cells["FR_FIRST_NAME"].Value.ToString();
                tbMiddle_name.Text = dgvStrSotrud.CurrentRow.Cells["FR_MIDDLE_NAME"].Value.ToString();
            }
        }

        /// <summary>
        /// Событие нажатия кнопки отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие нажатия кнопки выбрать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            perco_sync_id = Convert.ToDecimal( dgvStrSotrud.CurrentRow.Cells["PERCO_SYNC_ID"].Value);
            Close();
        }

    }
}
