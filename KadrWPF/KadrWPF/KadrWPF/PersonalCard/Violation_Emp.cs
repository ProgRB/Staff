using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using LibraryKadr;

namespace Kadr
{
    public partial class Violation_Emp : Form
    {
        DataRowView _drView;
        OracleDataAdapter _daViolation_Emp_Pun;
        DataSet _dsViolation_Emp;
        PersonalCard _personalCard;
        // Создание формы
        public Violation_Emp(DataRowView drView, DataSet dsViolation_Emp, PersonalCard personalCard)
        {
            InitializeComponent();
            _drView = drView;
            _dsViolation_Emp = dsViolation_Emp;
            _personalCard = personalCard;
            // Привязка компонентов
            cbREASON_DETENTION_ID.DataBindings.Add("SelectedValue", drView, "REASON_DETENTION_ID", true, DataSourceUpdateMode.OnPropertyChanged, null);
            deDETENTION_DATE.DataBindings.Add("Date", drView, "DETENTION_DATE", true, DataSourceUpdateMode.OnPropertyChanged, null);
            tbCOUNT_DAYS.DataBindings.Add("Text", drView, "COUNT_DAYS", true, DataSourceUpdateMode.OnPropertyChanged, "");
            tbPUNISHMENT_NUM_ORDER.DataBindings.Add("Text", drView, "PUNISHMENT_NUM_ORDER", true, DataSourceUpdateMode.OnPropertyChanged, "");
            dePUNISHMENT_DATE_ORDER.DataBindings.Add("Date", drView, "PUNISHMENT_DATE_ORDER", true, DataSourceUpdateMode.OnPropertyChanged, null);
            tbNOTE.DataBindings.Add("Text", drView, "NOTE", true, DataSourceUpdateMode.OnPropertyChanged, "");

            cbREASON_DETENTION_ID.DataSource = dsViolation_Emp.Tables["REASON_DETENTION"].DefaultView;
            cbREASON_DETENTION_ID.DisplayMember = "REASON_DETENTION_NAME";
            cbREASON_DETENTION_ID.ValueMember = "REASON_DETENTION_ID";

            _dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].Clear();
            // Select
            _daViolation_Emp_Pun = new OracleDataAdapter(string.Format(
                @"select VIOLATION_EMP_PUN_ID, VIOLATION_EMP_ID, TYPE_PUNISHMENT_ID, PERCENT_PUNISHMENT 
                from {0}.VIOLATION_EMP_PUN where VIOLATION_EMP_ID = :p_VIOLATION_EMP_ID",
                Connect.Schema), Connect.CurConnect);
            _daViolation_Emp_Pun.SelectCommand.BindByName = true;
            _daViolation_Emp_Pun.SelectCommand.Parameters.Add("p_VIOLATION_EMP_ID", OracleDbType.Varchar2).Value = _drView["VIOLATION_EMP_ID"];
            _daViolation_Emp_Pun.Fill(_dsViolation_Emp.Tables["VIOLATION_EMP_PUN"]);
            // Insert
            _daViolation_Emp_Pun.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.VIOLATION_EMP_PUN_UPDATE(:VIOLATION_EMP_PUN_ID, :VIOLATION_EMP_ID, :TYPE_PUNISHMENT_ID, :PERCENT_PUNISHMENT);
                END;", Connect.Schema), Connect.CurConnect);
            _daViolation_Emp_Pun.InsertCommand.BindByName = true;
            _daViolation_Emp_Pun.InsertCommand.Parameters.Add("VIOLATION_EMP_PUN_ID", OracleDbType.Decimal, 0, "VIOLATION_EMP_PUN_ID").Direction =
                ParameterDirection.InputOutput;
            _daViolation_Emp_Pun.InsertCommand.Parameters["VIOLATION_EMP_PUN_ID"].DbType = DbType.Decimal;
            _daViolation_Emp_Pun.InsertCommand.Parameters.Add("VIOLATION_EMP_ID", OracleDbType.Decimal, 0, "VIOLATION_EMP_ID");
            _daViolation_Emp_Pun.InsertCommand.Parameters.Add("TYPE_PUNISHMENT_ID", OracleDbType.Decimal, 0, "TYPE_PUNISHMENT_ID");
            _daViolation_Emp_Pun.InsertCommand.Parameters.Add("PERCENT_PUNISHMENT", OracleDbType.Decimal, 0, "PERCENT_PUNISHMENT").Direction =
                ParameterDirection.InputOutput;
            _daViolation_Emp_Pun.InsertCommand.Parameters["PERCENT_PUNISHMENT"].DbType = DbType.Decimal;
            // Update
            _daViolation_Emp_Pun.UpdateCommand = _daViolation_Emp_Pun.InsertCommand;
            // Delete
            _daViolation_Emp_Pun.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                        {0}.VIOLATION_EMP_PUN_DELETE(:VIOLATION_EMP_PUN_ID);
                    END;", Connect.Schema), Connect.CurConnect);
            _daViolation_Emp_Pun.DeleteCommand.BindByName = true;
            _daViolation_Emp_Pun.DeleteCommand.Parameters.Add("VIOLATION_EMP_PUN_ID", OracleDbType.Decimal, 0, "VIOLATION_EMP_PUN_ID");

