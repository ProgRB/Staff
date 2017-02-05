using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Staff;
using LibraryKadr;
using System.Data;
using Kadr;
using System.Windows.Forms;
using System.Drawing;
using Oracle.DataAccess.Client;
using System.ComponentModel;
using System.IO;
using Oracle.DataAccess.Types;
using Kadr.Vacation_schedule;
using System.Data.Odbc;
using WpfControlLibrary.Вase_Exchange;
using WpfControlLibrary.Classes;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Resume_Editor.xaml
    /// </summary>
    public partial class Add_New_Emp_Editor : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        static OracleDataAdapter _daEdu = new OracleDataAdapter(), _daPW = new OracleDataAdapter(), 
            _daPassport = new OracleDataAdapter(), _daRegistr = new OracleDataAdapter(), _daType_Per_Doc = new OracleDataAdapter(),
            _daHabit = new OracleDataAdapter(), _daPer_Data = new OracleDataAdapter(), _daMil_Card = new OracleDataAdapter(),
            _daProject_Transfer = new OracleDataAdapter(),
            _daTariff_Grid_Salary = new OracleDataAdapter(), _daProject_Term_Transfer = new OracleDataAdapter(), 
            _daProject_List_Repl_Emp = new OracleDataAdapter(), _daProject_Working_Conditions = new OracleDataAdapter(),
            _daProject_Add_Condition = new OracleDataAdapter(), _daProject_Approval = new OracleDataAdapter(),
            _daType_Approval = new OracleDataAdapter(), _daPlan_Approval = new OracleDataAdapter(),
            _daAppendix = new OracleDataAdapter();
        static DataSet _dsDataEmp, _dsDataTransfer, _dsProject_Approval, _dsAppendix;
        static OracleCommand _ocGet_Status_Project, _ocGet_Sign_Open_Approval, _ocPhotoEmp, _ocUpdate_Order, _ocProject_To_Transfer, 
            _ocRegistration_Project, _ocAnnul_Project;
        REGION_seq rregion, hregion;
        DISTRICT_seq rdistrict, hdistrict;
        CITY_seq rcity, hcity;
        LOCALITY_seq rlocality, hlocality;
        STREET_seq rstreet, hstreet;
        REGISTR_seq registr;
        HABIT_seq habit;
        bool f_LoadAddress = false;
        Address formregistr, formhabit;
        private static bool _fl_reload = false;
        public static bool Fl_reload
        {
            get { return _fl_reload; }
            set { _fl_reload = value; }
        }
        private bool _fl_Add_Approval = false;
        public bool Fl_Add_Approval
        {
            get { return _fl_Add_Approval; }
            set 
            {
                if (value != _fl_Add_Approval)
                {
                    _fl_Add_Approval = value;
                    OnPropertyChanged("Fl_Add_Approval");
                }
            }
        }
        DataRowView _row_Approval;
        public static bool VisibleSource_Employability
        {
            get 
            {
                return GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE");
            }
        }
        public Add_New_Emp_Editor(DataRowView dataContext)
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Add_New_Emp_Editor_PropertyChanged);
            InitializeComponent();
            this.DataContext = dataContext;
            registr = new REGISTR_seq(Connect.CurConnect);
            registr.Fill(string.Format("where PER_NUM = '{0}'", dataContext["PER_NUM"]));
            _dsDataEmp.Tables["REGISTR"].Rows.Clear();
            _daRegistr.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daRegistr.Fill(_dsDataEmp.Tables["REGISTR"]);
            if (registr.Count == 0)
            {
                registr.AddNew();
                ((REGISTR_obj)(registr[0])).PER_NUM = dataContext["PER_NUM"].ToString();
                _dsDataEmp.Tables["REGISTR"].Rows.Add(_dsDataEmp.Tables["REGISTR"].DefaultView.AddNew().Row);
                _dsDataEmp.Tables["REGISTR"].DefaultView[0]["PER_NUM"] = dataContext["PER_NUM"];
            }
            habit = new HABIT_seq(Connect.CurConnect);
            habit.Fill(string.Format("where PER_NUM = '{0}'", dataContext["PER_NUM"]));
            _dsDataEmp.Tables["HABIT"].Rows.Clear();
            _daHabit.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daHabit.Fill(_dsDataEmp.Tables["HABIT"]);
            if (habit.Count == 0)
            {
                habit.AddNew();
                ((HABIT_obj)(habit[0])).PER_NUM = dataContext["PER_NUM"].ToString();
                _dsDataEmp.Tables["HABIT"].Rows.Add(_dsDataEmp.Tables["HABIT"].DefaultView.AddNew().Row);
                _dsDataEmp.Tables["HABIT"].DefaultView[0]["PER_NUM"] = dataContext["PER_NUM"];
            }
            gbHabit.DataContext = _dsDataEmp.Tables["HABIT"].DefaultView;
            TabItem_DragEnter();

            _dsDataEmp.Tables["PER_DATA"].Rows.Clear();
            _daPer_Data.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daPer_Data.Fill(_dsDataEmp.Tables["PER_DATA"]);
            if (_dsDataEmp.Tables["PER_DATA"].Rows.Count == 0)
            {
                _dsDataEmp.Tables["PER_DATA"].Rows.Add(_dsDataEmp.Tables["PER_DATA"].DefaultView.AddNew().Row);
                _dsDataEmp.Tables["PER_DATA"].DefaultView[0]["PER_NUM"] = dataContext["PER_NUM"];
            }
            _dsDataEmp.Tables["PASSPORT"].Rows.Clear();
            _daPassport.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daPassport.Fill(_dsDataEmp.Tables["PASSPORT"]);
            if (_dsDataEmp.Tables["PASSPORT"].Rows.Count == 0)
            {
                _dsDataEmp.Tables["PASSPORT"].Rows.Add(_dsDataEmp.Tables["PASSPORT"].DefaultView.AddNew().Row);
                _dsDataEmp.Tables["PASSPORT"].DefaultView[0]["PER_NUM"] = dataContext["PER_NUM"];
            }
            _dsDataEmp.Tables["MIL_CARD"].Rows.Clear();
            _daMil_Card.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daMil_Card.Fill(_dsDataEmp.Tables["MIL_CARD"]);
            if (_dsDataEmp.Tables["MIL_CARD"].Rows.Count == 0)
            {
                _dsDataEmp.Tables["MIL_CARD"].Rows.Add(_dsDataEmp.Tables["MIL_CARD"].DefaultView.AddNew().Row);
                _dsDataEmp.Tables["MIL_CARD"].DefaultView[0]["PER_NUM"] = dataContext["PER_NUM"];
            }

            
            _dsDataEmp.Tables["EDU"].Rows.Clear();
            _daEdu.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _daEdu.Fill(_dsDataEmp.Tables["EDU"]);

            dcSPEC_ID.ItemsSource = AppDataSet.Tables["SPECIALITY"].DefaultView;
            dcINSTIT_ID.ItemsSource = AppDataSet.Tables["INSTIT"].DefaultView;
            dcTYPE_STUDY_ID.ItemsSource = AppDataSet.Tables["TYPE_STUDY"].DefaultView;
            dcTYPE_EDU_ID.ItemsSource = AppDataSet.Tables["TYPE_EDU"].DefaultView;
            dcQUAL_ID.ItemsSource = AppDataSet.Tables["QUAL"].DefaultView;
            dcGR_SPEC_ID.ItemsSource = AppDataSet.Tables["GROUP_SPEC"].DefaultView;

            cbSource_Complect.ItemsSource = AppDataSet.Tables["SOURCE_COMPLECT"].DefaultView;
            cbSource_Employability.ItemsSource = AppDataSet.Tables["SOURCE_EMPLOYABILITY"].DefaultView;
            cbType_Per_Doc.ItemsSource = AppDataSet.Tables["TYPE_PER_DOC"].DefaultView;
            cbMar_State_ID.ItemsSource = AppDataSet.Tables["MAR_STATE"].DefaultView;

            cbSubdiv.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;
            cbSubdivOld.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;
            cbSubdivOld.DataContext = dataContext;

            dgEdu.DataContext = _dsDataEmp.Tables["EDU"].DefaultView;

            gbPassport.DataContext = _dsDataEmp.Tables["PASSPORT"].DefaultView[0];
            gbBirth.DataContext = _dsDataEmp.Tables["PASSPORT"].DefaultView[0];
            gbOther.DataContext = _dsDataEmp.Tables["PER_DATA"].DefaultView[0];
            cbSource_Complect.DataContext = dataContext;

            _dsDataTransfer.Tables["PROJECT_TRANSFER"].Clear();
            _daProject_Transfer.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Transfer.Fill(_dsDataTransfer.Tables["PROJECT_TRANSFER"]);
            if (_dsDataTransfer.Tables["PROJECT_TRANSFER"].DefaultView.Count == 0)
            {
                _dsDataTransfer.Tables["PROJECT_TRANSFER"].Rows.Add(_dsDataTransfer.Tables["PROJECT_TRANSFER"].DefaultView.AddNew().Row);
            }
            tcTransfer_Project.DataContext = _dsDataTransfer.Tables["PROJECT_TRANSFER"].DefaultView[0];
            ((DataRowView)tcTransfer_Project.DataContext).DataView.Table.ColumnChanged += new DataColumnChangeEventHandler(Table_ColumnChanged);

            // Поставил это чтобы формы оплаты обновлялись при каждой смене категории или были пустыми когда кат. не установлена
            _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "1 = 2";

            cbPosition.ItemsSource = AppDataSet.Tables["POSITION"].DefaultView;
            cbDegree.ItemsSource = AppDataSet.Tables["DEGREE"].DefaultView;
            cbFormPay.ItemsSource = _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView;
            cbForm_Operation.ItemsSource = AppDataSet.Tables["FORM_OPERATION"].DefaultView;
            cbTariff_Grid.ItemsSource = AppDataSet.Tables["TARIFF_GRID"].DefaultView;
            cbWorking_Time.ItemsSource = ProjectDataSet.Tables["WORKING_TIME"].DefaultView;

            // Term_Contr
            _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].Rows.Clear();
            _daProject_Term_Transfer.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Term_Transfer.SelectCommand.Parameters["p_TYPE_TERM"].Value = 1;// dataContext["TYPE_TRANSFER_ID"];
            _daProject_Term_Transfer.Fill(_dsDataTransfer.Tables["PROJECT_TERM_CONTR"]);
            if (_dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView.Count == 0)
            {
                _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].Rows.Add(_dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView.AddNew().Row);
                _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["PROJECT_TRANSFER_ID"] = dataContext["PROJECT_TRANSFER_ID"];
            }
            gridTerm_Contr_Emp.DataContext = _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0];

            cbType_Term_Contr_Emp.ItemsSource = ProjectDataSet.Tables["TYPE_TERM_TRANSFER"].DefaultView;
            cbBase_Term_Contr_Emp.ItemsSource = ProjectDataSet.Tables["BASE_TERM_TRANSFER"].DefaultView;

            _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].Clear();
            _daProject_List_Repl_Emp.SelectCommand.Parameters["p_PROJECT_TERM_TRANSFER_ID"].Value =
                _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["PROJECT_TERM_TRANSFER_ID"];
            _daProject_List_Repl_Emp.Fill(_dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"]);
            dgList_Repl_Contr.DataContext = _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView;

            //cbSALARY.ItemsSource = Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView;

            if (((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"].ToString() == "2")
            {
                // Term_Transfer
                _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].Rows.Clear();
                _daProject_Term_Transfer.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
                _daProject_Term_Transfer.SelectCommand.Parameters["p_TYPE_TERM"].Value = 2;
                _daProject_Term_Transfer.Fill(_dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"]);
                if (_dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView.Count == 0)
                {
                    _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].Rows.Add(_dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView.AddNew().Row);
                    _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["PROJECT_TRANSFER_ID"] = dataContext["PROJECT_TRANSFER_ID"];
                }
                gridTerm_Transfer.DataContext = _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0];
                _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].Clear();
                _daProject_List_Repl_Emp.SelectCommand.Parameters["p_PROJECT_TERM_TRANSFER_ID"].Value = 
                    _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["PROJECT_TERM_TRANSFER_ID"];
                _daProject_List_Repl_Emp.Fill(_dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"]);
                dgList_Repl_Emp.DataContext = _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView;
                OracleDataAdapter _da = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectTransfer_Preview.sql"),
                    Connect.Schema), Connect.CurConnect);
                _da.SelectCommand.BindByName = true;
                _da.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = dataContext["FROM_POSITION"];
                _dsDataTransfer.Tables["TRANSFER_PREVIEW"].Clear();
                _da.Fill(_dsDataTransfer.Tables["TRANSFER_PREVIEW"]);
                gridBaseInfoOld.Visibility = System.Windows.Visibility.Visible;
                gridInfo_Add_Old.Visibility = System.Windows.Visibility.Visible;
                gridWork_Conditions_Old.Visibility = System.Windows.Visibility.Visible;
                if (_dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView.Count > 0)
                {
                    gridBaseInfoOld.DataContext = _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0];
                    gridInfo_Add_Old.DataContext = _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0];
                    gridAdd_Condition_Old.DataContext = _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0];
                    //gridTerm_Contr_Emp_Old.DataContext = _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0];
                    //gridTerm_Transfer_Old.DataContext = _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0];
                    cbType_Term_Contr_Emp_Old.ItemsSource = ProjectDataSet.Tables["TYPE_TERM_TRANSFER"].DefaultView;
                    cbBase_Term_Contr_Emp_Old.ItemsSource = ProjectDataSet.Tables["BASE_TERM_TRANSFER"].DefaultView;
                    cbType_Term_Transfer_Old.ItemsSource = ProjectDataSet.Tables["TYPE_TERM_TRANSFER"].DefaultView;
                    cbBase_Term_Transfer_Old.ItemsSource = ProjectDataSet.Tables["BASE_TERM_TRANSFER"].DefaultView;
                    _da = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectList_Repl_Emp.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _da.SelectCommand.BindByName = true;
                    _da.SelectCommand.Parameters.Add("p_TERM_TRANSFER_ID", OracleDbType.Decimal).Value =
                        _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0]["TERM_TRANSFER_ID_PREV"];
                    _dsDataTransfer.Tables["PREVIEW_LIST_REPL_EMP"].Clear();
                    _da.Fill(_dsDataTransfer.Tables["PREVIEW_LIST_REPL_EMP"]);
                    dgList_Repl_Contr_Old.DataContext = _dsDataTransfer.Tables["PREVIEW_LIST_REPL_EMP"];
                    dgList_Repl_Emp_Old.DataContext = _dsDataTransfer.Tables["PREVIEW_LIST_REPL_EMP"];
                    _da = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectProject_Working_Conditions.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _da.SelectCommand.BindByName = true;
                    _da.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                        _dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0]["PROJECT_TRANSFER_ID"];
                    _dsDataTransfer.Tables["PREVIEW_ADD_CONDITION"].Clear();
                    _da.Fill(_dsDataTransfer.Tables["PREVIEW_ADD_CONDITION"]);
                    dgWork_Conditions_Old.DataContext = _dsDataTransfer.Tables["PREVIEW_ADD_CONDITION"];
                    dcCONDITIONS_OF_WORK_ID_OLD.ItemsSource = AppDataSet.Tables["CONDITIONS_OF_WORK"].DefaultView;
                }
            }

            cbType_Term_Transfer.ItemsSource = ProjectDataSet.Tables["TYPE_TERM_TRANSFER"].DefaultView;
            cbBase_Term_Transfer.ItemsSource = ProjectDataSet.Tables["BASE_TERM_TRANSFER"].DefaultView;


            _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].Rows.Clear();
            _daProject_Working_Conditions.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Working_Conditions.Fill(_dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"]);
            dgWork_Conditions.DataContext = _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView;

            dcCONDITIONS_OF_WORK_ID.ItemsSource = AppDataSet.Tables["CONDITIONS_OF_WORK"].DefaultView;
            _dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].Rows.Clear();
            _daProject_Add_Condition.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Add_Condition.Fill(_dsDataTransfer.Tables["PROJECT_ADD_CONDITION"]);
            if (_dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].DefaultView.Count == 0)
            {
                _dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].Rows.Add(_dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].DefaultView.AddNew().Row);
                _dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].DefaultView[0]["PROJECT_TRANSFER_ID"] = dataContext["PROJECT_TRANSFER_ID"];
            }  
            gridAdd_Condition.DataContext = _dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].DefaultView[0];
            cbPRIVILEGED_POSITION_ID.ItemsSource = AppDataSet.Tables["PRIVILEGED_POSITION"].DefaultView;

            _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.Clear();            
            _daProject_Approval.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Approval.Fill(_dsProject_Approval.Tables["PROJECT_APPROVAL"]);
            dgProject_Approval.DataContext = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView;

            dcPROJECT_PLAN_APPROVAL_ID.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;
            //dcTYPE_APPROVAL_ID.ItemsSource = AppDataSet.Tables["TYPE_APPROVAL"].DefaultView;

            cbTYPE_APPROVAL_ID.ItemsSource = _dsProject_Approval.Tables["TYPE_APPROVAL"].DefaultView;

            cbBASE_DOC_ID.ItemsSource = ProjectDataSet.Tables["BASE_DOC"].DefaultView;
            _dsProject_Approval.Tables["PLAN_APPROVAL"].Clear();
            _daPlan_Approval.SelectCommand.Parameters["p_PROJECT_PLAN_APPROVAL_ID"].Value = dataContext["PROJECT_PLAN_APPROVAL_ID"];
            _daPlan_Approval.Fill(_dsProject_Approval.Tables["PLAN_APPROVAL"]);

            // Команда возвращает статус проекта
            _ocGet_Status_Project.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _ocGet_Sign_Open_Approval.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];

            GetStatusProject();

            _ocPhotoEmp.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _ocPhotoEmp.ExecuteNonQuery();
            if (!(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                    new MemoryStream((byte[])(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);

            if (_dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView.Count > 0)
            {
                dpStatusProject.DataContext = _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0];
            }

            _daAppendix.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];            
            _daAppendix.Fill(_dsAppendix.Tables["PROJECT_APPENDIX"]);
            dgAppendix.DataContext = _dsAppendix.Tables["PROJECT_APPENDIX"].DefaultView;
            if (!_dsAppendix.Tables["PROJECT_APPENDIX"].Columns.Contains("DOCUMENT"))
            {
                _dsAppendix.Tables["PROJECT_APPENDIX"].Columns.Add("DOCUMENT", Type.GetType("System.Byte[]"));
            }
        }

        void Add_New_Emp_Editor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        void Table_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "CLASSIFIC" || e.Column.ColumnName == "TARIFF_GRID_ID")
            {
                if (//e.Row.Table.DefaultView[0]["DATE_HIRE"] != DBNull.Value &&
                    e.Row.Table.DefaultView[0]["CLASSIFIC"] != DBNull.Value &&
                    e.Row.Table.DefaultView[0]["TARIFF_GRID_ID"] != DBNull.Value)
                {
                    //Tariff_Grid_Salary.UpdateSet(System.Convert.ToDateTime(e.Row.Table.DefaultView[0]["DATE_HIRE"]));
                    Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.RowFilter =
                        string.Format("TARIFF_GRID_ID = {0} and TAR_CLASSIF = {1} and (TARIFF_END_DATE >= #{2}# And TAR_DATE <= #{2}# ) ",
                        e.Row.Table.DefaultView[0]["TARIFF_GRID_ID"], e.Row.Table.DefaultView[0]["CLASSIFIC"],
                        //System.Convert.ToDateTime(e.Row.Table.DefaultView[0]["DATE_HIRE"]).ToString("MM/dd/yyyy"));
                        DateTime.Today.ToString("MM/dd/yyyy"));
                    if (Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.Count == 1)
                    {
                        e.Row.Table.DefaultView[0]["SALARY"] = Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView[0]["TAR_SAL"];
                    }
                }
            }
        }

        static Add_New_Emp_Editor()
        {
            _dsDataEmp = new DataSet();
            _dsDataEmp.Tables.Add("PER_DATA");
            _dsDataEmp.Tables.Add("PASSPORT");
            _dsDataEmp.Tables.Add("REGISTR");
            _dsDataEmp.Tables.Add("HABIT");
            _dsDataEmp.Tables.Add("MIL_CARD");
            _dsDataEmp.Tables.Add("EDU");
            _dsDataTransfer = new DataSet();
            _dsDataTransfer.Tables.Add("PROJECT_TRANSFER");
            _dsDataTransfer.Tables.Add("TRANSFER_PREVIEW");
            _dsDataTransfer.Tables.Add("PREVIEW_LIST_REPL_EMP");
            _dsDataTransfer.Tables.Add("PREVIEW_ADD_CONDITION");
            _dsDataTransfer.Tables.Add("FORM_PAY_ON_DEGREE");
            _dsDataTransfer.Tables.Add("TARIFF_GRID_SALARY");
            _dsDataTransfer.Tables.Add("PROJECT_TERM_TRANSFER");
            _dsDataTransfer.Tables.Add("PROJECT_TERM_CONTR");
            _dsDataTransfer.Tables.Add("PROJECT_LIST_REPL_EMP");
            _dsDataTransfer.Tables.Add("PROJECT_LIST_REPL_CONTR");
            _dsDataTransfer.Tables.Add("PROJECT_WORK_CONDITIONS");
            _dsDataTransfer.Tables.Add("PROJECT_ADD_CONDITION");
            _dsDataTransfer.Tables.Add("TRANSFER_QM");
            _dsProject_Approval = new DataSet();
            _dsProject_Approval.Tables.Add("PROJECT_APPROVAL");
            _dsProject_Approval.Tables.Add("TYPE_APPROVAL");
            _dsProject_Approval.Tables.Add("PLAN_APPROVAL");
            _dsAppendix = new DataSet();
            _dsAppendix.Tables.Add("PROJECT_APPENDIX");
            #region Данные обновляет отдел кадров
            // Select
            _daPer_Data.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, TRIP_SIGN, RETIRER_SIGN, SIGN_PROFUNION, SIGN_YOUNG_SPEC, INSURANCE_NUM, SER_MED_POLUS, NUM_MED_POLUS, INN, SOURCE_EMPLOYABILITY_ID 
                from {0}.PER_DATA where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPer_Data.SelectCommand.BindByName = true;
            _daPer_Data.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daPer_Data.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PER_DATA_insert(:PER_NUM,:INSURANCE_NUM,:SER_MED_POLUS,
                        :NUM_MED_POLUS,:INN,:SOURCE_EMPLOYABILITY_ID,:TRIP_SIGN,:RETIRER_SIGN,:SIGN_PROFUNION,:SIGN_YOUNG_SPEC);
                END;", Connect.Schema), Connect.CurConnect);
            _daPer_Data.InsertCommand.BindByName = true;
            _daPer_Data.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPer_Data.InsertCommand.Parameters.Add("TRIP_SIGN", OracleDbType.Int16, 0, "TRIP_SIGN");
            _daPer_Data.InsertCommand.Parameters.Add("RETIRER_SIGN", OracleDbType.Int16, 0, "RETIRER_SIGN");
            _daPer_Data.InsertCommand.Parameters.Add("SIGN_PROFUNION", OracleDbType.Int16, 0, "SIGN_PROFUNION");
            _daPer_Data.InsertCommand.Parameters.Add("SIGN_YOUNG_SPEC", OracleDbType.Int16, 0, "SIGN_YOUNG_SPEC");
            _daPer_Data.InsertCommand.Parameters.Add("INSURANCE_NUM", OracleDbType.Varchar2, 0, "INSURANCE_NUM");
            _daPer_Data.InsertCommand.Parameters.Add("SER_MED_POLUS", OracleDbType.Varchar2, 0, "SER_MED_POLUS");
            _daPer_Data.InsertCommand.Parameters.Add("NUM_MED_POLUS", OracleDbType.Varchar2, 0, "NUM_MED_POLUS");
            _daPer_Data.InsertCommand.Parameters.Add("INN", OracleDbType.Varchar2, 0, "INN");
            _daPer_Data.InsertCommand.Parameters.Add("SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal, 0, "SOURCE_EMPLOYABILITY_ID");
            // Update
            _daPer_Data.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PER_DATA_update(:PER_NUM,:PER_NUM,:INSURANCE_NUM,:SER_MED_POLUS,
                        :NUM_MED_POLUS,:INN,:SOURCE_EMPLOYABILITY_ID,:TRIP_SIGN,:RETIRER_SIGN,:SIGN_PROFUNION,:SIGN_YOUNG_SPEC);
                END;", Connect.Schema), Connect.CurConnect);
            _daPer_Data.UpdateCommand.BindByName = true;
            _daPer_Data.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPer_Data.UpdateCommand.Parameters.Add("TRIP_SIGN", OracleDbType.Int16, 0, "TRIP_SIGN");
            _daPer_Data.UpdateCommand.Parameters.Add("RETIRER_SIGN", OracleDbType.Int16, 0, "RETIRER_SIGN");
            _daPer_Data.UpdateCommand.Parameters.Add("SIGN_PROFUNION", OracleDbType.Int16, 0, "SIGN_PROFUNION");
            _daPer_Data.UpdateCommand.Parameters.Add("SIGN_YOUNG_SPEC", OracleDbType.Int16, 0, "SIGN_YOUNG_SPEC");
            _daPer_Data.UpdateCommand.Parameters.Add("INSURANCE_NUM", OracleDbType.Varchar2, 0, "INSURANCE_NUM");
            _daPer_Data.UpdateCommand.Parameters.Add("SER_MED_POLUS", OracleDbType.Varchar2, 0, "SER_MED_POLUS");
            _daPer_Data.UpdateCommand.Parameters.Add("NUM_MED_POLUS", OracleDbType.Varchar2, 0, "NUM_MED_POLUS");
            _daPer_Data.UpdateCommand.Parameters.Add("INN", OracleDbType.Varchar2, 0, "INN");
            _daPer_Data.UpdateCommand.Parameters.Add("SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal, 0, "SOURCE_EMPLOYABILITY_ID");

            // Select EDU
            _daEdu = new OracleDataAdapter(string.Format(Queries.GetQuery("Resume/SelectEdu.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEdu.SelectCommand.BindByName = true;
            _daEdu.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daEdu.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EDU_insert(:EDU_ID,:SPEC_ID,:PER_NUM,:INSTIT_ID,:TYPE_STUDY_ID,:TYPE_EDU_ID,:MAIN_PROF,:SERIA_DIPLOMA,:NUM_DIPLOMA,:QUAL_ID,
                        :SPECIALIZATION,:YEAR_GRADUATING,:GR_SPEC_ID,:FROM_FACT);
                END;", Connect.Schema), Connect.CurConnect);
            _daEdu.InsertCommand.BindByName = true;
            _daEdu.InsertCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");
            _daEdu.InsertCommand.Parameters.Add("SPEC_ID", OracleDbType.Decimal, 0, "SPEC_ID");
            _daEdu.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daEdu.InsertCommand.Parameters.Add("INSTIT_ID", OracleDbType.Decimal, 0, "INSTIT_ID");
            _daEdu.InsertCommand.Parameters.Add("TYPE_STUDY_ID", OracleDbType.Decimal, 0, "TYPE_STUDY_ID");
            _daEdu.InsertCommand.Parameters.Add("TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID");
            _daEdu.InsertCommand.Parameters.Add("MAIN_PROF", OracleDbType.Int16, 0, "MAIN_PROF");
            _daEdu.InsertCommand.Parameters.Add("SERIA_DIPLOMA", OracleDbType.Varchar2, 0, "SERIA_DIPLOMA");
            _daEdu.InsertCommand.Parameters.Add("NUM_DIPLOMA", OracleDbType.Varchar2, 0, "NUM_DIPLOMA");
            _daEdu.InsertCommand.Parameters.Add("QUAL_ID", OracleDbType.Decimal, 0, "QUAL_ID");
            _daEdu.InsertCommand.Parameters.Add("SPECIALIZATION", OracleDbType.Varchar2, 0, "SPECIALIZATION");
            _daEdu.InsertCommand.Parameters.Add("YEAR_GRADUATING", OracleDbType.Date, 0, "YEAR_GRADUATING");
            _daEdu.InsertCommand.Parameters.Add("GR_SPEC_ID", OracleDbType.Decimal, 0, "GR_SPEC_ID");
            _daEdu.InsertCommand.Parameters.Add("FROM_FACT", OracleDbType.Int16, 0, "FROM_FACT");
            // Update
            _daEdu.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EDU_update(:EDU_ID,:SPEC_ID,:PER_NUM,:INSTIT_ID,:TYPE_STUDY_ID,:TYPE_EDU_ID,:MAIN_PROF,:SERIA_DIPLOMA,:NUM_DIPLOMA,:QUAL_ID,
                        :SPECIALIZATION,:YEAR_GRADUATING,:GR_SPEC_ID,:FROM_FACT);
                END;", Connect.Schema), Connect.CurConnect);
            _daEdu.UpdateCommand.BindByName = true;
            _daEdu.UpdateCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");
            _daEdu.UpdateCommand.Parameters.Add("SPEC_ID", OracleDbType.Decimal, 0, "SPEC_ID");
            _daEdu.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daEdu.UpdateCommand.Parameters.Add("INSTIT_ID", OracleDbType.Decimal, 0, "INSTIT_ID");
            _daEdu.UpdateCommand.Parameters.Add("TYPE_STUDY_ID", OracleDbType.Decimal, 0, "TYPE_STUDY_ID");
            _daEdu.UpdateCommand.Parameters.Add("TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID");
            _daEdu.UpdateCommand.Parameters.Add("MAIN_PROF", OracleDbType.Int16, 0, "MAIN_PROF");
            _daEdu.UpdateCommand.Parameters.Add("SERIA_DIPLOMA", OracleDbType.Varchar2, 0, "SERIA_DIPLOMA");
            _daEdu.UpdateCommand.Parameters.Add("NUM_DIPLOMA", OracleDbType.Varchar2, 0, "NUM_DIPLOMA");
            _daEdu.UpdateCommand.Parameters.Add("QUAL_ID", OracleDbType.Decimal, 0, "QUAL_ID");
            _daEdu.UpdateCommand.Parameters.Add("SPECIALIZATION", OracleDbType.Varchar2, 0, "SPECIALIZATION");
            _daEdu.UpdateCommand.Parameters.Add("YEAR_GRADUATING", OracleDbType.Date, 0, "YEAR_GRADUATING");
            _daEdu.UpdateCommand.Parameters.Add("GR_SPEC_ID", OracleDbType.Decimal, 0, "GR_SPEC_ID");
            _daEdu.UpdateCommand.Parameters.Add("FROM_FACT", OracleDbType.Int16, 0, "FROM_FACT");
            // Delete
            _daEdu.DeleteCommand = new OracleCommand(string.Format(
                "BEGIN {0}.EDU_delete(:EDU_ID); END;", Connect.Schema), Connect.CurConnect);
            _daEdu.DeleteCommand.BindByName = true;
            _daEdu.DeleteCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");
            
            // Select
            _daPassport.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, SERIA_PASSPORT,NUM_PASSPORT,WHO_GIVEN,WHEN_GIVEN,CITIZENSHIP,TYPE_PER_DOC_ID,MAR_STATE_ID,
                    COUNTRY_BIRTH,CITY_BIRTH,REGION_BIRTH,DISTR_BIRTH,LOCALITY_BIRTH
                from {0}.PASSPORT where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPassport.SelectCommand.BindByName = true;
            _daPassport.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daPassport.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PASSPORT_insert(:PER_NUM,:NUM_PASSPORT,:SERIA_PASSPORT ,:WHO_GIVEN,:WHEN_GIVEN,:CITIZENSHIP,:COUNTRY_BIRTH,:CITY_BIRTH,
                        :REGION_BIRTH,:DISTR_BIRTH,:LOCALITY_BIRTH,:MAR_STATE_ID,:TYPE_PER_DOC_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daPassport.InsertCommand.BindByName = true;
            _daPassport.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPassport.InsertCommand.Parameters.Add("NUM_PASSPORT", OracleDbType.Varchar2, 0, "NUM_PASSPORT");
            _daPassport.InsertCommand.Parameters.Add("SERIA_PASSPORT", OracleDbType.Varchar2, 0, "SERIA_PASSPORT");
            _daPassport.InsertCommand.Parameters.Add("WHO_GIVEN", OracleDbType.Varchar2, 0, "WHO_GIVEN");
            _daPassport.InsertCommand.Parameters.Add("WHEN_GIVEN", OracleDbType.Date, 0, "WHEN_GIVEN");
            _daPassport.InsertCommand.Parameters.Add("CITIZENSHIP", OracleDbType.Varchar2, 0, "CITIZENSHIP");
            _daPassport.InsertCommand.Parameters.Add("TYPE_PER_DOC_ID", OracleDbType.Decimal, 0, "TYPE_PER_DOC_ID");
            _daPassport.InsertCommand.Parameters.Add("MAR_STATE_ID", OracleDbType.Decimal, 0, "MAR_STATE_ID");
            _daPassport.InsertCommand.Parameters.Add("COUNTRY_BIRTH", OracleDbType.Varchar2, 0, "COUNTRY_BIRTH");
            _daPassport.InsertCommand.Parameters.Add("CITY_BIRTH", OracleDbType.Varchar2, 0, "CITY_BIRTH");
            _daPassport.InsertCommand.Parameters.Add("REGION_BIRTH", OracleDbType.Varchar2, 0, "REGION_BIRTH");
            _daPassport.InsertCommand.Parameters.Add("DISTR_BIRTH", OracleDbType.Varchar2, 0, "DISTR_BIRTH");
            _daPassport.InsertCommand.Parameters.Add("LOCALITY_BIRTH", OracleDbType.Varchar2, 0, "LOCALITY_BIRTH");
            // Update
            _daPassport.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PASSPORT_update(:PER_NUM,:PER_NUM,:NUM_PASSPORT,:SERIA_PASSPORT,:WHO_GIVEN,:WHEN_GIVEN,:CITIZENSHIP,
                        :COUNTRY_BIRTH,:CITY_BIRTH,:REGION_BIRTH,:DISTR_BIRTH,:LOCALITY_BIRTH,:MAR_STATE_ID,:TYPE_PER_DOC_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daPassport.UpdateCommand.BindByName = true;
            _daPassport.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPassport.UpdateCommand.Parameters.Add("SERIA_PASSPORT", OracleDbType.Varchar2, 0, "SERIA_PASSPORT");
            _daPassport.UpdateCommand.Parameters.Add("NUM_PASSPORT", OracleDbType.Varchar2, 0, "NUM_PASSPORT");
            _daPassport.UpdateCommand.Parameters.Add("WHO_GIVEN", OracleDbType.Varchar2, 0, "WHO_GIVEN");
            _daPassport.UpdateCommand.Parameters.Add("WHEN_GIVEN", OracleDbType.Date, 0, "WHEN_GIVEN");
            _daPassport.UpdateCommand.Parameters.Add("CITIZENSHIP", OracleDbType.Varchar2, 0, "CITIZENSHIP");
            _daPassport.UpdateCommand.Parameters.Add("TYPE_PER_DOC_ID", OracleDbType.Decimal, 0, "TYPE_PER_DOC_ID");
            _daPassport.UpdateCommand.Parameters.Add("MAR_STATE_ID", OracleDbType.Decimal, 0, "MAR_STATE_ID");
            _daPassport.UpdateCommand.Parameters.Add("COUNTRY_BIRTH", OracleDbType.Varchar2, 0, "COUNTRY_BIRTH");
            _daPassport.UpdateCommand.Parameters.Add("CITY_BIRTH", OracleDbType.Varchar2, 0, "CITY_BIRTH");
            _daPassport.UpdateCommand.Parameters.Add("REGION_BIRTH", OracleDbType.Varchar2, 0, "REGION_BIRTH");
            _daPassport.UpdateCommand.Parameters.Add("DISTR_BIRTH", OracleDbType.Varchar2, 0, "DISTR_BIRTH");
            _daPassport.UpdateCommand.Parameters.Add("LOCALITY_BIRTH", OracleDbType.Varchar2, 0, "LOCALITY_BIRTH");

            // Select REGISTR
            _daRegistr.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, REG_CODE_STREET, REG_HOUSE, REG_BULK, REG_FLAT, REG_POST_CODE, DATE_REG, REG_PHONE, SOURCE_FILL_ID
                from {0}.REGISTR where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daRegistr.SelectCommand.BindByName = true;
            _daRegistr.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daRegistr.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.REGISTR_insert(:PER_NUM,:REG_POST_CODE,:REG_FLAT,:REG_HOUSE,:REG_BULK,:REG_CODE_STREET,:REG_PHONE,:DATE_REG,:SOURCE_FILL_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daRegistr.InsertCommand.BindByName = true;
            _daRegistr.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daRegistr.InsertCommand.Parameters.Add("REG_CODE_STREET", OracleDbType.Varchar2, 0, "REG_CODE_STREET");
            _daRegistr.InsertCommand.Parameters.Add("REG_HOUSE", OracleDbType.Varchar2, 0, "REG_HOUSE");
            _daRegistr.InsertCommand.Parameters.Add("REG_BULK", OracleDbType.Varchar2, 0, "REG_BULK");
            _daRegistr.InsertCommand.Parameters.Add("REG_FLAT", OracleDbType.Varchar2, 0, "REG_FLAT");
            _daRegistr.InsertCommand.Parameters.Add("REG_POST_CODE", OracleDbType.Varchar2, 0, "REG_POST_CODE");
            _daRegistr.InsertCommand.Parameters.Add("DATE_REG", OracleDbType.Date, 0, "DATE_REG");
            _daRegistr.InsertCommand.Parameters.Add("REG_PHONE", OracleDbType.Varchar2, 0, "REG_PHONE");
            _daRegistr.InsertCommand.Parameters.Add("SOURCE_FILL_ID", OracleDbType.Decimal, 0, "SOURCE_FILL_ID");
            // Update
            _daRegistr.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.REGISTR_update(:PER_NUM,:PER_NUM,:REG_POST_CODE,:REG_FLAT,:REG_HOUSE,:REG_BULK,:REG_CODE_STREET,:REG_PHONE,:DATE_REG,:SOURCE_FILL_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daRegistr.UpdateCommand.BindByName = true;
            _daRegistr.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daRegistr.UpdateCommand.Parameters.Add("REG_CODE_STREET", OracleDbType.Varchar2, 0, "REG_CODE_STREET");
            _daRegistr.UpdateCommand.Parameters.Add("REG_HOUSE", OracleDbType.Varchar2, 0, "REG_HOUSE");
            _daRegistr.UpdateCommand.Parameters.Add("REG_BULK", OracleDbType.Varchar2, 0, "REG_BULK");
            _daRegistr.UpdateCommand.Parameters.Add("REG_FLAT", OracleDbType.Varchar2, 0, "REG_FLAT");
            _daRegistr.UpdateCommand.Parameters.Add("REG_POST_CODE", OracleDbType.Varchar2, 0, "REG_POST_CODE");
            _daRegistr.UpdateCommand.Parameters.Add("DATE_REG", OracleDbType.Date, 0, "DATE_REG");
            _daRegistr.UpdateCommand.Parameters.Add("REG_PHONE", OracleDbType.Varchar2, 0, "REG_PHONE");
            _daRegistr.UpdateCommand.Parameters.Add("SOURCE_FILL_ID", OracleDbType.Decimal, 0, "SOURCE_FILL_ID");

            // Select HABIT
            _daHabit.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, HAB_POST_CODE, HAB_FLAT, HAB_HOUSE, HAB_BULK, HAB_CODE_STREET, HAB_PHONE, HAB_NON_KLADR_ADDRESS 
                from {0}.HABIT where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daHabit.SelectCommand.BindByName = true;
            _daHabit.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daHabit.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.HABIT_insert(:PER_NUM,:HAB_POST_CODE,:HAB_FLAT,:HAB_HOUSE,:HAB_BULK,:HAB_CODE_STREET,:HAB_PHONE,:HAB_NON_KLADR_ADDRESS);
                END;", Connect.Schema), Connect.CurConnect);
            _daHabit.InsertCommand.BindByName = true;
            _daHabit.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daHabit.InsertCommand.Parameters.Add("HAB_POST_CODE", OracleDbType.Varchar2, 0, "HAB_POST_CODE");
            _daHabit.InsertCommand.Parameters.Add("HAB_FLAT", OracleDbType.Varchar2, 0, "HAB_FLAT");
            _daHabit.InsertCommand.Parameters.Add("HAB_HOUSE", OracleDbType.Varchar2, 0, "HAB_HOUSE");
            _daHabit.InsertCommand.Parameters.Add("HAB_BULK", OracleDbType.Varchar2, 0, "HAB_BULK");
            _daHabit.InsertCommand.Parameters.Add("HAB_CODE_STREET", OracleDbType.Varchar2, 0, "HAB_CODE_STREET");
            _daHabit.InsertCommand.Parameters.Add("HAB_PHONE", OracleDbType.Varchar2, 0, "HAB_PHONE");
            _daHabit.InsertCommand.Parameters.Add("HAB_NON_KLADR_ADDRESS", OracleDbType.Varchar2, 0, "HAB_NON_KLADR_ADDRESS");
            // Update
            _daHabit.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.HABIT_update(:PER_NUM,:PER_NUM,:HAB_POST_CODE,:HAB_FLAT,:HAB_HOUSE,:HAB_BULK,:HAB_CODE_STREET,:HAB_PHONE,:HAB_NON_KLADR_ADDRESS);
                END;", Connect.Schema), Connect.CurConnect);
            _daHabit.UpdateCommand.BindByName = true;
            _daHabit.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daHabit.UpdateCommand.Parameters.Add("HAB_POST_CODE", OracleDbType.Varchar2, 0, "HAB_POST_CODE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_FLAT", OracleDbType.Varchar2, 0, "HAB_FLAT");
            _daHabit.UpdateCommand.Parameters.Add("HAB_HOUSE", OracleDbType.Varchar2, 0, "HAB_HOUSE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_BULK", OracleDbType.Varchar2, 0, "HAB_BULK");
            _daHabit.UpdateCommand.Parameters.Add("HAB_CODE_STREET", OracleDbType.Varchar2, 0, "HAB_CODE_STREET");
            _daHabit.UpdateCommand.Parameters.Add("HAB_PHONE", OracleDbType.Varchar2, 0, "HAB_PHONE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_NON_KLADR_ADDRESS", OracleDbType.Varchar2, 0, "HAB_NON_KLADR_ADDRESS");

            // Select MIL_CARD
            _daMil_Card.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, MIL_RANK_ID, MED_CLASSIF_ID, RES_CAT, NAME_MIL_SPEC, COMM_ID, MIL_STATE, DATE_MOB, 
                    DATE_DEMOB, RECRUIT_SIGN, NUM_DOC_RECRUIT, MIL_CAT_ID, TYPE_TROOP_ID, MOB_ORDER, MIL_GROUP, SPECIAL_REG, 
                    PLACE_SERVICE, MATTER_NO_SERVICE, DATE_POST_FACT, MATTER_REMOVE, DATE_REMOVE, DATE_GET_MIL_CARD 
                from {0}.MIL_CARD where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daMil_Card.SelectCommand.BindByName = true;
            _daMil_Card.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daMil_Card.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.MIL_CARD_insert(:PER_NUM,:MIL_RANK_ID,:MED_CLASSIF_ID,:RES_CAT,:NAME_MIL_SPEC,:COMM_ID,:MIL_STATE,:DATE_MOB,
                        :DATE_DEMOB,:RECRUIT_SIGN,:NUM_DOC_RECRUIT,:MIL_CAT_ID,:TYPE_TROOP_ID,:MOB_ORDER,:MIL_GROUP,:SPECIAL_REG,
                        :PLACE_SERVICE,:MATTER_NO_SERVICE,:DATE_POST_FACT,:MATTER_REMOVE,:DATE_REMOVE,:DATE_GET_MIL_CARD);
                END;", Connect.Schema), Connect.CurConnect);
            _daMil_Card.InsertCommand.BindByName = true;
            _daMil_Card.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daMil_Card.InsertCommand.Parameters.Add("MIL_RANK_ID", OracleDbType.Decimal, 0, "MIL_RANK_ID");
            _daMil_Card.InsertCommand.Parameters.Add("MED_CLASSIF_ID", OracleDbType.Decimal, 0, "MED_CLASSIF_ID");
            _daMil_Card.InsertCommand.Parameters.Add("RES_CAT", OracleDbType.Varchar2, 0, "RES_CAT");
            _daMil_Card.InsertCommand.Parameters.Add("NAME_MIL_SPEC", OracleDbType.Varchar2, 0, "NAME_MIL_SPEC");
            _daMil_Card.InsertCommand.Parameters.Add("COMM_ID", OracleDbType.Decimal, 0, "COMM_ID");
            _daMil_Card.InsertCommand.Parameters.Add("MIL_STATE", OracleDbType.Decimal, 0, "MIL_STATE");
            _daMil_Card.InsertCommand.Parameters.Add("DATE_MOB", OracleDbType.Date, 0, "DATE_MOB");
            _daMil_Card.InsertCommand.Parameters.Add("DATE_DEMOB", OracleDbType.Date, 0, "DATE_DEMOB");
            _daMil_Card.InsertCommand.Parameters.Add("RECRUIT_SIGN", OracleDbType.Decimal, 0, "RECRUIT_SIGN");
            _daMil_Card.InsertCommand.Parameters.Add("NUM_DOC_RECRUIT", OracleDbType.Varchar2, 0, "NUM_DOC_RECRUIT");
            _daMil_Card.InsertCommand.Parameters.Add("MIL_CAT_ID", OracleDbType.Decimal, 0, "MIL_CAT_ID");
            _daMil_Card.InsertCommand.Parameters.Add("TYPE_TROOP_ID", OracleDbType.Decimal, 0, "TYPE_TROOP_ID");
            _daMil_Card.InsertCommand.Parameters.Add("MOB_ORDER", OracleDbType.Varchar2, 0, "MOB_ORDER");
            _daMil_Card.InsertCommand.Parameters.Add("MIL_GROUP", OracleDbType.Varchar2, 0, "MIL_GROUP");
            _daMil_Card.InsertCommand.Parameters.Add("SPECIAL_REG", OracleDbType.Varchar2, 0, "SPECIAL_REG");
            _daMil_Card.InsertCommand.Parameters.Add("PLACE_SERVICE", OracleDbType.Varchar2, 0, "PLACE_SERVICE");
            _daMil_Card.InsertCommand.Parameters.Add("MATTER_NO_SERVICE", OracleDbType.Varchar2, 0, "MATTER_NO_SERVICE");
            _daMil_Card.InsertCommand.Parameters.Add("DATE_POST_FACT", OracleDbType.Date, 0, "DATE_POST_FACT");
            _daMil_Card.InsertCommand.Parameters.Add("MATTER_REMOVE", OracleDbType.Varchar2, 0, "MATTER_REMOVE");
            _daMil_Card.InsertCommand.Parameters.Add("DATE_REMOVE", OracleDbType.Date, 0, "DATE_REMOVE");
            _daMil_Card.InsertCommand.Parameters.Add("DATE_GET_MIL_CARD", OracleDbType.Date, 0, "DATE_GET_MIL_CARD");
            // Update
            _daMil_Card.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.MIL_CARD_update(:PER_NUM,:PER_NUM,:MIL_RANK_ID,:MED_CLASSIF_ID,:RES_CAT,:NAME_MIL_SPEC,:COMM_ID,:MIL_STATE,:DATE_MOB,
                        :DATE_DEMOB,:RECRUIT_SIGN,:NUM_DOC_RECRUIT,:MIL_CAT_ID,:TYPE_TROOP_ID,:MOB_ORDER,:MIL_GROUP,:SPECIAL_REG,
                        :PLACE_SERVICE,:MATTER_NO_SERVICE,:DATE_POST_FACT,:MATTER_REMOVE,:DATE_REMOVE,:DATE_GET_MIL_CARD);
                END;", Connect.Schema), Connect.CurConnect);
            _daMil_Card.UpdateCommand.BindByName = true;
            _daMil_Card.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daMil_Card.UpdateCommand.Parameters.Add("MIL_RANK_ID", OracleDbType.Decimal, 0, "MIL_RANK_ID");
            _daMil_Card.UpdateCommand.Parameters.Add("MED_CLASSIF_ID", OracleDbType.Decimal, 0, "MED_CLASSIF_ID");
            _daMil_Card.UpdateCommand.Parameters.Add("RES_CAT", OracleDbType.Varchar2, 0, "RES_CAT");
            _daMil_Card.UpdateCommand.Parameters.Add("NAME_MIL_SPEC", OracleDbType.Varchar2, 0, "NAME_MIL_SPEC");
            _daMil_Card.UpdateCommand.Parameters.Add("COMM_ID", OracleDbType.Decimal, 0, "COMM_ID");
            _daMil_Card.UpdateCommand.Parameters.Add("MIL_STATE", OracleDbType.Decimal, 0, "MIL_STATE");
            _daMil_Card.UpdateCommand.Parameters.Add("DATE_MOB", OracleDbType.Date, 0, "DATE_MOB");
            _daMil_Card.UpdateCommand.Parameters.Add("DATE_DEMOB", OracleDbType.Date, 0, "DATE_DEMOB");
            _daMil_Card.UpdateCommand.Parameters.Add("RECRUIT_SIGN", OracleDbType.Decimal, 0, "RECRUIT_SIGN");
            _daMil_Card.UpdateCommand.Parameters.Add("NUM_DOC_RECRUIT", OracleDbType.Varchar2, 0, "NUM_DOC_RECRUIT");
            _daMil_Card.UpdateCommand.Parameters.Add("MIL_CAT_ID", OracleDbType.Decimal, 0, "MIL_CAT_ID");
            _daMil_Card.UpdateCommand.Parameters.Add("TYPE_TROOP_ID", OracleDbType.Decimal, 0, "TYPE_TROOP_ID");
            _daMil_Card.UpdateCommand.Parameters.Add("MOB_ORDER", OracleDbType.Varchar2, 0, "MOB_ORDER");
            _daMil_Card.UpdateCommand.Parameters.Add("MIL_GROUP", OracleDbType.Varchar2, 0, "MIL_GROUP");
            _daMil_Card.UpdateCommand.Parameters.Add("SPECIAL_REG", OracleDbType.Varchar2, 0, "SPECIAL_REG");
            _daMil_Card.UpdateCommand.Parameters.Add("PLACE_SERVICE", OracleDbType.Varchar2, 0, "PLACE_SERVICE");
            _daMil_Card.UpdateCommand.Parameters.Add("MATTER_NO_SERVICE", OracleDbType.Varchar2, 0, "MATTER_NO_SERVICE");
            _daMil_Card.UpdateCommand.Parameters.Add("DATE_POST_FACT", OracleDbType.Date, 0, "DATE_POST_FACT");
            _daMil_Card.UpdateCommand.Parameters.Add("MATTER_REMOVE", OracleDbType.Varchar2, 0, "MATTER_REMOVE");
            _daMil_Card.UpdateCommand.Parameters.Add("DATE_REMOVE", OracleDbType.Date, 0, "DATE_REMOVE");
            _daMil_Card.UpdateCommand.Parameters.Add("DATE_GET_MIL_CARD", OracleDbType.Date, 0, "DATE_GET_MIL_CARD");
            #endregion

            #region Данные обновляет инспектор по кадрам
            // Select
            _daProject_Transfer.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectTransfer_Project.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Transfer.SelectCommand.BindByName = true;
            _daProject_Transfer.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            // Update
            _daProject_Transfer.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.PROJECT_TRANSFER_INSPECTOR(:PROJECT_TRANSFER_ID,:POS_ID,:POS_NOTE,:FORM_PAY,:DEGREE_ID,:PROBA_PERIOD,:FORM_OPERATION_ID,
                    :HARMFUL_VAC,:ADDITIONAL_VAC,:HARMFUL_ADDITION_ADD,:COMB_ADDITION,:COMB_ADDITION_NOTE,:SALARY,:CLASSIFIC,:TARIFF_GRID_ID,
                    :SECRET_ADDITION,:SECRET_ADDITION_NOTE,:SIGN_MAT_RESP_CONTR,:OTHER_LIABILITY_BOSS,:WORKING_TIME_ID,:WORKING_TIME_COMMENT); 
                END;",
                Connect.Schema), Connect.CurConnect);
            _daProject_Transfer.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Transfer.UpdateCommand.BindByName = true;
            _daProject_Transfer.UpdateCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daProject_Transfer.UpdateCommand.Parameters.Add("POS_ID", OracleDbType.Decimal, 0, "POS_ID");
            _daProject_Transfer.UpdateCommand.Parameters.Add("POS_NOTE", OracleDbType.Varchar2, 0, "POS_NOTE");
            _daProject_Transfer.UpdateCommand.Parameters.Add("FORM_PAY", OracleDbType.Decimal, 0, "FORM_PAY");
            _daProject_Transfer.UpdateCommand.Parameters.Add("DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID");
            _daProject_Transfer.UpdateCommand.Parameters.Add("PROBA_PERIOD", OracleDbType.Decimal, 0, "PROBA_PERIOD");
            _daProject_Transfer.UpdateCommand.Parameters.Add("FORM_OPERATION_ID", OracleDbType.Decimal, 0, "FORM_OPERATION_ID");
            _daProject_Transfer.UpdateCommand.Parameters.Add("HARMFUL_VAC", OracleDbType.Decimal, 0, "HARMFUL_VAC");
            _daProject_Transfer.UpdateCommand.Parameters.Add("ADDITIONAL_VAC", OracleDbType.Decimal, 0, "ADDITIONAL_VAC");
            _daProject_Transfer.UpdateCommand.Parameters.Add("HARMFUL_ADDITION_ADD", OracleDbType.Decimal, 0, "HARMFUL_ADDITION_ADD");
            _daProject_Transfer.UpdateCommand.Parameters.Add("COMB_ADDITION", OracleDbType.Decimal, 0, "COMB_ADDITION");
            _daProject_Transfer.UpdateCommand.Parameters.Add("COMB_ADDITION_NOTE", OracleDbType.Varchar2, 0, "COMB_ADDITION_NOTE");
            _daProject_Transfer.UpdateCommand.Parameters.Add("SALARY", OracleDbType.Decimal, 0, "SALARY");
            _daProject_Transfer.UpdateCommand.Parameters.Add("CLASSIFIC", OracleDbType.Decimal, 0, "CLASSIFIC").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Transfer.UpdateCommand.Parameters["CLASSIFIC"].DbType = DbType.Decimal;
            _daProject_Transfer.UpdateCommand.Parameters.Add("TARIFF_GRID_ID", OracleDbType.Decimal, 0, "TARIFF_GRID_ID").Direction =
                ParameterDirection.InputOutput;
            _daProject_Transfer.UpdateCommand.Parameters["TARIFF_GRID_ID"].DbType = DbType.Decimal;
            _daProject_Transfer.UpdateCommand.Parameters.Add("SECRET_ADDITION", OracleDbType.Decimal, 0, "SECRET_ADDITION");
            _daProject_Transfer.UpdateCommand.Parameters.Add("SECRET_ADDITION_NOTE", OracleDbType.Varchar2, 0, "SECRET_ADDITION_NOTE");
            _daProject_Transfer.UpdateCommand.Parameters.Add("SIGN_MAT_RESP_CONTR", OracleDbType.Decimal, 0, "SIGN_MAT_RESP_CONTR");
            _daProject_Transfer.UpdateCommand.Parameters.Add("OTHER_LIABILITY_BOSS", OracleDbType.Varchar2, 0, "OTHER_LIABILITY_BOSS");
            _daProject_Transfer.UpdateCommand.Parameters.Add("WORKING_TIME_ID", OracleDbType.Decimal, 0, "WORKING_TIME_ID");
            _daProject_Transfer.UpdateCommand.Parameters.Add("WORKING_TIME_COMMENT", OracleDbType.Varchar2, 0, "WORKING_TIME_COMMENT");

            new OracleDataAdapter(string.Format(@"select FORM_PAY_ON_DEGREE_ID, DEGREE_ID, FORM_PAY, NAME_FORM_PAY from {0}.FORM_PAY_ON_DEGREE FPD
                join {0}.FORM_PAY FP on(FPD.FORM_PAY_ID=FP.FORM_PAY)",
                Connect.Schema), Connect.CurConnect).Fill(_dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"]);

            // Term_Transfer
            _daProject_Term_Transfer.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectProject_Term_Transfer.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Term_Transfer.SelectCommand.BindByName = true;
            _daProject_Term_Transfer.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            _daProject_Term_Transfer.SelectCommand.Parameters.Add("p_TYPE_TERM", OracleDbType.Int16);
            // Insert, Update
            _daProject_Term_Transfer.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_TERM_TRANSFER_UPDATE(:PROJECT_TERM_TRANSFER_ID,:PROJECT_TRANSFER_ID,:TYPE_TERM_TRANSFER_ID,:DATE_TERM_TRANSFER,:BASE_TERM_TRANSFER_ID,:TYPE_TERM);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Term_Transfer.UpdateCommand.BindByName = true;
            _daProject_Term_Transfer.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("PROJECT_TERM_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TERM_TRANSFER_ID");
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("TYPE_TERM_TRANSFER_ID", OracleDbType.Decimal, 0, "TYPE_TERM_TRANSFER_ID");
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("DATE_TERM_TRANSFER", OracleDbType.Date, 0, "DATE_TERM_TRANSFER");
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("BASE_TERM_TRANSFER_ID", OracleDbType.Decimal, 0, "BASE_TERM_TRANSFER_ID");
            _daProject_Term_Transfer.UpdateCommand.Parameters.Add("TYPE_TERM", OracleDbType.Decimal, 0, "TYPE_TERM");

            // List_Repl_Emp
            _daProject_List_Repl_Emp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectProject_List_Repl_Emp.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_List_Repl_Emp.SelectCommand.BindByName = true;
            _daProject_List_Repl_Emp.SelectCommand.Parameters.Add("p_PROJECT_TERM_TRANSFER_ID", OracleDbType.Decimal);
            // Insert, Update
            _daProject_List_Repl_Emp.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_LIST_REPL_EMP_UPDATE(:PROJECT_LIST_REPL_EMP_ID,:PROJECT_TERM_TRANSFER_ID,:TRANSFER_ID,:ORDER_REPL_EMP);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_List_Repl_Emp.InsertCommand.BindByName = true;
            _daProject_List_Repl_Emp.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_List_Repl_Emp.InsertCommand.Parameters.Add("PROJECT_LIST_REPL_EMP_ID", OracleDbType.Decimal, 0, "PROJECT_LIST_REPL_EMP_ID").Direction =
                ParameterDirection.InputOutput;
            _daProject_List_Repl_Emp.InsertCommand.Parameters["PROJECT_LIST_REPL_EMP_ID"].DbType = DbType.Decimal;
            _daProject_List_Repl_Emp.InsertCommand.Parameters.Add("PROJECT_TERM_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TERM_TRANSFER_ID");
            _daProject_List_Repl_Emp.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daProject_List_Repl_Emp.InsertCommand.Parameters.Add("ORDER_REPL_EMP", OracleDbType.Decimal, 0, "ORDER_REPL_EMP");
            _daProject_List_Repl_Emp.UpdateCommand = _daProject_List_Repl_Emp.InsertCommand;
            // Delete
            _daProject_List_Repl_Emp.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_LIST_REPL_EMP_DELETE(:PROJECT_LIST_REPL_EMP_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_List_Repl_Emp.DeleteCommand.BindByName = true;
            _daProject_List_Repl_Emp.DeleteCommand.Parameters.Add("PROJECT_LIST_REPL_EMP_ID", OracleDbType.Decimal, 0, "PROJECT_LIST_REPL_EMP_ID");

            // Project_Working_Conditions
            _daProject_Working_Conditions.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("TP/SelectProject_Working_Conditions.sql"), Connect.Schema), Connect.CurConnect);
            _daProject_Working_Conditions.SelectCommand.BindByName = true;
            _daProject_Working_Conditions.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            // Insert, Update
            _daProject_Working_Conditions.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_WORK_CONDITIONS_UPDATE(:PROJECT_WORK_CONDITIONS_ID,:PROJECT_TRANSFER_ID,:TYPE_CONDITION_ID,:CONDITIONS_OF_WORK_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Working_Conditions.UpdateCommand.BindByName = true;
            _daProject_Working_Conditions.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Working_Conditions.UpdateCommand.Parameters.Add("PROJECT_WORK_CONDITIONS_ID", OracleDbType.Decimal, 0, "PROJECT_WORK_CONDITIONS_ID").Direction =
                ParameterDirection.InputOutput;
            _daProject_Working_Conditions.UpdateCommand.Parameters["PROJECT_WORK_CONDITIONS_ID"].DbType = DbType.Decimal;
            _daProject_Working_Conditions.UpdateCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daProject_Working_Conditions.UpdateCommand.Parameters.Add("TYPE_CONDITION_ID", OracleDbType.Decimal, 0, "TYPE_CONDITION_ID");
            _daProject_Working_Conditions.UpdateCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");

            // Select
            _daProject_Add_Condition.SelectCommand = new OracleCommand(string.Format(
                @"select PROJECT_TRANSFER_ID, SIGN_MILK, SIGN_PREVENTIVE_NUTRITION, SIGN_PREFERENTIAL_PENS, PRIVILEGED_POSITION_ID 
                from {0}.PROJECT_ADD_CONDITION where PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _daProject_Add_Condition.SelectCommand.BindByName = true;
            _daProject_Add_Condition.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            // Update
            _daProject_Add_Condition.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_ADD_CONDITION_UPDATE(:PROJECT_TRANSFER_ID,:SIGN_MILK,:SIGN_PREVENTIVE_NUTRITION,:SIGN_PREFERENTIAL_PENS,:PRIVILEGED_POSITION_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Add_Condition.UpdateCommand.BindByName = true;
            _daProject_Add_Condition.UpdateCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daProject_Add_Condition.UpdateCommand.Parameters.Add("SIGN_MILK", OracleDbType.Decimal, 0, "SIGN_MILK");
            _daProject_Add_Condition.UpdateCommand.Parameters.Add("SIGN_PREVENTIVE_NUTRITION", OracleDbType.Decimal, 0, "SIGN_PREVENTIVE_NUTRITION");
            _daProject_Add_Condition.UpdateCommand.Parameters.Add("SIGN_PREFERENTIAL_PENS", OracleDbType.Decimal, 0, "SIGN_PREFERENTIAL_PENS");
            _daProject_Add_Condition.UpdateCommand.Parameters.Add("PRIVILEGED_POSITION_ID", OracleDbType.Decimal, 0, "PRIVILEGED_POSITION_ID");
            #endregion

            // Select
            _daProject_Approval.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectProject_Approval.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Approval.SelectCommand.BindByName = true;
            _daProject_Approval.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            // Insert
            _daProject_Approval.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_APPROVAL_UPDATE(:PROJECT_APPROVAL_ID,:PROJECT_TRANSFER_ID,:PROJECT_PLAN_APPROVAL_ID,:DATE_APPROVAL,:NOTE_APPROVAL,
                        :USER_NAME,:TYPE_APPROVAL_ID,:USER_FIO);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Approval.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Approval.InsertCommand.BindByName = true;
            _daProject_Approval.InsertCommand.Parameters.Add("PROJECT_APPROVAL_ID", OracleDbType.Decimal, 0, "PROJECT_APPROVAL_ID").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["PROJECT_APPROVAL_ID"].DbType = DbType.Decimal;
            _daProject_Approval.InsertCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            _daProject_Approval.InsertCommand.Parameters.Add("PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal, 0, "PROJECT_PLAN_APPROVAL_ID").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["PROJECT_PLAN_APPROVAL_ID"].DbType = DbType.Decimal;
            _daProject_Approval.InsertCommand.Parameters.Add("DATE_APPROVAL", OracleDbType.Date, 0, "DATE_APPROVAL").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["DATE_APPROVAL"].DbType = DbType.DateTime;
            _daProject_Approval.InsertCommand.Parameters.Add("NOTE_APPROVAL", OracleDbType.Varchar2, 0, "NOTE_APPROVAL");
            _daProject_Approval.InsertCommand.Parameters.Add("USER_NAME", OracleDbType.Varchar2, 30, "USER_NAME").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["USER_NAME"].DbType = DbType.String;
            _daProject_Approval.InsertCommand.Parameters.Add("TYPE_APPROVAL_ID", OracleDbType.Decimal, 0, "TYPE_APPROVAL_ID");
            _daProject_Approval.InsertCommand.Parameters.Add("USER_FIO", OracleDbType.Varchar2, 100, "USER_FIO").Direction =
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["USER_FIO"].DbType = DbType.String;

            // Select
            _daType_Approval.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectType_Approval_By_Project.sql"),
                Connect.Schema), Connect.CurConnect);
            _daType_Approval.SelectCommand.BindByName = true;
            _daType_Approval.SelectCommand.Parameters.Add("p_PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal);
            _daType_Approval.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);

            _daPlan_Approval.SelectCommand = new OracleCommand(string.Format(
                @"select PROJECT_PLAN_APPROVAL_ID,PROJECT_PLAN_APPROVAL_ID_PRIOR from {0}.PROJECT_PLAN_APPROVAL
                where PROJECT_PLAN_APPROVAL_ID=:p_PROJECT_PLAN_APPROVAL_ID",
                Connect.Schema), Connect.CurConnect);
            _daPlan_Approval.SelectCommand.BindByName = true;
            _daPlan_Approval.SelectCommand.Parameters.Add("p_PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal);

            _ocGet_Status_Project = new OracleCommand(string.Format(
                @"SELECT NVL((select NVL(PROJECT_PLAN_APPROVAL_ID,0) from {0}.PROJECT_TRANSFER 
                                WHERE PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID),0) from dual",
                Connect.Schema), Connect.CurConnect);
            _ocGet_Status_Project.BindByName = true;
            _ocGet_Status_Project.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);

            _ocGet_Sign_Open_Approval = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectSign_Open_Approval.sql"),
                Connect.Schema), Connect.CurConnect);
            _ocGet_Sign_Open_Approval.BindByName = true;
            _ocGet_Sign_Open_Approval.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);

            _ocPhotoEmp = new OracleCommand(string.Format(
                "begin SELECT (select E.PHOTO from {0}.EMP E where PER_NUM = :p_PER_NUM) into :p_PHOTO from dual; end;", 
                Connect.Schema), Connect.CurConnect);
            _ocPhotoEmp.BindByName = true;
            _ocPhotoEmp.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _ocPhotoEmp.Parameters.Add("p_PHOTO", OracleDbType.Blob).Direction = ParameterDirection.Output;

            _ocUpdate_Order = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_TRANSFER_ORDER(:p_PROJECT_TRANSFER_ID, :p_PER_NUM, :p_DATE_TRANSFER, :p_TR_NUM_ORDER,:p_TR_DATE_ORDER,:p_CONTR_EMP,:p_DATE_CONTR,:p_CHAN_SIGN);
                END;", Connect.Schema), Connect.CurConnect);
            _ocUpdate_Order.BindByName = true;
            _ocUpdate_Order.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            _ocUpdate_Order.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2, 10).Direction = ParameterDirection.InputOutput;
            _ocUpdate_Order.Parameters.Add("p_DATE_TRANSFER", OracleDbType.Date);
            _ocUpdate_Order.Parameters.Add("p_TR_NUM_ORDER", OracleDbType.Varchar2, 6).Direction = ParameterDirection.InputOutput;
            _ocUpdate_Order.Parameters.Add("p_TR_DATE_ORDER", OracleDbType.Date);
            _ocUpdate_Order.Parameters.Add("p_CONTR_EMP", OracleDbType.Varchar2, 6).Direction = ParameterDirection.InputOutput;
            _ocUpdate_Order.Parameters.Add("p_DATE_CONTR", OracleDbType.Date);
            _ocUpdate_Order.Parameters.Add("p_CHAN_SIGN", OracleDbType.Int16);

            // Формирование перевода в основной базе
            _ocProject_To_Transfer = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_TO_TRANSFER(:p_PROJECT_TRANSFER_ID,:p_TRANSFER_ID,:p_PERCO_SYNC_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocProject_To_Transfer.BindByName = true;
            _ocProject_To_Transfer.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            _ocProject_To_Transfer.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            _ocProject_To_Transfer.Parameters.Add("p_PERCO_SYNC_ID", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;

            // Установка признака регистрации проекта в Штатном расписании
            _ocRegistration_Project = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_REGISTRATION(:p_PROJECT_TRANSFER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocRegistration_Project.BindByName = true;
            _ocRegistration_Project.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);

            // Установка признака аннулирования проекта
            _ocAnnul_Project = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_ANNUL(:p_PROJECT_TRANSFER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocAnnul_Project.BindByName = true;
            _ocAnnul_Project.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);

            // Приложения к проекту перевода
            _daAppendix.SelectCommand = new OracleCommand(string.Format(
                @"SELECT PROJECT_APPENDIX_ID, NOTE_DOCUMENT, PROJECT_TRANSFER_ID 
                FROM {0}.PROJECT_APPENDIX WHERE PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _daAppendix.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            // Insert
            _daAppendix.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_APPENDIX_UPDATE(:PROJECT_APPENDIX_ID,:NOTE_DOCUMENT,:DOCUMENT,:PROJECT_TRANSFER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.InsertCommand.BindByName = true;
            _daAppendix.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daAppendix.InsertCommand.Parameters.Add("PROJECT_APPENDIX_ID", OracleDbType.Decimal, 0, "PROJECT_APPENDIX_ID").Direction = 
                ParameterDirection.InputOutput;
            _daAppendix.InsertCommand.Parameters["PROJECT_APPENDIX_ID"].DbType = DbType.Decimal;
            _daAppendix.InsertCommand.Parameters.Add("NOTE_DOCUMENT", OracleDbType.Varchar2, 0, "NOTE_DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("DOCUMENT", OracleDbType.Blob, 0, "DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
            // Update
            _daAppendix.UpdateCommand = _daAppendix.InsertCommand;
            // Delete
            _daAppendix.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_APPENDIX_DELETE(:PROJECT_APPENDIX_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.DeleteCommand.BindByName = true;
            _daAppendix.DeleteCommand.Parameters.Add("PROJECT_APPENDIX_ID", OracleDbType.Decimal, 0, "PROJECT_APPENDIX_ID");            
        }
        
        private void SaveEmp_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            /*try
            {
                bool f1 = registr.IsDataChanged();
            }
            catch { }
            try
            {
                bool f1 = habit.IsDataChanged();
            }
            catch { }*/
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && this.DataContext != null && 
                (((DataRowView)this.DataContext).DataView.Table.GetChanges() != null ||
                (_dsDataEmp != null && _dsDataEmp.HasChanges()) ||
                (registr != null && registr.IsDataChanged() == true) || (habit != null && habit.IsDataChanged() == true)) &&
                (((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value /*&&
                ((DataRowView)this.DataContext)["PROJECT_PLAN_APPROVAL_ID"].ToString() != "-1" &&
                _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"] != DBNull.Value*/
                ))
            {
                e.CanExecute = Array.TrueForAll<DependencyObject>(grPerson_Data.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
            }
        }
        
        private void SaveEmp_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Transfer_Project_Viewer.SaveProject())
            {
                SaveEmp();
                SaveTransfer_QM();
            }
        }

        void SaveEmp()
        {
            //_dsDataEmp.Tables["REGISTR"].Rows[0]["REG_CODE_STREET"] = ((REGISTR_obj)registr[0]).REG_CODE_STREET;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_CODE_STREET"] = formregistr.cbStreet.SelectedValue;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_HOUSE"] = ((REGISTR_obj)registr[0]).REG_HOUSE;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_BULK"] = ((REGISTR_obj)registr[0]).REG_BULK;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_FLAT"] = ((REGISTR_obj)registr[0]).REG_FLAT;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_POST_CODE"] = ((REGISTR_obj)registr[0]).REG_POST_CODE;
            if (((REGISTR_obj)registr[0]).DATE_REG == null)
                _dsDataEmp.Tables["REGISTR"].Rows[0]["DATE_REG"] = DBNull.Value;
            else
                _dsDataEmp.Tables["REGISTR"].Rows[0]["DATE_REG"] = ((REGISTR_obj)registr[0]).DATE_REG;
            _dsDataEmp.Tables["REGISTR"].Rows[0]["REG_PHONE"] = ((REGISTR_obj)registr[0]).REG_PHONE;

            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_CODE_STREET"] = ((HABIT_obj)habit[0]).HAB_CODE_STREET;
            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_HOUSE"] = ((HABIT_obj)habit[0]).HAB_HOUSE;
            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_BULK"] = ((HABIT_obj)habit[0]).HAB_BULK;
            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_FLAT"] = ((HABIT_obj)habit[0]).HAB_FLAT;
            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_POST_CODE"] = ((HABIT_obj)habit[0]).HAB_POST_CODE;
            _dsDataEmp.Tables["HABIT"].Rows[0]["HAB_PHONE"] = ((HABIT_obj)habit[0]).HAB_PHONE;

            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daPer_Data.InsertCommand.Transaction = transact;
                _daPer_Data.UpdateCommand.Transaction = transact;
                _daPer_Data.Update(_dsDataEmp.Tables["PER_DATA"]);
                _daPassport.InsertCommand.Transaction = transact;
                _daPassport.UpdateCommand.Transaction = transact;
                _daPassport.Update(_dsDataEmp.Tables["PASSPORT"]);
                _daMil_Card.InsertCommand.Transaction = transact;
                _daMil_Card.UpdateCommand.Transaction = transact;
                _daMil_Card.Update(_dsDataEmp.Tables["MIL_CARD"]);
                _daRegistr.InsertCommand.Transaction = transact;
                _daRegistr.UpdateCommand.Transaction = transact;
                _daRegistr.Update(_dsDataEmp.Tables["REGISTR"]);
                _daHabit.InsertCommand.Transaction = transact;
                _daHabit.UpdateCommand.Transaction = transact;
                _daHabit.Update(_dsDataEmp.Tables["HABIT"]);
                transact.Commit();
                registr.Clear();
                registr.Fill(string.Format("where PER_NUM = '{0}'", ((DataRowView)this.DataContext)["PER_NUM"]));
                habit.Clear();
                habit.Fill(string.Format("where PER_NUM = '{0}'", ((DataRowView)this.DataContext)["PER_NUM"]));
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }
        
        void SaveTransfer_QM()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                OracleCommand _ocUpdateTransfer_QM = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TRANSFER_QM_UPDATE(:TRANSFER_ID,:PROJECT_TRANSFER_ID,:p_PROJECT_STATEMENT_ID,:SIGN_TRANSFER_QM);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocUpdateTransfer_QM.BindByName = true;
                _ocUpdateTransfer_QM.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
                _ocUpdateTransfer_QM.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID").Value =
                    ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                _ocUpdateTransfer_QM.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
                _ocUpdateTransfer_QM.Parameters.Add("SIGN_TRANSFER_QM", OracleDbType.Decimal, 0, "SIGN_TRANSFER_QM").Value =
                    ((DataRowView)this.DataContext)["SIGN_TRANSFER_QM"];
                _ocUpdateTransfer_QM.Transaction = transact;
                _ocUpdateTransfer_QM.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show("Ошибка сохранения признака УТК!\n\n" + ex.Message, 
                    "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void SaveTransfer_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && _dsDataTransfer.HasChanges() &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value /*&&
                ((DataRowView)this.DataContext)["PROJECT_PLAN_APPROVAL_ID"].ToString() != "-1"*/ &&
                _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"] != DBNull.Value)
            {
                e.CanExecute = 
                    Array.TrueForAll<DependencyObject>(gridBaseInfo.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false) &&
                    (chSIGN_PREFERENTIAL_PENS.IsChecked == false ||
                        (chSIGN_PREFERENTIAL_PENS.IsChecked == true && Array.TrueForAll<DependencyObject>(dpPriv_Pos.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false)));
            }
        }

        private void SaveTransfer_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SaveTransfer_Project())
            {
                GetStatusProject();
                SaveTerm_Transfer();
                SaveWork_Condition();
                SaveAdd_Condition();
            }
        }
        
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        
        private void TabItem_DragEnter()
        {
            // Таблицы для адреса места прописки и проживания   
            if (!f_LoadAddress)
            {
                // Инициализация таблиц и заполнение их данными
                rregion = new REGION_seq(Connect.CurConnect);
                rregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION));
                rdistrict = new DISTRICT_seq(Connect.CurConnect);
                rcity = new CITY_seq(Connect.CurConnect);
                rlocality = new LOCALITY_seq(Connect.CurConnect);
                rstreet = new STREET_seq(Connect.CurConnect);
                // Создание и настройка формы для работы с местом прописки
                REGISTR_obj r_registr = (REGISTR_obj)registr[0];
                formregistr = new Address((object)r_registr, typeof(REGISTR_seq), rregion, rdistrict, rcity, rlocality, rstreet);
                formregistr.TopLevel = false;
                formregistr.Dock = DockStyle.Fill;
                formregistr.cbRegion.SelectedItem = null;
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.EnabledComboBox);
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.cbRegion_SelectedIndexChanged);
                formregistr.tbHouse.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_HOUSE);
                formregistr.tbBulk.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_BULK);
                formregistr.tbFlat.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_FLAT);
                formregistr.tbPhone.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_PHONE);
                formregistr.tbPost_Code.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_POST_CODE);
                //formregistr.mbDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                formregistr.deDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                whRegistr.Child = formregistr;
                System.Windows.Forms.Button bt = new System.Windows.Forms.Button();
                bt.Name = "btFromRegistrToHabit";
                bt.Location = new System.Drawing.Point(24, 283);
                bt.Size = new System.Drawing.Size(335, 27);
                bt.Font = new Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold);
                bt.Text = "Скопировать в адрес проживания";
                bt.ForeColor = System.Drawing.Color.FromArgb(0, 70, 213);
                bt.Enabled = false;
                bt.Click += new EventHandler(bt_Click);
                formregistr.Controls.Add(bt);
                string stregistr = r_registr.REG_CODE_STREET;
                if (stregistr != null && stregistr != "")
                {
                    formregistr.LoadAddress(stregistr);
                }
                formregistr.Show();
                formregistr.DisableAll(true, System.Drawing.Color.White);

                // Инициализация таблиц и заполнение их данными
                hregion = new REGION_seq(Connect.CurConnect);
                hregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION));
                hdistrict = new DISTRICT_seq(Connect.CurConnect);
                hcity = new CITY_seq(Connect.CurConnect);
                hlocality = new LOCALITY_seq(Connect.CurConnect);
                hstreet = new STREET_seq(Connect.CurConnect);
                // Создание и настройка формы для работы с местом прописки
                HABIT_obj r_habit = (HABIT_obj)habit[0];
                formhabit = new Address((object)r_habit, typeof(HABIT_seq), hregion, hdistrict, hcity, hlocality, hstreet);
                formhabit.TopLevel = false;
                formhabit.Dock = DockStyle.Fill;
                formhabit.cbRegion.SelectedItem = null;
                formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.EnabledComboBox);
                formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.cbRegion_SelectedIndexChanged);
                formhabit.tbHouse.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_HOUSE);
                formhabit.tbBulk.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_BULK);
                formhabit.tbFlat.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_FLAT);
                formhabit.tbPhone.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_PHONE);
                formhabit.tbPost_Code.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_POST_CODE);
                formhabit.deDate_Reg.Visible = false;
                formhabit.tbPhone.Location = formhabit.mbDate_Reg.Location;
                formhabit.lbDate.Visible = false;
                formhabit.lbPhone.Location = new System.Drawing.Point(24, 240);

                Address_None_Kladr.Per_num = r_habit.PER_NUM;
                tbHab_Non_Kladr_Address.Text = Address_None_Kladr.Address_None_Kladr_Property;

                whHabit.Child = formhabit;
                string sthabit = r_habit.HAB_CODE_STREET;
                if (sthabit != null && sthabit != "")
                {
                    formhabit.LoadAddress(sthabit);
                }
                formhabit.Show();
                formhabit.DisableAll(true, System.Drawing.Color.White);

                f_LoadAddress = true;
            }
        }
        
        void bt_Click(object sender, EventArgs e)
        {
            if (formregistr.cbStreet.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Вы не ввели адрес прописки для копирования", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                if (System.Windows.MessageBox.Show("Вы действительно хотите скопировать адрес прописки\nв адрес проживания?\nЭто займет некоторое время.", "АСУ \"Кадры\"", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //string st = registr[0].REG_CODE_STREET;

                    string st = formregistr.cbStreet.SelectedValue.ToString();

                    habit[0].HAB_CODE_STREET = st;
                    formhabit.LoadAddress(st);
                    formhabit.tbHouse.Text = registr[0].REG_HOUSE;
                    formhabit.tbBulk.Text = registr[0].REG_BULK;
                    formhabit.tbFlat.Text = registr[0].REG_FLAT;
                    formhabit.tbPost_Code.Text = registr[0].REG_POST_CODE;
                    formhabit.tbPhone.Text = registr[0].REG_PHONE;
                    formhabit.tbHouse.Enabled = true;
                    formhabit.tbBulk.Enabled = true;
                    formhabit.tbFlat.Enabled = true;
                    formhabit.tbPost_Code.Enabled = true;
                    formhabit.tbPhone.Enabled = true;
                }
            }
        }

        private void AddEdu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentEdu = _dsDataEmp.Tables["EDU"].DefaultView.AddNew();
            _currentEdu["MAIN_PROF"] = 0;
            _currentEdu["FROM_FACT"] = 0;
            _dsDataEmp.Tables["EDU"].Rows.Add(_currentEdu.Row);
            dgEdu.SelectedItem = _currentEdu;

            Edu_Editor edu = new Edu_Editor(_currentEdu, _dsDataEmp);
            edu.Owner = Window.GetWindow(this);
            if (edu.ShowDialog() == true)
            {
                SaveEmp();
                SaveEdu();
            }
            else
            {
                _dsDataEmp.Tables["EDU"].RejectChanges();
            }
        }

        private void EditEdu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgEdu != null && dgEdu.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgEdu.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();
            
            Edu_Editor edu = new Edu_Editor(rowSelected, _dsDataEmp);
            edu.Owner = Window.GetWindow(this);
            if (edu.ShowDialog() == true)
            {
                SaveEdu();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            } 
        }

        private void DeleteEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgEdu.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgEdu.SelectedCells[0].Item).Delete();
                }
                SaveEdu();
            }
            dgEdu.Focus();
        }

        void SaveEdu()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _dsDataEmp.Tables["EDU"].DefaultView.RowStateFilter;
                _dsDataEmp.Tables["EDU"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _dsDataEmp.Tables["EDU"].DefaultView.Count; ++i)
                {
                    _dsDataEmp.Tables["EDU"].DefaultView[i]["EDU_ID"] =
                        new OracleCommand(string.Format("select {0}.EDU_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                    _dsDataEmp.Tables["EDU"].DefaultView[i]["PER_NUM"] =
                        ((DataRowView)this.DataContext)["PER_NUM"];
                }
                _dsDataEmp.Tables["EDU"].DefaultView.RowStateFilter = rs;
                _daEdu.InsertCommand.Transaction = transact;
                _daEdu.UpdateCommand.Transaction = transact;
                _daEdu.DeleteCommand.Transaction = transact;
                _daEdu.Update(_dsDataEmp.Tables["EDU"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                _dsDataEmp.Tables["EDU"].RejectChanges();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void cbType_Per_Doc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mbSeria_Passport.Mask = (cbType_Per_Doc.SelectedItem as DataRowView)["TEMPL_SER"].ToString();
            mbNum_Passport.Mask = (cbType_Per_Doc.SelectedItem as DataRowView)["TEMPL_NUM"].ToString();
        }
        
        private void cbDegree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                if (((DataRowView)tcTransfer_Project.DataContext)["DEGREE_ID"] != DBNull.Value)
                    _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "DEGREE_ID = " +
                        ((DataRowView)tcTransfer_Project.DataContext)["DEGREE_ID"];
                else
                    _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "1 = 2";
            }
        }
        
        bool SaveTransfer_Project()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daProject_Transfer.UpdateCommand.Transaction = transact;
                _daProject_Transfer.Update(_dsDataTransfer.Tables["PROJECT_TRANSFER"]);
                transact.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения проекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        void SaveTerm_Transfer()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                // Если тип срока договора не выбран, то удаляем все данные
                if (cbType_Term_Contr_Emp.SelectedItem == null)
                {
                    // Если тип договора не выбран, убираем все данные по сроку договора
                    _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["DATE_TERM_TRANSFER"] = DBNull.Value;
                    _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["BASE_TERM_TRANSFER_ID"] = DBNull.Value;
                    while (_dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView.Count > 0)
                    {
                        _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView.Delete(0);
                    }
                }
                else
                {
                    // Если тип срока договора не подразумевает установку даты и основания - убираем их принудительно  
                    if (((DataRowView)cbType_Term_Contr_Emp.SelectedItem)["SIGN_BASE_TEMP_TRANSFER"].ToString() != "1")
                    {
                        _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["DATE_TERM_TRANSFER"] = DBNull.Value;
                        _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["BASE_TERM_TRANSFER_ID"] = DBNull.Value;
                    }
                    // Если тип срока перевода не подразумевает ввод замещаемых работников - убираем их принудительно  
                    if (((DataRowView)cbType_Term_Contr_Emp.SelectedItem)["SIGN_REPL_EMP"].ToString() != "1")
                    {
                        while (_dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView.Count > 0)
                        {
                            _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView.Delete(0);
                        }
                    }
                }
                _daProject_Term_Transfer.UpdateCommand.Transaction = transact;
                _daProject_Term_Transfer.Update(_dsDataTransfer.Tables["PROJECT_TERM_CONTR"]);
                transact.Commit();
                SaveList_Repl_Contr();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения срока договора", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            transact = Connect.CurConnect.BeginTransaction();
            try
            {
                // Сохраняем изменения в сроке перевода только для переводов
                if (((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"].ToString() == "2")
                {
                    // Если тип срока договора не выбран, то удаляем все данные
                    if (cbType_Term_Transfer.SelectedItem == null)
                    {
                        // Если тип договора не выбран, убираем все данные по сроку договора
                        _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["DATE_TERM_TRANSFER"] = DBNull.Value;
                        _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["BASE_TERM_TRANSFER_ID"] = DBNull.Value;
                        while (_dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView.Count > 0)
                        {
                            _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView.Delete(0);
                        }
                    }
                    else
                    {
                        // Если тип срока договора не подразумевает установку даты и основания - убираем их принудительно  
                        if (((DataRowView)cbType_Term_Transfer.SelectedItem)["SIGN_BASE_TEMP_TRANSFER"].ToString() != "1")
                        {
                            _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["DATE_TERM_TRANSFER"] = DBNull.Value;
                            _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["BASE_TERM_TRANSFER_ID"] = DBNull.Value;
                        }
                        // Если тип срока перевода не подразумевает ввод замещаемых работников - убираем их принудительно  
                        if (((DataRowView)cbType_Term_Transfer.SelectedItem)["SIGN_REPL_EMP"].ToString() != "1")
                        {
                            while (_dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView.Count > 0)
                            {
                                _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView.Delete(0);
                            }
                        }
                    }
                    _daProject_Term_Transfer.UpdateCommand.Transaction = transact;
                    _daProject_Term_Transfer.Update(_dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"]);
                    transact.Commit();
                    SaveList_Repl_Emp();
                }
                else
                    transact.Rollback();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения срока перевода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();             
        }

        void SaveList_Repl_Emp()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daProject_List_Repl_Emp.InsertCommand.Transaction = transact;
                _daProject_List_Repl_Emp.UpdateCommand.Transaction = transact;
                _daProject_List_Repl_Emp.DeleteCommand.Transaction = transact;
                _daProject_List_Repl_Emp.Update(_dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения замещаемых лиц", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        void SaveList_Repl_Contr()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daProject_List_Repl_Emp.InsertCommand.Transaction = transact;
                _daProject_List_Repl_Emp.UpdateCommand.Transaction = transact;
                _daProject_List_Repl_Emp.DeleteCommand.Transaction = transact;
                _daProject_List_Repl_Emp.Update(_dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения замещаемых лиц", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        void SaveAdd_Condition()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                // Если признак льготного пенсионного обеспечения не установлен - обнуляем льготную профессию
                if (chSIGN_PREFERENTIAL_PENS.IsChecked == false)
                {
                    _dsDataTransfer.Tables["PROJECT_ADD_CONDITION"].DefaultView[0]["PRIVILEGED_POSITION_ID"] = DBNull.Value;                    
                }
                _daProject_Add_Condition.UpdateCommand.Transaction = transact;
                _daProject_Add_Condition.Update(_dsDataTransfer.Tables["PROJECT_ADD_CONDITION"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения доп. условий труда", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        void SaveWork_Condition()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView.RowStateFilter;
                _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
                for (int i = 0; i < _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView.Count; ++i)
                {
                    _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView[i]["PROJECT_TRANSFER_ID"] =
                        ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                }
                _dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"].DefaultView.RowStateFilter = rs;
                _daProject_Working_Conditions.UpdateCommand.Transaction = transact;
                _daProject_Working_Conditions.Update(_dsDataTransfer.Tables["PROJECT_WORK_CONDITIONS"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения условий труда", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Edit_List_Repl_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgList_Repl_Emp != null && dgList_Repl_Emp.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void Add_List_Repl_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRepl = _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].DefaultView.AddNew();
            _dsDataTransfer.Tables["PROJECT_LIST_REPL_EMP"].Rows.Add(_currentRepl.Row);
            dgList_Repl_Emp.SelectedItem = _currentRepl;

            Find_Emp findEmp = new Find_Emp(
                ((DataRowView)this.DataContext)["DATE_TRANSFER"] != DBNull.Value ?
                Convert.ToDateTime(((DataRowView)this.DataContext)["DATE_TRANSFER"]) :
                DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                _currentRepl["PROJECT_TERM_TRANSFER_ID"] = _dsDataTransfer.Tables["PROJECT_TERM_TRANSFER"].DefaultView[0]["PROJECT_TERM_TRANSFER_ID"];
                _currentRepl["TRANSFER_ID"] = findEmp.Transfer_ID;
                _currentRepl["PER_NUM"] = findEmp.Per_Num;
                _currentRepl["EMP_LAST_NAME"] = findEmp.Last_Name;
                _currentRepl["EMP_FIRST_NAME"] = findEmp.First_Name;
                _currentRepl["EMP_MIDDLE_NAME"] = findEmp.Middle_Name;
                //_currentRepl.Row.AcceptChanges();
            }
            else
            {
                _currentRepl.Row.RejectChanges();
            }
        }

        private void Edit_List_Repl_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgList_Repl_Emp.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            Find_Emp findEmp = new Find_Emp(
                ((DataRowView)this.DataContext)["DATE_TRANSFER"] != DBNull.Value ?
                Convert.ToDateTime(((DataRowView)this.DataContext)["DATE_TRANSFER"]) :
                DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                rowSelected["TRANSFER_ID"] = findEmp.Transfer_ID;
                rowSelected["PER_NUM"] = findEmp.Per_Num;
                rowSelected["EMP_LAST_NAME"] = findEmp.Last_Name;
                rowSelected["EMP_FIRST_NAME"] = findEmp.First_Name;
                rowSelected["EMP_MIDDLE_NAME"] = findEmp.Middle_Name;
                //rowSelected.Row.AcceptChanges();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            } 
        }

        private void Delete_List_Repl_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgList_Repl_Emp.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgList_Repl_Emp.SelectedCells[0].Item).Delete();
                }
            }
            dgList_Repl_Emp.Focus();
        }

        private void dgWork_Conditions_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((System.Windows.Controls.DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((System.Windows.Controls.DataGrid)sender).BeginEdit();
        }

        private void cbSubdiv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                AppDataSet.Tables["PRIVILEGED_POSITION"].DefaultView.RowFilter = 
                    string.Format("SUBDIV_ID = {0} and POS_ID = {1}",
                        ((DataRowView)this.DataContext)["SUBDIV_ID"] == DBNull.Value ? "0" : ((DataRowView)this.DataContext)["SUBDIV_ID"].ToString(),
                        ((DataRowView)tcTransfer_Project.DataContext)["POS_ID"] == DBNull.Value ? "0" : ((DataRowView)tcTransfer_Project.DataContext)["POS_ID"].ToString());
                ((DataRowView)this.DataContext)["CODE_SUBDIV"] = ((DataRowView)cbSubdiv.SelectedItem)["CODE_SUBDIV"];
            }
        }

        private void _this_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _dsAppendix.Tables["PROJECT_APPENDIX"].Clear();
            this.DataContext = null;
        }

        private void Edit_List_Repl_Contr_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgList_Repl_Contr != null && dgList_Repl_Contr.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void Add_List_Repl_Contr_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRepl = _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].DefaultView.AddNew();
            _dsDataTransfer.Tables["PROJECT_LIST_REPL_CONTR"].Rows.Add(_currentRepl.Row);
            dgList_Repl_Contr.SelectedItem = _currentRepl;

            Find_Emp findEmp = new Find_Emp(
                ((DataRowView)this.DataContext)["DATE_TRANSFER"] != DBNull.Value ?
                Convert.ToDateTime(((DataRowView)this.DataContext)["DATE_TRANSFER"]) :
                DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                _currentRepl["PROJECT_TERM_TRANSFER_ID"] = _dsDataTransfer.Tables["PROJECT_TERM_CONTR"].DefaultView[0]["PROJECT_TERM_TRANSFER_ID"];
                _currentRepl["TRANSFER_ID"] = findEmp.Transfer_ID;
                _currentRepl["PER_NUM"] = findEmp.Per_Num;
                _currentRepl["EMP_LAST_NAME"] = findEmp.Last_Name;
                _currentRepl["EMP_FIRST_NAME"] = findEmp.First_Name;
                _currentRepl["EMP_MIDDLE_NAME"] = findEmp.Middle_Name;
                //_currentRepl.Row.AcceptChanges();
            }
            else
            {
                _currentRepl.Row.RejectChanges();
            }
        }

        private void Edit_List_Repl_Contr_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgList_Repl_Contr.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            Find_Emp findEmp = new Find_Emp(
                ((DataRowView)this.DataContext)["DATE_TRANSFER"] != DBNull.Value ?
                Convert.ToDateTime(((DataRowView)this.DataContext)["DATE_TRANSFER"]) :
                DateTime.Today);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                rowSelected["TRANSFER_ID"] = findEmp.Transfer_ID;
                rowSelected["PER_NUM"] = findEmp.Per_Num;
                rowSelected["EMP_LAST_NAME"] = findEmp.Last_Name;
                rowSelected["EMP_FIRST_NAME"] = findEmp.First_Name;
                rowSelected["EMP_MIDDLE_NAME"] = findEmp.Middle_Name;
                //rowSelected.Row.AcceptChanges();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            }
        }

        private void Delete_List_Repl_Contr_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgList_Repl_Contr.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgList_Repl_Contr.SelectedCells[0].Item).Delete();
                }
            }
            dgList_Repl_Contr.Focus();
        }
        
        private void Save_Project_Approval_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value &&
                _dsProject_Approval != null && _dsProject_Approval.HasChanges() &&
                Fl_Add_Approval)
            {
                e.CanExecute = true;
            }
        }

        private void Save_Project_Approval_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cbTYPE_APPROVAL_ID.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Не выбрано решение!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (((DataRowView)cbTYPE_APPROVAL_ID.SelectedItem)["SIGN_APPROVAL_NOTE"].ToString() == "1")
            {
                if (tbNOTE_APPROVAL.Text.Trim() == "" || String.IsNullOrEmpty(tbNOTE_APPROVAL.Text.Trim()))
                {
                    System.Windows.MessageBox.Show("Для данного решения необходимо заполнить поле Примечание!", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (SaveProject_Approval())
            {
                GetStatusProject();
            }
        }
        
        bool SaveProject_Approval()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _row_Approval.Row["TYPE_APPROVAL_ID"] = cbTYPE_APPROVAL_ID.SelectedValue;
                _row_Approval.Row["TYPE_APPROVAL_NAME"] = cbTYPE_APPROVAL_ID.Text;
                _daProject_Approval.InsertCommand.Transaction = transact;
                _daProject_Approval.Update(_dsProject_Approval.Tables["PROJECT_APPROVAL"]);
                transact.Commit();
                CommandManager.InvalidateRequerySuggested();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения решения", MessageBoxButton.OK, MessageBoxImage.Error);
                GetStatusProject();
                CommandManager.InvalidateRequerySuggested();
                return false;
            }
        }

        void GetStatusProject()
        {
            _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID"] =
                _ocGet_Status_Project.ExecuteScalar();
            _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"] =
                ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView.Table.Select().Where(r =>
                    r["PROJECT_PLAN_APPROVAL_ID"].ToString() == _dsProject_Approval.Tables["PLAN_APPROVAL"].
                        DefaultView[0]["PROJECT_PLAN_APPROVAL_ID"].ToString()).FirstOrDefault()["PROJECT_PLAN_APPROVAL_ID_PRIOR"];

            dgProject_Approval.DataContext = null;
            _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.Clear();
            _daProject_Approval.Fill(_dsProject_Approval.Tables["PROJECT_APPROVAL"]);
            dgProject_Approval.DataContext = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView;

            _dsProject_Approval.Tables["TYPE_APPROVAL"].Clear();
            Fl_Add_Approval = false;          
            // После обновления статуса проекта, нужно просмотреть открыто ли редактирование
            // Если открыто, то проверяем, можно ли добавлять новое решение
            if (Convert.ToBoolean(_ocGet_Sign_Open_Approval.ExecuteScalar()))
            {
                _daType_Approval.SelectCommand.Parameters["p_PROJECT_PLAN_APPROVAL_ID"].Value =
                    _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"];
                _daType_Approval.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value =
                    ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                _daType_Approval.Fill(_dsProject_Approval.Tables["TYPE_APPROVAL"]);
                // После заполнения типов решений, смотрим доступно ли хоть одно решение. 
                // Если доступно, то добавляем новую запись чтобы пользователю осталось нажать лишь кнопку сохранить
                if (_dsProject_Approval.Tables["TYPE_APPROVAL"].DefaultView.Count > 0)
                {
                    Fl_Add_Approval = true;
                    _dsProject_Approval.Tables["PROJECT_APPROVAL"].RejectChanges();
                    _row_Approval = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView.AddNew();
                    _row_Approval["PROJECT_TRANSFER_ID"] = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                    _row_Approval["PROJECT_PLAN_APPROVAL_ID"] =
                        _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"];
                    _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.InsertAt(_row_Approval.Row, 0);
                    dgProject_Approval.SelectedItem = _row_Approval;
                }
            }
        }

        private void Form_Order_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            /*try
            {
                bool f1 = _dsDataTransfer != null && !_dsDataTransfer.HasChanges();
                object dfa = _dsDataTransfer.GetChanges();
            }
            catch { }
            try
            {
                bool f2 = this.DataContext != null;
            }
            catch { }
            try
            {
                bool f3 = ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value;
            }
            catch { }*/
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null &&
                (((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value || Connect.UserId.ToUpper() == "BMW12714") &&
                (((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"] != DBNull.Value && Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"])))
            {
                e.CanExecute = true;
            }
        }

        private void Form_Order_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выполняем небольшую проверку на наличие прав
            int _type_Transfer_ID = Convert.ToInt16(((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"]);
            if (_type_Transfer_ID == 1)
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для формирования приказа о Приеме!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["DATE_TRANSFER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не указали дату приема!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
                {
                    ((DataRowView)this.DataContext)["TR_DATE_ORDER"] = DateTime.Now;
                    return;
                }
                if (((DataRowView)this.DataContext)["DATE_CONTR"] == DBNull.Value)
                {
                    ((DataRowView)this.DataContext)["DATE_CONTR"] = DateTime.Now;
                    return;
                }
            }
            else
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_PERSONNEL"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для формирования приказа о Переводе!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["DATE_TRANSFER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не указали дату перевода!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                // Если установлен признак канцелярии, то обязательно должен быть заполнен номер приказа
                if (Convert.ToBoolean(((DataRowView)this.DataContext)["CHAN_SIGN"]) && ((DataRowView)this.DataContext)["TR_NUM_ORDER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не присвоили значение номеру приказа о переводе!\nПовторите ввод.", "АСУ \"Кадры\"", 
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
                {
                    ((DataRowView)this.DataContext)["TR_DATE_ORDER"] = DateTime.Now;
                    return;
                }  
            }
            if (((DataRowView)this.DataContext).DataView.Table.GetChanges() != null)
            {
                _ocUpdate_Order.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                _ocUpdate_Order.Parameters["p_PER_NUM"].Value = ((DataRowView)this.DataContext)["PER_NUM"];
                _ocUpdate_Order.Parameters["p_DATE_TRANSFER"].Value = ((DataRowView)this.DataContext)["DATE_TRANSFER"];
                _ocUpdate_Order.Parameters["p_TR_NUM_ORDER"].Value = ((DataRowView)this.DataContext)["TR_NUM_ORDER"];
                _ocUpdate_Order.Parameters["p_TR_DATE_ORDER"].Value = ((DataRowView)this.DataContext)["TR_DATE_ORDER"];
                _ocUpdate_Order.Parameters["p_CONTR_EMP"].Value = ((DataRowView)this.DataContext)["CONTR_EMP"];
                _ocUpdate_Order.Parameters["p_DATE_CONTR"].Value = ((DataRowView)this.DataContext)["DATE_CONTR"];
                _ocUpdate_Order.Parameters["p_CHAN_SIGN"].Value = ((DataRowView)this.DataContext)["CHAN_SIGN"];
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocUpdate_Order.Transaction = transact;
                    _ocUpdate_Order.ExecuteNonQuery();
                    transact.Commit();
                    ((DataRowView)this.DataContext)["TR_NUM_ORDER"] = _ocUpdate_Order.Parameters["p_TR_NUM_ORDER"].Value;
                    ((DataRowView)this.DataContext)["CONTR_EMP"] = _ocUpdate_Order.Parameters["p_CONTR_EMP"].Value;
                    ((DataRowView)this.DataContext)["PER_NUM"] = _ocUpdate_Order.Parameters["p_PER_NUM"].Value;
                    _dsDataTransfer.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transact.Rollback();
                    System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка обновления номера приказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (_type_Transfer_ID == 1)
            {
                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "Hire_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                {
                    Transfer_Project_Viewer.Ds.Tables["SIGNES"].Clear();
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                        Connect.CurConnect).Fill(Transfer_Project_Viewer.Ds.Tables["SIGNES"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        Transfer_Project_Viewer.Ds.Tables["SIGNES"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                    }
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectOrder_Hire_From_Project.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Clear();
                    _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"]);
                    ReportViewerWindow report = new ReportViewerWindow(
                        "Приказ о приеме", "Reports/Order_Hire.rdlc", Transfer_Project_Viewer.Ds,
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                    );
                    report.Show();
                }
            }
            else
            {
                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "Transfer_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                {
                    Transfer_Project_Viewer.Ds.Tables["SIGNES"].Clear();
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                        Connect.CurConnect).Fill(Transfer_Project_Viewer.Ds.Tables["SIGNES"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        Transfer_Project_Viewer.Ds.Tables["SIGNES"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                    }
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectOrder_Transfer_From_Project.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Clear();
                    _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"]);
                    // Разделяем название должности на 3 строки чтобы была красивая печать
                    List<string> slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS_NAME"].ToString() +
                        " " + Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS_NOTE"].ToString(), ' ');
                    List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 20, 38);
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS1"] = arrayPos[0];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS2"] = arrayPos[1];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS3"] = arrayPos[2];
                    slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PREV_POS_NAME"].ToString() +
                        " " + Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PREV_POS_NOTE"].ToString(), ' ');
                    arrayPos = Kadr.Transfer.ArraySlov(slova, 20, 38);
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PREV_POS1"] = arrayPos[0];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PREV_POS2"] = arrayPos[1];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PREV_POS3"] = arrayPos[2];
                    ReportViewerWindow report = new ReportViewerWindow(
                        "Приказ о переводе", "Reports/Order_Transfer.rdlc", Transfer_Project_Viewer.Ds,
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                    );
                    report.Show();
                }
            }
        }

        private void Project_To_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value &&
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]) &&
                ((DataRowView)this.DataContext)["TR_NUM_ORDER"] != DBNull.Value)
            {
                e.CanExecute = true;
            }
        }

        private void Project_To_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выполняем небольшую проверку на наличие прав
            int _type_Transfer_ID = Convert.ToInt16(((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"]);
            if (_type_Transfer_ID == 1)
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для проведения проекта в Основную БД!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_PERSONNEL"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для проведения проекта в Основную БД!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            _ocProject_To_Transfer.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            _ocProject_To_Transfer.Parameters["p_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["TRANSFER_ID"];
            _ocProject_To_Transfer.Parameters["p_PERCO_SYNC_ID"].Value = ((DataRowView)this.DataContext)["PERCO_SYNC_ID"];
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocProject_To_Transfer.Transaction = transact;
                _ocProject_To_Transfer.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка проведения проекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (((DataRowView)gridAdd_Condition.DataContext)["PRIVILEGED_POSITION_ID"] != DBNull.Value)
            {
                System.Windows.MessageBox.Show("Данный работник оформляется На вредную профессию!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (_dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView.Count > 0 &&
                    ((DataRowView)_dsDataTransfer.Tables["TRANSFER_PREVIEW"].DefaultView[0])["PRIVILEGED_POSITION_ID"] != DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Данный работник переводится С вредной профессии!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            // Объекты для изменения данных в различных DBF-таблицах
            OdbcConnection odbcCon;
            OdbcCommand _rezult;
            DataRowView _currentEmp = (DataRowView)this.DataContext;
            if (_type_Transfer_ID == 1)
            {
                if (_currentEmp["VISUAL_PER_NUM"] == DBNull.Value)
                {
                    FormMain.employees.InsertEmployee(new PercoXML.Employee(_ocProject_To_Transfer.Parameters["p_PERCO_SYNC_ID"].Value.ToString(),
                        _currentEmp["PER_NUM"].ToString(), _currentEmp["EMP_LAST_NAME"].ToString(),
                        _currentEmp["EMP_FIRST_NAME"].ToString(), _currentEmp["EMP_MIDDLE_NAME"].ToString(),
                        _currentEmp["SUBDIV_ID"].ToString(), _currentEmp["POS_ID"].ToString()
                        ) { DateBegin = _currentEmp["DATE_HIRE"].ToString() });
                }
                else
                {
                    // 15.08.2016 Нужно проверять по основной работе принимается человек или по совмещению.
                    // Если по совмещению, то нужно проверить если основная работа
                    if (_currentEmp["SIGN_COMB"].ToString() == "1")
                    {
                        OracleCommand _ocCheck = new OracleCommand(string.Format(
                            @"select COUNT(*) from {0}.TRANSFER T where T.PER_NUM = :p_PER_NUM and T.SIGN_CUR_WORK = 1 and T.SIGN_COMB = 0",
                            Connect.Schema), Connect.CurConnect);
                        _ocCheck.BindByName = true;
                        _ocCheck.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _currentEmp["PER_NUM"];
                        //  Если основной работы нет, то обновляем данные в Перко, иначе обновлять ничего не нужно
                        if (Convert.ToInt16(_ocCheck.ExecuteScalar()) == 0)
                        {
                            FormMain.employees.RecoveryEmployee(_currentEmp["PERCO_SYNC_ID"].ToString(), _currentEmp["PER_NUM"].ToString());
                            FormMain.employees.UpdateEmployee(new PercoXML.Employee(_currentEmp["PERCO_SYNC_ID"].ToString(),
                                _currentEmp["PER_NUM"].ToString(), _currentEmp["EMP_LAST_NAME"].ToString(),
                                _currentEmp["EMP_FIRST_NAME"].ToString(), _currentEmp["EMP_MIDDLE_NAME"].ToString(),
                                _currentEmp["SUBDIV_ID"].ToString(), _currentEmp["POS_ID"].ToString()
                                ) { DateBegin = _currentEmp["DATE_HIRE"].ToString() });
                        }
                    }
                    else
                    {
                        FormMain.employees.RecoveryEmployee(_currentEmp["PERCO_SYNC_ID"].ToString(), _currentEmp["PER_NUM"].ToString());
                        FormMain.employees.UpdateEmployee(new PercoXML.Employee(_currentEmp["PERCO_SYNC_ID"].ToString(),
                            _currentEmp["PER_NUM"].ToString(), _currentEmp["EMP_LAST_NAME"].ToString(),
                            _currentEmp["EMP_FIRST_NAME"].ToString(), _currentEmp["EMP_MIDDLE_NAME"].ToString(),
                            _currentEmp["SUBDIV_ID"].ToString(), _currentEmp["POS_ID"].ToString()
                            ) { DateBegin = _currentEmp["DATE_HIRE"].ToString() });
                    }
                }

                #region Обновление таблиц DBF
                /*Добавляем новую запись в SPR*/
                odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SPR"]);
                odbcCon.Open();
                _rezult = new OdbcCommand("", odbcCon);
                DateTime datePost;
                if (_currentEmp["DATE_ADD"] != DBNull.Value)
                {
                    datePost = (DateTime)_currentEmp["DATE_ADD"];
                }
                else
                {
                    datePost = new DateTime(1, 1, 1);
                }
                DateTime date_servant;
                if (_currentEmp["DATE_SERVANT"] != DBNull.Value)
                {
                    date_servant = (DateTime)_currentEmp["DATE_SERVANT"];
                }
                else
                {
                    date_servant = _currentEmp["DATE_HIRE"] != DBNull.Value ? (DateTime)_currentEmp["DATE_HIRE"] : DateTime.Today;
                }
                // Проверяем наличие записей в SPR по сотруднику в данном подразделении
                _rezult.CommandText = string.Format(
                    "select count(*) from SPR where podr = '{0}' and tnom = '{1}' and p_rab = '{2}'",
                    _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "");
                int countRow = Convert.ToInt32(_rezult.ExecuteScalar());
                /// Если записей нет, то вставляем новую запись. Если записи есть, то обновляем существующую запись.
                if (countRow == 0)
                {
                    _rezult.CommandText = string.Format(
                        "insert into SPR(PODR,TNOM,FIO,DAT_POST,KT,SHPR,POL,DAT_ROG,DAT_KOR, " +
                        "FAM,NAM,OTC,P_RAB,RAZ,OKL," +
                        "PRF,PRNAD, NAL,ALIM,NAD_VRED,BIR,SC_OT,PR13,DUB_NAL,KOL_IG14,KOL_IG15, " +
                        "KOL_IG16,KOL_IG17,KOL_IG18,KOL_IG19,KOL_IG20,KOL_IG21,POST_INDEX,OKL_N, " +
                        "VSET, INN_STRAH,KOD_DOC,SER_DOC,NOM_DOC,STATE,CITY,PUNKT,STREET,HOUSE,KORPUS,FLAT,P_PI, " +
                        "PUNKT_ROG,DISTR_ROG,REGION_ROG,STRANA_ROG,KEM_VID,PHONE,NALOG_INN,SER_MEDP,NOM_MEDP,ZAKAZ, " +
                        "DAT_VIS,DAT_UV,DAT_PI,DAT_DOK,DAT_PR,DAT_PRIEM,DAT_PRUZ)" +
                        "values('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8}, " +
                        "'{9}','{10}','{11}','{12}', {14}, {15}, " +
                        /*19*/
                        "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, " + "'{16}'," +
                        /*22*/
                        "'','','','','','','','','','','','','','','','','','','','','','', " +
                        "{13},ctod(''),ctod(''),ctod(''),ctod(''),ctod(''),ctod(''))",
                        _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"],
                        string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                            _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)),
                        "{^" + datePost.Year.ToString() + "-" + datePost.Month.ToString().PadLeft(2, '0') + "-" + datePost.Day.ToString().PadLeft(2, '0') + "}",
                        _currentEmp["CODE_DEGREE"], _currentEmp["CODE_POS"], _currentEmp["EMP_SEX"].ToString() == "М" ? "1" : "2",
                        "{^" + Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Year.ToString() + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Month.ToString().PadLeft(2, '0') + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Day.ToString().PadLeft(2, '0') + "}",
                        "date()",
                        _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"], _currentEmp["EMP_MIDDLE_NAME"],
                        Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                        "{^" + date_servant.Year.ToString() + "-" + date_servant.Month.ToString().PadLeft(2, '0') + "-" + date_servant.Day.ToString().PadLeft(2, '0') + "}",
                        _currentEmp["CLASSIFIC"] == DBNull.Value ? 0 : _currentEmp["CLASSIFIC"],
                        _currentEmp["SALARY"] == DBNull.Value ? "0" : _currentEmp["SALARY"].ToString().Replace(",", "."),
                        _currentEmp["CODE_TARIFF_GRID"]);
                }
                else
                {
                    _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                        "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                        "NAM = '{10}', OTC = '{11}', DAT_VIS = {13}, DAT_UV = {14}, RAZ = {15}, OKL = {16}, VSET = '{17}' " +
                        "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",
                        _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                        string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                            _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)),
                        "{^" + datePost.Year.ToString() + "-" +
                            datePost.Month.ToString().PadLeft(2, '0') + "-" +
                            datePost.Day.ToString().PadLeft(2, '0') + "}",
                        _currentEmp["CODE_DEGREE"], _currentEmp["CODE_POS"], _currentEmp["EMP_SEX"].ToString() == "М" ? "1" : "2",
                        "{^" + Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Year.ToString() + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Month.ToString().PadLeft(2, '0') + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Day.ToString().PadLeft(2, '0') + "}",
                        "date()",
                        _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"], _currentEmp["EMP_MIDDLE_NAME"],
                        _currentEmp["CODE_SUBDIV"],
                        "{^" + date_servant.Year.ToString() + "-" +
                            date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                            date_servant.Day.ToString().PadLeft(2, '0') + "}", "ctod('')",
                        _currentEmp["CLASSIFIC"] == DBNull.Value ? 0 : _currentEmp["CLASSIFIC"],
                        _currentEmp["SALARY"] == DBNull.Value ? "0" : _currentEmp["SALARY"].ToString().Replace(",", "."),
                        _currentEmp["CODE_TARIFF_GRID"]);
                }
                _rezult.ExecuteNonQuery();
                odbcCon.Close();
                #endregion
            }
            else
            {

                FormMain.employees.UpdateEmployee(new PercoXML.Employee(_currentEmp["PERCO_SYNC_ID"].ToString(),
                    _currentEmp["PER_NUM"].ToString(), _currentEmp["EMP_LAST_NAME"].ToString(),
                    _currentEmp["EMP_FIRST_NAME"].ToString(), _currentEmp["EMP_MIDDLE_NAME"].ToString(),
                    _currentEmp["SUBDIV_ID"].ToString(), _currentEmp["POS_ID"].ToString()));

                #region Обновление таблиц DBF
                /*При переводе работника необходимо обновить данные в таблицах PODOT, SUD, UD_SUD */
                /*Обновление PODOT*/
                odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["PODOT"]);
                odbcCon.Open();
                _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format("update podot set podr = '{0}' where tnom = '{1}'",
                    _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"]);
                try
                {
                    /* Обновляем данные в подотчете только для основных работников! */
                    if (!Convert.ToBoolean(_currentEmp["SIGN_COMB"]))
                    {
                        _rezult.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Ошибка при обновлении таблицы PODOT!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                odbcCon.Close();
                /*Обновление SUD, UD_SUD*/
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SUD"]);
                odbcCon.Open();
                _rezult.CommandText = string.Format("update SUD set podr = '{0}' where tnom = '{1}' and p_rab = '{2}'",
                    _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "");
                try
                {
                    _rezult.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Ошибка при обновлении таблицы SUD!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                _rezult.CommandText = string.Format("select pnsud from SUD where tnom = '{0}' and p_rab = '{1}'",
                    _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "");
                OdbcCommand updUd_Sud = new OdbcCommand("", odbcCon);
                OdbcDataReader reader = _rezult.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        updUd_Sud.CommandText = string.Format("update UD_SUD set podr = '{0}' where tnom = '{1}' and pnsud = '{2}'",
                           _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"], reader[0].ToString().Trim());
                        updUd_Sud.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Ошибка при обновлении таблицы UD_SUD!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                odbcCon.Close();

                // Выбираем подразделение из какого он перевелся для обновления справочника
                OracleCommand comSub = new OracleCommand("", Connect.CurConnect);
                comSub.BindByName = true;
                comSub.CommandText = string.Format(
                    @"select SUBDIV_ID, CODE_SUBDIV from {0}.SUBDIV S
                        WHERE S.SUBDIV_ID = 
                            (select T.SUBDIV_ID 
                            from {0}.TRANSFER T
                            join {0}.PROJECT_TRANSFER PT on (T.TRANSFER_ID = PT.FROM_POSITION)
                            where PT.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID)",
                    Connect.Schema);
                comSub.BindByName = true;
                comSub.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = _currentEmp["PROJECT_TRANSFER_ID"];
                string _old_code_subdiv = "";
                decimal _old_subdiv_id = 0;
                OracleDataReader orSub = comSub.ExecuteReader();
                while (orSub.Read())
                {
                    _old_code_subdiv = orSub["CODE_SUBDIV"].ToString();
                    _old_subdiv_id = Convert.ToDecimal(orSub["SUBDIV_ID"]);
                }
                /*При добавлении нового перевода, когда меняется подразделение нужно в SPR занести новую запись*/
                if (Convert.ToDecimal(_currentEmp["SUBDIV_ID"]) != _old_subdiv_id)
                {                    
                    // При добавлении нового перевода нужно обновить старую запись, установив значения 
                    // в полях FIO, DAT_UV
                    odbcCon.ConnectionString = string.Format(
                        @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                        ParVal.Vals["SPR"]);
                    odbcCon.Open();
                    _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_UV = {3} " +
                        "where podr = '{4}' and tnom = '{0}' and p_rab = '{1}'",
                        _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                        string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                            _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)).Trim().PadRight(21, ' ') + "*",
                            "{^" + Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).AddDays(-1).Year.ToString() + "-" +
                            Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).AddDays(-1).Month.ToString().PadLeft(2, '0') + "-" +
                            Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).AddDays(-1).Day.ToString().PadLeft(2, '0') + "}",
                        _old_code_subdiv);
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    DateTime datePost = (DateTime)_currentEmp["DATE_ADD"];
                    DateTime date_servant;
                    if (_currentEmp["DATE_SERVANT"] != DBNull.Value)
                    {
                        date_servant = (DateTime)_currentEmp["DATE_SERVANT"];
                    }
                    else
                    {
                        date_servant = (DateTime)_currentEmp["DATE_HIRE"];
                    }
                    // Проверяем наличие записей в SPR по сотруднику в данном подразделении
                    _rezult.CommandText = string.Format(
                        "select count(*) from SPR where podr = '{0}' and tnom = '{1}' and p_rab = '{2}'",
                        _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "");
                    int countRow = 0;
                    try
                    {
                        Convert.ToInt32(_rezult.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка при запросе к таблице SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    // Если записей нет, то вставляем новую запись. Если записи есть, то обновляем существующую запись.
                    if (countRow == 0)
                    {
                        /// Затем нужно вставить новую запись
                        _rezult.CommandText = string.Format(
                            "insert into SPR(PODR,TNOM,FIO,DAT_POST,KT,SHPR,POL,DAT_ROG,DAT_KOR, " +
                            "FAM,NAM,OTC,P_RAB,RAZ,OKL," +
                            "PRF,PRNAD,NAL,ALIM,NAD_VRED,BIR,SC_OT,PR13,DUB_NAL,KOL_IG14,KOL_IG15, " +
                            "KOL_IG16,KOL_IG17,KOL_IG18,KOL_IG19,KOL_IG20,KOL_IG21,POST_INDEX,OKL_N, " +
                            "VSET, INN_STRAH,KOD_DOC,SER_DOC,NOM_DOC,STATE,CITY,PUNKT,STREET,HOUSE,KORPUS,FLAT,P_PI, " +
                            "PUNKT_ROG,DISTR_ROG,REGION_ROG,STRANA_ROG,KEM_VID,PHONE,NALOG_INN,SER_MEDP,NOM_MEDP,ZAKAZ, " +
                            "DAT_VIS,DAT_UV,DAT_PI,DAT_DOK,DAT_PR,DAT_PRIEM,DAT_PRUZ)" +
                            "values('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8}, " +
                            "'{9}','{10}','{11}','{12}', {14}, {15}, " +
                            /*19*/
                            "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, " + "'{16}'," +
                            /*22*/
                            "'','','','','','','','','','','','','','','','','','','','','','', " +
                            "{13},ctod(''),ctod(''),ctod(''),ctod(''),ctod(''),ctod(''))",
                            _currentEmp["CODE_SUBDIV"], _currentEmp["PER_NUM"],
                            string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                                _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)),
                            "{^" + datePost.Year.ToString() + "-" + datePost.Month.ToString().PadLeft(2, '0') + "-" + datePost.Day.ToString().PadLeft(2, '0') + "}",
                            _currentEmp["CODE_DEGREE"], _currentEmp["CODE_POS"], _currentEmp["EMP_SEX"].ToString() == "М" ? "1" : "2",
                            "{^" + Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Year.ToString() + "-" +
                                Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Month.ToString().PadLeft(2, '0') + "-" +
                                Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Day.ToString().PadLeft(2, '0') + "}",
                            "date()",
                            _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"], _currentEmp["EMP_MIDDLE_NAME"],
                            Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                            "{^" + date_servant.Year.ToString() + "-" + date_servant.Month.ToString().PadLeft(2, '0') + "-" + date_servant.Day.ToString().PadLeft(2, '0') + "}",
                            _currentEmp["CLASSIFIC"] == DBNull.Value ? 0 : _currentEmp["CLASSIFIC"],
                            _currentEmp["SALARY"] == DBNull.Value ? "0" : _currentEmp["SALARY"].ToString().Replace(",", "."),
                            _currentEmp["CODE_TARIFF_GRID"]);
                    }
                    else
                    {
                        _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                            "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                            "NAM = '{10}', OTC = '{11}', DAT_VIS = {13}, DAT_UV = {14}, RAZ = {15}, OKL = {16}, VSET = '{17}' " +
                            "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",                            
                            _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                            string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                                _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)),
                            "{^" + datePost.Year.ToString() + "-" +
                                datePost.Month.ToString().PadLeft(2, '0') + "-" +
                                datePost.Day.ToString().PadLeft(2, '0') + "}",
                            _currentEmp["CODE_DEGREE"], _currentEmp["CODE_POS"], _currentEmp["EMP_SEX"].ToString() == "М" ? "1" : "2",
                            "{^" + Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Year.ToString() + "-" +
                                Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Month.ToString().PadLeft(2, '0') + "-" +
                                Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Day.ToString().PadLeft(2, '0') + "}",
                            "date()",
                            _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"], _currentEmp["EMP_MIDDLE_NAME"],
                            _currentEmp["CODE_SUBDIV"],
                            "{^" + date_servant.Year.ToString() + "-" +
                                date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                                date_servant.Day.ToString().PadLeft(2, '0') + "}", "ctod('')",
                            _currentEmp["CLASSIFIC"] == DBNull.Value ? 0 : _currentEmp["CLASSIFIC"],
                            _currentEmp["SALARY"] == DBNull.Value ? "0" : _currentEmp["SALARY"].ToString().Replace(",", "."),
                            _currentEmp["CODE_TARIFF_GRID"]);
                    }
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    odbcCon.Close();
                }
                else
                {
                    // При редактировании перевода нужно обновить запись, установив новые значения 
                    // в полях PODR, FIO, DAT_POST, KT, SHPR, POL, DAT_ROG, DAT_KOR, FAM, NAM, OTC
                    odbcCon.ConnectionString = string.Format(
                        @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                        ParVal.Vals["SPR"]);
                    odbcCon.Open();
                    DateTime datePost = new DateTime();
                    if (_currentEmp["DATE_ADD"] != DBNull.Value)
                    {
                        datePost = (DateTime)_currentEmp["DATE_ADD"];
                    }
                    DateTime date_servant;
                    if (_currentEmp["DATE_SERVANT"] != DBNull.Value)
                    {
                        date_servant = (DateTime)_currentEmp["DATE_SERVANT"];
                    }
                    else
                    {
                        date_servant = (DateTime)_currentEmp["DATE_HIRE"];
                    }
                    _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                        "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                        "NAM = '{10}', OTC = '{11}', PODR = '{13}', DAT_VIS = {14}, DAT_UV = {15}, RAZ = {16}, OKL = {17}, VSET = '{18}' " +
                        "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",
                        _currentEmp["PER_NUM"], Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                        string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                            _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)),
                        "{^" + datePost.Year.ToString() + "-" +
                            datePost.Month.ToString().PadLeft(2, '0') + "-" +
                            datePost.Day.ToString().PadLeft(2, '0') + "}",
                        _currentEmp["CODE_DEGREE"], _currentEmp["CODE_POS"], _currentEmp["EMP_SEX"].ToString() == "М" ? "1" : "2",
                        "{^" + Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Year.ToString() + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Month.ToString().PadLeft(2, '0') + "-" +
                            Convert.ToDateTime(_currentEmp["EMP_BIRTH_DATE"]).Day.ToString().PadLeft(2, '0') + "}",
                        "date()",
                        _currentEmp["EMP_LAST_NAME"], _currentEmp["EMP_FIRST_NAME"], _currentEmp["EMP_MIDDLE_NAME"],
                        _old_code_subdiv, _currentEmp["CODE_SUBDIV"],
                        "{^" + date_servant.Year.ToString() + "-" +
                            date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                            date_servant.Day.ToString().PadLeft(2, '0') + "}", "ctod('')",
                        _currentEmp["CLASSIFIC"] == DBNull.Value ? 0 : _currentEmp["CLASSIFIC"],
                        _currentEmp["SALARY"] == DBNull.Value ? "0" : _currentEmp["SALARY"].ToString().Replace(",", "."),
                        _currentEmp["CODE_TARIFF_GRID"]);
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    odbcCon.Close();
                }
                #endregion
            }
            System.Windows.MessageBox.Show("Изменения сохранены в базе данных!",
                "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.Close();
        }

        private void Form_Contract_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null && 
                (((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value || Connect.UserId.ToUpper() == "BMW12714" || Connect.UserId=="KNV14534") &&
                ((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"] != DBNull.Value &&
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]) &&
                ((DataRowView)this.DataContext)["DATE_TRANSFER"] != DBNull.Value &&
                ((DataRowView)this.DataContext)["TR_DATE_ORDER"] != DBNull.Value)
            {
                e.CanExecute = true;
            }
        }

        private void Form_Contract_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выполняем небольшую проверку на наличие прав
            int _type_Transfer_ID = Convert.ToInt16(((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"]);
            if (_type_Transfer_ID == 1)
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для формирования приказа о Приеме!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не указали дату приказа!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["DATE_CONTR"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не указали дату договора!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                if (!GrantedRoles.GetGrantedRole("STAFF_PERSONNEL"))
                {
                    System.Windows.MessageBox.Show("У Вас недостаточно прав для формирования приказа о Переводе!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                // Если установлен признак канцелярии, то обязательно должен быть заполнен номер приказа
                if (Convert.ToBoolean(((DataRowView)this.DataContext)["CHAN_SIGN"]) && ((DataRowView)this.DataContext)["TR_NUM_ORDER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не присвоили значение номеру приказа о переводе!\nПовторите ввод.", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
                {
                    System.Windows.MessageBox.Show("Вы не указали дату приказа!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            if (((DataRowView)this.DataContext).DataView.Table.GetChanges() != null)
            {
                _ocUpdate_Order.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                _ocUpdate_Order.Parameters["p_PER_NUM"].Value = ((DataRowView)this.DataContext)["PER_NUM"];
                _ocUpdate_Order.Parameters["p_DATE_TRANSFER"].Value = ((DataRowView)this.DataContext)["DATE_TRANSFER"];
                _ocUpdate_Order.Parameters["p_TR_NUM_ORDER"].Value = ((DataRowView)this.DataContext)["TR_NUM_ORDER"];
                _ocUpdate_Order.Parameters["p_TR_DATE_ORDER"].Value = ((DataRowView)this.DataContext)["TR_DATE_ORDER"];
                _ocUpdate_Order.Parameters["p_CONTR_EMP"].Value = ((DataRowView)this.DataContext)["CONTR_EMP"];
                _ocUpdate_Order.Parameters["p_DATE_CONTR"].Value = ((DataRowView)this.DataContext)["DATE_CONTR"];
                _ocUpdate_Order.Parameters["p_CHAN_SIGN"].Value = ((DataRowView)this.DataContext)["CHAN_SIGN"];
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocUpdate_Order.Transaction = transact;
                    _ocUpdate_Order.ExecuteNonQuery();
                    transact.Commit();
                    ((DataRowView)this.DataContext)["TR_NUM_ORDER"] = _ocUpdate_Order.Parameters["p_TR_NUM_ORDER"].Value;
                    ((DataRowView)this.DataContext)["CONTR_EMP"] = _ocUpdate_Order.Parameters["p_CONTR_EMP"].Value;
                    ((DataRowView)this.DataContext)["PER_NUM"] = _ocUpdate_Order.Parameters["p_PER_NUM"].Value;
                    _dsDataTransfer.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transact.Rollback();
                    System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка обновления номера приказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (_type_Transfer_ID == 1)
            {
                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "Hire_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos, 1) == System.Windows.Forms.DialogResult.OK)
                {
                    int _sign_Dir_Sugnature = 0;
                    string _fio_Signature = "";
                    Transfer_Project_Viewer.Ds.Tables["SIGNES"].Clear();
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO, 0 DEFAULT_NUMBER from dual where 1 = 2",
                        Connect.CurConnect).Fill(Transfer_Project_Viewer.Ds.Tables["SIGNES"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        Transfer_Project_Viewer.Ds.Tables["SIGNES"].Rows.Add(new object[] { 
                            s_pos[i][0].ToString(), s_pos[i][1].ToString(), s_pos[i][2].ToString() });
                        // Пожелания Эльканова Р.Д. - он хочет чтобы ФИО подписанта выходила в договоре и допнике. 
                        // Притом в формате Фамилия И.О., поэтому проводим такие махинации, меняя местами ИО и фамилию
                        if (s_pos[i][1].ToString().IndexOf(" ") > 0)
                            _fio_Signature = s_pos[i][1].ToString().Substring(s_pos[i][1].ToString().LastIndexOf(" ")) + " " + 
                                s_pos[i][1].ToString().Substring(0, s_pos[i][1].ToString().LastIndexOf(" "));
                        else
                            _fio_Signature = s_pos[i][1].ToString();
                        if (s_pos[i][2].ToString() == "1")
                            _sign_Dir_Sugnature = 1;
                    }
                    // Доп.соглашение
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepEmp_Contract.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PER_NUM"];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Clear();
                    _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"]);
                    if (Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows.Count > 0)
                    {
                        // Заполняем поле SIGN_DIR_SIGNATURE
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["SIGN_DIR_SIGNATURE"] = _sign_Dir_Sugnature;
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["FIO_SIGNATURE"] = _fio_Signature;
                        // Условия труда для допника
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepWorking_Conditions.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                            ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP_COND"].Clear();
                        _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP_COND"]);
                        // Согласование
                        Transfer_Project_Viewer.Ds.Tables["APPROVAL"].Clear();
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                            ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                        _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["APPROVAL"]);
                        // Вывод отчета на экран
                        if (System.Windows.MessageBox.Show("Вывести отчет на формате А3?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ReportViewerWindow reportContract = new ReportViewerWindow(
                                "Трудовой договор", "Reports/Emp_ContractA3.rdlc", Transfer_Project_Viewer.Ds,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                            );
                            reportContract.Show();
                        }
                        else
                        {
                            ReportViewerWindow reportContract = new ReportViewerWindow(
                                "Трудовой договор", "Reports/Emp_Contract.rdlc", Transfer_Project_Viewer.Ds,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                            );
                            reportContract.Show();
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Данных по переводу нет!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "Transfer_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos, 1) == System.Windows.Forms.DialogResult.OK)
                {
                    int _sign_Dir_Sugnature = 0;
                    string _fio_Signature = "";
                    Transfer_Project_Viewer.Ds.Tables["SIGNES"].Clear();
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO, 0 DEFAULT_NUMBER from dual where 1 = 2",
                        Connect.CurConnect).Fill(Transfer_Project_Viewer.Ds.Tables["SIGNES"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        Transfer_Project_Viewer.Ds.Tables["SIGNES"].Rows.Add(new object[] { 
                            s_pos[i][0].ToString(), s_pos[i][1].ToString(), s_pos[i][2].ToString() });
                        // Пожелания Эльканова Р.Д. - он хочет чтобы ФИО подписанта выходила в договоре и допнике. 
                        // Притом в формате Фамилия И.О., поэтому проводим такие махинации, меняя местами ИО и фамилию
                        if (s_pos[i][1].ToString().IndexOf(" ") > 0)
                            _fio_Signature = s_pos[i][1].ToString().Substring(s_pos[i][1].ToString().LastIndexOf(" ")) + " " +
                                s_pos[i][1].ToString().Substring(0, s_pos[i][1].ToString().LastIndexOf(" "));
                        else
                            _fio_Signature = s_pos[i][1].ToString();
                        if (s_pos[i][2].ToString() == "1")
                            _sign_Dir_Sugnature = 1;
                    }
                    // Доп.соглашение
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepEmp_Contract.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Decimal).Value =
                        ((DataRowView)this.DataContext)["PER_NUM"];
                    Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Clear();
                    _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"]);
                    if (Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows.Count > 0)
                    {
                        // Заполняем поле SIGN_DIR_SIGNATURE
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["SIGN_DIR_SIGNATURE"] = _sign_Dir_Sugnature;
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["FIO_SIGNATURE"] = _fio_Signature;
                        // Условия труда для допника
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepWorking_Conditions.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                            ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP_COND"].Clear();
                        _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP_COND"]);
                        // Согласование
                        Transfer_Project_Viewer.Ds.Tables["APPROVAL"].Clear();
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                            ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                        _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["APPROVAL"]);
                        // Вывод отчета на экран
                        ReportViewerWindow reportContract = new ReportViewerWindow(
                            "Дополнительное соглашение", "Reports/Add_Agreement.rdlc", Transfer_Project_Viewer.Ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                        );
                        reportContract.Show();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Данных по переводу нет!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void AddProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRow = _dsAppendix.Tables["PROJECT_APPENDIX"].DefaultView.AddNew();
            _currentRow["PROJECT_TRANSFER_ID"] = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            _dsAppendix.Tables["PROJECT_APPENDIX"].Rows.Add(_currentRow.Row);
            dgAppendix.SelectedItem = _currentRow;

            //Appendix_Editor appEditor = new Appendix_Editor(_currentRow);
            Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(null, "", false);
            appEditor.Owner = Window.GetWindow(this);
            if (appEditor.ShowDialog() == true)
            {
                _currentRow["NOTE_DOCUMENT"] = appEditor.Note_Document;
                _currentRow["DOCUMENT"] = appEditor.Document;
                SaveAppendix();
            }
            else
            {
                _dsAppendix.Tables["PROJECT_APPENDIX"].RejectChanges();
            }
        }

        private void EditProject_Appendix_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgAppendix != null && dgAppendix.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgAppendix.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            //Appendix_Editor appEditor = new Appendix_Editor(rowSelected);
            OracleCommand _ocSelectDocument = new OracleCommand(string.Format(
                "begin SELECT NVL((select DOCUMENT from {0}.PROJECT_APPENDIX where PROJECT_APPENDIX_ID = :p_PROJECT_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocSelectDocument.BindByName = true;
            _ocSelectDocument.Parameters.Add("p_PROJECT_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["PROJECT_APPENDIX_ID"];
            _ocSelectDocument.Parameters.Add("p_DOCUMENT", OracleDbType.Blob).Direction = ParameterDirection.Output;
            _ocSelectDocument.ExecuteNonQuery();
            byte[] _document = null;
            if (!(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).IsNull)
            {
                _document = (byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value;
            }
            Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(_document, rowSelected["NOTE_DOCUMENT"].ToString(), false);
            appEditor.Owner = Window.GetWindow(this);
            if (appEditor.ShowDialog() == true)
            {
                rowSelected["NOTE_DOCUMENT"] = appEditor.Note_Document;
                rowSelected["DOCUMENT"] = appEditor.Document;
                SaveAppendix();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            }
        }
        
        private void DeleteProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgAppendix.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgAppendix.SelectedCells[0].Item).Delete();
                }
                SaveAppendix();
            }
            dgAppendix.Focus();
        }
        
        private void ViewProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgAppendix.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            //Appendix_Editor appEditor = new Appendix_Editor(rowSelected);
            OracleCommand _ocSelectDocument = new OracleCommand(string.Format(
                "begin SELECT NVL((select DOCUMENT from {0}.PROJECT_APPENDIX where PROJECT_APPENDIX_ID = :p_PROJECT_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocSelectDocument.BindByName = true;
            _ocSelectDocument.Parameters.Add("p_PROJECT_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["PROJECT_APPENDIX_ID"];
            _ocSelectDocument.Parameters.Add("p_DOCUMENT", OracleDbType.Blob).Direction = ParameterDirection.Output;
            _ocSelectDocument.ExecuteNonQuery();
            byte[] _document = null;
            if (!(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).IsNull)
            {
                _document = (byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value;
            }
            Appendix_PDF_Viewer appEditor = new Appendix_PDF_Viewer(_document, rowSelected["NOTE_DOCUMENT"].ToString(), true);
            appEditor.Owner = Window.GetWindow(this);
            appEditor.ShowDialog();
        }

        void SaveAppendix()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daAppendix.InsertCommand.Transaction = transact;
                _daAppendix.UpdateCommand.Transaction = transact;
                _daAppendix.DeleteCommand.Transaction = transact;
                _daAppendix.Update(_dsAppendix.Tables["PROJECT_APPENDIX"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Registration_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null && Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_NO_REGISTRATION"]))
            {
                e.CanExecute = true;
            }
        }

        private void Registration_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ocRegistration_Project.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocRegistration_Project.Transaction = transact;
                _ocRegistration_Project.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка регистрации проекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Annul_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null /*&& 
                ((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"] != DBNull.Value &&
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"])*/
                )
            {
                e.CanExecute = true;
            }
        }

        private void Annul_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ocAnnul_Project.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocAnnul_Project.Transaction = transact;
                _ocAnnul_Project.ExecuteNonQuery();
                transact.Commit();
                System.Windows.MessageBox.Show("Изменения проведены в БД", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка аннулирования проекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Edit_Address_None_Kladr_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Address_None_Kladr addr = new Address_None_Kladr();
            addr.ShowDialog();
            tbHab_Non_Kladr_Address.Text = Address_None_Kladr.Address_None_Kladr_Property;
        }

        private void Edit_From_Position_By_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null &&
                ((DataRowView)this.DataContext)["TYPE_TRANSFER_ID"].ToString() == "2" &&
                ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value)
            {
                e.CanExecute = true;
            }
        }

        private void Edit_From_Position_By_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectFrom_Position_For_Project selectFrom_Position =
                new SelectFrom_Position_For_Project(Convert.ToDecimal(((DataRowView)this.DataContext)["FROM_POSITION"]));
            selectFrom_Position.Owner = Window.GetWindow(this);
            if (selectFrom_Position.ShowDialog() == true)
            {
                OracleCommand _ocUpdateFrom_Position = new OracleCommand(string.Format(
                    @"UPDATE {0}.PROJECT_TRANSFER PT SET FROM_POSITION = :p_FROM_POSITION WHERE PT.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID",
                    Connect.Schema), Connect.CurConnect);
                _ocUpdateFrom_Position.BindByName = true;
                _ocUpdateFrom_Position.Parameters.Add("p_FROM_POSITION", OracleDbType.Decimal).Value = selectFrom_Position.Transfer_ID;
                _ocUpdateFrom_Position.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocUpdateFrom_Position.Transaction = _transact;
                    _ocUpdateFrom_Position.ExecuteNonQuery();
                    _transact.Commit();
                    System.Windows.MessageBox.Show("Изменения сохранены в базе данных!", 
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (Exception ex)
                {
                    _transact.Rollback();
                    System.Windows.MessageBox.Show("Ошибка сохранения родительского перевода!\n"+
                        ex.Message, "АСУ \"Кадры\" - Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgAppendix_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wpf_Commands.ViewProject_Appendix.Execute(null, null);
        }

        private void btMatching_List_Statement_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand _ocProject_Statement_ID = new OracleCommand(string.Format(
                @"select PROJECT_STATEMENT_ID from {0}.PROJECT_STATEMENT WHERE PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID", 
                Connect.Schema), Connect.CurConnect);
            _ocProject_Statement_ID.BindByName = true;
            _ocProject_Statement_ID.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            OracleDataReader _dr = _ocProject_Statement_ID.ExecuteReader();
            if (_dr.Read())
            {
                Matching_List_Statement_View _matching_List = new Matching_List_Statement_View(_dr["PROJECT_STATEMENT_ID"]);
                _matching_List.Owner = Window.GetWindow(this);
                _matching_List.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Данный проект перевода был создан без электронного согласования заявления!", 
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);                
            }
        }
    }
}