using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace LibraryKadr
{
    public enum TypeColValue
    {
        Query = 0,
        String = 1,
        Number = 2
    }
    public class SP
    {
        public string ColName
        { get; set; }
        public object ColumnValue
        { get; set; }
        public TypeColValue ValueType
        { get; set; }
        public SP(string _ColName, object _ColumnValue, TypeColValue _TypeValue)
        {
            ColName = _ColName;
            ColumnValue = _ColumnValue ?? "";
            ValueType = _TypeValue;
        }
        public SP(string _ColName, object _ColumnValue) : this(_ColName, _ColumnValue, TypeColValue.String) { }
    }
    public class DMLCommands
    {
        public static string InsertInto(string table, string Schema, OracleConnection connection, string[][] Values)
        {
            string s = "", st = "";

            for (int i = 0; i < Values.Length; i++)
                s += Values[i][0] + ",";

            s = s.Substring(0, s.Length - 1);

            for (int i = 0; i < Values.Length; i++)
                if (Values[i][1].Length > 0 && Values[i][1][0] == '(')
                    st += Values[i][1] + ",";
                else
                    st += "'" + Values[i][1] + "',";
            st = st.Substring(0, st.Length - 1);
            s = string.Format("INSERT INTO {0}.{1}({2}) VALUES({3})", Schema, table, s, st);
            s = (string)new OracleCommand(s, connection).ExecuteScalar();
            Connect.Commit();
            return s;
        }
        public static void Update(string table, string Schema, OracleConnection connection, string[][] Values, string Where)
        {
            string s = "";
            for (int i = 0; i < Values.Length; i++)
            {
                s += Values[i][0] + "=" + ((Values[i][1].Length > 0 && Values[i][1][0] == '(') ?
                                        Values[i][1] + "," :
                                        "'" + Values[i][1] + "',");
            }
            s = s.Substring(0, s.Length - 1);
            s = string.Format("UPDATE {0}.{1} SET  {2} where {3}", Schema, table, s, Where);
            new OracleCommand(s, connection).ExecuteNonQuery();
            Connect.Commit();
        }


        public static string InsertInto(string table,string Schema,OracleConnection conn, SP[] Values)
        {
            string s = "", st = "";
            for (int i = 0; i < Values.Length; i++)
                s += Values[i].ColName + ",";

            s = s.Substring(0, s.Length - 1);

            for (int i = 0; i < Values.Length; i++)
                if (Values[i].ValueType == TypeColValue.Query)
                    st += Values[i].ColumnValue.ToString() + ",";
                else
                    st += "'" + Values[i].ColumnValue.ToString() + "',";
            st = st.Substring(0, st.Length - 1);
            s = string.Format("INSERT INTO {0}.{1}({2}) VALUES({3})", Schema, table, s, st);
            s = (string)new OracleCommand(s, conn).ExecuteScalar();
            Connect.Commit();
            return s;
        }
        /// <summary>
        /// Обновление таблицы
        /// </summary>
        /// <param name="table">имя таблицы</param>
        /// <param name="Values">Пары (столбец-значение[тип запроса])</param>
        /// <param name="Where">условие фильтра формата "Where ..." - если не указать перепишет все записи в таблице</param>
        public static void Update(string table, string Schema, OracleConnection conn, SP[] Values, string Where)
        {
            string s = "";
            for (int i = 0; i < Values.Length; i++)
            {
                s += Values[i].ColName + "=" + (Values[i].ValueType == TypeColValue.Query ?
                                        Values[i].ColumnValue.ToString() + "," :
                                        "'" + Values[i].ColumnValue.ToString() + "',");
            }
            s = s.Substring(0, s.Length - 1);
            s = string.Format("UPDATE {0}.{1} SET  {2} {3}", Schema, table, s, Where);
            new OracleCommand(s, conn).ExecuteNonQuery();

            new OracleCommand("commit", conn).ExecuteNonQuery();
        }
        /// <summary>
        /// Удаление из таблицы
        /// </summary>
        /// <param name="table">имя таблицы</param>
        /// <param name="Where">фильтр в формаете "Where ..."- если не указать удалит все из таблицы</param>
        public static void Delete(string table, string Schema, OracleConnection conn, string Where)
        {
            string s = string.Format("DELETE FROM {0}.{1} {2}", Schema, table, Where);
            new OracleCommand(s, conn).ExecuteNonQuery();
            new OracleCommand("commit", conn).ExecuteNonQuery();
        }
    }
}
