using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using MExcel= Microsoft.Office.Interop.Excel;
using Staff;
using Oracle.DataAccess.Client;
using Kadr;
namespace Tabel
{
    public partial class OrdersOnHoliday : System.Windows.Forms.UserControl
    {
        OracleDataTable dtOrders, dtPay_Type;
        private static DataTable _dtDate_for_order;
        public static DataTable DTDate_for_order
        {
            get { return _dtDate_for_order; }
        }
        private static OracleDataAdapter _odaDate_for_order;
        public static OracleDataAdapter ODADate_for_order
        {
            get { return _odaDate_for_order; }
        }
        public OrdersOnHoliday(int _subdiv_id)
        {            
            InitializeComponent();
            ssOrderOnHoliday.subdiv_id = _subdiv_id;
            CreateTable();
            dtpBegin.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpBegin.ValueChanged += new EventHandler(dtpBegin_ValueChanged);
            dtpEnd.ValueChanged += new EventHandler(dtpEnd_ValueChanged);
            dtpEnd.Value = dtpBegin.Value.AddMonths(1).AddDays(-1);
            dgDate_For_Order.DataSource = _dtDate_for_order;
            RefreshGrid();            
            dgOrder_On_Holiday.DoubleClick +=new EventHandler(btEditOrder_Click);
            dtPay_Type.Fill();
            cbPay_Type.DataSource = dtPay_Type;
            cbPay_Type.DisplayMember = "PAY_TYPE_NAME";
            cbPay_Type.ValueMember = "PAY_TYPE_ID";
            cbPay_Type.SelectedIndexChanged += new EventHandler(cbPay_Type_SelectedIndexChanged);
            /*if (GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))*/
            /* 01.04.2016 со слов Усольцевой М.Е. скрываем кнопку для всех кроме их бюро*/
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
            {
                btAddOrderOnFIO.Visible = true;
            }
        }

