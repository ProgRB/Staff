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
    public partial class SetAccess_Template_Editor : Window
    {
        private static DataSet _ds;
        private static OracleDataAdapter _daAccess_Templ_By_Subdiv = new OracleDataAdapter();
        decimal _subdiv_id;
        DateTime? _start_Date_Valid;
        public DateTime? Start_Date_Valid
        {
            get { return _start_Date_Valid; }
            set { _start_Date_Valid = value; }
        }

        DateTime? _end_Date_Valid;
        public DateTime? End_Date_Valid
        {
            get { return _end_Date_Valid; }
            set { _end_Date_Valid = value; }
        }

        int _sign_Temporary_Shablon;
        public int Sign_Temporary_Shablon
        {
            get { return _sign_Temporary_Shablon; }
            set { _sign_Temporary_Shablon = value; }
        }

        decimal? _ID_Shablon_Main;
        public decimal? ID_Shablon_Main
        {
            get { return _ID_Shablon_Main; }
            set { _ID_Shablon_Main = value; }
        }

        public SetAccess_Template_Editor(decimal subdiv_id)
        {
            InitializeComponent();
            _subdiv_id = subdiv_id;
            GetAccess_Template_By_Subdiv();
        }

        static SetAccess_Template_Editor()
        {
            _ds = new DataSet();
            _ds.Tables.Add("ACCESS_TEMPL");
            _daAccess_Templ_By_Subdiv.SelectCommand = new OracleCommand(string.Format(
                @"select ACT.ID_SHABLON_MAIN, 
                    (select AC.DISPLAY_NAME from {0}.ACCESS_TEMPLATE AC where AC.ID_SHABLON_MAIN = ACT.ID_SHABLON_MAIN) DISPLAY_NAME
                from {0}.ACCESS_TEMPL_BY_SUBDIV ACT
                where ACT.SUBDIV_ID = :p_SUBDIV_ID",
                Connect.Schema), Connect.CurConnect);
            _daAccess_Templ_By_Subdiv.SelectCommand.BindByName = true;
            _daAccess_Templ_By_Subdiv.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
        }

        private void GetAccess_Template_By_Subdiv()
        {
            dgAccess_Templ_By_Subdiv.DataContext = null;
            _ds.Tables["ACCESS_TEMPL"].Clear();
            _daAccess_Templ_By_Subdiv.SelectCommand.Parameters["p_SUBDIV_ID"].Value = _subdiv_id;
            _daAccess_Templ_By_Subdiv.Fill(_ds.Tables["ACCESS_TEMPL"]);
            dgAccess_Templ_By_Subdiv.DataContext = _ds.Tables["ACCESS_TEMPL"].DefaultView;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btSetAccess_Template_Click(object sender, RoutedEventArgs e)
        {
            if (dpSTART_DATE_VALID.SelectedDate == null)
            {
                MessageBox.Show("Для продолжения работы нужно выбрать Дату установки шаблона!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (chSIGN_TEMPORARY_SHABLON.IsChecked == true && dpEND_DATE_VALID.SelectedDate == null)
            {
                MessageBox.Show("Так как выбран Признак временного шаблона, нужно ввести Дату окончания действия шаблона!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (dgAccess_Templ_By_Subdiv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Для продолжения работы нужно выбрать Шаблон доступа!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            _start_Date_Valid = dpSTART_DATE_VALID.SelectedDate;
            _end_Date_Valid = dpEND_DATE_VALID.SelectedDate;
            _sign_Temporary_Shablon = Convert.ToInt16(chSIGN_TEMPORARY_SHABLON.IsChecked);
            _ID_Shablon_Main = (decimal)((DataRowView)dgAccess_Templ_By_Subdiv.SelectedCells[0].Item)["ID_SHABLON_MAIN"]; 
            this.DialogResult = true;
            this.Close();
        }
    }
}
