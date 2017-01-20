using EntityGenerator;
using KadrWPF.Helpers;
using LibraryKadr;
using LibrarySalary.Helpers;
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
using WpfControlLibrary;

namespace ManningTable
{
    /// <summary>
    /// Логика взаимодействия для WorkPlaceViewer.xaml
    /// </summary>
    public partial class WorkPlaceViewer : UserControl
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(WorkPlaceViewer),
            new PropertyMetadata(false, readOnlyChanged));

        private static void readOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                Staff.DataGridAddition.SetDoubleClickCommand((d as WorkPlaceViewer).dgWP, ((bool)e.NewValue ? ApplicationCommands.Open : AppCommands.EditWorkPlace));
            }
        }

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

        private WorkPlaceViewModel _model;

        /// <summary>
        /// Просмотр рабочего места карточки
        /// </summary>
        public WorkPlaceViewer()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                _model = new WorkPlaceViewModel();
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                DataContext = Model;
                Model.SubdivID = Model.SubdivSource.Count > 0 ? Model?.SubdivSource[0].SubdivID : null;
                Model?.RefreshWorkplaceSource();
                if (IsReadOnly)
                    Staff.DataGridAddition.SetDoubleClickCommand(dgWP, ApplicationCommands.Open);
                else
                    Staff.DataGridAddition.SetDoubleClickCommand(dgWP, AppCommands.EditWorkPlace);
            }
        }

        public WorkPlaceViewModel Model
        {
            get
            {
                return _model;
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Model.RefreshWorkplaceSource();
        }

        private void AddPlace_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WorkPlaceEditor f = new WorkPlaceEditor(null);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                Model.RefreshWorkplaceSource();
            }
        }

        private void Edit_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model != null && Model.CurrentWorkPlace != null && !IsReadOnly;
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WorkPlaceEditor f = new WorkPlaceEditor(Model.CurrentWorkPlaceID);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog()==true)
            {
                Model.RefreshWorkplaceSource();
            }
        }

        /// <summary>
        /// Удаление записи из бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить выбранную запись?", "Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                Exception ex = Model.DeleteWorkPlace(Model.CurrentWorkPlaceID);
                if (ex != null)
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка удаления данных");
                else
                    Model.RefreshWorkplaceSource();
                
            }
        }
    }

    public class WorkPlaceViewModel : NotificationObject
    {
        DataView _workPlaceSource, _workPlaceProtSource;
        private decimal? _subdivId;

        DataSet ds;

        OracleDataAdapter odaWorkPlaceProtection, odaWorkPlace;

        public WorkPlaceViewModel()
        {
            ds = new DataSet();
            odaWorkPlace = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectWorkplaceView.sql"), Connect.CurConnect);
            odaWorkPlace.SelectCommand.BindByName = true;
            odaWorkPlace.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, SubdivID, ParameterDirection.Input);
            odaWorkPlace.TableMappings.Add("Table", "WORK_PLACE");

            odaWorkPlaceProtection = new OracleDataAdapter(Queries.GetQuery(@"MT/SelectWorkPlaceProtectionView.sql"), Connect.CurConnect);
            odaWorkPlaceProtection.SelectCommand.BindByName = true;
            odaWorkPlaceProtection.SelectCommand.Parameters.Add("p_work_place_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaWorkPlaceProtection.TableMappings.Add("Table", "WORK_PLACE_PROTECTION");
        }

        /// <summary>
        /// Источник данных список рабочих мест
        /// </summary>
        public List<DataRowView> WorkPlaceSource
        {
            get
            {
                if (_workPlaceSource == null)
                    return null;
                else
                    return _workPlaceSource.OfType<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// ТЕкущая выбранная карта рабочего места
        /// </summary>
        public DataRowView CurrentWorkPlace
        {
            get
            {
                return _currentWorkPlace;
            }
            set
            {
                _currentWorkPlace = value;
                RaisePropertyChanged(() => CurrentWorkPlace);
                RefreshProtectionSource();
            }
        }

        /// <summary>
        /// Айдишник текущего выбранного рабочего места
        /// </summary>
        [OracleParameterMapping(ParameterName ="p_work_place_id")]
        public decimal? CurrentWorkPlaceID
        {
            get
            {
                if (CurrentWorkPlace == null)
                    return null;
                else
                    return CurrentWorkPlace.Row.Field<decimal>("WORK_PLACE_ID");
            }
        }

        /// <summary>
        /// Подразделение выбранное для фильтра
        /// </summary>
        [OracleParameterMapping(ParameterName ="p_subdiv_id")]
        public decimal? SubdivID
        {
            get
            {
                return _subdivId;
            }
            set
            {
                _subdivId = value;
                RaisePropertyChanged(() => SubdivID);
            }
        }

        List<Subdiv> _subdivSource;
        private DataRowView _currentWorkPlace;

        /// <summary>
        /// Список подразделений доступных для фильтра
        /// </summary>
        public List<Subdiv> SubdivSource
        {
            get
            {
                if (_subdivSource == null)
                {
                    _subdivSource = AppDataSet.Tables["ACCESS_SUBDIV"].Select("APP_NAME='MANNING_TABLE'").Select(r=>new Subdiv() { DataRow = r }).ToList();
                }
                return _subdivSource;
            }
        }

        public List<DataRowView> WorkplaceProtectionSource
        {
            get
            {
                if (_workPlaceProtSource==null)
                    return null;
                else
                    return _workPlaceProtSource.OfType<DataRowView>().ToList();
            }
        }

        /// <summary>
        /// Обновление списка карт условий труда
        /// </summary>
        public void RefreshWorkplaceSource()
        {
            odaWorkPlace.SelectCommand.SetParameters(this);
            Exception ex = odaWorkPlace.TryFillWithClear(ds, this);
            if (ex != null)
                MessageBox.Show(ex.GetFormattedException(), "Ошибка загрузки данных");
            if (_workPlaceSource == null)
            {
                if (ds != null && ds.Tables.Contains("WORK_PLACE"))
                {
                    _workPlaceSource = new DataView(ds.Tables["WORK_PLACE"], "", "", DataViewRowState.CurrentRows);
                }
            }
            RaisePropertyChanged(() => WorkPlaceSource);
        }

        /// <summary>
        /// Обновление привязанных средств инд. защиты
        /// </summary>
        public void RefreshProtectionSource()
        {
            odaWorkPlaceProtection.SelectCommand.SetParameters(this);
            Exception ex = odaWorkPlaceProtection.TryFillWithClear(ds, this);
            if (ex != null)
                MessageBox.Show(ex.GetFormattedException(), "Ошибка загрузки данных");
            if (_workPlaceProtSource == null)
            {
                if (ds != null && ds.Tables.Contains("WORK_PLACE_PROTECTION"))
                {
                    _workPlaceProtSource = new DataView(ds.Tables["WORK_PLACE_PROTECTION"], "", "", DataViewRowState.CurrentRows);
                }
            }
            RaisePropertyChanged(() => WorkplaceProtectionSource);
        }

        internal Exception DeleteWorkPlace(decimal? id)
        {
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(@"BEGIN {0}.WORK_PLACE_DELETE(:p_WORK_PLACE_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_WORK_PLACE_ID", OracleDbType.Decimal, id, ParameterDirection.Input);
                cmd.ExecuteNonQuery();
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }
    }
}
