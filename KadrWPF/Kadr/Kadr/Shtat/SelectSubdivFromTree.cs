using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Security;
using System.Security.Permissions;
using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr.Shtat
{
    public partial class SelectSubdivFromTree : Form
    {        
        public OracleConnection connect;
        public string subdiv_name,subdiv_id,code_subdiv;
        public string Path="";
        private string With_AcSubd="";
        private Dictionary<string, HashSet<string> > Tree_sub;
        private Dictionary<string, string> Captions;

        private List<string> GetMainVerse()
        {
            HashSet<string> visit = new HashSet<string>();
            Dictionary<string, HashSet<string>>.Enumerator it = Tree_sub.GetEnumerator();
            while (it.MoveNext())
            {
                HashSet<string>.Enumerator j = it.Current.Value.GetEnumerator();
                while (j.MoveNext())
                {
                    visit.Add(j.Current);
                }
            }
            it = Tree_sub.GetEnumerator();
            List<string> l = new List<string>();
            while (it.MoveNext())
                if (!visit.Contains(it.Current.Key))
                    l.Add(it.Current.Key);
            return l;
        }
        /// <summary>
        /// Строит дерево из исходного словаря значений
        /// </summary>
        /// <param name="?"></param>
        private void BuildTree(List<string> sub,TreeNode t)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                BuildTree(Tree_sub[sub[i]].ToList<string>(),t.Nodes.Add(sub[i],Captions[sub[i]]));
                t.ToolTipText=Captions[sub[i]].Substring(1, 3);
            }
        }
        public SelectSubdivFromTree(string path, string _Text)
        {
            subdiv_name = subdiv_id = "0";
            connect = Connect.CurConnect;
            Path = path;            
            InitializeComponent();
            this.Text = _Text;
        }
       
        public SelectSubdivFromTree(int CurrenSubdivID, string _Text)
        {
            connect = Connect.CurConnect;
            InitializeComponent();
            subdiv_name = "";
            subdiv_id = CurrenSubdivID.ToString();
            Path = "";
            this.Text = _Text;
            With_AcSubd = "";
            /**************Пошло построение дерева подразделений**********/
            OracleDataAdapter a = new OracleDataAdapter(string.Format("select * from {0}.subdiv where sub_actual_sign=1 order by code_subdiv",
                DataSourceScheme.SchemeName), connect);
            a.SelectCommand.BindByName = true;
            DataTable t = new DataTable();
            a.Fill(t);
            string k, p;
            Tree_sub = new Dictionary<string, HashSet<string>>();
            Captions = new Dictionary<string, string>();
            for (int i = 0; i < t.Rows.Count; i++)
            {
                k = t.Rows[i]["subdiv_id"].ToString();
                p = t.Rows[i]["parent_id"].ToString();
                if (p == "") p = "-1";
                if (!Tree_sub.ContainsKey(p))
                    Tree_sub.Add(p, new HashSet<string>(new string[] { k }));
                else
                    Tree_sub[p].Add(k);
                if (!Tree_sub.ContainsKey(k))
                    Tree_sub.Add(k, new HashSet<string>());
                if (k == "0")
                    Captions.Add("0", "Улан-Удэнский Авиационный завод");
                else
                    Captions.Add(k, string.Format("({0}) {1}", t.Rows[i]["code_subdiv"].ToString(), t.Rows[i]["subdiv_name"].ToString()));
            }
            Dictionary<string, HashSet<string>> s1 = new Dictionary<string, HashSet<string>>(Tree_sub);
            Tree_sub.Clear();
            Dictionary<string, HashSet<string>>.Enumerator it = s1.GetEnumerator();
            while (it.MoveNext())
                if (Captions.ContainsKey(it.Current.Key))
                    Tree_sub.Add(it.Current.Key, it.Current.Value);
            treeSubdiv.Nodes.Clear();
            BuildTree(GetMainVerse(), treeSubdiv.Nodes.Add("-1", "Подразделения завода"));
            TreeNode[] tr = treeSubdiv.Nodes.Find(CurrenSubdivID.ToString(),true);
            if (tr.Count() > 0)
            {
                tr[0].EnsureVisible();
                treeSubdiv.SelectedNode = tr[0];
            }
        }
        public SelectSubdivFromTree(int CurrenSubdivID, string _Text, string WithAccessSubdiv)
        {
            connect = Connect.CurConnect;
            InitializeComponent();
            subdiv_name = "";
            subdiv_id = CurrenSubdivID.ToString();
            Path = "";
            this.Text = _Text;
            With_AcSubd = WithAccessSubdiv;
            /**************Пошло построение дерева подразделений**********/
            OracleDataAdapter a = new OracleDataAdapter(string.Format("select * from {0}.subdiv_ROLES where upper(app_name)=upper('{1}') order by code_subdiv", 
                DataSourceScheme.SchemeName,WithAccessSubdiv), connect);
            a.SelectCommand.BindByName = true;
            DataTable t = new DataTable();
            a.Fill(t);
            string k , p;
            Tree_sub = new Dictionary<string, HashSet<string>>();
            Captions = new Dictionary<string, string>();
            for (int i = 0; i < t.Rows.Count; i++)
            {
                k = t.Rows[i]["subdiv_id"].ToString();
                p = t.Rows[i]["parent_id"].ToString();
                if (p == "") p = "-1";
                if (!Tree_sub.ContainsKey(p))
                    Tree_sub.Add(p,new HashSet<string>(new string[]{k}));
                else
                    Tree_sub[p].Add(k);
                if (!Tree_sub.ContainsKey(k))
                    Tree_sub.Add(k, new HashSet<string>());
                if (k == "0")
                    Captions["0"]="Улан-Удэнский Авиационный завод";
                else
                    Captions[k]=string.Format("({0}) {1}",t.Rows[i]["code_subdiv"].ToString(),t.Rows[i]["subdiv_name"].ToString());
            }
            Dictionary<string, HashSet<string>> s1 = new Dictionary<string,HashSet<string>>(Tree_sub);
            Tree_sub.Clear();
            Dictionary<string, HashSet<string>>.Enumerator it = s1.GetEnumerator();
            while (it.MoveNext())
                if (Captions.ContainsKey(it.Current.Key))
                    Tree_sub.Add(it.Current.Key,it.Current.Value);
            treeSubdiv.Nodes.Clear();
            BuildTree(GetMainVerse(),treeSubdiv.Nodes.Add("-1","Подразделения завода"));
            TreeNode[] tr = treeSubdiv.Nodes.Find(CurrenSubdivID.ToString(), true);
            if (tr.Count() > 0)
            {
                tr[0].EnsureVisible();
                treeSubdiv.SelectedNode = tr[0];
            }
        }

        public void SelectSubdivFromTree_Load(object sender, EventArgs e)
        {
            if (this.Owner!=null) this.Owner.Enabled = false;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (treeSubdiv.SelectedNode == null || treeSubdiv.SelectedNode.Name == "-1")
            {
                ToolTip t = new ToolTip();
                t.ToolTipIcon = ToolTipIcon.Info;
                t.Show("Не выбрано подразделение",btOk,2000);
                return; 
            }
            string s = treeSubdiv.SelectedNode.FullPath;
            if (s.IndexOf('/') > 0)
                s = s.Substring(s.IndexOf('/')+1, s.Length - s.IndexOf('/')-1);
            subdiv_name = s;
            subdiv_id = treeSubdiv.SelectedNode.Name;
            if (subdiv_id != "0")
            {
                OracleDataReader r = new OracleCommand(string.Format("select code_subdiv from {0}.subdiv where subdiv_id={1}", DataSourceScheme.SchemeName, subdiv_id), connect).ExecuteReader();
                if (r.Read())
                    code_subdiv = r[0].ToString();
            }
            this.Close();                
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            subdiv_id = "-1";
            this.Close();           
        }

        private void SelectSubdivFromTree_Shown(object sender, EventArgs e)
        {
            ++this.Width;
        }
        private void SelectSubdivFromTree_Activated(object sender, EventArgs e)
        { 
            int i;
            if ((i=this.PointToScreen(new Point(0, 0)).Y) + this.Height > Screen.PrimaryScreen.Bounds.Height)
                this.Height = Screen.PrimaryScreen.Bounds.Height - this.PointToScreen(new Point(0,0)).Y-40;
        }

        private void SelectSubdivFromTree_Paint(object sender, PaintEventArgs e)
        {
            Pen p= new Pen(Color.SkyBlue);
            p.Width=6;
            e.Graphics.DrawLines(p,new Point[]{new Point(0,0),new Point(0,this.Height),new Point(this.Width,this.Height),new Point(this.Width,0)});
            p.Width = 1;
            p.Color = Color.DodgerBlue;
            e.Graphics.DrawLines(p, new Point[] { new Point(2, 0), new Point(2, this.Height-2), new Point(this.Width-2, this.Height-2), new Point(this.Width-2, 0) });

        }

        private void SelectSubdivFromTree_ResizeEnd(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void treeStaff_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void SelectSubdivFromTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Owner != null) this.Owner.Enabled = true;
        }

       
    }
}
