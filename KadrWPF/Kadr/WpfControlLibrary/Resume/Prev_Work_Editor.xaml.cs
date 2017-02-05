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
    /// Interaction logic for Prev_Work_Editor.xaml
    /// </summary>
    public partial class Prev_Work_Editor : Window
    {
        DataSet _dsEdu;
        public Prev_Work_Editor(DataRowView currentPW, DataSet dsEdu)
        {
            _dsEdu = dsEdu;
            InitializeComponent();
            this.DataContext = currentPW;
        }

        private void SavePrev_Work_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dsEdu != null && _dsEdu.HasChanges() &&
                ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = Array.TrueForAll<DependencyObject>(grPerson_Data.Children.Cast<UIElement>().ToArray(), t => Validation.GetHasError(t) == false);
        }

        private void SavePrev_Work_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null && dpPW_DATE_START.SelectedDate != null && dpPW_DATE_END.SelectedDate != null)
            {
                Resume_Editor.ocStanding.Parameters["p_date_begin"].Value = ((DataRowView)this.DataContext)["PW_DATE_START"];
                Resume_Editor.ocStanding.Parameters["p_date_end"].Value = ((DataRowView)this.DataContext)["PW_DATE_END"];
                Resume_Editor.ocStanding.ExecuteNonQuery();
                ((DataRowView)this.DataContext)["STAGYEAR"] = Resume_Editor.ocStanding.Parameters["p_years"].Value;
                ((DataRowView)this.DataContext)["STAGMONTH"] = Resume_Editor.ocStanding.Parameters["p_months"].Value;
                ((DataRowView)this.DataContext)["STAGDAY"] = Resume_Editor.ocStanding.Parameters["p_days"].Value;
            }
        }
    }
}
