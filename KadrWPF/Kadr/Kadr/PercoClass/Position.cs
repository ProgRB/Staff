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
    #region Классы по работе с должностями
    /// <summary>
    /// Класс "Должность"
    /// </summary>
    public class Position
    {
        string _id;
        string _displayName;
        string _description;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="displayName">Имя должности</param>
        public Position(string id, string displayName) : this(id, displayName, null) { }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="displayName">Имя должности</param>
        /// <param name="description">Описание должности</param>
        public Position(string id, string displayName, string description)
        {
            _id = id;
            _displayName = displayName;
            _description = description;
        }
        //public Position(string id, string displayName, string description, string idRef)
        //{
        //    _id = id;
        //    _displayName = displayName;
        //    _description = description;
           
        //}
        /// <summary>
        /// Уникальный идентификатор из нашей базы
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Наименование должности
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        /// <summary>
        /// Описание должности
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Уникальный идентификатор из базы Перко
        /// </summary>
       /* public string IDRef
        {
            get { return _idRef; }
            set { _idRef = value; }
        }*/
    }
    public enum PositionElement { ID, DisplayName, Description }
    /// <summary>
    /// Класс-контейнер "Должности"
    /// </summary>
    public class Positions : IEnumerable<Position>
    {
        //HashSet<Division> _divisions = new HashSet<Division>();
        PERCO_1C_S20Class _perco;
        DOMDocument30Class _xml_doc;
        //IXMLDOMNode _node;
        //XDocument _xdoc;
        HashSet<Position> _positions = new HashSet<Position>();
    //    bool _dataGot;
        string _loginName;//Имя пользователя, производившего изменеия или выборку данных
        string _ipAddress;
        string _port;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="perco">Класс перко</param>
        /// <param name="loginName">Логин</param>
        public Positions(PERCO_1C_S20Class perco, string loginName, string ipAddress, string port)
        {
            _perco = perco;
            _loginName = loginName;
            _xml_doc = new DOMDocument30Class();
            _ipAddress = ipAddress;
            _port = port;
        }
        #region Процедуры вставки
        /// <summary>
        /// Функция вставки должности
        /// </summary>
        /// <param name="position">Вставляемое должность</param>
        private bool Insert(Position position)
        {
            try
            {
                //if (position.ID == "") //Если id не определен то выходим из функции
                //    return false;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "appoint");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "append");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("appoint");
                pos.setAttribute("id", position.ID);
                pos.setAttribute("displayname", position.DisplayName);
                pos.setAttribute("description", position.Description);
                workMode.appendChild(pos);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    return false;
                }
                else
                {
                    //Вставка удалась
                    //_positions.Add(position);
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
        /// Функция вставки должности
        /// </summary>
        /// <param name="position">Вставляемое должность</param>
        public bool InsertPosition(Position position)
        {
            bool isPosInsert = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isPosInsert = Insert(position);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isPosInsert = false;
            }
            return isPosInsert;
        }
        #endregion

        #region Функции обновления
        /// <summary>
        /// Функция обновления подразделения
        /// </summary>
        /// <param name="position">Обновляемое подразделение</param>
        /// <returns></returns>
        private bool Update(Position position)
        {
            try
            {
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "appoint");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "update");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("appoint");
                //Устанавливаем значения
                pos.setAttribute("id", position.ID);
                pos.setAttribute("displayname", position.DisplayName);
                pos.setAttribute("description", position.Description);
                workMode.appendChild(pos);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    return false;
                }
                else
                {
                    //Переприваеваем получившуюся позицию
                   // lastPosition = position;
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
        /// <param name="position">Обновляемое подразделение</param>
        /// <returns></returns>
        public bool UpdatePosition(Position position)
        {
            bool isPosUpdate = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isPosUpdate = Update(position);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isPosUpdate = false;
            }
            return isPosUpdate;
        }
        #endregion

        #region Функции удаления
        /// <summary>
        /// Удаление подразделений
        /// </summary>
        /// <param name="position">Удаляемое подразделение</param>
        /// <returns></returns>
        private bool Delete(string Pos_ID)
        {
            try
            {
                Position position = new Position(Pos_ID,"");
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "appoint");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "delete");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("appoint");
                pos.setAttribute("id", position.ID);
                pos.setAttribute("displayname", position.DisplayName);
                pos.setAttribute("description", position.Description);
                workMode.appendChild(pos);
                if (_perco.SendData(xml_doc) != 0)
                {
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
        /// <param name="position">Удаляемое подразделение</param>
        /// <returns></returns>
        public bool DeletePosition(string Pos_ID)
        {
            bool isPosDelete = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isPosDelete = Delete(Pos_ID);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isPosDelete = false;
            }
            return isPosDelete;
        }
        #endregion
        /// <summary>
        /// Деструктор класса
        /// </summary>
        ~Positions()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #region IEnumerable<Position> Members

        public IEnumerator<Position> GetEnumerator()
        {
            return _positions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _positions.GetEnumerator();
        }

        #endregion
        
        
    }
    #endregion
}