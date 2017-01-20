using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace LibraryKadr
{
    public static class GetVersionFramework
    {
        public static void GetVersionFromRegistry()
        {
            bool _flag_v4 = false;
            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5  
                // or later, you can use: 
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,  
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v4"))
                    {
                        _flag_v4 = true;
                        /*
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            //Console.WriteLine(versionKeyName + "  " + name);
                            MessageBox.Show(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                //Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                                MessageBox.Show(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                //Console.WriteLine(versionKeyName + "  " + name);
                                MessageBox.Show(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    //Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                    MessageBox.Show("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    //Console.WriteLine("  " + subKeyName + "  " + name);
                                    MessageBox.Show("  " + subKeyName + "  " + name);
                                }

                            }

                        }*/

                    }
                }
                if (_flag_v4 == false)
                {
                    FileStream f = new FileStream(Application.StartupPath + "/Log/NotInstallFramework4.txt", FileMode.Open, FileAccess.Read);
                    StreamReader r = new StreamReader(f);
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    string[] s = new string[] { };
                    string st;
                    while (!r.EndOfStream)
                    {
                        st = r.ReadLine().Trim();
                        if (st.Length > 1 && st.Substring(0, 2) == "//") continue;
                        s = st.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        parameters.Add(s[0].ToUpper(), s[1].ToUpper());
                    }
                    r.Close();
                    if (!parameters.ContainsKey(Environment.MachineName))
                    {
                        f = new FileStream(Application.StartupPath + "/Log/NotInstallFramework4.txt", FileMode.Append, FileAccess.Write);
                        StreamWriter w = new StreamWriter(f, Encoding.GetEncoding(1251));
                        w.WriteLine(Environment.MachineName + " " + SystemInformation.UserName);
                        w.Close();
                    }
                }
            }

        }
    }
}
