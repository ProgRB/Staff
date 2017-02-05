using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class EditDateSubdivFT : Form
    {
        int subdiv_id;
        public EditDateSubdivFT(int _subdiv_id, object _date_advance, object _date_salary, int _sign)
        {
            InitializeComponent();
            subdiv_id = _subdiv_id;
            if (_date_advance != DBNull.Value)
            {
                deDate_Advance.Date = Convert.ToDateTime(_date_advance);
            }
            if (_date_salary != DBNull.Value)
            {
                deDate_Salary.Date = Convert.ToDateTime(_date_salary);
            }
            chSign_Processing.Checked = Convert.ToBoolean(_sign);
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(Queries.GetQuery("Table/UpdateSubdivFT.sql"), Connect.Schema);
            com.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            com.Parameters.Add("p_date_advance", OracleDbType.Date);
            com.Parameters.Add("p_date_salary", OracleDbType.Date);
            com.Parameters.Add("p_sign_processing", OracleDbType.Decimal);
            com.Parameters["p_SUBDIV_ID"].Value = subdiv_id;
            com.Parameters["p_date_advance"].Value = deDate_Advance.Date;
            com.Parameters["p_date_salary"].Value = deDate_Salary.Date;
            com.Parameters["p_sign_processing"].Value = Convert.ToInt32(chSign_Processing.Checked);
            com.ExecuteNonQuery();
            Connect.Commit();
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
