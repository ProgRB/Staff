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
using WExcel = Microsoft.Office.Interop.Excel;

namespace StaffReports
{
    public partial class OrderOnEncouraging : Form
    {
        DataTable _dtEmp = new DataTable(), _dtEmpForOrder = new DataTable();
        OracleDataAdapter _daEmp = new OracleDataAdapter();
        string _filter_Subdiv, _filter_Per_Num, _filter_FIO;
        List<DataRow> rowsPay;
        public OrderOnEncouraging()
        {
            InitializeComponent();
            deDate_Filter.Date = DateTime.Today;
            //dgEmp.AutoGenerateColumns = false;
            //dgEmpForOrder.AutoGenerateColumns = false;

            _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("SelectEmpForOrderOnEncouraging.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp.SelectCommand.BindByName = true;
            _daEmp.SelectCommand.Parameters.Add("p_DATE", OracleDbType.Date).Value = deDate_Filter.Date;
            _daEmp.Fill(_dtEmp);
            dgEmp.DataSource = _dtEmp.DefaultView;
            dgEmp.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgEmp.Columns["CODE_SUBDIV"].Width = 40;
            dgEmp.Columns["SUBDIV_NAME"].Visible = false;
            dgEmp.Columns["PER_NUM"].HeaderText = "Таб.№";
            dgEmp.Columns["PER_NUM"].Width = 50;
            dgEmp.Columns["FIO"].HeaderText = "ФИО";
            dgEmp.Columns["FIO"].Width = 150;
            dgEmp.Columns["COMB"].HeaderText = "С.";
            dgEmp.Columns["COMB"].Width = 20;
            dgEmp.Columns["SIGN_COMB"].Visible = false;
            dgEmp.Columns["POS_NAME"].HeaderText = "Должность";
            dgEmp.Columns["POS_NAME"].Width = 250;
            dgEmp.Columns["EMP_LAST_NAME"].Visible = false;
            dgEmp.Columns["EMP_FIRST_NAME"].Visible = false;
            dgEmp.Columns["EMP_MIDDLE_NAME"].Visible = false;

            _daEmp.Fill(_dtEmpForOrder);
            _dtEmpForOrder.Clear();
            dgEmpForOrder.DataSource = _dtEmpForOrder.DefaultView;
            dgEmpForOrder.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgEmpForOrder.Columns["CODE_SUBDIV"].Width = 40;
            dgEmpForOrder.Columns["SUBDIV_NAME"].Visible = false;
            dgEmpForOrder.Columns["PER_NUM"].HeaderText = "Таб.№";
            dgEmpForOrder.Columns["PER_NUM"].Width = 50;
            dgEmpForOrder.Columns["FIO"].HeaderText = "ФИО";
            dgEmpForOrder.Columns["FIO"].Width = 150;
            dgEmpForOrder.Columns["COMB"].HeaderText = "С.";
            dgEmpForOrder.Columns["COMB"].Width = 20;
            dgEmpForOrder.Columns["SIGN_COMB"].Visible = false;
            dgEmpForOrder.Columns["POS_NAME"].HeaderText = "Должность";
            dgEmpForOrder.Columns["POS_NAME"].Width = 250;
            dgEmpForOrder.Columns["EMP_LAST_NAME"].Visible = false;
            dgEmpForOrder.Columns["EMP_FIRST_NAME"].Visible = false;
            dgEmpForOrder.Columns["EMP_MIDDLE_NAME"].Visible = false;

            deDate_Filter.TextChanged += new EventHandler(deDate_Filter_TextChanged);

            rowsPay = new List<DataRow>();
        }

        void deDate_Filter_TextChanged(object sender, EventArgs e)
        {
            if (deDate_Filter.Date != null)
            {
                _dtEmp.Clear();
                _daEmp.SelectCommand.Parameters["p_DATE"].Value = deDate_Filter.Date;
                _daEmp.Fill(_dtEmp);
                _dtEmpForOrder.Clear();
            }
        }

        void FilterEmp()
        {
            string _row_Filter = "";
            if (!String.IsNullOrEmpty(_filter_Subdiv))
            {
                _row_Filter = "CODE_SUBDIV LIKE '" + _filter_Subdiv + "'";
            }
            if (!String.IsNullOrEmpty(_filter_Per_Num))
            {
                _row_Filter = string.Format("{0} {2} {1}", _row_Filter, "PER_NUM LIKE '" + _filter_Per_Num + "'",
                    _row_Filter != "" ? "and" : "").Trim();
            }
            if (!String.IsNullOrEmpty(_filter_FIO))
            {
                _row_Filter = string.Format("{0} {2} {1}", _row_Filter, "FIO LIKE '" + _filter_FIO + "'",
                    _row_Filter != "" ? "and" : "").Trim();
            }
            _dtEmp.DefaultView.RowFilter = _row_Filter;
        }

        private void tbFilter_Code_Subdiv_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter_Code_Subdiv.Text.Trim() != "")
            {
                _filter_Subdiv = tbFilter_Code_Subdiv.Text.Trim() + "%";
            }
            else
            {
                _filter_Subdiv = null;
            }
            FilterEmp();
        }

