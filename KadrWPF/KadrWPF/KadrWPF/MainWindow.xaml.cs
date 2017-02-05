using ARM_PROP;
using Kadr;
using KadrWPF.Table;
using LibraryKadr;
using LibraryKadr.Helpers;
using LibrarySalary.Helpers;
using LibrarySalary.ViewModel;
using ManningTable;
using Microsoft.Reporting.WinForms;
using Oracle.DataAccess.Client;
using Pass_Office;
using PERCo_S20_1C;
using PercoXML;
using Staff;
using StaffReports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tabel;
using VacationSchedule;
using WpfControlLibrary;
using WpfControlLibrary.Emps_Access;
using WpfControlLibrary.Table;

namespace KadrWPF
{
    /// <summary>tsmViewTable_Click
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Форма отображает продолжительность работы программы, одновременно блокируя главную форму
        /// </summary>
        public TimeExecute timeExecute;
        /// <summary>
        /// Дата для формирования файла для расчета зп
        /// </summary>
        DateTime dateDump = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        /// <summary>
        /// Форма выбора периода для отчета
        /// </summary>
        SelectPeriod selPeriod;

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));

            ControlAccess.EnableByRules(new System.Windows.Forms.Control());

            InitVacCommandBindings();

#if DEBUG
           if (Connect.UserID.ToUpper()=="KNV14534" || Connect.UserID.ToUpper() == "KNVTEST")
                this.Loaded += (p, pw) => { Test(); };
           else
                Ribbon.SelectedIndex = 0;
#else
           Ribbon.SelectedIndex = 0;
#endif
            ribbonApplicationMenu.Items.Add(new Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenuItem().Header = Connect.UserID + " " + Connect.UserFullName);
            
        }

        private void Test()
        {
            Helpers.AppCommands.OpenViewManningTable.Execute(null, null);
        }

        static MainWindow()
        {
            ListLinkKadr.AddLink(new LinkKadr("Карточка отпусков", "VSKard", ViewCard.OpenStaticView, ViewCard.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в табеле", "Table", Table_Viewer.OpenLink, Table_Viewer.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в АРМ бухгалтера", "Account", grid_ras.OpenLink, grid_ras.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Показать в кадрах", "Kadr", ListEmp.OpenLink, ListEmp.CanOpenLink));
            ListLinkKadr.AddLink(new LinkKadr("Карточка совмещений", "TableCombineCard", Kadr.Shtat.ReplEmpAdd.OpenStaticView, Kadr.Shtat.ReplEmpAdd.CanOpenLink));
        }
        public ViewTabCollection OpenTabs
        {
            get
            {
                return (ViewTabCollection)this.FindResource("OpenTabs");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void MenuCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState((e.Command as RoutedUICommand).Name);
        }

        private void Sql_Trace_Enabled_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OracleCommand _ocSet_Sql_Trace;
            if (btSql_Trace_Enabled.IsChecked)
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
                btSql_Trace_Enabled.Header = "Отключить трассировку для SPID=" + _ocSet_Sql_Trace.Parameters["p_SPID"].Value.ToString();
            }
            else
            {
                _ocSet_Sql_Trace = new OracleCommand("alter session set sql_trace=false",
                    Connect.CurConnect);
                _ocSet_Sql_Trace.ExecuteNonQuery();
                btSql_Trace_Enabled.Header = "Включить трассировку";
            }
        }

        private void BtMainDb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListEmpMain));
            if (t == null)
            {
                /// Создаем и работаем с формой первоначального поиска.
                LoadEmp loadEmp = new LoadEmp(false);
                System.Windows.Forms.DialogResult rez = loadEmp.ShowDialog();
                if (rez == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                OpenTabs.AddNewTab("Основная база", new WindowsFormsList_Viewer(new ListEmpMain(loadEmp.per_num, loadEmp.sort.ToString(), "ListEmpMain")));
                ///* Изменения от 20,07,2013 - если у пользователя роль STAFF_VIEW_ONLYLISTEMP - 
                // * то отключаем кнопку отчета произвольной формы*/
                //if (GrantedRoles.GetGrantedRole("STAFF_VIEW_ONLYLISTEMP"))
                //{
                //    btRepOtherType.Enabled = false;
                //}
                //else
                //{
                //    btRepOtherType.Enabled = true;
                //}ns 
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtArchives_Executed(object sender, ExecutedRoutedEventArgs e)
        {           
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListEmpArchive));
            if (t == null)
            {
                LoadEmp loadEmp = new LoadEmp(true);
                System.Windows.Forms.DialogResult rez = loadEmp.ShowDialog();
                if (rez == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                OpenTabs.AddNewTab("Архивная база", new WindowsFormsList_Viewer(new ListEmpArchive(loadEmp.per_num, loadEmp.sort.ToString(), "ListEmpArchive")));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtTempDb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListEmpTemp));
            if (t == null)
                OpenTabs.AddNewTab("Приемная база", new WindowsForms_Viewer(new ListEmpTemp()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtResume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Resume_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Резюме", new Resume_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

#region Работа с переводами
        
        private void ListEmpButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlRoles.GetState((e.Command as RoutedUICommand).Name) && OpenTabs.SelectedTab != null &&
                (OpenTabs.SelectedTab == OpenTabs.GetOpenTabForm(typeof(ListEmpMain)) || OpenTabs.SelectedTab == OpenTabs.GetOpenTabForm(typeof(ListEmpArchive))))
                e.CanExecute = true;
        }

        private void ListEmpMainDbButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlRoles.GetState((e.Command as RoutedUICommand).Name) && OpenTabs.SelectedTab != null &&
                OpenTabs.SelectedTab == OpenTabs.GetOpenTabForm(typeof(ListEmpMain)))
                e.CanExecute = true;
        }

        private void ListEmpArchiveButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlRoles.GetState((e.Command as RoutedUICommand).Name) && OpenTabs.SelectedTab != null &&
                OpenTabs.SelectedTab == OpenTabs.GetOpenTabForm(typeof(ListEmpArchive)))
                e.CanExecute = true;
        }

        private void BtViewTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).ViewTransfer();            
        }

        private void BtAddTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).AddTransfer();
        }

        private void BtEditTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).EditTransfer();            
        }

        private void BtDeleteTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).DeleteTransfer();
        }

        private void BtTransferComb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).TransferComb();
        }
                
        private void BtAddOld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).AddOld();
        }

        private void BtEditOld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).EditOld();
        }

        private void BtDeleteOld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).DeleteOld();
        }

        private void BtRecoveryTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).RecoveryTransfer();
        }

        private void BtReverseTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).ReverseTransfer();
        }

        private void BtAdd_Agreement_For_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).Add_Agreement_For_Emp();
        }

        private void BtDismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).Dismiss();
        }

        private void BtDismissToFR_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).DismissToFR();
        }

        private void BtDismissComb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).DismissComb();
        }

        private void BtProject_Order_Dismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).Project_Order_Dismiss();
        }

