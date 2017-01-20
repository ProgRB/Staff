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
    public partial class ViewLimit : System.Windows.Forms.UserControl
    {
        DataTable dtLimit124, dtLimit106, dtLimitD124, dtLimitD106;
        OracleDataAdapter odaLimit, odaLimitD, odaLimitDAll;
        public ViewLimit()
        {
            InitializeComponent();            
            ssLimit.subdiv_id = 0;            
            CreateTable();
            nudYear.Value = DateTime.Today.Year;
            //FillLimit();
            RefreshGrid();
            if (GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
            {
                tsbAddL106.Enabled = true;
                tsbAddL124.Enabled = true;
                tsbDeleteL106.Enabled = true;
                tsbDeleteL124.Enabled = true;
            }
            else
            {
                tsbAddL106.Enabled = false;
                tsbAddL124.Enabled = false;
                tsbDeleteL106.Enabled = false;
                tsbDeleteL124.Enabled = false;
            }
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
            {
                tsbRepExcel124All.Enabled = true;
                tsbRepExcel106All.Enabled = true;
            }
            else
            { 
                tsbRepExcel124All.Enabled = false;
                tsbRepExcel106All.Enabled = false;
            }
        }

        /// <summary>
        /// Заполнение данных
        /// </summary>
        private void FillLimit()
        {
            dtLimit124.Clear();
            dtLimit106.Clear();
            odaLimit.SelectCommand.Parameters["p_subdiv_id"].Value = ssLimit.subdiv_id;            
            odaLimit.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
            odaLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 124;
            odaLimit.Fill(dtLimit124);
            odaLimit.SelectCommand.Parameters["p_pay_type_id"].Value = 106;
            odaLimit.Fill(dtLimit106);
            dtLimitD124.Clear();
            dtLimitD106.Clear();
            odaLimitD.SelectCommand.Parameters["p_subdiv_id"].Value = ssLimit.subdiv_id;
            odaLimitD.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
            odaLimitD.SelectCommand.Parameters["p_pay_type_id"].Value = 124;
            odaLimitD.Fill(dtLimitD124);
            odaLimitD.SelectCommand.Parameters["p_pay_type_id"].Value = 106;
            odaLimitD.Fill(dtLimitD106);
            foreach (DataGridViewColumn column in dgLimitD124.Columns)
            {
                /*Скрываем пустые колонки*/
                if (dgLimitD124.RowCount == 0 || dgLimitD124.Rows[dgLimitD124.RowCount - 1].Cells[column.Name].Value == DBNull.Value)
                {
                    column.Visible = false;
                }
                else
                {
                    column.Visible = true;
                    int pos = column.Name.IndexOf("план");
                    if (pos > 0)
                        column.DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
            foreach (DataGridViewColumn column in dgLimitD106.Columns)
            {
                if (dgLimitD106.RowCount == 0 || dgLimitD106.Rows[dgLimitD106.RowCount - 1].Cells[column.Name].Value == DBNull.Value)
                {
                    column.Visible = false;
                }
                else
                {
                    column.Visible = true;
                    int pos = column.Name.IndexOf("план");
                    if (pos > 0)
                        column.DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
            if (dgLimitD124.ColumnCount > 0)
            {
                dgLimitD124.Columns["F_GROUP"].Visible = false;
                dgLimitD124.Columns["FL"].Visible = false;
            }
            if (dgLimitD106.ColumnCount > 0)
            {
                dgLimitD106.Columns["F_GROUP"].Visible = false;
                dgLimitD106.Columns["FL"].Visible = false;
            }
        }

        /// <summary>
        /// Создание таблицы и добавление параметров
        /// </summary>
        private void CreateTable()
        {
            dtLimit124 = new DataTable();
            dtLimit106 = new DataTable();
            odaLimit = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/LimitOnSubdiv.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            odaLimit.SelectCommand.BindByName = true;
            odaLimit.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            odaLimit.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            odaLimit.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            dtLimitD124 = new DataTable();
            dtLimitD106 = new DataTable();
            odaLimitD = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/ViewLimitDegree.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            odaLimitD.SelectCommand.BindByName = true;
            odaLimitD.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            odaLimitD.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            odaLimitD.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            odaLimitD.SelectCommand.Parameters.Add("p_month1", OracleDbType.Decimal).Value = 1;
            odaLimitD.SelectCommand.Parameters.Add("p_month2", OracleDbType.Decimal).Value = 12;
            odaLimitD.SelectCommand.Parameters.Add("p_all_rows", OracleDbType.Decimal).Value = 1;
            odaLimitD.SelectCommand.Parameters.Add("p_sign", OracleDbType.Decimal).Value = 0;

            odaLimitDAll = new OracleDataAdapter(string.Format(
                Queries.GetQuery("Table/ViewLimitDegree.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            odaLimitDAll.SelectCommand.BindByName = true;
            odaLimitDAll.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = -1;
            odaLimitDAll.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            odaLimitDAll.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal);
            odaLimitDAll.SelectCommand.Parameters.Add("p_month1", OracleDbType.Decimal).Value = 1;
            odaLimitDAll.SelectCommand.Parameters.Add("p_month2", OracleDbType.Decimal).Value = 12;
            odaLimitDAll.SelectCommand.Parameters.Add("p_all_rows", OracleDbType.Decimal).Value = 0;
            odaLimitDAll.SelectCommand.Parameters.Add("p_sign", OracleDbType.Decimal).Value = 0;
        }
        
        /// <summary>
        /// Настройка грида для отображения данных
        /// </summary>
        void RefreshGrid()
        {
            dgLimit124.DataSource = dtLimit124;
            dgLimit124.Columns["LIMIT_ON_SUBDIV_ID"].Visible = false;
            dgLimit106.DataSource = dtLimit106;
            dgLimit106.Columns["LIMIT_ON_SUBDIV_ID"].Visible = false;
            dgLimitD124.DataSource = dtLimitD124;
            dgLimitD106.DataSource = dtLimitD106;
            foreach (DataGridViewColumn column in dgLimitD124.Columns)
            {
                /*Настраиваем отображение колонок*/
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
            }
            foreach (DataGridViewColumn column in dgLimitD106.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                
            }
            dgLimitD124.Columns["Месяц"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgLimitD106.Columns["Месяц"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgLimitD124.Columns["Месяц"].Frozen = true;
            dgLimitD106.Columns["Месяц"].Frozen = true;
            dgLimitD124.Columns["F_GROUP"].Visible = false;
            dgLimitD106.Columns["F_GROUP"].Visible = false;
            dgLimitD124.Columns["FL"].Visible = false;
            dgLimitD106.Columns["FL"].Visible = false;
            dgLimitD124.Columns["ПОДР."].Visible = false;
            dgLimitD106.Columns["ПОДР."].Visible = false;
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
                if (((ToolStripButton)sender).Name == "tsbAddL124")
                {
                    ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).PAY_TYPE_ID = 124;
                }
                else
                {
                    ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).PAY_TYPE_ID = 106;
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
            if (((ToolStripButton)sender).Name == "tsbEditL124")
            {
                if (dgLimit124.CurrentRow != null &&
                    dgLimit124.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {                    
                    limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                        dgLimit124.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                    EditLimit editLimit = new EditLimit(false, limit);
                    editLimit.Text = "Редактирование данных по лимитам";
                    editLimit.ShowDialog();
                    FillLimit();
                }
            }
            else
            {
                if (dgLimit106.CurrentRow != null &&
                    dgLimit106.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {                    
                    limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                        dgLimit106.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
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
            if (((ToolStripButton)sender).Name == "tsbDeleteL124")
            {
                if (dgLimit124.CurrentRow != null &&
                    dgLimit124.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {                        
                        limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                            dgLimit124.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
                        limit.Remove((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current);
                        limit.Save();
                        Connect.Commit();
                        FillLimit();
                    }
                }
            }
            else
            {
                if (dgLimit106.CurrentRow != null && 
                    dgLimit106.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {                        
                        limit.Fill("where LIMIT_ON_SUBDIV_ID = " +
                            dgLimit106.CurrentRow.Cells["LIMIT_ON_SUBDIV_ID"].Value);
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
                if (((ToolStripButton)sender).Name == "tsbRepExcel124")
                {
                    DataTable dtExcel = dtLimitD124.Clone();
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру " +
                        "124 в подразделении " + ssLimit.CodeSubdiv));                    
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dgLimitD124.RowCount == 0 || 
                            dgLimitD124.Rows[dgLimitD124.RowCount - 1].Cells[column.ColumnName].Value == DBNull.Value)
                        {
                            dtLimitD124.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL")
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD124 }, excelPar.ToArray(),
                        new TotalRowsStyle[] {
                            new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                            new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
                else
                {
                    DataTable dtExcel = dtLimitD106.Clone();
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру " +
                        "106 в подразделении " + ssLimit.CodeSubdiv));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dgLimitD106.RowCount == 0 || dgLimitD106.Rows[dgLimitD106.RowCount - 1].Cells[column.ColumnName].Value == DBNull.Value)
                        {
                            dtLimitD106.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL")
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD106 }, 
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
                odaLimitDAll.SelectCommand.Parameters["p_year"].Value = nudYear.Value;
                odaLimitDAll.SelectCommand.Parameters["p_month1"].Value = selPeriod.BeginDate.Month;
                odaLimitDAll.SelectCommand.Parameters["p_month2"].Value = selPeriod.EndDate.Month;
                Microsoft.Office.Interop.Excel.XlBordersIndex[] borders = new Microsoft.Office.Interop.Excel.XlBordersIndex[]
                    { Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom,
                        Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight,
                    Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop,
                    Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft};
                List<ExcelParameter> excelPar = new List<ExcelParameter>();
                int col = 1;
                if (((ToolStripButton)sender).Name == "tsbRepExcel124All")
                {
                    DataTable dtExcel = dtLimitD124.Clone();
                    dtExcel.Clear();
                    DataTable dtLimitD124All = dtExcel.Clone();
                    odaLimitDAll.SelectCommand.Parameters["p_pay_type_id"].Value = 124;                    
                    odaLimitDAll.Fill(dtLimitD124All);
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру 124"));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dtLimitD124All.Rows.Count == 0 ||
                            dtLimitD124All.Rows[dtLimitD124All.Rows.Count - 1][column.ColumnName] == DBNull.Value)
                        {
                            dtLimitD124All.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL")
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD124All }, excelPar.ToArray(),
                        new TotalRowsStyle[] {
                        new TotalRowsStyle("FL", Color.Yellow, Color.Black, 1m),
                        new TotalRowsStyle("F_GROUP", Color.Yellow, Color.Black, 1m)});
                }
                else
                {
                    DataTable dtExcel = dtLimitD106.Clone();
                    dtExcel.Clear();
                    DataTable dtLimitD106All = dtExcel.Clone();
                    odaLimitDAll.SelectCommand.Parameters["p_pay_type_id"].Value = 106;
                    odaLimitDAll.Fill(dtLimitD106All);
                    excelPar.Add(new ExcelParameter("A1", "Отчет об использовании рабочего времени по шифру 106"));
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        /*Скрываем пустые колонки*/
                        if (dtLimitD106All.Rows.Count == 0 ||
                            dtLimitD106All.Rows[dtLimitD106All.Rows.Count - 1][column.ColumnName] == DBNull.Value)
                        {
                            dtLimitD106All.Columns.Remove(column.ColumnName);
                        }
                        else
                        {
                            if (column.ColumnName != "F_GROUP" && column.ColumnName != "FL")
                            {
                                excelPar.Add(new ExcelParameter(new Point(col, 3), new Point(col++, 3), column.ColumnName,
                                    borders));
                            }
                        }
                    }
                    Excel.PrintWithBorder(true, "LimitOnDegree.xlt", "A4", new DataTable[] { dtLimitD106All },
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
