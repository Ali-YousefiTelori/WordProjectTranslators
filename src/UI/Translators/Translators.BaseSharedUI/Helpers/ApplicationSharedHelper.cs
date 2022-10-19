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

        public static Color GetColorFromHex(string hex)
        {
#if (!WPF)
{
#if (Xamarin)
            return Color.FromHex(hex);
#else
            return Color.FromArgb(hex);
#endif
}
#else
            return (Color)ColorConverter.ConvertFromString(hex);
#endif
        }

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
