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
using Kadr;

namespace Tabel
{ 
    public partial class Date_For_Order : Form
    {
        //List<CountHoursOnDegree> listHours;
        //OracleDataTable dtLimitOnSubdiv;
        DATE_FOR_ORDER_seq date_for_order;
        OracleDataTable dtEmp, dtEmpForPay, dtEmpForHoliday;
        OracleDataAdapter daLimit, daLimit304, daEmp;
        DataTable dtLimit, dtLimit304;
        OracleCommand ocSignHoliday, _ocSign_Order_Accessibility;
        bool _sign_Surnames_Order;
        decimal? _pay_type_id;
        decimal _order_on_hol_id;
        List<DataRow> rowsPay, rowsHoliday;
        public Date_For_Order(DATE_FOR_ORDER_seq date_for, int pos, 
            decimal order_on_hol_id, bool sign_Surnames_Order, decimal? pay_type_id, 
            DateTime? date_closing_order, int subdiv_id)
        {
            InitializeComponent();
            dgEmp.AutoGenerateColumns = false;
            dgEmpForPay.AutoGenerateColumns = false;
            dgEmpForHoliday.AutoGenerateColumns = false;
            date_for_order = date_for;
            _sign_Surnames_Order = sign_Surnames_Order;
            _pay_type_id = pay_type_id;
            _order_on_hol_id = order_on_hol_id;
            ((CurrencyManager)BindingContext[date_for_order]).Position = pos;
            ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).ORDER_ON_HOLIDAY_ID = _order_on_hol_id;
            deWork_Date.AddBindingSource(date_for_order, DATE_FOR_ORDER_seq.ColumnsName.DATE_WORK_ORDER);
            tbResp.AddBindingSource(date_for_order, DATE_FOR_ORDER_seq.ColumnsName.RESPONSIBLE_WORKER);

            OracleDataAdapter daLimitOnSubdiv = new OracleDataAdapter();
            daLimitOnSubdiv.SelectCommand = new OracleCommand("", Connect.CurConnect);
            daLimitOnSubdiv.SelectCommand.BindByName = true;

            dtLimit = new DataTable();
            daLimit = new OracleDataAdapter("", Connect.CurConnect);            
            daLimit.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/ViewLimitForDoc.sql"), Connect.Schema);
            daLimit.SelectCommand.BindByName = true;
            daLimit.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            daLimit.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = _pay_type_id;
            daLimit.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal).Value = null;
            daLimit.SelectCommand.Parameters.Add("p_date_doc", OracleDbType.Date);
            daLimit.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value = _order_on_hol_id;

