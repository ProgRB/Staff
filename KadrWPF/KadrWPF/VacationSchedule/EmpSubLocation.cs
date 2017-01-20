using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace VacationSchedule
{
    public partial class EmpSubLocation : UserControl
    {
        DataSet ds;
        OracleDataAdapter odaWorkPeriod;
        public EmpSubLocation()
        {
            ds = new DataSet();
            odaRegions = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelecteRegionsBySub.sql"), Connect.Schema), Connect.CurConnect);
            odaRegions.SelectCommand.BindByName = true;
            odaRegions.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaRegions.TableMappings.Add("Table", "RegionSubdiv");
            odaRegions.InsertCommand = new OracleCommand(string.Format(@"begin {0}.REGION_SUBDIV_UPDATE
                          (
                           :p_REGION_SUBDIV_ID 
                          ,:p_SUBDIV_ID
                          ,:p_REGION_NAME 
                          ,:p_CODE_REGION
                          ,:p_DATE_START_REG
                          ,:p_DATE_END_REG); end;", Connect.Schema), Connect.CurConnect);
            odaRegions.InsertCommand.BindByName = true;
            odaRegions.InsertCommand.Parameters.Add("p_REGION_SUBDIV_ID", OracleDbType.Decimal, 0, "REGION_SUBDIV_ID").Direction = ParameterDirection.InputOutput;
            odaRegions.InsertCommand.Parameters["p_REGION_SUBDIV_ID"].DbType = DbType.Decimal;
            odaRegions.InsertCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID");
            odaRegions.InsertCommand.Parameters.Add("p_REGION_NAME", OracleDbType.Varchar2, 0, "REGION_NAME");
            odaRegions.InsertCommand.Parameters.Add("p_CODE_REGION", OracleDbType.Varchar2, 0, "CODE_REGION");
            odaRegions.InsertCommand.Parameters.Add("p_DATE_START_REG", OracleDbType.Date, 0, "DATE_START_REG");
            odaRegions.InsertCommand.Parameters.Add("p_DATE_END_REG", OracleDbType.Date, 0, "DATE_END_REG");
            odaRegions.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;

            odaRegions.UpdateCommand = new OracleCommand(string.Format(@"begin {0}.REGION_SUBDIV_UPDATE
                          (
                           :p_REGION_SUBDIV_ID 
                          ,:p_SUBDIV_ID 
                          ,:p_REGION_NAME 
                          ,:p_CODE_REGION
                          ,:p_DATE_START_REG
                          ,:p_DATE_END_REG); end;", Connect.Schema), Connect.CurConnect);
            odaRegions.UpdateCommand.BindByName = true;
            odaRegions.UpdateCommand.Parameters.Add("p_REGION_SUBDIV_ID", OracleDbType.Decimal, 0, "REGION_SUBDIV_ID").Direction = ParameterDirection.InputOutput;
            odaRegions.UpdateCommand.Parameters["p_REGION_SUBDIV_ID"].DbType = DbType.Decimal;
            odaRegions.UpdateCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID");
            odaRegions.UpdateCommand.Parameters.Add("p_REGION_NAME", OracleDbType.Varchar2, 0, "REGION_NAME");
            odaRegions.UpdateCommand.Parameters.Add("p_CODE_REGION", OracleDbType.Varchar2, 0, "CODE_REGION");
            odaRegions.UpdateCommand.Parameters.Add("p_DATE_START_REG", OracleDbType.Date, 0, "DATE_START_REG");
            odaRegions.UpdateCommand.Parameters.Add("p_DATE_END_REG", OracleDbType.Date, 0, "DATE_END_REG");

            odaRegions.DeleteCommand = new OracleCommand(string.Format(@"begin {0}.REGION_SUBDIV_DELETE(:p_REGION_SUBDIV_ID); end;", Connect.Schema), Connect.CurConnect);
            odaRegions.DeleteCommand.Parameters.Add("p_REGION_SUBDIV_ID", OracleDbType.Decimal, 0, "REGION_SUBDIV_ID");

            ds.Tables.Add("RegionSubdiv");

            odaEmps = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectEmpBySubdiv.sql"), Connect.Schema), Connect.CurConnect);
            odaEmps.SelectCommand.BindByName = true;
            odaEmps.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaEmps.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, null, ParameterDirection.Input);
            odaEmps.TableMappings.Add("Table", "Emps");            
            ds.Tables.Add("Emps");

            odaWorkPeriod = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectEmpAllocateRegion.sql"), Connect.Schema), Connect.CurConnect);
            odaWorkPeriod.SelectCommand.BindByName = true;
            odaWorkPeriod.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, 0, ParameterDirection.Input);
            odaWorkPeriod.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, 0, ParameterDirection.Input);
            odaWorkPeriod.TableMappings.Add("Table", "EmpRegion");
            ds.Tables.Add("EmpRegion");
            odaWorkPeriod.InsertCommand = new OracleCommand(String.Format(@"begin {0}.EMP_REGION_UPDATE(:p_EMP_REGION_ID, 
                            :P_TRANSFER_ID,
                            :p_REGION_SUBDIV_ID,
                            :p_DATE_START_WORK,
                            :p_DATE_END_WORK);End;", Connect.Schema), Connect.CurConnect);
            odaWorkPeriod.InsertCommand.BindByName = true;
            odaWorkPeriod.InsertCommand.Parameters.Add("p_EMP_REGION_ID", OracleDbType.Decimal, 0, "EMP_REGION_ID").Direction = ParameterDirection.InputOutput;
            odaWorkPeriod.InsertCommand.Parameters["p_EMP_REGION_ID"].DbType = DbType.Decimal;
            odaWorkPeriod.InsertCommand.Parameters.Add("P_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            odaWorkPeriod.InsertCommand.Parameters.Add("p_REGION_SUBDIV_ID", OracleDbType.Decimal, 0, "REGION_SUBDIV_ID");
            odaWorkPeriod.InsertCommand.Parameters.Add("p_DATE_START_WORK", OracleDbType.Date, 0, "DATE_START_WORK");
            odaWorkPeriod.InsertCommand.Parameters.Add("p_DATE_END_WORK", OracleDbType.Date, 0, "DATE_END_WORK");
            odaWorkPeriod.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaWorkPeriod.UpdateCommand = new OracleCommand(String.Format(@"begin {0}.EMP_REGION_UPDATE(:p_EMP_REGION_ID, 
                            :P_TRANSFER_ID,
                            :p_REGION_SUBDIV_ID,
                            :p_DATE_START_WORK,
                            :p_DATE_END_WORK);End;", Connect.Schema), Connect.CurConnect);
            odaWorkPeriod.UpdateCommand.BindByName = true;
            odaWorkPeriod.UpdateCommand.Parameters.Add("p_EMP_REGION_ID", OracleDbType.Decimal, 0, "EMP_REGION_ID").Direction = ParameterDirection.InputOutput;
            odaWorkPeriod.UpdateCommand.Parameters["p_EMP_REGION_ID"].DbType = DbType.Decimal;
            odaWorkPeriod.UpdateCommand.Parameters.Add("P_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            odaWorkPeriod.UpdateCommand.Parameters.Add("p_REGION_SUBDIV_ID", OracleDbType.Decimal, 0, "REGION_SUBDIV_ID");
            odaWorkPeriod.UpdateCommand.Parameters.Add("p_DATE_START_WORK", OracleDbType.Date, 0, "DATE_START_WORK");
            odaWorkPeriod.UpdateCommand.Parameters.Add("p_DATE_END_WORK", OracleDbType.Date, 0, "DATE_END_WORK");
            odaWorkPeriod.DeleteCommand = new OracleCommand(String.Format(@"begin {0}.EMP_REGION_DELETE(:p_EMP_REGION_ID);end;", Connect.Schema), Connect.CurConnect);
            odaWorkPeriod.DeleteCommand.BindByName = true;
            odaWorkPeriod.DeleteCommand.Parameters.Add("p_EMP_REGION_ID", OracleDbType.Decimal, 0, "EMP_REGION_ID");

            InitializeComponent();
            subdivSelector1.AppRoleName = "VS_EDIT";
            subdivSelector1.SubdivChanged += subdivSelector1_SubdivChanged;
            dgRegions.AutoGenerateColumns = dgEmps.AutoGenerateColumns = dgWorkerRegion.AutoGenerateColumns = false;
            ColumnWidthSaver.FillWidthOfColumn(dgRegions);
            ColumnWidthSaver.FillWidthOfColumn(dgEmps);
            ColumnWidthSaver.FillWidthOfColumn(dgWorkerRegion);
            dgRegions.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            dgEmps.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            dgWorkerRegion.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            tsdpCurDate.Value = DateTime.Today;
            tsdpCurDate.ValueChanged += new EventHandler(tsdpCurDate_ValueChanged);
            UpdateRegions(SubdivID);
            dgclRegionWorker.DisplayMember = "REGION_NAME";
            dgclRegionWorker.ValueMember = "REGION_SUBDIV_ID";
            dgclRegionWorker.DataSource = new DataView(ds.Tables["RegionSubdiv"], "", "REGION_NAME", DataViewRowState.CurrentRows);
            UpdateEmps(SubdivID, tsdpCurDate.Value);
            UpdateEmpRegions(null, null);
            dgRegions.DataSource = new DataView(ds.Tables["RegionSubdiv"], "", "REGION_NAME", DataViewRowState.CurrentRows);
            dgEmps.DataSource = new DataView(ds.Tables["Emps"], "", "", DataViewRowState.CurrentRows);
            dgWorkerRegion.DataSource = new DataView(ds.Tables["EmpRegion"], "", "DATE_START_WORK", DataViewRowState.CurrentRows);
        }

        public decimal? SubdivID
        {
            get
            {
                return subdivSelector1.SubdivId;
            }
        }

        void tsdpCurDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateEmps(SubdivID, tsdpCurDate.Value);
        }

        OracleDataAdapter odaRegions;
        private void UpdateRegions(object subdiv_id)
        {
            odaRegions.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            if (ds.Tables.Contains(odaRegions.TableMappings[0].DataSetTable))
                ds.Tables[odaRegions.TableMappings[0].DataSetTable].Rows.Clear();
            try
            {
                odaRegions.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }

        OracleDataAdapter odaEmps;
        private void UpdateEmps(object subdiv_id, object current_date)
        {
            odaEmps.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            odaEmps.SelectCommand.Parameters["p_date"].Value = current_date;
            if (ds.Tables.Contains(odaEmps.TableMappings[0].DataSetTable))
                ds.Tables[odaEmps.TableMappings[0].DataSetTable].Rows.Clear();
            try
            {
                odaEmps.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex), "Ошибка обновления");
            }
        }

        private void UpdateEmpRegions(object transfer_id, object subdiv_id)
        {
            if (ds.Tables.Contains(odaWorkPeriod.TableMappings[0].DataSetTable))
                ds.Tables[odaWorkPeriod.TableMappings[0].DataSetTable].Rows.Clear();
            odaWorkPeriod.SelectCommand.Parameters["p_transfer_id"].Value = transfer_id;
            odaWorkPeriod.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            odaWorkPeriod.Fill(ds);
        }

        void subdivSelector1_SubdivChanged(object sender, EventArgs e)
        {
            UpdateRegions(SubdivID);
            UpdateEmps(SubdivID, tsdpCurDate.Value);
        }

        private void tsbtSaveWorkerRegionVS_Click(object sender, EventArgs e)
        {
            dgWorkerRegion.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (ds.Tables.Contains("EmpRegion")) dgWorkerRegion.BindingContext[ds.Tables["EmpRegion"]].EndCurrentEdit();
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                odaWorkPeriod.Update(ds, "EmpRegion");
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                MessageBox.Show(Library.GetMessageException(ex), "Ошибка сохранения данных");
            }
        }

        private void dgEmps_SelectionChanged(object sender, EventArgs e)
        {
            object sel_tr = dgEmps.CurrentRow!=null?(dgEmps.CurrentRow.DataBoundItem as DataRowView)["TRANSFER_ID"]:null;
            UpdateEmpRegions(sel_tr, SubdivID);
        }

        private void tsbtAddWorkerRegionVS_Click(object sender, EventArgs e)
        {
            if (dgEmps.CurrentRow!=null)
            {
                DataRow r = ds.Tables[odaWorkPeriod.TableMappings[0].DataSetTable].NewRow();
                r["TRANSFER_ID"] = (dgEmps.CurrentRow.DataBoundItem as DataRowView)["TRANSFER_ID"];
                ds.Tables[odaWorkPeriod.TableMappings[0].DataSetTable].Rows.Add(r);
            }
            else
                MessageBox.Show("Не выбран сотрудник!");
        }

        private void tsbtDeleteWorkerRegionVS_Click(object sender, EventArgs e)
        {
            if (dgWorkerRegion.CurrentRow!=null && MessageBox.Show("Удалить запись об участке сотрудника?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)== System.Windows.Forms.DialogResult.Yes)
            {
                (dgWorkerRegion.CurrentRow.DataBoundItem as DataRowView).Delete();
            }
        }

        private void tsbtNewRegionVS_Click(object sender, EventArgs e)
        {
            if (SubdivID != null)
            {
                DataRow r = ds.Tables[odaRegions.TableMappings[0].DataSetTable].NewRow();
                r["SUBDIV_ID"] = SubdivID;
                ds.Tables[odaRegions.TableMappings[0].DataSetTable].Rows.Add(r);
            }
            else
                MessageBox.Show("Не выбрано подразделение!");
        }

        private void tsbtDeleteRegionVS_Click(object sender, EventArgs e)
        {
            if (dgRegions.CurrentRow!=null && MessageBox.Show("Удалить участок и все связанные с ним данные?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                (dgRegions.CurrentRow.DataBoundItem as DataRowView).Delete();
            }
        }

        private void tsbtSaveRegionsVac_Click(object sender, EventArgs e)
        {
            dgRegions.EndEdit(DataGridViewDataErrorContexts.Commit);
            dgRegions.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (ds.Tables.Contains("RegionSubdiv")) dgRegions.BindingContext[ds.Tables["RegionSubdiv"]].EndCurrentEdit();
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                odaRegions.Update(ds, "RegionSubdiv");
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }

        private void dgWorkerRegion_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            (sender as DataGridView).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void tsbtRefreshEmps_Click(object sender, EventArgs e)
        {
            UpdateRegions(SubdivID);
            UpdateEmps(SubdivID, tsdpCurDate.Value);
        }

        private void dgWorkerRegion_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

    }

        
}
