using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;
using LibraryKadr;
using System.IO;

namespace Kadr
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Args = args;
            Elegant.Ui.RibbonLicenser.LicenseKey = "5DB0-D11C-A582-4F1E-6178-7F38-AFFB-8021";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Authorization autho = new Authorization();
            if (autho.ShowDialog() == DialogResult.OK)
            {           
                FormMain f = new FormMain(autho.employees);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(f.UnhandledException);
                AppDomain ap = AppDomain.CurrentDomain;
                ap.UnhandledException += new UnhandledExceptionEventHandler(f.UnhandledException);
                Application.Run(f);
            }
        }
		public static string[] Args;
        public static void ParseArgs(string[] args)
        {
            try
            {
                args = new string[] { "ffff" }.Concat(args).ToArray();
                int k = args.Select((p, i) => new { param = p, index = i }).FirstOrDefault(it => it.param.ToUpper() == "-USER").index;
                if ((k != default(int)) && args.Length > k + 1)
                {
                    Connect.UserId = args[k + 1];
                }
                k = args.Select((p, i) => new { param = p, index = i }).FirstOrDefault(it => it.param.ToUpper() == "-PASS").index;
                if (k != default(int) && args.Length > k + 1)
                {
                    Connect.SetPassword(args[k + 1]);
                }
                for (int i = 0; i < args.Length; ++i)
                {
                    if (args[i][0] == '-' && ListLinkKadr.ListLink.Count(p => p.CommandName.ToUpper() == args[0].Substring(1).ToUpper()) > 0)
                    {
                        LinkKadr lk = ListLinkKadr.ListLink.First(p => p.CommandName.ToUpper() == args[0].Substring(1).ToUpper());
                        decimal k1;
                        if (decimal.TryParse(args[1], out k1))
                        {
                            LinkData ld = new LinkData(string.Empty, k1);
                            if (lk.CanExecute(ld))
                                lk.Execute(ld);
                        }
                    }
                }
            }
            catch { };
        }
    }
}
