using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Staff;
using System.Reflection;
using Elegant.Ui;
using LibraryKadr;
using StaffReports;
using System.IO;
using PERCo_S20_1C;
using PercoXML;
using System.Threading;
using ARM_PROP;
using Tabel;
using WExcel = Microsoft.Office.Interop.Excel;
using System.Data.Odbc;
using MExcel = Microsoft.Office.Interop.Excel;
using Oracle.DataAccess.Types;
using Microsoft.Reporting.WinForms;
using Pass_Office;
using System.Globalization;
using System.Windows.Interop;
using Kadr.Vacation_schedule;
using WpfControlLibrary.Table;
using WpfControlLibrary;
using Helpers;
using WpfControlLibrary.Emps_Access;
namespace Kadr
{
    
    public partial class FormMain : Form
    {
        public EMP_seq emp;
        PER_NUM_BOOK_seq per_num_book;
        BindingSource bs = new BindingSource();
        HandBook formspr;
        ListEmpTemp t_listemp;
        public ListEmp listemp;
        ListEmp listEmpArch;
        FR_Emp listFR_Emp;
        /// <summary>
        /// Форма для работы с нарушителями
        /// </summary>
        public ZurnNaruRe jurnal;
        /// <summary>
        /// Строка запроса для списка нарушителей
        /// </summary>
        public string textBlockNaru;

        string strFilterEmp = " order by per_num";
        /// <summary>
        /// Строка запроса списка работников
        /// </summary>
        public string textQuery, textQueryArch, textQueryFR, textQueryFRDismiss;
        public static bool flagFilter, flagFilterFR_Emp, flagArchive;
        public StringBuilder sort;
        public static Dictionary<int, int> FormTabDepend = new Dictionary<int, int>();
        public static SUBDIV_seq subdiv;
        public static POSITION_seq position;
        public static SUBDIV_seq allSubdiv;
        public static POSITION_seq allPosition;
        public static Employees employees;
        public static SUBDIV_seq subdivFR;
        public static DataSet dsSubdivTable;
        private DataSet _dsTempForReport;
        /// Изменения от 06.09.2010
        /// Добавил новую переменную для признака фильтра по подразделению 
        /// Добавил новую переменную для скрытия вкладок от экономистов (табельщиков) подразделений
        public static string filterOnSubdiv = "";
        /// Изменения от 08.12.2010
        /// Создал новую переменную для пути исполняемого файла
        public string executablePath = "";
        /// Создал новую переменную для хранения хэш-кода для исполняемого файла
        public int hashCode = 0;
        /// Переменные используемые при работе с табелем
        /// <summary>
        /// Идентификатор подразделения, с которым сейчас работаем
        /// </summary>
        public static int subdiv_id = 0;
        /// <summary>
        /// Код подразделения, с которым сейчас работаем
        /// </summary>
        public static string code_subdiv = "";
        /// <summary>
        /// Наименование подразделения, с которым сейчас работаем
        /// </summary>
        public static string subdiv_name = "";
        /// <summary>
        /// Команда для различных запросов при работе с табелем
        /// </summary>
        OracleCommand command;
        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        public TimeExecute timeExecute;
        /// <summary>
        /// Дата для формирования файла для расчета зп
        /// </summary>
        DateTime dateDump = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        /// <summary>
        /// Путь сохранения файлов для аванса и зарплаты
        /// </summary>
        //public static string pathFileTable;
        /// <summary>
        /// Путь сохранения данных по численности
        /// </summary>
        //public static string pathAppendix;
        /// <summary>
        /// Форма списка разрешений
        /// </summary>
        ListPermit listPermit;
        /// <summary>
        /// Список сотрудников в АРМ бухгалтера
        /// </summary>
        public static grid_ras grid_form;
        /// <summary>
        /// Биндингсоурс для АРМ Бухгалтера
        /// </summary>
        BindingSource bsRas = new BindingSource();
        /// <summary>
        /// Биндингсоурс1 для АРМ Бухгалтера
        /// </summary>
        BindingSource bsRas1 = new BindingSource();
        /// <summary>
        /// Таблица для шифра налога
        /// </summary>
        public OracleDataTable _okl;
        /// <summary>
        /// Форма выбора периода для отчета
        /// </summary>
        SelectPeriod selPeriod;
        SelPeriod_Subdiv selPeriod_Subdiv;
        // Переменные для определения какие листы табеля нужно формировать
        bool fl_form_title = false;
        bool fl_form_table = false;
        bool fl_form_appendix = false;
        /// <summary>
        /// Переменная определяет, нужно ли разбивать табель по заказам или нет
        /// </summary>
        bool fl_break_order = true;
        /// <summary>
        /// Конструктор главной формы программы
        /// </summary>
        public FormMain(Employees _employees)
        {
            InitializeComponent();
            this.MdiChildActivate += new EventHandler(FormMain_MdiChildActivate);
            employees = _employees;
            LoadDataBase();            
            //ribbon1.EnableByRules(connection);
            rpKadr.EnableByRules();
            rpTable.EnableByRules(false);
            rpHandbook.EnableByRules();
            rpBlank.EnableByRules();
            rpFR_Emp.EnableByRules();
            rpRas.EnableByRules(false);

            //************************************************************************/
            /****************************шта расписание инит*************************/
            /*************************************************************************
            rpShtatPage.EnableByRules(false);
            InitializeShtatSchedule();*/
            rpShtatPage.Visible = false;
            /**************************************************************************/
            /************************Графики отпусков права****************************/
            /**************************************************************************/
            rpVacationSchedule.EnableByRules(false);
            rgFilterVS.Enabled = true;
            VacEvents.InitEvents(this);
            /**************************************************/
            /*************АРМ БУХГАЛТЕРА***********************/
            RASInitializeButtons();
            btEditEmp.Enabled = false;
            btDeleteEmp.Enabled = false;
            rgTransfer.Visible = false;
            rgFilter.Visible = false;
            rgReport.Visible = true;
            rgFilterNaru.Visible = false;
            btAdd_narush.Enabled = false;
            btInsert_narush.Enabled = false;
            btDel_narush.Enabled = false;
            btRepOtherType.Enabled = false;
            emp = new EMP_seq(Connect.CurConnect);            
            /// Сохраняем значение пути исполняемого файла и его хэш-код
            executablePath = Application.ExecutablePath;
            DateTime date = File.GetLastWriteTime(executablePath);
            hashCode = date.GetHashCode();
            /// Запускаем таймер проверки актуальности программы      
            TimerForUpdate.Start();    
            /// Запускаем таймер для очистки буфера
            //TimerForPerco.Start();
            /// Запускаем таймер для проверки соединения с БД
            TimerForPing.Start();
            SetSeparatorLevel();
            UserNameSettings(Connect.CurConnect, Connect.Schema);
            // Установка русского языка ввода (производится 1 раз при создании главной формы)
            //Application.CurrentInputLanguage =
            //    InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
            OracleGlobalization oraGlob = Connect.CurConnect.GetSessionInfo();
            oraGlob.Territory = "America";
            Connect.CurConnect.SetSessionInfo(oraGlob);     
            // Делаем первой вкладкой Кадры
            ribbon1.CurrentTabPage = rpKadr;

            // 02.02.2015 Отменяем проверку фрэймворка
            //GetVersionFramework.GetVersionFromRegistry();
            //if (Connect.UserId.ToUpper() == "BMW12714")
            //{
            //    btTable_By_Period.Visible = true;
            //    btTable_By_Period.Enabled = true;
            //}
            if (GrantedRoles.GetGrantedRole("DBA"))
                btSql_Trace_Enabled.Visible = true;

            // Кнопка списка переводов для группы приема. 
            // Она находится в пункте Списки сотрудников, поэтому если она не доступна, я ее просто скрываю
            btTransfer_Emp_For_Group_Hire.Visible = btTransfer_Emp_For_Group_Hire.Enabled;
        }

        /// <summary>
        /// Статичный конструктор для определения независимых моментов программы
        /// </summary>
        static FormMain()
        {
            ListLinkKadr.AddLink(new LinkKadr("Карточка отпусков", "VSKard", Vacation_schedule.ViewCard.OpenStaticView, Vacation_schedule.ViewCard.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в табеле", "Table", Table.OpenLink, Table.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в АРМ бухгалтера", "Account", grid_ras.OpenLink, grid_ras.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в кадрах", "Kadr", ListEmp.OpenLink, ListEmp.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Карточка совмещений", "TableCombineCard", Kadr.Shtat.ReplEmpAdd.OpenStaticView, Kadr.Shtat.ReplEmpAdd.CanOpenLink));
            dsSubdivTable = new DataSet();
            dsSubdivTable.Tables.Add("SUBDIV_TABLE");
            OracleDataAdapter odaSubdiv = new OracleDataAdapter(string.Format(
                @"select S.* from {0}.SUBDIV S
                join {0}.TYPE_SUBDIV TS ON (S.TYPE_SUBDIV_ID = TS.TYPE_SUBDIV_ID)
                where nvl(PARENT_ID,0) = 0 and SIGN_SUBDIV_PLANT = 1
                    and subdiv_id in 
                    (select SUBDIV_ID FROM {0}.SUBDIV
                        start with subdiv_id in (
                            select subdiv_id from {0}.access_subdiv 
                            where upper(user_name) = ora_login_user and upper(app_name) = 'TABLE' AND                                 
                                SYSDATE BETWEEN NVL(date_start_access, DATE '1000-01-01') AND NVL(date_end_access, DATE '3000-01-01'))
                        connect by prior subdiv_id = parent_id) 
                order by SUBDIV_NAME", Connect.Schema),
                Connect.CurConnect);
            odaSubdiv.SelectCommand.BindByName = true;
            //odaSubdiv.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            odaSubdiv.Fill(dsSubdivTable.Tables["SUBDIV_TABLE"]);
            dsSubdivTable.Tables["SUBDIV_TABLE"].PrimaryKey = new DataColumn[] { dsSubdivTable.Tables["SUBDIV_TABLE"].Columns["SUBDIV_ID"] };

            dsSubdivTable.Tables.Add("SUBDIV_ALL");
            odaSubdiv = new OracleDataAdapter(string.Format(
                @"select * from {0}.SUBDIV where nvl(PARENT_ID,0) = 0 and SUBDIV_ID != 0 order by CODE_SUBDIV, SUBDIV_NAME", Connect.Schema),
                Connect.CurConnect);
            odaSubdiv.Fill(dsSubdivTable.Tables["SUBDIV_ALL"]);
            dsSubdivTable.Tables["SUBDIV_ALL"].PrimaryKey = new DataColumn[] { dsSubdivTable.Tables["SUBDIV_ALL"].Columns["SUBDIV_ID"] };
            dsSubdivTable.Tables["SUBDIV_ALL"].Columns.Add("DISP_SUBDIV").Expression = "CODE_SUBDIV+' ('+SUBDIV_NAME+')'+IIF(SUB_ACTUAL_SIGN=0,' <не актуально>','')";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //TemporaryFiles.DeleteAllPreviouslyUsed();
                if (Transfer_Project_Viewer.OdProject_Transfer != null && Connect.CurConnect != null &&
                    Connect.CurConnect.State == ConnectionState.Open)
                    Transfer_Project_Viewer.OdProject_Transfer.RemoveRegistration(Connect.CurConnect);
            }
            catch { }
            try
            {
                Connect.CurConnect.Close();
            }
            catch { }
        }

        /// <summary>
        /// Создание и загрузка таблиц подразделение и должности
        /// </summary>
        void LoadDataBase()
        {
            try
            {
                subdiv = new SUBDIV_seq(Connect.CurConnect);
                subdiv.Fill(string.Format("where {0} = 1 and {2} != 7 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
                    SUBDIV_seq.ColumnsName.SUBDIV_NAME, SUBDIV_seq.ColumnsName.WORK_TYPE_ID));
                position = new POSITION_seq(Connect.CurConnect);
                position.Fill(string.Format("where {0} = 1 order by {1}", POSITION_seq.ColumnsName.POS_ACTUAL_SIGN.ToString(),
                    POSITION_seq.ColumnsName.POS_NAME.ToString()));
                allSubdiv = new SUBDIV_seq(Connect.CurConnect);
                allSubdiv.Fill(string.Format("where {0} != 7 order by {1}", SUBDIV_seq.ColumnsName.WORK_TYPE_ID, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                allPosition = new POSITION_seq(Connect.CurConnect);
                allPosition.Fill(string.Format("order by {0}", POSITION_seq.ColumnsName.POS_NAME.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data: " + ex.Data + "Message: " + ex.Message + "TargetSite: " + ex.TargetSite);
            }
        }

        /// <summary>
        /// Осуществляет принудительную настройку вида определенных разделитилей на ленте,
        /// потому что они сворачиваются по непонятным причинам
        /// </summary>
        void SetSeparatorLevel()
        {
            srTable.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srPopul.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srTable_By_Period.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srSaveTable.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srCloseTable.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srTotalHours.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srRepOnAppendix.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srRepOnAppendix2.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
            srRepHoursByOrders.InformativenessFixedLevel = Elegant.Ui.PopupMenuSeparatorInformativenessLevel.SeparatorWithText.ToString();
        }

        /// <summary>
        /// Метод определяет имя текущего пользователя программы
        /// </summary>
        /// <param name="_con">Соединение</param>
        /// <param name="_scheme">Имя схемы</param>
        void UserNameSettings(OracleConnection _con, string _scheme)
        {
            string userName = "";
            if (Connect.UserId.Length >= 5)
            {
                OracleCommand comEmp = new OracleCommand("", _con);
                comEmp.BindByName = true;
                comEmp.CommandText = string.Format(
                    "select upper(emp_last_name)||' '||upper(emp_first_name)||' '||upper(emp_middle_name) as UserName " +
                    "from {0}.EMP where per_num = :p_per_num", _scheme);
                comEmp.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value =
                    Connect.UserId.Substring(Connect.UserId.Length - 5, 5);
                OracleDataReader reader = comEmp.ExecuteReader();
                if (reader.Read())
                {
                    userName = reader[0].ToString();
                }
                else
                {
                    comEmp = new OracleCommand("", _con);
                    comEmp.BindByName = true;
                    comEmp.CommandText = string.Format(
                        "select upper(emp_last_name)||' '||upper(emp_first_name)||' '||upper(emp_middle_name) as UserName " +
                        "from {0}.EMP where per_num = " +
                        "(select per_num from {0}.USERS_KADR where upper(login) = :p_userID)", _scheme);
                    comEmp.Parameters.Add("p_userID", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    reader = comEmp.ExecuteReader();
                    if (reader.Read())
                    {
                        userName = reader[0].ToString();
                    }
                }
            }
            RecentDocumentsControl recent = new RecentDocumentsControl();
            recent.Caption = "Информация о пользователе программы";
            if (userName != "")
                recent.Items.Add(userName);
            recent.Items.Add(Connect.UserId.ToUpper());
            applicationMenu1.RightPaneControl = recent;
        }

#region КАДРЫ!

        /// <summary>
        /// Событие нажатия кнопки просмотра приемной базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTempDb_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "t_listemp").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name == "t_listemp").First().Activate();
                return;
            }
            flagArchive = false;
            btDate_DismissSorter.Visible = false;
            ddTransfer.Enabled = false;
            btDeleteEmp.Enabled = true;
            btEditEmp.Enabled = true;
            emp = new EMP_seq(Connect.CurConnect);
            textQuery = string.Format(Queries.GetQuery("SelectListEmpHire.sql"),
                Connect.Schema, " order by per_num");
            OracleDataTable dtEmp = new OracleDataTable(textQuery, Connect.CurConnect);
            t_listemp = new ListEmpTemp(dtEmp, this);
            t_listemp.Text = "Приемная база данных работников";
            t_listemp.Name = "t_listemp";
            t_listemp.MdiParent = this;
            CreateButtonApp(t_listemp, sender);
            t_listemp.Show();         

        }

        /// <summary>
        /// Событие нажатия кнопки просмотра основной базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btMainDb_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "listemp").Count() != 0)
            {
                //MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
                this.MdiChildren.Where(i => i.Name == "listemp").First().Activate();
                textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                    Connect.Schema, " order by per_num");
                return;
            }  
            flagArchive = false;
            btDate_DismissSorter.Visible = false;
            //ddTransfer.Enabled = true;
            /// Создаем и работаем с формой первоначального поиска.
            LoadEmp loadEmp = new LoadEmp();
            DialogResult rez = loadEmp.ShowDialog();
            if (rez == DialogResult.Cancel)
            {
                return;            
            }     
            /// Строка сортировки берется из формы первоначального поиска.
            sort = loadEmp.sort;
            rgFilter.Visible = true;
            rgTransfer.Visible = true;
            btEditEmp.Enabled = true;
            emp = new EMP_seq(Connect.CurConnect);
            textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                Connect.Schema, sort.ToString());
            /// Создаем таблицу работников завода.
            OracleDataTable dtEmp = new OracleDataTable(textQuery, Connect.CurConnect);
            /// Создаем форму списка работников завода.
            listemp = new ListEmp(dtEmp, this, "listemp");
            /// Перебираем все строки пока не будет найден нужный табельный номер.
            for (int i = 0; i < listemp.dgEmp.Rows.Count; i++)
            {
                if (listemp.dgEmp["per_num", i].Value.ToString() == loadEmp.per_num)
                {
                    listemp.bsEmp.Position = listemp.dgEmp["per_num", i].RowIndex;
                    break;
                }
            }
            listemp.MdiParent = this;
            listemp.Text = "Список работников завода";
            CreateButtonApp(listemp, sender);
            listemp.Show();
            /* Изменения от 20,07,2013 - если у пользователя роль STAFF_VIEW_ONLYLISTEMP - 
             * то отключаем кнопку отчета произвольной формы*/
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
            {
                if (Connect.UserId.ToUpper() != "BMW12714")
                    btRepOtherType.Enabled = false;
                else
                    btRepOtherType.Enabled = true;
            }
            else
            {
                btRepOtherType.Enabled = true;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки просмотра архивной базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btArchives_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "listemparch").Count() != 0)
            {
                //MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
                this.MdiChildren.Where(i => i.Name == "listemparch").First().Activate();
                return;
            }  
            flagArchive = true;
            btDate_DismissSorter.Visible = true;            
            LoadEmp loadEmp = new LoadEmp();
            DialogResult rez = loadEmp.ShowDialog();
            if (rez == DialogResult.Cancel)
            {
                return;
            }            
            sort = loadEmp.sort;
            rgFilter.Visible = true;
            rgTransfer.Visible = true;
            btEditEmp.Enabled = true;
            emp = new EMP_seq(Connect.CurConnect);
            textQueryArch = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                Connect.Schema, sort.ToString());
            OracleDataTable dtEmp = new OracleDataTable(textQueryArch, Connect.CurConnect);
            listEmpArch = new ListEmp(dtEmp, this, "listemparch");
            for (int i = 0; i < listEmpArch.dgEmp.Rows.Count; i++)
            {
                if (listEmpArch.dgEmp["per_num", i].Value.ToString() == loadEmp.per_num)
                {
                    listEmpArch.bsEmp.Position = listEmpArch.dgEmp["per_num", i].RowIndex;
                }
            }
            listEmpArch.MdiParent = this;
            listEmpArch.Text = "Список работников завода из архивной базы данных";
            CreateButtonApp(listEmpArch, sender);
            listEmpArch.Show();
            /* Изменения от 20,07,2013 - если у пользователя роль STAFF_VIEW_ONLYLISTEMP - 
             * то отключаем кнопку отчета произвольной формы*/
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
            {
                btRepOtherType.Enabled = false;
            }
            else
            {
                btRepOtherType.Enabled = true;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки добавления нового сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddEmp_Click(object sender, EventArgs e)
        {
            /*if (MessageBox.Show("Человек является бывшим работником завода?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {*/
                AddFindEmp addFindEmp = new AddFindEmp(this);
                addFindEmp.WindowState = FormWindowState.Normal;
                addFindEmp.ShowInTaskbar = false;
                addFindEmp.ShowDialog();
            /*}
            else
            {
                PER_NUM_BOOK_seq per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                per_num_book.Fill(string.Format("where PER_NUM = (select min(TO_NUMBER(PER_NUM)) from {0}.per_num_book where FREE_SIGN = 1)",
                    Connect.Schema));
                ((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = false;
                per_num_book.Save();
                Connect.Commit();
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.AddNew();
                ((EMP_obj)(((CurrencyManager)BindingContext[record_emp]).Current)).PER_NUM = per_num_book[0].PER_NUM.ToString();
                PersonalCard personalCard = new PersonalCard(per_num_book[0].PER_NUM.ToString(), 0, record_emp, true, true, 
                    true, 0, listemp);
                personalCard.Text = "Личная карточка работника приемной базы данных";
                personalCard.ShowInTaskbar = false;
                personalCard.ShowDialog();
            }*/
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btEditEmp_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() == "t_listemp".ToUpper())
            {
                if (t_listemp.dgViewEmpTemp.RowCount != 0)
                {
                    string per_num = t_listemp.dgViewEmpTemp.Rows[t_listemp.bsEmp.Position].Cells["per_num"].Value.ToString();
                    int transfer_id = Convert.ToInt32(t_listemp.dgViewEmpTemp.Rows[t_listemp.bsEmp.Position].Cells["transfer_id"].Value);
                    int sign_comb = t_listemp.dgViewEmpTemp.Rows[t_listemp.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                    EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                    record_emp.Fill(string.Format("where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                    PersonalCard personalcard = new PersonalCard(per_num, transfer_id, record_emp, false, false, false, 
                        sign_comb, listemp);
                    personalcard.Text = "Личная карточка работника приемной базы данных";
                    personalcard.ShowInTaskbar = false;
                    personalcard.ShowDialog();
                }
            }
            else
            {
                if (this.ActiveMdiChild.Name.ToUpper() == "listemp".ToUpper())
                {
                    if (listemp.dgEmp.RowCount != 0)
                    {
                        string per_num = listemp.dgEmp.Rows[listemp.bsEmp.Position].Cells["per_num"].Value.ToString();
                        int transfer_id = Convert.ToInt32(listemp.dgEmp.Rows[listemp.bsEmp.Position].Cells["transfer_id"].Value);
                        int sign_comb = listemp.dgEmp.Rows[listemp.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                        EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                        record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                        PersonalCard personalcard = new PersonalCard(per_num, transfer_id, record_emp, true, false, false, 
                            sign_comb, listemp);                        
                        personalcard.Text = "Личная карточка работника";
                        personalcard.ShowInTaskbar = false;
                        personalcard.ShowDialog();
                    }
                }
                else
                    if (this.ActiveMdiChild.Name.ToUpper() == "listemparch".ToUpper())
                    {
                        if (listEmpArch.dgEmp.RowCount != 0)
                        {
                            string per_num = listEmpArch.dgEmp.Rows[listEmpArch.bsEmp.Position].Cells["per_num"].Value.ToString();
                            int transfer_id = Convert.ToInt32(listEmpArch.dgEmp.Rows[listEmpArch.bsEmp.Position].Cells["transfer_id"].Value);
                            int sign_comb = listEmpArch.dgEmp.Rows[listEmpArch.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                            PersonalCard personalcard = new PersonalCard(per_num, transfer_id, record_emp, true, false, false, sign_comb, listEmpArch);
                            personalcard.Text = "Личная карточка работника";
                            personalcard.ShowInTaskbar = false;
                            personalcard.ShowDialog();
                        }
                    }
                    else
                    {
                        if (((ListEmp)this.ActiveMdiChild).dgEmp.RowCount != 0)
                        {
                            string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                            int transfer_id = Convert.ToInt32(((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["transfer_id"].Value);
                            int sign_comb = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                            PersonalCard personalcard = new PersonalCard(per_num, transfer_id, record_emp, true, false, false, sign_comb, ((ListEmp)this.ActiveMdiChild));
                            personalcard.Text = "Личная карточка работника";
                            personalcard.ShowInTaskbar = false;
                            personalcard.ShowDialog();
                        }
                    }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки удаления сотрудников из приемной базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteEmp_Click(object sender, EventArgs e)
        {
            if (FormMain.flagArchive)
            {
                MessageBox.Show("Невозможно удалить данные из архива!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.MdiChildren.Length != 0)
            {
                if (this.ActiveMdiChild.Name == "t_listemp")
                {
                    string perNum = t_listemp.dgViewEmpTemp.Rows[t_listemp.bsEmp.Position].Cells["per_num"].Value.ToString();
                    if (MessageBox.Show("Вы действительно хотите удалить данные работника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (t_listemp.dgViewEmpTemp.RowCount != 0)
                        {
                            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                            int signcomb = t_listemp.dgViewEmpTemp.Rows[t_listemp.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                            transfer.Fill(string.Format(" where {4} = '{3}' and {6} = {8} and ({2} = 1 or " +
                                "(not exists(select null from {0}.{1} tr1 where '{3}' = tr1.{4} and {2} = 1) " + /*У человека нет текущей работы*/
                                "and ({7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 0 ) " + /*Последнюю основную должность*/
                                "or {7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 1 ))))",
                                Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, perNum,
                                TRANSFER_seq.ColumnsName.PER_NUM, TRANSFER_seq.ColumnsName.DATE_TRANSFER,
                                TRANSFER_seq.ColumnsName.SIGN_COMB, "tab1", signcomb));
                            transfer.RemoveAt(0);
                            transfer.Save();
                            transfer.Fill(string.Format("where {0} = '{1}'", TRANSFER_seq.ColumnsName.PER_NUM, perNum));
                            if (transfer.Count == 0)
                            {
                                per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
                                per_num_book.Fill(string.Format("where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, perNum));
                                ((PER_NUM_BOOK_obj)(per_num_book.Where(i => i.PER_NUM == perNum).FirstOrDefault())).FREE_SIGN = true;
                                per_num_book.Save();
                                emp.Fill(string.Format("where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, perNum));
                                emp.RemoveAt(0);
                                emp.Save();
                            }
                            Connect.Commit();
                            t_listemp.EnterDataGridView();
                        }
                    }
                }
                else
                {
                    //string perNum = listemp.dgViewEmp.Rows[listemp.bsEmp.Position].Cells["per_num"].Value.ToString();
                    //if (MessageBox.Show("Вы действительно хотите удалить данные работника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //    if (listemp.dgViewEmp.RowCount != 0)
                    //    {
                    //        TRANSFER_seq transfer = new TRANSFER_seq(connection);
                    //        transfer.Fill(string.Format("where {0} = '{1}'", TRANSFER_seq.ColumnsName.PER_NUM, perNum));
                    //        if (transfer.Count <= 1)
                    //        {
                    //            transfer.Clear();
                    //            int signcomb = listemp.dgViewEmp.Rows[listemp.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                    //            transfer.Fill(string.Format(" where {4} = '{3}' and {6} = {8} and ({2} = 1 or " +
                    //                "(not exists(select null from {0}.{1} tr1 where '{3}' = tr1.{4} and {2} = 1) " + /*У человека нет текущей работы*/
                    //                "and ({7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 0 ) " + /*Последнюю основную должность*/
                    //                "or {7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 1 ))))",
                    //                Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, perNum,
                    //                TRANSFER_seq.ColumnsName.PER_NUM, TRANSFER_seq.ColumnsName.DATE_TRANSFER,
                    //                TRANSFER_seq.ColumnsName.SIGN_COMB, "tab1", signcomb));
                    //            transfer.RemoveAt(0);
                    //            transfer.Save();
                    //            transfer.Fill(string.Format("where {0} = '{1}'", TRANSFER_seq.ColumnsName.PER_NUM, perNum));
                    //            if (transfer.Count == 0)
                    //            {
                    //                per_num_book = new PER_NUM_BOOK_seq(connection);
                    //                per_num_book.Fill(string.Format("where {0} = '{1}'", PER_NUM_BOOK_seq.ColumnsName.PER_NUM, perNum));
                    //                ((PER_NUM_BOOK_obj)(per_num_book.Where(i => i.PER_NUM == perNum).FirstOrDefault())).FREE_SIGN = true;
                    //                per_num_book.Save();
                    //                emp.Fill(string.Format("where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, perNum));
                    //                emp.RemoveAt(0);
                    //                emp.Save();
                    //            }
                    //            connection.Commit();
                    //        }
                    //        else
                    //        {
                    //            transfer.Clear();
                    //            int signcomb = listemp.dgViewEmp.Rows[listemp.bsEmp.Position].Cells["sign_comb"].Value.ToString() != "" ? 1 : 0;
                    //            transfer.Fill(string.Format(" where {4} = '{3}' and {6} = {8} and ({2} = 1 or " +
                    //                "(not exists(select null from {0}.{1} tr1 where '{3}' = tr1.{4} and {2} = 1) " + /*У человека нет текущей работы*/
                    //                "and ({7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 0 ) " + /*Последнюю основную должность*/
                    //                "or {7}.{5} = (select max({5}) from {0}.{1} tr2 where {7}.{4} = tr2.{4} and tr2.{6} = 1 ))))",
                    //                Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, perNum,
                    //                TRANSFER_seq.ColumnsName.PER_NUM, TRANSFER_seq.ColumnsName.DATE_TRANSFER,
                    //                TRANSFER_seq.ColumnsName.SIGN_COMB, "tab1", signcomb));
                    //            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 1)
                    //            {
                    //                transfer.RemoveAt(0);
                    //                transfer.Save();
                    //            }
                    //        }
                    //        //listemp.EnterDataGridView();
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки фильтра данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btFilter_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() == "LISTEMP" ||
                this.ActiveMdiChild.Name.ToUpper() == "LISTEMPARCH" ||
                this.ActiveMdiChild.Name.ToUpper() == "LISTEMP_TERM")
            {
                Filter_Emp filter_emp = new Filter_Emp();
                DialogResult rezFilter = filter_emp.ShowDialog(this);
                if (rezFilter == DialogResult.OK || rezFilter == DialogResult.Abort)
                {
                    string strCount = "", strTemp;
                    if (this.ActiveMdiChild.Name.ToUpper() == "LISTEMP_TERM")
                        if (rezFilter == DialogResult.OK)
                            strTemp = " where " + filter_emp.str_filter.ToString() + " ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM";
                        else
                            strTemp = "ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM";
                    else
                        if (rezFilter == DialogResult.OK)
                            strTemp = " where " + filter_emp.str_filter.ToString() + " order by CODE_SUBDIV, PER_NUM";
                        else
                            strTemp = " order by CODE_SUBDIV, PER_NUM";

                    strFilterEmp = strTemp;
                    switch (this.ActiveMdiChild.Name.ToUpper())
                    {
                        case "LISTEMPARCH":
                            textQuery = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                                Connect.Schema, strTemp);
                            strCount = string.Format(Queries.GetQuery("SelectListEmpArchiveCount.sql"),
                                Connect.Schema, rezFilter == DialogResult.OK ? " where " + filter_emp.str_filter.ToString() : "");
                            break;
                        case "LISTEMP":
                            textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                                Connect.Schema, strTemp);
                            strCount = string.Format(Queries.GetQuery("SelectListEmpCount.sql"),
                                Connect.Schema, rezFilter == DialogResult.OK ? " where " + filter_emp.str_filter.ToString() : "");
                            break;
                        case "LISTEMP_TERM":
                            textQuery = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),
                                Connect.Schema, strTemp);
                            break;
                        default: break;
                    }
                    OracleDataTable newDataEmp = new OracleDataTable(textQuery, Connect.CurConnect);
                    newDataEmp.Fill();
                    int count, countComb, countWomen; // Основная работа, Совместителей, Женщин (Основная работа)
                    count = countComb = countWomen = 0;
                    foreach (DataRow row in newDataEmp.Rows)
                    {
                        if (row["SIGN_COMB"].ToString() == "")
                            count++;
                        else
                            countComb++;
                        if (row["EMP_SEX"].ToString() == "Ж" && row["SIGN_COMB"].ToString() == "")
                            countWomen++;
                    }
                    MessageBox.Show("Количество работников по основной должности - "
                        + count.ToString() + ",\nколичество совместителей - " + countComb.ToString()
                        + ",\nколичество женщин (основная работа) - " + countWomen.ToString(),
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OracleDataTable dtEmpCount;
                    switch (this.ActiveMdiChild.Name.ToUpper())
                    {
                        case "LISTEMPARCH":
                            dtEmpCount = new OracleDataTable(strCount, Connect.CurConnect);
                            dtEmpCount.Fill();
                            listEmpArch.ssCountEmp.Visible = true;
                            listEmpArch.tsslMainEmp.Text = dtEmpCount.Rows[0]["COUNT_EMP"].ToString();
                            listEmpArch.tsslWoman.Text = dtEmpCount.Rows[0]["COUNT_WOMAN"].ToString();
                            listEmpArch.tsslComb.Text = dtEmpCount.Rows[0]["COUNT_COMB"].ToString();
                            listEmpArch.tsslCombIn.Text = dtEmpCount.Rows[0]["COUNT_IN_COMB"].ToString();
                            listEmpArch.tsslCombOut.Text = dtEmpCount.Rows[0]["COUNT_OUT_COMB"].ToString();

                            listEmpArch.dgEmp.DataSource = null;
                            listEmpArch.dtEmp = newDataEmp;
                            listEmpArch.bsEmp.PositionChanged -= listEmpArch.PositionChange;
                            listEmpArch.bsEmp.DataSource = listEmpArch.dtEmp;
                            listEmpArch.dgEmp.DataSource = listEmpArch.dtEmp;
                            listEmpArch.RefreshGridEmp();
                            listEmpArch.bsEmp.PositionChanged += new EventHandler(listEmpArch.PositionChange);
                            listEmpArch.bsEmp.Position = 0;
                            listEmpArch.PositionChange(listEmpArch.bsEmp, null);
                            break;
                        case "LISTEMP":
                            dtEmpCount = new OracleDataTable(strCount, Connect.CurConnect);
                            dtEmpCount.Fill();
                            listemp.ssCountEmp.Visible = true;
                            listemp.tsslMainEmp.Text = dtEmpCount.Rows[0]["COUNT_EMP"].ToString();
                            listemp.tsslWoman.Text = dtEmpCount.Rows[0]["COUNT_WOMAN"].ToString();
                            listemp.tsslComb.Text = dtEmpCount.Rows[0]["COUNT_COMB"].ToString();
                            listemp.tsslCombIn.Text = dtEmpCount.Rows[0]["COUNT_IN_COMB"].ToString();
                            listemp.tsslCombOut.Text = dtEmpCount.Rows[0]["COUNT_OUT_COMB"].ToString();

                            listemp.dgEmp.DataSource = null;
                            listemp.dtEmp = newDataEmp;
                            listemp.bsEmp.PositionChanged -= listemp.PositionChange;
                            listemp.bsEmp.DataSource = listemp.dtEmp;
                            listemp.dgEmp.DataSource = listemp.dtEmp;
                            listemp.RefreshGridEmp();
                            listemp.bsEmp.PositionChanged += new EventHandler(listemp.PositionChange);
                            listemp.bsEmp.Position = 0;
                            listemp.PositionChange(listemp.bsEmp, null);
                            break;
                        case "LISTEMP_TERM":
                            ((ListEmp)this.ActiveMdiChild).dgEmp.DataSource = null;
                            ((ListEmp)this.ActiveMdiChild).dtEmp = newDataEmp;
                            ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged -= ((ListEmp)this.ActiveMdiChild).PositionChange;
                            ((ListEmp)this.ActiveMdiChild).bsEmp.DataSource = ((ListEmp)this.ActiveMdiChild).dtEmp;
                            ((ListEmp)this.ActiveMdiChild).dgEmp.DataSource = ((ListEmp)this.ActiveMdiChild).dtEmp;
                            ((ListEmp)this.ActiveMdiChild).RefreshGridEmp();
                            ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged += new EventHandler(((ListEmp)this.ActiveMdiChild).PositionChange);
                            ((ListEmp)this.ActiveMdiChild).bsEmp.Position = 0;
                            break;
                        default: break;
                    }
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки поиска данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btFind_Click(object sender, EventArgs e)
        {
            Find_Emp find_emp;
            switch (this.ActiveMdiChild.Name.ToUpper())
            {
                case "LISTEMPARCH":
                    //find_emp = new Find_Emp(textQueryArch);
                    find_emp = new Find_Emp(listEmpArch.dtEmp.SelectCommand.CommandText);
                    find_emp.Tag = this;
                    find_emp.ShowInTaskbar = false;
                    if (find_emp.ShowDialog(this) == DialogResult.OK)
                    {
                        if (find_emp.OracleDataTable.Rows.Count != 0)
                        {
                            int pos = listEmpArch.dtEmp.SelectCommand.CommandText.IndexOf("order by");
                            string strSelect = listEmpArch.dtEmp.SelectCommand.CommandText.Substring(0, pos) + find_emp.sort;
                            /// Перезаполняю таблицу, чтобы поставить нужную сортировку
                            listEmpArch.bsEmp.PositionChanged -= listEmpArch.PositionChange;
                            listEmpArch.dtEmp.Clear();
                            listEmpArch.dtEmp.SelectCommand.CommandText = strSelect;
                            listEmpArch.dtEmp.Fill();
                            listEmpArch.bsEmp.PositionChanged += new EventHandler(listEmpArch.PositionChange);
                            /// Получаю первый табельный номер, который удовлетворяет условию
                            string tnom = find_emp.OracleDataTable.Rows[0][1].ToString();
                            //int pos = listemp.dtEmp.AsEnumerable().Select((s, i) => new { row = s.Field<string>("per_num"), pos = i }).Where(s => s.row == find_emp.OracleDataTable.Rows[0][0].ToString()).Select(s => s.pos).FirstOrDefault();
                            for (int i = 0; i < listEmpArch.dgEmp.Rows.Count; i++)
                            {
                                if (listEmpArch.dgEmp["per_num", i].Value.ToString() == tnom)
                                {
                                    listEmpArch.bsEmp.Position = listEmpArch.dgEmp["per_num", i].RowIndex;
                                    break;
                                }
                            }
                            listEmpArch.dgEmp.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Данные найдены в базе данных работников.\nНо заданные критерии фильтрации не позволяют\nпоместить курсор на нужную позицию.\nИзмените критерии фильтра и попробуйте еще раз.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    break;
                case "LISTEMP":
                    //textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"), Connect.Schema, strFilterEmp);
                    textQuery = listemp.dtEmp.SelectCommand.CommandText;
                    find_emp = new Find_Emp(textQuery);
                    find_emp.Tag = this;
                    find_emp.ShowInTaskbar = false;
                    if (find_emp.ShowDialog(this) == DialogResult.OK)
                    {
                        if (find_emp.OracleDataTable.Rows.Count != 0)
                        {
                            int pos = listemp.dtEmp.SelectCommand.CommandText.IndexOf("order by");
                            string strSelect = listemp.dtEmp.SelectCommand.CommandText.Substring(0, pos) + find_emp.sort;
                            /// Перезаполняю таблицу, чтобы поставить нужную сортировку
                            listemp.bsEmp.PositionChanged -= listemp.PositionChange;
                            listemp.dtEmp.Clear();
                            listemp.dtEmp.SelectCommand.CommandText = strSelect;
                            listemp.dtEmp.Fill();
                            listemp.bsEmp.PositionChanged += new EventHandler(listemp.PositionChange);
                            /// Получаю первый табельный номер, который удовлетворяет условию
                            string tnom = find_emp.OracleDataTable.Rows[0][1].ToString();
                            //int pos = listemp.dtEmp.AsEnumerable().Select((s, i) => new { row = s.Field<string>("per_num"), pos = i }).Where(s => s.row == find_emp.OracleDataTable.Rows[0][0].ToString()).Select(s => s.pos).FirstOrDefault();
                            for (int i = 0; i < listemp.dgEmp.Rows.Count; i++)
                            {
                                if (listemp.dgEmp["per_num", i].Value.ToString() == tnom)
                                {
                                    listemp.bsEmp.Position = listemp.dgEmp["per_num", i].RowIndex;
                                    break;
                                }
                            }
                            listemp.dgEmp.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Данные найдены в базе данных работников.\nНо заданные критерии фильтрации не позволяют\nпоместить курсор на нужную позицию.\nИзмените критерии фильтра и попробуйте еще раз.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    break;
                case "LISTEMP_TERM":
                    //textQuery = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),  Connect.Schema, strFilterEmp);
                    textQuery = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText;
                    find_emp = new Find_Emp(textQuery);
                    find_emp.Tag = this;
                    find_emp.ShowInTaskbar = false;
                    if (find_emp.ShowDialog(this) == DialogResult.OK)
                    {
                        if (find_emp.OracleDataTable.Rows.Count != 0)
                        {
                            int pos = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.ToUpper().IndexOf("ORDER BY");
                            string strSelect = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.Substring(0, pos) + find_emp.sort;
                            /// Перезаполняю таблицу, чтобы поставить нужную сортировку
                            ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged -= ((ListEmp)this.ActiveMdiChild).PositionChange;
                            ((ListEmp)this.ActiveMdiChild).dtEmp.Clear();
                            ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText = strSelect;
                            ((ListEmp)this.ActiveMdiChild).dtEmp.Fill();
                            ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged += new EventHandler(((ListEmp)this.ActiveMdiChild).PositionChange);
                            /// Получаю первый табельный номер, который удовлетворяет условию
                            string tnom = find_emp.OracleDataTable.Rows[0][1].ToString();
                            //int pos = listemp.dtEmp.AsEnumerable().Select((s, i) => new { row = s.Field<string>("per_num"), pos = i }).Where(s => s.row == find_emp.OracleDataTable.Rows[0][0].ToString()).Select(s => s.pos).FirstOrDefault();
                            for (int i = 0; i < ((ListEmp)this.ActiveMdiChild).dgEmp.Rows.Count; i++)
                            {
                                if (((ListEmp)this.ActiveMdiChild).dgEmp["per_num", i].Value.ToString() == tnom)
                                {
                                    ((ListEmp)this.ActiveMdiChild).bsEmp.Position = ((ListEmp)this.ActiveMdiChild).dgEmp["per_num", i].RowIndex;
                                    break;
                                }
                            }
                            ((ListEmp)this.ActiveMdiChild).dgEmp.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Данные найдены в базе данных работников.\nНо заданные критерии фильтрации не позволяют\nпоместить курсор на нужную позицию.\nИзмените критерии фильтра и попробуйте еще раз.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Метод заполняет данные в таблице работников.
        /// </summary>
        public void RefreshGrid(ListEmp _listEmp)
        {
            _listEmp.bsEmp.PositionChanged -= _listEmp.PositionChange;
            int pos = _listEmp.bsEmp.Position;
            _listEmp.dtEmp.Clear();
            _listEmp.dtEmp.Fill();
            _listEmp.RefreshGridEmp();
            _listEmp.bsEmp.PositionChanged += new EventHandler(_listEmp.PositionChange);
            _listEmp.bsEmp.Position = pos;
        }

        /// <summary>
        /// Отображение формы списка работников со срочными трудовыми договорами и доп.соглашениями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransfer_Term_Click(object sender, EventArgs e)
        {
            /*Connect_To_AD viewAD = new Connect_To_AD();
            viewAD.Show();*/
            if (this.MdiChildren.Where(i => i.Name == "listemp_term").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name == "listemp_term").First().Activate();
                textQuery = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),
                    Connect.Schema, "ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM");
                return;
            }
            emp = new EMP_seq(Connect.CurConnect);
            textQuery = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),
                Connect.Schema, "ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM");
            /// Создаем таблицу работников завода.
            OracleDataTable dtEmp = new OracleDataTable(textQuery, Connect.CurConnect);
            /// Создаем форму списка работников завода.
            listemp = new ListEmp(dtEmp, this, "listemp_term");
            listemp.MdiParent = this;
            listemp.Text = "Список работников со срочным трудовым договором и доп. соглашениям";
            CreateButtonApp(listemp, sender);
            listemp.Show();
            /* Изменения от 20,07,2013 - если у пользователя роль STAFF_VIEW_ONLYLISTEMP - 
             * то отключаем кнопку отчета произвольной формы*/
            if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
            {
                btRepOtherType.Enabled = false;
            }
            else
            {
                btRepOtherType.Enabled = true;
            }
        }
        
        /// <summary>
        /// Список сотрудников для установки подкласса условий труда
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransferCond_Of_Work_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "listEmpConditionWork").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name == "listEmpConditionWork").First().Activate();
                return;
            }
            ListEmpConditionWork listEmpConditionWork = new ListEmpConditionWork(this);
            listEmpConditionWork.MdiParent = this;
            listEmpConditionWork.Name = "listEmpConditionWork";
            CreateButtonApp(listEmpConditionWork, sender);
            listEmpConditionWork.Show();
        }

        /// <summary>
        /// Список переводов для обработки группой приема
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransfer_Emp_For_Group_Hire_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_FOR_GH").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_FOR_GH").First().Activate();
                return;
            }
            Wpf_Control_Viewer _dis = new Wpf_Control_Viewer();
            _dis.Name = "TRANSFER_FOR_GH";
            _dis.Text = "Список переводов для обработки";
            Transfer_For_Group_Hire_View _transfer_Emp_View = new Transfer_For_Group_Hire_View();
            _dis.elementHost1.Child = _transfer_Emp_View;
            _dis.MdiParent = this;
            _dis.WindowState = FormWindowState.Maximized;
            _dis.Show();
        }

        /// <summary>
        /// Просмотр списка резюме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btResume_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "RESUME").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "RESUME").First().Activate();
                return;
            }
            Wpf_Control_Viewer resume = new Wpf_Control_Viewer();
            resume.Name = "RESUME";
            resume.Text = "Резюме";
            Resume_Viewer resume_Viewer = new Resume_Viewer();
            resume.elementHost1.Child = resume_Viewer;
            resume.MdiParent = this;
            resume.WindowState = FormWindowState.Maximized;
            resume.Show();
        }

#endregion

        #region Перехват исключений и обработка дочерних форм
        public void UnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                TextWriter writer = new StreamWriter(Application.StartupPath + "\\LOG\\" +
                    string.Format("{0}.{1:00}.{2:00} {3:00}.{4:00}.{5:00} - {6}", DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                    Connect.UserId) + ".txt", false, Encoding.GetEncoding(1251));
                writer.WriteLine("Путь приложения: " + Application.ExecutablePath);
                writer.WriteLine("Объект: " + sender.ToString());
                writer.WriteLine("Ошибка: " + e.Exception.Message);
                writer.WriteLine("Компонент: " + e.Exception.Source);
                writer.WriteLine("Метод: " + e.Exception.TargetSite);
                System.Collections.IDictionaryEnumerator it = e.Exception.Data.GetEnumerator();
                while (it.MoveNext())
                    writer.WriteLine("Данные: " + it.Current.ToString());
                try
                {
                    writer.WriteLine("Пользователь:" + Connect.UserId.ToUpper());
                    //Application.
                }
                catch
                {
                    writer.WriteLine("Пользователь: неизвестно");
                }
                writer.Close();
            }
            catch
            { }
            if (MessageBox.Show("В приложении возникла непредвиденная ошибка. \n Автоматически сгенерирован файл отчета. Приносим свои извинения. \nПерезагрузить программу?",
                "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
            {
                Application.ExitThread();
                Application.Restart();
            }
        }

        /// <summary>
        /// Обработчик всех исключений приложения
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                TextWriter writer = new StreamWriter(Application.StartupPath + "\\LOG\\" +
                    string.Format("{0}.{1:00}.{2:00} {3:00}.{4:00}.{5:00} - {6}", DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                    Connect.UserId) + ".txt", false, Encoding.GetEncoding(1251));
                writer.WriteLine("Путь приложения: " + Application.ExecutablePath);
                writer.WriteLine("Объект: " + sender.ToString());
                writer.WriteLine("Ошибка: " + e.ExceptionObject.ToString());
                writer.Close();
            }
            catch
            { }
            if (MessageBox.Show("В приложении возникла непредвиденная ошибка. \n Автоматически сгенерирован файл отчета. Приносим свои извинения. \nПеразагрузить программу?",
                "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
            {
                Application.ExitThread();
                Application.Restart();
            }
        }

        void FormMain_MdiChildActivate(object sender, EventArgs e)
        {
            try
            {
                if (FormTabDepend.ContainsKey(this.ActiveMdiChild.GetHashCode()))
                    this.ribbon1.CurrentTabPage = this.ribbon1.TabPages[FormTabDepend[this.ActiveMdiChild.GetHashCode()]];
                else
                {
                    FormTabDepend.Add(this.ActiveMdiChild.GetHashCode(), this.ribbon1.TabPages.IndexOf(this.ribbon1.CurrentTabPage));
                    this.ActiveMdiChild.FormClosing += new FormClosingEventHandler(ActiveMdiChild_FormClosing);
                }
            }
            catch { }
        }

        void ActiveMdiChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormTabDepend.ContainsKey(sender.GetHashCode()))
                FormTabDepend.Remove(sender.GetHashCode());
        }

        #endregion

        #region Работа со справочниками

        // Работа со справочником Вид обучения
        private void btType_Study_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_STUDY_seq));
        }

        // Работа со справочником Вид образования
        private void btType_Edu_Click(object sender, EventArgs e)
        {
            //formspr = new HandBook(this, typeof(TYPE_EDU_seq));
            HandBookSpec handBookSpec = new HandBookSpec(this, "type_edu");
            handBookSpec.MdiParent = this;
            handBookSpec.Show();
        }

        // Работа со справочником учебных заведений
        private void btInstit_Click(object sender, EventArgs e)
        {
            HandBookInstit handBookInstit = new HandBookInstit(this);
            handBookInstit.MdiParent = this;
            handBookInstit.Show();
        }

        // Работа со справочником документов личности
        private void btType_Per_Doc_Click(object sender, EventArgs e)
        {
            HandBookSpec handBookSpec = new HandBookSpec(this, "type_per_doc");
            handBookSpec.MdiParent = this;
            handBookSpec.Show();
        }

        // Работа со справочником городов учебных заведений
        private void btStudy_City_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(STUDY_CITY_seq));
        }

        // Работа со справочником Группы специальности
        private void btGroup_Spec_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(GROUP_SPEC_seq));
        }

        // Работа со Справочником квалификаций
        private void btQual_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(QUAL_seq));
        }

