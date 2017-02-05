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
using Oracle.DataAccess.Types;
using Microsoft.Reporting.WinForms;
namespace Kadr.Vacation_schedule
{
    public partial class ViewCard : Form
    {
        string transfer_id, vac_id;
        DataSet ds = new DataSet();
        private OracleCommand cmd_load_vac;
        OracleDataAdapter ad_rem_vac;
        public ViewCard(string transfer, string vac) : this(transfer, vac, false) { }
        public ViewCard(string transfer, string vac, bool IsLocked)
        {
            transfer_id = transfer;
            vac_id = vac;
            InitializeComponent();
            cmd_load_vac = new OracleCommand(string.Format(@"begin {0}.VAC_SCHED_PACK.GET_EMP_VACS(:p_transfer_id,:vac_data);end;", Connect.Schema), Connect.CurConnect);
            cmd_load_vac.BindByName = true;
            cmd_load_vac.Parameters.Add("p_transfer_id", transfer_id);
            cmd_load_vac.Parameters.Add("vac_data", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            new OracleDataAdapter(string.Format("select * from {0}.VAC_GROUP_TYPE", Connect.Schema), Connect.CurConnect).Fill(ds, "vac_group_type");
            ds.Tables.Add("vs");
            ds.Tables.Add("rem_vacs");
            if (IsLocked)
            {
                panel6_commands.Enabled = toolStrip1.Enabled = false;
            }
            else
            {
                panel6_commands.EnableByRules(false);
                tStripAddVacPeriod.EnableByRules(false);
                toolStrip1.EnableByRules(false);
                check_all.Enabled = true;
            }
            tsbtOnlyActual.Enabled = true;
            ad_rem_vac = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go\GetRemVacs.sql"), Connect.Schema), Connect.CurConnect);
            ad_rem_vac.SelectCommand.BindByName = true;
            ad_rem_vac.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, decimal.Parse(transfer_id), ParameterDirection.Input);
            ad_rem_vac.SelectCommand.Parameters.Add("p_date_calc", OracleDbType.Date);
            ad_rem_vac.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            gridRemVacsVS.AutoGenerateColumns = false;
            gridRemVacsVS.Columns.Add(new MDataGridViewTextBoxColumn("vac_group_name", "Тип отпуска", "GROUP_VAC_NAME"));
            gridRemVacsVS.Columns.Add(new MDataGridViewTextBoxColumn("period_end", "Использован по", "PERIOD_END"));
            gridRemVacsVS.Columns.Add(new MDataGridViewTextBoxColumn("rem_days", "Заработано дней", "REM_DAYS"));
            dpDateRemVac.Value = DateTime.Now;
            gridRemVacsVS.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            OracleCommand cmd_aud = new OracleCommand(string.Format("begin APSTAFF.TABLE_AUDIT_EX_INSERT(:p_table_id, :p_type_kard); end;", Connect.Schema), Connect.CurConnect);
            cmd_aud.BindByName = true;
            cmd_aud.Parameters.Add("p_table_id", OracleDbType.Decimal, transfer_id, ParameterDirection.Input);
            cmd_aud.Parameters.Add("p_type_kard", OracleDbType.Varchar2, "vs", ParameterDirection.Input);
            cmd_aud.ExecuteNonQuery();
        }

        /// <summary>
        /// Текущее подразделение сотрудника
        /// </summary>
        public decimal SubdivID
        {
            get;
            set;
        }

