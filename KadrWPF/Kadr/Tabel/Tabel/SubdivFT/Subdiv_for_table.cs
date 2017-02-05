using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using System.Threading;
using System.IO;
using System.Data.Odbc;
using Staff;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Kadr;

namespace Tabel
{
    public partial class Subdiv_for_table : Form
    {
        OracleDataTable dtSubdiv_for_table, dtEmp, dtHoursAppendix;
        DateTime dateAbsence, dateSalary, dateCheck;
        OracleCommand ocDelPn, ocInsPn, ocUpdSingP, ocCommit, ocTableForAdvance, ocSelAdvance, ocDelAdvance, 
            ocTableForSalary, ocSelSalary, ocCalc_Appendix, ocUpdateSign;
        OracleDataAdapter odaRep;
        OracleDataReader reader;
        TextWriter writer;
        FormMain formMain;
        /// <summary>
        /// Подключение к таблице по численности
        /// </summary>
        OdbcConnection odbcCon;
        /// <summary>
        /// Команда подсчета количества строк в численности
        /// </summary>
        OdbcCommand _rezult;
        int month, year;
        private int subdiv_id;
        private string code_subdiv;
        /// <summary>
        /// Количество закрытых подразделений
        /// </summary>
        int countSub = 0, countProc = 0;
        public int Subdiv_id
        {
            get { return subdiv_id; }
            set { subdiv_id = value; }
        }

        public string Code_subdiv
        {
            get { return code_subdiv; }
            set { code_subdiv = value; }
        }
        
        public Subdiv_for_table(FormMain _formMain)
        {
            InitializeComponent();
            formMain = _formMain;             
            dtSubdiv_for_table = new OracleDataTable("", Connect.CurConnect);
            dtSubdiv_for_table.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectSubdiv_for_table.sql"), 
                Staff.DataSourceScheme.SchemeName);
            dtSubdiv_for_table.Fill();
            dgSubdiv_for_table.DataSource = dtSubdiv_for_table;
            RefreshGridSubdiv();

            cbMonth.SelectedIndex = DateTime.Now.Month - 1;
            cbMonth.SelectedIndexChanged += new EventHandler(cbMonth_SelectedIndexChanged);
            nudYear.Value = DateTime.Now.Year;
            label5.Text = "Количество подразделений = " + dtSubdiv_for_table.Rows.Count;
            if (DateTime.Now.Day < 16)
                rbSalary.Checked = true;
            else
                rbAdvance.Checked = true;

            /// Создаем новую команду и заполняем ее строку запроса, 
            /// которая будет удалять все записи из временной таблицы PN_TMP для 
            /// данного пользователя
            ocDelPn = new OracleCommand(
                string.Format("delete from {0}.PN_TMP where user_name = :p_user_name",
                Staff.DataSourceScheme.SchemeName), Connect.CurConnect);
            ocDelPn.BindByName = true;
            ocDelPn.Parameters.Add("p_user_name", Connect.UserId.ToUpper());
            /// Создаем строку запроса, которая будет вставлять во временную таблицу
            /// табельные номера
            ocInsPn = new OracleCommand(string.Format("insert into {0}.PN_TMP values (:PN, :UN, :TR)",
                Staff.DataSourceScheme.SchemeName), Connect.CurConnect);
            ocInsPn.BindByName = true;
            ocInsPn.Parameters.Add("PN", OracleDbType.Varchar2, 0, "PN");
            ocInsPn.Parameters.Add("UN", OracleDbType.Varchar2, 0, "UN");
            ocInsPn.Parameters.Add("TR", OracleDbType.Decimal, 0, "TR");
            /// Создаем команду для фиксации внесенных изменений в базе данных
            ocCommit = new OracleCommand("commit", Connect.CurConnect);
            ocCommit.BindByName = true;
            /// Создание таблицы сотрудников подразделения
            dtEmp = new OracleDataTable("", Connect.CurConnect);
            dtEmp.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectEmpTable.sql"),
                Staff.DataSourceScheme.SchemeName, "");
            dtEmp.SelectCommand.Parameters.Add("beginDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("endDate", OracleDbType.Date);
            dtEmp.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);

            /// Создаем команду для процедуры расчета данных аванса
            ocTableForAdvance = new OracleCommand(
                string.Format(
                "begin {0}.TABLEForAdvance(:p_month, :p_year, :p_user_name, :p_subdiv_id, :p_temp_table_id); end;",
                Connect.Schema), Connect.CurConnect);
            ocTableForAdvance.BindByName = true;
            ocTableForAdvance.Parameters.Add("p_month", OracleDbType.Decimal);
            ocTableForAdvance.Parameters.Add("p_year", OracleDbType.Decimal);
            ocTableForAdvance.Parameters.Add("p_user_name", OracleDbType.Varchar2);
            ocTableForAdvance.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ocTableForAdvance.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
            ocTableForAdvance.Parameters["p_temp_table_id"].Direction = ParameterDirection.Output;

