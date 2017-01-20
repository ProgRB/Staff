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
using Oracle.DataAccess.Client;


namespace Kadr
{
    public partial class AddFindEmp : Form
    {
        OracleDataTable dtEmp;
        SUBDIV_seq subdiv;
        BindingSource bsEmp, bsEmpResume;
        StringBuilder str_find;
        static OracleDataAdapter _daResume = new OracleDataAdapter();
        static DataSet _dsEmpResume = new DataSet();
        /// <summary>
        /// Конструктор формы поиска бывшего работника в базе данных
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_parentForm">Родительская форма</param>
        public AddFindEmp()
        {
            InitializeComponent();
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("order by {0}", SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedItem = null;
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
        }

        static AddFindEmp()
        {
            _dsEmpResume.Tables.Add("RESUME");
            _daResume.SelectCommand = new OracleCommand(string.Format(Queries.GetQuery("Resume/SelectResume.sql"),
                Connect.Schema), Connect.CurConnect);
            _daResume.SelectCommand.BindByName = true;
            // Параметры выбора резюме - либо доступные, либо уже перешедшие в архив
            _daResume.SelectCommand.Parameters.Add("p_begin_period", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_end_period", OracleDbType.Date);
            // Параметры фильтрации
            _daResume.SelectCommand.Parameters.Add("p_EMP_LAST_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_FIRST_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_MIDDLE_NAME", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_EMP_SEX", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_BEGIN_BIRTH_DATE", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_END_BIRTH_DATE", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_SOURCE_EMPLOYABILITY_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_BEGIN_FILING", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_END_FILING", OracleDbType.Date);
            _daResume.SelectCommand.Parameters.Add("p_PW_NAME_POS", OracleDbType.Varchar2);
            _daResume.SelectCommand.Parameters.Add("p_INSTIT_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_SPEC_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_QUAL_ID", OracleDbType.Decimal);
            _daResume.SelectCommand.Parameters.Add("p_TYPE_EDU_ID", OracleDbType.Decimal);
        }

        /// <summary>
        /// Событие нажатия кнопки поиска работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFindEmp_Click(object sender, EventArgs e)
        {
            /// Строка условий
            str_find = new StringBuilder();
            if (mbBirth_Date.Text.Replace(".", "").Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} " +
                    " and {0}.{1}.{2} = to_date('{3}','dd.MM.yyyy'))",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_BIRTH_DATE.ToString(),
                    mbBirth_Date.Text, str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
                _daResume.SelectCommand.Parameters["p_BEGIN_BIRTH_DATE"].Value = Convert.ToDateTime(mbBirth_Date.Text);
                _daResume.SelectCommand.Parameters["p_END_BIRTH_DATE"].Value = Convert.ToDateTime(mbBirth_Date.Text).AddDays(1).AddSeconds(-1);
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} " +
                    " and upper({0}.{1}.{2}) = upper('{3}'))",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(),
                    tbEmp_Last_Name.Text.Trim(), str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
                _daResume.SelectCommand.Parameters["p_EMP_LAST_NAME"].Value = tbEmp_Last_Name.Text.Trim();
            } 
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} " +
                    " and upper({0}.{1}.{2}) = upper('{3}'))",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_FIRST_NAME.ToString(),
                    tbEmp_First_Name.Text.Trim(), str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
                _daResume.SelectCommand.Parameters["p_EMP_FIRST_NAME"].Value = tbEmp_First_Name.Text.Trim();
            }
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} " +
                    " and upper({0}.{1}.{2}) = upper('{3}'))",
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_MIDDLE_NAME.ToString(),
                    tbEmp_Middle_Name.Text.Trim(), str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
                _daResume.SelectCommand.Parameters["p_EMP_MIDDLE_NAME"].Value = tbEmp_Middle_Name.Text.Trim();
            }
            if (mbInn.Text.Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} and " +
                    "{0}.{1}.{2} = '{3}') ",
                    Staff.DataSourceScheme.SchemeName, "per_data", PER_DATA_seq.ColumnsName.INN.ToString(),
                    mbInn.Text.Trim(), str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
            }
            if (mbInsurance_Num.Text.Trim() != "")
            {
                str_find.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{5} = CUR_EMP.{5} and " +
                    "{0}.{1}.{2} = '{3}') ",
                    Staff.DataSourceScheme.SchemeName, "per_data", PER_DATA_seq.ColumnsName.INSURANCE_NUM.ToString(),
                    mbInsurance_Num.Text.Trim(), str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.PER_NUM.ToString()));
            }
            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_find.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{6} = CUR_EMP.{6} and " +
                    "{0}.{1}.{4} = CUR_EMP.{4} and {0}.{1}.{2} = '{3}')",
                    Staff.DataSourceScheme.SchemeName, "transfer", TRANSFER_seq.ColumnsName.SUBDIV_ID.ToString(),
                    cbSubdiv_Name.SelectedValue, EMP_seq.ColumnsName.PER_NUM.ToString(),
                    str_find.Length != 0 ? " and" : "", TRANSFER_seq.ColumnsName.TRANSFER_ID.ToString()));
            }
            /// Строка запроса
            string textBlock;
            if (str_find.Length != 0)
            {
                textBlock = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                    Staff.DataSourceScheme.SchemeName, string.Format(" where dismiss = '*' and {0} ", str_find.ToString()));
            }
            else
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dtEmp = new OracleDataTable(textBlock, Connect.CurConnect);
            dtEmp.Fill();
            if (dtEmp.Rows.Count != 0)
            {
                dgViewOldEmp.DataSource = dtEmp;
                bsEmp = new BindingSource();
                bsEmp.DataSource = dtEmp;
                dgViewOldEmp.DataSource = bsEmp;
                dgViewOldEmp.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
                dgViewOldEmp.Columns["CODE_SUBDIV"].Width = 50;
                dgViewOldEmp.Columns["CODE_SUBDIV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgViewOldEmp.Columns["per_num"].HeaderText = "Таб.№";
                dgViewOldEmp.Columns["per_num"].Width = 55;
                dgViewOldEmp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgViewOldEmp.Columns["emp_last_name"].HeaderText = "Фамилия";
                dgViewOldEmp.Columns["emp_last_name"].Width = 140;
                dgViewOldEmp.Columns["emp_first_name"].HeaderText = "Имя";
                dgViewOldEmp.Columns["emp_first_name"].Width = 120;
                dgViewOldEmp.Columns["emp_middle_name"].HeaderText = "Отчество";
                dgViewOldEmp.Columns["emp_middle_name"].Width = 160;
                dgViewOldEmp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
                dgViewOldEmp.Columns["emp_birth_date"].Width = 80;
                dgViewOldEmp.Columns["sign_comb"].Visible = false;
                //dgViewOldEmp.Columns["signcomb"].HeaderText = "Совм.";
                //dgViewOldEmp.Columns["signcomb"].Width = 50;
                dgViewOldEmp.Columns["dismiss"].Visible = false;
                //dgViewOldEmp.Columns["dismiss"].HeaderText = "Увол.";
                //dgViewOldEmp.Columns["dismiss"].Width = 50;
                dgViewOldEmp.Columns["CODE_POS"].HeaderText = "Шифр профессии";
                dgViewOldEmp.Columns["CODE_POS"].Width = 90;
                dgViewOldEmp.Columns["POS_NAME"].HeaderText = "Наименование профессия";
                dgViewOldEmp.Columns["POS_NAME"].Width = 500;
                dgViewOldEmp.Columns["transfer_id"].Visible = false;
                dgViewOldEmp.Columns["WORKER_ID"].Visible = false;
                dgViewOldEmp.Columns["EMP_SEX"].Visible = false;
                dgViewOldEmp.Columns["POS_ID"].Visible = false;
                dgViewOldEmp.Columns["MED_INSPECTION_DATE"].Visible = false;
                dgViewOldEmp.Columns["STUDY_LABOR_SAFETY"].Visible = false;
                dgViewOldEmp.Columns["date_hire"].HeaderText = "Дата приема";
                dgViewOldEmp.Columns["date_hire"].Width = 80;
                dgViewOldEmp.Columns["date_transfer"].HeaderText = "Дата увольнения";
                dgViewOldEmp.Columns["date_transfer"].Width = 100;
                dgViewOldEmp.Columns["reason_name"].HeaderText = "Причина увольнения";
                dgViewOldEmp.Columns["reason_name"].Width = 200;
                dgViewOldEmp.Invalidate();
            }
            else
            {
                MessageBox.Show("В базе данных бывших работников не найдены работники, удовлетворяющие условиям поиска.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgViewOldEmp.DataSource = null;
                dgViewOldEmp.Invalidate();
            }
            bsEmpResume = new BindingSource();
            _dsEmpResume.Tables["RESUME"].Clear();
            _daResume.Fill(_dsEmpResume.Tables["RESUME"]);
            if (_dsEmpResume.Tables["RESUME"].Rows.Count != 0)
            {
                bsEmpResume.DataSource = _dsEmpResume.Tables["RESUME"];
                dgViewEmpResume.DataSource = bsEmpResume;
                dgViewEmpResume.Columns["RESUME_ID"].Visible = false;
                dgViewEmpResume.Columns["RESUME_PER_NUM"].Visible = false;
                dgViewEmpResume.Columns["SOURCE_EMPLOYABILITY_ID"].Visible = false;
                dgViewEmpResume.Columns["FILING_DATE_RESUME"].HeaderText = "Дата подачи резюме";
                dgViewEmpResume.Columns["FILING_DATE_RESUME"].Width = 140;
                dgViewEmpResume.Columns["emp_last_name"].HeaderText = "Фамилия";
                dgViewEmpResume.Columns["emp_last_name"].Width = 140;
                dgViewEmpResume.Columns["emp_first_name"].HeaderText = "Имя";
                dgViewEmpResume.Columns["emp_first_name"].Width = 120;
                dgViewEmpResume.Columns["emp_middle_name"].HeaderText = "Отчество";
                dgViewEmpResume.Columns["emp_middle_name"].Width = 160;
                dgViewEmpResume.Columns["emp_birth_date"].HeaderText = "Дата рождения";
                dgViewEmpResume.Columns["emp_birth_date"].Width = 80;
                dgViewEmpResume.Columns["EMP_SEX"].HeaderText = "Пол";
                dgViewEmpResume.Columns["EMP_SEX"].Width = 40;
                dgViewEmpResume.Columns["SOURCE_EMPLOYABILITY_NAME"].HeaderText = "Источник трудоустройства";
                dgViewEmpResume.Columns["SOURCE_EMPLOYABILITY_NAME"].Width = 180;
                dgViewEmpResume.Invalidate();
            }
            else
            {
                MessageBox.Show("В базе резюме не найдены данные, удовлетворяющие условиям поиска.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bsEmpResume.DataSource = null;
                dgViewEmpResume.DataSource = bsEmpResume;
                dgViewEmpResume.Invalidate();
            }
        }

        /// <summary>
        /// Событие ввода шифра подразделения, при котором в комбобоксе подразделения появляется его название
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Событие изменения индеска подразделения, при котором ищется шифр выбранного подразделения
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
        /// Событие нажатия кнопки, при котором появляется форма ввода нового работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNewEmp_Click(object sender, EventArgs e)
        {
            PER_NUM_BOOK_seq per_num_book = new PER_NUM_BOOK_seq(Connect.CurConnect);
            per_num_book.Fill(string.Format("where PER_NUM = (select min(TO_NUMBER(PER_NUM)) from {0}.per_num_book where FREE_SIGN = 1)",
                Connect.Schema));
            ((PER_NUM_BOOK_obj)(((CurrencyManager)BindingContext[per_num_book]).Current)).FREE_SIGN = false;
            per_num_book.Save();
            Connect.Commit();
            EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
            record_emp.AddNew();
            ((EMP_obj)(((CurrencyManager)BindingContext[record_emp]).Current)).PER_NUM = per_num_book[0].PER_NUM.ToString();
            PersonalCard personalCard = new PersonalCard(per_num_book[0].PER_NUM.ToString(), 0, record_emp, true, true, true, 0, null, false);
            personalCard.Text = "Личная карточка работника приемной базы данных";
            personalCard.ShowInTaskbar = false;
            this.Hide();
            personalCard.ShowDialog();
            Close();
        }

        /// <summary>
        /// Событие нажатия кнопки выбора найденного работника, при котором появляется форма
        /// редактирования данных бывшего работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPer_Num_Click(object sender, EventArgs e)
        {
            if (str_find != null)
            {
                if (dtEmp.Rows.Count != 0)
                {
                    string per_num = dgViewOldEmp.Rows[bsEmp.Position].Cells[1].Value.ToString();
                    EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                    record_emp.Fill(string.Format("where per_num = {0}", per_num));
                    PersonalCard personalcard = new PersonalCard(per_num, 0, record_emp, false, true, false, 0, null, false);                    
                    //personalcard.btOrder.Location = personalcard.btEditForPrivPos.Location;
                    //personalcard.btEdit.Location = new Point(277, personalcard.btEdit.Location.Y);
                    //personalcard.btSave.Location = new Point(407, personalcard.btSave.Location.Y);
                    personalcard.Text = "Личная карточка работника приемной базы данных";
                    personalcard.ShowInTaskbar = false;
                    this.Hide();
                    personalcard.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Табельный номер не выбран.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Вы не осуществили поиск бывших работников.\nВведите критерии поиска и осуществите поиск.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }            
        }

        /// <summary>
        /// Событие нажатия кнопки закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btEmp_By_Resume_Click(object sender, EventArgs e)
        {
            if (str_find != null)
            {
                if (_dsEmpResume.Tables["RESUME"].Rows.Count != 0)
                {
                    string resume_per_num = dgViewEmpResume.Rows[bsEmpResume.Position].Cells["RESUME_PER_NUM"].Value.ToString();
                    OracleCommand _ocEmp_Hire = new OracleCommand(string.Format(
                        "BEGIN {0}.EMP_HIRE(:p_RESUME_PER_NUM, :p_NEW_PER_NUM); END;", Connect.Schema), Connect.CurConnect);
                    _ocEmp_Hire.BindByName = true;
                    _ocEmp_Hire.Parameters.Add("p_RESUME_PER_NUM", OracleDbType.Varchar2).Value = resume_per_num;
                    _ocEmp_Hire.Parameters.Add("p_NEW_PER_NUM", OracleDbType.Varchar2, 10, "p_NEW_PER_NUM").Direction = ParameterDirection.InputOutput;
                    _ocEmp_Hire.Parameters["p_NEW_PER_NUM"].Value = "00001";
                    OracleTransaction Transact = Connect.CurConnect.BeginTransaction();
                    _ocEmp_Hire.Transaction = Transact;
                    _ocEmp_Hire.ExecuteNonQuery();
                    Transact.Commit();
                    string _per_num = _ocEmp_Hire.Parameters["p_NEW_PER_NUM"].Value.ToString();
                    EMP_seq record_emp = new EMP_seq(Connect.CurConnect);
                    record_emp.Fill(string.Format("where per_num = {0}", _per_num));
                    PersonalCard personalcard = new PersonalCard(_per_num, 0, record_emp, false, true, false, 0, null, false);
                    personalcard.Text = "Личная карточка работника приемной базы данных";
                    personalcard.ShowInTaskbar = false;
                    this.Hide();
                    personalcard.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Резюме не выбрано.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Вы не осуществили поиск бывших работников.\nВведите критерии поиска и осуществите поиск.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
