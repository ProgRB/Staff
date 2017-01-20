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
    public partial class EditWork_Pay_Type : Form
    {        
        decimal worked_day_id, transfer_id, work_pay_type_id;
        DataTable _dtEditable_Pay_Type;
        bool flagAdd;
        float seconds = 0;        
        public EditWork_Pay_Type(bool _flagAdd, decimal _work_pay_type_id,
            decimal _worked_day_id, decimal _transfer_id, DataTable dtEditable_Pay_Type,
            decimal _pay_type_id, Int32 _valid_time)
        {
            InitializeComponent();
            worked_day_id = _worked_day_id;
            transfer_id = _transfer_id;
            flagAdd = _flagAdd;
            work_pay_type_id = _work_pay_type_id;
            _dtEditable_Pay_Type = dtEditable_Pay_Type;
            teWork_Pay_Type.OnChangeTime += new EventHandler(TE_OnChangeTime);          

            cbPay_Type_Name.AddBindingSource(PAY_TYPE_seq.ColumnsName.PAY_TYPE_ID.ToString(),
                new LinkArgument(_dtEditable_Pay_Type, PAY_TYPE_seq.ColumnsName.PAY_TYPE_NAME));
            cbPay_Type_Name.SelectedIndexChanged += new EventHandler(cbPay_Type_SelectedIndexChanged);
            if (!flagAdd)
            {
                tbCode_Pay_Type.Text = _pay_type_id.ToString();
                cbPay_Type_Name.SelectedValue = _pay_type_id;
                teWork_Pay_Type.Seconds = _valid_time;
            }
        }

        void cbPay_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPay_Type_Name.SelectedValue != null)
            {
                tbCode_Pay_Type.Text = Library.CodeBySelectedValue(Connect.CurConnect, PAY_TYPE_seq.ColumnsName.CODE_PAY_TYPE.ToString(),
                    Staff.DataSourceScheme.SchemeName, "pay_type", PAY_TYPE_seq.ColumnsName.PAY_TYPE_ID.ToString(), cbPay_Type_Name.SelectedValue.ToString());
            }
        }


        void TE_OnChangeTime(object sender, EventArgs e)
        {
            seconds = ((TimeEditorClass)sender).Seconds;
        }

        private void btSave_Click(object sender, EventArgs e)
        { 
            if (tbCode_Pay_Type.Text.ToString() == "")
            {
                MessageBox.Show("Вы не выбрали вид оплат.\n",
                    "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            switch (tbCode_Pay_Type.Text.ToString())
            {
                case "101":
                    if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                    {
                        MessageBox.Show("Недостаточно прав для редактирования данного вида оплат!" +
                            "\nПо вопросам обращаться в отдел 5.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode_Pay_Type.Focus();
                        return;
                    }
                    break;
                case "102":
                    if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                    {
                        MessageBox.Show("Недостаточно прав для редактирования данного вида оплат!" +
                            "\nПо вопросам обращаться в отдел 5.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode_Pay_Type.Focus();
                        return;
                    }
                    break;
                case "112":
                    if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                    {
                        MessageBox.Show("Недостаточно прав для редактирования данного вида оплат!" +
                            "\nПо вопросам обращаться в отдел 3.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode_Pay_Type.Focus();
                        return;
                    }
                    break;
                case "110":
                    if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_PT"))
                    {
                        MessageBox.Show("Недостаточно прав для редактирования данного вида оплат!" +
                            "\nПо вопросам обращаться в отдел 3.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode_Pay_Type.Focus();
                        return;
                    }
                    OracleCommand ocCountHarmful = new OracleCommand(string.Format(
                        @"select count(*) from {0}.TRANSFER T
                        join {0}.ACCOUNT_DATA A on (A.TRANSFER_ID = DECODE(T.TYPE_TRANSFER_ID,3,T.FROM_POSITION,T.TRANSFER_ID))
                        where T.TRANSFER_ID = :p_transfer_id and A.HARMFUL_ADDITION_ADD > 0", Connect.Schema),
                        Connect.CurConnect);
                    ocCountHarmful.BindByName = true;
                    ocCountHarmful.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
                    int _count = Convert.ToInt16(ocCountHarmful.ExecuteScalar());
                    if (_count == 0 && Connect.UserId.ToUpper() != "BMW12714")
                    {
                        MessageBox.Show("Редактирование данного вида оплат невозможно!" +
                            "\nДанному сотруднику не установлен процент вредности дополнительно.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode_Pay_Type.Focus();
                        return;
                    }
                    break;
                default:
                    break;
            }
            OracleCommand ocMergeValidTime = new OracleCommand("", Connect.CurConnect);
            ocMergeValidTime.BindByName = true;
            ocMergeValidTime.CommandText = string.Format(
                Queries.GetQuery("Table/MergeWork_Pay_Type.sql"), 
                Connect.Schema);
            ocMergeValidTime.Parameters.Add("p_valid_time", OracleDbType.Decimal).Value = seconds;
            ocMergeValidTime.Parameters.Add("p_worked_day_id", OracleDbType.Decimal).Value = worked_day_id;
            ocMergeValidTime.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 
                cbPay_Type_Name.SelectedValue;
            ocMergeValidTime.Parameters.Add("p_work_pay_type_id", OracleDbType.Decimal).Value = work_pay_type_id;
            ocMergeValidTime.ExecuteNonQuery();
            Connect.Commit();
            Close();
        }

        private void tbCode_Pay_Type_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pay_Type, cbPay_Type_Name, 3, Connect.CurConnect, e, PAY_TYPE_seq.ColumnsName.PAY_TYPE_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "pay_type", PAY_TYPE_seq.ColumnsName.CODE_PAY_TYPE.ToString(), tbCode_Pay_Type.Text);
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
