using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;

namespace WpfControlLibrary
{
    public class ValidationNullRule : ValidationRule
    {
        public ValidationNullRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value == DBNull.Value || value == DependencyProperty.UnsetValue || String.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Значение не может быть пустым!");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }


    public class MonthRangeRule : ValidationRule
    {
        private int _min;
        private int _max;

        public MonthRangeRule()
        {
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int age = 0;

            try
            {
                if (((string)value).Length > 0)
                    age = Int32.Parse((String)value);
            }
            catch 
            {
                return new ValidationResult(false, "Неверный символ!!! Необходимо ввести число!");
            }

            if ((age < Min) || (age > Max))
            {
                return new ValidationResult(false,
                  "Допустимый промежуток: " + Min + " - " + Max + ".");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
