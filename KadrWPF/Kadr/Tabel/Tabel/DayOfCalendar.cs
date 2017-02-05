using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tabel
{
    class DayOfCalendar
    {
        private string _day;
        private Color _colorDay;
        private int _number;
        public DayOfCalendar(int number, string day, Color colorDay)
        {
            _number = number;
            _day = day;
            _colorDay = colorDay;
        }
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }
        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }
        public Color ColorDay
        {
            get { return _colorDay; }
            set { _colorDay = value; }
        }
    }
}
