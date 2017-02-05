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
using System.Data.Odbc;
using WpfControlLibrary.Classes;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Resume_Editor.xaml
    /// </summary>
    public partial class Dismiss_Emp_Editor : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        static OracleDataAdapter _daTariff_Grid_Salary = new OracleDataAdapter(), _daProject_Approval = new OracleDataAdapter(),
            _daType_Approval = new OracleDataAdapter(), _daPlan_Approval = new OracleDataAdapter(),
            _daAppendix = new OracleDataAdapter();
        static DataSet _dsDataTransfer, _dsProject_Approval, _dsAppendix;
        static OracleCommand _ocGet_Status_Project, _ocGet_Sign_Open_Approval, _ocPhotoEmp, _ocUpdate_Order,
            _ocProject_To_Transfer, _ocRegistration_Project, _ocAnnul_Project;        
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
        public Dismiss_Emp_Editor(DataRowView dataContext)
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Add_New_Emp_Editor_PropertyChanged);
            InitializeComponent();
            this.DataContext = dataContext;                      

            cbSubdiv.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;                      

            // Поставил это чтобы формы оплаты обновлялись при каждой смене категории или были пустыми когда кат. не установлена
            _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "1 = 2";

            cbPosition.ItemsSource = AppDataSet.Tables["POSITION"].DefaultView;
            cbDegree.ItemsSource = AppDataSet.Tables["DEGREE"].DefaultView;
            cbFormPay.ItemsSource = _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView;
            cbForm_Operation.ItemsSource = AppDataSet.Tables["FORM_OPERATION"].DefaultView;
            cbTariff_Grid.ItemsSource = AppDataSet.Tables["TARIFF_GRID"].DefaultView;
            cbREASON_ID.ItemsSource = _dsDataTransfer.Tables["REASON_DISMISS"].DefaultView;
            cbBASE_DOC_ID.ItemsSource = ProjectDataSet.Tables["BASE_DOC"].DefaultView;

            _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.Clear();            
            _daProject_Approval.SelectCommand.Parameters["p_PROJECT_TRANSFER_ID"].Value = dataContext["PROJECT_TRANSFER_ID"];
            _daProject_Approval.Fill(_dsProject_Approval.Tables["PROJECT_APPROVAL"]);
            dgProject_Approval.DataContext = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView;

            dcPROJECT_PLAN_APPROVAL_ID.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;

            cbTYPE_APPROVAL_ID.ItemsSource = _dsProject_Approval.Tables["TYPE_APPROVAL"].DefaultView;

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

        static Dismiss_Emp_Editor()
        {
            _dsDataTransfer = new DataSet();
            _dsDataTransfer.Tables.Add("FORM_PAY_ON_DEGREE");
            _dsDataTransfer.Tables.Add("TARIFF_GRID_SALARY");
            _dsDataTransfer.Tables.Add("REASON_DISMISS");
            _dsProject_Approval = new DataSet();
            _dsProject_Approval.Tables.Add("PROJECT_APPROVAL");
            _dsProject_Approval.Tables.Add("TYPE_APPROVAL");
            _dsProject_Approval.Tables.Add("PLAN_APPROVAL");
            _dsAppendix = new DataSet();
            _dsAppendix.Tables.Add("PROJECT_APPENDIX");

            new OracleDataAdapter(string.Format(@"select FORM_PAY_ON_DEGREE_ID, DEGREE_ID, FORM_PAY, NAME_FORM_PAY from {0}.FORM_PAY_ON_DEGREE FPD
                join {0}.FORM_PAY FP on(FPD.FORM_PAY_ID=FP.FORM_PAY)",
                Connect.Schema), Connect.CurConnect).Fill(_dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"]);

            new OracleDataAdapter(string.Format(
                "select REASON_ID, REASON_NAME, REASON_ARTICLE, REASON_ORDER, SIGN_GOOD_REASON from {0}.REASON_DISMISS order by REASON_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_dsDataTransfer.Tables["REASON_DISMISS"]);
            
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

            // Установка признака аннулирования проекта
            _ocAnnul_Project = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_ANNUL(:p_PROJECT_TRANSFER_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocAnnul_Project.BindByName = true;
            _ocAnnul_Project.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
        }

        private void SaveDismiss_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value &&
                ((DataRowView)(this.DataContext)).DataView.Table.GetChanges() != null)
            {
                e.CanExecute = Array.TrueForAll<DependencyObject>(gridDismiss.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
            }
        }

        private void SaveDismiss_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Transfer_Project_Viewer.SaveProject())
            {
                GetStatusProject();
            }
        }
        
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
                
        private void cbDegree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                if (((DataRowView)this.DataContext)["DEGREE_ID"] != DBNull.Value)
                    _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "DEGREE_ID = " +
                        ((DataRowView)this.DataContext)["DEGREE_ID"];
                else
                    _dsDataTransfer.Tables["FORM_PAY_ON_DEGREE"].DefaultView.RowFilter = "1 = 2";
            }
        }

        private void _this_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _dsAppendix.Tables["PROJECT_APPENDIX"].Clear();
            this.DataContext = null;
        }
        
        private void Save_Project_Approval_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value &&
                ((DataRowView)(this.DataContext)).DataView.Table.GetChanges() == null &&
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
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value && 
                //((DataRowView)(this.DataContext)).DataView.Table.GetChanges() == null &&
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]))
            {
                e.CanExecute = true;
            }
        }

        private void Form_Order_Project_Executed(object sender, ExecutedRoutedEventArgs e)
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
                System.Windows.MessageBox.Show("Вы не присвоили значение номеру приказа об увольнении!\nПовторите ввод.", "АСУ \"Кадры\"", 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
            //{
            //    ((DataRowView)this.DataContext)["TR_DATE_ORDER"] = DateTime.Now;
            //    //return;
            //}  
            if (((DataRowView)this.DataContext).DataView.Table.GetChanges() != null)
            {
                _ocUpdate_Order.Parameters["p_PROJECT_TRANSFER_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                _ocUpdate_Order.Parameters["p_PER_NUM"].Value = ((DataRowView)this.DataContext)["PER_NUM"];
                _ocUpdate_Order.Parameters["p_DATE_TRANSFER"].Value = ((DataRowView)this.DataContext)["DATE_TRANSFER"];
                if (((DataRowView)this.DataContext)["TR_DATE_ORDER"] == DBNull.Value)
                    ((DataRowView)this.DataContext)["TR_DATE_ORDER"] = DateTime.Now;
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
                    _dsDataTransfer.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transact.Rollback();
                    System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка обновления номера приказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            string[][] s_pos = new string[][] { };
            if (Signes.Show(0, "Dismiss_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == true)
            {
                //Transfer_Project_Viewer.Ds.Tables["SIGNES"].Clear();
                //new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                //    Connect.CurConnect).Fill(Transfer_Project_Viewer.Ds.Tables["SIGNES"]);
                //for (int i = 0; i < s_pos.Count(); i++)
                //{
                //    Transfer_Project_Viewer.Ds.Tables["SIGNES"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                //}
                //OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/SelectOrder_Hire_From_Project.sql"),
                //        Connect.Schema), Connect.CurConnect);
                //_daOrder_Hire.SelectCommand.BindByName = true;
                //_daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                //    ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                //Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Clear();
                //_daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"]);
                //// Разделяем название должности на 3 строки чтобы была красивая печать
                //List<string> slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER"].ToString(), ' ');
                //List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 65, 65);
                //Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER"] = arrayPos[0];
                //Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER1"] = arrayPos[1];
                //Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER2"] = arrayPos[2];
                //// Согласование
                //Transfer_Project_Viewer.Ds.Tables["APPROVAL"].Clear();
                //_daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                //    Connect.Schema), Connect.CurConnect);
                //_daOrder_Hire.SelectCommand.BindByName = true;
                //_daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                //    ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                //_daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["APPROVAL"]);

                //ReportViewerWindow report = new ReportViewerWindow(
                //    "Приказ об увольнении", "Reports/Order_Dismiss.rdlc", Transfer_Project_Viewer.Ds,
                //    new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                //);
                //report.Show();
            }
        }

        private void Project_Order_Dismiss_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null && ((DataRowView)this.DataContext)["TRANSFER_ID"] == DBNull.Value &&
                ((DataRowView)(this.DataContext)).DataView.Table.GetChanges() == null &&
                !Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]))
            {
                e.CanExecute = true;
            }
        }

        private void Project_Order_Dismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string[][] s_pos = new string[][] { };
            if (Signes.Show(0, "Dismiss_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == true)
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
                //                // Разделяем название должности на 3 строки чтобы была красивая печать
                //                List<string> slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER"].ToString(), ' ');                               
                //                List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 65, 65);
                //                slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["BASE_DOC_NAME"].ToString() +
                //                        " " + Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["BASE_DOC_NOTE"].ToString(), ' ');
                //                List<string> arrayBase_Doc = Kadr.Transfer.ArraySlov(slova, 60, 65);
                ///*                 
                //                Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER"] = arrayPos[0].Trim();
                //                Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER1"] = arrayPos[1].Trim();
                //                Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER2"] = arrayPos[2].Trim();
                //                // Согласование
                //                Transfer_Project_Viewer.Ds.Tables["APPROVAL"].Clear();
                //                _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                //                    Connect.Schema), Connect.CurConnect);
                //                _daOrder_Hire.SelectCommand.BindByName = true;
                //                _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                //                    ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
                //                _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["APPROVAL"]);

                //                ReportViewerWindow report = new ReportViewerWindow(
                //                    "Приказ об увольнении", "Reports/Order_Dismiss.rdlc", Transfer_Project_Viewer.Ds,
                //                    new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                //                );
                //                report.Show();*/
                //20.01.2016 - попросили формировать проект приказа в эксель, чтобы была возможность редактировать
                DateTime? _date_Transfer = null;
                if (Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["DATE_TRANSFER"] != DBNull.Value)
                {
                    _date_Transfer = Convert.ToDateTime(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["DATE_TRANSFER"]);
                }
                // Разделяем название должности на 3 строки чтобы была красивая печать
                List<string> slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ORDER"].ToString(), ' ');
                List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 65, 65);
                slova = Kadr.Transfer.Slova(Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["BASE_DOC_NAME"].ToString() +
                        " " + Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["BASE_DOC_NOTE"].ToString(), ' ');
                List<string> arrayBase_Doc = Kadr.Transfer.ArraySlov(slova, 60, 65);
                CellParameter[] cellParameters = new CellParameter[] {
                    new CellParameter(16, 11, (_date_Transfer.HasValue ? _date_Transfer.Value.Day.ToString() : ""), null),
                    new CellParameter(16, 16, (_date_Transfer.HasValue ? Library.MyMonthName(_date_Transfer.Value) : ""), null),
                    new CellParameter(16, 26, (_date_Transfer.HasValue ? _date_Transfer.Value.Year.ToString() : ""), null),
                    new CellParameter(17, 56, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["PER_NUM"].ToString(), null),
                    new CellParameter(18, 1, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["EMP_LAST_NAME"] + " " +
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["EMP_FIRST_NAME"] + " " +
                        Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["EMP_MIDDLE_NAME"], null),
                    new CellParameter(20, 20, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["CODE_SUBDIV"].ToString(), null),
                    new CellParameter(21, 1, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["SUBDIV_NAME"].ToString(), null),
                    new CellParameter(23, 1, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["POS_NAME"].ToString(), null),
                    new CellParameter(25, 8, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["CLASSIFIC"].ToString(), null),
                    new CellParameter(26, 11, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["DEGREE_NAME"].ToString(), null),
                    new CellParameter(26, 61, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["CODE_DEGREE"].ToString(), null),
                    new CellParameter(30, 1, Transfer_Project_Viewer.Ds.Tables["ORDER_EMP"].Rows[0]["REASON_ARTICLE"].ToString(), null),
                    new CellParameter(31, 1, arrayPos[0].Trim(), null),
                    new CellParameter(32, 1, arrayPos[1].Trim(), null),
                    new CellParameter(33, 1, (arrayPos[2].Trim() == "" ? arrayBase_Doc[0].Trim() : arrayPos[2].Trim()), null),
                    new CellParameter(34, 1, (arrayPos[2].Trim() != "" ? arrayBase_Doc[0].Trim() + arrayBase_Doc[1].Trim() : arrayBase_Doc[1].Trim()), null),
                    new CellParameter(35, 1, (arrayPos[2].Trim() != "" ? arrayBase_Doc[1].Trim() + arrayBase_Doc[2].Trim() : arrayBase_Doc[2].Trim()), null),
                    new CellParameter(40, 1, s_pos[0][0], null), new CellParameter(41, 48, s_pos[0][1], null)
                };
                Excel.PrintR1C1(false, "Dismiss.xlt", cellParameters);
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
            //string ffd1 = ((DataRowView)this.DataContext)["SIGN_COMB"].ToString();
            //bool fdf3 = Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_COMB"]);
            //bool fd2f3 = Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_COMB"].ToString());
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
            // Объекты для изменения данных в различных DBF-таблицах
            OdbcConnection odbcCon;
            OdbcCommand _rezult;
            DataRowView _currentEmp = (DataRowView)this.DataContext;
            if (_type_Transfer_ID == 3)
            {
                //// Для совмещения делаем дополнительную проверку, работает ли сотрудник по основной работе
                //if (Convert.ToBoolean(_currentEmp["SIGN_COMB"]) == true)
                //{
                //    OracleCommand _ocTrans = new OracleCommand(string.Format(
                //        @"select count(*) from {0}.TRANSFER T WHERE PER_NUM = :p_PER_NUM and SIGN_COMB = 0 and SIGN_CUR_WORK = 1",
                //        Connect.Schema), Connect.CurConnect);
                //    _ocTrans.BindByName = true;
                //    _ocTrans.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _currentEmp["PER_NUM"];
                //    // Если основной работы нет, то увольняем его в Перке.
                //    if (Convert.ToInt16(_ocTrans.ExecuteScalar()) == 0)
                //    {
                //        FormMain.employees.DismissEmployee(_currentEmp["PERCO_SYNC_ID"].ToString(), _currentEmp["PER_NUM"].ToString(),
                //            DateTime.Today.AddDays(-1));
                //        //(DateTime)r_transfer.DATE_TRANSFER.Value.AddDays(-1));
                //    }
                //}
                //else
                //{
                //    FormMain.employees.DismissEmployee(_currentEmp["PERCO_SYNC_ID"].ToString(), _currentEmp["PER_NUM"].ToString(),
                //        DateTime.Today.AddDays(-1));
                //    //(DateTime)r_transfer.DATE_TRANSFER.Value.AddDays(-1));
                //}
                /*При увольнении работника необходимо обновить данные в таблицах PODOT, ALSPR */
                /*Обновление PODOT*/
                odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["PODOT"]);
                odbcCon.Open();
                _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format("update podot set dat_uv = {0} where tnom = '{1}'",
                    "{^" + Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Year + "-" + Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Month +
                    "-" + Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Day + "}", _currentEmp["PER_NUM"].ToString());
                try
                {
                    if (Convert.ToBoolean(_currentEmp["SIGN_COMB"]) == false)
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
                /// При увольнении работника нужно обновить старую запись, установив значения 
                /// в полях FIO, DAT_UV
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SPR"]);
                odbcCon.Open();
                _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_UV = {3} " +
                    "where podr = '{4}' and tnom = '{0}' and p_rab = '{1}'",
                    _currentEmp["PER_NUM"].ToString(), Convert.ToBoolean(_currentEmp["SIGN_COMB"]) ? "2" : "",
                    string.Format("{0} {1} {2}", _currentEmp["EMP_LAST_NAME"].ToString(),
                        _currentEmp["EMP_FIRST_NAME"].ToString().Substring(0, 1),
                        _currentEmp["EMP_MIDDLE_NAME"].ToString().Substring(0, 1)).Trim().PadRight(21, ' ') + "*",
                    "{^" + Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Year.ToString() + "-" +
                        Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Month.ToString().PadLeft(2, '0') + "-" +
                        Convert.ToDateTime(_currentEmp["DATE_TRANSFER"]).Day.ToString().PadLeft(2, '0') + "}",
                    _currentEmp["CODE_SUBDIV"].ToString()
                    );
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
            System.Windows.MessageBox.Show("Изменения сохранены в базе данных!",
                "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.Close();
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

        private void AddProject_Appendix_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
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

        private void Matching_List_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null &&
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]))
            {
                e.CanExecute = true;
            }
        }

        private void Matching_List_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Согласование
            Transfer_Project_Viewer.Ds.Tables["APPROVAL"].Clear();
            OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                Connect.Schema), Connect.CurConnect);
            _daOrder_Hire.SelectCommand.BindByName = true;
            _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((DataRowView)this.DataContext)["PROJECT_TRANSFER_ID"];
            _daOrder_Hire.Fill(Transfer_Project_Viewer.Ds.Tables["APPROVAL"]);

            ReportViewerWindow report = new ReportViewerWindow(
                "Лист согласования", "Reports/Statement_Matching_List.rdlc", Transfer_Project_Viewer.Ds,
                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
            );
            report.Show();
        }
    }
}