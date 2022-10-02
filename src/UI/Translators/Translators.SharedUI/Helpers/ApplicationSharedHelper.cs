namespace Translators.SharedUI.Helpers
{
    public class ApplicationSharedHelper
    {
        public static bool IsDarkTheme()
        {
            var currentTheme = Application.Current.RequestedTheme;
#if (Xamarin)
            return currentTheme == OSAppTheme.Dark;
#else
            return currentTheme == AppTheme.Dark;
#endif
        }

        public static Color GetColorFromHex(string hex)
        {
#if (Xamarin)
            return Color.FromHex(hex);
#else
            return Color.FromArgb(hex);
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
