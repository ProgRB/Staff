using LibraryKadr;
using PERCo_S20_1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PercoXML
{
    public class DictionaryPerco
    {
        public static Employees employees;
        public static Positions positions;
        public static Subdivisions subdivisions;
        //public static Graphs_Work graphs_Work;
        public static Holidays_Class holidays;

        static DictionaryPerco()
        {
            try
            {
                PERCO_1C_S20Class perco = new PERCO_1C_S20Class();
                employees = new Employees(perco, Connect.UserId.ToUpper(), ParVal.Vals["ip_Perco"], ParVal.Vals["port_Perco"]);
                positions = new Positions(perco, Connect.UserId.ToUpper(), ParVal.Vals["ip_Perco"], ParVal.Vals["port_Perco"]);
                subdivisions = new Subdivisions(perco, Connect.UserId.ToUpper(), ParVal.Vals["ip_Perco"], ParVal.Vals["port_Perco"]);
                //graphs_Work = new Graphs_Work(Connect.UserId.ToUpper(), ParVal.Vals["IP_PercoTest"], port_Perco, "ADMIN", "h");
                //graphs_Work = new Graphs_Work(Connect.UserId.ToUpper(), ip_Perco, port_Perco, "ADMIN", "h");
                holidays = new PercoXML.Holidays_Class(Connect.UserId.ToUpper(), ParVal.Vals["ip_Perco"], ParVal.Vals["port_Perco"], "ADMIN", "h");
            }
            catch { }
        }
    }
}
