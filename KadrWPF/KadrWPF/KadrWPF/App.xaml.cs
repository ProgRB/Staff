﻿using LibraryKadr;
using LibrarySalary.Helpers;
using LibrarySalary.ViewModel;
using Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KadrWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DateTime lastErrorDate = DateTime.Now;
        public static System.Windows.Forms.Timer t;
        protected override void OnStartup(StartupEventArgs e)
        {
            t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(t_Tick);
            t.Interval = 10000;
            t.Start();
            ParseArgs(e.Args);
            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            //Assembly asm = Assembly.Load("Oracle.DataAccess, Version=4.112.3.0");
            Autorization f = new Autorization();
            f.OkClick += new CancelEventHandler(f_OkClick);
            f.ChangePassClick += new CancelEventHandler(f_ChangePassClick);
            if (!(f.ShowDialog() ?? false))
                this.Shutdown();
            else
            {
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                ControlRoles.LoadControlRoles();
                base.OnStartup(e);
            }
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //AppDomain.CurrentDomain.
        }
        
        void t_Tick(object sender, EventArgs e)
        {
            DateTime? dt;
            if ((dt = Connect.BlockApp).HasValue)
            {
                t.Stop();
                AppCloseForm f = new AppCloseForm(dt.Value);
                f.RemainTime = 20;
                f.Show();
            }
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DateTime.Now.Subtract(lastErrorDate).Seconds > 6)
            {
                lastErrorDate = DateTime.Now;
                try
                {
                    System.IO.TextWriter writer = new System.IO.StreamWriter(Connect.CurrentAppPath + "\\LOG\\" +
                        string.Format("{0}.{1:00}.{2:00} {3:00}.{4:00}.{5:00} - {6}", DateTime.Now.Year, DateTime.Now.Month,
                        DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                        Connect.UserId) + ".txt", false, System.Text.Encoding.GetEncoding(1251));
                    writer.WriteLine("Путь приложения: " + "Kadr.exe");
                    writer.WriteLine("Объект: " + sender.ToString());
                    writer.WriteLine("Ошибка: " + e.Exception.ToString());
                    writer.Close();
                }
                catch
                { }
                MessageBox.Show(string.Format("В приложении возникло необработанное исключение. Обратитесь к специалисту\n {0}", e.Exception.Message + e.Exception.StackTrace), "Неизвестная ошибка приложения. Только без паники");
                lastErrorDate = DateTime.Now;
            }
            e.Handled = true;
        }

        void f_OkClick(object sender, CancelEventArgs e)
        {
            Autorization t = sender as Autorization; // получаем форму с которой надо взять логин и пароль
            ResultConnection res = Connect.NewConnection(t.UserName, t.Password); // пытаемся создать подключение и получаем результат.
            e.Cancel = ValidateResultWithMessage(res);
            if (res.State == ConnectState.PassExpired) t.PasswordChangingState = true;
        }

        void f_ChangePassClick(object sender, CancelEventArgs e)
        {
            Autorization t = sender as Autorization; // получаем форму с которой надо взять логин и пароль
            ResultConnection res = Connect.NewConnection(t.UserName, t.Password, t.NewPass); // пытаемся создать подключение и получаем результат. Теперь с этим соединением мы будем менять пароль
            e.Cancel = ValidateResultWithMessage(res);
            if (e.Cancel)
                MessageBox.Show("Поздравляю! Пароль успешно изменен. Не сообщайте пароль третьим лицам. Вся ответственность за действия под вашим пользователем ложится на Вас",
                    "Изменение пароля",
                     MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool ValidateResultWithMessage(ResultConnection res)
        {
            bool fl = false;
            switch (res.State)
            {
                case ConnectState.AccountLock: MessageBox.Show("Пользователь заблокирован. Для разблокировки обратитесь к администратору приложения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Asterisk); break;
                case ConnectState.PassExpired: MessageBox.Show("Дата действия пароля истекла. Вы обязаны установить новый пароль, соответствущий требованиям безопасности", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Asterisk); break;
                case ConnectState.InvalidPassword: MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); break;
                case ConnectState.ImpossibleNewPassword: MessageBox.Show("Вы пытаетесь установить пароль, не соответствующий требованиям безопасности. Придумайте и запомните более СЛОЖНЫЙ пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); break;
                case ConnectState.OtherError: MessageBox.Show(string.Format("Ошибка создания соединения. \n Объект: {0}.\n  Сообщение: {1}", res.Exception.Source, res.Exception.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop); break;
                case ConnectState.Open: fl = true; break;
            }
            return fl;
        }
        
        /// <summary>
        /// При выходе удаляем подписку на события закрытия и сервера
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {

            try
            {
                Connect.CloseConnection();
            }
            catch { };
            base.OnExit(e);
        }

        /// <summary>
        /// процедура обработки входных аргументов
        /// </summary>
        /// <param name="args"></param>
        public static void ParseArgs(string[] args)
        {
            try
            {
                args = new string[] { "ffff" }.Concat(args).ToArray();
                int k = args.Select((p, i) => new { param = p, index = i }).FirstOrDefault(it => it.param.ToUpper() == "-USER").index;
                if ((k != default(int)) && args.Length > k + 1)
                {
                    Connect.UserID = args[k + 1];
                }
                k = args.Select((p, i) => new { param = p, index = i }).FirstOrDefault(it => it.param.ToUpper() == "-PASS").index;
                if (k != default(int) && args.Length > k + 1)
                {
                    Connect.SetPassword(args[k + 1]);
                }
            }
            catch { };
        }
        
        public static ViewTabCollection OpenTabs
        {
            get
            {
                return (ViewTabCollection)App.Current.FindResource("OpenTabs");
            }
        }
    }
}
