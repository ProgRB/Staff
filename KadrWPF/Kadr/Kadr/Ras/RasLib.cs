using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kadr.Ras
{
    class RasLib
    {
        public static void InitEvent(object sender)
        {
            FormMain f = (sender as FormMain);
            f.btView_All_Transfer.Click += new EventHandler(btAVGNoteRas_Click);
        }

        static void btAVGNoteRas_Click(object sender, EventArgs e)
        {
            AvgViewData f=null;
            foreach(Form p in  Application.OpenForms)
                if (p.GetType()==typeof(AvgViewData))
                {
                    f=(p as AvgViewData);
                    break;
                }
            if (f==null) f = new AvgViewData();
            f.MdiParent = Application.OpenForms["FormMain"];
            f.Show();
        }
    }
}
