using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;

namespace Premium.Classes
{
    [OracleCustomTypeMapping("APSTAFF.TYPE_TABLE_NUMBER")]
    public class NestedTableDecimalFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        // IOracleCustomTypeFactory
        public IOracleCustomType CreateObject()
        {
            return new NestedTableDecimal();
        }

        // IOracleArrayTypeFactory Inteface
        public Array CreateArray(int numElems)
        {
            return new Decimal[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            // CreateStatusArray may return null if null status information 
            // is not required.
            return new OracleUdtStatus[numElems];
        }
    }
    
    public class NestedTableDecimal : IOracleCustomType, INullable
    {
        [OracleArrayMapping()]
        public Decimal[] Array;

        private OracleUdtStatus[] m_statusArray;
        public OracleUdtStatus[] StatusArray
        {
            get
            {
                return this.m_statusArray;
            }
            set
            {
                this.m_statusArray = value;
            }
        }

        private bool m_bIsNull;

        public bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }

        public static NestedTableDecimal Null
        {
            get
            {
                NestedTableDecimal obj = new NestedTableDecimal();
                obj.m_bIsNull = true;
                return obj;
            }
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            object objectStatusArray = null;
            Array = (Decimal[])OracleUdt.GetValue(con, pUdt, 0, out objectStatusArray);
            m_statusArray = (OracleUdtStatus[])objectStatusArray;
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, Array, m_statusArray);
        }

        public override string ToString()
        {
            if (m_bIsNull)
                return "NestedTableDecimal.Null";
            else
            {
                string rtnstr = String.Empty;
                if (m_statusArray[0] == OracleUdtStatus.Null)
                    rtnstr = "NULL";
                else
                    rtnstr = Array.GetValue(0).ToString();
                for (int i = 1; i < m_statusArray.Length; i++)
                {
                    if (m_statusArray[i] == OracleUdtStatus.Null)
                        rtnstr += "," + "NULL";
                    else
                        rtnstr += "," + Array.GetValue(i).ToString();
                }
                return "NestedTableDecimal(" + rtnstr + ")";
            }
        }
    }

    [OracleCustomTypeMapping("APSTAFF.LONG_VARCHAR_COLLECTION_TYPE")]
    public class NestedTableStringFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        // IOracleCustomTypeFactory
        public IOracleCustomType CreateObject()
        {
            return new NestedTableString();
        }

        // IOracleArrayTypeFactory Inteface
        public Array CreateArray(int numElems)
        {
            return new String[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            // CreateStatusArray may return null if null status information 
            // is not required.
            return new OracleUdtStatus[numElems];
        }
    }

    public class NestedTableString : IOracleCustomType, INullable
    {
        [OracleArrayMapping()]
        public String[] Array;

        private OracleUdtStatus[] m_statusArray;
        public OracleUdtStatus[] StatusArray
        {
            get
            {
                return this.m_statusArray;
            }
            set
            {
                this.m_statusArray = value;
            }
        }

        private bool m_bIsNull;

        public bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }

        public static NestedTableString Null
        {
            get
            {
                NestedTableString obj = new NestedTableString();
                obj.m_bIsNull = true;
                return obj;
            }
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            object objectStatusArray = null;
            Array = (String[])OracleUdt.GetValue(con, pUdt, 0, out objectStatusArray);
            m_statusArray = (OracleUdtStatus[])objectStatusArray;
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, Array, m_statusArray);
        }

        public override string ToString()
        {
            if (m_bIsNull)
                return "NestedTableString.Null";
            else
            {
                string rtnstr = String.Empty;
                if (m_statusArray[0] == OracleUdtStatus.Null)
                    rtnstr = "NULL";
                else
                    rtnstr = Array.GetValue(0).ToString();
                for (int i = 1; i < m_statusArray.Length; i++)
                {
                    if (m_statusArray[i] == OracleUdtStatus.Null)
                        rtnstr += "," + "NULL";
                    else
                        rtnstr += "," + Array.GetValue(i).ToString();
                }
                return "NestedTableString(" + rtnstr + ")";
            }
        }
    }
}
