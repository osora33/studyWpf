using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfBikeShop
{
    public class TwiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return int.Parse(value.ToString()) * 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}
