using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using PercoXML;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class HBConditionWork : System.Windows.Forms.UserControl
    {   
        static DataSet _ds;
        public static DataSet DS
        {
            get { return _ds; }
        }
        static OracleDataAdapter _daCond, _daPercent;
        public HBConditionWork()
        {
            InitializeComponent();
            _ds.Tables["COND"].Rows.Clear();
            _ds.Tables["PERCENT"].Rows.Clear();
            _daCond.Fill(_ds.Tables["COND"]);
            _daPercent.Fill(_ds.Tables["PERCENT"]);

            dgCondition_Of_Work.DataSource = _ds.Tables["COND"].DefaultView;
            dgCondition_Of_Work_Percent.DataSource = _ds.Tables["PERCENT"].DefaultView;

            tsCondition.EnableByRules(true);
            tsConditionPercent.EnableByRules();
        }

        static HBConditionWork()
        {
            _ds = new DataSet();
            _ds.Tables.Add("COND");
            _ds.Tables.Add("PERCENT");

            // Select
            _daCond = new OracleDataAdapter(string.Format(
                @"select CONDITIONS_OF_WORK_ID, SUBCLASS_NUMBER 
                from {0}.CONDITIONS_OF_WORK
                ORDER BY SUBCLASS_NUMBER", Connect.Schema), Connect.CurConnect);
            // Insert
            _daCond.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.CONDITIONS_OF_WORK(CONDITIONS_OF_WORK_ID, SUBCLASS_NUMBER)
                VALUES(:CONDITIONS_OF_WORK_ID, :SUBCLASS_NUMBER)",
                Connect.Schema), Connect.CurConnect);
            _daCond.InsertCommand.BindByName = true;
            _daCond.InsertCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");
            _daCond.InsertCommand.Parameters.Add("SUBCLASS_NUMBER", OracleDbType.Varchar2, 0, "SUBCLASS_NUMBER");
            // Update
            _daCond.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.CONDITIONS_OF_WORK
                SET SUBCLASS_NUMBER=:SUBCLASS_NUMBER
                WHERE CONDITIONS_OF_WORK_ID=:CONDITIONS_OF_WORK_ID",
                Connect.Schema), Connect.CurConnect);
            _daCond.UpdateCommand.BindByName = true;
            _daCond.UpdateCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");
            _daCond.UpdateCommand.Parameters.Add("SUBCLASS_NUMBER", OracleDbType.Varchar2, 0, "SUBCLASS_NUMBER");
            // Delete
            _daCond.DeleteCommand = new OracleCommand(string.Format(
                "DELETE {0}.CONDITIONS_OF_WORK WHERE CONDITIONS_OF_WORK_ID=:CONDITIONS_OF_WORK_ID",
                Connect.Schema), Connect.CurConnect);
            _daCond.DeleteCommand.BindByName = true;
            _daCond.DeleteCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");

            // Select
            _daPercent = new OracleDataAdapter(string.Format(
                @"select CONDITIONS_OF_WORK_PERCENT_ID, CONDITIONS_OF_WORK_ID, PERCENT_ADD_RATE, DATE_START_PERCENT, DATE_END_PERCENT 
                from {0}.CONDITIONS_OF_WORK_PERCENT CP
                where CP.CONDITIONS_OF_WORK_ID = :p_CONDITIONS_OF_WORK_ID
                ORDER BY PERCENT_ADD_RATE", Connect.Schema), Connect.CurConnect);
            _daPercent.SelectCommand.BindByName = true;
            _daPercent.SelectCommand.Parameters.Add("p_CONDITIONS_OF_WORK_ID", OracleDbType.Decimal).Value = 0;
            // Insert
            _daPercent.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.CONDITIONS_OF_WORK_PERCENT(CONDITIONS_OF_WORK_PERCENT_ID, CONDITIONS_OF_WORK_ID, PERCENT_ADD_RATE, DATE_START_PERCENT, DATE_END_PERCENT)
                VALUES(:CONDITIONS_OF_WORK_PERCENT_ID,:CONDITIONS_OF_WORK_ID, :PERCENT_ADD_RATE, TRUNC(:DATE_START_PERCENT), TRUNC(:DATE_END_PERCENT)+86399/86400)",
                Connect.Schema), Connect.CurConnect);
            _daPercent.InsertCommand.BindByName = true;
            _daPercent.InsertCommand.Parameters.Add("CONDITIONS_OF_WORK_PERCENT_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_PERCENT_ID");
            _daPercent.InsertCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");
            _daPercent.InsertCommand.Parameters.Add("PERCENT_ADD_RATE", OracleDbType.Decimal, 0, "PERCENT_ADD_RATE");
            _daPercent.InsertCommand.Parameters.Add("DATE_START_PERCENT", OracleDbType.Date, 0, "DATE_START_PERCENT");
            _daPercent.InsertCommand.Parameters.Add("DATE_END_PERCENT", OracleDbType.Date, 0, "DATE_END_PERCENT");
            // Update
            _daPercent.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.CONDITIONS_OF_WORK_PERCENT
                SET PERCENT_ADD_RATE=:PERCENT_ADD_RATE,DATE_START_PERCENT=TRUNC(:DATE_START_PERCENT),DATE_END_PERCENT=TRUNC(:DATE_END_PERCENT)+86399/86400
                WHERE CONDITIONS_OF_WORK_PERCENT_ID=:CONDITIONS_OF_WORK_PERCENT_ID",
                Connect.Schema), Connect.CurConnect);
            _daPercent.UpdateCommand.BindByName = true;
            _daPercent.UpdateCommand.Parameters.Add("CONDITIONS_OF_WORK_PERCENT_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_PERCENT_ID");
            _daPercent.UpdateCommand.Parameters.Add("PERCENT_ADD_RATE", OracleDbType.Decimal, 0, "PERCENT_ADD_RATE");
            _daPercent.UpdateCommand.Parameters.Add("DATE_START_PERCENT", OracleDbType.Date, 0, "DATE_START_PERCENT");
            _daPercent.UpdateCommand.Parameters.Add("DATE_END_PERCENT", OracleDbType.Date, 0, "DATE_END_PERCENT");
            // Delete
            _daPercent.DeleteCommand = new OracleCommand(string.Format(
                "DELETE {0}.CONDITIONS_OF_WORK_PERCENT WHERE CONDITIONS_OF_WORK_PERCENT_ID=:CONDITIONS_OF_WORK_PERCENT_ID",
                Connect.Schema), Connect.CurConnect);
            _daPercent.DeleteCommand.BindByName = true;
            _daPercent.DeleteCommand.Parameters.Add("CONDITIONS_OF_WORK_PERCENT_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_PERCENT_ID");

            _ds.Tables["COND"].Rows.Clear();
            _daCond.Fill(_ds.Tables["COND"]);
        }

        private void tsbAddCondition_Click(object sender, EventArgs e)
        {
            DataRowView newRow = _ds.Tables["COND"].DefaultView.AddNew();
            _ds.Tables["COND"].Rows.Add(newRow.Row);
            this.Invalidate();
        }

        private void tsbDeleteCondition_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dgCondition_Of_Work.CurrentRow != null)
                {
                    dgCondition_Of_Work.Rows.Remove(dgCondition_Of_Work.CurrentRow);
                    SaveCondition();
                }
            }
        }

        private bool SaveCondition()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["COND"].Rows.Count; ++i)
                    if (_ds.Tables["COND"].Rows[i].RowState == DataRowState.Added)
                    {
                        _ds.Tables["COND"].Rows[i]["CONDITIONS_OF_WORK_ID"] =
                            new OracleCommand(string.Format("select {0}.CONDITIONS_OF_WORK_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                    }
                _daCond.InsertCommand.Transaction = transact;
                _daCond.UpdateCommand.Transaction = transact;
                _daCond.DeleteCommand.Transaction = transact;
                _daCond.Update(_ds.Tables["COND"]);
                transact.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void tsbSaveCondition_Click(object sender, EventArgs e)
        {
            if (SaveCondition())
                MessageBox.Show("Данные сохранены!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbCancelCondition_Click(object sender, EventArgs e)
        {
            _ds.Tables["COND"].RejectChanges();
            Connect.Rollback();
            dgCondition_Of_Work.Invalidate();
            this.Invalidate();
        }

        private void dgCondition_Of_Work_SelectionChanged(object sender, EventArgs e)
        {
            if (dgCondition_Of_Work.CurrentRow != null)
            {
                _ds.Tables["PERCENT"].Rows.Clear();
                _daPercent.SelectCommand.Parameters["p_CONDITIONS_OF_WORK_ID"].Value = 
                    dgCondition_Of_Work.CurrentRow.Cells["CONDITIONS_OF_WORK_ID"].Value;
                _daPercent.Fill(_ds.Tables["PERCENT"]);
            }
        }

        private void tsbAddPercentCond_Click(object sender, EventArgs e)
        {
            if (dgCondition_Of_Work.CurrentRow != null)
            {
                DataRowView newRow = _ds.Tables["PERCENT"].DefaultView.AddNew();
                _ds.Tables["PERCENT"].Rows.Add(newRow.Row);
            }
        }

        private void tsbDeletePercentCond_Click(object sender, EventArgs e)
        {
            if (dgCondition_Of_Work_Percent.CurrentRow != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgCondition_Of_Work_Percent.Rows.Remove(dgCondition_Of_Work_Percent.CurrentRow);
                    SavePercent();
                }
            }
        }

        private void tsbSavePercentCond_Click(object sender, EventArgs e)
        {
            dgCondition_Of_Work_Percent.EndEdit(DataGridViewDataErrorContexts.Commit);
            dgCondition_Of_Work_Percent.CommitEdit(DataGridViewDataErrorContexts.Commit);
            dgCondition_Of_Work_Percent.BindingContext[_ds.Tables["PERCENT"]].EndCurrentEdit();
            if (SavePercent())
                MessageBox.Show("Данные сохранены!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbCancelPercentCond_Click(object sender, EventArgs e)
        {
            _ds.Tables["PERCENT"].RejectChanges();
            Connect.Rollback();
            dgCondition_Of_Work.Invalidate();
            this.Invalidate();
        }

        private bool SavePercent()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                dgCondition_Of_Work_Percent.BindingContext[_ds.Tables["PERCENT"]].EndCurrentEdit();
                for (int i = 0; i < _ds.Tables["PERCENT"].Rows.Count; ++i)
                    if (_ds.Tables["PERCENT"].Rows[i].RowState == DataRowState.Added)
                    {
                        _ds.Tables["PERCENT"].Rows[i]["CONDITIONS_OF_WORK_PERCENT_ID"] =
                            new OracleCommand(string.Format("select {0}.COND_OF_WORK_PERCENT_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                        _ds.Tables["PERCENT"].Rows[i]["CONDITIONS_OF_WORK_ID"] =
                            dgCondition_Of_Work.CurrentRow.Cells["CONDITIONS_OF_WORK_ID"].Value;
                    }
                _daPercent.InsertCommand.Transaction = transact;
                _daPercent.UpdateCommand.Transaction = transact;
                _daPercent.DeleteCommand.Transaction = transact;
                _daPercent.Update(_ds.Tables["PERCENT"]);
                transact.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
