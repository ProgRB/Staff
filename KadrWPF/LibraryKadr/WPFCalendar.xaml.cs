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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Salary;

namespace LibraryKadr
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class WPFCalendar : UserControl
    {
        public static readonly DependencyProperty SelectedYearProperty;
        private static  Dictionary<DateTime, SolidColorBrush> tablecalendar;
        private Dictionary<DateTime, LibCalendarDay> calendar  = new Dictionary<DateTime,LibCalendarDay>();
        private static LibCalendarDay[] static_days = new LibCalendarDay[31];
        private static LibCalendarDay[] static_usual_days = new LibCalendarDay[31];
        private KeyValuePair<DateTime, DateTime> border_day = new KeyValuePair<DateTime, DateTime>(DateTime.MinValue, DateTime.MinValue);
        public WPFCalendar()
        {
            InitializeComponent();
            
            OnYearChanged(this, new DependencyPropertyChangedEventArgs(SelectedYearProperty, DateTime.Now, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
        }

        static WPFCalendar()
        {
            FrameworkPropertyMetadata m = new FrameworkPropertyMetadata(new DateTime(DateTime.Now.Year, 1, 1), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnYearChanged));
            SelectedYearProperty = DependencyProperty.Register("SelectedYear", typeof(DateTime), typeof(WPFCalendar), m);
            for (int i = 0; i < 31; ++i)
            {
                static_days[i] = new LibCalendarDay();
                static_days[i].DayText =(i + 1).ToString();
                static_usual_days[i] = new LibCalendarDay();
                static_usual_days[i].DayText = (i + 1).ToString();
                static_usual_days[i].DayColor = Brushes.Black;
            }
            tablecalendar = new Dictionary<DateTime, SolidColorBrush>();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                try
                {
                    OracleDataTable t = new OracleDataTable(string.Format("select calendar_day, type_day_id from {0}.calendar where type_day_id!=2 and calendar_day>add_months(trunc(sysdate,'year'),-12)", Connect.SchemaApstaff), Connect.CurConnect);
                    t.Fill();
                    for (int i = 0; i < t.Rows.Count; ++i)
                        switch (t.Rows[i]["type_day_id"].ToString())
                        {
                            case "1":TableCalendar.Add((DateTime)t.Rows[i]["calendar_day"], Brushes.IndianRed); break;
                            case "3":TableCalendar.Add((DateTime)t.Rows[i]["calendar_day"], Brushes.Navy); break;
                            case "4":TableCalendar.Add((DateTime)t.Rows[i]["calendar_day"], Brushes.Firebrick); break;
                        }
                }
                catch { }
            }
        }

        private static void OnYearChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WPFCalendar wc = sender as WPFCalendar;
            DateTime cur_date;
            LibCalendarDay ld;
            SolidColorBrush b;
            for (int i = 0; i < 12; ++i)
            {
                DateTime t = new DateTime(((DateTime)e.NewValue).Year, i+1,1);
                int first_day = ((int)t.DayOfWeek+6)%7;

                WPFMonthCalendar cm = wc.CalGrid.Children[i] as WPFMonthCalendar;
                for (int j = 0; j < 42; ++j)
                    if (j < first_day)
                    {
                        cm.list_s[j] = static_days[t.AddMonths(-1).DaysInMonth() - (first_day-j)];
                    }
                    else
                        if (j < first_day + t.DaysInMonth())
                        {
                            cur_date = new DateTime(t.Year, t.Month, j - first_day + 1);
                            if (wc.calendar.TryGetValue(cur_date, out ld))
                            {
                                cm.list_s[j] = ld;
                            }
                            else
                            {
                                if (tablecalendar.TryGetValue(cur_date, out b))
                                {
                                    ld = new LibCalendarDay();
                                    ld.DayColor = b;
                                    ld.DayText = cur_date.Day.ToString();
                                    wc.calendar.Add(cur_date, ld);
                                    cm.list_s[j] = ld;
                                }
                                else
                                {
                                    cm.list_s[j] = static_usual_days[j - first_day];
                                }
                            }
                        }
                        else
                            cm.list_s[j] = static_days[j - first_day - t.DaysInMonth()];
            }
        }

        public void SetInterval(DateTime start, DateTime end, SolidColorBrush color)
        {
            LibCalendarDay c;
            for (DateTime t = start; t <= end; t = t.AddDays(1))
                if (calendar.TryGetValue(t, out c))
                {
                    c.DayBackground = color;
                }
                else
                {
                    c = new LibCalendarDay();
                    c.DayBackground = color;
                    c.DayColor = Brushes.Black;
                    c.DayText = t.Day.ToString();
                    calendar.Add(t, c);
                }
            if (start<=new DateTime(this.SelectedYear.Year, 12,31) && end>= this.SelectedYear)
                OnYearChanged(this, new DependencyPropertyChangedEventArgs(SelectedYearProperty, null, this.SelectedYear));
        }

        public void ClearIntevals()
        {
            List<DateTime> l = new List<DateTime>();
            foreach (DateTime t in calendar.Keys)
                calendar[t].DayBackground = Brushes.Transparent;
            OnYearChanged(this, new DependencyPropertyChangedEventArgs(SelectedYearProperty, null, this.SelectedYear));
        }
        
        public DateTime SelectedYear
        {
            get
            {
                return (DateTime)GetValue(SelectedYearProperty);
            }
            set
            {
                SetValue(SelectedYearProperty, new DateTime(((DateTime)value).Year,1,1));
            }
        }

        public static Dictionary<DateTime, SolidColorBrush> TableCalendar
        {
            get
            {
                return tablecalendar;
            }
            set
            {
                tablecalendar = value;
            }

        }

        public void SetBorderInterval(DateTime StartDate, DateTime EndDate)
        {
            LibCalendarDay c;
            if (border_day.Key!=DateTime.MinValue)
            {
                for (DateTime t=border_day.Key;t<=border_day.Value;t=t.AddDays(1))
                    if (calendar.TryGetValue(t, out c))
                    {
                        c.DayBorder = LibCalendarDay.DefaultDayBorder;
                        if (c.DayColor==LibCalendarDay.DefaultDayColor && 
                            c.DayBackground==LibCalendarDay.DefaultDayBackground)
                            calendar.Remove(t);
                    }
            }
            for (DateTime t=StartDate;t<=EndDate;t=t.AddDays(1))
                if (calendar.TryGetValue(t, out c))
                {
                    c.DayBorder = Brushes.Crimson;
                }
                else
                {
                    c = new LibCalendarDay();
                    c.DayBorder = Brushes.Crimson;
                    c.DayText = t.Day.ToString();
                    c.DayColor = Brushes.Black;
                    calendar.Add(t, c);
                }
            border_day = new KeyValuePair<DateTime, DateTime>(StartDate, EndDate);
            //OnYearChanged(this, new DependencyPropertyChangedEventArgs(SelectedYearProperty, null, this.SelectedYear));
        }

        public void ClearBorderInterval()
        {
            LibCalendarDay c;
            if (border_day.Key != DateTime.MinValue)
            {
                for (DateTime t = border_day.Key; t <= border_day.Value; t = t.AddDays(1))
                    if (calendar.TryGetValue(t, out c))
                    {
                        c.DayBorder = LibCalendarDay.DefaultDayBorder;
                        if (c.DayColor == LibCalendarDay.DefaultDayColor &&
                            c.DayBackground == (SolidColorBrush)LibCalendarDay.DefaultDayBackground)
                            calendar.Remove(t);
                    }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedYear = SelectedYear.AddYears(-1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.SelectedYear = SelectedYear.AddYears(1);
        }
    }
    public class IncMonthDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            DateTime t = (DateTime)value;
            return t.AddMonths((int)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public struct DayDataStruct
    {
        public DateTime daystart, dayend;
        public SolidColorBrush c;
    }
}
