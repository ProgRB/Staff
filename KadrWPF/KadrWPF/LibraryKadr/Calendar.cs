using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;


namespace LibraryKadr
{
    public partial class Calendar : UserControl
    {
        public static string[] WeekDays = new string[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
        public static string[] Months = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public int NumberDayOfWeek(DateTime Day)
        {
            DayOfWeek n = Day.DayOfWeek;
            if (n == DayOfWeek.Friday) return 5;
            else
                if (n == DayOfWeek.Monday) return 1;
                else
                    if (n == DayOfWeek.Saturday) return 6;
                    else
                        if (n == DayOfWeek.Sunday) return 7;
                        else
                            if (n == DayOfWeek.Thursday) return 4;
                            else
                                if (n == DayOfWeek.Tuesday) return 2;
                                else
                                    return 3;
        }
        private DateTime _FirstVisibleMonth = DateTime.Now;
        public DateTime FirstVisibleMonth 
        {
            get { return _FirstVisibleMonth; }
            set
            {
                _FirstVisibleMonth = value;
                CurYear.Value = value.Year;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }

        private OracleConnection _connection = null;
        private string _Schema = "";

        public OracleConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                DrawCalendar();
            }
        }

        /********************** Дни отмеченные на календаре ********************/
        private HashSet<DateTime> _PastVS = new HashSet<DateTime>(), _PlanVS = new HashSet<DateTime>(), _CarryVS = new HashSet<DateTime>(), _CurrentVS= new HashSet<DateTime>();
        /// <summary>
        /// Прошедшие дни
        /// </summary>
        public System.Collections.Generic.HashSet<DateTime> Past
        {
            get {
                if (_PastVS == null) _PastVS = new System.Collections.Generic.HashSet<DateTime>();
                return _PastVS; }
            set
            {
                _PastVS = value;
                DrawCalendar();
            }
        }
        public System.Collections.Generic.HashSet<DateTime> Plan
        {
            get {
                if (_PlanVS == null) _PlanVS = new System.Collections.Generic.HashSet<DateTime>();
                return _PlanVS; }
            set
            {
                _PlanVS = value;
                DrawCalendar();
            }
        }
        public System.Collections.Generic.HashSet<DateTime> Carry
        {
            get {
                if (_CarryVS == null)
                    _CarryVS = new System.Collections.Generic.HashSet<DateTime>();
                return _CarryVS; }
            set
            {
                _CarryVS = value;
                DrawCalendar();
            }
        }
        public System.Collections.Generic.HashSet<DateTime> Current
        {
            get {
                if (_CurrentVS == null)
                    _CurrentVS = new System.Collections.Generic.HashSet<DateTime>();
                return _CurrentVS; }
            set
            {
                _CurrentVS = value;
                DrawCalendar();
            }
        }

