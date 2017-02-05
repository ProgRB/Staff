using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class ReportForEconDev : Form
    {
        OracleDataAdapter odaRepForEconDev;
        DataTable dtRepForEconDev;
        int subdiv_id;
        string nameQuery, nameExcel;        
        public ReportForEconDev(string _nameQuery, string _nameExcel)
        {
            InitializeComponent();
            subdiv_id = 0;
            nameQuery = _nameQuery;
            nameExcel = _nameExcel;
            ssFilterForReport.subdiv_id = subdiv_id;            
            dtpBeginPeriod.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEndPeriod.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            odaRepForEconDev = new OracleDataAdapter("", Connect.CurConnect);
            odaRepForEconDev.SelectCommand.BindByName = true;
            dtRepForEconDev = new DataTable();
        }

        private void btOrderTruancy_Click(object sender, EventArgs e)
        {
            dtRepForEconDev.Clear();
            if (dtpEndPeriod.Value < dtpBeginPeriod.Value)
            {
                MessageBox.Show("Вы ввели неверные даты!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            OracleCommand ocCom = new OracleCommand("", Connect.CurConnect);
            ocCom.BindByName = true;
            ocCom.CommandText = string.Format("delete from {0}.PN_TMP where USER_NAME = :p_user_name",
                Connect.Schema);
            ocCom.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            ocCom.ExecuteNonQuery();
            ocCom = new OracleCommand("", Connect.CurConnect);
            ocCom.BindByName = true;
            ocCom.CommandText = string.Format(Queries.GetQuery("Table/InsertIntoPn_tmp.sql"),
                Connect.Schema);
            ocCom.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            ocCom.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = ssFilterForReport.subdiv_id;
            ocCom.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                dtpBeginPeriod.Value;
            ocCom.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                dtpEndPeriod.Value.AddDays(1).AddSeconds(-1); ;
            ocCom.ExecuteNonQuery();
            odaRepForEconDev.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/"+nameQuery), Connect.Schema);
            odaRepForEconDev.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = 
                dtpBeginPeriod.Value;
            odaRepForEconDev.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = 
                dtpEndPeriod.Value.AddDays(1).AddSeconds(-1);;
            odaRepForEconDev.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();      
            odaRepForEconDev.Fill(dtRepForEconDev);
            Connect.Rollback();
            if (dtRepForEconDev.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "за период с " + 
                        dtpBeginPeriod.Value.ToShortDateString() + " по " + 
                        dtpEndPeriod.Value.ToShortDateString())};
                Excel.PrintWithBorder(true, nameExcel, "A4", new DataTable[] { dtRepForEconDev },
                    excelParameters, null, true);                
            }
            else
            {
                MessageBox.Show("В подразделении нет выбранных документов.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
