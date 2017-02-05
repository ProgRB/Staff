using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;

namespace ARM_PROP
{
    public partial class FiltrJurn : Form
    {
        SUBDIV_seq subdiv;
        public BindingSource bsEmp, bsTransfer;
        public OracleDataTable dtEmp, dtTransfer;
        public string per_num;
        public StringBuilder str_filter = new StringBuilder();
        public StringBuilder sort = new StringBuilder();

        public FiltrJurn()
        {
            InitializeComponent();
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
            SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));            

            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbSubdiv_Name.SelectedItem = null;

            foreach (Control control in this.gbFilter.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }   
        }
       
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        private void bt_Ok_Click(object sender, EventArgs e)
        {
            if (tbPer_Num.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{2} = emp_link.{2} " +
                    " and {0}.{1}.{2} = '{3}')",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.PER_NUM.ToString(),
                    tbPer_Num.Text.Trim().PadLeft(5, '0'),
                    str_filter.Length != 0 ? " and" : ""));
            }

            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = emp_link.{5} and " +
                    "{0}.{1}.per_num = emp_link.per_num and {0}.{1}.{2} = '{3}')",
                    Staff.DataSourceScheme.SchemeName, "transfer",
                    TRANSFER_seq.ColumnsName.SUBDIV_ID,
                    cbSubdiv_Name.SelectedValue, str_filter.Length != 0 ? " and" : "", TRANSFER_seq.ColumnsName.TRANSFER_ID));
            }

            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для фильтра!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //FormMenu.flagFilter = true;
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                this.MdiChildren[0].Close();
            }
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
        public class FieldFilter
        {
            private string tableName;
            private string fieldName;
            private string controlValue;

            public string TableName
            {
                get
                {
                    return tableName;
                }
                set
                {
                    tableName = value;
                }
            }

            public string FieldName
            {
                get
                {
                    return fieldName;
                }
                set
                {
                    fieldName = value;
                }
            }

            public string ControlValue
            {
                get
                {
                    return controlValue;
                }
                set
                {
                    controlValue = value;
                }
            }

            public FieldFilter(string _tableName, string _fieldName, string _controlValue)
            {
                tableName = _tableName;
                fieldName = _fieldName;
                controlValue = _controlValue;
            }
        }

    }
}
