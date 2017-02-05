using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WWord = Microsoft.Office.Interop.Word;

namespace LibraryKadr
{
    public class Word
    {
        public static void PrintDocument(string nameOfTemplate, System.Data.DataTable table)
        {
            WWord.Application m_WordApp = new WWord.Application();
            try
            {
                object oMissing = System.Reflection.Missing.Value;
                m_WordApp.Visible = true;
                m_WordApp.Documents.Add(Application.StartupPath + @"\Reports\" + nameOfTemplate,
                    oMissing, oMissing, oMissing);
                WWord.Document _doc = m_WordApp.ActiveDocument;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    foreach (WWord.Bookmark item in _doc.Bookmarks)
                    {
                        if (table.Columns.Contains(item.Name))
                            item.Range.Text = table.Rows[0][item.Name].ToString();
                    }
                }
                m_WordApp.ScreenUpdating = true;
                m_WordApp.Visible = true;
                m_WordApp.Activate();
                //for (int i = 0; i < tables.Count(); i++)
                //{
                //    //Перебираем все колонки
                //    int j = -1;
                //    for (int column = 0; column < tables[i].Columns.Count; column++)
                //    {
                //        RowInStr = sumCountRow;
                //        if (hs.ContainsKey(tables[i].Columns[column].ColumnName.ToUpper()))
                //        {
                //            Dictionary<int, string> t = hs[tables[i].Columns[column].ColumnName.ToUpper()];
                //            for (int k = 0; k < tables[i].Rows.Count; ++k, RowInStr++)
                //                if (t.ContainsKey(tables[i].Rows[k][column].GetHashCode()))
                //                {
                //                    WExcel.Range r_row = m_Sheet.get_Range(Excel.AddRows(startExcel, RowInStr), Excel.AddCols(Excel.AddRows(startExcel, RowInStr), max - 1));
                //                    r_row.Style = t[tables[i].Rows[k][column].GetHashCode()];
                //                    r_row.HorizontalAlignment = WExcel.XlHAlign.xlHAlignCenter;
                //                    r_row.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                //                    r_row.Borders.Weight = WExcel.XlBorderWeight.xlThin;
                //                }
                //        }
                //        else
                //        {
                //            ++j;
                //            if (tables[i].Columns[column].DataType == typeof(DateTime))
                //                for (int row = 0; row < tables[i].Rows.Count; row++, RowInStr++)
                //                    str[RowInStr, j] = (tables[i].Rows[row][column] == DBNull.Value ? "" :
                //                        ShowLongDate ? ((DateTime)tables[i].Rows[row][column]).ToString() : ((DateTime)tables[i].Rows[row][column]).ToShortDateString());
                //            else
                //                for (int row = 0; row < tables[i].Rows.Count; row++, RowInStr++)
                //                    str[RowInStr, j] = tables[i].Rows[row][column];
                //        }
                //    }
                //    sumCountRow += tables[i].Rows.Count;
                //}
                //r.set_Value(Type.Missing, str);
                //m_ExcelApp.DisplayAlerts = false;
                //m_ExcelApp.ScreenUpdating = true;
                //m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                //m_ExcelApp.Visible = true;
                //if (!ShowEditor)
                //{
                //    m_Sheet.PrintPreview(true);
                //    m_ExcelApp.Quit();
                //}
            }
            catch
            {
                m_WordApp.Visible = true;
                m_WordApp.Quit();
                m_WordApp = null;
                throw;
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}
