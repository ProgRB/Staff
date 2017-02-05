using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class red : Form
    {
        ACCOUNT_DATA_seq account;
        public string _date_start_priv,_date_end_priv ;
        public string per_num;
        bool signInvalid;
        REL_TYPE_seq rel_typeChild;
        RELATIVE_seq relativeChild;
        CHILD_REARING_LEAVE_seq child_rearing_leave;
        TARIFF_GRID_seq tariff_grid;
        Emp_Priv formemp_priv;
        BASE_DOC_seq base_doc;
        TYPE_PRIV_seq type_priv;
        EMP_PRIV_seq emp_priv;
        PER_DATA_seq per_data;
        PASSPORT_seq passport;
        REGISTR_seq registr;
        HABIT_seq habit;
        MAR_STATE_seq mar_state;
        SOURCE_COMPLECT_seq source_complect;
        TYPE_PER_DOC_seq type_per_doc;
        TRANSFER_seq transfer;
        bool f_LoadAddress, f_LoadEmp_priv, f_LoadDep, f_LoadRelativeChild, f_LoadOld_FIO, f_LoadPrev_Work;
        Address formregistr, formhabit;
        REGION_seq rregion, hregion;
        DISTRICT_seq rdistrict, hdistrict;
        CITY_seq rcity, hcity;
        LOCALITY_seq rlocality, hlocality;
        STREET_seq rstreet, hstreet;
        REGISTR_obj r_registr;
        HABIT_obj r_habit;
        SOURCE_FILL_seq source_fill;
        PREV_WORK_seq prev_work;
        grid_ras grid_ras;
        VisiblePanel pnVisible = new VisiblePanel();
        OracleDataAdapter odaDep;
        OracleDataTable dtDep;
        decimal sign_comb;
        decimal _transfer_id;
        OracleCommand ocStanding, ocStart_Work;
        OracleDataAdapter _daProfUnion;
        DataTable _dtProfUnion;
        /// <summary>
        /// Конструктор формы редактирования бухгалтерских данных
        /// </summary>
        /// <param name="_grid_ras"></param>
        /// <param name="_transfer_id"></param>
        public red(grid_ras _grid_ras, decimal transfer_id, decimal _sign_comb)
        {
            InitializeComponent();
            pnSaveAccountData.EnableByRules();
            pnEmp_Priv_Button.EnableByRules();
            pnEditDependents.EnableByRules();
            grid_ras = _grid_ras;
            sign_comb = _sign_comb;
            _transfer_id = transfer_id;
            tbEmp_last_name.Text = grid_ras.dgEmp.CurrentRow.Cells["EMP_LAST_NAME"].Value.ToString();
            tbEmp_first_name.Text = grid_ras.dgEmp.CurrentRow.Cells["EMP_FIRST_NAME"].Value.ToString();
            tbEmp_middle_name.Text = grid_ras.dgEmp.CurrentRow.Cells["EMP_MIDDLE_NAME"].Value.ToString();
            tbPer_num.Text = grid_ras.dgEmp.CurrentRow.Cells["PER_NUM"].Value.ToString();
            tbEmp_Sex.Text = grid_ras.dgEmp.CurrentRow.Cells["EMP_SEX"].Value.ToString();
            tbEmp_Birth_Date.Text = 
                Convert.ToDateTime(grid_ras.dgEmp.CurrentRow.Cells["EMP_BIRTH_DATE"].Value).ToShortDateString();
            tbCode_Subdiv.Text = grid_ras.dgEmp.CurrentRow.Cells["CODE_SUBDIV"].Value.ToString();
            tbCode_Pos.Text = grid_ras.dgEmp.CurrentRow.Cells["CODE_POS"].Value.ToString();
            tbPos_Name.Text = grid_ras.dgEmp.CurrentRow.Cells["pos_name"].Value.ToString();
            tbCode_Degree.Text = grid_ras.dgEmp.CurrentRow.Cells["CODE_DEGREE"].Value.ToString();

            per_num = tbPer_num.Text;

            pictureBox1.Image = EmployeePhoto.GetPhoto(tbPer_num.Text);       

            account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            account.Fill(string.Format("where TRANSFER_ID = {0}",
                _transfer_id));
            tariff_grid = new TARIFF_GRID_seq(Connect.CurConnect);
            tariff_grid.Fill(string.Format("where TARIFF_GRID_ID = {0}", 
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).TARIFF_GRID_ID != null ?
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).TARIFF_GRID_ID : -1));

            // Шифр налога
            tbTax_code.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.TAX_CODE);
            //
            tbSalary.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SALARY);
            tbSalaryMission.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SALARY_MISSION);
            // тарифная сетка
            tbCode_tariff_grid.AddBindingSource(tariff_grid, TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID);
            //
            deDate_Add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.DATE_ADD);            
            deDate_Servant.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.DATE_SERVANT);
            tbClassific.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.CLASSIFIC);
            tbPercent13.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.PERCENT13);
            tB_count_dep14.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP14);
            tB_count_dep15.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP15);
            tB_count_dep16.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP16);
            tB_count_dep17.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP17);
            tB_count_dep18.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP18);
            tB_count_dep19.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP19);
            tB_count_dep20.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP20);
            tB_count_dep21.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COUNT_DEP21);
            tB_comb_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COMB_ADDITION);
            tB_secret_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SECRET_ADDITION);
            tB_prof_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.PROF_ADDITION);
            tB_class_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.CLASS_ADDITION);
            tB_chief_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.CHIEF_ADDITION);
            tbTrip_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.TRIP_ADDITION);
            tB_harmful_addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION);
            tB_harmful_addition_add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION_ADD);
            tbGovSecret_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.GOVSECRET_ADDITION);
            tbEncoding_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.ENCODING_ADDITION);
            // Надбавка за стаж
            tB_sign_add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SIGN_ADD);
            //
            // Надбавка за выслугу лет
            tbService_Add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SERVICE_ADD);
            //
            tbSum_young_spec.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SUM_YOUNG_SPEC);
            deD_E_Y_S.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.DATE_END_YOUNG_SPEC);

            /// Ставим проверку на ввод разделителя+
            tB_comb_addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbPercent13.KeyPress += new KeyPressEventHandler(Library.InputSeparator);            
            tB_prof_addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tB_class_addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tB_chief_addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbTrip_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);            
            tbSum_young_spec.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbSalaryMission.KeyPress += new KeyPressEventHandler(Library.InputSeparator);

            // ВЫТАСКИВАЮ ДАННЫЕ ПО ПРОФСОЮЗУ
            /* 13.02.2015 - Убираем выборку признака профсоюза из таблицы PER_DATA
            OracleDataTable prof = new OracleDataTable("", Connect.CurConnect);
            prof.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/SignProfUnion.sql"),
                DataSourceScheme.SchemeName);
            prof.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            prof.Fill();
            if (prof.Rows.Count > 0)
            {
                signInvalid = Convert.ToBoolean(prof.Rows[0]["SIGN_PROFUNION"]);
                checkB_SIGN_PROFUNION.Checked = signInvalid;
            }*/

            per_data = new PER_DATA_seq(Connect.CurConnect);
            passport = new PASSPORT_seq(Connect.CurConnect);
            registr = new REGISTR_seq(Connect.CurConnect);
            habit = new HABIT_seq(Connect.CurConnect);
            per_data.Fill(string.Format("where {0} = '{1}'", PER_DATA_seq.ColumnsName.PER_NUM, per_num));
            passport.Fill(string.Format("where {0} = '{1}'", PASSPORT_seq.ColumnsName.PER_NUM, per_num));
            registr.Fill(string.Format("where {0} = '{1}'", REGISTR_seq.ColumnsName.PER_NUM, per_num));
            habit.Fill(string.Format("where {0} = '{1}'", HABIT_seq.ColumnsName.PER_NUM, per_num));
            mar_state = new MAR_STATE_seq(Connect.CurConnect);
            mar_state.Fill(string.Format("order by {0}", MAR_STATE_seq.ColumnsName.NAME_STATE));
            source_complect = new SOURCE_COMPLECT_seq(Connect.CurConnect);
            source_complect.Fill(string.Format("order by {0}", SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME));
            type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
            type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID, _transfer_id));

            OracleCommand cmd_aud = new OracleCommand(string.Format(
                    "begin APSTAFF.TABLE_AUDIT_EX_INSERT(:p_table_id, :p_type_kard); end;", Connect.Schema), Connect.CurConnect);
            cmd_aud.BindByName = true;
            cmd_aud.Parameters.Add("p_table_id", OracleDbType.Decimal, _transfer_id, ParameterDirection.Input);
            cmd_aud.Parameters.Add("p_type_kard", OracleDbType.Varchar2, "AD", ParameterDirection.Input);
            cmd_aud.ExecuteNonQuery();

            cbType_Per_Doc_ID.AddBindingSource(passport, TYPE_PER_DOC_seq.ColumnsName.TYPE_PER_DOC_ID,
                new LinkArgument(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            mbInsurance_Num.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INSURANCE_NUM);
            mbInn.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INN);
            mbSeria_Passport.AddBindingSource(passport, PASSPORT_seq.ColumnsName.SERIA_PASSPORT);
            mbNum_Passport.AddBindingSource(passport, PASSPORT_seq.ColumnsName.NUM_PASSPORT);
            tbWho_Given.AddBindingSource(passport, PASSPORT_seq.ColumnsName.WHO_GIVEN);
            tbCitizenship.AddBindingSource(passport, PASSPORT_seq.ColumnsName.CITIZENSHIP);
            tbCountry_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.COUNTRY_BIRTH);
            tbRegion_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.REGION_BIRTH);
            tbCity_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.CITY_BIRTH);
            tbDistr_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.DISTR_BIRTH);
            tbLocality_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.LOCALITY_BIRTH);
            deWhen_Given.AddBindingSource(passport, PASSPORT_seq.ColumnsName.WHEN_GIVEN);

            // Привязка чекбоксов к данным
            chTrip_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.TRIP_SIGN);
            chRetiree_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.RETIRER_SIGN);
            cbMar_State_ID.AddBindingSource(passport, MAR_STATE_seq.ColumnsName.MAR_STATE_ID,
                new LinkArgument(mar_state, MAR_STATE_seq.ColumnsName.NAME_STATE));
            cbSource_ID.AddBindingSource(transfer, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_ID,
                new LinkArgument(source_complect, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME));

            mbDate_HirePlant.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_HIRE);
            if (transfer.Count != 0 &&
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 3)
                mbDate_Dismiss.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);

            /// Задаем цвет вкладкам
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                tabPage.BackColor = Color.FromArgb(236, 233, 216);
            }
            // Так как дата надбавки за стаж не активна, задаем ей цвет. По умолчанию он ставится серый
            deDate_Add.BackColor = Color.WhiteSmoke;

            ///// Выбираем является ли сейчас инвалидом
            //OracleCommand ocSign_Invalid = new OracleCommand("", Connect.CurConnect);
            //ocSign_Invalid.CommandText = string.Format(
            //    "select count(1) from {0}.EMP_PRIV ep join {0}.TYPE_PRIV tp on TP.TYPE_PRIV_ID = EP.TYPE_PRIV_ID " +
            //    "where EP.PER_NUM = :p_PER_NUM and TP.SIGN_INVALID = 1 and sysdate between EP.DATE_START_PRIV and " +
            //        "(case when EP.DATE_END_PRIV is not null then EP.DATE_END_PRIV else sysdate + 1 end)",
            //        Connect.Schema);
            //ocSign_Invalid.Parameters.Add("p_per_num", _per_num2);
            //if (Convert.ToInt32(ocSign_Invalid.ExecuteScalar()) > 0)
            //{                
            //    cbSign_Invalid.Checked = true;
            //}
            //else
            //{
            //    cbSign_Invalid.Checked = false;
            //}

            //dtTax_Code = new OracleDataTable("", Connect.CurConnect);
            //dtTax_Code.SelectCommand.CommandText = string.Format(
            //    Queries.GetQuery("RAS/SelectType_AD.sql"), Connect.Schema, "TAX_CODE");
            //dtTax_Code.Fill();
            //cbTax_Code.DataBindings.Add("SelectedValue", account, "TAX_CODE", true, DataSourceUpdateMode.OnPropertyChanged, null);
            //cbTax_Code.DataSource = dtTax_Code;
            //cbTax_Code.DisplayMember = "CODE_AD";
            //cbTax_Code.ValueMember = "DOMAIN_ACCOUNT_ID";

            ocStanding = new OracleCommand("", Connect.CurConnect);
            ocStanding.CommandText = string.Format(
                @"begin
                    {0}.CALC_STANDING(:p_per_num, :p_date_end_calc, :p_work_plant, :p_sick_list, :p_prev_work, 
                        :p_years, :p_months, :p_days);
                end;", Connect.Schema);
            ocStanding.BindByName = true;
            ocStanding.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            ocStanding.Parameters.Add("p_date_end_calc", OracleDbType.Date);
            ocStanding.Parameters.Add("p_work_plant", OracleDbType.Decimal);
            ocStanding.Parameters.Add("p_sick_list", OracleDbType.Decimal);
            ocStanding.Parameters.Add("p_prev_work", OracleDbType.Decimal);
            ocStanding.Parameters.Add("p_years", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_years"].DbType = DbType.Decimal;
            ocStanding.Parameters.Add("p_months", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_months"].DbType = DbType.Decimal;
            ocStanding.Parameters.Add("p_days", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ocStanding.Parameters["p_days"].DbType = DbType.Decimal;

            ocStart_Work = new OracleCommand("", Connect.CurConnect);
            ocStart_Work.CommandText = string.Format(
                @"select distinct MIN(Trunc(PW.PW_DATE_START)) OVER() START_WORK, 
                    MIN(DECODE(PW.WORK_IN_FACT,1,Trunc(PW.PW_DATE_START))) OVER() START_WORK_PLANT    
                from {0}.PREV_WORK PW
                where PER_NUM = :p_per_num", Connect.Schema);
            ocStart_Work.BindByName = true;
            ocStart_Work.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;

            // Select
            _daProfUnion = new OracleDataAdapter(string.Format(Queries.GetQuery("ras/SelectProfUnion.sql"),
                Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
            _daProfUnion.SelectCommand.BindByName = true;
            _daProfUnion.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value =
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID;
            // Insert
            _daProfUnion.InsertCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.PROFUNION_UPDATE(:RETENTION_ID, :TRANSFER_ID, :DATE_ADD, :DATE_START_RET, :DATE_END_RET); END;",
                Connect.Schema), Connect.CurConnect);
            _daProfUnion.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            _daProfUnion.InsertCommand.BindByName = true;
            _daProfUnion.InsertCommand.Parameters.Add("RETENTION_ID", OracleDbType.Decimal, 0, "RETENTION_ID").Direction = ParameterDirection.InputOutput;
            _daProfUnion.InsertCommand.Parameters["RETENTION_ID"].DbType = DbType.Decimal;
            _daProfUnion.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daProfUnion.InsertCommand.Parameters.Add("DATE_ADD", OracleDbType.Date, 0, "DATE_ADD");
            _daProfUnion.InsertCommand.Parameters.Add("DATE_START_RET", OracleDbType.Date, 0, "DATE_START_RET");
            _daProfUnion.InsertCommand.Parameters.Add("DATE_END_RET", OracleDbType.Date, 0, "DATE_END_RET");
            // Update
            _daProfUnion.UpdateCommand = _daProfUnion.InsertCommand;
            // Delete
            _daProfUnion.DeleteCommand = new OracleCommand(string.Format(
                "BEGIN {0}.PROFUNION_DELETE(:RETENTION_ID); END;",
                Connect.Schema), Connect.CurConnect);
            _daProfUnion.DeleteCommand.BindByName = true;
            _daProfUnion.DeleteCommand.Parameters.Add("RETENTION_ID", OracleDbType.Decimal, 0, "RETENTION_ID");

            _dtProfUnion = new DataTable();
            _daProfUnion.Fill(_dtProfUnion);
            dgProfUnion.AutoGenerateColumns = false;
            dgProfUnion.DataSource = _dtProfUnion;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
      
        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            // Если значение признака профсоюза изменилось, меняем данные. 
            if (signInvalid != checkB_SIGN_PROFUNION.Checked)
            {
                OracleCommand prof_update = new OracleCommand("", Connect.CurConnect);
                prof_update.BindByName = true;
                prof_update.CommandText = string.Format(Queries.GetQuery("ras/prof_update.txt"),
                    DataSourceScheme.SchemeName);
                prof_update.Parameters.Add("p_SIGN_PROFUNION", OracleDbType.Int16).Value = Convert.ToInt32(checkB_SIGN_PROFUNION.Checked);
                prof_update.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                prof_update.ExecuteNonQuery();
            }
            account.Save();
            Connect.Commit();
            MessageBox.Show("Данные сохранены!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Close(); 
        }

        #region Активация вкладки адреса

        private void tpRegistr_Enter(object sender, EventArgs e)
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
                hregion = new REGION_seq(Connect.CurConnect);
                hregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION));
                hdistrict = new DISTRICT_seq(Connect.CurConnect);
                hcity = new CITY_seq(Connect.CurConnect);
                hlocality = new LOCALITY_seq(Connect.CurConnect);
                hstreet = new STREET_seq(Connect.CurConnect);
                // Создание и настройка формы для работы с местом прописки
                r_registr = (REGISTR_obj)((CurrencyManager)BindingContext[registr]).Current;
                formregistr = new Address((object)r_registr, typeof(REGISTR_seq), rregion, rdistrict, rcity, rlocality, rstreet);
                formregistr.TopLevel = false;
                formregistr.Dock = DockStyle.Fill;
                formregistr.cbRegion.SelectedItem = null;
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.cbRegion_SelectedIndexChanged);
                formregistr.tbHouse.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_HOUSE);
                formregistr.tbBulk.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_BULK);
                formregistr.tbFlat.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_FLAT);
                formregistr.tbPhone.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_PHONE);
                formregistr.tbPost_Code.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_POST_CODE);
                //formregistr.mbDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                formregistr.deDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);                
                gbRegistr.Controls.Add(formregistr);
                string stregistr = r_registr.REG_CODE_STREET;
                if (stregistr != null && stregistr != "")
                {
                    formregistr.LoadAddress(stregistr);
                }
                formregistr.Show();

                // Создание и настройка формы для работы с местом проживания
                if (habit.Count != 0)
                {
                    r_habit = (HABIT_obj)((CurrencyManager)BindingContext[habit]).Current;
                    string sthabit = r_habit.HAB_CODE_STREET;
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
                    //formhabit.mbDate_Reg.Visible = false;
                    formhabit.deDate_Reg.Visible = false;
                    formhabit.tbPhone.Location = formhabit.mbDate_Reg.Location;
                    formhabit.lbDate.Visible = false;
                    formhabit.lbPhone.Location = new Point(24, 240);
                    formhabit.Controls.Add(lbSource_Fill);
                    formhabit.Controls.Add(cbSource_Fill);
                    source_fill = new SOURCE_FILL_seq(Connect.CurConnect);
                    source_fill.Fill(string.Format("order by {0}", SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_NAME.ToString()));
                    cbSource_Fill.AddBindingSource(registr, SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_ID, new LinkArgument(source_fill, SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_NAME));
                    if (((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).SOURCE_FILL_ID == null)
                    {
                        cbSource_Fill.SelectedItem = null;
                    }
                    //tbHab_Non_Kladr_Address.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_NON_KLADR_ADDRESS);
                    Address_None_Kladr.Per_num = per_num;
                    tbHab_Non_Kladr_Address.Text = Address_None_Kladr.Address_None_Kladr_Property;

                    lbSource_Fill.Location = new Point(formhabit.lbPhone.Location.X, formhabit.lbPhone.Location.Y + 25);
                    cbSource_Fill.Location = new Point(198, formhabit.tbPhone.Location.Y + 25);
                    gbHabit.Controls.Add(formhabit);
                    if (sthabit != null && sthabit != "")
                    {
                        formhabit.LoadAddress(sthabit);
                    }
                    formhabit.Show();
                }
                f_LoadAddress = true;
                tpRegistr.DisableAll(false, Color.White);
            }

        }

        #endregion 

        #region Работа с вкладкой Стаж и расчет стажа

        // Дата первой работы 
        DateTime startFirstWork;
        // Дата поступления на завод
        DateTime? dateHire;
        /// <summary>
        /// Событие активации вкладки Стаж и его расчет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpWork_Length_Enter(object sender, EventArgs e)
        {
            #region Старый вариант
            /*try
            {
                TRANSFER_seq transferPer_Num = new TRANSFER_seq(Connect.CurConnect);
                string textBlock = string.Format(Queries.GetQuery("SelectCurrentTransfer.sql"),
                    Staff.DataSourceScheme.SchemeName, "transfer",
                    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.PER_NUM,
                    TRANSFER_seq.ColumnsName.DATE_TRANSFER, TRANSFER_seq.ColumnsName.SIGN_COMB,
                    TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, per_num,
                    TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_COMB,
                    TRANSFER_seq.ColumnsName.DATE_HIRE);
                transferPer_Num.Fill(textBlock);
                dateHire = (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPer_Num]).Current)).DATE_HIRE;
                startFirstWork = dateHire;
                startFirstWorkFact = dateHire;
                prev_work_stag = new PREV_WORK_seq(Connect.CurConnect);
                prev_work_stag.Fill(string.Format("where {0} = {1}", PREV_WORK_seq.ColumnsName.PER_NUM, per_num));
                mbDate_Calc.Validating += new CancelEventHandler(Library.ValidatingDate);

                yearPrev = monthPrev = dayPrev = yearPlant = monthPlant = dayPlant = yearPrevBL = monthPrevBL = dayPrevBL = 0;
                foreach (PREV_WORK_obj r_prev_work in prev_work_stag)
                {
                    /// Проверка работал человек на заводе или нет.
                    /// Если да, то работаем со стажем на заводе.
                    /// Если нет, то работаем с общим стажем.
                    if (r_prev_work.WORK_IN_FACT == true)
                    {
                        /// Проверка включать ли в стаж на больничный лист.
                        /// Если признак стоит True (не включать), то увеличиваем только стаж на заводе   
                        /// Если признак стоит False (включать), то увеличиваем стаж на заводе и стаж на больничный лист
                        if (r_prev_work.MEDICAL_SIGN)
                        {
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                        }
                        else
                        {
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPrevBL, ref monthPrevBL, ref dayPrevBL);
                        }
                        /// Если дата начала периода текущей предыдущей работы на заводе меньше даты начала первой работы на заводе,
                        /// то присваеваем дате начала первой работы на заводе дату начала периода текущей предыдущей работы
                        if (startFirstWorkFact > r_prev_work.PW_DATE_START)
                        {
                            startFirstWorkFact = (DateTime)r_prev_work.PW_DATE_START;
                        }
                        /// Если дата начала периода текущей предыдущей работы меньше даты начала первой работы,
                        /// то присваеваем дате начала первой работы дату начала периода текущей предыдущей работы
                        if (startFirstWork > r_prev_work.PW_DATE_START)
                        {
                            startFirstWork = (DateTime)r_prev_work.PW_DATE_START;
                        }
                    }
                    else
                    {
                        /// Проверка включать ли в стаж на больничный лист.
                        /// Если признак стоит True (не включать), то увеличиваем только общий стаж   
                        /// Если признак стоит False (включать), то увеличиваем общий стаж и стаж на больничный лист
                        if (r_prev_work.MEDICAL_SIGN)
                        {
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPrev, ref monthPrev, ref dayPrev);
                        }
                        else
                        {
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPrev, ref monthPrev, ref dayPrev);
                            Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPrevBL, ref monthPrevBL, ref dayPrevBL);
                        }
                        /// Если дата начала периода текущей предыдущей работы меньше даты начала первой работы,
                        /// то присваеваем дате начала первой работы дату начала периода текущей предыдущей работы
                        if (startFirstWork > r_prev_work.PW_DATE_START)
                        {
                            startFirstWork = (DateTime)r_prev_work.PW_DATE_START;
                        }
                    }
                }
                mbDate_Hire.Text = dateHire.ToShortDateString();

                yearPrev += yearPlant;
                monthPrev += monthPlant;
                dayPrev += dayPlant;

                if (dayPrev > 29)
                {
                    dayPrev %= 30;
                    monthPrev += 1;
                }
                if (monthPrev > 11)
                {
                    monthPrev %= 12;
                    yearPrev += 1;
                }

                tbC_Y.Text = yearPrev > 0 ? yearPrev.ToString() : "";
                tbC_M.Text = monthPrev > 0 ? monthPrev.ToString() : "";
                tbC_D.Text = dayPrev > 0 ? dayPrev.ToString() : "";
                tbC_YZ.Text = yearPlant > 0 ? yearPlant.ToString() : "";
                tbC_MZ.Text = monthPlant > 0 ? monthPlant.ToString() : "";
                tbC_DZ.Text = dayPlant > 0 ? dayPlant.ToString() : "";

                /// Общий стаж на текущее время и больничный лист
                int yearStagNow, monthStagNow, dayStagNow, yearStagBLNow, monthStagBLNow, dayStagBLNow;
                yearStagNow = yearPrev;
                monthStagNow = monthPrev;
                dayStagNow = dayPrev;
                yearStagBLNow = yearPrevBL;
                monthStagBLNow = monthPrevBL;
                dayStagBLNow = dayPrevBL;
                // Непрерывный стаж на текущее время на заводе
                int yearPlantNow, monthPlantNow, dayPlantNow;
                yearPlantNow = monthPlantNow = dayPlantNow = 0;
                /// Переменная хранит дату, на которую осуществляется расчет
                DateTime dateCalc;
                /// Если человек уволен
                if (FormMain.flagArchive)
                {
                    /// Берем дату последнего увольнения
                    dateCalc = (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER;
                }
                else
                {
                    /// Берем текущую дату
                    dateCalc = DateTime.Now;
                }
                /// Увеличиваем стаж общий
                Library.CalculationWork_Length(dateHire, dateCalc, ref yearStagNow, ref monthStagNow, ref dayStagNow);
                /// Увеличиваем стаж на больничный лист
                Library.CalculationWork_Length(dateHire, dateCalc, ref yearStagBLNow, ref monthStagBLNow, ref dayStagBLNow);
                /// Расчитываем непрерывный стаж
                Library.CalculationWork_Length(dateHire, dateCalc, ref yearPlantNow, ref monthPlantNow, ref dayPlantNow);

                /// Стаж общий
                tbC_YN.Text = yearStagNow > 0 ? yearStagNow.ToString() : "";
                tbC_MN.Text = monthStagNow > 0 ? monthStagNow.ToString() : "";
                tbC_DN.Text = dayStagNow > 0 ? dayStagNow.ToString() : "";
                /// Стаж на больничный лист
                tbM_YN.Text = yearStagBLNow > 0 ? yearStagBLNow.ToString() : "";
                tbM_MN.Text = monthStagBLNow > 0 ? monthStagBLNow.ToString() : "";
                tbM_DN.Text = dayStagBLNow > 0 ? dayStagBLNow.ToString() : "";
                /// Стаж непрерывный
                tbU_YNZ.Text = tbU_YN.Text = yearPlantNow > 0 ? yearPlantNow.ToString() : "";
                tbU_MNZ.Text = tbU_MN.Text = monthPlantNow > 0 ? monthPlantNow.ToString() : "";
                tbU_DNZ.Text = tbU_DN.Text = dayPlantNow > 0 ? dayPlantNow.ToString() : "";

                /// Стаж общий на заводе на текущий момент
                int yearPlantStag, monthPlantStag, dayPlantStag;
                yearPlantStag = monthPlantStag = dayPlantStag = 0;
                if ((dayPlantNow + dayPlant) > 29)
                {
                    dayPlantStag = (dayPlantNow + dayPlant) % 30;
                    monthPlantStag += 1;
                }
                else
                {
                    dayPlantStag = dayPlantNow + dayPlant;
                }
                monthPlantStag += monthPlantNow + monthPlant;
                if (monthPlantStag > 11)
                {
                    monthPlantStag %= 12;
                    yearPlantStag += 1;
                }
                yearPlantStag += yearPlantNow + yearPlant;

                tbC_YNZ.Text = yearPlantStag > 0 ? yearPlantStag.ToString() : "";
                tbC_MNZ.Text = monthPlantStag > 0 ? monthPlantStag.ToString() : "";
                tbC_DNZ.Text = dayPlantStag > 0 ? dayPlantStag.ToString() : "";
            }
            catch { }*/
            #endregion
            #region Новый вариант
            try
            {
                mbDate_Calc.Validating += new CancelEventHandler(Library.ValidatingDate);
                dateHire = ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
                if (dateHire != null)
                {
                    mbDate_Hire.Text = dateHire.Value.ToShortDateString();
                    OracleDataReader odreader = ocStart_Work.ExecuteReader();
                    if (odreader.Read())
                    {
                        startFirstWork = Convert.ToDateTime(odreader["START_WORK"]);                        
                    }
                    else
                    {
                        startFirstWork = (DateTime)dateHire;
                    }
                    // Начинаем считать стаж 
                    // Общий на дату поступления
                    ocStanding.Parameters["p_date_end_calc"].Value = dateHire.Value.AddDays(-1);
                    ocStanding.Parameters["p_work_plant"].Value = null;
                    ocStanding.Parameters["p_sick_list"].Value = null;
                    ocStanding.Parameters["p_prev_work"].Value = 1;
                    ocStanding.ExecuteNonQuery();
                    tbC_Y.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_M.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_D.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Общий на текущий момент
                    ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.ExecuteNonQuery();
                    tbC_YN.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MN.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DN.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Общий на авиазаводе на дату поступления
                    ocStanding.Parameters["p_date_end_calc"].Value = dateHire.Value.AddDays(-1);
                    ocStanding.Parameters["p_work_plant"].Value = 1;
                    ocStanding.ExecuteNonQuery();
                    tbC_YZ.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MZ.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DZ.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Общий на авиазаводе на текущий момент
                    ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.ExecuteNonQuery();
                    tbC_YNZ.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MNZ.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DNZ.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Непрерывный на текущий момент
                    ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.Parameters["p_prev_work"].Value = 0;
                    ocStanding.ExecuteNonQuery();
                    tbU_YN.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbU_MN.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbU_DN.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Непрерывный на авиазаводе на текущий момент
                    /*ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.Parameters["p_prev_work"].Value = 0;
                    ocStanding.ExecuteNonQuery();*/
                    tbU_YNZ.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbU_MNZ.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbU_DNZ.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // На больничный лист
                    ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.Parameters["p_prev_work"].Value = 1;
                    ocStanding.Parameters["p_work_plant"].Value = null;
                    ocStanding.Parameters["p_sick_list"].Value = 0;
                    ocStanding.ExecuteNonQuery();
                    tbM_YN.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbM_MN.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbM_DN.Text = ocStanding.Parameters["p_days"].Value.ToString();
                }
            }
            catch { }
            #endregion
            mbDate_Calc.Focus();
        }

        /// <summary>
        /// Событие нажатия кнопки расчета стажа на введенную дату
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCalculation_Click(object sender, EventArgs e)
        {
            /*if (mbDate_Calc.Text.Replace(".", " ").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату расчета.", "АСУ \"Кадры\"", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                mbDate_Calc.Focus();
                return;
            }
            if (Convert.ToDateTime(mbDate_Calc.Text) < startFirstWork)
            {
                MessageBox.Show("Стаж не может быть рассчитан на введенную дату.", "АСУ \"Кадры\"", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                mbDate_Calc.Focus();
                return;
            }
            // Общий стаж на расчетную дату
            int yearStagNow, monthStagNow, dayStagNow, yearStagBLNow, monthStagBLNow, dayStagBLNow;
            yearStagNow = yearPrev;
            monthStagNow = monthPrev;
            dayStagNow = dayPrev;
            yearStagBLNow = yearPrevBL;
            monthStagBLNow = monthPrevBL;
            dayStagBLNow = dayPrevBL;

            // Непрерывный стаж на расчетную дату на заводе
            int yearPlantNow, monthPlantNow, dayPlantNow;
            yearPlantNow = monthPlantNow = dayPlantNow = 0;

            Library.CalculationWork_Length(dateHire, Convert.ToDateTime(mbDate_Calc.Text), ref yearStagNow,
                ref monthStagNow, ref dayStagNow);
            Library.CalculationWork_Length(dateHire, Convert.ToDateTime(mbDate_Calc.Text), ref yearStagBLNow,
                ref monthStagBLNow, ref dayStagBLNow);
            Library.CalculationWork_Length(dateHire, Convert.ToDateTime(mbDate_Calc.Text), ref yearPlantNow,
                ref monthPlantNow, ref dayPlantNow);

            tbC_YCalc.Text = yearStagNow > 0 ? yearStagNow.ToString() : "";
            tbC_MCalc.Text = monthStagNow > 0 ? monthStagNow.ToString() : "";
            tbC_DCalc.Text = dayStagNow > 0 ? dayStagNow.ToString() : "";

            tbM_YCalc.Text = yearStagBLNow > 0 ? yearStagBLNow.ToString() : "";
            tbM_MCalc.Text = monthStagBLNow > 0 ? monthStagBLNow.ToString() : "";
            tbM_DCalc.Text = dayStagBLNow > 0 ? dayStagBLNow.ToString() : "";

            tbU_YZCalc.Text = tbU_YCalc.Text = yearPlantNow > 0 ? yearPlantNow.ToString() : "";
            tbU_MZCalc.Text = tbU_MCalc.Text = monthPlantNow > 0 ? monthPlantNow.ToString() : "";
            tbU_DZCalc.Text = tbU_DCalc.Text = dayPlantNow > 0 ? dayPlantNow.ToString() : "";

            // Стаж общий на заводе на расчетную дату
            int yearPlantStag, monthPlantStag, dayPlantStag;
            yearPlantStag = monthPlantStag = dayPlantStag = 0;
            if ((dayPlantNow + dayPlant) > 29)
            {
                dayPlantStag = (dayPlantNow + dayPlant) % 30;
                monthPlantStag += 1;
            }
            else
            {
                dayPlantStag = dayPlantNow + dayPlant;
            }
            monthPlantStag += monthPlantNow + monthPlant;
            if (monthPlantStag > 11)
            {
                monthPlantStag %= 12;
                yearPlantStag += 1;
            }
            yearPlantStag += yearPlantNow + yearPlant;

            tbC_YZCalc.Text = yearPlantStag > 0 ? yearPlantStag.ToString() : "";
            tbC_MZCalc.Text = monthPlantStag > 0 ? monthPlantStag.ToString() : "";
            tbC_DZCalc.Text = dayPlantStag > 0 ? dayPlantStag.ToString() : "";*/
        }

        /// <summary>
        /// Ввод даты, на которую нужно расчитать стаж
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbDate_Calc_TextChanged(object sender, EventArgs e)
        {
            #region Старый вариант
            /*/// Если длина введенного текста равна 10, то есть введена полная дата, производим расчет
            if (mbDate_Calc.Text.Trim().Length == 10)
            {
                try
                {
                    /// Получаем дату расчета
                    DateTime dateCalc = Convert.ToDateTime(mbDate_Calc.Text);
                    if (dateCalc < startFirstWork)
                    {
                        MessageBox.Show("Стаж не может быть рассчитан на введенную дату./n" +
                            "Она раньше даты начала трудовой деятельности.", "АСУ \"Кадры\"", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        mbDate_Calc.Focus();
                        return;
                    }
                    // Общий стаж на расчетную дату
                    int yearStagNow, monthStagNow, dayStagNow, yearStagBLNow, monthStagBLNow, dayStagBLNow;
                    yearStagNow = yearPrev;
                    monthStagNow = monthPrev;
                    dayStagNow = dayPrev;
                    yearStagBLNow = yearPrevBL;
                    monthStagBLNow = monthPrevBL;
                    dayStagBLNow = dayPrevBL;

                    // Непрерывный стаж на расчетную дату на заводе
                    int yearPlantNow, monthPlantNow, dayPlantNow;
                    yearPlantNow = monthPlantNow = dayPlantNow = 0;

                    Library.CalculationWork_Length(dateHire, dateCalc, ref yearStagNow, ref monthStagNow, ref dayStagNow);
                    Library.CalculationWork_Length(dateHire, dateCalc, ref yearStagBLNow, ref monthStagBLNow, ref dayStagBLNow);
                    Library.CalculationWork_Length(dateHire, dateCalc, ref yearPlantNow, ref monthPlantNow, ref dayPlantNow);

                    tbC_YCalc.Text = yearStagNow > 0 ? yearStagNow.ToString() : "";
                    tbC_MCalc.Text = monthStagNow > 0 ? monthStagNow.ToString() : "";
                    tbC_DCalc.Text = dayStagNow > 0 ? dayStagNow.ToString() : "";

                    tbM_YCalc.Text = yearStagBLNow > 0 ? yearStagBLNow.ToString() : "";
                    tbM_MCalc.Text = monthStagBLNow > 0 ? monthStagBLNow.ToString() : "";
                    tbM_DCalc.Text = dayStagBLNow > 0 ? dayStagBLNow.ToString() : "";

                    tbU_YZCalc.Text = tbU_YCalc.Text = yearPlantNow > 0 ? yearPlantNow.ToString() : "";
                    tbU_MZCalc.Text = tbU_MCalc.Text = monthPlantNow > 0 ? monthPlantNow.ToString() : "";
                    tbU_DZCalc.Text = tbU_DCalc.Text = dayPlantNow > 0 ? dayPlantNow.ToString() : "";

                    // Стаж общий на заводе на расчетную дату
                    int yearPlantStag, monthPlantStag, dayPlantStag;
                    yearPlantStag = monthPlantStag = dayPlantStag = 0;
                    if ((dayPlantNow + dayPlant) > 29)
                    {
                        dayPlantStag = (dayPlantNow + dayPlant) % 30;
                        monthPlantStag += 1;
                    }
                    else
                    {
                        dayPlantStag = dayPlantNow + dayPlant;
                    }
                    monthPlantStag += monthPlantNow + monthPlant;
                    if (monthPlantStag > 11)
                    {
                        monthPlantStag %= 12;
                        yearPlantStag += 1;
                    }
                    yearPlantStag += yearPlantNow + yearPlant;

                    tbC_YZCalc.Text = yearPlantStag > 0 ? yearPlantStag.ToString() : "";
                    tbC_MZCalc.Text = monthPlantStag > 0 ? monthPlantStag.ToString() : "";
                    tbC_DZCalc.Text = dayPlantStag > 0 ? dayPlantStag.ToString() : "";
                }
                catch
                { }
            }*/
            #endregion
            #region Новый вариант
            /// Если длина введенного текста равна 10, то есть введена полная дата, производим расчет
            if (mbDate_Calc.Text.Trim().Length == 10)
            {
                try
                {
                    /// Получаем дату расчета
                    DateTime dateCalc = Convert.ToDateTime(mbDate_Calc.Text);
                    if (dateCalc < startFirstWork)
                    {
                        MessageBox.Show("Стаж не может быть рассчитан на введенную дату./n" +
                            "Она раньше даты начала трудовой деятельности.", "АСУ \"Кадры\"", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        mbDate_Calc.Focus();
                        return;
                    }
                    // Начинаем считать стаж 
                    // Общий на заданную дату
                    ocStanding.Parameters["p_date_end_calc"].Value = dateCalc;
                    ocStanding.Parameters["p_work_plant"].Value = null;
                    ocStanding.Parameters["p_sick_list"].Value = null;
                    ocStanding.Parameters["p_prev_work"].Value = 1;
                    ocStanding.ExecuteNonQuery();
                    tbC_YCalc.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MCalc.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DCalc.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Общий на авиазаводе на заданную дату
                    ocStanding.Parameters["p_work_plant"].Value = 1;
                    ocStanding.ExecuteNonQuery();
                    tbC_YZCalc.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MZCalc.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DZCalc.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Непрерывный на заданную дату
                    ocStanding.Parameters["p_prev_work"].Value = 0;
                    ocStanding.ExecuteNonQuery();
                    tbU_YCalc.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbU_MCalc.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbU_DCalc.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Непрерывный на авиазаводе на заданную дату
                    /*ocStanding.Parameters["p_date_end_calc"].Value = DateTime.Now;
                    ocStanding.Parameters["p_prev_work"].Value = 0;
                    ocStanding.ExecuteNonQuery();*/
                    tbU_YZCalc.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbU_MZCalc.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbU_DZCalc.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // На больничный лист
                    ocStanding.Parameters["p_prev_work"].Value = 1;
                    ocStanding.Parameters["p_work_plant"].Value = null;
                    ocStanding.Parameters["p_sick_list"].Value = 0;
                    ocStanding.ExecuteNonQuery();
                    tbM_YCalc.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbM_MCalc.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbM_DCalc.Text = ocStanding.Parameters["p_days"].Value.ToString();
                }
                catch
                { }
            }
            #endregion
        }

        #endregion

        #region Работа с таблицей Предыдущая деятельность

        int countPrev_Work = 0;
        // Активация вкладки Предыдущая деятельность
        private void tpPrev_Work_Enter(object sender, EventArgs e)
        {
            if (!f_LoadPrev_Work)
            {
                // Инициализация таблиц и заполнение их данными                
                prev_work = new PREV_WORK_seq(Connect.CurConnect);
                prev_work.Fill(string.Format("where {0} = '{1}' order by {2}",
                PREV_WORK_seq.ColumnsName.PER_NUM, per_num, PREV_WORK_seq.ColumnsName.PW_DATE_START));
                dgPrev_Work.AddBindingSource(prev_work);
                dgPrev_Work.Columns["per_num"].Visible = false;
                dgPrev_Work.Invalidate();
                DataGridViewTextBoxColumn dcYear = new DataGridViewTextBoxColumn();
                dcYear.Name = "stagYear";
                dgPrev_Work.Columns.Insert(6, dcYear);
                dgPrev_Work.Columns["stagYear"].HeaderText = "Стаж лет";
                dgPrev_Work.Columns["stagYear"].Width = 50;
                dgPrev_Work.Columns["stagYear"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridViewTextBoxColumn dcMonth = new DataGridViewTextBoxColumn();
                dcMonth.Name = "stagMonth";
                dgPrev_Work.Columns.Insert(7, dcMonth);
                dgPrev_Work.Columns["stagMonth"].HeaderText = "Стаж мес";
                dgPrev_Work.Columns["stagMonth"].Width = 50;
                dgPrev_Work.Columns["stagMonth"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridViewTextBoxColumn dcDay = new DataGridViewTextBoxColumn();
                dcDay.Name = "stagDay";
                dgPrev_Work.Columns.Insert(8, dcDay);
                dgPrev_Work.Columns["stagDay"].HeaderText = "Стаж дней";
                dgPrev_Work.Columns["stagDay"].Width = 50;
                dgPrev_Work.Columns["stagDay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                f_LoadPrev_Work = true;
                countPrev_Work = dgPrev_Work.Rows.Count;
            }
        }

        bool flagPrev_Work = true;

        private void dgViewPrev_Work_MouseEnter(object sender, EventArgs e)
        {
            if (flagPrev_Work)
            {
                decimal stagYear, stagMonth, stagDay;
                for (int i = 0; i < dgPrev_Work.Rows.Count; i++)
                {
                    stagYear = stagMonth = stagDay = 0;
                    ((CurrencyManager)BindingContext[prev_work]).Position = i;
                    Library.CalcStanding((DateTime)((PREV_WORK_obj)(((CurrencyManager)BindingContext[prev_work]).Current)).PW_DATE_START.Value,
                        (DateTime)((PREV_WORK_obj)(((CurrencyManager)BindingContext[prev_work]).Current)).PW_DATE_END.Value,
                        ref stagYear, ref stagMonth, ref stagDay);
                    dgPrev_Work.Rows[i].Cells["stagYear"].Value = stagYear.ToString();
                    dgPrev_Work.Rows[i].Cells["stagMonth"].Value = stagMonth.ToString();
                    dgPrev_Work.Rows[i].Cells["stagDay"].Value = stagDay.ToString();
                }
                if (countPrev_Work == dgPrev_Work.Rows.Count)
                    flagPrev_Work = false;
            }
        }


        // Редактирование данных о предыдущей деятельности
        private void btEditPrev_Work_Click(object sender, EventArgs e)
        {
            //if (dgPrev_Work.RowCount == 0)
            //    return;
            //Prev_Work formprev_work = new Prev_Work(per_num, false, BindingContext[prev_work].Position,
            //    prev_work, FormMain.position, dgPrev_Work, pnVisible,
            //    (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE);
            //formprev_work.Text = "Просмотр данных о предыдущей деятельности";
            //formprev_work.ShowDialog();
        }

        private void dgViewPrev_Work_DoubleClick(object sender, EventArgs e)
        {
            btEditPrev_Work_Click(sender, e);
        }

        private void dgViewPrev_Work_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
                btEditPrev_Work_Click(sender, e);
        }

        # endregion

        #region Работа с таблицей Социальные льготы

        // Активация вкладки Социальные льготы
        private void tpEmp_Priv_Enter(object sender, EventArgs e)
        {
            if (!f_LoadEmp_priv)
            {
                // Инициализация таблиц и заполнение их данными    
                type_priv = new TYPE_PRIV_seq(Connect.CurConnect);
                type_priv.Fill(string.Format("where SIGN_INVALID = 1 order by {0}", TYPE_PRIV_seq.ColumnsName.NAME_PRIV));
                base_doc = new BASE_DOC_seq(Connect.CurConnect);
                base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                emp_priv = new EMP_PRIV_seq(Connect.CurConnect);
                emp_priv.Fill(string.Format("where per_num = '{0}' and TYPE_PRIV_ID in " +
                    "(select TYPE_PRIV_ID from {1}.TYPE_PRIV where SIGN_INVALID = 1) " +
                    "order by DATE_START_PRIV", per_num, Connect.Schema));
                dgEmp_Priv.AddBindingSource(emp_priv, new LinkArgument(type_priv, "name_priv"), new LinkArgument(base_doc, "base_doc_name"));
                dgEmp_Priv.Invalidate();
                dgEmp_Priv.Columns["per_num"].Visible = false;
                f_LoadEmp_priv = true;
            }
        }

        // Добавление данных Социальные льготы
        private void btAddEmp_Priv_Click(object sender, EventArgs e)
        {
            formemp_priv = new Emp_Priv(per_num, true, BindingContext[emp_priv].Position, emp_priv, type_priv, base_doc, dgEmp_Priv, pnVisible);
            formemp_priv.Text = "Добавление данных о социальных льготах и инвалидности";
            formemp_priv.ShowDialog();
        }

        // Редактирование данных Социальные льготы
        private void btEditEmp_Priv_Click(object sender, EventArgs e)
        {
            if (dgEmp_Priv.RowCount == 0)
                return;
            formemp_priv = new Emp_Priv(per_num, false, BindingContext[emp_priv].Position, emp_priv, type_priv, base_doc, dgEmp_Priv, pnVisible);
            formemp_priv.Text = "Редактирование данных о социальных льготах и инвалидности";
            formemp_priv.ShowDialog();
        }

        // Уданение данных Социальные льготы
        private void btDeleteEmp_Priv_Click(object sender, EventArgs e)
        {
            if (dgEmp_Priv.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgEmp_Priv.Rows.RemoveAt(((CurrencyManager)BindingContext[emp_priv]).Position);
                emp_priv.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Корректировка иждивенцев

        private void tpDependants_Enter(object sender, EventArgs e)
        {
            if (!f_LoadDep)
            {
                odaDep = new OracleDataAdapter("", Connect.CurConnect);
                odaDep.SelectCommand.CommandText = string.Format(Queries.GetQuery("Ras/SelectDependents.sql"),
                    Connect.Schema);
                odaDep.SelectCommand.BindByName = true;
                odaDep.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
                odaDep.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Varchar2);
                odaDep.SelectCommand.Parameters["p_per_num"].Value = per_num;
                odaDep.SelectCommand.Parameters["p_sign_comb"].Value = sign_comb == 0 ? " " : "2";
                odaDep.UpdateCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Ras/UpdateDependents.sql"), Connect.Schema), Connect.CurConnect);
                odaDep.UpdateCommand.BindByName = true;
                odaDep.UpdateCommand.Parameters.Add("p_CODE_SUBDIV", OracleDbType.Decimal, 3, "CODE_SUBDIV");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_TAX_CODE", OracleDbType.Decimal, 2, "DEP_TAX_CODE");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT114", OracleDbType.Decimal, 2, "DEP_COUNT114");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT115", OracleDbType.Decimal, 2, "DEP_COUNT115");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT116", OracleDbType.Decimal, 2, "DEP_COUNT116");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT117", OracleDbType.Decimal, 2, "DEP_COUNT117");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT118", OracleDbType.Decimal, 2, "DEP_COUNT118");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT119", OracleDbType.Decimal, 2, "DEP_COUNT119");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT120", OracleDbType.Decimal, 2, "DEP_COUNT120");
                odaDep.UpdateCommand.Parameters.Add("p_DEP_COUNT121", OracleDbType.Decimal, 2, "DEP_COUNT121");
                odaDep.UpdateCommand.Parameters.Add("p_dep_id", OracleDbType.Decimal, 36, "DEPENDENTS_ID");
                odaDep.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                dtDep = new OracleDataTable("", Connect.CurConnect);
                odaDep.Fill(dtDep);

                dgDependents.AutoGenerateColumns = false;
                dgDependents.DataSource = dtDep;
                DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
                c1.Name = "DEP_MONTH";
                c1.HeaderText = "Месяц";
                c1.ReadOnly = true;
                c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgDependents.Columns.Add(c1);
                dgDependents.Columns["DEP_MONTH"].DataPropertyName = "DEP_MONTH";

                MDataGridViewMaskedTextColumn c2 = new MDataGridViewMaskedTextColumn();
                c2.Name = "CODE_SUBDIV";
                c2.HeaderText = "Подр.";
                c2.CellMask = "000";
                dgDependents.Columns.Add(c2);
                dgDependents.Columns["CODE_SUBDIV"].DataPropertyName = "CODE_SUBDIV";

                c1 = new DataGridViewTextBoxColumn();
                c1.Name = "SIGN_COMB";
                c1.HeaderText = "Совм.";
                c1.ReadOnly = true;
                c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgDependents.Columns.Add(c1);
                dgDependents.Columns["SIGN_COMB"].DataPropertyName = "SIGN_COMB";

                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_TAX_CODE", "Шифр налога", "DEP_TAX_CODE") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT114", "Шифр 114", "DEP_COUNT114") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT115", "Шифр 115", "DEP_COUNT115") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT116", "Шифр 116", "DEP_COUNT116") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT117", "Шифр 117", "DEP_COUNT117") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT118", "Шифр 118", "DEP_COUNT118") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT119", "Шифр 119", "DEP_COUNT119") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT120", "Шифр 120", "DEP_COUNT120") { MinimumValue = 0, MaximumValue = 99 });
                dgDependents.Columns.Add(new MDataGridViewNumericColumn("DEP_COUNT121", "Шифр 121", "DEP_COUNT121") { MinimumValue = 0, MaximumValue = 99 });
                  
                f_LoadDep = true;
            }
        }

        private void btEditDep_Click(object sender, EventArgs e)
        {
            dgDependents.ReadOnly = false;
            btSaveDep.Enabled = true;
            btEditDep.Enabled = false;
        }

        private void btSaveDep_Click(object sender, EventArgs e)
        {
            dgDependents.ReadOnly = true;
            btSaveDep.Enabled = false;
            btEditDep.Enabled = true;
            try
            {
                //DataTable dtChanges = dtDep.GetChanges();            
                //if (dtChanges != null && dtChanges.Rows.Count > 0)
                //{
                    odaDep.Update(dtDep);
                    Connect.Commit();
                    MessageBox.Show("Данные по иждивенцам сохранены!", "АРМ 'Кадры'",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);                
                //} 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        #endregion        

        #region Активация вкладки Отпуск по уходу за ребенком

        /// <summary>
        /// Активация вкладки Отпуск по уходу за ребенком
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpVacBabyMinding_Enter(object sender, EventArgs e)
        {
            if (!f_LoadRelativeChild)
            {
                // Инициализация таблиц и заполнение их данными                
                rel_typeChild = new REL_TYPE_seq(Connect.CurConnect);
                rel_typeChild.Fill("where upper(NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА') order by NAME_REL");
                relativeChild = new RELATIVE_seq(Connect.CurConnect);
                relativeChild.Fill(string.Format("where per_num = '{1}' " +
                    "and REL_ID in (select RT.REL_ID from {0}.rel_type RT " +
                                    "where upper(RT.NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА') )" +
                    "and ((REL_BIRTH_DATE is not null and REL_BIRTH_DATE >= ADD_MONTHS(sysdate, -216)) or " +
                    "(REL_BIRTH_YEAR is not null and REL_BIRTH_YEAR >= extract(year from sysdate) - 18))",
                    DataSourceScheme.SchemeName, per_num));
                child_rearing_leave = new CHILD_REARING_LEAVE_seq(Connect.CurConnect);
                dgChild_Rearing_Leave.AddBindingSource(relativeChild, new LinkArgument(rel_typeChild, "name_rel"));
                dgChild_Rearing_Leave.Invalidate();
                dgChild_Rearing_Leave.Columns["REL_LAST_NAME"].DisplayIndex = 0;
                dgChild_Rearing_Leave.Columns["REL_FIRST_NAME"].DisplayIndex = 1;
                dgChild_Rearing_Leave.Columns["REL_MIDDLE_NAME"].DisplayIndex = 2;
                dgChild_Rearing_Leave.Columns["REL_BIRTH_DATE"].DisplayIndex = 3;
                dgChild_Rearing_Leave.Columns["REL_ID"].DisplayIndex = 4;
                dgChild_Rearing_Leave.Columns["REL_BIRTH_CERTIFICATE_SER"].DisplayIndex = 5;
                dgChild_Rearing_Leave.Columns["REL_BIRTH_CERTIFICATE_NUM"].DisplayIndex = 6;
                dgChild_Rearing_Leave.Columns["per_num"].Visible = false;
                dgChild_Rearing_Leave.Columns["REL_BIRTH_YEAR"].Visible = false;
                dgChild_Rearing_Leave.Columns["rel_per_num"].Visible = false;
                f_LoadRelativeChild = true;
                tbRel_Last_Name.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_LAST_NAME);
                tbRel_First_Name.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_FIRST_NAME);
                tbRel_Middle_Name.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_MIDDLE_NAME);
                deRel_Birth_Date.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_BIRTH_DATE);
                cbRel_ID.AddBindingSource(relativeChild, REL_TYPE_seq.ColumnsName.REL_ID,
                    new LinkArgument(rel_typeChild, REL_TYPE_seq.ColumnsName.NAME_REL));
                tbRel_Birth_Certification_ser.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_BIRTH_CERTIFICATE_SER);
                tbRel_Birth_Certification_num.AddBindingSource(relativeChild, RELATIVE_seq.ColumnsName.REL_BIRTH_CERTIFICATE_NUM);
                deCRL_Date_Begin_1_5.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_BEGIN_1_5);
                deCRL_Date_End_1_5.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_END_1_5);
                chCRL_Breakpoint_1_5.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_BREAKPOINT_1_5);
                deCRL_Date_Begin_3.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_BEGIN_3);
                deCRL_Date_End_3.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_END_3);
                chCRL_Breakpoint_3.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_BREAKPOINT_3);
                deCRL_Date_Departure.AddBindingSource(child_rearing_leave,
                    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_DEPARTURE);
                if (relativeChild.Count == 0)
                {
                    cbRel_ID.SelectedItem = null;
                }
                EnableForChild(false);
            }
            else
            {
                relativeChild.Clear();
                relativeChild.Fill(string.Format("where per_num = '{1}' " +
                    "and REL_ID in (select RT.REL_ID from {0}.rel_type RT " +
                                    "where upper(RT.NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА') )" +
                    "and ((REL_BIRTH_DATE is not null and REL_BIRTH_DATE >= ADD_MONTHS(sysdate, -216)) or " +
                    "(REL_BIRTH_YEAR is not null and REL_BIRTH_YEAR >= extract(year from sysdate) - 18))",
                    DataSourceScheme.SchemeName, per_num));
            }
        }

        /// <summary>
        /// Настройка компонентов вкладки Отпуск по уходу за ребенком
        /// </summary>
        /// <param name="_flag"></param>
        void EnableForChild(bool _flag)
        {
            tbRel_Last_Name.Enabled = _flag;
            tbRel_First_Name.Enabled = _flag;
            tbRel_Middle_Name.Enabled = _flag;
            deRel_Birth_Date.Enabled = _flag;
            cbRel_ID.Enabled = _flag;
            tbRel_Birth_Certification_ser.Enabled = _flag;
            tbRel_Birth_Certification_num.Enabled = _flag;
            deCRL_Date_Begin_1_5.Enabled = _flag;
            deCRL_Date_End_1_5.Enabled = _flag;
            chCRL_Breakpoint_1_5.Enabled = _flag;
            chCRL_Prolongation_1_5.Enabled = _flag;
            deCRL_Date_Begin_3.Enabled = _flag;
            deCRL_Date_End_3.Enabled = _flag;
            chCRL_Breakpoint_3.Enabled = _flag;
            dgChild_Rearing_Leave.Enabled = !_flag;
        }

        /// <summary>
        /// Изменение отображаемых данных по ребенку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgChild_Rearing_Leave_SelectionChanged(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.CurrentRow != null)
            {
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID != null)
                {
                    child_rearing_leave.Clear();
                    child_rearing_leave.Fill(string.Format("where relative_id = {0}",
                        ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID));
                    if (child_rearing_leave.Count > 0)
                    {
                        if (((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_PROLONGATION_1_5)
                        {
                            chCRL_Prolongation_1_5.Checked = true;
                        }
                        else
                        {
                            chCRL_Prolongation_1_5.Checked = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Работа с таблицей Старое ФИО

        /// <summary>
        /// Активация вкладки Смена ФИО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgOld_FIO_Enter(object sender, EventArgs e)
        {
            if (!f_LoadOld_FIO)
            {
                DataTable dtOld_FIO = new DataTable();
                OracleDataAdapter daOdl_FIO = new OracleDataAdapter(
                    string.Format("select OLD_LAST_NAME \"Старая фамилия\", OLD_FIRST_NAME \"Старое имя\", " +
                    "OLD_MIDDLE_NAME \"Старое отчество\" from {0}.EMP_OLD_NAME where per_num = :p_per_num",
                    Connect.Schema), Connect.CurConnect);
                daOdl_FIO.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                daOdl_FIO.Fill(dtOld_FIO);
                dgOld_FIO.DataSource = dtOld_FIO;
                dgOld_FIO.Invalidate();
                f_LoadOld_FIO = true;
            }
        }

        # endregion

        private void tsbAddProfUnion_Click(object sender, EventArgs e)
        {
            DataRowView currentRow = _dtProfUnion.DefaultView.AddNew();
            _dtProfUnion.Rows.InsertAt(currentRow.Row, 0);
        }

        private void tsbDeleteProfUnion_Click(object sender, EventArgs e)
        {
            if (dgProfUnion.CurrentRow != null && MessageBox.Show("Удалить строку?", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                (dgProfUnion.CurrentRow.DataBoundItem as DataRowView).Delete();
                if (SaveProfUnion())
                    _dtProfUnion.AcceptChanges();
                else
                    _dtProfUnion.RejectChanges();
            }
        }

        private void tsbSaveProfUnion_Click(object sender, EventArgs e)
        {
            if (SaveProfUnion())
            {
                _dtProfUnion.AcceptChanges();
                MessageBox.Show("Данные сохранены!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                _dtProfUnion.RejectChanges();
        }

        bool SaveProfUnion()
        {
            dgProfUnion.EndEdit(DataGridViewDataErrorContexts.Commit);
            dgProfUnion.CommitEdit(DataGridViewDataErrorContexts.Commit);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                dgProfUnion.BindingContext[_dtProfUnion].EndCurrentEdit();
                for (int i = 0; i < _dtProfUnion.Rows.Count; ++i)
                {
                    if (_dtProfUnion.Rows[i].RowState == DataRowState.Added)
                    {
                        _dtProfUnion.Rows[i]["TRANSFER_ID"] = _transfer_id;
                        _dtProfUnion.Rows[i]["DATE_ADD"] = DateTime.Now;
                    }
                }
                _daProfUnion.UpdateCommand.Transaction = transact;
                _daProfUnion.InsertCommand.Transaction = transact;
                _daProfUnion.DeleteCommand.Transaction = transact;
                _daProfUnion.Update(_dtProfUnion);
                transact.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АРМ Бухгалтера-расчетчика - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void tsbCancelProfUnion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Отменить все изменения?", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == 
                System.Windows.Forms.DialogResult.Yes)
            {
                _dtProfUnion.RejectChanges();
            }
        }
    }
}
