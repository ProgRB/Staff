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


namespace Tabel
{
    public partial class Filter : Form
    {
        public BindingSource bsEmp, bsTransfer;
        public OracleDataTable dtEmp, dtTransfer;
        public string per_num, code_subdiv, subdiv_name;
        public int subdiv_id, month, year;
        public bool flagFilter;
        /// <summary>
        /// Конструктор формы фильтра сотрудников для табеля
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Filter()
        {
            InitializeComponent();
            /*subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("where nvl(PARENT_ID,0) = 0 and " + 
                "(exists(select null from user_role_privs where granted_role = 'TABLE_FULL_FILL') " + 
                "or subdiv_id in (select subdiv_id from {0}.access_subdiv where upper(user_name) = upper('{1}') " + 
                "and upper(app_name) = 'TABLE')) order by SUBDIV_NAME",
                DataSourceScheme.SchemeName, Connect.UserId.ToUpper()));
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));*/

            cbSubdiv_Name.DataSource = Kadr.FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"];
            cbSubdiv_Name.DisplayMember = "SUBDIV_NAME";
            cbSubdiv_Name.ValueMember = "SUBDIV_ID";

            cbSubdiv_Name.SelectedItem = null;
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            tbMonth.Text = DateTime.Now.Month.ToString();
            tbYear.Text = DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Выход из формы фильтра сотрудников для табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Фильтрация сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilter_Click(object sender, EventArgs e)
        {
            btFilter.Focus();
            if (cbSubdiv_Name.SelectedValue != null)
            {
                subdiv_id = Convert.ToInt32(cbSubdiv_Name.SelectedValue);
                code_subdiv = tbCode_Subdiv.Text;
                subdiv_name = cbSubdiv_Name.Text;
                month = Convert.ToInt32(tbMonth.Text);
                year = Convert.ToInt32(tbYear.Text);
                flagFilter = true;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Вы не выбрали подразделение!", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
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

        /// <summary>
        /// Изменение индеска списка подразделений
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
        /// Проверка правильности введенного месяца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMonth_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(tbMonth.Text) < 1 || Convert.ToInt32(tbMonth.Text) > 12)
                {
                    MessageBox.Show("Вы ввели некорректный месяц!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbMonth.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Вы ввели некорректный месяц!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbMonth.Focus();
                return;
            }
        }

        private void tbYear_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(tbYear.Text) < 2000 || Convert.ToInt32(tbYear.Text) > 2999)
                {
                    MessageBox.Show("Вы ввели некорректный год!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbYear.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Вы ввели некорректный год!", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbYear.Focus();
                return;
            }
        }
    }
}

