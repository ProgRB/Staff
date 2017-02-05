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
    public partial class ListVeteranSubdiv : Form
    {
        public ListVeteranSubdiv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("ListVeteranSubdiv.sql"), maskedTextBox1.Text, Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            //int count = table.Rows.Count;
            if (table.Rows.Count != 0)
            {
                Excel.Print("ListVeteranPlant.xlt", "A3", new DataTable[] { table });
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
