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

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for WorkPlaceForm.xaml
    /// </summary>
    public partial class GrWorkForm : Window
    {

        public GrWorkForm()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = grWorkViewer.Model.CurrentGrWork != null;
        }

        public GrWorkViewModel Model
        {
            get
            {
                return grWorkViewer?.Model;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.IsActive) // почему-то заходит сюда 2 раза код. не пойму
            {
                this.DialogResult = true;
                Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        /// <summary>
        /// Выбранный адишник карты рабочего места
        /// </summary>
        public decimal? SelectedGrWorkID
        {
            get
            {
                return grWorkViewer.Model.CurrentGrWorkID;
            }
        }
    }
}
