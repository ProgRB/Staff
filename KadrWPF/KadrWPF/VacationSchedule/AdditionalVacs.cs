using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;
using Kadr;

namespace VacationSchedule
{
    public partial class AdditionalVacs : Form
    {
        DataSet ds = new DataSet();
        DialogResult d = DialogResult.Cancel;
        OracleDataAdapter a;
        private object last_sel = null;
        public object TransferId
        {
            get;set;
        }
        public AdditionalVacs(object transfer_id)
        {
            InitializeComponent();
            TransferId = transfer_id;
            OracleCommand cmd= new OracleCommand(string.Format(Queries.GetQuery("go/GetEmpVacAddPeriods.sql"),Connect.Schema),Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_transfer_id",OracleDbType.Decimal, TransferId, ParameterDirection.Input);
            cmd.Parameters.Add("p_vac_group_type_id",OracleDbType.Decimal);
            a = new OracleDataAdapter(cmd);
            a.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.VAC_ADD_PERIOD_INSERT(:p_VAC_ADD_PERIOD_ID, :p_CALC_BEGIN, :p_CALC_END, :p_VAC_GROUP_TYPE_ID, :p_TRANSFER_ID); end;", Connect.Schema), Connect.CurConnect);
            a.InsertCommand.BindByName = true;
            a.InsertCommand.Parameters.Add("p_VAC_ADD_PERIOD_ID", OracleDbType.Decimal, 0, "VAC_ADD_PERIOD_ID").Direction=ParameterDirection.InputOutput;
            a.InsertCommand.Parameters["p_VAC_ADD_PERIOD_ID"].DbType = DbType.Decimal;
            a.InsertCommand.Parameters.Add("p_CALC_BEGIN",OracleDbType.Date,8,"CALC_BEGIN");
            a.InsertCommand.Parameters.Add("p_CALC_END",OracleDbType.Date,8,"CALC_END");
            a.InsertCommand.Parameters.Add("p_VAC_GROUP_TYPE_ID",OracleDbType.Decimal);
            a.InsertCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, TransferId, ParameterDirection.Input);
            a.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;

            a.UpdateCommand = new OracleCommand(string.Format(@"begin {0}.VAC_ADD_PERIOD_UPDATE(:p_VAC_ADD_PERIOD_ID, :p_CALC_BEGIN, :p_CALC_END); end;",Connect.Schema),Connect.CurConnect);
            a.UpdateCommand.BindByName = true;
            a.UpdateCommand.Parameters.Add("p_VAC_ADD_PERIOD_ID", OracleDbType.Decimal, 0, "VAC_ADD_PERIOD_ID");
            a.UpdateCommand.Parameters.Add("p_CALC_BEGIN",OracleDbType.Date,8,"CALC_BEGIN");
            a.UpdateCommand.Parameters.Add("p_CALC_END",OracleDbType.Date,8,"CALC_END");

            a.DeleteCommand = new OracleCommand(string.Format(@"begin {0}.VAC_ADD_PERIOD_DELETE(:p_VAC_ADD_PERIOD_ID); end; ", Connect.Schema), Connect.CurConnect);
            a.DeleteCommand.BindByName = true;
            a.DeleteCommand.Parameters.Add("p_VAC_ADD_PERIOD_ID",OracleDbType.Decimal,0,"VAC_ADD_PERIOD_ID");

            grid.AutoGenerateColumns = false;

            new OracleDataAdapter(string.Format(Queries.GetQuery(@"\go\SelectVacAddTypes.sql"), Connect.Schema), Connect.CurConnect).Fill(ds,"VAC_GROUP_TYPE");
            gridAddVac.ValueMember ="VAC_GROUP_TYPE_ID";
            gridAddVac.DisplayMember = "GROUP_VAC_NAME";
            gridAddVac.DataSource = new DataView(ds.Tables["VAC_GROUP_TYPE"], "", "GROUP_VAC_NAME", DataViewRowState.OriginalRows);

            FillPeriods();
            grid.DataSource = ds.Tables["periods"];
            grid.Columns.Add(new MDataGridViewCalendarColumn("calc_begin", "Дата начала работы для доп. дней отпуска", "CALC_BEGIN"));
            grid.Columns.Add(new MDataGridViewCalendarColumn("calc_end", "Дата окончания работы для доп. дней отпуска", "CALC_END"));

            gridAddVac.SelectedValueChanged += new EventHandler(gridAddVac_SelectedValueChanged);
            grid.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            ColumnWidthSaver.FillWidthOfColumn(grid);
            toolStrip1.EnableByRules(false);
        }

