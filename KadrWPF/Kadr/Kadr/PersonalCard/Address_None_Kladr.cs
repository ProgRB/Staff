using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Windows;

namespace Kadr
{
    public partial class Address_None_Kladr : Form
    {
        public static string Address_None_Kladr_Property
        {
            get 
            {
                if (_dtAddress.Rows.Count == 0)
                    return null;
                return string.Format("{0}, {1}, {2}, {3}, {4}",
                    _dtAddress.Rows[0]["NAME_REGION"],
                    _dtAddress.Rows[0]["NAME_DISTRICT"],
                    _dtAddress.Rows[0]["NAME_CITY"],
                    _dtAddress.Rows[0]["NAME_LOCALITY"],
                    _dtAddress.Rows[0]["NAME_STREET"]); 
            }
        }

        private static string _per_num = "";

        public static string Per_num
        {
            get { return Address_None_Kladr._per_num; }
            set 
            { 
                Address_None_Kladr._per_num = value;
                _daAddress.SelectCommand.Parameters["p_PER_NUM"].Value = _per_num;
                _dtAddress.Clear();
                _daAddress.Fill(_dtAddress);
            }
        }

        static DataTable _dtAddress;
        static OracleDataAdapter _daAddress;
        public Address_None_Kladr()
        {
            InitializeComponent();           

            if (_dtAddress.Rows.Count == 0)
            { 
                DataRowView _row = _dtAddress.DefaultView.AddNew();
                _row["PER_NUM"] = _per_num;
                _dtAddress.Rows.Add(_row.Row);
            }
            tbName_Region.DataBindings.Add("Text", _dtAddress, "NAME_REGION", true, DataSourceUpdateMode.OnPropertyChanged, "");
            tbName_District.DataBindings.Add("Text", _dtAddress, "NAME_DISTRICT", true, DataSourceUpdateMode.OnPropertyChanged, "");
            tbName_City.DataBindings.Add("Text", _dtAddress, "NAME_CITY", true, DataSourceUpdateMode.OnPropertyChanged, "");
            tbName_Locality.DataBindings.Add("Text", _dtAddress, "NAME_LOCALITY", true, DataSourceUpdateMode.OnPropertyChanged, "");
            tbName_Street.DataBindings.Add("Text", _dtAddress, "NAME_STREET", true, DataSourceUpdateMode.OnPropertyChanged, "");            
        }

        static Address_None_Kladr()
        {
            _dtAddress = new DataTable();
            // Select
            _daAddress = new OracleDataAdapter(string.Format(
                @"SELECT PER_NUM, NAME_REGION, NAME_DISTRICT, NAME_CITY, NAME_LOCALITY, NAME_STREET
                FROM {0}.ADDRESS_NONE_KLADR WHERE PER_NUM = :p_PER_NUM", Connect.Schema), Connect.CurConnect);
            _daAddress.SelectCommand.BindByName = true;
            _daAddress.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daAddress.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.ADDRESS_NONE_KLADR_INSERT(:PER_NUM,:NAME_REGION,:NAME_DISTRICT,:NAME_CITY,:NAME_LOCALITY,:NAME_STREET);
                END;", Connect.Schema), Connect.CurConnect);
            _daAddress.InsertCommand.BindByName = true;
            _daAddress.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daAddress.InsertCommand.Parameters.Add("NAME_REGION", OracleDbType.Varchar2, 0, "NAME_REGION");
            _daAddress.InsertCommand.Parameters.Add("NAME_DISTRICT", OracleDbType.Varchar2, 0, "NAME_DISTRICT");
            _daAddress.InsertCommand.Parameters.Add("NAME_CITY", OracleDbType.Varchar2, 0, "NAME_CITY");
            _daAddress.InsertCommand.Parameters.Add("NAME_LOCALITY", OracleDbType.Varchar2, 0, "NAME_LOCALITY");
            _daAddress.InsertCommand.Parameters.Add("NAME_STREET", OracleDbType.Varchar2, 0, "NAME_STREET");
            // Update
            _daAddress.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.ADDRESS_NONE_KLADR_UPDATE(:PER_NUM,:NAME_REGION,:NAME_DISTRICT,:NAME_CITY,:NAME_LOCALITY,:NAME_STREET);
                END;", Connect.Schema), Connect.CurConnect);
            _daAddress.UpdateCommand.BindByName = true;
            _daAddress.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daAddress.UpdateCommand.Parameters.Add("NAME_REGION", OracleDbType.Varchar2, 0, "NAME_REGION");
            _daAddress.UpdateCommand.Parameters.Add("NAME_DISTRICT", OracleDbType.Varchar2, 0, "NAME_DISTRICT");
            _daAddress.UpdateCommand.Parameters.Add("NAME_CITY", OracleDbType.Varchar2, 0, "NAME_CITY");
            _daAddress.UpdateCommand.Parameters.Add("NAME_LOCALITY", OracleDbType.Varchar2, 0, "NAME_LOCALITY");
            _daAddress.UpdateCommand.Parameters.Add("NAME_STREET", OracleDbType.Varchar2, 0, "NAME_STREET");
            // Delete
            _daAddress.DeleteCommand = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.ADDRESS_NONE_KLADR_DELETE(:PER_NUM);
                END;", Connect.Schema), Connect.CurConnect);
            _daAddress.DeleteCommand.BindByName = true;
            _daAddress.DeleteCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            _dtAddress.RejectChanges();
            Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveAddress();
            Close();
        }

        void SaveAddress()
        {
            if (_dtAddress.DefaultView.Count > 0)
            {
                _dtAddress.DefaultView[0].EndEdit();
            }
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daAddress.InsertCommand.Transaction = transact;
                _daAddress.UpdateCommand.Transaction = transact;
                _daAddress.DeleteCommand.Transaction = transact;
                _daAddress.Update(_dtAddress);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btDeleteAddress_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("Вы уверены, что нужно удалить данные?", "АСУ \"Кадры\"", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _dtAddress.DefaultView.Delete(0);
                SaveAddress();
                Close();
            }
        }
    }
}
