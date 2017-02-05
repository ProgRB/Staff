using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Salary.Helpers;
using System.Windows.Controls;
using System.ComponentModel;
using LibrarySalary.Helpers;
using System.Windows;
using System.Windows.Input;

namespace LibrarySalary.ViewModel
{
    public class ViewTabCollection: NotificationObject
    {
        static ObservableCollection<ViewTabBase> _openTabs;
        public static ObservableCollection<ViewTabBase> OpenTabs
        {
            get
            {
                if (_openTabs == null)
                    _openTabs = new ObservableCollection<ViewTabBase>();
                return _openTabs;
            }
        }
        ViewTabBase _selectedTab;
        public ViewTabBase SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                _selectedTab = value;
                RaisePropertyChanged(() => SelectedTab);
                if (_selectedTab!=null)
                    _selectedTab.ContentData.Focus();
            }
        }
        public ViewTabBase AddNewTab(string tabheader, UserControl content)
        {
            ViewTabBase v = new ViewTabBase(tabheader, content);
            OpenTabs.Add(v);
            SelectedTab = v;
            return v;
        }

        public ViewTabBase AddNewTab(string tabheader, UserControl content, Uri uriIcon)
        {
            ViewTabBase v = new ViewTabBase(tabheader, content, uriIcon);
            OpenTabs.Add(v);
            SelectedTab = v;
            return v;
        }

        public void CloseTab(ViewTabBase v)
        { 
            CancelEventArgs e = new CancelEventArgs();
            if (v!=null) v.ValidateClose(e);
            if (!e.Cancel)
            {
                OpenTabs.Remove(v);
            }
        }
        /*
        private static RelayCommand _closeTabCommand;
        public static RelayCommand CloseTabCommand
        {
            get
            {
                if (_closeTabCommand == null)
                    _closeTabCommand = new RelayCommand((e)=>{CloseTab((ViewTabBase)e);});
                return _closeTabCommand;
            }
        }
        */
        /// <summary>
        /// Возвращает вкладку с указанным типом или налл
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ViewTabBase GetOpenTab(Type t)
        {
            return _openTabs.Where(r => r.ContentData.GetType() == t).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает вкладку с указанным типом или налл
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ViewTabBase[] GetOpenTabArray(Type t)
        {
            return _openTabs.Where(r => r.ContentData.GetType() == t).ToArray();
        }
        /// <summary>
        /// Возвращает вкладку с указанным типом или налл
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ViewTabBase GetOpenTab(string title)
        {
            return _openTabs.Where(r => r.HeaderText == title).FirstOrDefault();
        }
        /// <summary>
        /// Возвращает вкладку с указанным типом или налл
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ViewTabBase GetOpenTabForm(Type t)
        {
            return _openTabs.Where(r => r.ContentData is LibraryKadr.IWindowsForms_Viewer).Where(r => ((LibraryKadr.IWindowsForms_Viewer)r.ContentData).TypeChildForm == t).FirstOrDefault();
        }
    }
}
