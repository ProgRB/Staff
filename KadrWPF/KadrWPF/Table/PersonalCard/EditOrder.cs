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
    public partial class EditOrder : Form
    {
        OracleDataTable dtEmp_Order;
        DataTable _dtOrders;
        OracleDataAdapter _daOrders;
        private int _order_id = -1;
        private string _order_name = "";
        private DateTime date_order;
        public int Order_ID_Property
        {
            get { return _order_id; }            
        }

        public string Order_Name_Property
        {
            get { return _order_name; }
        }
        public DateTime DATE_ORDER
        {
            get { return date_order; }
        }
        bool flagDate_Order;
        /// <summary>
        /// Форма выбора заказа
        /// </summary>
        /// <param name="_con">Подключение</param>
        /// <param name="flagDate_Order">Флаг определяет нужно ли вводить дату установки заказа</param>
        public EditOrder(bool _flagDate_Order)
        {
            InitializeComponent();
            flagDate_Order = _flagDate_Order;
            CreateTable();   
            lbDate_Order.Visible = flagDate_Order;
            deDate_Order.Visible = flagDate_Order;
            splitContainer1.Panel2Collapsed = true;
            this.Width = 320;
            this.Height = 444;
        }

        /// <summary>
        /// Форма выбора заказа
        /// </summary>
        /// <param name="_con">Подключение</param>
        /// <param name="flagDate_Order">Флаг определяет нужно ли вводить дату установки заказа</param>
        public EditOrder(bool _flagDate_Order, string _per_num, decimal _transfer_id)
        {
            InitializeComponent();
            flagDate_Order = _flagDate_Order;
            CreateTable();    
            dtEmp_Order = new OracleDataTable("", Connect.CurConnect);
            dtEmp_Order.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectEmp_Order.sql"), Connect.Schema);
            dtEmp_Order.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = _per_num;
            dtEmp_Order.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;            
            dtEmp_Order.Fill();
            dgOrderEmp.DataSource = dtEmp_Order;
            foreach (DataGridViewColumn column in dgOrderEmp.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        void CreateTable()
        {
            _daOrders = new OracleDataAdapter(string.Format(
                "SELECT ORDER_ID, ORDER_NAME FROM {0}.ORDERS ORDER BY ORDER_NAME",
                Connect.Schema), Connect.CurConnect);
            _dtOrders = new DataTable();
            _daOrders.Fill(_dtOrders);
            dgOrders.AutoGenerateColumns = false;
            dgOrders.DataSource = _dtOrders.DefaultView;  
            _daOrders.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.ORDERS_insert(:ORDER_ID,:ORDER_NAME);
                END;", Connect.Schema), Connect.CurConnect);
            _daOrders.InsertCommand.BindByName = true;
            _daOrders.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daOrders.InsertCommand.Parameters.Add("ORDER_ID", OracleDbType.Decimal, 0, "ORDER_ID").Direction = ParameterDirection.InputOutput;
            _daOrders.InsertCommand.Parameters["ORDER_ID"].DbType = DbType.Decimal;
            _daOrders.InsertCommand.Parameters.Add("ORDER_NAME", OracleDbType.Varchar2, 0, "ORDER_NAME");
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (flagDate_Order && deDate_Order.Date == null)
            {
                MessageBox.Show("Вы не ввели дату установки заказа!", 
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                deDate_Order.Focus();
                return;
            }
            if (_dtOrders.DefaultView.Count > 0)
            {
                if (dgOrders.CurrentRow != null)
                {
                    if (((flagDate_Order && Table.dCloseTable < deDate_Order.Date) || !flagDate_Order) || 
                        GrantedRoles.GetGrantedRole("TABLE_FORM_FILE"))
                    {
                        _order_id = Convert.ToInt32(dgOrders.CurrentRow.Cells["ORDER_ID"].Value);
                        _order_name = dgOrders.CurrentRow.Cells["ORDER_NAME"].Value.ToString();
                        if (flagDate_Order)
                        {
                            date_order = (DateTime)deDate_Order.Date;
                        }
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверная дата установки заказа - за указанный период табель закрыт!",
                            "АРМ \"Учет рабочего времени\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали заказ!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Вы ввели некорректный номер заказа!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btAddOrder_Click(object sender, EventArgs e)
        {
            OracleDataTable dtOrdersTemp = new OracleDataTable("", Connect.CurConnect);
            dtOrdersTemp.SelectCommand.CommandText = string.Format(
                "select ORDER_ID,ORDER_NAME from {0}.ORDERS where order_name = :p_order_name",
                Connect.Schema);
            dtOrdersTemp.SelectCommand.Parameters.Add("p_order_name", OracleDbType.Varchar2).Value =
                tbOrder_Name.Text.Trim();
            dtOrdersTemp.Fill();            
            if (dtOrdersTemp.Rows.Count > 0)
            {
                MessageBox.Show("Вы ввели номер заказа, который уже существует в справочнике!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Convert.ToDecimal(tbOrder_Name.Text.Trim());
                if (tbOrder_Name.Text.Length != 13)
                {
                    MessageBox.Show("Вы ввели короткий или длинный номер заказа!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Вы ввели некорректный номер заказа!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataRowView _drView = _dtOrders.DefaultView.AddNew();
            _drView["ORDER_NAME"] = tbOrder_Name.Text.Trim();
            _dtOrders.Rows.Add(_drView.Row);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daOrders.InsertCommand.Transaction = transact;
                _daOrders.Update(_dtOrders);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show("Ошибка добавления заказа!\n" + ex.Message,
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbOrder_Name_TextChanged(object sender, EventArgs e)
        {
            if (tbOrder_Name.Text.Trim() != "")
                _dtOrders.DefaultView.RowFilter = string.Format("order_name like '{0}%'", tbOrder_Name.Text.Trim());
            else
                _dtOrders.DefaultView.RowFilter = "";
        }
    }
}
