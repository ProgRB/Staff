using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace WpfControlLibrary
{
    public class IsEnabledEdit_MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(values[0]) || System.Convert.ToBoolean(values[1]))
                return true;
            else
                return false;
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsEnabledColor_ValueConvert : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value) == true)
                return new SolidColorBrush(Colors.White);
            else
                return new SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 252, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolInvert_ValueConvert : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            return !System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsChechedSCP_ValueConvert : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value) == true)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisiblePer_Num_ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt16(value) == 1)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Tariff_RateMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Установлена дата приема/перевода
            if (values[0] != DependencyProperty.UnsetValue)
            {
                // Установлена тарифная сетка и разряд
                if (values[2] != DependencyProperty.UnsetValue && values[3] != DependencyProperty.UnsetValue)
                {
                    //Tariff_Grid_Salary.UpdateSet(System.Convert.ToDateTime(values[0]));
                    Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.RowFilter =
                        string.Format("TARIFF_GRID_ID = {0} and TAR_CLASSIF = {1} and (TARIFF_END_DATE >= #{2}# And TAR_DATE <= #{2}# ) ", values[2], values[3],
                        System.Convert.ToDateTime(values[0]).ToString("MM/dd/yyyy"));
                    if (Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.Count == 1)
                    {
                        return Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView[0]["TAR_SAL"];
                    }
                }
            }
            return values[1];
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SalaryMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime _dateTransfer = DateTime.Today;
            // Установлена дата приема/перевода
            if (values[0] != DependencyProperty.UnsetValue && values[0] != null && values[0] != DBNull.Value)
            {
                _dateTransfer = System.Convert.ToDateTime(values[0]);
            }
            // Установлена тарифная сетка и разряд
            if (values[2] != DependencyProperty.UnsetValue && values[3] != DependencyProperty.UnsetValue)
            {
                //Tariff_Grid_Salary.UpdateSet(_dateTransfer);
                Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.RowFilter =
                    string.Format("TARIFF_GRID_ID = {0} and TAR_CLASSIF = {1} and (TARIFF_END_DATE >= #{2}# And TAR_DATE <= #{2}# )", values[2], values[3],
                    _dateTransfer.ToString("MM/dd/yyyy"));
                if (Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.Count > 0 &&
                    values[1] != DependencyProperty.UnsetValue && values[1] != null && values[1] != DBNull.Value)
                //if (Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.Count == 1)
                {
                    return Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.ToTable().Select().
                        Where(k => System.Convert.ToDecimal(k["TAR_SAL"]) == System.Convert.ToDecimal(values[1])).Select(j => j["TAR_MONTH"]).FirstOrDefault();
                    //return Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView[0]["TAR_MONTH"];
                }
            }
            else
            {
                if (values[1] != DependencyProperty.UnsetValue && values[1].ToString() != "")
                {
                    Base_Tariff.DTBase_Tariff.DefaultView.RowFilter =
                        string.Format("EDATE >= #{0}# And BDATE <= #{0}#", 
                        _dateTransfer.ToString("MM/dd/yyyy"));
                    decimal _tariff = 0;
                    if (Base_Tariff.DTBase_Tariff.DefaultView.Count > 0)
                        _tariff = System.Convert.ToDecimal(Base_Tariff.DTBase_Tariff.DefaultView[0]["TARIFF"]);
                    return Math.Round(_tariff * Decimal.Parse(values[1].ToString()), 0, MidpointRounding.AwayFromZero);
                }
            }
            return 0;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AdditionMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime _dateTransfer = DateTime.Today;
            // Установлена дата приема/перевода
            if (values[0] != DependencyProperty.UnsetValue && values[0] != null && values[0] != DBNull.Value)
            {
                _dateTransfer = System.Convert.ToDateTime(values[0]);
            }
            // Установлена дата приема/перевода
            if (values[1] != DependencyProperty.UnsetValue)
            {
                Base_Tariff.DTBase_Tariff.DefaultView.RowFilter =
                        string.Format("EDATE >= #{0}# And BDATE <= #{0}#",
                        _dateTransfer.ToString("MM/dd/yyyy"));
                decimal _tariff = 0;
                if (Base_Tariff.DTBase_Tariff.DefaultView.Count > 0)
                    _tariff = System.Convert.ToDecimal(Base_Tariff.DTBase_Tariff.DefaultView[0]["TARIFF"]);
                return Math.Round(_tariff * Decimal.Parse(values[1].ToString()),0);
            }
            return 0;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != DependencyProperty.UnsetValue && value != DBNull.Value && System.Convert.ToInt16(value) == 1)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ComboBoxSalaryVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != DependencyProperty.UnsetValue && value != DBNull.Value && System.Convert.ToInt16(value) > 1)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MainConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value))
            {
                return new SolidColorBrush(Color.FromRgb(255, 225, 200));
            }
            return new SolidColorBrush(Colors.White);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class IsEnabledInputSalary_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return false;
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class IsEnabledInputSalary_MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue)
            {
                return false;
            }
            if (values[1] != DependencyProperty.UnsetValue && values[2] != DependencyProperty.UnsetValue)
            {                
                Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.RowFilter =
                    string.Format("TARIFF_GRID_ID = {0} and TAR_CLASSIF = {1} and (TARIFF_END_DATE >= #{2}# And TAR_DATE <= #{2}# )", values[1], values[2],
                    DateTime.Today.ToString("MM/dd/yyyy"));
                if (Tariff_Grid_Salary.DTTariff_Grid_Salary.DefaultView.Count > 1)
                {
                    return false;
                }
            }
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class Project_ApprovalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue && value != DBNull.Value)
            {
                return ProjectDataSet.Tables["PROJECT_PLAN_APPROVAL"].DefaultView.FindRows(value).FirstOrDefault()["NOTE_ROLE_APPROVAL"];
            }
            return "Не установлено";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class ProjectColor_MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Если процесс согласования завершен, то проходим дальше
            if (values[0] != DependencyProperty.UnsetValue && System.Convert.ToBoolean(values[0]))
                // Если Номер приказа уже заполнен, то обозначаем приказ как уже готовый к проведению в основную базу
                if (values[1] != DependencyProperty.UnsetValue)
                    // Для Экономического управления отображаем проекты, которые еще не зарегистрированы в Штатном расписании особым цветом
                    if (values[2] != DependencyProperty.UnsetValue && System.Convert.ToBoolean(values[2]))
                        return new SolidColorBrush(Color.FromRgb(255, 165, 165));
                    else
                        return new SolidColorBrush(Colors.GreenYellow);
                else
                    return new SolidColorBrush(Colors.Yellow);            
            return new SolidColorBrush(Colors.White);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Project_StatementColor_MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Если процесс согласования завершен, то проходим дальше
            if (values[0] != DependencyProperty.UnsetValue && System.Convert.ToBoolean(values[0]))
                // Если уже установлен признак Печати листа согласования, то цвет зеленый, иначе желтенький))
                if (values[1] != DependencyProperty.UnsetValue && System.Convert.ToBoolean(values[1]))
                    return new SolidColorBrush(Colors.GreenYellow);
                else
                    return new SolidColorBrush(Colors.Yellow);       
            return new SolidColorBrush(Colors.White);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Transfer_Id_From_PositionColor_ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если процесс согласования завершен, то проходим дальше
            if (value != DependencyProperty.UnsetValue && System.Convert.ToBoolean(value))
                // Выделяем текущий родительский перевод для проекта цветом
                return new SolidColorBrush(Colors.Yellow);
            return new SolidColorBrush(Colors.White);
        }
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderTab_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != DependencyProperty.UnsetValue)
            {
                if (System.Convert.ToInt16(value) == 1)
                    return "Данные о приеме";
                return "Данные о переводе";
            }
            return "Данные о - Не установлено";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class GroupNoteRoleApproval_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GroupNoteRoleApproval)value).Name_Group;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class IsEnabledAllChkBxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null)
                return false;
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class View_OnlyVisibility_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != DependencyProperty.UnsetValue)
            { 
                return System.Convert.ToBoolean(value) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public class Table_Closing_Color_ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если процесс согласования завершен, то проходим дальше
            if (value != DependencyProperty.UnsetValue && value != DBNull.Value && value != null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString()));
            return new SolidColorBrush(Colors.White);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
