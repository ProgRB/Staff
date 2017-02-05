using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;


namespace StaffReports
{
    public partial class PrintNarush : Form
    {
        public PrintNarush()
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

        private void button1_Click(object sender, EventArgs e)
        {
            //if (Library.TestingInput(maskedTextBox1, maskedTextBox2) == true)
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    string sql = string.Format(Queries.GetQuery("RepCountDismiss.sql"), maskedTextBox1.Text, maskedTextBox2.Text, schema);
            //    OracleDataAdapter adapter = new OracleDataAdapter(sql, connection);
            //    DataTable table = new DataTable();
            //    adapter.Fill(table);
            //    if (table.Rows.Count != 0)
            //    {
            //        Excel.Print("RepCountDismiss.xlt", "A4", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A4", string.Format("Отчёт о количестве нарушителей с {0} по {1}", maskedTextBox1.Text, maskedTextBox2.Text)) });
            //    }
            //    else
            //    {
            //        MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }
            //    this.Cursor = Cursors.Default;
            //    this.Close();
            //}            
        }
    }
}

