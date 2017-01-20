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
    public partial class EditPermit : Form
    {
        OracleDataTable _dtEmpPermit;
        PERMIT_seq permit;
        TYPE_PERMIT_seq type_permit;
        decimal transfer_id;
        string per_num;
        public EditPermit(string _code_subdiv, decimal _transfer_id, string _per_num, string _emp_last_name, 
            string _emp_first_name,
            string _emp_middle_name, string _pos_name, OracleDataTable dtEmpPermit)
        {
            InitializeComponent();
            transfer_id = _transfer_id;
            per_num = _per_num;
            _dtEmpPermit = dtEmpPermit;
            if (_dtEmpPermit.DefaultView.RowFilter == "")
                tschShowAllPermit.Checked = true;
            permit = new PERMIT_seq(Connect.CurConnect);
            type_permit = new TYPE_PERMIT_seq(Connect.CurConnect);
            type_permit.Fill("order by " + TYPE_PERMIT_seq.ColumnsName.PERMIT_NAME);

            tbCode_Subdiv.Text = _code_subdiv;
            tbPer_num.Text = _per_num;
            tbEmp_last_name.Text = _emp_last_name;
            tbEmp_first_name.Text = _emp_first_name;
            tbEmp_middle_name.Text = _emp_middle_name;
            tbPos_name.Text = _pos_name;
            pbPhoto.Image = EmployeePhoto.GetPhoto(_per_num);
            //dgEmpPermit.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);

            dgEmpPermit.DataSource = dtEmpPermit.DefaultView;
            dgEmpPermit.Columns["permit_id"].Visible = false;
            dgEmpPermit.Columns["FL_ARCHIV"].Visible = false;
            dgEmpPermit.Columns["NUM_DOC_PERMIT"].HeaderText = "№ служебной";
            dgEmpPermit.Columns["DATE_DOC_PERMIT"].HeaderText = "Дата служебной";
            dgEmpPermit.Columns["PERMIT_NAME"].HeaderText = "Наименование разрешения";
            dgEmpPermit.Columns["DATE_START_PERMIT"].HeaderText = "Дата начала разрешения";
            dgEmpPermit.Columns["DATE_END_PERMIT"].HeaderText = "Дата окончания разрешения";

            tbNum_Doc_Permit.AddBindingSource(permit, PERMIT_seq.ColumnsName.NUM_DOC_PERMIT);
            deDate_Doc_Permit.AddBindingSource(permit, PERMIT_seq.ColumnsName.DATE_DOC_PERMIT);
            cbPermit_Name.AddBindingSource(permit, TYPE_PERMIT_seq.ColumnsName.TYPE_PERMIT_ID,
                new LinkArgument(type_permit, TYPE_PERMIT_seq.ColumnsName.PERMIT_NAME));
            if (permit.Count == 0)
                cbPermit_Name.SelectedItem = null;
            deDate_Start_Permit.AddBindingSource(permit, PERMIT_seq.ColumnsName.DATE_START_PERMIT);
            deDate_End_Permit.AddBindingSource(permit, PERMIT_seq.ColumnsName.DATE_END_PERMIT);
            EnableControls(false);
            if (!GrantedRoles.GetGrantedRole("STAFF_PERMIT"))
            {
                tsButton.Enabled = false;
            }

            tschShowAllPermit.CheckedChanged += new EventHandler(tschShowAllPermit_CheckedChanged);
        }

        void EnableControls(bool _value)
        {
            tbNum_Doc_Permit.Enabled = _value;
            deDate_Doc_Permit.Enabled = _value;
            deDate_Start_Permit.Enabled = _value;
            deDate_End_Permit.Enabled = _value;
            cbPermit_Name.Enabled = _value;
            tsbAddPermit.Enabled = !_value;
            tsbEditPermit.Enabled = !_value;
            tsbDeletePermit.Enabled = !_value;
            tsbSavePermit.Enabled = _value;
            tsbRefreshPermit.Enabled = _value;
            dgEmpPermit.Enabled = !_value;
        }

        private void tsbAddPermit_Click(object sender, EventArgs e)
        {
            permit.AddNew();            
            ((PERMIT_obj)((CurrencyManager)BindingContext[permit]).Current).TRANSFER_ID = transfer_id;
            ((PERMIT_obj)((CurrencyManager)BindingContext[permit]).Current).PER_NUM = per_num;
            EnableControls(true);
            tbNum_Doc_Permit.Focus();
        }

        private void tsbEditPermit_Click(object sender, EventArgs e)
        {
            if (dgEmpPermit.CurrentRow != null)
            {
                permit.Fill("where permit_id = " + dgEmpPermit.CurrentRow.Cells["permit_id"].Value.ToString());
                EnableControls(true);
                tbNum_Doc_Permit.Focus();
            }
        }

        private void tsbDeletePermit_Click(object sender, EventArgs e)
        {
            if (dgEmpPermit.CurrentRow != null)
            {
                if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    permit.Fill("where permit_id = " + dgEmpPermit.CurrentRow.Cells["permit_id"].Value.ToString());
                    permit.Remove(((PERMIT_obj)((CurrencyManager)BindingContext[permit]).Current));
                    permit.Save();
                    Connect.Commit();
                    _dtEmpPermit.Clear();
                    _dtEmpPermit.Fill();
                }
            }
        }

        private void tsbSavePermit_Click(object sender, EventArgs e)
        {
            Control[] controls = new Control[] { tbNum_Doc_Permit, deDate_Doc_Permit, cbPermit_Name, 
                deDate_Start_Permit, deDate_End_Permit};
            foreach (Control contr in controls)
            {
                if (contr.Text == "" || contr.Text == null)
                {
                    MessageBox.Show("Вы не ввели данные.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    contr.Focus();
                    return;
                }
            }
            ((CurrencyManager)BindingContext[permit]).EndCurrentEdit();
            permit.Save();
            Connect.Commit();
            permit.Clear();
            EnableControls(false);
            _dtEmpPermit.Clear();
            _dtEmpPermit.Fill();
            btExit.Focus();
        }

        private void tsbRefreshPermit_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            _dtEmpPermit.Clear();
            _dtEmpPermit.Fill();
        }

        private void dgEmpPermit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если сотрудник уволен красим строку в серый цвет
            if (dgEmpPermit["FL_ARCHIV", e.RowIndex].Value.ToString() == "1")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
        }

        private void tschShowAllPermit_CheckedChanged(object sender, EventArgs e)
        {
            if (tschShowAllPermit.Checked)
            {
                _dtEmpPermit.DefaultView.RowFilter = "";
            }
            else
            {
                _dtEmpPermit.DefaultView.RowFilter = string.Format(
                    "DATE_START_PERMIT <= #{0}# and DATE_END_PERMIT >= #{0}#",
                    DateTime.Today.ToString("MM/dd/yyyy"));
            }
        }
    }
}
