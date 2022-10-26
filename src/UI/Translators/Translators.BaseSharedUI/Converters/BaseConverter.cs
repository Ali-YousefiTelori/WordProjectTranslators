using System;
using System.Globalization;
#if (CSHTML5)
using Windows.UI.Xaml.Data;
#endif

namespace Translators.UI.Converters
{
    public abstract class BaseConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

#if (CSHTML5)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
