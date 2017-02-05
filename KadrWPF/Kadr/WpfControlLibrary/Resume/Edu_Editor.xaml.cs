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
using Kadr;
using System.Data;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Edu_Editor.xaml
    /// </summary>
    public partial class Edu_Editor : Window
    {
        DataSet _dsEdu;
        public Edu_Editor(DataRowView currentEdu, DataSet dsEdu)
        {
            this.DataContext = currentEdu;
            _dsEdu = dsEdu;
            InitializeComponent();

            cbGR_SPEC_ID.ItemsSource = AppDataSet.Tables["GROUP_SPEC"].DefaultView;
            cbINSTIT_ID.ItemsSource = AppDataSet.Tables["INSTIT"].DefaultView;
            cbQUAL_ID.ItemsSource = AppDataSet.Tables["QUAL"].DefaultView;
            cbSPEC_ID.ItemsSource = AppDataSet.Tables["SPECIALITY"].DefaultView;
            cbTYPE_EDU_ID.ItemsSource = AppDataSet.Tables["TYPE_EDU"].DefaultView;
            cbTYPE_STUDY_ID.ItemsSource = AppDataSet.Tables["TYPE_STUDY"].DefaultView;
        }

        private void SaveEdu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dsEdu != null && _dsEdu.HasChanges() &&
                ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void SaveEdu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
