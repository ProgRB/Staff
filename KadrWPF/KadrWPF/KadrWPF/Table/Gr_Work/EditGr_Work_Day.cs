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

namespace Tabel
{
    public partial class EditGr_Work_Day : Form
    {
        TIME_ZONE_seq time_zone;
        TIME_ZONE_seq time_zoneHol;
        TIME_ZONE_seq time_zoneCom;
        GR_WORK_DAY_seq gr_work_day;
        bool flagAdd;
        int gr_work_id, gr_work_day_id;
        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_flagAdd">Флаг добавления данных</param>
        /// <param name="_gr_work_id">Идентификатор графика работы</param>
        /// <param name="_gr_work_day_id">Номер дня графика работы</param>
        public EditGr_Work_Day(bool _flagAdd, int _gr_work_id, int _gr_work_day_id)
        {
            InitializeComponent();
            time_zone = new TIME_ZONE_seq(Connect.CurConnect);
            time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
            flagAdd = _flagAdd;
            gr_work_id = _gr_work_id;
            gr_work_day_id = _gr_work_day_id;            
            /// Если добавление
            if (flagAdd)
            {
                /// Даем доступ для ввода дня
                mbNumber_Day.Enabled = true;
                /// Создаем таблицу дней графика работы и добавляем новую запись
                gr_work_day = new GR_WORK_DAY_seq(Connect.CurConnect);
                gr_work_day.AddNew();
            }
            else
            {
                /// Создаем таблицу и заполняем ее редактируемым днем
                gr_work_day = new GR_WORK_DAY_seq(Connect.CurConnect);
                gr_work_day.Fill(string.Format("where {0} = {1}", GR_WORK_DAY_seq.ColumnsName.GR_WORK_DAY_ID,
                    gr_work_day_id));
                /// Закрываем выбор дня
                mbNumber_Day.Enabled = false;
                ///// Устанавливаем нужную позицию в списке
                //cbDayOfWeek.SelectedIndex = (int)((GR_WORK_DAY_obj)(((CurrencyManager)BindingContext[gr_work_day]).Current)).NUMBER_DAY - 1;
            }
            mbNumber_Day.AddBindingSource(gr_work_day, GR_WORK_DAY_seq.ColumnsName.NUMBER_DAY);
            cbTime_Zone.AddBindingSource(gr_work_day, GR_WORK_DAY_seq.ColumnsName.TIME_ZONE_ID,
                new LinkArgument(time_zone, TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME));

            time_zoneHol = new TIME_ZONE_seq(Connect.CurConnect);
            time_zoneHol.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
            time_zoneCom = new TIME_ZONE_seq(Connect.CurConnect);
            time_zoneCom.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
            cbHoliday_time_zone_id.DataSource = time_zoneHol;
            cbHoliday_time_zone_id.DisplayMember = TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME.ToString();
            cbHoliday_time_zone_id.ValueMember = TIME_ZONE_seq.ColumnsName.TIME_ZONE_ID.ToString();
            cbHoliday_time_zone_id.DataBindings.Add("SelectedValue", gr_work_day,
                GR_WORK_DAY_seq.ColumnsName.HOLIDAY_TIME_ZONE_ID.ToString(), true, DataSourceUpdateMode.OnValidation, null);
            cbCompact_time_zone_id.DataSource = time_zoneCom;
            cbCompact_time_zone_id.DisplayMember = TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME.ToString();
            cbCompact_time_zone_id.ValueMember = TIME_ZONE_seq.ColumnsName.TIME_ZONE_ID.ToString();
            cbCompact_time_zone_id.DataBindings.Add("SelectedValue", gr_work_day,
                GR_WORK_DAY_seq.ColumnsName.COMPACT_TIME_ZONE_ID.ToString(), true, DataSourceUpdateMode.OnValidation, null);
            chSign_Evening_Time.AddBindingSource(gr_work_day, GR_WORK_DAY_seq.ColumnsName.SIGN_EVENING_TIME);

            /// При изменении текста комбобокса
            cbTime_Zone.TextChanged += new EventHandler(cbTime_Zone_TextChanged);
            cbHoliday_time_zone_id.TextChanged += new EventHandler(cbTime_Zone_TextChanged);
            cbCompact_time_zone_id.TextChanged += new EventHandler(cbTime_Zone_TextChanged);

            /// События нажатия кнопки
            btTime_Zone.Click += new EventHandler(btTime_Zone_Click);
            btHoliday_Time_Zone.Click += new EventHandler(btTime_Zone_Click);
            btCompact_Time_Zone.Click += new EventHandler(btTime_Zone_Click);
        }

        /// <summary>
        /// Событие изменения текста комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbTime_Zone_TextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "")
            {
                ((ComboBox)sender).SelectedItem = null;
            }
        }

