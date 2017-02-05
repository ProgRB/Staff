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
using Staff;
namespace StaffReports
{
    public partial class ListDismissJobman : Form
    {
        public ListDismissJobman()
        {
            InitializeComponent();
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is MaskedTextBox && control.Name != "mbSubdiv")
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Library.TestingInput(maskedTextBox1, maskedTextBox2) == true)
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("ListDismissJobman.sql"), maskedTextBox1.Text, 
                    maskedTextBox2.Text, mbSubdiv.Text, Connect.Schema);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                //EMP_seq emp = new EMP_seq(_connection);
                //emp.Fill(" where per_num = 13021");
                //EMP_obj Michael = emp[0];
                
                if (table.Rows.Count != 0)
                {
                    Excel.Print("ListDismissJobman.xlt", "A4", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Список уволенных с {0} по {1} (сдельщики)", maskedTextBox1.Text, maskedTextBox2.Text)), new ExcelParameter("A2", string.Format("Подразделение {0}", mbSubdiv.Text)) });
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
