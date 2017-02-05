using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using MExcel= Microsoft.Office.Interop.Excel;
using Staff;
using Oracle.DataAccess.Client;
using MReport=Microsoft.Reporting.WinForms;

namespace Kadr.Shtat
{
    public partial class ReportsReplEmp : Form, INotifyPropertyChanged
    {
        private TypeReportRepl _typ;
        public ReportsReplEmp(TypeReportRepl typ, decimal? subdiv_id, string SubRule) : this(typ, subdiv_id, null, SubRule) { }
        public ReportsReplEmp(TypeReportRepl typ,decimal? subdiv_id,DateTime? StartDate, string SubRule, bool isCombine = true)
        {
            CombineIndex = isCombine?1:0;
            _typ = typ;
            InitializeComponent();
            sub_sel_repl.subdiv_id = subdiv_id;
            sub_sel_repl.ByRule = SubRule;
            sub_sel_repl.SubdivChanged += new EventHandler(ReportServiceRecordRepl_Load);
            GridReplReports.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            dateStart.Value = StartDate??new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dateEnd.Value = dateStart.Value.AddMonths(1);
            tsCBTypeList.ComboBox.DataBindings.Add("SelectedIndex", this, "CombineIndex");
        }
        public void ReportServiceRecordRepl_Load(object sender, EventArgs e)
        {
            OracleDataAdapter adapt = new OracleDataAdapter(string.Format(Queries.GetQuery(@"new\GetReplDataByPeriod.sql"),
                DataSourceScheme.SchemeName),Connect.CurConnect);
            adapt.SelectCommand.BindByName = true;
            adapt.SelectCommand.Parameters.Add("p_dateBegin",dateStart.Value);
            adapt.SelectCommand.Parameters.Add("p_dateEnd",dateEnd.Value);
            adapt.SelectCommand.Parameters.Add("p_subdiv_id",sub_sel_repl.subdiv_id);
            DataTable table = new DataTable();
            table.Columns.Add("check", typeof(decimal)).DefaultValue = 0m;
            adapt.Fill(table);
            GridReplReports.AutoGenerateColumns = false;
            GridReplReports.DataSource = new DataView(table, string.Join(" and ", new string[] { tscbOnlyNotClose.Checked ? "SIGN_LOCK_REPL=0" : "", "SIGN_COMBINE=" + tsCBTypeList.SelectedIndex }.Where(r => !String.IsNullOrEmpty(r))), 
                                                        "", DataViewRowState.CurrentRows);
            GridReplReports.Columns.Clear();
            GridReplReports.Columns.Add(new DataGridViewCheckBoxColumn());
            GridReplReports.Columns[0].Name = "CHECK";
            GridReplReports.Columns[0].HeaderText = "";
            GridReplReports.Columns[0].DataPropertyName = "check";
            for (int i = 0; i < table.Columns.Count; i++)
                if (table.Columns[i].ColumnName.ToUpper()!="SIGN_LOCK_REPL" && table.Columns[i].ColumnName.ToUpper()!="REPL_EMP_ID"
                    && table.Columns[i].ColumnName.ToUpper() != "CHECK" && table.Columns[i].ColumnName.ToUpper() != "SIGN_COMBINE")
                {
                    int p =GridReplReports.Columns.Add(table.Columns[i].Caption, table.Columns[i].Caption);
                    GridReplReports.Columns[p].ReadOnly = true;
                    GridReplReports.Columns[p].DataPropertyName = table.Columns[i].ColumnName;
                }
            table.RowChanged += new DataRowChangeEventHandler(table_RowChanged);
            Settings.SetDataGridCaption(ref GridReplReports);
            ColumnWidthSaver.FillWidthOfColumn(GridReplReports);
            
        }

        private int _combIndex=-1;
        public int CombineIndex
        {
            get
            {
                return _combIndex;
            }
            set
            {
                _combIndex = value;
                OnPropertyChanged("CombineIndex");
            }
        }

