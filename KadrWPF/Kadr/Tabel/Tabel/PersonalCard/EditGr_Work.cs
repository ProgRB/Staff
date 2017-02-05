using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Staff;
using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class EditGr_Work : Form
    {
        OracleDataTable dtGr_work, dtGr_Work_day, dtHistoryGraph;
        public int gr_work_id;
        public EditGr_Work(int _subdiv_id, string _per_num, decimal _transfer_id)
        {
            InitializeComponent();
            dtGr_work = new OracleDataTable("", Connect.CurConnect);
            dtGr_work.SelectCommand.CommandText = 
                string.Format("select GW.GR_WORK_ID, GW.GR_WORK_NAME \"Наименование графика работы\", GW.DATE_END_GRAPH \"Окончание графика\" " + 
                "from {0}.GR_WORK GW " + 
                "where GW.gr_work_id in (select gr_work_id from {0}.ACCESS_GR_WORK AG where AG.subdiv_id = :p_subdiv_id) " +
                "and NVL(GW.DATE_END_GRAPH,DATE '3000-01-01') >= NVL(:p_DATE_END_GRAPH,DATE '1000-01-01') " +
                "order by GR_WORK_NAME", Connect.Schema);
            dtGr_work.SelectCommand.BindByName = true;
            dtGr_work.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal).Value = _subdiv_id;
            dtGr_work.SelectCommand.Parameters.Add("p_DATE_END_GRAPH", OracleDbType.Date);
            dtGr_work.Fill();            
            dtGr_Work_day = new OracleDataTable("", Connect.CurConnect);
            dtGr_Work_day.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectTime_Zone.sql"),
                Connect.Schema);
            dtGr_Work_day.SelectCommand.BindByName = true;
            dtGr_Work_day.SelectCommand.Parameters.Add("GR_WORK_ID", OracleDbType.Decimal).Value = 0;
            dtGr_Work_day.Fill();

            dgGR_Work.DataSource = dtGr_work;
            dgGR_Work.Columns["GR_WORK_ID"].Visible = false;

            cbGr_work_day_num.DataSource = dtGr_Work_day;
            cbGr_work_day_num.DisplayMember = "TIME_ZONE_NAME";
            cbGr_work_day_num.ValueMember = "NUMBER_DAY";

            dtHistoryGraph = new OracleDataTable("", Connect.CurConnect);
            dtHistoryGraph.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHistoryGraph.sql"),
                Connect.Schema);
            dtHistoryGraph.SelectCommand.Parameters.Add("p_per_num", _per_num);
            dtHistoryGraph.SelectCommand.Parameters.Add("p_transfer_id", _transfer_id);
            dtHistoryGraph.Fill();
            dgHistoryGraph.DataSource = dtHistoryGraph.DefaultView;
            dgHistoryGraph.Columns["EMP_GR_WORK_ID"].Visible = false;
            dgHistoryGraph.Columns["YEAR_GR_WORK"].Visible = false;
            foreach (DataGridViewColumn column in dgHistoryGraph.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            panel1.EnableByRules();
            dtpDate_End_Graph.Value = DateTime.Today;

            nudYearFilterHistoryGraph.ValueChanged += new EventHandler(nudYearFilterHistoryGraph_ValueChanged); 
            nudYearFilterHistoryGraph.Value = DateTime.Today.Year;
            chFilterToYearHistoryGraph.CheckedChanged += new EventHandler(chFilterToYearHistoryGraph_CheckedChanged);  
        }

        private void btSaveGR_Click(object sender, EventArgs e)
        {
            if (dgGR_Work.CurrentRow == null)
            {
                MessageBox.Show("Вы не выбрали график работы!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (deGr_work_date_begin.Date == null)
            {
                MessageBox.Show("Вы не ввели дату начала работы сотрудника по графику!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (deGr_work_date_begin.Date > Table.dCloseTable || GrantedRoles.GetGrantedRole("TABLE_EDIT_OLD"))
            {
                if (deGr_work_date_begin.Date <= Table.dCloseTable)
                {
                    if (MessageBox.Show("Данный график за прошедший период!\nВы действительно хотите продолжить?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                gr_work_id = Convert.ToInt32(dgGR_Work.CurrentRow.Cells["GR_WORK_ID"].Value);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Нельзя устанавливать графики за прошедший период!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }                

        /// <summary>
        /// Удаление графика работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteGR_Work_Click(object sender, EventArgs e)
        {
            if (Table.dCloseTable < (Convert.ToDateTime(dgHistoryGraph.CurrentRow.Cells["Дата установки"].Value)) || GrantedRoles.GetGrantedRole("TABLE_EDIT_OLD"))
            {
                if (Table.dCloseTable >= (Convert.ToDateTime(dgHistoryGraph.CurrentRow.Cells["Дата установки"].Value)))
                {
                    if (MessageBox.Show("Данный график за прошедший период!\nВы действительно хотите продолжить?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                if (MessageBox.Show("Удалить график работы?", "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    OracleCommand ocDeleteGr_Work = new OracleCommand("", Connect.CurConnect);
                    ocDeleteGr_Work.BindByName = true;
                    ocDeleteGr_Work.CommandText = string.Format(
                        "BEGIN {0}.EMP_GR_WORK_DELETE(:p_EMP_GR_WORK_ID); END;",
                        DataSourceScheme.SchemeName);
                    ocDeleteGr_Work.Parameters.Add("p_EMP_GR_WORK_ID",
                        dgHistoryGraph.CurrentRow.Cells["EMP_GR_WORK_ID"].Value);
                    ocDeleteGr_Work.ExecuteNonQuery();
                    Connect.Commit();
                    dgHistoryGraph.Rows.Remove(dgHistoryGraph.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалять графики за прошедший период!", 
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void dgGR_Work_SelectionChanged(object sender, EventArgs e)
        {
            if (dgGR_Work.CurrentRow != null)
            {
                gr_work_id = Convert.ToInt32(dgGR_Work.CurrentRow.Cells["GR_WORK_ID"].Value);
                dtGr_Work_day.Clear();
                dtGr_Work_day.SelectCommand.Parameters[0].Value = gr_work_id;
                dtGr_Work_day.Fill();
            }
        }

        private void dgGR_Work_DoubleClick(object sender, EventArgs e)
        {
            if (dgGR_Work.CurrentRow != null)
            {
                Gr_Work_Day gr_work_day = new Gr_Work_Day(false, gr_work_id);
                gr_work_day.Text = "Просмотр информации по графику работы";
                gr_work_day.ShowInTaskbar = false;
                gr_work_day.ShowDialog();
            }
        }

        private void dtpDate_End_Graph_ValueChanged(object sender, EventArgs e)
        {
            dtGr_work.Clear();
            if (dtpDate_End_Graph.Checked)
                dtGr_work.SelectCommand.Parameters["p_DATE_END_GRAPH"].Value = dtpDate_End_Graph.Value;
            else
                dtGr_work.SelectCommand.Parameters["p_DATE_END_GRAPH"].Value = null;
            dtGr_work.Fill();
        }

        /// <summary>
        /// Событие изменения года для фильтра отгулов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nudYearFilterHistoryGraph_ValueChanged(object sender, EventArgs e)
        {
            FilterHistoryGraph();
        }

        /// <summary>
        /// Событие изменения признака фильтра отгулов по году
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chFilterToYearHistoryGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (chFilterToYearHistoryGraph.Checked)
            {
                dtHistoryGraph.DefaultView.RowFilter = "";
            }
            else
            {
                dtHistoryGraph.DefaultView.RowFilter = "YEAR_GR_WORK = " + nudYearFilterHistoryGraph.Value.ToString();
            }
            nudYearFilterHistoryGraph.Enabled = !chFilterToYearHistoryGraph.Checked;
        }

        /// <summary>
        /// Фильтрация отгулов по годам
        /// </summary>
        /// <param name="_year_begin"></param>
        /// <param name="_year_end"></param>
        void FilterHistoryGraph()
        {
            dtHistoryGraph.DefaultView.RowFilter = "YEAR_GR_WORK = " + nudYearFilterHistoryGraph.Value.ToString();
        }   
    }
}