        private void tbFilter_Per_Num_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter_Per_Num.Text.Trim() != "")
            {
                _filter_Per_Num = tbFilter_Per_Num.Text.Trim() + "%";
            }
            else
            {
                _filter_Per_Num = null;
            }
            FilterEmp();
        }

        private void tbFilter_FIO_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter_FIO.Text.Trim() != "")
            {
                _filter_FIO = tbFilter_FIO.Text.Trim() + "%";
            }
            else
            {
                _filter_FIO = null;
            }
            FilterEmp();
        }

        private void dgEmp_DoubleClick(object sender, EventArgs e)
        {
            if (dgEmp.SelectedRows != null)
            {
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmp.SelectedRows)
                {
                    _dtEmpForOrder.ImportRow(_dtEmp.DefaultView[row.Index].Row);
                    rowsPay.Add(_dtEmp.DefaultView[row.Index].Row);                        
                }
                foreach (DataRow row in rowsPay)
                {
                    _dtEmp.Rows.Remove(row);
                }
            }
        }

        private void dgEmpForOrder_DoubleClick(object sender, EventArgs e)
        {
            if (dgEmpForOrder.SelectedRows != null)
            {
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmpForOrder.SelectedRows)
                {
                    _dtEmp.ImportRow(_dtEmpForOrder.Rows[row.Index]);
                    rowsPay.Add(_dtEmpForOrder.Rows[row.Index]);
                }
                foreach (DataRow row in rowsPay)
                {
                    _dtEmpForOrder.Rows.Remove(row);
                }
            }
        }

        private void btExecute_Click(object sender, EventArgs e)
        {
            switch (dgEmpForOrder.Rows.Count)
            {
                case 0:
                    MessageBox.Show("Вы не выбрали сотрудников для формирования приказа!", "АСУ \"Кадры\"", 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 1:
                    CellParameter[] cellParameters = new CellParameter[] { 
                        new CellParameter(17, 56, dgEmpForOrder.Rows[0].Cells["PER_NUM"].Value.ToString(), null), 
                        new CellParameter(18, 1, dgEmpForOrder.Rows[0].Cells["EMP_LAST_NAME"].Value.ToString() + " " + 
                            dgEmpForOrder.Rows[0].Cells["EMP_FIRST_NAME"].Value.ToString() + " " + 
                            dgEmpForOrder.Rows[0].Cells["EMP_MIDDLE_NAME"].Value.ToString(), null), 
                        new CellParameter(20, 20, dgEmpForOrder.Rows[0].Cells["CODE_SUBDIV"].Value.ToString(), null), 
                        new CellParameter(21,1, dgEmpForOrder.Rows[0].Cells["SUBDIV_NAME"].Value.ToString(), null),
                        new CellParameter(23, 1, dgEmpForOrder.Rows[0].Cells["POS_NAME"].Value.ToString(), null)                
                    };
                    Excel.PrintR1C1(false, "OrderOnEncouraging_OneEmp.xlt", cellParameters);
                    break;
                default:
                    DataTable _dtRep = new DataTable();
                    new OracleDataAdapter("select 0 RN, '' FIO, '' PER_NUM, '' CODE_SUBDIV, '' POS_NAME from dual", Connect.CurConnect).Fill(_dtRep);
                    _dtRep.Clear();
                    int _rn = 1;
                    foreach (DataGridViewRow row in dgEmpForOrder.Rows)
                    {
                        _dtRep.Rows.Add(_rn++,
                            row.Cells["EMP_LAST_NAME"].Value.ToString() + " " +
                            row.Cells["EMP_FIRST_NAME"].Value.ToString() + " " +
                            row.Cells["EMP_MIDDLE_NAME"].Value.ToString(), 
                            row.Cells["PER_NUM"].Value.ToString(),
                            row.Cells["CODE_SUBDIV"].Value.ToString(),
                            row.Cells["POS_NAME"].Value.ToString());
                    }
                    /* Старый вариант*/
                    WExcel.Application m_ExcelApp = new WExcel.Application();
                    try
                    {
                        WExcel._Worksheet m_Sheet;
                        object oMissing = System.Reflection.Missing.Value;
                        m_ExcelApp.Visible = false;
                        string PathOfTemplate = Application.StartupPath + @"\Reports\OrderOnEncouraging_ManyEmps.xlt";
                        m_ExcelApp.Workbooks.Open(PathOfTemplate, oMissing, oMissing,
                            oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                            oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                        m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                        
                        WExcel.Range r = m_Sheet.get_Range("A24", "BL24");
                        int n = r.Rows.Count, m = r.Columns.Count;
                        for (int i = 1; i < _dtRep.Rows.Count; i++)
                        {
                            //newRange = m_Sheet.get_Range(AddRows(LeftUpCornerTemplate, n * i), AddRows(RightDownCornerTemplate, n * i));
                            r.Copy();
                            r.Insert(WExcel.XlInsertShiftDirection.xlShiftDown, Type.Missing);
                        }
                        for (int row = 0; row < _dtRep.Rows.Count; row++)
                        {
                            m_Sheet.Cells[row + 24, 1] = _dtRep.Rows[row]["RN"];
                            m_Sheet.Cells[row + 24, 5] = _dtRep.Rows[row]["FIO"];
                            m_Sheet.Cells[row + 24, 22] = _dtRep.Rows[row]["PER_NUM"];
                            m_Sheet.Cells[row + 24, 28] = _dtRep.Rows[row]["CODE_SUBDIV"];
                            m_Sheet.Cells[row + 24, 33] = _dtRep.Rows[row]["POS_NAME"];
                        }

                        m_ExcelApp.DisplayAlerts = false;
                        m_ExcelApp.Visible = true;
                    }
                    catch
                    {
                        m_ExcelApp.DisplayAlerts = false;
                        m_ExcelApp.Visible = true;
                        m_ExcelApp.Quit();
                        m_ExcelApp = null;
                        throw;
                    }
                    finally
                    {
                        //Что бы там ни было вызываем сборщик мусора
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                    ///* 22.04.2016 - новый вариант через RDLC - отказался от него из-за того, что эксель не расширяет строки при экспорте*/
                    //ReportViewerWindow.RenderToExcel(this, "OrderOnEncouraging_ManyEmps.rdlc", _dtRep,
                    //    null, "Приказ о поощрении работников", "xls");
                    break;
            }
        }
    }
}
