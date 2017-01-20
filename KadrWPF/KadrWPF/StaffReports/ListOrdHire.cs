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
    public partial class ListOrdHire : Form
    {
        public ListOrdHire()
        {
            InitializeComponent();
            foreach (Control control in this.groupBox1.Controls)
            {
                if ((control is MaskedTextBox) && (control.Name != "maskedTextBox3") && (control.Name != "maskedTextBox4"))
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((Library.TestingInput(maskedTextBox1, maskedTextBox2) == true) && (maskedTextBox3.Text != "") && (maskedTextBox4.Text != ""))
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("ListOrdHire.sql"), maskedTextBox1.Text, maskedTextBox2.Text, 
                    Connect.Schema, maskedTextBox3.Text, maskedTextBox4.Text);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                //Excel.Print("ListOrdHire.xlt", "A4", new DataTable[] { table });
                if (table.Rows.Count != 0)
                {
                    Excel.Print("ListOrdHire.xlt", "A4", new DataTable[] { table/*, table2 */}, new ExcelParameter[] { /*new ExcelParameter("A1", string.Format("дата {0}.{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)),*/ new ExcelParameter("A2", string.Format("Опись приказов о приёме", maskedTextBox1.Text, maskedTextBox2.Text)) });
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
