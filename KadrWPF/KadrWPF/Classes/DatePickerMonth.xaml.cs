using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KadrWPF.Classes
{
    /// <summary>
    /// Логика взаимодействия для DatePickerMonth.xaml
    /// </summary>
    public partial class DatePickerMonth : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string selectedDate)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selectedDate));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return this._selectedDate; }
            set
            {
                if (value != this._selectedDate)
                {
                    this._selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }
        public DatePickerMonth()
        {
            this.PropertyChanged += DatePickerMonth_PropertyChanged;
            InitializeComponent();
        }

        private void DatePickerMonth_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {

            }
        }
    }
}
