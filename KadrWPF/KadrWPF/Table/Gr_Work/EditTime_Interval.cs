using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;

namespace Tabel
{
    public partial class EditTime_Interval : Form
    {
        bool flagAdd;
        TIME_INTERVAL_seq time_interval;
        TYPE_INTERVAL_seq type_interval;
        public EditTime_Interval(bool _flagAdd, int _time_zone_id, 
            TIME_INTERVAL_seq _time_interval, 
            TYPE_INTERVAL_seq _type_interval, int pos)
        {
            InitializeComponent();
            flagAdd = _flagAdd;
            time_interval = _time_interval;
            type_interval = _type_interval;
            if (_flagAdd)
            {
                time_interval.AddNew();
                ((CurrencyManager)BindingContext[time_interval]).Position = 
                    ((CurrencyManager)BindingContext[time_interval]).Count;
                ((TIME_INTERVAL_obj)(((CurrencyManager)BindingContext[time_interval]).Current)).TIME_ZONE_ID = _time_zone_id;                
            }
            else
            {
                ((CurrencyManager)BindingContext[time_interval]).Position = pos;
            }
            mbTime_Begin.AddBindingSource(time_interval, TIME_INTERVAL_seq.ColumnsName.TIME_BEGIN);
            mbTime_End.AddBindingSource(time_interval, TIME_INTERVAL_seq.ColumnsName.TIME_END);
            cbType_Interval.AddBindingSource(time_interval, TYPE_INTERVAL_seq.ColumnsName.TYPE_INTERVAL_ID,
                new LinkArgument(type_interval, TYPE_INTERVAL_seq.ColumnsName.TYPE_INTERVAL_NAME));
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (mbTime_Begin.Text.Trim() == ":")
            {
                MessageBox.Show("Вы не ввели время начала интервала!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbTime_Begin.Focus();
                return;
            }
            if (mbTime_End.Text.Trim() == ":")
            {
                MessageBox.Show("Вы не ввели время окончания интервала!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbTime_End.Focus();
                return;
            }
            if (cbType_Interval.SelectedValue == null || cbType_Interval.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели тип интервала!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbType_Interval.Focus();
                return;
            }
            if (((TIME_INTERVAL_obj)(((CurrencyManager)BindingContext[time_interval]).Current)).TYPE_INTERVAL_ID == 1)
            {
                DateTime dateStart = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + mbTime_Begin.Text.Trim());
            }
            time_interval.Save();
            Connect.Commit();
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            time_interval.ResetBindings();
            Close();
        }

        private void mbTime_Begin_Validating(object sender, CancelEventArgs e)
        {
            mbTime_Begin.Text = mbTime_Begin.Text.Replace(" ","").PadRight(5, '0');
            if (mbTime_Begin.Text.Trim() != ":")
            {
                try
                {
                    DateTime.Parse(DateTime.Now.ToShortDateString() + " " + mbTime_Begin.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Вы ввели неверное время!\nПовторите ввод.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }

        private void mbTime_End_Validating(object sender, CancelEventArgs e)
        {
            mbTime_End.Text = mbTime_End.Text.Replace(" ", "").PadRight(5, '0');
            if (mbTime_End.Text.Trim() != ":")
            {
                try
                {
                    DateTime.Parse(DateTime.Now.ToShortDateString() + " " + mbTime_End.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Вы ввели неверное время!\nПовторите ввод.", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }
    }
}