            dcTYPE_PUNISHMENT_ID.DataSource = _dsViolation_Emp.Tables["TYPE_PUNISHMENT"].DefaultView;
            dcTYPE_PUNISHMENT_ID.DisplayMember = "TYPE_PUNISHMENT_NAME";
            dcTYPE_PUNISHMENT_ID.ValueMember = "TYPE_PUNISHMENT_ID";
            dcPERCENT_PUNISHMENT.DataSource = _dsViolation_Emp.Tables["PERCENT_PUNISHMENT"].DefaultView;
            dcPERCENT_PUNISHMENT.DisplayMember = "PERCENT_PUNISHMENT";
            dcPERCENT_PUNISHMENT.ValueMember = "PERCENT_PUNISHMENT";
            dgViolation_Emp_Pun.DataSource = _dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].DefaultView;
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            if (this.BindingContext[_drView] != null)
            {
                if (_dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].GetChanges() != null)
                {
                    SaveViolation_Emp_Pun();
                }
                //_drView["REASON_DETENTION_NAME"] = cbREASON_DETENTION_ID.Text;
                this.BindingContext[_drView].EndCurrentEdit();
                _personalCard.SaveViolation_Emp();
                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            //this.Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btAddViolation_Emp_Pun_Click(object sender, EventArgs e)
        {
            if (_drView["VIOLATION_EMP_ID"] != DBNull.Value && _drView["VIOLATION_EMP_ID"] != null)
            {
                DataRowView _drViewPun = _dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].DefaultView.AddNew();
                _drViewPun["VIOLATION_EMP_ID"] = _drView["VIOLATION_EMP_ID"];
                _dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].Rows.Add(_drViewPun.Row);
            }
            else
            {
                MessageBox.Show("Сначала нужно сохранить само нарушение, а затем попробовать снова добавлять наказание.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btDeleteViolation_Emp_Pun_Click(object sender, EventArgs e)
        {
            if (dgViolation_Emp_Pun.CurrentRow == null)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgViolation_Emp_Pun.Rows.Remove(dgViolation_Emp_Pun.CurrentRow);
                SaveViolation_Emp_Pun();
            }
        }

        private void btSaveViolation_Emp_Pun_Click(object sender, EventArgs e)
        {
            SaveViolation_Emp_Pun();
        }

        void SaveViolation_Emp_Pun()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daViolation_Emp_Pun.InsertCommand.Transaction = transact;
                _daViolation_Emp_Pun.UpdateCommand.Transaction = transact;
                _daViolation_Emp_Pun.DeleteCommand.Transaction = transact;
                _daViolation_Emp_Pun.Update(_dsViolation_Emp.Tables["VIOLATION_EMP_PUN"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                _dsViolation_Emp.Tables["VIOLATION_EMP_PUN"].RejectChanges();
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
