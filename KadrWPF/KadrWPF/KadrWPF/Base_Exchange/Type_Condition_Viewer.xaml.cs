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
using Kadr;
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Data;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Type_Condition_Viewer.xaml
    /// </summary>
    public partial class Type_Condition_Viewer : UserControl
    {
        private static OracleDataAdapter _daType_Condition = new OracleDataAdapter();
        private static DataTable _dtType_Condition;
        public Type_Condition_Viewer()
        {
            InitializeComponent();
            dgType_Condition.DataContext = ProjectDataSet.Tables["TYPE_CONDITION"].DefaultView;
            dcPARENT_ID.ItemsSource = _dtType_Condition.DefaultView;
        }

        static Type_Condition_Viewer()
        {
            _dtType_Condition = ProjectDataSet.Tables["TYPE_CONDITION"].DefaultView.ToTable();
            // Insert
            _daType_Condition.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.TYPE_CONDITION_UPDATE(:TYPE_CONDITION_ID,:TYPE_CONDITION_NAME,:PARENT_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daType_Condition.InsertCommand.BindByName = true;
            _daType_Condition.InsertCommand.Parameters.Add("TYPE_CONDITION_ID", OracleDbType.Decimal, 0, "TYPE_CONDITION_ID").Direction =
                System.Data.ParameterDirection.InputOutput;
            _daType_Condition.InsertCommand.Parameters["TYPE_CONDITION_ID"].DbType = System.Data.DbType.Decimal;
            _daType_Condition.InsertCommand.Parameters.Add("TYPE_CONDITION_NAME", OracleDbType.Varchar2, 0, "TYPE_CONDITION_NAME");
            _daType_Condition.InsertCommand.Parameters.Add("PARENT_ID", OracleDbType.Decimal, 0, "PARENT_ID");
            // Update
            _daType_Condition.UpdateCommand = _daType_Condition.InsertCommand;
            // Delete
            _daType_Condition.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.TYPE_CONDITION_DELETE(:TYPE_CONDITION_ID);
                END;", Connect.Schema), Connect.CurConnect);
            _daType_Condition.DeleteCommand.BindByName = true;
            _daType_Condition.DeleteCommand.Parameters.Add("TYPE_CONDITION_ID", OracleDbType.Decimal, 0, "TYPE_CONDITION_ID");
        }

        private void Add_Type_Condition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void Add_Type_Condition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRow = ProjectDataSet.Tables["TYPE_CONDITION"].DefaultView.AddNew();
            ProjectDataSet.Tables["TYPE_CONDITION"].Rows.Add(_currentRow.Row);
            dgType_Condition.SelectedItem = _currentRow;
        }

        private void Delete_Type_Condition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgType_Condition != null && dgType_Condition.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void Delete_Type_Condition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgType_Condition.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgType_Condition.SelectedCells[0].Item).Delete();
                }
                Save_Type_Condition();
            }
            dgType_Condition.Focus();
        }

        private void Save_Type_Condition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                ProjectDataSet.Tables["TYPE_CONDITION"] != null && ProjectDataSet.Tables["TYPE_CONDITION"].GetChanges() != null)
            {
                e.CanExecute = true;
            }
        }

        private void Save_Type_Condition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Type_Condition();
        }

        void Save_Type_Condition()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daType_Condition.InsertCommand.Transaction = transact;
                _daType_Condition.UpdateCommand.Transaction = transact;
                _daType_Condition.DeleteCommand.Transaction = transact;
                _daType_Condition.Update(ProjectDataSet.Tables["TYPE_CONDITION"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void dgType_Condition_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
        }

        private void Cancel_Type_Condition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ProjectDataSet.Tables["TYPE_CONDITION"] != null && ProjectDataSet.Tables["TYPE_CONDITION"].GetChanges() != null)
            {
                e.CanExecute = true;
            }
        }

        private void Cancel_Type_Condition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectDataSet.Tables["TYPE_CONDITION"].RejectChanges();
        }

        private void _this_Closing(object sender, RoutedEventArgs e)
        {
            ProjectDataSet.Tables["TYPE_CONDITION"].RejectChanges();
        }
    }
}
