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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LibraryKadr
{
    /// <summary>
    /// Interaction logic for WPFMonthCalendar.xaml
    /// </summary>
    public partial class WPFMonthCalendar : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(WPFMonthCalendar), 
            new FrameworkPropertyMetadata(new DateTime(DateTime.Now.Year,DateTime.Now.Month,1), FrameworkPropertyMetadataOptions.AffectsRender));
        public ObservableCollection<LibCalendarDay> list_s = new ObservableCollection<LibCalendarDay>();
        public WPFMonthCalendar()
        {
            InitializeComponent();
            for (int i = 0; i < 42; i++)
            {
                list_s.Add(new LibCalendarDay());
                list_s[i].DayIndex=i;
            }
            this.DataContext = list_s;
        }
        public DateTime SelectedDate
        {
            get
            {
                return (DateTime)GetValue(SelectedDateProperty);
            }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }

    }

    /// <summary>
    /// Класс являющийся источником данных для шаблона дня в календаре
    /// </summary>
    public class LibCalendarDay: INotifyPropertyChanged
    {
        SolidColorBrush _dayColor = Brushes.LightGray, _dayBackground = Brushes.Transparent, _dayBorder = Brushes.Transparent;
        string _dayText;
        public static SolidColorBrush DefaultDayColor = Brushes.LightGray, DefaultDayBackground = Brushes.Transparent, DefaultDayBorder = Brushes.Transparent;
        public SolidColorBrush DayColor
        {
            get
            {
                return _dayColor;
            }
            set
            {
                _dayColor = value;
                OnPropertyChanged("DayColor");
            }
        }
        public String DayText
        {
            get
            {
                return _dayText;
            }
            set
            {
                _dayText = value;
                OnPropertyChanged("DayText");
            }
        }
        public SolidColorBrush DayBackground
        {
            get
            {
                return _dayBackground;
            }
            set
            {
                _dayBackground = value;
                OnPropertyChanged("DayBackground");
            }
        }
        public SolidColorBrush DayBorder
        {
            get
            {
                return _dayBorder;
            }
            set
            {
                _dayBorder = value;
                OnPropertyChanged("DayBorder");
            }
        }
        
        public int DayIndex
        {
            get;
            set;
        }
        public LibCalendarDay() 
        { }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public static class DateTimeExtension
    {
        public static int DaysInMonth(this DateTime t)
        {
            return DateTime.DaysInMonth(t.Year, t.Month);
        }
    }

    public class IndexConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value%7;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CalDay : Visual
    {
 
    }
}
