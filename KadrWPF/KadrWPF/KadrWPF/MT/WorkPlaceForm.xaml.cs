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
    public partial class WorkPlaceForm : Window
    {
        public WorkPlaceForm()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = workPlaceViewer.Model.CurrentWorkPlace != null;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.IsActive) // почему-то заходит сюда 2 раза код. не пойму
            {
               // System.Diagnostics.Debug.WriteLine("Вызвали закрытие!");
                DialogResult = true;
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
        public decimal? SelectedWorkPlaceID
        {
            get
            {
                return workPlaceViewer.Model.CurrentWorkPlaceID;
            }
        }
    }
}
