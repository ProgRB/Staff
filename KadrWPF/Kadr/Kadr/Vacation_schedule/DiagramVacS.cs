using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;
namespace Kadr.Vacation_schedule
{
    public partial class DiagramVacS : Form
    {
        public DiagramVacS()
        {
            InitializeComponent();
            elementHost1.Child = new WpfControlLibrary.VacSchedule.VacDiagramView();
        }
   

        private void DiagramVacS_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain f = ((FormMain)Application.OpenForms["FormMain"]);
            f.UpdateButtonsState_VS(this);
        }
    }
}
        