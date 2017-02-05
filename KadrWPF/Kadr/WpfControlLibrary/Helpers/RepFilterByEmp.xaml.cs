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
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace Helpers
{
    /// <summary>
    /// Interaction logic for RepFilterByEmp.xaml
    /// </summary>
    public partial class RepFilterByEmp : Window, INotifyPropertyChanged
    {
        private List<DataRowView> _source;
        private decimal? subdiv_id;
        private DateTime? _date_begin;
        private DateTime? _date_end;
        public RepFilterByEmp(List<DataRowView> _srs, decimal? _subdiv_id, DateTime selectedDate):this(_srs, _subdiv_id, new DateTime(selectedDate.Year, selectedDate.Month,1),
                new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)))
        {
        }

        public RepFilterByEmp(List<DataRowView> _srs, decimal? _subdiv_id, DateTime selectedDateBegin, DateTime selectedDateEnd, bool allowEmpFilter = true, bool allowBeginSelection=true, bool allowEndSelection = true)
        {
            subdiv_id = _subdiv_id;
            _source = _srs;
            _date_begin = selectedDateBegin;
            _date_end = selectedDateEnd;
            InitializeComponent();
            IsEmpListAllowed = allowEmpFilter;
            AllowBegin = allowBeginSelection ? Visibility.Visible : Visibility.Collapsed;
            AllowEnd = allowEndSelection ? Visibility.Visible : Visibility.Collapsed;
            
            dgEmpList.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(TheDataGrid_PreviewMouseLeftButtonDown);
        }

        void TheDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {/*
            // get the DataGridRow at the clicked point
            var o = DataGridHelper.TryFindFromPoint<DataGridRow>(dgEmpList, e.GetPosition(dgEmpList));
            // only handle this when Ctrl or Shift not pressed 
            ModifierKeys mods = Keyboard.PrimaryDevice.Modifiers;
            if (o != null && ((int)(mods & ModifierKeys.Control) == 0 && (int)(mods & ModifierKeys.Shift) == 0))
            {
                o.IsSelected = !o.IsSelected;
                e.Handled = true;
            }*/
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }
        public List<DataRowView> GridSource
        {
            get
            {
                return _source;
            }
        }

        bool _bySubdiv = true;
        public bool BySubdivReport
        {
            get
            {
                return _bySubdiv;
            }
            set
            {
                _bySubdiv = value;
                OnPropertyChanged("BySubdivReport");
            }
        }
        bool _isSubdivAllowed = true;
        public bool IsSubdivAllowed
        {
            get
            {
                return _isSubdivAllowed;
            }
            set
            {
                _isSubdivAllowed = value;
                OnPropertyChanged("IsSubdivAllowed");
            }
        }

        bool _allowEmpFilter = true;
        /// <summary>
        /// Доступен ли список сотрудников для выбора
        /// </summary>
        public bool IsEmpListAllowed
        {
            get
            {
                return _allowEmpFilter;
            }
            set
            {
                _allowEmpFilter = value;
                OnPropertyChanged("IsEmpListAllowed");
                if (value) gridRowMain.Height = new GridLength(1, GridUnitType.Star);
                else gridRowMain.Height = new GridLength(0, GridUnitType.Auto);
            }
        }

        public IList SelectedRows
        {
            get
            {
                return dgEmpList.SelectedItems;
            }
        }

        public Decimal? SubdivID
        {
            get { return subdiv_id; }
            set { subdiv_id = value; OnPropertyChanged("SubdivID"); }
        }

        public DateTime? DateEnd
        {
            get { return _date_end; }
            set { _date_end = value; }
        }

        Visibility _allowBegin = Visibility.Visible;
        public Visibility AllowBegin
        {
            get
            {
                return _allowBegin;
            }
            set
            {
                _allowBegin = value;
                OnPropertyChanged("AllowBegin");
            }
        }

        Visibility _allowEnd = Visibility.Visible;
        public Visibility AllowEnd
        {
            get
            {
                return _allowEnd;
            }
            set
            {
                _allowEnd = value;
                OnPropertyChanged("AllowEnd");
            }
        }

        public DateTime? DateBegin
        {
            get
            {
                return _date_begin;
            }
            set
            {
                _date_begin = value;
                OnPropertyChanged("DateBegin");
            }
        }

        private void PerformCustomSort(DataGridColumn column)
        {
            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ?
                                ListSortDirection.Ascending : ListSortDirection.Descending;
            column.SortDirection = direction;
            ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dgEmpList.ItemsSource);
            MySort mySort = new MySort(direction, column);
            lcv.CustomSort = mySort;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void Check_all_Checked(object sender, RoutedEventArgs e)
        {
            if (checkAll.IsChecked == true)
                dgEmpList.SelectAll();
            else
                dgEmpList.SelectedItems.Clear();

        }

