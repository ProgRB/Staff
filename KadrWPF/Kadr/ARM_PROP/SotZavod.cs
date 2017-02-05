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
    public partial class SotZavod : Form
    {
        public OracleDataTable dtEmp;
        public BindingSource bsEmp;
        public string per_num;

        EMP_seq emp;
        SUBDIV_seq subdiv;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq types_violation;        

        /// <summary>
        /// Конструктор формы сотрудников завода
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public SotZavod(OracleDataTable _dtEmp, EMP_seq _emp, string perNum)
        {
            InitializeComponent();
            perNum = per_num;
            dtEmp = _dtEmp;
            emp = _emp;
            dgvSotrZavod.DataSource = dtEmp;

            emp = new EMP_seq(Connect.CurConnect);
            emp.Fill();
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill();
            types_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            types_violation.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();

            dgvSotrZavod.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgvSotrZavod.Columns["CODE_SUBDIV"].Width = 68;
            dgvSotrZavod.Columns["CODE_SUBDIV"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["CODE_SUBDIV"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSotrZavod.Columns["PER_NUM"].HeaderText = "Табельный номер";
            dgvSotrZavod.Columns["PER_NUM"].Width = 80;
            dgvSotrZavod.Columns["PER_NUM"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["PER_NUM"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSotrZavod.Columns["EMP_LAST_NAME"].HeaderText = "Фамилия";
            dgvSotrZavod.Columns["EMP_LAST_NAME"].Width = 120;
            dgvSotrZavod.Columns["EMP_LAST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["EMP_LAST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSotrZavod.Columns["EMP_FIRST_NAME"].HeaderText = "Имя";
            dgvSotrZavod.Columns["EMP_FIRST_NAME"].Width = 100;
            dgvSotrZavod.Columns["EMP_FIRST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["EMP_FIRST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSotrZavod.Columns["EMP_MIDDLE_NAME"].HeaderText = "Отчество";
            dgvSotrZavod.Columns["EMP_MIDDLE_NAME"].Width = 120;
            dgvSotrZavod.Columns["EMP_MIDDLE_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["EMP_MIDDLE_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSotrZavod.Columns["transfer_id"].Visible = false;
            //dgvSotrZavod.Columns["ispink"].Visible = false;
            dgvSotrZavod.Columns["CODE_POS"].Visible = false;
            dgvSotrZavod.Columns["POS_NAME"].HeaderText = "Должность";
            dgvSotrZavod.Columns["POS_NAME"].Width = 250;
            dgvSotrZavod.Columns["POS_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvSotrZavod.Columns["POS_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
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
        /// Событие выполняемые при изминении выбора текущей строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSotrZavod_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSotrZavod.CurrentRow != null)
            {
                string per_num = dgvSotrZavod.CurrentRow.Cells["per_num"].Value.ToString();
                tbPodr.Text = dgvSotrZavod.CurrentRow.Cells["CODE_SUBDIV"].Value.ToString();
                tbFam.Text = dgvSotrZavod.CurrentRow.Cells["EMP_LAST_NAME"].Value.ToString();
                tbName.Text = dgvSotrZavod.CurrentRow.Cells["EMP_FIRST_NAME"].Value.ToString();
                tbOtch.Text = dgvSotrZavod.CurrentRow.Cells["EMP_MIDDLE_NAME"].Value.ToString();
                pbPhoto.Image = EmployeePhoto.GetPhoto(per_num);
            }
           
       }

        /// <summary>
        /// Событие выбора текущей строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSotrZavod_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvSotrZavod.Rows[e.RowIndex].Selected = true;

            dgvSotrZavod.RowsDefaultCellStyle.SelectionBackColor =Color.LightBlue;

            dgvSotrZavod.RowsDefaultCellStyle.SelectionForeColor = Color.Black;           
        }

        /// <summary>
        /// Событие нажатия кнопки выбрать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            per_num = dgvSotrZavod.CurrentRow.Cells["per_num"].Value.ToString();
            Close();
        }

        /// <summary>
        /// Событие выполняемые при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SotZavod_Load(object sender, EventArgs e)
        {
            if (dgvSotrZavod.RowCount == 0)
            {
                btSave.Enabled = false;
            }
        }
    }
}
