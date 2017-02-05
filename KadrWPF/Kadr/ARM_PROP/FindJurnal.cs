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
    public partial class FindJurnal : Form
    {
        SUBDIV_seq subdiv;
        //FormMenu parentForm;
        public StringBuilder str_find = new StringBuilder();
        public StringBuilder sort = new StringBuilder();
        string textFilter;
        ZurnNaruRe jurnal;

        /// <summary>
        /// Конструктор формы поиска
        /// </summary>
        /// <param name="_connection">Строка подключения</param>        
        public FindJurnal(string _textFilter)
        {
            InitializeComponent();
            textFilter = _textFilter;
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
            SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbSubdiv_Name.SelectedItem = null;
        }
        OracleDataTable oracleTable;

        public OracleDataTable OracleDataTable
        {
            get
            {
                return oracleTable;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFind_Click(object sender, EventArgs e)
        {
            if (tbPer_Num.Text.Trim() != "")
            {
                str_find.Append(string.Format("per_num = '{0}'", tbPer_Num.Text.Trim().PadLeft(5, '0')));
            }

            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} tr1 where tr1.{5} = emp_list.{5} and " +
                    "tr1.{2} = '{3}'and tr1.SIGN_CUR_WORK = 1)",
                    Staff.DataSourceScheme.SchemeName, "transfer",
                    TRANSFER_seq.ColumnsName.SUBDIV_ID,
                    cbSubdiv_Name.SelectedValue, str_find.Length != 0 ? " and" : "", TRANSFER_seq.ColumnsName.TRANSFER_ID));
            }

            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper (last_name) like upper('{1}%')", str_find.Length != 0 ? " and" : "",
                tbEmp_Last_Name.Text.Trim()));//EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(),
                
            }
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper (first_name) like upper('{1}%')", str_find.Length != 0 ? " and" : "",
                    tbEmp_First_Name.Text.Trim()));//EMP_seq.ColumnsName.EMP_FIRST_NAME.ToString(), 
            }
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper (middle_name) like upper('{1}%')", str_find.Length != 0 ? " and" : "",
                    tbEmp_Middle_Name.Text.Trim()));//EMP_seq.ColumnsName.EMP_MIDDLE_NAME.ToString(),
            }
            if (str_find.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //str_find.Insert(0, " where ");
                sort = new StringBuilder();
                if (tbCode_Subdiv.Text.Trim() != "")
                {
                    sort.Append("subdiv_name");
                    if (tbPer_Num.Text.Trim() == "")
                    {
                        sort.Append(",per_num");
                    }
                }
                if (tbPer_Num.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",per_num");
                    }
                    else
                    {
                        sort.Append("per_num");
                    }
                }
                if (tbEmp_Last_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",last_name");
                    }
                    else
                    {
                        sort.Append("last_name");
                    }
                }
                if (tbEmp_First_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",first_name");
                    }
                    else
                    {
                        sort.Append("first_name");
                    }
                }
                if (tbEmp_Middle_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",middle_name");
                    }
                    else
                    {
                        sort.Append("middle_name");
                    }
                }
                if (sort.Length != 0)
                {
                    sort.Insert(0, " order by ");
                }
                else
                {
                    sort.Append("order by subdiv_name, per_num");
                }
                string sql = string.Format(Queries.GetQuery("QueryFind.sql"), Staff.DataSourceScheme.SchemeName, "emp", "transfer",
                    EMP_seq.ColumnsName.PER_NUM, TRANSFER_seq.ColumnsName.TRANSFER_ID, EMP_seq.ColumnsName.EMP_LAST_NAME,
                    EMP_seq.ColumnsName.EMP_FIRST_NAME, EMP_seq.ColumnsName.EMP_MIDDLE_NAME,
                    SUBDIV_seq.ColumnsName.CODE_SUBDIV, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID);
                oracleTable = new OracleDataTable(sql, Connect.CurConnect);
                oracleTable.Fill();
                if (oracleTable.Rows.Count == 0)
                {
                    MessageBox.Show("В базе данных не найдена введенная информация!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    str_find.Remove(0, str_find.Length);
                    return;
                }
                textFilter = textFilter.Replace("  ", " ");
                int pos = textFilter.IndexOf(" order by ");
                int pos2 = textFilter.IndexOf("emp_link where");
                string str;
                if (pos2 != -1)
                    str = textFilter.Substring(0, pos) + " and " + str_find + sort;
                else
                    str = textFilter.Substring(0, pos) + " where " + str_find + sort;
                oracleTable.Clear();
                oracleTable.SelectCommand.CommandText = str;
                oracleTable.Fill();
                this.DialogResult = DialogResult.OK;
                Close();
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
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }
    }
}
