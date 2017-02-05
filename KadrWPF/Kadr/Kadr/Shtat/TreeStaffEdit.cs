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
   
    public partial class TreeStaffEdit : Form
    {
         Dictionary<string, my> d;
        struct my
        {
            public string name, fam;
            public List<my> Nodes;
        }
        private my make_my(string n,string m,List<my> l)
        {
            my c= new my();
            c.name=n;
            c.fam = m;
            c.Nodes = l;
            return c;
        }
        public TreeStaffEdit()
        {
            InitializeComponent();
            btToTree.Click += new EventHandler(this.subdiv_TextChanged);
        }
        public TreeNode FillTree(string name)
        {
            if (d.ContainsKey(name))
            {
                TreeNode t = new TreeNode();
                t.Name = name;
                t.Text = d[name].fam;
                for (int i = 0; i < d[name].Nodes.Count; i++)
                {
                    TreeNode n;
                    if ((n=FillTree(d[name].Nodes[i].name))!=null)
                    t.Nodes.Add(n);
                }
                return t;
            }
            else return null;
        }
        public void TreeStaffEdit_Load(object sender, EventArgs e)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select staffs.staffs_id,staff_parent_id,pos_name,emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' \"last_name\" " +
                        " from {0}.staffs join {0}.position on (staffs.pos_id=position.pos_id) left join (select per_num,staffs_id from {0}.transfer where sign_cur_work=1) t  on (staffs.staffs_id=t.staffs_id) left join {0}.emp on (emp.per_num=t.per_num) " +
                        " where staff_sign!=2 {1} order by pos_name", DataSourceScheme.SchemeName,
                        (FormMain.ShtatFilter.subdiv_id=="0"?"":string.Format(" and staffs.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id) ",DataSourceScheme.SchemeName,FormMain.ShtatFilter.subdiv_id))
                        +
                        (FormMain.ShtatFilter.DegreeName == "Все" ? "" : string.Format(" and staffs.degree_id=(select degree_id from {0}.degree where degree_name='{1}') ", DataSourceScheme.SchemeName, FormMain.ShtatFilter.DegreeName))
                        ), Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.SelectCommand.BindByName = true;
            adapter.Fill(table);
            d = new Dictionary<string,my>();
            string staffs_id,last_name, pos_name,staff_parent_id;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                staffs_id = table.Rows[i]["staffs_id"].ToString();
                staff_parent_id = table.Rows[i]["staff_parent_id"].ToString();
                last_name = table.Rows[i]["last_name"].ToString();
                pos_name = table.Rows[i]["pos_name"].ToString();
                pos_name = pos_name + " (" + last_name + ")";
                if (staff_parent_id == "")
                    if (d.ContainsKey("+"))
                        d["+"].Nodes.Add(make_my(staffs_id, pos_name ,new List<my>()));
                    else
                    {
                        d.Add("+", make_my("+", "+",new List<my>()));
                        d["+"].Nodes.Add(make_my(staffs_id, pos_name ,new List<my>()));
                    }
                else
                    if (d.ContainsKey(table.Rows[i]["staff_parent_id"].ToString()))
                    {
                        d[staff_parent_id].Nodes.Add(make_my(staffs_id, pos_name ,new List<my>()));
                    }
                    else
                    {
                        d.Add(staff_parent_id, make_my("", "",new List<my>()));
                        d[staff_parent_id].Nodes.Add(make_my(staffs_id, pos_name ,new List<my>()));
                    }
                if (d.ContainsKey(staffs_id))
                {
                    d[staffs_id] = make_my(staffs_id, pos_name , d[staffs_id].Nodes);
                }
                else
                    d.Add(staffs_id, make_my(staffs_id, pos_name , new List<my>()));
            }
            TreeStaff.Nodes.Clear();
            if (table.Rows.Count > 0)
                for (int i = 0; i < d["+"].Nodes.Count; i++)
                {
                    TreeNode t = new TreeNode();
                    if ((t = FillTree(d["+"].Nodes[i].name)) != null)
                        TreeStaff.Nodes.Add(t);
                }
        }

        private void subdiv_TextChanged(object sender, EventArgs e)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select staffs.staffs_id,staff_parent_id,pos_name \"Профессия\", emp_last_name||' '||substr(emp_first_name,1,1)||'.'||substr(emp_middle_name,1,1)||'.' \"ФИО\" " +
                " from {0}.staffs join {0}.position on (staffs.pos_id=position.pos_id) left join (select per_num,staffs_id from {0}.transfer where sign_cur_work=1) t  on (staffs.staffs_id=t.staffs_id) left join on (emp.per_num=t.per_num) " +
                " where staffs.staff_sign!=2 and  staffs.subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id = {1} connect by prior subdiv_id=parent_id) order by pos_name", DataSourceScheme.SchemeName, subdivSelector1.subdiv_id), Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.SelectCommand.BindByName = true;
            adapter.Fill(table);
            GridStaffs.DataSource = table;
            
            GridStaffs.Columns["staffs_id"].Visible = false;
            GridStaffs.Columns["staff_parent_id"].Visible = false;
        }

        private void btToTree_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand("", Connect.CurConnect);
            cmd.BindByName = true;
            Dictionary<string, string> d = new Dictionary<string, string>();//это АССОЦИАТИВНЫЙ КОНТЕЙНЕР! МАССИВ ПОЧТИ, НО НЕ ОЧЕНЬ
            d.Clear();
            for (int i = 0; i < GridStaffs.SelectedRows.Count; i++)
                d.Add(GridStaffs.SelectedRows[i].Cells["staffs_id"].Value.ToString(), "");
            for (int i = GridStaffs.SelectedRows.Count - 1;i>-1;i--)
                if (GridStaffs.SelectedRows[i].Cells["staff_parent_id"].Value.ToString() != TreeStaff.SelectedNode.Name && GridStaffs.SelectedRows[i].Cells["staffs_id"].Value.ToString() != TreeStaff.SelectedNode.Name && !d.ContainsKey(GridStaffs.SelectedRows[i].Cells["staff_parent_id"].Value.ToString()))
                {
                    TreeNode node = TreeStaff.Nodes.Find(GridStaffs.SelectedRows[i].Cells["staffs_id"].Value.ToString(), true)[0];
                    node.ForeColor = Color.Red;
                    TreeStaff.Nodes.Remove(node);
                    TreeStaff.SelectedNode.Nodes.Add(node);
                    GridStaffs.SelectedRows[i].DefaultCellStyle.SelectionBackColor = Color.Green;
                    cmd.CommandText = string.Format("update {0}.staffs set staff_parent_id={1} where staffs_id={2}", DataSourceScheme.SchemeName, TreeStaff.SelectedNode.Name, GridStaffs.SelectedRows[i].Cells["staffs_id"].Value.ToString());
                    cmd.ExecuteNonQuery();
                }
             cmd.CommandText="commit";
            cmd.ExecuteNonQuery();

        }

        private void btFromTree_Click(object sender, EventArgs e)
        {
            if (TreeStaff.SelectedNode!=null)
            {
                OracleCommand cmd = new OracleCommand(string.Format("update {0}.staffs set staff_parent_id=null where staffs_id={1}", DataSourceScheme.SchemeName, TreeStaff.SelectedNode.Name), Connect.CurConnect);
                    cmd.ExecuteNonQuery();
                    TreeNode t=TreeStaff.SelectedNode;
                    TreeStaff.Nodes.Remove(TreeStaff.SelectedNode);
                    TreeStaff.Nodes.Add(t);
            }
        }

        private void TreeStaffEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain.UnCheckButtonShtat("btEditSubdiv");
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TreeStaff_ChangeUICues(object sender, UICuesEventArgs e)
        {
            Elegant.Ui.ScreenTipData d = screenTip1.GetScreenTip(TreeStaff);
            TreeNode node = TreeStaff.GetNodeAt(MousePosition);    
            if (node!=null)
            {

                d.Text = node.Name;
            }
        }

        private void TreeStaff_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            Elegant.Ui.ScreenTipData d = screenTip1.GetScreenTip(TreeStaff);
            TreeNode node = TreeStaff.GetNodeAt(TreeStaff.PointToClient(MousePosition));
            if (node != null)
            {
                //TreeStaff.SelectedNode = node;
                OracleDataReader reader = new OracleCommand(string.Format("select subdiv_name from {0}.staffs join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) where staffs_id={1}", DataSourceScheme.SchemeName, node.Name), Connect.CurConnect).ExecuteReader(); ;
                if (reader.Read())
                    d.Text = reader[0].ToString();
            }
        }
        

        private void TreeStaff_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode t = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode",true);
            TreeNode t1 = TreeStaff.SelectedNode;
            if (t1!=t)
            { 
                TreeStaff.Nodes.Remove(t);
                if (t1!=null)
                    TreeStaff.GetNodeAt(TreeStaff.PointToClient(new Point(e.X, e.Y))).Nodes.Add(t);
                else
                    TreeStaff.Nodes.Add(t);

                //OracleCommand cmd = new OracleCommand(string.Format("update {0}.staffs set staff_parent_id='{1}' where staffs_id=''",DataSourceScheme.SchemeName, 
                t.EnsureVisible();
                TreeStaff.SelectedNode = t;
            }
            t.ForeColor = Color.Black;
            t.BackColor = Color.White;
        }

        private void TreeStaff_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeStaff.SelectedNode = (TreeNode)e.Item;
            TreeStaff.SelectedNode.BackColor = Color.Gainsboro;
            TreeStaff.SelectedNode.ForeColor = Color.Coral;
            DoDragDrop(TreeStaff.SelectedNode, DragDropEffects.Move);           
        }

        private void TreeStaff_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        private void TreeStaff_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
            {
                TreeNode t = TreeStaff.GetNodeAt(TreeStaff.PointToClient(new Point(e.X, e.Y)));
                TreeNode t1 = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode", true);
                if (t != null && t1.Nodes.Find(t.Name, true).Length == 0)
                {
                    TreeStaff.SelectedNode = TreeStaff.GetNodeAt(TreeStaff.PointToClient(new Point(e.X, e.Y)));
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    TreeStaff.SelectedNode = null;
                }
            }
            else
                e.Effect = DragDropEffects.None;
        }

        
    }
}