#region Интерфейс получения данных фильтра
        public DateTime GetDate()
        {
            return DateBegin.Value;
        }

        public DateTime GetDateBegin()
        {
            return DateBegin.Value;
        }

        public DateTime GetDateEnd()
        {
            return DateEnd.Value;
        }

        public decimal? GetSubdivID()
        {
            return SubdivID;
        }

        public decimal[] GetDegreeIDs()
        {
            throw new NotImplementedException();
        }
#endregion

        string _dateBeginCaption = "Дата начала периода";
        /// <summary>
        /// надпись перед началом периода
        /// </summary>
        public string TextBeginCaption
        {
            get
            {
                return _dateBeginCaption;            
            }
            set
            {
                _dateBeginCaption = value;
                OnPropertyChanged("TextBeginCaption");
            }
        }
        string _dateEndCaption = "Дата окончания периода";
        /// <summary>
        /// Надпись пере окончанием периода
        /// </summary>
        public string TextEndCaption
        {
            get
            {
                return _dateEndCaption;            
            }
            set
            {
                _dateEndCaption = value;
                OnPropertyChanged("TextEndCaption");
            }
        }
    }

    public class MySort : IComparer
    {
        public MySort(ListSortDirection direction, DataGridColumn column)
        {
            Direction = direction;
            Column = column;
        }

        public ListSortDirection Direction
        {
            get;
            private set;
        }

        public DataGridColumn Column
        {
            get;
            private set;
        }

        int StringCompare(string s1, string s2)
        {
            if (Direction == ListSortDirection.Ascending)
                return s1.CompareTo(s2);
            return s2.CompareTo(s1);
        }

        int IComparer.Compare(object X, object Y)
        {
            DataRowView r1 = X as DataRowView, r2 = Y as DataRowView;
            if (r1[Column.SortMemberPath] is Decimal)
                if (Direction == ListSortDirection.Ascending)
                    return decimal.Compare(r1[Column.SortMemberPath] == DBNull.Value ? 0 : (decimal)r1[Column.SortMemberPath], r2[Column.SortMemberPath] == DBNull.Value ? 0 : (decimal)r2[Column.SortMemberPath]);
                else
                    return decimal.Compare(r2[Column.SortMemberPath] == DBNull.Value ? 0 : (decimal)r2[Column.SortMemberPath], r1[Column.SortMemberPath] == DBNull.Value ? 0 : (decimal)r1[Column.SortMemberPath]);
            else
                if (Direction == ListSortDirection.Ascending)
                    return StringComparer.CurrentCulture.Compare(r1[Column.SortMemberPath] == DBNull.Value ? "" : (string)r1[Column.SortMemberPath], r2[Column.SortMemberPath] == DBNull.Value ? "" : (string)r2[Column.SortMemberPath]);
                else
                    return StringComparer.CurrentCulture.Compare(r2[Column.SortMemberPath] == DBNull.Value ? "" : (string)r2[Column.SortMemberPath], r1[Column.SortMemberPath] == DBNull.Value ? "" : (string)r1[Column.SortMemberPath]);
        }
    }

}
