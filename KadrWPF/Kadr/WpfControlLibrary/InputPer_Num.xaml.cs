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
    /// Interaction logic for InputPer_Num.xaml
    /// </summary>
    public partial class InputPer_Num : Window
    {
        public string PerNum
        {
            get { return tbPER_NUM.Text.Trim(); }
        }
        public bool SignComb
        {
            get { return (bool)chSign_Comb.IsChecked; }
        }

        public InputPer_Num()
        {
            InitializeComponent();
        }

        private void btContinue_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
