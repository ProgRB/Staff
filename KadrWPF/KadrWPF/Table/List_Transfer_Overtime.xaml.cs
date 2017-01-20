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
using LibraryKadr;
using Oracle.DataAccess.Client;
using System.Data;
using Kadr;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for List_Transfer_Overtime.xaml
    /// </summary>
    public partial class List_Transfer_Overtime : Window
    {
        private DataSet _ds = new DataSet();
        private OracleDataAdapter _daEmp = new OracleDataAdapter();
        public List_Transfer_Overtime(decimal transfer_id)
        {
            InitializeComponent();
            // Select
            _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectList_Transfer_Overtime.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp.SelectCommand.BindByName = true;
            _daEmp.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = transfer_id;
            // Update
            _daEmp.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.TRANSFER_OVERTIME_UPDATE(:TRANSFER_OVERTIME_ID, :TRANSFER_ID, :SIGN_OVERTIME);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daEmp.UpdateCommand.BindByName = true;
            _daEmp.UpdateCommand.Parameters.Add("TRANSFER_OVERTIME_ID", OracleDbType.Decimal, 0, "TRANSFER_OVERTIME_ID");
            _daEmp.UpdateCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daEmp.UpdateCommand.Parameters.Add("SIGN_OVERTIME", OracleDbType.Decimal, 0, "SIGN_OVERTIME");

            _daEmp.Fill(_ds, "EMP");
            dgEmp.DataContext = _ds.Tables["EMP"].DefaultView;
        }

        private void SaveTransfer_Overtime_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && _ds.HasChanges())
                e.CanExecute = true;
        }

        private void SaveTransfer_Overtime_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daEmp.UpdateCommand.Transaction = _transact;
                _daEmp.Update(_ds.Tables["EMP"]);
                _transact.Commit();
                System.Windows.MessageBox.Show("Изменения сохранены в базе данных!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                _transact.Rollback();
                System.Windows.MessageBox.Show("Ошибка сохранения!\n" +
                    ex.Message, "АСУ \"Кадры\" - Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void dgEmp_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
        }
    }
}
