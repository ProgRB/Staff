using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WpfControlLibrary
{
    public static class BitmapConvertion
    {
        [DllImport("gdi32.dll")]
        static extern int DeleteObject(IntPtr o);

        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(this System.Drawing.Bitmap source)
        {
            var hBitmap = source.GetHbitmap();
            var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return result;
        }
    }
}
