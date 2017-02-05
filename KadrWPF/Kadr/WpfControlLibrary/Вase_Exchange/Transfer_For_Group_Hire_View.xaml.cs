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
    /// Interaction logic for Dismissed_Emp_View.xaml
    /// </summary>
    public partial class Transfer_For_Group_Hire_View : UserControl
    {
        private DataSet _ds = new DataSet();
        public DataSet Ds
        {
            get { return _ds; }
        }
        private OracleDataAdapter _daDismissed_Emp = new OracleDataAdapter();
        public Transfer_For_Group_Hire_View()
        {
            _ds = new DataSet();
            _ds.Tables.Add("DISMISSED_EMP");
            _ds.Tables.Add("DISMISSED_EMP_ROW");
            _daDismissed_Emp.SelectCommand = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("PO/SelectDismissed_Emp.sql"),
                Connect.Schema), Connect.CurConnect);
            _daDismissed_Emp.SelectCommand.BindByName = true;
            _daDismissed_Emp.SelectCommand.Parameters.Add("p_TYPE_TRANSFER_ID", OracleDbType.Decimal).Value = 2;
            _daDismissed_Emp.SelectCommand.Parameters.Add("p_SIGN_PURPOSE_RECORD", OracleDbType.Decimal).Value = 2;
            // Update
            _daDismissed_Emp.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.DISMISSED_EMP_UPDATE(:DISMISSED_EMP_ID, :TRANSFER_ID, :DATE_PROCESSING, :SIGN_PURPOSE_RECORD); 
                END;", Connect.Schema), Connect.CurConnect);
            _daDismissed_Emp.UpdateCommand.BindByName = true;
            _daDismissed_Emp.UpdateCommand.Parameters.Add("DISMISSED_EMP_ID", OracleDbType.Decimal, 0, "DISMISSED_EMP_ID");
            _daDismissed_Emp.UpdateCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daDismissed_Emp.UpdateCommand.Parameters.Add("DATE_PROCESSING", OracleDbType.Date, 0, "DATE_PROCESSING");
            _daDismissed_Emp.UpdateCommand.Parameters.Add("SIGN_PURPOSE_RECORD", OracleDbType.Decimal, 0, "SIGN_PURPOSE_RECORD");
            InitializeComponent();
            GetViolations();
            _ds.Tables["DISMISSED_EMP"].DefaultView.RowFilter = "DATE_PROCESSING is null";
            dgTransfer_Emp.DataContext = _ds.Tables["DISMISSED_EMP"].DefaultView;
            
        }

        private void GetViolations()
        {
            _ds.Tables["DISMISSED_EMP"].Clear();
            _daDismissed_Emp.Fill(_ds.Tables["DISMISSED_EMP"]);
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["DISMISSED_EMP"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["DISMISSED_EMP"].DefaultView.RowStateFilter;
                for (int i = 0; i < _ds.Tables["DISMISSED_EMP"].DefaultView.Count; ++i)
                {
                    if (Convert.ToBoolean(_ds.Tables["DISMISSED_EMP"].DefaultView[i]["SIGN_PROCESSING"]) &&
                        _ds.Tables["DISMISSED_EMP"].DefaultView[i]["DATE_PROCESSING"] == DBNull.Value)
                        _ds.Tables["DISMISSED_EMP"].DefaultView[i]["DATE_PROCESSING"] = DateTime.Now;
                }
                _ds.Tables["DISMISSED_EMP"].DefaultView.RowStateFilter = rs;
                _daDismissed_Emp.UpdateCommand.Transaction = transact;
                _daDismissed_Emp.Update(_ds.Tables["DISMISSED_EMP"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["DISMISSED_EMP"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ds.Tables["DISMISSED_EMP"].RejectChanges();
        }

        private void dgTransfer_Emp_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((System.Windows.Controls.DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((System.Windows.Controls.DataGrid)sender).BeginEdit();
        }

        private void tbtFilter_Processing_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)tbtFilter_Processing.IsChecked)
            {
                tcDate_Processing.Visibility = System.Windows.Visibility.Visible;
                _ds.Tables["DISMISSED_EMP"].DefaultView.RowFilter = "";
            }
            else
            {
                tcDate_Processing.Visibility = System.Windows.Visibility.Collapsed;
                _ds.Tables["DISMISSED_EMP"].DefaultView.RowFilter = "DATE_PROCESSING is null";
            }
        }
    }
}
