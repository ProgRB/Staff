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
    public partial class ReportDocsOnPay_Type : Form
    {
        OracleDataTable dtDoc_List;
        OracleDataAdapter odaDocsOnPay_Type;
        DataTable dtDocsOnPay_Type;
        int subdiv_id;
        string code_subdiv;
        public ReportDocsOnPay_Type(DateTime _dateBegin, DateTime _dateEnd, 
            int _subdiv_id, string _code_subdiv)
        {
            InitializeComponent();
            subdiv_id = _subdiv_id;
            code_subdiv = _code_subdiv;
            ssFilterForReport.subdiv_id = subdiv_id;
            dtDoc_List = new OracleDataTable("", Connect.CurConnect);
            dtDoc_List.SelectCommand.CommandText = string.Format(
                "select D.DOC_LIST_ID, D.DOC_NAME, D.PAY_TYPE_ID from {0}.DOC_LIST D " + 
                "order by D.DOC_NAME", Connect.Schema);
            dtDoc_List.Fill();
            cbDoc_List_Name.DataSource = dtDoc_List;
            cbDoc_List_Name.DisplayMember = "DOC_NAME";
            cbDoc_List_Name.ValueMember = "DOC_LIST_ID";
            dtpBeginPeriod.Value = _dateBegin;
            dtpEndPeriod.Value = _dateEnd;
            odaDocsOnPay_Type = new OracleDataAdapter("", Connect.CurConnect);
            odaDocsOnPay_Type.SelectCommand.BindByName = true;
            odaDocsOnPay_Type.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectDocsOnPayType.sql"), Connect.Schema);
            odaDocsOnPay_Type.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            odaDocsOnPay_Type.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            odaDocsOnPay_Type.SelectCommand.Parameters.Add("p_doc_list_id", OracleDbType.Decimal);
            odaDocsOnPay_Type.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            dtDocsOnPay_Type = new DataTable();
        }

        private void btOrderTruancy_Click(object sender, EventArgs e)
        {
            dtDocsOnPay_Type.Clear();
            if (cbDoc_List_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали документ!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbDoc_List_Name.Focus();
                return;
            }
            if (dtpEndPeriod.Value < dtpBeginPeriod.Value)
            {
                MessageBox.Show("Вы ввели неверные даты!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            odaDocsOnPay_Type.SelectCommand.Parameters["p_date_begin"].Value = dtpBeginPeriod.Value;
            odaDocsOnPay_Type.SelectCommand.Parameters["p_date_end"].Value = 
                dtpEndPeriod.Value.AddDays(1).AddSeconds(-1);
            odaDocsOnPay_Type.SelectCommand.Parameters["p_doc_list_id"].Value = cbDoc_List_Name.SelectedValue;
            odaDocsOnPay_Type.SelectCommand.Parameters["p_subdiv_id"].Value = ssFilterForReport.subdiv_id;
            odaDocsOnPay_Type.Fill(dtDocsOnPay_Type);
            if (dtDocsOnPay_Type.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "по документу \"" + cbDoc_List_Name.Text + "\""),
                    new ExcelParameter("A3", "за период с " + 
                        dtpBeginPeriod.Value.ToShortDateString() + " по " + 
                        dtpEndPeriod.Value.ToShortDateString())};
                if (cbDoc_List_Name.Text == "Работа за территорией предприятия")
                {
                    Excel.PrintWithBorder(true, "ReportDocsByWorkOut.xlt", "A5", new DataTable[] { dtDocsOnPay_Type },
                        excelParameters);
                }
                else
                {
                    dtDocsOnPay_Type.Columns.Remove("DOC_LOCATION");
                    Excel.PrintWithBorder(true, "ReportDocsByPay_Type.xlt", "A5", new DataTable[] { dtDocsOnPay_Type },
                        excelParameters);
                }
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
