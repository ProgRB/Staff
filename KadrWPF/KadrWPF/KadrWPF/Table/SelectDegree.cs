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
    public partial class SelectDegree : Form
    {
        DEGREE_seq degree;
        public SelectDegree()
        {
            InitializeComponent();
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME));
            cbDegree_Name.AddBindingSource(DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            cbDegree_Name.SelectedItem = null;
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);
        }

        private void tbCode_Degree_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Degree, cbDegree_Name, 2, Connect.CurConnect, e, DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(), tbCode_Degree.Text);
        }

        /// <summary>
        /// Событие изменения индекса категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbDegree_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDegree_Name.SelectedValue != null)
            {
                tbCode_Degree.Text = Library.CodeBySelectedValue(Connect.CurConnect, DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(),
                    Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), cbDegree_Name.SelectedValue.ToString());
            }
        }

        private void btSelectDegree_Click(object sender, EventArgs e)
        {
            btSelectDegree.Focus();
            if (cbDegree_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали категорию!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbDegree_Name.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
