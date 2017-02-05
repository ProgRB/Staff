using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace LibraryKadr
{
    public partial class ReportViewerWindow : Form
    {
        public ReportViewerWindow(string nameReport, string pathToReport, DataSet dsReport, List<ReportParameter> listParams)
        {
            InitializeComponent();
            this.Text += nameReport;
            _reportViewer.Load += new EventHandler(ReportViewerWindow_Load);
            _reportViewer.Reset();
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + @"\" + pathToReport;
            if (dsReport.Tables.Contains("REPORT"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Report", dsReport.Tables["REPORT"]));
            if (dsReport.Tables.Contains("VS_REPORT"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_VS_Report", dsReport.Tables["VS_REPORT"]));
            if (dsReport.Tables.Contains("HEADING"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Heading", dsReport.Tables["HEADING"]));
            if (dsReport.Tables.Contains("SIGNES"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Signes", dsReport.Tables["SIGNES"]));
            if (dsReport.Tables.Contains("ORDER_EMP"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Emp", dsReport.Tables["ORDER_EMP"]));
            if (dsReport.Tables.Contains("ORDER_EMP_COND"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Condition", dsReport.Tables["ORDER_EMP_COND"]));
            if (dsReport.Tables.Contains("APPROVAL"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Approval", dsReport.Tables["APPROVAL"]));
            if (dsReport.Tables.Contains("TABLE"))
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DT_Table", dsReport.Tables["TABLE"]));
            if (listParams != null)
                _reportViewer.LocalReport.SetParameters(listParams);
            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
        }

        public ReportViewerWindow(string nameReport, string pathToReport, DataTable[] dtTables, List<ReportParameter> listParams)
        {
            InitializeComponent();
            this.Text += nameReport;
            _reportViewer.Load += new EventHandler(ReportViewerWindow_Load);
            _reportViewer.Reset();
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + @"\" + pathToReport;

            if (dtTables != null)
                for (int i = 1; i <= dtTables.Length; ++i)
                    _reportViewer.LocalReport.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), dtTables[i - 1]));

            if (listParams != null)
                _reportViewer.LocalReport.SetParameters(listParams);
            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
        }

        public ReportViewerWindow(string nameReport, string pathToReport, DataSet dsReport, List<ReportParameter> listParams, bool newContructor = true)
        {
            InitializeComponent();
            this.Text += nameReport;
            _reportViewer.Load += new EventHandler(ReportViewerWindow_Load);
            _reportViewer.Reset();
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + @"\" + pathToReport;
            for (int i = 0; i < dsReport.Tables.Count; i++ )
            {
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet"+(i+1).ToString(), dsReport.Tables[i]));
            }
            if (listParams != null)
                _reportViewer.LocalReport.SetParameters(listParams);
            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
        }

        public ReportViewerWindow(string[] enabledExtensions = null)
        {
            this.EnabledExportExtension = enabledExtensions ?? new string[] { "WORD", "PDF", "EXCEL" };
            InitializeComponent();
            this._reportViewer.Load += new EventHandler(repViewer_Load);
            this._reportViewer.ReportExport += new ExportEventHandler(repViewer_ReportExport);
        }

        private void ReportViewerWindow_Load(object sender, EventArgs e)
        {
            this._reportViewer.RefreshReport();
        }
        public static void ShowReport(string nameReport, string path, DataTable table, IEnumerable<ReportParameter> r, System.Drawing.Printing.Duplex duplex = System.Drawing.Printing.Duplex.Simplex, string[] exportFormats = null)
        {
            ShowReport(nameReport, path, new DataTable[] { table }, r, duplex, exportFormats);
        }

        public static void ShowReport(string nameReport, string path, DataTable[] tables, IEnumerable<ReportParameter> r, System.Drawing.Printing.Duplex duplex = System.Drawing.Printing.Duplex.Simplex, string[] exportFormats = null)
        {
            ReportViewerWindow f = new ReportViewerWindow(exportFormats);
            f.Text += nameReport;
            f._reportViewer.LocalReport.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
            f._reportViewer.PrinterSettings.Duplex = duplex;
            f._reportViewer.LocalReport.DataSources.Clear();
            //f.repViewer.LocalReport.
            if (tables != null)
                for (int i = 1; i <= tables.Length; ++i)
                    f._reportViewer.LocalReport.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), tables[i - 1]));
            if (r != null)
                f._reportViewer.LocalReport.SetParameters(r);
            f._reportViewer.RefreshReport();
            f._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            f.ShowDialog();
        }

        public static WaitDialogForm wait_form;
        
        /// <summary>
        /// Выгрузка отчета в Эксель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="path">Имя шаблона отчета</param>
        /// <param name="tables">Таблица с данными</param>
        /// <param name="r">Параметры отчета</param>
        public static void RenderToExcel(Form sender, string path, DataTable table, List<ReportParameter> r)
        {
            RenderToExcel(sender, path, new DataTable[] { table }, r);
        }

        /// <summary>
        /// Выгрузка отчета в Эксель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="path">Имя шаблона отчета</param>
        /// <param name="tables">Таблицы с данными</param>
        /// <param name="r">Параметры отчета</param>
        public static void RenderToExcel(Form sender, string path, DataTable[] tables, List<ReportParameter> r)
        {
            BackgroundWorker bw = new BackgroundWorker();
            wait_form = new WaitDialogForm();
            wait_form.Owner = sender;
            wait_form.Show();
            bw.DoWork += (bwk, e) =>
                {
                    LocalReport f = new LocalReport();
                    f.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
                    f.DataSources.Clear();
                    //f.repViewer.LocalReport.
                    tables  = e.Argument as DataTable[];
                    if (tables != null)
                        for (int i = 1; i <= tables.Length; ++i)
                            f.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), tables[i - 1]));
                    if (r != null)
                        f.SetParameters(r);
                    e.Result = f.Render("Excel");
                };
            bw.RunWorkerCompleted += (bwk, e) =>
                {
                    if (wait_form != null && !wait_form.IsDisposed && wait_form.Visible)
                    {
                        wait_form.Close();
                    }
                    if (e.Error != null)
                        System.Windows.Forms.MessageBox.Show(e.Error.Message, "Ошибка формирования отчета");
                    else
                    {
                        bool fl = true;
                        bool file_saved = false;
                        SaveFileDialog sf = new SaveFileDialog();
                        sf.Filter = "Файлы Excel (xls)|*.xls";
                        while (fl)
                        {
                            try
                            {
                                if (sf.ShowDialog(sender) == DialogResult.OK)
                                {
                                    File.WriteAllBytes(sf.FileName, (byte[])e.Result);

                                    fl = false;
                                    file_saved = true;
                                }
                                else fl = false;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(sender, ex.Message, "Сохранения");
                            }
                        }
                        if (file_saved)
                            try
                            {
                                System.Diagnostics.Process.Start(sf.FileName);
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message, "Ошибка запуска файла");
                            }

                    }
                };
            bw.RunWorkerAsync(tables);              
        }
        
        /// <summary>
        /// Выгрузка отчета в Эксель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="path">Имя шаблона отчета</param>
        /// <param name="tables">Таблица с данными</param>
        /// <param name="r">Параметры отчета</param>
        /// <param name="nameTempFile">Имя для выгружаемого файла (без расширения)</param>
        public static void RenderToExcel(Form sender, string path, DataTable table, List<ReportParameter> r, string nameTempFile, string extensionFile = "xls")
        {
            RenderToExcel(sender, path, new DataTable[] { table }, r, nameTempFile, extensionFile);
        }

        /// <summary>
        /// Выгрузка отчета в Эксель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="path">Имя шаблона отчета</param>
        /// <param name="tables">Таблицы с данными</param>
        /// <param name="r">Параметры отчета</param>
        /// <param name="nameTempFile">Имя для выгружаемого файла (без расширения)</param>
        public static void RenderToExcel(Form sender, string path, DataTable[] tables, List<ReportParameter> r, 
            string nameTempFile, string extensionFile = "xls")
        {
            BackgroundWorker bw = new BackgroundWorker();
            wait_form = new WaitDialogForm();
            wait_form.Owner = sender;
            wait_form.Show();
            bw.DoWork += (bwk, e) =>
            {
                LocalReport f = new LocalReport();
                f.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
                f.DataSources.Clear();
                //f.repViewer.LocalReport.
                tables = e.Argument as DataTable[];
                if (tables != null)
                    for (int i = 1; i <= tables.Length; ++i)
                        f.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), tables[i - 1]));
                if (r != null)
                    f.SetParameters(r);
                switch (extensionFile)
                {
                    case "xls":
                        e.Result = f.Render("Excel");
                        break;
                    case "doc":
                        e.Result = f.Render("Word");
                        break;
                    case "pdf":
                        e.Result = f.Render("PDF");
                        break;
                    default:
                        break;
                }
            };
            bw.RunWorkerCompleted += (bwk, e) =>
            {
                if (wait_form != null && !wait_form.IsDisposed && wait_form.Visible)
                {
                    wait_form.Close();
                }
                if (e.Error != null)
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Ошибка формирования отчета");
                else
                {
                    //bool fl = true;
                    bool file_saved = false;
                    SaveFileDialog sf = new SaveFileDialog();
                    //sf.Filter = "Файлы Excel (xls)|*.xls";
                    //while (fl)
                    {
                        try
                        {
                            sf.FileName = System.IO.Path.GetTempPath() + nameTempFile + "." + extensionFile;
                            //if (sf.ShowDialog(sender) == DialogResult.OK)
                            {
                                File.WriteAllBytes(sf.FileName, (byte[])e.Result);

                                //fl = false;
                                file_saved = true;
                            }
                            //else fl = false;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(sender, ex.Message, "Сохранения");
                        }
                    }
                    if (file_saved)
                        try
                        {
                            System.Diagnostics.Process.Start(sf.FileName);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Ошибка запуска файла");
                        }

                }
            };
            bw.RunWorkerAsync(tables);
        }

        /// <summary>
        /// Выгрузка отчета в Эксель и обработка формул
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="path">Имя шаблона отчета</param>
        /// <param name="tables">Таблицы с данными</param>
        /// <param name="r">Параметры отчета</param>
        /// <param name="nameTempFile">Имя для выгружаемого файла (без расширения)</param>
        /// <param name="nameTempFile">Имя для выгружаемого файла (без расширения)</param>
        public static void RenderToExcelWithFormulas(Form sender, string path, DataTable[] tables, List<ReportParameter> r)
        {
            BackgroundWorker bw = new BackgroundWorker();
            wait_form = new WaitDialogForm();
            wait_form.Owner = sender;
            wait_form.Show();
            bw.DoWork += (bwk, e) =>
            {
                LocalReport f = new LocalReport();
                f.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
                f.DataSources.Clear();
                //f.repViewer.LocalReport.
                tables = e.Argument as DataTable[];
                if (tables != null)
                    for (int i = 1; i <= tables.Length; ++i)
                        f.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), tables[i - 1]));
                if (r != null)
                    f.SetParameters(r);
                e.Result = f.Render("Excel");
            };
            bw.RunWorkerCompleted += (bwk, e) =>
            {
                if (wait_form != null && !wait_form.IsDisposed && wait_form.Visible)
                {
                    wait_form.Close();
                }
                if (e.Error != null)
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Ошибка формирования отчета");
                else
                {
                    bool fl = true;
                    bool file_saved = false;
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "Файлы Excel (xls)|*.xls";
                    while (fl)
                    {
                        try
                        {
                            if (sf.ShowDialog(sender) == DialogResult.OK)
                            {
                                File.WriteAllBytes(sf.FileName, (byte[])e.Result);
                                fl = false;
                                file_saved = true;
                            }
                            else fl = false;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(sender, ex.Message, "Сохранения");
                        }
                    }
                    if (file_saved)
                        try
                        {
                            //System.Diagnostics.Process.Start(sf.FileName);
                            Microsoft.Office.Interop.Excel.Application m_ExcelApp;
                            //Создание книги Excel
                            Microsoft.Office.Interop.Excel._Workbook m_Book;
                            //private Excel.Range Range;
                            Microsoft.Office.Interop.Excel.Workbooks m_Books;
                            object oMissing = System.Reflection.Missing.Value;
                            m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                            m_ExcelApp.Visible = false;
                            m_ExcelApp.DisplayAlerts = false;
                            m_Books = m_ExcelApp.Workbooks;
                            m_Book = m_Books.Open(sf.FileName, oMissing, oMissing,
                                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                            m_ExcelApp.ReferenceStyle = Microsoft.Office.Interop.Excel.XlReferenceStyle.xlR1C1;
                            for (int i = 1; i <= m_ExcelApp.Sheets.Count; i++)
                            {
                                ((Microsoft.Office.Interop.Excel._Worksheet)m_ExcelApp.Sheets[i]).Cells.Replace("'", "");
                            }
                            m_ExcelApp.ScreenUpdating = true;
                            m_ExcelApp.Calculation = Microsoft.Office.Interop.Excel.XlCalculation.xlCalculationAutomatic;
                            m_ExcelApp.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Ошибка запуска файла");
                        }

                }
            };
            bw.RunWorkerAsync(tables);
        }

        /*public static void DisableAllExportFormat(ReportViewer ReportViewerID)
        {
            DisableUnwantedExportFormat(ReportViewerID, "PDF");
            DisableUnwantedExportFormat(ReportViewerID, "Excel");
            DisableUnwantedExportFormat(ReportViewerID, "Word");
        }*/

        public static void RenderToWord(Form sender, string path, DataTable table, List<ReportParameter> r)
        {
            RenderToWord(sender, path, new DataTable[] { table }, r);
        }

        public static void RenderToWord(Form sender, string path, DataTable[] tables, List<ReportParameter> r)
        {
            BackgroundWorker bw = new BackgroundWorker();
            wait_form = new WaitDialogForm();
            wait_form.Owner = sender;
            wait_form.Show();
            bw.DoWork += (bwk, e) =>
            {
                LocalReport f = new LocalReport();
                f.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
                f.DataSources.Clear();
                //f.repViewer.LocalReport.
                tables = e.Argument as DataTable[];
                if (tables != null)
                    for (int i = 1; i <= tables.Length; ++i)
                        f.DataSources.Add(new ReportDataSource(string.Format("DataSet{0}", i), tables[i - 1]));
                if (r != null)
                    f.SetParameters(r);
                e.Result = f.Render("Word");
            };
            bw.RunWorkerCompleted += (bwk, e) =>
            {
                if (wait_form != null && !wait_form.IsDisposed && wait_form.Visible)
                {
                    wait_form.Close();
                }
                if (e.Error != null)
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Ошибка формирования отчета");
                else
                {
                    bool fl = true;
                    bool file_saved = false;
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "Файлы Excel (doc)|*.doc";
                    while (fl)
                    {
                        try
                        {
                            if (sf.ShowDialog(sender) == DialogResult.OK)
                            {
                                File.WriteAllBytes(sf.FileName, (byte[])e.Result);

                                fl = false;
                                file_saved = true;
                            }
                            else fl = false;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(sender, ex.Message, "Сохранения");
                        }
                    }
                    if (file_saved)
                        try
                        {
                            System.Diagnostics.Process.Start(sf.FileName);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Ошибка запуска файла");
                        }

                }
            };
            bw.RunWorkerAsync(tables);
        }

        public static void DisableUnwantedExportFormat(ReportViewer ReportViewerID, string strFormatName)
        {
            FieldInfo info;

            foreach (RenderingExtension extension in ReportViewerID.LocalReport.ListRenderingExtensions())
            {
                if (extension.Name == strFormatName)
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }
            }
        }

        #region Вспомогательные данные по подсказами и экспорту
        void repViewer_ReportExport(object sender, ReportExportEventArgs e)
        {
            e.Cancel = true;
        }

        public string[] EnabledExportExtension
        {
            get;
            set;
        }
        void repViewer_Load(object sender, EventArgs e)
        {
            /*try
            {*/
            ReportViewer rep = sender as ReportViewer;
            ToolStrip toolStrip = (ToolStrip)rep.Controls.Find("toolStrip1", true)[0];
            ToolStripButton firstPage = (ToolStripButton)toolStrip.Items.Find("firstPage", true)[0];
            firstPage.AutoToolTip = false;
            firstPage.ToolTipText = "На первую страницу";
            ToolStripButton previousPage = (ToolStripButton)toolStrip.Items.Find("previousPage", true)[0];
            previousPage.AutoToolTip = false;
            previousPage.ToolTipText = "Предыдущая страница";

            ToolStripTextBox currentPage = (ToolStripTextBox)toolStrip.Items.Find("currentPage", true)[0];
            currentPage.AutoToolTip = false;
            currentPage.ToolTipText = "Текущая страница";

            ToolStripLabel totalPages = (ToolStripLabel)toolStrip.Items.Find("totalPages", true)[0];
            totalPages.AutoToolTip = false;
            totalPages.ToolTipText = "Всего страниц";

            ToolStripButton nextPage = (ToolStripButton)toolStrip.Items.Find("nextPage", true)[0];
            nextPage.AutoToolTip = false;
            nextPage.ToolTipText = "Следующая страница";
            ToolStripButton lastPage = (ToolStripButton)toolStrip.Items.Find("lastPage", true)[0];
            lastPage.AutoToolTip = false;
            lastPage.ToolTipText = "На последнюю страницу";

            ToolStripButton stop = (ToolStripButton)toolStrip.Items.Find("stop", true)[0];
            stop.AutoToolTip = false;
            stop.ToolTipText = "Остановить подготовку к просмотру";
            ToolStripButton refresh = (ToolStripButton)toolStrip.Items.Find("refresh", true)[0];
            refresh.AutoToolTip = false;
            refresh.ToolTipText = "Обновить";

            ToolStripButton print = (ToolStripButton)toolStrip.Items.Find("print", true)[0];
            print.AutoToolTip = false;
            print.Text = "Печать";
            print.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            print.ToolTipText = "Печать";
            ToolStripButton printPreview = (ToolStripButton)toolStrip.Items.Find("printPreview", true)[0];
            printPreview.AutoToolTip = false;
            printPreview.ToolTipText = "Предварительный просмотр";

            ToolStripButton pageSetup = (ToolStripButton)toolStrip.Items.Find("pageSetup", true)[0];
            pageSetup.AutoToolTip = false;
            pageSetup.ToolTipText = "Параметры страницы";


            ToolStripTextBox textToFind = (ToolStripTextBox)toolStrip.Items.Find("textToFind", true)[0];
            textToFind.AutoToolTip = false;
            textToFind.ToolTipText = "Введите текст для поиска в отчете (недоступно в предварительном просмотре)";

            ToolStripButton find = (ToolStripButton)toolStrip.Items.Find("find", true)[0];
            find.AutoToolTip = false;
            find.ToolTipText = "Найти этот текст в отчете";
            ToolStripButton findNext = (ToolStripButton)toolStrip.Items.Find("findNext", true)[0];
            findNext.AutoToolTip = false;
            findNext.ToolTipText = "Найти следующий";

            ToolStripDropDownButton export = (ToolStripDropDownButton)toolStrip.Items.Find("export", true)[0];
            export.AutoToolTip = false;
            export.Text = "Экспорт";
            export.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            export.ToolTipText = "Экспорт в другой формат";

            export.DropDownOpening += new EventHandler(export_DropDownOpening);
        }

        void export_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownButton b = (ToolStripDropDownButton)sender;
            b.DropDownItems.Clear();
            RenderingExtension[] extensions = _reportViewer.LocalReport.ListRenderingExtensions();
            Populate(b, openExportDialog, extensions, this.EnabledExportExtension);
        }

        public static void Populate(ToolStripDropDownItem dropDown, EventHandler handler, RenderingExtension[] extensions, string[] EnabledExtension)
        {
            dropDown.DropDownItems.Clear();
            foreach (RenderingExtension extension in extensions)
            {
                if (ShouldDisplay(extension))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = extension.LocalizedName;
                    item.Tag = extension;
                    item.ToolTipText = "Экспорт в формат " + extension.LocalizedName;
                    if (handler != null)
                    {
                        item.Click += handler;
                    }
                    if (EnabledExtension == null || EnabledExtension.Count(r => r.ToUpper() == (item.Text.ToUpper())) == 0)
                        item.Enabled = false;
                    dropDown.DropDownItems.Add(item);
                }
            }
        }
        void openExportDialog(object sender, EventArgs e)
        {
            RenderingExtension ext = (RenderingExtension)((sender as ToolStripItem).Tag);
            string FileName = UserSpecialFolder + @"\1" + GetRenderExtString(ext);
            SaveFileDialog sf = new SaveFileDialog();
            sf.OverwritePrompt = true;
            sf.Filter = string.Format("Документ {0} ({1})|*{1}", ext.Name, GetRenderExtString(ext));
            if (sf.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                FileName = sf.FileName;
                if (this._reportViewer.ExportDialog(ext, null, FileName) == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(FileName))
                        Process.Start(FileName);
                }
            }
        }

        public string UserSpecialFolder
        {
            get
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }
        private string GetRenderExtString(RenderingExtension r)
        {
            switch (r.LocalizedName.ToUpper())
            {
                case "WORD": return ".docx"; break;
                case "EXCEL": return ".xlsx"; break;
                case "PDF": return ".pdf"; break;
                default: return string.Empty;
            }
            return null;
        }

        private static bool ShouldDisplay(RenderingExtension extension)
        {
            return extension.Visible;
        }
        #endregion


    }

}
