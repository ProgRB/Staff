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
    public partial class ListRetirerSubdiv : Form
    {
        public ListRetirerSubdiv()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //string sql = string.Format(Queries.GetQuery("ListRetirerSubdiv.sql"), maskedTextBox1.Text, Connect.Schema);
            //OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            // Начиная с 01.04.2016 начинаю использовать один запрос для отчета по заводу и по подразделению
            string sql = string.Format(Queries.GetQuery("ListRetirerPlant.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            adapter.SelectCommand.Parameters.Add("p_CODE_SUBDIV", OracleDbType.Varchar2).Value =
                maskedTextBox1.Text.Trim().PadLeft(3,'0');
            DataTable table = new DataTable();
            adapter.Fill(table);
            //int count = table.Rows.Count;
            if (table.Rows.Count != 0)
            {
                Excel.Print("ListRetirerPlant.xlt", "A3", new DataTable[] { table });
            }
            else
            {

                MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
                this.Cursor = Cursors.Default;
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             
        }

        private void RetirSub_Load(object sender, EventArgs e)
        {

        }
    }
}
