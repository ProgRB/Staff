using LibraryKadr;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace WpfControlLibrary.Вase_Exchange
{
    /// <summary>
    /// Interaction logic for Matching_List_Statement_View.xaml
    /// </summary>
    public partial class Matching_List_Statement_View : Window
    {
        OracleDataAdapter _daProject_Approval = new OracleDataAdapter();
        public Matching_List_Statement_View(object project_Transfer_ID)
        {
            InitializeComponent();

            // Select
            _daProject_Approval.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectProject_Statement_Approval.sql"),
                Connect.Schema), Connect.CurConnect);
            _daProject_Approval.SelectCommand.BindByName = true;
            _daProject_Approval.SelectCommand.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal).Value = project_Transfer_ID;
            DataTable _dt = new DataTable();
            _daProject_Approval.Fill(_dt);
            dgProject_Approval.DataContext = _dt.DefaultView;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
