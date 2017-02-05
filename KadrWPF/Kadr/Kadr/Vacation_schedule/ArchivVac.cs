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

namespace Kadr.Vacation_schedule
{
    public partial class ArchivVac : Form
    {
        DataSet ds = new DataSet();
        OracleDataAdapter ad_emps, ad_vacs;
        public ArchivVac()
        {
            InitializeComponent();
            ds.Tables.Add("Emps");
            ds.Tables.Add("Vacs");
            ad_emps = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\FillArchivEmps.sql"), Connect.Schema), Connect.CurConnect);
            ad_emps.SelectCommand.BindByName = true;
            ad_emps.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            ad_emps.SelectCommand.Parameters.Add("p_year", OracleDbType.Date);
            ad_emps.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            ad_vacs = new OracleDataAdapter(string.Format(Queries.GetQuery(@"go\GetEmpVacForMakeGrid.sql"), Connect.Schema), Connect.CurConnect);
            ad_vacs.SelectCommand.BindByName = true;
            ad_vacs.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            dgArchivVS.CurrentCellChanged += new EventHandler(dgArchivVS_CurrentCellChanged);
            dgArchivVS.CellDoubleClick += new DataGridViewCellEventHandler(dgArchivVS_CellDoubleClick);
            dgArchivVS.AutoGenerateColumns = false;
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("code_subdiv", "Подр.", "code_subdiv"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("fio", "ФИО", "fio"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("per_num", "Таб.№", "per_num"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("sign_comb", "Совм.", "sign_comb"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("pos_name", "Должность", "pos_name"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("code_degree", "Категория", "code_degree"));
            dgArchivVS.Columns.Add(new MDataGridViewTextBoxColumn("group_master_name", "Группа мастера", "group_master_name"));
            dgArchivVS.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            dgVacsArchivEmp.AutoGenerateColumns = false;
            dgVacsArchivEmp.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            dgVacsArchivEmp.CellFormatting += new DataGridViewCellFormattingEventHandler(dgVacsArchivEmp_CellFormatting);
            this.Load += new EventHandler(ArchivVac_Load);
            FilterVS.FilterChanged +=new EventHandler(FillEmps);
            dgArchivVS.DataSource = new DataView(ds.Tables["emps"], "", "", DataViewRowState.CurrentRows);
            dgVacsArchivEmp.DataSource = new DataView(ds.Tables["vacs"], "", "", DataViewRowState.CurrentRows);
        }

        void dgVacsArchivEmp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dg = sender as DataGridView;
            if (dg.Columns[e.ColumnIndex].Name.ToUpper() == "СТАТУС")
                e.CellStyle.BackColor = e.CellStyle.SelectionForeColor = Color.LightGray;                
        }

        void ArchivVac_Load(object sender, EventArgs e)
        {
            FillEmps(this, null);
            ColumnWidthSaver.FillWidthOfColumn(dgArchivVS);
            ColumnWidthSaver.FillWidthOfColumn(dgVacsArchivEmp);
            
        }

        void dgArchivVS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tsbtViewCard_Click(this, null);
        }

        private void FillEmps(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate() { FillArhivEmps(FilterVS.YearVS, FilterVS.subdiv_id, FilterVS.per_num); });
        }
        void dgArchivVS_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgArchivVS.CurrentRow != null)
            {
                this.BeginInvoke((MethodInvoker)delegate() { FillVacs((decimal)(dgArchivVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"]); });
            }
        }
        private void FillArhivEmps(decimal year, decimal subdiv_id, string per_num)
        {
            int k=0;
            k = dgArchivVS.BindingContext[ds.Tables["Emps"]].Position;
            ds.Tables["Emps"].Clear();
            ad_emps.SelectCommand.Parameters["p_subdiv_id"].Value = subdiv_id;
            ad_emps.SelectCommand.Parameters["p_year"].Value = new DateTime((int)year,1,1);
            ad_emps.SelectCommand.Parameters["p_per_num"].Value = per_num;
            ad_emps.Fill(ds.Tables["Emps"]);
            if (k > -1 && k < ds.Tables["emps"].Rows.Count)
                dgArchivVS.BindingContext[ds.Tables["emps"]].Position = k;
        }
        private void FillVacs(decimal p_transfer_id)
        {
            ds.Tables["vacs"].Clear();
            ad_vacs.SelectCommand.Parameters["p_transfer_id"].Value = p_transfer_id;
            ad_vacs.Fill(ds.Tables["vacs"]);
            if (dgVacsArchivEmp.Columns.Contains("close_sign"))
                dgVacsArchivEmp.Columns["close_sign"].Visible = false;
            if (dgVacsArchivEmp.Columns.Contains("vac_id"))
                dgVacsArchivEmp.Columns["vac_id"].Visible = false;
        }

        private void tsbtViewCard_Click(object sender, EventArgs e)
        {
            if (dgArchivVS.CurrentRow != null)
            {
                ViewCard f = new ViewCard((dgArchivVS.CurrentRow.DataBoundItem as DataRowView)["transfer_id"].ToString(), null);
                f.ShowDialog(this);
            }
        }
    }
}
