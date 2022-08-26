using System;
using System.Globalization;
using Xamarin.Forms;

namespace Translators.UI.Converters
{
    public class EvenColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if (value is bool b && b)
            {
                if (currentTheme == OSAppTheme.Dark)
                {
                    return Color.FromHex("#212121");
                }
                else
                {
                    return Color.FromHex("#C4C4C4");
                }
            }
            else
            {
                if (currentTheme == OSAppTheme.Dark)
                {
                    if (Application.Current.Resources.TryGetValue("DarkBackgroundColor", out object color) && color is Color darkBackgroundColor)
                    {
                        return darkBackgroundColor;
                    }
                }
                else
                {
                    if (Application.Current.Resources.TryGetValue("LightBackgroundColor", out object color) && color is Color darkBackgroundColor)
                    {
                        return darkBackgroundColor;
                    }
                }
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
