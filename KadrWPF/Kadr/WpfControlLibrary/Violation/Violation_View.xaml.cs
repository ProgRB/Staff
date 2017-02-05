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
using System.Globalization;
using System.Data;
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.ComponentModel;
using Staff;
using Oracle.DataAccess.Types;
using System.IO;
using System.Runtime.InteropServices;
using Kadr;
using WpfControlLibrary;

namespace Pass_Office
{
    /// <summary>
    /// Interaction logic for Violation_View.xaml
    /// </summary>
    public partial class Violation_View : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string isEnabledControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(isEnabledControl));
            }
        }

        private static bool _fl_reload = false;
        public static bool Fl_reload
        {
            get { return _fl_reload; }
            set { _fl_reload = value; }
        }

        private static bool _fl_Delete_Violation = false;
        public static bool Fl_Delete_Violation
        {
            get { return _fl_Delete_Violation; }
            set { _fl_Delete_Violation = value; }
        }

        private bool _isEnabledStolen_Property;
        public bool IsEnabledStolen_Property
        {
            get { return this._isEnabledStolen_Property; }
            set
            {
                if (value != this._isEnabledStolen_Property)
                {
                    this._isEnabledStolen_Property = value;
                    OnPropertyChanged("IsEnabledStolen_Property");
                }
            }
        }

        private bool _isEnabledPunishment;
        public bool IsEnabledPunishment
        {
            get { return this._isEnabledPunishment; }
            set
            {
                if (value != this._isEnabledPunishment)
                {
                    this._isEnabledPunishment = value;
                    OnPropertyChanged("IsEnabledPunishment");
                }
            }
        }
        
        private static DataSet _ds = new DataSet();
        public static DataSet Ds
        {
            get { return _ds; }
        }
        private static OracleDataAdapter _daReason = new OracleDataAdapter(), _daSign = new OracleDataAdapter(), 
            _daTSP = new OracleDataAdapter(), _daStolen_Property = new OracleDataAdapter(),
            _daPunishment = new OracleDataAdapter(), _daList_Pun = new OracleDataAdapter(), 
            _daType_Pun = new OracleDataAdapter(), _daPercent_Pun = new OracleDataAdapter();
        private static OracleCommand _ocPhotoEmp, _ocPhotoFR_Emp;
        DataRowView rowStolen_Property, rowPunishment;
        public Violation_View(object dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
            cbSign_Detention.ItemsSource = _ds.Tables["SIGN_DETENTION"].DefaultView;
            cbReason_Detention.ItemsSource = _ds.Tables["REASON_DETENTION"].DefaultView;
            cbTSP.ItemsSource = _ds.Tables["TSP"].DefaultView;
            if (((DataRowView)this.DataContext)["TRANSFER_ID"] != DBNull.Value)
            {
                _ocPhotoEmp.Parameters["p_PER_NUM"].Value = ((DataRowView)this.DataContext)["PER_NUM"];
                _ocPhotoEmp.ExecuteNonQuery();
                if (!(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                    imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                        new MemoryStream((byte[])(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);
            }
            else
                if (((DataRowView)this.DataContext)["PERCO_SYNC_ID"] != DBNull.Value)
                {
                    rdFR_Emp.IsChecked = true;
                    _ocPhotoFR_Emp.Parameters["p_PERCO_SYNC_ID"].Value = ((DataRowView)this.DataContext)["PERCO_SYNC_ID"];
                    _ocPhotoFR_Emp.ExecuteNonQuery();
                    if (!(_ocPhotoFR_Emp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                        imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                            new MemoryStream((byte[])(_ocPhotoFR_Emp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);
                }
                else
                    if (((DataRowView)this.DataContext)["OTHER_VIOLATOR_ID"] != DBNull.Value)
                    {
                        rdOther_Emp.IsChecked = true;
                        imPhoto.Source = null;
                    }
            _daStolen_Property.SelectCommand.Parameters["p_VIOLATION_ID"].Value = ((DataRowView)this.DataContext)["VIOLATION_ID"];
            _ds.Tables["STOLEN_PROPERTY"].Clear();
            _daStolen_Property.Fill(_ds.Tables["STOLEN_PROPERTY"]);
            if (_ds.Tables["STOLEN_PROPERTY"].Rows.Count > 0)
            {
                IsEnabledStolen_Property = true;
                rowStolen_Property = _ds.Tables["STOLEN_PROPERTY"].DefaultView[0];
            }
            else
            {
                IsEnabledStolen_Property = false;
                rowStolen_Property = _ds.Tables["STOLEN_PROPERTY"].DefaultView.AddNew();
            }
            tiStolen_Property.DataContext = rowStolen_Property;

            _daPunishment.SelectCommand.Parameters["p_VIOLATION_ID"].Value = ((DataRowView)this.DataContext)["VIOLATION_ID"];
            _ds.Tables["PUNISHMENT"].Clear();
            _daPunishment.Fill(_ds.Tables["PUNISHMENT"]);
            if (_ds.Tables["PUNISHMENT"].Rows.Count > 0)
            {
                IsEnabledPunishment = true;
                rowPunishment = _ds.Tables["PUNISHMENT"].DefaultView[0];
                _daList_Pun.SelectCommand.Parameters["p_PUNISHMENT_ID"].Value = _ds.Tables["PUNISHMENT"].Rows[0]["PUNISHMENT_ID"];
            }
            else
            {
                IsEnabledPunishment = false;
                rowPunishment = _ds.Tables["PUNISHMENT"].DefaultView.AddNew();
                _daList_Pun.SelectCommand.Parameters["p_PUNISHMENT_ID"].Value = null;
            }
            tiPunishment.DataContext = rowPunishment;

            _ds.Tables["LP"].Clear();
            _daList_Pun.SelectCommand.Parameters["p_SIGN_PUNISHMENT_CHIEF"].Value = 0;
            _daList_Pun.Fill(_ds.Tables["LP"]);
            dgList_Punishment.ItemsSource = _ds.Tables["LP"].DefaultView;
            _ds.Tables["LP_CHIEF"].Clear();
            _daList_Pun.SelectCommand.Parameters["p_SIGN_PUNISHMENT_CHIEF"].Value = 1;
            _daList_Pun.Fill(_ds.Tables["LP_CHIEF"]);
            dgList_Punishment_Chief.ItemsSource = _ds.Tables["LP_CHIEF"].DefaultView;

            _ds.Tables["TP"].Clear();
            _daType_Pun.Fill(_ds.Tables["TP"]);
            dcTP.ItemsSource = _ds.Tables["TP"].DefaultView;
            dcTP_CHIEF.ItemsSource = _ds.Tables["TP"].DefaultView;
            _ds.Tables["PP"].Clear();
            _daPercent_Pun.Fill(_ds.Tables["PP"]);
            dcPP.ItemsSource = _ds.Tables["PP"].DefaultView;
            dcPP_CHIEF.ItemsSource = _ds.Tables["PP"].DefaultView;

            _fl_reload = false;
            _fl_Delete_Violation = false;
        }

        static Violation_View()
        {
            _ds.Tables.Add("REASON_DETENTION");
            _daReason.SelectCommand = new OracleCommand(string.Format(
                "select REASON_DETENTION_ID, REASON_DETENTION_NAME, SIGN_THEFT from {0}.REASON_DETENTION ORDER BY REASON_DETENTION_NAME",
                Connect.Schema), Connect.CurConnect);
            _daReason.Fill(_ds.Tables["REASON_DETENTION"]);

            _ds.Tables.Add("SIGN_DETENTION");
            _daSign.SelectCommand = new OracleCommand(string.Format(
                "select SIGN_DETENTION_ID, SIGN_DETENTION_NAME from {0}.SIGN_DETENTION ORDER BY SIGN_DETENTION_NAME",
                Connect.Schema), Connect.CurConnect);
            _daSign.Fill(_ds.Tables["SIGN_DETENTION"]);

            _ds.Tables.Add("TSP");
            _daTSP.SelectCommand = new OracleCommand(string.Format(
                "select TYPE_STOLEN_PROPERTY_ID, TYPE_STOLEN_PROPERTY_NAME from {0}.TYPE_STOLEN_PROPERTY ORDER BY TYPE_STOLEN_PROPERTY_NAME",
                Connect.Schema), Connect.CurConnect);
            _daTSP.Fill(_ds.Tables["TSP"]);

            _daStolen_Property.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/SelectStolen_Property.sql"), Connect.Schema), Connect.CurConnect);
            _daStolen_Property.SelectCommand.BindByName = true;
            _daStolen_Property.SelectCommand.Parameters.Add("p_VIOLATION_ID", OracleDbType.Decimal);
            _ds.Tables.Add("STOLEN_PROPERTY");
            _daStolen_Property.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.STOLEN_PROPERTY_insert(:STOLEN_PROPERTY_ID, :VIOLATION_ID, :TYPE_STOLEN_PROPERTY_ID, :UNIT_OF_MEASUREMENT, :COUNT_STOLEN, 
                    :SUM_STOLEN, :SIGN_CRIMINAL_PROSECUTION, :CRIMINAL_CASE_NUMBER, :DATE_OF_INITIATION, :DATE_OF_DECISION, :SIGN_RESTRICTED_ACCESS_INFORM);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daStolen_Property.InsertCommand.BindByName = true;
            _daStolen_Property.InsertCommand.Parameters.Add("STOLEN_PROPERTY_ID", OracleDbType.Decimal, 0, "STOLEN_PROPERTY_ID");
            _daStolen_Property.InsertCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daStolen_Property.InsertCommand.Parameters.Add("TYPE_STOLEN_PROPERTY_ID", OracleDbType.Decimal, 0, "TYPE_STOLEN_PROPERTY_ID");
            _daStolen_Property.InsertCommand.Parameters.Add("UNIT_OF_MEASUREMENT", OracleDbType.Varchar2, 0, "UNIT_OF_MEASUREMENT");
            _daStolen_Property.InsertCommand.Parameters.Add("COUNT_STOLEN", OracleDbType.Decimal, 0, "COUNT_STOLEN");
            _daStolen_Property.InsertCommand.Parameters.Add("SUM_STOLEN", OracleDbType.Decimal, 0, "SUM_STOLEN");
            _daStolen_Property.InsertCommand.Parameters.Add("SIGN_CRIMINAL_PROSECUTION", OracleDbType.Decimal, 0, "SIGN_CRIMINAL_PROSECUTION");
            _daStolen_Property.InsertCommand.Parameters.Add("CRIMINAL_CASE_NUMBER", OracleDbType.Varchar2, 0, "CRIMINAL_CASE_NUMBER");
            _daStolen_Property.InsertCommand.Parameters.Add("DATE_OF_INITIATION", OracleDbType.Date, 0, "DATE_OF_INITIATION");
            _daStolen_Property.InsertCommand.Parameters.Add("DATE_OF_DECISION", OracleDbType.Date, 0, "DATE_OF_DECISION");
            _daStolen_Property.InsertCommand.Parameters.Add("SIGN_RESTRICTED_ACCESS_INFORM", OracleDbType.Decimal, 0, "SIGN_RESTRICTED_ACCESS_INFORM");
            _daStolen_Property.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.STOLEN_PROPERTY_update(:STOLEN_PROPERTY_ID, :VIOLATION_ID, :TYPE_STOLEN_PROPERTY_ID, :UNIT_OF_MEASUREMENT, :COUNT_STOLEN, 
                    :SUM_STOLEN, :SIGN_CRIMINAL_PROSECUTION, :CRIMINAL_CASE_NUMBER, :DATE_OF_INITIATION, :DATE_OF_DECISION, :SIGN_RESTRICTED_ACCESS_INFORM);
                END;",
                Connect.Schema), Connect.CurConnect);
            _daStolen_Property.UpdateCommand.BindByName = true;
            _daStolen_Property.UpdateCommand.Parameters.Add("STOLEN_PROPERTY_ID", OracleDbType.Decimal, 0, "STOLEN_PROPERTY_ID");
            _daStolen_Property.UpdateCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daStolen_Property.UpdateCommand.Parameters.Add("TYPE_STOLEN_PROPERTY_ID", OracleDbType.Decimal, 0, "TYPE_STOLEN_PROPERTY_ID");
            _daStolen_Property.UpdateCommand.Parameters.Add("UNIT_OF_MEASUREMENT", OracleDbType.Varchar2, 0, "UNIT_OF_MEASUREMENT");
            _daStolen_Property.UpdateCommand.Parameters.Add("COUNT_STOLEN", OracleDbType.Decimal, 0, "COUNT_STOLEN");
            _daStolen_Property.UpdateCommand.Parameters.Add("SUM_STOLEN", OracleDbType.Decimal, 0, "SUM_STOLEN");
            _daStolen_Property.UpdateCommand.Parameters.Add("SIGN_CRIMINAL_PROSECUTION", OracleDbType.Decimal, 0, "SIGN_CRIMINAL_PROSECUTION");
            _daStolen_Property.UpdateCommand.Parameters.Add("CRIMINAL_CASE_NUMBER", OracleDbType.Varchar2, 0, "CRIMINAL_CASE_NUMBER");
            _daStolen_Property.UpdateCommand.Parameters.Add("DATE_OF_INITIATION", OracleDbType.Date, 0, "DATE_OF_INITIATION");
            _daStolen_Property.UpdateCommand.Parameters.Add("DATE_OF_DECISION", OracleDbType.Date, 0, "DATE_OF_DECISION");
            _daStolen_Property.UpdateCommand.Parameters.Add("SIGN_RESTRICTED_ACCESS_INFORM", OracleDbType.Decimal, 0, "SIGN_RESTRICTED_ACCESS_INFORM");
            _daStolen_Property.DeleteCommand = new OracleCommand(string.Format(
                "delete from {0}.STOLEN_PROPERTY where STOLEN_PROPERTY_ID = :STOLEN_PROPERTY_ID",
                Connect.Schema), Connect.CurConnect);
            _daStolen_Property.DeleteCommand.BindByName = true;
            _daStolen_Property.DeleteCommand.Parameters.Add("STOLEN_PROPERTY_ID", OracleDbType.Decimal, 0, "STOLEN_PROPERTY_ID");

            _daPunishment.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/SelectPunishment.sql"), Connect.Schema), Connect.CurConnect);
            _daPunishment.SelectCommand.BindByName = true;
            _daPunishment.SelectCommand.Parameters.Add("p_VIOLATION_ID", OracleDbType.Decimal);
            _ds.Tables.Add("PUNISHMENT");
            _daPunishment.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.PUNISHMENT_INSERT(:PUNISHMENT_ID, :VIOLATION_ID, :PUNISHMENT_NUM_ORDER, :PUNISHMENT_DATE_ORDER, :CHIEF_TRANSFER_ID); END;",
                Connect.Schema), Connect.CurConnect);
            _daPunishment.InsertCommand.BindByName = true;
            _daPunishment.InsertCommand.Parameters.Add("PUNISHMENT_ID", OracleDbType.Decimal, 0, "PUNISHMENT_ID");
            _daPunishment.InsertCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daPunishment.InsertCommand.Parameters.Add("PUNISHMENT_NUM_ORDER", OracleDbType.Varchar2, 0, "PUNISHMENT_NUM_ORDER");
            _daPunishment.InsertCommand.Parameters.Add("PUNISHMENT_DATE_ORDER", OracleDbType.Date, 0, "PUNISHMENT_DATE_ORDER");
            _daPunishment.InsertCommand.Parameters.Add("CHIEF_TRANSFER_ID", OracleDbType.Decimal, 0, "CHIEF_TRANSFER_ID");
            _daPunishment.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.PUNISHMENT_UPDATE(:PUNISHMENT_ID, :VIOLATION_ID, :PUNISHMENT_NUM_ORDER, :PUNISHMENT_DATE_ORDER, :CHIEF_TRANSFER_ID); END;",
                Connect.Schema), Connect.CurConnect);
            _daPunishment.UpdateCommand.BindByName = true;
            _daPunishment.UpdateCommand.Parameters.Add("PUNISHMENT_ID", OracleDbType.Decimal, 0, "PUNISHMENT_ID");
            _daPunishment.UpdateCommand.Parameters.Add("VIOLATION_ID", OracleDbType.Decimal, 0, "VIOLATION_ID");
            _daPunishment.UpdateCommand.Parameters.Add("PUNISHMENT_NUM_ORDER", OracleDbType.Varchar2, 0, "PUNISHMENT_NUM_ORDER");
            _daPunishment.UpdateCommand.Parameters.Add("PUNISHMENT_DATE_ORDER", OracleDbType.Date, 0, "PUNISHMENT_DATE_ORDER");
            _daPunishment.UpdateCommand.Parameters.Add("CHIEF_TRANSFER_ID", OracleDbType.Decimal, 0, "CHIEF_TRANSFER_ID");
            _daPunishment.DeleteCommand = new OracleCommand(string.Format(
                "delete from {0}.PUNISHMENT where PUNISHMENT_ID = :PUNISHMENT_ID",
                Connect.Schema), Connect.CurConnect);
            _daPunishment.DeleteCommand.BindByName = true;
            _daPunishment.DeleteCommand.Parameters.Add("PUNISHMENT_ID", OracleDbType.Decimal, 0, "PUNISHMENT_ID");

            _daList_Pun.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/SelectList_Punishment.sql"), Connect.Schema), Connect.CurConnect);
            _daList_Pun.SelectCommand.BindByName = true;
            _daList_Pun.SelectCommand.Parameters.Add("p_PUNISHMENT_ID", OracleDbType.Decimal);
            _daList_Pun.SelectCommand.Parameters.Add("p_SIGN_PUNISHMENT_CHIEF", OracleDbType.Decimal);
            _ds.Tables.Add("LP");
            _ds.Tables.Add("LP_CHIEF");
            _daList_Pun.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.LIST_OF_PUNISHMENT_INSERT(:LIST_OF_PUNISHMENT_ID, :PUNISHMENT_ID, :TYPE_PUNISHMENT_ID, :PERCENT_PUNISHMENT, :SIGN_PUNISHMENT_CHIEF); END;",
                Connect.Schema), Connect.CurConnect);
            _daList_Pun.InsertCommand.BindByName = true;
            _daList_Pun.InsertCommand.Parameters.Add("LIST_OF_PUNISHMENT_ID", OracleDbType.Decimal, 0, "LIST_OF_PUNISHMENT_ID");
            _daList_Pun.InsertCommand.Parameters.Add("PUNISHMENT_ID", OracleDbType.Decimal, 0, "PUNISHMENT_ID");
            _daList_Pun.InsertCommand.Parameters.Add("TYPE_PUNISHMENT_ID", OracleDbType.Decimal, 0, "TYPE_PUNISHMENT_ID");
            _daList_Pun.InsertCommand.Parameters.Add("PERCENT_PUNISHMENT", OracleDbType.Decimal, 0, "PERCENT_PUNISHMENT");
            _daList_Pun.InsertCommand.Parameters.Add("SIGN_PUNISHMENT_CHIEF", OracleDbType.Int16, 0, "SIGN_PUNISHMENT_CHIEF");
            _daList_Pun.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.LIST_OF_PUNISHMENT_UPDATE(:LIST_OF_PUNISHMENT_ID, :PUNISHMENT_ID, :TYPE_PUNISHMENT_ID, :PERCENT_PUNISHMENT, :SIGN_PUNISHMENT_CHIEF); END;",
                Connect.Schema), Connect.CurConnect);
            _daList_Pun.UpdateCommand.BindByName = true;
            _daList_Pun.UpdateCommand.Parameters.Add("LIST_OF_PUNISHMENT_ID", OracleDbType.Decimal, 0, "LIST_OF_PUNISHMENT_ID");
            _daList_Pun.UpdateCommand.Parameters.Add("PUNISHMENT_ID", OracleDbType.Decimal, 0, "PUNISHMENT_ID");
            _daList_Pun.UpdateCommand.Parameters.Add("TYPE_PUNISHMENT_ID", OracleDbType.Decimal, 0, "TYPE_PUNISHMENT_ID");
            _daList_Pun.UpdateCommand.Parameters.Add("PERCENT_PUNISHMENT", OracleDbType.Decimal, 0, "PERCENT_PUNISHMENT");
            _daList_Pun.UpdateCommand.Parameters.Add("SIGN_PUNISHMENT_CHIEF", OracleDbType.Int16, 0, "SIGN_PUNISHMENT_CHIEF");
            _daList_Pun.DeleteCommand = new OracleCommand(string.Format(
                "delete from {0}.LIST_OF_PUNISHMENT where LIST_OF_PUNISHMENT_ID = :LIST_OF_PUNISHMENT_ID",
                Connect.Schema), Connect.CurConnect);
            _daList_Pun.DeleteCommand.BindByName = true;
            _daList_Pun.DeleteCommand.Parameters.Add("LIST_OF_PUNISHMENT_ID", OracleDbType.Decimal, 0, "LIST_OF_PUNISHMENT_ID");

            _daType_Pun.SelectCommand = new OracleCommand(string.Format(
                @"select TYPE_PUNISHMENT_ID, TYPE_PUNISHMENT_NAME, SIGN_FINANCIAL, SIGN_DELETE_VIOLATION, TYPE_GROUP_PUNISHMENT_ID 
                from {0}.TYPE_PUNISHMENT order by TYPE_PUNISHMENT_NAME",
                Connect.Schema), Connect.CurConnect);
            _ds.Tables.Add("TP");

            _daPercent_Pun.SelectCommand = new OracleCommand(string.Format(
                Queries.GetQuery("PO/SelectPercent_Punishment.sql"), Connect.Schema), Connect.CurConnect);
            _ds.Tables.Add("PP");

            _ocPhotoEmp = new OracleCommand(string.Format(
                "begin select E.PHOTO into :p_PHOTO from {0}.EMP E where PER_NUM = :p_PER_NUM; end;", 
                Connect.Schema), Connect.CurConnect);
            _ocPhotoEmp.BindByName = true;
            _ocPhotoEmp.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _ocPhotoEmp.Parameters.Add("p_PHOTO", OracleDbType.Blob).Direction = ParameterDirection.Output;

            _ocPhotoFR_Emp = new OracleCommand(string.Format(
                "begin select FR.FR_PHOTO into :p_PHOTO from {0}.FR_EMP FR where FR.PERCO_SYNC_ID = :p_PERCO_SYNC_ID; end;", 
                Connect.Schema), Connect.CurConnect);
            _ocPhotoFR_Emp.BindByName = true;
            _ocPhotoFR_Emp.Parameters.Add("p_PERCO_SYNC_ID", OracleDbType.Decimal);
            _ocPhotoFR_Emp.Parameters.Add("p_PHOTO", OracleDbType.Blob).Direction = ParameterDirection.Output;
        }

        private void Find_Violator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Find_Violator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool? _dialogResult;
            if (((Button)sender).Name == "btFind_Emp")
            {
                Find_Violator find = new Find_Violator(1);
                _dialogResult = find.ShowDialog();
                if (_dialogResult == true)
                {
                    ((DataRowView)this.DataContext)["TRANSFER_ID"] = find.Emp_ID;
                    ((DataRowView)this.DataContext)["PERCO_SYNC_ID"] = DBNull.Value;
                    ((DataRowView)this.DataContext)["OTHER_VIOLATOR_ID"] = DBNull.Value;
                    ((DataRowView)this.DataContext)["PER_NUM"] = find.Per_Num;
                    ((DataRowView)this.DataContext)["LAST_NAME"] = find.Last_Name;
                    ((DataRowView)this.DataContext)["FIRST_NAME"] = find.First_Name;
                    ((DataRowView)this.DataContext)["MIDDLE_NAME"] = find.Middle_Name;
                    ((DataRowView)this.DataContext)["CODE_SUBDIV"] = find.Code_Subdiv;
                    ((DataRowView)this.DataContext)["SUBDIV_NAME"] = find.Subdiv_Name;
                    ((DataRowView)this.DataContext)["POS_NAME"] = find.Pos_Name;
                    _ocPhotoEmp.Parameters["p_PER_NUM"].Value = ((DataRowView)this.DataContext)["PER_NUM"];
                    _ocPhotoEmp.ExecuteNonQuery();
                    if (!(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                        imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                            new MemoryStream((byte[])(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);
                    else
                        imPhoto.Source = null;
                }
            }
            else
                if (((Button)sender).Name == "btFind_FR_Emp")
                {
                    Find_Violator find = new Find_Violator(2);
                    _dialogResult = find.ShowDialog();
                    if (_dialogResult == true)
                    {
                        ((DataRowView)this.DataContext)["TRANSFER_ID"] = DBNull.Value;
                        ((DataRowView)this.DataContext)["PERCO_SYNC_ID"] = find.Emp_ID;
                        ((DataRowView)this.DataContext)["OTHER_VIOLATOR_ID"] = DBNull.Value;
                        ((DataRowView)this.DataContext)["PER_NUM"] = find.Per_Num;
                        ((DataRowView)this.DataContext)["LAST_NAME"] = find.Last_Name;
                        ((DataRowView)this.DataContext)["FIRST_NAME"] = find.First_Name;
                        ((DataRowView)this.DataContext)["MIDDLE_NAME"] = find.Middle_Name;
                        ((DataRowView)this.DataContext)["CODE_SUBDIV"] = find.Subdiv_Name;
                        ((DataRowView)this.DataContext)["SUBDIV_NAME"] = find.Subdiv_Name;
                        ((DataRowView)this.DataContext)["POS_NAME"] = find.Pos_Name;
                        _ocPhotoFR_Emp.Parameters["p_PERCO_SYNC_ID"].Value = ((DataRowView)this.DataContext)["PERCO_SYNC_ID"];
                        _ocPhotoFR_Emp.ExecuteNonQuery();
                        if (!(_ocPhotoFR_Emp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                            imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                                new MemoryStream((byte[])(_ocPhotoFR_Emp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);
                        else
                            imPhoto.Source = null;
                    }
                }
                else
                {
                    Other_Violator find = new Other_Violator();
                    _dialogResult = find.ShowDialog();
                    if (_dialogResult == true)
                    {
                        ((DataRowView)this.DataContext)["TRANSFER_ID"] = DBNull.Value;
                        ((DataRowView)this.DataContext)["PERCO_SYNC_ID"] = DBNull.Value;
                        ((DataRowView)this.DataContext)["OTHER_VIOLATOR_ID"] = find.Other_Violator_ID;
                        ((DataRowView)this.DataContext)["PER_NUM"] = DBNull.Value;
                        ((DataRowView)this.DataContext)["LAST_NAME"] = find.Last_Name;
                        ((DataRowView)this.DataContext)["FIRST_NAME"] = find.First_Name;
                        ((DataRowView)this.DataContext)["MIDDLE_NAME"] = find.Middle_Name;
                        ((DataRowView)this.DataContext)["CODE_SUBDIV"] = find.Subdiv_Name;
                        ((DataRowView)this.DataContext)["SUBDIV_NAME"] = find.Subdiv_Name;
                        ((DataRowView)this.DataContext)["POS_NAME"] = find.Pos_Name;
                        imPhoto.Source = null;
                    }
                }            
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) && this.DataContext != null &&
                ((DataRowView)this.DataContext).DataView.Table.GetChanges() != null)
                e.CanExecute = Array.TrueForAll<DependencyObject>(gridViolation.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false)
                    && Array.TrueForAll<DependencyObject>(gridViolation2.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false); ;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((cbReason_Detention.SelectedItem as DataRowView)["SIGN_THEFT"].ToString() != "1" &&
                _ds.Tables["STOLEN_PROPERTY"].Rows.Count > 0)
            {
                MessageBox.Show("Внимание!\n\nНайдено несоответствие в данных:\nПричина задержания не равна Попытке хищения, но данные по хищению заполнены!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
        }

        private void Add_Stolen_Property_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["STOLEN_PROPERTY"].Rows.Count == 0)
                e.CanExecute = true;
        }

        private void Add_Stolen_Property_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (((DataRowView)this.cbReason_Detention.SelectedItem)["SIGN_THEFT"].ToString() == "1")
            {
                if (((DataRowView)this.DataContext).DataView.Table.GetChanges() == null)
                {
                    chSCP.IsChecked = false;
                    chSR.IsChecked = false;
                    _ds.Tables["STOLEN_PROPERTY"].Rows.Add(rowStolen_Property.Row);
                    IsEnabledStolen_Property = true;
                }
                else
                {
                    MessageBox.Show("Нельзя добавить информацию по похищенному ТМЦ, так как есть несохраненные данные!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Нельзя добавить информацию, так как Причина задержания не Попытка хищения!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Delete_Stolen_Property_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["STOLEN_PROPERTY"].Rows.Count == 1 &&
                _ds.Tables["STOLEN_PROPERTY"].GetChanges() == null)
                e.CanExecute = true;
        }

        private void Delete_Stolen_Property_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rowStolen_Property["SIGN_RESTRICTED_ACCESS_INFORM"] = DBNull.Value;
            rowStolen_Property["TYPE_STOLEN_PROPERTY_ID"] = DBNull.Value;
            rowStolen_Property["UNIT_OF_MEASUREMENT"] = DBNull.Value;
            rowStolen_Property["COUNT_STOLEN"] = DBNull.Value;
            rowStolen_Property["SUM_STOLEN"] = DBNull.Value;
            rowStolen_Property["SIGN_CRIMINAL_PROSECUTION"] = DBNull.Value;
            rowStolen_Property["CRIMINAL_CASE_NUMBER"] = DBNull.Value;
            rowStolen_Property["DATE_OF_INITIATION"] = DBNull.Value;
            rowStolen_Property["DATE_OF_DECISION"] = DBNull.Value;
            _ds.Tables["STOLEN_PROPERTY"].DefaultView.Delete(0);
            IsEnabledStolen_Property = false;
        }

        private void Save_Stolen_Property_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["STOLEN_PROPERTY"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void Save_Stolen_Property_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["STOLEN_PROPERTY"].DefaultView.Count; ++i)
                {
                    if (_ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["STOLEN_PROPERTY_ID"] == DBNull.Value)
                    {
                        _ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["STOLEN_PROPERTY_ID"] =
                            new OracleCommand(string.Format("select {0}.STOLEN_PROPERTY_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                        _ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["VIOLATION_ID"] =
                            ((DataRowView)this.DataContext)["VIOLATION_ID"];
                    }
                    if (_ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["SIGN_CRIMINAL_PROSECUTION"].ToString() != "1")
                    {
                        _ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["CRIMINAL_CASE_NUMBER"] = DBNull.Value;
                        _ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["DATE_OF_INITIATION"] = DBNull.Value;
                        _ds.Tables["STOLEN_PROPERTY"].DefaultView[i]["DATE_OF_DECISION"] = DBNull.Value;
                    }
                }
                _daStolen_Property.InsertCommand.Transaction = transact;
                _daStolen_Property.UpdateCommand.Transaction = transact;
                _daStolen_Property.DeleteCommand.Transaction = transact;
                _daStolen_Property.Update(_ds.Tables["STOLEN_PROPERTY"]);
                transact.Commit();
                if (_ds.Tables["STOLEN_PROPERTY"].Rows.Count > 0)
                {
                    IsEnabledStolen_Property = true;
                }
                else
                {
                    IsEnabledStolen_Property = false;
                }
                Fl_reload = true;
                MessageBox.Show("Изменения сохранены в БД!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Add_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["PUNISHMENT"].Rows.Count == 0)
                e.CanExecute = true;
        }

        private void Add_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /* 07.07.2015 Убираем проверку на признак дисциплинарной комиссии, чтобы Группа приема могла вводить взыскания
            if (((DataRowView)this.DataContext)["SIGN_DISCIPLINARY_COMM"].ToString() == "1")
            {*/
                if (((DataRowView)this.DataContext).DataView.Table.GetChanges() == null)
                {
                    DataRowView newRow = rowPunishment; // _ds.Tables["PUNISHMENT"].DefaultView.AddNew();
                    _ds.Tables["PUNISHMENT"].Rows.Add(newRow.Row);
                    IsEnabledPunishment = true;
                    tiPunishment.DataContext = newRow;
                }
                else
                {
                    MessageBox.Show("Нельзя добавить информацию по взысканию, так как есть несохраненные данные!",
                        "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            /*}
            else
            {
                MessageBox.Show("Нельзя добавить информацию, так как не установлен признак Дисциплинарной комиссии!",
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private void Delete_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["PUNISHMENT"].Rows.Count == 1 &&
                _ds.Tables["PUNISHMENT"].GetChanges() == null)
                e.CanExecute = true;
        }

        private void Delete_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //rowPunishment["PUNISHMENT_NUM_ORDER"] = DBNull.Value;
            //rowPunishment["PUNISHMENT_DATE_ORDER"] = DBNull.Value;
            //rowPunishment["CHIEF_TRANSFER_ID"] = DBNull.Value;
            //rowPunishment["FIO_CHIEF"] = DBNull.Value;
            _ds.Tables["PUNISHMENT"].DefaultView.Delete(0);
            //_ds.Tables["PUNISHMENT"].Rows[0].Delete();
            tiPunishment.DataContext = null;
            dgList_Punishment.ItemsSource = null;
            dgList_Punishment_Chief.ItemsSource = null;
            IsEnabledPunishment = false;
        }

        private void Save_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["PUNISHMENT"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void Save_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["PUNISHMENT"].DefaultView.Count; ++i)
                {
                    if (_ds.Tables["PUNISHMENT"].DefaultView[i]["PUNISHMENT_ID"] == DBNull.Value)
                    {
                        _ds.Tables["PUNISHMENT"].DefaultView[i]["PUNISHMENT_ID"] =
                            new OracleCommand(string.Format("select {0}.PUNISHMENT_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                        _ds.Tables["PUNISHMENT"].DefaultView[i]["VIOLATION_ID"] =
                            ((DataRowView)this.DataContext)["VIOLATION_ID"];
                    }
                }
                _daPunishment.InsertCommand.Transaction = transact;
                _daPunishment.UpdateCommand.Transaction = transact;
                _daPunishment.DeleteCommand.Transaction = transact;
                _daPunishment.Update(_ds.Tables["PUNISHMENT"]);
                transact.Commit();
                Save_List_Punishment();
                Save_List_Punishment_Chief();
                if (_ds.Tables["PUNISHMENT"].Rows.Count > 0)
                {
                    IsEnabledPunishment = true;
                }
                else
                {
                    IsEnabledPunishment = false;
                }
                Fl_reload = true;
                tiPunishment.DataContext = rowPunishment;
                MessageBox.Show("Изменения сохранены в БД!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }
        
        private void Cancel_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_ds.Tables["PUNISHMENT"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void Cancel_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ds.Tables["PUNISHMENT"].RejectChanges();
            rowPunishment.Row.RejectChanges();
            if (_ds.Tables["PUNISHMENT"].Rows.Count > 0)
            {
                IsEnabledPunishment = true;
                dgList_Punishment.ItemsSource = _ds.Tables["LP"].DefaultView;
                dgList_Punishment_Chief.ItemsSource = _ds.Tables["LP_CHIEF"].DefaultView;
                _ds.Tables["LP_CHIEF"].RejectChanges();
            }
            else
            {
                IsEnabledPunishment = false;
            }
            Fl_reload = true;
            tiPunishment.DataContext = rowPunishment;
        }

        private void Add_List_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
                if (e.Parameter.ToString() == "0")
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        _ds.Tables["PUNISHMENT"].Rows.Count == 1 &&
                        _ds.Tables["PUNISHMENT"].GetChanges() == null)
                        e.CanExecute = true;
                }
                else
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        _ds.Tables["PUNISHMENT"].Rows.Count == 1 &&
                        _ds.Tables["PUNISHMENT"].GetChanges() == null &&
                        _ds.Tables["PUNISHMENT"].DefaultView[0]["CHIEF_TRANSFER_ID"] != DBNull.Value)
                        e.CanExecute = true;
                }
        }

        private void Add_List_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "0")
            {
                DataRowView newRow = _ds.Tables["LP"].DefaultView.AddNew();
                _ds.Tables["LP"].Rows.Add(newRow.Row);
                dgList_Punishment.SelectedItem = newRow;
            }
            else
            {
                DataRowView newRow = _ds.Tables["LP_CHIEF"].DefaultView.AddNew();
                _ds.Tables["LP_CHIEF"].Rows.Add(newRow.Row);
                dgList_Punishment_Chief.SelectedItem = newRow;
            }
        }

        private void Delete_List_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
                if (e.Parameter.ToString() == "0")
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        dgList_Punishment != null &&
                        dgList_Punishment.SelectedCells.Count > 0)
                        e.CanExecute = true;
                }
                else
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        dgList_Punishment_Chief != null &&
                        dgList_Punishment_Chief.SelectedCells.Count > 0)
                        e.CanExecute = true;
                }
        }

        private void Delete_List_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "0")
            {
                while (dgList_Punishment.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgList_Punishment.SelectedCells[0].Item).Delete();
                }
            }
            else
            {
                while (dgList_Punishment_Chief.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgList_Punishment_Chief.SelectedCells[0].Item).Delete();
                }
            }
        }

        private void Save_List_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
                if (e.Parameter.ToString() == "0")
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        _ds.Tables["LP"].GetChanges() != null)
                        e.CanExecute = true;
                }
                else
                {
                    if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                        _ds.Tables["LP_CHIEF"].GetChanges() != null)
                        e.CanExecute = true;
                }
        }

        private void Save_List_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "0")
            {
                Save_List_Punishment();
            }
            else
            {
                Save_List_Punishment_Chief();   
            }
        }

        void Save_List_Punishment()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["LP"].DefaultView.Count; ++i)
                {
                    if (_ds.Tables["LP"].DefaultView[i]["LIST_OF_PUNISHMENT_ID"] == DBNull.Value)
                    {
                        _ds.Tables["LP"].DefaultView[i]["LIST_OF_PUNISHMENT_ID"] =
                            new OracleCommand(string.Format("select {0}.LIST_OF_PUNISHMENT_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                        _ds.Tables["LP"].DefaultView[i]["PUNISHMENT_ID"] =
                            ((DataRowView)tiPunishment.DataContext)["PUNISHMENT_ID"];
                        _ds.Tables["LP"].DefaultView[i]["SIGN_PUNISHMENT_CHIEF"] = 0;
                    }
                }
                _daList_Pun.InsertCommand.Transaction = transact;
                _daList_Pun.UpdateCommand.Transaction = transact;
                _daList_Pun.DeleteCommand.Transaction = transact;
                _daList_Pun.Update(_ds.Tables["LP"]);
                transact.Commit();                
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();

            DataTable _dtType_Punishment_Temp = _ds.Tables["TP"].DefaultView.ToTable();
            _fl_Delete_Violation = false;
            // Берем TYPE_PUNISHMENT_ID сохраненных взысканий
            string _filterTP = String.Concat(_ds.Tables["LP"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("TYPE_PUNISHMENT_ID").ToString() + ",").ToArray()).TrimEnd(',');
            if (_filterTP != "")
            {
                // Устанавливаем фильтр по взысканиям и признаку удаления нарушения
                _dtType_Punishment_Temp.DefaultView.RowFilter = "TYPE_PUNISHMENT_ID in (" + _filterTP + ") and SIGN_DELETE_VIOLATION = 1";
                // Если признак удаления установлен хотя бы в одном взыскании, то ставим признак удаления всего нарушения
                if (_dtType_Punishment_Temp.DefaultView.Count > 0)
                {
                    _fl_Delete_Violation = true;
                    MessageBox.Show("Выбран такой тип взыскания, что нарушение будет автоматически удалено из базы!", "АСУ \"Кадры\"",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            //MessageBox.Show("Изменения сохранены в БД!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Cancel_List_Punishment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
                if (e.Parameter.ToString() == "0")
                {
                    if (_ds.Tables["LP"].GetChanges() != null)
                        e.CanExecute = true;
                }
                else
                {
                    if (_ds.Tables["LP_CHIEF"].GetChanges() != null)
                        e.CanExecute = true;
                }
        }

        private void Cancel_List_Punishment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "0")
            {
                _ds.Tables["LP"].RejectChanges();
            }
            else
            {
                _ds.Tables["LP_CHIEF"].RejectChanges();
            }
        } 

        void Save_List_Punishment_Chief()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                for (int i = 0; i < _ds.Tables["LP_CHIEF"].DefaultView.Count; ++i)
                {
                    if (_ds.Tables["LP_CHIEF"].DefaultView[i]["LIST_OF_PUNISHMENT_ID"] == DBNull.Value)
                    {
                        _ds.Tables["LP_CHIEF"].DefaultView[i]["LIST_OF_PUNISHMENT_ID"] =
                            new OracleCommand(string.Format("select {0}.LIST_OF_PUNISHMENT_ID_seq.NEXTVAL from dual",
                                Connect.Schema), Connect.CurConnect).ExecuteScalar();
                        _ds.Tables["LP_CHIEF"].DefaultView[i]["PUNISHMENT_ID"] =
                            ((DataRowView)tiPunishment.DataContext)["PUNISHMENT_ID"];
                        _ds.Tables["LP_CHIEF"].DefaultView[i]["SIGN_PUNISHMENT_CHIEF"] = 1;
                    }
                }
                _daList_Pun.InsertCommand.Transaction = transact;
                _daList_Pun.UpdateCommand.Transaction = transact;
                _daList_Pun.DeleteCommand.Transaction = transact;
                _daList_Pun.Update(_ds.Tables["LP_CHIEF"]);
                transact.Commit();
                //MessageBox.Show("Изменения сохранены в БД!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void dgList_Punishment_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ((DataGrid)sender).CommitEdit(DataGridEditingUnit.Row, true);
            ((DataGrid)sender).BeginEdit();
        }

        private void Select_Chief_Violator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Find_Violator find_chief = new Find_Violator(1);
            find_chief.Owner = this;
            if (find_chief.ShowDialog() == true)
            {
                ((DataRowView)tiPunishment.DataContext)["CHIEF_TRANSFER_ID"] = find_chief.Emp_ID;
                ((DataRowView)tiPunishment.DataContext)["FIO_CHIEF"] = string.Format("{0} - {1} {2} {3}",
                    find_chief.Code_Subdiv, find_chief.Last_Name, find_chief.First_Name, find_chief.Middle_Name);
            }
        }

        private void Select_Chief_Violator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                _ds.Tables["PUNISHMENT"].Rows.Count == 1)
                e.CanExecute = true;
        }

        private void Clear_Chief_Violator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rowPunishment["CHIEF_TRANSFER_ID"] = DBNull.Value;
            rowPunishment["FIO_CHIEF"] = DBNull.Value;
            while (_ds.Tables["LP_CHIEF"].DefaultView.Count > 0)
            {
                _ds.Tables["LP_CHIEF"].DefaultView[0].Delete();
            }
        }      
    }       

    public class IsEnabledEdit_MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(values[0]) || System.Convert.ToBoolean(values[1]))
                return true;
            else
                return false;
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsEnabledColor_ValueConvert : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value) == true)
                return new SolidColorBrush(Colors.White);
            else
                return new SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 252, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsChechedSCP_ValueConvert : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value) == true)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
