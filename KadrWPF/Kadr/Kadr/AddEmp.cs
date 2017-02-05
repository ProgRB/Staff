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

using System.Collections;
using LibraryKadr;

namespace Kadr
{
    public partial class AddEmp : Form
    {
        // Объявление строки подключения
        OracleConnection connection;

        // Объявление справочных таблиц
        PER_NUM_BOOK_seq per_num_book;
        MAR_STATE_seq mar_state;
        SOURCE_COMPLECT_seq source_complect;
        TYPE_PER_DOC_seq type_per_doc;
        REGION_seq rregion, hregion;
        DISTRICT_seq rdistrict, hdistrict;
        CITY_seq rcity, hcity;
        LOCALITY_seq rlocality, hlocality;
        STREET_seq rstreet, hstreet;
        SUBDIV_seq subdiv;
        POSITION_seq position;
        SOURCE_FILL_seq source_fill;

        // Объявление рабочих таблиц
        EMP_seq emp;
        PER_DATA_seq per_data;
        PASSPORT_seq passport;
        TRANSFER_seq transfer;
        REGISTR_seq registr;
        HABIT_seq habit;

        // Объявление строк для таблиц
        PER_NUM_BOOK_obj r_per_num_book;
        TYPE_PER_DOC_obj r_type_per_doc;

        // Объявление временной таблицы Пол работника
        DataTable Sex_Table;
        Address formhabit;
        Address formregistr;

        // Переменная указывает было ли событие активации формы
        bool focusPer_num, f_OldEmp = false;

        // Табельный номер и имя схемы
        string per_num;

