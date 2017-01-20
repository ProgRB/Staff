using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARM_PROP
{
    public partial class FilterForPermit : Form
    {
        public string strFilter = "";
        public FilterForPermit()
        {
            InitializeComponent();
            tbCode_Subdiv.Text = tbEmp_Last_Name.Text = tbPer_num.Text = "";
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            btApply.Focus();
            strFilter += tbCode_Subdiv.Text != "" ?
                    " and S.CODE_SUBDIV = '" +
                    tbCode_Subdiv.Text.Trim() + "'":
                    "";
            strFilter += tbPer_num.Text != "" ?
                    " and E.PER_NUM = '" + tbPer_num.Text.Trim() + "'":
                    "";
            strFilter += tbEmp_Last_Name.Text != "" ?
                    " and upper(E.EMP_LAST_NAME) like upper('" +
                    tbEmp_Last_Name.Text.Trim() + "%')" :
                    "";
        }

        private void tbCode_Subdiv_Leave(object sender, EventArgs e)
        {
            tbCode_Subdiv.Text = tbCode_Subdiv.Text.Trim() != "" ? 
                tbCode_Subdiv.Text.Trim().PadLeft(3, '0') :
                "";
        }

        private void tbPer_num_Leave(object sender, EventArgs e)
        {
            tbPer_num.Text = tbPer_num.Text.Trim() != "" ?
                tbPer_num.Text.Trim().PadLeft(5, '0') :
                "";
        }
    }
}
