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
    public partial class ViewVacancy : Form
    {
        string subdiv_id;
        public SelectSubdivFromTree frm;
        public ViewVacancy()
        {
            subdiv_id = "0";
            InitializeComponent();
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            OracleDataAdapter adapter = new OracleDataAdapter("select * from dual", Connect.CurConnect);
            string txt_where=" where ";
            adapter.SelectCommand.CommandText = string.Format("select code_pos \"Код профессии\",pos_name \"Профессия\",subdiv_name \"Подразделение\",date_end_vacant \"Дата окончания вакансии\", date_begin_staff \"Дата начала действия шт.ед.\",date_end_staff  \"Дата окончания действия шт.ед.\" from {0}.staffs      left join {0}.position on (staffs.pos_id=position.pos_id) " +
                                                        " left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id)",DataSourceScheme.SchemeName);
            txt_where += (subdiv_id == "0" ? "" : string.Format(" subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id={1} connect by prior subdiv_id=parent_id) and ",DataSourceScheme.SchemeName,subdiv_id));
            txt_where += (pos_name.Text.Length > 0 ? string.Format(" upper(pos_name) like upper('%{0}%') and ", pos_name.Text) : "");
            if (txt_where.Length > 7)
                adapter.SelectCommand.CommandText += txt_where + "   staffs.vacant_sign=1 and ( date_end_vacant is null or date_end_vacant>trunc(sysdate)) and (date_end_staff is null or date_end_staff>trunc(sysdate)) ";
            else adapter.SelectCommand.CommandText += " where  staffs.vacant_sign=1 and ( date_end_vacant is null or date_end_vacant>trunc(sysdate)) and (date_end_staff is null or date_end_staff>trunc(sysdate)) ";
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }       

        private void ViewVacancy_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain.UnCheckButtonShtat("btViewVacant");
        }
    }
}
