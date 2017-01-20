using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace LibraryKadr
{
    public class Queries
    {
        /// <summary>
        /// Метод получения тела запроса по его имени 
        /// </summary>
        /// <param name="queryName">Имя запроса(файла) с расширением (пример: employee.sql)</param>
        /// <returns>Тело запроса</returns>
        public static string GetQuery(string queryName)
        {
            TextReader reader = new StreamReader(Application.StartupPath + @"\Queries\"+queryName,Encoding.GetEncoding(1251));
            string rezult = reader.ReadToEnd();
            reader.Close();
            return rezult;
        }

        /// <summary>
        /// Чтение файла с подстановкой схем
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static string GetQueryWithSchema(string queryName)
        {
            return string.Format(GetQuery(queryName), Connect.SchemaApstaff, Connect.SchemaSalary);
        }
    }
}