        static OrdersOnHoliday()
        {
            _dtDate_for_order = new DataTable();
            _odaDate_for_order = new OracleDataAdapter();
            _odaDate_for_order.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("Table/SelectDate_for_Order.sql"), Connect.Schema), Connect.CurConnect);
            _odaDate_for_order.SelectCommand.BindByName = true;
            _odaDate_for_order.SelectCommand.Parameters.Add("p_ORDER_ON_HOLIDAY_ID", OracleDbType.Decimal).Value = 0;
            _odaDate_for_order.Fill(_dtDate_for_order);
        }

        void cbPay_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillOrders();
        }

        void RefreshGrid()
        {
            dgOrder_On_Holiday.DataSource = dtOrders;
            foreach (DataGridViewColumn col in dgOrder_On_Holiday.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgOrder_On_Holiday.Columns["ORDER_ON_HOLIDAY_ID"].Visible = false;
            dgOrder_On_Holiday.Columns["DATE_CLOSING_ORDER"].HeaderText = "Закрыто";
            dgOrder_On_Holiday.Columns["DATE_CLOSING_ORDER"].Width = 80;
            dgOrder_On_Holiday.Columns["NUM_ORDER"].HeaderText = "№ приказа";
            dgOrder_On_Holiday.Columns["NUM_ORDER"].Width = 70;
            dgOrder_On_Holiday.Columns["DATE_ORDER"].HeaderText = "Дата приказа";
            dgOrder_On_Holiday.Columns["DATE_ORDER"].Width = 80;
            dgOrder_On_Holiday.Columns["SIGN_ORDER_PLANT"].HeaderText = "По заводу";
            dgOrder_On_Holiday.Columns["SIGN_ORDER_PLANT"].Width = 60;
            dgOrder_On_Holiday.Columns["SIGN_ORDER_PLANT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgOrder_On_Holiday.Columns["SIGN_SURNAMES_ORDER"].HeaderText = "Пофамильный приказ";
            dgOrder_On_Holiday.Columns["SIGN_SURNAMES_ORDER"].Width = 115;
            dgOrder_On_Holiday.Columns["SIGN_SURNAMES_ORDER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgOrder_On_Holiday.Columns["PAY_TYPE_NAME"].HeaderText = "Тип документа";
            dgOrder_On_Holiday.Columns["PAY_TYPE_NAME"].Width = 200;
            dgOrder_On_Holiday.Columns["BASE_ORDER"].HeaderText = "Основание приказа";
            dgOrder_On_Holiday.Columns["BASE_ORDER"].Width = 500;
            dgOrder_On_Holiday.Columns["OFFICIAL"].HeaderText = "Должностное лицо";
            dgOrder_On_Holiday.Columns["OFFICIAL"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgDate_For_Order.Columns["DATE_FOR_ORDER_ID"].Visible = false;
            dgDate_For_Order.Columns["RESPONSIBLE_WORKER"].HeaderText = "Ответственный за ППБ и ТБ";
            dgDate_For_Order.Columns["DATE_WORK_ORDER"].HeaderText = "Дата работы";
        }

        void CreateTable()
        {
            dtOrders = new OracleDataTable("", Connect.CurConnect);
            dtOrders.SelectCommand.CommandText = string.Format
                (Queries.GetQuery("Table/OrdersOnHoliday.sql"), DataSourceScheme.SchemeName);
            dtOrders.SelectCommand.Parameters.Add("subdiv_id", OracleDbType.Decimal);
            dtOrders.SelectCommand.Parameters.Add("d_b", OracleDbType.Date);
            dtOrders.SelectCommand.Parameters.Add("d_e", OracleDbType.Date);
            dtOrders.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);
            dgOrder_On_Holiday.DataSource = dtOrders;
            dtPay_Type = new OracleDataTable("", Connect.CurConnect);
            dtPay_Type.SelectCommand.CommandText = string.Format(
                "select 0 as PAY_TYPE_ID, '' as PAY_TYPE_NAME  from dual " +
                "union " +
                "select PAY_TYPE_ID, PAY_TYPE_NAME " +
                "from {0}.PAY_TYPE P " +
                "where P.PAY_TYPE_ID in (106, 124) " +
                "order by 1", DataSourceScheme.SchemeName);            
        }

        private void btEditOrder_Click(object sender, EventArgs e)
        {
            if (dgOrder_On_Holiday.Rows.Count > 0)
            {
                ORDER_ON_HOLIDAY_seq order_on_holiday = new ORDER_ON_HOLIDAY_seq(Connect.CurConnect);
                order_on_holiday.Fill("where order_on_holiday_id = " +
                    dgOrder_On_Holiday.CurrentRow.Cells["ORDER_ON_HOLIDAY_ID"].Value);
                OrderHoliday orderHoliday = new OrderHoliday(false, order_on_holiday,
                    ssOrderOnHoliday.subdiv_id.ToString(), ssOrderOnHoliday.CodeSubdiv, 
                    ssOrderOnHoliday.SubdivName);
                orderHoliday.ShowDialog();
                FillOrders();                
            }
        }

        private void dgOrder_On_Holiday_SelectionChanged(object sender, EventArgs e)
        {
            _dtDate_for_order.Clear();
            if (dgOrder_On_Holiday.CurrentRow != null)
            {                
                _odaDate_for_order.SelectCommand.Parameters["p_ORDER_ON_HOLIDAY_ID"].Value =
                    dgOrder_On_Holiday.CurrentRow.Cells["ORDER_ON_HOLIDAY_ID"].Value;
                _odaDate_for_order.Fill(_dtDate_for_order);
                if (dgDate_For_Order.Columns.IndexOf(dgDate_For_Order.Columns["DATE_FOR_ORDER_ID"]) != -1)
                    dgDate_For_Order.Columns["DATE_FOR_ORDER_ID"].Visible = false;
            }
        }
        
        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBegin.Value > dtpEnd.Value)
            {
                dtpBegin.Value = dtpEnd.Value;
            }
            FillOrders();
        }

        private void dtpBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBegin.Value > dtpEnd.Value)
            {
                dtpEnd.Value = dtpBegin.Value;
            }
            FillOrders();
        }

        void FillOrders()
        {
            _dtDate_for_order.Clear();
            dtOrders.Clear();
            dtOrders.SelectCommand.Parameters["subdiv_id"].Value = ssOrderOnHoliday.subdiv_id;
            dtOrders.SelectCommand.Parameters["d_b"].Value = dtpBegin.Value;
            dtOrders.SelectCommand.Parameters["d_e"].Value = dtpEnd.Value;
            dtOrders.SelectCommand.Parameters["p_pay_type_id"].Value = cbPay_Type.SelectedValue;
            dtOrders.Fill();
        }

        private void btAddOrder_Click(object sender, EventArgs e)
        {
            ORDER_ON_HOLIDAY_seq order_on_holiday = new ORDER_ON_HOLIDAY_seq(Connect.CurConnect);
            order_on_holiday.AddNew();
            _dtDate_for_order.Clear();
            ((ORDER_ON_HOLIDAY_obj)((CurrencyManager)BindingContext[order_on_holiday]).Current).SUBDIV_ID =
                ssOrderOnHoliday.subdiv_id;
            OrderHoliday orderHoliday = new OrderHoliday(true, order_on_holiday, 
                ssOrderOnHoliday.subdiv_id.ToString(), ssOrderOnHoliday.CodeSubdiv, ssOrderOnHoliday.SubdivName);
            orderHoliday.ShowDialog();
            FillOrders();            
        }

        private void btDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgOrder_On_Holiday.CurrentRow != null)
            {
                if (dgOrder_On_Holiday.CurrentRow.Cells["DATE_CLOSING_ORDER"].Value == DBNull.Value)
                {
                    if (MessageBox.Show("Удалить приказ о работе в выходные дни?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        OracleCommand delOrder = new OracleCommand(string.Format(
                            @"delete from {0}.ORDER_ON_HOLIDAY where ORDER_ON_HOLIDAY_ID = :p_ORDER_ON_HOLIDAY_ID",
                            Connect.Schema), Connect.CurConnect);
                        delOrder.BindByName = true;
                        delOrder.Parameters.Add("p_ORDER_ON_HOLIDAY_ID", OracleDbType.Decimal).Value =
                            dgOrder_On_Holiday.CurrentRow.Cells["ORDER_ON_HOLIDAY_ID"].Value;
                        delOrder.ExecuteNonQuery();
                        Connect.Commit();
                        FillOrders();
                        if (dtOrders.Rows.Count == 0)
                            _dtDate_for_order.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Нельзя удалить закрытый приказ!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void ssOrderOnHoliday_SubdivChanged(object sender, EventArgs e)
        {
            FillOrders();
        }

        private void btCopyOrder_Click(object sender, EventArgs e)
        {
            if (dgOrder_On_Holiday.Rows.Count > 0)
            {
                if (MessageBox.Show("Вы действительно хотите скопировать текущий приказ?",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                    System.Windows.Forms.DialogResult.Yes)
                {
                    OracleCommand ocCopyOrder = new OracleCommand(string.Format(
                        @"begin {0}.COPY_ORDER_ON_HOLIDAY(:p_ORDER_ON_HOLIDAY_ID); end;", Connect.Schema),
                        Connect.CurConnect);
                    ocCopyOrder.BindByName = true;
                    ocCopyOrder.Parameters.Add("p_ORDER_ON_HOLIDAY_ID", OracleDbType.Decimal).Value =
                        dgOrder_On_Holiday.CurrentRow.Cells["ORDER_ON_HOLIDAY_ID"].Value;
                    ocCopyOrder.ExecuteNonQuery();
                    Connect.Commit();
                    FillOrders();
                }
            }
        }

        private void btAddOrderOnFIO_Click(object sender, EventArgs e)
        {
            if (ssOrderOnHoliday.subdiv_id > 0)
            {
                if (MessageBox.Show("Вы действительно хотите добавить пофамильный приказ?",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    OracleCommand ocInsertSurnamesOrder = new OracleCommand("", Connect.CurConnect);
                    ocInsertSurnamesOrder.CommandText = string.Format(
                        @"INSERT INTO {0}.ORDER_ON_HOLIDAY(ORDER_ON_HOLIDAY_ID, SUBDIV_ID, PAY_TYPE_ID, SIGN_ORDER_PLANT, SIGN_SURNAMES_ORDER)
                        VALUES({0}.ORDER_ON_HOLIDAY_ID_SEQ.nextval, :p_SUBDIV_ID, 124, 1, 1)", Connect.Schema);
                    ocInsertSurnamesOrder.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = ssOrderOnHoliday.subdiv_id;
                    ocInsertSurnamesOrder.ExecuteNonQuery();
                    Connect.Commit();
                    FillOrders();
                }
            }
            else
            {
                MessageBox.Show("Для данной операции необходимо выбрать подразделение!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }        
    }
}