        // Работа со справочником иностранных языков
        private void btLang_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(LANG_seq));
        }

        // Работа со справочником Степень владения языком
        private void btLevel_Know_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(LEVEL_KNOW_seq));
        }

        // Работа со справочником Военный комиссариат
        private void btComm_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(COMM_seq));
        }

        // Работа со справочником Коды ВУС
        private void btMil_Spec_Click(object sender, EventArgs e)
        {
            //formspr = new FormSpr(this, connection, typeof(MIL_SPEC_seq));
        }

        // Работа со справочником Категории годности
        private void btMed_Classif_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(MED_CLASSIF_seq));
        }

        // Работа со справочником воинских званий
        private void btMil_Rank_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(MIL_RANK_seq));
        }

        // Работа со справочником Воинский состав
        private void btMil_Cat_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(MIL_CAT_seq));
        }

        // Работа со справочником Виды войск
        private void btType_Troop_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_TROOP_seq));
        }

        // Работа со справочником Типы льгот
        private void btType_Priv_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_PRIV_seq));
        }

        // Работа со справочником Вид отпуска
        private void btType_Vac_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_VAC_seq));
        }

        // Работа со справочником Степень родства
        private void btRel_Type_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(REL_TYPE_seq));
        }

        // Работа со справочником Состояние в браке
        private void btMar_State_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(MAR_STATE_seq));
        }

        // Работа со справочником оснований документов
        private void btBase_Doc_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(BASE_DOC_seq));
        }

        // Работа со справочником Вид повышения квалификации
        private void btType_Rise_Qual_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_RISE_QUAL_seq));
        }

        // Работа со справочником Специальности
        private void btSpeciality_Click(object sender, EventArgs e)
        {
            HandBookSpec handBookSpec = new HandBookSpec(this, "speciality");
            handBookSpec.MdiParent = this;
            handBookSpec.Show();
        }

        // Работа со справочником Тип перевода  
        private void btPrivileged_Position_Click(object sender, EventArgs e)
        {
            HBPrivileged_Position priv_pos = new HBPrivileged_Position();
            priv_pos.MdiParent = this;
            priv_pos.WindowState = FormWindowState.Maximized;
            priv_pos.Show();
        }

        // Работа со справочником Характер работы
        private void btChar_Works_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(CHAR_WORK_seq));
        }

        // Работа со Справочником типов работ
        private void btWork_Type_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(WORK_TYPE_seq));
        }

        // Работа со Справочником служб
        private void btServices_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(SERVICE_seq));
        }

        // Работа со Справочником тарифных сеток
        private void btTariff_Grids_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TARIFF_GRID_seq));
        }

        // Работа со справочником Вид послевузовского образования
        private void btType_Postg_Study_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_POSTG_STUDY_seq));
        }
        
        // Работа со справочником источников комплектования
        private void btSource_Complect_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(SOURCE_COMPLECT_seq));
        }

        // Работа со справочником источников трудоустройства
        private void btSource_Employability_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(SOURCE_EMPLOYABILITY_seq));
        }

        // Работа со справочником причин увольнения
        private void btReason_dismiss_Click(object sender, EventArgs e)
        {
            HandBookSpec handBookSpec = new HandBookSpec(this, "reason_dismiss");
            handBookSpec.MdiParent = this;
            handBookSpec.Show();
        }

        /// <summary>
        /// Работа со справочником должностей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPosition_Click(object sender, EventArgs e)
        {
            HBPosition hbPosition = new HBPosition(this);
            hbPosition.MdiParent = this;
            hbPosition.Show();
        }

        /// <summary>
        /// Работа со справочником подразделений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdiv_Click(object sender, EventArgs e)
        {
            HBSubdiv hbSubdiv = new HBSubdiv(this);
            hbSubdiv.MdiParent = this;
            hbSubdiv.Show();
        }

        /// <summary>
        /// Справочник наименований наград
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReward_Name_Click(object sender, EventArgs e)
        {
            HandBookReward hbReward = new HandBookReward(this);
            hbReward.MdiParent = this;
            hbReward.Show();
        }

        /// <summary>
        /// Справочник типов наград
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btType_Reward_Click(object sender, EventArgs e)
        {
            formspr = new HandBook(this, typeof(TYPE_REWARD_seq));
        }

        /// <summary>
        /// Справочник подклассов условий труда
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConditions_Of_Work_Click(object sender, EventArgs e)
        {
            HBConditionWork hbConditionWork = new HBConditionWork(this);
            hbConditionWork.MdiParent = this;
            hbConditionWork.Show();
        }

    #endregion 

    #region Формирование отчетов

        /// <summary>
        /// Отчет о количестве уволенных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepCountDismiss_Click(object sender, EventArgs e)
        {
            RepCountDismiss repCountDismiss = new RepCountDismiss();
            repCountDismiss.ShowDialog();
        }

        /// <summary>
        /// Отчет о количестве уволенных (обратная сторона)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepCountDismissR_Click(object sender, EventArgs e)
        {
            RepCountDismissR repCountDismissR = new RepCountDismissR();
            repCountDismissR.ShowDialog();
        }

        /// <summary>
        /// Отчет по причинам увольнения (1 форма)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepReasonDismiss1_Click(object sender, EventArgs e)
        {
            RepReasonDismiss1 repReasonDismiss1 = new RepReasonDismiss1();
            repReasonDismiss1.ShowDialog();
        }

        /// <summary>
        /// Отчет по причинам увольнения (2 форма)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepReasonDismiss2_Click(object sender, EventArgs e)
        {
            RepReasonDismiss2 repReasonDismiss2 = new RepReasonDismiss2();
            repReasonDismiss2.ShowDialog();
        }

        /// <summary>
        /// Отчет о количестве принятых
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepCountHire_Click(object sender, EventArgs e)
        {
            RepCountHire repCountHire = new RepCountHire();
            repCountHire.ShowDialog();
        }

        /// <summary>
        /// Опись приказов о приеме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListOrdHire_Click(object sender, EventArgs e)
        {
            ListOrdHire listOrdHire = new ListOrdHire();
            listOrdHire.ShowDialog();
        }

        /// <summary>
        /// Отчет Нарушения за период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btViolations_By_Period_Click(object sender, EventArgs e)
        {
            SelectPeriod selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("RepViolations_By_Period.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_BEGIN", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_DATE_END", OracleDbType.Date).Value = selPeriod.EndDate;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "Table1");
                if (_ds.Tables["Table1"].Rows.Count > 0)
                {
                        ReportViewerWindow.RenderToExcel(this, "RepViolations_By_Period.rdlc", _ds.Tables["Table1"],
                            new List<ReportParameter>() {
                                new ReportParameter("P_DATE_BEGIN", selPeriod.BeginDate.ToShortDateString()),
                                new ReportParameter("P_DATE_END", selPeriod.EndDate.ToShortDateString())},
                            "Нарушители за период");
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Опись приказов об увольнении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListOrdDismiss_Click(object sender, EventArgs e)
        {
            ListOrdDismiss listOrdDismiss = new ListOrdDismiss();
            listOrdDismiss.ShowDialog();
        }

        /// <summary>
        /// Окончание срока договора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEndOfContr_Click(object sender, EventArgs e)
        {
            EndOfContr endOfContr = new EndOfContr();
            endOfContr.ShowDialog();
        }

        /// <summary>
        /// Книга приказов об увольнении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrdDismiss_Click(object sender, EventArgs e)
        {
            BkOrdDismiss bkOrdDismiss = new BkOrdDismiss();
            bkOrdDismiss.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о переводах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrdTransfer_Click(object sender, EventArgs e)
        {
            BkOrdTransfer bkOrdTransfer = new BkOrdTransfer();
            bkOrdTransfer.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о приеме за период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBkOrdHireForPeriod_Click(object sender, EventArgs e)
        {
            BkOrdHireForPeriod bkOrdHireForPeriod = new BkOrdHireForPeriod();
            bkOrdHireForPeriod.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о приеме по году и номеру последнего напечатанного приказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBkOrdHireYear_Click(object sender, EventArgs e)
        {
            BkOrdHireYear bkOrdHireYear = new BkOrdHireYear();
            bkOrdHireYear.ShowDialog();
        }

        /// <summary>
        /// Список для заполнения страховых свидетельств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEmptyInsurNum_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Список для заполнения страховых свидетельств\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.EmptyInsurNum();
            }

        }

        /// <summary>
        /// Список уволенных полный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListDismissFull_Click(object sender, EventArgs e)
        {
            ListDismissFull listDismissFull = new ListDismissFull();
            listDismissFull.ShowDialog();
        }

        /// <summary>
        /// Список уволенных без адресов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListDismissShort_Click(object sender, EventArgs e)
        {
            ListDismissShort listDismissShort = new ListDismissShort();
            listDismissShort.ShowDialog();
        }

        /// <summary>
        /// Список уволенных (сдельщики)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListDismissJobman_Click(object sender, EventArgs e)
        {
            ListDismissJobman listDismissJobman = new ListDismissJobman();
            listDismissJobman.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Численность, состав и движение по заводу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListRetirerStruct_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Данные по пенсионерам. Численность, состав и движение по заводу\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListRetirerStruct();
            }
        }

        /// <summary>
        /// Данные по пенсионерам. Численность, состав и движение по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListRetirerStructSubdiv_Click(object sender, EventArgs e)
        {
            ListRetirerStructSubdiv listRetirerStructSubdiv = new ListRetirerStructSubdiv();
            listRetirerStructSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Список пенсионеров по заводу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListRetirerPlant_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Данные по пенсионерам. Список пенсионеров по заводу\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListRetirerPlant();
            }
        }

        /// <summary>
        /// Данные по пенсионерам. Список пенсионеров по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListRetirerSubdiv_Click(object sender, EventArgs e)
        {
            ListRetirerSubdiv listRetirerSubdiv = new ListRetirerSubdiv();
            listRetirerSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Список уволенных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListRetirerDismiss_Click(object sender, EventArgs e)
        {
            ListRetirerDismiss listRetirerDismiss = new ListRetirerDismiss();
            listRetirerDismiss.ShowDialog();
        }

        /// <summary>
        /// Список для заполнения страховых свидетельств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEmptyInsurNum_Leave(object sender, EventArgs e)
        {
            this.ddReports.HidePopup();
            Form1.EmptyInsurNum();
        }

        /// <summary>
        /// Справка-объективка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSprObj_Click(object sender, EventArgs e)
        {
            SprObj sprObj = new SprObj();
            sprObj.ShowDialog();
        }

        /// <summary>
        /// Личная карточка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPersonalCard_Click(object sender, EventArgs e)
        {
            FormPersonCard formPersonCard = new FormPersonCard();
            formPersonCard.ShowDialog();
        }

        /// <summary>
        /// Список работников, имеющих ученую степень
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListAcadDegree_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Список работников, имеющих ученую степень\"?", "АРМ Кадры", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListAcadDegree();
            }
        }

        /// <summary>
        /// Отчет произвольной формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepOtherType_Click(object sender, EventArgs e)
        {
            try
            {
                string full_str = "";
                //OracleCommand command = new OracleCommand(string.Format("truncate table {0}.PN_TMP", Connect.Schema), connection);
                OracleCommand command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                command.CommandText = string.Format(
                    "delete from {0}.PN_TMP where user_name = :p_user_name", Connect.Schema);
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = 
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                Connect.Commit();
                command = new OracleCommand(string.Format(
                    "insert into {0}.PN_TMP(PNUM, USER_NAME,TRANSFER_ID) values (:PN, :UN, :TR)", 
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                command.Parameters.Add("PN", OracleDbType.Varchar2, 0, "PN");
                command.Parameters.Add("UN", OracleDbType.Varchar2, 0, "UN").Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal, 0, "TR");
                for (int i = 0; i < ((ListEmp)this.ActiveMdiChild).dgEmp.RowCount; i++)
                {
                    command.Parameters[0].Value = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[i].Cells["per_num"].Value.ToString();
                    command.Parameters[2].Value = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[i].Cells["TRANSFER_ID"].Value;
                    command.ExecuteNonQuery();
                }
                Connect.Commit();
                //full_str = string.Format("Exists (SELECT PNUM FROM {0}.PN_tmp WHERE em.per_num = PNUM and user_name = '{1}'", Connect.Schema, Connect.UserId.ToUpper());
                full_str = string.Format("TR.TRANSFER_ID in (SELECT TRANSFER_ID FROM {0}.PN_tmp WHERE user_name = '{1}'", Connect.Schema, Connect.UserId.ToUpper());
                int pos = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.IndexOf("order by");
                string strOrder = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.Substring(pos);
                //RepOtherType repOtherType = new RepOtherType(connection, Connect.Schema, full_str.Substring(0, full_str.Length - 1), strOrder);
                RepOtherType repOtherType = new RepOtherType(/*full_str, strOrder, */filterOnSubdiv, flagArchive);
                repOtherType.ShowDialog();
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Численность сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRep_Staff_Subdiv_Click(object sender, EventArgs e)
        {
            FormRepStaff form = new FormRepStaff();
            form.ShowDialog();
        }

        /// <summary>
        /// Формирование Уведомление по трудовому договору
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNotice_Expiry_Contract_Click(object sender, EventArgs e)
        {
            Form_Notice_By_Emp("Notice_Expiry.sql", "Notice_Expiry_Contract.rdlc", "Уведомление по трудовому договору");
        }

        /// <summary>
        /// Формирование Уведомление по доп.соглашению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNotice_Expiry_Add_Agreement_Click(object sender, EventArgs e)
        {
            Form_Notice_By_Emp("Notice_Expiry.sql", "Notice_Expiry_Add_Agreement.rdlc", "Уведомление по доп.соглашению");
        }

        /// <summary>
        /// Формирование уведомлений
        /// </summary>
        /// <param name="nameQuery">Имя запроса</param>
        /// <param name="namePattern">Имя шаблона</param>
        /// <param name="nameTempFile">Имя файла для сохранения</param>
        void Form_Notice_By_Emp(string nameQuery, string namePattern, string nameTempFile)
        {
            WpfControlLibrary.Find_Emp find_Emp = new WpfControlLibrary.Find_Emp(DateTime.Today);
            WindowInteropHelper wih = new WindowInteropHelper(find_Emp);
            wih.Owner = this.Handle;
            if (find_Emp.ShowDialog() == true)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery(nameQuery),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = find_Emp.Transfer_ID;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "Table1");
                _daReport.Fill(_ds, "Table1");
                if (_ds.Tables["Table1"].Rows.Count > 0)
                {
                    string[][] s_pos = new string[][] { };
                    if (Vacation_schedule.Signes.Show(this, 0, "Notice_Expiry", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos) == DialogResult.OK)
                        ReportViewerWindow.RenderToExcel(this, namePattern, _ds.Tables["Table1"],
                            new List<ReportParameter>() {
                                new ReportParameter("P_SIGNES_POS", s_pos[0][0]),
                                new ReportParameter("P_SIGNES_FIO", s_pos[0][1])},
                            nameTempFile);
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование Справка по месту требования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReference_On_Place_Requirements_Click(object sender, EventArgs e)
        {
            WpfControlLibrary.Find_Emp find_Emp = new WpfControlLibrary.Find_Emp(DateTime.Today);
            WindowInteropHelper wih = new WindowInteropHelper(find_Emp);
            wih.Owner = this.Handle;
            if (find_Emp.ShowDialog() == true)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Reference_By_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = find_Emp.Transfer_ID;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "Table1");
                if (_ds.Tables["Table1"].Rows.Count > 0)
                {
                    DataTable _dt = new DataTable();
                    if (Vacation_schedule.Signes.Show(this, 0, "Reference_By_Emp", "Введите должность и ФИО ответственного лица (для подписи)", 2, ref _dt) == DialogResult.OK)
                    {
                        ReportViewerWindow.RenderToExcel(this, "Reference_On_Place_Requirements.rdlc", new DataTable[] { _ds.Tables["Table1"], _dt },
                            null, "Справка по месту требования", "doc");
                    }
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование Справка по уходу за ребенком
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReference_Child_Realing_Leave_Click(object sender, EventArgs e)
        {
            WpfControlLibrary.Find_Emp find_Emp = new WpfControlLibrary.Find_Emp(DateTime.Today);
            WindowInteropHelper wih = new WindowInteropHelper(find_Emp);
            wih.Owner = this.Handle;
            if (find_Emp.ShowDialog() == true)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("RepReference_Pregnancy.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = find_Emp.Transfer_ID;
                _daReport.SelectCommand.Parameters.Add("p_RELATIVE_ID", OracleDbType.Decimal);
                _daReport.SelectCommand.Parameters.Add("p_DATE", OracleDbType.Date).Value = DateTime.Today;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "Table1");
                if (_ds.Tables["Table1"].Rows.Count > 0)
                {
                    DataTable _dt = new DataTable();
                    if (Vacation_schedule.Signes.Show(this, 0, "Reference_By_Emp", "Введите должность и ФИО ответственного лица (для подписи)", 2, ref _dt) == DialogResult.OK)
                    {
                        ReportViewerWindow.RenderToExcel(this, "Reference_Child_Care_Leave.rdlc", new DataTable[] { _ds.Tables["Table1"], _dt },
                            null, "Справка по уходу за ребенком", "doc");
                    }
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет по материально-ответственным лицам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepMRP_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Отчёт по материально-ответственным лицам\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.RepMRP();
            }
        }

        /// <summary>
        /// Отчет - Список переводов по льготным профессиям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransferPrivPos_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("SelectTransferPrivPos.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);

                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " + 
                        selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "TransferPrivPos.xlt", "A4", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        /// <summary>
        /// Формирование отчета Численность, состав и движение по заводу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNumbersVeteranPlant_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Данные по ветеранам труда. Численность, состав и движение по заводу\"?",
                "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListVeteranStruct();
            }
        }

        /// <summary>
        /// Формирование отчета Численность, состав и движение по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNumbersVeteranSub_Click(object sender, EventArgs e)
        {
            ListVeteransStructSubdiv listVeteransStructSubdiv =
                new ListVeteransStructSubdiv();
            listVeteransStructSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по ветеранам труда. Список ветеранов по заводу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListVeteranPlant_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Данные по ветеранам труда. Список ветеранов по заводу\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListVeteranPlant();
            }
        }

        /// <summary>
        /// Данные по ветеранам труда. Список уволенных.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListDismissVeteran_Click(object sender, EventArgs e)
        {
            ListVeteranDismiss listVeteranDismiss = new ListVeteranDismiss();
            listVeteranDismiss.ShowDialog();
        }

        /// <summary>
        /// Список ветеранов труда по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListVeteranSub_Click(object sender, EventArgs e)
        {
            ListVeteranSubdiv listVeteranSubdiv = new ListVeteranSubdiv();
            listVeteranSubdiv.ShowDialog();
        }

        /// <summary>
        /// Список инвалидов по заводу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInvalid_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Список инвалидов по заводу\"?",
                "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.ListInvalidPlant();
            }
        }

        /// <summary>
        /// Список инвалидов по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListInvalidSub_Click(object sender, EventArgs e)
        {
            ListInvalidSubdiv listInvalidSubdiv = new ListInvalidSubdiv();
            listInvalidSubdiv.ShowDialog();
        }

        /// <summary>
        /// Список уволенных - Без адресов (новая форма)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListDismissForDir_Click(object sender, EventArgs e)
        {
            ListDismissForDir listDismissForDir = new ListDismissForDir();
            listDismissForDir.ShowDialog();
        }
        
        /// <summary>
        /// Отчет МСФО - Демографическая и финансовая 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepMSFOSal_Click(object sender, EventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == DialogResult.OK)
            {
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepMSFOSal(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();                
            }
        }

        /// <summary>
        /// Формирование табеля
        /// </summary>
        /// <param name="data"></param>
        void RepMSFOSal(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            try
            {                
                ((BackgroundWorker)sender).ReportProgress(15);
                // Если формировали табель или нужно формировать приложение, то создаем команду

                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("SelectReportMSFOSal.sql"), Connect.Schema);
                odaReport.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateDump;
                DataTable dtReport = new DataTable();
                odaReport.Fill(dtReport);
                if (dtReport.Rows.Count > 0)
                {
                    Excel.PrintWithBorder(true, "ReportMSFOSal.xlt", "A4", new DataTable[] { dtReport },
                        new ExcelParameter[] { 
                            new ExcelParameter("A2", "по состоянию на " + dateDump.ToShortDateString()) });
                }
                else
                {
                    MessageBox.Show("АСУ \"Кадры\"", "За указанный период данные отсутствуют");
                }
                ((BackgroundWorker)sender).ReportProgress(80);
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Отчет МСФО - Статистика текучести персонала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepMSFODisEmp_Click(object sender, EventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == DialogResult.OK)
            {
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("SelectReportMSFODisEmp.sql"), Connect.Schema);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateDump;
                DataTable dtReport = new DataTable();
                odaReport.Fill(dtReport);
                if (dtReport.Rows.Count > 0)
                {
                    Excel.PrintWithBorder(true, "ReportMSFODisEmp.xlt", "A4", new DataTable[] { dtReport },
                        new ExcelParameter[] { 
                            new ExcelParameter("A2", "по состоянию на " + dateDump.ToShortDateString()) });
                }
                else
                {
                    MessageBox.Show("АСУ \"Кадры\"", "За указанный период данные отсутствуют");
                }
            }
        }
        
        /// <summary>
        /// Формирование отчета по льготникам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListEmpPrivPos_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dtReport = new DataTable();
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("SelectListEmpOnPrivPos.sql"),
                    Connect.Schema);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = selPeriod.BeginDate;
                odaReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = selPeriod.EndDate;
                odaReport.Fill(dtReport);
                if (dtReport.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", " за период с " + 
                        selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "ListEmpOnPrivPos.xlt", "A5", new DataTable[] { dtReport }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Работники участвующие в программе НПО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepNonState_Pens_Prov_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по участникам НПО?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand("", Connect.CurConnect);
                daReport.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("RepNonState_Pens_Prov.sql"), Connect.Schema);
                DataTable dtReport = new DataTable();
                daReport.Fill(dtReport);
                Excel.PrintWithBorder(true, "RepNonState_Pens_Prov.xlt", "A4", new DataTable[] { dtReport }, null);
            }
        }
        
        /// <summary>
        /// Работники участвующие в программе НПО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepNPP_With_Date_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по участникам НПО?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand("", Connect.CurConnect);
                daReport.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("RepNPP_With_Date.sql"), Connect.Schema);
                DataTable dtReport = new DataTable();
                daReport.Fill(dtReport);
                Excel.PrintWithBorder(true, "RepNPP_With_Date.xlt", "A4", new DataTable[] { dtReport }, null);
            }
        }

        /// <summary>
        /// Отчет Количество сотрудников по службам завода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEmp_By_Service_Click(object sender, EventArgs e)
        {
            NumbersEmpOnService numbers = new NumbersEmpOnService();
            numbers.ShowInTaskbar = false;
            numbers.ShowDialog();
        }
        
        /// <summary>
        /// Отчет по молодым специалистам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepYoung_Specialist_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null && 
                (this.ActiveMdiChild.Name.ToUpper() == "LISTEMP" || this.ActiveMdiChild.Name.ToUpper() == "LISTEMPARCH"))
            {
                selPeriod = new SelectPeriod();
                if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSet _dsReport = new DataSet();
                    OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                    switch (this.ActiveMdiChild.Name.ToUpper())
                    {
                        case "LISTEMPARCH":
                            odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepYoung_Specialist.sql"),
                                Connect.Schema, "not");
                            break;
                        case "LISTEMP":
                            odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepYoung_Specialist.sql"),
                                Connect.Schema, "");
                            break;
                        default: break;
                    }
                    odaReport.SelectCommand.BindByName = true;
                    odaReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                    odaReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                    odaReport.Fill(_dsReport, "REPORT");
                    if (_dsReport.Tables["REPORT"].Rows.Count > 0)
                    {
                        ReportViewerWindow report =
                            new ReportViewerWindow(
                                "Протокол переводов", "Reports/RepYoung_Specialist.rdlc",
                                _dsReport,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGINPERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_ENDPERIOD", selPeriod.EndDate.ToShortDateString())}
                            );
                        report.Show();
                    }
                    else
                    {
                        MessageBox.Show("За указанный период данные не найдены.",
                            "АСУ \"Кадры\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Необходимо активировать список сотрудников основной или архивной базы.",
                    "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// Отчет для администраторов о.78 - Список переводов за период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepTransfer_By_Period_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(false, true, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == DialogResult.OK)
            {
                DataTable dtRepTransSub = new DataTable();
                OracleDataAdapter odaRepTransSub = new OracleDataAdapter(
                    string.Format(Queries.GetQuery("RepTransfer_By_Period.sql"),
                    Connect.Schema),
                    Connect.CurConnect);
                odaRepTransSub.SelectCommand.BindByName = true;
                odaRepTransSub.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                    selPeriod_Subdiv.BeginDate;
                odaRepTransSub.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                    selPeriod_Subdiv.EndDate;
                odaRepTransSub.Fill(dtRepTransSub);
                if (dtRepTransSub.Rows.Count > 0)
                {
                    Excel.PrintWithBorder(true, "RepTransfer_By_Period.xlt", "A5",
                        new DataTable[] { dtRepTransSub },
                        new ExcelParameter[] {new ExcelParameter("A3", "за период" +
                            " с " + selPeriod_Subdiv.BeginDate.ToShortDateString() + " по " +
                            selPeriod_Subdiv.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("В подразделении за указанный период нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отобразить форму формирования приказа о поощрении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrderOnEncouraging_Click(object sender, EventArgs e)
        {
            OrderOnEncouraging _orderOnEncouraging = new OrderOnEncouraging();
            _orderOnEncouraging.ShowDialog();
        }

        /// <summary>
        /// Формирование доп. соглашений по подразделению на определенную дату
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_Agreement_On_Subdiv_Click(object sender, EventArgs e)
        {
            SelectSubdiv_And_Date _formSelect = new SelectSubdiv_And_Date();
            if (_formSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmps_For_Add_Agreement.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_TARIFF", OracleDbType.Date).Value = _formSelect.SelectedDate;
                _daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = _formSelect.Subdiv_ID;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "ORDER_EMP");
                if (_ds.Tables["ORDER_EMP"].Rows.Count > 0)
                {
                    ReportViewerWindow _rep = new ReportViewerWindow("Дополнительные соглашения", "Reports/Add_Agreement_Short.rdlc",
                        _ds, null, true);
                    _rep.Show();
                    //ReportViewerWindow.RenderToExcel(this, "Add_Agreement_Short.rdlc", _ds.Tables["ORDER_EMP"], null);
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование уведомлений о сокращении рабочего времени
        /// </summary>
        /// <param name="namePattern">Имя шаблона</param>
        void Notification_Short_Work(string namePattern)
        {
            SelectSubdiv_And_Date _formSelect = new SelectSubdiv_And_Date();
            if (_formSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmps_For_Notification.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = _formSelect.SelectedDate;
                _daReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = _formSelect.SelectedDate;
                _daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = _formSelect.Subdiv_ID;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "ORDER_EMP");
                if (_ds.Tables["ORDER_EMP"].Rows.Count > 0)
                {
                    string[][] s_pos = new string[][] { };
                    if (Vacation_schedule.Signes.Show(this, _formSelect.Subdiv_ID, "Notice_Expiry", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos) == DialogResult.OK)
                    {
                        ReportViewerWindow _rep = new ReportViewerWindow("Уведомление", "Reports/" + namePattern,
                            _ds, new List<ReportParameter>() {
                                new ReportParameter("P_SIGNES_POS", s_pos[0][0]),
                                new ReportParameter("P_SIGNES_FIO", s_pos[0][1])}, true);
                        _rep.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Уведомление о сокращении рабочей недели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNotification_Short_Week_Click(object sender, EventArgs e)
        {
            Notification_Short_Work("Notification_Short_Week.rdlc");
        }

        /// <summary>
        /// Уведомление о позднем начале рабочего времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNotification_Late_Start_Click(object sender, EventArgs e)
        {
            Notification_Short_Work("Notification_Late_Start.rdlc");
        }

        /// <summary>
        /// Уведомление о раннем окончании рабочего времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNotification_Early_Ending_Click(object sender, EventArgs e)
        {
            Notification_Short_Work("Notification_Early_Ending.rdlc");
        }

        /// <summary>
        /// Отчет допсоглашение для сокращенного дня/недели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_AgreementShortDay_On_Subdiv_Click(object sender, EventArgs e)
        {
            AddAgreementShortDay("DAY");
        }

        /// <summary>
        /// Отчет допсоглашение для сокращенного дня/недели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_AgreementShortDay_On_Subdiv_Click1(object sender, EventArgs e)
        {
            AddAgreementShortDay("WEEK");
        }

        /// <summary>
        /// Формируем отчет по доп соглашениям с параметром либо это день или неделя
        /// </summary>
        /// <param name="short_day_week"></param>
        private void AddAgreementShortDay(string short_day_week)
        {
            RepFilterByEmp _formSelect = new RepFilterByEmp(null, null, new DateTime(2016, 06, 6), new DateTime(2016, 12, 5), false);
            WindowInteropHelper f = new WindowInteropHelper(_formSelect);
            f.Owner = this.Handle;
            _formSelect.TextBeginCaption = "Начало действия доп. согл.";
            _formSelect.TextEndCaption = "Окончание действия доп. согл.";
            _formSelect.BySubdivReport = true;
            if (_formSelect.ShowDialog() == true)
            {
                DataTable t = new DataTable();
                try
                {
                    OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmps_For_Add_AgreementShortDay.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daReport.SelectCommand.BindByName = true;
                    _daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = _formSelect.SubdivID;
                    _daReport.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
                    _daReport.Fill(t);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения списка сотрудников. " + ex.Message, "Кадры");
                    return;
                }
                string[][] ss = null;
                if (Signes.Show(this, 0, "AddAgreementShortDay", "Выберите ответственное лицо", 1, ref ss) == System.Windows.Forms.DialogResult.OK)
                {
                    ReportViewerWindow.ShowReport("Дополнительные соглашения", "Add_Agreement_ShortDay.rdlc", t, new ReportParameter[]{
                        new ReportParameter("P_DATE_BEGIN", _formSelect.DateBegin.Value.ToShortDateString()),
                        new ReportParameter("P_DATE_END", _formSelect.DateEnd.Value.ToShortDateString()),
                        new ReportParameter("P_SHORT_DAY_SIGN", short_day_week),
                        new ReportParameter("P_FIO", ss[0][1]),
                        new ReportParameter("P_POS_NAME", ss[0][0])}
                        );
                }
            }
        }

    #endregion             

    #region Сортировка и перемещение по списку сотрудников
        /// <summary>
        /// Событие нажатия кнопки закрытия дочернего окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExitWindow_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                this.ActiveMdiChild.Close();                
            }
        }

        /// <summary>
        /// Метод меняет сортировку списка работников и перезаполняет таблицу
        /// </summary>
        /// <param name="sort">Строка сортировки</param>
        void Sorter(string sort)
        {
            string tnom, strSelect;
            int pos;
            switch (this.ActiveMdiChild.Name.ToUpper())
            {
                case "LISTEMPARCH" :
                    tnom = listEmpArch.dgEmp.Rows[listEmpArch.bsEmp.Position].Cells["per_num"].Value.ToString();
                    pos = listEmpArch.dtEmp.SelectCommand.CommandText.IndexOf("order by");
                    strSelect = listEmpArch.dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
                    listEmpArch.bsEmp.PositionChanged -= listEmpArch.PositionChange;
                    listEmpArch.dtEmp.Clear();
                    listEmpArch.dtEmp.SelectCommand.CommandText = strSelect;
                    listEmpArch.dtEmp.Fill();
                    listEmpArch.bsEmp.PositionChanged += new EventHandler(listEmpArch.PositionChange);
                    for (int i = 0; i < listEmpArch.dgEmp.Rows.Count; i++)
                    {
                        if (listEmpArch.dgEmp["per_num", i].Value.ToString() == tnom)
                        {
                            listEmpArch.bsEmp.Position = listEmpArch.dgEmp["per_num", i].RowIndex;
                            break;
                        }
                    }
                    break;
                case "LISTEMP":
                    tnom = listemp.dgEmp.Rows[listemp.bsEmp.Position].Cells["per_num"].Value.ToString();
                    pos = listemp.dtEmp.SelectCommand.CommandText.IndexOf("order by");
                    strSelect = listemp.dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
                    listemp.bsEmp.PositionChanged -= listemp.PositionChange;
                    listemp.dtEmp.Clear();
                    listemp.dtEmp.SelectCommand.CommandText = strSelect;
                    listemp.dtEmp.Fill();
                    listemp.bsEmp.PositionChanged += new EventHandler(listemp.PositionChange);
                    for (int i = 0; i < listemp.dgEmp.Rows.Count; i++)
                    {
                        if (listemp.dgEmp["per_num", i].Value.ToString() == tnom)
                        {
                            listemp.bsEmp.Position = listemp.dgEmp["per_num", i].RowIndex;
                            break;
                        }
                    }
                    break;
                case "LISTEMP_TERM":
                    tnom = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                    pos = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.ToUpper().IndexOf("ORDER BY");
                    strSelect = ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
                    ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged -= ((ListEmp)this.ActiveMdiChild).PositionChange;
                    ((ListEmp)this.ActiveMdiChild).dtEmp.Clear();
                    ((ListEmp)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText = strSelect;
                    ((ListEmp)this.ActiveMdiChild).dtEmp.Fill();
                    ((ListEmp)this.ActiveMdiChild).bsEmp.PositionChanged += new EventHandler(((ListEmp)this.ActiveMdiChild).PositionChange);
                    for (int i = 0; i < ((ListEmp)this.ActiveMdiChild).dgEmp.Rows.Count; i++)
                    {
                        if (((ListEmp)this.ActiveMdiChild).dgEmp["per_num", i].Value.ToString() == tnom)
                        {
                            ((ListEmp)this.ActiveMdiChild).bsEmp.Position = ((ListEmp)this.ActiveMdiChild).dgEmp["per_num", i].RowIndex;
                            break;
                        }
                    }
                    break;
                default: break;
            }        
        }

        /// <summary>
        /// Метод меняет сортировку списка сторонних работников и перезаполняет таблицу
        /// </summary>
        /// <param name="sort">Строка сортировки</param>
        void SorterFR_Emp(string sort)
        {
            string perco_sync_id = listFR_Emp.dgFR_Emp.Rows[listFR_Emp.bsFR_Emp.Position].Cells["perco_sync_id"].Value.ToString();
            int pos = listFR_Emp.dtFR_Emp.SelectCommand.CommandText.IndexOf("order by");
            string strSelect = listFR_Emp.dtFR_Emp.SelectCommand.CommandText.Substring(0, pos) + sort;
            listFR_Emp.dtFR_Emp.Clear();
            listFR_Emp.dtFR_Emp.SelectCommand.CommandText = strSelect;
            listFR_Emp.dtFR_Emp.Fill();
            for (int i = 0; i < listFR_Emp.dtFR_Emp.Rows.Count; i++)
            {
                if (listFR_Emp.dgFR_Emp["perco_sync_id", i].Value.ToString() == perco_sync_id)
                {
                    listFR_Emp.bsFR_Emp.Position = listFR_Emp.dgFR_Emp["perco_sync_id", i].RowIndex;
                    break;
                }
            }
        }

        /// <summary>
        /// Сортировка по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivSorter_Click(object sender, EventArgs e)
        {            
            string sort = string.Format("order by {0}", SUBDIV_seq.ColumnsName.CODE_SUBDIV);
            Sorter(sort);
        }       

        /// <summary>
        /// Сортировка по табельному номеру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPer_NumSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}", EMP_seq.ColumnsName.PER_NUM);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по подразделению и табельному номеру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivPer_NumSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}", SUBDIV_seq.ColumnsName.CODE_SUBDIV, EMP_seq.ColumnsName.PER_NUM);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по фамилии, имени, отчеству
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFIOSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}",
                EMP_seq.ColumnsName.EMP_LAST_NAME,
                EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по подразделению и фамилии, имени, отчеству
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivFIOSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}, {3}",
                SUBDIV_seq.ColumnsName.CODE_SUBDIV, EMP_seq.ColumnsName.EMP_LAST_NAME,
                EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по дате рождения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBirth_DateSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}, {3}", EMP_seq.ColumnsName.EMP_BIRTH_DATE,
                EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по дню рождения (по порядку в течение месяца)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDay_BirthSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by to_char({0},'mm.dd.yyyy'), {1}, {2}, {3}",
                EMP_seq.ColumnsName.EMP_BIRTH_DATE, EMP_seq.ColumnsName.EMP_LAST_NAME, 
                EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по дате приема на работу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDate_HireSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}, {3}",
                TRANSFER_seq.ColumnsName.DATE_HIRE, EMP_seq.ColumnsName.EMP_LAST_NAME,
                EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Сортировка по дате увольнения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDate_DismissSorter_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}, {3}",
                TRANSFER_seq.ColumnsName.DATE_TRANSFER, EMP_seq.ColumnsName.EMP_LAST_NAME,
                EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME);
            Sorter(sort);
        }

        /// <summary>
        /// Событие нажатия кнопки перехода на первую запись в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUp_Click(object sender, EventArgs e)
        {
            try
            {
                ((ListEmp)this.ActiveMdiChild).bsEmp.MoveFirst();
            }
            catch
            { }
        }

        /// <summary>
        /// Событие нажатия кнопки перехода на последнюю запись в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDown_Click(object sender, EventArgs e)
        {
            try
            {
                ((ListEmp)this.ActiveMdiChild).bsEmp.MoveLast();
            }
            catch
            { }
        }

    #endregion

    #region Работа с переводами

        /// <summary>
        /// Событие нажатия кнопки добавления текущего перевода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                /// Табельный номер.
                string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                /// Данные сотрудника.
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                /// Текущая запись в переводах по основной работе.
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0", TRANSFER_seq.ColumnsName.PER_NUM,
                    per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.SIGN_COMB));
                /// Если записей по основной работе нет, то выводим сообщение об ошибке
                if (transfer.Count == 0)
                {
                    MessageBox.Show("Данных по основной деятельности работника нет!\nДобавить перевод невозможно!", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Строка перевода, дублирующая текущий перевод. Редактирование необходимых полей.
                TRANSFER_obj r_transfer = (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                r_transfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                r_transfer.FROM_POSITION = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
                r_transfer.CONTR_EMP = "";
                r_transfer.DATE_CONTR = null;
                r_transfer.DATE_TRANSFER = null;
                r_transfer.DATE_END_CONTR = null;
                r_transfer.TR_NUM_ORDER = "";
                r_transfer.TR_DATE_ORDER = null;
                r_transfer.TYPE_TRANSFER_ID = 2;
                r_transfer.FORM_PAY = null;
                r_transfer.DF_BOOK_ORDER = null;
                /// Переводы сотрудника, в которые добавляется новая отредактированная запись.
                TRANSFER_seq transferNew = new TRANSFER_seq(Connect.CurConnect);
                transferNew.AddObject(r_transfer);
                Transfer.flagAdd = true;
                /// Бухгалтерские данные по предыдущему переводу.
                ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
                accountPrev.Fill(string.Format("where TRANSFER_ID = {1} and " +
                    "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})", 
                    Connect.Schema, r_transfer.FROM_POSITION));
                /// Бухгалтерские данные. Добавление новой записи для нового перевода.
                ACCOUNT_DATA_obj r_account_date = (ACCOUNT_DATA_obj)((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[accountPrev]).Current)).Clone();
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.AddObject(r_account_date);
                ((ACCOUNT_DATA_obj)(((CurrencyManager)BindingContext[account]).Current)).TRANSFER_ID = 
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNew]).Current)).TRANSFER_ID;
                /// Форма для редактирования данных перевода.
                Transfer formtransfer = new Transfer(record_emp, transferNew, transfer, account,
                    accountPrev, false, true, true, ((ListEmp)this.ActiveMdiChild));
                formtransfer.Text = "Переводы";
                formtransfer.ShowDialog();
                RefreshGrid(((ListEmp)this.ActiveMdiChild));
            }
            catch (Exception exp1 )
            { MessageBox.Show( exp1.Message); }
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования данных текущего перевода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                /// Табельный номер.
                string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                /// Данные сотрудника.
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                /// Текущая запись в переводах по основной работе
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                    TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    MessageBox.Show("Данных по основной деятельности работника нет!\nРедактировать перевод невозможно!", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Бухгалтерские данные по текущему переводу.
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.Fill(string.Format("where TRANSFER_ID = {1} and " +
                    "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})", 
                    Connect.Schema, 
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                /// Предыдущий перевод.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                /// Если предыдущей позиции нет, не заполняем предыдущий перевод.
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION != null)
                    transferPrev.Fill(string.Format(" where {0} = {1}",
                        TRANSFER_seq.ColumnsName.TRANSFER_ID,
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FROM_POSITION));
                /// Бухгалтерские данные по предыдущему переводу.
                ACCOUNT_DATA_seq accountPrev = new ACCOUNT_DATA_seq(Connect.CurConnect);
                if (transferPrev.Count != 0)
                {
                    accountPrev.Fill(string.Format("where TRANSFER_ID = {1} and " +
                        "CHANGE_DATE = (select max(CHANGE_DATE) from {0}.ACCOUNT_DATA where TRANSFER_ID = {1})", 
                        Connect.Schema, 
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).TRANSFER_ID));
                }
                Transfer.flagAdd = false;
                /// Форма для редактирования данных перевода.
                Transfer formtransfer = new Transfer(record_emp, transfer, transferPrev, account,
                    accountPrev, false, true, true, ((ListEmp)this.ActiveMdiChild));
                formtransfer.Text = "Переводы";
                formtransfer.ShowDialog();
                RefreshGrid(((ListEmp)this.ActiveMdiChild));
            }
            catch { }
        }

        /// <summary>
        /// Событие нажатия кнопки удаления текущего перевода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteTransfer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить текущей перевод?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /* 13.04.2016 - после очередного неудачного удаления перевода переделываю код
                string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                /// Текущая запись в переводах.                
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                    TRANSFER_seq.ColumnsName.SIGN_COMB));
                /// Если человек не работает на заводе по основной деятельности.
                if (transfer.Count == 0)
                {
                    MessageBox.Show("Данных по основной деятельности работника нет!\nУдалить перевод невозможно!", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                /// Предыдущий перевод. Заполняется по полю FROM_POSITION текущего перевода.
                TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                transferPrev.Fill(string.Format(" where {0} = {1}",
                    TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FROM_POSITION));
                /// Бухгалтерские данные по текущему переводу. Удаляем их.
                OracleCommand ocDelAcc = new OracleCommand("", Connect.CurConnect);
                ocDelAcc.BindByName = true;
                ocDelAcc.CommandText = string.Format(
                    "delete from {0}.ACCOUNT_DATA where TRANSFER_ID = :p_transfer_id", 
                    Connect.Schema);
                ocDelAcc.Parameters.Add("p_transfer_id", 
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID);
                ocDelAcc.ExecuteNonQuery();
                //ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(connection);
                //account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                //foreach (ACCOUNT_DATA_obj ad in account)
                //{
                //    account.Remove(ad);
                //}
                ////account.Remove((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current);
                //account.Save();
                /// Ставим признак текущей работы предыдущему переводу.
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).SIGN_CUR_WORK = true;
                TRANSFER_obj r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current;
                /// Удаляем текущей перевод. Сохраняем данные.
                transfer.Remove((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current);
                transfer.Save();
                transferPrev.Save();
                Connect.Commit();
                /// Данные по сотруднику.
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                EMP_obj r_emp = (EMP_obj)((CurrencyManager)BindingContext[record_emp]).Current;
                /// Обновление данных в перко.
                FormMain.employees.UpdateEmployee(new PercoXML.Employee(r_emp.PERCO_SYNC_ID.ToString(),
                    r_emp.PER_NUM, r_emp.EMP_LAST_NAME, r_emp.EMP_FIRST_NAME, r_emp.EMP_MIDDLE_NAME,
                    r_transfer.SUBDIV_ID.ToString(), r_transfer.POS_ID.ToString()));
                RefreshGrid(((ListEmp)this.ActiveMdiChild));
                 */
                /* Делаю через процедуру с возможностью откатить все данные*/
                DataGridViewRow curRow = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position];
                OracleCommand _ocTransfer_Cur_Delete = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TRANSFER_CUR_delete(:p_TRANSFER_ID, :p_PREV_SUBDIV_ID, :p_PREV_POS_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocTransfer_Cur_Delete.BindByName = true;
                _ocTransfer_Cur_Delete.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                    curRow.Cells["TRANSFER_ID"].Value;
                _ocTransfer_Cur_Delete.Parameters.Add("p_PREV_SUBDIV_ID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                _ocTransfer_Cur_Delete.Parameters["p_PREV_SUBDIV_ID"].DbType = DbType.Decimal;
                _ocTransfer_Cur_Delete.Parameters.Add("p_PREV_POS_ID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                _ocTransfer_Cur_Delete.Parameters["p_PREV_POS_ID"].DbType = DbType.Decimal;
                OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocTransfer_Cur_Delete.Transaction = _transact;
                    _ocTransfer_Cur_Delete.ExecuteNonQuery();
                    _transact.Commit();

                    FormMain.employees.UpdateEmployee(new PercoXML.Employee(curRow.Cells["PERCO_SYNC_ID"].Value.ToString(),
                        curRow.Cells["PER_NUM"].Value.ToString(),
                        curRow.Cells["EMP_LAST_NAME"].Value.ToString(), 
                        curRow.Cells["EMP_FIRST_NAME"].Value.ToString(), 
                        curRow.Cells["EMP_MIDDLE_NAME"].Value.ToString(),
                        _ocTransfer_Cur_Delete.Parameters["p_PREV_SUBDIV_ID"].ToString(),
                        _ocTransfer_Cur_Delete.Parameters["p_PREV_POS_ID"].ToString()));

                    RefreshGrid(((ListEmp)this.ActiveMdiChild));
                }
                catch (Exception ex)
                {
                    _transact.Rollback();
                    MessageBox.Show(ex.Message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Работа с переводами по совмещению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransferComb_Click(object sender, EventArgs e)
        {
            /// Табельный номер.
            string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
            /// Данные сотрудника.
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Текущая запись в переводах по совмещению.
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1", TRANSFER_seq.ColumnsName.PER_NUM,
                per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.SIGN_COMB));
            /// Если записей по совмещению нет, то выводим сообщение об ошибке
            if (transfer.Count == 0)
            {
                MessageBox.Show("Данных по совмещению работника нет!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ListCombTransfer listCombTransfer = new ListCombTransfer(record_emp, ((ListEmp)this.ActiveMdiChild));
            listCombTransfer.ShowDialog();
            RefreshGrid(((ListEmp)this.ActiveMdiChild));
        }  

        /// <summary>
        /// Добавление старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddOld_Click(object sender, EventArgs e)
        {
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.AddNew();
            /// Табельный номер по которому добавляется перевод            
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).PER_NUM =
                ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).WORKER_ID = 
                (decimal)((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["WORKER_ID"].Value;
            OldTransfer oldTransfer = new OldTransfer(transfer, true);
            oldTransfer.Text = "Добавление старого перевода";
            oldTransfer.ShowDialog();
            RefreshGrid(((ListEmp)this.ActiveMdiChild));
        }

        /// <summary>
        /// Редактирование старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditOld_Click(object sender, EventArgs e)
        {
            /// Табельный номер по которому добавляется перевод
            int transfer_id = Convert.ToInt32(((ListEmp)this.ActiveMdiChild).dgTransfer.Rows[((ListEmp)this.ActiveMdiChild).dgTransfer.CurrentRow.Index].Cells["transfer_id"].Value);
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            transfer.Fill(string.Format("where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID, transfer_id));
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 1)
            {
                MessageBox.Show("Невозможно редактировать приемную запись!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            OldTransfer oldTransfer = new OldTransfer(transfer, false);
            oldTransfer.Text = "Редактирование старого перевода";
            oldTransfer.ShowDialog();
            RefreshGrid((ListEmp)this.ActiveMdiChild);
        }

        /// <summary>
        /// Удаление старых переводов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteOld_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить перевод?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Создаем таблицу для перевода.
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                /// Заполняем ее переводом, который сейчас выбран.
                transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    listemp.tcTransfer.SelectedIndex == 0 ?
                    Convert.ToInt32(listemp.dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                    Convert.ToInt32(listemp.dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
                /// Если выбран перевод, то продолжаем работу.
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TYPE_TRANSFER_ID == 2)
                {
                    /// Создаем новую таблицу.
                    TRANSFER_seq transferNext = new TRANSFER_seq(Connect.CurConnect);
                    /// Заполняем таблицу переводом, который является следующим после удаляемого перевода.
                    transferNext.Fill(string.Format("where {0} = {1}",
                        TRANSFER_seq.ColumnsName.FROM_POSITION,
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID));
                    /// Проверяем наличие следующего перевода.
                    if (transferNext.Count != 0)
                    {
                        /// Если он есть, то выставляем FROM_POSITION
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferNext]).Current)).FROM_POSITION =
                            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION;
                        /// Удаляем выбранный перевод.
                        transfer.Remove((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current));
                        /// Сохраняем все изменения в базе данных.
                        transferNext.Save();
                        transfer.Save();
                        Connect.Commit();
                        RefreshGrid((ListEmp)this.ActiveMdiChild);
                    }
                }
                /// Если не перевод, то выводим сообщение.
                else
                {
                    MessageBox.Show("Данный перевод удалить нельзя.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки просмотра данных перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btViewTransfer_Click(object sender, EventArgs e)
        {
            if (btViewTransfer.Enabled)
            {
                string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format(" where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                    ((ListEmp)this.ActiveMdiChild).tcTransfer.SelectedIndex == 0 ?
                    Convert.ToInt32(((ListEmp)this.ActiveMdiChild).dgTransfer.CurrentRow.Cells["transfer_id"].Value) :
                    Convert.ToInt32(((ListEmp)this.ActiveMdiChild).dgTransferComb.CurrentRow.Cells["transfer_id"].Value)));
                ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
                account.Fill(string.Format("where TRANSFER_ID = {0}",
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                bool flagDismiss;
                if (((ListEmp)this.ActiveMdiChild).dgTransfer.Rows[((ListEmp)this.ActiveMdiChild).dgTransfer.CurrentRow.Index].Cells["type_transfer_name"].Value.ToString().ToUpper() == "УВОЛЬНЕНИЕ")
                { flagDismiss = true; }
                else
                { flagDismiss = false; }
                Transfer formtransfer = new Transfer(record_emp, transfer, transfer, account, account,
                    flagDismiss, false, false, ((ListEmp)this.ActiveMdiChild));
                formtransfer.Text = "Переводы";
                formtransfer.ShowDialog();
            }
        }

        /// <summary>
        /// Событие нажатия кнопки увольнения сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDismiss_Click(object sender, EventArgs e)
        {
            /// Табельный номер работника
            string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
            /// Создаем таблицу сотрудник и заполняем ее по табельному номеру
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Создаем таблицы перевода и бухгалтерских данных
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            /// Заполняем переводы текущей должностью
            //transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1", EMP_seq.ColumnsName.PER_NUM, per_num,
            //    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 0",
                TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                TRANSFER_seq.ColumnsName.SIGN_COMB));
            /// Если сотрудник работает по основному месту
            if (transfer.Count != 0)
            {
                //transfer.Clear();
                //transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1",
                //    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                //    TRANSFER_seq.ColumnsName.SIGN_COMB));
                //if (transfer.Count == 0)
                //{
                //    transfer.Fill(string.Format("where {0} = {1} and {2} = 1 ", TRANSFER_seq.ColumnsName.PER_NUM,
                //        per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                //}
                TRANSFER_obj newTransfer =
                    (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                newTransfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                newTransfer.FROM_POSITION = newTransfer.TRANSFER_ID;
                newTransfer.CONTR_EMP = "";
                newTransfer.DATE_CONTR = null;
                newTransfer.DATE_TRANSFER = null;
                newTransfer.DATE_END_CONTR = null;
                newTransfer.TR_NUM_ORDER = "";
                newTransfer.TR_DATE_ORDER = null;
                newTransfer.TYPE_TRANSFER_ID = 3;
                transfer.Clear();
                transfer.AddObject(newTransfer);
                Transfer.flagAdd = true;
            }
            else
            {
                /// Старое
                //transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                //    "(select max({3}) from {4}.transfer where {0} = {1})",
                //    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                //    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema));
                /// Новое
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {5} = 0 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1})",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema, TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    MessageBox.Show("По работнику нет данных об основном месте работы.\nВозможно он совместитель.", 
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ///
                Transfer.flagAdd = false;
            }
            Transfer formtransfer = new Transfer(record_emp, transfer, transfer, account, account,
                true, true, true, (ListEmp)this.ActiveMdiChild);
            formtransfer.Text = "Увольнение";
            formtransfer.ShowDialog();
            RefreshGrid((ListEmp)this.ActiveMdiChild);
        }

        /// <summary>
        /// Увольнение в стороннию организацию на территории завода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDismissToFR_Click(object sender, EventArgs e)
        {
            /// Табельный номер работника
            string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
            /// Создаем таблицу сотрудник и заполняем ее по табельному номеру
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Создаем таблицы перевода и бухгалтерских данных
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            ACCOUNT_DATA_seq account = new ACCOUNT_DATA_seq(Connect.CurConnect);
            /// Заполняем переводы текущей должностью
            transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1", EMP_seq.ColumnsName.PER_NUM, per_num,
                TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            /// Проверяем работает ли сотрудник
            if (transfer.Count != 0)
            {
                transfer.Clear();
                transfer.Fill(string.Format("where {0} = {1} and {2} = 1 and {3} = 1",
                        TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK,
                        TRANSFER_seq.ColumnsName.SIGN_COMB));
                if (transfer.Count == 0)
                {
                    transfer.Fill(string.Format("where {0} = {1} and {2} = 1 ", TRANSFER_seq.ColumnsName.PER_NUM,
                        per_num, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                }
                TRANSFER_obj newTransfer =
                    (TRANSFER_obj)((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).Clone();
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
                newTransfer.DATE_HIRE = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE;
                newTransfer.FROM_POSITION = newTransfer.TRANSFER_ID;
                newTransfer.CONTR_EMP = "";
                newTransfer.DATE_CONTR = null;
                newTransfer.DATE_TRANSFER = null;
                newTransfer.DATE_END_CONTR = null;
                newTransfer.TR_NUM_ORDER = "";
                newTransfer.TR_DATE_ORDER = null;
                newTransfer.TYPE_TRANSFER_ID = 3;
                newTransfer.SIGN_CUR_WORK = false;
                newTransfer.DF_BOOK_ORDER = null;
                transfer.Clear();
                transfer.AddObject(newTransfer);
            }
            else
            {
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1} and {2} = 3)",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_HIRE, Connect.Schema));
            }
            TransferToFR formtransfer = new TransferToFR(record_emp, transfer, transfer, account, account,
                true, true, true, ((ListEmp)this.ActiveMdiChild));
            formtransfer.Text = "Увольнение";
            formtransfer.ShowDialog();
            RefreshGrid((ListEmp)this.ActiveMdiChild);
        }

        /// <summary>
        /// Событие нажатия кнопки восстановления работника в должности.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRecoveryTransfer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите восстановить работника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
                /// Последнее увольнение работника.                
                TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
                transfer.Fill(string.Format("where {0} = {1} and {2} = 3 and {3} = " +
                    "(select max({3}) from {4}.transfer where {0} = {1} and {2} = 3)",
                    TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    TRANSFER_seq.ColumnsName.DATE_TRANSFER, Connect.Schema));
                /// Если запись существует, восстанавливаем работника.
                if (transfer.Count != 0)
                {
                    /// Предыдущий перевод. Заполняется по полю FROM_POSITION текущего перевода.
                    TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
                    transferPrev.Fill(string.Format(" where {0} = {1}",
                        TRANSFER_seq.ColumnsName.TRANSFER_ID,
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FROM_POSITION));
                    /// Ставим признак текущей работы предыдущему переводу.
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current).SIGN_CUR_WORK = true;
                    TRANSFER_obj r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current;
                    /// Удаляем текущей перевод. Сохраняем данные.
                    transfer.Remove((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current);
                    /// Данные по сотруднику.
                    EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                    record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
                    /// Проверяем буфер.
                    BUFFER_EMP_seq buffer_emp = new BUFFER_EMP_seq(Connect.CurConnect);
                    buffer_emp.Fill(string.Format("where {0} = {1} and {2} = 3", BUFFER_EMP_seq.ColumnsName.PER_NUM, per_num,
                        BUFFER_EMP_seq.ColumnsName.BUFFER_TYPE));
                    /// Если буфер по этому человеку содержит данные работаем с ними.
                    if (buffer_emp.Count != 0)
                    {
                        /// Удаляем человека из буфера, чтобы он не попал в Перко как уволенный.
                        buffer_emp.Remove((BUFFER_EMP_obj)((CurrencyManager)BindingContext[buffer_emp]).Current);
                        transfer.Save();
                        transferPrev.Save();
                        buffer_emp.Save();
                        Connect.Commit();
                    }
                    /// Если буфер пустой, работаем с Перко.
                    else
                    {
                        /// Восстановление данных в Перко.
                        if (FormMain.employees.RecoveryEmployee(
                            ((EMP_obj)((CurrencyManager)BindingContext[record_emp]).Current).PERCO_SYNC_ID.ToString(), per_num))
                        {
                            transfer.Save();
                            transferPrev.Save();
                            buffer_emp.Save();
                            Connect.Commit();
                        }
                    }
                    RefreshGrid((ListEmp)this.ActiveMdiChild);
                }
                /// Если человек не был уволен с завода.
                else
                {
                    MessageBox.Show("У работника нет увольнеиня.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        /// <summary>
        /// Формирование обратного перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReverseTransfer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите перевести работника на предыдущее место работы?", "АСУ \"Кадры\"", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime _date_Transfer = DateTime.Today;
                if (InputDataForm.ShowForm(ref _date_Transfer, "dd.MM.yyyy") == System.Windows.Forms.DialogResult.OK)
                {
                    decimal _cur_worker_id =
                        Convert.ToDecimal(((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["WORKER_ID"].Value);
                    OracleCommand _ocTransfer_Reverse = new OracleCommand(string.Format(
                        @"BEGIN 
                            {0}.TRANSFER_REVERSE(:p_WORKER_ID, :p_DATE_TRANSFER);
                        END;", Connect.Schema), Connect.CurConnect);
                    _ocTransfer_Reverse.BindByName = true;
                    _ocTransfer_Reverse.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = _cur_worker_id;
                    _ocTransfer_Reverse.Parameters.Add("p_DATE_TRANSFER", OracleDbType.Date).Value = _date_Transfer;
                    OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                    try
                    {
                        _ocTransfer_Reverse.Transaction = transact;
                        _ocTransfer_Reverse.ExecuteNonQuery();
                        transact.Commit();
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        MessageBox.Show(ex.Message, "АСУ \"Кадры\" - Ошибка сохранения перевода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Формирование бланка доп.соглашения по выбранному сотруднику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_Agreement_For_Emp_Click(object sender, EventArgs e)
        {
            OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectAdd_Agreement_For_Emp.sql"),
                    Connect.Schema), Connect.CurConnect);
            _daReport.SelectCommand.BindByName = true;
            _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["TRANSFER_ID"].Value;
            DataSet _ds = new DataSet();
            _daReport.Fill(_ds, "Table1");
            if (_ds.Tables["Table1"].Rows.Count > 0)
            {
                //ReportViewerWindow _rep = new ReportViewerWindow("Дополнительное соглашение", "Reports/Add_Agreement_For_Emp.rdlc",
                //    _ds, null, true);
                //_rep.Show();
                ReportViewerWindow.RenderToExcel(this, "Add_Agreement_For_Emp.rdlc", _ds.Tables["Table1"], null);
            }
            else
            {
                MessageBox.Show("Нет данных!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Просмотр увольнений по совмещению работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDismissComb_Click(object sender, EventArgs e)
        {
            /// Табельный номер.
            string per_num = ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["per_num"].Value.ToString();
            /// Данные сотрудника.
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.Fill(string.Format(" where {0} = '{1}'", EMP_seq.ColumnsName.PER_NUM, per_num));
            /// Текущая запись в переводах по совмещению.
            TRANSFER_seq transfer = new TRANSFER_seq(Connect.CurConnect);
            //transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1 and {3} = 3", TRANSFER_seq.ColumnsName.PER_NUM,
            //    per_num, TRANSFER_seq.ColumnsName.SIGN_COMB, TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID));
            transfer.Fill(string.Format("where {0} = '{1}' and {2} = 1 and ({3} = 3 or ({3} != 3 and {4} = 1))",
                TRANSFER_seq.ColumnsName.PER_NUM, per_num, TRANSFER_seq.ColumnsName.SIGN_COMB,
                TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
            /// Если записей по совмещению нет, то выводим сообщение об ошибке
            if (transfer.Count == 0)
            {
                MessageBox.Show("Данных по увольнению с совмещения работника нет!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ListCombDismiss listCombDismiss = new ListCombDismiss(record_emp, ((ListEmp)this.ActiveMdiChild));
            listCombDismiss.ShowDialog();
            RefreshGrid((ListEmp)this.ActiveMdiChild);
        }

        /// <summary>
        /// Проект приказа об увольнении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProject_Order_Dismiss_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null && this.ActiveMdiChild.Name.ToUpper() == "LISTEMP")
            {
                Order_Dismiss_Project order_Dismiss_Project = new Order_Dismiss_Project(
                    ((ListEmp)this.ActiveMdiChild).dgEmp.Rows[((ListEmp)this.ActiveMdiChild).bsEmp.Position].Cells["TRANSFER_ID"].Value);
                order_Dismiss_Project.ShowDialog();
            }
            else
            {
                MessageBox.Show("Необходимо активировать список основной базы!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    #endregion

    #region Работа со сторонними сотрудниками

        /// <summary>
        /// Просмотр сторонних сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPrevFR_Emp_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTFR_EMP").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTFR_EMP").First().Activate();
                return;
            }
            rgFilterFR_Emp.Visible = true;
            btAddFR_Emp.Enabled = true;

            /// Создаем строку условия для заполнения таблицы работающих сторонних сотрудников.
            string whereFR_Emp = "where FR_DATE_END is null";
            /// Создаем строку условия для заполнения таблицы работающих сторонних сотрудников.
            string whereFR_EmpDismiss = "where FR_DATE_END is not null";
            /// Создаем строку сортировки для заполнения таблицы сторонних сотрудников.
            string orderFR_Emp = "order by FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";

            /// Создаем строку запроса работающих сторонних сотрудников и таблицу.
            textQueryFR = string.Format(Queries.GetQuery("SelectListFR_Emp.sql"),
                Connect.Schema, whereFR_Emp, orderFR_Emp);
            OracleDataTable dtFR_Emp = new OracleDataTable(textQueryFR, Connect.CurConnect);

            /// Создаем строку запроса работающих сторонних сотрудников и таблицу.
            textQueryFRDismiss = string.Format(Queries.GetQuery("SelectListFR_Emp.sql"),
                Connect.Schema, whereFR_EmpDismiss, orderFR_Emp);
            OracleDataTable dtFR_EmpDismiss = new OracleDataTable(textQueryFRDismiss, Connect.CurConnect);

            /// Создаем форму списка сторонних работников завода.
            listFR_Emp = new FR_Emp(this, dtFR_Emp, dtFR_EmpDismiss, textQueryFR);
            listFR_Emp.Name = "LISTFR_EMP";
            listFR_Emp.MdiParent = this;
            listFR_Emp.Text = "Список сторонних работников завода";
            listFR_Emp.WindowState = FormWindowState.Maximized;
            listFR_Emp.Show();
            rgFilterFR_Emp.Visible = true;
        }

        /// <summary>
        /// Редактирование данных стороннего сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btEditFR_Emp_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren[0].Name.ToUpper() == "fr_Emp".ToUpper())
            {
                object newForm = this.MdiChildren[0];
                this.MdiChildren[0].GetType().InvokeMember("btEditFamily2_Click",
                    BindingFlags.Default | BindingFlags.SetProperty,
                    null, newForm, new object[] { sender, e });
            }
        }

        /// <summary>
        /// Фильтр сторонних сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilterFR_Emp_Click(object sender, EventArgs e)
        {
            /// Создаем форму фильтра.
            FR_EmpFilter filter_emp = new FR_EmpFilter();
            /// Выводим на экран.
            DialogResult rezFilter = filter_emp.ShowDialog();
            /// Если нажали кнопки Фильтра или Отмены фильтра, выполняем действия.
            if (rezFilter == DialogResult.OK || rezFilter == DialogResult.Abort)
            {
                /// Формируем строку условия.
                string strFilter = rezFilter == DialogResult.OK ? " where " + filter_emp.str_filter.ToString() : "";
                /// Формируем строку сортировки.
                string strOrder = " order by FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";
                /// Формируем строку запроса.
                textQueryFR = string.Format(Queries.GetQuery("SelectListFR_Emp.sql"),
                    Connect.Schema, strFilter, strOrder);
                /// Создаем и заполняем таблицу.
                OracleDataTable newDataEmp = new OracleDataTable(textQueryFR, Connect.CurConnect);
                newDataEmp.Fill();
                listFR_Emp.dgFR_Emp.DataSource = null;
                listFR_Emp.dtFR_Emp = newDataEmp;
                listFR_Emp.bsFR_Emp.DataSource = listFR_Emp.dtFR_Emp;
                listFR_Emp.dgFR_Emp.DataSource = listFR_Emp.bsFR_Emp;
                listFR_Emp.RefreshDataGrid(listFR_Emp.dgFR_Emp);
                listFR_Emp.bsFR_Emp.Position = 0;
            }
        }

        /// <summary>
        /// Поиск сторонних сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFindFR_Emp_Click(object sender, EventArgs e)
        {
            /// Создаем форму поиска.
            FR_EmpFind find_emp = new FR_EmpFind();
            /// Если получен положительный результат, то продолжаем работу.
            if (find_emp.ShowDialog() == DialogResult.OK)
            {
                /// Запоминаем нужную нам позицию - первая в списке.
                string perco_sync_id = find_emp.OracleDataTable.Rows[0][0].ToString();
                /// Ищем позицию начала сортировки.
                int posOrder = textQueryFR.IndexOf(" order by ");
                /// Обновляем строку запроса новой сортировкой, согласно заполненным полям поиска.
                textQueryFR = textQueryFR.Substring(0, posOrder) + find_emp.sort.ToString();
                /// Очищаем таблицу, заполняем строку запроса и заполняем таблицу отсортированными данными.
                listFR_Emp.dtFR_Emp.Clear();
                listFR_Emp.dtFR_Emp.SelectCommand.CommandText = textQueryFR;
                listFR_Emp.dtFR_Emp.Fill();
                /// Ищем нужную нам позицию в новом списке.
                for (int i = 0; i < listFR_Emp.dgFR_Emp.Rows.Count; i++)
                {
                    if (listFR_Emp.dgFR_Emp["perco_sync_id", i].Value.ToString() == perco_sync_id)
                    {
                        listFR_Emp.bsFR_Emp.Position = listFR_Emp.dgFR_Emp["perco_sync_id", i].RowIndex;
                        break;
                    }
                }
                listFR_Emp.dgFR_Emp.Focus();
            }
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименованию подразделения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivSort_Click(object sender, EventArgs e)
        {
            string sort = "order by SUBDIV_NAME";
            SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по ФИО.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFIOSort_Click(object sender, EventArgs e)
        {
            string sort = "order by FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";
            SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименованию подразделения и ФИО.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivFIOSort_Click(object sender, EventArgs e)
        {
            string sort = "order by SUBDIV_NAME, FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";
            SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименовании должности.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPositionSort_Click(object sender, EventArgs e)
        {
            string sort = "order by POS_NAME";
            SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по дате начала работы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDate_StartSort_Click(object sender, EventArgs e)
        {
            string sort = "order by FR_DATE_START";
            SorterFR_Emp(sort);
        }

        /// <summary>
        /// Переход на первую строку в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUpFR_Emp_Click(object sender, EventArgs e)
        {
            listFR_Emp.bsFR_Emp.Position = 0;
        }

        /// <summary>
        /// Переход на последнюю строку в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDownFR_Emp_Click(object sender, EventArgs e)
        {
            listFR_Emp.bsFR_Emp.Position = listFR_Emp.bsFR_Emp.Count;
        }

        /// <summary>
        /// Очистка буфера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReleaseBuffer_Click(object sender, EventArgs e)
        {
            FormMain.employees.ReleaseBuffer();
            Connect.Commit();
        }

    #endregion                
        
    #region Работа с нарушителями и привилегиями

        private void btSubdivSorter_nar_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}", "subdiv_name");
            Sorter_nar(sort);
        }

        private void btPer_NumSorter_nar_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}", "per_num");
            Sorter_nar(sort);
        }

        private void btSubdivPer_NumSorter_nar_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}", "subdiv_name", "per_num");
            Sorter_nar(sort);
        }

        private void btFIOSorter_nar_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}",
               "LAST_NAME",
               "FIRST_NAME", "MIDDLE_NAME");
            Sorter_nar(sort);
        }

        private void btSubdivFIOSorter_nar_Click(object sender, EventArgs e)
        {
            string sort = string.Format("order by {0}, {1}, {2}, {3}",
                "subdiv_name", "LAST_NAME",
               "FIRST_NAME", "MIDDLE_NAME");
            Sorter_nar(sort);
        }

        /// <summary>
        /// Метод меняет сортировку списка нарушителей и перезаполняет таблицу
        /// </summary>
        /// <param name="sort">Строка сортировки</param>
        void Sorter_nar(string sort)
        {
            string tnom = jurnal.dgvGurnal.Rows[jurnal.bsEmp.Position].Cells["per_num"].Value.ToString();
            int pos = jurnal.dtEmp.SelectCommand.CommandText.IndexOf("order by");
            string strSelect = jurnal.dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
            jurnal.bsEmp.PositionChanged -= jurnal.PositionChange;
            jurnal.dtEmp.Clear();
            jurnal.dtEmp.SelectCommand.CommandText = strSelect;
            jurnal.dtEmp.Fill();
            jurnal.bsEmp.PositionChanged += new EventHandler(jurnal.PositionChange);
            for (int i = 0; i < jurnal.dgvGurnal.Rows.Count; i++)
            {
                if (jurnal.dgvGurnal["per_num", i].Value.ToString() == tnom)
                {
                    jurnal.bsEmp.Position = jurnal.dgvGurnal["per_num", i].RowIndex;
                    break;
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки добавления нарушителя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btAdd_narush_Click(object sender, EventArgs e)
        {
            jurnal.btAdd_Click(sender, e);
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btInsert_narush_Click(object sender, EventArgs e)
        {
            jurnal.btInsert_Click(sender, e);
        }

        /// <summary>
        /// Событие нажатия кнопки удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btDel_narush_Click(object sender, EventArgs e)
        {
            jurnal.btDelete_Click(sender, e);
        }
        
        private void btPrint_narush_Click(object sender, EventArgs e)
        {
            PrintJurnal pj = new PrintJurnal();
            pj.ShowDialog();
        }

        private void btPrint_podr_Click(object sender, EventArgs e)
        {
            PrintNarush pn = new PrintNarush();
            pn.ShowDialog();
        }

        /// <summary>
        /// Вывод списка нарушителей на экран
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btNaruRe_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "ZURNNARURE").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "ZURNNARURE").First().Activate();
                return;
            }
            emp = new EMP_seq(Connect.CurConnect);
            textBlockNaru = string.Format(Queries.GetQuery("SelectJurnal.sql"), Connect.Schema, "");
            OracleDataTable dtEmp = new OracleDataTable(textBlockNaru, Connect.CurConnect);
            dtEmp.Fill();
            jurnal = new ZurnNaruRe(dtEmp, emp);
            jurnal.MdiParent = this;
            jurnal.Show();
            rgFilterNaru.Visible = true;
            rgNaruRe.EnableByRules();
        }

        /// <summary>
        /// Вывод формы фильтра для нарушителей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFiltr_Click(object sender, EventArgs e)
        {
            JurnalFiltr filter = new JurnalFiltr(textBlockNaru);
            DialogResult rezFilter = filter.ShowDialog(this);
            if (rezFilter == DialogResult.OK || rezFilter == DialogResult.Abort)
            {
                string strTemp = rezFilter == DialogResult.OK ?
                    " where " + filter.str_filter.ToString() : "";
                if (FormMain.flagArchive)
                {
                    textQuery = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                        Connect.Schema, strTemp);
                }
                else
                {
                    textQuery = string.Format(Queries.GetQuery("SelectJurnal.sql"),
                        Connect.Schema, strTemp);
                }
                OracleDataTable newDataEmp = new OracleDataTable(textQuery, Connect.CurConnect);
                newDataEmp.Fill();
                jurnal.dgvGurnal.DataSource = null;
                jurnal.dtEmp = newDataEmp;
                jurnal.bsEmp.PositionChanged -= jurnal.PositionChange;
                jurnal.bsEmp.DataSource = jurnal.dtEmp;
                jurnal.dgvGurnal.DataSource = jurnal.dtEmp;
                jurnal.RefreshGridNarush();
                jurnal.bsEmp.PositionChanged += new EventHandler(jurnal.PositionChange);
                jurnal.bsEmp.Position = 0;
            }
            jurnal.PositionChange(jurnal.bsEmp, null);    
        }

        /// <summary>
        /// Вывод формы поиска нарушителей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFindNaru_Click(object sender, EventArgs e)
        {
            FindJurnal find = new FindJurnal(textBlockNaru);
            find.ShowInTaskbar = false;
            if (find.ShowDialog() == DialogResult.OK)
            {
                if (find.OracleDataTable.Rows.Count != 0)
                {
                    int pos = jurnal.dtEmp.SelectCommand.CommandText.IndexOf("order by");
                    string strSelect = jurnal.dtEmp.SelectCommand.CommandText.Substring(0, pos) + find.sort;
                    jurnal.dtEmp.Clear();
                    jurnal.dtEmp.SelectCommand.CommandText = strSelect;
                    jurnal.dtEmp.Fill();
                    string tnom = find.OracleDataTable.Rows[0][0].ToString();
                    for (int i = 0; i < jurnal.dgvGurnal.Rows.Count; i++)
                    {
                        if (jurnal.dgvGurnal["per_num", i].Value.ToString() == tnom)
                        {
                            jurnal.bsEmp.Position = jurnal.dgvGurnal["per_num", i].RowIndex;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Данные найдены в базе данных работников.\nНо заданные критерии фильтрации не позволяют\nпоместить курсор на нужную позицию.\nИзмените критерии фильтра и попробуйте еще раз.", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Привилегии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPermit_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTPERMIT").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTPERMIT").First().Activate();
                return;
            }
            FilterForPermit filterForPermit = new FilterForPermit();
            if (filterForPermit.ShowDialog() == DialogResult.OK)
            {
                listPermit = new ListPermit(filterForPermit.strFilter);
                listPermit.MdiParent = this;
                listPermit.Name = "listPermit";
                listPermit.Text = "Учет жетонов, вкладышей и пропусков";
                btEditFilterPermit.Enabled = true;
                listPermit.Show();
            }
        }

        /// <summary>
        /// Типы привилегий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btType_Permit_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "HBTYPE_PERMIT").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "HBTYPE_PERMIT").First().Activate();
                return;
            }
            HBType_Permit hbType_Permit = new HBType_Permit(this);
            hbType_Permit.MdiParent = this;
            hbType_Permit.Show();
        }

        /// <summary>
        /// Фильтр сотрудников (окно привилегий)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditFilterPermit_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTPERMIT").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "LISTPERMIT").First().Activate();
            }
            else
            {
                return;
            }
            FilterForPermit filterForPermit = new FilterForPermit();
            if (filterForPermit.ShowDialog() == DialogResult.OK)
            {
                listPermit.LoadPermit(filterForPermit.strFilter);
            }
        }

        /// <summary>
        /// Список нарушителей по новой версии программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btViewViolators_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "VIOLATIONS").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "VIOLATIONS").First().Activate();
                return;
            }
            Wpf_Control_Viewer violations = new Wpf_Control_Viewer();
            violations.Name = "VIOLATIONS";
            violations.Text = "Нарушители режима";
            List_Violations_Editor list_Violations = new List_Violations_Editor();
            violations.elementHost1.Child = list_Violations;
            violations.MdiParent = this;
            violations.WindowState = FormWindowState.Maximized;
            violations.Show();
        }

        /// <summary>
        /// Список уволенных сотрудников для обработки в Бюро пропусков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDismissed_Emp_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "DISMISSED_EMP").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "DISMISSED_EMP").First().Activate();
                return;
            }
            Wpf_Control_Viewer _dis = new Wpf_Control_Viewer();
            _dis.Name = "DISMISSED_EMP";
            _dis.Text = "Список уволенных";
            Dismissed_Emp_View _dismissed_Emp_View = new Dismissed_Emp_View(3);
            _dis.elementHost1.Child = _dismissed_Emp_View;
            _dis.MdiParent = this;
            _dis.WindowState = FormWindowState.Maximized;
            _dis.Show();
        }

        /// <summary>
        /// Список уволенных сотрудников для обработки в Бюро пропусков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransfer_Emp_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_EMP").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_EMP").First().Activate();
                return;
            }
            Wpf_Control_Viewer _dis = new Wpf_Control_Viewer();
            _dis.Name = "TRANSFER_EMP";
            _dis.Text = "Список переведенных";
            Dismissed_Emp_View _dismissed_Emp_View = new Dismissed_Emp_View(2);
            _dis.elementHost1.Child = _dismissed_Emp_View;
            _dis.MdiParent = this;
            _dis.WindowState = FormWindowState.Maximized;
            _dis.Show();
        }

    #endregion

    #region Работа с таймерами

        /// <summary>
        /// Переменная необходима для смены цвета сообщения об обновлении программы
        /// </summary>
        bool flagColor = true;
        /// <summary>
        /// Форма для вывода сообщения о потере соединения
        /// </summary>
        ReopenConnect reopenConnect;
        /// <summary>
        /// Поток, в котором пытаемся заново открыть соединение
        /// </summary>
        Thread thrReopen;
        /// <summary>
        /// Поток, в котором запускаем очистку буфера
        /// </summary>
        Thread thrReleaseBuffer;

        /// <summary>
        /// Событие изменения цвета сообщения об обновлении программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerForColor_Tick(object sender, EventArgs e)
        {
            if (flagColor)
            {
                lbColor.BackColor = Color.Gold;
                flagColor = false;
            }
            else
            {
                lbColor.BackColor = Color.Orange;
                flagColor = true;
            }
        }

        /// <summary>
        /// Событие проверки соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerForPing_Tick(object sender, EventArgs e)
        {
            DateTime? dt;//Проверяем, заблокировано ли приложение? Тогда вызываем форму завершения программы с таймером в 1 минуту
            if ((dt = Connect.BlockApp).HasValue && Connect.UserId.ToUpper() != "BMW12714")
            {
                AppCloseForm f = new AppCloseForm(dt.Value);
                f.Owner = this;
                f.Show();
                TimerForPing.Stop();
            }
            /// Если соединение отсутствует
            if (!Connect.Ping())
            {
                /// Останавливаем таймер проверки соединения
                TimerForPing.Stop();
                reopenConnect = new ReopenConnect(this);
                reopenConnect.lbMessage.Text = "Возникли проблемы соединения с базой данных!" +
                    "\nНеобходимо подождать пока соединение будет восстановлено.";
                reopenConnect.StartPosition = FormStartPosition.CenterScreen;
                //message.StartPosition = FormStartPosition.Manual;
                //message.Location = new Point(this.Location.X, this.Height - message.Height);
                thrReopen = new Thread(new ParameterizedThreadStart(ReopenConnection));
                thrReopen.Start();
                if (reopenConnect.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    thrReopen.Abort();
                    TimerForPing.Start();
                }
                else
                {
                    thrReopen.Abort();
                    /*MessageBox.Show("Невозможно продолжать работу из-за блокировки пользователя!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Проверка актуальности программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerForUpdate_Tick(object sender, EventArgs e)
        {
            /// Создаем дату создания файла программы, который находится сейчас в сети
            DateTime date = File.GetLastWriteTime(executablePath);
            /// Если хэш-коды запущенной программы и файла в сети не равны
            if (hashCode != date.GetHashCode())
            {
                //message = new MessageAboutRestart();
                //message.StartPosition = FormStartPosition.Manual;
                //message.Location = new Point(this.Location.X, this.Height - message.Height);
                //TimerForUpdate.Stop();
                ////message.ShowDialog(this);
                //TimerForUpdate.Start();
                /// Показываем панель о необходимости перезапустить программу
                /// и запускаем таймер смены цвета панели
                lbColor.Text = "В программе были сделаны изменения. Пожалуйста перезапустите программу.";
                pnUpdate.Height = 22;
                pnUpdate.Visible = true;
                TimerForColor.Start();
            }
            else
            {
                /// Скрываем панель и останавливаем таймер
                pnUpdate.Visible = false;
                TimerForColor.Stop();
            }
        }

        /// <summary>
        /// Запуск очистки буфера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerForPerco_Tick(object sender, EventArgs e)
        {
            thrReleaseBuffer = new Thread(new ParameterizedThreadStart(RunReleaseBuffer));
            //TimerForPerco.Stop();
            thrReleaseBuffer.Start();
        }
        /// <summary>
        /// Пытаемся заново открыть соединение при потере связи с сервером
        /// </summary>
        /// <param name="data"></param>
        void ReopenConnection(object data)
        {
            /// Закрываем соединение, иначе оно находится в состоянии открыто при потере связи
            Connect.CurConnect.Close();
            bool F = false;
            /// Выполняем цикл до тех пор, пока соединение не будет открыто
            while (F == false)
                try
                {
                    /// Пытаемся открыть соединение
                    Connect.CurConnect.Open();
                    F = true;
                    /// При открытии соединения закрываем форму вывода сообщения о потере соединения
                    reopenConnect.DialogResult = DialogResult.OK;
                }
                catch(OracleException ex)
                {
                    // Обрабатываем ситуацию, что во время потери соединения, пользователь был заблокирован
                    if (ex.Number != 28000)
                        F = false;
                    else
                    {
                        F = true;
                        /// При блокировке пользователя, закрываем программу
                        reopenConnect.DialogResult = DialogResult.Cancel;
                    }
                }
            /// При открытии соединения закрываем форму вывода сообщения о потере соединения
            //reopenConnect.DialogResult = DialogResult.OK;
        }
           
        /// <summary>
        /// Очистка буфера
        /// </summary>
        /// <param name="data"></param>
        void RunReleaseBuffer(object data)
        {
            FormMain.employees.ReleaseBuffer();
            //TimerForPerco.Start();
            //t.Abort();
        }

    #endregion

    #region выход, Помощь

        /// <summary>
        /// Событие нажатия кнопки закрытия приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExitApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Процедура вывова помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHelp_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = null;
                object fileName = Application.StartupPath + @"\help.doc";
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;
                doc = word.Documents.Open(ref fileName, ref missing, ref trueValue,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                word.Visible = true;
            }
            finally
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }     
                
#endregion

    #region Работа с табелем

        /// <summary>
        /// Работа с календарем рабочего времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCalendar_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "calendar").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name == "calendar").First().Activate();
                return;
            }  
            /// Создаем форму календаря, задаем родителя и отображаем ее на экране
            Tabel.Calendar calendar = new Tabel.Calendar();
            calendar.Name = "calendar";
            calendar.MdiParent = this;
            CreateButtonApp(calendar, sender);            
            calendar.Show();
        }

        /// <summary>
        /// Создание кнопки меню ленты, связанное с дочерним окном главной формы
        /// </summary>
        /// <param name="_form"></param>
        /// <param name="_sender"></param>
        public void CreateButtonApp(Form _form, object _sender)
        {
            Elegant.Ui.Button buttonApp = new Elegant.Ui.Button() {
                Text = /*((Elegant.Ui.Button)_sender).Text*/ _form.Text,
                Name = ((Elegant.Ui.Button)_sender).Name,
                TextImageRelation = TextImageRelation.Overlay,
                DefaultLargeImage = ((Elegant.Ui.Button)_sender).DefaultLargeImage,
                Tag = _form
            };
            buttonApp.Click += new EventHandler(ButtonAppClick);
            applicationMenu1.Items.Add(buttonApp);
            _form.FormClosing += new FormClosingEventHandler(ClosingChildForm);
            _form.Tag = buttonApp;
        }

        /// <summary>
        /// Событие нажатия кнопки меню ленты, которое активирует связанное окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonAppClick(object sender, EventArgs e)
        {
            try
            {
                this.MdiChildren.Where(i => i == ((Elegant.Ui.Button)sender).Tag).First().Activate();
            }
            catch { }
        }

        /// <summary>
        /// Событие закрытия формы, при котором удаляется кнопка из меню ленты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClosingChildForm(object sender, FormClosingEventArgs e)
        {
            try
            {
                applicationMenu1.Items.Remove((Elegant.Ui.Button)((Form)sender).Tag);
            }
            catch { }
        }

        /// <summary>
        /// Отображение формы табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTable_Click(object sender, EventArgs e)
        {
            /// Переменная определяет если ли активные формы табеля
            bool flagActiveTable = false;
            /// Определяет создана ли уже форма работы с табелем
            for (int i = 0; i < this.MdiChildren.Length; i++)
            {
                /// Если это форма табеля
                if (this.MdiChildren[i].Name.ToUpper() == "table".ToUpper())
                {
                    /// Активируем эту форму
                    this.MdiChildren[i].Activate();
                    flagActiveTable = true;
                }
            }
            /// Если на экране показаны формы табеля, то заходим
            if (flagActiveTable)
            {
                DialogResult drQuestion = MessageBox.Show("Открыть табель в новом окне?" +
                    "\n(Да - откроется новое окно табеля)" +
                    "\n(Нет - будет использоваться текущее окно табеля)",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (drQuestion == DialogResult.No)
                {
                    ((Table)this.ActiveMdiChild).Close();
                    /// Скрываем отображение элементов                
                    rgOperation.Visible = false;
                    rgOrders.Visible = false;
                }
                else
                    if (drQuestion == DialogResult.Cancel)
                    {
                        return;
                    }
            }
            /// Проверяем на сколько подразделений дан доступ пользователю
            Table table;
            if (dsSubdivTable.Tables["SUBDIV_TABLE"].Rows.Count == 1)
            {
                FormMain.flagFilter = true;
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                table = new Table();
                table.subdiv_id =
                    Convert.ToInt32(FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_ID"]);
                table.code_subdiv =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["CODE_SUBDIV"].ToString();
                table.subdiv_name =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_NAME"].ToString();
                table.month = DateTime.Now.Month;
                table.year = DateTime.Now.Year;
                table.MdiParent = this;
                table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                table.Show();
                ClickErrorGraph(false);
                CreateButtonApp(table, sender);
                
            }
            else
            {
                /// Создаем форму фильтра
                Filter filter = new Filter();
                filter.Text = "Фильтр подразделений";
                /// Результат выполнения формы
                DialogResult rezFilter = filter.ShowDialog();
                /// Если результат ОК
                if (rezFilter == DialogResult.OK)
                {
                    flagFilter = filter.flagFilter;                    
                    /// Создаем форму табеля, задаем родителя и показываем ее на экране
                    table = new Table();
                    table.subdiv_id = filter.subdiv_id;
                    table.code_subdiv = filter.code_subdiv;
                    table.subdiv_name = filter.subdiv_name;
                    table.month = filter.month;
                    table.year = filter.year;
                    table.MdiParent = this;
                    table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;                    
                    table.Show();    
                    ClickErrorGraph(false);
                    CreateButtonApp(table, sender);
                }
            }
        }

        /// <summary>
        /// Поиск работников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFindEmp_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!", 
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Создаем форму поиска
            Find_EmpTable find_empTable = new Find_EmpTable(((Table)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText);
            find_empTable.ShowInTaskbar = false;
            /// Если результат ОК
            if (find_empTable.ShowDialog() == DialogResult.OK)
            {
                OracleDataTable dtFind = new OracleDataTable("", Connect.CurConnect);
                /// Ищем позицию вхождения сортировки
                int pos = ((Table)(this.ActiveMdiChild)).dtEmp.SelectCommand.CommandText.LastIndexOf("order by");
                /// Формируем строку запроса, подставляя новую сортировку
                dtFind.SelectCommand.CommandText =
                    ((Table)this.ActiveMdiChild).dtEmp.SelectCommand.CommandText.Substring(0, pos) + find_empTable.str_find + 
                    find_empTable.sort;
                Array arPar = new OracleParameter[((Table)(this.ActiveMdiChild)).dtEmp.SelectCommand.Parameters.Count];
                ((Table)(this.ActiveMdiChild)).dtEmp.SelectCommand.Parameters.CopyTo(arPar, 0);
                dtFind.SelectCommand.Parameters.AddRange(arPar);
                /*foreach (OracleParameter param in ((Table)(this.ActiveMdiChild)).dtEmp.SelectCommand.Parameters)
                {
                    dtFind.SelectCommand.Parameters[param.ParameterName].Direction = param.Direction;
                    dtFind.SelectCommand.Parameters[param.ParameterName].DbType = param.DbType;                    
                    dtFind.SelectCommand.Parameters[param.ParameterName].Value = param.Value;
                }*/
                dtFind.Fill();                
                /// Если данные найдены
                if (dtFind.Rows.Count != 0)
                {
                    
                    /// Формируем строку запроса, подставляя новую сортировку
                    //string strSelect = table.dtEmp.SelectCommand.CommandText.Substring(0, pos) + find_empTable.sort;
                    /// 10,03,2011 Не меняем сортировку в списке сотрудников
                    //string strSelect = table.dtEmp.SelectCommand.CommandText;
                    //table.dgEmp.SelectionChanged -= table.dgEmp_SelectionChanged;
                    //table.dtEmp.Clear();
                    //table.dtEmp.SelectCommand.CommandText = strSelect;
                    //table.dtEmp.Fill();
                    //table.dgEmp.SelectionChanged += table.dgEmp_SelectionChanged;
                    /// Табельный номер первого в списке найденных
                    string tnom = dtFind.Rows[0]["TRANSFER_ID"].ToString();
                    /// Идем по списку работников
                    for (int i = 0; i < ((Table)(this.ActiveMdiChild)).dgEmp.Rows.Count; i++)
                    {
                        /// Если табельный номер совпадает
                        if (((Table)(this.ActiveMdiChild)).dgEmp["TRANSFER_ID", i].Value.ToString() == tnom)
                        {
                            /// Устанавливаем позицию и завершаем работу
                            ((Table)(this.ActiveMdiChild)).bsEmp.Position =
                                ((Table)(this.ActiveMdiChild)).dgEmp["TRANSFER_ID", i].RowIndex;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Данные не найдены.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Обновление данных по отработанному времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRefresh_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос на обновление данных по табелю
            if (MessageBox.Show("Вы действительно хотите обновить табель за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " + 
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {  
                /// Заносим табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RefreshTable));
                /// Запускаем его
                t.Start();
                /// Создаем форму продолжительности работы потока
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                timeExecute.pbPercentExecute.Style = ProgressBarStyle.Marquee;
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RefreshTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
                ((Table)this.ActiveMdiChild).dgEmp.SelectionChanged -= 
                    ((Table)this.ActiveMdiChild).dgEmp_SelectionChanged;
                /// Очистка таблицы списка работников
                ((Table)this.ActiveMdiChild).dtEmp.Clear();
                /// Заполнение таблицы списка работников
                ((Table)this.ActiveMdiChild).dtEmp.Fill();
                /// Создаем биндингсоурс
                ((Table)this.ActiveMdiChild).bsEmp = new BindingSource();
                /// Настраиваем отображение данных
                ((Table)this.ActiveMdiChild).bsEmp.DataSource = ((Table)this.ActiveMdiChild).dtEmp;
                ((Table)this.ActiveMdiChild).dgEmp.SelectionChanged += 
                    new EventHandler(((Table)this.ActiveMdiChild).dgEmp_SelectionChanged);
                ((Table)this.ActiveMdiChild).dgEmp.DataSource = ((Table)this.ActiveMdiChild).bsEmp;
            }
        }

        /// <summary>
        /// Метод, который рассчитывает время для табеля
        /// </summary>
        /// <param name="data"></param>
        void RefreshTable(object sender, DoWorkEventArgs e)
        {
            // Создаем новую команду
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format(
                "begin {0}.Table_Update_New(:p_month, :p_year, :p_user_name, :p_subdiv_id); end;",
                Connect.Schema);
            command.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                ((Table)this.ActiveMdiChild).BeginDate.Month;
            command.Parameters.Add("p_year", OracleDbType.Decimal).Value =
                ((Table)this.ActiveMdiChild).BeginDate.Year;
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            // Выполняем команду
            try
            {
                command.ExecuteNonQuery();
            }
            catch(OracleException ex)
            {
                MessageBox.Show("Ошибка обновления табеля\n" +
                    ex.Message, "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Показать графики работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGr_Work_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "GR_WORK").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "GR_WORK").First().Activate();
                return;
            }
            Gr_Work gr_work = new Gr_Work();
            gr_work.MdiParent = this;
            gr_work.Text = "Графики работы";
            CreateButtonApp(gr_work, sender);
            gr_work.Show();
        }

        /// <summary>
        /// Показать временные зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTime_Zone_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "TIME_ZONE").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "TIME_ZONE").First().Activate();
                return;
            }
            Time_Zone time_zone = new Time_Zone();
            time_zone.MdiParent = this;
            time_zone.Text = "Временные зоны";
            CreateButtonApp(time_zone, sender); 
            time_zone.Show();
        }

   #region Формирование отчетов по табелю (табель, приложение)
           
        /// <summary>
        /// Формирование табеля на аванс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTableForAdvance_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать табель на аванс за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    TableForAdvance(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog(); 
            }
        }

        /// <summary>
        /// Формирование отчета по табелю на аванс
        /// </summary>
        /// <param name="data"></param>
        void TableForAdvance(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            int month = ((Table)this.ActiveMdiChild).EndDate.Month;
            int year = ((Table)this.ActiveMdiChild).EndDate.Year;
            /// Создаем запрос
            command.CommandText = string.Format(
                "begin {0}.TABLEForAdvance(:p_month, :p_year, :p_user_name, :p_subdiv_id, :p1); end;",
                Connect.Schema);
            command.Parameters.Add("p_month", OracleDbType.Int16).Value = month;
            command.Parameters.Add("p_year", OracleDbType.Int16).Value = year;
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            command.Parameters.Add("p1", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            command.Parameters["p1"].DbType = DbType.Decimal;
            ((BackgroundWorker)sender).ReportProgress(10);
            /// Выполняем команду
            command.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(40);
            decimal tempTableID = Convert.ToDecimal(command.Parameters["p1"].Value);
            /// Создаем таблицу для часов и заполняем ее
            OracleDataTable dtHours = new OracleDataTable("", Connect.CurConnect);
            /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
            ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("D1", code_subdiv), 
                new ExcelParameter("F1", string.Format("{0:d2}", month)), 
                new ExcelParameter("H1", string.Format("{0:d4}", year)),
                new ExcelParameter("K1", ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["code_degree"].Value.ToString()),
                new ExcelParameter("Z1", "1")
            };
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                /// Номер текущей строки в отчете куда будет производиться вставка данных
                int rowNow = 5;
                /// Номер листа
                int sheetNumber = 1;
                /// Переменная определяет сколько строк уже заполнено в отчете
                /// Если их количество равно 6, следующий работник будет размещаться на
                /// следующем листе отчета.
                int rowsCount = 1;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Application.StartupPath + @"\Reports\AdvanceA4.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                /// Заполняем отдельные параметры
                if (excelParameters != null)
                    foreach (ExcelParameter parameter in excelParameters)
                    {
                        m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                    }
                /// Объявляем переменные для категорий и ячеек
                string degree1, degree2, cellExcel1, cellExcel2;
                degree1 = degree2 = cellExcel1 = cellExcel2 = "";
                //01.03.2011 - заполнение таблицы выходных дней (числа за выбранный месяц)
                StringBuilder Sel = new StringBuilder();
                //Sel.AppendLine("SELECT Extract(Day from Calendar_Day) FROM APStaff.Calendar ");
                Sel.AppendLine(string.Format("SELECT Extract(Day from Calendar_Day) FROM {0}.Calendar ", Connect.Schema));
                Sel.AppendLine("WHERE Calendar_Day between :D1 and :D2 ");
                Sel.AppendLine("	and Type_Day_ID in (1,4) ");
                Sel.AppendLine("ORDER BY Calendar_Day");
                OracleDataTable dtDayOff = new OracleDataTable(Sel.ToString(), Connect.CurConnect);
                dtDayOff.SelectCommand.Parameters.Add("D1", OracleDbType.Date, ((Table)this.ActiveMdiChild).BeginDate, ParameterDirection.Input);
                dtDayOff.SelectCommand.Parameters.Add("D2", OracleDbType.Date, ((Table)this.ActiveMdiChild).EndDate, ParameterDirection.Input);
                dtDayOff.Fill();
                //заполнение таблицы выходных дней
                //раскраска выходных дней
                m_Book.Styles[1].Interior.Color = 0xDFDFDF;
                m_Book.Styles[1].Font.Size = 8;
                ///// Создаем команду для выбора текущей базы для расчета оклада
                dtHours.SelectCommand.CommandText = string.Format(
                    "select * from {0}.TEMP_TABLE where per_num = :p_per_num and TEMP_TABLE_ID = :P_TEMP_TABLE_ID",
                    Connect.Schema);
                dtHours.SelectCommand.Parameters.Add("P_TEMP_TABLE_ID", OracleDbType.Decimal).Value = tempTableID;
                dtHours.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, "0", ParameterDirection.Input);
                /// Цикл по строкам таблицы
                for (int row = 0; row < ((Table)this.ActiveMdiChild).dgEmp.RowCount; row++)
                {
                    ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)row * 50 / ((Table)this.ActiveMdiChild).dgEmp.RowCount, 0)) + 40);
                    if (((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["sign_comb"].Value.ToString() != "1")
                    {
                        /// Заносим значения
                        m_Sheet.get_Range(string.Format("A{0}", rowNow), Type.Missing).Value2 =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[0].Value;
                        m_Sheet.get_Range(string.Format("B{0}", rowNow), Type.Missing).Value2 =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[5].Value;
                        m_Sheet.get_Range(string.Format("C{0}", rowNow), Type.Missing).Value2 =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[7].Value;
                        m_Sheet.get_Range(string.Format("I{0}", rowNow), Type.Missing).Value2 =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[1].Value;
                        dtHours.Clear();
                        dtHours.SelectCommand.Parameters["p_per_num"].Value =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[0].Value;
                        dtHours.Fill();

                        //27.02.2011 - добавлена проверка на заполнение dtHours
                        if (dtHours.Rows.Count > 0) //Проверка dtHours на заполнение данными
                        {
                            /// Цикл заполняет ячейки часов и итоги за половину месяца
                            for (int col = 0; col <= 15; col++)
                            {
                                /// Заполняем значения в ячейках для текущей строки
                                /// Увеличиваем номер колонки на 3, потому что в таблице часов
                                /// первые 3 поля не хранят часы
                                m_Sheet.get_Range(string.Format("{0}{1}", Excel.ParseColNum(col + 75), rowNow), Type.Missing).Value2 =
                                    dtHours.Rows[0][col + 3];
                            }
                        }

                        /// Если текущая строка + 1 меньше количества строк
                        if (row + 1 < ((Table)this.ActiveMdiChild).dgEmp.RowCount &&
                            !(row + 1 == ((Table)this.ActiveMdiChild).dgEmp.RowCount
                            && ((Table)this.ActiveMdiChild).dgEmp.Rows[((Table)this.ActiveMdiChild).dgEmp.RowCount - 1].Cells["sign_comb"].Value.ToString() != "1"))
                        {
                            /// Если количество заполненных строк меньше 6
                            if (rowsCount < 19)
                            {
                                /// Заносим в переменные значения категорий текущей строки и следующей
                                degree1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[9].Value.ToString();
                                degree2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells[9].Value.ToString();
                                /// Если категории равны копируем только строки для вставки данных по работнику
                                if (degree1 == degree2)
                                {
                                    /// Определяем адреса начальной и конечной ячейки для копирования
                                    cellExcel1 = string.Format("A{0}", rowNow + 1);
                                    cellExcel2 = string.Format("Z{0}", rowNow + 1);
                                    /// Копируем строки по адресам ячеек
                                    m_Sheet.get_Range("A5", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                    /// Устанавливаем высоту строк в выбранных ячейках                              
                                    m_Sheet.get_Range(cellExcel1, Type.Missing).RowHeight = 26;
                                    /// Увеличиваем номер строки в отчете на 4, 
                                    /// чтобы следующий работник вставился на нужные строки
                                    rowNow++;
                                    /// Увеличиваем количество вставленных строк
                                    rowsCount++;
                                }
                                /// Если категории не равны копируем шапку отчета
                                else
                                {
                                    /// Определяем адреса начальной и конечной ячейки для копирования
                                    cellExcel1 = string.Format("A{0}", rowNow + 1);
                                    cellExcel2 = string.Format("Z{0}", rowNow + 5);
                                    /// Копируем строки по адресам ячеек
                                    m_Sheet.get_Range("A1", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                    /// Вставляем разрыв страницы, для того чтобы новая категория была
                                    /// на новом листе отчета
                                    m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(cellExcel1, Type.Missing));
                                    /// Заносим категорию
                                    m_Sheet.get_Range(string.Format("K{0}", rowNow + 1), Type.Missing).Value2 =
                                        degree2;
                                    /// Заносим номер страницы
                                    m_Sheet.get_Range(string.Format("Z{0}", rowNow + 1), Type.Missing).Value2 =
                                        ++sheetNumber;
                                    /// Настраиваем высоту нужных строк
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 2), Type.Missing).RowHeight = 5;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 3), Type.Missing).RowHeight = 15;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 4), Type.Missing).RowHeight = 30;
                                    m_Sheet.get_Range(string.Format("A{0}", rowNow + 5), Type.Missing).RowHeight = 26;
                                    /// Увеличиваем номер строки в отчете на 8, 
                                    /// чтобы следующий работник вставился на нужные строки
                                    rowNow += 5;
                                    /// Сбрасываем количество вставленных строк в отчете,
                                    /// так как новую категорию печатаем на новом листе
                                    rowsCount = 1;
                                }
                            }
                            /// Если количество заполненных строк равно 19
                            else
                            {
                                for (int c = 0; c < dtDayOff.Rows.Count; c++)
                                {
                                    int ColDay = Convert.ToInt32(dtDayOff.Rows[c][0].ToString());
                                    if (ColDay < 16)
                                    {
                                        string ColName1 = string.Format("{0}{1}", Excel.ParseColNum(ColDay + 74), rowNow - 19);
                                        string ColName2 = string.Format("{0}{1}", Excel.ParseColNum(ColDay + 74), rowNow);
                                        m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[1];
                                    }
                                }
                                /// Определяем адреса начальной и конечной ячейки для копирования
                                cellExcel1 = string.Format("A{0}", rowNow + 1);
                                cellExcel2 = string.Format("Z{0}", rowNow + 5);
                                /// Копируем строки по адресам ячеек
                                m_Sheet.get_Range("A1", "Z5").Copy(m_Sheet.get_Range(cellExcel1, cellExcel2));
                                /// Вставляем разрыв страницы, для того чтобы следующий работник
                                /// печатался на новом листе отчета
                                m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(cellExcel1, Type.Missing));
                                /// Заносим номер страницы
                                m_Sheet.get_Range(string.Format("Z{0}", rowNow + 1), Type.Missing).Value2 =
                                    ++sheetNumber;
                                /// Настраиваем высоту нужных строк
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 2), Type.Missing).RowHeight = 5;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 3), Type.Missing).RowHeight = 15;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 4), Type.Missing).RowHeight = 30;
                                m_Sheet.get_Range(string.Format("A{0}", rowNow + 5), Type.Missing).RowHeight = 26;
                                /// Увеличиваем номер строки в отчете на 8, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 5;
                                /// Сбрасываем количество вставленных строк в отчете,
                                /// так как новую категорию печатаем на новом листе
                                rowsCount = 1;
                            }
                        }
                    }
                    else
                    {
                        /* Если вставляли последнюю строку и она была по совместителю, ее надо удалить */
                        if (row == ((Table)this.ActiveMdiChild).dgEmp.RowCount - 1)
                        {
                            m_Sheet.get_Range(cellExcel1, cellExcel2).Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
                        }
                    }
                }
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Visible = true;
                //m_Sheet.PrintPreview(true);
                //m_ExcelApp.Quit();                            
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = {1}",
                Connect.Schema, tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }

        /// <summary>
        /// Нажатие кнопки формирования табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReportTable_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            DialogResult drQuestion = MessageBox.Show("Внимание!\n" + "Сформировать табель за " +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года в разбивке на заказы?\n\n" +
                "(Да - при смене заказа сотрудник будет выводиться на новом листе отчета)\n" +
                "(Нет - при смене заказа сотрудник будет оставаться на текущем листе отчета)\n" +
                "(Отмена - табель за указанный месяц не будет формироваться)",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (drQuestion != DialogResult.Cancel)
            {
                fl_form_title = false;
                fl_form_table = true;
                fl_form_appendix = false;
                // Вставляем табельные номера
                InsertPerNum();
                if (drQuestion == DialogResult.Yes)
                    fl_break_order = true;
                else
                    fl_break_order = false;
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();             
            }
        }       

        /// <summary>
        /// Нажатие кнопки формирования общего табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTableTotal_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Запрос, нужно ли формировать отчет по табелю
            DialogResult drQuestion = MessageBox.Show("Внимание!\n" + "Сформировать табель за " +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " + 
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года в разбивке на заказы?\n\n" + 
                "(Да - при смене заказа сотрудник будет выводиться на новом листе отчета)\n" + 
                "(Нет - при смене заказа сотрудник будет оставаться на текущем листе отчета)\n" + 
                "(Отмена - табель за указанный месяц не будет формироваться)",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);            
            if (drQuestion != DialogResult.Cancel)
            {
                fl_form_title = true;
                fl_form_table = true;
                fl_form_appendix = true;
                if (drQuestion == DialogResult.Yes)
                    fl_break_order = true;
                else
                    fl_break_order = false;
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование табеля
        /// </summary>
        /// <param name="data"></param>
        void TotalTable(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                string PathOfTemplate = Application.StartupPath + @"\Reports\Title.xlt";
                m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                if (fl_form_title)
                {
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                    ExcelParameter[] paramTitle = new ExcelParameter[] {
                        new ExcelParameter("AA15", code_subdiv), 
                        new ExcelParameter("AF13", string.Format("{0:d2}", ((Table)this.ActiveMdiChild).EndDate.Month)), 
                        new ExcelParameter("AK13", string.Format("{0:d4}", ((Table)this.ActiveMdiChild).EndDate.Year))
                    };
                    if (paramTitle != null)
                        foreach (ExcelParameter parameter in paramTitle)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                decimal tempTableID = 0;
                OracleDataTable dtHours;
                ExcelParameter[] excelParameters;
                if (fl_form_table)
                {
                    /// Создаем новую команду
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    /// Указываем тип - хранимая процедура
                    command.CommandType = CommandType.Text;
                    /// Создаем запрос
                    command.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                        ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                        Connect.Schema);
                    /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
                    /// часов для табеля
                    command.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                        DateTime.DaysInMonth(((Table)this.ActiveMdiChild).EndDate.Year,
                        ((Table)this.ActiveMdiChild).EndDate.Month);
                    command.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                        ((Table)this.ActiveMdiChild).EndDate.Month;
                    command.Parameters.Add("p_year", OracleDbType.Decimal).Value =
                        ((Table)this.ActiveMdiChild).EndDate.Year;
                    command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
                    command.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
                    command.Parameters["p_temp_table_id"].Direction = ParameterDirection.Output;
                    ((BackgroundWorker)sender).ReportProgress(3);
                    try
                    {
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    if (timeExecute.backWorker.CancellationPending)
                    {
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    ((BackgroundWorker)sender).ReportProgress(8);
                    /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                    tempTableID = (decimal)((OracleDecimal)(command.Parameters["p_temp_table_id"].Value));
                    /// Создаем таблицу для часов и заполняем ее
                    dtHours = new OracleDataTable("", Connect.CurConnect);
                    dtHours.SelectCommand.CommandText = string.Format(Queries.GetQuery(
                        "Table/SelectHoursForTable.sql"), Connect.Schema);
                    //dtHours.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal);
                    //dtHours.SelectCommand.Parameters.Add("p_sign_comb", OracleDbType.Decimal);
                    /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
                    excelParameters = new ExcelParameter[] {
                        new ExcelParameter("H2", code_subdiv), 
                        new ExcelParameter("L2", string.Format("{0:d2}", ((Table)this.ActiveMdiChild).EndDate.Month)), 
                        new ExcelParameter("P2", string.Format("{0:d4}", ((Table)this.ActiveMdiChild).EndDate.Year)),
                        new ExcelParameter("W2", ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["code_degree"].Value.ToString()),
                        new ExcelParameter("AR2", "1"),
                        new ExcelParameter("AC2", ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["group_master"].Value.ToString() != "" ?
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["group_master"].Value.ToString() + 
                            (((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["code_degree"].Value.ToString() == "01" ||
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["code_degree"].Value.ToString() == "02" ?
                            "" 
                            : " / " + ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["order_name"].Value.ToString())
                            : ((Table)this.ActiveMdiChild).dgEmp.Rows[0].Cells["order_name"].Value.ToString())
                    };

                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[2];
                    // 29.07.2016 Добавляю время закрытия в печать
                    OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                    adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectTimeClosingTable.sql"),
                        Connect.Schema);
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal).Value = ((Table)this.ActiveMdiChild).Table_Closing_ID;
                    DataTable dtProtocol = new DataTable();
                    adapter.Fill(dtProtocol);
                    if (dtProtocol.Rows.Count > 0)
                    {
                        m_Sheet.Cells[1, 1] = "Закрыто " + dtProtocol.Rows[0]["DATE_APPROVAL"];
                    }
                    /// Создаем области для копирования строк по работнику и заголовка
                    Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("1:9", Type.Missing);
                    //Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("A1", "AR9");
                    Microsoft.Office.Interop.Excel.Range range_Emp = m_Sheet.get_Range("6:9", Type.Missing);
                    Microsoft.Office.Interop.Excel.Range range_cur;
                    /// Номер текущей строки в отчете куда будет производиться вставка данных
                    int rowNow = 6;
                    /// Номер листа
                    int sheetNumber = 1;
                    /// Переменная определяет сколько строк уже заполнено в отчете
                    /// Если их количество равно 6, следующий работник будет размещаться на
                    /// сделующем листе отчета.
                    int rowsCount = 1;
                    /// Заполняем отдельные параметры
                    if (excelParameters != null)
                        foreach (ExcelParameter parameter in excelParameters)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                    /// Объявляем переменные для категорий и ячеек
                    string degree1, degree2, order1, order2;
                    OracleDataTable dtDayOff = new OracleDataTable("", Connect.CurConnect);
                    dtDayOff.SelectCommand.CommandText = string.Format(
                        Queries.GetQuery("Table/SelectHolidayForTable.sql"), Connect.Schema);
                    dtDayOff.SelectCommand.Parameters.Add("D1", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).BeginDate;
                    dtDayOff.SelectCommand.Parameters.Add("D2", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).EndDate;
                    dtDayOff.Fill();
                    //заполнение таблицы выходных дней
                    //раскраска выходных дней
                    m_Book.Styles[1].Interior.Color = 0xDFDFDF;
                    m_Book.Styles[1].Font.Size = 6;
                    m_Book.Styles[2].Interior.Color = 0xDFDFDF;
                    m_Book.Styles[2].Font.Size = 8;
                    string[,] strHours = new string[4, 44];
                    for (int row = 0; row < ((Table)this.ActiveMdiChild).dgEmp.RowCount; row++)
                    {
                        if (timeExecute.backWorker.CancellationPending)
                        {
                            m_Book.Close(false, Type.Missing, Type.Missing);
                            m_ExcelApp.Quit();
                            return;
                        }
                        ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)row * 50 / ((Table)this.ActiveMdiChild).dgEmp.RowCount, 0)) + 10);
                        /// Заносим значения
                        strHours[0, 0] = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[0].Value.ToString();
                        strHours[0, 1] = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[5].Value.ToString();
                        strHours[0, 2] = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[7].Value.ToString();
                        strHours[0, 3] = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[1].Value.ToString();
                        /// Выбираеи из временной таблицы часы для текущего работника
                        dtHours.Clear();
                        dtHours.SelectCommand.Parameters["p_temp_table_id"].Value = tempTableID;
                        dtHours.SelectCommand.Parameters["p_TRANSFER_ID"].Value =
                            ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["TRANSFER_ID"].Value;
                        dtHours.Fill();
                        //27.02.2011 - добавлена проверка на заполнение dtHours
                        if (dtHours.Rows.Count > 0) //Проверка dtHours на заполнение данными
                        {
                            //Перебираем все колонки
                            for (int column = 0; column < 34; column++)
                            {
                                for (int rrow = 0; rrow < 4; rrow++)
                                    strHours[rrow, column + 4] = dtHours.Rows[rrow][column].ToString();
                            }
                            strHours[0, 38] = dtHours.Rows[0][34].ToString();
                            strHours[0, 39] = dtHours.Rows[0][35].ToString();
                            strHours[0, 40] = dtHours.Rows[0][36].ToString();
                            strHours[0, 41] = dtHours.Rows[0][37].ToString();
                            strHours[0, 42] = dtHours.Rows[0][38].ToString();
                            strHours[0, 43] = dtHours.Rows[0][39].ToString();
                        }
                        m_Sheet.get_Range(string.Format("A{0}", rowNow), string.Format("AR{0}", rowNow + 3)
                                ).set_Value(Type.Missing, strHours);
                        /// Если текущая строка + 1 меньше количества строк. 
                        /// Делаем это для того, чтобы не вставить лишний заголовок если следующая строка последняя
                        if (row + 1 < ((Table)this.ActiveMdiChild).dgEmp.RowCount)
                        {
                            /// Заносим в переменные значения категорий текущей строки и следующей
                            degree1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells[9].Value.ToString();
                            degree2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells[9].Value.ToString();
                            // При формировании заказов определяем нужно ли нам разбивать отчет по заказам
                            // Если нет, то выводим в заказ лишь группу мастера у кого есть.
                            // Если группа мастера пустая, то в order1 пишем заказ
                            if (((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["group_master"].Value == DBNull.Value)
                            {
                                if (fl_break_order)
                                    order1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["order_name"].Value.ToString();
                                else
                                    order1 = "(нет разбивки)";
                            }
                            else
                            {
                                /* Если подразделение цех 61, 
                                     то в order1 записываем группу мастера + номер заказа.
                                 Иначе в order1 записываем группу мастера + номер заказа для категорий != 01 и 02*/
                                if (code_subdiv == "061")
                                {
                                    order1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["group_master"].Value.ToString() + "/" +
                                        ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["order_name"].Value.ToString();
                                }
                                else
                                {
                                    if (fl_break_order)
                                        order1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["group_master"].Value.ToString() +
                                            (degree1 == "01" || degree1 == "02" ? ""
                                            : " / " + ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["order_name"].Value.ToString());
                                    else
                                        order1 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row].Cells["group_master"].Value.ToString() +
                                            (degree1 == "01" || degree1 == "02" ? ""
                                            : " / " + "(нет разбивки)");
                                }
                            }
                            // Если группа мастера пустая, то в order2 пишем заказ
                            if (((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["group_master"].Value == DBNull.Value)
                            {
                                if (fl_break_order)
                                    order2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["order_name"].Value.ToString();
                                else
                                    order2 = "(нет разбивки)";
                            }
                            else
                            {
                                /* Если подразделение цех 61, 
                                     то в order2 записываем группу мастера + номер заказа.
                                 Иначе в order2 записываем группу мастера + номер заказа для категорий != 01 и 02*/
                                if (code_subdiv == "061")
                                {
                                    order2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["group_master"].Value.ToString() + "/" +
                                        ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["order_name"].Value.ToString();
                                }
                                else
                                {
                                    if (fl_break_order)
                                        order2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["group_master"].Value.ToString() +
                                            (degree2 == "01" || degree2 == "02" ? ""
                                            : " / " + ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["order_name"].Value.ToString());
                                    else
                                        order2 = ((Table)this.ActiveMdiChild).dgEmp.Rows[row + 1].Cells["group_master"].Value.ToString() +
                                            (degree2 == "01" || degree2 == "02" ? ""
                                            : " / " + "(нет разбивки)");
                                }
                            }

                            /// Если количество заполненных строк меньше 6, категории равны и заказы равны
                            if (rowsCount < 6 && degree1 == degree2 && order1 == order2)
                            {
                                /// Увеличиваем номер строки в отчете на 4, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 4;
                                /// Копируем строки по адресам ячеек                                
                                range_Emp.Rows.Copy(m_Sheet.get_Range(
                                    string.Format("{0}:{1}", rowNow, rowNow + 3), Type.Missing));
                                /// Увеличиваем количество вставленных строк
                                rowsCount++;
                            }
                            else
                            {
                                /// Увеличиваем номер строки в отчете на 3, 
                                /// чтобы встать на последнюю вставленную строку
                                rowNow += 3;
                                string ColName1, ColName2;
                                foreach (DataRow rowCal in dtDayOff.Rows)
                                {
                                    // Устанавливаем цвет заголовка столбцов. Для этого от последней 
                                    // вставленной строки поднимаемся вверх на первую строку по человеку
                                    // и еще -1, чтобы быть на заголовке столбца
                                    ColName1 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_BEGIN"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1) - 1);
                                    ColName2 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_END"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1) - 1);
                                    m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[2];
                                    // Устанавливаем цвет дней табеля
                                    ColName1 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_BEGIN"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow - (rowsCount * 4 - 1));
                                    ColName2 = string.Format("{0}{1}",
                                        Excel.ParseColNum(Convert.ToInt32(rowCal["D_END"]) + 69 +
                                        Convert.ToInt32(rowCal["GR_15"])),
                                        rowNow);
                                    m_Sheet.get_Range(ColName1, ColName2).Style = m_Book.Styles[1];
                                }
                                /// Увеличиваем номер строки в отчете на 1, 
                                /// чтобы заголовок покал на нужные строки
                                rowNow += 1;
                                /// Копируем строки по адресам ячеек                              
                                range_cur = m_Sheet.get_Range(
                                    string.Format("{0}:{1}", rowNow, rowNow + 8), Type.Missing);
                                range_Title.Rows.Copy(range_cur);
                                /// Вставляем разрыв страницы, для того чтобы новая категория была
                                /// на новом листе отчета
                                m_Sheet.HPageBreaks.Add(range_cur);

                                /// Увеличиваем номер строки в отчете на 1, 
                                /// чтобы вставлять категорию и номер страницы на нужные строки
                                rowNow += 1;
                                /// Заносим категорию
                                m_Sheet.get_Range(string.Format("W{0}", rowNow), Type.Missing).Value2 =
                                    degree2;
                                /// Заносим заказ
                                m_Sheet.get_Range(string.Format("AC{0}", rowNow), Type.Missing).Value2 =
                                    order2;
                                /// Заносим номер страницы
                                m_Sheet.get_Range(string.Format("AR{0}", rowNow), Type.Missing).Value2 =
                                    ++sheetNumber;
                                /// Увеличиваем номер строки в отчете на 4, 
                                /// чтобы следующий работник вставился на нужные строки
                                rowNow += 4;
                                /// Сбрасываем количество вставленных строк в отчете,
                                /// так как новую категорию печатаем на новом листе
                                rowsCount = 1;
                            }
                        }
                    }
                    m_Sheet.Protect("euflfq", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                decimal temp_salary_id = 0;
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(65);
                // Если формировали табель или нужно формировать приложение, то создаем команду
                if (fl_form_table || fl_form_appendix)
                {
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        "begin {0}.Calc_Appendix(:p_user_name, :p_subdiv_id, :p_begin_date, :p_end_date, :p_temp_salary_id); end;",
                        Connect.Schema);
                    command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
                    command.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).BeginDate;
                    command.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).EndDate;
                    command.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    /// Выполняем команду
                    try
                    {
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_Book.Close(false, Type.Missing, Type.Missing);
                        m_ExcelApp.Quit();
                        return;
                    }
                    /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                    temp_salary_id = (decimal)((OracleDecimal)(command.Parameters["p_temp_salary_id"].Value));
                }
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(75);
                // Если формировали табель, то нужно вывести протокол ошибок
                if (fl_form_table)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                    adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectProtocol.sql"),
                        Connect.Schema);
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("p_temp_table_id", OracleDbType.Decimal).Value = tempTableID;
                    adapter.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = temp_salary_id;
                    DataTable dtProtocol = new DataTable();
                    adapter.Fill(dtProtocol);

                    if (dtProtocol.Rows.Count > 0)
                    {
                        ExcelParameter[] excelParameters1 = new ExcelParameter[] {
                            new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                                ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                                ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                        Excel.PrintWithBorder(true, "ProtocolTable.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters1);
                    }
                    // Убираем данные из временной таблицы табеля
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandText = string.Format(
                        "begin DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id; commit; end;",
                        Connect.Schema);
                    command.Parameters.Add("p_temp_table_id", tempTableID);
                    command.ExecuteNonQuery();
                }
                if (timeExecute.backWorker.CancellationPending)
                {
                    m_Book.Close(false, Type.Missing, Type.Missing);
                    m_ExcelApp.Quit();
                    return;
                }
                ((BackgroundWorker)sender).ReportProgress(80);
                // Если нужно формировать приложение
                if (fl_form_appendix)
                {
                    /// Создаем таблицу и заполняем ее
                    dtHours = new OracleDataTable("", Connect.CurConnect);
                    dtHours.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAppendix.sql"),
                        Connect.Schema);
                    dtHours.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = 
                        temp_salary_id;
                    dtHours.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = 
                        ((Table)this.ActiveMdiChild).BeginDate;
                    dtHours.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = 
                        ((Table)this.ActiveMdiChild).EndDate;
                    dtHours.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
                    dtHours.SelectCommand.Parameters.Add("p_days", OracleDbType.Decimal).Value = 
                        ((Table)this.ActiveMdiChild).EndDate.Day;
                    dtHours.SelectCommand.Parameters.Add("p_code_posC", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_degree", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o1", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o2", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos1", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos2", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos3", OracleDbType.Decimal);
                    /// Создаем массив параметров: подразделение, месяц, год, категория, номер страницы
                    excelParameters = new ExcelParameter[] {
                        new ExcelParameter("Z2", code_subdiv), 
                        new ExcelParameter("AB2", string.Format("{0:d2}", ((Table)this.ActiveMdiChild).EndDate.Month)), 
                        new ExcelParameter("AE2", string.Format("{0:d4}", ((Table)this.ActiveMdiChild).EndDate.Year))
                    };

                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[3];
                    //Заполняем отдельные параметры
                    if (excelParameters != null)
                        foreach (ExcelParameter parameter in excelParameters)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                    dtHours.Clear();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "02";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "09";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "2";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "3";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "4";
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "05";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "61";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "07";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "11";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "12";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "13";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.Fill();
                    if (dtHours.Rows.Count > 0)
                    {
                        string[,] mas = new string[dtHours.Rows.Count, dtHours.Columns.Count];
                        for (int i = 0; i < dtHours.Rows.Count; i++)
                        {
                            ((BackgroundWorker)sender).ReportProgress(Convert.ToInt32(Math.Round((decimal)i * 10 / ((Table)this.ActiveMdiChild).dgEmp.RowCount, 0)) + 85);
                            for (int j = 0; j < dtHours.Columns.Count; j++)
                            {
                                mas[i, j] = dtHours.Rows[i][j].ToString();
                            }
                        }
                        m_Sheet.get_Range("F13", "AM28").set_Value(Type.Missing, mas);
                        // Выбор согласования закрытия табеля
                        OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                        adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectApprovalForTableClosing.sql"),
                            Connect.Schema);
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add("p_TABLE_CLOSING_ID", OracleDbType.Decimal).Value = ((Table)this.ActiveMdiChild).Table_Closing_ID;
                        DataTable dtProtocol = new DataTable();
                        adapter.Fill(dtProtocol);

                        foreach (DataRow row in dtProtocol.Rows)
                        {
                            m_Sheet.Cells[Convert.ToInt16(row["ROW_INDEX"]), Convert.ToInt16(row["COLUMN_INDEX"])] = 
                                row["USER_FIO"].ToString() + " " + row["NOTE"].ToString();
                        }
                        m_Sheet.Protect("euflfq", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                }
                else
                {
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[3]).Visible =
                        Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                m_ExcelApp.Visible = true;

            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Формирование отчета по численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReportPopul_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Выдаем запрос нужно ли формировать отчет
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по численности за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                fl_form_title = false;
                fl_form_table = false;
                fl_form_appendix = true;
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    TotalTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Отработанные часы 102 ш.о. по заказам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepHoursByOrders_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по отработанным часам за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Вставляем табельные номера
                InsertPerNum();
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepHoursByOrders(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование файла для расчета зарплаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RepHoursByOrders(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = null;
            com.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter _daRep = new OracleDataAdapter("", Connect.CurConnect);
            _daRep.SelectCommand.BindByName = true;
            _daRep.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepHoursByOrders.sql"),
                Connect.Schema);
            _daRep.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = temp_salary_id;
            DataTable _dtRep = new DataTable();
            _daRep.Fill(_dtRep);
            Excel.PrintWithBorder(true, "RepHoursByOrders.xlt", "A6",
                        new DataTable[] { _dtRep },
                        new ExcelParameter[] {new ExcelParameter("A2", "за " + 
                            ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0').ToString() +
                            " месяц " + ((Table)this.ActiveMdiChild).BeginDate.Year.ToString() + " г.")});
        }

        /// <summary>
        /// Сброс текстового файла на аванс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFileAdvance_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать файл на аванс за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " + 
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {                        
                // Вставляем табельные номера
                InsertPerNum();
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    FileAdvance(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование файла для расчета аванса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileAdvance(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            // Создаем новую команду
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            // Указываем тип - хранимая процедура
            command.CommandType = CommandType.Text;
            int month = ((Table)this.ActiveMdiChild).EndDate.Month;
            int year = ((Table)this.ActiveMdiChild).EndDate.Year;
            // Создаем запрос
            command.CommandText = string.Format(string.Format(
                "begin {0}.TABLEForAdvance(:p_month, :p_year, :p_user_name, :p_subdiv_id, :p1); end;",
                Connect.Schema, month, year, Connect.UserId.ToUpper(), subdiv_id));
            command.Parameters.Add("p_month", OracleDbType.Int16).Value = month;
            command.Parameters.Add("p_year", OracleDbType.Int16).Value = year;
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            command.Parameters.Add("p_subdiv_id", OracleDbType.Int32).Value = subdiv_id;
            command.Parameters.Add("p1", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            // Выполняем команду
            command.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(80);
            decimal tempTableID = (decimal)((OracleDecimal)(command.Parameters[0].Value));
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format(Queries.GetQuery("Table/SelectForAdvance.sql"),
                Connect.Schema);
            command.Parameters.Add("p_temp_table_id", OracleDbType.Decimal).Value = tempTableID;
            command.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2).Value = code_subdiv;
            OracleDataReader reader = command.ExecuteReader();
            DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                ((Table)this.ActiveMdiChild).BeginDate.Year.ToString(),
                ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0')));
            if (!dir.Exists)
            {
                dir.Create();
            }
            TextWriter writer = new StreamWriter(dir.FullName + string.Format(@"\A{0}{1}{2}.txt", code_subdiv,
                ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0').ToString(),
                ((Table)this.ActiveMdiChild).BeginDate.Year.ToString().Substring(2, 2)), false, Encoding.GetEncoding(1251));
            ((BackgroundWorker)sender).ReportProgress(90);
            string st = "";
            while (reader.Read())
            {
                st = reader["PTN"].ToString() + reader["SC"].ToString() + reader["NP"].ToString() +
                    reader["ZN"].ToString() + reader["P_RAB"].ToString() + reader["VOP"].ToString() +
                    reader["PR"].ToString() + reader["ZAK"].ToString() + reader["TN"].ToString() + reader["HCAS"].ToString() +
                    reader["SUM"].ToString() + reader["GM"].ToString() + reader["YN"].ToString() + reader["KT"].ToString();
                writer.WriteLine(st);
            }
            writer.Close();
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p1",
                Connect.Schema);
            command.Parameters.Add("p1", OracleDbType.Decimal).Value = tempTableID;
            command.ExecuteNonQuery();
            Connect.Commit();
        }

        /// <summary>
        /// Сброс текстового файла на зарплату
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilePay_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать файл на зарплату за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    FilePay(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование файла для расчета зарплаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FilePay(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = null;
            com.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(Queries.GetQuery("Table/SelectForPay_New.sql"),
                Connect.Schema);
            com.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2).Value = code_subdiv;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = temp_salary_id;
            OracleDataReader reader = com.ExecuteReader();
            DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                ((Table)this.ActiveMdiChild).BeginDate.Year.ToString(), ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0')));
            if (!dir.Exists)
            {
                dir.Create();
            }
            ((BackgroundWorker)sender).ReportProgress(95);
            TextWriter writer = new StreamWriter(dir.FullName + string.Format(@"\Z{0}{1}{2}.txt", code_subdiv,
                ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0').ToString(),
                ((Table)this.ActiveMdiChild).BeginDate.Year.ToString().Substring(2, 2)), false, Encoding.GetEncoding(1251));
            string st = "";
            while (reader.Read())
            {
                st = reader["PTN"].ToString() + reader["SC"].ToString() + reader["NP"].ToString() +
                    reader["ZN"].ToString() + reader["P_RAB"].ToString() + reader["VOP"].ToString() +
                    reader["PR"].ToString() + reader["ZAK"].ToString() + reader["TN"].ToString() + reader["HCAS"].ToString() +
                    reader["SUM"].ToString() + reader["GM"].ToString() + reader["YN"].ToString() + reader["KT"].ToString();
                writer.WriteLine(st);
            }
            Connect.Commit();
            writer.Close();
        }

        /// <summary>
        /// Сохранение данных о численности работников в таблице DBF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveTable_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Выдаем запрос нужно ли формировать отчет
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по численности за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                OdbcConnection odbcCon = new OdbcConnection(@"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB=" + ParVal.Vals["PathAppendix"]);
                //OdbcConnection odbcCon = new OdbcConnection(@"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB=c:\WORK\DBF\");
                odbcCon.Open();
                OdbcCommand _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format("select count(*) from turco " +
                    "where podr = '{0}' and month(data) = {1} and year(data) = {2}", code_subdiv,
                    ((Table)this.ActiveMdiChild).BeginDate.Month,
                    ((Table)this.ActiveMdiChild).BeginDate.Year);
                int rez = Convert.ToInt32(_rezult.ExecuteScalar());
                if (rez == 0)
                {
                    _rezult = new OdbcCommand("", odbcCon);
                    _rezult.CommandText = string.Format(
                        "select kdvpodr from spodr where podr = '{0}' and left(kdvpodr,1) = '0'",
                        code_subdiv);
                    OdbcDataReader reader = _rezult.ExecuteReader();
                    string kdvpodr = "";
                    while (reader.Read())
                    {
                        kdvpodr = reader[0].ToString();
                    }
                    OracleCommand command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        "begin {0}.Calc_Appendix(:p_user_name, :p_subdiv_id, :p_begin_date, :p_end_date, :p_temp_salary_id); end;",
                        Connect.Schema);
                    command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                    command.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
                    command.Parameters.Add("p_begin_date", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).BeginDate;
                    command.Parameters.Add("p_end_date", OracleDbType.Date).Value =
                        ((Table)this.ActiveMdiChild).EndDate;
                    command.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
                    command.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
                    /// Выполняем команду
                    command.ExecuteNonQuery();
                    /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                    decimal temp_salary_id = (decimal)((OracleDecimal)(command.Parameters["p_temp_salary_id"].Value));
                    /// Создаем таблицу и заполняем ее
                    OracleDataTable dtHours = new OracleDataTable("", Connect.CurConnect);
                    dtHours.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAppendixDump.sql"),
                        Connect.Schema);
                    dtHours.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_degree", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_npp", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_kdvpodr", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o1", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_f_o2", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos1", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos2", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_pos3", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
                    dtHours.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
                    dtHours.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters.Add("p_code_posC", OracleDbType.Varchar2);
                    dtHours.SelectCommand.Parameters.Add("p_days", OracleDbType.Decimal);
                    dtHours.SelectCommand.Parameters["p_temp_salary_id"].Value = temp_salary_id;
                    dtHours.SelectCommand.Parameters["p_code_subdiv"].Value = code_subdiv;
                    dtHours.SelectCommand.Parameters["p_kdvpodr"].Value = kdvpodr;
                    dtHours.SelectCommand.Parameters["p_date_begin"].Value = ((Table)this.ActiveMdiChild).BeginDate;
                    dtHours.SelectCommand.Parameters["p_date_end"].Value = ((Table)this.ActiveMdiChild).EndDate;
                    dtHours.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
                    dtHours.SelectCommand.Parameters["p_days"].Value = ((Table)this.ActiveMdiChild).EndDate.Day;
                    dtHours.Clear();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 1;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "01";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 2;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 3;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "08";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 4;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "02";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 5;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "09";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 6;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 7;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 2;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 8;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 3;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "3";
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 9;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "04";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = "4";
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 10;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "05";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 11;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "61";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 12;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "07";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 13;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "11";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 14;
                    dtHours.Fill();
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "12";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 15;
                    dtHours.Fill();
                    /// Для 13 категории выбираем вид производства еще раз
                    _rezult = new OdbcCommand("", odbcCon);
                    _rezult.CommandText = string.Format(
                        "select kdvpodr from spodr where podr = '{0}' and left(kdvpodr,1) != '0'",
                        code_subdiv);
                    reader = _rezult.ExecuteReader();
                    while (reader.Read())
                    {
                        kdvpodr = reader[0].ToString();
                    }
                    dtHours.SelectCommand.Parameters["p_kdvpodr"].Value = kdvpodr;
                    dtHours.SelectCommand.Parameters["p_code_degree"].Value = "13";
                    dtHours.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                    dtHours.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                    dtHours.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                    dtHours.SelectCommand.Parameters["p_code_posC"].Value = null;
                    dtHours.SelectCommand.Parameters["p_npp"].Value = 16;
                    dtHours.Fill();
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandText = "commit";
                    command.ExecuteNonQuery();
                    for (int f = 0; f < dtHours.Rows.Count; f++)
                    {
                        string strIn = dtHours.Rows[f][0].ToString() + ", ";
                        for (int j = 1; j < 4; j++)
                        {
                            strIn += "'" + dtHours.Rows[f][j].ToString() + "', ";
                        }
                        for (int j = 4; j < dtHours.Columns.Count; j++)
                        {
                            strIn += dtHours.Rows[f][j].ToString() == "" ? "0, " :
                                dtHours.Rows[f][j].ToString().Replace(",", ".") + ", ";
                        }
                        strIn += "{^" + ((Table)this.ActiveMdiChild).EndDate.Year + "-" +
                            ((Table)this.ActiveMdiChild).EndDate.Month + "-" +
                            ((Table)this.ActiveMdiChild).EndDate.Day + "}";
                        _rezult = new OdbcCommand("", odbcCon);
                        _rezult.CommandText = string.Format(
                            "insert into turco(npp, kdvpodr, podr, kt, cdur, vrur, ccur, vcur, opsb, oppr, opnv, opgd, oppd, " +
                            "opkm, opot, oppv, opbl, sprv, goob, adad, adot, ohot, nevk, oouh, nouh, prog, slxr, " +
                            "vprd, subv, vsne, vsdr, vskm, vsnd, srsp, vskn, vsqm, prrd, data) " +
                            "values({0})", strIn
                            );
                        _rezult.ExecuteNonQuery();
                    }
                    command = new OracleCommand("", Connect.CurConnect);
                    command.BindByName = true;
                    command.CommandText = "commit";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные по численности работников сохранены в базе данных.",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Данные по численности работников уже были сохранены в базе данных.",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                odbcCon.Close();
            }
            //    }
            //}
        }
                
        /// <summary>
        /// Табель за период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTable_By_Period_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Emp_By_Period_For_Table_View emp_View = new Emp_By_Period_For_Table_View(subdiv_id, DateTime.Today.AddMonths(-1));
            WindowInteropHelper wih = new WindowInteropHelper(emp_View);
            wih.Owner = this.Handle;
            emp_View.ShowDialog();
        }
        
        /// <summary>
        /// Отчет по декретным отпускам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepChild_Realing_Leave_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dtReport = new DataTable();
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepChild_Rearing_Leave_Staff_Pension.sql"),
                    Connect.Schema);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = selPeriod.BeginDate;
                odaReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = selPeriod.EndDate;
                odaReport.Fill(dtReport);
                if (dtReport.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", " за период с " + 
                        selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepChild_Realing_Leave.xlt", "A6", new DataTable[] { dtReport }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        /// <summary>
        /// Отчет по декретным отпускам (форма для личного стола без лишних полей)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepChild_Realing_Leave2_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("REPORT");
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("RepChild_Rearing_Leave.sql"),
                    Connect.Schema);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = selPeriod.BeginDate;
                odaReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = selPeriod.EndDate;
                odaReport.Fill(ds.Tables["REPORT"]);
                if (ds.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow _report = new ReportViewerWindow("Отчет по декретным отпускам", "Reports/RepChild_Realing_Leave.rdlc",
                        ds, new List<ReportParameter> {
                            new ReportParameter("P_BEGINPERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new ReportParameter("P_ENDPERIOD", selPeriod.EndDate.ToShortDateString())});
                    _report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет Общие данные гидропроцедур по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btMiddleWatterSubdivTbl_Click(object sender, EventArgs e)
        {
            Ras.AccountSertificate.PrintMiddleWaterProcData(((Table)this.ActiveMdiChild).subdiv_id,
                ((Table)this.ActiveMdiChild).code_subdiv, ((Table)this.ActiveMdiChild).BeginDate);
        }

   #endregion

        /// <summary>
        /// Вставка табельных номеров во временную таблицу по текущему списку работников
        /// </summary>
        void InsertPerNum()
        {
            /// Создаем новую команду и заполняем ее строку запроса, 
            /// которая будет удалять все записи из временной таблицы PN_TMP для 
            /// данного пользователя
            command = new OracleCommand(
                string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                Connect.Schema), Connect.CurConnect);
            command.BindByName = true;
            /// Выполняем команду
            command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            command.ExecuteNonQuery();
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            /// Создаем строку запроса, которая будет вставлять во временную таблицу
            /// табельные номера
            command.CommandText = string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)", Connect.Schema);
            command.Parameters.Add("PN", OracleDbType.Varchar2);
            command.Parameters.Add("UN", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            command.Parameters.Add("TR", OracleDbType.Decimal);
            /// Идем по списку работников
            for (int i = 0; i < ((Table)this.ActiveMdiChild).dgEmp.RowCount; i++)
            {
                /// Заносим в первый параметр табельный номер
                command.Parameters[0].Value =
                    ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["per_num"].Value.ToString();
                /// Заносим в 3 параметр признак совмещения
                command.Parameters[2].Value =
                    ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["transfer_id"].Value.ToString();
                /// Выполняем команду
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Просмотр приказов на работу в выходные и сверхурочные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btViewOrders_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "ORDERSONHOLIDAY").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "ORDERSONHOLIDAY").First().Activate();
                return;
            }
            /// Создаем форму, задаем родителя и отображаем ее на экране
            OrdersOnHoliday ordersOnHoliday = new OrdersOnHoliday(subdiv_id);
            ordersOnHoliday.Name = "ORDERSONHOLIDAY";
            ordersOnHoliday.MdiParent = this;
            CreateButtonApp(ordersOnHoliday, sender);
            ordersOnHoliday.WindowState = FormWindowState.Maximized;
            ordersOnHoliday.Show();
        }

        #region Отчеты табеля
        
        /// <summary>
        /// Закрытие табеля на аванс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCloseTableAdvance_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите закрыть табель на аванс?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 13.04.2016 Старый стремный вариант проверки
                //foreach (DataGridViewRow row in ((Table)this.ActiveMdiChild).dgEmp.Rows)
                //{
                //    if (row.Cells["ISPINK"].Value.ToString() == "1")
                //    {
                //        MessageBox.Show("Невозможно закрыть табель!\n" +
                //            "В списке есть работники с несбалансированным временем работы.\n" +
                //            "(горят розовым цветом)",
                //            "АРМ \"Учет рабочего времени\"",
                //            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }
                //}
                // Новый вариант
                if (((Table)this.ActiveMdiChild).dtEmp.Select("ISPINK = 1").Count() > 0)
                {
                    MessageBox.Show("Невозможно закрыть табель!\n" +
                        "В списке есть работники с несбалансированным временем работы.\n" +
                        "(горят розовым цветом)",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                DateTime p_date = DateTime.Parse(string.Format("15.{0}.{1} {2}:{3}:{4}",
                    ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0'),
                    ((Table)this.ActiveMdiChild).BeginDate.Year,
                    DateTime.Now.Hour, DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                    DateTime.Now.Second.ToString().PadLeft(2, '0')));
                UpdateDateForTable("Date_ADVANCE", p_date, subdiv_id);

                // 13.04.2016 Записываем нужные данные для нового варианта закрытия табеля
                OracleCommand _ocTable_Close = new OracleCommand(string.Format(
                    @"BEGIN
                        {0}.TABLE_CLOSE(:p_SUBDIV_ID, :p_TABLE_DATE, :p_TYPE_TABLE_ID);
                    END;", Connect.Schema), Connect.CurConnect);
                _ocTable_Close.BindByName = true;
                _ocTable_Close.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
                _ocTable_Close.Parameters.Add("p_TABLE_DATE", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
                _ocTable_Close.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal).Value = 1;
                OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _ocTable_Close.Transaction = _transact;
                    _ocTable_Close.ExecuteNonQuery();
                    _transact.Commit();
                    ((Table)this.ActiveMdiChild).GetTable_Approval();
                }
                catch (Exception ex)
                {
                    _transact.Rollback();
                    MessageBox.Show("Ошибка закрытия табеля!\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Закрытие табеля на зарплату
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCloseTableSalary_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите закрыть табель на зарплату?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 13.04.2016 Старый стремный вариант проверки
                //foreach (DataGridViewRow row in ((Table)this.ActiveMdiChild).dgEmp.Rows)
                //{
                //    if (row.Cells["ISPINK"].Value.ToString() == "1")
                //    {
                //        MessageBox.Show("Невозможно закрыть табель!\n" +
                //            "В списке есть работники с несбалансированным временем работы.\n" +
                //            "(горят розовым цветом)",
                //            "АРМ \"Учет рабочего времени\"",
                //            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }
                //}
                // Новый вариант
                if (((Table)this.ActiveMdiChild).dtEmp.Select("ISPINK = 1").Count() > 0)
                {
                    MessageBox.Show("Невозможно закрыть табель!\n" +
                        "В списке есть работники с несбалансированным временем работы.\n" +
                        "(горят розовым цветом)",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                DateTime p_date = DateTime.Parse(string.Format("{0}.{1}.{2} {3}:{4}:{5}",
                    ((Table)this.ActiveMdiChild).EndDate.Day,
                    ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0'),
                    ((Table)this.ActiveMdiChild).BeginDate.Year,
                    DateTime.Now.Hour, DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                    DateTime.Now.Second.ToString().PadLeft(2, '0')));
                UpdateDateForTable("Date_Salary", p_date, subdiv_id);
            }
        }                

        /// <summary>
        /// Протокол ошибок в данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProtocolTable_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolTable));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    ProtocolTable(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре
        /// </summary>
        /// <param name="data"></param>
        void ProtocolTable(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду, которая расчитывает данные для вывода на печать
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            /// Создаем запрос
            com.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                Connect.Schema);
            /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
            /// часов для табеля
            com.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                DateTime.DaysInMonth(((Table)this.ActiveMdiChild).EndDate.Year,
                ((Table)this.ActiveMdiChild).EndDate.Month);
            com.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                ((Table)this.ActiveMdiChild).EndDate.Month;
            com.Parameters.Add("p_year", OracleDbType.Decimal).Value = ((Table)this.ActiveMdiChild).EndDate.Year;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
            com.Parameters["p_temp_table_id"].Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            // Выполняем команду
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            /// Переменная содержит идентификатор записей во временной таблице часов для табеля
            decimal tempTableID = (decimal)((OracleDecimal)(com.Parameters["p_temp_table_id"].Value));
            /// Создаем команду для расчета данных, которые сбрасываются в файл
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Value = null;
            com.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectProtocol.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_table_id", tempTableID);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            DataTable dtProtocol = new DataTable();
            ((BackgroundWorker)sender).ReportProgress(95);
            adapter.Fill(dtProtocol);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolTable.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id",
                Connect.Schema);
            command.Parameters.Add("p_temp_table_id", tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }

        /// <summary>
        /// Протокол ошибочных графиков работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btErrorGraph_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ClickErrorGraph(true);
            MessageBox.Show("Работа над протоколом неверных графиков закончена." + 
                "\nЕсли на экране не отобразился документ MS Excel, значит все графики верны.",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.OK, MessageBoxIcon.Information);     
        }

        /// <summary>
        /// Формирование отчета по ошибочным графикам работы
        /// </summary>
        /// <param name="_flagQuestion"></param>
        public void ClickErrorGraph(bool _flagQuestion)
        {
            // Вставляем табельные номера
            InsertPerNum();
            if (_flagQuestion)
            {
                if (MessageBox.Show("Вы действительно хотите сформировать протокол неверных графиков?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            // Создаем форму прогресса
            timeExecute = new TimeExecute();
            // Настраиваем что он должен выполнять
            timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
            {
                ErrorGraph(timeExecute.backWorker, e1);
            });
            // Запускаем теневой процесс
            timeExecute.backWorker.RunWorkerAsync();
            // Отображаем форму
            timeExecute.ShowDialog();
        }

        /// <summary>
        /// Формирование протокола ошибочных графиков
        /// </summary>
        /// <param name="data"></param>
        void ErrorGraph(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectErrorGraph.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", Connect.UserId.ToUpper());
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", subdiv_id);
            DataTable dtProtocol = new DataTable();
            ((BackgroundWorker)sender).ReportProgress(10);
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(90);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolErrorGraph.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }            
        }

        /// <summary>
        /// Отчет по работе за территорией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btWorkOut_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по работе за территорией?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*/// Вставляем табельные номера
                InsertPerNum();
                /// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(WorkOut));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    WorkOut(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();                
            }
        }

        /// <summary>
        ///  Формирование отчета по работе за территорией
        /// </summary>
        /// <param name="data"></param>
        void WorkOut(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOut.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", Connect.UserId.ToUpper());
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            adapter.SelectCommand.Parameters["p_date_begin"].Value = ((Table)this.ActiveMdiChild).BeginDate;
            adapter.SelectCommand.Parameters["p_date_end"].Value = ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "подразделения " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "WorkOut.xlt", "A4", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении нет документов по работе за территорией.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Отчет по опозданиям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLateness_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по опоздавшим
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по опозданиям?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(Lateness));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    Lateness(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();    
            }
        }

        /// <summary>
        ///  Формирование отчета по опозданиям
        /// </summary>
        /// <param name="data"></param>
        void Lateness(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepLateness.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            adapter.SelectCommand.Parameters["p_date_begin"].Value = ((Table)this.ActiveMdiChild).BeginDate;
            adapter.SelectCommand.Parameters["p_date_end"].Value = ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A2", "по подразделению " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "Lateness.xlt", "A4", new DataTable[] { dtProtocol }, excelParameters, null, true);
            }
            else
            {
                MessageBox.Show("В подразделении нет документов по опозданиям.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Ведомость расчета табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepCalc_Salary_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать ведомость расчета табеля за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepCalc_Salary));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepCalc_Salary(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  

            }
        }

        /// <summary>
        /// Формирование ведомости расчета табеля
        /// </summary>
        /// <param name="data"></param>
        void RepCalc_Salary(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем команду для расчета данных, которые сбрасываются в файл
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(5);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepCalc_Salary.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", ((Table)this.ActiveMdiChild).subdiv_id);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtRep = new DataTable();
            adapter.Fill(dtRep);
            if (dtRep.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters1 = new ExcelParameter[] {
                    new ExcelParameter("A1", "Ведомость расчета табеля в подразделении " + code_subdiv + 
                        " за " + ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "RepCalc_Salary.xlt", "A5", new DataTable[] { dtRep }, excelParameters1);
            }
            ((BackgroundWorker)sender).ReportProgress(80);
            adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepCalc_SalaryPrint.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", ((Table)this.ActiveMdiChild).subdiv_id);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_month",
                ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0'));
            adapter.SelectCommand.Parameters.Add("p_year",
                ((Table)this.ActiveMdiChild).BeginDate.Year.ToString());
            ((BackgroundWorker)sender).ReportProgress(90);
            dtRep = new DataTable();
            adapter.Fill(dtRep);
            if (dtRep.Rows.Count > 0)
            {
                TextWriter writer = new StreamWriter("c:" + string.Format(@"\Z1{0}{1}{2}.txt", code_subdiv,
                    ((Table)this.ActiveMdiChild).BeginDate.Month.ToString().PadLeft(2, '0').ToString(),
                    ((Table)this.ActiveMdiChild).BeginDate.Year.ToString().Substring(2, 2)), false, Encoding.GetEncoding(866));
                string st = "";
                int numpage = 1;
                int k = dtRep.Rows.Count / 64;
                int i, j, j1; i = j = 0;
                try
                {
                    for (i = 0; i < k + 1; i++)
                    {
                        writer.WriteLine("ЛИСТ " + numpage++);
                        j = j1 = 0;
                        while (j < 62)
                        {
                            st = dtRep.Rows[i * 64 + j1][2].ToString();
                            j += Convert.ToInt32(dtRep.Rows[i * 64 + j1][1]);
                            writer.WriteLine(st);
                            j1++;
                        }
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                    }
                }
                catch
                {
                    for (j1 = j; j1 < 66; j1++)
                    {
                        writer.WriteLine("");
                    }
                }
                //foreach (DataRow row in dtRep.Rows)
                //{
                //    //string[] str = Array.ConvertAll<object, string>(row.ItemArray, (el => el.ToString()));
                //    st = row.ItemArray[1].ToString(); // string.Join("", str, 1, 1);
                //    writer.WriteLine(st);
                //}
                writer.Close();
            }
        }

        /// <summary>
        /// Отчет о количестве часов по категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHoursOnDegree_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SelectDegree sd = new SelectDegree();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                /// Вставляем табельные номера
                OracleCommand command = new OracleCommand(
                    string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                /// Выполняем команду
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                /// Создаем строку запроса, которая будет вставлять во временную таблицу
                /// табельные номера
                command.CommandText = string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)", Connect.Schema);
                command.Parameters.Add("PN", OracleDbType.Varchar2);
                command.Parameters.Add("UN", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal);
                for (int i = 0; i < ((Table)this.ActiveMdiChild).dgEmp.RowCount; i++)
                {
                    if (((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["CODE_DEGREE"].Value.ToString() == sd.tbCode_Degree.Text)
                    {
                        command.Parameters[0].Value = ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["per_num"].Value.ToString();
                        command.Parameters[2].Value = ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["transfer_id"].Value.ToString();
                        /// Выполняем команду
                        command.ExecuteNonQuery();
                    }
                }
                Connect.Commit();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnDegree));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    HoursOnDegree(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        /// <summary>
        /// Формирование отчета о количестве часов по категории
        /// </summary>
        /// <param name="data"></param>
        void HoursOnDegree(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHoursOnDegree.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", subdiv_id);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            //adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "HoursOnDegree.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", Color.Yellow, Color.Black, 1m) });
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц нет данных по категории.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// Отчет о количестве часов по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHoursOnSubdiv_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите сформировать отчет?\n" +
                "Это займет продолжительное время.", "АРМ \"Учет рабочего времени\"", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                /// Вставляем табельные номера
                OracleCommand command = new OracleCommand(
                    string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                    Connect.Schema), Connect.CurConnect);
                command.BindByName = true;
                /// Выполняем команду
                command.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                command.ExecuteNonQuery();
                command = new OracleCommand("", Connect.CurConnect);
                command.BindByName = true;
                /// Создаем строку запроса, которая будет вставлять во временную таблицу
                /// табельные номера
                command.CommandText = string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)", Connect.Schema);
                command.Parameters.Add("PN", OracleDbType.Varchar2);
                command.Parameters.Add("UN", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
                command.Parameters.Add("TR", OracleDbType.Decimal);
                for (int i = 0; i < ((Table)this.ActiveMdiChild).dgEmp.RowCount; i++)
                {
                    command.Parameters[0].Value = ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["per_num"].Value.ToString();
                    command.Parameters[2].Value = ((Table)this.ActiveMdiChild).dgEmp.Rows[i].Cells["transfer_id"].Value.ToString();
                    /// Выполняем команду
                    command.ExecuteNonQuery();
                }
                Connect.Commit();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnDegree));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    HoursOnDegree(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Отчет по работе в выходные дни
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepWorkHol_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = ((Table)this.ActiveMdiChild).BeginDate;
            selPeriod.MaxDate = ((Table)this.ActiveMdiChild).EndDate;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepWorkHol));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepWorkHol(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        /// <summary>
        /// Формирование отчета по работе в выходные дни
        /// </summary>
        /// <param name="data"></param>
        void RepWorkHol(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepWorkHol.sql"),
                Connect.Schema, "PERCO");
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A3", "в подразделении " + code_subdiv + " с " + 
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepWorkHol.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", Color.Yellow, Color.Black, 1m) },
                    true);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Отчет по 124 шифру оплат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepWorkPT_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = ((Table)this.ActiveMdiChild).BeginDate;
            selPeriod.MaxDate = ((Table)this.ActiveMdiChild).EndDate;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepWorkPT));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepWorkPT(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        /// <summary>
        /// Формирование отчета по 124 шифру оплат
        /// </summary>
        /// <param name="data"></param>
        void RepWorkPT(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepPay_Type.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 124;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A3", "в подразделении " + code_subdiv + " с " + 
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepWorkHol.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", Color.Yellow, Color.Black, 1m) },
                    true);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Отчет по командировкам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepMission_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            selPeriod = new SelectPeriod();
            selPeriod.MinDate = ((Table)this.ActiveMdiChild).BeginDate.AddYears(-100);
            selPeriod.MaxDate = ((Table)this.ActiveMdiChild).EndDate.AddYears(100);
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(RepMission));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepMission(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        /// <summary>
        /// Формирование отчета по командировкам
        /// </summary>
        /// <param name="data"></param>
        void RepMission(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepMission.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_degree_id", OracleDbType.Decimal).Value = 8;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value = selPeriod.BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = selPeriod.EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                new ExcelParameter("A2", "в подразделении " + code_subdiv + " с " + 
                    selPeriod.BeginDate.ToShortDateString() + " по " + selPeriod.EndDate.ToShortDateString())};
                Excel.PrintWithBorder(true, "RepMission.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный период нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Отчет о количестве часов по видам оплат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepHoursOnSubdiv_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по видам оплат в разрезе группы мастера?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(HoursOnSubdiv));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    HoursOnSubdiv(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        /// <summary>
        /// Формирование отчета о количестве часов по видам оплат
        /// </summary>
        /// <param name="data"></param>
        void HoursOnSubdiv(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));

            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHoursOnSubdiv.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", subdiv_id);
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(55);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(80);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "HoursOnSubdiv.xlt", "A6", new DataTable[] { dtProtocol },
                    excelParameters,
                    new TotalRowsStyle[] { new TotalRowsStyle("GR1", Color.Yellow, Color.Black, 1m) });
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц нет данных по категории.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }                
                
        private void btDocsOnPay_Type_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ReportDocsOnPay_Type reportDocs = new ReportDocsOnPay_Type(
                ((Table)this.ActiveMdiChild).BeginDate, ((Table)this.ActiveMdiChild).EndDate,
                subdiv_id, code_subdiv);
            reportDocs.ShowDialog();
        }

        private void btProtokolForAccount_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для поиска!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolForAccount));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    ProtocolForAccount(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog(); 
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре для бухгалтерии
        /// </summary>
        /// <param name="data"></param>
        void ProtocolForAccount(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем новую команду, которая расчитывает данные для вывода на печать
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            /// Создаем запрос
            com.CommandText = string.Format("begin {0}.TABLEFORSALARY(:p_daysOfMonth, :p_month, " +
                ":p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                Connect.Schema);
            /// Создаем параметр, который будет хранить идентификатор записей во временной таблице
            /// часов для табеля
            com.Parameters.Add("p_daysOfMonth", OracleDbType.Decimal).Value =
                DateTime.DaysInMonth(((Table)this.ActiveMdiChild).EndDate.Year,
                ((Table)this.ActiveMdiChild).EndDate.Month);
            com.Parameters.Add("p_month", OracleDbType.Decimal).Value =
                ((Table)this.ActiveMdiChild).EndDate.Month;
            com.Parameters.Add("p_year", OracleDbType.Decimal).Value = ((Table)this.ActiveMdiChild).EndDate.Year;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_table_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(10);
            /// Выполняем команду
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(50);
            /// Переменная содержит идентификатор записей во временной таблице часов для табеля
            decimal tempTableID = (decimal)((OracleDecimal)(com.Parameters["p_temp_table_id"].Value));

            /// Создаем команду для расчета данных, которые сбрасываются в файл
            com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            com.Parameters.Add("p_beginDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            com.Parameters.Add("p_endDate", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).EndDate;
            com.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            com.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            com.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            ((BackgroundWorker)sender).ReportProgress(55);
            com.ExecuteNonQuery();
            ((BackgroundWorker)sender).ReportProgress(90);
            decimal temp_salary_id = (decimal)((OracleDecimal)(com.Parameters["p_temp_salary_id"].Value));
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectProtocolForAccount.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_temp_table_id", tempTableID);
            adapter.SelectCommand.Parameters.Add("p_temp_salary_id", temp_salary_id);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(95);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "в подразделении " + code_subdiv + " за " + 
                        ((Table)this.ActiveMdiChild).EndDate.Month + " месяц " + 
                        ((Table)this.ActiveMdiChild).EndDate.Year + "г.")};
                Excel.PrintWithBorder(true, "ProtocolForAccount.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            command = new OracleCommand("", Connect.CurConnect);
            command.BindByName = true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id",
                Connect.Schema);
            command.Parameters.Add("p_temp_table_id", tempTableID);
            command.ExecuteNonQuery();
            Connect.Commit();
        }
        
        private void btRepFailedOrders_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать протокол за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                /*/// Создаем новый поток
                Thread t = new Thread(new ParameterizedThreadStart(ProtocolForAccount));
                /// Запускаем созданный поток на выполнение
                t.Start();
                /// Создаем форму с продолжительностью работы программы
                CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepFailedOrders(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Формирование протокола ошибок в 102 шифре для бухгалтерии
        /// </summary>
        /// <param name="data"></param>
        void RepFailedOrders(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/Rep_ClosedOrders.sql"), 
                Connect.Schema), Connect.CurConnect);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataSet dtProtocol = new DataSet();
            adapter.Fill(dtProtocol, "DataSet1");
            ((BackgroundWorker)sender).ReportProgress(95);
            if (dtProtocol.Tables["DataSet1"].Rows.Count > 0)
            {
                ReportViewerWindow _rep = new ReportViewerWindow("Протокол закрытых или несуществующих заказов", 
                    "Reports/RepFailedOrders.rdlc", dtProtocol, 
                    new List<ReportParameter>() { 
                        new ReportParameter("P_CODE_SUBDIV", code_subdiv),
                        new ReportParameter("P_DATE", ((Table)this.ActiveMdiChild).BeginDate.ToShortDateString())}, true);
                _rep.ShowDialog();
            }
            else
            {
                MessageBox.Show("В подразделении за указанный месяц все данные корректны.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обновление даты аванса или зарплаты для подразделения
        /// </summary>
        /// <param name="_columnName">Поле для обновления (дата аванса или дата зарплаты)</param>
        /// <param name="_p_date">Дата готовности</param>
        /// <param name="_p_subdiv_id">Ключ подразделения</param>
        void UpdateDateForTable(string _columnName, DateTime _p_date, int _p_subdiv_id)
        {
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format("update {0}.SUBDIV_FOR_TABLE set {1} = :p_date where SUBDIV_ID = :p_subdiv_id",
                Connect.Schema, _columnName);
            com.Parameters.Add("p_date", _p_date);
            com.Parameters.Add("p_subdiv_id", _p_subdiv_id);
            com.ExecuteNonQuery();
            Connect.Commit();
        }
        
        /// <summary>
        /// Приказы на выходные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrderHoliday_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "ORDERSONHOLIDAY").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "ORDERSONHOLIDAY").First().Activate();
                return;
            }
            OrdersOnHoliday ordersOnHoliday = new OrdersOnHoliday(subdiv_id);
            ordersOnHoliday.Name = "ORDERSONHOLIDAY";
            ordersOnHoliday.MdiParent = this;
            CreateButtonApp(ordersOnHoliday, sender);
            ordersOnHoliday.Show();
        }

        /// <summary>
        /// Список подразделений электронного табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubdivTable_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "subdiv_for_table".ToUpper()).Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "subdiv_for_table".ToUpper()).First().Activate();
                return;
            }
            /// Создаем форму календаря, задаем родителя и отображаем ее на экране
            Subdiv_for_table subdiv_for_table = new Subdiv_for_table(this);
            subdiv_for_table.MdiParent = this;
            CreateButtonApp(subdiv_for_table, sender);
            subdiv_for_table.WindowState = FormWindowState.Maximized;
            subdiv_for_table.Show();
        }

        /// <summary>
        /// Закрытие табеля - новый вариант запущен 14.07.2016
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btList_Closing_Table_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "TABLE_CLOSING").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "TABLE_CLOSING").First().Activate();
                return;
            }
            Wpf_Control_Viewer table_Closing = new Wpf_Control_Viewer();
            table_Closing.Name = "TABLE_CLOSING";
            table_Closing.Text = "Закрытие табеля и согласование";
            Table_Closing_Viewer table_Closing_Viewer = new Table_Closing_Viewer(this);
            table_Closing.elementHost1.Child = table_Closing_Viewer;
            table_Closing.MdiParent = this;
            table_Closing.WindowState = FormWindowState.Maximized;
            table_Closing.Show();
        }
        
        /// <summary>
        /// Просмотр лимитов по подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLimit_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "viewLimit".ToUpper()).Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "viewLimit".ToUpper()).First().Activate();
                return;
            }
            /// Создаем форму календаря, задаем родителя и отображаем ее на экране
            ViewLimit viewLimit = new ViewLimit();
            viewLimit.MdiParent = this;
            CreateButtonApp(viewLimit, sender);
            viewLimit.WindowState = FormWindowState.Maximized;
            viewLimit.Show();
        }

        /// <summary>
        /// Просмотр лимитов по 303 по подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLimit303_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "viewLimit303".ToUpper()).Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "viewLimit303".ToUpper()).First().Activate();
                return;
            }
            /// Создаем форму календаря, задаем родителя и отображаем ее на экране
            ViewLimit303 viewLimit303 = new ViewLimit303();
            viewLimit303.MdiParent = this;
            CreateButtonApp(viewLimit303, sender);
            viewLimit303.WindowState = FormWindowState.Maximized;
            viewLimit303.Show();
        }

        /// <summary>
        /// Протокол начисления замещения по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProtokolReplSalT_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView d = ((Table)this.ActiveMdiChild).dgEmp;
                List<string> l = new List<string>();
                decimal t = 0m;
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery(@"new/CalcSalaryWithClear.sql"), Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_subdiv_id", ((Table)this.ActiveMdiChild).subdiv_id);
                cmd.Parameters.Add("p_beginDate", ((Table)this.ActiveMdiChild).BeginDate);
                cmd.Parameters.Add("p_endDate", ((Table)this.ActiveMdiChild).EndDate);
                cmd.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
                cmd.Parameters.Add("p_sign_calc", 1m);
                cmd.Parameters.Add("p_temp_salary_id", t);
                for (int i = 0; i < d.Rows.Count; ++i)
                {
                    cmd.Parameters["p_transfer_id"].Value = d["transfer_id", i].Value;
                    cmd.ExecuteNonQuery();
                    l.Add(t.ToString());
                }
                DataTable t1 = new DataTable();
                new OracleDataAdapter(string.Format(Queries.GetQuery(@"new/R_ProtokolReplSal.sql"), Connect.Schema, string.Join(",", l.ToArray())), Connect.CurConnect).Fill(t1);
                if (t1.Rows.Count > 0)
                    Excel.PrintWithBorder("ProtokolReplSal.xlt", "A4", new DataTable[] { t1 }, new ExcelParameter[]{ 
                        new ExcelParameter("A1",string.Format("Протокол начислений ЗП на замещения по подразделению {0} с {1} по {2}",((Table)this.ActiveMdiChild).code_subdiv,((Table)this.ActiveMdiChild).BeginDate,((Table)this.ActiveMdiChild).EndDate))});
                else
                    MessageBox.Show("Нет начислений по замещениям за выбранный период", "АРМ Учет рабочего времени");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);



            }
        }

        /// <summary>
        /// Замещения сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btViewReplEmpTable_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Table && ((Table)this.ActiveMdiChild).dgEmp.CurrentRow != null)
            {
                Shtat.ReplEmpForm f = new Kadr.Shtat.ReplEmpForm((decimal)((Table)this.ActiveMdiChild).dgEmp.CurrentRow.Cells["transfer_id"].Value, "TABLE");
                f.ShowDialog(this);
            }
        }

        /// <summary>
        /// Обновление списка работников в табеле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRefTableList_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ((Table)this.ActiveMdiChild).dgEmp.SelectionChanged -=
                ((Table)this.ActiveMdiChild).dgEmp_SelectionChanged;
            /// Очистка таблицы списка работников
            ((Table)this.ActiveMdiChild).dtEmp.Clear();
            /// Заполнение таблицы списка работников
            ((Table)this.ActiveMdiChild).dtEmp.Fill();
            /// Создаем биндингсоурс
            ((Table)this.ActiveMdiChild).bsEmp = new BindingSource();
            /// Настраиваем отображение данных
            ((Table)this.ActiveMdiChild).bsEmp.DataSource = ((Table)this.ActiveMdiChild).dtEmp;
            ((Table)this.ActiveMdiChild).dgEmp.SelectionChanged +=
                new EventHandler(((Table)this.ActiveMdiChild).dgEmp_SelectionChanged);
            ((Table)this.ActiveMdiChild).dgEmp.DataSource = ((Table)this.ActiveMdiChild).bsEmp;
        }

        /// <summary>
        /// Нажатие кнопки формирование рабочего наряда на выходные дни
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btWorkOrder_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по табелю
            if (MessageBox.Show("Вы действительно хотите сформировать отчет за\n" +
                ((Table)this.ActiveMdiChild).BeginDate.Month + " месяц " +
                ((Table)this.ActiveMdiChild).BeginDate.Year + " года?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepWorkOrder(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog(); 
            }
        }

        /// <summary>
        /// Формирование рабочего наряда на выходные дни
        /// </summary>
        /// <param name="data"></param>
        void RepWorkOrder(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectWorkOrder.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                ((Table)this.ActiveMdiChild).BeginDate;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(50);
            if (dtProtocol.Rows.Count == 0)
            {
                MessageBox.Show("В подразделении за указанный месяц данные отсутствуют.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    WExcel.Application m_ExcelApp;
                    //Создание книги Excel
                    WExcel._Workbook m_Book;
                    //Создание страницы книги Excel
                    WExcel._Worksheet m_Sheet;
                    //private Excel.Range Range;
                    WExcel.Workbooks m_Books;
                    object oMissing = System.Reflection.Missing.Value;
                    m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    m_ExcelApp.Visible = false;
                    m_Books = m_ExcelApp.Workbooks;
                    string PathOfTemplate = Application.StartupPath + @"\Reports\WorkOrder.xlt";
                    m_Book = m_Books.Open(PathOfTemplate, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                    m_ExcelApp.ScreenUpdating = false;
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                    ExcelParameter[] excelPars;
                    Microsoft.Office.Interop.Excel.Range range_Title = m_Sheet.get_Range("1:6", Type.Missing);
                    Microsoft.Office.Interop.Excel.Range r_cur = null;
                    int rowNow = 1;
                    int rowIns = 0;
                    string st_key = dtProtocol.Rows[0]["WORK_DATE"].ToString() +
                        dtProtocol.Rows[0]["NGM"].ToString() + dtProtocol.Rows[0]["CODE_DEGREE"].ToString();
                    object[,] strWork = new object[dtProtocol.Rows.Count, 13];
                    for (int row = 0; row < dtProtocol.Rows.Count; row++)
                    {
                        ((BackgroundWorker)sender).ReportProgress((row * 40 / dtProtocol.Rows.Count) + 50);
                        if (rowNow != 1)
                        {
                            range_Title.Rows.Copy(m_Sheet.get_Range(
                                string.Format("{0}:{1}", rowNow, rowNow + 6), Type.Missing));
                        }
                        excelPars = new ExcelParameter[] {
                            new ExcelParameter("A"+(rowNow+1), Convert.ToDateTime(dtProtocol.Rows[row]["WORK_DATE"]).ToShortDateString()), 
                            new ExcelParameter("B"+(rowNow+1), dtProtocol.Rows[row]["CODE_SUBDIV"].ToString()), 
                            new ExcelParameter("C"+(rowNow+1), dtProtocol.Rows[row]["NGM"].ToString()),
                            new ExcelParameter("D"+(rowNow+1), dtProtocol.Rows[row]["CODE_DEGREE"].ToString())};
                        foreach (ExcelParameter parameter in excelPars)
                        {
                            m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Value2 = parameter.Value;
                        }
                        try
                        {
                            do
                            {
                                for (int column = 0; column < 13; column++)
                                {
                                    strWork[rowIns, column] = dtProtocol.Rows[row][column + 4];
                                }
                                rowIns++;
                                st_key = dtProtocol.Rows[row]["WORK_DATE"].ToString() +
                                    dtProtocol.Rows[row]["NGM"].ToString() + dtProtocol.Rows[row]["CODE_DEGREE"].ToString();
                                row++;
                            }
                            while (st_key == dtProtocol.Rows[row]["WORK_DATE"].ToString() +
                                dtProtocol.Rows[row]["NGM"].ToString() + dtProtocol.Rows[row]["CODE_DEGREE"].ToString());
                        }
                        catch { }
                        row--;
                        rowNow += 6;
                        r_cur = m_Sheet.get_Range(string.Format("C{0}", rowNow),
                            string.Format("O{0}", rowNow + rowIns - 1));
                        r_cur.set_Value(Type.Missing, strWork);
                        r_cur = m_Sheet.get_Range(string.Format("A{0}", rowNow),
                            string.Format("P{0}", rowNow + rowIns - 1));
                        r_cur.Borders.LineStyle = WExcel.XlLineStyle.xlContinuous;
                        rowNow += rowIns;
                        m_Sheet.HPageBreaks.Add(m_Sheet.get_Range(string.Format("{0}:{0}", rowNow), Type.Missing));
                        /// Увеличиваем номер строки в отчете на 4, 
                        /// чтобы следующий работник вставился на нужные строки

                        /// Копируем строки по адресам ячеек    
                        rowIns = 0;

                    };

                    m_ExcelApp.ScreenUpdating = true;
                    m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                    m_ExcelApp.DisplayAlerts = false;
                    m_ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                    m_ExcelApp.ActiveWindow.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                    m_ExcelApp.Visible = true;

                }
                finally
                {
                    //Что бы там ни было вызываем сборщик мусора
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }
        
        /// <summary>
        /// Отчет по административным отпускам для группы табельного учета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepOnAdministrVac_Click(object sender, EventArgs e)
        {
            RepOnAdministrVac _repOnAdministrVac = new RepOnAdministrVac();
            _repOnAdministrVac.ShowDialog();
        }

        /// <summary>
        /// Отчет по временной нетрудоспособности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepOnHospital_Click(object sender, EventArgs e)
        {
            RepOnHospital _repOnHospital = new RepOnHospital();
            _repOnHospital.ShowDialog();
        }

        /// <summary>
        /// Отчет по отпуску по беременности и родам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepOnPregnancy_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepOnPregnancy.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 501;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod.Subdiv_ID;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                        new ExcelParameter("A1", "ОТЧЕТ по ОТПУСКУ ПО БЕРЕМЕННОСТИ И РОДАМ"),
                        new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " + 
                            selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepOnChildCare.xlt", "A5", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по отпуску по уходу за ребенком до 3-х лет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepOnChildCare_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepOnChildCare.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                adapter.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal).Value = 532;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod.Subdiv_ID;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ExcelParameter[] excelParameters = new ExcelParameter[] {
                        new ExcelParameter("A1", "ОТЧЕТ по ОТПУСКУ ПО УХОДУ ЗА РЕБЕНКОМ ДО 3-Х ЛЕТ"),
                        new ExcelParameter("A2", "за период с " + selPeriod.BeginDate.ToShortDateString() + " по " + 
                            selPeriod.EndDate.ToShortDateString())};
                    Excel.PrintWithBorder(true, "RepOnChildCare.xlt", "A5", new DataTable[] { dtProtocol }, excelParameters);
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Формирование отчета для отдела 3 по опозданиям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepLateness_Click(object sender, EventArgs e)
        {
            ReportForEconDev reportDocs = new ReportForEconDev(
                "SelectRepLateness.sql", "Lateness.xlt");
            reportDocs.ShowDialog();
        }

        /// <summary>
        /// Формирование отчета для отдела 3 по работе за территорией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepWorkOut_Click(object sender, EventArgs e)
        {
            ReportForEconDev reportDocs = new ReportForEconDev(
                "SelectWorkOut.sql", "WorkOut.xlt");
            reportDocs.ShowDialog();
        }

        /// <summary>
        /// Формирование отчета по оправдательным документам сразу по заводу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepDocsOnPay_Type_Click(object sender, EventArgs e)
        {
            ReportDocsOnPay_Type reportDocs = new ReportDocsOnPay_Type(
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                0, "");
            reportDocs.ShowDialog();
        }


        /// <summary>
        /// Формирование отчета о доступных часах отгулов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepAbsenceOnSubdiv_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name.ToUpper() != "TABLE")
            {
                MessageBox.Show("Необходимо активировать форму табеля для формирования отчета!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Запрос, нужно ли формировать отчет по опоздавшим
            if (MessageBox.Show("Вы действительно хотите сформировать отчет по отгулам?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /// Вставляем табельные номера
                InsertPerNum();
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    AbsenceOnSubdiv(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        ///  Формирование отчета по отгулам
        /// </summary>
        /// <param name="data"></param>
        void AbsenceOnSubdiv(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            /// Создаем адаптер и заполняем с помощью него данные
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectAbsenceOnSubdiv.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = subdiv_id;
            adapter.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                ((Table)this.ActiveMdiChild).EndDate;
            ((BackgroundWorker)sender).ReportProgress(10);
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);
            ((BackgroundWorker)sender).ReportProgress(20);
            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", "по подразделению " + code_subdiv + " на " + 
                        DateTime.Today.ToShortDateString() + " г.")};
                Excel.PrintWithBorder(true, "RepAbsenceOnSubdiv.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters, null, true);
            }
            else
            {
                MessageBox.Show("В подразделении нет данных.",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Отчет по среднесписочной численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepAverage_Number_Click(object sender, EventArgs e)
        {
            ReportClasses.RepAverage_Number();
        }

        /// <summary>
        /// Отчет по списочной численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepListEmp_Number_Click(object sender, EventArgs e)
        {
            ReportClasses.RepListEmp_Number();
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по месяцам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepUseOfWorkTime_Click(object sender, EventArgs e)
        {
            ReportClasses.RepUseOfWorkTime();
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по подразделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepUseOfWorkTimeOnSub_Click(object sender, EventArgs e)
        {
            ReportClasses.RepUseOfWorkTimeOnSub();
        }

        /// <summary>
        /// Создание отчета Сводный отчет по неотработанному времени и ССЧ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepTimeNotWorker_Click(object sender, EventArgs e)
        {
            ReportClasses.RepTimeNotWorker();
        }

        /// <summary>
        /// Отработанные часы 102 ш.о. по заказам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepHoursByOrdersPlant_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(false, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(
                    Queries.GetQuery("Table/SelectRepHoursByOrdersPlant.sql"),
                    Connect.Schema, Connect.SchemaSalary);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value =
                    selPeriod.BeginDate;
                adapter.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value =
                    selPeriod.EndDate;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    Excel.PrintWithBorder(true, "RepHoursByOrdersPlant.xlt", "A6",
                            new DataTable[] { dtProtocol },
                            new ExcelParameter[] {new ExcelParameter("A2", "за период с" + 
                        selPeriod.BeginDate.ToShortDateString() +
                        " по " + selPeriod.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        /// <summary>
        /// Отчет по неотработанному рабочему времени в связи с переходом на режим неполного рабочего времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRep_Pay_Type_545_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod = new SelPeriod_Subdiv(true, true, false);
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == DialogResult.OK)
            {
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepPay_Type_By_Period.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                daReport.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                daReport.SelectCommand.Parameters.Add("p_CODE_PAY_TYPE", OracleDbType.Varchar2).Value = "545";
                daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = selPeriod.Subdiv_ID;
                DataTable dt = new DataTable();
                daReport.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Отчет по 545 шифру оплат", "Reports/RepPay_Type_545.rdlc",
                    //        new DataTable[] { dt },
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                    //    );
                    //report.Show();
                    ReportViewerWindow.RenderToExcel(this, "RepPay_Type_545.rdlc",
                            new DataTable[] { dt },
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                                new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                        );
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет об использовании сверхурочного рабочего времени (новый отчет по лимитам)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepAll_Limits_By_Subdiv_Click(object sender, EventArgs e)
        {
            selPeriod_Subdiv = new SelPeriod_Subdiv(true, false, false, true);
            selPeriod_Subdiv.ShowInTaskbar = false;
            if (selPeriod_Subdiv.ShowDialog() == DialogResult.OK)
            {
                /// Создаем адаптер и заполняем с помощью него данные
                OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
                adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/RepAll_Limits_By_Subdiv.sql"),
                    Connect.Schema);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = selPeriod_Subdiv.Subdiv_ID == 0 ? -1 : selPeriod_Subdiv.Subdiv_ID;
                adapter.SelectCommand.Parameters.Add("p_year", OracleDbType.Decimal).Value = selPeriod_Subdiv.Year;
                DataTable dtProtocol = new DataTable();
                adapter.Fill(dtProtocol);
                if (dtProtocol.Rows.Count > 0)
                {
                    ReportViewerWindow.RenderToExcel(this, "RepAll_Limits_By_Subdiv.rdlc",
                            new DataTable[] { dtProtocol },
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", selPeriod_Subdiv.Year.ToString())}
                        );
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Создание отчета Отчет по неявкам (подразделений)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepTimeNotWorkerOnSub_Click(object sender, EventArgs e)
        {
            ReportClasses.RepTimeNotWorkerOnSub();
        }

        /// <summary>
        /// Справка по среднесписочной численности для группы личного стола отдела кадров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepAverage_Number_Personnel_Click(object sender, EventArgs e)
        {
            ReportClasses.RepAverage_Number_Personnel();
        }

        /// <summary>
        /// Отчет по прогульщикам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrderTruancy_Click(object sender, EventArgs e)
        {
            OrderTruancy orderTruancy = new OrderTruancy();
            orderTruancy.ShowDialog();
        }

        /// <summary>
        /// Отчет по совмещениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btCombReplReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\SelectPayType153Rep.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, Table.CurrentActivForm.subdiv_id, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, Table.CurrentActivForm.BeginDate, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, Table.CurrentActivForm.EndDate, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add(":c", OracleDbType.RefCursor, ParameterDirection.Output);
                a.Fill(t);
                Excel.PrintWithBorder("SelectPayType153Rep.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("C2", Table.CurrentActivForm.code_subdiv), new ExcelParameter("F2", Table.CurrentActivForm.BeginDate.ToString("MMMM yyyy") + " г.") });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }


        /// <summary>
        /// Формирование отчета по среднесписочной численности в разрезе подразделений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepAverage_Number_On_Plant_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    PrintRepAvg(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
            }
        }

        /// <summary>
        /// Среднесписочная численность в разрезе подразделений
        /// </summary>
        /// <param name="backgroundWorker"></param>
        /// <param name="e1"></param>
        private void PrintRepAvg(BackgroundWorker backgroundWorker, DoWorkEventArgs e1)
        {
            backgroundWorker.ReportProgress(0);
            try
            {
                WExcel.Application m_ExcelApp;
                //Создание книги Excel
                WExcel._Workbook m_Book;
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                //private Excel.Range Range;
                WExcel.Workbooks m_Books;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                m_ExcelApp.Visible = false;
                m_Books = m_ExcelApp.Workbooks;
                backgroundWorker.ReportProgress(5);

                DataSet _ds = new DataSet();
                _ds.Tables.Add("LIST_SUBDIV");
                _ds.Tables.Add("REPORT");
                OracleDataAdapter _daSubdiv = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/SelectListSubdiv.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daSubdiv.SelectCommand.BindByName = true;
                _daSubdiv.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daSubdiv.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                _daSubdiv.Fill(_ds, "LIST_SUBDIV");
                OracleDataAdapter _daData = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number_On_Subdiv.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daData.SelectCommand.BindByName = true;
                _daData.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daData.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = selPeriod.EndDate;
                _daData.SelectCommand.Parameters.Add("p_TABLE_ID", OracleDbType.Array).UdtTypeName =
                    Connect.Schema.ToUpper() + ".TYPE_TABLE_NUMBER";
                OracleDataAdapter _daCountDoc = new OracleDataAdapter(string.Format(
                    Queries.GetQuery("Table/SelectCountEmp_On_Doc.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                _daCountDoc.SelectCommand.BindByName = true;
                _daCountDoc.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daCountDoc.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                _daCountDoc.SelectCommand.Parameters.Add("p_pay_type_id", OracleDbType.Decimal);

                m_Book = m_Books.Open(Application.StartupPath + @"\Reports\RepAverage_Number_On_Plant.xlt", oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[1];
                m_Sheet.Name = "Завод";
                _daData.SelectCommand.Parameters["p_TABLE_ID"].Value =
                    _ds.Tables["LIST_SUBDIV"].DefaultView.Cast<DataRowView>().Select(i => i.Row.Field<Decimal>("SUBDIV_ID")).ToArray(); 
                _ds.Tables["REPORT"].Clear();
                _daData.Fill(_ds.Tables["REPORT"]);
                if (_ds.Tables["REPORT"].Rows.Count > 0)
                {
                    object[,] val1 = new object[6, 12];
                    // Обрабатываем 01 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 1";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val1[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F2", "Q7").set_Value(Type.Missing, val1);
                    // Обрабатываем 08 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 8";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val1[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F8", "Q13").set_Value(Type.Missing, val1);
                    object[,] val2 = new object[3, 12];
                    // Обрабатываем 02 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 2";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F14", "Q16").set_Value(Type.Missing, val2);
                    // Обрабатываем 09 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 9";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F17", "Q19").set_Value(Type.Missing, val2);
                    object[,] val3 = new object[12, 12];
                    // Обрабатываем 04 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 4";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val3[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F23", "Q34").set_Value(Type.Missing, val3);
                    // Обрабатываем 05 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 5";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F35", "Q37").set_Value(Type.Missing, val2);
                    // Обрабатываем 07 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 7";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F38", "Q40").set_Value(Type.Missing, val2);
                    // Обрабатываем 11 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 11";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F41", "Q43").set_Value(Type.Missing, val2);
                    // Обрабатываем 12 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 12";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F44", "Q46").set_Value(Type.Missing, val2);
                    // Обрабатываем 13 кат.
                    _ds.Tables["REPORT"].DefaultView.RowFilter = "DEGREE_ID = 13";
                    for (int i = 0; i < _ds.Tables["REPORT"].DefaultView.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val2[i, j - 3] = _ds.Tables["REPORT"].DefaultView[i][j];
                        }
                    }
                    m_Sheet.get_Range("F50", "Q52").set_Value(Type.Missing, val2);

                    object[,] val4 = new object[1, 17];
                    _ds.Tables.Add("COUNT_DOC");
                    _daCountDoc.SelectCommand.Parameters["p_pay_type_id"].Value = 501;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F64", "V64").set_Value(Type.Missing, val4);
                    _ds.Tables["COUNT_DOC"].Clear();
                    _daCountDoc.SelectCommand.Parameters["p_pay_type_id"].Value = 532;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F65", "V65").set_Value(Type.Missing, val4);
                    _ds.Tables["COUNT_DOC"].Clear();
                    _daCountDoc.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Table/SelectCountComb_By_Month.sql"),
                        Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                    _daCountDoc.SelectCommand.BindByName = true;
                    _daCountDoc.SelectCommand.Parameters.Add("p_beginDate", OracleDbType.Date).Value = selPeriod.BeginDate;
                    _daCountDoc.SelectCommand.Parameters.Add("p_endDate", OracleDbType.Date).Value = selPeriod.EndDate;
                    _daCountDoc.Fill(_ds.Tables["COUNT_DOC"]);
                    for (int i = 0; i < _ds.Tables["COUNT_DOC"].Rows.Count; i++)
                    {
                        for (int j = 0; j < _ds.Tables["COUNT_DOC"].Columns.Count; j++)
                        {
                            val4[i, j] = _ds.Tables["COUNT_DOC"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F66", "V66").set_Value(Type.Missing, val4);
                }

                int numSheet = 2;
                object[,] val = new object[48, 12];

                backgroundWorker.ReportProgress(10);
                /*m_ExcelApp.Visible = true; 
                    */
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                numSheet = 2;
                foreach (DataRow rowSub in _ds.Tables["LIST_SUBDIV"].Rows)
                {
                    // Копируем лист шаблона перед началом работы
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Copy(Type.Missing, m_ExcelApp.Sheets[numSheet++]);
                    ((WExcel._Worksheet)m_ExcelApp.Sheets[numSheet]).Select();
                    // Задаем рабочую облаcть - новый рабочий лист
                    m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[numSheet];
                    m_Sheet.Name = rowSub["CODE_SUBDIV"].ToString() + (rowSub["SUB_ACTUAL_SIGN"].ToString() == "1" ? "" : "(не актуально)");
                    _daData.SelectCommand.Parameters["p_TABLE_ID"].Value = new decimal[] { Convert.ToDecimal(rowSub["SUBDIV_ID"]) };
                    _ds.Tables["REPORT"].Clear();
                    _daData.Fill(_ds.Tables["REPORT"]);
                    for (int i = 0; i < _ds.Tables["REPORT"].Rows.Count; i++)
                    {
                        for (int j = 3; j < _ds.Tables["REPORT"].Columns.Count; j++)
                        {
                            val[i, j - 3] = _ds.Tables["REPORT"].Rows[i][j];
                        }
                    }
                    m_Sheet.get_Range("F2", "Q49").set_Value(Type.Missing, val);
                    backgroundWorker.ReportProgress(90 * (numSheet - 1) / _ds.Tables["LIST_SUBDIV"].Rows.Count + 10);
                    //break;
                }

                m_ExcelApp.DisplayAlerts = false;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Select();
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).get_Range("A1");
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Delete();
                m_ExcelApp.DisplayAlerts = true;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.ScreenUpdating = true;
                /*((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Visible =
                    Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                ((WExcel._Worksheet)m_ExcelApp.Sheets[2]).Select();*/
                /*m_Sheet.get_Range("A1", Type.Missing).Select();
                m_Sheet.Protect("258", Boolean.TrueString, Boolean.TrueString, Boolean.TrueString, Type.Missing, Boolean.TrueString, Boolean.TrueString, Boolean.TrueString,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);*/
                ((WExcel._Worksheet)m_ExcelApp.Sheets[1]).Select();
                m_ExcelApp.Visible = true;

                //if (!GrantedRoles.GetGrantedRole("TABLE_FORM_FILE"))
                //{
                //    // Начинаем цикл по листам книги со 2 листа (1 - шаблон, который скрыт)
                //    for (int i = 1; i < numSheet; i++)
                //    {
                //        m_Sheet = (WExcel._Worksheet)m_ExcelApp.Sheets[i];
                //        m_Sheet.PrintPreview(true);
                //    }
                //    m_ExcelApp.Quit();
                //}
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Формирование отчета по отсутствию (добавлен 15.12.2015)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepFailureToAppear_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            //selPeriod.BeginDate = new DateTime(2016, 1, 1);
            //selPeriod.EndDate = new DateTime(2016, 1, 31);
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet _dsReport = new DataSet();
                _dsReport.Tables.Add("Table1");
                _dsReport.Tables.Add("Table2");
                _dsReport.Tables.Add("Table3");
                _dsReport.Tables.Add("Table4");
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureToAppear.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table1"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureToAppearOnDoc.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table2"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Entry_Chief.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table3"]);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepFailureTotalBySubdiv.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_BEGIN_PERIOD", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_END_PERIOD", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsReport.Tables["Table4"]);
                if (_dsReport.Tables["Table1"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Отчет по отсутствию", "Reports/RepFailureToAppear.rdlc",
                    //        new DataTable[] { _dsReport.Tables["Table1"], _dsReport.Tables["Table2"], _dsReport.Tables["Table3"], _dsReport.Tables["Table4"] },
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //        new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                    //        new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())}
                    //    );
                    //report.Show();

                    ReportViewerWindow.RenderToExcelWithFormulas(this, "RepFailureToAppear.rdlc",
                        new DataTable[] { _dsReport.Tables["Table1"], _dsReport.Tables["Table2"], _dsReport.Tables["Table3"], _dsReport.Tables["Table4"] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGIN_PERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_END_PERIOD", selPeriod.EndDate.ToShortDateString())});                    
                }
                else
                {
                    MessageBox.Show("За указанный период нет данных.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Формирование отчета Поздний выход (добавлен 06.02.2016)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepLate_Pass_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            //selPeriod.BeginDate = new DateTime(2016, 1, 1);
            //selPeriod.EndDate = new DateTime(2016, 1, 31);
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _dsTempForReport = new DataSet();
                _dsTempForReport.Tables.Add("Table1");
                _dsTempForReport.Tables.Add("Table2");
                timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    RepLate_Pass(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();
                if (_dsTempForReport.Tables["Table1"].Rows.Count > 0 || _dsTempForReport.Tables["Table2"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Поздний выход", "Reports/RepLate_Pass.rdlc",
                    //        _dsTempForReport,
                    //        null
                    //    );
                    //report.Show();

                    ReportViewerWindow.RenderToExcel(this, "RepLate_Pass.rdlc",
                        new DataTable[] { _dsTempForReport.Tables["Table1"], _dsTempForReport.Tables["Table2"] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE_BEGIN", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE_END", selPeriod.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("За указанный период нет данных.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }               
            }            
        }

        /// <summary>
        /// Формирование табеля
        /// </summary>
        /// <param name="data"></param>
        void RepLate_Pass(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            try
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Pass.sql"),
                    Connect.Schema, "PERCO"), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_BEGIN", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_DATE_END", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsTempForReport.Tables["Table1"]);
                ((BackgroundWorker)sender).ReportProgress(50);
                _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/RepLate_Pass_Holiday.sql"),
                    Connect.Schema, "PERCO"), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_DATE_BEGIN", OracleDbType.Date).Value = selPeriod.BeginDate;
                _daReport.SelectCommand.Parameters.Add("p_DATE_END", OracleDbType.Date).Value = selPeriod.EndDate;
                _daReport.Fill(_dsTempForReport.Tables["Table2"]);
                ((BackgroundWorker)sender).ReportProgress(80);
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        #endregion
        
        #endregion

    #region Работа с базой проектов электронных переводов

        /// <summary>
        /// Обменная база проектов приказов о приеме, переводе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBase_Exchange_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_PROJECT").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "TRANSFER_PROJECT").First().Activate();
                return;
            }
            Wpf_Control_Viewer trans_proj = new Wpf_Control_Viewer();
            trans_proj.Name = "TRANSFER_PROJECT";
            trans_proj.Text = "Список проектов приказов";
            Transfer_Project_Viewer trans_proj_Viewer = new Transfer_Project_Viewer();
            trans_proj_Viewer.ParentWinForm = this;
            trans_proj.elementHost1.Child = trans_proj_Viewer;
            trans_proj.MdiParent = this;
            trans_proj.WindowState = FormWindowState.Maximized;
            trans_proj.Show();
        }

        /// <summary>
        /// Справочник типов условий труда
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btType_Condition_Click(object sender, EventArgs e)
        {
            Type_Condition_Viewer type_Condition_Viewer = new Type_Condition_Viewer();
            WindowInteropHelper wih = new WindowInteropHelper(type_Condition_Viewer);
            wih.Owner = this.Handle;
            type_Condition_Viewer.ShowDialog();
        }

        /// <summary>
        /// База заявлений о переводе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProject_Statement_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "PROJECT_STATEMENT").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "PROJECT_STATEMENT").First().Activate();
                return;
            }
            Wpf_Control_Viewer trans_proj = new Wpf_Control_Viewer();
            trans_proj.Name = "PROJECT_STATEMENT";
            trans_proj.Text = "Список проектов заявлений о переводе";
            Project_Statement_Viewer proj_Statement_Viewer = new Project_Statement_Viewer();
            proj_Statement_Viewer.ParentWinForm = this;
            trans_proj.elementHost1.Child = proj_Statement_Viewer;
            trans_proj.MdiParent = this;
            trans_proj.WindowState = FormWindowState.Maximized;
            trans_proj.Show();
        }   

    #endregion

#region Работа с графиками отпусков

        private void btViewArchivVS_Click(object sender, EventArgs e)
        {
            
            btArchivVS.Pressed = true;
            for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is Vacation_schedule.ArchivVac)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            Vacation_schedule.ArchivVac frmViewVS = new Vacation_schedule.ArchivVac();
            frmViewVS.MdiParent = this;
            frmViewVS.Show();
            rgFilterVS.Visible = true;
        }

        private void btMakeVS_Click(object sender, EventArgs e)
        {
            btMakeVS.Pressed = true;
            rgFilterVS.Visible = true;
            for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is Vacation_schedule.MakeVS)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            Vacation_schedule.MakeVS frmFormVS = new Vacation_schedule.MakeVS();
            frmFormVS.MdiParent = this;
            frmFormVS.Show();
            cbDegreeVS_TextChanged(this, null);
        }

        private void btConfirmVS_Click(object sender, EventArgs e)
        {
            btMakePlanVS.Pressed = true;
            rgFilterVS.Visible = true;
            for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is Vacation_schedule.ConfirmVS)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            Vacation_schedule.ConfirmVS frmConfirmVS = new Kadr.Vacation_schedule.ConfirmVS();
            frmConfirmVS.MdiParent = this;
            frmConfirmVS.Show();
        }

        private void btViewEmpLocationVac_Click(object sender, EventArgs e)
        {
            btViewEmpLocationVac.Pressed = true;
            rgFilterVS.Visible = true;
            for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is Vacation_schedule.EmpSubLocation)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            Vacation_schedule.EmpSubLocation frm = new Kadr.Vacation_schedule.EmpSubLocation();
            frm.MdiParent = this;
            frm.Show();
        }   

        private void btDiagramsVS_Click(object sender, EventArgs e)
        {
            btDiagramsVS.Pressed = true;
            for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is Vacation_schedule.DiagramVacS)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            Vacation_schedule.DiagramVacS frmDiagramVS = new Kadr.Vacation_schedule.DiagramVacS();
            frmDiagramVS.MdiParent = this;
            frmDiagramVS.Show();
            rgFilterVS.Visible = true;
        }

        public void cbDegreeVS_TextChanged(object sender, EventArgs e)
        {
            if (sender == tbPerNumVS)
                FilterVS.per_num = tbPerNumVS.Text != "" ? tbPerNumVS.Text.Trim().PadLeft(5, '0') : "";
            else if (sender == nupdYearVS)
                FilterVS.YearVS = (int)nupdYearVS.Value;
            else if (sender == subdivSelectorVSFilter)
                FilterVS.subdiv_id = subdivSelectorVSFilter.subdiv_id==null ? -1m : subdivSelectorVSFilter.subdiv_id.Value;
        }

        private void nupdYearVS_ValueChanged(object sender, EventArgs e)
        {           
            if (((System.Windows.Forms.Control)sender).Focused)
                cbDegreeVS_TextChanged(this, EventArgs.Empty);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            DataSet t = new DataSet();
            new OracleDataAdapter(string.Format("select degree_id,Degree_name from {0}.degree order by 1", Connect.Schema), Connect.CurConnect).Fill(t);
            t.Tables[0].Rows.Add(-1, "Все");
            nupdYearVS.Value = DateTime.Now.Year;
            subdivSelectorVSFilter.SubdivChanged += new EventHandler(cbDegreeVS_TextChanged);
            nupdYearVS.ValueChanged += new EventHandler(cbDegreeVS_TextChanged);
            tbPerNumVS.KeyUp += new KeyEventHandler(VacEvents.PerNumKeyUp);
            bt4.Click += new EventHandler(VacEvents.PerNumChanging);
            // Перенес установку языка ввода в метод Load из конструктора формы, потому что в 64-х разрядной системе язык не переключается
            Application.CurrentInputLanguage =
                InputLanguage.FromCulture(new CultureInfo("ru-RU"));
        }

        #region ОТЧЕТЫ ГРАФИКОВ

        private void R_btPrikazZavodVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod fGet = new Kadr.Vacation_schedule.GetPeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1));
            fGet.vacPeriodForm1.Model.IsDegreeEnabled = true;
            if (fGet.ShowDialog(this) == DialogResult.OK && fGet.SelectedVacIDs.Count > 0)
            {
                DataTable t = new DataTable();
                new OracleDataAdapter(string.Format("select vac_consist_id from {0}.vac_consist where vac_sched_id in ({1})", Connect.Schema, string.Join(",", fGet.SelectedVacIDs.ToArray())), Connect.CurConnect).Fill(t);
                Vacation_schedule.ViewCard.PrintOrderPlantReport(this, FilterVS.subdiv_id, string.Join(",", t.Rows.OfType<DataRow>().Select(r => r[0].ToString()).ToArray()));
            }
        }

        private void R_btSubPrikazVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod frmGetPer = new Kadr.Vacation_schedule.GetPeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1));
            string[][] s_pos = new string[][] { };
            frmGetPer.vacPeriodForm1.Model.IsDegreeEnabled = true;
            if (frmGetPer.ShowDialog(this) == DialogResult.OK && frmGetPer.SelectedVacIDs.Count > 0 && Vacation_schedule.Signes.Show(this, FilterVS.subdiv_id, "OrderPlantSubdivVS", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos) == DialogResult.OK)
            {
                OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("GO/OrderSubdivReport.sql"), Connect.Schema, string.Join(",", frmGetPer.SelectedVacIDs.ToArray())), Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("subd_id", frmGetPer.SubdivID);
                DataTable table = new DataTable();
                adapter.Fill(table);
                this.Refresh();
                Excel.PrintWithBorder("VSOrderSubdiv.xlt", "A12", new DataTable[] { table }, new ExcelParameter[] 
                {
                    new ExcelParameter("B2",string.Format("{0}",frmGetPer.CodeSubdiv),null),
                    new ExcelParameter("A3",string.Format("\"{0}\" {1} {2}г.",DateTime.Today.Day,Library.MyMonthName(DateTime.Today.Month),DateTime.Today.Year),null),
                    new ExcelParameter("B4","г.Улан-Удэ",null),
                    new ExcelParameter(MExcel.XlHAlign.xlHAlignLeft, new Point(1,table.Rows.Count+13),new Point(1,table.Rows.Count+13),"Руководитель подразделения"),
                    new ExcelParameter(new Point(2,table.Rows.Count+12),new Point(5,table.Rows.Count+13),s_pos[0][0],new MExcel.XlBordersIndex[]{MExcel.XlBordersIndex.xlEdgeBottom}),
                    new ExcelParameter(new Point(6,table.Rows.Count+13),new Point(7,table.Rows.Count+13),"_______________"),
                    new ExcelParameter(new Point(9,table.Rows.Count+13),new Point(12,table.Rows.Count+13),s_pos[0][1],new MExcel.XlBordersIndex[]{MExcel.XlBordersIndex.xlEdgeBottom})
                });
            }
        }

        private void R_btCountVSOnDate_Click(object sender, EventArgs e)
        {
            string st = InputBox.Show("Ввод даты", "Введите дату окончания расчетного периода:", DateTime.Now.ToShortDateString(), "00/00/0000");
            if (st.Replace('.', ' ').Trim() != "")
            {
                DataTable table = new DataTable();
                OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("go/R_btCountVSOnDate.SQL"), Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_date", OracleDbType.Date, DateTime.Parse(st), ParameterDirection.Input);
                cmd.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
                try
                {
                    new OracleDataAdapter(cmd).Fill(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка получения данных");
                }
                Excel.PrintWithBorder(true, "R_btCountVSOnDate.xlt", "A4", new DataTable[] { table },
                    new ExcelParameter[] { new ExcelParameter("A1", string.Format("Отчет по неиспользованным отпускам на {0} г.", st)) });
            }
        }


        private void R_CountPlanSumDaysVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod frm = new Kadr.Vacation_schedule.GetPeriod(new DateTime(DateTime.Today.Year, 1, 1).AddYears(1), new DateTime(DateTime.Today.Year, 1, 1).AddYears(2), true);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery("go/R_CountPlanSumDaysVS.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date, frm.DateBegin.Date, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date, frm.DateEnd.Date, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.SubdivID, ParameterDirection.Input);
                a.Fill(t);
                Excel.PrintWithBorder(true, "R_CountPlanSumDaysVS.xlt", "A4", new DataTable[] { t }, new ExcelParameter[]
                {
                    new ExcelParameter("A1",string.Format("Отчет по планируемым отпускам в период с {0} по {1}",frm.DateBegin.ToShortDateString(),frm.DateEnd.ToShortDateString()))
                });
            }
        }

        private void R_btActualDatesByPeriod_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod f = new Kadr.Vacation_schedule.GetPeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1), true);
            if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                DataTable t = new DataTable();
                new OracleDataAdapter(string.Format(Queries.GetQuery("go/R_btActualDateByPeriod.sql"), Connect.Schema,
                    f.SubdivID,
                    f.DateBegin.ToString("yyy-MM-dd"),
                    f.DateEnd.ToString("yyyy-MM-dd")), Connect.CurConnect).Fill(t);
                Excel.PrintWithBorder(true, "R_btActualDateByPeriod.xlt", "A4", new DataTable[] { t },
                    new ExcelParameter[]
                    {
                        new ExcelParameter("A2",string.Format("в период с {0} по {1}",f.DateBegin.ToShortDateString(),f.DateEnd.AddDays(-1).ToShortDateString()))
                    });
            }
        }

        #endregion КОНЕЦ ОТЧЕТОВ ГРАФИКОВ

