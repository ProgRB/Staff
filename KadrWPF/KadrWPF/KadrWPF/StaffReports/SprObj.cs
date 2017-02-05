using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using WExcel = Microsoft.Office.Interop.Excel;
using LibraryKadr;
using Staff;
using System.IO;
namespace StaffReports
{
    public partial class SprObj : Form
    {
        BindingSource bs = new BindingSource();
        DataTable _tabDir;
        public SprObj()
        {
            InitializeComponent();
            string sql = string.Format(@"select dir.per_num as per_num, substr(emp.emp_first_name,1,1)||'.'||substr(emp.emp_middle_name,1,1)||'. '||emp.emp_last_name as FIO, 
                pos.pos_name from {0}.emp, {0}.director dir, {0}.position pos 
                where dir.dir_code_pos = pos.code_pos and dir.per_num = emp.per_num and POS.POS_ACTUAL_SIGN = 1", Connect.Schema);
            OracleDataAdapter adapDir = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable tabDir = new DataTable();
            adapDir.Fill(tabDir);
            //comboBoxDir.AddBindingSource(tabDir, );
            bs.DataSource = tabDir;
            
            comboBoxDir.DataSource = bs;
            comboBoxDir.ValueMember = "per_num";
            comboBoxDir.DisplayMember = "fio";
            _tabDir = tabDir;


            //OracleCommand commDir = new OracleCommand(string.Format(sql, _nameOfSchema), _connection);
            //OracleDataReader readerDir = commDir.ExecuteReader();
            //while (readerDir.Read())
            //    comboBoxDir.Items.Add(readerDir["fio"]/*.ToString()*/);
            
            //DIRECTOR_seq director = new DIRECTOR_seq(connection);
            //director.Fill();
            //EMP_seq emp = new EMP_seq(connection);
            //emp.Fill();
            //comboBoxDir.AddBindingSource(director, DIRECTOR_seq.ColumnsName.PER_NUM,
            //    new LinkArgument(emp, EMP_seq.ColumnsName.PER_NUM));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.Trim() != "")
            {
                OracleCommand command = new OracleCommand(string.Format("SELECT per_num FROM {0}.emp em WHERE em.per_num ={1}", Connect.Schema, maskedTextBox1.Text.Trim()), Connect.CurConnect);
                if (command.ExecuteReader().HasRows)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable table = new DataTable();
                    DataColumn column1 = new DataColumn();
                    column1.ColumnName = "Head_Column";
                    column1.DataType = typeof(string);
                    table.Columns.Add(column1);
                    DataColumn column2 = new DataColumn();
                    column2.ColumnName = "Info_Emp";
                    column2.DataType = typeof(string);
                    table.Columns.Add(column2);
                    DataRow row1 = table.NewRow();
                    string sql = string.Format(Queries.GetQuery("ObjFioAddr.sql"), maskedTextBox1.Text, Connect.Schema);
                    command.CommandText = string.Format(sql, Connect.Schema);
                    sql = string.Format(Queries.GetQuery("ObjEdu.sql"), maskedTextBox1.Text, Connect.Schema);
                    OracleCommand command1 = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                    command1.BindByName = true;
                    OracleDataReader reader = command.ExecuteReader();
                    OracleDataReader reader1 = command1.ExecuteReader();
                    table.Rows.Add(row1);
                    row1["Head_Column"] = "Ф.И.О";
                    if (reader.Read())
                    {
                        row1["Info_Emp"] = reader["fio"].ToString();
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "в должности";
                        row1["Info_Emp"] = reader["qualif"].ToString();
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "в ОАО\"У-УАЗ\"";
                        row1["Info_Emp"] = reader["timeFull"].ToString();
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "Дата рождения";
                        row1["Info_Emp"] = reader["emp_birth_date"].ToString();
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "Место рождения";
                        row1["Info_Emp"] = reader["p_birth"].ToString();
                        string listEdu = "";
                        int i = 0;
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "Образование";                        
                        while (reader1.Read())
                            listEdu += ++i + ". " + reader1["te_name"].ToString() + "\n";
                        if (listEdu.Trim() != "")
                        {
                            listEdu = listEdu.Remove(listEdu.Length - 1);
                        }
                        row1["Info_Emp"] = listEdu;

                        listEdu = "";
                        i = 0;
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        reader1 = command1.ExecuteReader();
                        row1["Head_Column"] = "окончил(что,когда)";
                        while (reader1.Read())
                            listEdu += ++i + ". " + reader1["ins_name"].ToString() + "\n";
                        if (listEdu.Trim() != "")
                        {
                            listEdu = listEdu.Remove(listEdu.Length - 1);
                        }
                        row1["Info_Emp"] = listEdu;

                        listEdu = "";
                        i = 0;
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        reader1 = command1.ExecuteReader();
                        row1["Head_Column"] = "Специальность, квалификация";
                        while (reader1.Read())
                            listEdu += ++i + ". " + reader1["spc_ql"].ToString() + "\n";
                        if (listEdu.Trim() != "")
                        {
                            listEdu = listEdu.Remove(listEdu.Length - 1);
                        }
                        row1["Info_Emp"] = listEdu;

                        sql = string.Format(Queries.GetQuery("ObjEduLevel.sql"), maskedTextBox1.Text, Connect.Schema);
                        command1 = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                        command1.BindByName = true;
                        reader1 = command1.ExecuteReader();

                        listEdu = "";
                        if (reader1.Read())
                        {
                            listEdu = reader1["edu_level"].ToString();
                        }
                        while (reader1.Read())
                        {
                            listEdu = listEdu + "\n" + reader1["edu_level"].ToString();
                        }
                        ////listEdu = listEdu.Remove(listEdu.Length - 1);
                        //row1 = table.NewRow();
                        //table.Rows.Add(row1);
                        //row1["Head_Column"] = "Ученая степень";
                        //row1["Info_Emp"] = listEdu;
                        //row1 = table.NewRow();
                        //table.Rows.Add(row1);
                        //row1["Head_Column"] = "Домашний адрес";
                        //row1["Info_Emp"] = reader["address"].ToString();

                        sql = string.Format(Queries.GetQuery("ObjReward.sql"), maskedTextBox1.Text, Connect.Schema);
                        command1 = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                        command1.BindByName = true;
                        reader1 = command1.ExecuteReader();
                        row1 = table.NewRow();
                        table.Rows.Add(row1);
                        row1["Head_Column"] = "Награды:";
                        listEdu = "";
                        if (reader1.Read())
                        {
                            listEdu = reader1["rewar"].ToString();

                            while (reader1.Read())
                            {
                                listEdu += "\n" + reader1["rewar"].ToString();
                            }

                            row1["Info_Emp"] = listEdu;
                        }
                    }
                    DataTable table1 = new DataTable();
                    DataColumn column = new DataColumn();
                    column.ColumnName = "worked";
                    column.DataType = typeof(string);
                    table1.Columns.Add(column);
                    sql = string.Format(Queries.GetQuery("ObjPrevWork.sql"), maskedTextBox1.Text, Connect.Schema);
                    command1 = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                    command1.BindByName = true;
                    reader1 = command1.ExecuteReader();
                    row1 = table1.NewRow();
                    table1.Rows.Add(row1);
                    row1["worked"] = "Трудовая деятельность";
                    while (reader1.Read())
                    {
                        if (reader1["pw_firm"].ToString().Trim() != "")
                        {
                            row1 = table1.NewRow();
                            table1.Rows.Add(row1);
                            row1["worked"] = reader1["pw_firm"];
                        }
                        row1 = table1.NewRow();
                        table1.Rows.Add(row1);
                        row1["worked"] = reader1["prev"];
                    }
                    //while (reader1.Read())
                    //{
                    //    row1 = table1.NewRow();
                    //    table1.Rows.Add(row1);
                    //    row1["worked"] = reader1["pw_firm"];
                    //    row1 = table1.NewRow();
                    //    table1.Rows.Add(row1);
                    //    row1["worked"] = reader1["prev"];
                    //}
                    sql = string.Format(Queries.GetQuery("ObjWorkPlant.sql"), maskedTextBox1.Text, Connect.Schema);
                    command1 = new OracleCommand(string.Format(sql, Connect.Schema), Connect.CurConnect);
                    command1.BindByName = true;
                    reader1 = command1.ExecuteReader();
                    row1 = table1.NewRow();
                    row1["worked"] = "ОАО Улан-Удэнский авиационный завод";
                    table1.Rows.Add(row1);
                    while (reader1.Read())
                    {
                        row1 = table1.NewRow();
                        table1.Rows.Add(row1);
                        row1["worked"] = reader1["years"];
                    }
                    if (table1.Rows.Count > 2 && table.Rows.Count > 1)
                    {
                        ExcelParameter par1, par2, par3;
                        if (((DataRowView)bs.Current)["pos_name"].ToString().Trim().Length > 32)
                        {
                            int pos1 = ((DataRowView)bs.Current)["pos_name"].ToString().Trim().IndexOf(" ", 32);
                            par1 = new ExcelParameter("A" + Convert.ToString(table1.Rows.Count + table.Rows.Count + 5),
                                    ((DataRowView)bs.Current)["pos_name"].ToString().Trim().Substring(0, pos1));
                            par2 = new ExcelParameter("A" + Convert.ToString(table1.Rows.Count + table.Rows.Count + 6),
                                    ((DataRowView)bs.Current)["pos_name"].ToString().Trim().Substring(pos1+1));
                            par3 = new ExcelParameter("C" + Convert.ToString(table1.Rows.Count + table.Rows.Count + 5),
                                ((DataRowView)bs.Current)["fio"].ToString());
                        }
                        else
                        {
                            par1 = new ExcelParameter("A" + Convert.ToString(table1.Rows.Count + table.Rows.Count + 5),
                                    ((DataRowView)bs.Current)["pos_name"].ToString());
                            par2 = new ExcelParameter("C" + Convert.ToString(table1.Rows.Count + table.Rows.Count + 5),
                                ((DataRowView)bs.Current)["fio"].ToString());
                            par3 = par2;
                        }
                        ExcelParameter[] exPar = new ExcelParameter[] {
                            new ExcelParameter("A1", "Справка-объективка"),
                             par1, par2, par3, };                        
 
                        string pathTemp = Application.UserAppDataPath + @"\photo.jpg";
                        Image image = EmployeePhoto.GetPhoto(maskedTextBox1.Text.Trim().PadLeft(5, '0'));                        
                        if (image != null)
                        {
                            image = Library.ScaleImage(image, 140, 175);
                        }
                        Print1("HelpObject.xlt", "A3", new DataTable[] { table, table1 },
                            exPar, new Image[] {image},
                                false);
                        this.Cursor = Cursors.Default;
                    }
                    else
                        MessageBox.Show("Проверьте правильность ввода табельного номера!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Cursor = Cursors.Default;
                }
                else MessageBox.Show("Работника с введённым табельным номером не существует!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else MessageBox.Show("Введите табельный номер!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        void Print1(string nameOfTemplate, string startExcel, System.Data.DataTable[] tables, 
            ExcelParameter[] excelParameters, Image[] pictParams, bool flagQuit)
        {
            WExcel.Application m_ExcelApp = new WExcel.Application();
            try
            {
                //Создание страницы книги Excel
                WExcel._Worksheet m_Sheet;
                object oMissing = System.Reflection.Missing.Value;
                m_ExcelApp.Visible = false;
                string PathOfTemplate = Application.StartupPath + @"\Reports\" + nameOfTemplate;
                m_ExcelApp.Workbooks.Open(PathOfTemplate, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationManual;
                m_ExcelApp.ScreenUpdating = false;
                //Заполняем отдельные параметры
                if (excelParameters != null)
                    foreach (ExcelParameter parameter in excelParameters)
                    {
                        WExcel.Range r = m_Sheet.get_Range(parameter.NameOfExcel, parameter.NameOfEndMergeExcel);
                        r.Value2 = parameter.Value;
                        r.Merge(false);
                        if (parameter.Borders != null)
                        {
                            foreach (WExcel.XlBordersIndex border in parameter.Borders)
                            {
                                m_Sheet.get_Range(parameter.NameOfExcel, Type.Missing).Borders[border].LineStyle = WExcel.XlLineStyle.xlContinuous;
                            }
                        }
                    }
                if (pictParams != null)
                    foreach (Image picture in pictParams)
                    {
                        try
                        {
                            WExcel.Range oRange = (WExcel.Range)m_Sheet.Cells[1, 3];
                            System.Windows.Forms.Clipboard.SetDataObject(picture, true);
                            m_Sheet.Paste(oRange, Type.Missing); 
                        }
                        catch { }
                    }
                //Заполняем массив данных
                //Перебираем все таблицы
                int sumCountRow = 0, sum = 0, max = 1, RowInStr;
                if (tables != null)
                {
                    for (int i = 0; i < tables.Count(); i++)
                    {
                        sum += tables[i].Rows.Count;
                        max = Math.Max(tables[i].Columns.Count, max);
                    }
                    sum = Math.Max(sum, 1);
                    if (tables.Count() > 0)//если есть че выполнять - выделять.
                    {
                        WExcel.Range r = m_Sheet.get_Range(startExcel, Excel.AddCols(Excel.AddRows(startExcel, sum - 1), max - 1));
                        string[,] str = new string[sum, max];
                        for (int i = 0; i < tables.Count(); i++)
                        {
                            //Перебираем все колонки
                            for (int column = 0; column < tables[i].Columns.Count; column++)
                            {
                                RowInStr = sumCountRow;
                                if (tables[i].Columns[column].DataType == typeof(DateTime))
                                    for (int row = 0; row < tables[i].Rows.Count; row++, RowInStr++)
                                        str[RowInStr, column] = (tables[i].Rows[row][column] == DBNull.Value ? "" : ((DateTime)tables[i].Rows[row][column]).ToShortDateString());
                                else
                                    for (int row = 0; row < tables[i].Rows.Count; row++, RowInStr++)
                                        str[RowInStr, column] = tables[i].Rows[row][column].ToString();
                            }
                            sumCountRow += tables[i].Rows.Count;
                        }
                        r.set_Value(Type.Missing, str);
                        //заверщили заполнение.
                    }
                }
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Calculation = WExcel.XlCalculation.xlCalculationAutomatic;
                m_ExcelApp.ScreenUpdating = true;
                m_ExcelApp.Visible = true;
                if (flagQuit)
                {
                    m_Sheet.PrintPreview(true);
                    m_ExcelApp.Quit();
                }
            }
            catch
            {
                m_ExcelApp.DisplayAlerts = false;
                m_ExcelApp.Visible = true;
                m_ExcelApp.Quit();
                m_ExcelApp = null;
                throw;
            }
            finally
            {
                //Что бы там ни было вызываем сборщик мусора
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }        
    }
}
