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

namespace Kadr.Shtat
{
    public partial class AddToTreeEmp : Form
    {
        public SelectSubdivFromTree frm;
        public AddToTreeEmp()
        {
            InitializeComponent();
            btToTree.Click += new EventHandler(this.subdiv_TextChanged);
            btFromTree.Click += new EventHandler(this.subdiv_TextChanged);
        }

        private void AddToTreeEmp_Load(object sender, EventArgs e)
        {
            this.treeStaff.Nodes.Add("0","Улан-Удэнский авиационный завод");
            treeStaff.Nodes[0].Nodes.Add("No");
            LibraryKadr.Settings.SetDataGridCoumnWidth(ref listStaffsShtat);
        }

        private void treeStaff_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeStaff.SelectedNode = e.Node;
            e.Node.Nodes.Clear();
            DataTable table = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select pos_name||'('||count(sf.pos_id)||')' from {0}.position p join {0}.staffs sf on (sf.pos_id=p.pos_id) join {0}.subdiv sb on (sf.subdiv_id=sb.subdiv_id) group by subdiv_id,pos_name having subdiv_id={1} order by pos_name", DataSourceScheme.SchemeName, treeStaff.SelectedNode.Name), Connect.CurConnect);
            /*adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
                e.Node.Nodes.Add("-1",table.Rows[i][0].ToString());*/
            adapter.SelectCommand.CommandText=string.Format("select subdiv_name,subdiv_id from {0}.subdiv where parent_id={1} and sub_actual_sign=1 and code_subdiv<'700' order by 1", DataSourceScheme.SchemeName, treeStaff.SelectedNode.Name);
            adapter.SelectCommand.BindByName = true;
            table.Reset();
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                e.Node.Nodes.Add(table.Rows[i]["subdiv_id"].ToString(),table.Rows[i]["subdiv_name"].ToString());
                e.Node.Nodes[e.Node.Nodes.Count-1].Nodes.Add("-1","Нет сотрудников");
            }

