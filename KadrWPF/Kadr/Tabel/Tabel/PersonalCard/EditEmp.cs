using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Kadr;
using System.Reflection;
using Staff;
using LibraryKadr;
using WpfControlLibrary;
using System.Windows.Interop;

namespace Tabel
{

    public partial class EditEmp : Form
    {        
        OracleDataTable dtEmpPermit;
        public StringBuilder str_filter = new StringBuilder();
        string per_num;
        decimal _transfer_id, _worker_id;
        DateTime date_end;
        OracleDataReader odrGR_Work;
        OracleCommand ocGr_Work, _ocSign_Overtime;
        int subdiv_id, sign_comb;
        OracleDataAdapter daTransfer_Harmful;
        DataTable dtTransfer_Harmful;
        public EditEmp(string _per_num, decimal transfer_id, decimal worker_id, string last_name, 
            string first_name, string middle_name, string pos_name, int _subdiv_id, int _sign_comb, string _order_name, 
            string _code_degree, string _group_master, DateTime _date_end)
        {
            InitializeComponent();
            per_num = _per_num;
            _transfer_id = transfer_id;
            date_end = _date_end;
            _worker_id = worker_id;
            dtEmpPermit = new OracleDataTable("", Connect.CurConnect);
            dtEmpPermit.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("PermitForEmp.sql"), Connect.Schema);
            dtEmpPermit.SelectCommand.Parameters.Add("p_worker_id", OracleDbType.Decimal);
            dtEmpPermit.SelectCommand.Parameters["p_worker_id"].Value = worker_id;
            dtEmpPermit.Fill();
            dgEmp_Auth.DataSource = dtEmpPermit.DefaultView;
            dgEmp_Auth.Columns["permit_id"].Visible = false;
            dgEmp_Auth.Columns["FL_ARCHIV"].Visible = false;
            dgEmp_Auth.Columns["NUM_DOC_PERMIT"].Visible = false;
            dgEmp_Auth.Columns["DATE_DOC_PERMIT"].Visible = false;
            dgEmp_Auth.Columns["PERMIT_NAME"].HeaderText = "Наименование разрешения";
            dgEmp_Auth.Columns["DATE_START_PERMIT"].HeaderText = "Дата начала разрешения";
            dgEmp_Auth.Columns["DATE_END_PERMIT"].HeaderText = "Дата окончания разрешения";
            
            ocGr_Work = new OracleCommand(
                string.Format("select GW.GR_WORK_NAME " + 
                "from {0}.GR_WORK GW " + 
                "join (select distinct FIRST_VALUE(EGW.GR_WORK_ID) OVER (ORDER BY EGW.GR_WORK_DATE_BEGIN desc) GR_WORK_ID " + 
                "       from {0}.EMP_GR_WORK EGW " + 
                "       where EGW.TRANSFER_ID in ( " + 
                "           select T.TRANSFER_ID from {0}.TRANSFER T start with T.TRANSFER_ID = :p_transfer_id " + 
                "           connect by nocycle prior T.TRANSFER_ID = T.FROM_POSITION or T.TRANSFER_ID = prior T.FROM_POSITION)) GW1 " + 
                "   on GW.GR_WORK_ID = GW1.GR_WORK_ID", Connect.Schema), Connect.CurConnect);
            ocGr_Work.BindByName = true;
            ocGr_Work.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
            odrGR_Work = ocGr_Work.ExecuteReader();
            while(odrGR_Work.Read())
            {
                tbGr_Work.Text = odrGR_Work[0].ToString();
            }
            tbPer_num.Text = per_num;
            lastName.Text = last_name;
            firstName.Text = first_name;
            middleName.Text = middle_name;
            empPositon.Text = pos_name;
            tbOrder_Name.Text = _order_name;
            tbCode_Degree.Text = _code_degree;
            tbGroup_Master.Text = _group_master;
            chSign_Comb.Checked = Convert.ToBoolean(sign_comb);
            pbPhoto.Image = EmployeePhoto.GetPhoto(per_num);
            //dgEmp_Auth.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            panel1.EnableByRules();
            btExit.Enabled = true;            
            subdiv_id = _subdiv_id;
            sign_comb = _sign_comb;
            panel2.EnableByRules();
            OracleDataTable dtWaterProc = new OracleDataTable("", Connect.CurConnect);
            dtWaterProc.SelectCommand.CommandText = string.Format(
                "select nvl(AD.WATER_PROC,0) from {0}.ACCOUNT_DATA AD " + 
                "where AD.TRANSFER_ID = :p_transfer_id and rownum = 1", Connect.Schema);
            dtWaterProc.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
            dtWaterProc.Fill();
            if (dtWaterProc.Rows.Count > 0)
                chWater_Proc.Checked = Convert.ToBoolean(dtWaterProc.Rows[0][0]);
            dtWaterProc = new OracleDataTable("", Connect.CurConnect);
            dtWaterProc.SelectCommand.CommandText = string.Format(
                "select 1 from {0}.EMP_WAYBILL WB where WB.PER_NUM = :p_per_num " + 
                "and WB.TRANSFER_ID in " + 
                "(select T.TRANSFER_ID from {0}.TRANSFER T where T.PER_NUM = :p_per_num and T.SUBDIV_ID = :p_subdiv_id) ",
                Connect.Schema);
            dtWaterProc.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            dtWaterProc.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Int32).Value = subdiv_id;
            dtWaterProc.Fill();
            if (dtWaterProc.Rows.Count > 0)
                chWaybill.Checked = Convert.ToBoolean(1);

