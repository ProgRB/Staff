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
    public partial class InputDataForm : Form
    {
        private InputDataForm()
        {
            InitializeComponent();
        }
        public static DialogResult ShowForm(ref DateTime selected_date, string dateFormat)
        {
            InputDataForm f = new InputDataForm();
            f.DialogResult = DialogResult.Cancel;
            f.date_pic.Value = selected_date;
            f.date_pic.Format = DateTimePickerFormat.Custom;
            f.date_pic.CustomFormat = (string.IsNullOrEmpty(dateFormat) ? "dd MMMM yyyy" : dateFormat);
            f.ShowDialog();
            selected_date = f.date_pic.Value;
            return f.DialogResult;
        }
        private void btOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
