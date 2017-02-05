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

namespace WpfControlLibrary.Emps_Access
{
    /// <summary>
    /// Interaction logic for Access_Template_Viewer.xaml
    /// </summary>
    public partial class Access_Template_Viewer : UserControl
    {
        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }
        private static OracleDataAdapter _daAccess_Template = new OracleDataAdapter(), 
            _daAccess_Templ_By_Subdiv = new OracleDataAdapter(),
            _daAccess_Parameters = new OracleDataAdapter();
        private static string stFilter = "";
        public Access_Template_Viewer()
        {
            InitializeComponent();
            GetAccess_Template();
            GetAccess_Templ_By_Subdiv(-1, -1);
        }

        static Access_Template_Viewer()
        {
            _ds = new DataSet();
            _ds.Tables.Add("ACCESS_TEMPLATE");
            _ds.Tables.Add("ACCESS_TEMPLATE_ROW");
            _ds.Tables.Add("ACCESS_TEMPL_BY_SUBDIV");
            _ds.Tables.Add("ACCESS_PARAMETERS");            
            _daAccess_Template.SelectCommand = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("PO/SelectAccess_Template.sql"),
                Connect.Schema), Connect.CurConnect);
            _daAccess_Template.SelectCommand.BindByName = true;
            // Update
            _daAccess_Template.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PERCO_PKG.ACCESS_TEMPLATE_UPDATE(:ID_SHABLON_MAIN, :BEGIN_ACCESS, :END_ACCESS); 
                END;", Connect.Schema), Connect.CurConnect);
            _daAccess_Template.UpdateCommand.BindByName = true;
            _daAccess_Template.UpdateCommand.Parameters.Add("ID_SHABLON_MAIN", OracleDbType.Decimal, 0, "ID_SHABLON_MAIN");
            _daAccess_Template.UpdateCommand.Parameters.Add("BEGIN_ACCESS", OracleDbType.Date, 0, "BEGIN_ACCESS");
            _daAccess_Template.UpdateCommand.Parameters.Add("END_ACCESS", OracleDbType.Date, 0, "END_ACCESS");

            _daAccess_Templ_By_Subdiv.SelectCommand = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("PO/SelectAccess_Templ_By_Subdiv.sql"),
                Connect.Schema), Connect.CurConnect);
            _daAccess_Templ_By_Subdiv.SelectCommand.BindByName = true;
            _daAccess_Templ_By_Subdiv.SelectCommand.Parameters.Add("p_ID_SHABLON_MAIN", OracleDbType.Decimal);
            // Update
            _daAccess_Templ_By_Subdiv.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PERCO_PKG.ACCESS_TEMPLATE_UPDATE(:ID_SHABLON_MAIN, :BEGIN_ACCESS, :END_ACCESS); 
                END;", Connect.Schema), Connect.CurConnect);
            _daAccess_Templ_By_Subdiv.UpdateCommand.BindByName = true;
            _daAccess_Templ_By_Subdiv.UpdateCommand.Parameters.Add("ID_SHABLON_MAIN", OracleDbType.Decimal, 0, "ID_SHABLON_MAIN");
            _daAccess_Templ_By_Subdiv.UpdateCommand.Parameters.Add("BEGIN_ACCESS", OracleDbType.Date, 0, "BEGIN_ACCESS");
            _daAccess_Templ_By_Subdiv.UpdateCommand.Parameters.Add("END_ACCESS", OracleDbType.Date, 0, "END_ACCESS");
            
            _daAccess_Parameters.SelectCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PERCO_PKG.GET_ACCESS_PARAMETERS(:p_type_shablon, :p_ID_SHABLON_MAIN, :p_cursor);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daAccess_Parameters.SelectCommand.BindByName = true;
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_type_shablon", OracleDbType.Decimal);
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_ID_SHABLON_MAIN", OracleDbType.Decimal);
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        }

        private void GetAccess_Template()
        {
            dgAccess_Template.DataContext = null;
            _ds.Tables["ACCESS_TEMPLATE"].Clear();
            _daAccess_Template.Fill(_ds.Tables["ACCESS_TEMPLATE"]);
            dgAccess_Template.DataContext = _ds.Tables["ACCESS_TEMPLATE"].DefaultView;
        }

        private void GetAccess_Templ_By_Subdiv(object type_shablon, object id_shablon)
        {
            dgAccess_Templ_By_Subdiv.DataContext = null;
            _ds.Tables["ACCESS_TEMPL_BY_SUBDIV"].Clear();
            _daAccess_Templ_By_Subdiv.SelectCommand.Parameters["p_ID_SHABLON_MAIN"].Value = id_shablon;
            _daAccess_Templ_By_Subdiv.Fill(_ds.Tables["ACCESS_TEMPL_BY_SUBDIV"]);
            dgAccess_Templ_By_Subdiv.DataContext = _ds.Tables["ACCESS_TEMPL_BY_SUBDIV"].DefaultView;

            dgAccess_Parameters.DataContext = null;
            _ds.Tables["ACCESS_PARAMETERS"].Clear();
            _daAccess_Parameters.SelectCommand.Parameters["p_type_shablon"].Value = type_shablon;
            _daAccess_Parameters.SelectCommand.Parameters["p_ID_SHABLON_MAIN"].Value = id_shablon;
            _daAccess_Parameters.Fill(_ds.Tables["ACCESS_PARAMETERS"]);
            dgAccess_Parameters.DataContext = _ds.Tables["ACCESS_PARAMETERS"].DefaultView;
        }

        private void dgAccess_Template_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((System.Windows.Controls.DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((System.Windows.Controls.DataGrid)sender).BeginEdit();
        }

        private void SaveAccess_Template_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["ACCESS_TEMPLATE"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void SaveAccess_Template_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daAccess_Template.UpdateCommand.Transaction = transact;
                _daAccess_Template.Update(_ds.Tables["ACCESS_TEMPLATE"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void CancelAccess_Template_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["ACCESS_TEMPLATE"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void CancelAccess_Template_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ds.Tables["ACCESS_TEMPLATE"].RejectChanges();
        }

        private void UnloadTemplate_From_Perco_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (//ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["ACCESS_TEMPLATE"].GetChanges() == null)
                e.CanExecute = true;
        }

        private void UnloadTemplate_From_Perco_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleCommand _ocUnloadTemplate = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PERCO_PKG.ACCESS_TEMPLATE_SYNCHRONIZE(1);
                END;", Connect.Schema), Connect.CurConnect);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocUnloadTemplate.Transaction = transact;
                _ocUnloadTemplate.ExecuteNonQuery();
                transact.Commit();
                GetAccess_Template();
                MessageBox.Show("Шаблоны доступа обновлены из системы Perco-S-20.", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка обновления шаблонов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAccess_Templ_By_Subdiv_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (//ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["ACCESS_TEMPLATE"].GetChanges() == null &&
                dgAccess_Template != null && dgAccess_Template.SelectedCells.Count > 0)
                e.CanExecute = true;            
        }

        private void EditAccess_Templ_By_Subdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Access_Templ_By_Subdiv editSubdiv = 
                new Access_Templ_By_Subdiv(_ds.Tables["ACCESS_TEMPL_BY_SUBDIV"], ((DataRowView)_currentItem)["ID_SHABLON_MAIN"]);
            editSubdiv.ShowInTaskbar = false;
            if (editSubdiv.ShowDialog() == true)
            {
                GetAccess_Templ_By_Subdiv(((DataRowView)_currentItem)["TEMPORARY_ACC"], ((DataRowView)_currentItem)["ID_SHABLON_MAIN"]);
            }
        }

        object _currentItem;
        private void dgAccess_Template_CurrentCellChanged(object sender, EventArgs e)
        {
            if (_currentItem != dgAccess_Template.CurrentItem && dgAccess_Template.CurrentItem != null)
            {
                _currentItem = dgAccess_Template.CurrentItem;
                GetAccess_Templ_By_Subdiv(((DataRowView)_currentItem)["TEMPORARY_ACC"], ((DataRowView)_currentItem)["ID_SHABLON_MAIN"]);
            }
        }

        private void btFilter_Apply_Click(object sender, RoutedEventArgs e)
        {
            stFilter = "";
            if (tbFilterCode_Subdiv.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "ALL_CODE_SUBDIV like '%" + tbFilterCode_Subdiv.Text.Trim().PadLeft(3, '0') + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbFilterName_Access_Template.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "DISPLAY_NAME like '%" + tbFilterName_Access_Template.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            _ds.Tables["ACCESS_TEMPLATE"].DefaultView.RowFilter = stFilter;
        }

        private void btFilter_Clear_Click(object sender, RoutedEventArgs e)
        {
            stFilter = "";
            tbFilterCode_Subdiv.Text = "";
            tbFilterName_Access_Template.Text = "";
            _ds.Tables["ACCESS_TEMPLATE"].DefaultView.RowFilter = "";
        }

        private void btFind_Template_Click(object sender, RoutedEventArgs e)
        {
            Find_Template _find_Template = new Find_Template(stFilter);
            _find_Template.ShowDialog();
        }
    }
}
