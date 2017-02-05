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
    public partial class EndOfContr : Form
    {
        public EndOfContr()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("EndOfContr.sql"), numericUpDown1.Value, maskedTextBox1.Text, 
                Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count != 0)
            {
            StringBuilder mon = new StringBuilder();
            switch (Convert.ToInt32(numericUpDown1.Value)) {
                case 1: 
                    mon.Append("Январе"); 
                    break;
                case 2: mon.Append("Феврале");
                    break;
                case 3: mon.Append("Марте");
                    break;
                case 4: mon.Append("Апреле");
                    break;
                case 5: mon.Append("Мае");
                    break;
                case 6: mon.Append("Июне");
                    break;
                case 7: mon.Append("Июле");
                    break;
                case 8: mon.Append("Августе");
                    break;
                case 9: mon.Append("Сентябре");
                    break;
                case 10: mon.Append("Октябре");
                    break;
                case 11: mon.Append("Ноябре");
                    break;
                case 12: mon.Append("Декабре");
                    break;
            }
            Excel.Print("EndOfContr.xlt", "A3", new DataTable[] { table}, 
                new ExcelParameter[] { new ExcelParameter("A1", string.Format("Список работников с окончанием договора в {0} {1}г.", mon, maskedTextBox1.Text)) },
                false);
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
    }
}