        void table_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (Source.Count>0)
            {
                tsLabelCountRows.Text = string.Format("Выбрано {0} из {1} записей", Source.OfType<DataRowView>().Count(t => t["check"]!=DBNull.Value && (decimal)t["check"] == 1m), Source.Count);
            }
            else
                tsLabelCountRows.Text = "Выбрано 0 из 0 записей";
            //e.Row
        }

        private DataView Source
        {
            get
            {
                return GridReplReports.DataSource as DataView;
            }
        }

        private void btFormReport_Click(object sender, EventArgs e)
        {
            
            GridReplReports.CommitEdit(DataGridViewDataErrorContexts.Commit);
            List<string> l = Source.OfType<DataRowView>().Where(p => p["check"] != DBNull.Value && (decimal)p["check"] == 1m).Select(b => b["REPL_EMP_ID"].ToString()).ToList();
           if (l.Count > 0)
               switch (_typ)
            {
                case TypeReportRepl.ServiseNote:
                    {
                        string[][] sp = new string [][]{};
                        if (Vacation_schedule.Signes.Show(this, sub_sel_repl.subdiv_id, "ServiseNoteReplEmp", "Ввод должности и ФИО для подписи", 2, ref sp) == System.Windows.Forms.DialogResult.OK)
                        {
                            DataTable tsignes = new DataTable();
                            tsignes.Columns.Add("POS_NAME");
                            tsignes.Columns.Add("FIO");
                            for (int i = 0; i < sp.Length;++i)
                                tsignes.Rows.Add(sp[i][0], sp[i][1]);
                            string s = string.Join(",", l.ToArray());
                            OracleDataAdapter adapt = new OracleDataAdapter(string.Format(Queries.GetQuery(@"new\R_btServiseNoteReplShtat.sql"), DataSourceScheme.SchemeName, s), Connect.CurConnect);
                            DataTable table = new DataTable();
                            adapt.Fill(table);
                            ReportViewerWindow.ShowReport("Служебная записка на совмещение/замещение", "Rep_ServiceNoteReplEmp.rdlc", new DataTable[]{table, tsignes} ,
                                new MReport.ReportParameter[]{ new MReport.ReportParameter("P_CODE_SUBDIV", sub_sel_repl.SubdivName),
                                        new MReport.ReportParameter("P_SIGN_COMBINE", CombineIndex.ToString())}, System.Drawing.Printing.Duplex.Default, new string[]{});

                           /* Excel.PrintWithBorder(GrantedRoles.GetGrantedRole("STAFF_REPORT_EDITING"), "ServiceNoteReplEmp.xlt", "A9", new DataTable[] { table }, new ExcelParameter[]{
                                new ExcelParameter("A1",string.Format("ПОДРАЗДЕЛЕНИЕ {0}",sub_sel_repl.SubdivName)),
                                new ExcelParameter("A5",string.Format("Прошу   оплатить совмещение на основании приказов: {0}",sub_sel_repl.SubdivName)),
                                new ExcelParameter(MExcel.XlHAlign.xlHAlignLeft, new Point(2, table.Rows.Count+10), new Point(6, table.Rows.Count+10), string.Format("{0} ________________ {1}", sp[0][0], sp[0][1])),
                                new ExcelParameter(MExcel.XlHAlign.xlHAlignLeft, new Point(2, table.Rows.Count+12), new Point(6, table.Rows.Count+12), string.Format("{0} ________________ {1}", sp[1][0], sp[1][1]))
                                });*/
                        }
                    }; break;
                case TypeReportRepl.OrderPlant:
                    {
                        string[][] s_pos = new string[][]{};
                        if (Vacation_schedule.Signes.Show(this, sub_sel_repl.subdiv_id, "ReplOrderPlantShtat", "Ввод должности и ФИО на месте подписи", 1, ref s_pos) == DialogResult.OK)
                        {
                            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery("new/R_btReplOrderPlant.SQL"), Connect.Schema), Connect.CurConnect);
                            a.SelectCommand.BindByName = true;
                            a.SelectCommand.Parameters.Add("p_repl_id", OracleDbType.Array, l.Select(r => Convert.ToDecimal(r)).ToArray(), ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                            DataTable t = new DataTable();
                            a.Fill(t);
                            ReportViewerWindow.ShowReport("Приказ по заводу на совмещение/замещение", "Rep_ReplOrderPlant.rdlc", t,
                                new MReport.ReportParameter[]{ new MReport.ReportParameter("P_POS_NAME", s_pos[0][0]),
                                        new MReport.ReportParameter("P_FIO", s_pos[0][1])}, System.Drawing.Printing.Duplex.Default, new string[]{"Word", "Excel"});
                            /*List<ExcelParameter> c = new List<ExcelParameter>();
                            for (int i = 0; i < l.Count; i++)
                            {
                                OracleCommand cmd =new OracleCommand(string.Format(Queries.GetQuery("new/R_btReplOrderPlant.SQL"), DataSourceScheme.SchemeName, l[i]), Connect.CurConnect);
                                cmd.BindByName = true;
                                cmd.Parameters.Add("repl_id", l[i]);
                                OracleDataReader rd = cmd.ExecuteReader();
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 4 + i * 42), new Point(13, 4 + 42 * i), "ОТКРЫТОЕ"));
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 5 + i * 42), new Point(13, 5 + 42 * i), "АКЦИОНЕРНОЕ ОБЩЕСТВО"));
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 6 + i * 42), new Point(13, 6 + 42 * i), "«УЛАН-УДЭНСКИЙ"));
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 7 + i * 42), new Point(13, 7 + 42 * i), "АВИАЦИОННЫЙ ЗАВОД»"));
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 8 + i * 42), new Point(13, 8 + 42 * i), "ПРИКАЗ"));
                                c.Add(new ExcelParameter(MExcel.XlHAlign.xlHAlignRight, new Point(1, 10 + 42 * i), "«"));
                                c.Add(new ExcelParameter(MExcel.XlHAlign.xlHAlignLeft, new Point(4, 10 + 42 * i), "»"));
                                c.Add(new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight, new Point(13, 10 + 42 * i), "№"));
                                c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignRight, new Point(2, 12 + 42 * i), new Point(8, 12 + 42 * i), "г.Улан-Удэ"));
                                c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignLeft, new Point(2, 14 + 42 * i), new Point(8, 14 + 42 * i), "О замещении"));
                                c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignCenter, new Point(15, 30 + 42 * i), new Point(23, 30 + 42 * i), s_pos[0][1]));
                                c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 28 + 42 * i), new Point(12, 32 + 42 * i), @s_pos[0][0]));
                                if (rd.Read())
                                {
                                    c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignJustify, new Point(1, 16 + 42 * i), new Point(23, 18 + 42 * i), rd["part1"].ToString()));
                                    c.Add(new ExcelParameter(new Font("Arial", 14, FontStyle.Bold), MExcel.XlHAlign.xlHAlignCenter, new Point(1, 20 + 42 * i), new Point(6, 20 + 42 * i), "ПРИКАЗЫВАЮ:"));
                                    c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignJustify, new Point(1, 21 + 42 * i), new Point(23, 26 + 42 * i), rd["part2"].ToString()));
                                    if (rd["date_order"].ToString() != "")
                                    {
                                        c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignCenter, new Point(2, 10 + 42 * i), new Point(3, 10 + 42 * i), ""));
                                        c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignCenter, new Point(5, 10 + 42 * i), new Point(8, 10 + 42 * i), Library.MyMonthName(Convert.ToDateTime(rd["date_order"].ToString()))));
                                        c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignLeft, new Point(9, 10 + 42 * i), new Point(11, 10 + 42 * i), Convert.ToDateTime(rd["date_order"].ToString()).Year.ToString() + "г."));
                                    }
                                    c.Add(new ExcelParameter(new Font("Arial", 14), MExcel.XlHAlign.xlHAlignLeft, new Point(14, 10 + 42 * i), new Point(17, 10 + 42 * i), rd["repl_order"].ToString()));
                                }
                            }
                            ExcelParameter[] c1 = c.ToArray();

                            Excel.PrintCellWithMerge(true, "StatReplPrikaz.xlt", c1);*/
                        }
                    }; break;
                case TypeReportRepl.OrderSubdiv:
                    {
                        string st = string.Join(",",l.ToArray());
                        string[][] pos = new string[][] { };
                        if (Vacation_schedule.Signes.Show(this,sub_sel_repl.subdiv_id,"ReplOrderSubdivShtat","Введите ФИО и должности ответственных лиц",1,ref pos) == DialogResult.OK)
                        {
                            DataTable tsignes = new DataTable();
                            tsignes.Columns.Add("POS_NAME");
                            tsignes.Columns.Add("FIO");
                            for (int i = 0; i < pos.Length; ++i)
                                tsignes.Rows.Add(pos[i][0], pos[i][1]);
                            DataSet ds = new DataSet();
                            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"new\R_ReplOrderSubdiv.sql"), Connect.Schema, st), Connect.CurConnect);
                            a.SelectCommand.Parameters.Add("p_repl_id", OracleDbType.Array, l.Select(r => Convert.ToDecimal(r)).ToArray(), ParameterDirection.Input).UdtTypeName = "APSTAFF.TYPE_TABLE_NUMBER";
                            a.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
                            a.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
                            a.SelectCommand.BindByName = true;

