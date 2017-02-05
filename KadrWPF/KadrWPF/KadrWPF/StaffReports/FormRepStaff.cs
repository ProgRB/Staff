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
using WExcel = Microsoft.Office.Interop.Excel;

namespace StaffReports
{
    public partial class FormRepStaff : Form
    {
        public FormRepStaff()
        {
            InitializeComponent();
        }

        private void FormRepStaff_Load(object sender, EventArgs e)
        {
            OracleCommand command = new OracleCommand(string.Format("SELECT code_subdiv, subdiv_name FROM {0}.Subdiv S WHERE S.SUB_ACTUAL_SIGN = 1 and nvl(S.PARENT_ID,0) = 0 and S.TYPE_SUBDIV_ID in (1,2,3,4) ORDER BY S.CODE_SUBDIV", Connect.Schema), Connect.CurConnect);
            command.BindByName = true;
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //string[] mas = new string[] { reader["code_subdiv"].ToString(), reader["subdiv_name"].ToString() };
                    listViewSubd.Items.Add(new ListViewItem(new string[] { reader["code_subdiv"].ToString(), reader["subdiv_name"].ToString() }));
                    //listViewSubd.Items.Add(new ListViewItem(reader["code_subdiv"].ToString(), reader["subdiv_name"].ToString()));                    
                }                
            
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listViewSubd.CheckedItems.Count > 0)
            {
                string listSubd = "";
                for (int i = 0; i < listViewSubd.Items.Count; i++)
                {
                    if (listViewSubd.Items[i].Checked)
                    {
                        listSubd += "'" + listViewSubd.Items[i].Text + "',";
                    }
                }
                listSubd = listSubd.Substring(0, listSubd.Length - 1);


                string sql = string.Format(Queries.GetQuery("RepStaffSubdiv.sql"), listSubd, Connect.Schema);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    Excel.Print("RepStaffSubdiv.xlt", "A5", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Отчёт по численности сотрудников по подразделениям на {0}", DateTime.Now)) });// .{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) });
                }
                else
                {
                    MessageBox.Show("Данные по текущему запросу отсутствуют", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одного подразделения!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Cursor = Cursors.Default;
                return;
            }


            this.Cursor = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btNotCheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewSubd.Items.Count; i++)
                listViewSubd.Items[i].Checked = false;
        }

        private void btAllCheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewSubd.Items.Count; i++)
                listViewSubd.Items[i].Checked = true;
        }
    }
}
