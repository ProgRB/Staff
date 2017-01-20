using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace LibraryKadr
{
    public class ParVal
    {
        public static Dictionary<string, string> Vals;

        public class MyComparer: IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.ToUpper() == y.ToUpper();
            }

            public int GetHashCode(string z)
            {
                return z.ToUpper().GetHashCode();
            }
        }

        static ParVal()
        {
            Vals = new Dictionary<string, string>(new MyComparer());
            string str;
            TextReader reader = new StreamReader(Application.StartupPath + @"\Params.ini", Encoding.GetEncoding(1251));
            while ( reader.Peek() != -1)
            {
                str = reader.ReadLine();
                Vals.Add(str.Substring(0, str.IndexOf(' ')), str.Substring(str.IndexOf(' ') + 1));
            }
            reader.Close();
        }
    }
}