            if (e.Node.Nodes.Count == 0)
                e.Node.Nodes.Add("-1","Нет сотрудников");
        }

        private void treeStaff_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            treeStaff.SelectedNode = e.Node;
            if (e.Node != this.treeStaff.Nodes[0])
            {
                e.Node.Nodes.Clear();
                e.Node.Nodes.Add("Нет сотрудников");
            }
        }
        private void menuTreeNode_Opening(object sender, CancelEventArgs e)
        {
            OracleCommand cmd = new OracleCommand(string.Format("select subdiv_id from {0}.subdiv where subdiv_id={1}", DataSourceScheme.SchemeName, treeStaff.SelectedNode.Name), Connect.CurConnect);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                menuTreeNode.Items["add"].Enabled = true;
                menuTreeNode.Items["rename"].Enabled = true;
                cmd.CommandText = string.Format("select subdiv_id from {0}.subdiv where parent_id={1} and sub_actual_sign=1", DataSourceScheme.SchemeName, reader["subdiv_id"].ToString());
                reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                /*OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select pos_name||'('||count(staffs.pos_id)||')' from {0}.position join {0}.staffs on (staffs.pos_id=position.pos_id) join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) group by subdiv_id,pos_name,pos_actual_sign having subdiv_id={1} and pos_actual_sign=1 order by pos_name", DataSourceScheme.SchemeName, treeStaff.SelectedNode.Name), Connect.CurConnect);
                adapter.Fill(table);*/
                if (!reader.Read() && table.Rows.Count==0)
                {
                    menuTreeNode.Items["delete"].Enabled = true;
                }
                else 
                {
                    menuTreeNode.Items["delete"].Enabled = false;
                }
            }
            else
            {
                menuTreeNode.Items["rename"].Enabled = false;
                menuTreeNode.Items["add"].Enabled = false;
                menuTreeNode.Items["delete"].Enabled = false;
            };
        }

        private void добавитьУзелToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSubdivToTree frmAddNode = new AddSubdivToTree(Connect.CurConnect, "add", treeStaff.SelectedNode.Name);
            frmAddNode.ShowDialog();
            treeStaff.SelectedNode.Collapse(true);
            treeStaff.SelectedNode.Expand();
        }

        private void btToTree_Click(object sender, EventArgs e)
        {
            if (SS1.subdiv_id != 0 && treeStaff.SelectedNode != null)
            {
                TreeNode s;
                if (treeStaff.SelectedNode.Name == "-1")
                    s = treeStaff.SelectedNode.Parent;
                else s = treeStaff.SelectedNode;
            
                for (int i = 0; i < listStaffsShtat.Rows.Count; i++)
                if (listStaffsShtat["check",i].Value.ToString()=="True")
                {
                    OracleCommand cmd = new OracleCommand(string.Format("update {0}.staffs set subdiv_id={1} where staffs_id={2}",
                    DataSourceScheme.SchemeName, s.Name, listStaffsShtat.Rows[i].Cells["staffs_id"].Value.ToString()), Connect.CurConnect);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                }
                s.Collapse();
                s.Expand();
            }
            else
            {
                toolTip1.Show("Не выбрано подразделение!", splitContainer1,MousePosition, 3000);
            }

        }
        private void btFromTree_Click(object sender, EventArgs e)
        {
            if (SS1.subdiv_id == 0) return;
            OracleCommand cmd =new OracleCommand();
            cmd.Connection = Connect.CurConnect;
            for (int i=0;i<GridEmp.SelectedRows.Count;i++)
            {
                cmd.CommandText=string.Format("update {0}.staffs set subdiv_id={1} where staffs_id={2}",
                        DataSourceScheme.SchemeName, SS1.subdiv_id, GridEmp.SelectedRows[i].Cells["staffs_id"].Value.ToString());
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
            }
           
            treeStaff.SelectedNode.Parent.Collapse();// свернем и развернем чтобы обновить список
            treeStaff.SelectedNode.Expand();
        }
        private void rename_Click(object sender, EventArgs e)
        {
            AddSubdivToTree frmEditNode = new AddSubdivToTree(Connect.CurConnect, "edit", treeStaff.SelectedNode.Name);
            frmEditNode.ShowDialog();
            treeStaff.SelectedNode.Parent.Collapse(true);
            treeStaff.SelectedNode.Expand();
        }
        private void delete_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand(string.Format("delete from {0}.subdiv where subdiv_id={1}", DataSourceScheme.SchemeName, treeStaff.SelectedNode.Name), Connect.CurConnect);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            treeStaff.SelectedNode.Parent.Collapse();
            treeStaff.SelectedNode.Expand();
        }

        private void treeStaff_AfterSelect(object sender, TreeViewEventArgs e)
        {
            /*treeStaff.SelectedNode = e.Node;
            DataTable table = new DataTable();
            if (e.Node.Text.IndexOf('(') > 0)
            {
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/select_staffs_for_tree.sql"), DataSourceScheme.SchemeName, e.Node.Text.Substring(0, e.Node.Text.IndexOf('(')), treeStaff.SelectedNode.Parent.Name), Connect.CurConnect);
                adapter.Fill(table);
            }
            GridEmp.DataSource = table;
            if (GridEmp.Columns.Contains("staffs_id"))
            GridEmp.Columns["staffs_id"].Visible = false;*/

        }

        private void treeStaff_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeStaff.SelectedNode = e.Node;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void subdiv_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable(); //поменяли отдел в выпадающем списке,пока перегрузить список сотрудников из базы
            
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/load_listStaffs_by_subdiv.sql"), DataSourceScheme.SchemeName, SS1.subdiv_id,
                (listStaffsShtat.SortedColumn != null ? (listStaffsShtat.SortedColumn.Index.ToString() + (listStaffsShtat.SortOrder == SortOrder.Ascending ? " asc " : " desc ")) : " 1 ")), Connect.CurConnect);
            adapter.Fill(table);
            listStaffsShtat.Rows.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                listStaffsShtat.Rows.Add();
                listStaffsShtat["check",i].Value=false;
                listStaffsShtat["pos", i].Value = table.Rows[i]["pos_name"].ToString();
                listStaffsShtat["name", i].Value = table.Rows[i]["emp_first_name"].ToString();
                listStaffsShtat["fam", i].Value = table.Rows[i]["emp_last_name"].ToString();
                listStaffsShtat["s_name", i].Value = table.Rows[i]["emp_middle_name"].ToString();
                listStaffsShtat["staffs_id", i].Value = table.Rows[i]["staffs_id"].ToString();
            }

        }

        private void btToTree_MouseEnter(object sender, EventArgs e)
        {
            if (treeStaff.SelectedNode != null)
            {
                TreeNode nd;
                if (treeStaff.SelectedNode.Name == "-1")
                    nd = treeStaff.SelectedNode.Parent;
                else
                    nd = treeStaff.SelectedNode;
                nd.EnsureVisible();
                btToTree.Focus();
                treeStaff.HideSelection = true;
                nd.BackColor = Color.LightGreen;
                for (int i = 0; i < listStaffsShtat.RowCount; i++)
                    if (listStaffsShtat["check",i].Value.ToString()=="True")
                    {
                        listStaffsShtat.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        listStaffsShtat.Rows[i].DefaultCellStyle.SelectionBackColor = Color.LightGreen;
                    }
            }
            
        }

        private void btToTree_MouseLeave(object sender, EventArgs e)
        {
            if (treeStaff.SelectedNode != null)
            {
                treeStaff.HideSelection = false;
                TreeNode nd;
                if (treeStaff.SelectedNode.Name == "-1")
                    nd = treeStaff.SelectedNode.Parent;
                else
                    nd = treeStaff.SelectedNode;
                nd.BackColor = Color.White;
                for (int i = 0; i < listStaffsShtat.RowCount; i++)
                    if (listStaffsShtat["check", i].Value.ToString() == "True")
                    {
                        listStaffsShtat.Rows[i].DefaultCellStyle.BackColor = Color.White;
                        listStaffsShtat.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
                    }
            }
        }

        private void listStaffsShtat_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            LibraryKadr.ColumnWidthSaver.SaveWidthOfColumn(sender,e);
        }
    }
}
