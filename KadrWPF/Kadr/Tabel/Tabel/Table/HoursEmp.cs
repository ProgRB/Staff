using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Tabel
{
    public partial class HoursEmp : Form
    {
        public HoursEmp(OracleDataTable _dtHoursPeriod)
        {
            InitializeComponent();
            dgHours.DataSource = _dtHoursPeriod;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
