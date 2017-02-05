using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryKadr
{
    public partial class SelectPeriod : Form
    {
        private DateTime _maxDate = DateTime.Now.AddYears(1000);
        private DateTime _minDate = DateTime.Now.AddYears(-1000);
        private DateTime _beginDate;
        private DateTime _endDate;

        public DateTime MaxDate
        {
            get { return _maxDate; }
            set { _maxDate = value; }
        }

        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value; }
        }

        public DateTime BeginDate
        {
            get { return _beginDate; }
            set 
            { 
                _beginDate = value;
                de1.Date = _beginDate;
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { 
                _endDate = value;
                de2.Date = _endDate;
            }
        }

        public SelectPeriod()
        {
            InitializeComponent();            
        }

        private void de1_Validating(object sender, CancelEventArgs e)
        {
            if (de1.Date != null)
            {
                if (!(de1.Date >= MinDate && de1.Date <= MaxDate))
                {
                    MessageBox.Show("Дата начала периода должна быть в промежутке: \n" +
                        MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    de1.Focus();
                    return;
                }
                BeginDate = (DateTime)de1.Date;
            }
        }

        private void de2_Validating(object sender, CancelEventArgs e)
        {
            if (de2.Date != null)
            {
                if (!(de2.Date >= MinDate && de2.Date <= MaxDate))
                {
                    MessageBox.Show("Дата окончания периода должна быть в промежутке: \n" +
                        MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    de2.Focus();
                    return;
                }
                EndDate = (DateTime)de2.Date.Value.AddDays(1).AddSeconds(-1);
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            btSelect.Focus();
            if (de1.Date == null)
            {
                MessageBox.Show("Дата начала периода должна быть в промежутке: \n" +
                    MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                de1.Focus();
                return;
            }
            if (de2.Date == null)
            {
                MessageBox.Show("Дата окончания периода должна быть в промежутке: \n" +
                    MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                de2.Focus();
                return;
            }
            Close();
            this.DialogResult = DialogResult.OK;
        }

        private void SelectPeriod_Activated(object sender, EventArgs e)
        {
            de1.Focus();
        }
    }
}
