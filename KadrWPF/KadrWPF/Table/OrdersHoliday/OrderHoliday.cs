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

using WExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using Oracle.DataAccess.Client;
using Kadr;

namespace Tabel
{
    public partial class OrderHoliday : Form
    {
        bool flagAdd;
        int subdiv_id;
        string code_subdiv, subdiv_name;
        ORDER_ON_HOLIDAY_seq order_on_holiday;
        DATE_FOR_ORDER_seq date_for_order;
        DataTable dtPay_type;
        OracleCommand ocUpdateDCO, ocDeleteDCO;
        DataTable dtLimit;
        OracleDataAdapter odaLimit;
        string[][] s_pos;
        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        public TimeExecute timeExecute;
        public OrderHoliday(bool _flagAdd, ORDER_ON_HOLIDAY_seq _orders, string _subdiv_id, 
            string _code_subdiv, string _subdiv_name)
        {
            InitializeComponent();
            flagAdd = _flagAdd;
            subdiv_id = Convert.ToInt32(_subdiv_id);
            code_subdiv = _code_subdiv;
            subdiv_name = _subdiv_name;
            order_on_holiday = _orders;
            tbNum_Order.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.NUM_ORDER);
            deDate_Order.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.DATE_ORDER);
            tbBase.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.BASE_ORDER);
            tbOfficial.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.OFFICIAL);
            deDate_Closing_Order.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.DATE_CLOSING_ORDER);
            dtPay_type = new DataTable();
            new OracleDataAdapter(string.Format(
                "SELECT PAY_TYPE_ID, PAY_TYPE_NAME from {0}.PAY_TYPE where pay_type_id in (106, 124) order by pay_type_id",
                Connect.Schema), Connect.CurConnect).Fill(dtPay_type);
            cbPay_Type.DataBindings.Add("SelectedValue", order_on_holiday, "PAY_TYPE_ID", true, DataSourceUpdateMode.OnPropertyChanged);
            cbPay_Type.DataSource = dtPay_type;
            cbPay_Type.DisplayMember = "PAY_TYPE_NAME";
            cbPay_Type.ValueMember = "PAY_TYPE_ID";
            chSign_Order_Plant.AddBindingSource(order_on_holiday, ORDER_ON_HOLIDAY_seq.ColumnsName.SIGN_ORDER_PLANT);
            dgDate_For_Order.DataSource = OrdersOnHoliday.DTDate_for_order;
            dgDate_For_Order.Columns["DATE_FOR_ORDER_ID"].Visible = false;
            dgDate_For_Order.Columns["RESPONSIBLE_WORKER"].HeaderText = "Ответственный за ППБ и ТБ";
            dgDate_For_Order.Columns["DATE_WORK_ORDER"].HeaderText = "Дата работы";
            date_for_order = new DATE_FOR_ORDER_seq(Connect.CurConnect);
            if (flagAdd)
            {
                object n = new object();
                EventArgs e = new EventArgs();
                btEditOrder_Click(n, e);
            }
            else
            {
                tbBase.Enabled = false;
                tbNum_Order.Enabled = false;
                deDate_Order.Enabled = false;
                chSign_Order_Plant.Enabled = false;
                tbOfficial.Enabled = false;
                cbPay_Type.Enabled = false;
                btEditOrder.Enabled = true;
                btSave.Enabled = false;
                btOrder.Enabled = true;
            }
            /*ocUpdateDCO = new OracleCommand(string.Format(
                "update {0}.ORDER_ON_HOLIDAY OH set OH.DATE_CLOSING_ORDER = :p_date_closing_order " +
                "where OH.ORDER_ON_HOLIDAY_ID = :p_order_on_holiday_id",
                Connect.Schema), Connect.CurConnect);*/
            ocUpdateDCO = new OracleCommand(string.Format(
                "begin {0}.ORDER_ON_HOLIDAY_CLOSE(:p_order_on_holiday_id, :p_date_closing_order, :p_SIGN_CHECK_LIMIT); end;",
                Connect.Schema), Connect.CurConnect);
            ocUpdateDCO.BindByName = true;
            ocUpdateDCO.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value =
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
            ocUpdateDCO.Parameters.Add("p_date_closing_order", OracleDbType.Date);
            ocUpdateDCO.Parameters.Add("p_SIGN_CHECK_LIMIT", OracleDbType.Int16);
            if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER != null)
                chDate_Closing_Order.Checked = true;
            gbClosign_Order.EnableByRules();
            ocDeleteDCO = new OracleCommand(string.Format(
                @"delete from {0}.DATE_FOR_ORDER where DATE_FOR_ORDER_ID = :p_DATE_FOR_ORDER_ID", 
                Connect.Schema), Connect.CurConnect);
            ocDeleteDCO.BindByName = true;
            ocDeleteDCO.Parameters.Add("p_DATE_FOR_ORDER_ID", OracleDbType.Decimal);
            dtLimit = new DataTable();
            odaLimit = new OracleDataAdapter("", Connect.CurConnect);
            odaLimit.SelectCommand.BindByName = true;
            odaLimit.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/ViewLimitForDegree.sql"), Connect.Schema);
            odaLimit.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).SUBDIV_ID;
            odaLimit.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value =
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID;
            odaLimit.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value =
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
        }

        void FillDate_for_order()
        {
            OrdersOnHoliday.DTDate_for_order.Clear();
            OrdersOnHoliday.ODADate_for_order.SelectCommand.Parameters["p_ORDER_ON_HOLIDAY_ID"].Value =
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
            OrdersOnHoliday.ODADate_for_order.Fill(OrdersOnHoliday.DTDate_for_order);
        }

        private void btOrder_Click(object sender, EventArgs e)
        {            
            s_pos = new string[][] { };
            if (Signes.Show(subdiv_id, "OrderHoliday",
                "Введите должность и ФИО лица (подготовил)", 1, ref s_pos) == true)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    PrintOrder(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();   
            }
        }

        /// <summary>
        ///  Формирование талонов
        /// </summary>
        /// <param name="data"></param>
        void PrintOrder(object sender, DoWorkEventArgs e)
        {
            #region Печать приказа
            ((BackgroundWorker)sender).ReportProgress(0);
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
                string PathOfTemplate;
                /// Создаем таблицу для дней работы по приказу
                OracleDataTable dtDate_For_Order = new OracleDataTable("", Connect.CurConnect);
                dtDate_For_Order.SelectCommand.CommandText = string.Format(
                    "select * from {0}.DATE_FOR_ORDER " +
                    "where ORDER_ON_HOLIDAY_ID = :p_order_on_holiday_id and DATE_WORK_ORDER is not null " +
                    "order by DATE_WORK_ORDER",
                    Connect.Schema);
                dtDate_For_Order.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal);
                ((BackgroundWorker)sender).ReportProgress(10);
                if (chSign_Order_Plant.Checked)
                {
                    PathOfTemplate = Application.StartupPath + @"\Reports\Table_Holiday_New.xlt";

                    m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                    m_ExcelApp.ScreenUpdating = false;
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                    if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER != null)
                    {
                        m_Sheet.Cells[2, 1] = string.Format("«{0}» {1} {2} г.",
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Day,
                            Library.MyMonthName(((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Month),
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Year);
                        m_Sheet.Cells[2, 7] = string.Format("{0}",
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).NUM_ORDER);
                    }
                    else
                    {
                        m_Sheet.Cells[2, 1] = string.Format("«   »                  20   г.",
                            DateTime.Today.Year);
                    }
                    /// Заносим код подразделения
                    m_Sheet.Cells[5, 1] = "О работе подразделения " + code_subdiv +
                        (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID == 124 ?
                        " в выходные и нерабочие дни" : " в сверхурочное время");

                    dtDate_For_Order.SelectCommand.Parameters["p_order_on_holiday_id"].Value =
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
                    dtDate_For_Order.Fill();
                    OracleDataTable dtEmp_For_Order = new OracleDataTable("", Connect.CurConnect);
                    OracleDataAdapter odaEmp_For_Order = new OracleDataAdapter("", Connect.CurConnect);
                    odaEmp_For_Order.SelectCommand.BindByName = true;
                    string strDW1 = "";
                    string strDW2 = "";
                    string strDW3 = "";
                    string strDW4 = "";
                    // Колонка для вставки данных: 64 - код колонки A, 27 - код колонки AA, начинаем с нее.
                    int col = 64 + 6;
                    string cell1, cell2;
                    WExcel.Range r_titlecolumn;
                    foreach (DataRow row_date in dtDate_For_Order.Rows)
                    {
                        StreamReader srQuery = new StreamReader(
                            Application.StartupPath + "/Queries/Table/SubQuaryEmp_For_Order.sql",
                            Encoding.GetEncoding(1251));
                        strDW1 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW2 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW3 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW4 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        srQuery.Close();                        
                        odaEmp_For_Order.SelectCommand.Parameters.Add(
                            "p_date" + row_date["DATE_FOR_ORDER_ID"], OracleDbType.Date).Value =
                            row_date["DATE_WORK_ORDER"];

                        /* 11.02.2016 - изменилась форма приказа - добавлены поля подпись сотрудника и дата подписания*/
                        m_Sheet.get_Range(Excel.ParseColNum(col) + ":" + Excel.ParseColNum(col)).Columns.Insert(
                            WExcel.XlInsertShiftDirection.xlShiftToRight, WExcel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                        m_Sheet.get_Range(Excel.ParseColNum(col) + ":" + Excel.ParseColNum(col)).ColumnWidth = 4.3;
                        m_Sheet.Cells[12, col - 64] = "Дата";
                        /* 11.02.2016 */

                        cell1 = Excel.ParseColNum(col++) + "13";
                        r_titlecolumn = m_Sheet.get_Range(cell1, Type.Missing);
                        r_titlecolumn.Borders.LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r_titlecolumn.Font.Size = 9;
                        r_titlecolumn.Value2 =
                            Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToString("dd.MM.") + "\n" +
                            Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToString("yyyy");
                        m_Sheet.get_Range("E:E").ColumnWidth =
                            Convert.ToDouble(m_Sheet.get_Range("E:E").ColumnWidth) - 5; // 5.3 ;
                    }
                    /* Настраиваем высоту строки основания приказа. 
                        * Для этого в ячейку не входящую в отчет, помещаем основание и ставим автоподбор ширины.
                        * После этого берем высоту той ячейки - она будет нашей искомой высотой*/
                    int row = 7;
                    WExcel.Range r_setting, r_data;
                    string _add_for_base = Queries.GetQuery("Table/Add_For_Base_Order_Holiday.txt").Replace("\r", "");
                    string st1 = _add_for_base + tbBase.Text.Replace("\r", "").Trim('\n');
                    object i1;
                    while (st1.IndexOf("\n") != -1)
                    {
                        r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                        r_data.Insert(WExcel.XlDirection.xlDown);
                        r_setting = m_Sheet.get_Range("GG4");
                        r_setting.ColumnWidth = 90;
                        r_setting.WrapText = true;
                        r_setting.Rows.AutoFit();
                        r_setting.Value2 = st1.Substring(0, st1.IndexOf("\n")).TrimEnd();
                        i1 = r_setting.RowHeight;
                        r_setting.Value2 = "";
                        r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                        r_data.Merge();
                        r_data.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                        r_data.HorizontalAlignment = WExcel.XlHAlign.xlHAlignJustify;
                        r_data.WrapText = true;
                        r_data.RowHeight = i1;
                        /// Заносим основание
                        r_data.Value2 = st1.Substring(0, st1.IndexOf("\n")).TrimEnd();
                        st1 = st1.Substring(st1.IndexOf("\n") + 1);
                        row++;
                    }
                    ((BackgroundWorker)sender).ReportProgress(30);
                    r_setting = m_Sheet.get_Range("GG4");
                    r_setting.ColumnWidth = 90;
                    r_setting.WrapText = true;
                    r_setting.Rows.AutoFit();
                    r_setting.Value2 = st1;
                    i1 = r_setting.RowHeight;
                    r_setting.Value2 = "";
                    r_setting = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                    r_setting.Merge();
                    r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                    r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignJustify;
                    r_setting.WrapText = true;
                    r_setting.RowHeight = i1;
                    /// Заносим основание
                    r_setting.Value2 = st1.Replace("\r", "").Trim();

                    odaEmp_For_Order.SelectCommand.CommandText =
                        string.Format(Queries.GetQuery("Table/SelectEmp_For_Order.sql"),
                        DataSourceScheme.SchemeName, strDW1, strDW2, strDW3, strDW4);
                    odaEmp_For_Order.SelectCommand.Parameters.Add("DFO_id",
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]
                        ).Current).ORDER_ON_HOLIDAY_ID);
                    odaEmp_For_Order.SelectCommand.Parameters.Add("p_subdiv_id",
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]
                        ).Current).SUBDIV_ID);
                    odaEmp_For_Order.Fill(dtEmp_For_Order);
                    row += 7;
                    cell1 = "A" + row.ToString();
                    string[,] val = new string[dtEmp_For_Order.Rows.Count, dtEmp_For_Order.Columns.Count];
                    for (int i = 0; i < dtEmp_For_Order.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtEmp_For_Order.Columns.Count; j++)
                        {
                            val[i, j] = dtEmp_For_Order.Rows[i][j].ToString();
                        }
                        row++;
                    }
                    ((BackgroundWorker)sender).ReportProgress(70);
                    cell2 = Excel.ParseColNum(col - 1) + (row - 1).ToString();
                    m_Sheet.get_Range(cell1, cell2).set_Value(Type.Missing, val);
                    // Добавляю 2 колонки чтобы подпись и дата тоже были с границами
                    cell2 = Excel.ParseColNum(col + 1) + (row - 1).ToString();
                    m_Sheet.get_Range(cell1, cell2).Borders.LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    row += 1;
                    string strP2 = "2. Оплату за работу произвести в соответствии с ТК РФ.";
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Value2 = strP2;
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Font.Size = 12;
                    row += 2;
                    strP2 = "3. Ответственным за соблюдение ППБ и ТБ назначить: ";
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Value2 = strP2;
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Font.Size = 12;
                    foreach (DataRow row_date in dtDate_For_Order.Rows)
                    {
                        row++;
                        strP2 = Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToShortDateString() + " - " +
                            row_date["RESPONSIBLE_WORKER"].ToString();
                        r_setting = m_Sheet.get_Range("GG4");
                        r_setting.ColumnWidth = 90;
                        r_setting.WrapText = true;
                        r_setting.Rows.AutoFit();
                        r_setting.Value2 = strP2;
                        i1 = r_setting.RowHeight;
                        r_setting.Value2 = "";
                        r_setting = m_Sheet.get_Range("A" + row.ToString() + ":E" + row.ToString());
                        r_setting.Merge();
                        r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                        r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignLeft;
                        r_setting.WrapText = true;
                        r_setting.RowHeight = i1;
                        r_setting.Font.Size = 12;
                        /// Заносим основание
                        r_setting.Value2 = strP2;
                    }
                    ((BackgroundWorker)sender).ReportProgress(95);
                    row += 3;
                    // Настраиваем высоту строки для наименования должности подписывающего
                    r_setting = m_Sheet.get_Range("GG4");
                    r_setting.ColumnWidth = 30;
                    r_setting.WrapText = true;
                    r_setting.Rows.AutoFit();
                    r_setting.Value2 = s_pos[0][0].ToString();
                    r_setting.Font.Size = 12;
                    i1 = r_setting.RowHeight;
                    r_setting.Value2 = "";
                    // Настраиваем ячейки для наименования должности подписывающего
                    r_setting = m_Sheet.get_Range("A" + row.ToString() + ":D" + row.ToString());
                    r_setting.Merge();
                    r_setting.WrapText = true;
                    r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                    r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignLeft;
                    r_setting.RowHeight = i1;
                    r_setting.Value2 = s_pos[0][0].ToString();
                    r_setting.Font.Size = 12;
                    // Настраиваем ячейки для ФИО подписывающего
                    r_setting = m_Sheet.get_Range(Excel.ParseColNum(col + 1) + row.ToString(), Type.Missing);
                    r_setting.WrapText = false;
                    r_setting.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    r_setting.Value2 = s_pos[0][1].ToString();
                    r_setting.Font.Size = 12;
                    m_Sheet.get_Range("A1", Type.Missing).Select();
                }
                else
                {
                    PathOfTemplate = Application.StartupPath + @"\Reports\Table_HolidaySub_New.xlt";
                    m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                    m_ExcelApp.ScreenUpdating = false;
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                    /// Заносим код подразделения
                    m_Sheet.Cells[1, 1] = subdiv_name;
                    if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER != null)
                    {
                        m_Sheet.Cells[4, 1] = string.Format("от \" {0} \"  {1}  {2} г. № {3}",
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Day,
                            Library.MyMonthName(((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Month),
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_ORDER.Value.Year,
                            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).NUM_ORDER);
                    }
                    else
                    {
                        m_Sheet.Cells[4, 1] = string.Format("от \"__\"__________ {0} г.№____",
                            DateTime.Today.Year);
                    }
                    m_Sheet.Cells[7, 1] =
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID == 124 ?
                        "О работе в выходные и нерабочие дни" : "О работе в сверхурочное время";

                    dtDate_For_Order.SelectCommand.Parameters["p_order_on_holiday_id"].Value =
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
                    dtDate_For_Order.Fill();
                    OracleDataTable dtEmp_For_Order = new OracleDataTable("", Connect.CurConnect);
                    OracleDataAdapter odaEmp_For_Order = new OracleDataAdapter("", Connect.CurConnect);
                    odaEmp_For_Order.SelectCommand.BindByName = true;
                    string strDW1 = "";
                    string strDW2 = "";
                    string strDW3 = "";
                    string strDW4 = "";
                    // Колонка для вставки данных: 64 - код колонки A, 27 - код колонки AA, начинаем с нее.
                    int col = 64 + 6;
                    string cell1, cell2;
                    WExcel.Range r_titlecolumn;
                    foreach (DataRow row_date in dtDate_For_Order.Rows)
                    {
                        StreamReader srQuery = new StreamReader(
                            Application.StartupPath + "/Queries/Table/SubQuaryEmp_For_Order.sql",
                            Encoding.GetEncoding(1251));
                        strDW1 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW2 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW3 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        strDW4 += ", " + string.Format(
                            srQuery.ReadLine(), row_date["DATE_FOR_ORDER_ID"]);
                        srQuery.Close();
                        odaEmp_For_Order.SelectCommand.Parameters.Add(
                            "p_date" + row_date["DATE_FOR_ORDER_ID"], OracleDbType.Date).Value =
                            row_date["DATE_WORK_ORDER"];

                        /* 11.02.2016 - изменилась форма приказа - добавлены поля подпись сотрудника и дата подписания*/
                        m_Sheet.get_Range(Excel.ParseColNum(col) + ":" + Excel.ParseColNum(col)).Columns.Insert(
                            WExcel.XlInsertShiftDirection.xlShiftToRight, WExcel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                        m_Sheet.get_Range(Excel.ParseColNum(col) + ":" + Excel.ParseColNum(col)).ColumnWidth = 4.3;
                        m_Sheet.Cells[14, col - 64] = "Дата";
                        /* 11.02.2016 */

                        cell1 = Excel.ParseColNum(col++) + "15";
                        r_titlecolumn = m_Sheet.get_Range(cell1, Type.Missing);
                        r_titlecolumn.Borders.LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r_titlecolumn.Font.Size = 9;
                        r_titlecolumn.Value2 =
                            Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToString("dd.MM.") + "\n" +
                            Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToString("yyyy");
                        m_Sheet.get_Range("E:E").ColumnWidth =
                            Convert.ToDouble(m_Sheet.get_Range("E:E").ColumnWidth) - 5; // 5.3;
                    }
                    /* Настраиваем высоту строки основания приказа. 
                        * Для этого в ячейку не входящую в отчет, помещаем основание и ставим автоподбор ширины.
                        * После этого берем высоту той ячейки - она будет нашей искомой высотой*/
                    int row = 9;
                    WExcel.Range r_setting, r_data;
                    string st1 = tbBase.Text.Replace("\r", "").Trim('\n');
                    object i1;
                    while (st1.IndexOf("\n") != -1)
                    {
                        //r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col - 1) + row.ToString());
                        r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                        r_data.Insert(WExcel.XlDirection.xlDown);
                        r_setting = m_Sheet.get_Range("GG4");
                        r_setting.ColumnWidth = 90;
                        r_setting.WrapText = true;
                        r_setting.Rows.AutoFit();
                        r_setting.Value2 = st1.Substring(0, st1.IndexOf("\n")).TrimEnd();
                        i1 = r_setting.RowHeight;
                        r_setting.Value2 = "";
                        //r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col - 1) + row.ToString());
                        r_data = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                        r_data.Merge();
                        r_data.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                        r_data.HorizontalAlignment = WExcel.XlHAlign.xlHAlignJustify;
                        r_data.WrapText = true;
                        r_data.RowHeight = i1;
                        /// Заносим основание
                        r_data.Value2 = st1.Substring(0, st1.IndexOf("\n")).TrimEnd();
                        st1 = st1.Substring(st1.IndexOf("\n") + 1);
                        row++;
                    }
                    ((BackgroundWorker)sender).ReportProgress(30);
                    r_setting = m_Sheet.get_Range("GG4");
                    r_setting.ColumnWidth = 90;
                    r_setting.WrapText = true;
                    r_setting.Rows.AutoFit();
                    r_setting.Value2 = st1;
                    i1 = r_setting.RowHeight;
                    r_setting.Value2 = "";
                    r_setting = m_Sheet.get_Range("A" + row.ToString() + ":" + Excel.ParseColNum(col + 1) + row.ToString());
                    r_setting.Merge();
                    r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                    r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignJustify;
                    r_setting.WrapText = true;
                    r_setting.RowHeight = i1;
                    /// Заносим основание
                    r_setting.Value2 = st1.Replace("\r", "").Trim();

                    odaEmp_For_Order.SelectCommand.CommandText =
                        string.Format(Queries.GetQuery("Table/SelectEmp_For_Order.sql"),
                        DataSourceScheme.SchemeName, strDW1, strDW2, strDW3, strDW4);
                    odaEmp_For_Order.SelectCommand.Parameters.Add("DFO_id",
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]
                        ).Current).ORDER_ON_HOLIDAY_ID);
                    odaEmp_For_Order.SelectCommand.Parameters.Add("p_subdiv_id",
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]
                        ).Current).SUBDIV_ID);
                    odaEmp_For_Order.Fill(dtEmp_For_Order);
                    row += 7;
                    cell1 = "A" + row.ToString();
                    string[,] val = new string[dtEmp_For_Order.Rows.Count, dtEmp_For_Order.Columns.Count];
                    for (int i = 0; i < dtEmp_For_Order.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtEmp_For_Order.Columns.Count; j++)
                        {
                            val[i, j] = dtEmp_For_Order.Rows[i][j].ToString();
                        }
                        row++;
                    }
                    ((BackgroundWorker)sender).ReportProgress(70);
                    cell2 = Excel.ParseColNum(col - 1) + (row - 1).ToString();
                    m_Sheet.get_Range(cell1, cell2).set_Value(Type.Missing, val);
                    // Добавляю 2 колонки чтобы подпись и дата тоже были с границами
                    cell2 = Excel.ParseColNum(col + 1) + (row - 1).ToString();
                    m_Sheet.get_Range(cell1, cell2).Borders.LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    row += 1;
                    string strP2 = "2. Оплату за работу произвести в соответствии с ТК РФ.";
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Value2 = strP2;
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Font.Size = 12;
                    row += 2;
                    strP2 = "3. Ответственным за соблюдение ППБ и ТБ назначить: ";
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Value2 = strP2;
                    m_Sheet.get_Range("A" + row.ToString(), Type.Missing).Font.Size = 12;
                    foreach (DataRow row_date in dtDate_For_Order.Rows)
                    {
                        row++;
                        strP2 = Convert.ToDateTime(row_date["DATE_WORK_ORDER"]).ToShortDateString() + " - " +
                            row_date["RESPONSIBLE_WORKER"].ToString();
                        r_setting = m_Sheet.get_Range("GG4");
                        r_setting.ColumnWidth = 90;
                        r_setting.WrapText = true;
                        r_setting.Rows.AutoFit();
                        r_setting.Value2 = strP2;
                        i1 = r_setting.RowHeight;
                        r_setting.Value2 = "";
                        r_setting = m_Sheet.get_Range("A" + row.ToString() + ":E" + row.ToString());
                        r_setting.Merge();
                        r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                        r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignLeft;
                        r_setting.WrapText = true;
                        r_setting.RowHeight = i1;
                        r_setting.Font.Size = 12;
                        /// Заносим основание
                        r_setting.Value2 = strP2;
                    } 
                    ((BackgroundWorker)sender).ReportProgress(95);
                    row += 3;
                    // Настраиваем высоту строки для наименования должности подписывающего
                    r_setting = m_Sheet.get_Range("GG4");
                    r_setting.ColumnWidth = 30;
                    r_setting.WrapText = true;
                    r_setting.Rows.AutoFit();
                    r_setting.Value2 = s_pos[0][0].ToString();
                    r_setting.Font.Size = 12;
                    i1 = r_setting.RowHeight;
                    r_setting.Value2 = "";
                    // Настраиваем ячейки для наименования должности подписывающего
                    r_setting = m_Sheet.get_Range("A" + row.ToString() + ":D" + row.ToString());
                    r_setting.Merge();
                    r_setting.WrapText = true;
                    r_setting.VerticalAlignment = WExcel.XlVAlign.xlVAlignTop;
                    r_setting.HorizontalAlignment = WExcel.XlHAlign.xlHAlignLeft;
                    r_setting.RowHeight = i1;
                    r_setting.Font.Size = 12;
                    r_setting.Value2 = s_pos[0][0].ToString();
                    // Настраиваем ячейки для ФИО подписывающего
                    r_setting = m_Sheet.get_Range(Excel.ParseColNum(col + 1) + row.ToString(), Type.Missing);
                    r_setting.WrapText = false;
                    r_setting.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    r_setting.Value2 = s_pos[0][1].ToString();
                    r_setting.Font.Size = 12;
                    m_Sheet.get_Range("A1", Type.Missing).Select();
                }
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Visible = true;
                m_Sheet.Protect("258", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }            
            #endregion
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbBase.Text == "" || tbBase.Text == null)
            {
                MessageBox.Show("Вы не ввели основание формирования приказа!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbBase.Focus();
                return;
            }
            if (tbOfficial.Text == "" || tbOfficial.Text == null)
            {
                MessageBox.Show("Вы не ввели должностное лицо для формирования приказа!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbOfficial.Focus();
                return;
            }
            if (cbPay_Type.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали тип приказа!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbPay_Type.Focus();
                return;
            }
            order_on_holiday.Save();
            Connect.Commit();
            tbBase.Enabled = false;
            tbNum_Order.Enabled = false;
            deDate_Order.Enabled = false;
            chSign_Order_Plant.Enabled = false;
            tbOfficial.Enabled = false;
            cbPay_Type.Enabled = false;
            btEditOrder.Enabled = true;
            btSave.Enabled = false;
            btOrder.Enabled = true;
            btAddDate_For_Order.Enabled = true;
            btDeleteDate_For_Order.Enabled = true;
            btEditDate_For_Order.Enabled = true;
        }

        private void btEditOrder_Click(object sender, EventArgs e)
        {
            if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER == null)
            {
                if (!((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).SIGN_SURNAMES_ORDER)
                {
                    chSign_Order_Plant.Enabled = true;
                    cbPay_Type.Enabled = true;
                }
                tbBase.Enabled = true;
                tbNum_Order.Enabled = true;
                deDate_Order.Enabled = true;
                tbOfficial.Enabled = true;
                btEditOrder.Enabled = false;
                btSave.Enabled = true;
                btOrder.Enabled = false;
                btAddDate_For_Order.Enabled = false;
                btDeleteDate_For_Order.Enabled = false;
                btEditDate_For_Order.Enabled = false;
            }
            else
            {
                MessageBox.Show("Нельзя редактировать закрытый приказ!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btAddDate_For_Order_Click(object sender, EventArgs e)
        {
            if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER == null)
            {
                date_for_order.Clear();
                date_for_order.AddNew();
                Date_For_Order date_for_ord = new Date_For_Order(date_for_order,
                    ((CurrencyManager)BindingContext[date_for_order]).Count,
                    (decimal)((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID,
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).SIGN_SURNAMES_ORDER,
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID,
                    null, subdiv_id);
                date_for_ord.ShowDialog();
                FillDate_for_order();
            }
            else
            {
                MessageBox.Show("Нельзя редактировать закрытый приказ!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btEditDate_For_Order_Click(object sender, EventArgs e)
        {
            if (dgDate_For_Order.Rows.Count > 0)
            {
                date_for_order.Clear();
                date_for_order.Fill("where date_for_order_id = " +
                    dgDate_For_Order.CurrentRow.Cells["DATE_FOR_ORDER_ID"].Value.ToString());
                Date_For_Order date_for_ord = new Date_For_Order(date_for_order, 
                    ((CurrencyManager)BindingContext[date_for_order]).Position,
                    (decimal)((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID,
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).SIGN_SURNAMES_ORDER,
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID,
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER,
                    subdiv_id);
                date_for_ord.ShowDialog();
                FillDate_for_order();
            }
        }

        private void btDeleteDate_For_Order_Click(object sender, EventArgs e)
        {
            if (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER == null)
            {
                if (dgDate_For_Order.Rows.Count > 0)
                {
                    if (MessageBox.Show("Удалить запись?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        ocDeleteDCO.Parameters["p_DATE_FOR_ORDER_ID"].Value =
                            dgDate_For_Order.CurrentRow.Cells["DATE_FOR_ORDER_ID"].Value;
                        ocDeleteDCO.ExecuteNonQuery();
                        Connect.Commit();
                        FillDate_for_order();
                    }
                }
            }
            else
            {
                MessageBox.Show("Нельзя редактировать закрытый приказ!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgDate_For_Order_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgDate_For_Order["DATE_WORK_ORDER", e.RowIndex].Value == DBNull.Value)
            {
                /// Красим в розовый цвет
                e.CellStyle.BackColor = Color.Red;
            }
        }

        private void btSaveDate_Closing_Click(object sender, EventArgs e)
        {
            if (dgDate_For_Order.Rows.Count > 0)
            {
                if (chDate_Closing_Order.Checked)
                {
                    string stError = "";
                    dtLimit.Clear();
                    odaLimit.SelectCommand.Parameters["p_pay_type_id"].Value =
                        ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID;
                    odaLimit.Fill(dtLimit);
                    foreach (DataRow row in dtLimit.Rows)
                    {
                        if (Convert.ToDecimal(row["PLAN"]) < Convert.ToDecimal(row["FACT"]))
                        {
                            stError += string.Format("период - {0}.{5}, категория - {1}, план = {2}, факт = {3}, часы по приказу = {4}\n",
                                row["NUM_MONTH"].ToString(), row["CODE_DEGREE"].ToString(), row["PLAN"].ToString(),
                                row["FACT"].ToString(), row["HOURS_ON_ORDER"].ToString(), row["NUM_YEAR"].ToString());
                        }
                    }
                    if (stError != "")
                    {
                        MessageBox.Show("Превышен лимит: \n" + stError,
                            "АСУ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    stError = "";
                    dtLimit.Clear();
                    odaLimit.SelectCommand.Parameters["p_pay_type_id"].Value =
                        (((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).PAY_TYPE_ID == 124 ? 304 : 306);
                    odaLimit.Fill(dtLimit);
                    foreach (DataRow row in dtLimit.Rows)
                    {
                        if (Convert.ToDecimal(row["PLAN"]) < Convert.ToDecimal(row["FACT303"]))
                        {
                            stError += string.Format("период - {0}.{5}, категория - {1}, план = {2}, факт = {3}, часы по приказу = {4}\n",
                                row["NUM_MONTH"].ToString(), row["CODE_DEGREE"].ToString(), row["PLAN"].ToString(),
                                row["FACT303"].ToString(), row["HOURS_ON_ORDER"].ToString(), row["NUM_YEAR"].ToString());
                        }
                    }
                    if (stError != "")
                    {
                        MessageBox.Show("Превышен лимит на работу в счет отгула: \n" + stError,
                            "АСУ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    deDate_Closing_Order.Date = DateTime.Now;
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER = DateTime.Now;                   
                }
                else
                {
                    deDate_Closing_Order.Date = null;
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER = null;
                }
                ocUpdateDCO.Parameters["p_date_closing_order"].Value = deDate_Closing_Order.Date;
                ocUpdateDCO.Parameters["p_SIGN_CHECK_LIMIT"].Value = 1;
                ocUpdateDCO.ExecuteNonQuery();
                Connect.Commit();
                MessageBox.Show("Данные сохранены", "АСУ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btClosingOrder_Click(object sender, EventArgs e)
        {
            if (dgDate_For_Order.Rows.Count > 0)
            {               
                deDate_Closing_Order.Date = DateTime.Now;
                ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).DATE_CLOSING_ORDER = DateTime.Now;
                ocUpdateDCO.Parameters["p_date_closing_order"].Value = deDate_Closing_Order.Date;
                ocUpdateDCO.Parameters["p_SIGN_CHECK_LIMIT"].Value = 0;
                ocUpdateDCO.ExecuteNonQuery();
                Connect.Commit();
                MessageBox.Show("Данные сохранены", "АСУ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btPrintCoupon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать талоны?\nЭто может занять продолжительное время.\nХотите продолжить?",
                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    PrintCoupon(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        ///  Формирование талонов
        /// </summary>
        /// <param name="data"></param>
        void PrintCoupon(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
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
                /// Создаем таблицу для дней работы по приказу
                OracleDataTable dtCoupon = new OracleDataTable("", Connect.CurConnect);
                dtCoupon.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectCoupons.sql"),
                    Connect.Schema);
                dtCoupon.SelectCommand.BindByName = true;
                dtCoupon.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value =
                    ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).ORDER_ON_HOLIDAY_ID;
                dtCoupon.Fill();
                ((BackgroundWorker)sender).ReportProgress(10);
                if (dtCoupon.Rows.Count > 0)
                {
                    // Колонка для вставки данных: 65 - код колонки A, начинаем с нее.
                    int col = 65, row = 1, numSheet = 2;
                    string cell1, cell2;
                    string[,] val = new string[14, 8];
                    val[2, 5] = "Ф.о. 114-017";
                    val[3, 1] = "ТАЛОН";
                    val[4, 1] = "для работы в выходные дни";
                    val[5, 1] = "Ф.";
                    val[6, 1] = "И.";
                    val[7, 1] = "О.";
                    val[8, 1] = "Цех (отдел)";
                    val[9, 1] = "Действителен на";
                    val[10, 1] = "с";
                    val[11, 1] = "до";
                    val[12, 1] = "Нач. цеха (отд.)";
                    val[10, 3] = "час.";
                    val[11, 3] = "час.";
                    val[10, 5] = "мин.";
                    val[11, 5] = "мин.";
                    val[13, 4] = "подпись, печать";
                    m_Book = m_Books.Open(Application.StartupPath + @"\Reports\Coupons.xlt", oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                    WExcel.Range r_copyTitle, r_paste;
                    /*m_ExcelApp.Visible = true; */
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                    m_ExcelApp.ScreenUpdating = false;
                    m_ExcelApp.DisplayAlerts = false;
                    int numSheetNew = numSheet = 1;
                    dtCoupon.DefaultView.RowFilter = "SIGN_ACTUAL_TIME_FOR_COUPON = 0";
                    if (dtCoupon.DefaultView.Count > 0)
                    {
                        // Копируем лист шаблона перед началом работы
                        ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                        ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                        // Задаем рабочую облать - новый рабочий лист
                        m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                        m_Sheet.Name = "Талон" + (numSheet - 1).ToString();
                        // Задаем область копирования - лист шаблона и копируем ее
                        r_copyTitle = ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).get_Range("A1", "H17");
                        // Определяем область копирования и вставки - изначально первый талон.
                        // Сначала надо вставить один талон, чтобы задать формат. После этого копируем область вставки в область копирования.
                        // Именно она будет использоваться в последующих операциях
                        r_copyTitle.Copy(Type.Missing);
                        for (int i = 0; i < dtCoupon.DefaultView.Count; i++)
                        {
                            cell1 = Excel.ParseColNum(col) + row.ToString();
                            cell2 = Excel.ParseColNum(col + 7) + (row + 13).ToString();
                            r_paste = m_Sheet.get_Range(cell1, cell2);
                            r_paste.PasteSpecial(WExcel.XlPasteType.xlPasteFormats, WExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone,
                                false, false);
                            m_Sheet.Paste(r_paste, Type.Missing);
                            val[5, 2] = dtCoupon.DefaultView[i]["EMP_LAST_NAME"].ToString();
                            val[6, 2] = dtCoupon.DefaultView[i]["EMP_FIRST_NAME"].ToString();
                            val[7, 2] = dtCoupon.DefaultView[i]["EMP_MIDDLE_NAME"].ToString();
                            val[8, 4] = dtCoupon.DefaultView[i]["CODE_SUBDIV"].ToString() +
                                (dtCoupon.DefaultView[i]["NGM"] == DBNull.Value ? "" : " (" + dtCoupon.DefaultView[i]["NGM"].ToString() + ")");
                            val[9, 4] = dtCoupon.DefaultView[i]["DATE_WORK"].ToString();
                            val[10, 2] = dtCoupon.DefaultView[i]["HOURS_BEGIN"].ToString();
                            val[11, 2] = dtCoupon.DefaultView[i]["HOURS_END"].ToString();
                            val[10, 4] = dtCoupon.DefaultView[i]["MINUTE_BEGIN"].ToString();
                            val[11, 4] = dtCoupon.DefaultView[i]["MINUTE_END"].ToString();
                            r_paste.set_Value(Type.Missing, val);
                            // Если мы вставили 120 талонов (20 листов), то надо перейти на новый лист
                            if ((i + 1) % 120 == 0)
                            {
                                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                                m_Sheet.Name = "Талон" + (numSheet - 1).ToString();
                                col = 65; row = 1;
                                r_copyTitle.Copy(Type.Missing);
                            }
                            else
                            {
                                // Если количество вставленных талонов кратно 3, то мы должны начать с 1 столбца, иначе прибавляем заполненные столбцы
                                if ((i + 1) % 3 == 0)
                                {
                                    col = 65;
                                    row += 17;
                                    r_paste = m_Sheet.get_Range(row.ToString() + ":" + (row + 1).ToString());
                                    r_paste.RowHeight = 3.75;
                                    //if ((i + 1) % 9 == 0)
                                    //m_Sheet.HPageBreaks.Add(r_paste);
                                }
                                else
                                {
                                    col += 9;
                                    ((BackgroundWorker)sender).ReportProgress(90 * (i + 1) / dtCoupon.DefaultView.Count + 10);
                                }
                            }
                        }
                    }
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Select();
                    //r_copyTitle.Delete();
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Delete();
                    col = 65; 
                    row = 1;
                    numSheetNew = numSheet;
                    dtCoupon.DefaultView.RowFilter = "SIGN_ACTUAL_TIME_FOR_COUPON = 1";
                    if (dtCoupon.DefaultView.Count > 0)
                    {
                        // Копируем лист шаблона перед началом работы
                        ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                        ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                        // Задаем рабочую облать - новый рабочий лист
                        m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                        m_Sheet.Name = "Талон" + (numSheet - 1).ToString();
                        // Задаем область копирования - лист шаблона и копируем ее
                        r_copyTitle = ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).get_Range("A1", "H14");
                        // Определяем область копирования и вставки - изначально первый талон.
                        // Сначала надо вставить один талон, чтобы задать формат. После этого копируем область вставки в область копирования.
                        // Именно она будет использоваться в последующих операциях
                        r_copyTitle.Copy(Type.Missing);
                        for (int i = 0; i < dtCoupon.DefaultView.Count; i++)
                        {
                            cell1 = Excel.ParseColNum(col) + row.ToString();
                            cell2 = Excel.ParseColNum(col + 7) + (row + 13).ToString();
                            r_paste = m_Sheet.get_Range(cell1, cell2);
                            r_paste.PasteSpecial(WExcel.XlPasteType.xlPasteFormats, WExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone,
                                false, false);
                            m_Sheet.Paste(r_paste, Type.Missing);
                            val[5, 2] = dtCoupon.DefaultView[i]["EMP_LAST_NAME"].ToString();
                            val[6, 2] = dtCoupon.DefaultView[i]["EMP_FIRST_NAME"].ToString();
                            val[7, 2] = dtCoupon.DefaultView[i]["EMP_MIDDLE_NAME"].ToString();
                            val[8, 4] = dtCoupon.DefaultView[i]["CODE_SUBDIV"].ToString() +
                                (dtCoupon.DefaultView[i]["NGM"] == DBNull.Value ? "" : " (" + dtCoupon.DefaultView[i]["NGM"].ToString() + ")");
                            val[9, 4] = dtCoupon.DefaultView[i]["DATE_WORK"].ToString();
                            val[10, 2] = dtCoupon.DefaultView[i]["HOURS_BEGIN"].ToString();
                            val[11, 2] = dtCoupon.DefaultView[i]["HOURS_END"].ToString();
                            val[10, 4] = dtCoupon.DefaultView[i]["MINUTE_BEGIN"].ToString();
                            val[11, 4] = dtCoupon.DefaultView[i]["MINUTE_END"].ToString();
                            r_paste.set_Value(Type.Missing, val);
                            // Если мы вставили 180 талонов (20 листов), то надо перейти на новый лист
                            if ((i + 1) % 180 == 0)
                            {
                                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                                ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                                m_Sheet.Name = "Талон" + (numSheet - 1).ToString();
                                col = 65; row = 1;
                                r_copyTitle.Copy(Type.Missing);
                            }
                            else
                            {
                                // Если количество вставленных талонов кратно 3, то мы должны начать с 1 столбца, иначе прибавляем заполненные столбцы
                                if ((i + 1) % 3 == 0)
                                {
                                    col = 65;
                                    row += 14;
                                    r_paste = m_Sheet.get_Range(row.ToString() + ":" + (row + 1).ToString());
                                    r_paste.RowHeight = 3.75;
                                }
                                else
                                {
                                    col += 9;
                                    ((BackgroundWorker)sender).ReportProgress(90 * (i + 1) / dtCoupon.DefaultView.Count + 10);
                                }
                            }
                        }
                    }
                    m_ExcelApp.DisplayAlerts = false;
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                    m_ExcelApp.ScreenUpdating = true;
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Select();
                    //r_copyTitle.Delete();
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheetNew]).Delete();
                    /*((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Select();*/
                    /*m_Sheet.get_Range("A1", Type.Missing).Select();
                    m_Sheet.Protect("258", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);*/
                    /*m_ExcelApp.DisplayAlerts = false;
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                    m_ExcelApp.ScreenUpdating = true;*/
                    m_ExcelApp.Visible = true;
                    if (!GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
                    {
                        // Начинаем цикл по листам книги со 2 листа (1 - шаблон, который скрыт)
                        for (int i = 1; i < numSheet; i++)
                        {
                            m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[i];
                            m_Sheet.PrintPreview(true);
                        }                        
                        m_ExcelApp.Quit();
                    }
                }
                else
                {
                    MessageBox.Show("По текущему приказу нет данных о работниках!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
