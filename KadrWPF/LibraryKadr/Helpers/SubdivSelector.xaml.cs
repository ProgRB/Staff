using System;
using System.Collections.Generic;
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
using Oracle.DataAccess.Client;
using System.Data;
using System.ComponentModel;
using WpfControlLibrary;
using System.Windows.Threading;

namespace LibraryKadr.Helpers
{
    /// <summary>
    /// Interaction logic for SubdivSelector.xaml
    /// </summary>
    public partial class SubdivSelector : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SubdivIdProperty, AppNameRoleProperty, ShowPromtProperty;
        public static readonly DependencyProperty DelaySubdivBindingProperty = DependencyProperty.Register("DelaySubdivBinding", typeof(bool), typeof(SubdivSelector),
            new PropertyMetadata(true, delayBindingPropertyChanged));

        private static void delayBindingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as SubdivSelector)?.SetBindinSubdivID((bool)e.NewValue);
            }
        }

        private void SetBindinSubdivID(bool value)
        {
           /* if (value)
            {
                
                BindingOperations.ClearBinding(CODE_SUBDIV, ComboBox.SelectedValueProperty);
                DelayBinding.SetBinding(CODE_SUBDIV, ComboBox.SelectedValueProperty, new TimeSpan(0, 0, 0, 0, 800), new Binding("SubdivId") { ElementName = "_this" });
            }
            else
            {
                BindingOperations.ClearBinding(CODE_SUBDIV, ComboBox.SelectedValueProperty);
                BindingOperations.SetBinding(CODE_SUBDIV, ComboBox.SelectedValueProperty, new Binding("SubdivId") { ElementName = "_this", UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            }*/
        }

        /// <summary>
        /// Требуется ли отложенная привязка значения подразделения, по умолчанию - истина
        /// </summary>
        public bool DelaySubdivBinding
        {
            get
            {
                return (bool)GetValue(DelaySubdivBindingProperty);
            }
            set
            {
                SetValue(DelaySubdivBindingProperty, value);
            }
        }

        private DataTable t;
        DataSet CurrentDataset;
        DispatcherTimer tm;
        public event RoutedEventHandler SubdivChanged;
        private DataView sub_view;
        static SubdivSelector()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(null);
            metadata.PropertyChangedCallback += new PropertyChangedCallback(ValidateSubdiv);
            SubdivIdProperty = DependencyProperty.Register("SubdivId", typeof(Decimal?), typeof(SubdivSelector), metadata);
            FrameworkPropertyMetadata metadata1 = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            AppNameRoleProperty = DependencyProperty.Register("AppNameRole", typeof(string), typeof(SubdivSelector), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(AppNamePropertyChanged)));
            ShowPromtProperty = DependencyProperty.Register("ShowPromt", typeof(bool), typeof(SubdivSelector), new PropertyMetadata(true));
        }
        private static void AppNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                DataView dv = (d as SubdivSelector).sub_view;
                if (dv != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
                    if (e.NewValue.ToString() != "APSTAFF.SUBDIV")
                        dv.RowFilter = 
                                string.Format("APP_NAME in ({0})", string.Join(",", e.NewValue.ToString().Split(new char[]{',',' '},  StringSplitOptions.RemoveEmptyEntries).Select(r=>"'"+r+"'")));
                    else
                        (d as SubdivSelector).SubdivView= new DataView(AppDataSet.Tables["SUBDIV"], "SUBDIV_ID not in (201)", "CODE_SUBDIV", DataViewRowState.CurrentRows);
            }
        }
        private static void ValidateSubdiv(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubdivSelector s = d as SubdivSelector;
            if (s != null && (e.NewValue as Decimal?) != (e.OldValue as Decimal?))
                s.StartTimer();
        }

        private void StartTimer()
        {
            tm.Stop();
            tm.Start();
        }

        public Decimal? SubdivId
        {
            get { return (Decimal?)GetValue(SubdivIdProperty); }
            set { SetValue(SubdivIdProperty, value); }
        }
        public string AppRoleName
        {
            get
            {
                return (string)GetValue(AppNameRoleProperty);
            }
            set
            {
                SetValue(AppNameRoleProperty, value);
            }
        }

        /// <summary>
        /// Свойство для скрытия приглашения "подразделение" в компоненте
        /// </summary>
        public bool ShowPromt
        {
            get
            {
                return (bool)GetValue(ShowPromtProperty);
            }
            set
            {
                SetValue(ShowPromtProperty, value);
            }
        }

        public SubdivSelector()
        {
            CurrentDataset = new DataSet();
            tm = new DispatcherTimer();
            tm.Tick += Tm_Tick;
            tm.Interval = new TimeSpan(0,0,0,0, 800);
            InitializeComponent();
            SetBindinSubdivID(DelaySubdivBinding);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                    if (AppRoleName == "APSTAFF.SUBDIV")
                        SubdivView = new DataView(AppDataSet.Tables["SUBDIV"], "", "CODE_SUBDIV", DataViewRowState.CurrentRows);
                    else
                        if (LoadAccessSubdiv())
                            SubdivView = new DataView(CurrentDataset.Tables["ACCESS_SUBDIV"], string.Format("APP_NAME in ({0})", string.IsNullOrWhiteSpace(AppRoleName) ? "''" : string.Join(",", AppRoleName.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(r => "'" + r + "'"))), "CODE_SUBDIV", DataViewRowState.CurrentRows);
            }
        }

        /// <summary>
        /// Если таймер тикнул то событие вызываем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tm_Tick(object sender, EventArgs e)
        {
            lock (tm)
            {
                tm.Stop();
            }
            OnSubdivChanged();
        }

        /// <summary>
        /// ПОдгружаем каждый раз список подразделений доступных по заданному фильтру
        /// </summary>
        /// <param name="AppName"></param>
        /// <returns></returns>
        private bool LoadAccessSubdiv()
        {
            if (CurrentDataset.Tables.Contains("ACCESS_SUBDIV"))
            {
                CurrentDataset.Tables["ACCESS_SUBDIV"].Rows.Clear();
            }
            try
            {
                OracleDataAdapter oda = new OracleDataAdapter(string.Format(@"select subdiv_id, code_subdiv, subdiv_name, 
                    app_name, parent_id, sub_actual_sign from apstaff.subdiv_roles_all"), LibraryKadr.Connect.CurConnect);
                oda.TableMappings.Add("Table", "ACCESS_SUBDIV");
                oda.Fill(CurrentDataset);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения доступа подразделений", "АРМ Кадры");
                return false;
            }
        }

        /// <summary>
        /// Код Подразделения
        /// </summary>
        public string CodeSubdiv
        {
            get
            {
                return (SubdivId.HasValue ? sub_view.Table.Select("SUBDIV_ID=" + SubdivId.Value.ToString())[0]["CODE_SUBDIV"].ToString() : "");
            }
        }

        /// <summary>
        /// Код Подразделения
        /// </summary>
        public string SubdivName
        {
            get
            {
                return (SubdivId.HasValue ? sub_view.Table.Select("SUBDIV_ID=" + SubdivId.Value.ToString())[0]["SUBDIV_NAME"].ToString() : "");
            }
        }

        /// <summary>
        /// Код Подразделения
        /// </summary>
        public bool SubActualSign
        {
            get
            {
                return (SubdivId.HasValue ? Convert.ToBoolean(sub_view.Table.Select("SUBDIV_ID=" + SubdivId.Value.ToString())[0]["SUB_ACTUAL_SIGN"]) : true);
            }
        }

        /// <summary>
        /// Представление для показа подразделений
        /// </summary>
        public DataView SubdivView
        {
            get
            {
                return sub_view;
            }
            set
            {
                sub_view = value;
                OnPropertyChanged("SubdivView");
            }
        }

        private void OnSubdivChanged()
        {
            if (SubdivChanged != null)
                SubdivChanged(this, new RoutedEventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
