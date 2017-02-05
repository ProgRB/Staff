using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

using LibraryKadr;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Application = System.Windows.Forms.Application;
using Oracle.DataAccess.Client;
using Staff;
using System.Text.RegularExpressions;
using System.Globalization;
using Kadr.Vacation_schedule;
using Microsoft.Reporting.WinForms;

using Helpers;

namespace Kadr
{
    public static class FilterVS
    {
        private static string
            _Degree_name,
            _per_num;
        private static decimal _subdiv_id;
        static int _yearVS;
        public static decimal subdiv_id
        {
            get
            {
                return (((FormMain)Application.OpenForms["FormMain"]).subdivSelectorVSFilter.subdiv_id == null ? -1 : int.Parse(((FormMain)Application.OpenForms["FormMain"]).subdivSelectorVSFilter.subdiv_id.ToString()));

            }
            set
            {
                if (_subdiv_id != value)
                {
                    _subdiv_id = value;
                    if (FilterChanged != null)
                        FilterChanged(null, EventArgs.Empty);
                }
            }
        }
        public static string code_subdiv
        {
            get
            {
                return ((FormMain)Application.OpenForms["FormMain"]).subdivSelectorVSFilter.CodeSubdiv;
            }
        }
        public static string subdiv_name
        {
            get
            {
                return ((FormMain)Application.OpenForms["FormMain"]).subdivSelectorVSFilter.SubdivName;
            }
        }
        public static string DegreeName
        {
            get
            {
                return _Degree_name;
            }
            set
            {
                if (_Degree_name != value)
                {
                    _Degree_name = value;
                    if (FilterChanged != null)
                        FilterChanged(null, EventArgs.Empty);
                }
            }
        }

        public static string per_num
        {
            get
            {
                return _per_num;
            }
            set
            {
                if (_per_num != value)
                {
                    _per_num = value;
                    if (FilterChanged != null)
                        FilterChanged(null, EventArgs.Empty);
                }
            }
        }
        public static int YearVS
        {
            get
            {
                return _yearVS;
            }
            set
            {
                if (_yearVS != value)
                {
                    _yearVS = value;
                    if (FilterChanged != null)
                        FilterChanged(null, EventArgs.Empty);
                }
            }
        }
        static FilterVS()
        {
            subdiv_id = -1;
            _Degree_name = "Все";
            _yearVS = DateTime.Today.Year;
            _per_num = "";
        }
        public static event EventHandler FilterChanged;
    }
    public static class VacEvents
    {
        public static void InitEvents(object sender)
        {
            (sender as FormMain).btAggReservDataPeriodVS.Click+=new EventHandler(btAggReservDataPeriodVS_Click);  //резерв отпусков оценочные обязательства
            (sender as FormMain).btSvodVS.Click += new EventHandler(btSvodVS_Click);  // сводный отчет по месяцам
            (sender as FormMain).R_btActualVacByRegion.Click += new EventHandler(R_btActualVacByRegion_Click);
            (sender as FormMain).R_btConsolidVacByRegion.Click += new EventHandler(R_btConsolidVacByRegion_Click);
            (sender as FormMain).btVSByGroupMaster.Click += btVsByGroupMaster;
            (sender as FormMain).btPlanOnYearVS.Click += btPlanOnYearVS_Click;
            (sender as FormMain).bt_alloc_on_Months_VS.Click += bt_alloc_on_Months_VS_Click;
            (sender as FormMain).tsbtRNoteVacVS.Click += new EventHandler(tsbtRNoteVacVS_Click);
            (sender as FormMain).btVS_RemainVacs.Click += new EventHandler(btVS_RemainVacs_Click);
            (sender as FormMain).btListVsBlock.Click += new EventHandler(btListVsBlock_Click);
            (sender as FormMain).btNoteAccountVS.Click += new EventHandler(btNoteAccountVS_Click);
        }

        /// <summary>
        /// Формирование записок-расчет по указанным отпускам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void btNoteAccountVS_Click(object sender, EventArgs e)
        {
            DateTime current_month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            GetPeriod f = new GetPeriod(current_month, new DateTime(current_month.Year, current_month.Month, DateTime.DaysInMonth(current_month.Year, current_month.Month)),
                    false, true);
            Form f_owner = sender as Form;
            if (f.ShowDialog(f_owner) == DialogResult.OK)
            {
                ViewCard.NoteAccountReport(f_owner, FilterVS.subdiv_id, f.SelectedVacIDs.ToArray());
            }
        }

