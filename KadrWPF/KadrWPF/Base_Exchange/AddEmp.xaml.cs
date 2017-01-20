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
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Data;
using Kadr;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for AddEmp.xaml
    /// </summary>
    public partial class AddEmp : Window
    {
        StringBuilder str_find;
        private static DataSet _dsEmp;
        private static OracleDataAdapter _daResume, _daEmp;
        DataTable _dtBase;
        DataGrid _dgBase;
        public AddEmp(DataTable dtBase, DataGrid dgBase)
        {
            InitializeComponent();
            _dtBase = dtBase;
            _dgBase = dgBase;
            _dsEmp.Tables["EMP"].Rows.Clear();
            _dsEmp.Tables["RESUME"].Rows.Clear();
            dgEmp.ItemsSource = _dsEmp.Tables["EMP"].DefaultView;
            dgResume.ItemsSource = _dsEmp.Tables["RESUME"].DefaultView;

            cbSubdiv.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;
        }

        static AddEmp()
        {
            _dsEmp = new DataSet();
            _dsEmp.Tables.Add("EMP");
            _dsEmp.Tables.Add("RESUME");

            _daEmp = new OracleDataAdapter();
            _daResume = new OracleDataAdapter();
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
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Find_Old_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void Find_Old_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /// Строка условий
            str_find = new StringBuilder();
            if (dpEmp_Birth_Date.Text.Replace(".", "").Trim() != "")
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.EMP where {0}.EMP.PER_NUM = CUR_EMP.PER_NUM " +
                    " and {0}.EMP.EMP_BIRTH_DATE = to_date('{1}','dd.MM.yyyy'))",
                    Connect.Schema, dpEmp_Birth_Date.SelectedDate.Value.ToShortDateString(), 
                    str_find.Length != 0 ? " and" : ""));
                _daResume.SelectCommand.Parameters["p_BEGIN_BIRTH_DATE"].Value = dpEmp_Birth_Date.SelectedDate.Value;
                _daResume.SelectCommand.Parameters["p_END_BIRTH_DATE"].Value = dpEmp_Birth_Date.SelectedDate.Value.AddDays(1).AddSeconds(-1);
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.EMP where {0}.EMP.PER_NUM = CUR_EMP.PER_NUM " +
                    " and upper({0}.EMP.EMP_LAST_NAME) = upper('{1}'))",
                    Connect.Schema, tbEmp_Last_Name.Text.Trim(), str_find.Length != 0 ? " and" : ""));
                _daResume.SelectCommand.Parameters["p_EMP_LAST_NAME"].Value = tbEmp_Last_Name.Text.Trim();
            }
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.EMP where {0}.EMP.PER_NUM = CUR_EMP.PER_NUM " +
                    " and upper({0}.EMP.EMP_FIRST_NAME) = upper('{1}'))",
                    Connect.Schema, tbEmp_First_Name.Text.Trim(), str_find.Length != 0 ? " and" : ""));
                _daResume.SelectCommand.Parameters["p_EMP_FIRST_NAME"].Value = tbEmp_First_Name.Text.Trim();
            }
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.EMP where {0}.EMP.PER_NUM = CUR_EMP.PER_NUM " +
                    " and upper({0}.EMP.EMP_MIDDLE_NAME) = upper('{1}'))",
                    Connect.Schema, tbEmp_Middle_Name.Text.Trim(), str_find.Length != 0 ? " and" : ""));
                _daResume.SelectCommand.Parameters["p_EMP_MIDDLE_NAME"].Value = tbEmp_Middle_Name.Text.Trim();
            }
            if (tbInn.Text.Trim() != "")
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.PER_DATA where {0}.PER_DATA.PER_NUM = CUR_EMP.PER_NUM and " +
                    "{0}.PER_DATA.INN = '{1}') ",
                    Connect.Schema, tbInn.Text.Trim(), str_find.Length != 0 ? " and" : ""));
            }
            if (mbInsurance_Num.Value != null)
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.PER_DATA where {0}.PER_DATA.PER_NUM = CUR_EMP.PER_NUM and " +
                    "{0}.PER_DATA.INSURANCE_NUM = '{1}') ",
                    Connect.Schema, mbInsurance_Num.Value.ToString(), str_find.Length != 0 ? " and" : ""));
            }
            if (cbSubdiv.SelectedValue != null)
            {
                str_find.Append(string.Format("{2} exists " +
                    "(select null from {0}.TRANSFER where {0}.TRANSFER.TRANSFER_ID = CUR_EMP.TRANSFER_ID and " +
                    "{0}.TRANSFER.PER_NUM = CUR_EMP.PER_NUM and {0}.TRANSFER.SUBDIV_ID = {1})",
                    Connect.Schema, cbSubdiv.SelectedValue, str_find.Length != 0 ? " and" : ""));
            }
            /// Строка запроса
            string textBlock;
            if (str_find.Length != 0)
            {
                textBlock = string.Format(Queries.GetQuery("TP/SelectListEmpForHire.sql"),
                    Staff.DataSourceScheme.SchemeName, string.Format(" where {0} ", str_find.ToString()));
            }
            else
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска.", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            _daEmp.SelectCommand = new OracleCommand(textBlock, Connect.CurConnect);
            _dsEmp.Tables["EMP"].Rows.Clear();
            _daEmp.Fill(_dsEmp.Tables["EMP"]);
            if (_dsEmp.Tables["EMP"].Rows.Count == 0)
            {
                MessageBox.Show("В базе данных бывших работников не найдены работники, удовлетворяющие условиям поиска.", "АСУ \"Кадры\"",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            _dsEmp.Tables["RESUME"].Clear();
            _daResume.Fill(_dsEmp.Tables["RESUME"]);
            if (_dsEmp.Tables["RESUME"].Rows.Count == 0)
            {
                MessageBox.Show("В базе резюме не найдены данные, удовлетворяющие условиям поиска.", "АСУ \"Кадры\"", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Hire_New_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleCommand _ocNewPer_Num = new OracleCommand(string.Format(
                "BEGIN {0}.GET_NEW_PER_NUM(:p_NEW_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
            _ocNewPer_Num.BindByName = true;
            _ocNewPer_Num.Parameters.Add("p_NEW_PER_NUM", OracleDbType.Varchar2, 10).Direction = ParameterDirection.InputOutput;
            _ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value = "300000";
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocNewPer_Num.Transaction = transact;
                _ocNewPer_Num.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show("Ошибка добавления нового сотрудника!\n\n" + ex.Message, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataRowView _currentEmp = _dtBase.DefaultView.AddNew();
            _currentEmp["PER_NUM"] = _ocNewPer_Num.Parameters["p_NEW_PER_NUM"].Value;
            _currentEmp["TYPE_TRANSFER_ID"] = 1;
            _currentEmp["PROJECT_TRANSFER_ID"] = -1;
            _currentEmp["PROJECT_PLAN_APPROVAL_ID"] = 0;
            _dtBase.Rows.Add(_currentEmp.Row);
            _dgBase.SelectedItem = _currentEmp;
            Add_New_Emp_Editor trans_pr = new Add_New_Emp_Editor(_currentEmp);
            trans_pr.Owner = Window.GetWindow(this);
            if (trans_pr.ShowDialog() != true && _dtBase.GetChanges() != null)
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
                _dtBase.RejectChanges();
            }
        }

        private void Hire_Old_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentOldEmp = (DataRowView)dgEmp.SelectedCells[0].Item;
            DataRowView _currentEmp = _dtBase.DefaultView.AddNew();
            _currentEmp["PROJECT_TRANSFER_ID"] = -1;
            _currentEmp["PER_NUM"] = _currentOldEmp["PER_NUM"];
            _currentEmp["VISUAL_PER_NUM"] = _currentOldEmp["PER_NUM"];
            _currentEmp["EMP_LAST_NAME"] = _currentOldEmp["EMP_LAST_NAME"];
            _currentEmp["EMP_FIRST_NAME"] = _currentOldEmp["EMP_FIRST_NAME"];
            _currentEmp["EMP_MIDDLE_NAME"] = _currentOldEmp["EMP_MIDDLE_NAME"];
            _currentEmp["EMP_SEX"] = _currentOldEmp["EMP_SEX"];
            _currentEmp["EMP_BIRTH_DATE"] = _currentOldEmp["EMP_BIRTH_DATE"];
            _currentEmp["TYPE_TRANSFER_ID"] = 1;
            _currentEmp["PROJECT_PLAN_APPROVAL_ID"] = 0;
            _dtBase.Rows.Add(_currentEmp.Row);
            _dgBase.SelectedItem = _currentEmp;
            Add_New_Emp_Editor trans_pr = new Add_New_Emp_Editor(_currentEmp);
            trans_pr.Owner = Window.GetWindow(this);
            if (trans_pr.ShowDialog() != true)
            {
                _dtBase.RejectChanges();
            }
        }

        private void Hire_Resume_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentOldEmp = (DataRowView)dgResume.SelectedCells[0].Item;
            DataRowView _currentEmp = _dtBase.DefaultView.AddNew();
            _currentEmp["PER_NUM"] = _currentOldEmp["RESUME_PER_NUM"];
            _currentEmp["EMP_LAST_NAME"] = _currentOldEmp["EMP_LAST_NAME"];
            _currentEmp["EMP_FIRST_NAME"] = _currentOldEmp["EMP_FIRST_NAME"];
            _currentEmp["EMP_MIDDLE_NAME"] = _currentOldEmp["EMP_MIDDLE_NAME"];
            _currentEmp["EMP_SEX"] = _currentOldEmp["EMP_SEX"];
            _currentEmp["EMP_BIRTH_DATE"] = _currentOldEmp["EMP_BIRTH_DATE"];
            _currentEmp["TYPE_TRANSFER_ID"] = 1;
            _currentEmp["PROJECT_PLAN_APPROVAL_ID"] = 0;
            // Ставим признак приема по резюме, чтобы при утверждении приема изменить данные в Резюме
            _currentEmp["SIGN_RESUME"] = 1;
            // Ставим поле Источник из резюме
            _currentEmp["SOURCE_EMPLOYABILITY_ID"] = _currentOldEmp["SOURCE_EMPLOYABILITY_ID"]; ;
            _dtBase.Rows.Add(_currentEmp.Row);
            _dgBase.SelectedItem = _currentEmp;
            Add_New_Emp_Editor trans_pr = new Add_New_Emp_Editor(_currentEmp);
            trans_pr.Owner = Window.GetWindow(this);
            if (trans_pr.ShowDialog() != true)
            {
                _dtBase.RejectChanges();
            }
            /*string resume_per_num = dgViewEmpResume.Rows[bsEmpResume.Position].Cells["RESUME_PER_NUM"].Value.ToString();
            OracleCommand _ocEmp_Hire = new OracleCommand(string.Format(
                "BEGIN {0}.EMP_HIRE(:p_RESUME_PER_NUM, :p_NEW_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
            _ocEmp_Hire.BindByName = true;
            _ocEmp_Hire.Parameters.Add("p_RESUME_PER_NUM", OracleDbType.Varchar2).Value = resume_per_num;
            _ocEmp_Hire.Parameters.Add("p_NEW_PER_NUM", OracleDbType.Varchar2, 10, "p_NEW_PER_NUM").Direction = ParameterDirection.InputOutput;
            _ocEmp_Hire.Parameters["p_NEW_PER_NUM"].Value = "00001";
            Connect.Transact = Connect.CurConnect.BeginTransaction();
            _ocEmp_Hire.Transaction = Connect.Transact;
            _ocEmp_Hire.ExecuteNonQuery();
            string _per_num = _ocEmp_Hire.Parameters["p_NEW_PER_NUM"].Value.ToString();
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format("where per_num = {0}", _per_num));
            PersonalCard personalcard = new PersonalCard(_per_num, 0, record_emp, false, true, false, 0, null);
            personalcard.Text = "Личная карточка работника приемной базы данных";
            personalcard.ShowInTaskbar = false;
            this.Hide();
            personalcard.ShowDialog();
            this.Close();*/
        }

        private void Hire_Old_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && 
                dgEmp != null && dgEmp.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void Hire_Resume_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgResume != null && dgResume.SelectedCells.Count > 0)
                e.CanExecute = true;
        }
    }
}
