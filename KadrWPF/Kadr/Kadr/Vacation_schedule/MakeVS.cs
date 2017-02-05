using System;
using System.Data;
using System.Windows.Forms;

using LibraryKadr;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using Staff;
using Oracle.DataAccess.Client;
namespace Kadr.Vacation_schedule
{
    public partial class MakeVS : Form, IDataLinkKadr
    {
        private Timer tmr;
        private OracleDataAdapter a;
        DataTable t = new DataTable();
        public MakeVS()
        {
            InitializeComponent();
            viewVacsControl1.Owner32Window = this;
            /*FilterDateBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            FilterDateEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            a= new OracleDataAdapter(string.Format(Queries.GetQuery("Go/FillMakeVS.sql"),Connect.Schema),Connect.CurConnect);
            a.SelectCommand.BindByName=true;
            a.SelectCommand.Parameters.Add("emp_last_name", OracleDbType.Varchar2, txtFam.Text.Trim(), ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("emp_first_name", OracleDbType.Varchar2, txtName.Text.Trim(), ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("emp_middle_name", OracleDbType.Varchar2, txtS_name.Text.Trim(), ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date);
            a.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date);
            a.SelectCommand.Parameters.Add("p_group_master", OracleDbType.Varchar2, group_master.Text.Trim(), ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(FilterVS.YearVS,1,1), ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("degree_id", OracleDbType.Decimal, FilterVS.Degree_id, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, FilterVS.per_num, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            GridMakeVS.AutoGenerateColumns = false;
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("code_subdiv", "Подразделение", "code_subdiv"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("pos_name", "Должность", "pos_name"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("FIO", "ФИО", "FIO"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("per_num", "Таб.№", "per_num"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("sign_comb", "Совм.", "sign_comb"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("code_degree", "Категория", "code_degree"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("next_vac", "Следующий отпуск в году", "next_vac"));
            GridMakeVS.Columns.Add(new MDataGridViewTextBoxColumn("name_group_master", "Группа мастера", "name_group_master"));
            GridMakeVS.DataSource = new DataView(t, "", "", DataViewRowState.CurrentRows);
            AddFilterPanel.StateHideChanded += new EventHandler(btFind_Click);
            GridMakeVS.SortCompare+= new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_vacs_emp.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_vacs_emp.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            grid_vacs_emp.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_vacs_emp_CellFormatting);
            GridMakeVS.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            GridMakeVS.CurrentCellChanged += new EventHandler(GridMakeVS_CurrentCellChanged);
            this.Load += new EventHandler(MakeVS_Load);
            FilterVS.FilterChanged += new EventHandler(btFind_Click);
            tmr = new Timer();
            tmr.Interval = 9000;
            tmr.Tick += new EventHandler(tmr_tick);
            tmr.Start();
            if (GridMakeVS.ContextMenuStrip != null)
                GridMakeVS.ContextMenuStrip.Items.Add(ListLinkKadr.GetMenuItem(this));*/
        }
        /*
        void grid_vacs_emp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            grid_vacs_emp.Rows[e.RowIndex].DefaultCellStyle.BackColor =
            grid_vacs_emp.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = (grid_vacs_emp["CLOSE_SIGN", e.RowIndex].Value.ToString() == "0" ? Color.White : Color.LightGray);
        }

        void MakeVS_Load(object sender, EventArgs e)
        {
            btFind_Click(this, null);
        }

        void GridMakeVS_CurrentCellChanged(object sender, EventArgs e)
        {
            if (GridMakeVS.CurrentRow != null)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\GetEmpVacForMakeGrid.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_transfer_now", (GridMakeVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"]);
                a.Fill(t);
                grid_vacs_emp.DataSource = t;
                grid_vacs_emp.Columns["vac_id"].Visible = false;
                grid_vacs_emp.Columns["close_sign"].Visible = false;
                ColumnWidthSaver.FillWidthOfColumn(grid_vacs_emp);
            }
            else grid_vacs_emp.DataSource = null;
        }
        
        private void tmr_tick(object sender, EventArgs e)
        {
            Reminder.ShowForm(this);
            tmr.Stop();
        }
        
        private void frmForm_Vacation_Schedule_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ((FormMain)Application.OpenForms["FormMain"]).UpdateButtonsState_VS(this);
                this.Dispose(true);
            }
            catch 
            {
                this.Dispose(true);
            }
        }
        
        private void GridFormSchedule_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GridMakeVS.CurrentRow!=null && e.RowIndex > -1)
            {
                ViewCard frm = new ViewCard((GridMakeVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), grid_vacs_emp["vac_id", e.RowIndex].Value.ToString());
                frm.ShowDialog();
                GridMakeVS_CurrentCellChanged(this, EventArgs.Empty);
            }
        }
        
        public void btFind_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (this.Created)
                this.BeginInvoke(new EventHandler(FillVacs), sender, e);
            this.Cursor = Cursors.Default;
        }
        private void FillVacs(object sender, EventArgs e)
        {
            if (!AddFilterPanel.IsHidden)
            {
                FilterVS.per_num = null;
                a.SelectCommand.Parameters["emp_last_name"].Value = txtFam.Text.Trim();
                a.SelectCommand.Parameters["emp_first_name"].Value = txtName.Text.Trim();
                a.SelectCommand.Parameters["emp_middle_name"].Value = txtS_name.Text.Trim();
                if (check_UsedDateInFilter.Checked) a.SelectCommand.Parameters["p_date1"].Value = FilterDateBegin.Value.Date; else a.SelectCommand.Parameters["p_date1"].Value = null;
                if (check_UsedDateInFilter.Checked) a.SelectCommand.Parameters["p_date2"].Value = FilterDateEnd.Value.Date; else a.SelectCommand.Parameters["p_date2"].Value = null;
                a.SelectCommand.Parameters["p_group_master"].Value = group_master.Text.Trim();
            }
            else
            {
                a.SelectCommand.Parameters["emp_last_name"].Value =
                a.SelectCommand.Parameters["emp_first_name"].Value =
                a.SelectCommand.Parameters["emp_middle_name"].Value =
                a.SelectCommand.Parameters["p_date1"].Value =
                a.SelectCommand.Parameters["p_date2"].Value =
                a.SelectCommand.Parameters["p_group_master"].Value = DBNull.Value;
            }
            a.SelectCommand.Parameters["p_per_num"].Value = FilterVS.per_num;
            a.SelectCommand.Parameters["p_date"].Value = new DateTime(FilterVS.YearVS, 1, 1);
            a.SelectCommand.Parameters["p_subdiv_id"].Value = FilterVS.subdiv_id;
            a.SelectCommand.Parameters["degree_id"].Value = FilterVS.Degree_id;

            int FDSRI;
            decimal SelKey;
            if (GridMakeVS.CurrentRow != null)
                SelKey = (decimal)(GridMakeVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"];
            else SelKey = -1m;
            FDSRI = GridMakeVS.FirstDisplayedScrollingRowIndex;
            t.Rows.Clear();
            a.Fill(t);
            ColumnWidthSaver.FillWidthOfColumn(GridMakeVS);
            if (FDSRI > -1 && FDSRI < t.Rows.Count) GridMakeVS.FirstDisplayedScrollingRowIndex = FDSRI;
            DataView d = GridMakeVS.DataSource as DataView;
            for (int i = 0; i < d.Count; ++i)
                if ((decimal)d[i]["transfer_id"] == SelKey)
                {
                    GridMakeVS.Rows[i].Selected = true;
                    GridMakeVS.CurrentCell = GridMakeVS[GridMakeVS.Columns.GetFirstColumn(DataGridViewElementStates.Visible).Name, i];
                    break;
                }
        }
        
        private void check_UsedDateInFilter_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = check_UsedDateInFilter.Checked;
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GridMakeVS.CurrentRow != null)
            {
                ViewCard frm = new ViewCard((GridMakeVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), null);
                frm.ShowDialog();
                btFind_Click(null, null);
            }
        }

        private void проверитьНапоминаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Reminder.ShowForm(this))
                MessageBox.Show(this, "Неоформленных отпусков на текущий день по выбранному подразделению не найдено!", "АРМ Кадры");
        }

        
        private void GridMakeVS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GridMakeVS.CurrentRow != null)
            {
                ViewCard frm = new ViewCard((GridMakeVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), null);
                frm.ShowDialog();
                GridMakeVS_CurrentCellChanged(this, EventArgs.Empty);
            }
        }*/

        public ViewVacsViewModel Model
        {
            get
            {
                return (elementHost1.Child as ViewVacsControl).Model;
            }
        }

        public LinkData GetDataLink(object sender)
        {
            if (this.GridMakeVS.CurrentRow != null)
                return new LinkData(null, (GridMakeVS.CurrentRow.DataBoundItem as DataRowView).Row.Field<Decimal>("transfer_id"));
            else
                return null;
        }
    }
}
