using Classes;
using KadrWPF.Helpers;
using LibraryKadr;
using Microsoft.Reporting.WinForms;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VacationSchedule;
using MExcel = Microsoft.Office.Interop.Excel;

namespace VacationSchedule
{
    public interface IVacFilter
    {
        string GetPerNum();
        decimal? GetSubdivID();
        string GetCodeSubdiv();
        string GetSubdivName();
        int GetCurrentYear();
    }

}
namespace KadrWPF
{ 
    public partial class MainWindow
    {
        public void InitVacCommandBindings()
        {
            CommandBindings.Add(new CommandBinding(AppCommands.BtPlanOnYearVS, BtPlanOnYearVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtPlanOnYearVS, BtPlanOnYearVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtPlanOnYearVS, BtPlanOnYearVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.Bt_alloc_on_Months_VS, Bt_alloc_on_Months_VS_Execute, MenuCommand_CanExecute));

            CommandBindings.Add(new CommandBinding(AppCommands.BtSvodVS, BtSvodVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtVSByGroupMaster, BtVSByGroupMaster_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_btActualVacByRegion, R_btActualVacByRegion_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_btConsolidVacByRegion, R_btConsolidVacByRegion_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtVS_RemainVacs, BtVS_RemainVacs_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtListVsBlock, BtListVsBlock_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_btPrikazZavodVS, R_btPrikazZavodVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_btSubPrikazVS, R_btSubPrikazVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.TsbtRNoteVacVS, TsbtRNoteVacVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtNoteAccountVS, BtNoteAccountVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_btActualDatesByPeriod, R_btActualDatesByPeriod_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.R_CountPlanSumDaysVS, R_CountPlanSumDaysVS_Execute, MenuCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(AppCommands.BtAggReservDataPeriodVS, BtAggReservDataPeriodVS_Execute, MenuCommand_CanExecute));
        }

        /// <summary>
        /// ТЕкущий выбранный год
        /// </summary>
        public int CurrentVacYear
        {
            get
            {
                if (OpenTabs.SelectedTab.ContentData is IVacFilter)
                    return (OpenTabs.SelectedTab.ContentData as IVacFilter).GetCurrentYear();
                else
                    return DateTime.Today.Year;
            }
        }

        /// <summary>
        /// Текущее выбранное подарзделение оптуска
        /// </summary>
        public decimal? CurrentVacSubdivID
        {
            get
            {
                if (OpenTabs.SelectedTab.ContentData is IVacFilter)
                    return (OpenTabs.SelectedTab.ContentData as IVacFilter).GetSubdivID();
                else
                    return null;
            }
        }
        /// <summary>
        /// Текущий выбранное подразделение отпуска
        /// </summary>
        public string CurrentVacCodeSubdiv
        {
            get
            {
                if (OpenTabs.SelectedTab.ContentData is IVacFilter)
                    return (OpenTabs.SelectedTab.ContentData as IVacFilter).GetCodeSubdiv();
                else
                    return null;
            }
        }


        /// <summary>
        /// Отчет  - план-график на год
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPlanOnYearVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm frm = new  VacPeriodForm(new DateTime(CurrentVacYear, 1, 1), new DateTime(CurrentVacYear, 12, 31), CurrentVacSubdivID, true, false, false);
            frm.Model.IsFormOpertaionEnabled = true;
            frm.Model.IsDegreeEnabled = true;
            frm.Owner = Window.GetWindow(this);
            if (frm.ShowDialog() == true)
            {
                DataTable t_sign = null;
                if (LibraryKadr.Signes.Show(CurrentVacSubdivID, "VSPlanYearReport", "Укажите ответственных лиц", 4, ref t_sign) == true)
                {
                    OracleDataAdapter a = new OracleDataAdapter(Queries.GetQueryWithSchema("go/R_btPlanOnYearVS.sql"), Connect.CurConnect);
                    a.SelectCommand.BindByName = true;
                    a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, frm.Model.DateBegin, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, frm.Model.DateEnd, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.Model.SubdivID, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, frm.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, frm.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных", a, a.SelectCommand,
                        (p, pw) =>
                        {
                            ReportViewerWindow.ShowReport("Отчет план-график на год", "Rep_VS_plan_year.rdlc", new DataTable[] { (pw.Result as DataSet).Tables[0], t_sign },
                                new ReportParameter[] {
                                new ReportParameter("P_DATE", frm.Model.DateEnd.Value.ToShortDateString()),
                                new ReportParameter("P_CODE_SUBDIV", frm.Model.CodeSubdiv),
                                new ReportParameter("P_FIO1", t_sign.Rows.OfType<DataRow>().OrderBy(r=>r["ORDER_NUMBER"].ToString()).Select(r=>r["FIO"].ToString()).FirstOrDefault())
                                });
                        });
                }
            }
        }

