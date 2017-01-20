using EntityGenerator;
using LibraryKadr;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for GrWorkViewer.xaml
    /// </summary>
    public partial class GrWorkViewer : UserControl
    {

        static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(GrWorkViewer),
            new PropertyMetadata(false, readOnlyChanged));

        /// <summary>
        /// Только для чтения компонент?
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        private static void readOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                Staff.DataGridAddition.SetDoubleClickCommand((d as GrWorkViewer).dgGrWorks, ((bool)e.NewValue ? ApplicationCommands.Open : KadrWPF.Helpers.AppCommands.EditGrWork));
            }
        }

        public GrWorkViewer()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Model = new GrWorkViewModel();
                InitializeComponent();
                DataContext = Model;
            }
            else
                InitializeComponent();
        }

        public GrWorkViewModel Model { get; set; }

        private void WrapPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Model.RefreshGrWork(null);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Model.RefreshGrWork(null);
        }
    }

    public class GrWorkViewModel : NotificationObject
    {
        DataSet ds;
        OracleDataAdapter odaGrWork, odaGrAccess;
        private string _codeSubdiv;
        private DataRowView _currentGrWork;
        private DateTime? _dateEndGraph;

        public GrWorkViewModel()
        {
            ds = new DataSet();
            odaGrWork = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectGrWorkView.sql"), Connect.CurConnect);
            odaGrWork.SelectCommand.BindByName = true;
            odaGrWork.SelectCommand.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2, CodeSubdiv, ParameterDirection.Input);
            odaGrWork.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, DateEndGraph, ParameterDirection.Input);
            odaGrWork.TableMappings.Add("Table", "GR_WORK");

            odaGrAccess = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectAccessGrWorkView.sql"), Connect.CurConnect);
            odaGrAccess.SelectCommand.BindByName = true;
            odaGrAccess.SelectCommand.Parameters.Add("p_gr_work_id", OracleDbType.Decimal, CurrentGrWorkID, ParameterDirection.Input);
            odaGrAccess.TableMappings.Add("Table", "ACCESS_GR_WORK");
        }

        /// <summary>
        /// Текущий выбранный график работы
        /// </summary>
        public DataRowView CurrentGrWork
        {
            get
            {
                return _currentGrWork;
            }
            set
            {
                _currentGrWork = value;
                RaisePropertyChanged(() => CurrentGrWork);
                RefreshAccessGraph();
            }
        }

        /// <summary>
        /// Айдишник текущего графика работы
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_gr_work_id")]
        public decimal? CurrentGrWorkID
        {
            get
            {
                return CurrentGrWork?.Row.Field2<Decimal?>("GR_WORK_ID");
            }
        }

        DataView _grWorkSource, _grWorkAccess;
        /// <summary>
        /// Список графиков работы
        /// </summary>
        public List<DataRowView> GrWorkSource
        {
            get
            {
                if (_grWorkSource == null)
                    _grWorkSource = new DataView(ds.Tables["GR_WORK"], "", "", DataViewRowState.CurrentRows);
                return _grWorkSource?.Cast<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// Список подразделений для графика работы
        /// </summary>
        public List<DataRowView> GrWorkAccessSource
        {
            get
            {
                if (_grWorkAccess == null)
                    _grWorkAccess = new DataView(ds.Tables["ACCESS_GR_WORK"], "", "", DataViewRowState.CurrentRows);
                return _grWorkAccess?.Cast<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// Подразделение для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName="p_code_subdiv")]
        public string CodeSubdiv
        {
            get
            {
                return _codeSubdiv;
            }
            set
            {
                _codeSubdiv = value;
                RaisePropertyChanged(() => CodeSubdiv);
            }
        }

        /// <summary>
        /// Дата окончания графика работы для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName = "p_date_end")]
        public DateTime? DateEndGraph
        {
            get
            {
                return _dateEndGraph;
            }
            set
            {
                _dateEndGraph = value;
                RaisePropertyChanged(() => DateEndGraph);
            }
        }

        /// <summary>
        /// Обновление данных по графикам работы
        /// </summary>
        public void RefreshGrWork(decimal? curGrWorkID)
        {
            decimal? curid = curGrWorkID??CurrentGrWorkID;
            Exception ex = odaGrWork.TryFillWithClear(ds, this);
            if (ex != null)
            {
                MessageBox.Show(ex.GetFormattedException(), "Ошибка получения данных графиков работы");
                return;
            }
            if (curid != null)
                CurrentGrWork = GrWorkSource.Where(r => r.Row.Field2<Decimal?>("GR_WORK_ID") == curid).FirstOrDefault();
            RaisePropertyChanged(() => GrWorkSource);

        }

        /// <summary>
        /// Обновление данных подразделений
        /// </summary>
        public void RefreshAccessGraph()
        {
            Exception ex = odaGrAccess.TryFillWithClear(ds, this);
            if (ex != null)
            {
                MessageBox.Show(ex.GetFormattedException(), "Ошибка получения доступных подразделений");
                return;
            }
            else
                RaisePropertyChanged(() => GrWorkAccessSource);
        }
    }
}
