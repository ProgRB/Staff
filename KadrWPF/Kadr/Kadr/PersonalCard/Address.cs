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
using EditorsLibrary;

namespace Kadr
{
    public partial class Address : Form
    {
        //REGION_seq region;
        //DISTRICT_seq district;
        //CITY_seq city;
        //LOCALITY_seq locality;
        //STREET_seq street;
        DataSet _dsAddress;
        object obj;        
        private TableMove[] tablemove = TableMove.ComboMove().ToArray();
        private TableMove[] tablebutton = TableMove.ButtonMove().ToArray();
        Type type_seq;
        OracleDataAdapter _daRegion, _daDistrict1_2, _daCity1_2, _daCity1_5, _daLocality1_5, _daLocality1_8, _daStreet1_2, _daStreet1_8, _daStreet1_11;
        // Создание формы и привязка комбобоксов
        public Address(object _obj, Type _type_seq, REGION_seq _region, DISTRICT_seq _district, CITY_seq _city,
                       LOCALITY_seq _locality, STREET_seq _street)
        {
            InitializeComponent();
            _dsAddress = new DataSet();
            _dsAddress.Tables.Add("REGION");
            _dsAddress.Tables.Add("DISTRICT");
            _dsAddress.Tables.Add("CITY");
            _dsAddress.Tables.Add("LOCALITY");
            _dsAddress.Tables.Add("STREET");
            //region = _region;
            //district = _district;
            //city = _city;
            //locality = _locality;
            //street = _street;
            obj = _obj;
            type_seq = _type_seq;

            _daRegion = new OracleDataAdapter(string.Format(
                "SELECT CODE_REGION, NAME_REGION, ABBREV_ID FROM {0}.REGION ORDER BY NAME_REGION", Connect.Schema),
                Connect.CurConnect);
            _daRegion.Fill(_dsAddress.Tables["REGION"]);

            _daDistrict1_2 = new OracleDataAdapter(string.Format(
                "SELECT CODE_DISTRICT, NAME_DISTRICT, ABBREV_ID FROM {0}.DISTRICT WHERE SUBSTR(CODE_DISTRICT,1,2) = :p_DISTRICT ORDER BY NAME_DISTRICT", 
                Connect.Schema), Connect.CurConnect);
            _daDistrict1_2.SelectCommand.Parameters.Add("p_DISTRICT", OracleDbType.Varchar2).Value = "";
            _daDistrict1_2.Fill(_dsAddress.Tables["DISTRICT"]);

            _daCity1_2 = new OracleDataAdapter(string.Format(
                "SELECT CODE_CITY, NAME_CITY, ABBREV_ID FROM {0}.CITY WHERE SUBSTR(CODE_CITY,1,2) = :p_CITY ORDER BY NAME_CITY",
                Connect.Schema), Connect.CurConnect);
            _daCity1_2.SelectCommand.Parameters.Add("p_CITY", OracleDbType.Varchar2).Value = "";
            _daCity1_2.Fill(_dsAddress.Tables["CITY"]);

            _daCity1_5 = new OracleDataAdapter(string.Format(
                "SELECT CODE_CITY, NAME_CITY, ABBREV_ID FROM {0}.CITY WHERE SUBSTR(CODE_CITY,1,5) = :p_CITY ORDER BY NAME_CITY",
                Connect.Schema), Connect.CurConnect);
            _daCity1_5.SelectCommand.Parameters.Add("p_CITY", OracleDbType.Varchar2).Value = "";

            _daLocality1_5 = new OracleDataAdapter(string.Format(
                "SELECT CODE_LOCALITY, LOCALITY_NAME, ABBREV_ID FROM {0}.LOCALITY WHERE SUBSTR(CODE_LOCALITY,1,5) = :p_LOCALITY ORDER BY LOCALITY_NAME",
                Connect.Schema), Connect.CurConnect);
            _daLocality1_5.SelectCommand.Parameters.Add("p_LOCALITY", OracleDbType.Varchar2).Value = "";
            _daLocality1_5.Fill(_dsAddress.Tables["LOCALITY"]);

            _daLocality1_8 = new OracleDataAdapter(string.Format(
                "SELECT CODE_LOCALITY, LOCALITY_NAME, ABBREV_ID FROM {0}.LOCALITY WHERE SUBSTR(CODE_LOCALITY,1,8) = :p_LOCALITY ORDER BY LOCALITY_NAME",
                Connect.Schema), Connect.CurConnect);
            _daLocality1_8.SelectCommand.Parameters.Add("p_LOCALITY", OracleDbType.Varchar2).Value = "";

            _daStreet1_2 = new OracleDataAdapter(string.Format(
                "SELECT CODE_STREET, NAME_STREET, ABBREV_ID, STR_POST_CODE FROM {0}.STREET WHERE SUBSTR(CODE_STREET,1,2) = :p_STREET ORDER BY NAME_STREET",
                Connect.Schema), Connect.CurConnect);
            _daStreet1_2.SelectCommand.Parameters.Add("p_STREET", OracleDbType.Varchar2).Value = "";
            _daStreet1_2.Fill(_dsAddress.Tables["STREET"]);

            _daStreet1_8 = new OracleDataAdapter(string.Format(
                "SELECT CODE_STREET, NAME_STREET, ABBREV_ID, STR_POST_CODE FROM {0}.STREET WHERE SUBSTR(CODE_STREET,1,8) = :p_STREET ORDER BY NAME_STREET",
                Connect.Schema), Connect.CurConnect);
            _daStreet1_8.SelectCommand.Parameters.Add("p_STREET", OracleDbType.Varchar2).Value = "";

            _daStreet1_11 = new OracleDataAdapter(string.Format(
                "SELECT CODE_STREET, NAME_STREET, ABBREV_ID, STR_POST_CODE FROM {0}.STREET WHERE SUBSTR(CODE_STREET,1,11) = :p_STREET ORDER BY NAME_STREET",
                Connect.Schema), Connect.CurConnect);
            _daStreet1_11.SelectCommand.Parameters.Add("p_STREET", OracleDbType.Varchar2).Value = "";

            //cbRegion.DataSource = region;
            cbRegion.DataSource = _dsAddress.Tables["REGION"].DefaultView;
            cbRegion.DisplayMember = REGION_seq.ColumnsName.NAME_REGION.ToString();
            cbRegion.ValueMember = REGION_seq.ColumnsName.CODE_REGION.ToString();
            //cbDistrict.DataSource = district;
            cbDistrict.DataSource = _dsAddress.Tables["DISTRICT"].DefaultView;
            cbDistrict.DisplayMember = DISTRICT_seq.ColumnsName.NAME_DISTRICT.ToString();
            cbDistrict.ValueMember = DISTRICT_seq.ColumnsName.CODE_DISTRICT.ToString();
            //cbCity.DataSource = city;
            cbCity.DataSource = _dsAddress.Tables["CITY"].DefaultView;
            cbCity.DisplayMember = CITY_seq.ColumnsName.NAME_CITY.ToString();
            cbCity.ValueMember = CITY_seq.ColumnsName.CODE_CITY.ToString();
            //cbLocality.DataSource = locality;
            cbLocality.DataSource = _dsAddress.Tables["LOCALITY"].DefaultView;
            cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
            cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
            //cbStreet.DataSource = street;
            cbStreet.DataSource = _dsAddress.Tables["STREET"].DefaultView;
            cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
            cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();
        }
        
