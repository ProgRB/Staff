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

namespace ARM_PROP
{
    public partial class Group : Form
    {
        OracleConnection connection;
        string textBlockGroup;
        LIST_VIOLATOR_seq list_violator;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq types_violation;
        TYPE_EXACT_seq types_exact;
        string schema;
        public OracleDataTable dtEmp;
        public BindingSource bsEmp;
        ZurnNaruRe jurnal;
        EMP_seq emp;
        public Group(ZurnNaruRe _jurnal, OracleConnection _connection, OracleDataTable _dtEmp, EMP_seq _emp, string _schema)
        {
            InitializeComponent();
            connection = _connection;

            schema = _schema;
            dtEmp = _dtEmp;
            emp = _emp;
            jurnal = _jurnal;
            dgvGroup.DataSource = dtEmp;
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;

            list_violator = new LIST_VIOLATOR_seq(connection);
            list_violator.Fill();
            violation_log = new VIOLATION_LOG_seq(connection);
            violation_log.Fill();
            types_violation = new TYPE_VIOLATION_seq(connection);
            types_violation.Fill();
            types_exact = new TYPE_EXACT_seq(connection);
            types_exact.Fill();

            tbFirst.Text = dgvGroup.Columns["LAST_NAME"].ValueType.ToString();

            RefreshGridNarush(dgvGroup);
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        private void Group_Load(object sender, EventArgs e)
        {

        }

        //private void ButtonSave_Click(object sender, EventArgs e)
        //{

        //}

        //private void ButtonExit_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        public static void RefreshGridNarush(DataGridView _dgvGroup)
        {
            _dgvGroup.Columns["SUBDIV_NAME"].HeaderText = "Подразд.";
            _dgvGroup.Columns["SUBDIV_NAME"].Width = 70;
            _dgvGroup.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["LAST_NAME"].HeaderText = "Фамилия";
            _dgvGroup.Columns["LAST_NAME"].Width = 135;
            _dgvGroup.Columns["LAST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["LAST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["FIRST_NAME"].HeaderText = "Имя";
            _dgvGroup.Columns["FIRST_NAME"].Width = 100;
            _dgvGroup.Columns["FIRST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["FIRST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["MIDDLE_NAME"].HeaderText = "Отчество";
            _dgvGroup.Columns["MIDDLE_NAME"].Width = 135;
            _dgvGroup.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["POS_NAME"].HeaderText = "Должность";
            _dgvGroup.Columns["POS_NAME"].Width = 250;
            _dgvGroup.Columns["POS_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["POS_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["PER_NUM"].HeaderText = "Табельный номер";
            _dgvGroup.Columns["PER_NUM"].Width = 70;
            _dgvGroup.Columns["PER_NUM"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            _dgvGroup.Columns["PER_NUM"].DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvGroup.Columns["VIOLATION_LOG_ID"].Visible = false;
            _dgvGroup.Columns["LIST_VIOLATOR_ID"].Visible = false;
            _dgvGroup.Columns["PERCO_SYNC_ID"].Visible = false;
            _dgvGroup.Columns["OTHER_VIOLATOR_ID"].Visible = false;
            ColumnWidthSaver.FillWidthOfColumn(_dgvGroup);
        }

        private void dgvGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGroup.CurrentRow != null)
            {
                string per_num = dgvGroup.CurrentRow.Cells["per_num"].Value.ToString();
                pictureBox1.Image = EmployeePhoto.GetPhoto(per_num);
            }
        }
    }
}