            /// Создаем команду для запроса данных аванса
            ocSelAdvance = new OracleCommand("", Connect.CurConnect);
            ocSelAdvance.CommandText = string.Format(Queries.GetQuery("Table/SelectForAdvance.sql"),
                Connect.Schema);
            ocSelAdvance.BindByName = true;
            ocSelAdvance.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);
            ocSelAdvance.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2);

            /// Создаем команду для удаления данных 
            ocDelAdvance = new OracleCommand("", Connect.CurConnect);
            ocDelAdvance.BindByName = true;
            ocDelAdvance.CommandText = string.Format("DELETE FROM {0}.TEMP_TABLE WHERE TEMP_TABLE_ID = :p_temp_table_id",
                Connect.Schema);
            ocDelAdvance.Parameters.Add("p_temp_table_id", OracleDbType.Decimal);

            /// Создаем команду для расчета данных аванса
            ocTableForSalary = new OracleCommand("", Connect.CurConnect);
            ocTableForSalary.BindByName = true;
            ocTableForSalary.CommandText = string.Format(
                "begin {0}.TABLEFORFILE(:p_beginDate, :p_endDate, :p_user_name, :p_subdiv_id, :p_temp_salary_id); end;",
                Connect.Schema);
            ocTableForSalary.Parameters.Add("p_beginDate", OracleDbType.Date);
            ocTableForSalary.Parameters.Add("p_endDate", OracleDbType.Date);
            ocTableForSalary.Parameters.Add("p_user_name", OracleDbType.Varchar2);
            ocTableForSalary.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ocTableForSalary.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            ocTableForSalary.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;

            /// Создаем команду для запроса данных зарплаты
            ocSelSalary = new OracleCommand("", Connect.CurConnect);
            ocSelSalary.BindByName = true;
            ocSelSalary.CommandText = string.Format(
                "begin {0}.TABLE_PKG.TABLE_EXPORT_TO_SALARY(:p_subdiv_id,:p_code_subdiv, :p_temp_salary_id, :p_begin_date, :p_end_date, :p_cur_table); end;",
                Connect.Schema);
            ocSelSalary.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2);
            ocSelSalary.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            ocSelSalary.Parameters.Add("p_begin_date", OracleDbType.Date);
            ocSelSalary.Parameters.Add("p_end_date", OracleDbType.Date);
            ocSelSalary.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ocSelSalary.Parameters.Add("p_cur_table", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            /// Создаем команду для расчета данных численности
            ocCalc_Appendix = new OracleCommand("", Connect.CurConnect);
            ocCalc_Appendix.BindByName = true;
            ocCalc_Appendix.CommandText = string.Format(
                "begin {0}.Calc_Appendix(:p_user_name, :p_subdiv_id, :p_begin_date, :p_end_date, :p_temp_salary_id); end;",
                Staff.DataSourceScheme.SchemeName);
            ocCalc_Appendix.Parameters.Add("p_user_name", OracleDbType.Varchar2);      
            ocCalc_Appendix.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ocCalc_Appendix.Parameters.Add("p_begin_date", OracleDbType.Date);
            ocCalc_Appendix.Parameters.Add("p_end_date", OracleDbType.Date);
            ocCalc_Appendix.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            ocCalc_Appendix.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ocCalc_Appendix.Parameters["p_temp_salary_id"].Direction = ParameterDirection.Output;

            /// Создаем таблицу и заполняем ее
            dtHoursAppendix = new OracleDataTable("", Connect.CurConnect);
            dtHoursAppendix.SelectCommand.CommandText = 
                string.Format(Queries.GetQuery("Table/SelectAppendixDump.sql"),
                DataSourceScheme.SchemeName);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_subdiv", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_degree", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_npp", OracleDbType.Decimal);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_kdvpodr", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_f_o1", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_f_o2", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_pos1", OracleDbType.Decimal);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_pos2", OracleDbType.Decimal);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_pos3", OracleDbType.Decimal); 
            dtHoursAppendix.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_code_posC", OracleDbType.Varchar2);
            dtHoursAppendix.SelectCommand.Parameters.Add("p_days", OracleDbType.Decimal);

            odaRep = new OracleDataAdapter("", Connect.CurConnect);
            odaRep.SelectCommand.BindByName = true;
            odaRep.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectRepCalc_SalaryPrint.sql"),
                Staff.DataSourceScheme.SchemeName);
            odaRep.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            odaRep.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                Connect.UserId.ToUpper();
            odaRep.SelectCommand.Parameters.Add("p_temp_salary_id", OracleDbType.Decimal);
            odaRep.SelectCommand.Parameters.Add("p_month", OracleDbType.Varchar2);
            odaRep.SelectCommand.Parameters.Add("p_year", OracleDbType.Varchar2);

            ocUpdSingP = new OracleCommand("", Connect.CurConnect);
            ocUpdSingP.BindByName = true;
            ocUpdSingP.CommandText = string.Format(
                "update APSTAFF.SUBDIV_FOR_TABLE ST set ST.SIGN_PROCESSING = 1 " + 
                "where ST.SUBDIV_ID = :p_subdiv_id", Connect.Schema);
            ocUpdSingP.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);

            ocUpdateSign = new OracleCommand("", Connect.CurConnect);
            ocUpdateSign.BindByName = true;
            ocUpdateSign.CommandText = string.Format(
                "update {0}.SUBDIV_FOR_TABLE set SIGN_PROCESSING = :p_sign_proc", Connect.Schema);
            ocUpdateSign.Parameters.Add("p_sign_proc", OracleDbType.Int16);
            
            tsButtonPanel.EnableByRules();
                        
            dgSubdiv_for_table.ContextMenuStrip = cmSubdiv_for_table;
        }

        private void RefreshGridSubdiv()
        {
            dgSubdiv_for_table.Columns["subdiv_id"].Visible = false;
            dgSubdiv_for_table.Columns[0].Width = 50;
            dgSubdiv_for_table.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgSubdiv_for_table.Columns["code_subdiv"].HeaderText = "Шифр";
            dgSubdiv_for_table.Columns["code_subdiv"].Width = 50;
            dgSubdiv_for_table.Columns["subdiv_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgSubdiv_for_table.Columns["subdiv_name"].HeaderText = "Наименование подразделения";
            dgSubdiv_for_table.Columns["subdiv_name"].Width = 400;
            dgSubdiv_for_table.Columns["date_advance"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgSubdiv_for_table.Columns["date_advance"].HeaderText = "Дата готовности аванса";
            dgSubdiv_for_table.Columns["date_advance"].Width = 150;
            dgSubdiv_for_table.Columns["date_salary"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgSubdiv_for_table.Columns["date_salary"].HeaderText = "Дата готовности зарплаты";
            dgSubdiv_for_table.Columns["date_salary"].Width = 150;
            dgSubdiv_for_table.Columns.Remove(dgSubdiv_for_table.Columns["sign_processing"]);
            DataGridViewCheckBoxColumn c1 = new DataGridViewCheckBoxColumn();
            c1.Name = "sign_processing";
            c1.HeaderText = "Признак обработки";
            c1.ReadOnly = true;
            c1.Width = 150;
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgSubdiv_for_table.Columns.Add(c1);
            dgSubdiv_for_table.Columns["sign_processing"].DataPropertyName = "sign_processing";
        }

        /// <summary>
        /// Выбор месяца из комбобокса отображаемого табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            month = cbMonth.SelectedIndex + 1;
            year = (int)nudYear.Value;
            dateAbsence = DateTime.Parse("15." + month.ToString() + "." + nudYear.Value.ToString());
            dateSalary = DateTime.Parse(DateTime.DaysInMonth(year, month).ToString() + "." + 
                month.ToString() + "." + year.ToString());
            if (rbAdvance.Checked)
            {
                dateCheck = dateAbsence;
            }
            else
            {
                dateCheck = dateSalary;
            }
            RefreshSubdiv();
            dgSubdiv_for_table.Focus();
        }

        /// <summary>
        /// Ввод года отображаемого табеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudYear_ValueChanged(object sender, EventArgs e)
        {
            month = cbMonth.SelectedIndex + 1;
            year = (int)nudYear.Value;
            dateAbsence = DateTime.Parse("15." + month.ToString() + "." + nudYear.Value.ToString());
            dateSalary = DateTime.Parse(DateTime.DaysInMonth(year, month).ToString() + "." +
                month.ToString() + "." + year.ToString());
            if (rbAdvance.Checked)
            {
                dateCheck = dateAbsence;
            }
            else
            {
                dateCheck = dateSalary;
            }
            RefreshSubdiv();
            dgSubdiv_for_table.Focus();
        }

        /// <summary>
        /// Форматирование ячеек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgSubdiv_for_table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgSubdiv_for_table[rbAdvance.Checked ? "DATE_ADVANCE" : "DATE_SALARY", e.RowIndex].Value != DBNull.Value)
            {
                if (Convert.ToDateTime(dgSubdiv_for_table[rbAdvance.Checked ?
                    "DATE_ADVANCE" : "DATE_SALARY", e.RowIndex].Value).Date >= dateCheck
                    && Convert.ToDateTime(dgSubdiv_for_table[rbAdvance.Checked ?
                        "DATE_ADVANCE" : "DATE_SALARY", e.RowIndex].Value).Date < DateTime.Today.Date.AddDays(2))
                {
                    /// Красим в розовый цвет
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    /// Красим в розовый цвет
                    e.CellStyle.BackColor = Color.LightPink;
                }
                /*if (Convert.ToDateTime(dgSubdiv_for_table[rbAdvance.Checked ? 
                    "DATE_ADVANCE" : "DATE_SALARY", e.RowIndex].Value).Date 
                    >= dateCheck)
                {
                    /// Красим в розовый цвет
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    /// Красим в розовый цвет
                    e.CellStyle.BackColor = Color.LightPink;
                }*/
            }
            else
            {
                e.CellStyle.BackColor = Color.LightPink;
            }
        }

        private void rbAbsence_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                dateCheck = dateAbsence;
                dgSubdiv_for_table.DataSource = null;
                dgSubdiv_for_table.DataSource = dtSubdiv_for_table;                
                RefreshGridSubdiv();
                CountSubdiv();
            }
            dgSubdiv_for_table.Focus();
        }

        private void rbSalary_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                dateCheck = dateSalary;
                dgSubdiv_for_table.DataSource = null;
                dgSubdiv_for_table.DataSource = dtSubdiv_for_table;                
                RefreshGridSubdiv();
                CountSubdiv();
            }
            dgSubdiv_for_table.Focus();
        }

        private void tsbRefreshSubdiv_Click(object sender, EventArgs e)
        {
            RefreshSubdiv();
        }

        void RefreshSubdiv()
        {
            dtSubdiv_for_table.Clear();
            dtSubdiv_for_table.Fill();
            CountSubdiv();
        }

        void CountSubdiv()
        {
            countSub = countProc = 0;
            for (int i = 0; i < dtSubdiv_for_table.Rows.Count; i++)
            {
                if (dgSubdiv_for_table[rbAdvance.Checked ? "DATE_ADVANCE" : "DATE_SALARY", i].Value != DBNull.Value
                    && Convert.ToDateTime(dgSubdiv_for_table[rbAdvance.Checked ?
                    "DATE_ADVANCE" : "DATE_SALARY", i].Value)
                    >= dateCheck)
                {
                    countSub++;
                }
                if (Convert.ToInt32(dtSubdiv_for_table.Rows[i]["SIGN_PROCESSING"]) == 0)
                {
                    countProc++;
                }
            }
            label5.Text = "Количество подразделений - " + dtSubdiv_for_table.Rows.Count + 
                ". Закрыто подразделений - " + countSub.ToString() + 
                ". Не обработано - " + countProc.ToString() + ".";
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Нажатие кнопки формирования файла аванса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFormFileAdvance_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать файл на аванс?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*Thread t = new Thread(new ParameterizedThreadStart(FormAdvance));
                t.Start();
                formMain.CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                formMain.timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                formMain.timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    FormAdvance(formMain.timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                formMain.timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                formMain.timeExecute.ShowDialog();  
                RefreshSubdiv();
                if (!formMain.timeExecute.backWorker.CancellationPending)
                {
                    MessageBox.Show("Формирование файла для расчета аванса закончено!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
                /*if (formMain.timeExecute.backWorker.CancellationPending)
                {
                    MessageBox.Show("Формирование файла для расчета аванса прервано!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Формирование файла для расчета аванса закончено!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }*/
            }
        }

        /// <summary>
        /// Формирование данных для аванса
        /// </summary>
        /// <param name="data"></param>
        void FormAdvance(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                year, month.ToString().PadLeft(2, '0')));
            if (!dir.Exists)
            {
                dir.Create();
            }
            writer = new StreamWriter(dir.FullName + string.Format(@"\A{0}_{1}.txt",
                year.ToString(),
                month.ToString().PadLeft(2, '0')), false, Encoding.GetEncoding(1251));
            for (int i = 0; i < dtSubdiv_for_table.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtSubdiv_for_table.Rows[i]["SIGN_PROCESSING"]) == 0 &&
                    dtSubdiv_for_table.Rows[i]["DATE_ADVANCE"] != DBNull.Value &&
                    Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_ADVANCE"]).Date >=
                    dateAbsence && Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_ADVANCE"]).Date < DateTime.Today.Date)
                {
                    if (formMain.timeExecute.backWorker.CancellationPending)
                    {
                        MessageBox.Show("Формирование файла для расчета аванса прервано!",
                            "АРМ \"Учет рабочего времени\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    ((BackgroundWorker)sender).ReportProgress(k++ * 100 / countProc);
                    Subdiv_id = Convert.ToInt32(dtSubdiv_for_table.Rows[i]["subdiv_id"]);
                    Code_subdiv = dtSubdiv_for_table.Rows[i]["CODE_SUBDIV"].ToString();
                    LoadList(Subdiv_id, false);
                    FileAdvance();
                    UpdateSignProcessing(Subdiv_id);
                }
            }
            writer.Close();
        }

        /// <summary>
        /// Заполнение таблицы работников
        /// </summary>
        /// <param name="_subdiv_id">Идентификатор подразделения</param>
        /// <param name="_fl_insert_comb">Признак добавления данных совместителей</param>
        void LoadList(int _subdiv_id, bool _fl_insert_comb)
        {
            dtEmp.Clear();
            dtEmp.SelectCommand.Parameters["beginDate"].Value = 
                DateTime.Parse("01." + month.ToString().PadLeft(2, '0') 
                + "." + year.ToString());
            dtEmp.SelectCommand.Parameters["endDate"].Value = dateSalary;
            dtEmp.SelectCommand.Parameters["p_subdiv_id"].Value = _subdiv_id;
            dtEmp.Fill();
            InsertPerNum(dtEmp, _fl_insert_comb);
        }

        /// <summary>
        /// Вставляем данные во временную таблицу табельных номеров
        /// </summary>
        /// <param name="_dtEmp">Таблица, из которой вставляются нужные данные</param>
        /// <param name="fl_insert_comb">Признак вставки данных совместителей</param>
        void InsertPerNum(OracleDataTable _dtEmp, bool fl_insert_comb)
        {
            /// Выполняем команду
            ocDelPn.ExecuteNonQuery();
            /// Заносим во второй параметр имя пользователя  
            ocInsPn.Parameters[1].Value = Connect.UserId.ToUpper();
            /// Идем по списку работников            
            if (fl_insert_comb)
            {
                for (int i = 0; i < _dtEmp.Rows.Count; i++)
                {
                    /// Заносим в первый параметр табельный номер
                    ocInsPn.Parameters[0].Value = _dtEmp.Rows[i]["per_num"].ToString();                 
                    /// Заносим в 3 параметр признак совмещения
                    ocInsPn.Parameters[2].Value = Convert.ToInt32(_dtEmp.Rows[i]["transfer_id"]);
                    /// Выполняем команду
                    ocInsPn.ExecuteNonQuery();
                }
            }
            else
            {
                for (int i = 0; i < _dtEmp.Rows.Count; i++)
                {
                    if (_dtEmp.Rows[i]["sign_comb"].ToString() == "0")
                    {                        
                        /// Заносим в первый параметр табельный номер
                        ocInsPn.Parameters[0].Value = _dtEmp.Rows[i]["per_num"].ToString();
                        /// Заносим в 3 параметр признак совмещения
                        ocInsPn.Parameters[2].Value = Convert.ToInt32(_dtEmp.Rows[i]["transfer_id"]);
                        /// Выполняем команду
                        ocInsPn.ExecuteNonQuery();
                    }
                }
            }
            /// Выполняем команду
            ocCommit.ExecuteNonQuery();
        }

        /// <summary>
        /// Расчет данных аванса и формирование файла
        /// </summary>
        /// <param name="data"></param>
        void FileAdvance()
        {
            ocTableForAdvance.Parameters["p_month"].Value = month;
            ocTableForAdvance.Parameters["p_year"].Value = year;
            ocTableForAdvance.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ocTableForAdvance.Parameters["p_subdiv_id"].Value = Subdiv_id;            
            /// Выполняем команду
            ocTableForAdvance.ExecuteNonQuery();
            decimal tempTableID = (decimal)((OracleDecimal)(ocTableForAdvance.Parameters["p_temp_table_id"].Value));
            ocSelAdvance.Parameters["p_temp_table_id"].Value = tempTableID;
            ocSelAdvance.Parameters["p_code_subdiv"].Value = Code_subdiv;
            reader = ocSelAdvance.ExecuteReader();
            string st = "";
            while (reader.Read())
            {
                st = reader["PTN"].ToString() + reader["SC"].ToString() + reader["NP"].ToString() +
                    reader["ZN"].ToString() + reader["P_RAB"].ToString() + reader["VOP"].ToString() +
                    reader["PR"].ToString() + reader["ZAK"].ToString() + reader["TN"].ToString() + reader["HCAS"].ToString() +
                    reader["SUM"].ToString() + reader["GM"].ToString() + reader["YN"].ToString() + reader["KT"].ToString();
                writer.WriteLine(st);
            }
            ocDelAdvance.Parameters["p_temp_table_id"].Value = tempTableID;
            ocDelAdvance.ExecuteNonQuery();
        }

        /// <summary>
        /// Нажатие кнопки формирования файла зарплаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFormFileSalary_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать файл на зарплату?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*Thread t = new Thread(new ParameterizedThreadStart(FormSalary));
                t.Start();
                formMain.CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                formMain.timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                formMain.timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    FormSalary(formMain.timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                formMain.timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                formMain.timeExecute.ShowDialog();  
                RefreshSubdiv();
                if (formMain.timeExecute.backWorker.CancellationPending)
                {
                    MessageBox.Show("Формирование файла для расчета зарплаты закончено!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Формирование файла для расчета зарплаты прервано!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Формирование данных для зарплаты
        /// </summary>
        /// <param name="data"></param>
        void FormSalary(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            /* 16.07.2015 - прекращаем формировать текстовый файл по табелю - данные скидываются напрямую в таблицу SALARY.SALARY_FROM_TABLE
            DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                year, month.ToString().PadLeft(2, '0')));
            if (!dir.Exists)
            {
                dir.Create();
            }
            writer = new StreamWriter(dir.FullName + string.Format(@"\Z{0}_{1}.txt",
                year.ToString(),
                month.ToString().PadLeft(2, '0')), false, Encoding.GetEncoding(866));*/
            for (int i = 0; i < dtSubdiv_for_table.Rows.Count; i++)
            {                
                if (Convert.ToInt32(dtSubdiv_for_table.Rows[i]["SIGN_PROCESSING"]) == 0/* &&
                    dtSubdiv_for_table.Rows[i]["DATE_SALARY"] != DBNull.Value &&
                    Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_SALARY"]).Date >=
                    dateSalary && Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_SALARY"]).Date < DateTime.Today.Date*/)
                {
                    if (formMain.timeExecute.backWorker.CancellationPending)
                        break;
                    ((BackgroundWorker)sender).ReportProgress(k++ * 100 / countProc);
                    Subdiv_id = Convert.ToInt32(dtSubdiv_for_table.Rows[i]["subdiv_id"]);
                    Code_subdiv = dtSubdiv_for_table.Rows[i]["CODE_SUBDIV"].ToString();
                    LoadList(Subdiv_id, true);
                    FileSalary();
                    UpdateSignProcessing(Subdiv_id);
                }
            }
            //writer.Close();
        }

        /// <summary>
        /// Расчет данных зарплаты и формирование файла
        /// </summary>
        void FileSalary()
        {
            ocTableForSalary.Parameters["p_beginDate"].Value = 
                DateTime.Parse("01." + month.ToString().PadLeft(2, '0') + "." + year.ToString());
            ocTableForSalary.Parameters["p_endDate"].Value = new DateTime(dateSalary.Year, dateSalary.Month,
                dateSalary.Day).AddDays(1).AddSeconds(-1);
            ocTableForSalary.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ocTableForSalary.Parameters["p_subdiv_id"].Value = Subdiv_id;
            ocTableForSalary.ExecuteNonQuery();
            decimal temp_salary_id = (decimal)((OracleDecimal)(ocTableForSalary.Parameters["p_temp_salary_id"].Value));
            ocSelSalary.Parameters["p_subdiv_id"].Value = Subdiv_id;
            ocSelSalary.Parameters["p_code_subdiv"].Value = Code_subdiv;
            ocSelSalary.Parameters["p_temp_salary_id"].Value = temp_salary_id;
            ocSelSalary.Parameters["p_begin_date"].Value =
                DateTime.Parse("01." + month.ToString().PadLeft(2, '0') + "." + year.ToString());
            ocSelSalary.Parameters["p_end_date"].Value = new DateTime(dateSalary.Year, dateSalary.Month,
                dateSalary.Day).AddDays(1).AddSeconds(-1);
            ocSelSalary.ExecuteNonQuery();
            /* 16.07.2015 - прекращаем формировать текстовый файл по табелю - данные скидываются напрямую в таблицу SALARY.SALARY_FROM_TABLE
            reader = (ocSelSalary.Parameters["p_cur_table"].Value as OracleRefCursor).GetDataReader(); 
            while (reader.Read())
            {
                writer.WriteLine(reader["DATASTRING"].ToString());
            }*/
        }

        /// <summary>
        /// Нажатие кнопки формирования данных по численности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbFormAppendix_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать данные по численности?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*Thread t = new Thread(new ParameterizedThreadStart(FormAppendix));
                t.Start();
                formMain.CreateFormProgress(t);*/
                // Новый вариант от 25.09.2013
                // Создаем форму прогресса
                formMain.timeExecute = new TimeExecute();
                // Настраиваем что он должен выполнять
                formMain.timeExecute.backWorker.DoWork += new DoWorkEventHandler(delegate(object sender1, DoWorkEventArgs e1)
                {
                    FormAppendix(formMain.timeExecute.backWorker, e1);
                });
                // Запускаем теневой процесс
                formMain.timeExecute.backWorker.RunWorkerAsync();
                // Отображаем форму
                formMain.timeExecute.ShowDialog(); 
                RefreshSubdiv();
                if (formMain.timeExecute.backWorker.CancellationPending)
                {
                    MessageBox.Show("Сброс данных по численности закончен!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Сброс данных по численности прерван!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Формирование данных по численности
        /// </summary>
        /// <param name="data"></param>
        void FormAppendix(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            odbcCon = new OdbcConnection(@"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB=" + ParVal.Vals["PathAppendix"]);
            odbcCon.Open();
            for (int i = 0; i < dtSubdiv_for_table.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtSubdiv_for_table.Rows[i]["SIGN_PROCESSING"]) == 0 &&
                    dtSubdiv_for_table.Rows[i]["DATE_SALARY"] != DBNull.Value &&
                    Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_SALARY"]).Date >=
                    dateSalary && Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_SALARY"]).Date < DateTime.Today.Date)
                {
                    if (formMain.timeExecute.backWorker.CancellationPending)
                        break;
                    ((BackgroundWorker)sender).ReportProgress(k++ * 100 / countProc);
                    Subdiv_id = Convert.ToInt32(dtSubdiv_for_table.Rows[i]["subdiv_id"]);
                    Code_subdiv = dtSubdiv_for_table.Rows[i]["CODE_SUBDIV"].ToString();
                    LoadList(Subdiv_id, false);
                    DumpAppendix();
                    UpdateSignProcessing(Subdiv_id);
                }
            }
            odbcCon.Close();
        }

        void DumpAppendix()
        {
            _rezult = new OdbcCommand("", odbcCon);
            _rezult.CommandText = string.Format("select count(*) from turco " +
                "where podr = '{0}' and month(data) = {1} and year(data) = {2}", 
                Code_subdiv, month, year);
            int rez = Convert.ToInt32(_rezult.ExecuteScalar());
            if (rez == 0)
            {
                _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format(
                    "select kdvpodr from spodr where podr = '{0}' and left(kdvpodr,1) = '0'",
                    code_subdiv);
                OdbcDataReader reader = _rezult.ExecuteReader();
                string kdvpodr = "";
                while (reader.Read())
                {
                    kdvpodr = reader[0].ToString();
                }
                ocCalc_Appendix.Parameters["p_subdiv_id"].Value = Subdiv_id;
                ocCalc_Appendix.Parameters["p_begin_date"].Value =
                    DateTime.Parse("01." + month.ToString().PadLeft(2, '0') + "." + year.ToString());
                ocCalc_Appendix.Parameters["p_end_date"].Value = dateSalary;
                /// Выполняем команду
                ocCalc_Appendix.ExecuteNonQuery();
                /// Переменная содержит идентификатор записей во временной таблице часов для табеля
                decimal temp_salary_id = (decimal)((OracleDecimal)(ocCalc_Appendix.Parameters["p_temp_salary_id"].Value));
                /// Готовим данные для сброса
                dtHoursAppendix.SelectCommand.Parameters["p_temp_salary_id"].Value = temp_salary_id;
                dtHoursAppendix.SelectCommand.Parameters["p_code_subdiv"].Value = Code_subdiv;
                dtHoursAppendix.SelectCommand.Parameters["p_kdvpodr"].Value = kdvpodr;
                dtHoursAppendix.SelectCommand.Parameters["p_date_begin"].Value =
                    DateTime.Parse("01." + month.ToString().PadLeft(2, '0') + "." + year.ToString()); ;
                dtHoursAppendix.SelectCommand.Parameters["p_date_end"].Value = dateSalary;
                dtHoursAppendix.SelectCommand.Parameters["p_subdiv_id"].Value = Subdiv_id;
                dtHoursAppendix.SelectCommand.Parameters["p_days"].Value = DateTime.DaysInMonth(year, month);
                dtHoursAppendix.Clear();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "01";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 1;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "01";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 2;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "08";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 3;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "08";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 4;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "02";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 5;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "09";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 6;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "04";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 7;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "04";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 2;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 2;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 2;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 8;
                dtHoursAppendix.Fill();                
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "04";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 3;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 3;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 3;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = "3";
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 9;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "04";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 4;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 4;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 4;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = "4";
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 10;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "05";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 11;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "61";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 12;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "07";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 13;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "11";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 14;
                dtHoursAppendix.Fill();
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "12";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 15;
                dtHoursAppendix.Fill();
                /// Для 13 категории выбираем вид производства еще раз
                _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format(
                    "select kdvpodr from spodr where podr = '{0}' and left(kdvpodr,1) != '0'",
                    code_subdiv);
                reader = _rezult.ExecuteReader();
                while (reader.Read())
                {
                    kdvpodr = reader[0].ToString();
                }
                dtHoursAppendix.SelectCommand.Parameters["p_kdvpodr"].Value = kdvpodr;    
                dtHoursAppendix.SelectCommand.Parameters["p_code_degree"].Value = "13";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o1"].Value = "1";
                dtHoursAppendix.SelectCommand.Parameters["p_code_f_o2"].Value = "2";
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos1"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos2"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_pos3"].Value = 0;
                dtHoursAppendix.SelectCommand.Parameters["p_code_posC"].Value = null;
                dtHoursAppendix.SelectCommand.Parameters["p_npp"].Value = 16;
                dtHoursAppendix.Fill();
                for (int f = 0; f < dtHoursAppendix.Rows.Count; f++)
                {
                    string strIn = dtHoursAppendix.Rows[f][0].ToString() + ", ";
                    for (int j = 1; j < 4; j++)
                    {
                        strIn += "'" + dtHoursAppendix.Rows[f][j].ToString() + "', ";
                    }
                    for (int j = 4; j < dtHoursAppendix.Columns.Count; j++)
                    {
                        strIn += dtHoursAppendix.Rows[f][j].ToString() == "" ? "0, " :
                            dtHoursAppendix.Rows[f][j].ToString().Replace(",", ".") + ", ";
                    }
                    strIn += "{^" + year + "-" + month + "-" + DateTime.DaysInMonth(year, month) + "}";
                    _rezult = new OdbcCommand("", odbcCon);
                    _rezult.CommandText = string.Format(
                        "insert into turco(npp, kdvpodr, podr, kt, cdur, vrur, ccur, vcur, opsb, oppr, opnv, opgd, oppd, " +
                        "opkm, opot, oppv, opbl, sprv, goob, adad, adot, ohot, nevk, oouh, nouh, prog, slxr, " +
                        "vprd, subv, vsne, vsdr, vskm, vsnd, srsp, vskn, vsqm, prrd, data) " +
                        "values({0})", strIn
                        );
                    _rezult.ExecuteNonQuery();
                }                
                ocCommit.ExecuteNonQuery();                
            }       
        }

        private void tsbEditSubdiv_Click(object sender, EventArgs e)
        {
            EditSubdivFT editSubdivFT = new EditSubdivFT();
            editSubdivFT.ShowInTaskbar = false;
            if (editSubdivFT.ShowDialog() == DialogResult.OK)
            {
                RefreshSubdiv();
            }
        }

        private void tsbEditDate_Click(object sender, EventArgs e)
        {
            if (dgSubdiv_for_table.CurrentRow != null)
            {
                EditDateSubdivFT editDate = new EditDateSubdivFT(
                    Convert.ToInt32(dgSubdiv_for_table.CurrentRow.Cells["SUBDIV_ID"].Value),
                    dgSubdiv_for_table.CurrentRow.Cells["DATE_ADVANCE"].Value,
                    dgSubdiv_for_table.CurrentRow.Cells["DATE_SALARY"].Value,
                    Convert.ToInt32(dgSubdiv_for_table.CurrentRow.Cells["SIGN_PROCESSING"].Value)
                    );
                editDate.ShowInTaskbar = false;
                if (editDate.ShowDialog() == DialogResult.OK)
                {
                    RefreshSubdiv();
                }
            }
        }

        private void tsbDff_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сформировать файл на зарплату?", "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Thread t = new Thread(new ParameterizedThreadStart(RepCalc_Salary));
                //t.Start();
                //formMain.CreateFormProgress(t);
            }
        }

        void RepCalc_Salary(object data)
        {
            /*DirectoryInfo dir = new DirectoryInfo(ParVal.Vals["PathFileTable"] + string.Format(@"\{0}_{1}",
                    year, month.ToString().PadLeft(2, '0')));
            if (!dir.Exists)
            {
                dir.Create();
            }
            writer = new StreamWriter("C:" + string.Format(@"\R{0}_{1}.txt",
                year.ToString(),
                month.ToString().PadLeft(2, '0')), false, Encoding.GetEncoding(866));
            for (int i = 0; i < dtSubdiv_for_table.Rows.Count; i++)
            {
                if (dtSubdiv_for_table.Rows[i]["DATE_SALARY"] != DBNull.Value &&
                    Convert.ToDateTime(dtSubdiv_for_table.Rows[i]["DATE_SALARY"]).ToShortDateString() ==
                    dateSalary.ToShortDateString())
                {
                    Subdiv_id = Convert.ToInt32(dtSubdiv_for_table.Rows[i]["subdiv_id"]);
                    Code_subdiv = dtSubdiv_for_table.Rows[i]["CODE_SUBDIV"].ToString();
                    LoadList(Subdiv_id, true);
                    FileRep();
                }
            }
            writer.Close();*/
            //formMain.formProgress.DialogResult = DialogResult.OK;
        }

        void FileRep()
        {
            /*ocTableForSalary.Parameters["p_beginDate"].Value =
                    DateTime.Parse("01." + month.ToString().PadLeft(2, '0') + "." + year.ToString());
            ocTableForSalary.Parameters["p_endDate"].Value = new DateTime(dateSalary.Year, dateSalary.Month,
                dateSalary.Day).AddDays(1).AddSeconds(-1);
            ocTableForSalary.Parameters["p_user_name"].Value = Connect.UserId.ToUpper();
            ocTableForSalary.Parameters["p_subdiv_id"].Value = Subdiv_id;
            ocTableForSalary.ExecuteNonQuery();
            decimal temp_salary_id = (decimal)((OracleDecimal)(ocTableForSalary.Parameters["p_temp_salary_id"].Value));

            odaRep.SelectCommand.Parameters["p_subdiv_id"].Value = Subdiv_id;            
            odaRep.SelectCommand.Parameters["p_temp_salary_id"].Value = temp_salary_id;
            odaRep.SelectCommand.Parameters["p_month"].Value = month.ToString().PadLeft(2, '0');
            odaRep.SelectCommand.Parameters["p_year"].Value = year.ToString();
            DataTable dtRep = new DataTable();
            odaRep.Fill(dtRep);
            if (dtRep.Rows.Count > 0)
            {
                string st = "";
                int numpage = 1;
                int k = dtRep.Rows.Count / 64;
                int i, j, j1; i = j = 0;
                try
                {
                    for (i = 0; i < k + 1; i++)
                    {
                        writer.WriteLine("ЛИСТ " + numpage++);
                        j = j1 = 0;
                        while (j < 62)
                        {
                            st = dtRep.Rows[i * 64 + j1][2].ToString();
                            j += Convert.ToInt32(dtRep.Rows[i * 64 + j1][1]);
                            writer.WriteLine(st);
                            j1++;
                        }
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");
                    }
                }
                catch
                {
                    for (j1 = j; j1 < 66; j1++)
                    {
                        writer.WriteLine("");
                    }
                }                
            }    */   
        }

        private void tsbClear_Sign_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите очистить признак обработки всем подразделениям?", 
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ocUpdateSign.Parameters["p_sign_proc"].Value = 0;                
                ocUpdateSign.ExecuteNonQuery();
                Connect.Commit();
                RefreshSubdiv();
            }
        }

        private void tsbSet_Sign_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите установить признак обработки всем подразделениям?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ocUpdateSign.Parameters["p_sign_proc"].Value = 1;
                ocUpdateSign.ExecuteNonQuery();
                Connect.Commit();
                RefreshSubdiv();
            }
        }

        void UpdateSignProcessing(decimal _subdiv_id)
        {
            ocUpdSingP.Parameters["p_subdiv_id"].Value = _subdiv_id;
            ocUpdSingP.ExecuteNonQuery();
            Connect.Commit();
        }

        private void dgSubdiv_for_table_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tsbEditDate.Enabled)
                tsbEditDate_Click(sender, e);
        }

        private void tsmViewTable_Click(object sender, EventArgs e)
        {
            if (!FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows.Contains(dgSubdiv_for_table.CurrentRow.Cells["SUBDIV_ID"].Value))
            {
                MessageBox.Show("Нет доступа на просмотр данного подразделения в табеле!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /// Переменная определяет если ли активные формы табеля
            bool flagActiveTable = false;
            /// Определяет создана ли уже форма работы с табелем
            for (int i = 0; i < formMain.MdiChildren.Length; i++)
            {
                /// Если это форма табеля
                if (formMain.MdiChildren[i].Name.ToUpper() == "TABLE")
                {
                    /// Активируем эту форму
                    formMain.MdiChildren[i].Activate();
                    flagActiveTable = true;
                }
            }
            /// Если на экране показаны формы табеля, то заходим
            if (flagActiveTable)
            {
                DialogResult drQuestion = MessageBox.Show("Открыть табель в новом окне?" +
                    "\n(Да - откроется новое окно табеля)" +
                    "\n(Нет - будет использоваться текущее окно табеля)",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (drQuestion == DialogResult.No)
                {
                    ((Table)formMain.ActiveMdiChild).Close();
                    /// Скрываем отображение элементов                
                    formMain.rgOperation.Visible = false;
                    formMain.rgOrders.Visible = false;
                }
                else
                    if (drQuestion == DialogResult.Cancel)
                    {
                        return;
                    }
            }
            /// Проверяем на сколько подразделений дан доступ пользователю
            Table table;
            if (FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows.Count == 1)
            {
                FormMain.flagFilter = true;
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                table = new Table();
                table.subdiv_id =
                    Convert.ToInt32(FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_ID"]);
                table.code_subdiv =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["CODE_SUBDIV"].ToString();
                table.subdiv_name =
                    FormMain.dsSubdivTable.Tables["SUBDIV_TABLE"].Rows[0]["SUBDIV_NAME"].ToString();
                table.month = cbMonth.SelectedIndex+1;
                table.year = Convert.ToInt16(nudYear.Value);
                table.MdiParent = formMain;
                table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                table.Show();
                formMain.ClickErrorGraph(false);
                formMain.CreateButtonApp(table, formMain.btTable);
            }
            else
            {
                FormMain.flagFilter = true;
                /// Создаем форму табеля, задаем родителя и показываем ее на экране
                table = new Table();
                table.subdiv_id = Convert.ToInt16(dgSubdiv_for_table.CurrentRow.Cells["SUBDIV_ID"].Value);
                table.code_subdiv = dgSubdiv_for_table.CurrentRow.Cells["CODE_SUBDIV"].Value.ToString();
                table.subdiv_name = dgSubdiv_for_table.CurrentRow.Cells["SUBDIV_NAME"].Value.ToString();
                table.month = cbMonth.SelectedIndex + 1;
                table.year = Convert.ToInt16(nudYear.Value);
                table.MdiParent = formMain;
                table.Text = "Ведение табельного учета в подр. " + table.code_subdiv;
                table.Show();
                formMain.ClickErrorGraph(false);
                formMain.CreateButtonApp(table, formMain.btTable);
            }
        }
    }
}
