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
using Staff;
using LibraryKadr;
using System.Data;
using Kadr;
using System.Windows.Forms;
using System.Drawing;
using Oracle.DataAccess.Client;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Resume_Editor.xaml
    /// </summary>
    public partial class Resume_Editor : Window
    {
        static OracleDataAdapter _daEdu, _daPW;
        public static OracleCommand ocStanding;
        DataSet _ds;
        REGION_seq rregion, hregion;
        DISTRICT_seq rdistrict, hdistrict;
        CITY_seq rcity, hcity;
        LOCALITY_seq rlocality, hlocality;
        STREET_seq rstreet, hstreet;
        REGISTR_seq registr;
        HABIT_seq habit;
        bool f_LoadAddress = false;
        Address formregistr, formhabit;
        private static bool _fl_reload = false;
        public static bool Fl_reload
        {
            get { return _fl_reload; }
            set { _fl_reload = value; }
        }

        public Resume_Editor(bool flagAdd, DataRowView dataContext, DataSet ds)
        {
            InitializeComponent();
            this.DataContext = dataContext;
            _ds = ds;
            registr = new REGISTR_seq(Connect.CurConnect);
            habit = new HABIT_seq(Connect.CurConnect);
            if (flagAdd)
            {
                registr.AddNew();
                ((REGISTR_obj)(registr[0])).PER_NUM = dataContext["RESUME_PER_NUM"].ToString();
                habit.AddNew();
                ((HABIT_obj)(habit[0])).PER_NUM = dataContext["RESUME_PER_NUM"].ToString();
            }
            else
            {
                registr.Fill(string.Format("where PER_NUM = '{0}'", dataContext["RESUME_PER_NUM"]));
                habit.Fill(string.Format("where PER_NUM = '{0}'", dataContext["RESUME_PER_NUM"]));
            }

            gbHabit.DataContext = _ds.Tables["HABIT"].DefaultView;
            cbSource_Employability.ItemsSource = _ds.Tables["S_E"].DefaultView;
            TabItem_DragEnter();

            dgEdu.DataContext = null;
            _ds.Tables["EDU"].Rows.Clear();
            _daEdu.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["RESUME_PER_NUM"];
            _daEdu.Fill(_ds.Tables["EDU"]);

            dcSPEC_ID.ItemsSource = _ds.Tables["SPECIALITY"].DefaultView;
            dcINSTIT_ID.ItemsSource = _ds.Tables["INSTIT"].DefaultView;
            dcTYPE_STUDY_ID.ItemsSource = _ds.Tables["TYPE_STUDY"].DefaultView;
            dcTYPE_EDU_ID.ItemsSource = _ds.Tables["TYPE_EDU"].DefaultView;
            dcQUAL_ID.ItemsSource = _ds.Tables["QUAL"].DefaultView;
            dcGR_SPEC_ID.ItemsSource = _ds.Tables["GROUP_SPEC"].DefaultView;
            dgEdu.DataContext = _ds.Tables["EDU"].DefaultView;

            dgPrev_Work.DataContext = null;
            _ds.Tables["PW"].Rows.Clear();
            _daPW.SelectCommand.Parameters["p_PER_NUM"].Value = dataContext["RESUME_PER_NUM"];
            _daPW.Fill(_ds.Tables["PW"]);
            dgPrev_Work.DataContext = _ds.Tables["PW"].DefaultView;

            for (int i = 0; i < _ds.Tables["PW"].Rows.Count; i++)
            {
                ocStanding.Parameters["p_date_begin"].Value = _ds.Tables["PW"].Rows[i]["PW_DATE_START"];
                ocStanding.Parameters["p_date_end"].Value = _ds.Tables["PW"].Rows[i]["PW_DATE_END"];
                ocStanding.ExecuteNonQuery();    
                _ds.Tables["PW"].Rows[i]["STAGYEAR"] = ocStanding.Parameters["p_years"].Value;
                _ds.Tables["PW"].Rows[i]["STAGMONTH"] = ocStanding.Parameters["p_months"].Value;
                _ds.Tables["PW"].Rows[i]["STAGDAY"] = ocStanding.Parameters["p_days"].Value;
            }
        }

        static Resume_Editor()
        {
            // Select
            _daEdu = new OracleDataAdapter(string.Format(Queries.GetQuery("Resume/SelectEdu.sql"),
                Connect.Schema), Connect.CurConnect);
            _daEdu.SelectCommand.BindByName = true;
            _daEdu.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daEdu.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EDU_insert(:EDU_ID,:SPEC_ID,:PER_NUM,:INSTIT_ID,:TYPE_STUDY_ID,:TYPE_EDU_ID,:MAIN_PROF,:SERIA_DIPLOMA,:NUM_DIPLOMA,:QUAL_ID,
                        :SPECIALIZATION,:YEAR_GRADUATING,:GR_SPEC_ID,:FROM_FACT);
                END;", Connect.Schema), Connect.CurConnect);
            _daEdu.InsertCommand.BindByName = true;
            _daEdu.InsertCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");
            _daEdu.InsertCommand.Parameters.Add("SPEC_ID", OracleDbType.Decimal, 0, "SPEC_ID");
            _daEdu.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daEdu.InsertCommand.Parameters.Add("INSTIT_ID", OracleDbType.Decimal, 0, "INSTIT_ID");
            _daEdu.InsertCommand.Parameters.Add("TYPE_STUDY_ID", OracleDbType.Decimal, 0, "TYPE_STUDY_ID");
            _daEdu.InsertCommand.Parameters.Add("TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID");
            _daEdu.InsertCommand.Parameters.Add("MAIN_PROF", OracleDbType.Int16, 0, "MAIN_PROF");
            _daEdu.InsertCommand.Parameters.Add("SERIA_DIPLOMA", OracleDbType.Varchar2, 0, "SERIA_DIPLOMA");
            _daEdu.InsertCommand.Parameters.Add("NUM_DIPLOMA", OracleDbType.Varchar2, 0, "NUM_DIPLOMA");
            _daEdu.InsertCommand.Parameters.Add("QUAL_ID", OracleDbType.Decimal, 0, "QUAL_ID");
            _daEdu.InsertCommand.Parameters.Add("SPECIALIZATION", OracleDbType.Varchar2, 0, "SPECIALIZATION");
            _daEdu.InsertCommand.Parameters.Add("YEAR_GRADUATING", OracleDbType.Date, 0, "YEAR_GRADUATING");
            _daEdu.InsertCommand.Parameters.Add("GR_SPEC_ID", OracleDbType.Decimal, 0, "GR_SPEC_ID");
            _daEdu.InsertCommand.Parameters.Add("FROM_FACT", OracleDbType.Int16, 0, "FROM_FACT");
            // Update
            _daEdu.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.EDU_update(:EDU_ID,:SPEC_ID,:PER_NUM,:INSTIT_ID,:TYPE_STUDY_ID,:TYPE_EDU_ID,:MAIN_PROF,:SERIA_DIPLOMA,:NUM_DIPLOMA,:QUAL_ID,
                        :SPECIALIZATION,:YEAR_GRADUATING,:GR_SPEC_ID,:FROM_FACT);
                END;", Connect.Schema), Connect.CurConnect);
            _daEdu.UpdateCommand.BindByName = true;
            _daEdu.UpdateCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");
            _daEdu.UpdateCommand.Parameters.Add("SPEC_ID", OracleDbType.Decimal, 0, "SPEC_ID");
            _daEdu.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daEdu.UpdateCommand.Parameters.Add("INSTIT_ID", OracleDbType.Decimal, 0, "INSTIT_ID");
            _daEdu.UpdateCommand.Parameters.Add("TYPE_STUDY_ID", OracleDbType.Decimal, 0, "TYPE_STUDY_ID");
            _daEdu.UpdateCommand.Parameters.Add("TYPE_EDU_ID", OracleDbType.Decimal, 0, "TYPE_EDU_ID");
            _daEdu.UpdateCommand.Parameters.Add("MAIN_PROF", OracleDbType.Int16, 0, "MAIN_PROF");
            _daEdu.UpdateCommand.Parameters.Add("SERIA_DIPLOMA", OracleDbType.Varchar2, 0, "SERIA_DIPLOMA");
            _daEdu.UpdateCommand.Parameters.Add("NUM_DIPLOMA", OracleDbType.Varchar2, 0, "NUM_DIPLOMA");
            _daEdu.UpdateCommand.Parameters.Add("QUAL_ID", OracleDbType.Decimal, 0, "QUAL_ID");
            _daEdu.UpdateCommand.Parameters.Add("SPECIALIZATION", OracleDbType.Varchar2, 0, "SPECIALIZATION");
            _daEdu.UpdateCommand.Parameters.Add("YEAR_GRADUATING", OracleDbType.Date, 0, "YEAR_GRADUATING");
            _daEdu.UpdateCommand.Parameters.Add("GR_SPEC_ID", OracleDbType.Decimal, 0, "GR_SPEC_ID");
            _daEdu.UpdateCommand.Parameters.Add("FROM_FACT", OracleDbType.Int16, 0, "FROM_FACT");
            // Delete
            _daEdu.DeleteCommand = new OracleCommand(string.Format(
                "BEGIN {0}.EDU_delete(:EDU_ID); END;", Connect.Schema), Connect.CurConnect);
            _daEdu.DeleteCommand.BindByName = true;
            _daEdu.DeleteCommand.Parameters.Add("EDU_ID", OracleDbType.Decimal, 0, "EDU_ID");

            // Select
            _daPW = new OracleDataAdapter(string.Format(Queries.GetQuery("Resume/SelectPrev_Work.sql"),
                Connect.Schema), Connect.CurConnect);
            _daPW.SelectCommand.BindByName = true;
            _daPW.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            // Insert
            _daPW.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PREV_WORK_insert(:PREV_WORK_ID,:PER_NUM,:PW_FIRM,:PW_NAME_POS,:PW_DATE_START,:PW_DATE_END,:WORK_IN_FACT,:MEDICAL_SIGN);
                END;", Connect.Schema), Connect.CurConnect);
            _daPW.InsertCommand.BindByName = true;
            _daPW.InsertCommand.Parameters.Add("PREV_WORK_ID", OracleDbType.Decimal, 0, "PREV_WORK_ID");
            _daPW.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPW.InsertCommand.Parameters.Add("PW_FIRM", OracleDbType.Varchar2, 0, "PW_FIRM");
            _daPW.InsertCommand.Parameters.Add("PW_NAME_POS", OracleDbType.Varchar2, 0, "PW_NAME_POS");
            _daPW.InsertCommand.Parameters.Add("PW_DATE_START", OracleDbType.Date, 0, "PW_DATE_START");
            _daPW.InsertCommand.Parameters.Add("PW_DATE_END", OracleDbType.Date, 0, "PW_DATE_END");
            _daPW.InsertCommand.Parameters.Add("WORK_IN_FACT", OracleDbType.Int16, 0, "WORK_IN_FACT");
            _daPW.InsertCommand.Parameters.Add("MEDICAL_SIGN", OracleDbType.Int16, 0, "MEDICAL_SIGN");
            // Update
            _daPW.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN 
                    {0}.PREV_WORK_update(:PREV_WORK_ID,:PER_NUM,:PW_FIRM,:PW_NAME_POS,:PW_DATE_START,:PW_DATE_END,:WORK_IN_FACT,:MEDICAL_SIGN);
                END;", Connect.Schema), Connect.CurConnect);
            _daPW.UpdateCommand.BindByName = true;
            _daPW.UpdateCommand.Parameters.Add("PREV_WORK_ID", OracleDbType.Decimal, 0, "PREV_WORK_ID");
            _daPW.UpdateCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
            _daPW.UpdateCommand.Parameters.Add("PW_FIRM", OracleDbType.Varchar2, 0, "PW_FIRM");
            _daPW.UpdateCommand.Parameters.Add("PW_NAME_POS", OracleDbType.Varchar2, 0, "PW_NAME_POS");
            _daPW.UpdateCommand.Parameters.Add("PW_DATE_START", OracleDbType.Date, 0, "PW_DATE_START");
            _daPW.UpdateCommand.Parameters.Add("PW_DATE_END", OracleDbType.Date, 0, "PW_DATE_END");
            _daPW.UpdateCommand.Parameters.Add("WORK_IN_FACT", OracleDbType.Int16, 0, "WORK_IN_FACT");
            _daPW.UpdateCommand.Parameters.Add("MEDICAL_SIGN", OracleDbType.Int16, 0, "MEDICAL_SIGN");
            // Delete
            _daPW.DeleteCommand = new OracleCommand(string.Format(
                "BEGIN {0}.PREV_WORK_delete(:PREV_WORK_ID); END;", Connect.Schema), Connect.CurConnect);
            _daPW.DeleteCommand.BindByName = true;
            _daPW.DeleteCommand.Parameters.Add("PREV_WORK_ID", OracleDbType.Decimal, 0, "PREV_WORK_ID");

            ocStanding = new OracleCommand("", Connect.CurConnect);
            ocStanding.CommandText = string.Format(
                @"begin
                    {0}.CALC_STANDING_ROW(:p_date_begin, :p_date_end, :p_years, :p_months, :p_days);
                end;", Connect.Schema);
            ocStanding.BindByName = true;
            ocStanding.Parameters.Add("p_date_begin", OracleDbType.Date);
            ocStanding.Parameters.Add("p_date_end", OracleDbType.Date);
            ocStanding.Parameters.Add("p_years", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_years"].DbType = DbType.Decimal;
            ocStanding.Parameters.Add("p_months", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_months"].DbType = DbType.Decimal;
            ocStanding.Parameters.Add("p_days", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_days"].DbType = DbType.Decimal;
        }

        private void SaveResume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) /*&&
                _ds != null && _ds.HasChanges()*/)
                e.CanExecute = Array.TrueForAll<DependencyObject>(grPerson_Data.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
        }

        private void SaveResume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveEmp();
            this.DialogResult = true;
            this.Close();
        }

        void SaveEmp()
        {
            _ds.Tables["REGISTR"].Rows[0]["REG_HOUSE"] = ((REGISTR_obj)registr[0]).REG_HOUSE;
            _ds.Tables["REGISTR"].Rows[0]["REG_BULK"] = ((REGISTR_obj)registr[0]).REG_BULK;
            _ds.Tables["REGISTR"].Rows[0]["REG_FLAT"] = ((REGISTR_obj)registr[0]).REG_FLAT;
            _ds.Tables["REGISTR"].Rows[0]["REG_POST_CODE"] = ((REGISTR_obj)registr[0]).REG_POST_CODE;
            if (((REGISTR_obj)registr[0]).DATE_REG == null)
                _ds.Tables["REGISTR"].Rows[0]["DATE_REG"] = DBNull.Value;
            else
                _ds.Tables["REGISTR"].Rows[0]["DATE_REG"] = ((REGISTR_obj)registr[0]).DATE_REG;
            _ds.Tables["REGISTR"].Rows[0]["REG_PHONE"] = ((REGISTR_obj)registr[0]).REG_PHONE;
            _ds.Tables["REGISTR"].Rows[0]["REG_CODE_STREET"] = ((REGISTR_obj)registr[0]).REG_CODE_STREET;

            _ds.Tables["HABIT"].Rows[0]["HAB_HOUSE"] = ((HABIT_obj)habit[0]).HAB_HOUSE;
            _ds.Tables["HABIT"].Rows[0]["HAB_BULK"] = ((HABIT_obj)habit[0]).HAB_BULK;
            _ds.Tables["HABIT"].Rows[0]["HAB_FLAT"] = ((HABIT_obj)habit[0]).HAB_FLAT;
            _ds.Tables["HABIT"].Rows[0]["HAB_POST_CODE"] = ((HABIT_obj)habit[0]).HAB_POST_CODE;
            _ds.Tables["HABIT"].Rows[0]["HAB_PHONE"] = ((HABIT_obj)habit[0]).HAB_PHONE;
            _ds.Tables["HABIT"].Rows[0]["HAB_CODE_STREET"] = ((HABIT_obj)habit[0]).HAB_CODE_STREET;

            ((DataRowView)this.DataContext)["SOURCE_EMPLOYABILITY_NAME"] = cbSource_Employability.Text;

            Resume_Viewer.SaveResume();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
                System.Windows.Forms.Button bt = new System.Windows.Forms.Button();
                bt.Name = "btFromRegistrToHabit";
                bt.Location = new System.Drawing.Point(24, 283);
                bt.Size = new System.Drawing.Size(335, 27);
                bt.Font = new Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold);
                bt.Text = "Скопировать в адрес проживания";
                bt.ForeColor = System.Drawing.Color.FromArgb(0, 70, 213);
                bt.Enabled = false;
                bt.Click += new EventHandler(bt_Click);
                formregistr.Controls.Add(bt);
                string stregistr = r_registr.REG_CODE_STREET;
                if (stregistr != null && stregistr != "")
                {
                    formregistr.LoadAddress(stregistr);
                }
                formregistr.Show();
                formregistr.DisableAll(true, System.Drawing.Color.White);

                // Инициализация таблиц и заполнение их данными
                hregion = new REGION_seq(Connect.CurConnect);
                hregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION));
                hdistrict = new DISTRICT_seq(Connect.CurConnect);
                hcity = new CITY_seq(Connect.CurConnect);
                hlocality = new LOCALITY_seq(Connect.CurConnect);
                hstreet = new STREET_seq(Connect.CurConnect);
                // Создание и настройка формы для работы с местом прописки
                HABIT_obj r_habit = (HABIT_obj)habit[0];
                formhabit = new Address((object)r_habit, typeof(HABIT_seq), hregion, hdistrict, hcity, hlocality, hstreet);
                formhabit.TopLevel = false;
                formhabit.Dock = DockStyle.Fill;
                formhabit.cbRegion.SelectedItem = null;
                formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.EnabledComboBox);
                formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.cbRegion_SelectedIndexChanged);
                formhabit.tbHouse.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_HOUSE);
                formhabit.tbBulk.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_BULK);
                formhabit.tbFlat.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_FLAT);
                formhabit.tbPhone.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_PHONE);
                formhabit.tbPost_Code.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_POST_CODE);
                formhabit.deDate_Reg.Visible = false;
                formhabit.tbPhone.Location = formhabit.mbDate_Reg.Location;
                formhabit.lbDate.Visible = false;
                formhabit.lbPhone.Location = new System.Drawing.Point(24, 240);

                whHabit.Child = formhabit;
                string sthabit = r_habit.HAB_CODE_STREET;
                if (sthabit != null && sthabit != "")
                {
                    formhabit.LoadAddress(sthabit);
                }
                formhabit.Show();
                formhabit.DisableAll(true, System.Drawing.Color.White);

                f_LoadAddress = true;
            }
        }


        void bt_Click(object sender, EventArgs e)
        {
            if (formregistr.cbStreet.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Вы не ввели адрес прописки для копирования", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                if (System.Windows.MessageBox.Show("Вы действительно хотите скопировать адрес прописки\nв адрес проживания?\nЭто займет некоторое время.", "АСУ \"Кадры\"", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string st = registr[0].REG_CODE_STREET;
                    habit[0].HAB_CODE_STREET = st;
                    formhabit.LoadAddress(st);
                    formhabit.tbHouse.Text = registr[0].REG_HOUSE;
                    formhabit.tbBulk.Text = registr[0].REG_BULK;
                    formhabit.tbFlat.Text = registr[0].REG_FLAT;
                    formhabit.tbPost_Code.Text = registr[0].REG_POST_CODE;
                    formhabit.tbPhone.Text = registr[0].REG_PHONE;
                    formhabit.tbHouse.Enabled = true;
                    formhabit.tbBulk.Enabled = true;
                    formhabit.tbFlat.Enabled = true;
                    formhabit.tbPost_Code.Enabled = true;
                    formhabit.tbPhone.Enabled = true;
                }
            }
        }

        private void AddEdu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentEdu = _ds.Tables["EDU"].DefaultView.AddNew();
            _currentEdu["MAIN_PROF"] = 0;
            _currentEdu["FROM_FACT"] = 0;
            _ds.Tables["EDU"].Rows.Add(_currentEdu.Row);
            dgEdu.SelectedItem = _currentEdu;

            Edu_Editor edu = new Edu_Editor(_currentEdu, _ds);
            edu.Owner = Window.GetWindow(this);
            if (edu.ShowDialog() == true)
            {
                SaveEmp();
                SaveEdu();
            }
            else
            {
                _ds.Tables["EDU"].RejectChanges();
            }
        }

        private void EditEdu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgEdu != null && dgEdu.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgEdu.SelectedCells[0].Item);
            rowSelected.Row.RejectChanges();
            
            Edu_Editor edu = new Edu_Editor(rowSelected, _ds);
            edu.Owner = Window.GetWindow(this);
            if (edu.ShowDialog() == true)
            {
                SaveEdu();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            } 
        }

        private void DeleteEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgEdu.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgEdu.SelectedCells[0].Item).Delete();
                }
                SaveEdu();
            }
            dgEdu.Focus();
        }

        void SaveEdu()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["EDU"].DefaultView.RowStateFilter;
                _ds.Tables["EDU"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["EDU"].DefaultView.Count; ++i)
                {
                    _ds.Tables["EDU"].DefaultView[i]["EDU_ID"] =
                        new OracleCommand(string.Format("select {0}.EDU_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                    _ds.Tables["EDU"].DefaultView[i]["PER_NUM"] =
                        ((DataRowView)this.DataContext)["RESUME_PER_NUM"];
                }
                _ds.Tables["EDU"].DefaultView.RowStateFilter = rs;
                _daEdu.InsertCommand.Transaction = transact;
                _daEdu.UpdateCommand.Transaction = transact;
                _daEdu.DeleteCommand.Transaction = transact;
                _daEdu.Update(_ds.Tables["EDU"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void AddPrev_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentPW = _ds.Tables["PW"].DefaultView.AddNew();
            _currentPW["WORK_IN_FACT"] = 0;
            _currentPW["MEDICAL_SIGN"] = 0;
            _ds.Tables["PW"].Rows.Add(_currentPW.Row);
            dgEdu.SelectedItem = _currentPW;

            Prev_Work_Editor pw = new Prev_Work_Editor(_currentPW, _ds);
            pw.Owner = Window.GetWindow(this);
            if (pw.ShowDialog() == true)
            {
                SaveEmp();
                SavePrev_Work();
            }
            else
            {
                _ds.Tables["PW"].RejectChanges();
            }
        }

        private void EditPrev_Work_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgPrev_Work != null && dgPrev_Work.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void EditPrev_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView rowSelected = ((DataRowView)dgPrev_Work.SelectedCells[0].Item);
            //rowSelected.Row.RejectChanges();

            Prev_Work_Editor pw = new Prev_Work_Editor(rowSelected, _ds);
            pw.Owner = Window.GetWindow(this);
            if (pw.ShowDialog() == true)
            {
                SavePrev_Work();
            }
            else
            {
                rowSelected.Row.RejectChanges();
            } 
        }

        private void DeletePrev_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgPrev_Work.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgPrev_Work.SelectedCells[0].Item).Delete();
                }
                SavePrev_Work();
            }
            dgPrev_Work.Focus();
        }

        void SavePrev_Work()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                DataViewRowState rs = _ds.Tables["PW"].DefaultView.RowStateFilter;
                _ds.Tables["PW"].DefaultView.RowStateFilter = DataViewRowState.Added;
                for (int i = 0; i < _ds.Tables["PW"].DefaultView.Count; ++i)
                {
                    _ds.Tables["PW"].DefaultView[i]["PREV_WORK_ID"] =
                        new OracleCommand(string.Format("select {0}.PREV_WORK_ID_seq.NEXTVAL from dual",
                            Connect.Schema), Connect.CurConnect).ExecuteScalar();
                    _ds.Tables["PW"].DefaultView[i]["PER_NUM"] =
                        ((DataRowView)this.DataContext)["RESUME_PER_NUM"];
                }
                _ds.Tables["PW"].DefaultView.RowStateFilter = rs;
                _daPW.InsertCommand.Transaction = transact;
                _daPW.UpdateCommand.Transaction = transact;
                _daPW.DeleteCommand.Transaction = transact;
                _daPW.Update(_ds.Tables["PW"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                System.Windows.MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CommandManager.InvalidateRequerySuggested();
        }
    }
}