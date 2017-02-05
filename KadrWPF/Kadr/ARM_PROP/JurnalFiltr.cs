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
    public partial class JurnalFiltr : Form
    {
        public StringBuilder str_filter = new StringBuilder();
        string fieldFilter;
        public static bool flagArchive;
        SUBDIV_seq subdiv;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq type_violation;

        public JurnalFiltr(string _fieldFilter)
        {
            InitializeComponent();
            fieldFilter = _fieldFilter;
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
            SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();

            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbSubdiv_Name.SelectedItem = null;
            cbPr_nar.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_VIOLATION_ID, new LinkArgument(type_violation, TYPE_VIOLATION_seq.ColumnsName.TYPE_VIOLATION_NAME)); 
            foreach (Control control in this.gbFilter.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }   
        }

        /// <summary>
        /// Событие нажатия кнопки формирования строки фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Ok_Click(object sender, EventArgs e)
        {        
            /// Фильтрация по табельному номеру
            if (tbPer_Num.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{2} = emp_list.{2} " +
                    " and {0}.{1}.{2} = '{3}')",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.PER_NUM.ToString(),
                    tbPer_Num.Text.Trim().PadLeft(5, '0'),
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по фамилии
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = emp_list.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Staff.DataSourceScheme.SchemeName, EMP_seq.ColumnsName.EMP_LAST_NAME, tbEmp_Last_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по имени
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = emp_list.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Staff.DataSourceScheme.SchemeName, EMP_seq.ColumnsName.EMP_FIRST_NAME, tbEmp_First_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по отчеству
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = emp_list.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Staff.DataSourceScheme.SchemeName, EMP_seq.ColumnsName.EMP_MIDDLE_NAME, tbEmp_Middle_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация для даты задержания
            if (mtbZader.Text.Replace(".", "").Trim() != "" && mtbZaderOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.violation_log_id = emp_list.violation_log_id and " +
                    "{0}.{1}.{2} between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                    Staff.DataSourceScheme.SchemeName, "violation_log", VIOLATION_LOG_seq.ColumnsName.ARREST_DATE.ToString(),
                    mtbZader.Text, mtbZaderOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mtbZader.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.violation_log_id = emp_list.violation_log_id and " +
                    "{0}.{1}.{2} >= to_date('{3}','dd.MM.yyyy') )",
                    Staff.DataSourceScheme.SchemeName, "violation_log", VIOLATION_LOG_seq.ColumnsName.ARREST_DATE.ToString(),
                    mtbZader.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mtbZaderOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.violation_log_id = emp_list.violation_log_id and " +
                    "{0}.{1}.{2} <= to_date('{3}','dd.MM.yyyy') )",
                    Staff.DataSourceScheme.SchemeName, "violation_log", VIOLATION_LOG_seq.ColumnsName.ARREST_DATE.ToString(),
                    mtbZaderOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация по признаку кражи
            if (chbprizKrag.Checked == true)
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.violation_log vl3 where vl3.violation_log_id = emp_list.violation_log_id " +
                    " and vl3.{1} = {2})",
                    Staff.DataSourceScheme.SchemeName, VIOLATION_LOG_seq.ColumnsName.SIGN_STEAL, 
                    1,
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация по признаку уголовного дела
            if (checkBox2.Checked == true)
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.violation_log vl3 where vl3.violation_log_id = emp_list.violation_log_id " +
                    " and vl3.{1} = {2})",
                    Staff.DataSourceScheme.SchemeName, VIOLATION_LOG_seq.ColumnsName.SIGN_CRIMINAL,
                    1,
                    str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по признаку групового нарушения
            if (checkBox1.Checked == true)
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.violation_log vl3 where vl3.violation_log_id = emp_list.violation_log_id " +
                    " and vl3.{1} = {2})",
                    Staff.DataSourceScheme.SchemeName, VIOLATION_LOG_seq.ColumnsName.SIGN_GROUP,
                    1,
                    str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по подразделению
            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} tr1 where tr1.{5} = emp_list.{5} and " +
                    "tr1.{2} = '{3}'and tr1.SIGN_CUR_WORK = 1)",
                    Staff.DataSourceScheme.SchemeName, "transfer",
                    TRANSFER_seq.ColumnsName.SUBDIV_ID,
                    cbSubdiv_Name.SelectedValue, str_filter.Length != 0 ? " and" : "", TRANSFER_seq.ColumnsName.TRANSFER_ID));
            }

            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для фильтра!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //FormMenu.flagFilter = true;
                this.DialogResult = DialogResult.OK;
                Close();
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

             private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
             {

             }

             private void gbFilter_Enter(object sender, EventArgs e)
             {

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

         /// <summary>
         /// Событие изменения индекса подразделения
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param> 
         private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (cbSubdiv_Name.SelectedValue != null)
             {
                 tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                     Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
             }
         }

         /// <summary>
         /// Проверка введенного шифра подразделения и изменение позиции комбобокса
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void tbCoden_Subdiv_Validating(object sender, CancelEventArgs e)
         {
             //Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, connection, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
             //    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
         }

         /// <summary>
         /// Событие нажатия кнопки отмены фильтра
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void bt_Cancel_Click(object sender, EventArgs e)
         {
             //FormMenu.flagFilter = false;
             this.DialogResult = DialogResult.Abort;
             Close();
         }
    }
}
