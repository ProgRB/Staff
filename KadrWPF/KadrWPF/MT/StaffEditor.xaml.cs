using EntityGenerator;
using KadrWPF.Helpers;
using LibraryKadr;
using LibrarySalary.Helpers;
using Oracle.DataAccess.Client;
using Salary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for StaffEditor.xaml
    /// </summary>
    public partial class StaffEditor : Window
    {
        StaffViewModel _model;
        public StaffEditor(decimal? staffID)
        {
            _model = new StaffViewModel(staffID);
            InitializeComponent();
            ValueToIDConverter c = (ValueToIDConverter)FindResource("OrderToNameConverter");
            c.ValueCollection = _model.OrderSource;
            DataContext = Model;
        }
        public StaffViewModel Model
        {
            get
            {
                return _model;
            }
        }

        public static RoutedUICommand ChooseGrWork { get; private set; }
        public static RoutedUICommand DeleteGrWork { get; private set; }

        static StaffEditor()
        {
            ChooseGrWork = new RoutedUICommand("Выбрать график работы", "EditStaff", typeof(StaffEditor));
            DeleteGrWork = new RoutedUICommand("Убрать график работы", "EditStaff", typeof(StaffEditor));
        }

        private void ChooseWork_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }

        private void ChooseWork_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GrWorkForm f = new GrWorkForm();
            f.Model.CodeSubdiv = this.Model.CodeSubdiv;
            f.Model.DateEndGraph = DateTime.Today;
            f.Model.RefreshGrWork(Model.GrWorkID);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                this.Model.GrWorkID = f.Model.CurrentGrWorkID;
            }
        }

        private void DeleteGrWork_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model!=null && Model.GrWork!=null;
        }

        private void DeleteGrWork_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.GrWorkID = null;
        }

        private void ChooseWorkPlace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WorkPlaceForm f = new WorkPlaceForm();
            f.workPlaceViewer.Model.SubdivID = this.Model.SubdivID;
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                this.Model.WorkPlaceID = f.SelectedWorkPlaceID;
            }

        }

        private void DeleteWorkPlace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.WorkPlaceID != null;
        }

        private void DeleteWorkPlace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Model.WorkPlaceID = null;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.HasChanges;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exception ex;
            if ((ex = Model.Save()) != null)
                MessageBox.Show(ex.GetFormattedException(), "Ошибка сохранения данных");
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }

    public class StaffViewModel : EntityGenerator.Staff, IDataErrorInfo
    {
        OracleDataAdapter odaStaff, odaStaff_Addition, odaStaff_Period, odaStaff_Vac;
        DataSet ds;
        public StaffViewModel(decimal? staffId)
        {
            odaStaff = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectStaffData.sql"), Connect.CurConnect);
            odaStaff.SelectCommand.BindByName = true;
            odaStaff.SelectCommand.Parameters.Add("p_staff_id", OracleDbType.Decimal, staffId, ParameterDirection.Input);
            odaStaff.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaStaff.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
            odaStaff.SelectCommand.Parameters.Add("c3", OracleDbType.RefCursor, ParameterDirection.Output);
            odaStaff.SelectCommand.Parameters.Add("c4", OracleDbType.RefCursor, ParameterDirection.Output);
            odaStaff.SelectCommand.Parameters.Add("c5", OracleDbType.RefCursor, ParameterDirection.Output);

            odaStaff.TableMappings.Add("Table", "STAFF");
            odaStaff.TableMappings.Add("Table1", "STAFF_PERIOD");
            odaStaff.TableMappings.Add("Table2", "STAFF_ADDITION");
            odaStaff.TableMappings.Add("Table3", "STAFF_VAC");
            odaStaff.TableMappings.Add("Table4", "SUBDIV_PARTITION");
            ds = new DataSet();
            ds.Tables.Add(WpfControlLibrary.ManningDataSet.Tables["TYPE_STAFF_ADDITION"].Copy());
            ds.Tables.Add(WpfControlLibrary.ManningDataSet.Tables["VAC_GROUP_TYPE"].Copy());

            #region Адаптер штатных единиц
            odaStaff.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_UPDATE(p_STAFF_ID=>:p_STAFF_ID,p_POS_ID=>:p_POS_ID,p_SUBDIV_ID=>:p_SUBDIV_ID,p_DEGREE_ID=>:p_DEGREE_ID,p_COMMENTS=>:p_COMMENTS,p_ORDER_ID=>:p_ORDER_ID,p_TARIFF_GRID_ID=>:p_TARIFF_GRID_ID,p_TAR_BY_SCHEMA=>:p_TAR_BY_SCHEMA,p_CLASSIFIC=>:p_CLASSIFIC,p_STAFF_SECTION_ID=>:p_STAFF_SECTION_ID,p_SUBDIV_PARTITION_ID=>:p_SUBDIV_PARTITION_ID,p_FORM_OPERATION_ID=>:p_FORM_OPERATION_ID,p_POS_NOTE=>:p_POS_NOTE,p_STAFF_COUNT=>:p_STAFF_COUNT,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_GR_WORK_ID=>:p_GR_WORK_ID,p_WORKING_TIME_ID=>:p_WORKING_TIME_ID,p_SIGN_MAT_RESP_CONTR=>:p_SIGN_MAT_RESP_CONTR,p_TYPE_EDU_ID=>:p_TYPE_EDU_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff.InsertCommand.BindByName = true;
            odaStaff.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff.InsertCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.InputOutput;
            odaStaff.InsertCommand.Parameters["p_STAFF_ID"].DbType = DbType.Decimal;
            odaStaff.InsertCommand.Parameters.Add("p_POS_ID", OracleDbType.Decimal, 0, "POS_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_COMMENTS", OracleDbType.Varchar2, 0, "COMMENTS").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_ORDER_ID", OracleDbType.Decimal, 0, "ORDER_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_TARIFF_GRID_ID", OracleDbType.Decimal, 0, "TARIFF_GRID_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_TAR_BY_SCHEMA", OracleDbType.Decimal, 0, "TAR_BY_SCHEMA").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_CLASSIFIC", OracleDbType.Decimal, 0, "CLASSIFIC").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_STAFF_SECTION_ID", OracleDbType.Decimal, 0, "STAFF_SECTION_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_SUBDIV_PARTITION_ID", OracleDbType.Decimal, 0, "SUBDIV_PARTITION_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_FORM_OPERATION_ID", OracleDbType.Decimal, 0, "FORM_OPERATION_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_POS_NOTE", OracleDbType.Varchar2, 0, "POS_NOTE").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_STAFF_COUNT", OracleDbType.Decimal, 0, "STAFF_COUNT").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_WORKING_TIME_ID", OracleDbType.Decimal, 0, "WORKING_TIME_ID").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_SIGN_MAT_RESP_CONTR", OracleDbType.Decimal, 0, "SIGN_MAT_RESP_CONTR").Direction = ParameterDirection.Input;
            odaStaff.InsertCommand.Parameters.Add("p_TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID").Direction = ParameterDirection.Input;

            odaStaff.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_UPDATE(p_STAFF_ID=>:p_STAFF_ID,p_POS_ID=>:p_POS_ID,p_SUBDIV_ID=>:p_SUBDIV_ID,p_DEGREE_ID=>:p_DEGREE_ID,p_COMMENTS=>:p_COMMENTS,p_ORDER_ID=>:p_ORDER_ID,p_TARIFF_GRID_ID=>:p_TARIFF_GRID_ID,p_TAR_BY_SCHEMA=>:p_TAR_BY_SCHEMA,p_CLASSIFIC=>:p_CLASSIFIC,p_STAFF_SECTION_ID=>:p_STAFF_SECTION_ID,p_SUBDIV_PARTITION_ID=>:p_SUBDIV_PARTITION_ID,p_FORM_OPERATION_ID=>:p_FORM_OPERATION_ID,p_POS_NOTE=>:p_POS_NOTE,p_STAFF_COUNT=>:p_STAFF_COUNT,p_WORK_PLACE_ID=>:p_WORK_PLACE_ID,p_GR_WORK_ID=>:p_GR_WORK_ID,p_WORKING_TIME_ID=>:p_WORKING_TIME_ID,p_SIGN_MAT_RESP_CONTR=>:p_SIGN_MAT_RESP_CONTR,p_TYPE_EDU_ID=>:p_TYPE_EDU_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff.UpdateCommand.BindByName = true;
            odaStaff.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff.UpdateCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.InputOutput;
            odaStaff.UpdateCommand.Parameters["p_STAFF_ID"].DbType = DbType.Decimal;
            odaStaff.UpdateCommand.Parameters.Add("p_POS_ID", OracleDbType.Decimal, 0, "POS_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_COMMENTS", OracleDbType.Varchar2, 0, "COMMENTS").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_ORDER_ID", OracleDbType.Decimal, 0, "ORDER_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_TARIFF_GRID_ID", OracleDbType.Decimal, 0, "TARIFF_GRID_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_TAR_BY_SCHEMA", OracleDbType.Decimal, 0, "TAR_BY_SCHEMA").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_CLASSIFIC", OracleDbType.Decimal, 0, "CLASSIFIC").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_STAFF_SECTION_ID", OracleDbType.Decimal, 0, "STAFF_SECTION_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_SUBDIV_PARTITION_ID", OracleDbType.Decimal, 0, "SUBDIV_PARTITION_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_FORM_OPERATION_ID", OracleDbType.Decimal, 0, "FORM_OPERATION_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_POS_NOTE", OracleDbType.Varchar2, 0, "POS_NOTE").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_STAFF_COUNT", OracleDbType.Decimal, 0, "STAFF_COUNT").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, 0, "WORK_PLACE_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_WORKING_TIME_ID", OracleDbType.Decimal, 0, "WORKING_TIME_ID").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_SIGN_MAT_RESP_CONTR", OracleDbType.Decimal, 0, "SIGN_MAT_RESP_CONTR").Direction = ParameterDirection.Input;
            odaStaff.UpdateCommand.Parameters.Add("p_TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID").Direction = ParameterDirection.Input;

            odaStaff.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_DELETE(:p_STAFF_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff.DeleteCommand.BindByName = true;
            odaStaff.DeleteCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.InputOutput;

            odaStaff.AcceptChangesDuringUpdate = false;
            #endregion

            #region Адаптер надбавок штатных единиц
            odaStaff_Addition = new OracleDataAdapter();
            odaStaff_Addition.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_ADDITION_UPDATE(p_STAFF_ADDITION_ID=>:p_STAFF_ADDITION_ID,p_STAFF_ID=>:p_STAFF_ID,p_TYPE_STAFF_ADDITION_ID=>:p_TYPE_STAFF_ADDITION_ID,p_ADDITION_VALUE=>:p_ADDITION_VALUE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Addition.InsertCommand.BindByName = true;
            odaStaff_Addition.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Addition.InsertCommand.Parameters.Add("p_STAFF_ADDITION_ID", OracleDbType.Decimal, 0, "STAFF_ADDITION_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Addition.InsertCommand.Parameters["p_STAFF_ADDITION_ID"].DbType = DbType.Decimal;
            odaStaff_Addition.InsertCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaStaff_Addition.InsertCommand.Parameters.Add("p_TYPE_STAFF_ADDITION_ID", OracleDbType.Decimal, 0, "TYPE_STAFF_ADDITION_ID").Direction = ParameterDirection.Input;
            odaStaff_Addition.InsertCommand.Parameters.Add("p_ADDITION_VALUE", OracleDbType.Decimal, 0, "ADDITION_VALUE").Direction = ParameterDirection.Input;

            odaStaff_Addition.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_ADDITION_UPDATE(p_STAFF_ADDITION_ID=>:p_STAFF_ADDITION_ID,p_STAFF_ID=>:p_STAFF_ID,p_TYPE_STAFF_ADDITION_ID=>:p_TYPE_STAFF_ADDITION_ID,p_ADDITION_VALUE=>:p_ADDITION_VALUE);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Addition.UpdateCommand.BindByName = true;
            odaStaff_Addition.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Addition.UpdateCommand.Parameters.Add("p_STAFF_ADDITION_ID", OracleDbType.Decimal, 0, "STAFF_ADDITION_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Addition.UpdateCommand.Parameters["p_STAFF_ADDITION_ID"].DbType = DbType.Decimal;
            odaStaff_Addition.UpdateCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaStaff_Addition.UpdateCommand.Parameters.Add("p_TYPE_STAFF_ADDITION_ID", OracleDbType.Decimal, 0, "TYPE_STAFF_ADDITION_ID").Direction = ParameterDirection.Input;
            odaStaff_Addition.UpdateCommand.Parameters.Add("p_ADDITION_VALUE", OracleDbType.Decimal, 0, "ADDITION_VALUE").Direction = ParameterDirection.Input;

            odaStaff_Addition.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_ADDITION_DELETE(:p_STAFF_ADDITION_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Addition.DeleteCommand.BindByName = true;
            odaStaff_Addition.DeleteCommand.Parameters.Add("p_STAFF_ADDITION_ID", OracleDbType.Decimal, 0, "STAFF_ADDITION_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Addition.AcceptChangesDuringUpdate = false;

            #endregion

            #region Адаптер периодов действия штатных единиц
            odaStaff_Period = new OracleDataAdapter();
            odaStaff_Period.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_PERIOD_UPDATE(p_STAFF_PERIOD_ID=>:p_STAFF_PERIOD_ID,p_DATE_STAFF_BEGIN=>:p_DATE_STAFF_BEGIN,p_DATE_STAFF_END=>:p_DATE_STAFF_END,p_STAFF_ID=>:p_STAFF_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Period.InsertCommand.BindByName = true;
            odaStaff_Period.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Period.InsertCommand.Parameters.Add("p_STAFF_PERIOD_ID", OracleDbType.Decimal, 0, "STAFF_PERIOD_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Period.InsertCommand.Parameters["p_STAFF_PERIOD_ID"].DbType = DbType.Decimal;
            odaStaff_Period.InsertCommand.Parameters.Add("p_DATE_STAFF_BEGIN", OracleDbType.Date, 0, "DATE_STAFF_BEGIN").Direction = ParameterDirection.Input;
            odaStaff_Period.InsertCommand.Parameters.Add("p_DATE_STAFF_END", OracleDbType.Date, 0, "DATE_STAFF_END").Direction = ParameterDirection.Input;
            odaStaff_Period.InsertCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;

            odaStaff_Period.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_PERIOD_UPDATE(p_STAFF_PERIOD_ID=>:p_STAFF_PERIOD_ID,p_DATE_STAFF_BEGIN=>:p_DATE_STAFF_BEGIN,p_DATE_STAFF_END=>:p_DATE_STAFF_END,p_STAFF_ID=>:p_STAFF_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Period.UpdateCommand.BindByName = true;
            odaStaff_Period.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Period.UpdateCommand.Parameters.Add("p_STAFF_PERIOD_ID", OracleDbType.Decimal, 0, "STAFF_PERIOD_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Period.UpdateCommand.Parameters["p_STAFF_PERIOD_ID"].DbType = DbType.Decimal;
            odaStaff_Period.UpdateCommand.Parameters.Add("p_DATE_STAFF_BEGIN", OracleDbType.Date, 0, "DATE_STAFF_BEGIN").Direction = ParameterDirection.Input;
            odaStaff_Period.UpdateCommand.Parameters.Add("p_DATE_STAFF_END", OracleDbType.Date, 0, "DATE_STAFF_END").Direction = ParameterDirection.Input;
            odaStaff_Period.UpdateCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;

            odaStaff_Period.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_PERIOD_DELETE(:p_STAFF_PERIOD_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Period.DeleteCommand.BindByName = true;
            odaStaff_Period.DeleteCommand.Parameters.Add("p_STAFF_PERIOD_ID", OracleDbType.Decimal, 0, "STAFF_PERIOD_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Period.AcceptChangesDuringUpdate = false;
            #endregion

            #region Адаптер доп. отпусков штатных единиц
            odaStaff_Vac = new OracleDataAdapter();
            odaStaff_Vac.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_VAC_UPDATE(p_STAFF_VAC_ID=>:p_STAFF_VAC_ID,p_STAFF_ID=>:p_STAFF_ID,p_VAC_GROUP_TYPE_ID=>:p_VAC_GROUP_TYPE_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Vac.InsertCommand.BindByName = true;
            odaStaff_Vac.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Vac.InsertCommand.Parameters.Add("p_STAFF_VAC_ID", OracleDbType.Decimal, 0, "STAFF_VAC_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Vac.InsertCommand.Parameters["p_STAFF_VAC_ID"].DbType = DbType.Decimal;
            odaStaff_Vac.InsertCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaStaff_Vac.InsertCommand.Parameters.Add("p_VAC_GROUP_TYPE_ID", OracleDbType.Decimal, 0, "VAC_GROUP_TYPE_ID").Direction = ParameterDirection.Input;

            odaStaff_Vac.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_VAC_UPDATE(p_STAFF_VAC_ID=>:p_STAFF_VAC_ID,p_STAFF_ID=>:p_STAFF_ID,p_VAC_GROUP_TYPE_ID=>:p_VAC_GROUP_TYPE_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Vac.UpdateCommand.BindByName = true;
            odaStaff_Vac.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaStaff_Vac.UpdateCommand.Parameters.Add("p_STAFF_VAC_ID", OracleDbType.Decimal, 0, "STAFF_VAC_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Vac.UpdateCommand.Parameters["p_STAFF_VAC_ID"].DbType = DbType.Decimal;
            odaStaff_Vac.UpdateCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaStaff_Vac.UpdateCommand.Parameters.Add("p_VAC_GROUP_TYPE_ID", OracleDbType.Decimal, 0, "VAC_GROUP_TYPE_ID").Direction = ParameterDirection.Input;

            odaStaff_Vac.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.STAFF_VAC_DELETE(:p_STAFF_VAC_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaStaff_Vac.DeleteCommand.BindByName = true;
            odaStaff_Vac.DeleteCommand.Parameters.Add("p_STAFF_VAC_ID", OracleDbType.Decimal, 0, "STAFF_VAC_ID").Direction = ParameterDirection.InputOutput;
            odaStaff_Vac.AcceptChangesDuringUpdate = false;
            #endregion

            odaStaff.Fill(ds);
            if (ds.Tables["STAFF"].Rows.Count == 0)
            {
                ds.Tables["STAFF"].Rows.Add();
                IsAdding = true;
            }
            else
            {
                IsAdding = false;
            }
            DataRow = ds.Tables["STAFF"].Rows[0];

            SetBaseTariff(DateTime.Today);
        }

        public bool IsAdding { get; set; } = false;

        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <returns></returns>
        public new Exception Save()
        {
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                if (IsAdding)
                    this.StaffID = null;
                odaStaff.Update(new DataRow[] { this.DataRow });

                foreach (var p in StaffPeriodSource)
                {
                    if (p.EntityState == DataRowState.Added)
                        p.StaffPeriodID = null;
                    p.StaffID = StaffID;
                }
                odaStaff_Period.Update(ds.Tables["STAFF_PERIOD"]);


                foreach (var p in StaffAdditionList)
                {
                    if (p.EntityState == DataRowState.Added)
                        p.StaffAdditionID = null;
                    p.StaffID = StaffID;
                }
                odaStaff_Addition.Update(ds.Tables["STAFF_ADDITION"]);

                foreach (var p in StaffVacList)
                {
                    if (p.EntityState == DataRowState.Added)
                        p.StaffVacID = null;
                    p.StaffID = StaffID;
                }
                odaStaff_Vac.Update(ds.Tables["STAFF_VAC"]);

                ds.AcceptChanges();
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }

        #region Свойства для работы

        public new string this[string column_name]
        {
            get
            {
                if (IsProperty(column_name, () => StaffID))
                    return string.Empty;
                string s = base[column_name];
                if (s != null)
                    return s;
                return string.Empty;
            }
        }

        /// <summary>
        /// Ошибка всей модели
        /// </summary>
        public new string Error
        {
            get
            {
                if (StaffPeriodSource.Count == 0)
                    return "Не указан период действия штатной единицы";
                var items = StaffPeriodSource.OrderBy(r => new Tuple<DateTime?, DateTime?>(r.DateStaffBegin, r.DateStaffEnd)).ToList();
                for (int i = 1; i < items.Count; ++i)
                {
                    if (items[i].DateStaffBegin == null)
                        return "Дата начала действия штатной единицы - обязательный реквизит";
                    if ((items[i - 1].DateStaffEnd??new DateTime(3000, 1, 1)) > (items[i].DateStaffBegin??new DateTime(1900,1,1)))
                        return "Периоды действия штатной единицы не должны пересекаться";
                }
                if (SubdivID == null)
                    return "Не указано подразделение";
                if (DegreeID == null)
                    return "Не указана категория";
                if (PosID == null)
                    return "Не указана должность";
                if (FormOperationID == null)
                    return "Не указан вид производства";
                if (TarBySchema == null)
                    return "Не указан тарифный коэффициент";
                foreach (var p in StaffAdditionSource)
                    if (!string.IsNullOrEmpty(p.Error))
                        return p.Error;
                return "";
            }
        }

        WorkPlaceModel _workPlace;

        /// <summary>
        /// Модель для рабочего места, заполнение авто
        /// </summary>
        public WorkPlaceModel WorkPlace
        {
            get
            {
                if (_workPlace == null)
                    RefreshWorkPlace();
                return _workPlace;
            }
        }

        private void RefreshWorkPlace()
        {
            _workPlace = new WorkPlaceModel(this.WorkPlaceID) { IsReadOnly = true };
        }

        /// <summary>
        /// Айдишник ссылка на рабочее место
        /// </summary>
        public new decimal? WorkPlaceID
        {
            get
            {
                return base.WorkPlaceID;
            }
            set
            {
                if (base.WorkPlaceID != value)
                {
                    base.WorkPlaceID = value;
                    _workPlace = null;
                    RaisePropertyChanged(() => WorkPlace);
                }
            }
        }

        /// <summary>
        /// Адишник графика работы
        /// </summary>
        public new decimal? GrWorkID
        {
            get
            {
                return base.GrWorkID;
            }
            set
            {
                if (base.GrWorkID != value)
                {
                    base.GrWorkID = value;
                    _grWork = null;
                    RaisePropertyChanged(()=>GrWork);
                }
            }
        }

        /// <summary>
        /// Перегруженный разряд работ
        /// </summary>
        public new decimal? Classific
        {
            get
            {
                return base.Classific;
            }
            set
            {
                if (base.Classific != value)
                {
                    base.Classific = value;
                    TarBySchema = GetTarFromTarifGrid(StaffPeriodSource.Select(r => r.DateStaffEnd).LastOrDefault() ?? DateTime.Today);
                    RaisePropertyChanged(() => TarSchemaEditable);
                }
            }
        }

        /// <summary>
        /// Адишник тарифной сетки - перегруженная для событий
        /// </summary>
        public new decimal? TariffGridID
        {
            get
            {
                return base.TariffGridID;
            }
            set
            {
                if (base.TariffGridID != value)
                {
                    base.TariffGridID = value;
                    RaisePropertyChanged(() => TariffGrid);
                    TarBySchema = GetTarFromTarifGrid(StaffPeriodSource.Select(r => r.DateStaffEnd).LastOrDefault() ?? DateTime.Today);
                    RaisePropertyChanged(() => TarSchemaEditable);
                }
            }
        }

        /// <summary>
        /// Выбранная тарифная сетка
        /// </summary>
        public TariffGrid TariffGrid
        {
            get
            {
                return TariffGridSource.Where(r => r.TariffGridID == this.TariffGridID).FirstOrDefault();
            }
        }

        GrWorkModel _grWork;
        /// <summary>
        /// График работы штатной ед
        /// </summary>
        public GrWorkModel GrWork
        {
            get
            {
                if (_grWork == null)
                    _grWork = new GrWorkModel(this.GrWorkID);
                return _grWork;
            }
        }

        /// <summary>
        /// Код выбранного подразделения
        /// 
        /// </summary>
        public string CodeSubdiv
        {
            get
            {
                return WpfControlLibrary.AppDataSet.Tables["SUBDIV"].Rows.Cast<DataRow>().Where(r => r.Field2<Decimal?>("SUBDIV_ID") == this.SubdivID).Select(r => r.Field2<string>("CODE_SUBDIV")).FirstOrDefault();
            }
        }

        decimal? _baseTariff;
        /// <summary>
        /// Тарифная база коэффициента
        /// </summary>
        public decimal? TariffBase
        {
            get
            {
                return _baseTariff;
            }
            set
            {
                _baseTariff = value;
                RaisePropertyChanged(() => TariffBase);
                RaisePropertyChanged(() => TarByMonth);
                SetNewBase();
                RaisePropertyChanged(() => SumSalMonth);
                RaisePropertyChanged(() => WaistAddMonth);
                RaisePropertyChanged(() => ExpAddMonth);
            }
        }

        private void SetNewBase()
        {
            foreach (var p in StaffAdditionSource)
            {
                p.BaseValue = TariffBase;
                p.BaseMonthValue = TarByMonth;
            }
            RaisePropertyChanged(()=>ExpAddMonth);
            RaisePropertyChanged(()=>WaistAddMonth);
            RaisePropertyChanged(() => SumSalMonth);
        }

        private void SetBaseTariff(DateTime? dt)
        {
            if (dt == null)
                TariffBase = null;
            else
                TariffBase = BaseTariffSource.Where(r => dt <= r.DateEnd && dt >= r.DateBegin).Select(r => r.TariffValue).FirstOrDefault();
        }

        /// <summary>
        /// Перегружаем коэффициент чтбы работать с рублями
        /// </summary>
        public new decimal? TarBySchema
        {
            get
            {
                return base.TarBySchema;
            }
            set
            {
                base.TarBySchema = value;
                RaisePropertyChanged(() => TarByMonth);
                SetNewBase();
            }
        }

        private decimal? GetTarFromTarifGrid(DateTime dt)
        {
            return WpfControlLibrary.Tariff_Grid_Salary.TariffGridSalaryList
                .Where(r => r.TarClassif == Classific.ToString() && r.TariffGridID == TariffGridID && dt >= r.TarDate && dt <= r.TariffEndDate).Select(r => r.TarSal).FirstOrDefault();
        }

        /// <summary>
        /// Доступно ли редактирование коэффициента
        /// </summary>
        public bool TarSchemaEditable
        {
            get
            {
                return TariffGridID == null || Classific==0 && TariffGrid.TypeTariffGridID == 2;
            }
        }

        /// <summary>
        /// Оклад за месяц
        /// </summary>
        public decimal? TarByMonth
        {
            get
            {
                if (TarBySchema == null || TariffBase==null) return null;
                if (TariffGridID == null || Classific == null)
                    return Math.Round(TarBySchema.Value * TariffBase.Value, 0, MidpointRounding.AwayFromZero);
                else
                    return Math.Round(TarBySchema.Value * TariffBase.Value, 2, MidpointRounding.AwayFromZero);
            }
        }

        public bool HasChanges
        {
            get
            {
                return ds != null && ds.HasChanges();
            }
        }
        #endregion

        #region Псевдостолбцы
        /// <summary>
        /// Итого за месяц
        /// </summary>
        public decimal? SumSalMonth
        {
            get
            {
                return TarByMonth + StaffAdditionSource.Sum(r => r.AdditionValueMonth);
            }
        }

        public decimal? WaistAddition
        {
            get
            {
                return 0.4m;
            }
        }
        public decimal? WaistAddMonth
        {
            get
            {
                return WaistAddition * SumSalMonth;
            }
        }

        public decimal? ExpAddition
        {
            get
            {
                return 0.3m;
            }
        }

        public decimal? ExpAddMonth
        {
            get
            {
                return ExpAddition * SumSalMonth;
            }
        }
        #endregion


        #region Зависимые таблицы данных

        EntityRelationList<StaffPeriod> _staffPeriodSource;
        List<StaffAdditionTypeViewModel> _staffAdditionSource;
        List<StaffVacViewModel> _staffVacSource;
        /// <summary>
        /// Источник данных - список периодов существования штатной единицы
        /// </summary>
        public EntityRelationList<StaffPeriod> StaffPeriodSource
        {
            get
            {
                if (_staffPeriodSource == null)
                {
                    _staffPeriodSource = new EntityRelationList<StaffPeriod>(ds.Tables["STAFF_PERIOD"].ConvertToEntityList<StaffPeriod>()) { RelatedEntity = this};
                    foreach (var p in _staffPeriodSource)
                        p.PropertyChanged += OnPeriodChanged;
                    _staffPeriodSource.ListChanged += _staffPeriodSource_ListChanged;
                }
                return _staffPeriodSource;
            }
        }

        private void _staffPeriodSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded && e.NewIndex>-1 && _staffPeriodSource[e.NewIndex]!=null)
            {
                _staffPeriodSource[e.NewIndex].PropertyChanged += OnPeriodChanged;
                RaisePropertyChanged(() => Error);
            }
        }

        private void OnPeriodChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DateStaffBegin" || e.PropertyName == "DateStaffEnd")
                RaisePropertyChanged(() => Error);
        }

        /// <summary>
        /// Коллекция для просмотра и отображения надбавок для штатной единицы
        /// </summary>
        public List<StaffAdditionTypeViewModel> StaffAdditionSource
        {
            get
            {
                if (_staffAdditionSource == null)
                {
                    _staffAdditionSource = ds.Tables["TYPE_STAFF_ADDITION"].ConvertToEntityList<StaffAdditionTypeViewModel>();
                    foreach (var p in _staffAdditionSource)
                        p.PropertyChanged += (pe, pw) =>
                          {
                              if (pw.PropertyName == "AdditionValue")
                              {
                                  RaisePropertyChanged(() => ExpAddMonth);
                                  RaisePropertyChanged(() => WaistAddMonth);
                                  RaisePropertyChanged(() => SumSalMonth);
                                  RaisePropertyChanged(() => Error);
                              }
                          };
                }
                return _staffAdditionSource;
            }
        }

        /// <summary>
        /// Список надбавок штатки
        /// </summary>
        public List<StaffAddition> StaffAdditionList
        {
            get
            {
                return ds.Tables["STAFF_ADDITION"].ConvertToEntityList<StaffAddition>();
            }
        }

        /// <summary>
        /// Список отпусков для просмотра дополнительных шт. единицы
        /// </summary>
        public List<StaffVacViewModel> StaffVacSource
        {
            get
            {
                if (_staffVacSource == null)
                    _staffVacSource = ds.Tables["VAC_GROUP_TYPE"].ConvertToEntityList<StaffVacViewModel>()
                        .Where(r=>r.TypeVacCalcPeriodID==2 && r.VacGroupTypeID!=5 && r.VacGroupTypeID!=6).OrderBy(r=>r.GroupVacFullName).ToList();
                return _staffVacSource;
            }
        }

        /// <summary>
        /// Доп отпуска к штатке - сами данные
        /// </summary>
        public List<StaffVac> StaffVacList
        {
            get
            {
                return ds.Tables["STAFF_VAC"].ConvertToEntityList<StaffVac>();
            }
        }


        #endregion

        #region Истоники данных
        List<SubdivPartition> _subdivPartitionSource;
        /// <summary>
        /// Источник данных список подструктур подразделений
        /// </summary>
        public List<SubdivPartition> SubdivPartitionSource
        {
            get
            {
                if (_subdivPartitionSource==null)
                    _subdivPartitionSource =  ds.Tables["SUBDIV_PARTITION"].ConvertToEntityList<SubdivPartition>();
                return _subdivPartitionSource;
            }
        }

        List<Position> _positionSource;
        /// <summary>
        /// Источник данны список должностей
        /// </summary>
        public List<Position> PositionSource
        {
            get
            {
                if (_positionSource ==null)
                    _positionSource = WpfControlLibrary.AppDataSet.Tables["POSITION"].ConvertToEntityList<Position>();
                return _positionSource;
            }
        }

        /// <summary>
        /// Список баз коэффициентов
        /// </summary>
        public List<BaseTariff> BaseTariffSource
        {
            get
            {
                return WpfControlLibrary.ManningDataSet.Tables["BASE_TARIFF"].ConvertToEntityList<BaseTariff>();
            }
        }

        Dictionary<decimal?, string> _orderSource;
        /// <summary>
        /// Источник данных  - список заказов
        /// </summary>
        public Dictionary<decimal?, string> OrderSource
        {
            get
            {
                if (_orderSource == null)
                    _orderSource = WpfControlLibrary.ManningDataSet.Tables["ORDERS"].Rows.Cast<DataRow>().ToDictionary(r => r.Field2<decimal?>("ORDER_ID"), r => r.Field2<string>("ORDER_NAME"));
                return _orderSource;
            }
        }

        List<Degree> _degreeSource;
        /// <summary>
        /// Список категорий
        /// </summary>
        public List<Degree> DegreeSource
        {
            get
            {
                if (_degreeSource == null)
                    _degreeSource = WpfControlLibrary.AppDataSet.Tables["DEGREE"].ConvertToEntityList<Degree>();
                return _degreeSource;
            }
        }

        List<FormOperation> _formOperationSource;
        /// <summary>
        /// Список категорий
        /// </summary>
        public List<FormOperation> FormOperationSource
        {
            get
            {
                if (_formOperationSource == null)
                    _formOperationSource = WpfControlLibrary.AppDataSet.Tables["FORM_OPERATION"].ConvertToEntityList<FormOperation>();
                return _formOperationSource;
            }
        }

        List<WorkingTime> _workingTimeSource;

        /// <summary>
        /// Список условий графика работы
        /// </summary>
        public List<WorkingTime> WorkingTimeSource
        {
            get
            {
                if (_workingTimeSource == null)
                    _workingTimeSource = WpfControlLibrary.ManningDataSet.Tables["WORKING_TIME"].ConvertToEntityList<WorkingTime>();
                return _workingTimeSource;
            }
        }

        List<TypeEdu> _typeEduSource;
        /// <summary>
        /// Тип образования сотрудника
        /// </summary>
        public List<TypeEdu> TypeEduSource
        {
            get
            {
                if (_typeEduSource==null)
                    _typeEduSource = WpfControlLibrary.AppDataSet.Tables["TYPE_EDU"].ConvertToEntityList<TypeEdu>();
                return _typeEduSource;
            }
        }

        List<TariffGrid> _tariffGridSource;
        /// <summary>
        /// Список тарифных сеток
        /// </summary>
        public List<TariffGrid> TariffGridSource
        {
            get
            {
                if (_tariffGridSource==null)
                    _tariffGridSource = WpfControlLibrary.AppDataSet.Tables["TARIFF_GRID"].ConvertToEntityList<TariffGrid>();
                return _tariffGridSource;
            }
        }


        #endregion

        private bool IsProperty<T>(string columnName, Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return columnName==propertyName;
        }
    }



    public class StaffAdditionTypeViewModel : TypeStaffAddition, IDataErrorInfo
    {
        /// <summary>
        /// Значение для надбавки
        /// </summary>
        public decimal? AdditionValue
        {
            get
            {
                return currentValue;
            }
            set
            {
                if (currentValue != value)
                {
                    if (value == null)
                        DeleteCond();
                    if (value != null)
                    {
                        if (currentValue == null)
                            InsertCond(value.Value);
                        else
                            UpdateCond(value.Value);
                    }
                    RaisePropertyChanged(() => AdditionValue);
                    RaisePropertyChanged(() => AdditionValueMonth);
                }
            }
        }

        #region remove, delete, add condition helper
        private decimal? currentValue
        {
            get
            {
                return DataSet.Tables["STAFF_ADDITION"].DefaultView.OfType<DataRowView>()
                    .Where(r => r.Row.Field2<Decimal?>("TYPE_STAFF_ADDITION_ID") == this.TypeStaffAdditionID).Select(r => r.Row.Field2<decimal?>("ADDITION_VALUE")).FirstOrDefault();
            }
        }

        private void DeleteCond()
        {
            foreach (var row in DataSet.Tables["STAFF_ADDITION"].DefaultView.OfType<DataRowView>().Where(r => r.Row.Field2<Decimal?>("TYPE_STAFF_ADDITION_ID") == this.TypeStaffAdditionID))
            {
                row.Delete();
            }
        }

        private void InsertCond(decimal value)
        {
            DataRow r = DataSet.Tables["STAFF_ADDITION"].Rows.Add();
            r["TYPE_STAFF_ADDITION_ID"] = this.TypeStaffAdditionID;
            r["ADDITION_VALUE"] = value;
        }

        private void UpdateCond(decimal value)
        {
            DataRowView rview =
            DataSet.Tables["STAFF_ADDITION"].DefaultView.OfType<DataRowView>()
                    .Where(r => r.Row.Field2<Decimal?>("TYPE_STAFF_ADDITION_ID") == this.TypeStaffAdditionID).FirstOrDefault();
            if (rview != null)
                rview["ADDITION_VALUE"] = value;
        }
        #endregion

        decimal? _baseTariff=1000;
        public decimal? BaseValue
        {
            get
            {
                return _baseTariff;
            }
            set
            {
                _baseTariff = value;
                RaisePropertyChanged(() => BaseValue);
                RaisePropertyChanged(() => AdditionValueMonth);
            }
        }

        decimal? _coefficient = 2.22m;
        public decimal? BaseMonthValue
        {
            get
            {
                return _coefficient;
            }
            set
            {
                _coefficient = value;
                RaisePropertyChanged(() => BaseMonthValue);
                if (this.TypeAddMeasureID == 1)
                    RaisePropertyChanged(() => AdditionValueMonth);
            }
        }

        /// <summary>
        /// Значение надбавки за месяц в рублях
        /// </summary>
        public decimal? AdditionValueMonth
        {
            get
            {
                if (BaseValue == null || AdditionValue == null || BaseMonthValue==null) return null;
                if (TypeAddMeasureID == 1) // если надбавка это коэффициент то считаем от тар. коэффициента
                    return Math.Round(BaseValue.Value * AdditionValue.Value, 2, MidpointRounding.AwayFromZero);
                else
                    return Math.Round(BaseMonthValue.Value * AdditionValue.Value /100, 2, MidpointRounding.AwayFromZero);
            }
        }

        public new string this[string column_name]
        {
            get
            {
                if (column_name == "AdditionValue"  && (AdditionValue < MinValue || AdditionValue > MaxValue))
                    return $"Значение не попадает в диапазон разрешнных ({MinValue}-{MaxValue})";
                return string.Empty;
            }
        }

        public new  string Error
        {
            get
            {
                return AdditionValue < MinValue || AdditionValue > MaxValue ?
                    $"Значение не попадает в диапазон разрешнных ({MinValue}-{MaxValue})" : "";
            }
        }
    }
    public class StaffVacViewModel : VacGroupType
    {
        /// <summary>
        /// Значение для надбавки
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return currentChecked;
            }
            set
            {
                if (currentChecked != value)
                {
                    if (value != true)
                        DeleteCond();
                    else
                    {
                        if (!currentChecked)
                            InsertCond();
                    }
                    RaisePropertyChanged(() => IsChecked);
                }
            }
        }

        #region remove, delete, add condition helper
        private bool currentChecked
        {
            get
            {
                return DataSet.Tables["STAFF_VAC"].DefaultView.OfType<DataRowView>()
                    .Any(r => r.Row.Field2<Decimal?>("VAC_GROUP_TYPE_ID") == this.VacGroupTypeID);
            }
        }

        private void DeleteCond()
        {
            foreach (var row in DataSet.Tables["STAFF_VAC"].DefaultView.OfType<DataRowView>().Where(r => r.Row.Field2<Decimal?>("VAC_GROUP_TYPE_ID") == this.VacGroupTypeID))
            {
                row.Delete();
            }
        }

        private void InsertCond()
        {
            DataRow r = DataSet.Tables["STAFF_VAC"].Rows.Add();
            r["VAC_GROUP_TYPE_ID"] = this.VacGroupTypeID;
        }
        #endregion
    }


    public class GrWorkModel : GrWork
    {
        OracleDataAdapter odaGr_Work;
        DataSet ds;
        public GrWorkModel(decimal? grWorkID)
        {
            ds = new DataSet();
            odaGr_Work = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT\SelectGrWorkData.sql"), Connect.CurConnect);
            odaGr_Work.SelectCommand.BindByName = true;
            odaGr_Work.SelectCommand.Parameters.Add("p_gr_work_id", OracleDbType.Decimal, grWorkID, ParameterDirection.Input);
            odaGr_Work.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaGr_Work.TableMappings.Add("Table", "GR_WORK");

            odaGr_Work.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.GR_WORK_UPDATE(p_GR_WORK_ID=>:p_GR_WORK_ID,p_GR_WORK_NAME=>:p_GR_WORK_NAME,p_COUNT_DAY=>:p_COUNT_DAY,p_SIGN_HOLIDAY_WORK=>:p_SIGN_HOLIDAY_WORK,p_SIGN_COMPACT_DAY_WORK=>:p_SIGN_COMPACT_DAY_WORK,p_SIGN_FLOATING=>:p_SIGN_FLOATING,p_COMPACT_TIME_ZONE_ID=>:p_COMPACT_TIME_ZONE_ID,p_HOLIDAY_TIME_ZONE_ID=>:p_HOLIDAY_TIME_ZONE_ID,p_HOURS_FOR_NORM=>:p_HOURS_FOR_NORM,p_HOURS_DINNER=>:p_HOURS_DINNER,p_SIGN_SUMMARIZE=>:p_SIGN_SUMMARIZE,p_HOURS_FOR_GRAPH=>:p_HOURS_FOR_GRAPH,p_DATE_END_GRAPH=>:p_DATE_END_GRAPH,p_SIGN_SHORTEN=>:p_SIGN_SHORTEN,p_HOURS_NORM_CALENDAR=>:p_HOURS_NORM_CALENDAR,p_SIGN_SHIFTMAN=>:p_SIGN_SHIFTMAN);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaGr_Work.InsertCommand.BindByName = true;
            odaGr_Work.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaGr_Work.InsertCommand.Parameters.Add("p_GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID").Direction = ParameterDirection.InputOutput;
            odaGr_Work.InsertCommand.Parameters["p_GR_WORK_ID"].DbType = DbType.Decimal;
            odaGr_Work.InsertCommand.Parameters.Add("p_GR_WORK_NAME", OracleDbType.Varchar2, 0, "GR_WORK_NAME").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_COUNT_DAY", OracleDbType.Decimal, 0, "COUNT_DAY").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_HOLIDAY_WORK", OracleDbType.Decimal, 0, "SIGN_HOLIDAY_WORK").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_COMPACT_DAY_WORK", OracleDbType.Decimal, 0, "SIGN_COMPACT_DAY_WORK").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_FLOATING", OracleDbType.Decimal, 0, "SIGN_FLOATING").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_COMPACT_TIME_ZONE_ID", OracleDbType.Decimal, 0, "COMPACT_TIME_ZONE_ID").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_HOLIDAY_TIME_ZONE_ID", OracleDbType.Decimal, 0, "HOLIDAY_TIME_ZONE_ID").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_HOURS_FOR_NORM", OracleDbType.Decimal, 0, "HOURS_FOR_NORM").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_HOURS_DINNER", OracleDbType.Decimal, 0, "HOURS_DINNER").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_SUMMARIZE", OracleDbType.Decimal, 0, "SIGN_SUMMARIZE").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_HOURS_FOR_GRAPH", OracleDbType.Decimal, 0, "HOURS_FOR_GRAPH").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_DATE_END_GRAPH", OracleDbType.Date, 0, "DATE_END_GRAPH").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_SHORTEN", OracleDbType.Decimal, 0, "SIGN_SHORTEN").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_HOURS_NORM_CALENDAR", OracleDbType.Decimal, 0, "HOURS_NORM_CALENDAR").Direction = ParameterDirection.Input;
            odaGr_Work.InsertCommand.Parameters.Add("p_SIGN_SHIFTMAN", OracleDbType.Decimal, 0, "SIGN_SHIFTMAN").Direction = ParameterDirection.Input;

            odaGr_Work.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.GR_WORK_UPDATE(p_GR_WORK_ID=>:p_GR_WORK_ID,p_GR_WORK_NAME=>:p_GR_WORK_NAME,p_COUNT_DAY=>:p_COUNT_DAY,p_SIGN_HOLIDAY_WORK=>:p_SIGN_HOLIDAY_WORK,p_SIGN_COMPACT_DAY_WORK=>:p_SIGN_COMPACT_DAY_WORK,p_SIGN_FLOATING=>:p_SIGN_FLOATING,p_COMPACT_TIME_ZONE_ID=>:p_COMPACT_TIME_ZONE_ID,p_HOLIDAY_TIME_ZONE_ID=>:p_HOLIDAY_TIME_ZONE_ID,p_HOURS_FOR_NORM=>:p_HOURS_FOR_NORM,p_HOURS_DINNER=>:p_HOURS_DINNER,p_SIGN_SUMMARIZE=>:p_SIGN_SUMMARIZE,p_HOURS_FOR_GRAPH=>:p_HOURS_FOR_GRAPH,p_DATE_END_GRAPH=>:p_DATE_END_GRAPH,p_SIGN_SHORTEN=>:p_SIGN_SHORTEN,p_HOURS_NORM_CALENDAR=>:p_HOURS_NORM_CALENDAR,p_SIGN_SHIFTMAN=>:p_SIGN_SHIFTMAN);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaGr_Work.UpdateCommand.BindByName = true;
            odaGr_Work.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaGr_Work.UpdateCommand.Parameters.Add("p_GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID").Direction = ParameterDirection.InputOutput;
            odaGr_Work.UpdateCommand.Parameters["p_GR_WORK_ID"].DbType = DbType.Decimal;
            odaGr_Work.UpdateCommand.Parameters.Add("p_GR_WORK_NAME", OracleDbType.Varchar2, 0, "GR_WORK_NAME").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_COUNT_DAY", OracleDbType.Decimal, 0, "COUNT_DAY").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_HOLIDAY_WORK", OracleDbType.Decimal, 0, "SIGN_HOLIDAY_WORK").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_COMPACT_DAY_WORK", OracleDbType.Decimal, 0, "SIGN_COMPACT_DAY_WORK").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_FLOATING", OracleDbType.Decimal, 0, "SIGN_FLOATING").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_COMPACT_TIME_ZONE_ID", OracleDbType.Decimal, 0, "COMPACT_TIME_ZONE_ID").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_HOLIDAY_TIME_ZONE_ID", OracleDbType.Decimal, 0, "HOLIDAY_TIME_ZONE_ID").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_HOURS_FOR_NORM", OracleDbType.Decimal, 0, "HOURS_FOR_NORM").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_HOURS_DINNER", OracleDbType.Decimal, 0, "HOURS_DINNER").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_SUMMARIZE", OracleDbType.Decimal, 0, "SIGN_SUMMARIZE").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_HOURS_FOR_GRAPH", OracleDbType.Decimal, 0, "HOURS_FOR_GRAPH").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_DATE_END_GRAPH", OracleDbType.Date, 0, "DATE_END_GRAPH").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_SHORTEN", OracleDbType.Decimal, 0, "SIGN_SHORTEN").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_HOURS_NORM_CALENDAR", OracleDbType.Decimal, 0, "HOURS_NORM_CALENDAR").Direction = ParameterDirection.Input;
            odaGr_Work.UpdateCommand.Parameters.Add("p_SIGN_SHIFTMAN", OracleDbType.Decimal, 0, "SIGN_SHIFTMAN").Direction = ParameterDirection.Input;

            odaGr_Work.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.GR_WORK_DELETE(:p_GR_WORK_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaGr_Work.DeleteCommand.BindByName = true;
            odaGr_Work.DeleteCommand.Parameters.Add("p_GR_WORK_ID", OracleDbType.Decimal, 0, "GR_WORK_ID").Direction = ParameterDirection.InputOutput;

            odaGr_Work.Fill(ds);
            if (ds.Tables["GR_WORK"].Rows.Count == 0)
            {
                ds.Tables["GR_WORK"].Rows.Add();
            }
            DataRow = ds.Tables["GR_WORK"].Rows[0];
        }
    }

    public class TestAddition
    {
        public IEnumerable StaffAdditionSource
        {
            get
            {
                return new int[4] { 1, 2, 3, 4 }.Select(r => new TestAdd() { ShortNameStaffAdd = "Надб бла бла №" + r.ToString(),  NameStaffAdd = "Надбавка бла бла №" + r.ToString(), AdditionValue = r / 2 });
            }
        }
        public class TestAdd
        {
            public string NameStaffAdd
            {
                get;
                set;
            }
            public decimal? AdditionValue
            {
                get;set;
            }

            public string ShortNameStaffAdd
            {
                get;set;
            }
        }
    }

}
