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
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Oracle.DataAccess.Client;
using LibraryKadr;
using Oracle.DataAccess.Types;
using MoonPdfLib.MuPdf;
using Kadr;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Application_Editor.xaml
    /// </summary>
    public partial class Appendix_Editor : Window
    {
        OracleCommand _ocSelectDocument;
        public Appendix_Editor(DataRowView currentRow)
        {
            this.DataContext = currentRow;
            InitializeComponent();
            _ocSelectDocument = new OracleCommand(string.Format(
                "begin SELECT NVL((select DOCUMENT from {0}.PROJECT_APPENDIX where PROJECT_APPENDIX_ID = :p_PROJECT_APPENDIX_ID),null) into :p_DOCUMENT from dual; end;",
                Connect.Schema), Connect.CurConnect);
            _ocSelectDocument.BindByName = true;
            _ocSelectDocument.Parameters.Add("p_PROJECT_APPENDIX_ID", OracleDbType.Decimal).Value = currentRow["PROJECT_APPENDIX_ID"];
            _ocSelectDocument.Parameters.Add("p_DOCUMENT", OracleDbType.Blob).Direction = ParameterDirection.Output;
            _ocSelectDocument.ExecuteNonQuery();
            if (!(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).IsNull)
            {
                //var source = new MemorySource((byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value);
                //moonPdfPanel.Open(source);
                moonPdfPanel.Open((byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SaveProject_Appendix_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name) &&
                ((DataRowView)this.DataContext).DataView.Table.GetChanges() != null)
                e.CanExecute = true;
        }

        private void SaveProject_Appendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void SearchFileAppendix_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ControlAccess.GetState(((RoutedUICommand)e.Command).Name))
                e.CanExecute = true;
        }

        private void SearchFileAppendix_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog _openFile = new OpenFileDialog();
            _openFile.Filter = "PDF files (*.pdf)|*.pdf";
            _openFile.CheckFileExists = true;
            _openFile.CheckPathExists = true;
            _openFile.Multiselect = false;
            if (_openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //System.Windows.MessageBox.Show(_openFile.FileName);
                if (_openFile.FileName.Substring(_openFile.FileName.Length - 3, 3).ToUpper() == "PDF")
                {
                    ((DataRowView)this.DataContext)["DOCUMENT"] = File.ReadAllBytes(_openFile.FileName);
                    var source = new MemorySource((byte[])((DataRowView)this.DataContext)["DOCUMENT"]);
                    moonPdfPanel.Open(source);
                }
                else
                {
                    System.Windows.MessageBox.Show("Неверный формат файла!\nПолное имя файла: \n" +
                        _openFile.FileName, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
