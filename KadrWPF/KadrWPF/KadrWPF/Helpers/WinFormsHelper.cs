using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace KadrWPF.Helpers
{
    public static class WinFormsHelper
    {
        public static void SetWpfOwner(this System.Windows.Forms.Form sender, Window parentWindow)
        {
            WindowInteropHelper helper = new WindowInteropHelper(parentWindow);
            SetWindowLong(new HandleRef(sender, sender.Handle), -8, helper.Handle.ToInt32());
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);
    }
}