        ///// <summary>
        ///// Событие изменения текста комбобокса
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cbHoliday_time_zone_id_TextChanged(object sender, EventArgs e)
        //{
        //    if (cbHoliday_time_zone_id.Text.ToString() == "")
        //    {
        //        cbHoliday_time_zone_id.SelectedItem = null;
        //    }
        //}

        ///// <summary>
        ///// Событие изменения текста комбобокса
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cbCompact_time_zone_id_TextChanged(object sender, EventArgs e)
        //{
        //    if (cbCompact_time_zone_id.Text.ToString() == "")
        //    {
        //        cbCompact_time_zone_id.SelectedItem = null;
        //    }
        //}

        /// <summary>
        /// Сохранение данных в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            /// Если добавление
            if (flagAdd)
            {
                /// Создаем таблицу дней графика работы
                GR_WORK_DAY_seq gr = new GR_WORK_DAY_seq(Connect.CurConnect);
                /// Заполняем ее выбранным днем графика работы
                //gr.Fill(string.Format("where {0} = {1} and {2} = {3}", 
                //    GR_WORK_DAY_seq.ColumnsName.GR_WORK_ID, gr_work_id,
                //    GR_WORK_DAY_seq.ColumnsName.NUMBER_DAY, cbDayOfWeek.SelectedIndex + 1));
                gr.Fill(string.Format("where {0} = {1} and {2} = {3}",
                    GR_WORK_DAY_seq.ColumnsName.GR_WORK_ID, gr_work_id,
                    GR_WORK_DAY_seq.ColumnsName.NUMBER_DAY, mbNumber_Day.Text.Trim()));
                /// Если на данных день графика данные отсутствуют
                if (gr.Count == 0)
                {
                    /// Заполняем нужные поля
                    //((GR_WORK_DAY_obj)(((CurrencyManager)BindingContext[gr_work_day]).Current)).NUMBER_DAY =
                    //    cbDayOfWeek.SelectedIndex + 1;
                    ((GR_WORK_DAY_obj)(((CurrencyManager)BindingContext[gr_work_day]).Current)).GR_WORK_ID =
                        gr_work_id;
                }
                else
                {
                    /// Выводим сообщение и возвращаемся в форму добавления
                    MessageBox.Show("На данный день данные уже сохранены в базе данных!", "Внимание", 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                GR_WORK_seq gr_work = new GR_WORK_seq(Connect.CurConnect);
                gr_work.Fill("where gr_work_id = " + gr_work_id);
                if (((GR_WORK_DAY_obj)(((CurrencyManager)BindingContext[gr_work_day]).Current)).NUMBER_DAY >
                    ((GR_WORK_obj)(((CurrencyManager)BindingContext[gr_work]).Current)).COUNT_DAY)
                {
                    MessageBox.Show("Вы ввели неверный номер дня графика!\n" +
                    "Значение не может быть больше " +
                    ((GR_WORK_obj)(((CurrencyManager)BindingContext[gr_work]).Current)).COUNT_DAY.Value.ToString()
                    + " для данного графика", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            /// Сохраняем
            gr_work_day.Save();
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

        /// <summary>
        /// Событие нажатия кнопки изменения временной зоны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btTime_Zone_Click(object sender, EventArgs e)
        {
            Time_Zone formTime_zone = new Time_Zone();
            formTime_zone.Text = "Временные зоны";
            formTime_zone.btSelectTime_Zone.Visible = true;
            if (formTime_zone.ShowDialog() == DialogResult.Yes)
            {
                if (((Button)sender).Tag.ToString() == "Holiday")
                {
                    time_zoneHol.Clear();
                    time_zoneHol.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
                    cbHoliday_time_zone_id.SelectedValue = formTime_zone.Time_Zone_ID;
                    ((GR_WORK_DAY_obj)((CurrencyManager)BindingContext[gr_work_day]).Current).HOLIDAY_TIME_ZONE_ID =
                        formTime_zone.Time_Zone_ID;
                }
                else
                    if (((Button)sender).Tag.ToString() == "Compact")
                    {
                        time_zoneCom.Clear();
                        time_zoneCom.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
                        cbCompact_time_zone_id.SelectedValue = formTime_zone.Time_Zone_ID;
                        ((GR_WORK_DAY_obj)((CurrencyManager)BindingContext[gr_work_day]).Current).COMPACT_TIME_ZONE_ID =
                            formTime_zone.Time_Zone_ID;
                    }
                    else
                    {
                        time_zone.Clear();
                        time_zone.Fill("order by " + TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
                        cbTime_Zone.SelectedValue = formTime_zone.Time_Zone_ID;
                        ((GR_WORK_DAY_obj)((CurrencyManager)BindingContext[gr_work_day]).Current).TIME_ZONE_ID =
                            formTime_zone.Time_Zone_ID;
                    }
            }
        }
    }
}