            dtLimit304 = new DataTable();
            daLimit304 = new OracleDataAdapter("", Connect.CurConnect);
            daLimit304.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/ViewLimitForDoc.sql"), Connect.Schema);
            daLimit304.SelectCommand.BindByName = true;
            daLimit304.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            daLimit304.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = (_pay_type_id == 124 ? 304 : 306);
            daLimit304.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal).Value = null;
            daLimit304.SelectCommand.Parameters.Add("p_date_doc", OracleDbType.Date);
            daLimit304.SelectCommand.Parameters.Add("p_order_on_holiday_id", OracleDbType.Decimal).Value = _order_on_hol_id;
            
            ocSignHoliday = new OracleCommand("", Connect.CurConnect);
            ocSignHoliday.BindByName = true;
            ocSignHoliday.CommandText = string.Format(
                Queries.GetQuery("Table/NextHoliday.sql"), Connect.Schema);
            ocSignHoliday.Parameters.Add("p_date_work", OracleDbType.Date);           
            
            daEmp = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/Per_Num_and_FIO.sql"), Connect.Schema), 
                Connect.CurConnect);
            daEmp.SelectCommand.BindByName = true;
            daEmp.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = 0;
            daEmp.SelectCommand.Parameters.Add("p_DFO_ID", OracleDbType.Decimal).Value = 0;
            daEmp.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 0;
            daEmp.SelectCommand.Parameters.Add("p_WORK_DATE", OracleDbType.Date).Value = DateTime.Today;

            dtEmp = new OracleDataTable("", Connect.CurConnect);
            dtEmpForPay = new OracleDataTable("", Connect.CurConnect);
            dtEmpForHoliday = new OracleDataTable("", Connect.CurConnect);

            daEmp.Fill(dtEmpForPay);
            daEmp.Fill(dtEmpForHoliday);
            daEmp.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            daEmp.SelectCommand.Parameters["p_DFO_ID"].Value =
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID;
            daEmp.SelectCommand.Parameters["p_pay_type_id"].Value = pay_type_id;
            daEmp.SelectCommand.Parameters["p_WORK_DATE"].Value = 
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_WORK_ORDER;
            daEmp.Fill(dtEmp);
            dgEmp.DataSource = dtEmp;            
            dgEmpForPay.DataSource = dtEmpForPay;            
            dgEmpForHoliday.DataSource = dtEmpForHoliday;

            _ocSign_Order_Accessibility = new OracleCommand(string.Format(Queries.GetQuery("Table/Sign_Order_Accessibility.sql"),
                Connect.Schema), Connect.CurConnect);
            _ocSign_Order_Accessibility.BindByName = true;
            _ocSign_Order_Accessibility.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
            _ocSign_Order_Accessibility.Parameters.Add("p_PAY_TYPE_ID", OracleDbType.Decimal).Value = pay_type_id;
            _ocSign_Order_Accessibility.Parameters.Add("p_DATE_WORK", OracleDbType.Date);

            string code_subdiv = FormMain.allSubdiv.Where(s => s.SUBDIV_ID == subdiv_id).FirstOrDefault().CODE_SUBDIV.ToString();

            #region Добавление колонок в датагриды
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn c2 = new DataGridViewCheckBoxColumn();
            c1.Name = "per_num";
            c1.HeaderText = "Таб.№";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 60;
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["per_num"].DataPropertyName = "per_num";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "empl";
            c1.HeaderText = "ФИО сотрудника";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;     
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["empl"].DataPropertyName = "empl";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COMB";
            c1.HeaderText = "С.";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c1.Width = 20;
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["COMB"].DataPropertyName = "COMB";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "TRANSFER_ID";
            c1.Visible = false;
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["TRANSFER_ID"].DataPropertyName = "TRANSFER_ID";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "CODE_DEGREE";
            c1.HeaderText = "КТ";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 30;
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["CODE_DEGREE"].DataPropertyName = "CODE_DEGREE";
            dgEmp.Columns["CODE_DEGREE"].Name = "CODE_DEGREE";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COUNT_HOURS";
            c1.HeaderText = "Часы";
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 40;
            if (GrantedRoles.GetGrantedRole("TABLE_ECON_DEV"))
            {
                c1.ReadOnly = false;
            }
            else
            {
                c1.ReadOnly = true;
            }
            dgEmp.Columns.Add(c1);
            dgEmp.Columns["COUNT_HOURS"].DataPropertyName = "COUNT_HOURS";

            c2 = new DataGridViewCheckBoxColumn();
            c2.Name = "SIGN_ACTUAL_TIME";
            c2.HeaderText = "По факту";            
            c2.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c2.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c2.Width = 50;
            if (GrantedRoles.GetGrantedRole("TABLE_HOURS_ON_FACT"))
            {                
                if (code_subdiv == "028" && !GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
                    c2.ReadOnly = true;
                else
                    c2.ReadOnly = false;
            }
            else
            {
                c2.ReadOnly = true;
            }
            dgEmp.Columns.Add(c2);
            dgEmp.Columns["SIGN_ACTUAL_TIME"].DataPropertyName = "SIGN_ACTUAL_TIME";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "per_num";
            c1.HeaderText = "Таб.№";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 60;
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["per_num"].DataPropertyName = "per_num";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "empl";
            c1.HeaderText = "Сотрудники работающие в счет оплаты";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;       
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["empl"].DataPropertyName = "empl";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COMB";
            c1.HeaderText = "С.";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c1.Width = 20;
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["COMB"].DataPropertyName = "COMB";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "TRANSFER_ID";
            c1.Visible = false;
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["TRANSFER_ID"].DataPropertyName = "TRANSFER_ID";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "CODE_DEGREE";
            c1.HeaderText = "КТ";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 30;
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["CODE_DEGREE"].DataPropertyName = "CODE_DEGREE";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COUNT_HOURS";
            c1.HeaderText = "Часы";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 40;
            dgEmpForPay.Columns.Add(c1);
            dgEmpForPay.Columns["COUNT_HOURS"].DataPropertyName = "COUNT_HOURS";

            c2 = new DataGridViewCheckBoxColumn();
            c2.Name = "SIGN_ACTUAL_TIME";
            c2.HeaderText = "По факту";            
            c2.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c2.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c2.Width = 50;
            if (GrantedRoles.GetGrantedRole("TABLE_HOURS_ON_FACT"))
            {
                if (code_subdiv == "028" && !GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
                    c2.ReadOnly = true;
                else
                    c2.ReadOnly = false;
            }
            else
            {
                c2.ReadOnly = true;
            }
            dgEmpForPay.Columns.Add(c2);
            dgEmpForPay.Columns["SIGN_ACTUAL_TIME"].DataPropertyName = "SIGN_ACTUAL_TIME";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "per_num";
            c1.HeaderText = "Таб.№";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 60; 
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["per_num"].DataPropertyName = "per_num";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "empl";
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;     
            c1.HeaderText = "Сотрудники работающие в счет отгула";
            c1.ReadOnly = true;
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["empl"].DataPropertyName = "empl";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COMB";
            c1.HeaderText = "С.";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c1.Width = 20;
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["COMB"].DataPropertyName = "COMB";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "TRANSFER_ID";
            c1.Visible = false;
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["TRANSFER_ID"].DataPropertyName = "TRANSFER_ID";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "CODE_DEGREE";
            c1.HeaderText = "КТ";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 30;
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["CODE_DEGREE"].DataPropertyName = "CODE_DEGREE";

            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "COUNT_HOURS";
            c1.HeaderText = "Часы";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 40;
            dgEmpForHoliday.Columns.Add(c1);
            dgEmpForHoliday.Columns["COUNT_HOURS"].DataPropertyName = "COUNT_HOURS";

            c2 = new DataGridViewCheckBoxColumn();
            c2.Name = "SIGN_ACTUAL_TIME";
            c2.HeaderText = "По факту";
            c2.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c2.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c2.Width = 50;
            if (GrantedRoles.GetGrantedRole("TABLE_HOURS_ON_FACT"))
            {
                if (code_subdiv == "028" && !GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
                    c2.ReadOnly = true;
                else
                    c2.ReadOnly = false;
            }
            else
            {
                c2.ReadOnly = true;
            }
            dgEmpForHoliday.Columns.Add(c2);
            dgEmpForHoliday.Columns["SIGN_ACTUAL_TIME"].DataPropertyName = "SIGN_ACTUAL_TIME";

            #endregion

            rowsPay = new List<DataRow>();
            rowsHoliday = new List<DataRow>();

            foreach (DataRow row in dtEmp.Rows)
            {
                if (row["SIGN_HOLIDAY"] == DBNull.Value)
                {

                }
                else
                {                    
                    if (Convert.ToInt32(row["SIGN_HOLIDAY"]) == 0)
                    {
                        dtEmpForPay.ImportRow(row);
                        rowsPay.Add(row);
                    }
                    else
                    {
                        dtEmpForHoliday.ImportRow(row);
                        rowsHoliday.Add(row);
                    }
                }
            }

            foreach (DataRow row in rowsPay)
            {
                dtEmp.Rows.Remove(row);
            }
            foreach (DataRow row in rowsHoliday)
            {
                dtEmp.Rows.Remove(row);
            }

            if (date_closing_order != null)
            {
                btSave.Visible = false;
                btAddEmpForPay.Visible = false;
                btDeleteEmpForPay.Visible = false;
                btAddEmpForHoliday.Visible = false;
                btDeleteEmpForHoliday.Visible = false;
            }

            if (Connect.UserId.ToUpper() == "BMW12714")
                button1.Visible = true;
        }

        private void Date_For_Order_Load(object sender, EventArgs e)
        {
            CalcLimit();
            dgEmp.SelectionChanged += new EventHandler(dgEmp_SelectionChanged);
            if (dgEmp.CurrentRow != null &&
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_WORK_ORDER != null)
            {
                dtLimit.DefaultView.RowFilter = "CODE_DEGREE = '" + dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString() + "'";
                dtLimit304.DefaultView.RowFilter = dtLimit.DefaultView.RowFilter;
                if (dtLimit.DefaultView.Count > 0)
                {
                    lbInfo.Text = string.Format(
                        "Доступно для оплаты по кат. {0} = {1} ч.",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        Convert.ToDecimal(dtLimit.DefaultView[0]["PLAN"]) - Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]));
                }
                else
                {
                    lbInfo.Text = string.Format(
                        "Доступно для оплаты по кат. {0} = {1}",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        " -не установлено- ");
                }
                if (dtLimit304.DefaultView.Count > 0)
                {
                    lb303.Text = string.Format(
                        "Доступно в счет отгулов по кат. {0} = {1} ч.",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        Convert.ToDecimal(dtLimit304.DefaultView[0]["PLAN"]) - Convert.ToDecimal(dtLimit304.DefaultView[0]["FACT"]));
                }
                else
                {
                    lb303.Text = string.Format(
                        "Доступно в счет отгулов по кат. {0} = {1}",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        " -не установлено- ");
                }
            }
        }

        void dgEmp_SelectionChanged(object sender, EventArgs e)
        {
            if (dgEmp.CurrentRow != null && deWork_Date.Date != null)
            {
                dtLimit.DefaultView.RowFilter = "CODE_DEGREE = '" + dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString() + "'";
                dtLimit304.DefaultView.RowFilter = dtLimit.DefaultView.RowFilter;
                if (dtLimit.DefaultView.Count > 0)
                {
                    lbInfo.Text = string.Format(
                        "Доступно для оплаты по кат. {0} = {1} ч.",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        Convert.ToDecimal(dtLimit.DefaultView[0]["PLAN"]) - Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]));
                }
                else
                {
                    lbInfo.Text = string.Format(
                        "Доступно для оплаты по кат. {0} = {1}",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        " -не установлено- ");
                }
                if (dtLimit304.DefaultView.Count > 0)
                {
                    lb303.Text = string.Format(
                        "Доступно в счет отгулов по кат. {0} = {1} ч.",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        Convert.ToDecimal(dtLimit304.DefaultView[0]["PLAN"]) - Convert.ToDecimal(dtLimit304.DefaultView[0]["FACT"]));
                }
                else
                {
                    lb303.Text = string.Format(
                        "Доступно в счет отгулов по кат. {0} = {1}",
                        dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString(),
                        " -не установлено- ");
                }
            }
        }
        
        void CalcLimit()
        {
            dtLimit.Clear();
            if (deWork_Date.Date != null)
            {
                daLimit.SelectCommand.Parameters["p_date_doc"].Value = deWork_Date.Date;
                daLimit.Fill(dtLimit);
            }
            dtLimit304.Clear();
            if (deWork_Date.Date != null)
            {
                daLimit304.SelectCommand.Parameters["p_date_doc"].Value = deWork_Date.Date;
                daLimit304.Fill(dtLimit304);
            }
            /*decimal countHours = 0;
            foreach (DataRow row in dtLimit.Rows)
            {
                Decimal.TryParse(dtEmpForPay.Compute("SUM(COUNT_HOURS)", "CODE_DEGREE = '" + row["CODE_DEGREE"] + "'").ToString(), out countHours);
                row["FACT"] = Convert.ToDecimal(row["FACT"]) + countHours;
            }*/
            
            /*dtLimitOnSubdiv.Clear();
            dtLimitOnSubdiv.SelectCommand.Parameters["p_subdiv_id"].Value = FormMain.subdiv_id;
            dtLimitOnSubdiv.SelectCommand.Parameters["p_date_work"].Value =
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_WORK_ORDER;
            dtLimitOnSubdiv.SelectCommand.Parameters["p_pay_type_id"].Value = pay_type_id;
            dtLimitOnSubdiv.Fill();
            listHours.Clear();
            foreach (DataRow dataRow in dtLimitOnSubdiv.Rows)
            {
                listHours.Add(new CountHoursOnDegree(dataRow["code_degree"].ToString(),
                    Convert.ToDecimal(dataRow["LIMIT_HOURS_PLAN"]),
                    Convert.ToDecimal(dataRow["LIMIT_HOURS_FACT"])));
            }*/
        }

        void GetList_Emp()
        {
            dtEmp.Clear();
            daEmp.SelectCommand.Parameters["p_WORK_DATE"].Value = deWork_Date.Date.Value;
            daEmp.Fill(dtEmp);
        }

        private void deWork_Date_Validating(object sender, CancelEventArgs e)
        {
            if (deWork_Date.Date != null)
            {
                if (!SingAddOrder(deWork_Date.Date.Value))
                {
                    MessageBox.Show("Нельзя добавлять приказ на данную дату!\nДействует ограничение на работу в сверхурочное время и выходные дни.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = false;
                }
                CalcLimit();
                GetList_Emp();
                dtLimit.DefaultView.RowFilter = "CODE_DEGREE = '-1'";
                dgEmp_SelectionChanged(null, null);
            }
        }

        /// <summary>
        /// Проверка доступности формирования приказа
        /// </summary>
        /// <param name="dateOrder"></param>
        /// <returns></returns>
        bool SingAddOrder(DateTime dateOrder)
        {
            _ocSign_Order_Accessibility.Parameters["p_DATE_WORK"].Value = dateOrder;
            if (Convert.ToBoolean(_ocSign_Order_Accessibility.ExecuteScalar()))
                return true;
            return false;
        }


        private void btAddEmpForPay_Click(object sender, EventArgs e)
        {
            if (dgEmp.SelectedRows != null)
            {
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmp.SelectedRows)
                {
                    if (dtLimit.DefaultView.Count > 0)
                    {
                        if (_sign_Surnames_Order || Convert.ToDecimal(dtLimit.DefaultView[0]["PLAN"]) >=
                            (Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]) + Convert.ToDecimal(dtEmp.DefaultView[row.Index].Row["COUNT_HOURS"])))
                        {
                            dtLimit.DefaultView[0]["FACT"] = Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]) +
                                Convert.ToDecimal(dtEmp.DefaultView[row.Index].Row["COUNT_HOURS"]);
                            dtEmp.DefaultView[row.Index].Row["SIGN_HOLIDAY"] = 0;
                            dtEmp.DefaultView[row.Index].Row["DATE_FOR_ORDER_ID"] =
                                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID;
                            dtEmpForPay.ImportRow(dtEmp.DefaultView[row.Index].Row);
                            rowsPay.Add(dtEmp.DefaultView[row.Index].Row);
                        }
                        else
                        {
                            MessageBox.Show("Нельзя добавить работника данной категории в приказ!\n" +
                                "Превышен допустимый лимит часов.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("За указанный период не рассчитан лимит!\n" +
                                "Проверьте дату работы по приказу.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                }
                foreach (DataRow row in rowsPay)
                {
                    dtEmp.Rows.Remove(row);
                }
            }
        }

        private void btDeleteEmpForPay_Click(object sender, EventArgs e)
        {
            if (dgEmpForPay.SelectedRows != null)
            {
                string filterOld = dtLimit.DefaultView.RowFilter;
                dtLimit.DefaultView.RowFilter = "CODE_DEGREE = '" + dgEmpForPay.CurrentRow.Cells["CODE_DEGREE"].Value.ToString() + "'";
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmpForPay.SelectedRows)
                {
                    dtLimit.DefaultView[0]["FACT"] = Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]) -
                        Convert.ToDecimal(dtEmpForPay.DefaultView[row.Index].Row["COUNT_HOURS"]);
                    dtEmp.ImportRow(dtEmpForPay.Rows[row.Index]);
                    rowsPay.Add(dtEmpForPay.Rows[row.Index]);
                }
                foreach (DataRow row in rowsPay)
                {
                    dtEmpForPay.Rows.Remove(row);
                }
                dtLimit.DefaultView.RowFilter = filterOld;
                /// Если количество часов по плану больше количества часов по факту + часы 
                /// добавляемого работника, то все нормально и мы увеличиваем часы по факту.
                /// Иначе мы не можем добавить работника из-за того, что превышен лимит по 
                /// данной категории работников.
                /*25.09.2012 - с показаний о.3,
                    убираем проверку лимитов при добавлении работников в приказ:)*/
                //try
                //{
                //    listHours.Where(i =>
                //        i.Code_Degree == row.Cells["code_degree"].Value.ToString()).First().HoursFact =
                //        listHours.Where(i =>
                //            i.Code_Degree == row.Cells["code_degree"].Value.ToString()).First().HoursFact -
                //        Convert.ToInt32(row.Cells["COUNT_HOURS"].Value);                  
                //}
                //catch { }
                /*dgEmpForPay.Rows.Remove(row);
                dgEmp.Rows.Add(row);*/
            }
        }

        private void btAddEmpForHoliday_Click(object sender, EventArgs e)
        {
            if (dgEmp.SelectedRows != null)
            {
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmp.SelectedRows)
                {
                    if (dtLimit304.DefaultView.Count > 0)
                    {
                        if (_sign_Surnames_Order || Convert.ToDecimal(dtLimit304.DefaultView[0]["PLAN"]) >=
                            (Convert.ToDecimal(dtLimit304.DefaultView[0]["FACT"]) + Convert.ToDecimal(dtEmp.DefaultView[row.Index].Row["COUNT_HOURS"])))
                        {
                            dtLimit304.DefaultView[0]["FACT"] = Convert.ToDecimal(dtLimit304.DefaultView[0]["FACT"]) +
                                Convert.ToDecimal(dtEmp.DefaultView[row.Index].Row["COUNT_HOURS"]);
                            dtEmp.DefaultView[row.Index].Row["SIGN_HOLIDAY"] = 1;
                            dtEmp.DefaultView[row.Index].Row["DATE_FOR_ORDER_ID"] =
                                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID;
                            dtEmpForHoliday.ImportRow(dtEmp.DefaultView[row.Index].Row);
                            rowsPay.Add(dtEmp.DefaultView[row.Index].Row);
                        }
                        else
                        {
                            MessageBox.Show("Нельзя добавить работника данной категории в приказ!\n" +
                                "Превышен допустимый лимит часов.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("За указанный период не рассчитан лимит!\n" +
                                "Проверьте дату работы по приказу.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }

                }
                foreach (DataRow row in rowsPay)
                {
                    dtEmp.Rows.Remove(row);
                }   
            }
        }

        private void btDeleteEmpForHoliday_Click(object sender, EventArgs e)
        {
            if (dgEmpForHoliday.SelectedRows != null)
            {
                string filterOld = dtLimit304.DefaultView.RowFilter;
                dtLimit304.DefaultView.RowFilter = "CODE_DEGREE = '" + dgEmpForHoliday.CurrentRow.Cells["CODE_DEGREE"].Value.ToString() + "'";
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmpForHoliday.SelectedRows)
                {
                    dtLimit304.DefaultView[0]["FACT"] = Convert.ToDecimal(dtLimit304.DefaultView[0]["FACT"]) -
                        Convert.ToDecimal(dtEmpForHoliday.DefaultView[row.Index].Row["COUNT_HOURS"]);
                    dtEmp.ImportRow(dtEmpForHoliday.Rows[row.Index]);
                    rowsPay.Add(dtEmpForHoliday.Rows[row.Index]);
                }
                foreach (DataRow row in rowsPay)
                {
                    dtEmpForHoliday.Rows.Remove(row);
                }
                dtLimit304.DefaultView.RowFilter = filterOld;

            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbResp.Text.ToString() == "")
            {
                MessageBox.Show("Вы не ввели ответственного для формирования приказа!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbResp.Focus();
                return;
            }
            if (deWork_Date.Date == null)
            {
                MessageBox.Show("Вы не ввели дату работы для формирования приказа!",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deWork_Date.Focus();
                return;
            }
            else
            {
                OracleCommand com1 = new OracleCommand("", Connect.CurConnect);
                com1.BindByName = true;
                com1.CommandText = string.Format(
                    @"select C.TYPE_DAY_ID from {0}.CALENDAR C where C.CALENDAR_DAY = :work_date",
                    Connect.Schema);
                com1.Parameters.Add("work_date", OracleDbType.Date).Value = deWork_Date.Date;
                OracleDataReader odr1 = com1.ExecuteReader();
                int type_day = 0;
                while (odr1.Read())
                {
                    type_day = Convert.ToInt32(odr1[0]);
                }
                if (((type_day == 2 || type_day == 3) && _pay_type_id == 124) || 
                    ((type_day == 1 || type_day == 4) && _pay_type_id == 106))
                {
                    MessageBox.Show("Вы ввели некорректную дату работы!\nПроверьте тип документа и дату.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deWork_Date.Focus();
                    return;
                }
                com1 = new OracleCommand("", Connect.CurConnect);
                com1.BindByName = true;
                com1.CommandText = string.Format(
                    @"select count(*) from {0}.DATE_FOR_ORDER DFO 
                    where DFO.ORDER_ON_HOLIDAY_ID = :p_ORDER_ON_HOLIDAY_ID and 
                        DFO.DATE_WORK_ORDER = :p_DATE_WORK_ORDER and DFO.DATE_FOR_ORDER_ID != :p_DATE_FOR_ORDER_ID",
                    Connect.Schema);
                com1.Parameters.Add("p_ORDER_ON_HOLIDAY_ID", OracleDbType.Decimal).Value = _order_on_hol_id;
                com1.Parameters.Add("p_DATE_WORK_ORDER", OracleDbType.Date).Value = deWork_Date.Date;
                com1.Parameters.Add("p_DATE_FOR_ORDER_ID", OracleDbType.Decimal).Value = 
                    ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID;
                int countDay = Convert.ToInt16(com1.ExecuteScalar());
                if (countDay > 0)
                {
                    MessageBox.Show("Вы ввели некорректную дату работы!\nНа данную дату уже есть данные.",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deWork_Date.Focus();
                    return;
                }
                if (!SingAddOrder(deWork_Date.Date.Value))
                {
                    MessageBox.Show("Нельзя добавлять приказ на данную дату!\nДействует ограничение на работу в сверхурочное время и выходные дни.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (dgEmpForPay.RowCount == 0 && dgEmpForHoliday.RowCount == 0)
            {
                MessageBox.Show("Вы не выбрали ни одного работника.",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btSave.Focus();
                return;
            }
            date_for_order.Save();
            Connect.Commit();

            // 18.02.2016 - переделал сохранение сотрудников, чтобы они обновлялись, а не удалялись полностью и записывались снова
            OracleDataAdapter _daEmp_For_Order = new OracleDataAdapter();
            _daEmp_For_Order.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.EMP_FOR_ORDER_UPDATE(:EMP_FOR_ORDER_ID, :SIGN_HOLIDAY, :DATE_FOR_ORDER_ID, :TRANSFER_ID, :COUNT_HOURS, :SIGN_ACTUAL_TIME);
                END;", Connect.Schema), Connect.CurConnect);
            _daEmp_For_Order.InsertCommand.BindByName = true;
            _daEmp_For_Order.InsertCommand.Parameters.Add("EMP_FOR_ORDER_ID", OracleDbType.Decimal, 0, "EMP_FOR_ORDER_ID");
            _daEmp_For_Order.InsertCommand.Parameters.Add("SIGN_HOLIDAY", OracleDbType.Decimal, 0, "SIGN_HOLIDAY");
            _daEmp_For_Order.InsertCommand.Parameters.Add("DATE_FOR_ORDER_ID", OracleDbType.Decimal, 0, "DATE_FOR_ORDER_ID");
            _daEmp_For_Order.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daEmp_For_Order.InsertCommand.Parameters.Add("COUNT_HOURS", OracleDbType.Decimal, 0, "COUNT_HOURS");
            _daEmp_For_Order.InsertCommand.Parameters.Add("SIGN_ACTUAL_TIME", OracleDbType.Decimal, 0, "SIGN_ACTUAL_TIME");
            _daEmp_For_Order.UpdateCommand = _daEmp_For_Order.InsertCommand;
            _daEmp_For_Order.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.EMP_FOR_ORDER_DELETE(:EMP_FOR_ORDER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daEmp_For_Order.DeleteCommand.BindByName = true;
            _daEmp_For_Order.DeleteCommand.Parameters.Add("EMP_FOR_ORDER_ID", OracleDbType.Decimal, 0, "EMP_FOR_ORDER_ID");
            OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daEmp_For_Order.InsertCommand.Transaction = _transact;
                // Обновляем добавленные и отредактированные строчки
                _daEmp_For_Order.Update(dtEmpForPay);
                _daEmp_For_Order.Update(dtEmpForHoliday);                
                _transact.Commit();
            }
            catch(Exception ex) 
            {
                _transact.Rollback();
                MessageBox.Show("Ошибка обновления списка сотрудников для приказа:\n" +
                    ex.Message,
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            _transact = Connect.CurConnect.BeginTransaction();
            try
            {
                // Удаляем всех из списка сотрудников, чтобы удалились сотрудники, которые были в приказе и их исключили
                while (dgEmp.Rows.Count > 0)
                {
                    dgEmp.Rows.RemoveAt(0);
                }
                _daEmp_For_Order.Update(dtEmp);
                _transact.Commit();
            }
            catch(Exception ex)
            {
                _transact.Rollback();
                MessageBox.Show("Ошибка удаления сотрудников из приказа:\n" +
                    ex.Message,
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            /* 18.02.2016
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format("delete from {0}.EMP_FOR_ORDER where DATE_FOR_ORDER_ID = :date_for_order_id",
                Connect.Schema);
            com.Parameters.Add("date_for_order_id",
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID);
            com.ExecuteNonQuery();
            Connect.Commit();
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "insert into {0}.EMP_FOR_ORDER(EMP_FOR_ORDER_ID, DATE_FOR_ORDER_ID, TRANSFER_ID, SIGN_HOLIDAY, COUNT_HOURS, SIGN_ACTUAL_TIME) " +
                "values({0}.EMP_FOR_ORDER_ID_SEQ.nextval, :p_DATE_FOR_ORDER_ID, :p_TRANSFER_ID, :p_SIGN_HOLIDAY, :p_COUNT_HOURS, :p_SIGN_ACTUAL_TIME)",
                Connect.Schema);
            com.Parameters.Add("p_DATE_FOR_ORDER_ID",
                ((DATE_FOR_ORDER_obj)((CurrencyManager)BindingContext[date_for_order]).Current).DATE_FOR_ORDER_ID);
            com.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal);
            com.Parameters.Add("p_SIGN_HOLIDAY", OracleDbType.Decimal);
            com.Parameters.Add("p_COUNT_HOURS", OracleDbType.Decimal);
            com.Parameters.Add("p_SIGN_ACTUAL_TIME", OracleDbType.Decimal);
            com.Parameters["p_SIGN_HOLIDAY"].Value = 0;
            foreach (DataGridViewRow row in dgEmpForPay.Rows)
            {
                com.Parameters["p_TRANSFER_ID"].Value = row.Cells["TRANSFER_ID"].Value;
                com.Parameters["p_COUNT_HOURS"].Value = row.Cells["COUNT_HOURS"].Value;
                com.Parameters["p_SIGN_ACTUAL_TIME"].Value = row.Cells["SIGN_ACTUAL_TIME"].Value;
                com.ExecuteNonQuery();
            }
            com.Parameters["p_SIGN_HOLIDAY"].Value = 1;
            foreach (DataGridViewRow row in dgEmpForHoliday.Rows)
            {
                com.Parameters["p_TRANSFER_ID"].Value = row.Cells["TRANSFER_ID"].Value;
                com.Parameters["p_COUNT_HOURS"].Value = row.Cells["COUNT_HOURS"].Value;
                com.Parameters["p_SIGN_ACTUAL_TIME"].Value = row.Cells["SIGN_ACTUAL_TIME"].Value;
                com.ExecuteNonQuery();
            } 
             * 18.02.2016*/
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            date_for_order.RollBack();
            Close();
        }

        private void tbFilterEmp_TextChanged(object sender, EventArgs e)
        {
            if (tbFilterEmp.Text.Trim() != "")
            {
                dtEmp.DefaultView.RowFilter = "EMPL LIKE '" + tbFilterEmp.Text.Trim().ToUpper() + "%'";
            }
            else
            {
                dtEmp.DefaultView.RowFilter = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgEmp.SelectedRows != null)
            {
                rowsPay.Clear();
                foreach (DataGridViewRow row in dgEmp.SelectedRows)
                {
                    if (dtLimit.DefaultView.Count > 0)
                    {
                        dtLimit.DefaultView[0]["FACT"] = Convert.ToDecimal(dtLimit.DefaultView[0]["FACT"]) +
                            Convert.ToDecimal(dtEmp.DefaultView[row.Index].Row["COUNT_HOURS"]);
                        dtEmpForPay.ImportRow(dtEmp.DefaultView[row.Index].Row);
                        rowsPay.Add(dtEmp.DefaultView[row.Index].Row);
                    }
                    else
                    {
                        MessageBox.Show("За указанный период не рассчитан лимит!\n" +
                                "Проверьте дату работы по приказу.",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                }
                foreach (DataRow row in rowsPay)
                {
                    dtEmp.Rows.Remove(row);
                }
            }
        }
    }
}
