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
using Oracle.DataAccess.Client;
using System.Threading;
namespace Kadr.Vacation_schedule
{
    public partial class ConfirmVS : Form
    {
        private DataSet ds = new DataSet();
        private BackgroundWorker b_loader;
        public ConfirmVS()
        {
            InitializeComponent();
            panel_conf_commmands.EnableByRules(false);
            tsbtEditVs.Enabled = btEditVS.Enabled;
            ds.Tables.Add("check");
            DataGridViewCheckBoxColumn c1 = new DataGridViewCheckBoxColumn();
            c1.Name="FL";
            gridConfirmVS.AutoGenerateColumns = false;
            c1.DataPropertyName="FL";
            c1.HeaderText = "Отмечено";
            gridConfirmVS.Columns.Add(c1);
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("per_num", "Таб.№", "per_num",true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("fio", "ФИО", "fio", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("sign_comb", "Совм.", "sign_comb", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("code_degree", "Категория", "code_degree", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("last_vac", "Последний отпуск", "last_vac", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("last_period", "Использован по", "last_period", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("next_vac", "Следующий отпуск", "next_vac", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("plan_begin", "Запланирован отпуск на текущий год", "plan_begin", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("to_be_period", "Будет использован по", "to_be_period", true));
            gridConfirmVS.Columns.Add(new MDataGridViewTextBoxColumn("fl_check", "Статус проверки на ошибки", "fl_check", true));
            gridConfirmVS.RowsAdded += new DataGridViewRowsAddedEventHandler(gridConfirmVS_RowsAdded);
            gridConfirmVS.CellFormatting += new DataGridViewCellFormattingEventHandler(gridConfirmVS_CellFormatting);
            ColumnWidthSaver.FillWidthOfColumn(gridConfirmVS);
            gridConfirmVS.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            this.FormClosing += new FormClosingEventHandler(ConfirmVS_FormClosing);
            FilterVS.FilterChanged+=new EventHandler(FillGrid);
            tsbtRefreshConfVS.Enabled = true;
            b_loader = new BackgroundWorker();
            b_loader.DoWork += new DoWorkEventHandler(checker_DoWork);
            b_loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(b_loader_RunWorkerCompleted);
            b_loader.WorkerSupportsCancellation = true;
            FillGrid(null, null);
        }

        void ConfirmVS_FormClosing(object sender, FormClosingEventArgs e)
        {
            b_loader.CancelAsync();
            b_loader.Dispose();
        }

        void gridConfirmVS_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && gridConfirmVS.Columns[e.ColumnIndex].DataPropertyName.ToUpper() == "FL_CHECK")
            {
                if (!string.IsNullOrEmpty(e.Value.ToString()))
                    e.CellStyle.SelectionForeColor = e.CellStyle.BackColor = Color.LightCoral;
                else
                    e.CellStyle.SelectionForeColor = e.CellStyle.BackColor = gridConfirmVS.Rows[e.RowIndex].DefaultCellStyle.BackColor;
            }
        }

        void gridConfirmVS_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i=e.RowIndex, k=0;k<e.RowCount;++k,++i)
            {
                if ((gridConfirmVS.Rows[i].DataBoundItem as DataRowView)["CONFIRM_SIGN"].ToString() == "1")
                    gridConfirmVS.Rows[i].DefaultCellStyle.BackColor =
                    gridConfirmVS.Rows[i].DefaultCellStyle.SelectionForeColor = Color.LightGreen;
                else
                    gridConfirmVS.Rows[i].DefaultCellStyle.BackColor =
                    gridConfirmVS.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }

        void b_loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.IsDisposed || this.Disposing)
                return;
            if (e.Error==null)
                new ToolTip().Show("Проверка завершена", this, 10, 10, 5000);   
            else
                new ToolTip().Show("Проверка завершена c ошибкой: "+e.Error.Message, this, 10, 10, 5000);   
        }

        void checker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery(@"go/CheckVSForConfirm.sql"), Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
                DataTable t = new DataTable();
                new OracleDataAdapter(cmd).Fill(t);
                if (b_loader.CancellationPending) { e.Cancel = true; return; }
                Dictionary<decimal, DataRow> d = new Dictionary<decimal, DataRow>();
                foreach (DataRow r in t.Rows)
                {
                    if (b_loader.CancellationPending) { e.Cancel = true; return; }
                    d.Add(r.Field<Decimal>("transfer_id"), r);
                }
                foreach (DataRow r in ds.Tables["ConfirmVS"].Rows)
                {
                    if (b_loader.CancellationPending) { e.Cancel = true; return; }
                    r["fl_check"] = (d.ContainsKey(r.Field<Decimal>("transfer_id"))?d[r.Field<Decimal>("transfer_id")]["err_text"]:(object)"");
                }
            }
            catch
            { }
        }

        private DataTable ConfirmT
        {
            get
            {
                return ds.Tables["ConfirmVS"];
            }
        }

