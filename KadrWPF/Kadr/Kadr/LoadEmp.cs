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


namespace Kadr
{
    public partial class LoadEmp : Form
    {
        StringBuilder str_find = new StringBuilder();
        public string per_num;
        public StringBuilder sort = new StringBuilder();        
        public LoadEmp()
        {
            InitializeComponent();            
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (Code_Subdiv.Text.Trim() != "")
            {
                sort.Append(Code_Subdiv.Name);
                if (Per_num.Text.Trim() == "")
                {
                    sort.Append("," + Per_num.Name);
                }
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.TRANSFER where {0}.TRANSFER.TRANSFER_ID = CUR_EMP.TRANSFER_ID and " +
                    "{0}.TRANSFER.{1} = (select {1} from {0}.SUBDIV where {2} = '{3}' and SUB_ACTUAL_SIGN = 1))",
                    Connect.Schema, TRANSFER_seq.ColumnsName.SUBDIV_ID, SUBDIV_seq.ColumnsName.CODE_SUBDIV,
                    Code_Subdiv.Text.Trim().PadLeft(3, '0'), str_find.Length != 0 ? " and" : ""));
            }
            if (Per_num.Text.Trim() != "")
            {
                str_find.Append(string.Format("{2} CUR_EMP.{0} = '{1}'", EMP_seq.ColumnsName.PER_NUM,
                    Per_num.Text.Trim().PadLeft(5, '0'), str_find.Length != 0 ? " and" : ""));
                if (sort.Length != 0)
                {
                    sort.Append("," + Per_num.Name);
                }
                else
                {
                    sort.Append(Per_num.Name);
                }
            }
            if (Emp_last_name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(CUR_EMP.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(), Emp_last_name.Text.Trim()));
                if (sort.Length != 0)
                {
                    sort.Append(string.Format(",{0},{1},{2}", EMP_seq.ColumnsName.EMP_LAST_NAME,
                        EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME));
                }
                else
                {
                    sort.Append(string.Format("{0},{1},{2}", EMP_seq.ColumnsName.EMP_LAST_NAME,
                        EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME));
                }
            }
            if (sort.Length != 0)
            {
                sort.Insert(0, " order by ");
            }
            else
            {
                sort.Append(" order by CODE_SUBDIV, PER_NUM");
            }
            if (str_find.Length != 0)
                str_find.Insert(0, " where ");
            string sql = "";
            if (FormMain.flagArchive)
            {
                //sql = string.Format(Queries.GetQuery("SelectFindArchive.sql"), Staff.DataSourceScheme.SchemeName, str_find.ToString() + sort);
                sql = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"), 
                    Connect.Schema, str_find.ToString() + sort);
            }
            else
            {
                //sql = string.Format(Queries.GetQuery("SelectFind.sql"), Staff.DataSourceScheme.SchemeName, str_find.ToString() + sort);
                sql = string.Format(Queries.GetQuery("SelectListEmp.sql"), Connect.Schema, 
                    str_find.ToString() + sort);
            }
            OracleDataTable oracleTable = new OracleDataTable(sql, Connect.CurConnect);
            oracleTable.Fill();
            if (oracleTable.Rows.Count == 0)
            {
                MessageBox.Show("В базе данных не найдена введенная информация!", "АСУ \"Кадры\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                per_num = oracleTable.Rows[0][1].ToString();
            }
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
