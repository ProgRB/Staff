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

namespace Kadr.Shtat
{
    public partial class ReplEmpForm : Form
    {
        private DataSet ds = new DataSet();
        private string _SubRole = null;
        private void FillReplTable(DataGridView d,string transfe_id,decimal sign_combine,decimal whos_repl)
        {
            try
            {
                string cur_id = null;
                if (d.CurrentRow != null)
                    cur_id = d.CurrentRow.Cells["repl_emp_id"].Value.ToString();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\FillReplEmp.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("repl_tr_id", transfe_id);
                a.SelectCommand.Parameters.Add("sign_combine", sign_combine);
                a.SelectCommand.Parameters.Add("whos_repl", whos_repl);
                DataTable t = new DataTable();
                a.Fill(t);
                d.DataSource = t;
                d.Columns["repl_emp_id"].Visible = false;
                d.Columns["SIGN_LOCK_REPL"].Visible = false;
                for (int i = 0; i < d.Rows.Count; ++i)
                    if (d["repl_emp_id", i].Value.ToString() == cur_id)
                    {
                        d.CurrentCell = d[d.Columns.GetFirstColumn(DataGridViewElementStates.Visible).Name, i];
                        break;
                    }
            }
            catch
            { }
            Settings.SetDataGridCaption(ref d);
            ColumnWidthSaver.FillWidthOfColumn(d);
        }

