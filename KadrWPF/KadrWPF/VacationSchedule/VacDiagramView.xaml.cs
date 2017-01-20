using EntityGenerator;
using LibraryKadr;
using Microsoft.Reporting.WinForms;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

namespace VacationSchedule
{
    /// <summary>
    /// Interaction logic for VacDiagramView.xaml
    /// </summary>
    public partial class VacDiagramView : System.Windows.Controls.UserControl, IVacFilter
    {
        private VacDiagramFilterModel _model;
        OracleDataAdapter odaLoadVac;
        BackgroundWorker bw;
        public VacDiagramView()
        {
            try
            {
                _model = new VacDiagramFilterModel();
                odaLoadVac = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Go\SelectVacDiagram.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                odaLoadVac.SelectCommand.BindByName = true;
                odaLoadVac.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
                odaLoadVac.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, Model.DateBegin, ParameterDirection.Input);
                odaLoadVac.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, Model.DateEnd, ParameterDirection.Input);
                odaLoadVac.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, Model.SubdivID, ParameterDirection.Input);
                odaLoadVac.SelectCommand.Parameters.Add("p_type_vac_ids", OracleDbType.Array, null, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                Table = new DataTable();
                bw = new BackgroundWorker();
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            }
            catch
            { }
            InitializeComponent();
            DataContext = Model;
            SetReport();
        }

        

        public VacDiagramFilterModel Model
        {
            get
            {
                return _model;
            }
        }

        public DataTable Table
        {
            get;
            set;
        }

        private void SetReport()
        {
            repViewer.Load += new EventHandler(repViewer_Load);
            repViewer.ReportExport += new ExportEventHandler(repViewer_ReportExport);
            string path = "Rep_VacDiagram.rdlc";
            repViewer.LocalReport.LoadReportDefinition(File.OpenRead(System.Windows.Forms.Application.StartupPath + @"\Reports\" + path));
            UpdateReport();
        }

        /// <summary>
        /// Параметры вложенного отчета отчета
        /// </summary>
        public IEnumerable<ReportParameter> Parameters
        {
            get 
            {
                return new ReportParameter[] 
                { 
                        new ReportParameter("P_DATE1", Model.DateBegin.Value.ToShortDateString()),
                        new ReportParameter("P_DATE2", Model.DateEnd.Value.ToShortDateString()),
                        new ReportParameter("P_CODE_SUBDIV", Model.CodeSubdiv)
                };
            }
        }

        /// <summary>
        /// Обновляем данные из бд
        /// </summary>
        public void UpdateSource()
        {
            if (Model.IsLoadBusy || bw.IsBusy)
                return;
            Model.IsLoadBusy = true;
            bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model.IsLoadBusy = false;
            if (e.Cancelled) return;
            else if (e.Error != null)
                System.Windows.MessageBox.Show(e.Error.GetFormattedException(), "Ошибка загрузки данных");
            else
            {
                UpdateReport();
            }
        }

        /// <summary>
        /// Асинхронная загрузка данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            odaLoadVac.SelectCommand.Parameters["p_date_begin"].Value = Model.DateBegin;
            odaLoadVac.SelectCommand.Parameters["p_date_end"].Value = Model.DateEnd;
            odaLoadVac.SelectCommand.Parameters["p_subdiv_id"].Value = Model.SubdivID;
            odaLoadVac.SelectCommand.Parameters["p_type_vac_ids"].Value = Model.SelectedTypeVacIDs;
            Table.Rows.Clear();
            odaLoadVac.Fill(Table);
        }
        
        /// <summary>
        /// Обновление самого отчета
        /// </summary>
        public void UpdateReport()
        {
            if (Parameters != null)
                repViewer.LocalReport.SetParameters(Parameters);
            repViewer.LocalReport.DataSources.Clear();
            repViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", Table));
            repViewer.RefreshReport();
        }

        #region Технические вставки для подсказок и тп
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
            ReportViewer rep = sender as ReportViewer;
            ToolStrip toolStrip = (ToolStrip)rep.Controls.Find("toolStrip1", true)[0];
            ToolStripDropDownButton export = (ToolStripDropDownButton)toolStrip.Items.Find("export", true)[0];
            export.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            export.DropDownOpening += new EventHandler(export_DropDownOpening);
            
        }

        void export_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownButton b = (ToolStripDropDownButton)sender;
            b.DropDownItems.Clear();
            RenderingExtension[] extensions = repViewer.LocalReport.ListRenderingExtensions();
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
                    item.Enabled = true;
                    dropDown.DropDownItems.Add(item);
                }
            }
        }

        void openExportDialog(object sender, EventArgs e)
        {
            RenderingExtension ext = (RenderingExtension)((sender as ToolStripItem).Tag);
            string FileName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\1" + GetRenderExtString(ext);
            SaveFileDialog sf = new SaveFileDialog();
            sf.OverwritePrompt = true;
            sf.Filter = string.Format("Документ {0} ({1})|*{1}", ext.Name, GetRenderExtString(ext));
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = sf.FileName;
                if (this.repViewer.ExportDialog(ext, null, FileName) == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(FileName))
                        Process.Start(FileName);
                }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateSource();
        }

        public string GetPerNum()
        {
            return string.Empty;
        }

        public decimal? GetSubdivID()
        {
            return Model.SubdivID;
        }

        public string GetCodeSubdiv()
        {
            return Model.CodeSubdiv;
        }

        public string GetSubdivName()
        {
            return Model.SubdivName;
        }

        public int GetCurrentYear()
        {
            return Model.DateBegin.Value.Year;   
        }
    }

    /// <summary>
    /// Модель для фильтра диаграммы
    /// </summary>
    public class VacDiagramFilterModel: NotificationObject
    {
        OracleDataAdapter odaData;
        List<TypeVacSel> _typeVacSource;
        public VacDiagramFilterModel()
        {
            ds = new DataSet();
            odaData = new OracleDataAdapter(String.Format(Queries.GetQuery(@"go\SelectVacDiagramData.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
            odaData.SelectCommand.BindByName = true;
            odaData.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaData.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
            odaData.TableMappings.Add("Table", "TYPE_VAC");
            odaData.TableMappings.Add("Table1", "SUBDIV");
            try
            {
                odaData.Fill(ds);
            }
            catch (Exception ex)
            { 
            }
            if (ds.Tables.Contains("TYPE_VAC"))
            { 
                ds.Tables["TYPE_VAC"].Columns.Add("IS_CHECKED", typeof(Boolean));
                _typeVacSource = ds.Tables["TYPE_VAC"].Rows.OfType<DataRow>().Select(r => new TypeVacSel() { DataRow = r }).OrderBy(r=>r.NumberCalc).ToList();
                foreach (var p in TypeVacSource)
                { 
                    if (p.SingPayment>0)
                        p.IsChecked=true;
                }
            }
            else
                _typeVacSource = new List<TypeVacSel>();
        }

        private DateTime? _dateBegin  = new DateTime(DateTime.Today.Year, 1, 1);
        public DateTime? DateBegin
        {
            get
            {
                return _dateBegin;
            }
            set
            {
                _dateBegin = value;
                RaisePropertyChanged(() => DateBegin);
            }
        }

        private DateTime? _dateEnd = new DateTime(DateTime.Today.Year, 12, 31);
        private decimal? _subdivID;

        
        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public DateTime? DateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                _dateEnd = value;
                RaisePropertyChanged(() => DateEnd);
            }
        }

        /// <summary>
        /// Выбранный айдишник подразделения
        /// </summary>
        public decimal? SubdivID
        {
            get
            {
                return _subdivID;
            }
            set
            {
                _subdivID = value;
                RaisePropertyChanged(() => SubdivID);
            }
        }

        private DataSet ds;
        private bool _isBusy = false;

        /// <summary>
        /// Источник данных для списка подразделений
        /// </summary>
        public IEnumerable<EntityGenerator.Subdiv> SubdivSource
        {
            get
            {
                return ds.Tables["SUBDIV"].Rows.OfType<DataRow>()
                    .Select(r => new EntityGenerator.Subdiv() { DataRow = r})
                    .OrderBy(r=>r.CodeSubdiv);
            }
        }

        /// <summary>
        /// Выбранное подразделение  в фильтре
        /// </summary>
        public string CodeSubdiv
        {
            get
            {
                return SubdivSource.Where(r=>r.SubdivID ==this.SubdivID).Select(r=>r.CodeSubdiv).FirstOrDefault();
            }
        }
        public string SubdivName
        {
            get
            {
                return SubdivSource.Where(r=>r.SubdivID ==this.SubdivID).Select(r=>r.SubdivName).FirstOrDefault();
            }
        }

        /// <summary>
        /// Источник данных типы отпусков
        /// </summary>
        public List<TypeVacSel> TypeVacSource
        {
            get
            {
                return _typeVacSource;
            }
        }

        /// <summary>
        /// Выбранные типы оптусков
        /// </summary>
        public decimal[] SelectedTypeVacIDs
        {
            get
            {
                return _typeVacSource.Where(r => r.IsChecked).Select(r => r.TypeVacID.Value).ToArray();
            }
        }

        /// <summary>
        /// Занят ли сейчас адаптер и загрузка)
        /// </summary>
        public bool IsLoadBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsLoadBusy);
                RaisePropertyChanged(() => IsLoadNotBusy);
            }
        }

        public bool IsLoadNotBusy
        {
            get
            {
                return !_isBusy;
            }
        }

    }

    /// <summary>
    /// Класс для возможности выбора данных по типам отпусков
    /// </summary>
    public partial class TypeVacSel : TypeVac
    { 
        [System.Data.Linq.Mapping.Column(Name="IS_CHECKED")]
        public Boolean IsChecked
        {
            get
            {
                return this.GetDataRowField<Boolean>(()=>IsChecked);
            }
            set
            {
                this.UpdateDataRow(() => IsChecked, value);
            }
        }
    }

}
