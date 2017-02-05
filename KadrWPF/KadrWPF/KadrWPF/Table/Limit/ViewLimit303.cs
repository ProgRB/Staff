using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class ViewLimit303 : System.Windows.Forms.UserControl
    {
        DataTable dtLimit304, dtLimit306, dtLimitD304, dtLimitD306;
        OracleDataAdapter daLimit, daLimitD, daLimitDAll;
        public ViewLimit303()
        {
            InitializeComponent();            
            ssLimit.subdiv_id = 0;            
            CreateTable();
            nudYear.Value = DateTime.Today.Year;
            //FillLimit();
            RefreshGrid();
            if (GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
            {
                tsbAddL304.Enabled = true;
                tsbDeleteL304.Enabled = true;
                tsbAddL306.Enabled = true;
                tsbDeleteL306.Enabled = true;
            }
            else
            {
                tsbAddL304.Enabled = false;
                tsbDeleteL304.Enabled = false;
                tsbAddL306.Enabled = false;
                tsbDeleteL306.Enabled = false;
            }
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
            {
                tsbRepExcel304All.Enabled = true;
                tsbRepExcel306All.Enabled = true;
            }
            else
            {
                tsbRepExcel304All.Enabled = false;
                tsbRepExcel306All.Enabled = false;
            }
        }

        /// <summary>
        /// Заполнение данных
        /// </summary>
        private void FillLimit()
        {
            dtLimit304.Clear();
            dtLimit306.Clear();
            daLimit.SelectCommand.Parameters["p_subdiv_id"].Value = ssLimit.subdiv_id;            
            daLimit.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
            daLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 304;
            daLimit.Fill(dtLimit304);
            daLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 306;
            daLimit.Fill(dtLimit306);
            dtLimitD304.Clear();
            dtLimitD306.Clear();
            daLimitD.SelectCommand.Parameters["p_subdiv_id"].Value = ssLimit.subdiv_id;
            daLimitD.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
            daLimitD.SelectCommand.Parameters["p_pay_type_id"].Value = 304;
            daLimitD.Fill(dtLimitD304);
            daLimitD.SelectCommand.Parameters["p_pay_type_id"].Value = 306;
            daLimitD.Fill(dtLimitD306);
            foreach (DataGridViewColumn column in dgLimitD304.Columns)
            {
                /*Скрываем пустые колонки*/
                if (dgLimitD304.RowCount == 0 || dgLimitD304.Rows[dgLimitD304.RowCount - 1].Cells[column.Name].Value == DBNull.Value)
                {
                    column.Visible = false;
                }
                else
                {
                    column.Visible = true;
                    int pos = column.Name.IndexOf("план");
                    if (pos > 0)
                        column.DefaultCellStyle.BackColor = Color.AliceBlue;
                    pos = column.Name.IndexOf("303");
                    if (pos > 0)
                        column.Visible = false;
                }
            }
            if (dgLimitD304.ColumnCount > 0)
            {
                dgLimitD304.Columns["F_GROUP"].Visible = false;
                dgLimitD304.Columns["FL"].Visible = false;
            }
            foreach (DataGridViewColumn column in dgLimitD306.Columns)
            {
                /*Скрываем пустые колонки*/
                if (dgLimitD306.RowCount == 0 || dgLimitD306.Rows[dgLimitD306.RowCount - 1].Cells[column.Name].Value == DBNull.Value)
                {
                    column.Visible = false;
                }
                else
                {
                    column.Visible = true;
                    int pos = column.Name.IndexOf("план");
                    if (pos > 0)
                        column.DefaultCellStyle.BackColor = Color.AliceBlue;
                    pos = column.Name.IndexOf("303");
                    if (pos > 0)
                        column.Visible = false;
                }
            }
            if (dgLimitD306.ColumnCount > 0)
            {
                dgLimitD306.Columns["F_GROUP"].Visible = false;
                dgLimitD306.Columns["FL"].Visible = false;
            }
        }

        /// <summary>
        /// Создание таблицы и добавление параметров
        /// </summary>
        private void CreateTable()
        {
            dtLimit304 = new DataTable();
            dtLimit306 = new DataTable();
            daLimit = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/LimitOnSubdiv.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            daLimit.SelectCommand.BindByName = true;
            daLimit.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            daLimit.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            daLimit.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            dtLimitD304 = new DataTable();
            dtLimitD306 = new DataTable();
            daLimitD = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/ViewLimitDegree.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            daLimitD.SelectCommand.BindByName = true;
            daLimitD.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            daLimitD.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            daLimitD.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            daLimitD.SelectCommand.Parameters.Add("p_month1", OracleDbType.Decimal).Value = 1;
            daLimitD.SelectCommand.Parameters.Add("p_month2", OracleDbType.Decimal).Value = 12;
            daLimitD.SelectCommand.Parameters.Add("p_all_rows", OracleDbType.Decimal).Value = 1;
            daLimitD.SelectCommand.Parameters.Add("p_sign", OracleDbType.Decimal);

            daLimitDAll = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/ViewLimitDegree.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            daLimitDAll.SelectCommand.BindByName = true;
            daLimitDAll.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = -1;
            daLimitDAll.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            daLimitDAll.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            daLimitDAll.SelectCommand.Parameters.Add("p_month1", OracleDbType.Decimal).Value = 1;
            daLimitDAll.SelectCommand.Parameters.Add("p_month2", OracleDbType.Decimal).Value = 12;
            daLimitDAll.SelectCommand.Parameters.Add("p_all_rows", OracleDbType.Decimal).Value = 0;
            daLimitDAll.SelectCommand.Parameters.Add("p_sign", OracleDbType.Decimal).Value = 0;
        }
        
        /// <summary>
        /// Настройка грида для отображения данных
        /// </summary>
        void RefreshGrid()
        {
            dgLimit304.DataSource = dtLimit304;
            dgLimit304.Columns["LIMIT_ON_SUBDIV_ID"].Visible = false;
            dgLimit306.DataSource = dtLimit306;
            dgLimit306.Columns["LIMIT_ON_SUBDIV_ID"].Visible = false;
            dgLimitD304.DataSource = dtLimitD304;
            foreach (DataGridViewColumn column in dgLimitD304.Columns)
            {
                /*Настраиваем отображение колонок*/
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
            }
            dgLimitD304.Columns["Месяц"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgLimitD304.Columns["Месяц"].Frozen = true;
            dgLimitD304.Columns["F_GROUP"].Visible = false;
            dgLimitD304.Columns["FL"].Visible = false;
            dgLimitD304.Columns["ПОДР."].Visible = false;

            dgLimitD306.DataSource = dtLimitD306;
            foreach (DataGridViewColumn column in dgLimitD306.Columns)
            {
                /*Настраиваем отображение колонок*/
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
            }
            dgLimitD306.Columns["Месяц"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgLimitD306.Columns["Месяц"].Frozen = true;
            dgLimitD306.Columns["F_GROUP"].Visible = false;
            dgLimitD306.Columns["FL"].Visible = false;
            dgLimitD306.Columns["ПОДР."].Visible = false;
        }                

        /// <summary>
        /// Изменение подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssLimit_Subdiv_idChanged(object sender, EventArgs e)
        {
            FillLimit();            
        }

        /// <summary>
        /// Добавление данных по лимитам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddLimit_Click(object sender, EventArgs e)
        {
            if (ssLimit.subdiv_id != 0)
            {
                LIMIT_ON_SUBDIV_seq limit = new LIMIT_ON_SUBDIV_seq(Connect.CurConnect);
                limit.AddNew();
                ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).SUBDIV_ID =
                    ssLimit.subdiv_id.Value;
                if (((ToolStripButton)sender).Name == "tsbAddL304")
                {
                    ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).PAY_TYPE_ID = 304;
                }
                else
                {
                    ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).PAY_TYPE_ID = 306;
                }
                EditLimit editLimit = new EditLimit(true, limit);
                editLimit.Text = "Добавление данных по лимитам";
                editLimit.ShowDialog();
                FillLimit();
            }
        }

        /// <summary>
        /// Редактирование данных по лимитам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEditLimit_Click(object sender, EventArgs e)
        {
            LIMIT_ON_SUBDIV_seq limit = new LIMIT_ON_SUBDIV_seq(Connect.CurConnect);;
            if (((ToolStripButton)sender).Name == "tsbEditL304")
            {
                if (dgLimit304.CurrentRow != null &&
                    dgLimit304.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {                    
                    limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                        dgLimit304.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                    EditLimit editLimit = new EditLimit(false, limit);
                    editLimit.Text = "Редактирование данных по лимитам";
                    editLimit.ShowDialog();
                    FillLimit();
                }
            }            
            else
            {
                if (dgLimit306.CurrentRow != null &&
                    dgLimit306.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {
                    limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                        dgLimit306.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                    EditLimit editLimit = new EditLimit(false, limit);
                    editLimit.Text = "Редактирование данных по лимитам";
                    editLimit.ShowDialog();
                    FillLimit();
                }
            }
        }

        /// <summary>
        /// Удаление данных по лимитам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteLimit_Click(object sender, EventArgs e)
        {
            LIMIT_ON_SUBDIV_seq limit = new LIMIT_ON_SUBDIV_seq(Connect.CurConnect);
            if (((ToolStripButton)sender).Name == "tsbDeleteL304")
            {
                if (dgLimit304.CurrentRow != null &&
                    dgLimit304.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {                        
                        limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                            dgLimit304.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                        limit.Remove((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current);
                        limit.Save();
                        Connect.Commit();
                        FillLimit();
                    }
                }
            }
            else
            {
                if (dgLimit306.CurrentRow != null &&
                    dgLimit306.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                            dgLimit306.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                        limit.Remove((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current);
                        limit.Save();
                        Connect.Commit();
                        FillLimit();
                    }
                }
            }
        }

        /// <summary>
        /// Формирование отчета в экселе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRepExcel_Click(object sender, EventArgs e)
        {
            if (ssLimit.subdiv_id != 0)
            {
                Microsoft.Office.Interop.Excel.XlBordersIndex[] borders = new Microsoft.Office.Interop.Excel.XlBordersIndex[]
                { Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom,
                    Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight,
                Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop,
                Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft};
                List<ExcelParameter> excelPar = new List<ExcelParameter>();
                int col = 1;
                if (((ToolStripButton)sender).Name == "tsbRepExcel304")
                {
                    DataTable dtExcel = dtLimitD304.Clone();
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру " +
                        "304 в подразделении " + ssLimit.CodeSubdiv));                    
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dgLimitD304.RowCount == 0 || 
                            dgLimitD304.Rows[dgLimitD304.RowCount - 1].Cells[column.ColumnName].Value == DBNull.Value)
                        {
                            dtLimitD304.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            int pos = column.ColumnName.IndexOf("303");
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL" && pos == -1)
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }                            
                            if (pos > 0)
                                dtLimitD304.Columns.Remove(column.ColumnName);
                        }                        
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD304 }, excelPar.ToArray(),
                        new TotalRowsStyle[] {
                            new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                            new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
                else
                {
                    DataTable dtExcel = dtLimitD306.Clone();
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру " +
                        "306 в подразделении " + ssLimit.CodeSubdiv));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dgLimitD306.RowCount == 0 || dgLimitD306.Rows[dgLimitD306.RowCount - 1].Cells[column.ColumnName].Value == DBNull.Value)
                        {
                            dtLimitD306.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            int pos = column.ColumnName.IndexOf("303");
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL" && pos == -1)
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                            if (pos > 0)
                                dtLimitD306.Columns.Remove(column.ColumnName);
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD306 },
                        excelPar.ToArray(),
                        new TotalRowsStyle[] {
                            new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                            new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
            }
        }

        /// <summary>
        /// Формирование отчета в экселе по всем подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRepExcelAll_Click(object sender, EventArgs e)
        {
            SelectPeriod selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                daLimitDAll.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
                daLimitDAll.SelectCommand.Parameters["p_month1"].Value = selPeriod.BeginDate.Month;
                daLimitDAll.SelectCommand.Parameters["p_month2"].Value = selPeriod.EndDate.Month;
                Microsoft.Office.Interop.Excel.XlBordersIndex[] borders = new Microsoft.Office.Interop.Excel.XlBordersIndex[]
                    { Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom,
                        Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight,
                    Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop,
                    Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft};
                List<ExcelParameter> excelPar = new List<ExcelParameter>();
                int col = 1;
                if (((ToolStripButton)sender).Name == "tsbRepExcel304All")
                {
                    DataTable dtExcel = dtLimitD304.Clone();
                    dtExcel.Clear();
                    DataTable dtLimitD304All = dtExcel.Clone();
                    daLimitDAll.SelectCommand.Parameters["p_pay_type_id"].Value = 304;
                    daLimitDAll.Fill(dtLimitD304All);
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру 304"));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dtLimitD304All.Rows.Count == 0 ||
                            dtLimitD304All.Rows[dtLimitD304All.Rows.Count - 1][column.ColumnName] == DBNull.Value)
                        {
                            dtLimitD304All.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            int pos = column.ColumnName.IndexOf("303");
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL" && pos == -1)
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                            if (pos > 0)
                                dtLimitD304All.Columns.Remove(column.ColumnName);
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD304All }, excelPar.ToArray(),
                        new TotalRowsStyle[] {
                        new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                        new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
                else
                {
                    DataTable dtExcel = dtLimitD306.Clone();
                    dtExcel.Clear();
                    DataTable dtLimitD306All = dtExcel.Clone();
                    daLimitDAll.SelectCommand.Parameters["p_pay_type_id"].Value = 306;
                    daLimitDAll.Fill(dtLimitD306All);
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру 106"));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dtLimitD306All.Rows.Count == 0 ||
                            dtLimitD306All.Rows[dtLimitD306All.Rows.Count - 1][column.ColumnName] == DBNull.Value)
                        {
                            dtLimitD306All.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            int pos = column.ColumnName.IndexOf("303");
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL" && pos == -1)
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                            if (pos > 0)
                                dtLimitD306All.Columns.Remove(column.ColumnName);
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD306All },
                        excelPar.ToArray(),
                        new TotalRowsStyle[] {
                        new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                        new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
            }
        }

        /// <summary>
        /// Изменение цвета итоговой строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgLimitD_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Convert.ToInt32(((DataGridView)sender)["F_GROUP", e.RowIndex].Value) == 0)
            {

            }
            else if (Convert.ToInt32(((DataGridView)sender)["F_GROUP", e.RowIndex].Value) == 1)
            {                
                e.CellStyle.BackColor = Color.FromArgb(255, 255, 190);
            }
            else if (Convert.ToInt32(((DataGridView)sender)["F_GROUP", e.RowIndex].Value) == 2)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 255, 150);
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 255, 0);
            }
        }  
    }
}
