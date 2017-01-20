using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.IO;
using Oracle.DataAccess.Client;
using LibraryKadr;
namespace Staff
{
    #region Базовые класы для последовательности

    #region Классы атрибутов
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ColumnAttribute : Attribute
    {
        private string _cSharpType;
        private int _dataLength;
        private bool _nullable;
        private string _comment;
        private string _caption;
        private bool _fromSequence;
        private string _oracleDataType;
        private string _refTable;
        private bool _visible;
        private bool _primary;
        public ColumnAttribute()
        {
        }
        public string CScharpType
        {
            get
            {
                return _cSharpType;
            }
            set
            {
                _cSharpType = value;
            }
        }
        public int DataLength
        {
            get
            {
                return _dataLength;
            }
            set
            {
                _dataLength = value;
            }
        }
        public bool Nullable
        {
            get
            {
                return _nullable;
            }
            set
            {
                _nullable = value;
            }
        }
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }
        public bool FromSequence
        {
            get
            {
                return _fromSequence;
            }
            set
            {
                _fromSequence = value;
            }
        }
        public string OracleDataType
        {
            get
            {
                return _oracleDataType;
            }
            set
            {
                _oracleDataType = value;
            }
        }
        public string RefTable
        {
            get
            {
                return _refTable;
            }
            set
            {
                _refTable = value;
            }
        }
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }
        public bool Primary
        {
            get
            {
                return _primary;
            }
            set
            {
                _primary = value;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        private string _schema;
        private string _tableName;
        private string _comment;
        public string Schema
        {
            get
            {
                return _schema;
            }
            set
            {
                _schema = value;
            }
        }
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
    }
    #endregion

    #region Дополнительные классы
    public class Functions
    {
        public static string nameUser;
        public static string pathToLog;
        public static string GetSqlPrivileges(string scheme, string tableName)
        {
            string sql = string.Format(
            " select case (select count(*) from " +
            " (select owner,table_name,privilege from user_tab_privs " +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union " +
            " select owner,table_name,privilege from role_tab_privs) " +
            " where table_name = upper('delete_{0}') " +
            " and owner = upper('{1}') and privilege = upper('execute')) " +
            @" when 1 then 'True' else 'False' end as ""delete"", " +
            " case (select count(*) from " +
            " (select owner,table_name,privilege from user_tab_privs " +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union " +
            " select owner,table_name,privilege from role_tab_privs) " +
            " where table_name = upper('update_{0}') " +
            " and owner = upper('{1}') and privilege = upper('execute')) " +
            @" when 1 then 'True' else 'False' end as ""update"", " +
            " case " +
            " case " +
            " (select count(*) from all_constraints where constraint_name in " +
            " (select constraint_name from all_cons_columns where owner = upper('{1}') " +
            " and table_name = upper('{0}') and column_name like '%_ID') and constraint_type = 'P') " +
            " when" +
            " 0 " +
            " then " +
            " (select count(*) from dual where exists " +
            " (select * from (select owner,table_name,privilege from user_tab_privs " +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union " +
            " select owner,table_name,privilege from role_tab_privs) " +
            " where table_name = upper('insert_{0}') " +
            " and owner = upper('{1}') and privilege = upper('execute'))) " +
            " else " +
            " (select count(*) from dual where " +
            " exists " +
            "(select owner,table_name,privilege from user_tab_privs" +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union" +
            " select owner,table_name,privilege from role_tab_privs " +
            " where table_name = upper('insert_{0}') " +
            " and owner = upper('{1}') and privilege = upper('execute')) " +
            " and exists " +
            " (select owner,table_name,privilege from user_tab_privs " +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union " +
            " select owner,table_name,privilege from role_tab_privs " +
            " where table_name = (select column_name||'_SEQ' from all_cons_columns col " +
            " where table_name = upper('{0}') and column_name like '%_ID'" +
            " and owner = upper('{1}') and " +
            " (select constraint_type from all_constraints tab " +
            " where  tab.constraint_name = col.constraint_name) = 'P') " +
            " and owner = upper('{1}') and privilege = upper('select'))) " +
            " end " +
            " when 1 then 'True' else 'False' " +
            @" end as ""insert"", " +
            " case (select count(*) from " +
            " (select owner,table_name,privilege from user_tab_privs " +
            " union " +
            " select owner,table_name,privilege from user_tab_privs_recd " +
            " union " +
            " select owner,table_name,privilege from role_tab_privs) " +
            " where table_name = upper('{0}') " +
            " and owner = upper('{1}') and privilege = upper('select')) " +
            @" when 1 then 'True' else 'False' end as ""select"" " +
            " from dual", tableName, scheme);
            return sql;
        }
        public static void ShowMessage(OracleException ex)
        {
            string message = ex.Message;
            //string rez = "ORA-00001: нарушено ограничение уникальности (KADRI.EMP_PER_NUM_IDX)\nORA-06512: на  \"KADRI.INSERT_EMP\", line 4\nORA-06512: на  line 2";
            //bool result = rez.Contains(string.Format("{0}.EMP_PER_NUM_IDX",DataSourceScheme.SchemeName));
            Dictionary<int, string> listOfError = new Dictionary<int, string>();
            listOfError.Add(54, "Эта строка в данный момент редактируется другим пользователем");
            if (listOfError.Select(s => s.Key).Contains(ex.ErrorCode))
                MessageBox.Show(listOfError[ex.ErrorCode]);
            //Если уже такой табельный есть
            else if (message.Contains(string.Format("{0}.EMP_PER_NUM_IDX", DataSourceScheme.SchemeName.ToUpper())))
                MessageBox.Show("Такой табельный номер уже занят, укажите другой табельный номер");
            else
            {
                string err = string.Format("Код ошибки {0}, описание ошибки: {1} данные: {2}", ex.ErrorCode, ex.Message, ex.Data);
                //string path = string.Format(@"\Log\{0}-{1:d2}-{2:d2}_{3:d2}-{4:d2}-{5:d2}_{6}.err", DateTime.Now.Year, DateTime.Now.Month,
                //    DateTime.Now.Day, DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds,
                //    nameUser != null ? nameUser.ToUpper() : ""); 
                string path = string.Format(@"\{0}.{1:d2}.{2:d2} {3:d2}.{4:d2}.{5:d2} - {6}_{7}.err", DateTime.Now.Year, 
                    DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, 
                    DateTime.Now.TimeOfDay.Seconds, nameUser != null ? nameUser.ToUpper() : "", ex.ErrorCode);
                //TextWriter writer = new StreamWriter(Application.StartupPath + path, false, Encoding.GetEncoding(1251));
                try
                {
                    TextWriter writer = new StreamWriter(pathToLog + path, false, Encoding.GetEncoding(1251));
                    writer.WriteLine(string.Format("Код ошибки: {0}", ex.ErrorCode));
                    writer.WriteLine("Описание ошибки:");
                    string str = ex.Message;
                    int i, pos;
                    i = pos = 0;
                    while (i < str.Length)
                    {
                        pos = str.IndexOf("\n", i);
                        if (pos > 0)
                        {
                            writer.WriteLine(str.Substring(i, pos - i));
                            i = pos + 1;
                        }
                        else
                        {
                            writer.WriteLine(str.Substring(i, str.Length - i));
                            i = str.Length;
                        }
                    }
                    writer.WriteLine(string.Format("Данные: {0}", ex.Data));
                    writer.Close();
                    MessageBox.Show(string.Format("Код ошибки {0}, описание ошибки: {1} данные: {2}", ex.ErrorCode, ex.Message, ex.Data));   
                }
                catch
                {
                    MessageBox.Show(string.Format("Невозможно записать файл ошибки по пути:\n{0}", pathToLog));
                }                             
            }
        }

        public static void ShowMessage(OracleException ex, string sql)
        {
            string message = ex.Message;
            //string rez = "ORA-00001: нарушено ограничение уникальности (KADRI.EMP_PER_NUM_IDX)\nORA-06512: на  \"KADRI.INSERT_EMP\", line 4\nORA-06512: на  line 2";
            //bool result = rez.Contains(string.Format("{0}.EMP_PER_NUM_IDX",DataSourceScheme.SchemeName));
            Dictionary<int, string> listOfError = new Dictionary<int, string>();
            listOfError.Add(54, "Эта строка в данный момент редактируется другим пользователем");
            if (listOfError.Select(s => s.Key).Contains(ex.ErrorCode))
                MessageBox.Show(listOfError[ex.ErrorCode]);
            //Если уже такой табельный есть
            else if (message.Contains(string.Format("{0}.EMP_PER_NUM_IDX", DataSourceScheme.SchemeName.ToUpper())))
                MessageBox.Show("Такой табельный номер уже занят, укажите другой табельный номер");
            else
            {
                string err = string.Format("Код ошибки {0}, описание ошибки: {1} данные: {2}", ex.ErrorCode, ex.Message, ex.Data);
                //string path = string.Format(@"\Log\{0}-{1:d2}-{2:d2}_{3:d2}-{4:d2}-{5:d2}_{6}.err", DateTime.Now.Year, DateTime.Now.Month,
                //    DateTime.Now.Day, DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds,
                //    nameUser != null ? nameUser.ToUpper() : ""); 
                string path = string.Format(@"\{0}.{1:d2}.{2:d2} {3:d2}.{4:d2}.{5:d2} - {6}_{7}.err", DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes,
                    DateTime.Now.TimeOfDay.Seconds, nameUser != null ? nameUser.ToUpper() : "", ex.ErrorCode);
                //TextWriter writer = new StreamWriter(Application.StartupPath + path, false, Encoding.GetEncoding(1251));
                try
                {
                    TextWriter writer = new StreamWriter(pathToLog + path, false, Encoding.GetEncoding(1251));
                    writer.WriteLine(string.Format("Код ошибки: {0}", ex.ErrorCode));
                    writer.WriteLine("Описание ошибки:");
                    string str = ex.Message;
                    int i, pos;
                    i = pos = 0;
                    while (i < str.Length)
                    {
                        pos = str.IndexOf("\n", i);
                        if (pos > 0)
                        {
                            writer.WriteLine(str.Substring(i, pos - i));
                            i = pos + 1;
                        }
                        else
                        {
                            writer.WriteLine(str.Substring(i, str.Length - i));
                            i = str.Length;
                        }
                    }
                    writer.WriteLine(string.Format("Данные: {0}", ex.Data));
                    writer.WriteLine(string.Format("Запрос: {0}", sql));
                    writer.Close();
                    MessageBox.Show(string.Format("Код ошибки {0}, описание ошибки: {1} данные: {2}", ex.ErrorCode, ex.Message, ex.Data));
                }
                catch
                {
                    MessageBox.Show(string.Format("Невозможно записать файл по пути:\n{0}", pathToLog));
                }                
            }
        }
    }

    public class CreateCopy
    {
        public static object CopyObject(object input)
        {
            if (input is string)
                return input.ToString().Clone();
            else if (input is DateTime)
            {
                DateTime buff = new DateTime();
                DateTime inputDateTime = (DateTime)input;
                buff = buff.AddMilliseconds(inputDateTime.Millisecond);
                buff = buff.AddSeconds(inputDateTime.Second);
                buff = buff.AddMinutes(inputDateTime.Minute);
                buff = buff.AddHours(inputDateTime.Hour);
                buff = buff.AddDays(inputDateTime.Day - 1);
                buff = buff.AddMonths(inputDateTime.Month - 1);
                buff = buff.AddYears(inputDateTime.Year - 1);
                return (object)buff;
            }
            else if (input is decimal)
                return (object)input;
            else if (input is bool)
                return (object)input;
            else
                return null;
        }
    }
    public class PreviousStateEventArgs : EventArgs
    {
        object _objectBefore;
        public PreviousStateEventArgs()
        {
        }
        public PreviousStateEventArgs(object objectBefore)
        {
            _objectBefore = objectBefore;
        }
        public object ObjectBefore
        {
            get { return _objectBefore; }
        }
    }
    #endregion

    #region Класс DataObject

    public abstract class DataObject
    {
        //protected void Clear(object childType, params object[] enums)
        //{
        //    if (enums != null)
        //    {
        //    }
        //    if (enums.Count() != 0)
        //    {
        //        List<string> names = new List<string>();
        //        foreach (object en in enums)
        //            names.Add(Enum.GetName(enums[0].GetType(), en));
        //        PropertyInfo[] properties = childType.GetType().GetProperties();
        //        PropertyInfo[] rezultProperties =
        //        (from pr in properties
        //         from name in names
        //         where name.ToUpper() == pr.Name.ToUpper()
        //         select pr).ToArray();
        //        foreach (PropertyInfo property in rezultProperties)
        //        {
        //            if (property.PropertyType != typeof(bool))
        //                property.SetValue(childType, null, null);
        //            else
        //                property.SetValue(childType, false, null);
        //        }
        //    }
        //}
        //protected void Clear(object childType, Type enumType)
        //{
        //    string[] enumNames = Enum.GetNames(enumType);
        //    PropertyInfo[] properties = (from propertry in childType.GetType().GetProperties()
        //                                 from name in enumNames
        //                                 where propertry.Name == name
        //                                 select propertry).ToArray();
        //    foreach (PropertyInfo property in properties)
        //    {
        //        if (property.PropertyType != typeof(bool))
        //            property.SetValue(childType, null, null);
        //        else
        //            property.SetValue(childType, false, null);
        //    }
        //}

        //protected void Clear( params object[] enums)
        //{
        //    if (enums != null)
        //    {
        //    }
        //    if (enums.Count() != 0)
        //    {
        //        List<string> names = new List<string>();
        //        foreach (object en in enums)
        //            names.Add(Enum.GetName(enums[0].GetType(), en));
        //        PropertyInfo[] properties = this.GetType().GetProperties();
        //        PropertyInfo[] rezultProperties =
        //        (from pr in properties
        //         from name in names
        //         where name.ToUpper() == pr.Name.ToUpper()
        //         select pr).ToArray();
        //        foreach (PropertyInfo property in rezultProperties)
        //        {
        //            if (property.PropertyType != typeof(bool))
        //                property.SetValue(this, null, null);
        //            else
        //                property.SetValue(this, false, null);
        //        }
        //    }
        //}
        /// <summary>
        /// Очистка занчений всех полей
        /// </summary>
        public void Clear()
        {
            string[] names = (from property in this.GetType().GetProperties()
                              from attributes in property.GetCustomAttributes(false)
                              where attributes is ColumnAttribute
                              select property.Name).ToArray();
            Clear(names);
        }
        /// <summary>
        /// Функция очистки значений полей строки таблицы
        /// </summary>
        /// <param name="enums">Поля перечисления класса _seq.ColumnNames </param>
        public void Clear(params object[] enums)
        {

            if (enums != null && enums.Count() != 0)
            {
                List<string> names = new List<string>();
                foreach (object en in enums)
                    names.Add(Enum.GetName(enums[0].GetType(), en));
                Clear(names);
            }
        }
        /// <summary>
        /// Функция очистки значений полей строки таблицы
        /// </summary>
        /// <param name="names">Имена колонок таблицы</param>
        public void Clear(params string[] names)
        {
            //string[] enumNames = Enum.GetNames(enumType);
            PropertyInfo[] properties = (from propertry in this.GetType().GetProperties()
                                         from name in names
                                         where propertry.Name.ToUpper() == name.ToUpper()
                                         select propertry).ToArray();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType != typeof(bool))
                    property.SetValue(this, null, null);
                else
                    property.SetValue(this, false, null);
            }
            //   Seq.BindingSource.ResetBindings(true);
        }
        protected IDataSequence _seq;
        /// <summary>
        /// Интерфейс IDataSequence, который реализует любой класс _seq
        /// </summary>
        [Browsable(false)]
        public IDataSequence Seq
        {
            get
            {
                return _seq;
            }
            set
            {
                _seq = value;
            }
        }
        //protected EMP_seq _emp_seq;
        //[Browsable(false)]
        //public EMP_seq EmpSeq
        //{
        //    get
        //    {
        //        return _emp_seq;
        //    }
        //    set
        //    {
        //        _emp_seq = value;
        //    }
        //}
    }
    #endregion

    #region Класс DataStorage
    public class DataStorage<TClass> : MarshalByValueComponent, IListSource, IEnumerable<TClass>
    {
        BindingList<TClass> _bingingList = new BindingList<TClass>();
        #region IListSource Members

        public bool ContainsListCollection
        {
            get { return false; }
        }

        public IList GetList()
        {
            return _bingingList;
        }

        #endregion
        protected virtual object AddNewCore()
        {
            return _bingingList.AddNew();
        }
        protected virtual void RemoveItem(int index)
        {
            _bingingList.RemoveAt(index);
        }
        public TClass this[int index]
        {
            get
            {
                return _bingingList[index];
            }
            set
            {
                _bingingList[index] = value;
            }
        }
        public void Add(TClass obj)
        {
            _bingingList.Add(obj);
        }
        public int Count
        {
            get
            {
                return _bingingList.Count;
            }
        }
        public void Clear()
        {
            _bingingList.Clear();
        }
        public void Remove(TClass obj)
        {
            _bingingList.Remove(obj);
        }

        #region IEnumerable<TClass> Members

        public IEnumerator<TClass> GetEnumerator()
        {
            foreach (TClass obj in _bingingList)
                yield return obj;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (TClass obj in _bingingList)
                yield return obj;
        }

        #endregion
    }
    #endregion

    public class FillPage
    {
        int _page = -1;
        int _startRecord = 0;
        int _maxRecord = 0;
        bool _isPageByFill = false;
        public FillPage()
        {
        }
        public bool IsPageByFill
        {
            get
            {
                return _isPageByFill;
            }
            set
            {
                if (!value)
                    _page = -1;
                _isPageByFill = value;
            }
        }
        public int StartRecord
        {
            get
            {
                return _startRecord;
            }
        }
        public int MaxRecord
        {
            get
            {
                return _maxRecord;
            }
        }
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _isPageByFill = true;
                _startRecord = 0;
                _maxRecord = value;
                _page = value;
            }
        }
        public void NextPage()
        {
            _startRecord += _page;
            _maxRecord += _page;
        }
    }

    #region Класс DataSequence
    //DataStorage
    //BindingList
    public abstract class DataSequence<class_obj> : BindingList<class_obj>, IDataSequence
        where class_obj : DataObject
    {
        protected OracleConnection _connection;
        protected string _sql;
        protected string _whereSql;
        protected bool _isSorted = false;
        //protected int _page = -1;
        protected FillPage _page = new FillPage();
        protected PropertyDescriptor _property;
        protected ListSortDirection _direction;
        protected List<class_obj> _inserted = new List<class_obj>();
        protected List<class_obj> _deleted = new List<class_obj>();
        protected List<class_obj> _updated = new List<class_obj>();
        protected List<class_obj> _previous = new List<class_obj>();
        // protected BindingSource _bindingSource = new BindingSource();
        //public abstract bool Fill();
        //public abstract bool Fill(string sql);
        public abstract void Save();
        protected abstract bool Read(OracleDataReader reader);
        public abstract void RollBack();
        public DataSequence()
        {
            if (DataSourceScheme.SchemeName == null || DataSourceScheme.SchemeName == "")
                throw new Exception("Вы не задали имя схемы из которой берутся данные");
            _schemeDataSource = DataSourceScheme.SchemeName;
        }
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<class_obj> itemsList = (List<class_obj>)this.Items;
            if (prop.PropertyType.GetInterface("IComparable") != null || prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(decimal?))
            {
                itemsList.Sort(new Comparison<class_obj>(delegate(class_obj x, class_obj y)
                {
                    if (prop.GetValue(x) != null)
                        return ((IComparable)prop.GetValue(x)).CompareTo(prop.GetValue(y)) * (direction == ListSortDirection.Descending ? -1 : 1);
                    else if (prop.GetValue(y) != null)
                        return ((IComparable)prop.GetValue(y)).CompareTo(prop.GetValue(x)) * (direction == ListSortDirection.Descending ? 1 : -1);
                    else
                        return 0;
                }));
                _isSorted = true;
                _property = prop;
                _direction = direction;
            }
        }
        protected static string _schemeDataSource;
        /// <summary>
        /// Наименование схемы, из которой беруться данные
        /// </summary>
        public static string SchemeDataSource
        {
            get
            {
                return _schemeDataSource;
            }
            set
            {
                _schemeDataSource = value;
            }
        }
        /// <summary>
        /// Функция дозаполнения таблицы по условию
        /// <summary>
        /// <param name = "sql">Условие запроса (начинается с "where ...")</param>
        protected bool AppendPage()
        {
            string sql = _sql + _whereSql;
            try
            {
                if (_page.IsPageByFill)//Если постраничная подгрузка
                {
                    /*OracleCommand command = new OracleCommand(sql, _connection);
                    OracleDataReader reader = command.ExecutePageReader(CommandBehavior.Default, _page.StartRecord, _page.Page);
                    command.FetchSize = 500;
                    while (this.Read(reader)) ;
                    reader.Close();*/
                    _page.NextPage();//Слудующая страница
                }
                else //Обыкновенная подгрузка
                {
                    OracleCommand command = new OracleCommand(sql, _connection);
                    command.BindByName = true;
                    OracleDataReader reader = command.ExecuteReader();
                    while (this.Read(reader)) ;
                    reader.Close();
                }
                return true;
            }
            catch (OracleException ex)
            {
                Functions.ShowMessage(ex, sql);
                return false;
            }
        }
        /// <summary>
        /// Получение следующей страницы класса
        /// </summary>
        public bool NextPage()
        {
            if (_page.IsPageByFill)
                return AppendPage();
            else
                return false;
        }
        /// <summary>
        /// Функция заполнения таблицы
        /// <summary>
        public bool Fill()
        {
            return Fill("");
        }
        /// <summary>
        /// Функция заполнения таблицы по условию
        /// <summary>
        /// <param name = "sql">Условие запроса (начинается с "where ...")</param>
        public bool Fill(string whereSql)
        {
            _page.IsPageByFill = false;
            _whereSql = whereSql;
            _inserted.Clear();
            _deleted.Clear();
            _updated.Clear();
            this.Clear();
            return AppendPage();
        }
        /// <summary>
        /// Функция постраничного заполнения
        /// </summary>
        /// <param name="page">Страница (по сколько строк будет происходить подгрузка)</param>
        /// <returns></returns>
        public bool FillByPage(int page)
        {
            return FillByPage(page, "");
        }
        public bool FillByPage(int page, string whereSql)
        {

            _page.IsPageByFill = true;
            _page.Page = page;
            _whereSql = whereSql;
            _inserted.Clear();
            _deleted.Clear();
            _updated.Clear();
            this.Clear();
            return AppendPage();
        }
        protected override bool IsSortedCore
        {
            get
            {
                return _isSorted;
            }
        }
        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }
        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return _direction;
            }
        }
        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                return _property;
            }
        }
        /// <summary> 
        /// Функция проверки изменения данных 
        /// </summary> 
        /// <returns> Изменились ли данные, после последнего сохранения</returns>
        public bool IsDataChanged()
        {
            if (_inserted.Count != 0 || _deleted.Count != 0 || _updated.Count != 0)
                return true;
            return false;
        }
        /// <summary> 
        /// Функция, которая очишает последовательность
        /// без применения изменений
        /// </summary> 
        public void ClearWithoutAcceptChanges()
        {
            this.Clear();
            _inserted.Clear();
            _deleted.Clear();
            _updated.Clear();
            _previous.Clear();
        }
        protected bool _canInsert;
        /// <summary>
        /// Свойство, которое определяет
        /// есть ли права на вставку строк в таблицу 
        /// <summary>
        public bool CanInsert
        {
            get
            {
                return _canInsert;
            }
        }
        protected bool _canUpdate;
        /// <summary>
        /// Свойство, которое определяет
        /// есть ли права на обновление строк в таблицу 
        /// <summary>
        public bool CanUpdate
        {
            get
            {
                return _canUpdate;
            }
        }
        protected bool _canDelete;
        /// <summary>
        /// Свойство, которое определяет
        /// есть ли права на удаление строк из таблицы 
        /// <summary>
        public bool CanDelete
        {
            get
            {
                return _canDelete;
            }
        }
        protected bool _canSelect;
        /// <summary>
        /// Свойство, которое определяет
        /// есть ли права на выборку данных из таблицы 
        /// <summary>
        public bool CanSelect
        {
            get
            {
                return _canSelect;
            }
        }
        /// <summary>
        /// Соединение с Oracle
        /// </summary>
        public OracleConnection Connection
        {
            get
            {
                return _connection;
            }
        }
        /// <summary>
        /// Свойство, определющее используется ли 
        /// постраничная загрузка данных
        /// </summary>
        public bool IsPageFill
        {
            get
            {
                return _page.IsPageByFill;
            }
        }
        private string _dataSourceSchemeName;
        /// <summary>
        /// Наименоваие схемы, из которой беруться и изменяются данные
        /// </summary>
        public string DataSourceSchemeName
        {
            get
            {
                return _dataSourceSchemeName;
            }
            set
            {
                _dataSourceSchemeName = value;
            }
        }
        ///// <summary>
        ///// Закончина ли привязка данных к объекту
        ///// </summary>
        //public bool IsBindingComplited
        //{
        //    get
        //    {
        //        return _isBindingComplited;
        //    }
        //    set
        //    {
        //        _isBindingComplited = value;
        //    }
        //}
        /// <summary>
        /// Получение BindingSource данного элемента
        /// </summary>

        //BindingSource IDataSequence.BindingSource
        //{
        //    get
        //    {
        //        return _bindingSource;
        //    }
        //}
    }

    #endregion

    #region Интерфейсы
    public interface IFillData : IFill, IFillByPage
    {

    }
    public interface IFill
    {
        bool Fill();
        bool Fill(string whereSql);
        bool IsPageFill { get; }
    }
    public interface IFillByPage
    {
        bool FillByPage(int page);
        bool FillByPage(int page, string whereSql);
    }
    public interface ISaveData
    {
        void Save();
    }
    public interface IRollBack
    {
        void RollBack();
        bool IsDataChanged();
    }
    public interface IRulesOnTable
    {
        bool CanDelete { get; }
        bool CanInsert { get; }
        bool CanUpdate { get; }
        bool CanSelect { get; }
    }
    public interface IManipulationData : ISaveData, IFillData, IRollBack
    {

    }
    public interface IDataSequence : IManipulationData, IRulesOnTable
    {
        OracleConnection Connection
        {
            get;
        }
    }
    #endregion

    /// <summary>
    /// Класс, который хранит в себе имя источника данных схемы
    /// </summary>
    public static class DataSourceScheme
    {
        private static int _count = 0;
        private static string _schemeName;
        /// <summary>
        /// Наименование схемы из которой беруться данные,
        /// наименование можно дать только один раз
        /// </summary>
        public static string SchemeName
        {
            get
            {
                return _schemeName;
            }
            set
            {
                if (_count == 0)
                {
                    _schemeName = value;
                    _count++;
                }
                else
                {
                    throw new Exception("Вы уже присвоили имя схеме");
                }
            }
        }
    }

    #endregion

    #region Классы надстройки
    public struct FullTableRef
    {
        public string RefTable;//Таблица на которую ссылается
        public string RefColumn;//Колонка таблицы на которое ссылаются
    }
    public static class DataGridViewSuperStructure
    {
        /// <summary>
        /// Функция, которая добавляет BindingSource к DataGridView
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="dataSequence">DataSequence  таблице</param>
        public static void AddBindingSource(this DataGridView dataGrid, object dataSequence)
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            AddBindingSource(dataGrid, dataSequence, null);
        }
        /// <summary>
        /// Функция, которая добавляет BindingSource к DataGridView
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="dataSequence">DataSequence таблицы</param>
        /// <param name="linkAguments">Данные о внешней таблице</param>
        public static void AddBindingSource(this DataGridView dataGrid, object dataSequenece, params LinkArgument[] linkAguments)
        {
            if (dataSequenece is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            object obj = dataSequenece;
            Type row_obj = GetObjFromSeq(obj.GetType());
            //Если такой тип есть
            if (row_obj != null)
            {
                PropertyInfo[] properties = row_obj.GetProperties();
                //Перебираем все свойства класса
                //        int linkAg = 0;
                //Количество вторичных ключей
                //if (linkAguments != null)
                //{
                //    int countOfForeignKey = properties.SelectMany(s => s.GetCustomAttributes(false).Where(k => k is ColumnAttribute).Select(m => (ColumnAttribute)m).Where(z => z.RefTable != "")).Count();
                //    if (countOfForeignKey > linkAguments.Count())
                //        throw new Exception("Вы ввели не данные о внешних ключах");
                //    else if (countOfForeignKey > linkAguments.Count())
                //        throw new Exception("Вы ввели лишние данные о внешних ключах");
                //}
                foreach (PropertyInfo property in properties)
                {
                    //Получаем аттрибут
                    ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                    //Если есть такой аттрибут
                    if (attribute != null)
                    {

                        //Если данное поле ссылается на таблицу, то
                        if (attribute.RefTable != "" && linkAguments != null)
                        {
                            //Получаем таблицу, на которую ссылаемся
                            FullTableRef tableRef = getTableAndField(attribute.RefTable);
                            //Создаем объект последовательности для данной таблицы
                            /*Переделать*/
                            LinkArgument linkAgument = linkAguments.Where(s => GetNameOfTable(s.Sequence.GetType()).ToUpper() == tableRef.RefTable.ToUpper()).FirstOrDefault(); //linkAguments.Where(s => s.RefField.ToUpper() == property.Name.ToUpper()).FirstOrDefault();
                            if (linkAgument != null && linkAgument.Sequence != null)
                            {
                                object refSeq = linkAgument.Sequence;//Activator.CreateInstance(refSeqType, connection);
                                //Заполняем
                                //refSeqType.InvokeMember("Fill", BindingFlags.Default | BindingFlags.InvokeMethod, null, refSeq, null);
                                //Создаем BindingSource
                                BindingSource bs = new BindingSource();
                                bs.DataSource = refSeq;
                                //Создаем колонку для dataGridView
                                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                                comboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                                comboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                comboBox.DataSource = bs;
                                comboBox.Name = property.Name.ToUpper();
                                comboBox.DataPropertyName = property.Name.ToUpper();
                                comboBox.ValueMember = tableRef.RefColumn.ToUpper();//Первичный ключ родительской таблицы
                                comboBox.DisplayMember = linkAgument.FieldName.ToUpper();//Поле, которое будет отображаться
                                comboBox.HeaderText = GetCommentForField(refSeq.GetType(), linkAgument.FieldName);
                                dataGrid.Columns.Add(comboBox);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    comboBox.Visible = false;
                            }
                            //          linkAg++;//Увеличиваем
                        }
                        else//в противном случае 
                        {
                            //Если это не тип bool
                            if (attribute.CScharpType != "bool")
                            {
                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                column.Name = property.Name.ToUpper();
                                column.DataPropertyName = property.Name.ToUpper();
                                column.HeaderText = attribute.Caption;
                                dataGrid.Columns.Add(column);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    column.Visible = false;
                            }
                            //Если тип bool
                            else if (attribute.CScharpType == "bool")
                            {
                                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                column.Name = property.Name.ToUpper();
                                column.DataPropertyName = property.Name.ToUpper();
                                column.TrueValue = "True";
                                column.FalseValue = "False";
                                column.HeaderText = attribute.Caption;
                                dataGrid.Columns.Add(column);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    column.Visible = false;
                            }
                        }
                    }
                }
            }
            //Присваеваем источник данных
            dataGrid.DataSource = dataSequenece;
            //08.04.2015 - убрал это, так как метода NextPage не существует и в программе возникала ошибка
            //ScrollBar scrollBar = dataGrid.Controls.Cast<ScrollBar>().Where(s => s is VScrollBar).FirstOrDefault();
            //scrollBar.Scroll += new ScrollEventHandler(delegate(object sender, ScrollEventArgs e)
            //{
            //    if (e.Type == ScrollEventType.EndScroll && dataGrid.Rows[dataGrid.Rows.Count - 1].Displayed)
            //        obj.GetType().InvokeMember("NextPage", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
            //});
            //08.04.2015

            //int count =  scrollBar.Controls.Count;
            ////scrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
            //dataGrid.MouseUp += new MouseEventHandler(delegate(object sender, MouseEventArgs e)
            //{
            //    if (e.Button == MouseButtons.Left)
            //    {
            //        MessageBox.Show("Работает");
            //    }
            //});
            ////dataGrid.MouseDown += new MouseEventHandler(delegate(object sender, MouseEventArgs e)
            ////{
            ////    if (e.Button == MouseButtons.Left)
            ////    {
            ////        isMouseDown = true;
            ////    }
            ////});
            ////Обрабатываем Scrolling
            //dataGrid.Scroll += new ScrollEventHandler(delegate(object sender, ScrollEventArgs e)
            //{
            //    //if (dataGrid.Rows[dataGrid.Rows.Count - 1].Displayed)
            //    //    obj.GetType().InvokeMember("NextPage", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
            //    //if (e.NewValue == dataGrid.Rows.Count - dataGrid.DisplayedRowCount(true))
            //    //    isEndOfList = true;
            //    //else
            //    //    isEndOfList = false;
            //        //if (isMouseDown)
            //        //{
            //        //    obj.GetType().InvokeMember("NextPage", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
            //        //    isMouseDown = false;
            //        //}
            //    if (e.Type == ScrollEventType.Last)
            //        MessageBox.Show("!!!!!!");
            //        //if (e.NewValue == dataGrid.Rows.Count - dataGrid.DisplayedRowCount(true))
            //        //    obj.GetType().InvokeMember("NextPage", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
            //});
        }
        /// <summary>
        /// Функция, которая добавляет BindingSource к DataGridView
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="dataSequence">DataSequence таблицы</param>
        /// <param name="linkAguments">Данные о внешней таблице</param>
        public static void AddBindingSource(this DataGridView dataGrid, object dataSequenece, bool columnSizeMode, params LinkArgument[] linkAguments)
        {
            if (dataSequenece is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            object obj = dataSequenece;
            Type row_obj = GetObjFromSeq(obj.GetType());
            //Если такой тип есть
            if (row_obj != null)
            {
                PropertyInfo[] properties = row_obj.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //Получаем аттрибут
                    ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                    //Если есть такой аттрибут
                    if (attribute != null)
                    {

                        //Если данное поле ссылается на таблицу, то
                        if (attribute.RefTable != "" && linkAguments != null)
                        {
                            //Получаем таблицу, на которую ссылаемся
                            FullTableRef tableRef = getTableAndField(attribute.RefTable);
                            //Создаем объект последовательности для данной таблицы
                            /*Переделать*/
                            LinkArgument linkAgument = linkAguments.Where(s => GetNameOfTable(s.Sequence.GetType()).ToUpper() == tableRef.RefTable.ToUpper()).FirstOrDefault(); //linkAguments.Where(s => s.RefField.ToUpper() == property.Name.ToUpper()).FirstOrDefault();
                            if (linkAgument != null && linkAgument.Sequence != null)
                            {
                                object refSeq = linkAgument.Sequence;//Activator.CreateInstance(refSeqType, connection);
                                //Заполняем
                                //refSeqType.InvokeMember("Fill", BindingFlags.Default | BindingFlags.InvokeMethod, null, refSeq, null);
                                //Создаем BindingSource
                                BindingSource bs = new BindingSource();
                                bs.DataSource = refSeq;
                                //Создаем колонку для dataGridView
                                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                                comboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                                comboBox.DataSource = bs;
                                comboBox.Name = property.Name.ToUpper();
                                comboBox.DataPropertyName = property.Name.ToUpper();
                                comboBox.ValueMember = tableRef.RefColumn.ToUpper();//Первичный ключ родительской таблицы
                                comboBox.DisplayMember = linkAgument.FieldName.ToUpper();//Поле, которое будет отображаться
                                comboBox.HeaderText = GetCommentForField(refSeq.GetType(), linkAgument.FieldName);
                                dataGrid.Columns.Add(comboBox);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    comboBox.Visible = false;
                            }
                            //          linkAg++;//Увеличиваем
                        }
                        else//в противном случае 
                        {
                            //Если это не тип bool
                            if (attribute.CScharpType != "bool")
                            {
                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.Name = property.Name.ToUpper();
                                column.DataPropertyName = property.Name.ToUpper();
                                column.HeaderText = attribute.Caption;
                                dataGrid.Columns.Add(column);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    column.Visible = false;
                            }
                            //Если тип bool
                            else if (attribute.CScharpType == "bool")
                            {
                                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
                                column.Name = property.Name.ToUpper();
                                column.DataPropertyName = property.Name.ToUpper();
                                column.TrueValue = "True";
                                column.FalseValue = "False";
                                column.HeaderText = attribute.Caption;
                                dataGrid.Columns.Add(column);
                                if (!attribute.Visible || property.Name.ToLower() == "data_change" || property.Name.ToLower() == "user_login")
                                    column.Visible = false;
                            }
                        }
                    }
                }
            }
            //Присваеваем источник данных
            dataGrid.DataSource = dataSequenece;
            ScrollBar scrollBar = dataGrid.Controls.Cast<ScrollBar>().Where(s => s is VScrollBar).FirstOrDefault();
            scrollBar.Scroll += new ScrollEventHandler(delegate(object sender, ScrollEventArgs e)
            {
                if (e.Type == ScrollEventType.EndScroll && dataGrid.Rows[dataGrid.Rows.Count - 1].Displayed)
                    obj.GetType().InvokeMember("NextPage", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
            });
        }
        //Функция, которая получает отдельно таблицу и отдельно колонку на которую ссылается
        public static FullTableRef getTableAndField(string fullRef)
        {
            List<string> rezult = new List<string>();
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < fullRef.Length; i++)
            {
                if (fullRef[i] != ';')
                {
                    buffer.Append(fullRef[i]);
                }
                else
                {
                    rezult.Add(buffer.ToString());
                    buffer.Remove(0, buffer.Length);
                }
            }
            return new FullTableRef() { RefTable = rezult[0], RefColumn = rezult[1] };
        }
        //Функция, которая получает коментарии к колонке
        private static string GetCommentForField(Type type_seq, string fieldName)
        {
            //ColumnAttribute attribute =  (ColumnAttribute)type.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
            //Получаем свойства
            Type type = GetObjFromSeq(type_seq);
            PropertyInfo property = type.GetProperties().Where(s => s.Name.ToUpper() == fieldName.ToUpper()).FirstOrDefault();
            ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
            return attribute.Caption;
        }
        //Функция, которая получает тип _obj из типа _seq
        public static Type GetObjFromSeq(Type seqType)
        {
            string nameOfClass_seq = seqType.Name;
            string namespaceOfClass = seqType.Namespace;
            //Получаем имя класса строки
            StringBuilder nameOfClass_obj = new StringBuilder();
            nameOfClass_obj.Append(nameOfClass_seq);
            nameOfClass_obj[nameOfClass_obj.Length - 3] = 'o';
            nameOfClass_obj[nameOfClass_obj.Length - 2] = 'b';
            nameOfClass_obj[nameOfClass_obj.Length - 1] = 'j';
            //Получаем тип класса
            //Assembly assembly = Assembly.GetEntryAssembly();
            Assembly assembly = Assembly.GetAssembly(typeof(Staff.EMP_obj));
            Type row_obj = assembly.GetType(string.Format("{0}.{1}", namespaceOfClass, nameOfClass_obj));
            return row_obj;
        }
        /// <summary>
        /// Функция полуения имя таблицы из имени класса
        /// </summary>
        /// <param name="seqType">Тип одного из классов созданных для таблицы</param>
        /// <returns>Имя таблицы, для которой создовался данный класс</returns>
        private static string GetNameOfTable(Type seqType)
        {
            string objectName = seqType.Name;
            StringBuilder rezult = new StringBuilder();
            for (int i = 0; i < objectName.Length - 4; i++)
            {
                rezult.Append(objectName[i]);
            }
            return rezult.ToString();
        }
        /* public static void View(this DataGridView dbv,object[] massivOfObject)
         {
             if (massivOfObject.Count() > 0)
             {
                 IEnumerable<string> rezult = from property in massivOfObject[0].GetType().GetProperties()
                                              from attribute in property.GetCustomAttributes(false)
                                              where attribute is ColumnAttribute && ((ColumnAttribute)attribute).Primary
                                              let rez = property.Name
                                              select rez;
                 string primaryKeyName = rezult.ToArray()[0];
                 Type typeOfObj = massivOfObject[0].GetType();
                 PropertyInfo[] properties = typeOfObj.GetProperties();
                 //Перебираем все колонки DataGridView
                 for (int i = 0; i < dbv.Rows.Count-1; i++)
                 {
                     for (int j = 0; j < massivOfObject.Count(); j++)
                     {
                         if (dbv[primaryKeyName, i].Value.ToString() == typeOfObj.InvokeMember(primaryKeyName, BindingFlags.Default | BindingFlags.GetProperty, null, massivOfObject[i], null).ToString())
                         {
                             dbv.Rows[i].Visible = true;
                         }
                         else
                         {
                             dbv.Rows[i].Visible = false;
                         }
                     }
                 }
             }
         }*/
    }
    /// <summary>
    /// Класс, хранящий в себе тип данной последовательности
    /// </summary>
    public class LinkArgument
    {
        //Отображаемое поле
        private string _fieldName;
        private object _sequence;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sequence">Класс родительской таблицы</param>
        /// <param name="fieldName">Имя, которое неоходимо отображать</param>
        public LinkArgument(object sequence, string fieldName)
        {
            //_type = type;
            _fieldName = fieldName;
            _sequence = sequence;
        }
        public LinkArgument(object sequence, object column)
        {
            _fieldName = Enum.GetName(column.GetType(), column);
            _sequence = sequence;
        }
        /// <summary>
        /// Имя поля, которое должно отображаться в combobox
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }
        /// <summary>
        /// Класс последовательности с подчеркиванием _seq
        /// </summary>
        public object Sequence
        {
            get { return _sequence; }
        }
    }
    //--------------------------------//
    //--------------------------------//
    //Надстройка для работы с TreeView//
    //--------------------------------//
    //--------------------------------//
    public static class TreeViewSuperStructure
    {
        public static void AddBindingSource(this TreeView treeView, BindingSource bs, string displayField)
        {
            object obj_seq = bs.DataSource;
            //Получаем тип последовательности
            Type typeSeq = obj_seq.GetType();
            //Получаем тип строки
            Type typeObj = GetObjFromSeq(typeSeq);
            PropertyInfo primary = GetPrimaryProperty(typeObj.GetProperties());
            //Получаем ссылочное свойство
            PropertyInfo reference = GetRefField(typeObj.GetProperties(), GetNameOfTable(typeObj));
            //Получаем отображаемое совойство
            PropertyInfo display = typeObj.GetProperties().Where(s => s.Name == displayField).FirstOrDefault();
            //Заполняем наше дерево
            FillTreeView(treeView, bs, displayField);
            //Обрабатываем событие раскрытия 
            treeView.BeforeExpand += new TreeViewCancelEventHandler(delegate(object sender, TreeViewCancelEventArgs e)
            {
                FillNode(e.Node, bs, displayField);
            });
            //Обрабатываем событие
            treeView.BeforeSelect += new TreeViewCancelEventHandler(delegate(object sender, TreeViewCancelEventArgs e)
            {
                IEnumerator enumerator = bs.GetEnumerator();
                enumerator.Reset();
                int position = 0;
                while (enumerator.MoveNext())
                {
                    if (Convert.ToDecimal(e.Node.Name) == Convert.ToDecimal(typeObj.InvokeMember(primary.Name, BindingFlags.Default | BindingFlags.GetProperty, null, enumerator.Current, null)))
                    {
                        //bs.Current = enumerator.Current;
                        bs.Position = position;
                        break;
                    }
                    position++;
                }
            });
            //Dictionary<TreeNode, object> binder = FillTreeView(treeView, obj_seq, displayField);
            /*  //Добавляем контекстное меню
              AddContextMenu(treeView);
              //Получаем конекстное меню
              ContextMenuStrip contextMenuStrip = treeView.ContextMenuStrip;
              TreeNode previousNode = null;
              //------------------------
              //Обрабатываем добавление
              //------------------------
              contextMenuStrip.Items["addToolStripMenuItem"].Click += new EventHandler(delegate(object sender, EventArgs e)
              {
                  if (treeView.SelectedNode != null)
                  {
                      //Если Предыдущий узел не равен нулю
                      //и у предыдущего узла нет идентификатора
                      if (previousNode != null && treeView.SelectedNode.Name == "")
                      {//Предлагаем пользователю сохраниться
                          if (MessageBox.Show("Вы пытаетесь добавить подразделение " +
                              " в несохраненное подразделение сохранить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                          {//Сохраняем изменеия
                              typeSeq.InvokeMember("Save", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, null);
                              //Получаем ключ для данного объекат
                              object Prev_Obj = binder[treeView.SelectedNode];
                              treeView.SelectedNode.Name = Convert.ToString(primary.GetValue(Prev_Obj, null));
                              //Добавляем узел
                              previousNode = treeView.SelectedNode.Nodes.Add("Новый узел");
                              //Получаем новый объект последовательности
                              object rez_obj = typeSeq.InvokeMember("AddNew", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, null);
                              //Заполняем ссылочное поле
                              reference.SetValue(rez_obj, Convert.ToDecimal(treeView.SelectedNode.Name), null);
                              //Заполняем отображаемое поле
                              display.SetValue(rez_obj, "Новый узел", null);
                              //Связываем
                              binder.Add(previousNode, rez_obj);
                          }
                      }
                      else //В противном случае добавляем без сохранения
                      {
                          previousNode = treeView.SelectedNode.Nodes.Add("Новый узел");
                          //Получаем новый объект последовательности
                          object rez_obj = typeSeq.InvokeMember("AddNew", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, null);
                          //Заполняем ссылочное поле
                          reference.SetValue(rez_obj, Convert.ToDecimal(treeView.SelectedNode.Name), null);
                          //Заполняем отображаемое поле
                          display.SetValue(rez_obj, "Новый узел", null);
                          //Связываем
                          binder.Add(previousNode, rez_obj);
                      }
                  }
                  //Доработать
                  else //В противном случае добавляем в корень дерева
                  {
                      previousNode = treeView.Nodes.Add("Новый узел");
                      object rez_obj = typeSeq.InvokeMember("AddNew", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, null);
                      //Заполняем ссылочное поле
                      //reference.SetValue(rez_obj, Convert.ToDecimal(treeView.SelectedNode.Name), null);
                      //Заполняем отображаемое поле
                      display.SetValue(rez_obj, "Новый узел", null);
                  }
              });
              //Обрабатываем удаление
              contextMenuStrip.Items["removeToolStripMenuItem"].Click += new EventHandler(delegate(object sender, EventArgs e)
              {
                  if (treeView.SelectedNode != null)
                  {
                      IBindingList bindingList = obj_seq as IBindingList;
                      //Удаляем
                      RemoveNodes(treeView.SelectedNode, binder, bindingList);
                  }
              });
              //Обрабатываем редактирование
              treeView.AfterLabelEdit += new NodeLabelEditEventHandler(delegate(object sender, NodeLabelEditEventArgs e)
              {
                  object cur_obj = binder[e.Node];
                  display.SetValue(cur_obj, e.Label, null);
              });*/
        }
        private static void FillTreeView(TreeView treeView, BindingSource bs, string displayField)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            object obj_seq = bs.DataSource;
            //Получаем тип последовательности
            Type typeSeq = obj_seq.GetType();
            //Получаем тип строки
            Type typeObj = GetObjFromSeq(typeSeq);
            PropertyInfo primary = GetPrimaryProperty(typeObj.GetProperties());
            //Получаем ссылочное свойство
            PropertyInfo reference = GetRefField(typeObj.GetProperties(), GetNameOfTable(typeObj));
            //Получаем отображаемое совойство
            PropertyInfo display = typeObj.GetProperties().Where(s => s.Name.ToUpper() == displayField.ToUpper()).FirstOrDefault();
            //Вызываем функцию заполения
            typeSeq.InvokeMember("FillСhildren", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, new Object[] { string.Format(" where tab1.{0} is null", reference.Name) });
            IEnumerator enumerator = bs.GetEnumerator();
            enumerator.Reset();
            //Перебираем все узлы
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                //Если принадлежит данному узлу
                if (Convert.ToString(typeObj.InvokeMember(reference.Name, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null)) == "")
                {
                    TreeNode childNode = nodes.Add(primary.GetValue(obj, null).ToString(), display.GetValue(obj, null).ToString());
                    //Если есть дети то добавляем еще один узел
                    if (Convert.ToBoolean(typeObj.InvokeMember("has_children".ToUpper(), BindingFlags.Default | BindingFlags.GetProperty, null, obj, null)))
                    {
                        childNode.Nodes.Add("extend");
                    }
                }
            }
        }
        private static void FillNode(TreeNode node, BindingSource bs, string displayField)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "extend")
            {
                //Удаляем временный узел
                node.Nodes.Remove(node.Nodes[0]);
                //Получаем ссылку на коллекцию узлов
                TreeNodeCollection nodes = node.Nodes;
                //Получаем уникальный идентификатор узла
                string nodeName = node.Name;
                object obj_seq = bs.DataSource;
                //Получаем тип последовательности
                Type typeSeq = obj_seq.GetType();
                //Получаем тип строки
                Type typeObj = GetObjFromSeq(typeSeq);
                PropertyInfo primary = GetPrimaryProperty(typeObj.GetProperties());
                //Получаем ссылочное свойство
                PropertyInfo reference = GetRefField(typeObj.GetProperties(), GetNameOfTable(typeObj));
                //Получаем отображаемое совойство
                PropertyInfo display = typeObj.GetProperties().Where(s => s.Name.ToUpper() == displayField.ToUpper()).FirstOrDefault();
                //Вызываем функцию заполения
                typeSeq.InvokeMember("FillСhildren", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj_seq, new Object[] { string.Format(" where {0} = {1}", reference.Name, nodeName) });
                IEnumerator enumerator = bs.GetEnumerator();
                enumerator.Reset();
                //Перебираем все узлы
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    //Если принадлежит данному узлу
                    if (typeObj.InvokeMember(reference.Name, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null) != null && Convert.ToDecimal(typeObj.InvokeMember(reference.Name, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null)) == Convert.ToDecimal(nodeName))
                    {
                        TreeNode childNode = nodes.Add(primary.GetValue(obj, null).ToString(), display.GetValue(obj, null).ToString());
                        //Если есть дети то добавляем еще один узел
                        if (Convert.ToBoolean(typeObj.InvokeMember("has_children".ToUpper(), BindingFlags.Default | BindingFlags.GetProperty, null, obj, null)))
                        {
                            childNode.Nodes.Add("extend");
                        }
                    }
                }
            }
        }

        private static void RemoveNodes(TreeNode node, Dictionary<TreeNode, object> binder, IBindingList list)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                RemoveNodes(currentNode, binder, list);
            }
            //Удаляем из последовательности
            list.Remove(binder[node]);
            //Удаляем из связывателя
            binder.Remove(node);
            //Удаляем из дерева
            node.Remove();
        }
        /// <summary>
        /// Фукция добавления конексного меню
        /// </summary>
        /// <param name="treeView"></param>
        private static void AddContextMenu(TreeView treeView)
        {
            ToolStripMenuItem addToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            addToolStripMenuItem.Text = "Добавить";

            ToolStripMenuItem removeToolStripMenuItem = new ToolStripMenuItem();
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            removeToolStripMenuItem.Text = "Удалить";

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            addToolStripMenuItem,
            removeToolStripMenuItem});

            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(136, 48);
            treeView.ContextMenuStrip = contextMenuStrip;
        }
        /* /// <summary>
         /// Функция заполнения дерева
         /// </summary>
         /// <param name="treeView">Дерево</param>
         /// <param name="obj_seq">Последовательность</param>
         /// <param name="displayField">Имя, которое необходимо отображать</param>
         /// <returns></returns>
         static Dictionary<TreeNode, object> FillTreeView(TreeView treeView, object obj_seq, string displayField)
         {
             //Получаем тип последовательности
             Type typeSeq = obj_seq.GetType();
             //Получаем тип строки
             Type typeObj = GetObjFromSeq(typeSeq);
             //Получаем свойство первичного ключа
             PropertyInfo primary = GetPrimaryProperty(typeObj.GetProperties());
             //Получаем ссылочное свойство
             PropertyInfo reference = GetRefField(typeObj.GetProperties(), GetNameOfTable(typeObj));
             //Получаем отображаемое совойство
             PropertyInfo display = typeObj.GetProperties().Where(s => s.Name == displayField).FirstOrDefault();
             //Список из последовательности
             IBindingList tableList = obj_seq as IBindingList;
             //Для связки кажого узла дерева с объектом последовательности
             Dictionary<TreeNode, object> binder = new Dictionary<TreeNode, object>();
             //Получаем массив вершинных объектов
             object[] ObjectWithoutRef = GetObjects(tableList, reference.Name, s => s == null);
             foreach (object current in ObjectWithoutRef)
             {
                 TreeNode node = treeView.Nodes.Add(typeObj.InvokeMember(primary.Name, BindingFlags.Default | BindingFlags.GetProperty, null, current, null).ToString(), typeObj.InvokeMember(displayField, BindingFlags.Default | BindingFlags.GetProperty, null, current, null).ToString());
                 //связываем
                 binder.Add(node, current);
                 decimal uniqueID = (decimal)primary.GetValue(current, null);
                 //Раскручиваем дерево
                 FillTreeViewNode(node, uniqueID, tableList, reference.Name, displayField, primary, binder);
             }
             return binder;
         }
         static void FillTreeViewNode(TreeNode treeNode, decimal uniqueID, IBindingList sequence, string sequenceName, string displayField, PropertyInfo primary, Dictionary<TreeNode, object> binder)
         {
             object[] rootObj = GetObjects(sequence, sequenceName, s => s == uniqueID);
             for (int i = 0; i < rootObj.Count(); i++)
             {
                 object current = rootObj[i];
                 string id = current.GetType().InvokeMember(primary.Name, BindingFlags.Default | BindingFlags.GetProperty, null, current, null).ToString();
                 string name = current.GetType().InvokeMember(displayField, BindingFlags.Default | BindingFlags.GetProperty, null, current, null).ToString();
                 //Связываем дерево с объектами
                 TreeNode node = treeNode.Nodes.Add(id, name);
                 binder.Add(node, current);
                 decimal uniqueIDNext = (decimal)primary.GetValue(current, null);
                 FillTreeViewNode(node, uniqueIDNext, sequence, sequenceName, displayField, primary, binder);
             }
         }*/
        /// <summary>
        /// Получения типа объекта из типа последовательности
        /// </summary>
        /// <param name="seqType"></param>
        /// <returns></returns>
        private static Type GetObjFromSeq(Type seqType)
        {
            string nameOfClass_seq = seqType.Name;
            string namespaceOfClass = seqType.Namespace;
            //Получаем имя класса строки
            StringBuilder nameOfClass_obj = new StringBuilder();
            nameOfClass_obj.Append(nameOfClass_seq);
            nameOfClass_obj[nameOfClass_obj.Length - 3] = 'o';
            nameOfClass_obj[nameOfClass_obj.Length - 2] = 'b';
            nameOfClass_obj[nameOfClass_obj.Length - 1] = 'j';
            //Получаем тип класса
            Assembly assembly = Assembly.GetEntryAssembly();
            Type row_obj = assembly.GetType(string.Format("{0}.{1}", namespaceOfClass, nameOfClass_obj));
            return row_obj;
        }
        /// <summary>
        /// Функция полуения имя таблицы из имени класса
        /// </summary>
        /// <param name="seqType">Тип одного из классов созданных для таблицы</param>
        /// <returns>Имя таблицы, для которой создовался данный класс</returns>
        private static string GetNameOfTable(Type seqType)
        {
            string objectName = seqType.Name;
            StringBuilder rezult = new StringBuilder();
            for (int i = 0; i < objectName.Length - 4; i++)
            {
                rezult.Append(objectName[i]);
            }
            return rezult.ToString();
        }
        private static PropertyInfo GetRefField(PropertyInfo[] properties, string tableName)
        {
            //Перебираем все поля
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                if (attribute.RefTable != "")
                {
                    FullTableRef tableRef = getTableAndField(attribute.RefTable);
                    if (tableRef.RefTable.ToUpper() == tableName.ToUpper())
                        return property;
                }
            }
            return null;
        }
        //Функция, которая получает отдельно таблицу и отдельно колонку на которую ссылается
        private static FullTableRef getTableAndField(string fullRef)
        {
            List<string> rezult = new List<string>();
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < fullRef.Length; i++)
            {
                if (fullRef[i] != ';')
                {
                    buffer.Append(fullRef[i]);
                }
                else
                {
                    rezult.Add(buffer.ToString());
                    buffer.Remove(0, buffer.Length);
                }
            }
            return new FullTableRef() { RefTable = rezult[0], RefColumn = rezult[1] };
        }
        //Получить первчный ключ
        private static PropertyInfo GetPrimaryProperty(PropertyInfo[] properties)
        {
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                if (attribute.Primary)
                {
                    return property;
                }
            }
            return null;
        }
        /// <summary>
        ///Функция получения массива объектов 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="Field">Имя поля для поиска</param>
        /// <param name="func">Функция поиска</param>
        /// <returns></returns>
        private static object[] GetObjects(IBindingList sequence, string Field, Func<decimal?, bool> func)
        {
            //Результирующий список объектов
            List<object> rezult = new List<object>();

            foreach (object obj in sequence)
            {
                decimal? propertyValue = (decimal?)obj.GetType().InvokeMember(Field, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null);
                if (func(propertyValue))
                    rezult.Add(obj);
            }
            return rezult.ToArray();
        }

    }
    //--------------------------------------//
    //--------------------------------------//
    //Надстройка для работы с DateTimePicker//
    //--------------------------------------//
    //--------------------------------------//
    public static class DateTimePickerSuperStructure
    {
        /// <summary>
        /// Функция подключения источника данных
        /// к DateTimePicker
        /// </summary>
        /// <typeparam name="TClass_obj">Класс строки таблицы с расширением "_obj"</typeparam>
        /// <typeparam name="TClass_seq">Класс последовательности с расширением "_seq"</typeparam>
        /// <param name="picker"></param>
        /// <param name="bindingSource">Источник данных</param>
        /// <param name="nameOfColumn">Наименоване колонки таблицы, которую необходимо отображать</param>
        public static void AddBindingSource<TClass_obj, TClass_seq>(this DateTimePicker picker, BindingSource bindingSource, string nameOfColumn)
        {

            TClass_obj obj_obj = (TClass_obj)bindingSource.Current;
            DateTime? time = (DateTime?)obj_obj.GetType().InvokeMember(nameOfColumn.ToUpper(), BindingFlags.Default | BindingFlags.GetProperty, null, obj_obj, null);
            if (time != null)
            {
                picker.Value = Convert.ToDateTime(time);
            }
            else
            {
                picker.Text = "";
            }

            bindingSource.CurrentChanged += new EventHandler(delegate(object sender, EventArgs e)
            {
                obj_obj = (TClass_obj)bindingSource.Current;
                time = (DateTime?)obj_obj.GetType().InvokeMember(nameOfColumn.ToUpper(), BindingFlags.Default | BindingFlags.GetProperty, null, obj_obj, null);
                if (time != null && time != DateTime.MinValue)
                {
                    picker.Value = Convert.ToDateTime(time);
                }
                else
                {
                    picker.Text = "";
                }
            });
            //Обрабатываем закрытие календаря
            picker.CloseUp += new EventHandler(delegate(object sender, EventArgs e)
            {
                obj_obj = (TClass_obj)bindingSource.Current;
                DateTime selectTime = new DateTime(picker.Value.Year, picker.Value.Month, picker.Value.Day);
                obj_obj.GetType().InvokeMember(nameOfColumn.ToUpper(), BindingFlags.Default | BindingFlags.SetProperty, null, obj_obj, new Object[] { (DateTime?)selectTime });
            });
            //Изменение текста
            picker.TextChanged += new EventHandler(delegate(object sender, EventArgs e)
            {
                obj_obj = (TClass_obj)bindingSource.Current;
                DateTime selectTime = new DateTime(picker.Value.Year, picker.Value.Month, picker.Value.Day);
                obj_obj.GetType().InvokeMember(nameOfColumn.ToUpper(), BindingFlags.Default | BindingFlags.SetProperty, null, obj_obj, new Object[] { (DateTime?)selectTime });
            });
        }
    }
    /*//-------------------------------
    //Надстройка для OracleConnection
    //-------------------------------
    public static class OracleConnectionSubstructure
    {
        public static bool IsTransactionBegin(this OracleConnection connection)
        {
            try
            {
                connection.BeginTransaction();
                connection.Commit();
                return false;
            }
            catch
            {
                return true;
            }
        }
    }*/
    public static class TextBoxSubStructure
    {
        /// <summary>
        ///Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="bs">Источник данных</param>
        /// <param name="column">Колонка из перечисления типа [Имя колонки]_seq.ColumnName</param>
        public static void AddBindingSource(this TextBox textBox, object dataSequence, object column)
        //   where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            textBox.DataBindings.Add("Text", dataSequence, Enum.GetName(column.GetType(), column), true, DataSourceUpdateMode.OnPropertyChanged, "");
        }
        /// <summary>
        /// Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="dataSequence">Источник данных типа DataSequence</param>
        /// <param name="columnName">Наименование колонки, для отображения</param>
        public static void AddBindingSource(this TextBox textBox, object dataSequence, string columnName)
        //where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            textBox.DataBindings.Add("Text", dataSequence, columnName, true, DataSourceUpdateMode.OnPropertyChanged, "");
        }
        /// <summary>
        /// Функция добавления источника данных
        /// </summary>
        /// <param name="maskedTextBox"></param>
        /// <param name="dataSequence">Источник данных</param>
        /// <param name="column">Колонка из перечисления типа [Имя колонки]_seq.ColumnName</param>
        public static void AddBindingSource(this MaskedTextBox maskedTextBox, object dataSequence, object column)
        //where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            maskedTextBox.DataBindings.Add("Text", dataSequence, Enum.GetName(column.GetType(), column), true, DataSourceUpdateMode.OnPropertyChanged, "");
        }
        /// <summary>
        /// Функция добавления источника данных
        /// </summary>
        /// <param name="maskedTextBox"></param>
        /// <param name="bs">Источник данных</param>
        /// <param name="columnName">Наименование колонки, для отображения</param>
        public static void AddBindingSource(this MaskedTextBox maskedTextBox, object dataSequence, string columnName)
        //where T : Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            maskedTextBox.DataBindings.Add("Text", dataSequence, columnName, true, DataSourceUpdateMode.OnPropertyChanged, "");
        }
    }

    public static class DateEditorSubStructure
    {
        /// <summary>
        ///Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="bs">Источник данных</param>
        /// <param name="column">Колонка из перечисления типа [Имя колонки]_seq.ColumnName</param>
        public static void AddBindingSource(this DateEditor dateEditor, object dataSequence, object column)
        //   where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            //dateEditor.DataBindings.Add("Date", dataSequence, Enum.GetName(column.GetType(), column), true, DataSourceUpdateMode.OnPropertyChanged, (DateTime?)null);
            dateEditor.DataBindings.Add("Date", dataSequence, Enum.GetName(column.GetType(), column), true, DataSourceUpdateMode.OnPropertyChanged, (DateTime?)null);
        }
        /// <summary>
        /// Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="dataSequence">Источник данных типа DataSequence</param>
        /// <param name="columnName">Наименование колонки, для отображения</param>
        public static void AddBindingSource(this DateEditor dateEditor, object dataSequence, string columnName)
        //where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            //dateEditor.DataBindings.Add("Date", dataSequence, columnName, true, DataSourceUpdateMode.OnPropertyChanged, (DateTime?)null);
            dateEditor.DataBindings.Add("Date", dataSequence, columnName, true, DataSourceUpdateMode.OnPropertyChanged, (DateTime?)null);
        }
    }

    public static class ComboBoxSubStructure
    {
        ///// <summary>
        ///// Функция которая только отображает данные, но не изменяет
        ///// </summary>
        ///// <param name="comboBox"></param>
        ///// <param name="linkAgumtent">Данные связующей таблицы</param>
        //public static void AddBindingSource(this ComboBox comboBox, LinkAgument linkAgumtent)
        //{
        //    comboBox.DataSource = (linkAgumtent.Sequnce as IDataSequence).BindingSource;
        //    comboBox.DisplayMember = linkAgumtent.FieldName;
        //}
        ///// <summary>
        ///// Функция привязки данных к ComboBox
        ///// </summary>
        ///// <param name="comboBox"></param>
        ///// <param name="dataSource">Источник данных (таблица)</param>
        ///// <param name="dataMember">Имя поля (вторичного ключа) в таблице</param>
        ///// <param name="linkAgumtent">Данные связующей таблицы (из которой берутся данные
        ///// по вторичному ключу)</param>
        //public static void AddBindingSource(this ComboBox comboBox, BindingSource dataSource, string dataMember, LinkAgument linkAgumtent)
        //{
        //    comboBox.DataBindings.Add("SelectedItem", dataSource, dataMember);
        //    AddBindingSource(comboBox, linkAgumtent);
        //}
        ///// <summary>
        ///// Функция привязки данных к ComboBox
        ///// </summary>
        ///// <param name="comboBox"></param>
        ///// <param name="dataSource">Источник данных (таблица)</param>
        ///// <param name="dataMember">Имя поля (вторичного ключа) в таблице (берется из перечисления [имя таблицы]_seq.ColumnName)</param>
        ///// <param name="linkAgumtent">Данные связующей таблицы (из которой берутся данные
        ///// по вторичному ключу)</param>
        //public static void AddBindingSource(this ComboBox comboBox, BindingSource dataSource, object dataMember, LinkAgument linkAgumtent)
        //{
        //    comboBox.DataBindings.Add("SelectedItem", dataSource, Enum.GetName(dataMember.GetType(), dataMember));
        //    AddBindingSource(comboBox, linkAgumtent);
        //}
        /// <summary>
        /// Функция которая только отображает данные, но не изменяет
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="linkAgumtent">Данные связующей таблицы</param>
        public static void AddBindingSource(this ComboBox comboBox, string dataMember, LinkArgument linkAgumtent)
        {
            //  comboBox.DataSource = (linkAgumtent.Sequnce as IDataSequence).BindingSource;
            comboBox.DataSource = linkAgumtent.Sequence;
            comboBox.DisplayMember = linkAgumtent.FieldName;
            comboBox.ValueMember = dataMember;
        }
        /// <summary>
        /// Функция привязки данных к ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="dataSource">Источник данных (таблица)</param>
        /// <param name="dataMember">Имя поля (вторичного ключа) в таблице</param>
        /// <param name="linkAgumtent">Данные связующей таблицы (из которой берутся данные
        /// по вторичному ключу)</param>
        public static void AddBindingSource(this ComboBox comboBox, object dataSource, string dataMember, LinkArgument linkAgumtent)
        // where T:Staff.DataObject
        {
            if (dataSource is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            comboBox.DataBindings.Add("SelectedValue", dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged, "");
            AddBindingSource(comboBox, dataMember, linkAgumtent);
        }
        /// <summary>
        /// Функция привязки данных к ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="dataSource">Источник данных (таблица)</param>
        /// <param name="dataMember">Имя поля (вторичного ключа) в таблице (берется из перечисления [имя таблицы]_seq.ColumnName)</param>
        /// <param name="linkAgumtent">Данные связующей таблицы (из которой берутся данные
        /// по вторичному ключу)</param>
        public static void AddBindingSource(this ComboBox comboBox, object dataSource, object dataMember, LinkArgument linkAgumtent)
        //where T:Staff.DataObject
        {
            if (dataSource is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");/* или мозгов ))) сперва надо заполнять источник данных, а потом уже привязывать к значениям!! исправить надо выше тоже*/
            AddBindingSource(comboBox, Enum.GetName(dataMember.GetType(), dataMember), linkAgumtent);
            comboBox.DataBindings.Add("SelectedValue", dataSource, Enum.GetName(dataMember.GetType(), dataMember), true, DataSourceUpdateMode.OnPropertyChanged, "");
        }
    }
    public static class CheckBoxSubStructure
    {
        /// <summary>
        ///Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="dataSequence">Объект типа DataSequence</param>
        /// <param name="column">Колонка из перечисления типа [Имя колонки]_seq.ColumnName</param>
        public static void AddBindingSource(this CheckBox checkBox, object dataSequence, object column)
        // where T:Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            checkBox.DataBindings.Add("Checked", dataSequence, Enum.GetName(column.GetType(), column));
        }
        /// <summary>
        /// Функция добавления источника данных
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="dataSequence">Источник данных</param>
        /// <param name="columnName">Наименование колонки, для отображения</param>
        public static void AddBindingSource(this CheckBox checkBox, object dataSequence, string columnName)
        // where T : Staff.DataObject
        {
            if (dataSequence is BindingSource)
                throw new Exception("Нельзя использовать bindingSource, он приводит к утечке памяти");
            checkBox.DataBindings.Add("Checked", dataSequence, columnName);
        }
    }
    #endregion
}