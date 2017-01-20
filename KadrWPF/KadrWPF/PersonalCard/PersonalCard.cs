using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using System.Reflection;
using LibraryKadr;
using System.IO;
using System.Drawing.Printing;
using System.Data.Odbc;
using WpfControlLibrary;
using PercoXML;
namespace Kadr
{
    public partial class PersonalCard : Form
    {
        // Объявление таблиц
        PER_NUM_BOOK_seq per_num_book;
        MAR_STATE_seq mar_state;
        SOURCE_COMPLECT_seq source_complect;
        SOURCE_EMPLOYABILITY_seq source_employability;
        TYPE_PER_DOC_seq type_per_doc;
        SPECIALITY_seq speciality;
        INSTIT_seq instit;
        QUAL_seq qual;
        GROUP_SPEC_seq group_spec;
        TYPE_STUDY_seq type_study;
        TYPE_EDU_seq type_edu;
        REL_TYPE_seq rel_type;
        REL_TYPE_seq rel_typeChild;
        MIL_RANK_seq mil_rank;
        MED_CLASSIF_seq med_classif;
        COMM_seq comm;
        TYPE_TROOP_seq type_troop;
        MIL_CAT_seq mil_cat;
        LANG_seq lang;
        LEVEL_KNOW_seq level_know;
        BASE_DOC_seq base_doc;
        TYPE_PRIV_seq type_priv;
        TYPE_RISE_QUAL_seq type_rise_qual;
        TYPE_POSTG_STUDY_seq type_postg_study;
        SOURCE_FILL_seq source_fill;
        EMP_seq emp;
        PER_DATA_seq per_data;
        PASSPORT_seq passport;
        TRANSFER_seq transfer;
        REGISTR_seq registr;
        HABIT_seq habit;
        EDU_seq edu;
        RELATIVE_seq relative;
        RELATIVE_seq relativeChild;
        MIL_CARD_seq mil_card;
        PREV_WORK_seq prev_work;
        KNOW_LANG_seq know_lang;
        REWARD_seq reward;
        REWARD_NAME_seq reward_name;
        EMP_PRIV_seq emp_priv;
        ATTEST_seq attest;
        PROF_TRAIN_seq prof_train;
        RISE_QUAL_seq rise_qual;
        POSTG_STUDY_seq postg_study;
        REGION_seq rregion, hregion;
        DISTRICT_seq rdistrict, hdistrict;
        CITY_seq rcity, hcity;
        LOCALITY_seq rlocality, hlocality;
        STREET_seq rstreet, hstreet;
        FOREIGN_PASSPORT_seq foreign_passport;
        SYSTEM_USER_seq system_user;
        //CHILD_REARING_LEAVE_seq child_rearing_leave;
        RISE_QUAL_seq compulsary_edu;
        PENSION_PLAN_seq pension_plan;
        /// <summary>
        /// Переводы по предыдущей работе
        /// </summary>
        TRANSFER_seq prevTransfer;
        //MAT_RESP_PERSON_seq mat_resp_person;

        // Объявление строк для таблиц
        PASSPORT_obj r_passport;
        REGISTR_obj r_registr;
        HABIT_obj r_habit;
        MIL_CARD_obj r_mil_card;

        // Объявление временной таблицы Пол работника
        DataTable Sex_Table, _dtMed_Inspection, _dtYoung_Specialist, _dtChild_Care_Leave_1_5, _dtChild_Care_Leave_3;
        OracleDataAdapter _daMed_Inspection, _daYoung_Specialist, _daChild_Care_Leave, _daViolation_Emp;
        DataSet _dsViolation_Emp;

        // Объявление форм для работы с таблицами
        Edu formedu;
        Family formfamily;
        Prev_Work formprev_work;
        Know_Lang formknow_lang;
        Rewards formrewards;
        Emp_Priv formemp_priv;
        //Vac formvac;
        Attest formattest;
        Prof_Train formprof_train;
        Rise_Qual formrise_qual;
        PostG_Study formpostg_study;
        Foreign_Passport formFP;
        // Панель отображения отсутсвия данных
        VisiblePanel pnVisible = new VisiblePanel();
        OracleCommand ocStanding, ocStart_Work, ocNonstate_Pension, ocUpdateNPP;
        // Флаги добавления и редактирования данных в таблицах
        bool f_AddEdu, f_AddRelative, f_AddPrev_work, f_AddKnow_lang, f_AddReward, f_AddEmp_priv;
        bool f_AddAttest, f_AddProf_train, f_AddRise_qual, f_AddPostg_study, f_AddFP, f_AddCompulsory_edu;

        // Флаги загрузки таблиц
        bool f_LoadEdu, f_LoadRelative, f_LoadMil_Card, f_LoadPrev_Work, f_LoadKnow_Lang, f_LoadReward, f_LoadEmp_priv;
        bool f_LoadBase_doc, f_LoadAttest, f_LoadProf_train, f_LoadSpec, f_LoadRise_qual, f_LoadInstit;
        bool f_LoadPostg_study, f_LoadAddress, fLoadPrev_Transfer, f_LoadFP, f_LoadSystem_User,
            f_LoadRelativeChild, f_LoadOld_FIO, f_LoadCompulsary_edu, f_LoadType_rise_qual, 
            f_LoadMed_Inspection, f_LoadYoung_Specialist;

