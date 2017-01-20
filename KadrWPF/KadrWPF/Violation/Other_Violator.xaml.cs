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
using System.ComponentModel;
using System.Globalization;

namespace Pass_Office
{
    /// <summary>
    /// Interaction logic for Find_Violator.xaml
    /// </summary>
    public partial class Other_Violator : Window
    {        
        private decimal _other_violator_id;
        public decimal Other_Violator_ID { get { return _other_violator_id; } }

        private string _last_name;
        public string Last_Name{ get { return _last_name; }}

        private string _first_name;
        public string First_Name { get { return _first_name; } }

        private string _middle_name;
        public string Middle_Name { get { return _middle_name; } }

        private string _pos_name;
        public string Pos_Name{ get { return _pos_name; }}

        private string _subdiv_name;
        public string Subdiv_Name { get { return _subdiv_name; } }

        private static DataSet _ds = new DataSet();
        private static OracleDataAdapter _daEmp = new OracleDataAdapter();
        private OracleCommand _oc;
        public Other_Violator()
        {
            InitializeComponent();
            GetEmp();
            _ds.Tables["O_V"].DefaultView.RowFilter = "";
            dgEmp.ItemsSource = _ds.Tables["O_V"].DefaultView;
            tbLast_Name.Focus();

            _oc = new OracleCommand(string.Format(
                    @"select count(*) from {0}.VIOLATOR where OTHER_VIOLATOR_ID = :OTHER_VIOLATOR_ID",
                    Connect.Schema), Connect.CurConnect);
            _oc.BindByName = true;
            _oc.Parameters.Add("OTHER_VIOLATOR_ID", OracleDbType.Decimal);
        }
        
        static Other_Violator()
        {          
            _ds.Tables.Add("O_V");
            _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("PO/SelectOther_Violator.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp.InsertCommand = new OracleCommand(string.Format(
                @"insert into {0}.OTHER_VIOLATOR(OTHER_VIOLATOR_ID, OT_LAST_NAME, OT_FIRST_NAME, OT_MIDDLE_NAME, OV_POS_NAME, OV_SUBDIV_NAME)
                values(:OTHER_VIOLATOR_ID, :OT_LAST_NAME, :OT_FIRST_NAME, :OT_MIDDLE_NAME, :OV_POS_NAME, :OV_SUBDIV_NAME)", 
                Connect.Schema), Connect.CurConnect);
            _daEmp.InsertCommand.BindByName = true;
            _daEmp.InsertCommand.Parameters.Add("OTHER_VIOLATOR_ID", OracleDbType.Decimal, 0, "OTHER_VIOLATOR_ID");
            _daEmp.InsertCommand.Parameters.Add("OT_LAST_NAME", OracleDbType.Varchar2, 0, "LAST_NAME");
            _daEmp.InsertCommand.Parameters.Add("OT_FIRST_NAME", OracleDbType.Varchar2, 0, "FIRST_NAME");
            _daEmp.InsertCommand.Parameters.Add("OT_MIDDLE_NAME", OracleDbType.Varchar2, 0, "MIDDLE_NAME");
            _daEmp.InsertCommand.Parameters.Add("OV_POS_NAME", OracleDbType.Varchar2, 0, "POS_NAME");
            _daEmp.InsertCommand.Parameters.Add("OV_SUBDIV_NAME", OracleDbType.Varchar2, 0, "SUBDIV_NAME");
            _daEmp.DeleteCommand = new OracleCommand(string.Format(
                @"delete from {0}.OTHER_VIOLATOR where OTHER_VIOLATOR_ID = :OTHER_VIOLATOR_ID",
                Connect.Schema), Connect.CurConnect);
            _daEmp.DeleteCommand.BindByName = true;
            _daEmp.DeleteCommand.Parameters.Add("OTHER_VIOLATOR_ID", OracleDbType.Decimal, 0, "OTHER_VIOLATOR_ID");
        }