        private void FillPeriods()
        {
            CancelEventArgs ex = new CancelEventArgs();
            SavePeriods(this, ex);
            if (ex.Cancel)
            {
                gridAddVac.SelectedValueChanged -= gridAddVac_SelectedValueChanged;
                gridAddVac.SelectedValue = last_sel;
                gridAddVac.SelectedValueChanged += gridAddVac_SelectedValueChanged;
                return;
            }
            if (ds.Tables.Contains("periods"))
                ds.Tables["periods"].Rows.Clear();
            if (gridAddVac.SelectedValue != null)
            {
                last_sel = gridAddVac.SelectedValue;
                a.SelectCommand.Parameters["p_vac_group_type_id"].Value = gridAddVac.SelectedValue;
                a.Fill(ds,"periods");
            }
            if (grid.DataSource == null)
                grid.DataSource = ds.Tables["periods"];
        }

        public void SavePeriods(object sender, EventArgs e) { SavePeriods(sender, new CancelEventArgs()); }

        public void SavePeriods(object sender, CancelEventArgs e)
        {
            grid.EndEdit(DataGridViewDataErrorContexts.Commit);
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (ds.Tables.Contains("periods")) grid.BindingContext[ds.Tables["periods"]].EndCurrentEdit();
            d = DialogResult.Yes;
            if (ds != null && ds.HasChanges())
            {
                foreach (DataRow r in ds.Tables["PERIODS"].Rows)
                    if (r.RowState != DataRowState.Deleted && (r["CALC_BEGIN"] == DBNull.Value ? DateTime.MinValue : r.Field<DateTime>("CALC_BEGIN")) > (r["CALC_END"] == DBNull.Value ? DateTime.MaxValue : r.Field<DateTime>("CALC_END")))
                    {
                        MessageBox.Show("Дата начала периода больше чем дата окончания", "Графики отпусков");
                        return;
                    }
            }
            if (ds.HasChanges() && (d=MessageBox.Show("Сохранить изменения?","Графики отпусков", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))== DialogResult.Yes)
            {
                
                OracleTransaction tr = Connect.CurConnect.BeginTransaction();
                try
                {
                    a.InsertCommand.Parameters["p_vac_group_type_id"].Value = gridAddVac.SelectedValue;
                    foreach (DataRow r in ds.Tables["PERIODS"].Rows)
                    {
                        if (r.RowState!= DataRowState.Deleted && (r["CALC_BEGIN"]==DBNull.Value?DateTime.MinValue:r.Field<DateTime>("CALC_BEGIN"))>(r["CALC_END"]==DBNull.Value?DateTime.MaxValue:r.Field<DateTime>("CALC_END")))
                        {
                            MessageBox.Show("Дата начала периода больше чем дата окончания", "Графики отпусков");
                            return;
                        }
                    }
                    a.Update(ds.Tables["PERIODS"]);
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show("Ошибка сохранения"+ex.Message);
                }
            }
            if (d == System.Windows.Forms.DialogResult.Cancel)
                e.Cancel = true;
        }

        private void tsbtAddVacAddPeriod_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables["periods"].NewRow();
            r["transfer_id"]=TransferId;
            r["vac_group_type_id"] = gridAddVac.SelectedValue;
            ds.Tables["periods"].Rows.Add(r);
        }

        private void gridAddVac_SelectedValueChanged(object sender, EventArgs e)
        {
            FillPeriods();
        }

        private void tsbtDelVacAddPeriod_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow != null && MessageBox.Show("Удалить строку?","График отпусков", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)== System.Windows.Forms.DialogResult.OK)
            {
                (grid.CurrentRow.DataBoundItem as DataRowView).Delete();
            }
        }

        private void AdditionalVacs_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelEventArgs er = new CancelEventArgs(false);
            SavePeriods(this, er);
            if (er.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                this.DialogResult = d;
            }
        }
    }
}
