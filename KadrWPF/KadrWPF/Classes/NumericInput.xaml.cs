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
using System.Windows.Shapes;

namespace Classes
{
    /// <summary>
    /// Interaction logic for NumericInput.xaml
    /// </summary>
    public partial class NumericInput : Window, INotifyPropertyChanged
    {
        private string _textPromt =  "Введите значение";
        private string _header = "Ввод значения";
        private decimal _value;

        public NumericInput()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static bool ShowPromt(Window sender, string header, string promt, ref decimal value, decimal dec_places = 0)
        {
            NumericInput f = new NumericInput();
            if (promt!=string.Empty)
                f.TextPromt = promt;
            f.FormHeader = header;
            f.Value = value;
            if (sender!=null)
                f.Owner = sender;
            value = f.Value;
            return f.ShowDialog() ?? false;
        }

        public string TextPromt
        {
            get
            {
                return _textPromt;
            }
            set
            {
                _textPromt = value;
                OnPropertyChanged("TextPromt");
            }
        }

        public string FormHeader
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged("FormHeader");
            }
        }

        /// <summary>
        /// Значение ввода данных
        /// </summary>
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;   
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
