using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Kadr;
using Staff;
using LibraryKadr;

namespace Tabel
{
    public partial class Time_Zone : Form
    {
        static DataSet _ds;
        static OracleDataAdapter _daGr_Work;
        TIME_INTERVAL_seq time_interval;
        TIME_ZONE_seq time_zone;
        TYPE_INTERVAL_seq type_interval;
        private decimal? time_zone_id;
        public decimal? Time_Zone_ID
        { 
            get 
            { 
                return time_zone_id; 
            }            
        }
        public Time_Zone()
        {
            InitializeComponent();
            time_zone = new TIME_ZONE_seq(Connect.CurConnect);
            time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
            type_interval = new TYPE_INTERVAL_seq(Connect.CurConnect);
            type_interval.Fill();
            time_interval = new TIME_INTERVAL_seq(Connect.CurConnect);
            if (time_zone.Count != 0)
            {
                time_interval.Fill(string.Format("where {0} = {1}", TIME_INTERVAL_seq.ColumnsName.TIME_ZONE_ID,
                    ((TIME_ZONE_obj)(((CurrencyManager)BindingContext[time_zone]).Current)).TIME_ZONE_ID));
                tsTime_Interval.Enabled = true;
            }
            else
            {
                tsTime_Interval.Enabled = false;
            }
            dgTime_Zone.AddBindingSource(time_zone);
            dgTime_Interval.AddBindingSource(time_interval,
                new LinkArgument(type_interval, TYPE_INTERVAL_seq.ColumnsName.TYPE_INTERVAL_NAME));
            dgTime_Interval.Columns["TIME_BEGIN"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTime_Interval.Columns["TIME_END"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTime_Interval.Columns["TIME_ZONE_ID"].Visible = false;
            dgTime_Zone.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            dgTime_Interval.RowEnter += new DataGridViewCellEventHandler(Library.DataGridView_RowEnter);
            ((CurrencyManager)BindingContext[time_zone]).PositionChanged += new EventHandler(Time_Zone_PositionChanged);
            tsTime_Zone.EnableByRules();              
            tsTime_Interval.EnableByRules();

            dgGr_Work.DataSource = _ds.Tables["GR_WORK"].DefaultView;

            Time_Zone_PositionChanged(null, null);
        }

        static Time_Zone()
        {
            _ds = new DataSet();
            _ds.Tables.Add("GR_WORK");

            _daGr_Work = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/SelectGr_Work_For_Time_Zone.sql"),
                Connect.Schema), Connect.CurConnect);
            _daGr_Work.SelectCommand.BindByName = true;
            _daGr_Work.SelectCommand.Parameters.Add("p_TIME_ZONE_ID", OracleDbType.Decimal);


        }

        /// <summary>
        /// Изменение позиции в таблице временных зон
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Time_Zone_PositionChanged(object sender, EventArgs e)
        {
            _ds.Tables["GR_WORK"].Clear();
            /// Если не пусто
            if (time_zone.Count != 0)
            {
                /// Заполняем таблицу интервалов по текущей временной зоне
                time_interval.Fill(string.Format("where {0} = {1}", TIME_INTERVAL_seq.ColumnsName.TIME_ZONE_ID,
                    ((TIME_ZONE_obj)(((CurrencyManager)BindingContext[time_zone]).Current)).TIME_ZONE_ID));
                /// Активируем панель для работы с временными интервалами
                tsTime_Interval.Enabled = true;
                _daGr_Work.SelectCommand.Parameters["p_TIME_ZONE_ID"].Value = 
                    ((TIME_ZONE_obj)(((CurrencyManager)BindingContext[time_zone]).Current)).TIME_ZONE_ID;
                _daGr_Work.Fill(_ds.Tables["GR_WORK"]);
            }
            else
            {
                /// Деактивируем панель для работы с временными интервалами
                tsTime_Interval.Enabled = false;
            }
        }

        /// <summary>
        /// Кнопка выхода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Добавление новой временной зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddTime_Zone_Click(object sender, EventArgs e)
        {
            EditTime_Zone editTime_Zone = new EditTime_Zone(true, 
                time_zone, ((CurrencyManager)BindingContext[time_zone]).Position);
            editTime_Zone.ShowDialog();
            int pos = ((CurrencyManager)BindingContext[time_zone]).Position;
            time_zone.Clear();
            time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
            ((CurrencyManager)BindingContext[time_zone]).Position = pos;
        }

        /// <summary>
        /// Редактирование временной зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEditTime_Zone_Click(object sender, EventArgs e)
        {
            /// Если записи есть
            if (dgTime_Zone.Rows.Count != 0)
            {
                EditTime_Zone tz = new EditTime_Zone(false,
                    time_zone, ((CurrencyManager)BindingContext[time_zone]).Position);
                tz.ShowDialog();
                /// Сохраняем текущую позицию
                int pos = ((CurrencyManager)BindingContext[time_zone]).Position;
                /// Ощищаем и заполняем таблицу
                time_zone.Clear();
                time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
                /// Устанавливаем старую позицию
                ((CurrencyManager)BindingContext[time_zone]).Position = pos;
            }
        }

        /// <summary>
        /// Удаление временной зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteTime_Zone_Click(object sender, EventArgs e)
        {
            /// Если записи есть
            if (dgTime_Zone.Rows.Count != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    /// Удаляем
                    time_zone.Remove((TIME_ZONE_obj)((CurrencyManager)BindingContext[time_zone]).Current);
                    /// Сохраняем
                    time_zone.Save();
                    Connect.Commit();
                    /// Ощищаем и заполняем таблицу
                    time_zone.Clear();
                    time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
                }
            }
        }

        private void btAddTime_Interval_Click(object sender, EventArgs e)
        {
            EditTime_Interval edittime_interval = new EditTime_Interval(true,
                (int)((TIME_ZONE_obj)(((CurrencyManager)BindingContext[time_zone]).Current)).TIME_ZONE_ID,
                time_interval, type_interval, 0);
            edittime_interval.ShowDialog();
            /// Ощищаем и заполняем таблицу
            Time_Zone_PositionChanged(sender, e);
        }

        private void btEditTime_Interval_Click(object sender, EventArgs e)
        {
            if (dgTime_Interval.RowCount != 0)
            {
                EditTime_Interval edittime_interval = new EditTime_Interval(false,
                    (int)((TIME_ZONE_obj)(((CurrencyManager)BindingContext[time_zone]).Current)).TIME_ZONE_ID,
                    time_interval, type_interval, ((CurrencyManager)BindingContext[time_interval]).Position);
                edittime_interval.ShowDialog();
                /// Ощищаем и заполняем таблицу
                Time_Zone_PositionChanged(sender, e);
            }
        }

        private void btDeleteTime_Interval_Click(object sender, EventArgs e)
        {
            if (dgTime_Interval.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    /// Удаляем
                    time_interval.Remove((TIME_INTERVAL_obj)((CurrencyManager)BindingContext[time_interval]).Current);
                    /// Сохраняем
                    time_interval.Save();
                    Connect.Commit();
                    /// Ощищаем и заполняем таблицу
                    Time_Zone_PositionChanged(sender, e);
                }
            }
        }

        private void btSelectTime_Zone_Click(object sender, EventArgs e)
        {
            time_zone_id = ((TIME_ZONE_obj)((CurrencyManager)BindingContext[time_zone]).Current).TIME_ZONE_ID;
            Close();
        }         
    }
}
