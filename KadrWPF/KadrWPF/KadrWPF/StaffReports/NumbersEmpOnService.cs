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
using Oracle.DataAccess.Client;

namespace StaffReports
{
    public partial class NumbersEmpOnService : Form
    {
        DataTable dtService;
        public NumbersEmpOnService()
        {
            InitializeComponent();
            dtService = new DataTable();
            new OracleDataAdapter(string.Format(
                "select 0 FL, SERVICE_ID, SERVICE_NAME from {0}.SERVICE ORDER BY SERVICE_NAME", Connect.Schema),
                Connect.CurConnect).Fill(dtService);
            dgService.AutoGenerateColumns = false;
            dgService.DataSource = dtService;
            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name = "fl";
            c.HeaderText = "Выбрать";
            c.DataPropertyName = "fl";
            dgService.Columns.Add(c);
            dgService.Columns.Add(new MDataGridViewTextBoxColumn("SERVICE_NAME", "Наименование службы", "SERVICE_NAME", true));            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (deBegin_Period.Date != null && deEnd_Period.Date != null)
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("SelectNumbersEmpOnService.sql"), Connect.Schema);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("P_TABLE_SERVICE", OracleDbType.Array).UdtTypeName =
                    Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
                dtService.DefaultView.RowFilter = "FL = 1";
                adapter.SelectCommand.Parameters["P_TABLE_SERVICE"].Value =
                    dtService.DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("SERVICE_ID")).ToArray();
                dtService.DefaultView.RowFilter = "";
                adapter.SelectCommand.Parameters.Add("P_DATE_BEGIN", OracleDbType.Date).Value = deBegin_Period.Date;
                adapter.SelectCommand.Parameters.Add("P_DATE_END", OracleDbType.Date).Value = deEnd_Period.Date;
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    Excel.Print("RepNumbersEmpOnService.xlt", "A3", new DataTable[] { table }, null, false);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                this.Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Ошибки в датах", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
