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
    public partial class Gr_Work_Day : Form
    {
        GR_WORK_seq gr_work;
        OracleDataTable dtAccessSubdiv, dtGr_Work_Day, dtTime_Interval, dtAccess_Template;
        int gr_work_id;
        bool flagAdd;
        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_flagAdd">Флаг добавления данных</param>
        /// <param name="_gr_work_id">Идентификатор графика работы</param>
        public Gr_Work_Day(bool _flagAdd, int _gr_work_id)
        {
            InitializeComponent();
            gr_work_id = _gr_work_id;
            flagAdd = _flagAdd;
            gr_work = new GR_WORK_seq(Connect.CurConnect);
            /// Если добавление данных
            if (flagAdd)
            {
                /// Добавляем новую запись и переписываем ID
                gr_work.AddNew();
                gr_work_id = (int)((GR_WORK_obj)(((CurrencyManager)BindingContext[gr_work]).Current)).GR_WORK_ID;
            }
            else
            {
                /// Заполняем по ключу
                gr_work.Fill(string.Format("where {0} = {1}", GR_WORK_seq.ColumnsName.GR_WORK_ID, _gr_work_id));
            }
            tbGr_Work_Name.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.GR_WORK_NAME);
            mbCount_Day.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.COUNT_DAY);
            mbHours_For_Norm.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.HOURS_FOR_NORM);
            mbHours_For_Graph.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.HOURS_FOR_GRAPH);
            mbHours_For_Norm_106.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.HOURS_NORM_CALENDAR);
            chSign_compact_day_work.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_COMPACT_DAY_WORK);
            chSign_holiday_work.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_HOLIDAY_WORK);
            chSign_Floating.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_FLOATING);
            chSign_Summarize.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_SUMMARIZE);
            mbHours_Dinner.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.HOURS_DINNER);
            deDate_End_Graph.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.DATE_END_GRAPH);
            chSign_Shorten.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_SHORTEN);
            chSign_Shiftman.AddBindingSource(gr_work, GR_WORK_seq.ColumnsName.SIGN_SHIFTMAN);

            mbHours_For_Norm.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            mbHours_Dinner.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            
            dtTime_Interval = new OracleDataTable("", Connect.CurConnect);
            dtTime_Interval.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SelectTime_Interval.sql"), Connect.Schema);
            dtTime_Interval.SelectCommand.Parameters.Add("P_GR_WORK_DAY_ID", OracleDbType.Decimal).Value =
                -1;
            dtTime_Interval.Fill();
            dgTime_Interval.DataSource = dtTime_Interval;
            //dgTime_Interval.Columns[0].Width = 40;
            dgGr_Work_Day.SelectionChanged += new EventHandler(dgGr_Work_Day_SelectionChanged);
            
            /// Таблица
            dtGr_Work_Day = new OracleDataTable(
                string.Format(Queries.GetQuery("Table/SelectGR_WORK_DAY.sql"), Connect.Schema), Connect.CurConnect);
            dtGr_Work_Day.SelectCommand.Parameters.Add("p_gr_work_id", OracleDbType.Decimal).Value = gr_work_id;
            GetGrWork_Day();
            RefreshGrWorkDay();

            dgGr_Work_Day.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            dtAccessSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtAccessSubdiv.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Access_SubdivGR.sql"),
                DataSourceScheme.SchemeName);
            dtAccessSubdiv.SelectCommand.Parameters.Add("p_gr_work_id", gr_work_id);            
            RefreshAccessSubdiv();
            dgAccessSubdiv.DataSource = dtAccessSubdiv;
            dgAccessSubdiv.Columns["subdiv_id"].Visible = false;

            //dtAccess_Template = new OracleDataTable("", Connect.CurConnect);
            //dtAccess_Template.SelectCommand.CommandText = string.Format("SELECT * FROM {0}.ACCESS_TEMPLATE ORDER BY ACCESS_TEMPLATE_NAME",
            //    Connect.Schema);
            //dtAccess_Template.Fill();
            //cbACCESS_TEMPLATE.DataSource = dtAccess_Template;
            //cbACCESS_TEMPLATE.DisplayMember = "ACCESS_TEMPLATE_NAME";
            //cbACCESS_TEMPLATE.ValueMember = "ACCESS_TEMPLATE_ID";
            //cbACCESS_TEMPLATE.DataBindings.Add("SelectedValue", gr_work, "ACCESS_TEMPLATE_ID", true, DataSourceUpdateMode.OnPropertyChanged, null);

            foreach (DataGridViewColumn col in dgAccessSubdiv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if (!GrantedRoles.GetGrantedRole("TABLE_EDIT_GR"))
            {
                tsbAddGR_Work_Day.Enabled = false;
                tsbEditGR_Work_Day.Enabled = false;
                tsbDeleteGR_Work_Day.Enabled = false;
                tsbAccessSubdiv.Enabled = false;
                btSave.Enabled = false;
            }
            else
            {
                tsbAddGR_Work_Day.Enabled = true;
                tsbEditGR_Work_Day.Enabled = true;
                tsbDeleteGR_Work_Day.Enabled = true;
                tsbAccessSubdiv.Enabled = true;
                btSave.Enabled = true;
            }            
        }
                
        /// <summary>
        /// Добавление нового графика работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            /// Если не пусто, то сохраняем
            if (tbGr_Work_Name.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели наименование графика работы!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (mbCount_Day.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели количество дней в графике!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            gr_work.Save();
            Connect.Commit();
            EditGr_Work_Day editGrWorkDay = new EditGr_Work_Day(true, gr_work_id, 0);
            editGrWorkDay.Text = "Добавление данных дня графика работы";
            editGrWorkDay.ShowDialog();
            GetGrWork_Day();            
        }

        /// <summary>
        /// Редактирование графика работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (dgGr_Work_Day.Rows.Count != 0 && tsbEditGR_Work_Day.Enabled == true)
            {
                /// Идентификатор редактируемого дня графика работы
                int gr_work_day_id = Convert.ToInt32(dgGr_Work_Day.CurrentRow.Cells["gr_work_day_id"].Value);
                EditGr_Work_Day editGrWorkDay = new EditGr_Work_Day(false, gr_work_id, gr_work_day_id);
                editGrWorkDay.Text = "Редактирование данных дня графика работы";
                editGrWorkDay.ShowDialog();
                GetGrWork_Day();
            }
        }

        /// <summary>
        /// Удаление графика работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgGr_Work_Day.Rows.Count != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {                    
                    /// Идентификатор удаляемого дня графика работы
                    int gr_work_day_id = Convert.ToInt32(dgGr_Work_Day.CurrentRow.Cells["gr_work_day_id"].Value);
                    /// Создаем и заполняем таблицу дня графика работы
                    GR_WORK_DAY_seq gr_work_day = new GR_WORK_DAY_seq(Connect.CurConnect);
                    gr_work_day.Fill(string.Format("where {0} = {1}", GR_WORK_DAY_seq.ColumnsName.GR_WORK_DAY_ID,
                        gr_work_day_id));
                    /// Удаляем
                    gr_work_day.Remove((GR_WORK_DAY_obj)((CurrencyManager)BindingContext[gr_work_day]).Current);
                    gr_work_day.Save();
                    Connect.Commit();
                    GetGrWork_Day();
                }
            }
        }

        /// <summary>
        /// Метод заполняет грид дней графика работы
        /// </summary>
        void RefreshGrWorkDay()
        {             
            dgGr_Work_Day.DataSource = dtGr_Work_Day;
            dgGr_Work_Day.Columns["gr_work_day_id"].Visible = false;
            dgGr_Work_Day.Columns["gr_work_id"].Visible = false;
            dgGr_Work_Day.Columns["MIN_TIME_BEGIN"].Visible = false;
            dgGr_Work_Day.Columns["MAX_TIME_END"].Visible = false;
            dgGr_Work_Day.Columns["FOR_NEXT_DAY"].Visible = false;
            dgGr_Work_Day.Columns["NUMBER_DAY"].Visible = false;
            foreach (DataGridViewColumn col in dgGr_Work_Day.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        void GetGrWork_Day()
        {
            dtGr_Work_Day.Clear();
            dtGr_Work_Day.Fill();
        }

        void RefreshTime_Interval()
        {
            dtTime_Interval.Clear();
            dtTime_Interval.Fill();
        }

        void RefreshAccessSubdiv()
        {
            dtAccessSubdiv.Clear();
            dtAccessSubdiv.Fill();
        }
        /// <summary>
        /// Сохранение данных в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            /// Если не пусто, то сохраняем
            if (tbGr_Work_Name.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели наименование графика работы!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbGr_Work_Name.Focus();
                return;
            }
            if (mbCount_Day.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели количество дней в графике!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbCount_Day.Focus();
                return;
            }
            if (mbHours_For_Norm.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Вы не ввели количество часов по норме!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbHours_For_Norm.Focus();
                return;
            }
            /* 22.04.2016 - отказались от работы с графиками в Перко, так как они никак не влияют на пропуск сотрудника на предприятие...
             * Все время было потрачено зря...
            List<PercoXML.Graph_Work_Day> listGraph_Days = new List<PercoXML.Graph_Work_Day>();
            foreach (DataRow row in dtGr_Work_Day.Rows)
            {
                listGraph_Days.Add(new PercoXML.Graph_Work_Day(row["NUMBER_DAY"].ToString(), row["MIN_TIME_BEGIN"].ToString(),
                        row["MAX_TIME_END"].ToString(), row["FOR_NEXT_DAY"].ToString()));
            }
            
            // Если добавление данных
            if (flagAdd)
            {
                if (Kadr.Authorization.graphs_Work.InsertGraph_Work(
                    new PercoXML.Graph_Work(
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr_work]).Current).GR_WORK_ID.ToString(),
                        "",
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr_work]).Current).GR_WORK_NAME,
                        "removable", DateTime.Today.ToShortDateString(), "false",
                        "false", "00:30", "00:30", "00:30", "00:30",
                        listGraph_Days)) == false)
                    return;
            }
            else
            {
                if (Kadr.Authorization.graphs_Work.UpdateGraph_Work(
                    new PercoXML.Graph_Work(
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr_work]).Current).GR_WORK_ID.ToString(),
                        "",
                        ((GR_WORK_obj)((CurrencyManager)BindingContext[gr_work]).Current).GR_WORK_NAME,
                        "removable", DateTime.Today.ToShortDateString(), "false",
                        "false", "00:30", "00:30", "00:30", "00:30",
                        listGraph_Days)) == false)
                    return;
            }*/
            gr_work.Save();
            Connect.Commit();
            Close();
        }

        /// <summary>
        /// Выход из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbAdd_Subdiv_Click(object sender, EventArgs e)
        {
            Access_Gr_Subdiv access_gr_subdiv = new Access_Gr_Subdiv(gr_work_id);
            access_gr_subdiv.ShowInTaskbar = false;
            if (access_gr_subdiv.ShowDialog() == DialogResult.OK)
            {
                RefreshAccessSubdiv();
            }
        }

        private void dgGr_Work_Day_SelectionChanged(object sender, EventArgs e)
        {
            if (dgGr_Work_Day.CurrentRow != null)
            {
                dtTime_Interval.SelectCommand.Parameters["P_GR_WORK_DAY_ID"].Value =
                    dgGr_Work_Day.CurrentRow.Cells["gr_work_day_id"].Value;
                RefreshTime_Interval();
            }
        }
    }
}