        // Обработка правил активизации комбобоксов
        public void EnabledComboBox(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            foreach (Control control in this.Controls)
                if (!(control is Label))
                    control.Enabled = false;
            string[] rezul = tablemove.Where(s => s.Place == comboBox.Name).Select(s => s.InputPlace).ToArray();
            IEnumerable<Control> controls = from Control control in this.Controls
                                            from string nameOfControl in rezul
                                            where control.Name == nameOfControl
                                            select control;
            foreach (Control control in controls)
            {
                control.Enabled = true;
                String st = control.Name;
                string namebutton = "bt" + st.Substring(2);
                this.Controls[namebutton].Enabled = true;
            }

        }

        // Обработка правил активизации кнопок
        public void EnabledButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            foreach (Control control in this.Controls)
                if (!(control is Label))
                    control.Enabled = false;
            string[] rezul = tablebutton.Where(s => s.Place == button.Name).Select(s => s.InputPlace).ToArray();
            IEnumerable<Control> controls = from Control control in this.Controls
                                            from string nameOfControl in rezul
                                            where control.Name == nameOfControl
                                            select control;
            foreach (Control control in controls)
            {
                control.Enabled = true;
                String st = control.Name;
                if ((button.Name).Substring(2) == "District")
                {
                    // Заполняем таблицу городов из выбранного региона по 2 символам кода улицы
                    _daCity1_2.SelectCommand.Parameters["p_CITY"].Value = cbRegion.SelectedValue.ToString();
                    _daCity1_2.Fill(_dsAddress.Tables["CITY"]);
                    cbCity.SelectedItem = null;
                }
                string namebutton = "bt" + st.Substring(2);
                this.Controls[namebutton].Enabled = true;
                if (String.Compare(control.Name, 3, button.Name, 3, control.Name.Length) == 0)
                    this.Controls[control.Name].Text = "";
            }
            foreach (Control control in this.Controls)
                if (control is ComboBox && control.Enabled == false)
                    control.Text = "";
        }

