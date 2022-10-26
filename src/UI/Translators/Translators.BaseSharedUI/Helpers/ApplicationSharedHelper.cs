#if (CSHTML5)
using Windows.UI;
#endif
namespace Translators.UI.Helpers
{
    public class ApplicationSharedHelper
    {
        public static bool IsDarkTheme()
        {
#if (!WPF)
{
            var currentTheme = Application.Current.RequestedTheme;
#if (Xamarin)
            return currentTheme == OSAppTheme.Dark;
#else
            return currentTheme == AppTheme.Dark;
#endif
}
#else
            return false;
#endif
        }
        
#if (CSHTML5)
            
#elif (WPF)
        public static Brush GetColorFromHex(string hex)
        {
            return (Brush)new BrushConverter().ConvertFromString(hex);
        }
#else
        public static Color GetColorFromHex(string hex)
        {
#if (Xamarin)
            return Color.FromHex(hex);
#else
            return Color.FromArgb(hex);
#endif
        }
#endif

        public static Color GetTransparentColor()
        {
#if (Xamarin)
            return Color.Transparent;
#else
            return Colors.Transparent;
#endif
        }
    }
}
