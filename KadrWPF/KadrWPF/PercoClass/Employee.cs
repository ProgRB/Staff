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
using LibraryKadr;
using PERCo_S20_SDK;

//using Staff;
namespace PercoXML
{
    #region Класс "Сотрудник"
    /// <summary>
    /// Класс "Сотрудник"
    /// </summary>
    public class Employee
    {
        string _id;
        string _lastName;
        string _firstName;
        string _middleName;
        string _tabelID;
        string _subdivID;
        string _appointID;
        string _graphID;
        string _dateBedin;
        string _photo;
        string _idRef;
        string _id_Card;
        string _id_Shablon;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="tabelID">Табельный номер</param>
        public Employee(string id, string tabelID,string lastName, string firstName,string middleName,string subdivID,string posID)
        {
            _id = id;
            _tabelID = tabelID;
            _lastName = lastName!=null && lastName!=""? lastName[0].ToString().ToUpper()+lastName.Substring(1,lastName.Length-1).ToLower():"";
            _middleName = middleName!=null && middleName!=""? middleName[0].ToString().ToUpper()+middleName.Substring(1,middleName.Length-1).ToLower():"";
            _firstName = firstName!=null && firstName!=""? firstName[0].ToString().ToUpper()+firstName.Substring(1,firstName.Length-1).ToLower():"";
            _subdivID = subdivID;
            _appointID = posID;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="tableID">Табельный номер</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="middleName">Отчество</param>
        /// <param name="subdivID">Код подразделения</param>
        /// <param name="appointID">Код должности</param>
        /// <param name="graphID">Код графика работы</param>
        /// <param name="dateBegin">Дата начала работы сотрудника</param>
        /// <param name="photo">Фотография сотрудника</param>
        public Employee(string id, string tableID, string lastName, string firstName, string middleName, string subdivID, string pos_ID, string graphID, string dateBegin, string photo, string idRef)
            : this(id, tableID, lastName, firstName, middleName, subdivID, pos_ID)
        {
            /*_id = id;
            _tabelID = tableID;
            _lastName = lastName != null && lastName != "" ? lastName[0].ToString().ToUpper() + lastName.Substring(1, lastName.Length).ToLower() : "";
            _middleName = middleName != null && middleName != "" ? middleName[0].ToString().ToUpper() + middleName.Substring(1, middleName.Length).ToLower() : "";
            _firstName = firstName != null && firstName != "" ? firstName[0].ToString().ToUpper() + firstName.Substring(1, firstName.Length).ToLower() : "";
            _tabelID = tableID;
            _subdivID = subdivID;
            _appointID = appointID;*/
            _graphID = graphID;
            _dateBedin = dateBegin;
            _photo = photo;
            _idRef = idRef;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="tabelID">Табельный номер</param>
        public Employee(string id, string tabelID, string id_Card, string id_Shablon)
        {
            _id = id;
            _tabelID = tabelID;
            _id_Card = id_Card;
            _id_Shablon = id_Shablon;
        }
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public string ID { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Табельный номер
        /// </summary>
        public string TabelID { get { return _tabelID; } set { _tabelID = value; } }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName 
        { 
            get 
            { 
                return _lastName; 
            } 
            set 
            {
                _lastName = value != null && value != "" ? value[0].ToString().ToUpper() + value.Substring(1, value.Length - 1).ToLower() : "";  
            } 
        }
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName 
        { 
            get 
            { 
                return _firstName; 
            } 
            set 
            {
                _firstName = value != null && value != "" ? value[0].ToString().ToUpper() + value.Substring(1, value.Length - 1).ToLower() : "";  
            } 
        }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName 
        { 
            get 
            { 
                return _middleName; 
            } 
            set 
            {
                _middleName = _middleName = value != null && value != "" ? value[0].ToString().ToUpper() + value.Substring(1, value.Length - 1).ToLower() : "";  
            } 
        }
        /// <summary>
        /// Код подразделения
        /// </summary>
        public string SubdivID { get { return _subdivID; } set { _subdivID = value; } }
        /// <summary>
        /// Код должности
        /// </summary>
        public string AppointID { get { return _appointID; } set { _appointID = value; } }
        /// <summary>
        /// Код графика работы
        /// </summary>
        public string GraphID { get { return _graphID; } set { _graphID = value; } }
        /// <summary>
        /// Дата начала
        /// </summary>
        public string DateBegin { get { return _dateBedin; } set { _dateBedin = value; } }
        /// <summary>
        /// Путь к фотографии
        /// </summary>
        public string Photo { get { return _photo; } set { _photo = value; } }
        /// <summary>
        /// Уникальный идентификатор в базе Perco соответствует id_staff
        /// </summary>
        public string IDRef
        {
            get { return _idRef; }
            set { _idRef = value; }
        }
        /// <summary>
        /// Идентификатор карты сотрудника
        /// </summary>
        public string ID_Card
        {
            get { return _id_Card; }
            set { _id_Card = value; }
        }
        /// <summary>
        /// Идентификатор шаблона доступа сотрудника
        /// </summary>
        public string ID_Shablon
        {
            get { return _id_Shablon; }
            set { _id_Shablon = value; }
        }
    }
    #endregion

    public class Employees  
    {
        PERCO_1C_S20Class _perco;
        CoExchangeMainClass _percoSDK;
        DOMDocument30Class _xml_doc;
        //IXMLDOMNode _node;
        //XDocument _xdoc;
        HashSet<Employee> _employees = new HashSet<Employee>();
       // bool _dataGot;
        string _loginName;//Имя пользователя, производившего изменения или выборку данных
        string _ipAddress;
        string _port;
        BUFFER_EMP_seq _buffer;
        private OracleConnection _oracleConnection;
        public Employees(PERCO_1C_S20Class perco, string loginName, string idAdress, string port)
        {
            _perco = perco;
            _loginName = loginName;
            _xml_doc = new DOMDocument30Class();
            _ipAddress = idAdress;
            _port = port;
            _oracleConnection = LibraryKadr.Connect.CurConnect;
            _buffer = new BUFFER_EMP_seq(_oracleConnection);
            _percoSDK = new CoExchangeMainClass();
        }

        ~Employees()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #region Функции восстановления

        private bool Recovery(string ID)
        {
            try
            {
                bool isEmpInsert = false;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "recovery");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("staff");
                //pos.setAttribute("action", "insert");
                //pos.setAttribute("action", "update");
                //pos.setAttribute("action", "delete");
                //pos.setAttribute("action", "dismiss");
         //       pos.setAttribute("action","recovery");
                pos.setAttribute("id", ID);
                //pos.setAttribute("id_from_1c", employee.ID);
           /*   pos.setAttribute("first_name", employee.FirstName == null ? "" : employee.FirstName);
                pos.setAttribute("last_name", employee.LastName == null ? "" : employee.LastName);
                pos.setAttribute("middle_name", employee.MiddleName == null ? "" : employee.MiddleName);
                pos.setAttribute("tabel_id", employee.TabelID);
                pos.setAttribute("subdiv_id", employee.SubdivID == null ? "0" : employee.SubdivID);
                pos.setAttribute("appoint_id", employee.AppointID == null ? "0" : employee.AppointID);
                pos.setAttribute("graph_id", employee.GraphID == null ? "0" : employee.GraphID);
                pos.setAttribute("date_begin", employee.DateBegin == null ? string.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) : employee.DateBegin);
                pos.setAttribute("photo", employee.Photo == null ? "" : employee.Photo);*/
                workMode.appendChild(pos);
            //    Console.WriteLine(xml_doc.xml);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    isEmpInsert = false;
                }
                else
                {
                    //Вставка удалась
                   // _employees.Add(employee);
                    isEmpInsert = true;
                }
                return isEmpInsert;
            }
            catch //Если ошибка, то возвращаем ложь
            {
                return false;
            }
        }
        /// <summary>
        /// Метод восстановления сотрудника
        /// </summary>
        /// <param name="ID">ID_From_1c сотрудника</param>
        /// <param name="perNum">табельный номер сотрудника</param>
        /// <param name="useBuffer">используем ли буффер при ошибках восстановления</param>
        /// <returns>Выполнена ли функция восстановления сотрудника</returns>
        public bool RecoveryEmployee(string ID,string perNum, bool useBuffer)
        {
            bool isEmpInsert = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isEmpInsert = Recovery(ID);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isEmpInsert = false;
            }
            if (!isEmpInsert)//Если вставка не была выполнена и используется буффер
            {
                if (useBuffer)//если используем буффер
                {
                    _buffer.Fill(string.Format("where per_num = '{0}' and buffer_type = 4", perNum));
                    //Удаление предыдущих версий по этому человеку
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        _buffer.Remove(_buffer[i]);
                        i--;
                    }
                    _buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = perNum, BUFFER_TYPE = 4 });
                    _buffer.Save();
                }
            }
            return isEmpInsert;
        }
        /// <summary>
        /// Метод восстановления сотрудника
        /// </summary>
        /// <param name="ID">ID_From_1c сотрудника</param>
        /// <param name="perNum">табельный номер сотрудника</param>
        /// <returns>Выполнена ли функция восстановления сотрудника</returns>
        public bool RecoveryEmployee(string ID, string perNum)
        {
            return RecoveryEmployee(ID,perNum, true);
        }
        #endregion

        //Функция удаления
        #region функции удаления

      /*  private bool Delete(string id,DateTime dateDismiss)
        {
            try
            {
                bool isEmpInsert = false;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "delete");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("staff");
                //pos.setAttribute("action", "insert");
                //pos.setAttribute("action", "update");
                //pos.setAttribute("action", "delete");
                //pos.setAttribute("action", "dismiss");
            //    pos.setAttribute("action", "delete");
                pos.setAttribute("id", id);
                //pos.setAttribute("date_dismiss", string.Format("{0}.{1}.{2}", dateDismiss.Day, dateDismiss.Month, dateDismiss.Year));
                if (dateDismiss.Month >= 10)
                    element.setAttribute("date_dismiss", string.Format("{0}.{1}.{2}", dateDismiss.Day, dateDismiss.Month, dateDismiss.Year));
                else
                    element.setAttribute("date_dismiss", string.Format("{0}.0{1}.{2}", dateDismiss.Day, dateDismiss.Month, dateDismiss.Year));
                //pos.setAttribute("id_from_1c", employee.ID);
                
                workMode.appendChild(pos);
                Console.WriteLine(xml_doc.xml);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    isEmpInsert = false;
                }
                else
                {
                    //Вставка удалась
                   // _employees.Add(employee);
                    isEmpInsert = true;
                }
                return isEmpInsert;
            }
            catch //Если ошибка, то возвращаем ложь
            {
                return false;
            }
        }

        public bool DeleteEmployee(string id,string tabelID, DateTime dateDismiss, bool useBuffer)
        {
            bool isEmpInsert = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isEmpInsert = Delete(id, dateDismiss);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isEmpInsert = false;
            }
            if (!isEmpInsert)//Если вставка не была выполнена и используется буффер
            {
                if (useBuffer)//если используем буффер
                {
                    _buffer.Fill(string.Format("where per_num = '{0}' and buffer_type = 5", tabelID));
                    //Удаление предыдущих версий по этому человеку
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        _buffer.Remove(_buffer[i]);
                        i--;
                    }
                    //_buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = tabelID, BUFFER_TYPE = 5, POS_ID = Convert.ToDecimal(employee.AppointID), SUBDIV_ID = Convert.ToDecimal(employee.SubdivID) });
                    _buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = tabelID, BUFFER_TYPE = 3, DATE_DISMISS = dateDismiss });
                    _buffer.Save();
                }
            }
            return isEmpInsert;
        }

        public bool DeleteEmployee(string id, string tabelID, DateTime dateDismiss)
        {
            return DeleteEmployee(id, tabelID, dateDismiss, true);
        }*/

        #endregion

        #region Функции вставки

        private bool Insert(Employee employee)
        {
            try
            {
                bool isEmpInsert = false;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "append");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("staff");
                pos.setAttribute("id", employee.ID);
                //pos.setAttribute("id_from_1c", employee.ID);
                pos.setAttribute("first_name", employee.FirstName == null ? "" : employee.FirstName);
                pos.setAttribute("last_name", employee.LastName == null ? "" : employee.LastName);
                pos.setAttribute("middle_name", employee.MiddleName == null ? "" : employee.MiddleName);
                pos.setAttribute("tabel_id", employee.TabelID);
                pos.setAttribute("subdiv_id", employee.SubdivID == null ? "0" : employee.SubdivID);
                pos.setAttribute("appoint_id", employee.AppointID == null ? "0" : employee.AppointID);
                pos.setAttribute("graph_id", employee.GraphID == null ? "0" : employee.GraphID);
                pos.setAttribute("date_begin", employee.DateBegin == null ? string.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) : employee.DateBegin.Substring(0, 10));
                pos.setAttribute("photo", employee.Photo == null ? "" : employee.Photo);
                workMode.appendChild(pos);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    isEmpInsert = false;
                }
                else
                {
                    //Вставка удалась
                    _employees.Add(employee);
                    isEmpInsert = true;
                }
                return isEmpInsert;
            }
            catch //Если ошибка, то возвращаем ложь
            {
                return false;
            }
        }
        /// <summary>
        /// Функция вставки сотрудника
        /// </summary>
        /// <param name="employee">вставляемый сотрудник</param>
        /// <param name="useBuffer">используем ли buffer</param>
        /// <returns></returns>
        public bool InsertEmployee(Employee employee,bool useBuffer)
        {
            bool isEmpInsert = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Пытаемся вствить сотрудника
                isEmpInsert = Insert(employee);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Соединиться не удалось возвращаем неудачу
            {
                isEmpInsert = false;
            }
            if (!isEmpInsert)//Если вставка не была выполнена и используется буффер
            {
                if (useBuffer) //если используем буффер
                {
                    _buffer.Fill(string.Format("where per_num = '{0}' and buffer_type = 1", employee.TabelID));
                    //Удаление предыдущих версий по этому человеку
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        _buffer.Remove(_buffer[i]);
                        i--;
                    }
                    _buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = employee.TabelID, BUFFER_TYPE = 1, POS_ID = Convert.ToDecimal(employee.AppointID), SUBDIV_ID = Convert.ToDecimal(employee.SubdivID), DATE_BEGIN = Convert.ToDateTime(employee.DateBegin) });
                    _buffer.Save();
                    Connect.Commit();
                }
            }
            return isEmpInsert;
        }

        /// <summary>
        /// Функция вставки сотрудника
        /// </summary>
        /// <param name="employee">вставляемый сотрудник</param>
        /// <returns></returns>
        public bool InsertEmployee(Employee employee)
        {
            return InsertEmployee(employee,true);
        }
        #endregion

        #region Функциии обновления сотрудника

        private bool Update(Employee employee)
        {
            try
            {
                bool isEmpUpdate = true;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "update");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("staff");
                //Устанавливаем значения
                pos.setAttribute("id", employee.ID);
                pos.setAttribute("first_name", employee.FirstName);
                pos.setAttribute("last_name", employee.LastName);
                pos.setAttribute("middle_name", employee.MiddleName);
                pos.setAttribute("tabel_id", employee.TabelID);
                pos.setAttribute("subdiv_id", employee.SubdivID);
                pos.setAttribute("appoint_id", employee.AppointID);
                pos.setAttribute("graph_id", employee.GraphID);
                pos.setAttribute("date_begin", employee.DateBegin);
                pos.setAttribute("photo", employee.Photo);                
                workMode.appendChild(pos);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    isEmpUpdate = false;
                }
                else
                {
                    //Вставка удалась
                    isEmpUpdate = true;
                }
                return isEmpUpdate;
            }
            catch
            {
                return false;
            }
        }

        private bool UpdateGr_Work(Employee employee)
        {
            try
            {
                bool isEmpUpdate = true;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                xml_doc.appendChild(element);
                //вставляем логин
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                element.appendChild(login);
                IXMLDOMElement workMode = xml_doc.createElement("workmode");
                workMode.setAttribute("mode", "update");
                login.appendChild(workMode);
                IXMLDOMElement pos = xml_doc.createElement("staff");
                //Устанавливаем значения
                pos.setAttribute("last_name", employee.LastName);
                pos.setAttribute("first_name", employee.FirstName);
                pos.setAttribute("middle_name", employee.MiddleName);
                pos.setAttribute("tabel_id", employee.TabelID);
                pos.setAttribute("id_external", employee.ID);
                pos.setAttribute("id_internal", "");
                pos.setAttribute("subdiv_id", employee.SubdivID);
                pos.setAttribute("appoint_id", employee.AppointID);
                pos.setAttribute("graph_id", employee.GraphID);
                pos.setAttribute("subdiv_id_internal", "");
                pos.setAttribute("appoint_id_internal", "");
                pos.setAttribute("graph_id_internal", "0");
                pos.setAttribute("date_begin", employee.DateBegin);
                pos.setAttribute("photo", employee.Photo);
                workMode.appendChild(pos);
                IXMLDOMElement identifiers = xml_doc.createElement("identifiers");
                //IXMLDOMElement identifier = xml_doc.createElement("identifier");
                //identifier.setAttribute("shablon_id_internal", "1161446");
                //identifiers.appendChild(identifier);
                pos.appendChild(identifiers);
                if (_percoSDK.SendData(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _percoSDK.GetErrorDescription(Error);
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
                    isEmpUpdate = false;
                }
                else
                {
                    //Вставка удалась
                    isEmpUpdate = true;
                }
                return isEmpUpdate;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Обновляем сотрудника
        /// </summary>
        /// <param name="employee">Обновляемый сотрудник</param>
        /// <param name="useBuffer">используем ли мы буффер</param>
        /// <returns>выполнелось ли обновление</returns>
        public bool UpdateEmployee(Employee employee,bool useBuffer)
        {
            bool isEmpUpdate = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Обновляем данные сотрудника
                isEmpUpdate = Update(employee);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else //Если не смогли открыть соединение
            {
                isEmpUpdate = false;
            }
            //if (_perco2.SetConnect(_ipAddress, _port, "ADMIN", "hn-j[hfyf") == 0)
            //{
            //    //Обновляем данные сотрудника
            //    isEmpUpdate = Update2(employee);
            //    //Разединяемся с перко
            //    _perco.DisConnect();
            //}
            //else //Если не смогли открыть соединение
            //{
            //    isEmpUpdate = false;
            //}
            if (!isEmpUpdate)//Если вставка не была выполнена и используется буффер
            {
                if (useBuffer) //если используем буффер
                {
                    _buffer.Fill(string.Format("where per_num = '{0}' and buffer_type = 2", employee.TabelID));
                    for (int i = 0; i < _buffer.Count; i++) //Удаление предыдущих версий по этому человеку
                    {
                        _buffer.Remove(_buffer[i]);
                        i--;
                    }
                    _buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = employee.TabelID, BUFFER_TYPE = 2, POS_ID = Convert.ToDecimal(employee.AppointID), SUBDIV_ID = Convert.ToDecimal(employee.SubdivID), DATE_BEGIN = Convert.ToDateTime(employee.DateBegin) });
                    _buffer.Save();
                    Connect.Commit();
                }
            }
            return isEmpUpdate;
        }
        /// <summary>
        /// Обновляем сотрудника
        /// </summary>
        /// <param name="employee">Обновляемый сотрудник</param>
        /// <returns>выполнелось ли обновление</returns>
        public bool UpdateEmployee(Employee employee)
        {
            return UpdateEmployee(employee,true);
        }
        #endregion

        #region Функции увольнения
        private bool Dismiss(string employee_id,string tabelID,DateTime dateDismiss)
        {
            try
            {
                bool isEmpDismiss = true;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "staff");
                IXMLDOMNode node = xml_doc.appendChild(element);
                //создание элемента оператора
                element = xml_doc.createElement("login");
                element.setAttribute("loginname", "login name");
                node = node.appendChild(element);
                IXMLDOMElement elemRoot = xml_doc.createElement("workmode");
                elemRoot.setAttribute("mode", "dismiss");
                IXMLDOMNode nodeRoot = node.appendChild(elemRoot);
                element = xml_doc.createElement("staff");
               // element.setAttribute("action", "dismiss");
                element.setAttribute("id", employee_id);
                if(dateDismiss.Month >= 10)
                    element.setAttribute("date_dismiss", string.Format("{0}.{1}.{2}", dateDismiss.Day, dateDismiss.Month, dateDismiss.Year));
                else
                    element.setAttribute("date_dismiss", string.Format("{0}.0{1}.{2}", dateDismiss.Day, dateDismiss.Month, dateDismiss.Year));
                node = nodeRoot.appendChild(element);
                if (_perco.SendData(xml_doc) != 0)
                {
                    //Вставка не удалась
                    isEmpDismiss = false;
                }
                else
                {
                    //Вставка удалась
                    isEmpDismiss = true;
                }
                return isEmpDismiss;
            }
            catch //Если ошибка, то возвращаем ложь
            {
                return false;
            }
        }
        /// <summary>
        /// Функция увольнения сотрудника
        /// </summary>
        /// <param name="employee_id">Уникальный идентификатор ID_FROM_1c</param>
        /// <param name="tabelID">Табельный номер</param>
        /// <param name="dateDismiss">Дата увольнения</param>
        /// <param name="useBuffer">Используем ли буффер</param>
        /// <returns>выполнелось ли увольнение в perco</returns>
        public bool DismissEmployee(string employee_id,string tabelID, DateTime dateDismiss,bool useBuffer)
        {
            bool isEmpDismiss = true;
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Увольняем сотрудинка
                isEmpDismiss = Dismiss(employee_id, tabelID, dateDismiss);
                //Разединяемся с перко
                _perco.DisConnect();
            }
            else
            {
                isEmpDismiss = false;
            }
            if (!isEmpDismiss)//Если увольнение не удалось и используется буффер, то тогда пихаем в буффер
            {
                if (useBuffer) //Если используем буффер
                {
                    _buffer.Fill(string.Format("where per_num = '{0}' and buffer_type = 3", tabelID));
                    //Очищаем все по этому человеку
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        _buffer.Remove(_buffer[i]);
                        i--;
                    }
                    _buffer.AddObject(new BUFFER_EMP_obj() { PER_NUM = tabelID, BUFFER_TYPE = 3, DATE_DISMISS = dateDismiss });
                    _buffer.Save();
                    Connect.Commit();
                }
            }
            return isEmpDismiss;
        }
        /// <summary>
        /// Функция увольнения сотрудника
        /// </summary>
        /// <param name="employee_id">Уникальный идентификатор ID_FROM_1c</param>
        /// <param name="tabelID">Табельный номер</param>
        /// <param name="dateDismiss">Дата увольнения</param>
        /// <returns>выполнелось ли увольнение в perco</returns>
        public bool DismissEmployee(string employee_id, string tabelID, DateTime dateDismiss)
        {
            return DismissEmployee(employee_id, tabelID, dateDismiss, true);
        }
        #endregion

        /// <summary>
        /// Функция высвобождения буффера
        /// </summary>
        /// <returns>Был ли буффер перенесен в перко</returns>
        public bool ReleaseBuffer()
        {
            bool isBufferReleased = true;
            //Если соединение прошло успешно
            if (_perco.SetConnection(_ipAddress, _port) == 0)
            {
                //Получаем сотрудников
                EMP_seq emps = new EMP_seq(_oracleConnection);
                _buffer.Fill();
                //------------------
                //Обходим увольнения
                //------------------
                for (int i = 0; i < _buffer.Count; i++)
                {
                    emps.Fill(string.Format(" where per_num = '{0}'", _buffer[i].PER_NUM));
                    EMP_obj emp = emps[0];
                    BUFFER_EMP_obj buffer = _buffer[i];
                    //Если строка относится к увольнению сотрудника
                    if (buffer.BUFFER_TYPE == 3)
                    {
                        if (Dismiss(emp.PERCO_SYNC_ID.ToString(),emp.PER_NUM,Convert.ToDateTime(buffer.DATE_DISMISS)))
                        {
                            _buffer.Remove(buffer);//Удаляем из буффера строку
                            i--;
                        }
                    }
                }
                //---------------------------
                //Обходим принятых на работу-
                //---------------------------
                for (int i = 0; i < _buffer.Count; i++)
                {
                    emps.Fill(string.Format(" where per_num = '{0}'", _buffer[i].PER_NUM));
                    EMP_obj emp = emps[0];
                    BUFFER_EMP_obj buffer = _buffer[i];
                    //Если строка относиться к вставке сотрудника
                    if (buffer.BUFFER_TYPE == 1)
                    {
                        //Вставляем сотрудника
                        //Если вставка удалась
                        if (Insert(new Employee(emp.PERCO_SYNC_ID.ToString(), emp.PER_NUM, emp.EMP_LAST_NAME, emp.EMP_FIRST_NAME, emp.EMP_MIDDLE_NAME, buffer.SUBDIV_ID.ToString(), buffer.POS_ID.ToString())))
                        {
                            _buffer.Remove(buffer);//Удаляем из буффера строку
                            i--;
                        }
                    }
                }
                //-------------------
                //Обходим обновления-
                //-------------------
                for (int i = 0; i < _buffer.Count;i++)
                {
                    emps.Fill(string.Format(" where per_num = '{0}'", _buffer[i].PER_NUM));
                    EMP_obj emp = emps[0];
                    BUFFER_EMP_obj buffer = _buffer[i];
                    //Если строка относится к обновлению сотрудника
                    if (buffer.BUFFER_TYPE == 2)
                    {
                        if (Update(new Employee(emp.PERCO_SYNC_ID.ToString(), emp.PER_NUM, emp.EMP_LAST_NAME, emp.EMP_FIRST_NAME, emp.EMP_MIDDLE_NAME, buffer.SUBDIV_ID.ToString(), buffer.POS_ID.ToString())))
                        {
                            _buffer.Remove(buffer);//Удаляем из буффера строку
                            i--;
                        }
                    }
                }
                _buffer.Save();
                Connect.Commit();
                //Разединяемся с перко
                _perco.DisConnect();
            }
            //Соединиться не удалось
            else
            {
                isBufferReleased = false;
            }
            return isBufferReleased;
        }
        
        private bool UpdateAccess(Employee employee)
        {
            try
            {
                bool isEmpUpdate = true;
                DOMDocument30Class xml_doc = new DOMDocument30Class();
                xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                IXMLDOMElement element = xml_doc.createElement("documentrequest");
                element.setAttribute("type", "sendcards");
                element.setAttribute("mode", "withdraw_and_insert_access_staff");
                xml_doc.appendChild(element);
                IXMLDOMElement login = xml_doc.createElement("login");
                login.setAttribute("loginname", _loginName);
                IXMLDOMElement employs = xml_doc.createElement("employs");
                IXMLDOMElement employ = xml_doc.createElement("employ");
                employ.setAttribute("id_external", employee.ID);
                employ.setAttribute("id_internal", "");
                IXMLDOMElement card_access = xml_doc.createElement("card_access");
                card_access.setAttribute("id_card", employee.ID_Card);
                card_access.setAttribute("shablon_id_external", "");
                card_access.setAttribute("shablon_id_internal", "");
                employ.appendChild(card_access);
                employs.appendChild(employ);
                login.appendChild(employs);
                element.appendChild(login);
                if (_percoSDK.Withdraw_Access(xml_doc) != 0)
                {
                    DOMDocument30Class Error = new DOMDocument30Class();
                    _percoSDK.GetErrorDescription(Error);
                    string _textError = "PERCo: Ошибка работы!\nСодержание ошибки:";
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
                    //    MessageBox.Show(_textError, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Error);                

                    //Вставка не удалась
                    isEmpUpdate = false;
                }
                else
                {
                    xml_doc = new DOMDocument30Class();
                    xml_doc.appendChild(_xml_doc.createProcessingInstruction("xml", @"version = ""1.0"" encoding = ""windows-1251"" standalone = ""yes"""));
                    element = xml_doc.createElement("documentrequest");
                    element.setAttribute("type", "sendcards");
                    element.setAttribute("mode", "withdraw_and_insert_access_staff");
                    xml_doc.appendChild(element);
                    login = xml_doc.createElement("login");
                    login.setAttribute("loginname", _loginName);
                    employs = xml_doc.createElement("employs");
                    employ = xml_doc.createElement("employ");
                    employ.setAttribute("id_external", employee.ID);
                    employ.setAttribute("id_internal", "");
                    card_access = xml_doc.createElement("card_access");
                    card_access.setAttribute("id_card", employee.ID_Card);
                    card_access.setAttribute("shablon_id_external", "");
                    card_access.setAttribute("shablon_id_internal", employee.ID_Shablon);
                    employ.appendChild(card_access);
                    employs.appendChild(employ);
                    login.appendChild(employs);
                    element.appendChild(login);
                    if (_percoSDK.Append_Access(xml_doc) != 0)
                    {
                        DOMDocument30Class Error = new DOMDocument30Class();
                        _percoSDK.GetErrorDescription(Error);
                        string _textError = "PERCo: Ошибка работы!\nСодержание ошибки:";
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
                    }
                    //Вставка удалась
                    isEmpUpdate = true;
                }

                return isEmpUpdate;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Обновляем сотрудника
        /// </summary>
        /// <param name="employee">Обновляемый сотрудник</param>
        /// <param name="useBuffer">используем ли мы буффер</param>
        /// <returns>выполнелось ли обновление</returns>
        public bool UpdateAccessEmployee(Employee employee)
        {
            bool isEmpUpdate = true;
            if (_percoSDK.SetConnect(_ipAddress, _port, "ADMIN", "hn-j[hfyf") == 0)
            {
                //Обновляем данные сотрудника
                isEmpUpdate = UpdateAccess(employee);
                //Разединяемся с перко
                _percoSDK.DisConnect();
            }
            else //Если не смогли открыть соединение
            {
                isEmpUpdate = false;
            }
            return isEmpUpdate;
        }
    }
}