        private bool DeleteRepl(object repl_emp_id)
        {
            if (MessageBox.Show("Удалить выбранное замещение/совмещение?", "АРМ Кадры", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                try
                {
                    OracleCommand cmd = new OracleCommand(string.Format("BEGIN {0}.REPL_EMP_DELETE(:p_repl_id);end;", DataSourceScheme.SchemeName), Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_repl_id", OracleDbType.Decimal, repl_emp_id, ParameterDirection.Input);
                    cmd.ExecuteNonQuery();
                    tr.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show(Library.GetMessageException(ex));
                }
            }
            return false;
        }

        private bool _showAddCombine;
        public ReplEmpForm(decimal? transfer_id, string SubRole =  null, bool show_add_combine = false)
        {
            InitializeComponent();
            _SubRole = SubRole;
            grid_CombineEmp.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            grid_ReplEmp.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            grid_ReplEmp.SortCompare+=new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_CombineEmp.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_whosRepl.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            grid_whosRepl.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_WhosCombine.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            grid_WhosCombine.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            grid_CombineEmp.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CombineEmp_CellFormatting);
            grid_ReplEmp.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CombineEmp_CellFormatting);
            grid_whosRepl.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CombineEmp_CellFormatting);
            grid_WhosCombine.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CombineEmp_CellFormatting);
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\GetEmpData.sql"),DataSourceScheme.SchemeName),Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_transfer_id",transfer_id);
            a.Fill(ds, "emp_data");
            if (ds.Tables["emp_data"].Rows.Count>0)
            {
                l_per_num.Text = ds.Tables["emp_data"].Rows[0]["per_num"].ToString();
                l_FIO.Text = string.Format("{0} {1} {2}", ds.Tables["emp_data"].Rows[0]["emp_last_name"].ToString(), ds.Tables["emp_data"].Rows[0]["emp_first_name"], ds.Tables["emp_data"].Rows[0]["emp_middle_name"]);
                l_pos_name.Text = ds.Tables["emp_data"].Rows[0]["pos_name"].ToString();
                l_degree.Text = ds.Tables["emp_data"].Rows[0]["degree_name"].ToString();
                l_salary.Text = ds.Tables["emp_data"].Rows[0]["salary"].ToString();
                picturePhoto.Image = Staff.EmployeePhoto.GetPhoto(ds.Tables["emp_data"].Rows[0]["per_num"].ToString());
            }
            ToolsStripCombine1.EnableByRules(false);
            toolStripcombRepl2.EnableByRules(false);
            ToolStripReplEmp1.EnableByRules(false);
            ToolStripReplEmp2.EnableByRules(false);
            tabs_repl_Selected(null,new TabControlEventArgs(tabs_repl.TabPages[0],0, TabControlAction.Selected));
            _showAddCombine = show_add_combine;
            this.Shown += new EventHandler(ReplEmpForm_Shown);
        }

        /// <summary>
        /// Процедура показа формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReplEmpForm_Shown(object sender, EventArgs e)
        {
            if (_showAddCombine)// если надо добавлять сразу совмщещаемого - то показываем форму
            {
                tabs_repl.SelectedIndex = 1;
                btNewCombReplEmp_Click(this, EventArgs.Empty);
            }
        }

        void grid_CombineEmp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && (sender as DataGridView).Columns[e.ColumnIndex].Name.ToUpper()=="REPL_START")
            {
                DataGridViewRow r = (sender as DataGridView).Rows[e.RowIndex];
                r.DefaultCellStyle.BackColor = r.DefaultCellStyle.SelectionForeColor = ((sender as DataGridView).Rows[e.RowIndex].DataBoundItem as DataRowView)["SIGN_LOCK_REPL"].ToString() == "1" ? 
                    ((r.DataBoundItem as DataRowView)["REPL_START"]==DBNull.Value || (DateTime)(r.DataBoundItem as DataRowView)["REPL_START"]>=DateTime.Now.Date.AddDays(-DateTime.Now.Date.Day+1)?Color.LightGreen: Color.LimeGreen) : Color.White;
            }
        }  

        private void tabs_repl_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0: FillReplTable(grid_ReplEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 0); break;
                case 1: FillReplTable(grid_CombineEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 0); break;
                case 2: FillReplTable(grid_whosRepl, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 1); break;
                case 3: FillReplTable(grid_WhosCombine, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 1); break;
            }
        }

        private void btNewReplReplEmp_Click(object sender, EventArgs e)
        {
            if (new ReplEmpAdd(null, ds.Tables["emp_data"].Rows[0]["transfer_id"], false).ShowDialog(this) == DialogResult.OK)
            {
                FillReplTable(grid_ReplEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(),0,0);
            }
        }

        private void btEditReplReplEmp_Click(object sender, EventArgs e)
        {
            if (grid_ReplEmp.CurrentRow!=null && new ReplEmpAdd((grid_ReplEmp.CurrentRow.DataBoundItem as DataRowView)["repl_emp_id"], ds.Tables["emp_data"].Rows[0]["transfer_id"], false).ShowDialog(this)== DialogResult.OK)
                FillReplTable(grid_ReplEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 0);
        }

        private void btDelReplReplEmp_Click(object sender, EventArgs e)
        {
            if (grid_ReplEmp.CurrentRow != null && DeleteRepl((grid_ReplEmp.CurrentRow.DataBoundItem as DataRowView)["repl_emp_id"]))
                FillReplTable(grid_ReplEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 0);
        }

        private void btNewCombReplEmp_Click(object sender, EventArgs e)
        {
            if (new ReplEmpAdd(null, ds.Tables["emp_data"].Rows[0]["transfer_id"], true).ShowDialog(this) == DialogResult.OK)
            {
                FillReplTable(grid_CombineEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 0);
            }
        }

        private void btEditCombReplEmp_Click(object sender, EventArgs e)
        {
            if (grid_CombineEmp.CurrentRow != null && new ReplEmpAdd(grid_CombineEmp.CurrentRow.Cells["repl_emp_id"].Value, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), true).ShowDialog(this) == DialogResult.OK)
                FillReplTable(grid_CombineEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 0);
        }

        private void btDeleteCombReplEmp_Click(object sender, EventArgs e)
        {
            if (grid_CombineEmp.CurrentRow != null && DeleteRepl(grid_CombineEmp.CurrentRow.Cells["repl_emp_id"].Value))
                FillReplTable(grid_CombineEmp, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 0);
        }

        
        private void tsbtAddReplEmpWhos_Click(object sender, EventArgs e)
        {
            ReplEmpAdd f = new ReplEmpAdd(null, null, ds.Tables["emp_data"].Rows[0]["transfer_id"], false);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                FillReplTable(grid_whosRepl, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 1);
            }
        }

        private void tsbtEditReplEmpWhos_Click(object sender, EventArgs e)
        {
            if (grid_whosRepl.CurrentRow != null && new ReplEmpAdd(grid_whosRepl.CurrentRow.Cells["repl_emp_id"].Value, ds.Tables["emp_data"].Rows[0]["transfer_id"], false).ShowDialog(this) == DialogResult.OK)
                FillReplTable(grid_whosRepl, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 1);
        }

        private void tsbtDeleteReplEmpWhos_Click(object sender, EventArgs e)
        {
            if (grid_whosRepl.CurrentRow!=null && DeleteRepl(grid_whosRepl.CurrentRow.Cells["repl_emp_id"].Value))
                FillReplTable(grid_whosRepl, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 0, 1);
        }

        private void tsbtAddCombineEmpWhos_Click(object sender, EventArgs e)
        {
            ReplEmpAdd f = new ReplEmpAdd(null, null, ds.Tables["emp_data"].Rows[0]["transfer_id"], true);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                FillReplTable(grid_WhosCombine, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 1);
            }
        }

        private void tsbtEditCombineEmpWhos_Click(object sender, EventArgs e)
        {
            if (grid_WhosCombine.CurrentRow != null && new ReplEmpAdd(grid_WhosCombine.CurrentRow.Cells["repl_emp_id"].Value, ds.Tables["emp_data"].Rows[0]["transfer_id"],true).ShowDialog(this) == DialogResult.OK)
                FillReplTable(grid_WhosCombine, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 1);
        }

        private void tsbtDeleteCombineEmpWhos_Click(object sender, EventArgs e)
        {
            if (grid_WhosCombine.CurrentRow != null && DeleteRepl(grid_WhosCombine.CurrentRow.Cells["repl_emp_id"].Value))
                FillReplTable(grid_WhosCombine, ds.Tables["emp_data"].Rows[0]["transfer_id"].ToString(), 1, 1);
        }