#endregion

        /// <summary>
        /// Отображение формы списка работников со срочными трудовыми договорами и доп.соглашениями
        /// </summary>
        private void BtTransfer_Term_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListEmpTerm));
            if (t == null)
                OpenTabs.AddNewTab("Срочные ТД и ДС", new WindowsFormsList_Viewer(new ListEmpTerm("", "", "ListEmpTerm")));
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Список сотрудников для установки подкласса условий труда
        /// </summary>
        private void BtTransferCond_Of_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListEmpConditionWork));
            if (t == null)
                OpenTabs.AddNewTab("Подклассы условий труда", new WindowsForms_Viewer(new ListEmpConditionWork()));
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Список переводов для обработки группой приема
        /// </summary>
        private void BtTransfer_Emp_For_Group_Hire_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Transfer_For_Group_Hire_View));
            if (t == null)
                OpenTabs.AddNewTab("Список переводов для обработки", new Transfer_For_Group_Hire_View());
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtBase_Exchange_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Transfer_Project_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Список проектов приказов", new Transfer_Project_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtProject_Statement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Project_Statement_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Список проектов заявлений о переводе", new Project_Statement_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

        private void Project_Transfer_Combined_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Project_Combined_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Проекты переводов", new Project_Combined_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtEditEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).EditEmp();
        }

        private void View_Emp_Mobile_Comm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Emp_Mobile_Comm_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Список абонентов", new Emp_Mobile_Comm_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

