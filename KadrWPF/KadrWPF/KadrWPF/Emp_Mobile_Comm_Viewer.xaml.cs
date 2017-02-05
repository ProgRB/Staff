using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KadrWPF
{
    /// <summary>
    /// Логика взаимодействия для Emp_Mobile_Comm_Viewer.xaml
    /// </summary>
    public partial class Emp_Mobile_Comm_Viewer : UserControl
    {
        private DataSet _ds = new DataSet();
        private OracleDataAdapter _daEmp_Mobile_Comm = new OracleDataAdapter();
        public Emp_Mobile_Comm_Viewer()
        {
            InitializeComponent();
            _ds = new DataSet();
            _ds.Tables.Add("EMP_MOB");
            _ds.Tables.Add("EMP_MOB_ROW");
            _daEmp_Mobile_Comm.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("SelectEmp_Mobile_Comm.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp_Mobile_Comm.SelectCommand.BindByName = true;
            _daEmp_Mobile_Comm.SelectCommand.Parameters.Add("p_EMP_MOBILE_COMM_ID", OracleDbType.Decimal);
            // Update
            _daEmp_Mobile_Comm.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EMP_MOBILE_COMM_UPDATE(:EMP_MOBILE_COMM_ID, :PER_NUM, :TRANSFER_ID); 
                END;", Connect.Schema), Connect.CurConnect);
            _daEmp_Mobile_Comm.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daEmp_Mobile_Comm.UpdateCommand.BindByName = true;
            _daEmp_Mobile_Comm.UpdateCommand.Parameters.Add("EMP_MOBILE_COMM_ID", OracleDbType.Decimal, 0, "EMP_MOBILE_COMM_ID").Direction = 
                ParameterDirection.InputOutput;
            _daEmp_Mobile_Comm.UpdateCommand.Parameters["EMP_MOBILE_COMM_ID"].DbType = DbType.Decimal;
            _daEmp_Mobile_Comm.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daEmp_Mobile_Comm.UpdateCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            // Insert
            _daEmp_Mobile_Comm.InsertCommand = _daEmp_Mobile_Comm.UpdateCommand;
            // Delete
            _daEmp_Mobile_Comm.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EMP_MOBILE_COMM_DELETE(:EMP_MOBILE_COMM_ID); 
                END;", Connect.Schema), Connect.CurConnect);
            _daEmp_Mobile_Comm.DeleteCommand.BindByName = true;
            _daEmp_Mobile_Comm.DeleteCommand.Parameters.Add("EMP_MOBILE_COMM_ID", OracleDbType.Decimal, 0, "EMP_MOBILE_COMM_ID");

            InitializeComponent();
            GetEmp();
            dgEmp_Mobile_Com.DataContext = _ds.Tables["EMP_MOB"].DefaultView;            
        }

        private void GetEmp()
        {
            dgEmp_Mobile_Com.DataContext = null;
            _ds.Tables["EMP_MOB"].Clear();
            _daEmp_Mobile_Comm.Fill(_ds.Tables["EMP_MOB"]);
            dgEmp_Mobile_Com.DataContext = _ds.Tables["EMP_MOB"].DefaultView;
        }

        private void AddEmp_Mobile_Comm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddEmp_Mobile_Comm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Find_Emp findEmp = new WpfControlLibrary.Find_Emp(DateTime.Today, false);
            findEmp.Owner = Window.GetWindow(this);
            if (findEmp.ShowDialog() == true)
            {
                DataRowView _currentEmp = _ds.Tables["EMP_MOB"].DefaultView.AddNew();
                _currentEmp["EMP_MOBILE_COMM_ID"] = -1;
                _currentEmp["PER_NUM"] = findEmp.Per_Num;
                _currentEmp["TRANSFER_ID"] = findEmp.Transfer_ID;
                _ds.Tables["EMP_MOB"].Rows.Add(_currentEmp.Row);
                dgEmp_Mobile_Com.SelectedItem = _currentEmp;
                if (SaveEmp_Mobile_Comm())
                    ReloadEmp(_currentEmp["EMP_MOBILE_COMM_ID"]);  
            }
        }

        public void ReloadEmp(object row_ID)
        {
            _daEmp_Mobile_Comm.SelectCommand.Parameters["p_EMP_MOBILE_COMM_ID"].Value = row_ID;
            _ds.Tables["EMP_MOB_ROW"].Clear();
            _daEmp_Mobile_Comm.Fill(_ds.Tables["EMP_MOB_ROW"]);
            if (_ds.Tables["EMP_MOB_ROW"].Rows.Count > 0)
            {
                _ds.Tables["EMP_MOB"].PrimaryKey = new DataColumn[] { _ds.Tables["EMP_MOB"].Columns["EMP_MOBILE_COMM_ID"] };
                _ds.Tables["EMP_MOB"].LoadDataRow(_ds.Tables["EMP_MOB_ROW"].Rows[0].ItemArray, LoadOption.OverwriteChanges);
            }
            else
            {
                _ds.Tables["EMP_MOB"].PrimaryKey = new DataColumn[] { _ds.Tables["EMP_MOB"].Columns["EMP_MOBILE_COMM_ID"] };
                _ds.Tables["EMP_MOB"].Rows.Remove(_ds.Tables["EMP_MOB"].Rows.Find(row_ID));
            }
            _daEmp_Mobile_Comm.SelectCommand.Parameters["p_EMP_MOBILE_COMM_ID"].Value = null;
            dgEmp_Mobile_Com.Items.Refresh();
        }

        private void EditEmp_Mobile_Comm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgEmp_Mobile_Com != null && dgEmp_Mobile_Com.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditEmp_Mobile_Comm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Convert.ToBoolean(((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item)["SIGN_DISMISS_EMP"]) == false)
            {
                if (Convert.ToDecimal(((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item)["TRANSFER_ID"]) !=
                    Convert.ToDecimal(((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item)["TRANSFER_ID_CUR"]))
                {
                    ((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item)["TRANSFER_ID"] =
                        ((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item)["TRANSFER_ID_CUR"];
                    SaveEmp_Mobile_Comm();
                    ReloadEmp(null);
                }
                else
                {
                    MessageBox.Show("Редактирование не нужно, так как данные абонента актуальны.",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
                MessageBox.Show("Редактирование невозможно, так как данные абонента устарели.",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void DeleteEmp_Mobile_Comm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgEmp_Mobile_Com.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgEmp_Mobile_Com.SelectedCells[0].Item).Delete();
                }
                SaveEmp_Mobile_Comm();
            }
            dgEmp_Mobile_Com.Focus();
        }

        private bool SaveEmp_Mobile_Comm()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daEmp_Mobile_Comm.InsertCommand.Transaction = transact;
                _daEmp_Mobile_Comm.UpdateCommand.Transaction = transact;
                _daEmp_Mobile_Comm.DeleteCommand.Transaction = transact;
                _daEmp_Mobile_Comm.Update(_ds.Tables["EMP_MOB"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                _ds.Tables["EMP_MOB"].RejectChanges();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            CommandManager.InvalidateRequerySuggested();
            return true;
        }

        private void btRefreshState_Click(object sender, RoutedEventArgs e)
        {
            GetEmp();
        }
    }
}