#region Отчеты

        public decimal? SubdivID
        {
            get
            {
                return (decimal?)ds.Tables["emp_data"].Rows[0]["subdiv_id"];
            }
        }
        private void ReportCommand_Click(object sender, EventArgs e)
        {
            /*DateTime t;
            ReportsReplEmp f;
            if (grid_CombineEmp.CurrentRow!=null && DateTime.TryParse(grid_CombineEmp.CurrentRow.Cells["repl_start"].Value.ToString(),out t))
                f= new ReportsReplEmp(TypeReportRepl.ServiseNote,(decimal?)ds.Tables["emp_data"].Rows[0]["subdiv_id"],t, _SubRole);
            else
                f = new ReportsReplEmp(TypeReportRepl.ServiseNote, (decimal?)ds.Tables["emp_data"].Rows[0]["subdiv_id"], null, _SubRole);
            f.Show(this);*/
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int tp_report = Convert.ToInt32(item.Tag);
            ReportsReplEmp f = new ReportsReplEmp((TypeReportRepl)tp_report, SubdivID, CurrentDGRow() == null ? null : (DateTime?)CurrentDGRow().Row.Field<DateTime>("REPL_START"), _SubRole,
                (tabs_repl.SelectedTab== tp_combines || tabs_repl.SelectedTab== tab_whose_combine) );
            f.Show(this);

        }

        private DataRowView CurrentDGRow()
        {
            if (tabs_repl.SelectedTab != null)
            {
                TabPage t = tabs_repl.SelectedTab;
                Func<Control, Type, object> find_grid = null;
                find_grid = (e, type) =>
                    {
                        if (e.GetType() == type) return e;
                        else
                            foreach (Control c in e.Controls)
                            {
                                if (c is DataGridView)
                                    return c;
                                else
                                {
                                    object c1 = find_grid(c, type);
                                    if (c1 != null) return c1;
                                }
                            }
                        return null;
                    };
                DataGridView d= (DataGridView)find_grid(t, typeof(DataGridView));
                if (d != null && d.CurrentRow != null)
                    return d.CurrentRow.DataBoundItem as DataRowView;
            }
            return null;
        }
#endregion

        /// <summary>
        /// Статичная форма для показа совмещений и автопоказа формы добавления совмещения
        /// </summary>
        /// <param name="transfer_id"></param>
        /// <param name="sender"></param>
        public static void AddNewCombine(decimal? transfer_id, Form sender)
        {
            ReplEmpForm f = new ReplEmpForm(transfer_id, null, true);
            f.ShowDialog(sender);
        }
    }
}
