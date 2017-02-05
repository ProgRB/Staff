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
namespace StaffReports
{
    public partial class ListInvalidSubdiv : Form
    {
        public ListInvalidSubdiv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dateRep = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateRep, "dd MMMM yyyy") == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("SelectInvalidSub.sql"), Connect.Schema);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_dateRep", OracleDbType.Date).Value = dateRep;
                adapter.SelectCommand.Parameters.Add("p_CODE_SUBDIV", OracleDbType.Varchar2).Value = maskedTextBox1.Text.Trim().PadLeft(3, '0');
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    ExcelParameter[] param = new ExcelParameter[] { new ExcelParameter("A1", "Список инвалидов подразделения " +
                        maskedTextBox1.Text.Trim().PadLeft(3, '0'), null) };
                    //Excel.Print("ListInvalidSub.xlt", "A3", new DataTable[] { table }, param);
                    Excel.Print("ListInvalid.xlt", "A3", new DataTable[] { table }, param);
                }
                else
                {

                    MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
             
        }

        private void RetirSub_Load(object sender, EventArgs e)
        {

        }
    }
}
