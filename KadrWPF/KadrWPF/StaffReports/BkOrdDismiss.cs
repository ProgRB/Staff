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
    public partial class BkOrdDismiss : Form
    {
        public BkOrdDismiss()
        {
            InitializeComponent();
            mbYear.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("BkOrdDismiss.sql"), 
                Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = 
                new DateTime(Convert.ToInt32(mbYear.Text), Convert.ToInt32(nudMonth.Value), 1);
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                new DateTime(Convert.ToInt32(mbYear.Text), Convert.ToInt32(nudMonth.Value), 1).AddMonths(1).AddSeconds(-1);
            adapter.SelectCommand.Parameters.Add("p_num", OracleDbType.Decimal).Value =
                maskedTextBox2.Text == "" ? 0 : Convert.ToInt32(maskedTextBox2.Text);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count != 0)
            {
                Excel.Print("BkOrdDismiss.xlt", "A3", new DataTable[] { table }, 
                    new ExcelParameter[] { new ExcelParameter("A1", string.Format("Книга приказов об уволенных                                                           Дата {0}.{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) },
                    false);
            }
            else
            {

                MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
                this.Cursor = Cursors.Default;
                this.Close();
        }

        private void BkOrdDismiss_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