        string per_num;
        bool flagEmp, flagTransfer, flagAdd, fSpecial, fPlace_Service, fMob_Order, fMatter_No_Service, flagSave, flagArchive;
        Address formregistr, formhabit;
        ListEmp listEmp;
        OracleDataTable dtMatRespPerson;
        private static OracleCommand _ocSign_ProfUnion, _ocSign_Young_Spec;
        /// <summary>
        /// Переменная для хранения идентификатора перевода
        /// </summary>
        decimal transfer_id = 0;
        /// <summary>
        /// ФИО сотрудника используется для проверки редактирования фамилии для сохранения изменения ФИО
        /// </summary>
        string last_name, first_name, middle_name;
        /// <summary>
        /// Конструктор формы личной карточки работника
        /// </summary>
        /// <param name="connectionstring">Строка подключения</param>
        /// <param name="_per_num">Табельный номер сотрудника</param>
        /// <param name="_transfer_id">Идентификатор записи в переводах, которую нужно выбрать при редактировании</param>
        /// <param name="_emp">Таблица, содержащая строку данных работника</param>
        /// <param name="_flagEmp">Переменная определяет работает человек на заводе или является претендентом </param> 
        /// <param name="_sing_comb">Признак совместителя </param> 
        /// <param name="_flagTransfer">Флаг добавления данных в переводы</param>
        /// <param name="_flagAdd">Флаг добавления нового работника, не работающего ранее на заводе</param>
        public PersonalCard(string _per_num, decimal _transfer_id, EMP_seq _emp, bool _flagEmp, bool _flagTransfer,
            bool _flagAdd, int _sing_comb, ListEmp _listEmp, bool _flagArchive)
        {
            InitializeComponent();
            per_num = _per_num;
            emp = _emp;
            flagEmp = _flagEmp;
            flagTransfer = _flagTransfer;
            flagAdd = _flagAdd;
            listEmp = _listEmp;
            transfer_id = _transfer_id;
            flagArchive = _flagArchive;
            // Создание и заполнение таблиц данными
            mar_state = new MAR_STATE_seq(Connect.CurConnect);
            mar_state.Fill(string.Format("order by {0}", MAR_STATE_seq.ColumnsName.NAME_STATE));
            source_complect = new SOURCE_COMPLECT_seq(Connect.CurConnect);
            source_complect.Fill(string.Format("order by {0}", SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME));
            source_employability = new SOURCE_EMPLOYABILITY_seq(Connect.CurConnect);
            source_employability.Fill("ORDER BY SOURCE_EMPLOYABILITY_NAME");
            type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
            type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            pension_plan = new PENSION_PLAN_seq(Connect.CurConnect);
            pension_plan.Fill();
            per_data = new PER_DATA_seq(Connect.CurConnect);
            passport = new PASSPORT_seq(Connect.CurConnect);
            registr = new REGISTR_seq(Connect.CurConnect);
            habit = new HABIT_seq(Connect.CurConnect);
            mil_card = new MIL_CARD_seq(Connect.CurConnect);            
            if (flagAdd)
            {
                per_data.AddNew();
                ((PER_DATA_obj)(((CurrencyManager)BindingContext[per_data]).Current)).PER_NUM = per_num;
                passport.AddNew();
                ((PASSPORT_obj)(((CurrencyManager)BindingContext[passport]).Current)).PER_NUM = per_num;
                registr.AddNew();
                ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).PER_NUM = per_num;
                habit.AddNew();
                ((HABIT_obj)(((CurrencyManager)BindingContext[habit]).Current)).PER_NUM = per_num;
                mil_card.AddNew();
                ((MIL_CARD_obj)(((CurrencyManager)BindingContext[mil_card]).Current)).PER_NUM = per_num;
            }
            else
            {
                per_data.Fill(string.Format("where {0} = '{1}'", PER_DATA_seq.ColumnsName.PER_NUM, per_num));
                passport.Fill(string.Format("where {0} = '{1}'", PASSPORT_seq.ColumnsName.PER_NUM, per_num));
                registr.Fill(string.Format("where {0} = '{1}'", REGISTR_seq.ColumnsName.PER_NUM, per_num));
                habit.Fill(string.Format("where {0} = '{1}'", HABIT_seq.ColumnsName.PER_NUM, per_num));
                mil_card.Fill(string.Format("where {0} = '{1}'", MIL_CARD_seq.ColumnsName.PER_NUM, per_num));
            }
            transfer = new TRANSFER_seq(Connect.CurConnect);
            if (flagTransfer)
            {
                transfer.AddNew();
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM = per_num;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID = 1;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SUBDIV_ID = 0;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).POS_ID = 0;
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID =
                    Convert.ToDecimal(new OracleCommand(string.Format("select {0}.worker_id_seq.nextval from dual", Connect.Schema), 
                        Connect.CurConnect).ExecuteScalar());
            }
            else
            {
                transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID, _transfer_id));
                OracleCommand cmd_aud = new OracleCommand(string.Format(
                    "begin APSTAFF.TABLE_AUDIT_EX_INSERT(:p_table_id, :p_type_kard); end;", Connect.Schema), Connect.CurConnect);
                cmd_aud.BindByName = true;
                cmd_aud.Parameters.Add("p_table_id", OracleDbType.Decimal, _transfer_id, ParameterDirection.Input);
                cmd_aud.Parameters.Add("p_type_kard", OracleDbType.Varchar2, "KADR", ParameterDirection.Input);
                cmd_aud.ExecuteNonQuery();
            }                        

            /// Настройка для работы с МОЛ
            dtMatRespPerson = new OracleDataTable("", Connect.CurConnect);
            dtMatRespPerson.SelectCommand.CommandText = string.Format(
                "select * from {0}.MAT_RESP_PERSON where PER_NUM = :p_per_num", Connect.Schema);
            dtMatRespPerson.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            dtMatRespPerson.Fill();
            if (dtMatRespPerson.Rows.Count > 0)
                chSign_MRP.Checked = true;

            // Привязка комбобоксов к данным    
            if (!flagArchive)
            {           
                cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID,
                    new LinkArgument(AppDataSet.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID,
                    new LinkArgument(AppDataSet.position, POSITION_seq.ColumnsName.POS_NAME));
            }
            else
            {
                cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID,
                    new LinkArgument(AppDataSet.allSubdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID,
                    new LinkArgument(AppDataSet.allPosition, POSITION_seq.ColumnsName.POS_NAME));
            }
            cbMar_State_ID.AddBindingSource(passport, MAR_STATE_seq.ColumnsName.MAR_STATE_ID, new LinkArgument(mar_state, MAR_STATE_seq.ColumnsName.NAME_STATE));
            cbMar_State_ID2.AddBindingSource(passport, MAR_STATE_seq.ColumnsName.MAR_STATE_ID, new LinkArgument(mar_state, MAR_STATE_seq.ColumnsName.NAME_STATE));
            cbType_Per_Doc_ID.AddBindingSource(passport, TYPE_PER_DOC_seq.ColumnsName.TYPE_PER_DOC_ID, new LinkArgument(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            /// Если тип документа личности не выбран, то выбираем по умолчанию Паспорт гражданина РФ
            if (((PASSPORT_obj)(((CurrencyManager)BindingContext[passport]).Current)).TYPE_PER_DOC_ID == null)
            {
                ((PASSPORT_obj)(((CurrencyManager)BindingContext[passport]).Current)).TYPE_PER_DOC_ID = 21;
            }
            cbSource_ID.AddBindingSource(transfer, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_ID, new LinkArgument(source_complect, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME));
            cbSource_Employability.AddBindingSource(per_data, SOURCE_EMPLOYABILITY_seq.ColumnsName.SOURCE_EMPLOYABILITY_ID,
                new LinkArgument(source_employability, SOURCE_EMPLOYABILITY_seq.ColumnsName.SOURCE_EMPLOYABILITY_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbType_Per_Doc_ID.SelectedIndexChanged += new EventHandler(cbType_Per_Doc_ID_SelectedIndexChanged);

            if (!flagAdd)
            {
                if (!flagArchive)
                {
                    tbCode_Subdiv.Text = AppDataSet.subdiv.Where(s => s.SUBDIV_ID.ToString() == cbSubdiv_Name.SelectedValue.ToString()).FirstOrDefault().CODE_SUBDIV.ToString();
                    tbCode_Pos.Text = AppDataSet.position.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
                }
                else
                {
                    tbCode_Subdiv.Text = AppDataSet.allSubdiv.Where(s => s.SUBDIV_ID.ToString() == cbSubdiv_Name.SelectedValue.ToString()).FirstOrDefault().CODE_SUBDIV.ToString();
                    tbCode_Pos.Text = AppDataSet.allPosition.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
                }
            }

            // Создание и заполнение таблицы Пол сотрудника
            Sex_Table = new DataTable();
            Sex_Table.Columns.Add("sex_emp", typeof(string));
            Sex_Table.Rows.Add("М");
            Sex_Table.Rows.Add("Ж");
            BindingSource bssex = new BindingSource();
            bssex.DataSource = Sex_Table;

            // Привязка комбобокса Пол сотрудника к данным
            cbSex.DataBindings.Add("selectedvalue", emp, "emp_sex");
            cbSex.DataSource = bssex;
            cbSex.DisplayMember = "sex_emp";
            cbSex.ValueMember = "sex_emp";

            // Привязка тесктбоксов и маскеттекстбоксов к данным
            tbPer_Num.AddBindingSource(emp, EMP_seq.ColumnsName.PER_NUM);
            tbSername.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_LAST_NAME);
            tbName.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_FIRST_NAME);
            tbMiddle_Name.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            mbInsurance_Num.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INSURANCE_NUM);
            mbInn.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INN);
            mbSer_Med_Polus.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.SER_MED_POLUS);
            mbNum_Med_Polus.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.NUM_MED_POLUS);
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
            deBirth_Date.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_BIRTH_DATE);            


            mbDate_HirePlant.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_HIRE);
            if (transfer.Count != 0 && 
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 3)
                mbDate_Dismiss.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);

            // Привязка чекбоксов к данным
            chTrip_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.TRIP_SIGN);
            chRetiree_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.RETIRER_SIGN);        

            //chSign_ProfUnion.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.SIGN_PROFUNION);
            // 12.02.2015 Вычисляем признак профсоюза
            _ocSign_ProfUnion.Parameters["p_WORKER_ID"].Value =
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).WORKER_ID;
            _ocSign_ProfUnion.Parameters["p_DATE"].Value = DateTime.Now;
            chSign_ProfUnion.Checked = Convert.ToBoolean(_ocSign_ProfUnion.ExecuteScalar());    

            //chSign_Young_Spec.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.SIGN_YOUNG_SPEC);
            // 10.11.2015 Вычисляем признак молодого специалиста
            Get_Sign_Young_Spec();

            // Привязка фотографии
            pbPhoto.Image = EmployeePhoto.GetPhoto(per_num);
            /// старые размеры фото (132; 154) и StretchImage

            gbPhoto.EnableByRules();
            tpEdu.EnableByRules();
            tpFamily.EnableByRules();
            tpMil_Card.EnableByRules();
            tpPrev_Work.EnableByRules();
            tpKnow_Lang.EnableByRules();
            tpRewards.EnableByRules();
            tpEmp_Priv.EnableByRules();
            tpAttest.EnableByRules();
            tpProf_Train.EnableByRules();
            tpRise_Qual.EnableByRules();
            tpPostG_Study.EnableByRules();
            tpForeign_Passport.EnableByRules();
            tpVacBabyMinding.EnableByRules();
            tpCompulsory_edu.EnableByRules();
            pnButton.EnableByRules();

            btSave.Enabled = false;
            btOrder.Visible = true;

            // Деактивация всех компонентов формы кроме меток
            gbEmp.DisableAll(false, Color.White);
            tpPer_Data.DisableAll(false, Color.White);
            tpMil_Card.DisableAll(false, Color.White);
            //tpMedPolus.DisableAll(false, Color.White);
            if (!flagArchive && flagAdd)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btEdit_Click(sender, e);
            }

            if (flagArchive)
            {
                if (GrantedRoles.GetGrantedRole("STAFF_EDIT_OLD_EMP"))
                {
                    btEdit.Visible = true;
                    btSave.Visible = true;
                }
                else
                {
                    btEdit.Visible = false;
                    btSave.Visible = false;
                }
                btHireComb.Visible = false;
                btOrder.Visible = false;
                btEditPhoto.Enabled = false;
                DisableButton(tcData);              
            }

            tcData.TabPages.Clear();
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
            {
                btHireComb.Visible = false;
            }            
            // Проверяем грантована ли пользователю роль STAFF_VIEW
            // Если да, то добавляем доступные вкладки
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW"))
            {
                tcData.TabPages.Add(tpPer_Data);
                tcData.TabPages.Add(tpRegistr);
                tcData.TabPages.Add(tpEdu);
                tcData.TabPages.Add(tpFamily);
                tcData.TabPages.Add(tpWork_Length);
                tcData.TabPages.Add(tpPrev_Work);
                tcData.TabPages.Add(tpPrev_Transfer);
                /*btHireComb.Visible = false;
                label80.Visible = false;
                tbHab_Non_Kladr_Address.Visible = false;*/
            }
            if (GrantedRoles.GetGrantedRole("STAFF_AUDIT_OFFICE"))
            {
                if (tcData.TabPages.IndexOf(tpPer_Data) == -1)
                {
                    tcData.TabPages.Add(tpPer_Data);
                }
                //btHireComb.Visible = false;
            }
            // Даем доступ на вкладки Общие и Стаж для о. 43
            if (GrantedRoles.GetGrantedRole("STAFF_MED_INSPECTION"))
            {
                if (tcData.TabPages.IndexOf(tpPer_Data) == -1)
                {
                    tcData.TabPages.Add(tpPer_Data);
                }
                if (tcData.TabPages.IndexOf(tpWork_Length) == -1)
                {
                    tcData.TabPages.Add(tpWork_Length);
                }
                if (tcData.TabPages.IndexOf(tpMed_Inspection) == -1)
                {
                    tcData.TabPages.Add(tpMed_Inspection);
                }
            }
            // Даем доступ на вкладки Семья
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_RELATIVE"))
            {
                if (tcData.TabPages.IndexOf(tpFamily) == -1)
                {
                    tcData.TabPages.Add(tpFamily);
                    tpFamily_Enter(null, null);
                }
            }
            if (GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE") ||
                GrantedRoles.GetGrantedRole("STAFF_MEDICAL") ||
                GrantedRoles.GetGrantedRole("STAFF_MILITARY") ||
                GrantedRoles.GetGrantedRole("STAFF_PENSION") ||
                GrantedRoles.GetGrantedRole("STAFF_PERSONNEL") ||
                GrantedRoles.GetGrantedRole("STAFF_RETRAINING") ||
                GrantedRoles.GetGrantedRole("STAFF_OTHER") ||
                GrantedRoles.GetGrantedRole("STAFF_INSPECTOR"))
            {
                tcData.TabPages.Add(tpMil_Card);
                //tcData.TabPages.Add(tpMedPolus);
                tcData.TabPages.Add(tpKnow_Lang);
                tcData.TabPages.Add(tpRewards);
                tcData.TabPages.Add(tpEmp_Priv);
                tcData.TabPages.Add(tpAttest);
                tcData.TabPages.Add(tpProf_Train);
                tcData.TabPages.Add(tpRise_Qual);
                tcData.TabPages.Add(tpCompulsory_edu);
                tcData.TabPages.Add(tpPostG_Study);
                tcData.TabPages.Add(tpVacBabyMinding);
                tcData.TabPages.Add(tpOld_FIO);
                tcData.TabPages.Add(tpYoung_Specialist);
            }
            if (GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"))
            {
                tcData.TabPages.Add(tpViolation_Emp);
            }
            // Изменения от 19.03.2012            
            // Проверяем условие, что пользователю грантована роль STAFF_FOREIGN_PASS
            if (GrantedRoles.GetGrantedRole("STAFF_FOREIGN_PASS"))
            {
                tcData.TabPages.Add(tpForeign_Passport);
                /*btHireComb.Visible = false;
                btEdit.Visible = false;
                btSave.Visible = false;
                btOrder.Visible = false;*/
                tpForeign_Passport_Enter(null, null);
            }
            // определяем грантована ли пользователю роль STAFF_VIEW_SYSNAME
            // (просмотр имени пользователя сети и Интернет)
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_SYSNAME"))
            {
                tcData.TabPages.Add(tpSystem_User);
            }
            // Проверяем является ли работник пользователем сети
            OracleCommand ocCommon = new OracleCommand("", Connect.CurConnect);
            ocCommon.BindByName = true;
            ocCommon.CommandText = string.Format(
                "select count(*) from {0}.SYSTEM_USER where per_num = :p_per_num",
                DataSourceScheme.SchemeName);
            ocCommon.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            if (Convert.ToInt32(ocCommon.ExecuteScalar()) > 0)
                cbSystem_User.Checked = true;            

            prevTransfer = new TRANSFER_seq(Connect.CurConnect);
            try
            {
                prevTransfer.Fill(string.Format("where {0} = {1} and {2} != to_date('{3}', 'DD.MM.YYYY')",
                    TRANSFER_seq.ColumnsName.PER_NUM,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM,
                    TRANSFER_seq.ColumnsName.DATE_HIRE,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE.Value.ToShortDateString()));
            }
            catch { }
            if (prevTransfer.Count == 0)
            {
                tcData.TabPages.Remove(tpPrev_Transfer);
            }
            /// Изменения от 22.08.2011
            /// Даем доступ пенсионному бюро на редактирование страхового свидетельства
            /// независимо от того работает человек или уволен
            if (GrantedRoles.GetGrantedRole("STAFF_PENSION") || GrantedRoles.GetGrantedRole("STAFF_PRIV_POS_VIEW"))
            {
                btEdit.Visible = true;
                btSave.Visible = true;
                btEditSignPrivPos.Visible = true;
            }
            ocCommon = new OracleCommand("", Connect.CurConnect);
            ocCommon.BindByName = true;
            ocCommon.CommandText = string.Format(Queries.GetQuery("CountReward.sql"), DataSourceScheme.SchemeName);
            ocCommon.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            if (Convert.ToInt32(ocCommon.ExecuteScalar()) > 0)
                cbWorkVeteran.Checked = true;
            tbSername.Focus();
            /// Задаем цвет вкладкам
            foreach (TabPage tabPage in tcData.TabPages)
            {
                tabPage.BackColor = Color.FromArgb(236, 233, 216);
            }

            last_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME;
            first_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME; 
            middle_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME;

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

            ocNonstate_Pension = new OracleCommand("", Connect.CurConnect);
            ocNonstate_Pension.BindByName = true;
            ocNonstate_Pension.CommandText = string.Format(
                @"select PER_NUM, DATE_SETTING_PENS, /*PENSION_PLAN_ID,*/ DATE_OF_ENTRY from {0}.NONSTATE_PENS_PROV where PER_NUM = :p_PER_NUM", Connect.Schema);
            ocNonstate_Pension.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;

            //cbPension_Plan.DataSource = pension_plan;
            //cbPension_Plan.DisplayMember = "PENSION_PLAN_NAME";
            //cbPension_Plan.ValueMember = "PENSION_PLAN_ID";
            //cbPension_Plan.SelectedItem = null;
            OracleDataReader dr = ocNonstate_Pension.ExecuteReader();
            if (dr.Read())
            {
                chSign_NPP.Checked = true;
                if (dr["DATE_SETTING_PENS"] != DBNull.Value)
                    deDate_Setting_Pens.Date = Convert.ToDateTime(dr["DATE_SETTING_PENS"]);
                if (dr["DATE_OF_ENTRY"] != DBNull.Value)
                    deDate_Of_Entry.Date = Convert.ToDateTime(dr["DATE_OF_ENTRY"]);
                //cbPension_Plan.SelectedValue = dr["PENSION_PLAN_ID"];
            }
            if (tcData.TabCount == 0)
                this.Height = 242;
        }

        static PersonalCard()
        {
            _ocSign_ProfUnion = new OracleCommand(string.Format("select {0}.GET_SIGN_PROFUNION(:p_WORKER_ID, :p_DATE) from dual",
                Connect.Schema), Connect.CurConnect);
            _ocSign_ProfUnion.BindByName = true;
            _ocSign_ProfUnion.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal);
            _ocSign_ProfUnion.Parameters.Add("p_DATE", OracleDbType.Date);

            _ocSign_Young_Spec = new OracleCommand(string.Format(
                @"select NVL((SELECT 1 FROM {0}.YOUNG_SPECIALIST YS 
                        WHERE YS.PER_NUM = :p_PER_NUM and 
                            :p_DATE BETWEEN NVL(YS.DATE_BEGIN_ADD, DATE '1000-01-01') and NVL(YS.DATE_END_ADD, DATE '2000-01-01')),0) 
                from dual ",
                Connect.Schema), Connect.CurConnect);
            _ocSign_Young_Spec.BindByName = true;
            _ocSign_Young_Spec.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _ocSign_Young_Spec.Parameters.Add("p_DATE", OracleDbType.Date);
        }

        /// <summary>
        /// Процедура блокирует кнопки добавления и удаления данных в архив, оставляя активными лишь 
        /// кнопки редактирования данных
        /// </summary>
        /// <param name="control"></param>
        void DisableButton(Control control)
        {
            if (control != null)
            {
                if (control.Controls.Count != 0)
                {
                    foreach (Control contr in control.Controls)
                    {
                        if (contr is Button)
                        {
                            if (contr.Name.Substring(0, 5).ToUpper() == "btAdd".ToUpper() || contr.Name.Substring(0, 8).ToUpper() == "btDelete".ToUpper())
                            {
                                contr.Enabled = false;
                            }
                        }
                        else
                            if (contr is Control)
                            {
                                DisableButton(contr);
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                if (Connect.Transact != null && Connect.Transact.Connection != null)                    
                    Connect.Transact.Rollback();
                Close();
                if (!GrantedRoles.GetGrantedRole("STAFF_VIEW"))
                {
                    if (flagAdd && !flagSave)
                    {
                        per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                        per_num_book.Fill(string.Format("where {0} = {1}", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                        ((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = true;
                        per_num_book.Save();
                        Connect.Commit();
                    }
                    try
                    {
                        if (listEmp != null && !flagArchive)
                        {
                            listEmp.bsEmp.PositionChanged -= listEmp.PositionChange;
                            int pos = listEmp.bsEmp.Position;
                            //listEmp.dtEmp.RefreshRow(listEmp.dtEmp.Rows[listEmp.bsEmp.Position]);
                            listEmp.dtEmp.Clear();
                            listEmp.dtEmp.Fill();
                            listEmp.RefreshGridEmp();
                            listEmp.bsEmp.PositionChanged += new EventHandler(listEmp.PositionChange);
                            listEmp.bsEmp.Position = pos;
                            //listEmp.dtEmp.RefreshRow(listEmp.dtEmp.Rows[listEmp.bsEmp.Position]);
                        }
                    }
                    catch
                    {
                        //MessageBox.Show( ex.Message);
                    }
                }
            }
            else
            {
                if (flagAdd)
                {
                    per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                    per_num_book.Fill(string.Format("where {0} = {1}", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                    ((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = true;
                    per_num_book.Save();
                    Connect.Commit();
                }
                if (dtMatRespPerson.Rows.Count > 0)
                    chSign_MRP.Checked = true;
                else
                    chSign_MRP.Checked = false;
                emp.RollBack();
                emp.ResetBindings();
                transfer.RollBack();
                transfer.ResetBindings();
                per_data.RollBack();
                per_data.ResetBindings();
                passport.RollBack();
                passport.ResetBindings();
                registr.RollBack();
                registr.ResetBindings();
                habit.RollBack();
                habit.ResetBindings();
                mil_card.RollBack();
                mil_card.ResetBindings();
                Connect.Rollback();
                //btSave.Enabled = false;
                //btEdit.Enabled = true;
                //btOrder.Enabled = true;
                //btPer_Num.Enabled = true;
                pnButton.EnableByRules();
                btSave.Enabled = false;
                btExit.Text = "Выход";
                gbEmp.DisableAll(false, Color.White);
                tpPer_Data.DisableAll(false, Color.White);
                tpRegistr.DisableAll(false, Color.White);
                tpMil_Card.DisableAll(false, Color.White);
                //tpMedPolus.DisableAll(false, Color.White);
                //tpWork_Length.DisableAll(false, Color.White);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки изменения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btEdit_Click(object sender, EventArgs e)
        {
            btSave.Enabled = true;
            btEdit.Enabled = false;
            btOrder.Enabled = false;
            //btEditForPrivPos.Enabled = false;
            btExit.Text = "Отмена";
            //gbEmp.DisableAll(true, Color.White);
            //tpPer_Data.DisableAll(true, Color.White);
            //tpRegistr.DisableAll(true, Color.White);
            //tpMil_Card.DisableAll(true, Color.White);
            //tpMedPolus.DisableAll(true, Color.White);
            gbPhoto.EnableByRules();
            gbEmp.EnableByRules();
            tpPer_Data.EnableByRules();
            tpRegistr.EnableByRules();
            tpMil_Card.EnableByRules();
            //tpMedPolus.EnableByRules(); 
           
            //tpEdu.EnableByRules(connection);
            //tpFamily.EnableByRules(connection);
            //tpPrev_Work.EnableByRules(connection);
            //tpKnow_Lang.EnableByRules(connection);
            //tpRewards.EnableByRules(connection);
            //tpEmp_Priv.EnableByRules(connection);
            //tpVac.EnableByRules(connection);
            //tpAttest.EnableByRules(connection);
            //tpProf_Train.EnableByRules(connection);
            //tpRise_Qual.EnableByRules(connection);
            //tpPostG_Study.EnableByRules(connection);
            //tpWork_Length.DisableAll(true, Color.White);
            if (flagEmp)
            {
                if (flagTransfer)
                {
                    cbSubdiv_Name.Enabled = true;
                    cbPos_Name.Enabled = true;
                    tbCode_Subdiv.Enabled = true;
                    tbCode_Pos.Enabled = true;
                    tbPer_Num.Enabled = true;
                    gbEmp.EnableByRules();
                }
                else
                {
                    tbPer_Num.Enabled = false;
                    tbCode_Pos.Enabled = false;
                    tbCode_Subdiv.Enabled = false;
                    cbSubdiv_Name.Enabled = false;
                    cbPos_Name.Enabled = false;
                    cbSubdiv_Name.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbPos_Name.DropDownStyle = ComboBoxStyle.DropDownList;
                    gbEmp.EnableByRules();
                }
            }
        }
                
        /// <summary>
        /// Событие нажатия кнопки сохранения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbType_Per_Doc_ID.SelectedValue != null)
            {
                PASSPORT_seq ps = new PASSPORT_seq(Connect.CurConnect);
                ps.Fill(string.Format(" where {0} = '{1}' and {2} = '{3}' and {4} = '{5}' and PER_NUM < '79000'",
                    PASSPORT_seq.ColumnsName.TYPE_PER_DOC_ID, cbType_Per_Doc_ID.SelectedValue,
                    PASSPORT_seq.ColumnsName.SERIA_PASSPORT, mbSeria_Passport.Text,
                    PASSPORT_seq.ColumnsName.NUM_PASSPORT, mbNum_Passport.Text));
                if (ps.Count != 0 && ((PASSPORT_obj)(((CurrencyManager)BindingContext[ps]).Current)).PER_NUM != per_num
                    && ((PASSPORT_obj)(((CurrencyManager)BindingContext[ps]).Current)).PER_NUM.Substring(0,1) != "9")
                {
                    MessageBox.Show("Работник с введенными данными документа личности\nсуществует в базе данных работников!\nВведите другие данные.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbType_Per_Doc_ID.Focus();
                    return;
                }
            }
            if (tbSername.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели фамилию работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbSername.Focus();
                return;
            }
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели имя работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbName.Focus();
                return;
            }
            //if (tbMiddle_Name.Text == "")
            //{
            //    MessageBox.Show("Вы не ввели отчество работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.tbMiddle_Name.Focus();
            //    return;
            //}
            if (cbSex.Text.Trim() == "")
            {
                MessageBox.Show("Вы не выбрали пол работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cbSex.Focus();
                return;
            }
            if (deBirth_Date.Text == null || deBirth_Date.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("А кто будет вводить дату рождения?!", "АСУ \"Кадры\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.deBirth_Date.Focus();
                return;
            }
            else
                if (deBirth_Date.Date.Value.Year == DateTime.Now.Year)
                {
                    MessageBox.Show("А год даты рождения равен текущему году?!", "АСУ \"Кадры\"", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.deBirth_Date.Focus();
                    return;
                }
            if (tbPer_Num.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели табельный номер. Как это понимать?!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbPer_Num.Focus();
                return;
            } 
            if (cbPos_Name.SelectedValue == null || (cbPos_Name.SelectedValue.ToString() == "0" && 
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).HIRE_SIGN == true))
            {
                MessageBox.Show("Вы не ввели должность!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbCode_Pos.Focus();
                return;
            }
            if (cbSubdiv_Name.SelectedValue == null || (cbSubdiv_Name.SelectedValue.ToString() == "0" &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).HIRE_SIGN == true))
            {
                MessageBox.Show("Вы не ввели подразделение!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbCode_Subdiv.Focus();
                return;
            }
            //PER_NUM_BOOK_seq per_num_book = new PER_NUM_BOOK_seq(connection);
            //per_num_book.Fill(string.Format("where {0} = {1}", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
            //((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = false;
            //per_num_book.Save();
            EMP_seq empTemp = new EMP_seq(Connect.CurConnect);
            empTemp.Fill(string.Format("where EMP_LAST_NAME = '{0}' and EMP_FIRST_NAME = '{1}' and EMP_MIDDLE_NAME = '{2}' and " +
                "EMP_BIRTH_DATE = to_date('{3}', 'DD.MM.YYYY') and PER_NUM != '{4}' and PER_NUM < '79000'",
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).EMP_LAST_NAME,
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).EMP_FIRST_NAME,
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).EMP_MIDDLE_NAME,
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).EMP_BIRTH_DATE.Value.ToShortDateString(),
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM));
            if (empTemp.Count != 0)
            {
                MessageBox.Show("Работник с введенными ФИО и датой рождения уже есть в базе данных!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbSername.Focus();
                return;
            }
            bool flagEdit = false;
            if (emp.IsDataChanged() || transfer.IsDataChanged())
                flagEdit = true;
            flagSave = true;
            // 16.10.2012 - Создал эту переменную для явного определения изменения данных по МОЛ
            bool fl_EditMol = false;
            /// Если человек является МОЛ
            if (chSign_MRP.Checked)
            {
                /// Если его нет в таблице МОЛ
                if (dtMatRespPerson.Rows.Count == 0)
                {
                    fl_EditMol = true;
                    /// Добавляем новую запись для этого человека
                    MAT_RESP_PERSON_seq mat_resp_person = new MAT_RESP_PERSON_seq(Connect.CurConnect);
                    mat_resp_person.AddNew();
                    ((MAT_RESP_PERSON_obj)(((CurrencyManager)BindingContext[mat_resp_person]).Current)).PER_NUM = per_num;
                    mat_resp_person.Save();
                }
            }
            /// Если человек не является МОЛ
            else
            {
                /// Если он был в таблице МОЛ
                if (dtMatRespPerson.Rows.Count > 0)
                {
                    /// Удаляем запись
                    OracleCommand ocDeleteMat_Resp_Person = new OracleCommand("", Connect.CurConnect);
                    ocDeleteMat_Resp_Person.BindByName = true;
                    ocDeleteMat_Resp_Person.CommandText = string.Format(
                        "delete from {0}.MAT_RESP_PERSON where PER_NUM = :p_per_num",
                        Connect.Schema);
                    ocDeleteMat_Resp_Person.Parameters.Add("p_per_num", OracleDbType.Decimal).Value =
                        per_num;
                    ocDeleteMat_Resp_Person.ExecuteNonQuery();
                    fl_EditMol = true;
                }
            }
            if (!flagAdd && 
                (last_name != ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME ||
                first_name != ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME ||
                middle_name != ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME))
            {
                OracleCommand ocAddOldName = new OracleCommand(
                    string.Format("insert into {0}.EMP_OLD_NAME(PER_NUM,OLD_LAST_NAME,OLD_FIRST_NAME,OLD_MIDDLE_NAME) " +
                    "VALUES(:p_per_num,:p_old_last_name,:p_old_first_name,:p_old_middle_name)", Connect.Schema), Connect.CurConnect);
                ocAddOldName.BindByName = true;
                ocAddOldName.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
                ocAddOldName.Parameters.Add("p_old_last_name", OracleDbType.Varchar2).Value = last_name;
                ocAddOldName.Parameters.Add("p_old_first_name", OracleDbType.Varchar2).Value = first_name;
                ocAddOldName.Parameters.Add("p_old_middle_name", OracleDbType.Varchar2).Value = middle_name;
                ocAddOldName.ExecuteNonQuery();
                last_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME;
                first_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME;
                middle_name = ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME;
            }
            emp.Save();
            transfer.Save();            
            per_data.Save();
            passport.Save();
            registr.Save();
            habit.Save();
            mil_card.Save();
            if (GrantedRoles.GetGrantedRole("STAFF_PENSION"))
            {
                ocUpdateNPP = new OracleCommand("", Connect.CurConnect);
                ocUpdateNPP.BindByName = true;
                ocUpdateNPP.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                if (chSign_NPP.Checked)
                {
                    ocUpdateNPP.CommandText = string.Format(
                        @"merge into {0}.NONSTATE_PENS_PROV NPP
                        using (select :p_PER_NUM PER_NUM, :p_DATE_SETTING_PENS DATE_SETTING_PENS, :p_DATE_OF_ENTRY DATE_OF_ENTRY/*, :p_PENSION_PLAN_ID PENSION_PLAN_ID*/ from dual) N_NPP
                            on (NPP.PER_NUM = N_NPP.PER_NUM)
                        when matched then
                            update set DATE_SETTING_PENS = :p_DATE_SETTING_PENS, DATE_OF_ENTRY = :p_DATE_OF_ENTRY/*, PENSION_PLAN_ID = :p_PENSION_PLAN_ID*/
                        when not matched then
                            insert(PER_NUM, DATE_SETTING_PENS, DATE_OF_ENTRY/*, PENSION_PLAN_ID*/)
                            values(N_NPP.PER_NUM, N_NPP.DATE_SETTING_PENS, N_NPP.DATE_OF_ENTRY/*, N_NPP.PENSION_PLAN_ID*/)", Connect.Schema);
                    ocUpdateNPP.Parameters.Add("p_DATE_SETTING_PENS", OracleDbType.Date).Value = deDate_Setting_Pens.Date;
                    ocUpdateNPP.Parameters.Add("p_DATE_OF_ENTRY", OracleDbType.Date).Value = deDate_Of_Entry.Date;
                    //ocUpdateNPP.Parameters.Add("p_PENSION_PLAN_ID", OracleDbType.Decimal).Value = cbPension_Plan.SelectedValue;
                }
                else
                {
                    ocUpdateNPP.CommandText = string.Format(
                        @"delete from {0}.NONSTATE_PENS_PROV NPP where PER_NUM = :p_PER_NUM", Connect.Schema);
                    deDate_Setting_Pens.Date = null;
                    deDate_Of_Entry.Date = null;
                    //cbPension_Plan.SelectedItem = null;
                }
                ocUpdateNPP.ExecuteNonQuery();
            }
            if (Connect.Transact != null && Connect.Transact.Connection != null)
                Connect.Transact.Commit();
            Connect.Commit();
            /*Проверяем редактировались ли данные по сотруднику.
             fl_EditMol - определяет что редактировали МОЛ, значит редактировать данные в 
             справочнике работающих не нужно.*/
            if (flagEdit && !fl_EditMol)
            {
                if (((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PERCO_SYNC_ID != null
                    /*&& ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).HIRE_SIGN == true*/)
                {
                    EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                    TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                    transfer_id = (int)r_transferNew.TRANSFER_ID;
                    DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                        r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                        r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()));
                }
                try
                {
                    OdbcConnection odbcCon = new OdbcConnection("");
                    odbcCon.ConnectionString = string.Format(
                        @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                        ParVal.Vals["SPR"]);
                    odbcCon.Open();
                    OdbcCommand _rezult = new OdbcCommand("", odbcCon);
                    _rezult.CommandText = string.Format(
                        "update SPR set FIO = iif(empty(dat_uv),'{1}',padr('{1}',21,' ')+'*'), POL = '{2}', DAT_ROG = {3}, " +
                        "FAM = '{4}', NAM = '{5}', OTC = '{6}' " +
                        "where tnom = '{0}'",
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PER_NUM,
                        string.Format("{0} {1} {2}",
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME,
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME.Substring(0, 1),
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME.Substring(0, 1)),
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_SEX == "М" ? "1" : "2",
                         "{^" + ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_BIRTH_DATE.Value.Year.ToString() + "-" +
                            ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_BIRTH_DATE.Value.Month.ToString().PadLeft(2, '0') + "-" +
                            ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_BIRTH_DATE.Value.Day.ToString().PadLeft(2, '0') + "}",
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME,
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME,
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME);
                    _rezult.ExecuteNonQuery();
                    odbcCon.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Нет доступа на справочник работающих!"+
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }       
            //btEdit.Enabled = true;
            //btOrder.Enabled = true;
            pnButton.EnableByRules();
            btSave.Enabled = false;
            //btPer_Num.Enabled = true;
            btExit.Text = "Выход";
            gbPhoto.DisableAll(false, Color.White);
            gbEmp.DisableAll(false, Color.White);
            tpPer_Data.DisableAll(false, Color.White);
            tpRegistr.DisableAll(false, Color.White);
            tpMil_Card.DisableAll(false, Color.White);
            //tpMedPolus.DisableAll(false, Color.White);
         }

        void Get_Sign_Young_Spec()
        {
            _ocSign_Young_Spec.Parameters["p_PER_NUM"].Value =
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM;
            _ocSign_Young_Spec.Parameters["p_DATE"].Value = DateTime.Now;
            chSign_Young_Spec.Checked = Convert.ToBoolean(_ocSign_Young_Spec.ExecuteScalar());
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
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.EnabledComboBox);
                formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.cbRegion_SelectedIndexChanged);
                formregistr.tbHouse.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_HOUSE);
                formregistr.tbBulk.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_BULK);
                formregistr.tbFlat.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_FLAT);
                formregistr.tbPhone.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_PHONE);
                formregistr.tbPost_Code.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_POST_CODE);
                //formregistr.mbDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                formregistr.deDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
                Button bt = new Button();
                bt.Name = "btFromRegistrToHabit";
                bt.Location = new Point(24, 283);
                bt.Size = new Size(335, 27);
                bt.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                bt.Text = "Скопировать в адрес проживания";
                bt.ForeColor = Color.FromArgb(0, 70, 213);
                bt.Enabled = false;
                bt.Click += new EventHandler(bt_Click);
                formregistr.Controls.Add(bt);
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
                if (btExit.Text == "Выход")
                {
                    tpRegistr.DisableAll(false, Color.White);
                }
                else
                {                    
                    tpRegistr.DisableAll(true, Color.White);
                    tpRegistr.EnableByRules();
                }
            }
            
        }

        /// <summary>
        /// Событие нажатия кнопки копирования адреса прописки в адрес проживания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_Click(object sender, EventArgs e)
        {
            if (formregistr.cbStreet.SelectedValue == null)
            {
                MessageBox.Show("Вы не ввели адрес прописки для копирования", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите скопировать адрес прописки\nв адрес проживания?\nЭто займет некоторое время.", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string st = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_CODE_STREET;
                    ((HABIT_obj)(((CurrencyManager)BindingContext[habit]).Current)).HAB_CODE_STREET = st;
                    formhabit.LoadAddress(st);
                    formhabit.tbHouse.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_HOUSE;
                    formhabit.tbBulk.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_BULK;
                    formhabit.tbFlat.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_FLAT;
                    formhabit.tbPost_Code.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_POST_CODE;
                    formhabit.tbPhone.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_PHONE;
                    formhabit.tbHouse.Enabled = true;
                    formhabit.tbBulk.Enabled = true;
                    formhabit.tbFlat.Enabled = true;
                    formhabit.tbPost_Code.Enabled = true;
                    formhabit.tbPhone.Enabled = true;
                }
            }
        }

        #endregion 

        #region Работа с таблицей Образование

        // Активация вкладки Образование
        private void tpEdu_Enter(object sender, EventArgs e)
        {
            if (!f_LoadEdu)
            {
                // Инициализация таблиц и заполнение их данными
                if (!f_LoadSpec)
                {
                    speciality = new SPECIALITY_seq(Connect.CurConnect);
                    speciality.Fill(string.Format("order by {0}", SPECIALITY_seq.ColumnsName.NAME_SPEC));
                    f_LoadSpec = true;
                }
                instit = new INSTIT_seq(Connect.CurConnect);
                instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
                qual = new QUAL_seq(Connect.CurConnect);
                qual.Fill(string.Format("order by {0}", QUAL_seq.ColumnsName.QUAL_NAME));
                group_spec = new GROUP_SPEC_seq(Connect.CurConnect);
                group_spec.Fill(string.Format("order by {0}", GROUP_SPEC_seq.ColumnsName.GS_NAME));
                type_study = new TYPE_STUDY_seq(Connect.CurConnect);
                type_study.Fill(string.Format("order by {0}", TYPE_STUDY_seq.ColumnsName.TS_NAME));
                type_edu = new TYPE_EDU_seq(Connect.CurConnect);
                type_edu.Fill(string.Format("order by {0}", TYPE_EDU_seq.ColumnsName.TE_NAME));
                edu = new EDU_seq(Connect.CurConnect);
                edu.Fill(string.Format("where per_num = '{0}'",per_num));

                dgEdu.AddBindingSource(edu, new LinkArgument(speciality, SPECIALITY_seq.ColumnsName.NAME_SPEC), new LinkArgument(instit, INSTIT_seq.ColumnsName.INSTIT_NAME), 
                    new LinkArgument(type_study, TYPE_STUDY_seq.ColumnsName.TS_NAME), new LinkArgument(type_edu, TYPE_EDU_seq.ColumnsName.TE_NAME), 
                    new LinkArgument(qual, QUAL_seq.ColumnsName.QUAL_NAME), new LinkArgument(group_spec, GROUP_SPEC_seq.ColumnsName.GS_NAME));
                dgEdu.Invalidate();
                dgEdu.Columns["per_num"].Visible = false;
                f_LoadEdu = true;
            }
            //btDeleteEdu.Enabled = true;
        }

        // Добавление данных об образовании работника
        private void btAddEdu_Click(object sender, EventArgs e)
        {
            f_AddEdu = true;            
            formedu = new Edu(per_num, f_AddEdu, BindingContext[edu].Position, edu, speciality, instit, type_study, type_edu, qual, group_spec, dgEdu, pnVisible, flagArchive);
            formedu.Text = "Добавление данных об образовании";
            formedu.ShowDialog();            
        }
                
        // Редактирование данных об образовании работника
        private void btEditEdu_Click(object sender, EventArgs e)
        {
            if (dgEdu.RowCount == 0)
                return;
            f_AddEdu = false;
            formedu = new Edu(per_num, f_AddEdu, BindingContext[edu].Position, edu, speciality, instit, type_study, type_edu, qual, group_spec, dgEdu, pnVisible, flagArchive);
            formedu.Text = "Редактирование данных об образовании";
            formedu.ShowDialog();
        }

        // Удаление данных об образовании работника
        private void btDeleteEdu_Click(object sender, EventArgs e)
        {
            if (dgEdu.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgEdu.Rows.RemoveAt(((CurrencyManager)BindingContext[edu]).Position);
                edu.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Семья

        // Активация вкладки Семья 
        private void tpFamily_Enter(object sender, EventArgs e)
        {
            if (!f_LoadRelative)
            {
                // Инициализация таблиц и заполнение их данными                
                rel_type = new REL_TYPE_seq(Connect.CurConnect);
                rel_type.Fill(string.Format("order by {0}", REL_TYPE_seq.ColumnsName.NAME_REL));
                relative = new RELATIVE_seq(Connect.CurConnect);
                relative.Fill(string.Format("where per_num = '{0}'", per_num));

                dgFamily.AddBindingSource(relative, new LinkArgument(rel_type, "name_rel"));
                dgFamily.Invalidate();
                dgFamily.Columns["REL_LAST_NAME"].DisplayIndex = 1;
                dgFamily.Columns["REL_FIRST_NAME"].DisplayIndex = 2;
                dgFamily.Columns["REL_MIDDLE_NAME"].DisplayIndex = 3;
                dgFamily.Columns["REL_BIRTH_DATE"].DisplayIndex = 4;
                dgFamily.Columns["REL_BIRTH_YEAR"].DisplayIndex = 5;
                dgFamily.Columns["per_num"].Visible = false;
                dgFamily.Columns["REL_BIRTH_CERTIFICATE_SER"].Visible = false;
                dgFamily.Columns["REL_BIRTH_CERTIFICATE_NUM"].Visible = false;
                dgFamily.Columns["per_num"].Visible = false;
                dgFamily.Columns["rel_per_num"].Visible = false;
                f_LoadRelative = true;
            }
            else
            {
                relative.Clear();
                relative.Fill(string.Format("where per_num = '{0}'", per_num));
            }

        }

        // Добавление данных о семье работника
        private void btAddFamily_Click(object sender, EventArgs e)
        {
            f_AddRelative = true;
            formfamily = new Family(per_num, f_AddRelative, BindingContext[relative].Position, relative, rel_type, dgFamily, pnVisible, emp, flagArchive);
            formfamily.Text = "Добавление данных о семье";
            formfamily.ShowDialog();    
        }

        // Редактирование данных о семье работника
        private void btEditFamily_Click(object sender, EventArgs e)
        {
            if (dgFamily.RowCount == 0)
                return;
            f_AddRelative = false;
            formfamily = new Family(per_num, f_AddRelative, BindingContext[relative].Position, relative, rel_type, dgFamily, pnVisible, emp, flagArchive);
            formfamily.Text = "Редактирование данных о семье";
            formfamily.ShowDialog();
        }

        // Уданение данных о семье работника
        private void btDeleteFamily_Click(object sender, EventArgs e)
        {
            if (dgFamily.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CHILD_REARING_LEAVE_seq crl = new CHILD_REARING_LEAVE_seq(Connect.CurConnect);
                crl.Fill(string.Format("where relative_id = {0}",
                    ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).RELATIVE_ID));
                if (crl.Count > 0)
                {
                    MessageBox.Show("Невозможно удалить запись!\n" +
                        "Необходимо удалить запись из вкладки \"Отпуск по уходу за ребенком\".",
                        "АРМ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgFamily.Rows.RemoveAt(((CurrencyManager)BindingContext[relative]).Position);
                relative.Save();
                Connect.Commit();
            }
        }
        # endregion 

        #region Работа с таблицей Воинский учет

        private void tpMil_Card_Enter(object sender, EventArgs e)
        {
            if (!f_LoadMil_Card)
            {
                // Инициализация таблиц и заполнение их данными
                mil_rank = new MIL_RANK_seq(Connect.CurConnect);
                mil_rank.Fill(string.Format("order by {0}", MIL_RANK_seq.ColumnsName.NAME_MIL_RANK));
                med_classif = new MED_CLASSIF_seq(Connect.CurConnect);
                med_classif.Fill(string.Format("order by {0}", MED_CLASSIF_seq.ColumnsName.NAME_MED_CLASSIF));
                comm = new COMM_seq(Connect.CurConnect);
                comm.Fill(string.Format("order by {0}", COMM_seq.ColumnsName.COMM_NAME));
                type_troop = new TYPE_TROOP_seq(Connect.CurConnect);
                type_troop.Fill(string.Format("order by {0}", TYPE_TROOP_seq.ColumnsName.NAME_TYPE));
                mil_cat = new MIL_CAT_seq(Connect.CurConnect);
                mil_cat.Fill(string.Format("order by {0}", MIL_CAT_seq.ColumnsName.MIL_CAT_NAME));

                // Привязка компонентов
                cbMil_Rank_ID.AddBindingSource(mil_card, MIL_RANK_seq.ColumnsName.MIL_RANK_ID, new LinkArgument(mil_rank, MIL_RANK_seq.ColumnsName.NAME_MIL_RANK));
                cbMed_Classif_ID.AddBindingSource(mil_card, MED_CLASSIF_seq.ColumnsName.MED_CLASSIF_ID, new LinkArgument(med_classif, MED_CLASSIF_seq.ColumnsName.NAME_MED_CLASSIF));
                cbComm_ID.AddBindingSource(mil_card, COMM_seq.ColumnsName.COMM_ID, new LinkArgument(comm, COMM_seq.ColumnsName.COMM_NAME));                
                cbMil_Cat_ID.AddBindingSource(mil_card, MIL_CAT_seq.ColumnsName.MIL_CAT_ID, new LinkArgument(mil_cat, MIL_CAT_seq.ColumnsName.MIL_CAT_NAME));
                tbName_Mil_Spec.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.NAME_MIL_SPEC);
                tbNum_Doc_Recruit.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.NUM_DOC_RECRUIT);
                tbRes_Cat.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.RES_CAT);
                tbMil_Group.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.MIL_GROUP);
                tbMatter_Remove.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.MATTER_REMOVE);

                // Привязка ричтекстбоксов к полям таблиц
                rtSpecial.DataBindings.Add("text", mil_card, MIL_CARD_seq.ColumnsName.SPECIAL_REG.ToString(), true, DataSourceUpdateMode.OnPropertyChanged, "");
                rtMob_Order.DataBindings.Add("text", mil_card, MIL_CARD_seq.ColumnsName.MOB_ORDER.ToString(), true, DataSourceUpdateMode.OnPropertyChanged, "");
                rtPlace_Service.DataBindings.Add("text", mil_card, MIL_CARD_seq.ColumnsName.PLACE_SERVICE.ToString(), true, DataSourceUpdateMode.OnPropertyChanged, "");
                rtMatter_No_Service.DataBindings.Add("text", mil_card, MIL_CARD_seq.ColumnsName.MATTER_NO_SERVICE.ToString(), true, DataSourceUpdateMode.OnPropertyChanged, "");

                // Привязка чекбоксов к полям таблиц
                chRecruit_Sign.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.RECRUIT_SIGN);

                // Привязка дат
                //mbData_Mob.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_MOB);
                //mbDate_Demob.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_DEMOB);
                //mbDate_Post_Fact.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_POST_FACT);
                //mbDate_Remove.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_REMOVE);
                //mbDate_Post_Comm.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_GET_MIL_CARD);    
                deData_Mob.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_MOB);
                deDate_Demob.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_DEMOB);
                deDate_Post_Fact.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_POST_FACT);
                deDate_Remove.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_REMOVE);
                deDate_Post_Comm.AddBindingSource(mil_card, MIL_CARD_seq.ColumnsName.DATE_GET_MIL_CARD);   

                if (((CurrencyManager)BindingContext[mil_card]).Position == -1)
                {
                    r_mil_card = mil_card.AddNew();
                    r_mil_card.PER_NUM = per_num;
                }
                else
                {
                    r_mil_card = (MIL_CARD_obj)((CurrencyManager)BindingContext[mil_card]).Current;                    
                }
                f_LoadMil_Card = true;
                //rtSpecial.Enter += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.EnterRichTextBox(rtSpecial);
                //});
                //rtPlace_Service.Enter += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.EnterRichTextBox(rtPlace_Service);
                //});
                //rtMob_Order.Enter += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.EnterRichTextBox(rtMob_Order);
                //});
                //rtMatter_No_Service.Enter += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.EnterRichTextBox(rtMatter_No_Service);
                //});
                //rtSpecial.Leave += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    RichTextBoxClick(rtSpecial, ref fSpecial);
                //});
                //rtPlace_Service.Leave += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.LeaveRichTextBox(rtPlace_Service);
                //});
                //rtMob_Order.Leave += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    RichTextBoxClick(rtMob_Order, ref fMob_Order);
                //});
                //rtMatter_No_Service.Leave += new EventHandler(delegate(object sender1, EventArgs e1)
                //{
                //    Library.LeaveRichTextBox(rtMatter_No_Service);
                //});

                //foreach (Control control in tpMil_Card.Controls)
                //{
                //    if (control is MaskedTextBox)
                //    { 
                //        ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                //    }
                //}
                
            }      
        }

        private void tpMil_Card_Leave(object sender, EventArgs e)
        {
            //if (mil_card.IsDataChanged())
            //{
            //    Type type = typeof(MIL_CARD_obj);
            //    PropertyInfo[] properties = type.GetProperties();
            //    foreach (PropertyInfo pr in properties)
            //    {
            //        object rezult = type.InvokeMember(pr.Name, BindingFlags.GetField | BindingFlags.GetProperty, null, r_mil_card, null);
            //        if (rezult == null)
            //            f_AddMil_card = false;
            //        else if ((pr.Name != "PER_NUM") && (pr.Name != "RECRUIT_SIGN") && (pr.Name != "MIL_STATE"))
            //        {
            //            f_AddMil_card = true;
            //            break;
            //        }
            //    }
            //    if (f_AddMil_card)
            //    {
            //        ((CurrencyManager)BindingContext[mil_card]).EndCurrentEdit();
            //        mil_card.Save();
            //    }           
            //}            
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
                    TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, tbPer_Num.Text.Trim(),
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
                mbDate_Com.Text = startFirstWork.ToShortDateString();
                mbDate_Com_Fact.Text = startFirstWorkFact.ToShortDateString();

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
                    /*mbDate_Com.Text = startFirstWork.ToShortDateString();
                    mbDate_Com_Fact.Text = startFirstWorkFact.ToShortDateString();*/
                    OracleDataReader odreader = ocStart_Work.ExecuteReader();
                    if (odreader.Read())
                    {
                        startFirstWork = Convert.ToDateTime(odreader["START_WORK"]);
                        mbDate_Com.Text = startFirstWork.ToShortDateString();
                        if (odreader["START_WORK_PLANT"] == DBNull.Value)
                        {
                            mbDate_Com_Fact.Text = dateHire.Value.ToShortDateString();
                        }
                        else
                        {
                            mbDate_Com_Fact.Text = odreader["START_WORK_PLANT"].ToString();
                        }
                    }
                    else
                    {
                        startFirstWork = (DateTime)dateHire;
                        mbDate_Com.Text = dateHire.Value.ToShortDateString();
                        mbDate_Com_Fact.Text = dateHire.Value.ToShortDateString();
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

                    // Дата окончания расчета стажа: если уволенный - дата увольнения, иначе - текущая дата
                    DateTime? _date_end_calc;
                    if (transfer.Count != 0 &&
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 3)
                        _date_end_calc = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER;
                    else
                        _date_end_calc = DateTime.Now;
                    // Общий на текущий момент
                    ocStanding.Parameters["p_date_end_calc"].Value = _date_end_calc;
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
                    ocStanding.Parameters["p_date_end_calc"].Value = _date_end_calc;
                    ocStanding.ExecuteNonQuery();
                    tbC_YNZ.Text = ocStanding.Parameters["p_years"].Value.ToString();
                    tbC_MNZ.Text = ocStanding.Parameters["p_months"].Value.ToString();
                    tbC_DNZ.Text = ocStanding.Parameters["p_days"].Value.ToString();

                    // Непрерывный на текущий момент
                    ocStanding.Parameters["p_date_end_calc"].Value = _date_end_calc;
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
                    ocStanding.Parameters["p_date_end_calc"].Value = _date_end_calc;
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
                    /*((CurrencyManager)BindingContext[prev_work]).Position = i;
                    stagYear = stagMonth = stagDay = 0;
                    try
                    {
                        Library.CalculationWork_Length(
                            (DateTime)((PREV_WORK_obj)(((CurrencyManager)BindingContext[prev_work]).Current)).PW_DATE_START,
                            (DateTime)((PREV_WORK_obj)(((CurrencyManager)BindingContext[prev_work]).Current)).PW_DATE_END,
                            ref stagYear, ref stagMonth, ref stagDay);
                    }
                    catch { }
                    dgPrev_Work.Rows[i].Cells["stagYear"].Value = stagYear.ToString();
                    dgPrev_Work.Rows[i].Cells["stagMonth"].Value = stagMonth.ToString();
                    dgPrev_Work.Rows[i].Cells["stagDay"].Value = stagDay.ToString();*/
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

        // Добавление данных о предыдущей деятельности
        private void btAddPrev_Work_Click(object sender, EventArgs e)
        {
            f_AddPrev_work = true;
            formprev_work = new Prev_Work(per_num, f_AddPrev_work, BindingContext[prev_work].Position, 
                prev_work, AppDataSet.position, dgPrev_Work, pnVisible,
                (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE, flagArchive);
            formprev_work.Text = "Добавление данных о предыдущей деятельности";
            formprev_work.ShowDialog();
            prev_work.Clear();
            prev_work.Fill(string.Format("where {0} = '{1}' order by {2}",
                    PREV_WORK_seq.ColumnsName.PER_NUM, per_num, PREV_WORK_seq.ColumnsName.PW_DATE_START));
            flagPrev_Work = true;
        }

        // Редактирование данных о предыдущей деятельности
        private void btEditPrev_Work_Click(object sender, EventArgs e)
        {
            if (dgPrev_Work.RowCount == 0)
                return;
            f_AddPrev_work = false;
            formprev_work = new Prev_Work(per_num, f_AddPrev_work, BindingContext[prev_work].Position,
                prev_work, AppDataSet.position, dgPrev_Work, pnVisible,
                (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE, flagArchive);
            formprev_work.Text = "Редактирование данных о предыдущей деятельности";
            formprev_work.ShowDialog();
            prev_work.Clear();
            prev_work.Fill(string.Format("where {0} = '{1}' order by {2}",
                    PREV_WORK_seq.ColumnsName.PER_NUM, per_num, PREV_WORK_seq.ColumnsName.PW_DATE_START));
            flagPrev_Work = true;
        }

        // Уданение данных о предыдущей деятельности
        private void btDeletePrev_Work_Click(object sender, EventArgs e)
        {
            if (dgPrev_Work.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgPrev_Work.Rows.RemoveAt(((CurrencyManager)BindingContext[prev_work]).Position);
                prev_work.Save();
                Connect.Commit();                
            }
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

        #region Работа с переводами прошлых лет
        
        /// <summary>
        /// Активация вкладки Предыдущие переводы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpPrev_Transfer_Enter(object sender, EventArgs e)
        {
            if (!fLoadPrev_Transfer)
            {
                string sql = string.Format(Queries.GetQuery("SelectOldTransfer.sql"), Staff.DataSourceScheme.SchemeName,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM,
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE.Value.ToShortDateString());
                OracleDataTable dtOldTransfer = new OracleDataTable(sql, Connect.CurConnect);
                //dtOldTransfer.MissingSchemaAction = MissingSchemaAction.Add;
                dtOldTransfer.Fill();
                dgPrev_Transfer.AddBindingSource(dtOldTransfer);
                dgPrev_Transfer.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
                dgPrev_Transfer.Columns["SIGN_COMB"].HeaderText = "Совм.";
                dgPrev_Transfer.Columns["DISMISS"].HeaderText = "Увол.";
                dgPrev_Transfer.Columns["POS_NAME"].HeaderText = "Наименовение должности";
                dgPrev_Transfer.Columns["DATE_TRANSFER"].HeaderText = "Дата движения";
                dgPrev_Transfer.Columns["TR_NUM_ORDER"].HeaderText = "Номер приказа";
                dgPrev_Transfer.Columns["TR_DATE_ORDER"].HeaderText = "Дата приказа";
                dgPrev_Transfer.Columns["REASON_NAME"].HeaderText = "Причина увольнения";
                dgPrev_Transfer.Columns["CODE_SUBDIV"].Width = 50;
                dgPrev_Transfer.Columns["SIGN_COMB"].Width = 50;
                dgPrev_Transfer.Columns["DISMISS"].Width = 50;
                dgPrev_Transfer.Columns["POS_NAME"].Width = 350;
                dgPrev_Transfer.Columns["DATE_TRANSFER"].Width = 80;
                dgPrev_Transfer.Columns["TR_NUM_ORDER"].Width = 70;
                dgPrev_Transfer.Columns["TR_DATE_ORDER"].Width = 80;
                dgPrev_Transfer.Columns["REASON_NAME"].Width = 300;
                fLoadPrev_Transfer = true;
            }
        }

        /// <summary>
        /// Добавление старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddPrev_Transfer_Click(object sender, EventArgs e)
        {
            //fAddPrevTransfer = true;
            //formprev_work = new Prev_Work(connection, per_num, f_AddPrev_work, BindingContext[prev_work].Position,
            //    prev_work, FormMain.position, dgPrev_Work, pnVisible,
            //    (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE);
            //formprev_work.Text = "Добавление данных о предыдущей деятельности";
            //formprev_work.ShowDialog();
            //prev_work.Clear();
            //prev_work.Fill(string.Format("where {0} = '{1}' order by {2}",
            //        PREV_WORK_seq.ColumnsName.PER_NUM, per_num, PREV_WORK_seq.ColumnsName.PW_DATE_START));
            //flagPrev_Work = true;
        }

        /// <summary>
        /// Редактирование старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditPrev_Transfer_Click(object sender, EventArgs e)
        {
            //if (dgPrev_Transfer.RowCount == 0)
            //    return;
            //f_AddPrev_work = false;
            //formprev_work = new Prev_Work(connection, per_num, f_AddPrev_work, BindingContext[prev_work].Position,
            //    prev_work, FormMain.position, dgPrev_Work, pnVisible,
            //    (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE);
            //formprev_work.Text = "Редактирование данных о предыдущей деятельности";
            //formprev_work.ShowDialog();
            //prev_work.Clear();
            //prev_work.Fill(string.Format("where {0} = '{1}' order by {2}",
            //        PREV_WORK_seq.ColumnsName.PER_NUM, per_num, PREV_WORK_seq.ColumnsName.PW_DATE_START));
            //flagPrev_Work = true;
        }

        /// <summary>
        /// Удаление старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeletePrev_Transfer_Click(object sender, EventArgs e)
        {
            //if (dgPrev_Work.RowCount == 0)
            //    return;
            //if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    dgPrev_Work.Rows.RemoveAt(((CurrencyManager)BindingContext[prev_work]).Position);
            //    prev_work.Save();
            //    connection.Commit();
            //}
        }

        private void dgPrev_Transfer_DoubleClick(object sender, EventArgs e)
        {
            btEditPrev_Transfer_Click(sender, e);
        }

        private void dgPrev_Transfer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
                btEditPrev_Transfer_Click(sender, e);
        } 

        #endregion

        #region Работа с таблицей Знание иностранных языков

        // Активация вкладки Знание иностранных языков
        private void tpKnow_Lang_Enter(object sender, EventArgs e)
        {
            if (!f_LoadKnow_Lang)
            {
                // Инициализация таблиц и заполнение их данными    
                lang = new LANG_seq(Connect.CurConnect);
                lang.Fill(string.Format("order by {0}", LANG_seq.ColumnsName.LANGUAGE));
                level_know = new LEVEL_KNOW_seq(Connect.CurConnect);
                level_know.Fill(string.Format("order by {0}", LEVEL_KNOW_seq.ColumnsName.LEV));
                know_lang = new KNOW_LANG_seq(Connect.CurConnect);
                know_lang.Fill(string.Format("where per_num = '{0}'", per_num));

                dgKnow_Lang.AddBindingSource(know_lang, new LinkArgument(lang, "language"), new LinkArgument(level_know, "lev"));
                dgKnow_Lang.Invalidate();
                dgKnow_Lang.Columns["per_num"].Visible = false;
                dgKnow_Lang.Columns["lang_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgKnow_Lang.Columns["level_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgKnow_Lang.Columns["lang_id"].Width = dgKnow_Lang.Width / 2 - 26;
                dgKnow_Lang.Columns["level_id"].Width = dgKnow_Lang.Width - 26 - dgKnow_Lang.Columns["lang_id"].Width;
                f_LoadKnow_Lang = true;
            }       
        }

        // Добавление данных о знаниях иностранных языков
        private void btAddKnow_Lang_Click(object sender, EventArgs e)
        {
            f_AddKnow_lang = true;
            formknow_lang = new Know_Lang(per_num, f_AddKnow_lang, BindingContext[know_lang].Position, know_lang, lang, level_know, dgKnow_Lang, pnVisible, flagArchive);
            formknow_lang.Text = "Добавление данных о знании иностранных языков";
            formknow_lang.ShowDialog();
        }

        // Редактирование данных о знаниях иностранных языков
        private void btEditKnow_Lang_Click(object sender, EventArgs e)
        {
            if (dgKnow_Lang.RowCount == 0)
                return;
            f_AddKnow_lang = false;
            formknow_lang = new Know_Lang(per_num, f_AddKnow_lang, BindingContext[know_lang].Position, know_lang, lang, level_know, dgKnow_Lang, pnVisible, flagArchive);
            formknow_lang.Text = "Редактирование данных о знании иностранных языков";
            formknow_lang.ShowDialog();
        }

        // Уданение данных о знаниях иностранных языков
        private void btDeleteKnow_Lang_Click(object sender, EventArgs e)
        {
            if (dgKnow_Lang.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgKnow_Lang.Rows.RemoveAt(((CurrencyManager)BindingContext[know_lang]).Position);
                know_lang.Save();
                Connect.Commit();
            }
        }
        # endregion        

        #region Работа с таблицей Награды

        // Активация вкладки Награды
        private void tpRewards_Enter(object sender, EventArgs e)
        {
            if (!f_LoadReward)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                reward_name = new REWARD_NAME_seq(Connect.CurConnect);
                reward_name.Fill(string.Format("order by {0}", REWARD_NAME_seq.ColumnsName.REWARD_NAME));
                reward = new REWARD_seq(Connect.CurConnect);
                reward.Fill(string.Format("where per_num = '{0}' order by {1}", per_num, 
                    REWARD_seq.ColumnsName.DATE_REWARD));
                dgRewards.AddBindingSource(reward, new LinkArgument(reward_name, REWARD_NAME_seq.ColumnsName.REWARD_NAME));
                dgRewards.Invalidate();
                dgRewards.Columns["num_reward"].DisplayIndex = 5;
                dgRewards.Columns["reward_name_id"].DisplayIndex = 0;
                dgRewards.Columns["per_num"].Visible = false;
                dgRewards.Columns["base_doc_id"].Visible = false;
                dgRewards.Columns["series"].Visible = false;
                f_LoadReward = true;
            }
            DataGridViewTextBoxColumn dcType_Reward = new DataGridViewTextBoxColumn();
            dcType_Reward.Name = "type_Reward";
            dgRewards.Columns.Insert(4, dcType_Reward);
            dgRewards.Columns["type_Reward"].HeaderText = "Тип награды";
            dgRewards.Columns["type_Reward"].Width = 50;
            dgRewards.Columns["type_Reward"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dgRewards.Columns["dcType_Reward"].DisplayIndex = 7;
            TYPE_REWARD_seq type_reward = new TYPE_REWARD_seq(Connect.CurConnect);
            int i = 0;
            foreach (REWARD_obj rew in reward)
            {
                type_reward.Clear();
                type_reward.Fill(string.Format("where {0} = (select {0} from {1}.REWARD_NAME RN " + 
                    "where RN.REWARD_NAME_ID = {2})", TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_ID, Staff.DataSourceScheme.SchemeName,
                    rew.REWARD_NAME_ID));
                dgRewards.Rows[i].Cells["type_Reward"].Value =
                    ((TYPE_REWARD_obj)((CurrencyManager)BindingContext[type_reward]).Current).TYPE_REWARD_NAME;
                i++;
            }

        }

        // Добавление данных о наградах
        private void btAddRewards_Click(object sender, EventArgs e)
        {
            f_AddReward = true;
            formrewards = new Rewards(per_num, f_AddReward, BindingContext[reward].Position, reward, reward_name, base_doc, dgRewards, pnVisible, flagArchive);
            formrewards.Text = "Добавление данных о наградах";
            formrewards.ShowDialog();
        }

        // Редактирование данных о наградах
        private void btEditRewards_Click(object sender, EventArgs e)
        {
            if (dgRewards.RowCount == 0)
                return;
            f_AddReward = false;
            formrewards = new Rewards(per_num, f_AddReward, BindingContext[reward].Position, reward, reward_name, base_doc, dgRewards, pnVisible, flagArchive);
            formrewards.Text = "Редактирование данных о наградах";
            formrewards.ShowDialog();
        }

        // Уданение данных о наградах
        private void btDeleteRewards_Click(object sender, EventArgs e)
        {
            if (dgRewards.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgRewards.Rows.RemoveAt(((CurrencyManager)BindingContext[reward]).Position);
                reward.Save();
                Connect.Commit();
            }
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
                type_priv.Fill(string.Format("order by {0}", TYPE_PRIV_seq.ColumnsName.NAME_PRIV));
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                emp_priv = new EMP_PRIV_seq(Connect.CurConnect);
                emp_priv.Fill(string.Format("where per_num = '{0}'", per_num));

                dgEmp_Priv.AddBindingSource(emp_priv, new LinkArgument(type_priv, "name_priv"), new LinkArgument(base_doc, "base_doc_name"));
                dgEmp_Priv.Invalidate();
                dgEmp_Priv.Columns["per_num"].Visible = false;
                dgEmp_Priv.Columns["DATE_RECALC"].Visible = false;
                f_LoadEmp_priv = true;
            }
        }

        // Добавление данных Социальные льготы
        private void btAddEmp_Priv_Click(object sender, EventArgs e)
        {
            f_AddEmp_priv = true;
            formemp_priv = new Emp_Priv(per_num, f_AddEmp_priv, BindingContext[emp_priv].Position, emp_priv, type_priv, base_doc, dgEmp_Priv, pnVisible, flagArchive);
            formemp_priv.Text = "Добавление данных о социальных льготах и инвалидности";
            formemp_priv.ShowDialog();
        }

        // Редактирование данных Социальные льготы
        private void btEditEmp_Priv_Click(object sender, EventArgs e)
        {
            if (dgEmp_Priv.RowCount == 0)
                return;
            f_AddEmp_priv = false;
            formemp_priv = new Emp_Priv(per_num, f_AddEmp_priv, BindingContext[emp_priv].Position, emp_priv, type_priv, base_doc, dgEmp_Priv, pnVisible, flagArchive);
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
              
        #region Работа с таблицей Аттестация

        // Активация вкладки Аттестация
        private void tpAttest_Enter(object sender, EventArgs e)
        {
            if (!f_LoadAttest)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                attest = new ATTEST_seq(Connect.CurConnect);
                attest.Fill(string.Format("where per_num = '{0}'", per_num));

                // Инициализация биндингсоурсов и привязка к ним таблиц      
                dgAttest.AddBindingSource(attest, new LinkArgument(base_doc, "base_doc_name"));
                dgAttest.Invalidate();
                dgAttest.Columns["per_num"].Visible = false;
                f_LoadAttest = true;
            }
        }

        // Добавление данных Аттестация
        private void btAddAttest_Click(object sender, EventArgs e)
        {
            f_AddAttest = true;
            formattest = new Attest(per_num, f_AddAttest, BindingContext[attest].Position, attest, base_doc, dgAttest, pnVisible, flagArchive);
            formattest.Text = "Добавление данных об аттестации";
            formattest.ShowDialog();
        }

        // Редактирование данных Аттестация
        private void btEditAttest_Click(object sender, EventArgs e)
        {
            if (dgAttest.RowCount == 0)
                return;
            f_AddAttest = false;
            formattest = new Attest(per_num, f_AddAttest, BindingContext[attest].Position, attest, base_doc, dgAttest, pnVisible, flagArchive);
            formattest.Text = "Редактирование данных об аттестации";
            formattest.ShowDialog();
        }

        // Уданение данных Аттестация
        private void btDeleteAttest_Click(object sender, EventArgs e)
        {
            if (dgAttest.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgAttest.Rows.RemoveAt(((CurrencyManager)BindingContext[attest]).Position);
                attest.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Профессиональная переподготовка

        // Активация вкладки Профессиональная переподготовка
        private void tpProf_Train_Enter(object sender, EventArgs e)
        {
            if (!f_LoadProf_train)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                if (!f_LoadSpec)
                {
                    speciality = new SPECIALITY_seq(Connect.CurConnect);
                    speciality.Fill(string.Format("order by {0}", SPECIALITY_seq.ColumnsName.NAME_SPEC));
                    f_LoadSpec = true;
                }
                prof_train = new PROF_TRAIN_seq(Connect.CurConnect);
                prof_train.Fill(string.Format("where per_num = '{0}'", per_num));

                dgProf_Train.AddBindingSource(prof_train, new LinkArgument(base_doc, "base_doc_name"));
                dgProf_Train.Invalidate();
                dgProf_Train.Columns["per_num"].Visible = false;
                f_LoadProf_train = true;
            }
        }

        // Добавление данных Профессиональная переподготовка
        private void btAddProf_Train_Click(object sender, EventArgs e)
        {
            f_AddProf_train = true;
            formprof_train = new Prof_Train(per_num, f_AddProf_train, BindingContext[prof_train].Position, prof_train, base_doc, dgProf_Train, pnVisible, flagArchive);
            formprof_train.Text = "Добавление данных о профессиональной переподготовке";
            formprof_train.ShowDialog();
        }

        // Редактирование данных Профессиональная переподготовка
        private void btEditProf_Train_Click(object sender, EventArgs e)
        {
            if (dgProf_Train.RowCount == 0)
                return;
            f_AddProf_train = false;
            formprof_train = new Prof_Train(per_num, f_AddProf_train, BindingContext[prof_train].Position, prof_train, base_doc, dgProf_Train, pnVisible, flagArchive);
            formprof_train.Text = "Редактирование данных о профессиональной переподготовке";
            formprof_train.ShowDialog();
        }

        // Уданение данных Профессиональная переподготовка
        private void btDeleteProf_Train_Click(object sender, EventArgs e)
        {
            if (dgProf_Train.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgProf_Train.Rows.RemoveAt(((CurrencyManager)BindingContext[prof_train]).Position);
                prof_train.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Повышение квалификации

        // Активация вкладки Повышение квалификации
        private void tpRise_Qual_Enter(object sender, EventArgs e)
        {
            if (!f_LoadRise_qual)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                if (!f_LoadInstit)
                {
                    instit = new INSTIT_seq(Connect.CurConnect);
                    instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
                    f_LoadInstit = true;
                }
                type_rise_qual = new TYPE_RISE_QUAL_seq(Connect.CurConnect);
                type_rise_qual.Fill(string.Format("order by {0}", TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_NAME));
                rise_qual = new RISE_QUAL_seq(Connect.CurConnect);
                rise_qual.Fill(string.Format("where per_num = '{0}' and sign_rise_qual = 1", per_num));

                dgRise_Qual.AddBindingSource(rise_qual, new LinkArgument(base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME),
                    new LinkArgument(instit, INSTIT_seq.ColumnsName.INSTIT_NAME), 
                    new LinkArgument(type_rise_qual, TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_NAME));
                dgRise_Qual.Invalidate();
                dgRise_Qual.Columns["per_num"].Visible = false;
                dgRise_Qual.Columns["SIGN_RISE_QUAL"].Visible = false;
                dgRise_Qual.Columns["RQ_DATE_ISSUE_DOC"].Visible = false;
                dgRise_Qual.Columns["RQ_DATE_EXPIRY"].Visible = false;
                f_LoadRise_qual = true;
            }
        }

        // Добавление данных Повышение квалификации
        private void btAddRise_Qual_Click(object sender, EventArgs e)
        {
            f_AddRise_qual = true;
            formrise_qual = new Rise_Qual(per_num, f_AddRise_qual, BindingContext[rise_qual].Position,
                rise_qual, base_doc, instit, type_rise_qual, dgRise_Qual, pnVisible, true, flagArchive);
            formrise_qual.Text = "Добавление данных о повышении квалификации";
            formrise_qual.ShowDialog();
        }

        // Редактирование данных Повышение квалификацииа
        private void btEditRise_Qual_Click(object sender, EventArgs e)
        {
            if (dgRise_Qual.RowCount == 0)
                return;
            f_AddRise_qual = false;
            formrise_qual = new Rise_Qual(per_num, f_AddRise_qual, BindingContext[rise_qual].Position,
                rise_qual, base_doc, instit, type_rise_qual, dgRise_Qual, pnVisible, true, flagArchive);
            formrise_qual.Text = "Редактирование данных о повышении квалификации";
            formrise_qual.ShowDialog();
        }

        // Уданение данных Повышение квалификации
        private void btDeleteRise_Qual_Click(object sender, EventArgs e)
        {
            if (dgRise_Qual.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgRise_Qual.Rows.RemoveAt(((CurrencyManager)BindingContext[rise_qual]).Position);
                rise_qual.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Повышение квалификации (Обязательное образование)

        // Активация вкладки
        private void tpCompulsory_edu_Enter(object sender, EventArgs e)
        {
            if (!f_LoadCompulsary_edu)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                if (!f_LoadInstit)
                {
                    instit = new INSTIT_seq(Connect.CurConnect);
                    instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
                    f_LoadInstit = true;
                }
                if (!f_LoadType_rise_qual)
                {
                    type_rise_qual = new TYPE_RISE_QUAL_seq(Connect.CurConnect);
                    type_rise_qual.Fill(string.Format("order by {0}", TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_NAME));
                    f_LoadType_rise_qual = true;
                }
                compulsary_edu = new RISE_QUAL_seq(Connect.CurConnect);
                compulsary_edu.Fill(string.Format("where per_num = '{0}' and sign_rise_qual = 0", per_num));

                dgCompulsory_edu.AddBindingSource(compulsary_edu, new LinkArgument(base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME),
                    new LinkArgument(instit, INSTIT_seq.ColumnsName.INSTIT_NAME),
                    new LinkArgument(type_rise_qual, TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_NAME));
                dgCompulsory_edu.Invalidate();
                dgCompulsory_edu.Columns["per_num"].Visible = false;
                dgCompulsory_edu.Columns["sign_rise_qual"].Visible = false;
                dgCompulsory_edu.Columns["RQ_DATE_DOC"].Visible = false;
                f_LoadCompulsary_edu = true;
            }
        }

        private void btAddCompulsary_edu_Click(object sender, EventArgs e)
        {
            f_AddCompulsory_edu = true;
            formrise_qual = new Rise_Qual(per_num, f_AddCompulsory_edu, BindingContext[compulsary_edu].Position,
                compulsary_edu, base_doc, instit, type_rise_qual, dgCompulsory_edu, pnVisible, false, flagArchive);
            formrise_qual.Text = "Добавление данных об обязательном обучении";
            formrise_qual.ShowDialog();
        }

        private void btEditCompulsary_edu_Click(object sender, EventArgs e)
        {
            if (dgCompulsory_edu.RowCount == 0)
                return;
            f_AddCompulsory_edu = false;
            formrise_qual = new Rise_Qual(per_num, f_AddCompulsory_edu, BindingContext[compulsary_edu].Position,
                compulsary_edu, base_doc, instit, type_rise_qual, dgCompulsory_edu, pnVisible, false, flagArchive);
            formrise_qual.Text = "Редактирование данных об обязательном обучении";
            formrise_qual.ShowDialog();
        }

        private void btDeleteCompulsary_edu_Click(object sender, EventArgs e)
        {
            if (dgCompulsory_edu.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgCompulsory_edu.Rows.RemoveAt(((CurrencyManager)BindingContext[compulsary_edu]).Position);
                compulsary_edu.Save();
                Connect.Commit();
            }
        }
        # endregion

        #region Работа с таблицей Послевузовское образование

        // Активация вкладки Послевузовское образование
        private void tpPostG_Study_Enter(object sender, EventArgs e)
        {
            if (!f_LoadPostg_study)
            {
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadInstit)
                {
                    instit = new INSTIT_seq(Connect.CurConnect);
                    instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
                    f_LoadInstit = true;
                }
                type_postg_study = new TYPE_POSTG_STUDY_seq(Connect.CurConnect);
                type_postg_study.Fill(string.Format("order by {0}", TYPE_POSTG_STUDY_seq.ColumnsName.TYPE_POSTG_STUDY_NAME));
                postg_study = new POSTG_STUDY_seq(Connect.CurConnect);
                postg_study.Fill(string.Format("where per_num = '{0}'", per_num));

                dgPostG_Study.AddBindingSource(postg_study, new LinkArgument(instit, "instit_name"), new LinkArgument(type_postg_study, "type_postg_study_name"));
                dgPostG_Study.Invalidate();
                dgPostG_Study.Columns["per_num"].Visible = false;
                f_LoadPostg_study = true;
            }
        }

        // Добавление данных Послевузовское образование
        private void btAddPostG_Study_Click(object sender, EventArgs e)
        {
            f_AddPostg_study = true;
            formpostg_study = new PostG_Study(per_num, f_AddPostg_study, BindingContext[postg_study].Position, postg_study, instit, type_postg_study, dgPostG_Study, pnVisible, flagArchive);
            formpostg_study.Text = "Добавление данных о послевузовском образовании";
            formpostg_study.ShowDialog();
        }

        // Редактирование данных Послевузовское образование
        private void btEditPostG_Study_Click(object sender, EventArgs e)
        {
            if (dgPostG_Study.RowCount == 0)
                return;
            f_AddPostg_study = false;
            formpostg_study = new PostG_Study(per_num, f_AddPostg_study, BindingContext[postg_study].Position, postg_study, instit, type_postg_study, dgPostG_Study, pnVisible, flagArchive);
            formpostg_study.Text = "Редактирование данных о послевузовском образовании";
            formpostg_study.ShowDialog();
        }

        // Уданение данных Послевузовское образование
        private void btDeletePostG_Study_Click(object sender, EventArgs e)
        {
            if (dgPostG_Study.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgPostG_Study.Rows.RemoveAt(((CurrencyManager)BindingContext[postg_study]).Position);
                postg_study.Save();
                Connect.Commit();
                //Library.VisiblePanel(dgViewPostG_Study, pnVisible);
            }
        }

        // Изменение ширины визуальной панели при изменении ширины столбцов
        private void dgViewPostG_Study_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            //if (pnVisible.Visible)
            //{
            //    Library.VisiblePanel(dgViewPostG_Study, pnVisible);
            //}
        }

        # endregion

        #region Работа с таблицей Загранпаспорт

        /// <summary>
        /// Активация вкладки Загранпаспорт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpForeign_Passport_Enter(object sender, EventArgs e)
        {
            if (!f_LoadFP)
            {
                // Инициализация таблиц и заполнение их данными    
                foreign_passport = new FOREIGN_PASSPORT_seq(Connect.CurConnect);
                foreign_passport.Fill(string.Format("where per_num = '{0}'", per_num));
                dgFP.AddBindingSource(foreign_passport);
                dgFP.Invalidate();
                dgFP.Columns["per_num"].Visible = false;
                f_LoadFP = true;
            }
        }

        /// <summary>
        /// Добавление данных о загранпаспорте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddFP_Click(object sender, EventArgs e)
        {
            f_AddFP = true;
            formFP = new Foreign_Passport(per_num, f_AddFP, BindingContext[foreign_passport].Position,
                foreign_passport, dgFP, flagArchive);
            formFP.Text = "Добавление данных о загранпаспорте";
            formFP.ShowDialog();
        }

        /// <summary>
        /// Редактирование данных о загранпаспорте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditFP_Click(object sender, EventArgs e)
        {
            if (dgFP.RowCount == 0)
                return;
            f_AddFP = false;
            formFP = new Foreign_Passport(per_num, f_AddFP, BindingContext[foreign_passport].Position,
                foreign_passport, dgFP, flagArchive);
            formFP.Text = "Редактирование данных о загранпаспорте";
            formFP.ShowDialog();
        }

        /// <summary>
        /// Удаление данных о загранпаспорте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteFP_Click(object sender, EventArgs e)
        {
            if (dgFP.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgFP.Rows.RemoveAt(((CurrencyManager)BindingContext[foreign_passport]).Position);
                foreign_passport.Save();
                Connect.Commit();
            }
        }

        # endregion

        #region Активация вкладки Пользователь сети

        /// <summary>
        /// Активация вкладки Пользователь сети
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpSystem_User_Enter(object sender, EventArgs e)
        {
            if (f_LoadSystem_User == false)
            {
                system_user = new SYSTEM_USER_seq(Connect.CurConnect);
                system_user.Fill(string.Format("where per_num = {0}", per_num));
                tbNet_Name.AddBindingSource(system_user, SYSTEM_USER_seq.ColumnsName.NET_NAME);
                tbWww_Name.AddBindingSource(system_user, SYSTEM_USER_seq.ColumnsName.WWW_NAME);
                f_LoadSystem_User = true;
            }
            else
            {
                system_user.Clear();
                system_user.Fill(string.Format("where per_num = {0}", per_num));
            }
        }

        /// <summary>
        /// Добавить имя пользователя сети и Интернет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddSystem_User_Click(object sender, EventArgs e)
        {
            if (system_user.Count == 0)
            {
                system_user.AddNew();
                ((SYSTEM_USER_obj)((CurrencyManager)BindingContext[system_user]).Current).PER_NUM = per_num;
            }
            tbNet_Name.Enabled = true;
            tbWww_Name.Enabled = true;
            tbNet_Name.Focus();
        }

        /// <summary>
        /// Редактировать имя пользователя сети и Интернет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditSystem_User_Click(object sender, EventArgs e)
        {
            if (system_user.Count != 0)
            {
                tbNet_Name.Enabled = true;
                tbWww_Name.Enabled = true;
            }
        }

        /// <summary>
        /// Удалить имя пользователя сети и Интернет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteSystem_User_Click(object sender, EventArgs e)
        {
            if (system_user.Count != 0)
            {
                if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    system_user.Remove((SYSTEM_USER_obj)((CurrencyManager)BindingContext[system_user]).Current);
                    system_user.Save();
                    Connect.Commit();
                }
            }
        }

        /// <summary>
        /// Сохранение имени пользователя сети и Интернет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveSystem_User_Click(object sender, EventArgs e)
        {
            if (system_user.Count > 0)
            {
                if (((SYSTEM_USER_obj)((CurrencyManager)BindingContext[system_user]).Current).NET_NAME == null ||
                    ((SYSTEM_USER_obj)((CurrencyManager)BindingContext[system_user]).Current).NET_NAME.Trim() == "")
                {
                    MessageBox.Show("Имя пользователя сети не введено!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbNet_Name.Focus();
                    return;
                }
                system_user.Save();
                Connect.Commit();
            }
            tbNet_Name.Enabled = false;
            tbWww_Name.Enabled = false;
            
        }    

        # endregion

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
                rel_typeChild.Fill("where upper(NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА','ПЛЕМЯННИК','ПЛЕМЯННИЦА','ДРУГАЯ СТЕПЕНЬ РОДСТВА') order by NAME_REL");
                relativeChild = new RELATIVE_seq(Connect.CurConnect);
                relativeChild.Fill(string.Format("where per_num = '{1}' " +
                    "and REL_ID in (select RT.REL_ID from {0}.rel_type RT " +
                                    "where upper(RT.NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА','ПЛЕМЯННИК','ПЛЕМЯННИЦА','ДРУГАЯ СТЕПЕНЬ РОДСТВА') )" + 
                    "and ((REL_BIRTH_DATE is not null and REL_BIRTH_DATE >= ADD_MONTHS(sysdate, -216)) or " + 
                    "(REL_BIRTH_YEAR is not null and REL_BIRTH_YEAR >= extract(year from sysdate) - 18))",
                    DataSourceScheme.SchemeName, per_num));
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
                
                //child_rearing_leave = new CHILD_REARING_LEAVE_seq(Connect.CurConnect);
                //deCRL_Date_Begin_1_5.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_BEGIN_1_5);
                //deCRL_Date_End_1_5.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_END_1_5);
                //chCRL_Breakpoint_1_5.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_BREAKPOINT_1_5);
                //deCRL_Date_Begin_3.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_BEGIN_3);
                //deCRL_Date_End_3.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_END_3);
                //chCRL_Breakpoint_3.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_BREAKPOINT_3);
                //deCRL_Date_Departure.AddBindingSource(child_rearing_leave,
                //    CHILD_REARING_LEAVE_seq.ColumnsName.CRL_DATE_DEPARTURE);
                
                _dtChild_Care_Leave_1_5 = new DataTable();
                _dtChild_Care_Leave_3 = new DataTable();
                _daChild_Care_Leave = new OracleDataAdapter();
                // Select
                _daChild_Care_Leave.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("SelectChild_Care_Leave_For_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daChild_Care_Leave.SelectCommand.BindByName = true;
                _daChild_Care_Leave.SelectCommand.Parameters.Add("p_RELATIVE_ID", OracleDbType.Decimal).Value = -1;
                _daChild_Care_Leave.SelectCommand.Parameters.Add("p_SIGN_LEAVE_1_5_YEARS", OracleDbType.Decimal).Value = 1;
                // Insert
                _daChild_Care_Leave.InsertCommand = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.CHILD_CARE_LEAVE_UPDATE(:CHILD_CARE_LEAVE_ID, :RELATIVE_ID, :DATE_BEGIN_LEAVE, :DATE_END_LEAVE, 
                            :BREAKPOINT_LEAVE, :PROLONGATION_LEAVE, :DATE_DEPARTURE, :SIGN_LEAVE_1_5_YEARS);
                    END;", Connect.Schema), Connect.CurConnect);
                _daChild_Care_Leave.InsertCommand.BindByName = true;
                _daChild_Care_Leave.InsertCommand.Parameters.Add("CHILD_CARE_LEAVE_ID", OracleDbType.Decimal, 0, "CHILD_CARE_LEAVE_ID").Direction = 
                    ParameterDirection.InputOutput;
                _daChild_Care_Leave.InsertCommand.Parameters["CHILD_CARE_LEAVE_ID"].DbType = DbType.Decimal;
                _daChild_Care_Leave.InsertCommand.Parameters.Add("RELATIVE_ID", OracleDbType.Decimal, 0, "RELATIVE_ID");
                _daChild_Care_Leave.InsertCommand.Parameters.Add("DATE_BEGIN_LEAVE", OracleDbType.Date, 0, "DATE_BEGIN_LEAVE");
                _daChild_Care_Leave.InsertCommand.Parameters.Add("DATE_END_LEAVE", OracleDbType.Date, 0, "DATE_END_LEAVE").Direction =
                    ParameterDirection.InputOutput;
                _daChild_Care_Leave.InsertCommand.Parameters["DATE_END_LEAVE"].DbType = DbType.Date;
                _daChild_Care_Leave.InsertCommand.Parameters.Add("BREAKPOINT_LEAVE", OracleDbType.Decimal, 0, "BREAKPOINT_LEAVE");
                _daChild_Care_Leave.InsertCommand.Parameters.Add("PROLONGATION_LEAVE", OracleDbType.Decimal, 0, "PROLONGATION_LEAVE").Direction =
                    ParameterDirection.InputOutput;
                _daChild_Care_Leave.InsertCommand.Parameters["PROLONGATION_LEAVE"].DbType = DbType.Decimal;
                _daChild_Care_Leave.InsertCommand.Parameters.Add("DATE_DEPARTURE", OracleDbType.Date, 0, "DATE_DEPARTURE").Direction =
                    ParameterDirection.InputOutput;
                _daChild_Care_Leave.InsertCommand.Parameters["DATE_DEPARTURE"].DbType = DbType.Date;
                _daChild_Care_Leave.InsertCommand.Parameters.Add("SIGN_LEAVE_1_5_YEARS", OracleDbType.Decimal, 0, "SIGN_LEAVE_1_5_YEARS");
                // Insert
                _daChild_Care_Leave.UpdateCommand = _daChild_Care_Leave.InsertCommand;
                // Delete
                _daChild_Care_Leave.DeleteCommand = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.CHILD_CARE_LEAVE_DELETE(:CHILD_CARE_LEAVE_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _daChild_Care_Leave.DeleteCommand.BindByName = true;
                _daChild_Care_Leave.DeleteCommand.Parameters.Add("CHILD_CARE_LEAVE_ID", OracleDbType.Decimal, 0, "CHILD_CARE_LEAVE_ID");

                _daChild_Care_Leave.Fill(_dtChild_Care_Leave_1_5);
                dgChild_Care_Leave_1_5.AutoGenerateColumns = false;
                dgChild_Care_Leave_1_5.DataSource = _dtChild_Care_Leave_1_5;

                _daChild_Care_Leave.Fill(_dtChild_Care_Leave_3);
                dgChild_Care_Leave_3.AutoGenerateColumns = false;
                dgChild_Care_Leave_3.DataSource = _dtChild_Care_Leave_3;

                if (relativeChild.Count == 0)
                {
                    cbRel_ID.SelectedItem = null;
                }
                //deCRL_Date_Departure.Enabled = false;
                EnableForChild(false);

                dgChild_Rearing_Leave.SelectionChanged += new EventHandler(dgChild_Rearing_Leave_SelectionChanged);
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
            if (chCRL_Breakpoint_3.Checked)
                deCRL_Date_Departure.Enabled = _flag;
        }

        /// <summary>
        /// Добавление данных ребенка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddCRL_Click(object sender, EventArgs e)
        {
            relativeChild.AddNew();
            ((CurrencyManager)BindingContext[relativeChild]).Position =
                ((CurrencyManager)BindingContext[relativeChild]).Count - 1;
            ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).PER_NUM = per_num;
            //child_rearing_leave.Clear();
            //child_rearing_leave.AddNew();
            //((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).RELATIVE_ID =
            //    ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID;
            EnableForChild(true);
            tbRel_Last_Name.Focus();
        }

        /// <summary>
        /// Отмена внесения изменений в данные ребенка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancelChild_Click(object sender, EventArgs e)
        {
            relativeChild.Clear();
            relativeChild.Fill(string.Format("where per_num = '{1}' " +
                "and REL_ID in (select RT.REL_ID from {0}.rel_type RT " +
                                    "where upper(RT.NAME_REL) in ('СЫН','ДОЧЬ','ВНУК','ВНУЧКА') )" +
                    "and ((REL_BIRTH_DATE is not null and REL_BIRTH_DATE >= ADD_MONTHS(sysdate, -216)) or " +
                    "(REL_BIRTH_YEAR is not null and REL_BIRTH_YEAR >= extract(year from sysdate) - 18))",
                DataSourceScheme.SchemeName, per_num));
            deCRL_Date_Departure.Enabled = false;
            EnableForChild(false);
        }

        /// <summary>
        /// Редактирование данных ребенка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditCRL_Click(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.RowCount == 0)
                return;
            //if (child_rearing_leave.Count == 0)
            //{
            //    child_rearing_leave.Clear();
            //    child_rearing_leave.AddNew();
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).RELATIVE_ID =
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID;
            //}
            EnableForChild(true);
            tbRel_Last_Name.Focus();
        }

        /// <summary>
        /// Удаление данных ребенка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteCRL_Click(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.RowCount == 0)
                return;
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //if (child_rearing_leave.Count > 0)
                //{
                //    child_rearing_leave.Remove(
                //        ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current));
                //    child_rearing_leave.Save();
                //}
                dgChild_Rearing_Leave.Rows.RemoveAt(((CurrencyManager)BindingContext[relativeChild]).Position);
                relativeChild.Save();
                Connect.Commit();
                GetChild_Care_Leave_1_5(-1);
                GetChild_Care_Leave_3(-1);
            }
        }

        /// <summary>
        /// Сохранение данных ребенка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveCRL_Click(object sender, EventArgs e)
        {
            if (relativeChild.Count > 0)
            {
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_LAST_NAME == null
                    || ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_LAST_NAME == "")
                {
                    MessageBox.Show("Вы не указали фамилию!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbRel_Last_Name.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_FIRST_NAME == null
                    || ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_FIRST_NAME == "")
                {
                    MessageBox.Show("Вы не указали имя!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbRel_First_Name.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_MIDDLE_NAME == null
                    || ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_MIDDLE_NAME == "")
                {
                    MessageBox.Show("Вы не указали отчество!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbRel_Middle_Name.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_DATE == null)
                {
                    MessageBox.Show("Вы не указали дату рождения!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    deRel_Birth_Date.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_ID == null)
                {
                    MessageBox.Show("Вы не указали степень родства!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    cbRel_ID.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_CERTIFICATE_SER == null
                    || ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_CERTIFICATE_SER == "")
                {
                    MessageBox.Show("Вы не ввели серию свидетельства о рождении!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbRel_Birth_Certification_ser.Focus();
                    return;
                }
                if (((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_CERTIFICATE_NUM == null
                    || ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_CERTIFICATE_NUM == "")
                {
                    MessageBox.Show("Вы не ввели номер свидетельства о рождении!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    tbRel_Birth_Certification_num.Focus();
                    return;
                }

                //if ((chCRL_Breakpoint_1_5.Checked || chCRL_Breakpoint_3.Checked) && deCRL_Date_Departure.Date == null)
                //{
                //    MessageBox.Show("Вы не указали дату выхода на работу!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                //        MessageBoxIcon.Exclamation);
                //    deCRL_Date_Departure.Focus();
                //    return;
                //}
                relativeChild.Save();
                //child_rearing_leave.Save();
                Connect.Commit();
                deCRL_Date_Departure.Enabled = false;
                EnableForChild(false);
                GetChild_Care_Leave_1_5(((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID);
                GetChild_Care_Leave_3(((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID);
            }
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
                    if (_dtChild_Care_Leave_1_5 != null)
                    {
                        GetChild_Care_Leave_1_5(((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID);
                        GetChild_Care_Leave_3(((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID);
                    }

                    //child_rearing_leave.Clear();
                    //child_rearing_leave.Fill(string.Format("where relative_id = {0}",
                    //    ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID));
                    //if (child_rearing_leave.Count > 0)
                    //{
                    //    if (((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_PROLONGATION_1_5)
                    //    {
                    //        chCRL_Prolongation_1_5.Checked = true;
                    //    }
                    //    else
                    //    {
                    //        chCRL_Prolongation_1_5.Checked = false;
                    //    }
                    //}
                }
            }
        }

        void GetChild_Care_Leave_1_5(decimal? relative_id)
        {
            _dtChild_Care_Leave_1_5.Clear();
            _daChild_Care_Leave.SelectCommand.Parameters["p_RELATIVE_ID"].Value = relative_id;
            _daChild_Care_Leave.SelectCommand.Parameters["p_SIGN_LEAVE_1_5_YEARS"].Value = 1;
            _daChild_Care_Leave.Fill(_dtChild_Care_Leave_1_5);
        }

        void GetChild_Care_Leave_3(decimal? relative_id)
        {
            _dtChild_Care_Leave_3.Clear();
            _daChild_Care_Leave.SelectCommand.Parameters["p_RELATIVE_ID"].Value = relative_id;
            _daChild_Care_Leave.SelectCommand.Parameters["p_SIGN_LEAVE_1_5_YEARS"].Value = 0;
            _daChild_Care_Leave.Fill(_dtChild_Care_Leave_3);
        }

        /// <summary>
        /// Формирование справки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFormReference_Click(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.CurrentRow != null)
            {
                /* 10.03.2016 - Формируем отчет по новому варианту
                 * string lenght, dateB, dateE;
                lenght = dateB = dateE = "";
                if (deCRL_Date_Begin_3.Date == null)
                {
                    lenght = deCRL_Date_Begin_1_5.Date != null ? "1,5" : "";
                    dateB = deCRL_Date_Begin_1_5.Date != null ? deCRL_Date_Begin_1_5.Date.Value.ToShortDateString() : "";
                    dateE = deCRL_Date_End_1_5.Date != null ? deCRL_Date_End_1_5.Date.Value.ToShortDateString() : "";
                }
                else
                {
                    lenght = deCRL_Date_Begin_3.Date != null ? "3" : "";
                    dateB = deCRL_Date_Begin_3.Date != null ? deCRL_Date_Begin_3.Date.Value.ToShortDateString() : "";
                    dateE = deCRL_Date_End_3.Date != null ? deCRL_Date_End_3.Date.Value.ToShortDateString() : "";
                }
                ExcelParameter[] excelParameter = new ExcelParameter[] 
                { 
                    new ExcelParameter("P5", tbCode_Subdiv.Text), 
                    new ExcelParameter("U5", tbPer_Num.Text),
                    new ExcelParameter("C7", string.Format("{0} {1} {2}", 
                        Padeg.Dateln(tbSername.Text, 
                            cbSex.Text == "М" ? Padeg.Type_value.Mans_Family : Padeg.Type_value.Womens_Family), 
                        Padeg.Dateln(tbName.Text,
                            cbSex.Text == "М" ? Padeg.Type_value.Mans_Name : Padeg.Type_value.Womens_Name), 
                        Padeg.Dateln(tbMiddle_Name.Text,
                            cbSex.Text == "М" ? Padeg.Type_value.Mans_SName : Padeg.Type_value.Womens_SName))),
                    new ExcelParameter("C12", lenght),
                    new ExcelParameter("J12", dateB),
                    new ExcelParameter("R12", dateE),
                    new ExcelParameter("J14", deRel_Birth_Date.Date.Value.ToShortDateString()),
                    new ExcelParameter("R16", tbRel_Birth_Certification_ser.Text),
                    new ExcelParameter("W16", tbRel_Birth_Certification_num.Text),
                    new ExcelParameter("M18", mbDate_HirePlant.Text),
                    new ExcelParameter("N20", DateTime.Now.ToShortDateString())
                };
                Excel.Print("PregnancyRef.xlt", "A1", null, excelParameter);*/
                DateTime _date = DateTime.Today;
                if (InputDataForm.ShowForm(ref _date, "dd MMMM yyyy") == DialogResult.OK)
                {
                    OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("RepReference_Pregnancy.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daReport.SelectCommand.BindByName = true;
                    _daReport.SelectCommand.Parameters.Add("p_RELATIVE_ID", OracleDbType.Decimal).Value = dgChild_Rearing_Leave.CurrentRow.Cells["RELATIVE_ID"].Value;
                    _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = transfer_id;
                    _daReport.SelectCommand.Parameters.Add("p_DATE", OracleDbType.Date).Value = _date;
                    DataSet _ds = new DataSet();
                    _daReport.Fill(_ds, "Table1");
                    if (_ds.Tables["Table1"].Rows.Count > 0)
                    {
                        DataTable _dt = new DataTable();
                        if (Signes.Show(0, "Reference_By_Emp", "Введите должность и ФИО ответственного лица (для подписи)", 2, ref _dt) == true)
                        {
                            ReportViewerWindow.RenderToExcel(this, "Reference_Pregnancy.rdlc", new DataTable[] { _ds.Tables["Table1"], _dt },
                                null, "Справка по уходу за ребенком", "doc");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет данных!",
                            "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        /// <summary>
        /// Ввод даты начала отпуска на 1.5 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deCRL_Date_Begin_1_5_Validating(object sender, CancelEventArgs e)
        {
            //if (deCRL_Date_Begin_1_5.Date != null)
            //{
            //    // Прибавляем к дате рождения 1.5 года и записываем ее в дату окончания отпуска
            //    deCRL_Date_End_1_5.Date = deRel_Birth_Date.Date.Value.AddMonths(18);
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_1_5 =
            //        deCRL_Date_End_1_5.Date;
            //    chCRL_Prolongation_1_5.Enabled = true;                
            //}
            //else
            //{
            //    deCRL_Date_End_1_5.Date = null;
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_1_5 =
            //        deCRL_Date_End_1_5.Date;
            //    chCRL_Prolongation_1_5.Enabled = false;
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_PROLONGATION_1_5 =
            //        false;
            //}
        }

        /// <summary>
        /// Продление отпуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chCRL_Prolongation_1_5_CheckedChanged(object sender, EventArgs e)
        {
            //if (chCRL_Prolongation_1_5.Checked)
            //{
            //    deCRL_Date_Begin_3.Date =
            //        ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_1_5.Value.AddDays(1);
            //    // Приравниваем даты начала отпусков
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_BEGIN_3 =
            //        deCRL_Date_Begin_3.Date;
            //    deCRL_Date_End_3.Date =
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).REL_BIRTH_DATE.Value.AddYears(3);
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_3 =
            //        deCRL_Date_End_3.Date;
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_PROLONGATION_1_5 = true;
            //    chCRL_Prolongation_1_5.Checked = true;  
            //}
            //else
            //{ 
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_PROLONGATION_1_5 = false;
            //    chCRL_Prolongation_1_5.Checked = false;                
            //}
        }

        /// <summary>
        /// Ввод даты начала отпуска на 3 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deCRL_Date_Begin_3_Validating(object sender, CancelEventArgs e)
        {
            //if (deCRL_Date_Begin_3.Date != null)
            //{
            //    // Прибавляем к дате рождения 3 года и записываем ее в дату окончания отпуска
            //    deCRL_Date_End_3.Date = deRel_Birth_Date.Date.Value.AddYears(3);
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_3 =
            //        deCRL_Date_End_3.Date;
            //}
            //else
            //{
            //    deCRL_Date_End_3.Date = null;
            //    ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_END_3 =
            //        deCRL_Date_End_3.Date;
            //    chCRL_Prolongation_1_5.Checked = false;
            //}
        }
        
        private void chCRL_Breakpoint_3_CheckedChanged(object sender, EventArgs e)
        {
            //if (deCRL_Date_Begin_1_5.Enabled)
            //{
            //    if (chCRL_Breakpoint_3.Checked)
            //    {
            //        deCRL_Date_Departure.Enabled = true;
            //    }
            //    else
            //    {
            //        deCRL_Date_Departure.Enabled = false;
            //        if (child_rearing_leave.Count > 0)
            //        {
            //            ((CHILD_REARING_LEAVE_obj)((CurrencyManager)BindingContext[child_rearing_leave]).Current).CRL_DATE_DEPARTURE = null;
            //            deCRL_Date_Departure.Date = null;
            //        }
            //    }
            //}
        }

        private void chCRL_Breakpoint_1_5_CheckedChanged(object sender, EventArgs e)
        {
            //if (deCRL_Date_Begin_1_5.Enabled)
            //{
            //    if (chCRL_Breakpoint_1_5.Checked)
            //    {
            //        deCRL_Date_Departure.Enabled = true;
            //    }
            //    else
            //    {
            //        deCRL_Date_Departure.Enabled = false;
            //    }
            //}
        }

        private void btAddCRL_1_5_Click(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.SelectedCells.Count > 0)
            {
                DataRowView _drView = _dtChild_Care_Leave_1_5.DefaultView.AddNew();
                _drView["RELATIVE_ID"] =
                    ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID;
                _drView["SIGN_LEAVE_1_5_YEARS"] = 1;
                _dtChild_Care_Leave_1_5.Rows.Add(_drView.Row);
            }
        }

        private void btDeleteCRL_1_5_Click(object sender, EventArgs e)
        {
            if (dgChild_Care_Leave_1_5.CurrentRow != null &&
                MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                System.Windows.Forms.DialogResult.Yes)
            {
                dgChild_Care_Leave_1_5.Rows.Remove(dgChild_Care_Leave_1_5.CurrentRow);
                SaveCRL_1_5();
            }
        }

        private void btCancelCRL_1_5_Click(object sender, EventArgs e)
        {
            _dtChild_Care_Leave_1_5.RejectChanges();
        }

        private void btSaveCRL_1_5_Click(object sender, EventArgs e)
        {
            SaveCRL_1_5();
            GetChild_Care_Leave_3(((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID);
        }

        void SaveCRL_1_5()
        {
            dgChild_Care_Leave_1_5.CommitEdit(DataGridViewDataErrorContexts.Commit);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daChild_Care_Leave.UpdateCommand.Transaction = transact;
                _daChild_Care_Leave.InsertCommand.Transaction = transact;
                _daChild_Care_Leave.DeleteCommand.Transaction = transact;
                _daChild_Care_Leave.Update(_dtChild_Care_Leave_1_5);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                //_dtChild_Care_Leave_1_5.RejectChanges();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btAddCRL_3_Click(object sender, EventArgs e)
        {
            if (dgChild_Rearing_Leave.SelectedCells.Count > 0)
            {
                DataRowView _drView = _dtChild_Care_Leave_3.DefaultView.AddNew();
                _drView["RELATIVE_ID"] =
                    ((RELATIVE_obj)((CurrencyManager)BindingContext[relativeChild]).Current).RELATIVE_ID;
                _drView["SIGN_LEAVE_1_5_YEARS"] = 0;
                _dtChild_Care_Leave_3.Rows.Add(_drView.Row);
            }
        }

        private void btDeleteCRL_3_Click(object sender, EventArgs e)
        {
            if (dgChild_Care_Leave_3.CurrentRow != null &&
                MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                System.Windows.Forms.DialogResult.Yes)
            {
                dgChild_Care_Leave_3.Rows.Remove(dgChild_Care_Leave_3.CurrentRow);
                SaveCRL_3();
            }
        }

        private void btCancelCRL_3_Click(object sender, EventArgs e)
        {
            _dtChild_Care_Leave_3.RejectChanges();
        }

        private void btSaveCRL_3_Click(object sender, EventArgs e)
        {
            SaveCRL_3();
        }

        void SaveCRL_3()
        {
            dgChild_Care_Leave_3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daChild_Care_Leave.UpdateCommand.Transaction = transact;
                _daChild_Care_Leave.InsertCommand.Transaction = transact;
                _daChild_Care_Leave.DeleteCommand.Transaction = transact;
                _daChild_Care_Leave.Update(_dtChild_Care_Leave_3);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                //_dtChild_Care_Leave_1_5.RejectChanges();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Активация вкладки Мед.осмотр

        /// <summary>
        /// Активация вкладки Мед.осмотр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpMed_Inspection_Enter(object sender, EventArgs e)
        {
            if (f_LoadMed_Inspection == false)
            {
                _dtMed_Inspection = new DataTable();
                // Select
                _daMed_Inspection = new OracleDataAdapter(string.Format(
                    @"SELECT MED_INSPECTION_ID, PER_NUM, MED_INSPECTION_DATE, STUDY_LABOR_SAFETY 
                FROM {0}.MED_INSPECTION WHERE PER_NUM = :p_PER_NUM",
                    Connect.Schema), Connect.CurConnect);
                _daMed_Inspection.SelectCommand.BindByName = true;
                _daMed_Inspection.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                // Insert, Update
                _daMed_Inspection.InsertCommand = new OracleCommand(string.Format(
                    @"BEGIN {0}.MED_INSPECTION_UPDATE(:MED_INSPECTION_ID, :PER_NUM, :MED_INSPECTION_DATE, :STUDY_LABOR_SAFETY); END;",
                    Connect.Schema), Connect.CurConnect);
                _daMed_Inspection.InsertCommand.BindByName = true;
                _daMed_Inspection.InsertCommand.Parameters.Add("MED_INSPECTION_ID", OracleDbType.Decimal, 22, "MED_INSPECTION_ID").Direction =
                    ParameterDirection.InputOutput;
                _daMed_Inspection.InsertCommand.Parameters["MED_INSPECTION_ID"].DbType = DbType.Decimal;
                _daMed_Inspection.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
                _daMed_Inspection.InsertCommand.Parameters.Add("MED_INSPECTION_DATE", OracleDbType.Date, 0, "MED_INSPECTION_DATE");
                _daMed_Inspection.InsertCommand.Parameters.Add("STUDY_LABOR_SAFETY", OracleDbType.Date, 0, "STUDY_LABOR_SAFETY");
                _daMed_Inspection.UpdateCommand = _daMed_Inspection.InsertCommand;
                _daMed_Inspection.Fill(_dtMed_Inspection);
                if (_dtMed_Inspection.Rows.Count == 0)
                {
                    DataRowView rowView = _dtMed_Inspection.DefaultView.AddNew();
                    rowView["PER_NUM"] = per_num;
                    _dtMed_Inspection.Rows.Add(rowView.Row);
                }
                dgMed_Inspection.DataSource = _dtMed_Inspection;
                pnMed_Inspection.EnableByRules();
                btSaveMed_Inspection.Enabled = false;
                btCancelMed_Inspection.Enabled = false;
                f_LoadMed_Inspection = true;
            }
        }
        
        /// <summary>
        /// Редактировать мед.осмотр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditMed_Inspection_Click(object sender, EventArgs e)
        {
            dgMed_Inspection.ReadOnly = false;
            btSaveMed_Inspection.Enabled = true;
            btCancelMed_Inspection.Enabled = true;
            btEditMed_Inspection.Enabled = false;
        }

        /// <summary>
        /// Отмена изменения мед.осмотра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancelMed_Inspection_Click(object sender, EventArgs e)
        {
            dgMed_Inspection.ReadOnly = true;
            btSaveMed_Inspection.Enabled = false;
            btCancelMed_Inspection.Enabled = false;
            btEditMed_Inspection.Enabled = true;
            _dtMed_Inspection.RejectChanges();
        }

        /// <summary>
        /// Сохранение мед.осмотра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveMed_Inspection_Click(object sender, EventArgs e)
        {
            dgMed_Inspection.ReadOnly = true;
            btSaveMed_Inspection.Enabled = false;
            btCancelMed_Inspection.Enabled = false;
            btEditMed_Inspection.Enabled = true;
            //_dtMed_Inspection.AcceptChanges();
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daMed_Inspection.InsertCommand.Transaction = transact;
                _daMed_Inspection.UpdateCommand.Transaction = transact;
                _daMed_Inspection.Update(_dtMed_Inspection);
                transact.Commit();
                MessageBox.Show("Данные сохранены!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(OracleException ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        # endregion

        #region Активация вкладки Молодой специалист

        /// <summary>
        /// Активация вкладки Молодой специалист
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tpYoung_Specialist_Enter(object sender, EventArgs e)
        {
            if (f_LoadYoung_Specialist == false)
            {
                _dtYoung_Specialist = new DataTable();
                // Select
                _daYoung_Specialist = new OracleDataAdapter(string.Format(
                    @"SELECT YOUNG_SPECIALIST_ID, BASE_DOC_ID, NUMBER_BASE_DOC, DATE_BASE_DOC, DATE_BEGIN_ADD, DATE_END_ADD, SUM_PAYMENT, PER_NUM 
                    FROM {0}.YOUNG_SPECIALIST WHERE PER_NUM = :p_PER_NUM ORDER BY DATE_BEGIN_ADD",
                    Connect.Schema), Connect.CurConnect);
                _daYoung_Specialist.SelectCommand.BindByName = true;
                _daYoung_Specialist.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                // Insert, Update
                _daYoung_Specialist.InsertCommand = new OracleCommand(string.Format(
                    @"BEGIN {0}.YOUNG_SPECIALIST_UPDATE(:YOUNG_SPECIALIST_ID, :BASE_DOC_ID, :NUMBER_BASE_DOC, :DATE_BASE_DOC, 
                        :DATE_BEGIN_ADD, :DATE_END_ADD, :SUM_PAYMENT, :PER_NUM); 
                    END;",
                    Connect.Schema), Connect.CurConnect);
                _daYoung_Specialist.InsertCommand.BindByName = true;
                _daYoung_Specialist.InsertCommand.Parameters.Add("YOUNG_SPECIALIST_ID", OracleDbType.Decimal, 0, "YOUNG_SPECIALIST_ID").Direction =
                    ParameterDirection.InputOutput;
                _daYoung_Specialist.InsertCommand.Parameters["YOUNG_SPECIALIST_ID"].DbType = DbType.Decimal;
                _daYoung_Specialist.InsertCommand.Parameters.Add("BASE_DOC_ID", OracleDbType.Decimal, 0, "BASE_DOC_ID");
                _daYoung_Specialist.InsertCommand.Parameters.Add("NUMBER_BASE_DOC", OracleDbType.Varchar2, 0, "NUMBER_BASE_DOC");
                _daYoung_Specialist.InsertCommand.Parameters.Add("DATE_BASE_DOC", OracleDbType.Date, 0, "DATE_BASE_DOC");
                _daYoung_Specialist.InsertCommand.Parameters.Add("DATE_BEGIN_ADD", OracleDbType.Date, 0, "DATE_BEGIN_ADD");
                _daYoung_Specialist.InsertCommand.Parameters.Add("DATE_END_ADD", OracleDbType.Date, 0, "DATE_END_ADD");
                _daYoung_Specialist.InsertCommand.Parameters.Add("SUM_PAYMENT", OracleDbType.Decimal, 0, "SUM_PAYMENT");
                _daYoung_Specialist.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
                _daYoung_Specialist.UpdateCommand = _daYoung_Specialist.InsertCommand;
                // Delete
                _daYoung_Specialist.DeleteCommand = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.YOUNG_SPECIALIST_DELETE(:YOUNG_SPECIALIST_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _daYoung_Specialist.DeleteCommand.BindByName = true;
                _daYoung_Specialist.DeleteCommand.Parameters.Add("YOUNG_SPECIALIST_ID", OracleDbType.Decimal, 0, "YOUNG_SPECIALIST_ID");
                // Fill
                _daYoung_Specialist.Fill(_dtYoung_Specialist);
                dgYoung_Specialist.AutoGenerateColumns = false;
                dgYoung_Specialist.DataSource = _dtYoung_Specialist;
                pnYoung_Specialist.EnableByRules();
                f_LoadYoung_Specialist = true;
                // Инициализация таблиц и заполнение их данными    
                if (!f_LoadBase_doc)
                {
                    base_doc = new BASE_DOC_seq(Connect.CurConnect);
                    base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
                    f_LoadBase_doc = true;
                }
                BASE_DOC_ID.DataSource = base_doc;
                BASE_DOC_ID.DisplayMember = "BASE_DOC_NAME";
                BASE_DOC_ID.ValueMember = "BASE_DOC_ID";
            }
        }

        /// <summary>
        /// Редактировать мед.осмотр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddYoung_Specialist_Click(object sender, EventArgs e)
        {
            DataRowView rowView = _dtYoung_Specialist.DefaultView.AddNew();
            rowView["PER_NUM"] = per_num;
            _dtYoung_Specialist.Rows.Add(rowView.Row);
        }
        
        /// <summary>
        /// Редактировать мед.осмотр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteYoung_Specialist_Click(object sender, EventArgs e)
        {
            if (dgYoung_Specialist.SelectedCells.Count > 0 &&
                MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                _dtYoung_Specialist.DefaultView[dgYoung_Specialist.CurrentRow.Index].Delete();
                btSaveYoung_Specialist_Click(sender, e);
            }
        }

        /// <summary>
        /// Отмена изменения мед.осмотра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancelYoung_Specialist_Click(object sender, EventArgs e)
        {
            _dtMed_Inspection.RejectChanges();
        }

        /// <summary>
        /// Сохранение мед.осмотра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveYoung_Specialist_Click(object sender, EventArgs e)
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                dgYoung_Specialist.CommitEdit(DataGridViewDataErrorContexts.Commit);
                _daYoung_Specialist.InsertCommand.Transaction = transact;
                _daYoung_Specialist.UpdateCommand.Transaction = transact;
                _daYoung_Specialist.DeleteCommand.Transaction = transact;
                _daYoung_Specialist.Update(_dtYoung_Specialist);
                transact.Commit();
                Get_Sign_Young_Spec();
                MessageBox.Show("Данные сохранены!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
            catch (OracleException ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        # endregion

        #region Работа с таблицей Нарушения сотрудника

        // Активация вкладки Нарушения
        private void tpViolation_Emp_Enter(object sender, EventArgs e)
        {
            if (_dsViolation_Emp == null)
            {
                _dsViolation_Emp = new DataSet();
                _dsViolation_Emp.Tables.Add("VIOLATION_EMP");
                _dsViolation_Emp.Tables.Add("VIOLATION_EMP_PUN");

                new OracleDataAdapter(string.Format(
                    @"select REASON_DETENTION_ID, REASON_DETENTION_NAME, SIGN_THEFT from {0}.REASON_DETENTION 
                    WHERE SIGN_VISIBLE_IN_PERSONAL_CARD = 1 ORDER BY REASON_DETENTION_NAME",
                    Connect.Schema), Connect.CurConnect).Fill(_dsViolation_Emp, "REASON_DETENTION");
                new OracleDataAdapter(string.Format(
                    @"select TYPE_PUNISHMENT_ID, TYPE_PUNISHMENT_NAME, SIGN_FINANCIAL, SIGN_DELETE_VIOLATION, TYPE_GROUP_PUNISHMENT_ID 
                    from {0}.TYPE_PUNISHMENT", 
                    Connect.Schema), Connect.CurConnect).Fill(_dsViolation_Emp, "TYPE_PUNISHMENT");
                new OracleDataAdapter(string.Format(
                    @"select PERCENT_PUNISHMENT from {0}.PERCENT_PUNISHMENT ORDER BY PERCENT_PUNISHMENT",
                    Connect.Schema), Connect.CurConnect).Fill(_dsViolation_Emp, "PERCENT_PUNISHMENT");

                // Select
                _daViolation_Emp = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectViolation_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daViolation_Emp.SelectCommand.BindByName = true;
                _daViolation_Emp.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = per_num;
                _daViolation_Emp.Fill(_dsViolation_Emp.Tables["VIOLATION_EMP"]);
                // Insert
                _daViolation_Emp.InsertCommand = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.VIOLATION_EMP_UPDATE(:VIOLATION_EMP_ID, :TRANSFER_ID, :PER_NUM, :DETENTION_DATE, :REASON_DETENTION_ID, 
                            :PUNISHMENT_NUM_ORDER, :PUNISHMENT_DATE_ORDER, :COUNT_DAYS, :NOTE);
                    END;", Connect.Schema), Connect.CurConnect);
                _daViolation_Emp.InsertCommand.BindByName = true;
                _daViolation_Emp.InsertCommand.Parameters.Add("VIOLATION_EMP_ID", OracleDbType.Decimal, 0, "VIOLATION_EMP_ID").Direction = 
                    ParameterDirection.InputOutput;
                _daViolation_Emp.InsertCommand.Parameters["VIOLATION_EMP_ID"].DbType = DbType.Decimal;
                _daViolation_Emp.InsertCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
                _daViolation_Emp.InsertCommand.Parameters.Add("PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM");
                _daViolation_Emp.InsertCommand.Parameters.Add("DETENTION_DATE", OracleDbType.Date, 0, "DETENTION_DATE");
                _daViolation_Emp.InsertCommand.Parameters.Add("REASON_DETENTION_ID", OracleDbType.Decimal, 0, "REASON_DETENTION_ID");
                //_daViolation_Emp.InsertCommand.Parameters.Add("TYPE_PUNISHMENT_ID", OracleDbType.Decimal, 0, "TYPE_PUNISHMENT_ID");
                //_daViolation_Emp.InsertCommand.Parameters.Add("PERCENT_PUNISHMENT", OracleDbType.Decimal, 0, "PERCENT_PUNISHMENT").Direction =
                //    ParameterDirection.InputOutput;
                //_daViolation_Emp.InsertCommand.Parameters["VIOLATION_EMP_ID"].DbType = DbType.Decimal;
                _daViolation_Emp.InsertCommand.Parameters.Add("PUNISHMENT_NUM_ORDER", OracleDbType.Varchar2, 0, "PUNISHMENT_NUM_ORDER");
                _daViolation_Emp.InsertCommand.Parameters.Add("PUNISHMENT_DATE_ORDER", OracleDbType.Date, 0, "PUNISHMENT_DATE_ORDER");
                _daViolation_Emp.InsertCommand.Parameters.Add("COUNT_DAYS", OracleDbType.Decimal, 0, "COUNT_DAYS");
                _daViolation_Emp.InsertCommand.Parameters.Add("NOTE", OracleDbType.Varchar2, 0, "NOTE");
                // Update
                _daViolation_Emp.UpdateCommand = _daViolation_Emp.InsertCommand;
                // Delete
                _daViolation_Emp.DeleteCommand = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.VIOLATION_EMP_DELETE(:VIOLATION_EMP_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _daViolation_Emp.DeleteCommand.BindByName = true;
                _daViolation_Emp.DeleteCommand.Parameters.Add("VIOLATION_EMP_ID", OracleDbType.Decimal, 0, "VIOLATION_EMP_ID");

                dgViolation_Emp.AutoGenerateColumns = false;
                dgViolation_Emp.DataSource = _dsViolation_Emp.Tables["VIOLATION_EMP"].DefaultView;
                //dcREASON_DETENTION_ID.DataSource = _dsViolation_Emp.Tables["REASON_DETENTION"].DefaultView;
                //dcREASON_DETENTION_ID.DisplayMember = "REASON_DETENTION_NAME";
                //dcREASON_DETENTION_ID.ValueMember = "REASON_DETENTION_ID";
                //dcTYPE_PUNISHMENT_ID.DataSource = _dsViolation_Emp.Tables["TYPE_PUNISHMENT"].DefaultView;
                //dcTYPE_PUNISHMENT_ID.DisplayMember = "TYPE_PUNISHMENT_NAME";
                //dcTYPE_PUNISHMENT_ID.ValueMember = "TYPE_PUNISHMENT_ID";
                //dcPERCENT_PUNISHMENT.DataSource = _dsViolation_Emp.Tables["PERCENT_PUNISHMENT"].DefaultView;
                //dcPERCENT_PUNISHMENT.DisplayMember = "PERCENT_PUNISHMENT";
                //dcPERCENT_PUNISHMENT.ValueMember = "PERCENT_PUNISHMENT";
            }
        }

        // Добавление данных Нарушения
        private void btAddViolation_Emp_Click(object sender, EventArgs e)
        {
            DataRowView _drView = _dsViolation_Emp.Tables["VIOLATION_EMP"].DefaultView.AddNew();
            _drView["PER_NUM"] = per_num;
            _drView["TRANSFER_ID"] = transfer_id;
            _dsViolation_Emp.Tables["VIOLATION_EMP"].Rows.Add(_drView.Row);
            Violation_Emp violation_emp = new Violation_Emp(_drView, _dsViolation_Emp, this);
            if (violation_emp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveViolation_Emp();
            }            
            _dsViolation_Emp.Tables["VIOLATION_EMP"].Clear();
            _daViolation_Emp.Fill(_dsViolation_Emp.Tables["VIOLATION_EMP"]);
        }

        // Редактирование данных Нарушения
        private void btEditViolation_Emp_Click(object sender, EventArgs e)
        {
            if (dgViolation_Emp.CurrentRow == null)
                return;
            if (Convert.ToDecimal(dgViolation_Emp.CurrentRow.Cells["VIOLATION_EMP_ID"].Value) == -10)
            {
                MessageBox.Show("Нельзя редактировать данное нарушение!\nОно создано в ПО \"Нарушители режима\"", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataRowView _drView = dgViolation_Emp.CurrentRow.DataBoundItem as DataRowView;
            Violation_Emp violation_emp = new Violation_Emp(_drView, _dsViolation_Emp, this);
            if (violation_emp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveViolation_Emp();
            }
            _dsViolation_Emp.Tables["VIOLATION_EMP"].Clear();
            _daViolation_Emp.Fill(_dsViolation_Emp.Tables["VIOLATION_EMP"]);
        }

        // Уданение данных Нарушения
        private void btDeleteViolation_Emp_Click(object sender, EventArgs e)
        {
            if (dgViolation_Emp.CurrentRow == null)
                return;
            if (Convert.ToDecimal(dgViolation_Emp.CurrentRow.Cells["VIOLATION_EMP_ID"].Value) == -10)
            {
                MessageBox.Show("Нельзя удалить данное нарушение!\nОно создано в ПО \"Нарушители режима\"", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Удалить запись?", "АРМ 'Кадры'", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgViolation_Emp.Rows.Remove(dgViolation_Emp.CurrentRow);
                SaveViolation_Emp();
            }
        }

        public void SaveViolation_Emp()
        {
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _daViolation_Emp.InsertCommand.Transaction = transact;
                _daViolation_Emp.UpdateCommand.Transaction = transact;
                _daViolation_Emp.DeleteCommand.Transaction = transact;
                _daViolation_Emp.Update(_dsViolation_Emp.Tables["VIOLATION_EMP"]);
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void dgViolation_Emp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgViolation_Emp["VIOLATION_EMP_ID", e.RowIndex].Value != DBNull.Value)
            {
                if (Convert.ToDecimal(dgViolation_Emp["VIOLATION_EMP_ID", e.RowIndex].Value) == -10)
                {
                    // Красим в серый цвет те строчки, которые идут из ПО Нарушители режима
                    e.CellStyle.BackColor = Color.Gainsboro;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        # endregion

        /// <summary>
        /// Событие нажатия кнопки формирования приказа о приеме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void btOrder_Click(object sender, EventArgs e)
        {
            if (passport.Count != 0)
            {
                if (((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).TYPE_PER_DOC_ID == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).SERIA_PASSPORT == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).NUM_PASSPORT == null)
                    MessageBox.Show("Вы не ввели данные о документе личности.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (tbCode_Subdiv.Text == "" || tbCode_Pos.Text == "")
                {
                    MessageBox.Show("Вы не ввели должность или подразделение.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    TRANSFER_seq transferHire = new TRANSFER_seq(Connect.CurConnect);
                    /// Определяем добавление данных или редактирование
                    if (!flagTransfer)
                    {
                        transferHire.Fill(string.Format(" where PER_NUM = '{1}' and SIGN_COMB = 0 and TYPE_TRANSFER_ID = 1 and DATE_HIRE = " +
                            "(select max(DATE_HIRE) from {0}.transfer where PER_NUM = '{1}' and SIGN_COMB = 0 and TYPE_TRANSFER_ID = 1)",
                            Connect.Schema, per_num));
                        /// Если нет данных по основной работе выводим сообщение
                        if (transferHire.Count == 0)
                        {
                            ////transfer.Fill(string.Format(" where {0} = '{1}' and {2} = 1 and {3} = 1 and {4} = " +
                            ////    "(select max({4}) from {5}.transfer where {0} = '{1}' and {2} = 1 and {3} = 1)",
                            ////    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_COMB,
                            ////    TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.DATE_HIRE, Staff.DataSourceScheme.SchemeName));
                            //MessageBox.Show("Данный работник не был принят на основную должность.\nВозможно он совместитель.",
                            //    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //return;
                            transferHire.AddNew();
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).PER_NUM = per_num;
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).TYPE_TRANSFER_ID = 1;
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).SUBDIV_ID = 0;
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).POS_ID = 0;
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferHire]).Current)).SIGN_COMB = false;
                        }
                        else
                        {
                            
                            cbPos_Name.DataBindings.Clear();
                            cbPos_Name.DataSource = null;
                            cbSubdiv_Name.DataBindings.Clear();
                            cbSubdiv_Name.DataSource = null;
                            
                            // 12,12,2016 - Убираем данное обновление справочников
                            //// Добавлено 13,03,2013 для того, чтобы в приказе о приеме отображались старые должности и подразделения
                            //// зачем здесь косяк был время : 22,03,2013 8:48
                            //AppDataSet.position.Clear();
                            //AppDataSet.position.Fill(string.Format("where POS_ACTUAL_SIGN = 1 " +
                            //    "union " +
                            //    "SELECT case POS_CHIEF_OR_DEPUTY_SIGN when 1 then 'True' else 'False' end as \"POS_CHIEF_OR_DEPUTY_SIGN\",POS_ID,CODE_POS,POS_NAME, " +
                            //    "case POS_ACTUAL_SIGN when 1 then 'True' else 'False' end as \"POS_ACTUAL_SIGN\", " +
                            //    "POS_DATE_START,POS_DATE_END,FROM_POS_ID " +
                            //    "FROM {0}.POSITION tab1 where POS_ID = {1} " +
                            //    "order by POS_NAME", Connect.Schema,
                            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferHire]).Current).POS_ID));
                            //AppDataSet.subdiv.Clear();
                            //AppDataSet.subdiv.Fill(string.Format(
                            //    @"where SUB_ACTUAL_SIGN = 1 and WORK_TYPE_ID != 7 
                            //    union 
                            //    select TYPE_SUBDIV_ID,SUBDIV_ID,CODE_SUBDIV,SUBDIV_NAME, case SUB_ACTUAL_SIGN when 1 then 'True' else 'False' end as SUB_ACTUAL_SIGN,
                            //        WORK_TYPE_ID,SERVICE_ID,SUB_DATE_START,SUB_DATE_END,PARENT_ID,FROM_SUBDIV,GR_WORK_ID
                            //    from {0}.SUBDIV tab1 where SUBDIV_ID = {1} 
                            //    order by SUBDIV_NAME", Connect.Schema,
                            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferHire]).Current).SUBDIV_ID));
                            //// Привязка комбобоксов к данным
                            //// добавил 
                                
                            //transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID, p_transfer_id));
                            if (!flagArchive)
                            {
                                cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID,
                                    new LinkArgument(AppDataSet.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                                cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID,
                                    new LinkArgument(AppDataSet.position, POSITION_seq.ColumnsName.POS_NAME));
                            }
                            else
                            {
                                cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID,
                                    new LinkArgument(AppDataSet.allSubdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                                cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID,
                                    new LinkArgument(AppDataSet.allPosition, POSITION_seq.ColumnsName.POS_NAME));
                            }
                        }
                    }
                    else
                    {
                        transferHire = transfer;                        
                    }
                    Hire_Order order = new Hire_Order(emp, transferHire);
                    order.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Событие изменения индекса типа документа личности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbType_Per_Doc_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType_Per_Doc_ID.SelectedValue != null)
            {
                r_passport = passport.Where(s => s is PASSPORT_obj).FirstOrDefault();
                type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
                type_per_doc.Fill(" where type_per_doc_id = " + cbType_Per_Doc_ID.SelectedValue.ToString());
                TYPE_PER_DOC_obj r_type_per_doc = type_per_doc.Where(s => s is TYPE_PER_DOC_obj).FirstOrDefault();
                mbSeria_Passport.Mask = r_type_per_doc.TEMPL_SER;
                mbNum_Passport.Mask = r_type_per_doc.TEMPL_NUM;
            }
        }        

        /// <summary>
        /// Событие изменения индекса подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = Library.CodeBySelectedValue(Connect.CurConnect, POSITION_seq.ColumnsName.CODE_POS.ToString(), Staff.DataSourceScheme.SchemeName,
                    "position", POSITION_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, tbCode_Pos.Text.Trim().Length, Connect.CurConnect, e, POSITION_seq.ColumnsName.POS_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);      
        } 

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonalCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!GrantedRoles.GetGrantedRole("STAFF_VIEW"))
            {
                emp.RollBack();
                per_data.RollBack();
                passport.RollBack();
                registr.RollBack();
                habit.RollBack();
                Connect.Rollback();
                if (flagAdd && !flagSave)
                {
                    per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                    per_num_book.Fill(string.Format("where {0} = {1}", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                    ((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = true;
                    per_num_book.Save();
                    Connect.Commit();
                }
                try
                {
                    if (listEmp != null && !flagArchive)
                    {
                        listEmp.bsEmp.PositionChanged -= listEmp.PositionChange;
                        int pos = listEmp.bsEmp.Position;
                        //listEmp.dtEmp.RefreshRow(listEmp.dtEmp.Rows[listEmp.bsEmp.Position]);
                        listEmp.dtEmp.Clear();
                        listEmp.dtEmp.Fill();
                        listEmp.RefreshGridEmp();
                        listEmp.bsEmp.PositionChanged += new EventHandler(listEmp.PositionChange);
                        listEmp.bsEmp.Position = pos;
                        //listEmp.dtEmp.RefreshRow(listEmp.dtEmp.Rows[listEmp.bsEmp.Position]);
                    }
                }
                catch
                {
                    //MessageBox.Show( ex.Message);
                }
            }
            if (Connect.Transact != null && Connect.Transact.Connection != null)
                Connect.Transact.Rollback();
            Connect.Commit();
            #region Старая версия
            //// Проверка изменения данных
            //if (emp.IsDataChanged() || per_data.IsDataChanged())
            //{
            //    DialogResult dr = MessageBox.Show("Были сделаны изменения личных данных работника.\nСохранить внесенные изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (dr == DialogResult.Yes)
            //    {
            //        emp.Save();
            //        per_data.Save();
            //        connection.Commit();
            //    }
            //    else if (dr == DialogResult.No)
            //    {
            //        emp.RollBack();
            //        per_data.RollBack();
            //        connection.Rollback();
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //if (passport.IsDataChanged())
            //{
            //    DialogResult dr = MessageBox.Show("Были сделаны изменения паспортных данных. \nСохранить внесенные изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (dr == DialogResult.Yes)
            //    {
            //        passport.Save();
            //        connection.Commit();
            //    }
            //    else if (dr == DialogResult.No)
            //    {
            //        passport.RollBack();
            //        connection.Rollback();
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //if (registr.IsDataChanged())
            //{
            //    DialogResult dr = MessageBox.Show("Были сделаны изменения данных места прописки.\nСохранить внесенные изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (dr == DialogResult.Yes)
            //    {
            //        registr.Save();
            //        connection.Commit();
            //    }
            //    else if (dr == DialogResult.No)
            //    {
            //        registr.RollBack();
            //        connection.Rollback();
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //if (habit.IsDataChanged())
            //{
            //    DialogResult dr = MessageBox.Show("Были сделаны изменения данных места проживания.\nСохранить внесенные изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (dr == DialogResult.Yes)
            //    {
            //        habit.Save();
            //        connection.Commit();
            //    }
            //    else if (dr == DialogResult.No)
            //    {
            //        habit.RollBack();
            //        connection.Rollback();
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //if (per_num_book != null)
            //    per_num_book.Clear();
            //if (mar_state != null)                
            //    mar_state.Clear();
            //if (source_complect != null)
            //    source_complect.Clear();
            //if (type_per_doc != null)
            //    type_per_doc.Clear();
            //if (speciality != null)
            //    speciality.Clear();
            //if (instit != null)
            //    instit.Clear();
            //if (qual != null)
            //    qual.Clear();
            //if (group_spec != null)
            //    group_spec.Clear();
            //if (type_study != null)
            //    type_study.Clear();
            //if (type_edu != null)
            //    type_edu.Clear();
            //if (rel_type != null)
            //    rel_type.Clear();
            //if (mil_rank != null)
            //    mil_rank.Clear();
            //if (med_classif != null)
            //    med_classif.Clear();
            //if (comm != null)
            //    comm.Clear();
            //if (type_troop != null)
            //    type_troop.Clear();
            //if (mil_cat != null)
            //    mil_cat.Clear();
            //if (lang != null)
            //    lang.Clear();
            //if (level_know != null)
            //    level_know.Clear();
            //if (base_doc != null)
            //    base_doc.Clear();
            //if (type_priv != null)
            //    type_priv.Clear();
            //if (type_vac != null)
            //    type_vac.Clear();
            //if (type_rise_qual != null)
            //    type_rise_qual.Clear();
            //if (type_postg_study != null)
            //    type_postg_study.Clear();
            //if (source_fill != null)
            //    source_fill.Clear();
            //if (emp != null)
            //    emp.Clear();
            //if (per_data != null)
            //    per_data.Clear();
            //if (passport != null)
            //    passport.Clear();
            //if (transfer != null)
            //    transfer.Clear();
            //if (registr != null)
            //    registr.Clear();
            //if (habit != null)
            //    habit.Clear();
            //if (edu != null)
            //    edu.Clear();
            //if (relative != null)
            //    relative.Clear();
            //if (mil_card != null)
            //    mil_card.Clear();
            //if (prev_work != null)
            //    prev_work.Clear();
            //if (know_lang != null)
            //    know_lang.Clear();
            //if (reward != null)
            //    reward.Clear();
            //if (emp_priv != null)
            //    emp_priv.Clear();
            //if (vac != null)
            //    vac.Clear();
            //if (attest != null)
            //    attest.Clear();
            //if (prof_train != null)
            //    prof_train.Clear();
            //if (rise_qual != null)
            //    rise_qual.Clear();
            //if (postg_study != null)
            //    postg_study.Clear();
            //if (rregion != null)
            //    rregion.Clear();
            //if (hregion != null)
            //    hregion.Clear();
            //if (rdistrict != null)
            //    rdistrict.Clear();
            //if (hdistrict != null)
            //    hdistrict.Clear();
            //if (rcity != null)
            //    rcity.Clear();
            //if (hcity != null)
            //    hcity.Clear();
            //if (rlocality != null)
            //    rlocality.Clear();
            //if (hlocality != null)
            //    hlocality.Clear();
            //if (rstreet != null)
            //    rstreet.Clear();
            //if (hstreet != null)
            //    hstreet.Clear();
            ////if (subdiv != null)
            ////    subdiv.Clear();
            ////if (position != null)
            ////    position.Clear();
            #endregion 
        }

        /// <summary>
        /// Метод проверяет табельный номер по базе данных работников
        /// </summary>
        void EditPer_num()
        {
            // Проверка существует ли введенный табельный номер в табельной книге
            if (tbPer_Num.Text.Trim() != "")
            {
                per_num = tbPer_Num.Text.Trim();
                PER_NUM_BOOK_seq per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                per_num_book.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                if (per_num_book.Count != 0)
                {
                    per_num_book.Fill(string.Format(" where {0} = 1 and {1} = '{2}'",
                        PER_NUM_BOOK_seq.ColumnsName.FREE_SIGN, PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                    // Если табельный свободен добавляем работника
                    if (per_num_book.Count != 0)
                    {
                        ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM = per_num;
                        tbPer_Num.Text = per_num;
                        ((PER_DATA_obj)(((CurrencyManager)BindingContext[per_data]).Current)).PER_NUM = per_num;
                        ((PASSPORT_obj)(((CurrencyManager)BindingContext[passport]).Current)).PER_NUM = per_num;
                        ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).PER_NUM = per_num;
                        ((HABIT_obj)(((CurrencyManager)BindingContext[habit]).Current)).PER_NUM = per_num;
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM = per_num;
                        ((MIL_CARD_obj)(((CurrencyManager)BindingContext[mil_card]).Current)).PER_NUM = per_num; 
                    }
                    else
                    {
                        TRANSFER_seq transferPer_Num = new TRANSFER_seq(Connect.CurConnect);
                        //string textBlock = string.Format(Queries.GetQuery("SelectCurrentTransfer.sql"),
                        //    Staff.DataSourceScheme.SchemeName, "transfer",
                        //    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.PER_NUM,
                        //    TRANSFER_seq.ColumnsName.DATE_TRANSFER, TRANSFER_seq.ColumnsName.SIGN_COMB,
                        //    TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, tbPer_Num.Text.Trim(),
                        //    TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_COMB);
                        string textBlock = string.Format(Queries.GetQuery("SelectCurrentTransfer.sql"),
                            Staff.DataSourceScheme.SchemeName, "transfer",
                            TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.PER_NUM,
                            TRANSFER_seq.ColumnsName.DATE_TRANSFER, TRANSFER_seq.ColumnsName.SIGN_COMB,
                            TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, tbPer_Num.Text.Trim(),
                            TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_COMB,
                            TRANSFER_seq.ColumnsName.DATE_HIRE);
                        transferPer_Num.Fill(textBlock);
                        if (transferPer_Num.Count != 0)
                        {
                            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transferPer_Num]).Current).TYPE_TRANSFER_ID == 3)
                            {
                                string str;
                                emp.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                if (((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_SEX == "М" || ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_SEX == "M")
                                { str = "Уволенный "; }
                                else
                                { str = "Уволенная "; }
                                string str1 = string.Format("{0} {1} {2} имеет таб.№ {3}.\nВосстановить старые данные \n(это займет некоторое время)?",
                                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_LAST_NAME,
                                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_FIRST_NAME,
                                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).EMP_MIDDLE_NAME,
                                        tbPer_Num.Text);
                                str = String.Concat(str, str1);
                                if (MessageBox.Show(str, "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    //f_OldEmp = true;
                                    emp.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    per_data.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    passport.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    mil_card.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    transfer.AddNew();
                                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM = per_num;
                                    registr.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    //FormRegistr();
                                    string str2 = ((REGISTR_obj)((CurrencyManager)BindingContext[registr]).Current).REG_CODE_STREET;
                                    if (str2 != null)
                                    {
                                        formregistr.LoadAddress(str2);
                                        formregistr.DisableAll(true, Color.White);
                                    }
                                    habit.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    //FormHabit();
                                    string str3 = ((HABIT_obj)((CurrencyManager)BindingContext[habit]).Current).HAB_CODE_STREET;
                                    if (str3 != null)
                                    {
                                        formhabit.LoadAddress(str3);
                                        formhabit.DisableAll(true, Color.White);
                                    }
                                }
                                else
                                {
                                    emp.Clear();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Невозможно произвести операцию!\nВведенный табельный номер занят другим работником.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[emp]).Refresh();
                                ((PER_DATA_obj)(((CurrencyManager)BindingContext[per_data]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[per_data]).Refresh();
                                ((PASSPORT_obj)(((CurrencyManager)BindingContext[passport]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[passport]).Refresh();
                                ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[registr]).Refresh();
                                ((HABIT_obj)(((CurrencyManager)BindingContext[habit]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[habit]).Refresh();
                                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[transfer]).Refresh();
                                ((MIL_CARD_obj)(((CurrencyManager)BindingContext[mil_card]).Current)).PER_NUM = "";
                                ((CurrencyManager)BindingContext[mil_card]).Refresh();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно произвести операцию!\nВведенный табельный номер не существует в табельной книге.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbPer_Num.Text = "";
                    return;
                }
            }
        }

        /// <summary>
        /// Событие проверки действительности текста табельного номера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPer_Num_Validating(object sender, CancelEventArgs e)
        {
            //EditPer_num();
        }

        /// <summary>
        /// Метод развертывает или свертывает ричтекстбокс в зависимости от его состояния
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="flag"></param>
        void RichTextBoxClick(RichTextBox rt, ref bool flag)
        {
            if (!flag)
            {
                rt.Height = 63;
                rt.ScrollBars = RichTextBoxScrollBars.None;
                flag = true;
            }
            else
            {
                rt.Height = 21;
                rt.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
                flag = false;
            }
        }

        /// <summary>
        /// Событие нажатия на ричтекстбокс специального учета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtSpecial_MouseClick(object sender, MouseEventArgs e)
        {
            RichTextBoxClick((RichTextBox)sender, ref fSpecial);
        }

        /// <summary>
        /// Событие нажатия на ричтекстбокс мобилизационного предписания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtMob_Order_MouseClick(object sender, MouseEventArgs e)
        {
            RichTextBoxClick((RichTextBox)sender, ref fMob_Order);
        }

        /// <summary>
        /// Событие нажатия на ричтекстбокс места прохождения службы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtPlace_Service_MouseClick(object sender, MouseEventArgs e)
        {
            RichTextBoxClick((RichTextBox)sender, ref fPlace_Service);
        }

        /// <summary>
        /// Событие нажатия на ричтекстбокс причины непрохождения службы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtMatter_No_Service_MouseClick(object sender, MouseEventArgs e)
        {
            RichTextBoxClick((RichTextBox)sender, ref fMatter_No_Service);
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования фото сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditPhoto_Click(object sender, EventArgs e)
        {
            if (ofdAddPhoto.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(ofdAddPhoto.FileName);
                string pathTemp = Application.UserAppDataPath + file.Name;
                file.CopyTo(pathTemp, true);
                EmployeePhoto.SetPhoto(per_num, Connect.CurConnect, pathTemp);
                Connect.Commit();
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                    r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { Photo = pathTemp });
                pbPhoto.Image = EmployeePhoto.GetPhoto(per_num);
            }         
        }

        /// <summary>
        /// Перевод фамилии в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSername_Validating(object sender, CancelEventArgs e)
        {
            if (tbSername.Text.Trim().Length == 1)
            {
                MessageBox.Show("Проверьте ввод данных. Длина введенной фамилии равна 1.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            tbSername.Text = tbSername.Text.ToUpper();
        }

        /// <summary>
        /// Перевод имени в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbName_Validating(object sender, CancelEventArgs e)
        {
            if (tbName.Text.Trim().Length == 1)
            {
                MessageBox.Show("Проверьте ввод данных. Длина введенного ммени равна 1.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            tbName.Text = tbName.Text.ToUpper();
        }

        /// <summary>
        /// Перевод отчества в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMiddle_Name_Validating(object sender, CancelEventArgs e)
        {
            if (tbMiddle_Name.Text.Trim().Length == 1)
            {
                MessageBox.Show("Проверьте ввод данных. Длина введенного отчества равна 1.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            tbMiddle_Name.Text = tbMiddle_Name.Text.ToUpper();
        }

        /// <summary>
        /// Событие нажатия кнопки формирования приказа о приеме по совмещению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHireComb_Click(object sender, EventArgs e)
        {
            if (passport.Count != 0)
            {
                if (((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).TYPE_PER_DOC_ID == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).SERIA_PASSPORT == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).NUM_PASSPORT == null)
                    MessageBox.Show("Вы не ввели данные о документе личности.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (tbCode_Subdiv.Text == "" || tbCode_Pos.Text == "")
                {
                    MessageBox.Show("Вы не ввели должность или подразделение.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    ListComb listComb = new ListComb(emp);
                    listComb.ShowDialog();

                    ///// Создаем и заполняем таблицу переводов данными по совмещению.
                    ///// Если данные отсутствуют, добавляем новую запись.
                    //TRANSFER_seq transferComb = new TRANSFER_seq(connection);
                    //transferComb.Fill(string.Format(" where {0} = '{1}' and {2} = 1 and {3} = 1 and {4} = " +
                    //    "(select max({4}) from {5}.transfer where {0} = '{1}' and {2} = 1 and {3} = 1) and not exists " +
                    //    "(select null from {5}.transfer where {0} = '{1}' and {2} = 1 and {3} = 3 and {4} = " +
                    //    "(select max({4}) from {5}.transfer where {0} = '{1}' and {2} = 1)) ",
                    //    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_COMB,
                    //    TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.DATE_TRANSFER, Staff.DataSourceScheme.SchemeName));
                    //if (transferComb.Count == 0)
                    //{
                    //    transferComb.AddNew();
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).PER_NUM = per_num;
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).TYPE_TRANSFER_ID = 1;
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).SUBDIV_ID = 0;
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).POS_ID = 0;
                    //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferComb]).Current)).SIGN_COMB = true;
                    //}
                    //Hire_Order order = new Hire_Order(connection, emp, transferComb);
                    //order.ShowDialog();
                }
            }
        }

        /// <summary>
        /// При потере фокуса нужно переводить в верхний регистр поле Кем выдан
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWho_Given_Validating(object sender, CancelEventArgs e)
        {
            tbWho_Given.Text = tbWho_Given.Text.Replace("\n", " ").Replace("\r", " ").ToUpper();
        }

        /// <summary>
        /// Проверка на ввод ИНН
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbInn_Validating(object sender, CancelEventArgs e)
        {
            if (mbInn.Text.Trim().Length != 12)
            {
                MessageBox.Show("Вы не полностью ввели ИНН!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        #region Работа с фото сотрудника
        /// <summary>
        /// Событие нажатия кнопки редактирования фото сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClearPhoto_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите очистить фото в Перко?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string pathTemp = Application.StartupPath + "\\temp.jpg";
                FileInfo file = new FileInfo(pathTemp);
                file.CopyTo(Application.UserAppDataPath + "\\tempPhoto.jpg", true);
                //EmployeePhoto.SetPhoto(per_num, connection, Application.UserAppDataPath + "\\tempPhoto.jpg");
                //connection.Commit();
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                    r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { Photo = Application.UserAppDataPath + "\\tempPhoto.jpg" });
                //pbPhoto.Image = EmployeePhoto.GetPhoto(per_num, connection);
                MessageBox.Show("Фото в Перко обновлено!");
            }         
        }

        /// <summary>
        /// Печатать фото
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPrintPhoto_Click(object sender, EventArgs e)
        {
            PrintDialog pdPhoto = new PrintDialog();
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            pdPhoto.Document = pd;
            DialogResult rez = pdPhoto.ShowDialog();
            if (rez == DialogResult.OK)
            {
                pd.Print();
            }      
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pbPhoto.Image, new Rectangle(1, 1, 160, Convert.ToInt32(160*1.34)));
        }
        #endregion

        private void btEditForPrivPos_Click(object sender, EventArgs e)
        {
            EditSignPrivPos editSignPrivPos = new EditSignPrivPos(
                (decimal)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID);
            editSignPrivPos.ShowInTaskbar = false;
            editSignPrivPos.ShowDialog();
        }

        private void btSavePhoto_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdPhoto = new SaveFileDialog();
            sfdPhoto.DefaultExt = "jpg";
            sfdPhoto.AddExtension = true;
            DialogResult res = sfdPhoto.ShowDialog();
            if (res == DialogResult.OK)
            {
                Bitmap bitmap = (Bitmap)pbPhoto.Image;
                Bitmap newBitmap = new Bitmap(bitmap);
                newBitmap.SetResolution(200, 200);
                newBitmap.Save(sfdPhoto.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                //pbPhoto.Image.Save(sfdPhoto.FileName);
            }
        }

        private void btAddress_None_Kladr_Click(object sender, EventArgs e)
        {
            Address_None_Kladr addr = new Address_None_Kladr();
            addr.ShowDialog();
            tbHab_Non_Kladr_Address.Text = Address_None_Kladr.Address_None_Kladr_Property;
        }

    }
}
