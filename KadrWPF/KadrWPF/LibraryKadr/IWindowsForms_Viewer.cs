using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryKadr
{
    public interface IWindowsForms_Viewer
    {
        System.Windows.Forms.Control ChildForm
        {
            get;
        }

        Type TypeChildForm
        {
            get;
        }

    }
}
