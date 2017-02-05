using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Kadr;
using Elegant.Ui;
using System.IO;
using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;
namespace Kadr.Shtat
{
    public partial class VIEW : Form
    {
        public int GridRegSortedColumn=1, GridLayotSortedColumn=1,GridAllocSortedColumn=1;
       
        public VIEW()
        {
            InitializeComponent();
            tsbtTableModeShtat.Click += new EventHandler(ModePreviewClick);
            tsbtTreeModeShtat.Click+=new EventHandler(ModePreviewClick);
            GridRegistration.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            GridLayot.SortCompare += new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
            gridTempAlloc.SortCompare +=new DataGridViewSortCompareEventHandler(Library.DataGridView_CellsCompare);
        }

        private void ModePreviewClick(object sender, EventArgs e)
        {
            if (sender == tsbtTableModeShtat)
            {
                tsbtTreeModeShtat.Checked = false;
                ((ToolStripMenuItem)sender).Checked = true;
                GridLayot.Visible = true;
                treeStaff.Visible = false;
                FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, 0);
                GridLayot.Focus();
            }
            else
            {
                tsbtTableModeShtat.Checked = false;
                ((ToolStripMenuItem)sender).Checked = true;
                treeStaff.Visible = true;
                GridLayot.Visible = false;
                if (treeStaff.SelectedNode != null)
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name != "-1" ? treeStaff.SelectedNode.Name : treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);
                else
                    FillGridLayot(Convert.ToInt32(ShtatFilter.Subdiv_id), 0);
                treeStaff.Focus();
            }
        }
        
        public void FillGridLayot(int FirstVisibleRow,int SelectedRow)//для дерева это будет первый видимый узел и выбранный узел
        {
            if (tsbtTableModeShtat.Checked)
            {
                DataTable table = new DataTable();
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("new/FILL_VIEWGRID.sql"), DataSourceScheme.SchemeName,
                                                                                        (ShtatFilter.DegreeId == null? "" : string.Format(" and degree_id= {0} ",ShtatFilter.DegreeId))
                                                                                        + (ShtatFilter.TypeStaff==null ? "" : " and type_staff=" + ShtatFilter.TypeStaff.ToString())
                                                                                ), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_subdiv_id", ShtatFilter.Subdiv_id);
                cmd.Parameters.Add("p_cur_date", cur_date.Value);
                new OracleDataAdapter(cmd).Fill(table);
                GridLayot.DataSource = table;
                if (SelectedRow > -1 && SelectedRow < GridLayot.RowCount)
                    GridLayot.CurrentCell=GridLayot[GridLayot.Columns.GetFirstColumn(DataGridViewElementStates.Visible).Name,SelectedRow];
                ColumnWidthSaver.FillWidthOfColumn(GridLayot);
                Settings.SetDataGridCaption(ref GridLayot);
                Settings.SetDataGridColumnAlign(ref GridLayot);
            }
            else
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(string.Format("select subdiv_id from {0}.subdiv start with subdiv_id=:p_subdiv_id connect by prior parent_id=subdiv_id", DataSourceScheme.SchemeName),Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_subdiv_id",FirstVisibleRow);
                    DataTable table = new DataTable();
                    new OracleDataAdapter(cmd).Fill(table);
                    TreeNode t=treeStaff.Nodes[table.Rows[table.Rows.Count-1][0].ToString()];
                    for (int i=table.Rows.Count-2;i>-1;i--)
                    {
                        t.Collapse();
                        t.Expand();
                        t=t.Nodes[table.Rows[i][0].ToString()];
                    }
                    t.Collapse();
                    t.Expand();
                    if (t.Nodes.Count>SelectedRow)
                        treeStaff.SelectedNode = t.Nodes[SelectedRow];
                    else
                        treeStaff.SelectedNode = t;
                    treeStaff.SelectedNode.EnsureVisible();
                }
                catch
                {
                }
            }
        }
        public void FillGridInclude(int cur_row)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/gridInclude.sql"), DataSourceScheme.SchemeName, FormMain.ShtatFilter.subdiv_id,
                (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format(" and s4.degree_id=(select degree_id from {0}.degree where degree_name='{1}') ", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                + (FormMain.ShtatFilter.type_staff == 0 ? "" : " and type_staff=" + (FormMain.ShtatFilter.type_staff - 1).ToString())
                   ), Connect.CurConnect);
            adapter.SelectCommand.BindByName = true;
            DataTable table = new DataTable();
            adapter.SelectCommand.BindByName = true;
            adapter.Fill(table);
            GridInclude.Columns.Clear();
            DataGridViewCheckBoxColumn cl = new DataGridViewCheckBoxColumn();
            cl.Name = "checkIn";
            cl.HeaderText = "Отправлены";
            GridInclude.Columns.Add(cl);
            GridInclude.Columns[0].ReadOnly = false;
            GridInclude.Columns.Add("date_begin_staff", "Ввести с:");
            GridInclude.Columns.Add("code_pos", "Код профессии");
            GridInclude.Columns.Add("pos_name", "Профессия");
            GridInclude.Columns.Add("col_ed", "Количество единиц");
            GridInclude.Columns.Add("classific", "Разряд");
            GridInclude.Columns.Add("kf_schema", "Коэфф. по схеме");
            GridInclude.Columns.Add("kf_stavka", "Ставка");
            GridInclude.Columns.Add("in_r", "рубл.");
            GridInclude.Columns.Add("comb_addition", "Надбавка за вредн.");
            GridInclude.Columns.Add("in_r1", "Рубл.");
            GridInclude.Columns.Add("add_exp_area", "За расш.зоны.обсл.");
            GridInclude.Columns.Add("in_r2", "Рубл.");
            GridInclude.Columns.Add("harmful_add", "За вредность");
            GridInclude.Columns.Add("tar_grid", "Тарифная сетка");
            for (int i = 1; i < GridInclude.ColumnCount; i++)
                GridInclude.Columns[i].ReadOnly = true;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                GridInclude.Rows.Add();
                if (table.Rows[i]["fl"].ToString() == "1")
                {
                    GridInclude.Rows[i].Cells["checkIn"].Value = true;
                    GridInclude.Rows[i].Cells["checkIn"].ReadOnly = true;
                    GridInclude.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    GridInclude.Rows[i].DefaultCellStyle.SelectionForeColor = Color.LightGreen;
                }
                else
                    GridInclude.Rows[i].Cells["checkIn"].Value = false;
                GridInclude["date_begin_staff", i].Value = table.Rows[i]["Ввести с"].ToString();
                GridInclude["code_pos", i].Value = table.Rows[i]["Код профессии"].ToString();
                GridInclude["pos_name", i].Value = table.Rows[i]["Профессия"].ToString();
                GridInclude["col_ed", i].Value = table.Rows[i]["Количество единиц"].ToString();
                GridInclude["classific", i].Value = table.Rows[i]["Разряд"].ToString();
                GridInclude["kf_schema", i].Value = table.Rows[i]["Тарифный коэфф. по схеме"].ToString();
                GridInclude["kf_stavka", i].Value = table.Rows[i]["Тарифная ставка"].ToString();
                GridInclude["in_r", i].Value = table.Rows[i]["рубл."].ToString();
                GridInclude["comb_addition", i].Value = table.Rows[i]["Надбавка за совм"].ToString();
                GridInclude["in_r1", i].Value = table.Rows[i]["руб."].ToString();
                GridInclude["add_exp_area", i].Value = table.Rows[i]["Надбавка за расш. обслуж."].ToString();
                GridInclude["in_r2", i].Value = table.Rows[i]["pубл."].ToString();
                GridInclude["in_r2", i].Value = table.Rows[i]["Мес. фонд. ЗП"].ToString();
                GridInclude["harmful_add", i].Value = table.Rows[i]["Надбавка за вредн."].ToString();
                GridInclude["tar_grid", i].Value = table.Rows[i]["Тарифная сетка"].ToString();
            }
            if (cur_row < GridInclude.RowCount)
            {
                GridInclude.FirstDisplayedScrollingRowIndex = cur_row;
                GridInclude.Rows[cur_row].Selected = true;
            }
            Settings.SetDataGridCoumnWidth(ref GridInclude);
            Settings.SetDataGridColumnAlign(ref GridInclude);
            Settings.SetDataGridCaption(ref GridInclude);
        }
        public void FillGridExclude(int cur_row)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/GridExclude.sql"), 
                DataSourceScheme.SchemeName, 
                ShtatFilter.Subdiv_id,
                (ShtatFilter.DegreeId == null ? "" : string.Format(" and s4.degree_id={0} ", ShtatFilter.TypeStaff)
                )
                + (ShtatFilter.TypeStaff == null? "" : " and type_staff=" + ShtatFilter.TypeStaff.ToString())
                    ), Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.SelectCommand.BindByName = true;
            adapter.Fill(table);
            GridExclude.Columns.Clear();
            DataGridViewCheckBoxColumn cl = new DataGridViewCheckBoxColumn();
            cl.Name = "checkEx";
            cl.HeaderText = "Отправлены";
            GridExclude.Columns.Add(cl);
            GridExclude.Columns[0].ReadOnly = false;
            for (int i = 1; i < table.Columns.Count; i++)
            {
                GridExclude.Columns.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName);
                GridExclude.Columns[i].ReadOnly = true;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                GridExclude.Rows.Add();
                if (table.Rows[i]["fl"].ToString() == "1")
                {
                    GridExclude.Rows[i].Cells["checkEx"].Value = true;
                    GridExclude.Rows[i].Cells["checkEx"].ReadOnly = true;
                    GridExclude.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    GridExclude.Rows[i].DefaultCellStyle.SelectionForeColor = Color.LightGreen;
                }
                else
                    GridExclude.Rows[i].Cells["checkEx"].Value = false;
                for (int j = 0; j < table.Columns.Count - 1; j++)
                    GridExclude[j+1, i].Value = table.Rows[i][j].ToString();
            }
            if (cur_row < GridExclude.RowCount)
            {
                GridExclude.FirstDisplayedScrollingRowIndex = cur_row;
                GridExclude.Rows[cur_row].Selected = true;
            }

            Settings.SetDataGridCoumnWidth(ref GridExclude);
            Settings.SetDataGridColumnAlign(ref GridExclude);
            Settings.SetDataGridCaption(ref GridExclude);
        }
        public void FillGridRegistration(int SelectedRow, int FirstDisplRow)
        {
            ListSortDirection ListDirection = new ListSortDirection();
            if (GridRegistration.SortedColumn != null)
            {
                GridRegSortedColumn = GridRegistration.SortedColumn.Index + 1;
                ListDirection = GridRegistration.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            }
            else
            {
                // GridRegSortedColumn = 1;
                // ListDirection = ListSortDirection.Ascending;
            }
            OracleCommand cmd  = new OracleCommand(string.Format(Queries.GetQuery("new/FillGridRegistration.sql"), DataSourceScheme.SchemeName, (ShtatFilter.DegreeId ==null ? "" :string.Format( " and sf.degree_id={0}",ShtatFilter.DegreeId))
                + (ShtatFilter.TypeStaff == null ? "" : " and type_staff=" + ShtatFilter.TypeStaff.ToString())
                , "order by " + GridRegSortedColumn.ToString() + (ListDirection == ListSortDirection.Ascending ? " asc " : " desc ")
                ), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_subdiv_id",ShtatFilter.Subdiv_id);
            DataTable table = new DataTable();
            new OracleDataAdapter(cmd).Fill(table);
            GridRegistration.DataSource = table;
            GridRegistration.Columns["staffs_id"].Visible = false;
            GridRegistration.Columns["transfer_id"].Visible = false;
            GridRegistration.Columns["in_otpusk"].Visible = false;
            GridRegistration.Columns["need_transfer"].Visible = false;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["in_otpusk"] != DBNull.Value)
                {
                    GridRegistration.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(128, 128, 200);
                    GridRegistration.Rows[i].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionForeColor = Color.FromArgb(200, 200, 200);
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                }
                if (table.Rows[i]["need_transfer"].ToString() == "1")
                {
                    GridRegistration.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                }
            }
            if (FirstDisplRow < GridRegistration.RowCount)
                GridRegistration.FirstDisplayedScrollingRowIndex = FirstDisplRow;
            if (SelectedRow > -1 && SelectedRow < GridRegistration.RowCount)
                GridRegistration.Rows[SelectedRow].Selected = true;

            ColumnWidthSaver.FillWidthOfColumn(GridRegistration);
            Settings.SetDataGridCaption(ref GridRegistration);
            Settings.SetDataGridColumnAlign(ref GridRegistration);

        }
        public void FillGridTempAlloc(int SelectedRow, int FirstDisplRow)
        {
            ListSortDirection ListDirection = new ListSortDirection();
            if (gridTempAlloc.SortedColumn != null)
            {
                GridAllocSortedColumn = gridTempAlloc.SortedColumn.Index + 1;
                ListDirection = gridTempAlloc.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            }
            
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(Queries.GetQuery("new/FILL_TEMPALLOCSTAFF.sql"), DataSourceScheme.SchemeName, ShtatFilter.Subdiv_id,
                (ShtatFilter.DegreeId == null ? "" : " and degree_name='" + ShtatFilter.DegreeName + "' ")
                + (ShtatFilter.TypeStaff == null ? "" : " and type_staff=" + ShtatFilter.TypeStaff.ToString())
                , "order by " + GridAllocSortedColumn.ToString() + (ListDirection == ListSortDirection.Ascending ? " asc " : " desc ")
                ), Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.SelectCommand.BindByName = true;
            adapter.Fill(table);
            gridTempAlloc.DataSource = table;
            gridTempAlloc.Columns["staffs_id"].Visible = false;
            gridTempAlloc.Columns["repl_emp_id"].Visible = false;
            gridTempAlloc.Columns["t_subdiv_id"].Visible = false;
            gridTempAlloc.Columns["staffs_temp_id"].Visible = false;

            if (FirstDisplRow < gridTempAlloc.RowCount)
                gridTempAlloc.FirstDisplayedScrollingRowIndex = FirstDisplRow;
            if (SelectedRow > -1 && SelectedRow < gridTempAlloc.RowCount)
                gridTempAlloc.Rows[SelectedRow].Selected = true;


            //Settings.SetDataGridCoumnWidth(ref gridTempAlloc);
            ColumnWidthSaver.FillWidthOfColumn(gridTempAlloc);
            Settings.SetDataGridCaption(ref gridTempAlloc);
            Settings.SetDataGridColumnAlign(ref gridTempAlloc);

        }
        private void Fillgrid()
        {
 
        }
        public string nvl(object o, string replace)
        {
            if (o.ToString() == "") return replace;
            else return o.ToString();
        }
        public  void Find(object sender, EventArgs e)
        {
            if (this == null || this.IsDisposed) return;
            switch (tabControl1.SelectedIndex)
            {
                case 0: if (container_Layot.Enabled) FillGridLayot((tsbtTableModeShtat.Checked ? GridLayot.FirstDisplayedScrollingRowIndex : ShtatFilter.Subdiv_id), (tsbtTableModeShtat.Checked ? 0 : ShtatFilter.Subdiv_id)); break;
                case 1: if (containerProjects.Enabled) { FillGridExclude(0); FillGridInclude(0); } break;
                case 2: if (container_RegistrPersonal.Enabled) FillGridRegistration(0, 0);  break;
                case 3: if (container_AllocShtat.Enabled) FillGridTempAlloc(0, 0); break;
            }
            
           
        }
        
        private void VIEW_Load(object sender, EventArgs e)
        {
            this.treeStaff.Nodes.Add("0", "Улан-Удэнский авиационный завод");
            treeStaff.Nodes[0].Nodes.Add("-1", "No");
            GridLayot.Visible = true;
            treeStaff.Visible = false;
            containerProjects.EnableByRules();
            //group_command_shtat.EnableByRules(Connect.CurConnect, false);

            command_regist_shtat.EnableByRules(false);
            btEditRegistration.Enabled = btEditShtat.Enabled;
            btRepWorkerMenuShtat.Enabled = btReplaceEmpShtat.Enabled;
            btClearMenuShtat.Enabled = btClearStaffsId.Enabled;
            tpStaffsAllocation.EnableByRules(false);
        }

        private void treeStaff_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            treeStaff.SelectedNode = e.Node;
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("Нет сотрудников");
        }

        private void treeStaff_AfterSelect(object sender, TreeViewEventArgs e)
        {

            treeStaff.SelectedNode = e.Node;
            DataTable table = new DataTable();
            if (e.Node.Text.IndexOf('(') > -1)
            {
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("new/select_staffs_for_tree_curr_all.sql"), DataSourceScheme.SchemeName,(ShtatFilter.TypeStaff == null ? "" : " and type_staff=" + ShtatFilter.TypeStaff.ToString())), Connect.CurConnect);
                cmd.Parameters.Add("p_subdiv_id",treeStaff.SelectedNode.Parent.Name);
                cmd.Parameters.Add("p_pos_id",e.Node.Tag);
                cmd.Parameters.Add("p_cur_date",cur_date.Value);
                cmd.BindByName = true;
                new OracleDataAdapter(cmd).Fill(table);
            }
            grid.DataSource = table;

        }

        private void treeStaff_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeStaff.SelectedNode = e.Node;
            treeStaff.SelectedNode.Nodes.Clear();
            DataTable table = new DataTable();
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("new/select_posNameCurrent_all.sql"),DataSourceScheme.SchemeName,(ShtatFilter.DegreeId==null?"":string.Format(" and s.degree_id={0} ",ShtatFilter.DegreeId))+
                                                (ShtatFilter.TypeStaff== null? "" : string.Format(" and type_staff={0}",ShtatFilter.TypeStaff))),Connect.CurConnect);
            cmd.Parameters.Add("p_subdiv_id",OracleDbType.Decimal).Value=treeStaff.SelectedNode.Name;
            cmd.Parameters.Add("p_cur_date",cur_date.Value);
            cmd.BindByName = true;
            new OracleDataAdapter(cmd).Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                TreeNode tn = e.Node.Nodes.Add("-1",string.Format("{0} ({1})",table.Rows[i]["pos_name"],table.Rows[i]["cnt"]));
                tn.Tag = table.Rows[i]["pos_id"];
            }

            OracleCommand cmd1 = new OracleCommand(string.Format("select subdiv_name,subdiv_id from {0}.subdiv where parent_id=:p_subdiv_id and sub_actual_sign=1 and code_subdiv<'700' order by subdiv_name", DataSourceScheme.SchemeName),Connect.CurConnect);
            cmd1.BindByName = true;
            cmd1.Parameters.Add("p_subdiv_id",treeStaff.SelectedNode.Name);
            table.Reset();
            new OracleDataAdapter(cmd1).Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                e.Node.Nodes.Add(table.Rows[i]["subdiv_id"].ToString(), table.Rows[i]["subdiv_name"].ToString());
                e.Node.Nodes[e.Node.Nodes.Count - 1].Nodes.Add("Нет сотрудников");
            }

            if (e.Node.Nodes.Count == 0)
                e.Node.Nodes.Add("-2","Нет сотрудников");
        }
        
        private void VIEW_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain.UnCheckButtonShtat("btViewLayot");
        }
        private void grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btEdit_Click(null, null);
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow!=null)
            {
                Add_Edit_staff frmEdit = new Add_Edit_staff( TypeAdditionShtat.EditExists, grid.Rows[grid.SelectedCells[0].RowIndex].Cells["staffs_id"].Value.ToString());
                if (grid.Columns[grid.SelectedCells[0].ColumnIndex].HeaderText == "ФИО")
                    frmEdit.tabControl1.SelectedIndex = 1;
                else if (grid.Columns[grid.SelectedCells[0].ColumnIndex].HeaderText == "Замещает")
                    frmEdit.tabControl1.SelectedIndex = 2;
                frmEdit.ShowDialog(this);
                
                if (frmEdit.Result)
                if (tsbtTableModeShtat.Checked)
                    FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, GridLayot.CurrentRow.Index);
                else
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name != "-1" ? treeStaff.SelectedNode.Name : treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);
            }
        }
        private void btEditGroup_Click(object sender, EventArgs e)
        {
            if (grid.Rows.Count>0)
            {
                List<string> l = new List<string>();
                HashSet<string> h = new HashSet<string>();
                for (int i = 0; i < grid.RowCount; i++)
                    if (!h.Contains(grid["staffs_id", i].Value.ToString()))
                    {
                        h.Add(grid["staffs_id", i].Value.ToString());
                        l.Add(grid["staffs_id", i].Value.ToString());
                    }
                Add_Edit_staff frm_edit = new Add_Edit_staff( TypeAdditionShtat.EditGroup,null,l);
                frm_edit.ShowDialog(this);
                if (tsbtTableModeShtat.Checked)
                    FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, GridLayot.SelectedRows[0].Index);
                else
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name != "-1" ? treeStaff.SelectedNode.Name : treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);

            }
        }
        private void btTransferStaff_Click(object sender, EventArgs e)
        {
            if (grid.SelectedCells.Count > 0)
            {
                Add_Edit_staff frmEdit = new Add_Edit_staff(TypeAdditionShtat.TransferToNew, grid.Rows[grid.SelectedCells[0].RowIndex].Cells["staffs_id"].Value.ToString());
                frmEdit.ShowDialog(this);
                if (tsbtTableModeShtat.Checked)
                    FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, GridLayot.SelectedCells[0].RowIndex);
                else
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name != "-1" ? treeStaff.SelectedNode.Name : treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);
            }
        }
        private void btExclude_Click(object sender, EventArgs e)
        {
            /*if (grid.SelectedCells.Count > 0&& MessageBox.Show("Удалить единицу из базы?","АРМ Штатное расписание",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
               /* DropStaff_id frmEdit = new DropStaff_id(connect,grid.SelectedRows[0].Cells["staffs_id"].Value.ToString());
                frmEdit.ShowDialog(this);
                if (Vid.Text == Vid.Items[0].ToString())
                    FillGridLayot(GridLayot.Rows.IndexOf(GridLayot.SelectedRows[0]));
                else//вот здесь вот надо поставить обратно, когда будет рабочая программа, в эксплуатации
             */
            if (grid.CurrentRow!=null && MessageBox.Show("Удалить штатную единицу?","АРМ Штатное расписание",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.confirm_staffs where staffs_id={1}", DataSourceScheme.SchemeName, grid.CurrentRow.Cells["staffs_id"].Value), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("delete from {0}.AUDIT_TABLE where PRIMARY_KEY={1} and TABLE_NAME='STAFFS'", DataSourceScheme.SchemeName, grid.CurrentRow.Cells["staffs_id"].Value);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("update {0}.transfer SET staffs_id=null where staffs_id={1}", DataSourceScheme.SchemeName, grid.CurrentRow.Cells["staffs_id"].Value);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("delete from {0}.STAFFS where staffs_id={1}", DataSourceScheme.SchemeName, grid.CurrentRow.Cells["staffs_id"].Value);
                cmd.ExecuteNonQuery();
                Connect.Commit();
                /*OracleCommand cmd = new OracleCommand(string.Format("update {0}.staffs set staff_sign=2,date_end_staff=trunc(SYSDATE) where staffs_id='{1}'", DataSourceScheme.SchemeName, grid.Rows[grid.SelectedCells[0].RowIndex].Cells["staffs_id"].Value.ToString()), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();*/
                if (tsbtTableModeShtat.Checked)
                    FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, GridLayot.CurrentRow.Index);
                else
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name!="-1"?treeStaff.SelectedNode.Name:treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);
            }
        }
        private void Add_by_select_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow!=null)
            {
                Add_Edit_staff frmEdit = new Add_Edit_staff(TypeAdditionShtat.NewByExists, grid.CurrentRow.Cells["staffs_id"].Value.ToString());
                frmEdit.ShowDialog(this);
                if (tsbtTableModeShtat.Checked)
                    FillGridLayot(GridLayot.FirstDisplayedScrollingRowIndex, GridLayot.CurrentRow.Index);
                else
                    FillGridLayot(Convert.ToInt32((treeStaff.SelectedNode.Name != "-1" ? treeStaff.SelectedNode.Name : treeStaff.SelectedNode.Parent.Name)), treeStaff.SelectedNode.Index);
            }
        }


        private void checkAllInclude_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllInclude.Checked)
                for (int i = 0; i < GridInclude.RowCount; i++)
                {
                    GridInclude["checkin", i].Value = true;
                }
            else
                for (int i = 0; i < GridInclude.RowCount; i++)
                {
                    GridInclude["checkin", i].Value = false;
                }

        }
        private void checkAllExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllExclude.Checked)
                for (int i = 0; i < GridExclude.RowCount; i++)
                {
                    GridExclude["checkex", i].Value = true;
                }
            else
                for (int i = 0; i < GridExclude.RowCount; i++)
                {
                    GridExclude["checkex", i].Value = false;
                }

        }
        private void btEditProject_Click(object sender, EventArgs e)
        {
            if (GridDataGroup.SelectedRows.Count > 0)
            {
                Add_Edit_staff frmEdit = new Add_Edit_staff(TypeAdditionShtat.EditExists, GridDataGroup.SelectedRows[0].Cells["staffs_id"].Value.ToString());
                frmEdit.ShowDialog(this);
            }
        }
        private void GridExclude_SelectionChanged(object sender, EventArgs e)
        {
            if (GridExclude.SelectedRows.Count > 0 && GridExclude.SelectedRows[0].Cells["code_pos"].Value!=null)
            {
                DataGridViewRow r = GridExclude.SelectedRows[0];
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/get_data_by_GROUP_INFO.sql"), DataSourceScheme.SchemeName,
                    r.Cells["code_pos"].Value.ToString(),
                    FormMain.ShtatFilter.subdiv_id,
                    nvl(r.Cells["classific"].Value, "-1"),
                    nvl(r.Cells["kf_schema"].Value, "-1"),
                    nvl(r.Cells["kf_stavka"].Value, "-1"),
                    nvl(r.Cells["comb_addition"].Value, "-1"),
                    nvl(r.Cells["add_exp_area"].Value, "-1"),
                    nvl(r.Cells["harmful_add"].Value, "-1"),
                    nvl(r.Cells["tar_grid"].Value, "-1"),
                    (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format("and s.degree_id=(select from {0}.degree where degree_name='{1}')", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                    + (FormMain.ShtatFilter.type_staff == 0? "" : " and type_staff=" + (FormMain.ShtatFilter.type_staff - 1).ToString())
                        + string.Format(" and nvl(ptable.date_end,'01.01.1000')='{0}'", nvl(r.Cells["date_end"].Value, "01.01.1000")),
                    
                    string.Format("select s4.staffs_id, case when exists( select sign_confirm   from {0}.confirm_staffs c      where s4.staffs_id=c.staffs_id and c.sign_confirm=-1)  then 0 else 1 end  fl  "+
                                  "from {0}.staffs s4 where "+
                    "s4.staff_sign!=2 and exists(select * from {0}.CONFIRM_STAFFS cs where cs.staffs_id=s4.staffs_id and sign_modifi!=0 and nvl(sign_confirm,-1)<0) ", DataSourceScheme.SchemeName),
                    (r.Cells["checkEx"].ReadOnly ? "1" : "0")

                    ),
                    Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                GridDataGroup.DataSource = table;
                groupAbout.Text = "Подробнее(ИСКЛЮЧИТЬ):";
            }
            else
            {
                DataTable table = new DataTable();
                GridDataGroup.DataSource = table;
            }
        }
        private void GridInclude_SelectionChanged(object sender, EventArgs e)
        {
            if (GridInclude.SelectedRows.Count > 0 && GridInclude.SelectedRows[0].Cells["code_pos"].Value!=null)
            {
                DataGridViewRow r = GridInclude.SelectedRows[0];
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/get_data_by_GROUP_INFO.sql"), DataSourceScheme.SchemeName,
                r.Cells["code_pos"].Value.ToString(),
                FormMain.ShtatFilter.subdiv_id,
                nvl(r.Cells["classific"].Value, "-1"),
                nvl(r.Cells["kf_schema"].Value, "-1"),
                nvl(r.Cells["kf_stavka"].Value, "-1"),
                nvl(r.Cells["comb_addition"].Value, "-1"),
                nvl(r.Cells["add_exp_area"].Value, "-1"),
                nvl(r.Cells["harmful_add"].Value, "-1"),
                nvl(r.Cells["tar_grid"].Value, "-1"),
                (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format("and s.degree_id=(select from {0}.degree where degree_name='{1}')", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                + (FormMain.ShtatFilter.type_staff == 0 ? "" : " and type_staff=" + (FormMain.ShtatFilter.type_staff - 1).ToString())
                + string.Format(" and nvl(s.date_begin_staff,'01.01.1000')='{0}'", nvl(r.Cells["date_begin_staff"].Value, "01.01.1000")),
                string.Format("select s4.staffs_id,nvl((select case   when sign_confirm=-1 then 0  else 1 end fl from {0}.confirm_staffs c where s4.staffs_id=c.staffs_id ),0) fl " +
                              "from {0}.staffs s4 where  s4.staff_sign=0 and not exists(select * from {0}.CONFIRM_STAFFS cs where cs.staffs_id=s4.staffs_id and sign_modifi!=0 and nvl(sign_confirm,-1)<0) "
		                      , DataSourceScheme.SchemeName),
                (r.Cells["checkIn"].ReadOnly ? "1" : "0")
                ),

                Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                GridDataGroup.DataSource = table;
                groupAbout.Text = "Подробнее(ВВЕСТИ):"; 
            }
            else
            {
                DataTable table = new DataTable();
                GridDataGroup.DataSource = table;
            }
        }

        private void GridExclude_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridExclude_SelectionChanged(null, null);
        }
        private void GridInclude_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridInclude_SelectionChanged(null, null);
        }

        private void GridDataGroup_DataSourceChanged(object sender, EventArgs e)
        {
            if (GridDataGroup.Columns.Contains("staffs_id"))
                GridDataGroup.Columns["staffs_id"].Visible = false;
            if (GridDataGroup.Columns.Contains("need_transfer"))
                GridDataGroup.Columns["need_transfer"].Visible = false;
            if (GridDataGroup.Columns.Contains("transfer_id"))
                GridDataGroup.Columns["transfer_id"].Visible = false;
        }
        private void btDeleteProjIn_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            if (GridInclude.SelectedRows.Count > 0 &&
                MessageBox.Show(this,
                string.Format("Вы действительно хотите удалить группу записей:({0} количество:{1} ставка:{2})",
                GridInclude.SelectedRows[0].Cells["pos_name"].Value.ToString(),
                GridInclude.SelectedRows[0].Cells["col_ed"].Value.ToString(),
                GridInclude.SelectedRows[0].Cells["kf_stavka"].Value.ToString()
                ), "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DataGridViewRow r = GridInclude.SelectedRows[0];
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/get_data_by_GROUP_INFO.sql"), DataSourceScheme.SchemeName,
                r.Cells["code_pos"].Value.ToString(),
                FormMain.ShtatFilter.subdiv_id,
                nvl(r.Cells["classific"].Value, "-1"),
                nvl(r.Cells["kf_schema"].Value, "-1"),
                nvl(r.Cells["kf_stavka"].Value, "-1"),
                nvl(r.Cells["comb_addition"].Value, "-1"),
                nvl(r.Cells["add_exp_area"].Value, "-1"),
                nvl(r.Cells["harmful_add"].Value, "-1"),
                nvl(r.Cells["tar_grid"].Value, "-1"),
                (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format("and s.degree_id=(select from {0}.degree where degree_name='{1}')", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                + (FormMain.ShtatFilter.type_staff == 0 ? "" : " and type_staff=" + (FormMain.ShtatFilter.type_staff - 1).ToString())
                + string.Format(" and nvl(s.date_begin_staff,'01.01.1000')='{0}'", nvl(r.Cells["date_begin_staff"].Value, "01.01.1000")),
                string.Format("select s4.staffs_id,nvl((select case   when sign_confirm=-1 then 0  else 1 end fl from {0}.confirm_staffs c where s4.staffs_id=c.staffs_id ),0) fl " +
                              "from {0}.staffs s4 where  s4.staff_sign=0 and not exists(select * from {0}.CONFIRM_STAFFS cs where cs.staffs_id=s4.staffs_id and sign_modifi!=0 and nvl(sign_confirm,-1)<0) "
                              , DataSourceScheme.SchemeName),
                (r.Cells["checkIn"].ReadOnly ? "1" : "0")
                ),

                Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                string Delet = "";
                for (int j = 0; j < table.Rows.Count; j++)
                    Delet += table.Rows[j]["staffs_id"].ToString() + ",";
                if (Delet != "")
                {
                    OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.confirm_staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, Delet.Substring(0, Delet.Length - 1)), Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("delete from {0}.audit_table where table_name='STAFFS' and PRIMARY_KEY in ({1})", DataSourceScheme.SchemeName, Delet.Substring(0, Delet.Length - 1));
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("delete from {0}.staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, Delet.Substring(0, Delet.Length - 1));
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    FillGridInclude(0);
                }
            }
            
        }
        private void btDeleteEx_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            if (GridExclude.SelectedRows.Count > 0 &&
                MessageBox.Show(this,
                string.Format("Вы действительно хотите удалить группу записей:({0} количество:{1} ставка:{2})",
                GridExclude.SelectedRows[0].Cells["pos_name"].Value.ToString(),
                GridExclude.SelectedRows[0].Cells["col_ed"].Value.ToString(),
                GridExclude.SelectedRows[0].Cells["kf_stavka"].Value.ToString()
                ), "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DataGridViewRow r = GridExclude.SelectedRows[0];
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/get_data_by_GROUP_INFO.sql"), DataSourceScheme.SchemeName,
                    r.Cells["code_pos"].Value.ToString(),
                    FormMain.ShtatFilter.subdiv_id,
                    nvl(r.Cells["classific"].Value, "-1"),
                    nvl(r.Cells["kf_schema"].Value, "-1"),
                    nvl(r.Cells["kf_stavka"].Value, "-1"),
                    nvl(r.Cells["comb_addition"].Value, "-1"),
                    nvl(r.Cells["add_exp_area"].Value, "-1"),
                    nvl(r.Cells["harmful_add"].Value, "-1"),
                    nvl(r.Cells["tar_grid"].Value, "-1"),
                    (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format("and s.degree_id=(select from {0}.degree where degree_name='{1}')", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                    + (FormMain.ShtatFilter.type_staff == 0 ? "" : " and type_staff=" + (FormMain.ShtatFilter.type_staff - 1).ToString())
                    + string.Format(" and nvl(ptable.date_end,'01.01.1000')='{0}'", nvl(r.Cells["date_end"].Value, "01.01.1000")),
                    string.Format("select s4.staffs_id, case when exists( select sign_confirm   from {0}.confirm_staffs c      where s4.staffs_id=c.staffs_id and c.sign_confirm=-1)  then 0 else 1 end  fl  from {0}.staffs s4 where " +
                    "s4.staff_sign!=2 and exists(select * from {0}.CONFIRM_STAFFS cs where cs.staffs_id=s4.staffs_id and sign_modifi!=0 and nvl(sign_confirm,-1)<0) ", DataSourceScheme.SchemeName),
                    (r.Cells["checkEx"].ReadOnly ? "1" : "0")

                    ),
                    Connect.CurConnect);

                DataTable table = new DataTable();
                adapter.Fill(table);
                string Delet = "";
                for (int j = 0; j < table.Rows.Count; j++)
                    Delet += table.Rows[j]["staffs_id"].ToString() + ",";
                if (Delet != "")
                {
                    OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.confirm_staffs where staffs_id in ({1}) and sign_modifi=-1 and (sign_confirm is null or sign_confirm=-1 )", DataSourceScheme.SchemeName, Delet.Substring(0, Delet.Length - 1)), Connect.CurConnect);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    FillGridExclude(0);
                }
            }            
        }       
        private void btDel_Click(object sender, EventArgs e)
        {
            if (GridDataGroup.SelectedRows.Count>0 && GridDataGroup.SelectedRows[0]!=null)
            if (groupAbout.Text.Substring(groupAbout.Text.Length - 9) == "(ВВЕСТИ):")
            {
                OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.confirm_staffs where staffs_id={1}", DataSourceScheme.SchemeName, GridDataGroup.SelectedRows[0].Cells["staffs_id"].Value.ToString()), Connect.CurConnect);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("delete from {0}.audit_table where table_name='STAFFS' and PRIMARY_KEY={1}", DataSourceScheme.SchemeName, GridDataGroup.SelectedRows[0].Cells["staffs_id"].Value.ToString());
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("delete from {0}.staffs where staffs_id ={1}", DataSourceScheme.SchemeName, GridDataGroup.SelectedRows[0].Cells["staffs_id"].Value.ToString());
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    FillGridInclude(0);
            }
            else
            {
                OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.confirm_staffs where staffs_id ={1} and sign_modifi=-1 and (sign_confirm is null or sign_confirm=-1 )", DataSourceScheme.SchemeName, GridDataGroup.SelectedRows[0].Cells["staffs_id"].Value.ToString()), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                FillGridExclude(0);
            }
        }
        private void GridInclude_Click(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }
        private void btReplace_Click(object sender, EventArgs e)
        {
            if (GridRegistration.SelectedCells.Count > 0 && GridRegistration.SelectedCells[0] != null)
            {
                FindEmpSingle fmFind = new FindEmpSingle();
                fmFind.ShowDialog(this);
                if (fmFind.transfer_id!=null)
                {
                    OracleCommand cmd = new OracleCommand(string.Format("UPDATE {0}.transfer set STAFFS_ID={1} where transfer_id={2}", DataSourceScheme.SchemeName, GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["staffs_id"].Value.ToString(), fmFind.transfer_id), Connect.CurConnect);
                    cmd.ExecuteNonQuery();
                    if (GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["transfer_id"].Value != DBNull.Value)
                    {
                        cmd.CommandText = string.Format("update {0}.transfer set STAFFS_ID=null where transfer_id={1}", DataSourceScheme.SchemeName, GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["transfer_id"].Value.ToString());
                        cmd.ExecuteNonQuery();

                    }
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    FillGridRegistration(GridRegistration.SelectedCells[0].RowIndex, GridRegistration.FirstDisplayedScrollingRowIndex);
                }
            }
        }
        private void btEditRegistration_Click(object sender, EventArgs e)
        {
            if (GridRegistration.SelectedCells.Count > 0)
            {
                Add_Edit_staff frmEdit = new Add_Edit_staff( TypeAdditionShtat.EditExists, GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["staffs_id"].Value.ToString());
                if (GridRegistration.Columns[GridRegistration.SelectedCells[0].ColumnIndex].HeaderText == "ФИО")
                    frmEdit.tabControl1.SelectedIndex = 1;
                else if (GridRegistration.Columns[GridRegistration.SelectedCells[0].ColumnIndex].HeaderText == "Замещает")
                    frmEdit.tabControl1.SelectedIndex = 2;
                else
                    frmEdit.tabControl1.SelectedIndex = 0;

                frmEdit.ShowDialog(this);
                if (frmEdit.Result) FillGridRegistration(GridRegistration.SelectedCells[0].RowIndex, GridRegistration.FirstDisplayedScrollingRowIndex);
            }
        }
        
        private void btClearStaffsId_Click(object sender, EventArgs e)
        {
            if (GridRegistration.SelectedCells.Count > 0 && GridRegistration.SelectedCells[0] != null && GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["transfer_id"].Value != DBNull.Value 
                && MessageBox.Show("Убрать работника с данной штатной единицы?","АРМ Штатное расписание",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("update {0}.transfer set STAFFS_ID=null where transfer_id={1}", DataSourceScheme.SchemeName, GridRegistration.Rows[GridRegistration.SelectedCells[0].RowIndex].Cells["transfer_id"].Value.ToString()), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                FillGridRegistration(GridRegistration.SelectedCells[0].RowIndex, GridRegistration.FirstDisplayedScrollingRowIndex);
            }
        }
/*************************НАВЕДЕНИЕ МЫШИ на КНОПКИ основные***********************************************/
        private void btEditGroup_MouseEnter(object sender, EventArgs e)
        {
            if (tsbtTreeModeShtat.Checked)
            {
                if (treeStaff.SelectedNode != null && treeStaff.SelectedNode.Name == "-1" && treeStaff.SelectedNode.Text != "Нет сотрудников")
                {
                    treeStaff.HideSelection = true;
                    treeStaff.SelectedNode.BackColor = Color.LimeGreen;
                    btEditGroupShtat.ForeColor = Color.LimeGreen;
                }
            }
            else
                if (GridLayot.SelectedRows.Count > 0)
                {
                    GridLayot.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                    btEditGroupShtat.ForeColor = Color.LimeGreen;
                }
        }
        private void btEditGroup_MouseLeave(object sender, EventArgs e)
        {
            if (tsbtTreeModeShtat.Checked)
            {
                if (treeStaff.SelectedNode != null)
                {
                    treeStaff.Focus();
                    treeStaff.HideSelection = false;
                    treeStaff.SelectedNode.BackColor = Color.White;
                    btEditGroupShtat.ForeColor = Color.Blue;
                }
            }
            else
                if (GridLayot.SelectedRows.Count > 0)
                {
                    btEditGroupShtat.ForeColor = Color.Blue;
                    GridLayot.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                }
        }
        private void btEdit_MouseEnter(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                grid.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                btEditShtat.ForeColor = Color.LimeGreen;
            }
        }
        private void btEdit_MouseLeave(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                grid.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                btEditShtat.ForeColor = Color.Blue;
            }
        }
        private void btTransferStaff_MouseEnter(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                grid.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                btTransferStaff.ForeColor = Color.LimeGreen;
            }
        }
        private void btTransferStaff_MouseLeave(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                grid.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                btTransferStaff.ForeColor = Color.Blue;
            }
        }
        private void Add_by_select_MouseEnter(object sender, EventArgs e)
        {
            if (tsbtTreeModeShtat.Checked)
            {
                if (treeStaff.SelectedNode != null && treeStaff.SelectedNode.Name == "-1" && treeStaff.SelectedNode.Text != "Нет сотрудников")
                {
                    treeStaff.HideSelection = true;
                    //Add_by_select_Shtat.Focus();
                    treeStaff.SelectedNode.BackColor = Color.LimeGreen;
                    Add_by_select_Shtat.ForeColor = Color.LimeGreen;
                }
            }
            else
                if (GridLayot.SelectedRows.Count > 0)
                {
                    GridLayot.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                    Add_by_select_Shtat.ForeColor = Color.LimeGreen;
                }
        }
        private void Add_by_select_MouseLeave(object sender, EventArgs e)
        {
            if (tsbtTreeModeShtat.Checked)
            {
                if (treeStaff.SelectedNode != null)
                {
                    treeStaff.Focus();
                    treeStaff.HideSelection = false;
                    treeStaff.SelectedNode.BackColor = Color.White;
                    Add_by_select_Shtat.ForeColor = Color.Blue;
                }
            }
            else
                if (GridLayot.SelectedRows.Count > 0)
                {
                    Add_by_select_Shtat.ForeColor = Color.Blue;
                    GridLayot.SelectedRows[0].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                }
        }
        private void btExclude_MouseEnter(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
                for (int i=0;i<grid.RowCount;i++)
                    if (grid["staffs_id", i].Value.ToString() == grid.SelectedRows[0].Cells["staffs_id"].Value.ToString())
                    {
                        grid.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Crimson;
                        grid.Rows[i].DefaultCellStyle.BackColor = Color.Crimson;
                        btExcludeShtat.ForeColor = Color.Crimson;
                    }
        }
        private void btExclude_MouseLeave(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
                for (int i = 0; i < grid.RowCount; i++)
                {
                    grid.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                    grid.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (grid["in_otpusk", i].Value != DBNull.Value)
                    {
                        grid.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(128, 128, 200);
                        grid.Rows[i].DefaultCellStyle.BackColor = Color.LemonChiffon;
                        grid.Rows[i].DefaultCellStyle.SelectionForeColor = Color.FromArgb(200, 200, 200);
                        grid.Rows[i].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                        if (i > 0 && grid["staffs_id", i - 1].Value.ToString() == grid["staffs_id", i].Value.ToString())
                        {
                            grid.Rows[i - 1].DefaultCellStyle.BackColor = Color.LemonChiffon;
                            grid.Rows[i - 1].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                        }
                        if (i < grid.Rows.Count - 1 && grid["staffs_id", i + 1].Value.ToString() == grid["staffs_id", i].Value.ToString())
                        {
                            grid.Rows[i + 1].DefaultCellStyle.BackColor = Color.LemonChiffon;
                            grid.Rows[i + 1].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                            ++i;
                        }

                    }
                    if (grid["need_transfer", i].Value.ToString() == "1")
                    {
                        grid.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        grid.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }
            btExcludeShtat.ForeColor = Color.Blue;
        }
/********************************************************************************************************/
        private void GridRegistration_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btEditRegistration_Click(null, null);
        }
        private void GridDataGroup_Click(object sender, EventArgs e)
        {
            btEditProject_Click(null, null);
        }
        private void btReplace_MouseEnter(object sender, EventArgs e)
        {
            /*if (GridRegistration.SelectedRows.Count > 0)
            {
                GridRegistration.SelectedRows[0].Cells["ФИО"].d
            }*/
        }
        private void grid_DataSourceChanged(object sender, EventArgs e)
        {
            if (grid.Columns.Contains("need_transfer")) 
                grid.Columns["need_transfer"].Visible = false;
            if (grid.Columns.Contains("staffs_id")) 
                grid.Columns["staffs_id"].Visible = false;
            if (grid.Columns.Contains("transfer_id")) 
                grid.Columns["transfer_id"].Visible = false;
            Settings.SetDataGridCoumnWidth(ref grid);
            Settings.SetDataGridCaption(ref grid);
            Settings.SetDataGridColumnAlign(ref grid);
            for (int i = 0; i < grid.RowCount; i++)
            {
                if (grid["need_transfer",i].Value.ToString() == "1")
                {
                    grid.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    grid.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                }   
            }
        }
        
        private void GridRegistration_Sorted_1(object sender, EventArgs e)
        {
            for (int i = 0; i < GridRegistration.Rows.Count; i++)
            {
                if (GridRegistration["in_otpusk",i].Value.ToString() != "")
                {
                    GridRegistration.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(128, 128, 200);
                    GridRegistration.Rows[i].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionForeColor = Color.FromArgb(200, 200, 200);
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                }
                if (GridRegistration["need_transfer",i].Value.ToString() == "1")
                {
                    GridRegistration.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    GridRegistration.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                }
            }
        }
        private void GridLayot_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btEditGroup_Click(null, null);
        }

        private void gridTempAlloc_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (tpStaffsAllocation.Enabled && gridTempAlloc.SelectedCells.Count > 0)
            {
                if (gridTempAlloc.Columns[gridTempAlloc.SelectedCells[0].ColumnIndex].HeaderText == "Дополн. направлена")
                {
                    SelectSubdivFromTree sft;
                    if (gridTempAlloc.SelectedCells[0].Value.ToString() != "")
                    {
                        sft = new SelectSubdivFromTree(Convert.ToInt32(gridTempAlloc["t_subdiv_id", gridTempAlloc.SelectedCells[0].RowIndex].Value.ToString()), "Выберите подразделение для дополнительного направления");
                    }
                    else
                    {
                        sft = new SelectSubdivFromTree(Convert.ToInt32(FormMain.ShtatFilter.subdiv_id), "Выберите подразделение для дополнительного направления");
                    }
                    sft.ShowDialog(this);
                    if (sft.subdiv_id != "-1" && MessageBox.Show(string.Format("Вы действительно хотите направить дополнительно данную единицу в '{0}'", sft.subdiv_name), "АРМ Штатное расписание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        OracleCommand cmd = new OracleCommand(string.Format("UPDATE {0}.staffs_temp_alloc set actual_alloc=0 where staffs_temp_id='{1}'", DataSourceScheme.SchemeName, gridTempAlloc["staffs_temp_id", gridTempAlloc.SelectedCells[0].RowIndex].Value.ToString()), Connect.CurConnect);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format("INSERT INTO {0}.staffs_temp_alloc(staffs_temp_id,staffs_id,t_subdiv_id,actual_alloc) VALUES({0}.staffs_t_alloc_id_seq.nextval,{1},{2},1)", DataSourceScheme.SchemeName,
                            gridTempAlloc["staffs_id", gridTempAlloc.SelectedCells[0].RowIndex].Value.ToString(), sft.subdiv_id);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "commit";
                        cmd.ExecuteNonQuery();
                        FillGridTempAlloc(gridTempAlloc.SelectedCells[0].RowIndex, gridTempAlloc.FirstDisplayedScrollingRowIndex);
                    }
                }
                else
                    if (gridTempAlloc.Columns[gridTempAlloc.SelectedCells[0].ColumnIndex].Name == "Замещение")
                    {
                        
                    }


            }
        }

        private void MenuStTempAlloc_Opening(object sender, CancelEventArgs e)
        {
            menuStTempAlloc_Edit.Enabled = menuStTempAlloc_Clear.Enabled = tpStaffsAllocation.Enabled;
            Point p = MousePosition;
                p = gridTempAlloc.PointToClient(p);
                if (gridTempAlloc.HitTest(p.X, p.Y).Type == DataGridViewHitTestType.Cell && gridTempAlloc.Columns[gridTempAlloc.HitTest(p.X, p.Y).ColumnIndex].HeaderText == "Дополн. направлена")
                    gridTempAlloc[gridTempAlloc.HitTest(p.X, p.Y).ColumnIndex, gridTempAlloc.HitTest(p.X, p.Y).RowIndex].Selected = true;
                else
                {
                    menuStTempAlloc_Edit.Enabled = menuStTempAlloc_Clear.Enabled = false;
                };
                
        }

        private void menuStTempAlloc_Edit_Click(object sender, EventArgs e)
        {
            gridTempAlloc_CellMouseDoubleClick(null, null);
        }

        private void menuStTempAlloc_Clear_Click(object sender, EventArgs e)
        {
            
            if (gridTempAlloc.SelectedCells.Count > 0 && MessageBox.Show("Вы действительно хотите отправить данное распределение в архив?", "АРМ Штатное расписание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new OracleCommand(string.Format("update {0}.staffs_temp_alloc set actual_alloc=0 where staffs_temp_id='{1}'", DataSourceScheme.SchemeName, gridTempAlloc["staffs_temp_id", gridTempAlloc.SelectedCells[0].RowIndex].Value.ToString()), Connect.CurConnect).ExecuteNonQuery();
                new OracleCommand("commit", Connect.CurConnect).ExecuteNonQuery();
                FillGridTempAlloc(gridTempAlloc.SelectedCells[0].RowIndex, gridTempAlloc.FirstDisplayedScrollingRowIndex);
            }
        }

        private void menuGridRegistrShtat_Opening(object sender, CancelEventArgs e)
        {
            Point p = MousePosition;
            p = GridRegistration.PointToClient(p);
            if (GridRegistration.HitTest(p.X, p.Y).Type == DataGridViewHitTestType.Cell)
                GridRegistration[GridRegistration.HitTest(p.X, p.Y).ColumnIndex, GridRegistration.HitTest(p.X, p.Y).RowIndex].Selected = true;
            else
                e.Cancel = true;
        }

        private void grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthSaver.SaveWidthOfColumn((DataGridView)sender);
        }

        private void GridLayot_CurrentCellChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            if (GridLayot.CurrentRow != null)
            {
                DataGridViewRow r = GridLayot.CurrentRow;
                OracleDataAdapter a = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/select_about_staff_gridVIEW.sql"),
                    DataSourceScheme.SchemeName, (ShtatFilter.DegreeId == null ? "" : string.Format(" and degree_id={0} ", ShtatFilter.DegreeId))
                                               + (ShtatFilter.TypeStaff == null ? "" : string.Format(" and type_staff={0} ", ShtatFilter.TypeStaff.ToString()))
                    ), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters["psubdiv_id"].Value = ShtatFilter.Subdiv_id;
                a.SelectCommand.Parameters["pcur_date"].Value = cur_date.Value;
                a.SelectCommand.Parameters["pcode_pos"].Value = r.Cells["Код профессии"].Value;
                a.SelectCommand.Parameters["pclassific"].Value = r.Cells["Разряд"].Value;
                a.SelectCommand.Parameters["ptar_by_schema"].Value = r.Cells["Тарифный коэфф. по схеме"].Value;
                a.SelectCommand.Parameters["pstavka"].Value = r.Cells["Тарифная ставка"].Value;
                a.SelectCommand.Parameters["pcomb_addition"].Value = r.Cells["Надбавка за совм"].Value;
                a.SelectCommand.Parameters["padd_exp_area"].Value = r.Cells["Надбавка за расш. обслуж."].Value;
                a.SelectCommand.Parameters["pharm_add"].Value = r.Cells["Надбавка за вредн."].Value;
                a.SelectCommand.Parameters["ptar_grid"].Value = r.Cells["Тарифная сетка"].Value;

                a.Fill(table);
            }
            grid.DataSource = table;
        }    

    }

    public class TreeData
    {
        public TreeData()
        {
        }
    }
}
