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
    public partial class ListVeteransStructSubdiv : Form
    {
        public ListVeteransStructSubdiv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("VeteransBossS.sql"), maskedTextBox1.Text, Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();            
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransSpecS.sql"), maskedTextBox1.Text, 
                Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransOtherS.sql"), maskedTextBox1.Text, 
                Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransJobS.sql"), maskedTextBox1.Text, 
                Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransTotalSubdivS.sql"), 
                maskedTextBox1.Text, Connect.Schema);
            adapter.Fill(table);
            //decimal rez = table.AsEnumerable().Select(s => s.Field<decimal>("Rtr")).Sum();
            if (table.Rows.Count != 0)
            {
                Excel.Print("ListVeteransStructSubdiv.xlt", "B4", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Численность, состав и движение ветеранов труда ОАО \"УУАЗ\" ")),
            new ExcelParameter("A3", string.Format("Подразделение {0}", maskedTextBox1.Text)),
            new ExcelParameter("A4", string.Format("Руководители")),
            new ExcelParameter("A5", string.Format("Специалисты")),
            new ExcelParameter("A6", string.Format("Др. служащие")),
            new ExcelParameter("A7", string.Format("Рабочие")),
            new ExcelParameter("A8", string.Format("Итого")) });
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

        private void RtrSub_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