        public void FillGrid(object sender, EventArgs e)
        {
            if (gridConfirmVS == null) return;
            int i = gridConfirmVS.FirstDisplayedScrollingRowIndex, cur_x = 0;
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery("GO/GetVacForConfirm.sql"), Connect.Schema), Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_date", new DateTime(FilterVS.YearVS, 1, 1));
            a.SelectCommand.Parameters.Add("p_subdiv", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
            if (ds.Tables.Contains("ConfirmVS"))
                ConfirmT.Rows.Clear();
            a.Fill(ds, "ConfirmVS");
            if (gridConfirmVS.DataSource == null)
                gridConfirmVS.DataSource = new DataView(ConfirmT, string.Join(" and ", new string[] { "", (only_conf.Checked ? " confirm_sign=0" : "") }.Where(t => t != string.Empty).ToArray()), "", DataViewRowState.CurrentRows);
            else
                (gridConfirmVS.DataSource as DataView).RowFilter = string.Join(" and ", new string[] { "", (only_conf.Checked ? " confirm_sign=0" : "") }.Where(t => t != string.Empty).ToArray());
            cur_x = gridConfirmVS.FirstDisplayedScrollingColumnIndex;
            checkAll.Checked = false;
            if (b_loader.IsBusy)
                b_loader.CancelAsync();
            if (!b_loader.IsBusy)
                b_loader.RunWorkerAsync();
            try
            {
                gridConfirmVS.FirstDisplayedCell = gridConfirmVS[cur_x, 0];
                gridConfirmVS.FirstDisplayedScrollingRowIndex = i;
            }
            catch { }
        }
        
        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Вы действительно хотите {0} для выбранных отпусков?",(sender == btConfirmVS ?"установить признак 'СОГЛАСОВАНО'":"отменить согласование")),
                    "АРМ Графики отпусков", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.CONFIRM_VAC(:p_vacs, :p_sign);end;", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_vacs", OracleDbType.Varchar2, "", ParameterDirection.Input);
                cmd.Parameters.Add("p_sign", OracleDbType.Decimal, sender == btConfirmVS ? 1 : 0, ParameterDirection.Input);
                gridConfirmVS.CommitEdit(DataGridViewDataErrorContexts.Commit);
                try
                {
                    OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        string[] l = ConfirmT.Rows.Cast<DataRow>().Where(t => t["VAC_SCHED_ID"] != DBNull.Value && t["FL"].ToString() == "True").Select<DataRow, string>(u => u["VAC_SCHED_ID"].ToString()).ToArray();
                        if (l.Length == 0) { tr.Commit(); return; }
                        for (int i = 0; i < l.Length / 500; ++i)
                        {
                            cmd.Parameters["p_vacs"].Value = string.Join(",", l, 500 * i, 500);
                            cmd.ExecuteNonQuery();
                        }
                        if (l.Length % 500 > 0)
                        {
                            cmd.Parameters["p_vacs"].Value = string.Join(",", l.ToArray(), l.Length - l.Length % 500, l.Length % 500);
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();
                        ConfirmT.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show("Ошибка согласования: " + ex.Message);
                    }
                }
                catch { };
                FillGrid(null,null);
            }
        }

        private void Confirm_Vac_Schedule_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (b_loader.IsBusy)
                b_loader.CancelAsync();
            ((FormMain)Application.OpenForms["FormMain"]).UpdateButtonsState_VS(this);
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRowView r in (gridConfirmVS.DataSource as DataView))
                r["FL"] = checkAll.Checked;
        }

        private void gridConfirmVS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridConfirmVS.CurrentRow!=null)
            {
                DataRow r = (gridConfirmVS.CurrentRow.DataBoundItem as DataRowView).Row;
                ViewCard frm = new ViewCard(r["transfer_id"].ToString(), r["vac_sched_id"].ToString(), false);
                frm.ShowDialog();
                //FillGrid(null,null);
            }
        }

        private void btAddVS_Click(object sender, EventArgs e)
        {
            if (gridConfirmVS.CurrentRow != null)
            {
                EditVac f = new EditVac((gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), true);
                if (f.ShowDialog(this) == DialogResult.OK)
                    FillGrid(null,null);
            }
        }

        private void личнаяКарточкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridConfirmVS.CurrentRow != null)
            {
                ViewCard f = new ViewCard((gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), (gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["vac_sched_id"].ToString());
                f.ShowDialog(this);
            }
        }

        private void btEditVS_Click(object sender, EventArgs e)
        {
            if (gridConfirmVS.CurrentRow != null && (gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["vac_sched_id"] != DBNull.Value)
            {
                EditVac f = new EditVac((gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["vac_sched_id"], (gridConfirmVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), true);
                if (f.ShowDialog(this) == DialogResult.OK)
                    FillGrid(null,null);
            }
        }

        private void only_conf_CheckedChanged(object sender, EventArgs e)
        {
            DataView d = gridConfirmVS.DataSource as DataView;
            d.RowFilter = string.Join(" and ", new string[]{"", (only_conf.Checked ? "confirm_sign=0" : "")}.Where(t=>t!=string.Empty).ToArray());
        }

        private void tsbtAllSubdivVacConfirmStatistic_Click(object sender, EventArgs e)
        {
            ConfirmPercentView f = new ConfirmPercentView(FilterVS.YearVS);
            f.TopMost = true;
            f.Show(this);
        }

    }
}

