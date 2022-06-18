using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Translators.UI.Converters
{
    public class EvenColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
            {
                OSAppTheme currentTheme = Application.Current.RequestedTheme;
                return currentTheme == OSAppTheme.Dark ? Color.FromHex("#2FFFFFFF") : Color.FromHex("#2F000000");
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
