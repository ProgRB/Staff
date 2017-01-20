using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Kadr;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using WExcel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Tabel
{
    public class ReportClasses
    {
        private static DataSet _dsTempForReport;
        private static DateTime dateTemp;
        /// <summary>
        /// Форма ввода месяца и года для формирования отчетов
        /// </summary>
        static ReportTable reportTable;
        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        static TimeExecute timeExecute;

        static SelectPeriod selPeriod;
        /// <summary>
        /// Отчет по среднесписочной численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RepAverage_Number()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
                //daReport.SelectCommand.Parameters.Add("p_days", OracleDbType.Int16).Value =
                //    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1).Day;
                daReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Среднесписочная численность", "Reports/RepAverage_Number.rdlc",
                            ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_MONTH", reportTable.MonthTable.ToString().PadLeft(2, '0')),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Сводный Отчет по среднесписочной численности
        /// </summary>
        public static void RepAverage_Number_Consolidated()
        {
            ReportTable reportTable = new ReportTable(false, true);
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number_Consolidated.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = new DateTime(reportTable.YearTable, 1, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = new DateTime(reportTable.YearTable + 1, 1, 1).AddSeconds(-1);
                daReport.SelectCommand.Parameters.Add("p_cur_table1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                daReport.SelectCommand.Parameters.Add("p_cur_table2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                daReport.Fill(ds);
                if (ds.Tables["Table"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Среднесписочная численность", "Reports/RepAverage_Number_Consolidated.rdlc",
                    //        ds,
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}, 
                    //        true
                    //    );
                    //report.Show();
                    ReportViewerWindow.RenderToExcel(new Form(), "RepAverage_Number_Consolidated.rdlc",
                        new DataTable[] { ds.Tables[0], ds.Tables[1] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())});
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет по списочной численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RepListEmp_Number()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepCount_Number.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
                daReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Списочная численность", "Reports/RepCount_Number.rdlc",
                            ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE", 
                                new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1).ToShortDateString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по месяцам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RepUseOfWorkTime()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepUseOfWorkTime.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
                daReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Использование рабочего времени по месяцам", "Reports/RepUseOfWorkTime.rdlc",
                            ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RepUseOfWorkTimeOnSub()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepUseOfWorkTimeOnSub.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
                daReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Использование рабочего времени по месяцам", "Reports/RepUseOfWorkTimeOnSub.rdlc",
                            ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_MONTH", reportTable.MonthTable.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        /// <summary>
        /// Создание отчета Сводный отчет по неотработанному времени и ССЧ
        /// </summary>
        public static void RepTimeNotWorker()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    DoRepTimeNotWorker(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }


        /// <summary>
        /// Сводный отчет по неотработанному времени и ССЧ
        /// </summary>
        /// <param name="data"></param>
        static void DoRepTimeNotWorker(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);

            DataSet ds = new DataSet();
            ds.Tables.Add("REPORT");
            OracleDataAdapter daReport = new OracleDataAdapter();
            daReport.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("Table/RepTimeNotWorker.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
            daReport.SelectCommand.BindByName = true;
            daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
            daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
            daReport.SelectCommand.Parameters.Add("p_TABLE_DEG_PRIORITY", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
            OracleDataAdapter daDeg_Avg_Number = new OracleDataAdapter();
            daDeg_Avg_Number.SelectCommand = new OracleCommand(string.Format(
                "select * from {0}.DEGREE_AVG_NUMBER where SIGN_REP_ECON = 1 order by DEG_NUM_GROUP, DEG_PRIORITY", Connect.Schema), Connect.CurConnect);
            daDeg_Avg_Number.Fill(ds, "DEGREE_AVG_NUMBER");

            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Application.StartupPath + @"\Reports\RepTimeNotWorker.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Cells[3, 1] = "за " + reportTable.YearTable + " г.";

                Dictionary<string, Dictionary<int, string>> hs = new Dictionary<string, Dictionary<int, string>>();
                TotalRowsStyle c = new TotalRowsStyle("GR1", Color.FromArgb(216, 216, 216), Color.Black, 1m);
                WExcel.Style my_style = m_ExcelApp.ActiveWorkbook.Styles.Add("style_" + c.TotalFlagColumnName + c.TotalFlagValue.GetHashCode().ToString(), Type.Missing);
                my_style.Interior.Color = System.Drawing.ColorTranslator.ToOle(c.BackColor);
                if (c.ForeColor != Color.Empty)
                    my_style.Font.Color = ColorTranslator.ToOle(c.ForeColor);
                hs.Add(c.TotalFlagColumnName.ToUpper(), new Dictionary<int, string>());
                hs[c.TotalFlagColumnName.ToUpper()].Add(c.TotalFlagValue.GetHashCode(), my_style.Name);

                int numSheet = 1, j;
                string startExcel = "A7";
                object[,] str;

                WExcel.Range r;

                foreach (DataRow dataRow in ds.Tables["DEGREE_AVG_NUMBER"].Rows)
                {
                    ds.Tables["REPORT"].Clear();
                    ((BackgroundWorker)sender).ReportProgress(
                        Convert.ToInt16(Math.Truncate(((numSheet-1) / Convert.ToDecimal(ds.Tables["DEGREE_AVG_NUMBER"].Rows.Count)) * 100)));
                    // Копируем лист шаблона
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                    // Задаем рабочую облать - новый рабочий лист
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                    m_Sheet.Name = dataRow["DEG_NOTE"].ToString();
                    m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: " + dataRow["DEG_NOTE"].ToString();

                    daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                        new decimal[] { Convert.ToDecimal(dataRow["DEG_PRIORITY"]) };
                    daReport.Fill(ds.Tables["REPORT"]);

                    r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                    r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                    r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                    r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                    str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                    //Перебираем все колонки
                    j = -1;
                    for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                    {
                        if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                        {
                            Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                            for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                                if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                                {
                                    WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                    r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                    r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                    r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                    r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                                }
                        }
                        else
                        {
                            ++j;
                            for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                                str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                        }
                    }
                    r.set_Value(Type.Missing, str);
                }

                // Собираем данные по ППП
                // Копируем лист шаблона
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet - 1]);
                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                // Задаем рабочую облать - новый рабочий лист
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                m_Sheet.Name = "СВОД ППП";
                m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: СВОД ППП";

                ds.Tables["REPORT"].Clear();
                ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.RowFilter = "DEG_LEVEL = 1 and DEG_NOTE <> '13'";
                daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                    ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("DEG_PRIORITY")).ToArray();
                daReport.Fill(ds.Tables["REPORT"]);

                r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                //Перебираем все колонки
                j = -1;
                for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                {
                    if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                    {
                        Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                        for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                            if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                            {
                                WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                            }
                    }
                    else
                    {
                        ++j;
                        for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                            str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                    }
                }
                r.set_Value(Type.Missing, str);

                numSheet += 1;
                // Собираем данные по Всему заводу
                // Копируем лист шаблона
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                // Задаем рабочую облать - новый рабочий лист
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                m_Sheet.Name = "ВСЕГО ЗАВОД";
                m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: ВСЕГО ЗАВОД";

                ds.Tables["REPORT"].Clear();
                ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.RowFilter = "DEG_LEVEL = 1";
                daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                    ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("DEG_PRIORITY")).ToArray();
                daReport.Fill(ds.Tables["REPORT"]);

                r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                //Перебираем все колонки
                j = -1;
                for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                {
                    if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                    {
                        Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                        for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                            if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                            {
                                WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                            }
                    }
                    else
                    {
                        ++j;
                        for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                            str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                    }
                }
                r.set_Value(Type.Missing, str);

                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();
                m_ExcelApp.DisplayAlerts = false;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Delete();
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();

                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.Visible = true;
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Создание отчета Сводный отчет по неотработанному времени и ССЧ по подразделениям
        /// </summary>
        public static void RepTimeNotWorkerOnSub()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    DoRepTimeNotWorkerOnSub(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }


        /// <summary>
        /// Сводный отчет по неотработанному времени и ССЧ по подразделениям
        /// </summary>
        /// <param name="data"></param>
        static void DoRepTimeNotWorkerOnSub(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);

            DataSet ds = new DataSet();
            ds.Tables.Add("REPORT");
            OracleDataAdapter daReport = new OracleDataAdapter();
            daReport.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("Table/RepTimeNotWorkerOnSub.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
            daReport.SelectCommand.BindByName = true;
            daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
            daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
            daReport.SelectCommand.Parameters.Add("p_TABLE_DEG_PRIORITY", OracleDbType.Array).UdtTypeName =
                Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
            OracleDataAdapter daDeg_Avg_Number = new OracleDataAdapter();
            daDeg_Avg_Number.SelectCommand = new OracleCommand(string.Format(
                "select * from {0}.DEGREE_AVG_NUMBER where SIGN_REP_ECON = 1 order by DEG_NUM_GROUP, DEG_PRIORITY", Connect.Schema), Connect.CurConnect);
            daDeg_Avg_Number.Fill(ds, "DEGREE_AVG_NUMBER");

            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = true;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Application.StartupPath + @"\Reports\RepTimeNotWorkerOnSub.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = true;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Cells[3, 1] = "за " + reportTable.MonthTable + " месяц " + reportTable.YearTable + " г.";

                Dictionary<string, Dictionary<int, string>> hs = new Dictionary<string, Dictionary<int, string>>();
                TotalRowsStyle c = new TotalRowsStyle("GR1", Color.FromArgb(216, 216, 216), Color.Black, 1m);
                WExcel.Style my_style = m_ExcelApp.ActiveWorkbook.Styles.Add("style_" + c.TotalFlagColumnName + c.TotalFlagValue.GetHashCode().ToString(), Type.Missing);
                my_style.Interior.Color = System.Drawing.ColorTranslator.ToOle(c.BackColor);
                if (c.ForeColor != Color.Empty)
                    my_style.Font.Color = ColorTranslator.ToOle(c.ForeColor);
                hs.Add(c.TotalFlagColumnName.ToUpper(), new Dictionary<int, string>());
                hs[c.TotalFlagColumnName.ToUpper()].Add(c.TotalFlagValue.GetHashCode(), my_style.Name);

                int numSheet = 1, j;
                string startExcel = "A7";
                object[,] str;

                WExcel.Range r;

                foreach (DataRow dataRow in ds.Tables["DEGREE_AVG_NUMBER"].Rows)
                {
                    ds.Tables["REPORT"].Clear();
                    ((BackgroundWorker)sender).ReportProgress(
                        Convert.ToInt16(Math.Truncate(((numSheet - 1) / Convert.ToDecimal(ds.Tables["DEGREE_AVG_NUMBER"].Rows.Count)) * 100)));
                    // Копируем лист шаблона
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                    // Задаем рабочую облать - новый рабочий лист
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                    m_Sheet.Name = dataRow["DEG_NOTE"].ToString();
                    m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: " + dataRow["DEG_NOTE"].ToString();

                    daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                        new decimal[] { Convert.ToDecimal(dataRow["DEG_PRIORITY"]) };
                    daReport.Fill(ds.Tables["REPORT"]);

                    r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                    r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                    r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                    r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                    str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                    //Перебираем все колонки
                    j = -1;
                    for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                    {
                        if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                        {
                            Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                            for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                                if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                                {
                                    WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                    r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                    r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                    r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                    r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                                }
                        }
                        else
                        {
                            ++j;
                            for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                                str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                        }
                    }
                    r.set_Value(Type.Missing, str);
                }

                // Собираем данные по ППП
                // Копируем лист шаблона
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet - 1]);
                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                // Задаем рабочую облать - новый рабочий лист
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                m_Sheet.Name = "СВОД ППП";
                m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: СВОД ППП";

                ds.Tables["REPORT"].Clear();
                ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.RowFilter = "DEG_LEVEL = 1 and DEG_NOTE <> '13'";
                daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                    ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("DEG_PRIORITY")).ToArray();
                daReport.Fill(ds.Tables["REPORT"]);

                r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                //Перебираем все колонки
                j = -1;
                for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                {
                    if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                    {
                        Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                        for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                            if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                            {
                                WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                            }
                    }
                    else
                    {
                        ++j;
                        for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                            str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                    }
                }
                r.set_Value(Type.Missing, str);

                numSheet += 1;
                // Собираем данные по Всему заводу
                // Копируем лист шаблона
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                // Задаем рабочую облать - новый рабочий лист
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                m_Sheet.Name = "ВСЕГО ЗАВОД";
                m_Sheet.Cells[4, 1] = "КАТЕГОРИЯ: ВСЕГО ЗАВОД";

                ds.Tables["REPORT"].Clear();
                ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.RowFilter = "DEG_LEVEL = 1";
                daReport.SelectCommand.Parameters["p_TABLE_DEG_PRIORITY"].Value =
                    ds.Tables["DEGREE_AVG_NUMBER"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("DEG_PRIORITY")).ToArray();
                daReport.Fill(ds.Tables["REPORT"]);

                r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, ds.Tables["REPORT"].Rows.Count - 1), ds.Tables["REPORT"].Columns.Count - 2));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;

                str = new object[ds.Tables["REPORT"].Rows.Count, ds.Tables["REPORT"].Columns.Count];
                //Перебираем все колонки
                j = -1;
                for (int column = 0; column < ds.Tables["REPORT"].Columns.Count; column++)
                {
                    if (hs.ContainsKey(ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()))
                    {
                        Dictionary<int, string> t = hs[ds.Tables["REPORT"].Columns[column].ColumnName.ToUpper()];
                        for (int k = 0; k < ds.Tables["REPORT"].Rows.Count; ++k)
                            if (t.ContainsKey(ds.Tables["REPORT"].Rows[k][column].GetHashCode()))
                            {
                                WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, k), Excel.AddCols(Excel.AddRows(startExcel, k), ds.Tables["REPORT"].Columns.Count - 2));
                                r_row.Style = t[ds.Tables["REPORT"].Rows[k][column].GetHashCode()];
                                r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                                r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                                r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                            }
                    }
                    else
                    {
                        ++j;
                        for (int row = 0; row < ds.Tables["REPORT"].Rows.Count; row++)
                            str[row, j] = ds.Tables["REPORT"].Rows[row][column];
                    }
                }
                r.set_Value(Type.Missing, str);

                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();
                m_ExcelApp.DisplayAlerts = false;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Delete();
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();

                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.Visible = true;
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }


        /// <summary>
        /// Отчет по среднесписочной численности для группы личного стола отдела кадров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RepAverage_Number_Personnel()
        {
            reportTable = new ReportTable();
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number_Personnel.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1);
                daReport.SelectCommand.Parameters.Add("p_days", OracleDbType.Int16).Value =
                    new DateTime(reportTable.YearTable, reportTable.MonthTable, 1).AddMonths(1).AddSeconds(-1).Day;
                daReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Среднесписочная численность", "Reports/RepAverage_Number_Personnel.rdlc",
                            ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_MONTH", reportTable.MonthTable.ToString().PadLeft(2, '0')),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет по отпуску по беременности и родам
        /// </summary>
        public static void RepOnPregnancy()
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepOnPregnancy.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 501;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod.Subdiv_ID;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                        new ExcelParameter("A1", "ОТЧЕТ по ОТПУСКУ ПО БЕРЕМЕННОСТИ И РОДАМ"),
                        new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " +
                            selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepOnChildCare.xlt", "A5", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по отпуску по уходу за ребенком до 3-х лет
        /// </summary>
        public static void RepOnChildCare()
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepOnChildCare.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 532;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod.Subdiv_ID;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                        new ExcelParameter("A1", "ОТЧЕТ по ОТПУСКУ ПО УХОДУ ЗА РЕБЕНКОМ ДО 3-Х ЛЕТ"),
                        new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " +
                            selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepOnChildCare.xlt", "A5", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Формирование отчета по среднесписочной численности в разрезе подразделений
        /// </summary>
        public static void RepAverage_Number_On_Plant()
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    PrintRepAvg(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Среднесписочная численность в разрезе подразделений
        /// </summary>
        /// <param name="backgroundWorker"></param>
        /// <param name="e1"></param>
        private static void PrintRepAvg(BackgroundWorker backgroundWorker, DoWorkEventArgs e1)
        {
            backgroundWorker.ReportProgress(0);
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                backgroundWorker.ReportProgress(5);

                DataSet _ds = new DataSet();
                _ds.Tables.Add("LIST_SUBDIV");
                _ds.Tables.Add("REPORT");
                OracleDataAdapter _daSubdiv = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/SelectListSubdiv.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daSubdiv.SelectCommand.BindByName = true;
                _daSubdiv.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daSubdiv.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                _daSubdiv.Fill(_ds, "LIST_SUBDIV");
                OracleDataAdapter _daData = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number_On_Subdiv.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daData.SelectCommand.BindByName = true;
                _daData.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daData.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                _daData.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                    Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
                OracleDataAdapter _daCountDoc = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/SelectCountEmp_On_Doc.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daCountDoc.SelectCommand.BindByName = true;
                _daCountDoc.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daCountDoc.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                _daCountDoc.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);

                m_Book = m_Books.Open(Application.StartupPath + @"\Reports\RepAverage_Number_On_Plant.xlt", oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                m_Sheet.Name = "Завод";
                _daData.SelectCommand.Parameters["p_TABLE_ID"].Value =
                    _ds.Tables["LIST_SUBDIV"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("SUBDIV_ID")).ToArray();
                _ds.Tables["REPORT"].Clear();
                _daData.Fill(_ds.Tables["REPORT"]);
                if (_ds.Tables["REPORT"].Rows.Count > 0)
                {
                    object[,] val1 = new object[6, 12];
                    // Обрабатываем 01 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 1";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val1[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F2", "Q7").set_Value(Type.Missing, val1);
                    // Обрабатываем 08 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 8";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val1[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F8", "Q13").set_Value(Type.Missing, val1);
                    object[,] val2 = new object[3, 12];
                    // Обрабатываем 02 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 2";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F14", "Q16").set_Value(Type.Missing, val2);
                    // Обрабатываем 09 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 9";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F17", "Q19").set_Value(Type.Missing, val2);
                    object[,] val3 = new object[12, 12];
                    // Обрабатываем 04 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 4";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val3[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F23", "Q34").set_Value(Type.Missing, val3);
                    // Обрабатываем 05 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 5";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F35", "Q37").set_Value(Type.Missing, val2);
                    // Обрабатываем 07 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 7";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F38", "Q40").set_Value(Type.Missing, val2);
                    // Обрабатываем 11 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 11";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F41", "Q43").set_Value(Type.Missing, val2);
                    // Обрабатываем 12 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 12";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F44", "Q46").set_Value(Type.Missing, val2);
                    // Обрабатываем 13 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 13";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F50", "Q52").set_Value(Type.Missing, val2);

                    object[,] val4 = new object[1, 17];
                    _ds.Tables.Add("COUNT_DOC");
                    _daCountDoc.SelectCommand.Parameters["p_pay_type_id"].Value = 501;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F64", "V64").set_Value(Type.Missing, val4);
                    _ds.Tables["COUNT_DOC"].Clear();
                    _daCountDoc.SelectCommand.Parameters["p_pay_type_id"].Value = 532;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F65", "V65").set_Value(Type.Missing, val4);
                    _ds.Tables["COUNT_DOC"].Clear();
                    _daCountDoc.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectCountComb_By_Month.sql"),
                        Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                    _daCountDoc.SelectCommand.BindByName = true;
                    _daCountDoc.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                    _daCountDoc.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F66", "V66").set_Value(Type.Missing, val4);
                }

                int numSheet = 2;
                object[,] val = new object[48, 12];

                backgroundWorker.ReportProgress(10);
                /*m_ExcelApp.Visible = true; 
                    */
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                numSheet = 2;
                foreach (DataRow rowSub in _ds.Tables["LIST_SUBDIV"].Rows)
                {
                    // Копируем лист шаблона перед началом работы
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                    // Задаем рабочую облаcть - новый рабочий лист
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                    m_Sheet.Name = rowSub["CODE_SUBDIV"].ToString() + (rowSub["SUB_ACTUAL_SIGN"].ToString() == "1" ? "" : "(не актуально)");
                    _daData.SelectCommand.Parameters["p_TABLE_ID"].Value = new decimal[] { Convert.ToDecimal(rowSub["SUBDIV_ID"]) };
                    _ds.Tables["REPORT"].Clear();
                    _daData.Fill(_ds.Tables["REPORT"]);
                    for (int i = 0; i < _ds.Tables["REPORT"].Rows.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val[i, j - 3] = _ds.Tables["REPORT"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F2", "Q49").set_Value(Type.Missing, val);
                    backgroundWorker.ReportProgress(90 * (numSheet - 1) / _ds.Tables["LIST_SUBDIV"].Rows.Count + 10);
                    //break;
                }

                m_ExcelApp.DisplayAlerts = false;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Select();
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).get_Range("A1");
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Delete();
                m_ExcelApp.DisplayAlerts = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.ScreenUpdating = true;
                /*((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Visible =
                    Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Select();*/
                /*m_Sheet.get_Range("A1", Type.Missing).Select();
                m_Sheet.Protect("258", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);*/
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();
                m_ExcelApp.Visible = true;

                //if (!GrantedRoles.GetGrantedRole("TABLE_FORM_FILE"))
                //{
                //    // Начинаем цикл по листам книги со 2 листа (1 - шаблон, который скрыт)
                //    for (int i = 1; i < numSheet; i++)
                //    {
                //        m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[i];
                //        m_Sheet.PrintPreview(true);
                //    }
                //    m_ExcelApp.Quit();
                //}
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Формирование отчета по отсутствию (добавлен 15.12.2015)
        /// </summary>
        public static void RepFailureToAppear()
        {
            selPeriod = new SelectPeriod();
            //selPeriod.BeginDate = new DateTime(2016, 1, 1);
            //selPeriod.EndDate = new DateTime(2016, 1, 31);
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet _dsReport = new DataSet();
                _dsReport.Tables.Add("Table1");
                _dsReport.Tables.Add("Table2");
                _dsReport.Tables.Add("Table3");
                _dsReport.Tables.Add("Table4");
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureToAppear.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table1"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureToAppearOnDoc.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table2"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Entry_Chief.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table3"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureTotalBySubdiv.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table4"]);
                if (_dsReport.Tables["Table1"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Отчет по отсутствию", "Reports/RepFailureToAppear.rdlc",
                    //        new DataTable[] { _dsReport.Tables["Table1"], _dsReport.Tables["Table2"], _dsReport.Tables["Table3"], _dsReport.Tables["Table4"] },
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //        new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                    //        new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                    //    );
                    //report.Show();

                    ReportViewerWindow.RenderToExcelWithFormulas(new Form(), "RepFailureToAppear.rdlc",
                        new DataTable[] { _dsReport.Tables["Table1"], _dsReport.Tables["Table2"], _dsReport.Tables["Table3"], _dsReport.Tables["Table4"] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("За указанный период нет данных.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Формирование отчета Поздний выход (добавлен 06.02.2016)
        /// </summary>
        public static void RepLate_Pass()
        {
            selPeriod = new SelectPeriod();
            //selPeriod.BeginDate = new DateTime(2016, 1, 1);
            //selPeriod.EndDate = new DateTime(2016, 1, 31);
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _dsTempForReport = new DataSet();
                _dsTempForReport.Tables.Add("Table1");
                _dsTempForReport.Tables.Add("Table2");
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    RepLate_Pass_backWorker(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
                if (_dsTempForReport.Tables["Table1"].Rows.Count > 0 || _dsTempForReport.Tables["Table2"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Поздний выход", "Reports/RepLate_Pass.rdlc",
                    //        _dsTempForReport,
                    //        null
                    //    );
                    //report.Show();

                    ReportViewerWindow.RenderToExcel(new Form(), "RepLate_Pass.rdlc",
                        new DataTable[] { _dsTempForReport.Tables["Table1"], _dsTempForReport.Tables["Table2"] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE_BEGIN", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE_END", selPeriod.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("За указанный период нет данных.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет Поздний выход
        /// </summary>
        /// <param name="data"></param>
        private static void RepLate_Pass_backWorker(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            try
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Pass.sql"),
                    Connect.Schema, "PERCO"), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_BEGIN", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_DATE_END", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsTempForReport.Tables["Table1"]);
                ((BackgroundWorker)sender).ReportProgress(50);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Pass_Holiday.sql"),
                    Connect.Schema, "PERCO"), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_BEGIN", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_DATE_END", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsTempForReport.Tables["Table2"]);
                ((BackgroundWorker)sender).ReportProgress(80);
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        
        /// <summary>
        /// Отработанные часы 102 ш.о. по заказам
        /// </summary>
        public static void RepHoursByOrdersPlant()
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(false, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepHoursByOrdersPlant.sql"),
                    Connect.Schema, Connect.SchemaSalary);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    Excel.PrintWithBorder(true, "RepHoursByOrdersPlant.xlt", "A6",
                            new DataTable[] { dtProtocol },
                            new ExcelParameter[] {new ExcelParameter("A2", "за период с" +
                        selPeriod.BeginDate.ToShortDateString() +
                        " по " + selPeriod.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по неотработанному рабочему времени в связи с переходом на режим неполного рабочего времени
        /// </summary>
        public static void Rep_Pay_Type_545()
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepPay_Type_By_Period.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                daReport.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                daReport.SelectCommand.Parameters.Add("p_CODE_PAY_TYPE", OracleDbType.Varchar2).Value = "545";
                daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = selPeriod.Subdiv_ID;
                DataTable dt = new DataTable();
                daReport.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Отчет по 545 шифру оплат", "Reports/RepPay_Type_545.rdlc",
                    //        new DataTable[] { dt },
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                    //    );
                    //report.Show();
                    ReportViewerWindow.RenderToExcel(new Form(), "RepPay_Type_545.rdlc",
                            new DataTable[] { dt },
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                                new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                        );
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет об использовании сверхурочного рабочего времени (новый отчет по лимитам)
        /// </summary>
        public static void RepAll_Limits_By_Subdiv()
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(true, false, false, true);
            selPeriod_Subdiv.ShowInTaskbar = false;
            if (selPeriod_Subdiv.ShowDialog() == DialogResult.OK)
            {
                /// Создаем адаптер и заполняем с помощью него данные
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/RepAll_Limits_By_Subdiv.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = selPeriod_Subdiv.Subdiv_ID == 0 ? -1 : selPeriod_Subdiv.Subdiv_ID;
                adapter.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal).Value = selPeriod_Subdiv.Year;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ReportViewerWindow.RenderToExcel(new Form(), "RepAll_Limits_By_Subdiv.rdlc",
                            new DataTable[] { dtProtocol },
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", selPeriod_Subdiv.Year.ToString())}
                        );
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
