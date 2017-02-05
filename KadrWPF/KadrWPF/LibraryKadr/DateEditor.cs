using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryKadr
{
    public partial class DateEditor : UserControl
    {
        public event EventHandler TextChanged;
        //private string fText;
        //public string Text
        //{
        //    get
        //    {
        //        return fText;
        //    }
        //    set
        //    {
        //        maskedTextBox1.Text = value;
        //        this.Text = value;
        //    }
        //}

        private string fText;
        public string TextDate
        {
            get
            {
                if (fText == "")
                    return null;
                else
                    return fText;
            }
            set
            {
                maskedTextBox1.Text = value;
                fText = value;
            }
        }

        public DateTime? Date
        {
            get 
            {
                try
                {
                    return DateTime.Parse(this.Text);
                }
                catch
                {
                    return null;
                }
            }
            set 
            {
                maskedTextBox1.Text = value.ToString();
                //this.Text = value.ToString();
            }
        }

        public bool ReadOnly
        {
            get { return maskedTextBox1.ReadOnly; }
            set { maskedTextBox1.ReadOnly = value; }
        }

        public DateEditor()
        {
            InitializeComponent();
            maskedTextBox1.TextChanged += new EventHandler(maskedTextBox1_TextChanged);
            //maskedTextBox1.BackColor = this.BackColor;
            //this.Validating += new CancelEventHandler(DateEditor_Validating);
        }

        //void DateEditor_Validating(object sender, CancelEventArgs e)
        //{
        //    maskedTextBox1_Validating(sender, e);
        //}             

        void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            //fText = maskedTextBox1.Text;
            this.Text = maskedTextBox1.Text;
            fText = maskedTextBox1.Text;
            this.TextDate = maskedTextBox1.Text;
            if (this.TextChanged != null)
            {
                this.TextChanged(sender, e);
            }
        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {
            if (maskedTextBox1.Text.Replace(".", " ").Trim() == "")
            {
                this.Text = "";
                this.TextDate = "";
                this.Date = null;
            }
            else
            {
                this.TextDate = maskedTextBox1.Text;
                try
                {
                    this.Date = DateTime.Parse(maskedTextBox1.Text);
                }
                catch
                {
                    this.Date = null;
                }
            }
        }

        private void DateEditor_EnabledChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Enabled = this.Enabled;
        }

        private void DateEditor_BackColorChanged(object sender, EventArgs e)
        {
            if (this.BackColor != Color.Transparent)
            {
                maskedTextBox1.BackColor = this.BackColor;
            }
        }


                
    }
}
