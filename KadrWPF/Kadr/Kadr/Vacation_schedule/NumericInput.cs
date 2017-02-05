using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kadr.Vacation_schedule
{
    public partial class NumericInput : Form
    {
        private NumericInput()
        {
            InitializeComponent();
        }
        public static DialogResult ShowForm(string Caption, string Promt, ref decimal DefaultValue, int DecimalPlaces)
        {
            NumericInput f = new NumericInput();
            f.Text = Caption;
            f.label_text.Text = Promt;
            f.numval.Value = DefaultValue;
            f.numval.DecimalPlaces = DecimalPlaces;
            DialogResult t = f.ShowDialog();
            DefaultValue = f.numval.Value;
            return t;
        }

    }
}
