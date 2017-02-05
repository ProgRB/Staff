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
    public partial class InputBox : Form
    {
        private bool fl = false;
        private InputBox(string Text, string Promt, string DefaultValue)
        {
            InitializeComponent();
            this.Text = Text;
            label1.Text = Promt;
            maskedTextBox1.Text = DefaultValue;            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Promt"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="Mask"> параметр может принимать значения маски для следующих: "00/00/0000"</param>
        private InputBox(string Text, string Promt, string DefaultValue,string Mask)
        {
            InitializeComponent();
            this.Text = Text;
            label1.Text = Promt;
            maskedTextBox1.Mask = Mask;
            if (Mask == "00/00/0000")
                maskedTextBox1.Validating += new CancelEventHandler(Library.ValidatingDate);
            maskedTextBox1.Text = DefaultValue;
        }
        public static string Show(string Text, string Promt, string DefaultValue)
        {
            InputBox f = new InputBox(Text, Promt, DefaultValue);
            f.ShowDialog();
            return f.maskedTextBox1.Text;
        }
        public static string Show(string Text, string Promt, string DefaultValue, string Mask)
        {
            InputBox f = new InputBox(Text, Promt, DefaultValue,Mask);
            f.ShowDialog();
            return f.maskedTextBox1.Text;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            maskedTextBox1.Text = "";
            this.Close();
        }
    }
}