        /// <summary>
        /// Функция по коду загружает адрес
        /// </summary>
        /// <param name="st">Код улицы</param> 
        public void LoadAddress(string st)
        {
            cbRegion.SelectedIndexChanged -= cbRegion_SelectedIndexChanged;
            cbDistrict.SelectedIndexChanged -= cbDistrict_SelectedIndexChanged;
            cbDistrict.SelectedIndexChanged -= EnabledComboBox;
            cbCity.SelectedIndexChanged -= cbCity_SelectedIndexChanged;
            cbCity.SelectedIndexChanged -= EnabledComboBox;
            cbLocality.SelectedIndexChanged -= cbLocality_SelectedIndexChanged;
            cbLocality.SelectedIndexChanged -= EnabledComboBox;
            cbStreet.SelectedIndexChanged -= cbStreet_SelectedIndexChanged;
            

            _dsAddress.Clear();
            // Заполняем таблицу регионов
            _daRegion.Fill(_dsAddress.Tables["REGION"]);
            
            // Настраиваем комбобокс для отображения регионов.
            cbRegion.DataSource = _dsAddress.Tables["REGION"].DefaultView;
            cbRegion.DisplayMember = REGION_seq.ColumnsName.NAME_REGION.ToString();
            cbRegion.ValueMember = REGION_seq.ColumnsName.CODE_REGION.ToString();

            // Ищем нужный регион по 2 первым символам кода улицы.
            DataView tempView = _dsAddress.Tables["REGION"].DefaultView.ToTable().DefaultView;
            tempView.RowFilter = "CODE_REGION = '" + st.Substring(0, 2) + "'";
            cbRegion.SelectedValue = tempView[0]["CODE_REGION"];

            // Заполняем таблицу районов по коду выбранного региона.
            _daDistrict1_2.SelectCommand.Parameters["p_DISTRICT"].Value = cbRegion.SelectedValue.ToString();
            _daDistrict1_2.Fill(_dsAddress.Tables["DISTRICT"]);

            /// Настраиваем комбобокс для отображения районов.
            cbDistrict.DataSource = _dsAddress.Tables["DISTRICT"].DefaultView;
            cbDistrict.DisplayMember = DISTRICT_seq.ColumnsName.NAME_DISTRICT.ToString();
            cbDistrict.ValueMember = DISTRICT_seq.ColumnsName.CODE_DISTRICT.ToString();

            tempView = _dsAddress.Tables["DISTRICT"].DefaultView.ToTable().DefaultView;
            tempView.RowFilter = "CODE_DISTRICT = '" + st.Substring(0, 5) + "'";

            /// Если таблица районов содержится в коде улицы
            if (tempView.Count > 0)
            {
                /// Ищем нужный район в таблице по 5 символам кода улицы
                cbDistrict.SelectedValue = tempView[0]["CODE_DISTRICT"];
            }
            else
            {
                cbDistrict.SelectedItem = null;
            }

            // Настраиваем комбобокс для отображения городов
            cbCity.DataSource = _dsAddress.Tables["CITY"].DefaultView; 
            cbCity.DisplayMember = CITY_seq.ColumnsName.NAME_CITY.ToString();
            cbCity.ValueMember = CITY_seq.ColumnsName.CODE_CITY.ToString();
            // Если выбран район
            if (cbDistrict.SelectedItem != null)
            {
                // Заполняем таблицу городов из выбранного региона по 5 символам кода улицы
                _daCity1_5.SelectCommand.Parameters["p_CITY"].Value = cbDistrict.SelectedValue.ToString();
                _daCity1_5.Fill(_dsAddress.Tables["CITY"]);
            }
            else
            {
                // Заполняем таблицу городов из выбранного региона по 2 символам кода улицы
                _daCity1_2.SelectCommand.Parameters["p_CITY"].Value = cbRegion.SelectedValue.ToString();
                _daCity1_2.Fill(_dsAddress.Tables["CITY"]);
            }

            tempView = _dsAddress.Tables["CITY"].DefaultView.ToTable().DefaultView;
            tempView.RowFilter = "CODE_CITY = '" + st.Substring(0, 8) + "'";

            /// Если в таблице городов найдена улица по 8 символам кода улицы
            if (tempView.Count > 0)
            {
                /// Ищем нужный город в таблице по 8 символам кода улицы 
                cbCity.SelectedValue = tempView[0]["CODE_CITY"];
            }
            else
            {
                cbCity.SelectedItem = null;
            }

            // Настраиваем комбобокс для отображения населенных пунктов
            cbLocality.DataSource = _dsAddress.Tables["LOCALITY"].DefaultView;
            cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
            cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
            // Если выбран город
            if (cbCity.SelectedItem != null)
            {
                // Заполняем таблицу пунктов по городу 
                _daLocality1_8.SelectCommand.Parameters["p_LOCALITY"].Value = cbCity.SelectedValue.ToString();
                _daLocality1_8.Fill(_dsAddress.Tables["LOCALITY"]);
            }
            else
            {
                if (cbDistrict.SelectedItem != null)
                {
                    // Заполняем таблицу пунктов по району
                    _daLocality1_5.SelectCommand.Parameters["p_LOCALITY"].Value = cbDistrict.SelectedValue.ToString();
                    _daLocality1_5.Fill(_dsAddress.Tables["LOCALITY"]);
                }
                else
                {
                    if (cbRegion.Text.ToUpper() != "МОСКВА" && cbRegion.Text.ToUpper() != "САНКТ-ПЕТЕРБУРГ")
                    {
                        _dsAddress.Tables["LOCALITY"].Clear();
                    }
                }
            }

            tempView = _dsAddress.Tables["LOCALITY"].DefaultView.ToTable().DefaultView;
            tempView.RowFilter = "CODE_LOCALITY = '" + st.Substring(0, 11) + "'";

            // Если пункт найден в коде улицы
            if (tempView.Count > 0)
            {
                // Ищем пункт в таблице пунктов по 11 символам
                cbLocality.SelectedValue = tempView[0]["CODE_LOCALITY"];
            }
            else
            {
                cbLocality.SelectedItem = null;
            }

            // Настраиваем комбобокс для работы с улицами
            cbStreet.DataSource = _dsAddress.Tables["STREET"].DefaultView; 
            cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
            cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();
            
            // Если выбран пункт
            if (cbLocality.SelectedItem != null)
            {
                // Заполняем таблицу улиц по 11 символам выбранного пункта
                _daStreet1_11.SelectCommand.Parameters["p_STREET"].Value = cbLocality.SelectedValue.ToString();
                _daStreet1_11.Fill(_dsAddress.Tables["STREET"]);
            }
            else
            {
                if (cbCity.SelectedItem != null)
                {
                    _dsAddress.Tables["STREET"].Clear();
                    // Заполняем таблицу улиц по 8 символам города
                    _daStreet1_8.SelectCommand.Parameters["p_STREET"].Value = cbCity.SelectedValue.ToString();
                    _daStreet1_8.Fill(_dsAddress.Tables["STREET"]);
                }
                else
                {
                    // Заполняем таблицу улиц по 2 символам региона
                    _daStreet1_2.SelectCommand.Parameters["p_STREET"].Value = cbRegion.SelectedValue.ToString();
                    _daStreet1_2.Fill(_dsAddress.Tables["STREET"]);
                }
            }

            // Ищем улицу по ее коду
            cbStreet.SelectedValue = st;
            // Устанавливаем значение индекса
            tbPost_Code.Text = (cbStreet.SelectedItem as DataRowView)["STR_POST_CODE"].ToString();
            // Добавляем событие изменения улицы
            cbRegion.SelectedIndexChanged += new EventHandler(cbRegion_SelectedIndexChanged);
            cbDistrict.SelectedIndexChanged += new EventHandler(cbDistrict_SelectedIndexChanged);
            cbDistrict.SelectedIndexChanged += new EventHandler(EnabledComboBox);
            cbCity.SelectedIndexChanged += new EventHandler(cbCity_SelectedIndexChanged);
            cbDistrict.SelectedIndexChanged += new EventHandler(EnabledComboBox);
            cbLocality.SelectedIndexChanged += new EventHandler(cbLocality_SelectedIndexChanged);
            cbDistrict.SelectedIndexChanged += new EventHandler(EnabledComboBox);
            cbStreet.SelectedIndexChanged += new EventHandler(cbStreet_SelectedIndexChanged);

            /*cbRegion.DataSource = _dtRegion;
            cbRegion.DisplayMember = REGION_seq.ColumnsName.NAME_REGION.ToString();
            cbRegion.ValueMember = REGION_seq.ColumnsName.CODE_REGION.ToString();
            /// Ищем нужный регион по 2 первым символам кода улицы.
            ((CurrencyManager)BindingContext[region]).Position =
                region.Select((s, i) => new { s, i }).Where(f => f.s.CODE_REGION ==
                    st.Substring(0, 2)).Select(k => k.i).FirstOrDefault();
            /// Заполняем таблицу районов по коду выбранного региона.
            district.Fill(string.Format("where substr(code_district,1,2) = '{0}' order by name_district",
                cbRegion.SelectedValue.ToString()));
            /// Настраиваем комбобокс для отображения районов.
            cbDistrict.DataSource = district;
            cbDistrict.DisplayMember = DISTRICT_seq.ColumnsName.NAME_DISTRICT.ToString();
            cbDistrict.ValueMember = DISTRICT_seq.ColumnsName.CODE_DISTRICT.ToString();
            /// Если таблица районов содержится в коде улицы
            if (district.Where(s => s.CODE_DISTRICT == st.Substring(0, 5)).FirstOrDefault() != null)
            {
                /// Ищем нужный район в таблице по 5 символам кода улицы
                ((CurrencyManager)BindingContext[district]).Position =
                    district.Select((s, i) => new { s, i }).Where(f => f.s.CODE_DISTRICT ==
                        st.Substring(0, 5)).Select(k => k.i).FirstOrDefault();
            }
            else
            {
                cbDistrict.SelectedItem = null;
            }

            /// Настраиваем комбобокс для отображения городов
            cbCity.DataSource = city;
            cbCity.DisplayMember = CITY_seq.ColumnsName.NAME_CITY.ToString();
            cbCity.ValueMember = CITY_seq.ColumnsName.CODE_CITY.ToString();
            /// Если выбран район
            if (cbDistrict.SelectedItem != null)
            {
                /// Заполняем таблицу городов из выбранного региона по 5 символам кода улицы
                city.Fill(string.Format("where substr(code_city,1,5) = '{0}' order by name_city", cbDistrict.SelectedValue.ToString()));
            }
            else
            {
                /// Заполняем таблицу городов из выбранного региона по 2 символам кода улицы
                city.Fill(string.Format("where substr(code_city,1,2) = '{0}' order by name_city", cbRegion.SelectedValue.ToString()));
            }
            /// Если в таблице городов найдена улица по 8 символам кода улицы
            if (city.Where(s => s.CODE_CITY == st.Substring(0, 8)).FirstOrDefault() != null)
            {
                /// Ищем нужный город в таблице по 8 символам кода улицы 
                ((CurrencyManager)BindingContext[city]).Position = city.Select((s, i) => new { s, i }).Where(f => f.s.CODE_CITY == st.Substring(0, 8)).Select(k => k.i).FirstOrDefault();
            }
            else
            {
                cbCity.SelectedItem = null;
            }            
            /// Настраиваем комбобокс для отображения населенных пунктов
            cbLocality.DataSource = locality;
            cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
            cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
            /// Если выбран город
            if (cbCity.SelectedItem != null)
            {
                /// Заполняем таблицу пунктов по городу 
                locality.Fill(string.Format("where substr(code_locality,1,8) = '{0}' order by locality_name", cbCity.SelectedValue.ToString()));
            }
            else
            {
                if (cbDistrict.SelectedItem != null)
                {
                    /// Заполняем таблицу пунктов по району
                    locality.Fill(string.Format(" where substr(code_locality,1,5) = '{0}' order by locality_name", cbDistrict.SelectedValue.ToString()));
                }
                else
                {
                    if (cbRegion.Text.ToUpper() != "МОСКВА" && cbRegion.Text.ToUpper() != "САНКТ-ПЕТЕРБУРГ")
                    {
                        locality.Clear();
                    }
                }
            }
            /// Если пункт найден в коде улицы
            if (locality.Where(s => s.CODE_LOCALITY == st.Substring(0, 11)).FirstOrDefault() != null)
            {
                /// Ищем пункт в таблице пунктов по 11 символам
                ((CurrencyManager)BindingContext[locality]).Position = locality.Select((s, i) => new { s, i }).Where(f => f.s.CODE_LOCALITY == st.Substring(0, 11)).Select(k => k.i).FirstOrDefault();
            }
            else
            {
                cbLocality.SelectedItem = null;
            }

            /// Настраиваем комбобокс для работы с улицами
            cbStreet.DataSource = street;
            cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
            cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();
            /// Если выбран пункт
            if (cbLocality.SelectedItem != null)
            {
                /// Заполняем таблицу улиц по 11 символам выбранного пункта
                street.Fill(string.Format(" where substr(code_street,1,11) = '{0}' order by name_street", cbLocality.SelectedValue.ToString()));
            }
            else
            {
                if (cbCity.SelectedItem != null)
                {
                    /// Заполняем таблицу улиц по 8 символам города
                    street.Fill(string.Format(" where substr(code_street,1,8) = '{0}' order by name_street", cbCity.SelectedValue.ToString()));
                }
                else
                {
                    /// Заполняем таблицу улиц по 8 символам города
                    street.Fill(string.Format(" where substr(code_street,1,2) = '{0}' order by name_street", cbRegion.SelectedValue.ToString()));
                }
            }
            /// Убираем событие изменения улицы, чтобы не очищать поля дома, квартиры и т.п.
            cbStreet.SelectedIndexChanged -= cbStreet_SelectedIndexChanged;
            /// Ищем улицу по ее коду
            ((CurrencyManager)BindingContext[street]).Position = street.Select((s, i) => new { s, i }).Where(f => f.s.CODE_STREET == st).Select(k => k.i).FirstOrDefault();
            /// Устанавливаем значение индекса
            tbPost_Code.Text = street.Where(i => i.CODE_STREET == st).FirstOrDefault().STR_POST_CODE;
            /// Добавляем событие изменения улицы
            cbStreet.SelectedIndexChanged += new EventHandler(cbStreet_SelectedIndexChanged);*/
        }

