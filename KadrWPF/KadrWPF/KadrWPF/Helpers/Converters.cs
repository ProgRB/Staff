using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using Salary.Helpers;
using System.ComponentModel;
using System.Collections;
using WpfControlLibrary;
using System.Globalization;
using System.Diagnostics;

namespace KadrWPF.Helpers
{

    public class ArithmConverters : IMultiValueConverter
    {
        public string Operator
        {
            get;
            set;
        }

        /// <summary>
        /// Конвертер чисел в их произведение.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Thickness))
            {
                if (values.All(r => r != null && r != System.Windows.DependencyProperty.UnsetValue))
                {
                    Decimal? v = 1;
                    if (parameter != null)
                        v = decimal.Parse(parameter.ToString());
                    for (int i = 0; i < values.Length; ++i)
                    {
                        v *= decimal.Parse(values[i].ToString());
                    }
                    return new Thickness(System.Convert.ToDouble(v), 0, 0, 0);
                }
                else
                    return new Thickness(0, 0, 0, 0);
            }
            else
                if (NumericHelper.IsNumericType(targetType))
            {
                if (values.All(r => r != null && r != System.Windows.DependencyProperty.UnsetValue))
                {
                    Decimal? v = 1;
                    if (parameter != null)
                        v = decimal.Parse(parameter.ToString());
                    for (int i = 0; i < values.Length; ++i)
                    {
                        v *= decimal.Parse(values[i].ToString());
                    }
                    return v;
                }
                else
                    return 0m;
            }
            else
                return 0m;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Вспомогательный класс проверки на число
    /// </summary>
    public static class NumericHelper
    {
        public static bool IsNumeric(this object t)
        {
            switch (Type.GetTypeCode(t.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumericType(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

    public class MultiSumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!Array.TrueForAll(values, r => r != null && r != DependencyProperty.UnsetValue))
                return GridLength.Auto;
            double d = values.Sum(r => double.IsNaN((double)r) ? 0 : (double)r) + double.Parse((parameter == null ? "0" : parameter.ToString()), System.Globalization.CultureInfo.InvariantCulture);
            return new GridLength(d);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DivideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double c = double.Parse(parameter.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            return ((double)value) * c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MinusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double c = double.Parse(parameter.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            return ((double)value) - c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

    public class SourceToNameConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!Array.TrueForAll(values, t => t != null && t != DependencyProperty.UnsetValue))
                return null;
            else
            {
                string propertyName = (string)parameter;
                CollectionViewSource c = values[0] as CollectionViewSource;
                DataView list = c.View.SourceCollection as DataView;
                DataRow rr = list.Table.Rows.Find(values[1]);
                if (rr != null)
                    return rr[propertyName];
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FindFieldConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!Array.TrueForAll(values, t => t != null && t != DependencyProperty.UnsetValue))
                return null;
            else
            {
                string propertyName = (string)parameter;
                IEnumerable l1 = values[0] as IEnumerable;
                IEnumerable<object> l = l1.OfType<object>();
                if (l.Count() > 0)
                {
                    PropertyDescriptor td = TypeDescriptor.GetProperties(l.ElementAt(0))[propertyName];
                    var t = from p in l
                            where td.GetValue(p).Equals(values[1])
                            select p;
                    return t.FirstOrDefault();
                }
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class GetErrorElementConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return ValidationHelper.GetError(value as FrameworkElement);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == DBNull.Value || value == null || System.Convert.ToDecimal(value) == 0)
                return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? d = (bool?)value;
            if (d.HasValue && d.Value)
                return 1m;
            else return 0m;
        }
    }
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool?)value == false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool?)value == false);
        }
    }
    public class InvertVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility?)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility?)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }
    }
    public class BoolToGridDetailsVisiblConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool?)value == false ? DataGridRowDetailsVisibilityMode.Collapsed : DataGridRowDetailsVisibilityMode.VisibleWhenSelected);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DataGridRowDetailsVisibilityMode)value == DataGridRowDetailsVisibilityMode.Visible || (DataGridRowDetailsVisibilityMode)value == DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
    }
    public class DecimalToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value == null || (decimal)value == 0 ? DataGridRowDetailsVisibilityMode.Collapsed : DataGridRowDetailsVisibilityMode.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((DataGridRowDetailsVisibilityMode)value == DataGridRowDetailsVisibilityMode.Visible || (DataGridRowDetailsVisibilityMode)value == DataGridRowDetailsVisibilityMode.VisibleWhenSelected ? 1 : 0);
        }
    }

    public class ChildConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return (value as DataRowView).CreateChildView(parameter.ToString());
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsNullConverter : IValueConverter 
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool fl = value == null || value == DBNull.Value;
            return fl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Самый полезный конвертер - суммирует поля коллекции по имени свойств через запятую, и возвращает массив в порядке перечисления суммируемых свойств
    /// </summary>
    public class CollectionsToSumsConverter : IMultiValueConverter
    {
        private string _sumField;
        /// <summary>
        /// Свойство суммирования по полям. Можно указать несколько полей через пробел
        /// </summary>
        public string SumField
        {
            get
            {
                return _sumField;
            }
            set
            {
                _sumField = value;
                SummarizedFields = value.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
        /// <summary>
        /// Поля для суммирования значений
        /// </summary>
        private List<string> SummarizedFields
        {
            get;
            set;
        }

        /// <summary>
        /// Суммирование полей процедура
        /// </summary>
        /// <param name="value"></param>
        /// <returns>возвращает массив данных</returns>
        private decimal[] GetSum(object value)
        {
            decimal[] result = new decimal[SummarizedFields.Count];
            if (value != null)
            {
                if (value is IEnumerable) // если это множество, то от каждого элемента пытаемся получить данные, иначе пытаемся у элемента данные получить
                {
                    foreach (object v in value as IEnumerable)
                    {
                        result = result.Zip(GetSum(v), (x, y) => x + y).ToArray();
                    }
                }
                else
                {
                    if (value is CollectionViewGroup)
                        result = GetSum((value as CollectionViewGroup).Items);
                    else
                    {
                        PropertyDescriptorCollection pd = TypeDescriptor.GetProperties(value); // получаем все свойства у объекта
                        for (int i = 0; i < SummarizedFields.Count; ++i)
                        {
                            string prop = SummarizedFields[i];
                            if (pd[prop] != null) // если есть такое свойство у объекта, то суммируем его
                            {
                                object temp = pd[prop].GetValue(value);
                                result[i] += temp == null||temp==DBNull.Value ? 0 : (decimal)temp; // если значение не Null то суммируем его
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Конвертируем коллекцию в массив сумм выходных значений. Если поле суммироемое одно, то возвращаем 1 число, иначе массив чисел
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal[] k = new decimal[SummarizedFields.Count];
            foreach (object v in values)
            {
                k = k.Zip(GetSum(v), (x, y) => x + y).ToArray();
            }
            if (SummarizedFields.Count == 1)
                return k[0];
            else
                return k;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Конвертер значения 
    /// </summary>
    public class ValueToIDConverter : IValueConverter
    {
        public object ValueCollection
        {
            get; set;
        }

        /// <summary>
        /// Имя ключевого поля
        /// </summary>
        public string KeyField
        {
            get; set;
        }

        /// <summary>
        /// Поле наименования значения
        /// </summary>
        public string ValueField
        {
            get; set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ValueCollection != null)
            {
                Type t = ValueCollection.GetType();
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    IDictionary v = ValueCollection as IDictionary; //если это словарь то пытаемся вернуть значение
                    try
                    {
                        return v[value];
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return null;
                    }
                }
                else
                    if (ValueCollection is IEnumerable)
                {
                    IEnumerable v = ValueCollection as IEnumerable;
                    if (v != null)
                    {
                        PropertyDescriptorCollection pd = null;
                        foreach (object p in v)
                        {
                            if (pd == null)
                            {
                                pd = TypeDescriptor.GetProperties(p); // получаем все свойства у объекта
                                if (pd[KeyField] == null || pd[ValueField] == null)
                                {
                                    throw new Exception($"Не найдены поля для конвертации значения {KeyField} в {ValueField}");
                                }
                            }

                            if (value.Equals(pd[KeyField].GetValue(p))) // если есть такое свойство у объекта, то сравниваем ег
                            {
                                return pd[ValueField].GetValue(p);
                            }
                        }
                        return null;
                    }
                }
                throw new Exception("Объект поиска значения не является коллекцией или словарем");
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ValueCollection != null)
            {
                Type t = ValueCollection.GetType();
                PropertyDescriptorCollection pd = null;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    IDictionary v = ValueCollection as IDictionary; //если это словарь то пытаемся вернуть значение

                    foreach (var p in v)
                    {
                        if (pd == null)
                        {
                            pd = TypeDescriptor.GetProperties(p); // получаем все свойства у объекта
                        }

                        if (string.IsNullOrEmpty(ValueField) || pd[ValueField] == null) // если поле для поиска значения не установление или не существует то берем само значение в словаре и сравниваем
                        {
                            if (value.Equals(pd["Value"].GetValue(p)))
                                return pd["Key"].GetValue(p);
                        }
                        else
                        {
                            if (value.Equals(pd[ValueField].GetValue(p)))
                                return pd["Key"].GetValue(p);
                        }
                        
                    }
                }
                else
                if (ValueCollection is IEnumerable)
                {
                    IEnumerable v = ValueCollection as IEnumerable;
                    if (v != null)
                    {
                        foreach (object p in v)
                        {
                            if (pd == null)
                            {
                                pd = TypeDescriptor.GetProperties(p); // получаем все свойства у объекта
                                if (pd[KeyField] == null || pd[ValueField] == null)
                                {
                                    throw new Exception($"Не найдены поля для конвертации значения {ValueField} в {KeyField}");
                                }
                            }
                             // если есть такое свойство у объекта, то сравниваем его
                            if (value.Equals(pd[ValueField].GetValue(p)))
                            {
                                return pd[KeyField].GetValue(p);
                            }
                        }
                        return null;
                    }
                }
                throw new Exception("Объект поиска значения не является коллекцией или словарем");
            }
            else
                return null;
        }
    }
}

namespace Salary.View
{
    public class CollectionToSumConverter : IValueConverter
    {
        public string SumField
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return GetSum(value);
        }

        private decimal? GetSum(object value)
        {
            if (value != null)
            {
                IList<Object> dv = null;
                IList<Object> vg = value as IList<Object>;
                if (vg != null)
                    dv = vg;
                else
                {
                    Xceed.Wpf.DataGrid.DataGridCollectionView v1 = value as Xceed.Wpf.DataGrid.DataGridCollectionView;
                    if (v1 != null)
                        dv = (v1.SourceCollection as DataView).OfType<Object>().ToList();
                    else
                    {
                        CollectionViewSource v = value as CollectionViewSource;
                        if (v != null)
                        {
                            dv = (v.Source as DataView).OfType<Object>().ToList();
                        }
                        else
                        {
                            DataView vv = value as DataView;
                            if (vv != null)
                                dv = vv.OfType<Object>().ToList();
                        }
                    }
                }
                if (dv != null)
                {
                    if (dv.Count>0)
                    {
                        Type tp = dv[0].GetType();
                        if (tp == typeof(DataRowView))
                            return dv.OfType<Object>().Sum(t => (t as DataRowView).Row.Field2<Decimal?>(SumField));
                        else
                            return dv.OfType<Object>().Sum(t => GetSum((t as CollectionViewGroup).Items));
                    }
                    else return 0;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EqualConverter : IMultiValueConverter
    {
        public object DefaulValue
        {
            get;
            set;
        }
        public object FalseValue
        {
            get;
            set;
        }
        public object TrueValue
        {
            get;
            set;
        }
        public Type CompareType
        {
            get;
            set;
        }
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length > 1)
            {
                if (Array.TrueForAll(values, t=>t==DependencyProperty.UnsetValue || t==null))
                    return DefaulValue;
                if (values[0].Equals(values[1]))
                    return TrueValue;
                else
                    return FalseValue;
            }
            else return DefaulValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

namespace Reporting.Converters
{
    public class ExpandStateSaver : IMultiValueConverter
    {
        public static Dictionary<string, bool> states_exp = new Dictionary<string, bool>();
        public ExpandStateSaver()
        { }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != null && values[1] != null)
            {
                string st = values[1].ToString();
                if (states_exp.ContainsKey(st))
                    return states_exp[st];
                else return false;
            }
            else return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { true, null };
        }
    }
}