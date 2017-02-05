using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace Kadr
{
    public partial class View_All_Transfer : Form
    {
        DataTable _dtTransfer = new DataTable();
        OracleDataAdapter _daTransfer = new OracleDataAdapter();
        public View_All_Transfer()
        {
            InitializeComponent();
            _daTransfer.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Ras/Select_Find_All_Transfer.sql"),
                Connect.Schema), Connect.CurConnect);
            _daTransfer.SelectCommand.BindByName = true;
            _daTransfer.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = "0";
            _daTransfer.SelectCommand.Parameters.Add("p_EMP_LAST_NAME", OracleDbType.Varchar2);
            _daTransfer.SelectCommand.Parameters.Add("p_EMP_FIRST_NAME", OracleDbType.Varchar2);
            _daTransfer.SelectCommand.Parameters.Add("p_EMP_MIDDLE_NAME", OracleDbType.Varchar2);
            _daTransfer.Fill(_dtTransfer);
            dgView_Transfer.AutoGenerateColumns = false;
            dgView_Transfer.DataSource = _dtTransfer;

        }

        private void btFindEmp_Click(object sender, EventArgs e)
        {
            if (tB_per_num.Text.Trim() + tB_emp_last_name.Text.Trim() + tB_emp_first_name.Text.Trim() + tB_emp_middle_name.Text.Trim() != "")
            {
                _daTransfer.SelectCommand.Parameters["p_PER_NUM"].Value = String.IsNullOrEmpty(tB_per_num.Text.Trim()) ? null : tB_per_num.Text.Trim().PadLeft(5, '0');
                _daTransfer.SelectCommand.Parameters["p_EMP_LAST_NAME"].Value = tB_emp_last_name.Text.Trim();
                _daTransfer.SelectCommand.Parameters["p_EMP_FIRST_NAME"].Value = tB_emp_first_name.Text.Trim();
                _daTransfer.SelectCommand.Parameters["p_EMP_MIDDLE_NAME"].Value = tB_emp_middle_name.Text.Trim();
                _dtTransfer.Clear();
                _daTransfer.Fill(_dtTransfer);
            }
        }
    }
}