            dtTransfer_Harmful = new DataTable();
            daTransfer_Harmful = new OracleDataAdapter();
            daTransfer_Harmful.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/Transfer_With_Harmful.sql"),
                Connect.Schema), Connect.CurConnect);
            daTransfer_Harmful.SelectCommand.BindByName = true;
            daTransfer_Harmful.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
            daTransfer_Harmful.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = date_end;
            daTransfer_Harmful.UpdateCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/MergeChange_Harmful.sql"),
                Connect.Schema), Connect.CurConnect);
            daTransfer_Harmful.UpdateCommand.BindByName = true;
            daTransfer_Harmful.UpdateCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
            daTransfer_Harmful.UpdateCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = date_end;
            daTransfer_Harmful.UpdateCommand.Parameters.Add("PERCENTAGE_OF_EMPLOYMENT", OracleDbType.Decimal, 0, "PERCENTAGE_OF_EMPLOYMENT");
            daTransfer_Harmful.Fill(dtTransfer_Harmful);
            decimal sumHarmful = 0;
            Decimal.TryParse(dtTransfer_Harmful.Compute("SUM(HARMFUL_ADDITION_ADD)", "HARMFUL_ADDITION_ADD>0").ToString(), out sumHarmful);
            if (sumHarmful == 0)
            {
                gbTransfer_Harmful.Visible = false;
            }
            else
            {
                gbTransfer_Harmful.Visible = true;
                dgTransfer_Harmful.DataSource = dtTransfer_Harmful;
                dgTransfer_Harmful.Columns["DATE_TRANSFER"].HeaderText = "Дата перевода";
                dgTransfer_Harmful.Columns["DATE_TRANSFER"].ReadOnly = true;
                dgTransfer_Harmful.Columns["END_TRANSFER"].HeaderText = "Дата окончания перевода";
                dgTransfer_Harmful.Columns["END_TRANSFER"].ReadOnly = true;
                dgTransfer_Harmful.Columns["HARMFUL_ADDITION_ADD"].HeaderText = "Надбавка за вредность";
                dgTransfer_Harmful.Columns["HARMFUL_ADDITION_ADD"].ReadOnly = true;
                /*dgTransfer_Harmful.Columns["PERCENTAGE_OF_EMPLOYMENT"].HeaderText = "Процент занятости";*/
            }

            _ocSign_Overtime = new OracleCommand(string.Format(
                @"SELECT 1 FROM {0}.TRANSFER_OVERTIME TRO WHERE TRO.TRANSFER_ID = :p_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _ocSign_Overtime.BindByName = true;
            _ocSign_Overtime.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
            chSign_Overtime.Checked = _ocSign_Overtime.ExecuteReader().HasRows;
        }                

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();            
        }

        private void btEditGr_Work_Click(object sender, EventArgs e)
        {
            EditGr_Work editGr_Work = new EditGr_Work(subdiv_id, per_num, _transfer_id);
            editGr_Work.ShowInTaskbar = false;
            if (editGr_Work.ShowDialog() == DialogResult.OK)
            {
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(Queries.GetQuery("Table/UpdateGr_Work.sql"), DataSourceScheme.SchemeName);
                com.Parameters.Add("PER_NUM", OracleDbType.Varchar2).Value = per_num;
                com.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
                com.Parameters.Add("GR_WORK_ID", OracleDbType.Decimal).Value = editGr_Work.gr_work_id;
                com.Parameters.Add("GR_WORK_DATE_BEGIN", OracleDbType.Date).Value = 
                    (DateTime)editGr_Work.deGr_work_date_begin.Date;
                com.Parameters.Add("GR_WORK_DAY_NUM", OracleDbType.Decimal).Value = 
                    Convert.ToInt32(editGr_Work.cbGr_work_day_num.SelectedValue);                
                com.ExecuteNonQuery();
                com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(
                    "update {0}.TRANSFER T set T.GR_WORK_ID = :p_gr_work_id " + 
                    "where T.TRANSFER_ID = :p_transfer_id", Connect.Schema);
                com.Parameters.Add("p_gr_work_id", OracleDbType.Decimal).Value = editGr_Work.gr_work_id;
                com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                com.ExecuteNonQuery();
                Connect.Commit();                
            }
            odrGR_Work = ocGr_Work.ExecuteReader();
            while (odrGR_Work.Read())
            {
                tbGr_Work.Text = odrGR_Work[0].ToString();
            }
        }

        private void btEditOrder_Click(object sender, EventArgs e)
        {
            EditOrder editOrder = new EditOrder(true, per_num, _transfer_id);
            editOrder.ShowInTaskbar = false;
            editOrder.ShowDialog();
            if (editOrder.Order_ID_Property != -1)
            {
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(Queries.GetQuery("Table/UpdateOrder.sql"), DataSourceScheme.SchemeName);
                com.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                com.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
                com.Parameters.Add("p_ORDER_ID", OracleDbType.Decimal).Value = editOrder.Order_ID_Property;
                com.Parameters.Add("p_DATE_ORDER", OracleDbType.Date).Value = editOrder.DATE_ORDER;
                com.Parameters.Add("p_SIGN_COMB", OracleDbType.Decimal).Value = sign_comb;
                com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                com.ExecuteNonQuery();
                Connect.Commit();

                tbOrder_Name.Text = editOrder.Order_Name_Property;

                com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(Queries.GetQuery("Table/UpdateWork_Pay_Type.sql"), 
                    DataSourceScheme.SchemeName);
                com.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                com.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                com.Parameters.Add("p_ORDER_ID", OracleDbType.Decimal).Value = editOrder.Order_ID_Property;
                com.Parameters.Add("p_DATE_ORDER_begin", OracleDbType.Date).Value = editOrder.DATE_ORDER;
                com.Parameters.Add("p_DATE_ORDER_end", OracleDbType.Date).Value = date_end;
                com.ExecuteNonQuery();
                Connect.Commit(); ;
            }
        }

        private void tbGroup_Master_Click(object sender, EventArgs e)
        {
            //if (tbCode_Degree.Text == "01" || tbCode_Degree.Text == "02" || tbCode_Degree.Text == "61")
            //{
                Edit_Group_Master edit_GM = new Edit_Group_Master(_transfer_id, per_num);
                edit_GM.ShowInTaskbar = false;
                if (edit_GM.ShowDialog() == DialogResult.OK)
                {
                    //OracleCommand com = new OracleCommand("", Connect.CurConnect);
                    //com.CommandText = string.Format(Queries.GetQuery("Table/UpdateGroup_Master.sql"), DataSourceScheme.SchemeName);
                    //com.Parameters.Add("P_PER_NUM", OracleDbType.Varchar2);
                    //com.Parameters.Add("P_SIGN_COMB", OracleDbType.Decimal);
                    //com.Parameters.Add("P_DATE_GROUP_MASTER", OracleDbType.Date);
                    //com.Parameters.Add("P_NAME_GROUP_MASTER", OracleDbType.Varchar2);
                    //com.Parameters["P_PER_NUM"].Value = per_num;
                    //com.Parameters["P_SIGN_COMB"].Value = sign_comb;
                    //com.Parameters["P_DATE_GROUP_MASTER"].Value = edit_GM.Date_GM;
                    //com.Parameters["P_NAME_GROUP_MASTER"].Value = edit_GM.Name_GM;
                    //com.ExecuteNonQuery();
                    //com = new OracleCommand("commit", Connect.CurConnect);
                    //com.ExecuteNonQuery();
                    //tbGroup_Master.Text = edit_GM.Name_GM;
                }
            //}
            //else
            //{
            //    MessageBox.Show("Группа мастера может быть установлена для 01, 02 и 61 категории", "АРМ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
        }

        private void btSaveWaterProc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сохранить признак гидропроцедур?", 
                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                DialogResult.Yes)
            {
                OracleCommand ocUpdAccData = new OracleCommand("", Connect.CurConnect);
                ocUpdAccData.BindByName = true;
                ocUpdAccData.CommandText = string.Format(
                    "update {0}.ACCOUNT_DATA AD set AD.WATER_PROC = :p_water_proc " +
                    "where AD.TRANSFER_ID = :p_transfer_id",
                    Connect.Schema);
                ocUpdAccData.Parameters.Add("p_water_proc", OracleDbType.Decimal).Value =
                    Convert.ToInt32(chWater_Proc.Checked);
                ocUpdAccData.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = _transfer_id;
                ocUpdAccData.ExecuteNonQuery();
                Connect.Commit();
            }
        }

        private void btSignWaybill_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сохранить признак путевого листа?",
                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                OracleCommand ocUpdWaybill = new OracleCommand("", Connect.CurConnect);
                ocUpdWaybill.BindByName = true;
                if (chWaybill.Checked)
                {
                    ocUpdWaybill.CommandText = string.Format(Queries.GetQuery("Table/MergeEmp_Waybill.sql"),
                        Connect.Schema);
                    ocUpdWaybill.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
                    ocUpdWaybill.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                }
                else
                {
                    ocUpdWaybill.CommandText = string.Format(
                        "delete from {0}.EMP_WAYBILL WB where WB.PER_NUM = :p_per_num ",
                        Connect.Schema);
                    ocUpdWaybill.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                }
                ocUpdWaybill.ExecuteNonQuery();
                Connect.Commit();
            }
        }

        private void btCancelEdit_Click(object sender, EventArgs e)
        {
            dtTransfer_Harmful.Clear();
            daTransfer_Harmful.Fill(dtTransfer_Harmful);
        }

        private void btSaveChange_Harmful_Click(object sender, EventArgs e)
        {
            if (dtTransfer_Harmful.GetChanges() != null)
            {
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    daTransfer_Harmful.UpdateCommand.Transaction = transact;
                    daTransfer_Harmful.Update(dtTransfer_Harmful);
                    transact.Commit();
                    MessageBox.Show("Изменения сохранены!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transact.Rollback();
                    MessageBox.Show(ex.Message, "АРМ \"Учет рабочего времени\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Нет изменений для сохранения!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgEmp_Auth_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если сотрудник уволен красим строку в серый цвет
            if (dgEmp_Auth["FL_ARCHIV", e.RowIndex].Value.ToString() == "1")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
        }

        private void btView_Transfer_Overtime_Click(object sender, EventArgs e)
        {
            List_Transfer_Overtime listTransfer = new List_Transfer_Overtime(_transfer_id);
            WindowInteropHelper wih = new WindowInteropHelper(listTransfer);
            wih.Owner = this.Handle;
            listTransfer.ShowDialog();
            chSign_Overtime.Checked = _ocSign_Overtime.ExecuteReader().HasRows;
        }
    } 
}
