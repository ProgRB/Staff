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

namespace Kadr
{
    public partial class Order_Dismiss_Project : Form
    {
        BASE_DOC_seq base_doc;
        REASON_DISMISS_seq reason;
        object _transfer_id;
        public Order_Dismiss_Project(object transfer_id)
        {
            InitializeComponent();
            _transfer_id = transfer_id;
            base_doc = new BASE_DOC_seq(Connect.CurConnect);
            base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME.ToString()));
            reason = new REASON_DISMISS_seq(Connect.CurConnect);
            reason.Fill(string.Format("order by {0}", REASON_DISMISS_seq.ColumnsName.REASON_NAME.ToString()));
            cbBase_doc.AddBindingSource(BASE_DOC_seq.ColumnsName.BASE_DOC_ID.ToString(), new LinkArgument(base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            cbBase_doc.SelectedItem = null;
            cbReason_dismiss.AddBindingSource(REASON_DISMISS_seq.ColumnsName.REASON_ID.ToString(), new LinkArgument(reason, REASON_DISMISS_seq.ColumnsName.REASON_NAME));
            cbReason_dismiss.SelectedItem = null;
        }

        private void btPreview_Click(object sender, EventArgs e)
        {
            string[][] s_pos = new string[][] { };
            if (Signes.Show(0, "Dismiss_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == true)
            {
                string reason_article = "";
                string reason_order = "";
                if (cbReason_dismiss.SelectedValue != null)
                {
                    reason_article = reason.Where(i => i.REASON_ID == Convert.ToDecimal(cbReason_dismiss.SelectedValue)).FirstOrDefault().REASON_ARTICLE;
                    reason_order = reason.Where(i => i.REASON_ID == Convert.ToDecimal(cbReason_dismiss.SelectedValue)).FirstOrDefault().REASON_ORDER;
                }
                OracleCommand _ocEmp = new OracleCommand(string.Format(Queries.GetQuery("SelectEmp_Project_Order_Dismiss.sql"),
                    Connect.Schema), Connect.CurConnect);
                _ocEmp.BindByName = true;
                _ocEmp.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
                OracleDataReader _orEmp = _ocEmp.ExecuteReader();
                if (_orEmp.Read())
                {
                    string dayDismiss, monthDismiss, yearDismiss;
                    dayDismiss = monthDismiss = yearDismiss = "";
                    if (dpDate_Transfer.Value != null)
                    {
                        dayDismiss = dpDate_Transfer.Value.Day.ToString();
                        monthDismiss = Library.MyMonthName(dpDate_Transfer.Value);
                        yearDismiss = dpDate_Transfer.Value.Year.ToString();
                    }
                    List<string> slova = Kadr.Transfer.Slova(reason_order, ' ');
                    List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 65, 65);
                    CellParameter[] cellParameters = new CellParameter[]{};
                    // Отдел кадров хочет поменять местами основание и причину увольнения
                    //CellParameter[] cellParameters = new CellParameter[] { 
                    //    new CellParameter(14, 54, _orEmp["CONTR_EMP"].ToString(), null), 
                    //    new CellParameter(15, 54, (_orEmp["DATE_CONTR"] != DBNull.Value ? Convert.ToDateTime(_orEmp["DATE_CONTR"]).ToShortDateString() : ""), null), 
                    //    new CellParameter(16, 11, dayDismiss, null), new CellParameter(16, 16, monthDismiss, null), 
                    //    new CellParameter(16, 26, yearDismiss, null),            
                    //    new CellParameter(17, 56, _orEmp["PER_NUM"].ToString(), null), new CellParameter(18, 1, _orEmp["EMP_LAST_NAME"] + " " + 
                    //        _orEmp["EMP_FIRST_NAME"] + " " + _orEmp["EMP_MIDDLE_NAME"], null),
                    //    new CellParameter(20, 20, _orEmp["CODE_SUBDIV"].ToString(), null), new CellParameter(21, 1, _orEmp["SUBDIV_NAME"].ToString(), null), 
                    //    new CellParameter(23, 1, _orEmp["POS_NAME"].ToString(), null), 
                    //    new CellParameter(25, 8, _orEmp["CLASSIFIC"].ToString(), null), 
                    //    new CellParameter(26, 11, _orEmp["DEGREE_NAME"].ToString(), null), new CellParameter(26, 61, _orEmp["CODE_DEGREE"].ToString(), null), 
                    //    new CellParameter(27, 25, cbBase_doc.Text, null), 
                    //    new CellParameter(28, 1, reason_article, null), 
                    //    new CellParameter(29, 1, arrayPos[0].Trim(), null), new CellParameter(30, 1, arrayPos[1].Trim(), null), 
                    //    new CellParameter(31, 1, arrayPos[2].Trim(), null),
                    //    new CellParameter(37, 1, s_pos[0][0], null), new CellParameter(38, 48, s_pos[0][1], null)
                    //};
                    if (arrayPos[2].Trim() != "")
                    {
                        cellParameters = new CellParameter[] { 
                            new CellParameter(14, 54, _orEmp["CONTR_EMP"].ToString(), null), 
                            new CellParameter(15, 54, (_orEmp["DATE_CONTR"] != DBNull.Value ? Convert.ToDateTime(_orEmp["DATE_CONTR"]).ToShortDateString() : ""), null), 
                            new CellParameter(16, 11, dayDismiss, null), new CellParameter(16, 16, monthDismiss, null), 
                            new CellParameter(16, 26, yearDismiss, null),            
                            new CellParameter(17, 56, _orEmp["PER_NUM"].ToString(), null), new CellParameter(18, 1, _orEmp["EMP_LAST_NAME"] + " " + 
                                _orEmp["EMP_FIRST_NAME"] + " " + _orEmp["EMP_MIDDLE_NAME"], null),
                            new CellParameter(20, 20, _orEmp["CODE_SUBDIV"].ToString(), null), new CellParameter(21, 1, _orEmp["SUBDIV_NAME"].ToString(), null), 
                            new CellParameter(23, 1, _orEmp["POS_NAME"].ToString(), null), 
                            new CellParameter(25, 8, _orEmp["CLASSIFIC"].ToString(), null), 
                            new CellParameter(26, 11, _orEmp["DEGREE_NAME"].ToString(), null), new CellParameter(26, 61, _orEmp["CODE_DEGREE"].ToString(), null),                             
                            new CellParameter(28, 1, reason_article, null), 
                            new CellParameter(29, 1, arrayPos[0].Trim(), null), new CellParameter(30, 1, arrayPos[1].Trim(), null), 
                            new CellParameter(31, 1, arrayPos[2].Trim(), null),
                            new CellParameter(32, 1, cbBase_doc.Text, null), 
                            new CellParameter(38, 1, s_pos[0][0], null), new CellParameter(39, 48, s_pos[0][1], null)
                        };
                    }
                    else
                    {
                        cellParameters = new CellParameter[] { 
                            new CellParameter(14, 54, _orEmp["CONTR_EMP"].ToString(), null), 
                            new CellParameter(15, 54, (_orEmp["DATE_CONTR"] != DBNull.Value ? Convert.ToDateTime(_orEmp["DATE_CONTR"]).ToShortDateString() : ""), null), 
                            new CellParameter(16, 11, dayDismiss, null), new CellParameter(16, 16, monthDismiss, null), 
                            new CellParameter(16, 26, yearDismiss, null),            
                            new CellParameter(17, 56, _orEmp["PER_NUM"].ToString(), null), new CellParameter(18, 1, _orEmp["EMP_LAST_NAME"] + " " + 
                                _orEmp["EMP_FIRST_NAME"] + " " + _orEmp["EMP_MIDDLE_NAME"], null),
                            new CellParameter(20, 20, _orEmp["CODE_SUBDIV"].ToString(), null), new CellParameter(21, 1, _orEmp["SUBDIV_NAME"].ToString(), null), 
                            new CellParameter(23, 1, _orEmp["POS_NAME"].ToString(), null), 
                            new CellParameter(25, 8, _orEmp["CLASSIFIC"].ToString(), null), 
                            new CellParameter(26, 11, _orEmp["DEGREE_NAME"].ToString(), null), new CellParameter(26, 61, _orEmp["CODE_DEGREE"].ToString(), null),                             
                            new CellParameter(30, 1, reason_article, null), 
                            new CellParameter(31, 1, arrayPos[0].Trim(), null), new CellParameter(32, 1, arrayPos[1].Trim(), null), 
                            new CellParameter(33, 1, cbBase_doc.Text, null), 
                            new CellParameter(40, 1, s_pos[0][0], null), new CellParameter(41, 48, s_pos[0][1], null)
                        };
                    }

                    Excel.PrintR1C1(false, "Dismiss.xlt", cellParameters);
                }
                this.Close();
            }
        }
    }
}