        /// <summary>
        /// Распределение отпусков по месяцам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_alloc_on_Months_VS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm frm = new VacPeriodForm(new DateTime(CurrentVacYear, 1, 1), new DateTime(CurrentVacYear, 12, 31), CurrentVacSubdivID, true, false, false);
            decimal kf = 0;
            frm.Model.IsFormOpertaionEnabled = true;
            frm.Model.IsDegreeEnabled = true;
            frm.Owner = Window.GetWindow(this);
            if (frm.ShowDialog() == true/* && NumericInput.ShowForm("Введите процент", "Введите процент увеличения средней заработной платы:", ref kf, 2) == DialogResult.OK*/)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go\AllocVSByMonths.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.Model.SubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, frm.Model.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, frm.Model.DateEnd, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, frm.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, frm.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
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
                    new ExcelParameter("A2",string.Format("Распределение отпусков по месяцам на {0} г. (по цеху/отделу № {1})",frm.Model.DateBegin.Value.Year,frm.Model.CodeSubdiv))
                    });
            }
        }

        /// <summary>
        /// Сводный график отпусков. Отчет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtSvodVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm f = new VacPeriodForm(new DateTime(CurrentVacYear, 1, 1), new DateTime(CurrentVacYear, 12, 31), CurrentVacSubdivID, true, false);
            f.Model.IsDegreeEnabled = true;
            f.Model.IsFormOpertaionEnabled = true;
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                DataTable t = new DataTable();
                bool fl = true;
                try
                {
                    OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/ComprehSchedule.sql"), Connect.Schema), Connect.CurConnect);
                    a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.Model.SubdivID, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, f.Model.DateBegin, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, f.Model.DateEnd, ParameterDirection.Input);
                    a.SelectCommand.Parameters.Add("p_degree_ids", OracleDbType.Array, f.Model.SelectedDegrees, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                    a.SelectCommand.Parameters.Add("p_form_oper_ids", OracleDbType.Array, f.Model.SelectedFormOperations, ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
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
                new ExcelParameter("A1",string.Format("Сводный график отпусков на {1} г. (по подразделению № {0})",f.Model.CodeSubdiv, f.Model.DateBegin.Value.Year)),
                new ExcelParameter(new System.Drawing.Point(5,3),string.Format("Отпуска на {0} г. (к.д.)",f.Model.DateBegin.Value.Year)),
                new ExcelParameter(new System.Drawing.Point(2,6+t.Rows.Count),new System.Drawing.Point(7,6+t.Rows.Count),"Руководитель подразделения _____________________________")});
            }
        }

        /// <summary>
        /// Отчет по группе мастера отпуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtVSByGroupMaster_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm f = new VacPeriodForm(new DateTime(CurrentVacYear, 1, 1), new DateTime(CurrentVacYear, 12, 31), CurrentVacSubdivID, true, false);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/Rep_VS_ByGroupMaster.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.Model.SubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date, f.Model.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date, f.Model.DateEnd, ParameterDirection.Input);
                a.SelectCommand.BindByName = true;
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных", a, a.SelectCommand,
                    (p, pw) =>
                    {
                        ReportViewerWindow.ShowReport("Отпуска по группе мастера", "Rep_VacByGroupMaster.rdlc", (pw.Result as DataSet).Tables[0],
                        new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("P_CODE_SUBDIV", string.IsNullOrEmpty(f.Model.CodeSubdiv)?"У-УАЗ":f.Model.CodeSubdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE1", f.Model.DateBegin.Value.ToShortDateString()),
                            new Microsoft.Reporting.WinForms.ReportParameter("P_DATE2", f.Model.DateBegin.Value.ToShortDateString())}.ToList());
                    });
            }
        }

        /// <summary>
        /// Фактические отпуска по группе бюро участку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btActualVacByRegion_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectActualVacByRegion.sql"), Connect.Schema), Connect.CurConnect);
                DataTable t = new DataTable();
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, CurrentVacSubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(CurrentVacYear, 1, 1), ParameterDirection.Input);
                a.SelectCommand.BindByName = true;
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных...", a, a.SelectCommand,
                    (p, pw) =>
                    {
                        ReportViewerWindow.ShowReport("Фактические отпуска по участкам", "ActualVacByRegions.rdlc", t,
                            new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("CodeSubdiv", CurrentVacCodeSubdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("Year",CurrentVacYear.ToString())}.ToList());
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex));
            }
        }

        /// <summary>
        /// Сводный отчет по участкам подразделения. Заказал 17 цех
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btConsolidVacByRegion_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectConsolidVacsByRegions.sql"), Connect.Schema), Connect.CurConnect);
            DataTable t = new DataTable();
            a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, CurrentVacSubdivID, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, new DateTime(CurrentVacYear, 1, 1), ParameterDirection.Input);
            a.SelectCommand.BindByName = true;

            AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных...", a, a.SelectCommand,
                (p, pw) =>
                {
                    ReportViewerWindow.ShowReport("Сводный отчет по участкам подразделения", "RepConsolidVacByRegion.rdlc", t,
                new Microsoft.Reporting.WinForms.ReportParameter[]{ new Microsoft.Reporting.WinForms.ReportParameter("CodeSubdiv", CurrentVacCodeSubdiv),
                            new Microsoft.Reporting.WinForms.ReportParameter("Year", CurrentVacYear.ToString())}.ToList());
                });
        }

        /// <summary>
        /// Печать отчета по отстатка периодов сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtVS_RemainVacs_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            decimal k = 18;
            if (NumericInput.ShowPromt(Window.GetWindow(this), "Установка параметра отчета", "Укажите кол-во месяцев заработанного периода отпуска", ref k, 0))
            {
                OracleDataAdapter oda = new OracleDataAdapter(String.Format(Queries.GetQuery(@"Go\Rep_RemainPeriodsVac.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                oda.SelectCommand.BindByName = true;
                oda.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, CurrentVacSubdivID, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_months", OracleDbType.Decimal, k, ParameterDirection.Input);
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных...", oda, oda.SelectCommand,
                    (p, pw) =>
                {
                    ReportViewerWindow.ShowReport("Неиспользованные периоды отпуска", "Rep_RemainPeriodVacs.rdlc", (pw.Result as DataSet).Tables[0],
                        new ReportParameter[] { new ReportParameter("P_CODE_SUBDIV", CurrentVacCodeSubdiv) });
                });
            }
        }

        /// <summary>
        /// Формирование отчет по людям которым надо заблокировать пропуска для оптуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtListVsBlock_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime next_month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
            VacPeriodForm f = new VacPeriodForm(next_month,
                    new DateTime(next_month.Year, next_month.Month, DateTime.DaysInMonth(next_month.Year, next_month.Month)), CurrentVacSubdivID,
                    true, true);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                OracleDataAdapter oda = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go/Rep_ListBlockEmpVS.sql"), Connect.Schema, Connect.SchemaSalary), Connect.CurConnect);
                oda.SelectCommand.BindByName = true;
                oda.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, f.Model.DateBegin, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, f.Model.DateEnd, ParameterDirection.Input);
                oda.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.Model.SubdivID, ParameterDirection.Input);
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Получение данных...", oda, oda.SelectCommand,
                    (p, pw) =>
                    {
                        ReportViewerWindow.ShowReport("Список закрытых отпусков для сотрудников", "Rep_ListBlockEmpVS.rdlc",
                            (pw.Result as DataSet).Tables[0], new ReportParameter[]{
                        new ReportParameter("P_DATE1", f.Model.DateBegin.Value.ToShortDateString()),
                        new ReportParameter("P_DATE2", f.Model.DateEnd.Value.ToShortDateString()),
                        new ReportParameter("P_CODE_SUBDIV", f.Model.CodeSubdiv)});
                    });
                
            }
        }

        private void R_btPrikazZavodVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm fGet = new VacPeriodForm(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1), CurrentVacSubdivID);
            fGet.Model.IsDegreeEnabled = true;
            fGet.Owner = Window.GetWindow(this);
            if (fGet.ShowDialog() == true && fGet.SelectedVacIDs.Count > 0)
            {
                DataTable t = new DataTable();
                new OracleDataAdapter(string.Format("select vac_consist_id from {0}.vac_consist where vac_sched_id in ({1})", Connect.Schema, string.Join(",", fGet.SelectedVacIDs.ToArray())), Connect.CurConnect).Fill(t);
                ViewCard.PrintOrderPlantReport(fGet.Model.SubdivID, string.Join(",", t.Rows.OfType<DataRow>().Select(r => r[0].ToString()).ToArray()));
            }
        }

        /// <summary>
        /// Формирвоание приказа по подразделению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btSubPrikazVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm frmGetPer = new VacPeriodForm(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1), CurrentVacSubdivID);
            SignesRecord[] s_pos = new SignesRecord[] { };
            frmGetPer.Model.IsDegreeEnabled = true;
            frmGetPer.Owner = Window.GetWindow(this);
            if (frmGetPer.ShowDialog() == true && frmGetPer.SelectedVacIDs.Count > 0 && Signes.Show(frmGetPer.Model.SubdivID, "OrderPlantSubdivVS", "Введите должность и ФИО ответственного лица (для подписи)", 1, ref s_pos, this) == true)
            {
                try
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(string.Format(LibraryKadr.Queries.GetQuery("GO/OrderSubdivReport.sql"), Connect.Schema, string.Join(",", frmGetPer.SelectedVacIDs.ToArray())), Connect.CurConnect);
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("subd_id", frmGetPer.Model.SubdivID);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    ReportViewerWindow.ShowReport("Распоряжение по поразделению", "Rep_VSOrderSubdiv.rdlc", table, new ReportParameter[] { new ReportParameter("P_POS_NAME", s_pos[0].PosName),
                            new ReportParameter("P_FIO", s_pos[0].EmpName), new ReportParameter("P_CODE_SUBDIV", frmGetPer.Model.CodeSubdiv) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка формирования отчета");
                }
            }
        }

        private void TsbtRNoteVacVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm f = new VacPeriodForm(DateTime.Today, DateTime.Today.AddMonths(1), CurrentVacSubdivID);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                ViewCard.RepVacNotification(f.Model.SubdivID??0, f.SelectedVacIDs.ToArray());
            }
        }


        /// <summary>
        /// Формирование записок-расчет по указанным отпускам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtNoteAccountVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime current_month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            VacPeriodForm f = new VacPeriodForm(current_month, new DateTime(current_month.Year, current_month.Month, DateTime.DaysInMonth(current_month.Year, current_month.Month)), CurrentVacSubdivID,
                    false, true);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                ViewCard.NoteAccountReport(f.Model.SubdivID, f.SelectedVacIDs.ToArray());
            }
        }

        /// <summary>
        /// Фактические отпуска по группе бюро участку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_btActualDatesByPeriod_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm f = new VacPeriodForm(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1), CurrentVacSubdivID, true, true);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(Queries.GetQueryWithSchema("go/R_btActualDateByPeriod.sql"), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, f.Model.SubdivID, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_begin", OracleDbType.Date, f.Model.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date_end", OracleDbType.Date, f.Model.DateEnd, ParameterDirection.Input);
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Формирование данных отчета...", a, a.SelectCommand,
                    (p, pw) =>
                    {
                        ReportViewerWindow.RenderToExcelWPF(this, "ActualVacDaysByPeriod.rdlc", new DataTable[] { (pw.Result as DataSet).Tables[0] }, 
                            new ReportParameter[] {
                                    new ReportParameter("CODE_SUBDIV", f.Model.CodeSubdiv),
                                    new ReportParameter("P_DATE1", f.Model.DateBegin.Value.ToShortDateString()),
                                    new ReportParameter("P_DATE2", f.Model.DateBegin.Value.ToShortDateString()) });
                    });
            }
        }

        /// <summary>
        /// Отчет по плановым дням
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_CountPlanSumDaysVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm frm = new VacPeriodForm(new DateTime(DateTime.Today.Year, 1, 1).AddYears(1), new DateTime(DateTime.Today.Year, 1, 1).AddYears(2), CurrentVacSubdivID, true, false);
            frm.Owner = Window.GetWindow(this);
            if (frm.ShowDialog() == true)
            {
                DataTable t = new DataTable();
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery("go/R_CountPlanSumDaysVS.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_date1", OracleDbType.Date, frm.Model.DateBegin, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_date2", OracleDbType.Date, frm.Model.DateEnd, ParameterDirection.Input);
                a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, frm.Model.SubdivID, ParameterDirection.Input);
                a.Fill(t);
                Excel.PrintWithBorder(true, "R_CountPlanSumDaysVS.xlt", "A4", new DataTable[] { t }, new ExcelParameter[]
                {
                    new ExcelParameter("A1",string.Format("Отчет по планируемым отпускам в период с {0} по {1}",frm.Model.DateBegin.Value.ToShortDateString(),frm.Model.DateEnd.Value.ToShortDateString()))
                });
            }
        }

        /// <summary>
        /// Резерв отпусков. Оценочные обязательства. Сразу в эксель редендриться
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAggReservDataPeriodVS_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            VacPeriodForm f = new VacPeriodForm(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), CurrentVacSubdivID, true, true);
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\R_btSvondVSByMonths_new.sql"), Connect.Schema), Connect.CurConnect);
                a.SelectCommand.BindByName = true;
                a.SelectCommand.Parameters.Add("p_subdiv_id", f.Model.SubdivID);
                a.SelectCommand.Parameters.Add("p_date1", f.Model.DateBegin);
                a.SelectCommand.Parameters.Add("p_date2", f.Model.DateEnd);
                a.SelectCommand.Parameters.Add("t", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(this, "Загрузка данных...", a, a.SelectCommand,
                    (p, pw) =>
                    {
                        ReportViewerWindow.RenderToExcelWPF(this, "RepProvisionVacByMonth.rdlc", new DataTable[] { (pw.Result as DataSet).Tables[0] }, null);
                    }
                );
            }
        }
    }
}
