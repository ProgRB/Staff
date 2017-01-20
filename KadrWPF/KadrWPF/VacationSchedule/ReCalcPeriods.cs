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
namespace Kadr.Vacation_schedule
{
    public partial class ReCalcPeriods : Form
    {
        private bool fl = false;
        private static string transfer_id_now = "";
        private DataSet ds = new DataSet();
        private object bitset;
        public static bool ReCalcList(string _transfer_id_now,object vac_group_type_id)
        {
            transfer_id_now = _transfer_id_now;
            ReCalcPeriods r = new ReCalcPeriods(vac_group_type_id);
            r.ShowDialog();
            return r.fl;
        }

        private void Calc_Periods()
        {
            OracleCommand cmd =new OracleCommand(string.Format(Queries.GetQuery(@"go\ReCalcPeriods.sql"),Connect.Schema),Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("transfer_id", transfer_id_now);
            cmd.Parameters.Add("p_vac_group_type_id", bitset);
            cmd.Parameters.Add("p1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataAdapter a = new OracleDataAdapter(cmd);
            a.TableMappings.Add("Table", "data");
            if (ds.Tables.Contains("data"))
                ds.Tables["data"].Rows.Clear();
            a.Fill(ds);
            foreach (DataRow r in tbl.Rows)
                if (!string.IsNullOrEmpty(r["er_message1"].ToString()) || !string.IsNullOrEmpty(r["er_message2"].ToString()) || !string.IsNullOrEmpty(r["er_message3"].ToString()))
                r.RowError = r["er_message1"].ToString()+"\n"+r["er_message2"].ToString()+"\n"+r["er_message3"].ToString();
        }
        private DataTable tbl
        {
            get { return ds.Tables["data"]; }
        }
        private ReCalcPeriods(object vac_group_type_id)
        {
            InitializeComponent();
            gridRecalc.AutoGenerateColumns = false;
            bitset = vac_group_type_id;
            
            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name="fl";
            c.HeaderText="Сохранить";
            c.ReadOnly=false;
            c.DataPropertyName = "fl_check";
            gridRecalc.Columns.Add(c);
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("plan_begin", "Дата отпуска", "plan_begin")); gridRecalc.Columns["plan_begin"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("cnt_d", "Кол-во дней", "cnt_d")); gridRecalc.Columns["cnt_d"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("name_vac", "Тип отпуска", "NAME_VAC")); gridRecalc.Columns["name_vac"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("period_begin", "За период С (старая дата)", "PERIOD_BEGIN")); gridRecalc.Columns["period_begin"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("period_end", "За период ПО (старая дата)", "PERIOD_END")); gridRecalc.Columns["period_end"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("new_begin", "За период С (новая дата)", "NEW_BEGIN")); gridRecalc.Columns["new_begin"].ReadOnly = true;
            gridRecalc.Columns.Add(new MDataGridViewTextBoxColumn("new_end", "За период ПО (новая дата)", "NEW_END")); gridRecalc.Columns["new_end"].ReadOnly = true;

            gridRecalc.Columns["plan_begin"].DefaultCellStyle.Format =
            gridRecalc.Columns["period_begin"].DefaultCellStyle.Format =
            gridRecalc.Columns["period_end"].DefaultCellStyle.Format =
            gridRecalc.Columns["new_begin"].DefaultCellStyle.Format =
            gridRecalc.Columns["new_end"].DefaultCellStyle.Format = "dd/MM/yyyy H:mm:ss";
            for (int i = 0; i < gridRecalc.Columns.Count; i++)
                gridRecalc.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridRecalc.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
            Calc_Periods();
            gridRecalc.DataSource = tbl;
            gridRecalc.RowsAdded += new DataGridViewRowsAddedEventHandler(gridRecalc_RowsAdded);
            ColumnWidthSaver.FillWidthOfColumn(gridRecalc);
        }

        void gridRecalc_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i=e.RowIndex,k=0;k<e.RowCount;++k,++i)
            {
                gridRecalc["PERIOD_BEGIN",i].Style.BackColor=gridRecalc["PERIOD_BEGIN",i].Style.SelectionForeColor =
                    (!string.IsNullOrEmpty((gridRecalc.Rows[i].DataBoundItem as DataRowView)["er_message1"].ToString())? Color.LightCoral : Color.White);
                gridRecalc["PERIOD_END",i].Style.BackColor=gridRecalc["PERIOD_END",i].Style.SelectionForeColor =
                    (!string.IsNullOrEmpty((gridRecalc.Rows[i].DataBoundItem as DataRowView)["er_message2"].ToString())? Color.LightCoral: Color.White);
            }
        }

        private void btSaveRecalcPerVS_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand(string.Format("begin {0}.VAC_UPDATE_PERIOD(:p1,:p2,:p3); end;", Connect.Schema), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p1", OracleDbType.Decimal);
            cmd.Parameters.Add("p2", OracleDbType.Date);
            cmd.Parameters.Add("p3", OracleDbType.Date);
            foreach (DataRow r in tbl.Rows)
                if (!(string.IsNullOrEmpty(r["er_message1"].ToString()) && string.IsNullOrEmpty(r["er_message2"].ToString())))
                {
                    cmd.Parameters["p1"].Value = r["vac_consist_id"];
                    cmd.Parameters["p2"].Value = r["new_begin"];
                    cmd.Parameters["p3"].Value = r["new_end"];
                    cmd.ExecuteNonQuery();
                }
            ToolTip t = new ToolTip();
            t.ToolTipIcon = ToolTipIcon.Info;
            t.UseAnimation = true;
            t.Show("Изменения успешно сохранены!",(IWin32Window)sender, 0, 1, 3000);
            Calc_Periods();
        }

    }

}
