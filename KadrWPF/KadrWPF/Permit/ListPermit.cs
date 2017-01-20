using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Kadr;
using Oracle.DataAccess.Client;

namespace ARM_PROP
{
    public partial class ListPermit : System.Windows.Forms.UserControl
    {
        OracleDataTable dtPermit, dtEmpPermit;

        string strFilterPermit = "";
        BindingSource bsPermit;
        public ListPermit(string _strFilter)
        {
            InitializeComponent();
            strFilterPermit = _strFilter;
            bsPermit = new BindingSource();
            dtEmpPermit = new OracleDataTable("", Connect.CurConnect);
            dtEmpPermit.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("PermitForEmp.sql"), Connect.Schema);
            dtEmpPermit.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = -1;
            dtEmpPermit.Fill();
            dtEmpPermit.DefaultView.RowFilter = string.Format(
                "DATE_START_PERMIT <= #{0}# and DATE_END_PERMIT >= #{0}#",
                DateTime.Today.ToString("MM/dd/yyyy"));
            dgEmpPermit.DataSource = dtEmpPermit.DefaultView;
            dgEmpPermit.Columns["permit_id"].Visible = false;
            dgEmpPermit.Columns["FL_ARCHIV"].Visible = false;
            dgEmpPermit.Columns["NUM_DOC_PERMIT"].HeaderText = "№ служебной";
            dgEmpPermit.Columns["DATE_DOC_PERMIT"].HeaderText = "Дата служебной";
            dgEmpPermit.Columns["PERMIT_NAME"].HeaderText = "Наименование разрешения";
            dgEmpPermit.Columns["DATE_START_PERMIT"].HeaderText = "Дата начала разрешения";
            dgEmpPermit.Columns["DATE_END_PERMIT"].HeaderText = "Дата окончания разрешения";
            dtPermit = new OracleDataTable("", Connect.CurConnect);
            dtPermit.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("SelectPermit.sql"), Connect.Schema, strFilterPermit);
            dtPermit.SelectCommand.BindByName = true;
            dtPermit.SelectCommand.Parameters.Add("p_SYSDATE", OracleDbType.Date).Value = DateTime.Today;
            dtPermit.Fill();
            bsPermit.DataSource = dtPermit;
            dgPermit.DataSource = bsPermit;
            dgPermit.Columns["TRANSFER_ID"].Visible = false;
            dgPermit.Columns["WORKER_ID"].Visible = false;
            dgPermit.Columns["code_subdiv"].HeaderText = "Подр.";
            dgPermit.Columns["countP"].HeaderText = "Кол-во разрешений (действующие)";
            dgPermit.Columns["countPAll"].HeaderText = "Кол-во разрешений (всего)";
            dgPermit.Columns["COMB"].HeaderText = "Совм.";
            dgPermit.Columns["PER_NUM"].HeaderText = "Таб.№";
            dgPermit.Columns["EMP_LAST_NAME"].HeaderText = "Фамилия";
            dgPermit.Columns["EMP_FIRST_NAME"].HeaderText = "Имя";
            dgPermit.Columns["EMP_MIDDLE_NAME"].HeaderText = "Отчество";
            dgPermit.Columns["POS_NAME"].HeaderText = "Наименование профессии";
            dgPermit.Columns["PER_NUM"].Width = 55;
            dgPermit.Columns["code_subdiv"].Width = 50;
            dgPermit.Columns["EMP_LAST_NAME"].Width = 120;
            dgPermit.Columns["EMP_FIRST_NAME"].Width = 100;
            dgPermit.Columns["EMP_MIDDLE_NAME"].Width = 130;
            dgPermit.Columns["countP"].Width = 110;
            dgPermit.Columns["countPAll"].Width = 110;
            dgPermit.Columns["COMB"].Width = 50;
            dgPermit.Columns["POS_NAME"].Width = 500;
            dgPermit.Columns["PER_NUM"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["EMP_LAST_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["EMP_FIRST_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["EMP_MIDDLE_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["POS_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["countP"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgPermit.Columns["COMB"].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgPermit_SelectionChanged(object sender, EventArgs e)
        {
            dtEmpPermit.Clear();
            if (dgPermit.CurrentRow != null)
            {
                dtEmpPermit.SelectCommand.Parameters["p_WORKER_ID"].Value =
                    dgPermit.CurrentRow.Cells["WORKER_ID"].Value;
                dtEmpPermit.Fill();
            }
        }

        private void dgPermit_DoubleClick(object sender, EventArgs e)
        {
            if (dgPermit.CurrentRow != null)
            {
                EditPermit editPermit = new EditPermit( 
                    dgPermit.CurrentRow.Cells["code_subdiv"].Value.ToString(),
                    Convert.ToDecimal(dgPermit.CurrentRow.Cells["transfer_id"].Value),
                    dgPermit.CurrentRow.Cells["per_num"].Value.ToString(),
                    dgPermit.CurrentRow.Cells["emp_last_name"].Value.ToString(),
                    dgPermit.CurrentRow.Cells["emp_first_name"].Value.ToString(),
                    dgPermit.CurrentRow.Cells["emp_middle_name"].Value.ToString(),
                    dgPermit.CurrentRow.Cells["pos_name"].Value.ToString(),
                    dtEmpPermit);
                editPermit.ShowInTaskbar = false;
                editPermit.ShowDialog();
                int pos = bsPermit.Position;
                LoadPermit(strFilterPermit);
                bsPermit.Position = pos;
            }
        }

        public void LoadPermit(string _str)
        {
            strFilterPermit = _str;
            dtPermit.Clear();
            dtPermit.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("SelectPermit.sql"), DataSourceScheme.SchemeName, strFilterPermit);
            dtPermit.Fill();
        }

        private void chShowAllPermit_CheckedChanged(object sender, EventArgs e)
        {
            if (chShowAllPermit.Checked)
            {
                dtEmpPermit.DefaultView.RowFilter = "";
            }
            else
            {
                dtEmpPermit.DefaultView.RowFilter = string.Format(
                    "DATE_START_PERMIT <= #{0}# and DATE_END_PERMIT >= #{0}#", 
                    DateTime.Today.ToString("MM/dd/yyyy"));
            }
        }

        private void dgEmpPermit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если сотрудник уволен красим строку в серый цвет
            if (dgEmpPermit["FL_ARCHIV", e.RowIndex].Value.ToString() == "1")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
        }
    }
}
