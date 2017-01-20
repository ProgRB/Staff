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
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace WpfControlLibrary.Вase_Exchange
{
    /// <summary>
    /// Interaction logic for SelectFrom_Position_For_Project.xaml
    /// </summary>
    public partial class SelectFrom_Position_For_Project : Window
    {
        private decimal _transfer_id;
        public decimal Transfer_ID { get { return _transfer_id; } }
                
        private string _per_num;
        public string Per_Num { get { return _per_num; } }

        private string _last_name;
        public string Last_Name { get { return _last_name; } }

        private string _first_name;
        public string First_Name { get { return _first_name; } }

        private string _middle_name;
        public string Middle_Name { get { return _middle_name; } }

        private string _comb;
        public string Comb { get { return _comb; } }

        private int _sign_Comb;
        public int Sign_Comb { get { return _sign_Comb; } }

        private string _pos_name;
        public string Pos_Name { get { return _pos_name; } }
        
        private string _code_subdiv;
        public string Code_Subdiv { get { return _code_subdiv; } }

        private DataSet _ds = new DataSet();
        private OracleDataAdapter _daEmp = new OracleDataAdapter();
        public SelectFrom_Position_For_Project(decimal _from_position)
        {
            InitializeComponent();
            _daEmp.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("TP/SelectList_Transfer_By_Per_Num.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEmp.SelectCommand.BindByName = true;
            _daEmp.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = _from_position;
            _daEmp.Fill(_ds, "EMP");
            dgEmp.DataContext = _ds.Tables["EMP"].DefaultView;
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
            _transfer_id = Convert.ToDecimal(drView["TRANSFER_ID"]);
            _per_num = drView["PER_NUM"].ToString();
            _last_name = drView["EMP_LAST_NAME"].ToString();
            _first_name = drView["EMP_FIRST_NAME"].ToString();
            _middle_name = drView["EMP_MIDDLE_NAME"].ToString();
            _pos_name = drView["POS_NAME"].ToString();
            _code_subdiv = drView["CODE_SUBDIV"].ToString();
            _comb = drView["SIGN_COMB"].ToString();
            _sign_Comb = Convert.ToInt16(drView["SIGN_COMB"]);
            this.DialogResult = true;
            this.Close();
        }
    }
}
