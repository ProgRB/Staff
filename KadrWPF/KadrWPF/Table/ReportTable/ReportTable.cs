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


namespace Tabel
{
    public partial class ReportTable : Form
    {
        private int _monthTable;
        public int MonthTable
        {
            get { return _monthTable; }
        }
        private int _yearTable;
        public int YearTable
        {
            get { return _yearTable; }
        }
        public ReportTable() : 
            this(true, true)
        {}

        bool _visibleMonth = false, _visibleYear = false;
        public ReportTable(bool visibleMonth, bool visibleYear)
        {
            InitializeComponent();
            _visibleMonth = visibleMonth;
            _visibleYear = visibleYear;
            if (!_visibleMonth)
            {
                lbMonth.Visible = false;
                mbMonth.Visible = false;
                lbYear.Location = lbMonth.Location;
                mbYear.Location = mbMonth.Location;
            }
            if (!_visibleYear)
            {
                lbYear.Visible = false;
                mbYear.Visible = false;
            }
        }
        private void btPreview_Click(object sender, EventArgs e)
        {
            btPreview.Focus();
            int month = 0;
            Int32.TryParse(mbMonth.Text.Trim(), out month);
            if ((month > 12 || month == 0) && _visibleMonth)
            {
                MessageBox.Show("Вы ввели некорректный месяц!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbMonth.Focus();
                return;
            }
            int year = 0;
            Int32.TryParse(mbYear.Text.Trim(), out year);
            if (year == 0 && _visibleYear)
            {
                MessageBox.Show("Вы ввели некорректный год!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbYear.Focus();
                return;
            }
            _monthTable = month;
            _yearTable = year;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
