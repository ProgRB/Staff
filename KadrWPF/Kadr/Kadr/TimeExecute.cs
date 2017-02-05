using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kadr
{
    public partial class TimeExecute : Form
    {
        public BackgroundWorker backWorker;
        DateTime timeExecute = new DateTime();
        System.Threading.Timer tmExecute;
        public delegate void InvokeDelegate();
        public TimeExecute(System.Windows.Forms.ProgressBarStyle progressBarStyle, bool labelPercentVisible)
        {
            InitializeComponent();
            System.Threading.TimerCallback timeCB = new System.Threading.TimerCallback(PrintTime);
            tmExecute = new System.Threading.Timer(timeCB, null, 1000, 1000);
            pbPercentExecute.Style = progressBarStyle;
            lbPercentExecute.Visible = labelPercentVisible;
            backWorker = new BackgroundWorker();
            backWorker.WorkerReportsProgress = true;
            backWorker.WorkerSupportsCancellation = true;
            backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backWorker_RunWorkerCompleted);
            backWorker.ProgressChanged += new ProgressChangedEventHandler(backWorker_ProgressChanged); 
        }

        public TimeExecute()
            : this(System.Windows.Forms.ProgressBarStyle.Blocks, true)
        { }

        void PrintTime(object state)
        {
            this.BeginInvoke(new InvokeDelegate(PrintTime2));
        }

        void PrintTime2()
        {
            timeExecute = timeExecute.AddSeconds(1);
            lbTimeExecute.Text = "Продолжительность работы - " + timeExecute.ToLongTimeString();
        }

        private void TimeExecute_FormClosed(object sender, FormClosedEventArgs e)
        {
            backWorker.CancelAsync();
            tmExecute.Dispose();
        }

        void backWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 101)
            {
                this.pbPercentExecute.Value = e.ProgressPercentage;
                this.lbPercentExecute.Text = e.ProgressPercentage.ToString() + "%";
            }
        }

        void backWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
        }
    }
}
