using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Staff;


namespace Kadr
{
    public partial class FR_EmpFilter : Form
    {
        /// Строка фильтра
        public StringBuilder str_filter = new StringBuilder();

        public FR_EmpFilter()
        {
            InitializeComponent();
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), 
                new LinkArgument(FR_Emp.subdivFR, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedItem = null;
            cbPos_Name.AddBindingSource(POSITION_seq.ColumnsName.POS_ID.ToString(), 
                new LinkArgument(FR_Emp.positionFR, POSITION_seq.ColumnsName.POS_NAME));
            cbPos_Name.SelectedItem = null;     
        }

        private void btFilter_Click(object sender, EventArgs e)
        {
            if (tbFR_Last_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{0} upper(em.{1}) like upper('{2}%') ",
                    str_filter.Length != 0 ? " and" : "", FR_EMP_seq.ColumnsName.FR_LAST_NAME,
                    tbFR_Last_Name.Text.Trim()));   
            }

            if (tbFR_First_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{0} upper(em.{1}) like upper('{2}%') ",
                     str_filter.Length != 0 ? " and" : "", FR_EMP_seq.ColumnsName.FR_FIRST_NAME,
                     tbFR_First_Name.Text.Trim()));   
            }

            if (tbFR_Middle_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{0} upper(em.{1}) like upper('{2}%') ",
                    str_filter.Length != 0 ? " and" : "", FR_EMP_seq.ColumnsName.FR_MIDDLE_NAME,
                    tbFR_Middle_Name.Text.Trim()));   
            }

            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{0} em.{1} = {2} ",
                    str_filter.Length != 0 ? " and" : "", FR_EMP_seq.ColumnsName.SUBDIV_ID,
                    cbSubdiv_Name.SelectedValue));                            
            }

            if (cbPos_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{0} em.{1} = {2} ",
                    str_filter.Length != 0 ? " and" : "", FR_EMP_seq.ColumnsName.POS_ID,
                    cbPos_Name.SelectedValue)); 
            }

            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для фильтра!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                FormMain.flagFilterFR_Emp = true;
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btNoFilter_Click(object sender, EventArgs e)
        {
            FormMain.flagFilterFR_Emp = false;
            this.DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
