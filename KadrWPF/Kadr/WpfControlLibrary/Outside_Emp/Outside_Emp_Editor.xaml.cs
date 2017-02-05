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
using Kadr;
using Oracle.DataAccess.Client;
using LibraryKadr;
using Staff;
using System.Windows.Forms;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Outside_Emp_Editor.xaml
    /// </summary>
    public partial class Outside_Emp_Editor : Window
    {
        DataRowView _currentEmp;
        DataSet _ds;
        REGION_seq rregion; 
        DISTRICT_seq rdistrict;
        CITY_seq rcity; 
        LOCALITY_seq rlocality; 
        STREET_seq rstreet;
        REGISTR_seq registr;
        Address formregistr;
        bool f_LoadAddress = false;
        public Outside_Emp_Editor(bool flagAdd, DataRowView currentEmp, DataSet ds)
        {
            InitializeComponent();

            _currentEmp = currentEmp;
            _ds = ds;

            registr = new REGISTR_seq(Connect.CurConnect);

            if (flagAdd)
            {
                registr.AddNew();
                ((REGISTR_obj)(registr[0])).PER_NUM = _currentEmp["per_num"].ToString();
            }
            else
            {
                registr.Fill(string.Format("where {0} = '{1}'", REGISTR_seq.ColumnsName.PER_NUM, _currentEmp["per_num"]));
            }
            gbPerson_Data.DataContext = _currentEmp;
            gbPassport.DataContext = _ds.Tables["PASSPORT"].DefaultView[0];
            gbOther.DataContext = _ds.Tables["PER_DATA"].DefaultView[0];

            cbType_Per_Doc.ItemsSource = _ds.Tables["TYPE_PD"].DefaultView;

            TabItem_DragEnter();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SaveOutside_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name)/* &&
                _ds != null && _ds.HasChanges()*/)
                e.CanExecute = Array.TrueForAll<DependencyObject>(grPerson_Data.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
        }

        private void SaveOutside_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleCommand _ocCanExecute_Add = new OracleCommand(string.Format(Queries.GetQuery("SelectOutside_Emp_CanExecute_Add.sql"),
                Connect.Schema), Connect.CurConnect);
            _ocCanExecute_Add.BindByName = true;
            _ocCanExecute_Add.Parameters.Add("p_EMP_LAST_NAME", OracleDbType.Varchar2).Value = _currentEmp["EMP_LAST_NAME"];
            _ocCanExecute_Add.Parameters.Add("p_EMP_FIRST_NAME", OracleDbType.Varchar2).Value = _currentEmp["EMP_FIRST_NAME"];
            _ocCanExecute_Add.Parameters.Add("p_EMP_MIDDLE_NAME", OracleDbType.Varchar2).Value = _currentEmp["EMP_MIDDLE_NAME"];
            _ocCanExecute_Add.Parameters.Add("p_EMP_BIRTH_DATE", OracleDbType.Date).Value = _currentEmp["EMP_BIRTH_DATE"];
            _ocCanExecute_Add.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _currentEmp["PER_NUM"];
            if (Convert.ToInt16(_ocCanExecute_Add.ExecuteScalar()) != 0)
            {
                System.Windows.MessageBox.Show("Работник с введенными ФИО и датой рождения уже есть в базе данных!", 
                    "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _ds.Tables["REGISTR"].Rows[0]["REG_HOUSE"] = ((REGISTR_obj)registr[0]).REG_HOUSE;
            _ds.Tables["REGISTR"].Rows[0]["REG_BULK"] = ((REGISTR_obj)registr[0]).REG_BULK;
            _ds.Tables["REGISTR"].Rows[0]["REG_FLAT"] = ((REGISTR_obj)registr[0]).REG_FLAT;
            _ds.Tables["REGISTR"].Rows[0]["REG_POST_CODE"] = ((REGISTR_obj)registr[0]).REG_POST_CODE;
            ((REGISTR_obj)registr[0]).DATE_REG = formregistr.deDate_Reg.Date;
            if (((REGISTR_obj)registr[0]).DATE_REG != null)
                _ds.Tables["REGISTR"].Rows[0]["DATE_REG"] = ((REGISTR_obj)registr[0]).DATE_REG;
            else
                _ds.Tables["REGISTR"].Rows[0]["DATE_REG"] = DBNull.Value;
            _ds.Tables["REGISTR"].Rows[0]["REG_PHONE"] = ((REGISTR_obj)registr[0]).REG_PHONE;
            _ds.Tables["REGISTR"].Rows[0]["REG_CODE_STREET"] = ((REGISTR_obj)registr[0]).REG_CODE_STREET;
            this.DialogResult = true;
            this.Close();
        }

        private void cbType_Per_Doc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mbSeria_Passport.Mask = (cbType_Per_Doc.SelectedItem as DataRowView)["TEMPL_SER"].ToString();
            mbNum_Passport.Mask = (cbType_Per_Doc.SelectedItem as DataRowView)["TEMPL_NUM"].ToString();
        }

        private void TabItem_DragEnter()
        {
            // Таблицы для адреса места прописки и проживания   
            if (!f_LoadAddress)
            {
                // Инициализация таблиц и заполнение их данными
                rregion = new REGION_seq(Connect.CurConnect);
                rregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION));
                rdistrict = new DISTRICT_seq(Connect.CurConnect);
                rcity = new CITY_seq(Connect.CurConnect);
                rlocality = new LOCALITY_seq(Connect.CurConnect);
                rstreet = new STREET_seq(Connect.CurConnect);
                // Создание и настройка формы для работы с местом прописки
                REGISTR_obj r_registr = (REGISTR_obj)registr[0];
                formregistr = new Address((object)r_registr, typeof(REGISTR_seq), rregion, rdistrict, rcity, rlocality, rstreet);
                formregistr.TopLevel = false;
                formregistr.Dock = DockStyle.Fill;
                formregistr.cbRegion.SelectedItem = null;
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.EnabledComboBox);
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.cbRegion_SelectedIndexChanged);
                formregistr.tbHouse.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_HOUSE);
                formregistr.tbBulk.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_BULK);
                formregistr.tbFlat.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_FLAT);
                formregistr.tbPhone.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_PHONE);
                formregistr.tbPost_Code.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_POST_CODE);
                //formregistr.mbDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                formregistr.deDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                whRegistr.Child = formregistr;
                string stregistr = r_registr.REG_CODE_STREET;
                if (stregistr != null && stregistr != "")
                {
                    formregistr.LoadAddress(stregistr);
                }
                formregistr.Show();
                formregistr.DisableAll(true, System.Drawing.Color.White);
                //formregistr.EnableByRules();
                f_LoadAddress = true;
            }
        }
    }
}
