using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using PERCo_S20_1C;
using MSXML2;
using Staff;
using Oracle.DataAccess.Client;
using PERCo_S20_SDK;

namespace PercoXML
{
    #region Классы по работе с праздничными днями
    /// <summary>
    /// Класс "Праздничные дни"
    /// </summary>
    public class Holidays
    {
        string _year;
        List<Holiday> _listHolidays;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="displayName">Наименование графика</param>
        public Holidays(string year, List<Holiday> listHolidays)
        {
            _year = year;
            _listHolidays = listHolidays;
        }

        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }
        /// <summary>
        /// Список дней 
        /// </summary>
        public List<Holiday> ListHolidays
        {
            get { return _listHolidays; }
            set { _listHolidays = value; }
        }
    }
    public class Holiday
    {
        string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        string _id_holidays;

        public string Id_holidays
        {
            get { return _id_holidays; }
            set { _id_holidays = value; }
        }
        string _day_holiday;

        public string Day_holiday
        {
            get { return _day_holiday; }
            set { _day_holiday = value; }
        }
        string _type_holiday;
        /// <summary>
        /// 0 – Праздничный день
        /// 1 – Предпраздничный день, в этом случае также необходимо заполнить атрибут pref_type_holiday – сокращение рабочего времени (не более 2 часов).
        /// 2 – Рабочий выходной. Это - суббота или воскресенье, назначенные рабочим днём. В этом случае необходимо заполнить атрибут sat_san_day_iswork – это должна быть дата, по которой учитывается график работы в выходной рабочий день. Разность между датами не должна быть больше 10 дней.
        /// </summary>
        public string Type_holiday
        {
            get { return _type_holiday; }
            set { _type_holiday = value; }
        }
        string _pref_type_holiday;
        /// <summary>
        /// сокращение рабочего времени (не более 2 часов)
        /// </summary>
        public string Pref_type_holiday
        {
            get { return _pref_type_holiday; }
            set { _pref_type_holiday = value; }
        }
        string _sat_san_day_iswork;
        /// <summary>
        /// дата, по которой учитывается график работы в выходной рабочий день
        /// </summary>
        public string Sat_san_day_iswork
        {
            get { return _sat_san_day_iswork; }
            set { _sat_san_day_iswork = value; }
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="numberday">Номер для в рабочем графике</param>
        /// <param name="time_begin">Начало интервала</param>
        /// <param name="time_end">Конец интервала</param>
        /// <param name="for_next_day">Признак к какому дню относятся интервалы (0 - time_begin и time_end в один день, 1 - time_begin в одном, а time_end в следующем, 2 - time_begin и time_end в следующем</param>
        public Holiday(string displayName, string id_holidays, string day_holiday, string type_holiday, string pref_type_holiday, string sat_san_day_iswork)
        {
            _displayName = displayName;
            _id_holidays = id_holidays;
            _day_holiday = day_holiday;
            _type_holiday = type_holiday;
            _pref_type_holiday = pref_type_holiday;
            _sat_san_day_iswork = sat_san_day_iswork;
        }
    }

    /// <summary>
    /// Класс-контейнер "Графики работы"
    /// </summary>
    public class Holidays_Class 
    {
        CoExchangeMainClass _perco;
        DOMDocument30Class _xml_doc;
        HashSet<Holidays> _graph_Work = new HashSet<Holidays>();
        string _loginName;//Имя пользователя, производившего изменения или выборку данных
        string _ipAddress;
        string _port;
        string _loginPerco;
        string _passwordPerco;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="perco">Класс перко</param>
        /// <param name="loginName">Логин</param>
        public Holidays_Class(string loginName, string ipAddress, string port, string loginPerco, string passwordPerco)
        {
            _perco = new CoExchangeMainClass();
            _loginName = loginName;
            _xml_doc = new DOMDocument30Class();
            _ipAddress = ipAddress;
            _port = port;
            _loginPerco = loginPerco;
            _passwordPerco = passwordPerco;
        }
        #region Процедуры вставки
        /// <summary>
        /// Функция вставки графика работы
        /// </summary>
        /// <param name="graph_Work">Вставляемый график работы</param>
        private bool Insert(Holidays holidays)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""UTF-8"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "holidays");
                element.setAttribute("year", holidays.Year);
                xml_doc.appendChild(element);
                IXMLDOMElement element1 = xml_doc.createElement("login");
                element1.setAttribute("loginname", _loginName);
                element.appendChild(element1);
                IXMLDOMElement hol = xml_doc.createElement("holidays");
                for (int i = 0; i < holidays.ListHolidays.Count; i++)
                {
                    IXMLDOMElement daysm = xml_doc.createElement("holidaysnode");
                    daysm.setAttribute("displayname", holidays.ListHolidays[i].DisplayName);
                    daysm.setAttribute("id_holidays", holidays.ListHolidays[i].Id_holidays);
                    daysm.setAttribute("day_holiday", holidays.ListHolidays[i].Day_holiday);
                    daysm.setAttribute("type_holiday", holidays.ListHolidays[i].Type_holiday);
                    daysm.setAttribute("pref_type_holiday", holidays.ListHolidays[i].Pref_type_holiday);
                    daysm.setAttribute("sat_san_day_iswork", holidays.ListHolidays[i].Sat_san_day_iswork);
                    hol.appendChild(daysm);
                }
                element.appendChild(hol);                
                if (_perco.SendData(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _perco.GetErrorDescription(Error);
                    string _textError = "PERCo: Ошибка при добавлении праздничного дня!\nСодержание ошибки:";
                    foreach (IXMLDOMNode item in Error.childNodes)
                    {
                        IXMLDOMNode objNamedNodeMap = item.lastChild;
                        if (objNamedNodeMap != null)
                        {
                            IXMLDOMNamedNodeMap objNamedNodeMap1 = objNamedNodeMap.attributes;
                            for (int j = 0; j < objNamedNodeMap1.length; j++)
                            {
                                if (objNamedNodeMap1[j].nodeName.ToUpper() == "ERROR")
                                    _textError += "\n" + objNamedNodeMap1[j].nodeValue.ToString();
                            }
                        }
                    }
                    System.Windows.Forms.MessageBox.Show(_textError, "АСУ \"Кадры\"", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    //Вставка не удалась
                    return false;
                }
                else
                {
                    //Вставка удалась
                    return true;
                }
            }
            finally
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        /// <summary>
        /// Функция вставки графика работы
        /// </summary>
        /// <param name="position">Вставляемый график работы</param>
        public bool InsertHolidays(Holidays holidays)
        {
            bool isGraphInsert = true;
            if (_perco.SetConnect(_ipAddress, _port, _loginPerco, _passwordPerco) == 0)
            {
                //Пытаемся вставить график работы
                isGraphInsert = Insert(holidays);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                DOMDocument30Class Error = new DOMDocument30Class();
                _perco.GetErrorDescription(Error);
                string _textError = "PERCo: Ошибка подключение к базе данных!\nСодержание ошибки:"; 
                foreach (IXMLDOMNode item in Error.childNodes)
                {     
                    IXMLDOMNode objNamedNodeMap = item.lastChild;
                    if (objNamedNodeMap != null)
                    {
                        IXMLDOMNamedNodeMap objNamedNodeMap1 = objNamedNodeMap.attributes;
                        for (int j = 0; j < objNamedNodeMap1.length; j++)
                        {
                            if (objNamedNodeMap1[j].nodeName.ToUpper() == "ERROR")
                                _textError += "\n" + objNamedNodeMap1[j].nodeValue.ToString();
                        }
                    }
                }
                System.Windows.Forms.MessageBox.Show(_textError, "АСУ \"Кадры\"", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                isGraphInsert = false;
            }
            return isGraphInsert;
        }
        #endregion

        /// <summary>
        /// Деструктор класса
        /// </summary>
        ~Holidays_Class()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }             
    }
    #endregion
}