using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.IO;
using LibraryKadr;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using PERCo_S20_1C;
using PercoXML;
using System.Threading;
using Staff;

namespace Kadr
{
    public partial class Authorization : Form
    {
        private System.Windows.Forms.Timer t;
        DirectoryInfo dirStaff;
        public Employees employees;
        public static Positions positions;
        public static Subdivisions subdivisions;
        //public static Graphs_Work graphs_Work;
        public static Holidays_Class holidays;
        private ConnectState cur_state = ConnectState.OtherError;
        /// <summary>
        /// Конструктор формы авторизации
        /// </summary>
        /// 
        public Authorization()
        {
            InitializeComponent();
            this.Load += new EventHandler(Authorization_Load);
            this.FormClosed += new FormClosedEventHandler(Authorization_FormClosed);
            this.Size = new Size(332, 150);
            gbPassword.Size = new Size(326, 75);
            lbNewPass.Visible = false;
            tbNewPass.Visible = false;
            btInput.Click += new EventHandler(btInput_Click);
            btExit.Click += new EventHandler(btExit_Click);          
            dirStaff = new DirectoryInfo(Application.UserAppDataPath);
            if (dirStaff.Exists)
            {
                FileStream fileStream = File.Open(dirStaff.FullName + @"\StaffSettings.bin", FileMode.OpenOrCreate);
                BinaryFormatter formater = new BinaryFormatter();
                if (fileStream.Length != 0)
                {
                    tbUserName.Text = (string)formater.Deserialize(fileStream);
                }
                fileStream.Close();
            }
            else
            {
                dirStaff.Create();
            }
            t = new System.Windows.Forms.Timer();
            t.Interval = 10000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
            Program.ParseArgs(Program.Args);
            if (!string.IsNullOrEmpty(Connect.UserId))
            {
                this.tbUserName.Text = Connect.UserId;
                this.tbPassword.Text = Connect.Password;
                btInput_Click(this, EventArgs.Empty);
            }
        }

        void Authorization_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (t != null)
            {
                try
                {
                    t.Stop();
                }
                catch
                { }
            }
        }

