using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryKadr
{
    public class ConnectionParams
    {
        private string _parameter;
        private string _value;
        public ConnectionParams(string parameter, string value)
        {
            _parameter = parameter;
            _value = value;
        }
        public string Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }
        public string ValueParameter
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
