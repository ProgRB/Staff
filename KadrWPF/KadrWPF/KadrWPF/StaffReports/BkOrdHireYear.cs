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
    public partial class BkOrdHireYear : Form
    {
        public BkOrdHireYear()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void BkOrdAcss_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("BkOrdHireYear.sql"), maskedTextBox1.Text, maskedTextBox2.Text, 
                Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count != 0)
            {
                Excel.Print("BkOrdHire.xlt", "A3", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Книга приказов о приёме на работу              Дата {0}.{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) });
            }
            else
            {

                MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
                this.Cursor = Cursors.Default;
                this.Close();
        }
    }
}
