using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp2.Converters
{
    public class BoolToPaidStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPaid)
                return isPaid ? "Đã nộp phạt" : "Chưa nộp phạt";
            return "Chưa nộp phạt";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
          
            throw new NotImplementedException();
        }
    }
}
