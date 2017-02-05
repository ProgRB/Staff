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
    public partial class ViewArchiv : Form
    {
        string subdiv_id;
        SelectSubdivFromTree frm;
        public ViewArchiv()
        {
            InitializeComponent();
        }

        private void ViewArchiv_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

       
        private void btFind_Click(object sender, EventArgs e)
        {
            OracleDataAdapter adapter = new OracleDataAdapter("select pos_name \"Профессия\",subdiv_name \"Подразделение\",degree_name \"Категория\",date_begin_staff \"Дата начала\",date_end_staff \"Дата окончания\",staffs_id from {0}.staffs left join {0}.position on (staffs.pos_id=position.pos_id) left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.degree on (staffs.degree_id=degree.degree_id) where staff_sign=2", Connect.CurConnect);
            adapter.SelectCommand.CommandText+=(pos_name.Text.Length>0?" and upper(pos_name) like upper('%"+pos_name.Text+"%')":"");
            adapter.SelectCommand.CommandText += (Subdiv.Text.Length > 0 ? " and subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=" + subdiv_id + " connect by prior subdiv_id=parent_id)" : "");
            adapter.SelectCommand.CommandText = string.Format(adapter.SelectCommand.CommandText, DataSourceScheme.SchemeName);
            adapter.SelectCommand.BindByName = true;
            DataTable table = new DataTable();
            adapter.Fill(table);
            gridFindResult.DataSource = table;
            gridFindResult.Columns["staffs_id"].Visible = false;
        }

        

        private void Subdiv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                Subdiv.Text = "";
                subdiv_id = "0";
            }
        }

        private void gridFindResult_SelectionChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            if (gridFindResult.SelectedRows.Count > 0)
            {
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("new/select_HISTORY_staff.sql"), DataSourceScheme.SchemeName, gridFindResult.SelectedRows[0].Cells[5].Value.ToString()), Connect.CurConnect);
                
                adapter.Fill(table);
                GridChanges.DataSource = null;
                GridChanges.Rows.Clear();
                GridChanges.Columns.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                    GridChanges.Columns.Add(table.Rows[i][8].ToString(), table.Rows[i][8].ToString());

                GridChanges.Rows.Add(table.Columns.Count);
                GridChanges.Rows[0].HeaderCell.Value = "Профессия";
                GridChanges.Rows[1].HeaderCell.Value = "Подразделение"; ;
                GridChanges.Rows[2].HeaderCell.Value = "Категория";
                GridChanges.Rows[3].HeaderCell.Value = "График работы";
                GridChanges.Rows[4].HeaderCell.Value = "Надбавка за вредность";
                GridChanges.Rows[5].HeaderCell.Value = "Максимальный тариф";
                GridChanges.Rows[6].HeaderCell.Value = "Вакантность";
                GridChanges.Rows[7].HeaderCell.Value = "Дата окончания вакансии";
                GridChanges.Rows[8].HeaderCell.Value = "Дата начала действия единицы";
                GridChanges.Rows[9].HeaderCell.Value = "Дата окончания действия";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i][10].ToString() == gridFindResult.SelectedRows[0].Cells[5].Value.ToString())
                    {
                        GridChanges.Columns[i].HeaderCell.Style.BackColor = Color.Blue;
                    }
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        GridChanges.Rows[j].Cells[i].Value = table.Rows[i][j].ToString();
                        if (table.Rows[i][10].ToString() == gridFindResult.SelectedRows[0].Cells[5].Value.ToString())
                            GridChanges.Rows[j].Cells[i].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaptionText);

                        if (i > 0)
                            if (GridChanges.Rows[j].Cells[i].Value.ToString() != GridChanges.Rows[j].Cells[i - 1].Value.ToString())
                                GridChanges.Rows[j].Cells[i].Style.BackColor = Color.LightCoral;
                    }
                }

            }
            else GridChanges.DataSource = table;

        }

        

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewArchiv_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            Kadr.FormMain.ActiveForm.Enabled = false;
            Kadr.FormMain.ActiveForm.Enabled = true;
        }

        private void Subdiv_Click(object sender, EventArgs e)
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new SelectSubdivFromTree(Subdiv.Text, "");
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormBorderStyle = FormBorderStyle.None;
                Rectangle R = new Rectangle(new Point(0, 0), Subdiv.Size);
                Rectangle r = Subdiv.RectangleToScreen(R);
                frm.Left = r.Left;
                frm.Top = r.Bottom + 1;
                frm.Width = r.Width + 1;
                frm.ShowDialog(this);
                if (frm.subdiv_id != "-1")
                {
                    subdiv_id = frm.subdiv_id;
                    this.Subdiv.Text = frm.subdiv_name;
                }
                frm.Dispose();
            }
        }

       
    }
}
