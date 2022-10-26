using System;
using System.Globalization;
using Translators.UI.Helpers;

#if (CSHTML5)
using Windows.UI.Xaml;
using Windows.UI;
#endif

namespace Translators.UI.Converters
{
    public class EvenColorConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
                    if (Application.Current.Resources.TryGetValue("DarkBackgroundColor", out object color))
                    {
                        if (color is Color darkBackgroundColor)
                            return darkBackgroundColor;
                        else if (color is Brush darkBackgroundBrush)
                            return darkBackgroundBrush;
                    }
                }
                else
                {
                    if (Application.Current.Resources.TryGetValue("LightBackgroundColor", out object color))
                    {
                        if (color is Color lightBackgroundColor)
                            return lightBackgroundColor;
                        else if (color is Brush lightBackgroundBrush)
                            return lightBackgroundBrush;
                    }
                }
            }
            return ApplicationSharedHelper.GetTransparentColor();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