                            a.Fill(ds);
                            ReportViewerWindow.ShowReport("Приказ по подразделению на совмещение/замещение", "Rep_ReplOrderSubdiv.rdlc", new DataTable[] { ds.Tables[0], ds.Tables[1], tsignes },
                                new MReport.ReportParameter[]{ new MReport.ReportParameter("P_SUBDIV_NAME", sub_sel_repl.SubdivName),
                                        new MReport.ReportParameter("P_SIGN_COMBINE", CombineIndex.ToString())});
                            /*
                            text = ds.Tables[0].Rows.OfType<DataRow>().Select(t => t[0].ToString()).FirstOrDefault();

                            Excel.PrintWithBorder(GrantedRoles.GetGrantedRole("STAFF_REPORT_EDITING"),"StatReplOrderSubdiv.xlt", "A13",new DataTable[]{ds.Tables[1]}, new ExcelParameter[] {
                                new ExcelParameter("A1",sub_sel_repl.SubdivName),
                                new ExcelParameter("A4","От "+DateTime.Today.ToShortDateString()),
                                new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignJustify,new Point(1,8),text),
                                new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft,new Point(1,14+l.Count),new Point(2,14+l.Count),pos[0][0]),
                                new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft,new Point(4,14+l.Count),new Point(5,14+l.Count),pos[0][1])/*,
                                 new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft,new Point(1,15+l.Count),new Point(2,15+l.Count),pos[1][0]),
                                new ExcelParameter(Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft,new Point(4,15+l.Count),new Point(5,15+l.Count),pos[1][1])*
                            });*/
                        }
                    }; break;
            }
        }

        private void ReportServiceRecordRepl_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain.UnCheckButtonShtat("aaaa");
        }

        private void ReportsReplEmp_Shown(object sender, EventArgs e)
        {
            Elegant.Ui.ScreenTipData sd = new Elegant.Ui.ScreenTipData(btFormReport);
            sd.Caption = "Предупреждение";
            sd.Text = "Если фамилия склоняется не стандартно, добавьте ее в список исключений!";
            STip.SetScreenTip(btFormReport, sd);
        }

        private void btOpenExcept_Click(object sender, EventArgs e)
        {
            PadegException frmExc = new PadegException();
            frmExc.ShowDialog(this);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Source.Count; ++i)
                Source[i]["Check"] = cbCheckAll.Checked?1m:0m;
            GridReplReports.Refresh();
        }

        private void tscbOnlyNotClose_CheckedChanged(object sender, EventArgs e)
        {
            if (Source!=null)
                Source.RowFilter = string.Join(" and ", new string[]{tscbOnlyNotClose.Checked ? "SIGN_LOCK_REPL=0" : "", "SIGN_COMBINE="+tsCBTypeList.SelectedIndex}.Where(r=>!String.IsNullOrEmpty(r)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public enum TypeReportRepl
    {
        ServiseNote=1,
        OrderPlant=2,
        OrderSubdiv=3
    }
}