        /// <summary>
        /// Конструктор формы личной карточки работника
        /// </summary>
        /// <param name="_connection">Строка подключения</param> 
        public AddEmp(OracleConnection _connection)
        {
            InitializeComponent();
            connection = _connection;

            // Создание и заполнение таблиц данными
            per_num_book = new PER_NUM_BOOK_seq(connection);
            per_num_book.Fill(string.Format("where rownum = 1 and {0} = 1 order by {1}", 
                PER_NUM_BOOK_seq.ColumnsName.FREE_SIGN.ToString(), PER_NUM_BOOK_seq.ColumnsName.PER_NUM.ToString()));
            per_num = per_num_book[0].PER_NUM;
            mar_state = new MAR_STATE_seq(connection);
            mar_state.Fill(string.Format("order by {0}", MAR_STATE_seq.ColumnsName.NAME_STATE.ToString()));
            source_complect = new SOURCE_COMPLECT_seq(connection);
            source_complect.Fill(string.Format("order by {0}", SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME.ToString()));            
            type_per_doc = new TYPE_PER_DOC_seq(connection);
            type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC.ToString()));
            subdiv = new SUBDIV_seq(connection);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
                SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            position = new POSITION_seq(connection);
            position.Fill(string.Format("where {0} = 1 order by {1}", POSITION_seq.ColumnsName.POS_ACTUAL_SIGN.ToString(),
                POSITION_seq.ColumnsName.POS_NAME.ToString()));
            rregion = new REGION_seq(connection);
            rregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION.ToString()));
            hregion = new REGION_seq(connection);
            hregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION.ToString()));
            rdistrict = new DISTRICT_seq(connection);
            rcity = new CITY_seq(connection);
            rlocality = new LOCALITY_seq(connection);
            rstreet = new STREET_seq(connection);
            hdistrict = new DISTRICT_seq(connection);
            hcity = new CITY_seq(connection);
            hlocality = new LOCALITY_seq(connection);
            hstreet = new STREET_seq(connection);

            emp = new EMP_seq(connection);
            per_data = new PER_DATA_seq(connection);
            passport = new PASSPORT_seq(connection);
            transfer = new TRANSFER_seq(connection);
            registr = new REGISTR_seq(connection);
            habit = new HABIT_seq(connection);
            
            // Заполнение комбобоксов значениями из справочников
            cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID, new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedItem = null;
            cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID, new LinkArgument(position, POSITION_seq.ColumnsName.POS_NAME));
            cbPos_Name.SelectedItem = null;
            cbMar_State_ID.AddBindingSource(passport, MAR_STATE_seq.ColumnsName.MAR_STATE_ID, new LinkArgument(mar_state, MAR_STATE_seq.ColumnsName.NAME_STATE));
            cbMar_State_ID.SelectedItem = null;
            cbType_Per_Doc_ID.AddBindingSource(passport, TYPE_PER_DOC_seq.ColumnsName.TYPE_PER_DOC_ID, new LinkArgument(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            cbType_Per_Doc_ID.SelectedItem = null;
            cbSource_ID.AddBindingSource(transfer, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_ID, new LinkArgument(source_complect, SOURCE_COMPLECT_seq.ColumnsName.SOURCE_NAME));
            cbSource_ID.SelectedItem = null;        
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbType_Per_Doc_ID.SelectedIndexChanged += new EventHandler(cbType_Per_Doc_ID_SelectedIndexChanged);

            // Создание и заполнение таблицы Пол сотрудника
            Sex_Table = new DataTable();
            Sex_Table.Columns.Add("sex_emp", typeof(string));
            Sex_Table.Rows.Add("М");
            Sex_Table.Rows.Add("Ж");
            BindingSource bssex = new BindingSource();
            bssex.DataSource = Sex_Table;

            // Заполнение комбобокса Пол сотрудника
            cbSex.DataBindings.Add("selectedvalue", emp, "emp_sex");
            cbSex.DataSource = bssex;
            cbSex.DisplayMember = "sex_emp";
            cbSex.ValueMember = "sex_emp";
            cbSex.SelectedItem = null;
            
            // Привязка тесктбоксов к полям таблиц
            tbPer_Num.AddBindingSource(emp,EMP_seq.ColumnsName.PER_NUM);
            tbEmp_Last_Name.AddBindingSource(emp,EMP_seq.ColumnsName.EMP_LAST_NAME);
            tbEmp_First_Name.AddBindingSource(emp,EMP_seq.ColumnsName.EMP_FIRST_NAME);
            tbEmp_Middle_Name.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            mbInsurance_Num.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INSURANCE_NUM);
            mbInn.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.INN);
            tbSer_Med_Polus.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.SER_MED_POLUS);
            tbNum_Med_Polus.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.NUM_MED_POLUS);
            mbSeria_Passport.AddBindingSource(passport, PASSPORT_seq.ColumnsName.SERIA_PASSPORT);
            mbNum_Passport.AddBindingSource(passport, PASSPORT_seq.ColumnsName.NUM_PASSPORT);
            tbWho_Given.AddBindingSource(passport, PASSPORT_seq.ColumnsName.WHO_GIVEN);
            tbCitizenship.AddBindingSource(passport, PASSPORT_seq.ColumnsName.CITIZENSHIP);
            tbCountry_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.COUNTRY_BIRTH);
            tbRegion_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.REGION_BIRTH);
            tbCity_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.CITY_BIRTH);
            tbDistr_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.DISTR_BIRTH);
            tbLocality_Birth.AddBindingSource(passport, PASSPORT_seq.ColumnsName.LOCALITY_BIRTH);

            // Привязка чекбоксов к полям таблиц
            chTrip_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.TRIP_SIGN);
            chRetiree_Sign.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.RETIRER_SIGN);
            chSign_ProfUnion.AddBindingSource(per_data, PER_DATA_seq.ColumnsName.SIGN_PROFUNION);

            // Привязка дат
            mbWhen_Given.AddBindingSource(passport, PASSPORT_seq.ColumnsName.WHEN_GIVEN);
            mbBirth_Date.AddBindingSource(emp, EMP_seq.ColumnsName.EMP_BIRTH_DATE);

            btOrder.Enabled = false;
            //tbPer_Num.Enabled = false;
            tbPer_Num.Text = per_num;           

            mbBirth_Date.Validating += new CancelEventHandler(Library.ValidatingDate);
            mbWhen_Given.Validating += new CancelEventHandler(Library.ValidatingDate);
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
                PASSPORT_seq ps = new PASSPORT_seq(connection);
                ps.Fill(string.Format(" where {0} = '{1}' and {2} = '{3}' and {4} = '{5}'",
                    PASSPORT_seq.ColumnsName.TYPE_PER_DOC_ID, cbType_Per_Doc_ID.SelectedValue,
                    PASSPORT_seq.ColumnsName.SERIA_PASSPORT, mbSeria_Passport.Text,
                    PASSPORT_seq.ColumnsName.NUM_PASSPORT, mbNum_Passport.Text));
                if (ps.Count != 0 && ((PASSPORT_obj)(((CurrencyManager)BindingContext[ps]).Current)).PER_NUM != tbPer_Num.Text)
                {
                    MessageBox.Show("Работник с введенными данными документа личности\nсуществует в базе данных работников!\nВведите другие данные.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbType_Per_Doc_ID.Focus();
                    return;
                }
            }
            if (tbPer_Num.Text == "")
            {
                MessageBox.Show("Вы не присвоили табельный номер новому работнику!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbPer_Num.Focus();
                return;
            }
            if (tbEmp_Last_Name.Text == "")
            {
                MessageBox.Show("Вы не ввели фамилию нового работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbEmp_Last_Name.Focus();
                return;
            }
            if (tbEmp_First_Name.Text == "")
            {
                MessageBox.Show("Вы не ввели имя нового работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbEmp_First_Name.Focus();
                return;
            }
            if (tbEmp_Middle_Name.Text == "")
            {
                MessageBox.Show("Вы не ввели отчество нового работника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbEmp_Middle_Name.Focus();
                return;
            }
            if (cbSex.Text == "")
            {
                MessageBox.Show("Вы не выбрали пол сотрудника!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cbSex.Focus();
                return;
            }
            if (mbBirth_Date.Text == "")
            {
                MessageBox.Show("Вы не ввели дату рождения!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.mbBirth_Date.Focus();
                return;
            }
            if (cbSubdiv_Name.Text == "")
            {
                MessageBox.Show("Вы не выбрали подразделение!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cbSubdiv_Name.Focus();
                return;
            }
            if (cbPos_Name.Text == "")
            {
                MessageBox.Show("Вы не выбрали должность!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cbPos_Name.Focus();
                return;
            }
            if (cbSource_ID.Text == "")
            {
                MessageBox.Show("Вы не выбрали источник комплектования!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.cbSource_ID.Focus();
                return;
            }
            EMP_seq empRow = new EMP_seq(connection);
            empRow.Fill(string.Format("where upper({0}) = upper('{1}') and upper({2}) = upper('{3}') " +
                "and upper({4}) = upper('{5}') and to_char({6},'dd.MM.yyyy') = '{7}' and upper({8}) = upper('{9}')",
                EMP_seq.ColumnsName.EMP_LAST_NAME, tbEmp_Last_Name.Text.Trim(), 
                EMP_seq.ColumnsName.EMP_FIRST_NAME, tbEmp_First_Name.Text.Trim(),
                EMP_seq.ColumnsName.EMP_MIDDLE_NAME, tbEmp_Middle_Name.Text.Trim(),
                EMP_seq.ColumnsName.EMP_BIRTH_DATE, mbBirth_Date.Text,
                EMP_seq.ColumnsName.EMP_SEX, cbSex.Text));
            if (empRow.Count != 0 && !f_OldEmp)
            {
                MessageBox.Show("В базе данных уже есть сотрудник с введенными ФИО,\nполом и датой рождения.","АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            emp.Save();
            if (!f_OldEmp)
            {
                r_per_num_book = per_num_book.Where(s => s.PER_NUM == per_num).FirstOrDefault();
                r_per_num_book.FREE_SIGN = false;
                per_num_book.Save();
            }
            ((PER_DATA_obj)((CurrencyManager)BindingContext[per_data]).Current).PER_NUM = per_num;
            per_data.Save();
            ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).PER_NUM = per_num;
            passport.Save();
            ((REGISTR_obj)((CurrencyManager)BindingContext[registr]).Current).PER_NUM = per_num;
            registr.Save();
            ((HABIT_obj)((CurrencyManager)BindingContext[habit]).Current).PER_NUM = per_num;
            habit.Save();
            ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM = per_num;
            ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).HIRE_SIGN = false;
            ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TYPE_TRANSFER_ID = 1;
            ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER = DateTime.Now;
            transfer.Save();
            Connect.Commit();
            btOrder.Enabled = true;
            MessageBox.Show("Данные сохранены в базе данных.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Событие изменения индеска комбобокса типа документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbType_Per_Doc_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType_Per_Doc_ID.SelectedValue != null)
            {
                type_per_doc = new TYPE_PER_DOC_seq(connection);
                type_per_doc.Fill(string.Format(" where type_per_doc_id = {0}", cbType_Per_Doc_ID.SelectedValue.ToString()));
                r_type_per_doc = type_per_doc.Where(s => s is TYPE_PER_DOC_obj).FirstOrDefault();
                mbSeria_Passport.Mask = r_type_per_doc.TEMPL_SER;                    
                mbNum_Passport.Mask = r_type_per_doc.TEMPL_NUM;
                mbSeria_Passport.Enabled = true;
                mbNum_Passport.Enabled = true;
                tbWho_Given.Enabled = true;
                mbWhen_Given.Enabled = true;
                tbCitizenship.Enabled = true;
                mbSeria_Passport.Focus();
            }
        }
        
        /// <summary>
        /// Событие нажатия кнопки формирования приказа о приеме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrder_Click(object sender, EventArgs e)
        {
            if (((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).TYPE_PER_DOC_ID == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).SERIA_PASSPORT == null || ((PASSPORT_obj)((CurrencyManager)BindingContext[passport]).Current).NUM_PASSPORT == null)
                MessageBox.Show("Вы не ввели данные документа личности.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Hire_Order order = new Hire_Order(connection, emp, transfer, tbCode_Subdiv.Text, cbSubdiv_Name.Text, tbCode_Pos.Text, cbPos_Name.Text);
                //order.ShowDialog();
            }   
        }

        /// <summary>
        /// Событие потери фокуса текстбоксом табельного номера и его проверка по базе данных работников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPer_Num_Leave(object sender, EventArgs e)
        {
            //if (per_num == tbPer_Num.Text)
            //{
            //    return;
            //}
            //else
            //{
            //    per_num = tbPer_Num.Text;
            //    EditPer_num();
            //}
        }

        /// <summary>
        /// Событие изменения индеска комбобокса подразделения
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(connection, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индеска комбобокса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = Library.CodeBySelectedValue(connection, POSITION_seq.ColumnsName.CODE_POS.ToString(), Staff.DataSourceScheme.SchemeName,
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
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, connection, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, 5, connection, e, POSITION_seq.ColumnsName.POS_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);
        } 

        /// <summary>
        /// Событие потери фокуса текбоксом страхового свидетельства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbInsurance_Num_Leave(object sender, EventArgs e)
        {
            PER_DATA_seq pd = new PER_DATA_seq(connection);
            pd.Fill(String.Format(" where {0} = '{1}'", PER_DATA_seq.ColumnsName.INSURANCE_NUM.ToString(), mbInsurance_Num.Text.Trim()));
            if (pd.Count != 0 && ((PER_DATA_obj)((CurrencyManager)BindingContext[pd]).Current).PER_NUM != per_num)
            {
                MessageBox.Show("Введенный номер страхового свидетельства уже существует в базе данных работников!\nВведите другой номер.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mbInsurance_Num.Focus();
            }
        }

        /// <summary>
        /// Событие потери фокуса текбоксом ИНН
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbInn_Leave(object sender, EventArgs e)
        {
            PER_DATA_seq pd = new PER_DATA_seq(connection);
            pd.Fill(String.Format(" where {0} = '{1}'", PER_DATA_seq.ColumnsName.INN.ToString(), mbInn.Text.Trim()));
            if (pd.Count != 0 && ((PER_DATA_obj)((CurrencyManager)BindingContext[pd]).Current).PER_NUM != per_num)
            {
                MessageBox.Show("Введенный ИНН уже существует в базе данных работников!\nВведите другой номер.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mbInn.Focus();
            }
        }

        /// <summary>
        /// Создание и настройка формы для работы с местом прописки
        /// </summary>
        void FormRegistr()
        {
            rregion = new REGION_seq(connection);
            rregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION.ToString()));
            rdistrict = new DISTRICT_seq(connection);
            rcity = new CITY_seq(connection);
            rlocality = new LOCALITY_seq(connection);
            rstreet = new STREET_seq(connection);
            formregistr = new Address(((CurrencyManager)BindingContext[registr]).Current, typeof(REGISTR_seq), rregion, rdistrict, rcity, rlocality, rstreet);
            formregistr.TopLevel = false;
            formregistr.Dock = DockStyle.Fill;
            formregistr.DisableAll(false, Color.White);
            formregistr.cbRegion.SelectedItem = null;
            formregistr.cbDistrict.SelectedItem = null;
            formregistr.cbCity.SelectedItem = null;
            formregistr.cbLocality.SelectedItem = null;
            formregistr.cbStreet.SelectedItem = null;
            formregistr.cbRegion.Enabled = true;
            formregistr.btRegion.Enabled = true;
            formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.EnabledComboBox);
            formregistr.cbRegion.SelectedIndexChanged += new EventHandler(formregistr.cbRegion_SelectedIndexChanged);
            formregistr.tbHouse.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_HOUSE);
            formregistr.tbBulk.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_BULK);
            formregistr.tbFlat.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_FLAT);
            formregistr.tbPhone.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_PHONE);
            formregistr.tbPost_Code.AddBindingSource(registr, REGISTR_seq.ColumnsName.REG_POST_CODE);
            formregistr.mbDate_Reg.AddBindingSource(registr, REGISTR_seq.ColumnsName.DATE_REG);
            Button bt = new Button();
            bt.Name = "btFromRegistrToHabit";
            bt.Location = new Point(24, 283);
            bt.Size = new Size(335, 27);
            bt.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            bt.Text = "Скопировать в адрес проживания";
            bt.ForeColor = Color.FromArgb(0, 70, 213);
            bt.Enabled = false;
            bt.Click += new EventHandler(btCopy_Click);
            formregistr.Controls.Add(bt);
            gbRegistr.Controls.Clear();
            gbRegistr.Controls.Add(formregistr);
            formregistr.Show();
        }

        /// <summary>
        /// Событие нажатия кнопки копирования адреса прописки в адрес проживания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btCopy_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Создание и настройка формы для работы с местом проживания
        /// </summary>
        void FormHabit()
        {
            // Создание и настройка формы для работы с местом проживания
            hregion = new REGION_seq(connection);
            hregion.Fill(string.Format("order by {0}", REGION_seq.ColumnsName.NAME_REGION.ToString()));
            hdistrict = new DISTRICT_seq(connection);
            hcity = new CITY_seq(connection);
            hlocality = new LOCALITY_seq(connection);
            hstreet = new STREET_seq(connection);
            formhabit = new Address(((CurrencyManager)BindingContext[habit]).Current, typeof(HABIT_seq), hregion, hdistrict, hcity, hlocality, hstreet);
            formhabit.cbRegion.SelectedItem = null;
            formhabit.cbDistrict.SelectedItem = null;
            formhabit.cbCity.SelectedItem = null;
            formhabit.cbLocality.SelectedItem = null;
            formhabit.cbStreet.SelectedItem = null;
            formhabit.TopLevel = false;
            formhabit.Dock = DockStyle.Fill;
            formhabit.DisableAll(false, Color.White);
            formhabit.cbRegion.Enabled = true;
            formhabit.btRegion.Enabled = true;
            formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.EnabledComboBox);
            formhabit.cbRegion.SelectedIndexChanged += new EventHandler(formhabit.cbRegion_SelectedIndexChanged);
            formhabit.tbHouse.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_HOUSE);
            formhabit.tbBulk.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_BULK);
            formhabit.tbFlat.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_FLAT);
            formhabit.tbPhone.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_PHONE);
            formhabit.tbPost_Code.AddBindingSource(habit, HABIT_seq.ColumnsName.HAB_POST_CODE);
            formhabit.mbDate_Reg.Visible = false;
            formhabit.tbPhone.Location = formhabit.mbDate_Reg.Location;
            formhabit.lbDate.Visible = false;
            formhabit.lbPhone.Location = new Point(24, 249);
            source_fill = new SOURCE_FILL_seq(connection);
            source_fill.Fill(string.Format("order by {0}", SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_NAME.ToString()));
            
            Button btCopy = new Button();
            btCopy.Name = "btFromRegistrToHabit";
            btCopy.Location = new Point(24, 283);
            btCopy.Size = new Size(335, 27);
            btCopy.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            btCopy.Text = "Скопировать в адрес проживания";
            btCopy.ForeColor = Color.FromArgb(0, 70, 213);
            btCopy.Enabled = false;
            btCopy.Click += new EventHandler(btCopy_Click);
            formregistr.Controls.Add(btCopy);

            Label lbSource_Fill = new Label();
            lbSource_Fill.Location = new Point(formhabit.lbPhone.Location.X, formhabit.lbPhone.Location.Y + 32);
            lbSource_Fill.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lbSource_Fill.Text = "Источник пополнения";
            lbSource_Fill.AutoSize = true;
            ComboBox cbSource_Fill = new ComboBox();
            cbSource_Fill.Location = new Point(198, formhabit.tbPhone.Location.Y + 32);
            cbSource_Fill.Font = new Font("Microsoft Sans Serif", 9);
            cbSource_Fill.Size = new Size(161, 21);
            cbSource_Fill.AddBindingSource(registr, SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_ID, new LinkArgument(source_fill, SOURCE_FILL_seq.ColumnsName.SOURCE_FILL_NAME));
            cbSource_Fill.SelectedItem = null;
            formhabit.Controls.Add(lbSource_Fill);
            formhabit.Controls.Add(cbSource_Fill);
            gbHabit.Controls.Clear();
            gbHabit.Controls.Add(formhabit);
            gbHabit.Text = "";
            formhabit.Show();
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
                per_num_book.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                if (per_num_book.Count != 0)
                {
                    per_num_book.Fill(string.Format(" where {0} = 1 and {1} = '{2}'", 
                        PER_NUM_BOOK_seq.ColumnsName.FREE_SIGN, PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                    // Если табельный свободен добавляем работника
                    if (per_num_book.Count != 0)
                    {
                        emp.Clear();
                        emp.AddNew();
                        emp.Where(i => i.PER_NUM == null).FirstOrDefault().PER_NUM = per_num;
                        tbPer_Num.Text = per_num;
                        per_data.Clear();
                        per_data.AddNew();
                        passport.Clear();
                        passport.AddNew();
                        registr.Clear();
                        registr.AddNew();
                        habit.Clear();
                        habit.AddNew();
                        transfer.AddNew();
                        FormRegistr();
                        FormHabit();
                    }
                    else
                    {
                        TRANSFER_seq transferPer_Num = new TRANSFER_seq(connection);
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
                                    f_OldEmp = true;
                                    emp.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    per_data.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    passport.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    transfer.AddNew();
                                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM = per_num;
                                    registr.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    FormRegistr();
                                    string str2 = ((REGISTR_obj)((CurrencyManager)BindingContext[registr]).Current).REG_CODE_STREET;
                                    if (str2 != "")
                                    {
                                        formregistr.LoadAddress(str2);
                                        formregistr.DisableAll(true, Color.White);
                                    }                                    
                                    habit.Fill(string.Format(" where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, per_num));
                                    FormHabit();
                                    string str3 = ((HABIT_obj)((CurrencyManager)BindingContext[habit]).Current).HAB_CODE_STREET;
                                    if (str3 != "")
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
                                emp.Clear();
                                per_data.Clear();
                                passport.Clear();
                                registr.Clear();
                                habit.Clear();
                                transfer.Clear();
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
        /// Событие активации формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmp_Activated(object sender, EventArgs e)
        {
            if (!focusPer_num)
            {
                tbPer_Num.Focus();
                focusPer_num = true;
            }
        }

        private void tbPer_Num_Validating(object sender, CancelEventArgs e)
        {
            EditPer_num();
        }

        /// <summary>
        /// Событие нажатия кнопки копирования адреса прописки в адрес проживания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void bt_Click(object sender, EventArgs e)
        //{
        //    if (formregistr.cbStreet.SelectedValue == null)
        //    {
        //        MessageBox.Show("Вы не ввели адрес прописки для копирования", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }
        //    else
        //    {
        //        if (MessageBox.Show("Вы действительно хотите скопировать адрес прописки\nв адрес проживания?\nЭто займет некоторое время.", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            string st = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_CODE_STREET;
        //            ((HABIT_obj)(((CurrencyManager)BindingContext[habit]).Current)).HAB_CODE_STREET = st;
        //            formhabit.LoadAddress(st);
        //            formhabit.tbHouse.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_HOUSE;
        //            formhabit.tbBulk.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_BULK;
        //            formhabit.tbFlat.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_FLAT;
        //            formhabit.tbPost_Code.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_POST_CODE;
        //            formhabit.tbPhone.Text = ((REGISTR_obj)(((CurrencyManager)BindingContext[registr]).Current)).REG_PHONE;
        //            formhabit.tbHouse.Enabled = true;
        //            formhabit.tbBulk.Enabled = true;
        //            formhabit.tbFlat.Enabled = true;
        //            formhabit.tbPost_Code.Enabled = true;
        //            formhabit.tbPhone.Enabled = true;
        //        }
        //    }
        //}     
    }

    
}