#region Формирование отчетов АСУ КАДРЫ
        /// <summary>
        /// Отчет о количестве принятых
        /// </summary>
        private void BtRepCountHire_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepCountHire repCountHire = new RepCountHire();
            repCountHire.ShowDialog();
        }

        /// <summary>
        /// Опись приказов о приеме
        /// </summary>
        private void BtListOrdHire_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListOrdHire listOrdHire = new ListOrdHire();
            listOrdHire.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о приеме за период
        /// </summary>
        private void BtBkOrdHireForPeriod_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BkOrdHireForPeriod bkOrdHireForPeriod = new BkOrdHireForPeriod();
            bkOrdHireForPeriod.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о приеме по году и номеру последнего напечатанного приказа
        /// </summary>
        private void BtBkOrdHireYear_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BkOrdHireYear bkOrdHireYear = new BkOrdHireYear();
            bkOrdHireYear.ShowDialog();
        }

        /// <summary>
        /// Отчет Нарушения за период
        /// </summary>
        private void BtViolations_By_Period_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет о количестве уволенных
        /// </summary>
        private void BtRepCountDismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepCountDismiss repCountDismiss = new RepCountDismiss();
            repCountDismiss.ShowDialog();
        }

        /// <summary>
        /// Отчет о количестве уволенных (обратная сторона)
        /// </summary>
        private void BtRepCountDismissR_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepCountDismissR repCountDismissR = new RepCountDismissR();
            repCountDismissR.ShowDialog();
        }
        
        /// <summary>
        /// Отчет по причинам увольнения (1 форма)
        /// </summary>
        private void BtRepReasonDismiss1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepReasonDismiss1 repReasonDismiss1 = new RepReasonDismiss1();
            repReasonDismiss1.ShowDialog();
        }
        
        /// <summary>
        /// Отчет по причинам увольнения (2 форма)
        /// </summary>
        private void BtRepReasonDismiss2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepReasonDismiss2 repReasonDismiss2 = new RepReasonDismiss2();
            repReasonDismiss2.ShowDialog();
        }

        /// <summary>
        /// Опись приказов об увольнении
        /// </summary>
        private void BtListOrdDismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListOrdDismiss listOrdDismiss = new ListOrdDismiss();
            listOrdDismiss.ShowDialog();
        }

        /// <summary>
        /// Окончание срока договора
        /// </summary>
        private void BtEndOfContr_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EndOfContr endOfContr = new EndOfContr();
            endOfContr.ShowDialog();
        }

        /// <summary>
        /// Книга приказов об увольнении
        /// </summary>
        private void BtOrdDismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BkOrdDismiss bkOrdDismiss = new BkOrdDismiss();
            bkOrdDismiss.ShowDialog();
        }

        /// <summary>
        /// Книга приказов о переводах
        /// </summary>
        private void BtOrdTransfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BkOrdTransfer bkOrdTransfer = new BkOrdTransfer();
            bkOrdTransfer.ShowDialog();
        }

        /// <summary>
        /// Список уволенных полный
        /// </summary>
        private void BtListDismissFull_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListDismissFull listDismissFull = new ListDismissFull();
            listDismissFull.ShowDialog();
        }

        /// <summary>
        /// Список уволенных без адресов
        /// </summary>
        private void BtListDismissShort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListDismissShort listDismissShort = new ListDismissShort();
            listDismissShort.ShowDialog();
        }

        /// <summary>
        /// Список уволенных - Без адресов (новая форма)
        /// </summary>
        private void BtListDismissForDir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListDismissForDir listDismissForDir = new ListDismissForDir();
            listDismissForDir.ShowDialog();
        }

        /// <summary>
        /// Список уволенных (сдельщики)
        /// </summary>
        private void BtListDismissJobman_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListDismissJobman listDismissJobman = new ListDismissJobman();
            listDismissJobman.ShowDialog();
        }

        /// <summary>
        /// Список работников, имеющих ученую степень
        /// </summary>
        private void BtListAcadDegree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Список работников, имеющих ученую степень\"?", "АРМ Кадры", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListAcadDegree();
            }
        }

        /// <summary>
        /// Численность сотрудников
        /// </summary>
        private void BtRep_Staff_Subdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FormRepStaff form = new FormRepStaff();
            form.ShowDialog();
        }

        /// <summary>
        /// Формирование Уведомление по трудовому договору
        /// </summary>
        private void BtNotice_Expiry_Contract_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Form_Notice_By_Emp("Notice_Expiry.sql", "Notice_Expiry_Contract.rdlc", "Уведомление по трудовому договору");
        }

        /// <summary>
        /// Формирование Уведомление по доп.соглашению
        /// </summary>
        private void BtNotice_Expiry_Add_Agreement_Executed(object sender, ExecutedRoutedEventArgs e)
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
            find_Emp.Owner = this;
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
                    if (Signes.Show(0, "Notice_Expiry", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos) == true)
                        ReportViewerWindow.RenderToExcel(this, namePattern, _ds.Tables["Table1"],
                            new List<ReportParameter>() {
                                new ReportParameter("P_SIGNES_POS", s_pos[0][0]),
                                new ReportParameter("P_SIGNES_FIO", s_pos[0][1])},
                            nameTempFile);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование Справка по месту требования 
        /// </summary>
        private void BtReference_On_Place_Requirements_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Find_Emp find_Emp = new WpfControlLibrary.Find_Emp(DateTime.Today);
            find_Emp.Owner = this;
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
                    if (Signes.Show(0, "Reference_By_Emp", "Введите должность и ФИО ответственного лица (для подписи)", 2, ref _dt) == true)
                    {
                        ReportViewerWindow.RenderToExcel(this, "Reference_On_Place_Requirements.rdlc", new DataTable[] { _ds.Tables["Table1"], _dt },
                            null, "Справка по месту требования", "doc");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Формирование Справка по уходу за ребенком
        /// </summary>
        private void BtReference_Child_Realing_Leave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Find_Emp find_Emp = new WpfControlLibrary.Find_Emp(DateTime.Today);
            find_Emp.Owner = this;
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
                    if (Signes.Show(0, "Reference_By_Emp", "Введите должность и ФИО ответственного лица (для подписи)", 2, ref _dt) == true)
                    {
                        ReportViewerWindow.RenderToExcel(this, "Reference_Child_Care_Leave.rdlc", new DataTable[] { _ds.Tables["Table1"], _dt },
                            null, "Справка по уходу за ребенком", "doc");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Работники участвующие в программе НПО
        /// </summary>
        private void BtRepNonState_Pens_Prov_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет по участникам НПО?", "АСУ \"Кадры\"",
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
        private void BtRepNPP_With_Date_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет по участникам НПО?", "АСУ \"Кадры\"",
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
        /// Список инвалидов по заводу
        /// </summary>
        private void BtListInvalid_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Список инвалидов по заводу\"?",
                "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListInvalidPlant();
            }
        }

        /// <summary>
        /// Список инвалидов по подразделению
        /// </summary>
        private void BtListInvalidSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListInvalidSubdiv listInvalidSubdiv = new ListInvalidSubdiv();
            listInvalidSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Численность, состав и движение по заводу
        /// </summary>
        private void BtListRetirerStruct_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Данные по пенсионерам. Численность, состав и движение по заводу\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListRetirerStruct();
            }
        }

        /// <summary>
        /// Данные по пенсионерам. Численность, состав и движение по подразделению
        /// </summary>
        private void BtListRetirerStructSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListRetirerStructSubdiv listRetirerStructSubdiv = new ListRetirerStructSubdiv();
            listRetirerStructSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Список пенсионеров по заводу
        /// </summary>
        private void BtListRetirerPlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать данный отчет\n\"Данные по пенсионерам. Список пенсионеров по заводу\"?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListRetirerPlant();
            }
        }

        /// <summary>
        /// Данные по пенсионерам. Список пенсионеров по подразделению
        /// </summary>
        private void BtListRetirerSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListRetirerSubdiv listRetirerSubdiv = new ListRetirerSubdiv();
            listRetirerSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по пенсионерам. Список уволенных
        /// </summary>
        private void BtListRetirerDismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListRetirerDismiss listRetirerDismiss = new ListRetirerDismiss();
            listRetirerDismiss.ShowDialog();
        }

        /// <summary>
        /// Список для заполнения страховых свидетельств
        /// </summary>
        private void BtEmptyInsurNum_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Form1.EmptyInsurNum();
        }

        /// <summary>
        /// Справка-объективка
        /// </summary>
        private void BtSprObj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SprObj sprObj = new SprObj();
            sprObj.ShowDialog();
        }

        /// <summary>
        /// Личная карточка
        /// </summary>
        private void BtPersonalCard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FormPersonCard formPersonCard = new FormPersonCard();
            formPersonCard.ShowDialog();
        }

        /// <summary>
        /// Отчет произвольной формы
        /// </summary>
        private void BtRepOtherType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).RepOtherType();
        }

        /// <summary>
        /// Отчет по материально-ответственным лицам
        /// </summary>
        private void BtRepMRP_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Отчёт по материально-ответственным лицам\"?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.RepMRP();
            }
        }

        /// <summary>
        /// Отчет МСФО - Демографическая и финансовая 
        /// </summary>
        private void BtRepMSFOSal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("АСУ \"Кадры\"", "За указанный период данные отсутствуют");
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
        private void BtRepMSFODisEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("АСУ \"Кадры\"", "За указанный период данные отсутствуют");
                }
            }
        }

        /// <summary>
        /// Формирование отчета по льготникам
        /// </summary>
        private void BtListEmpPrivPos_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет - Список переводов по льготным профессиям
        /// </summary>
        private void BtTransferPrivPos_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            selPeriod = new SelectPeriod();
            selPeriod.ShowInTaskbar = false;
            if (selPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("За указанный период данные отсутствуют.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по декретным отпускам
        /// </summary>
        private void BtRepChild_Realing_Leave_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет Количество сотрудников по службам завода
        /// </summary>
        private void BtEmp_By_Service_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NumbersEmpOnService numbers = new NumbersEmpOnService();
            numbers.ShowInTaskbar = false;
            numbers.ShowDialog();
        }

        /// <summary>
        /// Формирование отчета Численность, состав и движение по заводу
        /// </summary>
        private void BtNumbersVeteranPlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Данные по ветеранам труда. Численность, состав и движение по заводу\"?",
                "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListVeteranStruct();
            }
        }

        /// <summary>
        /// Формирование отчета Численность, состав и движение по подразделению
        /// </summary>
        private void BtNumbersVeteranSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListVeteransStructSubdiv listVeteransStructSubdiv =
                new ListVeteransStructSubdiv();
            listVeteransStructSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по ветеранам труда. Список ветеранов по заводу.
        /// </summary>
        private void BtListVeteranPlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите сформировать отчет\n\"Данные по ветеранам труда. Список ветеранов по заводу\"?", "АСУ \"Кадры\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ListVeteranPlant();
            }
        }

        /// <summary>
        /// Список ветеранов труда по подразделению
        /// </summary>
        private void BtListVeteranSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListVeteranSubdiv listVeteranSubdiv = new ListVeteranSubdiv();
            listVeteranSubdiv.ShowDialog();
        }

        /// <summary>
        /// Данные по ветеранам труда. Список уволенных.
        /// </summary>
        private void BtListDismissVeteran_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ListVeteranDismiss listVeteranDismiss = new ListVeteranDismiss();
            listVeteranDismiss.ShowDialog();
        }

        /// <summary>
        /// Отчет для администраторов о.78 - Список переводов за период
        /// </summary>
        private void BtRepTransfer_By_Period_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(false, true, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("В подразделении за указанный период нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет по молодым специалистам
        /// </summary>
        private void BtRepYoung_Specialist_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ListEmp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).RepYoung_Specialist();            
        }

        /// <summary>
        /// Отобразить форму формирования приказа о поощрении
        /// </summary>
        private void BtOrderOnEncouraging_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OrderOnEncouraging _orderOnEncouraging = new OrderOnEncouraging();
            _orderOnEncouraging.ShowDialog();
        }

        /// <summary>
        /// Формирование доп. соглашений по подразделению на определенную дату
        /// </summary>
        private void BtAdd_Agreement_On_Subdiv_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Отчет допсоглашение для сокращенного дня/недели
        /// </summary>
        private void BtAdd_Agreement_On_ShortDay1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddAgreementShortDay("DAY");
        }

        /// <summary>
        /// Отчет допсоглашение для сокращенного дня/недели
        /// </summary>
        private void BtAdd_Agreement_On_ShortDay2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddAgreementShortDay("WEEK");
        }

        /// <summary>
        /// Формируем отчет по доп соглашениям с параметром либо это день или неделя
        /// </summary>
        /// <param name="short_day_week"></param>
        private void AddAgreementShortDay(string short_day_week)
        {
            RepFilterByEmp _formSelect = new RepFilterByEmp(null, null, new DateTime(2017, 03, 27), new DateTime(2017, 07, 26), false);
            _formSelect.TextBeginCaption = "Начало доп. соглашения";
            _formSelect.TextEndCaption = "Окончание доп. соглашения";
            _formSelect.BySubdivReport = true;
            _formSelect.Owner = Window.GetWindow(this);
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
                    System.Windows.Forms.MessageBox.Show("Ошибка получения списка сотрудников. " + ex.Message, "Кадры");
                    return;
                }
                string[][] ss = null;
                if (Signes.Show(0, "AddAgreementShortDay", "Выберите ответственное лицо", 1, ref ss) == true)
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

        /// <summary>
        /// Отчет по декретным отпускам (форма для личного стола без лишних полей)
        /// </summary>
        private void BtRepChild_Realing_Leave2_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Уведомление о сокращении рабочей недели
        /// </summary>
        private void BtNotification_Short_Week_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Notification_Short_Work("Notification_Short_Week.rdlc");
        }

        /// <summary>
        /// Уведомление о раннем окончании рабочего времени
        /// </summary>
        private void BtNotification_Early_Ending_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Notification_Short_Work("Notification_Early_Ending.rdlc");
        }

        /// <summary>
        /// Формирование уведомлений о сокращении рабочего времени
        /// </summary>
        /// <param name="namePattern">Имя шаблона</param>
        void Notification_Short_Work(string namePattern)
        {
            RepFilterByEmp _formSelect = new RepFilterByEmp(null, null, new DateTime(2017, 03, 27), new DateTime(2017, 07, 26), false);
            _formSelect.TextBeginCaption = "Начало режима";
            _formSelect.TextEndCaption = "Окончание режима";
            _formSelect.BySubdivReport = true;
            _formSelect.Owner = Window.GetWindow(this);
            if (_formSelect.ShowDialog() == true)
            {
                OracleDataAdapter _daReport = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmps_For_Notification.sql"),
                    Connect.Schema), Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_beginPeriod", OracleDbType.Date).Value = _formSelect.DateBegin;
                _daReport.SelectCommand.Parameters.Add("p_endPeriod", OracleDbType.Date).Value = _formSelect.DateEnd;
                _daReport.SelectCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = _formSelect.SubdivID;
                DataSet _ds = new DataSet();
                _daReport.Fill(_ds, "ORDER_EMP");
                if (_ds.Tables["ORDER_EMP"].Rows.Count > 0)
                {
                    string[][] s_pos = new string[][] { };
                    if (Signes.Show(_formSelect.SubdivID, "Notice_Expiry", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos) == true)
                    {
                        ReportViewerWindow _rep = new ReportViewerWindow("Уведомление", "Reports/" + namePattern,
                            _ds, new List<ReportParameter>() {
                                new ReportParameter("P_SIGNES_POS", s_pos[0][0]),
                                new ReportParameter("P_SIGNES_FIO", s_pos[0][1]),
                                new ReportParameter("P_DATE1", _formSelect.DateBegin.Value.ToShortDateString()),
                                new ReportParameter("P_DATE2", _formSelect.DateEnd.Value.ToShortDateString()),
                            }, true);
                        _rep.Show();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Подсчет количества сотрудников на НС и НН
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCount_Short_Graph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime _date = DateTime.Today;
            if (InputDataForm.ShowForm(ref _date, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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
                System.Windows.Forms.MessageBox.Show(_message, "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtCorporate_Support_Contract_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Find_Emp find = new WpfControlLibrary.Find_Emp(DateTime.Today);
            if (find.ShowDialog() == true)
            {
                DataTable _dtReport = new DataTable();
                OracleDataAdapter _daReport =
                    new OracleDataAdapter(Queries.GetQuery("SelectHousing_Program.sql"),
                    Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = find.Transfer_ID;
                _daReport.Fill(_dtReport);
                Word.PrintDocument("Corporate_Support_Contract.dotx", _dtReport);
            }
        }

        private void BtCorporate_Support_Agreement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WpfControlLibrary.Find_Emp find = new WpfControlLibrary.Find_Emp(DateTime.Today);
            if (find.ShowDialog() == true)
            {
                DataTable _dtReport = new DataTable();
                OracleDataAdapter _daReport =
                    new OracleDataAdapter(Queries.GetQuery("SelectHousing_Program.sql"),
                    Connect.CurConnect);
                _daReport.SelectCommand.BindByName = true;
                _daReport.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value = find.Transfer_ID;
                _daReport.Fill(_dtReport);
                Word.PrintDocument("Corporate_Support_Agreement.dotx", _dtReport);
            }
        }

        #endregion

#region Вкладка Бюро пропусков

        /// <summary>
        /// Привилегии
        /// </summary>
        private void BtPermit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListPermit));
            if (t == null)
            {
                FilterForPermit filterForPermit = new FilterForPermit();
                if (filterForPermit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ListPermit listPermit = new ListPermit(filterForPermit.strFilter);
                    listPermit.Name = "listPermit";
                    listPermit.Text = "Учет жетонов, вкладышей и пропусков";
                    OpenTabs.AddNewTab("Учет привилегий", new WindowsForms_Viewer(listPermit));
                }
            }
            else
                OpenTabs.SelectedTab = t;           
        }

        /// <summary>
        /// Фильтр сотрудников (окно привилегий)
        /// </summary>
        private void BtEditFilterPermit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ListPermit));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;    
            FilterForPermit filterForPermit = new FilterForPermit();
            if (filterForPermit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 ((ListPermit)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).LoadPermit(filterForPermit.strFilter);
            }
        }

        /// <summary>
        /// Типы привилегий
        /// </summary>
        private void BtType_Permit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(HBType_Permit));
            if (t == null)
                OpenTabs.AddNewTab("Подклассы условий труда", new WindowsForms_Viewer(new HBType_Permit()));
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Список нарушителей по новой версии программы
        /// </summary>
        private void BtViewViolators_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(List_Violations_Editor));
            if (t == null)
                OpenTabs.AddNewTab("Нарушители режима", new List_Violations_Editor());
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Список уволенных сотрудников для обработки в Бюро пропусков
        /// </summary>
        private void BtDismissed_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Dismissed_Emp_View_Dis));
            if (t == null)
                OpenTabs.AddNewTab("Список уволенных", new Dismissed_Emp_View_Dis(3));
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Список уволенных сотрудников для обработки в Бюро пропусков
        /// </summary>
        private void BtTransfer_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Dismissed_Emp_View_Trans));
            if (t == null)
                OpenTabs.AddNewTab("Список переведенных", new Dismissed_Emp_View_Trans(2));
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Шаблоны доступа из системы Перко
        /// </summary>
        private void BtAccess_Template_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Access_Template_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Шаблоны доступа", new Access_Template_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

        /// <summary>
        /// Сотрудники с шаблонами доступа
        /// </summary>
        private void BtList_Emp_With_Template_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(List_Emp_With_Template));
            if (t == null)
                OpenTabs.AddNewTab("Сотрудники и шаблоны доступа", new List_Emp_With_Template());
            else
                OpenTabs.SelectedTab = t;
        }

