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
using Kadr;
//using LibraryKadr;

namespace Tabel
{
    public partial class Calendar : Form
    {
        Control menuControl;
        CALENDAR_seq calendar;
        TYPE_DAY_seq type_day;
        /// <summary>
        /// Список дней календаря
        /// </summary>
        List<DayOfCalendar> dayOfCalendar;
        /// <summary>
        /// Список дат, на которые перенесение идет за субботу
        /// </summary>
        List<DateTime> listTransSat;
        /// <summary>
        /// Список дат, на которые перенесение идет за воскресенье
        /// </summary>
        List<DateTime> listTransSun;
        /// <summary>
        /// Конструктор формы заполнения календаря
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Calendar()
        {
            InitializeComponent();        
            cbMonth.SelectedIndex = DateTime.Now.Month - 1;         
            
            dayOfCalendar = new List<DayOfCalendar>();
            listTransSat = new List<DateTime>();
            listTransSun = new List<DateTime>();
            cbMonth.SelectedIndexChanged += new EventHandler(cbMonth_SelectedIndexChanged);
            nudYear.ValueChanged += new EventHandler(nudYear_ValueChanged);
            type_day = new TYPE_DAY_seq(Connect.CurConnect);
            type_day.Fill();
            calendar = new CALENDAR_seq(Connect.CurConnect);
            nudYear.Value = DateTime.Now.Year;
            calendar.Fill(string.Format("where extract(year from {0}) = {1} and extract(month from {0}) = {2}",
                CALENDAR_seq.ColumnsName.CALENDAR_DAY, nudYear.Value, cbMonth.SelectedIndex + 1));
            foreach (Control control in pnKalendar.Controls)
            {
                if (control is TextBox)
                {
                    control.ContextMenuStrip = cmCalendar;
                }
            }
            FillCalendar(calendar);  
            //this.FormClosing += new FormClosingEventHandler(FormMain.Closingc);
        }

