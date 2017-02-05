using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryKadr;
using Staff;

using System.Data;
using Elegant.Ui;
namespace Kadr.Shtat
{
    public static class ShtatFilter
    {
        /// <summary>
        /// Категория фильтра штатного расписания, или Null если не выбрана категория (Все)
        /// </summary>
        public static string DegreeName
        {
            get
            {
                return (((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.SelectedItem.ToString() == "Все" || ((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.Text=="Все"? null :
                    ((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.SelectedItem.ToString());
            }
        }
        /// <summary>
        /// ID Категории фильтра штатного расписания, или Null если не выбрана категория (Все)
        /// </summary>
        public static object DegreeId
        {
            get
            {
                return 
                    (((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.SelectedValue == null ||
                    ShtatFilter.DegreeName == null || (int)((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.SelectedValue==-1 ? null :
                    ((FormMain)Application.OpenForms["FormMain"]).cbDegreeShtat.SelectedValue);
            }
        }
        public static decimal? Subdiv_id
        {
            get
            {
                return ((FormMain)Application.OpenForms["FormMain"]).subdivSelectorShtat.subdiv_id;
            }
        }
        public static int CodeSubdiv
        {
            get
            {
                return int.Parse(((FormMain)Application.OpenForms["FormMain"]).subdivSelectorShtat.CodeSubdiv);
            }
        }
        public static int SubdivName
        {
            get
            {
                return int.Parse(((FormMain)Application.OpenForms["FormMain"]).subdivSelectorShtat.SubdivName);
            }
        }
        /// <summary>
        /// Категория штатной единицы: 0- постоянная, 1 - временная
        /// </summary>
        public static int? TypeStaff
        {
            get
            {
                if (((FormMain)Application.OpenForms["FormMain"]).ShtatStaffsType.SelectedIndex > 0)
                    return ((FormMain)Application.OpenForms["FormMain"]).ShtatStaffsType.SelectedIndex - 1;
                else return null;
            }
        }
    }
}
namespace Kadr
{
    using Shtat;
    using Oracle.DataAccess.Client;
    public partial class FormMain
    {
        public void InitializeShtatSchedule()
        {
            rgFilterShtat.Enabled = true;
            btViewVacant.Enabled = false;
            btArchivStaffs.Enabled = false;
            subdivSelectorShtat.SubdivChanged += subdiv_shtat_TextChanged;
            cbDegreeShtat.SelectedValueChanged += subdiv_shtat_TextChanged;
        }
#region отчеты ШТАТНого РАСПИСАНИя
        private void btViewLayot_Click(object sender, EventArgs e)
        {
            /*for (int i = 0; i < Application.OpenForms.Count; i++)
                if (Application.OpenForms[i] is VIEW)
                {
                    Application.OpenForms[i].Activate();
                    return;
                }
            VIEW FormView = new VIEW();
            FormView.MdiParent = this;
            rgFilterShtat.Visible = true;
            FormView.FillGridLayot(0, 0);
            FormView.Show();
            btEditShtat.Enabled = btDropShtat.Enabled = false;*/
        }

        private void btViewVacant_Click(object sender, EventArgs e)
        {
            /*if (this.MdiChildren.Count() != 0)
            {
                btViewVacant.Pressed ^= true;
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Shtat.ViewVacancy frmVacant = new Kadr.Shtat.ViewVacancy();
            frmVacant.MdiParent = this;
            frmVacant.Show();*/
        }

        private void btArchivStaffs_Click(object sender, EventArgs e)
        {
            /*if (this.MdiChildren.Count() != 0)
            {
                btArchivStaffs.Pressed ^= true;
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ViewArchiv frmArchiv = new Kadr.Shtat.ViewArchiv();
            frmArchiv.MdiParent = this;
            frmArchiv.Show();*/
        }

        private void btAddShtat_Click(object sender, EventArgs e)
        {
            /*Kadr.Shtat.Add_Edit_staff frmAddShtat = new Kadr.Shtat.Add_Edit_staff(Shtat.TypeAdditionShtat.NewEmpty);
            frmAddShtat.ShowDialog(this);
            subdiv_shtat_TextChanged(null, null);*/
        }

        partial void subdiv_shtat_TextChanged(object sender, EventArgs e)
        {
            /*ShtatFilter.DegreeName = cbDegreeShtat.Text;
            ShtatFilter.TypeStaff = ShtatStaffsType.SelectedIndex;
            ShtatFilter.Subdiv_id = subdivSelectorShtat.subdiv_id;

            foreach (object f in Application.OpenForms)
            {
                if (f is Shtat.VIEW) ((Shtat.VIEW)f).Find(null, null);
                if (f is Shtat.ReportsReplEmp) ((Shtat.ReportsReplEmp)f).ReportServiceRecordRepl_Load(null, EventArgs.Empty);
                if (f is Shtat.TreeStaffEdit) ((Shtat.TreeStaffEdit)f).TreeStaffEdit_Load(null, EventArgs.Empty);
            }*/
        }

        private void btEditShtat_Click(object sender, EventArgs e)
        {
            /*if (this.MdiChildren.Count() != 0)
            {
                btViewLayot.Pressed ^= true;
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Shtat.VIEW FormView = new Kadr.Shtat.VIEW();
            FormView.MdiParent = this;
            FormView.Show();
            FormView.tabControl1.SelectedTab = FormView.tabPage6;
            btEditShtat.Enabled = btDropShtat.Enabled = false;
            rgFilterShtat.Visible = true;*/
        }

        private void btDropShtat_Click(object sender, EventArgs e)
        {
            //rgConfirmShtat.Visible = false;
            //rgFilterShtat.Visible = true;
            //rgStructures.Visible = false;
            /*Shtat.DropStaff_id frmDrop = new Kadr.Shtat.DropStaff_id(null);
            frmDrop.ShowDialog(this);*/
        }

        private void btEditSubdiv_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() != 0)
            {
                btEditSubdiv.Pressed ^= true;
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Shtat.AddToTreeEmp frmTree = new Kadr.Shtat.AddToTreeEmp();//форма редактирования подразделений и распределения штаток по ним
            frmTree.MdiParent = this;
            frmTree.Show();
        }

        private void btTreeStaffEdit_Click(object sender, EventArgs e)
        {
            /*if (this.MdiChildren.Count() != 0)
            {
                btTreeStaffEdit.Pressed ^= true;
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Shtat.TreeStaffEdit frm = new Kadr.Shtat.TreeStaffEdit();
            frm.MdiParent = this;
            frm.Show();
            rgFilterShtat.Visible = true;*/
        }

        private void R_btForm094_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Основные рабочие повременщики(ф.о. 003-094)", "staff_shtat_preview");
            ssft.ShowDialog();
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/StaffTable.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, DateTime.Now.ToShortDateString(), " and degree_id=8 ");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                DataTable t = new DataTable();
                adapter.Fill(t);

                OracleCommand command = new OracleCommand(string.Format("select code_subdiv from {1}.subdiv where subdiv_id = '{0}'", ssft.subdiv_id, DataSourceScheme.SchemeName), Connect.CurConnect);
                OracleDataReader reader = command.ExecuteReader();

                if ((t.Rows.Count > 2) && (reader.Read()))
                {
                    string avg_tar = t.Rows[t.Rows.Count - 1]["Тарифный коэфф. по схеме"].ToString(),
                        avg_raz = t.Rows[t.Rows.Count - 1]["Разряд"].ToString(),
                        avg_oklad = t.Rows[t.Rows.Count - 1]["Тарифная ставка"].ToString();
                    t.Rows.RemoveAt(t.Rows.Count - 1);

                    this.Refresh();
                    Excel.PrintWithBorder("StaffsTable.xlt", "A7", new DataTable[] { t }, new ExcelParameter[] { 
                        new ExcelParameter("A1", "Штатное расписание"), 
                        (reader[0].ToString() == "0"?
                        new ExcelParameter("A2", " на основных рабочих повременщиков по заводу"):
                        new ExcelParameter("A2", string.Format(" на основных рабочих повременщиков по подразделению {0}", reader[0].ToString()))), 
                        new ExcelParameter("A3", string.Format("по состоянию на {0}", DateTime.Now.ToShortDateString())), 
                        new ExcelParameter("B" + (t.Rows.Count + 7).ToString(), string.Format("Средний тарифный коэффициент: {0}",avg_tar )), 
                        new ExcelParameter("A" + (t.Rows.Count + 8).ToString(), "В том числе:"), 
                        new ExcelParameter("B" + (t.Rows.Count + 8).ToString(), string.Format("Разряд - {0}",avg_raz  )), 
                        new ExcelParameter("B" + (t.Rows.Count + 9).ToString(), string.Format("Оклад - {0}", avg_oklad)), 
                        new ExcelParameter("B" + (t.Rows.Count + 10).ToString(), "Поясной коэффициент - 40"),
                        new ExcelParameter("B" + (t.Rows.Count + 14).ToString(), "Начальник ТЭБ ЭУ_____________________") });
                }
                else
                {

                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;

            }
        }

        private void R_btForm095_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Вспомогательные рабочие повременщики(ф.о. 003-095)", "staff_shtat_preview");
            ssft.ShowDialog();
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/StaffTable.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, DateTime.Now.ToShortDateString(), " and degree_id=9 ");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                DataTable table = new DataTable();
                adapter.Fill(table);
                OracleCommand command = new OracleCommand(string.Format("select code_subdiv from {1}.subdiv where subdiv_id = '{0}'", ssft.subdiv_id, DataSourceScheme.SchemeName), Connect.CurConnect);
                OracleDataReader reader = command.ExecuteReader();
                if ((table.Rows.Count > 2) && (reader.Read()))
                {
                    string avg_tar = table.Rows[table.Rows.Count - 1]["Тарифный коэфф. по схеме"].ToString(),
                         avg_raz = table.Rows[table.Rows.Count - 1]["Разряд"].ToString(),
                         avg_oklad = table.Rows[table.Rows.Count - 1]["Тарифная ставка"].ToString();
                    table.Rows.RemoveAt(table.Rows.Count - 1);

                    this.Refresh();
                    Excel.PrintWithBorder("StaffsTable.xlt", "A7", new DataTable[] { table }, new ExcelParameter[] { 
                        new ExcelParameter("A1", "Штатное расписание"), 
                        (reader[0].ToString() == "0"?
                        new ExcelParameter("A2", " на вспомогательных рабочих повременщиков по заводу"):
                        new ExcelParameter("A2", string.Format(" на вспомогательных рабочих повременщиков по поразделению {0}", reader[0].ToString()))),                         
                        new ExcelParameter("A3", string.Format("с {0}", DateTime.Now.ToShortDateString())), new ExcelParameter("B" + (table.Rows.Count + 7).ToString(), string.Format("Средний тарифный коэффициент: {0}", avg_tar)), 
                        new ExcelParameter("A" + (table.Rows.Count + 8).ToString(), "В том числе:"), 
                        new ExcelParameter("B" + (table.Rows.Count + 8).ToString(), string.Format("Разряд - {0}",avg_raz)), 
                        new ExcelParameter("B" + (table.Rows.Count + 9).ToString(), string.Format("Оклад - {0}",avg_oklad)), 
                        new ExcelParameter("B" + (table.Rows.Count + 10).ToString(), "Поясной коэффициент - 40"),
                        new ExcelParameter("B" + (table.Rows.Count + 14).ToString(), "Начальник ТЭБ ЭУ_____________________") });
                }
                else
                {

                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;

            }
            //else
            //{
            //    MessageBox.Show("Вы не выбрали подразделение!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }

        private void R_btForm096_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Служащие(ф.о. 003-096)", "staff_shtat_preview");
            ssft.ShowDialog();
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/StaffTable.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, DateTime.Now.ToShortDateString(), " and degree_id=4 ");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                DataTable table = new DataTable();
                adapter.Fill(table);
                OracleCommand command = new OracleCommand(string.Format("select code_subdiv from {1}.subdiv where subdiv_id = '{0}'", ssft.subdiv_id, DataSourceScheme.SchemeName), Connect.CurConnect);
                OracleDataReader reader = command.ExecuteReader();
                if (table.Rows.Count > 2 && reader.Read())
                {
                    this.Refresh();
                    string avg_tar = table.Rows[table.Rows.Count - 1]["Тарифная ставка"].ToString(),
                        count_aup = table.Rows[table.Rows.Count - 1]["Код профессии"].ToString(),
                        count_ptp = table.Rows[table.Rows.Count - 1]["Профессия"].ToString();
                    table.Rows.RemoveAt(table.Rows.Count - 1);
                    Excel.PrintWithBorder("StaffsTable.xlt", "A7", new DataTable[] { table }, new ExcelParameter[] { 
                            new ExcelParameter("A1", "Штатное расписание"), 
                            (reader[0].ToString() == "0"?
                            new ExcelParameter("A2", " на служащих по заводу"):
                            new ExcelParameter("A2", string.Format(" на служащих по подразделению {0}", reader[0].ToString()))),                             
                            new ExcelParameter("A3", string.Format("с {0}", DateTime.Now.ToShortDateString())), 
                            new ExcelParameter("B" + (table.Rows.Count + 7).ToString(), string.Format("Средний тарифный коэффициент: {0}", avg_tar)), 
                            new ExcelParameter("A" + (table.Rows.Count + 8).ToString(), "В том числе:"), 
                            new ExcelParameter("B" + (table.Rows.Count + 8).ToString(), string.Format("АУП - {0}", count_aup)), 
                            new ExcelParameter("B" + (table.Rows.Count + 9).ToString(), string.Format("ПТП - {0}", count_ptp)), 
                            new ExcelParameter("B" + (table.Rows.Count + 10).ToString(), "Поясной коэффициент - 40"),
                            new ExcelParameter("B" + (table.Rows.Count + 14).ToString(), "Начальник ТЭБ ЭУ_____________________") });
                }
                else
                {
                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                this.Cursor = Cursors.Default;

            }
        }

        private void R_btForm094WithDate_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Основные рабочие повременщики", "staff_shtat_preview");
            ssft.ShowDialog(this);
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/EXCEL_ShtatTable_PERSON.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, DateTime.Now.ToShortDateString(), "08");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 1)
                {
                    this.Refresh();
                    Excel.PrintWithBorder("ShtatTable(person).xlt", "A4", new DataTable[] { table }, new ExcelParameter[] { 
                            new ExcelParameter("D1", ssft.subdiv_name),                            
                            new ExcelParameter("I1", string.Format("по состоянию на {0}", DateTime.Now.ToShortDateString()))
                    });
                }
                else
                {
                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;
            }

        }

        private void R_btForm095WithDate_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Основные рабочие повременщики", "staff_shtat_preview");
            ssft.ShowDialog(this);
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/EXCEL_ShtatTable_PERSON.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, DateTime.Now.ToShortDateString(), "09");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 1)
                {
                    this.Refresh();
                    Excel.PrintWithBorder("ShtatTable(person).xlt", "A4", new DataTable[] { table }, new ExcelParameter[] { 
                            new ExcelParameter("D1", ssft.subdiv_name),                            
                            new ExcelParameter("I1", string.Format("по состоянию на {0}", DateTime.Now.ToShortDateString()))
                    });
                }
                else
                {
                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;
            }
        }

        private void R_btForm096WithDate_Click(object sender, EventArgs e)
        {
            Shtat.SelectSubdivFromTree ssft = new Kadr.Shtat.SelectSubdivFromTree(Convert.ToInt32(ShtatFilter.Subdiv_id), "Основные рабочие повременщики", "staff_shtat_preview");
            ssft.ShowDialog(this);
            if (ssft.subdiv_id != "-1")
            {
                this.Cursor = Cursors.WaitCursor;
                string sql = string.Format(Queries.GetQuery("new/EXCEL_ShtatTable_PERSON.sql"), DataSourceScheme.SchemeName, ssft.subdiv_id, "04");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 1)
                {
                    this.Refresh();
                    Excel.PrintWithBorder("ShtatTable(person).xlt", "A4", new DataTable[] { table }, new ExcelParameter[] { 
                            new ExcelParameter("D1", ssft.subdiv_name),                            
                            new ExcelParameter("I1", string.Format("по состоянию на {0}", DateTime.Now.ToShortDateString()))
                    });
                }
                else
                {
                    MessageBox.Show("По данным критериям данные отсутствуют", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                this.Cursor = Cursors.Default;
            }
        }

        private void R_ddChangeST_Click(object sender, EventArgs e)
        {
            Shtat.SelectInterval si = new Kadr.Shtat.SelectInterval(false, "Изменения штатного расписания");
            si.ShowDialog();
        }

        private void R_btChangesStaffsPeriod_Click(object sender, EventArgs e)
        {
            Shtat.SelectInterval st = new Shtat.SelectInterval(true, "Изменения штатного расписания за период");
            st.ShowDialog(this);
        }

        private void R_btReplOrder_Click(object sender, EventArgs e)
        {
            Shtat.ReportsReplEmp frmrep = new Kadr.Shtat.ReportsReplEmp(Shtat.TypeReportRepl.OrderPlant, ShtatFilter.Subdiv_id, "ShtatVIEW");
            rgFilterShtat.Visible = true;
            frmrep.Name = "frmRepReplRecord";
            //frmrep.MdiParent = this;
            frmrep.TopMost = true;
            frmrep.Show(this);
        }

        private void R_btPeplSubdiv_Click(object sender, EventArgs e)
        {
            Shtat.ReportsReplEmp frmrep = new Kadr.Shtat.ReportsReplEmp(Shtat.TypeReportRepl.OrderSubdiv, ShtatFilter.Subdiv_id, "ShtatVIEW");
            rgFilterShtat.Visible = true;
            frmrep.Name = "frmRepReplRecord";
            //frmrep.MdiParent = this;
            frmrep.TopMost = true;
            frmrep.Show(this);
        }

        private void R_btReplServiceNote_Click(object sender, EventArgs e)
        {
            Shtat.ReportsReplEmp frmrep = new Kadr.Shtat.ReportsReplEmp(Shtat.TypeReportRepl.ServiseNote, ShtatFilter.Subdiv_id, "ShtatVIEW");
            rgFilterShtat.Visible = true;
            frmrep.Name = "frmRepReplRecord";
            //frmrep.MdiParent = this;
            frmrep.TopMost = true;
            frmrep.Show(this);
        }

        public static void UnCheckButtonShtat(string btName)
        {
            /*System.Windows.Forms.Control[] c = Application.OpenForms["FormMain"].Controls.Find(btName, true);
            if (c.Length > 0 && c[0] is ToggleButton)
                ((ToggleButton)c[0]).Pressed = false;
            foreach (Form f1 in Application.OpenForms)
                if (f1 is Shtat.AddSubdivToTree || f1 is Shtat.DropStaff_id || f1 is Shtat.ReportsReplEmp || f1 is Shtat.VIEW
                    || f1 is Shtat.ViewArchiv || f1 is Shtat.ViewVacancy)
                    return;
            ((FormMain)Application.OpenForms["FormMain"]).rgFilterShtat.Visible = false;*/
        }

        public void UpdateButtonsState_VS(object f)
        {
            if (f is Vacation_schedule.ArchivVac) btArchivVS.Pressed = false;
            else
                if (f is Vacation_schedule.MakeVS) btMakeVS.Pressed = false;
                else
                    if (f is Vacation_schedule.ConfirmVS) btMakePlanVS.Pressed = false;
                    else
                        if (f is Vacation_schedule.DiagramVacS) btDiagramsVS.Pressed = false;
            foreach (Form f1 in Application.OpenForms)
                if (f1 is Vacation_schedule.ArchivVac || f1 is Vacation_schedule.MakeVS || f1 is Vacation_schedule.ConfirmVS || f1 is Vacation_schedule.DiagramVacS)
                    return;
            rgFilterVS.Visible = false;
        }

#endregion
    }
}