        void Authorization_Load(object sender, EventArgs e)
        {
            if (Connect.CurConnect != null && Connect.CurConnect.State == ConnectionState.Open)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        void t_Tick(object sender, EventArgs e)
        {
            DateTime? dt;//Проверяем, заблокировано ли приложение? Тогда вызываем форму завершения программы с таймером в 1 минуту
            if ((dt = Connect.BlockApp).HasValue)
            {
                if (Connect.UserId != null && Connect.UserId.ToUpper() == "BMW12714")
                    return;
                AppCloseForm f = new AppCloseForm(dt.Value);
                f.Owner = this;
                f.RemainTime = 10;
                f.Show();
                t.Stop();
            }
        }

        /// <summary>
        /// Событие нажатия кнопки входа в программу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInput_Click(object sender, EventArgs e)
        {
            //Настраиваем соединение
            try
            {
                cur_state = Connect.NewConnection(tbUserName.Text, tbPassword.Text);
                if (cur_state == ConnectState.PassExpired)
                {
                    btChangePass_Click(this, null);
                    return;
                }
                else if (cur_state!= ConnectState.Open)
                    return;
                DateTime? dt;
                if ((dt = Connect.BlockApp).HasValue && Connect.UserId.ToUpper() != "BMW12714")
                {
                    MessageBox.Show(string.Format("АСУ \"КАДРЫ\" заблокирована Администратором до {0}. \n Просьба до указанного времени не запускать программу!", dt.Value.ToLongTimeString()));
                    Connect.CurConnect.Close();
                    Application.Exit();
                }
                else
                {
                    //Если открывается и все нормально 
                    //то возвращаем диалог резальт
                    this.DialogResult = DialogResult.OK;
                    // 23.07.2013 - отключил регистрацию библиотек Перки, потому что они нужны только отделу кадров
                    // 25.03.2014 - включил регистрацию библиотек Перки чтобы у пользователей обновилась библиотека
                    // 17.04.2014 - отключил регистрацию библиотек Перки
                    /*Process process = Process.Start(new ProcessStartInfo("WScript",
                        Application.StartupPath + @"\CopyPercoDLL.js"));
                    int k = 0;
                    while (process.HasExited == false && k < 3)
                    {
                        Thread.Sleep(1000);
                        k++;
                    }*/
                    Functions.pathToLog = ParVal.Vals["PathToLog"];
                    Functions.nameUser = Connect.UserId.ToUpper();
                    Staff.DataSourceScheme.SchemeName = Connect.Schema;
                    string ip_Perco = ParVal.Vals["IP_Perco"];
                    string port_Perco = ParVal.Vals["Port_Perco"];
                    try
                    {
                        PERCO_1C_S20Class perco = new PERCO_1C_S20Class();
                        employees = new Employees(perco, Connect.UserId.ToUpper(), ip_Perco, port_Perco);
                        positions = new Positions(perco, Connect.UserId.ToUpper(), ip_Perco, port_Perco);
                        subdivisions = new Subdivisions(perco, Connect.UserId.ToUpper(), ip_Perco, port_Perco);
                        //graphs_Work = new Graphs_Work(Connect.UserId.ToUpper(), ParVal.Vals["IP_PercoTest"], port_Perco, "ADMIN", "ghznrb");
                        //graphs_Work = new Graphs_Work(Connect.UserId.ToUpper(), ip_Perco, port_Perco, "ADMIN", "hn-j[hfyf");
                        holidays = new PercoXML.Holidays_Class(Connect.UserId.ToUpper(), ip_Perco, port_Perco, "ADMIN", "hn-j[hfyf");
                    }
                    catch { }
                    FileStream fileStream = File.Open(dirStaff.FullName + @"\StaffSettings.bin", FileMode.OpenOrCreate);
                    BinaryFormatter formater = new BinaryFormatter();
                    formater.Serialize(fileStream, tbUserName.Text);
                    fileStream.Close();
                    t.Stop();
                }
            }
            catch(Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException Ex = ex as OracleException;
                    switch (Ex.Number)
                    {
                        case 1017: MessageBox.Show("Вы ввели неправильное имя пользователя или пароль!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error); tbPassword.Text = ""; break;
                        case 28001: MessageBox.Show("Срок действия временного пароля истек. Вы обязаны сменить пароль. Пароль не должен быть равен табельному номеру!", "АСУ \"Кадры\"",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information); btChangePass_Click(this, null); break;
                        default: MessageBox.Show(string.Format("Error code: {0} \nMessage: {1} \nCode: {2}", Ex.ErrorCode, Ex.Message, Ex.ErrorCode), "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error); break;
                    }
                }
                else
                    MessageBox.Show("Ошибка авторизации:" + ex.Message);
            }
        }
                
        /// <summary>
        /// Событие нажатия кнопки изменения пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePassword(object sender, EventArgs e)
        {            
            try
            {                
                if (tbNewPass.Text.Trim() == "")
                {
                    MessageBox.Show("Пароль не может быть пустым!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbNewPass.Focus();
                    return;
                }
                if (tbUserName.Text.Length >= 8 && tbNewPass.Text == tbUserName.Text.Trim().Substring(3, 5))
                {
                    MessageBox.Show("Запрещено устанавливать пароль совпадающий с табельным номером!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (cur_state== ConnectState.PassExpired)
                    cur_state= Connect.NewConnection(tbUserName.Text, tbPassword.Text, tbNewPass.Text);
                else
                {
                    if ((cur_state = Connect.NewConnection(tbUserName.Text, tbPassword.Text))!=ConnectState.Open)
                        return;
                    string textCommand = string.Format("alter user {0} identified by \"{1}\"", tbUserName.Text, tbNewPass.Text.Trim());
                    OracleCommand command = new OracleCommand(textCommand, Connect.CurConnect);
                    command.BindByName = true;
                    command.ExecuteNonQuery();
                }
                this.Size = new Size(332, 150);
                gbPassword.Size = new Size(326, 75);
                lbNewPass.Visible = false;
                tbNewPass.Visible = false;
                btInput.Click -= ChangePassword;
                btInput.Click += new EventHandler(btInput_Click);
                btExit.Click -= Cancel;
                btExit.Click += new EventHandler(btExit_Click);
                btInput.Text = "Вход";
                btExit.Text = "Выход";
                lbOldPass.Text = "Пароль";
                btChangePass.Enabled = true;
                tbPassword.Text = tbNewPass.Text;
            }
            catch(OracleException ex)
            {
                if (ex.Number==1017)
                {
                    MessageBox.Show("Вы ввели неправильное имя пользователя или пароль!", "АСУ \"Кадры\"", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    tbPassword.Text = "";
                }
                else
                {
                    MessageBox.Show(string.Format("Error code: {0} \nMessage: {1} \nCode: {2}",ex.ErrorCode,ex.Message,ex.ErrorCode), "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        /// <summary>
        /// Событие нажатия кнопки закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Событие нажатия кнопки отмены редактирования пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel(object sender, EventArgs e)
        {
            this.Size = new Size(332, 150);
            gbPassword.Size = new Size(326, 75);
            lbNewPass.Visible = false;
            tbNewPass.Visible = false;
            btInput.Click -= ChangePassword;
            btInput.Click += new EventHandler(btInput_Click);
            btExit.Click -= Cancel;
            btExit.Click += new EventHandler(btExit_Click);
            btInput.Text = "Вход";
            btExit.Text = "Выход";
            lbOldPass.Text = "Пароль";
            btChangePass.Enabled = true;
            cur_state = ConnectState.OtherError;
        }              

        /// <summary>
        /// Событие активации формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Authorization_Activated(object sender, EventArgs e)
        {
            if (tbUserName.Text.Trim() != "")
            {
                tbPassword.Focus();
            }
        }

        /// <summary>
        /// Событие нажатия кнопки смены пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btChangePass_Click(object sender, EventArgs e)
        {
            this.Size = new Size(332, 171);
            gbPassword.Size = new Size(326, 101);
            lbNewPass.Visible = true;
            tbNewPass.Visible = true;
            btInput.Click -= btInput_Click;
            btInput.Click += new EventHandler(ChangePassword);
            btExit.Click -= btExit_Click;
            btExit.Click += new EventHandler(Cancel);
            btInput.Text = "Сменить";
            btExit.Text = "Отмена";
            lbOldPass.Text = "Старый пароль";
            btChangePass.Enabled = false;
        }
    }
}
