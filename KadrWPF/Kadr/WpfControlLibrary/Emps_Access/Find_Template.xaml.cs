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

namespace WpfControlLibrary.Emps_Access
{
    /// <summary>
    /// Interaction logic for Find_Template.xaml
    /// </summary>
    public partial class Find_Template : Window
    {
        private static DataSet _ds;
        private static OracleDataAdapter _daAccess_Parameters = new OracleDataAdapter();
        string _stFilter = "";
        public Find_Template(string stFilter)
        {
            InitializeComponent();
            _stFilter = stFilter;
            GetAccess_Parameters();

            cbDEVICE_NAME.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["DEVICE_NAME"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray()); 
            cbAREA_NAME.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["AREA_NAME"].ToString() + " - " + p["PARENT_AREA"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray()); 
            cbACCESS_1.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["ACCESS_1"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray()); 
            cbACCESS_2.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["ACCESS_2"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray());
            cbACCESS_TIME1.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["ACCESS_TIME1"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray()); 
            cbACCESS_TIME2.ItemsSource = 
                from p in _ds.Tables["ACCESS_PARAMETERS"].Select()
                group p by p["ACCESS_TIME2"].ToString() into g
                select new GroupNoteRoleApproval(g.Key, g.Select(r => Convert.ToDecimal(r["ID_SHABLON_MAIN"])).ToArray()); 
        }

        static Find_Template()
        {
            _ds = new DataSet();
            _ds.Tables.Add("ACCESS_PARAMETERS");
            _daAccess_Parameters.SelectCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PERCO_PKG.GET_ACCESS_PARAMETERS(:p_type_shablon, :p_ID_SHABLON_MAIN, :p_cursor);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daAccess_Parameters.SelectCommand.BindByName = true;
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_type_shablon", OracleDbType.Decimal).Value = 0;
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_ID_SHABLON_MAIN", OracleDbType.Decimal).Value = null;
            _daAccess_Parameters.SelectCommand.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        }

        private void GetAccess_Parameters()
        {
            _ds.Tables["ACCESS_PARAMETERS"].Clear();
            _daAccess_Parameters.Fill(_ds.Tables["ACCESS_PARAMETERS"]);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            if (cbDEVICE_NAME.SelectedValue != null)
            {
                _stFilter = string.Format("{0} {2} {1}", _stFilter, "ID_SHABLON_MAIN in (" +
                    String.Join(",", ((decimal[])cbDEVICE_NAME.SelectedValue).ToArray()) + ")",
                    _stFilter != "" ? "and" : "").Trim();
            }
            if (cbAREA_NAME.SelectedValue != null)
            {
                _stFilter = string.Format("{0} {2} {1}", _stFilter, "ID_SHABLON_MAIN in (" +
                    String.Join(",", ((decimal[])cbAREA_NAME.SelectedValue).ToArray()) + ")",
                    _stFilter != "" ? "and" : "").Trim();
            }
            Access_Template_Viewer.Ds.Tables["ACCESS_TEMPLATE"].DefaultView.RowFilter = _stFilter;
            this.DialogResult = true;
            this.Close();
        }
    }
}
