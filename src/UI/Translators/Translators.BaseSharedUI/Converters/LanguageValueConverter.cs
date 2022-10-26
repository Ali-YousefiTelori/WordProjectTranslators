using System;
using Translators.Converters;
#if (CSHTML5)
using Windows.UI.Xaml.Data;
using System.Globalization;
#endif

namespace Translators.UI.Converters
{
    public class LanguageValueConverter : LanguageValueBaseConverter, IValueConverter
    {
#if (CSHTML5)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return base.Convert(value, targetType, parameter, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
