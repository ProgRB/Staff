using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kadr.Shtat
{
    public partial class SelectPeriod : Form
    {
        public string period_begin = "-1", period_end = "-1";
        bool b, ed;
        public SelectPeriod(bool CanBeginNull,bool CanEndNull):this(CanBeginNull,CanEndNull,null,null)
        {
        }
        public SelectPeriod(bool CanBeginNull, bool CanEndNull,string DefaultBegin,string DefaultEnd)
        {
            b = CanBeginNull;
            ed = CanEndNull;
            InitializeComponent();
            Period_Begin.Text = DefaultBegin;
            Period_End.Text = DefaultEnd;
        }

        private void Period_Begin_Validating(object sender, CancelEventArgs e)
        {
            char[] c = new char[10];
            c = "0123456789".ToCharArray();
            try
            {
                if (Period_Begin.Text.IndexOfAny(c) > -1) period_begin = Convert.ToDateTime(Period_Begin.Text).ToShortDateString();
                else
                    period_begin = "";
               
            }
            catch
            {
                MessageBox.Show(this, "Дата введена не верно", "АРМ Кадры");
                Period_Begin.Focus();
                period_begin = "-1";
                e.Cancel = true;
            }
        }

        private void Period_End_Validating(object sender, CancelEventArgs e)
        {
            char[] c = new char[10];
            c = "0123456789".ToCharArray();
            try
            {
                if (Period_End.Text.IndexOfAny(c) > -1) period_end = Convert.ToDateTime(Period_End.Text).ToShortDateString();
                else
                    period_end = "";
            }
            catch
            {
                MessageBox.Show(this, "Дата введена не верно", "АРМ Кадры");
                Period_End.Focus();
                period_end = "-1";
                e.Cancel = true;
            } 

        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (!b && !Period_Begin.MaskCompleted)
            {
                MessageBox.Show(this, "Дата начала не может быть пустой!", "АРМ Кадры");
                return;
            }
            if (!ed && (period_end == ""||period_end=="-1"))
            {
                MessageBox.Show(this, "Дата окончания не может быть пустой!", "АРМ Кадры");
                return;
            }
            
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            period_end = period_begin = "-1";
        }

        private void SelectPeriod_Load(object sender, EventArgs e)
        {

        }

    }
}
