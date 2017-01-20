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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using Kadr;
using System.Windows.Interop;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Outside_Emp_Viewer.xaml
    /// </summary>
    public partial class Outside_Emp_Viewer : UserControl
    {
        private static DataSet _ds = new DataSet();
        private static OracleDataAdapter _daOutside_Emp = new OracleDataAdapter(), _daOutside_Transfer = new OracleDataAdapter(),
            _daPer_Data = new OracleDataAdapter(), 
            _daPassport = new OracleDataAdapter(), _daRegistr = new OracleDataAdapter(), _daType_Per_Doc = new OracleDataAdapter();
        DataRowView _currentRow;
        public Outside_Emp_Viewer()
        {
            InitializeComponent();

            dgOutside_Emp.DataContext = _ds.Tables["OUTSIDE_EMP"].DefaultView;

            dgOutside_Transfer.DataContext = _ds.Tables["OUTSIDE_TRANSFER"].DefaultView;

            dcDegree.ItemsSource = _ds.Tables["DEGREE"].DefaultView;

            GetOutside_Emp();
        }

        static Outside_Emp_Viewer()
        {
            _ds = new DataSet();
            _ds.Tables.Add("OUTSIDE_EMP");
            _ds.Tables.Add("OUTSIDE_TRANSFER");
            // Select
            _daOutside_Emp.SelectCommand = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("SelectOutside_Emp.sql"),
                Connect.Schema), Connect.CurConnect);
            _daOutside_Emp.SelectCommand.BindByName = true;
            _daOutside_Emp.SelectCommand.Parameters.Add("p_EMP_LAST_NAME", OracleDbType.Varchar2);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_EMP_FIRST_NAME", OracleDbType.Varchar2);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_EMP_MIDDLE_NAME", OracleDbType.Varchar2);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_begin_period", OracleDbType.Date);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_end_period", OracleDbType.Date);
            _daOutside_Emp.SelectCommand.Parameters.Add("p_SIGN_STUDENT", OracleDbType.Decimal);
            // Insert
            _daOutside_Emp.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.EMP(PER_NUM, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME, EMP_SEX, EMP_BIRTH_DATE, PERCO_SYNC_ID)
                VALUES(:PER_NUM, :EMP_LAST_NAME, :EMP_FIRST_NAME, :EMP_MIDDLE_NAME, :EMP_SEX, :EMP_BIRTH_DATE, :PERCO_SYNC_ID)",
                Connect.Schema), Connect.CurConnect);
            _daOutside_Emp.InsertCommand.BindByName = true;
            _daOutside_Emp.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daOutside_Emp.InsertCommand.Parameters.Add("EMP_LAST_NAME", OracleDbType.Varchar2, 0, "EMP_LAST_NAME");
            _daOutside_Emp.InsertCommand.Parameters.Add("EMP_FIRST_NAME", OracleDbType.Varchar2, 0, "EMP_FIRST_NAME");
            _daOutside_Emp.InsertCommand.Parameters.Add("EMP_MIDDLE_NAME", OracleDbType.Varchar2, 0, "EMP_MIDDLE_NAME");
            _daOutside_Emp.InsertCommand.Parameters.Add("EMP_SEX", OracleDbType.Varchar2, 0, "EMP_SEX");
            _daOutside_Emp.InsertCommand.Parameters.Add("EMP_BIRTH_DATE", OracleDbType.Date, 0, "EMP_BIRTH_DATE");
            _daOutside_Emp.InsertCommand.Parameters.Add("PERCO_SYNC_ID", OracleDbType.Decimal, 0, "PERCO_SYNC_ID");
            // Update
            _daOutside_Emp.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.EMP SET EMP_LAST_NAME=:EMP_LAST_NAME, EMP_FIRST_NAME=:EMP_FIRST_NAME, EMP_MIDDLE_NAME=:EMP_MIDDLE_NAME, 
                    EMP_SEX=:EMP_SEX, EMP_BIRTH_DATE=:EMP_BIRTH_DATE
                WHERE PER_NUM = :PER_NUM", Connect.Schema),
                Connect.CurConnect);
            _daOutside_Emp.UpdateCommand.BindByName = true;
            _daOutside_Emp.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daOutside_Emp.UpdateCommand.Parameters.Add("EMP_LAST_NAME", OracleDbType.Varchar2, 0, "EMP_LAST_NAME");
            _daOutside_Emp.UpdateCommand.Parameters.Add("EMP_FIRST_NAME", OracleDbType.Varchar2, 0, "EMP_FIRST_NAME");
            _daOutside_Emp.UpdateCommand.Parameters.Add("EMP_MIDDLE_NAME", OracleDbType.Varchar2, 0, "EMP_MIDDLE_NAME");
            _daOutside_Emp.UpdateCommand.Parameters.Add("EMP_SEX", OracleDbType.Varchar2, 0, "EMP_SEX");
            _daOutside_Emp.UpdateCommand.Parameters.Add("EMP_BIRTH_DATE", OracleDbType.Date, 0, "EMP_BIRTH_DATE");
            // Delete
            _daOutside_Emp.DeleteCommand = new OracleCommand(string.Format(
                @"delete from {0}.EMP where PER_NUM = :PER_NUM", Connect.Schema), Connect.CurConnect);
            _daOutside_Emp.DeleteCommand.BindByName = true;
            _daOutside_Emp.DeleteCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM"); 

            // Select
            _daOutside_Transfer.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("SelectOutside_Transfer.sql"),
                Connect.Schema), Connect.CurConnect);
            _daOutside_Transfer.SelectCommand.BindByName = true;
            _daOutside_Transfer.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daOutside_Transfer.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.TRANSFER(TRANSFER_ID, PER_NUM, SUBDIV_ID, POS_ID, TYPE_TRANSFER_ID, GR_WORK_ID, 
                    DATE_HIRE, DATE_TRANSFER, DATE_END_CONTR, WORKER_ID, DEGREE_ID)
                VALUES(:TRANSFER_ID, :PER_NUM, :SUBDIV_ID, 0, 7, 0, 
                    :DATE_HIRE, :DATE_TRANSFER, :DATE_END_CONTR, :WORKER_ID, :DEGREE_ID)", Connect.Schema), Connect.CurConnect);
            _daOutside_Transfer.InsertCommand.BindByName = true;
            _daOutside_Transfer.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daOutside_Transfer.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daOutside_Transfer.InsertCommand.Parameters.Add("SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID");
            _daOutside_Transfer.InsertCommand.Parameters.Add("DATE_HIRE", OracleDbType.Date, 0, "DATE_HIRE");
            _daOutside_Transfer.InsertCommand.Parameters.Add("DATE_TRANSFER", OracleDbType.Date, 0, "DATE_TRANSFER");
            _daOutside_Transfer.InsertCommand.Parameters.Add("DATE_END_CONTR", OracleDbType.Date, 0, "DATE_END_CONTR");
            _daOutside_Transfer.InsertCommand.Parameters.Add("WORKER_ID", OracleDbType.Decimal, 0, "WORKER_ID");
            _daOutside_Transfer.InsertCommand.Parameters.Add("DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID");
            // Update
            _daOutside_Transfer.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.TRANSFER SET DATE_HIRE=:DATE_HIRE, DATE_TRANSFER=:DATE_TRANSFER, DATE_END_CONTR=:DATE_END_CONTR, DEGREE_ID=:DEGREE_ID
                WHERE PER_NUM = :PER_NUM", Connect.Schema), Connect.CurConnect);
            _daOutside_Transfer.UpdateCommand.BindByName = true;
            _daOutside_Transfer.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daOutside_Transfer.UpdateCommand.Parameters.Add("DATE_HIRE", OracleDbType.Date, 0, "DATE_HIRE");
            _daOutside_Transfer.UpdateCommand.Parameters.Add("DATE_TRANSFER", OracleDbType.Date, 0, "DATE_TRANSFER");
            _daOutside_Transfer.UpdateCommand.Parameters.Add("DATE_END_CONTR", OracleDbType.Date, 0, "DATE_END_CONTR");
            _daOutside_Transfer.UpdateCommand.Parameters.Add("DEGREE_ID", OracleDbType.Decimal, 0, "DEGREE_ID");
            // Delete
            _daOutside_Transfer.DeleteCommand = new OracleCommand(string.Format(
                "DELETE {0}.TRANSFER WHERE TRANSFER_ID = :TRANSFER_ID", Connect.Schema), Connect.CurConnect);
            _daOutside_Transfer.DeleteCommand.BindByName = true;
            _daOutside_Transfer.DeleteCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");

            _ds.Tables.Add("PER_DATA");
            _ds.Tables.Add("PASSPORT");
            _ds.Tables.Add("REGISTR");
            // Select
            _daPer_Data.SelectCommand = new OracleCommand(string.Format(
                "select PER_NUM, INSURANCE_NUM, INN from {0}.PER_DATA where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPer_Data.SelectCommand.BindByName = true;
            _daPer_Data.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daPer_Data.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.PER_DATA(PER_NUM, TRIP_SIGN, RETIRER_SIGN, SIGN_PROFUNION, SIGN_YOUNG_SPEC, INSURANCE_NUM, INN)
                VALUES(:PER_NUM, 0, 0, 0, 0, :INSURANCE_NUM, :INN)", Connect.Schema), Connect.CurConnect);
            _daPer_Data.InsertCommand.BindByName = true;
            _daPer_Data.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPer_Data.InsertCommand.Parameters.Add("INSURANCE_NUM", OracleDbType.Varchar2, 0, "INSURANCE_NUM");
            _daPer_Data.InsertCommand.Parameters.Add("INN", OracleDbType.Varchar2, 0, "INN");
            // Update
            _daPer_Data.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.PER_DATA SET INSURANCE_NUM=:INSURANCE_NUM, INN=:INN WHERE PER_NUM=:PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPer_Data.UpdateCommand.BindByName = true;
            _daPer_Data.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPer_Data.UpdateCommand.Parameters.Add("INSURANCE_NUM", OracleDbType.Varchar2, 0, "INSURANCE_NUM");
            _daPer_Data.UpdateCommand.Parameters.Add("INN", OracleDbType.Varchar2, 0, "INN");

            // Select
            _daPassport.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, SERIA_PASSPORT,NUM_PASSPORT,WHO_GIVEN,WHEN_GIVEN,CITIZENSHIP,TYPE_PER_DOC_ID 
                from {0}.PASSPORT where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPassport.SelectCommand.BindByName = true;
            _daPassport.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daPassport.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.PASSPORT(PER_NUM, SERIA_PASSPORT,NUM_PASSPORT,WHO_GIVEN,WHEN_GIVEN,CITIZENSHIP,TYPE_PER_DOC_ID)
                VALUES(:PER_NUM, :SERIA_PASSPORT,:NUM_PASSPORT,:WHO_GIVEN,:WHEN_GIVEN,:CITIZENSHIP,:TYPE_PER_DOC_ID)", Connect.Schema), Connect.CurConnect);
            _daPassport.InsertCommand.BindByName = true;
            _daPassport.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPassport.InsertCommand.Parameters.Add("SERIA_PASSPORT", OracleDbType.Varchar2, 0, "SERIA_PASSPORT");
            _daPassport.InsertCommand.Parameters.Add("NUM_PASSPORT", OracleDbType.Varchar2, 0, "NUM_PASSPORT");
            _daPassport.InsertCommand.Parameters.Add("WHO_GIVEN", OracleDbType.Varchar2, 0, "WHO_GIVEN");
            _daPassport.InsertCommand.Parameters.Add("WHEN_GIVEN", OracleDbType.Date, 0, "WHEN_GIVEN");
            _daPassport.InsertCommand.Parameters.Add("CITIZENSHIP", OracleDbType.Varchar2, 0, "CITIZENSHIP");
            _daPassport.InsertCommand.Parameters.Add("TYPE_PER_DOC_ID", OracleDbType.Decimal, 0, "TYPE_PER_DOC_ID");
            // Update
            _daPassport.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.PASSPORT SET SERIA_PASSPORT=:SERIA_PASSPORT, NUM_PASSPORT=:NUM_PASSPORT, WHO_GIVEN=:WHO_GIVEN, 
                    WHEN_GIVEN=:WHEN_GIVEN, CITIZENSHIP=:CITIZENSHIP, TYPE_PER_DOC_ID=:TYPE_PER_DOC_ID 
                WHERE PER_NUM=:PER_NUM", Connect.Schema), Connect.CurConnect);
            _daPassport.UpdateCommand.BindByName = true;
            _daPassport.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPassport.UpdateCommand.Parameters.Add("SERIA_PASSPORT", OracleDbType.Varchar2, 0, "SERIA_PASSPORT");
            _daPassport.UpdateCommand.Parameters.Add("NUM_PASSPORT", OracleDbType.Varchar2, 0, "NUM_PASSPORT");
            _daPassport.UpdateCommand.Parameters.Add("WHO_GIVEN", OracleDbType.Varchar2, 0, "WHO_GIVEN");
            _daPassport.UpdateCommand.Parameters.Add("WHEN_GIVEN", OracleDbType.Date, 0, "WHEN_GIVEN");
            _daPassport.UpdateCommand.Parameters.Add("CITIZENSHIP", OracleDbType.Varchar2, 0, "CITIZENSHIP");
            _daPassport.UpdateCommand.Parameters.Add("TYPE_PER_DOC_ID", OracleDbType.Decimal, 0, "TYPE_PER_DOC_ID");

            // Select
            _daRegistr.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, REG_CODE_STREET, REG_HOUSE, REG_BULK, REG_FLAT, REG_POST_CODE, DATE_REG, REG_PHONE 
                from {0}.REGISTR where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daRegistr.SelectCommand.BindByName = true;
            _daRegistr.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daRegistr.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.REGISTR(PER_NUM, REG_CODE_STREET, REG_HOUSE, REG_BULK, REG_FLAT, REG_POST_CODE, DATE_REG, REG_PHONE)
                VALUES(:PER_NUM, :REG_CODE_STREET, :REG_HOUSE, :REG_BULK, :REG_FLAT, :REG_POST_CODE, :DATE_REG, :REG_PHONE)", Connect.Schema), Connect.CurConnect);
            _daRegistr.InsertCommand.BindByName = true;
            _daRegistr.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daRegistr.InsertCommand.Parameters.Add("REG_CODE_STREET", OracleDbType.Varchar2, 0, "REG_CODE_STREET");
            _daRegistr.InsertCommand.Parameters.Add("REG_HOUSE", OracleDbType.Varchar2, 0, "REG_HOUSE");
            _daRegistr.InsertCommand.Parameters.Add("REG_BULK", OracleDbType.Varchar2, 0, "REG_BULK");
            _daRegistr.InsertCommand.Parameters.Add("REG_FLAT", OracleDbType.Varchar2, 0, "REG_FLAT");
            _daRegistr.InsertCommand.Parameters.Add("REG_POST_CODE", OracleDbType.Varchar2, 0, "REG_POST_CODE");
            _daRegistr.InsertCommand.Parameters.Add("DATE_REG", OracleDbType.Date, 0, "DATE_REG");
            _daRegistr.InsertCommand.Parameters.Add("REG_PHONE", OracleDbType.Varchar2, 0, "REG_PHONE");
            // Update
            _daRegistr.UpdateCommand = new OracleCommand(string.Format(
                @"UPDATE {0}.REGISTR SET REG_CODE_STREET=:REG_CODE_STREET, REG_HOUSE=:REG_HOUSE, REG_BULK=:REG_BULK, REG_FLAT=:REG_FLAT, 
                    REG_POST_CODE=:REG_POST_CODE, DATE_REG=:DATE_REG, REG_PHONE=:REG_PHONE
                WHERE PER_NUM=:PER_NUM", Connect.Schema), Connect.CurConnect);
            _daRegistr.UpdateCommand.BindByName = true;
            _daRegistr.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daRegistr.UpdateCommand.Parameters.Add("REG_CODE_STREET", OracleDbType.Varchar2, 0, "REG_CODE_STREET");
            _daRegistr.UpdateCommand.Parameters.Add("REG_HOUSE", OracleDbType.Varchar2, 0, "REG_HOUSE");
            _daRegistr.UpdateCommand.Parameters.Add("REG_BULK", OracleDbType.Varchar2, 0, "REG_BULK");
            _daRegistr.UpdateCommand.Parameters.Add("REG_FLAT", OracleDbType.Varchar2, 0, "REG_FLAT");
            _daRegistr.UpdateCommand.Parameters.Add("REG_POST_CODE", OracleDbType.Varchar2, 0, "REG_POST_CODE");
            _daRegistr.UpdateCommand.Parameters.Add("DATE_REG", OracleDbType.Date, 0, "DATE_REG");
            _daRegistr.UpdateCommand.Parameters.Add("REG_PHONE", OracleDbType.Varchar2, 0, "REG_PHONE");

            _ds.Tables.Add("TYPE_PD");
            _daType_Per_Doc.SelectCommand = new OracleCommand(string.Format(
                "select TYPE_PER_DOC_ID, NAME_DOC, TEMPL_SER, TEMPL_NUM from {0}.TYPE_PER_DOC order by NAME_DOC", Connect.Schema), Connect.CurConnect);
            _daType_Per_Doc.Fill(_ds.Tables["TYPE_PD"]);

            _ds.Tables.Add("DEGREE");
            new OracleDataAdapter(string.Format(
                "select D.DEGREE_ID, D.CODE_DEGREE, D.DEGREE_NAME from {0}.DEGREE D order by D.CODE_DEGREE", Connect.Schema),
                Connect.CurConnect).Fill(_ds.Tables["DEGREE"]);
            _ds.Tables["DEGREE"].Columns.Add("DISP_DEGREE").Expression = "CODE_DEGREE+' ('+DEGREE_NAME+')'";              
        }

        private void GetOutside_Emp()
        {
            dgOutside_Emp.DataContext = null;
            _ds.Tables["OUTSIDE_EMP"].Clear();
            _ds.Tables["OUTSIDE_EMP"].DefaultView.RowFilter = "";
            _daOutside_Emp.Fill(_ds.Tables["OUTSIDE_EMP"]);
            dgOutside_Emp.DataContext = _ds.Tables["OUTSIDE_EMP"].DefaultView;
        }

        private void GetOutside_Transfer(string cur_per_num)
        {
            _ds.Tables["OUTSIDE_TRANSFER"].Clear();
            _daOutside_Transfer.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daOutside_Transfer.Fill(_ds.Tables["OUTSIDE_TRANSFER"]);
        }

        private void btFilter_Apply_Click(object sender, RoutedEventArgs e)
        {
            _daOutside_Emp.SelectCommand.Parameters["p_begin_period"].Value = dpPeriodBegin.SelectedDate;
            _daOutside_Emp.SelectCommand.Parameters["p_end_period"].Value = dpPeriodEnd.SelectedDate;
            _daOutside_Emp.SelectCommand.Parameters["p_EMP_LAST_NAME"].Value = tbEmp_Last_name.Text;
            _daOutside_Emp.SelectCommand.Parameters["p_EMP_FIRST_NAME"].Value = tbEmp_First_name.Text;
            _daOutside_Emp.SelectCommand.Parameters["p_EMP_MIDDLE_NAME"].Value = tbEmp_Middle_name.Text;
            _daOutside_Emp.SelectCommand.Parameters["p_PER_NUM"].Value = tbPer_num.Text;
            _daOutside_Emp.SelectCommand.Parameters["p_SIGN_STUDENT"].Value = chSign_Student.IsChecked;

            GetOutside_Emp();
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            dpPeriodBegin.SelectedDate = null;
            dpPeriodEnd.SelectedDate = null;
            tbEmp_Last_name.Text = "";
            tbEmp_First_name.Text = "";
            tbEmp_Middle_name.Text = "";
            tbPer_num.Text = "";
            chSign_Student.IsChecked = null;
            btFilter_Apply_Click(sender, e);
        }

        private void dgOutside_Emp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wpf_Commands.EditOutside_Emp.Execute(null, null);
        }

        private void AddOutside_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddOutside_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выбираем новый Таб.№
            OracleCommand ocNewPer_Num = new OracleCommand(string.Format(
                "BEGIN {0}.GET_NEW_PER_NUM(:p_NEW_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
            ocNewPer_Num.BindByName = true;
            ocNewPer_Num.Parameters.Add("p_NEW_PER_NUM", OracleDbType.Varchar2, 10).Direction = ParameterDirection.InputOutput;
            ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value = "79000";
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                ocNewPer_Num.Transaction = transact;
                ocNewPer_Num.ExecuteNonQuery();
                transact.Commit();
                DataRowView _currentEmp = _ds.Tables["OUTSIDE_EMP"].DefaultView.AddNew();
                _currentEmp["PER_NUM"] = ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value;
                _ds.Tables["OUTSIDE_EMP"].Rows.Add(_currentEmp.Row);
                dgOutside_Emp.SelectedItem = _currentEmp;

                RefreshPersonData(_currentEmp["PER_NUM"].ToString());

                _ds.Tables["PER_DATA"].Rows.Add(_ds.Tables["PER_DATA"].DefaultView.AddNew().Row);
                _ds.Tables["PER_DATA"].DefaultView[0]["PER_NUM"] = _currentEmp["PER_NUM"];
                _ds.Tables["PASSPORT"].Rows.Add(_ds.Tables["PASSPORT"].DefaultView.AddNew().Row);
                _ds.Tables["PASSPORT"].DefaultView[0]["PER_NUM"] = _currentEmp["PER_NUM"];
                _ds.Tables["REGISTR"].Rows.Add(_ds.Tables["REGISTR"].DefaultView.AddNew().Row);
                _ds.Tables["REGISTR"].DefaultView[0]["PER_NUM"] = _currentEmp["PER_NUM"];
                Outside_Emp_Editor editor = new Outside_Emp_Editor(true, _currentEmp, _ds);
                editor.Owner = Window.GetWindow(this);
                //editor.Owner = Window.GetWindow(this);
                if (editor.ShowDialog() == true)
                {
                    SavePersonData();
                }
                else
                {
                    // Возвращаем флаг свободного табельного
                    OracleCommand ocUpdatePer_Num_Book = new OracleCommand(string.Format(
                        "UPDATE {0}.PER_NUM_BOOK SET FREE_SIGN = 1 WHERE PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
                    ocUpdatePer_Num_Book.BindByName = true;
                    ocUpdatePer_Num_Book.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _currentEmp["PER_NUM"];
                    OracleTransaction transact1 = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        ocUpdatePer_Num_Book.Transaction = transact1;
                        ocUpdatePer_Num_Book.ExecuteNonQuery();
                        transact1.Commit();
                    }
                    catch (Exception ex1)
                    {
                        transact1.Rollback();
                        MessageBox.Show("Ошибка обновления признака свободного табельного!\n\n" + ex1.Message, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    _ds.RejectChanges();
                }
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show("Ошибка добавления нового сотрудника!\n\n" + ex.Message, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }

        void RefreshPersonData(string cur_per_num)
        {
            // Очищаем таблицы перед заполнением
            _ds.Tables["PER_DATA"].Clear();
            _ds.Tables["PASSPORT"].Clear();
            _ds.Tables["REGISTR"].Clear();
            // Fill
            _daPer_Data.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daPassport.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daRegistr.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daPer_Data.Fill(_ds.Tables["PER_DATA"]);
            _daPassport.Fill(_ds.Tables["PASSPORT"]);
            _daRegistr.Fill(_ds.Tables["REGISTR"]);
        }

        void SavePersonData()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {

                DataViewRowState rs = _ds.Tables["OUTSIDE_EMP"].DefaultView.RowStateFilter;
                _ds.Tables["OUTSIDE_EMP"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["OUTSIDE_EMP"].DefaultView.Count; ++i)
                {
                    _ds.Tables["OUTSIDE_EMP"].DefaultView[i]["PERCO_SYNC_ID"] =
                        new OracleCommand(string.Format("select {0}.PERCO_SYNC_ID_SEQ.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                }
                _ds.Tables["OUTSIDE_EMP"].DefaultView.RowStateFilter = rs;
                _daOutside_Emp.InsertCommand.Transaction = transact;
                _daOutside_Emp.UpdateCommand.Transaction = transact;
                _daOutside_Emp.DeleteCommand.Transaction = transact;
                _daOutside_Emp.Update(_ds.Tables["OUTSIDE_EMP"]);
                _daPer_Data.InsertCommand.Transaction = transact;
                _daPer_Data.UpdateCommand.Transaction = transact;
                _daPer_Data.Update(_ds.Tables["PER_DATA"]);
                _daPassport.InsertCommand.Transaction = transact;
                _daPassport.UpdateCommand.Transaction = transact;
                _daPassport.Update(_ds.Tables["PASSPORT"]);
                _daRegistr.InsertCommand.Transaction = transact;
                _daRegistr.UpdateCommand.Transaction = transact;
                _daRegistr.Update(_ds.Tables["REGISTR"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void EditOutside_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && 
                dgOutside_Emp != null && dgOutside_Emp.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditOutside_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentEmp = dgOutside_Emp.SelectedCells[0].Item as DataRowView;

            RefreshPersonData(_currentEmp["PER_NUM"].ToString());

            Outside_Emp_Editor editor = new Outside_Emp_Editor(false, _currentEmp, _ds);
            editor.Owner = Window.GetWindow(this);
            if (editor.ShowDialog() == true)
            {
                SavePersonData();
            }
            else
            {
                _ds.RejectChanges();
            }
        }

        private void DeleteOutside_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["OUTSIDE_TRANSFER"].Rows.Count == 0)
            {
                e.CanExecute = true;
            }
        }

        private void DeleteOutside_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Collections.IList rowArray = dgOutside_Emp.SelectedItems;
                while (dgOutside_Emp.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgOutside_Emp.SelectedCells[0].Item).Delete();
                }
                SaveOutside_Emp();
            }
            dgOutside_Emp.Focus();
        }

        private void SaveOutside_Emp()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daOutside_Emp.InsertCommand.Transaction = transact;
                _daOutside_Emp.UpdateCommand.Transaction = transact;
                _daOutside_Emp.DeleteCommand.Transaction = transact;
                _daOutside_Emp.Update(_ds.Tables["OUTSIDE_EMP"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void dgOutside_Emp_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgOutside_Emp.SelectedCells.Count > 0 &&
                _currentRow != dgOutside_Emp.SelectedCells[0].Item)
            {
                _currentRow = dgOutside_Emp.SelectedCells[0].Item as DataRowView;
                GetOutside_Transfer(_currentRow["PER_NUM"].ToString());
            }
        }

        private void AddOutside_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _currentRow != null && _ds.Tables["OUTSIDE_TRANSFER"].GetChanges() == null)
                e.CanExecute = true;
        }

        private void AddOutside_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentTrans = _ds.Tables["OUTSIDE_TRANSFER"].DefaultView.AddNew();
            _ds.Tables["OUTSIDE_TRANSFER"].Rows.Add(_currentTrans.Row);
            dgOutside_Transfer.SelectedItem = _currentTrans;
        }

        private void DeleteOutside_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
               dgOutside_Transfer != null && dgOutside_Transfer.SelectedCells.Count > 0 &&
               _ds.Tables["OUTSIDE_TRANSFER"].GetChanges() == null)
                e.CanExecute = true;
        }

        private void DeleteOutside_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (dgOutside_Transfer.SelectedCells.Count > 0 &&
                MessageBox.Show("Удалить запись?", "АРМ \"Начисление премии\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                (dgOutside_Transfer.SelectedCells[0].Item as DataRowView).Delete();
                SaveOutside_Transfer_Executed(sender, e);
            }
            dgOutside_Transfer.Focus();
        }

        private void SaveOutside_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && _ds.Tables["OUTSIDE_TRANSFER"].GetChanges() != null)
            {
                e.CanExecute = true;
            }
        }

        private void SaveOutside_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            dgOutside_Transfer.CommitEdit(DataGridEditingUnit.Row, true);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["OUTSIDE_TRANSFER"].Rows.Count; ++i)
                {
                    if (_ds.Tables["OUTSIDE_TRANSFER"].Rows[i].RowState != DataRowState.Deleted)
                    {
                        _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["DATE_HIRE"] = _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["DATE_TRANSFER"];
                        if (_ds.Tables["OUTSIDE_TRANSFER"].Rows[i].RowState == DataRowState.Added)
                        {
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["TRANSFER_ID"] =
                                new OracleCommand(string.Format(
                                    "select {0}.TRANSFER_ID_seq.NEXTVAL from dual", Connect.Schema), Connect.CurConnect).ExecuteScalar();
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["WORKER_ID"] =
                                new OracleCommand(string.Format(
                                    "select {0}.WORKER_ID_seq.NEXTVAL from dual", Connect.Schema), Connect.CurConnect).ExecuteScalar();
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["PER_NUM"] = _currentRow["PER_NUM"];
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["CODE_SUBDIV"] = "300";
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["SUBDIV_ID"] =
                                new OracleCommand(string.Format(
                                    "select SUBDIV_ID from {0}.SUBDIV where CODE_SUBDIV = '300' and SUB_ACTUAL_SIGN = 1", Connect.Schema), Connect.CurConnect).ExecuteScalar(); ;
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["POS_ID"] = 0;
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["TYPE_TRANSFER_ID"] = 7;
                            _ds.Tables["OUTSIDE_TRANSFER"].Rows[i]["GR_WORK_ID"] = 0;
                        }
                    }
                }
                _daOutside_Transfer.UpdateCommand.Transaction = transact;
                _daOutside_Transfer.InsertCommand.Transaction = transact;
                _daOutside_Transfer.DeleteCommand.Transaction = transact;
                _daOutside_Transfer.Update(_ds.Tables["OUTSIDE_TRANSFER"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АРМ \"Начисление премии\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void CancelOutside_Transfer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["OUTSIDE_TRANSFER"].GetChanges() != null)
            {
                e.CanExecute = true;
            }
        }

        private void CancelOutside_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить все несохраненные изменения?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ds.Tables["OUTSIDE_TRANSFER"].RejectChanges();
            }
        }

        private void dgOutside_Transfer_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
        }
    }
}
