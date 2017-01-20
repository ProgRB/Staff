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
using WExcel = Microsoft.Office.Interop.Excel;
namespace StaffReports
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //OracleDataReader reader;
            /*Connect.CurConnect = new OracleConnection();
            Connect.CurConnect.Direct = true;
            Connect.CurConnect.Server = "192.169.100.91";
            Connect.CurConnect.Port = 1521;
            Connect.CurConnect.UserId = "kiv13772";
            Connect.CurConnect.Password = "13772";
            Connect.CurConnect.Sid = "ora10";
            Connect.CurConnect.Open();*/
        }
        private void btPrint_Click(object sender, EventArgs e)
        {
            string sql = string.Format(Queries.GetQuery("EmptyInsurNum.sql"), "kiv13772"); 
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            //DataTable table2 = new DataTable();
             adapter.Fill(table);
            //adapter.SelectCommand.CommandText = string.Format("select emp_first_name,emp_last_name,emp_middle_name from {0}.emp where rownum<=10", "bmw12714");
            //adapter.Fill(table2);
            //Excel.Print("EmptyInsurNum.xlt", "A3", new DataTable[] { table/*, table2 */}, new ExcelParameter[] { new ExcelParameter("D1", "Hello world") });
             Excel.Print("EmptyInsurNum.xlt", "A3", new DataTable[] { table/*, table2 */}, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Незаполненные страховые свидетельства дата {0}.{1:d2}.{2}", DateTime.Now.Day,DateTime.Now.Month, DateTime.Now.Year)) });
        }
        ~Form1()
        {
            Connect.CurConnect.Close();
        }

        private void insurToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            EmptyInsurNum();
            //this.Cursor = Cursors.WaitCursor;
            //string sql = string.Format(Queries.GetQuery("EmptyInsurNum.sql"), _nameOfSchema/**/);
            //OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            //DataTable table = new DataTable();
            //adapter.Fill(table);            
            //Excel.Print("EmptyInsurNum.xlt", "A3", new DataTable[] { table}, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Незаполненные страховые свидетельства дата {0}.{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) });
            //this.Cursor = Cursors.Default;
        }


        private void retir_plantToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ListRetirerPlant();
            //this.Cursor = Cursors.WaitCursor;
            //string sql = string.Format(Queries.GetQuery("ListRetirerPlant.sql"), _nameOfSchema/**/);
            //OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            //DataTable table = new DataTable();
            //adapter.Fill(table);                        
            //Excel.Print("ListRetirerPlant.xlt", "A3", new DataTable[] { table});
            //this.Cursor = Cursors.Default; 
        }                

        private void list_retirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void retir_subToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListRetirerSubdiv rs = new ListRetirerSubdiv();
            rs.ShowDialog();
        }

        private void lstordacssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListOrdHire loa = new ListOrdHire();
            loa.ShowDialog();
        }

        private void endOfContrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EndOfContr eoc = new EndOfContr();
            eoc.ShowDialog();
        }

        private void bkordacssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void yearNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BkOrdHireYear boa = new BkOrdHireYear();
            boa.ShowDialog();
        }

        private void forPeriodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BkOrdHireForPeriod fp = new BkOrdHireForPeriod();
            fp.ShowDialog();
        }

        private void fullLstDsmssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListDismissFull ld = new ListDismissFull();
            ld.ShowDialog();
        }

        private void wthtAdrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListDismissShort ldwa = new ListDismissShort();
            ldwa.ShowDialog();
        }

        private void описьПриказовОбУвольненииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListOrdDismiss lod = new ListOrdDismiss();
            lod.ShowDialog();
        }

        private void lstdsmssrtrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListRetirerDismiss ldr = new ListRetirerDismiss();
            ldr.ShowDialog();
        }

        private void bkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BkOrdDismiss bod = new BkOrdDismiss();
            bod.ShowDialog();
        }

        private void repndsmssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepCountDismissR rnd = new RepCountDismissR();
            rnd.ShowDialog();
        }

        private void lstdsmssdlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListDismissJobman ldd = new ListDismissJobman();
            ldd.ShowDialog();
        }

        private void bkordtrnsfrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BkOrdTransfer bot = new BkOrdTransfer();
            bot.ShowDialog();
        }

        private void repNAcssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepCountHire rna = new RepCountHire();
            rna.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void repndsmssToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RepCountDismiss rnds = new RepCountDismiss();
            rnds.ShowDialog();
        }

        private void repCsDsmss1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepReasonDismiss1 rcd1 = new RepReasonDismiss1();
            rcd1.ShowDialog();
        }

        private void repcsdsmss2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepReasonDismiss2 rcd2 = new RepReasonDismiss2();
            rcd2.ShowDialog();
        }

        private void subdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListRetirerStructSubdiv rsn = new ListRetirerStructSubdiv();
            rsn.ShowDialog();
        }

        private void plantAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListRetirerStruct();
        //    this.Cursor = Cursors.WaitCursor;           
        //    DataTable table = new DataTable();
        //    DataColumn column1 = new DataColumn();
        //    column1.ColumnName = "Degree_Worker";
        //    column1.Caption = "Degree_Worker";
            
        //    column1.DataType = typeof(string);
        //    table.Columns.Add(column1);
        //    DataColumn column2 = new DataColumn();
        //    column2.ColumnName = "Count_Retirer";
        //    column2.DataType = typeof(int);
        //    table.Columns.Add(column2);
        //    DataColumn column3 = new DataColumn();
        //    column3.ColumnName = "Women_Retirer";
        //    column3.DataType = typeof(int);
        //    table.Columns.Add(column3);
        //    DataRow row1 = table.NewRow();
        //    OracleCommand command = new OracleCommand(string.Format("select code_subdiv from {0}.subdiv where sub_actual_sign = 0 order by code_subdiv",_nameOfSchema), Connect.CurConnect);            
        //    OracleCommand command1 = new OracleCommand(string .Format("select count(*) as result from {0}.subdiv where sub_actual_sign = 0", _nameOfSchema), Connect.CurConnect);            
        //    OracleDataReader reader = command1.ExecuteReader();
        //    int count = 0;
        //    while (reader.Read())
        //        count = Convert.ToInt32(reader["result"]);
        //    int[] sub = new int[count-1];
        //    reader.Close();
        //    reader = command.ExecuteReader();
        //    int i = 0;
        //    while (reader.Read() == true)
        //    {
        //        sub[i] = Convert.ToInt32(reader["code_subdiv"]);
        //        if (i!=(count-2))
        //        i++;
        //        //string value = reader["per_num"].ToString();
        //    }
        //    reader.Close();
        //    StringBuilder sql = new StringBuilder();
        //    for (i = 0; i <= count - 2; i++)
        //    {
        //        DataRow row = table.NewRow();

        //        sql.Remove(0, sql.Length);
        //        sql.Append(string.Format(Queries.GetQuery("ret_ttl_sub.sql"), sub[i], _nameOfSchema));
        //        command.CommandText = sql.ToString();
        //        reader = command.ExecuteReader();
        //        if (reader.Read())
        //            if (reader["rtr"].ToString() != "0")
        //            {


        //                table.Rows.Add(row);
        //                row["Degree_Worker"] = string.Format("Подразделение '{0}'", sub[i]);
        //                //table.Rows.Add(row);

        //                row = table.NewRow();
        //                row["Degree_Worker"] = string.Format("Руководители");
        //                sql.Remove(0, sql.Length);
        //                sql.Append(string.Format(Queries.GetQuery("ret_boss.sql"), sub[i], _nameOfSchema));
        //                //command = new OracleCommand(sql, Connect.CurConnect);
        //                command.CommandText = sql.ToString();
        //                reader = command.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    row["Count_Retirer"] = reader["rtr"].ToString();
        //                    //if (reader.Read())
        //                    row["Women_Retirer"] = reader["rtr_w"].ToString();
        //                }
        //                table.Rows.Add(row);
        //                sql.Remove(0, sql.Length);
        //                reader.Close();

        //                row = table.NewRow();
        //                row["Degree_Worker"] = string.Format("Специалисты");
        //                sql.Append(string.Format(Queries.GetQuery("ret_spec.sql"), sub[i], _nameOfSchema));
        //                //OracleCommand command = new OracleCommand(sql, Connect.CurConnect);
        //                //OracleDataReader 
        //                command.CommandText = sql.ToString();
        //                reader = command.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    row["Count_Retirer"] = reader["rtr"].ToString();
        //                    //if (reader.Read())
        //                    row["Women_Retirer"] = reader["rtr_w"].ToString();
        //                }
        //                table.Rows.Add(row);
        //                sql.Remove(0, sql.Length);
        //                reader.Close();

        //                row = table.NewRow();
        //                row["Degree_Worker"] = string.Format("Др.служащие");
        //                sql.Append(string.Format(Queries.GetQuery("ret_oth.sql"), sub[i], _nameOfSchema));
        //                //OracleCommand command = new OracleCommand(sql, Connect.CurConnect);
        //                //OracleDataReader
        //                command.CommandText = sql.ToString();
        //                reader = command.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    row["Count_Retirer"] = reader["rtr"].ToString();
        //                    //if (reader.Read())
        //                    row["Women_Retirer"] = reader["rtr_w"].ToString();
        //                }
        //                table.Rows.Add(row);
        //                sql.Remove(0, sql.Length);
        //                reader.Close();

        //                row = table.NewRow();
        //                row["Degree_Worker"] = string.Format("Рабочие");
        //                sql.Append(string.Format(Queries.GetQuery("ret_job.sql"), sub[i], _nameOfSchema));
        //                //OracleCommand command = new OracleCommand(sql, Connect.CurConnect);
        //                //OracleDataReader 
        //                command.CommandText = sql.ToString();
        //                reader = command.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    row["Count_Retirer"] = reader["rtr"].ToString();
        //                    //if (reader.Read())
        //                    row["Women_Retirer"] = reader["rtr_w"].ToString();
        //                }
        //                table.Rows.Add(row);
        //                sql.Remove(0, sql.Length);
        //                reader.Close();

        //                row = table.NewRow();
        //                row["Degree_Worker"] = string.Format("Итого");
        //                sql.Append(string.Format(Queries.GetQuery("ret_ttl_sub.sql"), sub[i], _nameOfSchema));
        //                //OracleCommand command = new OracleCommand(sql, Connect.CurConnect);
        //                //OracleDataReader 
        //                command.CommandText = sql.ToString();
        //                reader = command.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    row["Count_Retirer"] = reader["rtr"].ToString();
        //                    //if (reader.Read())
        //                    row["Women_Retirer"] = reader["rtr_w"].ToString();
        //                }
        //                table.Rows.Add(row);
        //                reader.Close();
        //            }
        //    }
        //    if (table.Rows.Count != 0)
        //    {
        //        Excel.Print("ListRetirerStructSubdiv.xlt", "A3", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Численность, состав и движение пенсионеров ОАО \"УУАЗ\" ")) });
        //    }
        //    else
        //    {
        //        MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }                                   
        //    this.Cursor = Cursors.Default;
            
        }


        public static void ListRetirerStruct()
        {            
            string sql = string.Format(Queries.GetQuery("RetirersBoss1.sql"), Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("RetirersSpec1.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("RetirersOther1.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("RetirersJob1.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("RetirersTotalSubdiv1.sql"), Connect.Schema);
            adapter.Fill(table);
            //decimal rez = table.AsEnumerable().Select(s => s.Field<decimal>("Rtr")).Sum();
            if (table.Rows.Count != 0)
            {
                Excel.Print("ListRetirerStructSubdiv.xlt", "B4", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Численность, состав и движение пенсионеров ОАО \"УУАЗ\" ")),
            new ExcelParameter("A3", ""),
            new ExcelParameter("A4", "Руководители"),
            new ExcelParameter("A5", "Специалисты"),
            new ExcelParameter("A6", "Др. служащие"),
            new ExcelParameter("A7", "Рабочие"),
            new ExcelParameter("A8", "Итого по заводу") });
            }
            else
            {
                MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void ListRetirerPlant()
        {

            //this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("ListRetirerPlant.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            adapter.SelectCommand.Parameters.Add("p_CODE_SUBDIV", OracleDbType.Varchar2);
            DataTable table = new DataTable();
            adapter.Fill(table);
            sql.Remove(0, sql.Length);
            sql = (string.Format(Queries.GetQuery("RetirersWithoutPension.sql"), Connect.Schema));
            adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataRow row = table.NewRow();
            row["EMP_Last_Name"] = "Пенсионеры, не включ. в список, но являющиеся ими по возрасту";
            table.Rows.Add(row);
            adapter.Fill(table);
            
            Excel.Print("ListRetirerPlant.xlt", "A3", new DataTable[] { table });
            //this.Cursor = Cursors.Default;
        }

        public static void EmptyInsurNum()
        {
            //this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("EmptyInsurNum.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            Excel.Print("EmptyInsurNum.xlt", "A3", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("A1", string.Format("Незаполненные страховые свидетельства дата {0}.{1:d2}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) });
            //this.Cursor = Cursors.Default;
        }
        public static void ListAcadDegree()
        {
            //this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("ListAcadDegree.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            Excel ec = new Excel();
            WExcel.Application m_ExcelApp;
            //Создание страницы книги Excel
            WExcel._Worksheet m_Sheet;
            WExcel.Workbooks m_Books;
            m_ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            m_ExcelApp.Visible = false;
            m_Books = m_ExcelApp.Workbooks;
            m_Sheet = (WExcel._Worksheet)m_ExcelApp.ActiveSheet;
            //private Excel.Range Range;
           
            //ec.Print2("ListAcadDegree.xlt", "A11", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("C" + Convert.ToString(table.Rows.Count + 12), "Подпись") });
            //ec.m_Sheet.Cells[table.Rows.Count + 12, 3] = "Подпись";
            //ec.m_Sheet.Cells[table.Rows.Count + 12, 4] = "Подпись";
            Excel.Print("ListAcadDegree.xlt", "A11", new DataTable[] { table }, new ExcelParameter[] { new ExcelParameter("C" + Convert.ToString(table.Rows.Count + 12), "Подпись") });
            //m_Sheet.Cells[table.Rows.Count + 12, 3] = "Подпись";
            //m_Sheet.Cells[table.Rows.Count + 12, 4] = "Подпись";
            
            //m_Sheet. Cells[table.Rows.Count + 12, 3]

            //this.Cursor = Cursors.Default;

        }

        public static void RepMRP()
        {
            string sql = string.Format(Queries.GetQuery("RepMRP.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            Excel.Print("RepMRP.xlt", "A3", new DataTable[] { table }, new ExcelParameter[] 
                { new ExcelParameter("A1", string.Format("Отчёт по материально-ответственным лицам на {0:d2}.{1:d2}.{2}", 
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)) }, false);
        }

        public static void ListVeteranStruct()
        {
            string sql = string.Format(Queries.GetQuery("VeteransBoss.sql"), Connect.Schema);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransSpec.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransOther.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransJob.sql"), Connect.Schema);
            adapter.Fill(table);
            adapter.SelectCommand.CommandText = string.Format(Queries.GetQuery("VeteransTotalSubdiv.sql"), Connect.Schema);
            adapter.Fill(table);
            if (table.Rows.Count != 0)
            {
                Excel.Print("ListVeteransStructSubdiv.xlt", "B4", new DataTable[] { table },
                    new ExcelParameter[] { 
                    new ExcelParameter("A1", string.Format("Численность, состав и движение ветеранов труда ОАО \"УУАЗ\" ")),
                    new ExcelParameter("A3", ""),
                    new ExcelParameter("A4", "Руководители"),
                    new ExcelParameter("A5", "Специалисты"),
                    new ExcelParameter("A6", "Др. служащие"),
                    new ExcelParameter("A7", "Рабочие"),
                    new ExcelParameter("A8", "Итого по заводу") });
            }
            else
            {
                MessageBox.Show("За указанный период данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void ListVeteranPlant()
        {

            //this.Cursor = Cursors.WaitCursor;
            string sql = string.Format(Queries.GetQuery("ListVeteranPlant.sql"), Connect.Schema/**/);
            OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            Excel.Print("ListVeteranPlant.xlt", "A3", new DataTable[] { table });
            //this.Cursor = Cursors.Default;
        }

        public static void ListInvalidPlant()
        {
            DateTime dateRep = DateTime.Today;
            if (InputDataForm.ShowForm(ref dateRep, "dd MMMM yyyy") == DialogResult.OK)
            {
                string sql = string.Format(Queries.GetQuery("SelectInvalid.sql"), Connect.Schema/**/);
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.Parameters.Add("p_dateRep", OracleDbType.Date).Value = dateRep;
                DataTable table = new DataTable();
                adapter.Fill(table);
                Excel.Print("ListInvalid.xlt", "A3", new DataTable[] { table }, new ExcelParameter[] {new ExcelParameter("A1", "Список инвалидов завода")});
            }
        }
    }
}