        /// <summary>
        /// Изменение региона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRegion.SelectedValue != null)
            {
                cbDistrict.SelectedIndexChanged -= EnabledComboBox;
                cbDistrict.SelectedIndexChanged -= cbDistrict_SelectedIndexChanged;
                cbDistrict.DataSource = null;
                _dsAddress.Tables["DISTRICT"].Clear();
                _daDistrict1_2.SelectCommand.Parameters["p_DISTRICT"].Value = cbRegion.SelectedValue.ToString();
                _daDistrict1_2.Fill(_dsAddress.Tables["DISTRICT"]);
                //district.Fill(string.Format(" where substr(code_district,1,2) = '{0}' order by name_district", cbRegion.SelectedValue.ToString()));
                cbDistrict.DataSource = _dsAddress.Tables["DISTRICT"].DefaultView;
                cbDistrict.DisplayMember = DISTRICT_seq.ColumnsName.NAME_DISTRICT.ToString();
                cbDistrict.ValueMember = DISTRICT_seq.ColumnsName.CODE_DISTRICT.ToString();
                cbDistrict.SelectedItem = null;
                cbDistrict.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbDistrict.SelectedIndexChanged += new EventHandler(cbDistrict_SelectedIndexChanged);
                cbCity.SelectedIndexChanged -= EnabledComboBox;
                cbCity.SelectedIndexChanged -= cbCity_SelectedIndexChanged;
                cbCity.DataSource = null;
                // Заполняем таблицу городов из выбранного региона по 2 символам кода улицы
                _dsAddress.Tables["CITY"].Clear();
                _daCity1_2.SelectCommand.Parameters["p_CITY"].Value = cbRegion.SelectedValue.ToString();
                _daCity1_2.Fill(_dsAddress.Tables["CITY"]);
                cbCity.DataSource = _dsAddress.Tables["CITY"].DefaultView;
                cbCity.DisplayMember = CITY_seq.ColumnsName.NAME_CITY.ToString();
                cbCity.ValueMember = CITY_seq.ColumnsName.CODE_CITY.ToString();
                cbCity.SelectedItem = null;
                cbCity.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbCity.SelectedIndexChanged += new EventHandler(cbCity_SelectedIndexChanged);
                cbLocality.SelectedIndexChanged -= EnabledComboBox;
                cbLocality.SelectedIndexChanged -= cbLocality_SelectedIndexChanged;
                cbLocality.SelectedItem = null;
                cbLocality.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbLocality.SelectedIndexChanged += new EventHandler(cbLocality_SelectedIndexChanged);
                cbStreet.SelectedItem = null;
                if (cbRegion.Text.ToUpper() == "МОСКВА" || cbRegion.Text.ToUpper() == "САНКТ-ПЕТЕРБУРГ")
                {
                    cbLocality.SelectedIndexChanged -= EnabledComboBox;
                    cbLocality.SelectedIndexChanged -= cbLocality_SelectedIndexChanged;
                    cbLocality.DataSource = null;
                    _dsAddress.Tables["LOCALITY"].Clear();
                    _daLocality1_8.SelectCommand.Parameters["p_LOCALITY"].Value = cbRegion.SelectedValue.ToString().PadRight(8, '0');
                    _daLocality1_8.Fill(_dsAddress.Tables["LOCALITY"]);
                    cbLocality.DataSource = _dsAddress.Tables["LOCALITY"].DefaultView;
                    cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
                    cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
                    cbLocality.SelectedItem = null;
                    if (_dsAddress.Tables["LOCALITY"].DefaultView.Count == 0)
                        cbLocality.Enabled = false;
                    else
                        cbLocality.Enabled = true;
                    cbLocality.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                    cbLocality.SelectedIndexChanged += new EventHandler(cbLocality_SelectedIndexChanged);
                    cbStreet.SelectedIndexChanged -= cbStreet_SelectedIndexChanged;
                    cbStreet.DataSource = null;
                    _dsAddress.Tables["STREET"].Clear();
                    _daStreet1_8.SelectCommand.Parameters["p_STREET"].Value = cbRegion.SelectedValue.ToString().PadRight(8, '0');
                    _daStreet1_8.Fill(_dsAddress.Tables["STREET"]);
                    if (_dsAddress.Tables["STREET"].DefaultView.Count == 0)
                        cbStreet.Enabled = false;
                    else
                        cbStreet.Enabled = true;
                    cbStreet.DataSource = _dsAddress.Tables["STREET"].DefaultView;
                    cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
                    cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();
                    cbStreet.SelectedItem = null;
                    cbStreet.SelectedIndexChanged += new EventHandler(cbStreet_SelectedIndexChanged);
                }
            }
        }

        /// <summary>
        /// Изменение района
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDistrict.SelectedValue != null)
            {
                cbLocality.SelectedIndexChanged -= EnabledComboBox;
                cbLocality.SelectedIndexChanged -= cbLocality_SelectedIndexChanged;
                cbLocality.DataSource = null;
                _dsAddress.Tables["LOCALITY"].Clear();
                _daLocality1_5.SelectCommand.Parameters["p_LOCALITY"].Value = cbDistrict.SelectedValue.ToString();
                _daLocality1_5.Fill(_dsAddress.Tables["LOCALITY"]);
                cbLocality.DataSource = _dsAddress.Tables["LOCALITY"].DefaultView;
                cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
                cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
                cbLocality.SelectedItem = null;
                cbLocality.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbLocality.SelectedIndexChanged += new EventHandler(cbLocality_SelectedIndexChanged);
                cbCity.SelectedIndexChanged -= EnabledComboBox;
                cbCity.SelectedIndexChanged -= cbCity_SelectedIndexChanged;
                cbCity.DataSource = null;
                // Заполняем таблицу городов из выбранного региона по 2 символам кода улицы
                _dsAddress.Tables["CITY"].Clear();
                _daCity1_5.SelectCommand.Parameters["p_CITY"].Value = cbDistrict.SelectedValue.ToString();
                _daCity1_5.Fill(_dsAddress.Tables["CITY"]);
                if (_dsAddress.Tables["CITY"].Rows.Count == 0)
                    cbCity.Enabled = false;
                cbCity.DataSource = _dsAddress.Tables["CITY"].DefaultView;
                cbCity.DisplayMember = CITY_seq.ColumnsName.NAME_CITY.ToString();
                cbCity.ValueMember = CITY_seq.ColumnsName.CODE_CITY.ToString();
                cbCity.SelectedItem = null;
                cbCity.Text = "";
                cbCity.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbCity.SelectedIndexChanged += new EventHandler(cbCity_SelectedIndexChanged);
                cbStreet.SelectedItem = null;
            }
        }

        /// <summary>
        /// Изменение города
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCity.SelectedValue != null)
            {
                cbLocality.SelectedIndexChanged -= EnabledComboBox;
                cbLocality.SelectedIndexChanged -= cbLocality_SelectedIndexChanged;
                cbLocality.DataSource = null;
                _dsAddress.Tables["LOCALITY"].Clear();
                _daLocality1_8.SelectCommand.Parameters["p_LOCALITY"].Value = cbCity.SelectedValue.ToString();
                _daLocality1_8.Fill(_dsAddress.Tables["LOCALITY"]);
                cbLocality.DataSource = _dsAddress.Tables["LOCALITY"].DefaultView;
                cbLocality.DisplayMember = LOCALITY_seq.ColumnsName.LOCALITY_NAME.ToString();
                cbLocality.ValueMember = LOCALITY_seq.ColumnsName.CODE_LOCALITY.ToString();
                cbLocality.SelectedItem = null;
                if (_dsAddress.Tables["LOCALITY"].DefaultView.Count == 0)
                    cbLocality.Enabled = false;
                cbLocality.SelectedIndexChanged += new EventHandler(EnabledComboBox);
                cbLocality.SelectedIndexChanged += new EventHandler(cbLocality_SelectedIndexChanged);
                cbStreet.SelectedIndexChanged -= cbStreet_SelectedIndexChanged;
                cbStreet.DataSource = null;
                _dsAddress.Tables["STREET"].Clear();
                _daStreet1_8.SelectCommand.Parameters["p_STREET"].Value = cbCity.SelectedValue.ToString();
                _daStreet1_8.Fill(_dsAddress.Tables["STREET"]);
                if (_dsAddress.Tables["STREET"].DefaultView.Count == 0)
                    cbStreet.Enabled = false;
                cbStreet.DataSource = _dsAddress.Tables["STREET"].DefaultView;
                cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
                cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();                
                cbStreet.SelectedItem = null;
                cbStreet.SelectedIndexChanged += new EventHandler(cbStreet_SelectedIndexChanged);
            }            
        }

        /// <summary>
        /// Изменение пункта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cbLocality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLocality.SelectedValue != null)
            {
                cbStreet.SelectedIndexChanged -= cbStreet_SelectedIndexChanged;
                cbStreet.DataSource = null;
                _dsAddress.Tables["STREET"].Clear();
                _daStreet1_11.SelectCommand.Parameters["p_STREET"].Value = cbLocality.SelectedValue.ToString();
                _daStreet1_11.Fill(_dsAddress.Tables["STREET"]);
                cbStreet.DataSource = _dsAddress.Tables["STREET"].DefaultView;
                cbStreet.DisplayMember = STREET_seq.ColumnsName.NAME_STREET.ToString();
                cbStreet.ValueMember = STREET_seq.ColumnsName.CODE_STREET.ToString();
                cbStreet.SelectedItem = null;
                cbStreet.SelectedIndexChanged += new EventHandler(cbStreet_SelectedIndexChanged);
                if (_dsAddress.Tables["STREET"].DefaultView.Count == 0)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (control is TextBox)
                            ((TextBox)control).Enabled = true;
                        if (control is MaskedTextBox)
                            ((MaskedTextBox)control).Enabled = true;
                        if (control.Name == "btFromRegistrToHabit")
                            control.Enabled = true;
                    }
                    MessageBox.Show("Адрес не может быть сохранен полностью.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        /// <summary>
        /// Изменение улицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cbStreet_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbStreet.SelectedValue != null)
            {
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox)
                        ((TextBox)control).Enabled = true;
                    if (control is MaskedTextBox)
                        ((MaskedTextBox)control).Enabled = true;
                    if (control is DateEditor)
                    {
                        ((DateEditor)control).Enabled = false;
                        ((DateEditor)control).Enabled = true;
                    }
                    if (control.Name == "btFromRegistrToHabit")
                        control.Enabled = true;
                }
                obj.GetType().InvokeMember(type_seq.Name.Substring(0,3) + "_CODE_STREET", BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] {cbStreet.SelectedValue.ToString()});
                tbPost_Code.Text = (cbStreet.SelectedItem as DataRowView)["STR_POST_CODE"].ToString();
            }
        }

        /// <summary>
        /// Очистка комбобокса региона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRegion_Click(object sender, EventArgs e)
        {
            _dsAddress.Tables["DISTRICT"].Clear();
            _dsAddress.Tables["CITY"].Clear();
            _dsAddress.Tables["LOCALITY"].Clear();
            _dsAddress.Tables["STREET"].Clear();
            cbRegion.SelectedItem = null;
            cbRegion.Focus();
            obj.GetType().InvokeMember(type_seq.Name.Substring(0, 3) + "_CODE_STREET", 
                BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] { null });
        }

        /// <summary>
        /// Очистка комбобокса района
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDistrict_Click(object sender, EventArgs e)
        {
            _dsAddress.Tables["CITY"].Clear();
            _dsAddress.Tables["LOCALITY"].Clear();
            _dsAddress.Tables["STREET"].Clear();
            cbDistrict.SelectedItem = null;
            cbDistrict.Focus();
            obj.GetType().InvokeMember(type_seq.Name.Substring(0, 3) + "_CODE_STREET", 
                BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] { null });
        }

        /// <summary>
        /// Очистка комбобокса города
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCity_Click(object sender, EventArgs e)
        {
            _dsAddress.Tables["LOCALITY"].Clear();
            _dsAddress.Tables["STREET"].Clear();
            cbCity.SelectedItem = null;
            cbCity.Focus();
            obj.GetType().InvokeMember(type_seq.Name.Substring(0, 3) + "_CODE_STREET", 
                BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] { null });
        }

        /// <summary>
        /// Очистка комбобокса пункта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLocality_Click(object sender, EventArgs e)
        {
            _dsAddress.Tables["STREET"].Clear();
            cbLocality.SelectedItem = null;
            cbLocality.Focus();
            obj.GetType().InvokeMember(type_seq.Name.Substring(0, 3) + "_CODE_STREET", 
                BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] { null });
        }

        /// <summary>
        /// Очистка комбобокса улицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStreet_Click(object sender, EventArgs e)
        {
            cbStreet.SelectedItem = null;
            cbStreet.Focus();
            obj.GetType().InvokeMember(type_seq.Name.Substring(0, 3) + "_CODE_STREET", 
                BindingFlags.Default | BindingFlags.SetProperty, null, obj, new object[] { null });
        }

        private void tbHouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*int n = (int)e.KeyChar;
            if ((n >= 48 && n <= 57) || n == 8)
            { }
            else
            {
                e.Handled = true;
                MessageBox.Show("Разрешено вводить только цифры!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }
    }
}
