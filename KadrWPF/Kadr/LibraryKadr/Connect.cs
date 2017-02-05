using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Net.Sockets;
using Oracle.DataAccess.Types;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography;
namespace LibraryKadr
{
    public class Connect
    {
        private static OracleConnection _connect;
        private static OracleTransaction _transact;
        private static string _schema;
        private static string _schemaSalary;
        private static string _clientId;
        private static bool _AppBusy = false;
        private static Dictionary<string, string> parameters;
        private static Dictionary<string, DateTime?> Vals = new Dictionary<string, DateTime?>(new LibraryKadr.ParVal.MyComparer());
        public static ConnectState NewConnection(string user_id,string pass, string new_pass = null)
        {
            try
            {
                FileStream f = new FileStream(Application.StartupPath + "/Connect.ini", FileMode.Open, FileAccess.Read);
                StreamReader r = new StreamReader(f);
                parameters = new Dictionary<string, string>();
                string[] s = new string[] { };
                string st;
                while (!r.EndOfStream)
                {
                    st = r.ReadLine().Trim();
                    if (st.Length > 1 && st.Substring(0, 2) == "//") continue;
                    s = st.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    parameters.Add(s[0].ToUpper(),s[1].ToUpper());
                }
                r.Close();
                string con_string = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));POOLING=false;User Id={3};Password=\"{4}\"",
                        parameters["SERVER"],
                        parameters["PORT"],
                        parameters["SID"],
                        user_id.Trim(),
                        pass);
                _connect = new OracleConnection(con_string);
                _schema = parameters["SCHEMENAME"];
                _schemaSalary = parameters["SCHEMENAMESALARY"];
                try
                {
                    _clientId = user_id;
                    if (new_pass==null)
                    _connect.Open();
                    else _connect.OpenWithNewPassword(new_pass);
                    return ConnectState.Open;
                }
                catch (OracleException e)
                {
                    switch (e.Number)
                    {
                        case 1017: MessageBox.Show("Вы ввели неправильное имя пользователя или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return ConnectState.InvalidPassword; break;
                        case 28001: MessageBox.Show("Срок действия временного пароля истек. Вы обязаны установить новый пароль, неравный табельному номеру!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return ConnectState.PassExpired; break;
                        case 28000: MessageBox.Show("Пользователь заблокирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return ConnectState.AccountLock; break;
                        default: MessageBox.Show(string.Format("Error code: {0} \nMessage: {1} \nCode: {2}",e.ErrorCode,e.Message,e.Number), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);break;
                    }
                    return ConnectState.OtherError;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Объект: {0} \nMessage: {1}", e.TargetSite, e.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ConnectState.OtherError;
            }
        }

        private static bool ReConnect()
        {
            ToolTip t = new ToolTip();
            t.BackColor = Color.Red;
            t.ForeColor = Color.Black;
            t.ToolTipIcon = ToolTipIcon.Error;
            t.ToolTipTitle = "ОШИБКА!!!";
            t.UseFading = true;
            t.IsBalloon = true;
            while (_connect.State != System.Data.ConnectionState.Open || !Connect.Ping())
            {
                Thread.Sleep(1000);
                _connect.Close();
                t.RemoveAll();
                t.Show("Потеряно соединения с сервером. Подождите когда соединение будет восстановлено", Application.OpenForms["FormMain"], 50, 50, 1000);
                try
                {
                    _connect.Open();
                }
                catch
                {

                };
            }
            return true;
        }

        public static bool Ping()
        {
            try
            {

                new OracleCommand("select null from dual", _connect).ExecuteNonQuery();
                return true;
            }
            catch
            {
                try
                {
                    TcpClient tc = new TcpClient(parameters["server"], int.Parse(parameters["port"]));
                    tc.Close();
                    if (_connect.State == System.Data.ConnectionState.Open)
                        _connect.Close();
                    _connect.Open();
                    new OracleCommand("select null from dual", _connect).ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Возвращает текущее соединение или пытается его восстановить
        /// </summary>
        public static OracleConnection CurConnect
        {
            get
            {
                //if (!_connect.Ping())
                //    ReConnect();
                return _connect;
            }
        }
        public static string Schema
        {
            get { return _schema; }
        }
        public static string SchemaSalary
        {
            get { return _schemaSalary; }
        }
        public static string UserId
        {
            get
            {
                return _clientId;
            }
            set
            {
                _clientId = value;
            }
        }

        static byte[] pass_data;
        public static string Password
        {
            get
            {
                return ProtectString.Unprotect(pass_data);
            }
        }

        public static void SetPassword(string pass)
        {
            pass_data = ProtectString.Protect(pass);
        }
        
        public static void Rollback()
        {
            new OracleCommand("RollBack", _connect).ExecuteNonQuery();
        }

        public static void Commit()
        {
            new OracleCommand("commit", _connect).ExecuteNonQuery();
        }

        public static OracleTransaction Transact
        {
            get { return _transact; }
            set { _transact = value; }
        }
/*
        public static DateTime? CheckBlockApp
        {
            get 
            {
                if (_connect != null && !AppBusy)
                {
                    OracleCommand cmd = new OracleCommand(string.Format(@"select 	case when sysdate<time_end_block AND user not in ('BMW12714', 'KNV14534') THEN time_end_block else null end as time_end_block    from {0}.time_block_app    where upper(app_name)=upper(:p_app_name)", Connect.Schema), Connect.CurConnect);
                    cmd.Parameters.Add("p_app_name", OracleDbType.Varchar2, "KADR", System.Data.ParameterDirection.Input);
                    try
                    {
                        object o = cmd.ExecuteScalar();
                        return (o == null?(DateTime?)null:(DateTime)o);
                    }
                    catch { return null; }
                }
                return null;
            }
        }*/
        public static bool AppBusy
        {
            get { return _AppBusy; }
            set { _AppBusy = value; }
        }

        public static DateTime? BlockApp
        {
            get 
            {
                Vals.Clear();
                string str;
                int pos = 0;
                DateTime date1, dateTimeNow;
                try
                {
                    DirectoryContext context = new DirectoryContext(DirectoryContextType.DirectoryServer, "ds00.uuap.com");
                    DomainController dc = DomainController.GetDomainController(context);
                    dateTimeNow = TimeZoneInfo.ConvertTimeFromUtc(dc.CurrentTime, TimeZoneInfo.FindSystemTimeZoneById("North Asia Standard Time"));//North Asia East Standard Time
                }
                catch
                {
                    dateTimeNow = DateTime.Now;
                }
                /*Domain domain = Domain.GetCurrentDomain(); North Asia East Standard Time
                dt2 = TimeZoneInfo.ConvertTimeFromUtc(domain.DomainControllers[0].CurrentTime, TimeZoneInfo.FindSystemTimeZoneById("North Asia East Standard Time"));
                */
                
                List<String> rr = new List<string>();// = File.ReadAllLines(Application.StartupPath + @"\TimeBlock.ini", Encoding.GetEncoding(1251));
                TextReader reader = new StreamReader(new FileStream(Application.StartupPath + @"\TimeBlock.ini", FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.GetEncoding(1251));
                while (reader.Peek() != -1)
                {
                    rr.Add(reader.ReadLine());
                }
                reader.Close();
                reader.Dispose();
                for (int i =0; i<rr.ToArray().Length;++i)
                {
                    str = rr[i];
                    pos = str.IndexOf(' ');
                    if (pos != -1)
                    {
                        DateTime.TryParse(str.Substring(pos + 1), out date1);
                        Vals.Add(str.Substring(0, pos), date1);
                    }
                    else
                        Vals.Add(str, new DateTime());
                }                
                //reader.Close();                
                if (Vals["TimeBlockBegin"] != new DateTime() && Vals["TimeBlockEnd"] != new DateTime())
                {
                    if (Vals["TimeBlockBegin"] <= dateTimeNow && Vals["TimeBlockEnd"] >= dateTimeNow)
                        return Vals["TimeBlockEnd"];
                }
                else
                {
                    if (Vals["TimeBlockEnd"] != new DateTime() && Vals["TimeBlockEnd"] >= dateTimeNow)
                        return Vals["TimeBlockEnd"];
                }
                return null;
            }
        }
    }

    public enum ConnectState
    {
        Open = 1,
        InvalidPassword=2,
        PassExpired=3,
        AccountLock=4,
        OtherError = 5
    }
    public class ProtectString
    {
        private static byte[] s_entropy={8,7,15,15};
        public static byte[] Protect(string stringData)
        {
            try
            {
                return ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(stringData), s_entropy, DataProtectionScope.LocalMachine);
            }
            catch {};
            return null;
        }
        public static string Unprotect(byte[] data)
        {
            try
            {
                return System.Text.Encoding.Unicode.GetString(ProtectedData.Unprotect(data, s_entropy, DataProtectionScope.LocalMachine));
            }
            catch{};
            return null;
        }
    }
}