        /******************************** Цвета дней на календаре *******************************/
        private Color _SelectionBorderColor = Color.Red,_ClBackColor=Color.White,_PlanBackColor=Color.Yellow,
            _NumberWeekColor= Color.Gray,
            _PastBackColor=Color.LightGreen,_CarryBackColor=Color.FromArgb(200, 210, 120);
        private int _DayDimension = 2,_MonthDimension = 4;
        public Color SelectionBorderColor
        {
            get { return _SelectionBorderColor; }
            set
            {
                _SelectionBorderColor = value;
                DrawCalendar();
            }
        }
        public Color BackColorCal
        {
            get { return _ClBackColor; }
            set
            {
                _ClBackColor = value;
                DrawCalendar();
            }
        }
        public Color PlanBackColor
        {
            get { return _PlanBackColor; }
            set
            {
                _PlanBackColor = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }
        public Color NumberWeekColor
        {
            get { return _NumberWeekColor; }
            set
            {
                _NumberWeekColor = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }
        public Color PastBackColor
        {
            get { return _PastBackColor; }
            set
            {
                _PastBackColor = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }
        public Color CarryBackColor
        {
            get { return _CarryBackColor; }
            set
            {
                _CarryBackColor = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }
        public int DayDimension
        {
            get { return _DayDimension; }
            set
            {
                _DayDimension = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }
        public int MonthDimension
        {
            get { return _MonthDimension; }
            set
            {
                _MonthDimension = value;
                DrawCalendar();
                canvas_Paint(null, null);
            }
        }

        private Image i1;

        public Calendar()
        {
            InitializeComponent();
            int _Width, _Height;
            _PastVS = new HashSet<DateTime>();
            _PlanVS = new HashSet<DateTime>();
            _CarryVS = new HashSet<DateTime>();
            _CurrentVS = new HashSet<DateTime>();
            OtherDays = new Dictionary<DateTime, Color>();
            HeaderFont = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            NumberWeekFont = new Font("Microsoft Sans Serif", 6);
            fnt = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            _Width = MonthDimension + 3 * (MonthDimension + NumberWeekFont.Height * 2 + 7 * (fnt.Height + DayDimension));
            _Height = 4 * (MonthDimension + 2 * HeaderFont.Height + 2 * DayDimension + 6 * (fnt.Height + DayDimension)) + MonthDimension;
            i1 = new Bitmap(_Width, _Height);
        }
        public Color HolidayColor = Color.Red, //праздничные дни
                WorkDay = Color.Black,
                DayOff = Color.FromArgb(240, 100, 100),//выходные дни
                ShortDayColor = Color.Green/*Color.FromArgb(200, 128, 128)*/,//сокращенные дни
            NonMonthDays = Color.Gray, 
            HeaderTextColor = Color.Black,
            HeaderBackColor = Color.FromArgb(191, 219, 255);
        public Font HeaderFont, NumberWeekFont, fnt;
        
        public Dictionary<DateTime, Color> OtherDays;

        public void DrawCalendar()
        {
            //читаем из базы календарь
            if (_connection != null && Staff.DataSourceScheme.SchemeName!=null)
            {
               string sh=Staff.DataSourceScheme.SchemeName;
               OracleDataTable table = new OracleDataTable(string.Format("select * from {0}.calendar where calendar_day between DATE '{1}-01-01' and DATE '{1}-12-31' order by CALENDAR_DAY", sh, CurYear.Value), _connection);
               table.Fill();
               OtherDays.Clear();
               for (int i = 0; i < table.Rows.Count; i++)
                   switch (table.Rows[i]["type_day_id"].ToString())
                   {
                       case "1": OtherDays.Add((DateTime)table.Rows[i]["calendar_day"], DayOff); break;
                       case "3": OtherDays.Add((DateTime)table.Rows[i]["calendar_day"], ShortDayColor); break;
                       case "4": OtherDays.Add((DateTime)table.Rows[i]["calendar_day"], HolidayColor); break;
                       default: OtherDays.Add((DateTime)table.Rows[i]["calendar_day"], WorkDay); break;
                   }
            }

            int _Width = MonthDimension + 3 * (MonthDimension + NumberWeekFont.Height * 2 + 7 * (fnt.Height + DayDimension));
            int _Height = 4 * (MonthDimension + 2 * HeaderFont.Height + 2 * DayDimension + 6 * (fnt.Height + DayDimension)) + MonthDimension;
            i1 = new Bitmap(_Width, _Height);

            PointF MonthCorner = new PointF(0, 0);
            Graphics MainGraphics = Graphics.FromImage(i1);
            MainGraphics.FillRectangle(new Pen(_ClBackColor).Brush, new Rectangle(0, 0, i1.Width, i1.Height));

            DateTime CurrentDate = new DateTime(2000, 1, 1);
            int CountDays = NumberDayOfWeek(new DateTime(FirstVisibleMonth.Year, 1, 1));
            for (int m = 0; m < 12; m++)
            {
                int N = NumberDayOfWeek(new DateTime(FirstVisibleMonth.Year, m + 1, 1)) - 1;//количество "пустых" дней перед началом месяца
                int m1 = m % 3;
                MonthCorner.X =  MonthDimension + m1 * (MonthDimension + NumberWeekFont.Height * 2 + 7 * (fnt.Height + DayDimension));
                int n1 = m / 3;
                MonthCorner.Y =  MonthDimension + n1 * (MonthDimension + 2 * HeaderFont.Height + 2 * DayDimension + 6 * (fnt.Height + DayDimension));//6 недель это максимум в месяце

                MainGraphics.DrawLines(new Pen(Color.SteelBlue, 1), new Point[] {//рисуем линии для месяцев
                    new Point((int)(MonthCorner.X+DayDimension+NumberWeekFont.Height*2+7*(DayDimension+fnt.Height)),
                        (int)(MonthCorner.Y+2*(HeaderFont.Height+DayDimension))),
                    new Point((int)(MonthCorner.X+NumberWeekFont.Height*2+DayDimension),
                        (int)(MonthCorner.Y+2*(HeaderFont.Height+DayDimension))),
                    new Point((int)(MonthCorner.X+NumberWeekFont.Height*2+DayDimension),
                        (int)(MonthCorner.Y+DayDimension+2*(HeaderFont.Height+DayDimension)+6*(DayDimension+fnt.Height)))
                        });
                //пишем названия дней недели:
                for (int i = 0; i < 5; i++)
                    MainGraphics.DrawString(WeekDays[i].Substring(0, 1), HeaderFont, new Pen(HeaderTextColor).Brush,
                        MonthCorner.X + 2 * NumberWeekFont.Height + DayDimension + i * (fnt.Height + DayDimension),
                        MonthCorner.Y + HeaderFont.Height + DayDimension);//1-5 дней
                for (int i = 5; i < 7; i++)
                    MainGraphics.DrawString(WeekDays[i].Substring(0, 1), HeaderFont, new Pen(Color.Red).Brush,
                        MonthCorner.X + 2 * NumberWeekFont.Height + DayDimension + i * (fnt.Height + DayDimension),
                        MonthCorner.Y + HeaderFont.Height + DayDimension);//6-7 день недели
                int DayInMon = DateTime.DaysInMonth(FirstVisibleMonth.Year, m + 1);//количество дней в месяце
                for (int i = N + 1; i <= DayInMon + N; i++)//заполняем месяц
                {
                    DateTime d = new DateTime(FirstVisibleMonth.Year, m + 1, i - N);
                    if (Plan.Contains(d))//проверяем есть ли он в списке выбранных на план(день)
                        MainGraphics.FillRectangle(new Pen(_PlanBackColor).Brush,
                            1 + MonthCorner.X + ((i - 1) % 7) * (DayDimension + fnt.Height) + DayDimension + NumberWeekFont.Height * 2,
                        MonthCorner.Y + 2 * (HeaderFont.Height + DayDimension) + (i - 1) / 7 * (DayDimension + fnt.Height),
                                        fnt.Height + DayDimension,
                                        fnt.Height);
                    else
                        if (Past.Contains(d))//или уже прошел то зеленым(по умолчанию)
                            MainGraphics.FillRectangle(new Pen(_PastBackColor).Brush,
                                1 + MonthCorner.X + ((i - 1) % 7) * (DayDimension + fnt.Height) + DayDimension + NumberWeekFont.Height * 2,
                            MonthCorner.Y + 2 * (HeaderFont.Height + DayDimension) + (i - 1) / 7 * (DayDimension + fnt.Height),
                                            fnt.Height + DayDimension,
                                            fnt.Height);
                        else
                            if (Carry.Contains(d))//или планируется(тогда желтым по умолчанию)
                                MainGraphics.FillRectangle(new Pen(_CarryBackColor).Brush,
                                    1 + MonthCorner.X + ((i - 1) % 7) * (DayDimension + fnt.Height) + DayDimension + NumberWeekFont.Height * 2,
                                MonthCorner.Y + 2 * (HeaderFont.Height + DayDimension) + (i - 1) / 7 * (DayDimension + fnt.Height),
                                                fnt.Height + DayDimension,
                                                fnt.Height);
                    //проверяем выбран ли он. то обводим в рамку

                    if (Current.Contains(d))
                        MainGraphics.DrawRectangle(new Pen(SelectionBorderColor),
                                    1 + MonthCorner.X + ((i - 1) % 7) * (DayDimension + fnt.Height) + DayDimension + NumberWeekFont.Height * 2,
                                MonthCorner.Y + 2 * (HeaderFont.Height + DayDimension) + (i - 1) / 7 * (DayDimension + fnt.Height),
                                                fnt.Height + DayDimension-1,
                                                fnt.Height);

                    MainGraphics.DrawString((i - N).ToString(), fnt, new Pen((OtherDays.ContainsKey(d) ? OtherDays[d] : WorkDay)).Brush,
                        new PointF(MonthCorner.X + ((i - 1) % 7) * (DayDimension + fnt.Height) + DayDimension + NumberWeekFont.Height * 2,
                            MonthCorner.Y + 2 * (HeaderFont.Height + DayDimension) + (i - 1) / 7 * (DayDimension + fnt.Height))
                        );


                    if ((i) % 7 == 0 || ((i) % 7 == 1 && i - N > 24))//пишем подписи номера недели
                    {
                        MainGraphics.DrawString(((CountDays - 1) / 7 + 1).ToString(), NumberWeekFont,
                            new Pen(NumberWeekColor).Brush,
                            new PointF(MonthCorner.X + NumberWeekFont.Height,
                                        MonthCorner.Y + 2 * HeaderFont.Height + 2*DayDimension + (i - 1) / 7 * (DayDimension + fnt.Height) + (fnt.Height - NumberWeekFont.Height))
                        );
                    }
                    ++CountDays;
                }
                MainGraphics.FillRectangle(new Pen(HeaderBackColor).Brush, MonthCorner.X, MonthCorner.Y,
                    NumberWeekFont.Height * 2 + DayDimension + 7 * (DayDimension + fnt.Height), HeaderFont.Height + 1);
                string cap = Months[m] + " " + FirstVisibleMonth.Year.ToString();
                MainGraphics.DrawString(cap, HeaderFont, new Pen(HeaderTextColor).Brush, new PointF((float)
                    (MonthCorner.X + (NumberWeekFont.Height * 2 + 7 * (DayDimension + 2 * fnt.Height) - (cap.Length + 1) * HeaderFont.Height) / 2.0),
                    MonthCorner.Y));
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            if (i1 != null)
            {
                Graphics g = canvas.CreateGraphics();
                g.FillRectangle(new Pen(_ClBackColor).Brush, 0, 0, canvas.Width, canvas.Height);
                g.DrawImage(i1,new RectangleF(-i1.Width/100*hScroll.Value,-i1.Height/100*vScroll.Value,i1.Width+(float)((double)(Mashtab.Value-50)/100.0*i1.Width), i1.Height+(float)((double)(Mashtab.Value-50)/100.0*i1.Height)));
            }
        }

        private void CurYear_ValueChanged(object sender, EventArgs e)
        {
            FirstVisibleMonth = new DateTime((int)CurYear.Value, 1, 1);
            canvas_Paint(null, null);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            canvas_Paint(null, null);
        }

        private void Mashtab_Scroll(object sender, EventArgs e)
        {
            labelMashtab.Text = "x" + ((float)(Mashtab.Value - 49) / 10.0).ToString();
            canvas_Paint(null, null);
        }

        private void Title_Enter(object sender, EventArgs e)
        {

        }
        
    }
}
