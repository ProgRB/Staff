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

namespace ARM_PROP
{
    public partial class PrintJurnal : Form
    {
        SUBDIV_seq subdiv;
        LIST_VIOLATOR_seq list_violator;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq types_violation;
        //TYPE_EXACT_seq types_exact;
        //public BindingSource bsEmp, bsTransfer;
        //public OracleDataTable dtEmp, dtTransfer;

        public PrintJurnal()
        {
            InitializeComponent();
            types_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            types_violation.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            list_violator = new LIST_VIOLATOR_seq(Connect.CurConnect);
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
            SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));

            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbSubdiv_Name.SelectedItem = null;
            cbPriznak.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_VIOLATION_ID, new LinkArgument(types_violation, TYPE_VIOLATION_seq.ColumnsName.TYPE_VIOLATION_NAME));
            cbPriznak.SelectedItem = null;
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }
    }
}
