using System.Globalization;
using Translators.UI.Helpers;

namespace Translators.UI.Converters
{
    public class EvenColorConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isDarkTheme = ApplicationSharedHelper.IsDarkTheme();
            if (value is bool b && b)
            {
                if (isDarkTheme)
                {
                    return ApplicationSharedHelper.GetColorFromHex("#212121");
                }
                else
                {
                    return ApplicationSharedHelper.GetColorFromHex("#C4C4C4");
                }
            }
            else
            {
                if (isDarkTheme)
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
            return ApplicationSharedHelper.GetTransparentColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