        /// <summary>
        /// Метод заполняет календарь
        /// </summary>
        /// <param name="calend">Таблица содержит данные календаря</param>
        void FillCalendar(CALENDAR_seq calend)
        {
            dayOfCalendar.Clear();
            listTransSat.Clear();
            listTransSun.Clear();
            int year = (int)nudYear.Value;
            int month = cbMonth.SelectedIndex + 1;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime date = new DateTime(year, month, 1);
            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0)
                dayOfWeek = 7;
            int day = 1;
            /// Если данных за указанный период нет, то календарь заполняется по умолчанию.
            /// Если данные есть, то календарь заполняется по ним.
            /// 42 - число клеток в календаре
            if (calend.Count == 0)
            {
                for (int i = 0; i < 42; i++)
                {
                    if (i < dayOfWeek - 1 || i >= daysInMonth + dayOfWeek - 1)
                    {
                        dayOfCalendar.Add(new DayOfCalendar(i, "", Color.White));
                    }
                    else
                    {
                        date = new DateTime(year, month, day);
                        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dayOfCalendar.Add(new DayOfCalendar(i, (day++).ToString(), Color.LightCoral));
                        }
                        else
                        {
                            dayOfCalendar.Add(new DayOfCalendar(i, (day++).ToString(), Color.WhiteSmoke));
                        }
                    }
                }            
            }
            else
            {
                for (int i = 0; i < 42; i++)
                {
                    if (i < dayOfWeek - 1 || i >= daysInMonth + dayOfWeek - 1)
                    {
                        dayOfCalendar.Add(new DayOfCalendar(i, "", Color.White));
                    }
                    else
                    {
                        /// Тип дня
                        int type_day = (int)calendar.Where(k => k.CALENDAR_DAY.Value.Year == year && 
                            k.CALENDAR_DAY.Value.Month == month && 
                            k.CALENDAR_DAY.Value.Day == day).FirstOrDefault().TYPE_DAY_ID;
                        /// С какого дня переносится выходной
                        int day_trans = calendar.Where(k => k.CALENDAR_DAY.Value.Year == year &&
                            k.CALENDAR_DAY.Value.Month == month &&
                            k.CALENDAR_DAY.Value.Day == day).FirstOrDefault().DAY_TRANS != null ? 
                            (int)calendar.Where(k => k.CALENDAR_DAY.Value.Year == year &&
                            k.CALENDAR_DAY.Value.Month == month &&
                            k.CALENDAR_DAY.Value.Day == day).FirstOrDefault().DAY_TRANS : -1;
                        switch (type_day)
                        {
                            case 1:
                                {
                                    if (day_trans == -1)
                                    {
                                        dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.LightCoral));
                                    }
                                    else
                                        if (day_trans == 1)
                                        {
                                            dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.Yellow));
                                            listTransSat.Add(DateTime.Parse(string.Format("{0}.{1}.{2}",
                                                day.ToString(), (cbMonth.SelectedIndex + 1).ToString(), 
                                                nudYear.Value.ToString())));
                                        }
                                        else
                                        {
                                            dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.Firebrick));
                                            listTransSun.Add(DateTime.Parse(string.Format("{0}.{1}.{2}",
                                                day.ToString(), (cbMonth.SelectedIndex + 1).ToString(), 
                                                nudYear.Value.ToString())));
                                        }
                                    break;
                                }
                            case 2:
                                {
                                    dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.WhiteSmoke));
                                    break;
                                }
                            case 3:
                                {
                                    dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.GreenYellow));
                                    break;
                                }
                            case 4:
                                {
                                    dayOfCalendar.Add(new DayOfCalendar(i, day.ToString(), Color.Red));                                    
                                    break;
                                }
                            default:
                                break;
                        }                          
                        day++;
                    }
                }      
            }
            /// Заполнение календаря
            foreach (Control control in pnKalendar.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = dayOfCalendar.Where(i => i.Number.ToString() == control.Name.Substring(1)).FirstOrDefault().Day;
                    control.BackColor = dayOfCalendar.Where(i => i.Number.ToString() == control.Name.Substring(1)).FirstOrDefault().ColorDay;
                }
            }            
        }

        /// <summary>
        /// Метод пересчитывает нормы рабочего времени
        /// </summary>
        void FillWorkingTime()
        {
            int year = (int)nudYear.Value;
            /// Команда для запроса количества дней (рабочих, сокращенных или праздников)
            OracleCommand command = new OracleCommand();
            command.Connection = Connect.CurConnect;
            command.BindByName = true;
            /// По названию переменных можно понять смысл их содержимого.
            /// Комментарии для каждой излишни.
            int dayInMonth, dayInKvartal, dayInHalfYear, dayInYear, dayWorkInMonth, dayHolInMonth, dayShortInMonth, dayWorkInKvartal, dayWorkInHalfYear, dayWorkInYear, dayHolInKvartal, dayHolInHalfYear, dayHolInYear, dayShortInKvartal, dayShortInHalfYear, dayShortInYear;
            dayInMonth = dayInKvartal = dayInHalfYear = dayInYear = dayWorkInMonth = dayHolInMonth = dayShortInMonth = dayWorkInKvartal = dayWorkInHalfYear = dayWorkInYear = dayHolInKvartal = dayHolInHalfYear = dayHolInYear = dayShortInKvartal = dayShortInHalfYear = dayShortInYear = 0;
            command.CommandText =
                string.Format(Queries.GetQuery("Table/DaysInMonth.sql"), Connect.Schema);
            command.Parameters.Add("p_month", OracleDbType.Int32, 0, "p_month");
            command.Parameters.Add("p_year", OracleDbType.Int32, 0, "p_year").Value = year;
            command.Parameters.Add("p_type_day_id", OracleDbType.Int32, 0, "p_type_day_id");            
            for (int i = 1; i <= 12; i++)
            {
                command.Parameters["p_month"].Value = i;
                command.Parameters["p_type_day_id"].Value = 1;
                dayHolInMonth = Convert.ToInt32(command.ExecuteScalar());
                command.Parameters["p_type_day_id"].Value = 4;
                dayHolInMonth += Convert.ToInt32(command.ExecuteScalar());
                dayHolInKvartal += dayHolInMonth;
                dayHolInHalfYear += dayHolInMonth;
                dayHolInYear += dayHolInMonth;
                command.Parameters["p_type_day_id"].Value = 2;
                dayWorkInMonth = Convert.ToInt32(command.ExecuteScalar());
                dayWorkInKvartal += dayWorkInMonth;
                dayWorkInHalfYear += dayWorkInMonth;
                dayWorkInYear += dayWorkInMonth;
                command.Parameters["p_type_day_id"].Value = 3;
                dayShortInMonth = Convert.ToInt32(command.ExecuteScalar());
                dayShortInKvartal += dayShortInMonth;
                dayShortInHalfYear += dayShortInMonth;
                dayShortInYear += dayShortInMonth;
                gbCalendar.Controls["tbWork" + i].Text = string.Format("{0:####}", dayWorkInMonth + dayShortInMonth);
                gbCalendar.Controls["tbHol" + i].Text = string.Format("{0:####}", dayHolInMonth);
                gbCalendar.Controls["tb40" + i].Text = string.Format("{0:####.#}", (dayWorkInMonth * 8) + (dayShortInMonth * 7));
                gbCalendar.Controls["tb36" + i].Text = string.Format("{0:####.#}", (dayWorkInMonth * 7.2) + (dayShortInMonth * 6.2));
                gbCalendar.Controls["tb24" + i].Text = string.Format("{0:####.#}", (dayWorkInMonth * 4.8) + (dayShortInMonth * 3.8));
                dayInMonth = DateTime.DaysInMonth(year, i);
                gbCalendar.Controls["tbCal" + i].Text = dayInMonth.ToString();
                dayInKvartal += dayInMonth;
                dayInHalfYear += dayInMonth;
                dayInYear += dayInMonth;
                if (i % 3 == 0)
                {
                    gbCalendar.Controls["tbCal" + i / 3 + "kv"].Text = dayInKvartal.ToString();
                    gbCalendar.Controls["tbWork" + i / 3 + "kv"].Text = string.Format("{0:####}", dayWorkInKvartal + dayShortInKvartal);
                    gbCalendar.Controls["tbHol" + i / 3 + "kv"].Text = string.Format("{0:####}", dayHolInKvartal);
                    gbCalendar.Controls["tb40" + i / 3 + "kv"].Text = string.Format("{0:####.#}", (dayWorkInKvartal * 8) + (dayShortInKvartal * 7));
                    gbCalendar.Controls["tb36" + i / 3 + "kv"].Text = string.Format("{0:####.#}", (dayWorkInKvartal * 7.2) + (dayShortInKvartal * 6.2));
                    gbCalendar.Controls["tb24" + i / 3 + "kv"].Text = string.Format("{0:####.#}", (dayWorkInKvartal * 4.8) + (dayShortInKvartal * 3.8));
                    dayInKvartal = dayWorkInKvartal = dayShortInKvartal = dayHolInKvartal = 0;
                }
                if (i % 6 == 0)
                {
                    gbCalendar.Controls["tbCal" + i / 6 + "Year"].Text = dayInHalfYear.ToString();
                    gbCalendar.Controls["tbWork" + i / 6 + "Year"].Text = string.Format("{0:####}", dayWorkInHalfYear + dayShortInHalfYear);
                    gbCalendar.Controls["tbHol" + i / 6 + "Year"].Text = string.Format("{0:####}", dayHolInHalfYear);
                    gbCalendar.Controls["tb40" + i / 6 + "Year"].Text = string.Format("{0:####.#}", (dayWorkInHalfYear * 8) + (dayShortInHalfYear * 7));
                    gbCalendar.Controls["tb36" + i / 6 + "Year"].Text = string.Format("{0:####.#}", (dayWorkInHalfYear * 7.2) + (dayShortInHalfYear * 6.2));
                    gbCalendar.Controls["tb24" + i / 6 + "Year"].Text = string.Format("{0:####.#}", (dayWorkInHalfYear * 4.8) + (dayShortInHalfYear * 3.8));
                    dayInHalfYear = dayWorkInHalfYear = dayShortInHalfYear = dayHolInHalfYear = 0;
                }
            }
            tbCalYear.Text = dayInYear.ToString();
            tbWorkYear.Text = string.Format("{0:####}", dayWorkInYear + dayShortInYear);
            tbHolYear.Text = string.Format("{0:####}", dayHolInYear);
            tb40Year.Text = string.Format("{0:####.#}", (dayWorkInYear * 8) + (dayShortInYear * 7));
            tb36Year.Text = string.Format("{0:####.#}", (dayWorkInYear * 7.2) + (dayShortInYear * 6.2));
            tb24Year.Text = string.Format("{0:####.#}", (dayWorkInYear * 4.8) + (dayShortInYear * 3.8));
        }

        /// <summary>
        /// Событие изменение года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudYear_ValueChanged(object sender, EventArgs e)
        {
            lbYear.Text = nudYear.Value.ToString() + " год";
            calendar.Clear();
            calendar.Fill(string.Format("where extract(year from {0}) = {1} and extract(month from {0}) = {2}",
                CALENDAR_seq.ColumnsName.CALENDAR_DAY, nudYear.Value, cbMonth.SelectedIndex + 1));
            FillCalendar(calendar);
            FillWorkingTime();
        }

        /// <summary>
        /// Событие изменения индекса месяца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            calendar.Clear();
            calendar.Fill(string.Format("where extract(year from {0}) = {1} and extract(month from {0}) = {2}",
                CALENDAR_seq.ColumnsName.CALENDAR_DAY, nudYear.Value, cbMonth.SelectedIndex + 1));
            FillCalendar(calendar);
        }

        /// <summary>
        /// Рабочий день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmWorkDay_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
                menuControl.BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// Сокращенный день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmShortDay_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
                menuControl.BackColor = Color.GreenYellow;
        }

        /// <summary>
        /// Праздничный день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmHoliday_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
                menuControl.BackColor = Color.Red;
        }

        /// <summary>
        /// Обычный выходной день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCommomHol_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
            {
                menuControl.BackColor = Color.LightCoral;
                listTransSat.Remove(DateTime.Parse(string.Format("{0}.{1}.{2}",
                    menuControl.Text, (cbMonth.SelectedIndex + 1).ToString(), nudYear.Value.ToString())));
                listTransSun.Remove(DateTime.Parse(string.Format("{0}.{1}.{2}",
                    menuControl.Text, (cbMonth.SelectedIndex + 1).ToString(), nudYear.Value.ToString())));
            }
        }

        /// <summary>
        /// Событие нажатия кнопки сохрания данных календаря в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveCalendar_Click(object sender, EventArgs e)
        {
            try
            {
                int year = (int)nudYear.Value;
                int month = cbMonth.SelectedIndex + 1;
                CALENDAR_obj r_calendar;
                /// Если данные за указанный период отсутствуют, то добавляются новые данные.
                /// Если данные уже были сохранены ранее, то они редактируются.
                if (calendar.Count == 0)
                {
                    foreach (Control control in pnKalendar.Controls)
                    {
                        if (control is TextBox && control.Text != "")
                        {
                            int type_day = 0;
                            if (control.BackColor == Color.LightCoral || control.BackColor == Color.Firebrick ||
                                control.BackColor == Color.Yellow)
                            {
                                type_day = 1;
                            }
                            else
                                if (control.BackColor == Color.WhiteSmoke)
                                {
                                    type_day = 2;
                                }
                                else
                                    if (control.BackColor == Color.GreenYellow)
                                    {
                                        type_day = 3;
                                    }
                                    else
                                    {
                                        type_day = 4;
                                    }
                            r_calendar = calendar.AddNew();
                            r_calendar.CALENDAR_DAY =
                                new DateTime(year, month, Convert.ToInt32(control.Text.Trim()));
                            r_calendar.TYPE_DAY_ID = type_day;
                        }
                    }
                    foreach (CALENDAR_obj calend in calendar)
                    {
                        calend.DAY_TRANS = null;
                    }
                    foreach (DateTime date in listTransSat)
                    {
                        calendar.Where(k => k.CALENDAR_DAY.Value.ToShortDateString() ==
                            date.ToShortDateString()).FirstOrDefault().DAY_TRANS = 1;
                    }
                    foreach (DateTime date in listTransSun)
                    {
                        calendar.Where(k => k.CALENDAR_DAY.Value.ToShortDateString() ==
                            date.ToShortDateString()).FirstOrDefault().DAY_TRANS = 2;
                    }
                    listTransSat.Clear();
                    listTransSun.Clear();
                }
                else
                {
                    foreach (Control control in pnKalendar.Controls)
                    {
                        if (control is TextBox && control.Text != "")
                        {
                            int type_day = 0;
                            if (control.BackColor == Color.LightCoral || control.BackColor == Color.Firebrick ||
                                control.BackColor == Color.Yellow)
                            {
                                type_day = 1;
                            }
                            else
                                if (control.BackColor == Color.WhiteSmoke)
                                {
                                    type_day = 2;
                                }
                                else
                                    if (control.BackColor == Color.GreenYellow)
                                    {
                                        type_day = 3;
                                    }
                                    else
                                    {
                                        type_day = 4;
                                    }
                            calendar.Where(k => k.CALENDAR_DAY.Value.Year == year &&
                                k.CALENDAR_DAY.Value.Month == month &&
                                k.CALENDAR_DAY.Value.Day ==
                                Convert.ToInt32(control.Text.Trim())).FirstOrDefault().TYPE_DAY_ID = type_day;
                        }
                    }
                    foreach (CALENDAR_obj calend in calendar)
                    {
                        calend.DAY_TRANS = null;
                    }
                    foreach (DateTime date in listTransSat)
                    {
                        calendar.Where(k => k.CALENDAR_DAY.Value.ToShortDateString() ==
                            date.ToShortDateString()).FirstOrDefault().DAY_TRANS = 1;
                    }
                    foreach (DateTime date in listTransSun)
                    {
                        calendar.Where(k => k.CALENDAR_DAY.Value.ToShortDateString() ==
                            date.ToShortDateString()).FirstOrDefault().DAY_TRANS = 2;
                    }
                    listTransSat.Clear();
                    listTransSun.Clear();
                }
                calendar.Save();
                Connect.Commit();
                UpdateHolidayInPerco(year);
                FillCalendar(calendar);
                FillWorkingTime();
            }
            catch
            { }
        }

        void UpdateHolidayInPerco(decimal year)
        {
            OracleDataAdapter _daHoliday = new OracleDataAdapter(string.Format(Queries.GetQuery("Table/SelectHolidaysForPerco.sql"),
                Connect.Schema), Connect.CurConnect);
            _daHoliday.SelectCommand.BindByName = true;
            _daHoliday.SelectCommand.Parameters.Add("p_YEAR", OracleDbType.Decimal).Value = year;
            DataTable _dt = new DataTable();
            _daHoliday.Fill(_dt);
            if (_dt.Rows.Count > 0)
            {
                List<PercoXML.Holiday> _holidays = new List<PercoXML.Holiday>();
                foreach (DataRow row in _dt.Rows)
                {
                    _holidays.Add(new PercoXML.Holiday(row["DISPLAYNAME"].ToString(), "", 
                            (row["CALENDAR_DAY"] != DBNull.Value ? Convert.ToDateTime(row["CALENDAR_DAY"]).ToShortDateString() : ""),
                            row["TYPE_HOLIDAY"].ToString(), row["PREF_TYPE_HOLIDAY"].ToString(), 
                            (row["SAT_SAN_DAY_ISWORK"] != DBNull.Value ? Convert.ToDateTime(row["SAT_SAN_DAY_ISWORK"]).ToShortDateString() : "")));
                }
                Authorization.holidays.InsertHolidays(new PercoXML.Holidays(year.ToString(), _holidays));
            }
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }               

        /// <summary>
        /// Выходной день за счет субботы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSatHol_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
            {
                menuControl.BackColor = Color.Yellow;
                listTransSat.Add(DateTime.Parse(string.Format("{0}.{1}.{2}",
                    menuControl.Text, (cbMonth.SelectedIndex+1).ToString(), nudYear.Value.ToString())));
            }
        }

        /// <summary>
        /// Выходной день за счет воскресенья
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSunHol_Click(object sender, EventArgs e)
        {
            menuControl = cmCalendar.SourceControl;
            if (menuControl.Text != "")
            {
                menuControl.BackColor = Color.Firebrick;
                listTransSun.Add(DateTime.Parse(string.Format("{0}.{1}.{2}",
                    menuControl.Text, (cbMonth.SelectedIndex + 1).ToString(), nudYear.Value.ToString())));
            }
        }

        private void Calendar_FormClosing(object sender, FormClosingEventArgs e)
        {
            //((FormMain)ParentForm).applicationMenu1.Items.Remove((Elegant.Ui.Button)this.Tag);
        }
    }
}
