using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr.Ras
{
    public partial class AvgViewData : Form
    {
        private OracleCommand cmd = new OracleCommand(string.Format("begin {0}.PAYMENT.CLND_AVG_CALC_PER_NUM(:p_per_num,:p_sign_comb,:p_calc_date,:count_months, :t1,:t2,:t3,:t4);end;", Connect.Schema), Connect.CurConnect);
        DataSet ds = new DataSet();
        public AvgViewData()
        {
            InitializeComponent();
            this.date_calc.Value= DateTime.Now.Date;
            cmd.BindByName = true;
            cmd.Parameters.Add("p_per_num", per_num.Text.Trim());
            cmd.Parameters.Add("p_sign_comb", (sign_comb.Checked ? 1m : decimal.Zero));
            cmd.Parameters.Add("p_calc_date", date_calc.Value.Date);
            cmd.Parameters.Add("count_months", int.Parse(cnt_months.SelectedItem==null?"0":string.Format("{0}",cnt_months.SelectedItem)));
            cmd.Parameters.Add("t1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("t2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("t3", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("t4", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cnt_months.SelectedIndex = 1;
            DataTable t= new DataTable();
            new OracleDataAdapter(string.Format("select * from {0}.TYPE_AVG_SAL",Connect.Schema),Connect.CurConnect).Fill(t);
            type_avg.DataSource = t;
            type_avg.DisplayMember = "TYPE_NAME_AVG";
            type_avg.ValueMember = "TYPE_AVG_ID";
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            ds.Reset();
            switch((type_avg.SelectedValue==null?"":type_avg.SelectedValue.ToString()))
            {
                case "1":
                            cmd.Parameters["p_per_num"].Value = per_num.Text.Trim();
                            cmd.Parameters["p_sign_comb"].Value = (sign_comb.Checked ? 1m : decimal.Zero);
                            cmd.Parameters["p_calc_date"].Value = date_calc.Value.Date;
                            cmd.Parameters["count_months"].Value = int.Parse(cnt_months.SelectedItem==null ? "0" : cnt_months.SelectedItem.ToString());
                            try
                            {
                                new OracleDataAdapter(cmd).Fill(ds);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        break;
                case "2": break;
            }
            try
            {
                
                tabs.TabPages.Clear();
                for (int i = 0; i < ds.Tables.Count; ++i)
                {
                    tabs.TabPages.Add(i.ToString(), i.ToString());
                    TabPage tp = tabs.TabPages[i.ToString()];
                    DataGridView d = new DataGridView();
                    d.ReadOnly = true;
                    d.AlternatingRowsDefaultCellStyle.BackColor = d.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.LightGray;
                    d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    d.BackgroundColor = Color.White;
                    d.AllowUserToAddRows = false;
                    d.AllowUserToDeleteRows = false;
                    if (ds.Tables[i].Columns.Contains("ШИФР"))
                        d.DataSource = new DataView(ds.Tables[i], "", "ШИФР", DataViewRowState.OriginalRows);
                    else 
                        d.DataSource = ds.Tables[i];
                    d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    d.ColumnAdded += new DataGridViewColumnEventHandler(d_ColumnAdded);                    
                    d.Dock = DockStyle.Fill;
                    tp.Controls.Add(d);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка рассчетов " + ex.Message, "АРМ Бухгалтера");
            }
        }

        void d_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name.ToUpper()=="COLOR")
            {
                (sender as DataGridView).Columns["COLOR"].Visible = false;
                (sender as DataGridView).CellFormatting += new DataGridViewCellFormattingEventHandler(d_CellFormatting);
            }
            /*if (e.Column.Name.ToUpper() == "ШИФР")
                e.Column.Frozen = true;*/
        }

        void d_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DataGridView d = (sender as DataGridView);
                try
                {
                    d.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml((d.Rows[e.RowIndex].DataBoundItem as DataRowView).Row["COLOR"].ToString());
                }
                catch
                {
                    d.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void per_num_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btRefresh_Click(this, null);
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            List<ExcelPrintTable> l = new List<ExcelPrintTable>();
            int s =0;
            for (int i=0;i<ds.Tables.Count;++i)
            {
                l.Add(new ExcelPrintTable(ds.Tables[i],Excel.AddRows((i>0?"A3":"B3"),s),true));
                s+=ds.Tables[i].Rows.Count+2;
            }
            Excel.PrintWithBorder(true, "AVGViewCalculationRAS.xlt", l.ToArray(), new ExcelParameter[] { new ExcelParameter("A1", string.Format("Начисления и расчет средней дневной стоимости работника {0}", per_num.Text)) },
                new TotalRowsStyle[]{new TotalRowsStyle("COLOR",Color.White,Color.White,"")});
        }

        private void tsbtShortPrintAVGCalend_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet d = new DataSet();
                OracleCommand cmd = new OracleCommand(string.Format("begin {0}.PAYMENT.SHORT_NOTE_AVG_CALEND(:p_per_num, :p_sign_comb, :p_calc_date, :t1,:t2,:t3,:t4,:t5); end;", Connect.Schema), Connect.CurConnect);
                cmd.BindByName = true;
                cmd.Parameters.Add("p_per_num", per_num.Text.Trim());
                cmd.Parameters.Add("p_sign_comb", (sign_comb.Checked ? 1m : 0m));
                cmd.Parameters.Add("p_calc_date", date_calc.Value.Date);
                cmd.Parameters.Add("t1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("t2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("t3", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("t4", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("t5", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                new OracleDataAdapter(cmd).Fill(d);
                Excel.PrintWithBorder(false,"AvgNoteCalcCalendarShort.xlt", new ExcelPrintTable[]{
                        new ExcelPrintTable(d.Tables[0],"A5", true),
                        new ExcelPrintTable(d.Tables[1],Excel.AddRows("A5",Math.Max(d.Tables[0].Rows.Count,d.Tables[2].Rows.Count)+2), true),
                        new ExcelPrintTable(d.Tables[2],"E5", true),
                        new ExcelPrintTable(d.Tables[3],Excel.AddRows("E5",Math.Max(d.Tables[0].Rows.Count,d.Tables[2].Rows.Count)+2), true)},
                        new ExcelParameter[]{new ExcelParameter("B2", d.Tables[4].Rows[0]["FIO"].ToString()),
                            new ExcelParameter("B2", d.Tables[4].Rows[0]["FIO"].ToString()),
                            new ExcelParameter("F2", d.Tables[4].Rows[0]["PER_NUM"].ToString()),
                            new ExcelParameter("H2", d.Tables[4].Rows[0]["CODE_SUBDIV"].ToString()),
                            new ExcelParameter("B3", d.Tables[4].Rows[0]["CODE_ORDER"].ToString()),
                            new ExcelParameter("F3", d.Tables[4].Rows[0]["CODE_DEGREE"].ToString())});
                        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