        void GetEmp()
        {
            dgEmp.DataContext = null;
            _ds.Tables["O_V"].Clear();
            _daEmp.Fill(_ds.Tables["O_V"]);
            dgEmp.DataContext = _ds.Tables["O_V"].DefaultView;
        }

        private void Find_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strFilter = "";
            if (tbLast_Name.Text.Trim() != "")
                strFilter = "LAST_NAME like '" + tbLast_Name.Text.Trim().ToUpper() + "%'";
            if (tbFirst_Name.Text.Trim() != "")
                strFilter = (strFilter != "" ? strFilter + " and " : "") + "FIRST_NAME like '" + tbFirst_Name.Text.Trim().ToUpper() + "%'";
            if (tbMiddle_Name.Text.Trim() != "")
                strFilter = (strFilter != "" ? strFilter + " and " : "") + "MIDDLE_NAME like '" + tbMiddle_Name.Text.Trim().ToUpper() + "%'";
            _ds.Tables["O_V"].DefaultView.RowFilter = strFilter;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dgEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WpfControlLibrary.Pass_Office_Commands.SelectEmp.Execute(null, null);
        }

        private void SelectEmp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (dgEmp != null && dgEmp.SelectedItem != null)
            {
                e.CanExecute = true;
            }
        }

        private void SelectEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView drView = dgEmp.SelectedItem as DataRowView;
            _other_violator_id = Convert.ToDecimal(drView["OTHER_VIOLATOR_ID"]);
            _last_name = drView["LAST_NAME"].ToString();
            _first_name = drView["FIRST_NAME"].ToString();
            _middle_name = drView["MIDDLE_NAME"].ToString();
            _pos_name = drView["POS_NAME"].ToString();
            _subdiv_name = drView["SUBDIV_NAME"].ToString();
            this.DialogResult = true;
            this.Close();
        }

        private void Add_Other_Violator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["O_V"].DefaultView.Count == 0)
                e.CanExecute = true;
        }

        private void Add_Other_Violator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView newRow = _ds.Tables["O_V"].DefaultView.AddNew();
            newRow["OTHER_VIOLATOR_ID"] = new OracleCommand(string.Format("select {0}.OTHER_VIOLATOR_ID_seq.NEXTVAL from dual",
                Connect.Schema), Connect.CurConnect).ExecuteScalar();
            newRow["LAST_NAME"] = tbLast_Name.Text;
            newRow["FIRST_NAME"] = tbFirst_Name.Text;
            newRow["MIDDLE_NAME"] = tbMiddle_Name.Text;
            newRow["POS_NAME"] = tbPos_Name.Text;
            newRow["SUBDIV_NAME"] = tbSubdiv_Name.Text;
            _ds.Tables["O_V"].Rows.Add(newRow.Row);
            SaveOther_Violator();
        }

        void SaveOther_Violator()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daEmp.InsertCommand.Transaction = transact;
                _daEmp.DeleteCommand.Transaction = transact;
                _daEmp.Update(_ds.Tables["O_V"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                _ds.Tables["O_V"].RejectChanges();
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Delete_Other_Violator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["O_V"].DefaultView.Count != 0 && dgEmp.SelectedItem != null)
                e.CanExecute = true;
        }

        private void Delete_Other_Violator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _oc.Parameters["OTHER_VIOLATOR_ID"].Value = ((DataRowView)dgEmp.SelectedItems[0])["OTHER_VIOLATOR_ID"];
                int _count = Convert.ToInt16(_oc.ExecuteScalar());
                if (_count == 0)
                {
                    while (dgEmp.SelectedItems.Count > 0)
                    {
                        ((DataRowView)dgEmp.SelectedItems[0]).Delete();
                    }
                    SaveOther_Violator();
                }
                else
                {
                    MessageBox.Show("Нельзя удалять запись, так как по данному нарушителю есть нарушения!", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);                    
                }
            }
            dgEmp.Focus();
        }
    }

    public class VisiblePer_Num_ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt16(value) == 1)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