        private void Load_calendar()
        {
            //считываем данные о всех отпусках сотрудника
            treeView.Nodes.Clear();
            (elementHost1.Child as WPFCalendar).ClearIntevals();
            DataView d = new DataView(vs, "", "VAC_BEGIN desc, plan_begin", DataViewRowState.CurrentRows);
            foreach (DataRowView r in d)
            {   //отмечаем в календаре все даты сразу
                (elementHost1.Child as WPFCalendar).SetInterval(r.Row.Field<DateTime>("PLAN_BEGIN"),r.Row.Field<DateTime>("PLAN_END"), System.Windows.Media.Brushes.LightGreen);
                TreeNode tn;
                if (treeView.Nodes.Find(DateTime.Parse(r["vac_begin"].ToString()).Year.ToString(), false).Length == 0)
                    tn = treeView.Nodes.Add(DateTime.Parse(r["vac_begin"].ToString()).Year.ToString(), string.Format("{0} год", DateTime.Parse(r["vac_begin"].ToString()).Year));
                else
                    tn = treeView.Nodes.Find(DateTime.Parse(r["vac_begin"].ToString()).Year.ToString(), false)[0];
                if (tn.Nodes.Find(r["vac_sched_id"].ToString(), false).Length == 0)
                    tn = tn.Nodes.Add(r["vac_sched_id"].ToString(), string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", r["VAC_BEGIN"],r["VAC_END"]));
                else
                    tn = tn.Nodes.Find(r["vac_sched_id"].ToString(), false)[0];
                tn.Tag = r.Row;
                (tn=tn.Nodes.Add(r["vac_consist_id"].ToString(), string.Format("{0:dd.MM.yyyy} ({1})", r["plan_begin"], r["name_vac"]))).Tag = r.Row;
                tn.NodeFont = new System.Drawing.Font("Tahoma",9);
            }
            this.treeView_AfterSelect(null, null);
        }

        private DataTable vs
        {
            get { return ds.Tables["vs"]; }
        }
        private DataTable tg
        {
            get { return ds.Tables["vac_group_type"]; }
        }

        private void FillVacs()
        {
            vs.Rows.Clear();

            new OracleDataAdapter(cmd_load_vac).Fill(vs);
            //tabsTypeVS.TabPages.Clear();
            List<object> l = new List<object>();
            for (int i=0;i<vs.Rows.Count;++i)
                if (!tabsTypeVS.TabPages.ContainsKey("tp"+vs.Rows[i]["VAC_GROUP_TYPE_ID"].ToString()) )
                {
                    l.Add(vs.Rows[i]["VAC_GROUP_TYPE_ID"]);
                    tabsTypeVS.TabPages.Add("tp" + vs.Rows[i]["VAC_GROUP_TYPE_ID"].ToString(), tg.Compute("MAX(GROUP_VAC_NAME)", string.Format("VAC_GROUP_TYPE_ID={0}", vs.Rows[i]["VAC_GROUP_TYPE_ID"])).ToString());
                    TabPage t = tabsTypeVS.TabPages["tp" + vs.Rows[i]["VAC_GROUP_TYPE_ID"].ToString()];
                    DataGridView d = new DataGridView();
                    d.AutoGenerateColumns = false;
                    d.AllowUserToAddRows = d.AllowUserToDeleteRows = false;
                    d.RowsDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9,FontStyle.Regular);
                    d.Name = "gridTypeVC" + vs.Rows[i]["VAC_GROUP_TYPE_ID"].ToString();
                    d.RowsAdded += new DataGridViewRowsAddedEventHandler(d_RowsAdded);
                    d.DataSource = new DataView(vs, string.Format("VAC_GROUP_TYPE_ID={0} {1}", vs.Rows[i]["VAC_GROUP_TYPE_ID"], tsbtOnlyActual.Checked?"and PLAN_SIGN=0":""), "", DataViewRowState.CurrentRows);
                    DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
                    c.Name="fl";
                    c.HeaderText = "";
                    c.DataPropertyName = "fl";
                    d.Columns.Add(c);
                    d.Columns.Add(new MDataGridViewTextBoxColumn("NAME_VAC", "Тип", "NAME_VAC")); d.Columns["NAME_VAC"].ReadOnly = true;
                    d.Columns.Add(new MDataGridViewTextBoxColumn("PERIOD_BEGIN", "Начало периода", "PERIOD_BEGIN")); d.Columns["PERIOD_BEGIN"].ReadOnly = true; d.Columns["PERIOD_BEGIN"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    d.Columns.Add(new MDataGridViewTextBoxColumn("PERIOD_END", "Окончание периода", "PERIOD_END")); d.Columns["PERIOD_END"].ReadOnly = true; d.Columns["PERIOD_END"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    d.Columns.Add(new MDataGridViewTextBoxColumn("plan_begin", "Дата отпуска", "plan_begin")); d.Columns["plan_begin"].ReadOnly = true; d.Columns["plan_begin"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    d.Columns["plan_begin"].DefaultCellStyle.ForeColor = Color.DarkCyan; d.Columns["plan_begin"].DefaultCellStyle.BackColor = Color.Cornsilk;
                    d.Columns["plan_begin"].DefaultCellStyle.Padding = new System.Windows.Forms.Padding(15, 0,0, 0);
                    d.Columns.Add(new MDataGridViewTextBoxColumn("plan_end", "Окончание отпуска", "plan_end")); d.Columns["plan_end"].ReadOnly = true; d.Columns["plan_end"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    d.Columns["plan_end"].DefaultCellStyle.ForeColor = Color.DarkCyan; d.Columns["plan_end"].DefaultCellStyle.BackColor = Color.Cornsilk; d.Columns["plan_end"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    d.Columns.Add(new MDataGridViewTextBoxColumn("COUNT_DAYS", "Кол-во дней", "COUNT_DAYS")); d.Columns["COUNT_DAYS"].ReadOnly = true; d.Columns["COUNT_DAYS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    d.Columns.Add(new MDataGridViewTextBoxColumn("comments", "Комментарий", "comments")); d.Columns["comments"].ReadOnly = true;
                    d.Columns.Add(new MDataGridViewTextBoxColumn("VAC_STATE", "Состояние", "VAC_STATE")); d.Columns["VAC_STATE"].ReadOnly = true;
                    d.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
                    d.RowPostPaint += new DataGridViewRowPostPaintEventHandler(d_RowPostPaint);
                    d.CellPainting += new DataGridViewCellPaintingEventHandler(d_CellPainting);
                    d.CellClick += new DataGridViewCellEventHandler(d_CellClick);
                    (d.DataSource as DataView).Sort = "VAC_BEGIN DESC,NUMBER_CALC";
                    ColumnWidthSaver.FillWidthOfColumn(d);
                    d.ColumnHeadersHeight = 40;
                    d.Dock = DockStyle.Fill;
                    d.BackgroundColor = Color.White;
                    t.Controls.Add(d);
                }
            check_all.Checked = false;
            Load_calendar();
        }

        void d_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                Point p = (sender as DataGridView).PointToClient(MousePosition);
                Rectangle r = (sender as DataGridView).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Rectangle r1 = (sender as DataGridView)[e.ColumnIndex, e.RowIndex].ErrorIconBounds;
                r1.X += r.X; r1.Y += r.Y;
                if ((sender as DataGridView)[e.ColumnIndex, e.RowIndex].ErrorText != "" && r1.Contains(p))
                {
                    ToolTip t = new ToolTip();
                    t.IsBalloon = false;
                    t.ToolTipTitle = "Ошибка ведения графиков отпусков";
                    t.ToolTipIcon = ToolTipIcon.Error;
                    t.UseAnimation = true;
                    t.Show((sender as DataGridView)[e.ColumnIndex, e.RowIndex].ErrorText, (sender as DataGridView), 3500);
                }
            }
        }

        private void FillAddVacs()
        {
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("GO/GetAddVacEmp.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("transfer_id", transfer_id);
            DataTable t = new DataTable();
            new OracleDataAdapter(cmd).Fill(t);
            list_add_vac.AutoGenerateColumns = false;
            list_add_vac.DataSource = t;
            list_add_vac.Columns["name_vac1"].DataPropertyName = "NAME_VAC";
            list_add_vac.Columns["name_vac1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        void d_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView d = sender as DataGridView;
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; ++i)
            {
                d["PERIOD_BEGIN", i].ErrorText = (d.Rows[i].DataBoundItem as DataRowView).Row["er_message1"].ToString();
                d["PERIOD_END", i].ErrorText = (d.Rows[i].DataBoundItem as DataRowView).Row["er_message2"].ToString();
                d["PLAN_BEGIN", i].ErrorText = (d.Rows[i].DataBoundItem as DataRowView).Row["er_message3"].ToString();
            }
        }

        void d_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView d = sender as DataGridView;
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (e.RowIndex % 2 == 0)
                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Outset;
                else
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Single;

                if (e.RowIndex>0 && d.Columns[e.ColumnIndex].DataPropertyName.ToUpper() == "FL")
                {
                    if (string.Format("{0}", (d.Rows[e.RowIndex].DataBoundItem as DataRowView)["vac_sched_id"]) == string.Format("{0}", (d.Rows[e.RowIndex - 1].DataBoundItem as DataRowView)["vac_sched_id"]))
                    {
                        e.PaintBackground(e.CellBounds, false);
                        d[e.ColumnIndex, e.RowIndex].ReadOnly = true;
                        e.Handled = true;
                    }
                    else d[e.ColumnIndex, e.RowIndex].ReadOnly = false;
                }
                
            }
        }

        void d_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((sender as DataGridView).Rows[e.RowIndex].DataBoundItem!=null && int.Parse(((sender as DataGridView).Rows[e.RowIndex].DataBoundItem as DataRowView)["CLOSE_SIGN"].ToString()) > 0)
                (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.BackColor = (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.LightGray;
            else
                (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.BackColor = (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
            
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            ImageList IL = new ImageList();
            IL.Images.Add(global::Kadr.Properties.Resources.NoneSelect);
            IL.Images.Add(global::Kadr.Properties.Resources.SelecteNode);
            treeView.ImageList = IL;
            treeView.SelectedImageIndex = 1;
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("GO/GETEMPDATA.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("transfer_id",transfer_id);
            OracleDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                fam.Text = r["emp_last_name"].ToString();
                name.Text = r["emp_first_name"].ToString();
                middle_name.Text = r["emp_middle_name"].ToString();
                per_num.Text = r["per_num"].ToString();
                pos_name.Text = r["pos_name"].ToString();
                date_hire.Text = r["date_hire"].ToString();
                string st = r["sign_comb"].ToString();
                sing_comb.Checked = r["sign_comb"].ToString() == "1";
                emp_photo.Image = Staff.EmployeePhoto.GetPhoto(per_num.Text);
                this.SubdivID = Convert.ToDecimal(r["SUBDIV_ID"].ToString());
            }
            r.Close();
            FillAddVacs();
            FillVacs();
            if (!string.IsNullOrEmpty(vac_id))
                treeView.SelectedNode = treeView.Nodes.Find(vac_id, true)[0];
            panel6_commands.Visible = true;
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e!=null && e.Node != null)
            {
                try
                {
                    DataRow r = (e.Node.Tag as DataRow);
                    (elementHost1.Child as WPFCalendar).SelectedYear = new DateTime(r.Field<DateTime>("PLAN_BEGIN").Year,1,1);
                    if (e.Node.Level == 2)
                    {
                        dates_vac.Text = string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", r["plan_begin"], r["plan_end"]);
                        lb_count_days.Text = r["COUNT_DAYS"].ToString();
                        name_vac.Text = r["NAME_VAC"].ToString();
                        (elementHost1.Child as WPFCalendar).SetBorderInterval(r.Field<DateTime>("PLAN_BEGIN"), r.Field<DateTime>("PLAN_END"));
                    }
                    else
                    {
                        dates_vac.Text = string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", r["vac_begin"], r["vac_end"]);
                        lb_count_days.Text = vs.Compute("SUM(COUNT_DAYS)","vac_sched_id="+ r["vac_sched_id"].ToString()).ToString();
                        name_vac.Text = "";
                        (elementHost1.Child as WPFCalendar).SetBorderInterval(r.Field<DateTime>("vac_begin"), r.Field<DateTime>("vac_end"));
                    }                    
                    
                    periods_vac.Text=(e.Node.Level>1?string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", r["period_begin"],r["period_end"]):"");
                    comm.Text = r["comments"].ToString();
                    unused.Text = r["unused_days"].ToString();
                }
                catch 
                {
                    dates_vac.Text = lb_count_days.Text = periods_vac.Text = comm.Text = unused.Text = name_vac.Text = "";
                }
            }
            else
                dates_vac.Text = lb_count_days.Text = periods_vac.Text = comm.Text = unused.Text = name_vac.Text = "";
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView.SelectedNode = e.Node;
        }

        private void contMenu_Opening(object sender, CancelEventArgs e)
        {
            TreeNode t;
            if (!panel6_commands.Enabled) { e.Cancel = true; return; }

            if ((t = treeView.GetNodeAt(treeView.PointToClient(MousePosition))) == null)
            {
                contMenu.Items["edit"].Enabled = false;
                contMenu.Items["delete_curr"].Enabled = false;
                contMenu.Items["add_new"].Enabled = btAddVS.Enabled;
                contMenu.Items["NoteAccount"].Enabled = false;
            }
            else
            {
                contMenu.Items["edit"].Enabled = btEditVS.Enabled;
                contMenu.Items["delete_curr"].Enabled = btDelVs.Enabled;
                contMenu.Items["add_new"].Enabled = true;
                contMenu.Items["NoteAccount"].Enabled = true;
            }
        }

        private void add_new_Click(object sender, EventArgs e)
        {
            EditVac frm_new = new EditVac(transfer_id);
            if (frm_new.ShowDialog(this) == DialogResult.OK)
                FillVacs();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (edit.Enabled && treeView.SelectedNode != null && (treeView.SelectedNode.Level==1 || treeView.SelectedNode.Level==2))
            {

                string k = (treeView.SelectedNode.Level==2?treeView.SelectedNode.Parent.Name:treeView.SelectedNode.Name);
                EditVac frm_new = new EditVac(k, transfer_id);
                if (frm_new.ShowDialog(this) == DialogResult.OK)
                    FillVacs();
                TreeNode[] t;
                if ((t = treeView.Nodes.Find(k, true)).Length > 0)
                {
                    treeView.SelectedNode = t[0];
                    t[0].EnsureVisible();
                }
                treeView.Focus();

            }
        }

        private void delete_curr_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && (treeView.SelectedNode.Level==1 || treeView.SelectedNode.Level==2))
            {
                if (treeView.SelectedNode.Level == 2)
                {
                    treeView.SelectedNode = treeView.SelectedNode.Parent;
                    treeView.SelectedNode.Collapse();
                }
                if (MessageBox.Show(this, "Вы действительно хотите удалить данный отпуск и всю информацию о нём?", "АРМ Графики отпусков", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OracleCommand cmd = new OracleCommand(string.Format("begin {0}.VACATION_SCHEDULE_delete(:p_vac_sched_id);end;", DataSourceScheme.SchemeName), Connect.CurConnect);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_vac_sched_id", (treeView.SelectedNode.Tag as DataRow)["vac_sched_id"]);
                    OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(Library.GetMessageException(ex));
                    }
                    FillVacs();
                }
            }
        }

        /// <summary>
        /// Формирование отчета записка-расчет
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="subdiv_current"></param>
        /// <param name="vac_sched_ids"></param>
        public static void NoteAccountReport(Form owner, decimal? subdiv_current, decimal[] vac_sched_ids)
        {
            try
            {
                DataSet ds = new DataSet();
                using (OracleCommand cmd = new OracleCommand(string.Format("begin {0}.VAC_SCHED_PACK2.GET_NOTE_ACCOUNT(:p_vac_consist_id,:c); END;", Connect.Schema), Connect.CurConnect))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_vac_consist_id", OracleDbType.Array, vac_sched_ids, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    cmd.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
                    new OracleDataAdapter(cmd).Fill(ds);
                }
                string[][] s_pos = new string[][] { };
                if (ds.Tables[0].Rows.Count == 0) ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                if (Signes.Show(owner, subdiv_current, "noteAccountVS", "Введите должность и ФИО работника кадровой службы(место подписи)", 1, ref s_pos) == DialogResult.OK)
                {
                    ReportViewerWindow.ShowReport("Записка-расчет отпуска", "Rep_VSNoteAccount.rdlc", ds.Tables[0],
                        new ReportParameter[]{
                            new ReportParameter("P_POS_NAME", s_pos[0][0]),
                            new ReportParameter("P_FIO", s_pos[0][1])});
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка формирования отчета");
            }
        }

        private void NoteAccount_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode.Level>0)
            {
                NoteAccountReport(this, this.SubdivID,
                    new decimal[]{(treeView.SelectedNode.Tag as DataRow).Field2<Decimal>("VAC_SCHED_ID")});
               /* try
                {
                    if ((treeView.SelectedNode.Tag as DataRow)["ACTUAL_BEGIN"] == DBNull.Value)
                        throw new Exception("Не указаны фактические данные отпуска(только плановые)");
                    DataSet ds = new DataSet();
                    using (OracleCommand cmd = new OracleCommand(string.Format("begin {0}.VAC_SCHED_PACK.GET_NOTE_ACCOUNT(:p_vac_consist_id,:p1,:p2); END;", Connect.Schema), Connect.CurConnect))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("p_vac_consist_id", (treeView.SelectedNode.Tag as DataRow)["vac_consist_id"]);
                        cmd.Parameters.Add("p1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        new OracleDataAdapter(cmd).Fill(ds);
                    }
                    string[][] s_pos = new string[][] { };
                    if (ds.Tables[0].Rows.Count == 0) ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    if (Signes.Show(this, this.SubdivID, "noteAccountVS", "Введите должность и ФИО работника кадровой службы(место подписи)", 1, ref s_pos) == DialogResult.OK)
                    {
                        Excel.PrintWithBorderMergedTable(false,
                        "VSNoteAccount.xlt", "A21", new DataTable[] { ds.Tables[1] }, new ExcelParameter[]
                        {
                            new ExcelParameter("Z10",ds.Tables[0].Rows[0]["Z10"].ToString()),
                            new ExcelParameter("A11",ds.Tables[0].Rows[0]["A11"].ToString()),
                            new ExcelParameter("A13",ds.Tables[0].Rows[0]["A13"].ToString()),
                            new ExcelParameter("A15",ds.Tables[0].Rows[0]["A15"].ToString()),
                            new ExcelParameter("C32",s_pos[0][0]),
                            new ExcelParameter("W32",s_pos[0][1]),
                            new ExcelParameter("O37",ds.Tables[0].Rows[0]["O37"].ToString())
                        }, null
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Library.GetMessageException(ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }*/
            }
            else
                {
                    ToolTip t = new ToolTip();
                    t.ToolTipIcon = ToolTipIcon.Error;
                    t.Show("Для отчета требуется выбрать отпуск", groupBox1, 0, btNoteAccountVS.Height, 2100);
                    if (treeView.SelectedNode != null)
                        treeView.SelectedNode.ExpandAll();
                }
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (panel6_commands.Enabled) edit_Click(null, EventArgs.Empty);
        }

        private void btReCalcPeriodsVS_Click(object sender, EventArgs e)
        {
            if (tabsTypeVS.SelectedTab != null)
            {
                DataGridView d;
                (tabsTypeVS.SelectedTab.Controls[0] as DataGridView).EndEdit();
                d = tabsTypeVS.SelectedTab.Controls[0] as DataGridView;
                if (d.Rows.Count > 0)
                {
                    ReCalcPeriods.ReCalcList(transfer_id, (d.Rows[0].DataBoundItem as DataRowView).Row["VAC_GROUP_TYPE_ID"]);
                    FillVacs();
                }
            }
        }

        private void check_all_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView d;
            for (int i = 0; i < (d = tabsTypeVS.SelectedTab.Controls[0] as DataGridView).RowCount; i++)
                d["fl", i].Value = check_all.Checked;
        }

        private void btLockVS_Click(object sender, EventArgs e)
        {
            (tabsTypeVS.SelectedTab.Controls[0] as DataGridView).EndEdit();
            List<string> l = new List<string>();
            DataGridView d;
            for (int i = 0; i < (d = tabsTypeVS.SelectedTab.Controls[0] as DataGridView).Rows.Count; ++i)
                if ((d.Rows[i].DataBoundItem as DataRowView)["CLOSE_SIGN"].ToString() == "0" && string.Format("{0}",d["fl", i].Value).ToUpper() == "TRUE")
                    l.Add((d.Rows[i].DataBoundItem as DataRowView)["vac_sched_id"].ToString());
            if (l.Count > 0 && MessageBox.Show("Заблокировать выбранные отпуска?", "Графики Отпусков", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.CLOSE_VAC(:p,:p1,:p2);end;",DataSourceScheme.SchemeName), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p", string.Join(",", l.ToArray()));
                cmd.Parameters.Add("p1", OracleDbType.Varchar2,200,"",ParameterDirection.Output);
                cmd.Parameters.Add("p2", OracleDbType.Decimal, 1m, ParameterDirection.Input);
                try
                {
                    cmd.ExecuteNonQuery();
                    Connect.Commit();
                    MessageBox.Show(cmd.Parameters["p1"].Value.ToString(), "АСУ КАДРЫ");
                    FillVacs();
                }
                catch (Exception ex)
                {
                    Connect.Rollback();
                    MessageBox.Show(ex.Message);
                }
                OracleCommand comm = new OracleCommand(string.Format(
                    "begin {0}.UPDATE_VAC_REG_DOC(:tran_id, :start_scan_date); end;",
                  Staff.DataSourceScheme.SchemeName), Connect.CurConnect);
                comm.BindByName = true;
                comm.Parameters.Add("tran_id", OracleDbType.Decimal);
                comm.Parameters.Add("start_scan_date", OracleDbType.Date);
                comm.Parameters["tran_id"].Value = Convert.ToInt32(transfer_id);
                comm.Parameters["start_scan_date"].Value = DateTime.Now.AddMonths(-6);
                try
                {
                    comm.ExecuteNonQuery();
                    Connect.Commit();
                }
                catch (Exception ex)
                {
                    Connect.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btUnlockVS_Click(object sender, EventArgs e)
        {
            (tabsTypeVS.SelectedTab.Controls[0] as DataGridView).EndEdit();
            List<string> l = new List<string>();
            DataGridView d;
            for (int i = 0; i < (d= tabsTypeVS.SelectedTab.Controls[0] as DataGridView).Rows.Count; ++i)
                if ((d.Rows[i].DataBoundItem as DataRowView)["CLOSE_SIGN"].ToString() != "0" && string.Format("{0}",d["fl", i].Value).ToUpper() == "TRUE")
                    l.Add((d.Rows[i].DataBoundItem as DataRowView)["vac_sched_id"].ToString());
            if (l.Count > 0 && MessageBox.Show("Разблокировать выбранные отпуска?", "Графики Отпусков", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.CLOSE_VAC(:p,:p1,:p2);end;", DataSourceScheme.SchemeName), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p", OracleDbType.Varchar2, string.Join(",", l.ToArray()), ParameterDirection.Input);
                cmd.Parameters.Add("p1", OracleDbType.Varchar2, 200, "", ParameterDirection.Output);
                cmd.Parameters.Add("p2", OracleDbType.Decimal, 0, ParameterDirection.Input);
                try
                {
                    cmd.ExecuteNonQuery();
                    Connect.Commit();
                    MessageBox.Show(cmd.Parameters["p1"].Value.ToString(), "АСУ КАДРЫ");
                    FillVacs();
                }
                catch (Exception ex)
                {
                    Connect.Rollback();
                    MessageBox.Show(ex.Message);
                }

                OracleCommand comm = new OracleCommand(string.Format(
                    "begin {0}.UPDATE_VAC_REG_DOC(:tran_id, :start_scan_date); end;",
                  Staff.DataSourceScheme.SchemeName), Connect.CurConnect);
                comm.BindByName = true;
                comm.Parameters.Add("tran_id", OracleDbType.Decimal, Convert.ToInt32(transfer_id), ParameterDirection.Input);
                comm.Parameters.Add("start_scan_date", OracleDbType.Date, DateTime.Now.AddMonths(-6), ParameterDirection.Input);
                try
                {
                    comm.ExecuteNonQuery();
                    Connect.Commit();
                }
                catch (Exception ex)
                {
                    Connect.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        public static void PrintOrderPlantReport(IWin32Window wnd, decimal? subdiv_id, string vac_ids)
        {
            string[][] s_pos = new string[][] { };
            if (Vacation_schedule.Signes.Show(wnd, subdiv_id, "OrderPlantVacShed", "Ввод должность и ФИО на месте подписи", 1, ref s_pos) == DialogResult.OK)
            {
                OracleDataAdapter oda = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\Rep_VSOrderPlant.sql"), Connect.Schema), Connect.CurConnect);
                oda.SelectCommand.Parameters.Add("p_vac_consist", OracleDbType.Array, vac_ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).Select(r=>Convert.ToDecimal(r)).ToArray(), ParameterDirection.Input)
                    .UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                oda.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
                oda.SelectCommand.BindByName = true;
                try
                {
                    DataTable t = new DataTable();
                    oda.Fill(t);
                    ReportViewerWindow.ShowReport("Просмотр отчета Унифицированная форма Т-6", "Rep_VSOrderPlant.rdlc", t,
                            new ReportParameter[] { new ReportParameter("P_FIO", s_pos[0][1]), new ReportParameter("P_POS_NAME", s_pos[0][0]) }
                            , System.Drawing.Printing.Duplex.Default, new string[] { "Word", "Excel" });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Library.GetMessageException(ex), "Ошибка формирования отчета");
                }
                /*OracleCommand cmd = new OracleCommand(string.Format("begin {0}.VAC_SCHED_PACK.GET_VS_ORDER_PLANT(:p_vac_consist,:p1); end;", Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_vac_consist", vac_ids);
                cmd.Parameters.Add("p1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataSet table = new DataSet();
                try
                {
                    new OracleDataAdapter(cmd).Fill(table);
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < table.Tables[0].Rows.Count; i++)
                    {
                        table.Tables[0].Rows[i]["A40"] = s_pos[0][0];
                        table.Tables[0].Rows[i]["Y44"] = s_pos[0][1];
                    }
                    Excel.PrintTemplateForEachRow("VSOrderPlant.xlt", "A1", "AJ48", table.Tables[0],
                        new ExcelParameter[]
                    {
                        new ExcelParameter("Z18",""),
                        new ExcelParameter("A20",""),
                        new ExcelParameter("A22",""),
                        new ExcelParameter("A24",""),
                        new ExcelParameter("A26",""),

                        new ExcelParameter("A32",""),
                        new ExcelParameter("A33",""),
                        new ExcelParameter("A34",""),

                        new ExcelParameter("G32",""),
                        new ExcelParameter("G33",""),
                        new ExcelParameter("G34",""),

                        new ExcelParameter("N32",""),
                        new ExcelParameter("N33",""),
                        new ExcelParameter("N34",""),

                        new ExcelParameter("D37",""),
                        new ExcelParameter("G37",""),
                        new ExcelParameter("K37",""),
                        new ExcelParameter("R37",""),
                        new ExcelParameter("U37",""),
                        new ExcelParameter("Z37",""),
                        new ExcelParameter("A40",""),
                        new ExcelParameter("Y44","")
                    }, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Library.GetMessageException(ex), "Невозможно распечатать приказ");
                }
                Cursor.Current = Cursors.Default;*/
            }

        }
        private void btPrintOrderPlantVS_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode.Level > 0)
            {
                PrintOrderPlantReport(this, this.SubdivID, (treeView.SelectedNode.Tag as DataRow)["vac_consist_id"].ToString());
            }
            else
                {
                    ToolTip t = new ToolTip();
                    t.ToolTipIcon = ToolTipIcon.Error;
                    t.Show("Для отчета требуется выбрать отпуск", groupBox1, 0, btNoteAccountVS.Height, 2100);
                }
        }

        /// <summary>
        /// Выбранный отпуск
        /// </summary>
        private DataRow SelectedVac
        {
            get
            {
                if (treeView.SelectedNode != null && treeView.SelectedNode.Level > 0 && treeView.SelectedNode.Tag != null && treeView.SelectedNode.Tag is DataRow)
                    return treeView.SelectedNode.Tag as DataRow;
                else return null;
            }
        }

        private void btRegDocToTabelVS_Click(object sender, EventArgs e)
        {
            try
            {
                OracleCommand comm = new OracleCommand(string.Format(Queries.GetQuery(@"GO\SynchronizeVacDocumnts.sql"), Connect.Schema), Connect.CurConnect);
                comm.BindByName = true;
                comm.Parameters.Add("p_transfer_id", OracleDbType.Decimal, Convert.ToInt32(transfer_id), ParameterDirection.Input);
                comm.Parameters.Add("p_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                comm.ExecuteNonQuery();
                FillVacs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }

        }

        private void btViewVacAddP_Click(object sender, EventArgs e)
        {
            AdditionalVacs f = new AdditionalVacs(decimal.Parse(transfer_id));
            if (f.ShowDialog(this) == DialogResult.Yes)
            {
                FillAddVacs();
            }
        }

        private void tsbtRNoteVacVS_Click(object sender, EventArgs e)
        {
            if (SelectedVac != null)
                RepVacNotification(this, SubdivID, new Decimal[] { SelectedVac.Field2<Decimal>("VAC_SCHED_ID") });
            else
                MessageBox.Show("Не выбран отпуск для отчета");
        }

        private void tsbtOnlyActual_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tabsTypeVS.TabCount; ++i)
            {
                ((tabsTypeVS.TabPages[i].Controls[0] as DataGridView).DataSource as DataView).RowFilter = string.Format("VAC_GROUP_TYPE_ID={0} {1}", ((tabsTypeVS.TabPages[i].Controls[0] as DataGridView).DataSource as DataView)[0].Row["VAC_GROUP_TYPE_ID"], tsbtOnlyActual.Checked ? "and PLAN_SIGN=0" : "");
            }
        }

        private void FillRemVacsVS(DateTime d)
        {
            ds.Tables["rem_vacs"].Clear();
            ad_rem_vac.SelectCommand.Parameters["p_date_calc"].Value = d;
            ad_rem_vac.Fill(ds.Tables["rem_vacs"]);
            if (gridRemVacsVS.DataSource == null)
            {
                gridRemVacsVS.DataSource = new DataView(ds.Tables["rem_vacs"], "", "VAC_GROUP_TYPE_ID", DataViewRowState.CurrentRows);
                ColumnWidthSaver.FillWidthOfColumn(gridRemVacsVS);
            }
        }
        
        private void tsbtRecalcRemVacs_Click(object sender, EventArgs e)
        {
            FillRemVacsVS(dpDateRemVac.Value.Date);
        } 
        public static void OpenStaticView(object sender, LinkData e)
        {
            ViewCard f = new ViewCard(e.Transfer_id.ToString(), null);
            f.ShowDialog();
        }

        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Transfer_id, "VS_VIEW");
        }

        /// <summary>
        /// Отчет уведомление об отпуске
        /// </summary>
        /// <param name="subdivid"></param>
        /// <param name="vac_ids"></param>
        public static void RepVacNotification(IWin32Window owner, decimal subdivid, decimal[] vac_ids)
        {
            string[][] str = null;
            if (Signes.Show(subdivid, "VacNotification", "Выберите ответственные лица", 1, ref str) == DialogResult.OK)
            {
                try
                {
                    OracleDataAdapter oda = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Go\Rep_VacNotification.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                    oda.SelectCommand.BindByName = true;
                    oda.SelectCommand.Parameters.Add("p_vac_ids", OracleDbType.Array, vac_ids, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    DataSet ds = new DataSet();
                    oda.Fill(ds);

                    ReportViewerWindow.ShowReport("Уведомления об отпуске", "Rep_VacNotification.rdlc", ds.Tables[0], 
                        new ReportParameter[]{
                                                new ReportParameter("P_FIO", str[0][1]),
                                                new ReportParameter("P_POS_NAME", str[0][0])
                                                });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка получения данных");
                }
            }
        }
    }
}
