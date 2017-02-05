using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using Oracle.DataAccess.Client;

namespace Kadr.Shtat
{
    public partial class FindStaffEd : Form
    {
        OracleConnection connect;
        public string staffs_id, staff_pos_name, staff_subdiv_name, last_name, per_num;
        public FindStaffEd(OracleConnection cnt)
        {
            connect = cnt;
            per_num = "";
            staffs_id = "0";
            InitializeComponent();
            DG.CellDoubleClick += new DataGridViewCellEventHandler(this.btOk_Click);
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (Per_num.Text == "" && Subdiv.Text == "" && Degree.Text == "" && Position.Text == "")
            {
                MessageBox.Show("Вы не ввели ни одного критерия поиска!");
                return;
            }
            string filter = string.Format("select staffs.staffs_id, pos_name as \"Профессия\",subdiv_name as \"Подразделение\",degree_name as \"Категория\", per_num as \"Табельный номер\",emp_last_name as \"Фамилия\",emp_first_name as \"Имя\",emp_middle_name as \"Отчество\"" +
                " from {0}.staffs left join {0}.transfer on (transfer.staffs_id=staffs.Staffs_id) left join {0}.emp on (transfer.per_num=emp.per_num) left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.degree on (staffs.degree_id=degree.degree_id) left join {0}.position on (staffs.pos_id=position.pos_id)" +
                "where " + (Per_num.Text.Length > 0 ? " per_num=to_number('" + Per_num.Text + "') and " : "") + (Degree.Text.Length > 0 ? " upper(degree.degree_name)=upper('" + Degree.Text + "') and " : "") +
                (Subdiv.Text.Length> 0 ? " subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id="+ShtatFilter.Subdiv_id+" connect by prior subdiv_id=parent_id) and ": "") + (Position.Text.Length > 0 ? " upper(position.pos_name)like upper('%" + Position.Text + "%') and " : "") +
                (Per_num.Text.Length > 0 ? " transfer.per_num=" + Per_num.Text+ " and " : ""), DataSourceScheme.SchemeName);
            filter = filter.Substring(0, filter.Length - 4);
            OracleDataAdapter adapter = new OracleDataAdapter(filter+" order by pos_name", connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            DG.DataSource = table;
            DG.Columns["staffs_id"].Visible = false;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (DG.SelectedRows.Count > 0)
            {
                staffs_id = DG.SelectedRows[0].Cells["staffs_id"].Value.ToString();
                staff_pos_name = DG.SelectedRows[0].Cells["Профессия"].Value.ToString();
                staff_subdiv_name = DG.SelectedRows[0].Cells["Подразделение"].Value.ToString();
                per_num = DG.SelectedRows[0].Cells["Табельный номер"].Value.ToString();
                last_name = string.Format("{0} {1} {2}({3})", DG.SelectedRows[0].Cells["Фамилия"].Value.ToString(), DG.SelectedRows[0].Cells["Имя"].Value.ToString(), DG.SelectedRows[0].Cells["Отчество"].Value.ToString(), DG.SelectedRows[0].Cells["Табельный номер"].Value.ToString());
                this.Close();
            }
            else staffs_id = "0";
           
        }

        private void FindStaffEd_Load(object sender, EventArgs e)
        {
            Subdiv.Text = "";
            Degree.Items.Clear();
            DataTable table = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select degree_Name from {0}.Degree", DataSourceScheme.SchemeName), connect);
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
                Degree.Items.Add(table.Rows[i][0]);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            staffs_id = "-1";
            this.Close();            
        }
       

        

        private void code_subdiv_TextChanged(object sender, EventArgs e)
        {
            /*if (code_subdiv.Focused)
            {
                OracleDataReader r = new OracleCommand(string.Format("select subdiv_id,subdiv_name from {0}.subdiv where code_subdiv='{1}' and sub_actual_sign=1", DataSourceScheme.SchemeName, code_subdiv.Text.Trim()), connect).ExecuteReader();
                if (r.Read())
                {
                    ShtatFilter.Subdiv_id = (decimal?)r["subdiv_id"];
                    Subdiv.Text = r["subdiv_name"].ToString();
                }
                else
                {
                    ShtatFilter.Subdiv_id = null;
                    Subdiv.Text = "";
                }
            }*/
        }

       
       
    }
}
