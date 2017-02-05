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

namespace Kadr.Shtat
{
    public partial class SelectInterval : Form
    {
        public SelectSubdivFromTree frm;
        public bool _withdate;
        public SelectInterval(bool ReportWithFIO) 
        {
            _withdate = ReportWithFIO;
            InitializeComponent();            
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }
            subdivSelector1.subdiv_id = ShtatFilter.Subdiv_id;
        }
        public SelectInterval(bool ReportWithFIO,string _Text)
        {
            _withdate = ReportWithFIO;
            InitializeComponent();
            this.Text=_Text;
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }
            subdivSelector1.subdiv_id = ShtatFilter.Subdiv_id;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (subdivSelector1.subdiv_id==null)
            {
                MessageBox.Show("Подразделение не выбрано", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Library.TestingInput(mtBegDate, mtEndDate))
            {
                this.Cursor = Cursors.WaitCursor;
                string  sql = string.Format(Queries.GetQuery(!_withdate ? "new/ChangeStaffTable.sql" : "new/ChangeStaffTableWithFIO.sql"),DataSourceScheme.SchemeName,   subdivSelector1.subdiv_id, Degree.Text,mtBegDate.Text, mtEndDate.Text);

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 4)
                {
                    Excel.PrintWithBorder((_withdate ? "ChangeStaffsTableWithFIO.xlt" : "ChangeStaffsTable.xlt"), "A7", new DataTable[] { table }, new ExcelParameter[] {
                      new ExcelParameter("A2",string.Format("на категорию '{0}'",Degree.Text),null),
                      new ExcelParameter("A3",string.Format("по подразделению {0} c {1} по {2}", subdivSelector1.CodeSubdiv, mtBegDate.Text, mtEndDate.Text,Degree.Text))
                      //,new ExcelParameter("B" + (table.Rows.Count + 10).ToString(), "Начальник ТЭБ ЭУ_____________________") 
                    });
                }
                else
                {

                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;
            }
        }

        private void SelectInterval_Load(object sender, EventArgs e)
        {
            Degree.Items.Clear();
            OracleDataReader r = new OracleCommand(string.Format("select Degree_name from {0}.degree", DataSourceScheme.SchemeName), Connect.CurConnect).ExecuteReader();
            while (r.Read())
                Degree.Items.Add(r[0].ToString());
            Degree.SelectedIndex = 0;
        }

        private void SelectInterval_Activated(object sender, EventArgs e)
        {
            mtBegDate.Focus();
        }

    }
}
