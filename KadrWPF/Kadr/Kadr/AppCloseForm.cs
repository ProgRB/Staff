using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryKadr;

namespace Kadr
{
    public partial class AppCloseForm : Form
    {
        private Timer t;
        private int _remainTime = 30;
        public int RemainTime
        {
            get { return _remainTime; }
            set { _remainTime = value; }
        }
        public AppCloseForm( DateTime TimeCloseApp)
        {
            InitializeComponent();
            label_time_block.Text = TimeCloseApp.ToString();
            this.FormClosed+=new FormClosedEventHandler(AppCloseForm_FormClosed);
            t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            --_remainTime;
            label_remain_time.Text = _remainTime.ToString();
            if (_remainTime<1)
                this.Close();
        }

        void  AppCloseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Connect.CurConnect != null)
                Connect.CurConnect.Close();
            Application.Exit();
        }

        private void btCloseNow_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
