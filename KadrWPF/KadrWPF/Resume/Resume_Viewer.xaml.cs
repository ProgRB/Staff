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
using Kadr;
using LibraryKadr;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Resume_Viewer.xaml
    /// </summary>
    public partial class Resume_Viewer : UserControl
    {
        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }
        private static OracleDataAdapter _daResume = new OracleDataAdapter(), 
            _daRegistr = new OracleDataAdapter(), _daHabit = new OracleDataAdapter();
        public Resume_Viewer()
        {
            InitializeComponent();

            dgResume.DataContext = _ds.Tables["RESUME"].DefaultView;

            cbSOURCE_EMPLOYABILITY_ID.ItemsSource = _ds.Tables["S_E"].DefaultView;
            cbSPEC_ID.ItemsSource = _ds.Tables["SPECIALITY"].DefaultView;
            cbINSTIT_ID.ItemsSource = _ds.Tables["INSTIT"].DefaultView;
            cbTYPE_EDU_ID.ItemsSource = _ds.Tables["TYPE_EDU"].DefaultView;
            cbQUAL_ID.ItemsSource = _ds.Tables["QUAL"].DefaultView;

            btFilter_Clear_Click(null, null);
        }

        static Resume_Viewer()
        {
            _ds = new DataSet();
            _ds.Tables.Add("RESUME");
            _ds.Tables.Add("REGISTR");
            _ds.Tables.Add("HABIT");
            _daResume.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Resume/SelectResume.sql"),
                Connect.Schema), Connect.CurConnect);
            _daResume.SelectCommand.BindByName = true;
            // Параметры выбора резюме - либо доступные, либо уже перешедшие в архив
            _daResume.SelectCommand.Parameters.Add("p_begin_period", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_end_period", OracleDbType.Date);
            // Параметры фильтрации
            _daResume.SelectCommand.Parameters.Add("p_EMP_LAST_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_FIRST_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_MIDDLE_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_SEX", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_BEGIN_BIRTH_DATE", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_END_BIRTH_DATE", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_BEGIN_FILING", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_END_FILING", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_PW_NAME_POS", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_INSTIT_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_SPEC_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_QUAL_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_TYPE_EDU_ID", OracleDbType.Decimal);

            _daResume.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.RESUME_INSERT(:RESUME_ID, :RESUME_PER_NUM, :FILING_DATE_RESUME, :SOURCE_EMPLOYABILITY_ID, :EMP_LAST_NAME, 
                        :EMP_FIRST_NAME, :EMP_MIDDLE_NAME, :EMP_SEX, :EMP_BIRTH_DATE);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daResume.InsertCommand.BindByName = true;
            _daResume.InsertCommand.Parameters.Add("RESUME_ID", OracleDbType.Decimal, 0, "RESUME_ID");
            _daResume.InsertCommand.Parameters.Add("RESUME_PER_NUM", OracleDbType.Varchar2, 0, "RESUME_PER_NUM");
            _daResume.InsertCommand.Parameters.Add("FILING_DATE_RESUME", OracleDbType.Date, 0, "FILING_DATE_RESUME");
            _daResume.InsertCommand.Parameters.Add("SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal, 0, "SOURCE_EMPLOYABILITY_ID");
            _daResume.InsertCommand.Parameters.Add("EMP_LAST_NAME", OracleDbType.Varchar2, 0, "EMP_LAST_NAME");
            _daResume.InsertCommand.Parameters.Add("EMP_FIRST_NAME", OracleDbType.Varchar2, 0, "EMP_FIRST_NAME");
            _daResume.InsertCommand.Parameters.Add("EMP_MIDDLE_NAME", OracleDbType.Varchar2, 0, "EMP_MIDDLE_NAME");
            _daResume.InsertCommand.Parameters.Add("EMP_SEX", OracleDbType.Varchar2, 0, "EMP_SEX");
            _daResume.InsertCommand.Parameters.Add("EMP_BIRTH_DATE", OracleDbType.Date, 0, "EMP_BIRTH_DATE");
            _daResume.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.RESUME_UPDATE(:RESUME_ID, :RESUME_PER_NUM, :FILING_DATE_RESUME, :SOURCE_EMPLOYABILITY_ID, :EMP_LAST_NAME, 
                        :EMP_FIRST_NAME, :EMP_MIDDLE_NAME, :EMP_SEX, :EMP_BIRTH_DATE);
                END;", Connect.Schema),
                Connect.CurConnect);
            _daResume.UpdateCommand.BindByName = true;
            _daResume.UpdateCommand.Parameters.Add("RESUME_ID", OracleDbType.Decimal, 0, "RESUME_ID");
            _daResume.UpdateCommand.Parameters.Add("RESUME_PER_NUM", OracleDbType.Varchar2, 0, "RESUME_PER_NUM");
            _daResume.UpdateCommand.Parameters.Add("FILING_DATE_RESUME", OracleDbType.Date, 0, "FILING_DATE_RESUME");
            _daResume.UpdateCommand.Parameters.Add("SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal, 0, "SOURCE_EMPLOYABILITY_ID");
            _daResume.UpdateCommand.Parameters.Add("EMP_LAST_NAME", OracleDbType.Varchar2, 0, "EMP_LAST_NAME");
            _daResume.UpdateCommand.Parameters.Add("EMP_FIRST_NAME", OracleDbType.Varchar2, 0, "EMP_FIRST_NAME");
            _daResume.UpdateCommand.Parameters.Add("EMP_MIDDLE_NAME", OracleDbType.Varchar2, 0, "EMP_MIDDLE_NAME");
            _daResume.UpdateCommand.Parameters.Add("EMP_SEX", OracleDbType.Varchar2, 0, "EMP_SEX");
            _daResume.UpdateCommand.Parameters.Add("EMP_BIRTH_DATE", OracleDbType.Date, 0, "EMP_BIRTH_DATE");
            _daResume.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.RESUME_DELETE(:RESUME_ID,:RESUME_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
            _daResume.DeleteCommand.BindByName = true;
            _daResume.DeleteCommand.Parameters.Add("RESUME_ID", OracleDbType.Decimal, 0, "RESUME_ID");
            _daResume.DeleteCommand.Parameters.Add("RESUME_PER_NUM", OracleDbType.Varchar2, 0, "RESUME_PER_NUM");

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

            // Select
            _daHabit.SelectCommand = new OracleCommand(string.Format(
                @"select PER_NUM, HAB_POST_CODE, HAB_FLAT, HAB_HOUSE, HAB_BULK, HAB_CODE_STREET, HAB_PHONE, HAB_NON_KLADR_ADDRESS 
                from {0}.HABIT where PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daHabit.SelectCommand.BindByName = true;
            _daHabit.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daHabit.InsertCommand = new OracleCommand(string.Format(
                @"INSERT INTO {0}.HABIT(PER_NUM, HAB_POST_CODE, HAB_FLAT, HAB_HOUSE, HAB_BULK, HAB_CODE_STREET, HAB_PHONE, HAB_NON_KLADR_ADDRESS)
                VALUES(:PER_NUM, :HAB_POST_CODE, :HAB_FLAT, :HAB_HOUSE, :HAB_BULK, :HAB_CODE_STREET, :HAB_PHONE, :HAB_NON_KLADR_ADDRESS)", Connect.Schema), Connect.CurConnect);
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
                @"UPDATE {0}.HABIT SET HAB_POST_CODE=:HAB_POST_CODE, HAB_FLAT=:HAB_FLAT, HAB_HOUSE=:HAB_HOUSE, 
                    HAB_BULK=:HAB_BULK, HAB_CODE_STREET=:HAB_CODE_STREET, HAB_PHONE=:HAB_PHONE,HAB_NON_KLADR_ADDRESS=:HAB_NON_KLADR_ADDRESS
                WHERE PER_NUM=:PER_NUM", Connect.Schema), Connect.CurConnect);
            _daHabit.UpdateCommand.BindByName = true;
            _daHabit.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daHabit.UpdateCommand.Parameters.Add("HAB_POST_CODE", OracleDbType.Varchar2, 0, "HAB_POST_CODE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_FLAT", OracleDbType.Varchar2, 0, "HAB_FLAT");
            _daHabit.UpdateCommand.Parameters.Add("HAB_HOUSE", OracleDbType.Varchar2, 0, "HAB_HOUSE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_BULK", OracleDbType.Varchar2, 0, "HAB_BULK");
            _daHabit.UpdateCommand.Parameters.Add("HAB_CODE_STREET", OracleDbType.Varchar2, 0, "HAB_CODE_STREET");
            _daHabit.UpdateCommand.Parameters.Add("HAB_PHONE", OracleDbType.Varchar2, 0, "HAB_PHONE");
            _daHabit.UpdateCommand.Parameters.Add("HAB_NON_KLADR_ADDRESS", OracleDbType.Varchar2, 0, "HAB_NON_KLADR_ADDRESS");
            
            _ds.Tables.Add("S_E");
            _ds.Tables.Add("SPECIALITY");
            _ds.Tables.Add("INSTIT");
            _ds.Tables.Add("TYPE_STUDY");
            _ds.Tables.Add("TYPE_EDU");
            _ds.Tables.Add("QUAL");
            _ds.Tables.Add("GROUP_SPEC");

            FillDictionary();

            _ds.Tables.Add("EDU");
            _ds.Tables.Add("PW");
        }
        
        static void FillDictionary()
        {
            _ds.Tables["S_E"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "select SOURCE_EMPLOYABILITY_ID, SOURCE_EMPLOYABILITY_NAME from {0}.SOURCE_EMPLOYABILITY order by SOURCE_EMPLOYABILITY_NAME", 
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["S_E"]);
            _ds.Tables["SPECIALITY"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT SPEC_ID, CODE_SPEC, NAME_SPEC, EDU_SIGN FROM {0}.SPECIALITY ORDER BY NAME_SPEC",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["SPECIALITY"]);
            _ds.Tables["INSTIT"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT INSTIT_ID, INSTIT_NAME, CITY_ID FROM {0}.INSTIT ORDER BY INSTIT_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["INSTIT"]);
            _ds.Tables["TYPE_STUDY"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT TYPE_STUDY_ID, TS_NAME FROM {0}.TYPE_STUDY ORDER BY TS_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["TYPE_STUDY"]);
            _ds.Tables["TYPE_EDU"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT TYPE_EDU_ID, TE_NAME, TE_PRIORITY, TYPE_EDU_PRIOR FROM {0}.TYPE_EDU ORDER BY TE_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["TYPE_EDU"]);
            _ds.Tables["QUAL"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT QUAL_ID, QUAL_NAME FROM {0}.QUAL ORDER BY QUAL_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["QUAL"]);
            _ds.Tables["GROUP_SPEC"].Rows.Clear();
            new OracleDataAdapter(string.Format(
                "SELECT GR_SPEC_ID, GS_NAME FROM {0}.GROUP_SPEC ORDER BY GS_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds.Tables["GROUP_SPEC"]);
        }

        private void GetResume()
        {
            dgResume.DataContext = null;
            _ds.Tables["RESUME"].Clear();
            _daResume.Fill(_ds.Tables["RESUME"]);
            dgResume.DataContext = _ds.Tables["RESUME"].DefaultView;
        }

        private void AddResume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddResume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Выбираем новый Таб.№
            OracleCommand ocNewPer_Num = new OracleCommand(string.Format(
                "BEGIN {0}.GET_NEW_PER_NUM(:p_NEW_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
            ocNewPer_Num.BindByName = true;
            ocNewPer_Num.Parameters.Add("p_NEW_PER_NUM", OracleDbType.Varchar2, 10).Direction = ParameterDirection.InputOutput;
            ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value = "200000";
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                ocNewPer_Num.Transaction = transact;
                ocNewPer_Num.ExecuteNonQuery();
                transact.Commit();
                DataRowView _currentEmp = _ds.Tables["RESUME"].DefaultView.AddNew();
                _currentEmp["RESUME_PER_NUM"] = ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value;
                _ds.Tables["RESUME"].Rows.Add(_currentEmp.Row);
                dgResume.SelectedItem = _currentEmp;

                RefreshPersonData(_currentEmp["RESUME_PER_NUM"].ToString());

                _ds.Tables["REGISTR"].Rows.Add(_ds.Tables["REGISTR"].DefaultView.AddNew().Row);
                _ds.Tables["REGISTR"].DefaultView[0]["PER_NUM"] = _currentEmp["RESUME_PER_NUM"];
                _ds.Tables["HABIT"].Rows.Add(_ds.Tables["HABIT"].DefaultView.AddNew().Row);
                _ds.Tables["HABIT"].DefaultView[0]["PER_NUM"] = _currentEmp["RESUME_PER_NUM"];
                Resume_Editor resume = new Resume_Editor(true, _currentEmp, _ds);
                resume.Owner = Window.GetWindow(this);
                if (resume.ShowDialog() == true)
                {
                    //SaveResume();
                }
                else
                {
                    // Возвращаем флаг свободного табельного
                    OracleCommand ocUpdatePer_Num_Book = new OracleCommand(string.Format(
                        "UPDATE {0}.PER_NUM_BOOK SET FREE_SIGN = 1 WHERE PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
                    ocUpdatePer_Num_Book.BindByName = true;
                    ocUpdatePer_Num_Book.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _currentEmp["RESUME_PER_NUM"];
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

        private void EditResume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgResume != null && dgResume.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditResume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgResume.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();

            RefreshPersonData(rowSelected["RESUME_PER_NUM"].ToString());

            Resume_Editor violation = new Resume_Editor(false, rowSelected, _ds);
            //violation.DataContext = rowSelected;
            violation.Owner = Window.GetWindow(this);
            if (violation.ShowDialog() == true)
            {
                SaveResume();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            } 
        }

        private void DeleteResume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgResume.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgResume.SelectedCells[0].Item).Delete();
                }
                SaveResume();
            }
            dgResume.Focus();
        }

        public static void SaveResume()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["RESUME"].DefaultView.RowStateFilter;
                _ds.Tables["RESUME"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["RESUME"].DefaultView.Count; ++i)
                {
                    _ds.Tables["RESUME"].DefaultView[i]["RESUME_ID"] =
                        new OracleCommand(string.Format("select {0}.RESUME_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                }
                _ds.Tables["RESUME"].DefaultView.RowStateFilter = rs;
                _daResume.InsertCommand.Transaction = transact;
                _daResume.UpdateCommand.Transaction = transact;
                _daResume.DeleteCommand.Transaction = transact;
                _daResume.Update(_ds.Tables["RESUME"]);
                _daRegistr.InsertCommand.Transaction = transact;
                _daRegistr.UpdateCommand.Transaction = transact;
                _daRegistr.Update(_ds.Tables["REGISTR"]);
                _daHabit.InsertCommand.Transaction = transact;
                _daHabit.UpdateCommand.Transaction = transact;
                _daHabit.Update(_ds.Tables["HABIT"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void dgResume_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WpfControlLibrary.Wpf_Commands.EditResume.Execute(null, null);
        }

        private void btFilter_Apply_Click(object sender, RoutedEventArgs e)
        {
            _daResume.SelectCommand.Parameters["p_begin_period"].Value = dpBEGIN_DATE_HIRE.SelectedDate;
            _daResume.SelectCommand.Parameters["p_end_period"].Value = dpBEGIN_DATE_HIRE.SelectedDate;
            _daResume.SelectCommand.Parameters["p_EMP_LAST_NAME"].Value = tbEMP_LAST_NAME.Text;
            _daResume.SelectCommand.Parameters["p_EMP_FIRST_NAME"].Value = tbEMP_FIRST_NAME.Text;
            _daResume.SelectCommand.Parameters["p_EMP_MIDDLE_NAME"].Value = tbEMP_MIDDLE_NAME.Text;
            _daResume.SelectCommand.Parameters["p_EMP_SEX"].Value = cbEMP_SEX.Text;
            _daResume.SelectCommand.Parameters["p_BEGIN_BIRTH_DATE"].Value = dpBEGIN_BIRTH_DATE.SelectedDate;
            _daResume.SelectCommand.Parameters["p_END_BIRTH_DATE"].Value = dpEND_BIRTH_DATE.SelectedDate;
            _daResume.SelectCommand.Parameters["p_SOURCE_EMPLOYABILITY_ID"].Value = cbSOURCE_EMPLOYABILITY_ID.SelectedValue;
            _daResume.SelectCommand.Parameters["p_BEGIN_FILING"].Value = dpBEGIN_FILING.SelectedDate;
            _daResume.SelectCommand.Parameters["p_END_FILING"].Value = dpEND_FILING.SelectedDate;
            _daResume.SelectCommand.Parameters["p_PW_NAME_POS"].Value = tbPW_NAME_POS.Text == "" ? null : ("%" + tbPW_NAME_POS.Text + "%");
            _daResume.SelectCommand.Parameters["p_INSTIT_ID"].Value = cbINSTIT_ID.SelectedValue;
            _daResume.SelectCommand.Parameters["p_SPEC_ID"].Value = cbSPEC_ID.SelectedValue;
            _daResume.SelectCommand.Parameters["p_QUAL_ID"].Value = cbQUAL_ID.SelectedValue;
            _daResume.SelectCommand.Parameters["p_TYPE_EDU_ID"].Value = cbTYPE_EDU_ID.SelectedValue;

            GetResume();
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            dpBEGIN_DATE_HIRE.SelectedDate = null;
            dpEND_DATE_HIRE.SelectedDate = null;
            tbEMP_LAST_NAME.Text = "";
            tbEMP_FIRST_NAME.Text = "";
            tbEMP_MIDDLE_NAME.Text = "";
            cbEMP_SEX.Text = "";
            dpBEGIN_BIRTH_DATE.SelectedDate = null;
            dpEND_BIRTH_DATE.SelectedDate = null;
            cbSOURCE_EMPLOYABILITY_ID.SelectedValue = null;
            dpBEGIN_FILING.SelectedDate = null;
            dpEND_FILING.SelectedDate = null;
            tbPW_NAME_POS.Text = "";
            cbINSTIT_ID.SelectedValue = null;
            cbSPEC_ID.SelectedValue = null;
            cbQUAL_ID.SelectedValue = null;
            cbTYPE_EDU_ID.SelectedValue = null;
            btFilter_Apply_Click(sender, e);
        }

        void RefreshPersonData(string cur_per_num)
        {
            // Очищаем таблицы перед заполнением
            _ds.Tables["HABIT"].Clear();
            _ds.Tables["REGISTR"].Clear();
            // Fill
            _daHabit.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daRegistr.SelectCommand.Parameters["p_PER_NUM"].Value = cur_per_num;
            _daHabit.Fill(_ds.Tables["HABIT"]);
            _daRegistr.Fill(_ds.Tables["REGISTR"]);
        }
    }
}
