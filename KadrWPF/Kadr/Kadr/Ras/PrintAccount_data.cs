using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;


namespace Kadr
{
    public partial class PrintAccount_data : Form
    {
        SUBDIV_seq subdiv;
        DEGREE_seq degree;
        DataTable dtReport;
        OracleDataAdapter adReport;
        public PrintAccount_data()
        {
            InitializeComponent();
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format(@"where nvl(PARENT_ID,0) = 0 and TYPE_SUBDIV_ID != 6 and
                subdiv_id in (select SUBDIV_ID FROM {0}.SUBDIV
                    start with subdiv_id in (
                        select subdiv_id from {0}.access_subdiv 
                        where upper(user_name) = ora_login_user and upper(app_name) = 'ACCOUNT')
                    connect by prior subdiv_id = parent_id) order by SUBDIV_NAME",
                DataSourceScheme.SchemeName, Connect.UserId.ToUpper()));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME));
            
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), 
                new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbDegree_Name.AddBindingSource(DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            cbDegree_Name.SelectedItem = null;
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);

            adReport = new OracleDataAdapter("", Connect.CurConnect);
            adReport.SelectCommand.BindByName = true;
            adReport.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Ras/PrintAccount_Data.sql"), Connect.Schema);
            adReport.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            adReport.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal);
            adReport.SelectCommand.Parameters.Add("p_classific", OracleDbType.Decimal);
            adReport.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            adReport.SelectCommand.Parameters.Add("p_LAST_NAME", OracleDbType.Varchar2);
            adReport.SelectCommand.Parameters.Add("p_FIRST_NAME", OracleDbType.Varchar2);
            adReport.SelectCommand.Parameters.Add("p_MIDDLE_NAME", OracleDbType.Varchar2);            
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Изменение индеска списка подразделений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Проверка введенного шифра категории и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Degree_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Degree, cbDegree_Name, 2, Connect.CurConnect, e, DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(), tbCode_Degree.Text);
        }

        /// <summary>
        /// Событие изменения индекса категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbDegree_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDegree_Name.SelectedValue != null)
            {
                tbCode_Degree.Text = Library.CodeBySelectedValue(Connect.CurConnect, DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(),
                    Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), cbDegree_Name.SelectedValue.ToString());
            }
        }

        private void btPreview_Click(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue == null)
            {
                MessageBox.Show("Необходимо выбрать подразделение!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbSubdiv_Name.Focus();
                return;
            }
            adReport.SelectCommand.Parameters["p_subdiv_id"].Value = cbSubdiv_Name.SelectedValue;
            adReport.SelectCommand.Parameters["p_degree_id"].Value = cbDegree_Name.SelectedValue;
            adReport.SelectCommand.Parameters["p_per_num"].Value = tbPer_Num.Text;
            /* Сначала обнуляем значение параметра p_classific, а потом в зависимости от того, 
              занесли ли данные, задаем значение параметра*/
            adReport.SelectCommand.Parameters["p_classific"].Value = null;
            if (mbClassific.Text.ToString() != "")
            {
                adReport.SelectCommand.Parameters["p_classific"].Value = Convert.ToInt32(mbClassific.Text);
            }
            adReport.SelectCommand.Parameters["p_LAST_NAME"].Value = tbEmp_Last_Name.Text;
            adReport.SelectCommand.Parameters["p_FIRST_NAME"].Value = tbEmp_First_Name.Text;
            adReport.SelectCommand.Parameters["p_MIDDLE_NAME"].Value = tbEmp_Middle_Name.Text;
            dtReport = new DataTable();
            adReport.Fill(dtReport);
            Excel.PrintWithBorder(true, "PrintAccount_Data.xlt", "A5", new DataTable[] { dtReport}, 
                new ExcelParameter[] {new ExcelParameter("K1", tbCode_Subdiv.Text)});
        }
    }
}
