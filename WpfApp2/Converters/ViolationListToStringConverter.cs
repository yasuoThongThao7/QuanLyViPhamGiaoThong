using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfApp2.Converters
{
    public class ViolationListToStringConverter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null)
                return string.Empty;

            return ConvertListToString(values[0]);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertListToString(value);
        }

        private string ConvertListToString(object value)
        {
            if (value is IEnumerable list)
            {
                var items = list.Cast<object>()
                    .Select(item => item?.ToString())
                    .Where(s => !string.IsNullOrWhiteSpace(s));
                return string.Join(Environment.NewLine, items);
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