        /// <summary>
        /// Формирование отчет по людям которым надо заблокировать пропуска для оптуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void btListVsBlock_Click(object sender, EventArgs e)
        {
            DateTime next_month = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1).AddMonths(1);
            GetPeriod f = new GetPeriod(next_month,
                    new DateTime(next_month.Year, next_month.Month, DateTime.DaysInMonth(next_month.Year, next_month.Month)),
                    true, true);
            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OracleDataAdapter oda = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/Rep_ListBlockEmpVS.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                    oda.SelectCommand.BindByName = true;
                    oda.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, f.DateBegin, ParameterDirection.Input);
                    oda.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, f.DateEnd, ParameterDirection.Input);
                    oda.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.SubdivID, ParameterDirection.Input);
                    DataTable t = new DataTable();
                    oda.Fill(t);
                    ReportViewerWindow.ShowReport("Список закрытых отпусков для сотрудников", "Rep_ListBlockEmpVS.rdlc",
                        t, new ReportParameter[]{
                            new ReportParameter("P_DATE1", f.DateBegin.ToShortDateString()),
                            new ReportParameter("P_DATE2", f.DateEnd.ToShortDateString()),
                            new ReportParameter("P_CODE_SUBDIV", f.CodeSubdiv)}); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка формирования отчета");
                }
            }
        }

        /// <summary>
        /// Печать отчета по отстатка периодов сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void btVS_RemainVacs_Click(object sender, EventArgs e)
        {
            decimal k = 18;
            if (NumericInput.ShowForm("Установка параметра отчета", "Укажите кол-во месяцев заработанного периода отпуска", ref k, 0) == DialogResult.OK)
            { 
                OracleDataAdapter oda = new OracleDataAdapter(String.Format(Queries.GetQuery(@"Go\Rep_RemainPeriodsVac.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                oda.SelectCommand.BindByName = true;
                oda.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_months", OracleDbType.Decimal, k, ParameterDirection.Input);
                DataSet ds = new DataSet();
                try
                {
                    oda.Fill(ds);
                    ReportViewerWindow.ShowReport("Неиспользованные периоды отпуска", "Rep_RemainPeriodVacs.rdlc", ds.Tables[0],
                        new ReportParameter[] { new ReportParameter("P_CODE_SUBDIV", DateTime.Today.ToShortDateString()) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения данных" + ex.Message, "Графики отпусков");
                }
            }
        }

        /// <summary>
        /// Формирование уведомлений для выбранных отпусков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void tsbtRNoteVacVS_Click(object sender, EventArgs e)
        {
            GetPeriod f = new GetPeriod(DateTime.Today, DateTime.Today.AddMonths(1));
            if (f.ShowDialog() == DialogResult.OK)
            {
                ViewCard.RepVacNotification(Application.OpenForms["FormMain"], FilterVS.subdiv_id, f.SelectedVacIDs.ToArray());
            }
        }

        /// <summary>
        /// Сводный отчет по участкам подразделения. Заказал 17 цех
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void R_btConsolidVacByRegion_Click(object sender, EventArgs e)
        {
            try
            {
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectConsolidVacsByRegions.sql"), Connect.Schema), Connect.CurConnect);
                DataTable t = new DataTable();
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(FilterVS.YearVS, 1, 1), ParameterDirection.Input);
                a.SelectCommand.BindByName = true;
                a.Fill(t);
                ReportViewerWindow.ShowReport("Сводный отчет по участкам подразделения", "RepConsolidVacByRegion.rdlc", t,
                    new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("CodeSubdiv", string.IsNullOrEmpty(FilterVS.code_subdiv)?"У-УАЗ":FilterVS.code_subdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("Year", FilterVS.YearVS.ToString())}.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }

        /// <summary>
        /// Фактические отпуска по группе бюро участку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void R_btActualVacByRegion_Click(object sender, EventArgs e)
        {
            try
            {
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectActualVacByRegion.sql"), Connect.Schema), Connect.CurConnect);
                DataTable t = new DataTable();
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, FilterVS.subdiv_id, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(FilterVS.YearVS,1,1), ParameterDirection.Input);
                a.SelectCommand.BindByName=true;
                a.Fill(t);
                ReportViewerWindow.ShowReport("Фактические отпуска по участкам", "ActualVacByRegions.rdlc", t,
                    new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("CodeSubdiv", string.IsNullOrEmpty(FilterVS.code_subdiv)?"У-УАЗ":FilterVS.code_subdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("Year", FilterVS.YearVS.ToString())}.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }
        /// <summary>
        /// При измеенни табельного номера вызывается наверное
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PerNumChanging(object sender, EventArgs e)
        {
            ((FormMain)Application.OpenForms["FormMain"]).cbDegreeVS_TextChanged(((FormMain)Application.OpenForms["FormMain"]).tbPerNumVS, EventArgs.Empty);
        }
        public static void PerNumKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((FormMain)Application.OpenForms["FormMain"]).cbDegreeVS_TextChanged(((FormMain)Application.OpenForms["FormMain"]).tbPerNumVS, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Резерв отпусков. Оценочные обязательства. Сразу в эксель редендриться
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void btAggReservDataPeriodVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod f = new Vacation_schedule.GetPeriod(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), true, true);
            if (f.ShowDialog() == DialogResult.OK)
            {
                DataTable t = new DataTable();
                //OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\R_btSvondVSByMonths.sql"), Connect.Schema), Connect.CurConnect);
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\R_btSvondVSByMonths_new.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_subdiv_id", f.SubdivID);
                a.SelectCommand.Parameters.Add("p_date1", f.DateBegin);
                a.SelectCommand.Parameters.Add("p_date2", f.DateEnd);
                a.SelectCommand.Parameters.Add("t",OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                try
                {
                    a.Fill(t);
                    ReportViewerWindow.RenderToExcel(Application.OpenForms["FormMain"], "RepProvisionVacByMonth.rdlc", t, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка получения данных");
                }
            }
        }
        
        /// <summary>
        /// Сводный график отпусков. Отчет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void btSvodVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod f = new Kadr.Vacation_schedule.GetPeriod(new DateTime(FilterVS.YearVS, 1, 1), new DateTime(FilterVS.YearVS, 12, 31), true, false);
            f.vacPeriodForm1.Model.IsDegreeEnabled = true;
            f.vacPeriodForm1.Model.IsFormOpertaionEnabled = true;
            if (f.ShowDialog()== DialogResult.OK)
            {
                DataTable t = new DataTable();
                bool fl = true;
                try
                {
                    OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/ComprehSchedule.sql"), Connect.Schema), Connect.CurConnect);
                    a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.SubdivID, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, f.DateBegin, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, f.DateEnd, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, f.vacPeriodForm1.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, f.vacPeriodForm1.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    a.SelectCommand.BindByName = true;
                    a.Fill(t);
                }
                catch (Exception ex)
                {
                    fl = false;
                    MessageBox.Show(ex.Message, "Ошибка получения данных");
                }
                if (fl)
                Excel.PrintWithBorder(true, "SvodnVSbyYEAR.xlt", "A5", new DataTable[] { t }, new ExcelParameter[]{
                new ExcelParameter("A1",string.Format("Сводный график отпусков на {1} г. (по подразделению № {0})",f.CodeSubdiv, Convert.ToDateTime(f.DateBegin).Year)),
                new ExcelParameter(new System.Drawing.Point(5,3),string.Format("Отпуска на {0} г. (к.д.)",f.DateBegin.Year)),
                new ExcelParameter(new System.Drawing.Point(2,6+t.Rows.Count),new System.Drawing.Point(7,6+t.Rows.Count),"Руководитель подразделения _____________________________")});
            }
        }

        /// <summary>
        /// Отпуска с разбивкой на группы мастера плановые или фактические
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void btVsByGroupMaster(object sender, EventArgs e)
        {
            GetPeriod f = new GetPeriod(new DateTime(FilterVS.YearVS, 1, 1), new DateTime(FilterVS.YearVS, 12, 31), true, false);
            if (f.ShowDialog()== DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/Rep_VS_ByGroupMaster.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, (decimal)f.SubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date, f.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date, f.DateEnd, ParameterDirection.Input);
                a.SelectCommand.BindByName = true;
                try
                {
                    a.Fill(t);
                    ReportViewerWindow.ShowReport("Отпуска по группе мастера", "Rep_VacByGroupMaster.rdlc", t,
                    new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("P_CODE_SUBDIV", string.IsNullOrEmpty(FilterVS.code_subdiv)?"У-УАЗ":FilterVS.code_subdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE1", f.DateBegin.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE2", f.DateBegin.ToShortDateString())}.ToList());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка формирования отчета");
                }
            }
        }


        /// <summary>
        /// Отчет  - план-график на год
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void btPlanOnYearVS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod frm = new Kadr.Vacation_schedule.GetPeriod(new DateTime(FilterVS.YearVS, 1, 1), new DateTime(FilterVS.YearVS, 12, 31), true, false, false);
            frm.vacPeriodForm1.Model.IsFormOpertaionEnabled = true;
            frm.vacPeriodForm1.Model.IsDegreeEnabled = true;
            if (frm.ShowDialog()== DialogResult.OK)
            {
                DataTable t_sign = null;
                if (Signes.Show(sender as Control, FilterVS.subdiv_id, "VSPlanYearReport", "Укажите ответственных лиц", 4, ref t_sign) == DialogResult.OK)
                {
                    OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery("go/R_btPlanOnYearVS.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
                    a.SelectCommand.BindByName = true;
                    a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, frm.DateBegin, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, frm.DateEnd, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.SubdivID, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, frm.vacPeriodForm1.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, frm.vacPeriodForm1.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    DataTable t = new DataTable();
                    try
                    {
                        a.Fill(t);
                        ReportViewerWindow.ShowReport("Отчет план-график на год", "Rep_VS_plan_year.rdlc", new DataTable[] { t, t_sign },
                            new ReportParameter[] { 
                                new ReportParameter("P_DATE", frm.DateEnd.ToShortDateString()), 
                                new ReportParameter("P_CODE_SUBDIV", frm.CodeSubdiv),
                                new ReportParameter("P_FIO1", t_sign.Rows.OfType<DataRow>().OrderBy(r=>r["ORDER_NUMBER"].ToString()).Select(r=>r["FIO"].ToString()).FirstOrDefault())
                            });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка формирования отчета");
                    }
                }
            }
        }

        /// <summary>
        /// Распределение отпусков по месяцам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void bt_alloc_on_Months_VS_Click(object sender, EventArgs e)
        {
            Vacation_schedule.GetPeriod frm = new Kadr.Vacation_schedule.GetPeriod(new DateTime(FilterVS.YearVS, 1, 1), new DateTime(FilterVS.YearVS, 12, 31), true, false, false);
            decimal kf = 0;
            frm.vacPeriodForm1.Model.IsFormOpertaionEnabled = true;
            frm.vacPeriodForm1.Model.IsDegreeEnabled = true;
            if (frm.ShowDialog() == DialogResult.OK && Vacation_schedule.NumericInput.ShowForm("Введите процент", "Введите процент увеличения средней заработной платы:", ref kf, 2) == DialogResult.OK)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go\AllocVSByMonths.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.SubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, frm.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, frm.DateEnd, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, frm.vacPeriodForm1.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, frm.vacPeriodForm1.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                a.SelectCommand.Parameters.Add("kf_proc", OracleDbType.Decimal, kf, ParameterDirection.Input);
                a.SelectCommand.BindByName = true;
                bool fl = true;
                try
                {
                    a.Fill(t);
                }
                catch (Exception ex)
                {
                    fl = false;
                    MessageBox.Show(ex.Message, "Ошибка получения данных");
                }
                if (fl)
                Excel.PrintWithBorder(true, "VSAllocVSbyMonth.xlt", "A7", new DataTable[] { t }, new ExcelParameter[]
                {
                    new ExcelParameter("A2",string.Format("Распределение отпусков по месяцам на {0} г. (по цеху/отделу № {1})",frm.DateBegin.Year,frm.CodeSubdiv))
                });
            }
        }
    }
}
