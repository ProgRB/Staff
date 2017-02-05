using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oracle.DataAccess.Client;
using LibraryKadr;
using Staff;
using WExcel = Microsoft.Office.Interop.Excel;

namespace StaffReports
{ 
    public partial class FormPersonCard : Form
    {
        public FormPersonCard()
        {
            InitializeComponent();
        }
        private string GetNameOfMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "января";
                case 2:
                    return "февраля";
                case 3:
                    return "марта";
                case 4:
                    return "апреля";
                case 5:
                    return "мая";
                case 6:
                    return "июня";
                case 7:
                    return "июля";
                case 8:
                    return "августа";
                case 9:
                    return "сентября";
                case 10:
                    return "октября";
                case 11:
                    return "ноября";
                case 12:
                    return "декабря";
                default:
                    return "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.Trim() != "")
            {
                OracleCommand command = new OracleCommand(string.Format("SELECT per_num FROM {0}.emp em WHERE em.per_num ={1}", Connect.Schema, maskedTextBox1.Text.Trim()), Connect.CurConnect);
                if (command.ExecuteReader().HasRows)
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        WExcel.Application m_ExcelApp;
                        //Создание страницы книги Excel
                        WExcel._Worksheet m_Sheet;
                        //private Excel.Range Range;
                        WExcel.Workbooks m_Books;

                        object oMissing = System.Reflection.Missing.Value;
                        m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                        //  m_ExcelApp.Visible = false;
                        m_ExcelApp.Visible = false;
                        m_Books = m_ExcelApp.Workbooks;
                        string PathOfTemplate = Application.StartupPath + @"\Reports\PersonalCard.xlt"/*+nameOfTemplate*/;
                        m_Books.Open(PathOfTemplate, oMissing, oMissing,
                       oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                       oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                        m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;

                        //m_Sheet.get_Range("R61C1", "R61C1").Value2 = "2";
                        //m_Sheet.Cells[61, 1] = "asd";
                        //m_Sheet.Cells[
                        //m_Sheet.get_Range("A1","B2").Borders
                        string sql = string.Format(Queries.GetQuery("PCShortInf.sql"), maskedTextBox1.Text, Connect.Schema);
                        command.CommandText = string.Format(sql, Connect.Schema);
                        OracleDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {

                            m_Sheet.Cells[6, 1] = reader["dat"].ToString();
                            m_Sheet.Cells[6, 7] = reader["per_num"].ToString();
                            m_Sheet.Cells[6, 12] = reader["inn"].ToString();
                            m_Sheet.Cells[6, 22] = reader["insurance_num"].ToString();
                            m_Sheet.Cells[6, 32] = reader["alphab"].ToString();
                            m_Sheet.Cells[6, 35] = reader["char_w"].ToString();
                            m_Sheet.Cells[6, 40] = reader["type_w"].ToString();
                            m_Sheet.Cells[6, 46] = reader["emp_sex"].ToString();
                            m_Sheet.Cells[10, 46] = reader["contr_emp"].ToString();
                            m_Sheet.Cells[11, 45] = reader["date_contr"].ToString();
                            m_Sheet.Cells[12, 8] = reader["emp_last_name"].ToString();
                            m_Sheet.Cells[12, 23] = reader["emp_first_name"].ToString();
                            m_Sheet.Cells[12, 39] = reader["emp_middle_name"].ToString();
                            m_Sheet.Cells[15, 11] = reader["birth_date"].ToString();
                            m_Sheet.Cells[17, 12] = reader["p_birth"].ToString();
                            m_Sheet.Cells[18, 10] = reader["citizenship"].ToString();
                            m_Sheet.Cells[19, 17] = reader["language"].ToString();
                            m_Sheet.Cells[49, 9] = reader["pos_name"].ToString();
                            m_Sheet.Cells[50, 46] = reader["code_pos"].ToString();
                            //m_Sheet.Cells[202, 1] = reader["emp_note"].ToString();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[52, 9] = reader["pos_name"].ToString();
                                m_Sheet.Cells[52, 46] = reader["code_pos"].ToString();
                            }

                            //Library.CalculationWork_Length();


                            sql = string.Format(Queries.GetQuery("PCEduc.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                                sql = reader["te_name"].ToString();
                            while (reader.Read())
                                sql = sql + ", " + reader["te_name"].ToString();
                            m_Sheet.Cells[22, 10] = sql;
                            sql = string.Format(Queries.GetQuery("PCEduc.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[26, 1] = reader["instit_name"].ToString();
                                m_Sheet.Cells[27, 22] = reader["doc"].ToString();
                                m_Sheet.Cells[27, 32] = reader["seria_diploma"].ToString();
                                m_Sheet.Cells[27, 36] = reader["num_diploma"].ToString();
                                m_Sheet.Cells[26, 40] = reader["year_graduating"].ToString();
                                m_Sheet.Cells[29, 1] = reader["qual_name"].ToString();
                                m_Sheet.Cells[29, 22] = reader["name_spec"].ToString();
                                m_Sheet.Cells[29, 46] = reader["code_spec"].ToString();
                            }
                            if (reader.Read())
                            {
                                m_Sheet.Cells[33, 1] = reader["instit_name"].ToString();
                                m_Sheet.Cells[34, 22] = reader["doc"].ToString();
                                m_Sheet.Cells[34, 32] = reader["seria_diploma"].ToString();
                                m_Sheet.Cells[34, 36] = reader["num_diploma"].ToString();
                                m_Sheet.Cells[33, 40] = reader["year_graduating"].ToString();
                                m_Sheet.Cells[36, 1] = reader["qual_name"].ToString();
                                m_Sheet.Cells[36, 22] = reader["name_spec"].ToString();
                                m_Sheet.Cells[36, 46] = reader["code_spec"].ToString();
                            }

                            sql = string.Format(Queries.GetQuery("PCPostGEduc.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[39, 26] = reader["t_p_s_n"].ToString();
                                m_Sheet.Cells[43, 1] = reader["instit_name"].ToString();
                                m_Sheet.Cells[43, 22] = reader["pgs_doc"].ToString();
                                m_Sheet.Cells[43, 40] = reader["year_grad"].ToString();
                            }

                            // m_Sheet.Cells[56, 18] = string.Format("{0}.{1}.{2}",DateTime.Now.Day,DateTime.Now.Month,DateTime.Now.Year) + ")";
                            m_Sheet.Cells[56, 19] = string.Format("{0}", DateTime.Now.Day);
                            m_Sheet.Cells[56, 22] = GetNameOfMonth(DateTime.Now.Month);
                            m_Sheet.Cells[56, 34] = string.Format("{0}", DateTime.Now.Year);

                            int yearPlant, monthPlant, dayPlant, yearPlantBL, monthPlantBL, dayPlantBL, yearPlantNow, monthPlantNow, dayPlantNow;
                            yearPlant = monthPlant = dayPlant = yearPlantBL = monthPlantBL = dayPlantBL = yearPlantNow = monthPlantNow = dayPlantNow = 0;
                            Stag(maskedTextBox1.Text, ref yearPlant, ref monthPlant, ref dayPlant, ref yearPlantBL, ref monthPlantBL, ref dayPlantBL, ref yearPlantNow, ref monthPlantNow, ref dayPlantNow);
                            /*******************************************************************



                            string per__num;
                            per__num = maskedTextBox1.Text;
                            TRANSFER_seq transferPer_Num = new TRANSFER_seq(_connection);
                            string textBlock = string.Format(Queries.GetQuery("SelectCurrentTransfer.sql"),
                                _nameOfSchema, "transfer",
                                TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.PER_NUM,
                                TRANSFER_seq.ColumnsName.DATE_TRANSFER, TRANSFER_seq.ColumnsName.SIGN_COMB,
                                TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, per__num.Trim(),
                                TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_COMB);
                            transferPer_Num.Fill(textBlock);
                            DateTime dateHire = (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPer_Num]).Current)).DATE_HIRE;
                            PREV_WORK_seq prev_work = new PREV_WORK_seq(_connection);
                            prev_work.Fill(string.Format("where {0} = {1}", PREV_WORK_seq.ColumnsName.PER_NUM, per__num));
                            int yearPlant, monthPlant, dayPlant, yearPlantBL, monthPlantBL, dayPlantBL;
                            yearPlant = monthPlant = dayPlant = yearPlantBL = monthPlantBL = dayPlantBL = 0;
                            foreach (PREV_WORK_obj r_prev_work in prev_work)
                            {
                                /// Проверка включать ли в стаж на больничный лист.
                                /// Если признак стоит True (не включать), то увеличиваем только стаж на заводе   
                                /// Если признак стоит False (включать), то увеличиваем стаж на заводе и стаж на больничный лист
                                if (r_prev_work.MEDICAL_SIGN)
                                {
                                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                                }
                                else
                                {
                                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlantBL, ref monthPlantBL, ref dayPlantBL);
                                }
                            }
                            if (dayPlant > 29)
                            {
                                dayPlant %= 30;
                                monthPlant += 1;
                            }
                            if (monthPlant > 11)
                            {
                                monthPlant %= 12;
                                yearPlant += 1;
                            }
                            if (dayPlantBL > 29)
                            {
                                dayPlantBL %= 30;
                                monthPlantBL += 1;
                            }
                            if (monthPlantBL > 11)
                            {
                                monthPlantBL %= 12;
                                yearPlantBL += 1;
                            }
                            // Непрерывный стаж на текущее время на заводе
                            int yearPlantNow, monthPlantNow, dayPlantNow;
                            yearPlantNow = monthPlantNow = dayPlantNow = 0;
                            /// Увеличиваем стаж общий
                            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlant, ref monthPlant, ref dayPlant);
                            /// Увеличиваем стаж общий
                            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlantBL, ref monthPlantBL, ref dayPlantBL);
                            /// Расчитываем непрерывный стаж
                            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlantNow, ref monthPlantNow, ref dayPlantNow);
                            if (dayPlant > 29)
                            {
                                dayPlant %= 30;
                                monthPlant += 1;
                            }
                            if (monthPlant > 11)
                            {
                                monthPlant %= 12;
                                yearPlant += 1;
                            }
                            if (dayPlantBL > 29)
                            {
                                dayPlantBL %= 30;
                                monthPlantBL += 1;
                            }
                            if (monthPlantBL > 11)
                            {
                                monthPlantBL %= 12;
                                yearPlantBL += 1;
                            }*/
                            m_Sheet.Cells[58, 45] = yearPlant;
                            m_Sheet.Cells[58, 36] = monthPlant;
                            m_Sheet.Cells[58, 28] = dayPlant;
                            m_Sheet.Cells[59, 28] = dayPlantNow;
                            m_Sheet.Cells[59, 36] = monthPlantNow;
                            m_Sheet.Cells[59, 45] = yearPlantNow;
                            //m_Sheet.Cells[60, 28] = Дающий право на надбавку за выслугу лет
                            //m_Sheet.Cells[60, 36] =
                            //m_Sheet.Cells[60, 45] = 
                            m_Sheet.Cells[61, 28] = dayPlantBL;
                            m_Sheet.Cells[61, 36] = monthPlantBL;
                            m_Sheet.Cells[61, 45] = yearPlantBL;






                            /*******************************************************************/
                            //Stag(_connection, maskedTextBox1.Text);
                            /*m_Sheet.Cells[58, 28] = общий
                            /*m_Sheet.Cells[58, 36] = 
                            m_Sheet.Cells[58, 45] =
                            m_Sheet.Cells[59, 28] = Непрерывный
                            m_Sheet.Cells[59, 36] =
                            m_Sheet.Cells[59, 45] =
                            m_Sheet.Cells[60, 28] = Дающий право на надбавку за выслугу лет
                            m_Sheet.Cells[60, 36] =
                            m_Sheet.Cells[60, 45] = */

                            sql = string.Format(Queries.GetQuery("PCPassport.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[63, 13] = reader["name_state"].ToString();
                                m_Sheet.Cells[63, 46] = reader["mar_state_id"].ToString();
                                m_Sheet.Cells[75, 8] = reader["seria_passport"].ToString();
                                m_Sheet.Cells[75, 14] = reader["num_passport"].ToString();
                                m_Sheet.Cells[75, 29] = reader["when_given"].ToString();
                                m_Sheet.Cells[76, 8] = reader["who_given"].ToString();
                                m_Sheet.Cells[81, 10] = reader["reg_post_code"].ToString();
                                m_Sheet.Cells[80, 20] = reader["rgstr"].ToString();
                                m_Sheet.Cells[85, 23] = reader["date_reg"].ToString();
                                m_Sheet.Cells[86, 11] = reader["reg_phone"].ToString();
                            }

                            sql = string.Format(Queries.GetQuery("PCTrueAddress.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[83, 10] = reader["hab_post_code"].ToString();
                                m_Sheet.Cells[82, 20] = reader["rgstr"].ToString();
                            }

                            sql = string.Format(Queries.GetQuery("PCStructFamily.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            for (int i = 68; i <= 73; i++)
                                if (reader.Read())
                                {
                                    m_Sheet.Cells[i, 1] = reader["name_rel"].ToString();
                                    m_Sheet.Cells[i, 11] = reader["relat"].ToString();
                                    m_Sheet.Cells[i, 44] = reader["year_birth"].ToString();
                                }

                            sql = string.Format(Queries.GetQuery("PCMilitary.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                m_Sheet.Cells[90, 12] = reader["res_cat"].ToString();
                                m_Sheet.Cells[91, 12] = reader["name_mil_rank"].ToString();
                                m_Sheet.Cells[92, 12] = reader["mil_cat_name"].ToString();
                                m_Sheet.Cells[93, 21] = reader["name_mil_spec"].ToString();
                                m_Sheet.Cells[94, 23] = reader["design"].ToString();
                                m_Sheet.Cells[91, 30] = reader["comm_name"].ToString();
                                m_Sheet.Cells[92, 46] = reader["mil_st"].ToString();
                                m_Sheet.Cells[94, 38] = reader["special_reg"].ToString();
                                m_Sheet.Cells[95, 32] = reader["remov"].ToString();
                            }

                            sql = string.Format(Queries.GetQuery("PCHireTransfers.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            int k = 0;
                            while ((reader.Read()) & (k <= 12))
                            {
                                m_Sheet.Cells[(110 + k), 1] = reader["date_transfer"].ToString();
                                m_Sheet.Cells[(110 + k), 7] = reader["subdiv_name"].ToString();
                                m_Sheet.Cells[(110 + k), 14] = reader["pos_name"].ToString();
                                m_Sheet.Cells[(110 + k), 30] = reader["salary"].ToString();
                                m_Sheet.Cells[(110 + k), 35] = reader["emp_in"].ToString();
                                m_Sheet.Cells[(110 + k), 41] = reader["emp_out"].ToString();
                                k++;
                            }
                            sql = string.Format(Queries.GetQuery("PCAttest.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            while ((reader.Read()) & (k <= 5))
                            {
                                m_Sheet.Cells[(129 + k), 1] = reader["date_attest"].ToString();
                                m_Sheet.Cells[(129 + k), 7] = reader["solution"].ToString();
                                m_Sheet.Cells[(129 + k), 33] = reader["num_protocol"].ToString();
                                m_Sheet.Cells[(129 + k), 38] = reader["date_protocol"].ToString();
                                m_Sheet.Cells[(129 + k), 44] = reader["base_doc_name"].ToString();
                                k++;
                            }

                            sql = string.Format(Queries.GetQuery("PCRiseQual.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            while ((reader.Read()) & (k <= 5))
                            {
                                m_Sheet.Cells[(141 + k), 1] = reader["rq_date_start"].ToString();
                                m_Sheet.Cells[(141 + k), 7] = reader["rq_date_end"].ToString();
                                m_Sheet.Cells[(141 + k), 13] = reader["type_rise_qual_name"].ToString();
                                m_Sheet.Cells[(141 + k), 18] = reader["instit_name"].ToString();
                                m_Sheet.Cells[(141 + k), 30] = reader["rq_name_doc"].ToString();
                                m_Sheet.Cells[(141 + k), 37] = reader["ser"].ToString();
                                m_Sheet.Cells[(141 + k), 41] = reader["rq_date_doc"].ToString();
                                m_Sheet.Cells[(141 + k), 46] = reader["base_doc_name"].ToString();
                                k++;
                            }

                            sql = string.Format(Queries.GetQuery("PCProfRetrain.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            while ((reader.Read()) & (k <= 3))
                            {
                                m_Sheet.Cells[(153 + k), 1] = reader["pf_date_start"].ToString();
                                m_Sheet.Cells[(153 + k), 7] = reader["pf_date_end"].ToString();
                                m_Sheet.Cells[(153 + k), 13] = reader["spec"].ToString();
                                m_Sheet.Cells[(153 + k), 28] = reader["pf_name_doc"].ToString();
                                m_Sheet.Cells[(153 + k), 35] = reader["pf_num_doc"].ToString();
                                m_Sheet.Cells[(153 + k), 39] = reader["pf_date_doc"].ToString();
                                m_Sheet.Cells[(153 + k), 45] = reader["base_DOC_NAME"].ToString();
                                k++;
                            }
                            sql = string.Format(Queries.GetQuery("PCReward.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            while ((reader.Read()) & (k <= 3))
                            {
                                m_Sheet.Cells[(163 + k), 1] = reader["rew_name"].ToString();
                                m_Sheet.Cells[(163 + k), 25] = reader["rew_doc_name"].ToString();
                                m_Sheet.Cells[(163 + k), 37] = reader["num_reward"].ToString();
                                m_Sheet.Cells[(163 + k), 43] = reader["date_reward"].ToString();
                                k++;
                            }

                            //////sql = string.Format(Queries.GetQuery("PCHoliday.sql"), maskedTextBox1.Text, _nameOfSchema);
                            //////command = new OracleCommand(string.Format(sql, _nameOfSchema), _connection);
                            //////reader = command.ExecuteReader();
                            //////k = 0;
                            //////while ((reader.Read()) & (k <= 12))
                            //////{
                            //////    m_Sheet.Cells[(173 + k), 1] = reader["name_vac"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 16] = reader["period_work_from"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 22] = reader["period_work_to"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 28] = reader["count_days"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 33] = reader["vac_date_start"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 39] = reader["vac_date_end"].ToString();
                            //////    m_Sheet.Cells[(173 + k), 45] = reader["base_DOC_NAME"].ToString();
                            //////    k++;
                            //////}

                            sql = string.Format(Queries.GetQuery("PCVac.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            while ((reader.Read()) & (k <= 3))
                            {
                                m_Sheet.Cells[(193 + k), 1] = reader["name_priv"].ToString();

                                m_Sheet.Cells[(193 + k), 26] = reader["priv_num_doc"].ToString();
                                m_Sheet.Cells[(193 + k), 33] = reader["date_give"].ToString();
                                m_Sheet.Cells[(193 + k), 40] = reader["base_doc_name"].ToString();
                                k++;
                            }
                            sql = string.Format(Queries.GetQuery("PCDismiss.sql"), maskedTextBox1.Text, Connect.Schema);
                            command = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                            command.BindByName = true;
                            reader = command.ExecuteReader();
                            k = 0;
                            if (reader.Read())
                            {
                                m_Sheet.Cells[208, 20] = reader["base_doc_name"].ToString();

                                m_Sheet.Cells[210, 12] = reader["date_transferD"].ToString();
                                m_Sheet.Cells[210, 15] = GetNameOfMonth(Convert.ToInt32(reader["date_transferM"]));
                                m_Sheet.Cells[210, 26] = reader["date_transferY"].ToString();

                                m_Sheet.Cells[211, 16] = reader["tr_num_order"].ToString();

                                m_Sheet.Cells[211, 25] = reader["tr_date_orderD"].ToString();
                                m_Sheet.Cells[211, 30] = GetNameOfMonth(Convert.ToInt32(reader["tr_date_orderM"]));
                                m_Sheet.Cells[211, 41] = reader["tr_date_orderY"].ToString();
                            }




                        }
                        reader.Close();
                        //EMP_seq emp = new EMP_seq(_connection);
                        //emp.Fill(" where per_num = 13021");
                        //EMP_obj Michael = emp[0];

                        //DataTable table = new DataTable();


                        /**/
                        m_ExcelApp.DisplayAlerts = false;
                        m_ExcelApp.Visible = true;
                        m_Sheet.PrintPreview(true);
                        m_ExcelApp.Quit();
                    }
                    finally
                    {
                        //Что бы там ни было вызываем сборщик мусора
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                    this.Cursor = Cursors.Default;
                }
                else MessageBox.Show("Работника с введённым табельным номером не существует!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else MessageBox.Show("Введите табельный номер!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void Stag(string per_num, ref int yearPlant, ref int monthPlant, ref int dayPlant, ref int yearPlantBL, ref int monthPlantBL, ref int dayPlantBL, ref int yearPlantNow, ref int monthPlantNow, ref int dayPlantNow)
        {
            per_num = maskedTextBox1.Text;
            TRANSFER_seq transferPer_Num = new TRANSFER_seq(Connect.CurConnect);
            string textBlock = string.Format(Queries.GetQuery("SelectCurrentTransfer.sql"),
                Connect.Schema , "transfer",
                TRANSFER_seq.ColumnsName.SIGN_CUR_WORK, TRANSFER_seq.ColumnsName.PER_NUM,
                TRANSFER_seq.ColumnsName.DATE_TRANSFER, TRANSFER_seq.ColumnsName.SIGN_COMB,
                TRANSFER_seq.ColumnsName.HIRE_SIGN, 1, per_num.Trim(),
                TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID, TRANSFER_seq.ColumnsName.SIGN_COMB,
                TRANSFER_seq.ColumnsName.DATE_HIRE);
            transferPer_Num.Fill(textBlock);
            DateTime dateHire = (DateTime)((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPer_Num]).Current)).DATE_HIRE;
            PREV_WORK_seq prev_work = new PREV_WORK_seq(Connect.CurConnect);
            prev_work.Fill(string.Format("where {0} = {1}", PREV_WORK_seq.ColumnsName.PER_NUM, per_num));
            //int yearPlant, monthPlant, dayPlant, yearPlantBL, monthPlantBL, dayPlantBL;
            //yearPlant = monthPlant = dayPlant = yearPlantBL = monthPlantBL = dayPlantBL = 0;
            foreach (PREV_WORK_obj r_prev_work in prev_work)
            {
                /// Проверка включать ли в стаж на больничный лист.
                /// Если признак стоит True (не включать), то увеличиваем только стаж на заводе   
                /// Если признак стоит False (включать), то увеличиваем стаж на заводе и стаж на больничный лист
                if (r_prev_work.MEDICAL_SIGN)
                {
                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                }
                else
                {
                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlant, ref monthPlant, ref dayPlant);
                    Library.CalculationWork_Length(r_prev_work.PW_DATE_START.Value, r_prev_work.PW_DATE_END.Value, ref yearPlantBL, ref monthPlantBL, ref dayPlantBL);
                }
            }
            if (dayPlant > 29)
            {
                dayPlant %= 30;
                monthPlant += 1;
            }
            if (monthPlant > 11)
            {
                monthPlant %= 12;
                yearPlant += 1;
            }
            if (dayPlantBL > 29)
            {
                dayPlantBL %= 30;
                monthPlantBL += 1;
            }
            if (monthPlantBL > 11)
            {
                monthPlantBL %= 12;
                yearPlantBL += 1;
            }
            // Непрерывный стаж на текущее время на заводе
           // int yearPlantNow, monthPlantNow, dayPlantNow;
            yearPlantNow = monthPlantNow = dayPlantNow = 0;
            /// Увеличиваем стаж общий
            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlant, ref monthPlant, ref dayPlant);
            /// Увеличиваем стаж общий
            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlantBL, ref monthPlantBL, ref dayPlantBL);
            /// Расчитываем непрерывный стаж
            Library.CalculationWork_Length(dateHire, DateTime.Now, ref yearPlantNow, ref monthPlantNow, ref dayPlantNow);
            if (dayPlant > 29)
            {
                dayPlant %= 30;
                monthPlant += 1;
            }
            if (monthPlant > 11)
            {
                monthPlant %= 12;
                yearPlant += 1;
            }
            if (dayPlantBL > 29)
            {
                dayPlantBL %= 30;
                monthPlantBL += 1;
            }
            if (monthPlantBL > 11)
            {
                monthPlantBL %= 12;
                yearPlantBL += 1;
            }
           // m_Sheet.Cells[58, 28] = yearPlant;
        }
    }
}
