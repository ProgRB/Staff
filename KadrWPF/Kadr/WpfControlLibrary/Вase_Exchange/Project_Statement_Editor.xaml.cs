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
using WpfControlLibrary.Classes;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Resume_Editor.xaml
    /// </summary>
    public partial class Project_Statement_Editor : Window, INotifyPropertyChanged
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
        static OracleCommand _ocGet_Status_Project, _ocPhotoEmp,
            _ocProject_To_Transfer;        
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
        public Project_Statement_Editor(DataRowView dataContext)
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
            cbTO_SUBDIV_ID.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;  
            cbBASE_DOC_ID.ItemsSource = ProjectDataSet.Tables["BASE_DOC"].DefaultView;

            _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.Clear();
            _daProject_Approval.SelectCommand.Parameters["p_PROJECT_STATEMENT_ID"].Value = dataContext["PROJECT_STATEMENT_ID"];
            _daProject_Approval.Fill(_dsProject_Approval.Tables["PROJECT_APPROVAL"]);
            dgProject_Approval.DataContext = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView;

            dcPROJECT_PLAN_APPROVAL_ID.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;

            cbTYPE_APPROVAL_ID.ItemsSource = _dsProject_Approval.Tables["TYPE_APPROVAL"].DefaultView;

            _dsProject_Approval.Tables["PLAN_APPROVAL"].Clear();
            _daPlan_Approval.SelectCommand.Parameters["p_PROJECT_PLAN_APPROVAL_ID"].Value = dataContext["PROJECT_PLAN_APPROVAL_ID"];
            _daPlan_Approval.Fill(_dsProject_Approval.Tables["PLAN_APPROVAL"]);

            // Команда возвращает статус проекта
            _ocGet_Status_Project.Parameters["p_PROJECT_STATEMENT_ID"].Value = dataContext["PROJECT_STATEMENT_ID"];

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

            _daAppendix.SelectCommand.Parameters["p_PROJECT_STATEMENT_ID"].Value = dataContext["PROJECT_STATEMENT_ID"];            
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

        static Project_Statement_Editor()
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
            _daProject_Approval.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectProject_Statement_Approval.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Approval.SelectCommand.BindByName = true;
            _daProject_Approval.SelectCommand.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
            // Insert
            _daProject_Approval.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PROJECT_ST_APPROVAL_UPDATE(:PROJECT_STATEMENT_APPROVAL_ID,:PROJECT_STATEMENT_ID,:PROJECT_PLAN_APPROVAL_ID,:DATE_APPROVAL,:NOTE_APPROVAL,
                        :USER_NAME,:TYPE_APPROVAL_ID,:USER_FIO);
                END;", Connect.Schema), Connect.CurConnect);
            _daProject_Approval.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProject_Approval.InsertCommand.BindByName = true;
            _daProject_Approval.InsertCommand.Parameters.Add("PROJECT_STATEMENT_APPROVAL_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_APPROVAL_ID").Direction = 
                ParameterDirection.InputOutput;
            _daProject_Approval.InsertCommand.Parameters["PROJECT_STATEMENT_APPROVAL_ID"].DbType = DbType.Decimal;
            _daProject_Approval.InsertCommand.Parameters.Add("PROJECT_STATEMENT_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_ID");
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
            _daType_Approval.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectType_Approval_By_Statement.sql"),
                Connect.Schema), Connect.CurConnect);
            _daType_Approval.SelectCommand.BindByName = true;
            _daType_Approval.SelectCommand.Parameters.Add("p_PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal);

            _daPlan_Approval.SelectCommand = new OracleCommand(string.Format(
                @"select PROJECT_PLAN_APPROVAL_ID,PROJECT_PLAN_APPROVAL_ID_PRIOR from {0}.PROJECT_PLAN_APPROVAL
                where PROJECT_PLAN_APPROVAL_ID=:p_PROJECT_PLAN_APPROVAL_ID",
                Connect.Schema), Connect.CurConnect);
            _daPlan_Approval.SelectCommand.BindByName = true;
            _daPlan_Approval.SelectCommand.Parameters.Add("p_PROJECT_PLAN_APPROVAL_ID", OracleDbType.Decimal);

            _ocGet_Status_Project = new OracleCommand(string.Format(
                @"SELECT NVL((select NVL(PROJECT_PLAN_APPROVAL_ID,0) from {0}.PROJECT_STATEMENT
                                WHERE PROJECT_STATEMENT_ID = :p_PROJECT_STATEMENT_ID),0) from dual",
                Connect.Schema), Connect.CurConnect);
            _ocGet_Status_Project.BindByName = true;
            _ocGet_Status_Project.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
            
            _ocPhotoEmp = new OracleCommand(string.Format(
                "begin SELECT (select E.PHOTO from {0}.EMP E where PER_NUM = :p_PER_NUM) into :p_PHOTO from dual; end;", 
                Connect.Schema), Connect.CurConnect);
            _ocPhotoEmp.BindByName = true;
            _ocPhotoEmp.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _ocPhotoEmp.Parameters.Add("p_PHOTO", OracleDbType.Blob).Direction = ParameterDirection.Output;
            
            // Формирование перевода в основной базе
            _ocProject_To_Transfer = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_STATEMENT_TO_PT(:p_PROJECT_STATEMENT_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _ocProject_To_Transfer.BindByName = true;
            _ocProject_To_Transfer.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);

            _daAppendix.SelectCommand = new OracleCommand(string.Format(
                @"SELECT PROJECT_STATEMENT_APPENDIX_ID, NOTE_DOCUMENT, PROJECT_STATEMENT_ID 
                FROM {0}.PROJECT_STATEMENT_APPENDIX WHERE PROJECT_STATEMENT_ID = :p_PROJECT_STATEMENT_ID",
                Connect.Schema), Connect.CurConnect);
            _daAppendix.SelectCommand.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
            // Insert
            _daAppendix.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_ST_APPENDIX_UPDATE(:PROJECT_STATEMENT_APPENDIX_ID,:NOTE_DOCUMENT,:DOCUMENT,:PROJECT_STATEMENT_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.InsertCommand.BindByName = true;
            _daAppendix.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daAppendix.InsertCommand.Parameters.Add("PROJECT_STATEMENT_APPENDIX_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_APPENDIX_ID").Direction = 
                ParameterDirection.InputOutput;
            _daAppendix.InsertCommand.Parameters["PROJECT_STATEMENT_APPENDIX_ID"].DbType = DbType.Decimal;
            _daAppendix.InsertCommand.Parameters.Add("NOTE_DOCUMENT", OracleDbType.Varchar2, 0, "NOTE_DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("DOCUMENT", OracleDbType.Blob, 0, "DOCUMENT");
            _daAppendix.InsertCommand.Parameters.Add("PROJECT_STATEMENT_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_ID");
            // Update
            _daAppendix.UpdateCommand = _daAppendix.InsertCommand;
            // Delete
            _daAppendix.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PROJECT_ST_APPENDIX_DELETE(:PROJECT_STATEMENT_APPENDIX_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daAppendix.DeleteCommand.BindByName = true;
            _daAppendix.DeleteCommand.Parameters.Add("PROJECT_STATEMENT_APPENDIX_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_APPENDIX_ID");
        }

        private void SaveDismiss_Project_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                this.DataContext != null && 
                ((DataRowView)(this.DataContext)).DataView.Table.GetChanges() != null)
            {
                e.CanExecute = Array.TrueForAll<DependencyObject>(gridDismiss.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
            }
        }

        private void SaveDismiss_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Project_Statement_Viewer.SaveProject())
            {
                SaveTransfer_QM();
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
                this.DataContext != null && 
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
            //if (Convert.ToBoolean(_ocGet_Sign_Open_Approval.ExecuteScalar()))
            {
                _daType_Approval.SelectCommand.Parameters["p_PROJECT_PLAN_APPROVAL_ID"].Value =
                    _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"];
                _daType_Approval.Fill(_dsProject_Approval.Tables["TYPE_APPROVAL"]);
                // После заполнения типов решений, смотрим доступно ли хоть одно решение. 
                // Если доступно, то добавляем новую запись чтобы пользователю осталось нажать лишь кнопку сохранить
                if (_dsProject_Approval.Tables["TYPE_APPROVAL"].DefaultView.Count > 0)
                {
                    Fl_Add_Approval = true;
                    _dsProject_Approval.Tables["PROJECT_APPROVAL"].RejectChanges();
                    _row_Approval = _dsProject_Approval.Tables["PROJECT_APPROVAL"].DefaultView.AddNew();
                    _row_Approval["PROJECT_STATEMENT_ID"] = ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
                    _row_Approval["PROJECT_PLAN_APPROVAL_ID"] =
                        _dsProject_Approval.Tables["PLAN_APPROVAL"].DefaultView[0]["PROJECT_PLAN_APPROVAL_ID_PRIOR"];
                    _dsProject_Approval.Tables["PROJECT_APPROVAL"].Rows.InsertAt(_row_Approval.Row, 0);
                    dgProject_Approval.SelectedItem = _row_Approval;
                }
            }
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
            Project_Statement_Viewer.Ds.Tables["APPROVAL"].Clear();
            OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForStatement.sql"),
                Connect.Schema), Connect.CurConnect);
            _daOrder_Hire.SelectCommand.BindByName = true;
            _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal).Value =
                ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
            _daOrder_Hire.Fill(Project_Statement_Viewer.Ds.Tables["APPROVAL"]);

            ReportViewerWindow report = new ReportViewerWindow(
                "Лист согласования заявления", "Reports/Statement_Matching_List.rdlc", Project_Statement_Viewer.Ds,
                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
            );
            report.Show();

            OracleCommand _updateSIGN_PRINT_MATCHING = new OracleCommand(string.Format(
                @"UPDATE {0}.PROJECT_STATEMENT SET SIGN_PRINT_MATCHING = 1 WHERE PROJECT_STATEMENT_ID = :p_PROJECT_STATEMENT_ID", Connect.Schema), Connect.CurConnect);
            _updateSIGN_PRINT_MATCHING.BindByName = true;
            _updateSIGN_PRINT_MATCHING.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal).Value =
                ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _updateSIGN_PRINT_MATCHING.Transaction = transact;
                _updateSIGN_PRINT_MATCHING.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show("Ошибка установки признака печати листа согласования:\n"+ex.Message, 
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Project_To_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _dsDataTransfer != null && !_dsDataTransfer.HasChanges() &&
                this.DataContext != null && 
                Convert.ToBoolean(((DataRowView)this.DataContext)["SIGN_FULL_APPROVAL"]))
            {
                e.CanExecute = true;
            }
        }

        private void Project_To_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Вы действительно хотите провести заявление в Обменную БД?",
                    "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ocProject_To_Transfer.Parameters["p_PROJECT_STATEMENT_ID"].Value = ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
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
                    System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка проведения заявления", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }            
            }
            System.Windows.MessageBox.Show("Изменения сохранены в базе данных!",
                "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.Close();
        }
        
        private void AddProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRow = _dsAppendix.Tables["PROJECT_APPENDIX"].DefaultView.AddNew();
            _currentRow["PROJECT_STATEMENT_ID"] = ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
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
                "begin SELECT NVL((select DOCUMENT from {0}.PROJECT_STATEMENT_APPENDIX where PROJECT_STATEMENT_APPENDIX_ID = :p_PROJECT_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocSelectDocument.BindByName = true;
            _ocSelectDocument.Parameters.Add("p_PROJECT_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["PROJECT_STATEMENT_APPENDIX_ID"];
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

        private void AddProject_Appendix_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void ViewProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgAppendix.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            //Appendix_Editor appEditor = new Appendix_Editor(rowSelected);
            OracleCommand _ocSelectDocument = new OracleCommand(string.Format(
                "begin SELECT NVL((select DOCUMENT from {0}.PROJECT_STATEMENT_APPENDIX where PROJECT_STATEMENT_APPENDIX_ID = :p_PROJECT_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocSelectDocument.BindByName = true;
            _ocSelectDocument.Parameters.Add("p_PROJECT_APPENDIX_ID", OracleDbType.Decimal).Value = rowSelected["PROJECT_STATEMENT_APPENDIX_ID"];
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
        
        void SaveTransfer_QM()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                OracleCommand _ocUpdateTransfer_QM = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TRANSFER_QM_UPDATE2(:TRANSFER_ID,:PROJECT_TRANSFER_ID,:PROJECT_STATEMENT_ID,:SIGN_TRANSFER_QM);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocUpdateTransfer_QM.BindByName = true;
                _ocUpdateTransfer_QM.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
                _ocUpdateTransfer_QM.Parameters.Add("PROJECT_TRANSFER_ID", OracleDbType.Decimal, 0, "PROJECT_TRANSFER_ID");
                _ocUpdateTransfer_QM.Parameters.Add("PROJECT_STATEMENT_ID", OracleDbType.Decimal, 0, "PROJECT_STATEMENT_ID").Value =
                    ((DataRowView)this.DataContext)["PROJECT_STATEMENT_ID"];
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
    }
}