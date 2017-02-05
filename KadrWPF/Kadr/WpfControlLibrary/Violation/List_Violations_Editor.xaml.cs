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
using WpfControlLibrary;
using System.Windows.Interop;
namespace Pass_Office
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class List_Violations_Editor : UserControl
    {
        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }
        private static OracleDataAdapter _daViolation = new OracleDataAdapter();
        public List_Violations_Editor()
        {
            InitializeComponent();
            dgList_Violations.DataContext = _ds.Tables["VIOLATION"].DefaultView;
            //cbCodeSubdivFilter.ItemsSource = FormMain.dsSubdivTable.Tables["SUBDIV_ALL"].DefaultView;
            cbSubdivNameFilter.ItemsSource = FormMain.dsSubdivTable.Tables["SUBDIV_ALL"].DefaultView;
            cbTYPE_GROUP_PUNISHMENT.ItemsSource = _ds.Tables["TYPE_GROUP_PUNISHMENT"].DefaultView;
            GetViolations("");
        }

        int _viol_id = -1;
        static List_Violations_Editor()
        {
            _ds = new DataSet();
            _ds.Tables.Add("VIOLATION");
            _ds.Tables.Add("VIOLATION_ROW");
            _ds.Tables.Add("REPORT");
            _daViolation.SelectCommand = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("PO/SelectViolation.sql"),
                LibraryKadr.Connect.Schema), LibraryKadr.Connect.CurConnect);
            _daViolation.SelectCommand.BindByName = true;
            _daViolation.SelectCommand.Parameters.Add("p_VIOLATION_ID", OracleDbType.Decimal);
            _daViolation.SelectCommand.Parameters.Add("p_TYPE_GROUP_PUNISHMENT_ID", OracleDbType.Decimal);
            _daViolation.SelectCommand.Parameters.Add("p_GRANTED_GROUP_HIRE", OracleDbType.Decimal).Value = 
                Convert.ToInt16(GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"));
            _daViolation.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.VIOLATION_INSERT(:VIOLATION_ID,:DETENTION_DATE,:INFORM_SUBDIV_DATE,:SIGN_DISCIPLINARY_COMM,:REASON_DETENTION_ID,
                    :SIGN_DETENTION_ID,:MEASURES_TAKEN,:NOTE,:SIGN_GROUP_VIOLATION,:VIOLATOR_ID,:TRANSFER_ID,
                    :PERCO_SYNC_ID,:OTHER_VIOLATOR_ID); END;",
                Connect.Schema), Connect.CurConnect);
            _daViolation.InsertCommand.BindByName = true;
            _daViolation.InsertCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daViolation.InsertCommand.Parameters.Add("DETENTION_DATE", OracleDbType.Date, 0, "DETENTION_DATE");
            _daViolation.InsertCommand.Parameters.Add("INFORM_SUBDIV_DATE", OracleDbType.Date, 0, "INFORM_SUBDIV_DATE");
            _daViolation.InsertCommand.Parameters.Add("SIGN_DISCIPLINARY_COMM", OracleDbType.Decimal, 0, "SIGN_DISCIPLINARY_COMM");
            _daViolation.InsertCommand.Parameters.Add("REASON_DETENTION_ID", OracleDbType.Decimal, 0, "REASON_DETENTION_ID");
            _daViolation.InsertCommand.Parameters.Add("SIGN_DETENTION_ID", OracleDbType.Decimal, 0, "SIGN_DETENTION_ID");
            _daViolation.InsertCommand.Parameters.Add("MEASURES_TAKEN", OracleDbType.Varchar2, 0, "MEASURES_TAKEN");
            _daViolation.InsertCommand.Parameters.Add("NOTE", OracleDbType.Varchar2, 0, "NOTE");
            _daViolation.InsertCommand.Parameters.Add("SIGN_GROUP_VIOLATION", OracleDbType.Decimal, 0, "SIGN_GROUP_VIOLATION");
            _daViolation.InsertCommand.Parameters.Add("VIOLATOR_ID", OracleDbType.Decimal, 0, "VIOLATOR_ID");
            _daViolation.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daViolation.InsertCommand.Parameters.Add("PERCO_SYNC_ID", OracleDbType.Decimal, 0, "PERCO_SYNC_ID");
            _daViolation.InsertCommand.Parameters.Add("OTHER_VIOLATOR_ID", OracleDbType.Decimal, 0, "OTHER_VIOLATOR_ID");
            _daViolation.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.VIOLATION_UPDATE(:VIOLATION_ID,:DETENTION_DATE,:INFORM_SUBDIV_DATE,:SIGN_DISCIPLINARY_COMM,:REASON_DETENTION_ID,
                    :SIGN_DETENTION_ID,:MEASURES_TAKEN,:NOTE,:SIGN_GROUP_VIOLATION,:VIOLATOR_ID,:TRANSFER_ID,
                    :PERCO_SYNC_ID,:OTHER_VIOLATOR_ID); END;", Connect.Schema),
                Connect.CurConnect);
            _daViolation.UpdateCommand.BindByName = true;
            _daViolation.UpdateCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daViolation.UpdateCommand.Parameters.Add("DETENTION_DATE", OracleDbType.Date, 0, "DETENTION_DATE");
            _daViolation.UpdateCommand.Parameters.Add("INFORM_SUBDIV_DATE", OracleDbType.Date, 0, "INFORM_SUBDIV_DATE");
            _daViolation.UpdateCommand.Parameters.Add("SIGN_DISCIPLINARY_COMM", OracleDbType.Decimal, 0, "SIGN_DISCIPLINARY_COMM");
            _daViolation.UpdateCommand.Parameters.Add("REASON_DETENTION_ID", OracleDbType.Decimal, 0, "REASON_DETENTION_ID");
            _daViolation.UpdateCommand.Parameters.Add("SIGN_DETENTION_ID", OracleDbType.Decimal, 0, "SIGN_DETENTION_ID");
            _daViolation.UpdateCommand.Parameters.Add("MEASURES_TAKEN", OracleDbType.Varchar2, 0, "MEASURES_TAKEN");
            _daViolation.UpdateCommand.Parameters.Add("NOTE", OracleDbType.Varchar2, 0, "NOTE");
            _daViolation.UpdateCommand.Parameters.Add("SIGN_GROUP_VIOLATION", OracleDbType.Decimal, 0, "SIGN_GROUP_VIOLATION");
            _daViolation.UpdateCommand.Parameters.Add("VIOLATOR_ID", OracleDbType.Decimal, 0, "VIOLATOR_ID");
            _daViolation.UpdateCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daViolation.UpdateCommand.Parameters.Add("PERCO_SYNC_ID", OracleDbType.Decimal, 0, "PERCO_SYNC_ID");
            _daViolation.UpdateCommand.Parameters.Add("OTHER_VIOLATOR_ID", OracleDbType.Decimal, 0, "OTHER_VIOLATOR_ID");
            _daViolation.DeleteCommand = new OracleCommand(string.Format(
                @"delete from {0}.VIOLATION where VIOLATION_ID = :VIOLATION_ID", Connect.Schema), Connect.CurConnect);
            _daViolation.DeleteCommand.BindByName = true;
            _daViolation.DeleteCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            
            new OracleDataAdapter(string.Format(
                @"select TYPE_GROUP_PUNISHMENT_ID, TYPE_GROUP_PUNISHMENT_NAME from {0}.TYPE_GROUP_PUNISHMENT order by TYPE_GROUP_PUNISHMENT_NAME",
                Connect.Schema), Connect.CurConnect).Fill(_ds, "TYPE_GROUP_PUNISHMENT");
        }

        private void GetViolations(string stFilter)
        {
            _ds.Tables["VIOLATION"].Clear();
            _daViolation.Fill(_ds.Tables["VIOLATION"]);
            _ds.Tables["VIOLATION"].DefaultView.RowFilter = stFilter;
            if (!_ds.Tables["VIOLATION"].Columns.Contains("PHOTO"))
            {
                _ds.Tables["VIOLATION"].Columns.Add("PHOTO", Type.GetType("System.Byte[]"));
            }
            CalcCountViolation();
        }

        void CalcCountViolation()
        {
            tbCountViolation.Text = _ds.Tables["VIOLATION"].DefaultView.Count.ToString();
            tbCountViolator.Text = _ds.Tables["VIOLATION"].DefaultView.ToTable().Select().
                GroupBy(r => r["PER_NUM"].ToString()+r["PERCO_SYNC_ID"].ToString()+r["OTHER_VIOLATOR_ID"].ToString()).Count().ToString() ;
        }

        private void Add_Violation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void Add_Violation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = _ds.Tables["VIOLATION"].DefaultView.AddNew();
            rowSelected["VIOLATION_ID"] = _viol_id--;
            rowSelected["SIGN_DISCIPLINARY_COMM"] = 0;
            rowSelected["SIGN_GROUP_VIOLATION"] = 0;
            _ds.Tables["VIOLATION"].Rows.Add(rowSelected.Row);
            dgList_Violations.SelectedItem = rowSelected;

            Violation_View violation = new Violation_View(rowSelected);
            //violation.DataContext = newRow;
            violation.Owner = Window.GetWindow(this);            
            if (violation.ShowDialog() == true)
            {
                // Проверяем признак удаления нарушения. Если он установлен, убираем редактируемую строчку.
                if (Violation_View.Fl_Delete_Violation)
                {
                    rowSelected.Delete();
                    SaveViolation();
                    return;
                }
                SaveViolation();
                _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = rowSelected["VIOLATION_ID"];                
                _ds.Tables["VIOLATION_ROW"].Clear();
                _daViolation.Fill(_ds.Tables["VIOLATION_ROW"]);
                _ds.Tables["VIOLATION"].PrimaryKey = new DataColumn[1] { _ds.Tables["VIOLATION"].Columns["VIOLATION_ID"] };
                rowSelected.Row.Table.LoadDataRow(_ds.Tables["VIOLATION_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = null;
            }
            else
            {
                rowSelected.Row.RejectChanges();
            }
        }

        private void SaveViolation()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["VIOLATION"].DefaultView.RowStateFilter;
                _ds.Tables["VIOLATION"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["VIOLATION"].DefaultView.Count; ++i)
                {
                    _ds.Tables["VIOLATION"].DefaultView[i]["VIOLATION_ID"] =
                        new OracleCommand(string.Format("select {0}.VIOLATION_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                    _ds.Tables["VIOLATION"].DefaultView[i]["VIOLATOR_ID"] =
                        new OracleCommand(string.Format("select {0}.VIOLATOR_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                }
                _ds.Tables["VIOLATION"].DefaultView.RowStateFilter = rs;
                _daViolation.InsertCommand.Transaction = transact;
                _daViolation.UpdateCommand.Transaction = transact;
                _daViolation.DeleteCommand.Transaction = transact;
                _daViolation.Update(_ds.Tables["VIOLATION"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Edit_Violation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && 
                dgList_Violations != null && dgList_Violations.SelectedItem != null)
                e.CanExecute = true;
        }

        private void Edit_Violation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgList_Violations.SelectedItem);
            rowSelected.Row.RejectChanges();
            Violation_View violation = new Violation_View(rowSelected);
            //violation.DataContext = rowSelected;
            violation.Owner = Window.GetWindow(this);
            if (violation.ShowDialog() == true)
            {
                // Проверяем признак удаления нарушения. Если он установлен, убираем редактируемую строчку.
                if (Violation_View.Fl_Delete_Violation)
                {
                    rowSelected.Delete();
                    SaveViolation();
                    return;
                }
                _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = rowSelected["VIOLATION_ID"];
                SaveViolation();
                _ds.Tables["VIOLATION_ROW"].Clear();
                _daViolation.Fill(_ds.Tables["VIOLATION_ROW"]);
                _ds.Tables["VIOLATION"].PrimaryKey = new DataColumn[1] { _ds.Tables["VIOLATION"].Columns["VIOLATION_ID"] };
                rowSelected.Row.Table.LoadDataRow(_ds.Tables["VIOLATION_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = null;
            }
            else
            {
                // Проверяем признак удаления нарушения. Если он установлен, убираем редактируемую строчку.
                if (Violation_View.Fl_Delete_Violation)
                {
                    rowSelected.Delete();
                    SaveViolation();
                    return;
                }
                rowSelected.Row.RejectChanges();
                if (Violation_View.Fl_reload)
                {
                    _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = rowSelected["VIOLATION_ID"];
                    _ds.Tables["VIOLATION_ROW"].Clear();
                    _daViolation.Fill(_ds.Tables["VIOLATION_ROW"]);
                    _ds.Tables["VIOLATION"].PrimaryKey = new DataColumn[1] { _ds.Tables["VIOLATION"].Columns["VIOLATION_ID"] };
                    rowSelected.Row.Table.LoadDataRow(_ds.Tables["VIOLATION_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
                    _daViolation.SelectCommand.Parameters["p_VIOLATION_ID"].Value = null;
                }
            }  
        }

        private void dgList_Violations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WpfControlLibrary.Pass_Office_Commands.Edit_Violation.Execute(null, null);
        }

        private void btFilter_Apply_Click(object sender, RoutedEventArgs e)
        {
            string stFilter = "";
            if (cbSubdivNameFilter.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "SUBDIV_ID = " + cbSubdivNameFilter.SelectedValue,
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbPer_num.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "PER_NUM = '" + tbPer_num.Text.Trim().PadLeft(5, '0') + "'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbLast_name.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "LAST_NAME like '" + tbLast_name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbFirst_name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "FIRST_NAME like '" + tbFirst_name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbMiddle_name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "MIDDLE_NAME like '" + tbMiddle_name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            DateTime _dateBegin, _dateEnd;
            if (dpPeriodBegin.SelectedDate != null)
                _dateBegin = (DateTime)dpPeriodBegin.SelectedDate;
            else
                _dateBegin = new DateTime(1000, 1, 1);
            if (dpPeriodEnd.SelectedDate != null)
                _dateEnd = (DateTime)dpPeriodEnd.SelectedDate;
            else
                _dateEnd = new DateTime(3000, 1, 1);
            stFilter = string.Format("{0} {2} {1}", stFilter, "(DETENTION_DATE >= '" + _dateBegin + "' and DETENTION_DATE <= '" + _dateEnd + "')",
                stFilter != "" ? "and" : "").Trim();
            if (chSign_Theft.IsChecked != null)
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "SIGN_THEFT = " + Convert.ToInt16(chSign_Theft.IsChecked),
                    stFilter != "" ? "and" : "").Trim();
            }
            if (chSign_Criminal.IsChecked != null)
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "SIGN_CRIMINAL_PROSECUTION = " + Convert.ToInt16(chSign_Criminal.IsChecked),
                    stFilter != "" ? "and" : "").Trim();
            }
            if (chSign_Group.IsChecked != null)
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "SIGN_GROUP_VIOLATION = " + Convert.ToInt16(chSign_Group.IsChecked),
                    stFilter != "" ? "and" : "").Trim();
            }
            if (cbTYPE_GROUP_PUNISHMENT.SelectedValue != null)
            {
                _daViolation.SelectCommand.Parameters["p_TYPE_GROUP_PUNISHMENT_ID"].Value = cbTYPE_GROUP_PUNISHMENT.SelectedValue;
            }
            else
            {
                _daViolation.SelectCommand.Parameters["p_TYPE_GROUP_PUNISHMENT_ID"].Value = null;
            }
            GetViolations(stFilter);
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            cbSubdivNameFilter.SelectedValue = null;
            tbPer_num.Text = "";
            tbLast_name.Text = "";
            tbFirst_name.Text = "";
            tbMiddle_name.Text = "";
            dpPeriodBegin.SelectedDate = null;
            dpPeriodEnd.SelectedDate = null;
            chSign_Theft.IsChecked = null;
            chSign_Criminal.IsChecked = null;
            chSign_Group.IsChecked = null;
            cbTYPE_GROUP_PUNISHMENT.SelectedValue = null;
            _daViolation.SelectCommand.Parameters["p_TYPE_GROUP_PUNISHMENT_ID"].Value = null;
            GetViolations("");
        }

        private void Delete_Violation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Collections.IList rowArray = dgList_Violations.SelectedItems;
                while (dgList_Violations.SelectedItems.Count > 0)
                {
                    ((DataRowView)dgList_Violations.SelectedItems[0]).Delete();
                }
                SaveViolation();
            }
            dgList_Violations.Focus();
        }

        private void PrintViolationByPeriod_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViolationByPeriod selViol = new SelectViolationByPeriod(Convert.ToInt16(e.Parameter));
            selViol.Owner = Window.GetWindow(this);
            selViol.ShowDialog();
        }

        private void SummaryDataOfTheEmployee_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ds.Tables["REPORT"].Rows.Clear();
            OracleDataAdapter daReport = new OracleDataAdapter();
            daReport.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/RepSummaryData.sql"), Connect.Schema), Connect.CurConnect);
            daReport.SelectCommand.BindByName = true;
            daReport.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = 
                ((DataRowView)dgList_Violations.SelectedItem)["PER_NUM"];
            daReport.SelectCommand.Parameters.Add("p_PERCO_SYNC_ID", OracleDbType.Decimal).Value = 
                ((DataRowView)dgList_Violations.SelectedItem)["PERCO_SYNC_ID"];
            daReport.SelectCommand.Parameters.Add("p_FR_LAST_NAME", OracleDbType.Varchar2).Value =
                ((DataRowView)dgList_Violations.SelectedItem)["LAST_NAME"];
            daReport.SelectCommand.Parameters.Add("p_FR_FIRST_NAME", OracleDbType.Varchar2).Value =
                ((DataRowView)dgList_Violations.SelectedItem)["FIRST_NAME"];
            daReport.SelectCommand.Parameters.Add("p_FR_MIDDLE_NAME", OracleDbType.Varchar2).Value =
                ((DataRowView)dgList_Violations.SelectedItem)["MIDDLE_NAME"];
            daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value =
                ((DataRowView)dgList_Violations.SelectedItem)["SUBDIV_ID"];
            daReport.SelectCommand.Parameters.Add("p_OTHER_VIOLATOR_ID", OracleDbType.Decimal).Value = 
                ((DataRowView)dgList_Violations.SelectedItem)["OTHER_VIOLATOR_ID"];
            daReport.Fill(_ds.Tables["REPORT"]);
            if (_ds.Tables["REPORT"].Rows.Count > 0)
            {
                ReportViewerWindow report =
                    new ReportViewerWindow(
                        "Сводные данные по нарушителю", "Reports/RepSummaryData.rdlc", _ds,
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {}
                    );
                report.Show();
            }
            else
            {
                MessageBox.Show("Данных нет!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DisciplineDisturbers_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InputMonthAndYear inputMY = new InputMonthAndYear(false);
            inputMY.Owner = Window.GetWindow(this);
            if (inputMY.ShowDialog() == true)
            {
                _ds.Tables["REPORT"].Rows.Clear();
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("PO/RepDisciplineDisturbers.sql"), Connect.Schema), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value =
                    new DateTime(InputMonthAndYear.NumYear, 1, 1);
                daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value =
                    new DateTime(InputMonthAndYear.NumYear, 12, 31);
                daReport.Fill(_ds.Tables["REPORT"]);

                if (_ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Нарушители дисциплины", "Reports/RepDisciplineDisturbers.rdlc", _ds,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() { 
                            new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", InputMonthAndYear.NumYear.ToString())
                        }
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("Данных нет!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
