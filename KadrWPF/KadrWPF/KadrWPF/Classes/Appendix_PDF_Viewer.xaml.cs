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
using System.Windows.Forms;
using MoonPdfLib.MuPdf;
using System.IO;

namespace WpfControlLibrary.Classes
{
    /// <summary>
    /// Interaction logic for Appendix_PDF_Viewer.xaml
    /// </summary>
    public partial class Appendix_PDF_Viewer : Window
    {
        byte[] _document;
        public byte[] Document
        {
            get { return _document; }
            set { _document = value; }
        }

        string _note_Document;
        public string Note_Document
        {
            get { return _note_Document; }
            set { _note_Document = value; }
        }

        bool _view_Only;
        public bool View_Only
        {
            get { return _view_Only; }
            set { _view_Only = value; }
        }

        public Appendix_PDF_Viewer(byte[] document, string note_Document, bool view_Only)
        {
            _document = document;
            _note_Document = note_Document;
            _view_Only = view_Only;
            InitializeComponent();
            if (_document != null)
            {
                //var source = new MemorySource((byte[])(_ocSelectDocument.Parameters["p_DOCUMENT"].Value as OracleBlob).Value);
                //moonPdfPanel.Open(source);
                moonPdfPanel.Open(_document);
            }
        }

        private void btSearchFileAppendix_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFile = new OpenFileDialog();
            _openFile.Filter = "PDF files (*.pdf)|*.pdf";
            _openFile.CheckFileExists = true;
            _openFile.CheckPathExists = true;
            _openFile.Multiselect = false;
            if (_openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (_openFile.FileName.Substring(_openFile.FileName.Length - 3, 3).ToUpper() == "PDF")
                {
                    Document = File.ReadAllBytes(_openFile.FileName);
                    var source = new MemorySource(Document);
                    moonPdfPanel.Open(source);
                }
                else
                {
                    System.Windows.MessageBox.Show("Неверный формат файла!\nПолное имя файла: \n" +
                        _openFile.FileName, "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void btSaveAppendix_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Note_Document))
            {
                System.Windows.MessageBox.Show("Не указано описание документа!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Document == null)
            {
                System.Windows.MessageBox.Show("Не указан документ!", "АСУ \"Кадры\"", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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
