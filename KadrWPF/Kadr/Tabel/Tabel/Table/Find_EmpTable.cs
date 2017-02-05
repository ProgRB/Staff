using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using System.Reflection;
using LibraryKadr;
using Tabel;

namespace Tabel
{
    public partial class Find_EmpTable : Form
    {
        public StringBuilder str_find = new StringBuilder();
        public StringBuilder sort = new StringBuilder();
        SUBDIV_seq subdiv;
        TYPE_PER_DOC_seq type_per_doc;
        string textFilter;
        /// <summary>
        /// Конструктор формы поиска
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Find_EmpTable(string _textFilter)
        {
            InitializeComponent();
            textFilter = _textFilter;
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
                SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
            type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC.ToString()));
        }

        /// <summary>
        /// Событие нажатия кнопки поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFind_Click(object sender, EventArgs e)
        {
            btFind.Focus();
            if (tbPer_Num.Text.Trim() != "")
            {
                str_find.Append(string.Format("where per_num = '{0}'", tbPer_Num.Text.Trim().PadLeft(5, '0')));
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper({1}) like upper('{2}%')", str_find.Length != 0 ? " and" : " where ",
                    EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(), tbEmp_Last_Name.Text.Trim()));
            }
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper({1}) like upper('{2}%')", str_find.Length != 0 ? " and" : " where ",
                    EMP_seq.ColumnsName.EMP_FIRST_NAME.ToString(), tbEmp_First_Name.Text.Trim()));
            }
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper({1}) like upper('{2}%')", str_find.Length != 0 ? " and" : " where ",
                    EMP_seq.ColumnsName.EMP_MIDDLE_NAME.ToString(), tbEmp_Middle_Name.Text.Trim()));
            }
            if (chSign_Comb.Checked)
            {
                str_find.Append(string.Format("{0} SIGN_COMB = 1", str_find.Length != 0 ? " and" : " where "));
            }
            else
            {
                str_find.Append(string.Format("{0} SIGN_COMB = 0", str_find.Length != 0 ? " and" : " where "));
            }
            if (str_find.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                sort = new StringBuilder();
                if (tbPer_Num.Text.Trim() != "")
                {
                    sort.Append("per_num");
                }
                if (tbEmp_Last_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",emp_last_name");
                    }
                    else
                    {
                        sort.Append("emp_last_name");
                    }
                }
                if (tbEmp_First_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",emp_first_name");
                    }
                    else
                    {
                        sort.Append("emp_first_name");
                    }
                }
                if (tbEmp_Middle_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",emp_middle_name");
                    }
                    else
                    {
                        sort.Append("emp_middle_name");
                    }
                }
                if (sort.Length != 0)
                {
                    sort.Insert(0, " order by ");
                }
                else
                {
                    sort.Append("order by per_num, emp_last_name");
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }  
        }

        /// <summary>
        /// Выход из формы поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
