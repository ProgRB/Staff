using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace Kadr.Vacation_schedule
{
    
    public partial class EditVac : Form
    {
        OracleCommand cmd_end_vac,cmd_end_period, cmd_begin_period;
        object edit_vac_id, transfer_id;
        private DataSet ds;
        private OracleDataAdapter a;
        private BackgroundWorker bw = new BackgroundWorker();
        private void LockAll()
        {
            LockPlan();

             toolStrip2.Enabled = panel2.Enabled= group_changes.Enabled = btSaveVS.Enabled = stripEditVs.Enabled= false;
             gridActualVac.ReadOnly = true;
        }
        private void LockPlan()
        {
            toolStrip1.Enabled = panel3.Enabled = false;
            gridPlanVac.ReadOnly = true;
        }

        public EditVac(object _transfer_id) : this(null, _transfer_id, false) { }
        public EditVac(object _transfer_id, bool IsPlanCompose) : this(null, _transfer_id, IsPlanCompose) { }
        public EditVac(object vac_id, string _transfer_id) : this(vac_id, _transfer_id, false) { }

        public EditVac(object vac_id, object _transfer_id,bool IsPlanCompose)
        {
            edit_vac_id = vac_id;
            transfer_id = _transfer_id;
            cmd_end_vac = new OracleCommand(string.Format("select {0}.END_OF_VAC(:p_cal_days,:p_work_days,:p_usual_days,:p_date) from dual",Connect.Schema),Connect.CurConnect);
            cmd_end_vac.BindByName = true;
            cmd_end_vac.Parameters.Add("p_cal_days", OracleDbType.Decimal);
            cmd_end_vac.Parameters.Add("p_work_days", OracleDbType.Decimal);
            cmd_end_vac.Parameters.Add("p_usual_days", OracleDbType.Decimal );
            cmd_end_vac.Parameters.Add("p_date",OracleDbType.Date);
            cmd_end_period = new OracleCommand(string.Format("select {0}.VAC_SCHED_PACK.CALC_END_PERIOD(:p_date, :type_vac_id,:cnt_d, :p_transfer_id, :p_date_vac) from dual", Connect.Schema), Connect.CurConnect);
            cmd_end_period.BindByName = true;
            cmd_end_period.Parameters.Add("p_date", OracleDbType.Date);
            cmd_end_period.Parameters.Add("cnt_d", OracleDbType.Decimal);
            cmd_end_period.Parameters.Add("type_vac_id", OracleDbType.Decimal);
            cmd_end_period.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value=transfer_id;
            cmd_end_period.Parameters.Add("p_date_vac", OracleDbType.Decimal);
            cmd_begin_period = new OracleCommand(string.Format("select {0}.VAC_SCHED_PACK.CALC_BEGIN_PERIOD(:p_date, :type_vac_id,:cnt_d, :p_transfer_id, :p_date_vac) from dual", Connect.Schema), Connect.CurConnect);
            cmd_begin_period.BindByName = true;
            cmd_begin_period.Parameters.Add("p_date", OracleDbType.Date);
            cmd_begin_period.Parameters.Add("cnt_d", OracleDbType.Decimal);
            cmd_begin_period.Parameters.Add("type_vac_id", OracleDbType.Decimal);
            cmd_begin_period.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
            cmd_begin_period.Parameters.Add("p_date_vac", OracleDbType.Decimal);
            InitializeComponent();
            this.SuspendLayout();
            gridActualVac.AutoGenerateColumns = false;
            gridPlanVac.AutoGenerateColumns = false;
            
            ds = new DataSet();
#region Инициализация Датасета
            a = new OracleDataAdapter(string.Format("begin {0}.VAC_SCHED_PACK.GET_VAC_DATA(:p,:main_data,:det_data1,:type_vacs);end;", DataSourceScheme.SchemeName), Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p", edit_vac_id);
            a.SelectCommand.Parameters.Add("main_data", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            a.SelectCommand.Parameters.Add("det_data1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            a.SelectCommand.Parameters.Add("type_vacs", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            a.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.UPD_VAC_CONSIST(
                                                                 :p_VAC_CONSIST_ID
                                                                ,:p_VAC_SCHED_ID
                                                                ,:p_TYPE_VAC_ID
                                                                ,:p_PERIOD_BEGIN
                                                                ,:p_PERIOD_END
                                                                ,:p_COUNT_DAYS
                                                                ,:p_PLAN_SIGN
                                                                ,:p_STABLE_PERIOD_SIGN);END;", Connect.Schema), Connect.CurConnect);
            a.InsertCommand.BindByName = true;
            a.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            a.InsertCommand.Parameters.Add("p_VAC_CONSIST_ID", OracleDbType.Int16, 0, "VAC_CONSIST_ID").Direction = ParameterDirection.InputOutput;
            a.InsertCommand.Parameters["p_VAC_CONSIST_ID"].DbType = DbType.Decimal;
            a.InsertCommand.Parameters.Add("p_VAC_SCHED_ID", OracleDbType.Decimal);
            a.InsertCommand.Parameters.Add("p_TYPE_VAC_ID", OracleDbType.Decimal, 0, "TYPE_VAC_ID");
            a.InsertCommand.Parameters.Add("p_PERIOD_BEGIN", OracleDbType.Date, 8, "PERIOD_BEGIN");
            a.InsertCommand.Parameters.Add("p_PERIOD_END", OracleDbType.Date, 8, "PERIOD_END");
            a.InsertCommand.Parameters.Add("p_COUNT_DAYS", OracleDbType.Decimal, 0, "COUNT_DAYS");
            a.InsertCommand.Parameters.Add("p_PLAN_SIGN", OracleDbType.Decimal, 0, "PLAN_SIGN");
            a.InsertCommand.Parameters.Add("p_STABLE_PERIOD_SIGN", OracleDbType.Decimal, 0,"STABLE_PERIOD_SIGN");
            a.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.UPD_VAC_CONSIST(
                                                                 :p_VAC_CONSIST_ID
                                                                ,:p_VAC_SCHED_ID
                                                                ,:p_TYPE_VAC_ID
                                                                ,:p_PERIOD_BEGIN
                                                                ,:p_PERIOD_END
                                                                ,:p_COUNT_DAYS
                                                                ,:p_PLAN_SIGN
                                                                ,:p_STABLE_PERIOD_SIGN);END;", Connect.Schema), Connect.CurConnect);
            a.UpdateCommand.BindByName = true;
            a.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            a.UpdateCommand.Parameters.Add("p_VAC_CONSIST_ID", OracleDbType.Int32, 0, "VAC_CONSIST_ID").Direction = ParameterDirection.InputOutput;
            a.UpdateCommand.Parameters["p_VAC_CONSIST_ID"].DbType = DbType.Decimal;
            a.UpdateCommand.Parameters.Add("p_VAC_SCHED_ID", OracleDbType.Decimal);
            a.UpdateCommand.Parameters.Add("p_TYPE_VAC_ID", OracleDbType.Decimal,0,"TYPE_VAC_ID");
            a.UpdateCommand.Parameters.Add("p_PERIOD_BEGIN", OracleDbType.Date, 8, "PERIOD_BEGIN");
            a.UpdateCommand.Parameters.Add("p_PERIOD_END", OracleDbType.Date,8, "PERIOD_END");
            a.UpdateCommand.Parameters.Add("p_COUNT_DAYS", OracleDbType.Decimal,0,"COUNT_DAYS");
            a.UpdateCommand.Parameters.Add("p_PLAN_SIGN", OracleDbType.Decimal,0,"PLAN_SIGN");
            a.UpdateCommand.Parameters.Add("p_STABLE_PERIOD_SIGN", OracleDbType.Decimal, 0, "STABLE_PERIOD_SIGN");
            a.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.DEL_VAC_CONSIST(:p_VAC_CONSIST_ID);END;", Connect.Schema), Connect.CurConnect);
            a.DeleteCommand.BindByName = true;
            a.DeleteCommand.Parameters.Add("p_VAC_CONSIST_ID", OracleDbType.Int32, 0, "VAC_CONSIST_ID");
            a.TableMappings.Add("Table", "main");
            a.TableMappings.Add("Table1", "vacs");
            a.TableMappings.Add("Table2", "type_vac");
            a.Fill(ds);

            OracleDataAdapter b = new OracleDataAdapter(string.Format("begin {0}.VAC_SCHED_PACK.GET_ROUND_AND_START_PERIOD(:p_transfer_id,:p1); END;",Connect.Schema),Connect.CurConnect);
            b.SelectCommand.BindByName = true;
            b.SelectCommand.Parameters.Add("p_transfer_id", transfer_id);
            b.SelectCommand.Parameters.Add("p1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            b.TableMappings.Add("Table","period_data");
            b.Fill(ds);

            ds.Tables["vacs"].Columns.Add("all",typeof(decimal),"SUM(COUNT_DAYS)");

            ds.Tables["vacs"].ParentRelations.Add("type_vac_id_fk", ds.Tables["type_vac"].Columns["type_vac_id"], ds.Tables["vacs"].Columns["type_vac_id"],false);
            /*ds.Relations.Add("type_vac_id_fk31", ds.Tables["period_data"].Columns["type_vac_id"], ds.Tables["vacs1"].Columns["type_vac_id"], false);
            ds.Relations.Add("type_vac_id_fk32", ds.Tables["period_data"].Columns["type_vac_id"], ds.Tables["vacs2"].Columns["type_vac_id"], false);*/
#endregion
            
        #region Создание столбцов таблиц
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Format = "D";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            // 
            // actual_type_vac_cl
            // 
            MDataGridViewComboBoxColumn c1 = new MDataGridViewComboBoxColumn("type_vac_cl", "Тип отпуска", "type_vac_id", "type_vac_id", "NAME_VAC", new DataView(ds.Tables["type_vac"], "", "type_vac_id", DataViewRowState.CurrentRows));
            c1.Frozen = true;
            c1.Width = 180;
            // 
            // actual_count_days_cl
            //
            MDataGridViewNumericColumn c2 = new LibraryKadr.MDataGridViewNumericColumn("count_days_cl","Количество дней","count_days");
            c2.DecimalPlaces = 0;
            c2.MaximumValue = Decimal.MaxValue;
            c2.MinimumValue = 0m;
            // 
            // actual_period_begin_cl
            // 
            MDataGridViewCalendarColumn c3 = new LibraryKadr.MDataGridViewCalendarColumn("period_begin_cl", "Начало периода", "period_begin");
            c3.DateFormat = "dd.MM.yyyy";
            c3.Resizable = DataGridViewTriState.True;
            c3.ToolTipText = "Начало периода за который предоставляется отпуск";
            // 
            // actual_period_end_cl
            // 
            MDataGridViewCalendarColumn c4 = new LibraryKadr.MDataGridViewCalendarColumn("period_end_cl", "Окончание периода", "period_end");
            c4.DateFormat = "dd.MM.yyyy";
            c4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            c4.ToolTipText = "Окончание периода за который предоставляется отпуск";
            // 
            // actual_begin_cl
            // 
            MDataGridViewTextBoxColumn c5 = new MDataGridViewTextBoxColumn("begin_vac_cl", "Начало отпуска", "plan_begin");
            c5.DefaultCellStyle = dataGridViewCellStyle1;
            c5.ReadOnly = true;
            c5.SortMode = DataGridViewColumnSortMode.NotSortable;

            // 
            // actual_end_cl
            // 
            DataGridViewTextBoxColumn c6 = new MDataGridViewTextBoxColumn("end_vac_cl", "Окончание отпуска", "plan_end");
            c6.DefaultCellStyle = dataGridViewCellStyle1;
            c6.ReadOnly = true;
            c6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // stable period cl
            //
            DataGridViewCheckBoxColumn c7 = new MDataGridViewCheckBoxColumn("stable_period_sign_cl", "Признак периода", "STABLE_PERIOD_SIGN");
            c7.Width = 60;
            c7.ToolTipText = "Признак неизменного периода - период не будет меняться или проверяться при проверке и перерасчете периодов";
            c7.ReadOnly = !ControlAccess.GetAccess(c7);
            // 
            // plan_type_vs
            // 
            DataGridViewComboBoxColumn cc1 = new MDataGridViewComboBoxColumn("type_vac_cl","Тип отпуска", "type_vac_id", "type_vac_id", "NAME_VAC", new DataView(ds.Tables["type_vac"],"","type_vac_id", DataViewRowState.CurrentRows));
            cc1.Frozen = true;
            cc1.Width = 180;

            // 
            // plan_count_days_cl
            // 
            MDataGridViewNumericColumn cc2 = new LibraryKadr.MDataGridViewNumericColumn("count_days_cl","Количество дней","count_days");
            cc2.DecimalPlaces = 0;
            cc2.MaximumValue = decimal.MaxValue;
            cc2.MinimumValue = 0m;
            // 
            // plan_period_begin_cl
            // 
            MDataGridViewCalendarColumn cc3 = new LibraryKadr.MDataGridViewCalendarColumn("period_begin_cl", "Начало периода", "period_begin");
            cc3.DateFormat = "dd.MM.yyyy";
            cc3.Resizable = DataGridViewTriState.True;
            cc3.ToolTipText = "Начало периода за который предоставляется отпуск";
            // 
            // plan_period_end_cl
            // 
            MDataGridViewCalendarColumn cc4 = new LibraryKadr.MDataGridViewCalendarColumn("period_end_cl", "Окончание периода", "period_end");
            cc4.DateFormat = "dd.MM.yyyy";
            cc4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            cc4.ToolTipText = "Окончание периода за который предоставляется отпуск";
            // 
            // plan_begin_cl
            // 
            MDataGridViewTextBoxColumn cc5 = new MDataGridViewTextBoxColumn("begin_vac_cl", "Начало отпуска", "plan_begin");
            cc5.DefaultCellStyle = dataGridViewCellStyle1;
            cc5.ReadOnly = true;
            cc5.SortMode = DataGridViewColumnSortMode.NotSortable;
            cc5.DefaultCellStyle= dataGridViewCellStyle1;
            // 
            // plan_end_cl
            // 
            DataGridViewTextBoxColumn cc6 = new MDataGridViewTextBoxColumn("end_vac_cl", "Окончание отпуска", "plan_end");
            cc6.DefaultCellStyle = dataGridViewCellStyle1;
            cc6.ReadOnly = true;
            cc6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // stable period cl
            //
            DataGridViewCheckBoxColumn cc7 = new MDataGridViewCheckBoxColumn("stable_period_sign_cl", "Признак периода", "STABLE_PERIOD_SIGN");
            cc7.Width = 60;
            cc7.ToolTipText = "Признак неизменного периода - Период не будет меняться или проверяться при проверке и перерасчете периодов";
            cc7.ReadOnly = !ControlAccess.GetAccess(cc7);

            this.gridPlanVac.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {c1,c2,c3,c4,c5,c6,c7});
            this.gridActualVac.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {cc1,cc2,cc3,cc4,cc5,cc6,cc7});

            gridPlanVac.DataSource = new DataView(ds.Tables["vacs"], "PLAN_SIGN=1", "", DataViewRowState.CurrentRows);
            gridActualVac.DataSource = new DataView(ds.Tables["vacs"], "PLAN_SIGN=0", "", DataViewRowState.CurrentRows);

            ColumnWidthSaver.FillWidthOfColumn(gridActualVac);
            ColumnWidthSaver.FillWidthOfColumn(gridPlanVac);
            gridPlanVac.RowsAdded += new DataGridViewRowsAddedEventHandler(gridVac_RowsAdded);
            gridActualVac.RowsAdded += gridVac_RowsAdded;
            gridActualVac.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            gridPlanVac.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            plan_begin.Validating += Library.ValidatingDate;
            plan_end.Validating += Library.ValidatingDate;
            actual_begin.Validating += Library.ValidatingDate;
            actual_end.Validating += Library.ValidatingDate;
        #endregion
            
            ds.Tables["vacs"].ColumnChanged+=new DataColumnChangeEventHandler(vacs_ColumnChanged);
            gridPlanVac.CellValueChanged += new DataGridViewCellEventHandler(gridVac_CellValueChanged);
            gridActualVac.CellValueChanged += new DataGridViewCellEventHandler(gridVac_CellValueChanged);
            ds.Tables["main"].ColumnChanging += new DataColumnChangeEventHandler(main_ColumnChanging);

            /*создаем команду получения даты "Округления" текущего периода отпуска*/

            if (ds.Tables["main"].Rows.Count == 0)
            {
                DataRow r = ds.Tables["main"].NewRow();
                r["TRANSFER_ID"] = transfer_id;
                r["CLOSE_SIGN"] = decimal.Zero;
                r["CONFIRM_SIGN"] = decimal.Zero;
                ds.Tables["main"].Rows.Add(r);                
            }
            stripEditVs.EnableByRules(false);

            actual_begin.DataBindings.Add("Text", ds.Tables["main"], "ACTUAL_BEGIN",true, DataSourceUpdateMode.OnPropertyChanged);
            actual_end.DataBindings.Add("Text", ds.Tables["main"], "ACTUAL_END", true, DataSourceUpdateMode.OnPropertyChanged);
            plan_begin.DataBindings.Add("Text", ds.Tables["main"], "PLAN_BEGIN", true, DataSourceUpdateMode.OnPropertyChanged);
            plan_end.DataBindings.Add("Text", ds.Tables["main"], "PLAN_END", true, DataSourceUpdateMode.OnPropertyChanged);
            
            tscbCloseSign.CheckBoxControl.DataBindings.Add("Checked", ds.Tables["main"], "CLOSE_SIGN", true, DataSourceUpdateMode.OnPropertyChanged);
            tscbCloseSign.CheckBoxControl.DataBindings[0].Format += new ConvertEventHandler(tsCloseSign_Format);
            tscbCloseSign.CheckBoxControl.DataBindings[0].Parse += new ConvertEventHandler(tsCloseSign_Parse);
            tscbConfirmSign.CheckBoxControl.DataBindings.Add("Checked", ds.Tables["main"], "CONFIRM_SIGN", true, DataSourceUpdateMode.OnPropertyChanged);
            tscbConfirmSign.CheckBoxControl.DataBindings[0].Format += new ConvertEventHandler(tsCloseSign_Format);
            tscbConfirmSign.CheckBoxControl.DataBindings[0].Parse += new ConvertEventHandler(tsCloseSign_Parse);

            DATE_CONFIRM.Text = (ds.Tables["main"].Rows[0]["DATE_CONFIRM"] != DBNull.Value ? ds.Tables["main"].Rows[0].Field<DateTime>("DATE_CONFIRM").ToShortDateString() : "");
            DATE_CLOSE.Text = (ds.Tables["main"].Rows[0]["DATE_CLOSE"] != DBNull.Value ? ds.Tables["main"].Rows[0].Field<DateTime>("DATE_CLOSE").ToShortDateString() : "");

            if (!GrantedRoles.GetGrantedRole("STAFF_VS_TOTAL_LOCK"))
            {
                if (Main["CLOSE_SIGN"].ToString() == "2") LockAll();
                if (Main["CLOSE_SIGN"].ToString() == "1" && (!GrantedRoles.GetGrantedRole("STAFF_VS_CLOSING") || Main["ACTUAL_BEGIN"] == DBNull.Value || Main.Field<DateTime>("ACTUAL_BEGIN") < DateTime.Now.AddDays(-30))) LockAll();
                if (!GrantedRoles.GetGrantedRole("STAFF_VS_CONFIRM") && Main["CONFIRM_SIGN"].ToString() == "1") LockPlan();
            }
                
            actual_begin.DataBindings[0].Parse += new ConvertEventHandler(MaskedDate_Parse);
            plan_begin.DataBindings[0].Parse += new ConvertEventHandler(MaskedDate_Parse);
            this.ResumeLayout();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        }

        
        void gridVac_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var c = (sender as DataGridView).Columns.OfType<DataGridViewColumn>().Where(n => n.DataPropertyName.ToUpper() == "TYPE_VAC_ID");
            int k = (c.ElementAt(0) as DataGridViewColumn).Index;
            for (int i = e.RowIndex; i < (sender as DataGridView).RowCount; ++i)
                gridVac_CellValueChanged(sender, new DataGridViewCellEventArgs(k, i));
        }

        void tsCloseSign_Parse(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = ((bool)e.Value ? 1m : 0m);
            }
        }

        void tsCloseSign_Format(object sender, ConvertEventArgs e)
        {

            e.Value = (decimal.Parse(e.Value.ToString())>0 ? true : false);
        }

        void main_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName.ToUpper() == "PLAN_BEGIN")
            {
                e.Row["PLAN_END"] = GetEndVac(gridPlanVac.DataSource as DataView, e.ProposedValue);
                plan_end.DataBindings[0].ReadValue();
            }
            if (e.Column.ColumnName.ToUpper() == "ACTUAL_BEGIN")
            {
                e.Row["ACTUAL_END"] = GetEndVac(gridActualVac.DataSource as DataView, e.ProposedValue);
                actual_begin.DataBindings[0].ReadValue();
            }
        }

        
        void  vacs_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Deleted) return;
            if (e.Column.ColumnName.ToUpper() == "TYPE_VAC_ID" || e.Column.ColumnName.ToUpper() == "COUNT_DAYS")
            {
                DataRow r;
                if (e.Row["type_vac_id"] != DBNull.Value && (r = e.Row.GetParentRow("type_vac_id_fk")) != null)
                {
                    e.Row["PLAN_END"] = GetEndVac(e.Row, e.Row["PLAN_BEGIN"]);
                    if (r["NEED_PERIOD"].ToString() == "0")
                        e.Row["PERIOD_BEGIN"] = e.Row["PERIOD_END"] = DBNull.Value;
                    else
                        if (e.Column.ColumnName.ToUpper()== "TYPE_VAC_ID")
                            e.Row["PERIOD_BEGIN"] = GetBeginPeriod(e.Row);
                }
                else
                    e.Row["PLAN_END"] = DBNull.Value;
                e.Row["PERIOD_END"]=GetEndPeriod(e.Row,e.Row["PERIOD_BEGIN"]);
            }
            if (e.Column.ColumnName.ToUpper() == "PERIOD_BEGIN")
            {
                e.Row["PERIOD_END"] = GetEndPeriod(e.Row, e.Row["PERIOD_BEGIN"]);
            }
            if (sender == gridPlanVac.DataSource)
            {
                Main["PLAN_END"] = GetEndVac(gridPlanVac.DataSource as DataView, Main["PLAN_BEGIN"]);
                plan_begin.DataBindings[0].ReadValue();
                gridPlanVac.Invalidate();
            }
            else
            {
                Main["ACTUAL_END"] = GetEndVac(gridActualVac.DataSource as DataView, Main["ACTUAL_BEGIN"]);
                actual_begin.DataBindings[0].ReadValue();
                gridActualVac.Invalidate();
            }
        }

        private void gridVac_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView d = sender as DataGridView;
            if (d.Columns[e.ColumnIndex].DataPropertyName.ToUpper() == "TYPE_VAC_ID")
            {
                DataRow r;
                if ((r = Tb1.Rows[e.RowIndex].GetParentRow("type_vac_id_fk")) != null && r["NEED_PERIOD"].ToString() == "1")
                    d["period_begin_cl", e.RowIndex].ReadOnly =
                        d["period_end_cl", e.RowIndex].ReadOnly = false;
                else
                    d["period_begin_cl", e.RowIndex].ReadOnly =
                        d["period_end_cl", e.RowIndex].ReadOnly = true;
            }
        }
        
        public DataTable Tb1
        {
            get
            { return ds.Tables["vacs"]; }
        }
        public DataRow Main
        {
            get
            {
                return ds.Tables["main"].Rows[0];
            }
        }
        public DataTable period_data
        {
            get
            {
                return ds.Tables["period_data"];
            }
        }

        private object GetEndVac(DataRow r, object e)
        {
            foreach (OracleParameter i in cmd_end_vac.Parameters)
                i.Value=null;
            DataRow d = r.GetParentRow("type_vac_id_fk");
            if (r.RowState!= DataRowState.Deleted && d != null)
            {
                switch (d["TYPE_VAC_CALC_ID"].ToString())
                {
                    case "1": cmd_end_vac.Parameters["p_cal_days"].Value=r["COUNT_DAYS"]; break;
                    case "2": cmd_end_vac.Parameters["p_work_days"].Value=r["COUNT_DAYS"]; break;
                    case "3": cmd_end_vac.Parameters["p_usual_days"].Value=r["COUNT_DAYS"]; break;
                }
                cmd_end_vac.Parameters["p_date"].Value = e;
                return cmd_end_vac.ExecuteScalar();
            }
            return DBNull.Value;
        }

        private object GetEndVac(DataView t, object e)
        {
            if (e != null)
            {
                decimal i = 0, j = 0, k = 0;
                DataRow d;
                foreach (DataRowView r in t)
                {
                    if (r["COUNT_DAYS"] != DBNull.Value && (d = r.Row.GetParentRow("type_vac_id_fk")) != null)
                    {
                        switch (d["TYPE_VAC_CALC_ID"].ToString())
                        {
                            case "1": i += int.Parse(r["COUNT_DAYS"].ToString()); break;
                            case "2": j += decimal.Parse(r["COUNT_DAYS"].ToString()); break;
                            case "3": k += decimal.Parse(r["COUNT_DAYS"].ToString()); break;
                        }
                    }
                }
                cmd_end_vac.Parameters["p_cal_days"].Value = i;
                cmd_end_vac.Parameters["p_work_days"].Value = j;
                cmd_end_vac.Parameters["p_usual_days"].Value = k;
                cmd_end_vac.Parameters["p_date"].Value = e;
                return cmd_end_vac.ExecuteScalar();
            }
            return DBNull.Value;
        }


        private object GetBeginPeriod(DataRow r)
        {
            try
            {
                DateTime period = DateTime.Parse(period_data.Compute("MAX(period_end)", string.Format("type_vac_id={0} and plan_sign={1}", r["type_vac_id"], r["plan_sign"])).ToString());
                DataRow d = r.GetParentRow(ds.Relations["type_vac_id_fk"]);
                Object k = d["VAC_GROUP_TYPE_ID"];
                Object period2 = Tb1.Compute("MAX(PERIOD_END)", string.Format("PARENT(type_vac_id_fk).VAC_GROUP_TYPE_ID={0} and plan_sign={1}", k, r["PLAN_SIGN"]));
                if (period2!=DBNull.Value && ((DateTime)period2)>period)
                    period = ((DateTime) period2).AddSeconds(1); 
                return period;
            }
            catch
            {
                return DBNull.Value;
            }
                   
        }

        private object GetEndPeriod(DataRow r, object e)
        {
            try
            {
                DataRow d = r.GetParentRow("type_vac_id_fk");
                if (d != null && d["NEED_PERIOD"].ToString() == "1")
                {
                    cmd_end_period.Parameters["P_DATE"].Value = e;
                    cmd_end_period.Parameters["cnt_d"].Value = r["COUNT_DAYS"];
                    cmd_end_period.Parameters["type_vac_id"].Value = r["TYPE_VAC_ID"];
                    return cmd_end_period.ExecuteScalar();
                }
                return DBNull.Value;
            }
            catch
            {
                return DBNull.Value;
            }
        }

        void MaskedDate_Parse(object sender, ConvertEventArgs e)
        {
            if (string.IsNullOrEmpty(((MaskedTextBox)(((Binding)sender).Control)).MaskedTextProvider.ToString(false, false)) &&
                ((MaskedTextBox)(((Binding)sender).Control)).MaskedTextProvider.Mask=="00-00-0000")
                e.Value = DBNull.Value;
        }


        private void btSaveVS_Click(object sender, EventArgs e)
        {
            gridActualVac.EndEdit();
            gridPlanVac.EndEdit();
#region Не критичная валидация отпуска
            if (Main["plan_begin"] != DBNull.Value && Tb1.Rows.Cast<DataRow>().Count(t=>t.RowState!= DataRowState.Deleted && t["PLAN_SIGN"].ToString()=="1")==0)
            {
                MessageBox.Show("Указана плановая дата отпуска, но не указано ни одного типа отпуска, ни количество дней!");
                return;
            }
            if (Tb1.Rows.Cast<DataRow>().Count(t => t.RowState!= DataRowState.Deleted && t["TYPE_VAC_ID"] == DBNull.Value) > 0)
            {
                MessageBox.Show("Требуется обязательно выбрать тип отпуска");
                return;
            }
            if (Main["actual_begin"] != DBNull.Value && Tb1.Rows.Cast<DataRow>().Count(t => t.RowState!= DataRowState.Deleted &&  t["PLAN_SIGN"].ToString() == "0") == 0)
            {
                MessageBox.Show("Указана дата отпуска, но не указано ни одного типа отпуска, ни количество дней!");
                return;
            }
           
            if (!plan_begin.MaskFull && !actual_begin.MaskFull)
            {
                MessageBox.Show(this, "Введите фактическую или плановую дату начала отпуска", "АРМ Кадры");
                return;
            }
#endregion
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                OracleCommand check_dates = new OracleCommand(string.Format(Queries.GetQuery(@"go\CheckDatesValid.sql"), Connect.Schema), Connect.CurConnect);
                check_dates.BindByName = true;
                check_dates.Parameters.Add("p_transfer_id", OracleDbType.Decimal, Main["transfer_id"], ParameterDirection.Input);
                check_dates.Parameters.Add("p_vac_sched_id", OracleDbType.Decimal, Main["vac_sched_id"], ParameterDirection.Input);
                check_dates.Parameters.Add("p_vac_begin", OracleDbType.Date, (Main["actual_begin"] != DBNull.Value ? Main["actual_begin"] : Main["Plan_begin"]), ParameterDirection.Input);
                check_dates.Parameters.Add("p_vac_end", OracleDbType.Date, (Main["actual_begin"] != DBNull.Value ? GetEndVac(gridActualVac.DataSource as DataView, Main["actual_begin"]) : GetEndVac(gridPlanVac.DataSource as DataView, Main["Plan_begin"])), ParameterDirection.Input);
                check_dates.ExecuteNonQuery();
                OracleCommand cmd = new OracleCommand(string.Format(@"begin {0}.VACATION_SCHEDULE_update( :p_VAC_SCHED_ID
                                                            ,:p_TRANSFER_ID 
                                                            ,:p_PLAN_BEGIN
                                                            ,:p_ACTUAL_BEGIN
                                                            ,:p_UNUSED_DAYS
                                                            ,:p_COMMENTS 
                                                            ,:p_CONFIRM_SIGN
                                                            ,:p_DATE_CONFIRM
                                                            ,:p_CLOSE_SIGN
                                                            ,:p_DATE_CLOSE
                                                            );end;", Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Transaction = tr;
                cmd.Parameters.Add("p_vac_sched_id", OracleDbType.Decimal, Main["vac_sched_id"], ParameterDirection.InputOutput);
                cmd.Parameters.Add("p_TRANSFER_ID", Main["TRANSFER_ID"]);
                cmd.Parameters.Add("p_PLAN_BEGIN", Main["PLAN_BEGIN"]);
                cmd.Parameters.Add("p_ACTUAL_BEGIN", Main["ACTUAL_BEGIN"]);
                cmd.Parameters.Add("p_UNUSED_DAYS", Main["UNUSED_DAYS"]);
                cmd.Parameters.Add("p_COMMENTS", OracleDbType.Varchar2, 100, Main["COMMENTS"], ParameterDirection.Input);
                cmd.Parameters.Add("p_CONFIRM_SIGN", Main["CONFIRM_SIGN"]);
                cmd.Parameters.Add("p_DATE_CONFIRM", Main["DATE_CONFIRM"]);
                cmd.Parameters.Add("p_CLOSE_SIGN", Main["CLOSE_SIGN"]);
                cmd.Parameters.Add("p_DATE_CLOSE", Main["DATE_CLOSE"]);
                cmd.ExecuteNonQuery();
                if (Main["vac_sched_id"] == DBNull.Value)
                    Main["vac_sched_id"] = ((OracleDecimal)cmd.Parameters["p_vac_sched_id"].Value).Value;
                a.InsertCommand.Transaction = tr;
                a.UpdateCommand.Transaction = tr;
                a.DeleteCommand.Transaction = tr;
                a.InsertCommand.Parameters["p_VAC_SCHED_ID"].Value = Main["vac_sched_id"];
                a.UpdateCommand.Parameters["p_VAC_SCHED_ID"].Value = Main["vac_sched_id"];
                a.Update(Tb1);                
                tr.Commit();
                Main.AcceptChanges();
                if (cbRecalcPeriods.Checked)
                    bw.RunWorkerAsync();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                tr.Rollback();

                MessageBox.Show("Ошибка сохранения: " + Library.GetMessageException(ex), "Графики отпусков");
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.UPDATE_VAC_PERIODS(:p_vac_sched);end;", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_vac_sched", OracleDbType.Decimal, Main["vac_sched_id"], ParameterDirection.Input);
                cmd.ExecuteNonQuery();
            }
            catch { };
        }

        
        private void tsbtPlanVsToFactVS_Click(object sender, EventArgs e)
        {
            gridPlanVac.EndEdit();
            gridPlanVac.BindingContext[gridPlanVac.DataSource].EndCurrentEdit();
            if (MessageBox.Show(this, "Проставить все поля фактического отпуска из планового?", "Графики отпусков", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Main["ACTUAL_BEGIN"] = Main["PLAN_BEGIN"];
                Main["ACTUAL_END"] = Main["PLAN_END"];
                for (int i = Tb1.Rows.Count - 1; i > -1; --i)
                    if (Tb1.Rows[i].RowState != DataRowState.Deleted && Tb1.Rows[i].RowState!= DataRowState.Detached && Tb1.Rows[i]["PLAN_SIGN"].ToString() == "0")
                        Tb1.Rows[i]. Delete();
                int k = Tb1.Rows.Count;
                for (int i=0;i<k;++i)
                    if (Tb1.Rows[i].RowState != DataRowState.Deleted && Tb1.Rows[i].RowState!= DataRowState.Detached)
                {
                    Tb1.Rows.Add(new object[] { DBNull.Value, Main["VAC_SCHED_ID"], Tb1.Rows[i]["TYPE_VAC_ID"], Tb1.Rows[i]["PERIOD_BEGIN"], Tb1.Rows[i]["PERIOD_END"], Tb1.Rows[i]["COUNT_DAYS"], (decimal)0m, Tb1.Rows[i]["PLAN_BEGIN"], Tb1.Rows[i]["PLAN_END"], Tb1.Rows[i]["STABLE_PERIOD_SIGN"] });
                }
                actual_begin.DataBindings[0].ReadValue();
            }
        }

        private void tsbtAddPlanVac_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables["vacs"].NewRow();
            r["PLAN_SIGN"] = 1m;
            DateTime d = DateTime.MinValue;
            foreach (DataRow i in Tb1.Rows)
                d = (i.RowState == DataRowState.Deleted || i["PLAN_END"] == DBNull.Value || i["PLAN_SIGN"].ToString()=="0" ? DateTime.MinValue : i.Field<DateTime>("PLAN_END"));
            if (d != DateTime.MinValue)
                r["PLAN_BEGIN"] = d.Date.AddDays(1);
            else
                r["PLAN_BEGIN"] = Main["PLAN_BEGIN"];
            ds.Tables["vacs"].Rows.Add(r);
        }

        private void tsbtAddActualVac_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables["vacs"].NewRow();
            r["PLAN_SIGN"] = (decimal)0m;
            DateTime d = DateTime.MinValue;
            foreach (DataRow i in Tb1.Rows)
                d = (i.RowState == DataRowState.Deleted || i["PLAN_END"] == DBNull.Value || i["PLAN_SIGN"].ToString() == "1" ? DateTime.MinValue : i.Field<DateTime>("PLAN_END"));
            if (d != DateTime.MinValue)
                r["PLAN_BEGIN"] = d.Date.AddDays(1);
            else
                r["PLAN_BEGIN"] = Main["ACTUAL_BEGIN"];
            ds.Tables["vacs"].Rows.Add(r);
        }

        private void tsbtDelPlanVac_Click(object sender, EventArgs e)
        {
            if (gridPlanVac.CurrentRow!=null)
                gridPlanVac.Rows.Remove(gridPlanVac.CurrentRow);
        }

        private void tsbtDelActualVac_Click(object sender, EventArgs e)
        {
            if (gridActualVac.CurrentRow != null) 
                gridActualVac.Rows.Remove(gridActualVac.CurrentRow);
        }

        private void gridPlanVac_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
