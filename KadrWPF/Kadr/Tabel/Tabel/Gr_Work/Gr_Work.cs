using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class Gr_Work : Form
    {
        OracleDataTable dtGR_Work;
        BindingSource bsGr_Work;
        OracleDataTable dtAccessSubdiv;
        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="typeForm">Заголовок формы</param>
        public Gr_Work()
        {
            InitializeComponent();
            dtGR_Work = new OracleDataTable("", Connect.CurConnect);
            dtGR_Work.SelectCommand.CommandText =
                string.Format(Queries.GetQuery(@"Table\SelectGR_WORK.sql"), DataSourceScheme.SchemeName);
            dtGR_Work.SelectCommand.BindByName = true;
            dtGR_Work.SelectCommand.Parameters.Add("p_sub", "");
            dtGR_Work.SelectCommand.Parameters.Add("p_DATE_END_GRAPH", OracleDbType.Date);
            dtGR_Work.Fill();
            dtAccessSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtAccessSubdiv.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Access_SubdivGR.sql"),
                DataSourceScheme.SchemeName);
            dtAccessSubdiv.SelectCommand.Parameters.Add("p_gr_work_id", OracleDbType.Decimal, 0, ParameterDirection.Input);
            dtAccessSubdiv.Fill();
            dgAccessSubdiv.DataSource = dtAccessSubdiv;            
            dgAccessSubdiv.Columns["subdiv_id"].Visible = false;
            dgAccessSubdiv.Columns[0].Width = 50;
            dgAccessSubdiv.Columns[1].Width = 350;
            foreach (DataGridViewColumn col in dgAccessSubdiv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            bsGr_Work = new BindingSource();
            bsGr_Work.DataSource = dtGR_Work;
            dgGR_Work.DataSource = bsGr_Work;
            dgGR_Work.Columns["GR_WORK_ID"].Visible = false;
            dgGR_Work.Columns["Подр."].Width = 50;
            dgGR_Work.Columns["Наименование графика работы"].Width = 320;
            dgGR_Work.Columns["Дней в графике"].Width = 70;
            dgGR_Work.Columns["Работа в вых. день"].Width = 60;
            dgGR_Work.Columns["Работа в сокр. день"].Width = 60;
            dgGR_Work.Columns["Плав. график"].Width = 60;
            dgGR_Work.Columns["Сокр. график (246)"].Width = 60;
            dgGR_Work.Columns["Сменщики"].Width = 80;
            dgGR_Work.Columns["Норма часов (для 102)"].Width = 75;
            dgGR_Work.Columns["Норма часов (для 111)"].Width = 75;
            dgGR_Work.Columns["Норма часов (для 106/124)"].Width = 75;
            dgGR_Work.Columns["Расчетные часы по графику"].Width = 80;
            dgGR_Work.Columns["Суммир-ый учет"].Width = 80;
            dgGR_Work.Columns["Обед. пер. при суммир. учете"].Width = 70;
            dgGR_Work.Columns["Расчет вечернего времени"].Width = 75;
            dgGR_Work.Columns["Дата окончания графика"].Width = 80;
            //dgGR_Work.Columns["ACCESS_TEMPLATE_ID"].Visible = false;
            //dgGR_Work.Columns["ACCESS_TEMPLATE_NAME"].Width = 280;
            //dgGR_Work.Columns["ACCESS_TEMPLATE_NAME"].HeaderText = "Наименование шаблона доступа";
            dgGR_Work.ColumnHeadersHeight = 80;
            dgGR_Work.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_GR"))
            {
                tsbAdd_Gr.Enabled = false;
                tsbEdit_Gr.Enabled = false;
                tsbDelete_Gr.Enabled = false;
            }
            else
            {
                tsbAdd_Gr.Enabled = true;
                tsbEdit_Gr.Enabled = true;
                tsbDelete_Gr.Enabled = true;
            }
            dtpDate_End_Graph.DateTimeControl.Checked = false;
            dtpDate_End_Graph.DateTimeControl.Format = DateTimePickerFormat.Short;
            dtpDate_End_Graph.DateTimeControl.ShowCheckBox = true;
            dtpDate_End_Graph.DateTimeControl.Value = DateTime.Today;
            dtpDate_End_Graph.DateTimeControl.Size = new Size(100, 26);
            dtpDate_End_Graph.Size = new Size(106, 26);
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            Gr_Work_Day grWorkDay = new Gr_Work_Day(true, 0);
            grWorkDay.Text = "Добавление нового графика работы";
            grWorkDay.ShowInTaskbar = false;
            grWorkDay.ShowDialog();
            /// Сохраняем текущую позицию
            try
            {
                int pos = bsGr_Work.Position;
                dtGR_Work.Clear();
                dtGR_Work.Fill();
                /// Восстанавливаем позицию            
                bsGr_Work.Position = pos;
            }
            catch { }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (dgGR_Work.CurrentRow != null)
            {
                /// Идентификатор редактируемого графика работы
                int gr_work_id = Convert.ToInt32(dgGR_Work.CurrentRow.Cells["GR_WORK_ID"].Value);
                Gr_Work_Day grWorkDay = new Gr_Work_Day(false, gr_work_id);
                grWorkDay.Text = "Редактирование графика работы";                    
                grWorkDay.ShowInTaskbar = false;
                grWorkDay.ShowDialog();
                /// Сохраняем текущую позицию
                int pos = bsGr_Work.Position;
                dtGR_Work.Clear();
                dtGR_Work.Fill();
                /// Восстанавливаем позицию
                bsGr_Work.Position = pos;
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgGR_Work.Rows.Count != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    /// Идентификатор удаляемого графика работы
                    int gr_work_id = Convert.ToInt32(dgGR_Work.CurrentRow.Cells["gr_work_id"].Value);
                    OracleCommand com = new OracleCommand("", Connect.CurConnect);
                    com.BindByName = true;
                    com.CommandText = string.Format(
                        "select count(*) from {0}.EMP_GR_WORK where GR_WORK_ID = :p_gr_work_id",
                        DataSourceScheme.SchemeName);
                    com.Parameters.Add(new OracleParameter("p_gr_work_id", gr_work_id));
                    int countGr = Convert.ToInt32(com.ExecuteScalar());
                    if (countGr > 0)
                    {
                        MessageBox.Show("Невозможно удалить запись, потому что график присвоен сотрудникам завода.",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }                    
                    /// Создаем и заполняем таблицу графика работы
                    GR_WORK_seq gr = new GR_WORK_seq(Connect.CurConnect);
                    gr.Fill(string.Format("where {0} = {1}", GR_WORK_seq.ColumnsName.GR_WORK_ID, gr_work_id));
                    /* 22.04.2016 - отказались от работы с графиками в Перко, так как они никак не влияют на пропуск сотрудника на предприятие...
                     * Все время было потрачено зря...
                    if (Kadr.Authorization.graphs_Work.DeleteGraph_Work(
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr]).Current).GR_WORK_ID.Value.ToString(),
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr]).Current).GR_WORK_NAME) == false)
                    {
                        return;
                    }
                     */

                    /// Удаляем
                    gr.Remove((GR_WORK_obj)((CurrencyManager)BindingContext[gr]).Current);
                    gr.Save();
                    Connect.Commit();
                    try
                    {
                        /// Сохраняем текущую позицию
                        int pos = bsGr_Work.Position;
                        dtGR_Work.Clear();
                        dtGR_Work.Fill();
                        /// Восстанавливаем позицию
                        bsGr_Work.Position = pos;
                    }
                    catch { }
                }
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgGR_Work_SelectionChanged(object sender, EventArgs e)
        {
            if (dgGR_Work.CurrentRow != null)
            {
                dtAccessSubdiv.Clear();
                dtAccessSubdiv.SelectCommand.Parameters["p_gr_work_id"].Value =
                    Convert.ToInt32(dgGR_Work.CurrentRow.Cells["GR_WORK_ID"].Value);
                dtAccessSubdiv.Fill();
            }
        }

        private void tstCode_Subdiv_TextChanged(object sender, EventArgs e)
        {
            dtGR_Work.Clear();
            dtGR_Work.SelectCommand.Parameters["p_sub"].Value = tstCode_Subdiv.Text.Trim();
            dtGR_Work.Fill();
        }

        /// <summary>
        /// Показать временные зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTime_Zone_Click(object sender, EventArgs e)
        {
            Time_Zone time_zone = new Time_Zone();            
            time_zone.Text = "Временные зоны";
            time_zone.ShowDialog();
        }

        private void toolStripDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dtGR_Work.Clear();
            if (dtpDate_End_Graph.DateTimeControl.Checked)
                dtGR_Work.SelectCommand.Parameters["p_DATE_END_GRAPH"].Value = dtpDate_End_Graph.DateTimeControl.Value;
            else
                dtGR_Work.SelectCommand.Parameters["p_DATE_END_GRAPH"].Value = null;
            dtGR_Work.Fill();   
        }

    }
}
