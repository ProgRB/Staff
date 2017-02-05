using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace WpfControlLibrary.Emps_Access
{
    /// <summary>
    /// Interaction logic for Access_Templ_By_Emp_Editor.xaml
    /// </summary>
    public partial class Access_Templ_By_Emp_Editor : Window
    {
        static OracleCommand _ocPhotoEmp;
        public Access_Templ_By_Emp_Editor(DataRowView dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext; 
            
            cbSubdiv.ItemsSource = AppDataSet.Tables["SUBDIV"].DefaultView;
            
            cbPosition.ItemsSource = AppDataSet.Tables["POSITION"].DefaultView;

            dgAccess_Templ_By_Emp.DataContext = List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].DefaultView;

            //dcID_SHABLON_MAIN.ItemsSource = ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView;
            
            _ocPhotoEmp.Parameters["p_PER_NUM"].Value = dataContext["PER_NUM"];
            _ocPhotoEmp.ExecuteNonQuery();
            if (!(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).IsNull)
                imPhoto.Source = BitmapConvertion.ToBitmapSource(System.Drawing.Bitmap.FromStream(
                    new MemoryStream((byte[])(_ocPhotoEmp.Parameters["p_PHOTO"].Value as OracleBlob).Value)) as System.Drawing.Bitmap);
        }

        static Access_Templ_By_Emp_Editor()
        {
            _ocPhotoEmp = new OracleCommand(string.Format(
                "begin SELECT (select E.PHOTO from {0}.EMP E where PER_NUM = :p_PER_NUM) into :p_PHOTO from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocPhotoEmp.BindByName = true;
            _ocPhotoEmp.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2);
            _ocPhotoEmp.Parameters.Add("p_PHOTO", OracleDbType.Blob).Direction = ParameterDirection.Output;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddAccess_Templ_By_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void AddAccess_Templ_By_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataRowView _currentRow = List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].DefaultView.AddNew();
            _currentRow["PER_NUM"] = ((DataRowView)this.DataContext)["PER_NUM"];
            _currentRow["PERCO_SYNC_ID"] = ((DataRowView)this.DataContext)["PERCO_SYNC_ID"];
            List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].Rows.InsertAt(_currentRow.Row,0);
            dgAccess_Templ_By_Emp.SelectedItem = _currentRow;
        }

        private void DeleteAccess_Templ_By_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                dgAccess_Templ_By_Emp != null && dgAccess_Templ_By_Emp.SelectedCells.Count > 0)
                e.CanExecute = true;
        }

        private void DeleteAccess_Templ_By_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Удалить запись?", "АСУ \"Кадры\"", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                while (dgAccess_Templ_By_Emp.SelectedCells.Count > 0)
                {
                    ((DataRowView)dgAccess_Templ_By_Emp.SelectedCells[0].Item).Delete();
                }
                List_Emp_With_Template.SaveAccess_Templ_By_Emp();
            }
            dgAccess_Templ_By_Emp.Focus();
        }

        private void SaveAccess_Templ_By_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void SaveAccess_Templ_By_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List_Emp_With_Template.SaveAccess_Templ_By_Emp();
        }

        private void CancelAccess_Templ_By_Emp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].GetChanges() != null)
                e.CanExecute = true;
        }

        private void CancelAccess_Templ_By_Emp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List_Emp_With_Template.Ds.Tables["ACCESS_EMP"].RejectChanges();
        }

        private void dgAccess_Templ_By_Emp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
