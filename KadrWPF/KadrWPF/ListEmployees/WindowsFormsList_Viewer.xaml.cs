using Kadr;
using LibraryKadr;
using System;
using System.Windows.Controls;

namespace KadrWPF
{
    /// <summary>
    /// Interaction logic for WindowsFormsList_Viewer.xaml
    /// </summary>
    public partial class WindowsFormsList_Viewer : UserControl, IWindowsForms_Viewer
    {
        public bool FlagArchive
        {
            get { return ((ListEmp)windowsFormsHost.Child).FlagArchive; }
        }

        public WindowsFormsList_Viewer(System.Windows.Forms.Control form)
        {
            InitializeComponent();
            windowsFormsHost.Child = form;
            windowsFormsHostFilter.Child = new Filter_Emp(form);
            windowsFormsHostFind.Child = new Kadr.Find_Emp(form, ((ListEmp)form).dtEmp.SelectCommand.CommandText);
            windowsFormsHostSort.Child = new Sort_Emp(form);
        }

        public System.Windows.Forms.Control ChildForm
        {
            get { return windowsFormsHost.Child; }
        }

        public Type TypeChildForm
        {
            get { return windowsFormsHost.Child.GetType(); }
        }
    }
}
