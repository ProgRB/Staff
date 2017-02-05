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
    #region Классы по работе с графиками работ
    /// <summary>
    /// Класс "График работы"
    /// </summary>
    public class Graph_Work
    {
        string _displayName;
        string _typegraph;
        string _datebeginsm;
        string _without_holidays;
        string _mode_firstenter_lastexit;
        string _val_late_enter;
        string _val_early_exit;
        string _val_delay;
        string _val_reseipts_earlier;
        string _id_external;
        string _id_internal;
        List<Graph_Work_Day> _listGraph_Work_Day;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="displayName">Наименование графика</param>
        public Graph_Work(string id_external, string id_internal, string displayName, string typegraph, string datebeginsm, string without_holidays, string mode_firstenter_lastexit,
            string val_late_enter, string val_early_exit, string val_delay, string val_reseipts_earlier, List<Graph_Work_Day> listGraph_Work_Day)
        {
            _displayName = displayName;
            _typegraph = typegraph;
            _datebeginsm = datebeginsm;
            _without_holidays = without_holidays;
            _mode_firstenter_lastexit = mode_firstenter_lastexit;
            _val_late_enter = val_late_enter;
            _val_early_exit = val_early_exit;
            _val_delay = val_delay;
            _val_reseipts_earlier = val_reseipts_earlier;
            _id_external = id_external;
            _id_internal = id_internal;
            _listGraph_Work_Day = listGraph_Work_Day;
        }
        /// <summary>
        /// Наименование графика
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        /// <summary>
        /// Тип графика (week – недельный, removable – сменный, month – месячный)
        /// </summary>
        public string Typegraph
        {
            get { return _typegraph; }
            set { _typegraph = value; }
        }
        /// <summary>
        /// Используется только при сменном графике и применяется для сопоставления дня смены с календарной датой
        /// </summary>
        public string Datebeginsm
        {
            get { return _datebeginsm; }
            set { _datebeginsm = value; }
        }
        /// <summary>
        /// Признак отсутствия учета праздников:
        /// true - при назначении сотруднику данного графика, праздничные дни не учитываются;
        /// false - при назначении сотруднику данного графика, праздничные дни учитываются
        /// </summary>
        public string Without_holidays
        {
            get { return _without_holidays; }
            set { _without_holidays = value; }
        }
        /// <summary>
        /// Признак сохранять время первого входа и последнего выхода:
        /// true - при назначении сотруднику данного графика не учитываются все промежуточные входы и выходы;
        /// false - при назначении сотруднику данного графика все промежуточные входы и выходы учитываются
        /// </summary>
        public string Mode_firstenter_lastexit
        {
            get { return _mode_firstenter_lastexit; }
            set { _mode_firstenter_lastexit = value; }
        }
        /// <summary>
        /// Точного описания нет, но судя по аналогии с Val_early_exit:
        /// Время, величина которого учитывается в дисциплинарных отчетах по поздним входам
        /// </summary>
        public string Val_late_enter
        {
            get { return _val_late_enter; }
            set { _val_late_enter = value; }
        }
        /// <summary>
        /// Время, величина которого учитывается в дисциплинарных отчетах по преждевременным уходам
        /// </summary>
        public string Val_early_exit
        {
            get { return _val_early_exit; }
            set { _val_early_exit = value; }
        }
        /// <summary>
        /// Время задержек на рабочем месте для дисциплинарных отчетов
        /// </summary>
        public string Val_delay
        {
            get { return _val_delay; }
            set { _val_delay = value; }
        }
        /// <summary>
        /// Время, величина которого учитывается в дисциплинарных отчетах по ранним приходам на рабочее место
        /// </summary>
        public string Val_reseipts_earlier
        {
            get { return _val_reseipts_earlier; }
            set { _val_reseipts_earlier = value; }
        }
        /// <summary>
        /// Внешний ключ
        /// </summary>
        public string Id_external
        {
            get { return _id_external; }
            set { _id_external = value; }
        }
        public string Id_internal
        {
            get { return _id_internal; }
            set { _id_internal = value; }
        }
        /// <summary>
        /// Список дней графика работы
        /// </summary>
        public List<Graph_Work_Day> ListGraph_Work_Day
        {
            get { return _listGraph_Work_Day; }
            set { _listGraph_Work_Day = value; }
        }
    }
    public class Graph_Work_Day
    {
        string _numberday;
        string _time_begin;
        string _time_end;
        string _for_next_day;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="numberday">Номер для в рабочем графике</param>
        /// <param name="time_begin">Начало интервала</param>
        /// <param name="time_end">Конец интервала</param>
        /// <param name="for_next_day">Признак к какому дню относятся интервалы (0 - time_begin и time_end в один день, 1 - time_begin в одном, а time_end в следующем, 2 - time_begin и time_end в следующем</param>
        public Graph_Work_Day(string numberday, string time_begin, string time_end, string for_next_day)
        {
            _numberday = numberday;
            _time_begin = time_begin;
            _time_end = time_end;
            _for_next_day = for_next_day;
        }
        /// <summary>
        /// Номер дня в рабочем графике
        /// </summary>
        public string Numberday
        {
            get { return _numberday; }
            set { _numberday = value; }
        }
        /// <summary>
        /// Начало интервала работы
        /// </summary>
        public string Time_begin
        {
            get { return _time_begin; }
            set { _time_begin = value; }
        }
        /// <summary>
        /// Конец интервала работы
        /// </summary>
        public string Time_end
        {
            get { return _time_end; }
            set { _time_end = value; }
        }
        /// <summary>
        /// 0 - time_begin и time_end время находится в пределах одного дня начала смены;
        /// 1 - time_begin – начальное время относится ко дню начала смены, а time_end – к следующему за ним;
        /// 2 - time_begin и time_end находятся в одном следующем дне конца смены
        /// </summary>
        public string For_next_day
        {
            get { return _for_next_day; }
            set { _for_next_day = value; }
        }
    }

    public enum Graph_WorkElement { ID, DisplayName, Typegraph, Datebeginsm, Without_holidays, Mode_firstenter_lastexit, Val_late_enter,
        Val_early_exit, Val_delay, Val_reseipts_earlier, Id_external, Id_internal }
    /// <summary>
    /// Класс-контейнер "Графики работы"
    /// </summary>
    public class Graphs_Work : IEnumerable<Graph_Work>
    {
        CoExchangeMainClass _perco;
        DOMDocument30Class _xml_doc;
        HashSet<Graph_Work> _graph_Work = new HashSet<Graph_Work>();
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
        public Graphs_Work(string loginName, string ipAddress, string port, string loginPerco, string passwordPerco)
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
        private bool Insert(Graph_Work graph_Work)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""UTF-8"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "graph_of_the_work");
                xml_doc.appendChild(element);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "append");
                element.appendChild(workMode);
                IXMLDOMElement graph = xml_doc.createElement("title");
                graph.setAttribute("displayname", graph_Work.DisplayName);
                graph.setAttribute("typegraph", graph_Work.Typegraph);
                graph.setAttribute("datebeginsm", graph_Work.Datebeginsm);
                graph.setAttribute("without_holidays", graph_Work.Without_holidays);
                graph.setAttribute("mode_firstenter_lastexit", graph_Work.Mode_firstenter_lastexit);
                graph.setAttribute("val_late_enter", graph_Work.Val_late_enter);
                graph.setAttribute("val_early_exit", graph_Work.Val_early_exit);
                graph.setAttribute("val_delay", graph_Work.Val_delay);
                graph.setAttribute("val_reseipts_earlier", graph_Work.Val_reseipts_earlier);
                graph.setAttribute("login", _loginName);
                graph.setAttribute("id_external", graph_Work.Id_external);
                for (int i = 0; i < graph_Work.ListGraph_Work_Day.Count; i++)
                {
                    IXMLDOMElement daysm = xml_doc.createElement("daysm");
                    daysm.setAttribute("numberday", graph_Work.ListGraph_Work_Day[i].Numberday);
                    IXMLDOMElement fields = xml_doc.createElement("fields");
                    fields.setAttribute("time_begin", graph_Work.ListGraph_Work_Day[i].Time_begin);
                    fields.setAttribute("time_end", graph_Work.ListGraph_Work_Day[i].Time_end);
                    fields.setAttribute("for_next_day", graph_Work.ListGraph_Work_Day[i].For_next_day);
                    daysm.appendChild(fields);
                    graph.appendChild(daysm);
                }
                element.appendChild(graph);                
                if (_perco.SendData(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _perco.GetErrorDescription(Error);
                    string _textError = "PERCo: Ошибка при добавлении графика работы!\nСодержание ошибки:";
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
        public bool InsertGraph_Work(Graph_Work graph_Work)
        {
            bool isGraphInsert = true;
            if (_perco.SetConnect(_ipAddress, _port, _loginPerco, _passwordPerco) == 0)
            {
                //Пытаемся вставить график работы
                isGraphInsert = Insert(graph_Work);
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

        #region Функции обновления
        /// <summary>
        /// Функция обновления графика работы
        /// </summary>
        /// <param name="graph_Work">Обновляемый график работы</param>
        /// <returns></returns>
        private bool Update(Graph_Work graph_Work)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""UTF-8"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "graph_of_the_work");
                xml_doc.appendChild(element);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "update");
                element.appendChild(workMode);
                IXMLDOMElement graph = xml_doc.createElement("title");
                //Устанавливаем значения
                graph.setAttribute("displayname", graph_Work.DisplayName);
                graph.setAttribute("typegraph", graph_Work.Typegraph);
                graph.setAttribute("datebeginsm", graph_Work.Datebeginsm);
                graph.setAttribute("without_holidays", graph_Work.Without_holidays);
                graph.setAttribute("mode_firstenter_lastexit", graph_Work.Mode_firstenter_lastexit);
                graph.setAttribute("val_late_enter", graph_Work.Val_late_enter);
                graph.setAttribute("val_early_exit", graph_Work.Val_early_exit);
                graph.setAttribute("val_delay", graph_Work.Val_delay);
                graph.setAttribute("val_reseipts_earlier", graph_Work.Val_reseipts_earlier);
                graph.setAttribute("login", _loginName);
                graph.setAttribute("id_external", graph_Work.Id_external);
                graph.setAttribute("id_internal", graph_Work.Id_internal);
                for (int i = 0; i < graph_Work.ListGraph_Work_Day.Count; i++)
                {
                    IXMLDOMElement daysm = xml_doc.createElement("daysm");
                    daysm.setAttribute("numberday", graph_Work.ListGraph_Work_Day[i].Numberday);
                    IXMLDOMElement fields = xml_doc.createElement("fields");
                    fields.setAttribute("time_begin", graph_Work.ListGraph_Work_Day[i].Time_begin);
                    fields.setAttribute("time_end", graph_Work.ListGraph_Work_Day[i].Time_end);
                    fields.setAttribute("for_next_day", graph_Work.ListGraph_Work_Day[i].For_next_day);
                    daysm.appendChild(fields);
                    graph.appendChild(daysm);
                }
                element.appendChild(graph);
                if (_perco.SendData(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _perco.GetErrorDescription(Error);
                    string _textError = "PERCo: Ошибка при обновлении графика работы!\nСодержание ошибки:";
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
        /// Функция обновления графика работы
        /// </summary>
        /// <param name="graph_Work">Обновляемый график работы</param>
        /// <returns></returns>
        public bool UpdateGraph_Work(Graph_Work graph_Work)
        {
            bool isGraphUpdate = true;
            if (_perco.SetConnect(_ipAddress, _port, _loginPerco, _passwordPerco) == 0)
            {
                //Пытаемся вствить сотрудника
                isGraphUpdate = Update(graph_Work);
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
                isGraphUpdate = false;
            }
            return isGraphUpdate;
        }
        #endregion

        #region Функции удаления
        /// <summary>
        /// Удаление подразделений
        /// </summary>
        /// <param name="position">Удаляемый график работы</param>
        /// <returns></returns>
        private bool Delete(string graph_ID, string displayName)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "graph_of_the_work");
                xml_doc.appendChild(element);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "delete");
                element.appendChild(workMode);
                IXMLDOMElement graph = xml_doc.createElement("title");
                graph.setAttribute("displayname", displayName);
                graph.setAttribute("login", _loginName);
                graph.setAttribute("id_external", graph_ID);
                graph.setAttribute("id_internal", "");
                element.appendChild(graph);
                if (_perco.SendData(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _perco.GetErrorDescription(Error);
                    string _textError = "PERCo: Ошибка при удалении графика работы!\nСодержание ошибки:";
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
                    //Удаление не удалось
                    return false;
                }
                else
                {
                    //Удаление  удалось
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
        /// Удаление подразделений
        /// </summary>
        /// <param name="position">Удаляемый график работы</param>
        /// <returns></returns>
        public bool DeleteGraph_Work(string graph_ID, string displayName)
        {
            bool isGraphDelete = true;
            if (_perco.SetConnect(_ipAddress, _port, _loginPerco, _passwordPerco) == 0)
            {
                //Пытаемся вствить сотрудника
                isGraphDelete = Delete(graph_ID, displayName);
                //Разъединяемся с перко
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
                isGraphDelete = false;
            }
            return isGraphDelete;
        }
        #endregion
        /// <summary>
        /// Деструктор класса
        /// </summary>
        ~Graphs_Work()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #region IEnumerable<Graph_Work> Members

        public IEnumerator<Graph_Work> GetEnumerator()
        {
            return _graph_Work.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _graph_Work.GetEnumerator();
        }

        #endregion
        
        
    }
    #endregion
}