#endregion

#region Работа со справочниками

        // Работа со справочником Вид обучения
        private void BtType_Study_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник типов обучения");
            if (t == null)
                OpenTabs.AddNewTab("Справочник типов обучения", new WindowsForms_Viewer(new HandBook(typeof(TYPE_STUDY_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        // Работа со справочником Вид образования
        private void BtType_Edu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник типов образования");
            if (t == null)
                OpenTabs.AddNewTab("Справочник типов образования", new WindowsForms_Viewer(new HandBookSpec("type_edu")));
            else
                OpenTabs.SelectedTab = t;
        }

        // Работа со справочником учебных заведений
        private void BtInstit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник учебных заведений");
            if (t == null)
                OpenTabs.AddNewTab("Справочник учебных заведений", new WindowsForms_Viewer(new HandBookInstit()));
            else
                OpenTabs.SelectedTab = t;
        }

        // Работа со справочником городов учебных заведений
        private void BtStudy_City_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник городов учебных заведений");
            if (t == null)
                OpenTabs.AddNewTab("Справочник городов учебных заведений", new WindowsForms_Viewer(new HandBook(typeof(STUDY_CITY_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtGroup_Spec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник групп специальности");
            if (t == null)
                OpenTabs.AddNewTab("Справочник групп специальности", new WindowsForms_Viewer(new HandBook(typeof(GROUP_SPEC_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtQual_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник квалификаций");
            if (t == null)
                OpenTabs.AddNewTab("Справочник квалификаций", new WindowsForms_Viewer(new HandBook(typeof(QUAL_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtSpeciality_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник специальностей");
            if (t == null)
                OpenTabs.AddNewTab("Справочник специальностей", new WindowsForms_Viewer(new HandBookSpec("speciality")));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtLang_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник иностранных языков");
            if (t == null)
                OpenTabs.AddNewTab("Справочник иностранных языков", new WindowsForms_Viewer(new HandBook(typeof(LANG_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtLevel_Know_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник уровня знаний иностранных языков");
            if (t == null)
                OpenTabs.AddNewTab("Справочник уровня знаний иностранных языков", new WindowsForms_Viewer(new HandBook(typeof(LEVEL_KNOW_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtComm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник комиссариатов");
            if (t == null)
                OpenTabs.AddNewTab("Справочник комиссариатов", new WindowsForms_Viewer(new HandBook(typeof(COMM_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtMil_Spec_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void BtMed_Classif_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник категорий годности");
            if (t == null)
                OpenTabs.AddNewTab("Справочник категорий годности", new WindowsForms_Viewer(new HandBook(typeof(MED_CLASSIF_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtMil_Rank_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник воинских званий");
            if (t == null)
                OpenTabs.AddNewTab("Справочник воинских званий", new WindowsForms_Viewer(new HandBook(typeof(MIL_RANK_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtMil_Cat_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Таблица воинского состава");
            if (t == null)
                OpenTabs.AddNewTab("Таблица воинского состава", new WindowsForms_Viewer(new HandBook(typeof(MIL_CAT_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Troop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник видов войск");
            if (t == null)
                OpenTabs.AddNewTab("Справочник видов войск", new WindowsForms_Viewer(new HandBook(typeof(TYPE_TROOP_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Priv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Тип льготы");
            if (t == null)
                OpenTabs.AddNewTab("Тип льготы", new WindowsForms_Viewer(new HandBook(typeof(TYPE_PRIV_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Rise_Qual_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Тип повышения квалификации");
            if (t == null)
                OpenTabs.AddNewTab("Тип повышения квалификации", new WindowsForms_Viewer(new HandBook(typeof(TYPE_RISE_QUAL_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        public void BtPrivileged_Position_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник льготных профессий");
            if (t == null)
                OpenTabs.AddNewTab("Справочник льготных профессий", new WindowsForms_Viewer(new HBPrivileged_Position()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Postg_Study_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Тип послевузовского образования");
            if (t == null)
                OpenTabs.AddNewTab("Тип послевузовского образования", new WindowsForms_Viewer(new HandBook(typeof(TYPE_POSTG_STUDY_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtSource_Employability_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Источник трудоустройства");
            if (t == null)
                OpenTabs.AddNewTab("Источник трудоустройства", new WindowsForms_Viewer(new HandBook(typeof(SOURCE_EMPLOYABILITY_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtReason_dismiss_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Причина увольнения");
            if (t == null)
                OpenTabs.AddNewTab("Причина увольнения", new WindowsForms_Viewer(new HandBookSpec("reason_dismiss")));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtPosition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник должностей");
            if (t == null)
                OpenTabs.AddNewTab("Справочник должностей", new WindowsForms_Viewer(new HBPosition()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtSubdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник подразделений");
            if (t == null)
                OpenTabs.AddNewTab("Справочник подразделений", new WindowsForms_Viewer(new HBSubdiv()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtReward_Name_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник наименований наград");
            if (t == null)
                OpenTabs.AddNewTab("Справочник наименований наград", new WindowsForms_Viewer(new HandBookReward()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Reward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Справочник типов наград");
            if (t == null)
                OpenTabs.AddNewTab("Справочник типов наград", new WindowsForms_Viewer(new HandBook(typeof(TYPE_REWARD_seq))));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtConditions_Of_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab("Подклассы условий труда");
            if (t == null)
                OpenTabs.AddNewTab("Подклассы условий труда", new WindowsForms_Viewer(new HBConditionWork()));
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtType_Condition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(List_Emp_With_Template));
            if (t == null)
                OpenTabs.AddNewTab("Справочник типов условий труда", new Type_Condition_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

#endregion

#region Сторонние сотрудники
        
        /// <summary>
        /// Просмотр сторонних сотрудников
        /// </summary>
        private void BtPrevFR_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {    
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
            {                
                OpenTabs.AddNewTab("Сторонние сотрудники", new WindowsForms_Viewer(new FR_Emp()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtReleaseBuffer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DictionaryPerco.employees.ReleaseBuffer();
            Connect.Commit();
        }

        /// <summary>
        /// Фильтр сторонних сотрудников.
        /// </summary>
        private void BtFilterFR_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).FilterFR_Emp();
        }

        /// <summary>
        /// Поиск сторонних сотрудников.
        /// </summary>
        private void BtFindFR_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).FindFR_Emp();
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименованию подразделения.
        /// </summary>
        private void BtSubdivSort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            string sort = "order by SUBDIV_NAME";
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по ФИО.
        /// </summary>
        private void BtFIOSort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            string sort = "order by FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименованию подразделения и ФИО.
        /// </summary>
        private void BtSubdivFIOSort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            string sort = "order by SUBDIV_NAME, FR_LAST_NAME, FR_FIRST_NAME, FR_MIDDLE_NAME";
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по наименовании должности.
        /// </summary>
        private void BtPositionSort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            string sort = "order by POS_NAME";
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).SorterFR_Emp(sort);
        }

        /// <summary>
        /// Сортировка сторонних сотрудников по дате начала работы.
        /// </summary>
        private void BtDate_StartSort_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(FR_Emp));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            string sort = "order by FR_DATE_START";
            ((FR_Emp)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).SorterFR_Emp(sort);
        }

#endregion

#region АРМ Бухгалтера

        private void BtDataFill_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(grid_ras));
            if (t == null)
            {
                OpenTabs.AddNewTab("Список сотрудников АРМ Бухгалтера", new WindowsForms_Viewer(new grid_ras()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void Bt_sbros_IBM_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (InputDataForm.ShowForm(ref dateDump, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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

        /// <summary>
        /// Проверка заполнения данных перед сбросом справочника работающих в файл
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
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
                System.Windows.Forms.MessageBox.Show("Файл для расчета заработной платы сформирован!",
                    "АРМ 'Кадры'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtFilterRas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(grid_ras));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;           
            filter filt = new filter(((grid_ras)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).bs);
            filt.Show();
        }

        private void BtFindRas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(grid_ras));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            find_spr find = new find_spr(((grid_ras)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).bs);
            find.Show();
        }

        private void BtUpdateRas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(grid_ras));
            if (t == null)
                return;
            else
                OpenTabs.SelectedTab = t;
            ((grid_ras)((LibraryKadr.IWindowsForms_Viewer)OpenTabs.SelectedTab.ContentData).ChildForm).UpdateRas();
        }

        private void BtView_All_Transfer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            View_All_Transfer _view_All_Transfer = new View_All_Transfer();
            _view_All_Transfer.ShowDialog();
        }

        private void BtOutside_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Outside_Emp_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Сторонние сотрудники", new Outside_Emp_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtPrintAD_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintAccount_data printAD = new PrintAccount_data();
            printAD.ShowInTaskbar = false;
            printAD.ShowDialog();
        }

        private void R_btRasCombList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime tm = DateTime.Now;
            if (InputDataForm.ShowForm(ref tm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
            {
                DataTable t = new DataTable();
                new OracleDataAdapter(string.Format(Queries.GetQuery("RAS/CombineEmp.sql"), Connect.Schema, tm.ToString("yyyy-MM-dd")), Connect.CurConnect).Fill(t);
                Excel.PrintWithBorder("RASCombineEmp.xlt", "A3", new DataTable[] { t }, new ExcelParameter[]{
                    new ExcelParameter("E1",tm.ToString("dd-MM-yyyy"))});
            }
        }

        private void R_btRASInvalidEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime tm = DateTime.Now;
            if (InputDataForm.ShowForm(ref tm, "MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter aRep = new OracleDataAdapter(string.Format(Queries.GetQuery("RAS/RASInvalidEmp.sql"),
                    Connect.Schema), Connect.CurConnect);
                aRep.SelectCommand.Parameters.Add("p_date_print", OracleDbType.Date).Value = tm;
                aRep.Fill(t);
                Excel.PrintWithBorder("RASInvalidEmp.xlt", "A3", new DataTable[] { t }, new ExcelParameter[]{
                    new ExcelParameter("G1",string.Format("{0} месяц {1}г.",tm.ToString("MM"),tm.ToString("yyyy")))});
            }
        }

        private void R_btRASProfEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(Queries.GetQuery(@"ras\ProfOrNotEmp.sql"), Connect.Schema, 1), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm;
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников состоящих в профсоюзе") });
            }
        }

        private void R_btRASNonProfEmp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter _daProfUnion = new OracleDataAdapter(string.Format(Queries.GetQuery(@"ras\ProfOrNotEmp.sql"), Connect.Schema, 0), Connect.CurConnect);
                _daProfUnion.SelectCommand.BindByName = true;
                _daProfUnion.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date).Value = dateForm;
                _daProfUnion.Fill(t);
                Excel.PrintWithBorder(true, "RASProfEmp.xlt", "A4", new DataTable[] { t }, new ExcelParameter[] { new ExcelParameter("A1", "Список работников не состоящих в профсоюзе") });
            }
        }

        private void btProfUnion_Entered_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime dateForm = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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

        private void btProfUnion_CameOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime dateForm = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
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

        private void BtProtocolDump_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime dateForm = DateTime.Now;
            if (InputDataForm.ShowForm(ref dateForm, "dd MMMM yyyy") == System.Windows.Forms.DialogResult.OK)
            {
                if (!FlagProtokolDump(dateForm))
                {
                    System.Windows.Forms.MessageBox.Show("В БД нет пустых дат на выслугу лет!",
                        "АРМ Бухгалтера",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Отчет по переводам в подразделении за период
        /// </summary>
        private void BtRepTransSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(true, true, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("В подразделении за указанный период нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Список работников со сроком окончания доп.соглашения
        /// </summary>
        private void BtRepEmpContract_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelPeriod_Subdiv selPeriod_Subdiv = new SelPeriod_Subdiv(true, false, false);
            selPeriod_Subdiv.Text = "Задайте параметры отчета";
            if (selPeriod_Subdiv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    System.Windows.Forms.MessageBox.Show("В подразделении нет данных!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Протокол несоответствия месяца даты движения и даты приказа о переводе
        /// </summary>
        private void BtProtocolTr_Date_Order_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    System.Windows.Forms.MessageBox.Show("За указанный период данные не найдены.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

#endregion

#region Табельный учет
        private void BtCalendar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(Tabel.Calendar));
            if (t == null)
            {
                OpenTabs.AddNewTab("Календарь", new WindowsForms_Viewer(new Tabel.Calendar()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Table_Viewer));
            if (t == null)
            {
                var p = new Table_Viewer();
                ViewTabBase v = OpenTabs.AddNewTab("Табель", p);
                p.OwnerTabBase = v;
            }
            else
            {
                DialogResult drQuestion = System.Windows.Forms.MessageBox.Show("Открыть табель в новом окне?" +
                    "\n(Да - откроется новое окно табеля)" +
                    "\n(Нет - будет использоваться текущее окно табеля)",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (drQuestion)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        var p = new Table_Viewer();
                        ViewTabBase v = OpenTabs.AddNewTab("Табель", p);
                        p.OwnerTabBase = v;
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        OpenTabs.SelectedTab = t;
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        return;
                        break;
                    default:
                        break;
                }
            }
        }

        private void BtGr_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(Tabel.Gr_Work));
            if (t == null)
            {
                OpenTabs.AddNewTab("Графики работы", new WindowsForms_Viewer(new Tabel.Gr_Work()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtList_Closing_Table_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTab(typeof(Table_Closing_Viewer));
            if (t == null)
                OpenTabs.AddNewTab("Закрытие табеля и согласование", new Table_Closing_Viewer());
            else
                OpenTabs.SelectedTab = t;
        }

#region Общие Отчеты по табелю для 3,5 отделов
        /// <summary>
        /// Отчет по прогульщикам
        /// </summary>
        private void BtOrderTruancy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OrderTruancy orderTruancy = new OrderTruancy();
            orderTruancy.ShowDialog();
        }

        /// <summary>
        /// Отчет по административным отпускам для группы табельного учета
        /// </summary>
        private void BtRepOnAdministrVac_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepOnAdministrVac _repOnAdministrVac = new RepOnAdministrVac();
            _repOnAdministrVac.ShowDialog();
        }

        /// <summary>
        /// Отчет по временной нетрудоспособности
        /// </summary>
        private void BtRepOnHospital_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RepOnHospital _repOnHospital = new RepOnHospital();
            _repOnHospital.ShowDialog();
        }

        /// <summary>
        /// Отчет по отпуску по беременности и родам
        /// </summary>
        private void BtRepOnPregnancy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepOnPregnancy();
        }

        /// <summary>
        /// Отчет по отпуску по уходу за ребенком до 3-х лет
        /// </summary>
        private void BtRepOnChildCare_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepOnChildCare();
        }

        /// <summary>
        /// Формирование отчета по среднесписочной численности в разрезе подразделений
        /// </summary>
        private void BtRepAverage_Number_On_Plant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepAverage_Number_On_Plant();
        }

        /// <summary>
        /// Справка по среднесписочной численности для группы личного стола отдела кадров
        /// </summary>
        private void BtRepAverage_Number_Personnel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepAverage_Number_Personnel();
        }

        /// <summary>
        /// Формирование отчета по отсутствию (добавлен 15.12.2015)
        /// </summary>
        private void BtRepFailureToAppear_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepFailureToAppear();
        }

        /// <summary>
        /// Формирование отчета Поздний выход (добавлен 06.02.2016)
        /// </summary>
        private void BtRepLate_Pass_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepLate_Pass();
        }

        /// <summary>
        /// Формирование отчета для отдела 3 по опозданиям
        /// </summary>
        private void BtRepLateness_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportForEconDev reportDocs = new ReportForEconDev(
                "SelectRepLateness.sql", "Lateness.xlt");
            reportDocs.ShowDialog();
        }

        /// <summary>
        /// Формирование отчета для отдела 3 по работе за территорией
        /// </summary>
        private void BtRepWorkOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportForEconDev reportDocs = new ReportForEconDev(
                "SelectWorkOut.sql", "WorkOut.xlt");
            reportDocs.ShowDialog();
        }

        /// <summary>
        /// Отчет по среднесписочной численности
        /// </summary>
        private void BtRepAverage_Number_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepAverage_Number();
        }

        /// <summary>
        /// Сводный Отчет по среднесписочной численности
        /// </summary>
        private void BtRepAverage_Number_Consolidated_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepAverage_Number_Consolidated();
        }

        /// <summary>
        /// Отчет по списочной численности
        /// </summary>
        private void BtRepListEmp_Number_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepListEmp_Number();
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по месяцам
        /// </summary>
        private void BtRepUseOfWorkTime_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepUseOfWorkTime();
        }

        /// <summary>
        /// Отчет - Использование рабочего времени по подразделениям
        /// </summary>
        private void BtRepUseOfWorkTimeOnSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepUseOfWorkTimeOnSub();
        }

        /// <summary>
        /// Создание отчета Сводный отчет по неотработанному времени и ССЧ
        /// </summary>
        private void BtRepTimeNotWorker_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepTimeNotWorker();
        }

        /// <summary>
        /// Создание отчета Отчет по неявкам (подразделений)
        /// </summary>
        private void BtRepTimeNotWorkerOnSub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepTimeNotWorkerOnSub();
        }

        /// <summary>
        /// Отработанные часы 102 ш.о. по заказам
        /// </summary>
        private void BtRepHoursByOrdersPlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepHoursByOrdersPlant();
        }
        
        /// <summary>
        /// Отчет по неотработанному рабочему времени в связи с переходом на режим неполного рабочего времени
        /// </summary>
        private void BtRep_Pay_Type_545_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.Rep_Pay_Type_545();
        }

        /// <summary>
        /// Отчет об использовании сверхурочного рабочего времени (новый отчет по лимитам)
        /// </summary>
        private void BtRepAll_Limits_By_Subdiv_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportClasses.RepAll_Limits_By_Subdiv();
        }

        /// <summary>
        /// Формирование отчета по оправдательным документам сразу по заводу
        /// </summary>
        private void BtRepDocsOnPay_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportDocsOnPay_Type reportDocs = new ReportDocsOnPay_Type(
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                0, "");
            reportDocs.ShowDialog();
        }

#endregion
        private void BtLimit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ViewLimit));
            if (t == null)
            {
                OpenTabs.AddNewTab("Лимиты", new WindowsForms_Viewer(new ViewLimit()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtLimit303_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(ViewLimit303));
            if (t == null)
            {
                OpenTabs.AddNewTab("Лимиты (303)", new WindowsForms_Viewer(new ViewLimit303()));
            }
            else
                OpenTabs.SelectedTab = t;
        }

        private void BtViewOrders_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewTabBase t = OpenTabs.GetOpenTabForm(typeof(OrdersOnHoliday));
            if (t == null)
            {
                OpenTabs.AddNewTab("Приказы на выходные (сверхурочные)", new WindowsForms_Viewer(new OrdersOnHoliday(-1)));
            }
            else
                OpenTabs.SelectedTab = t;
        }

#endregion

#region Графики отпусков
        private void BtViewArchivVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            ViewVacsControl v = new ViewVacsControl(true);
            OpenTabs.AddNewTab("Архив отпусков", v);
        }

        private void BtViewMakeVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Отпуска сотрудников", new ViewVacsControl());
        }

        private void BtConfirmVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Формирование плана", new ConfirmVacs());
        }

        private void BtDiagramsVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Диаграммы отпусков", new VacDiagramView());
        }

        private void BtViewEmpLocationVac_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Участки сотрудников", new WindowsForms_Viewer(new EmpSubLocation()));
        }

        #endregion

        #region Штатное расписание
        /// <summary>
        /// Справочник предприятия - структура подразделений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewSubdivPartition_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Структура предприятия", new SubdivPartitionView());
        }

        private void ViewWorkPlace_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Рабочие места", new WorkPlaceViewer());
        }

        private void ViewIndividProtectionCatalog_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenTabs.AddNewTab("Средства индивидуальной защиты", new IndividProtectionEditor());
        }

        private void OpenViewManningTable_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            ManningTableViewer f = new ManningTableViewer();
            f.OwnerTab = OpenTabs.AddNewTab("Штатное расписание", f, new Uri("pack://application:,,,/Kadr;component/Images/document2_3232.png"));
            
        }
        #endregion

        private void DockingManager_DocumentClosed(object sender, Xceed.Wpf.AvalonDock.DocumentClosedEventArgs e)
        {
            OpenTabs.CloseTab((ViewTabBase)e.Document.Content);
        }
    }
}
