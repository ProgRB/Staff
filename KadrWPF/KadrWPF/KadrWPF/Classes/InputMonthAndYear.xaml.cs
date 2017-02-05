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

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for InputMonthAndYear.xaml
    /// </summary>
    public partial class InputMonthAndYear : Window
    {
        private static bool _signInputMonth = true;
        public static bool SignInputMonth
        {
            get { return _signInputMonth; }
            set { _signInputMonth = value; }
        }

        private static int _numMonth = DateTime.Today.Month;
        public static int NumMonth
        {
            get { return _numMonth; }
            set { _numMonth = value; }
        }

        private static int _numYear = DateTime.Today.Year;
        public static int NumYear
        {
            get { return _numYear; }
            set { _numYear = value; }
        }

        public InputMonthAndYear(bool signInputMonth)
        {
            SignInputMonth = signInputMonth;
            InitializeComponent();
        }

        public InputMonthAndYear() : this(true) 
        { }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
