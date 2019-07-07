using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using Mohsen;

namespace SampleProject
{
    /// <summary>
    /// Converts DateTime values to PersianDate values and vice versa, to be used in XAML
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(PersianDate))]
    public class DateTimeToPersianDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            DateTime date = (DateTime)value;
            return new PersianDate(date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            PersianDate pDate = (PersianDate)value;
            return pDate.ToDateTime();
        }
    }
}
