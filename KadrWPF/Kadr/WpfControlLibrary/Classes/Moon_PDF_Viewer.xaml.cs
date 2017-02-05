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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for Moon_PDF_Viewer.xaml
    /// </summary>
    public partial class Moon_PDF_Viewer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string currentPage)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(currentPage));
            }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return this._currentPage; }
            set
            {
                if (value != this._currentPage)
                {
                    this._currentPage = value;
                    OnPropertyChanged("CurrentPage");
                }
            }
        }

        private int _totalPages = 1;
        public int TotalPages
        {
            get { return this._totalPages; }
            set
            {
                if (value != this._totalPages)
                {
                    this._totalPages = value;
                    OnPropertyChanged("TotalPages");
                }
            }
        }

        private static RoutedUICommand _firstPage, _previousPage, _nextPage, _lastPage, _zoomIn, _zoomOut, _unloadDocument;

        byte[] _bytes;

        public Moon_PDF_Viewer()
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Moon_PDF_Viewer_PropertyChanged);
            InitializeComponent();
            _bytes = null;
        }

        void Moon_PDF_Viewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                if (_currentPage != moonPdfPanel.GetCurrentPageNumber())
                    moonPdfPanel.GotoPage(_currentPage);
            }
        }

        static Moon_PDF_Viewer()
        {
            _firstPage = new RoutedUICommand("Первая страница", "FirstPage", typeof(Moon_PDF_Viewer));
            _previousPage = new RoutedUICommand("Предыдущая страница", "PreviousPage", typeof(Moon_PDF_Viewer));
            _nextPage = new RoutedUICommand("Следующая страница", "NextPage", typeof(Moon_PDF_Viewer));
            _lastPage = new RoutedUICommand("Последняя страница", "LastPage", typeof(Moon_PDF_Viewer));
            _zoomIn = new RoutedUICommand("Увеличить", "ZoomIn", typeof(Moon_PDF_Viewer));
            _zoomOut = new RoutedUICommand("Уменьшить", "ZoomOut", typeof(Moon_PDF_Viewer));
            _unloadDocument = new RoutedUICommand("Выгрузить документ", "UnloadDocument", typeof(Moon_PDF_Viewer));
        }

        public void Open(MoonPdfLib.MuPdf.IPdfSource source, string password = null )
        {
            moonPdfPanel.Open(source, password);
            CurrentPage = 1;
            TotalPages = moonPdfPanel.TotalPages;
        }

        public void Open(byte[] source, string password = null)
        {
            moonPdfPanel.Open(new MoonPdfLib.MuPdf.MemorySource(source), password);
            CurrentPage = 1;
            TotalPages = moonPdfPanel.TotalPages;
            _bytes = source;
        }

        public void OpenFile(string pdfFilename, string password = null)
        {
            moonPdfPanel.OpenFile(pdfFilename, password);
            _currentPage = 1;
            _totalPages = moonPdfPanel.TotalPages;        
        }

        public static RoutedUICommand FirstPage
        {
            get { return _firstPage; }
            set { _firstPage = value; }
        }

        public static RoutedUICommand PreviousPage
        {
            get { return _previousPage; }
            set { _previousPage = value; }
        }

        public static RoutedUICommand NextPage
        {
            get { return _nextPage; }
            set { _nextPage = value; }
        }

        public static RoutedUICommand LastPage
        {
            get { return _lastPage; }
            set { _lastPage = value; }
        }

        public static RoutedUICommand ZoomIn
        {
            get { return Moon_PDF_Viewer._zoomIn; }
            set { Moon_PDF_Viewer._zoomIn = value; }
        }

        public static RoutedUICommand ZoomOut
        {
            get { return Moon_PDF_Viewer._zoomOut; }
            set { Moon_PDF_Viewer._zoomOut = value; }
        }

        public static RoutedUICommand UnloadDocument
        {
            get { return Moon_PDF_Viewer._unloadDocument; }
            set { Moon_PDF_Viewer._unloadDocument = value; }
        }

        private void FirstPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (moonPdfPanel != null && moonPdfPanel.GetCurrentPageNumber() > 1)
            {
                e.CanExecute = true;
            }
        }

        private void ChangePage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter.ToString())
            { 
                case "First":
                    moonPdfPanel.GotoFirstPage();
                    break;
                case "Previous":
                    moonPdfPanel.GotoPreviousPage();
                    break;
                case "Next":
                    moonPdfPanel.GotoNextPage();
                    break;
                case "Last":
                    moonPdfPanel.GotoLastPage();
                    break;
                default:
                    break;
            }
            CurrentPage = moonPdfPanel.GetCurrentPageNumber();
        }

        private void LastPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (moonPdfPanel != null && moonPdfPanel.GetCurrentPageNumber() < moonPdfPanel.TotalPages)
            {
                e.CanExecute = true;
            }
        }

        private void ZoomIn_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "ZoomIn")
                moonPdfPanel.ZoomIn();
            else
                moonPdfPanel.ZoomOut();
        }

        private void UnloadDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_bytes != null)
                e.CanExecute = true;
        }

        private void UnloadDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string _tempFile = System.IO.Path.GetTempPath() + "_tempAppendixFile.pdf";
            if (LibraryKadr.Library.ByteArrayToFile(_tempFile, _bytes))
            {
                System.Diagnostics.Process.Start(_tempFile);
            }

        }
    }
}