#endregion              
            
        #region RAS, два, три... АРМ "БУХГАЛТЕРА!"

        /// <summary>
        /// Список работников в АРМ бухгалтера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDataFill_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name == "grid_ras").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name == "grid_ras").First().Activate();
                return;
            }
            grid_form = new grid_ras(this);
            grid_form.MdiParent = this;
            grid_form.Name = "grid_ras";
            CreateButtonApp(grid_form, sender);
            grid_form.Show();
            this.rgDataFill.Visible = true;
            this.rgFilterRas.Visible = true;
            this.rgUpdateRas.Visible = true;
        }

        /// <summary>
        /// Фильтр работников в АРМ бухгалтера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilterRas_Click(object sender, EventArgs e)
        {
            filter filt = new filter(this, grid_form.bs);
            filt.Show();
        }

        /// <summary>
        /// Поиск работников в АРМ бухгалтера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFindRas_Click(object sender, EventArgs e)
        {
            find_spr find = new find_spr(this, grid_form.bs, grid_form);
            find.Show();
        }

        /// <summary>
        /// Редактирование бухгалтерских данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btUpdateRas_Click(object sender, EventArgs e)
        {
            /// Если стоим на какой-то записи, то выводим ее для редактирования
            if (grid_form.dgEmp.CurrentRow != null)
            {
                red form_red = new red(grid_form,
                    grid_form.dgEmp.CurrentRow.Cells["TYPE_TRANSFER_ID"].Value.ToString() == "3" ?
                    Convert.ToDecimal(grid_form.dgEmp.CurrentRow.Cells["FROM_POSITION"].Value) :
                    Convert.ToDecimal(grid_form.dgEmp.CurrentRow.Cells["transfer_id"].Value),
                    Convert.ToDecimal(grid_form.dgEmp.CurrentRow.Cells["SIGN_COMB"].Value));
                form_red.ShowInTaskbar = false;
                if (form_red.ShowDialog() == DialogResult.OK)
                    grid_form.dgEmp_SelectionChanged(sender, e);
            }
        }

        /// <summary>
        /// Формирование отчета по справочнику работающих
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPrintAD_Click(object sender, EventArgs e)
        {
            PrintAccount_data printAD = new PrintAccount_data();
            printAD.ShowInTaskbar = false;
            printAD.ShowDialog();
        }  

        /// <summary>
        /// Сброс данных на IBM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_sbros_IBM_Click(object sender, EventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == DialogResult.OK)
            {
                // Создаем форму прогресса
                timeExecute = new TimeExecute();
                timeExecute.pbPercentExecute.Style = ProgressBarStyle.Marquee;
                // Настраиваем что он должен выполнять
                timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    DumpFileSpr(timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                timeExecute.ShowDialog();  
            }
        }

        private bool FlagProtokolDump(DateTime _date)
        {
            OracleDataAdapter adapter = new OracleDataAdapter("", Connect.CurConnect);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/ProtocolDump.sql"),
                Connect.Schema);
            adapter.SelectCommand.BindByName = true;
            adapter.SelectCommand.Parameters.Add("p_date_dump", OracleDbType.Date);
            adapter.SelectCommand.Parameters["p_date_dump"].Value = _date;
            DataTable dtProtocol = new DataTable();
            adapter.Fill(dtProtocol);

            if (dtProtocol.Rows.Count > 0)
            {
                ExcelParameter[] excelParameters = new ExcelParameter[] {
                    new ExcelParameter("A3", _date.ToShortDateString())};
                Excel.PrintWithBorder(true, "ProtocolDump.xlt", "A6", new DataTable[] { dtProtocol }, excelParameters);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Сброс данных для справочника работников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DumpFileSpr(object sender, DoWorkEventArgs e)
        {
            if (!FlagProtokolDump(dateDump))
            {
                // ВЫТАСКИВАЮ ДАННЫЕ ПО формированию
                OracleDataTable formir = new OracleDataTable("", Connect.CurConnect);
                formir.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/Dump_AD.sql"),
                    Connect.Schema);
                formir.SelectCommand.Parameters.Add("p_date_dump", OracleDbType.Date).Value = dateDump;
                formir.Fill();

                string fio;
                StreamWriter sw = new StreamWriter(ParVal.Vals["PathFileSpr"] + "SPRRABN.txt", false, Encoding.GetEncoding(866));
                StreamWriter swDop = new StreamWriter(ParVal.Vals["PathFileSpr"] + "SPRDOP.txt", false, Encoding.GetEncoding(866));
                for (int i = 0; i < formir.Rows.Count; i++)
                {
                    string[] str = Array.ConvertAll<object, string>(formir.Rows[i].ItemArray, (el => el.ToString()));
                    fio = string.Join("", str, 0, 34);
                    sw.WriteLine(fio);
                    fio = formir.Rows[i]["SC"].ToString() + formir.Rows[i]["TN"].ToString() +
                        formir.Rows[i]["DROG"].ToString() +
                        formir.Rows[i]["EMP_LAST_NAME"].ToString().PadRight(25, ' ') +
                        formir.Rows[i]["EMP_FIRST_NAME"].ToString().PadRight(15, ' ') +
                        formir.Rows[i]["EMP_MIDDLE_NAME"].ToString().PadRight(15, ' ');
                    swDop.WriteLine(fio);

                }
                sw.Close();
                swDop.Close();

                // Копирование справочника работников в папку табеля
                DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                    dateDump.Year, dateDump.Month.ToString().PadLeft(2, '0')));
                if (!dir.Exists)
                {
                    dir.Create();
                }
                File.Copy(ParVal.Vals["PathFileSpr"] + "SPRRABN.txt", 
                    dir.FullName + string.Format(@"\SPR{0}_{1}.txt", dateDump.Year.ToString(),
                    dateDump.Month.ToString().PadLeft(2, '0')), true);

                formir = new OracleDataTable("", Connect.CurConnect);
                formir.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/SelectEditdep.sql"),
                    Connect.Schema);
                formir.Fill();
                if (formir.Rows.Count > 0)
                {
                    sw = new StreamWriter(ParVal.Vals["PathFileSpr"] + "IZNAL.txt", false, Encoding.GetEncoding(866));
                    for (int i = 0; i < formir.Rows.Count; i++)
                    {
                        string[] str = Array.ConvertAll<object, string>(formir.Rows[i].ItemArray, (el => el.ToString()));
                        sw.WriteLine(string.Join("", str));
                    }
                    sw.Close();
                    File.Copy(ParVal.Vals["PathFileSpr"] + "IZNAL.txt",
                        dir.FullName + string.Format(@"\IZNAL{0}_{1}.txt", dateDump.Year.ToString(),
                        dateDump.Month.ToString().PadLeft(2, '0')), true);
                }
                MessageBox.Show("Файл для расчета заработной платы сформирован!",
                    "АРМ 'Кадры'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RASInitializeButtons()
        {
            R_btRASProfEmp.Click += new EventHandler(R_btRASProfEmp_Click);
            R_btRASNonProfEmp.Click += new EventHandler(R_btRASNonProfEmp_Click);
            //Ras.RasLib.InitEvent(this);
        }

        void R_btRASInvalidEmp_Click(object sender, EventArgs e)
        {
            DateTime tm = DateTime.Now;
            if (InputDataForm.ShowForm(ref tm, "MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter aRep = new OracleDataAdapter(string.Format(Queries.GetQuery("RAS/RASInvalidEmp.sql"), 
                    Connect.Schema), Connect.CurConnect);
                aRep.SelectCommand.Parameters.Add("p_date_print", OracleDbType.Date).Value = tm;
                aRep.Fill(t);
                Excel.PrintWithBorder("RASInvalidEmp.xlt", "A3", new DataTable[] { t }, new ExcelParameter[]{
                    new ExcelParameter("F1",string.Format("{0} месяц {1}г.",tm.ToString("MM"),tm.ToString("yyyy")))});
            }
        }

        void R_btRasCombList_Click(object sender, EventArgs e)
        {
            DateTime tm = DateTime.Now;
            if (InputDataForm.ShowForm(ref tm, "dd MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                new OracleDataAdapter(string.Format(Queries.GetQuery("RAS/CombineEmp.sql"), Connect.Schema, tm.ToString("yyyy-MM-dd")), Connect.CurConnect).Fill(t);
                Excel.PrintWithBorder("RASCombineEmp.xlt", "A3", new DataTable[] { t }, new ExcelParameter[]{
                    new ExcelParameter("E1",tm.ToString("dd-MM-yyyy"))});
            }
        }

        void R_btRASNonProfEmp_Click(object sender, EventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(Queries.GetQuery(@"ras\ProfOrNotEmp.sql"), Connect.Schema, 0), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm;
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников не состоящих в профсоюзе") });
            }
        }

        void R_btRASProfEmp_Click(object sender, EventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(Queries.GetQuery(@"ras\ProfOrNotEmp.sql"), Connect.Schema, 1), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm;
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников состоящих в профсоюзе") });
            }
        }

        private void btProtocolDump_Click(object sender, EventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == DialogResult.OK)
            {
                if (!FlagProtokolDump(dateForm))
                {
                    MessageBox.Show("В БД нет пустых дат на выслугу лет!",
                        "АРМ Бухгалтера",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по переводам в подразделении за период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepTransSub_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(true, true, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == DialogResult.OK)
            {
                DataTable dtRepTransSub = new DataTable();
                OracleDataAdapter odaRepTransSub = new OracleDataAdapter(
                    string.Format(Queries.GetQuery("RAS/SelectRepTransSub.sql"),
                    Connect.Schema),
                    Connect.CurConnect);
                odaRepTransSub.SelectCommand.BindByName = true;
                odaRepTransSub.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod_Subdiv.Subdiv_ID;
                odaRepTransSub.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date).Value =
                    selPeriod_Subdiv.BeginDate;
                odaRepTransSub.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value =
                    selPeriod_Subdiv.EndDate;
                odaRepTransSub.Fill(dtRepTransSub);
                if (dtRepTransSub.Rows.Count > 0)
                {
                    Excel.PrintWithBorder("RepTransSub.xlt", "A5",
                        new DataTable[] { dtRepTransSub },
                        new ExcelParameter[] {new ExcelParameter("A3", "в подразделении " +
                            " с " + selPeriod_Subdiv.BeginDate.ToShortDateString() + " по " +
                            selPeriod_Subdiv.EndDate.ToShortDateString())});
                }
                else
                {
                    MessageBox.Show("В подразделении за указанный период нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Список работников со сроком окончания доп.соглашения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRepEmpContract_Click(object sender, EventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(true, false, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == DialogResult.OK)
            {
                DataTable dtRepEmpContract = new DataTable();
                OracleDataAdapter odaRepEmpContract = new OracleDataAdapter(
                    string.Format(Queries.GetQuery("RAS/SelectRepEmpContract.sql"),
                    Connect.Schema),
                    Connect.CurConnect);
                odaRepEmpContract.SelectCommand.BindByName = true;
                odaRepEmpContract.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value =
                    selPeriod_Subdiv.Subdiv_ID;                
                odaRepEmpContract.Fill(dtRepEmpContract);
                if (dtRepEmpContract.Rows.Count > 0)
                {
                    Excel.PrintWithBorder("RepEmpContract.xlt", "A3", 
                        new DataTable[] { dtRepEmpContract }, null);
                }
                else
                {
                    MessageBox.Show("В подразделении нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отображение формы списка сторонних сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOutside_Emp_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "OUTSIDE_EMP").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "OUTSIDE_EMP").First().Activate();
                return;
            }
            Wpf_Control_Viewer outside_emp = new Wpf_Control_Viewer();
            outside_emp.Name = "OUTSIDE_EMP";
            outside_emp.Text = "Сторонние сотрудники";
            Outside_Emp_Viewer outside_emp_viewer = new Outside_Emp_Viewer(this);
            outside_emp.elementHost1.Child = outside_emp_viewer;
            outside_emp.MdiParent = this;
            outside_emp.WindowState = FormWindowState.Maximized;
            outside_emp.Show();
        }

        /// <summary>
        /// Список вступивших в профсоюз за отчетный период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProfUnion_Entered_Click(object sender, EventArgs e)
        {
            DateTime dateForm = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(
                    Queries.GetQuery(@"ras\SelectRepEnteredProfUnion.sql"), Connect.Schema, Connect.SchemaSalary, "DATE_START_RET"), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm.AddDays(1).AddSeconds(-1);
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников вступивших в профсоюз") });
            }
        }

        /// <summary>
        /// Список вышедщих из профсоюза за отчетный период
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProfUnion_CameOut_Click(object sender, EventArgs e)
        {
            DateTime dateForm = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(
                    Queries.GetQuery(@"ras\SelectRepEnteredProfUnion.sql"), Connect.Schema, Connect.SchemaSalary, "DATE_END_RET"), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm.AddDays(1).AddSeconds(-1);
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников вышедших из профсоюза") });
            }
        }
        
        /// <summary>
        /// Протокол несоответствия месяца даты движения и даты приказа о переводе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProtocolTr_Date_Order_Click(object sender, EventArgs e)
        {
            selPeriod = new SelectPeriod();
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet _dsReport = new DataSet();
                OracleDataAdapter odaReport = new OracleDataAdapter("", Connect.CurConnect);
                odaReport.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/SelectProtocolTr_Date_Order.sql"),
                    Connect.Schema);
                odaReport.SelectCommand.BindByName = true;
                odaReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = selPeriod.BeginDate;
                odaReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = selPeriod.EndDate;
                odaReport.Fill(_dsReport, "REPORT");
                if (_dsReport.Tables["REPORT"].Rows.Count > 0)
                {
                    ReportViewerWindow report =
                        new ReportViewerWindow(
                            "Протокол переводов", "Reports/RepProtocolTr_Date_Order.rdlc",
                            _dsReport,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                            new Microsoft.Reporting.WinForms.ReportParameter("P_BEGINPERIOD", selPeriod.BeginDate.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_ENDPERIOD", selPeriod.EndDate.ToShortDateString())}
                        );
                    report.Show();
                }
                else
                {
                    MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Поиск и просмотр переводов по сотрудникам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btView_All_Transfer_Click(object sender, EventArgs e)
        {
            View_All_Transfer _view_All_Transfer = new View_All_Transfer();
            _view_All_Transfer.ShowDialog();
        }

        #endregion
		
        partial void subdiv_shtat_TextChanged(object sender, EventArgs e);

        private void btSql_Trace_Enabled_Click(object sender, EventArgs e)
        {
            OracleCommand _ocSet_Sql_Trace;
            if (btSql_Trace_Enabled.Pressed)
            {
                _ocSet_Sql_Trace = new OracleCommand("alter session set sql_trace=true",
                    Connect.CurConnect);
                _ocSet_Sql_Trace.ExecuteNonQuery();
                _ocSet_Sql_Trace = new OracleCommand(
                    @"BEGIN
                        select p.spid into :p_SPID
                        from v$process p, v$session s
                        where p.addr = s.paddr
                          and s.audsid = userenv('SESSIONID');
                    END;",
                    Connect.CurConnect);
                _ocSet_Sql_Trace.BindByName = true;
                _ocSet_Sql_Trace.Parameters.Add("p_SPID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                _ocSet_Sql_Trace.ExecuteNonQuery();
                btSql_Trace_Enabled.Text = "Отключить трассировку для SPID=" + _ocSet_Sql_Trace.Parameters["p_SPID"].Value.ToString();
            }
            else
            {
                _ocSet_Sql_Trace = new OracleCommand("alter session set sql_trace=false",
                    Connect.CurConnect);
                _ocSet_Sql_Trace.ExecuteNonQuery();
                btSql_Trace_Enabled.Text = "Включить трассировку";
            }          
        }

        private void btAccess_Template_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "ACCESS_TEMPLATE").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "ACCESS_TEMPLATE").First().Activate();
                return;
            }
            Wpf_Control_Viewer _viewer = new Wpf_Control_Viewer();
            _viewer.Name = "ACCESS_TEMPLATE";
            _viewer.Text = "Шаблоны доступа";
            Access_Template_Viewer _access_Template_Viewer = new Access_Template_Viewer();
            _viewer.elementHost1.Child = _access_Template_Viewer;
            _viewer.MdiParent = this;
            _viewer.WindowState = FormWindowState.Maximized;
            _viewer.Show();
        }

        /// <summary>
        /// Запуск согласования закрытия табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStart_Approval_Table_Click(object sender, EventArgs e)
        {
            // 13.04.2016 Записываем нужные данные для нового варианта закрытия табеля
            OracleCommand _ocTable_Close = new OracleCommand(string.Format(
                @"BEGIN
                        {0}.TABLE_CLOSE(:p_SUBDIV_ID, :p_TABLE_DATE, :p_TYPE_TABLE_ID);
                    END;", Connect.Schema), Connect.CurConnect);
            _ocTable_Close.BindByName = true;
            _ocTable_Close.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = subdiv_id;
            _ocTable_Close.Parameters.Add("p_TABLE_DATE", OracleDbType.Date).Value = ((Table)this.ActiveMdiChild).BeginDate;
            _ocTable_Close.Parameters.Add("p_TYPE_TABLE_ID", OracleDbType.Decimal).Value = 2;
            OracleTransaction _transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocTable_Close.Transaction = _transact;
                _ocTable_Close.ExecuteNonQuery();
                _transact.Commit();
                ((Table)this.ActiveMdiChild).GetTable_Approval();
            }
            catch (Exception ex)
            {
                _transact.Rollback();
                MessageBox.Show("Ошибка закрытия табеля!\n" + ex.Message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btList_Emp_With_Template_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Where(i => i.Name.ToUpper() == "EMP_TEMPLATE").Count() != 0)
            {
                this.MdiChildren.Where(i => i.Name.ToUpper() == "EMP_TEMPLATE").First().Activate();
                return;
            }
            Wpf_Control_Viewer _viewer = new Wpf_Control_Viewer();
            _viewer.Name = "EMP_TEMPLATE";
            _viewer.Text = "Сотрудники и шаблоны доступа";
            List_Emp_With_Template _view = new List_Emp_With_Template();
            _view.ParentWinForm = this;
            _viewer.elementHost1.Child = _view;
            _viewer.MdiParent = this;
            _viewer.WindowState = FormWindowState.Maximized;
            _viewer.Show();
        }

        private void btCount_Short_Graph_Click(object sender, EventArgs e)
        {
            DateTime _date = DateTime.Today;
            if (InputDataForm.ShowForm(ref _date, "dd MMMM yyyy") == DialogResult.OK)
            {
                OracleCommand _ocCount = new OracleCommand(string.Format(
                    @"SELECT {0}.GET_COUNT_SHORT_GRAPH(:P_DATE, :P_LIKE_GR) from dual", Connect.Schema), Connect.CurConnect);
                _ocCount.BindByName = true;
                _ocCount.Parameters.Add("P_DATE", OracleDbType.Date).Value = _date;
                _ocCount.Parameters.Add("P_LIKE_GR", OracleDbType.Varchar2).Value = "НС";
                int _count1 = Convert.ToInt16(_ocCount.ExecuteScalar());
                _ocCount.Parameters["P_LIKE_GR"].Value = "НН";
                int _count2 = Convert.ToInt16(_ocCount.ExecuteScalar());
                string _message = string.Format("Количество сотрудников на НС = {0},\nКоличество сотрудников на НН = {1},\nИтого = {2}",
                    _count1, _count2, _count1 + _count2);
                MessageBox.Show(_message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btRepAverage_Number_Consolidated_Click(object sender, EventArgs e)
        {
            ReportTable reportTable = new ReportTable(false, true);
            if (reportTable.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                OracleDataAdapter daReport = new OracleDataAdapter();
                daReport.SelectCommand = new OracleCommand(string.Format(
                    Queries.GetQuery("Table/RepAverage_Number_Consolidated.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                daReport.SelectCommand.BindByName = true;
                daReport.SelectCommand.Parameters.Add("p_begin_date", OracleDbType.Date).Value = new DateTime(reportTable.YearTable, 1, 1);
                daReport.SelectCommand.Parameters.Add("p_end_date", OracleDbType.Date).Value = new DateTime(reportTable.YearTable + 1, 1, 1).AddSeconds(-1);
                daReport.SelectCommand.Parameters.Add("p_cur_table1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                daReport.SelectCommand.Parameters.Add("p_cur_table2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                daReport.Fill(ds);
                if (ds.Tables["Table"].Rows.Count > 0)
                {
                    //ReportViewerWindow report =
                    //    new ReportViewerWindow(
                    //        "Среднесписочная численность", "Reports/RepAverage_Number_Consolidated.rdlc",
                    //        ds,
                    //        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                    //            new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())}, 
                    //        true
                    //    );
                    //report.Show();
                    ReportViewerWindow.RenderToExcel(this, "RepAverage_Number_Consolidated.rdlc",
                        new DataTable[] { ds.Tables[0], ds.Tables[1] },
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() {
                                new Microsoft.Reporting.WinForms.ReportParameter("P_YEAR", reportTable.YearTable.ToString())});
                }
                else
                {
                    MessageBox.Show("За указанный период данных нет!", "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }      
}
