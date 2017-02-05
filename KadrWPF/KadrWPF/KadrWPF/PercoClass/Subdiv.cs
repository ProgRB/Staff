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

namespace PercoXML
{
    public enum SubdivElement { ID, ParentID, DisplayName, Description }
    /// <summary>
    /// Класс подразделение
    /// </summary>
    public class Subdivision
    {
        private string _id;
        private string _parentID;
        private string _displayName;
        private string _description;
        private string _idRef;
        // public Subdivision(string id, string parentID, string displayName):this(id,parentID,displayName,null) { }

        /// <summary>
        /// Конструктор для подразделений, которые никуда не входят
        /// </summary>
        /// <param name="id">Уникальный идентификатор подразделения</param>
        /// <param name="displayName">Наименование подразделения</param>
        /// <param name="description">Описание подразделения</param>
        public Subdivision(string id, string displayName, string description)
            : this(id, displayName, description, "-1")
        {
        }
        /// <summary>
        /// Конструктор для подчиненных подразделений
        /// </summary>
        /// <param name="id">Уникальный идентификатор подразделения</param>
        /// <param name="parentID">Идентификатор родительского подразделения</param>
        /// <param name="displayName">Наименование подразделения</param>
        /// <param name="description">Описание подразделения</param>
        public Subdivision(string id, string displayName, string description, string parentID)
        {
            _id = id;
            _parentID = parentID;
            _displayName = displayName;
            _description = description;
        }
        /// <summary>
        /// Конструктор для подчиненных подразделений
        /// </summary>
        /// <param name="id">Уникальный идентификатор подразделения</param>
        /// <param name="displayName">Наименование подразделения</param>
        /// <param name="description">Описание подразделения</param>
        /// <param name="parentID">Идентификатор родительского подразделения</param>
       /* public Subdivision(string id, string displayName, string description, string parentID)
        {
            _id = id;
            _parentID = parentID;
            _displayName = displayName;
            _description = description;
        //    _idRef = idRef;
        }*/
        /// <summary>
        /// Уникальный номер подразделения (нашей базы)
        /// </summary>
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// Уникальный номер родительского подразделения
        /// </summary>
        public string ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }
        /// <summary>
        /// Имя подразделения
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = DisplayName;
            }
        }
        /// <summary>
        /// Описание подразделения
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        /// <summary>
        /// Уникальный идентификатор базы Перко
        /// </summary>
        public string IDRef
        {
            get { return _idRef; }
            set { _idRef = value; }
        }
    }

    /// <summary>
    /// Класс-контейнер работы с подразделениями
    /// </summary>
    public class Subdivisions : IEnumerable<Subdivision>
    {
        //HashSet<Division> _divisions = new HashSet<Division>();
        PERCO_1C_S20Class _perco;
        DOMDocument30Class _xml_doc;
        //IXMLDOMNode _node;
        //XDocument _xdoc;
        HashSet<Subdivision> _subdivisions = new HashSet<Subdivision>();
     //   bool _dataGot;
        string _loginName;//Имя пользователя, производившего изменеия или выборку данных
        string _ipAddress;
        string _port;
        /// <summary>
        /// Конструктор класса-контейнера подразделения
        /// </summary>
        /// <param name="perco">Класс для работы с базой Перко</param>
        /// <param name="loginName">Имя пользователя, вносившего изменения</param>
        /// <param name="mode">C загрузкой подразделений или нет</param>
        public Subdivisions(PERCO_1C_S20Class perco, string loginName,string ipAddress,string port)
        {
            _ipAddress = ipAddress;
            _port = port;
            _perco = perco;
            _loginName = loginName;
            _xml_doc = new DOMDocument30Class();
        }
        #region Функции вставки
        /// <summary>
        /// Функция вставки подразделения
        /// </summary>
        /// <param name="subdivision">Вставляемое продразделение</param>
        private bool Insert(Subdivision subdivision)
        {
            try
            {
                //if (subdivision.ID == "") //Если id не определен то выходим из функции
                //    return false;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "subdiv");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "append");
                login.appendChild(workMode);
                IXMLDOMElement subdiv = xml_doc.createElement("subdiv");
                subdiv.setAttribute("id", subdivision.ID);
                subdiv.setAttribute("parentid", subdivision.ParentID);
                subdiv.setAttribute("displayname", subdivision.DisplayName);
                subdiv.setAttribute("description", subdivision.Description);
                workMode.appendChild(subdiv);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    return false;
                }
                else
                {
                    //_subdivisions.Add(subdivision);
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
        /// Функция вставки подразделения
        /// </summary>
        /// <param name="subdivision">Вставляемое продразделение</param>
        public bool InsertSubDiv(Subdivision subdivision)
        {
            bool isSubidvInsert = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isSubidvInsert = Insert(subdivision);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isSubidvInsert = false;
            }
            return isSubidvInsert;
        }
        #endregion

        #region Процедуры обновления
        /// <summary>
        /// Функция обновления подразделения
        /// </summary>
        /// <param name="subdiv">Обновляемое подразделение</param>
        /// <returns></returns>
        private bool Upadate(Subdivision subdivision)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "subdiv");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "update");
                login.appendChild(workMode);
                IXMLDOMElement subdiv = xml_doc.createElement("subdiv");
                //Устанавливаем значения
                subdiv.setAttribute("id", subdivision.ID);
                subdiv.setAttribute("parentid", subdivision.ParentID);
                subdiv.setAttribute("displayname", subdivision.DisplayName);
                subdiv.setAttribute("description", subdivision.Description);
                workMode.appendChild(subdiv);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    return false;
                }
                else
                {
                    //Переприсваеваем подразделения
                    //lastSubDiv = subdivision;
                    //Обновляем наше подразделение в оперативной памяти
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
        /// Функция обновления подразделения
        /// </summary>
        /// <param name="subdiv">Обновляемое подразделение</param>
        /// <returns></returns>
        public bool UpdateSubDiv(Subdivision subdivision)
        {
            bool isSubidvUpdate = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isSubidvUpdate = Upadate(subdivision);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isSubidvUpdate = false;
            }
            return isSubidvUpdate;
        }
        #endregion

        /// <summary>
        /// Удаление подразделений
        /// </summary>
        /// <param name="subdivision">Удаляемое подразделение</param>
        /// <returns></returns>
        public bool Delete(string Subidv_ID)
        {
            try
            {
                Subdivision subdivision = new Subdivision(Subidv_ID, "", "");
               /* if (subdivision.ID == "") //Если id не определен то выходим из функции
                    return false;
                //получим подразделение с данным id
                Subdivision lastSubDiv;
                if (_subdivisions != null) //Если подразделения заполнены
                    //То берем его из существующего подразделения
                    lastSubDiv = _subdivisions.Where(s => s.ID == subdivision.ID).First();
                else
                    lastSubDiv = GetDivisionsByElement(s => s == subdivision.ID, SubdivElement.ID)[0];

                if (lastSubDiv == null) //если нет такого подразделения то выходим из функции
                    return false;*/
                //Создаем XML документ
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "subdiv");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "delete");
                login.appendChild(workMode);
                IXMLDOMElement subdiv = xml_doc.createElement("subdiv");
                subdiv.setAttribute("id", subdivision.ID);
                subdiv.setAttribute("parentid", subdivision.ParentID);
                subdiv.setAttribute("displayname", subdivision.DisplayName);
                subdiv.setAttribute("description", subdivision.Description);
                workMode.appendChild(subdiv);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //DOMDocument30Class error = new DOMDocument30Class();
                    //Object obj = (object)error;
                    //_perco.GetErrorDescription(ref obj);
                    //IXMLDOMNodeList nodeList = error.getElementsByTagName("error");

                    //for (int i = 0; i < nodeList.length - 1; i++)
                    //{
                    //    IXMLDOMNode node = nodeList[i];
                    //    node = node.attributes.getNamedItem("error");
                    //    Console.WriteLine("Ошибка - {0}",node.nodeValue);
                    //}
                    //Удаление не удалось
                    return false;
                }
                else
                {
                    //Удаление  удалось
                   // _subdivisions.Remove(lastSubDiv);
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
        /// <param name="subdivision">Удаляемое подразделение</param>
        /// <returns></returns>
        public bool DeleteSubdiv(string Subidv_ID)
        {
            bool isSubdivDelete = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isSubdivDelete = Delete(Subidv_ID);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isSubdivDelete = false;
            }
            return isSubdivDelete;
        }

        ~Subdivisions()
        {
            GC.WaitForPendingFinalizers();//Сборка неуправляемых объектов
            GC.Collect();
        }

        #region IEnumerable<Subdivision> Members

        public IEnumerator<Subdivision> GetEnumerator()
        {
            return _subdivisions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _subdivisions.GetEnumerator();
        }

        #endregion
    }
}