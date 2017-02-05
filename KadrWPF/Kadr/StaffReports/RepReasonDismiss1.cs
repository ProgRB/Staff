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
    public partial class RepReasonDismiss1 : Form
    {
        public RepReasonDismiss1()
        {
            InitializeComponent();
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }
        }

        private void RepCsDsmss1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Library.TestingInput(maskedTextBox1, maskedTextBox2) == true)
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("RepReasonDismiss1.sql"), maskedTextBox1.Text, 
                    maskedTextBox2.Text, Connect.Schema);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    Excel.Print("RepReasonDismiss1.xlt", "A5", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Увольнение с {0} по {1}", maskedTextBox1.Text, maskedTextBox2.Text)) });
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
    }
}
