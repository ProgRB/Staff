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
        /// <summary>
        /// Форма ввода месяца и года для формирования отчетов
        /// </summary>
        static ReportTable reportTable;
        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        static TimeExecute timeExecute;

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
    }
}
