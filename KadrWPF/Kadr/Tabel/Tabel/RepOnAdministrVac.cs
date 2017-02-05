using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using WExcel = Microsoft.Office.Interop.Excel;
using Staff;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class RepOnAdministrVac : Form
    {
        public RepOnAdministrVac()
        {
            InitializeComponent();
        }

        private void btOrderTruancy_Click(object sender, EventArgs e)
        {
            /*SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepOnAdministrVac.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 531;
                adapter.SelectCommand.Parameters.Add("p_note", OracleDbType.Varchar2).Value = "А";
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod.Subdiv_ID;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " + 
                        selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepOnAdministrVac.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }*/
            if (deEndPeriod.Date < deBeginPeriod.Date)
            {
                MessageBox.Show("Вы ввели неверные даты!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (deEndPeriod.Date.Value.Year != deBeginPeriod.Date.Value.Year)
            {
                MessageBox.Show("Даты должны быть за один год!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataTable dtData = new DataTable();
            OracleDataAdapter odaData = new OracleDataAdapter("", Connect.CurConnect);
            odaData.SelectCommand.BindByName = true;
            odaData.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepOnAdministrVacFio.sql"),
                DataSourceScheme.SchemeName);
            odaData.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = deBeginPeriod.Date;
            odaData.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                deEndPeriod.Date.Value.AddDays(1).AddSeconds(-1);
            odaData.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 531;
            odaData.SelectCommand.Parameters.Add("p_note", OracleDbType.Varchar2).Value = "А";
            odaData.Fill(dtData);
            if (dtData.Rows.Count == 0)
            {
                MessageBox.Show("За данный период данных по прогульщикам нет.", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            WExcel.Application m_ExcelApp = new WExcel.Application();
            try
            {
                //Создание книги Excel
                WExcel._Worksheet m_Sheet;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp.Visible = false;
                m_ExcelApp.Workbooks.Open(Application.StartupPath + @"\Reports\RepOnAdministrVac.xlt", oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];

                //Заполняем отдельные параметры
                m_Sheet.get_Range("A2", Type.Missing).Value2 = string.Format(
                    "за период с {0} по {1}", deBeginPeriod.Date.Value.ToShortDateString(),
                    deEndPeriod.Date.Value.ToShortDateString());

                /// Работаем сначала со второй частью отчета - По подразделениям
                //Заполняем массив данных
                //Перебираем все таблицы
                int sumCountRow = 0, sum = 0, max = 1, RowInStr;
                sum = dtData.Rows.Count;
                max = Math.Max(dtData.Columns.Count, max);
                sum = Math.Max(sum, 1);
                string startExcel = "A7";
                WExcel.Range r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, sum - 1), max - 1));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                string[,] str = new string[sum, max];

                //Перебираем все колонки
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    RowInStr = sumCountRow;
                    if (dtData.Columns[column].DataType == typeof(DateTime))
                        for (int row = 0; row < dtData.Rows.Count; row++, RowInStr++)
                            str[RowInStr, column] = (dtData.Rows[row][column] == DBNull.Value ? "" : ((DateTime)dtData.Rows[row][column]).ToShortDateString());
                    else
                        for (int row = 0; row < dtData.Rows.Count; row++, RowInStr++)
                            str[RowInStr, column] = dtData.Rows[row][column].ToString();
                }

                r.set_Value(Type.Missing, str);

                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[2];

                m_Sheet.get_Range("A2", Type.Missing).Value2 = string.Format(
                    "за период с {0} по {1}", deBeginPeriod.Date.Value.ToShortDateString(),
                    deEndPeriod.Date.Value.ToShortDateString());

                dtData = new DataTable();
                odaData = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/SelectRepOnAdministrVac.sql"),
                    Connect.Schema), Connect.CurConnect);
                odaData.SelectCommand.BindByName = true;
                odaData.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    deBeginPeriod.Date;
                odaData.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    deEndPeriod.Date.Value.AddDays(1).AddSeconds(-1);
                odaData.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 531;
                odaData.SelectCommand.Parameters.Add("p_note", OracleDbType.Varchar2).Value = "А";
                odaData.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = 0;

                odaData.Fill(dtData);

                /// Вставляем первую часть отчета - По фамилиям
                sumCountRow = sum = 0;
                max = 1;
                sum = dtData.Rows.Count;
                max = Math.Max(dtData.Columns.Count, max);
                sum = Math.Max(sum, 1);
                startExcel = "A6";
                r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, sum - 1), max - 1));
                r.BorderAround(WExcel.XlLineStyle.xlContinuous, WExcel.XlBorderWeight.xlThin, WExcel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                r.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                r.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                str = new string[sum, max];

                //Перебираем все колонки
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    RowInStr = sumCountRow;
                    if (dtData.Columns[column].DataType == typeof(DateTime))
                        for (int row = 0; row < dtData.Rows.Count; row++, RowInStr++)
                            str[RowInStr, column] = (dtData.Rows[row][column] == DBNull.Value ? "" : ((DateTime)dtData.Rows[row][column]).ToShortDateString());
                    else
                        for (int row = 0; row < dtData.Rows.Count; row++, RowInStr++)
                            str[RowInStr, column] = dtData.Rows[row][column].ToString();
                }

                r.set_Value(Type.Missing, str);

                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Visible = true;
            }
            catch
            {
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Visible = true;
                m_ExcelApp.Quit();
                m_ExcelApp = null;
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
